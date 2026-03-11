-- Sounds
device_timer_dt = 0.05

local short_delay	= 0.5	-- [sec]
local long_delay	= 1.5	-- [sec]

CIU =
{
	path = "Aircrafts/AH-64D/Cockpit/",

	messages =
	{
		{ msg = "Betty/AftDeck",			priority = 3,	duration = 2.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/AltLow",				priority = 3,	duration = 2.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/ApuFire",			priority = 3,	duration = 2.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/Eng1fire",			priority = 3,	duration = 2.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/Eng1out",			priority = 3,	duration = 2.0,	looped = true,	delays = {short_delay} },
		{ msg = "Betty/Eng1ovsp",			priority = 3,	duration = 2.0,	looped = true,	delays = {short_delay} },
		{ msg = "Betty/Eng2fire",			priority = 3,	duration = 2.0,	looped = false,	delays = {short_delay} },
		{ msg = "Betty/Eng2out",			priority = 3,	duration = 2.0,	looped = true,	delays = {short_delay} },
		{ msg = "Betty/Eng2ovsp",			priority = 3,	duration = 2.0,	looped = true,	delays = {short_delay} },
		{ msg = "Betty/EngChop",			priority = 3,	duration = 2.0,	looped = true,	delays = {short_delay} },
		{ msg = "Betty/Hydraul",			priority = 3,	duration = 2.0,	looped = true,	delays = {short_delay} },
		{ msg = "Betty/LO Airspeed",		priority = 3,	duration = 2.0,	looped = true,	delays = {short_delay} },	
		{ msg = "Betty/RotorHigh",			priority = 3,	duration = 2.0,	looped = true,	delays = {short_delay} },
		{ msg = "Betty/RotorLow",			priority = 3,	duration = 2.0,	looped = true,	delays = {short_delay} },
		{ msg = "Betty/TailRotor",			priority = 3,	duration = 2.0,	looped = true,	delays = {short_delay} },
	},

	tones =
	{
--		{ tone = "Tones/detect",			priority = 2,	duration = 0.5,	looped = true, },
--		{ tone = "Tones/tracking",			priority = 1,	duration = 6.0,	looped = true, },	
--		{ tone = "Tones/launch",			priority = 0,	duration = 6.0,	looped = true, },
		{ tone = "Tones/bingo gas",			priority = 4,	duration = 0.5,	looped = false, },	
		{ tone = "Tones/flt-ctrl",			priority = 5,	duration = 4.0,	looped = false, },	
		{ tone = "Tones/flt-ctrl_short",	priority = 5,	duration = 0.5,	looped = false, },		
		{ tone = "Tones/master_caution",	priority = 6,	duration = 10.0,looped = true, },	--cautions
		{ tone = "Tones/flt-ctrl",			priority = 6,	duration = 2.5,	looped = false, },	--advisory		
	--	{ tone = "Tones/master_caution",	priority = 6,	duration = 2.5,	looped = false, },	--IDM		
		{ tone = "Tones/idm_tone",			priority = 6,	duration = 2.5,	looped = false, },	--IDM		
	},
}