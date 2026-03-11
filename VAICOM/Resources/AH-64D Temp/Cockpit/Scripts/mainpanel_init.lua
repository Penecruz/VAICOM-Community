dofile(LockOn_Options.script_path.."config.lua")
dofile(LockOn_Options.script_path.."VR_config.lua")
dofile(LockOn_Options.script_path.."/../../Views.lua")

need_to_be_closed	= true -- close lua state after initialization

shape_name			= "Cockpit_AH-64D"
draw_pilot			= true
render_debug_info	= false

args_initial_state = {}

local default_view		= SnapViews[1][1]
local eye_pos			= ViewSettings.Cockpit[1].EyePoint[1] or 0;
local default_v_angle	= math.rad(default_view.vAngle)
local default_x			= eye_pos * math.cos(default_v_angle) + default_view.x_trans
local default_y			= eye_pos * math.sin(default_v_angle) + default_view.y_trans
local default_z			= default_view.z_trans

desired_fight_adjustment =
{
	x		=  0.152   - default_x,
	y		= -0.001   - default_y,
	z		=  0.0295  - default_z,
	v_angle	= -4.0 - default_v_angle
}

controllers = LoRegisterPanelControls()
---------------------------------------------------------------
-- CONTROLLERS
---------------------------------------------------------------
local function rad_(value)
	return math.rad(value)
end

local function CreateGaugeLocal(arg, input, output, controller, params)
	local gauge			= CreateGauge()
	gauge.arg_number	= arg
	gauge.input			= input
	gauge.output		= output
	gauge.controller	= controller

	if params ~= nil then
		gauge.params = params
	end

	return gauge
end

local function CreateSimpleGauge(arg, controller, params)
	local gauge = CreateGaugeLocal(arg, {-1.0, 1.0}, {-1.0, 1.0}, controller, params)
	return gauge
end

local function CreateConnectedGauge(external_arg, arg, input, output)
	local gauge			= CreateGauge("external_arg")
	gauge.external_arg	= external_arg
	gauge.arg_number	= arg
	gauge.input			= input
	gauge.output		= output
	return gauge
end

local function CreateSimpleConnectedGauge(external_arg, arg)
	local gauge = CreateConnectedGauge(external_arg, arg, {-1.0, 1.0}, {-1.0, 1.0})
	return gauge
end


-- Draw Crewmembers
PLT_ShowBody				= CreateSimpleGauge(956, controllers.ShowCrewBody, {0})
CPG_ShowBody				= CreateSimpleGauge(957, controllers.ShowCrewBody, {1})

-- PLT animations
PLT_HeadTurnAz				= CreateSimpleConnectedGauge(39, 890)
PLT_HeadTurnEl				= CreateSimpleConnectedGauge(99, 891)

CPG_HeadTurnAz				= CreateSimpleConnectedGauge(337, 892)
CPG_HeadTurnEl				= CreateSimpleConnectedGauge(399, 893)

PLT_Wounded					= CreateSimpleConnectedGauge(459, 894)
CPG_Wounded					= CreateSimpleConnectedGauge(460, 895)

PLT_ControlStickPitch		= CreateSimpleConnectedGauge(500, 900)
PLT_ControlStickRoll		= CreateSimpleConnectedGauge(501, 901)
PLT_Throttle				= CreateSimpleConnectedGauge(502, 902)
PLT_Rudder					= CreateSimpleConnectedGauge(503, 903)

PLT_HelmetNVG				= CreateSimpleConnectedGauge(506, 906)
PLT_HelmetHMD				= CreateSimpleConnectedGauge(507, 907)
PLT_HelmetVisor				= CreateSimpleConnectedGauge(508, 908)
PLT_HelmetShowNVG			= CreateSimpleConnectedGauge(509, 909)
PLT_MaskVisibility			= CreateSimpleConnectedGauge(552, 952)

CPG_ControlStickPitch		= CreateSimpleConnectedGauge(510, 910)
CPG_ControlStickRoll		= CreateSimpleConnectedGauge(511, 911)
CPG_Throttle				= CreateSimpleConnectedGauge(512, 912)
CPG_Rudder					= CreateSimpleConnectedGauge(513, 913)

CPG_HelmetNVG				= CreateSimpleConnectedGauge(516, 916)
CPG_HelmetHMD				= CreateSimpleConnectedGauge(517, 917)
CPG_HelmetVisor				= CreateSimpleConnectedGauge(518, 918)
CPG_HelmetShowNVG			= CreateSimpleConnectedGauge(519, 919)
CPG_MaskVisibility			= CreateSimpleConnectedGauge(553, 953)

PLT_Breath					= CreateSimpleConnectedGauge(523, 920)
PLT_Shaking					= CreateSimpleConnectedGauge(524, 921)
--PLT_HelmetCorrection		= CreateSimpleConnectedGauge(529, )

CPG_Breath					= CreateSimpleConnectedGauge(540, 940)
CPG_Shaking					= CreateSimpleConnectedGauge(541, 941)
CPG_HelmetCorrection		= CreateSimpleConnectedGauge(549, 949)

CPG_MoveInTheSeat			= CreateSimpleConnectedGauge(542, 942)
CPG_LookAround				= CreateSimpleConnectedGauge(543, 943)
CPG_TakeHoldOfTheHandRails	= CreateSimpleConnectedGauge(544, 944)

CPG_WorkWithLeftMFD			= CreateSimpleConnectedGauge(546, 946)
CPG_WorkWithRightMFD		= CreateSimpleConnectedGauge(547, 947)
CPG_WorkWithSidePanel		= CreateSimpleConnectedGauge(548, 948)

PLT_TakeControls			= CreateSimpleConnectedGauge(550, 950)
CPG_TakeControls			= CreateSimpleConnectedGauge(551, 951)


-- CANOPY -----------------------------------------------------
--Canopy				= CreateGauge("external_arg")
--Canopy.external_arg	= 38
--Canopy.arg_number	= 181
--Canopy.input		= {0,1}
--Canopy.output		= {0,1}

--[[CanopyDamages 				= CreateGauge("external_arg")
CanopyDamages.external_arg	= 65
CanopyDamages.arg_number	= 905
CanopyDamages.input			= {0,1}
CanopyDamages.output		= {0,1}--]]


canopy  			= CreateGauge()
canopy.arg_number	= 798
canopy.input		= {0,1}
canopy.output		= {0,1}
canopy.controller	= controllers.DoorCPG

pilotDoor  			= CreateGauge()
pilotDoor.arg_number= 795
pilotDoor.input		= {0,1}
pilotDoor.output	= {0,1}
pilotDoor.controller= controllers.DoorPLT

-- CONTROLS ---------------------------------------------------
StickPitchPLT			= CreateSimpleGauge(470, controllers.StickPitchPLT)
StickRollPLT			= CreateSimpleGauge(471, controllers.StickRollPLT)
CollectivePLT			= CreateSimpleGauge(474, controllers.CollectivePLT)
LeftThrottlePLT			= CreateSimpleGauge(398, controllers.LeftThrottlePLT)
RightThrottlePLT		= CreateSimpleGauge(399, controllers.RightThrottlePLT)
RudderPLT				= CreateSimpleGauge(476, controllers.RudderPLT)
LeftWheelBrakePLT		= CreateSimpleGauge(480, controllers.LeftWheelBrakePLT)
RightWheelBrakePLT		= CreateSimpleGauge(481, controllers.RightWheelBrakePLT)

StickPitchCPG			= CreateSimpleGauge(472, controllers.StickPitchCPG)
StickRollCPG			= CreateSimpleGauge(473, controllers.StickRollCPG)
CollectiveCPG			= CreateSimpleGauge(475, controllers.CollectiveCPG)
LeftThrottleCPG			= CreateSimpleGauge(628, controllers.LeftThrottleCPG)
RightThrottleCPG		= CreateSimpleGauge(629, controllers.RightThrottleCPG)
RudderCPG				= CreateSimpleGauge(478, controllers.RudderCPG)
LeftWheelBrakeCPG		= CreateSimpleGauge(482, controllers.LeftWheelBrakeCPG)
RightWheelBrakeCPG		= CreateSimpleGauge(483, controllers.RightWheelBrakeCPG)

-- Cockpit mechanics
CanopyHandle			= CreateSimpleGauge(712, controllers.CanopyHandle)


-- WEAPONS ----------------------------------------------------

-- INSTRUMENTS ------------------------------------------------

--Standby Attitude Indicator
saiPitch						= CreateGaugeLocal(622,		{-rad_(85.0),	rad_(85.0)},	{-0.95,	0.95},	controllers.sai,			{0})
saiBank							= CreateGaugeLocal(623,		{-math.pi, math.pi}, 			{1.0, -1.0}, 	controllers.sai,			{1})
saiOffFlag						= CreateSimpleGauge(624,													controllers.sai,			{2})
saiArrowPointer					= CreateGaugeLocal(625,		{0.1, 0.9}, 					{-0.85, 1.0}, 	controllers.sai,			{3})
saiSlipBall						= CreateSimpleGauge(626,													controllers.sai,			{4})
saiTurn							= CreateGaugeLocal(627,		{-math.rad(4.0), math.rad(4.0)},{-1.0, 1.0},	controllers.sai,			{5})

--Standby Airspeed Indicator
IASinput	= {0.0,	20.0,	30.0,	40.0,	50.0,	60.0,	70.0,	80.0,	90.0,	100.0,	110.0,	120.0,	130.0,	140.0,	150.0,	200.0,	230.0,	240.0,	250.0}
IASoutput	= {0.0,	0.018,	0.05,	0.09,	0.142,	0.2,	0.27,	0.345,	0.394,	0.432,	0.465,	0.505,	0.543,	0.581,	0.623,	0.789,	0.896,	0.934,	0.967}
ias								= CreateGaugeLocal(469, 	IASinput,						IASoutput, 		controllers.ias)

--Standby Altimeter
BaroAltimeterAltitude			= CreateGaugeLocal(479,		{0.0, 1000.0},					{0.0, 1.0},		controllers.BaroAltimeter,	{0})
BaroAltimeterAltitude01000		= CreateGaugeLocal(605,		{0.0, 10.0},					{0.0, 1.0},		controllers.BaroAltimeter,	{1})
BaroAltimeterAltitude10000		= CreateGaugeLocal(606,		{0.0, 10.0},					{0.0, 1.0},		controllers.BaroAltimeter,	{2})
BaroAltimeterPressure0001		= CreateGaugeLocal(609,		{0.0, 10.0},					{0.0, 1.0},		controllers.BaroAltimeter,	{3})
BaroAltimeterPressure0010		= CreateGaugeLocal(608,		{0.0, 10.0},					{0.0, 1.0},		controllers.BaroAltimeter,	{4})
BaroAltimeterPressure1100		= CreateGaugeLocal(607,		{26.0, 31.0},					{0.0, 1.0},		controllers.BaroAltimeter,	{5})

--Free Air Temperature Gage
FATinput	= {-70.0,	 -60.0,	 -50.0,	-40.0,	-30.0,	50.0}
FAToutput	= {  0.0,	0.0674,	0.1405,	 0.22,	0.305,	 1.0}
FatGage							= CreateGaugeLocal(636, 	FATinput,						FAToutput, 		controllers.FatGage)

-- vibration
Vibration1			= CreateSimpleGauge(820, controllers.Vibration1)
Vibration2			= CreateSimpleGauge(821, controllers.Vibration2)
Vibration3			= CreateSimpleGauge(822, controllers.Vibration3)
Vibration4			= CreateSimpleGauge(823, controllers.Vibration4)
Vibration5			= CreateSimpleGauge(824, controllers.Vibration5)

-- Lamps
dofile(LockOn_Options.script_path.."MainPanel/lamps.lua")




---------------------------------------------------------------
-- MIRROR
---------------------------------------------------------------
mirrors_data =
{
	center_point		= {1.67,-0.44,-0.41},
	width				= 0.6,
	aspect				= 1.0,
	rotation_z			= 0.092676964,
	rotation_y			= 0.5309304,
	flaps				= { "MIRROR_1" },
	use_z_correction	= false,
}

mirror_fake				= CreateGauge()
mirror_fake.arg_number	= 635
mirror_fake.input		= {0,1}
mirror_fake.output		= {0,1}
mirror_fake.controller	= controllers.mirrors_draw


--
dofile(LockOn_Options.common_script_path.."tools.lua")
livery = find_custom_livery("AH-64D","default")