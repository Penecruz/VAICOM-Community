dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/FCR/MPD_FCR_Defs.lua")


local OnOffPageLINE = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 4}})
local VideoAreaPH = addPlaceholder("VideoAreaCenterLINE", {0, 45}, OnOffPageLINE.name, nil)

local controls = {}
controls=
{
    { pb.L1,    "0",    tp_default_border,              {{"DSPLS_FCR_AG_ShowLineTPM", 0}}},
    { pb.L2,    "1",    tp_default_border,              {{"DSPLS_FCR_AG_ShowLineTPM", 1}}},
    { pb.L3,    "2",    tp_default_border,              {{"DSPLS_FCR_AG_ShowLineTPM", 2}}},
	{ pb.L4,    "3",    tp_default_border,              {{"DSPLS_FCR_AG_ShowLineTPM", 3}}},
	{ pb.L5,    "4",    tp_default_border,              {{"DSPLS_FCR_AG_ShowLineTPM", 4}}},

    { pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
    { pb.R1,  {{"CLEARANCE",    nil         },  {"20", tp_default_border, {{"DSPLS_FCR_AG_ShowClearanceCaptionTPM"}}, { "20", "50", "100", "200" } }}},
    { pb.R2,  {{"ELEV",         nil         },  {"FAR", tp_default_border, {{"DSPLS_FCR_AG_ShowButtonElevationControlTPM", 1}}}}},
    { pb.R2,  {{"ELEV",         nil         },  {"NEAR", tp_default_border, {{"DSPLS_FCR_AG_ShowButtonElevationControlTPM", 0}}}}},
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
	verts_out[1][1][1] = -display_size_pix_05
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
	verts_out[4][2][2] = -display_size_pix_05 + 250
	verts_out[4][3][2] = verts_out[4][2][2]
	verts_out[4][4] = verts_out[3][4]

   return verts_out
end

local tp = tp_def_left
local c = {"L","L"}
local caption = "LINES"

local pos_up, pos_dn = { pb_props[pb.L1].pos[1]- 40, pb_props[pb.L1].pos[2] - 55}, { pb_props[pb.L1].pos[1], pb_props[pb.L4].pos[2] - 250}

local max_len =  4.5 * tp.width
local verts_r	= createVertsForLines( pos_up, pos_dn,  max_len, 1, "  ", c, tp )
local pos_caption = calculateCaptionPos( pos_up, pos_dn, max_len, 1, caption, c, tp )
local parent = nil

local caption_space =  tp.height / 8
verts_r[1][3][2] = pos_caption[2] + caption_space + 50
verts_r[2][3][2] = pos_caption[2] + #caption*tp.height + caption_space

local verts_dn = { {0, 0}, {0, 0}, {0, 0}, {0, 0} }
verts_dn[1] = {-437, -45}
verts_dn[2] = {-437, -45}
verts_dn[3] = {-437, -260}
verts_dn[4] = {-550, -260}

local verts_bckgnd = createVertsForBackground(verts_r[1], verts_dn)
addTranslucentBackground( verts_bckgnd[1], default_box_indices, VideoAreaPH.name, 1, h_clip_relations.INCREASE_LEVEL  )
addTranslucentBackground( verts_bckgnd[2], default_box_indices, VideoAreaPH.name, 1, h_clip_relations.INCREASE_LEVEL  )
addTranslucentBackground( verts_bckgnd[3], default_box_indices, VideoAreaPH.name, 1, h_clip_relations.INCREASE_LEVEL  )
addTranslucentBackground( verts_bckgnd[4], default_box_indices, VideoAreaPH.name, 1, h_clip_relations.INCREASE_LEVEL  )

-- draw upper line
draw_line( verts_r[1], tp.material, VideoAreaPH.name, nil, nil, nil, h_clip_relations.INCREASE_LEVEL , DEFAULT_LEVEL )

-- draw lower line
draw_line( verts_dn, tp.material, VideoAreaPH.name, nil, nil, nil, h_clip_relations.INCREASE_LEVEL , DEFAULT_LEVEL )

-- draw caption																		
for i = 1, #caption do
	addText( string.sub(caption, i, i).."\n", {pos_caption[1], pos_caption[2] + 50}, tp, nil, nil, nil, nil, VideoAreaPH.name, h_clip_relations.INCREASE_LEVEL, DEFAULT_LEVEL )
	pos_caption = {pos_caption[1], pos_caption[2] - tp.height}
end

createControls(controls, nil, OnOffPageLINE.name)
