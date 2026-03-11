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
	{ "NAV", 
				{ 
					{ pb.L1, 	"DOPPLER",			nil,				{{"DMS_NoBorder"}}},
					{ pb.L2, 	"INU 1",			nil,				{{"DMS_NoBorder"}}},
					{ pb.L3, 	"INU 2",			nil,				{{"DMS_NoBorder"}}},
					{ pb.L5, 	"RAD ALT",			nil,				{{"DMS_NoBorder"}}},
				}
	},
	
	{ "ASE", 
				{ 
					{ pb.R1, 	"RFI",				nil,				{{"DMS_NoBorder"}}},
					{ pb.R2, 	"RJAM",				nil,				{{"DMS_NoBorder"}}},
					{ pb.R3, 	"RLWR",				nil,				{{"DMS_NoBorder"}}},
					{ pb.R5, 	"PLT SWITCHES",		nil,				{{"DMS_NoBorder"}, {"DMS_PltCpg"}}, {"PLT SWITCHES", "CPG SWITCHES"}},
				}
	},
	
	{ "SUBSYSTEMS", 
				{ 
					{ pb.B2, 	"ACFT/\nCOMM",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B3, 	"CNTL/\nDSPL",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B4, 	"WPN/\nSIGHT",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B5, 	"PROC/\nDMS",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B6, 	"NAV/\nASE",		tp_default_border,	nil},
				}
	},
}

createControls( Controls )


