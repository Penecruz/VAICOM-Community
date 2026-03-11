need_to_be_closed	= true -- lua_state  will be closed in post_initialize()
device_timer_dt		= 0.02

dofile(LockOn_Options.script_path		.."command_defs.lua")

animations =
{
	-- Cyclic Stick Grip
	{ command = hotas_commands.CYCLIC_TRIGGER_GUARD,					arg = 563 },
	{ command = hotas_commands.CYCLIC_TRIGGER_1ST_DETENT,				arg = 564 },
	{ command = hotas_commands.CYCLIC_TRIGGER_2ND_DETENT,				arg = 564 },

	{ command = hotas_commands.CYCLIC_TRIM_HOLD_SW_UP,					arg = 565 },
	{ command = hotas_commands.CYCLIC_TRIM_HOLD_SW_DOWN,				arg = 565 },
	{ command = hotas_commands.CYCLIC_TRIM_HOLD_SW_LEFT,				arg = 566 },
	{ command = hotas_commands.CYCLIC_TRIM_HOLD_SW_RIGHT,				arg = 566 },

	{ command = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_UP,				arg = 567 },
	{ command = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_DOWN,			arg = 567 },
	{ command = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_LEFT,			arg = 568 },
	{ command = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_RIGHT,			arg = 568 },

	{ command = hotas_commands.CYCLIC_SYMBOLOGY_SELECT_SW_UP,			arg = 569 },
	{ command = hotas_commands.CYCLIC_SYMBOLOGY_SELECT_SW_DOWN,			arg = 569 },
	{ command = hotas_commands.CYCLIC_SYMBOLOGY_SELECT_SW_DEPRESS,		arg = 570 },

	{ command = hotas_commands.CYCLIC_CMDS_SW_FWD,						arg = 572 },
	{ command = hotas_commands.CYCLIC_CMDS_SW_AFT,						arg = 572 },

	{ command = hotas_commands.CYCLIC_RTS_SW_LEFT,						arg = 573 },
	{ command = hotas_commands.CYCLIC_RTS_SW_RIGHT,						arg = 573 },
	--{ command = hotas_commands.CYCLIC_RTS_SW_DEPRESS,					arg =  },

	{ command = hotas_commands.CYCLIC_FMC_RELEASE_SW,					arg = 574 },
	{ command = hotas_commands.CYCLIC_CHAFF_DISPENCE_BTN,				arg = 575 },
	{ command = hotas_commands.CYCLIC_FLARE_DISPENCE_BTN,				arg = 572 },
	{ command = hotas_commands.CYCLIC_ATA_CAGE_UNCAGE_BTN,				arg = 576 },

	-- Collective Mission Grip
	{ command = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_UP,				arg = 577 },
	{ command = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_DOWN,			arg = 577 },
	{ command = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_LEFT,			arg = 578 },
	{ command = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_RIGHT,			arg = 578 },

	{ command = hotas_commands.MISSION_SIGHT_SELECT_SW_UP,				arg = 579 },
	{ command = hotas_commands.MISSION_SIGHT_SELECT_SW_DOWN,			arg = 579 },
	{ command = hotas_commands.MISSION_SIGHT_SELECT_SW_LEFT,			arg = 580 },
	{ command = hotas_commands.MISSION_SIGHT_SELECT_SW_RIGHT,			arg = 580 },

	{ command = hotas_commands.MISSION_FCR_MODE_SW_UP,					arg = 581 },
	{ command = hotas_commands.MISSION_FCR_MODE_SW_DOWN,				arg = 581 },
	{ command = hotas_commands.MISSION_FCR_MODE_SW_LEFT,				arg = 582 },
	{ command = hotas_commands.MISSION_FCR_MODE_SW_RIGHT,				arg = 582 },

	{ command = hotas_commands.MISSION_CURSOR_UP,						arg = 583 },
	{ command = hotas_commands.MISSION_CURSOR_DOWN,						arg = 583 },
	{ command = hotas_commands.MISSION_CURSOR_LEFT,						arg = 584 },
	{ command = hotas_commands.MISSION_CURSOR_RIGHT,					arg = 584 },
	{ command = hotas_commands.MISSION_CURSOR_ENTER,					arg = 585 },
	{ command = hotas_commands.MISSION_ALTERNATE_CURSOR_ENTER,			arg = 586 },
	{ command = hotas_commands.MISSION_CURSOR_AXIS_X,					arg = 584 },
	{ command = hotas_commands.MISSION_CURSOR_AXIS_Y,					arg = 583 },

	{ command = hotas_commands.MISSION_CURSOR_DISPLAY_SELECT_BTN,		arg = 587 },
	{ command = hotas_commands.MISSION_FCR_SCAN_SW_SINGLE,				arg = 588 },
	{ command = hotas_commands.MISSION_FCR_SCAN_SW_CONTINUOUS,			arg = 588 },
	{ command = hotas_commands.MISSION_CUED_SEARCH_SW,					arg = 589 },
	{ command = hotas_commands.MISSION_MISSILE_ADVANCE_SW,				arg = 590 },

	-- Collective Flight Grip
	{ command = hotas_commands.FLIGHT_EMERGENCY_JETTISON_SW_GUARD,		arg = 591 },
	{ command = hotas_commands.FLIGHT_EMERGENCY_JETTISON_SW	,			arg = 592 },

	{ command = hotas_commands.FLIGHT_NVS_SELECT_SW_TADS,				arg = 593 },
	{ command = hotas_commands.FLIGHT_NVS_SELECT_SW_PNVS,				arg = 593 },
	{ command = hotas_commands.FLIGHT_BORESIGHT_POLARITY_SW_BS,			arg = 594 },
	{ command = hotas_commands.FLIGHT_BORESIGHT_POLARITY_SW_PLRT,		arg = 594 },

	{ command = hotas_commands.FLIGHT_STABILATOR_CONTROL_SW_NU,			arg = 595 },
	{ command = hotas_commands.FLIGHT_STABILATOR_CONTROL_SW_ND,			arg = 595 },
	{ command = hotas_commands.FLIGHT_STABILATOR_CONTROL_SW_DEPRESS,	arg = 596 },
	
	{ command = hotas_commands.FLIGHT_SEARCHLIGHT_SW_UP,				arg = 597 },
	{ command = hotas_commands.FLIGHT_SEARCHLIGHT_SW_DOWN,				arg = 597 },

	{ command = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_UP,		arg = 598 },
	{ command = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_DOWN,		arg = 598 },
	{ command = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_LEFT,		arg = 599 },
	{ command = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_RIGHT,	arg = 599 },

	{ command = hotas_commands.FLIGHT_CHOP_BTN_GUARD,					arg = 600 },
	{ command = hotas_commands.FLIGHT_CHOP_BTN,							arg = 601 },

	{ command = hotas_commands.FLIGHT_TAIL_WHEEL_BTN,					arg = 602 },
	
	{ command = hotas_commands.FLIGHT_BUCS_TRIGGER_GUARD,				arg = 603 },
	{ command = hotas_commands.FLIGHT_BUCS_TRIGGER,						arg = 604 },

	-- input commands
	--{ command = hotas_commands.CYCLIC_TRIGGER_GUARD_ITER,				arg =  },
	--{ command = hotas_commands.FLIGHT_EMERGENCY_JETTISON_SW_GUARD_ITER,	arg =  },
	--{ command = hotas_commands.FLIGHT_SEARCHLIGHT_SW_ITER,				arg =  },
	--{ command = hotas_commands.FLIGHT_CHOP_BTN_GUARD_ITER,				arg =  },
	--{ command = hotas_commands.FLIGHT_BUCS_TRIGGER_GUARD_ITER,			arg =  },

}
