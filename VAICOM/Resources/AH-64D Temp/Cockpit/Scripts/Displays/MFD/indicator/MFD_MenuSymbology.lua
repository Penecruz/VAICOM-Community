-- global table with precalculated pushbuttons positions (copy/paste fight)
pb_props = {}

function fill_pb_props()

	for pb_num = pb.T1, pb.L1 do

		pb_props[pb_num] = { tp = {}, pos = {0,0}, pos_up = {0,0}, pos_down = {0,0}, def_name, elem = nil }
		if	pb_num >= pb.L6 then
			pb_props[pb_num].def_name		= ("L ".. pb_num)
			pb_props[pb_num].tp				= tp_def_left
			pb_props[pb_num].pos			= getPosPB(pb_num)
			pb_props[pb_num].pos_c			= {pb_props[pb_num].pos[1], pb_props[pb_num].pos[2] - PB_dist_y/2}
			pb_props[pb_num].pos_cl			= {pb_props[pb_num].pos[1], pb_props[pb_num].pos[2] + PB_dist_y/2}
			pb_props[pb_num].pos_up			= {pb_props[pb_num].pos[1], pb_props[pb_num].pos[2] + 0.6*pb_props[pb_num].tp.height}
			pb_props[pb_num].pos_down		= {pb_props[pb_num].pos[1], pb_props[pb_num].pos[2] - 0.8*pb_props[pb_num].tp.height}
			pb_props[pb_num].show_barrier	= true

		elseif	pb_num >= pb.B6 then
			pb_props[pb_num].def_name		= ("B ".. pb_num)
			pb_props[pb_num].tp				= tp_def_bottom
			pb_props[pb_num].pos			= getPosPB(pb_num)
			pb_props[pb_num].pos_c			= {pb_props[pb_num].pos[1] + PB_dist_x/2, pb_props[pb_num].pos[2] }
			pb_props[pb_num].pos_cl			= {pb_props[pb_num].pos[1] - PB_dist_x/2, pb_props[pb_num].pos[2] }
			pb_props[pb_num].pos_up			= {pb_props[pb_num].pos[1], pb_props[pb_num].pos[2] + 1.35*pb_props[pb_num].tp.height}
			pb_props[pb_num].pos_down		= {pb_props[pb_num].pos[1], pb_props[pb_num].pos[2]}
			pb_props[pb_num].show_barrier	= true

		elseif	pb_num >= pb.R1 then
			pb_props[pb_num].def_name		= ("R ".. pb_num)
			pb_props[pb_num].tp				= tp_def_right
			pb_props[pb_num].pos			= getPosPB(pb_num)
			pb_props[pb_num].pos_c			= {pb_props[pb_num].pos[1], pb_props[pb_num].pos[2] - PB_dist_y/2}
			pb_props[pb_num].pos_cl			= {pb_props[pb_num].pos[1], pb_props[pb_num].pos[2] + PB_dist_y/2}
			pb_props[pb_num].pos_up			= {pb_props[pb_num].pos[1], pb_props[pb_num].pos[2] + 0.6*pb_props[pb_num].tp.height}
			pb_props[pb_num].pos_down		= {pb_props[pb_num].pos[1], pb_props[pb_num].pos[2] - 0.8*pb_props[pb_num].tp.height}
			pb_props[pb_num].show_barrier	= true

		elseif	pb_num >= pb.T1 then
			pb_props[pb_num].def_name		= ("T ".. pb_num)
			pb_props[pb_num].tp				= tp_def_top
			pb_props[pb_num].pos			= getPosPB(pb_num)
			pb_props[pb_num].pos_c			= {pb_props[pb_num].pos[1] + PB_dist_x/2, pb_props[pb_num].pos[2] }
			pb_props[pb_num].pos_cl			= {pb_props[pb_num].pos[1] - PB_dist_x/2, pb_props[pb_num].pos[2] }
			pb_props[pb_num].pos_up			= {pb_props[pb_num].pos[1], pb_props[pb_num].pos[2]}
			pb_props[pb_num].pos_down		= {pb_props[pb_num].pos[1], pb_props[pb_num].pos[2] - 1.35*pb_props[pb_num].tp.height}
			pb_props[pb_num].show_barrier	= true
		end
	end
end

function draw_large_border_for_PB( PB_num, controllers, str_len, str_count )
	local margins = { 5,0,0,0 }
	local Lstr = str_len or 4
	local Nstr = str_count or 2

	local frame_pos = {}
	local tp = pb_props[PB_num].tp
	local delta_x = Lstr*tp.width*0.5
	local delta_y = (pb_props[PB_num].pos_up[2] - pb_props[PB_num].pos_down[2])/2

	local c = parseAligment ( tp.alignment )

	if c[1] == "L" then
		frame_pos = { pb_props[PB_num].pos[1] + delta_x,	pb_props[PB_num].pos_up[2] - delta_y }
	elseif c[1] == "R" then
		frame_pos = { pb_props[PB_num].pos[1] - delta_x,	pb_props[PB_num].pos_up[2] - delta_y }
	elseif c[2] == "B" or c[2] == "T" then
		frame_pos = { pb_props[PB_num].pos[1],				pb_props[PB_num].pos_up[2] - delta_y }
	end

	if c[2] == "B" then
		frame_pos[2] = frame_pos[2] + tp.height*0.65
	else
		frame_pos[2] = frame_pos[2] - tp.height*0.25
	end

	frame_pos = {frame_pos[1]+margins[1],frame_pos[2]+margins[2]}
	
	local Lbl_W, Lbl_H = Lstr*tp.width*1.2, (Nstr-1)*tp.height*1.25 + tp.height*1.5

	local verts	=	{
						{frame_pos[1]-Lbl_W/2, frame_pos[2]-Lbl_H/2},
						{frame_pos[1]-Lbl_W/2, frame_pos[2]+Lbl_H/2},
						{frame_pos[1]+Lbl_W/2, frame_pos[2]+Lbl_H/2},
						{frame_pos[1]+Lbl_W/2, frame_pos[2]-Lbl_H/2},
						{frame_pos[1]-Lbl_W/2, frame_pos[2]-Lbl_H/2},
					}

	--	tp.material = IND_MPD_MATERIAL_RED	-- TODO: DBG

	controllers[#controllers + 1] = {"DSPLS_PB_Visibility", PB_num}		-- TODO: what if controllers == nil ?
	controllers[#controllers + 1] = {"DSPLS_PB_BOLD", 		PB_num}

	controllers[#controllers][3] = 0		-- boldness
	draw_line( verts, tp.material, nil, 3, nil, controllers, nil, nil )

	controllers[#controllers][3] = 1		-- boldness
	draw_line( verts, tp.material, nil, 5, nil, controllers, nil, nil )

	-- TODO: change method for flexible border, but here are 2 lines
	--addSimpleText( text, pos, text_properties, controllers, formats, margins, name, parent, h_clip_relation, level, bold )
	--addSimpleText( "", pos, text_properties, controllers, formats, margins, name, parent, h_clip_relation, level, bold )
end


function draw_tex_line( vertices, tex_params, pos, material, parent, controllers )
	local scale = 1
	local elem			= CreateElement "ceTexPoly"
	elem.vertices		= vertices

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= tex_params
	if parent ~= nil then
		elem.parent_element = parent
		end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	elem.level			= DEFAULT_LEVEL + 2
	Add(elem)
end
--------------------------------------------------------------------------------------------
function draw_dl_v( pos, vertices, material, parent, controllers )
	local tex_params	= {950*texture_scale_1024, 450*texture_scale_1024, texture_scale_1024, texture_scale_1024}
	draw_tex_line( vertices, tex_params,{ pos[1], pos[2]}, material, parent, controllers )
end
--------------------------------------------------------------------------------------------
function draw_dl_h( pos, vertices, material, parent, controllers )
	local tex_params	= {850*texture_scale_1024, 625*texture_scale_1024, texture_scale_1024, texture_scale_1024}
	draw_tex_line( vertices, tex_params,{ pos[1], pos[2]}, material, parent, controllers )
end

function draw_disable_line( pb_num, material, two_line_item, parent, controllers )

	local draw_scale = menu_draw_scale or 1.0
	two_line_item = two_line_item or 0
	
	local line_sz  = two_line_item == 1 and 40 or 32
	local dl_shift = 14
	local l_up = line_sz - dl_shift
	local l_dn = -(line_sz + dl_shift)
	
	if two_line_item ~= -1 then

		local disable_line_width = 15

		if		pb_num >= pb.L6 then	-- L
				local vertices =	{	{ (-5) * draw_scale, l_dn * draw_scale },
										{ (-5) * draw_scale, l_up * draw_scale },
										{ (-5 - disable_line_width) * draw_scale, l_up * draw_scale },
										{ (-5 - disable_line_width) * draw_scale, l_dn * draw_scale }	}
				draw_dl_v( pb_props[pb_num].pos, vertices, material, parent, controllers )

		elseif	pb_num >= pb.B6 then	-- B
				local vertices =	{	{ -32 * draw_scale, (0) * draw_scale },
										{  42 * draw_scale, (0) * draw_scale },
										{  42 * draw_scale, (0 - disable_line_width) * draw_scale },
										{ -32 * draw_scale, (0 - disable_line_width) * draw_scale }	}
				draw_dl_h( pb_props[pb_num].pos, vertices, material, parent, controllers )

		elseif	pb_num >= pb.R1 then	-- R
				local vertices =	{	{ (12) * draw_scale, l_dn * draw_scale },
										{ (12) * draw_scale, l_up * draw_scale },
										{ (12 + disable_line_width) * draw_scale, l_up * draw_scale },
										{ (12 + disable_line_width) * draw_scale, l_dn * draw_scale }	}
				draw_dl_v( pb_props[pb_num].pos, vertices, material, parent, controllers )

		elseif	pb_num >= pb.T1 then	-- T
				local vertices =	{	{ -32 * draw_scale, (12) * draw_scale },
										{  42 * draw_scale, (12) * draw_scale },
										{  42 * draw_scale, (12 + disable_line_width) * draw_scale },
										{ -32 * draw_scale, (12 + disable_line_width) * draw_scale }	}
				draw_dl_h( pb_props[pb_num].pos, vertices, material, parent, controllers )
		end

	end
end

function draw_command_failed_triangle( pb_num, material, parent, controllers )
	local draw_scale = menu_draw_scale or 1.0

	local pos = pb_props[pb_num].pos

	if		pb_num >= pb.L6 then	-- L
			local verts = buildBoxVerts(20 * draw_scale, 80 * draw_scale)
			local tex_params	= {810*texture_scale_1024, 160*texture_scale_1024, texture_scale_1024, texture_scale_1024}
			draw_tex_line( verts, tex_params, {pos[1] - 10 * draw_scale, pos[2] - 14 * draw_scale}, material, parent, controllers )

	elseif	pb_num >= pb.B6 then	-- B
			local verts = buildBoxVerts(80 * draw_scale, 20 * draw_scale)
			local tex_params	= {900*texture_scale_1024, 170*texture_scale_1024, texture_scale_1024, texture_scale_1024}
			draw_tex_line( verts, tex_params, {pos[1] + 5* draw_scale, pos[2] - 10 * draw_scale}, material, parent, controllers )

	elseif	pb_num >= pb.R1 then	-- R
			local verts = buildBoxVerts(20 * draw_scale, 80 * draw_scale)
			local tex_params	= {830*texture_scale_1024, 160*texture_scale_1024, texture_scale_1024, texture_scale_1024}
			draw_tex_line( verts, tex_params, {pos[1] + 20 * draw_scale, pos[2] - 14 * draw_scale}, material, parent, controllers )

	elseif	pb_num >= pb.T1 then	-- T
			local verts = buildBoxVerts(80 * draw_scale, 20 * draw_scale)
			local tex_params	= {900*texture_scale_1024, 150*texture_scale_1024, texture_scale_1024, texture_scale_1024}
			draw_tex_line( verts, tex_params, {pos[1] + 5* draw_scale, pos[2] + 20 * draw_scale}, material, parent, controllers )
	end
end

postfix_count = 0
function getPostfix()
	postfix_count = postfix_count + 1
	return "_" .. postfix_count
end

function addPB( pb_num, value, pos, tp_, controllers, formats, margins, parent, group_border, draw_text_background, is_transparent, two_line_item )
-- att: group_border suppresses flexible individual border (addText::addFlexBorderAroundText)
	draw_text_background = draw_text_background or 1
	is_transparent = is_transparent or false
	
	local controllers_pb = {{"DSPLS_PB_Visibility", pb_num},{"DSPLS_PB_BOLD", pb_num}}
	local caption = 0
	local tp_border
	local elem = {}
	local elem_bold = {}
	
	if pb_num >= pb.T1 and pb_num < pb.MAX then
		local tp = tp_ ~= nil and tp_ or pb_props[pb_num].tp
		if type(value) ~= "table" then																-- value is text
			tp.alignment = pb_props[pb_num].tp.alignment	-- in pb_props[num] only alignment is important
			caption = value or pb_props[pb_num].def_name

			local m = margins ~= nil and margins or tp.margins

			if tp ~= nil and group_border then
				tp_border = tp.border -- save value
				tp.border = false
			end
			if controllers ~= nil then
				for i=1, #controllers do
						controllers_pb[#controllers_pb+1] = controllers[i]
				end
			end

			if draw_text_background == 1 then
				controllers_pb[2][3] = 0; -- not bold
				elem[1]			= addText( caption, pos, tp, controllers_pb, formats, m, "PB"..pb_num..getPostfix(), parent, nil, nil, false, is_transparent)
				controllers_pb[2][3] = 1; -- bold
				elem_bold[1]	= addText( caption, pos, tp, controllers_pb, formats, m, "PB"..pb_num.."_bold"..getPostfix(), parent, nil, nil, true, is_transparent)
			else
				controllers_pb[2][3] = 0; -- not bold
				elem[1]			= addSimpleText( caption, pos, tp, controllers_pb, formats, m, "PB"..pb_num..getPostfix(), parent, nil, nil, false)
				controllers_pb[2][3] = 1; -- bold
				elem_bold[1]	= addSimpleText( caption, pos, tp, controllers_pb, formats, m, "PB"..pb_num.."_bold"..getPostfix(), parent, nil, nil, true)
			end

			if tp ~= nil and group_border then
				tp.border = tp_border -- restore
			end
			pb_props[pb_num].elem = elem[1]

			if pb_props[pb_num].show_barrier == true then
				draw_disable_line( pb_num, IND_MPD_1024_MATERIAL_DARK_GREEN, two_line_item, parent, {{"DSPLS_PB_Selectabled", pb_num}} )
				--draw_disable_line( pb_num, IND_MPD_1024_MATERIAL_DARK_GREEN, two_line_item, parent, nil )
			end
			draw_command_failed_triangle( pb_num, IND_MPD_1024_MATERIAL_WHITE, parent, {{"DSPLS_PB_CommandFailed", pb_num}} )
			--draw_command_failed_triangle( pb_num, IND_MPD_1024_MATERIAL_WHITE, parent, nil )

		else			-- value is table
			local tp = value[1][2] -- off individual border

			if tp ~= nil and group_border then
				tp_border = tp.border -- save value
				tp.border = false
			end

			-- value[x][1] -> text; value[x][2] -> tp; value[x][3] -> controller;
			local cap_up, elem_1, elem_bold_1 = addPB( pb_num, value[1][1], pb_props[pb_num].pos_up, tp, value[1][3], value[1][4], value[1][5], parent, nil, draw_text_background, is_transparent, 1 )

			if tp ~= nil and group_border then
				tp.border = tp_border -- restore value
			end

			tp = value[2][2] -- off individual border
			if tp ~= nil and group_border then
				tp_border = tp.border -- save value
				tp.border = false
			end
			local cap_dn, elem_2, elem_bold_2 = addPB( pb_num, value[2][1], pb_props[pb_num].pos_down, tp, value[2][3], value[2][4], value[2][5], parent, nil, draw_text_background, is_transparent, -1 )

			if tp ~= nil and group_border then
				tp.border = tp_border -- restore value
			end

			elem[1] = elem_1[1]
			elem[2] = elem_2[1]
			elem_bold[1] = elem_bold_1[1]
			elem_bold[2] = elem_bold_2[1]
			caption = #cap_up > #cap_dn and cap_up or cap_dn
			pb_props[pb_num].elem = elem[1]
		end
	end

	return caption, elem, elem_bold
end
-- ----------------------------------------------------------------------------------------------
function addPB_Arrow( pb_num, value, pos, tp_, controllers, formats, parent, draw_text_background, is_transparent )
-- att:: this function is intended for use in menu
-- att:: it suppresses flexible individual border and draws border around arrow via addBorderAroundText
	local tp = tp_ ~= nil and tp_ or pb_props[pb_num].tp
	local m = tp.margins
	local caption, elem, elem_bold = addPB( pb_num, value, pos, tp, controllers, formats, m, parent, true, draw_text_background, is_transparent ) --
	if #caption > 0 then
		if #elem == 1 then
			if tp.border then
				-- border is first child
				addBorderAroundText( pos, caption, tp, { m[1], m[2]-2, m[3], m[4]+15 }, elem[1].name, false ) -- margins for arrow: left, down, right, up
				addBorderAroundText( pos, caption, tp, { m[1], m[2]-2, m[3], m[4]+15 }, elem_bold[1].name, true )
			end
			-- arrow is second child
			draw_arrow_forPB( pb_num, caption, {0,0}, tp, elem[1].name, false )
			draw_arrow_forPB( pb_num, caption, {0,0}, tp, elem_bold[1].name, true )
		elseif #elem == 2 then -- two rows label
			if tp.border then
			-- border is first child
				addBorderAroundText( pos, caption, tp, { m[1], m[2]+tp.height*1.25, m[3], m[4]+15 }, elem[1].name, false ) -- margins for arrow: left, down, right, up
				addBorderAroundText( pos, caption, tp, { m[1], m[2]+tp.height*1.25, m[3], m[4]+15 }, elem_bold[1].name, true )
			end
			-- arrow is second child
			draw_arrow_forPB( pb_num, caption, {0, 0}, tp, elem[1].name, false )
			draw_arrow_forPB( pb_num, caption, {0, 0}, tp, elem_bold[1].name, true )
		end
	end
	return caption
end

function getCaption( value )
	local caption

	if type(value) ~= "table" then
		caption = value
	else			-- value is table
		local cap_up = value[1][1]
		local cap_dn = value[2][1]
		caption = #cap_up > #cap_dn and cap_up or cap_dn
	end

	return caption
end

function calcMaxItemLen( controls )
	local caption
	local max_item_len = 0
	local num_lines = 1
	local i,j

	for item = 1, #controls do
		if type(controls[item][1]) == "number" then
			caption = getCaption( controls[item][2] )						-- detect len in symbols
			i,j = string.find(caption, "\n")
			if i ~= nil then
				num_lines = 2
			end
			max_item_len = max_item_len > #caption and max_item_len or #caption
		end 
	end
	return max_item_len, num_lines
end
--------------------------------------------------------------------------------------------------
function createControls( controls, drawBorder, m_parent, draw_text_background, is_transparent )
	local caption
	local max_item_len = 0
	local dtb = draw_text_background or 1

	for item = 1, #controls do
		if type(controls[item][1]) == "number" then
			caption = createControlItem( controls[item], drawBorder, m_parent, dtb, is_transparent )		-- detect len in symbols
		--	max_item_len = max_item_len > #caption and max_item_len or #caption

		elseif type(controls[item][1]) == "string" then

			--it makes life easier
			caption					= controls[item][1]
			local inner_controls	= controls[item][2]
			local controllers		= controls[item][3]
			local pbs				= { inner_controls[1][1],	inner_controls[#inner_controls][1] }		-- upper&lower pbs in group
			local tp				= pb_props[pbs[1]].tp													-- tp of first button in group
			local pos_up			= pb_props[pbs[1]].pos
			local pos_dn			= pb_props[pbs[2]].pos
			local parent			= m_parent or inner_controls[1][6]

			local max_item_len, num_lines = calcMaxItemLen( inner_controls )

			if controllers ~= nil then
				parent = addPlaceholder(nil, {0,0}, nil, controllers)
				draw_border_with_caption( pos_up, pos_dn, max_item_len, num_lines, caption, tp, parent.name, is_transparent )
				createControls( inner_controls, true, parent, 0, is_transparent ) -- true to set border to all items in checkBox
			else
				draw_border_with_caption( pos_up, pos_dn, max_item_len, num_lines, caption, tp, parent, is_transparent )
				createControls( inner_controls, true, parent, 0, is_transparent ) -- true to set border to all items in checkBox
			end
		end
	end
--	return max_item_len
end
-------------------------------------------------------------------------------------------------
function createControlItem( controlItem, drawBorder, m_parent, draw_text_background, is_transparent )

	local pos		= controlItem[3] ~= nil and controlItem[3].pos or pb_props[controlItem[1]].pos
	local tp		= controlItem[3] ~= nil and controlItem[3]		or pb_props[controlItem[1]].tp
	local parent	= m_parent or controlItem[6]
	local border	= tp.border

	if drawBorder ~= nil then
		tp.border =		drawBorder -- set border to all controls in check box to change in controllers
	end
					--		pb_num,			value,			pos,	tp, controllers,	formats,		margins
	local caption = addPB(	controlItem[1], controlItem[2], pos,	tp, controlItem[4], controlItem[5], nil, parent, nil, draw_text_background, is_transparent )
	tp.border		=	border -- clean up

	return	caption
end

function draw_border_with_caption( pos_up, pos_dn, max_item_len, num_lines, caption, tp, parent, is_transparent )
	max_item_len = (max_item_len + 1) * tp.width												-- lenth in pixels

	local c = parseAligment ( tp.alignment )
	local verts = createVertsForLines( pos_up, pos_dn, max_item_len, num_lines, "  ", c, tp )		-- two lines with 3 points in each
	local pos_caption = calculateCaptionPos( pos_up, pos_dn, max_item_len, num_lines, caption, c, tp )

	if c[1] == "L" or c[1] == "R" then
		local caption_space = tp.height/8
		verts[1][3][2] = pos_caption[2] + caption_space
		verts[2][3][2] = pos_caption[2] - #caption*tp.height - caption_space
	else
		local caption_space = (#caption*tp.width)/2 + tp.width/8
		verts[1][3][1] = pos_caption[1] - caption_space
		verts[2][3][1] = pos_caption[1] + caption_space
	end

	local verts_bckgnd = createVertsForBackgroundWithCaption( verts, c )
	addTranslucentBackground( verts_bckgnd[1], default_box_indices, parent, nil, nil, is_transparent )
	addTranslucentBackground( verts_bckgnd[2], default_box_indices, parent, nil, nil, is_transparent )
	addTranslucentBackground( verts_bckgnd[3], default_box_indices, parent, nil, nil, is_transparent )

	-- draw caption
	-- addText( text, pos, text_properties, controllers, formats, margins, name, parent, h_clip_relation, level, bold, is_transparent )
	if c[1] == "L" or c[1] == "R" then
		for i = 1, #caption do
			addText( string.sub(caption, i, i).."\n", {pos_caption[1], pos_caption[2]}, tp, nil, nil, nil, nil, parent, h_clip_relations.INCREASE_LEVEL, DEFAULT_LEVEL, nil, is_transparent )
			pos_caption = {pos_caption[1], pos_caption[2] - tp.height }
		end
	else
		addText( caption, pos_caption, tp, nil, nil, nil, nil, parent, h_clip_relations.INCREASE_LEVEL, DEFAULT_LEVEL, nil, is_transparent )
	end

	-- draw upper&lower lines
	-- draw_line( verts, material, parent, width, name, controllers, h_clip_relation, level )
	draw_line( verts[1], tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )
	draw_line( verts[2], tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )
end

function createMenu( menu, m_parent, draw_text_background, is_transparent )
	local caption
	local max_item_len = 0
	local dtb = draw_text_background or 1

	for item = 1, #menu do
		if type(menu[item][1]) == "number" then
			caption = addPB_Menu( menu[item], m_parent, dtb, is_transparent )								-- detect len in symbols
		--	max_item_len = max_item_len > #caption and max_item_len or #caption

		elseif type(menu[item][1]) == "string" then
			--it makes life easier
			caption					= menu[item][1]
			local inner_menu		= menu[item][2]
			local pbs				= { inner_menu[1][1],	inner_menu[#inner_menu][1] }					-- upper&lower pbs in group
			local tp				= pb_props[pbs[1]].tp													-- tp of first button in group
			local pos_up			= pb_props[pbs[1]].pos
			local pos_dn			= pb_props[pbs[2]].pos
			local parent			= m_parent or inner_menu[1][6]

			local max_item_len, num_lines = calcMaxItemLen( inner_menu )
			draw_border_with_caption( pos_up, pos_dn, max_item_len, num_lines, caption, tp, parent, is_transparent )
			createMenu( inner_menu, parent, 0, is_transparent )
		end
	end
--	return max_item_len
end

function addPB_Menu( menuItem, m_parent, draw_text_background, is_transparent )
		local caption
		local pos = menuItem[3] ~= nil and menuItem[3].pos or pb_props[menuItem[1]].pos
		local pb_num = menuItem[1]
		local parent = m_parent or menuItem[6]

		if( pb_num == pb.B1) then
			caption = addPB( pb_num, menuItem[2], pos, menuItem[3], menuItem[4], menuItem[5], nil,	parent, false, draw_text_background, is_transparent ) -- special case for B1 button
		else
			caption = addPB_Arrow( pb_num, menuItem[2], pos, menuItem[3], menuItem[4], menuItem[5], parent, draw_text_background, is_transparent )
		end
		return caption
end

function draw_arrow( text_len, pos, tp, parent, controllers, tex_params, material, isvisible )
	local x_left, x_right, y_low, y_high = textCoordinatesAlignment( text_len, tp.height, tp.alignment, { 5,5,0,tp.height/2} )
	local arrow				= CreateElement "ceTexPoly"
	arrow.vertices			= { { -text_len, 6 }, { 0, 6 }, { 0, -6 }, { -text_len, -6 } }
	arrow.indices			= box_ind
	arrow.material			= material
	arrow.init_pos			= { pos[1] + x_right, pos[2] + y_high }										-- end of arrow
	arrow.tex_params		= tex_params
	arrow.parent_element	= parent
--	arrow.isvisible			= isvisible

	if controllers ~= nil then
		arrow.controllers = controllers
	end
	Add(arrow)
end
----------------------------------------------------------------------------------------------------
function draw_arrow_forPB( pb_num, value, pos, tp, parent, bold )
	local tex_params

	if (value ~= nil) and (type(value) == "string") then		-- value is text
		if tp == nil then
			tp = pb_props[pb_num].tp
		else 
			tp.alignment = pb_props[pb_num].tp.alignment	-- in pb_props[num] only alignment is important
		end

		if bold then
			tex_params	= { 512* texture_scale, 307 * texture_scale, texture_scale, texture_scale, false }
		else
			tex_params	= { 512* texture_scale, 320 * texture_scale, texture_scale, texture_scale, true }
		end

		-- consider two-lines value
		local arrow_length = #value*tp.width

		local upper_line_len = string.find(value,"\n")

		if upper_line_len~=nil then
			arrow_length = (upper_line_len-1)*tp.width
		end

		--draw_arrow( arrow_length, pos, tp, parent, nil, tex_params, "MFD_BACKGROUND" )
		--draw_arrow( arrow_length, pos, tp, parent, nil, tex_params, IND_MPD_MATERIAL_DARK_GREEN )
		draw_arrow( arrow_length, pos, tp, parent, nil, tex_params, tp.material )
	end
end
function addTwoLineMenuBarrierLR( pb_num ) -- for special case: two labels appointed to PB and one of then is one line. Only affects the L and R sides. For T and D must be adjusted individually
	pb_props[pb_num].show_barrier = false
	draw_disable_line( pb_num, IND_MPD_1024_MATERIAL_DARK_GREEN, 1, parent, {{"DSPLS_PB_Selectabled", pb_num}} )
end
