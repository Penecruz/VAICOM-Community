device_timer_dt		= 0.05
need_to_be_closed	= true -- lua_state  will be closed in post_initialize()

selfID	= 0

psedoRangeErrorProcessTx	= 25.0
SNRProcess					= 20.0

channel_quantity	= 5
use_L2				= true
use_carrier			= true
position_latency	= 0.07
position_update_dt	= 0.5	-- PSR measuring rate
status_update_dt	= 2.0	-- satellites re-acquisition and DOPS calculation rate

antenna_pos	= {-9.400, 2.760, 0.0}

min_elevation	= -10.0
terrainLOS		= true

Starting_Time = {}
Starting_Time["hot_start_time"]		= 90
Starting_Time["warm_start_time"]	= 120
Starting_Time["cold_start_time"]	= 180

Satellite_re_aquisition_time = {}
Satellite_re_aquisition_time[10] = 3
Satellite_re_aquisition_time[60] = 5

--coords extrapolation
coords_extrapolation_enable	= true
coords_extrapolation_dt		= 0.05	-- coords extrapolation rate

Kalman_filter_enable		= false