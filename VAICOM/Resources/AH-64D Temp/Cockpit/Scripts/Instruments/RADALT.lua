-- Radar Altimeter
need_to_be_closed = true -- lua_state  will be closed in post_initialize()

local gettext = require("i_18n")
_ = gettext.translate

device_pos = {-2.65, -0.7, 0.08} -- [m]

distance_lim			= 1428.0	-- [ft]
distance_lim_precise	= 1850.0	-- [ft]

antenna_limits = {
	pitch_min = math.rad(-55.0),
	pitch_max = math.rad(55.0),
	roll_min = math.rad(-55.0),
	roll_max = math.rad(55.0)
}

WarmUpTime = 20.0		-- [sec]
SearchTime = 0.5		-- [sec]

-- Failures
RALT_FAILURE_TOTAL		= 0
RALT_FAILURE_RT			= 1
RALT_FAILURE_ANTENNA	= 2

Damage =
{
	{Failure = RALT_FAILURE_TOTAL,		Failure_name = "RALT_FAILURE_TOTAL",	Failure_editor_name = _("Radar altimeter total failure"),			Element = 1, Integrity_Treshold = 0.5, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
	{Failure = RALT_FAILURE_RT,			Failure_name = "RALT_FAILURE_RT",		Failure_editor_name = _("Radar altimeter receiver-transmitter"),	Element = 1, Integrity_Treshold = 0.5, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
	{Failure = RALT_FAILURE_ANTENNA,	Failure_name = "RALT_FAILURE_ANTENNA",	Failure_editor_name = _("Radar altimeter antenna pair"),			Element = 1, Integrity_Treshold = 0.5, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
}