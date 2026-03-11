dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/Common/HAD_Messages.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)

-------Elevation Altitude Status Window-------
local scaleSize = 720.0 / 1024.0
local FOR_w				= 196
local FOR_h				= 64
local FOR_w05			= FOR_w / 2
local FOR_h05			= FOR_h / 2
local FOR_margin_bottom	= 8

local MSL = addPlaceholder("MSL_PH", {0, -video_area_h_05 + FOR_h05 + FOR_margin_bottom - 5}, VideoAreaPH.name, {{"DSPLS_FCR_DisableFOR", 1}})
addRoundedBox("centeBoxMSL",   {-3, 2},	"CenterCenter", {160 * scaleSize , 90 * scaleSize}, 10, 6, MSL.name)

addText("UpperLimit", 		"0", 			{70 * scaleSize,  20 * scaleSize},	    "RightCenter", MSL.name, {{"DSPLS_FCR_ATM_UpperLimit"}})
addText("LowerLimit", 		"0", 			{70 * scaleSize, -20 * scaleSize},	    "RightCenter", MSL.name, {{"DSPLS_FCR_ATM_LowerLimit"}})