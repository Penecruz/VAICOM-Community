dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

--addText( "TSD MAP PAGE TYPE",  {0, 0})

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.R3, "CTR",		tp_default_border,	{{"TSD_CTR_Button_frame"}}},
	{ pb.R4, "FRZ",		tp_default_border,	{{"TSD_FRZ_Button_frame"}}},	
	{ pb.R5, { {"ORIENT", nil, nil}, {"TRK-UP", tp_default_border, {{"TSD_MAP_Orient_Caption"}}, {"HDG-UP","TRK-UP","N-UP"}} } },
	{ pb.R6, { {"VIEW", nil, nil}, {"2D", tp_default_border, {{"TSD_MAP_View_Caption"}}, {"2D","3D"}} } },

	{ "TYPE",
			{ 
				{ pb.L2, "DIG",		nil, {{"TSD_MAP_TypeMenu_Selection",0}} }, 
				{ pb.L3, "CHART",	nil, {{"TSD_MAP_TypeMenu_Selection",1}} }, 
				{ pb.L4, "SAT",		nil, {{"TSD_MAP_TypeMenu_Selection",2}} },
				{ pb.L5, "STICK",	nil, {{"TSD_MAP_TypeMenu_Selection",3}} }
			} 
	},
	
	{ pb.L6, { {"FFD", nil, nil}, {"NONE", tp_default_border, {{"TSD_MAP_FFD_Caption"}}, {"AREAS","LINES","BOTH","NONE"}} } },
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- draw menus
createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

