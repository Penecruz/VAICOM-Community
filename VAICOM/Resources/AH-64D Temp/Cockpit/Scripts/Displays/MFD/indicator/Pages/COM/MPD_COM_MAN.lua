dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

local Menu = {}
Menu = 
{ 		
	{ pb.B1, "COM",		nil },
	{ pb.B2, "MAN",	tp_default_border,	nil},
	{ "SOI",
		{
			{ pb.T5, "AUTH",	nil, nil}, 
			{ pb.T6, "EXPND",	nil, nil}, 	
		}
	},	
}

local Controls = {}
Controls = 
{		
	{ pb.L1, { {"VHF FREQ>", nil, nil}, {"", tp_default_border, {{"MFD_COMM_MAN_Buttons", pb.L1}}} } },	
	{ pb.L2, { {"UHF FREQ>", nil, nil}, {"", tp_default_border, {{"MFD_COMM_MAN_Buttons", pb.L2}}} } },
	{ pb.L3, { {"FM1 FREQ>", nil, nil}, {"", tp_default_border, {{"MFD_COMM_MAN_Buttons", pb.L3}}} } },
	{ pb.L4, { {"FM2 FREQ>", nil, nil}, {"", tp_default_border, {{"MFD_COMM_MAN_Buttons", pb.L4}}} } },				
	{ pb.B3, {{"VHF", nil, nil}, {"NB", tp_default_border,      {{"MFD_COMM_MAN_Buttons", 12}}, {"NB", "WB"}} } },	
	{ pb.B4, {{"VHF", nil  },{ "TONE",	nil }}, nil  },			
	{ pb.B5, {{"UHF", nil, nil}, {"NB", tp_default_border,      {{"MFD_COMM_MAN_Buttons", 13}}, {"NB", "WB"}} } },	
	{ pb.B6, {{"UHF", nil  },{ "TONE",	nil }}, nil  },	
	{ "GUARD",
		{
			{ pb.T2, "VHF",	nil, {{"MFD_COMM_MAN_Buttons", 4}}}, 
			{ pb.T3, "UHF",	nil, {{"MFD_COMM_MAN_Buttons", 5}}}, 	
		}
	},	
}
local HF_Recv_Xmit_Controls = {}
HF_Recv_Xmit_Controls = 
{		
	
	{ "HF RECV",
				{
					{ pb.R1, {{"FREQ>",    nil, {{"MFD_COMM_MAN_Buttons", 6}}}, {"",    nil, {{"MFD_COMM_MAN_Buttons", 10}}} } },	
					{ pb.R2, {{"EMISSION", nil, {{"MFD_COMM_MAN_Buttons", 7}}}, {"USB", tp_default_border,      {{"MFD_COMM_MAN_Buttons", 14}}, {"LSB", "USB", "CW", "AME"}} } },	
				}
	},		
	{ "HF XMIT",
				{
					{ pb.R3, { {"FREQ>",   nil, {{"MFD_COMM_MAN_Buttons", 8}}}, {"",    nil, {{"MFD_COMM_MAN_Buttons", 11}}} } },	
					{ pb.R4, {{"EMISSION", nil, {{"MFD_COMM_MAN_Buttons", 9}}}, {"USB", tp_default_border,      {{"MFD_COMM_MAN_Buttons", 15}}, {"LSB", "USB", "CW", "AME"}} } },	
				}
	}	
}

createMenu( Menu )
createControls( Controls )
create_COMM_Controls( HF_Recv_Xmit_Controls )