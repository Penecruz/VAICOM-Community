-- TimeOverTarget.lua
-- Manage Time-over-Target for individual waypoints across flight plans.
--
-- Jester reads cockpit instruments (BDHI for distance, clock for time,
-- speed indicators for TAS/GS) to calculate required speeds.
--
-- Wheel path: Navigation > Time over Target > Primary Flight Plan   > WPT 1 > Set ToT
--                                                                           > Clear ToT
--                                           > Secondary Flight Plan > ...
--                                           > Start ToT Assist
--                                           > Stop ToT Assist

local Class             = require('base.Class')
local Behavior          = require('base.Behavior')
local Urge              = require('base.Urge')
local StressReaction    = require('base.StressReaction')
local Utilities         = require('base.Utilities')
local UpdateJesterWheel = require('behaviors.UpdateJesterWheel')
local SwitchTask        = require('tasks.common.SwitchTask')
local Task              = require('base.Task')

local TimeOverTarget = Class(Behavior)

TimeOverTarget.is_registered      = false
TimeOverTarget.prev_active_fp     = nil
TimeOverTarget.prev_active_wpt    = nil
TimeOverTarget.assist_active      = false
TimeOverTarget.assist_fp          = nil
TimeOverTarget.assist_wpt         = nil
TimeOverTarget.assist_required_gs = nil
TimeOverTarget.assist_remaining   = nil

-- ── Config ──────────────────────────────────────────────────────────────────
-- Based on available sound files
local MIN_FEASIBLE_GS = 240  -- kt
local MAX_FEASIBLE_GS = 600  -- kt

-- ── State ───────────────────────────────────────────────────────────────────

local tot = {}  -- tot[fp_no][wpt_no] = seconds (int) or nil

-- ── Helpers ─────────────────────────────────────────────────────────────────

local function get_tot(fp, wpt)
    return tot[fp] and tot[fp][wpt]
end

local function set_tot(fp, wpt, seconds)
    tot[fp] = tot[fp] or {}
    tot[fp][wpt] = seconds
end

local function parse_time(raw)
    local clean = raw:gsub("%s+", "")
    if #clean ~= 6 then return nil end
    local h, m, s = tonumber(clean:sub(1, 2)), tonumber(clean:sub(3, 4)), tonumber(clean:sub(5, 6))
    if not (h and m and s) then return nil end
    if h > 23 or m > 59 or s > 59 then return nil end
    return h * 3600 + m * 60 + s
end

local function fmt_time(secs)
    return string.format("%02d:%02d:%02d",
        math.floor(secs / 3600),
        math.floor(secs % 3600 / 60),
        secs % 60)
end

local function round10(n)
    return math.floor(n / 10 + 0.5) * 10
end

local function get_plan(fp_no)
    local memory = GetJester().memory
    return fp_no == 2 and memory:GetFlightPlan2() or memory:GetFlightPlan1()
end

-- ── Sensors ─────────────────────────────────────────────────────────────────

local function parse_property_value(raw)
    return tonumber(tostring(raw):match("([%d%.e%+%-]+)"))
end

local function read_property(device, prop_name)
    local prop = GetProperty(device, prop_name)
    if not prop or not prop:IsValid() then return nil end
    return parse_property_value(prop.value)
end

local function get_clock_seconds()
    local hours = read_property(
        '/WSO Cockpit/WSO Front Panel/Analog Clock/Analog Clock', 'Clock Time')
    return hours and hours * 3600
end

local function get_bdhi_distance()
    return read_property(
        '/Bearing Distance Heading Indicator/BDHI Meter', 'Distance Indication')
end

local function get_observation(key)
    local obs = jester.awareness:GetObservation(key)
    if not obs or not obs.value then return nil end
    local raw = obs.value
    if type(raw) ~= "number" then
        raw = parse_property_value(raw)
    end
    return raw
end

local function get_current_gs()  return get_observation("ground_speed")  end
local function get_current_tas() return get_observation("true_airspeed") end

-- Convert required GS to TAS using current wind offset (TAS - GS)
local function gs_to_tas(required_gs)
    local gs  = get_current_gs()
    local tas = get_current_tas()
    if not gs or not tas then return required_gs end
    return required_gs + (tas - gs)
end

-- ── ToT calculation ─────────────────────────────────────────────────────────

-- Returns required_gs, remaining, feasible — or nil
local function calc_required_gs(fp_no, wpt_no)
    local tot_secs = get_tot(fp_no, wpt_no)
    if not tot_secs then return nil end

    local clock = get_clock_seconds()
    if not clock then return nil end

    -- 12h clock — pick whichever AM/PM interpretation is in the future
    local rem_am = tot_secs - clock
    local rem_pm = tot_secs - (clock + 43200)
    local remaining
    if rem_am > 0 and rem_pm > 0 then
        remaining = math.min(rem_am, rem_pm)
    elseif rem_am > 0 then
        remaining = rem_am
    elseif rem_pm > 0 then
        remaining = rem_pm
    else
        return nil
    end

    -- BDHI only shows waypoint distance in NAV_COMP mode — switch if needed
    local cockpit = GetJester():GetCockpit()
    local bdhi_mode = cockpit:GetManipulator("BDHI Mode"):GetState()
    if bdhi_mode ~= "NAV_COMP" then
        GetJester():AddTask(SwitchTask:new("BDHI Mode", "NAV_COMP"))
        return nil
    end

    -- BDHI shows distance to the active waypoint only
    local memory = GetJester().memory
    if fp_no ~= memory:GetActiveFlightPlanNumber() or wpt_no ~= memory:GetActiveWaypointNumber() then
        return nil
    end

    local dist = get_bdhi_distance()
    if not dist then return nil end

    local required_gs = dist / (remaining / 3600)
    local feasible = required_gs >= MIN_FEASIBLE_GS and required_gs <= MAX_FEASIBLE_GS
    return required_gs, remaining, feasible
end

-- ── Voice helpers ───────────────────────────────────────────────────────────

local function say_number(task, n)
    if n < 20 then
        task:Say('Numbers/' .. Utilities.NumberToText(n))
    else
        local tens = math.floor(n / 10) * 10
        local ones = n % 10
        task:Say('Numbers/' .. Utilities.NumberToText(tens))
        if ones > 0 then
            task:Say('Numbers/' .. Utilities.NumberToText(ones))
        end
    end
end

-- ── Dynamic menu ────────────────────────────────────────────────────────────

local FP_LABELS = { [1] = "Primary Flight Plan", [2] = "Secondary Flight Plan" }
local MENU_PATH = { "Navigation", "Time over Target" }

local function build_wpt_items(fp_no, plan)
    local fp_str = tostring(fp_no)
    local items  = {}
    local wheel  = GetJester().behaviors[UpdateJesterWheel]

    for i, wpt in ipairs(plan.waypoints) do
        if i > Wheel.MAX_MENU_ITEMS then break end

        local waypoint_data = fp_str .. ";" .. tostring(i)
        local label = wheel:GetWaypointTextToDisplay(fp_no, i, wpt)
        local secs  = get_tot(fp_no, i)
        if secs then label = label .. " [" .. fmt_time(secs) .. "]" end

        items[#items + 1] = Wheel.Item:new({
            name = label,
            menu = Wheel.Menu:new({
                name = FP_LABELS[fp_no] .. ", WPT: " .. tostring(i),
                items = {
                    Wheel.Item:new({
                        name         = "Set ToT",
                        action       = "tot_set",
                        action_value = waypoint_data,
                        reaction     = Wheel.Reaction.NOTHING,
                        text_entry   = Wheel.TextEntry:new({
                            hint  = "HH MM SS",
                            max   = 8,
                            match = "[0-9 ]+",
                        }),
                    }),
                    Wheel.Item:new({
                        name         = "Clear ToT",
                        action       = "tot_clear",
                        action_value = waypoint_data,
                        reaction     = Wheel.Reaction.CLOSE_REMEMBER,
                    }),
                },
            }),
        })
    end
    return items
end

local function update_fp(fp_no)
    local plan = get_plan(fp_no)
    if not plan or not plan.waypoints or #plan.waypoints == 0 then return end

    Wheel.ReplaceItem(
        Wheel.Item:new({
            name = FP_LABELS[fp_no],
            menu = Wheel.Menu:new({
                name  = FP_LABELS[fp_no],
                items = build_wpt_items(fp_no, plan),
            }),
        }),
        FP_LABELS[fp_no],
        MENU_PATH
    )
end

local function update_flightplans()
    update_fp(1)
    update_fp(2)
end

-- ── Behavior ────────────────────────────────────────────────────────────────

function TimeOverTarget:Constructor()
    Behavior.Constructor(self)

    self.refresh_urge = Urge:new({
        time_to_release     = s(30),
        on_release_function = function() update_flightplans() end,
        stress_reaction     = StressReaction.ignorance,
    })

    local behavior = self
    self.monitor_urge = Urge:new({
        time_to_release     = s(30),
        on_release_function = function() behavior:MonitorToT() end,
        stress_reaction     = StressReaction.ignorance,
    })

    self.transition_pending = false
    self.transition_urge = Urge:new({
        time_to_release     = s(5),
        on_release_function = function() behavior:CheckTransition() end,
        stress_reaction     = StressReaction.ignorance,
    })
end

function TimeOverTarget:AskTotAssistance(required_gs, remaining, fp_no, wpt_no)
    self.assist_required_gs = required_gs
    self.assist_remaining   = remaining
    self.assist_fp          = fp_no
    self.assist_wpt         = wpt_no

    Dialog.Push(Dialog.Question:new({
        name    = "Jester",
        content = string.format("ToT reachable at %.0f kt. Want me to help you reach ToT?", gs_to_tas(required_gs)),
        label   = "Time over Target",
        timing  = Dialog.Timing:new({ question = s(10), action = s(15) }),
        options = {
            Dialog.Option:new({ response = "Yes", action = "tot_assist_yes" }),
            Dialog.Option:new({ response = "No",  action = "tot_assist_no" }),
        },
    }))
end

function TimeOverTarget:MonitorToT()
    if not self.assist_active then return end

    local required_gs, remaining, feasible = calc_required_gs(self.assist_fp, self.assist_wpt)
    if not required_gs or not feasible then
        self.assist_active = false
        Log("ToT: no longer reachable, stopping monitor")
        return
    end

    if remaining < 30 then
        self.assist_active = false
        Log("ToT: less than 30s remaining, stopping monitor")
        return
    end

    local current_tas = get_current_tas()
    if not current_tas then return end

    local required_tas = gs_to_tas(required_gs)
    local task = Task:new()
    if round10(current_tas) == round10(required_tas) then
        task:Say('refueling/LookingGood')
    else
        task:Say('awareness/' .. Utilities.NumberToText(round10(required_tas)))
    end
    GetJester():AddTask(task)

    -- Ramp up frequency under 3 minutes
    if remaining < 180 then
        self.monitor_urge:SetTimeToRelease(s(15))
    else
        self.monitor_urge:SetTimeToRelease(s(30))
    end
end

function TimeOverTarget:StartAssist(fp, wpt, required_gs, remaining)
    self.assist_active = true
    self.assist_fp     = fp
    self.assist_wpt    = wpt
    self.monitor_urge:SetTimeToRelease(s(30))
    self.monitor_urge:Restart()

    local task = Task:new()
    say_number(task, math.floor(remaining / 60))
    say_number(task, math.floor(remaining % 60))

    local required_tas = gs_to_tas(required_gs)
    task:Wait(s(2), { voice = true })
    task:Say('awareness/' .. Utilities.NumberToText(round10(required_tas)))

    GetJester():AddTask(task)
    Log(string.format("ToT assist: %d min %d sec, required TAS %.0f kt",
        math.floor(remaining / 60), math.floor(remaining % 60), required_tas))
end

function TimeOverTarget:StopAssist()
    self.assist_active = false
    self.assist_fp     = nil
    self.assist_wpt    = nil
end

function TimeOverTarget:CheckTransition()
    self.transition_pending = false
    local memory = GetJester().memory
    local fp  = memory:GetActiveFlightPlanNumber()
    local wpt = memory:GetActiveWaypointNumber()

    if get_tot(fp, wpt) then
        local required_gs, remaining, feasible = calc_required_gs(fp, wpt)
        if feasible then
            self:AskTotAssistance(required_gs, remaining, fp, wpt)
        end
    elseif self.assist_active then
        self:StopAssist()
    end
end

function TimeOverTarget:Register()
    ListenTo("tot_set", "ToT_Set", function(task, value)
        local fp_str, wpt_str, time_str = string.match(value, "(%d+);(%d+);(.+)")
        if not fp_str then task:CantDo(); return end
        local secs = parse_time(time_str)
        if not secs then task:CantDo(); return end
        set_tot(tonumber(fp_str), tonumber(wpt_str), secs)
        update_flightplans()
        Wheel.NavigateTo({ "Navigation", "Time over Target", FP_LABELS[tonumber(fp_str)] })
        task:Roger()
    end)

    ListenTo("tot_clear", "ToT_Clear", function(task, value)
        local fp_str, wpt_str = string.match(value, "(%d+);(%d+)")
        if not fp_str then task:CantDo(); return end
        local fp_no = tonumber(fp_str)
        set_tot(fp_no, tonumber(wpt_str), nil)
        update_flightplans()
        Wheel.NavigateTo({ "Navigation", "Time over Target", FP_LABELS[fp_no] })
        task:Roger()
    end)

    ListenTo("tot_assist_yes", "ToT_AssistYes", function()
        local gs  = self.assist_required_gs
        local rem = self.assist_remaining
        local fp  = self.assist_fp
        local wpt = self.assist_wpt
        if not (gs and rem and fp and wpt) then return end
        self:StartAssist(fp, wpt, gs, rem)
    end)

    ListenTo("tot_assist_no", "ToT_AssistNo", function()
        self:StopAssist()
        Log("ToT: pilot declined assistance")
    end)

    ListenTo("tot_assist_start", "ToT_AssistStart", function(task)
        local memory = GetJester().memory
        local fp  = memory:GetActiveFlightPlanNumber()
        local wpt = memory:GetActiveWaypointNumber()

        if not get_tot(fp, wpt) then
            task:CantDo(); return
        end

        local required_gs, remaining, feasible = calc_required_gs(fp, wpt)
        if not required_gs or not feasible then
            task:CantDo(); return
        end

        self:AskTotAssistance(required_gs, remaining, fp, wpt)
    end)

    ListenTo("tot_assist_stop", "ToT_AssistStop", function(task)
        self:StopAssist()
        Log("ToT: monitoring stopped by pilot")
        task:Roger()
    end)

    Wheel.AddItem(Wheel.Item:new({
        name = "Time over Target",
        menu = Wheel.Menu:new({
            name  = "Time over Target",
            items = {
                Wheel.Item:new({ name = "Primary Flight Plan" }),
                Wheel.Item:new({ name = "Secondary Flight Plan" }),
                Wheel.Item:new({
                    name     = "Start ToT Assist",
                    action   = "tot_assist_start",
                    reaction = Wheel.Reaction.CLOSE_REMEMBER,
                }),
                Wheel.Item:new({
                    name     = "Stop ToT Assist",
                    action   = "tot_assist_stop",
                    reaction = Wheel.Reaction.CLOSE_REMEMBER,
                }),
            },
        }),
    }), { "Navigation" })

    update_flightplans()
end

function TimeOverTarget:Tick()
    if not self.is_registered then
        self:Register()
        self.is_registered = true
    end

    self.refresh_urge:Tick()

    if self.assist_active then
        self.monitor_urge:Tick()
    end

    if self.transition_pending then
        self.transition_urge:Tick()
    end

    -- When active waypoint changes, schedule deferred check (5s)
    local memory = GetJester().memory
    local fp  = memory:GetActiveFlightPlanNumber()
    local wpt = memory:GetActiveWaypointNumber()

    if self.prev_active_wpt
        and (fp ~= self.prev_active_fp or wpt ~= self.prev_active_wpt)
    then
        self.transition_pending = true
        self.transition_urge:Restart()
    end

    self.prev_active_fp  = fp
    self.prev_active_wpt = wpt
end

TimeOverTarget.GetTotSeconds = get_tot

TimeOverTarget:Seal()
return TimeOverTarget
