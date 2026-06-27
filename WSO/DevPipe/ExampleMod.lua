-- See https://f4.manuals.heatblur.se/dcs/modding/jester/lua_api.html for details on the API
local Class = require('base.Class')
local Behavior = require('base.Behavior')
local Urge = require('base.Urge')
local StressReaction = require('base.StressReaction')

local default_interval = s(5)

local ExampleMod = Class(Behavior)

function ExampleMod:Constructor()
  Behavior.Constructor(self)

  local sayHello = function()
    -- Say hello (see Jester Console) roughly every 5 seconds
    Log("Hello World!")
  end

  self.check_urge = Urge:new({
    time_to_release = default_interval,
    on_release_function = sayHello,
    stress_reaction = StressReaction.ignorance,
  })
  self.check_urge:Restart()
end

function ExampleMod:Tick()
  if self.check_urge then
    self.check_urge:Tick()
  end
end

ExampleMod:Seal()
return ExampleMod
