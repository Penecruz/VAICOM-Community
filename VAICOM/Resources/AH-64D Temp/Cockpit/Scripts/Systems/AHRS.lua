-- Timer update time
device_timer_dt = 0.04

-- Gyro cage rate
CageRate = math.rad(160) -- rad/sec

-- Gyro mass
Mass = 0.18 -- kilograms
-- Moment of inertia along gyro main axis
Jz = 0.001 -- N * m * sec * sec
-- Moment of inertia along gyro X, Y axes
Jxy = 0.00043 -- N * m * sec * sec
-- Friction coefficients along X, Y axes
KfrX = 0.00005
KfrY = 0.00005

-- Nominal gyro rotation rate
NominalRotRate = 2090.0 -- rad/sec, 20000 revolutions/min
-- Spinup rate
SpinUpT = 12 -- time constant, spinup time - ~60 sec
-- Slowdown rate
SlowDownT = 80 -- time constant, slow down time - ~7 minutes
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
MaxDeflection = math.rad(8) -- radians
-- Correction rate with gyro axis deflected on MaxDeflection deg from apparent vertical
CorrectionRate = math.rad(0.4) -- rad/sec with the apex deflection at MaxDeflection degrees (maximum)
-- Minimum correction rate
MinCorrectionRate = CorrectionRate * 0.1 -- rad/sec

ElecConsumerParams = {42.0, true, 101.0, 103.5, 115.0}

GyroElectConsumerParams = {5.0, true, 101.0, 103.5, 115.0}

need_to_be_closed = true -- close lua state after initialization