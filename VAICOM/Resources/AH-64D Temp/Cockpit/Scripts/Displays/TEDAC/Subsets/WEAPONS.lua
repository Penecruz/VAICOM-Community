dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")

-- Center of the video area
local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)

--
local TADS_LOS_Reticle = add_TADS_LOS_Reticle("TADS_LOS_Reticle", {0,0}, VideoAreaPH.name, {{"DSPLS_TADS_LOS_ReticleShow"}, {"DSPLS_TADS_LOS_ReticleFlash"}})
local FLT_LOS_Reticle = add_HOVER_LOS_Reticle("FLT_LOS_Reticle", {0,0}, VideoAreaPH.name, {{"DSPLS_FLT_LOS_ReticleShow"}, {"DSPLS_HOVER_LOS_Reticle"}})

--
addText("SelectedSensor",	"FLIR",	{-291, 184}, "LeftBottom", VideoAreaPH.name, {{"DSPLS_WEAPON_SelectedSensor"}}, {"", "1FLIR", "2FLIR", "FLIR", "TV"})
addText("Airspeed",			"19",	{-104,-286}, "RightBottom", VideoAreaPH.name, {{"DSPLS_HOVER_Airspeed"}})
addText("RadarAltitude",	"243",	{ 104,-286}, "LeftBottom", VideoAreaPH.name, {{"DSPLS_HOVER_RadarAltitudeDigits"}})

-- Zoom Gates
local ZoomGatesPH = addPlaceholder("ZoomGatesPH", {0, 0}, VideoAreaPH.name, {{"DSPLS_WEAPON_ZoomGatesShow"}})
local zg_len = 25
local zoomGateLT = addLine("zoomGateLT", {{0,-zg_len},	{0,0},	{zg_len,0}},	4, ZoomGatesPH.name, {{"DSPLS_WEAPON_ZoomGates", -video_area_w_05, video_area_h_05}})
local zoomGateRT = addLine("zoomGateRT", {{-zg_len, 0},	{0,0},	{0,-zg_len}},	4, ZoomGatesPH.name, {{"DSPLS_WEAPON_ZoomGates", video_area_w_05, video_area_h_05}})
local zoomGateRB = addLine("zoomGateRB", {{0,zg_len},	{0,0},	{-zg_len,0}},	4, ZoomGatesPH.name, {{"DSPLS_WEAPON_ZoomGates", video_area_w_05, -video_area_h_05}})
local zoomGateLB = addLine("zoomGateLB", {{zg_len, 0},	{0,0},	{0,zg_len}},	4, ZoomGatesPH.name, {{"DSPLS_WEAPON_ZoomGates", -video_area_w_05, -video_area_h_05}})

-- LMC ON notches
local LMC_d = 8
local LMC_w = 100 + LMC_d
local LMC_h = 75 + LMC_d
local LMC_l = 7
local LMC_PH = addPlaceholder("LMC_PH", {0,0}, VideoAreaPH.name, {{"DSPLS_WEAPON_LMC_On"}})
local LMC_NotchL = addLine("LMC_NotchL", {{-LMC_w, -LMC_l}, {-LMC_w, LMC_l}}, 4, LMC_PH.name)
local LMC_NotchT = addLine("LMC_NotchT", {{-LMC_l, LMC_h}, {LMC_l, LMC_h}}, 4, LMC_PH.name)
local LMC_NotchR = addLine("LMC_NotchR", {{LMC_w, -LMC_l}, {LMC_w, LMC_l}}, 4, LMC_PH.name)
local LMC_NotchB = addLine("LMC_NotchB", {{-LMC_l, -LMC_h}, {LMC_l, -LMC_h}}, 4, LMC_PH.name)

-- Laser Firing Indicator
local LaserFiring_s = 30
local LaserFiring_e = 60
local LaserFiring_PH = addPlaceholder("LaserFiring_PH", {0,0}, VideoAreaPH.name, {{"DSPLS_WEAPON_LaserFiring"}})
local LaserFiring_RayLT = addLine("LaserFiring_RayLT", {{-LaserFiring_s, LaserFiring_s}, {-LaserFiring_e, LaserFiring_e}}, 4, LaserFiring_PH.name)
local LaserFiring_RayRT = addLine("LaserFiring_RayRT", {{ LaserFiring_s, LaserFiring_s}, { LaserFiring_e, LaserFiring_e}}, 4, LaserFiring_PH.name)
local LaserFiring_RayRB = addLine("LaserFiring_RayRB", {{ LaserFiring_s,-LaserFiring_s}, { LaserFiring_e,-LaserFiring_e}}, 4, LaserFiring_PH.name)
local LaserFiring_RayLB = addLine("LaserFiring_RayLB", {{-LaserFiring_s,-LaserFiring_s}, {-LaserFiring_e,-LaserFiring_e}}, 4, LaserFiring_PH.name)

local FixedGunAimingReticle = addCuedLosReticle("FixedGunAimingReticle", {0,0}, VideoAreaPH.name, {{"DSPLS_COMMON_FixedGunAimingReticle", video_area_w_05, video_area_h_05}})

-- IAT tracks
local TotalTrackCountBoxPos = { 370, 320 }
local TotalTrackCountPos = { TotalTrackCountBoxPos[1], TotalTrackCountBoxPos[2] - 3 }
addRoundedBox("TotalTrackCountBox", TotalTrackCountBoxPos, "CenterCenter", {30, 35}, 7, 4, VideoAreaPH.name, {{"DSPLS_WEAPON_TotalTrackCountShow"}})
addText("TotalTrackCount",	"0",	TotalTrackCountPos, "CenterCenter", VideoAreaPH.name, {{"DSPLS_WEAPON_TotalTrackCount"}})

-----
local CUEING_DOT =
{
	UP		= 0;
	DOWN	= 1;
	LEFT	= 2;
	RIGHT	= 3;
}
local s = 40
local CueingDot_PH = addPlaceholder("CueingDot_PH", {0, 0}, VideoAreaPH.name, {{"DSPLS_COMMON_CueingDotEnable"}})
local CueingDot_U = addDot("CueingDot_U", { 0, s}, CueingDot_PH.name, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.UP}})
local CueingDot_D = addDot("CueingDot_D", { 0,-s}, CueingDot_PH.name, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.DOWN}})
local CueingDot_L = addDot("CueingDot_L", {-s, 0}, CueingDot_PH.name, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.LEFT}})
local CueingDot_R = addDot("CueingDot_R", { s, 0}, CueingDot_PH.name, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.RIGHT}})

addcScopeSymbols(VideoAreaPH.name)