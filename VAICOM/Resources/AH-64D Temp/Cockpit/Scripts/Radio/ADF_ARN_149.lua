dofile(LockOn_Options.common_script_path..'Radio.lua')

local gettext = require("i_18n")
_ = gettext.translate

device_timer_dt = 0.1

signal_strength_Bias = {{valmin = 0.0, valmax = 1.0, bias = 0.01}}
signal_strength = {isLagElement = true, valmin = 0.0, valmax = 1.0, T1 = 0.2, bias = signal_strength_Bias}

--------------------------------
--receiver parameters
innerNoise			= getInnerNoise(2.5E-6, 6)	-- getInnerNoise(U, SNNRdB) { return U / (math.pow(10.0, SNNRdB / 20.0) - 1) }
frequency_accuracy 	= 100.0		--Hz
band_width			= 500.0		--Hz

--automatic gain regulator
agr = {
	input_signal_deviation		= 37.0, --Db
	output_signal_deviation		= 0.3,  --Db
	input_signal_linear_zone 	= 6.0,  --Db
	regulation_time				= 0.20, --sec
}

-----------------------------------------
goniometer = {isLagElement = true, T1 = 0.3, bias = {{valmin = math.rad(0), valmax = math.rad(360), bias = math.rad(0.5)}}}

accuracy = 2.0

mountainEffect = true
shorelineEffect = false --not implemented
nightEffect = true
-----------------------------------------

ARN_149_TOTAL_FAILURE	= 0
ARN_149_ANT_DAMAGE		= 1

Damage = {	
{Failure = ARN_149_TOTAL_FAILURE, Failure_name = "ARN_149_TOTAL_FAILURE", Failure_editor_name = _("ADF set total failure"),  Element = 56, Integrity_Treshold = 0.2, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
{Failure = ARN_149_ANT_DAMAGE, Failure_name = "ARN_149_ANT_DAMAGE", Failure_editor_name = _("ADF antenna damaged"),  Element = 56, Integrity_Treshold = 0.2, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
}

need_to_be_closed = true -- lua_state  will be closed in post_initialize()