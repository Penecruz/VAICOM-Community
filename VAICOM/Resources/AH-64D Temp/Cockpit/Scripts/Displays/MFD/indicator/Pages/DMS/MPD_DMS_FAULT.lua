dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

--addText( "DMS FAULT PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T1, "DTU",			nil },
	{ pb.T2, "FAULT",		tp_default_border },
	{ pb.T3, "IBIT",		nil },
	{ pb.T4, {{"SHUT",	nil },{"DOWN",	nil }}},
	{ pb.T5, "VERS",		nil },
	{ pb.T6, "UTIL",		nil },
	{ pb.B1, "DMS",			nil },
	{ pb.B6, "WCA",			nil },
}

createMenu( Menu )

local Controls = {}
Controls = 
{	
	{ "DATA TYPE", 
				{ 
					{ pb.R1, 	"ACT",		tp_default_border,	{{"DMS_FAULT_Border", 1}}},
					{ pb.R2, 	"HST",		tp_default_border,	{{"DMS_FAULT_Border", 2}}},
					{ pb.R3, 	"EXC",		tp_default_border,	{{"DMS_FAULT_Border", 3}}},
					{ pb.R4, 	"LRU",		tp_default_border,	{{"DMS_FAULT_Border", 4}}},
				}
	},
}

createControls( Controls )