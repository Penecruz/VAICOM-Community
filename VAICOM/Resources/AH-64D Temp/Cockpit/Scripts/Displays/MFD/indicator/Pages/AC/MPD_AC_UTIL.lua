dofile(LockOn_Options.common_script_path.."elements_defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local FMC_PITCH			= 1
local FMC_ROLL			= 2
local FMC_YAW			= 4 
local FMC_COLL			= 8
local FMC_TRIM			= 16
local FMC_NOE_A			= 32

local ANTIICE_SYSTEM	= 0
local ANTIICE_MODE		= 1
local ANTIICE_PITOT		= 2
local ANTIICE_INLET		= 4
local ANTIICE_CANOPY	= 8
local ANTIICE_SENSOR	= 16

local BLEED_AIR_1		= 1
local BLEED_AIR_2		= 2

local AIRCRAFT_ENG = 10
local AIRCRAFT_FLT = 11
local AIRCRAFT_FUEL = 12
local AIRCRAFT_PERF = 13
local AIRCRAFT_UTIL = 14

local Menu = {}
Menu = 
{ 
	{ pb.T1, "ENG",		tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_ENG }}},
	{ pb.T2, "FLT",		tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_FLT }}},
	{ pb.T3, "FUEL",	tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_FUEL }}},
	{ pb.T4, "PERF",	tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_PERF }}},
	{ pb.T6, "UTIL",	tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_UTIL }}},
}

createMenu( Menu )

local Controls = {}
Controls = 
{
	{ "FMC",
		{ 
			{ pb.L1, "{PITCH",	nil, {{"UTIL_FMC", FMC_PITCH}}, {"}PITCH", "{PITCH"}},
			{ pb.L2, "{ROLL",	nil, {{"UTIL_FMC", FMC_ROLL}}, 	{"}ROLL", "{ROLL"}},
			{ pb.L3, "{YAW",	nil, {{"UTIL_FMC", FMC_YAW}}, 	{"}YAW", "{YAW"}},
			{ pb.L4, "{COLL",	nil, {{"UTIL_FMC", FMC_COLL}}, 	{"}COLL", "{COLL"}},
			{ pb.L5, "{TRIM",	nil, {{"UTIL_FMC", FMC_TRIM}}, 	{"}TRIM", "{TRIM"}},
			{ pb.L6, "{NOE/A",	nil, {{"UTIL_FMC", FMC_NOE_A}},{"}NOE/A", "{NOE/A"}}
		} 
	},
	{ "ANTIICE", 
		{ 
			{ pb.R1, 
				{ 
					{"SYSTEM",	nil, {{"UTIL_AntiIce", ANTIICE_SYSTEM}}}, 
					{"AUTO", nil, {{"UTIL_AntiIce", ANTIICE_MODE}}, { "MANUAL", "AUTO"} } 
				} 
			},
			{ pb.R3, "PITOT{ ",	nil, {{"UTIL_AntiIce", ANTIICE_PITOT}} , {"PITOT}", "PITOT{"}},
			{ pb.R4, "INLET{ ",	nil, {{"UTIL_AntiIce", ANTIICE_INLET}} , {"INLET}", "INLET{"}},
			{ pb.R5, "CANOPY{ ",	nil, {{"UTIL_AntiIce", ANTIICE_CANOPY}} , {"CANOPY}", "CANOPY{"}},
			{ pb.R6, "SENSOR{ ",	nil, {{"UTIL_AntiIce", ANTIICE_SENSOR}} , {"SENSOR}", "SENSOR{"}}
		} 
	},
	{ pb.B1, "UTIL",	tp_default_border, {{"MFD_AC_OriginatorFmt"}}, {"ENG", "FLT", "FUEL", "PERF","UTIL"} },
	{ "BLEED AIR",
		{ 
			{ pb.B2, "{1",	nil, {{"UTIL_BleedAir", BLEED_AIR_1}}, {"}1", "{1"}},
			{ pb.B3, "{2",	nil, {{"UTIL_BleedAir", BLEED_AIR_2}}, {"}2", "{2"}}
		} 
	},
	{ pb.B4, {
				{"TEMP", nil },
				{"70^F", tp_default_border, {{"UTIL_ECS_Draw"},{"UTIL_SET_Temp_Button", pb.B4},{"MFD_DataEntryButton_frame", pb.B4}}}
			}
	},
	{ pb.B5, "{ECS",	nil, 			{{"UTIL_ECS_Inv", 0},{"UTIL_ECS"}} , {"}ECS", "{ECS"}},
	{ pb.B5, "{ECS",	tp_default_inv,	{{"UTIL_ECS_Inv", 1},{"UTIL_ECS"}} , {"}ECS", "{ECS"}},
}
createControls( Controls )
	
local MFD_NUM = readParameter("MFD_NUM")  -- see MFD_SELF_IDS table
local hdr
if MFD_NUM == MFD_SELF_IDS.PLT_LMFD or MFD_NUM == MFD_SELF_IDS.PLT_RMFD then
	hdr = "PLT CKPT TEMP"
else
	hdr = "CPG CKPT TEMP"
end

local Boxes = {}
	-- first line is required in format:	{pos_left_up, controllers,  name, parent, h_space,  text_properties,  margins,},
	-- next : 	{{ text, text_properties, controllers, 	formats, 	margins,  	name }, {},{}...}
	-- margins: left, down, right, up
Boxes = 
{ 	
	{
		{{-70, 300}, nil, nil, nil, nil, nil, { 10,10,10,-10 } },
		
			{
				{"FAT", tp_28, nil,nil,{35,0,0,0}}
			},
			{
				{"4", tp_36_right_bottom,{{"UTIL_Fat"}},nil,{75,0,0,0}},
				{"^C", tp_36, nil,nil,{0,0,0,0}} 
			}
		
	},
	{
		{{-70, 110}, nil, nil, nil, nil, nil, { 20,10,20,-10 } },
		
			{
				{"ICE", tp_28, nil,nil,{20,0,0,0}}
			},
			{
				{"TRACE", tp_36_center_bottom,{{"UTIL_Ice"}},{"TRACE", "LIGHT", "MODER", "SEVERE"},{55,0,0,0}}
			}
		
	},
	{
		{{-150, -70}, nil, nil, nil, nil, nil, { 10,10,10,-10 } },
		
			{
				{ hdr, tp_28, nil,nil,{1,0,0,0}}
			},
			{
				{"77", tp_36_right_bottom,{{"UTIL_ECS_Draw"},{"UTIL_Temp", MFD_NUM}},nil,{140,0,0,0}},
				{"^F", tp_36, nil,nil,{0,0,0,0}} 
			}
		
	}
}
createInfoBoxes( Boxes )


