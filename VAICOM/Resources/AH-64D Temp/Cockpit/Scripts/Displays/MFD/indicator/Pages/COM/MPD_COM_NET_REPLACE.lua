dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

function addNetReplaceSecondLabels()
local x = pb_props[pb.L1].pos_down[1] + tp_default.width
addPB( pb.L1, "", {x, pb_props[pb.L1].pos_up[2] }, tp_28_white, {{"MPD_COM_NET_CallSign", 0}}) 
addPB( pb.L2, "", {x, pb_props[pb.L2].pos_up[2] }, tp_28_white, {{"MPD_COM_NET_CallSign", 1}}) 
addPB( pb.L3, "", {x, pb_props[pb.L3].pos_up[2] }, tp_28_white, {{"MPD_COM_NET_CallSign", 2}})
addPB( pb.L4, "", {x, pb_props[pb.L4].pos_up[2] }, tp_28_white, {{"MPD_COM_NET_CallSign", 3}})
addPB( pb.L5, "", {x, pb_props[pb.L5].pos_up[2] }, tp_28_white, {{"MPD_COM_NET_CallSign", 4}})

local x = pb_props[pb.L1].pos_down[1] + tp_default.width
addPB( pb.L1, "", {x, pb_props[pb.L1].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_LB", 0}}) 
addPB( pb.L2, "", {x, pb_props[pb.L2].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_LB", 1}}) 
addPB( pb.L3, "", {x, pb_props[pb.L3].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_LB", 2}})
addPB( pb.L4, "", {x, pb_props[pb.L4].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_LB", 3}})
addPB( pb.L5, "", {x, pb_props[pb.L5].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_LB", 4}})

local x = pb_props[pb.L1].pos_down[1] + tp_default.width * 4 -6
addPB( pb.L1, "", {x, pb_props[pb.L1].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_TEAM", 0}}, {"", "TEAM"}) 
addPB( pb.L2, "", {x, pb_props[pb.L2].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_TEAM", 1}}, {"", "TEAM"}) 
addPB( pb.L3, "", {x, pb_props[pb.L3].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_TEAM", 2}}, {"", "TEAM"})
addPB( pb.L4, "", {x, pb_props[pb.L4].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_TEAM", 3}}, {"", "TEAM"})
addPB( pb.L5, "", {x, pb_props[pb.L5].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_TEAM", 4}}, {"", "TEAM"})

local x = pb_props[pb.L1].pos_down[1] + tp_default.width * 9 -6
addPB( pb.L1, "", {x, pb_props[pb.L1].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_PRI", 0}}, {"", "PRI"} ) 
addPB( pb.L2, "", {x, pb_props[pb.L2].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_PRI", 1}}, {"", "PRI"} ) 
addPB( pb.L3, "", {x, pb_props[pb.L3].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_PRI", 2}}, {"", "PRI"} )
addPB( pb.L4, "", {x, pb_props[pb.L4].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_PRI", 3}}, {"", "PRI"} )
addPB( pb.L5, "", {x, pb_props[pb.L5].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_PRI", 4}}, {"", "PRI"} )

x = pb_props[pb.R1].pos_down[1] - tp_default.width
addPB( pb.R1, "", {x, pb_props[pb.R1].pos_up[2] }, tp_28_white, {{"MPD_COM_NET_CallSign", 5}} ) 
addPB( pb.R2, "", {x, pb_props[pb.R2].pos_up[2] }, tp_28_white, {{"MPD_COM_NET_CallSign", 6}} ) 
addPB( pb.R3, "", {x, pb_props[pb.R3].pos_up[2] }, tp_28_white, {{"MPD_COM_NET_CallSign", 7}} )
addPB( pb.R4, "", {x, pb_props[pb.R4].pos_up[2] }, tp_28_white, {{"MPD_COM_NET_CallSign", 8}} )
addPB( pb.R5, "", {x, pb_props[pb.R5].pos_up[2] }, tp_28_white, {{"MPD_COM_NET_CallSign", 9}} )



x = pb_props[pb.R1].pos_down[1] - tp_default.width * 5
addPB( pb.R1, "", {x, pb_props[pb.R1].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_TEAM", 5}}, {"", "TEAM"}) 
addPB( pb.R2, "", {x, pb_props[pb.R2].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_TEAM", 6}}, {"", "TEAM"}) 
addPB( pb.R3, "", {x, pb_props[pb.R3].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_TEAM", 7}}, {"", "TEAM"})
addPB( pb.R4, "", {x, pb_props[pb.R4].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_TEAM", 8}}, {"", "TEAM"})
addPB( pb.R5, "", {x, pb_props[pb.R5].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_TEAM", 9}}, {"", "TEAM"})

x = pb_props[pb.R1].pos_down[1] - tp_default.width
addPB( pb.R1, "", {x, pb_props[pb.R1].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_PRI", 5}}, {"", "PRI"}) 
addPB( pb.R2, "", {x, pb_props[pb.R2].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_PRI", 6}}, {"", "PRI"}) 
addPB( pb.R3, "", {x, pb_props[pb.R3].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_PRI", 7}}, {"", "PRI"})
addPB( pb.R4, "", {x, pb_props[pb.R4].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_PRI", 8}}, {"", "PRI"})
addPB( pb.R5, "", {x, pb_props[pb.R5].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_PRI", 9}}, {"", "PRI"})


pb_props[pb.R1].tp.alignment = "LeftTop"
pb_props[pb.R2].tp.alignment = "LeftTop"
pb_props[pb.R3].tp.alignment = "LeftTop"
pb_props[pb.R4].tp.alignment = "LeftTop"
pb_props[pb.R5].tp.alignment = "LeftTop"

x = pb_props[pb.R1].pos_down[1] - tp_default.width * 12
addPB( pb.R1, "", {x, pb_props[pb.R1].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_LB", 5}} ) 
addPB( pb.R2, "", {x, pb_props[pb.R2].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_LB", 6}} ) 
addPB( pb.R3, "", {x, pb_props[pb.R3].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_LB", 7}} )
addPB( pb.R4, "", {x, pb_props[pb.R4].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_LB", 8}} )
addPB( pb.R5, "", {x, pb_props[pb.R5].pos_down[2] }, tp_28_white, {{"MPD_COM_NET_LB", 9}} )

pb_props[pb.R1].tp.alignment = "RightTop"
pb_props[pb.R2].tp.alignment = "RightTop"
pb_props[pb.R3].tp.alignment = "RightTop"
pb_props[pb.R4].tp.alignment = "RightTop"
pb_props[pb.R5].tp.alignment = "RightTop"

end                
local Menu = {}
Menu = 
{ 
	{ pb.L6,  { 
				{"MBR",	nil}, 
				{"DIR", nil} 
			}},
	
	{ pb.B1, "COM",		nil },	
	{ pb.B4, "NET",		tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.B4}} },
}
createMenu( Menu )

local pos_left_border  = {pb_props[pb.T5].pos[1]-35, pb_props[pb.T5].pos[2]+15 }	
local pos_right_border = {pb_props[pb.T6].pos[1], pb_props[pb.T6].pos[2]+15 } 
draw_border_with_caption( pos_left_border, pos_right_border, 7, 1, "STORE",  tp_default, nil, false )

Controls = 
{	
	{ pb.T5, "REPLACE",	tp_default_border, nil},
	{ pb.T6, "ADD",	tp_28, nil},

	{ pb.L1, { 
				{"f", tp_28_white}, 
				{"", tp_28_white} 
			}}, 
	{ pb.L2, { 
				{"f", tp_28_white}, 
				{"", tp_28_white} 
			}},      
	{ pb.L3, {      
				{"f", tp_28_white}, 
				{"", tp_28_white} 
			}},      
	{ pb.L4, {      
				{"f", tp_28_white}, 
				{"", tp_28_white} 
			}},      
	{ pb.L5, {      
				{"f", tp_28_white}, 
				{"", tp_28_white} 
			}},      
	{ pb.R1, {       
				{"g", tp_28_white}, 
				{"", tp_28_white} 
			}},      
	{ pb.R2, {      
				{"g", tp_28_white}, 
				{"", tp_28_white} 
			}},      
	{ pb.R3, {       
				{"g", tp_28_white}, 
				{"", tp_28_white} 
			}},      
	{ pb.R4, {      
				{"g", tp_28_white}, 
				{"", tp_28_white} 
			}},      
	{ pb.R5, {       
				{"g", tp_28_white}, 
				{"", tp_28_white} 
			}}, 
 	{ pb.B2, "c", tp_36, {{"MPD_COM_NET_PageVisible"}}},
	{ pb.B3, "d", tp_36, {{"MPD_COM_NET_PageVisible"}}},

 	{ pb.B2, "c",		tp_36 },
	{ pb.B3, "d",		tp_36 },				
}
createControls( Controls )

addNetReplaceSecondLabels()

addBorder( pb.L6, 80, nil, 15 )

local pos_left	= pb_props[pb.B2].pos	
local pos_right = pb_props[pb.B3].pos
local pos_center_lo = {(pos_left[1]+pos_right[1])/2, pos_left[2]}
local pos_center_hi = {pos_center_lo[1], pb_props[pb.B2].pos_up[2]}	
addText( "1/1",  pos_center_lo, tp_28_center_bottom, {{"MPD_COM_NET_Page"}}	)
addText( "PAGE",  pos_center_hi, tp_28_center_bottom )


tp_28 = createTextProperty( 28,  nil,	nil,  "LeftBottom" )

addCOM_NET_PRESET_Status_Window()

local w = tp_default.width*17.0
local h = tp_default.height*6.0
local w0 =  - w/2 
local h0 = h/2 - tp_default.steep_y	
AddRoundCornersWindow("COM_NET_MEMBER_Status_Window",	{ 5, pb_props[pb.L3].pos[2]  },
						w, h,
						{
							{"DL:", { w0 + tp_default.width, h0},							tp_28, {{"COM_NET_MEMBER_Status_Window", 1}}},  						
							{"TF:", { w0 + tp_default.width, h0 - tp_default.steep_y},		tp_28, {{"COM_NET_MEMBER_Status_Window", 2}}},
							{"TI:", { w0 + tp_default.width, h0 - 2*tp_default.steep_y},	tp_28, {{"COM_NET_MEMBER_Status_Window", 3}}}, 
							{"FS:", { w0 + tp_default.width, h0 - 3*tp_default.steep_y},	tp_28, {{"COM_NET_MEMBER_Status_Window", 4}}}		
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
addText( "REPLACE", {0, 350}, tp_28_white_center_bottom, nil	)							