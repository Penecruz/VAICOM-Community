dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)

--
local LOS_Reticle = add_HOVER_LOS_Reticle("PNVS_LOS_Reticle", {0,0}, VideoAreaPH.name, {{"DSPLS_HOVER_LOS_Reticle"}} )

--
addText("SelectedSensorPNVS",		"FLIR",			{-290, 217}, "LeftBottom", VideoAreaPH.name, {{"DSPLS_HOVER_SelectedSensorName"}}, {"", "TV", "FLIR", "1FLIR", "2FLIR"})
addText("EngineTorque",				"90%",			{-290, 190}, "LeftBottom", VideoAreaPH.name, {{"DSPLS_HOVER_EngineTorque"}})
addRoundedBox("torqueBorder_w", {-290, 190}, "LeftBottom", {70, 27}, 1, 4, VideoAreaPH.name, {{"DSPLS_HOVER_EngineTorqueBorder", 1}})
addRoundedBox("torqueBorder", {-290, 190}, "LeftBottom", {53, 27}, 1, 4, VideoAreaPH.name, {{"DSPLS_HOVER_EngineTorqueBorder", 0}})
addText("TurbineGasTemperature",	"860C",			{-290, 163}, "LeftBottom", VideoAreaPH.name, {{"DSPLS_HOVER_TurbineGasTemperature"}})
addText("Airspeed",					"188",			{-257,   7}, "RightBottom", VideoAreaPH.name, {{"DSPLS_HOVER_Airspeed"}})
addText("G-Status",					"1.3G",			{-257, -32}, "RightBottom", VideoAreaPH.name, {{"DSPLS_HOVER_G_Status"}})
addText("VideoStatus",				"VIDEO DEGR",	{   0, 170}, "CenterBottom", VideoAreaPH.name, {{"DSPLS_HOVER_VideoStatus"}}, {"", "VIDEO FROZEN", "VIDEO DEGR"})

addRoundedBox("AirspeedBorder", {-254, 6}, "RightBottom", {56, 27}, 1, 4, VideoAreaPH.name, {{"DSPLS_HOVER_AirspeedBorder"}})
addRoundedBox("AttitudeHold", {-247, 1}, "RightBottom", {70, 37}, 10, 4, VideoAreaPH.name, {{"DSPLS_HOVER_AttitudeHold"}})

-- Skid/Slip Ball
local SkidSlipInd_margin_bottom = 73
local SkidSlipInd_h				= 28
local SkidSlipInd_h05			= SkidSlipInd_h / 2
local SkidSlipInd_Gates_w		= 28
local SkidSlipInd_Gates_w05		= SkidSlipInd_Gates_w / 2
local SkidSlipInd_w				= SkidSlipInd_Gates_w * 7
local SkidSlipInd_w05			= SkidSlipInd_w / 2
local SkidSlipBallPH = addPlaceholder("SkidSlipBallPH", {0, -video_area_h_05 + SkidSlipInd_margin_bottom}, VideoAreaPH.name)

local FOR_w				= 196
local FOR_w05			= FOR_w / 2
addSimpleLine("FOR_up", {{-FOR_w05-7, 0}, {FOR_w05+7, 0}}, 5, "TEDAC_SYMBOLOGY", SkidSlipBallPH.name)

addLine("SkidSlipBall_Wall_l", {{-SkidSlipInd_Gates_w05, 0}, {-SkidSlipInd_Gates_w05, SkidSlipInd_h}}, 6, SkidSlipBallPH.name)
addLine("SkidSlipBall_Wall_r", {{ SkidSlipInd_Gates_w05, 0}, { SkidSlipInd_Gates_w05, SkidSlipInd_h}}, 6, SkidSlipBallPH.name)

local SkidSlipBall_Mask = openMaskArea(0, "SkidSlipBall_Mask", buildBoxVerts(SkidSlipInd_w + 7 + SkidSlipInd_h, SkidSlipInd_h, "CenterBottom"), default_box_indices, {0,0}, SkidSlipBallPH.name)
local SkidSlipBall = addSkidSlipBall("SkidSlipBall", {0, SkidSlipInd_h05}, SkidSlipBallPH.name, {{"DSPLS_HOVER_SkidSlipBall", SkidSlipInd_w05 + 6}})
setElemsClipLevel(SkidSlipBall, 1)
resetMask(SkidSlipBall_Mask, 0, SkidSlipBallPH.name)

-- Velocity Vector
local dbg_vv_pos	= {0,0}
local vv_length_max	= video_area_h_05 * 0.8
addLine("VelocityVector_line", {{0,0}, dbg_vv_pos}, 3, VideoAreaPH.name, {{"TEDAC_HOVER_VelocityVectorLine", vv_length_max}})
addPoint("VelocityVector_point", dbg_vv_pos, VideoAreaPH.name, {{"DSPLS_HOVER_VelocityVectorDot", vv_length_max}})

-- Acceleration Cue
local ac_length_max = vv_length_max
addAccelerationCue("AccelerationCue", {0, 0}, VideoAreaPH.name, {{"DSPLS_HOVER_AccelerationCue", ac_length_max}})

-- Vertical Scale
local VertScale_h	= 360
local VertScale_h05	= VertScale_h / 2
local VerticalScalePH = addPlaceholder("VerticalScalePH", {370, 20}, VideoAreaPH.name)
local AltitudeScalePH = addPlaceholder("AltitudeScalePH", {0,0}, VerticalScalePH.name, {{"DSPLS_HOVER_AltitudeScaleShow"}})
local RateOfClimbPH = addPlaceholder("RateOfClimbPH", {0,0}, VerticalScalePH.name, nil)
--
local VertScaleText_dx			= -45
local VertScaleText_dy			= -12
local VertScaleText_line_space	= 50
addText("AltitudeDigits",	"0",	{VertScaleText_dx, VertScaleText_dy},								"RightBottom", VerticalScalePH.name, {{"DSPLS_HOVER_RadarAltitudeDigits"}})
addBigText("AltitudeHI",	"HI",	{VertScaleText_dx, VertScaleText_dy + VertScaleText_line_space},	"RightBottom", VerticalScalePH.name, {{"DSPLS_HOVER_RadarAltitudeHI"}})
addBigText("AltitudeLO",	"LO",	{VertScaleText_dx, VertScaleText_dy - VertScaleText_line_space},	"RightBottom", VerticalScalePH.name, {{"DSPLS_HOVER_RadarAltitudeLO"}})
-- long notches
local VertScaleNotch_margin	= 6
local LongNotchLen			= 16
for i = -2,2 do
	local dh = VertScale_h / 4
	local h = dh * i
	addLine("AltitudeScale_LongNotch_"..i,	{{ VertScaleNotch_margin, h}, { VertScaleNotch_margin + LongNotchLen, h}}, 4, AltitudeScalePH.name)
	if i == 0 then
		addLine("RateOfClimb_LongNotch_"..i,	{{VertScaleNotch_margin, h}, {-VertScaleNotch_margin - LongNotchLen, h}}, 7, RateOfClimbPH.name)
	else
		addLine("RateOfClimb_LongNotch_"..i,	{{-VertScaleNotch_margin, h}, {-VertScaleNotch_margin - LongNotchLen, h}}, 4, RateOfClimbPH.name)
	end
end
-- short notches
local ShortNotchLen			= 7
for i = 1,4 do
	local dh = VertScale_h / 20
	local h = dh * i

	local alt_h = -VertScale_h05 + h
	addLine("AltitudeScale_ShortNotch_"..i,	{{ VertScaleNotch_margin, alt_h}, { VertScaleNotch_margin + ShortNotchLen, alt_h}}, 4, AltitudeScalePH.name)

	addLine("RateOfClimb_ShortNotch_Up_"..i, {{-VertScaleNotch_margin, h}, {-VertScaleNotch_margin - ShortNotchLen, h}}, 4, RateOfClimbPH.name)
	addLine("RateOfClimb_ShortNotch_Dn_"..i, {{-VertScaleNotch_margin,-h}, {-VertScaleNotch_margin - ShortNotchLen,-h}}, 4, RateOfClimbPH.name)
end
-- altitude tape
local AltitudeTape_w = VertScaleNotch_margin * 2 + 4
local AltitudeTape_Mask = openMaskArea(0, "AltitudeTape_Mask", buildBoxVerts(AltitudeTape_w, VertScale_h, "CenterCenter"), default_box_indices, {0,-VertScale_h}, AltitudeScalePH.name, {{"DSPLS_HOVER_RadarAltitudeTape", VertScale_h}})
local AltitudeTape = addSimpleLine("AltitudeTape", {{0,-VertScale_h05},{0,VertScale_h05}}, AltitudeTape_w, "TEDAC_SYMBOLOGY", AltitudeScalePH.name)
setClipLevel(AltitudeTape, 1)
resetMask(AltitudeTape_Mask, 0, AltitudeScalePH.name)
-- altitude hold
addAltitudeHold("AltitudeHold", {-VertScaleNotch_margin - LongNotchLen, 0}, RateOfClimbPH.name, {{"DSPLS_HOVER_AltitudeHold"}})
-- rate of climb
local RateOfClimbMarkPH = addPlaceholder("RateOfClimbMarkPH", {0,0}, RateOfClimbPH.name, {{"DSPLS_HOVER_RateOfClimb", VertScale_h05}})
addRateOfClimb("RateOfClimb", {-VertScaleNotch_margin - LongNotchLen, 0}, RateOfClimbMarkPH.name)
addText("RateOfClimbDIGITS", "0",	{-VertScaleNotch_margin - LongNotchLen - 20, 0}, "RightCenter", RateOfClimbMarkPH.name, {{"DSPLS_HOVER_RateOfClimbDigits"}})

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

local HeadTracker = addHeadTracker("HeadTracker", {0,0}, VideoAreaPH.name, {{"DSPLS_HOVER_HeadTracker", video_area_h_05, video_area_h_05}})
local FixedGunAimingReticle = addCuedLosReticle("FixedGunAimingReticle", {0,0}, VideoAreaPH.name, {{"DSPLS_COMMON_FixedGunAimingReticle", video_area_w_05, video_area_h_05}})

addcScopeSymbols(VideoAreaPH.name)
