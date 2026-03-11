local cockpit = folder.."../../../Cockpit/Scripts/"
dofile(cockpit.."devices.lua")
dofile(cockpit.."command_defs.lua")

local res = external_profile("Config/Input/Aircrafts/common_joystick_binding.lua")

join(res.keyCommands,{


---------------------------------------------
-- Cheat ------------------------------------
---------------------------------------------
{	down = iCommandEnginesStart,													name = _('Auto Start'),		category = _('Cheat')},
{	down = iCommandEnginesStop,														name = _('Auto Stop'),		category = _('Cheat')},
{	down = device_commands.Action_10,	cockpit_device_id = 0,	value_down = 1.0,	name = _('Recorder On'),	category = _('Cheat')},
{	down = device_commands.Action_11,	cockpit_device_id = 0,	value_down = 1.0,	name = _('Recorder Off'),	category = _('Cheat')},

---------------------------------------------
-- General ----------------------------------
---------------------------------------------
{	down = iCommandCockpitShowPilotOnOff,	name = _('Show Pilot Body'),	category = _('General')},
{	down = iCommandDebriefing,				name = _('Debriefing Window'),	category = _('General')},
{	down = iCommandToggleMirrors,			name = _('Toggle Mirrors'),		category = _('General')},
{	down = head_wrapper_commands.LockCrewBody,	cockpit_device_id = devices.HEAD_WRAPPER,	value_down = -1.0,	name = _('Lock crew body position'),	category = _('General')},


{	down = iCommandViewCockpitChangeSeat,	value_down = 1, name = _('Occupy Pilot Seat'),			category = _('View Cockpit')},
{	down = iCommandViewCockpitChangeSeat,	value_down = 2, name = _('Occupy Copilot/Gunner Seat'),	category = _('View Cockpit')},

{	down = cpt_mech_commands.OpenCloseServiceHatches, cockpit_device_id = devices.CPT_MECH, value_down =  0.0, name = _('Service hatches - Open/Close'),	category = _('Systems')},
{	down = cpt_mech_commands.OpenCloseServiceHatches, cockpit_device_id = devices.CPT_MECH, value_down =  1.0, name = _('Service hatches - Open'),			category = _('Systems')},
{	down = cpt_mech_commands.OpenCloseServiceHatches, cockpit_device_id = devices.CPT_MECH, value_down = -1.0, name = _('Service hatches - Close'),			category = _('Systems')},


------------------------------------------------
-- Cockpit Mechanics ---------------------------
------------------------------------------------
{	down = cpt_mech_commands.MECH_Door_EXT,	cockpit_device_id = devices.CPT_MECH,	value_down = 1,	name = _('Open/Close Cockpit Door'),	category = {_('Systems')}},

{	pressed = cpt_mech_commands.PLT_SunvisorLeft_ITER,	cockpit_device_id = devices.CPT_MECH,	value_pressed = -1.0,	name = _('Left Sunvisor - Pull'),	category = {_('Systems')}},
{	pressed = cpt_mech_commands.PLT_SunvisorLeft_ITER,	cockpit_device_id = devices.CPT_MECH,	value_pressed =  1.0,	name = _('Left Sunvisor - Push'),	category = {_('Systems')}},
{	pressed = cpt_mech_commands.PLT_SunvisorRight_ITER,	cockpit_device_id = devices.CPT_MECH,	value_pressed = -1.0,	name = _('Right Sunvisor - Pull'),	category = {_('Systems')}},
{	pressed = cpt_mech_commands.PLT_SunvisorRight_ITER,	cockpit_device_id = devices.CPT_MECH,	value_pressed =  1.0,	name = _('Right Sunvisor - Push'),	category = {_('Systems')}},

------------------------------------------------
-- VOIP ---------------------------
------------------------------------------------
{down = iCommandVoIPRadioPushToTalkEnableDisableVoice,		up = iCommandVoIPRadioPushToTalkEnableDisableVoice,		value_down = 1.0, value_up = 0.0,	name = _('PTT/RTS Switch - RADIO (VOIP)'),	category = {_('Cyclic Stick'), _('Communications')}},
{down = iCommandVoIPIntercomPushToTalkEnableDisableVoice,	up = iCommandVoIPIntercomPushToTalkEnableDisableVoice,	value_down = 1.0, value_up = 0.0,	name = _('PTT/RTS Switch - ICS (VOIP)'),	category = {_('Cyclic Stick'), _('Communications')}},

------------------------------------------------
--NightVision-----------------------------------
------------------------------------------------
{	down = iCommandViewNightVisionGogglesOn,		name = _('Toggle goggles'),		category = _('Sensors')},
{	pressed = iCommandPlane_Helmet_Brightess_Up,	name = _('Gain goggles up'),	category = _('Sensors')},
{	pressed = iCommandPlane_Helmet_Brightess_Down,	name = _('Gain goggles down'),	category = _('Sensors')},

------------------------------------------------
-- Trimmer -------------------------------------
------------------------------------------------
{	down = iCommandPlaneTrimCancel,					name = _('Trimmer - Reset'),	category = {_('Trimmer'), _('Flight Control')}},

------------------------------------------------
-- Cyclic Stick --------------------------------
------------------------------------------------
{	down = iCommandPlaneDownStart,			up = iCommandPlaneDownStop,			name = _('Cyclic - Nose Down'),		category = {_('Cyclic Stick'), _('Flight Control')}},
{	down = iCommandPlaneUpStart,			up = iCommandPlaneUpStop,			name = _('Cyclic - Nose Up'),		category = {_('Cyclic Stick'), _('Flight Control')}},
{	down = iCommandPlaneLeftStart,			up = iCommandPlaneLeftStop,			name = _('Cyclic - Bank Left'),		category = {_('Cyclic Stick'), _('Flight Control')}},
{	down = iCommandPlaneRightStart,			up = iCommandPlaneRightStop,		name = _('Cyclic - Bank Right'),	category = {_('Cyclic Stick'), _('Flight Control')}},

------------------------------------------------
-- Ins Rudder ----------------------------------
------------------------------------------------
{	down = iCommandPlaneLeftRudderStart,	up = iCommandPlaneLeftRudderStop,	name = _('Rudder Left'),			category = {_('Rudder'), _('Flight Control')}},
{	down = iCommandPlaneRightRudderStart,	up = iCommandPlaneRightRudderStop,	name = _('Rudder Right'),			category = {_('Rudder'), _('Flight Control')}},

------------------------------------------------
-- Collective Stick ----------------------------
------------------------------------------------
{	down = iCommandPlaneCollectiveIncrease,	up = iCommandPlaneCollectiveStop,	name = _('Collective - Up'),		category = {_('Collective Stick'), _('Flight Control')}},
{	down = iCommandPlaneCollectiveDecrease,	up = iCommandPlaneCollectiveStop,	name = _('Collective - Down'),		category = {_('Collective Stick'), _('Flight Control')}},

------------------------------------------------
-- Cyclic Stick Grip ---------------------------
------------------------------------------------
{	down = hotas_commands.CYCLIC_TRIGGER_GUARD,																					cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,					name = _('Weapons Trigger Guard - OPEN'),					category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_TRIGGER_GUARD,																					cockpit_device_id = devices.HOTAS_INPUT,	value_down =  0.0,					name = _('Weapons Trigger Guard - CLOSE'),					category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_TRIGGER_GUARD_ITER,																			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,					name = _('Weapons Trigger Guard - OPEN/CLOSE'),				category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_TRIGGER_1ST_DETENT,				up = hotas_commands.CYCLIC_TRIGGER_1ST_DETENT,				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Weapons Trigger Switch - FIRST DETENT'),			category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_TRIGGER_2ND_DETENT,				up = hotas_commands.CYCLIC_TRIGGER_2ND_DETENT,				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Weapons Trigger Switch - SECOND DETENT'),			category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_TRIM_HOLD_SW_UP,					up = hotas_commands.CYCLIC_TRIM_HOLD_SW_UP,					cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Force Trim/Hold Mode Switch - R/Up'),				category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_TRIM_HOLD_SW_DOWN,					up = hotas_commands.CYCLIC_TRIM_HOLD_SW_DOWN,				cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Force Trim/Hold Mode Switch - D/Down'),			category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_TRIM_HOLD_SW_LEFT,					up = hotas_commands.CYCLIC_TRIM_HOLD_SW_LEFT,				cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Force Trim/Hold Mode Switch - AT/Left'),			category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_TRIM_HOLD_SW_RIGHT,				up = hotas_commands.CYCLIC_TRIM_HOLD_SW_RIGHT,				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Force Trim/Hold Mode Switch - AL/Right'),			category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_UP,				up = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_UP,			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Weapons Action Switch - G/Up'),					category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_DOWN,			up = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_DOWN,			cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Weapons Action Switch - A/Down'),					category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_LEFT,			up = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_LEFT,			cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Weapons Action Switch - R/Left'),					category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_RIGHT,			up = hotas_commands.CYCLIC_WEAPONS_ACTION_SW_RIGHT,			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Weapons Action Switch - M/Right'),				category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_SYMBOLOGY_SELECT_SW_UP,			up = hotas_commands.CYCLIC_SYMBOLOGY_SELECT_SW_UP,			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Symbology Select Switch - Up'),					category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_SYMBOLOGY_SELECT_SW_DOWN,			up = hotas_commands.CYCLIC_SYMBOLOGY_SELECT_SW_DOWN,		cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Symbology Select Switch - Down'),					category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_SYMBOLOGY_SELECT_SW_DEPRESS,		up = hotas_commands.CYCLIC_SYMBOLOGY_SELECT_SW_DEPRESS,		cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Symbology Select Switch - Depress'),				category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_CMDS_SW_FWD,						up = hotas_commands.CYCLIC_CMDS_SW_FWD,						cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('CMDS Switch - Fwd/Center'),						category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_CMDS_SW_AFT,						up = hotas_commands.CYCLIC_CMDS_SW_AFT,						cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('CMDS Switch - Aft/Center'),						category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_RTS_SW_LEFT,						up = hotas_commands.CYCLIC_RTS_SW_LEFT,						cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('PTT/RTS Switch - RADIO/Left (call radio menu)'),	category = {_('Cyclic Stick'), _('HOCAS'), _('Communications')}},
{	down = hotas_commands.CYCLIC_RTS_SW_RIGHT,						up = hotas_commands.CYCLIC_RTS_SW_RIGHT,					cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('PTT/RTS Switch - ICS/Right (call radio menu)'),	category = {_('Cyclic Stick'), _('HOCAS'), _('Communications')}},
{	down = hotas_commands.CYCLIC_RTS_SW_DEPRESS,					up = hotas_commands.CYCLIC_RTS_SW_DEPRESS,					cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('PTT/RTS Switch - RTS/Depress'),					category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_FMC_RELEASE_SW,					up = hotas_commands.CYCLIC_FMC_RELEASE_SW,					cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('FMC Release Button - Depress'),					category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_CHAFF_DISPENCE_BTN,				up = hotas_commands.CYCLIC_CHAFF_DISPENCE_BTN,				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Chaff Dispense Button - Depress'),				category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_FLARE_DISPENCE_BTN,				up = hotas_commands.CYCLIC_FLARE_DISPENCE_BTN,				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Flare Dispense Button - Depress'),				category = {_('Cyclic Stick'), _('HOCAS')}},
{	down = hotas_commands.CYCLIC_ATA_CAGE_UNCAGE_BTN,				up = hotas_commands.CYCLIC_ATA_CAGE_UNCAGE_BTN,				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('ATA Missile Cage/Uncage Button - Depress'),		category = {_('Cyclic Stick'), _('HOCAS')}},

------------------------------------------------
-- Collective Stick Mission Grip ---------------
------------------------------------------------
{	down = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_UP,				up = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_UP,			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('FCR Scan Size Switch - Z/Up'),					category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_DOWN,			up = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_DOWN,			cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('FCR Scan Size Switch - M/Down'),					category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_LEFT,			up = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_LEFT,			cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('FCR Scan Size Switch - N/Left'),					category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_RIGHT,			up = hotas_commands.MISSION_FCR_SCAN_SIZE_SW_RIGHT,			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('FCR Scan Size Switch - W/Right'),					category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_SIGHT_SELECT_SW_UP,				up = hotas_commands.MISSION_SIGHT_SELECT_SW_UP,				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Sight Select Switch - HMD/Up'),					category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_SIGHT_SELECT_SW_DOWN,				up = hotas_commands.MISSION_SIGHT_SELECT_SW_DOWN,			cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Sight Select Switch - LINK/Down'),				category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_SIGHT_SELECT_SW_LEFT,				up = hotas_commands.MISSION_SIGHT_SELECT_SW_LEFT,			cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Sight Select Switch - FCR/Left'),					category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_SIGHT_SELECT_SW_RIGHT,			up = hotas_commands.MISSION_SIGHT_SELECT_SW_RIGHT,			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Sight Select Switch - TADS/Right'),				category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_FCR_MODE_SW_UP,					up = hotas_commands.MISSION_FCR_MODE_SW_UP,					cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('FCR Mode Switch - GTM/Up'),						category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_FCR_MODE_SW_DOWN,					up = hotas_commands.MISSION_FCR_MODE_SW_DOWN,				cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('FCR Mode Switch - ATM/Down'),						category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_FCR_MODE_SW_LEFT,					up = hotas_commands.MISSION_FCR_MODE_SW_LEFT,				cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('FCR Mode Switch - TPM/Left'),						category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_FCR_MODE_SW_RIGHT,				up = hotas_commands.MISSION_FCR_MODE_SW_RIGHT,				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('FCR Mode Switch - RMAP/Right'),					category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_CURSOR_UP,						up = hotas_commands.MISSION_CURSOR_UP,						cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Cursor Controller - Up'),							category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_CURSOR_DOWN,						up = hotas_commands.MISSION_CURSOR_DOWN,					cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Cursor Controller - Down'),						category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_CURSOR_LEFT,						up = hotas_commands.MISSION_CURSOR_LEFT,					cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Cursor Controller - Left'),						category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_CURSOR_RIGHT,						up = hotas_commands.MISSION_CURSOR_RIGHT,					cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Cursor Controller - Right'),						category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_CURSOR_ENTER,						up = hotas_commands.MISSION_CURSOR_ENTER,					cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Cursor Enter - Depress'),							category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_ALTERNATE_CURSOR_ENTER,			up = hotas_commands.MISSION_ALTERNATE_CURSOR_ENTER,			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Alternate Cursor Enter - Depress'),				category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_CURSOR_DISPLAY_SELECT_BTN,		up = hotas_commands.MISSION_CURSOR_DISPLAY_SELECT_BTN,		cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Cursor Display Select Button - Depress'),			category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_FCR_SCAN_SW_SINGLE,				up = hotas_commands.MISSION_FCR_SCAN_SW_SINGLE,				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('FCR Scan Switch - S (Single)/Center'),			category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_FCR_SCAN_SW_CONTINUOUS,			up = hotas_commands.MISSION_FCR_SCAN_SW_CONTINUOUS,			cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('FCR Scan Switch - C (Continuous)/Center'),		category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_CUED_SEARCH_SW,					up = hotas_commands.MISSION_CUED_SEARCH_SW,					cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('CUED Search Switch - Depress'),					category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	down = hotas_commands.MISSION_MISSILE_ADVANCE_SW,				up = hotas_commands.MISSION_MISSILE_ADVANCE_SW,				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Missile Advance Switch - Depress'),				category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},

------------------------------------------------
-- Collective Stick Flight Grip ----------------
------------------------------------------------
{	down = hotas_commands.FLIGHT_EMERGENCY_JETTISON_SW_GUARD,																	cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,					name = _('Emergency Jettison Guard - OPEN'),				category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_EMERGENCY_JETTISON_SW_GUARD,																	cockpit_device_id = devices.HOTAS_INPUT,	value_down =  0.0,					name = _('Emergency Jettison Guard - CLOSE'),				category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_EMERGENCY_JETTISON_SW_GUARD_ITER,																cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,					name = _('Emergency Jettison Guard - OPEN/CLOSE'),			category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_EMERGENCY_JETTISON_SW,				up = hotas_commands.FLIGHT_EMERGENCY_JETTISON_SW,			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Emergency Jettison Switch - Depress'),			category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_NVS_SELECT_SW_TADS,				up = hotas_commands.FLIGHT_NVS_SELECT_SW_TADS,				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('NVS Select Switch - TADS'),						category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_NVS_SELECT_SW_PNVS,				up = hotas_commands.FLIGHT_NVS_SELECT_SW_PNVS,				cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('NVS Select Switch - PNVS'),						category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
--{	down = hotas_commands.FLIGHT_BORESIGHT_POLARITY_SW_BS,			up = hotas_commands.FLIGHT_BORESIGHT_POLARITY_SW_BS,		cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Boresight/Polarity Switch - B/S'),				category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_BORESIGHT_POLARITY_SW_PLRT,		up = hotas_commands.FLIGHT_BORESIGHT_POLARITY_SW_PLRT,		cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Boresight/Polarity Switch - PLRT'),				category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_STABILATOR_CONTROL_SW_NU,			up = hotas_commands.FLIGHT_STABILATOR_CONTROL_SW_NU,		cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Stabilator Control Switch - NU'),					category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_STABILATOR_CONTROL_SW_ND,			up = hotas_commands.FLIGHT_STABILATOR_CONTROL_SW_ND,		cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Stabilator Control Switch - ND'),					category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_STABILATOR_CONTROL_SW_DEPRESS,		up = hotas_commands.FLIGHT_STABILATOR_CONTROL_SW_DEPRESS,	cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Stabilator Control Switch - Depress'),			category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_SEARCHLIGHT_SW_UP,																				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,					name = _('Searchlight Switch - ON'),						category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_SEARCHLIGHT_SW_UP,																				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  0.0,					name = _('Searchlight Switch - OFF'),						category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_SEARCHLIGHT_SW_DOWN,				up = hotas_commands.FLIGHT_SEARCHLIGHT_SW_DOWN,				cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Searchlight Switch - STOW'),						category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_SEARCHLIGHT_SW_ITER,																			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,					name = _('Searchlight Switch - Up'),						category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_SEARCHLIGHT_SW_ITER,				up = hotas_commands.FLIGHT_SEARCHLIGHT_SW_ITER,				cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Searchlight Switch - Down'),						category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_UP,		up = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_UP,		cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Searchlight Position Switch - EXT/Up'),			category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_DOWN,		up = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_DOWN,	cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Searchlight Position Switch - RET/Down'),			category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_LEFT,		up = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_LEFT,	cockpit_device_id = devices.HOTAS_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('Searchlight Position Switch - L/Left'),			category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_RIGHT,		up = hotas_commands.FLIGHT_SEARCHLIGHT_POSITION_SW_RIGHT,	cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Searchlight Position Switch - R/Right'),			category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_CHOP_BTN_GUARD,																				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,					name = _('CHOP Button Guard - OPEN'),						category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_CHOP_BTN_GUARD,																				cockpit_device_id = devices.HOTAS_INPUT,	value_down =  0.0,					name = _('CHOP Button Guard - CLOSE'),						category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_CHOP_BTN_GUARD_ITER,																			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,					name = _('CHOP Button Guard - OPEN/CLOSE'),					category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_CHOP_BTN,							up = hotas_commands.FLIGHT_CHOP_BTN,						cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('CHOP Button - Depress'),							category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_TAIL_WHEEL_BTN,					up = hotas_commands.FLIGHT_TAIL_WHEEL_BTN,					cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('Tail Wheel Lock/Unlock Button - Depress'),		category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_BUCS_TRIGGER_GUARD,																			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,					name = _('BUCS Trigger Guard - OPEN'),						category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_BUCS_TRIGGER_GUARD,																			cockpit_device_id = devices.HOTAS_INPUT,	value_down =  0.0,					name = _('BUCS Trigger Guard - CLOSE'),						category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_BUCS_TRIGGER_GUARD_ITER,																		cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,					name = _('BUCS Trigger Guard - OPEN/CLOSE'),				category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},
{	down = hotas_commands.FLIGHT_BUCS_TRIGGER,						up = hotas_commands.FLIGHT_BUCS_TRIGGER,					cockpit_device_id = devices.HOTAS_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('BUCS Trigger - Depress'),							category = {_('Collective Stick'), _('Flight Grip'), _('HOCAS')}},

---------------------------------------------
-- Power Lever Quadrant ---------------------
---------------------------------------------
{	down = iCommandThrottleIncrease,			up = iCommandThrottleStop,		name = _('Power Lever Smoothly (Both) - Increase'),		category = {_('Power Lever Quadrant'), _('Flight Control')}},
{	down = iCommandThrottleDecrease,			up = iCommandThrottleStop,		name = _('Power Lever Smoothly (Both) - Decrease'),		category = {_('Power Lever Quadrant'), _('Flight Control')}},
{	down = iCommandThrottle1Increase,			up = iCommandThrottle1Stop,		name = _('Power Lever Smoothly (Left) - Increase'),		category = {_('Power Lever Quadrant'), _('Flight Control')}},
{	down = iCommandThrottle1Decrease,			up = iCommandThrottle1Stop,		name = _('Power Lever Smoothly (Left) - Decrease'),		category = {_('Power Lever Quadrant'), _('Flight Control')}},
{	down = iCommandThrottle2Increase,			up = iCommandThrottle2Stop,		name = _('Power Lever Smoothly (Right) - Increase'),	category = {_('Power Lever Quadrant'), _('Flight Control')}},
{	down = iCommandThrottle2Decrease,			up = iCommandThrottle2Stop,		name = _('Power Lever Smoothly (Right) - Decrease'),	category = {_('Power Lever Quadrant'), _('Flight Control')}},
{	down = iCommandPlaneAUTIncreaseRegime,										name = _('Power Lever Step (Both) - Increase'),			category = {_('Power Lever Quadrant'), _('Flight Control')}},
{	down = iCommandPlaneAUTDecreaseRegime,										name = _('Power Lever Step (Both) - Decrease'),			category = {_('Power Lever Quadrant'), _('Flight Control')}},
{	down = iCommandPlaneAUTIncreaseRegimeLeft,									name = _('Power Lever Step (Left) - Increase'),			category = {_('Power Lever Quadrant'), _('Flight Control')}},
{	down = iCommandPlaneAUTDecreaseRegimeLeft,									name = _('Power Lever Step (Left) - Decrease'),			category = {_('Power Lever Quadrant'), _('Flight Control')}},
{	down = iCommandPlaneAUTIncreaseRegimeRight,									name = _('Power Lever Step (Right) - Increase'),		category = {_('Power Lever Quadrant'), _('Flight Control')}},
{	down = iCommandPlaneAUTDecreaseRegimeRight,									name = _('Power Lever Step (Right) - Decrease'),		category = {_('Power Lever Quadrant'), _('Flight Control')}},
--
{	down = iCommandLeftEngineStart,																						name = _('Power Lever (Left) - IDLE'),			category = {_('Power Lever Quadrant')}},
{	down = iCommandLeftEngineStop,																						name = _('Power Lever (Left) - OFF'),			category = {_('Power Lever Quadrant')}},
{	down = iCommandRightEngineStart,																					name = _('Power Lever (Right) - IDLE'),			category = {_('Power Lever Quadrant')}},
{	down = iCommandRightEngineStop,																						name = _('Power Lever (Right) - OFF'),			category = {_('Power Lever Quadrant')}},
{	down = iCommandLeftEngineStop,				up = iCommandLeftEngineStart,		value_down =  1.0,	value_up = 1.0,	name = _('Power Lever (Left) - OFF/IDLE'),		category = {_('Power Lever Quadrant')}},
{	down = iCommandRightEngineStop,				up = iCommandRightEngineStart,		value_down =  1.0,	value_up = 1.0,	name = _('Power Lever (Right) - OFF/IDLE'),		category = {_('Power Lever Quadrant')}},
--
{	down = ctrl_commands.FingerLiftLeft_EXT,	up = ctrl_commands.FingerLiftLeft_EXT,	cockpit_device_id = devices.CONTROL_INTERFACE,	value_down =  1.0,		value_up = 0.0,	name = _('Power Lever Finger Lift (Left) - UP/DOWN'),					category = {_('Power Lever Quadrant')}},
{	down = ctrl_commands.FingerLiftRight_EXT,	up = ctrl_commands.FingerLiftRight_EXT,	cockpit_device_id = devices.CONTROL_INTERFACE,	value_down =  1.0,		value_up = 0.0,	name = _('Power Lever Finger Lift (Right) - UP/DOWN'),					category = {_('Power Lever Quadrant')}},
{	down = ctrl_commands.FingerLiftBoth_EXT,	up = ctrl_commands.FingerLiftBoth_EXT,	cockpit_device_id = devices.CONTROL_INTERFACE,	value_down =  1.0,		value_up = 0.0,	name = _('Power Lever Finger Lift (Both) - UP/DOWN'),					category = {_('Power Lever Quadrant')}},
{	down = ctrl_commands.LockoutDetent,													cockpit_device_id = devices.CONTROL_INTERFACE,	value_down =  1.0,						name = _('Cycle Lockout Detent - ON/OFF'),								category = {_('Power Lever Quadrant')}},


------------------------------------------------
-- COMM Panel ----------------------------------
------------------------------------------------
{	pressed = comm_commands.VHF_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = -0.2,	name = _('VHF Volume Control Knob - CCW/Decrease'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.VHF_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = 0.2,	name = _('VHF Volume Control Knob - CW/Increase'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.UHF_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = -0.2,	name = _('UHF Volume Control Knob - CCW/Decrease'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.UHF_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = 0.2,	name = _('UHF Volume Control Knob - CW/Increase'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.FM1_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = -0.2,	name = _('FM1 Volume Control Knob - CCW/Decrease'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.FM1_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = 0.2,	name = _('FM1 Volume Control Knob - CW/Increase'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.FM2_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = -0.2,	name = _('FM2 Volume Control Knob - CCW/Decrease'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.FM2_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = 0.2,	name = _('FM2 Volume Control Knob - CW/Increase'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.HF_volume_ITER,			cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = -0.2,	name = _('HF Volume Control Knob - CCW/Decrease'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.HF_volume_ITER,			cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = 0.2,	name = _('HF Volume Control Knob - CW/Increase'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.IFF_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = -0.2,	name = _('IFF Volume Control Knob - CCW/Decrease'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.IFF_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = 0.2,	name = _('IFF Volume Control Knob - CW/Increase'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.RLWR_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = -0.2,	name = _('RLWR Volume Control Knob - CCW/Decrease'),		category = {_('COMM Panel')}},
{	pressed = comm_commands.RLWR_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = 0.2,	name = _('RLWR Volume Control Knob - CW/Increase'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.ATA_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = -0.2,	name = _('ATA Volume Control Knob - CCW/Decrease'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.ATA_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = 0.2,	name = _('ATA Volume Control Knob - CW/Increase'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.VCR_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = -0.2,	name = _('VCR Volume Control Knob - CCW/Decrease'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.VCR_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = 0.2,	name = _('VCR Volume Control Knob - CW/Increase'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.ADF_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = -0.2,	name = _('ADF Volume Control Knob - CCW/Decrease'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.ADF_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = 0.2,	name = _('ADF Volume Control Knob - CW/Increase'),			category = {_('COMM Panel')}},
{	pressed = comm_commands.MASTER_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = -0.2,	name = _('MASTER Volume Control Knob - CCW/Decrease'),		category = {_('COMM Panel')}},
{	pressed = comm_commands.MASTER_volume_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = 0.2,	name = _('MASTER Volume Control Knob - CW/Increase'),		category = {_('COMM Panel')}},
{	pressed = comm_commands.SensControl_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = -0.2,	name = _('Sensitivity Control Knob - CCW/Decrease'),		category = {_('COMM Panel')}},
{	pressed = comm_commands.SensControl_ITER,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_pressed = 0.2,	name = _('Sensitivity Control Knob - CW/Increase'),			category = {_('COMM Panel')}},
{	down = comm_commands.VHF_SQL_ON_EXT,			up = comm_commands.VHF_SQL_ON_EXT,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('VHF Squelch Switch - ON'),  category = {_('COMM Panel')}},
{	down = comm_commands.VHF_SQL_OFF_EXT,			up = comm_commands.VHF_SQL_OFF_EXT,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('VHF Squelch Switch - OFF'), category = {_('COMM Panel')}},
{	down = comm_commands.UHF_SQL_ON_EXT,			up = comm_commands.UHF_SQL_ON_EXT,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('UHF Squelch Switch - ON'),  category = {_('COMM Panel')}},
{	down = comm_commands.UHF_SQL_OFF_EXT,			up = comm_commands.UHF_SQL_OFF_EXT,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('UHF Squelch Switch - OFF'), category = {_('COMM Panel')}},
{	down = comm_commands.FM1_SQL_ON_EXT,			up = comm_commands.FM1_SQL_ON_EXT,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('FM1 Squelch Switch - ON'),  category = {_('COMM Panel')}},
{	down = comm_commands.FM1_SQL_OFF_EXT,			up = comm_commands.FM1_SQL_OFF_EXT,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('FM1 Squelch Switch - OFF'), category = {_('COMM Panel')}},
{	down = comm_commands.FM2_SQL_ON_EXT,			up = comm_commands.FM2_SQL_ON_EXT,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('FM2 Squelch Switch - ON'),  category = {_('COMM Panel')}},
{	down = comm_commands.FM2_SQL_OFF_EXT,			up = comm_commands.FM2_SQL_OFF_EXT,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('FM2 Squelch Switch - OFF'), category = {_('COMM Panel')}},
{	down = comm_commands.HF_SQL_ON_EXT,				up = comm_commands.HF_SQL_ON_EXT,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down =  1.0,	value_up = 0.0,	name = _('HF Squelch Switch - ON'),   category = {_('COMM Panel')}},
{	down = comm_commands.HF_SQL_OFF_EXT,			up = comm_commands.HF_SQL_OFF_EXT,		cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down = -1.0,	value_up = 0.0,	name = _('HF Squelch Switch - OFF'),  category = {_('COMM Panel')}},
{	down = comm_commands.ICS_MODE_ITER,				cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down =  1.0,		name = _('ICS Mode Switch - Up'),							category = {_('COMM Panel')}},
{	down = comm_commands.ICS_MODE_ITER,				cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down = -1.0,		name = _('ICS Mode Switch - Down'),							category = {_('COMM Panel')}},
{	down = comm_commands.ICS_MODE,					cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down =  1.0,		name = _('ICS Mode Switch - PTT'),							category = {_('COMM Panel')}},
{	down = comm_commands.ICS_MODE,					cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down =  0.0,		name = _('ICS Mode Switch - VOX'),							category = {_('COMM Panel')}},
{	down = comm_commands.ICS_MODE,					cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down = -1.0,		name = _('ICS Mode Switch - HOT MIC'),						category = {_('COMM Panel')}},
{	down = comm_commands.IDENT,	up = comm_commands.IDENT,	cockpit_device_id = devices.COMM_PANEL_INPUT,	value_down = 1.0,	value_up = 0.0,		name = _('IDENT Button'),			category = {_('COMM Panel')}},


------------------------------------------------
-- ExternalLightSystem (only PLT) --------------
------------------------------------------------
{	down = extlights_commands.NavLights_EXT,			cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	value_down =  1.0,		name = _('Navigation Lights Switch - BRIGHT'),		category = {_('EXT LT/INTR LT Panel')}},
{	down = extlights_commands.NavLights_EXT,			cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	value_down =  0.0,		name = _('Navigation Lights Switch - OFF'),			category = {_('EXT LT/INTR LT Panel')}},
{	down = extlights_commands.NavLights_EXT,			cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	value_down = -1.0,		name = _('Navigation Lights Switch - DIM'),			category = {_('EXT LT/INTR LT Panel')}},
{	down = extlights_commands.NavLights_ITER,			cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	value_down =  1.0,		name = _('Navigation Lights Switch - Up'),			category = {_('EXT LT/INTR LT Panel')}},
{	down = extlights_commands.NavLights_ITER,			cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	value_down = -1.0,		name = _('Navigation Lights Switch - Down'),		category = {_('EXT LT/INTR LT Panel')}},
{	pressed = extlights_commands.FormationLights_ITER,	cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	value_pressed = -1.0,	name = _('Formation Lights Control Knob - CCW'),	category = {_('EXT LT/INTR LT Panel')}},
{	pressed = extlights_commands.FormationLights_ITER,	cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	value_pressed =  1.0,	name = _('Formation Lights Control Knob - CW'),		category = {_('EXT LT/INTR LT Panel')}},
{	down = extlights_commands.AntiCollLights_EXT,		cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	value_down =  1.0,		name = _('Anti-Collision Lights Switch - WHT'),		category = {_('EXT LT/INTR LT Panel')}},
{	down = extlights_commands.AntiCollLights_EXT,		cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	value_down =  0.0,		name = _('Anti-Collision Lights Switch - OFF'),		category = {_('EXT LT/INTR LT Panel')}},
{	down = extlights_commands.AntiCollLights_EXT,		cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	value_down = -1.0,		name = _('Anti-Collision Lights Switch - RED'),		category = {_('EXT LT/INTR LT Panel')}},
{	down = extlights_commands.AntiCollLights_ITER,		cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	value_down =  1.0,		name = _('Anti-Collision Lights Switch - Up'),		category = {_('EXT LT/INTR LT Panel')}},
{	down = extlights_commands.AntiCollLights_ITER,		cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	value_down = -1.0,		name = _('Anti-Collision Lights Switch - Down'),	category = {_('EXT LT/INTR LT Panel')}},

------------------------------------------------
-- InternalLightSystem -------------------------
------------------------------------------------
-- only PLT
{	pressed = intlights_commands.StbyInst_ITER,													cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_pressed =  1.0,						name = _('Standby Lights Control Knob - CW'),		category = {_('EXT LT/INTR LT Panel')}},
{	pressed = intlights_commands.StbyInst_ITER,													cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_pressed = -1.0,						name = _('Standby Lights Control Knob - CCW'),		category = {_('EXT LT/INTR LT Panel')}},
-- both PLT and CPG
{	down = intlights_commands.MasterCaution_EXT,	up = intlights_commands.MasterCaution_EXT,	cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_down = 1.0,		value_up = 0.0,		name = _('Master Caution Button'),					category = {_('EXT LT/INTR LT Panel')}},
{	down = intlights_commands.MasterWarning_EXT,	up = intlights_commands.MasterWarning_EXT,	cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_down = 1.0,		value_up = 0.0,		name = _('Master Warning Button'),					category = {_('EXT LT/INTR LT Panel')}},
{	down = intlights_commands.TestLights_EXT,		up = intlights_commands.TestLights_EXT,		cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_down = 1.0,		value_up = 0.0,		name = _('Press To Test Button'),					category = {_('EXT LT/INTR LT Panel')}},
{	pressed = intlights_commands.Signal_ITER,													cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_pressed =  1.0,						name = _('Signal Lights Control Knob - CW'),		category = {_('EXT LT/INTR LT Panel')}},
{	pressed = intlights_commands.Signal_ITER,													cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_pressed = -1.0,						name = _('Signal Lights Control Knob - CCW'),		category = {_('EXT LT/INTR LT Panel')}},
{	pressed = intlights_commands.Primary_ITER,													cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_pressed =  1.0,						name = _('Primary Lights Control Knob - CW'),		category = {_('EXT LT/INTR LT Panel')}},
{	pressed = intlights_commands.Primary_ITER,													cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_pressed = -1.0,						name = _('Primary Lights Control Knob - CCW'),		category = {_('EXT LT/INTR LT Panel')}},
{	pressed = intlights_commands.Flood_ITER,													cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_pressed =  1.0,						name = _('Flood Lights Control Knob - CW'),			category = {_('EXT LT/INTR LT Panel')}},
{	pressed = intlights_commands.Flood_ITER,													cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_pressed = -1.0,						name = _('Flood Lights Control Knob - CCW'),		category = {_('EXT LT/INTR LT Panel')}},
{	pressed = intlights_commands.Utility_ITER,													cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_pressed =  1.0,						name = _('Utility Lights Rheostat Control - CW'),	category = {_('Utility Light')}},
{	pressed = intlights_commands.Utility_ITER,													cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_pressed = -1.0,						name = _('Utility Lights Rheostat Control - CCW'),	category = {_('Utility Light')}},
{	down = intlights_commands.UtilityButton_EXT,	up = intlights_commands.UtilityButton_EXT,	cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	value_down = 1.0,		value_up = 0.0,		name = _('Press To Hold Brt Button'),				category = {_('Utility Light')}},

------------------------------------------------
-- Standby Instruments (only PLT) --------------
------------------------------------------------
-- Standby Attitude Indicator
{	down = sai_commands.CageKnobPull_EXT,			up = sai_commands.CageKnobPull_EXT,		cockpit_device_id = devices.SAI,				value_down = 1.0,		value_up = 0.0,		name = _('SAI Cage Knob - PULL'),			category = {_('Instrument Panel')}},
{	pressed = sai_commands.CageKnobRotate_ITER,												cockpit_device_id = devices.SAI,				value_pressed = -1.0,						name = _('SAI Cage Knob - CCW'),			category = {_('Instrument Panel')}},
{	pressed = sai_commands.CageKnobRotate_ITER,												cockpit_device_id = devices.SAI,				value_pressed =  1.0,						name = _('SAI Cage Knob - CW'),				category = {_('Instrument Panel')}},
-- Standby Altimeter
{	pressed = baro_alt_commands.PressureSet_ITER,											cockpit_device_id = devices.BARO_ALTIMETER,		value_pressed = -1.0,						name = _('Pressure Set Knob - CCW'),		category = {_('Instrument Panel')}},
{	pressed = baro_alt_commands.PressureSet_ITER,											cockpit_device_id = devices.BARO_ALTIMETER,		value_pressed =  1.0,						name = _('Pressure Set Knob - CW'),			category = {_('Instrument Panel')}},


-- WheelBrakes
-- only PLT
{	down = iCommandPlaneWheelParkingBrake,																			name = _('Parking Brake Handle - Pull/Stow'),	category = _('Systems')},
{	down = gear_commands.AH64_ParkingBrake_EXT,		cockpit_device_id = devices.GEAR_INTERFACE,	value_down = 1.0,	name = _('Parking Brake Handle - Pull'),		category = _('Systems')},
{	down = gear_commands.AH64_ParkingBrake_EXT,		cockpit_device_id = devices.GEAR_INTERFACE,	value_down = 0.0,	name = _('Parking Brake Handle - Stow'),		category = _('Systems')},
-- both PLT and CPG
{	down = iCommandPlaneWheelBrakeOn,		up = iCommandPlaneWheelBrakeOff,		value_down = 1.0,	value_up = 0.0,		name = _('Wheel Brake - ON/OFF'),			category = _('Systems')},
{	down = iCommandPlaneWheelBrakeLeftOn,	up = iCommandPlaneWheelBrakeLeftOff,	value_down = 1.0,	value_up = 0.0,		name = _('Wheel Brake Left - ON/OFF'),		category = _('Systems')},
{	down = iCommandPlaneWheelBrakeRightOn,	up = iCommandPlaneWheelBrakeRightOff,	value_down = 1.0,	value_up = 0.0,		name = _('Wheel Brake Right - ON/OFF'),		category = _('Systems')},

------------------------------------------------
-- Keyboard Unit -------------------------------
------------------------------------------------
{	down = KU_commands.keyA,				up = KU_commands.keyA,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - A'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyB,				up = KU_commands.keyB,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - B'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyC,				up = KU_commands.keyC,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - C'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyD,				up = KU_commands.keyD,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - D'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyE,				up = KU_commands.keyE,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - E'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyF,				up = KU_commands.keyF,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - F'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyG,				up = KU_commands.keyG,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - G'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyH,				up = KU_commands.keyH,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - H'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyI,				up = KU_commands.keyI,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - I'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyJ,				up = KU_commands.keyJ,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - J'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyK,				up = KU_commands.keyK,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - K'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyL,				up = KU_commands.keyL,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - L'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyM,				up = KU_commands.keyM,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - M'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyN,				up = KU_commands.keyN,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - N'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyO,				up = KU_commands.keyO,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - O'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyP,				up = KU_commands.keyP,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - P'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyQ,				up = KU_commands.keyQ,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - Q'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyR,				up = KU_commands.keyR,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - R'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyS,				up = KU_commands.keyS,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - S'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyT,				up = KU_commands.keyT,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - T'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyU,				up = KU_commands.keyU,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - U'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyV,				up = KU_commands.keyV,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - V'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyW,				up = KU_commands.keyW,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - W'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyX,				up = KU_commands.keyX,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - X'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyY,				up = KU_commands.keyY,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - Y'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyZ,				up = KU_commands.keyZ,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - Z'),				category = _('Keyboard Unit')},
{	down = KU_commands.keySlash,			up = KU_commands.keySlash,				cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - /'),				category = _('Keyboard Unit')},

{	down = KU_commands.key0,				up = KU_commands.key0,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - 0'),				category = _('Keyboard Unit')},
{	down = KU_commands.key1,				up = KU_commands.key1,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - 1'),				category = _('Keyboard Unit')},
{	down = KU_commands.key2,				up = KU_commands.key2,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - 2'),				category = _('Keyboard Unit')},
{	down = KU_commands.key3,				up = KU_commands.key3,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - 3'),				category = _('Keyboard Unit')},
{	down = KU_commands.key4,				up = KU_commands.key4,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - 4'),				category = _('Keyboard Unit')},
{	down = KU_commands.key5,				up = KU_commands.key5,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - 5'),				category = _('Keyboard Unit')},
{	down = KU_commands.key6,				up = KU_commands.key6,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - 6'),				category = _('Keyboard Unit')},
{	down = KU_commands.key7,				up = KU_commands.key7,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - 7'),				category = _('Keyboard Unit')},
{	down = KU_commands.key8,				up = KU_commands.key8,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - 8'),				category = _('Keyboard Unit')},
{	down = KU_commands.key9,				up = KU_commands.key9,					cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - 9'),				category = _('Keyboard Unit')},

{	down = KU_commands.keySign,				up = KU_commands.keySign,				cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - +/-'),			category = _('Keyboard Unit')},
{	down = KU_commands.keyDot,				up = KU_commands.keyDot,				cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - .'),				category = _('Keyboard Unit')},

{	down = KU_commands.keyPlus,				up = KU_commands.keyPlus,				cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - +'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyMinus,			up = KU_commands.keyMinus,				cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - -'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyMultiply,			up = KU_commands.keyMultiply,			cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - *'),				category = _('Keyboard Unit')},
{	down = KU_commands.keyDivide,			up = KU_commands.keyDivide,				cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - / (divide)'),	category = _('Keyboard Unit')},

{	down = KU_commands.keyEnter,			up = KU_commands.keyEnter,				cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - ENTER'),			category = _('Keyboard Unit')},
{	down = KU_commands.keySPC,				up = KU_commands.keySPC,				cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - SPC'),			category = _('Keyboard Unit')},
{	down = KU_commands.keyBKS,				up = KU_commands.keyBKS,				cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - BKS'),			category = _('Keyboard Unit')},
{	down = KU_commands.keyLeft,				up = KU_commands.keyLeft,				cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - Left'),			category = _('Keyboard Unit')},
{	down = KU_commands.keyRight,			up = KU_commands.keyRight,				cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - Right'),			category = _('Keyboard Unit')},
{	down = KU_commands.keyCLR,				up = KU_commands.keyCLR,				cockpit_device_id = devices.KU_INPUT,	value_down = 1.0,		value_up = 0.0,		name = _('KU Key - CLR'),			category = _('Keyboard Unit')},

{	pressed = KU_commands.BrightnessKnob_KB,										cockpit_device_id = devices.KU_INPUT,	value_pressed = -1.0,						name = _('KU Scratchpad Brightness Knob - CCW/Decrease'),	category = _('Keyboard Unit')},
{	pressed = KU_commands.BrightnessKnob_KB,										cockpit_device_id = devices.KU_INPUT,	value_pressed =  1.0,						name = _('KU Scratchpad Brightness Knob - CW/Increase'),	category = _('Keyboard Unit')},


------------------------------------------------
-- MPDS ----------------------------------------
------------------------------------------------
-- Left MFD
{	down = mpd_commands.T1,					up = mpd_commands.T1,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - T1'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.T2,					up = mpd_commands.T2,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - T2'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.T3,					up = mpd_commands.T3,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - T3'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.T4,					up = mpd_commands.T4,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - T4'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.T5,					up = mpd_commands.T5,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - T5'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.T6,					up = mpd_commands.T6,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - T6'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.R1,					up = mpd_commands.R1,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - R1'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.R2,					up = mpd_commands.R2,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - R2'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.R3,					up = mpd_commands.R3,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - R3'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.R4,					up = mpd_commands.R4,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - R4'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.R5,					up = mpd_commands.R5,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - R5'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.R6,					up = mpd_commands.R6,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - R6'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.B6,					up = mpd_commands.B6,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - B6'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.B5,					up = mpd_commands.B5,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - B5'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.B4,					up = mpd_commands.B4,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - B4'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.B3,					up = mpd_commands.B3,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - B3'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.B2,					up = mpd_commands.B2,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - B2'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.B1,					up = mpd_commands.B1,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - B1/M(Menu)'),					category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.L6,					up = mpd_commands.L6,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - L6'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.L5,					up = mpd_commands.L5,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - L5'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.L4,					up = mpd_commands.L4,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - L4'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.L3,					up = mpd_commands.L3,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - L3'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.L2,					up = mpd_commands.L2,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - L2'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.L1,					up = mpd_commands.L1,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - L1'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.Asterisk,			up = mpd_commands.Asterisk,	cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - Asterisk'),						category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.VID,				up = mpd_commands.VID,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - VID'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.COM,				up = mpd_commands.COM,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - COM'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.AC,					up = mpd_commands.AC,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - AC'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.TSD,				up = mpd_commands.TSD,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - TSD'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.WPN,				up = mpd_commands.WPN,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - WPN'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.FCR,				up = mpd_commands.FCR,		cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,	value_up = 0.0,		name = _('Left MPD Pushbutton - FCR'),							category = {_('Instrument Panel'), _('MPDS')}},
{	pressed = mpd_commands.BRT_KNOB_ITER,								cockpit_device_id = devices.MFD_INPUT_LEFT,		value_pressed =  0.5,					name = _('Left MPD Brightness Control Knob - CW/Increase'),		category = {_('Instrument Panel'), _('MPDS')}},
{	pressed = mpd_commands.BRT_KNOB_ITER,								cockpit_device_id = devices.MFD_INPUT_LEFT,		value_pressed = -0.5,					name = _('Left MPD Brightness Control Knob - CCW/Decrease'),	category = {_('Instrument Panel'), _('MPDS')}},
{	pressed = mpd_commands.VID_KNOB_ITER,								cockpit_device_id = devices.MFD_INPUT_LEFT,		value_pressed =  0.5,					name = _('Left MPD Video Control Knob - CW/Increase'),			category = {_('Instrument Panel'), _('MPDS')}},
{	pressed = mpd_commands.VID_KNOB_ITER,								cockpit_device_id = devices.MFD_INPUT_LEFT,		value_pressed = -0.5,					name = _('Left MPD Video Control Knob - CCW/Decrease'),			category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.MODE_KNOB_ITER,									cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down = -1.0,						name = _('Left MPD Mode Knob - CW'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.MODE_KNOB_ITER,									cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,						name = _('Left MPD Mode Knob - CCW'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.MODE_KNOB,										cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  1.0,						name = _('Left MPD Mode Knob - DAY'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.MODE_KNOB,										cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  0.5,						name = _('Left MPD Mode Knob - NT'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.MODE_KNOB,										cockpit_device_id = devices.MFD_INPUT_LEFT,		value_down =  0.0,						name = _('Left MPD Mode Knob - MONO'),							category = {_('Instrument Panel'), _('MPDS')}},
-- Right MFD
{	down = mpd_commands.T1,					up = mpd_commands.T1,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - T1'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.T2,					up = mpd_commands.T2,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - T2'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.T3,					up = mpd_commands.T3,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - T3'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.T4,					up = mpd_commands.T4,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - T4'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.T5,					up = mpd_commands.T5,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - T5'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.T6,					up = mpd_commands.T6,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - T6'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.R1,					up = mpd_commands.R1,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - R1'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.R2,					up = mpd_commands.R2,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - R2'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.R3,					up = mpd_commands.R3,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - R3'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.R4,					up = mpd_commands.R4,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - R4'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.R5,					up = mpd_commands.R5,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - R5'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.R6,					up = mpd_commands.R6,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - R6'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.B6,					up = mpd_commands.B6,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - B6'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.B5,					up = mpd_commands.B5,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - B5'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.B4,					up = mpd_commands.B4,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - B4'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.B3,					up = mpd_commands.B3,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - B3'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.B2,					up = mpd_commands.B2,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - B2'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.B1,					up = mpd_commands.B1,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - B1/M(Menu)'),					category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.L6,					up = mpd_commands.L6,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - L6'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.L5,					up = mpd_commands.L5,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - L5'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.L4,					up = mpd_commands.L4,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - L4'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.L3,					up = mpd_commands.L3,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - L3'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.L2,					up = mpd_commands.L2,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - L2'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.L1,					up = mpd_commands.L1,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - L1'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.Asterisk,			up = mpd_commands.Asterisk,	cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - Asterisk'),					category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.VID,				up = mpd_commands.VID,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - VID'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.COM,				up = mpd_commands.COM,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - COM'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.AC,					up = mpd_commands.AC,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - AC'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.TSD,				up = mpd_commands.TSD,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - TSD'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.WPN,				up = mpd_commands.WPN,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - WPN'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.FCR,				up = mpd_commands.FCR,		cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,	value_up = 0.0,		name = _('Right MPD Pushbutton - FCR'),							category = {_('Instrument Panel'), _('MPDS')}},
{	pressed = mpd_commands.BRT_KNOB_ITER,								cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_pressed =  0.5,					name = _('Right MPD Brightness Control Knob - CW/Increase'),	category = {_('Instrument Panel'), _('MPDS')}},
{	pressed = mpd_commands.BRT_KNOB_ITER,								cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_pressed = -0.5,					name = _('Right MPD Brightness Control Knob - CCW/Decrease'),	category = {_('Instrument Panel'), _('MPDS')}},
{	pressed = mpd_commands.VID_KNOB_ITER,								cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_pressed =  0.5,					name = _('Right MPD Video Control Knob - CW/Increase'),			category = {_('Instrument Panel'), _('MPDS')}},
{	pressed = mpd_commands.VID_KNOB_ITER,								cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_pressed = -0.5,					name = _('Right MPD Video Control Knob - CCW/Decrease'),		category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.MODE_KNOB_ITER,									cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down = -1.0,						name = _('Right MPD Mode Knob - CW'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.MODE_KNOB_ITER,									cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,						name = _('Right MPD Mode Knob - CCW'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.MODE_KNOB,										cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  1.0,						name = _('Right MPD Mode Knob - DAY'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.MODE_KNOB,										cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  0.5,						name = _('Right MPD Mode Knob - NT'),							category = {_('Instrument Panel'), _('MPDS')}},
{	down = mpd_commands.MODE_KNOB,										cockpit_device_id = devices.MFD_INPUT_RIGHT,	value_down =  0.0,						name = _('Right MPD Mode Knob - MONO'),							category = {_('Instrument Panel'), _('MPDS')}},

------------------------------------------------
-- Enhanced Up-Front Display (EUFD) ------------
------------------------------------------------
{	down = eufd_commands.WCA_UP,		up = eufd_commands.WCA_UP,		cockpit_device_id = devices.EUFD_INPUT,	value_down =  1.0,		value_up = 0.0,	name = _('WCA Rocker Switch - Up'),			category = {_('EUFD'),	_('Instrument Panel')}},
{	down = eufd_commands.WCA_DOWN,		up = eufd_commands.WCA_DOWN,	cockpit_device_id = devices.EUFD_INPUT,	value_down = -1.0,		value_up = 0.0,	name = _('WCA Rocker Switch - Down'),		category = {_('EUFD'),	_('Instrument Panel')}},
{	down = eufd_commands.IDM_UP,		up = eufd_commands.IDM_UP,		cockpit_device_id = devices.EUFD_INPUT,	value_down =  1.0,		value_up = 0.0,	name = _('IDM Rocker Switch - Up'),			category = {_('EUFD'),	_('Instrument Panel')}},
{	down = eufd_commands.IDM_DOWN,		up = eufd_commands.IDM_DOWN,	cockpit_device_id = devices.EUFD_INPUT,	value_down = -1.0,		value_up = 0.0,	name = _('IDM Rocker Switch - Down'),		category = {_('EUFD'),	_('Instrument Panel')}},
{	down = eufd_commands.RTS_UP,		up = eufd_commands.RTS_UP,		cockpit_device_id = devices.EUFD_INPUT,	value_down =  1.0,		value_up = 0.0,	name = _('RTS Rocker Switch - Up'),			category = {_('EUFD'),	_('Instrument Panel')}},
{	down = eufd_commands.RTS_DOWN,		up = eufd_commands.RTS_DOWN,	cockpit_device_id = devices.EUFD_INPUT,	value_down = -1.0,		value_up = 0.0,	name = _('RTS Rocker Switch - Down'),		category = {_('EUFD'),	_('Instrument Panel')}},
{	down = eufd_commands.Preset,		up = eufd_commands.Preset,		cockpit_device_id = devices.EUFD_INPUT,	value_down =  1.0,		value_up = 0.0,	name = _('Preset Button'),					category = {_('EUFD'),	_('Instrument Panel')}},
{	down = eufd_commands.Enter,			up = eufd_commands.Enter,		cockpit_device_id = devices.EUFD_INPUT,	value_down =  1.0,		value_up = 0.0,	name = _('Enter Button'),					category = {_('EUFD'),	_('Instrument Panel')}},
{	down = eufd_commands.Stopwatch,		up = eufd_commands.Stopwatch,	cockpit_device_id = devices.EUFD_INPUT,	value_down =  1.0,		value_up = 0.0,	name = _('Stopwatch Button'),				category = {_('EUFD'),	_('Instrument Panel')}},
{	down = eufd_commands.Swap,			up = eufd_commands.Swap,		cockpit_device_id = devices.EUFD_INPUT,	value_down =  1.0,		value_up = 0.0,	name = _('Swap Button'),					category = {_('EUFD'),	_('Instrument Panel')}},
{	pressed = eufd_commands.BRT_ITER,									cockpit_device_id = devices.EUFD_INPUT,	value_pressed =  1.0,					name = _('Brightness Control Knob - CW'),	category = {_('EUFD'),	_('Instrument Panel')}},
{	pressed = eufd_commands.BRT_ITER,									cockpit_device_id = devices.EUFD_INPUT,	value_pressed = -1.0,					name = _('Brightness Control Knob - CCW'),	category = {_('EUFD'),	_('Instrument Panel')}},

------------------------------------------------
-- Video Control Panel (PLT only) --------------
------------------------------------------------
{	pressed = electric_commands.VCP_FLIR_GAIN_KNOB_ITER,	cockpit_device_id = devices.ELEC_INTERFACE,	value_pressed =  1.0,	name = _('FLIR GAIN Control Knob - CW/Increase'),		category = {_('Instrument Panel'), _('Video Control Panel')}},
{	pressed = electric_commands.VCP_FLIR_GAIN_KNOB_ITER,	cockpit_device_id = devices.ELEC_INTERFACE,	value_pressed = -1.0 ,	name = _('FLIR GAIN Control Knob - CCW/Decrease'),		category = {_('Instrument Panel'), _('Video Control Panel')}},
{	pressed = electric_commands.VCP_FLIR_LEV_KNOB_ITER,		cockpit_device_id = devices.ELEC_INTERFACE,	value_pressed =  1.0,	name = _('FLIR LVL Control Knob - CW/Increase'),		category = {_('Instrument Panel'), _('Video Control Panel')}},
{	pressed = electric_commands.VCP_FLIR_LEV_KNOB_ITER,		cockpit_device_id = devices.ELEC_INTERFACE,	value_pressed = -1.0 ,	name = _('FLIR LVL Control Knob - CCW/Decrease'),		category = {_('Instrument Panel'), _('Video Control Panel')}},
{	down = electric_commands.VCP_ACM_SW_EXT,				cockpit_device_id = devices.ELEC_INTERFACE,	value_down =  1.0,		name = _('Automatic Contrast Mode Switch - ACM'),		category = {_('Instrument Panel'), _('Video Control Panel')}},
{	down = electric_commands.VCP_ACM_SW_EXT,				cockpit_device_id = devices.ELEC_INTERFACE,	value_down =  0.0,		name = _('Automatic Contrast Mode Switch - OFF'),		category = {_('Instrument Panel'), _('Video Control Panel')}},
{	down = electric_commands.VCP_ACM_SW_ITER,				cockpit_device_id = devices.ELEC_INTERFACE,	value_down =  1.0,		name = _('Automatic Contrast Mode Switch - ACM/OFF'),	category = {_('Instrument Panel'), _('Video Control Panel')}},
{	pressed = electric_commands.VCP_SYM_BRT_KNOB_ITER,		cockpit_device_id = devices.ELEC_INTERFACE,	value_pressed =  1.0,	name = _('SYM BRT Control Knob - CW/Increase'),			category = {_('Instrument Panel'), _('Video Control Panel')}},
{	pressed = electric_commands.VCP_SYM_BRT_KNOB_ITER,		cockpit_device_id = devices.ELEC_INTERFACE,	value_pressed = -1.0 ,	name = _('SYM BRT Control Knob - CCW/Decrease'),		category = {_('Instrument Panel'), _('Video Control Panel')}},
{	pressed = electric_commands.VCP_IHADSS_CON_KNOB_ITER,	cockpit_device_id = devices.ELEC_INTERFACE,	value_pressed =  1.0,	name = _('IHADSS CON Control Knob - CW/Increase'),		category = {_('Instrument Panel'), _('Video Control Panel')}},
{	pressed = electric_commands.VCP_IHADSS_CON_KNOB_ITER,	cockpit_device_id = devices.ELEC_INTERFACE,	value_pressed = -1.0 ,	name = _('IHADSS CON Control Knob - CCW/Decrease'),		category = {_('Instrument Panel'), _('Video Control Panel')}},
{	pressed = electric_commands.VCP_IHADSS_BRT_KNOB_ITER,	cockpit_device_id = devices.ELEC_INTERFACE,	value_pressed =  1.0,	name = _('IHADSS BRT Control Knob - CW/Increase'),		category = {_('Instrument Panel'), _('Video Control Panel')}},
{	pressed = electric_commands.VCP_IHADSS_BRT_KNOB_ITER,	cockpit_device_id = devices.ELEC_INTERFACE,	value_pressed = -1.0 ,	name = _('IHADSS BRT Control Knob - CCW/Decrease'),		category = {_('Instrument Panel'), _('Video Control Panel')}},

-----------------------------------------------------------------------
-- Check Overspeed Test / Generator Reset Panel (PLT only) ------------
-----------------------------------------------------------------------
{	down = electric_commands.GEN1_RST_SW_EXT,		up = electric_commands.GEN1_RST_SW_EXT,			cockpit_device_id = devices.ELEC_INTERFACE,		value_down = -1.0,	value_up = 0.0,	name = _('Generator Reset Switch - GEN 1/OFF'),					category = {_('Instrument Panel'), _('CHK OVSP TEST/GEN RST Panel')}},
{	down = electric_commands.GEN2_RST_SW_EXT,		up = electric_commands.GEN2_RST_SW_EXT,			cockpit_device_id = devices.ELEC_INTERFACE,		value_down =  1.0,	value_up = 0.0,	name = _('Generator Reset Switch - GEN 2/OFF'),					category = {_('Instrument Panel'), _('CHK OVSP TEST/GEN RST Panel')}},
{	down = engine_commands.ChkOvspTestSwENG1A_EXT,	up = engine_commands.ChkOvspTestSwENG1A_EXT,	cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,	value_up = 0.0,	name = _('CKT A Check Overspeed Test Switch - ENG 1/OFF'),		category = {_('Instrument Panel'), _('CHK OVSP TEST/GEN RST Panel')}},
{	down = engine_commands.ChkOvspTestSwENG2A_EXT,	up = engine_commands.ChkOvspTestSwENG2A_EXT,	cockpit_device_id = devices.ENGINE_INTERFACE,	value_down = -1.0,	value_up = 0.0,	name = _('CKT A Check Overspeed Test Switch - ENG 2/OFF'),		category = {_('Instrument Panel'), _('CHK OVSP TEST/GEN RST Panel')}},
{	down = engine_commands.ChkOvspTestSwENG1B_EXT,	up = engine_commands.ChkOvspTestSwENG1B_EXT,	cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,	value_up = 0.0,	name = _('CKT B Check Overspeed Test Switch - ENG 1/OFF'),		category = {_('Instrument Panel'), _('CHK OVSP TEST/GEN RST Panel')}},
{	down = engine_commands.ChkOvspTestSwENG2B_EXT,	up = engine_commands.ChkOvspTestSwENG2B_EXT,	cockpit_device_id = devices.ENGINE_INTERFACE,	value_down = -1.0,	value_up = 0.0,	name = _('CKT B Check Overspeed Test Switch - ENG 2/OFF'),		category = {_('Instrument Panel'), _('CHK OVSP TEST/GEN RST Panel')}},


------------------------------------------------
-- Stores Jettison Panel -----------------------
------------------------------------------------
{	down = JETT_commands.STORE_LO_JETTISON_ARMED,		up = JETT_commands.STORE_LO_JETTISON_ARMED,			cockpit_device_id = devices.JETT_PANEL_INPUT,	value_down = 1.0,	value_up = 0.0,	name = _('L OUTBD Station Select Pushbutton - ARM/SAFE'),	category = {_('Stores Jettison Panel')}},
{	down = JETT_commands.STORE_LI_JETTISON_ARMED,		up = JETT_commands.STORE_LI_JETTISON_ARMED,			cockpit_device_id = devices.JETT_PANEL_INPUT,	value_down = 1.0,	value_up = 0.0,	name = _('L INBD Station Select Pushbutton - ARM/SAFE'),	category = {_('Stores Jettison Panel')}},
{	down = JETT_commands.STORE_RI_JETTISON_ARMED,		up = JETT_commands.STORE_RI_JETTISON_ARMED,			cockpit_device_id = devices.JETT_PANEL_INPUT,	value_down = 1.0,	value_up = 0.0,	name = _('R INBD Station Select Pushbutton - ARM/SAFE'),	category = {_('Stores Jettison Panel')}},
{	down = JETT_commands.STORE_RO_JETTISON_ARMED,		up = JETT_commands.STORE_RO_JETTISON_ARMED,			cockpit_device_id = devices.JETT_PANEL_INPUT,	value_down = 1.0,	value_up = 0.0,	name = _('R OUTBD Station Select Pushbutton - ARM/SAFE'),	category = {_('Stores Jettison Panel')}},
{	down = JETT_commands.STORE_JETTISON_LEFT_WINGTIP,	up = JETT_commands.STORE_JETTISON_LEFT_WINGTIP,		cockpit_device_id = devices.JETT_PANEL_INPUT,	value_down = 1.0,	value_up = 0.0,	name = _('L TIP Station Select Pushbutton - ARM/SAFE'),		category = {_('Stores Jettison Panel')}},
{	down = JETT_commands.STORE_JETTISON_RIGHT_WINGTIP,	up = JETT_commands.STORE_JETTISON_RIGHT_WINGTIP,	cockpit_device_id = devices.JETT_PANEL_INPUT,	value_down = 1.0,	value_up = 0.0,	name = _('R TIP Station Select Pushbutton - ARM/SAFE'),		category = {_('Stores Jettison Panel')}},
{	down = JETT_commands.STORES_JETT_PUSHBUTTON,		up = JETT_commands.STORES_JETT_PUSHBUTTON,			cockpit_device_id = devices.JETT_PANEL_INPUT,	value_down = 1.0,	value_up = 0.0,	name = _('JETT Pushbutton'),								category = {_('Stores Jettison Panel')}},

------------------------------------------------
-- NVS Mode Panel ------------------------------
------------------------------------------------
{	down = electric_commands.NVS_MODE_KNOB_ITER,													cockpit_device_id = devices.ELEC_INTERFACE,		value_down =  1.0,					name = _('NVS MODE Switch - Up'),				category = {_('Left Console'), _('NVS Mode Panel')}},
{	down = electric_commands.NVS_MODE_KNOB_ITER,													cockpit_device_id = devices.ELEC_INTERFACE,		value_down = -1.0,					name = _('NVS MODE Switch - Down'),				category = {_('Left Console'), _('NVS Mode Panel')}},
{	down = electric_commands.NVS_MODE_KNOB_EXT,														cockpit_device_id = devices.ELEC_INTERFACE,		value_down =  1.0,					name = _('NVS MODE Switch - FIXED'),			category = {_('Left Console'), _('NVS Mode Panel')}},
{	down = electric_commands.NVS_MODE_KNOB_EXT,														cockpit_device_id = devices.ELEC_INTERFACE,		value_down =  0.0,					name = _('NVS MODE Switch - NORM'),				category = {_('Left Console'), _('NVS Mode Panel')}},
{	down = electric_commands.NVS_MODE_KNOB_EXT,														cockpit_device_id = devices.ELEC_INTERFACE,		value_down = -1.0,					name = _('NVS MODE Switch - OFF'),				category = {_('Left Console'), _('NVS Mode Panel')}},
{	down = hydraulic_commands.TailWheelUnLock_EXT,	up = hydraulic_commands.TailWheelUnLock_EXT,	cockpit_device_id = devices.HYDRO_INTERFACE,	value_down =  1.0,	value_up = 0.0,	name = _('TAIL WHEEL Pushbutton - LOCK/UNLOCK'),	category = {_('Left Console'), _('NVS Mode Panel')}},

------------------------------------------------
-- Windshield Panel ----------------------------
------------------------------------------------
{	down = cpt_mech_commands.WiperSw_ITER,											cockpit_device_id = devices.CPT_MECH,	value_down =  1.0,					name = _('Wiper Control Switch - CW'),		category = {_('Windshield Panel')}},
{	down = cpt_mech_commands.WiperSw_ITER,	up = cpt_mech_commands.WiperSw_ITER,	cockpit_device_id = devices.CPT_MECH,	value_down = -1.0,	value_up = 0.0,	name = _('Wiper Control Switch - CCW'),		category = {_('Windshield Panel')}},
{	down = cpt_mech_commands.WiperSw_EXT,	up = cpt_mech_commands.WiperSw_EXT,		cockpit_device_id = devices.CPT_MECH,	value_down = -0.1,	value_up = 0.0,	name = _('Wiper Control Switch - PARK'),	category = {_('Windshield Panel')}},
{	down = cpt_mech_commands.WiperSw_EXT,											cockpit_device_id = devices.CPT_MECH,	value_down =  0.0,					name = _('Wiper Control Switch - OFF'),		category = {_('Windshield Panel')}},
{	down = cpt_mech_commands.WiperSw_EXT,											cockpit_device_id = devices.CPT_MECH,	value_down =  0.1,					name = _('Wiper Control Switch - LO'),		category = {_('Windshield Panel')}},
{	down = cpt_mech_commands.WiperSw_EXT,											cockpit_device_id = devices.CPT_MECH,	value_down =  0.2,					name = _('Wiper Control Switch - HI'),		category = {_('Windshield Panel')}},
{	down = cpt_mech_commands.DefogBtn_EXT,	up = cpt_mech_commands.DefogBtn_EXT,	cockpit_device_id = devices.CPT_MECH,	value_down = 1.0,	value_up = 0.0,	name = _('Defog Button'),					category = {_('Windshield Panel')}},

------------------------------------------------
-- PILOT POWER LEVER QUADRANT (PLT only) -------
------------------------------------------------
{	down = electric_commands.MIK_ITER,															cockpit_device_id = devices.ELEC_INTERFACE,		value_down =  1.0,					name = _('Master Ignition Switch - CW'),							category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = electric_commands.MIK_ITER,															cockpit_device_id = devices.ELEC_INTERFACE,		value_down = -1.0,					name = _('Master Ignition Switch - CCW'),							category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = electric_commands.MIK_EXT,															cockpit_device_id = devices.ELEC_INTERFACE,		value_down =  0.0,					name = _('Master Ignition Switch - OFF'),							category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = electric_commands.MIK_EXT,															cockpit_device_id = devices.ELEC_INTERFACE,		value_down =  0.5,					name = _('Master Ignition Switch - BATT'),							category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = electric_commands.MIK_EXT,															cockpit_device_id = devices.ELEC_INTERFACE,		value_down =  1.0,					name = _('Master Ignition Switch - EXT PWR'),						category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = hydraulic_commands.Rotor_Brake_Sw_ITER,												cockpit_device_id = devices.HYDRO_INTERFACE,	value_down =  1.0,					name = _('Rotor Brake Switch - Up'),								category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = hydraulic_commands.Rotor_Brake_Sw_ITER,												cockpit_device_id = devices.HYDRO_INTERFACE,	value_down = -1.0,					name = _('Rotor Brake Switch - Down'),								category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = hydraulic_commands.Rotor_Brake_Sw_EXT,												cockpit_device_id = devices.HYDRO_INTERFACE,	value_down =  1.0,					name = _('Rotor Brake Switch - OFF'),								category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = hydraulic_commands.Rotor_Brake_Sw_EXT,												cockpit_device_id = devices.HYDRO_INTERFACE,	value_down =  0.0,					name = _('Rotor Brake Switch - BRK'),								category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = hydraulic_commands.Rotor_Brake_Sw_EXT,												cockpit_device_id = devices.HYDRO_INTERFACE,	value_down = -1.0,					name = _('Rotor Brake Switch - LOCK'),								category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = engine_commands.APU_StartBtnCover_ITER,												cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,					name = _('APU Pushbutton Cover - OPEN/CLOSE'),						category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = engine_commands.APU_StartBtnCover_EXT,												cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,					name = _('APU Pushbutton Cover - OPEN'),							category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = engine_commands.APU_StartBtnCover_EXT,												cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  0.0,					name = _('APU Pushbutton Cover - CLOSE'),							category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = engine_commands.APU_StartBtn_EXT,		up = engine_commands.APU_StartBtn_EXT,		cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,	value_up = 0.0,	name = _('APU Pushbutton - Start/Stop'),							category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = engine_commands.Eng1StartSw_EXT,			up = engine_commands.Eng1StartSw_EXT,		cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,	value_up = 0.0,	name = _('No.1 Engine Start Switch - START'),						category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = engine_commands.Eng1IgnOrideSw_EXT,		up = engine_commands.Eng1IgnOrideSw_EXT,	cockpit_device_id = devices.ENGINE_INTERFACE,	value_down = -1.0,	value_up = 0.0,	name = _('No.1 Engine Start Switch - IGN ORIDE'),					category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = engine_commands.Eng2StartSw_EXT,			up = engine_commands.Eng2StartSw_EXT,		cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,	value_up = 0.0,	name = _('No.2 Engine Start Switch - START'),						category = {_('Left Console'), _('Power Lever Quadrant')}},
{	down = engine_commands.Eng2IgnOrideSw_EXT,		up = engine_commands.Eng2IgnOrideSw_EXT,	cockpit_device_id = devices.ENGINE_INTERFACE,	value_down = -1.0,	value_up = 0.0,	name = _('No.2 Engine Start Switch - IGN ORIDE'),					category = {_('Left Console'), _('Power Lever Quadrant')}},
{	pressed = ctrl_commands.FrictionLever_ITER,													cockpit_device_id = devices.CONTROL_INTERFACE,	value_pressed =  1.0,				name = _('Power Lever Friction Adjustment Lever - Fwd/Increase'),	category = {_('Left Console'), _('Power Lever Quadrant')}},
{	pressed = ctrl_commands.FrictionLever_ITER,													cockpit_device_id = devices.CONTROL_INTERFACE,	value_pressed = -1.0,				name = _('Power Lever Friction Adjustment Lever - Aft/Decrease'),	category = {_('Left Console'), _('Power Lever Quadrant')}},

------------------------------------------------
-- Armament Panel ------------------------------
------------------------------------------------
{	down = electric_commands.ARM_SAFE_BTN_EXT,	up = electric_commands.ARM_SAFE_BTN_EXT,	cockpit_device_id = devices.ELEC_INTERFACE,	value_down = 1.0,	value_up = 0.0,	name = _('A/S Pushbutton'),			category = {_('Instrument Panel'), _('Armament Panel')}},
{	down = electric_commands.GND_ORIDE_BTN_EXT,	up = electric_commands.GND_ORIDE_BTN_EXT,	cockpit_device_id = devices.ELEC_INTERFACE,	value_down = 1.0,	value_up = 0.0,	name = _('GND ORIDE Pushbutton'),	category = {_('Instrument Panel'), _('Armament Panel')}},

------------------------------------------------
-- Fire Detection/Extinguishing Panel ----------
------------------------------------------------
{	down = engine_commands.Eng1FireBtnCover_ITER,												cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,					name = _('ENG 1 Fire Pushbutton Cover - OPEN/CLOSE'),		category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.Eng1FireBtnCover_EXT,												cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,					name = _('ENG 1 Fire Pushbutton Cover - OPEN'),				category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.Eng1FireBtnCover_EXT,												cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  0.0,					name = _('ENG 1 Fire Pushbutton Cover - CLOSE'),			category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.Eng1FireBtn_EXT,			up = engine_commands.Eng1FireBtn_EXT,		cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,	value_up = 0.0,	name = _('ENG 1 Fire Pushbutton'),							category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.ApuFireBtnCover_ITER,												cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,					name = _('APU Fire Pushbutton Cover - OPEN/CLOSE'),			category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.ApuFireBtnCover_EXT,													cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,					name = _('APU Fire Pushbutton Cover - OPEN'),				category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.ApuFireBtnCover_EXT,													cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  0.0,					name = _('APU Fire Pushbutton Cover - CLOSE'),				category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.ApuFireBtn_EXT,			up = engine_commands.ApuFireBtn_EXT,		cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,	value_up = 0.0,	name = _('APU Fire Pushbutton'),							category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.Eng2FireBtnCover_ITER,												cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,					name = _('ENG 2 Fire Pushbutton Cover - OPEN/CLOSE'),		category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.Eng2FireBtnCover_EXT,												cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,					name = _('ENG 2 Fire Pushbutton Cover - OPEN'),				category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.Eng2FireBtnCover_EXT,												cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  0.0,					name = _('ENG 2 Fire Pushbutton Cover - CLOSE'),			category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.Eng2FireBtn_EXT,			up = engine_commands.Eng2FireBtn_EXT,		cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,	value_up = 0.0,	name = _('ENG 2 Fire Pushbutton'),							category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.PrimaryDischBtn_EXT,		up = engine_commands.PrimaryDischBtn_EXT,	cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,	value_up = 0.0,	name = _('Primary Fire Extinguisher Discharge Pushbutton'),	category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.ReserveDischBtn_EXT,		up = engine_commands.ReserveDischBtn_EXT,	cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,	value_up = 0.0,	name = _('Reserve Fire Extinguisher Discharge Pushbutton'),	category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.FireDetTestSw1_EXT,		up = engine_commands.FireDetTestSw1_EXT,	cockpit_device_id = devices.ENGINE_INTERFACE,	value_down = -1.0,	value_up = 0.0,	name = _('Fire Detection Circuit Test Switch - 1'),			category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},
{	down = engine_commands.FireDetTestSw2_EXT,		up = engine_commands.FireDetTestSw2_EXT,	cockpit_device_id = devices.ENGINE_INTERFACE,	value_down =  1.0,	value_up = 0.0,	name = _('Fire Detection Circuit Test Switch - 2'),			category = {_('Instrument Panel'), _('Fire Detection/Extinguishing Panel')}},

------------------------------------------------
-- CMWS Control Panel --------------------------
------------------------------------------------
{	down = CMWS_commands.CMWS_PWR_ITER,				up = CMWS_commands.CMWS_PWR_ITER,		cockpit_device_id = devices.CMWS,	value_down =  1.0,		value_up =  0.0,	name = _('CMWS PWR Switch - CW'),							category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_PWR_ITER,														cockpit_device_id = devices.CMWS,	value_down = -1.0,							name = _('CMWS PWR Switch - CCW'),							category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_PWR_EXT,														cockpit_device_id = devices.CMWS,	value_down = -1.0,							name = _('CMWS PWR Switch - OFF'),							category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_PWR_EXT,														cockpit_device_id = devices.CMWS,	value_down =  0.0,							name = _('CMWS PWR Switch - ON'),							category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_PWR_EXT,				up = CMWS_commands.CMWS_PWR_EXT,		cockpit_device_id = devices.CMWS,	value_down =  1.0,		value_up =  0.0,	name = _('CMWS PWR Switch - TEST'),							category = {_('CMWS Control Panel')}},
{	pressed = CMWS_commands.CMWS_AUDIO_KNOB_ITER,											cockpit_device_id = devices.CMWS,	value_pressed =  1.0,						name = _('CMWS Audio Volume Knob - CW/Increase'),			category = {_('CMWS Control Panel')}},
{	pressed = CMWS_commands.CMWS_AUDIO_KNOB_ITER,											cockpit_device_id = devices.CMWS,	value_pressed = -1.0,						name = _('CMWS Audio Volume Knob - CCW/Decrease'),			category = {_('CMWS Control Panel')}},
{	pressed = CMWS_commands.CMWS_LAMP_KNOB_ITER,											cockpit_device_id = devices.CMWS,	value_pressed =  1.0,						name = _('CMWS Lamp Knob - CW/Increase'),					category = {_('CMWS Control Panel')}},
{	pressed = CMWS_commands.CMWS_LAMP_KNOB_ITER,											cockpit_device_id = devices.CMWS,	value_pressed = -1.0,						name = _('CMWS Lamp Knob - CCW/Decrease'),					category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_ARM_SAFE_SW_ITER,												cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Flare Squibs Switch - ARM/SAFE'),			category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_ARM_SAFE_SW_EXT,												cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Flare Squibs Switch - ARM'),					category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_ARM_SAFE_SW_EXT,												cockpit_device_id = devices.CMWS,	value_down =  0.0,							name = _('CMWS Flare Squibs Switch - SAFE'),				category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_CMWS_NAV_SW_ITER,												cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Mode Switch - CMWS/NAV'),					category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_CMWS_NAV_SW_EXT,												cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Mode Switch - CMWS'),						category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_CMWS_NAV_SW_EXT,												cockpit_device_id = devices.CMWS,	value_down =  0.0,							name = _('CMWS Mode Switch - NAV'),							category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_BYPASS_AUTO_SW_ITER,											cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Operation Switch - BYPASS/AUTO'),			category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_BYPASS_AUTO_SW_EXT,											cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Operation Switch - BYPASS'),					category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_BYPASS_AUTO_SW_EXT,											cockpit_device_id = devices.CMWS,	value_down =  0.0,							name = _('CMWS Operation Switch - AUTO'),					category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_JETT_COVER_ITER,												cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Flare Jettison Switch Cover - OPEN/CLOSE'),	category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_JETT_COVER_EXT,												cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Flare Jettison Switch Cover - OPEN'),		category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_JETT_COVER_EXT,												cockpit_device_id = devices.CMWS,	value_down =  0.0,							name = _('CMWS Flare Jettison Switch Cover - CLOSE'),		category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_JETT_SW_EXT,			up = CMWS_commands.CMWS_JETT_SW_EXT,	cockpit_device_id = devices.CMWS,	value_down =  1.0,		value_up = 0.0,		name = _('CMWS Flare Jettison Switch'),						category = {_('CMWS Control Panel')}},

------------------------------------------------
-- Emergency Panel -----------------------------
------------------------------------------------
{	down = intercom_commands.UHF_GUARD_Btn_EXT,		up = intercom_commands.UHF_GUARD_Btn_EXT,	cockpit_device_id = devices.EMERGENCY_PANEL,			value_down = 1.0,	value_up = 0.0,	name = _('Guard Button - ON/OFF'),			category = {_('Emergency Panel')}},
{	down = intercom_commands.XPNDR_Btn_EXT,			up = intercom_commands.XPNDR_Btn_EXT,		cockpit_device_id = devices.EMERGENCY_PANEL,			value_down = 1.0,	value_up = 0.0,	name = _('XPNDR Button - ON/OFF'),			category = {_('Emergency Panel')}},
{	down = intercom_commands.ZEROIZE_Sw_ITER,													cockpit_device_id = devices.EMERGENCY_PANEL,			value_down =  1.0,					name = _('Zeroize Switch - ON/OFF'),		category = {_('Emergency Panel')}},
{	down = intercom_commands.ZEROIZE_Sw_EXT,													cockpit_device_id = devices.EMERGENCY_PANEL,			value_down =  1.0,					name = _('Zeroize Switch - ON'),			category = {_('Emergency Panel')}},
{	down = intercom_commands.ZEROIZE_Sw_EXT,													cockpit_device_id = devices.EMERGENCY_PANEL,			value_down =  0.0,					name = _('Zeroize Switch - OFF'),			category = {_('Emergency Panel')}},
{	down = hydraulic_commands.Emergency_HYD_EXT,	up = hydraulic_commands.Emergency_HYD_EXT,	cockpit_device_id = devices.HYDRO_INTERFACE,	value_down = 1.0,	value_up = 0.0,	name = _('EMERG HYD Pushbutton - ON/OFF'),	category = {_('Emergency Panel')}},


------------------------------------------------
-- Master Zeroize ------------------------------
------------------------------------------------
{	down = intercom_commands.MasterZeroizeSwCover_ITER,		cockpit_device_id = devices.EMERGENCY_PANEL,	value_down =  1.0,	name = _('Master Zeroize Switch Cover - OPEN/CLOSE'),	category = {_('Instrument Panel')}},
{	down = intercom_commands.MasterZeroizeSwCover_EXT,		cockpit_device_id = devices.EMERGENCY_PANEL,	value_down =  1.0,	name = _('Master Zeroize Switch Cover - OPEN'),			category = {_('Instrument Panel')}},
{	down = intercom_commands.MasterZeroizeSwCover_EXT,		cockpit_device_id = devices.EMERGENCY_PANEL,	value_down =  0.0,	name = _('Master Zeroize Switch Cover - CLOSE'),		category = {_('Instrument Panel')}},
{	down = intercom_commands.MasterZeroizeSw_ITER,			cockpit_device_id = devices.EMERGENCY_PANEL,	value_down =  1.0,	name = _('Master Zeroize Switch - ON/OFF'),				category = {_('Instrument Panel')}},
{	down = intercom_commands.MasterZeroizeSw_EXT,			cockpit_device_id = devices.EMERGENCY_PANEL,	value_down =  1.0,	name = _('Master Zeroize Switch - ON'),					category = {_('Instrument Panel')}},
{	down = intercom_commands.MasterZeroizeSw_EXT,			cockpit_device_id = devices.EMERGENCY_PANEL,	value_down =  0.0,	name = _('Master Zeroize Switch - OFF'),				category = {_('Instrument Panel')}},


-----------------------------------------------
-- IHADSS -------------------------------------
-----------------------------------------------
{combos = {{key = 'I'}},	down = hmd_commands.HMD_SHOW,	cockpit_device_id = devices.HMD,	value_down = 1.0,	name = _('IHADSS show'),	category = _('General')},
-- Display Adjust Panels
{	down = hmd_commands.DAP_SIZE_ITER,		cockpit_device_id = devices.HMD,	value_down = -1.0,	name = _('DAP SIZE Knob - CW/Increase'),				category = {_('Display Adjust Panel')}},
{	down = hmd_commands.DAP_SIZE_ITER,		cockpit_device_id = devices.HMD,	value_down =  1.0,	name = _('DAP SIZE Knob - CCW/Decrease'),				category = {_('Display Adjust Panel')}},
{	down = hmd_commands.DAP_V_CTRG_ITER,	cockpit_device_id = devices.HMD,	value_down = -1.0,	name = _('DAP VERTICAL CTRG Knob - CW/Down'),			category = {_('Display Adjust Panel')}},
{	down = hmd_commands.DAP_V_CTRG_ITER,	cockpit_device_id = devices.HMD,	value_down =  1.0,	name = _('DAP VERTICAL CTRG Knob - CCW/Up'),			category = {_('Display Adjust Panel')}},
{	down = hmd_commands.DAP_H_CTRG_ITER,	cockpit_device_id = devices.HMD,	value_down = -1.0,	name = _('DAP HORIZONTAL CTRG Knob - CW/Left'),			category = {_('Display Adjust Panel')}},
{	down = hmd_commands.DAP_H_CTRG_ITER,	cockpit_device_id = devices.HMD,	value_down =  1.0,	name = _('DAP HORIZONTAL CTRG Knob - CCW/Right'),		category = {_('Display Adjust Panel')}},


------------------------------------------------
-- Preston -------------------------------------
------------------------------------------------
{	down = preston_commands.ControlRequest,											cockpit_device_id = devices.PrestonAI,	value_down = 1.0,					name = _('Request Aircraft Control'),					category = {_('George AI'), _('Multicrew')}},
{	down = preston_commands.ShowHideMenu,	up = preston_commands.ShowHideMenu,		cockpit_device_id = devices.PrestonAI,	value_down = 1.0,	value_up = 0,	name = _('George AI - Show/Hide'),						category = {_('George AI')}},
{	down = preston_commands.MfInput,		up = preston_commands.MfInput,			cockpit_device_id = devices.PrestonAI,	value_down = 1.0,	value_up = 0,	name = _('George AI - Multifunctional Input (Center)'),	category = {_('George AI')}},
{	down = preston_commands.StoreTarget, cockpit_device_id = devices.PrestonAI,	value_down = 1.0,	name = _('George AI - TADS Store Target'),	category = {_('George AI')}},




})

-- joystick axes
join(res.axisCommands,{

{combos = defaultDeviceAssignmentFor("roll"),	action = iCommandPlaneRoll,			name = _('Cyclic Roll')},
{combos = defaultDeviceAssignmentFor("pitch"),	action = iCommandPlanePitch,		name = _('Cyclic Pitch')},
{combos = defaultDeviceAssignmentFor("rudder"),	action = iCommandPlaneRudder,		name = _('Rudder')},
{combos = defaultDeviceAssignmentFor("thrust"),	action = iCommandPlaneCollective,	name = _('Collective')},

---------------------------------------------
-- Systems ----------------------------------
---------------------------------------------
{	action = iCommandWheelBrake,		name = _('Wheels Brake Both')},
{	action = iCommandLeftWheelBrake,	name = _('Wheel Brake Left')},
{	action = iCommandRightWheelBrake,	name = _('Wheel Brake Right')},

---------------------------------------------
-- Power Lever Quadrant ---------------------
---------------------------------------------
-- both PLT and CPG
{	action = iCommandPlaneThrustLeft,																name = _('Power Lever (Left)'),						category = {_('Power Lever Quadrant')}},
{	action = iCommandPlaneThrustRight,																name = _('Power Lever (Right)'),					category = {_('Power Lever Quadrant')}},
{	action = iCommandPlaneThrustCommon,																name = _('Power Levers (Both)'),					category = {_('Power Lever Quadrant')}},
-- only PLT
{	action = ctrl_commands.FrictionLever_AXIS,				cockpit_device_id = devices.CONTROL_INTERFACE,	name = _('Power Levers Friction Adjusting Lever'),	category = {_('Power Lever Quadrant')}},

---------------------------------------------
-- HOTAS ------------------------------------
---------------------------------------------
-- both PLT and CPG
{	action = hotas_commands.MISSION_CURSOR_AXIS_X,			cockpit_device_id = devices.HOTAS_INPUT,		name = _('HOCAS Cursor Controller - X axis'),		category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},
{	action = hotas_commands.MISSION_CURSOR_AXIS_Y,			cockpit_device_id = devices.HOTAS_INPUT,		name = _('HOCAS Cursor Controller - Y axis'),		category = {_('Collective Stick'), _('Mission Grip'), _('HOCAS')}},


---------------------------------------------
-- Video Control Panel (VCP) ----------------
---------------------------------------------
-- only PLT
{	action = electric_commands.VCP_FLIR_LEV_KNOB_AXIS,		cockpit_device_id = devices.ELEC_INTERFACE,		name = _('VCP FLIR LVL Control Knob'),				category = {_('Instrument Panel'), _('Video Control Panel')}},
{	action = electric_commands.VCP_FLIR_GAIN_KNOB_AXIS,		cockpit_device_id = devices.ELEC_INTERFACE,		name = _('VCP FLIR GAIN Control Knob'),				category = {_('Instrument Panel'), _('Video Control Panel')}},
{	action = electric_commands.VCP_IHADSS_CON_KNOB_AXIS,	cockpit_device_id = devices.ELEC_INTERFACE,		name = _('VCP IHADSS CON Control Knob'),			category = {_('Instrument Panel'), _('Video Control Panel')}},
{	action = electric_commands.VCP_IHADSS_BRT_KNOB_AXIS,	cockpit_device_id = devices.ELEC_INTERFACE,		name = _('VCP IHADSS BRT Control Knob'),			category = {_('Instrument Panel'), _('Video Control Panel')}},
{	action = electric_commands.VCP_SYM_BRT_KNOB_AXIS,		cockpit_device_id = devices.ELEC_INTERFACE,		name = _('VCP SYM BRT Control Knob'),				category = {_('Instrument Panel'), _('Video Control Panel')}},

------------------------------------------------
-- CMWS Control Panel (PLT only) ---------------
------------------------------------------------
{	down = CMWS_commands.CMWS_PWR_ITER,				up = CMWS_commands.CMWS_PWR_ITER,		cockpit_device_id = devices.CMWS,	value_down =  1.0,		value_up =  0.0,	name = _('CMWS PWR Switch - CW'),							category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_PWR_ITER,														cockpit_device_id = devices.CMWS,	value_down = -1.0,							name = _('CMWS PWR Switch - CCW'),							category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_PWR_EXT,														cockpit_device_id = devices.CMWS,	value_down = -1.0,							name = _('CMWS PWR Switch - OFF'),							category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_PWR_EXT,														cockpit_device_id = devices.CMWS,	value_down =  0.0,							name = _('CMWS PWR Switch - ON'),							category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_PWR_EXT,				up = CMWS_commands.CMWS_PWR_EXT,		cockpit_device_id = devices.CMWS,	value_down =  1.0,		value_up =  0.0,	name = _('CMWS PWR Switch - TEST'),							category = {_('CMWS Control Panel')}},
{	pressed = CMWS_commands.CMWS_AUDIO_KNOB_ITER,											cockpit_device_id = devices.CMWS,	value_pressed =  1.0,						name = _('CMWS Audio Volume Knob - CW/Increase'),			category = {_('CMWS Control Panel')}},
{	pressed = CMWS_commands.CMWS_AUDIO_KNOB_ITER,											cockpit_device_id = devices.CMWS,	value_pressed = -1.0,						name = _('CMWS Audio Volume Knob - CCW/Decrease'),			category = {_('CMWS Control Panel')}},
{	pressed = CMWS_commands.CMWS_LAMP_KNOB_ITER,											cockpit_device_id = devices.CMWS,	value_pressed =  1.0,						name = _('CMWS Lamp Knob - CW/Increase'),					category = {_('CMWS Control Panel')}},
{	pressed = CMWS_commands.CMWS_LAMP_KNOB_ITER,											cockpit_device_id = devices.CMWS,	value_pressed = -1.0,						name = _('CMWS Lamp Knob - CCW/Decrease'),					category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_ARM_SAFE_SW_ITER,												cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Flare Squibs Switch - ARM/SAFE'),			category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_ARM_SAFE_SW_EXT,												cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Flare Squibs Switch - ARM'),					category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_ARM_SAFE_SW_EXT,												cockpit_device_id = devices.CMWS,	value_down =  0.0,							name = _('CMWS Flare Squibs Switch - SAFE'),				category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_CMWS_NAV_SW_ITER,												cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Mode Switch - CMWS/NAV'),					category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_CMWS_NAV_SW_EXT,												cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Mode Switch - CMWS'),						category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_CMWS_NAV_SW_EXT,												cockpit_device_id = devices.CMWS,	value_down =  0.0,							name = _('CMWS Mode Switch - NAV'),							category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_BYPASS_AUTO_SW_ITER,											cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Operation Switch - BYPASS/AUTO'),			category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_BYPASS_AUTO_SW_EXT,											cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Operation Switch - BYPASS'),					category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_BYPASS_AUTO_SW_EXT,											cockpit_device_id = devices.CMWS,	value_down =  0.0,							name = _('CMWS Operation Switch - AUTO'),					category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_JETT_COVER_ITER,												cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Flare Jettison Switch Cover - OPEN/CLOSE'),	category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_JETT_COVER_EXT,												cockpit_device_id = devices.CMWS,	value_down =  1.0,							name = _('CMWS Flare Jettison Switch Cover - OPEN'),		category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_JETT_COVER_EXT,												cockpit_device_id = devices.CMWS,	value_down =  0.0,							name = _('CMWS Flare Jettison Switch Cover - CLOSE'),		category = {_('CMWS Control Panel')}},
{	down = CMWS_commands.CMWS_JETT_SW_EXT,			up = CMWS_commands.CMWS_JETT_SW_EXT,	cockpit_device_id = devices.CMWS,	value_down =  1.0,		value_up = 0.0,		name = _('CMWS Flare Jettison Switch'),						category = {_('CMWS Control Panel')}},


------------------------------------------------
-- ExternalLightSystem (only PLT) --------------
------------------------------------------------
{	action = extlights_commands.FormationLights_AXIS,		cockpit_device_id = devices.EXTLIGHTS_SYSTEM,	name = _('Formation Lights Control Knob'),			category = {_('EXT LT/INTR LT Panel')}},

------------------------------------------------
-- InternalLightSystem -------------------------
------------------------------------------------
-- only PLT
{	action = intlights_commands.StbyInst_AXIS,				cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	name = _('Standby Lights Control Knob'),			category = {_('EXT LT/INTR LT Panel')}},
{	action = intlights_commands.Signal_AXIS,				cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	name = _('Signal Lights Control Knob'),				category = {_('EXT LT/INTR LT Panel')}},
{	action = intlights_commands.Primary_AXIS,				cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	name = _('Primary Lights Control Knob'),			category = {_('EXT LT/INTR LT Panel')}},
{	action = intlights_commands.Flood_AXIS,					cockpit_device_id = devices.CPTLIGHTS_SYSTEM,	name = _('Flood Lights Control Knob'),				category = {_('EXT LT/INTR LT Panel')}},

------------------------------------------------
-- MPDS ----------------------------------------
------------------------------------------------
-- both PLT and CPG
{	action = mpd_commands.BRT_KNOB_AXIS,					cockpit_device_id = devices.MFD_INPUT_LEFT,		name = _('Left MPD Brightness Control Knob'),		category = {_('Instrument Panel'), _('MPDS')}},
{	action = mpd_commands.VID_KNOB_AXIS,					cockpit_device_id = devices.MFD_INPUT_LEFT,		name = _('Left MPD Video Control Knob'),			category = {_('Instrument Panel'), _('MPDS')}},
{	action = mpd_commands.BRT_KNOB_AXIS,					cockpit_device_id = devices.MFD_INPUT_RIGHT,	name = _('Right MPD Brightness Control Knob'),		category = {_('Instrument Panel'), _('MPDS')}},
{	action = mpd_commands.VID_KNOB_AXIS,					cockpit_device_id = devices.MFD_INPUT_RIGHT,	name = _('Right MPD Video Control Knob'),			category = {_('Instrument Panel'), _('MPDS')}},

------------------------------------------------
-- Keyboard Unit -------------------------------
------------------------------------------------
-- both PLT and CPG
{	action = KU_commands.BrightnessKnob_AXIS,				cockpit_device_id = devices.KU_INPUT,			name = _('KU Scratchpad Brightness Knob'),			category = _('Keyboard Unit')},

------------------------------------------------
-- Enhanced Up-Front Display (EUFD) ------------
------------------------------------------------
-- both PLT and CPG
{	action = eufd_commands.BRT_AXIS,						cockpit_device_id = devices.EUFD_INPUT,			name = _('EUFD Brightness Control Knob'),			category = {_('EUFD'),	_('Instrument Panel')}},

------------------------------------------------
-- COMM Panel ----------------------------------
------------------------------------------------
-- both PLT and CPG
{	action = comm_commands.VHF_volume_AXIS,					cockpit_device_id = devices.COMM_PANEL_INPUT,	name = _('COMM VHF Volume Control Knob'),			category = {_('COMM Panel')}},
{	action = comm_commands.UHF_volume_AXIS,					cockpit_device_id = devices.COMM_PANEL_INPUT,	name = _('COMM UHF Volume Control Knob'),			category = {_('COMM Panel')}},
{	action = comm_commands.FM1_volume_AXIS,					cockpit_device_id = devices.COMM_PANEL_INPUT,	name = _('COMM FM1 Volume Control Knob'),			category = {_('COMM Panel')}},
{	action = comm_commands.FM2_volume_AXIS,					cockpit_device_id = devices.COMM_PANEL_INPUT,	name = _('COMM FM2 Volume Control Knob'),			category = {_('COMM Panel')}},
{	action = comm_commands.HF_volume_AXIS,					cockpit_device_id = devices.COMM_PANEL_INPUT,	name = _('COMM HF Volume Control Knob'),			category = {_('COMM Panel')}},
{	action = comm_commands.IFF_volume_AXIS,					cockpit_device_id = devices.COMM_PANEL_INPUT,	name = _('COMM IFF Volume Control Knob'),			category = {_('COMM Panel')}},
{	action = comm_commands.RLWR_volume_AXIS,				cockpit_device_id = devices.COMM_PANEL_INPUT,	name = _('COMM RLWR Volume Control Knob'),			category = {_('COMM Panel')}},
{	action = comm_commands.ATA_volume_AXIS,					cockpit_device_id = devices.COMM_PANEL_INPUT,	name = _('COMM ATA Volume Control Knob'),			category = {_('COMM Panel')}},
{	action = comm_commands.VCR_volume_AXIS,					cockpit_device_id = devices.COMM_PANEL_INPUT,	name = _('COMM VCR Volume Control Knob'),			category = {_('COMM Panel')}},
{	action = comm_commands.ADF_volume_AXIS,					cockpit_device_id = devices.COMM_PANEL_INPUT,	name = _('COMM ADF Volume Control Knob'),			category = {_('COMM Panel')}},
{	action = comm_commands.MASTER_volume_AXIS,				cockpit_device_id = devices.COMM_PANEL_INPUT,	name = _('COMM MASTER Volume Control Knob'),		category = {_('COMM Panel')}},
{	action = comm_commands.SensControl_AXIS,				cockpit_device_id = devices.COMM_PANEL_INPUT,	name = _('COMM Sensitivity Control Knob'),			category = {_('COMM Panel')}},

{	action = cpt_mech_commands.PLT_SunvisorLeft_AXIS,		cockpit_device_id = devices.CPT_MECH,			name = _('Left Sunvisor'),							category = {_('Systems')}},
{	action = cpt_mech_commands.PLT_SunvisorRight_AXIS,		cockpit_device_id = devices.CPT_MECH,			name = _('Right Sunvisor'),							category = {_('Systems')}},


})

return res
