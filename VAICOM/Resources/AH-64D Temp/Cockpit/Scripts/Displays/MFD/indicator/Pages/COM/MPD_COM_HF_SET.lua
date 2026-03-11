dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T3, "XPNDR",	tp_default_border, {{"MFD_COMMUNICATION_Menu", 2, pb.T3}}},
	{ pb.T4, "UHF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 3, pb.T4}}},
	{ pb.T5, "FM",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 4, pb.T5}}},
	{ pb.T6, "HF",		tp_default_border, {{"MFD_COMMUNICATION_Menu", 5, pb.T6}}},
	{ pb.B5, "SET",		tp_default_border },
}

createMenu( Menu )

function Add_COMM_HF_SET_Page_Crypto_RangeArrows(controller)
	local group_pos	= {pb_props[pb.R3].pos[1], (pb_props[pb.R3].pos[2] + pb_props[pb.R4].pos[2])/2 - tp_default.height*0.75}	
	local R3R4MenuLabelBase = addPlaceholder("COM_R3R4Menu_plaseholder", group_pos, nil, controller)
	local font_size = tp_default.height
	
	AddArrowMenuLabel(pb.R3, "R3R4Menu_Arr1",	{-font_size*0.5, font_size*2.1},	nil,	R3R4MenuLabelBase.name)
	AddArrowMenuLabel(pb.R4, "R3R4Menu_Arr2",	{-font_size*0.5, -font_size*2.1},	180,	R3R4MenuLabelBase.name)
	
	--local elem = addText("",	{0,0},	tp_def_right_center, {{"MPD_COMM_CRYPTO_ARROW"}}, nil, nil, "R3R4_text_lbl",	R3R4MenuLabelBase.name)
end

function draw_poly_line_with_caption( pos_up, pos_dn,  max_item_len, num_lines, caption, tp, parent )
	max_item_len = (max_item_len + 1) * tp.width											
			
	local c = parseAligment ( tp.alignment )
	local verts				= { { pos_up, {0, 0}, {0, 0} }, { pos_dn, {0, 0}, {0, 0} } }		
	local caption_space
	
	verts[1][1] = { video_area_w_05, verts[1][1][2] + 1.5 * tp.height }		
	verts[2][1] = { pos_dn[1] - max_item_len- 3.5*tp.width, verts[2][1][2] - 5.5 * tp.height }		
	verts[1][2] = { pos_up[1] - max_item_len, verts[1][1][2] }				
	verts[2][2] = { pos_dn[1] - max_item_len, verts[2][1][2] }				
	local caption_space = ( verts[1][1][2] - verts[2][1][2] - #caption * tp.height )/2
	verts[1][3] = { verts[1][2][1], verts[1][2][2] - caption_space }		
	verts[2][3] = { verts[2][2][1], verts[2][2][2] + caption_space }		
	
	local pos_caption = calculateCaptionPos( pos_up, pos_dn, max_item_len, num_lines, caption, c, tp )

	caption_space = tp.height/8
	verts[1][3][2] = pos_caption[2] + caption_space
	verts[2][3][2] = pos_caption[2] - #caption*tp.height - caption_space	

	local verts_bckgnd = createVertsForBackgroundWithCaption( verts, c )
	
	addTranslucentBackground( verts_bckgnd[1], default_box_indices, parent, nil, nil )
	addTranslucentBackground( verts_bckgnd[2], default_box_indices, parent, nil, nil )
	addTranslucentBackground( verts_bckgnd[3], default_box_indices, parent, nil, nil )

	for i = 1, #caption do
		addText( string.sub(caption, i, i).."\n", {pos_caption[1], pos_caption[2]},  tp, nil, nil, nil, nil, parent, h_clip_relations.INCREASE_LEVEL, DEFAULT_LEVEL )
		pos_caption = {pos_caption[1], pos_caption[2] - tp.height }
	end
	
	draw_line( verts[1], tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )
	draw_line( verts[2], tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )
	draw_line( {{verts[2][1][1], -video_area_h_05},{verts[2][1][1], verts[2][1][2]} }, tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )
end

function create_HF_SET_Page_Crypto_Controls( controls, drawBorder, m_parent, draw_text_background )
	local caption
	local max_item_len = 0
	local dtb = draw_text_background or 1
	
	for item = 1, #controls do
		if type(controls[item][1]) == "number" then
			caption = createControlItem( controls[item], drawBorder, m_parent, dtb )						
		
		elseif type(controls[item][1]) == "string" then
			
			caption					= controls[item][1]
			local inner_controls	= controls[item][2]
			local controllers		= controls[item][3]
			local pbs				= { inner_controls[1][1], inner_controls[#inner_controls][1] }		
			local tp				= pb_props[pbs[1]].tp													
			local pos_up			= pb_props[pbs[1]].pos
			local pos_dn			= pb_props[pbs[2]].pos
			local parent			= m_parent or inner_controls[1][6]
			
			local max_item_len, num_lines = calcMaxItemLen( inner_controls )
			
			if controllers ~= nil then
				parent = addPlaceholder(nil, {0,0}, nil, controllers)
				draw_poly_line_with_caption( pos_up, pos_dn,  max_item_len, num_lines, caption, tp, parent.name )
				createControls( inner_controls, true, parent) 
			else
				draw_poly_line_with_caption( pos_up, pos_dn,  max_item_len, num_lines, caption, tp, parent )
				createControls( inner_controls, true, parent) 
			end
		end 
	end
	Add_COMM_HF_SET_Page_Crypto_RangeArrows()
end

local Controls = {}
Controls = 
{		
	{ pb.L1, "LISTEN BEFORE CALL",	nil,  {{"MFD_COMM_HF_SET_Buttons", 5}}, {"{LISTEN BEFORE CALL", " LISTEN BEFORE CALL"}}, --TODO::
	{ pb.L2, "LISTEN BEFORE TALK",	nil,  {{"MFD_COMM_HF_SET_Buttons", 5}}, {"{LISTEN BEFORE TALK", " LISTEN BEFORE TALK"}}, --TODO::  
	{ pb.L3, "LINK PROTECT",		nil,  {{"MFD_COMM_HF_SET_Buttons", 5}}, {"{LINK PROTECT", " LINK PROTECT"}},       
	{ pb.L5, {{"POWER",   			nil, nil}, {"NORM", tp_default_border, {{"MFD_COMM_HF_SET_Buttons", 3}}, {"LOW", "MEDIUM", "HIGH"}} } },		
	{ pb.L6, {{"SQUELCH",   		nil, nil}, {"1", 	tp_default_border, {{"MFD_COMM_HF_SET_Buttons", 4}}, {"1", "2", "3", "4", "5"}} } },			
	{ "CRYPTO",
				{
					{ pb.R1,  {{"MODE",     nil, {{"MFD_COMM_HF_SET_Buttons", 0}}}, {"CIPHER", tp_default_border, {{"MFD_COMM_HF_SET_Buttons", 1}}, {"CIPHER", "PLAIN", "RECEIVE", "OFFLINE"}} } },					
					{ pb.R2,  {{"CNV",      nil, {{"MFD_COMM_HF_SET_Buttons", 0}}}, {"1", tp_default_border, {{"MFD_COMM_HF_SET_Buttons", 2}}, {"1", "2", "3", "4", "5", "6"}} } },	
					{ pb.R5,  {{"BACK",     nil, {{"MFD_COMM_HF_SET_Buttons", 0}}}, {"", nil }}, nil  },	--TODO::	
					{ pb.R6,  {{"INITIATE", nil, {{"MFD_COMM_HF_SET_Buttons", 0}}}, {"", nil }}, nil  },	--TODO::	
				}
	},	
	{ pb.B3, {{"FILL", 		nil },{"INFO",	nil, nil }}, nil, nil},--TODO::
	{ pb.B6,   "BYPASS", 	nil}, --TODO::
	{ pb.B1, "", tp_default_border, {{"MFD_COMM_OriginatorFmt", pb.B1}}, {"COM", "DL", "XPNDR", "UHF", "FM", "HF", "COM"} }	
}

AddRoundCornersWindow("CRYPTO_Status_Window",	{0.0,((pb_props[pb.L4].pos[2])) }, tp_default.width*12.0, tp_default.height*9.0,
						{
							{"CRYPTO:",				{15.0,									tp_default.height*10.0*0.5 - 40},		tp_def_right_center},
							{"CIPHER",				{-tp_default.width*15.0*0.5 + 53,		tp_default.height*10.0*0.5 - 75},		tp_def_left_center},
							{"AAAAAA",				{-tp_default.width*15.0*0.5 + 53,		tp_default.height*10.0*0.5 - 115},		tp_def_left_center,	{{"MFD_COMM_HF_SET_CRYPTO_Status_Window_Data", 0}}},
							{"XMIT VOICE",			{-tp_default.width*15.0*0.5 + 53,		tp_default.height*10.0*0.5 - 155},		tp_def_left_center,	{{"MFD_COMM_HF_SET_CRYPTO_Status_Window_Data", 1}}},
							{"16.0K WB",			{-tp_default.width*15.0*0.5 + 53,		tp_default.height*10.0*0.5 - 195},		tp_def_left_center,	{{"MFD_COMM_HF_SET_CRYPTO_Status_Window_Data", 2}}},
							{"MENU LOCK",			{-tp_default.width*15.0*0.5 + 53,		tp_default.height*10.0*0.5 - 235},		tp_def_left_center,	{{"MFD_COMM_HF_SET_CRYPTO_Status_Window_Data", 3}}},												
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)	

create_HF_SET_Page_Crypto_Controls( Controls )

