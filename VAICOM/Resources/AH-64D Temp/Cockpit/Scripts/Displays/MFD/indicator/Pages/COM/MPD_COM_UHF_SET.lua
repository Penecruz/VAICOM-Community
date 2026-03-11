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
	{ "CRYPTO",
				{
					{ pb.R1,  {{"MODE",   nil, {{"MFD_COMM_UHF_SET_Buttons", 0}}}, {"RECEIVE", tp_default_border, {{"MFD_COMM_UHF_SET_Buttons", 1}}, {"PLAIN", "CIPHER", "RECEIVE"}} } },					
					{ pb.R2,  {{"CNV",   nil, {{"MFD_COMM_UHF_SET_Buttons", 0}}}, {"1", tp_default_border, {{"MFD_COMM_UHF_SET_Buttons", 2}}, {"1", "2", "3", "4", "5", "6"}} } },	
				}
	},	
	{ pb.B1, "", tp_default, {{"MFD_COMM_OriginatorFmt", pb.B1}}, {"COM", "DL", "XPNDR", "UHF", "FM", "HF", "COM"} }	
}

createMenu( Menu )
createControls( Controls )