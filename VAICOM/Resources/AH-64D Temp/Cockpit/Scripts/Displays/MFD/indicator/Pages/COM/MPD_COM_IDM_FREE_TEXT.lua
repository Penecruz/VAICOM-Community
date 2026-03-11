dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	
	{ pb.T6, "TEXT",	tp_default_border,	nil },	
	{ "MSG",
		{
			{ pb.B5, "REC", nil },
			{ pb.B6, "SEND", tp_default_border, {{"MPD_COM_BTN_BORDER", pb.B6}} },
		}
	},
	{ pb.B1, "COM",		nil }
}
createMenu( Menu )

local pos_left_border  = {pb_props[pb.T3].pos[1]+15,pb_props[pb.T3].pos[2]+15 }	
local pos_right_border = {pb_props[pb.T4].pos[1]+5, pb_props[pb.T4].pos[2]+15 } 
draw_border_with_caption( pos_left_border, pos_right_border, 7, 1, "TEXT MSG",  tp_default, nil, false )

Controls = 
{	
	{ pb.T1,  
			{ 
				{"SOURCE",	nil}, 
				{"DL", tp_default_border, nil} 
			}
	},
	{ pb.T3, "MPS",	 tp_default_border, {{"MPD_COM_IDM_TEXT_MSG", 0}} },
	{ pb.T4, "FREE", tp_default_border, {{"MPD_COM_IDM_TEXT_MSG", 1}} },
	{ pb.B4, "CLEAR",		nil },
	{ pb.L1, {{"TEXT 1>", 	nil, nil }, {"?",	tp_default_border, {{"MPD_COM_IDM_FREE_TEXT_Border", 0},{"MPD_COM_IDM_FREE_TEXT_Line", 0}}} } },
	{ pb.L2, {{"TEXT 2>", 	nil, nil }, {"?",	tp_default_border, {{"MPD_COM_IDM_FREE_TEXT_Border", 1},{"MPD_COM_IDM_FREE_TEXT_Line", 1}}} } },
	{ pb.L3, {{"TEXT 3>", 	nil, nil }, {"?",	tp_default_border, {{"MPD_COM_IDM_FREE_TEXT_Border", 2},{"MPD_COM_IDM_FREE_TEXT_Line", 2}}} } },
	{ pb.L4, {{"TEXT 4>", 	nil, nil }, {"?",	tp_default_border, {{"MPD_COM_IDM_FREE_TEXT_Border", 3},{"MPD_COM_IDM_FREE_TEXT_Line", 3}}} } },
}
createControls( Controls )


AddSendBtn("FreeText_SendMessageWindow", nil, {{"MPD_COM_IDM_FREE_SEND_Visible"}} )

