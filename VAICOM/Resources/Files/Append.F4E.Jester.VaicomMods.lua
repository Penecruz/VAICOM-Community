-- VAICOM auto-installed Jester mods
-- Custom VAICOM behaviors for F-4E WSO.

local Class      = require('base.Class')
local Behavior   = require('base.Behavior')
local Task       = require('base.Task')
local Urge       = require('base.Urge')
local StressReaction = require('base.StressReaction')

local VaicomMods = Class(Behavior)
VaicomMods.is_registered = false

local MENU_PATH = { "Crew Contract" }

local RADALT_SPEED_GATE_KTS = 270
local RADALT_HYSTERESIS_FEET = 25
local RADALT_CALL_BELOW_TARGET_FEET = 20
local RADALT_PRESET_VALUES = { 500, 300, 200, 100, 50 }

local function parse_value(raw)
    if raw == nil then
        return nil
    end

    if type(raw) == "boolean" then
        return raw and 1 or 0
    end

    if type(raw) == "number" then
        return raw
    end

    local text = tostring(raw):lower()
    if text == "true" then
        return 1
    elseif text == "false" then
        return 0
    end

    local token = tostring(raw):match("([%d%.e%+%-]+)")
    return token and tonumber(token) or nil
end

local function read_property(path, name)
    local prop = GetProperty(path, name)
    if not prop or not prop:IsValid() then
        return nil
    end
    return parse_value(prop.value)
end

local function read_observation_value(key)
    local obs = GetJester().awareness:GetObservation(key)
    if obs == nil then
        return nil
    end

    if type(obs) == "boolean" then
        return obs
    end

    if obs.value ~= nil then
        return parse_value(obs.value)
    end

    return parse_value(obs)
end

local function get_radalt_warning_phrase(feet)
    local buckets = {
        { threshold = 10, phrase = 'checklists/10ft' },
        { threshold = 30, phrase = 'checklists/30ft' },
        { threshold = 50, phrase = 'checklists/50ft' },
        { threshold = 100, phrase = 'checklists/100ft' },
        { threshold = 200, phrase = 'checklists/200ft' },
        { threshold = 300, phrase = 'checklists/300ft' },
        { threshold = 400, phrase = 'checklists/400ft' },
        { threshold = 500, phrase = 'checklists/500ft' },
        { threshold = 1000, phrase = 'checklists/1000ft' },
    }

    for _, bucket in ipairs(buckets) do
        if feet <= bucket.threshold then
            return bucket.phrase
        end
    end

    return 'checklists/1000ft'
end

local function run_shutdown_sequence()
    local task = Task:new()
    task:Say('misc/roger')

    local tgp_power_on = read_property('/EO TGT Designator System/Target Designator Set Control', 'Power On Light')
    local rwr_powered = read_property('/RWR AN_ALR_46/WSO Lights/System Power Lamp', 'Powered')

    task:Click("Radar Power", "STBY")
        :Click("Screen Mode", "off")
        :Wait(s(4), { voice = true })
        :Click("ECM Mode Left", "STBY")
        :Click("ECM Mode Right", "STBY")
        :Wait(s(5), { voice = true })
        :Click("Chaff Mode", "OFF")
        :Wait(s(1), { voice = true })
        :Click("Flare Mode", "OFF")
        :Wait(s(5), { voice = true })

    if rwr_powered ~= nil and rwr_powered > 0 then
        task:Click("WSO RWR System Power Button", "ON")
    end
    task:Wait(s(3), { voice = true })
        :Click("Radio Mode", "OFF")
        :Wait(s(2), { voice = true })
        :Click("TACAN Function", "OFF")
        :Wait(s(6), { voice = true })
        :Click("INS Mode Knob", "OFF")
        :Click("Align Mode Knob", "OFF")
        :Wait(s(3), { voice = true })
        :Click("Nav Panel Function", "OFF")
        :Wait(s(6), { voice = true })

    if tgp_power_on ~= nil and tgp_power_on > 0 then
        task:ClickShort("TGP Power On", "ON")
    end

    task:Say('checklists/Hold')
        :Wait(s(7), { voice = true })
        :Say('checklists/continue')

    GetJester():AddTask(task)
end

local function monitor_radalt_bug(self)
    if not self.radalt_monitor_enabled or self.radalt_min_alt_feet == nil then
        return
    end

    local airborne = GetJester().awareness:GetObservation("airborne") or false
    if not airborne then
        self.radalt_latched = false
        return
    end

    local ias = read_observation_value('indicated_airspeed')
    if ias == nil or ias < RADALT_SPEED_GATE_KTS then
        self.radalt_latched = false
        return
    end

    local radar_alt = read_property('/Pilot Radar Altimeter', 'Indicated Altitude')
    if radar_alt == nil then
        return
    end

    local descending = self.last_radar_alt ~= nil and radar_alt < (self.last_radar_alt - 1)
    local call_threshold = self.radalt_min_alt_feet - RADALT_CALL_BELOW_TARGET_FEET
    if call_threshold < 0 then
        call_threshold = 0
    end
    local is_below_minimum = radar_alt <= call_threshold

    if not self.radalt_latched and descending and is_below_minimum then
        local phrase = get_radalt_warning_phrase(self.radalt_min_alt_feet)
        local task = Task:new()
        task:Say(phrase)
        GetJester():AddTask(task)
        self.radalt_latched = true
    elseif self.radalt_latched then
        if radar_alt > (self.radalt_min_alt_feet + RADALT_HYSTERESIS_FEET) then
            self.radalt_latched = false
        end
    end

    self.last_radar_alt = radar_alt

    if ias == nil or ias < RADALT_SPEED_GATE_KTS then
        self.last_radar_alt = nil
        self.radalt_latched = false
    end
end

function VaicomMods:Constructor()
    Behavior.Constructor(self)
    self.radalt_monitor_enabled = false
    self.radalt_min_alt_feet = nil
    self.radalt_latched = false
    self.last_radar_alt = nil
    self.monitor_urge = Urge:new({
        time_to_release = s(1),
        on_release_function = function()
            monitor_radalt_bug(self)
        end,
        stress_reaction = StressReaction.ignorance,
    })
    self.monitor_urge:Restart()
end

function VaicomMods:Register()
    ListenTo("custom_shutdown", "Custom_Shutdown", function(task)
        local airborne = GetJester().awareness:GetObservation("airborne") or false
        if airborne then
            task:CantDo()
            return
        end

        run_shutdown_sequence()
    end)

    ListenTo("vaicom_radalt_monitor", "VaicomMods", function(task, mode)
        local selected = tonumber(mode)
        if selected ~= nil then
            self.radalt_monitor_enabled = true
            self.radalt_min_alt_feet = selected
        else
            self.radalt_monitor_enabled = false
            self.radalt_min_alt_feet = nil
        end

        self.radalt_latched = false
        self.last_radar_alt = nil

        task:Say('misc/roger')
    end)

    local radalt_items = {}
    for _, value in ipairs(RADALT_PRESET_VALUES) do
        table.insert(radalt_items, Wheel.Item:new({
            name = "Set " .. tostring(value) .. " ft",
            action = "vaicom_radalt_monitor",
            action_value = tostring(value),
            reaction = Wheel.Reaction.CLOSE_REMEMBER,
        }))
    end
    table.insert(radalt_items, Wheel.Item:new({
        name = "Off",
        action = "vaicom_radalt_monitor",
        action_value = "off",
        reaction = Wheel.Reaction.CLOSE_REMEMBER,
    }))

    Wheel.AddItem(Wheel.Item:new({
        name = "VAICOM Mods",
        menu = Wheel.Menu:new({
            name = "VAICOM Mods",
            items = {
                Wheel.Item:new({
                    name = "Shutdown",
                    action = "custom_shutdown",
                    action_value = "start",
                    reaction = Wheel.Reaction.CLOSE_REMEMBER,
                }),
                Wheel.Item:new({
                    name = "Radar Alt Monitor",
                    menu = Wheel.Menu:new({
                        name = "Radar Alt Monitor",
                        items = radalt_items,
                    }),
                }),
            },
        }),
    }), MENU_PATH)
end

function VaicomMods:Tick()
    if not self.is_registered then
        self:Register()
        self.is_registered = true
    end

    if self.monitor_urge then
        self.monitor_urge:Tick()
    end
end

VaicomMods:Seal()
return VaicomMods
