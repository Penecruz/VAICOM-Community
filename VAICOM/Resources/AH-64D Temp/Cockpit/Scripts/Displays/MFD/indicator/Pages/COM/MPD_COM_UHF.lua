dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
--	{ pb.T2, "SOI",		tp_default_border, {{"MFD_COMMUNICATION_Menu", -1}}},
	{ pb.T3, "XPNDR",	tp_default_border, {{"MFD_COMMUNICATION_Menu", 2, pb.T3}}},
	{ pb.T4, "UHF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 3, pb.T4}}},
	{ pb.T5, "FM",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 4, pb.T5}}},
	{ pb.T6, "HF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 5, pb.T6}}},
	{ pb.B3, "WOD",		nil },
	{ pb.B4, "FMT",		nil }, 
	{ pb.B5, "SET",		nil },
}

local Controls = {}
Controls = 
{		
	{ "CLOCK",
				{
					{ pb.R3, {{"COLD START", nil, {{"MFD_COMM_UHF_Buttons", 3}}}, {"",	nil }}, nil  },	
					{ pb.R4, {{"GPS TIME",   nil, {{"MFD_COMM_UHF_Buttons", 3}}}, {"",	nil }}, nil  },	
					{ pb.R5, {{"RECEIVE",    nil, {{"MFD_COMM_UHF_Buttons", 3}}}, {"",	nil }}, nil  },	
					{ pb.R6, {{"SEND",       nil, {{"MFD_COMM_UHF_Buttons", 3}}}, {"",	nil }}, nil  },	
				}
	},		
	{ pb.L1, {{"NET>",     nil, nil}, {"00.000", tp_default_border,  {{"MFD_COMM_UHF_Buttons", 1}}} } },		
	{ pb.L2, {{"UHF MODE", nil, nil}, {"SC", tp_default_border,  }}},	
--	{ pb.L2, {{"UHF MODE", nil, nil}, {"SC", tp_default_border,      {{"MFD_COMM_UHF_Buttons", 2}}, {"SC", "FH"}} } },	
	{ pb.L5, "TONE",	   nil,		  {{"MFD_COMM_UHF_Buttons", 4}}, {"}TONE", "{TONE"}},     
	{ pb.L6, "GUARD RECEIVER",	nil,  {{"MFD_COMM_UHF_Buttons", 5}}, {"{GUARD RECEIVER", "}GUARD RECEIVER"}},      
	{ pb.B1, "", tp_default_border, {{"MFD_COMM_OriginatorFmt", pb.B1}}, {"COM", "DL", "XPNDR", "UHF", "FM", "HF", "COM"} }	
}

createMenu( Menu )
createControls( Controls )