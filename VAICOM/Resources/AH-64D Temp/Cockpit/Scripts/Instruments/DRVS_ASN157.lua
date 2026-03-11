-- DRVS AN/ASN-157 Doppler Radar Velocity Sensor

-- TODO: Almost all present data is from Mi8::DISS_15, although it is much like Ka50::DISS-32-28

device_timer_dt = 0.0625 -- seconds
device_pos = {-3.05, -0.7, 0.16} -- [m]

time_to_power_up = 30.0 -- seconds   -- ??? why? there were even 150.0 in Ka50 and Mi8!
time_to_wave_search = 5.0 -- seconds  -- ???

l0 = 0.02256 -- wave length (meters)	-- from f=13.25 .. 13.4 GHz
min_doppler_frequency = 3.0		--???
max_doppler_frequency = 10000.0 --???

min_beam_travel = 0.5 -- meters
max_beam_travel = 3048.0 -- meters

-- no info about this. Is these are common values?
beam_side_angle = math.rad(30.0); -- radians
beam_elev = math.rad(30.0); -- radians

wave_angleK = 9.25 -- dimentionless			-- not used in avDNS, only initialization

max_drift_angle = math.rad(46.0) -- radians	-- not used in avDNS, only initialization
max_pitch_bank = math.rad(30.0) -- radians	-- not used in avDNS, only initialization

Damage = {{Element = 55}, {Element = 58}}
--
--
-- calc_pos_change_speed = {init = 0.0, acc = 0.0, maximum = 0.0}

-- w = 25 / 3.6  -- m/s

-- needle_hover_Wx = {valmin = -w, valmax = 2*w, T1 = 0.9, T2 = 0.316, wmax = 0, bias = {{valmin = -w, valmax = 2*w, bias = 0.005}}}
-- needle_hover_Wy = {valmin = -10, valmax = 10, T1 = 0.9, T2 = 0.316, wmax = 0, bias = {{valmin = -10, valmax = 10, bias = 0.005}}}
-- needle_hover_Wz = {valmin = -w, valmax = w, T1 = 0.9, T2 = 0.316, wmax = 0, bias = {{valmin = -w, valmax = w, bias = 0.005}}}
-- needle_drift_angle = {valmin = -max_drift_angle, valmax = max_drift_angle, T1 = 0.9, T2 = 0.316, wmax = 0, bias = {{valmin = -max_drift_angle, valmax = max_drift_angle, bias = 0.005}}}


need_to_be_closed = true -- lua_state  will be closed in post_initialize()