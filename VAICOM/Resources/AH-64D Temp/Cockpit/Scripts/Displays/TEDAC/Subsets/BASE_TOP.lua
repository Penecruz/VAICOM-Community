dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)

-- reset all masks
addMaskArea(DEFAULT_LEVEL,  h_clip_relations.REWRITE_LEVEL, "CursorArea", buildBoxVerts(1024, 1024,  "CenterCenter"), default_box_indices, {0,0})


-- main placeholder
local OppositeCursorPH = addPlaceholder("OppositeCursorPlaceholder", {0,0}, VideoAreaPH.name, {{"DSPLS_OppositeCursorShow"}, {"DSPLS_OppositeCursorPos"}})
local CursorPH = addPlaceholder("CursorPlaceholder", {0,0}, VideoAreaPH.name, {{"DSPLS_CursorShow"}, {"DSPLS_CursorPos"}})

local CURSOR_TYPE =
{
	DEFAULT					= 0,
	FCR_ZOOM_OPERATION		= 1,
	TARGET_REFERENCE_POINT	= 2,
	PANNING_OPERATION		= 3,
}

local function addSpecificCursor(type, parent, material, size, tex_pos, crosshair_material, crosshair_size, crosshair_tex_pos, black_disc_material, black_disc_size, black_disc_tex_pos)
	local tex_scale		= 2

	-- add placeholder for each type of symbol
	local SpecificCursorPH = addPlaceholder("SpecificCursorPlaceholder_"..type, {0,0}, parent, {{"DSPLS_CursorType", type}})
	local CursorSymbol = buildSymbol("SpecificCursorSymbol_"..type, material, {size,size}, {0,0}, "CenterCenter", tex_pos, tex_scale, SpecificCursorPH.name, nil)
	local CursorSymbol_black = Copy(CursorSymbol)
	CursorSymbol_black.material = "TEDAC_SYMBOLOGY_BLACK"
	Add(CursorSymbol_black)
	Add(CursorSymbol)

	if crosshair_material ~= nil then
		local CentralCrosshair = buildSymbol("SpecificCursorSymbolCrosshair_"..type, crosshair_material, {crosshair_size, crosshair_size}, {0,0}, "CenterCenter", crosshair_tex_pos, tex_scale, SpecificCursorPH.name, {{"DSPLS_CursorCrosshair"}})
		local CentralCrosshair_black = Copy(CentralCrosshair)
		CentralCrosshair_black.material = "TEDAC_SYMBOLOGY_BLACK"
		Add(CentralCrosshair_black)
		Add(CentralCrosshair)
	end

	if black_disc_material ~= nil then
		local CentralBlackDisc = buildSymbol("SpecificCursorSymbolBlackDisc_"..type, black_disc_material, {black_disc_size, black_disc_size}, {0,0}, "CenterCenter", black_disc_tex_pos, tex_scale, SpecificCursorPH.name, {{"DSPLS_CursorBlackDisc"}})
		local CentralBlackDisc_black = Copy(CentralBlackDisc)
		CentralBlackDisc_black.material = "TEDAC_SYMBOLOGY_BLACK"
		Add(CentralBlackDisc_black)
		Add(CentralBlackDisc)
	end
end

local function addOppositeSpecificCursor(type, parent, material, size, tex_pos, crosshair_material, crosshair_size, crosshair_tex_pos, black_disc_material, black_disc_size, black_disc_tex_pos)
	local tex_scale		= 2

	-- add placeholder for each type of symbol
	local SpecificCursorPH = addPlaceholder("OppositeSpecificCursorPlaceholder_"..type, {0,0}, parent, {{"DSPLS_OppositeCursorType", type}})
	local CursorSymbol = buildSymbol("OppositeSpecificCursorSymbol_"..type, material, {size,size}, {0,0}, "CenterCenter", tex_pos, tex_scale, SpecificCursorPH.name, nil)
	local CursorSymbol_black = Copy(CursorSymbol)
	CursorSymbol_black.material = "TEDAC_SYMBOLOGY_BLACK"
	Add(CursorSymbol_black)
	Add(CursorSymbol)

	if crosshair_material ~= nil then
		local CentralCrosshair = buildSymbol("OppositeSpecificCursorSymbolCrosshair_"..type, crosshair_material, {crosshair_size, crosshair_size}, {0,0}, "CenterCenter", crosshair_tex_pos, tex_scale, SpecificCursorPH.name, {{"DSPLS_OppositeCursorCrosshair"}})
		local CentralCrosshair_black = Copy(CentralCrosshair)
		CentralCrosshair_black.material = "TEDAC_SYMBOLOGY_BLACK"
		Add(CentralCrosshair_black)
		Add(CentralCrosshair)
	end

	if black_disc_material ~= nil then
		local CentralBlackDisc = buildSymbol("OppositeSpecificCursorSymbolBlackDisc_"..type, black_disc_material, {black_disc_size, black_disc_size}, {0,0}, "CenterCenter", black_disc_tex_pos, tex_scale, SpecificCursorPH.name, {{"DSPLS_OppositeCursorBlackDisc"}})
		local CentralBlackDisc_black = Copy(CentralBlackDisc)
		CentralBlackDisc_black.material = "TEDAC_SYMBOLOGY_BLACK"
		Add(CentralBlackDisc_black)
		Add(CentralBlackDisc)
	end
end

--[[
addOppositeSpecificCursor(CURSOR_TYPE.DEFAULT,				OppositeCursorPH.name, IND_MPD_TSD_SYMBOLS_GREEN, 80, {1922, 1408}, IND_MPD_TSD_SYMBOLS_WHITE, 30, {1919, 1535}, IND_MPD_TSD_SYMBOLS_BLACK_CLR, 50, {1919, 1630})
addOppositeSpecificCursor(CURSOR_TYPE.PANNING_OPERATION,	OppositeCursorPH.name, IND_MPD_TSD_SYMBOLS_GREEN, 80, {1920, 1151})
--]]
-- TODO: add other cursor types

addSpecificCursor(CURSOR_TYPE.DEFAULT,				CursorPH.name, "TEDAC_SYMBOLOGY", 54, {455, 424}, "TEDAC_SYMBOLOGY", 20, {377, 390}, "TEDAC_SYMBOLOGY_BLACK", 34, {363, 448})
addSpecificCursor(CURSOR_TYPE.FCR_ZOOM_OPERATION,	CursorPH.name, "TEDAC_SYMBOLOGY", 76, {213, 286})
--addSpecificCursor(CURSOR_TYPE.PANNING_OPERATION,	CursorPH.name, "TEDAC_SYMBOLOGY", 54, {1920, 1151})
-- TODO: add other cursor types

-- FCR
local GTM_ph = addPlaceholder("GTM PH", {0, 0}, VideoAreaPH.name, {{"DSPLS_FCR_GTM_Root"}})
local Base_PH = addPlaceholder("FCR_Targets_PH", nil, GTM_ph.name, {{"DSPLS_FCR_AG_TacticalPoint_SetPosition"}})
addMapTacticalSymbol("FCR Target Symbol", "font_TEDAC_FCR_Target_symbol", {{"DSPLS_FCR_AG_SetColorHighPriority", 1}, {"DSPLS_FCR_AG_SetSymbols"}}, Base_PH.name, {0, 0}, 30)

------------------------------
