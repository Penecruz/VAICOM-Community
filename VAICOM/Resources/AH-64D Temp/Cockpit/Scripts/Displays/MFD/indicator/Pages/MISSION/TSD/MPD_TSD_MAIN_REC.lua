dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------
local MSG_TYPE =
{
	"WPT/HZD",				-- 0
	"CTRLM",				-- 1
	"TGT/THRT",				-- 2

	"FCR TGT RPT",			-- 3
	"RFHO",					-- 4

	"PP REPORT",			-- 5
	"BDA REPORT",			-- 6
	"FARM REPORT",			-- 7

	"PP QUERY",				-- 8
	"BDA QUERY",			-- 9
	"FARM QUERY",			-- 10
	
	"PF ZONE",				-- 11
	"NF ZONE",				-- 12
	"PF/NF ZONE",			-- 13

	""					   -- 888
}

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
	{ pb.R3, "CTR",		tp_default_border,	{{"TSD_CTR_Button_frame"}}},
	{ pb.R4, "FRZ",		tp_default_border,	{{"TSD_FRZ_Button_frame"}}},	
	{ pb.R5, "CAQ",		tp_default_border,	{{"TSD_CAQ_Button_frame"}}},
	
	{ pb.R6, { {"ACQ", nil, nil}, {"FXD", tp_default_border, {{"DSPLS_TSD_ACQ_Button_Caption"}}, {"PHS", "GHS", "SKR", "RFI", "FCR", "FXD", "TADS", "?00", "TRN", "ASE", "?PHS", "?GHS", "?SKR", "?RFI", "?FCR", "?FXD", "?TADS", "?00", "?TRN", "?ASE"}} } },
	
	--{"RECEIVE",
	--{
		{pb.L2, {{"FARM REPORT", nil, {{"TSD_REC_MenuItem_type", 0}}, MSG_TYPE},	{"AHE12 L", nil, {{"TSD_REC_MenuItem_name", 0}}}}},
		{pb.L3, {{"FARM REPORT", nil, {{"TSD_REC_MenuItem_type", 1}}, MSG_TYPE},	{"AHE12 L", nil, {{"TSD_REC_MenuItem_name", 1}}}}},
		{pb.L4, {{"FARM REPORT", nil, {{"TSD_REC_MenuItem_type", 2}}, MSG_TYPE},	{"AHE12 L", nil, {{"TSD_REC_MenuItem_name", 2}}}}},
		{pb.L5, {{"FARM REPORT", nil, {{"TSD_REC_MenuItem_type", 3}}, MSG_TYPE},	{"AHE12 L", nil, {{"TSD_REC_MenuItem_name", 3}}}}},
		{pb.L6, "NONE", nil},
	--}
 	--},
}

local pos_shift_x = 28
local b3_pocket,b4_pocket = pb_props[pb.B3].pos,pb_props[pb.B4].pos

pb_props[pb.B3].pos[1] = pb_props[pb.B3].pos[1] - pos_shift_x
pb_props[pb.B4].pos[1] = pb_props[pb.B4].pos[1] + pos_shift_x

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- 10
AddTSDInfoStatusWindows()

AddR1R2_MapRange_Arrows()

draw_border_with_caption( pb_props[pb.L2].pos, pb_props[pb.L6].pos,  11, 1, "RECEIVE", pb_props[pb.L6].tp )

createMenu( Menu )
createControls( Controls, false )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.B3].pos = b3_pocket
pb_props[pb.B4].pos = b4_pocket

