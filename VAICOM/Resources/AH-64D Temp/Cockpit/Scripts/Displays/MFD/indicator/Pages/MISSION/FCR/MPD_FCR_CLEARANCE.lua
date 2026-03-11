dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local OnOffPageClearanc = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 6}})
local VideoAreaPH = addPlaceholder("VideoAreaCenterClearance", video_area_pos, OnOffPageClearanc.name, nil)

local controls = {}
controls=
{

    { pb.R1,    "20",    	tp_default_border,               {{"DSPLS_FCR_AG_ShowClearanceTPM", 0}}  },
    { pb.R2,    "50",       tp_default_border,             	 {{"DSPLS_FCR_AG_ShowClearanceTPM", 1}}  },
    { pb.R3,    "100",    	tp_default_border,               {{"DSPLS_FCR_AG_ShowClearanceTPM", 2}}  },
    { pb.R4,    "200",    	tp_default_border,               {{"DSPLS_FCR_AG_ShowClearanceTPM", 3}}  },


    { pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},

    { pb.L1,  {{"PROF",         nil         },  {"GEOM", tp_default_border, {{"DSPLS_FCR_AG_ShowProfCaptionTPM"}}, { "GEOM", "ARITH", "TEST"}}}},
    { pb.L2,  {{"LINES",        nil         },  {"0", tp_default_border, {{"DSPLS_FCR_AG_ShowLineCaptionTPM"}}, { "0", "1", "2", "3", "4" }}}},

}

local function createVertsForBackground(verts_r, verts_dn)
	local caption_space =  tp_default.height / 8
	
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
		{verts_bg[1],	verts_bg[2], verts_bg[3],	{0, 0}},
		{{0,0},			verts_bg[4], verts_bg[5],	{0, 0}},
		{{0,0},			verts_bg[6], verts_bg[7],	{0, 0}},
		{verts_bg[8],	verts_bg[9], verts_bg[10],	{0, 0}}
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

local tp = tp_def_right
local c = {"R","R"}
local caption = "CLEARANCE"

local pos_up, pos_dn = { pb_props[pb.R1].pos[1] - 20, pb_props[pb.R1].pos[2] }, { pb_props[pb.R1].pos[1] - 20, pb_props[pb.R4].pos[2]}

local max_len = 3 * tp.width
local verts_r	= createVertsForLines( pos_up, pos_dn,  max_len, 1, "  ", c, tp )
local pos_caption = calculateCaptionPos( pos_up, pos_dn, max_len, 1, caption, c, tp )
local parent = nil

local caption_space =  tp.height / 10
verts_r[1][3][2] = pos_caption[2] + caption_space - 20
verts_r[2][3][2] = pos_caption[2] - #caption*tp.height - caption_space 

local verts_dn = { {0, 0}, {0, 0}, {0, 0}, {0, 0} }
verts_dn[1] = verts_r[2][3]
verts_dn[2] = {407,-110}
verts_dn[3] = {pb_props[pb.R4].pos[1] * tp.width, pos_dn[2] }
verts_dn[4] = {407,-110}

local verts_bckgnd = createVertsForBackground(verts_r[1], verts_dn)
addTranslucentBackground( verts_bckgnd[1], default_box_indices, VideoAreaPH.name, 1, h_clip_relations.INCREASE_LEVEL  )
addTranslucentBackground( verts_bckgnd[2], default_box_indices, VideoAreaPH.name, 1, h_clip_relations.INCREASE_LEVEL  )
addTranslucentBackground( verts_bckgnd[3], default_box_indices, VideoAreaPH.name, 1, h_clip_relations.INCREASE_LEVEL  )
addTranslucentBackground( verts_bckgnd[4], default_box_indices, VideoAreaPH.name, 1, h_clip_relations.INCREASE_LEVEL  )

-- draw upper line
draw_line( verts_r[1], tp.material, VideoAreaPH.name, nil, nil, nil, h_clip_relations.INCREASE_LEVEL, DEFAULT_LEVEL )

-- draw lower line
draw_line( verts_dn, tp.material, VideoAreaPH.name, nil, nil, nil, h_clip_relations.INCREASE_LEVEL, DEFAULT_LEVEL )

-- draw caption																		
for i = 1, #caption do
	addText( string.sub(caption, i, i).."\n", {pos_caption[1], pos_caption[2] }, tp, nil, nil, nil, nil, VideoAreaPH.name, h_clip_relations.INCREASE_LEVEL, DEFAULT_LEVEL )
	pos_caption = {pos_caption[1], pos_caption[2] - tp.height}
end

createControls(controls, nil, OnOffPageClearanc.name)