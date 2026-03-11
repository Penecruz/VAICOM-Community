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
	{ "WEAPONS", 
				{ 
					{ pb.L1, 	"GUN",				nil,				{{"DMS_NoBorder"}}},
					{ pb.L2, 	"HF MISSILES",		nil,				{{"DMS_NoBorder"}}},
					{ pb.L4, 	"ROCKETS",			nil,				{{"DMS_NoBorder"}}},
					{ pb.L5, 	"PLT SWITCHES",		nil,				{{"DMS_NoBorder"}, {"DMS_PltCpg"}}, {"PLT SWITCHES", "CPG SWITCHES"}},
				}
	},
	
					{ pb.L6, 	"TESS",				nil,				nil},
	
	{ "SIGHTS", 
				{ 
					{ pb.R1, 	"FCR",				nil,				{{"DMS_NoBorder"}}},
					{ pb.R2, 	"TADS CLASS A",		nil,				{{"DMS_NoBorder"}}},
					{ pb.R3, 	"PNVS CLASS A",		nil,				{{"DMS_NoBorder"}}},
					{ pb.R4, 	"IHADSS",			nil,				{{"DMS_NoBorder"}}},
					{ pb.R5, 	"PLT SWITCHES",		nil,				{{"DMS_NoBorder"}, {"DMS_PltCpg"}}, {"PLT SWITCHES", "CPG SWITCHES"}},
				}
	},
	
	{ "SUBSYSTEMS", 
				{ 
					{ pb.B2, 	"ACFT/\nCOMM",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B3, 	"CNTL/\nDSPL",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B4, 	"WPN/\nSIGHT",		tp_default_border,	nil},
					{ pb.B5, 	"PROC/\nDMS",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B6, 	"NAV/\nASE",		nil,				{{"DMS_NoBorder"}}},
				}
	},
}

createControls( Controls )

