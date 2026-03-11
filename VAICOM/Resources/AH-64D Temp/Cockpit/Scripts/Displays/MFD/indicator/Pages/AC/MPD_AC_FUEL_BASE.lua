dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_FUEL_Symbology.lua")

txt_controllers = txt_controllers or {}
		txt_controllers[#txt_controllers + 1] = {"MFD_SetColor_White"}
boost = 1;
boost0 = 0;
local AIRCRAFT_ENG = 10
local AIRCRAFT_FLT = 11
local AIRCRAFT_FUEL = 12
local AIRCRAFT_PERF = 13
local AIRCRAFT_UTIL = 14

local Menu = {}
Menu = 
{ 
	{ pb.T1, "ENG",		tp_default_border, {{"MFD_MenuPrevBorder",AIRCRAFT_ENG}}},
	{ pb.T2, "FLT",		tp_default_border, {{"MFD_MenuPrevBorder",AIRCRAFT_FLT}}},
	{ pb.T3, "FUEL",	tp_default_border, {{"MFD_MenuPrevBorder",AIRCRAFT_FUEL}}},
	{ pb.T4, "PERF",	tp_default_border, {{"MFD_MenuPrevBorder",AIRCRAFT_PERF}}},
	{ pb.T6, "UTIL",	tp_default_border, {{"MFD_MenuPrevBorder",AIRCRAFT_UTIL}}},
}
createMenu( Menu )

local Controls = {} -- Controls( pb_num,  value,  tp,  controllers, formats, margins)
Controls = 
{
	{ pb.B1, "FUEL",	tp_default_border, {{"MFD_AC_OriginatorFmt"}}, {"ENG", "FLT", "FUEL", "PERF","UTIL"} },
	{ pb.R6, { {"TYPE", nil }, {"JP8", tp_default_border , {{"FUEL_TypeSelect"}}, {"JP8", "JP5", "JP4"} } } },	
}
createControls( Controls )
local ExtFuelTanks = {}

ExtFuelTanks[1] = draw_fuel_tank( 1,  IND_MPD_MATERIAL_GREEN )
ExtFuelTanks[2] = draw_fuel_tank( 2,  IND_MPD_MATERIAL_GREEN  )
ExtFuelTanks[3] = draw_fuel_tank( 3,  IND_MPD_MATERIAL_GREEN  )
ExtFuelTanks[4] = draw_fuel_tank( 4,  IND_MPD_MATERIAL_GREEN  )

draw_plane( {0, 120},  IND_MPD_MATERIAL_GREEN )
--------------------------------------------------------------------------------------------------
	-- first line is required in format: 	{pos_left_up,  controllers,  name, parent, h_space,  text_properties,  margins,  },
	-- next : 	{{ text, tp, controllers, formats, margins,  name }, {},{}...} 
	-- margins: left, down, right, up 
------------------------------------------------------------------------------------------------------
--controller: args[0]:
--1: SIGNALS_RS343_MPD::DP_FwdFuelQty
--2: SIGNALS_RS343_MPD::DP_AftFuelQty
--3: SIGNALS_RS343_MPD::DP_IafsFuelQty
--4: SIGNALS_RS343_MPD::DP_IntFuelQty
--5: SIGNALS_RS343_MPD::DP_TotalFuelQty
--6: SIGNALS_RS343_MPD::DP_ExtFuelQty
--controller: args[1]: threshold to change color to YELLOW ( FWD & AFT only )
--controller: args[1], args[2]: threshold_fwd,  threshold_aft to change color to YELLOW ( IntFuelQty only )

local EXT_L_TANK		= 0	
local EXT_R_TANK		= 1

local FWD_FUEL_QTY		= 1	
local AFT_FUEL_QTY		= 2	
local IAFS_FUEL_QTY		= 3	
local INT_FUEL_QTY		= 4	
local TOTAL_FUEL_QTY	= 5	
local EXT_FUEL_QTY		= 6	
local ENG1_FLOW			= 7	
local ENG2_FLOW			= 8
local TOT_FLOW			= 9


local FWD_ENG1_LINE		= 1 
local FWD_ENG2_LINE		= 2
local AFT_ENG1_LINE		= 4
local AFT_ENG2_LINE		= 8
local FWD_L_AUX_LINE	= 16
local AFT_R_AUX_LINE	= 32
local IAFS_FWD_LINE		= 64
local IAFS_AFT_LINE		= 128
local FWD_AFT_LINE		= 256
local FWD_AFT_LINE_DOT	= 512

local FWD_FUEL_THRESHOLD		= 240 --LB	
local AFT_FUEL_THRESHOLD		= 260 --LB	

-- draw fuel lines
local verts = 
{
	{{-58,310}, {-138, 310}, {-138, 114}},
	{{ 60,310}, { 135, 310}, { 135, 116}},
	{{-25,-110}, {-25, -165}, {-120, -165}, {-120, -90}},
	{{ 25,-110}, { 25, -165}, { 120, -165}, { 120, -90}},
	{{-60,325}, {-190, 325}, {-190, 210}},					--FWD_L_AUX_LINE
	{{0,-110}, {0, -210}, {190, -210}, {190, 30}},			--AFT_R_AUX_LINE
	{{-5,278}, {-5, 185}},
	{{-5,135}, {-5, 7}},
	{{30,278}, {30, 15}},
	{{30,300}, {30, 0}},  								--last item for ...dot!!!
}
draw_fuel_dot_line( verts[#verts], FWD_AFT_LINE_DOT)	-- FWD - AFT

for  index = 1, #verts-1 do
	draw_fuel_line( verts[index], 2^(index-1) )
end			

--local openingMask = openMaskArea(0, getLabelName().."_Mask", buildBoxVerts(12, 80, "CenterCenter"), default_box_indices, {30,330} )
--local openingMask1 = openMaskArea(0, getLabelName().."_Mask", buildBoxVerts(12, 80, "CenterCenter"), default_box_indices, {30,-40} )


local Boxes = {}
	-- first line is required in format: 	{pos_left_up, controllers,  name, parent, h_space,  text_properties,  margins,},
	-- next : 	{{ text, text_properties, controllers, 	formats, 	margins,  	name }, {},{}...} 
	-- margins: left, down, right, up 
Boxes = 
{ 	
	{
		{{-160,-300}, nil, nil, nil, nil, nil, { 10,10,40,-10 } },
		{{"   CALC FLOW", tp_28}},
		{{"  1 ", tp_28},	{" 000", tp_36, {{"FUEL_Qty", ENG1_FLOW}} },	{"LB/HR", tp_28} },
		{{"  2 ", tp_28},	{" 000", tp_36, {{"FUEL_Qty", ENG2_FLOW}} },	{"LB/HR", tp_28} },
		{{"TOT ", tp_28},	{" 000", tp_36, {{"FUEL_Qty", TOT_FLOW}} },	{"LB/HR", tp_28} } 
	},
	{
		{{-70, -235}, nil, nil, nil, nil, nil, { 10,10, -10,-12 } },
		{{"SFR ", tp_28},	{".16",  tp_36, {{"FUEL_SFR"}} } }
	},
	{
		{{-450,-350}, nil, nil, nil, nil, nil, { 10,10,10,-12 } },
		{{"INT ", tp_28},	{"0000", tp_36 , {{"FUEL_Qty",INT_FUEL_QTY, FWD_FUEL_THRESHOLD, AFT_FUEL_THRESHOLD}} },		{"LB", tp_28} },
		{{"TOT ", tp_28, {{"FUEL_VisibleForAuxConfig"}}},	{"0000", tp_36 , {{"FUEL_Qty",TOTAL_FUEL_QTY}} },		{"LB",  tp_28, {{"FUEL_VisibleForAuxConfig"}}} } 
	},
	{
		{{200,-310}, nil, nil, nil, nil, nil, { 10,10,10,-10 } },
		{{"  ENDR", tp_28}},
		{{"INT ", tp_28},	{"    ", tp_36 , {{"FUEL_Endr_int"}} } },
		{{"TOT ", tp_28,  {{"FUEL_VisibleForAuxConfig"}}},	{"    ", tp_36 , {{"FUEL_Endr_Tot"}} } } 
	},
	{
		-- FWD Tank
		{{-57,340}, nil, nil, nil, nil, nil, {10,35,10,-20} },
		{	{"0000", tp_36, {{"FUEL_Qty", FWD_FUEL_QTY, FWD_FUEL_THRESHOLD}} } }
	},
	{
		-- AFT Tank
		{{-50, 10} , nil, nil, nil, nil, nil, {5,120,2, -50} },
		{	{"0000", tp_36, {{"FUEL_Qty", AFT_FUEL_QTY, AFT_FUEL_THRESHOLD}} } }
	},
	{
		-- IAFS Tank
		{{-75, 185} , {{"FUEL_IAFS_Show"}}, nil, nil, nil, nil, {10,10,5,-12} },
		{	{"000", tp_36, {{"FUEL_Qty", IAFS_FUEL_QTY}} } }
	}
}
createInfoBoxes( Boxes )