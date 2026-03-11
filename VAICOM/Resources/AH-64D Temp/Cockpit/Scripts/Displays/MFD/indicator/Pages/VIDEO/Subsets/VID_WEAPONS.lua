dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/VIDEO/VIDEO_PAGE_Symbology.lua")

--------------------------------------------------------------------------------------------------------
-- TADS LOS Reticle defs
local TADS_LOS_Reticle_w05 = 142
local TADS_LOS_Reticle_h05 = 107

function add_TADS_LOS_Reticle(name, pos, parent, controllers)
	local w05 = TADS_LOS_Reticle_w05
	local h05 = TADS_LOS_Reticle_h05
	local s_w05 = 20	-- space
	local s_h05 = 14	-- space

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local lu = addLine(name.."_up",		{{     0, s_h05}, {   0, h05}},	8, PH.name, nil)
	local ld = addLine(name.."_down",	{{     0,-s_h05}, {   0,-h05}},	8, PH.name, nil)
	local ll = addLine(name.."_left",	{{-s_w05,     0}, {-w05,   0}},	8, PH.name, nil)
	local lr = addLine(name.."_right",	{{ s_w05,     0}, { w05,   0}},	8, PH.name, nil)
	return {
		PH,
		lu[1], lu[2],
		ld[1], ld[2],
		ll[1], ll[2],
		lr[1], lr[2],
	} 
end

function add_LMC_On(name, pos, parent, controllers)
	local w05 = TADS_LOS_Reticle_w05 + 8	
	local h05 = TADS_LOS_Reticle_h05 + 8	
	local len = 24		
	local l05 = len / 2

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local l1 = addLine(name.."_up",		{{-l05, h05}, { l05, h05}}, 8, PH.name, nil)
	local l2 = addLine(name.."_down",	{{-l05,-h05}, { l05,-h05}}, 8, PH.name, nil)
	local l3 = addLine(name.."_left",	{{-w05,-l05}, {-w05, l05}}, 8, PH.name, nil)
	local l4 = addLine(name.."_right",	{{ w05,-l05}, { w05, l05}}, 8, PH.name, nil)
	return {
		PH,

		l1[1], l1[2],
		l2[1], l2[2],
		l3[1], l3[2],
		l4[1], l4[2],
	}
end

function add_LaserFiringIndicator(name, pos, parent, controllers)
	local len_gain = 7 / 10
	local w05 = TADS_LOS_Reticle_h05 * len_gain
	local h05 = w05
	local s_w05 = w05 / 2
	local s_h05 = s_w05

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local l1 = addLine(name.."_up_left",	{{-s_w05, s_h05}, {-w05, h05}},	8, PH.name, nil)
	local l2 = addLine(name.."_up_right",	{{ s_w05, s_h05}, { w05, h05}},	8, PH.name, nil)
	local l3 = addLine(name.."_down_left",	{{-s_w05,-s_h05}, {-w05,-h05}},	8, PH.name, nil)
	local l4 = addLine(name.."_down_right",	{{ s_w05,-s_h05}, { w05,-h05}},	8, PH.name, nil)
	return {
		PH,
		l1[1], l1[2],
		l2[1], l2[2],
		l3[1], l3[2],
		l4[1], l4[2],
	}
end
-------------------------------------------------------------------------------------------------------- 
-- Center of the video area
local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0, 0}, nil, nil)

--
local TADS_LOS_Reticle = add_TADS_LOS_Reticle("TADS_LOS_Reticle", {0,0}, VideoAreaPH.name, {{"DSPLS_TADS_LOS_ReticleShow"}, {"DSPLS_TADS_LOS_ReticleFlash"}})

addText("SelectedSensor",	"FLIR",	{-380, 242}, "LeftBottom", VideoAreaPH.name, {{"VIDEO_WEAPON_SelectedSensor"}}, {"", "1FLIR", "2FLIR", "FLIR", "TV"})
addText("Airspeed",			"19",	{-155, FOR_y + FOR_h + FOR_margin_bottom + 10}, "RightBottom", VideoAreaPH.name, {{"DSPLS_HOVER_Airspeed"}})
addText("RadarAltitude",	"243",	{ 155, FOR_y + FOR_h + FOR_margin_bottom + 10}, "LeftBottom", VideoAreaPH.name, {{"DSPLS_HOVER_RadarAltitudeDigits"}})

-- Zoom Gates
local ZoomGatesPH = addPlaceholder("ZoomGatesPH", {0, 0}, VideoAreaPH.name, {{"DSPLS_WEAPON_ZoomGatesShow"}})
local zg_len = 35
local cf = 720/768
local video_area_w_05_cf = video_area_w_05*cf
local video_area_h_05_cf = 0.75*video_area_w_05_cf
local zoomGateLT = addLine("zoomGateLT", {{0,-zg_len},	{0,0},	{zg_len,0}},	8, ZoomGatesPH.name, {{"VIDEO_WEAPON_ZoomGates", -video_area_w_05_cf,  video_area_h_05_cf}})
local zoomGateRT = addLine("zoomGateRT", {{-zg_len, 0},	{0,0},	{0,-zg_len}},	8, ZoomGatesPH.name, {{"VIDEO_WEAPON_ZoomGates",  video_area_w_05_cf,  video_area_h_05_cf}})
local zoomGateRB = addLine("zoomGateRB", {{0,zg_len},	{0,0},	{-zg_len,0}},	8, ZoomGatesPH.name, {{"VIDEO_WEAPON_ZoomGates",  video_area_w_05_cf, -video_area_h_05_cf}})
local zoomGateLB = addLine("zoomGateLB", {{zg_len, 0},	{0,0},	{0,zg_len}},	8, ZoomGatesPH.name, {{"VIDEO_WEAPON_ZoomGates", -video_area_w_05_cf, -video_area_h_05_cf}})

-- LMC ON notches
add_LMC_On("LMC_PH", {0,0}, VideoAreaPH.name, {{"DSPLS_WEAPON_LMC_On"}})

-- Laser Firing Indicator
add_LaserFiringIndicator("LaserFiring_PH", {0,0}, VideoAreaPH.name, {{"DSPLS_WEAPON_LaserFiring"}})

local s = 78
local s = PitchLadder_h/16 + 20 
local CueingDot_U = addDot("CueingDot_U", { 0, s}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.UP}})
local CueingDot_D = addDot("CueingDot_D", { 0,-s}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.DOWN}})
local CueingDot_L = addDot("CueingDot_L", {-s, 0}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.LEFT}})
local CueingDot_R = addDot("CueingDot_R", { s, 0}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.RIGHT}})

local CuedLosReticle = addCuedLosReticle("CuedLosReticle", {0,0}, VideoAreaPH.name, {{"VIDEO_COMMON_CuedLosReticle", 0, 0.75*video_area_h_05, video_area_h_05, 1.5*video_area_h_05}})
local FixedGunAimingReticle = addCuedLosReticle("FixedGunAimingReticle", {0,0}, VideoAreaPH.name, {{"VIDEO_COMMON_FixedGunAimingReticle", 0, 0.75*video_area_h_05, video_area_h_05, 1.5*video_area_h_05}})


-- IAT tracks
local TotalTrackCountBoxPos = { 370, 320 }
local TotalTrackCountPos = { TotalTrackCountBoxPos[1], TotalTrackCountBoxPos[2] - 3 }
addRoundedBox("TotalTrackCountBox", TotalTrackCountBoxPos, "CenterCenter", {30, 35}, 7, 4, VideoAreaPH.name, {{"DSPLS_WEAPON_TotalTrackCountShow"}})
addText("TotalTrackCount",	"0",	TotalTrackCountPos, "CenterCenter", VideoAreaPH.name, {{"DSPLS_WEAPON_TotalTrackCount"}})

