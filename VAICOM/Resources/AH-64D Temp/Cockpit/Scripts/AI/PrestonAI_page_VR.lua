dofile(LockOn_Options.common_script_path.."elements_defs.lua")
dofile(LockOn_Options.script_path.."AI/PrestonAI_page_common.lua")

local weap_control_size = 0.25
local compass_size = 0.45

create_and_add_elements(weap_control_size, compass_size, true)
