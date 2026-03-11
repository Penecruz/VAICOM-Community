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
	{ pb.B2, "c",	tp_36, {{"MPD_COM_REC_PageVisible"}} },
	{ pb.B3, "d",	tp_36, {{"MPD_COM_REC_PageVisible"}} },
}


local pos_left	= pb_props[pb.B2].pos	
local pos_right = pb_props[pb.B3].pos
local pos_center_lo = {(pos_left[1]+pos_right[1])/2, pos_left[2]}
local pos_center_hi = {pos_center_lo[1], pb_props[pb.B2].pos_up[2]}	
addText( "1/1",  pos_center_lo, tp_28_center_bottom, {{"MPD_COM_REC_PageNum"}, {"MPD_COM_REC_PageVisible"}}	)
addText( "PAGE",  pos_center_hi, tp_28_center_bottom, {{"MPD_COM_REC_PageVisible"}})

createMenu( Menu )
createControls( Controls )

---------------------------------------
local w = 1000	
local h = 800
local w0 = 10 - w/2 
local h0 = h/2 - tp_default.steep_y	
AddRoundCornersWindow("COM_MSG_REC",	{0, 0},
	w, h,
	{
		{"", { w0, h0},	tp_28, {{"MPD_COM_REVIEW_Text", 0}}},
		{"", { w0, h0 - tp_default.steep_y}, tp_28, {{"MPD_COM_REVIEW_Text", 1}}},
		{"", { w0, h0 - 2.0*tp_default.steep_y}, tp_28, {{"MPD_COM_REVIEW_Text", 2}}},
		{"", { w0, h0 - 3.0*tp_default.steep_y}, tp_28, {{"MPD_COM_REVIEW_Text", 3}}},
	},
	tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)