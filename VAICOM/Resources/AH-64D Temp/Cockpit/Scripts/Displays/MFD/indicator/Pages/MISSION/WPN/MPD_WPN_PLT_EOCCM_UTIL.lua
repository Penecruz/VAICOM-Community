dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN UTIL PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local MFD_NUM = readParameter("MFD_NUM")

local EOCCM_FILTER_1	= 0
local EOCCM_CLEAR		= 1				
local EOCCM_FILTER_2	= 2	

local Menu = {}
Menu = 
{ 
	{ pb.T1, "CHAN",		nil },
	{ pb.T2, "ASE",			tp_default_border, 	{{"MFD_AsePrevBorder", 0}} },
	{ pb.T4, "CODE",		nil },
	{ pb.T5, "COORD",		nil },
	{ pb.T6, "UTIL",		tp_default_border },
	
	{ pb.B1, "WPN",			nil },
	{ pb.B6, "LOAD",		nil },
}

local CommonControls = {}
CommonControls = 
{
	{ pb.L1, "{IHADSS",		nil,	{{"WPN_UTIL_IHADSS_ON"}}, 			{"{IHADSS",	"}IHADSS"}	},	-- {on , }off
	{ pb.L2, "}RFI",		nil,	{{"WPN_UTIL_RFI_Enable"}},		 	{"{RFI",	"}RFI"}		},
	{ pb.L3, "}FCR",		tp_default_inv,	{{"WPN_UTIL_FCR_Enable", 1}},		 	{"{FCR",	"}FCR"}		},
	{ pb.L3, "}FCR",		nil,	{{"WPN_UTIL_FCR_Enable", 0}},		 			{"{FCR",	"}FCR"}		},

	{ pb.B2, "{GUN",		nil,	{{"WPN_UTIL_GUN_System_Enable"}}, 	{"{GUN",	"}GUN"}		},
	{ pb.B3, "{MSL",		nil,	{{"WPN_UTIL_MSL_System_Enable"}}, 	{"{MSL",	"}MSL"}		},
	{ pb.B5, "{RKT",		nil,	{{"WPN_UTIL_RKT_System_Enable"}}, 	{"{RKT",	"}RKT"}		},
	
	{ pb.R2, "LNCHR ARM",	nil, 			{{"WPN_UTIL_LNCHR_ARM_Inverse",0}} },
	{ pb.R2, "LNCHR ARM",	tp_default_inv, {{"WPN_UTIL_LNCHR_ARM_Inverse",1}} },

	{ pb.R3, "FCR STOW",	tp_default_border, {{"WPN_UTIL_FCR_STOW_Frame"}} },
	{ pb.R5, "GROUND STOW",	tp_default_border, {{"WPN_UTIL_GROUND_STOW_Frame"}} },
	
	{ pb.R6, { {"MMA", nil, nil}, {"PINNED", tp_default_border, {{"WPN_UTIL_MMA_Selection"}}, {"PINNED","NORM"}} } },

	{ "EOCCM", { 
						{ pb.L4, "FILTER 1",	nil, {{"WPN_UTIL_EOCCM", EOCCM_FILTER_1}}}, 
						{ pb.L5, "CLEAR",		nil, {{"WPN_UTIL_EOCCM", EOCCM_CLEAR}}}, 
						{ pb.L6, "FILTER 2",	nil, {{"WPN_UTIL_EOCCM", EOCCM_FILTER_2}}} 
				} 
	},
	{ pb.R1, "CUEING",	tp_default_border, {{"WPN_UTIL_CUEING_Frame"}} }
}
	
-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createMenu( Menu, nil, 1, TRANSPARENT_BACKGROUND )
createControls( CommonControls, nil, nil, 1, TRANSPARENT_BACKGROUND )
Add_UTIL_RTCA_StatusWindow()

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------
