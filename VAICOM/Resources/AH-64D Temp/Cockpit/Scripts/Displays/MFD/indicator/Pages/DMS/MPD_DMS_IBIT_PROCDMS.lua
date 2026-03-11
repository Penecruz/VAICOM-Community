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
	{ "PROCESSORS", 
				{ 
					{ pb.L1, 	"DP 1",				nil,				{{"DMS_NoBorder"}}},
					{ pb.L2, 	"DP 2",				nil,				{{"DMS_NoBorder"}}},
					{ pb.L3, 	"SP 1",				nil,				{{"DMS_NoBorder"}}},
					{ pb.L4, 	"SP 2",				nil,				{{"DMS_NoBorder"}}},
					{ pb.L5, 	"WP 1",				nil,				{{"DMS_NoBorder"}}},
					{ pb.L6, 	"WP 2",				nil,				{{"DMS_NoBorder"}}},
				}
	},
	
	{ "DMS", 
				{ 
					{ pb.R1, 	"DTU",				nil,				{{"DMS_NoBorder"}}},
					{ pb.R2, 	"MDR",				nil,				{{"DMS_NoBorder"}}},
				}
	},
	
	{ "SUBSYSTEMS", 
				{ 
					{ pb.B2, 	"ACFT/\nCOMM",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B3, 	"CNTL/\nDSPL",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B4, 	"WPN/\nSIGHT",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B5, 	"PROC/\nDMS",		tp_default_border,	nil},
					{ pb.B6, 	"NAV/\nASE",		nil,				{{"DMS_NoBorder"}}},
				}
	},
}

createControls( Controls )

