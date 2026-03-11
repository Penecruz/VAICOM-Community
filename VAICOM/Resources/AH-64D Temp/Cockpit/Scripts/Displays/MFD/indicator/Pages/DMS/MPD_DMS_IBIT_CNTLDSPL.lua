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
	{ "PILOT", 
				{ 
					{ pb.L1, 	"KEYBOARD",			nil,				{{"DMS_NoBorder"}}},
					{ pb.L2, 	"EUFD",				nil,				{{"DMS_NoBorder"}}},
					{ pb.L3, 	"LEFT MPD",			nil,				{{"DMS_NoBorder"}}},
					{ pb.L4, 	"RIGHT MPD",		nil,				{{"DMS_NoBorder"}}},
					{ pb.L5, 	"SWITCHES",			nil,				{{"DMS_NoBorder"}}},
				}
	},
	
	{ "CPG", 
				{ 
					{ pb.R1, 	"KEYBOARD",			nil,				{{"DMS_NoBorder"}}},
					{ pb.R2, 	"EUFD",				nil,				{{"DMS_NoBorder"}}},
					{ pb.R3, 	"LEFT MPD",			nil,				{{"DMS_NoBorder"}}},
					{ pb.R4, 	"RIGHT MPD",		nil,				{{"DMS_NoBorder"}}},
					{ pb.R5, 	"SWITCHES",			nil,				{{"DMS_NoBorder"}}},
				}
	},
	
	{ "SUBSYSTEMS", 
				{ 
					{ pb.B2, 	"ACFT/\nCOMM",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B3, 	"CNTL/\nDSPL",		tp_default_border,	nil},
					{ pb.B4, 	"WPN/\nSIGHT",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B5, 	"PROC/\nDMS",		nil,				{{"DMS_NoBorder"}}},
					{ pb.B6, 	"NAV/\nASE",		nil,				{{"DMS_NoBorder"}}},
				}
	},
}

createControls( Controls )

