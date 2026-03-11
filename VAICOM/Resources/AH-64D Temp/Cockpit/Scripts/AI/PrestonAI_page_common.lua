dofile(LockOn_Options.common_script_path.."elements_defs.lua")

function create_and_add_elements(weap_control_size, compass_size, is_vr)
--intentionally no indentation
--for VR only!
local compass_pos 		= {0,	-0.40}
local weap_control_pos 	= {0,	0.32} --seems okay, may be a subject to change
if is_vr then
	SetCustomScale(1.0)
else
	SetScale(FOV)
	local total_aspect  = LockOn_Options.screen.aspect
	local total_w 		= LockOn_Options.screen.width
	local total_h 		= LockOn_Options.screen.height
	local start_x,start_y,main_w,main_h,gui_scale = get_UIMainView()

	total_w = total_w*gui_scale
	total_h = total_h*gui_scale

	compass_pos = {-0.82, -0.7}
	weap_control_pos = {0.8, -0.7}
	--new finally working viewports stuff (this time it does)
	--sorry dear players for a very late fix
	--kept you waiting huh
	if main_w ~= total_w or total_h ~= main_h or start_x ~= 0 or start_y ~= 0 then
		local function pix2rel(pix, total, inv) return (2*pix/total - 1) end
		local function rel2pix(rel, total, inv) return (1 + rel)*total/2 end
		local function transform_rel(total, main, start, inp) return pix2rel(start + rel2pix(inp, main), total) end
		compass_pos = {transform_rel(total_w, main_w, start_x, compass_pos[1]) * total_aspect, -transform_rel(total_h, main_h, start_y, -compass_pos[2])}
		weap_control_pos = {transform_rel(total_w, main_w, start_x, weap_control_pos[1]) * total_aspect, -transform_rel(total_h, main_h, start_y, -weap_control_pos[2])}
		compass_size = compass_size * main_h / total_h
		weap_control_size = weap_control_size * main_h / total_h
	else
		compass_pos = {compass_pos[1] * total_aspect, compass_pos[2]}
		weap_control_pos = {weap_control_pos[1] * total_aspect, weap_control_pos[2]}
	end
	--end viewports stuff
end

local former_size = 2400
local former_scale_multipiler = 4

local compass_size_adj = compass_size * former_scale_multipiler
local weap_control_size_adj = weap_control_size * former_scale_multipiler
local txt_box_size = compass_size_adj * 1.15

local text_frame_width = compass_size * 0.6
local text_frame_height = compass_size * 0.12

local list_arrow_width = compass_size * 0.060
local list_arrow_height = compass_size * 0.108
local list_text_x_offset = -text_frame_width * 0.37
local list_arrow_x_offset = -text_frame_width  * 0.04 --relative to list (middle entry) center

local list_red_arrow_width = list_arrow_width * 14 / 10
local list_red_arrow_height = list_arrow_height
local list_red_arrow_x_offset = list_arrow_x_offset - text_frame_width * 0.02 --relative to list (middle entry) center
local list_red_arrow_text_offset = { list_red_arrow_width * -0.15, list_arrow_height * 0.05 }

local weap_text_radius_near_hor = 0.23 * weap_control_size / 0.7
local weap_text_radius_near_vert = 0.21 * weap_control_size / 0.7
local weap_text_radius_far_hor = 0.44 * weap_control_size / 0.7
local weap_text_radius_far_vert = 0.40 * weap_control_size / 0.7
--local weap_text_extra_pic_pos = 0.35 * weap_control_size / 0.7

local caption_scale_weap_control
local caption_scale_compass
if not is_vr then
    caption_scale_weap_control = weap_control_size * 0.17
    caption_scale_compass = compass_size  * 0.12
else
    caption_scale_weap_control = weap_control_size * 2.2
    caption_scale_compass = compass_size  * 1.2
end

local VR_rotate_angle_Z_bot = 72.0
local VR_rotate_angle_Z_top = -60.0
local VR_add_height = compass_size * 0.06

local list_text_scale_compass = caption_scale_compass * 0.58
local list_text_scale_weap_control = caption_scale_weap_control * 0.7

local text_defs_compass = {list_text_scale_compass * 0.05, list_text_scale_compass * 0.05, 0, 0}
local text_defs_weap_control = {list_text_scale_weap_control * 0.05, list_text_scale_weap_control * 0.05, 0, 0}

ARIAL_TABLE  = {class  = "ceUITTF",  ttf = "Arial.ttf" , size = 20}
local arial_font   = MakeFont(ARIAL_TABLE,{255,255,255,255})

SEGOE_TABLE  = {class  = "ceUITTF",  ttf = "seguisym.ttf" , size = 30}
local segoe_font   = MakeFont(SEGOE_TABLE,{255,255,255,255})

local texture_w = 1024
local texture_h = 1024
local function minmax_to_coords(mm_table)
	return
	{
		{mm_table.min_x / texture_w, mm_table.max_y / texture_h},
		{mm_table.max_x / texture_w, mm_table.max_y / texture_h},
		{mm_table.max_x / texture_w, mm_table.min_y / texture_h},
		{mm_table.min_x / texture_w, mm_table.min_y / texture_h},
	}
end

local compass_center_tex_pos =
{
	min_x = 472,
	max_x = 498,
	min_y = 32,
	max_y = 70,
}

local compass_base_tex_pos =
{
	min_x = 24,
	max_x = 384,
	min_y = 192,
	max_y = 552,
}

local compass_scale_tex_pos =
{
	min_x = 424,
	max_x = 760,
	min_y = 204,
	max_y = 540,
}

local compass_waypoint_tex_pos =
{
	min_x = 392,
	max_x = 426,
	min_y = 32,
	max_y = 61,
}

local compass_waypoint_y_offset = 350

local compass_arrow_tex_pos =
{
	min_x = 288,
	max_x = 342,
	min_y = 11,
	max_y = 78,
}

local compass_arrow_y_offset = 560

local yellow_box_tex_pos =
{
	min_x = 16,
	max_x = 96,
	min_y = 16,
	max_y = 56,
}

local white_box_tex_pos =
{
	min_x = 136,
	max_x = 216,
	min_y = 16,
	max_y = 56,
}

local weap_circle_tex_pos =
{
	min_x = 472,
	max_x = 718,
	min_y = 664,
	max_y = 910,
}

local list_bg_tex_pos =
{
	min_x = 792,
	max_x = 984,
	min_y = 688,
	max_y = 880,
}

--local list_arrow_tex_pos =
--{
--	min_x = 552,
--	max_x = 568,
--	min_y = 32,
--	max_y = 47,
--}

local list_arrow_tex_pos =
{
	min_x = 588,
	max_x = 599,
	min_y = 31,
	max_y = 50,
}

local list_red_arrow_tex_pos =
{
	min_x = 614,
	max_x = 629,
	min_y = 31,
	max_y = 50,
}

local box_x_offset = 540 --* 1.01
local box_y_offset_down = -765 --* 0.96
local box_y_offset_up = 765 * 0.99
local box_y_offset_big = 984 --* 0.98

--IIOD

local function simple_rectangle(wid,hei)
	return	{
				{-wid/2,hei/2},
				{wid/2,hei/2},
				{wid/2,-hei/2},
				{-wid/2,-hei/2},
			}
end

local function simple_square(size)
	return simple_rectangle(size,size)
end

local function less_simple_rectangle(former, scale, wid, hei)
	return	{
				{-wid / 2 / former * scale,	-hei / 2 / former * scale},
				{wid / 2 / former * scale,	-hei / 2 / former * scale},
				{wid / 2 / former * scale,	hei / 2 / former * scale},
				{-wid / 2 / former * scale,	hei / 2 / former * scale},
			}
end

local function only_offset(former, scale, offset_pix_x, offset_pix_y)
	return {offset_pix_x / former * scale / former_scale_multipiler, offset_pix_y / former * scale / former_scale_multipiler}
end

local function less_simple_rectangle_with_offset(former, scale, wid, hei, offset_pix_x, offset_pix_y, scale_size)
	local no_offset = less_simple_rectangle(former, scale_size or scale, wid, hei)
	local offset = only_offset(former, scale, offset_pix_x, offset_pix_y)
	return	{
				{no_offset[1][1] + offset[1],	no_offset[1][2] + offset[2]},
				{no_offset[2][1] + offset[1],	no_offset[2][2] + offset[2]},
				{no_offset[3][1] + offset[1],	no_offset[3][2] + offset[2]},
				{no_offset[4][1] + offset[1],	no_offset[4][2] + offset[2]},
			}
end



local function get_w(mm_table)
	return mm_table.max_x - mm_table.min_x
end

local function get_h(mm_table)
	return mm_table.max_y - mm_table.min_y
end

local function get_size(mm_table)
	return get_w(mm_table), get_h(mm_table)
end

local function AddElement(elem)
	if not is_vr then
		elem.screenspace		= ScreenType.SCREENSPACE_TRUE
	end
	Add(elem)
end

local common_mat = MakeMaterial("Mods/aircraft/AH-64D/Cockpit/IndicationResources/AI/ai_combined.dds",{255,255,255,255})

local 	base_					= CreateElement "ceSimple"
		base_.controllers 		= {{"vr_size_control",compass_pos[2]}}
AddElement(base_)


--PilotAI
local compass_center			= CreateElement "ceTexPoly"
compass_center.name				= "compass_center"
compass_center.material			= common_mat
compass_center.vertices			= { --special case
									{-get_w(compass_center_tex_pos) / 2 / former_size * compass_size_adj,	-get_h(compass_center_tex_pos) * 25/38 / former_size * compass_size_adj},
									{get_w(compass_center_tex_pos) / 2 / former_size * compass_size_adj,		-get_h(compass_center_tex_pos) * 25/38 / former_size * compass_size_adj},
									{get_w(compass_center_tex_pos) / 2 / former_size * compass_size_adj,		get_h(compass_center_tex_pos) * 13/38 / former_size * compass_size_adj},
									{-get_w(compass_center_tex_pos) / 2 / former_size * compass_size_adj,	get_h(compass_center_tex_pos) * 13/38 / former_size * compass_size_adj}
								}
compass_center.init_pos			= compass_pos
compass_center.tex_coords		= minmax_to_coords(compass_center_tex_pos)
compass_center.indices			= default_box_indices
compass_center.parent_element	= base_.name
if not is_vr then
	compass_center.controllers	= {{"show_compass"}}
else
	compass_center.controllers	= {{"show_compass_VR", VR_rotate_angle_Z_bot}}
end
AddElement(compass_center)

local compass_base				= CreateElement "ceTexPoly"
compass_base.name				= "compass_base"
compass_base.material			= common_mat
compass_base.parent_element 	= compass_center.name
compass_base.vertices			= less_simple_rectangle(former_size, compass_size_adj, get_size(compass_base_tex_pos))
compass_base.tex_coords			= minmax_to_coords(compass_base_tex_pos)
compass_base.indices			= default_box_indices
compass_base.controllers		= {{"rotate_compass_base"}}
AddElement(compass_base)

local compass_scale				= CreateElement "ceTexPoly"
compass_scale.name				= "compass_scale"
compass_scale.material			= common_mat
compass_scale.parent_element 	= compass_center.name
compass_scale.vertices			= less_simple_rectangle(former_size, compass_size_adj, get_size(compass_scale_tex_pos))
compass_scale.tex_coords		= minmax_to_coords(compass_scale_tex_pos)
compass_scale.indices			= default_box_indices
compass_scale.controllers		= {{"rotate_compass_scale"}}
AddElement(compass_scale)

local compass_waypoint				= CreateElement "ceTexPoly"
compass_waypoint.name				= "compass_waypoint"
compass_waypoint.material			= common_mat
compass_waypoint.parent_element 	= compass_center.name
compass_waypoint.vertices			= less_simple_rectangle_with_offset(former_size, compass_size_adj, get_w(compass_waypoint_tex_pos), get_h(compass_waypoint_tex_pos), 0, compass_waypoint_y_offset)
compass_waypoint.tex_coords			= minmax_to_coords(compass_waypoint_tex_pos)
compass_waypoint.indices			= default_box_indices
compass_waypoint.controllers		= {{"rotate_compass_waypoint"}}
AddElement(compass_waypoint)


local compass_arrow				= CreateElement "ceTexPoly"
compass_arrow.name				= "compass_arrow"
compass_arrow.parent_element 	= compass_center.name
compass_arrow.material			= common_mat
compass_arrow.vertices			= less_simple_rectangle_with_offset(former_size, compass_size_adj, get_w(compass_arrow_tex_pos), get_h(compass_arrow_tex_pos), 0, compass_arrow_y_offset)
compass_arrow.tex_coords		= minmax_to_coords(compass_arrow_tex_pos)
compass_arrow.indices			= default_box_indices
compass_arrow.controllers		= {{"rotate_compass_arrow"}}
AddElement(compass_arrow)

local box_alt				= CreateElement "ceTexPoly"
box_alt.name				= "box_alt"
box_alt.parent_element 		= compass_center.name
box_alt.material			= common_mat
box_alt.init_pos 			= only_offset(former_size, compass_size_adj, box_x_offset, box_y_offset_up)
box_alt.vertices			= less_simple_rectangle(former_size, txt_box_size, get_size(white_box_tex_pos))
box_alt.tex_coords			= minmax_to_coords(white_box_tex_pos)
box_alt.indices				= default_box_indices
if is_vr then
	box_alt.controllers 		= {{'rotate_boxes_vr', VR_rotate_angle_Z_bot, VR_add_height}}
end
AddElement(box_alt)

local alt_text 			= CreateElement "ceStringPoly"
alt_text.name 			= "alt_text"
alt_text.parent_element = box_alt.name
alt_text.material 		= arial_font
--alt_text.init_pos 		= only_offset(former_size, compass_size_adj, box_x_offset, box_y_offset_up)
alt_text.stringdefs 	= text_defs_compass
alt_text.alignment 		= "CenterCenter"
alt_text.controllers 	= {{'alt_text'--[[, VR_rotate_angle_Z, VR_add_height]]}}
AddElement(alt_text)

local alt_text_top 			= CreateElement "ceStringPoly"
alt_text_top.name 			= "alt_text_top"
alt_text_top.material 		= arial_font
alt_text_top.parent_element	= box_alt.name
alt_text_top.init_pos 		= {0, compass_size * 0.065}
alt_text_top.stringdefs 	= text_defs_compass
alt_text_top.alignment 		= "CenterCenter"
alt_text_top.value 			= "ALT"
AddElement(alt_text_top)

local alt_add_text 			= CreateElement "ceStringPoly"
alt_add_text.name 			= "alt_add_text"
alt_add_text.material 		= arial_font
alt_add_text.parent_element	= box_alt.name
alt_add_text.init_pos 		= {compass_size * 0.11, 0}
alt_add_text.stringdefs 	= text_defs_compass
alt_add_text.alignment 		= "CenterCenter"
alt_add_text.controllers 	= {{'alt_add_text',--[[, VR_rotate_angle_Z, VR_add_height]]}}
AddElement(alt_add_text)

local alt_add_text_u 			= CreateElement "ceStringPoly"
alt_add_text_u.name 			= "alt_add_text_u"
alt_add_text_u.material 		= arial_font
alt_add_text_u.parent_element	= alt_add_text.name
alt_add_text_u.init_pos 		= {0, 0}
alt_add_text_u.stringdefs 		= text_defs_compass
alt_add_text_u.alignment 		= "CenterCenter"
alt_add_text_u.value 			= "_"
alt_add_text_u.controllers 		= {{'alt_add_text_u',--[[, VR_rotate_angle_Z, VR_add_height]]}}
AddElement(alt_add_text_u)

local box_speed				= CreateElement "ceTexPoly"
box_speed.name				= "box_speed"
box_speed.parent_element 	= compass_center.name
box_speed.material			= common_mat
box_speed.init_pos 			= only_offset(former_size, compass_size_adj, -box_x_offset, box_y_offset_up)
box_speed.vertices			= less_simple_rectangle(former_size, txt_box_size, get_size(white_box_tex_pos))
box_speed.tex_coords		= minmax_to_coords(white_box_tex_pos)
box_speed.indices			= default_box_indices
if is_vr then
	box_speed.controllers 		= {{'rotate_boxes_vr', VR_rotate_angle_Z_bot, VR_add_height}}
end
AddElement(box_speed)

local speed_text_value 			= CreateElement "ceStringPoly"
speed_text_value.name 			= "speed_text_value"
speed_text_value.parent_element	= box_speed.name
speed_text_value.material 		= arial_font
--speed_text_value.init_pos 		= only_offset(former_size, compass_size_adj, -box_x_offset, box_y_offset_up)
speed_text_value.stringdefs 		= text_defs_compass
speed_text_value.alignment 		= "CenterCenter"
speed_text_value.controllers 	= {{'speed_text'--[[, VR_rotate_angle_Z, VR_add_height]]}}
AddElement(speed_text_value)

local speed_text_label 			= CreateElement "ceStringPoly"
speed_text_label.name 			= "speed_text_label"
speed_text_label.material 		= arial_font
speed_text_label.parent_element	= box_speed.name
speed_text_label.init_pos 		= {0, compass_size * 0.065}
speed_text_label.stringdefs 	= text_defs_compass
speed_text_label.alignment 		= "CenterCenter"
speed_text_label.controllers 	= {{'speed_label'}}
AddElement(speed_text_label)

local box_course				= CreateElement "ceTexPoly"
box_course.name				= "box_course"
box_course.parent_element 	= compass_center.name
box_course.material			= common_mat
box_course.init_pos 		= only_offset(former_size, compass_size_adj, 0, box_y_offset_big)
box_course.vertices			= less_simple_rectangle(former_size, txt_box_size, get_size(white_box_tex_pos))
box_course.tex_coords		= minmax_to_coords(white_box_tex_pos)
box_course.indices			= default_box_indices
if is_vr then
	box_course.controllers 		= {{'rotate_boxes_vr', VR_rotate_angle_Z_bot, VR_add_height}}
end
AddElement(box_course)

local course_text 			= CreateElement "ceStringPoly"
course_text.name 			= "course_text"
course_text.parent_element	= box_course.name
course_text.material 		= arial_font
--course_text.init_pos 		= only_offset(former_size, compass_size_adj, 0, box_y_offset_big)
course_text.stringdefs 	    = text_defs_compass
course_text.alignment 		= "CenterCenter"
course_text.controllers 	= {{'course_text'--[[, VR_rotate_angle_Z, VR_add_height]]}}
AddElement(course_text)

local box_mode				= CreateElement "ceTexPoly"
box_mode.name				= "box_mode"
box_mode.parent_element 	= compass_center.name
box_mode.material			= common_mat
if is_vr then
	box_mode.init_pos 		= {0, compass_size * 0.1}
	box_mode.vertices 		= less_simple_rectangle(former_size, compass_size_adj, get_size(white_box_tex_pos))
else
	box_mode.vertices		= less_simple_rectangle_with_offset(former_size, compass_size_adj, get_w(white_box_tex_pos), get_h(white_box_tex_pos), -box_x_offset, box_y_offset_down, txt_box_size)
end
box_mode.tex_coords			= minmax_to_coords(yellow_box_tex_pos)
box_mode.indices			= default_box_indices
AddElement(box_mode)

local mode_text 			= CreateElement "ceStringPoly"
mode_text.name 			= "mode_text"
mode_text.material 		= arial_font
if is_vr then
	mode_text.parent_element	= box_mode.name
else
	mode_text.parent_element	= compass_center.name
	mode_text.init_pos			= {compass_size * -0.225, compass_size * -0.315}
end
mode_text.stringdefs 	= text_defs_compass
mode_text.alignment 		= "CenterCenter"
mode_text.controllers 	= {{'mode_text'--[[, VR_rotate_angle, VR_add_height]]}}
AddElement(mode_text)

--CPG AI

local weap_contol			= CreateElement "ceTexPoly"
weap_contol.name			= "weap_contol"
weap_contol.material		= common_mat
weap_contol.vertices		= simple_square(weap_control_size)
weap_contol.init_pos		= weap_control_pos
weap_contol.tex_coords		= minmax_to_coords(weap_circle_tex_pos)
weap_contol.indices			= default_box_indices
--weap_contol.parent_element	= base_.name
if not is_vr then
	weap_contol.controllers	= {{"show_weap_control"}}
else
	weap_contol.controllers	= {{"show_weap_control_VR", VR_rotate_angle_Z_top}}
end
AddElement(weap_contol)

local weapon_center 			= CreateElement "ceStringPoly"
weapon_center.name 				= "weapon_center"
weapon_center.parent_element	= weap_contol.name
weapon_center.material			= arial_font
weapon_center.stringdefs 		= text_defs_weap_control
weapon_center.alignment 		= "CenterCenter"
weapon_center.controllers 		= {{'weapon_center'}, {'weapon_text_color'}}
AddElement(weapon_center)

local weapon_up 				= CreateElement "ceStringPoly"
weapon_up.name 					= "weapon_up"
weapon_up.parent_element		= weap_contol.name
weapon_up.material				= arial_font
weapon_up.stringdefs 			= text_defs_weap_control
weapon_up.init_pos				= {0, weap_text_radius_near_vert}
weapon_up.alignment 			= "CenterCenter"
weapon_up.controllers 			= {{'weapon_up'}, {'weapon_text_color'}}
AddElement(weapon_up)

local weapon_right 				= CreateElement "ceStringPoly"
weapon_right.name 				= "weapon_right"
weapon_right.parent_element		= weap_contol.name
weapon_right.material			= arial_font
weapon_right.stringdefs 		= text_defs_weap_control
weapon_right.init_pos			= {weap_text_radius_near_hor, 0}
weapon_right.alignment 			= "CenterCenter"
weapon_right.controllers 		= {{'weapon_right'}, {'weapon_text_color'}}
AddElement(weapon_right)

local weapon_down 				= CreateElement "ceStringPoly"
weapon_down.name 				= "weapon_down"
weapon_down.parent_element		= weap_contol.name
weapon_down.material			= arial_font
weapon_down.stringdefs 			= text_defs_weap_control
weapon_down.init_pos			= {0, -weap_text_radius_near_vert}
weapon_down.alignment 			= "CenterCenter"
weapon_down.controllers 		= {{'weapon_down'}, {'weapon_text_color'}}
AddElement(weapon_down)

local weapon_left 				= CreateElement "ceStringPoly"
weapon_left.name 				= "weapon_left"
weapon_left.parent_element		= weap_contol.name
weapon_left.material			= arial_font
weapon_left.stringdefs 			= text_defs_weap_control
weapon_left.init_pos			= {-weap_text_radius_near_hor, 0}
weapon_left.alignment 			= "CenterCenter"
weapon_left.controllers 		= {{'weapon_left'}, {'weapon_text_color'}}
AddElement(weapon_left)

local weapon_far_up 			= CreateElement "ceStringPoly"
weapon_far_up.name 				= "weapon_far_up"
weapon_far_up.parent_element	= weap_contol.name
weapon_far_up.material			= arial_font
weapon_far_up.stringdefs 		= text_defs_weap_control
weapon_far_up.init_pos			= {0, weap_text_radius_far_vert}
weapon_far_up.alignment 		= "CenterCenter"
weapon_far_up.controllers 		= {{'weapon_far_up'}, {'weapon_text_color'}}
AddElement(weapon_far_up)

local weapon_far_right 			= CreateElement "ceStringPoly"
weapon_far_right.name 			= "weapon_far_right"
weapon_far_right.parent_element	= weap_contol.name
weapon_far_right.material		= arial_font
weapon_far_right.stringdefs 	= text_defs_weap_control
weapon_far_right.init_pos		= {weap_text_radius_far_hor, 0}
weapon_far_right.alignment 		= "CenterCenter"
weapon_far_right.controllers 	= {{'weapon_far_right'}, {'weapon_text_color'}}
AddElement(weapon_far_right)

local weapon_far_down 			= CreateElement "ceStringPoly"
weapon_far_down.name 			= "weapon_far_down"
weapon_far_down.parent_element	= weap_contol.name
weapon_far_down.material		= arial_font
weapon_far_down.stringdefs 		= text_defs_weap_control
weapon_far_down.init_pos		= {0, -weap_text_radius_far_vert}
weapon_far_down.alignment 		= "CenterCenter"
weapon_far_down.controllers 	= {{'weapon_far_down'}, {'weapon_text_color'}}
AddElement(weapon_far_down)

local weapon_far_left 			= CreateElement "ceStringPoly"
weapon_far_left.name 			= "weapon_far_left"
weapon_far_left.parent_element	= weap_contol.name
weapon_far_left.material		= arial_font
weapon_far_left.stringdefs 		= text_defs_weap_control
weapon_far_left.init_pos		= {-weap_text_radius_far_hor, 0}
weapon_far_left.alignment 		= "CenterCenter"
weapon_far_left.controllers 	= {{'weapon_far_left'}, {'weapon_text_color'}}
AddElement(weapon_far_left)

local middle_list_holder 			= CreateElement "ceTexPoly"
middle_list_holder.name				= "middle_list_holder"
middle_list_holder.material			= common_mat
middle_list_holder.vertices			= simple_rectangle(text_frame_width, text_frame_height * 5)
middle_list_holder.init_pos			= compass_pos
--middle_list_holder.parent_element	= base_.name
middle_list_holder.tex_coords		= minmax_to_coords(list_bg_tex_pos)
middle_list_holder.indices			= default_box_indices
if not is_vr then
	middle_list_holder.controllers		= {{"show_list"}}
else
	middle_list_holder.controllers		= {{"show_list_VR"}}
end

AddElement(middle_list_holder)

local middle_list_text 			= CreateElement "ceStringPoly"
middle_list_text.name 			= "middle_list_text"
middle_list_text.parent_element = middle_list_holder.name
middle_list_text.material 		= arial_font
middle_list_text.stringdefs 	= text_defs_compass
middle_list_text.init_pos		= {list_text_x_offset, 0}
middle_list_text.alignment 		= "LeftCenter"
middle_list_text.controllers 	= {{'target_list_text', 0}}
AddElement(middle_list_text)

local upper_upper_list_text 			= CreateElement "ceStringPoly"
upper_upper_list_text.name 				= "upper_upper_list_text"
upper_upper_list_text.parent_element 	= middle_list_text.name
upper_upper_list_text.material 			= arial_font
upper_upper_list_text.stringdefs 		= text_defs_compass
upper_upper_list_text.init_pos			= {0, text_frame_height*2}
upper_upper_list_text.alignment 		= "LeftCenter"
upper_upper_list_text.controllers 		= {{'target_list_text', -2}}
AddElement(upper_upper_list_text)

local upper_list_text 					= CreateElement "ceStringPoly"
upper_list_text.name 					= "upper_list_text"
upper_list_text.parent_element 			= middle_list_text.name
upper_list_text.material 				= arial_font
upper_list_text.stringdefs 				= text_defs_compass
upper_list_text.init_pos				= {0, text_frame_height}
upper_list_text.alignment 				= "LeftCenter"
upper_list_text.controllers 			= {{'target_list_text', -1}}
AddElement(upper_list_text)

local lower_list_text 					= CreateElement "ceStringPoly"
lower_list_text.name 					= "lower_list_text"
lower_list_text.parent_element 			= middle_list_text.name
lower_list_text.material 				= arial_font
lower_list_text.stringdefs 				= text_defs_compass
lower_list_text.init_pos				= {0, -text_frame_height}
lower_list_text.alignment 				= "LeftCenter"
lower_list_text.controllers 			= {{'target_list_text', 1}}
AddElement(lower_list_text)

local lower_lower_list_text 			= CreateElement "ceStringPoly"
lower_lower_list_text.name 				= "lower_lower_list_text"
lower_lower_list_text.parent_element 	= middle_list_text.name
lower_lower_list_text.material 			= arial_font
lower_lower_list_text.stringdefs 		= text_defs_compass
lower_lower_list_text.init_pos			= {0, -text_frame_height * 2}
lower_lower_list_text.alignment 		= "LeftCenter"
lower_lower_list_text.controllers 		= {{'target_list_text', 2}}
AddElement(lower_lower_list_text)

local middle_list_red_arrow 				= CreateElement "ceTexPoly"
middle_list_red_arrow.name 					= "middle_list_red_arrow"
middle_list_red_arrow.parent_element 		= middle_list_text.name
middle_list_red_arrow.material 				= common_mat
middle_list_red_arrow.vertices				= simple_rectangle(list_red_arrow_width, list_red_arrow_height)
middle_list_red_arrow.init_pos				= {list_red_arrow_x_offset, 0}
middle_list_red_arrow.tex_coords			= minmax_to_coords(list_red_arrow_tex_pos)
middle_list_red_arrow.indices				= default_box_indices
middle_list_red_arrow.controllers 			= {{'list_red_arrow', 0}}
AddElement(middle_list_red_arrow)

local middle_list_red_arrow_text 			= CreateElement "ceStringPoly"
middle_list_red_arrow_text.name				= "middle_list_red_arrow_text"
middle_list_red_arrow_text.parent_element 	= middle_list_red_arrow.name
middle_list_red_arrow_text.material 		= arial_font
middle_list_red_arrow_text.stringdefs 		= text_defs_compass
middle_list_red_arrow_text.alignment 		= "CenterCenter"
middle_list_red_arrow_text.controllers 		= {{'list_red_arrow_text', 0}}
middle_list_red_arrow_text.init_pos			= list_red_arrow_text_offset
AddElement(middle_list_red_arrow_text)

local upper_upper_list_red_arrow 				= CreateElement "ceTexPoly"
upper_upper_list_red_arrow.name 				= "upper_upper_list_red_arrow"
upper_upper_list_red_arrow.parent_element 		= upper_upper_list_text.name
upper_upper_list_red_arrow.material 			= common_mat
upper_upper_list_red_arrow.vertices				= simple_rectangle(list_red_arrow_width, list_red_arrow_height)
upper_upper_list_red_arrow.init_pos				= {list_red_arrow_x_offset, 0}
upper_upper_list_red_arrow.tex_coords			= minmax_to_coords(list_red_arrow_tex_pos)
upper_upper_list_red_arrow.indices				= default_box_indices
upper_upper_list_red_arrow.controllers 			= {{'list_red_arrow', -2}}
AddElement(upper_upper_list_red_arrow)

local upper_upper_list_red_arrow_text 			= CreateElement "ceStringPoly"
upper_upper_list_red_arrow_text.name			= "upper_upper_list_red_arrow_text"
upper_upper_list_red_arrow_text.parent_element 	= upper_upper_list_red_arrow.name
upper_upper_list_red_arrow_text.material 		= arial_font
upper_upper_list_red_arrow_text.stringdefs 		= text_defs_compass
upper_upper_list_red_arrow_text.alignment 		= "CenterCenter"
upper_upper_list_red_arrow_text.controllers 	= {{'list_red_arrow_text', -2}}
upper_upper_list_red_arrow_text.init_pos		= list_red_arrow_text_offset
AddElement(upper_upper_list_red_arrow_text)

local upper_list_red_arrow 				= CreateElement "ceTexPoly"
upper_list_red_arrow.name 				= "upper_list_red_arrow"
upper_list_red_arrow.parent_element 	= upper_list_text.name
upper_list_red_arrow.material 			= common_mat
upper_list_red_arrow.vertices			= simple_rectangle(list_red_arrow_width, list_red_arrow_height)
upper_list_red_arrow.init_pos			= {list_red_arrow_x_offset, 0}
upper_list_red_arrow.tex_coords			= minmax_to_coords(list_red_arrow_tex_pos)
upper_list_red_arrow.indices			= default_box_indices
upper_list_red_arrow.controllers 		= {{'list_red_arrow', -1}}
AddElement(upper_list_red_arrow)

local upper_list_red_arrow_text 			= CreateElement "ceStringPoly"
upper_list_red_arrow_text.name				= "upper_list_red_arrow_text"
upper_list_red_arrow_text.parent_element 	= upper_list_red_arrow.name
upper_list_red_arrow_text.material 			= arial_font
upper_list_red_arrow_text.stringdefs 		= text_defs_compass
upper_list_red_arrow_text.alignment 		= "CenterCenter"
upper_list_red_arrow_text.controllers 		= {{'list_red_arrow_text', -1}}
upper_list_red_arrow_text.init_pos			= list_red_arrow_text_offset
AddElement(upper_list_red_arrow_text)

local lower_list_red_arrow 				= CreateElement "ceTexPoly"
lower_list_red_arrow.name 				= "lower_list_red_arrow"
lower_list_red_arrow.parent_element 	= lower_list_text.name
lower_list_red_arrow.material 			= common_mat
lower_list_red_arrow.vertices			= simple_rectangle(list_red_arrow_width, list_red_arrow_height)
lower_list_red_arrow.init_pos			= {list_red_arrow_x_offset, 0}
lower_list_red_arrow.tex_coords			= minmax_to_coords(list_red_arrow_tex_pos)
lower_list_red_arrow.indices			= default_box_indices
lower_list_red_arrow.controllers 		= {{'list_red_arrow', 1}}
AddElement(lower_list_red_arrow)

local lower_list_red_arrow_text 			= CreateElement "ceStringPoly"
lower_list_red_arrow_text.name				= "lower_list_red_arrow_text"
lower_list_red_arrow_text.parent_element 	= lower_list_red_arrow.name
lower_list_red_arrow_text.material 			= arial_font
lower_list_red_arrow_text.stringdefs 		= text_defs_compass
lower_list_red_arrow_text.alignment 		= "CenterCenter"
lower_list_red_arrow_text.controllers 		= {{'list_red_arrow_text', 1}}
lower_list_red_arrow_text.init_pos			= list_red_arrow_text_offset
AddElement(lower_list_red_arrow_text)

local lower_lower_list_red_arrow 				= CreateElement "ceTexPoly"
lower_lower_list_red_arrow.name 				= "lower_lower_list_red_arrow"
lower_lower_list_red_arrow.parent_element 		= lower_lower_list_text.name
lower_lower_list_red_arrow.material 			= common_mat
lower_lower_list_red_arrow.vertices				= simple_rectangle(list_red_arrow_width, list_red_arrow_height)
lower_lower_list_red_arrow.init_pos				= {list_red_arrow_x_offset, 0}
lower_lower_list_red_arrow.tex_coords			= minmax_to_coords(list_red_arrow_tex_pos)
lower_lower_list_red_arrow.indices				= default_box_indices
lower_lower_list_red_arrow.controllers 			= {{'list_red_arrow', 2}}
AddElement(lower_lower_list_red_arrow)

local lower_lower_list_red_arrow_text 			= CreateElement "ceStringPoly"
lower_lower_list_red_arrow_text.name			= "lower_lower_list_red_arrow_text"
lower_lower_list_red_arrow_text.parent_element 	= lower_lower_list_red_arrow.name
lower_lower_list_red_arrow_text.material 		= arial_font
lower_lower_list_red_arrow_text.stringdefs 		= text_defs_compass
lower_lower_list_red_arrow_text.alignment 		= "CenterCenter"
lower_lower_list_red_arrow_text.controllers 	= {{'list_red_arrow_text', 2}}
lower_lower_list_red_arrow_text.init_pos		= list_red_arrow_text_offset
AddElement(lower_lower_list_red_arrow_text)


local middle_list_arrow 			= CreateElement "ceTexPoly"
middle_list_arrow.name				= "middle_list_arrow"
middle_list_arrow.parent_element 	= middle_list_text.name
middle_list_arrow.material			= common_mat
middle_list_arrow.vertices			= simple_rectangle(list_arrow_width, list_arrow_height)
middle_list_arrow.init_pos			= {list_arrow_x_offset, 0}
middle_list_arrow.tex_coords		= minmax_to_coords(list_arrow_tex_pos)
middle_list_arrow.indices			= default_box_indices
AddElement(middle_list_arrow)


end --function create_and_add_elements(cross_size, compass_size, is_vr)
