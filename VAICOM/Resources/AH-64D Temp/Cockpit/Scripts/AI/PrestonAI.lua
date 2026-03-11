long_press_time = 0.5
lag_time_for_ai_pilot = 1.0
course_change_low_speed = 10.0
alt_change_low_speed = 15.0
course_change_high_speed = 40.0
alt_change_high_speed = 50.0
course_alt_change_transition_time = 2.0
speed_change_time = 1.0

dofile(LockOn_Options.script_path.."AI/PrestonAI_sound.lua")
dofile(LockOn_Options.script_path.."AI/PrestonAI_reporting_names.lua")

min_angular_radius =
{
    lowres = 0.0045,
    medres = 0.012,
    hires = 0.027,
    iff = 0.031,
}

--aim_pid_wide = 
--{
--	aim_p = 10,
--	aim_d = 0,
--	aim_i = 15,
--	lim_i = 0.003,
--}
--
--aim_pid_other = 
--{
--	aim_p = 150,
--	aim_d = 0,
--	aim_i = 50,
--	lim_i = 0.01,
--}

aim_pid_wide = 
{
	aim_p = 0.35,
	aim_d = 0.0,
	aim_i = 0.01,
	lim_i = 0.2,
}

aim_pid_other = 
{
	aim_p = 6.0,
	aim_d = 0,
	aim_i = 1.5,
	lim_i = 0.6,
}

aim_pid_lmc = 
{
	ang_p = 10,
	max_vel = 120,
	
	vel_p = 0.075,
	vel_d = 0.075,
}

min_detailed_iff_time = 1.0
max_detailed_iff_time = 2.0

max_detailed_iff_chance = 1.0
min_detailed_iff_chance = 0.5

iff_horoshiy = true

min_fog_transparency = 0.15

text_color_red			= {"FF3333", "DB4325", "FFFFFF", "FFFF00"}
text_color_green		= {"00FF00", "01D4A8", "FFFFFF", "FFFF00"}
text_color_blue			= {"3380FF", "3380FF", "FFFFFF", "FFFF00"}
text_color_yellow		= {"FFFF00", "DCD800", "FFFFFF", "FFFF00"}
text_color_white		= {"FFFFFF", "FFFFFF", "FFFFFF", "FFFF00"}
text_color_black		= {"000000", "000000", "000000", "000000"}

debug_graphics = false --won't work in release builds anyway

need_to_be_closed = true -- lua_state  will be closed in post_initialize()