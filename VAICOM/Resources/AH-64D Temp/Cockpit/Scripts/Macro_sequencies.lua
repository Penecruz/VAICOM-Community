dofile(LockOn_Options.script_path.."command_defs.lua")
dofile(LockOn_Options.script_path.."devices.lua")

local std_message_timeout = 15

local	t_start	= 0.0
local	t_stop	= 0.0
local	dt		= 0.2
local	dt_mto	= 0.5
local	start_sequence_time		= 415.0
local	stop_sequence_time		= 105.0
local	dt_awt	= 235.0		-- alignment waiting time
local	dt_1es	= 30		-- first engine start time
local	dt_2es	= 40		-- second engine start time
local	dt_es	= dt_1es + dt_2es + 5

--
start_sequence_full 	  = {}
stop_sequence_full		  = {}
cockpit_illumination_full = {}

function push_command(sequence, run_t, command)
	sequence[#sequence + 1] =  command
	sequence[#sequence]["time"] = run_t
end

function push_start_command(delta_t, command)
	t_start = t_start + delta_t
	push_command(start_sequence_full,t_start, command)
end

function push_stop_command(delta_t, command)
	t_stop = t_stop + delta_t
	push_command(stop_sequence_full,t_stop, command)
end

--
local count = 0
local function counter()
	count = count + 1
	return count
end

-- conditions
count = -1

AH64_AD_NO_FAILURE				= counter()
AH64_AD_ERROR					= counter()

AH64_AD_COLLECTIVE_SET_DOWN		= counter()
AH64_AD_COLLECTIVE_AT_DOWN		= counter()

AH64_AD_L_PCL_SET_TO_OFF		= counter()
AH64_AD_L_PCL_AT_OFF			= counter()
AH64_AD_L_PCL_SET_TO_IDLE		= counter()
AH64_AD_L_PCL_AT_IDLE			= counter()
AH64_AD_L_PCL_SET_TO_FLY		= counter()
AH64_AD_L_PCL_AT_FLY			= counter()
AH64_AD_L_PCL_DOWN_TO_IDLE		= counter()

AH64_AD_R_PCL_SET_TO_OFF		= counter()
AH64_AD_R_PCL_AT_OFF			= counter()
AH64_AD_R_PCL_SET_TO_IDLE		= counter()
AH64_AD_R_PCL_AT_IDLE			= counter()
AH64_AD_R_PCL_SET_TO_FLY		= counter()
AH64_AD_R_PCL_AT_FLY			= counter()
AH64_AD_R_PCL_DOWN_TO_IDLE		= counter()

AH64_AD_APU_READY				= counter()
AH64_AD_NpNr_VERIFY				= counter()

AH64_AD_TAIL_WHEEL_LOCK			= counter()
AH64_AD_TAIL_WHEEL_UNLOCK		= counter()

AH64_AD_BLEED_AIR_1_ON			= counter()
AH64_AD_BLEED_AIR_2_ON			= counter()

AH64_AD_EMERG_HYDRAULIC_OFF		= counter()

AH64_AD_PNVS_OFF				= counter()
AH64_AD_TADS_OFF				= counter()
AH64_AD_FCR_OFF					= counter()
AH64_AD_IHADSS_BORESIGHT		= counter()



--
alert_messages = {}

alert_messages[AH64_AD_ERROR]					= { message = _("FM MODEL ERROR"),								message_timeout = std_message_timeout}

alert_messages[AH64_AD_COLLECTIVE_SET_DOWN]		= { message = _("COLLECTIVE - REDUCE TO FLAT PITCH"),			message_timeout = std_message_timeout}
alert_messages[AH64_AD_COLLECTIVE_AT_DOWN]		= { message = _("COLLECTIVE MUST BE REDUCED TO FLAT PITCH"),	message_timeout = std_message_timeout}

alert_messages[AH64_AD_L_PCL_SET_TO_OFF]		= { message = _("LEFT POWER CONTROL LEVER - TO OFF"),			message_timeout = std_message_timeout}
alert_messages[AH64_AD_L_PCL_AT_OFF]			= { message = _("LEFT POWER CONTROL LEVER MUST BE AT OFF"),		message_timeout = std_message_timeout}
alert_messages[AH64_AD_L_PCL_SET_TO_IDLE]		= { message = _("LEFT POWER CONTROL LEVER - TO IDLE"),			message_timeout = std_message_timeout}
alert_messages[AH64_AD_L_PCL_AT_IDLE]			= { message = _("LEFT POWER CONTROL LEVER MUST BE AT IDLE"),	message_timeout = std_message_timeout}
alert_messages[AH64_AD_L_PCL_SET_TO_FLY]		= { message = _("LEFT POWER CONTROL LEVER - TO FLY"),			message_timeout = std_message_timeout}
alert_messages[AH64_AD_L_PCL_AT_FLY]			= { message = _("LEFT POWER CONTROL LEVER MUST BE AT FLY"),		message_timeout = std_message_timeout}
alert_messages[AH64_AD_L_PCL_DOWN_TO_IDLE]		= { message = _("LEFT POWER CONTROL LEVER - TO IDLE"),			message_timeout = std_message_timeout}

alert_messages[AH64_AD_R_PCL_SET_TO_OFF]		= { message = _("RIGHT POWER CONTROL LEVER - TO OFF"),			message_timeout = std_message_timeout}
alert_messages[AH64_AD_R_PCL_AT_OFF]			= { message = _("RIGHT POWER CONTROL LEVER MUST BE AT OFF"),	message_timeout = std_message_timeout}
alert_messages[AH64_AD_R_PCL_SET_TO_IDLE]		= { message = _("RIGHT POWER CONTROL LEVER - TO IDLE"),			message_timeout = std_message_timeout}
alert_messages[AH64_AD_R_PCL_AT_IDLE]			= { message = _("RIGHT POWER CONTROL LEVER MUST BE AT IDLE"),	message_timeout = std_message_timeout}
alert_messages[AH64_AD_R_PCL_SET_TO_FLY]		= { message = _("RIGHT POWER CONTROL LEVER - TO FLY"),			message_timeout = std_message_timeout}
alert_messages[AH64_AD_R_PCL_AT_FLY]			= { message = _("RIGHT POWER CONTROL LEVER MUST BE AT FLY"),	message_timeout = std_message_timeout}
alert_messages[AH64_AD_R_PCL_DOWN_TO_IDLE]		= { message = _("RIGHT POWER CONTROL LEVER - TO IDLE"),			message_timeout = std_message_timeout}

alert_messages[AH64_AD_APU_READY]				= { message = _("APU ON LIGHT MUST BE ON WITHIN 35 SEC"),		message_timeout = std_message_timeout}
alert_messages[AH64_AD_NpNr_VERIFY]				= { message = _("Np AND Nr MUST BE 101%"),						message_timeout = std_message_timeout}

alert_messages[AH64_AD_TAIL_WHEEL_LOCK]			= { message = _("TAIL WHEEL - LOCK"),							message_timeout = std_message_timeout}
alert_messages[AH64_AD_TAIL_WHEEL_UNLOCK]		= { message = _("TAIL WHEEL - UNLOCK"),							message_timeout = std_message_timeout}

alert_messages[AH64_AD_BLEED_AIR_1_ON]			= { message = _("BLEED AIR 1 - ON"),							message_timeout = std_message_timeout}
alert_messages[AH64_AD_BLEED_AIR_2_ON]			= { message = _("BLEED AIR 2 - ON"),							message_timeout = std_message_timeout}

alert_messages[AH64_AD_EMERG_HYDRAULIC_OFF]		= { message = _("EMERGENCY HYDRAULIC - OFF"),					message_timeout = std_message_timeout}

alert_messages[AH64_AD_PNVS_OFF]				= { message = _("PNVS - OFF"),									message_timeout = std_message_timeout}
alert_messages[AH64_AD_TADS_OFF]				= { message = _("TADS - OFF"),									message_timeout = std_message_timeout}
alert_messages[AH64_AD_FCR_OFF]					= { message = _("FCR - OFF"),									message_timeout = std_message_timeout}


----------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------
-- Start sequence
push_start_command(2.0,	{message = _("AUTOSTART SEQUENCE IS RUNNING"), message_timeout = start_sequence_time})
--
-- Interior check
-- PLT
push_start_command(dt,		{message = _("- INTERIOR CHECK"),																message_timeout = dt * 64})
push_start_command(dt,		{message = _("- MSTR IGN SWITCH - OFF"),														message_timeout = dt_mto})
push_start_command(dt,		{device = devices.ELEC_INTERFACE,		action = electric_commands.MIK,							value = 0.0})
push_start_command(dt,		{message = _("- PARK BRAKE - SET"),																message_timeout = dt_mto})
push_start_command(dt,		{device = devices.GEAR_INTERFACE,		action = gear_commands.AH64_ParkingBrake,				value = 1.0})
push_start_command(dt,		{message = _("- CHOP BUTTON - GUARD DOWN"),														message_timeout = dt * 6})
push_start_command(dt,		{device = devices.HOTAS_PLT,			action = hotas_commands.FLIGHT_CHOP_BTN_GUARD,			value = 1.0})
push_start_command(dt,		{device = devices.HOTAS_PLT,			action = hotas_commands.FLIGHT_CHOP_BTN,				value = 0.0})
push_start_command(dt,		{device = devices.HOTAS_PLT,			action = hotas_commands.FLIGHT_CHOP_BTN_GUARD,			value = 0.0})
-- CPG
push_start_command(dt,		{device = devices.HOTAS_CPG,			action = hotas_commands.FLIGHT_CHOP_BTN_GUARD,			value = 1.0})
push_start_command(dt,		{device = devices.HOTAS_CPG,			action = hotas_commands.FLIGHT_CHOP_BTN,				value = 0.0})
push_start_command(dt,		{device = devices.HOTAS_CPG,			action = hotas_commands.FLIGHT_CHOP_BTN_GUARD,			value = 0.0})
push_start_command(dt,		{message = _("- STORES JETTISON PANEL - CHECK"),												message_timeout = dt * 14})
-- PLT
push_start_command(dt,		{device = devices.JETT_PANEL_PLT,		action = JETT_commands.STORE_LO_JETTISON_ARMED,			value = 0.0})
push_start_command(dt,		{device = devices.JETT_PANEL_PLT,		action = JETT_commands.STORE_LI_JETTISON_ARMED,			value = 0.0})
push_start_command(dt,		{device = devices.JETT_PANEL_PLT,		action = JETT_commands.STORE_RI_JETTISON_ARMED,			value = 0.0})
push_start_command(dt,		{device = devices.JETT_PANEL_PLT,		action = JETT_commands.STORE_RO_JETTISON_ARMED,			value = 0.0})
push_start_command(dt,		{device = devices.JETT_PANEL_PLT,		action = JETT_commands.STORE_JETTISON_LEFT_WINGTIP,		value = 0.0})
push_start_command(dt,		{device = devices.JETT_PANEL_PLT,		action = JETT_commands.STORE_JETTISON_RIGHT_WINGTIP,	value = 0.0})
push_start_command(dt,		{device = devices.JETT_PANEL_PLT,		action = JETT_commands.STORES_JETT_PUSHBUTTON,			value = 0.0})
-- CPG
push_start_command(dt,		{device = devices.JETT_PANEL_CPG,		action = JETT_commands.STORE_LO_JETTISON_ARMED,			value = 0.0})
push_start_command(dt,		{device = devices.JETT_PANEL_CPG,		action = JETT_commands.STORE_LI_JETTISON_ARMED,			value = 0.0})
push_start_command(dt,		{device = devices.JETT_PANEL_CPG,		action = JETT_commands.STORE_RI_JETTISON_ARMED,			value = 0.0})
push_start_command(dt,		{device = devices.JETT_PANEL_CPG,		action = JETT_commands.STORE_RO_JETTISON_ARMED,			value = 0.0})
push_start_command(dt,		{device = devices.JETT_PANEL_CPG,		action = JETT_commands.STORE_JETTISON_LEFT_WINGTIP,		value = 0.0})
push_start_command(dt,		{device = devices.JETT_PANEL_CPG,		action = JETT_commands.STORE_JETTISON_RIGHT_WINGTIP,	value = 0.0})
push_start_command(dt,		{device = devices.JETT_PANEL_CPG,		action = JETT_commands.STORES_JETT_PUSHBUTTON,			value = 0.0})
push_start_command(dt,		{message = _("- POWER LEVERS - OFF"),															message_timeout = (dt + 1) * 4})
push_start_command(dt,		{										check_condition = AH64_AD_L_PCL_DOWN_TO_IDLE,			message_timeout = dt_mto})
push_start_command(dt,		{										check_condition = AH64_AD_L_PCL_SET_TO_OFF,				message_timeout = dt_mto})
push_start_command(1.0,		{										check_condition = AH64_AD_L_PCL_AT_OFF,					message_timeout = dt_mto})
push_start_command(dt,		{										check_condition = AH64_AD_R_PCL_DOWN_TO_IDLE,			message_timeout = dt_mto})
push_start_command(dt,		{										check_condition = AH64_AD_R_PCL_SET_TO_OFF,				message_timeout = dt_mto})
push_start_command(1.0,		{										check_condition = AH64_AD_R_PCL_AT_OFF,					message_timeout = dt_mto})
push_start_command(dt,		{message = _("- ENGINE START SWITCHES - OFF"),													message_timeout = dt * 4})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.Eng1StartSw,					value = 0.0})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.Eng1IgnOrideSw,				value = 0.0})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.Eng2StartSw,					value = 0.0})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.Eng2IgnOrideSw,				value = 0.0})
push_start_command(dt,		{message = _("- RTR BRK SWITCH - OFF"),															message_timeout = dt_mto})
push_start_command(dt,		{device = devices.HYDRO_INTERFACE,		action = hydraulic_commands.Rotor_Brake,				value = 1.0})
push_start_command(dt,		{message = _("- EMERGENCY PANEL - CHECK"),														message_timeout = dt * 4})
-- PLT
push_start_command(dt,		{device = devices.EMERGENCY_PANEL,		action = intercom_commands.PLT_UHF_GUARD_Btn,			value = 0.0})
push_start_command(dt,		{device = devices.EMERGENCY_PANEL,		action = intercom_commands.PLT_XPNDR_Btn,				value = 0.0})
-- CPG
push_start_command(dt,		{device = devices.EMERGENCY_PANEL,		action = intercom_commands.CPG_UHF_GUARD_Btn,			value = 0.0})
push_start_command(dt,		{device = devices.EMERGENCY_PANEL,		action = intercom_commands.CPG_XPNDR_Btn,				value = 0.0})
push_start_command(dt,		{message = _("- EMER HYD - OFF"),																message_timeout = dt_mto})
-- TODO: turn emerg hydraulic off if it is on
-- PLT
push_start_command(dt,		{										check_condition = AH64_AD_EMERG_HYDRAULIC_OFF,			message_timeout = dt_mto})
push_start_command(dt,		{device = devices.HYDRO_INTERFACE,		action = hydraulic_commands.Emergency_HYD_PLT,			value = 0.0})
-- CPG
push_start_command(dt,		{device = devices.HYDRO_INTERFACE,		action = hydraulic_commands.Emergency_HYD_CPG,			value = 0.0})
push_start_command(dt,		{message = _("- ZEROIZE - CHECK IN AFT"),														message_timeout = dt_mto})
-- PLT
push_start_command(dt,		{device = devices.EMERGENCY_PANEL,		action = intercom_commands.PLT_ZEROIZE_Sw,				value = 0.0})
-- CPG
push_start_command(dt,		{device = devices.EMERGENCY_PANEL,		action = intercom_commands.CPG_ZEROIZE_Sw,				value = 0.0})
push_start_command(dt,		{message = _("- NVS MODE SWITCH - OFF"),														message_timeout = dt_mto})
-- PLT
push_start_command(dt,		{device = devices.ELEC_INTERFACE,		action = electric_commands.NVS_MODE_PLT_KNOB,			value = -1.0})
-- CPG
push_start_command(dt,		{device = devices.ELEC_INTERFACE,		action = electric_commands.NVS_MODE_CPG_KNOB,			value = -1.0})
push_start_command(dt,		{message = _("- CANOPY JETTISON - CHECK"),														message_timeout = dt_mto})
-- TODO: canopy jettison handle
push_start_command(dt,		{message = _("- MASTER ZEROIZE SWITCH - CHECK"),												message_timeout = dt * 6})
-- PLT
push_start_command(dt,		{device = devices.EMERGENCY_PANEL,		action = intercom_commands.PLT_MasterZeroizeSwCover,	value = 1.0})
push_start_command(dt,		{device = devices.EMERGENCY_PANEL,		action = intercom_commands.PLT_MasterZeroizeSw,			value = 0.0})
push_start_command(dt,		{device = devices.EMERGENCY_PANEL,		action = intercom_commands.PLT_MasterZeroizeSwCover,	value = 0.0})
-- CPG
push_start_command(dt,		{device = devices.EMERGENCY_PANEL,		action = intercom_commands.CPG_MasterZeroizeSwCover,	value = 1.0})
push_start_command(dt,		{device = devices.EMERGENCY_PANEL,		action = intercom_commands.CPG_MasterZeroizeSw,			value = 0.0})
push_start_command(dt,		{device = devices.EMERGENCY_PANEL,		action = intercom_commands.CPG_MasterZeroizeSwCover,	value = 0.0})
push_start_command(dt,		{message = _("- STANDBY FLIGHT INSTRUMENTS - CHECK"),											message_timeout = dt_mto})
push_start_command(dt,		{message = _("- STANDBY ATTITUDE INDICATOR - CAGE"),											message_timeout = dt_mto})
push_start_command(dt,		{device = devices.SAI,					action = sai_commands.CageKnobPull,						value = 1.0})
push_start_command(dt,		{message = _("- WIPER CONTROL - OFF"),															message_timeout = dt_mto})
-- PLT
push_start_command(dt,		{device = devices.CPT_MECH,				action = cpt_mech_commands.PLT_WiperSw,					value = 0.0})
-- CPG
push_start_command(dt,		{device = devices.CPT_MECH,				action = cpt_mech_commands.CPG_WiperSw,					value = 0.0})
-- TEDAC - CPG
push_start_command(dt,		{device = devices.TEDAC,				action = tedac_commands.TDU_MODE_KNOB,					value = 1.0})
-- Before starting APU - PILOT
push_start_command(dt,		{message = _("- BEFORE STARTING APU"),															message_timeout = dt * 11 + 2 * 4})
push_start_command(dt,		{message = _("- MSTR IGN SWITCH - BATT"),														message_timeout = dt_mto})
push_start_command(dt,		{device = devices.ELEC_INTERFACE,		action = electric_commands.MIK,							value = 0.5})
push_start_command(dt,		{message = _("- TAIL WHEEL BUTTON - VERIFY LOCKED"),											message_timeout = dt_mto})
-- PLT
push_start_command(dt,		{										check_condition = AH64_AD_TAIL_WHEEL_LOCK,				message_timeout = dt_mto})
push_start_command(dt,		{device = devices.HYDRO_INTERFACE,		action = hydraulic_commands.TailWheelUnLock_PLT,		value = 0.0})
-- CPG
push_start_command(dt,		{device = devices.HYDRO_INTERFACE,		action = hydraulic_commands.TailWheelUnLock_CPG,		value = 0.0})
push_start_command(dt,		{message = _("- ALL SIGNAL LIGHTS - CHECK"),													message_timeout = 4.5})
-- PLT
push_start_command(dt,		{device = devices.CPTLIGHTS_SYSTEM,		action = intlights_commands.TestLightsPLT,				value = 1.0})
push_start_command(2.0,		{device = devices.CPTLIGHTS_SYSTEM,		action = intlights_commands.TestLightsPLT,				value = 0.0})
-- CPG
push_start_command(dt,		{device = devices.CPTLIGHTS_SYSTEM,		action = intlights_commands.TestLightsCPG,				value = 1.0})
push_start_command(2.0,		{device = devices.CPTLIGHTS_SYSTEM,		action = intlights_commands.TestLightsCPG,				value = 0.0})
push_start_command(dt,		{message = _("- FIRE DET/EXTG - TEST"),															message_timeout = 9.0})
-- PLT
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.PLT_FireDetTestSw1,			value = -1.0})
push_start_command(2.0,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.PLT_FireDetTestSw1,			value = 0.0})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.PLT_FireDetTestSw2,			value = 1.0})
push_start_command(2.0,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.PLT_FireDetTestSw2,			value = 0.0})
-- CPG
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.CPG_FireDetTestSw1,			value = -1.0})
push_start_command(2.0,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.CPG_FireDetTestSw1,			value = 0.0})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.CPG_FireDetTestSw2,			value = 1.0})
push_start_command(2.0,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.CPG_FireDetTestSw2,			value = 0.0})
-- Starting APU - PILOT
push_start_command(dt,		{message = _("- STARTING APU"),																	message_timeout = 41.0})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.APU_StartBtnCover,				value = 1.0})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.APU_StartBtn,					value = 1.0})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.APU_StartBtn,					value = 0.0})
push_start_command(40.0,	{										check_condition = AH64_AD_APU_READY,					message_timeout = dt_mto})
-- After starting APU - PILOT
push_start_command(dt,		{message = _("- AFTER STARTING APU"),															message_timeout = dt * 3})
push_start_command(dt,		{message = _("- CANOPY DOOR - CLOSE"),															message_timeout = dt_mto})
-- PLT
push_start_command(dt,		{device = devices.CPT_MECH,				action = cpt_mech_commands.PLT_Door_Lock,				value = 0.0})
-- CPG
push_start_command(dt,		{device = devices.CPT_MECH,				action = cpt_mech_commands.CPG_Door_Lock,				value = 0.0})

push_start_command(dt,		{message = _("- IHADSS BORESIGHT"),																message_timeout = 2.0})
push_start_command(dt,		{										check_condition = AH64_AD_IHADSS_BORESIGHT,				message_timeout = dt_mto})

-- Before starting engines - PILOT
push_start_command(dt,		{message = _("- BEFORE STARTING ENGINES"),														message_timeout = dt * 10 + 2})
push_start_command(dt,		{message = _("- STANDBY ATTITUDE INDICATOR - UNCAGE"),											message_timeout = dt_mto})
push_start_command(2.0,		{device = devices.SAI,					action = sai_commands.CageKnobPull,						value = 0.0})
push_start_command(dt,		{message = _("- RTR BRK SWITCH - BRK"),															message_timeout = dt_mto})
push_start_command(dt,		{device = devices.HYDRO_INTERFACE,		action = hydraulic_commands.Rotor_Brake,				value = 0.0})
push_start_command(dt,		{message = _("- POWER LEVERS - OFF"),															message_timeout = dt_mto})
push_start_command(dt,		{										check_condition = AH64_AD_L_PCL_AT_OFF,					message_timeout = dt_mto})
push_start_command(dt,		{										check_condition = AH64_AD_R_PCL_AT_OFF,					message_timeout = dt_mto})
push_start_command(dt,		{message = _("- RTR BRK SWITCH - OFF"),															message_timeout = dt_mto})
push_start_command(dt,		{device = devices.HYDRO_INTERFACE,		action = hydraulic_commands.Rotor_Brake,				value = 1.0})
push_start_command(dt,		{message = _("- REDUCE COLLECTIVE TO FLAT PITCH"),												message_timeout = dt_mto})
push_start_command(dt,		{										check_condition = AH64_AD_COLLECTIVE_SET_DOWN,			message_timeout = dt_mto})
push_start_command(2.0,		{										check_condition = AH64_AD_COLLECTIVE_AT_DOWN,			message_timeout = dt_mto})
push_start_command(dt,		{message = _("- WAITING FOR EGI ALIGNMENT"),													message_timeout = dt_awt})
-- Starting engines - PILOT
push_start_command(dt_awt,	{message = _("- STARTING ENGINES"),																message_timeout = dt_es})
push_start_command(dt,		{message = _("- FIRST ENGINE"),																	message_timeout = dt_1es})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.Eng1StartSw,					value = 1.0})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.Eng1StartSw,					value = 0.0})
push_start_command(2.0,		{										check_condition = AH64_AD_L_PCL_SET_TO_IDLE,			message_timeout = dt_mto})
push_start_command(1.0,		{										check_condition = AH64_AD_L_PCL_AT_IDLE,				message_timeout = dt_mto})
-- TODO: check engine params
push_start_command(dt_1es,	{message = _("- SECOND ENGINE"),																message_timeout = dt_2es})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.Eng2StartSw,					value = 1.0})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.Eng2StartSw,					value = 0.0})
push_start_command(2.0,		{										check_condition = AH64_AD_R_PCL_SET_TO_IDLE,			message_timeout = dt_mto})
push_start_command(1.0,		{										check_condition = AH64_AD_R_PCL_AT_IDLE,				message_timeout = dt_mto})
-- TODO: check engine params
push_start_command(dt_2es,	{message = _("- POWER LEVERS - SMOOTHLY TO FLY"),												message_timeout = 10.0})
local SMOOTHLY_TO_FLY_N = 100
for i = 1, SMOOTHLY_TO_FLY_N, 1 do
	local PCL_IDLE	= 0.25
	local PCL_FLY	= 0.9
	local PCL_IDLE_TO_FLY	= PCL_FLY - PCL_IDLE
	local rel_pos = i / SMOOTHLY_TO_FLY_N
	local SMOOTHLY_TIME = 10.0
	local dt_SMOOTHLY = SMOOTHLY_TIME / SMOOTHLY_TO_FLY_N
	push_start_command(dt_SMOOTHLY,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.PLT_BothPowerLevers_EXT,		value = PCL_IDLE + PCL_IDLE_TO_FLY * rel_pos})
end
push_start_command(dt,		{										check_condition = AH64_AD_L_PCL_AT_FLY,					message_timeout = dt_mto})
push_start_command(dt,		{										check_condition = AH64_AD_L_PCL_AT_FLY,					message_timeout = dt_mto})
push_start_command(7.0,	{message = _("- Np and Nr - VERIFY 101%"),															message_timeout = dt_mto})
push_start_command(dt,		{										check_condition = AH64_AD_NpNr_VERIFY,					message_timeout = dt_mto})
push_start_command(dt,		{message = _("- APU OFF"),																		message_timeout = dt_mto})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.APU_StartBtn,					value = 1.0})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.APU_StartBtn,					value = 0.0})
push_start_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.APU_StartBtnCover,				value = 0.0})
push_start_command(dt,		{message = _("- BEFORE TAXI"),																	message_timeout = 3.0})
push_start_command(dt,		{message = _("- BLEED AIR - ON"),																message_timeout = 2.0})
push_start_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.AC,								value = 1.0})
push_start_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.AC,								value = 0.0})
push_start_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.T6,								value = 1.0})
push_start_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.T6,								value = 0.0})
push_start_command(dt,		{										check_condition = AH64_AD_BLEED_AIR_1_ON,				message_timeout = dt_mto})
push_start_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.B2,								value = 0.0})
push_start_command(dt,		{										check_condition = AH64_AD_BLEED_AIR_2_ON,				message_timeout = dt_mto})
push_start_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.B3,								value = 0.0})
push_start_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.T1,								value = 1.0})
push_start_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.T1,								value = 0.0})
push_start_command(dt,		{message = _("- PARK BRAKE - RELEASE"),															message_timeout = dt_mto})
push_start_command(dt,		{device = devices.GEAR_INTERFACE,		action = gear_commands.AH64_ParkingBrake,				value = 0.0})

--
push_start_command(3.0,	{message = _("AUTOSTART COMPLETE"),message_timeout = std_message_timeout})
--




----------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------
-- Stop sequence
push_stop_command(2.0,	{message = _("AUTOSTOP SEQUENCE IS RUNNING"),	message_timeout = stop_sequence_time})
--
push_stop_command(dt,		{message = _("- STARTING APU"),																	message_timeout = 41.0})
push_stop_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.APU_StartBtnCover,				value = 1.0})
push_stop_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.APU_StartBtn,					value = 1.0})
push_stop_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.APU_StartBtn,					value = 0.0})
push_stop_command(40.0,		{										check_condition = AH64_AD_APU_READY,					message_timeout = dt_mto})
push_stop_command(dt,		{message = _("- TAIL WHEEL BUTTON - LOCK"),														message_timeout = dt_mto})
-- PLT
push_stop_command(dt,		{										check_condition = AH64_AD_TAIL_WHEEL_LOCK,				message_timeout = dt_mto})
push_stop_command(dt,		{device = devices.HYDRO_INTERFACE,		action = hydraulic_commands.TailWheelUnLock_PLT,		value = 0.0})
-- CPG
push_stop_command(dt,		{device = devices.HYDRO_INTERFACE,		action = hydraulic_commands.TailWheelUnLock_CPG,		value = 0.0})
push_stop_command(dt,		{message = _("- PARK BRAKE - SET"),																message_timeout = dt_mto})
push_stop_command(dt,		{device = devices.GEAR_INTERFACE,		action = gear_commands.AH64_ParkingBrake,				value = 1.0})
push_stop_command(dt,		{message = _("- POWER LEVERS - IDLE"),															message_timeout = dt_mto})
local SMOOTHLY_TO_IDLE_N = 100
for i = 1, SMOOTHLY_TO_IDLE_N, 1 do
	local PCL_IDLE	= 0.25
	local PCL_FLY	= 0.9
	local PCL_IDLE_TO_FLY	= PCL_FLY - PCL_IDLE
	local rel_pos = 1.0 - i / SMOOTHLY_TO_IDLE_N
	local SMOOTHLY_TIME = 2.0
	local dt_SMOOTHLY = SMOOTHLY_TIME / SMOOTHLY_TO_IDLE_N
	push_stop_command(dt_SMOOTHLY,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.PLT_BothPowerLevers_EXT,		value = PCL_IDLE + PCL_IDLE_TO_FLY * rel_pos})
end
push_stop_command(dt,		{										check_condition = AH64_AD_L_PCL_AT_IDLE,				message_timeout = dt_mto})
push_stop_command(dt,		{										check_condition = AH64_AD_R_PCL_AT_IDLE,				message_timeout = dt_mto})
push_stop_command(dt,		{message = _("- STANDBY ATTITUDE INDICATOR - CAGE"),											message_timeout = dt_mto})
push_stop_command(dt,		{device = devices.SAI,					action = sai_commands.CageKnobPull,						value = 1.0})
push_stop_command(dt,		{message = _("- NVS MODE SWITCH - OFF"),														message_timeout = dt_mto})
-- PLT
push_stop_command(dt,		{device = devices.ELEC_INTERFACE,		action = electric_commands.NVS_MODE_PLT_KNOB,			value = -1.0})
-- CPG
push_stop_command(dt,		{device = devices.ELEC_INTERFACE,		action = electric_commands.NVS_MODE_CPG_KNOB,			value = -1.0})
--
push_stop_command(dt,		{message = _("- PNVS - OFF"),																	message_timeout = dt_mto})
push_stop_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.WPN,								value = 1.0})
push_stop_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.WPN,								value = 0.0})
push_stop_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.T6,								value = 1.0})
push_stop_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.T6,								value = 0.0})
push_stop_command(dt,		{										check_condition = AH64_AD_PNVS_OFF,						message_timeout = dt_mto})
push_stop_command(dt,		{device = devices.MFD_PLT_LEFT,			action = mpd_commands.L4,								value = 0.0})
push_stop_command(dt,		{message = _("- TADS, FCR - OFF"),																message_timeout = dt_mto})
push_stop_command(dt,		{device = devices.MFD_CPG_RIGHT,		action = mpd_commands.WPN,								value = 1.0})
push_stop_command(dt,		{device = devices.MFD_CPG_RIGHT,		action = mpd_commands.WPN,								value = 0.0})
push_stop_command(dt,		{device = devices.MFD_CPG_RIGHT,		action = mpd_commands.T6,								value = 1.0})
push_stop_command(dt,		{device = devices.MFD_CPG_RIGHT,		action = mpd_commands.T6,								value = 0.0})
push_stop_command(dt,		{										check_condition = AH64_AD_TADS_OFF,						message_timeout = dt_mto})
push_stop_command(dt,		{device = devices.MFD_CPG_RIGHT,		action = mpd_commands.L4,								value = 0.0})
push_stop_command(dt,		{										check_condition = AH64_AD_FCR_OFF,						message_timeout = dt_mto})
push_stop_command(dt,		{device = devices.MFD_CPG_RIGHT,		action = mpd_commands.L3,								value = 0.0})
push_stop_command(dt,		{message = _("- POWER LEVERS - OFF"),															message_timeout = (dt + 1) * 4})
push_stop_command(dt,		{										check_condition = AH64_AD_L_PCL_SET_TO_OFF,				message_timeout = dt_mto})
push_stop_command(1.0,		{										check_condition = AH64_AD_L_PCL_AT_OFF,					message_timeout = dt_mto})
push_stop_command(dt,		{										check_condition = AH64_AD_R_PCL_SET_TO_OFF,				message_timeout = dt_mto})
push_stop_command(1.0,		{										check_condition = AH64_AD_R_PCL_AT_OFF,					message_timeout = dt_mto})
push_stop_command(dt,		{message = _("- RTR BRK SWITCH - BRK (BELOW 50% Nr)"),											message_timeout = 5.0})
push_stop_command(5.0,		{device = devices.HYDRO_INTERFACE,		action = hydraulic_commands.Rotor_Brake,				value = 0.0})
push_stop_command(dt,		{message = _("- SEARCHLIGHT - OFF"),															message_timeout = dt_mto})
push_stop_command(dt,		{device = devices.HOTAS_PLT,			action = hotas_commands.FLIGHT_SEARCHLIGHT_SW_DOWN,		value = -1.0})
push_stop_command(5.0,		{device = devices.HOTAS_PLT,			action = hotas_commands.FLIGHT_SEARCHLIGHT_SW_DOWN,		value = 0.0})
push_stop_command(dt,		{message = _("- RTR BRK SWITCH - OFF (WHEN ROTOR STOPS)"),										message_timeout = 35.0})
push_stop_command(35.0,		{device = devices.HYDRO_INTERFACE,		action = hydraulic_commands.Rotor_Brake,				value = 1.0})
push_stop_command(dt,		{message = _("- EXT LT/INTR LT PANEL SWITCHES - OFF"),											message_timeout = dt_mto})
push_stop_command(dt,		{device = devices.EXTLIGHTS_SYSTEM,		action = extlights_commands.NavLights,					value = 0.0})
push_stop_command(dt,		{device = devices.EXTLIGHTS_SYSTEM,		action = extlights_commands.AntiCollLights,				value = 0.0})
push_stop_command(dt,		{message = _("- APU OFF"),																		message_timeout = dt_mto})
push_stop_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.APU_StartBtn,					value = 1.0})
push_stop_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.APU_StartBtn,					value = 0.0})
push_stop_command(dt,		{device = devices.ENGINE_INTERFACE,		action = engine_commands.APU_StartBtnCover,				value = 0.0})
push_stop_command(dt,		{message = _("- MSTR IGN SWITCH - OFF"),														message_timeout = dt_mto})
push_stop_command(dt,		{device = devices.ELEC_INTERFACE,		action = electric_commands.MIK,							value = 0.0})


--
push_stop_command(3.0,	{message = _("AUTOSTOP COMPLETE"),	message_timeout = std_message_timeout})
--