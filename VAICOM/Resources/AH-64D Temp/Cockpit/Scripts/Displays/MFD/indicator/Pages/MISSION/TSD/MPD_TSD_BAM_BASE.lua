dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{
	{ pb.B3, "BAM",		tp_default_border,	nil},
	{ pb.B4, "MAP",		nil,	nil}
}

local Controls = {}
Controls = 
{
	{ pb.B1, "TSD",		nil,	nil},
	
	{ pb.B2, "CLR",	nil,	{{"TSD_BAM_CLR_Show",0}} },
	{ pb.B2, { {"PHASE", nil, {{"TSD_BAM_CLR_Show",1}}}, {"NAV", tp_default_border, {{"TSD_BAM_CLR_Show",1},{"TSD_PHASE_Btn_Caption"}}, {"NAV", "ATK"}} } },
	{ pb.B5, "XMIT\nBOTH",		tp_default_border,	{{"TSD_BAM_XmitBoth_Selection"}}},

	{ pb.T5, "L07",		tp_default_border,	{{"TSD_BAM_Subscribers_Show"},{"DSPLS_TSD_Subscriber_Caption",0},{"TSD_BAM_Subscriber_Selection",0}}},
	{ pb.T6, "L33",		tp_default_border,	{{"TSD_BAM_Subscribers_Show"},{"DSPLS_TSD_Subscriber_Caption",1},{"TSD_BAM_Subscriber_Selection",1}}},

	{ pb.R1, "L40",		tp_default_border,	{{"TSD_BAM_Subscribers_Show"},{"DSPLS_TSD_Subscriber_Caption",2},{"TSD_BAM_Subscriber_Selection",2}}},
	{ pb.R2, "L52",		tp_default_border,	{{"TSD_BAM_Subscribers_Show"},{"DSPLS_TSD_Subscriber_Caption",3},{"TSD_BAM_Subscriber_Selection",3}}},
	{ pb.R3, "L14",		tp_default_border,	{{"TSD_BAM_Subscribers_Show"},{"DSPLS_TSD_Subscriber_Caption",4},{"TSD_BAM_Subscriber_Selection",4}}},
	{ pb.R4, "L88",		tp_default_border,	{{"TSD_BAM_Subscribers_Show"},{"DSPLS_TSD_Subscriber_Caption",5},{"TSD_BAM_Subscriber_Selection",5}}},
	{ pb.R5, "L09",		tp_default_border,	{{"TSD_BAM_Subscribers_Show"},{"DSPLS_TSD_Subscriber_Caption",6},{"TSD_BAM_Subscriber_Selection",6}}},
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
AddTSDInfoStatusWindows()

if DBG_SA_WINDOWS_SHOW then
AddSAStatusWindowSmall()
AddSAStatusWindowLarge()
end

createMenu( Menu )
createControls( Controls )

AddSendBtn("BAM_SendMessageWindow", nil, {{"TSD_BAM_SEND_Show"}})

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.B3].pos = b3_pocket
pb_props[pb.B4].pos = b4_pocket

