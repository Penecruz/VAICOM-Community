dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/FCR/MPD_FCR_Defs.lua")

local OnOffPageRFHO = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 2}})

local controls = {}
controls=
{
	{ pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
    { pb.L1, "N\nT\nS", 	nil,	            nil },
    { pb.L2, "c", 	        tp_52,	            nil},
    { pb.L4, "T\nG\nT",		nil,	            nil},
    { pb.L5,  {{"ELEV",     nil,                {{"DSPLS_FCR_AG_ShowButtonElevationControl", 1}}}, {"AUTO", tp_default_border, {{"DSPLS_FCR_AG_ShowButtonElevationControl", 1}}}}},
    { pb.L5, "a",           tp_52,              {{"DSPLS_FCR_AG_ShowButtonElevationControl", 0}, {"DSPLS_FCR_OnOffShowButtonElevationGTM", 1}, {"DSPLS_FCR_OnOffShowButtonElevationATM", 1}}},
    { pb.L6, "b",           tp_52,              {{"DSPLS_FCR_AG_ShowButtonElevationControl", 0}, {"DSPLS_FCR_OnOffShowButtonElevationGTM", 0}, {"DSPLS_FCR_OnOffShowButtonElevationATM", 0}}},
}

local Controls_Subscribers = {}
Controls_Subscribers = 
{
	{ pb.T5, "L07",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 0}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 0}}},
	{ pb.T6, "L33",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 1}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 1}}},

	{ pb.R1, "L40",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 2}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 2}}},
	{ pb.R2, "L52",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 3}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 3}}},
	{ pb.R3, "L14",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 4}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 4}}},
	{ pb.R4, "L88",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 5}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 5}}},
	{ pb.R5, "L09",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 6}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 6}}},
}

pb_props[pb.R6].tp = createTextProperty( nil, "WHITE", IND_MPD_MATERIAL_WHITE, "RightTop" )

local GTM_PH = addPlaceholder("GTM_PH", nil, OnOffPageRFHO.name, {{"DSPLS_FCR_GTM_Root"}})


add_missile_constraints_box(GTM_PH.name)
AddSendBtn("FCR_RFHO_SendMessageWindow", GTM_PH.name, {{"DSPLS_FCR_RHFO_Send_Button_Show"}})

local tp = tp_def_right
local c = {"R","T"}
local caption = "RFHO"

local pos_up, pos_dn = {pb_props[pb.R1].pos[1], pb_props[pb.R1].pos[2] + 55}, { pb_props[pb.R5].pos[1], pb_props[pb.R5].pos[2] }

local max_len = 5 * tp.width
local verts_r	= createVertsForLines( pos_up, pos_dn,  max_len, 1, "  ", c, tp )
local pos_caption = calculateCaptionPos( pos_up, pos_dn, max_len, 1, caption, c, tp )
local parent = OnOffPageRFHO.name

local caption_space =  tp.height/8
verts_r[1][3][2] = pos_caption[2] + caption_space
verts_r[2][3][2] = pos_caption[2] - #caption*tp.height - caption_space

local verts_top = { {0, 0}, {0, 0}, {0, 0}, {0, 0} }
verts_top[1] = verts_r[1][3]
verts_top[2] = verts_r[1][2]
verts_top[3] = {pb_props[pb.T5].pos[1] - 3.0 * tp.width, verts_r[1][1][2]}
verts_top[4] = {verts_top[3][1], display_size_pix_05}

local function createVertsForBackground(verts_r, verts_top)
	local caption_space =  tp_default.height/8
	
	local verts_bg	=		-- united form of menu background
	{
		{verts_r[1][1],verts_r[1][2]},--1
		{verts_r[2][1],verts_r[2][2]},--2
		
		{verts_r[3][1],verts_r[3][2] - caption_space},--3
		{verts_r[3][1],verts_r[3][2] - caption_space},--4
		
		{verts_top[1][1],verts_top[1][2] + caption_space},--5
		{verts_top[1][1],verts_top[1][2] + caption_space},--6
		
		{verts_top[2][1],verts_top[2][2]},--7

		{verts_top[3][1],verts_top[3][2]},--8
		{verts_top[4][1],verts_top[4][2]},--9
		
		{verts_r[1][1],verts_top[4][2]}--10
	}
	
	local verts_out = 		-- same background but separated into rectangle
	{
		{verts_bg[1],	verts_bg[2], verts_bg[3],	{0,0}},
		{{0,0},			verts_bg[4], verts_bg[5],	{0,0}},
		{{0,0},			verts_bg[6], verts_bg[7],	{0,0}},
		--{verts_bg[8],	verts_bg[9], verts_bg[10],	{0,0}}
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
	verts_out[4][2][2] = display_size_pix_05
	verts_out[4][3][1] = verts_out[1][1][1]
	verts_out[4][3][2] = verts_out[4][2][2]
	verts_out[4][4] = verts_out[3][4]

   return verts_out
end

local verts_bckgnd = createVertsForBackground(verts_r[2], verts_top)
addTranslucentBackground( verts_bckgnd[1], default_box_indices, parent, nil, nil)
addTranslucentBackground( verts_bckgnd[2], default_box_indices, parent, nil, nil)
addTranslucentBackground( verts_bckgnd[3], default_box_indices, parent, nil, nil)
addTranslucentBackground( verts_bckgnd[4], default_box_indices, parent, nil, nil)

-- draw upper line
draw_line( verts_top, tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )

-- draw lower line
draw_line( verts_r[2], tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )

-- draw caption																		
for i = 1, #caption do
	addText( string.sub(caption, i, i).."\n", {pos_caption[1], pos_caption[2]}, tp, nil, nil, nil, nil, parent, h_clip_relations.INCREASE_LEVEL, DEFAULT_LEVEL )
	pos_caption = {pos_caption[1], pos_caption[2] - tp.height }
end

local pos_y_R1 = 35
local pos_y_L1 = 25 -- for L4
local pos_y_dl_v = 10
local pos_x_dl_v_R2 = 33
local pos_x_dl_v_L2 = 10
local pos_y = 5

local verticesR2 = {{-5, -50}, {-5,  35}, {-32,  35}, {-32, -50}}
draw_dl_v( {pb_props[pb.R2].pos[1] + pos_x_dl_v_R2, pb_props[pb.R2].pos[2] - pos_y_dl_v}, verticesR2, IND_MPD_1024_MATERIAL_DARK_GREEN, nil, {{"DSPLS_PB_Selectabled", pb.R2}} )

local verticesL2 = 	{{-5, -50}, {-5,  35}, {-32,  35}, {-32, -50}} 
draw_dl_v( {pb_props[pb.L2].pos[1] - pos_x_dl_v_L2, pb_props[pb.L2].pos[2] - pos_y_dl_v}, verticesL2, IND_MPD_1024_MATERIAL_DARK_GREEN, nil, {{"DSPLS_PB_Selectabled", pb.L2}} )

pb_props[pb.R1].pos[2] = pb_props[pb.R1].pos[2] + pos_y_R1

pb_props[pb.L1].pos[2] = pb_props[pb.L1].pos[2] + pos_y_L1
pb_props[pb.L2].pos[1] = pb_props[pb.L2].pos[1] - pos_y_dl_v

pb_props[pb.L2].pos[2] = pb_props[pb.L2].pos[2] + pos_y
pb_props[pb.R2].pos[2] = pb_props[pb.R2].pos[2] + pos_y

pb_props[pb.L4].pos[2] = pb_props[pb.L4].pos[2] + pos_y_L1

createControls( controls, nil , OnOffPageRFHO.name )
createControls( Controls_Subscribers, nil, OnOffPageRFHO.name )

pb_props[pb.R1].pos[2] = pb_props[pb.R1].pos[2] - pos_y_R1
pb_props[pb.L1].pos[2] = pb_props[pb.L1].pos[2] - pos_y_L1
pb_props[pb.L2].pos[1] = pb_props[pb.L2].pos[1] + pos_y_dl_v
pb_props[pb.L2].pos[2] = pb_props[pb.L2].pos[2] - pos_y
pb_props[pb.R2].pos[2] = pb_props[pb.R2].pos[2] - pos_y
pb_props[pb.L4].pos[2] = pb_props[pb.L4].pos[2] - pos_y_L1
