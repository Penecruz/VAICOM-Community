dofile(LockOn_Options.script_path.."symbology_defs.lua")
dofile(LockOn_Options.script_path.."Displays/Common/Units.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/device/MFD_device_IDs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Page_Defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Materials.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_PB_Defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_StringSymbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_MenuSymbology.lua")


-- display size:
display_size_pix		= 1024
display_size_pix_05		= display_size_pix/ 2

SetCustomScale(pix_to_inch * InToMeter)

-- Video Area
video_area_w			= display_size_pix
video_area_h			= display_size_pix 
video_area_w_05			= video_area_w / 2
video_area_h_05			= video_area_h / 2

c_scope_w				= 512
c_scope_h				= c_scope_w

texture_scale			= 1/512
texture_scale_1024		= 1/1024
box_ind					= {0,1,2,2,3,0}
np_mask_ind_up			= {0, 1, 2, 0, 2, 3, 3, 4, 2 }
np_mask_verts_up		= { {-28, -480}, {28,-480}, {28,-26}, {-28,-26}, {0,0}, {0,-26} }
rotor_mask_ind_up		= {0, 1, 2, 0, 2, 3, 3, 4, 5, 2, 4, 5 }
rotor_mask_verts_up		= { {-42, -480}, {42,-480}, {42,-26}, {-42,-26}, {0,0}, {0,-26} }

rotor_mask_ind_dn		= {0, 1, 2, 0, 2, 5, 4, 5, 6, 4, 2, 3 }
rotor_mask_verts_dn		= { {-42, -480}, {42,-480}, {42,0}, {42,26}, {0,0}, {-42,0}, {-42,26} }

rotor_mask_ind_mid		= {0, 1, 2, 0, 2, 3 }
rotor_mask_verts_mid	= { {-42, -480}, {42,-480}, {42,0}, {-42,0} }

info_box_line_width		= 3
RoundingRadius			= 16

InformationBackgroundMaterial = "MFD_BACKGROUND"	-- TODO: "MFD_TRANSLUCENT" when there is video underlay on MPD

-------------------------------------------- LOCAL ---------------------
function addElem(elem)
	elem.additive_alpha	= additive_alpha
	Add(elem)
end
--------------------------------------------------------------------------------------------------------------
function resetMask(mask, level, parent)
	local area				= createMask(mask.name.."_Reset", mask.vertices, mask.indices, mask.init_pos, parent, nil, nil)
	area.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
	area.level				= DEFAULT_LEVEL + level
	return area
end
------------------------------------------------------------------------
function setElemsClipLevel(obj, level)
	if type(obj) == "table" then
		for i, o in pairs(obj) do
			setClipLevel(o, level)
		end
	else
		setClipLevel(obj, level)
	end
end
------------------------------------------------------------------------
local count = 0
local function counter()
	count = count + 1
	return count
end
------------------------------------------------------------------------
function parseAligment ( alignment )
	c = {}
	for l in string.gmatch( alignment, "%u+") do
		c[ #c+1] = l
	end
	return c
end
------------------------------------------------------------------------
function draw_dotted_line( verts, material, parent, width, name, controllers, h_clip_relation, level )
	local tex_param_default = texture_scale
	local scale = 1
	if width ~= nil then
		scale = 3 / width
	end
	local elem			= CreateElement "ceSimpleLineObject"
	elem.material			= material
	elem.width			= width or 3
	if parent ~= nil then
		elem.parent_element = parent
	end
	elem.tex_params		= {{0.0,490*texture_scale},{0.9,490*texture_scale}, {texture_scale*scale, tex_param_default*scale }}
	elem.vertices			= verts

	if controllers ~= nil then
		elem.controllers	= controllers
	end
	if name ~= nil then
		elem.name	= name
	end

	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
	end
	
	if level ~= nil then
		elem.level				= level
	end

	Add(elem)
	return elem
end

------------------------------------------------------------------------
function draw_line( verts, material, parent, width, name, controllers, h_clip_relation, level )
	local tex_param_default = texture_scale
	local scale = 1
	if width ~= nil then
		scale = 3 / width
	end
	local elem			= CreateElement "ceSimpleLineObject"
	elem.material			= material
	elem.width			= width or 3
	if parent ~= nil then
		elem.parent_element = parent
	end
	elem.tex_params		= {{0.0,498.5*texture_scale},{0.9,498.5*texture_scale}, {texture_scale*scale, tex_param_default*scale }}
	elem.vertices			= verts

	if controllers ~= nil then
		elem.controllers	= controllers
	end
	if name ~= nil then
		elem.name	= name
	end

	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
	end
	
	if level ~= nil then
		elem.level				= level
	end

	Add(elem)
	return elem
end

-- ----------------------------------------------------------------------
postfix_count = 0
function getPostfix()
	postfix_count = postfix_count + 1
	return "_" .. postfix_count
end

function get_stringdefs_MPD_by_height(height)
	return stringdefs_MPD[height]
end

function getPosPB(pb_num)
	return PB_Pos[pb_num]
end


-- -----------------------------------------------------------------
fill_pb_props()

-----------------------------------------------------------------------
function draw_label_box_offset( Hosts, material, parent ) -- for addBorderAroundText  offset parametrs 
	local GH = {}
	
	if Hosts ~= nil then
		for k, v in pairs(Hosts) do
			GH[#GH + 1] = v.name
		end
	end
	
	local box			= CreateElement "ceBoundingTexBox"
	
	for k, v in pairs(GH) do
		box.name = v.."_b"		
	end
	
	box.material			= material or IND_MPD_1024_MATERIAL_WHITE
	box.width				= 15	

	elem.tex_params		= {310 * texture_scale_1024, 940 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	box.geometry_hosts	= GH
	box.use_mipfilter	= true
	if parent ~= nil then
		box.parent_element	= parent
	end 
	Add(box)
end
--------------------------------------------------------------------
function add_MeshBox(Hosts)

	local GH = {}
	
	if Hosts ~= nil then
		for k, v in pairs(Hosts) do
			GH[#GH + 1] = v.name
		end
	end
	
	local elem					= CreateElement "ceMeshPoly"
	elem.primitivetype			= "triangles"
	for k, v in pairs(GH) do
		elem.name = elem.name .. "_" .. v
	end
	
	elem.primitivetype		= "triangles"
	elem.material			= "YELLOW"
	elem.geometry_hosts		= GH

	Add(elem)
	elem.use_mipfilter = true
	return elem
end
-------------------------------------------------------------------
local id = 0
local function getUnicID()
	id = id + 1
	return "_id:"..id
end
------------------------------------------- GLOBAL --------------------------------------------

----------------------------------------------------
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

-------------------------------------------------------------------------------------------------
function draw_border( pos_up, pos_dn,  max_item_len, num_lines, tp, parent, is_transparent )
	max_item_len = (max_item_len + 1) * tp.width												-- lenth in pixels

	local c = parseAligment ( tp.alignment )
--				createVertsForOneLine( pos_up, pos_dn, max_item_len, num_lines, c, tp )	
	local verts = 	createVertsForOneLine( pos_up, pos_dn, max_item_len, num_lines, c, tp )		
	
	draw_line( verts, tp.material, parent, nil, nil, nil, h_clip_relations.COMPARE, DEFAULT_LEVEL )
end
--------------------------------------------------------------------------------------------------
function calculateCaptionPos( pos_up, pos_dn, max_item_len, num_lines, caption, c, tp )
	local verts				= { { pos_up, {0, 0}, {0, 0} }, { pos_dn, {0, 0}, {0, 0} } }		-- two lines with 3 points in each
	local caption_space
	local pos = {}
	
	if c[1] == "L"  then
		verts[1][1] = { verts[1][1][1], verts[1][1][2] + 1.5 * tp.height }				--  point #1 for upper or left line
		verts[2][1] = { verts[2][1][1], verts[2][1][2] - 2.5 * tp.height }				--  point #1 for lower  right line
		verts[1][2] = { verts[1][1][1] + max_item_len + 10, verts[1][1][2] }			--  point #2 for upper line
		verts[2][2] = { verts[2][1][1] + max_item_len + 10, verts[2][1][2] }			--  point #2 for lower line
		caption_space = ( verts[1][1][2] - verts[2][1][2] - #caption * tp.height )/2 
		pos = { verts[1][2][1]-3, verts[1][2][2] - caption_space }


	elseif c[1] =="R" then
		verts[1][1] = { verts[1][1][1], verts[1][1][2] + 1.5 * tp.height }				--  point #1 for upper or left line
		verts[2][1] = { verts[2][1][1], verts[2][1][2] - 2.5 * tp.height }				--  point #1 for lower  right line
		verts[1][2] = { verts[1][1][1] - max_item_len, verts[1][1][2] }					--  point #2 for upper line
		verts[2][2] = { verts[2][1][1] - max_item_len, verts[2][1][2] }					--  point #2 for lower line
		caption_space = ( verts[1][1][2] - verts[2][1][2] - #caption * tp.height )/2
		pos = { verts[1][2][1]+3, verts[1][2][2] - caption_space }


	elseif c[2] == "T" then
		verts[1][1] = { verts[1][1][1] - 3.0 * tp.width, verts[1][1][2] }				--  point #1 for upper or left line
		verts[2][1] = { verts[2][1][1] + 3.0 * tp.width, verts[2][1][2] }				--  point #1 for lower  right line
		verts[1][2] = { verts[1][1][1], verts[1][1][2] - (num_lines + 1) * tp.height }	--  point #2 for upper line
		verts[2][2] = { verts[2][1][1], verts[2][1][2] - (num_lines + 1) * tp.height}	--  point #2 for lower line
		caption_space = ( verts[2][1][1] - verts[1][1][1] - #caption * tp.width )/2 
		verts[1][3] = { verts[1][2][1] + caption_space, verts[1][2][2]  }				--  point #3 for upper line
		verts[2][3] = { verts[2][2][1] - caption_space, verts[2][2][2]  }				--  point #3 for lower line
		pos = {  verts[1][3][1] + ( verts[2][3][1] - verts[1][3][1] )/2,	verts[2][2][2]+3  }

	elseif c[2] =="B" then
		verts[1][1] = { verts[1][1][1] - 3.0 * tp.width, verts[1][1][2] }				--  point #1 for upper or left line
		verts[2][1] = { verts[2][1][1] + 3.0 * tp.width, verts[2][1][2] }				--  point #1 for lower  right line
		verts[1][2] = { verts[1][1][1], verts[1][1][2] + (num_lines + 1) * tp.height }	--  point #2 for upper line
		verts[2][2] = { verts[2][1][1], verts[2][1][2] + (num_lines + 1) * tp.height}	--  point #2 for lower line
		caption_space = ( verts[2][1][1] - verts[1][1][1] - #caption * tp.width)/2 
		verts[1][3] = { verts[1][2][1] + caption_space, verts[1][2][2]  }				--  point #3 for upper line
		verts[2][3] = { verts[2][2][1] - caption_space, verts[2][2][2]  }				--  point #3 for lower line	
		pos = {  verts[1][3][1] + ( verts[2][3][1] - verts[1][3][1] )/2,	verts[2][2][2]-3  }
	end
	return pos
end
--------------------------------------------------------------------------------------------------
function createVertsForLines( pos_up, pos_dn, max_item_len, num_lines, caption, c, tp )			-- two lines with 3 points in each
	local verts				= { { pos_up, {0, 0}, {0, 0} }, { pos_dn, {0, 0}, {0, 0} } }		-- two lines with 3 points in each
	local caption_space

	if c[1] == "L"  then
		verts[1][1] = { -video_area_w_05, verts[1][1][2] + 1.5 * tp.height }	--  point #1 for upper or left line
		verts[2][1] = { -video_area_w_05, verts[2][1][2] - 2.5 * tp.height }	--  point #1 for lower  right line
		verts[1][2] = { pos_up[1] + max_item_len + 10, verts[1][1][2] }			--  point #2 for upper line
		verts[2][2] = { pos_dn[1] + max_item_len + 10, verts[2][1][2] }			--  point #2 for lower line
		caption_space = ( verts[1][1][2] - verts[2][1][2] - #caption * tp.height )/2
		verts[1][3] = { verts[1][2][1], verts[1][2][2] - caption_space }		--  point #3 for upper line
		verts[2][3] = { verts[2][2][1], verts[2][2][2] + caption_space }		--  point #3 for lower line

	elseif c[1] =="R" then
		verts[1][1] = { video_area_w_05, verts[1][1][2] + 1.5 * tp.height }		--  point #1 for upper or left line
		verts[2][1] = { video_area_w_05, verts[2][1][2] - 2.5 * tp.height }		--  point #1 for lower  right line
		verts[1][2] = { pos_up[1] - max_item_len, verts[1][1][2] }				--  point #2 for upper line
		verts[2][2] = { pos_dn[1] - max_item_len, verts[2][1][2] }				--  point #2 for lower line
		caption_space = ( verts[1][1][2] - verts[2][1][2] - #caption * tp.height )/2
		verts[1][3] = { verts[1][2][1], verts[1][2][2] - caption_space }		--  point #3 for upper line
		verts[2][3] = { verts[2][2][1], verts[2][2][2] + caption_space }		--  point #3 for lower line

	elseif c[2] == "T" then
		verts[1][1] = { verts[1][1][1] - 3.0 * tp.width,  video_area_h_05 }				--  point #1 for upper or left line
		verts[2][1] = { verts[2][1][1] + 3.0 * tp.width,  video_area_h_05}				--  point #1 for lower  right line
		verts[1][2] = { verts[1][1][1], pos_up[2] - (num_lines + 1) * tp.height }		--  point #2 for upper line
		verts[2][2] = { verts[2][1][1], pos_dn[2] - (num_lines + 1) * tp.height}		--  point #2 for lower line
		caption_space = ( verts[2][1][1] - verts[1][1][1] - #caption * tp.width )/2
		verts[1][3] = { verts[1][2][1] + caption_space, verts[1][2][2]  }				--  point #3 for upper line
		verts[2][3] = { verts[2][2][1] - caption_space, verts[2][2][2]  }				--  point #3 for lower line

	elseif c[2] =="B" then
		verts[1][1] = { verts[1][1][1] - 3.0 * tp.width, -video_area_h_05 }				--  point #1 for upper or left line
		verts[2][1] = { verts[2][1][1] + 3.0 * tp.width, -video_area_h_05 }				--  point #1 for lower  right line
		verts[1][2] = { verts[1][1][1], pos_up[2] + (num_lines + 1) * tp.height + 8 }	--  point #2 for upper line
		verts[2][2] = { verts[2][1][1], pos_dn[2] + (num_lines + 1) * tp.height + 8 }	--  point #2 for lower line
		caption_space = ( verts[2][1][1] - verts[1][1][1] - #caption * tp.width)/2
		verts[1][3] = { verts[1][2][1] + caption_space, verts[1][2][2]  }				--  point #3 for upper line
		verts[2][3] = { verts[2][2][1] - caption_space, verts[2][2][2]  }				--  point #3 for lower line
	end
	return verts
end
--------------------------------------------------------------------------------------------------
function createVertsForOneLine( pos_up, pos_dn, max_item_len, num_lines, c, tp )			-- two lines with 3 points in each
	local verts				= { pos_up, {0, 0}, {0, 0}, pos_dn }		-- one line with 3 points in each

	if c[1] == "L"  then
		verts[1] = { -video_area_w_05, verts[1][2] + 1.5 * tp.height }		--  point #1 
		verts[2] = { pos_up[1] + max_item_len + 10, verts[1][2] }			--  point #2 
		verts[3] = { verts[2][1], pos_dn[2] - 1.5 * tp.height }				--  point #3 
		verts[4] = { -video_area_w_05, verts[3][2] }						--  point #4 

	elseif c[1] =="R" then
		verts[1] = { video_area_w_05, verts[1][2] + 1.5 * tp.height }		--  point #1
		verts[2] = { verts[1][1] - max_item_len, verts[1][2] }				--  point #2
		verts[3] = { verts[2][1], pos_dn[2] - 1.5 * tp.height }				--  point #3
		verts[4] = { video_area_w_05, verts[3][2]  }
	-- T&B TODO
	end
	return verts
end
--------------------------------------------------------------------------------------------------
function createVertsForBackground( verts, c )
	local verts_bg	=
	{
		{verts[1][1][1],verts[1][1][2]},
		{verts[1][2][1],verts[1][2][2]},
		{verts[2][2][1],verts[2][2][2]},
		{verts[2][1][1],verts[2][1][2]}
	}
	-- expand background to the border of display
	if c[1] == "L" then
		verts_bg[1][1] = -display_size_pix_05
		verts_bg[4][1] = -display_size_pix_05
		verts_bg[2][1] = verts_bg[2][1] -- -4
		verts_bg[3][1] = verts_bg[3][1] -- -4
		
	elseif c[1] =="R" then
		verts_bg[1][1] = display_size_pix_05
		verts_bg[4][1] = display_size_pix_05
		verts_bg[2][1] = verts_bg[2][1]  --+4
		verts_bg[3][1] = verts_bg[3][1]  --+4

	elseif c[2] == "T" then
		verts_bg[1][2] = display_size_pix_05
		verts_bg[4][2] = display_size_pix_05

	elseif c[2] =="B" then
		verts_bg[1][2] = -display_size_pix_05
		verts_bg[4][2] = -display_size_pix_05
	end
	return verts_bg
end
--------------------------------------------------------------------------------------------------
function createVertsForBackgroundWithCaption( verts, c )
	local caption_space = tp_default.height/8

	local verts_bg	=					-- united form of menu background
	{
		{verts[1][1][1],verts[1][1][2]},
		{verts[1][2][1],verts[1][2][2]},
		
		{verts[1][3][1],verts[1][3][2]},
		{verts[1][3][1],verts[1][3][2]},
		
		{verts[2][3][1],verts[2][3][2]},
		{verts[2][3][1],verts[2][3][2]},
		
		{verts[2][2][1],verts[2][2][2]},
		{verts[2][1][1],verts[2][1][2]}
	}

	if c[1] == "L" or c[1] == "R" then
		verts_bg[3][2] = verts[1][3][2] - caption_space
		verts_bg[4][2] = verts[1][3][2] - caption_space
		
		verts_bg[5][2] = verts[2][3][2] + caption_space
		verts_bg[6][2] = verts[2][3][2] + caption_space
	else
		verts_bg[3][1] = verts[1][3][1] + caption_space
		verts_bg[4][1] = verts[1][3][1] + caption_space
		
		verts_bg[5][1] = verts[2][3][1] - caption_space
		verts_bg[6][1] = verts[2][3][1] - caption_space
	end

	local verts_out =					-- same background but separated into rectangle
	{
		{verts_bg[1],	verts_bg[2], verts_bg[3], {0,0}},
		{{0,0},			verts_bg[4], verts_bg[5], {0,0}},
		{verts_bg[6],	verts_bg[7], verts_bg[8], {0,0}}
	}

	-- expand background to the border of display and add offset for caption to not overlap background
	local offsetX, offsetY, offsetLine = 3, 3, info_box_line_width / 2

	if (c[1] == "L" or c[1] == "R") then
		local LR_coef = 1
		if c[1] == "L" then
			LR_coef = -1
		end

		verts_out[1][1][1] = LR_coef * display_size_pix_05
		verts_out[1][1][2] = verts_out[1][1][2] + offsetLine
		verts_out[1][2][1] = verts_out[1][2][1] - LR_coef * offsetLine
		verts_out[1][2][2] = verts_out[1][1][2]
		verts_out[1][3][1] = verts_out[1][2][1]
		verts_out[1][4] = {verts_out[1][1][1],verts_out[1][3][2]}

		verts_out[2][1] = verts_out[1][4]
		verts_out[2][2][1] = verts_out[2][2][1] + LR_coef * offsetX
		verts_out[2][3][1] = verts_out[2][2][1]
		verts_out[2][4] = {verts_out[2][1][1],verts_out[2][3][2]}

		verts_out[3][1][1] = verts_out[1][2][1]
		verts_out[3][2][1] = verts_out[3][1][1]
		verts_out[3][2][2] = verts_out[3][2][2] - offsetLine
		verts_out[3][3][1] = verts_out[1][1][1]
		verts_out[3][3][2] = verts_out[3][2][2]
		verts_out[3][4] = verts_out[2][4]

	elseif (c[2] == "T" or c[2] == "B") then
		local TB_coef = 1
		if c[2] == "B" then
			TB_coef = -1
		end

		verts_out[1][1][1] = verts_out[1][1][1] - offsetLine
		verts_out[1][1][2] = TB_coef * display_size_pix_05
		verts_out[1][2][1] = verts_out[1][1][1]
		verts_out[1][2][2] = verts_out[1][2][2] - TB_coef * offsetLine
		verts_out[1][3][2] = verts_out[1][2][2]
		verts_out[1][4] = {verts_out[1][3][1],verts_out[1][1][2]}

		verts_out[2][1] = verts_out[1][4]
		verts_out[2][2][2] = verts_out[2][2][2] + TB_coef * offsetY
		verts_out[2][3][2] = verts_out[2][2][2]
		verts_out[2][4] = {verts_out[2][3][1],verts_out[2][1][2]}

		verts_out[3][1][2] = verts_out[1][3][2]
		verts_out[3][2][1] = verts_out[3][2][1] + offsetLine
		verts_out[3][2][2] = verts_out[3][1][2]
		verts_out[3][3][1] = verts_out[3][2][1]
		verts_out[3][3][2] = verts_out[1][1][2]
		verts_out[3][4] = verts_out[2][4]
	end

	return verts_out
end
---------------------------------------------------------------------------------------------------
	-- first line is required in format:	{pos_left_up,  controllers,  name, parent, h_space,  text_properties,  margins,  },
	-- next :	{{ text,  controllers,	formats,	margins,	name }, {},{}...}
	-- margins: left, down, right, up
------------------------------------------------------------------------------------------------------
function createInfoBoxes( boxes, up_down )
	for item = 1, #boxes do
		createInfoBoxItem( boxes[item], up_down )
	end
end

--------------------------------------------------------------------------------------------------
	-- first line is required in format:	{pos_left_up,  controllers,  name, parent, h_space,  text_properties,  margins,  },
	-- next :	{{ text, tp, controllers, formats, margins,  name }, {},{}...} 
	-- margins: left, down, right, up 
------------------------------------------------------------------------------------------------------
function addInfoBoxBackground( w, h, pos_l_up, parent, controllers, level, h_clip_relation, is_transparent)
	local r = 10
	local verts =	{
		{ r, 0		}, { w-r, 0	}, { w,	-r }, { w,	-h+r}, { w-r, -h	}, { r,	-h	},
		{ 0, -h+r	}, { 0,	-r	}, { r,	-r }, { w-r, -r	}, { w-r, -h+r	}, { r,	-h+r}
	}

	local fon_mat = InformationBackgroundMaterial

	if is_transparent==true then
		fon_mat = "MFD_TRANSLUCENT"			-- TODO
	end

	local indices = { 0,1,4,0,4,5,7,8,11,7,11,6,9,2,3,9,3,10,0,8,7,9,1,2,10,3,4,11,5,6 }
	local elem = addMesh(nil, verts, indices, pos_l_up,"triangles", parent, controllers, fon_mat)
	
	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
	end
	if level ~= nil then
		elem.level	= level
	end
end
------------------------------------------------------------------------------------------------------
function createInfoBoxItem( box, up_down )
	local max_line_len = 0;
	local	l_up			= box[1][1]
	local	controllers		= box[1][2]
	local	name			= box[1][3] or "PH"..getUnicID()
	local	parent			= box[1][4]
	local	h_space			= box[1][5] or 20
	local	tp				= box[1][6] or text_properties_default -- used for calculate positions
	local	margins			= box[1][7] or { 10,10,10, -10 } -- left, down, right, up 
	local	pos				= { l_up[1]+margins[1], l_up[2]-margins[4] }
	local	line_len		= 0

	local level				= DEFAULT_LEVEL
	local h_clip_relation	= h_clip_relations.NONE
	
	if up_down ~= nil then
		if up_down == 1 then
			level			= DEFAULT_LEVEL+1
			h_clip_relation = h_clip_relations.REWRITE_LEVEL
		else
			level				= DEFAULT_LEVEL
			h_clip_relation		= h_clip_relations.COMPARE
		end
	end

	for item = 2, #box do
		pos				= { pos[1], pos[2] - tp.height - h_space }
		line_len		= calcBoxLen( box[item],  tp )
		max_line_len	= max_line_len > line_len and max_line_len or line_len
	end
	local	r_dn	= { pos[1] + max_line_len + margins[3] , pos[2] - margins[2] }

	local ph = addPlaceholder( name, l_up, nil, controllers )
	local w = math.abs(r_dn[1]-l_up[1])
	local h = math.abs(r_dn[2]-l_up[2])

	addInfoBoxBackground( w, h, {0,0}, ph.name, nil, level, h_clip_relation, false )
	pos	= { margins[1], margins[4] - tp.height } --restore pos
	for item = 2, #box do	
		createBoxLine( pos,  box[item], ph.name, level, h_clip_relation)
		pos = { pos[1], pos[2] - tp.height - h_space }
	end

	draw_box( {0,0}, { w, -h },  material,  ph.name, nil, level, h_clip_relation )
	return ph
end
-------------------------------------------------------------------------------------------------
function calcBoxLen( line, tp )
	local len = 0
	local marg = 0
	local ltp = {}
	local text = {}
	local text_len = text ~= "" and #text or 0
	local last = 0;

	for item = 1, #line do
		marg = line[item][5] ~= nil and line[item][5][1] or 0
		ltp = line[item][2] or tp
		text = line[item][1]
		text_len = text ~= "" and #text or 0

		if line[item][5] ~= nil then
			len = len + marg 
			last = 1
		else		
			len = len +  text_len * ltp.width
			last = 0
		end	
	end 

	if last == 1 then
		local c = parseAligment( ltp.alignment )
		if c[1] == "L"  then
			len = len + text_len * ltp.width
	
		elseif c[1] =="C" then
			len = len + text_len * ltp.width / 2
		end	
	end
	return  len
end
------------------------------------------------------------------------------------------------- 
function createBoxLine( pos, line, parent,  level, h_clip_relation)
	local  line_len = 0
	local pos_cur	= { pos[1], pos[2] }

	for item = 1, #line do
		local text		= line[item][1]
		local tp		= line[item][2] or text_properties_default -- used as real  tp
		local marg		= line[item][5] ~= nil and line[item][5][1] or 0
		local text_len	= text ~= "" and #text or 0

		if line[item][5] ~= nil then
			line_len = line_len + marg 
			pos_cur = { pos[1] + line_len, pos[2] }	
			addSimpleText( text, pos_cur, tp, line[item][3],	line[item][4],	nil,	line[item][6],	parent, h_clip_relation, level )
		else	
			addSimpleText( text, pos_cur, tp, line[item][3],	line[item][4],	nil,	line[item][6],	parent, h_clip_relation, level )
			line_len = line_len +  text_len * tp.width
			pos_cur = { pos[1] + line_len, pos[2] }
		end	
	end 
end
--------------------------------------------------------------------------------------
function addTextClipMask(text, vertical, maskController)
	local openingMask = openMaskArea(0, text.name.."_Mask", {}, {}, text.init_pos, nil, maskController)

	local width, height
	local symbolLen = #text.value

	if vertical then
		symbolLen = math.ceil(symbolLen / 2)
		width = 18
		height = symbolLen *	19
	else
		width = symbolLen * 15
		height =	22
	end

	set_box_w_h(openingMask, width, height)
end
-------------------------------------------------------------------------------------
function addSizeClipMask(symbol, width, height)
	local openingMask = openMaskArea(0, symbol.name.."_Mask", {}, {}, symbol.init_pos)
	set_box_w_h(openingMask, width, height)
end

--------------------------------------------------------------------------------------
function AddBackground(name, invisible, controllers)
	local element			= CreateElement "ceMeshPoly"
	element.name			= name
	element.primitivetype	= "triangles"
	element.material		= "MFD_BACKGROUND"
	element.vertices		= buildBoxVerts(video_area_w, video_area_h, "CenterCenter")
	element.indices			= default_box_indices
	element.blend_mode		= blend_mode.IBM_REGULAR
	element.change_opacity	= false
	
	if invisible == true then
		element.h_clip_relation = h_clip_relations.REWRITE_LEVEL
		element.level			= DEFAULT_LEVEL
		setAsInvisibleMask(element) -- changes material
	end
	
	if controllers ~= nil then
		element.controllers = controllers
	end
	
	Add(element)
end
--------------------------------------------------------------------------------------------------------
function AddRoundCornersWindow(name, pos, width, height, value, tp, material, parent, controllers, alignment, box_cntrs, up_down, is_transparent)
	-- pos is position of center of box
	
	local RoundedLabelBase = addPlaceholder(name.."_plaseholder", pos, parent, controllers)	

	local line_width	= info_box_line_width
	local text_prop		= tp or text_properties_default		
	local box_mat		= material or text_prop.material
	local label_H		= height or 0
	local label_W		= width or 0
	local text			= ""
	local margins		= nil
	
	local level				= DEFAULT_LEVEL
	local h_clip_relation	= h_clip_relations.NONE
	
	if up_down ~= nil then
		if up_down == 1 then
			level			= DEFAULT_LEVEL+1
			h_clip_relation = h_clip_relations.REWRITE_LEVEL
		else
			level				= DEFAULT_LEVEL
			h_clip_relation		= h_clip_relations.COMPARE
		end
	end
	
	if text_prop.border then
		margins = {3,3,3,3}
	end
	
	if type(value) == "string" or type(value) == "number" then
		text = tostring (value)
	elseif type(value) == "table" and #value == 1 then
		text = tostring (value[1][1])
	end
	
	if label_H == 0 and text ~= "" then
		label_H = text_prop.height*1.4
	end
	
	if label_W == 0 and text ~= ""  then
		label_W = text_prop.width*(#text+0.5)+RoundingRadius
	end

	-- fon	
	addInfoBoxBackground( label_W, label_H, {-label_W/2, label_H/2}, RoundedLabelBase.name, nil, level, h_clip_relation, is_transparent )

	-- data
	if type(value) == "string" or type(value) == "number" then

		text_prop.alignment = alignment or "CenterCenter"
	--				addSimpleText( text, pos, text_properties, controllers,		formats,		margins,	name,			parent, h_clip_relation, level, bold )
		local elem = addSimpleText(text,	{0,0},	text_prop,			nil,			nil,		margins,	name.."_text",	RoundedLabelBase.name, h_clip_relation, level)
		elem.stringdefs = text_prop.stringdefs~=nil and text_prop.stringdefs or elem.stringdefs
			
	elseif type(value)=="table" then		-- be like {value, rel_pos, tp, controller, formats}
		
		for i = 1, #value do
			text = tostring (value[i][1])
			
			text_prop = value[i][3] or text_properties_default	
			text_prop.alignment = text_prop.alignment or alignment or "LeftCenter"
			
			--							text,	pos,			text_properties,	controllers,	formats,		margins,	name,				parent
			local elem = addSimpleText(text,	value[i][2],	text_prop,			value[i][4],	value[i][5],	margins,	name.."_text_"..i,	RoundedLabelBase.name, h_clip_relation, level)
			elem.stringdefs = text_prop.stringdefs~=nil and text_prop.stringdefs or elem.stringdefs
		end

	end
	
	-- frame
	addRoundedBox(name.."_frame", {0,0}, "CenterCenter", {label_W, label_H}, RoundingRadius, line_width, RoundedLabelBase.name, nil, box_mat, box_cntrs, level, h_clip_relation)

	return RoundedLabelBase
end
--------------------------------------------------------------------------------------------------------
function AddArrowMenuLabel(pb_num, name, pos, rot, parent, controller, material, draw_background)
	-- pos is position of center of arrow
	
	rot			= rot or 0
	material	= material or IND_MPD_TSD_SYMBOLS_GREEN

	local ArrowLabel_PH = addRotPlaceholder(name.."_BasePH", pos, rot, parent, controller)

	-- dummy controls to get reaction from cursor enter
	createControls( {{ pb_num, "", tp_52 }}, nil, ArrowLabel_PH.name, nil )

	local function DrawTSDTexPoly( tex_bounds, tex_coords, controllers, parent, material, name )	
		-- more scale -> less symbol
		local l_scale = 2.55
		local w_scale = 3.75
		
		local tsd_texture_scale = 1/2048
	
		local x1, y1 = -tex_bounds[1]/w_scale, tex_bounds[2]/l_scale
		local x2, y2 = -x1, -y1
		
		local verts			= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
		
		local elem  		= CreateElement "ceTexPoly"
		elem.vertices		= verts
		elem.indices		= default_box_indices
		elem.tex_params		= {tex_coords[1] * tsd_texture_scale, tex_coords[2] * tsd_texture_scale, w_scale * tsd_texture_scale, l_scale * tsd_texture_scale} 

		setSymbolCommonProperties(elem, name, {0,0}, parent, controllers, material)

		Add(elem)
		return elem
	end

	local function drawArrowsSet(name_suff, material, draw_backgnd, backgnd_pos, texture_pos, parent_name, base_controllers)
		-- TODO: Most likely background is not transparent (black). At least as I see in present photos and pics.
		local bkgrnd_mat = IND_MPD_TSD_MATERIAL_BLACK
		-- local bkgrnd_mat = IND_MPD_TSD_MATERIAL_BLACK_TR

		local tex_w05 = 62
		local tex_h05 = 62

		-- not bold --
		local Base_PH 			= addPlaceholder(name.."_"..name_suff.."_BasePH", {0,0}, parent_name, base_controllers)
		
		local RegularArrow_PH 	= addPlaceholder(name.."_"..name_suff.."_PH", {0,0}, Base_PH.name, {{"DSPLS_PB_BOLD", pb_num, 0}})

		if draw_backgnd ~= false then
			DrawTSDTexPoly( {tex_w05,tex_h05}, backgnd_pos[1], 	nil, RegularArrow_PH.name, bkgrnd_mat,					name.."_"..name_suff.."_arr_bkgrnd" )
		else
			DrawTSDTexPoly( {tex_w05,tex_h05}, texture_pos[1], 	nil, RegularArrow_PH.name, IND_MPD_TSD_MATERIAL_BLACK,	name.."_"..name_suff.."_arr_fon" )
		end
		
		DrawTSDTexPoly( {tex_w05,tex_h05}, texture_pos[1], nil, RegularArrow_PH.name, material,							name.."_"..name_suff.."_arr" )

		-- bold --
		local BoldArrow_PH 		= addPlaceholder(name.."_"..name_suff.."_BoldPH", {0,0}, Base_PH.name, {{"DSPLS_PB_BOLD", pb_num, 1}})

		if draw_backgnd ~= false then
			DrawTSDTexPoly( {tex_w05,tex_h05}, backgnd_pos[2], 	nil, BoldArrow_PH.name, bkgrnd_mat,						name.."_"..name_suff.."_bold_arr_bkgrnd" )
		else
			DrawTSDTexPoly( {tex_w05,tex_h05}, texture_pos[2], 	nil, BoldArrow_PH.name, IND_MPD_TSD_MATERIAL_BLACK,		name.."_"..name_suff.."_bold_arr_fon" )
		end

		DrawTSDTexPoly( {tex_w05,tex_h05}, texture_pos[2], nil, BoldArrow_PH.name, material,							name.."_"..name_suff.."_bold_arr" )
	end

	local tex_pos		= {{64,		703.5},	{64,	832.5}}		-- regular / bold
	local bkgrnd_pos	= {{192,	703.5},	{192,	832.5}}		-- regular / bold
		
	drawArrowsSet("Empty",		material,	true,	bkgrnd_pos,	tex_pos,		ArrowLabel_PH.name,	{{"TSD_ArrowButtonClick", pb_num, 0}})
	drawArrowsSet("Filled",		material,	false,	nil, 		bkgrnd_pos,		ArrowLabel_PH.name,	{{"TSD_ArrowButtonClick", pb_num, 1}})
end
--------------------------------------------------------------------------------------------------------
function AddPagingGroup(name, controllers, controller_IsDraw, is_transparent)
	local tps_wide	= createTextProperty()
	local font_size	= tps_wide.height

	local arr1_x	= -font_size*3.2
	local arr2_x	= font_size*3.2

	local group_pos	= {(pb_props[pb.B3].pos[1]+pb_props[pb.B2].pos[1])/2, pb_props[pb.B3].pos[2]+font_size*0.50}
	
	--tps_wide.stringdefs = {tps_wide.height*GetScale(),tps_wide.height*GetScale(),tps_wide.height*GetScale()*0.05}	-- to set symbols looser
	
	local MenuLabelBase = addPlaceholder(name.."_PagingGroup_plaseholder", group_pos, nil, controller_IsDraw)

	--					text,			pos,				text_properties,	controllers,	formats,	margins,	name,						parent
	local elem = addText("PAGE\n1/10",	{0,font_size*0.50},	tps_wide,			controllers,	nil,		nil,		name.."_Paging_text_lbl",	MenuLabelBase.name, nil, nil, nil, is_transparent)
	--elem.stringdefs = tps_wide.stringdefs
	elem.alignment = "CenterCenter"

	AddArrowMenuLabel(pb.B2, name.."_Paging_Arr1",	{arr1_x,0},	90,		MenuLabelBase.name)
	AddArrowMenuLabel(pb.B3, name.."_Paging_Arr2",	{arr2_x,0},	-90,	MenuLabelBase.name)
end
--------------------------------------------------------------------------------------------------------
function AddNextWaypointStatusWindow( pos, controllers, parent_name )
	local smallfont_size	= 28
	local tp36				= createTextProperty( 36,  "GREEN",	IND_MPD_MATERIAL_GREEN, "RightBottom" )
	local tp28				= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftBottom" )
	local lbl_pos			= pos or {pb_props[pb.B2].pos[1]-27,pb_props[pb.L6].pos[2]-14}
	local lbl_W, lbl_H		= tp28.width*14.30, tp28.height*3.10

	local str1_x1	= -lbl_W/2 + tp28.width*0.6
	local str1_x2	= lbl_W/2 - tp28.width*2.7
	local str1_x3	= str1_x2 - tp28.width*0.4
	
	local str2_x1	= -lbl_W/2  + tp28.width*4.4
	local str2_x2	= lbl_W/2 - tp28.width*0.6
	
	local str1_y	= tp28.height*0.15
	local str2_y	= -tp28.height*1.3
	
	tp28.stringdefs = {smallfont_size*GetScale(),smallfont_size*GetScale(),smallfont_size*GetScale()*0.05}	-- to set symbols looser
	
	local WPWBase = addPlaceholder("WPWBase_PH", lbl_pos, parent_name, controllers)
	
	--						name,						pos,	width,	height
	AddRoundCornersWindow("NextWaypointStatusWindow",	{0,0},	lbl_W,	lbl_H,
						{	-- value
							{"W02",		{str1_x1,					str1_y},	tp28,	{{"NAV_WPStatus_Name"}}},
							{"61",		{str2_x1,					str2_y},	tp36,	{{"NAV_WPStatus_Speed_Show"},{"NAV_WPStatus_Speed"}}},
							{"6.9",		{str1_x3,					str1_y},	tp36,	{{"NAV_WPStatus_Dest_Show"},{"NAV_WPStatus_Distance"}}},
							{"KM",		{str1_x2,					str1_y},	tp28,	{{"NAV_WPStatus_Dest_Show"},{"NAV_WPStatus_Units"}}, {"KM","NM"}},
							{"3:43",	{str2_x2 - tp28.width*3,	str2_y},	tp36,	{{"NAV_WPStatus_Dest_Show"},{"NAV_WPStatus_Speed_Show"},{"NAV_WPStatus_Time", 0}}},
							{"3:43",	{str2_x2,					str2_y},	tp36,	{{"NAV_WPStatus_Dest_Show"},{"NAV_WPStatus_Speed_Show"},{"NAV_WPStatus_Time", 1}}}
						},
					--	tp		material,				parent,			controllers
						tp28,	IND_MPD_MATERIAL_GREEN,	WPWBase.name,	{{"NAV_WPStatus_Window"}})
						
	-- TODO: Manual p.150: "The distance to go digital readout is only presented when the destination point OR ground speed are presented."
	-- So it seems that if there are no WP but ground speed, distance-to-go should be the max distance with present endurance and speed.
end

--------------------------------------------------------------------------------------------------------
function addMaskArea(level,  h_clip_relation, name, vertices, indices, pos, parent, controllers, material )
	local mask				= CreateElement "ceMeshPoly"
	setSymbolCommonProperties(mask, name, pos, parent, controllers, material)
	mask.vertices			= vertices
	mask.indices			= indices
	mask.primitivetype		= "triangles"
	setAsInvisibleMask(mask) -- changes material
	mask.additive_alpha		= false
	mask.change_opacity		= false

	if h_clip_relation ~= nil then
		mask.h_clip_relation	= h_clip_relation
		mask.level				= level
	end 

	Add(mask)
	return mask
end
---------------------------------------------------------------------------------------------
local function buildArcVerts(pos, radius, angle_start_deg, angle_end_deg, segments_count)
	local count_default		= 24	-- for the whole circle
	local d_angle_default	= 360 / count_default
	local arc				= angle_end_deg - angle_start_deg
	local count				= segments_count or (math.floor(arc / d_angle_default) + 1)	-- segments count
	local v_count			= count + 1	-- verts count

	local verts = {}

	local angle = math.rad(angle_start_deg)
	local delta = math.rad(arc / count)

	local x0 = pos[1]
	local y0 = pos[2]

	for i = 0,count do
		local x = x0 + radius * math.cos(angle)
		local y = y0 + radius * math.sin(angle)
		verts[#verts + 1] = {x, y}
		angle = angle + delta
	end

	return verts
end

function splitVerts(tbl, verts)
	for i,v in pairs(verts) do
		if i ~= 1 then
			tbl[#tbl + 1] = v
		end
	end
end
--------------------------------------------------------------------------------------
function buildRoundedBoxVerts( size, radius )
	local segments_count	= 5
	local r					= radius or RoundingRadius
	local d					= r * 2
	local w					= size[1]
	local h					= size[2]
	local w05				= w / 2
	local h05				= h / 2

	local ww05 = w05 - r
	local hh05 = h05 - r

	local verts_lt = buildArcVerts({-ww05, hh05}, r,  90, 180, segments_count)
	local verts_lb = buildArcVerts({-ww05,-hh05}, r, 180, 270, segments_count)
	local verts_rt = buildArcVerts({ ww05, hh05}, r,   0,  90, segments_count)
	local verts_rb = buildArcVerts({ ww05,-hh05}, r, 270, 360, segments_count)
	local verts_l  = {{ -w05, hh05}, { -w05,-hh05}}
	local verts_r  = {{  w05,-hh05}, {  w05, hh05}}
	local verts_tl = {{    0,  h05}, {-ww05,  h05}}
	local verts_tr = {{ ww05,  h05}, {    0,  h05}}
	local verts_b  = {{-ww05, -h05}, { ww05, -h05}}

	local verts = verts_tl
	splitVerts(verts, verts_lt)
	splitVerts(verts, verts_l)
	splitVerts(verts, verts_lb)
	splitVerts(verts, verts_b)
	splitVerts(verts, verts_rb)
	splitVerts(verts, verts_r)
	splitVerts(verts, verts_rt)
	splitVerts(verts, verts_tr)
	return verts
end
-----------------------------------------------------------------------------
function addRoundedBox(name, pos, align, size, radius, line_width, parent, controllers, material, line_controllers, level, h_clip_relation)
	local w					= size[1]
	local h					= size[2]
	local w05				= w / 2
	local h05				= h / 2

	local x = pos[1]
	local y = pos[2]

	local c = {}
	if align == "CenterCenter" then
		c = {0,0}
	elseif align == "LeftCenter" then
		c = {w05,0}
	elseif align == "RightCenter" then
		c = {-w05,0}
	elseif align == "CenterBottom" then
		c = {0,h05}
	elseif align == "CenterTop" then
		c = {0,-h05}
	elseif align == "LeftBottom" then
		c = {w05,h05}
	elseif align == "LeftTop" then
		c = {w05,-h05}
	elseif align == "RightTop" then
		c = {-w05,-h05}
	elseif align == "RightBottom" then
		c = {-w05,h05}
	end

	local cx = c[1]
	local cy = c[2]

	local PH = addPlaceholder(name.."_PH", {x + cx, y + cy}, parent, controllers, level, h_clip_relation)
	local verts = buildRoundedBoxVerts( size, radius )
	
	draw_line( verts,  IND_MPD_MATERIAL_BLACK,				PH.name, line_width, name.."_black", line_controllers, h_clip_relation, level )
	draw_line( verts,  material or IND_MPD_MATERIAL_GREEN,	PH.name, line_width, name, line_controllers, h_clip_relation, level )
end
-- -----------------------------------------------------------------------------------------------
box_count = 0
function getBoxIndex()
	box_count = box_count + 1
	return "Box_" .. box_count 
end
function draw_box( l_up, r_dn,  material, parent, controllers, level, h_clip_relation )
	local name = getBoxIndex()
	addRoundedBox(name, l_up, "LeftTop", { math.abs(r_dn[1]-l_up[1]), math.abs(r_dn[2]-l_up[2])}, RoundingRadius, info_box_line_width, parent, controllers, nil, nil, level, h_clip_relation )
end

-----------------------------------------------------------
-- builds "ceTexPoly" element and fills all data
-- "size" and "pos" in display units
-- "tex_size" size of square texture file in pixels
-- "tex_pos" position of symbol on texture
-- "tex_scale" = symbol size on indicator / symbol size on texture
function buildSymbol(name, material, size, pos, align, tex_size, tex_pos, tex_scale, parent, controllers)
	local texturedMesh			= CreateElement "ceTexPoly"

	setSymbolCommonProperties(texturedMesh, name, pos, parent, controllers, material)
	texturedMesh.indices		= default_box_indices

	local width		= size[1]
	local height	= size[2]
	texturedMesh.vertices		= buildBoxVerts(width, height, align)

	local tex_x = tex_pos[1]
	local tex_y = tex_pos[2]
	local scale = tex_scale / tex_size
	texturedMesh.tex_params		= {tex_x/tex_size, tex_y/tex_size, scale, scale}

	texturedMesh.additive_alpha		= true

	texturedMesh.h_clip_relation	= h_clip_relations.COMPARE
	texturedMesh.level				= DEFAULT_LEVEL

	return texturedMesh
end
-----------------------------------------------------------
function addVideoSignal()
	local MFD_ID = readParameter("MFD_NUM");
	-- Add base
	local	verts = buildBoxVerts(video_area_w, video_area_w*3.0/4.0, "CenterCenter")

	local	render_back						= CreateElement "ceMeshPoly"
			render_back.name				= "VideoSignalBack"
			render_back.primitivetype		= "triangles"
			render_back.vertices			= verts
			render_back.indices				= default_box_indices
			render_back.isvisible			= false
			render_back.material			= "MASK_MATERIAL"
			render_back.level				= DEFAULT_LEVEL
			render_back.h_clip_relation		= h_clip_relations.REWRITE_LEVEL
			render_back.init_pos			= {0, 0, 0}

	Add(render_back)

	local	PNVS_underlay					= CreateElement "ceTexPoly"
			PNVS_underlay.name				= "PNVS_underlay"
			PNVS_underlay.vertices			= verts
			PNVS_underlay.indices			= default_box_indices
			PNVS_underlay.tex_coords		=  {{0.1, 1-0.195},{0.1, 0.205},{1-0.1, 0.205},{1-0.1, 1-0.195}}
			PNVS_underlay.material			= MakeMaterial("PNVS_CAM_AH64"..getMFD_suffix(MFD_ID), {255,255,255,255})
			PNVS_underlay.level				= DEFAULT_LEVEL
			PNVS_underlay.h_clip_relation	= h_clip_relations.COMPARE
			PNVS_underlay.parent_element	= render_back.name
			PNVS_underlay.controllers		= {{"MFD_underlay_PNVS"}, {"VideoSignal_Zoom"}, {"VideoSignal_Color"}}
	Add(PNVS_underlay)

	local	TADS_underlay					= CreateElement "ceTexPoly"
			TADS_underlay.name				= "TADS_underlay"
			TADS_underlay.vertices			= verts
			TADS_underlay.indices			= default_box_indices
			TADS_underlay.tex_coords		= {{0.1, 1-0.2},{0.1, 0.2},{1-0.1, 0.2},{1-0.1, 1-0.2}}
			TADS_underlay.material			= MakeMaterial("TADS_CAM_AH64"..getMFD_suffix(MFD_ID), {255,255,255,255})
			TADS_underlay.level				= DEFAULT_LEVEL
			TADS_underlay.h_clip_relation	= h_clip_relations.COMPARE
			TADS_underlay.parent_element	= render_back.name
			TADS_underlay.controllers		= {{"MFD_underlay_TADS"}, {"VideoSignal_Zoom"}, {"VideoSignal_Color"}}
	Add(TADS_underlay)

	local	MAP_underlay					= CreateElement "ceTexPoly"
			MAP_underlay.name				= "MAP_underlay"
			MAP_underlay.vertices			= buildBoxVerts(display_size_pix, display_size_pix, "CenterCenter")
			MAP_underlay.indices			= default_box_indices
			MAP_underlay.tex_coords			= {{0, 1},{0, 0},{1, 0},{1, 1}}
			---------------------------------------------------------------------------------------
			
			local MAP_material_name = "";

			if MFD_ID == MFD_SELF_IDS.PLT_LMFD then
				MAP_material_name = "MAP_AH64_PLT_L";
			elseif MFD_ID == MFD_SELF_IDS.PLT_RMFD then
				MAP_material_name = "MAP_AH64_PLT_R";
			elseif MFD_ID == MFD_SELF_IDS.CPG_LMFD then
				MAP_material_name = "MAP_AH64_CPG_L";
			elseif MFD_ID == MFD_SELF_IDS.CPG_RMFD then
				MAP_material_name = "MAP_AH64_CPG_R";
			end

			MAP_underlay.material			= MakeMaterial(MAP_material_name,{255,255,255,255})
			---------------------------------------------------------------------------------------
			MAP_underlay.level				= DEFAULT_LEVEL
			MAP_underlay.h_clip_relation	= h_clip_relations.COMPARE
			MAP_underlay.parent_element		= render_back.name
			MAP_underlay.controllers		= {{"MFD_underlay_MAP"}}
	Add(MAP_underlay)

	local RMAP_Pos = addPlaceholder("RMAP_Pos", {0, -375}, render_back.name, {{"MFD_underlay_RMAP"}})
	local MFD_ID = readParameter("MFD_NUM");

    local function addRmapVideo(namePostfix, width, zoneControllerIndex)
        local	RMAP_Video			= CreateElement "ceTexPoly"
        RMAP_Video.name				= "RMAP_Video_"..namePostfix
        RMAP_Video.vertices			= buildBoxVerts(width, 700, "CenterBottom")
        RMAP_Video.indices			= default_box_indices
        RMAP_Video.tex_coords		=  {{0, 1},{0, 0},{1, 0},{1, 1}}
        RMAP_Video.material			= MakeMaterial("RMAP_CAM_AH64"..getMFD_suffix(MFD_ID), {255,255,255,255})
        RMAP_Video.level			= DEFAULT_LEVEL
        RMAP_Video.h_clip_relation	= h_clip_relations.COMPARE
        RMAP_Video.parent_element	= RMAP_Pos.name
        RMAP_Video.controllers		= {{"DSPLS_FCR_RMAP_Scan_Zone", zoneControllerIndex}, {"VideoSignal_Color"}}
        RMAP_Video.additive_alpha	= true
        Add(RMAP_Video)
    end

    addRmapVideo("Wide", 750, 0)
    addRmapVideo("Medium", 600, 1)
    addRmapVideo("Narrow", 550, 2)
    addRmapVideo("Zoom", 200, 3)
end

function addRectangleTranslucentBackground( pos, height, width, align, parent, level, h_clip_relation )
	local	verts		= buildBoxVerts(width, height, align)
	return	addTranslucentBackground( pos, verts, default_box_indices, parent, level, h_clip_relation )	
end 

function addTranslucentBackground( verts, indices, parent, level, h_clip_relation, is_transparent )
	local fon_mat = InformationBackgroundMaterial

	if is_transparent==true then
		fon_mat = "MFD_TRANSLUCENT"			-- TODO
	end

	local	elem						= CreateElement "ceMeshPoly"
			elem.primitivetype			= "triangles"
			elem.vertices				= verts
			elem.indices				= indices
			elem.isvisible				= true
			elem.material				= fon_mat
			elem.init_pos				= pos
			
			if parent ~= nil then
				elem.parent_element		= parent
			end
			
			if h_clip_relation ~= nil then
				elem.h_clip_relation	= h_clip_relation
				elem.level				= level
			else 
				elem.h_clip_relation	= h_clip_relations.COMPARE	
				elem.level				= DEFAULT_LEVEL
			end

	Add(elem)
	return elem
end
--------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------
function buildEllipseVerts(pos, r_x, r_y, N)
	local n = N or 72
	local delta =  2*math.pi/n
	local verts = {}
	
--	verts[#verts + 1] = pos
	for i = 0, n do
		local angle = i * delta
		verts[#verts + 1] =  { pos[1] + r_x * math.cos( angle ), pos[2] + r_y * math.sin( angle ) }
	end
	return verts
end
-------------------------------------------------------------------------------------------
function buildEllipseIndices(N)
	local n = N or 72
	local indices = {0, n-1, 1}

	for i = 1, n-2 do
		indices[#indices + 1] = 0
		indices[#indices + 1] = i
		indices[#indices + 1] = i+1	
	end
	return indices
end
--------------------------------------------------------------------------------------------------------
local function buildLineBorder(name, verts, width, parent, controllers, material)
	local width05 = width / 2
	local tex_line_width = 10
	local tex_line_width05 = tex_line_width / 2

	local elem				= CreateElement "ceSimpleLineObject"
--	local elem				= CreateElement "ceSMultiLine"
	elem.name				= name
	elem.material			= material
	elem.width				= width05
	elem.vertices			= verts
	elem.tex_params			= {{0.0,280.0*texture_scale},{1.0,280.0*texture_scale}, {1.0/display_size_pix, 2 * (tex_line_width / width)/display_size_pix}}

	if parent ~= nil then
		elem.parent_element = parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end

--	setClipLevel(elem)

	return elem
end
--------------------------------------------------------------------------------------------------------
local function buildLine(name, verts, width, parent, controllers, material)
	local width05 = width / 2
	local tex_line_width = 10
	local tex_line_width05 = tex_line_width / 2

	local elem				= CreateElement "ceSimpleLineObject"
--	local elem				= CreateElement "ceSMultiLine"
	elem.name				= name
	elem.material			= material
	elem.width				= width05
	elem.vertices			= verts
	elem.tex_params			= {{0.0,308.0*texture_scale},{1.0,308.0*texture_scale}, {1.0/display_size_pix, 2 * (tex_line_width / width)/display_size_pix}}

	if parent ~= nil then
		elem.parent_element = parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end

--	setClipLevel(elem)

	return elem
end
function addSimpleLine(name, verts, width, material, parent, controllers)
	local line = buildLine(name, verts, width, parent, controllers, material)
	addElem(line)
	return line
end
function addSimpleLineBorder(name, verts, width, material, parent, controllers)
	local line = buildLineBorder(name, verts, width, parent, controllers, material)
	addElem(line)
	return line
end
function addLine(name, verts, width, parent, controllers, material)
local mat = material or IND_MPD_MATERIAL_GREEN
	local back_line = addSimpleLine(name.."_back", verts, width + 2, IND_MPD_MATERIAL_BLACK, parent, controllers)
	local line = addSimpleLine(name, verts, width, mat , parent, controllers)
	return {back_line, line}
end
function addLineBorder(name, verts, width, parent, controllers, material)
local mat = material or IND_MPD_MATERIAL_GREEN
	local line = addSimpleLineBorder(name, verts, width, mat , parent, controllers)
	return line
end
-- TADS LOS Reticle defs
local TADS_LOS_Reticle_w05 = 142
local TADS_LOS_Reticle_h05 = 107

function add_TADS_LOS_Reticle(name, pos, parent, controllers)
	local w05 = TADS_LOS_Reticle_w05
	local h05 = TADS_LOS_Reticle_h05
	local s_w05 = 20	-- space
	local s_h05 = 14	-- space

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)
--( {{80,-90},{80,-210}}, IND_MPD_MATERIAL_GREEN, nil, 3 )
	
	local lu = addLineBorder(name.."_up",		{{     0, s_h05}, {   0, h05}},	10, PH.name, nil)
	local ld = addLineBorder(name.."_down",	{{     0,-s_h05}, {   0,-h05}},	10, PH.name, nil)
	local ll = addLineBorder(name.."_left",	{{-s_w05,     0}, {-w05,   0}},	10, PH.name, nil)
	local lr = addLineBorder(name.."_right",	{{ s_w05,     0}, { w05,   0}},	10, PH.name, nil)
	return {
		PH,
		lu,
		ld,
		ll,
		lr,
	}
end
-----------------------------------------------------------
function AddCurrentHeadingLabel()
	local tprops		= createTextProperty( 36,  "GREEN",	IND_MPD_MATERIAL_GREEN, "RightCenter" )
	local lbl_W, lbl_H	= tprops.width*3+RoundingRadius, tprops.height*1.4
	local lbl_pos		= {0,pb_props[pb.T3].pos[2]}

	AddRoundCornersWindow("HeadingUpLbl",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"360",		{lbl_W/2*0.75,0},	tprops,	{{"TSD_HeadingStatusWindow"}}}
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	InfoWindowsBase.name,	{{"NAV_Heading_Valid"}}, "RightCenter")
end
---------------------------------------------------------
function AddNextWaypointHeadingLabel()
	local tprops			= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "RightCenter" )
	local lbl_W, lbl_H		= tprops.width*3+RoundingRadius, tprops.height*1.6
	local lbl_pos 			= {0,pb_props[pb.B3].pos[2] + tprops.height*0.95}

	AddRoundCornersWindow("NextWPHeadingDnLbl",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"360",		{lbl_W/2*0.75,0},	tprops,	{{"TSD_NextWPHeadingStatusWindow"}}}
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	InfoWindowsBase.name,	{{"NAV_Heading_Valid"}}, "RightCenter")
end
-------------------------------------------------------------------------------------------
function addBorderBase( btn, width, up_margin, dn_margin, controllers, dh_, dstart_, matherial )
	-- works with R and L side only
	local pos_up	= pb_props[btn].pos_up	
	local pos_dn	= pb_props[btn].pos_down
	
	local pos_left	= pb_props[btn].pos_cl
	local pos_right	= pb_props[btn].pos_c	
	
	local w			= 0
	local st		= 0
	local dh		=  dh_ or 0
	local dstart	=  dstart_ or 0	
	
	local up_1 = {}
	local up_2 = {}
	local dn_3 = {}
	local dn_4 = {}
	
	if	btn >= pb.L6 then
		w			= width
		st			= -10
		up_1 = {pos_up[1] + st + dstart, pos_up[2] + up_margin + dh}
		up_2 = {up_1[1] + w, 	up_1[2]}
		dn_3 = {up_2[1], 		pos_dn[2] - dn_margin}
		dn_4 = {up_1[1], 		dn_3[2]}
		
	elseif	btn >= pb.B6 then
		w			= width
		st			= 5
		up_1 = {pos_left[1] + dh + st,  pos_left[2]  + w}
		up_2 = {pos_right[1] - dh + st, pos_right[2] + w}
		dn_3 = {up_2[1], pos_right[2]}
		dn_4 = {up_1[1], pos_left[2]}
		
	elseif	btn >= pb.R1 then 
		w			= 0 - width
		st			= 0
		up_1 = {pos_up[1] + st - dstart, pos_up[2] + up_margin + dh }
		up_2 = {up_1[1] + w, 	up_1[2]}
		dn_3 = {up_2[1], 		pos_dn[2] - dn_margin}
		dn_4 = {up_1[1], 		dn_3[2]}
		
	elseif	btn >= pb.T1 then
		w			= 0 - width
		st			= -5
		up_1 = {pos_left[1] + dh,  pos_left[2]  + w}
		up_2 = {pos_right[1] - dh, pos_right[2] + w}
		dn_3 = {up_2[1], pos_right[2]}
		dn_4 = {up_1[1],  pos_left[2]}
	end
	
	addLineBorder("border"..btn, { up_1, up_2, dn_3, dn_4, up_1 }, 10, nil, controllers, matherial)
end
-------------------------------------------------------------------------------------------------------
function addBorder( btn, width, controllers, dh_, matherial )
	-- works with R and L side only
	addBorderBase( btn, width, 13, 32, controllers, dh_, 0, matherial )
end

function addBorderSmall( btn, width, controllers, dh_, matherial)
	-- works with R and L side only
	addBorderBase( btn, width, -10, 0, controllers, 0, -10, matherial )
end



function addArc(name, pos, radius, angle_start_deg, angle_end_deg, segments_count, line_width, parent, controllers,material)
	local verts = buildArcVerts(pos, radius, angle_start_deg, angle_end_deg, segments_count)
	local back_arc = addSimpleLine(name.."_back", verts, line_width + 2, IND_MPD_MATERIAL_BLACK, parent, controllers)
	local arc = addSimpleLine(name, verts, line_width, material, parent, controllers)
	return {back_arc, arc}
end

function AddSendMessageWindow(name, parent, ctrl_visibility)
	tprops					= createTextProperty( 28,  "WHITE",	IND_MPD_MATERIAL_WHITE, "LeftCenter" )
	local lbl_W, lbl_H		= tp_default.width*4.20, tp_default.height*3.1
	local lbl_pos			= {pb_props[pb.R6].pos[1]-tp_default.height*1.20, pb_props[pb.R6].pos[2]-tp_default.height*3.00}
	
	--					  name,	pos,		width,	height
	AddRoundCornersWindow(name,	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"FM1",	{-tp_default.width*1.4,tp_default.width*1.00},	tprops,		{{"DSPLS_COM_SendBtnRadio"}}, {"VHF", "UHF", "FM1", "FM2", "HF"}},
							{"L1",	{-tp_default.width*1.0,-tp_default.width*0.90},	tprops,		{{"DSPLS_SendBtnNet"}}}	
						},
					--	tp,			material,				parent,	controllers
						tp_default,	IND_MPD_MATERIAL_WHITE,	parent,	ctrl_visibility)
end

function AddSendBtn(name, parent, ctrl_visibility)
	local ctrl_window = {{"DSPLS_PB_Visibility", pb.R6}}
	
	if ctrl_visibility ~= nil then
		ctrl_window[#ctrl_window + 1] = ctrl_visibility[1]
	end
	
	local Controls =
	{
		{ pb.R6, "SEND",	tp_28_white, 		{{"DSPLS_COM_SendBtn"}}},
		{ pb.R6, "SEND",	tp_28_white_inv,	{{"DSPLS_COM_SendBtn_inv"}}},
	}
	local ControlsBase = addPlaceholder("ControlsBase_PH", {0, 0}, nil, ctrl_visibility)
	
	createControls( Controls, nil, ControlsBase.name )
	AddSendMessageWindow(name, parent, ctrl_window)
end

function AddDL_StatusWnd()
local pos_label	= {pb_props[pb.L1].pos[1]+tp_default.width*13.0*1.1/2.0, pb_props[pb.L1].pos[2] + PB_dist_y/2.0} 	
local wm = tp_default.width*13.0*1.1	
local hm = tp_default.height*1.7
AddRoundCornersWindow("DL_InStandByMsg", pos_label, wm, hm, {{"DL IN STANDBY", { 0, 0}, tp_28_white_center_center, nil }}, tp_default, IND_MPD_MATERIAL_WHITE, nil, {{"MPD_COM_ModemStatusMsg"}} )

pos_label	= {pb_props[pb.L1].pos[1]+tp_default.width*5.5, pb_props[pb.L1].pos[2] + PB_dist_y/2.0} 	
wm = tp_default.width*10.0*1.1	
AddRoundCornersWindow("DL_InhibitMsg", pos_label, wm, hm, {{"DL INHIBIT", { 0, 0}, tp_28_white_center_center, nil }}, tp_default, IND_MPD_MATERIAL_WHITE, nil, {{"MPD_COM_DLInhibitMsg"}} )
end

scale_coeff				= display_size_pix/720
local font_char_size_w		= 17 * scale_coeff
local font_char_space		= 0 * scale_coeff


function buildBorderedSymbolBrown(name, size, align, tex_pos, pos, parent, controllers)
	local elem = buildSymbol(name, IND_MPD_VIDEO_SYMBOLOGY, size, pos, align, tex_pos, 2/scale_coeff, parent, controllers)
	
	local back_elem = Copy(elem)
	back_elem.material = IND_MPD_VIDEO_SYMBOLOGY

	addElem(back_elem)
	addElem(elem)
	return {back_elem, elem}
end

local function buildBorderedSymbol(name, size, align, tex_pos, pos, parent, controllers)
	local elem = buildSymbol(name, IND_MPD_VIDEO_SYMBOLOGY, size, pos, align, 512, tex_pos, 2/1.4222, parent, controllers)
	
	local back_elem = Copy(elem)
	back_elem.material = IND_MPD_VIDEO_SYMBOLOGY_BLACK

	addElem(back_elem)
	addElem(elem)
	return {back_elem, elem}
end
--


function addPointer_FcrCenterlineBearing(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {19*scale_coeff,14*scale_coeff}, "CenterTop", {140,159}, pos, parent, controllers)
	return elems
end

function addPointer_AlternateSensorBearing(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {17,16}, "CenterTop", {177,159}, pos, parent, controllers)
	return elems
end

function addPointer_AdfBearing(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {12,24}, "CenterTop", {70,162}, pos, parent, controllers)
	return elems
end

function addPointer_BobUpHeading(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {22,22}, "CenterTop", {25,162}, pos, parent, controllers)
	return elems
end

function addDot(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {12,12}, "CenterCenter", {209,175}, pos, parent, controllers)
	return elems
end

function addSkidSlipBall(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {50, 50}, "CenterCenter", {53,282}, pos, parent, controllers)
	return elems
end

function addSkidSlipBallBlue(name, pos, parent, controllers)
	local elems = buildBorderedSymbolBlue(name, {50, 50}, "CenterCenter", {53,282}, pos, parent, controllers)
	return elems
end

function addSkidSlipBallBrown(name, pos, parent, controllers)
	local elems = buildBorderedSymbolBrown(name, {50, 50}, "CenterCenter", {53,282}, pos, parent, controllers)
	return elems
end


function addPoint(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {8*scale_coeff,8*scale_coeff}, "CenterCenter", {231,175}, pos, parent, controllers)
	return elems
end

function addFcrCurrentCenterline(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {6*scale_coeff,64*scale_coeff}, "CenterCenter", {5,275}, pos, parent, controllers)
	return elems
end

function addFcrLastCenterline(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {6*scale_coeff,64*scale_coeff}, "CenterCenter", {21,275}, pos, parent, controllers)
	return elems
end

function addAccelerationCue(name, pos, parent, controllers)
	local elems = addArc(name, pos, 8*scale_coeff, 0, 360, 12, 4, parent, controllers)
	return elems
end

local function getCenter(width, height, align)
	local w05 = width / 2
	local h05 = height / 2

	local c = {}

	if align == "CenterCenter" then
		c = {0,0}
	elseif align == "LeftCenter" then
		c = {w05,0}
	elseif align == "RightCenter" then
		c = {-w05,0}
	elseif align == "CenterBottom" then
		c = {0,h05}
	elseif align == "CenterTop" then
		c = {0,-h05}
	elseif align == "LeftBottom" then
		c = {w05,h05}
	elseif align == "LeftTop" then
		c = {w05,-h05}
	elseif align == "RightTop" then
		c = {-w05,-h05}
	elseif align == "RightBottom" then
		c = {-w05,h05}
	end

	return c
end

function addRect(name, width, height, pos, align, line_width, parent, controllers)
	local w05 = width / 2
	local h05 = height / 2

	local x = pos[1]
	local y = pos[2]

	local c = getCenter(width, height, align)

	local l = x + c[1] - w05
	local r = x + c[1] + w05
	local t = y + c[2] + h05
	local b = y + c[2] - h05

	local rect = addLine(name,
	{
		{l, t},
		{r, t},
		{r, b},
		{l, b},
		{l, t},
	},
	line_width, parent, controllers)

	return rect
end

function addMapTacticalSymbol(name, font, controllers, parent, pos, size, value)
	local font_size		= size or 16

	local elem			= CreateElement "ceStringPoly"
	elem.material		= font
	elem.alignment		= "CenterCenter"
	elem.stringdefs		= {font_size * GetScale(),font_size * GetScale()}
	elem.value			= value or ""

	setSymbolCommonProperties(elem, name, pos, parent, controllers, font)

	Add(elem)
end



--local inch_to_meter		= 0.0254
local pixel_size		= pix_to_inch * InToMeter

-- fonts properties:
-- chars 30 x 19 pix
local font_char_tex_size	= 64
local font_char_tex_h		= 50
local font_char_tex_w		= 34
local font_char_size_h		= 25 * scale_coeff
local font_char_size_w		= 17 * scale_coeff
local font_char_space		= 0  * scale_coeff
local font_line_space		= 0  * scale_coeff
local font_char_scale_h		= 1 / (font_char_tex_h/font_char_tex_size)
local font_char_scale_w		= 1 / (font_char_tex_w/font_char_tex_size)
local t_font_char_h			= pixel_size * font_char_size_h * font_char_scale_h
local t_font_char_w			= pixel_size * font_char_size_w * font_char_scale_w
local t_font_char_space		= pixel_size * font_char_space
local t_font_line_space		= pixel_size * font_line_space

local t_font_char_dx		= pixel_size * 0
local t_font_char_dy		= font_char_size_h * font_char_scale_h * ((font_char_tex_size - font_char_tex_h)/font_char_tex_size)

local font_stringdefs = { t_font_char_h, t_font_char_w, t_font_char_space, t_font_line_space - pixel_size * t_font_char_dy }

local function setCommonStringProperties(elem, name, text, align, parent, controllers, formats)
	elem.name			= name
	elem.value			= text
	elem.alignment		= align
	elem.use_mipfilter	= true

	if parent ~= nil then
		elem.parent_element	= parent
	end

	if controllers ~= nil then
		elem.controllers	= controllers
	end

	if formats ~= nil then
		elem.formats		= formats
	end

	setClipLevel(elem)
end


function addTextCom(name, text, pos, align, parent, controllers, formats, material, is_transparent)

	local dy = t_font_char_dy
	if align == "LeftBottom" or align == "CenterBottom" or align == "RightBottom" then
		dy = 0
	end

	local elem			= CreateElement "ceStringPoly"
	setCommonStringProperties(elem, name, text, align, parent, controllers, formats)
	
	elem.init_pos		= {pos[1] + t_font_char_dx, pos[2] + dy, 0}
	elem.stringdefs		= font_stringdefs

	local fon_mat = InformationBackgroundMaterial

	if is_transparent == true then
		fon_mat = "MFD_TRANSPARENT"
	end

	elem.BackgroundMaterial	= fon_mat

	if text == " " then
		elem.UseBackground		=  false
	else 
		elem.UseBackground		=  true
	end
	
	if material ~= nil then
		elem.material 	= fontPrefix..material
	else
		elem.material	= fontPrefix.."VIDEO"
	end

	local back_elem = Copy(elem)
	back_elem.name		= name.."_back"
	back_elem.material	= fontPrefix.."VIDEO_BLACK"

	addElem(back_elem)
	addElem(elem)
	return {back_elem, elem}
end


local function drawShotAtSymbol(pos, parent, controllerPH, height, width, material)
	pos = pos or {0, 0}
	height = height or 36
	width = width or 25
	material = material or IND_MPD_MATERIAL_GREEN

	local angle = math.floor(math.deg(math.atan(height / width)))
	local hypotenuse = math.floor(math.sqrt(height ^ 2 + width ^ 2))
	local line_width = 4

	local SymbolPH = addPlaceholder(nil, pos, parent, controllerPH)

	addFatLine(nil, 	hypotenuse, line_width, {-width / 2, -height / 2}, 	-90 + angle, SymbolPH.name, nil, "MFD_BACKGROUND")
	addFatLine(nil, 	hypotenuse, line_width, {-width / 2, -height / 2}, 	-90 + angle, SymbolPH.name, nil, material)
	addFatLine(nil, 	hypotenuse, line_width, {-width / 2, height / 2}, 	-90 - angle, SymbolPH.name, nil, "MFD_BACKGROUND")
	addFatLine(nil, 	hypotenuse, line_width, {-width / 2, height / 2}, 	-90 - angle, SymbolPH.name, nil, material)
end

function addcScopeSymbols(parent)
	local symbolSize = 50
	local coeff = 0.92
	local coeff_w = 0.75 * coeff
	local coeff_n = coeff
	local coeff_z = 1.5 * coeff
	
	for i = 0, 15 do
        drawShotAtSymbol(nil, parent, {{"FCR_VID_cScopeShootAtBehind", c_scope_w, c_scope_h, coeff_w, coeff_n, coeff_z, i}})
    end

	for i = 0, 15 do
        addMapTacticalSymbol(nil, fontPrefix.."FCR_Target_Symbol", {{"FCR_VID_cScopeSymbol", c_scope_w, c_scope_h, coeff_w, coeff_n, coeff_z, i }}, parent, {0, 0}, symbolSize)
    end

    for i = 0, 15 do
        drawShotAtSymbol(nil, parent, {{"FCR_VID_cScopeShootAtAbove", c_scope_w, c_scope_h, coeff_w, coeff_n, coeff_z, i }})
    end

	local scaleCoefficient = 0.7
    local multiplierCoefficient = 1.35
	
	local NtsFrameSize 			= {64 * multiplierCoefficient, 68 * multiplierCoefficient}
	local AntsFrameSize 		= {61 * multiplierCoefficient, 76 * multiplierCoefficient}
	
	FCR_NTS_TYPES =
	{
		NONE 						= 0,
		SOLID 						= 1,
		DASHED 						= 2
	}

	local NtsControllerSolid 	= {{"FCR_VID_cScopeNts", c_scope_w, c_scope_h,	coeff_w, coeff_n, coeff_z, FCR_NTS_TYPES.SOLID}}
	local NtsControllerDashed 	= {{"FCR_VID_cScopeNts", c_scope_w, c_scope_h,	coeff_w, coeff_n, coeff_z, FCR_NTS_TYPES.DASHED}}
	local AntsControllers 		= {{"FCR_VID_cScopeAnts",c_scope_w, c_scope_h,	coeff_w, coeff_n, coeff_z}}

	-- ANTS
    Add(buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_YELLOW, AntsFrameSize, {0, 0}, "CenterCenter", 512, {160, 20}, scaleCoefficient, parent, AntsControllers))
	-- NTS solid
    Add(buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_YELLOW, NtsFrameSize, {0, 0}, "CenterCenter", 512, {96, 29}, scaleCoefficient, parent, NtsControllerSolid))
	-- NTS dashed
    Add(buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_YELLOW, NtsFrameSize, {0, 0}, "CenterCenter", 512, {226, 29}, scaleCoefficient, parent, NtsControllerDashed))
end