need_to_be_closed	= true -- lua_state  will be closed in post_initialize()
device_timer_dt		= 0.02

dofile(LockOn_Options.script_path		.."command_defs.lua")

animations =
{
	-- Cyclic Stick Grip
	{ command = hotas_commands.CYCLIC_TRIGGER_GUARD,					arg = 522 },
	{ command = hotas_commands.CYCLIC_TRIGGER_1ST_DETENT,				arg = 523 },
	{ command = hotas_commands.CYCLIC_TRIGGER_2ND_DETENT,				arg = 523 },

	{ command = hotas_commands.CYCLIC_TRIM_HOLD_SW_UP,					arg = 524 },
	{ command = hotas_commands.CYCLIC_TRIM_HOLD_SW_DOWN,				arg = 524 },
	{ command = hotas_commands.CYCLIC_TRIM_HOLD_SW_LEFT,				arg = 525 },
	{ command = hotas_commands.CYCLIC_TRIM_HOLD_SW_RIGHT,				arg = 525 },

	{ command = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_UP,				arg = 526 },
	{ command = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_DOWN,			arg = 526 },
	{ command = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_LEFT,			arg = 527 },
	{ command = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_RIGHT,			arg = 527 },

	{ command = hotas_commands.CYCLIC_SYMBOLOGY_SELECT_SW_UP,			arg = 528 },
	{ command = hotas_commands.CYCLIC_SYMBOLOGY_SELECT_SW_DOWN,			arg = 528 },
	{ command = hotas_commands.CYCLIC_SYMBOLOGY_SELECT_SW_DEPRESS,		arg = 530 },

	{ command = hotas_commands.CYCLIC_CMDS_SW_FWD,						arg = 529 },
	{ command = hotas_commands.CYCLIC_CMDS_SW_AFT,						arg = 529 },

	{ command = hotas_commands.CYCLIC_RTS_SW_LEFT,						arg = 531 },
	{ command = hotas_commands.CYCLIC_RTS_SW_RIGHT,						arg = 531 },
	--{ command = hotas_commands.CYCLIC_RTS_SW_DEPRESS,					arg =  },

	{ command = hotas_commands.CYCLIC_FMC_RELEASE_SW,					arg = 534 },
	{ command = hotas_commands.CYCLIC_CHAFF_DISPENCE_BTN,				arg = 533 },
	{ command = hotas_commands.CYCLIC_FLARE_DISPENCE_BTN,				arg = 529 },
	{ command = hotas_commands.CYCLIC_ATA_CAGE_UNCAGE_BTN,				arg = 532 },

	-- Collective Mission Grip
	{ command = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_UP,				arg = 535 },
	{ command = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_DOWN,			arg = 535 },
	{ command = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_LEFT,			arg = 536 },
	{ command = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_RIGHT,			arg = 536 },

	{ command = hotas_commands.MISSION_SIGHT_SELECT_SW_UP,				arg = 537 },
	{ command = hotas_commands.MISSION_SIGHT_SELECT_SW_DOWN,			arg = 537 },
	{ command = hotas_commands.MISSION_SIGHT_SELECT_SW_LEFT,			arg = 538 },
	{ command = hotas_commands.MISSION_SIGHT_SELECT_SW_RIGHT,			arg = 538 },

	{ command = hotas_commands.MISSION_FCR_MODE_SW_UP,					arg = 539 },
	{ command = hotas_commands.MISSION_FCR_MODE_SW_DOWN,				arg = 539 },
	{ command = hotas_commands.MISSION_FCR_MODE_SW_LEFT,				arg = 540 },
	{ command = hotas_commands.MISSION_FCR_MODE_SW_RIGHT,				arg = 540 },

	{ command = hotas_commands.MISSION_CURSOR_UP,						arg = 541 },
	{ command = hotas_commands.MISSION_CURSOR_DOWN,						arg = 541 },
	{ command = hotas_commands.MISSION_CURSOR_LEFT,						arg = 542 },
	{ command = hotas_commands.MISSION_CURSOR_RIGHT,					arg = 542 },
	{ command = hotas_commands.MISSION_CURSOR_ENTER,					arg = 543 },
	{ command = hotas_commands.MISSION_ALTERNATE_CURSOR_ENTER,			arg = 544 },
	{ command = hotas_commands.MISSION_CURSOR_AXIS_X,					arg = 542 },
	{ command = hotas_commands.MISSION_CURSOR_AXIS_Y,					arg = 541 },

	{ command = hotas_commands.MISSION_CURSOR_DISPLAY_SELECT_BTN,		arg = 545 },
	{ command = hotas_commands.MISSION_FCR_SCAN_SW_SINGLE,				arg = 546 },
	{ command = hotas_commands.MISSION_FCR_SCAN_SW_CONTINUOUS,			arg = 546 },
	{ command = hotas_commands.MISSION_CUED_SEARCH_SW,					arg = 547 },
	{ command = hotas_commands.MISSION_MISSILE_ADVANCE_SW,				arg = 548 },

	-- Collective Flight Grip
	{ command = hotas_commands.FLIGHT_EMERGENCY_JETTISON_SW_GUARD,		arg = 549 },
	{ command = hotas_commands.FLIGHT_EMERGENCY_JETTISON_SW	,			arg = 550 },

	{ command = hotas_commands.FLIGHT_NVS_SELECT_SW_TADS,				arg = 551 },
	{ command = hotas_commands.FLIGHT_NVS_SELECT_SW_PNVS,				arg = 551 },
	{ command = hotas_commands.FLIGHT_BORESIGHT_POLARITY_SW_BS,			arg = 552 },
	{ command = hotas_commands.FLIGHT_BORESIGHT_POLARITY_SW_PLRT,		arg = 552 },

	{ command = hotas_commands.FLIGHT_STABILATOR_CONTROL_SW_NU,			arg = 553 },
	{ command = hotas_commands.FLIGHT_STABILATOR_CONTROL_SW_ND,			arg = 553 },
	{ command = hotas_commands.FLIGHT_STABILATOR_CONTROL_SW_DEPRESS,	arg = 554 },
	
	{ command = hotas_commands.FLIGHT_SEARCHLIGHT_SW_UP,				arg = 555 },
	{ command = hotas_commands.FLIGHT_SEARCHLIGHT_SW_DOWN,				arg = 555 },

	{ command = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_UP,		arg = 556 },
	{ command = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_DOWN,		arg = 556 },
	{ command = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_LEFT,		arg = 557 },
	{ command = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_RIGHT,	arg = 557 },

	{ command = hotas_commands.FLIGHT_CHOP_BTN_GUARD,					arg = 558 },
	{ command = hotas_commands.FLIGHT_CHOP_BTN,							arg = 559 },

	{ command = hotas_commands.FLIGHT_TAIL_WHEEL_BTN,					arg = 560 },
	
	{ command = hotas_commands.FLIGHT_BUCS_TRIGGER_GUARD,				arg = 561 },
	{ command = hotas_commands.FLIGHT_BUCS_TRIGGER,						arg = 562 },

	-- input commands
	--{ command = hotas_commands.CYCLIC_TRIGGER_GUARD_ITER,				arg =  },
	--{ command = hotas_commands.FLIGHT_EMERGENCY_JETTISON_SW_GUARD_ITER,	arg =  },
	--{ command = hotas_commands.FLIGHT_SEARCHLIGHT_SW_ITER,				arg =  },
	--{ command = hotas_commands.FLIGHT_CHOP_BTN_GUARD_ITER,				arg =  },
	--{ command = hotas_commands.FLIGHT_BUCS_TRIGGER_GUARD_ITER,			arg =  },

}
