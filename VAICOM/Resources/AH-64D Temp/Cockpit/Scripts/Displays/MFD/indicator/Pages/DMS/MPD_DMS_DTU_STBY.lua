dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T1, "DTU",			tp_default_border },
	{ pb.B1, "DMS",			nil },
}

createMenu( Menu )

local Controls = {}
Controls = 
{
	{ pb.B3, 	{{"DTU",	nil },{"OPER",	tp_default_border, {{"DMS_DTU_STBY_Switch"}}, 		{"STBY", "OPER"} }},	nil,	nil}
}

createControls( Controls )

addText( "OPER",		{pb_props[pb.B3].pos[1] + tp_default.width*0.3, pb_props[pb.B3].pos[2] + tp_default.height * 0.05}, tp_36_center_bottom,	{{"DMS_DTU_STBY_SwitchBlack"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )

addText( "ADVISORY:",		{pb_props[pb.T1].pos[1] - tp_default.width*2.0,	pb_props[pb.L1].pos[2]},		tp_def_left, {{"DMS_DTU_STBY_Loading"}})
draw_line( {{pb_props[pb.T1].pos[1] - tp_default.width*2.0,	 	pb_props[pb.L1].pos[2] - tp_default.height * 1.1},	{pb_props[pb.T1].pos[1] + tp_default.width*6.5,	pb_props[pb.L1].pos[2] - tp_default.height  * 1.1}},		tp_default.material,	nil,	nil,	nil, {{"DMS_DTU_STBY_Loading"}})

addText( "DTU INITIALIZATION IS IN PROGRESS",		{pb_props[pb.T1].pos[1] - tp_default.width*2.0,	pb_props[pb.L2].pos[2]},		tp_28_white_left_bottom, {{"DMS_DTU_STBY_Loading"}})