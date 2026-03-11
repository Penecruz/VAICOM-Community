dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
--dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_formats_IDs.lua")
dofile(LockOn_Options.script_path.."materials.lua")

----------------------------------------------------------------------------------------------
--txt_controllers = txt_controllers or {}
--		txt_controllers[#txt_controllers + 1] = {"MFD_SetColor_White"}

local Menu = {}
Menu = 
{ 
	{ pb.T1, "VIDEO",		nil },
	{ pb.T2, "VCR",			nil },
	{ pb.T6, "*",			nil },
	{ "COMMUNICATION", 
		{
			{ pb.R1, "DL",		nil },
			{ pb.R2, "XPNDR",	nil },
			{ pb.R3, "UHF",		nil },
			{ pb.R4, "FM",		nil },
			{ pb.R5, "HF",		nil },
			{ pb.R6, "COM",		nil }
		}
	},
	{ "AIRCRAFT", 
		{
			{ pb.B2, "ENG",		nil },
			{ pb.B3, "FLT",		nil },
			{ pb.B4, "FUEL",	nil },
			{ pb.B5, "PERF",	nil },
			{ pb.B6, "UTIL",	nil }
		}
	},
	{ "MISSION", 
		{
			{ pb.L3, "ASE",		nil },	
			{ pb.L4, "TSD",		nil },
			{ pb.L5, "WPN",		nil },
			{ pb.L6, "FCR",		nil }
		}
	},
	{ pb.B1, "DMS",		nil }	
}
	
createMenu( Menu )
--------------------------------------------------------------------------------------------------------
--local Boxes = {}
----	-- first line is required in format: 	{pos_left_up, controllers,  name, parent, h_space,  text_properties,  margins,},
----	-- next : 	{{ text, text_properties, controllers, 	formats, 	margins,  	name }, {},{}...} 
----	-- margins: left, down, right, up 
--
--Boxes = 
--{ 	
--	{
--		{{-80, 430}, {{"Test"}}, nil, nil, nil, nil, { 10,10,10,-10 } },
--		
--			{
--				{"---TEST UP---", tp_28, nil,nil,{20,0,0,0}}
--			},
--			{
--				{"---TEST UP---", tp_28, nil,nil,{20,0,0,0}}
--			},
--			{
--				{"---TEST UP---", tp_28, nil,nil,{20,0,0,0}}
--			},
--			{
--				{"---TEST UP---", tp_28, nil,nil,{20,0,0,0}}
--			},
--			{
--				{"---TEST UP---", tp_28, nil,nil,{20,0,0,0}}
--			},
--		
--	},
--}
--createInfoBoxes( Boxes, 1)
--
--Boxes = 
--{ 	
--	{
--		{{-150, 400}, nil, nil, nil, nil, nil, { 10,10,10,-10 } },
--		
--			{
--				{"TEST DOWN", tp_28, nil,nil,{20,0,0,0}}
--			},
--			{
--				{"400", tp_36_right_bottom, nil, nil,{75,0,0,0}},
--				{"^C", tp_36, nil,nil,{0,0,0,0}} 
--			}
--		
--	},
--	{
--		{{-150, 250}, nil, nil, nil, nil, nil, { 20,10,20,-10 } },
--		
--			{
--				{"TEST DOWN", tp_28, nil,nil,{20,0,0,0}}
--			},
--			{
--				{"TRACE", tp_36_center_bottom,{{"UTIL_Ice"}},{"TRACE", "LIGHT", "MODER", "SEVERE"},{55,0,0,0}}
--			}
--		
--	},
--}
--createInfoBoxes( Boxes, 0)
---- AddRoundCornersWindow(name, pos, radius, width, height, value, tp, material, parent, controllers, alignment, box_cntrs, up_down)
--AddRoundCornersWindow("down",	{-50, -100},	RoundingRadius,	200,	400,
--						{
--							{"TEST UP",		{0.0,	tp_default.height*5.0},		tp28,	nil, 0},
--							{"TEST UP",		{0.0,	tp_default.height*3.0},		tp28,	nil, 0},
--							{"TEST UP",		{0.0,	tp_default.height*1.0},		tp28,	nil, 0},
--							{"TEST UP",		{0.0,	tp_default.height*-1.0},	tp28,	nil, 0},
--							{"TEST UP",		{0.0,	tp_default.height*-3.0},	tp28,	nil, 0},
--							{"TEST UP",		{0.0,	tp_default.height*-5.0},	tp28,	nil, 0},			
--						},
--						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil, nil, nil, 1)
--						
--AddRoundCornersWindow("up",	{-150, -150},	RoundingRadius,	200,	400,
--						{
--							{"TEST DOWN",		{0.0,	tp_default.height*5.0},		tp28,	nil, 0},
--							{"TEST DOWN",		{0.0,	tp_default.height*3.0},		tp28,	nil, 0},
--							{"TEST DOWN",		{0.0,	tp_default.height*1.0},		tp28,	nil, 0},
--							{"TEST DOWN",		{0.0,	tp_default.height*-1.0},	tp28,	nil, 0},
--							{"TEST DOWN",		{0.0,	tp_default.height*-3.0},	tp28,	nil, 0},
--							{"TEST DOWN",		{0.0,	tp_default.height*-5.0},	tp28,	nil, 0},			
--						},
--						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil, nil, nil, 0)
