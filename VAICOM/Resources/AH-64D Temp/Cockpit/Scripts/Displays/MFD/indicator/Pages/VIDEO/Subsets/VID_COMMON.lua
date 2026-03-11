dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/VIDEO/VIDEO_PAGE_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/Common/HAD_Messages.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0, 0}, nil, nil )

----------------------------------------------------------------------------------------------------------------------------------
-- Heading Scale -----------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------
local HeadingScaleAreaPH = addPlaceholder("HeadingScaleAreaPH", {0, 400}, VideoAreaPH.name, {{"DSPLS_COMMON_HeadingShow"}})
addLine("LubberLine", {{0,0}, {0,-49}}, 10, HeadingScaleAreaPH.name, nil)


local HeadingScaleNotchSpace	= 28
local HeadingScaleNotchLen		= 20
local HeadingScaleNotchLen05	= HeadingScaleNotchLen / 2

addBigText("HeadingDigits", "000", {0,HeadingScaleNotchLen + 7}, "CenterBottom", HeadingScaleAreaPH.name, {{"DSPLS_COMMON_HeadingDigits"}})

-- open clipping areas
local HeadingScaleVisibleNotches	= 20
local HeadingScaleVisible_w			= HeadingScaleVisibleNotches * HeadingScaleNotchSpace
local HeadingScaleCurHdg_w			= 96
local HeadingScaleVisibleMarks_w	= (HeadingScaleVisible_w - HeadingScaleCurHdg_w) / 2
local HeadingScaleVisibleMarks_h	= 59
local HeadingScaleMarks_margin		= HeadingScaleCurHdg_w / 2
local HeadingScalePointers_h		= 52

local HeadingScaleMasksPH = addPlaceholder("HeadingScaleMasksPH", {0, 12}, HeadingScaleAreaPH.name, nil)

local HeadingScaleMarks_MaskLeft = openMaskArea(0, "HeadingScaleMarks_MaskLeft", buildBoxVerts(HeadingScaleVisibleMarks_w, HeadingScaleVisibleMarks_h, "RightBottom"), default_box_indices, {-HeadingScaleMarks_margin,0}, HeadingScaleMasksPH.name)
local HeadingScaleMarks_MaskRight = openMaskArea(0, "HeadingScaleMarks_MaskRight", buildBoxVerts(HeadingScaleVisibleMarks_w, HeadingScaleVisibleMarks_h, "LeftBottom"), default_box_indices, {HeadingScaleMarks_margin,0}, HeadingScaleMasksPH.name)
local HeadingScaleMarks_MaskNotch = openMaskArea(0, "HeadingScaleMarks_MaskNotch", buildBoxVerts(HeadingScaleVisible_w, HeadingScaleNotchLen + 2, "CenterTop"), default_box_indices, {0, HeadingScaleNotchLen05 + 1}, HeadingScaleMasksPH.name)
local HeadingScaleMarks_MaskPointers = openMaskArea(0, "HeadingScaleMarks_MaskPointers", buildBoxVerts(HeadingScaleVisible_w*1.1, HeadingScalePointers_h, "CenterTop"), default_box_indices, {0, -HeadingScaleNotchLen05 -2}, HeadingScaleMasksPH.name)

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
local HeadingScalePointers_y = -HeadingScaleNotchLen05 - 4
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
resetMask(HeadingScaleMarks_MaskNotch, 0, HeadingScaleMasksPH.name)
resetMask(HeadingScaleMarks_MaskPointers, 0, HeadingScaleMasksPH.name)


----------------------------------------------------------------------------------------------------------------------------------
-- Field of Regard (FOR) ---------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
local FOV_w	= 44
local FOV_h	= 31


local FOR_PH = addPlaceholder("FOR_PH", {0, FOR_y + FOR_h05 + FOR_margin_bottom}, VideoAreaPH.name, nil)
addRect("FOR_Box", FOR_w, FOR_h, {0,0}, "CenterCenter", 5, FOR_PH.name, nil)

local rect_verts = {{0, FOR_h05}, {-FOR_w05, FOR_h05}, {-FOR_w05, -FOR_h05},{FOR_w05, -FOR_h05},{FOR_w05, FOR_h05}, {0, FOR_h05}}
addLine("VidFOR", rect_verts, 6, FOR_PH.name)

-- Opened mask for FOR
local FOR_Mask = openMaskArea(0, "FOR_Mask", buildBoxVerts(FOR_w + FOV_w*1.1, FOR_h + FOV_h*1.1, "CenterCenter"), default_box_indices, {0,0}, FOR_PH.name)

-- FOR Notches
local function addNotch(name, pos, dir, parent)
	local FOR_NotchLen = 12

	local x = pos[1]
	local y = pos[2]
	local l = FOR_NotchLen

	local p1 = {x, y}
	local p2 = {}

	if dir == "up" then
		p2 = {x, y + l}
	elseif dir == "down" then
		p2 = {x, y - l}
	elseif dir == "left" then
		p2 = {x - l, y}
	elseif dir == "right" then
		p2 = {x + l, y}
	end

	local line = addLine(name, {p1, p2}, 3, parent, nil)
	setElemsClipLevel(line, 1)
end

-- FOR for PNVS
local FOR_PNVS_PH = addPlaceholder("FOR_PNVS_PH", {0, 0}, FOR_PH.name, {{"VIDEO_COMMON_FOR_Type", 1}})
addNotch("FOR_PNVS_Notch_0AZ_1", {0,  FOR_h05}, "down", FOR_PNVS_PH.name)
addNotch("FOR_PNVS_Notch_0AZ_2", {0, -FOR_h05}, "up", FOR_PNVS_PH.name)
local FOR_PNVS_ZERO_ELEV_H = FOR_h * (45 / 65)
addNotch("FOR_PNVS_Notch_0_EL_1", {-FOR_w05, -FOR_h05 + FOR_PNVS_ZERO_ELEV_H}, "right", FOR_PNVS_PH.name)
addNotch("FOR_PNVS_Notch_0_EL_2", { FOR_w05, -FOR_h05 + FOR_PNVS_ZERO_ELEV_H}, "left", FOR_PNVS_PH.name)

-- FOR for TADS
local FOR_TADS_PH = addPlaceholder("FOR_TADS_PH", {0, 0}, FOR_PH.name, {{"VIDEO_COMMON_FOR_Type", 2}})
addNotch("FOR_TADS_Notch_0AZ_1", {0,  FOR_h05}, "down", FOR_TADS_PH.name)
addNotch("FOR_TADS_Notch_0AZ_2", {0, -FOR_h05}, "up", FOR_TADS_PH.name)
local FOR_TADS_ZERO_ELEV_H = FOR_h * (60 / 90)
addNotch("FOR_TADS_Notch_0EL_1", {-FOR_w05, -FOR_h05 + FOR_TADS_ZERO_ELEV_H}, "right", FOR_TADS_PH.name)
addNotch("FOR_TADS_Notch_0EL_2", { FOR_w05, -FOR_h05 + FOR_TADS_ZERO_ELEV_H}, "left", FOR_TADS_PH.name)
local FOR_TADS_90AZ_W = FOR_w05 * (90 / 120)
addNotch("FOR_TADS_Notch_l90EL_1", {-FOR_TADS_90AZ_W,  FOR_h05}, "down", FOR_TADS_PH.name)
addNotch("FOR_TADS_Notch_l90EL_2", {-FOR_TADS_90AZ_W, -FOR_h05}, "up", FOR_TADS_PH.name)
addNotch("FOR_TADS_Notch_r90EL_1", { FOR_TADS_90AZ_W,  FOR_h05}, "down", FOR_TADS_PH.name)
addNotch("FOR_TADS_Notch_r90EL_2", { FOR_TADS_90AZ_W, -FOR_h05}, "up", FOR_TADS_PH.name)

--
local FCR_CurrentCenterline = addFcrCurrentCenterline("FCR_CurrentCenterline", {0,0}, FOR_PH.name, {{"DSPLS_COMMON_FOR_FcrCurrentCenterline", FOR_w05}})
setElemsClipLevel(FCR_CurrentCenterline, 1)

local FCR_LastCenterline = addFcrLastCenterline("FCR_LastCenterline", {0,0}, FOR_PH.name, {{"DSPLS_COMMON_FOR_FcrLastCenterline", FOR_w05}})
setElemsClipLevel(FCR_LastCenterline, 1)

-- FOV Box
--local fov_box = addRect("FOV_Box", FOV_w, FOV_h, {0,0}, "CenterCenter", 5, FOR_PH.name, {{"DSPLS_COMMON_FOR_FovPos", FOR_w05-FOV_w/2, FOR_h05-FOV_h/2}})
local fov_box = addRect("FOV_Box", FOV_w, FOV_h, {0,0}, "CenterCenter", 5, FOR_PH.name, {{"DSPLS_COMMON_FOR_FovPos", FOR_w05, FOR_h05}})
setElemsClipLevel(fov_box, 1)

local CuedLosDot = addDot("Cued_LOS_Dot", {0,0}, FOR_PH.name, {{"DSPLS_COMMON_FOR_CuedLosDotPos", FOR_w05, FOR_h05}})
setElemsClipLevel(CuedLosDot, 1)

-- Close mask for FOR
resetMask(FOR_Mask, 0, FOR_PH.name)

----------------------------------------------------------------------------------------------------------------------------------
-- HAD (High Action Display) -----------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------
local HAD_margin_bottom	= 35
local HAD_y				= -video_area_h_05 + HAD_margin_bottom
local HAD_L1			= 10					-- line 1
local HAD_L2			= 55					-- line 2
local HAD_L3			= 129					-- line 3
local HAD_L4			= HAD_L3 + HAD_L2		-- line 4
local HAD_margin		= 30
local HAD_W1		= FOR_w05 + HAD_margin		-- width 1
local HAD_W2		= HAD_W1 + getTextWidth(6)	-- width 2

local HAD_PH = addPlaceholder("HAD_PH", {0, HAD_y}, VideoAreaPH.name, nil)

addText("SightStatus",				"",	{-HAD_W1, HAD_L1},	"RightBottom",	HAD_PH.name, {{"VIDEO_COMMON_HAD_SightStatus"}}, HAD_SightStatusMsgs)
addText("RangeAndRangeSource",		"",	{-HAD_W1, HAD_L2},	"RightBottom",	HAD_PH.name, {{"VIDEO_COMMON_HAD_RangeAndRangeSource"}}, HAD_RangeAndRangeSourceStatusMsgs)
addText("WeaponControl",			"",	{ HAD_W1, HAD_L2},	"LeftBottom",	HAD_PH.name, {{"VIDEO_COMMON_HAD_WeaponControl"}}, HAD_WeaponControlStatusMsgs)
addText("WeaponStatus",				"",	{ HAD_W1, HAD_L1},	"LeftBottom",	HAD_PH.name, {{"VIDEO_COMMON_HAD_WeaponStatus"}}, HAD_WeaponStatusMsgs)
addText("WeaponInhibit",			"",	{ 0,	  HAD_L3},	"CenterBottom",	HAD_PH.name, {{"VIDEO_COMMON_HAD_WeaponInhibit"}}, HAD_WeaponInhibitStatusMsgs)
addText("SightSelectStatus",		"",	{-HAD_W2, HAD_L2},	"RightBottom",	HAD_PH.name, {{"VIDEO_COMMON_HAD_SightSelectStatus"}}, HAD_SightSelectStatusMsgs)
addText("OwnerCue",					"",	{ 0,	  HAD_L4},	"CenterBottom",	HAD_PH.name, {{"VIDEO_COMMON_HAD_OwnerCue"}}, HAD_OwnerCueStatusMsgs)
addText("AcquisitionSelectStatus",	"",	{ HAD_W2, HAD_L2},	"LeftBottom",	HAD_PH.name, {{"VIDEO_COMMON_HAD_AcquisitionSelectStatus"}}, HAD_AcquisitionStatusMsgs)


----------------------------------------------------------------------------------------------------------------------------------
-- Common Symbology --------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------

