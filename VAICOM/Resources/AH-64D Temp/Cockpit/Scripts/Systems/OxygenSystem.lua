local gettext = require("i_18n")
_ = gettext.translate

BreatheCyclesPerMinute	= 10

oxygen_pressure	= {valmin = 0, valmax = 200, T1 = 0.225, T2 = 0.112, bias = {{valmin = 0, valmax = 200, bias = 1.0}}}
flow_blinker	= {valmin = 0, valmax = 1, dvalue = 5.0}

O2_Starvation_limit		= 11000
StarvDeathTime			= 300

OXYGEN_FAILURE_TOTAL	= 0

Damage = {
	{Failure = OXYGEN_FAILURE_TOTAL, Failure_name = "oxy_FAILURE_TOTAL", Failure_editor_name = _("Oxygen System: Total Failure"),  Element = 4, Integrity_Treshold = 0.4, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300}	-- TODO: elem
}

need_to_be_closed = true -- close lua state after initialization
