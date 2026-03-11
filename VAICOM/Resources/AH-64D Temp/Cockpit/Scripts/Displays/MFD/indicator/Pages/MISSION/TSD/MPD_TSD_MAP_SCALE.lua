dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

--addText( "TSD MAP PAGE SCALE",  {0, 0})

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.L2, { {"TYPE", nil, nil}, {"DIG", tp_default_border, {{"TSD_MAP_TypeBtn_Caption"}}, {"DIG", "CHART", "SAT", "STICK"}} } },
	
	{ "SCALE",
				{ 
					{ pb.L3, "1:5M",	nil, {{"TSD_MAP_Scale_Selection",0}} }, 
					{ pb.L4, "1:2M",	nil, {{"TSD_MAP_Scale_Selection",1}} }, 
					{ pb.L5, "1:1M",	nil, {{"TSD_MAP_Scale_Selection",2}} },
					{ pb.L6, "1:500K",	nil, {{"TSD_MAP_Scale_Selection",3}} }
				} 
	},
	{ "SCALE",
				{ 
					{ pb.R3, "1:250K",	nil, {{"TSD_MAP_Scale_Selection",4}} }, 
					{ pb.R4, "1:100K",	nil, {{"TSD_MAP_Scale_Selection",5}} }, 
					{ pb.R5, "1:50K",	nil, {{"TSD_MAP_Scale_Selection",6}} },
					{ pb.R6, "1:12.5K",	nil, {{"TSD_MAP_Scale_Selection",7}} }
				} 
	},
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- draw menus
createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------


