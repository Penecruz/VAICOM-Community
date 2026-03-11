dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/Common/HAD_Messages.lua")


local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)


----------------------------------------------------------------------------------------------------------------------------------
-- Field of Regard (FOR) ---------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------
local FOR_w				= 196
local FOR_h				= 64
local FOR_w05			= FOR_w / 2
local FOR_h05			= FOR_h / 2
local FOR_margin_bottom	= 8

local FOR_PH = addPlaceholder("FOR_PH", {0, -video_area_h_05 + FOR_h05 + FOR_margin_bottom}, VideoAreaPH.name, nil)
--addRect("FOR_Box", FOR_w, FOR_h, {0,0}, "CenterCenter", 5, FOR_PH.name, nil)

-- FOR Notches
local function addNotch(name, pos, dir, parent, is_back)
	local FOR_NotchLen = 10

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

	local width = 3
	local line = {}

	if is_back == true then
		line = addSimpleBackLine(name.."_back", {p1, p2}, width + 2, "TEDAC_SYMBOLOGY_BLACK", parent, nil)
	else
		line = addSimpleLine(name, {p1, p2}, width, "TEDAC_SYMBOLOGY", parent, nil)
	end
end

local FOR_PNVS_ZERO_ELEV_H = FOR_h * (45 / 65)
local FOR_TADS_ZERO_ELEV_H = FOR_h * (60 / 90)
local FOR_TADS_90AZ_W = FOR_w05 * (90 / 120)

-- back black elements

local FOR_PNVS_PH_back = addPlaceholder("FOR_PNVS_PH_back", {0, 0}, FOR_PH.name, {{"DSPLS_COMMON_FOR_Type", 1}})
local FOR_TADS_PH_back = addPlaceholder("FOR_TADS_PH_back", {0, 0}, FOR_PH.name, {{"DSPLS_COMMON_FOR_Type", 2}})
-- FOR for PNVS
addNotch("FOR_PNVS_Notch_0AZ_1", {0,  FOR_h05}, "down", FOR_PNVS_PH_back.name, true)
addNotch("FOR_PNVS_Notch_0AZ_2", {0, -FOR_h05}, "up", FOR_PNVS_PH_back.name, true)
addNotch("FOR_PNVS_Notch_0_EL_1", {-FOR_w05, -FOR_h05 + FOR_PNVS_ZERO_ELEV_H}, "right", FOR_PNVS_PH_back.name, true)
addNotch("FOR_PNVS_Notch_0_EL_2", { FOR_w05, -FOR_h05 + FOR_PNVS_ZERO_ELEV_H}, "left", FOR_PNVS_PH_back.name, true)

-- FOR for TADS
addNotch("FOR_TADS_Notch_0AZ_1", {0,  FOR_h05}, "down", FOR_TADS_PH_back.name, true)
addNotch("FOR_TADS_Notch_0AZ_2", {0, -FOR_h05}, "up", FOR_TADS_PH_back.name, true)
addNotch("FOR_TADS_Notch_0EL_1", {-FOR_w05, -FOR_h05 + FOR_TADS_ZERO_ELEV_H}, "right", FOR_TADS_PH_back.name, true)
addNotch("FOR_TADS_Notch_0EL_2", { FOR_w05, -FOR_h05 + FOR_TADS_ZERO_ELEV_H}, "left", FOR_TADS_PH_back.name, true)
addNotch("FOR_TADS_Notch_l90EL_1", {-FOR_TADS_90AZ_W,  FOR_h05}, "down", FOR_TADS_PH_back.name, true)
addNotch("FOR_TADS_Notch_l90EL_2", {-FOR_TADS_90AZ_W, -FOR_h05}, "up", FOR_TADS_PH_back.name, true)
addNotch("FOR_TADS_Notch_r90EL_1", { FOR_TADS_90AZ_W,  FOR_h05}, "down", FOR_TADS_PH_back.name, true)
addNotch("FOR_TADS_Notch_r90EL_2", { FOR_TADS_90AZ_W, -FOR_h05}, "up", FOR_TADS_PH_back.name, true)

-- FOR box
local rect_verts = {{0, FOR_h05}, {-FOR_w05, FOR_h05}, {-FOR_w05, -FOR_h05},{FOR_w05, -FOR_h05},{FOR_w05, FOR_h05}, {0, FOR_h05}}
local for_box = addLine("FOR_BOX", rect_verts, 5, FOR_PH.name)

-- front white elements
-- addSimpleLine("VidSkidSlipBall_bottom", {{-FOR_w05 - 7, FOR_h05}, {FOR_w05 + 7, FOR_h05}}, 5, "TEDAC_SYMBOLOGY", FOR_PH.name)

local FOR_PNVS_PH = addPlaceholder("FOR_PNVS_PH", {0, 0}, FOR_PH.name, {{"DSPLS_COMMON_FOR_Type", 1}})
local FOR_TADS_PH = addPlaceholder("FOR_TADS_PH", {0, 0}, FOR_PH.name, {{"DSPLS_COMMON_FOR_Type", 2}})
-- FOR for PNVS
addNotch("FOR_PNVS_Notch_0AZ_1", {0,  FOR_h05}, "down", FOR_PNVS_PH.name, false)
addNotch("FOR_PNVS_Notch_0AZ_2", {0, -FOR_h05}, "up", FOR_PNVS_PH.name, false)
addNotch("FOR_PNVS_Notch_0_EL_1", {-FOR_w05, -FOR_h05 + FOR_PNVS_ZERO_ELEV_H}, "right", FOR_PNVS_PH.name, false)
addNotch("FOR_PNVS_Notch_0_EL_2", { FOR_w05, -FOR_h05 + FOR_PNVS_ZERO_ELEV_H}, "left", FOR_PNVS_PH.name, false)

-- FOR for TADS
addNotch("FOR_TADS_Notch_0AZ_1", {0,  FOR_h05}, "down", FOR_TADS_PH.name, false)
addNotch("FOR_TADS_Notch_0AZ_2", {0, -FOR_h05}, "up", FOR_TADS_PH.name, false)
addNotch("FOR_TADS_Notch_0EL_1", {-FOR_w05, -FOR_h05 + FOR_TADS_ZERO_ELEV_H}, "right", FOR_TADS_PH.name, false)
addNotch("FOR_TADS_Notch_0EL_2", { FOR_w05, -FOR_h05 + FOR_TADS_ZERO_ELEV_H}, "left", FOR_TADS_PH.name, false)
addNotch("FOR_TADS_Notch_l90EL_1", {-FOR_TADS_90AZ_W,  FOR_h05}, "down", FOR_TADS_PH.name, false)
addNotch("FOR_TADS_Notch_l90EL_2", {-FOR_TADS_90AZ_W, -FOR_h05}, "up", FOR_TADS_PH.name, false)
addNotch("FOR_TADS_Notch_r90EL_1", { FOR_TADS_90AZ_W,  FOR_h05}, "down", FOR_TADS_PH.name, false)
addNotch("FOR_TADS_Notch_r90EL_2", { FOR_TADS_90AZ_W, -FOR_h05}, "up", FOR_TADS_PH.name, false)

-- Opened mask for FOR
local FOR_Mask = openMaskArea(0, "FOR_Mask", buildBoxVerts(FOR_w+7, FOR_h+7, "CenterCenter"), default_box_indices, {0,0}, FOR_PH.name)

--
local FCR_CurrentCenterline = addFcrCurrentCenterline("FCR_CurrentCenterline", {0,0}, FOR_PH.name, {{"DSPLS_COMMON_FOR_FcrCurrentCenterline", FOR_w05}})
setElemsClipLevel(FCR_CurrentCenterline, 1)

local FCR_LastCenterline = addFcrLastCenterline("FCR_LastCenterline", {0,0}, FOR_PH.name, {{"DSPLS_COMMON_FOR_FcrLastCenterline", FOR_w05}})
setElemsClipLevel(FCR_LastCenterline, 1)

-- Close mask for FOR
resetMask(FOR_Mask, 0, FOR_PH.name)

-- FOV Box
local FOV_w	= 30
local FOV_h	= 21
local fov_box = addRect("FOV_Box", FOV_w, FOV_h, {0,0}, "CenterCenter", 5, FOR_PH.name, {{"DSPLS_COMMON_FOR_FovPos", FOR_w05, FOR_h05}})
--setElemsClipLevel(fov_box, 1)

local CuedLosDot = addDot("Cued_LOS_Dot", {0,0}, FOR_PH.name, {{"DSPLS_COMMON_FOR_CuedLosDotPos", FOR_w05, FOR_h05}})
--setElemsClipLevel(CuedLosDot, 1)