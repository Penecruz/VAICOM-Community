dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")

--local Format = FORMAT.CRUISE
local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)

addText("BarometricOrInertialAltitude",	"5200",		{292, 190}, "RightBottom", VideoAreaPH.name, {{"DSPLS_CRUISE_BaroOrInertAltitude"}})


-- Attitude Indicator
local AttitudeIndicatorPH = addPlaceholder("AttitudeIndicatorPH", {0, 0}, VideoAreaPH.name, {{"DSPLS_CRUISE_AttitudeIndicatorShow"}})
local BankAnglePH = addPlaceholder("BankAnglePH", {0, 0}, AttitudeIndicatorPH.name, {{"DSPLS_CRUISE_AttitudeIndicatorBank"}})
addBankAnglePointer("BankAngle", {0, 260}, BankAnglePH.name, nil)

local PitchLadder_05 = PitchLadder_h/2
local verts = buildEllipseVerts({0, 0}, PitchLadder_05, PitchLadder_05)
local indices =  buildEllipseIndices()
--addLine( "CRUISE_PL", verts, 4, VideoAreaPH.name )

local PitchLadder_Mask = openMaskArea(0, "PitchLadder_Mask", verts, indices, {0,0}, BankAnglePH.name)
local pitch_ladder = addPitchLadder("PitchLadder", {0,0}, BankAnglePH.name, {{"DSPLS_CRUISE_AttitudeIndicatorPitchLadder", PitchLadder_h/4/math.rad(10)}})
--local pitch_ladder = addPitchLadder("PitchLadder", {0,0}, BankAnglePH.name, {{"DSPLS_CRUISE_AttitudeIndicatorPitchLadder", PitchLadder_h}})
setElemsClipLevel(pitch_ladder, 1)
resetMask(PitchLadder_Mask, 0, BankAnglePH.name)

local LOS_Reticle = add_CRUISE_LOS_Reticle(nil, {0,0}, VideoAreaPH.name, {{"DSPLS_HOVER_LOS_Reticle" }})