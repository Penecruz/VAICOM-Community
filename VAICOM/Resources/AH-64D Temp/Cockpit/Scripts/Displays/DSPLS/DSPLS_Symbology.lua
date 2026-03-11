dofile(LockOn_Options.script_path.."symbology_defs.lua")
dofile(LockOn_Options.script_path.."Displays/Common/Units.lua")

DItoScreenUnits = DItoMil(1)
ScreenUnitsToDI = 1 / DItoScreenUnits

SetScale(MILLYRADIANS)
SetCustomScale(GetScale() * DItoScreenUnits )

display_size_pix		= DegToDI(30.0) --1540
display_size_pix_05		= display_size_pix / 2
scale_coeff				= 2.22979167
local tex_size			= 512
-------------------------------------------
stroke_thickness = 4.0
stroke_fuzziness = 1.0
stroke_font     	= "font_stroke_HMD"
stroke_material 	= "HMD_GREEN"
default_material	= "HMD_GREEN"
current_font		= "font_stroke_HMD"
additive_alpha  	= true
collimated      	= true
-------------------------------------------
THICKNESS_NORM = 3.0
FUZZINESS_NORM = 3.0

THICKNESS_NARROW = 2.0
FUZZINESS_NARROW = 2.0

THICKNESS_WIDE = 5.0
FUZZINESS_WIDE = 2.0
-------------------------------------------
SkidSlipIndTop		= -550 -- ref for sizing Pitch Ladder Scale
PitchLadder_h		= -2 * SkidSlipIndTop
SkidSlipInd_h		= 60
SkidSlipIndBottom	= SkidSlipIndTop - SkidSlipInd_h

-------------------------------------------
DEFAULT_LEVEL	= 8

function setElemsClipLevel(obj, level)
	if type(obj) == "table" then
		for i, o in pairs(obj) do
			setClipLevel(o, level)
		end
	else
		setClipLevel(obj, level)
	end
end

local function addElem(elem)
	elem.additive_alpha	= additive_alpha
	Add(elem)
end


-- Video Area
video_area_w			= display_size_pix
video_area_h			= video_area_w
video_area_w_05			= video_area_w / 2
video_area_h_05			= video_area_h /2
c_scope_w				= 800
c_scope_h				= 800

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

function AddBackground(name, invisible, parent)
	local element			= CreateElement "ceMeshPoly"
	element.name			= name
	element.primitivetype	= "triangles"
	element.material		= "HMD_BLACK"
	element.vertices		= buildBoxVerts(video_area_w, video_area_h, "CenterCenter")
	element.indices			= default_box_indices
	element.blend_mode		= blend_mode.IBM_REGULAR
	element.change_opacity	= false
	element.init_pos		= {0, 0, 0}

	if invisible == true then
		element.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
		element.level			= DEFAULT_LEVEL
		setAsInvisibleMask(element) -- changes material
	end
	if parent ~= nil then
		element.parent_element	= parent
	end
	Add(element)
	return element
end

-- dot fonts ( obsolete )
-- fonts properties:
-- chars 17 x 25 pix
local font_char_tex_size	= 64
local font_char_tex_h		= 50
local font_char_tex_w		= 34
local font_char_size_h		= 25*scale_coeff
local font_char_size_w		= 17*scale_coeff
local font_char_space		= 0*scale_coeff
local font_line_space		= 0*scale_coeff
local font_char_scale_h		= 1 / (font_char_tex_h/font_char_tex_size)
local font_char_scale_w		= 1 / (font_char_tex_w/font_char_tex_size)

local t_font_char_h			= GetScale() * font_char_size_h * font_char_scale_h
local t_font_char_w			= GetScale() * font_char_size_w * font_char_scale_w
local t_font_char_space		= GetScale() * font_char_space
local t_font_line_space		= GetScale() * font_line_space

local t_font_char_dx		=  0
local t_font_char_dy		=  GetScale() * font_char_size_h * font_char_scale_h * ((font_char_tex_size - font_char_tex_h)/font_char_tex_size)
local font_stringdefs = { t_font_char_h, t_font_char_w, t_font_char_space, t_font_line_space - t_font_char_dy }

function getTextWidth(symbol_num)
	return font_char_size_w * symbol_num + font_char_space * (symbol_num - 1)
end

-- chars 20 x 29 pix
local font_big_char_tex_size	= 64
local font_big_char_tex_h		= 58
local font_big_char_tex_w		= 40
local font_big_char_size_h		= 29*scale_coeff
local font_big_char_size_w		= 20*scale_coeff
local font_big_char_space		= 1*scale_coeff
local font_big_line_space		= 1*scale_coeff
local font_big_char_scale_h		= 1 / (font_big_char_tex_h/font_big_char_tex_size)
local font_big_char_scale_w		= 1 / (font_big_char_tex_w/font_big_char_tex_size)

local t_font_big_char_h			= GetScale() * font_big_char_size_h * font_big_char_scale_h
local t_font_big_char_w			= GetScale() * font_big_char_size_w * font_big_char_scale_w
local t_font_big_char_space		= GetScale() * font_big_char_space
local t_font_big_line_space		= GetScale() * font_big_line_space

local t_font_big_char_dx		=  0
local t_font_big_char_dy		= GetScale() * font_big_char_size_h * font_big_char_scale_h * ((font_big_char_tex_size - font_big_char_tex_h)/font_big_char_tex_size)

local font_big_stringdefs = { t_font_big_char_h, t_font_big_char_w, t_font_big_char_space, t_font_big_line_space -  t_font_big_char_dy }
--------------------------------------------------------------------------------------------------------------
-- stroke fonts
------------------------------------------------------------------------------
glyphNominalHeight100 = 20
glyphNominalWidth100  = 25
glyphAspect = glyphNominalWidth100 / glyphNominalHeight100

glyphNominalHeight120 = glyphNominalHeight100 * 1.2
glyphNominalHeight150 = glyphNominalHeight100 * 1.5
glyphNominalHeight200 = glyphNominalHeight100 * 2

glyphNominalWidth120 = roundDI(glyphNominalWidth100 * 1.2)
glyphNominalWidth150 = glyphNominalWidth100 * 1.5
glyphNominalWidth200 = glyphNominalWidth100 * 2

fontScaleY_100 = glyphNominalHeight100 * GetScale()
fontScaleY_120 = glyphNominalHeight120 * GetScale()
fontScaleY_150 = glyphNominalHeight150 * GetScale()
fontScaleY_200 = glyphNominalHeight200 * GetScale()

fontScaleX_100 = glyphNominalWidth100 * GetScale()
fontScaleX_120 = glyphNominalWidth120 * GetScale()
fontScaleX_150 = glyphNominalWidth150 * GetScale()
fontScaleX_200 = glyphNominalWidth200 * GetScale()

fontIntercharDflt100 		= glyphNominalHeight100 * 0.5
fontIntercharDflt120 		= glyphNominalHeight120 * 0.5
fontIntercharDflt150 		= glyphNominalHeight150 * 0.5
fontIntercharDflt200 		= glyphNominalHeight200 * 0.5
fontIntercharDflt120_wide 	= glyphNominalHeight120 * 0.5
fontIntercharDflt150_wide 	= glyphNominalHeight150 * 0.5

fontInterlineDflt100 		= glyphNominalHeight100 * 0.5
fontInterlineDflt120 		= glyphNominalHeight100 * 0.5
fontInterlineDflt150 		= glyphNominalHeight100 * 0.5
fontInterlineDflt200 		= glyphNominalHeight100 * 0.5

STROKE_FNT_DFLT_100 		= 10
STROKE_FNT_DFLT_120 		= 11
STROKE_FNT_DFLT_150 		= 12
STROKE_FNT_DFLT_200 		= 13
STROKE_FNT_DFLT_120_WIDE 	= 14
STROKE_FNT_DFLT_150_WIDE 	= 15

stringdefs[STROKE_FNT_DFLT_100] 		= {fontScaleY_100, fontScaleX_100, fontIntercharDflt100 * GetScale(), fontInterlineDflt100 * GetScale()}
stringdefs[STROKE_FNT_DFLT_120] 		= {fontScaleY_120, fontScaleX_120, fontIntercharDflt120 * GetScale(), fontInterlineDflt120 * GetScale()}
stringdefs[STROKE_FNT_DFLT_150] 		= {fontScaleY_150, fontScaleX_150, fontIntercharDflt150 * GetScale(), fontInterlineDflt150 * GetScale()}
stringdefs[STROKE_FNT_DFLT_200] 		= {fontScaleY_200, fontScaleX_200, fontIntercharDflt200 * GetScale(), fontInterlineDflt200 * GetScale()}
stringdefs[STROKE_FNT_DFLT_120_WIDE] 	= {fontScaleY_120, fontScaleX_120, fontIntercharDflt120_wide * GetScale(), fontInterlineDflt150 * GetScale()}
stringdefs[STROKE_FNT_DFLT_150_WIDE] 	= {fontScaleY_150, fontScaleX_150, fontIntercharDflt150_wide * GetScale(), fontInterlineDflt150 * GetScale()}
--------------------------------------------------------------------------------------------------------------
function resetMask(mask, level, parent)
	local area				= createMask(mask.name.."_Reset", mask.vertices, mask.indices, mask.init_pos, parent, nil, nil)
	area.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
	area.level				= DEFAULT_LEVEL + level
	return area
end

local function join(table, elem)
	table[#table + 1] = elem
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

function addText(name, text, pos, align, parent, controllers, formats)

	local dy = t_font_char_dy
	if align == "LeftBottom" or align == "CenterBottom" or align == "RightBottom" then
		dy = 0
	end

	local elem			= CreateElement "ceStringPoly"
	setCommonStringProperties(elem, name, text, align, parent, controllers, formats)
	elem.material		= "font_HMD"
	elem.init_pos		= {pos[1] + t_font_char_dx, pos[2] + dy, 0}
	elem.stringdefs		= font_stringdefs

	local back_elem = Copy(elem)
	back_elem.name		= name.."_back"
	back_elem.material	= "font_HMD_BLACK"

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
	elem.material		= "font_HMD_big"
	elem.init_pos		= {pos[1] + t_font_big_char_dx, pos[2] + dy, 0}
	elem.stringdefs		= font_big_stringdefs

	local back_elem = Copy(elem)
	back_elem.name		= name.."_back"
	back_elem.material	= "font_HMD_big_BLACK"

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


function buildLine(name, verts, width, parent, controllers, material)
	local width05 = width / 2
	local tex_line_width = 10
	local tex_line_width05 = tex_line_width / 2

	local elem				= CreateElement "ceSimpleLineObject"
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

function addLine(name, verts, width, parent, controllers)
	local line = addSimpleLine(name, verts, width, "HMD_SYMBOLOGY", parent, controllers)
	return line
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
	local verts = {{l, t}, {r, t}, {r, b}, {l, b}, {l, t}}
	local rect = addStrokeLine(name, verts, nil, nil, parent, controllers)
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

function buildArcVertsII(pos, radius, angle_start_deg, angle_end_deg, segments_count)
	local count_default		= 24	-- for the whole circle
	local d_angle_default	= 360 / count_default
	local arc				= angle_end_deg - angle_start_deg
	local count				= segments_count or (math.floor(arc / d_angle_default) + 1)	-- segments count
	local v_count			= count 	-- verts count

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

--function buildArcVerts2points(radius, start_point, end_point, segments_count)
--	local angle_start_deg	=  0.0
--	local angle_stop_deg	=  0.0
--	local x					=  0.0
--	local y					=  0.0
--
--	if start_point[2] == end_point[2] then -- hor
--
--		angle_start_deg		=  math.deg( math.asin( start_point[1]/radius ))
--		angle_stop_deg		=  math.deg( math.asin( end_point[1]/radius ))
--		y = math.sqrt( radius*radius - start_point[1]*start_point[1])
--		if start_point[2] > 0 then
--			y = start_point[2] - y
--			angle_start_deg	 = angle_start_deg + 90.0
--			angle_start_deg	 = angle_start_deg + 90.0
--		else
--			y = start_point[2] + y
--			angle_start_deg	 = angle_start_deg + 270.0
--			angle_start_deg	 = angle_start_deg + 270.0
--		end
--
--	else
--   	if start_point[1] == end_point[1] then -- vert
--    		angle_start_deg		=  math.deg( math.asin( start_point[2]/radius ))
--    		angle_stop_deg		=  math.deg( math.asin( end_point[2]/radius ))
--
--    		x = math.sqrt( radius*radius - start_point[2]*start_point[2])
--    		if start_point[1] > 0 then
--    			x = start_point[1] - x
--    		else
--    			x = start_point[1] + x
--   			angle_start_deg	 = angle_start_deg + 180.0
--    			angle_start_deg	 = angle_start_deg + 180.0
--   		end
--    	end
--	end
--	return buildArcVerts( {x,y}, radius, angle_start_deg, angle_stop_deg, segments_count)
--end


function addArc(name, pos, radius, angle_start_deg, angle_end_deg, segments_count, line_width, parent, controllers)
	local verts = buildArcVerts(pos, radius, angle_start_deg, angle_end_deg, segments_count)
	local arc = addStrokeLine(name, verts, nil, nil, parent, controllers)
	return arc
end

local function splitVerts(tbl, verts)
	for i,v in pairs(verts) do
		if i ~= 1 then
			tbl[#tbl + 1] = v
		end
	end
end

function avg_filter( s, ww )
	local d = {}
	local awg_x = 0.0
	local awg_y = 0.0
	local t = 0.0
	local i_d = 1
	local w = ww or 10
	local w05 = w/2
	local i = 0
	local j = 0


	for i_s = 1, #s + w do
		awg_x = 0.0
		awg_y = 0.0

		for i_w = 0, w-1 do
			i = (i_w + i_s)
			i =  i <= #s and i or (i - #s + 1)
			awg_x = awg_x + s[i][1]
			awg_y = awg_y + s[i][2]
		end

		d[#d + 1] = { awg_x/w, awg_y/w}
	end
	return d
end

	local function splitInvertXYVerts(tbl, vrts)
	for i = 1, #vrts do
		tbl[#tbl + 1] = {-vrts[i][1], -vrts[i][2]}
		end
	end

function buildHMD_maskVerts( count_)
	local w05 = RadToDI(math.tan(math.rad(20)))
	local h05 = RadToDI(math.tan(math.rad(15)))

	local verts = {}
	local radius = 1.18*w05
	local count =  count_ or 100

	local angle = math.rad(0)
	local delta = math.rad(360 / count)

	for i = 0,count do
		local x = radius * math.cos(angle)
		local y = radius * math.sin(angle)
		if x > w05 then
			x = w05
		end
		if x < -w05 then
			x = -w05
		end

		if y > h05 then
			y = h05
		end
		if y < -h05 then
			y = -h05
		end

		verts[#verts + 1] = {x, y}
		angle = angle + delta
	end

	verts[#verts + 1] = verts[1]
--	return  verts
	return  avg_filter( verts, 2 )
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

	local box = addStrokeLine(name, verts, nil, nil, PH.name)
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
	local elem = buildSymbol(name, "HMD_SYMBOLOGY", size, pos, align, tex_pos, 2/scale_coeff, parent, controllers)
	addElem(elem)
	return elem
end

function addPointer_FcrCenterlineBearing(name, pos, parent, controllers)
	local elem 	= addStrokeSymbolEx(name, {"stroke_symbols_HMD", "FcrCenterlineBearing"}, "FromSet", pos, 2.0, 2.0, parent, controllers)
	return elem
end

function addStrokePointer_AlternateSensorBearing(name, pos, parent, controllers)
	local elem 	= addStrokeSymbolEx(name, {"stroke_symbols_HMD", "AltBearing"}, "CenterTop", pos, 2.0, 2.0, parent, controllers, 0.9)
	return elem
end

function addStrokePointer_AdfBearing(name, pos, parent, controllers)
	local elem 	= addStrokeSymbolEx(name, {"stroke_symbols_HMD", "AdfBearing"}, "CenterTop", pos, 2.0, 2.0, parent, controllers, 0.9)
	return elem
end

function addStrokePointer_BobUpHeading(name, pos, parent, controllers)
	local elem 	= addStrokeSymbolEx(name, {"stroke_symbols_HMD", "BobUpHeading"}, "CenterTop", pos, 0.5, 0.5, parent, controllers, 0.9)
	return elem
end

function addStrokeAltitudeTape(name, pos, parent, controllers)
	local elem 	= addStrokeSymbolEx(name, {"stroke_symbols_HMD", "AltitudeTape"}, "CenterTop", pos, 0.5, 0.5, parent, controllers)
	return elem
end

function addStrokeDot(name, pos, parent, controllers)
	local elem 	= addStrokeSymbolEx(name, {"stroke_symbols_HMD", "Point-2"}, "FromSet", pos, 5.0, 1.0, parent, controllers)
	return elem
end

function addStrokeSkidSlipBall(name, pos, parent, controllers)
	local elem 	= addStrokeSymbolEx(name, {"stroke_symbols_HMD", "SkidSlipBall"}, "FromSet", pos, 1.4, 1.4, parent, controllers, 1.35)
	return elem
end

function addStrokePoint(name, pos, parent, controllers)
	local elem 	= addStrokeSymbolEx(name, {"stroke_symbols_HMD", "Point"}, "FromSet", pos, 0.5, 0.5, parent, controllers)
	return elem
end

function addStrokeRateOfClimb(name, pos, parent, controllers)
	local elem 	= addStrokeSymbolEx(name, {"stroke_symbols_HMD", "AltBearing"}, "FromSet", pos, 2.0, 2.0, parent, controllers)
	elem.init_rot = {-90,0,0}
	return elem
end

function addStrokeBankAngle(name, pos, parent, controllers)
	local elem 	= addStrokeSymbolEx(name, {"stroke_symbols_HMD", "AltBearing"}, "FromSet", pos, 2.0, 2.0, parent, controllers)
	return elem
end

function addFcrCurrentCenterline(name, h05, pos, parent, controllers)
	local rect_verts = {{0, h05}, {0, -h05}}
	local elem 	= addStrokeLine(name, rect_verts, THICKNESS_NARROW, FUZZINESS_NARROW, parent, controllers)
	return elem
end

function addFcrLastCenterline(name, h05, pos, parent, controllers)
	local len_c = h05/2
	local len_notch = 0.55*len_c;
	local len_space = len_c - len_notch;

	local start = h05
	local stop  = start - len_notch/2
	local elems = {}
	for i = 1,5 do
		local rect_verts = {{0, start}, {0, stop }}

		local elem = addStrokeLine(name..i, rect_verts, THICKNESS_NARROW, FUZZINESS_NARROW, parent, controllers)
		start = stop - len_space
		stop = (i < 4) and (start - len_notch) or -h05
		join(elems, elem)
	end
	return elems
end

function addStrokeAccelerationCue(name, pos, parent, controllers)
	local elem 	= addStrokeCircleEx(name, 22, pos, nil, nil, parent, controllers )
	return elem
end


function addStrokeCuedLosReticle(name, pos, parent, controllers)
	local elem 	= addStrokeSymbolEx(name, {"stroke_symbols_HMD", "CuedLosReticle"}, "FromSet", pos, 2.0, 2.0, parent, controllers)
	return elem
end

function addHeadTracker(name, pos, parent, controllers)
	local size			= 78*scale_coeff
	local s05			= size / 2
	local l				= s05 / 3
	local line_width	= 4

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	-- corners
	local c_u 	= addStrokeLine(name.."_up",	{{ -l, s05 - l},{0, s05}}, 		nil, nil, PH.name)
	local c_u_u = addStrokeLine(name.."_up_u",	{{  0, s05}, 	{l, s05 - l}}, 	nil, nil, PH.name)
	local c_d 	= addStrokeLine(name.."_down",	{{ -l,-s05 + l},{0,	-s05}}, 	nil, nil, PH.name)
	local c_d_d = addStrokeLine(name.."_down_d",{{  0,-s05}, 	{l,	-s05 + l}},	nil, nil, PH.name)
	local c_l 	= addStrokeLine(name.."_left",	{{-s05 + l,  l},{-s05,   0}},	nil, nil, PH.name)
	local c_l_l = addStrokeLine(name.."_left_l",{{-s05,  0}, 	{-s05 + l,-l}}, nil, nil, PH.name)
	local c_r	= addStrokeLine(name.."_right",	{{ s05 - l,  l},{ s05,   0}},	nil, nil, PH.name)
	local c_r_r = addStrokeLine(name.."_right_r",{{ s05, 0},	{ s05 - l,-l}}, nil, nil, PH.name)

	return {
		PH,
		c_u, c_u_u, c_d, c_d_d, c_l, c_l_l, c_r, c_r_r
	}
end


function add_LOS_Reticle(name, size, pos, parent, controllers, thickness )
	local s05 = 20	-- space
	local w05 = size - s05
	local h05 = w05
	t = thickness or THICKNESS_NORM

	local s = w05 + 2*s05

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local lu = addStrokeLine(name.."_up",	{{   0, s05}, {   0, s05+h05}}, t, nil, PH.name)
	local ld = addStrokeLine(name.."_down",	{{   0,-s05}, {   0, -s05-h05}},t, nil, PH.name)
	local ll = addStrokeLine(name.."_left",	{{-s05,   0}, {-s05-w05,   0}}, t, nil, PH.name)
	local lr = addStrokeLine(name.."_right",{{ s05,   0}, { s05+w05,   0}}, t, nil, PH.name)
	return { PH, lu, ld, ll, lr}
end

function add_HOVER_LOS_Reticle(name, pos, parent, controllers)
	local size = PitchLadder_h/16
	local ret_val = add_LOS_Reticle("HOVER", size, pos, parent, controllers, THICKNESS_NORM*1.5)
	return ret_val
end

function add_CRUISE_LOS_Reticle(name, pos, parent, controllers)
	local size = PitchLadder_h/16
	local ret_val = add_LOS_Reticle("CRUISE", size, pos, parent, controllers, THICKNESS_NORM*3.0)
	return ret_val
end

function add_TRANSITION_LOS_Reticle(name, pos, parent, controllers)
	local size = PitchLadder_h/16
	local ret_val = add_LOS_Reticle("TRANSITION", size, pos, parent, controllers, THICKNESS_NORM*1.5)
	return ret_val
end

function addCuedLosReticle(name, pos, parent, controllers )
	local size05 	= PitchLadder_h/16
	local s05 		= 20 -- space
	local w05_len	= (size05 - s05)/2.9
	local w05_sp	= 0.9 * w05_len
	local h05_len	= w05_len
	local h05_sp	= w05_sp


	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local s05_1 = s05 + h05_len + h05_sp

	local lu_1  = addStrokeLine(name.."_up_1",		{{   3, s05}, 	{   3, s05 + h05_len}}, nil, nil, PH.name) --4
	local lu_2  = addStrokeLine(name.."_up_2",		{{  -3, s05},	{  -3, s05 + h05_len}}, nil, nil, PH.name) --4
	local lu_11 = addStrokeLine(name.."_up_11",		{{   3, s05_1},	{   3, s05_1 + h05_len}}, nil, nil, PH.name) --4
	local lu_21 = addStrokeLine(name.."_up_21",		{{  -3, s05_1},	{  -3, s05_1 + h05_len}}, nil, nil, PH.name) --4

	local ld_1  = addStrokeLine(name.."_down_1",	{{   3, -s05}, 	{   3, -s05-h05_len}}, nil, nil, PH.name) --4
	local ld_2  = addStrokeLine(name.."_down_2",	{{  -3, -s05},	{  -3, -s05-h05_len}}, nil, nil, PH.name) --4
	local ld_11 = addStrokeLine(name.."_down_11",	{{   3, -s05_1},{   3, -s05_1-h05_len}}, nil, nil, PH.name) --4
	local ld_21 = addStrokeLine(name.."_down_21",	{{  -3, -s05_1},{  -3, -s05_1-h05_len}}, nil, nil, PH.name) --4

	s05_1 = s05 + w05_len + w05_sp

	local lr_1  = addStrokeLine(name.."_right_1",	{{ s05,   3}, { s05 + w05_len,   3}}, nil, nil, PH.name) --4
	local lr_2  = addStrokeLine(name.."_right_2",	{{ s05,  -3}, { s05 + w05_len,  -3}}, nil, nil, PH.name) --4
	local lr_11 = addStrokeLine(name.."_right_11",	{{ s05_1, 3}, { s05_1 + w05_len, 3}}, nil, nil, PH.name) --4
	local lr_21 = addStrokeLine(name.."_right_21",	{{ s05_1,-3}, { s05_1 + w05_len,-3}}, nil, nil, PH.name) --4

	local ll_1  = addStrokeLine(name.."_left_1",	{{ -s05,   3}, { -s05-w05_len,   3}}, nil, nil, PH.name) --4
	local ll_2  = addStrokeLine(name.."_left_2",	{{ -s05,  -3}, { -s05-w05_len,  -3}}, nil, nil, PH.name) --4
	local ll_11 = addStrokeLine(name.."_left_11",	{{ -s05_1, 3}, { -s05_1-w05_len, 3}}, nil, nil, PH.name) --4
	local ll_21 = addStrokeLine(name.."_left_21",	{{ -s05_1,-3}, { -s05_1-w05_len,-3}}, nil, nil, PH.name) --4

	return { PH, lu_1, lu_2, lu_11, lu_21, ld_1,ld_2,ld_11,ld_21, lr_1, lr_2, lr_11, lr_21, ll_1,ll_2,ll_11,ll_21 }
end

-- TADS LOS Reticle defs
local TADS_LOS_Reticle_w05 = 100*scale_coeff
local TADS_LOS_Reticle_h05 = 75*scale_coeff

function add_TADS_LOS_Reticle(name, pos, parent, controllers)
	local w05 = TADS_LOS_Reticle_w05
	local h05 = TADS_LOS_Reticle_h05
	local s_w05 = 14*scale_coeff	-- space
	local s_h05 = 10*scale_coeff	-- space

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local lu = addStrokeLine(name.."_up",	{{     0, s_h05}, {   0, h05}},	nil, nil, PH.name) --4
	local ld = addStrokeLine(name.."_down",	{{     0,-s_h05}, {   0,-h05}},	nil, nil, PH.name) --4
	local ll = addStrokeLine(name.."_left",	{{-s_w05,     0}, {-w05,   0}},	nil, nil, PH.name) --4
	local lr = addStrokeLine(name.."_right",{{ s_w05,     0}, { w05,   0}},	nil, nil, PH.name) --4
	return { PH, lu, ld, ll, lr }
end

function add_CueingDots(name, pos, parent)

	local x = pos[1]
	local y = pos[2]
	local s = 55*scale_coeff

	local PH = addPlaceholder(name.."_PH", pos, parent)

	local dot_u = addStrokeDot(name.."_up",		{ 0, s}, PH.name, nil)
	local dot_d = addStrokeDot(name.."_down",		{ 0,-s}, PH.name, nil)
	local dot_l = addStrokeDot(name.."_left",		{-s, 0}, PH.name, nil)
	local dot_r = addStrokeDot(name.."_right",	{ s, 0}, PH.name, nil)

	return {
		PH,

		dot_u[1], dot_u[2],
		dot_d[1], dot_d[2],
		dot_l[1], dot_l[2],
		dot_r[1], dot_r[2],
	}
end

function add_LMC_On(name, pos, parent, controllers)
	local w05 = TADS_LOS_Reticle_w05 + 5
	local h05 = TADS_LOS_Reticle_h05 + 5
	local len = 10
	local l05 = len / 2

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local l1 = addStrokeLine(name.."_up",	{{-l05, h05}, { l05, h05}}, nil, nil, PH.name)
	local l2 = addStrokeLine(name.."_down",	{{-l05,-h05}, { l05,-h05}}, nil, nil, PH.name)
	local l3 = addStrokeLine(name.."_left",	{{-w05,-l05}, {-w05, l05}}, nil, nil, PH.name)
	local l4 = addStrokeLine(name.."_right",{{ w05,-l05}, { w05, l05}}, nil, nil, PH.name)
	return { PH, l1, l2, l3, l4 }
end

function add_LaserFiringIndicator(name, pos, parent, controllers)
	local len_gain = 4 / 5
	local w05 = TADS_LOS_Reticle_h05 * len_gain
	local h05 = TADS_LOS_Reticle_h05 * len_gain
	local s_w05 = w05 / 2
	local s_h05 = s_w05

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local l1 = addStrokeLine(name.."_up_left",	{{-s_w05, s_h05}, {-w05, h05}},	nil, nil, PH.name)
	local l2 = addStrokeLine(name.."_up_right",	{{ s_w05, s_h05}, { w05, h05}},	nil, nil, PH.name)
	local l3 = addStrokeLine(name.."_down_left",	{{-s_w05,-s_h05}, {-w05,-h05}},	nil, nil, PH.name)
	local l4 = addStrokeLine(name.."_down_right",	{{ s_w05,-s_h05}, { w05,-h05}},	nil, nil, PH.name)
	return { PH, l1, l2, l3, l4 }
end

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

	-- corners
	local c_ul = addStrokeLine(name.."_up_left",	{{-s05, s05 - dash}, {-s05, s05}, {-s05 + dash, s05}},	 nil, nil, PH.name)
	local c_ur = addStrokeLine(name.."_up_right",	{{ s05, s05 - dash}, { s05, s05}, { s05 - dash, s05}},	 nil, nil, PH.name)
	local c_dl = addStrokeLine(name.."_down_left",	{{-s05,-s05 + dash}, {-s05,-s05}, {-s05 + dash,-s05}},	 nil, nil, PH.name)
	local c_dr = addStrokeLine(name.."_down_right",	{{ s05,-s05 + dash}, { s05,-s05}, { s05 - dash,-s05}},	 nil, nil, PH.name)
	-- centers
	local c_u = addStrokeLine(name.."_up",		{{-d05, s05}, { d05, s05}},	 nil, nil, PH.name)
	local c_d = addStrokeLine(name.."_down",	{{-d05,-s05}, { d05,-s05}},	 nil, nil, PH.name)
	local c_l = addStrokeLine(name.."_left",	{{-s05,-d05}, {-s05, d05}},	 nil, nil, PH.name)
	local c_r = addStrokeLine(name.."_right",	{{ s05,-d05}, { s05, d05}},	 nil, nil, PH.name)

	return
	{
		PH,
		--back_line,
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

	-- corners
	local c_ul = addStrokeLine(name.."_corner_up_left",		{{-s05, s05 - dash}, {-s05, s05}, {-s05 + dash, s05}},	nil, nil, PH.name)
	local c_ur = addStrokeLine(name.."_corner_up_right",	{{ s05, s05 - dash}, { s05, s05}, { s05 - dash, s05}},	nil, nil, PH.name)
	local c_dl = addStrokeLine(name.."_corner_down_left",	{{-s05,-s05 + dash}, {-s05,-s05}, {-s05 + dash,-s05}},	nil, nil, PH.name)
	local c_dr = addStrokeLine(name.."_corner_down_right",	{{ s05,-s05 + dash}, { s05,-s05}, { s05 - dash,-s05}},	nil, nil, PH.name)
	--
	local l0 = -s05 + dash + gap
	local dashes = {}

	for i = 0,5 do
		local lb = l0 + period * i
		local le = lb + dash
		local d_u = addStrokeLine(name.."_up"..i,		{{ lb, s05}, { le, s05}},	nil, nil, PH.name)
		local d_d = addStrokeLine(name.."_down"..i,		{{ lb,-s05}, { le,-s05}},	nil, nil, PH.name)
		local d_l = addStrokeLine(name.."_left"..i,		{{-s05, lb}, {-s05, le}},	nil, nil, PH.name)
		local d_r = addStrokeLine(name.."_right"..i,	{{ s05, lb}, { s05, le}},	nil, nil, PH.name)
		dashes[#dashes + 1] = d_u
		dashes[#dashes + 1] = d_d
		dashes[#dashes + 1] = d_l
		dashes[#dashes + 1] = d_r
	end

	local elems =
	{
		PH,
		--back_line,
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

function addAltitudeHold(name, pos, parent, controllers)
	local x = pos[1]
	local y = pos[2]

	local w = 20*scale_coeff
	local w05 = 8*scale_coeff
	local h = 22*scale_coeff
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
	local elems = addStrokeLine(name, verts, nil, nil, parent, controllers)
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
	local elems = addStrokeLine(name, verts, nil, nil, parent, controllers)
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

	-- front
	local circle 		= addStrokeCircleEx(name.."_circle", r, {0,0}, nil, nil, PH.name )
	local notch_up		= addStrokeLine(name.."_up",		up_verts, nil, nil, PH.name)
	local notch_lh		= addStrokeLine(name.."_lh",		lh_verts, nil, nil, PH.name)
	local notch_rh		= addStrokeLine(name.."_rh",		rh_verts, nil, nil, PH.name)

	return {
		PH,
		circle,			notch_up,		notch_lh,		notch_rh,
	}
end

function addNavFlyToCUE(name, pos, parent, controllers)
	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	local point = addStrokePoint(name.."_point", {0,0}, PH.name)

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

	local diamond = addStrokeLine(name.."_diamond", verts, nil, nil, PH.name) --4

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

	local period = 120
	local dash = 60

	local elems = { PH }

	for i = 1,4,1 do
		local ps = period * i
		local pe = ps + dash

		local dl = addStrokeLine(name.."_dash_lhs_"..i, {{-ps,0}, {-pe,0}}, nil, nil, PH.name)
		local dr = addStrokeLine(name.."_dash_rhs_"..i, {{ ps,0}, { pe,0}}, nil, nil, PH.name)

		elems[#elems + 1] = dl
		elems[#elems + 1] = dr
	end

	return elems
end

function addPitchLadder(name, pos, parent, controllers)
	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)
	local elems = { PH }

	local hor_line_len_05 = 600
	local digits = { 10, 20, 30, 45, 60 }
	local hor_line = addStrokeLine(name.."HorizonLine", {{-hor_line_len_05,0},{hor_line_len_05,0}}, nil, nil, PH.name)
	join(elems, hor_line)

	local step_y	= PitchLadder_h/4
	local dash		= 29
	local length	= dash * 5
	local width		= hor_line_len_05 / 3
	local gap		= width - length
	local dgap		= 29

	local y		= 0.0
	local xs	= 0.0
	local xe	= 0.0
	local xm	= 0.0
	local dym	= -7


	for i = 1,5 do
		local dy = (i < 4) and  step_y or 1.5 * step_y
		y = y + dy
		xs = gap + dgap * (i - 1)
		xe = xs + length
		xm = xe + 11

			-- sky portion
		local sky_lhs	= addStrokeLine(name.."_SkyLhs_"..i, {{-xs,y},{-xe,y}},			nil, nil, PH.name)
		local sky_lhs_c	= addStrokeLine(name.."_SkyLhs_c_"..i, {{-xe,y},{-xe,y-dash}},	nil, nil, PH.name)
		local sky_rhs 	= addStrokeLine(name.."_SkyRhs_"..i, {{ xs,y},{ xe,y}},			nil, nil, PH.name)
		local sky_rhs_c = addStrokeLine(name.."_SkyRhs_c_"..i, {{ xe,y},{ xe,y-dash}},	nil, nil, PH.name)
		join(elems, sky_lhs)
		join(elems, sky_lhs_c)
		join(elems, sky_rhs)
		join(elems, sky_rhs_c)

		local sky_mark_lhs = addStrokeText(name.."_SkyMarkLhs_"..i, digits[i], STROKE_FNT_DFLT_100, "RightCenter", {-xm,y+dym}, PH.name)
		local sky_mark_rhs = addStrokeText(name.."_SkyMarkRhs_"..i, digits[i], STROKE_FNT_DFLT_100, "LeftCenter", { xm,y+dym}, PH.name)
		join(elems, sky_mark_lhs)
		join(elems, sky_mark_rhs)
		-- ground portion
		local sky_lhs_1 = addStrokeLine(name.."_GroundLhs_1_"..i, {{-xs-0*dash,-y},{-xs-1*dash,-y}},	nil, nil, PH.name)
		local sky_lhs_2 = addStrokeLine(name.."_GroundLhs_2_"..i, {{-xs-2*dash,-y},{-xs-3*dash,-y}},	nil, nil, PH.name)
		local sky_lhs_c = addStrokeLine(name.."_GroundLhs_c_"..i, {{-xs-4*dash,-y},{-xs-5*dash,-y}},	nil, nil, PH.name)
		local sky_lhs_cc = addStrokeLine(name.."_GroundLhs_cc_"..i,{{-xs-5*dash,-y},{-xe,-y+dash}},		nil, nil, PH.name)
		join(elems, sky_lhs_1)
		join(elems, sky_lhs_2)
		join(elems, sky_lhs_c)
		join(elems, sky_lhs_cc)
		local sky_rhs_1 = addStrokeLine(name.."_GroundRhs_1_"..i, {{xs+0*dash,-y},{xs+1*dash,-y}}, 	nil, nil, PH.name)
		local sky_rhs_2 = addStrokeLine(name.."_GroundRhs_2_"..i, {{xs+2*dash,-y},{xs+3*dash,-y}}, 	nil, nil, PH.name)
		local sky_rhs_c = addStrokeLine(name.."_GroundRhs_c_"..i, {{xs+4*dash,-y},{xs+5*dash,-y}},	nil, nil, PH.name)
		local sky_rhs_cc = addStrokeLine(name.."_GroundRhs_cc_"..i,{{xs+5*dash,-y},{ xe,-y+dash}},	nil, nil, PH.name)
		join(elems, sky_rhs_1)
		join(elems, sky_rhs_2)
		join(elems, sky_rhs_c)
		join(elems, sky_rhs_cc)
		local ground_mark_lhs = addStrokeText(name.."_GroundMarkLhs_"..i, digits[i], STROKE_FNT_DFLT_100, "RightCenter", {-xm,-y+dym}, PH.name)
		local ground_mark_rhs = addStrokeText(name.."_GroundMarkRhs_"..i, digits[i], STROKE_FNT_DFLT_100, "LeftCenter", { xm,-y+dym}, PH.name)
		join(elems, ground_mark_lhs)
		join(elems, ground_mark_rhs)
	end
	--CLIMB&DIV
	y = y + 3.0 * step_y
	local climb_ball = addStrokeSkidSlipBall("climb_ball", {0, y}, PH.name )
	local climb_lhs	= addStrokeLine("climb_Lhs", 	{{-xs*0.8,y+xs/2},{-xs*0.8,y-xs/2}},	nil, nil, PH.name)
	local climb_rhs	= addStrokeLine("climb_Rhs", 	{{ xs*0.8,y+xs/2},{ xs*0.8,y-xs/2}},	nil, nil, PH.name)

	local climb_PH = addRotPlaceholder("climb_PH", {0, y}, 180,  PH.name)
	local climb_up = addStrokeText("climb_up", "CLIMB", STROKE_FNT_DFLT_100, "CenterBottom", {0,y+xs*0.7}, PH.name)
	local climb_dn = addStrokeText("climb_dn", "CLIMB", STROKE_FNT_DFLT_100, "CenterBottom", {0, xs*0.7}, climb_PH.name)

	join(elems, climb_ball)
	join(elems, climb_lhs)
	join(elems, climb_rhs)
	join(elems, climb_up)
	join(elems, climb_dn)
--	join(elems, climb_PH)

	local div_ball = addStrokeSkidSlipBall("div_ball", {0, -y}, PH.name )
	local div_lhs	= addStrokeLine("div_Lhs", 	{{-xs*0.8,-y+xs/2},{-xs*0.8,-y-xs/2}},	nil, nil, PH.name)
	local div_rhs	= addStrokeLine("div_Rhs", 	{{ xs*0.8,-y+xs/2},{ xs*0.8,-y-xs/2}},	nil, nil, PH.name)

	local div_PH = addRotPlaceholder("div_PH", {0, -y}, 180,  PH.name)
	local div_up = addStrokeText("div_up", "DIVE", STROKE_FNT_DFLT_100, "CenterBottom", {0,-y+xs*0.7}, PH.name)
	local div_dn = addStrokeText("div_dn", "DIVE", STROKE_FNT_DFLT_100, "CenterBottom", {0, xs*0.7}, div_PH.name)
	join(elems, div_ball)
	join(elems, div_lhs)
	join(elems, div_rhs)
	join(elems, div_up)
	join(elems, div_dn)
	--	join(elems, div_PH)
	return elems
end

function addBankAnglePointer(name, pos, parent, controllers)
	local elem = buildBorderedSymbol(name, {17,16}, "CenterTop", {177,178}, pos, parent, controllers)
	return elem
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

---------------------------------------- FCR C-SCOPE ---------------------------------------------------
function addStrokeTransparentMask(name, set, align, pos, thickness, fuzziness, parent, controllers, scale)
	local symbol		= CreateElement "ceSMultiLine"
	setSymbolCommonProperties(symbol, name, pos, parent, controllers, "MFD_TRANSPARENT")
	setSymbolAlignment(symbol, align)

	symbol.points_set	= set
	symbol.scale		= scale or 1
	symbol.thickness  = thickness or THICKNESS_NORM
	symbol.fuzziness  = fuzziness or FUZZINESS_NORM
	symbol.additive_alpha = true;

	symbol.level			= DEFAULT_LEVEL
	symbol.h_clip_relation	= h_clip_relations.INCREASE_IF_LEVEL

	Add(symbol)
	return symbol
end

function resetStrokeTransparentMask(name, set, align, pos, thickness, fuzziness, parent, controllers, scale)
	local symbol        = CreateElement "ceSMultiLine"
	setSymbolCommonProperties(symbol, name, pos, parent, controllers, "MFD_TRANSPARENT")
	setSymbolAlignment(symbol, align)

	symbol.points_set    = set
	symbol.scale        = scale or 1
	symbol.thickness  = thickness or THICKNESS_NORM
	symbol.fuzziness  = fuzziness or FUZZINESS_NORM
	symbol.additive_alpha = true;

	symbol.level              = DEFAULT_LEVEL
	symbol.h_clip_relation    = h_clip_relations.REWRITE_LEVEL

	Add(symbol)
	return symbol
end

FCR_SYMBOLS =
{
	LOAL_UNKNOWN 				= 0,
	LOAL_FLYER 					= 1,
	LOAL_HELICOPTER 			= 2,
	LOAL_AIRDEFENSE_MERGED 		= 3,
	LOAL_AIRDEFENSE_UNIT 		= 4,
	LOAL_WHEEL 					= 5,
	LOAL_TRACK 					= 6,

	LOBL_STAT_UNKNOWN 			= 7,
	LOBL_STAT_FLYER 			= 8,
	LOBL_STAT_HELICOPTER 		= 9,
	LOBL_STAT_AIRDEFENSE_MERGED	= 10,
	LOBL_STAT_AIRDEFENSE_UNIT 	= 11,
	LOBL_STAT_WHEEL 			= 12,
	LOBL_STAT_TRACK 			= 13,

	LOBL_MOV_UNKNOWN 			= 14,
	LOBL_MOV_FLYER 				= 15,
	LOBL_MOV_HELICOPTER 		= 16,
	LOBL_MOV_AIRDEFENSE_MERGED 	= 17,
	LOBL_MOV_AIRDEFENSE_UNIT 	= 18,
	LOBL_MOV_WHEEL 				= 19,
	LOBL_MOV_TRACK				= 20
}

FCR_NTS_TYPES =
{
	NONE 						= 0,
	SOLID 						= 1,
	DASHED 						= 2,
	ALWAYS 						= 3
}

function increaseLevel(obj, level)
	level					= level or 1
	obj.level				= DEFAULT_LEVEL + level
end

function setClipRelation(obj, relation)
	relation = relation or h_clip_relations.NULL
	obj.h_clip_relation = relation
end

function CScopeMasks(f, postName)
	postName = postName or ""
	local scale = 1.5
	local NormalThickness 	= 2.0
	local NormalFuzziness 	= 1.5

	local FCR_PH = addPlaceholder("FCR C-SCOPE Transparent Root"..postName, nil, nil, {{"HMD_COMMON_cScopeEnable"}, {"VideoSignal_Shift"}})
	for i = 0, 16 do
		local FCR_Symbol_PH = addPlaceholder("Symbol "..i..postName, nil, FCR_PH.name, {{"HMD_cScopeSymbolPos", c_scope_w, c_scope_h, i}})
		f("Helicopter "..i..postName, 	{"stroke_symbols_HMD", "FCR-Helicopter-LOBL-Stat"},	"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrTansparentSymbol", i, FCR_SYMBOLS.LOBL_STAT_HELICOPTER}, {"HMD_COMMON_FcrTansparentShow"}}, scale)
		f("ADU "..i..postName, 			{"stroke_symbols_HMD", "FCR-ADU-LOBL-Stat"}, 		"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrTansparentSymbol", i, FCR_SYMBOLS.LOBL_STAT_AIRDEFENSE_UNIT}, {"HMD_COMMON_FcrTansparentShow"}}, scale)
		f("Track "..i..postName, 		{"stroke_symbols_HMD", "FCR-Track-LOBL-Stat"}, 		"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrTansparentSymbol", i, FCR_SYMBOLS.LOBL_STAT_TRACK}, {"HMD_COMMON_FcrTansparentShow"}}, scale)
		f("Wheel "..i..postName, 		{"stroke_symbols_HMD", "FCR-Wheel-LOBL-Stat"}, 		"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrTansparentSymbol", i, FCR_SYMBOLS.LOBL_STAT_WHEEL}, {"HMD_COMMON_FcrTansparentShow"}}, scale)
		f("Unknown "..i..postName, 		{"stroke_symbols_HMD", "FCR-Unknown-LOBL-Stat"}, 	"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrTansparentSymbol", i, FCR_SYMBOLS.LOBL_STAT_UNKNOWN}, {"HMD_COMMON_FcrTansparentShow"}}, scale)

		f("Shoot At "..i..postName, 	{"stroke_symbols_HMD", "shoot-at"}, 				"FromSet", nil, NormalThickness, NormalThickness, FCR_PH.name, {{"HMD_COMMON_ShootAt", i, c_scope_w, c_scope_h}}, scale)
	end

	-- NTS | ANTS
	f("NTS"..postName,  {"stroke_symbols_HMD", "FCR-NTS-Solid"}, 		"FromSet", nil, NormalThickness, NormalFuzziness, FCR_PH.name, {{"HMD_cScopeNts",  c_scope_w, c_scope_h, FCR_NTS_TYPES.ALWAYS}, {"HMD_COMMON_FcrTansparentShow"}}, scale)
	f("ANTS"..postName, {"stroke_symbols_HMD", "FCR-ANTS"}, 			"FromSet", nil, NormalThickness, NormalFuzziness, FCR_PH.name, {{"HMD_cScopeAnts", c_scope_w, c_scope_h}, {"HMD_COMMON_FcrTansparentShow"}}, scale)
end

function AddCScopeSymbols(render_back)
	local scale = 1.5
	local NormalThickness 	= 3.0
	local NormalFuzziness 	= 2.0
	local WideThickness 	= 3.0
	local WideFuzziness 	= 2.0

	local FCR_PH = addPlaceholder("FCR C-SCOPE Root", nil, render_back.name, {{"HMD_COMMON_cScopeEnable"}, {"VideoSignal_Shift"}})
	for i = 0, 16 do
		local FCR_Symbol_PH = addPlaceholder("FCR Symbol Root "..i, nil, FCR_PH.name, {{"HMD_cScopeSymbolPos", c_scope_w, c_scope_h, i}})

		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Helicopter-LOAL"}, 		"FromSet", nil, WideThickness, 	 WideFuzziness,   FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOAL_HELICOPTER}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Helicopter-LOBL-Stat"},	"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOBL_STAT_HELICOPTER}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Helicopter-LOBL-Mov"}, 	"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOBL_MOV_HELICOPTER}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-ADU-LOAL"}, 				"FromSet", nil, WideThickness,   WideFuzziness,   FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOAL_AIRDEFENSE_UNIT}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-ADU-LOBL-Stat"}, 		"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOBL_STAT_AIRDEFENSE_UNIT}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-ADU-LOBL-Mov"}, 			"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOBL_MOV_AIRDEFENSE_UNIT}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Track-LOAL"}, 			"FromSet", nil, WideThickness,   WideFuzziness,   FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOAL_TRACK}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Track-LOBL-Stat"}, 		"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOBL_STAT_TRACK}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Track-LOBL-Mov"}, 		"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOBL_MOV_TRACK}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Wheel-LOAL"}, 			"FromSet", nil, WideThickness,   WideFuzziness,   FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOAL_WHEEL}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Wheel-LOBL-Stat"}, 		"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOBL_STAT_WHEEL}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Wheel-LOBL-Mov"}, 		"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOBL_MOV_WHEEL}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Unknown-LOAL"}, 			"FromSet", nil, WideThickness,   WideFuzziness,   FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOAL_UNKNOWN}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Unknown-LOBL-Stat"}, 	"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOBL_STAT_UNKNOWN}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Unknown-LOBL-Mov"}, 		"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOBL_MOV_UNKNOWN}}, scale)
		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-Flyer-LOBL-Mov"}, 		"FromSet", nil, NormalThickness, NormalFuzziness, FCR_Symbol_PH.name, {{"HMD_COMMON_FcrSymbol", i, FCR_SYMBOLS.LOBL_MOV_FLYER}}, scale)

		addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "shoot-at"}, 					"FromSet", nil, WideThickness, WideFuzziness, FCR_PH.name, {{"HMD_COMMON_ShootAt", i, c_scope_w, c_scope_h}}, scale)
	end

	-- NTS | ANTS
	addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-NTS-Solid"}, 	"FromSet", nil, WideThickness, WideFuzziness, FCR_PH.name, {{"HMD_cScopeNts",  c_scope_w, c_scope_h, FCR_NTS_TYPES.SOLID}}, scale)
	addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-NTS-Dashed"}, 	"FromSet", nil, WideThickness, WideFuzziness, FCR_PH.name, {{"HMD_cScopeNts",  c_scope_w, c_scope_h, FCR_NTS_TYPES.DASHED}}, scale)
	addStrokeSymbolEx(nil, {"stroke_symbols_HMD", "FCR-ANTS"}, 			"FromSet", nil, WideThickness, WideFuzziness, FCR_PH.name, {{"HMD_cScopeAnts", c_scope_w, c_scope_h}}, scale)
end


-------------------------------------------------------------------------------------------
function addVideoSignal()
	local hmd_color = {35, 140, 35, 255}

	-- NOTE !!!  we cannot work with angles,  when we project it to plane -> it is always TANS
	local Video_W = RadToDI(2 * math.tan(math.rad(24.4637680)))
	local Video_H = RadToDI(2 * math.tan(math.rad(24.4637680)))
	local verts_box = buildBoxVerts(Video_W, Video_H, "CenterCenter")
	local verts 	= buildHMD_maskVerts(200)
	local indices 	= buildEllipseIndices(#verts)

	local	render_back						= CreateElement "ceMeshPoly"
			render_back.name				= "VideoSignalBack"
			render_back.primitivetype		= "triangles"
			render_back.vertices			= verts
			render_back.indices				= indices
			render_back.isvisible			= false
			render_back.material			= "MASK_MATERIAL"
			render_back.level				= DEFAULT_LEVEL
			render_back.h_clip_relation		= h_clip_relations.REWRITE_LEVEL
			render_back.init_pos			= {0, 0, 0}
			render_back.controllers			= {{"VideoAreaPH"},{"VideoSignal_Show"}}
	Add(render_back)


	CScopeMasks(addStrokeTransparentMask, " Open")

	local	render_base	 = addRotPlaceholder("VideoSignalBase",{0,0}, 0, nil )

	local	PNVS_underlay					= CreateElement "ceTexPoly"
			PNVS_underlay.name				= "PNVS_underlay"
			PNVS_underlay.vertices			= verts_box
			PNVS_underlay.indices			= default_box_indices
			PNVS_underlay.tex_coords		= {{0, 1},{0, 0},{1, 0},{1, 1}}
			PNVS_underlay.material			= MakeMaterial("PNVS_CAM_AH64_HMD", hmd_color)
			PNVS_underlay.level				= DEFAULT_LEVEL
			PNVS_underlay.h_clip_relation	= h_clip_relations.COMPARE
			PNVS_underlay.parent_element	= render_base.name
			PNVS_underlay.additive_alpha	= true
		--	PNVS_underlay.input_space_SRGB	= true
			PNVS_underlay.controllers		= {{"VideoUnderlay_PNVS"}, {"VideoSignal_Zoom"}, {"VideoSignal_Shift"}}
	Add(PNVS_underlay)

	local	TADS_underlay					= CreateElement "ceTexPoly"
			TADS_underlay.name				= "TADS_underlay"
			TADS_underlay.vertices			= verts_box
			TADS_underlay.indices			= default_box_indices
			TADS_underlay.tex_coords		= {{0, 1},{0, 0},{1, 0},{1, 1}}
			TADS_underlay.material			= MakeMaterial("TADS_CAM_AH64_HMD", hmd_color)
			TADS_underlay.level				= DEFAULT_LEVEL
			TADS_underlay.h_clip_relation	= h_clip_relations.COMPARE
			TADS_underlay.parent_element	= render_base.name
			TADS_underlay.additive_alpha	= true
		--	TADS_underlay.input_space_SRGB	= true
			TADS_underlay.controllers		= {{"VideoUnderlay_TADS"}, {"VideoSignal_Zoom"}, {"VideoSignal_Shift"}}
	Add(TADS_underlay)

	CScopeMasks(resetStrokeTransparentMask, " Close")
	AddCScopeSymbols(render_back)

end
---------------------------------------------------------------------------------------------
function addStrokeLine(name, verts, thickness, fuzziness, parent, controllers)
	local N = #verts-1

	if N<=0 then return nil end

	local indices = {}

	if verts[1] == verts[#verts] then
		indices = {0, N}
	end

	local line = CreateElement "ceSMultiLine"
	setSymbolCommonProperties(line, name, {0,0}, parent, controllers, "HMD_GREEN")

	for i = 0, N-1 do
		indices[#indices + 1] = i
		indices[#indices + 1] = i+1
	end

	line.vertices	= verts
	line.indices	= indices
	line.thickness  = thickness or THICKNESS_NORM
	line.fuzziness  = fuzziness or FUZZINESS_NORM

	Add(line)
	return line
end

--------------------------------------------------------------------------------------------------
function addStrokeCircleEx(name, radius, pos, thickness, fuzziness, parent, controllers, arc, segment, gap, dashed, material)
	local segmentsN = 64

	local circle			= CreateElement "ceSCircle"
	setSymbolCommonProperties(circle, name, pos, parent, controllers, material)
	--setStrokeSymbolProperties(circle)
	circle.radius			= {radius, radius}
	circle.arc				= arc or {0, math.pi * 2}
	circle.segment			= segment or math.pi * 4 / segmentsN
	circle.gap				= gap or math.pi * 4 / segmentsN
	circle.segment_detail	= 4

	if dashed ~= nil then
		circle.dashed		= dashed
	else
		circle.dashed		= false
	end
	circle.thickness  = thickness or THICKNESS_NORM
	circle.fuzziness  = fuzziness or FUZZINESS_NORM
	Add(circle)
	return circle
end
--------------------------------------------------------------------------------------
-- Stroke symbol with points described in a .svg file
function addStrokeSymbolEx(name, set, align, pos, thickness, fuzziness, parent, controllers, scale, material)
	local symbol		= CreateElement "ceSMultiLine"
	setSymbolCommonProperties(symbol, name, pos, parent, controllers, material)
	setSymbolAlignment(symbol, align)
--	setStrokeSymbolProperties(symbol)
	symbol.points_set	= set
	symbol.scale		= scale or 1
	symbol.thickness  = thickness or THICKNESS_NORM
	symbol.fuzziness  = fuzziness or FUZZINESS_NORM
	symbol.additive_alpha = true
	Add(symbol)
	return symbol
end
---------------------------------------------------------------------------------------------------
function addSimpleLine(name, verts, width, material, parent, controllers, h_clip_relation, level)
	local line = buildLine(name, verts, width, parent, controllers, material)
	if h_clip_relation ~= nil then
		line.h_clip_relation	= h_clip_relation
		line.level				= level
	end
	addElem(line)
	return line
end

------------------------------------------------------------------------------------------------------
function addStrokeDashedLine(name, length, stroke, gap, pos, rot, parent, controllers)
	local segLength			= stroke + gap
	local numOfWholePairs	= math.floor(length / segLength)
	local reminder			= length - numOfWholePairs * segLength

	rot = rot or 0

	local rot_rad			= math.rad(rot)
	local dx				= -segLength * math.sin(rot_rad)
	local dy				=  segLength * math.cos(rot_rad)

	local function addSegment(num, len)
		local pos_seg0 = { pos[1] + dx * num,						pos[2] + dy * num}
		local pos_seg1 = { pos_seg0[1] - len * math.sin(rot_rad),	pos_seg0[2] + len * math.cos(rot_rad) }

		local seg_verts = {pos_seg0, pos_seg1}

		--addStrokeLine(name, verts, thickness, fuzziness, parent, controllers, h_clip_relation, level)
		local elem = addStrokeLine(name.." _seg_"..num, seg_verts, nil, nil, parent, controllers)
	end

	for segNum = 0, numOfWholePairs - 1 do
		addSegment(segNum, stroke)
	end

	if reminder > 0 then
		if reminder >= stroke then
			addSegment(numOfWholePairs, stroke)
		else
			addSegment(numOfWholePairs, reminder)
		end
	end
end

------------------------------------------------------------------------------------------------------
function add_RKT_SteeringCursor(parent)
	local scale_factor	= 1.15
	local base_name		= "RKT_SteeringCursor"

	local dash_h		= 19
	local dash_v		= 27
	local gap_h			= dash_h
	local gap_v			= 9

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

	-- draw if steering cursor must be drawn, set position
	local PH_Base = addPlaceholder(base_name.."_PH_Base", {0,0}, parent, {{"DSPLS_WEAPON_RocketSteeringCursor_Show"},{"DSPLS_WEAPON_RocketSteeringCursor_Pos", video_area_w_05, video_area_h_05}})

	-- draw if cursor is (1) or (2)
	local PH_Inhibit = addPlaceholder(base_name.."_Inhibit_PH", {0,0}, PH_Base.name, {{"DSPLS_WEAPON_RocketSteeringCursor_Dashed", 0}})

	-- (1)/(2) Articulating / Ground stow / inhibited
	addStrokeDashedLine(base_name.."_Inhibit_topline",		line_length.t, dash_h,	gap_h,	line_pos.top,		90,	PH_Inhibit.name, nil)
	addStrokeDashedLine(base_name.."_Inhibit_bottomline",	line_length.b, dash_h,	gap_h,	line_pos.bottom,	90,	PH_Inhibit.name, nil)

	addStrokeDashedLine(base_name.."_Inhibit_centerline_up",	line_length.cs, dash_v,	gap_v,	line_pos.center_t,	180,	PH_Inhibit.name, nil)
	addStrokeLine(base_name.."_Inhibit_centerline_cntr",		{{0,dash_v/2},{0,-dash_v/2}}, nil, nil, 					PH_Inhibit.name, {{"DSPLS_WEAPON_RocketSteeringCursor_Stowed", 0}})
	addStrokeDashedLine(base_name.."_Inhibit_centerline_dn",	line_length.cs, dash_v,	gap_v,	line_pos.center_d,	0,		PH_Inhibit.name, nil)

	-- ----------------------------------------------

	-- draw if cursor is (3) or (4)
	local PH_Normal = addPlaceholder(base_name.."_Normal_PH", {0,0}, PH_Base.name, {{"DSPLS_WEAPON_RocketSteeringCursor_Dashed", 1}})

	-- draw one or another
	local PH_Normal_Articulating	= addPlaceholder(base_name.."_Normal_Art_PH", {0,0}, PH_Normal.name, {{"DSPLS_WEAPON_RocketSteeringCursor_Stowed", 0}})
	local PH_Normal_Stowed			= addPlaceholder(base_name.."_Normal_Stow_PH", {0,0}, PH_Normal.name, {{"DSPLS_WEAPON_RocketSteeringCursor_Stowed", 1}})

	-- (3)/(4) Articulating / Ground stow
	addStrokeDashedLine(base_name.."_Normal_topline",		line_length.t, 2*dash_h+gap_h,	gap_h,	line_pos.top,		90,	PH_Normal.name, nil)
	addStrokeDashedLine(base_name.."_Normal_bottomline",	line_length.b, 2*dash_h+gap_h ,	gap_h,	line_pos.bottom,	90,	PH_Normal.name, nil)

	addStrokeLine(base_name.."_Normal_Art_centerline", {{0,line_length.c/2},{0,-line_length.c/2}}, nil, nil, PH_Normal_Articulating.name)
	addStrokeDashedLine(base_name.."_Normal_Stow_centerline",	line_length.c, 	2*dash_v+gap_v,	dash_v+2*gap_v,	line_pos.center_d,	0,	PH_Normal_Stowed.name, nil)

end
function buildRoundedBoxVerts( size, radius )
	local segments_count	= 5
	local r					= radius or 10
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

local function splitVerts(tbl, verts)
	for i,v in pairs(verts) do
		if i ~= 1 then
			tbl[#tbl + 1] = v
		end
	end
end