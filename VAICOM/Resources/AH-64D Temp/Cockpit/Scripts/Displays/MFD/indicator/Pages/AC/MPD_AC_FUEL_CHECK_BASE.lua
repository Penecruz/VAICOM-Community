dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_FUEL_Symbology.lua")


local Controls = {} -- Controls( pb_num,  value,  tp,  controllers, formats, margins)
Controls = 
{
	{ "MINUTES", { 
						{ pb.R2, "15",	nil, {{"FUEL_CheckMinutes", 15}} }, 
						{ pb.R3, "20",	nil, {{"FUEL_CheckMinutes", 20}}	}, 
						{ pb.R4, "30",	nil, {{"FUEL_CheckMinutes", 30}} } 
				} 
	},
	{ pb.R5, "START",	nil, {{"FUEL_CheckStart"}}, 	{"START", "STOP"} },
	{ pb.B6, "CHECK", tp_default_border  },	 
	{ pb.B1, "FUEL",	tp_default_border, {{"MFD_AC_OriginatorFmt"}}, {"ENG", "FLT", "FUEL", "PERF","UTIL"} },
	{ pb.R6, { {"TYPE", nil }, {"JP8", tp_default_border , {{"FUEL_TypeSelect"}}, {"JP8", "JP5", "JP4"} } } },	
}
createControls( Controls )	

local w = tp_28_left_center.width
local h = tp_28_left_center.height
	
	AddRoundCornersWindow("FuelStartCheckInfoWindow",	{-280, pb_props[pb.L1].pos[2] + 25 }, w*17.0, h*5.0,
	{
		{"RUN",		{-w*8,		h + 12},	tp_28_left_center }, 
		{"  ",		{ w*4,		h + 18},	tp_36_right_center, {{"FUEL_CheckStartInfo",	1}}},  
		{":",		{ w*4.4,	h + 18},	tp_36_right_center}, 
		{"  ",		{ w*6.8,	h + 18},	tp_36_right_center, {{"FUEL_CheckStartInfo",	2}}},  	
					 		
		{"START",	{-w*8,		h - 30},	tp_28_left_center}, 
		{"  ",		{ w*4,		h - 28},	tp_36_right_center, {{"FUEL_CheckStartInfo",	3}}}, 
		{":",		{ w*4.4,	h - 28},	tp_36_right_center},
		{"  ",		{ w*6.8,	h - 28},	tp_36_right_center, {{"FUEL_CheckStartInfo",	4}}},
		{" ",		{ w*7,		h - 30},	tp_28_left_center,	{{"FUEL_CheckStartInfo",	5}}},
					 		
		{"RATE",	{-w*8,		h - 72},	tp_28_left_center}, 	
		{"",		{ w*2,		h - 70},	tp_36_right_center, {{"FUEL_CheckStartInfo",	6}}},
		{"LB/HR",	{ w*3,		h - 72},	tp_28_left_center}, 
	},
	tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	{{"FUEL_StartCheckInfo"}}, "RightCenter", nil, 1, false)
							
	AddRoundCornersWindow("FuelCheckResInfoWindow",	{280, pb_props[pb.L1].pos[2] + 25 }, w*17.0, h*5.0,
	{
		{"BURNOUT",	{-w*8,		h + 12},	tp_28_left_center}, 
		{"  ",		{ w*4,		h + 18},	tp_36_right_center, {{"FUEL_CheckResInfo",	1}}}, 
		{":",		{ w*4.4,	h + 18},	tp_36_right_center},
		{"  ",		{ w*6.8,	h + 18},	tp_36_right_center, {{"FUEL_CheckResInfo",	2}}},
		{" ",		{ w*7,		h + 12},	tp_28_left_center,	{{"FUEL_CheckResInfo",	7}}},
					 		
		{"VFR RES",	{-w*8,		h - 30},	tp_28_left_center}, 
		{"  ",		{ w*4,		h - 28},	tp_36_right_center, {{"FUEL_CheckResInfo",	3}}}, 
		{":",		{ w*4.4,	h - 28},	tp_36_right_center},
		{"  ",		{ w*6.8,	h - 28},	tp_36_right_center, {{"FUEL_CheckResInfo",	4}}},
		{" ",		{ w*7,		h - 30},	tp_28_left_center,	{{"FUEL_CheckResInfo",	7}}},
					 		
		{"IFR RES",	{-w*8,		h - 72},	tp_28_left_center}, 
		{"  ",		{ w*4,		h - 70},	tp_36_right_center, {{"FUEL_CheckResInfo",	5}}}, 
		{":",		{ w*4.4,	h - 70},	tp_36_right_center},
		{"  ",		{ w*6.8,	h - 70},	tp_36_right_center, {{"FUEL_CheckResInfo",	6}}},
		{" ",		{ w*7,		h - 72},	tp_28_left_center,	{{"FUEL_CheckResInfo",	7}}},
	},
	tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	{{"FUEL_CheckResInfoVisible"}}, "RightCenter", nil, 1, false)						


