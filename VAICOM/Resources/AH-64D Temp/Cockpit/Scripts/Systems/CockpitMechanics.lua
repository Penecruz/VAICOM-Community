
need_to_be_closed = true -- lua_state  will be closed in post_initialize()

PLTcanopy = { door_ext_animation_arg = 38 }
CPGcanopy = { door_ext_animation_arg = 421 }

wiper = {valmin = 0, valmax = 1, dvalue = 1.5}

ElecConsumerWiperParams =
{
	20.0, 			-- load DC current wiper actuator, [A]
	true,
	10.0,			-- DC voltage value after which the wiper actuator is off, [V]
	15.0, 			-- DC voltage value after which the wiper actuator is on, [V]
	27.0			-- nominal DC voltage value wiper actuator, [V]
}

wiper_sounds_settings =
{
	pitch = {1.0, 3.0, 2.0, 1.0, 1.0}
}

local ratio_gauge_liner = 65;
local WiperSpeedFactor = 2.0 / 60.0;								-- actuator wiper speed , [step/sec]
WiperSpeed0 = WiperSpeedFactor * 60.0 * ratio_gauge_liner;			-- the number of double steps of the electric cleaner drive during start-up (corresponds to the "START" mode) , [step/min]
WiperSpeed1 = WiperSpeedFactor * 90.0 * ratio_gauge_liner;			-- number of double steps of the actuator wiper  (mode 1SPEED), [step/min]
WiperSpeed2 = WiperSpeedFactor * 60.0 * ratio_gauge_liner;			-- number of double steps of the actuator wiper (mode 2SPEED), [step/min]


Vibrations =
{
	--		frequency [rad/sec], amplitude [0..1], phase [rad]
	[1] = { frequency = 500.0, amplitude = 1.0, phase = 0.0 }, --820
	[2] = { frequency = 270.0, amplitude = 1.0, phase = 0.0 }, --821
	[3] = { frequency = 12.1, amplitude = 1.0, phase = 0.0 }, --822
	[4] = { frequency = 8.1, amplitude = 1.0, phase = math.pi }, --823
	[5] = { frequency = 5.4, amplitude = 1.0, phase = 0.0 }, --824
}
