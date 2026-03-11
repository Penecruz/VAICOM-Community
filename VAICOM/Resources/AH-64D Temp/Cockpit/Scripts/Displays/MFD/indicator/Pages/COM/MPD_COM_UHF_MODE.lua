dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T3, "XPNDR",	tp_default_border, {{"MFD_COMMUNICATION_Menu", 2, pb.T3}}},
	{ pb.T4, "UHF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 3, pb.T4}}},
	{ pb.T5, "FM",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 4, pb.T5}}},
	{ pb.T6, "HF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 5, pb.T6}}},
	{ pb.B5, "SET",		tp_default_border },
}

local Controls = {}
Controls = 
{		
	{ "UHF CRYPTO",
				{
					{ pb.R1, "PLAIN",	nil, {{"MFD_COMM_UHF_MODE_Buttons", 0}}},	
					{ pb.R2, "CIPHER",	nil, {{"MFD_COMM_UHF_MODE_Buttons", 1}}},	
					{ pb.R3, "RECEIVE", nil, {{"MFD_COMM_UHF_MODE_Buttons", 2}}},	
				}
	},		     
	{ pb.B1, "", tp_default, {{"MFD_COMM_OriginatorFmt", pb.B1}}, {"COM", "DL", "XPNDR", "UHF", "FM", "HF", "COM"} }	
}

createMenu( Menu )
createControls( Controls )