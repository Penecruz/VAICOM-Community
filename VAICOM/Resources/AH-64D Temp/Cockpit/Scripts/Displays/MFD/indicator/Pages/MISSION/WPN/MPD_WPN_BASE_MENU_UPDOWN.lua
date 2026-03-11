dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T1, "CHAN",		nil },
	{ pb.T2, "ASE",			tp_default_border, 	{{"MFD_AsePrevBorder", 0}} },
	{ pb.T4, "CODE",		nil },
	{ pb.T5, "COORD",		nil },
	{ pb.T6, "UTIL",		nil },
}

local Controls = {}
Controls = 
{
	{ pb.B1, "WPN",		tp_default_border,	nil},

	{ pb.B2, "GUN",		tp_default_border,	{{"WPN_Armament_System_Chosen",0}}},
	{ pb.B3, "MSL",		tp_default_border,	{{"WPN_Armament_System_Chosen",1}}},
	{ pb.B5, "RKT",		tp_default_border,	{{"WPN_Armament_System_Chosen",2}}},
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createMenu( Menu, nil, 1, TRANSPARENT_BACKGROUND )
createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------