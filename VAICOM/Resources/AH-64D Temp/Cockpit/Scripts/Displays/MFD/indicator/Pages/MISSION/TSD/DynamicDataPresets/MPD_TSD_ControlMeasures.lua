dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

local CMBase = addPlaceholder("CM_Dynamic_PH", {0, 0}, TacticalMapBase.name, {{"TSD_TacticalPoint_SetPosition"}})

addControlMeasureSymbol("Billet", CMBase.name,
	nil,
	{{"TSD_ControlMeasure_SetType"}},
	{{"TSD_ControlMeasure_SetColor"}},
	{{"TSD_ControlMeasure_SetText1"}},
	{{"TSD_ControlMeasure_SetText2"}}
)
