dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T1, "RPT",		tp_default_border,	nil},

	{ pb.B4, "ATHS",	nil,	nil},
	{ pb.B5, "ARTY",	nil,	nil},
	{ pb.B6, "AF",		nil,	nil}
}

local Controls_Base = {}
Controls_Base = 
{
	{ pb.B1, "TSD",			nil,	nil},
	{ pb.B2, { {"REPLY",	nil, {{"TSD_RPT_MSG_REPLY_Btn_Caption"}}, {"REPLY", "MSG"}},	{"SEND", tp_default_border, {{"TSD_RPT_MSG_REPLY_Btn_Selection"}},	{"AUTO","OFF","SEND","RQST"}} } },
}

local pos_shift_x = 28
local b3_pocket,b4_pocket = pb_props[pb.B3].pos,pb_props[pb.B4].pos

pb_props[pb.B3].pos[1] = pb_props[pb.B3].pos[1] - pos_shift_x
pb_props[pb.B4].pos[1] = pb_props[pb.B4].pos[1] + pos_shift_x

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- 8
-- ********** Ownship Sensor Layer **********
AddCompassRose()

-- 9
-- ********** Ownship Layer **********
AddFrozenOwnship()
AddOwnshipSymbol()

-- 10
-- ********** Info Windows and Menus **********
AddMapFrozenCue()
--AddCurrentHeadingLabel()
AddNextWaypointHeadingLabel()
AddNextWaypointStatusWindow(nil, {{"NAV_Heading_Valid"},{"TSD_Draw_If_PhaseNAV"},{"TSD_WPStatus_Window"}}, InfoWindowsBase.name)
AddEnduranceStatusWindow()
AddWindStatusWindow()
AddCursorLocationWindow()
AddPresentPositionWindow()
AddGridStatusLabel()

if DBG_SA_WINDOWS_SHOW then
AddSAStatusWindowSmall()
AddSAStatusWindowLarge()
end

createMenu( Menu )
createControls( Controls_Base )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.B3].pos			= b3_pocket
pb_props[pb.B4].pos			= b4_pocket

