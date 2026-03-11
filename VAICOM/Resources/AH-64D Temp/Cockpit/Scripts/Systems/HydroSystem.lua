device_timer_dt	= 0.05
-- TODO:  Digital Readout: 0 to 6000 psi, resolution 10 psi
pressure_indicator = {valmin = 0.0, valmax = 4000.0, T1 = 0.9, T2 = 0.316, wmax = 0.0, bias = {{valmin = 0.0, valmax = 4000.0, bias = 0.5}}}

need_to_be_closed = true -- lua_state  will be closed in post_initialize()
