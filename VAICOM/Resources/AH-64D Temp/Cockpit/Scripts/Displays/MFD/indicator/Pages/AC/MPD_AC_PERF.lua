dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

--addText( "AC PERF PAGE",  {0, 100})



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
	{ pb.B5, "HIT",		nil },
}
	
createMenu( Menu )

local Controls = {}
Controls = 
{
	{ pb.B1, "PERF",	tp_default_border, {{"MFD_AC_OriginatorFmt"}}, {"ENG", "FLT", "FUEL", "PERF","UTIL"} },
	{ pb.L1, 	{{"PA>",	nil },{"3210",	tp_default_border, 		{{"PERF_PA_Button"},	{"MFD_DataEntryButton_frame",pb.L1}} }},		nil,		nil},
	{ pb.L2, 	{{"FAT>",	nil },{"+6",	tp_default_border, 		{{"PERF_FAT_Button"},	{"MFD_DataEntryButton_frame",pb.L2}} }},		nil,		nil},
	{ pb.L3, 	{{"GWT>",	nil },{"17650",	tp_default_border, 		{{"PERF_GWT_Button"},	{"MFD_DataEntryButton_frame",pb.L3}} }},		nil,		nil},
	
	{ "PERF MODE", 
				{ 
					{ pb.B2, 	"CUR",		tp_default_border,		{{"PERF_MODE_Border", 0}}},
					{ pb.B3, 	"MAX",		tp_default_border,		{{"PERF_MODE_Border", 1}}},
					{ pb.B4, 	"PLAN",		tp_default_border,		{{"PERF_MODE_Border", 2}}},
				}
	},
	
	{ pb.B6, 	"WT",		nil,		nil},
}

createControls( Controls )


local tp_28_cc						= createTextProperty( 28,  nil,	nil,  "CenterCenter" )
local tp_28_lc						= createTextProperty( 28,  nil,	nil,  "LeftCenter" )
local tp_28_rc						= createTextProperty( 28,  nil,	nil,  "RightCenter" )

local tp_36_cc						= createTextProperty( 36,  nil,	nil,  "CenterCenter" )
local tp_36_lc						= createTextProperty( 36,  nil,	nil,  "LeftCenter" )
local tp_36_rc						= createTextProperty( 36,  nil,	nil,  "RightCenter" )

AddRoundCornersWindow("HoverTorqueStatusWindow",	{((pb_props[pb.T2].pos[1] + pb_props[pb.T3].pos[1]) / 2),((pb_props[pb.L1].pos[2] + pb_props[pb.L2].pos[2]) / 2)},
						tp_default.width*19.0, tp_default.height*9.5,
						{
							{"HOVER Q",				{-tp_default.width*9.0,		tp_default.height*3.75},		tp_28_lc,		nil},
							{"IGE  OGE",			{tp_default.width,			tp_default.height*1.875},		tp_28_lc,		nil},
							{"REQUIRED",			{-tp_default.width*9.0,		0.0},							tp_28_lc,		nil},
							{"GO-NO/GO",			{-tp_default.width*9.0,		-tp_default.height*1.875},		tp_28_lc,		nil},
							{"INDICATED",			{-tp_default.width*9.0,		-tp_default.height*3.75},		tp_28_lc,		nil},
							{"77",					{tp_default.width*4.0,		tp_default.height*0.1},			tp_36_rc,		{{"PERF_HoverQ", 0}}},--REQUIRED_IGE
							{"95",					{tp_default.width*9.0,		tp_default.height*0.1},			tp_36_rc,		{{"PERF_HoverQ", 1}}},--REQUIRED_OGE
							{"100",					{tp_default.width*4.0,		-tp_default.height*1.775},		tp_36_rc,		{{"PERF_HoverQ", 2}}},--GONOGO_IGE
							{"81",					{tp_default.width*9.0,		-tp_default.height*1.775},		tp_36_rc,		{{"PERF_HoverQ", 3}}},--GONOGO_OGE
							
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
						
AddRoundCornersWindow("IndicatedStatusWindow",	{(pb_props[pb.T3].pos[1] + tp_default.width),(pb_props[pb.L2].pos[2] - tp_default.height * 1.5)},
						tp_default.width*5.0, tp_default.height*1.5,
						{
							{"59",			{tp_default.width*1.75,	0.0},		tp_36_rc,					{{"PERF_Hover_Indicated"}}},
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
						
AddRoundCornersWindow("CruiseStatusWindow",		{((pb_props[pb.T5].pos[1] + pb_props[pb.T6].pos[1]) / 2),((pb_props[pb.L1].pos[2] + pb_props[pb.L2].pos[2]) / 2 + tp_default.height * 0.5)},
						tp_default.width*14.0, tp_default.height*8.0,
						{
							{"CRUISE",				{-tp_default.width*6.5,		tp_default.height*3.0},			tp_28_lc,		nil},
							{"RNG   END",			{-tp_default.width*2.5,		tp_default.height*1.0},			tp_28_lc,		nil},
							{"Q",					{-tp_default.width*5.5,		-tp_default.height*1.0},		tp_28_lc,		nil},
							{"FF",					{-tp_default.width*6.5,		-tp_default.height*3.0},		tp_28_lc,		nil},
							{"71",					{tp_default.width*1.0,		-tp_default.height*0.9},		tp_36_rc,		{{"PERF_Cruise_Q_RNG"}}},
							{"47",					{tp_default.width*6.5,		-tp_default.height*0.9},		tp_36_rc,		{{"PERF_Cruise_Q_END"}}},
							{"1050",				{tp_default.width*1.0,		-tp_default.height*2.9},		tp_36_rc,		{{"PERF_Cruise_FF_RNG"}}},
							{"820",					{tp_default.width*6.5,		-tp_default.height*2.9},		tp_36_rc,		{{"PERF_Cruise_FF_END"}}},
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
						
AddRoundCornersWindow("MaximumGrossWeightStatusWindow",	{((pb_props[pb.T2].pos[1] + pb_props[pb.T3].pos[1]) / 2),((pb_props[pb.L3].pos[2] + pb_props[pb.L4].pos[2]) / 2 - tp_default.height * 1.5)},
						tp_default.width*19.0, tp_default.height*8.0,
						{
							{"MAX GWT",				{-tp_default.width*9.0,		tp_default.height*3.0},			tp_28_lc,		nil},
							{"IGE     OGE",			{-tp_default.width*3.5,		tp_default.height*1.0},			tp_28_lc,		nil},
							{"DE",					{-tp_default.width*9.0,		-tp_default.height*1.0},		tp_28_lc,		nil},
							{"SE",					{-tp_default.width*9.0,		-tp_default.height*3.0},		tp_28_lc,		nil},
							{"21250",				{tp_default.width*1.0,		-tp_default.height*0.9},		tp_36_rc,		{{"PERF_MAX_GWT", 0}}},--DE_IGE
							{"18340",				{tp_default.width*8.5,		-tp_default.height*0.9},		tp_36_rc,		{{"PERF_MAX_GWT", 1}}},--DE_OGE
							{"13870",				{tp_default.width*1.0,		-tp_default.height*2.9},		tp_36_rc,		{{"PERF_MAX_GWT", 2}}},--SE_IGE
							{"11960",				{tp_default.width*8.5,		-tp_default.height*2.9},		tp_36_rc,		{{"PERF_MAX_GWT", 3}}},--SE_OGE
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
						
AddRoundCornersWindow("TrueAirspeedStatusWindow",	{((pb_props[pb.T5].pos[1] + pb_props[pb.T6].pos[1]) / 2),((pb_props[pb.L3].pos[2] + pb_props[pb.L4].pos[2]) / 2 - tp_default.height * 1.0)},
						tp_default.width*10.0, tp_default.height*9.5,
						{
							{"TAS",					{-tp_default.width*1.25,	tp_default.height*3.75},		tp_28_rc,		nil},
							{"VNE",					{-tp_default.width*0.25,	tp_default.height*1.875},		tp_28_rc,		nil},
							{"VSSE",				{-tp_default.width*0.25,	0.0},							tp_28_rc,		nil},
							{"RNG",					{-tp_default.width*0.25,	-tp_default.height*1.875},		tp_28_rc,		nil},
							{"END",					{-tp_default.width*0.25,	-tp_default.height*3.75},		tp_28_rc,		nil},
							{"179",					{tp_default.width*4.25,		tp_default.height*1.975},		tp_36_rc,		{{"PERF_TAS", 0}}},--VNE
							{"43",					{tp_default.width*4.25,		tp_default.height*0.1},			tp_36_rc,		{{"PERF_TAS_VSSE"}}},--VSSE
							{"124",					{tp_default.width*4.25,		-tp_default.height*1.675},		tp_36_rc,		{{"PERF_TAS", 1}}},--RNG
							{"75",					{tp_default.width*4.25,		-tp_default.height*3.65},		tp_36_rc,		{{"PERF_TAS", 2}}},--END
							
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
						

AddRoundCornersWindow("MaximumTorqueStatusWindow",	{(pb_props[pb.T1].pos[1]),(pb_props[pb.L6].pos[2] - tp_default.height)},
						tp_default.width*8.0, tp_default.height*6.0,
						{
							{"MAX Q",				{-tp_default.width*3.2,		tp_default.height*2.0},			tp_28_lc,		nil},
							{"DE",					{-tp_default.width*3.2,		0.0},							tp_28_lc,		nil},
							{"SE",					{-tp_default.width*3.2,		-tp_default.height*2.0},		tp_28_lc,		nil},
							{"117",					{tp_default.width*3.5,		0.0},							tp_36_rc,		{{"PERF_MAX_Q_DE"}}},
							{"120",					{tp_default.width*3.5,		-tp_default.height*2.0},		tp_36_rc,		{{"PERF_MAX_Q_SE"}}},
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)	
						
AddRoundCornersWindow("WindStatusWindow",	{((pb_props[pb.T5].pos[1] + pb_props[pb.T6].pos[1]) / 2),(pb_props[pb.L5].pos[2] - tp_default.height)},
						tp_default.width*12.0, tp_default.height*1.5,
						{
							{"WIND",				{-tp_default.width*5.5,		0.0},							tp_28_lc,		nil},
							{"120/15",				{tp_default.width*5.5,		0.0},							tp_28_rc,		{{"PERF_Wind"}}},
							
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	{{"PERF_WindIsDraw"}})

--CenterOfGravityStatus
local line_x1 = pb_props[pb.B2].pos[1]
local line_x2 = pb_props[pb.B6].pos[1] + tp_default.width*4.0
local line_y = pb_props[pb.L6].pos[2] - tp_default.height*4.0
draw_line( {{line_x1,	line_y},{line_x2, line_y}},		tp_default.material,	nil,	5, "CG_Line", {{"PERF_CG_IsDraw"}})

local v_y1	= line_y + tp_default.height*1.4
local v_y2	= line_y + tp_default.height*0.9
local v_y3	= line_y + tp_default.height*0.4
local v1_x1	= line_x1 + tp_default.width
local v1_x2	= v1_x1 + tp_default.width
local v2_x1	= line_x2 - tp_default.width*2.0
local v2_x2	= v2_x1 + tp_default.width
local verts_1 = {{v1_x1, v_y1}, {v1_x2, v_y2}, {v1_x1, v_y3}, {v1_x1, v_y1}}
local verts_2 = {{v2_x1, v_y2}, {v2_x2, v_y1}, {v2_x2, v_y3}, {v2_x1, v_y2}}
draw_line( verts_1, tp_default.material, nil, 3, "CG_Triangle_1", {{"PERF_CG_IsDraw"}})
draw_line( verts_2, tp_default.material, nil, 3, "CG_Triangle_2", {{"PERF_CG_IsDraw"}})

local text_y1 = pb_props[pb.L6].pos[2]
local text_y2 = text_y1 - tp_default.height*1.5
local text_x0 = line_x1 + tp_default.width
local text_x1 = text_x0
local text_x2 = text_x1 + tp_default.width*17.0
local text_x3 = text_x2 + tp_default.width*13.0

addText( "FWD          CG            AFT",	{text_x0, text_y1},		tp_28_lc,	{{"PERF_CG_IsDraw"}},						nil,	nil,	"CG_Label_0",	nil )
addText( "201.0", 							{text_x1, text_y2},		tp_36_lc,	{{"PERF_CG_IsDraw"}},						nil,	nil,	"CG_Label_1",	nil )
addText( "203.1", 							{text_x2, text_y2},		tp_36_rc,	{{"PERF_CG_IsDraw"}, {"PERF_CG_Label"}},	nil,	nil,	"CG_Label_2",	nil )
addText( "207.0", 							{text_x3, text_y2},		tp_36_rc,	{{"PERF_CG_IsDraw"}},						nil,	nil,	"CG_Label_3",	nil )

draw_line( {{0.0,-390.0}, {0.0,-350.0}},		tp_default.material,	nil,	5, "CenterGravity_line", {{"PERF_CG_IsDraw"}, {"PERF_CG_Line"}})