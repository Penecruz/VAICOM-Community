dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)

addBobUpBox("BobUpBox", {0, 0}, VideoAreaPH.name, {{"DSPLS_BOBUP_BobUpBox", (1-6.0/40.0)*PitchLadder_h/2 }})
