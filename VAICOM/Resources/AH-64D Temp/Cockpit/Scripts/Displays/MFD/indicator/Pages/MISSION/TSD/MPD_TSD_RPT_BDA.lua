dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "TSD RPT PAGE - BDA",  {0, 350}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.L1, "BDA",		tp_default_border,	nil},
	{ pb.L4, "ALL",		tp_default_border,	{{"TSD_RPT_BDA_ModeSelection",1}}},
	{ pb.L5, "OWN",		tp_default_border,	{{"TSD_RPT_BDA_ModeSelection",0}}},
}

local Controls_Subscribers = {}
Controls_Subscribers = 
{
	{ pb.T5, "L07",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption",0},{"TSD_RPT_Subscriber_Selection",0}}},
	{ pb.T6, "L33",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption",1},{"TSD_RPT_Subscriber_Selection",1}}},

	{ pb.R1, "L40",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption",2},{"TSD_RPT_Subscriber_Selection",2}}},
	{ pb.R2, "L52",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption",3},{"TSD_RPT_Subscriber_Selection",3}}},
	{ pb.R3, "L14",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption",4},{"TSD_RPT_Subscriber_Selection",4}}},
	{ pb.R4, "L88",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption",5},{"TSD_RPT_Subscriber_Selection",5}}},
	{ pb.R5, "L09",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption",6},{"TSD_RPT_Subscriber_Selection",6}}},
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

AddCurrentHeadingLabel()

createControls( Controls )
createControls( Controls_Subscribers)

AddSendBtn("RPT_SendMessageWindow", nil, {{"TSD_RPT_SEND_Btn_Show"}})

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

