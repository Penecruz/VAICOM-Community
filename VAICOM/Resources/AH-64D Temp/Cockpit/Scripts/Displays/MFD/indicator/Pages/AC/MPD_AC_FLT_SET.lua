dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_FLT_Symbology.lua")

local heading_scale_Y = 345;
pb_props[pb.L5].pos[1] = pb_props[pb.L5].pos[1] - 5
pb_props[pb.L6].pos[1] = pb_props[pb.L5].pos[1] - 5

local Controls = {}
Controls = 
{
	{ pb.T1, {
				{"HI>", nil, {{"FLT_SET_BTN_VISIBLE"}} },
				{"1200", tp_default_border, {{"FLT_SET_HI_Button", pb.T1}}}
			}
	},
	{ pb.T3, {
				{"LO>", nil, {{"FLT_SET_BTN_VISIBLE"}} },
				{"100", tp_default_border, {{"FLT_SET_LO_Button", pb.T3}}}
			}
	},
	{ pb.T4, {
				{"UNIT", nil },
				{"MB", tp_default_border, {{"FLT_SET_UNIT_PRESS_Button"}}, {"MB", "IN"}}
			}
	},
	{ pb.T5, {
				{"ALT>", nil, {{"FLT_SET_Alt_Button_Enabled"}}},
				{"1237", tp_default_border, {{"FLT_SET_ALT_Button", pb.T5}}}
			}
	},
	{ pb.T6, {
				{"PRES>", nil, {{"FLT_SET_Press_Button_Enabled"}}},
				{"30.14", tp_default_border, {{"FLT_SET_PRESS_Button", pb.T6 }}}
			}
	},
	{ pb.B2, {
				{"UNIT", nil },
				{"KM", tp_default_border, {{"FLT_SET_UINT_DIST_Button"}}, { "KM","NM"}}
			}
	},
	{ pb.R6, "RDR ALT}",	nil, {{"FLT_RDR_ALT_Button"}}, 	{"RDR ALT}", "RDR ALT{"} },	
	{ pb.B1, "FLT",	tp_default_border, {{"MFD_AC_OriginatorFmt"}}, {"ENG", "FLT", "FUEL", "PERF","UTIL"} },
	{ pb.B6, "SET",	tp_default_border },
	{ pb.L2, "G-RESET",	nil },
	
	
	{ pb.L5, "a", tp_52, nil},--BIAS UP
    { pb.L6, "b", tp_52, nil}, --BIAS DN 
	
}
createControls( Controls )

pb_props[pb.L5].pos[1] = pb_props[pb.L5].pos[1] + 5
pb_props[pb.L6].pos[1] = pb_props[pb.L5].pos[1] + 5

local Boxes = {}
Boxes = 
{ 	

	{
		{{340, heading_scale_Y+tp_36.height+20}, nil, nil, nil, nil, tp_36, { 20,25,10,-15 } },
		{{"1237", tp_36_right_bottom, {{"FLT_SET_BaroInertAltitude"}}, nil, {110,0,0,0} }},
		{{"30.14", tp_36_right_bottom, {{"FLT_SET_BaroPress"}}, nil, {110,0,0,0} }}
	}
}
createInfoBoxes( Boxes )
addText( "1.3G", {-440, -70}, tp_36,{{"FLT_G_SET"}} )
draw_acceleration()
