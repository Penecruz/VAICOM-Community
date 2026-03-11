dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/FCR/MPD_FCR_Defs.lua")

local Base_PH = addPlaceholder("FCR_Targets_PH", nil, nil, {{"TSD_TacticalPoint_SetPosition"}})
addMapTacticalSymbol("FcrTarget", fontPrefix.."FCR_Target_Symbol", {{"TSD_FCR_SetSymbolHighPriorityTarget"}, {"TSD_FCR_SetMaterial"}, {"TSD_FCR_SetScaleTarget", 1}}, Base_PH.name, {0, 0}, 50)
addMapTacticalSymbol("FcrTargetLow", fontPrefix.."FCR_Target_Symbol", {{"TSD_FCR_SetSymbolLowPriorityTarget"}, {"TSD_FCR_SetMaterial"}, {"TSD_FCR_SetScaleTarget", 1}}, Base_PH.name, {0, 0}, 30)

addMapTacticalSymbol("FcrTarget_50", fontPrefix.."FCR_Target_Symbol", {{"TSD_FCR_SetSymbolHighPriorityTarget"}, {"TSD_FCR_SetMaterial"}, {"TSD_FCR_SetScaleTarget", 2}}, Base_PH.name, {0, 0}, 20)
addMapTacticalSymbol("FcrTargetLow_50", fontPrefix.."FCR_Target_Symbol", {{"TSD_FCR_SetSymbolLowPriorityTarget"}, {"TSD_FCR_SetMaterial"}, {"TSD_FCR_SetScaleTarget", 2}}, Base_PH.name, {0, 0}, 10)