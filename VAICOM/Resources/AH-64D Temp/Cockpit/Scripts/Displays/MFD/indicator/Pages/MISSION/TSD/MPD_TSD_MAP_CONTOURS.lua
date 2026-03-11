dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

--addText( "TSD MAP CONTOURS SUBPAGE",  {0, 0})

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T1, "RPT",		nil,	nil},

	{ pb.T2, "PAN",		tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.PAN}}},
	{ pb.T2, "PAN",		nil,	nil},

	{ pb.T3, "SHOW",	nil,	nil},

	{ pb.L1, "INST",	tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.INST}}},
	{ pb.L1, "INST",	nil,	nil},

	{ pb.B3, "BAM",		tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.BAM}}},
	{ pb.B3, "BAM",		nil,	nil},

	{ pb.B4, "MAP",		tp_default_border,	nil},

	{ pb.B5, "RTE",		tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.RTE}}},
	{ pb.B5, "RTE",		nil,	nil},

	{ pb.B6, "POINT",	tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.POINT}}},
	{ pb.B6, "POINT",	nil,	nil},
}

local Controls_msContours =
{
	{ pb.B1, "TSD",		nil,	nil},
	{ pb.B2, { {"PHASE", nil, nil}, {"NAV", tp_default_border, {{"TSD_PHASE_Btn_Caption"}}, {"NAV", "ATK"}} } },
	
	{ "CONTOURS",
				{ 
					{ pb.L4, "1000",	nil, {{"TSD_MAP_ContoursMenu_Selection",5}} }, 
					{ pb.L5, "500",		nil, {{"TSD_MAP_ContoursMenu_Selection",4}} },
					{ pb.L6, "200",		nil, {{"TSD_MAP_ContoursMenu_Selection",3}} }
				} 
	},
	{ "CONTOURS",
				{ 
					{ pb.R4, "100",		nil, {{"TSD_MAP_ContoursMenu_Selection",2}} }, 
					{ pb.R5, "50",		nil, {{"TSD_MAP_ContoursMenu_Selection",1}} },
					{ pb.R6, "NONE",	nil, {{"TSD_MAP_ContoursMenu_Selection",0}} }
				} 
	}	
}

local pos_shift_x = 28
local t3_pocket,b3_pocket,b4_pocket = pb_props[pb.T3].pos,pb_props[pb.B3].pos,pb_props[pb.B4].pos

pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - pos_shift_x*1.2
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
--AddTSDInfoStatusWindows()
AddCurrentHeadingLabel()
AddNextWaypointHeadingLabel()

if DBG_SA_WINDOWS_SHOW then
AddSAStatusWindowSmall()
AddSAStatusWindowLarge()
end

--AddR1R2_MapRange_Arrows()

createMenu( Menu )
createControls( Controls_msContours )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = t3_pocket
pb_props[pb.B3].pos = b3_pocket
pb_props[pb.B4].pos = b4_pocket

