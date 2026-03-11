dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{
	{ pb.T3, "SHOW",		nil,	nil},
	{ pb.T5, "COORD",		nil,	nil},
	{ pb.T6, "UTIL",		nil,	nil},

	{ pb.L1, "INST",		tp_default_border,	nil},

	{ pb.B3, "BAM",			nil,	nil},
	{ pb.B4, "MAP",			nil,	nil},
	{ pb.B5, "RTE",			nil,	nil},
	{ pb.B6, "POINT",		nil,	nil}
}

local TimerControls = {}
TimerControls = 
{
	{ pb.T1, "START",	nil,	{{"TSD_INST_Timer_caption"}}, {"START", "STOP"}},
	{ pb.T2, "RESET",	nil,	nil},
}

local Controls = {}
Controls = 
{
	{ pb.L2, { {"HDG>", nil, nil}, {"360", tp_default_border, { {"TSD_SelectedHeading_caption"},{"MFD_DataEntryButton_frame",pb.L2} }} } },
	{ pb.L3, { {"FREQ>", nil, nil}, {"?", tp_default_border,  { {"TSD_BeaconFreq_caption"},{"MFD_DataEntryButton_frame",pb.L3} }} } },
	
	{ pb.L5, "LAST\nFREQ",		nil,	nil},			--TODO: Label on 2 lines
	
	{ pb.B1, "TSD",		nil,	nil},
	{ pb.B2, { {"PHASE", nil, nil}, {"NAV", tp_default_border, {{"TSD_PHASE_Btn_Caption"}}, {"NAV", "ATK"}} } },	

	{ pb.R3, "CTR",			tp_default_border,	{{"TSD_CTR_Button_frame"}}},
	{ pb.R4, "TONE",		tp_default_border,	{{"TSD_INST_ADF_TONE_Frame"}}},
	{ pb.R5, "IDENT",		tp_default_border,	{{"TSD_INST_ADF_IDENT_Frame"}}},

	{ pb.R6, "TEST",		nil,			{{"TSD_ADF_Test_inProcess",0}}},
	{ pb.R6, "TEST",		tp_default_inv,	{{"TSD_ADF_Test_inProcess",1}}},
}

local pos_shift_x = 28
local t3_pocket,t4_pocket,b3_pocket,b4_pocket = pb_props[pb.T3].pos,pb_props[pb.T4].pos,pb_props[pb.B3].pos,pb_props[pb.B4].pos

pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - pos_shift_x*1.2
pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + pos_shift_x*0.8
pb_props[pb.B3].pos[1] = pb_props[pb.B3].pos[1] - pos_shift_x
pb_props[pb.B4].pos[1] = pb_props[pb.B4].pos[1] + pos_shift_x

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

if DBG_LABEL_SHOW then
addText( "TSD INST PAGE",  {0, 350}, tp_36_white)
end

-- 8
-- ********** Ownship Sensor Layer **********
AddCompassRose(true)

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

AddNDBStatusWindow()

AddR1R2_MapRange_Arrows()

createMenu( Menu )
createControls( Controls )

draw_border_with_caption( pb_props[pb.T1].pos, pb_props[pb.T2].pos,  5, 1, "TIMER", tp_def_top )	-- because common method of createControls draws frames around buttons captions
createControls( TimerControls,  nil, nil, 0)
AddTimerWindow()

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = t3_pocket
pb_props[pb.T4].pos = t4_pocket
pb_props[pb.B3].pos = b3_pocket
pb_props[pb.B4].pos = b4_pocket

