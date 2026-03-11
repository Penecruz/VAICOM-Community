dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

--addText( "DMS IBIT PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T3, "IBIT",		tp_default_border },
	{ pb.B1, "DMS",			nil },
}

createMenu( Menu )


local Controls = {}
Controls = 
{	
	{ "FLT CNTRLS", 
				{ 
					{ pb.T4, 	"PRFLT",			nil,				{{"DMS_NoBorder"}}},
					{ pb.T5, 	"MAINT",			nil,				{{"DMS_NoBorder"}}},
				}
	},

	{ "ACFT", 
				{ 
					{ pb.L1, 	"ECS 1",			nil,				{{"DMS_NoBorder"}}},
					{ pb.L2, 	"ECS 2",			nil,				{{"DMS_NoBorder"}}},
					{ pb.L3, 	"EPMS 1",			nil,				{{"DMS_NoBorder"}}},
					{ pb.L4, 	"EPMS 2",			nil,				{{"DMS_NoBorder"}}},
					{ pb.L6, 	"PWR LEVER",		nil,				{{"DMS_NoBorder"}}},
				}
	},
	
	{ "COMM", 
				{ 
					{ pb.R1, 	"RADIOS",			nil,				{{"DMS_NoBorder"}}},
					{ pb.R2, 	"DL",				nil,				{{"DMS_NoBorder"}}},
					{ pb.R3, 	"XPNDR",			nil,				{{"DMS_NoBorder"}}},
					{ pb.R4, 	"CIU",				nil,				{{"DMS_NoBorder"}}},
					{ pb.R5, 	"PLT SWITCHES",		nil,				{{"DMS_NoBorder"}, {"DMS_PltCpg"}}, {"PLT SWITCHES", "CPG SWITCHES"}},
				}
	},
	
	{ "SUBSYSTEMS", 
				{ 
					{ pb.B2, 	"ACFT/\nCOMM",		tp_default_border,	nil},
					{ pb.B3, 	"CNTL/\nDSPL",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B4, 	"WPN/\nSIGHT",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B5, 	"PROC/\nDMS",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B6, 	"NAV/\nASE",		nil,				{{"DMS_NoBorder"}}},
				}
	},
}

createControls( Controls )

