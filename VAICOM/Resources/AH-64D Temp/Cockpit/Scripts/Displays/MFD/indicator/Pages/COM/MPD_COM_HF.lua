dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
--	{ pb.T2, "SOI",		tp_default_border, {{"MFD_COMMUNICATION_Menu", -1}}},
	{ pb.T3, "XPNDR",	tp_default_border, {{"MFD_COMMUNICATION_Menu", 2, pb.T3}}},
	{ pb.T4, "UHF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 3, pb.T4}}},
	{ pb.T5, "FM",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 4, pb.T5}}},
	{ pb.T6, "HF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 5, pb.T6}}},
	{ pb.B5, "SET",		nil },
	{ pb.B6, "ZERO",	nil },
	{ pb.L6, "SELF ADRESS",	nil}, 	
}

local Controls = {}
Controls = 
{		
	{ "CLOCK",
				{					
					{ pb.R3, {{"GPS TIME",nil, {{"MFD_COMM_HF_Buttons", 3}}}, {"",	nil }}, nil  },	
					{ pb.R4, {{"HF TIME", nil, {{"MFD_COMM_HF_Buttons", 3}}}, {"",	nil }}, nil  },	
					{ pb.R5, {{"TIME>",   nil, {{"MFD_COMM_HF_Buttons", 3}}}, {"",  nil, {{"MFD_COMM_HF_Buttons", 4}}} } },	
					{ pb.R6, {{"DATE>",   nil, {{"MFD_COMM_HF_Buttons", 3}}}, {"",	nil, {{"MFD_COMM_HF_Buttons", 5}}} } },	
				}
	},			
	{ pb.L1, {{"HF STATE",  nil, nil}, {"T/R", tp_default_border, {{"MFD_COMM_HF_Buttons", 1}}, {"STBY", "SILENT", "T/R"}} } },	
	{ pb.L2, {{"HF MODE",   nil, nil}, {"MANUAL", tp_default_border, {{"MFD_COMM_HF_Buttons", 2}}, {"MANUAL", "PRESET", "ALE", "ECCM", "EMERGENCY"}} } },		
	{ pb.L5, {{"HF MODEM",  nil, nil}, {"D600 110S", tp_default_border, {{"MFD_COMM_HF_Buttons", 0}}} } },		
	{ pb.B3, {{"GND", 		nil },     {"ORIDE",	nil, nil }}, nil, nil},--TODO::
	{ pb.B1, "", tp_default_border, {{"MFD_COMM_OriginatorFmt", pb.B1}}, {"COM", "DL", "XPNDR", "UHF", "FM", "HF", "COM"} }
}

createMenu( Menu )
createControls( Controls )