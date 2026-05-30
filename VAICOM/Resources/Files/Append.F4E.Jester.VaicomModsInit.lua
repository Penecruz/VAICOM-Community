local VaicomMods = require 'VaicomMods'

mod_init[#mod_init + 1] = function(jester)
    jester.behaviors[VaicomMods] = VaicomMods:new()
end
