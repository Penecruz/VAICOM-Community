device_timer_dt	= 0.05

Tachometer_Left 		 = {valmin = 0.0, valmax = 107.0, T1 = 0.3, T2 = 0.2, wmax = 0.0, bias = {{valmin = 0.0, valmax = 107.0, bias = 0.01}}}
Tachometer_Right 		 = {valmin = 0.0, valmax = 107.0, T1 = 0.3, T2 = 0.2, wmax = 0.0, bias = {{valmin = 0.0, valmax = 107.0, bias = 0.01}}}

EGT_Left = {valmin = 0.0, valmax = 1200.0, T1 = 0.3, T2 = 0.2, wmax = 0.0, bias = {{valmin = 0.0, valmax = 1200.0, bias = 0.5}}}
EGT_Right = {valmin = 0.0, valmax = 1200.0, T1 = 0.3, T2 = 0.2, wmax = 0.0, bias = {{valmin = 0.0, valmax = 1200.0, bias = 0.5}}}

OilPressure_Left = {valmin = 0.0, valmax = 100.0, T1 = 0.3, T2 = 0.2, wmax = 0.0, bias = {{valmin = 0.0, valmax = 100.0, bias = 0.5}}}
OilPressure_Right = {valmin = 0.0, valmax = 100.0, T1 = 0.3, T2 = 0.2, wmax = 0.0, bias = {{valmin = 0.0, valmax = 100.0, bias = 0.5}}}

NozzlePos_Left = {valmin = 0.0, valmax = 100.0, T1 = 0.3, T2 = 0.2, wmax = 0.0, bias = {{valmin = 0.0, valmax = 100.0, bias = 0.5}}}
NozzlePos_Right = {valmin = 0.0, valmax = 100.0, T1 = 0.3, T2 = 0.2, wmax = 0.0, bias = {{valmin = 0.0, valmax = 100.0, bias = 0.5}}}

FuelFlow_Left = {valmin = 0.0, valmax = 15000.0, T1 = 0.3, T2 = 0.2, wmax = 0.0, bias = {{valmin = 0.0, valmax = 15000.0, bias = 0.5}}}
FuelFlow_Right = {valmin = 0.0, valmax = 15000.0, T1 = 0.3, T2 = 0.2, wmax = 0.0, bias = {{valmin = 0.0, valmax = 15000.0, bias = 0.5}}}

ElecConsumerParams28	= {2.5, true, 21.2, 22.1, 28.0}
ElecConsumerParams115	= {5.0, true, 78.0, 88.0, 115.0}
ElecConsumerParams26	= {5.0, true, 19.8, 20.8, 26.0}

need_to_be_closed = true -- lua_state  will be closed in post_initialize()