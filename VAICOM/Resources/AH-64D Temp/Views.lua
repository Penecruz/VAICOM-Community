ViewSettings = {
	Cockpit = {
	
		CockpitAnchorPoint 			= {1.926, 0.922, 0.0},
		[1] = {-- player slot 1 (PLT)
			CameraViewAngleLimits	= {20.000000,140.000000},
			CockpitLocalPoint		= {1.784, 0.992, 0.045},
			CameraAngleRestriction	= {false,90.000000,0.500000},
			CameraAngleLimits		= {200,-90.000000,90.000000},
			EyePoint				= {0.090, 0.000, 0.000},
			ShoulderSize			= 0.25,
			Allow360rotation		= false,
			limits_6DOF				= {x = {-0.2,0.3}, y = {-0.3,0.2}, z = {-0.3,0.3}, roll = 90.000000},
		},
		[2] = {-- player slot 2 (CPG)
			CameraViewAngleLimits	= {20.000000,140.000000},
			CockpitLocalPoint		= {1.784 + 1.300, 0.992 - 0.45, 0.045},
			CameraAngleRestriction	= {false,90.000000,0.500000},
			CameraAngleLimits		= {200,-90.000000,90.000000},
			EyePoint				= {0.090, 0.000, 0.000},
			ShoulderSize			= 0.25,
			Allow360rotation		= false,
			limits_6DOF				= {x = {-0.2,0.3}, y = {-0.3,0.2}, z = {-0.3,0.3}, roll = 90.000000},
		},
	},
	Chase = {
		LocalPoint		= {-5.0,1.0,3.0},
		AnglesDefault	= {0.000000,0.000000},
	},
	Arcade = {
		LocalPoint		= {-21.500000,5.618000,0.000000},
		AnglesDefault	= {0.000000,-8.000000},
	},
}

local function head_pos_default_PLT(tab)
	if not tab then
		tab = {}
	end

	tab.viewAngle	= tab.viewAngle		or	76.0
	tab.hAngle		= tab.hAngle		or	0.0
	tab.vAngle		= tab.vAngle		or -10.0
	tab.x_trans		= tab.x_trans		or	0.0
	tab.y_trans		= tab.y_trans		or	0.0
	tab.z_trans		= tab.z_trans		or	0.0
	tab.rollAngle	= tab.rollAngle		or	0.0

	return tab
end

local function head_pos_default_CPG(tab)
	if not tab then
		tab = {}
	end

	tab.viewAngle	= tab.viewAngle		or	76.0
	tab.hAngle		= tab.hAngle		or	0.0
	tab.vAngle		= tab.vAngle		or  -25.0
	tab.x_trans		= tab.x_trans		or 0.22
	tab.y_trans		= tab.y_trans		or -0.018
	tab.z_trans		= tab.z_trans		or	0.0
	tab.rollAngle	= tab.rollAngle		or	0.0

	return tab
end

local function head_pos_default_HMD_PLT(tab)
	if not tab then
		tab = {}
	end

	tab.viewAngle	= tab.viewAngle		or	76.0
	tab.hAngle		= tab.hAngle		or	0.0
	tab.vAngle		= tab.vAngle		or -10.0
	tab.x_trans		= tab.x_trans		or	0.0
	tab.y_trans		= tab.y_trans		or	0.0
	tab.z_trans		= tab.z_trans		or	0.0
	tab.rollAngle	= tab.rollAngle		or	0.0

	return tab
end

local function head_pos_default_HMD_CPG(tab)
	if not tab then
		tab = {}
	end

	tab.viewAngle	= tab.viewAngle		or	76.0
	tab.hAngle		= tab.hAngle		or	0.0
	tab.vAngle		= tab.vAngle		or -5.0
	tab.x_trans		= tab.x_trans		or	0.22
	tab.y_trans		= tab.y_trans		or	0.0
	tab.z_trans		= tab.z_trans		or	0.0
	tab.rollAngle	= tab.rollAngle		or	0.0

	return tab
end

SnapViews = {
	[1] = {-- player slot 1
		[1] = head_pos_default_PLT({}),--Num 0
		[13] = head_pos_default_PLT({}),-- default view
		[14] = head_pos_default_HMD_PLT({}),-- default view if VR Active
	},
	[2] = {-- player slot 2
		[1] = head_pos_default_CPG({}),--Num 0
		[13] = head_pos_default_CPG({}),-- default view
		[14] = head_pos_default_HMD_CPG({}),-- default view if VR Active
	},
}
