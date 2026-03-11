dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

local TTBase = addPlaceholder("TT_Dynamic_PH", {0, 0}, TacticalMapBase.name, {{"TSD_TacticalPoint_SetPosition"}})

addTargetOrThreatSymbol("Billet", TTBase.name,
	nil,
	{{"TSD_TargetThreat_SetType"}},
	{{"TSD_TargetThreat_SetID"}})

AddIntervisibilityRing( "TT_Circle_Find", TTBase.name, {{"TSD_AirDefenseRings_RadiusFind"}}, IND_MPD_MATERIAL_YELLOW )
AddIntervisibilityRing( "TT_Circle_Kill", TTBase.name, {{"TSD_AirDefenseRings_RadiusKill"}}, IND_MPD_MATERIAL_RED )
