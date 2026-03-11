dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

--local inch_to_meter		= 0.0254
local pixel_size		= pix_to_inch * InToMeter

local tex_size			= 512
--scale_coeff = 1
-----------------------------------------------
heading_scale_y = 350
barometric_or_inertial_altitude_y = heading_scale_y
barometric_or_inertial_altitude_x = 360

vertical_scale_x = 450
vertical_scale_h = 520

navigation_data_x = -490
navigation_data_y = -250

scale_coeff				= display_size_pix/720 -- TEDAC -> MPD Video Page 

-------------------------------------------
SkidSlipIndTop = 	-350 -- ref for sizing Pitch Ladder Scale
PitchLadder_h = -2 * SkidSlipIndTop 
-------------------------------------------
FOR_y			= -display_size_pix_05
FOR_w			= 220
FOR_h			= FOR_w	/2.4
FOR_w05			= FOR_w / 2
FOR_h05			= FOR_h / 2
FOR_margin_bottom	= 35

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
-----------------------------------------------

-- fonts properties:
-- chars 30 x 19 pix
local font_char_tex_size	= 64
local font_char_tex_h		= 50
local font_char_tex_w		= 34
local font_char_size_h		= 25*scale_coeff
local font_char_size_w		= 17*scale_coeff
local font_char_space		= 0*scale_coeff
local font_line_space		= 0*scale_coeff
local font_char_scale_h		= 1 / (font_char_tex_h/font_char_tex_size)
local font_char_scale_w		= 1 / (font_char_tex_w/font_char_tex_size)
local t_font_char_h			= pixel_size * font_char_size_h * font_char_scale_h
local t_font_char_w			= pixel_size * font_char_size_w * font_char_scale_w
local t_font_char_space		= pixel_size * font_char_space
local t_font_line_space		= pixel_size * font_line_space

local t_font_char_dx		= pixel_size * 0
local t_font_char_dy		= font_char_size_h * font_char_scale_h * ((font_char_tex_size - font_char_tex_h)/font_char_tex_size)

local font_stringdefs = { t_font_char_h, t_font_char_w, t_font_char_space, t_font_line_space - pixel_size * t_font_char_dy }

-- chars 24 x 36 pix
local font_big_char_tex_size	= 64
local font_big_char_tex_h		= 58
local font_big_char_tex_w		= 40
local font_big_char_size_h		= 29*scale_coeff
local font_big_char_size_w		= 20*scale_coeff
local font_big_char_space		= 1*scale_coeff
local font_big_line_space		= 1*scale_coeff
local font_big_char_scale_h		= 1 / (font_big_char_tex_h/font_big_char_tex_size)
local font_big_char_scale_w		= 1 / (font_big_char_tex_w/font_big_char_tex_size)
local t_font_big_char_h			= pixel_size * font_big_char_size_h * font_big_char_scale_h
local t_font_big_char_w			= pixel_size * font_big_char_size_w * font_big_char_scale_w
local t_font_big_char_space		= pixel_size * font_big_char_space
local t_font_big_line_space		= pixel_size * font_big_line_space

local t_font_big_char_dx		= pixel_size * 0
local t_font_big_char_dy		= font_big_char_size_h * font_big_char_scale_h * ((font_big_char_tex_size - font_big_char_tex_h)/font_big_char_tex_size)

local font_big_stringdefs = { t_font_big_char_h, t_font_big_char_w, t_font_big_char_space, t_font_big_line_space - pixel_size * t_font_big_char_dy }


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

function addText(name, text, pos, align, parent, controllers, formats, material)

	local dy = t_font_char_dy
	if align == "LeftBottom" or align == "CenterBottom" or align == "RightBottom" then
		dy = 0
	end

	local elem			= CreateElement "ceStringPoly"
	setCommonStringProperties(elem, name, text, align, parent, controllers, formats)
	elem.init_pos		= {pos[1] + t_font_char_dx, pos[2] + dy, 0}
	elem.stringdefs		= font_stringdefs
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

function addBigText(name, text, pos, align, parent, controllers, formats, material)
	local mat = material or "VIDEO_BIG"
	local dy = t_font_big_char_dy
	if align == "LeftBottom" or align == "CenterBottom" or align == "RightBottom" then
		dy = 0
	end

	local elem			= CreateElement "ceStringPoly"
	setCommonStringProperties(elem, name, text, align, parent, controllers, formats)
	elem.material		= fontPrefix..mat
	elem.init_pos		= {pos[1] + t_font_big_char_dx, pos[2] + dy, 0}
	elem.stringdefs		= font_big_stringdefs

	local back_elem = Copy(elem)
	back_elem.name		= name.."_back"
	back_elem.material	= fontPrefix.."VIDEO_BIG_BLACK"

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
--	local elem				= CreateElement "ceSMultiLine"
	elem.name				= name
	elem.material			= material
	elem.width				= width05
	elem.vertices			= verts
	elem.tex_params			= {{0.0,151.0/tex_size},{1.0,151.0/tex_size}, {1.0/display_size_pix, 2 * (tex_line_width / width)/display_size_pix}}

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

function addLine(name, verts, width, parent, controllers, material)
local mat = material or IND_MPD_VIDEO_SYMBOLOGY
	local back_line = addSimpleLine(name.."_back", verts, width + 2, IND_MPD_VIDEO_SYMBOLOGY_BLACK, parent, controllers)
	local line = addSimpleLine(name, verts, width, mat , parent, controllers)
	return {back_line, line}
end

function addLineBrown(name, verts, width, parent, controllers)
	local back_line = addSimpleLine(name.."_back", verts, width + 2, IND_MPD_VIDEO_SYMBOLOGY_BLACK, parent, controllers)
	local line = addSimpleLine(name, verts, width, IND_MPD_VIDEO_SYMBOLOGY_BROWN, parent, controllers)
	return {back_line, line}
end
function addLineBlue(name, verts, width, parent, controllers)
	local back_line = addSimpleLine(name.."_back", verts, width + 2, IND_MPD_VIDEO_SYMBOLOGY_BLACK, parent, controllers)
	local line = addSimpleLine(name, verts, width, IND_MPD_VIDEO_SYMBOLOGY_BLUE, parent, controllers)
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
	local back_arc = addSimpleLine(name.."_back", verts, line_width + 2, IND_MPD_VIDEO_SYMBOLOGY_BLACK, parent, controllers)
	local arc = addSimpleLine(name, verts, line_width, IND_MPD_VIDEO_SYMBOLOGY, parent, controllers)
	return {back_arc, arc}
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

	local back_box = addSimpleLine(name.."_back", verts, line_width + 2, IND_MPD_VIDEO_SYMBOLOGY_BLACK, PH.name, nil)
	local box = addSimpleLine(name, verts, line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, controllers)
end

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

function buildBorderedSymbol(name, size, align, tex_pos, pos, parent, controllers)
	local elem = buildSymbol(name, IND_MPD_VIDEO_SYMBOLOGY, size, pos, align, tex_pos, 2/scale_coeff, parent, controllers)
	
	local back_elem = Copy(elem)
	back_elem.material = IND_MPD_VIDEO_SYMBOLOGY_BLACK

	addElem(back_elem)
	addElem(elem)
	return {back_elem, elem}
end

local function buildBorderedSymbolBlue(name, size, align, tex_pos, pos, parent, controllers)
	local elem = buildSymbol(name, IND_MPD_VIDEO_SYMBOLOGY_BLUE, size, pos, align, tex_pos, 2/scale_coeff, parent, controllers)
	
	local back_elem = Copy(elem)
	back_elem.material = IND_MPD_VIDEO_SYMBOLOGY_BLACK

	addElem(back_elem)
	addElem(elem)
	return {back_elem, elem}
end

local function buildBorderedSymbolBrown(name, size, align, tex_pos, pos, parent, controllers)
	local elem = buildSymbol(name, IND_MPD_VIDEO_SYMBOLOGY_BROWN, size, pos, align, tex_pos, 2/scale_coeff, parent, controllers)
	
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
	local elems = buildBorderedSymbol(name, {25, 25}, "CenterCenter", {53,282}, pos, parent, controllers)
	return elems
end

function addSkidSlipBallBlue(name, pos, parent, controllers)
	local elems = buildBorderedSymbolBlue(name, {25, 25}, "CenterCenter", {53,282}, pos, parent, controllers)
	return elems
end

function addSkidSlipBallBrown(name, pos, parent, controllers)
	local elems = buildBorderedSymbolBrown(name, {25, 25}, "CenterCenter", {53,282}, pos, parent, controllers)
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


function addCuedLosReticle(name, pos, parent, controllers )
	local size05 	= PitchLadder_h/16 
	local s05 		= 10 -- space
	local w05_len	= (size05 - s05)/2.9
	local w05_sp	= 0.9 * w05_len
	local h05_len	= w05_len
	local h05_sp	= w05_sp
	local w 		= 10
	local mat		= IND_MPD_VIDEO_SYMBOLOGY
	local sh 		= 2

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local s05_1 = s05 + h05_len + h05_sp

	local lu_1  = addSimpleLine(name.."_up_1",		{{   sh, s05}, 		{  sh, s05 + h05_len}},		w, mat, PH.name)
	local lu_2  = addSimpleLine(name.."_up_2",		{{  -sh, s05},		{ -sh, s05 + h05_len}},		w, mat, PH.name)
	local lu_11 = addSimpleLine(name.."_up_11",		{{   sh, s05_1},	{  sh, s05_1 + h05_len}},	w, mat, PH.name)
	local lu_21 = addSimpleLine(name.."_up_21",		{{  -sh, s05_1},	{ -sh, s05_1 + h05_len}},	w, mat, PH.name)
																							    
	local ld_1  = addSimpleLine(name.."_down_1",	{{   sh, -s05}, 	{  sh, -s05-h05_len}},		w, mat, PH.name)
	local ld_2  = addSimpleLine(name.."_down_2",	{{  -sh, -s05},		{ -sh, -s05-h05_len}},		w, mat, PH.name)
	local ld_11 = addSimpleLine(name.."_down_11",	{{   sh, -s05_1},	{  sh, -s05_1-h05_len}},	w, mat, PH.name)
	local ld_21 = addSimpleLine(name.."_down_21",	{{  -sh, -s05_1},	{ -sh, -s05_1-h05_len}},	w, mat, PH.name)
	
	s05_1 = s05 + w05_len + w05_sp
	
	local lr_1  = addSimpleLine(name.."_right_1",	{{ s05,   sh}, { s05 + w05_len,   sh}}, w, mat, PH.name)
	local lr_2  = addSimpleLine(name.."_right_2",	{{ s05,  -sh}, { s05 + w05_len,  -sh}}, w, mat, PH.name)
	local lr_11 = addSimpleLine(name.."_right_11",	{{ s05_1, sh}, { s05_1 + w05_len, sh}}, w, mat, PH.name)
	local lr_21 = addSimpleLine(name.."_right_21",	{{ s05_1,-sh}, { s05_1 + w05_len,-sh}}, w, mat, PH.name)
																							
	local ll_1  = addSimpleLine(name.."_left_1",	{{ -s05,   sh}, { -s05-w05_len,   sh}}, w, mat, PH.name)
	local ll_2  = addSimpleLine(name.."_left_2",	{{ -s05,  -sh}, { -s05-w05_len,  -sh}}, w, mat, PH.name)
	local ll_11 = addSimpleLine(name.."_left_11",	{{ -s05_1, sh}, { -s05_1-w05_len, sh}}, w, mat, PH.name)
	local ll_21 = addSimpleLine(name.."_left_21",	{{ -s05_1,-sh}, { -s05_1-w05_len,-sh}}, w, mat, PH.name)
	
	return { PH, lu_1, lu_2, lu_11, lu_21, ld_1,ld_2,ld_11,ld_21, lr_1, lr_2, lr_11, lr_21, ll_1,ll_2,ll_11,ll_21 }
end


function addHeadTracker(name, pos, parent, controllers)
	local size			= 84*scale_coeff
	local s05			= size / 2
	local l				= s05 / 3
	local line_width	= 4

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	-- corners
	local c_u = addSimpleLine(name.."_up",		{{      -l, s05 - l}, {   0, s05}, {       l, s05 - l}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	local c_d = addSimpleLine(name.."_down",	{{      -l,-s05 + l}, {   0,-s05}, {       l,-s05 + l}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	local c_l = addSimpleLine(name.."_left",	{{-s05 + l,       l}, {-s05,   0}, {-s05 + l,      -l}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	local c_r = addSimpleLine(name.."_right",	{{ s05 - l,       l}, { s05,   0}, { s05 - l,      -l}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)

	return {
		PH,
		c_u, c_d, c_l, c_r,
	}
end

 
function add_LOS_Reticle(name, pos, parent, controllers, width  )
	local size = PitchLadder_h/16 
	width = width or 4
	local s05 = 10	-- space
	local w05 = size - s05
	local h05 = w05 
	
	local s = w05 + 2*s05 
--	function addLine(name, verts, width, parent, controllers)
	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)
	local lu = addLine(name.."_up",	{{   0, s05}, {   0, s05+h05}}, width, PH.name) 
	local ld = addLine(name.."_dn",	{{   0,-s05}, {   0, -s05-h05}},width, PH.name) 
	local ll = addLine(name.."_l",	{{-s05,   0}, {-s05-w05,   0}}, width, PH.name) 
	local lr = addLine(name.."_r",	{{ s05,   0}, { s05+w05,   0}}, width, PH.name) 
	return { PH, lu, ld, ll, lr}
end

--
---- TADS LOS Reticle defs
--local TADS_LOS_Reticle_w05 = 100*scale_coeff
--local TADS_LOS_Reticle_h05 = 75*scale_coeff
--
--function add_TADS_LOS_Reticle(name, pos, parent)
--	local w05 = TADS_LOS_Reticle_w05
--	local h05 = TADS_LOS_Reticle_h05
--	local s_w05 = 14*scale_coeff	-- space
--	local s_h05 = 10*scale_coeff	-- space
--
--	local PH = addPlaceholder(name.."_PH", pos, parent)
--
--	local lu = addLine(name.."_up",		{{     0, s_h05}, {   0, h05}},	4, PH.name, nil)
--	local ld = addLine(name.."_down",	{{     0,-s_h05}, {   0,-h05}},	4, PH.name, nil)
--	local ll = addLine(name.."_left",	{{-s_w05,     0}, {-w05,   0}},	4, PH.name, nil)
--	local lr = addLine(name.."_right",	{{ s_w05,     0}, { w05,   0}},	4, PH.name, nil)
--	return {
--		PH,
--		lu[1], lu[2],
--		ld[1], ld[2],
--		ll[1], ll[2],
--		lr[1], lr[2],
--	}
--end
--
--function add_LMC_On(name, pos, parent, controllers)
--	local w05 = TADS_LOS_Reticle_w05 + 5
--	local h05 = TADS_LOS_Reticle_h05 + 5
--	local len = 10
--	local l05 = len / 2
--
--	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)
--
--	local l1 = addLine(name.."_up",		{{-l05, h05}, { l05, h05}}, 4, PH.name, nil)
--	local l2 = addLine(name.."_down",	{{-l05,-h05}, { l05,-h05}}, 4, PH.name, nil)
--	local l3 = addLine(name.."_left",	{{-w05,-l05}, {-w05, l05}}, 4, PH.name, nil)
--	local l4 = addLine(name.."_right",	{{ w05,-l05}, { w05, l05}}, 4, PH.name, nil)
--	return {
--		PH,
--
--		l1[1], l1[2],
--		l2[1], l2[2],
--		l3[1], l3[2],
--		l4[1], l4[2],
--	}
--end
--
--function add_LaserFiringIndicator(name, pos, parent, controllers)
--	local len_gain = 4 / 5
--	local w05 = TADS_LOS_Reticle_h05 * len_gain
--	local h05 = TADS_LOS_Reticle_h05 * len_gain
--	local s_w05 = w05 / 2
--	local s_h05 = s_w05
--
--	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)
--
--	local l1 = addLine(name.."_up_left",	{{-s_w05, s_h05}, {-w05, h05}},	4, PH.name, nil)
--	local l2 = addLine(name.."_up_right",	{{ s_w05, s_h05}, { w05, h05}},	4, PH.name, nil)
--	local l3 = addLine(name.."_down_left",	{{-s_w05,-s_h05}, {-w05,-h05}},	4, PH.name, nil)
--	local l4 = addLine(name.."_down_right",	{{ s_w05,-s_h05}, { w05,-h05}},	4, PH.name, nil)
--	return {
--		PH,
--		l1[1], l1[2],
--		l2[1], l2[2],
--		l3[1], l3[2],
--		l4[1], l4[2],
--	}
--end
--
-- TODO: The size of the LOBL constraint box represents +/- 20 deg constraints limit
-- and the LOAL constraint box represents a +/- 7.5 deg constraints limit from the ADL.
local LOAL_size	= 38*scale_coeff
local LOBL_size = 158*scale_coeff

function add_LOAL_out_of_constraints_missile_box(name, pos, parent, controllers)
	local size		= LOAL_size
	local s05		= size / 2
	local dash		= 8*scale_coeff
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
		line_width + 2, PH.name, nil, IND_MPD_VIDEO_SYMBOLOGY_BLACK)
	addElem(back_line)
	-- corners
	local c_ul = addSimpleLine(name.."_up_left",	{{-s05, s05 - dash}, {-s05, s05}, {-s05 + dash, s05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	local c_ur = addSimpleLine(name.."_up_right",	{{ s05, s05 - dash}, { s05, s05}, { s05 - dash, s05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	local c_dl = addSimpleLine(name.."_down_left",	{{-s05,-s05 + dash}, {-s05,-s05}, {-s05 + dash,-s05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	local c_dr = addSimpleLine(name.."_down_right",	{{ s05,-s05 + dash}, { s05,-s05}, { s05 - dash,-s05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	-- centers
	local c_u = addSimpleLine(name.."_up",		{{-d05, s05}, { d05, s05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	local c_d = addSimpleLine(name.."_down",	{{-d05,-s05}, { d05,-s05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	local c_l = addSimpleLine(name.."_left",	{{-s05,-d05}, {-s05, d05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	local c_r = addSimpleLine(name.."_right",	{{ s05,-d05}, { s05, d05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)

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
	local dash			= 11*scale_coeff
	local d05			= dash / 2
	local gap			= 10*scale_coeff
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
		line_width + 2, PH.name, nil, IND_MPD_VIDEO_SYMBOLOGY_BLACK)
	addElem(back_line)
	-- corners
	local c_ul = addSimpleLine(name.."_corner_up_left",		{{-s05, s05 - dash}, {-s05, s05}, {-s05 + dash, s05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	local c_ur = addSimpleLine(name.."_corner_up_right",	{{ s05, s05 - dash}, { s05, s05}, { s05 - dash, s05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	local c_dl = addSimpleLine(name.."_corner_down_left",	{{-s05,-s05 + dash}, {-s05,-s05}, {-s05 + dash,-s05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	local c_dr = addSimpleLine(name.."_corner_down_right",	{{ s05,-s05 + dash}, { s05,-s05}, { s05 - dash,-s05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
	-- 
	local l0 = -s05 + dash + gap
	local dashes = {}

	for i = 0,5 do
		local lb = l0 + period * i
		local le = lb + dash
		local d_u = addSimpleLine(name.."_up"..i,		{{ lb, s05}, { le, s05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
		local d_d = addSimpleLine(name.."_down"..i,		{{ lb,-s05}, { le,-s05}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
		local d_l = addSimpleLine(name.."_left"..i,		{{-s05, lb}, {-s05, le}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
		local d_r = addSimpleLine(name.."_right"..i,	{{ s05, lb}, { s05, le}},	line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name, nil)
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
end

function addRateOfClimb(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {30, 25}, "CenterCenter", {270,174}, pos, parent, controllers)
--	elems[1].init_rot = {-90,0,0}
--	elems[2].init_rot = {-90,0,0}
	return elems
end

function addAltitudeHold(name, pos, parent, controllers)
	local x = pos[1]
	local y = pos[2]

	local w = 28
	local w05 = 11
	local h = 31
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
	local elems = addLine(name, verts, 5, parent, controllers)
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
	local r				= 11*scale_coeff
	local line_width	= 4

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local c_verts = buildArcVerts({0,0}, r, 0, 360, 16)
	local up_verts = {{ 0,r}, {   0,r*2}}
	local lh_verts = {{-r,0}, {-r*2,  0}}
	local rh_verts = {{ r,0}, { r*2,  0}}

	-- back
	local circle_back	= addSimpleLine(name.."_circle_back",	c_verts,  line_width + 2, IND_MPD_VIDEO_SYMBOLOGY_BLACK, PH.name)
	local notch_up_back	= addSimpleLine(name.."_up_back",		up_verts, line_width + 2, IND_MPD_VIDEO_SYMBOLOGY_BLACK, PH.name)
	local notch_lh_back	= addSimpleLine(name.."_lh_back",		lh_verts, line_width + 2, IND_MPD_VIDEO_SYMBOLOGY_BLACK, PH.name)
	local notch_rh_back	= addSimpleLine(name.."_rh_back",		rh_verts, line_width + 2, IND_MPD_VIDEO_SYMBOLOGY_BLACK, PH.name)
	-- front
	local circle		= addSimpleLine(name.."_circle",	c_verts,  line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name)
	local notch_up		= addSimpleLine(name.."_up",		up_verts, line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name)
	local notch_lh		= addSimpleLine(name.."_lh",		lh_verts, line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name)
	local notch_rh		= addSimpleLine(name.."_rh",		rh_verts, line_width, IND_MPD_VIDEO_SYMBOLOGY, PH.name)

	return {
		PH,
		circle_back,	notch_up_back,	notch_lh_back,	notch_rh_back,
		circle,			notch_up,		notch_lh,		notch_rh,
	}
end

function addNavFlyToCUE(name, pos, parent, controllers)
	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local point = addPoint(name.."_point", {0,0}, PH.name)

	local s = 28*scale_coeff
	local f = 15*scale_coeff
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

function addHorizonLine(name, pos, parent, controllers)
	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local period = 80
	local dash = 40

	local elems = { PH }

	for i = 1,4,1 do
		local ps = period * i
		local pe = ps + dash

		local dl = addLine(name.."_dash_lhs_"..i, {{-ps,0}, {-pe,0}}, 4, PH.name, nil, IND_MPD_VIDEO_SYMBOLOGY_BROWN)
		local dr = addLine(name.."_dash_rhs_"..i, {{ ps,0}, { pe,0}}, 4, PH.name, nil, IND_MPD_VIDEO_SYMBOLOGY_BROWN)

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

	local hor_line_len_05 = 384
	local digits = { 10, 20, 30, 45, 60 }
	local hor_line = addLineBrown(name.."HorizonLine", {{-hor_line_len_05,0},{hor_line_len_05,0}}, 4, PH.name)
	join(elems, hor_line)

	local step_y	= PitchLadder_h/4
	local dash		= 18
	local length	= dash * 5
	local width		= hor_line_len_05 / 3
	local gap		= width - length
	local dgap		= 18

	local y		= 0.0
	local xs	= 0.0
	local xe	= 0.0
	local xm	= 0.0
	local dym	= -5
	
	for i = 1,5 do
		local dy = (i < 4) and  step_y or 1.5 * step_y
		y = y + dy 
		xs = gap + dgap * (i - 1)
		xe = xs + length
		xm = xe + 7
		--function addText(name, text, pos, align, parent, controllers, formats, material)
		-- sky portion
		local sky_lhs = addLineBlue(name.."_SkyLhs_"..i, {{-xs,y},{-xe,y},{-xe,y-dash}}, 4, PH.name)
		local sky_rhs = addLineBlue(name.."_SkyRhs_"..i, {{ xs,y},{ xe,y},{ xe,y-dash}}, 4, PH.name)
		join(elems, sky_lhs)
		join(elems, sky_rhs)
		local sky_mark_lhs = addText(name.."_SkyMarkLhs_"..i, digits[i], {-xm,y+dym}, "RightCenter", PH.name, nil, nil, "VIDEO_BLUE")
		local sky_mark_rhs = addText(name.."_SkyMarkRhs_"..i, digits[i], { xm,y+dym}, "LeftCenter",  PH.name, nil, nil, "VIDEO_BLUE")
		join(elems, sky_mark_lhs)
		join(elems, sky_mark_rhs)
		-- ground portion
		local ground_lhs_1 = addLineBrown(name.."_GroundLhs_1_"..i, {{-xs-0*dash,-y},{-xs-1*dash,-y}}, 4, PH.name)
		local ground_lhs_2 = addLineBrown(name.."_GroundLhs_2_"..i, {{-xs-2*dash,-y},{-xs-3*dash,-y}}, 4, PH.name)
		local ground_lhs_c = addLineBrown(name.."_GroundLhs_c_"..i, {{-xs-4*dash,-y},{-xs-5*dash,-y},{-xe,-y+dash}}, 4, PH.name)
		join(elems, ground_lhs_1)
		join(elems, ground_lhs_2)
		join(elems, ground_lhs_c)
		local ground_rhs_1 = addLineBrown(name.."_GroundRhs_1_"..i, {{xs+0*dash,-y},{xs+1*dash,-y}}, 4, PH.name)
		local ground_rhs_2 = addLineBrown(name.."_GroundRhs_2_"..i, {{xs+2*dash,-y},{xs+3*dash,-y}}, 4, PH.name)
		local ground_rhs_c = addLineBrown(name.."_GroundRhs_c_"..i, {{xs+4*dash,-y},{xs+5*dash,-y},{ xe,-y+dash}}, 4, PH.name)
		join(elems, ground_rhs_1)
		join(elems, ground_rhs_2)
		join(elems, ground_rhs_c)
		local ground_mark_lhs = addText(name.."_GroundMarkLhs_"..i, digits[i], {-xm,-y+dym}, "RightCenter", PH.name, nil, nil, "VIDEO_BROWN")
		local ground_mark_rhs = addText(name.."_GroundMarkRhs_"..i, digits[i], { xm,-y+dym}, "LeftCenter", PH.name, nil, nil, "VIDEO_BROWN")
		join(elems, ground_mark_lhs)
		join(elems, ground_mark_rhs)
	end
	--CLIMB&DIV 
	y = y + 3.0 * step_y
	local climb_ball = addSkidSlipBallBlue("climb_ball", {0, y}, PH.name )
	local climb_lhs	=  addLineBlue("climb_Lhs", 	{{-xs*0.66,y+xs/2},{-xs*0.66,y-xs/2}}, 4, PH.name)
	local climb_rhs	=  addLineBlue("climb_Rhs", 	{{ xs*0.66,y+xs/2},{ xs*0.66,y-xs/2}}, 4, PH.name)
	
	local climb_PH = addRotPlaceholder(name.."climb_PH", {0, y}, 180.0,  PH.name)
	local climb_up = addText("climb_up", "CLIMB", {0,y+xs*0.5}, "CenterBottom", PH.name, nil, nil, "VIDEO_BLUE")
	local climb_dn = addText("climb_dn", "CLIMB", {0, xs*0.5}, "CenterBottom", climb_PH.name, nil, nil, "VIDEO_BLUE")
	
	join(elems, climb_ball)
	join(elems, climb_lhs)
	join(elems, climb_rhs)
	join(elems, climb_up)
	join(elems, climb_dn)
--	join(elems, climb_PH)
	
	local div_ball = addSkidSlipBallBrown("div_ball", {0, -y}, PH.name )
	local div_lhs	=  addLineBrown("div_Lhs", 	{{-xs*0.66,-y+xs/2},{-xs*0.66,-y-xs/2}}, 4, PH.name)
	local div_rhs	=  addLineBrown("div_Rhs", 	{{ xs*0.66,-y+xs/2},{ xs*0.66,-y-xs/2}}, 4, PH.name)
	
	local div_PH = addRotPlaceholder(name.."div_PH", {0, -y}, 180.0,  PH.name)
	local div_up = addText("div_up", "DIVE", {0,-y+xs*0.5}, "CenterBottom", PH.name, nil, nil, "VIDEO_BROWN")
	local div_dn = addText("div_dn", "DIVE", {0, xs*0.5}, "CenterBottom", div_PH.name, nil, nil, "VIDEO_BROWN")
	join(elems, div_ball)
	join(elems, div_lhs)
	join(elems, div_rhs)
	join(elems, div_up)
	join(elems, div_dn)
--	join(elems, div_PH)
	return elems
end

function addBankAnglePointer(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {20,20}, "CenterCenter", {177,178}, pos, parent, controllers)
	return elems
end


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
	local line_mat		= IND_MPD_VIDEO_SYMBOLOGY
	local fon_mat		= IND_MPD_VIDEO_SYMBOLOGY_BLACK
	
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
