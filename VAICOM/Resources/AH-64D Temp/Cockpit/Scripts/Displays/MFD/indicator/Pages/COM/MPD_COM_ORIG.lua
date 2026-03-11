dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

pb_props[pb.T5].show_barrier = false
pb_props[pb.T6].show_barrier = false

local Menu = {}
Menu = 
{ 			
	{ pb.B1, "COM",		nil },	
	{ pb.L6, {{"ORIG",	nil },{"DIR", nil }}},
	{ pb.B4, {{"ORIG",	tp_default_darkgreen_border, },{"ID", tp_default_darkgreen_border }}},
--	{ pb.B4, "NET",		 {{"MPD_COM_BTN_BORDER", pb.B4}} },
}
createMenu( Menu )

local Controls = {}
Controls = 
{	

	{ pb.T5, "REPLACE",	tp_28_white, nil},
	
	{ pb.L1, {{"", 	nil, {{"MFD_COMM_MemberDir_FirstLine", 0}}}, {"",	nil, {{"MFD_COMM_MemberDir_SecondLine", 0}}} } },
	{ pb.L2, {{"", 	nil, {{"MFD_COMM_MemberDir_FirstLine", 1}}}, {"",	nil, {{"MFD_COMM_MemberDir_SecondLine", 1}}} } },
	{ pb.L3, {{"", 	nil, {{"MFD_COMM_MemberDir_FirstLine", 2}}}, {"",	nil, {{"MFD_COMM_MemberDir_SecondLine", 2}}} } },
	{ pb.L4, {{"", 	nil, {{"MFD_COMM_MemberDir_FirstLine", 3}}}, {"",	nil, {{"MFD_COMM_MemberDir_SecondLine", 3}}} } },
	
	{ pb.R1, {{"", 	nil, {{"MFD_COMM_MemberDir_FirstLine", 4}}}, {"",	nil, {{"MFD_COMM_MemberDir_SecondLine", 4}}} } },
	{ pb.R2, {{"", 	nil, {{"MFD_COMM_MemberDir_FirstLine", 5}}}, {"",	nil, {{"MFD_COMM_MemberDir_SecondLine", 5}}} } },
	{ pb.R3, {{"", 	nil, {{"MFD_COMM_MemberDir_FirstLine", 6}}}, {"",	nil, {{"MFD_COMM_MemberDir_SecondLine", 6}}} } },
	{ pb.R4, {{"", 	nil, {{"MFD_COMM_MemberDir_FirstLine", 7}}}, {"",	nil, {{"MFD_COMM_MemberDir_SecondLine", 7}}} } },
	
	{ pb.B2, "c",	tp_36, {{"MPD_COM_MBR_PageVisible"}} },
	{ pb.B3, "d",	tp_36, {{"MPD_COM_MBR_PageVisible"}} },
	{ pb.B6, {{"SRCH>", nil, nil}, {"?", nil, nil} } },	
}
createControls( Controls )

--PB_Pos[pb.T5][2] = TYLeftPos
--PB_Pos[pb.T6][2] = TYLeftPos
--------------------------------------------------------------
addBorderBase( pb.L1, 450, 15, 35, {{"MFD_COMM_OrigDirBorder", 0}} )
addBorderBase( pb.L2, 450, 15, 35, {{"MFD_COMM_OrigDirBorder", 1}} )
addBorderBase( pb.L3, 450, 15, 35, {{"MFD_COMM_OrigDirBorder", 2}} )
addBorderBase( pb.L4, 450, 15, 35, {{"MFD_COMM_OrigDirBorder", 3}} )
addBorderBase( pb.R1, 450, 15, 35, {{"MFD_COMM_OrigDirBorder", 4}}, 0, -10)
addBorderBase( pb.R2, 450, 15, 35, {{"MFD_COMM_OrigDirBorder", 5}}, 0, -10)
addBorderBase( pb.R3, 450, 15, 35, {{"MFD_COMM_OrigDirBorder", 6}}, 0, -10)
addBorderBase( pb.R4, 450, 15, 35, {{"MFD_COMM_OrigDirBorder", 7}}, 0, -10)
--------------------------------------------------------------

addBorder( pb.L6, 100, {{"MPD_COM_BORDER", pb.L6}}, 20 )
addBorder( pb.B4, 100, {{"MPD_COM_BORDER", pb.B4}}, 20, IND_MPD_MATERIAL_DARK_GREEN )

local pos_left	= pb_props[pb.B2].pos	
local pos_right = pb_props[pb.B3].pos
local pos_center_lo = {(pos_left[1]+pos_right[1])/2, pos_left[2]}
local pos_center_hi = {pos_center_lo[1], pb_props[pb.B2].pos_up[2]}	
addText( "1/1",  pos_center_lo, tp_28_center_bottom, {{"MPD_COM_Orig_Page"},{"MPD_COM_MBR_PageVisible"}}	)
addText( "PAGE",  pos_center_hi, tp_28_center_bottom, {{"MPD_COM_MBR_PageVisible"}} )
------------------------------------------------------------ status windows------------------------------
local w = tp_default.width*34.0	
local h = tp_default.height*9.0
local w0 = 10 - w/2 
local h0 = h/2 - tp_default.steep_y	
AddRoundCornersWindow("COM_NET_MemderDir_Status_Window",	{ 5, pb_props[pb.L5].pos_c[2]-30  },
						w, h,
						{
							{"CallSign", { w0,						h0},						tp_28, {{"COM_OrigDirStatus_Window", 1}}},-- row 1
							{"TI:", { w0, 							h0 - tp_default.steep_y},	tp_28, {{"COM_OrigDirStatus_Window", 2}}},-- row 2
							{"DL:", { w0 + tp_default.width*14,		h0 - tp_default.steep_y},	tp_28, {{"COM_OrigDirStatus_Window", 3}}},-- row 2
							{"TF:", { w0 + tp_default.width*22,		h0 - tp_default.steep_y},	tp_28, {{"COM_OrigDirStatus_Window", 4}}},-- row 2

							{"FS:", 	{ w0,						h0 - 2*tp_default.steep_y},	tp_28, {{"COM_OrigDirStatus_Window", 5}}},-- row 3
							{"FS IP:",	{ w0 + tp_default.width*13,	h0 - 2*tp_default.steep_y},	tp_28, {{"COM_OrigDirStatus_Window", 6}}},-- row 3

							{"ACCESS:", { w0 + tp_default.width,	h0 - 3*tp_default.steep_y},	tp_28, {{"COM_OrigDirStatus_Window", 7}}},-- row 4
							{"TOTAL:",	{ w0 + tp_default.width*14,	h0 - 3*tp_default.steep_y},	tp_28, {{"COM_OrigDirStatus_Window", 8}}},-- row 4
							{"ID:",		{ w0 + tp_default.width*25,	h0 - 3*tp_default.steep_y},	tp_28, {{"COM_OrigDirStatus_Window", 9}}},-- row 4

							{"",		{ w0,						h0 - 4*tp_default.steep_y},	tp_28, {{"COM_OrigDirStatus_Window", 10}}},-- row 5
							{"",		{ w0,						h0 - 5*tp_default.steep_y},	tp_28, {{"COM_OrigDirStatus_Window", 11}}},-- row 5
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
		
local verts	= { {0, 350}, {0, -100} }			
draw_line( verts, IND_MPD_MATERIAL_DARK_GREEN, nil, 5, "vert_line" )
						
pb_props[pb.T5].show_barrier = true
pb_props[pb.T6].show_barrier = true