dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/Common/HAD_Messages.lua")


local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)

add_RKT_SteeringCursor(VideoAreaPH.name)
add_missile_constraints_box(VideoAreaPH.name)

local CuedLosReticle = addCuedLosReticle("CuedLosReticle", {0,0}, VideoAreaPH.name, {{"DSPLS_COMMON_CuedLosReticle", video_area_h_05, video_area_h_05}})