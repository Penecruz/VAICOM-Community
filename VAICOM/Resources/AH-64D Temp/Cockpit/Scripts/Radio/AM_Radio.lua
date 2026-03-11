dofile(LockOn_Options.common_script_path..'Radio.lua')

local gettext = require("i_18n")
_ = gettext.translate

device_timer_dt = 0.2

innerNoise 			= getInnerNoise(3E-6, 6)--V/m (dB S+N/N)
innerNoise_108_116_MHz_coeff = 1.2
frequency_accuracy 	= 2000.0			--Hz
band_width			= 19E3				--Hz (6 dB selectivity)
power 				= 10.0				--Wt
NB_band_width = 9.5E3 -- Hz (Narrow Bandwidth)
WB_band_width = 18E3  -- Hz (Wide Bandwidth)

presets = {}
presets[ 1] = 127000000.0
presets[ 2] = 135000000.0
presets[ 3] = 136000000.0
presets[ 4] = 127000000.0
presets[ 5] = 125000000.0
presets[ 6] = 121000000.0
presets[ 7] = 141000000.0
presets[ 8] = 128000000.0
presets[ 9] = 126000000.0
presets[10] = 137000000.0

agr = {
	input_signal_deviation		= rangeUtoDb(4E-6, 0.5), --Db
	output_signal_deviation		= 4,  --Db
	input_signal_linear_zone 	= 15.0, --Db
	regulation_time				= 0.2, --sec
}

GUI = {
	range = {min = 108E6, max = 151.975E6, step = 25E3}, --Hz
	displayName = _('VHF AM'),
	AM = true,
	FM = false
}

VHF_RADIO_FAILURE_TOTAL	= 0

Damage = {	{Failure = VHF_RADIO_FAILURE_TOTAL, Failure_name = "VHF_AM_RADIO_FAILURE_TOTAL", Failure_editor_name = _("VHF AM radio total failure"),  Element = 59, Integrity_Treshold = 0.5, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300}}

need_to_be_closed = false -- close lua state after initialization 