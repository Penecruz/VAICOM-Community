device_timer_dt		= 0.006

error_k	= 1.021


MIN = 0.0
MAX = 250.0 -- kts

IAS_pointer = {valmin = 0.0, valmax = 250.0, T1 = 0.267, T2 = 0.258, wmax = 0, bias = {{valmin = 0.0, valmax = 250.0, bias = 0.3}}}

need_to_be_closed = true -- close lua state after initialization 