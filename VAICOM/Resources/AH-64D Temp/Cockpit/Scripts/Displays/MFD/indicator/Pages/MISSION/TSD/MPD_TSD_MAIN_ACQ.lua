dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.L2, "REC",		nil,					{{"TSD_REC_Button"}}},
	{ pb.L3, "JAM",		tp_default_border,		{{"TSD_JAM_Button_show"},{"TSD_JAM_Button_frame"}}},
	{ pb.L5, "HEADING",	tp_default_border,		{{"TSD_HDG_Button_caption"}}, {"","HDG","HEADING","HEADING","UPD"}},
	{ pb.L6, "POSITION",tp_default_border,		{{"TSD_SA_Button_caption"}}, {"SA","PSN","POSITION","POSITION","UPD"}},
}

pb_props[pb.L2].tp = createTextProperty( nil, "WHITE", IND_MPD_MATERIAL_WHITE, "LeftTop" )

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- 10
AddCurrentHeadingLabel()
AddNextWaypointStatusWindow(nil, {{"NAV_Heading_Valid"},{"TSD_Draw_If_PhaseNAV"},{"TSD_WPStatus_Window"}}, InfoWindowsBase.name)
AddEnduranceStatusWindow()
AddWindStatusWindow()
AddCursorLocationWindow()
AddPresentPositionWindow()
AddGridStatusLabel()

createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.L2].tp = tp_def_left