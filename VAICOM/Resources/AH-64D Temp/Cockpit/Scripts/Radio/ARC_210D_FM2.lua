dofile(LockOn_Options.common_script_path..'Radio.lua')
dofile(LockOn_Options.common_script_path.."mission_prepare.lua")

local gettext = require("i_18n")
_ = gettext.translate

device_timer_dt = 0.2

min_search_time = 0.0
max_search_time = 0.0

-- Receiver
innerNoise			= getInnerNoise(2.2E-6, 6)
frequency_accuracy 	= 40.0		--Hz
band_width			= 15000.0	--Hz 
power				= 10.0		-- Watt

presets = {}
presets[ 1] = 30000000.0
presets[ 2] = 30010000.0
presets[ 3] = 30015000.0
presets[ 4] = 30020000.0
presets[ 5] = 30025000.0
presets[ 6] = 30030000.0
presets[ 7] = 30035000.0
presets[ 8] = 30040000.0
presets[ 9] = 30045000.0
presets[10] = 30050000.0

agr = {
	input_signal_deviation		= 50.0, --Db
	output_signal_deviation		= 5.0,  --Db
	input_signal_linear_zone 	= 10.0, --Db
	regulation_time				= 0.08, --sec
}

GUI = {
	displayName = _('FM2: ARC-201D'),
	AM = false,
	FM = true,
}

staticNoises = {
    {
        effect = {"Aircrafts/Cockpits/Static_3"},
    },
}


need_to_be_closed = true -- close lua state after initialization 