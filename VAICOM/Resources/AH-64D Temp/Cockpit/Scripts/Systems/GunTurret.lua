need_to_be_closed = true -- close lua state after initialization
device_timer_dt		= 0.012

local gettext = require("i_18n")
_ = gettext.translate

argument_azimuth    = 19
argument_elevation  = 20


limits_azimuth   = {math.rad(-180),math.rad(180)}
limits_elevation = {math.rad(-90) ,math.rad(90)}

h_axis_velocity = math.rad(20.0)
v_axis_velocity = math.rad(20.0)

---- Damages
local failure_Lock	= 0
local failure_Mechanics	= 1

Damage =
{
	{ Failure = failure_Mechanics,	Failure_name = "failure_Mechanics",	Failure_editor_name = _("Gun failure"),	Element = 7, Integrity_Treshold = 0.0, Slope_param = 10.0, work_time_to_fail_probability = 0.0, work_time_to_fail = 3600*300 },
}

