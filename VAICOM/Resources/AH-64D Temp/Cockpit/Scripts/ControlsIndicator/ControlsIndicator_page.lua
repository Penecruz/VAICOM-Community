dofile(LockOn_Options.common_script_path.."elements_defs.lua")

SetCustomScale(1.0)

local show_masks		= false
DEFAULT_LEVEL			= 0

local DRAW_SCAS_INDICATION = true


function setAsInvisibleMask(obj)
	obj.isvisible	= show_masks
	obj.material	= "MASK_MATERIAL_PURPLE"
end

function setClipLevel(obj, level)
	level					= level or 0
	obj.h_clip_relation		= h_clip_relations.COMPARE
	obj.level				= DEFAULT_LEVEL + level
end

function AddElement(object)
	object.screenspace = ScreenType.SCREENSPACE_TRUE
	object.use_mipfilter = true
	setClipLevel(object)
	Add(object)
end

local aspect		= LockOn_Options.screen.aspect
local margin		= 0.05
local size			= 0.4
local size_w		= size
local size_h		= size
local size_w_05		= size_w * 0.5
local size_h_05		= size_h * 0.5

local font_scale = 0.1

-- cyclic stick area
local cyclic_size			= size * 0.9
local cyclic_size_w			= cyclic_size
local cyclic_size_h			= cyclic_size
local cyclic_size_w_05		= cyclic_size_w * 0.5
local cyclic_size_h_05		= cyclic_size_h * 0.5
local cyclic_center_x		= size_w_05 - cyclic_size_w_05
local cyclic_center_y		= size_h_05 - cyclic_size_h_05
local cyclic_mark_size		= cyclic_size * 0.05
local cyclic_mark_size_05	= cyclic_mark_size * 0.5
local cyclic_mark_size_w	= cyclic_mark_size * 0.8
local cyclic_mark_size_h	= cyclic_mark_size * 1.2
local cyclic_mark_size_w_05	= cyclic_mark_size_w * 0.5
local cyclic_mark_size_h_05	= cyclic_mark_size_h * 0.5
-- collective stick area
local collective_size_w		= size_w - cyclic_size_w
local collective_size_h		= size_h * 0.7
local collective_size_w_05	= collective_size_w * 0.5
local collective_size_h_05	= collective_size_h * 0.5
local collective_center_x	= -size_w_05 + collective_size_w_05
local collective_center_y	= size_h_05 - collective_size_h_05
-- throttle area
local throttle_size_w		= collective_size_w
local throttle_size_h		= size_h - collective_size_h
local throttle_size_w_05	= throttle_size_w * 0.5
local throttle_size_w_025	= throttle_size_w * 0.25
local throttle_size_h_05	= throttle_size_h * 0.5
local throttle_center_x		= collective_center_x
local throttle_center_y		= -size_h_05 + throttle_size_h_05
-- rudder area
local rudder_size_w			= cyclic_size_w
local rudder_size_h			= size_h - cyclic_size_h
local rudder_size_w_05		= rudder_size_w * 0.5
local rudder_size_h_05		= rudder_size_h * 0.5
local rudder_center_x		= cyclic_center_x
local rudder_center_y		= -size_h_05 + rudder_size_h_05
-- sas authority
local sas_authority_roll_w05		= cyclic_size_w * 0.1
local sas_authority_roll_w			= sas_authority_roll_w05 * 2
local sas_authority_pitch_h_fwd		= cyclic_size_h * 0.2
local sas_authority_pitch_h_aft		= cyclic_size_h * 0.1
local sas_authority_pitch_h			= sas_authority_pitch_h_fwd + sas_authority_pitch_h_aft
local sas_authority_pitch_h05		= sas_authority_pitch_h * 0.5
local sas_authority_rudder_w05		= rudder_size_w * 0.1
local sas_authority_collective_h05	= collective_size_h * 0.1


local function setPlaceholderCommonProperties(placeholder, name, pos, parent, controllers)
	placeholder.name			= name
	pos							= pos or {0, 0}
	placeholder.init_pos		= {pos[1], pos[2], 0}
	placeholder.collimated		= collimated or false

	if parent ~= nil then
		placeholder.parent_element	= parent
	end

	if controllers ~= nil then
		placeholder.controllers		= controllers
	end
end

function addPlaceholder(name, pos, parent, controllers)
	local placeholder			= CreateElement "ceSimple"
	setPlaceholderCommonProperties(placeholder, name, pos, parent, controllers)

	AddElement(placeholder)
	return placeholder
end

function addClippingBox(name, width, height, center_pos, clip_relation, level, parent, controllers)
	local pos		= center_pos or {0, 0}

	local area				= CreateElement "ceMeshPoly"
	area.name				= name
	area.primitivetype		= "triangles"
	area.vertices			=	{
									{ -width * 0.5, -height * 0.5 },
									{ -width * 0.5,  height * 0.5 },
									{  width * 0.5,  height * 0.5 },
									{  width * 0.5, -height * 0.5 }
								}
	area.indices			= default_box_indices
	area.additive_alpha		= false
	area.change_opacity		= false
	area.init_pos			= {pos[1], pos[2], 0}

	if parent ~= nil then
		area.parent_element = parent
	end

	if controllers ~= nil then
		if type(controllers) == "table" then
			area.controllers = controllers
		end
	end

	setAsInvisibleMask(area)
	AddElement(area)

	area.h_clip_relation	= clip_relation
	area.level				= DEFAULT_LEVEL + level

	return area
end

local function draw_base()
	local base				= CreateElement "ceMeshPoly"
	base.name				= "base"
	base.primitivetype		= "triangles"
	base.material			= "CONTROLS_INDICATOR_BACK"
	base.vertices			= {	{-size_w_05, -size_h_05},
								{-size_w_05,  size_h_05},
								{ size_w_05,  size_h_05},
								{ size_w_05, -size_h_05}}
	base.indices			= default_box_indices
	base.init_pos			= {-aspect + size_w_05 + margin, 1 - size_h_05 - margin}
	base.controllers		= {{"show"}}
	base.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
	base.level				= DEFAULT_LEVEL
	AddElement(base)
	return base
end

local function draw_cyclic_scas_area(parent, controllers)
	local area				= CreateElement "ceMeshPoly"
	area.name				= "cyclic_scas_area"
	area.primitivetype		= "triangles"
	area.material			= "CONTROLS_INDICATOR_SCAS"
	area.vertices			= {	{-sas_authority_roll_w05, -sas_authority_pitch_h_aft},
								{-sas_authority_roll_w05,  sas_authority_pitch_h_fwd},
								{ sas_authority_roll_w05,  sas_authority_pitch_h_fwd},
								{ sas_authority_roll_w05, -sas_authority_pitch_h_aft}}
	area.indices			= default_box_indices
	area.init_pos			= {0, 0}
	area.h_clip_relation	= h_clip_relations.COMPARE
	area.level				= DEFAULT_LEVEL

	if parent ~= nil then
		area.parent_element = parent
	end
	if controllers ~= nil then
		area.controllers	= controllers
	end

	AddElement(area)
	return area
end

local function draw_pedals_scas_area(parent, controllers)
	local area				= CreateElement "ceMeshPoly"
	area.name				= "pedals_scas_area"
	area.primitivetype		= "triangles"
	area.material			= "CONTROLS_INDICATOR_SCAS"
	area.vertices			= {	{-sas_authority_rudder_w05, -rudder_size_h_05},
								{-sas_authority_rudder_w05,  rudder_size_h_05},
								{ sas_authority_rudder_w05,  rudder_size_h_05},
								{ sas_authority_rudder_w05, -rudder_size_h_05}}
	area.indices			= default_box_indices
	area.init_pos			= {0, 0}
	area.h_clip_relation	= h_clip_relations.COMPARE
	area.level				= DEFAULT_LEVEL

	if parent ~= nil then
		area.parent_element = parent
	end
	if controllers ~= nil then
		area.controllers	= controllers
	end

	AddElement(area)
	return area
end

local function draw_collective_scas_area(parent, controllers)
	local area				= CreateElement "ceMeshPoly"
	area.name				= "collective_scas_area"
	area.primitivetype		= "triangles"
	area.material			= "CONTROLS_INDICATOR_SCAS"
	area.vertices			= {	{-collective_size_w_05, -sas_authority_collective_h05},
								{-collective_size_w_05,  sas_authority_collective_h05},
								{ collective_size_w_05,  sas_authority_collective_h05},
								{ collective_size_w_05, -sas_authority_collective_h05}}
	area.indices			= default_box_indices
	area.init_pos			= {0, 0}
	area.h_clip_relation	= h_clip_relations.COMPARE
	area.level				= DEFAULT_LEVEL

	if parent ~= nil then
		area.parent_element = parent
	end
	if controllers ~= nil then
		area.controllers	= controllers
	end

	AddElement(area)
	return area
end

local function draw_line(name, verts, parent, controllers)
	local elem			= CreateElement "ceSimpleLineObject"
	elem.name			= name
	elem.material		= "CONTROLS_INDICATOR_RED"
	elem.width			= size * 0.015
	elem.vertices		= verts
	elem.tex_params		= {{0.26,176.5/512},{0.74,176.5/512}, {0.5/size, 1.5/size}}

	elem.h_clip_relation	= h_clip_relations.COMPARE
	elem.level				= DEFAULT_LEVEL

	if parent ~= nil then
		elem.parent_element = parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end

	AddElement(elem)
	return elem
end

local function draw_line_white(name, verts, parent, controllers)
	local line = draw_line(name, verts, parent, controllers)
	line.material = "CONTROLS_INDICATOR_WHITE"
	return line
end

local function draw_line_green(name, verts, parent, controllers)
	local line = draw_line(name, verts, parent, controllers)
	line.material = "CONTROLS_INDICATOR_GREEN"
	return line
end

local function draw_line_turquoise(name, verts, parent, controllers)
	local line = draw_line(name, verts, parent, controllers)
	line.material = "CONTROLS_INDICATOR_TURQUOISE"
	return line
end

local function draw_diamond_mark(name, parent, controllers)
	local stick_position			= CreateElement "ceTexPoly"
	stick_position.name				= name
	stick_position.vertices			=	{
											{-cyclic_mark_size_w_05, -cyclic_mark_size_h_05},
											{-cyclic_mark_size_w_05,  cyclic_mark_size_h_05},
											{ cyclic_mark_size_w_05,  cyclic_mark_size_h_05},
											{ cyclic_mark_size_w_05, -cyclic_mark_size_h_05}
										}
	stick_position.indices			= default_box_indices
	stick_position.material			= "CONTROLS_INDICATOR_WHITE"
	stick_position.tex_params		= {330/512, 365.5/512, 40/512/cyclic_mark_size_w, 60/512/cyclic_mark_size_h}
	stick_position.controllers		= controllers
	stick_position.parent_element	= parent

	AddElement(stick_position)
	return stick_position
end


local function draw_text(name, text, pos, alignment, stringdefs, parent, controllers)
	local txt			= CreateElement "ceStringPoly"
	txt.name			= name
	txt.material		= "font_CONTROLS_INDICATOR_WHITE"
	txt.init_pos		= {pos[1], pos[2], 0}
	txt.value			= text
	txt.alignment		= alignment
	txt.parent_element	= parent
	txt.stringdefs		= stringdefs
	txt.controllers		= controllers

	AddElement(txt)
	return txt
end


-- area
local back = draw_base()

-- area separators
-- TODO: remove ???
--local left_separator = draw_line("left_separator", {{cyclic_center_x - cyclic_size_w_05, -size_h_05}, {cyclic_center_x - cyclic_size_w_05, size_h_05}}, back.name)
--local bottom_separator = draw_line("bottom_separator", {{cyclic_center_x - cyclic_size_w_05, cyclic_center_y - cyclic_size_h_05}, {cyclic_center_x + cyclic_size_w_05, cyclic_center_y - cyclic_size_h_05}}, back.name)

-- cyclic stick area
local cyclic_clip_area = addClippingBox("cyclic_mask", cyclic_size_w, cyclic_size_h, {cyclic_center_x,cyclic_center_y}, h_clip_relations.INCREASE_IF_LEVEL, 0, back.name)

local cyclic_area = addPlaceholder("cyclic_area", {cyclic_center_x, cyclic_center_y}, back.name)
setClipLevel(cyclic_area, 1)

if DRAW_SCAS_INDICATION then
	local cyclic_scas_area = draw_cyclic_scas_area(cyclic_area.name, {{"stick_pitch", cyclic_size_h_05},{"stick_roll", cyclic_size_w_05}})
	setClipLevel(cyclic_scas_area, 1)

	ATTITUDE_HOLD =
	{
		RECTANGLE = 0,
		CORNERS = 1,
		DASHES = 2,
	}

	local dash_len = sas_authority_roll_w * 0.3
	local dash_len05 = dash_len * 0.5

	-- attitude hold modes
	local sas_att_hold_rect_ph = addPlaceholder("sas_att_hold_rect_ph", {0, 0}, cyclic_scas_area.name, {{"sas_hold_attitude", ATTITUDE_HOLD.RECTANGLE}})
	setClipLevel(sas_att_hold_rect_ph, 1)
	local sas_att_hold_corn_ph = addPlaceholder("sas_att_hold_corn_ph", {0, 0}, cyclic_scas_area.name, {{"sas_hold_attitude", ATTITUDE_HOLD.CORNERS}})
	setClipLevel(sas_att_hold_corn_ph, 1)
	local sas_att_hold_dash_ph = addPlaceholder("sas_att_hold_dash_ph", {0, 0}, cyclic_scas_area.name, {{"sas_hold_attitude", ATTITUDE_HOLD.DASHES}})
	setClipLevel(sas_att_hold_dash_ph, 1)

	-- rectangle
	-- rectangle divided to pitch and roll indication:
	-- att pitch
	local sas_att_hold_pitch_fwd = draw_line_turquoise("sas_att_hold_pitch_fwd",
		{
			{-sas_authority_roll_w05,  sas_authority_pitch_h_fwd},
			{ sas_authority_roll_w05,  sas_authority_pitch_h_fwd}
		}, sas_att_hold_rect_ph.name, {{"sas_hold_pitch"}})
	setClipLevel(sas_att_hold_pitch_fwd, 1)
	local sas_att_hold_pitch_aft = draw_line_turquoise("sas_att_hold_pitch_aft",
		{
			{-sas_authority_roll_w05, -sas_authority_pitch_h_aft},
			{ sas_authority_roll_w05, -sas_authority_pitch_h_aft}
		}, sas_att_hold_rect_ph.name, {{"sas_hold_pitch"}})
	setClipLevel(sas_att_hold_pitch_aft, 1)
	-- att roll
	local sas_att_hold_roll_left = draw_line_turquoise("sas_att_hold_roll_left",
		{
			{-sas_authority_roll_w05, -sas_authority_pitch_h_aft},
			{-sas_authority_roll_w05,  sas_authority_pitch_h_fwd}
		}, sas_att_hold_rect_ph.name, {{"sas_hold_roll"}})
	setClipLevel(sas_att_hold_roll_left, 1)
	local sas_att_hold_roll_right = draw_line_turquoise("sas_att_hold_roll_right",
		{
			{ sas_authority_roll_w05, -sas_authority_pitch_h_aft},
			{ sas_authority_roll_w05,  sas_authority_pitch_h_fwd}
		}, sas_att_hold_rect_ph.name, {{"sas_hold_roll"}})
	setClipLevel(sas_att_hold_roll_right, 1)


	-- corners
	local sas_att_hold_corner_rt = draw_line_turquoise("sas_att_hold_corner_rt",
		{
			{ sas_authority_roll_w05 - dash_len,	sas_authority_pitch_h_fwd},
			{ sas_authority_roll_w05,				sas_authority_pitch_h_fwd},
			{ sas_authority_roll_w05,				sas_authority_pitch_h_fwd - dash_len}
		}, sas_att_hold_corn_ph.name)
	setClipLevel(sas_att_hold_corner_rt, 1)

	local sas_att_hold_corner_rb = draw_line_turquoise("sas_att_hold_corner_rb",
		{
			{ sas_authority_roll_w05 - dash_len,	-sas_authority_pitch_h_aft},
			{ sas_authority_roll_w05,				-sas_authority_pitch_h_aft},
			{ sas_authority_roll_w05,				-sas_authority_pitch_h_aft + dash_len}
		}, sas_att_hold_corn_ph.name)
	setClipLevel(sas_att_hold_corner_rb, 1)

	local sas_att_hold_corner_lb = draw_line_turquoise("sas_att_hold_corner_lb",
		{
			{ -sas_authority_roll_w05 + dash_len,	-sas_authority_pitch_h_aft},
			{ -sas_authority_roll_w05,				-sas_authority_pitch_h_aft},
			{ -sas_authority_roll_w05,				-sas_authority_pitch_h_aft + dash_len}
		}, sas_att_hold_corn_ph.name)
	setClipLevel(sas_att_hold_corner_lb, 1)

	local sas_att_hold_corner_lt = draw_line_turquoise("sas_att_hold_corner_lt",
		{
			{ -sas_authority_roll_w05 + dash_len,	sas_authority_pitch_h_fwd},
			{ -sas_authority_roll_w05,				sas_authority_pitch_h_fwd},
			{ -sas_authority_roll_w05,				sas_authority_pitch_h_fwd - dash_len}
		}, sas_att_hold_corn_ph.name)
	setClipLevel(sas_att_hold_corner_lt, 1)

	-- dashes
	local sas_att_hold_dash_l = draw_line_turquoise("sas_att_hold_dash_l",
		{
			{ -sas_authority_roll_w05,	-sas_authority_pitch_h_aft + sas_authority_pitch_h05 - dash_len05},
			{ -sas_authority_roll_w05,	-sas_authority_pitch_h_aft + sas_authority_pitch_h05 + dash_len05}
		}, sas_att_hold_dash_ph.name)
	setClipLevel(sas_att_hold_dash_l, 1)

	local sas_att_hold_dash_r = draw_line_turquoise("sas_att_hold_dash_r",
		{
			{ sas_authority_roll_w05,	-sas_authority_pitch_h_aft + sas_authority_pitch_h05 - dash_len05},
			{ sas_authority_roll_w05,	-sas_authority_pitch_h_aft + sas_authority_pitch_h05 + dash_len05}
		}, sas_att_hold_dash_ph.name)
	setClipLevel(sas_att_hold_dash_r, 1)
end

local pitch_scale = draw_line("pitch_scale", {{0, -cyclic_size_h_05}, {0, cyclic_size_h_05}}, cyclic_area.name)
local roll_scale = draw_line("roll_scale", {{-cyclic_size_w_05, 0}, {cyclic_size_w_05, 0}}, cyclic_area.name)
setClipLevel(pitch_scale, 1)
setClipLevel(roll_scale, 1)

local cyclic_trim_controllers = {{"stick_trim_pitch", cyclic_size_h_05},{"stick_trim_roll", cyclic_size_w_05}}
local cyclic_trim_mark_1 = draw_line("stick_trim_position_1", {{-cyclic_mark_size_w_05, -cyclic_mark_size_h_05}, {cyclic_mark_size_w_05,  cyclic_mark_size_h_05}}, cyclic_area.name, cyclic_trim_controllers)
local cyclic_trim_mark_2 = draw_line("stick_trim_position_2", {{-cyclic_mark_size_w_05,  cyclic_mark_size_h_05}, {cyclic_mark_size_w_05, -cyclic_mark_size_h_05}}, cyclic_area.name, cyclic_trim_controllers)
setClipLevel(cyclic_trim_mark_1, 1)
setClipLevel(cyclic_trim_mark_2, 1)

local cyclic_stick_mark = draw_diamond_mark("stick_position", cyclic_area.name, {{"stick_pitch", cyclic_size_h_05},{"stick_roll", cyclic_size_w_05}})
setClipLevel(cyclic_stick_mark, 1)

if DRAW_SCAS_INDICATION then
	local cyclic_scas_pos = addPlaceholder("cyclic_scas_pos", {0, 0}, cyclic_stick_mark.name, {{"cyclic_scas_pos", cyclic_size_w_05, cyclic_size_h_05}})
	local cyclic_scas_mark_h = draw_line_green("cyclic_scas_mark_h", {{-cyclic_mark_size_05, 0}, {cyclic_mark_size_05, 0}}, cyclic_scas_pos.name)
	local cyclic_scas_mark_v = draw_line_green("cyclic_scas_mark_v", {{0, -cyclic_mark_size_05}, {0, cyclic_mark_size_05}}, cyclic_scas_pos.name)
	setClipLevel(cyclic_scas_pos, 1)
	setClipLevel(cyclic_scas_mark_h, 1)
	setClipLevel(cyclic_scas_mark_v, 1)
end

local reset_cyclic_clip_area = addClippingBox("cyclic_mask_reset", cyclic_size_w, cyclic_size_h, {cyclic_center_x,cyclic_center_y}, h_clip_relations.REWRITE_LEVEL, 0, back.name)


-- rudder area
local pedals_clip_area = addClippingBox("pedals_mask", rudder_size_w, rudder_size_h, {rudder_center_x,rudder_center_y}, h_clip_relations.INCREASE_IF_LEVEL, 0, back.name)

local pedals_area = addPlaceholder("pedals_area", {rudder_center_x, rudder_center_y}, back.name)
setClipLevel(pedals_area, 1)

if DRAW_SCAS_INDICATION then
	local pedals_scas_area = draw_pedals_scas_area(pedals_area.name, {{"rudder", rudder_size_w_05}})
	setClipLevel(pedals_scas_area, 1)

	local sas_hold_rudder = addPlaceholder("sas_hold_rudder", {0, 0}, pedals_scas_area.name, {{"sas_hold_rudder"}})
	setClipLevel(sas_hold_rudder, 1)
	local sas_hold_rudder_l = draw_line_turquoise("sas_hold_rudder_l", {{-sas_authority_rudder_w05, rudder_size_h_05}, {-sas_authority_rudder_w05, -rudder_size_h_05}}, sas_hold_rudder.name)
	setClipLevel(sas_hold_rudder_l, 1)
	local sas_hold_rudder_r = draw_line_turquoise("sas_hold_rudder_r", {{ sas_authority_rudder_w05, rudder_size_h_05}, { sas_authority_rudder_w05, -rudder_size_h_05}}, sas_hold_rudder.name)
	setClipLevel(sas_hold_rudder_r, 1)
end

local pedals_scale = draw_line("pedals_scale", {{-rudder_size_w_05, 0}, {rudder_size_w_05, 0}}, pedals_area.name)
setClipLevel(pedals_scale, 1)

local pedals_trim_mark_1 = draw_line("pedals_trim_position_1", {{-cyclic_mark_size_w_05, -cyclic_mark_size_h_05}, {cyclic_mark_size_w_05,  cyclic_mark_size_h_05}}, pedals_scale.name, {{"pedals_trim", rudder_size_w_05}})
local pedals_trim_mark_2 = draw_line("pedals_trim_position_2", {{-cyclic_mark_size_w_05,  cyclic_mark_size_h_05}, {cyclic_mark_size_w_05, -cyclic_mark_size_h_05}}, pedals_scale.name, {{"pedals_trim", rudder_size_w_05}})
setClipLevel(pedals_trim_mark_1, 1)
setClipLevel(pedals_trim_mark_2, 1)

local pedals_mark = draw_diamond_mark("pedals_mark", pedals_scale.name, {{"rudder", rudder_size_w_05}})
local pedals_mark_vu = draw_line_white("pedals_mark_vu", {{0,  rudder_size_h_05 * 0.5}, {0,  rudder_size_h_05}}, pedals_mark.name)
local pedals_mark_vd = draw_line_white("pedals_mark_vd", {{0, -rudder_size_h_05 * 0.5}, {0, -rudder_size_h_05}}, pedals_mark.name)
setClipLevel(pedals_mark, 1)
setClipLevel(pedals_mark_vu, 1)
setClipLevel(pedals_mark_vd, 1)

if DRAW_SCAS_INDICATION then
	local pedals_scas_mark = draw_line_green("pedals_scas_mark", {{0, -rudder_size_h_05}, {0, rudder_size_h_05}}, pedals_mark.name, {{"rudder_scas", rudder_size_w_05}})
	setClipLevel(pedals_scas_mark, 1)
end

local reset_pedals_clip_area = addClippingBox("pedals_mask_reset", rudder_size_w, rudder_size_h, {rudder_center_x,rudder_center_y}, h_clip_relations.REWRITE_LEVEL, 0, back.name)

-- collective stick area
local collective_clip_area = addClippingBox("collective_clip_area", collective_size_w, collective_size_h, {collective_center_x,collective_center_y}, h_clip_relations.INCREASE_IF_LEVEL, 0, back.name)

local collective_area = addPlaceholder("collective_area", {collective_center_x, collective_center_y - collective_size_h_05}, back.name)
setClipLevel(collective_area, 1)

if DRAW_SCAS_INDICATION then
	local collective_scas_area = draw_collective_scas_area(collective_area.name, {{"collective", collective_size_h}})
	setClipLevel(collective_scas_area, 1)

	local sas_hold_collective = addPlaceholder("sas_hold_collective", {0, 0}, collective_scas_area.name, {{"sas_hold_collective"}})
	setClipLevel(sas_hold_collective, 1)
	local sas_hold_collective_u = draw_line_turquoise("sas_hold_collective_u", {{-collective_size_w_05,  sas_authority_collective_h05}, {collective_size_w_05,  sas_authority_collective_h05}}, sas_hold_collective.name)
	setClipLevel(sas_hold_collective_u, 1)
	local sas_hold_collective_d = draw_line_turquoise("sas_hold_collective_d", {{-collective_size_w_05, -sas_authority_collective_h05}, {collective_size_w_05, -sas_authority_collective_h05}}, sas_hold_collective.name)
	setClipLevel(sas_hold_collective_d, 1)
end

local collective_scale = draw_line("collective_scale", {{0, 0}, {0, collective_size_h}}, collective_area.name)
local collective_mark = draw_diamond_mark("collective_mark", collective_area.name, {{"collective", collective_size_h}})
local collective_mark_hl = draw_line_white("collective_mark_hl", {{-collective_size_w_05, 0}, {-collective_size_w_05 * 0.3, 0}}, collective_mark.name)
local collective_mark_hr = draw_line_white("collective_mark_hr", {{ collective_size_w_05, 0}, { collective_size_w_05 * 0.3, 0}}, collective_mark.name)
setClipLevel(collective_scale, 1)
setClipLevel(collective_mark, 1)
setClipLevel(collective_mark_hl, 1)
setClipLevel(collective_mark_hr, 1)

if DRAW_SCAS_INDICATION then
	local collective_scas_mark = draw_line_green("collective_scas_mark", {{-collective_size_w_05, 0}, {collective_size_w_05, 0}}, collective_mark.name, {{"collective_scas", collective_size_h}})
	setClipLevel(collective_scas_mark, 1)
end

local reset_collective_clip_area = addClippingBox("collective_clip_area_reset", collective_size_w, collective_size_h, {collective_center_x,collective_center_y}, h_clip_relations.REWRITE_LEVEL, 0, back.name)

-- throttle area
local throttle_area = addPlaceholder("throttle_area", {throttle_center_x, throttle_center_y - throttle_size_h_05}, back.name)
local throttle_L_area = addPlaceholder("throttle_L_area", {-throttle_size_w_025, 0}, throttle_area.name)
local throttle_R_area = addPlaceholder("throttle_R_area", { throttle_size_w_025, 0}, throttle_area.name)

local idle_pos	= 0.25 * throttle_size_h
local fly_pos	= 0.9 * throttle_size_h

local throttle_L_scale = draw_line("throttle_L_scale", {{0, idle_pos}, {0, throttle_size_h}}, throttle_L_area.name)
local throttle_R_scale = draw_line("throttle_R_scale", {{0, idle_pos}, {0, throttle_size_h}}, throttle_R_area.name)

local throttle_L_IDLE_mark = draw_line("throttle_L_IDLE_mark", {{-throttle_size_w_025, idle_pos}, {throttle_size_w_025, idle_pos}}, throttle_L_area.name)
local throttle_R_IDLE_mark = draw_line("throttle_R_IDLE_mark", {{-throttle_size_w_025, idle_pos}, {throttle_size_w_025, idle_pos}}, throttle_R_area.name)

local throttle_L_FLY_mark = draw_line("throttle_L_FLY_mark", {{-throttle_size_w_025, fly_pos}, {throttle_size_w_025, fly_pos}}, throttle_L_area.name)
local throttle_R_FLY_mark = draw_line("throttle_R_FLY_mark", {{-throttle_size_w_025, fly_pos}, {throttle_size_w_025, fly_pos}}, throttle_R_area.name)

local throttle_L_mark = draw_line_white("throttle_L_mark", {{-throttle_size_w_025, 0}, {throttle_size_w_025, 0}}, throttle_L_area.name, {{"throttle", 0, throttle_size_h}})
local throttle_R_mark = draw_line_white("throttle_R_mark", {{-throttle_size_w_025, 0}, {throttle_size_w_025, 0}}, throttle_R_area.name, {{"throttle", 1, throttle_size_h}})

local throttle_stringdefs = {throttle_size_w * 0.6 * font_scale, throttle_size_w * 0.6 * 1.3 * font_scale, 0.0, 0.0}

local Lockout_L_mark = draw_text("Lockout_L_mark", "L", {0,0}, "CenterBottom", throttle_stringdefs, throttle_L_area.name, {{"lockout", 0}})
local Lockout_R_mark = draw_text("Lockout_R_mark", "L", {0,0}, "CenterBottom", throttle_stringdefs, throttle_R_area.name, {{"lockout", 1}})


-- WHEEL BRAKES ---------------------------------------------------------------
local brakes_pos		= {cyclic_size_w_05,-cyclic_size_h_05}
local sz_wheel_brake	= cyclic_size_h_05 * 0.5
local wheel_brake_width	= 0.05 * sz_wheel_brake

for i = 1,2 do

	local signum = {-1,1}

	local wheel_brake_control_base = addClippingBox("wheel_brake_control_base"..i, cyclic_size_w, cyclic_size_h, {cyclic_center_x,cyclic_center_y}, h_clip_relations.REWRITE_LEVEL, 0, back.name)

	local	wheel_brake_mask					= CreateElement "ceMeshPoly"
			wheel_brake_mask.name				= "wheel_brake_mask_"..tostring(i)
			wheel_brake_mask.primitivetype		= "triangles"
			wheel_brake_mask.vertices			= {	{  0                               , 0},
													{  0                               , sz_wheel_brake},
													{ -signum[i] * 0.3 * sz_wheel_brake, sz_wheel_brake},
													{ -signum[i] * 0.3 * sz_wheel_brake, 0}}
			wheel_brake_mask.indices			= default_box_indices
			wheel_brake_mask.material			= "MASK_MATERIAL"
			wheel_brake_mask.init_pos			= {signum[i] * brakes_pos[1],brakes_pos[2]}
			wheel_brake_mask.parent_element		= wheel_brake_control_base.name
			wheel_brake_mask.controllers		= {{"brakes_value",i,sz_wheel_brake}}
			wheel_brake_mask.isvisible			= false
	AddElement(wheel_brake_mask)
			wheel_brake_mask.h_clip_relation	= h_clip_relations.INCREASE_LEVEL
			wheel_brake_mask.level				= DEFAULT_LEVEL

	local	wheel_brake							= CreateElement "ceMeshPoly"
			wheel_brake.name					= "wheel_brake_"..tostring(i)
			wheel_brake.primitivetype			= "triangles"
			wheel_brake.vertices				= {
													{0 ,0},
													{0 ,sz_wheel_brake},
													{ -signum[i] * 0.3 * sz_wheel_brake, sz_wheel_brake}}
			wheel_brake.indices					= {0,1,2}
			wheel_brake.material				= "ARCADE"
			wheel_brake.init_pos				= wheel_brake_mask.init_pos
			wheel_brake.parent_element			= wheel_brake_control_base.name
	AddElement(wheel_brake)

	local wheel_brake_actual_base = addClippingBox("wheel_brake_actual_base"..i, cyclic_size_w, cyclic_size_h, {cyclic_center_x,cyclic_center_y}, h_clip_relations.REWRITE_LEVEL, 0, back.name)

	local	wheel_brake_actual					= CreateElement "ceMeshPoly"
			wheel_brake_actual.name				= "wheel_brake_actual_"..tostring(i)
			wheel_brake_actual.primitivetype	= "triangles"
			wheel_brake_actual.vertices			= {	{  0                               , 0},
													{  0                               , wheel_brake_width},
													{ -signum[i] * 0.3 * sz_wheel_brake, wheel_brake_width},
													{ -signum[i] * 0.3 * sz_wheel_brake, 0}}
			wheel_brake_actual.indices			= default_box_indices
			wheel_brake_actual.material			= "ARCADE_WHITE"
			wheel_brake_actual.init_pos			= {signum[i] * brakes_pos[1],brakes_pos[2]}
			wheel_brake_actual.parent_element	= wheel_brake_actual_base.name
			wheel_brake_actual.controllers		= {{"brakes_value_actual",i,sz_wheel_brake}}
	AddElement(wheel_brake_actual)

end

parking_brake_lbl                 = CreateElement "ceStringPoly"
parking_brake_lbl.name            = "parking_brake_lbl"
parking_brake_lbl.material        = "font_CONTROLS_INDICATOR_RED"
parking_brake_lbl.init_pos		   = {-0.89 * brakes_pos[1], brakes_pos[2] + sz_wheel_brake * 1.25}
parking_brake_lbl.alignment       = "LeftBottom"
parking_brake_lbl.stringdefs      = {0.0035, 0.0035,0,0}
parking_brake_lbl.value           = "B"
parking_brake_lbl.controllers     = {{"parking_brake_value"}}
parking_brake_lbl.parent_element  = back.name
AddElement(parking_brake_lbl)



-- VRS Indication
local VRS_PB = addPlaceholder("VRS_PB", {0, 0}, cyclic_area.name, {{"VRS_Show"}})

local function draw_VRS_stage(name, stage, parent, controllers)

	local centers_x = { 0.75, 0.25, 0.75 }
	local centers_y = { 0.25, 0.75, 0.75 }

	local center_x = centers_x[stage]
	local center_y = centers_y[stage]


	local VRS_stage				= CreateElement "ceTexPoly"
	VRS_stage.name				= name
	VRS_stage.vertices			=	{
											{-cyclic_size_w_05, -cyclic_size_h_05},
											{-cyclic_size_w_05,  cyclic_size_h_05},
											{ cyclic_size_w_05,  cyclic_size_h_05},
											{ cyclic_size_w_05, -cyclic_size_h_05}
										}
	VRS_stage.indices			= default_box_indices
	VRS_stage.material			= "VRS_INDICATOR"
	VRS_stage.tex_coords		= {{center_x - 0.25, center_y + 0.25},{center_x - 0.25, center_y - 0.25},{center_x + 0.25, center_y - 0.25},{center_x + 0.25, center_y + 0.25}}
	VRS_stage.controllers		= controllers
	VRS_stage.parent_element	= parent

	--VRS_stage.init_pos			= {0, 0}
	--VRS_stage.h_clip_relation	= h_clip_relations.COMPARE
	--VRS_stage.level				= DEFAULT_LEVEL

	AddElement(VRS_stage)
	return VRS_stage
end

local VRS_stage_start		= draw_VRS_stage("VRS_start",		1, VRS_PB.name, {{"VRS_Stage", 0}})
local VRS_stage_evolution	= draw_VRS_stage("VRS_evolution",	2, VRS_PB.name, {{"VRS_Stage", 1}})
local VRS_stage_progress	= draw_VRS_stage("VRS_progress",	3, VRS_PB.name, {{"VRS_Stage", 2}})
setClipLevel(VRS_stage_start, 1)
setClipLevel(VRS_stage_evolution, 1)
setClipLevel(VRS_stage_progress, 1)




--  DDDDDDDDDDDDD      EEEEEEEEEEEEEEEEEEEEEEBBBBBBBBBBBBBBBBB   UUUUUUUU     UUUUUUUU       GGGGGGGGGGGGG
--  D::::::::::::DDD   E::::::::::::::::::::EB::::::::::::::::B  U::::::U     U::::::U    GGG::::::::::::G
--  D:::::::::::::::DD E::::::::::::::::::::EB::::::BBBBBB:::::B U::::::U     U::::::U  GG:::::::::::::::G
--  DDD:::::DDDDD:::::DEE::::::EEEEEEEEE::::EBB:::::B     B:::::BUU:::::U     U:::::UU G:::::GGGGGGGG::::G
--    D:::::D    D:::::D E:::::E       EEEEEE  B::::B     B:::::B U:::::U     U:::::U G:::::G       GGGGGG
--    D:::::D     D:::::DE:::::E               B::::B     B:::::B U:::::D     D:::::UG:::::G
--    D:::::D     D:::::DE::::::EEEEEEEEEE     B::::BBBBBB:::::B  U:::::D     D:::::UG:::::G
--    D:::::D     D:::::DE:::::::::::::::E     B:::::::::::::BB   U:::::D     D:::::UG:::::G    GGGGGGGGGG
--    D:::::D     D:::::DE:::::::::::::::E     B::::BBBBBB:::::B  U:::::D     D:::::UG:::::G    G::::::::G
--    D:::::D     D:::::DE::::::EEEEEEEEEE     B::::B     B:::::B U:::::D     D:::::UG:::::G    GGGGG::::G
--    D:::::D     D:::::DE:::::E               B::::B     B:::::B U:::::D     D:::::UG:::::G        G::::G
--    D:::::D    D:::::D E:::::E       EEEEEE  B::::B     B:::::B U::::::U   U::::::U G:::::G       G::::G
--  DDD:::::DDDDD:::::DEE::::::EEEEEEEE:::::EBB:::::BBBBBB::::::B U:::::::UUU:::::::U  G:::::GGGGGGGG::::G
--  D:::::::::::::::DD E::::::::::::::::::::EB:::::::::::::::::B   UU:::::::::::::UU    GG:::::::::::::::G
--  D::::::::::::DDD   E::::::::::::::::::::EB::::::::::::::::B      UU:::::::::UU        GGG::::::GGG:::G
--  DDDDDDDDDDDDD      EEEEEEEEEEEEEEEEEEEEEEBBBBBBBBBBBBBBBBB         UUUUUUUUU             GGGGGG   GGGG




local ds = 0.05 * size

--------------------------------------------------------------------------
-- Autopilot Indication
local autopilot_len = 2*size
local autopilot_height = autopilot_len*0.15
local au_tex_scale     		= 0.25/autopilot_len
local au_line_width    		= ((4.5/512)/au_tex_scale)/10
local au_ind_line_width 	= ((4.5/512)/au_tex_scale)/5
local au_ind_index_sz   	= (autopilot_len/8)*0.6


autopilot_base       		= CreateElement "ceMeshPoly"
autopilot_base.name		    = "autopilot_base"
autopilot_base.primitivetype  = "triangles"
--autopilot_base.material       = orange_mat -- "GREEN_TRANSPARENT"
autopilot_base.material       = "CONTROLS_INDICATOR_BACK"
autopilot_base.vertices       = {{0, -autopilot_height},
								{0,  autopilot_height  },
								{ autopilot_len                            ,  autopilot_height  },
								{ autopilot_len                            , -autopilot_height }}
autopilot_base.indices        = default_box_indices
autopilot_base.init_pos       = {0,(1 - autopilot_height)}
autopilot_base.controllers    = {{"autopilot_show"},{"screenspace_position",2,(aspect-autopilot_len),0},
                                {"screenspace_position",1,0,0}}

autopilot_base.h_clip_relation  = h_clip_relations.REWRITE_LEVEL
autopilot_base.level		      = DEFAULT_LEVEL
AddElement(autopilot_base)

function Add_HP_Limit(i,right,top)

	local line_len = au_ind_line_width*3*right
	local line_width = au_ind_line_width/2
	local color = "CONTROLS_INDICATOR_RED"
	if top == 0 then
		line_len = au_ind_line_width*3*right*1.5
		line_width = au_ind_line_width
		color = "CONTROLS_INDICATOR_WHITE"
	end


	au_HP_limit             = CreateElement "ceTexPoly"
	au_HP_limit.name         = "au_HP_limit"..i..right..top
	au_HP_limit.vertices     = {{0, -line_width},
									{0,  line_width},
									{ line_len,  line_width},
									{ line_len, -line_width}}
	au_HP_limit.indices      = default_box_indices
	au_HP_limit.material	  = color
	au_HP_limit.tex_params	  = {0.26,176.5/512},{0.74,176.5/512}, {0.5/size, 1.5/size}
	au_HP_limit.init_pos     = {(0.01+(autopilot_len/4)/2+i*autopilot_len/4)+au_ind_index_sz*right,top*(autopilot_len/4)*0.8/2}
	au_HP_limit.parent_element = autopilot_base.name
	AddElement(au_HP_limit)
end

function Add_R_Limit(right,top)

	local line_len = au_ind_line_width*3*top
	local line_width = au_ind_line_width/2
	local color = "CONTROLS_INDICATOR_RED"

	if right == 0 then
		line_len = au_ind_line_width*3*top*1.5
		line_width = au_ind_line_width
		color = "CONTROLS_INDICATOR_WHITE"
	end


	au_R_limit             = CreateElement "ceTexPoly"
	au_R_limit.name         = "au_R_limit"..right..top
	au_R_limit.vertices     = {{0, -line_width},
									{0,  line_width},
									{ line_len,  line_width},
									{ line_len, -line_width}}
	au_R_limit.indices      = default_box_indices
	au_R_limit.material	  = color
	au_R_limit.tex_params	  = {0.26,176.5/512},{0.74,176.5/512}, {0.5/size, 1.5/size}
	au_R_limit.init_pos       = {0.01+(autopilot_len/4)/2+(autopilot_len/4)*0.8/2*right,top*au_ind_index_sz}
	au_R_limit.parent_element = autopilot_base.name
	au_R_limit.init_rot       = {90,0,0}
	AddElement(au_R_limit)
end

function Add_B_30_limit(right)

	local line_width = au_ind_line_width/2
	au_B_limit           = CreateElement "ceTexPoly"
	au_B_limit.name		= "au_B_30_limit"..right
	au_B_limit.vertices  = {{0  , -line_width},
								{0  ,  line_width},
								{ au_ind_line_width*5*right   ,  line_width},
								{ au_ind_line_width*5*right   , -line_width}}
	au_B_limit.indices		= default_box_indices
	au_B_limit.material		= "CONTROLS_INDICATOR_RED"
	au_B_limit.tex_params	= {0.26,176.5/512},{0.74,176.5/512}, {0.5/size, 1.5/size}
	au_B_limit.init_pos     = {(0.01+(autopilot_len/4)/2+autopilot_len/4)+au_ind_index_sz*right,autopilot_height*0.3*0.5}
	au_B_limit.parent_element = autopilot_base.name
	au_B_limit.init_rot       = {30*right,0,0}
	AddElement(au_B_limit)
end

au_roll_scale           = CreateElement "ceTexPoly"
au_roll_scale.name		= "au_roll_scale"
au_roll_scale.vertices  = {{0   , -au_line_width},
                           {0   ,  au_line_width},
                           { autopilot_len   ,  au_line_width},
                           { autopilot_len   , -au_line_width}}
au_roll_scale.indices		= default_box_indices
au_roll_scale.material	    = "CONTROLS_INDICATOR_WHITE"
au_roll_scale.tex_params	= {0.26,176.5/512},{0.74,176.5/512}, {0.5/size, 1.5/size}
au_roll_scale.parent_element = autopilot_base.name
--AddElement(au_roll_scale)

--rudder
au_rudder_position              = CreateElement "ceTexPoly"
au_rudder_position.name         = "au_rudder_position"
au_rudder_position.vertices     = {{-au_ind_index_sz, -au_ind_line_width},
								{-au_ind_index_sz,  au_ind_line_width},
								{ au_ind_index_sz,  au_ind_line_width},
								{ au_ind_index_sz, -au_ind_line_width}}
au_rudder_position.indices      = default_box_indices
au_rudder_position.material	  	= "CONTROLS_INDICATOR_WHITE"
au_rudder_position.tex_params	= {0.26,176.5/512},{0.74,176.5/512}, {0.5/size, 1.5/size}
au_rudder_position.controllers  = {{"autopilot_value",(autopilot_len/4)*0.8/2,0}}
au_rudder_position.init_pos     = {0.01+(autopilot_len/4)/2,0.0}
au_rudder_position.parent_element = autopilot_base.name
AddElement(au_rudder_position)

Add_R_Limit(-1,1)
Add_R_Limit(0,1)
Add_R_Limit(1,1)

Add_R_Limit(-1,-1)
Add_R_Limit(0,-1)
Add_R_Limit(1,-1)

--- roll
au_roll_position              = CreateElement "ceTexPoly"
au_roll_position.name         = "au_roll_position"
au_roll_position.vertices     = {{-au_ind_index_sz, -au_ind_line_width},
								{-au_ind_index_sz,  au_ind_line_width},
								{ au_ind_index_sz,  au_ind_line_width},
								{ au_ind_index_sz, -au_ind_line_width}}
au_roll_position.indices      = default_box_indices
au_roll_position.material	  = "CONTROLS_INDICATOR_WHITE"
au_roll_position.tex_params	  = {0.26,176.5/512},{0.74,176.5/512}, {0.5/size, 1.5/size}
au_roll_position.controllers  = {{"autopilot_value",autopilot_height*0.8,1}}
au_roll_position.init_pos     = {0.01+(autopilot_len/4)/2+autopilot_len/4,0.0}
au_roll_position.parent_element = autopilot_base.name
AddElement(au_roll_position)

Add_B_30_limit(-1)
Add_B_30_limit(1)
Add_HP_Limit(1,-1,0)
Add_HP_Limit(1,1,0)


--- pitch
au_pitch_position             = CreateElement "ceTexPoly"
au_pitch_position.name         = "au_pitch_position"
au_pitch_position.vertices     = {{-au_ind_index_sz, -au_ind_line_width},
								{-au_ind_index_sz,  au_ind_line_width},
								{ au_ind_index_sz,  au_ind_line_width},
								{ au_ind_index_sz, -au_ind_line_width}}
au_pitch_position.indices      = default_box_indices
au_pitch_position.material	  = "CONTROLS_INDICATOR_WHITE"
au_pitch_position.tex_params	  = {0.26,176.5/512},{0.74,176.5/512}, {0.5/size, 1.5/size}
au_pitch_position.controllers  = {{"autopilot_value",(autopilot_len/4)*0.8/2,2}}
au_pitch_position.init_pos     = {0.01+(autopilot_len/4)/2+2*autopilot_len/4,0.0}
au_pitch_position.parent_element = autopilot_base.name
AddElement(au_pitch_position)

Add_HP_Limit(2,-1,-1)
Add_HP_Limit(2,-1,1)
Add_HP_Limit(2,-1,0)
Add_HP_Limit(2,1,0)
Add_HP_Limit(2,1,-1)
Add_HP_Limit(2,1,1)

--- height
au_height_position             = CreateElement "ceTexPoly"
au_height_position.name         = "au_height_position"
au_height_position.vertices     = {{-au_ind_index_sz, -au_ind_line_width},
								{-au_ind_index_sz,  au_ind_line_width},
								{ au_ind_index_sz,  au_ind_line_width},
								{ au_ind_index_sz, -au_ind_line_width}}
au_height_position.indices      = default_box_indices
au_height_position.material	  = "CONTROLS_INDICATOR_WHITE"
au_height_position.tex_params	  = {0.26,176.5/512},{0.74,176.5/512}, {0.5/size, 1.5/size}
au_height_position.controllers  = {{"autopilot_value",(autopilot_len/4)*0.8/2,3}}
au_height_position.init_pos     = {0.01+(autopilot_len/4)/2+3*autopilot_len/4,0.0}
au_height_position.parent_element = autopilot_base.name
AddElement(au_height_position)

Add_HP_Limit(3,-1,-1)
Add_HP_Limit(3,-1,1)
Add_HP_Limit(3,-1,0)
Add_HP_Limit(3,1,0)
Add_HP_Limit(3,1,-1)
Add_HP_Limit(3,1,1)

local chan_names = {"H","K","T","B"}

for i = 0,3 do
	txt_chan_name            	 = CreateElement "ceStringPoly"
	txt_chan_name.name       	 = "txt_chan_name"..i
	txt_chan_name.material       = "font_CONTROLS_INDICATOR_WHITE"
	txt_chan_name.init_pos		 = {0.01+i*autopilot_len/4,autopilot_height}
	txt_chan_name.alignment      = "LeftTop"
	txt_chan_name.stringdefs     = {0.0035, 0.0035,0,0}
	txt_chan_name.value          = chan_names[i+1]
	txt_chan_name.parent_element = autopilot_base.name
	txt_chan_name.isdraw 		 = true
	AddElement(txt_chan_name)
end

