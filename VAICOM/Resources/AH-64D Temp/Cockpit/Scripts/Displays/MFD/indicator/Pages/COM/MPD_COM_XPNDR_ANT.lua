dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
--	{ pb.T2, "SOI",		tp_default_border, {{"MFD_COMMUNICATION_Menu", -1}}},
	{ pb.T3, "XPNDR",	tp_default_border, {{"MFD_COMMUNICATION_Menu", 2, pb.T3}}},
	{ pb.T4, "UHF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 3, pb.T4}}},
	{ pb.T5, "FM",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 4, pb.T5}}},
	{ pb.T6, "HF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 5, pb.T6}}},
}
local Controls = {}
Controls = 
{		
	{ "ANTENNA",
				{
					{ pb.B2, "TOP",	   nil,		  {{"MFD_COMM_XPNDR_ANT_Buttons", 1}}}, 	
					{ pb.B3, "DIV",	   nil,		  {{"MFD_COMM_XPNDR_ANT_Buttons", 2}}},  
					{ pb.B4, "BOT",	   nil,		  {{"MFD_COMM_XPNDR_ANT_Buttons", 3}}},  				
				}
	},
	{ pb.B1, "", tp_default_border, {{"MFD_COMM_OriginatorFmt", pb.B1}}, {"COM", "DL", "XPNDR", "UHF", "FM", "HF", "COM"} }
}

createMenu( Menu )
createControls( Controls )