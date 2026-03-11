dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

local Base = addPlaceholder("ShotAt_Idm_Dynamic_PH", {0, 0}, TacticalMapBase.name, {{"TSD_ShotAt_Icon_SetPosition"}})

drawShotAtSymbol("ShotAt_Idm", {0,0}, Base.name, nil, nil, nil, IND_MPD_MATERIAL_DARK_GREEN)
