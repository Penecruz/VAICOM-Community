dofile(LockOn_Options.script_path.."Computers/WP/device/WP_device_IDs.lua")

device_timer_dt	= 0.05
selfID = WP_SELF_IDS.WP2
need_to_be_closed = true -- close lua state after initialization