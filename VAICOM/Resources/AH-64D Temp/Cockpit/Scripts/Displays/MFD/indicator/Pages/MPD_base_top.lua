dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-- reset all masks
addMaskArea(DEFAULT_LEVEL,  h_clip_relations.REWRITE_LEVEL, "CursorArea", buildBoxVerts(1024, 1024,  "CenterCenter"), default_box_indices, {0,0})


-- main placeholder
local OppositeCursorPH = addPlaceholder("OppositeCursorPlaceholder", {0,0}, nil, {{"DSPLS_OppositeCursorShow"}, {"DSPLS_OppositeCursorPos"}})
local CursorPH = addPlaceholder("CursorPlaceholder", {0,0}, nil, {{"DSPLS_CursorShow"}, {"DSPLS_CursorPos"}})

local CURSOR_TYPE =
{
	DEFAULT					= 0,
	FCR_ZOOM_OPERATION		= 1,
	TARGET_REFERENCE_POINT	= 2,
	PANNING_OPERATION		= 3,
}

local function addSpecificCursor(type, parent, material, size, tex_pos, crosshair_material, crosshair_size, crosshair_tex_pos, black_disc_material, black_disc_size, black_disc_tex_pos)
	local tex_size		= 2048
	local tex_scale		= 2

	-- add placeholder for each type of symbol
	local SpecificCursorPH = addPlaceholder("SpecificCursorPlaceholder_"..type, {0,0}, parent, {{"DSPLS_CursorType", type}})
	local CursorSymbol = buildSymbol("SpecificCursorSymbol_"..type, material, {size,size}, {0,0}, "CenterCenter", tex_size, tex_pos, tex_scale, SpecificCursorPH.name, nil)
	local CursorSymbol_black = Copy(CursorSymbol)
	CursorSymbol_black.material = IND_MPD_TSD_MATERIAL_BLACK
	Add(CursorSymbol_black)
	Add(CursorSymbol)

	if crosshair_material ~= nil then
		local CentralCrosshair = buildSymbol("SpecificCursorSymbolCrosshair_"..type, crosshair_material, {crosshair_size, crosshair_size}, {0,0}, "CenterCenter", tex_size, crosshair_tex_pos, tex_scale, SpecificCursorPH.name, {{"DSPLS_CursorCrosshair"}})
		local CentralCrosshair_black = Copy(CentralCrosshair)
		CentralCrosshair_black.material = IND_MPD_TSD_MATERIAL_BLACK
		Add(CentralCrosshair_black)
		Add(CentralCrosshair)
	end

	if black_disc_material ~= nil then
		local CentralBlackDisc = buildSymbol("SpecificCursorSymbolBlackDisc_"..type, black_disc_material, {black_disc_size, black_disc_size}, {0,0}, "CenterCenter", tex_size, black_disc_tex_pos, tex_scale, SpecificCursorPH.name, {{"DSPLS_CursorBlackDisc"}})
		local CentralBlackDisc_black = Copy(CentralBlackDisc)
		CentralBlackDisc_black.material = IND_MPD_TSD_MATERIAL_BLACK
		Add(CentralBlackDisc_black)
		Add(CentralBlackDisc)
	end
end

local function addOppositeSpecificCursor(type, parent, material, size, tex_pos, crosshair_material, crosshair_size, crosshair_tex_pos, black_disc_material, black_disc_size, black_disc_tex_pos)
	local tex_size		= 2048
	local tex_scale		= 2

	if type == CURSOR_TYPE.DEFAULT then
		tex_scale = 3
	end

	-- add placeholder for each type of symbol
	local SpecificCursorPH = addPlaceholder("OppositeSpecificCursorPlaceholder_"..type, {0,0}, parent, {{"DSPLS_OppositeCursorType", type}})
	local CursorSymbol = buildSymbol("OppositeSpecificCursorSymbol_"..type, material, {size,size}, {0,0}, "CenterCenter", tex_size, tex_pos, tex_scale, SpecificCursorPH.name, nil)
	local CursorSymbol_black = Copy(CursorSymbol)
	CursorSymbol_black.material = IND_MPD_TSD_MATERIAL_BLACK
	Add(CursorSymbol_black)
	Add(CursorSymbol)

	if crosshair_material ~= nil then
		local CentralCrosshair = buildSymbol("OppositeSpecificCursorSymbolCrosshair_"..type, crosshair_material, {crosshair_size, crosshair_size}, {0,0}, "CenterCenter", tex_size, crosshair_tex_pos, tex_scale, SpecificCursorPH.name, {{"DSPLS_OppositeCursorCrosshair"}})
		local CentralCrosshair_black = Copy(CentralCrosshair)
		CentralCrosshair_black.material = IND_MPD_TSD_MATERIAL_BLACK
		Add(CentralCrosshair_black)
		Add(CentralCrosshair)
	end

	if black_disc_material ~= nil then
		local CentralBlackDisc = buildSymbol("OppositeSpecificCursorSymbolBlackDisc_"..type, black_disc_material, {black_disc_size, black_disc_size}, {0,0}, "CenterCenter", tex_size, black_disc_tex_pos, tex_scale, SpecificCursorPH.name, {{"DSPLS_OppositeCursorBlackDisc"}})
		local CentralBlackDisc_black = Copy(CentralBlackDisc)
		CentralBlackDisc_black.material = IND_MPD_TSD_MATERIAL_BLACK
		Add(CentralBlackDisc_black)
		Add(CentralBlackDisc)
	end
end

-- TODO: more likely that there is only default opposite cursor shown - so others are not to implement. Maybe to change this addOppositeSpecificCursor() method.
addOppositeSpecificCursor(CURSOR_TYPE.DEFAULT,				OppositeCursorPH.name, IND_MPD_TSD_SYMBOLS_WHITE, 50, {1920, 1408}, IND_MPD_TSD_SYMBOLS_WHITE, 30, {1920, 1535})
addOppositeSpecificCursor(CURSOR_TYPE.PANNING_OPERATION,	OppositeCursorPH.name, IND_MPD_TSD_SYMBOLS_WHITE, 80, {1920, 1151})
-- TODO: add other cursor types

addSpecificCursor(CURSOR_TYPE.DEFAULT,				CursorPH.name, IND_MPD_TSD_SYMBOLS_GREEN, 80, {1920, 1408}, IND_MPD_TSD_SYMBOLS_WHITE, 30, {1920, 1535}, IND_MPD_TSD_SYMBOLS_BLACK_CLR, 50, {1920, 1630})
addSpecificCursor(CURSOR_TYPE.PANNING_OPERATION,	CursorPH.name, IND_MPD_TSD_SYMBOLS_GREEN, 80, {1920, 1151})
addSpecificCursor(CURSOR_TYPE.FCR_ZOOM_OPERATION,	CursorPH.name, IND_MPD_TSD_SYMBOLS_WHITE, 110, {1934, 1840})
AddTargetReferencePointCursor(CursorPH.name, {"DSPLS_CursorType", CURSOR_TYPE.TARGET_REFERENCE_POINT})
-- TODO: add other cursor types


-- over-cursor symbols
local	OverCursorSymbols_PH = addPlaceholder("OverCursorSymbols_PH", {0, 0}, nil, {{"TSD_OverCursorSymbols_Show"}})

-- WP
addWaypointSymbol("over_cursor", OverCursorSymbols_PH.name,
	{{"TSD_OverCursor_Waypoint_SetPosition"}},
	{{"TSD_OverCursor_Waypoint_SetType"}},
	{{"TSD_OverCursor_Waypoint_SetColor"}},
	{{"TSD_OverCursor_Waypoint_SetID"}}
)
addWaypointInverseID("over_cursor", OverCursorSymbols_PH.name, {"TSD_OverCursor_Waypoint_InverseID_Show"})

-- CM
addControlMeasureSymbol("over_cursor", OverCursorSymbols_PH.name,
	{{"TSD_OverCursor_ControlMeasure_SetPosition"}},
	{{"TSD_OverCursor_ControlMeasure_SetType"}},
	{{"TSD_OverCursor_ControlMeasure_SetColor"}},
	{{"TSD_OverCursor_ControlMeasure_SetText1"}},
	{{"TSD_OverCursor_ControlMeasure_SetText2"}}
)
addControlMeasureInverseID("over_cursor", OverCursorSymbols_PH.name, {"TSD_OverCursor_ControlMeasure_InverseID_Show"})

-- TT
addTargetOrThreatSymbol("over_cursor", OverCursorSymbols_PH.name,
	{{"TSD_OverCursor_TargetThreat_SetPosition"}},
	{{"TSD_OverCursor_TargetThreat_SetType"}},
	{{"TSD_OverCursor_TargetThreat_SetID"}}
)
addTargetOrThreatInverseID("over_cursor", OverCursorSymbols_PH.name, {"TSD_OverCursor_TargetThreat_InverseID_Show"})

-- TRN
AddTRNPointSymbol("PLT_OverCursor", "PLT", OverCursorSymbols_PH.name, {{"TSD_OverCursor_TRN_Cross_PLT_Show"},{"TSD_TRN_Cross_PLT_Position"}})
AddTRNPointSymbol("CPG_OverCursor", "CPG", OverCursorSymbols_PH.name, {{"TSD_OverCursor_TRN_Cross_CPG_Show"},{"TSD_TRN_Cross_CPG_Position"}})

-- FCR

local scaleSize = 30
local scaleSizeisRanked = 50

local Base_PH = addPlaceholder("FCR_Targets_PH", nil, nil, {{"DSPLS_FCR_AG_TacticalPoint_SetPosition"}})

addMapTacticalSymbol("FcrTargets", fontPrefix.."FCR_Target_Symbol", 			{{"DSPLS_FCR_AG_SetColorHighPriority", 0}, 		{"DSPLS_FCR_AG_SetSymbols"}}, Base_PH.name, {0, 0}, scaleSizeisRanked)
addMapTacticalSymbol("FcrTargetsNotRanked", fontPrefix.."FCR_Target_Symbol", 	{{"DSPLS_FCR_AG_SetColorLowPriority"}, 			{"DSPLS_FCR_AG_SetSymbols"}}, Base_PH.name, {0, 0}, scaleSize)
------------------------------
