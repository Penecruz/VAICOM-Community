dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.B3, "BAM",		nil,	nil},
	{ pb.B4, "MAP",		nil,	nil},
	{ pb.B5, "RTE",		nil,	nil},
	{ pb.B6, "POINT",	nil,	nil}
}

local Controls = {}
Controls = 
{
	{ pb.L2, "REC",		nil,					{{"TSD_REC_Button"}}},
	{ pb.L3, "JAM",		tp_default_border,		{{"TSD_JAM_Button_show"},{"TSD_JAM_Button_frame"}}},
	{ pb.L5, "HEADING",	tp_default_border,		{{"TSD_HDG_Button_caption"}}, {"","HDG","HEADING","HEADING","UPD"}},
	{ pb.L6, "POSITION",tp_default_border,		{{"TSD_SA_Button_caption"}}, {"SA","PSN","POSITION","POSITION","UPD"}},
	
	{ pb.R3, "CTR",		tp_default_border,	{{"TSD_CTR_Button_frame"}}},
	{ pb.R4, "FRZ",		tp_default_border,	{{"TSD_FRZ_Button_frame"}}},	
	{ pb.R5, "CAQ",		tp_default_border,	{{"TSD_CAQ_Button_frame"}}},
	
	{ pb.R6, { {"ACQ", nil, nil}, {"FXD", tp_default_border, {{"DSPLS_TSD_ACQ_Button_Caption"}}, {"PHS", "GHS", "SKR", "RFI", "FCR", "FXD", "TADS", "?00", "TRN", "ASE", "?PHS", "?GHS", "?SKR", "?RFI", "?FCR", "?FXD", "?TADS", "?00", "?TRN", "?ASE"}} } },
}

local pos_shift_x = 28
local b3_pocket,b4_pocket = pb_props[pb.B3].pos,pb_props[pb.B4].pos

pb_props[pb.B3].pos[1] = pb_props[pb.B3].pos[1] - pos_shift_x
pb_props[pb.B4].pos[1] = pb_props[pb.B4].pos[1] + pos_shift_x

pb_props[pb.L2].tp = createTextProperty( nil, "WHITE", IND_MPD_MATERIAL_WHITE, "LeftTop" )

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- 10
AddTSDInfoStatusWindows()

AddR1R2_MapRange_Arrows()

createMenu( Menu )
createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.L2].tp = tp_def_left

pb_props[pb.B3].pos = b3_pocket
pb_props[pb.B4].pos = b4_pocket

