dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/VIDEO/VIDEO_PAGE_Symbology.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0, 0}, nil, nil)
addText("BarometricOrInertialAltitude",	"5200",		{460, 320}, "RightBottom", VideoAreaPH.name, {{"DSPLS_CRUISE_BaroOrInertAltitude"}})

-- Attitude Indicator
local AttitudeIndicatorPH = addPlaceholder("AttitudeIndicatorPH", {0, 0}, VideoAreaPH.name, {{"DSPLS_CRUISE_AttitudeIndicatorShow"}})
local BankAnglePH = addPlaceholder("BankAnglePH", {0, 0}, AttitudeIndicatorPH.name, {{"DSPLS_CRUISE_AttitudeIndicatorBank"}})

local PitchLadder_05 = PitchLadder_h/2
addBankAnglePointer("BankAngle", {0, PitchLadder_05}, BankAnglePH.name, nil)
local verts	= buildEllipseVerts({0,0}, PitchLadder_05, PitchLadder_05)
local indices =  buildEllipseIndices()

--addLine( "VID_CRUISE_PL", verts, 4, VideoAreaPH.name )

local PitchLadder_Mask = openMaskArea(0, "PitchLadder_Mask", verts, indices, {0,0}, BankAnglePH.name)
local pitch_ladder = addPitchLadder("PitchLadder", {0,0}, BankAnglePH.name, {{"DSPLS_CRUISE_AttitudeIndicatorPitchLadder", PitchLadder_h/4/math.rad(10)}})
--local pitch_ladder = addPitchLadder("PitchLadder", {0,0}, BankAnglePH.name, {{"DSPLS_CRUISE_AttitudeIndicatorPitchLadder", PitchLadder_h}})
setElemsClipLevel(pitch_ladder, 1)
resetMask(PitchLadder_Mask, 0, BankAnglePH.name)

local LOS_Reticle = add_LOS_Reticle("Cruise", {0,0}, VideoAreaPH.name, {{"DSPLS_HOVER_LOS_Reticle"}}, 8)
