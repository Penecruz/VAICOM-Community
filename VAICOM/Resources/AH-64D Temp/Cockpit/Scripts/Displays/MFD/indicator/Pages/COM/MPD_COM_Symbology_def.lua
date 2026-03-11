
function draw_poly_line_with_caption( pos_up, pos_dn,  max_item_len, num_lines, caption, tp, parent )
	local verts				= { { pos_up, {0, 0}, {0, 0} }, { pos_dn, {0, 0}, {0, 0} } }		
	local caption_space		
	local c = parseAligment ( tp.alignment )	
	
	max_item_len = (max_item_len + 1) * tp.width
	
	if c[1] == "L"  then
		verts[1][1] = { -video_area_w_05, verts[1][1][2] + 1.5 * tp.height }	
		verts[2][1] = { -video_area_w_05, verts[2][1][2] - 3.0 * tp.height }	
		verts[1][2] = { pos_up[1] + max_item_len + 10, verts[1][1][2] }			
		verts[2][2] = { pos_dn[1] + max_item_len + 10, verts[2][1][2] }			
		caption_space = ( verts[1][1][2] - verts[2][1][2] - #caption * tp.height )/2
		verts[1][3] = { verts[1][2][1], verts[1][2][2] - caption_space }		
		verts[2][3] = { verts[2][2][1], verts[2][2][2] + caption_space }		

	elseif c[1] =="R" then
		verts[1][1] = { video_area_w_05, verts[1][1][2] + 1.5 * tp.height }		
		verts[2][1] = { video_area_w_05, verts[2][1][2] - 3.0 * tp.height }		
		verts[1][2] = { pos_up[1] - max_item_len, verts[1][1][2] }				
		verts[2][2] = { pos_dn[1] - max_item_len, verts[2][1][2] }				
		caption_space = ( verts[1][1][2] - verts[2][1][2] - #caption * tp.height )/2
		verts[1][3] = { verts[1][2][1], verts[1][2][2] - caption_space }		
		verts[2][3] = { verts[2][2][1], verts[2][2][2] + caption_space }		
	end
	
	local pos_caption = calculateCaptionPos( pos_up, pos_dn, max_item_len, num_lines, caption, c, tp )

	if  c[1] == "L" or c[1] == "R" then
		caption_space = tp.height/8
		verts[1][3][2] = pos_caption[2] + caption_space
		verts[2][3][2] = pos_caption[2] - #caption*tp.height - caption_space
	else
		caption_space = (#caption*tp.width)/2 + tp.width/8
		verts[1][3][1] = pos_caption[1] - caption_space
		verts[2][3][1] = pos_caption[1] + caption_space
	end

	local verts_bckgnd = createVertsForBackgroundWithCaption( verts, c )
	
	addTranslucentBackground( verts_bckgnd[1], default_box_indices, parent, nil, nil )
	addTranslucentBackground( verts_bckgnd[2], default_box_indices, parent, nil, nil )
	addTranslucentBackground( verts_bckgnd[3], default_box_indices, parent, nil, nil )
	
	if  c[1] == "L" or c[1] == "R" then																		
		for i = 1, #caption do
			addText( string.sub(caption, i, i).."\n", {pos_caption[1], pos_caption[2]},  tp, nil, nil, nil, nil, parent, h_clip_relations.INCREASE_LEVEL, DEFAULT_LEVEL )
			pos_caption = {pos_caption[1], pos_caption[2] - tp.height }
		end
	else 
		addText( caption, pos_caption,  tp, nil, nil, nil, nil, parent, h_clip_relations.INCREASE_LEVEL, DEFAULT_LEVEL )
	end 
	
	draw_line( verts[1], tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )
	draw_line( verts[2], tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )
	
end

function create_COMM_Controls( controls, drawBorder, m_parent, draw_text_background )
	local caption
	local max_item_len = 0
	local dtb = draw_text_background or 1
	
	for item = 1, #controls do
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
				createControls( inner_controls, true, parent, 0 ) 
			else
				draw_poly_line_with_caption( pos_up, pos_dn,  max_item_len, num_lines, caption, tp, parent )
				createControls( inner_controls, true, parent, 0 ) 
			end
	end	
end

function joinPresetBtns(table)
	local alignment_r = pb_props[pb.R1].tp.alignment
	pb_props[pb.R1].tp.alignment = "LeftTop"
	pb_props[pb.R2].tp.alignment = "LeftTop"
	pb_props[pb.R3].tp.alignment = "LeftTop"
	pb_props[pb.R4].tp.alignment = "LeftTop"
	pb_props[pb.R5].tp.alignment = "LeftTop"
	
	table[#table + 1] = { pb.L1, {{"", 	nil, {{"MFD_COMM_FirstLine_1", 0}}}, {"",	nil, {{"MFD_COMM_SecondLine", 0}}} } }
	table[#table + 1] =	{ pb.L2, {{"", 	nil, {{"MFD_COMM_FirstLine_1", 1}}}, {"", 	nil, {{"MFD_COMM_SecondLine", 1}}} } }
	table[#table + 1] = { pb.L3, {{"",  nil, {{"MFD_COMM_FirstLine_1", 2}}}, {"", 	nil, {{"MFD_COMM_SecondLine", 2}}} } }
	table[#table + 1] = { pb.L4, {{"",  nil, {{"MFD_COMM_FirstLine_1", 3}}}, {"", 	nil, {{"MFD_COMM_SecondLine", 3}}} } }
	table[#table + 1] = { pb.L5, {{"",	nil, {{"MFD_COMM_FirstLine_1", 4}}}, {"",	nil, {{"MFD_COMM_SecondLine", 4}}} } }
	
--	table[#table + 1] = { pb.R1, {{"",	nil, {{"MFD_COMM_FirstLine_1", 5}}}, {"",	nil, nil } } }
--	table[#table + 1] = { pb.R2, {{"",	nil, {{"MFD_COMM_FirstLine_1", 6}}}, {"",	nil, nil } } }
--	table[#table + 1] = { pb.R3, {{"",	nil, {{"MFD_COMM_FirstLine_1", 7}}}, {"",	nil, nil } } }
--	table[#table + 1] = { pb.R4, {{"",	nil, {{"MFD_COMM_FirstLine_1", 8}}}, {"",	nil, nil } } }
--	table[#table + 1] = { pb.R5, {{"",	nil, {{"MFD_COMM_FirstLine_1", 9}}}, {"",	nil, nil } } }
		
	pb_props[pb.R1].tp.alignment = alignment_r
	pb_props[pb.R2].tp.alignment = alignment_r
	pb_props[pb.R3].tp.alignment = alignment_r
	pb_props[pb.R4].tp.alignment = alignment_r
	pb_props[pb.R5].tp.alignment = alignment_r

	table[#table + 1] = { pb.R6, {{"XPNDR MASTER",	nil }, { "NORM", tp_default_border, {{"MFD_COMM_XPNDR_Buttons", 1}}, {"", "NORM", "STBY"} }}, nil}
end

function addPresetBorders()
	addBorder( pb.L1, 250, {{"MFD_COMM_PresetBorder", 1}} )
	addBorder( pb.L2, 250, {{"MFD_COMM_PresetBorder", 2}} )
	addBorder( pb.L3, 250, {{"MFD_COMM_PresetBorder", 3}} )
	addBorder( pb.L4, 250, {{"MFD_COMM_PresetBorder", 4}})
	addBorder( pb.L5, 250, {{"MFD_COMM_PresetBorder", 5}} )

	addBorder( pb.R1, 240, {{"MFD_COMM_PresetBorder", 6}} )
	addBorder( pb.R2, 240, {{"MFD_COMM_PresetBorder", 7}} )
	addBorder( pb.R3, 240, {{"MFD_COMM_PresetBorder", 8}} )
	addBorder( pb.R4, 240, {{"MFD_COMM_PresetBorder", 9}} )
	addBorder( pb.R5, 240, {{"MFD_COMM_PresetBorder", 10}} )
end

function addOwnShip_StatusWindow()

	AddRoundCornersWindow("OWNSHIP_Status_Window",	{0.0,((pb_props[pb.L4].pos[2] - 20))  },
							tp_default.width*20, tp_default.height*21.5,
							{
								{"OWNSHIP",				{0.0,								tp_default.height*21*0.5 - 20},		tp_center},
								{"AHE94",				{0.0,								tp_default.height*21*0.5 - 55},		tp_center,			{{"MFD_COMM_OWNSHIP_Status_Window_Data", 0}, {"MFD_COMM_OWNSHIP_Status_Window", 1}}},
								{"DL:",					{-tp_default.width*19*0.5 + 10,		tp_default.height*21*0.5 - 125},	tp_def_left_center,	{{"MFD_COMM_OWNSHIP_Status_Window_Data", 1}, {"MFD_COMM_OWNSHIP_Status_Window", 1}}},
								{"TF:",					{-tp_default.width*19*0.5 + 10,		tp_default.height*21*0.5 - 165},	tp_def_left_center,	{{"MFD_COMM_OWNSHIP_Status_Window_Data", 2}, {"MFD_COMM_OWNSHIP_Status_Window", 1}}},
								{"TI:",					{-tp_default.width*19*0.5 + 10,		tp_default.height*21*0.5 - 205},	tp_def_left_center,	{{"MFD_COMM_OWNSHIP_Status_Window_Data", 3}, {"MFD_COMM_OWNSHIP_Status_Window", 1}}},
								{"FS:",					{-tp_default.width*19*0.5 + 10,		tp_default.height*21*0.5 - 245},	tp_def_left_center,	{{"MFD_COMM_OWNSHIP_Status_Window_Data", 4}, {"MFD_COMM_OWNSHIP_Status_Window", 1}}},
								{"FIRE SUPPORT:",		{-tp_default.width*19*0.5 + 10,		tp_default.height*21*0.5 - 290},	tp_def_left_center},
								{"IP",					{-tp_default.width*19*0.5 + 40,		tp_default.height*21*0.5 - 335},	tp_def_left_center,	{{"MFD_COMM_OWNSHIP_Status_Window_Data", 5}, {"MFD_COMM_OWNSHIP_Status_Window", 1}}},
								{"MASK ",				{-tp_default.width*19*0.5 + 40,		tp_default.height*21*0.5 - 370},	tp_def_left_center,	{{"MFD_COMM_OWNSHIP_Status_Window_Data", 6}, {"MFD_COMM_OWNSHIP_Status_Window", 1}}},
								{"ACCESS RANK",			{-tp_default.width*19*0.5 + 40,		tp_default.height*21*0.5 - 420},	tp_def_left_center },
								{"TOTAL STATION",		{-tp_default.width*19*0.5 + 40,		tp_default.height*21*0.5 - 455},	tp_def_left_center },
								{"STATION ID",			{-tp_default.width*19*0.5 + 40,		tp_default.height*21*0.5 - 490},	tp_def_left_center },
								{"UTO:",				{-tp_default.width*19*0.5 + 10,		tp_default.height*21*0.5 - 555},	tp_def_left_center },
								
								{"0",					{tp_default.width*7,				tp_default.height*21*0.5 - 420},	tp_def_left_center, {{"MFD_COMM_OWNSHIP_Status_Window_Data", 7}}},
								{"0",					{tp_default.width*7,				tp_default.height*21*0.5 - 455},	tp_def_left_center, {{"MFD_COMM_OWNSHIP_Status_Window_Data", 8}}},
								{"0",					{tp_default.width*7,				tp_default.height*21*0.5 - 490},	tp_def_left_center, {{"MFD_COMM_OWNSHIP_Status_Window_Data", 9}}},
								{"?",					{-tp_default.width*5,				tp_default.height*21*0.5 - 555},	tp_def_left_center, {{"MFD_COMM_OWNSHIP_Status_Window_Data", 10}}},
								
													
							},
							tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)	
end

function addPresetEdit_StatusWindow( controller_name )
	--"MFD_COM_PRESET_Status_Window"
	local tp28				= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftCenter" )
	AddRoundCornersWindow("COM_PRESET_Status_Window.",	{5.0,((-pb_props[pb.L1].pos[2] + pb_props[pb.L2].pos[2]) /2 + 70) },
							tp_default.width*17.0, tp_default.height*27.0,
							{
								{"",					{-tp_default.width*21*0.5 + 55,		tp_default.height*26*0.5 - 35},		tp28, {{controller_name, 0}}},  						-- Unit ID  Number Call Sign
								{"",					{ tp_default.width* 4*0.5 + 55,		tp_default.height*26*0.5 - 35},		tp28, {{controller_name, 23}}}, 
								{"",					{-tp_default.width*21*0.5 + 55,		tp_default.height*26*0.5 - 70},		tp28, {{controller_name, 1}}}, 						-- DM Type and IDM Preset-Net
								{"VHF",					{-tp_default.width*21*0.5 + 75,		tp_default.height*26*0.5 - 125},	tp28, {{controller_name, 2}}}, 						-- VHF Frequency
								{"f",					{-tp_default.width*21*0.5 + 55,		tp_default.height*26*0.5 - 125},	tp28, {{controller_name, 19}}}, 					-->
								{"UHF:",				{-tp_default.width*21*0.5 + 55,		tp_default.height*26*0.5 - 175},	tp28, {{controller_name, 3}}}, 						-- UHF CNV status
								{"HQ",					{-tp_default.width*21*0.5 + 75,		tp_default.height*26*0.5 - 210},	tp28, {{controller_name, 4}}}, 						-- UHF HQ Net number
								{"SC",					{-tp_default.width*21*0.5 + 75,		tp_default.height*26*0.5 - 245},	tp28, {{controller_name, 5}}}, 						-- UHF Single Channel Frequency
								{"f",					{-tp_default.width*21*0.5 + 55,		tp_default.height*26*0.5 - 245},	tp28, {{controller_name, 20}}}, 						--> 
								{"FM1:",				{-tp_default.width*21*0.5 + 55,		tp_default.height*26*0.5 - 295},	tp28, {{controller_name, 6}}}, 						-- FM1 CNV status
								{"SINC",				{-tp_default.width*21*0.5 + 75,		tp_default.height*26*0.5 - 330},	tp28, {{controller_name, 7}}}, 						-- FM1 SINC net number
								{"SC",					{-tp_default.width*21*0.5 + 75,		tp_default.height*26*0.5 - 365},	tp28, {{controller_name, 8}}},  						-- FM1 Single Channel 
								{"f",					{-tp_default.width*21*0.5 + 55,		tp_default.height*26*0.5 - 365},	tp28, {{controller_name, 21}}},  						-->
								--Frequency													
								{"FM2:",				{-tp_default.width*21*0.5 + 55,		tp_default.height*26*0.5 - 415},	tp28, {{controller_name, 9}}}, 						-- FM2 CNV status
								{"SINC",				{-tp_default.width*21*0.5 + 75,		tp_default.height*26*0.5 - 450},	tp28, {{controller_name, 10}}},  					-- FM2 SINC HOPSET number
								{"SC",					{-tp_default.width*21*0.5 + 75,		tp_default.height*26*0.5 - 485},	tp28, {{controller_name, 11}}}, 						-- FM2 CNV status
								{"f",					{-tp_default.width*21*0.5 + 55,		tp_default.height*26*0.5 - 485},	tp28, {{controller_name, 22}}}, 					-->
								{"HF:",					{-tp_default.width*21*0.5 + 55,		tp_default.height*26*0.5 - 535},	tp28, {{controller_name, 12}}}, 						-- HF CNV status
								{"PRE",					{-tp_default.width*21*0.5 + 75,		tp_default.height*26*0.5 - 570},	tp28, {{controller_name, 13}}},						-- HF 
								{"ALE",					{-tp_default.width*21*0.5 + 75,		tp_default.height*26*0.5 - 605},	tp28, {{controller_name, 14}}},						-- HF
								{"ECCM",				{-tp_default.width*21*0.5 + 75,		tp_default.height*26*0.5 - 640},	tp28, {{controller_name, 15}}}, 						-- HF
								{"SC",					{-tp_default.width*21*0.5 + 75,		tp_default.height*26*0.5 - 675},	tp28, {{controller_name, 16}}},						-- HF 
								{"SC",					{-tp_default.width*0,				tp_default.height*26*0.5 - 675},	tp28, {{controller_name, 17}}},						-- HF 
								{"SC",					{-tp_default.width*0,				tp_default.height*26*0.5 - 710},	tp28, {{controller_name, 18}}},						-- HF
													
							},
							tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)

end

function addModem_StatusWindow()

	local w = tp_default.width*19.0
	local h = tp_default.height*3.0
	local w0 = 10 - w/2 
	local h0 = h/2 - tp_default.steep_y


	AddRoundCornersWindow("ComModemStatusWindow",	{5.0, pb_props[pb.L1].pos[2]},
							w, h,
							{
								{"", { w0,	h0},						tp_28,						{{"MFD_COM_PRESET_MODEM_Status_Window", 0}} },
								{"", { tp_default.width*5.5,	h0},	tp_28_white_left_bottom,	{{"MFD_COM_PRESET_MODEM_Status_Window", 1}} },
								{"", { w0, 	h0 - tp_default.steep_y},	tp_28,						{{"MFD_COM_PRESET_MODEM_Status_Window", 2}} },
							},
							tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
end

function addSendBox(controller_visible)
	PH = addPlaceholder("send_box_ph", {420, -320}, nil, controller_visible)
	send_box = addRoundedBox("send_box", {0, 0}, "LeftTop", {80, 90}, 10, 3, PH.name, nil, IND_MPD_MATERIAL_WHITE)
	addText( "UHF", {40, -40}, tp_28_white_center_bottom, {{"MPD_COM_SendBtnRadio"}}, {"VHF", "UHF", "FM1", "FM2", "HF"}, nil, nil, PH.name )
	addText( "L2",  {40, -80}, tp_28_white_center_bottom, {{"MPD_COM_SendBtnNet"}}, nil, nil, nil, PH.name )
end

function addMPS_TEXT_SecondLabels()

	pb_props[pb.L1].tp.alignment = "LeftTop"
	pb_props[pb.L2].tp.alignment = "LeftTop"
	pb_props[pb.L3].tp.alignment = "LeftTop"
	pb_props[pb.L4].tp.alignment = "LeftTop"
	pb_props[pb.L5].tp.alignment = "LeftTop"

	local x = pb_props[pb.L1].pos_up[1] -- + tp_default.width
	addPB( pb.L1, "", {x, pb_props[pb.L1].pos[2] }, tp_default, {{"MPD_COM_IDM_MPS_TEXT_Title", 0}} )
	addPB( pb.L2, "", {x, pb_props[pb.L2].pos[2] }, tp_default, {{"MPD_COM_IDM_MPS_TEXT_Title", 1}} )
	addPB( pb.L3, "", {x, pb_props[pb.L3].pos[2] }, tp_default, {{"MPD_COM_IDM_MPS_TEXT_Title", 2}} )
	addPB( pb.L4, "", {x, pb_props[pb.L4].pos[2] }, tp_default, {{"MPD_COM_IDM_MPS_TEXT_Title", 3}} )
	addPB( pb.L5, "", {x, pb_props[pb.L5].pos[2] }, tp_default, {{"MPD_COM_IDM_MPS_TEXT_Title", 4}} )

	x = pb_props[pb.R1].pos_up[1] - tp_default.width
	addPB( pb.R1, "", {x, pb_props[pb.R1].pos[2] }, tp_default, {{"MPD_COM_IDM_MPS_TEXT_Title", 5}} )
	addPB( pb.R2, "", {x, pb_props[pb.R2].pos[2] }, tp_default, {{"MPD_COM_IDM_MPS_TEXT_Title", 6}} )
	addPB( pb.R3, "", {x, pb_props[pb.R3].pos[2] }, tp_default, {{"MPD_COM_IDM_MPS_TEXT_Title", 7}} )
	addPB( pb.R4, "", {x, pb_props[pb.R4].pos[2] }, tp_default, {{"MPD_COM_IDM_MPS_TEXT_Title", 8}} )
	addPB( pb.R5, "", {x, pb_props[pb.R5].pos[2] }, tp_default, {{"MPD_COM_IDM_MPS_TEXT_Title", 9}} )

	--pb_props[pb.L1].tp.alignment = "RightTop"
	--pb_props[pb.L2].tp.alignment = "RightTop"
	--pb_props[pb.L3].tp.alignment = "RightTop"
	--pb_props[pb.L4].tp.alignment = "RightTop"
	--pb_props[pb.L5].tp.alignment = "RightTop"
end

function addSecondLabels()

	pb_props[pb.L1].tp.alignment = "LeftTop"
	pb_props[pb.L2].tp.alignment = "LeftTop"
	pb_props[pb.L3].tp.alignment = "LeftTop"
	pb_props[pb.L4].tp.alignment = "LeftTop"
	pb_props[pb.L5].tp.alignment = "LeftTop"

	pb_props[pb.R1].tp.alignment = "LeftTop"
	pb_props[pb.R2].tp.alignment = "LeftTop"
	pb_props[pb.R3].tp.alignment = "LeftTop"
	pb_props[pb.R4].tp.alignment = "LeftTop"
	pb_props[pb.R5].tp.alignment = "LeftTop"

	local x = pb_props[pb.L1].pos_up[1] + tp_default.width * 9
	addPB( pb.L1, "", {x, pb_props[pb.L1].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_2", 0}} )
	addPB( pb.L2, "", {x, pb_props[pb.L2].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_2", 1}} )
	addPB( pb.L3, "", {x, pb_props[pb.L3].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_2", 2}} )
	addPB( pb.L4, "", {x, pb_props[pb.L4].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_2", 3}} )
	addPB( pb.L5, "", {x, pb_props[pb.L5].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_2", 4}} )

	x = pb_props[pb.R1].pos_up[1] - tp_default.width * 12
	addPB( pb.R1, "", {x, pb_props[pb.R1].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_2", 5}} )
	addPB( pb.R2, "", {x, pb_props[pb.R2].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_2", 6}} )
	addPB( pb.R3, "", {x, pb_props[pb.R3].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_2", 7}} )
	addPB( pb.R4, "", {x, pb_props[pb.R4].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_2", 8}} )
	addPB( pb.R5, "", {x, pb_props[pb.R5].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_2", 9}} )
	------------------
	x = pb_props[pb.R1].pos_up[1] - tp_default.width * 3
	addPB( pb.R1, "", {x, pb_props[pb.R1].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_1", 5}} )
	addPB( pb.R2, "", {x, pb_props[pb.R2].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_1", 6}} )
	addPB( pb.R3, "", {x, pb_props[pb.R3].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_1", 7}} )
	addPB( pb.R4, "", {x, pb_props[pb.R4].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_1", 8}} )
	addPB( pb.R5, "", {x, pb_props[pb.R5].pos_up[2] }, tp_default, {{"MFD_COMM_FirstLine_1", 9}} )

	---------------------
	x = pb_props[pb.R1].pos_down[1] - tp_default.width * 12
	addPB( pb.R1, "", {x, pb_props[pb.R1].pos_down[2] }, tp_default, {{"MFD_COMM_SecondLine", 5}} )
	addPB( pb.R2, "", {x, pb_props[pb.R2].pos_down[2] }, tp_default, {{"MFD_COMM_SecondLine", 6}} )
	addPB( pb.R3, "", {x, pb_props[pb.R3].pos_down[2] }, tp_default, {{"MFD_COMM_SecondLine", 7}} )
	addPB( pb.R4, "", {x, pb_props[pb.R4].pos_down[2] }, tp_default, {{"MFD_COMM_SecondLine", 8}} )
	addPB( pb.R5, "", {x, pb_props[pb.R5].pos_down[2] }, tp_default, {{"MFD_COMM_SecondLine", 9}} )

	pb_props[pb.L1].tp.alignment = "RightTop"
	pb_props[pb.L2].tp.alignment = "RightTop"
	pb_props[pb.L3].tp.alignment = "RightTop"
	pb_props[pb.L4].tp.alignment = "RightTop"
	pb_props[pb.L5].tp.alignment = "RightTop"

	pb_props[pb.R1].tp.alignment = "RightTop"
	pb_props[pb.R2].tp.alignment = "RightTop"
	pb_props[pb.R3].tp.alignment = "RightTop"
	pb_props[pb.R4].tp.alignment = "RightTop"
	pb_props[pb.R5].tp.alignment = "RightTop"
end

function addNetSecondLabels()

	local x = pb_props[pb.L1].pos_down[1] + tp_default.width * 3 -6
	addPB( pb.L1, "", {x, pb_props[pb.L1].pos_down[2] }, tp_default, {{"MPD_COM_NET_TEAM", 0}}, {"", "TEAM"})
	addPB( pb.L2, "", {x, pb_props[pb.L2].pos_down[2] }, tp_default, {{"MPD_COM_NET_TEAM", 1}}, {"", "TEAM"})
	addPB( pb.L3, "", {x, pb_props[pb.L3].pos_down[2] }, tp_default, {{"MPD_COM_NET_TEAM", 2}}, {"", "TEAM"})
	addPB( pb.L4, "", {x, pb_props[pb.L4].pos_down[2] }, tp_default, {{"MPD_COM_NET_TEAM", 3}}, {"", "TEAM"})
	addPB( pb.L5, "", {x, pb_props[pb.L5].pos_down[2] }, tp_default, {{"MPD_COM_NET_TEAM", 4}}, {"", "TEAM"})

	local x = pb_props[pb.L1].pos_down[1] + tp_default.width * 8 -6
	addPB( pb.L1, "", {x, pb_props[pb.L1].pos_down[2] }, tp_default, {{"MPD_COM_NET_PRI", 0}}, {"", "PRI"} )
	addPB( pb.L2, "", {x, pb_props[pb.L2].pos_down[2] }, tp_default, {{"MPD_COM_NET_PRI", 1}}, {"", "PRI"} )
	addPB( pb.L3, "", {x, pb_props[pb.L3].pos_down[2] }, tp_default, {{"MPD_COM_NET_PRI", 2}}, {"", "PRI"} )
	addPB( pb.L4, "", {x, pb_props[pb.L4].pos_down[2] }, tp_default, {{"MPD_COM_NET_PRI", 3}}, {"", "PRI"} )
	addPB( pb.L5, "", {x, pb_props[pb.L5].pos_down[2] }, tp_default, {{"MPD_COM_NET_PRI", 4}}, {"", "PRI"} )

	x = pb_props[pb.R1].pos_down[1] - tp_default.width * 4
	addPB( pb.R1, "", {x, pb_props[pb.R1].pos_down[2] }, tp_default, {{"MPD_COM_NET_TEAM", 5}}, {"", "TEAM"})
	addPB( pb.R2, "", {x, pb_props[pb.R2].pos_down[2] }, tp_default, {{"MPD_COM_NET_TEAM", 6}}, {"", "TEAM"})
	addPB( pb.R3, "", {x, pb_props[pb.R3].pos_down[2] }, tp_default, {{"MPD_COM_NET_TEAM", 7}}, {"", "TEAM"})
	addPB( pb.R4, "", {x, pb_props[pb.R4].pos_down[2] }, tp_default, {{"MPD_COM_NET_TEAM", 8}}, {"", "TEAM"})
	addPB( pb.R5, "", {x, pb_props[pb.R5].pos_down[2] }, tp_default, {{"MPD_COM_NET_TEAM", 9}}, {"", "TEAM"})

	pb_props[pb.R1].tp.alignment = "LeftTop"
	pb_props[pb.R2].tp.alignment = "LeftTop"
	pb_props[pb.R3].tp.alignment = "LeftTop"
	pb_props[pb.R4].tp.alignment = "LeftTop"
	pb_props[pb.R5].tp.alignment = "LeftTop"

	x = pb_props[pb.R1].pos_down[1] - tp_default.width * 11
	addPB( pb.R1, "", {x, pb_props[pb.R1].pos_down[2] }, tp_default, {{"MPD_COM_NET_LB", 5}} )
	addPB( pb.R2, "", {x, pb_props[pb.R2].pos_down[2] }, tp_default, {{"MPD_COM_NET_LB", 6}} )
	addPB( pb.R3, "", {x, pb_props[pb.R3].pos_down[2] }, tp_default, {{"MPD_COM_NET_LB", 7}} )
	addPB( pb.R4, "", {x, pb_props[pb.R4].pos_down[2] }, tp_default, {{"MPD_COM_NET_LB", 8}} )
	addPB( pb.R5, "", {x, pb_props[pb.R5].pos_down[2] }, tp_default, {{"MPD_COM_NET_LB", 9}} )

	pb_props[pb.R1].tp.alignment = "RightTop"
	pb_props[pb.R2].tp.alignment = "RightTop"
	pb_props[pb.R3].tp.alignment = "RightTop"
	pb_props[pb.R4].tp.alignment = "RightTop"
	pb_props[pb.R5].tp.alignment = "RightTop"
end

function addCOM_NET_PRESET_Status_Window()

	local w = tp_default.width*17.0	
	local h = tp_default.height*4.0
	w0 = 10 - w/2 
	local h0 = h/2 - tp_default.steep_y	

	AddRoundCornersWindow("COM_NET_PRESET_Status_Window",	{ 5, pb_props[pb.L1].pos_c[2] },
							w, h,
							{
								{"", { w0, h0},							tp_28, {{"COM_NET_PRESET_Status_Window", 1}}},
								{"", { w0+tp_default.width*13	, h0},	tp_28, {{"COM_NET_PRESET_Status_Window", 2}}},
								{"", { w0, h0 - tp_default.steep_y},	tp_28, {{"COM_NET_PRESET_Status_Window", 3}}},
							},
							tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
end



