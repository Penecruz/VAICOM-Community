dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local FARM_REPORTS_TYPES =
{
	BASIC		= 0,
	MSL			= 1,
	EXPEN		= 2,
	PP			= 3,
}

local Menu = {}
Menu = 
{ 
	{ pb.T1, "WPTHZ",		nil,					nil},
	{ pb.T2, "CTRLM",		nil,					nil},
	{ pb.T3, "LINE",		nil,					nil},
	{ pb.T4, "AREA",		nil,					nil},
	{ pb.T5, "COORD",		nil,					nil},
	{ pb.T6, "SHOT",		nil,					nil},

	{ pb.B5, "FARM",		tp_default_border,		nil}
}

local Controls = {}
Controls = 
{	
	{ pb.B1, "TSD",		nil,	{{"TSD_COORD_originPage"}}, {"TSD", "WPN"}},
}

--tp_default alignment is "CenterTop"
local pos_x1	= pb_props[pb.L1].pos[1] + tp_default.width*2.5			-- C/S
local pos_x2	= pos_x1 + tp_default.width*9.0							-- TIME
local pos_x3	= pos_x2 + tp_default.width*9.0							-- column 3
local pos_x4	= pos_x3 + tp_default.width*7.0							-- column 4
local pos_x5	= pos_x4 + tp_default.width*7.0							-- column 5
local pos_x6	= pos_x5 + tp_default.width*7.0							-- column 6
local pos_x7	= pos_x6 + tp_default.width*7.0							-- column 7

local pos_y_caption	= pb_props[pb.L1].pos[2] + tp_default.height*1.0
local pos_y1		= pb_props[pb.L1].pos[2] - tp_default.height*1.5
local stepY 		= (pos_y1 - pb_props[pb.L6].pos[2]) / 6

local PH_Basic 	= addPlaceholder("FARMReports_BasePH_"..FARM_REPORTS_TYPES.BASIC,	{0.0,0.0}, nil, {{"TSD_FARM_ReportsVisibilityByTYPE",FARM_REPORTS_TYPES.BASIC}})
local PH_Msl 	= addPlaceholder("FARMReports_BasePH_"..FARM_REPORTS_TYPES.MSL,		{0.0,0.0}, nil, {{"TSD_FARM_ReportsVisibilityByTYPE",FARM_REPORTS_TYPES.MSL}})
local PH_Expen 	= addPlaceholder("FARMReports_BasePH_"..FARM_REPORTS_TYPES.EXPEN,	{0.0,0.0}, nil, {{"TSD_FARM_ReportsVisibilityByTYPE",FARM_REPORTS_TYPES.EXPEN}})
local PH_PP 	= addPlaceholder("FARMReports_BasePH_"..FARM_REPORTS_TYPES.PP,		{0.0,0.0}, nil, {{"TSD_FARM_ReportsVisibilityByTYPE",FARM_REPORTS_TYPES.PP}})

local function AddFARMString_Basic(num)
	local elem
	local pos_y		= pos_y1 - stepY*num

	local InfoLineBase = addPlaceholder("FARM_BASIC_InfoLine_"..num.."_plaseholder", {0.0, pos_y}, PH_Basic.name, {{"TSD_FARM_ReportsLinesVisibility",num+1}})

	-- C/S
	elem = addText( "AAAAA",		{pos_x1, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_CS",num}},		nil, nil, nil, InfoLineBase.name )
	-- TIME
	elem = addText( "00:00:00L",	{pos_x2, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_Time",num}},	nil, nil, nil, InfoLineBase.name )
	-- FUEL
	elem = addText( "XXXX",			{pos_x3, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_Fuel",num}},	nil, nil, nil, InfoLineBase.name )
	-- GUN
	elem = addText( "XXXX",			{pos_x4, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_Gun",num}},	nil, nil, nil, InfoLineBase.name )
	-- RKT
	elem = addText( "NA",			{pos_x5, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_Rkt",num}},	nil, nil, nil, InfoLineBase.name )
	-- RF
	elem = addText( "NA",			{pos_x6, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_RF",num}},		nil, nil, nil, InfoLineBase.name )
	-- SAL
	elem = addText( "NA",			{pos_x7, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_SAL2",num}},	nil, nil, nil, InfoLineBase.name )
end

local function AddFARMString_Msl(num)
	local elem
	local pos_y		= pos_y1 - stepY*num

	local InfoLineBase = addPlaceholder("FARM_MSL_InfoLine_"..num.."_plaseholder", {0.0, pos_y}, PH_Msl.name, {{"TSD_FARM_ReportsLinesVisibility",num+1}})

	-- C/S
	elem = addText( "AAAAA",		{pos_x1, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_CS",num}},			nil, nil, nil, InfoLineBase.name )
	-- TIME
	elem = addText( "00:00:00L",	{pos_x2, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_Time",num}},		nil, nil, nil, InfoLineBase.name )
	-- TOT
	elem = addText( "NA",			{pos_x3, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_MslTotal",num}},	nil, nil, nil, InfoLineBase.name )
	-- RF
	elem = addText( "NA",			{pos_x4, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_RF",num}},			nil, nil, nil, InfoLineBase.name )
	-- SAL1
	elem = addText( "NA",			{pos_x5, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_SAL1",num}},		nil, nil, nil, InfoLineBase.name )
	-- SAL2
	elem = addText( "NA",			{pos_x6, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_SAL2",num}},		nil, nil, nil, InfoLineBase.name )
	-- OTHER
	elem = addText( "NA",			{pos_x7, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_MslOther",num}},	nil, nil, nil, InfoLineBase.name )
end

local function AddFARMString_Expen(num)
	local elem
	local pos_y		= pos_y1 - stepY*num

	local InfoLineBase = addPlaceholder("FARM_EXPEN_InfoLine_"..num.."_plaseholder", {0.0, pos_y}, PH_Expen.name, {{"TSD_FARM_ReportsLinesVisibility",num+1}})

	-- C/S
	elem = addText( "AAAAA",		{pos_x1, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_CS",num}},		nil, nil, nil, InfoLineBase.name )
	-- TIME
	elem = addText( "00:00:00L",	{pos_x2, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_Time",num}},	nil, nil, nil, InfoLineBase.name )
	-- FLARE
	elem = addText( "NA",			{pos_x3, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_Flare",num}},	nil, nil, nil, InfoLineBase.name )
	-- CHAFF
	elem = addText( "NA",			{pos_x4, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_Chaff",num}},	nil, nil, nil, InfoLineBase.name )
	-- OTHER
	elem = addText( "0",			{pos_x5, 0.0},	tp_default,	nil,								nil, nil, nil, InfoLineBase.name )	
end

local function AddFARMString_PP(num)
	local elem
	local pos_y		= pos_y1 - stepY*num
	local pos_y_utm	= tp_default.height*0.65
	local pos_y_ll	= -tp_default.height*0.65

	local InfoLineBase = addPlaceholder("FARM_PP_InfoLine_"..num.."_plaseholder", {0.0, pos_y}, PH_PP.name, {{"TSD_FARM_ReportsLinesVisibility",num+1}})

	-- C/S
	elem = addText( "AAAAA",					{pos_x1, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_CS",num}},	nil, nil, nil, InfoLineBase.name )
	-- TIME
	elem = addText( "00:00:00L",				{pos_x2, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_Time",num}},	nil, nil, nil, InfoLineBase.name )
	
	-- Spheroid
	elem = addText( "AAA",						{pos_x3-tp_default.width*0.5, pos_y_utm},	tp_default,	{{"TSD_FARM_InfoLine_Spheroid",num}},	Spheroid, nil, nil, InfoLineBase.name )
	-- Datum
	elem = addText( "XX",						{pos_x4-tp_default.width*3.9, pos_y_utm},	tp_default,	{{"TSD_FARM_InfoLine_Datum",num}},	nil, nil, nil, InfoLineBase.name )
	-- UTM Coordinates
	elem = addText( "AAX AA XXXX XXXX",			{pos_x5-tp_default.width*0.7, pos_y_utm},	tp_default,	{{"TSD_FARM_InfoLine_UTM",num}},	nil, nil, nil, InfoLineBase.name )
	
	-- LAT/LON Coordinates
	elem = addText( "NXX XX.XX EXXX XX.XX",		{pos_x5-tp_default.width*4.0, pos_y_ll},	tp_default,	{{"TSD_FARM_InfoLine_LatLon",num}},	nil, nil, nil, InfoLineBase.name )
	
	-- MSL Elevation
	elem = addText( "YYYYYFT",					{pos_x6+tp_default.width*6.0, 0.0},	tp_default,	{{"TSD_FARM_InfoLine_Elevation",num}},	nil, nil, nil, InfoLineBase.name )
end

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

local text	= "FUEL/AMMUNITION/ROCKETS/MISSILES REPORTS"
AddRoundCornersWindow("FARMCaptionText", {0, pb_props[pb.L1].pos[2]+tp_default.height*3.2}, nil, nil, text, nil, nil, nil, nil)

-- COLUMNS CAPTIONS
local PH_Basic_Caption 			= addPlaceholder("FARMReports_BasePH_"..FARM_REPORTS_TYPES.BASIC.."_Caption",	{0.0, pos_y_caption},	PH_Basic.name, nil)
addTextUnderlined("C/S",			{pos_x1, 0.0},	tp_default, nil,	nil, nil, "FARM_Basic_Caption_Col1", PH_Basic_Caption.name )
addTextUnderlined("TIME",			{pos_x2, 0.0},	tp_default, nil,	nil, nil, "FARM_Basic_Caption_Col2", PH_Basic_Caption.name )
addTextUnderlined("FUEL",			{pos_x3, 0.0},	tp_default, nil,	nil, nil, "FARM_Basic_Caption_Col3", PH_Basic_Caption.name )
addTextUnderlined("GUN",			{pos_x4, 0.0},	tp_default, nil,	nil, nil, "FARM_Basic_Caption_Col4", PH_Basic_Caption.name )
addTextUnderlined("RKT",			{pos_x5, 0.0},	tp_default, nil,	nil, nil, "FARM_Basic_Caption_Col5", PH_Basic_Caption.name )
addTextUnderlined("RF",				{pos_x6, 0.0},	tp_default, nil,	nil, nil, "FARM_Basic_Caption_Col6", PH_Basic_Caption.name )
addTextUnderlined("SAL",			{pos_x7, 0.0},	tp_default, nil,	nil, nil, "FARM_Basic_Caption_Col7", PH_Basic_Caption.name )

local PH_MSL_Caption 			= addPlaceholder("FARMReports_BasePH_"..FARM_REPORTS_TYPES.MSL.."_Caption",	{0.0, pos_y_caption},	PH_Msl.name, nil)
addTextUnderlined("C/S",			{pos_x1, 0.0},	tp_default, nil,	nil, nil, "FARM_Msl_Caption_Col1", PH_MSL_Caption.name )
addTextUnderlined("TIME",			{pos_x2, 0.0},	tp_default, nil,	nil, nil, "FARM_Msl_Caption_Col2", PH_MSL_Caption.name )
addTextUnderlined("TOT",			{pos_x3, 0.0},	tp_default, nil,	nil, nil, "FARM_Msl_Caption_Col3", PH_MSL_Caption.name )
addTextUnderlined("RF",				{pos_x4, 0.0},	tp_default, nil,	nil, nil, "FARM_Msl_Caption_Col4", PH_MSL_Caption.name )
addTextUnderlined("SAL1",			{pos_x5, 0.0},	tp_default, nil,	nil, nil, "FARM_Msl_Caption_Col5", PH_MSL_Caption.name )
addTextUnderlined("SAL2",			{pos_x6, 0.0},	tp_default, nil,	nil, nil, "FARM_Msl_Caption_Col6", PH_MSL_Caption.name )
addTextUnderlined("OTHER",			{pos_x7, 0.0},	tp_default, nil,	nil, nil, "FARM_Msl_Caption_Col7", PH_MSL_Caption.name )

local PH_EXPEN_Caption 			= addPlaceholder("FARMReports_BasePH_"..FARM_REPORTS_TYPES.EXPEN.."_Caption",	{0.0, pos_y_caption},	PH_Expen.name, nil)
addTextUnderlined("C/S",			{pos_x1, 0.0},	tp_default, nil,	nil, nil, "FARM_Expen_Caption_Col1", PH_EXPEN_Caption.name )
addTextUnderlined("TIME",			{pos_x2, 0.0},	tp_default, nil,	nil, nil, "FARM_Expen_Caption_Col2", PH_EXPEN_Caption.name )
addTextUnderlined("FLARE",			{pos_x3, 0.0},	tp_default, nil,	nil, nil, "FARM_Expen_Caption_Col3", PH_EXPEN_Caption.name )
addTextUnderlined("CHAFF",			{pos_x4, 0.0},	tp_default, nil,	nil, nil, "FARM_Expen_Caption_Col4", PH_EXPEN_Caption.name )
addTextUnderlined("OTHER",			{pos_x5, 0.0},	tp_default, nil,	nil, nil, "FARM_Expen_Caption_Col5", PH_EXPEN_Caption.name )

local PH_PP_Caption 			= addPlaceholder("FARMReports_BasePH_"..FARM_REPORTS_TYPES.PP.."_Caption",	{0.0, pos_y_caption},	PH_PP.name, nil)
addTextUnderlined("C/S",				{pos_x1, 0.0},	tp_default, nil,	nil, nil, "FARM_PP_Caption_Col1", PH_PP_Caption.name )
addTextUnderlined("TIME",				{pos_x2, 0.0},	tp_default, nil,	nil, nil, "FARM_PP_Caption_Col2", PH_PP_Caption.name )
addTextUnderlined("PRESENT POSITION",	{pos_x4+tp_default.width*3.0, 0.0},	tp_default, nil,	nil, nil, "FARM_PP_Caption_Col3", PH_PP_Caption.name )
addTextUnderlined("ALT",				{pos_x6+tp_default.width*6.0, 0.0},	tp_default, nil,	nil, nil, "FARM_PP_Caption_Col4", PH_PP_Caption.name )


for i=0,6 do
	AddFARMString_Basic(i)
	AddFARMString_Msl(i)
	AddFARMString_Expen(i)
	AddFARMString_PP(i)
end

-- draw menus
createMenu( Menu )
createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------