dofile(LockOn_Options.script_path .. "command_defs.lua")
dofile(LockOn_Options.script_path .. "devices.lua")

VR_capture = 
{
	stick = 
	{
		range_roll   = math.rad(30.0),
		range_pitch  = math.rad(30.0),
		
		left_turn = {180,0,-90},
		left_shift = {0.08,-0.01,0.035},
		
		right_turn = {180,15,90},
		right_shift = {0.08,-0.04,-0.035},
	},

	collective_left = 
	{
		device_key = "collective",
		
		arg 		 = 475,
		send_command = 2087, --collective
		connector    = "THROTTLE_HANDLE1",
		
		left_turn = {100,15,0},
		left_shift = {0.0,0.025,0.09},
		
		right_turn = {100,40,0},
		right_shift = {0.02,-0.015,0.08},
	},
	collective_right = 
	{
		device_key = "collective",
		
		arg 		 = 474,
		send_command = 2087, --collective
		connector    = "THROTTLE_HANDLE2",
		
		left_turn = {100,15,0},
		left_shift = {0.0,0.025,0.09},
		
		right_turn = {100,40,0},
		right_shift = {0.02,-0.015,0.08},
	},
	
	sight_left =
	{
		device_key = "sight",
	
		connector = "SIGHT_HANDLE_L",
		
		controller = "left",
		
		left_turn = {180, 0, -90},
		left_shift = {0.08, -0.02, 0.045},
	},
	
	sight_right =
	{
		device_key = "sight",
	
		connector = "SIGHT_HANDLE_R",
		
		controller = "right",
		
		right_turn = {180,0,90},
		right_shift = {0.08,-0.02,-0.045},
	},
	
}

VR_device =
{
	cpg_engine_left = 
	{
		device_key = "throttle",
	
		left_turn = {-90, 0, 180},
		left_shift = {0.01,-0.04,-0.09},
		
		right_turn = {-130,0,-180},
		right_shift = {0.07,-0.03,-0.04},
	},
	cpg_engine_right = 
	{
		device_key = "throttle",
	
		left_turn = {-90, 0, 180},
		left_shift = {0.01,-0.04,-0.09},
		
		right_turn = {-130,0,-180},
		right_shift = {0.07,-0.03,-0.04},
	},

	pilot_engine_left = 
	{
		device_key = "throttle",
	
		left_turn = {-90, 0, 180},
		left_shift = {0.01,-0.04,-0.09},
		
		right_turn = {-130,0,-180},
		right_shift = {0.07,-0.03,-0.04},
	},
	pilot_engine_right = 
	{
		device_key = "throttle",
	
		left_turn = {-90, 0, 180},
		left_shift = {0.01,-0.04,-0.09},
		
		right_turn = {-130,0,-180},
		right_shift = {0.07,-0.03,-0.04},
	},


}