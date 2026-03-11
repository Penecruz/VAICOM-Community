dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	{ pb.B4, "ATHS", nil },
	{ "MSG",
		{
			{ pb.B5, "REC", tp_default_border, nil },
			{ pb.B6, "SEND", tp_default_border, {{"MPD_COM_BTN_BORDER", pb.B6}} },
		}
	},
}

local Controls = {}
Controls = 
{	
	{ pb.L1, { 
				{"", nil, {{"MPD_COM_REC_Title", 0}}}, 
				{"", nil, {{"MPD_COM_REC_TitleSecondLine", 0}}} 
			}}, 
	{ pb.L2, { 
				{"", nil, {{"MPD_COM_REC_Title", 1}}}, 
				{"", nil, {{"MPD_COM_REC_TitleSecondLine", 1}}} 
			}}, 
	{ pb.L3, { 
				{"", nil, {{"MPD_COM_REC_Title", 2}}}, 
				{"", nil, {{"MPD_COM_REC_TitleSecondLine", 2}}} 
			}}, 
	{ pb.L4, { 
				{"", nil, {{"MPD_COM_REC_Title", 3}}}, 
				{"", nil, {{"MPD_COM_REC_TitleSecondLine", 3}}} 
			}}, 
	{ pb.L5, { 
				{"", nil, {{"MPD_COM_REC_Title", 4}}}, 
				{"", nil, {{"MPD_COM_REC_TitleSecondLine", 4}}} 
			}}, 
	{ pb.L6, { 
				{"", nil, {{"MPD_COM_REC_Title", 5}}}, 
				{"", nil, {{"MPD_COM_REC_TitleSecondLine", 5}}} 
			}}, 
	{ pb.R1, { 
				{"", nil, {{"MPD_COM_REC_Title", 6}}}, 
				{"", nil, {{"MPD_COM_REC_TitleSecondLine", 6}}} 
			}}, 
	{ pb.R2, { 
				{"", nil, {{"MPD_COM_REC_Title", 7}}}, 
				{"", nil, {{"MPD_COM_REC_TitleSecondLine", 7}}} 
			}}, 
	{ pb.R3, { 
				{"", nil, {{"MPD_COM_REC_Title", 8}}}, 
				{"", nil, {{"MPD_COM_REC_TitleSecondLine", 8}}} 
			}}, 
	{ pb.R4, { 
				{"", nil, {{"MPD_COM_REC_Title", 9}}}, 
				{"", nil, {{"MPD_COM_REC_TitleSecondLine", 9}}} 
			}}, 
	{ pb.R5, { 
				{"", nil, {{"MPD_COM_REC_Title", 10}}}, 
				{"", nil, {{"MPD_COM_REC_TitleSecondLine", 10}}} 
			}}, 
	{ pb.R6, { 
				{"", nil, {{"MPD_COM_REC_Title", 11}}}, 
				{"", nil, {{"MPD_COM_REC_TitleSecondLine", 11}}} 
			}},
	---------------------------
	{ pb.B2, "c",	tp_36 },
	{ pb.B3, "d",	tp_36 },
}
draw_line( {{0, pb_props[pb.L1].pos[2]},{0, pb_props[pb.L6].pos_down[2]}}, IND_MPD_MATERIAL_GREEN, nil, 3, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )

local pos_left	= pb_props[pb.B2].pos	
local pos_right = pb_props[pb.B3].pos
local pos_center_lo = {(pos_left[1]+pos_right[1])/2, pos_left[2]}
local pos_center_hi = {pos_center_lo[1], pb_props[pb.B2].pos_up[2]}	
addText( "1/1",  pos_center_lo, tp_28_center_bottom, {{"MPD_COM_REC_PageNum"}, {"MPD_COM_REC_PageVisible"}}	)
addText( "PAGE",  pos_center_hi, tp_28_center_bottom, {{"MPD_COM_REC_PageVisible"}})

--------------------------------------------------------------
addBorder( pb.L1, 450, {{"MFD_COM_REC_Border", 0 }} )
addBorder( pb.L2, 450, {{"MFD_COM_REC_Border", 1 }} )
addBorder( pb.L3, 450, {{"MFD_COM_REC_Border", 2 }} )
addBorder( pb.L4, 450, {{"MFD_COM_REC_Border", 3 }} )
addBorder( pb.L5, 450, {{"MFD_COM_REC_Border", 4 }} )
addBorder( pb.L6, 450, {{"MFD_COM_REC_Border", 5 }} )
addBorder( pb.R1, 450, {{"MFD_COM_REC_Border", 6 }} )
addBorder( pb.R2, 450, {{"MFD_COM_REC_Border", 7 }} )
addBorder( pb.R3, 450, {{"MFD_COM_REC_Border", 8 }} )
addBorder( pb.R4, 450, {{"MFD_COM_REC_Border", 9 }} )
addBorder( pb.R5, 450, {{"MFD_COM_REC_Border", 10}} )
addBorder( pb.R6, 450, {{"MFD_COM_REC_Border", 11}} )
--------------------------------------------------------------
createMenu( Menu )
createControls( Controls )