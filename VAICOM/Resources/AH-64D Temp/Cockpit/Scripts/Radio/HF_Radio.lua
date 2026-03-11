dofile(LockOn_Options.common_script_path..'Radio.lua')
dofile(LockOn_Options.common_script_path.."mission_prepare.lua")

local gettext = require("i_18n")
_ = gettext.translate

device_timer_dt = 0.2

innerNoise 				= getInnerNoise(3.5E-6, 6.0)--V/m (dB S+N/N)
-- Frequency tuning accuracy
frequency_accuracy 		= 500.0				--Hz
-- Receiver bandwidth
band_width				= 16E3				--Hz 
-- Transmitter radiated power
power 					= 10.0				--Watts

-- Receiver frequency response filter quality (filter 'power', defines the freq response curve slopeness)
RxfreqResponseQuality 	= 10				--dimentionless
-- Transmitter spectrum power factor (defines the spectrum curve slopeness)
TxSpectrumPowerFactor  	= 8					--dimentionless
-- Transmitter spectrum bandwidth
TxBandwidth				= 7E3				--Hz

power_low           = 2.5		-- Watt (need to find out )
power_medium 		= 10.0		-- Watt (need to find out )
power_high 			= 40		-- Watt (need to find out )

presets = {}
presets[ 1] = 2000000.0
presets[ 2] = 2050000.0
presets[ 3] = 5000000.0
presets[ 4] = 5050000.0
presets[ 5] = 10000000.0
presets[ 6] = 10500000.0
presets[ 7] = 15000000.0
presets[ 8] = 20500000.0
presets[ 9] = 25500000.0
presets[10] = 29000000.0

agr = {
	input_signal_deviation		= rangeUtoDb(4E-6, 0.5), --Db
	output_signal_deviation		= 4,  --Db
	input_signal_linear_zone 	= 15.0, --Db
	regulation_time				= 0.25, --sec
}

GUI = {
	range = {min = 2E6, max = 29.9999E6, step = 0.1E3}, --Hz
	displayName = _('HF'),
	AM = true,
	FM = false
}

staticNoises = {
    {
        effect = {"Aircrafts/Cockpits/Static_2"},
    },
}

UHF_RADIO_FAILURE_TOTAL	= 0

need_to_be_closed = false -- close lua state after initialization 
