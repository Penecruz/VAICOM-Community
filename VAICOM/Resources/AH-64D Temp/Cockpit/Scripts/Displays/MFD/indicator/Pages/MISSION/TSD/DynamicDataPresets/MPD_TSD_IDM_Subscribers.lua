dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

local CMBase = addPlaceholder("IDM_SBCR_Dynamic_PH", {0, 0}, TacticalMapBase.name, {{"TSD_TacticalPoint_SetPosition"}})

addIdmSubscriberSymbol("Billet", CMBase.name,
	nil,
	{{"TSD_ControlMeasure_SetText1"}}
)
