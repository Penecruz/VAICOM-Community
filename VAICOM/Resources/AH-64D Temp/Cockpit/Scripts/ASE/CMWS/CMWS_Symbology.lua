dofile(LockOn_Options.script_path.."Displays/Common/Units.lua")
dofile(LockOn_Options.script_path.."symbology_defs.lua")

display_size_inch		= 2.0
display_size_pix		= 180
display_size_pix_05		= display_size_pix / 2
local pix_to_inch		= display_size_inch / display_size_pix
local inch_to_meter		= 0.0254
local pixel_size		= pix_to_inch * inch_to_meter

local tex_size			= 512
local texture_scale			= 1/tex_size
SetCustomScale(pix_to_inch * inch_to_meter)
local CMWS_Scale = 1/GetScale()

additive_alpha	= true
CMWS_LEVEL	= DEFAULT_LEVEL

function setElemsClipLevel(tbl, level)
	for i, obj in pairs(tbl) do
		setClipLevel(obj, level)
	end
end

local function addElem(elem)
	elem.additive_alpha	= additive_alpha
	Add(elem)
end




local glyphHeight_18				= 26
local glyphWidth_9					= 15
local fontScaleY_18				= glyphHeight_18 * GetScale()
local fontScaleX_9				= glyphWidth_9 * GetScale()
local fontScaleSpaceX			= 5 * GetScale()
local font_stringdefs = { fontScaleY_18, fontScaleX_9, fontScaleSpaceX, 0 }

function AddText(Name, Xpos, Ypos, Controllers, Value)
	local symb			= CreateElement "ceStringPoly"
	symb.name			= Name
	symb.material		= "font_CMWS"
	symb.stringdefs		= font_stringdefs
	symb.alignment		= "LeftBottom"
	symb.init_pos		= {Xpos, Ypos, 0.0}
	
	if Controllers ~= nil then
		symb.controllers	= Controllers
	end
	
	if Value ~= nil then
		symb.value		= Value
	end
	
--	symb.parent_element = "background"
	symb.additive_alpha = true
	symb.use_mipfilter = true
	Add(symb)
end

function resetMask(mask, level, parent)
	local area				= createMask(mask.name.."_Reset", mask.vertices, mask.indices, mask.init_pos, parent, nil, nil)
	area.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
	area.level				= DEFAULT_LEVEL + level
	return area
end



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


local function buildLine(name, verts, width, parent, controllers, material)
	local width05 = width / 2
	local tex_line_width = 10
	local tex_line_width05 = tex_line_width / 2

	local elem				= CreateElement "ceSimpleLineObject"
	elem.name				= name
	elem.material			= material
	elem.width				= width05
	elem.vertices			= verts
	elem.tex_params			= {{0.0,tex_line_width/tex_size},{1.0,tex_line_width/tex_size}, {1.0/display_size_pix, 2 * (tex_line_width / width)/display_size_pix}}

	if parent ~= nil then
		elem.parent_element = parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
--	elem.collimated = true
	elem.h_clip_relation	= h_clip_relations.COMPARE
	elem.level				= CMWS_LEVEL
	return elem
end

function addSimpleLine(name, verts, width, material, parent, controllers)
	local line = buildLine(name, verts, width, parent, controllers, material)
	addElem(line)
	return line
end

function addLine(name, verts, width, parent, controllers)
	local line = addSimpleLine(name, verts, width, BRU_Material, parent, controllers) -- 
	return {line}
end

-- angle CCW from +OX
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

function addArc(name, pos, radius, angle_start_deg, angle_end_deg, segments_count, line_width, parent, controllers)
	local verts = buildArcVerts(pos, radius, angle_start_deg, angle_end_deg, segments_count)
	local back_arc = addSimpleLine(name.."_back", verts, line_width + 2, "HMD_SYMBOLOGY_BLACK", parent, controllers)
	local arc = addSimpleLine(name, verts, line_width, "HMD_SYMBOLOGY", parent, controllers)
	return {back_arc, arc}
end

local function splitVerts(tbl, verts)
	for i,v in pairs(verts) do
		if i ~= 1 then
			tbl[#tbl + 1] = v
		end
	end
end

--------------------------------------------------------------------------------------------
local function buildSymbol(name, material, size, pos, align, tex_pos, tex_scale, parent, controllers)
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

	
	return texturedMesh
end

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
-------------------------------------------------------------------------------------------
function getPosOnCircle(radius, angle)
	return { radius * math.sin(math.rad(angle)), radius * math.cos(math.rad(angle)) }
end

------------------------------------------------------------------------------------------------
function draw_arrow( pos, rot, scale, material, parent, controllers )
	local elem  		= CreateElement "ceTexPoly"
	local w = 34/scale
	local h = 34/scale
	elem.vertices		=  buildBoxVerts(w, h, "CenterCenter")
	elem.indices		= default_box_indices
	
	elem.material		= material
	elem.init_pos		= pos
	elem.init_rot 		= {rot,0,0}
	elem.tex_params		= {30 * texture_scale, 50 * texture_scale, scale * texture_scale, scale * texture_scale} 
	
	if parent ~= nil then
	elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	
	elem.h_clip_relation	= h_clip_relations.COMPARE
	elem.level				= CMWS_LEVEL
	
	elem.additive_alpha = true
	elem.use_mipfilter = true
	
	Add(elem)
end

------------------------------------------------------------------------------------------------
function draw_D( pos, rot, scale, material, parent, controllers )
	local elem  		= CreateElement "ceTexPoly"
	local w = 9/scale
	local h = 10/scale
	elem.vertices		=  buildBoxVerts(w, h, "CenterCenter")
	elem.indices		= default_box_indices
	
	elem.material		= material
	elem.init_pos		= pos
	elem.init_rot 		= {rot,0,0}
	elem.tex_params		= {65 * texture_scale, 50 * texture_scale, scale * texture_scale, scale * texture_scale} 
	if parent ~= nil then
	elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	elem.h_clip_relation	= h_clip_relations.COMPARE
	elem.level				= CMWS_LEVEL
	
	elem.additive_alpha = true
	elem.use_mipfilter = true
	
	Add(elem)
end
------------------------------------------------------------------------------------------------
function draw_R( pos, rot, scale, material, parent, controllers )
	local elem  		= CreateElement "ceTexPoly"
	local w = 9/scale
	local h = 10/scale
	elem.vertices		=  buildBoxVerts(w, h, "CenterCenter")
	elem.indices		= default_box_indices
	
	elem.material		= material
	elem.init_pos		= pos
	elem.init_rot 		= {rot,0,0}
	elem.tex_params		= {75 * texture_scale, 50 * texture_scale, scale * texture_scale, scale * texture_scale} 
	if parent ~= nil then
	elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	elem.h_clip_relation	= h_clip_relations.COMPARE
	elem.level				= CMWS_LEVEL
	
	elem.additive_alpha = true
	elem.use_mipfilter = true
	
	Add(elem)
end
--------------------------------------------------------------------------------------------
local function draw_sector( ang, material, parent, controller_name )
	local radius_l			=	35
	local radius_b			=	40
	local radius_small_arr	= (radius_l+radius_b)/2
	local verts_line = { { radius_l * math.cos(math.rad(ang)), radius_l * math.sin(math.rad(ang))}, { radius_b * math.cos(math.rad(ang)), radius_b * math.sin(math.rad(ang))} } 
	local ang_rad = (ang+270) / 180.0 * 3.14159265358979323846
	local	controllers = controller_name and {{ controller_name, ang_rad}} or nil
	if ang % 45 == 0 then
		local point =  { radius_small_arr * math.cos(math.rad(ang)), radius_small_arr * math.sin(math.rad(ang))} 
		draw_arrow( point, ang, 5, material, parent.name, controllers )
	else
		addSimpleLine(nil, verts_line, 15.0, material, parent.name, controllers )
	end
end 
--------------------------------------------------------------------------------------------
function addPattern( material, controller_name, controller_big_arr_name, controller_small_arr_name ) 
	local	verts 		= buildEllipseVerts({0,0}, 90.0, 90.0)
	local	verts_small = buildEllipseVerts({0,0}, 40.0, 40.0)
	local	verts_big 	= buildEllipseVerts({0,0}, 70.0,70.0)
	local	verts_dot 	= buildEllipseVerts({0,0}, 7.0, 7.0)
	local	verts_dot1 	= buildEllipseVerts({0,0}, 15.0, 15.0)
	local	ellipse_indices = buildEllipseIndices()
	local	w05 = 180
	local	h05 = 90
	local	verts_rect = {{-w05, h05}, {w05, h05}, {w05, -h05}, {-w05, -h05}, {-w05, h05}}

	local	mask					= CreateElement "ceMeshPoly"
	--	mask.name				= "cmws_mask"
		mask.primitivetype		= "triangles"
		mask.vertices			= verts_rect
		mask.indices			= default_box_indices
		mask.isvisible			= false
		mask.material			= "MASK_MATERIAL"
		mask.level				= CMWS_LEVEL
		mask.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
		mask.init_pos			= {0, 0, 0}
		mask.isdraw		  		= true
		mask.change_opacity  	= false
		mask.collimated  		= false		
		Add(mask)

	local	controller_0 = controller_name and {{ controller_name, 0}} or nil
	local	controller_1 = controller_name and {{ controller_name, 1}} or nil
	local	controller_2 = controller_name and {{ controller_name, 2}} or nil
	local	controller_3 = controller_name and {{ controller_name, 3}} or nil
	
	local	controller_big_arr_0 = controller_big_arr_name and {{ controller_big_arr_name, 0}} or nil
	local	controller_big_arr_1 = controller_big_arr_name and {{ controller_big_arr_name, 1}} or nil
	local	controller_big_arr_2 = controller_big_arr_name and {{ controller_big_arr_name, 2}} or nil
	local	controller_big_arr_3 = controller_big_arr_name and {{ controller_big_arr_name, 3}} or nil
	
	local sector_ph 		= { addPlaceholder(nil, {45, 0, 0}, mask.name, controller_0), 
								addPlaceholder(nil, {45, 0, 0}, mask.name, controller_1), 
								addPlaceholder(nil, {45, 0, 0}, mask.name, controller_2), 
								addPlaceholder(nil, {45, 0, 0}, mask.name, controller_3)}
								
	local big_arrow_ph 		= { addPlaceholder(nil, {45, 0, 0}, mask.name, controller_big_arr_0), 
								addPlaceholder(nil, {45, 0, 0}, mask.name, controller_big_arr_1), 
								addPlaceholder(nil, {45, 0, 0}, mask.name, controller_big_arr_2), 
								addPlaceholder(nil, {45, 0, 0}, mask.name, controller_big_arr_3)}
	 
	for i = 0,90,15 do
		draw_sector( i, material, big_arrow_ph[4], nil)
	end
		
	for i = 90,180,15 do
		draw_sector( i, material, big_arrow_ph[3], nil)
	end
	
	for i = 180,270,15 do
		draw_sector( i, material, big_arrow_ph[2], nil)
	end
	
	for i = 270,360,15 do
		draw_sector( i, material, big_arrow_ph[1], nil)
	end
	
	
	local radius_big_arr	=	53
	local ang = 45
	local point =  { radius_big_arr * math.cos(math.rad(ang)), radius_big_arr * math.sin(math.rad(ang))} 
	draw_arrow( point, ang, 2, material, big_arrow_ph[4].name, nil )
	
	ang = 135
	point =  { radius_big_arr * math.cos(math.rad(ang)), radius_big_arr * math.sin(math.rad(ang))} 
	draw_arrow( point, ang, 2, material, big_arrow_ph[3].name, nil )
	
	ang = 225
	point =  { radius_big_arr * math.cos(math.rad(ang)), radius_big_arr * math.sin(math.rad(ang))} 
	draw_arrow( point, ang, 2, material, big_arrow_ph[2].name, nil )
	
	ang = 315
	point =  { radius_big_arr * math.cos(math.rad(ang)), radius_big_arr * math.sin(math.rad(ang))} 
	draw_arrow( point, ang, 2, material, big_arrow_ph[1].name, nil )
	
end
