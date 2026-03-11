dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

local WPBase = addPlaceholder("WP_Dynamic_PH", {0, 0}, TacticalMapBase.name, {{"TSD_TacticalPoint_SetPosition"}})

addWaypointSymbol("Billet", WPBase.name,
	nil,
	{{"TSD_Waypoint_SetType"}},
	{{"TSD_Waypoint_SetColor"}},
	{{"TSD_Waypoint_SetID"}})
