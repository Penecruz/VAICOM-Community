dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T1, "SUFFIX",		nil },
	{ pb.L6,  { 
				{"MBR",	nil}, 
				{"DIR", nil} 
			}},
	
	{ pb.B1, "COM",		nil },	
	{ pb.B4, "NET",		tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.B4}} },
	{ pb.R6, "MODEM",	tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.R6}}},
	{ pb.B6, {{"PRESET", nil, nil },{"EDIT", nil}}  }
}
createMenu( Menu )
addBorder( pb.B6, 100, {{"MPD_COM_BORDER", pb.B6}}, 0, IND_MPD_MATERIAL_DARK_GREEN )
local pos_left	= pb_props[pb.T2].pos	
local pos_right = {pb_props[pb.T6].pos[1], pb_props[pb.T6].pos[2]}
draw_border_with_caption( pos_left, pos_right, 5, 2, "MEMBER EDIT", pb_props[pb.T2].tp, nil, false )

Controls1 = 
{		
	{ pb.T2, "DEL",		nil },
	{ pb.T3, "TEAM",	tp_default_border, {{"MPD_COM_NET_TEAM_BORDER"}} },
	{ pb.T4, "PRI",		tp_default_border, {{"MPD_COM_NET_PRI_BORDER"}} },
	{ pb.T5, { 
			{"C/S>", nil }, 
			{"",  tp_default_border, {{"MPD_COM_NET_CurrCallSign", pb.T5}}} 
		}},
	{ pb.T6, { 
		{"SUB>", nil }, 
		{"",  tp_default_border, {{"MPD_COM_NET_CurrLB", pb.T6}}} 
	}}, 		
	{ pb.L1, { 
				{"", nil, {{"MPD_COM_NET_CallSign", 0}}}, 
				{"", nil, {{"MPD_COM_NET_LB", 0}}} 
			}}, 
	{ pb.L2, { 
				{"", nil, {{"MPD_COM_NET_CallSign", 1}}}, 
				{"", nil, {{"MPD_COM_NET_LB", 1}}} 
			}}, 
	{ pb.L3, { 
				{"", nil, {{"MPD_COM_NET_CallSign", 2}}}, 
				{"", nil, {{"MPD_COM_NET_LB", 2}}} 
			}}, 
	{ pb.L4, { 
				{"", nil, {{"MPD_COM_NET_CallSign", 3}}}, 
				{"", nil, {{"MPD_COM_NET_LB", 3}}} 
			}}, 
	{ pb.L5, { 
				{"", nil, {{"MPD_COM_NET_CallSign", 4}}}, 
				{"", nil, {{"MPD_COM_NET_LB", 4}}} 
			}}, 
	{ pb.R1, { 
				{"", nil, {{"MPD_COM_NET_CallSign", 5}}}, 
				{"", nil, {{"MPD_COM_NET_PRI", 5}}, {"", "PRI"}} 
			}}, 
	{ pb.R2, { 
				{"", nil, {{"MPD_COM_NET_CallSign", 6}}}, 
				{"", nil, {{"MPD_COM_NET_PRI", 6}}, {"", "PRI"}} 
			}}, 
	{ pb.R3, { 
				{"", nil, {{"MPD_COM_NET_CallSign", 7}}}, 
				{"", nil, {{"MPD_COM_NET_PRI", 7}}, {"", "PRI"}} 
			}}, 
	{ pb.R4, { 
				{"", nil, {{"MPD_COM_NET_CallSign", 8}}}, 
				{"", nil, {{"MPD_COM_NET_PRI", 8}}, {"", "PRI"}} 
			}}, 
	{ pb.R5, { 
				{"", nil, {{"MPD_COM_NET_CallSign", 9}}}, 
				{"", nil, {{"MPD_COM_NET_PRI", 9}}, {"", "PRI"}} 
			}}, 
 	{ pb.B2, "c", tp_36, {{"MPD_COM_NET_PageVisible"}}},
	{ pb.B3, "d", tp_36, {{"MPD_COM_NET_PageVisible"}}},				
}
createControls( Controls1 )

addNetSecondLabels()

addBorderBase( pb.L1, 220, 15, 35, {{"MFD_COM_NET_MemberBorder", 0}} )
addBorderBase( pb.L2, 220, 15, 35, {{"MFD_COM_NET_MemberBorder", 1}} )
addBorderBase( pb.L3, 220, 15, 35, {{"MFD_COM_NET_MemberBorder", 2}} )
addBorderBase( pb.L4, 220, 15, 35, {{"MFD_COM_NET_MemberBorder", 3}} )
addBorderBase( pb.L5, 220, 15, 35, {{"MFD_COM_NET_MemberBorder", 4}} )
addBorderBase( pb.R1, 220, 15, 35, {{"MFD_COM_NET_MemberBorder", 5}}, 0, -10 )
addBorderBase( pb.R2, 220, 15, 35, {{"MFD_COM_NET_MemberBorder", 6}}, 0, -10 )
addBorderBase( pb.R3, 220, 15, 35, {{"MFD_COM_NET_MemberBorder", 7}}, 0, -10 )
addBorderBase( pb.R4, 220, 15, 35, {{"MFD_COM_NET_MemberBorder", 8}}, 0, -10 )
addBorderBase( pb.R5, 220, 15, 35, {{"MFD_COM_NET_MemberBorder", 9}}, 0, -10 )

local pos_left	= pb_props[pb.B2].pos	
local pos_right = pb_props[pb.B3].pos
local pos_center_lo = {(pos_left[1]+pos_right[1])/2, pos_left[2]}
local pos_center_hi = {pos_center_lo[1], pb_props[pb.B2].pos_up[2]}	
addText( "1/1",  pos_center_lo, tp_28_center_bottom, {{"MPD_COM_NET_Page"}, {"MPD_COM_NET_PageVisible"}}	)
addText( "PAGE",  pos_center_hi, tp_28_center_bottom, {{"MPD_COM_NET_PageVisible"}} )

addCOM_NET_PRESET_Status_Window()


local w = tp_default.width*17.0
local h = tp_default.height*7.0
local w0 = 10 - w/2 
local h0 = h/2 - tp_default.steep_y	
AddRoundCornersWindow("COM_NET_MEMBER_Status_Window",	{ 5, pb_props[pb.L3].pos[2]  },
						w, h,
						{
							{"DL:", { w0, h0},							tp_28, {{"COM_NET_MEMBER_Status_Window", 1}}},  						
							{"TF:", { w0, h0 - tp_default.steep_y},		tp_28, {{"COM_NET_MEMBER_Status_Window", 2}}},
							{"TI:", { w0, h0 - 2*tp_default.steep_y},	tp_28, {{"COM_NET_MEMBER_Status_Window", 3}}}, 
							{"FS:", { w0, h0 - 3*tp_default.steep_y},	tp_28, {{"COM_NET_MEMBER_Status_Window", 4}}}		
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
							
h = tp_default.height*8.0
w0 = 10 - w/2 
h0 = h/2 - tp_default.steep_y	
AddRoundCornersWindow("COM_NET_ORIG_Status_Window",	{ 5, pb_props[pb.L6].pos[2]  },
						w, h,
						{
							{"ORIG:", { w0, h0},						tp_28, {{"COM_NET_ORIG_Status_Window", 1}}},  						
							{"DL:", { w0, h0 - tp_default.steep_y},		tp_28, {{"COM_NET_ORIG_Status_Window", 2}}},
							{"TF:", { w0, h0 - 2*tp_default.steep_y},	tp_28, {{"COM_NET_ORIG_Status_Window", 3}}}, 
							{"TI:", { w0, h0 - 3*tp_default.steep_y},	tp_28, {{"COM_NET_ORIG_Status_Window", 4}}},
							{"FS:", { w0, h0 - 4*tp_default.steep_y},	tp_28, {{"COM_NET_ORIG_Status_Window", 5}}}		
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)

