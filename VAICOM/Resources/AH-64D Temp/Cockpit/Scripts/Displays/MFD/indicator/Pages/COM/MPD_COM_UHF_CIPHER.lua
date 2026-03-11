dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T3, "XPNDR",	tp_default_border, {{"MFD_COMMUNICATION_Menu", 2, pb.T3}}},
	{ pb.T4, "UHF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 3, pb.T4}}},
	{ pb.T5, "FM",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 4, pb.T5}}},
	{ pb.T6, "HF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 5, pb.T6}}},
	{ pb.B5, "SET",		tp_default_border},
}

local Controls = {}
Controls = 
{		
	{ "UHF CRYPTO",
				{
					{ pb.R1, "1",	nil, {{"MFD_COMM_UHF_CIPHER_Buttons", 0}}},
					{ pb.R2, "2",	nil, {{"MFD_COMM_UHF_CIPHER_Buttons", 1}}},	
					{ pb.R3, "3",	nil, {{"MFD_COMM_UHF_CIPHER_Buttons", 2}}},	
					{ pb.R4, "4",	nil, {{"MFD_COMM_UHF_CIPHER_Buttons", 3}}},	
					{ pb.R5, "5",	nil, {{"MFD_COMM_UHF_CIPHER_Buttons", 4}}},	
					{ pb.R6, "6",	nil, {{"MFD_COMM_UHF_CIPHER_Buttons", 5}}},	
				}
	},		     
	{ pb.B1, "", tp_default, {{"MFD_COMM_OriginatorFmt", pb.B1}}, {"COM", "DL", "XPNDR", "UHF", "FM", "HF", "COM"} }	
}

createMenu( Menu )
createControls( Controls )