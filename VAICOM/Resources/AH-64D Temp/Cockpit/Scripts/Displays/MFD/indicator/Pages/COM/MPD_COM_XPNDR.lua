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
	{ "MODE",
				{
					{ pb.L1, "}1",		nil,	{{"MFD_COMM_XPNDR_MODE_Buttons", 1}}, {"}1", "{1"}}, 	
					{ pb.L2, "}2",		nil,	{{"MFD_COMM_XPNDR_MODE_Buttons", 2}}, {"}2", "{2"}},  
					{ pb.L3, "}3/A",	nil,	{{"MFD_COMM_XPNDR_MODE_Buttons", 3}}, {"}3/A", "{3/A"}},  
					{ pb.L4, "}C",		nil,	{{"MFD_COMM_XPNDR_MODE_Buttons", 4}}, {"}C", "{C"}},   					
					{ pb.L5, "}4",		nil,	{{"MFD_COMM_XPNDR_MODE_Buttons", 5}}, {"}4", "{4"}},   
					{ pb.L6, "}S",		nil,	{{"MFD_COMM_XPNDR_MODE_Buttons", 6}}, {"}S", "{S"}},   					
				}
	},		
	{ "CODE",
				{
					{ pb.R1, {{"MODE 1>",   nil, {{"MFD_COMM_XPNDR_CODE_Label", pb.R1 }} },	{"00",		nil, {{"MFD_COMM_XPNDR_CODE_Buttons",  pb.R1 }}} } },	
					{ pb.R2, {{"MODE 2>", 	nil, {{"MFD_COMM_XPNDR_CODE_Label", pb.R2 }} },	{"0000",	nil, {{"MFD_COMM_XPNDR_CODE_Buttons",  pb.R2 }}} } },
					{ pb.R3, {{"MODE 3/A>", nil, {{"MFD_COMM_XPNDR_CODE_Label", pb.R3 }} },	{"1200",	nil, {{"MFD_COMM_XPNDR_CODE_Buttons",  pb.R3 }}} } },
					{ pb.R4, {{"MODE 4>",   nil, {{"MFD_COMM_XPNDR_CODE_Label", pb.R4 }} },	{"A",		nil, {{"MFD_COMM_XPNDR_CODE_Buttons",  pb.R4}}, {"A", "B"}} } },
					{ pb.R5, {{"MODE S>",   nil, {{"MFD_COMM_XPNDR_CODE_Label", pb.R5 }}, {"MODE S>", "FLIGHT ID>", "A/C ADRSS"} },	{"?",		nil, {{"MFD_COMM_XPNDR_CODE_Buttons", pb.R5}}} } },
				}
	},		
	
	{ pb.R6, {{"XPNDR MASTER",	nil }, { "NORM", tp_default_border, {{"MFD_COMM_XPNDR_Buttons", 1}}, {"", "NORM", "STBY"} }}, nil},
	{ pb.B2, {{"ANTENNA",		nil }, { "DIV",  tp_default_border, {{"MFD_COMM_XPNDR_Buttons", 2}}, {"", "TOP", "DIV", "BOT" } }}, nil},	
	{ pb.B4, {{"REPLAY",		nil }, { "UDP&", tp_default_border, {{"MFD_COMM_XPNDR_Buttons", 3}}, {"", "UDP&", "UDP", "OFF"} }}, nil},
	{ pb.B5,   "IDENT",		tp_default, {{"MFD_COMM_XPNDR_Buttons", 4}}},	
	{ pb.B5,   "IDENT",		tp_default_inv, {{"MFD_COMM_XPNDR_Buttons", 5}}},
	{ pb.B1, "", tp_default_border, {{"MFD_COMM_OriginatorFmt", pb.B1}}, {"COM", "DL", "XPNDR", "UHF", "FM", "HF", "COM"} }
}

createMenu( Menu )
createControls( Controls )

AddRoundCornersWindow("Mode_S_Codes_Window",	{0.0, 0.0 },
							tp_default.width*24, tp_default.height*5.5,
							{
								{"MODE S CODES",		{0.0,							tp_default.height*4*0.5 - 0},		tp_center},
								{"FLIGHT ID:",			{-tp_default.width*22*0.5,		tp_default.height*4*0.5 - 60},		tp_def_left_center},
								{"A/S ADDRESS:",		{-tp_default.width*22*0.5,		tp_default.height*4*0.5 - 100},		tp_def_left_center},

								{"?",					{tp_default.width*4*0.5,		tp_default.height*4*0.5 - 60},		tp_def_left_center,	{{"MFD_COMM_XPNDR_CODE_Window", 1}}},
								{"?",					{tp_default.width*4*0.5,		tp_default.height*4*0.5 - 100},		tp_def_left_center,	{{"MFD_COMM_XPNDR_CODE_Window", 2}}},
							},
							tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)