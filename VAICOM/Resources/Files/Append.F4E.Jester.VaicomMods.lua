-- VAICOM auto-installed Jester mods
-- Custom VAICOM behaviors for F-4E WSO.

local Class      = require('base.Class')
local Behavior   = require('base.Behavior')
local Task       = require('base.Task')

local VaicomMods = Class(Behavior)
VaicomMods.is_registered = false

local MENU_PATH = { "Crew" }

local function run_shutdown_sequence()
    local task = Task:new()
    task:Say('misc/roger')

    task:Click("Radar Power", "STBY")
        :Wait(s(1), { voice = true })
        :Click("TACAN Function", "OFF")
        :Wait(s(1), { voice = true })
        :Click("Radio Mode", "OFF")
        :Wait(s(1), { voice = true })
        :Click("ECM Mode Left", "STBY")
        :Click("ECM Mode Right", "STBY")
        :Wait(s(1), { voice = true })
        :Click("Chaff Mode", "OFF")
        :Wait(s(1), { voice = true })
        :Click("Flare Mode", "OFF")
        :Say('checklists/continue')
        :Then(function()
            Log("Shutdown complete, ready for engine shutdown")
        end)

    GetJester():AddTask(task)
end

function VaicomMods:Register()
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

function VaicomMods:Tick()
    if not self.is_registered then
        self:Register()
        self.is_registered = true
    end
end

VaicomMods:Seal()
return VaicomMods
