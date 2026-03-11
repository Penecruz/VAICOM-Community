dofile(LockOn_Options.script_path.."Computers/DP/device/DP_Common.lua")

device_timer_dt	= 0.05
selfID = DP_SELF_IDS.DP_CPG
need_to_be_closed = true -- close lua state after initialization