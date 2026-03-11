dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

--addText( "TSD MAP PAGE FFD",  {0, 0})

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

	{ pb.L2, { {"TYPE", nil, nil}, {"DIG", tp_default_border, {{"TSD_MAP_TypeBtn_Caption"}}, {"DIG", "CHART", "SAT", "STICK"}} } },
	
	{ "FFD",
				{ 
					{ pb.L3, "AREAS",	nil, {{"TSD_MAP_FFD_Selection",0}} }, 
					{ pb.L4, "LINES",	nil, {{"TSD_MAP_FFD_Selection",1}} }, 
					{ pb.L5, "BOTH",	nil, {{"TSD_MAP_FFD_Selection",2}} },
					{ pb.L6, "NONE",	nil, {{"TSD_MAP_FFD_Selection",3}} }
				} 
	}	
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- draw menus
createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------


