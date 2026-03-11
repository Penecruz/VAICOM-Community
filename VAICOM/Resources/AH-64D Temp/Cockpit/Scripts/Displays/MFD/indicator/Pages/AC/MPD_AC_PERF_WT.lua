dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

--addText( "AC PERF WT PAGE",  {0, 100})

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
	{ pb.B5, "HIT",		nil },
}

createMenu( Menu )

local Controls = {}
Controls = 
{
	{ pb.B1, "PERF",	tp_default_border, {{"MFD_AC_OriginatorFmt"}}, {"ENG", "FLT", "FUEL", "PERF","UTIL"} },
	{ pb.L1, 	{{"AC BASIC WEIGHT>",	nil, {{"PERF_WT_WeightMoment"}}, {"AC BASIC WEIGHT>", "AC BASIC MOMENT>"}},	{"12289",	tp_default_border, {{"PERF_WT_ButtonL1"},	{"MFD_DataEntryButton_frame",pb.L1}} }},		nil,		nil},
	{ pb.L2, 	{{"LEFT AFT BAY>",		nil },	{"80",	tp_default_border, {{"PERF_WT_Button", 0},	{"MFD_DataEntryButton_frame",pb.L2}} }},		nil,		nil},
	{ pb.L3, 	{{"SURVIVAL KIT>",		nil },	{"50",	tp_default_border, {{"PERF_WT_Button", 1},	{"MFD_DataEntryButton_frame",pb.L3}} }},		nil,		nil},
	{ pb.L4, 	{{"PILOT>",				nil },	{"210",	tp_default_border, {{"PERF_WT_Button", 2},	{"MFD_DataEntryButton_frame",pb.L4}} }},		nil,		nil},
	{ pb.L5, 	{{"CPG>",				nil },	{"225",	tp_default_border, {{"PERF_WT_Button", 3},	{"MFD_DataEntryButton_frame",pb.L5}} }},		nil,		nil},

	{ pb.R1, 	{{"DUMMY MISSILES>",	nil },	{"4",	tp_default_border, {{"PERF_WT_Button", 4},	{"MFD_DataEntryButton_frame",pb.R1}} }},		nil,		nil},
	{ pb.R2, 	{{"DUMMY ROCKETS>",		nil },	{"0",	tp_default_border, {{"PERF_WT_Button", 5},	{"MFD_DataEntryButton_frame",pb.R2}} }},		nil,		nil},

	{ "PERF MODE", 
				{ 
					{ pb.B2, 	"CUR",		tp_default_border,		{{"PERF_MODE_Border", 0}}},
					{ pb.B3, 	"MAX",		tp_default_border,		{{"PERF_MODE_Border", 1}}},
					{ pb.B4, 	"PLAN",		tp_default_border,		{{"PERF_MODE_Border", 2}}},
				}
	},
	
	{ pb.B6, 	"WT",		tp_default_border,		nil},
}

createControls( Controls )