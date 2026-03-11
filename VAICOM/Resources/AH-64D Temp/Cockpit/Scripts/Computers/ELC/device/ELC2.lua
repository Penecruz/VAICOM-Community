dofile(LockOn_Options.script_path.."Computers/ELC/device/ELC_device_IDs.lua")

device_timer_dt	= 0.1
selfID = ELC_SELF_IDS.ELC2
need_to_be_closed = true -- close lua state after initialization