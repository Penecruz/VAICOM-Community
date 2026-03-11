local gettext = require("i_18n")
_ = gettext.translate

device_timer_dt		= 0.5
need_to_be_closed	= true -- close lua state after initialization 

-- MLWS sensors: four sensors 
eyes ={}

eyes[1] =
{
    position      = {x = 4.47,y = 0.3,z =  0.9},
    orientation   = {azimuth  = math.rad(45),elevation = -math.rad(0.0)},
    field_of_view = math.rad(120) 
}
eyes[2] =
{
    position      = {x = 4.47,y = 0.3,z = -0.9},
    orientation   = {azimuth  = math.rad(-45),elevation = -math.rad(0.0)},
    field_of_view = math.rad(120) 
}
eyes[3] =
{
    position      = {x = 0,y = 0.3,z =  2.5},
    orientation   = {azimuth  = math.rad(135),elevation = -math.rad(0.0)},
    field_of_view = math.rad(120) 
}
eyes[4] =
{
    position      = {x = 0,y = 0.3,z =  -2.5},
    orientation   = {azimuth  = math.rad(-135),elevation = -math.rad(0.0)},
    field_of_view = math.rad(120) 
}
--  for voice messaging system
msg_MLWS = 
{
    _03_Low  = 0,
    _03_High = 1,		
    _06_Low  = 2,
    _06_High = 3,		
    _09_Low  = 4,
    _09_High = 5,		
    _12_Low  = 6,
    _12_High = 7
}

message_legth = 3.0
notify_delta  = 4.0


message_table = {} 

for i = 0,7 do message_table[i] = 46 + i end

-- Sounds
device_timer_dt = 0.05

local short_delay	= 0.3	-- [sec]

CMWS =
{
	path = "Aircrafts/AH-64D/Cockpit/",

	messages =
	{	
		{ msg = "Betty/CWMS_MISSILE MISSILE",	priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/CWMS_FORWARD_LEFT",		priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/CWMS_FORWARD",			priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/CWMS_FORWARD_RIGHT",		priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/CWMS_RIGHT",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/CWMS_AFT_RIGHT",			priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/CWMS_AFT",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/CWMS_AFT_LEFT",			priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/CWMS_LEFT",				priority = 0,	duration = 1.0,	looped = false,	delays = {short_delay} }
	}
}

FAILURE_CHAFF_DISPENSER				= 0
FAILURE_CHAFF_DISPENSER_DETACH		= 1
FAILURE_FLARE_DISPENSER_LEFT		= 2
FAILURE_FLARE_DISPENSER_RIGHT		= 3
FAILURE_FLARE_DISPENSER_BOTH		= 4
FAILURE_TOTAL						= 5

Damage =
{
		{Failure = FAILURE_CHAFF_DISPENSER, 		Failure_name = "FAILURE_CHAFF_DISPENSER",			Element = 43--[["KEEL_L"]], 		Integrity_Treshold = 0.2, 	work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
		{Failure = FAILURE_CHAFF_DISPENSER_DETACH,	Failure_name = "FAILURE_CHAFF_DISPENSER_DETACH",	Element = 41--[["KEEL_CENTER"]], 	Integrity_Treshold = 0.01,	work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
		{Failure = FAILURE_FLARE_DISPENSER_LEFT, 	Failure_name = "FAILURE_FLARE_DISPENSER_LEFT",		Element = 43--[["KEEL_L"]], 		Integrity_Treshold = 0.2, 	work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
		{Failure = FAILURE_FLARE_DISPENSER_RIGHT, 	Failure_name = "FAILURE_FLARE_DISPENSER_RIGHT",		Element = 44--[["KEEL_R"]], 		Integrity_Treshold = 0.2, 	work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},
		{Failure = FAILURE_FLARE_DISPENSER_BOTH, 	Failure_name = "FAILURE_FLARE_DISPENSER_BOTH",		Element = 55--[["TAIL"]], 			Integrity_Treshold = 0.2, 	work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300},

		--{Failure = FAILURE_TOTAL, Failure_name = "FAILURE_TOTAL", Failure_editor_name = _("CMWS total failure"),  Element = 55, Integrity_Treshold = 0.25, work_time_to_fail_probability = 0.5, work_time_to_fail = 3600*300}
}
