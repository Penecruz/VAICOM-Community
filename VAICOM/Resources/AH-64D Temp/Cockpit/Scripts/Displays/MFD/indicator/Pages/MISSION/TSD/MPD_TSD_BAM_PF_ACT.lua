dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "TSD BAM PAGE PF ACT",  {0, 350}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.T4, "ASN",		nil,	nil},
	
	{ pb.T3, "NONE",	tp_default_border,	{{"TSD_BAM_PFZ_Active_Selection",-1}}},
	{ pb.T2, "PF1",		tp_default_border,	{{"TSD_BAM_PFZ_Active_Selection",0}}},
	{ pb.T1, "PF2",		tp_default_border,	{{"TSD_BAM_PFZ_Active_Selection",1}}},

	{ pb.L1, "PF3", 	tp_default_border,	{{"TSD_BAM_PFZ_Active_Selection",2}}},
	{ pb.L2, "PF4", 	tp_default_border,	{{"TSD_BAM_PFZ_Active_Selection",3}}},
	{ pb.L3, "PF5", 	tp_default_border,	{{"TSD_BAM_PFZ_Active_Selection",4}}},
	{ pb.L4, "PF6", 	tp_default_border,	{{"TSD_BAM_PFZ_Active_Selection",5}}},
	{ pb.L5, "PF7", 	tp_default_border,	{{"TSD_BAM_PFZ_Active_Selection",6}}},
	{ pb.L6, "PF8", 	tp_default_border,	{{"TSD_BAM_PFZ_Active_Selection",7}}},
}

pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + T3T4_posShiftX*1.0
pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - T3T4_posShiftX*1.2


local function createVertsForBackground(verts_l, verts_up)
	local caption_space =  tp_default.height/8
	
	local verts_bg	=		-- united form of menu background
	{
		{verts_l[1][1],verts_l[1][2]},
		{verts_l[2][1],verts_l[2][2]},
		
		{verts_l[3][1],verts_l[3][2] + caption_space},
		{verts_l[3][1],verts_l[3][2] + caption_space},
		
		{verts_up[1][1],verts_up[1][2] - caption_space},
		{verts_up[1][1],verts_up[1][2] - caption_space},
		
		{verts_up[2][1],verts_up[2][2]},
		{verts_up[3][1],verts_up[3][2]},
		{verts_up[4][1],verts_up[4][2]},
		
		{verts_l[1][1],verts_up[4][2]}
	}
	
	local verts_out = 		-- same background but separated into rectangle
	{
		{verts_bg[1],	verts_bg[2], verts_bg[3],	{0,0}},
		{{0,0},			verts_bg[4], verts_bg[5],	{0,0}},
		{{0,0},			verts_bg[6], verts_bg[7],	{0,0}},
		{verts_bg[8],	verts_bg[9], verts_bg[10],	{0,0}}
	}
   
   local offsetX, offsetLine = 3, info_box_line_width / 2
   
	-- expand background to the border of display and add offset for caption to not overlap background
	verts_out[1][1][1] = -display_size_pix_05
	verts_out[1][1][2] = verts_out[1][1][2] - offsetLine
	verts_out[1][2][1] = verts_out[1][2][1] + offsetLine
	verts_out[1][2][2] = verts_out[1][1][2]
	verts_out[1][3][1] = verts_out[1][2][1]
	verts_out[1][4] = {verts_out[1][1][1],verts_out[1][3][2]}
	
	verts_out[2][1] = verts_out[1][4]
	verts_out[2][2][1] = verts_out[2][2][1] - offsetX
	verts_out[2][3][1] = verts_out[2][2][1]
	verts_out[2][4] = {verts_out[1][1][1],verts_out[2][3][2]}
	
	verts_out[3][1] = verts_out[2][4]
	verts_out[3][2][1] = verts_out[1][2][1]
	verts_out[3][3][1] = verts_out[1][2][1]
	verts_out[3][3][2] = verts_out[3][3][2] - offsetLine
	verts_out[3][4] = {verts_out[1][1][1],verts_out[3][3][2]}
	
	verts_out[4][1][1] = verts_out[4][1][1] + offsetLine
	verts_out[4][1][2] = verts_out[3][3][2]
	verts_out[4][2][1] = verts_out[4][1][1]
	verts_out[4][2][2] = display_size_pix_05
	verts_out[4][3][1] = verts_out[1][1][1]
	verts_out[4][3][2] = verts_out[4][2][2]
	verts_out[4][4] = verts_out[3][4]

   return verts_out
end


-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------


--- like draw_border_with_caption() ---

local tp = tp_def_left
local c = {"L","L"}
local caption = "ACTIVE"

local pos_up, pos_dn = { pb_props[pb.L1].pos[1], pb_props[pb.L1].pos[2] + 75 }, pb_props[pb.L6].pos

local max_len = 3.5 * tp.width
local verts_l	= createVertsForLines( pos_up, pos_dn,  max_len, 1, "  ", c, tp )
local pos_caption = calculateCaptionPos( pos_up, pos_dn, max_len, 1, caption, c, tp )
local parent = nil

local caption_space =  tp.height/8
verts_l[1][3][2] = pos_caption[2] + caption_space
verts_l[2][3][2] = pos_caption[2] - #caption*tp.height - caption_space

local verts_up = { {0, 0}, {0, 0}, {0, 0}, {0, 0} }
verts_up[1] = verts_l[1][3]
verts_up[2] = verts_l[1][2]
verts_up[3] = {pb_props[pb.T3].pos[1] + 3.0 * tp.width,	verts_l[1][2][2]}
verts_up[4] = {verts_up[3][1], display_size_pix_05}

local verts_bckgnd = createVertsForBackground(verts_l[2], verts_up)
addTranslucentBackground( verts_bckgnd[1], default_box_indices, parent, nil, nil )
addTranslucentBackground( verts_bckgnd[2], default_box_indices, parent, nil, nil )
addTranslucentBackground( verts_bckgnd[3], default_box_indices, parent, nil, nil )
addTranslucentBackground( verts_bckgnd[4], default_box_indices, parent, nil, nil )

-- draw upper line
draw_line( verts_up, tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )

-- draw lower line
draw_line( verts_l[2], tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )

-- draw caption																		
for i = 1, #caption do
	addText( string.sub(caption, i, i).."\n", {pos_caption[1], pos_caption[2]}, tp, nil, nil, nil, nil, parent, h_clip_relations.INCREASE_LEVEL, DEFAULT_LEVEL )
	pos_caption = {pos_caption[1], pos_caption[2] - tp.height }
end

-- draw menus

createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = T3_pocket
pb_props[pb.T4].pos = T4_pocket

