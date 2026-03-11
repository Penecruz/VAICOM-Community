-- HAD (High Action Display)

-- Sight Status Messages (12 symbols): color, message
HAD_SightStatusMsgs =
{
	"",
	"BOT",
	"CHK",
	"CUE UPDT",
	"ENERGY LOW",
	"EOT",
	"FCR FAIL",
	"FCR HOT",
	"FCR",
	"FCR XMIT",
	"FCR NOT",
	"FIRE MSLS",
	"FIXED",
	"FLIR",
	"FLIR OFF",
	"HF TOF=%02d",
	"IHADSS B/S",
	"IHADSS FAIL",
	"IHADSS LOS",
	"INTERNAL B/S",
	"INTERNAL B/S",
	"LASE %d TRGT",
	"LASER FAIL",
	"LIMITS",
	"LRFD CODE ?",
	"LST CODE ?",
	"ARTY TOF=%02d",
	"MMA PINNED",
	"MSL LAUNCH",
	"NAV DATA",
	"NVS B/S ERR",
	"NVS DIRECT",
	"NVS FAIL",
	"NVS FIXED",
	"NVS NOT COOL",
	"OUTFRONT B/S",
	"PNVS SBIT",
	"RECORD FAIL",
	"RECORDING",
	"REMOTE",
	"RFI DATA?",
	"RFI FAIL",
	"RFI",
	"SANUC",
	"SANUC",
	"SIM LAUNCH",
	"TADS B/S",
	"TADS FAIL",
	"TADS SBIT",
	"TARGET DATA?",
	"WET",
	"?",
	-- second parts of long messages
	"NOT READY",
	"NOT COOL",
	"REQUIRED",
	"INVALID",
	"IN PROGRESS",
	"INSTALLED",
}

-- Range And Range Source Status Messages (5 symbols): color, message
HAD_RangeAndRangeSourceStatusMsgs =
{
	"",
	"%c%4d",	-- "*XXXX", * = firing, range in meters
	"1.5",
	"3.0",
	"A%.1f",	-- range in km
	"M%.1f",	-- range in km
	"N%.1f",	-- range in km
	"N%.1f",	-- range in km
	"R%.1f",	-- range in km
	"?",
}

-- Weapon Control Status Messages (5 symbols): color, message
HAD_WeaponControlStatusMsgs =
{
	"",
	"CGUN",
	"CMSL",
	"COOP",
	"CRKT",
	"PGUN",
	"PMSL",
	"PRKT",
	"?",
}

-- Weapon Status Messages (12 symbols): color, message
HAD_WeaponStatusMsgs =
{
	"",
	
	-- GUN
	"GUN B/S",
	"GUN FAIL",
	"GUN JAM",
	"ROUNDS %4d",
	
	-- MSL
	"RF MSL TRACK",
	"2 CHAN TRACK",
	"PRI CHAN TRK",
	"ALT CHAN TRK",
	"PRI CODE ?",
	"ALT CODE ?",
	"DIR MAN",
	"DIR NORM",
	"DIR RIPL",
	"HI MAN",
	"HI NORM",
	"HI RIPL",
	"LO MAN",
	"LO NORM",
	"LO RIPL",
	"LOAL MAN",
	"LOAL NORM",
	"LOBL INHIBIT",
	"LOBL MAN",
	"LOBL NORM",
	"LOBL RIPL",
	"LASE %d TRGT",
	"HF TOF=%02d",
	"FIRE MSLS",
	"HANGFIRE",
	"MISFIRE",
	"MSL LAUNCH",
	"MSL SELECT",
	"MSL TYPE?",
	"NO ACQUIRE",
	"NO MISSILES",
	"SAL SEL?",
	
	-- RKT
	"TYPE?",
	"NO ROCKETS",
	"RKT TOF=%02d",
	"RKT NORM",
	"RKT G-S",
	
	-- Pylons payload
	"LIMITS",
	"PYLON B/S",

	-- Other
	"DEF SCHEME A",
	"DEF SCHEME B",
	"DEF SCHEME C",
	"DEF SCHEME D",
	"DEF SCHEME E",
	"DEF SCHEME F",
	"DEF SCHEME G",
	
	"MOD SCHEME %1d",
	"SIM LAUNCH",
	"WEAPON?",
	"?",
}

-- Weapon Inhibit Status Messages (12 symbols): color, message
HAD_WeaponInhibitStatusMsgs =
{
	"",
	-- Generic Inhibits
	"ALL TRKS DEL",
	"LIVE AMMO",
	"TRK %1d DEL",
	"TRK %1d DROP",
	"?",
	-- Common Inhibits (for HELLFIRE, GUN, ROCKET)
	"ALT LAUNCH",
	"LOS INVALID",
	"TRAINING",
	"T%02d",
	"W%02d",
	"SAFE",
	"BAL LIMIT",
	-- HELLFIRE Inhibits
	"ACCEL LIMIT",
	"BACK SCATTER",
	"DATA INVALID",
	"GUN OBSTRUCT",
	"LASER RANGE?",
	"MSL NOT RDY",
	"PYLON ANGLE",
	"PYLON ERROR",
	"PYLON LIMIT",
	"RATE LIMIT",
	"ROLL LIMIT",
	"SKR LIMIT",
	"YAW LIMIT",
	-- GUN Inhibits
	"AZ LIMIT",
	"COINCIDENCE",
	"EL LIMIT",
	-- ROCKET Inhibits
	"ACCEL LIMIT",
	"GUN OBSTRUCT",
	"PYLON ERROR",
	"PYLON LIMIT",
	"TYPE SELECT",
}

-- Sight Select Status Messages (5 symbols): color, message
HAD_SightSelectStatusMsgs =
{
	"",
	"P-FCR ",
	"C-FCR ",
	"P-FCRL",
	"C-FCRL",
	"P-HMD ",
	"C-HMD ",
	" TADS ",
	" TADSL",
	"?",
}

-- Acquisition Status Messages (4 symbols): color, message
HAD_AcquisitionStatusMsgs =
{
	"PHS", 
	"GHS",
	"SKR",
	"RFI",
	"FCR",
	"FXD",
	"TADS",
	"TXX",
	"TRN",
	"?",
	"?PHS", 
	"?GHS",
	"?SKR",
	"?RFI",
	"?FCR",
	"FXD",
	"?TADS"
}

HAD_OwnerCueStatusMsgs =
{
	"",
	"PLT FORMAT",
	"CPG FORMAT",
}
