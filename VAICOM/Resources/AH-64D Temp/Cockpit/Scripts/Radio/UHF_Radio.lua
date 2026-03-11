dofile(LockOn_Options.common_script_path..'Radio.lua')
dofile(LockOn_Options.common_script_path.."mission_prepare.lua")

local gettext = require("i_18n")
_ = gettext.translate

device_timer_dt = 0.2

innerNoise 				= getInnerNoise(2.5E-6, 6.0)--V/m (dB S+N/N)
-- Frequency tuning accuracy
frequency_accuracy 		= 500.0				--Hz
-- Receiver bandwidth
band_width				= 22E3				--Hz (3 dB selectivity, equivalent of 24KHz for 6 dB selectivity)
-- Transmitter radiated power
power 					= 10.0				--Watts

-- Receiver frequency response filter quality (filter 'power', defines the freq response curve slopeness)
RxfreqResponseQuality 	= 10				--dimentionless
-- Transmitter spectrum power factor (defines the spectrum curve slopeness)
TxSpectrumPowerFactor  	= 8					--dimentionless
-- Transmitter spectrum bandwidth
TxBandwidth				= 7E3				--Hz

NB_band_width = 12E3 -- Hz (Narrow Bandwidth)
WB_band_width = 35E3 -- Hz (Wide Bandwidth)

goniometer = {isLagElement = true, T1 = 0.3, bias = {{valmin = math.rad(0), valmax = math.rad(360), bias = math.rad(1)}}}

presets = {}
presets[ 1] = 225000000.0
presets[ 2] = 264000000.0
presets[ 3] = 265000000.0
presets[ 4] = 256000000.0
presets[ 5] = 254000000.0
presets[ 6] = 250000000.0
presets[ 7] = 270000000.0
presets[ 8] = 257000000.0
presets[ 9] = 255000000.0
presets[10] = 262000000.0
				
agr = {
	input_signal_deviation		= rangeUtoDb(4E-6, 0.5), --Db
	output_signal_deviation		= 4,  --Db
	input_signal_linear_zone 	= 15.0, --Db
	regulation_time				= 0.15, --sec
}

GUI = {
	range = {min = 225E6, max = 399.975E6, step = 25E3}, --Hz
	displayName = _('UHF'),
	AM = true,
	FM = false
}

staticNoises = {
    {
        effect = {"Aircrafts/Cockpits/Static_2"},
    },
}

UHF_RADIO_FAILURE_TOTAL	= 0

Damage = {	{Failure = UHF_RADIO_FAILURE_TOTAL, Failure_name = "UHF_RADIO_FAILURE_TOTAL", Failure_editor_name = _("UHF radio total failure"),  Element = 55, Integrity_Treshold = 0.25, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300}}

need_to_be_closed = false -- close lua state after initialization 
