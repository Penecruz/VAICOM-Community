dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

local Base_PH = addPlaceholder("ADU_zones_PH", nil, nil, {{"TSD_TacticalPoint_SetPosition"}})

--local tp_cc 	= createTextProperty( 28,  "WHITE",	IND_MPD_MATERIAL_WHITE,  "CenterCenter" )
--addText("DBG TEMPLATE PRESET", {0, -50}, tp_cc, nil, nil, nil, "DBG_TMP_PRESET_Label", Base_PH.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)

AddIntervisibilityRing( "ADU_Circle_Find", Base_PH.name, {{"TSD_AirDefenseRings_RadiusFind"}}, IND_MPD_MATERIAL_YELLOW )
AddIntervisibilityRing( "ADU_Circle_Kill", Base_PH.name, {{"TSD_AirDefenseRings_RadiusKill"}}, IND_MPD_MATERIAL_RED )
