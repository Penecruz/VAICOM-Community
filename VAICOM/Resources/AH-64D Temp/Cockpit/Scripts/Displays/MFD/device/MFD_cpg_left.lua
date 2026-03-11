dofile(LockOn_Options.script_path.."Displays/MFD/device/MFD_device_IDs.lua")

device_timer_dt	= 0.05
selfID = MFD_SELF_IDS.CPG_LMFD
need_to_be_closed = true -- close lua state after initialization