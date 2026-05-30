-- Shutdown.lua
-- VAICOM Jester custom mod action to shut down the rear cockpit systems in sequence.

local Class      = require('base.Class')
local Behavior   = require('base.Behavior')
local Task       = require('base.Task')

local Shutdown = Class(Behavior)
Shutdown.is_registered = false

local MENU_PATH = { "Crew" }

local function queue_switch(manipulator, state)
    local jester = GetJester()
    if not jester then
        return
    end

    local ok = pcall(function()
        jester:AddTask(SwitchTask:new(manipulator, state))
    end)

    if not ok then
        Log("Shutdown: unable to set " .. manipulator .. " to " .. state)
    end
end

local function run_shutdown_sequence()
    local task = Task:new()
    task:Say('misc/roger')

    -- Rear cockpit systems shutdown are done one-by-one.
    task:Then(function() queue_switch("Radar Operation", "standby") end)
        :Wait(s(1), { voice = true })
        :Then(function() queue_switch("Jammer", "standby") end)
        :Wait(s(1), { voice = true })        
        :Then(function() queue_switch("Chaff Mode", "OFF") end)
        :Wait(s(1), { voice = true })
        :Then(function() queue_switch("Flare Mode", "OFF") end)
        :Wait(s(1), { voice = true })
        :Then(function() queue_switch("Tacan Mode", "Off") end)
        :Wait(s(1), { voice = true })
        :Then(function() queue_switch("Radio Mode", "Off") end)
        :Wait(s(1), { voice = true })
        :Then(function() queue_switch("Pave Spike Operation", "standby") end)
        :Say('refueling/LookingGood')
        :Then(function()
            Log("Shutdown complete, ready for engine shutdown")
        end)

    GetJester():AddTask(task)
end

function Shutdown:Register()
    ListenTo("custom_shutdown", "Custom_Shutdown", function(task)
        run_shutdown_sequence()
    end)

    Wheel.AddItem(Wheel.Item:new({
        name = "Shutdown",
        action = "custom_shutdown",
        action_value = "start",
        reaction = Wheel.Reaction.CLOSE_REMEMBER,
    }), MENU_PATH)
end

function Shutdown:Tick()
    if not self.is_registered then
        self:Register()
        self.is_registered = true
    end
end

Shutdown:Seal()
return Shutdown
