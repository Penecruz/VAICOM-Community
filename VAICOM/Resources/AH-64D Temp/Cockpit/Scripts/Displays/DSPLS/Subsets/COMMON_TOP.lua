dofile(LockOn_Options.script_path.."Displays/DSPLS/DSPLS_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/Common/HAD_Messages.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0, 0}, nil, {{"VideoAreaPH"}, {"VideoSignal_Show"}})

add_RKT_SteeringCursor(VideoAreaPH.name)
add_missile_constraints_box(VideoAreaPH.name)