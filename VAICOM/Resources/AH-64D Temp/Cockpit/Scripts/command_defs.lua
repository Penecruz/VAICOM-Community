-- Commands : not intended for end-user editing
Keys =
{
	iCommandPlaneWheelBrakeOn		= 74,
	iCommandPlaneWheelBrakeOff		= 75,

	iCommandPlaneThrustCommon		= 2004,

}

start_command = 3000
local count = 0
local function counter()
	count = count + 1
	return count
end
local function reset_counter()
	count = start_command
end


reset_counter()
device_commands = {}
for cmd_num = 1,70 do
	device_commands["Action_"..cmd_num] = counter()
end

reset_counter()
electric_commands =
{
	GEN1_RST_SW					= counter();
	GEN2_RST_SW					= counter();
	MIK							= counter();
	SP_SELECT_SW				= counter();

--Night Vision Mode Panel------------------------------
	NVS_MODE_PLT_KNOB			= counter();
	NVS_MODE_CPG_KNOB			= counter();

--Video Control Panel----------------------------------
	VCP_FLIR_GAIN_KNOB			= counter();
	VCP_FLIR_LEV_KNOB			= counter();
	VCP_ACM_SW					= counter();
	VCP_IHADSS_CON_KNOB			= counter();
	VCP_IHADSS_BRT_KNOB			= counter();
	VCP_SYM_BRT_KNOB			= counter();
-- Atmament Panel-------------------------------------
	GND_ORIDE_PLT_BTN			= counter();
	ARM_SAFE_PLT_BTN			= counter();
	GND_ORIDE_CPG_BTN			= counter();
	ARM_SAFE_CPG_BTN			= counter();
--input commands
	GEN1_RST_SW_EXT				= counter();
	GEN2_RST_SW_EXT				= counter();
	MIK_EXT						= counter();
	MIK_ITER					= counter();
	SP_SELECT_SW_EXT			= counter();
	SP_SELECT_SW_ITER			= counter();

--input commands for Night Vision Mode Panel------------------------------
	NVS_MODE_KNOB_EXT			= counter();
	NVS_MODE_KNOB_ITER			= counter();

--input commands for Video Control Panel----------------------------------
	VCP_FLIR_GAIN_KNOB_ITER		= counter();
	VCP_FLIR_LEV_KNOB_ITER		= counter();
	VCP_ACM_SW_ITER				= counter();
	VCP_IHADSS_CON_KNOB_ITER	= counter();
	VCP_IHADSS_BRT_KNOB_ITER	= counter();
	VCP_SYM_BRT_KNOB_ITER		= counter();
	VCP_FLIR_LEV_KNOB_AXIS		= counter();
	VCP_FLIR_GAIN_KNOB_AXIS		= counter();
	VCP_IHADSS_CON_KNOB_AXIS	= counter();
	VCP_IHADSS_BRT_KNOB_AXIS	= counter();
	VCP_SYM_BRT_KNOB_AXIS		= counter();
	VCP_FLIR_GAIN_KNOB_EXT		= counter();
	VCP_FLIR_LEV_KNOB_EXT		= counter();
	VCP_ACM_SW_EXT				= counter();
	VCP_IHADSS_CON_KNOB_EXT		= counter();
	VCP_IHADSS_BRT_KNOB_EXT		= counter();
	VCP_SYM_BRT_KNOB_EXT		= counter();

	GND_ORIDE_BTN_EXT			= counter();
	ARM_SAFE_BTN_EXT			= counter();
}

reset_counter()
CMWS_commands =
{
	-- CMWS panel-----------------------------------------------------------
	CMWS_PWR					= counter();
	CMWS_PWR_TEST				= counter();
	CMWS_AUDIO_KNOB				= counter();
	CMWS_LAMP_KNOB				= counter();
	CMWS_ARM_SAFE_SW			= counter();
	CMWS_CMWS_NAV_SW			= counter();
	CMWS_BYPASS_AUTO_SW			= counter();
	CMWS_JETT_COVER				= counter();
	CMWS_JETT_SW				= counter();

	-- input commands
	CMWS_PWR_ITER				= counter();
	CMWS_PWR_EXT				= counter();

	CMWS_AUDIO_KNOB_AXIS		= counter();
	CMWS_AUDIO_KNOB_ITER		= counter();

	CMWS_LAMP_KNOB_AXIS			= counter();
	CMWS_LAMP_KNOB_ITER			= counter();

	CMWS_ARM_SAFE_SW_EXT		= counter();
	CMWS_ARM_SAFE_SW_ITER		= counter();

	CMWS_CMWS_NAV_SW_EXT		= counter();
	CMWS_CMWS_NAV_SW_ITER		= counter();

	CMWS_BYPASS_AUTO_SW_EXT		= counter();
	CMWS_BYPASS_AUTO_SW_ITER	= counter();

	CMWS_JETT_COVER_EXT			= counter();
	CMWS_JETT_COVER_ITER		= counter();
	CMWS_JETT_SW_EXT			= counter();
}

reset_counter()
hydraulic_commands =
{
	Rotor_Brake			= counter();
	Emergency_HYD_PLT	= counter();
	TailWheelUnLock_PLT	= counter();

	Emergency_HYD_CPG	= counter();
	TailWheelUnLock_CPG	= counter();

	-- input commands
	Rotor_Brake_Sw_EXT	= counter();
	Rotor_Brake_Sw_ITER	= counter();
	Emergency_HYD_EXT	= counter();
	TailWheelUnLock_EXT	= counter();
}

reset_counter()
gear_commands =
{
	AH64_ParkingBrake			= counter();

	-- input commands
	AH64_ParkingBrake_EXT		= counter();
}

reset_counter()
ctrl_commands =
{
	FrictionLever		= counter();

	FingerLiftLeft_PLT	= counter();
	FingerLiftRight_PLT	= counter();
	FingerLiftBoth_PLT	= counter();
	FingerLiftLeft_CPG	= counter();
	FingerLiftRight_CPG	= counter();
	FingerLiftBoth_CPG	= counter();

	-- input commands
	FrictionLever_EXT	= counter();
	FrictionLever_ITER	= counter();
	FrictionLever_AXIS	= counter();

	FingerLiftLeft_EXT	= counter();
	FingerLiftRight_EXT	= counter();
	FingerLiftBoth_EXT	= counter();

	LockoutDetent		= counter();
}

reset_counter()
engine_commands =
{
	APU_StartBtn				= counter();
	APU_StartBtnCover			= counter();

	Eng1StartSw					= counter();
	Eng1IgnOrideSw				= counter();
	Eng2StartSw					= counter();
	Eng2IgnOrideSw				= counter();

	ChkOvspTestSwENG1A			= counter();
	ChkOvspTestSwENG2A			= counter();
	ChkOvspTestSwENG1B			= counter();
	ChkOvspTestSwENG2B			= counter();

	PLT_Eng1FireBtn				= counter();
	PLT_Eng1FireBtnCover		= counter();
	PLT_Eng2FireBtn				= counter();
	PLT_Eng2FireBtnCover		= counter();
	PLT_ApuFireBtn				= counter();
	PLT_ApuFireBtnCover			= counter();
	PLT_PrimaryDischBtn			= counter();
	PLT_ReserveDischBtn			= counter();
	PLT_FireDetTestSw1			= counter();
	PLT_FireDetTestSw2			= counter();

	CPG_Eng1FireBtn				= counter();
	CPG_Eng1FireBtnCover		= counter();
	CPG_Eng2FireBtn				= counter();
	CPG_Eng2FireBtnCover		= counter();
	CPG_ApuFireBtn				= counter();
	CPG_ApuFireBtnCover			= counter();
	CPG_PrimaryDischBtn			= counter();
	CPG_ReserveDischBtn			= counter();
	CPG_FireDetTestSw1			= counter();
	CPG_FireDetTestSw2			= counter();

	PLT_L_PowerLever			= counter();
	PLT_R_PowerLever			= counter();
	CPG_L_PowerLever			= counter();
	CPG_R_PowerLever			= counter();

	-- input commands
	APU_StartBtn_EXT			= counter();
	APU_StartBtnCover_EXT		= counter();
	APU_StartBtnCover_ITER		= counter();

	Eng1StartSw_EXT				= counter();
	Eng1IgnOrideSw_EXT			= counter();
	Eng2StartSw_EXT				= counter();
	Eng2IgnOrideSw_EXT			= counter();

	ChkOvspTestSwENG1A_EXT		= counter();
	ChkOvspTestSwENG2A_EXT		= counter();
	ChkOvspTestSwENG1B_EXT		= counter();
	ChkOvspTestSwENG2B_EXT		= counter();

	Eng1FireBtn_EXT				= counter();
	Eng1FireBtnCover_EXT		= counter();
	Eng1FireBtnCover_ITER		= counter();
	Eng2FireBtn_EXT				= counter();
	Eng2FireBtnCover_EXT		= counter();
	Eng2FireBtnCover_ITER		= counter();
	ApuFireBtn_EXT				= counter();
	ApuFireBtnCover_EXT			= counter();
	ApuFireBtnCover_ITER		= counter();
	PrimaryDischBtn_EXT			= counter();
	ReserveDischBtn_EXT			= counter();
	FireDetTestSw1_EXT			= counter();
	FireDetTestSw2_EXT			= counter();

	PLT_BothPowerLevers_EXT		= counter();
	CPG_BothPowerLevers_EXT		= counter();
}

reset_counter()
extlights_commands =
{
	FormationLights			= counter();
	NavLights				= counter();
	AntiCollLights			= counter();
	FormationLights_ITER	= counter();
	FormationLights_AXIS	= counter();
	NavLights_EXT			= counter();
	NavLights_ITER			= counter();
	AntiCollLights_EXT		= counter();
	AntiCollLights_ITER		= counter();
}

reset_counter()
intlights_commands =
{
	MasterCautionPLT			= counter();
	MasterWarningPLT			= counter();
	MasterCautionCPG			= counter();
	MasterWarningCPG			= counter();
	TestLightsPLT				= counter();
	SignalPLT					= counter();
	PrimaryPLT					= counter();
	FloodPLT					= counter();
	StbyInstPLT					= counter();
	UtilityPLT					= counter();
	UtilityButtonPLT			= counter();
	TestLightsCPG				= counter();
	SignalCPG					= counter();
	PrimaryCPG					= counter();
	FloodCPG					= counter();
	UtilityCPG					= counter();
	UtilityButtonCPG			= counter();
	MasterCaution_EXT			= counter();
	MasterWarning_EXT			= counter();
	TestLights_EXT				= counter();
	UtilityButton_EXT			= counter();
	Signal_ITER					= counter();
	Primary_ITER				= counter();
	Flood_ITER					= counter();
	StbyInst_ITER				= counter();
	Utility_ITER				= counter();
	Signal_AXIS					= counter();
	Primary_AXIS				= counter();
	Flood_AXIS					= counter();
	StbyInst_AXIS				= counter();
	Utility_AXIS				= counter();
}

reset_counter()
sai_commands =
{
	CageKnobPull				= counter();
	CageKnobRotate				= counter();
	CageKnobPull_EXT			= counter();
	CageKnobRotate_ITER			= counter();
	CageKnobRotate_AXIS			= counter();
}

reset_counter()
baro_alt_commands =
{
	PressureSet					= counter();
	PressureSet_ITER			= counter();
	PressureSet_AXIS			= counter();
}

reset_counter()
mpd_commands =
{
	T1				= counter();
	T2				= counter();
	T3				= counter();
	T4				= counter();
	T5				= counter();
	T6				= counter();
	R1				= counter();
	R2				= counter();
	R3				= counter();
	R4				= counter();
	R5				= counter();
	R6				= counter();
	B6				= counter();
	B5				= counter();
	B4				= counter();
	B3				= counter();
	B2				= counter();
	B1				= counter();
	L6				= counter();
	L5				= counter();
	L4				= counter();
	L3				= counter();
	L2				= counter();
	L1				= counter();
	Asterisk		= counter();
	VID				= counter();
	COM				= counter();
	AC				= counter();
	TSD				= counter();
	WPN				= counter();
	FCR				= counter();
	BRT_KNOB		= counter();
	VID_KNOB		= counter();
	MODE_KNOB		= counter();
	-- input commands
	BRT_KNOB_ITER	= counter();
	BRT_KNOB_AXIS	= counter();
	VID_KNOB_ITER	= counter();
	VID_KNOB_AXIS	= counter();
	MODE_KNOB_ITER	= counter();
}

reset_counter()
KU_commands =
{
	keyCLR		= counter();
	keyBKS		= counter();
	keySPC		= counter();
	keyLeft		= counter();
	keyRight	= counter();
	keyEnter	= counter();
	keyA		= counter();	keyB	= counter();	keyC	= counter();
	keyD		= counter();	keyE	= counter();	keyF	= counter();
	keyG		= counter();	keyH	= counter();	keyI	= counter();
	keyJ		= counter();	keyK	= counter();	keyL	= counter();
	keyM		= counter();	keyN	= counter();	keyO	= counter();
	keyP		= counter();	keyQ	= counter();	keyR	= counter();
	keyS		= counter();	keyT	= counter();	keyU	= counter();
	keyV		= counter();	keyW	= counter();	keyX	= counter();
	keyY		= counter();	keyZ	= counter();
	key1		= counter();	key2	= counter();	key3	= counter();
	key4		= counter();	key5	= counter();	key6	= counter();
	key7		= counter();	key8	= counter();	key9	= counter();
	keyDot		= counter();	key0	= counter();	keySign	= counter();
	keySlash	= counter();
	keyPlus		= counter();
	keyMinus	= counter();
	keyDivide	= counter();
	keyMultiply	= counter();

	BrightnessKnob		= counter();
	BrightnessKnob_KB	= counter();
	BrightnessKnob_AXIS	= counter();
}

reset_counter()
eufd_commands =
{
	WCA_UP		= counter();
	WCA_DOWN	= counter();
	IDM_UP		= counter();
	IDM_DOWN	= counter();
	RTS_UP		= counter();
	RTS_DOWN	= counter();
	Preset		= counter();
	Enter		= counter();
	Stopwatch	= counter();
	Swap		= counter();
	BRT			= counter();
	-- input commands
	BRT_ITER	= counter();
	BRT_AXIS	= counter();
}

reset_counter()
hotas_commands =
{
	-- Cyclic Stick Grip
	CYCLIC_TRIGGER_GUARD					= counter();
	CYCLIC_TRIGGER_1ST_DETENT				= counter();
	CYCLIC_TRIGGER_2ND_DETENT				= counter();

	CYCLIC_TRIM_HOLD_SW_UP					= counter();	-- (R) FORCE TRIM RELEASE
	CYCLIC_TRIM_HOLD_SW_DOWN				= counter();	-- (D) HOLD MODES DISENGAGE
	CYCLIC_TRIM_HOLD_SW_LEFT				= counter();	-- (AT) ATTITUDE/HOVER HOLD
	CYCLIC_TRIM_HOLD_SW_RIGHT				= counter();	-- (AL) ALTITUDE HOLD

	CYCLIC_WEAPONS_ACTION_SW_UP				= counter();	-- (G) GUN
	CYCLIC_WEAPONS_ACTION_SW_DOWN			= counter();	-- (A) AIR-TO-AIR
	CYCLIC_WEAPONS_ACTION_SW_LEFT			= counter();	-- (R) ROCKET
	CYCLIC_WEAPONS_ACTION_SW_RIGHT			= counter();	-- (M) MISSILE

	CYCLIC_SYMBOLOGY_SELECT_SW_UP			= counter();	-- (T),(C)
	CYCLIC_SYMBOLOGY_SELECT_SW_DOWN			= counter();	-- (B),(H)
	CYCLIC_SYMBOLOGY_SELECT_SW_DEPRESS		= counter();

	CYCLIC_CMDS_SW_FWD						= counter();
	CYCLIC_CMDS_SW_AFT						= counter();

	CYCLIC_RTS_SW_LEFT						= counter();	-- RADIO
	CYCLIC_RTS_SW_RIGHT						= counter();	-- ICS
	CYCLIC_RTS_SW_DEPRESS					= counter();	-- RTS

	CYCLIC_FMC_RELEASE_SW					= counter();
	CYCLIC_CHAFF_DISPENCE_BTN				= counter();
	CYCLIC_FLARE_DISPENCE_BTN				= counter();
	CYCLIC_ATA_CAGE_UNCAGE_BTN				= counter();

	-- Collective Mission Grip
	MISSION_FCR_SCAN_SIZE_SW_UP				= counter();	-- (Z) ZOOM
	MISSION_FCR_SCAN_SIZE_SW_DOWN			= counter();	-- (M) MEDIUM
	MISSION_FCR_SCAN_SIZE_SW_LEFT			= counter();	-- (N) NARROW
	MISSION_FCR_SCAN_SIZE_SW_RIGHT			= counter();	-- (W) WIDE

	MISSION_SIGHT_SELECT_SW_UP				= counter();	-- (HMD)
	MISSION_SIGHT_SELECT_SW_DOWN			= counter();	-- (LINK)
	MISSION_SIGHT_SELECT_SW_LEFT			= counter();	-- (FCR)
	MISSION_SIGHT_SELECT_SW_RIGHT			= counter();	-- (TADS)

	MISSION_FCR_MODE_SW_UP					= counter();	-- (GTM) Ground Targeting Mode
	MISSION_FCR_MODE_SW_DOWN				= counter();	-- (ATM) Air Targeting Mode
	MISSION_FCR_MODE_SW_LEFT				= counter();	-- (TPM) Terrain Profile Mode
	MISSION_FCR_MODE_SW_RIGHT				= counter();	-- (RMAP) Radar MAP

	MISSION_CURSOR_UP						= counter();
	MISSION_CURSOR_DOWN						= counter();
	MISSION_CURSOR_LEFT						= counter();
	MISSION_CURSOR_RIGHT					= counter();
	MISSION_CURSOR_ENTER					= counter();
	MISSION_ALTERNATE_CURSOR_ENTER			= counter();
	MISSION_CURSOR_AXIS_X					= counter();
	MISSION_CURSOR_AXIS_Y					= counter();

	MISSION_CURSOR_DISPLAY_SELECT_BTN		= counter();
	MISSION_FCR_SCAN_SW_SINGLE				= counter();
	MISSION_FCR_SCAN_SW_CONTINUOUS			= counter();
	MISSION_CUED_SEARCH_SW					= counter();
	MISSION_MISSILE_ADVANCE_SW				= counter();

	-- Collective Flight Grip
	FLIGHT_EMERGENCY_JETTISON_SW_GUARD		= counter();
	FLIGHT_EMERGENCY_JETTISON_SW			= counter();

	FLIGHT_NVS_SELECT_SW_TADS				= counter();
	FLIGHT_NVS_SELECT_SW_PNVS				= counter();
	FLIGHT_BORESIGHT_POLARITY_SW_BS			= counter();
	FLIGHT_BORESIGHT_POLARITY_SW_PLRT		= counter();

	FLIGHT_STABILATOR_CONTROL_SW_NU			= counter();	-- NOSE UP
	FLIGHT_STABILATOR_CONTROL_SW_ND			= counter();	-- NOSE DOWN
	FLIGHT_STABILATOR_CONTROL_SW_DEPRESS	= counter();	-- STABILATOR AUTO MODE

	FLIGHT_SEARCHLIGHT_SW_UP				= counter();	-- ON/OFF
	FLIGHT_SEARCHLIGHT_SW_DOWN				= counter();	-- STOW/OFF

	FLIGHT_SEARCHLIGHT_POSITION_SW_UP		= counter();	-- EXT
	FLIGHT_SEARCHLIGHT_POSITION_SW_DOWN		= counter();	-- RET
	FLIGHT_SEARCHLIGHT_POSITION_SW_LEFT		= counter();	-- L
	FLIGHT_SEARCHLIGHT_POSITION_SW_RIGHT	= counter();	-- R

	FLIGHT_CHOP_BTN_GUARD					= counter();
	FLIGHT_CHOP_BTN							= counter();

	FLIGHT_TAIL_WHEEL_BTN					= counter();	-- LOCK/UNLOCK

	FLIGHT_BUCS_TRIGGER_GUARD				= counter();	-- only in CPG
	FLIGHT_BUCS_TRIGGER						= counter();	-- only in CPG

	-- input commands
	CYCLIC_TRIGGER_GUARD_ITER				= counter();
	FLIGHT_EMERGENCY_JETTISON_SW_GUARD_ITER	= counter();
	FLIGHT_SEARCHLIGHT_SW_ITER				= counter();
	FLIGHT_CHOP_BTN_GUARD_ITER				= counter();
	FLIGHT_BUCS_TRIGGER_GUARD_ITER			= counter();
}

reset_counter()
tedac_commands =
{
	-- Display Unit -----------------------------------
	TDU_MODE_KNOB					= counter();
	TDU_GAIN_KNOB					= counter();
	TDU_LEV_KNOB					= counter();
	TDU_ASTERISK_BTN				= counter();
	-- Video Select Buttons
	TDU_VIDEO_SELECT_TAD_BTN		= counter();
	TDU_VIDEO_SELECT_FCR_BTN		= counter();
	TDU_VIDEO_SELECT_PNV_BTN		= counter();
	TDU_VIDEO_SELECT_GS_BTN			= counter();
	-- Bezel Controls
	TDU_SYM_INC						= counter();
	TDU_SYM_DEC						= counter();
	TDU_BRT_INC						= counter();
	TDU_BRT_DEC						= counter();
	TDU_CON_INC						= counter();
	TDU_CON_DEC						= counter();
	TDU_AZ_LEFT						= counter();
	TDU_AZ_RIGHT					= counter();
	TDU_EL_UP						= counter();
	TDU_EL_DOWN						= counter();
	TDU_RF_UP						= counter();
	TDU_RF_DOWN						= counter();
	-- Bottom Buttons
	TDU_B1							= counter();	-- AZ/EL
	TDU_B2							= counter();	-- ACM
	TDU_B3							= counter();	-- FREEZE
	TDU_B4							= counter();	-- FILTER

	-- input commands
	TDU_MODE_KNOB_ITER				= counter();
	TDU_GAIN_KNOB_ITER				= counter();
	TDU_GAIN_KNOB_AXIS				= counter();
	TDU_LEV_KNOB_ITER				= counter();
	TDU_LEV_KNOB_AXIS				= counter();

	-- Left Hand Grip ---------------------------------
	LHG_IAT_OFS_SW_IAT				= counter();	-- Image Auto Track / Offset
	LHG_IAT_OFS_SW_OFS				= counter();	-- Image Auto Track / Offset

	LHG_TADS_FOV_SW_Z				= counter();	-- Zoom
	LHG_TADS_FOV_SW_M				= counter();	-- Medium
	LHG_TADS_FOV_SW_N				= counter();	-- Narrow
	LHG_TADS_FOV_SW_W				= counter();	-- Wide

	LHG_TADS_SENSOR_SELECT_SW_FLIR	= counter();
	LHG_TADS_SENSOR_SELECT_SW_TV	= counter();
	LHG_TADS_SENSOR_SELECT_SW_DVO	= counter();	-- ORT only

	LHG_STORE_UPDT_SW_STORE			= counter();
	LHG_STORE_UPDT_SW_UPDT			= counter();

	LHG_FCR_SCAN_SW_S				= counter();	-- single (as in collective mission grip)
	LHG_FCR_SCAN_SW_C				= counter();	-- continuous (as in collective mission grip)

	LHG_CUED_SEARCH_BTN				= counter();	-- (as in collective mission grip)
	LHG_LMC_BTN						= counter();	-- Linear Motion Compensation

	LHG_FCR_MODE_SW_UP				= counter();	-- (GTM) Ground Targeting Mode		(as in collective mission grip)
	LHG_FCR_MODE_SW_DOWN			= counter();	-- (ATM) Air Targeting Mode			(as in collective mission grip)
	LHG_FCR_MODE_SW_LEFT			= counter();	-- (TPM) Terrain Profile Mode		(as in collective mission grip)
	LHG_FCR_MODE_SW_RIGHT			= counter();	-- (RMAP) Radar MAP					(as in collective mission grip)

	LHG_WEAPONS_ACTION_SW_UP		= counter();	-- (G) GUN				(as in collective mission grip)
	LHG_WEAPONS_ACTION_SW_DOWN		= counter();	-- (A) AIR-TO-AIR		(as in collective mission grip)
	LHG_WEAPONS_ACTION_SW_LEFT		= counter();	-- (R) ROCKET			(as in collective mission grip)
	LHG_WEAPONS_ACTION_SW_RIGHT		= counter();	-- (M) MISSILE			(as in collective mission grip)

	LHG_CURSOR_UP					= counter();
	LHG_CURSOR_DOWN					= counter();
	LHG_CURSOR_LEFT					= counter();
	LHG_CURSOR_RIGHT				= counter();
	LHG_CURSOR_ENTER				= counter();
	LHG_CURSOR_AXIS_X				= counter();
	LHG_CURSOR_AXIS_Y				= counter();

	LHG_LR_BTN						= counter();	-- (L/R Switch) Cursor Display Select Btn
	LHG_WEAPON_TRIGGER_1ST_DETENT	= counter();
	LHG_WEAPON_TRIGGER_2ND_DETENT	= counter();

	-- Right Hand Grip --------------------------------
	RHG_SIGHT_SELECT_SW_UP			= counter();	-- (HMD)		(as in collective mission grip)
	RHG_SIGHT_SELECT_SW_DOWN		= counter();	-- (LINK)		(as in collective mission grip)
	RHG_SIGHT_SELECT_SW_LEFT		= counter();	-- (FCR)		(as in collective mission grip)
	RHG_SIGHT_SELECT_SW_RIGHT		= counter();	-- (TADS)		(as in collective mission grip)

	RHG_LT_SW_A						= counter();	-- Automatic	Laser Tracker Switch
	RHG_LT_SW_O						= counter();	-- Off
	RHG_LT_SW_M						= counter();	-- Manual

	RHG_FCR_SCAN_SIZE_SW_UP			= counter();	-- (Z) ZOOM
	RHG_FCR_SCAN_SIZE_SW_DOWN		= counter();	-- (M) MEDIUM
	RHG_FCR_SCAN_SIZE_SW_LEFT		= counter();	-- (N) NARROW
	RHG_FCR_SCAN_SIZE_SW_RIGHT		= counter();	-- (W) WIDE

	RHG_C_SCOPE_SW					= counter();
	RHG_FLIR_PLRT_BTN				= counter();
	RHG_SIGHT_SLAVE_BTN				= counter();
	RHG_DISPLAY_ZOOM_BTN			= counter();
	RHG_LRFD_TRIGGER				= counter();

	RHG_SPARE_SW_FWD				= counter();	-- MTADS only, MTT Switch (Multi Target Tracker, 3-pos, rocker)
	RHG_SPARE_SW_AFT				= counter();	-- MTADS only, MTT Switch (Multi Target Tracker, 3-pos, rocker)

	RHG_HDD_SW						= counter();	-- HDD/HOD
	RHG_ENTER_BTN					= counter();

	RHG_MAN_TRK_UP					= counter();	-- Sensor (Sight) Manual Tracker
	RHG_MAN_TRK_DOWN				= counter();
	RHG_MAN_TRK_LEFT				= counter();
	RHG_MAN_TRK_RIGHT				= counter();
	RHG_MAN_TRK_AXIS_X				= counter();
	RHG_MAN_TRK_AXIS_Y				= counter();

	RHG_IAT_POLARITY_W				= counter();	-- White
	RHG_IAT_POLARITY_A				= counter();	-- Auto
	RHG_IAT_POLARITY_B				= counter();	-- Black

	-- additional commands
	LHG_TADS_SENSOR_SELECT_SW		= counter();	-- for clickability
	RHG_LT_SW						= counter();	-- for clickability
	RHG_IAT_POLARITY_SW				= counter();	-- for clickability

	RHG_MAN_TRK_DOWN_LEFT			= counter();	-- for input
	RHG_MAN_TRK_DOWN_RIGHT			= counter();	-- for input
	RHG_MAN_TRK_UP_LEFT				= counter();	-- for input
	RHG_MAN_TRK_UP_RIGHT			= counter();	-- for input
}

reset_counter()
hmd_commands =
{
	-- Display Adjust Panels
	DAP_PLT_SIZE_KNOB			= counter();
	DAP_PLT_H_CTRG_KNOB			= counter();
	DAP_PLT_V_CTRG_KNOB			= counter();

	DAP_CPG_SIZE_KNOB			= counter();
	DAP_CPG_H_CTRG_KNOB			= counter();
	DAP_CPG_V_CTRG_KNOB			= counter();

	-- input commands
	DAP_SIZE_ITER				= counter();
	DAP_H_CTRG_ITER				= counter();
	DAP_V_CTRG_ITER				= counter();

	HMD_SHOW					= counter();
}

reset_counter()
BRU_commands =
{
	BRT_KNOB			= counter();
	BRT_KNOB_ITER		= counter();
	BRT_KNOB_AXIS		= counter();
}

reset_counter()
JETT_commands =
{
	STORE_LO_JETTISON_ARMED			= counter();
	STORE_LI_JETTISON_ARMED			= counter();
	STORE_RI_JETTISON_ARMED			= counter();
	STORE_RO_JETTISON_ARMED			= counter();

	STORE_JETTISON_LEFT_WINGTIP		= counter();
	STORE_JETTISON_RIGHT_WINGTIP	= counter();

	STORES_JETT_PUSHBUTTON			= counter();
}

reset_counter()
preston_commands =
{
	ControlRequest					= counter();
	ShowHideMenu					= counter();
	HatUp							= counter();
	HatDown							= counter();
	HatLeft							= counter();
	HatRight						= counter();
	StickFolding					= counter();
	MfInput							= counter();
	StoreTarget						= counter();
}

reset_counter()
comm_commands =
{
	VHF_volume			= counter();
	UHF_volume			= counter();
	FM1_volume			= counter();
	FM2_volume			= counter();
	HF_volume			= counter();

	IFF_volume			= counter();
	RLWR_volume			= counter();
	ATA_volume			= counter();
	VCR_volume			= counter();
	ADF_volume			= counter();

	MASTER_volume		= counter();
	SensControl			= counter();

	VHF_SQL_ON			= counter();
	UHF_SQL_ON			= counter();
	FM1_SQL_ON			= counter();
	FM2_SQL_ON			= counter();
	HF_SQL_ON			= counter();

	ICS_MODE			= counter();
	IDENT				= counter();

	VHF_disable			= counter();
	UHF_disable			= counter();
	FM1_disable			= counter();
	FM2_disable			= counter();
	HF_disable			= counter();

	IFF_disable			= counter();
	RLWR_disable		= counter();
	ATA_disable			= counter();
	VCR_disable			= counter();
	ADF_disable			= counter();

	-- input commands
	VHF_volume_ITER		= counter();
	VHF_volume_AXIS		= counter();
	UHF_volume_ITER		= counter();
	UHF_volume_AXIS		= counter();
	FM1_volume_ITER		= counter();
	FM1_volume_AXIS		= counter();
	FM2_volume_ITER		= counter();
	FM2_volume_AXIS		= counter();
	HF_volume_ITER		= counter();
	HF_volume_AXIS		= counter();

	IFF_volume_ITER		= counter();
	IFF_volume_AXIS		= counter();
	RLWR_volume_ITER	= counter();
	RLWR_volume_AXIS	= counter();
	ATA_volume_ITER		= counter();
	ATA_volume_AXIS		= counter();
	VCR_volume_ITER		= counter();
	VCR_volume_AXIS		= counter();
	ADF_volume_ITER		= counter();
	ADF_volume_AXIS		= counter();

	MASTER_volume_ITER	= counter();
	MASTER_volume_AXIS	= counter();
	SensControl_ITER	= counter();
	SensControl_AXIS	= counter();

	VHF_SQL_ITER		= counter();
	UHF_SQL_ITER		= counter();
	FM1_SQL_ITER		= counter();
	FM2_SQL_ITER		= counter();
	HF_SQL_ITER			= counter();

	ICS_MODE_ITER		= counter();

	VHF_disable_ITER	= counter();
	UHF_disable_ITER	= counter();
	FM1_disable_ITER	= counter();
	FM2_disable_ITER	= counter();
	HF_disable_ITER		= counter();

	IFF_disable_ITER	= counter();
	RLWR_disable_ITER	= counter();
	ATA_disable_ITER	= counter();
	VCR_disable_ITER	= counter();
	ADF_disable_ITER	= counter();

	VHF_SQL_OFF			= counter();
	UHF_SQL_OFF			= counter();
	FM1_SQL_OFF			= counter();
	FM2_SQL_OFF			= counter();
	HF_SQL_OFF			= counter();

	VHF_SQL_ON_EXT		= counter();
	UHF_SQL_ON_EXT		= counter();
	FM1_SQL_ON_EXT		= counter();
	FM2_SQL_ON_EXT		= counter();
	HF_SQL_ON_EXT		= counter();
	VHF_SQL_OFF_EXT		= counter();
	UHF_SQL_OFF_EXT		= counter();
	FM1_SQL_OFF_EXT		= counter();
	FM2_SQL_OFF_EXT		= counter();
	HF_SQL_OFF_EXT		= counter();
}

reset_counter()
intercom_commands =
{
	PLT_UHF_GUARD_Btn			= counter();
	CPG_UHF_GUARD_Btn			= counter();

	PLT_XPNDR_Btn				= counter();
	CPG_XPNDR_Btn				= counter();

	PLT_ZEROIZE_Sw				= counter();
	CPG_ZEROIZE_Sw				= counter();

	PLT_MasterZeroizeSw			= counter();
	PLT_MasterZeroizeSwCover	= counter();
	CPG_MasterZeroizeSw			= counter();
	CPG_MasterZeroizeSwCover	= counter();

	-- input commands
	UHF_GUARD_Btn_EXT			= counter();
	XPNDR_Btn_EXT				= counter();
	ZEROIZE_Sw_EXT				= counter();
	ZEROIZE_Sw_ITER				= counter();

	MasterZeroizeSw_EXT			= counter();
	MasterZeroizeSw_ITER		= counter();
	MasterZeroizeSwCover_EXT	= counter();
	MasterZeroizeSwCover_ITER	= counter();
}

reset_counter()
cpt_mech_commands =
{
	PLT_DefogBtn					= counter();
	PLT_WiperSw						= counter();
	CPG_DefogBtn					= counter();
	CPG_WiperSw						= counter();
	PLT_Door_Lock					= counter();
	CPG_Door_Lock					= counter();
	PLT_M4_Safety					= counter();
	CPG_M4_Safety					= counter();
	PLT_M4_Trigger					= counter();
	CPG_M4_Trigger					= counter();
	-- input commands
	DefogBtn_EXT					= counter();
	WiperSw_EXT						= counter();
	WiperSw_ITER					= counter();
	PLT_Door_Lock_EXT				= counter();
	CPG_Door_Lock_EXT				= counter();
	MECH_Door_EXT					= counter();
	-- additional
	PLT_SunvisorLeft				= counter();
	PLT_SunvisorRight				= counter();
	PLT_SunvisorLeft_ITER			= counter();
	PLT_SunvisorRight_ITER			= counter();
	PLT_SunvisorLeft_AXIS			= counter();
	PLT_SunvisorRight_AXIS			= counter();
	PLT_WiperSw_PARK				= counter();
	CPG_WiperSw_PARK				= counter();

	OpenCloseServiceHatches			= counter();
}

reset_counter()
head_wrapper_commands =
{
	LockCrewBody	= counter();
}

reset_counter()
DTU_commands =
{
	KB_FLARE_IterBurstCount		= counter();
	KB_FLARE_IterBurstInterval	= counter();
	KB_FLARE_IterSalvoCount		= counter();
	KB_FLARE_IterSalvoInterval	= counter();
	KB_FLARE_IterDelayTime		= counter();
	OpenDTCInterface			= counter();
}