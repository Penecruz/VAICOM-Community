dofile(LockOn_Options.script_path.."symbology_defs.lua")

-- display size:
-- 5 x 5 inches
-- 960 x 960 pixels
display_size_inch		= 5.0
display_size_pix		= GetHalfWidth() * 2	--960
display_size_pix_05		= display_size_pix / 2
local pix_to_inch		= display_size_inch / display_size_pix
local inch_to_meter		= 0.0254
--local pixel_size		= pix_to_inch * inch_to_meter
local pixel_size		= GetHalfWidth() * 2.0 / 1280.0

local tex_size			= 512
local tex_size_h		= 512
local tex_size_v		= 256

ind_scale = GetHalfWidth() * 2.0 / 1280.0

--SetCustomScale(pix_to_inch * inch_to_meter)
collimated		= false
additive_alpha	= true
DEFAULT_LEVEL	= 1

function setElemsClipLevel(tbl, level)
	for i, obj in pairs(tbl) do
		setClipLevel(obj, level)
	end
end

local function addElem(elem)
	elem.additive_alpha	= additive_alpha
	Add(elem)
end


-- Video Area
video_area_w			= display_size_pix
video_area_h			= video_area_w * 3/4
video_area_w_05			= video_area_w / 2
video_area_h_05			= video_area_h / 2


-------------------------------------------
SkidSlipIndTop		= -260 -- ref for sizing Pitch Ladder Scale
PitchLadder_h		= -2 * SkidSlipIndTop
SkidSlipInd_h		= 60
SkidSlipIndBottom	= SkidSlipIndTop - SkidSlipInd_h
-------------------------------------------
FORMAT =
{
	NONE		= 0;
	HOVER		= 2;
	BOB_UP		= 4;
	TRANSITION	= 8;
	CRUISE		= 16;
	WEAPON		= 32;
}

CUEING_DOT =
{
	UP		= 0;
	DOWN	= 1;
	LEFT	= 2;
	RIGHT	= 3;
}

function AddBackground(name, invisible)
	local element			= CreateElement "ceMeshPoly"
	element.name			= name
	element.primitivetype	= "triangles"
	element.material		= "TEDAC_BLACK"
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


-- fonts properties:
-- chars 17 x 25 pix
local font_char_tex_size	= 64
local font_char_tex_h		= 50
local font_char_tex_w		= 34
local font_char_size_h		= 25
local font_char_size_w		= 17
local font_char_space		= 0
local font_line_space		= 0
local font_char_scale_h		= 1 / (font_char_tex_h/font_char_tex_size)
local font_char_scale_w		= 1 / (font_char_tex_w/font_char_tex_size)
local t_font_char_h			= pixel_size * font_char_size_h * font_char_scale_h
local t_font_char_w			= pixel_size * font_char_size_w * font_char_scale_w
local t_font_char_space		= pixel_size * font_char_space
local t_font_line_space		= pixel_size * font_line_space

local t_font_char_dx		= pixel_size * 0
local t_font_char_dy		= pixel_size * font_char_size_h * font_char_scale_h * ((font_char_tex_size - font_char_tex_h)/font_char_tex_size)

font_stringdefs = { t_font_char_h, t_font_char_w, t_font_char_space, t_font_line_space - pixel_size * t_font_char_dy }

function getTextWidth(symbol_num)
	return font_char_size_w * symbol_num + font_char_space * (symbol_num - 1)
end

-- chars 20 x 29 pix
local font_big_char_tex_size	= 64
local font_big_char_tex_h		= 58
local font_big_char_tex_w		= 40
local font_big_char_size_h		= 29
local font_big_char_size_w		= 20
local font_big_char_space		= 0
local font_big_line_space		= 0
local font_big_char_scale_h		= 1 / (font_big_char_tex_h/font_big_char_tex_size)
local font_big_char_scale_w		= 1 / (font_big_char_tex_w/font_big_char_tex_size)
local t_font_big_char_h			= pixel_size * font_big_char_size_h * font_big_char_scale_h
local t_font_big_char_w			= pixel_size * font_big_char_size_w * font_big_char_scale_w
local t_font_big_char_space		= pixel_size * font_big_char_space
local t_font_big_line_space		= pixel_size * font_big_line_space

local t_font_big_char_dx		= pixel_size * 0
local t_font_big_char_dy		= font_big_char_size_h * font_big_char_scale_h * ((font_big_char_tex_size - font_big_char_tex_h)/font_big_char_tex_size)

local font_big_stringdefs = { t_font_big_char_h, t_font_big_char_w, t_font_big_char_space, t_font_big_line_space - pixel_size * t_font_big_char_dy }



--
function resetMask(mask, level, parent)
	local area				= createMask(mask.name.."_Reset", mask.vertices, mask.indices, mask.init_pos, parent, nil, nil)
	area.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
	area.level				= DEFAULT_LEVEL + level
	return area
end



function setCommonStringProperties(elem, name, text, align, parent, controllers, formats)
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

function addText(name, text, pos, align, parent, controllers, formats)
	local pxy = { pos[1] * ind_scale, pos[2] * ind_scale }

	local dy = t_font_char_dy
	if align == "LeftBottom" or align == "CenterBottom" or align == "RightBottom" then
		dy = 0
	end

	local elem			= CreateElement "ceStringPoly"
	setCommonStringProperties(elem, name, text, align, parent, controllers, formats)
	elem.material		= "font_TEDAC"
	elem.init_pos		= {pxy[1] + t_font_char_dx, pxy[2] + dy, 0}
	elem.stringdefs		= font_stringdefs

	local back_elem = Copy(elem)
	back_elem.name		= name.."_back"
	back_elem.material	= "font_TEDAC_BLACK"

	addElem(back_elem)
	addElem(elem)
	return {back_elem, elem}
end

function addBigText(name, text, pos, align, parent, controllers, formats)

	local dy = t_font_big_char_dy
	if align == "LeftBottom" or align == "CenterBottom" or align == "RightBottom" then
		dy = 0
	end

	local elem			= CreateElement "ceStringPoly"
	setCommonStringProperties(elem, name, text, align, parent, controllers, formats)
	elem.material		= "font_TEDAC_big"
	elem.init_pos		= {pos[1] + t_font_big_char_dx, pos[2] + dy, 0}
	elem.stringdefs		= font_big_stringdefs

	local back_elem = Copy(elem)
	back_elem.name		= name.."_back"
	back_elem.material	= "font_TEDAC_big_BLACK"

	addElem(back_elem)
	addElem(elem)
	return {back_elem, elem}
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
	elem.tex_params			= {{0.0,140.0/tex_size_h},{1.0,140.0/tex_size_v}, {1.0/display_size_pix, 2 * (tex_line_width / width)/display_size_pix}}

	if parent ~= nil then
		elem.parent_element = parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end

	setClipLevel(elem)

	return elem
end

function addSimpleLine(name, verts, width, material, parent, controllers)
	local line = buildLine(name, verts, width, parent, controllers, material)
	addElem(line)
	return line
end



local function buildBackLineVerts(p1, p2)
	local back_edge = 1.2
	local v12 = {p2[1] - p1[1], p2[2] - p1[2] }
	local v21 = {p1[1] - p2[1], p1[2] - p2[2] }
	local line_len = math.sqrt(v12[1] * v12[1] + v12[2] * v12[2])
	local v12abs = {v12[1] / line_len, v12[2] / line_len}
	local v21abs = {v21[1] / line_len, v21[2] / line_len}
	local p11 = {p1[1] + v21abs[1] * back_edge, p1[2] + v21abs[2] * back_edge}	-- p11 = p1 + v21abs * back_edge
	local p22 = {p2[1] + v12abs[1] * back_edge, p2[2] + v12abs[2] * back_edge}	-- p22 = p2 + v12abs * back_edge
	return {p11, p22}
end

local function buildBackVerts(v)
	-- we need to change only first and last points in verts
	local p_first	= v[1]
	local p_last	= v[#v]
	local v_first	= buildBackLineVerts(v[1], v[2])
	local v_last	= buildBackLineVerts(v[#v-1], v[#v])
	-- copy verts
	local res = {}
	for i,o in pairs(v) do
		res[i] = o
	end

	if p_first[1] ~= p_last[1] or p_first[2] ~= p_last[2] then
		res[1] = v_first[1]
		res[#res] = v_last[2]
	end

	return res
end

function addSimpleBackLine(name, verts, width, material, parent, controllers)
	return addSimpleLine(name, buildBackVerts(verts), width, material, parent, controllers)
end

function addLine(name, verts, width, parent, controllers)
	local back_line = addSimpleLine(name.."_back", buildBackVerts(verts), width + 2, "TADS_SYMBOLOGY_BLACK", parent, controllers)
	local line = addSimpleLine(name, verts, width, "TADS_SYMBOLOGY", parent, controllers)
	return {back_line, line}
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
	local back_arc = addSimpleLine(name.."_back", verts, line_width + 2, "TADS_SYMBOLOGY_BLACK", parent, controllers)
	local arc = addSimpleLine(name, verts, line_width, "TADS_SYMBOLOGY", parent, controllers)
	return {back_arc, arc}
end

local function splitVerts(tbl, verts)
	for i,v in pairs(verts) do
		if i ~= 1 then
			tbl[#tbl + 1] = v
		end
	end
end

function addRoundedBox(name, pos, align, size, radius, line_width, parent, controllers)
	local segments_count	= 3
	local r					= radius or 8
	local d					= r * 2
	local w					= size[1]
	local h					= size[2]
	local w05				= w / 2
	local h05				= h / 2

	local x = pos[1]
	local y = pos[2]

	local c = getCenter(w, h, align)

	local cx = c[1]
	local cy = c[2]

	local PH = addPlaceholder(name.."_PH", {x + cx, y + cy}, parent, controllers)

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

	local back_box = addSimpleLine(name.."_back", verts, line_width + 2, "TADS_SYMBOLOGY_BLACK", PH.name, nil)
	local box = addSimpleLine(name, verts, line_width, "TADS_SYMBOLOGY", PH.name, nil)
end

function buildSymbol(name, material, size, pos, align, tex_pos, tex_scale, parent, controllers)
	local texturedMesh			= CreateElement "ceTexPoly"

	setSymbolCommonProperties(texturedMesh, name, pos, parent, controllers, material)
	texturedMesh.indices		= default_box_indices

	local width		= size[1]
	local height	= size[2]
	texturedMesh.vertices		= buildBoxVerts(width, height, align)

	local tex_x = tex_pos[1]
	local tex_y = tex_pos[2]
	local scale_h = tex_scale / tex_size_h
	local scale_v = tex_scale / tex_size_v
	texturedMesh.tex_params		= {tex_x/tex_size_h, tex_y/tex_size_v, scale_h, scale_v}

	return texturedMesh
end

function buildBorderedSymbol(name, size, align, tex_pos, pos, parent, controllers)
	local sz = { size[1] * ind_scale, size[2] * ind_scale }
	local pxy = { pos[1] * ind_scale, pos[2] * ind_scale }

	local elem = buildSymbol(name, "TADS_SYMBOLOGY", sz, pxy, align, tex_pos, 2 / ind_scale, parent, controllers)

	local back_elem = Copy(elem)
	back_elem.material = "TADS_SYMBOLOGY_BLACK"

	addElem(back_elem)
	addElem(elem)
	return {back_elem, elem}
end


--
function addPointer_FcrCenterlineBearing(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {19,14}, "CenterTop", {112,147}, pos, parent, controllers)
	return elems
end

function addPointer_AlternateSensorBearing(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {17,16}, "CenterTop", {149,147}, pos, parent, controllers)
	return elems
end

function addPointer_AdfBearing(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {12,24}, "CenterTop", {70,147}, pos, parent, controllers)
	return elems
end

function addPointer_BobUpHeading(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {22,22}, "CenterTop", {25,147}, pos, parent, controllers)
	return elems
end

function addDot(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {12,12}, "CenterCenter", {181,163}, pos, parent, controllers)
	return elems
end

function addSkidSlipBall(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {23,23}, "CenterCenter", {53,224}, pos, parent, controllers)
	return elems
end

function addPoint(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {8,8}, "CenterCenter", {203,163}, pos, parent, controllers)
	return elems
end

function addFcrCurrentCenterline(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {6,64}, "CenterCenter", {5,266}, pos, parent, controllers)
	return elems
end

function addFcrLastCenterline(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {6,64}, "CenterCenter", {21,266}, pos, parent, controllers)
	return elems
end

function addAccelerationCue(name, pos, parent, controllers)
	local elems = addArc(name, pos, 8, 0, 360, 12, 4, parent, controllers)
	return elems
end

function addCuedLosReticle(name, pos, parent, controllers )
	local size05 	= PitchLadder_h/16
	local s05 		= 8 -- space
	local w05_len	= (size05 - s05)/2.9
	local w05_sp	= 0.9 * w05_len
	local h05_len	= w05_len
	local h05_sp	= w05_sp
	local w 		= 12
	local sh 		= 0

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local s05_1 = s05 + h05_len + h05_sp

	local lu_1  = addLine(name.."_up_1",	{{   0, s05}, 	{ 0, s05 + h05_len}},	w, PH.name)
	local lu_11 = addLine(name.."_up_11",	{{   0, s05_1},	{ 0, s05_1 + h05_len}},	w, PH.name)

	local ld_1	= addLine(name.."_down_1",	{{   0, -s05}, 	{ 0, -s05-h05_len}},	w, PH.name)
	local ld_11	= addLine(name.."_down_11",	{{   0, -s05_1},{ 0, -s05_1-h05_len}},	w, PH.name)

	s05_1 = s05 + w05_len + w05_sp

	local lr_1	= addLine(name.."_right_1",	{{ s05,   0},	{ s05 + w05_len,   0}},	w, PH.name)
	local lr_11	= addLine(name.."_right_11",{{ s05_1, 0},	{ s05_1 + w05_len, 0}},	w, PH.name)

	local ll_1	= addLine(name.."_left_1",	{{ -s05,   0},	{ -s05-w05_len,   0}},	w,  PH.name)
	local ll_11	= addLine(name.."_left_11",	{{ -s05_1, 0},	{ -s05_1-w05_len, 0}},	w,  PH.name)

	return { PH, lu_1[1], lu_1[2], lu_11[1], lu_11[2], ld_1[1], ld_1[2], ld_11[1], ld_11[2], lr_1[1], lr_1[2], lr_11[1], lr_11[2], ll_1[1], ll_1[2], ll_11[1], ll_11[2], }
end


function addHeadTracker(name, pos, parent, controllers)
	local size			= 84
	local s05			= size / 2
	local l				= s05 / 3
	local line_width	= 4

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	-- corners
	local c_u = addSimpleLine(name.."_up",		{{      -l, s05 - l}, {   0, s05}, {       l, s05 - l}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	local c_d = addSimpleLine(name.."_down",	{{      -l,-s05 + l}, {   0,-s05}, {       l,-s05 + l}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	local c_l = addSimpleLine(name.."_left",	{{-s05 + l,       l}, {-s05,   0}, {-s05 + l,      -l}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	local c_r = addSimpleLine(name.."_right",	{{ s05 - l,       l}, { s05,   0}, { s05 - l,      -l}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)

	return {
		PH,
		c_u, c_d, c_l, c_r,
	}
end


function add_HOVER_LOS_Reticle(name, pos, parent, controllers)
	local size = PitchLadder_h/16
	local ret_val = add_LOS_Reticle("HOVER", size, pos, parent, controllers, 4)
	return ret_val
end

function add_CRUISE_LOS_Reticle(name, pos, parent, controllers)
	local size = PitchLadder_h/16
	local ret_val = add_LOS_Reticle("CRUISE", size, pos, parent, controllers, 8)
	return ret_val
end

function add_TRANSITION_LOS_Reticle(name, pos, parent, controllers)
	local size = PitchLadder_h/16
	local ret_val = add_LOS_Reticle("TRANSITION", size, pos, parent, controllers, 4)
	return ret_val
end

function add_LOS_Reticle(name, size, pos, parent, controllers, thickness )
	local s05 = 10	-- space
	local w05 = size - s05
	local h05 = w05
	t = thickness or 4
--	local s = w05 + 2*s05
	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local lu = addLine(name.."_up",		{{   0, s05}, {   0, s05+h05}}, t, PH.name)
	local ld = addLine(name.."_down",	{{   0,-s05}, {   0, -s05-h05}},t, PH.name)
	local ll = addLine(name.."_left",	{{-s05,   0}, {-s05-w05,   0}}, t, PH.name)
	local lr = addLine(name.."_right",	{{ s05,   0}, { s05+w05,   0}}, t, PH.name)
	return { PH, lu, ld, ll, lr}
end
-- TADS LOS Reticle defs
local TADS_LOS_Reticle_w05 = 100
local TADS_LOS_Reticle_h05 = 75

function add_TADS_LOS_Reticle(name, pos, parent, controllers)
	local w05 = TADS_LOS_Reticle_w05
	local h05 = TADS_LOS_Reticle_h05
	local s_w05 = 14	-- space
	local s_h05 = 10	-- space

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local lu = addLine(name.."_up",		{{     0, s_h05}, {   0, h05}},	4, PH.name, nil)
	local ld = addLine(name.."_down",	{{     0,-s_h05}, {   0,-h05}},	4, PH.name, nil)
	local ll = addLine(name.."_left",	{{-s_w05,     0}, {-w05,   0}},	4, PH.name, nil)
	local lr = addLine(name.."_right",	{{ s_w05,     0}, { w05,   0}},	4, PH.name, nil)
	return {
		PH,
		lu[1], lu[2],
		ld[1], ld[2],
		ll[1], ll[2],
		lr[1], lr[2],
	}
end

function add_LMC_On(name, pos, parent, controllers)
	local w05 = TADS_LOS_Reticle_w05 + 5
	local h05 = TADS_LOS_Reticle_h05 + 5
	local len = 10
	local l05 = len / 2

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local l1 = addLine(name.."_up",		{{-l05, h05}, { l05, h05}}, 4, PH.name, nil)
	local l2 = addLine(name.."_down",	{{-l05,-h05}, { l05,-h05}}, 4, PH.name, nil)
	local l3 = addLine(name.."_left",	{{-w05,-l05}, {-w05, l05}}, 4, PH.name, nil)
	local l4 = addLine(name.."_right",	{{ w05,-l05}, { w05, l05}}, 4, PH.name, nil)
	return {
		PH,

		l1[1], l1[2],
		l2[1], l2[2],
		l3[1], l3[2],
		l4[1], l4[2],
	}
end

function add_LaserFiringIndicator(name, pos, parent, controllers)
	local len_gain = 4 / 5
	local w05 = TADS_LOS_Reticle_h05 * len_gain
	local h05 = TADS_LOS_Reticle_h05 * len_gain
	local s_w05 = w05 / 2
	local s_h05 = s_w05

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local l1 = addLine(name.."_up_left",	{{-s_w05, s_h05}, {-w05, h05}},	4, PH.name, nil)
	local l2 = addLine(name.."_up_right",	{{ s_w05, s_h05}, { w05, h05}},	4, PH.name, nil)
	local l3 = addLine(name.."_down_left",	{{-s_w05,-s_h05}, {-w05,-h05}},	4, PH.name, nil)
	local l4 = addLine(name.."_down_right",	{{ s_w05,-s_h05}, { w05,-h05}},	4, PH.name, nil)
	return {
		PH,
		l1[1], l1[2],
		l2[1], l2[2],
		l3[1], l3[2],
		l4[1], l4[2],
	}
end

-- TODO: The size of the LOBL constraint box represents +/- 20 deg constraints limit
-- and the LOAL constraint box represents a +/- 7.5 deg constraints limit from the ADL.
local LOAL_size	= 38
local LOBL_size = 158

function add_LOAL_out_of_constraints_missile_box(name, pos, parent, controllers)
	local size		= LOAL_size
	local s05		= size / 2
	local dash		= 8
	local d05		= dash / 2
	--local gap		= 7
	local line_width		= 3	-- line width

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	-- back rect
	local back_line = buildLine(name.."_back",
		{
			{-s05, s05},
			{ s05, s05},
			{ s05,-s05},
			{-s05,-s05},
			{-s05, s05},
		},
		line_width + 2, PH.name, nil, "TADS_SYMBOLOGY_BLACK")
	addElem(back_line)
	-- corners
	local c_ul = addSimpleLine(name.."_up_left",	{{-s05, s05 - dash}, {-s05, s05}, {-s05 + dash, s05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	local c_ur = addSimpleLine(name.."_up_right",	{{ s05, s05 - dash}, { s05, s05}, { s05 - dash, s05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	local c_dl = addSimpleLine(name.."_down_left",	{{-s05,-s05 + dash}, {-s05,-s05}, {-s05 + dash,-s05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	local c_dr = addSimpleLine(name.."_down_right",	{{ s05,-s05 + dash}, { s05,-s05}, { s05 - dash,-s05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	-- centers
	local c_u = addSimpleLine(name.."_up",		{{-d05, s05}, { d05, s05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	local c_d = addSimpleLine(name.."_down",	{{-d05,-s05}, { d05,-s05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	local c_l = addSimpleLine(name.."_left",	{{-s05,-d05}, {-s05, d05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	local c_r = addSimpleLine(name.."_right",	{{ s05,-d05}, { s05, d05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)

	return
	{
		PH,
		back_line,
		c_ul, c_ur, c_dl, c_dr,
		c_u, c_d, c_l, c_r,
	}
end

function add_LOBL_out_of_constraints_missile_box(name, pos, parent, controllers)
	local size			= LOBL_size
	local s05			= size / 2
	local dash			= 11
	local d05			= dash / 2
	local gap			= 10
	local period		= dash + gap
	local line_width	= 3	-- line width

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	-- back rect
	local back_line = buildLine(name.."_back",
		{
			{-s05, s05},
			{ s05, s05},
			{ s05,-s05},
			{-s05,-s05},
			{-s05, s05},
		},
		line_width + 2, PH.name, nil, "TADS_SYMBOLOGY_BLACK")
	addElem(back_line)
	-- corners
	local c_ul = addSimpleLine(name.."_corner_up_left",		{{-s05, s05 - dash}, {-s05, s05}, {-s05 + dash, s05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	local c_ur = addSimpleLine(name.."_corner_up_right",	{{ s05, s05 - dash}, { s05, s05}, { s05 - dash, s05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	local c_dl = addSimpleLine(name.."_corner_down_left",	{{-s05,-s05 + dash}, {-s05,-s05}, {-s05 + dash,-s05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	local c_dr = addSimpleLine(name.."_corner_down_right",	{{ s05,-s05 + dash}, { s05,-s05}, { s05 - dash,-s05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
	--
	local l0 = -s05 + dash + gap
	local dashes = {}

	for i = 0,5 do
		local lb = l0 + period * i
		local le = lb + dash
		local d_u = addSimpleLine(name.."_up"..i,		{{ lb, s05}, { le, s05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
		local d_d = addSimpleLine(name.."_down"..i,		{{ lb,-s05}, { le,-s05}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
		local d_l = addSimpleLine(name.."_left"..i,		{{-s05, lb}, {-s05, le}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
		local d_r = addSimpleLine(name.."_right"..i,	{{ s05, lb}, { s05, le}},	line_width, "TADS_SYMBOLOGY", PH.name, nil)
		dashes[#dashes + 1] = d_u
		dashes[#dashes + 1] = d_d
		dashes[#dashes + 1] = d_l
		dashes[#dashes + 1] = d_r
	end

	local elems =
	{
		PH,
		back_line,
		c_ul, c_ur, c_dl, c_dr,
	}

	for i,v in pairs(dashes) do
		elems[#elems + 1] = v
	end

	return elems
end

function add_LOAL_in_constraints_missile_box(name, pos, parent, controllers)
	local rect = addRect(name, LOAL_size, LOAL_size, pos, "CenterCenter", 5, parent, controllers)
	return rect
end

function add_LOBL_in_constraints_missile_box(name, pos, parent, controllers)
	local rect = addRect(name, LOBL_size, LOBL_size, pos, "CenterCenter", 5, parent, controllers)
	return rect
end

function add_missile_constraints_box(parent)
	local PH = addPlaceholder("MissileConstraintsBox_PH", {0,0}, parent, {{"DSPLS_WEAPON_MissileConstraintsBox_Show"},{"DSPLS_WEAPON_MissileConstraintsBox_Pos", video_area_w_05, video_area_h_05}})
	local isLOAL, isLOBL 	= 0, 1
	local isOK, isInvalid 	= 0, 1

	add_LOAL_out_of_constraints_missile_box("LOAL_out_MissileBox",	{0,0},	PH.name, {{"DSPLS_WEAPON_MissileConstraintsBox_isLOBL", isLOAL}, {"DSPLS_WEAPON_MissileConstraintsBox_Dashed", isInvalid}})
	add_LOBL_out_of_constraints_missile_box("LOBL_out_MissileBox",	{0,0},	PH.name, {{"DSPLS_WEAPON_MissileConstraintsBox_isLOBL", isLOBL}, {"DSPLS_WEAPON_MissileConstraintsBox_Dashed", isInvalid}})
	add_LOAL_in_constraints_missile_box("LOAL_in_MissileBox",		{0,0}, 	PH.name, {{"DSPLS_WEAPON_MissileConstraintsBox_isLOBL", isLOAL}, {"DSPLS_WEAPON_MissileConstraintsBox_Dashed", isOK}})
	add_LOBL_in_constraints_missile_box("LOBL_in_MissileBox",		{0,0}, 	PH.name, {{"DSPLS_WEAPON_MissileConstraintsBox_isLOBL", isLOBL}, {"DSPLS_WEAPON_MissileConstraintsBox_Dashed", isOK}})

	-- DBG
	-- local name = "dbg_MissileBox_ScreenBorders"
	-- local PH_dbg = addPlaceholder("dbg_MissileBox_ScreenBorders_PH_0", {0,0}, parent, {{"dbgDSPLS_borders_Pos", 0, video_area_w_05, video_area_w_05}})
	-- addLine(name.."0", {{-video_area_w_05, 0}, {video_area_w_05, 0}}, 3, PH_dbg.name, nil)
	-- PH_dbg = addPlaceholder("dbg_MissileBox_ScreenBorders_PH_1", {0,0}, parent, {{"dbgDSPLS_borders_Pos", 1, video_area_w_05, video_area_w_05}})
	-- addLine(name.."1", {{-video_area_w_05, 0}, {video_area_w_05, 0}}, 3, PH_dbg.name, nil)
	-- PH_dbg = addPlaceholder("dbg_MissileBox_ScreenBorders_PH_2", {0,0}, parent, {{"dbgDSPLS_borders_Pos", 2, video_area_w_05, video_area_w_05}})
	-- addLine(name.."2", {{0, -video_area_h_05}, {0, video_area_h_05}}, 3, PH_dbg.name, nil)
	-- PH_dbg = addPlaceholder("dbg_MissileBox_ScreenBorders_PH_3", {0,0}, parent, {{"dbgDSPLS_borders_Pos", 3, video_area_w_05, video_area_w_05}})
	-- addLine(name.."3", {{0, -video_area_h_05}, {0, video_area_h_05}}, 3, PH_dbg.name, nil)
end

function addRateOfClimb(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {17,16}, "CenterTop", {149,147}, pos, parent, controllers)
	elems[1].init_rot = {-90,0,0}
	elems[2].init_rot = {-90,0,0}
	return elems
end

function addAltitudeHold(name, pos, parent, controllers)
	local x = pos[1]
	local y = pos[2]

	local w = 20
	local w05 = 8
	local h = 22
	local h05 = h / 2
	local verts =
	{
		{x-w, y},
		{x-w, y+h05},
		{x-w05, y+h05},
		{x, y},
		{x-w05, y-h05},
		{x-w, y-h05},
		{x-w, y},
	}
	local elems = addLine(name, verts, 4, parent, controllers)
	return elems
end

function addBobUpBox(name, pos, parent, controllers)
	local x		= pos[1]
	local y		= pos[2]
	local s		= (1-6.0/40.0)*PitchLadder_h/80.0*12.0
	local s05	= s / 2.0
	local w05	= s / 4.0

	local verts =
	{
		{x,			y + s05},
		{x - w05,	y + s05},
		{x - s05,	y + w05},
		{x - s05,	y - w05},
		{x - w05,	y - s05},
		{x + w05,	y - s05},
		{x + s05,	y - w05},
		{x + s05,	y + w05},
		{x + w05,	y + s05},
		{x,			y + s05},
	}
	local elems = addLine(name, verts, 4, parent, controllers)
	return elems
end

function addFlightPath(name, pos, parent, controllers)
	local r				= 11
	local line_width	= 4

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local c_verts = buildArcVerts({0,0}, r, 0, 360, 16)
	local up_verts = {{ 0,r}, {   0,r*2}}
	local lh_verts = {{-r,0}, {-r*2,  0}}
	local rh_verts = {{ r,0}, { r*2,  0}}

	-- back
	local circle_back	= addSimpleBackLine(name.."_circle_back",	c_verts, line_width + 2, "TADS_SYMBOLOGY_BLACK", PH.name)
	local notch_up_back	= addSimpleBackLine(name.."_up_back",		up_verts, line_width + 2, "TADS_SYMBOLOGY_BLACK", PH.name)
	local notch_lh_back	= addSimpleBackLine(name.."_lh_back",		lh_verts, line_width + 2, "TADS_SYMBOLOGY_BLACK", PH.name)
	local notch_rh_back	= addSimpleBackLine(name.."_rh_back",		rh_verts, line_width + 2, "TADS_SYMBOLOGY_BLACK", PH.name)
	-- front
	local circle		= addSimpleLine(name.."_circle",	c_verts, line_width, "TADS_SYMBOLOGY", PH.name)
	local notch_up		= addSimpleLine(name.."_up",		up_verts, line_width, "TADS_SYMBOLOGY", PH.name)
	local notch_lh		= addSimpleLine(name.."_lh",		lh_verts, line_width, "TADS_SYMBOLOGY", PH.name)
	local notch_rh		= addSimpleLine(name.."_rh",		rh_verts, line_width, "TADS_SYMBOLOGY", PH.name)

	return {
		PH,
		circle_back,	notch_up_back,	notch_lh_back,	notch_rh_back,
		circle,			notch_up,		notch_lh,		notch_rh,
	}
end

function addNavFlyToCUE(name, pos, parent, controllers)
	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local point = addPoint(name.."_point", {0,0}, PH.name)

	local s = 28
	local f = 15
	local verts =
	{
		{0, -f},
		{s-f, -f},
		{s, 0},
		{0, s},
		{-s, 0},
		{-s+f, -f},
		{0, -f},
	}
	local diamond = addLine(name.."_diamond", verts, 4, PH.name)

	return {
		PH,
		point, diamond
	}
end

function SetScreenSpace(object)
	object.h_clip_relation = h_clip_relations.NULL
	object.screenspace = 3
end

function addHorizonLine(name, pos, parent, controllers)
	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local period = 55
	local dash = 27

	local elems = { PH }

	for i = 1,4,1 do
		local ps = period * i
		local pe = ps + dash

		local dl = addLine(name.."_dash_lhs_"..i, {{-ps,0}, {-pe,0}}, 4, PH.name)
		local dr = addLine(name.."_dash_rhs_"..i, {{ ps,0}, { pe,0}}, 4, PH.name)

		elems[#elems + 1] = dl[1]
		elems[#elems + 1] = dl[2]
		elems[#elems + 1] = dr[1]
		elems[#elems + 1] = dr[2]
	end

	return elems
end

local function join(table, elems)
	for i,v in pairs(elems) do
		table[#table + 1] = v
	end
end

function addPitchLadder(name, pos, parent, controllers)
	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)
	local elems = { PH }

	local hor_line_len_05 = 270
	local digits = { 10, 20, 30, 45, 60 }
	local hor_line = addLine(name.."HorizonLine", {{-hor_line_len_05,0},{hor_line_len_05,0}}, 4, PH.name)
	join(elems, hor_line)

	local step_y	= PitchLadder_h/4
	local dash		= 13
	local length	= dash * 5
	local width		= hor_line_len_05 / 3
	local gap		= width - length
	local dgap		= 13

	local y		= 0.0
	local xs	= 0.0
	local xe	= 0.0
	local xm	= 0.0
	local dym	= -3

	for i = 1,5 do
		local dy = (i < 4) and  step_y or 1.5 * step_y
		y = y + dy
		xs = gap + dgap * (i - 1)
		xe = xs + length
		xm = xe + 5

			-- sky portion
		local sky_lhs = addLine(name.."_SkyLhs_"..i, {{-xs,y},{-xe,y},{-xe,y-dash}}, 4, PH.name)
		local sky_rhs = addLine(name.."_SkyRhs_"..i, {{ xs,y},{ xe,y},{ xe,y-dash}}, 4, PH.name)
		join(elems, sky_lhs)
		join(elems, sky_rhs)
		local sky_mark_lhs = addText(name.."_SkyMarkLhs_"..i, digits[i], {-xm,y+dym}, "RightCenter", PH.name)
		local sky_mark_rhs = addText(name.."_SkyMarkRhs_"..i, digits[i], { xm,y+dym}, "LeftCenter", PH.name)
		join(elems, sky_mark_lhs)
		join(elems, sky_mark_rhs)
		-- ground portion
		local sky_lhs_1 = addLine(name.."_GroundLhs_1_"..i, {{-xs-0*dash,-y},{-xs-1*dash,-y}}, 4, PH.name)
		local sky_lhs_2 = addLine(name.."_GroundLhs_2_"..i, {{-xs-2*dash,-y},{-xs-3*dash,-y}}, 4, PH.name)
		local sky_lhs_c = addLine(name.."_GroundLhs_c_"..i, {{-xs-4*dash,-y},{-xs-5*dash,-y},{-xe,-y+dash}}, 4, PH.name)
		join(elems, sky_lhs_1)
		join(elems, sky_lhs_2)
		join(elems, sky_lhs_c)
		local sky_rhs_1 = addLine(name.."_GroundRhs_1_"..i, {{xs+0*dash,-y},{xs+1*dash,-y}}, 4, PH.name)
		local sky_rhs_2 = addLine(name.."_GroundRhs_2_"..i, {{xs+2*dash,-y},{xs+3*dash,-y}}, 4, PH.name)
		local sky_rhs_c = addLine(name.."_GroundRhs_c_"..i, {{xs+4*dash,-y},{xs+5*dash,-y},{ xe,-y+dash}}, 4, PH.name)
		join(elems, sky_rhs_1)
		join(elems, sky_rhs_2)
		join(elems, sky_rhs_c)
		local ground_mark_lhs = addText(name.."_GroundMarkLhs_"..i, digits[i], {-xm,-y+dym}, "RightCenter", PH.name)
		local ground_mark_rhs = addText(name.."_GroundMarkRhs_"..i, digits[i], { xm,-y+dym}, "LeftCenter", PH.name)
		join(elems, ground_mark_lhs)
		join(elems, ground_mark_rhs)
	end
		--CLIMB&DIV
	y = y + 3.0 * step_y
	local climb_ball = addSkidSlipBall(name.."climb_ball", {0, y}, PH.name )
	local climb_lhs	=  addLine(name.."climb_Lhs", 	{{-xs*0.66,y+xs/2},{-xs*0.66,y-xs/2}}, 4, PH.name)
	local climb_rhs	=  addLine(name.."climb_Rhs", 	{{ xs*0.66,y+xs/2},{ xs*0.66,y-xs/2}}, 4, PH.name)

	local climb_PH = addRotPlaceholder(name.."climb_PH", {0, y}, 180.0,  PH.name)
	local climb_up = addText(name.."climb_up", "CLIMB", {0,y+xs*0.5}, "CenterBottom", PH.name)
	local climb_dn = addText(name.."climb_dn", "CLIMB", {0, xs*0.5}, "CenterBottom", climb_PH.name)

	join(elems, climb_ball)
	join(elems, climb_lhs)
	join(elems, climb_rhs)
	join(elems, climb_up)
	join(elems, climb_dn)

	local div_ball = addSkidSlipBall(name.."div_ball", {0, -y}, PH.name )
	local div_lhs	=  addLine(name.."div_Lhs", 	{{-xs*0.66,-y+xs/2},{-xs*0.66,-y-xs/2}}, 4, PH.name)
	local div_rhs	=  addLine(name.."div_Rhs", 	{{ xs*0.66,-y+xs/2},{ xs*0.66,-y-xs/2}}, 4, PH.name)

	local div_PH = addRotPlaceholder(name.."div_PH", {0, -y}, 180.0,  PH.name)
	local div_up = addText(name.."div_up", "DIVE", {0,-y+xs*0.5}, "CenterBottom", PH.name)
	local div_dn = addText(name.."div_dn", "DIVE", {0, xs*0.5}, "CenterBottom", div_PH.name)
	join(elems, div_ball)
	join(elems, div_lhs)
	join(elems, div_rhs)
	join(elems, div_up)
	join(elems, div_dn)
	return elems
end

function addBankAnglePointer(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {17,16}, "CenterTop", {149,147}, pos, parent, controllers)
	return elems
end


--function addVideoSignal()
--	-- Add base
--	local	verts = buildBoxVerts(960, 720, "CenterCenter")
--	local	render_back						= CreateElement "ceMeshPoly"
--			render_back.name				= "VideoSignalBack"
--			render_back.primitivetype		= "triangles"
--			render_back.vertices			= verts
--			render_back.indices				= default_box_indices
--			render_back.isvisible			= false
--			render_back.material			= "MASK_MATERIAL"
--			render_back.level				= DEFAULT_LEVEL
--			render_back.h_clip_relation		= h_clip_relations.REWRITE_LEVEL
--			render_back.init_pos			= {0, display_size_pix_05 - video_area_h_05, 0}
--	Add(render_back)
--
--	local	render_base	 = addPlaceholder("VideoSignalBase", {0,0}, render_back.name)
--
--	local	PNVS_underlay					= CreateElement "ceTexPoly"
--			PNVS_underlay.name				= "PNVS_underlay"
--			PNVS_underlay.vertices			= verts
--			PNVS_underlay.indices			= default_box_indices
--			PNVS_underlay.tex_coords		= {{0, 1},{0, 0},{1, 0},{1, 1}}
--			PNVS_underlay.material			= MakeMaterial("PNVS_CAM_AH64_TEDAC",{255,255,255,255})
--			PNVS_underlay.level				= DEFAULT_LEVEL
--			PNVS_underlay.h_clip_relation	= h_clip_relations.COMPARE
--			PNVS_underlay.parent_element	= render_base.name
--			PNVS_underlay.additive_alpha	= true
--			PNVS_underlay.controllers		= {{"VideoUnderlay_PNVS"}}
--	Add(PNVS_underlay)
--
--	local	TADS_underlay					= CreateElement "ceTexPoly"
--			TADS_underlay.name				= "TADS_underlay"
--			TADS_underlay.vertices			= verts
--			TADS_underlay.indices			= default_box_indices
--			TADS_underlay.tex_coords		= {{0, 1},{0, 0},{1, 0},{1, 1}}
--			TADS_underlay.material			= MakeMaterial("TADS_CAM_AH64_TEDAC",{255,255,255,255})
--			TADS_underlay.level				= DEFAULT_LEVEL
--			TADS_underlay.h_clip_relation	= h_clip_relations.COMPARE
--			TADS_underlay.parent_element	= render_base.name
--			TADS_underlay.additive_alpha	= true
--			TADS_underlay.controllers		= {{"VideoUnderlay_TADS"}}
--	Add(TADS_underlay)
--end
--
------------------------------------------------------------------------------------------------------
function add_RKT_SteeringCursor(parent)
	local scale_factor	= 0.9
	local base_name		= "RKT_SteeringCursor"

	local dash_h		= 19
	local dash_v		= 27
	local gap_h			= dash_h
	local gap_v			= 9
	local line_width	= 3
	local fon_delta		= 2		-- outline thickness
	local line_mat		= "TADS_SYMBOLOGY"
	local fon_mat		= "TADS_SYMBOLOGY_BLACK"

	dash_h		= dash_h*scale_factor
	dash_v		= dash_v*scale_factor
	gap_h		= gap_h*scale_factor
	gap_v		= gap_v*scale_factor

	local line_length =
	{
		t = dash_h*4 + gap_h*3,
		b = dash_h*4 + gap_h*3,
		c = dash_v*5 + gap_v*4,
		cs = dash_v*2 + gap_v
	}

	local line_pos =
	{
		top			= {line_length.t/2,	line_length.c/2 + dash_v/3},
		bottom		= {line_length.b/2,	-(line_length.c/2 + dash_v/3)},
		center_t	= {0,				line_length.c/2},
		center_d	= {0,				-line_length.c/2}
	}

	local fon_pos =
	{
		top			= {line_pos.top[1]+fon_delta,		line_pos.top[2]},
		bottom		= {line_pos.bottom[1]+fon_delta,	line_pos.bottom[2]},
		center_t	= {line_pos.center_t[1],			line_pos.center_t[2]+fon_delta},
		center_d	= {line_pos.center_d[1],			line_pos.center_d[2]-fon_delta}
	}

	-- draw if steering cursor must be drawn, set position
	local PH_Base = addPlaceholder(base_name.."_PH_Base", {0,0}, parent, {{"DSPLS_WEAPON_RocketSteeringCursor_Show"},{"DSPLS_WEAPON_RocketSteeringCursor_Pos", video_area_w_05, video_area_h_05}})

	-- draw if cursor is (1) or (2)
	local PH_Inhibit = addPlaceholder(base_name.."_Inhibit_PH", {0,0}, PH_Base.name, {{"DSPLS_WEAPON_RocketSteeringCursor_Dashed", 0}})

	-- (1)/(2) Articulating / Ground stow / inhibited
	--	fon
	addFatDashedLine(base_name.."_Inhibit_topline_fon",		line_length.t+fon_delta*2,	dash_h+fon_delta*2,	gap_h-fon_delta*2,	line_width+fon_delta*2, fon_pos.top,	90,	PH_Inhibit.name, nil, fon_mat)
	addFatDashedLine(base_name.."_Inhibit_bottomline_fon",	line_length.b+fon_delta*2,	dash_h+fon_delta*2,	gap_h-fon_delta*2,	line_width+fon_delta*2, fon_pos.bottom,	90,	PH_Inhibit.name, nil, fon_mat)

	addFatDashedLine(base_name.."_Inhibit_centerline_up_fon",	line_length.cs+fon_delta*2, dash_v+fon_delta*2,	gap_v-fon_delta*2,	line_width+fon_delta*2, line_pos.center_t,		180,	PH_Inhibit.name, nil, fon_mat)
	addFatLine(base_name.."_Inhibit_centerline_cntr_fon",		dash_v+fon_delta*2,													line_width+fon_delta*2, {0,dash_v/2+fon_delta},	180,	PH_Inhibit.name, {{"DSPLS_WEAPON_RocketSteeringCursor_Stowed", 0}}, fon_mat)
	addFatDashedLine(base_name.."_Inhibit_centerline_dn_fon",	line_length.cs+fon_delta*2, dash_v+fon_delta*2,	gap_v-fon_delta*2,	line_width+fon_delta*2, line_pos.center_d,		0,		PH_Inhibit.name, nil, fon_mat)

	-- lines
	addFatDashedLine(base_name.."_Inhibit_topline",		line_length.t, dash_h,	gap_h,	line_width, line_pos.top,		90,	PH_Inhibit.name, nil, line_mat)
	addFatDashedLine(base_name.."_Inhibit_bottomline",	line_length.b, dash_h,	gap_h,	line_width, line_pos.bottom,	90,	PH_Inhibit.name, nil, line_mat)

	addFatDashedLine(base_name.."_Inhibit_centerline_up",	line_length.cs, dash_v,	gap_v,	line_width, line_pos.center_t,	180,	PH_Inhibit.name, nil, line_mat)
	addFatLine(base_name.."_Inhibit_centerline_cntr",		dash_v,							line_width, {0,dash_v/2},		180,	PH_Inhibit.name, {{"DSPLS_WEAPON_RocketSteeringCursor_Stowed", 0}}, line_mat)
	addFatDashedLine(base_name.."_Inhibit_centerline_dn",	line_length.cs, dash_v,	gap_v,	line_width, line_pos.center_d,	0,		PH_Inhibit.name, nil, line_mat)

	-- ----------------------------------------------

	-- draw if cursor is (3) or (4)
	local PH_Normal = addPlaceholder(base_name.."_Normal_PH", {0,0}, PH_Base.name, {{"DSPLS_WEAPON_RocketSteeringCursor_Dashed", 1}})

	-- draw one or another
	local PH_Normal_Articulating	= addPlaceholder(base_name.."_Normal_Art_PH", {0,0}, PH_Normal.name, {{"DSPLS_WEAPON_RocketSteeringCursor_Stowed", 0}})
	local PH_Normal_Stowed			= addPlaceholder(base_name.."_Normal_Stow_PH", {0,0}, PH_Normal.name, {{"DSPLS_WEAPON_RocketSteeringCursor_Stowed", 1}})

	-- (3)/(4) Articulating / Ground stow
	-- fon
	addFatDashedLine(base_name.."_Normal_topline_fon",		line_length.t+fon_delta*2,	2*dash_h+gap_h+fon_delta*2,	gap_h-fon_delta*2,	line_width+fon_delta*2, fon_pos.top,	90,	PH_Normal.name, nil, fon_mat)
	addFatDashedLine(base_name.."_Normal_bottomline_fon",	line_length.b+fon_delta*2,	2*dash_h+gap_h+fon_delta*2,	gap_h-fon_delta*2,	line_width+fon_delta*2, fon_pos.bottom,	90,	PH_Normal.name, nil, fon_mat)

	addFatLine(base_name.."_Normal_Art_centerline_fon", 		line_length.c+fon_delta*2, line_width+fon_delta*2, 															fon_pos.center_d, 	0,	PH_Normal_Articulating.name,	nil, fon_mat)
	addFatDashedLine(base_name.."_Normal_Stow_centerline_fon",	line_length.c+fon_delta*2, 	2*dash_v+gap_v+fon_delta*2,	dash_v+2*gap_v-fon_delta*2,	line_width+fon_delta*2, fon_pos.center_d,	0,	PH_Normal_Stowed.name, 			nil, fon_mat)

	-- lines
	addFatDashedLine(base_name.."_Normal_topline",		line_length.t, 2*dash_h+gap_h,	gap_h,	line_width, line_pos.top,		90,	PH_Normal.name, nil, line_mat)
	addFatDashedLine(base_name.."_Normal_bottomline",	line_length.b, 2*dash_h+gap_h ,	gap_h,	line_width, line_pos.bottom,	90,	PH_Normal.name, nil, line_mat)

	addFatLine(base_name.."_Normal_Art_centerline", 		line_length.c, line_width, 									line_pos.center_d,	0,	PH_Normal_Articulating.name,	nil, line_mat)
	addFatDashedLine(base_name.."_Normal_Stow_centerline",	line_length.c, 	2*dash_v+gap_v,	dash_v+2*gap_v,	line_width, line_pos.center_d,	0,	PH_Normal_Stowed.name,			nil, line_mat)

end
-------------------------------------------------------------------------------------------
function buildEllipseVerts(pos, r_x, r_y, N)
	local n = N or 72
	local delta =  2*math.pi/n
	local verts = {}

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