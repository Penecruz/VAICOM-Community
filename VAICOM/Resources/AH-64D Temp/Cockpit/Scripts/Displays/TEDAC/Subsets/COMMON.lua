dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/Common/HAD_Messages.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)

local FOR_w				= 196
local FOR_h				= 64
local FOR_w05			= FOR_w / 2
local FOR_h05			= FOR_h / 2
local FOR_margin_bottom	= 8

----------------------------------------------------------------------------------------------------------------------------------
-- Heading Scale -----------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------
local HeadingScaleAreaPH = addPlaceholder("HeadingScaleAreaPH", {0, 290}, VideoAreaPH.name, {{"DSPLS_COMMON_HeadingShow"}})
addLine("LubberLine", {{0,0}, {0,-33}}, 10, HeadingScaleAreaPH.name, nil)


local HeadingScaleNotchSpace	= 19
local HeadingScaleNotchLen		= 14
local HeadingScaleNotchLen05	= HeadingScaleNotchLen / 2

addBigText("HeadingDigits", "000", {0,HeadingScaleNotchLen + 5}, "CenterBottom", HeadingScaleAreaPH.name, {{"DSPLS_COMMON_HeadingDigits"}})

-- open clipping areas
local HeadingScaleVisibleNotches	= 20
local HeadingScaleVisible_w			= HeadingScaleVisibleNotches * HeadingScaleNotchSpace
local HeadingScaleCurHdg_w			= 65
local HeadingScaleVisibleMarks_w	= (HeadingScaleVisible_w - HeadingScaleCurHdg_w) / 2
local HeadingScaleVisibleMarks_h	= 40
local HeadingScaleMarks_margin		= HeadingScaleCurHdg_w / 2
local HeadingScalePointers_h		= 35

local HeadingScaleMasksPH = addPlaceholder("HeadingScaleMasksPH", {0, 8}, HeadingScaleAreaPH.name, nil)

local HeadingScaleMarks_MaskLeft = openMaskArea(0, "HeadingScaleMarks_MaskLeft", buildBoxVerts(HeadingScaleVisibleMarks_w, HeadingScaleVisibleMarks_h, "RightBottom"), default_box_indices, {-HeadingScaleMarks_margin,0}, HeadingScaleMasksPH.name)
local HeadingScaleMarks_MaskRight = openMaskArea(0, "HeadingScaleMarks_MaskRight", buildBoxVerts(HeadingScaleVisibleMarks_w, HeadingScaleVisibleMarks_h, "LeftBottom"), default_box_indices, {HeadingScaleMarks_margin,0}, HeadingScaleMasksPH.name)
local HeadingScaleMarks_MaskBottom = openMaskArea(0, "HeadingScaleMarks_MaskBottom", buildBoxVerts(HeadingScaleVisible_w+20, HeadingScaleNotchLen + 2 + HeadingScalePointers_h, "CenterTop"), default_box_indices, {0,HeadingScaleNotchLen05 + 1}, HeadingScaleMasksPH.name)

-- notches and marks
local HeadingScalePH = addPlaceholder("HeadingScalePH", {0, 0}, HeadingScaleMasksPH.name, {{"DSPLS_COMMON_HeadingScale", HeadingScaleNotchSpace * 18}})

for i = -27,27 do
	local x				= HeadingScaleNotchSpace * i
	local notch_len025	= HeadingScaleNotchLen05 / 2
	local labels		= {"N", "3", "6", "E", "12", "15", "S", "21", "24", "W", "30", "33"}

	if i%3 == 0 then	-- long notch
		local line = addLine("HeadingScaleNotch_L"..i, {{x, -HeadingScaleNotchLen05},{x, HeadingScaleNotchLen05}}, 3, HeadingScalePH.name)
		setElemsClipLevel(line, 1)

		local index = i / 3 + 1
		if index <= 0 then
			index = index + 12
		end
		local mark = addText("HeadingNotchMark"..i, labels[index], {x, HeadingScaleNotchLen05 + 4}, "CenterBottom", HeadingScalePH.name)
		setElemsClipLevel(mark, 1)
	else				-- short notch
		local line = addLine("HeadingScaleNotch_S"..i, {{x, -notch_len025},{x, notch_len025}}, 3, HeadingScalePH.name)
		setElemsClipLevel(line, 1)
	end
end

-- pointers
local HeadingScalePointers_y = -HeadingScaleNotchLen05 - 3
local fcr_centerline_bearing = addPointer_FcrCenterlineBearing("FCR_CenterlineBearing", {0, HeadingScalePointers_y}, HeadingScaleMasksPH.name, {{"DSPLS_COMMON_FcrCenterlineBearing", HeadingScaleNotchSpace * 18}})
setElemsClipLevel(fcr_centerline_bearing, 1)

local alternate_sensor_bearing = addPointer_AlternateSensorBearing("AlternateSensorBearing", {0, HeadingScalePointers_y}, HeadingScaleMasksPH.name, {{"DSPLS_COMMON_AlternateSensorBearing", HeadingScaleNotchSpace * 18}})
setElemsClipLevel(alternate_sensor_bearing, 1)

local adf_bearing = addPointer_AdfBearing("ADF_Bearing", {0, HeadingScalePointers_y}, HeadingScaleMasksPH.name, {{"DSPLS_COMMON_AdfBearing", HeadingScaleNotchSpace * 18}})
setElemsClipLevel(adf_bearing, 1)

local bobup_heading = addPointer_BobUpHeading("CommonOrBobUpHeading", {0, HeadingScalePointers_y}, HeadingScaleMasksPH.name, {{"DSPLS_COMMON_CommonOrBobUpHeading", HeadingScaleNotchSpace * 18}})
setElemsClipLevel(bobup_heading, 1)

-- reset clipping areas
resetMask(HeadingScaleMarks_MaskLeft, 0, HeadingScaleMasksPH.name)
resetMask(HeadingScaleMarks_MaskRight, 0, HeadingScaleMasksPH.name)
resetMask(HeadingScaleMarks_MaskBottom, 0, HeadingScaleMasksPH.name)

----------------------------------------------------------------------------------------------------------------------------------
-- HAD (High Action Display) -----------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------
local HAD_margin_bottom	= 14
local HAD_y				= -video_area_h_05 + HAD_margin_bottom
local HAD_L2			= 27					-- line 2
local HAD_L3			= 65					-- line 3
local HAD_L4			= HAD_L3 + HAD_L2		-- line 4
local HAD_margin		= 20
local HAD_W1		= FOR_w05 + HAD_margin		-- width 1
local HAD_W2		= HAD_W1 + getTextWidth(6)	-- width 2

local HAD_PH = addPlaceholder("HAD_PH", {0, HAD_y}, VideoAreaPH.name, nil)

addText("SightStatus",				"SIGHT STATUS",	{-HAD_W1,	0},			"RightBottom",	HAD_PH.name, {{"DSPLS_COMMON_HAD_SightStatus"}}, HAD_SightStatusMsgs)
addText("RangeAndRangeSource",		"*XXXX",		{-HAD_W1,	HAD_L2},	"RightBottom",	HAD_PH.name, {{"DSPLS_COMMON_HAD_RangeAndRangeSource"}}, HAD_RangeAndRangeSourceStatusMsgs)
addText("WeaponControl",			"C GUN",		{ HAD_W1,	HAD_L2},	"LeftBottom",	HAD_PH.name, {{"DSPLS_COMMON_HAD_WeaponControl"}}, HAD_WeaponControlStatusMsgs)
addText("WeaponStatus",				"WEAPONSTATUS",	{ HAD_W1,	0},			"LeftBottom",	HAD_PH.name, {{"DSPLS_COMMON_HAD_WeaponStatus"}}, HAD_WeaponStatusMsgs)
addText("WeaponInhibit",			"WEAPON INHIB",	{ 0,		HAD_L3},	"CenterBottom",	HAD_PH.name, {{"DSPLS_COMMON_HAD_WeaponInhibit"}}, HAD_WeaponInhibitStatusMsgs)
addText("SightSelectStatus",		"SIGHTL",		{-HAD_W2,	HAD_L2},	"RightBottom",	HAD_PH.name, {{"DSPLS_COMMON_HAD_SightSelectStatus"}}, HAD_SightSelectStatusMsgs)
addText("OwnerCue",					"",				{ 0,		HAD_L4},	"CenterBottom",	HAD_PH.name, {{"DSPLS_COMMON_HAD_OwnerCue"}})
addText("AcquisitionSelectStatus",	"TADS",			{ HAD_W2,	HAD_L2},	"LeftBottom",	HAD_PH.name, {{"DSPLS_COMMON_HAD_AcquisitionSelectStatus"}}, HAD_AcquisitionStatusMsgs)


----------------------------------------------------------------------------------------------------------------------------------
-- Common Symbology --------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------
--local CuedLosReticle = addCuedLosReticle("CuedLosReticle", {0,0}, VideoAreaPH.name, {{"DSPLS_COMMON_CuedLosReticle", video_area_w_05, video_area_h_05}})



