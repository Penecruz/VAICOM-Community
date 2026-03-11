dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local pos_left	= {pb_props[pb.T3].pos[1], pb_props[pb.T3].pos[2] - tp_default.height }	
local pos_right = {pb_props[pb.T4].pos[1], pb_props[pb.T4].pos[2] - tp_default.height } 
draw_border_with_caption( pos_left, pos_right, 5, 1, "ROUTE", tp_default, nil, false )
pos_left	= {pb_props[pb.T5].pos[1], pb_props[pb.T5].pos[2] - tp_default.height }	
pos_right = {pb_props[pb.T6].pos[1], pb_props[pb.T6].pos[2] - tp_default.height } 
draw_border_with_caption( pos_left, pos_right, 7, 1, "WPT/HZD", tp_default, nil, false )
pos_left	= {pb_props[pb.B5].pos[1], pb_props[pb.B5].pos[2] + tp_default.height }	
pos_right = {pb_props[pb.B6].pos[1], pb_props[pb.B6].pos[2] + tp_default.height } 
draw_border_with_caption( pos_left, pos_right, 10, 1, "CONTR MEAS", tp_def_bottom, nil, false )
pos_left	= {pb_props[pb.R5].pos[1] - tp_default.width, pb_props[pb.R5].pos[2] }	
pos_right = {pb_props[pb.R6].pos[1] - tp_default.width, pb_props[pb.R6].pos[2] - tp_default.height } 
draw_border_with_caption( pos_left, pos_right, 3, 1, "T/T", pb_props[pb.R5].tp, nil, false )

local Controls = {}
Controls = 
{	
			{ pb.T3, 	"ALL",																										nil,	{{"DMS_NoBorder"}}},
			{ pb.T4, 	{{"ONE>",	{{"DMS_NoBorder"}} },{"?",	tp_default_border, nil }},											nil,	nil},
			{ pb.T5, 	"ALL",																										nil,	{{"DMS_NoBorder"}}},
			{ pb.T6, 	{{"ONE>",	{{"DMS_NoBorder"}} },{"?",	tp_default_border, {{"MFD_DataEntryButton_frame",pb.T6}} }},		nil,	nil},
			{ pb.B3, 	{{"DTU",	nil },		{"OPER",	tp_default_border, {{"DMS_SHUTDOWN_DTU_Button"}}, {"STBY", "OPER"} }},	nil,	nil},
			{ pb.B5, 	"ALL",																										nil,	{{"DMS_NoBorder"}}},
			{ pb.B6, 	{{"ONE>",	{{"DMS_NoBorder"}} },{"?",	tp_default_border, {{"MFD_DataEntryButton_frame",pb.B6}} }},		nil,	nil},
			{ pb.L1, 	"ALL",																										nil,	nil},
			{ pb.L2, 	{{"DATA",	nil },{"MISSION 1",	tp_default_border, {{"DMS_DTU_MsnPage"}}, {"", "MISSION 1", "MISSION 2"}  }},	nil,	nil},
	{ "ZONES", 
		{ 
			{ pb.L4, 	"ALL",																										nil,	{{"DMS_NoBorder"}}},
			{ pb.L5, 	"NF",																										nil,	{{"DMS_NoBorder"}}},
			{ pb.L6, 	"PF",																										nil,	{{"DMS_NoBorder"}}},
		}
	},
			{ pb.R1, 	{{"PRIORITY",	nil },{"SCHEME",	nil }},																	nil,	nil},
			{ pb.R5, 	"ALL",																										nil,	{{"DMS_NoBorder"}}},
			{ pb.R6, 	{{"ONE>",	{{"DMS_NoBorder"}} },{"?",	tp_default_border, {{"MFD_DataEntryButton_frame",pb.R6}} }},		nil,	nil},
}

createControls( Controls )