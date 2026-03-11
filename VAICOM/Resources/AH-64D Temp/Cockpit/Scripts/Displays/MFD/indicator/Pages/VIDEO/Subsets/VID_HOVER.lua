dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/VIDEO/VIDEO_PAGE_Symbology.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0, 0}, nil, nil)

addText("SelectedSensorPNVS",		"FLIR",			{-400, 347}, "LeftBottom", VideoAreaPH.name,  {{"VIDEO_HOVER_SelectedSensor"}},  {"", "1FLIR", "2FLIR", "FLIR", "TV"})

addText("EngineTorque",				"90%",			{-400, 310}, "LeftBottom", VideoAreaPH.name,  {{"FLT_Torque"}})
addRoundedBox("torqueBorder_w", {-400, 305},  "LeftBottom" , {100, 45}, 1, 6,VideoAreaPH.name, {{"FLT_TorqueBorder", 1}})
addRoundedBox("torqueBorder", {-400, 305},  "LeftBottom" , {75, 45}, 1, 6,VideoAreaPH.name, {{"FLT_TorqueBorder", 0}})
addText("TurbineGasTemperature",	"860C",			{-400, 273}, "LeftBottom", VideoAreaPH.name,  {{"DSPLS_HOVER_TurbineGasTemperature"}})
addText("Airspeed",					" 13",			{-355,  10}, "RightBottom", VideoAreaPH.name, {{"DSPLS_HOVER_Airspeed"}})
addText("G-Status",					"1.3G",			{-400, -45}, "RightBottom", VideoAreaPH.name, {{"DSPLS_HOVER_G_Status"}})
addText("VideoStatus",				"VIDEO DEGR",	{   0, 240}, "CenterBottom", VideoAreaPH.name,{{"DSPLS_HOVER_VideoStatus"}}, {"", "VIDEO FROZEN", "VIDEO DEGR"})

addRoundedBox("AirspeedBorder", {-356, 10}, "RightBottom", {69, 34}, 1, 6, VideoAreaPH.name, {{"DSPLS_HOVER_AirspeedBorder"}})
addRoundedBox("AttitudeHold", {-349, 2}, "RightBottom", { 85, 48}, 10, 6, VideoAreaPH.name, {{"FLT_AttitudeHold"}})


-- Skid/Slip Ball

local SkidSlipInd_h				= 35
local FOR_w						= SkidSlipInd_h*7

local SkidSlipInd_h05			= SkidSlipInd_h / 2
local SkidSlipInd_Gates_w		= SkidSlipInd_h	
local SkidSlipInd_Gates_w05		= SkidSlipInd_Gates_w / 2
local SkidSlipInd_w				= FOR_w
local SkidSlipInd_w05			= SkidSlipInd_w / 2

local SkidSlipBallPH = addPlaceholder("SkidSlipBallPH", {0,  SkidSlipIndTop - SkidSlipInd_h }, VideoAreaPH.name)

local SkidSlipBall_Mask = openMaskArea(0, "SkidSlipBall_Mask", buildBoxVerts(SkidSlipInd_w + SkidSlipInd_h, SkidSlipInd_h, "CenterBottom"), default_box_indices, {0,0}, SkidSlipBallPH.name)
local SkidSlipBall = addSkidSlipBall("SkidSlipBall", {0, SkidSlipInd_h05}, SkidSlipBallPH.name, {{"DSPLS_HOVER_SkidSlipBall", SkidSlipInd_w05 - 0.2 * SkidSlipInd_h05}})
setElemsClipLevel(SkidSlipBall, 1)
resetMask(SkidSlipBall_Mask, 0, SkidSlipBallPH.name)

addLine("VidSkidSlipBall_bottom", {{-FOR_w05-10, 0}, {FOR_w05+10, 0}}, 6, SkidSlipBallPH.name)

addLine("SkidSlipBall_Wall_l", {{-SkidSlipInd_Gates_w05, 0}, {-SkidSlipInd_Gates_w05, SkidSlipInd_h}}, 6, SkidSlipBallPH.name)
addLine("SkidSlipBall_Wall_r", {{ SkidSlipInd_Gates_w05, 0}, { SkidSlipInd_Gates_w05, SkidSlipInd_h}}, 6, SkidSlipBallPH.name)
------------------------------------------------------
-- Velocity Vector
local dbg_vv_pos	= {1.0,1.0}
local vv_length_max	= video_area_h_05 *0.75
addLine("VelocityVector_line", {{0.0,0.0}, dbg_vv_pos}, 3, VideoAreaPH.name, {{"VIDEO_HOVER_VelocityVectorLine", vv_length_max}})
addPoint("VelocityVector_point", dbg_vv_pos, VideoAreaPH.name, {{"DSPLS_HOVER_VelocityVectorDot", vv_length_max}})

-- Acceleration Cue
local ac_length_max = vv_length_max
addAccelerationCue("AccelerationCue", {0, 0}, VideoAreaPH.name, {{"DSPLS_HOVER_AccelerationCue", ac_length_max}})

-- Vertical Scale
local VertScale_h	= 532
local VertScale_h05	= VertScale_h / 2
local VerticalScalePH = addPlaceholder("VerticalScalePH", {480, 30}, VideoAreaPH.name)
local AltitudeScalePH = addPlaceholder("AltitudeScalePH", {0,0}, VerticalScalePH.name, {{"DSPLS_HOVER_AltitudeScaleShow"}})
local RateOfClimbPH = addPlaceholder("RateOfClimbPH", {0,0}, VerticalScalePH.name, nil)

local VertScaleText_dx			= -45
local VertScaleText_dy			= 0
local VertScaleText_line_space	= 75
addText("AltitudeDigits",	"0",	{VertScaleText_dx, VertScaleText_dy},								"RightCenter", VerticalScalePH.name, {{"VIDEO_HOVER_RadarAltitudeDigits"}})
addBigText("AltitudeHI",	"HI",	{VertScaleText_dx-30, VertScaleText_dy + VertScaleText_line_space},	"RightCenter", VerticalScalePH.name, {{"DSPLS_HOVER_RadarAltitudeHI"}},nil, "VIDEO_BIG_YELLOW")
addBigText("AltitudeLO",	"LO",	{VertScaleText_dx-30, VertScaleText_dy - VertScaleText_line_space},	"RightCenter", VerticalScalePH.name, {{"DSPLS_HOVER_RadarAltitudeLO"}},nil, "VIDEO_BIG_RED")
-- long notches
local VertScaleNotch_margin	= 2
local LongNotchLen			= 24
for i = -2,2 do
	local dh = VertScale_h / 4
	local h = dh * i
	addLine("AltitudeScale_LongNotch_"..i,	{{ VertScaleNotch_margin, h}, { VertScaleNotch_margin + LongNotchLen, h}}, 4, AltitudeScalePH.name)
	if i == 0 then
		addLine("RateOfClimb_LongNotch_"..i,	{{-VertScaleNotch_margin + 14, h}, {-VertScaleNotch_margin - LongNotchLen, h}}, 10, RateOfClimbPH.name)
	else
		addLine("RateOfClimb_LongNotch_"..i,	{{-VertScaleNotch_margin, h}, {-VertScaleNotch_margin - LongNotchLen, h}}, 4, RateOfClimbPH.name)
	end
end
-- short notches
local ShortNotchLen			= 11
for i = 1,4 do
	local dh = VertScale_h / 20
	local h = dh * i

	local alt_h = -VertScale_h05 + h
	addLine("AltitudeScale_ShortNotch_"..i,	{{ VertScaleNotch_margin, alt_h}, { VertScaleNotch_margin + ShortNotchLen, alt_h}}, 4, AltitudeScalePH.name)

	addLine("RateOfClimb_ShortNotch_Up_"..i, {{-VertScaleNotch_margin, h}, {-VertScaleNotch_margin - ShortNotchLen, h}}, 4, RateOfClimbPH.name)
	addLine("RateOfClimb_ShortNotch_Dn_"..i, {{-VertScaleNotch_margin,-h}, {-VertScaleNotch_margin - ShortNotchLen,-h}}, 4, RateOfClimbPH.name)
end
-- altitude tape
local AltitudeTape_w = 20
local AltitudeTape_Mask = openMaskArea(0, "AltitudeTape_Mask", buildBoxVerts(AltitudeTape_w, VertScale_h, "CenterCenter"), default_box_indices, {0,-VertScale_h}, AltitudeScalePH.name, {{"DSPLS_HOVER_RadarAltitudeTape", VertScale_h}})
local AltitudeTape = addSimpleLine("AltitudeTape", {{0,-VertScale_h05},{0,VertScale_h05}}, AltitudeTape_w, IND_MPD_VIDEO_SYMBOLOGY, AltitudeScalePH.name, {{"VIDEO_HOVER_RadarAltitudeTapeColor"}})
setClipLevel(AltitudeTape, 1)
resetMask(AltitudeTape_Mask, 0, AltitudeScalePH.name)
-- altitude hold
addAltitudeHold("AltitudeHold", {10-VertScaleNotch_margin - LongNotchLen, 0}, RateOfClimbPH.name, {{"FLT_AltitudeHold"}})
-- rate of climb
local RateOfClimbMarkPH = addPlaceholder("RateOfClimbMarkPH", {0,0}, RateOfClimbPH.name, {{"DSPLS_HOVER_RateOfClimb", VertScale_h05}})
addRateOfClimb("RateOfClimb", {VertScaleNotch_margin - LongNotchLen, 0}, RateOfClimbMarkPH.name)
addText("RateOfClimbDIGITS", "0",	{-VertScaleNotch_margin - LongNotchLen - 30, 0}, "RightCenter", RateOfClimbMarkPH.name, {{"DSPLS_HOVER_RateOfClimbDigits"}})

local LOS_Reticle = add_LOS_Reticle("Hover", {0,0}, VideoAreaPH.name, {{"DSPLS_HOVER_LOS_Reticle"}})

local s = PitchLadder_h/16 + 20 
local CueingDot_U = addDot("CueingDot_U", { 0, s}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.UP}, {"DSPLS_HOVER_LOS_Reticle"}})
local CueingDot_D = addDot("CueingDot_D", { 0,-s}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.DOWN}, {"DSPLS_HOVER_LOS_Reticle"}})
local CueingDot_L = addDot("CueingDot_L", {-s, 0}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.LEFT}, {"DSPLS_HOVER_LOS_Reticle"}})
local CueingDot_R = addDot("CueingDot_R", { s, 0}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.RIGHT}, {"DSPLS_HOVER_LOS_Reticle"}})

local HeadTracker = addHeadTracker("HeadTracker", {0,0}, VideoAreaPH.name, {{"VIDEO_HOVER_HeadTracker", 0, 0.75*video_area_h_05, video_area_h_05, 1.5*video_area_h_05}})
local CuedLosReticle = addCuedLosReticle("CuedLosReticle", {0,0}, VideoAreaPH.name, {{"VIDEO_COMMON_CuedLosReticle", 0, 0.75*video_area_h_05, video_area_h_05, 1.5*video_area_h_05}})
local FixedGunAimingReticle = addCuedLosReticle("FixedGunAimingReticle", {0,0}, VideoAreaPH.name, {{"VIDEO_COMMON_FixedGunAimingReticle", 0, 0.75*video_area_h_05, video_area_h_05, 1.5*video_area_h_05}})

