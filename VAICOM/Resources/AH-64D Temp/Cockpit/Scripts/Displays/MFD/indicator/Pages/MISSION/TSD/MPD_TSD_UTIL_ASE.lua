dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ "ASE AUTOPAGE", { 
						{ pb.R1, "SEARCH",		nil, {{"TSD_UTIL_ASE_AUTOPAGE_Selection", 0}}	}, 
						{ pb.R2, "ACQUISITION",	nil, {{"TSD_UTIL_ASE_AUTOPAGE_Selection", 1}}	}, 
						{ pb.R3, "TRACK",		nil, {{"TSD_UTIL_ASE_AUTOPAGE_Selection", 2}}	}, 
						{ pb.R4, "OFF",			nil, {{"TSD_UTIL_ASE_AUTOPAGE_Selection", 3}}	},
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
