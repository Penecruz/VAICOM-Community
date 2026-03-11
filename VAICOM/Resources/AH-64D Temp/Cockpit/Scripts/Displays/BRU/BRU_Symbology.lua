dofile(LockOn_Options.script_path.."Displays/Common/Units.lua")
dofile(LockOn_Options.script_path.."symbology_defs.lua")
dofile(LockOn_Options.script_path.."Displays/BRU/BRU_Tools.lua")

display_size_inch		= 1.0
display_size_pix		= 128

local pix_to_inch		= display_size_inch / display_size_pix
local inch_to_meter		= 0.0254

local tex_size			= 64
local texture_scale		= 1/tex_size
SetCustomScale(pix_to_inch * inch_to_meter)

additive_alpha	= true
BRU_LEVEL	= 5
local BRU_Material = readParameter("BRU_Material")

function setElemsClipLevel(tbl, level)
	for i, obj in pairs(tbl) do
		setClipLevel(obj, level)
	end
end

local function addElem(elem)
	elem.additive_alpha	= additive_alpha
	Add(elem)
end

function AddBackground(name, invisible)
	local element			= CreateElement "ceMeshPoly"
	element.name			= name
	element.primitivetype	= "triangles"
	element.material		= "BRU_BLACK" 
	element.vertices		= buildBoxVerts(video_area_w, video_area_h, "CenterCenter")
	element.indices			= default_box_indices
	element.blend_mode		= blend_mode.IBM_REGULAR
	element.change_opacity	= false
	element.init_pos		= {0, display_size_pix_05 - video_area_h_05, 0}

	if invisible == true then
		element.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
		element.level			= DEFAULT_LEVEL
		setAsInvisibleMask(element) -- changes material
	end

	Add(element)
	return element
end

local function buildLine(name, verts, width, parent, controllers, material)
	local width05 = width / 2
	local tex_line_width = 10

	local elem				= CreateElement "ceSimpleLineObject"
	elem.name				= name
	elem.material			= material
	elem.width				= width05
	elem.vertices			= verts
	elem.tex_params			= {{0.0, 59.0/tex_size},{1.0, 59.0/tex_size}, {1.0/display_size_pix, 2 * (tex_line_width / width)/display_size_pix}}

	if parent ~= nil then
		elem.parent_element = parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end

	elem.h_clip_relation	= h_clip_relations.COMPARE
	elem.level				= BRU_LEVEL
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

----------------------------------------------------------------------------------------------
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
---------------------------------------------------------------------------------------------
function addDot(name, pos, parent, scale, controllers)
	local elem = buildSymbol(name, BRU_Material, {20,20}, pos, "CenterCenter", {20,20}, scale, parent, controllers)
	
	elem.h_clip_relation	= h_clip_relations.COMPARE
	elem.level				= BRU_LEVEL
	Add(elem)
	return elem
end



function addPattern() 
	local	verts 		= buildEllipseVerts({0,0}, 70.0, 70.0)
	local	verts_small = buildEllipseVerts({0,0}, 35.0, 35.0)
	local	verts_big 	= buildEllipseVerts({0,0}, 65.0,65.0)
	local	verts_dot 	= buildEllipseVerts({0,0}, 15.0, 15.0)
--	local	verts_dot1 	= buildEllipseVerts({0,0}, 15.0, 15.0)
	local	ellipse_indices = buildEllipseIndices()

	local	mask					= CreateElement "ceMeshPoly"
			mask.name				= "bru_mask"
			mask.primitivetype		= "triangles"
			mask.vertices			= verts
			mask.indices			= ellipse_indices
			mask.isvisible			= false
			mask.material			= "MASK_MATERIAL"
			mask.level				= BRU_LEVEL
			mask.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
			mask.init_pos			= {0, 0, 0}
			mask.isdraw		  		= true
			mask.change_opacity  	= false
			mask.collimated  		= false
			mask.controllers	= {{"BRU_Mask"}}
			Add(mask)
	
	local bru_dot_ph 		= addPlaceholder("bru_dot_ph", {0, 0, 0}, nil, {{"BRU_Dot"}})
	local dot	 			= addDot("bru_center_dot", {0,0}, bru_dot_ph.name, 2.0)
--	local dot_small 		= addSimpleLine("bru_dot_small", verts_dot, 2.0, BRU_Material, bru_dot_ph.name)
	
	local circle_small_ph 	= addPlaceholder("circle_small_ph", {0, 0, 0}, nil, {{"BRU_CircleSmall"}})
	local circle_small 		= addSimpleLine("bru_circle_small", verts_small, 20.0, BRU_Material, circle_small_ph.name)
		
	local circle_big_ph 	= addPlaceholder("circle_big_ph", {0, 0, 0}, nil, {{"BRU_CircleBig"}})
	local circle_big		= addSimpleLine("bru_circle_big", verts_big, 20.0, BRU_Material, circle_big_ph.name)
end
