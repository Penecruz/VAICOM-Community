dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "TSD RPT PAGE - STAT",  {0, 350}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ "STAT",
				{ 
					{ pb.T2, "NONE",	tp_default_border, {{"TSD_RPT_STAT_Btn_Selection",0}} }, 
					{ pb.T3, "MTO",		tp_default_border, {{"TSD_RPT_STAT_Btn_Selection",1}} }, 
					{ pb.T4, "AFM",		tp_default_border, {{"TSD_RPT_STAT_Btn_Selection",2}} },
					{ pb.T5, "SPOT",	tp_default_border, {{"TSD_RPT_STAT_Btn_Selection",3}} },
					{ pb.T6, "SIT",		tp_default_border, {{"TSD_RPT_STAT_Btn_Selection",4}} }
				} 
	},
	
	{ pb.L1, "BDA",		nil,	nil},
	{ pb.L2, "TGT",		nil,	nil},
	{ pb.L3, "PP",		nil,	nil},
	{ pb.L4, "FARM",	nil,	nil},
	{ pb.L5, "SIT",		nil,	nil},
	{ pb.L6, "SPOT",	nil,	nil},
	
	{ pb.R3, "CTR",		tp_default_border,	{{"TSD_CTR_Button_frame"}}},
	{ pb.R4, "FRZ",		tp_default_border,	{{"TSD_FRZ_Button_frame"}}}
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

AddR1R2_MapRange_Arrows(nil)

createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

