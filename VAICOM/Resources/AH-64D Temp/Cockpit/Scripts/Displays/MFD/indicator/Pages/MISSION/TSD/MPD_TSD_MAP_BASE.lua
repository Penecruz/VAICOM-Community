dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

--addText( "TSD MAP PAGE BASE",  {0, 50})

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

local Controls_Base = {}
Controls_Base = 
{
	{ pb.T4, "GRAY",			tp_default_border,	{{"TSD_MAP_GrayBtn_Show"},{"TSD_MAP_GRAY_frame"}}},
	{ pb.T5, "GRID",			tp_default_border,	{{"TSD_GRID_Btn_Frame"}}},
	{ pb.T6, "SLOPE\nSHADE",	tp_default_border,	{{"TSD_MAP_SlopeShadeBtn_frame"}}},
	
	{ pb.B1, "TSD",		nil,	nil},
	{ pb.B2, { {"PHASE", nil, nil}, {"NAV", tp_default_border, {{"TSD_PHASE_Btn_Caption"}}, {"NAV", "ATK"}} } },
}

local pos_shift_x = 28
local t3_pocket,t4_pocket,t5_pocket,t6_pocket = pb_props[pb.T3].pos,pb_props[pb.T4].pos,pb_props[pb.T5].pos,pb_props[pb.T6].pos
local b3_pocket,b4_pocket = pb_props[pb.B3].pos,pb_props[pb.B4].pos

pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - pos_shift_x*1.2
pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + pos_shift_x*1.0
pb_props[pb.T5].pos[1] = pb_props[pb.T5].pos[1] - pos_shift_x*0.2
pb_props[pb.T6].pos[1] = pb_props[pb.T6].pos[1] - pos_shift_x*1.0
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
AddTSDInfoStatusWindows()

if DBG_SA_WINDOWS_SHOW then
	AddSAStatusWindowSmall()
	AddSAStatusWindowLarge()
end

AddR1R2_MapRange_Arrows()

createMenu( Menu )
createControls( Controls_Base )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = t3_pocket
pb_props[pb.T4].pos = t4_pocket
pb_props[pb.T5].pos = t5_pocket
pb_props[pb.T6].pos = t6_pocket
pb_props[pb.B3].pos = b3_pocket
pb_props[pb.B4].pos = b4_pocket

