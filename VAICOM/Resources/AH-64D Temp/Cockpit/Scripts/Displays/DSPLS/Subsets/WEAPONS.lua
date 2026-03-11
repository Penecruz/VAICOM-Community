dofile(LockOn_Options.script_path.."Displays/DSPLS/DSPLS_Symbology.lua")

local scale_coeff				= display_size_pix/720
-- Center of the video area
local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0, 0}, nil, {{"VideoAreaPH"}, {"VideoSignal_Show"}})

--
local TADS_LOS_Reticle = add_TADS_LOS_Reticle("TADS_LOS_Reticle", {0,0}, VideoAreaPH.name, {{"DSPLS_TADS_LOS_ReticleShow"}, {"DSPLS_TADS_LOS_ReticleFlash"}})

addStrokeText("SelectedSensor",	"FLIR",	STROKE_FNT_DFLT_100, "LeftBottom",	{-620, 395}, VideoAreaPH.name, {{"DSPLS_WEAPON_SelectedSensor"}}, {"", "1FLIR", "2FLIR", "FLIR", "TV"})
addStrokeText("Airspeed",		"19",	STROKE_FNT_DFLT_100, "RightBottom",	{-220,-570}, VideoAreaPH.name, {{"DSPLS_HOVER_Airspeed"}})
addStrokeText("RadarAltitude",	"243",	STROKE_FNT_DFLT_100, "LeftBottom",	{ 220,-570}, VideoAreaPH.name, {{"DSPLS_HOVER_RadarAltitudeDigits"}})

-- Zoom Gates
local ZoomGatesPH = addPlaceholder("ZoomGatesPH", {0, 0}, VideoAreaPH.name, {{"DSPLS_WEAPON_ZoomGatesShow"}})
local zg_len = 55
local cf = 720/768
local video_w_05 = RadToDI(2 * math.tan(math.rad(20.0)))/2
local video_w_05_cf = video_w_05*cf
local video_h_05_cf = 0.75*video_w_05_cf

local zoomGateLT = addStrokeLine("zoomGateLT", {{0,-zg_len},	{0,0},	{zg_len,0}},	4,	nil,	ZoomGatesPH.name, {{"DSPLS_WEAPON_ZoomGates", -video_w_05_cf,  video_h_05_cf}})
local zoomGateRT = addStrokeLine("zoomGateRT", {{-zg_len, 0},	{0,0},	{0,-zg_len}},	4,	nil,	ZoomGatesPH.name, {{"DSPLS_WEAPON_ZoomGates",  video_w_05_cf,  video_h_05_cf}})
local zoomGateRB = addStrokeLine("zoomGateRB", {{0,zg_len},		{0,0},	{-zg_len,0}},	4,	nil,	ZoomGatesPH.name, {{"DSPLS_WEAPON_ZoomGates",  video_w_05_cf, -video_h_05_cf}})
local zoomGateLB = addStrokeLine("zoomGateLB", {{zg_len, 0},	{0,0},	{0,zg_len}},	4,	nil,	ZoomGatesPH.name, {{"DSPLS_WEAPON_ZoomGates", -video_w_05_cf, -video_h_05_cf}})

-- LMC ON notches
local LMC_d = 25
local LMC_w = 215 + LMC_d
local LMC_h = 160 + LMC_d
local LMC_l = 15
local LMC_PH = addPlaceholder("LMC_PH", {0,0}, VideoAreaPH.name, {{"DSPLS_WEAPON_LMC_On"}})
local LMC_NotchL = addStrokeLine("LMC_NotchL", {{-LMC_w, -LMC_l},	{-LMC_w, LMC_l}},	4, nil, LMC_PH.name)
local LMC_NotchT = addStrokeLine("LMC_NotchT", {{-LMC_l, LMC_h},	{LMC_l, LMC_h}},	4, nil, LMC_PH.name)
local LMC_NotchR = addStrokeLine("LMC_NotchR", {{LMC_w, -LMC_l},	{LMC_w, LMC_l}},	4, nil, LMC_PH.name)
local LMC_NotchB = addStrokeLine("LMC_NotchB", {{-LMC_l, -LMC_h},	{LMC_l, -LMC_h}},	4, nil, LMC_PH.name)

-- Laser Firing Indicator
local LaserFiring_s = 65
local LaserFiring_e = 130
local LaserFiring_PH = addPlaceholder("LaserFiring_PH", {0,0}, VideoAreaPH.name, {{"DSPLS_WEAPON_LaserFiring"}})
local LaserFiring_RayLT = addStrokeLine("LaserFiring_RayLT", {{-LaserFiring_s, LaserFiring_s}, {-LaserFiring_e, LaserFiring_e}}, 4, nil, LaserFiring_PH.name)
local LaserFiring_RayRT = addStrokeLine("LaserFiring_RayRT", {{ LaserFiring_s, LaserFiring_s}, { LaserFiring_e, LaserFiring_e}}, 4, nil, LaserFiring_PH.name)
local LaserFiring_RayRB = addStrokeLine("LaserFiring_RayRB", {{ LaserFiring_s,-LaserFiring_s}, { LaserFiring_e,-LaserFiring_e}}, 4, nil, LaserFiring_PH.name)
local LaserFiring_RayLB = addStrokeLine("LaserFiring_RayLB", {{-LaserFiring_s,-LaserFiring_s}, {-LaserFiring_e,-LaserFiring_e}}, 4, nil, LaserFiring_PH.name)

local s = 120
local CueingDot_U = addStrokeDot("CueingDot_U", { 0, s}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.UP}})
local CueingDot_D = addStrokeDot("CueingDot_D", { 0,-s}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.DOWN}})
local CueingDot_L = addStrokeDot("CueingDot_L", {-s, 0}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.LEFT}})
local CueingDot_R = addStrokeDot("CueingDot_R", { s, 0}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.RIGHT}})

local CuedLosReticle = addCuedLosReticle("CuedLosReticle", {0,0}, VideoAreaPH.name, {{"HMD_COMMON_CuedLosReticle", video_area_w_05, video_area_h_05}})
local FixedGunAimingReticle = addCuedLosReticle("FixedGunAimingReticle", {0,0}, VideoAreaPH.name, {{"HMD_COMMON_FixedGunAimingReticle", video_area_w_05, video_area_h_05}})

-- IAT tracks
local TotalTrackCountBoxPos = { 370 * scale_coeff, 320 * scale_coeff }
local TotalTrackCountPos = { TotalTrackCountBoxPos[1], TotalTrackCountBoxPos[2] - 3 }
addRoundedBox("TotalTrackCountBox", TotalTrackCountBoxPos, "CenterCenter", {30 * scale_coeff, 35 * scale_coeff}, 7, 4, VideoAreaPH.name, {{"DSPLS_WEAPON_TotalTrackCountShow"}})
--addText("TotalTrackCount",	"0",	TotalTrackCountPos, "CenterCenter", VideoAreaPH.name, {{"DSPLS_WEAPON_TotalTrackCount"}})
addStrokeText("TotalTrackCount",	"0",	STROKE_FNT_DFLT_100, "CenterCenter",	TotalTrackCountPos, VideoAreaPH.name, {{"DSPLS_WEAPON_TotalTrackCount"}})
