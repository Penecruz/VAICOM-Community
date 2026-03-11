dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

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
	{ pb.L1, "f",	tp_28, {{"MPD_COM_IDM_MPS_ARROW", 0}} },
	{ pb.L2, "f",	tp_28, {{"MPD_COM_IDM_MPS_ARROW", 1}} },
	{ pb.L3, "f",	tp_28, {{"MPD_COM_IDM_MPS_ARROW", 2}} },
	{ pb.L4, "f",	tp_28, {{"MPD_COM_IDM_MPS_ARROW", 3}} },
	{ pb.L5, "f",	tp_28, {{"MPD_COM_IDM_MPS_ARROW", 4}} },
	{ pb.R1, "g",	tp_28, {{"MPD_COM_IDM_MPS_ARROW", 5}} },
	{ pb.R2, "g",	tp_28, {{"MPD_COM_IDM_MPS_ARROW", 6}} },
	{ pb.R3, "g",	tp_28, {{"MPD_COM_IDM_MPS_ARROW", 7}} },
	{ pb.R4, "g",	tp_28, {{"MPD_COM_IDM_MPS_ARROW", 8}} },
	{ pb.R5, "g",	tp_28, {{"MPD_COM_IDM_MPS_ARROW", 9}} },
}

local pos_left_border  = {pb_props[pb.T3].pos[1]+15,pb_props[pb.T3].pos[2]+15 }	
local pos_right_border = {pb_props[pb.T4].pos[1]+5, pb_props[pb.T4].pos[2]+15 } 
draw_border_with_caption( pos_left_border, pos_right_border, 7, 1, "TEXT MSG",  tp_default, nil, false )

pb_props[pb.L1].tp.alignment = "RightTop"
pb_props[pb.L2].tp.alignment = "RightTop"
pb_props[pb.L3].tp.alignment = "RightTop"
pb_props[pb.L4].tp.alignment = "RightTop"
pb_props[pb.L5].tp.alignment = "RightTop"

createControls( Controls )
addMPS_TEXT_SecondLabels()

--function draw_line( verts, material, parent, width, name, controllers, h_clip_relation, level )
draw_line( {{0, pb_props[pb.L1].pos[2]},{0, pb_props[pb.L6].pos[2]}}, IND_MPD_MATERIAL_GREEN, nil, 3, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )
-- small borders
addBorderSmall( pb.L1, 30, {{"MPD_COM_IDM_MPS_TEXT_Border", 0}} )
addBorderSmall( pb.L2, 30, {{"MPD_COM_IDM_MPS_TEXT_Border", 1}} )
addBorderSmall( pb.L3, 30, {{"MPD_COM_IDM_MPS_TEXT_Border", 2}} )
addBorderSmall( pb.L4, 30, {{"MPD_COM_IDM_MPS_TEXT_Border", 3}} )
addBorderSmall( pb.L5, 30, {{"MPD_COM_IDM_MPS_TEXT_Border", 4}} )
addBorderSmall( pb.R1, 30, {{"MPD_COM_IDM_MPS_TEXT_Border", 5}} )
addBorderSmall( pb.R2, 30, {{"MPD_COM_IDM_MPS_TEXT_Border", 6}} )
addBorderSmall( pb.R3, 30, {{"MPD_COM_IDM_MPS_TEXT_Border", 7}} )
addBorderSmall( pb.R4, 30, {{"MPD_COM_IDM_MPS_TEXT_Border", 8}} )
addBorderSmall( pb.R5, 30, {{"MPD_COM_IDM_MPS_TEXT_Border", 9}} )

local w = 990
local w0 = 10 - w/2 
local h = tp_default.height*5.0
local h0 = h/2 - tp_default.steep_y + 10	

tp_28.alignment = "LeftBottom"
local value = {	-- value
							{"", {w0, h0},							tp_28,	{{"MPD_COM_IDM_MPS_TEXT_Line", 0}}},
							{"", {w0, h0 - 1.0*tp_default.steep_y},	tp_28,	{{"MPD_COM_IDM_MPS_TEXT_Line", 1}}},
							{"", {w0, h0 - 2.0*tp_default.steep_y},	tp_28,	{{"MPD_COM_IDM_MPS_TEXT_Line", 2}}},
							{"", {w0, h0 - 3.0*tp_default.steep_y},	tp_28,	{{"MPD_COM_IDM_MPS_TEXT_Line", 3}}},
						}
local r = RoundingRadius						
RoundingRadius = 1			
			
--function AddRoundCornersWindow(name, pos, width, height, value, tp, material, parent, controllers, alignment, box_cntrs, up_down, is_transparent)
AddRoundCornersWindow("MPS_Text_Window_1",	 { 15, pb_props[pb.L1].pos[2]-15},	w,	160, value, tp_28,	IND_MPD_MATERIAL_GREEN,	nil, {{"MPD_COM_IDM_MPS_TEXT_Border", 0}}, nil, 0, nil, false )
AddRoundCornersWindow("MPS_Text_Window_2",	 { 15, pb_props[pb.L2].pos[2]-15},	w,	160, value, tp_28,	IND_MPD_MATERIAL_GREEN,	nil, {{"MPD_COM_IDM_MPS_TEXT_Border", 1}}, nil, 0, nil, false )
AddRoundCornersWindow("MPS_Text_Window_3",	 { 15, pb_props[pb.L3].pos[2]-15},	w,	160, value, tp_28,	IND_MPD_MATERIAL_GREEN,	nil, {{"MPD_COM_IDM_MPS_TEXT_Border", 2}}, nil, 0, nil, false )
AddRoundCornersWindow("MPS_Text_Window_4",	 { 15, pb_props[pb.L4].pos[2]-15},	w,	160, value, tp_28,	IND_MPD_MATERIAL_GREEN,	nil, {{"MPD_COM_IDM_MPS_TEXT_Border", 3}}, nil, 0, nil, false )
AddRoundCornersWindow("MPS_Text_Window_5",	 { 15, pb_props[pb.L5].pos[2]-15},	w,	160, value, tp_28,	IND_MPD_MATERIAL_GREEN,	nil, {{"MPD_COM_IDM_MPS_TEXT_Border", 4}}, nil, 0, nil, false )
AddRoundCornersWindow("MPS_Text_Window_6",	 {-25, pb_props[pb.R1].pos[2]-15},	w,	160, value, tp_28,	IND_MPD_MATERIAL_GREEN,	nil, {{"MPD_COM_IDM_MPS_TEXT_Border", 5}}, nil, 0, nil, false )
AddRoundCornersWindow("MPS_Text_Window_7",	 {-25, pb_props[pb.R2].pos[2]-15},	w,	160, value, tp_28,	IND_MPD_MATERIAL_GREEN,	nil, {{"MPD_COM_IDM_MPS_TEXT_Border", 6}}, nil, 0, nil, false )
AddRoundCornersWindow("MPS_Text_Window_8",	 {-25, pb_props[pb.R3].pos[2]-15},	w,	160, value, tp_28,	IND_MPD_MATERIAL_GREEN,	nil, {{"MPD_COM_IDM_MPS_TEXT_Border", 7}}, nil, 0, nil, false )
AddRoundCornersWindow("MPS_Text_Window_9",	 {-25, pb_props[pb.R4].pos[2]-15},	w,	160, value, tp_28,	IND_MPD_MATERIAL_GREEN,	nil, {{"MPD_COM_IDM_MPS_TEXT_Border", 8}}, nil, 0, nil, false )
AddRoundCornersWindow("MPS_Text_Window_10",	 {-25, pb_props[pb.R5].pos[2]-15},	w,	160, value, tp_28,	IND_MPD_MATERIAL_GREEN,	nil, {{"MPD_COM_IDM_MPS_TEXT_Border", 9}}, nil, 0, nil, false )
RoundingRadius = r

AddSendBtn("MpsText_SendMessageWindow", nil, {{"MPD_COM_IDM_MPS_SEND_Visible"}} )

