dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------
-- see MFD_types_AH64.h, enum MFD_MultipleSymbolsSets
DYNAMIC_DATA = 
{
	WAYPOINTS				= 0,
	CONTROL_MEASURES		= 1,
	TARGETS_THREATS			= 2,
	
	AIR_DEFENSE_RINGS		= 3,
	FCR						= 4,

	SHOT_AT_OWN_Above		= 5,
	SHOT_AT_OWN_Below		= 6,
	SHOT_AT_IDM				= 7,
	
	IDM_SUBSCRIBER			= 8,
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------
-- graphic should have strict order by layers,
-- see https://jira.dcs.dev/browse/AH64D-1627
-----------------------------------------------------------

-- ********** Map Layer **********
-- 2
AddGridLines()
AddOwnshipShadow()

-- 3
-- ********** TSD Line/Area Layer **********
-- Zones, Lines, Routeline, Direct-to Line
AddCurrentRouteLine()
AddDirectToLine()

-- 4
-- ********** TSD Point Layer **********
addPlaceholder("VIS_RINGS_Keeper", nil, TacticalMapBase.name, {{"MPD_updateMultipleSymbolsBuffer", DYNAMIC_DATA.AIR_DEFENSE_RINGS}})

-- Visibility rings for Ownship and Ghostship symbols
local OwnshipRingBase = addPlaceholder("OwnshipRingBase_PH", nil, TacticalMapBase.name, {{"TSD_OwnshipVisRing_Show"},{"TSD_OwnshipPos_Move"}})
AddIntervisibilityRing("Ownship_Vis_Ring", OwnshipRingBase.name, {{"TSD_OwnshipVisRing_Radius"}}, IND_MPD_TSD_SYMBOLS_CYAN)

local GhostshipRingBase = addPlaceholder("OwnshipRingBase_frozen_PH", {0, 0}, TacticalMapBase.name, {{"TSD_MapFrozenCue_Show"},{"TSD_GhostshipVisRing_Show"},{"TSD_MapFrozenCue_Ownship",-GetScale()*ScreenSize/5}})
AddIntervisibilityRing("Ghostship_Vis_Ring", GhostshipRingBase.name, {{"TSD_OwnshipVisRing_Radius"}}, IND_MPD_TSD_SYMBOLS_CYAN)

addPlaceholder("TSD_Points_KeeperCheater_ShotAtIdm",		nil, TacticalMapBase.name, {{"MPD_updateMultipleSymbolsBuffer", DYNAMIC_DATA.SHOT_AT_IDM}})
addPlaceholder("TSD_Points_KeeperCheater_WP",				nil, TacticalMapBase.name, {{"MPD_updateMultipleSymbolsBuffer", DYNAMIC_DATA.WAYPOINTS}})
addPlaceholder("TSD_Points_KeeperCheater_CM",				nil, TacticalMapBase.name, {{"MPD_updateMultipleSymbolsBuffer", DYNAMIC_DATA.CONTROL_MEASURES}})
addPlaceholder("TSD_Points_KeeperCheater_TT",				nil, TacticalMapBase.name, {{"MPD_updateMultipleSymbolsBuffer", DYNAMIC_DATA.TARGETS_THREATS}})

AddSpecialSymbols_TSD()	-- TRN symbols and others
AddUniqueTSDSymbols()
AddMapUnits_SA()

-- for selected tactical points (on POINT and RTE pages)
addWaypointInverseID("", "TSD_Points_KeeperCheater_WP", {"TSD_Waypoint_InverseID_Show"})
addControlMeasureInverseID("", "TSD_Points_KeeperCheater_CM", {"TSD_ControlMeasure_InverseID_Show"})
addTargetOrThreatInverseID("", "TSD_Points_KeeperCheater_TT", {"TSD_TargetThreat_InverseID_Show"})

-- 5
-- ********** FCR Symbology Layer **********
draw_PFZ_Set()
draw_NFZ_Set()

addPlaceholder("TSD_Points_KeeperCheater_ShotAtOwn_Below",	nil, TacticalMapBase.name, {{"MPD_updateMultipleSymbolsBuffer", DYNAMIC_DATA.SHOT_AT_OWN_Below}})
addPlaceholder("TSD_Points_KeeperCheater_FCR",				nil, TacticalMapBase.name, {{"MPD_updateMultipleSymbolsBuffer", DYNAMIC_DATA.FCR}})
addPlaceholder("TSD_Points_KeeperCheater_ShotAtOwn_Above",	nil, TacticalMapBase.name, {{"MPD_updateMultipleSymbolsBuffer", DYNAMIC_DATA.SHOT_AT_OWN_Above}})

NtsAntsSymbols()

-- 6
-- ********** IDM Subscriber Layer **********
addPlaceholder("TSD_Points_KeeperCheater_IDM_Subscribers",	nil, TacticalMapBase.name, {{"MPD_updateMultipleSymbolsBuffer", DYNAMIC_DATA.IDM_SUBSCRIBER}})

-- 7
-- ********** Ownship Sensor Layer **********
AddFcrCone()
AddTADSFootprintSymbol()
AddAseRect()


