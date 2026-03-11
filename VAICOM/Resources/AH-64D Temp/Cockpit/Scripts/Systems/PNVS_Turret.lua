local gettext = require("i_18n")
_ = gettext.translate

argument_azimuth   = 600

limits_azimuth   = {math.rad(-90),math.rad(90)}
limits_elevation = {math.rad(-45) ,math.rad(20)}

azimuth_gear_ratio 		= 0.5 --0.5
elevation_gear_ratio 	= 0.3 --0.3

pos_local			= {5.0, 0.10, 0.0}
pos_axis_y_local	= {4.8, 0.10, 0.0}

need_to_be_closed = true -- lua_state  will be closed in post_initialize()К