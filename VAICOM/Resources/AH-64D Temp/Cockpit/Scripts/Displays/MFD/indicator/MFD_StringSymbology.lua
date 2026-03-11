

fontPrefix				=	readParameter( "FONT_PREFIX" )


-- glyph_height		- [indicator units] height of glyph
-- glyph_width		- [indicator units] width of glyph
-- interglyph_space	- [indicator units] distance between horizontally adjacent glyphs
-- interline_space	- [indicator units] vertical space between adjacent lines
function make_stringdefs(glyph_height, glyph_width, interglyph_space, interline_space)
	local scale = GetScale()
	return {
		scale * glyph_height,
		scale * glyph_width,
		scale * interglyph_space,
		scale * interline_space,
	}
end

-- WARNING!
-- Do not define your own sizes!

-- AH-64D
FNT_DFLT_MPD_8_12		= 12
FNT_DFLT_MPD_10_14		= 14
FNT_DFLT_MPD_14_18		= 18
FNT_DFLT_MPD_17_24		= 24
FNT_DFLT_MPD_20_28		= 28
FNT_DFLT_MPD_28_36		= 36
FNT_DFLT_MPD_37_52		= 52
FNT_DFLT_MPD_ADD_34		= 34

-- Att: aspect ratio of fonts in FontMPD_64.tga and FontMPD_64_inv.tga  is 1.4, so here glyphHeight_xx_xx = glyphWidth_xx_xx for all sizes 

stringdefs_MPD = {}
--										make_stringdefs(	height,	width,	char-space,	line-space)
stringdefs_MPD[FNT_DFLT_MPD_8_12]		= make_stringdefs(	12,		12,		0,			0.3)
stringdefs_MPD[FNT_DFLT_MPD_10_14]		= make_stringdefs(	14,		14,		0,			0.5)
stringdefs_MPD[FNT_DFLT_MPD_14_18]		= make_stringdefs(	18,		18,		0,			0.7)
stringdefs_MPD[FNT_DFLT_MPD_17_24]		= make_stringdefs(	24,		24,		0,			1)
stringdefs_MPD[FNT_DFLT_MPD_20_28]		= make_stringdefs(	28,		28,		0,			1)
stringdefs_MPD[FNT_DFLT_MPD_28_36]		= make_stringdefs(	36,		36*0.9,	0,			1.2)
stringdefs_MPD[FNT_DFLT_MPD_37_52]		= make_stringdefs(	52,		37,		0,			1)
stringdefs_MPD[FNT_DFLT_MPD_ADD_34]		= make_stringdefs(	34,		30,		0.1,		1.2)



-------------------------------------------- LOCAL ---------------------
local PB_TextFont		= FNT_DFLT_MPD_20_28
----Box Margins around PB Menu Items -----------------------------------
MarginsDef				= {  5, 5,  5,  5 }		-- left, down, right, up 
MarginsForMenu			= { 10, 5, 10, 20 }		-- left, down, right, up  ( up bigger for arrow )
-- default TextProperties -----------------------------------------------
text_properties_default = {}
text_properties_default.color			= "GREEN"
text_properties_default.material		= IND_MPD_MATERIAL_GREEN
text_properties_default.alignment		= "CenterTop"
text_properties_default.height			= 28
text_properties_default.width			= mfd_font_aspect * text_properties_default.height --mfd_font_aspect from fonts.lua
text_properties_default.inv				= false
text_properties_default.use_mipfilter	= true
text_properties_default.additive_alpha	= false
text_properties_default.collimated		= false
text_properties_default.border			= false
text_properties_default.margins			= { 5,5,5,5 }
text_properties_default.margins_for_menu= { 10,5,10,20 }

------------------------------------------------------------------------
function parseAligment ( alignment )
	c = {}
	for l in string.gmatch( alignment, "%u+") do
		c[ #c+1] = l
	end
	return c
end
-----------------------------------------------------------------------
function textCoordinatesAlignment( text_len, text_height, alignment, margins )

	local x_left	= - text_len/2
	local x_right	= - x_left
	local y_low		= - text_height/2
	local y_high	= - y_low

	c = parseAligment ( alignment )

	if c[1] == "L" then
		x_left = 0
		x_right = text_len

	elseif c[1] =="R" then
		x_left = - text_len
		x_right = 0

	elseif c[1] =="C" then
		x_left = - text_len/2
		x_right = -x_left
	end

	if c[2] == "T" then
		y_low = -text_height
		y_high = 0

	elseif c[2] =="B" then
		y_low = 0
		y_high = text_height

	elseif c[2] =="C" then
		y_low = -text_height/2
		y_high = -y_low
	end

	if margins ~= nil then
		x_left	= x_left	- margins[1]
		x_right	= x_right	+ margins[3]
		y_low	= y_low		- margins[2]
		y_high	= y_high	+ margins[4]
	end
	return x_left, x_right, y_low, y_high
end

-- ----------------------------------------------------------------------
 label_count = 0
function getLabelName()
	label_count = label_count + 1
	return "LABEL " .. label_count -- "Label_"
end

-- -----------------------------------------------------------------
function createTextProperty( height, color, material, alignment, inv, border, margins, margins_for_menu, use_mipfilter, additive_alpha, collimated )
	text_prop = {}
	text_prop.color				= color				or text_properties_default.color
	text_prop.material			= material			or text_properties_default.material
	text_prop.alignment			= alignment			or text_properties_default.alignment
	text_prop.height			= height			or text_properties_default.height
	text_prop.steep_y			= text_prop.height * 1.35
	text_prop.width				= text_prop.height * mfd_font_aspect
	text_prop.inv				= inv				or text_properties_default.inv
	text_prop.border			= border			or text_properties_default.border
	text_prop.use_mipfilter		= use_mipfilter		or text_properties_default.use_mipfilter
	text_prop.additive_alpha	= additive_alpha	or text_properties_default.additive_alpha
	text_prop.collimated		= collimated		or text_properties_default.collimated
	text_prop.margins			= margins			or text_properties_default.margins
	text_prop.margins_for_menu	= margins_for_menu	or text_properties_default.margins_for_menu
	return text_prop
end

-- create TextProperties for any possible cases------------------
tp_default					= createTextProperty()
tp_default_border			= createTextProperty(nil, nil,		nil,					nil, nil, true)
tp_default_inv				= createTextProperty(nil, nil,		nil,					nil, true )
tp_default_darkgreen_border	= createTextProperty(nil, nil,		IND_MPD_MATERIAL_DARK_GREEN,	nil, nil, true)
tp_28_red					= createTextProperty( 28, "RED",	IND_MPD_MATERIAL_RED )
tp_28_red_inv				= createTextProperty( 28, "RED",	IND_MPD_MATERIAL_RED,	nil, true )
tp_def_yellow				= createTextProperty(nil, "YELLOW",	IND_MPD_MATERIAL_YELLOW )
tp_def_yellow_inv			= createTextProperty(nil, "YELLOW",	IND_MPD_MATERIAL_YELLOW, "CenterCenter", true )
tp_36_white					= createTextProperty( 36, "WHITE",	IND_MPD_MATERIAL_WHITE,  "CenterCenter" )
tp_36_white_center_bottom	= createTextProperty( 36, "WHITE",	IND_MPD_MATERIAL_WHITE,  "CenterBottom" )

tp_28						= createTextProperty( 28, nil,		nil,  "LeftBottom" )
tp_28_white					= createTextProperty( 28, "WHITE",	IND_MPD_MATERIAL_WHITE	)
tp_28_white_inv				= createTextProperty( 28, "WHITE",	IND_MPD_MATERIAL_WHITE, nil, true	)
tp_28_white_left_bottom		= createTextProperty( 28, "WHITE",	IND_MPD_MATERIAL_WHITE, "LeftBottom")
tp_28_white_center_bottom	= createTextProperty( 28, "WHITE",	IND_MPD_MATERIAL_WHITE, "CenterBottom")
tp_28_white_center_center	= createTextProperty( 28, "WHITE",	IND_MPD_MATERIAL_WHITE, "CenterCenter")
tp_28_white_right_bottom	= createTextProperty( 28, "WHITE",	IND_MPD_MATERIAL_WHITE, "RightBottom" )
tp_28_right_bottom			= createTextProperty( 28, nil,		nil,  "RightBottom" )
tp_28_center_bottom			= createTextProperty( 28, nil,		nil,  "CenterBottom" )
tp_28_center_top			= createTextProperty( 28, nil,		nil,  "CenterTop" )
tp_28_left_center			= createTextProperty( 28, nil,		nil,  "LeftCenter" )
tp_36						= createTextProperty( 36, nil,		nil,  "LeftBottom" )
tp_36_right_bottom			= createTextProperty( 36, nil,		nil,  "RightBottom" )
tp_36_center_center			= createTextProperty( 36, nil,		nil,  "CenterCenter" )
tp_36_center_top			= createTextProperty( 36, nil,		nil,  "CenterTop" )
tp_36_right_bottom_border	= createTextProperty( 36, nil,		nil,  "RightBottom", nil, true )
tp_36_yellow				= createTextProperty( 36, "YELLOW",	IND_MPD_MATERIAL_YELLOW,  "LeftBottom" )
tp_36_red					= createTextProperty( 36, "RED",	IND_MPD_MATERIAL_RED,  "LeftBottom" )
tp_36_border				= createTextProperty( 36, nil,		nil,  "LeftBottom", nil, true )
tp_36_white_inv				= createTextProperty( 36, "WHITE",	IND_MPD_MATERIAL_WHITE,  nil, true )
tp_36_center_bottom			= createTextProperty( 36, nil,		nil,  "CenterBottom" )
tp_36_left_center			= createTextProperty( 36, nil,		nil,  "LeftCenter" )
tp_36_right_center			= createTextProperty( 36, nil,		nil,  "RightCenter" )
tp_36_right_center_white	= createTextProperty( 36, "WHITE",	IND_MPD_MATERIAL_WHITE,   "RightCenter" )
tp_36_blue_center			= createTextProperty( 36, "BLUE",	IND_MPD_MATERIAL_BLUE,   "CenterCenter" )
tp_36_brown_center			= createTextProperty( 36, "BROWN",	IND_MPD_MATERIAL_BROWN,   "CenterCenter" )
tp_52						= createTextProperty( 52, nil,		nil,  "LeftBottom" )

tp_def_top					= createTextProperty(nil, nil, nil, "CenterTop" ) -- only alignment
tp_def_bottom				= createTextProperty(nil, nil, nil, "CenterBottom" )
tp_def_right				= createTextProperty(nil, nil, nil, "RightTop" )
tp_def_left					= createTextProperty(nil, nil, nil, "LeftTop" )
tp_def_left_center			= createTextProperty(nil, nil, nil, "LeftCenter" )
tp_def_right_center			= createTextProperty(nil, nil, nil, "RightCenter" )
tp_24						= createTextProperty( 24, nil, nil )
tp_24_left_center			= createTextProperty( 24, nil, nil, "LeftCenter" )
tp_24_center_top			= createTextProperty( 24, nil, nil, "CenterTop" )
tp_24_center_bottom			= createTextProperty( 24, nil, nil, "CenterBottom" )

tp_black_left				= createTextProperty(nil, "SOLID_BLACK", IND_MPD_SOLID_BLACK	, "LeftTop" )

tp_24_brown_right_center	= createTextProperty( 24, "BROWN",	IND_MPD_MATERIAL_BROWN, "RightCenter")
tp_24_brown_left_center		= createTextProperty( 24, "BROWN",	IND_MPD_MATERIAL_BROWN, "LeftCenter")
tp_24_brown_center			= createTextProperty( 24, "BROWN",	IND_MPD_MATERIAL_BROWN, "CenterBottom")

tp_24_blue_right_center		= createTextProperty( 24, "BLUE",	IND_MPD_MATERIAL_BLUE, "RightCenter")
tp_24_blue_left_center		= createTextProperty( 24, "BLUE",	IND_MPD_MATERIAL_BLUE, "LeftCenter")
tp_24_blue_center			= createTextProperty( 24, "BLUE",	IND_MPD_MATERIAL_BLUE, "CenterBottom")
tp_24_white					= createTextProperty( 24, "WHITE",	IND_MPD_MATERIAL_WHITE	)

tp_18_black					= createTextProperty( 18, "YELLOW",	IND_MPD_MATERIAL_BLACK, "CenterBottom" )
tp_18_yellow				= createTextProperty( 18, "YELLOW",	IND_MPD_MATERIAL_YELLOW, "CenterBottom" )

tp_14_black					= createTextProperty( 14, "YELLOW",	IND_MPD_MATERIAL_BLACK, "CenterBottom" )
tp_14_yellow				= createTextProperty( 14, "YELLOW",	IND_MPD_MATERIAL_YELLOW, "CenterBottom" )

tp_18_right_center			= createTextProperty( 18, nil, nil, "RightCenter" )

-- -----------------------------------------------------------------
function setTextProperties(elem, text_properties, bold )

	if text_properties == nil then
		elem.alignment		= text_properties_default.alignment
		elem.stringdefs		= get_stringdefs_MPD_by_height(text_properties_default.height)
		elem.use_mipfilter	= text_properties_default.use_mipfilter
		elem.additive_alpha	= text_properties_default.additive_alpha
		elcollimated		= text_properties_default.collimated
		local font_name		= fontPrefix..text_properties_default.color
		
		if bold ~= nil and bold then
			elem.material	= text_properties_default.inv and (font_name .."_inv_bold") or font_name.."_bold"
		else
		--	elem.material	= text_properties_default.inv and (font_name .."_inv") or font_name
			elem.material	= text_properties_default.inv and (font_name .."_inv_bold") or font_name
		end
	else
		elem.alignment		= text_properties.alignment or text_properties_default.alignment
		elem.stringdefs		= get_stringdefs_MPD_by_height(text_properties.height or text_properties_default.height)
		elem.use_mipfilter	= text_properties.use_mipfilter or text_properties_default.use_mipfilter
		elem.additive_alpha	= text_properties.additive_alpha  or text_properties_default.additive_alpha
		elem.collimated		= text_properties.collimated  or text_properties_default.collimated
		local font_name		= text_properties.color and (fontPrefix..text_properties.color) or (fontPrefix..text_properties_default.color)

		if bold ~= nil and bold then
			elem.material	= text_properties.inv and (text_properties.inv and (font_name .."_inv_bold") or font_name.."_bold" ) or (text_properties_default.inv and (font_name .."_inv_bold") or font_name.."_bold")
		else
			elem.material	= text_properties.inv and (text_properties.inv and (font_name .."_inv_bold") or font_name ) or (text_properties_default.inv and (font_name .."_inv_bold") or font_name)
		--	elem.material	= text_properties.inv and (text_properties.inv and (font_name .."_inv") or font_name ) or (text_properties_default.inv and (font_name .."_inv") or font_name)
		end
	end
end
-----------------------------------------------------------------------
function draw_box_offset( init_pos, x_left_offset, x_right_offset, y_low_offset, y_high_offset, material, parent, bold ) -- for addBorderAroundText  offset parametrs 
	local tex_param_default = texture_scale
	local box			= CreateElement "ceBoundingTexBox"
	box.width			= info_box_line_width
	box.material		= material
	if parent ~= nil then
		box.parent_element	= parent
		box.name			= parent.."_b"
		box.init_pos		= {0,0}
	else 
		box.init_pos		= init_pos
	end 
	if bold then
		box.tex_params		= {{0.1,309.0*texture_scale},{0.9,309.0*texture_scale}, {texture_scale, tex_param_default }} --0.7*1.5
	else
		box.tex_params		= {{0.1,498.5*texture_scale},{0.9,498.5*texture_scale}, {texture_scale, tex_param_default }} --0.7*1.5
	end
	box.vertices		= {{x_left_offset, y_low_offset},{ x_right_offset, y_low_offset},{ x_right_offset, y_high_offset},{x_left_offset, y_high_offset}}						
	Add(box)
end
--------------------------------------------------------------------
function addBorderAroundText( pos, text, text_properties, margins, parent, bold )
	local border_width = #text*text_properties.width
	local border_height = text_properties.height
	
	local line1_len, line2_len = string.find(text,"\n")
	
	if line1_len~=nil then
		line1_len, line2_len = line1_len-1, #text-line2_len
		
		if line1_len > line2_len then
			border_width = line1_len*text_properties.width
		else
			border_width = line2_len*text_properties.width
		end
		
		border_height = border_height*2 + 1.2	-- 1.2 is interline offset
	end

	local x_left, x_right, y_low, y_high = textCoordinatesAlignment( border_width, border_height, text_properties.alignment, margins )
	draw_box_offset( pos, x_left, x_right, y_low, y_high,  text_properties.material, parent, bold )
end
-------------------------------------------------------------------
function addFlexBorderAroundText( hosts, text_properties, parent, bold )
	AddFlexibleFrameForLabel(hosts, parent, nil, text_properties.material, nil, bold)
end
-------------------------------------------------------------------
function addStringBackground( text, pos, text_properties, controllers, formats, margins, name, parent, bold, is_transparent )

	local elem = addString( text, pos, text_properties, controllers, formats, margins,  name, parent, bold )
	local fon_mat = InformationBackgroundMaterial

	if is_transparent==true then
		fon_mat = "MFD_TRANSPARENT"			-- TODO
	end

	elem.BackgroundMaterial	= fon_mat

	if text == " " then
		elem.UseBackground		=  false
	else 
		elem.UseBackground		=  true
	end

	return elem
end
-------------------------------------------------------------------
function addString( text, pos, text_properties, controllers, formats, margins, name, parent, bold )
	local elem			= CreateElement "ceStringPoly"
	setTextProperties(elem, text_properties, bold)

	elem.name				= name or getLabelName()
	elem.init_pos			= {pos[1], pos[2], 0}
	elem.isdraw				= true 
	elem.value				= text
	
	if margins ~= nil then
		elem.init_pos		= {pos[1]+margins[1], pos[2]+margins[2], 0}
	end

	if controllers ~= nil then
		elem.controllers	= controllers
	end
	
	if formats ~= nil then
		elem.formats		= formats
	end
	
	if parent ~= nil then
		elem.parent_element = parent
	end
	Add(elem)

	return elem
end
-------------------------------------------------------------------
function addText( text, pos, text_properties, controllers, formats, margins, name, parent, h_clip_relation, level, bold, is_transparent )
	local elem = addStringBackground( text, pos, text_properties, controllers, formats, margins, name, parent, bold, is_transparent )
	--if text_properties ~= nil then
	--	if text_properties.inv == true then
	--		text_properties.inv = false
	--		local color = text_properties.color
	--		text_properties.color = "BLACK"
	--		local mat = text_properties.material
	--		text_properties.material = IND_MPD_MATERIAL_BLACK
	--		addStringBackground( text, pos, text_properties, controllers, formats, margins, nil, parent, true, false )
	--		text_properties.inv = true
	--		text_properties.color = color
	--		text_properties.material = mat
	--	end
	--end
	
	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
		elem.level				= level
	end

	if text_properties ~= nil and text_properties.border == true then 
		addFlexBorderAroundText( {elem}, text_properties, elem.name, bold )
	end

	return elem, elem.name
end
-------------------------------------------------------------------
function addSimpleText( text, pos, text_properties, controllers, formats, margins, name, parent, h_clip_relation, level, bold )
	local elem = addString( text, pos, text_properties, controllers, formats, margins, name, parent, bold )

	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
		elem.level				= level
	end

	if text_properties ~= nil and text_properties.border == true then
		addFlexBorderAroundText( {elem}, text_properties, elem.name, bold )
	end

	return elem, elem.name
end
-------------------------------------------------------------------
function addTextUnderlined(text, pos, text_properties, controllers, formats, margins, name, parent, h_clip_relation, level, bold, is_transparent)
	local PH = addPlaceholder(name.."_Placeholder", pos, parent, nil)
	
	addText( text, {0.0,0.0}, text_properties, controllers, formats, margins, name, PH.name, h_clip_relation, level, bold, is_transparent )
	
	local line_len = #text * text_properties.width * 1.04
	local line_len02 = line_len / 2
	local line_y = -text_properties.height*1.2
	
	draw_line( {{-line_len02,line_y},{line_len02,line_y}}, text_properties.material, PH.name, 3 )	-- IND_MPD_MATERIAL_GREEN
end

------------------------------------------- GLOBAL --------------------------------------------

--------------------------------------------------------------------------------------
function addTextSizeClipMask(text, StrLen, RowNum)
	addSizeClipMask(text, StrLen * 15, RowNum * 19)
end

--------------------------------------------------------------------------------------------------------
function AddFlexibleFrameForLabel(Hosts, parent, controllers, material, name, bold)
	local GH = {}
	
	if Hosts ~= nil then
		for k, v in pairs(Hosts) do
			GH[#GH + 1] = v.name
		end
	end
	
	local Frame				= CreateElement "ceBoundingTexBox"
	
	if name == nil then
		for k, v in pairs(GH) do
			Frame.name = v.."_b"
		end
	else
		Frame.name = name
	end
	
	local common_line_width = 5
	
	Frame.material			= material or IND_MPD_MATERIAL_GREEN
	Frame.width				= common_line_width+3	-- 3 is indent
	if bold == true then
		Frame.tex_params	= {{0.1,312.0*texture_scale},{0.9,312.0*texture_scale},{texture_scale, texture_scale}}
	else
		Frame.tex_params	= {{0.1,504.0*texture_scale},{1.0,504.0*texture_scale},{texture_scale, texture_scale}}	
	end
	Frame.controllers		= controllers
	Frame.geometry_hosts	= GH
	Frame.parent_element	= parent
	Frame.use_mipfilter		= true
	
	Add(Frame)
	return Frame
end
--------------------------------------------------------------------------------------------------------
scale_coeff				= display_size_pix/720
local font_char_size_w		= 17 * scale_coeff
local font_char_space		= 0 * scale_coeff

function getTextWidth(symbol_num)
	return font_char_size_w * symbol_num + font_char_space * (symbol_num - 1)
end
