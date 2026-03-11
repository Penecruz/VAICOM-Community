dofile(LockOn_Options.script_path.."Displays/DSPLS/DSPLS_Symbology.lua")

local Format = FORMAT.HOVER + FORMAT.BOB_UP + FORMAT.CRUISE + FORMAT.TRANSITION
local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0, 0}, nil, {{"VideoAreaPH"}, {"VideoSignal_Show"}})

local LOS_Reticle = add_HOVER_LOS_Reticle(nil, {0,0}, VideoAreaPH.name, {{"DSPLS_HOVER_LOS_Reticle"}} )

--
addStrokeText("SelectedSensorPNVS",		"FLIR",			STROKE_FNT_DFLT_100, "LeftBottom",	{-620, 560}, 	VideoAreaPH.name, {{"DSPLS_HOVER_SelectedSensorName"}}, {"", "TV", "FLIR", "1FLIR", "2FLIR"})
addStrokeText("EngineTorque",			"90%",			STROKE_FNT_DFLT_100, "LeftBottom",	{-620, 500}, 	VideoAreaPH.name, {{"DSPLS_HOVER_EngineTorque"}})
addRoundedBox("EngineTorqueBorder_w", {-630,480}, "LeftBottom",{150, 60}, 1, 1, VideoAreaPH.name, {{"DSPLS_HOVER_EngineTorqueBorder", 1}})
addRoundedBox("EngineTorqueBorder", {-630,480}, "LeftBottom",{113, 60}, 1, 1, VideoAreaPH.name, {{"DSPLS_HOVER_EngineTorqueBorder", 0}})
addStrokeText("TurbineGasTemperature",	"860C",			STROKE_FNT_DFLT_100, "LeftBottom",	{-620, 440}, 	VideoAreaPH.name, {{"DSPLS_HOVER_TurbineGasTemperature"}})
addStrokeText("Airspeed",				" 13",			STROKE_FNT_DFLT_100, "RightBottom",	{-570,  15},  	VideoAreaPH.name, {{"DSPLS_HOVER_Airspeed"}})
addStrokeText("G-Status",				"1.3G",			STROKE_FNT_DFLT_100, "RightBottom",	{-600, -70},  	VideoAreaPH.name, {{"DSPLS_HOVER_G_Status"}})
addStrokeText("VideoStatus",			"VIDEO DEGR",	STROKE_FNT_DFLT_100, "CenterBottom",{   0, 380},  	VideoAreaPH.name, {{"DSPLS_HOVER_VideoStatus"}}, {"", "VIDEO FROZEN", "VIDEO DEGR"})

addRoundedBox("AirspeedBorder", {-561,-4}, "RightBottom", {110, 60}, 1, 1, VideoAreaPH.name, {{"DSPLS_HOVER_AirspeedBorder"}})
addRoundedBox("AttitudeHold", {-550, -15}, "RightBottom", { 132, 82}, 10, 1, VideoAreaPH.name, {{"DSPLS_HOVER_AttitudeHold"}})

--local verts_circle	= buildEllipseVerts({0,0}, SkidSlipIndTop, SkidSlipIndTop)
--local circle = addStrokeLine( "first", verts_circle, nil, nil, VideoAreaPH.name)

-- Skid/Slip Ball
local FOR_w						= 476
local SkidSlipInd_h05			= FOR_w / 14
local SkidSlipInd_Gates_w		= SkidSlipInd_h
local SkidSlipInd_Gates_w05		= SkidSlipInd_Gates_w / 2
local SkidSlipInd_w				= FOR_w
local SkidSlipInd_w05			= SkidSlipInd_w / 2
local FOR_w05			= 200

local SkidSlipBallPH = addPlaceholder("SkidSlipBallPH", {0, SkidSlipIndBottom }, VideoAreaPH.name)

local SkidSlipBall_Mask = openMaskArea(0, "SkidSlipBall_Mask", buildBoxVerts(SkidSlipInd_w, SkidSlipInd_h, "CenterBottom"), default_box_indices, {0,0}, SkidSlipBallPH.name)
local SkidSlipBall = addStrokeSkidSlipBall("SkidSlipBall", {0, SkidSlipInd_h05}, SkidSlipBallPH.name, {{"DSPLS_HOVER_SkidSlipBall", SkidSlipInd_w05 - 0.7 * SkidSlipInd_h05}})

local SkidSlipBall_bottom  = addStrokeLine("SkidSlipBall_bottom", {{-FOR_w05-10, 0}, {FOR_w05+10, 0}}, THICKNESS_NARROW, FUZZINESS_NARROW, SkidSlipBallPH.name, nil)
local SkidSlipBall_Wall_l = addStrokeLine("SkidSlipBall_Wall_l", {{-SkidSlipInd_Gates_w05, 0}, {-SkidSlipInd_Gates_w05, SkidSlipInd_h}}, nil, nil, SkidSlipBallPH.name)
local SkidSlipBall_Wall_r =  addStrokeLine("SkidSlipBall_Wall_r", {{ SkidSlipInd_Gates_w05, 0}, { SkidSlipInd_Gates_w05, SkidSlipInd_h}}, nil, nil, SkidSlipBallPH.name)

setElemsClipLevel(SkidSlipBall_bottom, 1)
setElemsClipLevel(SkidSlipBall_Wall_l, 1)
setElemsClipLevel(SkidSlipBall_Wall_r, 1)
setElemsClipLevel(SkidSlipBall, 1)
resetMask(SkidSlipBall_Mask, 0, SkidSlipBallPH.name)

-- Velocity Vector
local dbg_vv_pos	= {0,0}
local vv_length_max	= video_area_h_05*0.8

addStrokeLine("VelocityVector_line", {{0,0}, {1,1}}, nil, nil, VideoAreaPH.name, {{"DSPLS_HOVER_VelocityVectorLine", vv_length_max}})
addStrokePoint("VelocityVector_point", dbg_vv_pos, VideoAreaPH.name, {{"DSPLS_HOVER_VelocityVectorDot", vv_length_max}})

-- Acceleration Cue
local ac_length_max = vv_length_max
addStrokeAccelerationCue("AccelerationCue", {0, 0}, VideoAreaPH.name, {{"DSPLS_HOVER_AccelerationCue", ac_length_max}})

-- Vertical Scale
local VertScale_h	= 800 
local VertScale_h05	= 400
local VerticalScalePH	= addPlaceholder("VerticalScalePH", {780, 45}, VideoAreaPH.name)
local AltitudeScalePH	= addPlaceholder("AltitudeScalePH", {0,0}, VerticalScalePH.name, {{"DSPLS_HOVER_AltitudeScaleShow"}})
local RateOfClimbPH		= addPlaceholder("RateOfClimbPH", {0,0}, VerticalScalePH.name, nil)

local VertScaleText_dx			= -90
local VertScaleText_dy			= 0 
local VertScaleText_line_space	= 112

addStrokeText("AltitudeDigits",	"0",	STROKE_FNT_DFLT_100, "RightCenter", {VertScaleText_dx, VertScaleText_dy},								VerticalScalePH.name, {{"DSPLS_HOVER_RadarAltitudeDigits"}})
addStrokeText("AltitudeHI",	"HI",		STROKE_FNT_DFLT_120, "RightCenter", {VertScaleText_dx - 30, VertScaleText_dy + VertScaleText_line_space},	VerticalScalePH.name, {{"DSPLS_HOVER_RadarAltitudeHI"}})
addStrokeText("AltitudeLO",	"LO",		STROKE_FNT_DFLT_120, "RightCenter", {VertScaleText_dx -30, VertScaleText_dy - VertScaleText_line_space},	VerticalScalePH.name, {{"DSPLS_HOVER_RadarAltitudeLO"}})

-- long notches
local VertScaleNotch_margin	= 11
local LongNotchLen			= 36

for i = -2,2 do
	local dh = VertScale_h / 4
	local h = dh * i
	addStrokeLine("AltitudeScale_LongNotch_"..i,	{{ VertScaleNotch_margin, h}, { VertScaleNotch_margin + LongNotchLen, h}}, nil, nil, AltitudeScalePH.name)
	if i == 0 then
		addStrokeLine("RateOfClimb_LongNotch_"..i,	{{VertScaleNotch_margin, h}, {-VertScaleNotch_margin - LongNotchLen, h}}, 7, nil, RateOfClimbPH.name)
	else
		addStrokeLine("RateOfClimb_LongNotch_"..i,	{{-VertScaleNotch_margin, h}, {-VertScaleNotch_margin - LongNotchLen, h}}, nil, nil, RateOfClimbPH.name)
	end
end

-- short notches
local ShortNotchLen			= 16
for i = 1,4 do
	local dh = VertScale_h / 20
	local h = dh * i

	local alt_h = -VertScale_h05 + h
	addStrokeLine("AltitudeScale_ShortNotch_"..i,	{{ VertScaleNotch_margin, alt_h}, { VertScaleNotch_margin + ShortNotchLen, alt_h}}, nil, nil, AltitudeScalePH.name)
	addStrokeLine("RateOfClimb_ShortNotch_Up_"..i,	{{-VertScaleNotch_margin-2, h}, {-VertScaleNotch_margin - ShortNotchLen, h}}, nil, nil, RateOfClimbPH.name)
	addStrokeLine("RateOfClimb_ShortNotch_Dn_"..i,	{{-VertScaleNotch_margin,-h}, {-VertScaleNotch_margin - ShortNotchLen,-h}}, nil, nil, RateOfClimbPH.name)
end
-- altitude tape
local AltitudeTape_w 	= VertScaleNotch_margin * 2.0 
local AltitudeTape_Mask = openMaskArea(0, "AltitudeTape_Mask", buildBoxVerts(AltitudeTape_w, VertScale_h, "CenterCenter"), default_box_indices, {0,-VertScale_h}, AltitudeScalePH.name, {{"DSPLS_HOVER_RadarAltitudeTape", VertScale_h}})
local AltitudeTape 		=  addStrokeLine(name, {{0,-VertScale_h05-3},{0,VertScale_h05+3}}, 20, nil, AltitudeScalePH.name)

setClipLevel(AltitudeTape, 1)
resetMask(AltitudeTape_Mask, 0, AltitudeScalePH.name)
-- altitude hold
addAltitudeHold("AltitudeHold", {5-VertScaleNotch_margin - LongNotchLen, 0}, RateOfClimbPH.name, {{"DSPLS_HOVER_AltitudeHold"}})
-- rate of climb
local RateOfClimbMarkPH = addPlaceholder("RateOfClimbMarkPH", {0,0}, RateOfClimbPH.name, {{"DSPLS_HOVER_RateOfClimb", VertScale_h05}})
addStrokeRateOfClimb("RateOfClimb", {-VertScaleNotch_margin - LongNotchLen - 15, 0}, RateOfClimbMarkPH.name)
addStrokeText("RateOfClimbDIGITS", "0",	 STROKE_FNT_DFLT_100, "RightCenter", {-VertScaleNotch_margin - LongNotchLen - 45, 0}, RateOfClimbMarkPH.name, {{"DSPLS_HOVER_RateOfClimbDigits"}})

local s = PitchLadder_h/16 + 20
local CueingDot_U = addStrokeDot("CueingDot_U", { 0, s}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.UP}, {"DSPLS_HOVER_LOS_Reticle"}})
local CueingDot_D = addStrokeDot("CueingDot_D", { 0,-s}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.DOWN},{"DSPLS_HOVER_LOS_Reticle"}})
local CueingDot_L = addStrokeDot("CueingDot_L", {-s, 0}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.LEFT},{"DSPLS_HOVER_LOS_Reticle"}})
local CueingDot_R = addStrokeDot("CueingDot_R", { s, 0}, nil, {{"DSPLS_COMMON_CueingDot", CUEING_DOT.RIGHT},{"DSPLS_HOVER_LOS_Reticle"}})

-- elements for safe zone

local HeadTracker = addHeadTracker("HeadTracker", {0,0}, VideoAreaPH.name, {{"HMD_HOVER_HeadTracker", video_area_w_05, video_area_h_05}})
local CuedLosReticle = addCuedLosReticle("CuedLosReticle", {0,0}, VideoAreaPH.name, {{"HMD_COMMON_CuedLosReticle", video_area_w_05, video_area_h_05}})
local FixedGunAimingReticle = addCuedLosReticle("FixedGunAimingReticle", {0,0}, VideoAreaPH.name, {{"HMD_COMMON_FixedGunAimingReticle", video_area_w_05, video_area_h_05}})
