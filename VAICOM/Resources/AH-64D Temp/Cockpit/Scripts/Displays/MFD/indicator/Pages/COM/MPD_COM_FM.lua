dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

local Menu = {}
Menu = 
{ 
--	{ pb.T2, "SOI",		tp_default_border, {{"MFD_COMMUNICATION_Menu", -1}}},
	{ pb.T3, "XPNDR",	tp_default_border, {{"MFD_COMMUNICATION_Menu", 2, pb.T3}}},
	{ pb.T4, "UHF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 3, pb.T4}}},
	{ pb.T5, "FM",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 4, pb.T5}}},
	{ pb.T6, "HF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 5, pb.T6}}},
	{ pb.B2, "ERF",		nil },
	{ pb.B5, "SET",		nil },
	{ pb.B6, "ZERO",	nil },
}

local Controls = {}
Controls = 
{		
	{ pb.R1, { {"HOPSET", nil, nil}, {"", tp_default_border, {{"MFD_COMM_FM_Buttons", 1}}} } },	
	{ pb.R2, { {"OFFSET", nil, nil}, {"", tp_default_border, {{"MFD_COMM_FM_Buttons", 2}}} } },	
	{ "CLOCK",
				{					
					{ pb.R4, {{"GPS TIME", nil, {{"MFD_COMM_FM_Buttons", 3}}}, {"",	nil }}, nil  },						
					{ pb.R5, {{"FM TIME",    nil, {{"MFD_COMM_FM_Buttons", 3}}}, {"",	nil }}, nil  },		
					{ pb.R6, {{"FM DATE/TIME>", nil, {{"MFD_COMM_FM_Buttons", 3}}}, {"",	nil, {{"MFD_COMM_FM_Buttons", 6}}} } },	
					
				}
	},			
	{ pb.L1, { {"RADIO",   nil, nil}, {"SC", tp_default_border,  {{"MFD_COMM_FM_Buttons", 9}}, {"FM1", "FM2"}} } },			
	{ pb.L2, { {"FM MODE", nil, nil}, {"FM1", tp_default_border, {{"MFD_COMM_FM_Buttons", 8}}, {"SC","FH/M", "FH"}} } },
	{ pb.B1, "", tp_default_border, {{"MFD_COMM_OriginatorFmt", pb.B1}}, {"COM", "DL", "XPNDR", "UHF", "FM", "HF", "COM"} }
}
local TuneControls = {}
TuneControls = 
{	
	{ "MAN",
				{
					{ pb.L3, {{"FREQ>",   nil, {{"MFD_COMM_FM_Buttons", 3}}}, {"",  nil, {{"MFD_COMM_FM_Buttons", 4}}} } },	
					{ pb.L4, {{"TUNE",    nil, {{"MFD_COMM_FM_Buttons", 3}}}, {"",	nil }}, nil  },	
				}
	},		
	{ "CUE",
				{
					{ pb.L5, {{"FREQ>",   nil, {{"MFD_COMM_FM_Buttons", 3}}}, {"",  nil, {{"MFD_COMM_FM_Buttons", 5}}} } },
					{ pb.L6, {{"TUNE",    nil, {{"MFD_COMM_FM_Buttons", 3}}}, {"",	nil }}, nil  },	
				}
	}			
}

createMenu( Menu )
createControls( Controls )
create_COMM_Controls( TuneControls )