-- Timer update time
device_timer_dt = 0.04

-- Gyro cage rate
CageRate = math.rad(160.0) -- rad/sec

-- Gyro mass
Mass = 0.14 -- kilograms
-- Moment of inertia along gyro main axis
Jz = 0.00086 -- N * m * sec * sec
-- Moment of inertia along gyro X, Y axes
Jxy = 0.00043 -- N * m * sec * sec
-- Friction coefficients along X, Y axes
KfrX = 0.00005
KfrY = 0.00005

-- Nominal gyro rotation rate
NominalRotRate = 2090.0 -- rad/sec, 20000 revolutions/min
-- Spinup rate
SpinUpT = 36 -- time constant, spinup time - ~3 minutes
-- Slowdown rate
SlowDownT = 100 -- time constant, slow down time - ~9 minutes
-- Threshold spinning velocity, rad/sec
ThresholdRotRate = 0.01

-- Time constant of pitch drum-servo motor feedback ctrl system
T_Pitch = 0.2 -- sec
-- Time constant of tracking frame-servo motor feedback ctrl system
T_Bank = 0.2 -- сек

-- Mass displacement constant scale along X gyro axis
DmassX_const_scale = 0.0002
-- Mass displacement constant scale along Y gyro axis
DmassY_const_scale = DmassX_const_scale
-- Mass displacement constant scale along Z gyro axis
DmassZ_const_scale = DmassX_const_scale

-- Mass displacement random scale along X gyro axis
DmassX_rnd_scale = DmassX_const_scale * 0.3
-- Mass displacement random scale along Y gyro axis
DmassY_rnd_scale = DmassX_rnd_scale
-- Mass displacement random scale along Z gyro axis
DmassZ_rnd_scale = DmassX_rnd_scale

-- Constant drift (rad/sec)
ConstDriftYRate_Scale = math.rad(0.0084)
ConstDriftXRate_Scale = ConstDriftYRate_Scale

-- Constant drift noise
T_Corr = 15.0 -- sec
E = math.rad(0.006) -- rad/sec

----------------------------------------------------
-- Radial correction
--
-- Max deflection of gyro main axis from apparent vertical
MaxDeflection = math.rad(7) -- radians
-- Correction rate with gyro axis deflected on MaxDeflection deg from apparent vertical
CorrectionRate = math.rad(0.4) -- rad/sec with the apex deflection at MaxDeflection degrees (maximum)
-- Minimum correction rate
MinCorrectionRate = CorrectionRate * 0.1 -- rad/sec

pitch = {isLagElement = true, valmin = -math.rad(85.0), valmax = math.rad(85.0), T1 = 0.1, wmax = math.rad(80.0), bias = {{valmin = -math.rad(90.0), valmax = math.rad(90.0), bias = math.rad(0.3)}}}
bank = {isLagElement = true, valmin = 0.0, valmax = math.rad(360.0), T1 = 0.1, bias = {{valmin = 0.0, valmax = math.rad(360.0), bias = math.rad(0.3)}}}
attWarnFlag = {valmin = 0, valmax = 1, dvalue = 11}
slipBall = {dt = 0.015, tubeLen = 0.0582, tubeSector = math.rad(30)}
deviationBars = {isLagElement = true, valmin = -1, valmax = 1, T1 = 0.2} -- to remove

TurnNeedle = {valmin = -math.rad(4.0), valmax = math.rad(4.0), T1 = 0.9, T2 = 0.5, bias = {{valmin = -math.rad(4.0), valmax = math.rad(4.0), bias = 0.001}}}
GyroElectConsumerParams = {20.0, true, 20.2, 21.5, 28.0}

need_to_be_closed = true -- lua_state  will be closed in post_initialize()
