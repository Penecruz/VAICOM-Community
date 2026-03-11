dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/FCR/MPD_FCR_Defs.lua")

local OnOffPageACQ = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_AcqSubset"}})
-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.R1, "PHS",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection",0}, {"DSPLS_TSD_ACQ_Source_Valid", 0 }}, {"?PHS","PHS"} }, 
	{ pb.R2, "GHS",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection",1}, {"DSPLS_TSD_ACQ_Source_Valid", 1 }}, {"?GHS","GHS"} }, 
	{ pb.R3, "SKR",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection",2}, {"DSPLS_TSD_ACQ_Source_Valid", 2 }}, {"?SKR","SKR"} }, 
	{ pb.R4, "RFI",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection",3}, {"DSPLS_TSD_ACQ_Source_Valid", 3 }}, {"?RFI","RFI"} },  
	{ pb.R5, "FCR",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection",4}, {"DSPLS_TSD_ACQ_Source_Valid", 4 }}, {"?FCR","FCR"} }, 
	{ pb.R6, "FXD",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection",5}} }, 

	{ pb.B6, "TADS",tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection",6}, {"DSPLS_TSD_ACQ_Source_Valid", 6 }},{"?TADS","TADS"} },
	{ pb.B5, "?00",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection",7}, {"DSPLS_TSD_ACQ_TXX_Selected_Caption"}}},
	{ pb.B4, "TRN",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection",8}, {"DSPLS_TSD_ACQ_TRN_Selected_Caption"}}},
	--{ pb.B3, "ASE",	tp_default_border, {{"TSD_ACQ_Button_Selection",9}} },
}


local function createVertsForBackground(verts_r, verts_dn)
	local caption_space =  tp_default.height/8
	
	local verts_bg	=		-- united form of menu background
	{
		{verts_r[1][1],verts_r[1][2]},
		{verts_r[2][1],verts_r[2][2]},
		
		{verts_r[3][1],verts_r[3][2] - caption_space},
		{verts_r[3][1],verts_r[3][2] - caption_space},
		
		{verts_dn[1][1],verts_dn[1][2] + caption_space},
		{verts_dn[1][1],verts_dn[1][2] + caption_space},
		
		{verts_dn[2][1],verts_dn[2][2]},
		{verts_dn[3][1],verts_dn[3][2]},
		{verts_dn[4][1],verts_dn[4][2]},
		
		{verts_r[1][1],verts_dn[4][2]}
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
	verts_out[1][1][1] = display_size_pix_05
	verts_out[1][1][2] = verts_out[1][1][2] + offsetLine
	verts_out[1][2][1] = verts_out[1][2][1] - offsetLine
	verts_out[1][2][2] = verts_out[1][1][2]
	verts_out[1][3][1] = verts_out[1][2][1]
	verts_out[1][4] = {verts_out[1][1][1],verts_out[1][3][2]}
	
	verts_out[2][1] = verts_out[1][4]
	verts_out[2][2][1] = verts_out[2][2][1] + offsetX
	verts_out[2][3][1] = verts_out[2][2][1]
	verts_out[2][4] = {verts_out[1][1][1],verts_out[2][3][2]}
	
	verts_out[3][1] = verts_out[2][4]
	verts_out[3][2][1] = verts_out[1][2][1]
	verts_out[3][3][1] = verts_out[1][2][1]
	verts_out[3][3][2] = verts_out[3][3][2] + offsetLine
	verts_out[3][4] = {verts_out[1][1][1],verts_out[3][3][2]}
	
	verts_out[4][1][1] = verts_out[4][1][1] - offsetLine
	verts_out[4][1][2] = verts_out[3][3][2]
	verts_out[4][2][1] = verts_out[4][1][1]
	verts_out[4][2][2] = -display_size_pix_05
	verts_out[4][3][1] = verts_out[1][1][1]
	verts_out[4][3][2] = verts_out[4][2][2]
	verts_out[4][4] = verts_out[3][4]

   return verts_out
end

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

--- like draw_border_with_caption() ---

local tp = tp_def_right
local c = {"R","R"}
local caption = "ACQUISITION"

local pos_up, pos_dn = pb_props[pb.R1].pos, { pb_props[pb.R6].pos[1], pb_props[pb.R6].pos[2] - 75 }

local max_len = 5 * tp.width
local verts_r	= createVertsForLines( pos_up, pos_dn,  max_len, 1, "  ", c, tp )
local pos_caption = calculateCaptionPos( pos_up, pos_dn, max_len, 1, caption, c, tp )
local parent = OnOffPageACQ.name

local caption_space =  tp.height/8
verts_r[1][3][2] = pos_caption[2] + caption_space
verts_r[2][3][2] = pos_caption[2] - #caption*tp.height - caption_space

local verts_dn = { {0, 0}, {0, 0}, {0, 0}, {0, 0} }
verts_dn[1] = verts_r[2][3]
verts_dn[2] = verts_r[2][2]
verts_dn[3] = {pb_props[pb.B4].pos[1] - 3.0 * tp.width, pos_dn[2] - 2.5 * tp.height}
verts_dn[4] = {verts_dn[3][1], -display_size_pix_05}

local verts_bckgnd = createVertsForBackground(verts_r[1], verts_dn)
addTranslucentBackground( verts_bckgnd[1], default_box_indices, parent, nil, nil )
addTranslucentBackground( verts_bckgnd[2], default_box_indices, parent, nil, nil )
addTranslucentBackground( verts_bckgnd[3], default_box_indices, parent, nil, nil )
addTranslucentBackground( verts_bckgnd[4], default_box_indices, parent, nil, nil )

-- draw upper line
draw_line( verts_r[1], tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )

-- draw lower line
draw_line( verts_dn, tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )

-- draw caption																		
for i = 1, #caption do
	addText( string.sub(caption, i, i).."\n", {pos_caption[1], pos_caption[2]}, tp, nil, nil, nil, nil, parent, h_clip_relations.INCREASE_LEVEL, DEFAULT_LEVEL )
	pos_caption = {pos_caption[1], pos_caption[2] - tp.height }
end

-- draw menus

createControls( Controls, nil, OnOffPageACQ.name )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------