dofile(LockOn_Options.common_script_path..'AN_ALR_SymbolsBase.lua')

local gettext = require("i_18n")
_ = gettext.translate

device_timer_dt     = 0.2
MaxThreats          = 10
EmitterLiveTime     = 11
EmitterSoundTime    = 0.5
LaunchSoundDelay    = 15.0

RWR_detection_coeff = 0.85

SoundsPath = "Aircrafts/Cockpits/RWR/"
DefaultSearchSnd = SoundsPath .. "SearchNewUS"

-- sounds for radars
acquisition_sounds = 
{
--    {EWR_1L13_,             DefaultSearchSnd},
--    {EWR_55G6_,             DefaultSearchSnd},
--	{S300PS_SR_5N66M,       DefaultSearchSnd},
--    {S300PS_SR_64H6E,       DefaultSearchSnd},
--    {RLO_9C15MT_,           DefaultSearchSnd},
--    {RLO_9C19M2_,           DefaultSearchSnd},
--    {Buk_SR_9S18M1,         DefaultSearchSnd},
--    {Kub_STR_9S91,          DefaultSearchSnd},
--    {Dog_Ear,               DefaultSearchSnd},
--    {Roland_rdr,            DefaultSearchSnd},
--    {Patriot_STR_ANMPQ_53,  DefaultSearchSnd},
--    {Hawk_SR_ANMPQ_50,      DefaultSearchSnd},
--    {S300PS_TR_30N6,        DefaultSearchSnd},
--    {RLS_5H63C_,            DefaultSearchSnd},
--    {RLS_9C32_1_,           DefaultSearchSnd},
--    {Hawk_TR_ANMPQ_46,      DefaultSearchSnd},
--    {S300V_9A82_,           DefaultSearchSnd},
--    {S300V_9A83,            DefaultSearchSnd},
--    {Buk_LN_9A310M1,        DefaultSearchSnd},
--    {BUK_LL_,               DefaultSearchSnd},
--    {Osa_9A33,              DefaultSearchSnd},
--    {Tor_9A331,             DefaultSearchSnd},
--    {Roland_ADS,            DefaultSearchSnd},
--    {Tunguska_2S6,          DefaultSearchSnd},
--    {ZSU_23_4_Shilka,       DefaultSearchSnd},
--    {Gepard,                DefaultSearchSnd},
--    {Vulcan_M163,           DefaultSearchSnd},
--	{S125_SR_P_19,          DefaultSearchSnd},
--    {S125_TR_SNR,         	DefaultSearchSnd},
--	{DEFAULT_TYPE_,         DefaultSearchSnd},
}

DefaultLockSnd = SoundsPath .. "LockNewUS"

lock_sounds = 
{
--    {EWR_1L13_,             DefaultLockSnd},
--    {EWR_55G6_,             DefaultLockSnd},
--    {S300PS_SR_5N66M,       DefaultLockSnd},
--    {S300PS_SR_64H6E,       DefaultLockSnd},
--    {RLO_9C15MT_,           DefaultLockSnd},
--    {RLO_9C19M2_,           DefaultLockSnd},
--    {Buk_SR_9S18M1,         DefaultLockSnd},
--    {Kub_STR_9S91,          DefaultLockSnd},
--    {Dog_Ear,               DefaultLockSnd},
--    {Roland_rdr,            DefaultLockSnd},
--    {Patriot_STR_ANMPQ_53,  DefaultLockSnd},
--    {Hawk_SR_ANMPQ_50,      DefaultLockSnd},
--    {S300PS_TR_30N6,        DefaultLockSnd},
--    {RLS_5H63C_,            DefaultLockSnd},
--    {RLS_9C32_1_,           DefaultLockSnd},
--    {Hawk_TR_ANMPQ_46,      DefaultLockSnd},
--    {S300V_9A82_,           DefaultLockSnd},
--    {S300V_9A83,            DefaultLockSnd},
--    {Buk_LN_9A310M1,        DefaultLockSnd},
--    {BUK_LL_,               DefaultLockSnd},
--    {Osa_9A33,              DefaultLockSnd},
--    {Tor_9A331,             DefaultLockSnd},
--    {Roland_ADS,            DefaultLockSnd},
--    {Tunguska_2S6,          DefaultLockSnd},
--    {ZSU_23_4_Shilka,       DefaultLockSnd},
--    {Gepard,                DefaultLockSnd},
--    {Vulcan_M163,           DefaultLockSnd},
--	{S125_SR_P_19,          DefaultLockSnd},
--    {S125_TR_SNR,         	DefaultLockSnd},
--    {DEFAULT_TYPE_,         DefaultLockSnd},    
}

-- RWR sensors: AH64 has four sensors 
eyes ={}

eyes[1] =
{
    position      = {x = 4.3,y = 0.56,z =  0.71},
    orientation   = {azimuth  = math.rad(45),elevation = math.rad(0.0)},
    field_of_view = math.rad(120) 
}
eyes[2] =
{
    position      = {x = 4.3,y = 0.56,z = -0.71},
    orientation   = {azimuth  = math.rad(-45),elevation = math.rad(0.0)},
    field_of_view = math.rad(120) 
}
eyes[3] =
{
    position      = {x = -9.71,y = 2.72,z =  0.093},
    orientation   = {azimuth  = math.rad(135),elevation = math.rad(0.0)},
    field_of_view = math.rad(120) 
}
eyes[4] =
{
    position      = {x = -9.71,y = 2.72,z =  -0.093},
    orientation   = {azimuth  = math.rad(-135),elevation = math.rad(0.0)},
    field_of_view = math.rad(120) 
}
-- Sounds
device_timer_dt = 0.05

local short_delay	= 0.5	-- [sec]
local long_delay	= 1.5	-- [sec]

RWR =
{
	path = "Aircrafts/AH-64D/Cockpit/",

	messages =
	{
		{ msg = "Betty/ASE LINES NORMAL_12_SEARCHING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_01_SEARCHING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_02_SEARCHING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_03_SEARCHING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_04_SEARCHING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_05_SEARCHING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_06_SEARCHING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_07_SEARCHING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_08_SEARCHING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_09_SEARCHING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_10_SEARCHING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_11_SEARCHING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_12_TRACKING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_01_TRACKING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_02_TRACKING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_03_TRACKING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_04_TRACKING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_05_TRACKING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_06_TRACKING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_07_TRACKING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_08_TRACKING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_09_TRACKING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_10_TRACKING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_11_TRACKING",			priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_12_LAUNCH",				priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_01_LAUNCH",				priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_02_LAUNCH",				priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_03_LAUNCH",				priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_04_LAUNCH",				priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_05_LAUNCH",				priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_06_LAUNCH",				priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_07_LAUNCH",				priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_08_LAUNCH",				priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_09_LAUNCH",				priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_10_LAUNCH",				priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_11_LAUNCH",				priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_RADAR_SEARCHING_TERSE",	priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_RADAR_TRACKING_TERSE",	priority = 0,	duration = 1.5,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_RADAR_LAUNCH",			priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_2S6",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_GUN",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_HAWK",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_HQ7",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_PATRIOT",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_RAPIER",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_ROLAND",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA2",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA3",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA4",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA5",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA6",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA8",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA10",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA11",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA12",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA13",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA15",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA17",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA20",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SA22",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_SAM",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_ZSU",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_Unknown",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_Radar",					priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ASE LINES NORMAL_FIXED WING",			priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER_RANGING",							priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER_DESIGNATING",						priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER_BEAMING",							priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER WARNING NORMAL_12",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER WARNING NORMAL_01",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER WARNING NORMAL_02",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER WARNING NORMAL_03",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER WARNING NORMAL_04",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER WARNING NORMAL_05",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER WARNING NORMAL_06",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER WARNING NORMAL_07",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER WARNING NORMAL_08",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER WARNING NORMAL_09",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER WARNING NORMAL_10",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/LASER WARNING NORMAL_11",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
	}
}


AN_APR39_FAILURE_SENSOR_NOSE_RIGHT = 0
AN_APR39_FAILURE_SENSOR_NOSE_LEFT  = 1
AN_APR39_FAILURE_SENSOR_TAIL_RIGHT = 2
AN_APR39_FAILURE_SENSOR_TAIL_LEFT  = 3
AN_APR39_FAILURE_TOTAL         = 4

Damage = {  {Failure = AN_APR39_FAILURE_SENSOR_NOSE_RIGHT, Failure_name =  "AN_APR39_FAILURE_SENSOR_NOSE_RIGHT", Failure_editor_name = _("AN/APR-39(V) sensor nose right"),  Element = 0, Integrity_Treshold = 0.0, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
        {Failure = AN_APR39_FAILURE_SENSOR_NOSE_LEFT, Failure_name =  "AN_APR39_FAILURE_SENSOR_NOSE_LEFT", Failure_editor_name = _("AN/APR-39 sensor tail right"),  Element = 0, Integrity_Treshold = 0.25, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
        {Failure = AN_APR39_FAILURE_SENSOR_TAIL_RIGHT, Failure_name =  "AN_APR39_FAILURE_SENSOR_TAIL_RIGHT", Failure_editor_name = _("AN/APR-39 sensor tail left"),  Element = 55, Integrity_Treshold = 0.5, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
        {Failure = AN_APR39_FAILURE_SENSOR_TAIL_LEFT, Failure_name =  "AN_APR39_FAILURE_SENSOR_TAIL_LEFT", Failure_editor_name = _("AN/APR-39 sensor nose left"),  Element = 55, Integrity_Treshold = 0.5, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
        {Failure = AN_APR39_FAILURE_TOTAL, Failure_name =  "AN_APR39_FAILURE_TOTAL", Failure_editor_name = _("AN/APR-39 total failure"),  Element = 4, Integrity_Treshold = 0.0, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300}}

need_to_be_closed = true -- close lua state after initialization 
