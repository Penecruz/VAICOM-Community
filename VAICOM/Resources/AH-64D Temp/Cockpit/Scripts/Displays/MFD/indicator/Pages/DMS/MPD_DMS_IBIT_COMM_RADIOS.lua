dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

--addText( "DMS IBIT COMM RADIOS",  {0, 100})

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
	{ "RADIOS", 
				{ 
					{ pb.L1, 	"VHF",				nil,				{{"DMS_NoBorder"}}},
					{ pb.L2, 	"UHF",				nil,				{{"DMS_NoBorder"}}},
					{ pb.L3, 	"FM1",				nil,				{{"DMS_NoBorder"}}},
					{ pb.L4, 	"FM2",				nil,				{{"DMS_NoBorder"}}},
					{ pb.L5, 	"HF",				nil,				{{"DMS_NoBorder"}}},
				}
	},
	
					{ pb.R1, 	"RADIOS",			tp_default_border,				nil},
	
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

