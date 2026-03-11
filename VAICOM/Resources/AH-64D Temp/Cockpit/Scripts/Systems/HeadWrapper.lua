local gettext = require("i_18n")
_ = gettext.translate

---- Damages
local PLT_KILLED_FAILURE	= 0
local CPG_KILLED_FAILURE	= 1

Damage =
{
	{ Failure = PLT_KILLED_FAILURE,	Failure_name = "PLT_KILLED_FAILURE",	Failure_editor_name = _("Pilot Killed"),	Element = 90, Integrity_Treshold = 0.0, Slope_param = 10.0, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300 },
	{ Failure = CPG_KILLED_FAILURE,	Failure_name = "CPG_KILLED_FAILURE",	Failure_editor_name = _("CP/G Killed"),		Element = 91, Integrity_Treshold = 0.0, Slope_param = 10.0, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300 },
}

need_to_be_closed = true -- lua_state  will be closed in post_initialize()
