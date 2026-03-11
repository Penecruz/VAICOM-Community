dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T1, "WPTHZ",		nil,					nil},
	{ pb.T2, "CTRLM",		nil,					nil},
	{ pb.T3, "LINE",		nil,					nil},
	{ pb.T4, "AREA",		nil,					nil},
	{ pb.T5, "COORD",		nil,					nil},
	{ pb.T6, "SHOT",		tp_default_border,		nil},

	{ pb.B5, "FARM",		nil,					nil}
}

local Controls = {}
Controls = 
{	
	{ pb.L3, "DEL",		nil,	{{"TSD_SHOT_DEL_Menu_Show",0}}},
	
	{ pb.B1, "TSD",		nil,	{{"TSD_COORD_originPage"}}, {"TSD", "WPN"}},
}

local DeleteControls = {}
DeleteControls = 
{
	{ pb.L2, "YES",	nil, nil},
	{ pb.L3, "NO",	nil, nil},
}

--tp_default alignment is "CenterTop"
local pos_x1		= pb_props[pb.T2].pos[1] - tp_default.width*0.0			-- File number
local pos_x2		= pos_x1 + tp_default.width*3.5							-- Icon
local pos_x3		= pos_x2 + tp_default.width*4.0							-- Missile type
local pos_x4		= pos_x3 + tp_default.width*7.5							-- Time
local pos_x5		= pos_x4 + tp_default.width*7.5							-- OWN/IDM
local pos_x6		= pos_x1 + tp_default.width*3.5							-- Spheroid
local pos_x7		= pos_x6 + tp_default.width*4.1							-- Datum
local pos_x8		= pos_x7 + tp_default.width*11.0						-- UTM Coordinates

local pos_y1		= pb_props[pb.L1].pos[2] - tp_default.height*0.0
local pos_y_data	= tp_default.height*0.65
local pos_y_utm		= -tp_default.height*0.65
local stepY 		= (pos_y1 - pb_props[pb.L6].pos[2]) / 5

local function AddBDAString(num)
	local elem
	local pos_y		= pos_y1 - stepY*num

	local InfoLineBase = addPlaceholder("BDA_InfoLine_"..num.."_plaseholder", {0.0, pos_y}, nil, {{"TSD_SHOT_ReportsLinesVisibility",num}})

	-- File number
	elem = addText( "XX",					{pos_x1, pos_y_data},	tp_default,	{{"TSD_SHOT_InfoLine_FileNumber",num}},	nil, nil, nil, InfoLineBase.name )
	-- Icon
	addMapTacticalSymbol("BDA_InfoLine_"..num.."FcrTargetSymbol", fontPrefix.."FCR_Target_Symbol", {{"TSD_SHOT_InfoLine_Icon", num}}, InfoLineBase.name, {pos_x2, pos_y_data - tp_default.height*0.4}, 50, "h" )
	-- Missile type
	elem = addText( "SAL",					{pos_x3, pos_y_data},	tp_default,	{{"TSD_SHOT_InfoLine_MslType",num}},	{"SAL","RF"}, nil, nil, InfoLineBase.name )
	-- Time
	elem = addText( "00:00:00Z",			{pos_x4, pos_y_data},	tp_default,	{{"TSD_SHOT_InfoLine_Time",num}},		nil, nil, nil, InfoLineBase.name )
	-- OWN/IDM
	elem = addText( "OWN",					{pos_x5, pos_y_data},	tp_default,	{{"TSD_SHOT_InfoLine_Source",num}},		{"OWN","DL"}, nil, nil, InfoLineBase.name )
	
	-- Spheroid
	elem = addText( "AAA",					{pos_x6, pos_y_utm},	tp_default,	{{"TSD_SHOT_InfoLine_Spheroid",num}},	Spheroid, nil, nil, InfoLineBase.name )
	-- Datum
	elem = addText( "XX",					{pos_x7, pos_y_utm},	tp_default,	{{"TSD_SHOT_InfoLine_Datum",num}},		nil, nil, nil, InfoLineBase.name )
	-- UTM Coordinates
	elem = addText( "AAX AA XXXX XXXX",		{pos_x8, pos_y_utm},	tp_default,	{{"TSD_SHOT_InfoLine_UTM",num}},		nil, nil, nil, InfoLineBase.name )
end

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

local text	= "SHOT AT"
AddRoundCornersWindow("SHOTCaptionText", {0, pb_props[pb.L1].pos[2]+tp_default.height*3.2}, nil, nil, text, nil, nil, nil, nil)


for i=0,5 do
	AddBDAString(i)
end

local DeleteFrameBase = addPlaceholder("DeleteFrameBase_PH", {0, 0}, nil, {{"TSD_SHOT_DEL_Menu_Show",1}})
draw_border_with_caption( pb_props[pb.L2].pos, pb_props[pb.L3].pos,  4, 1, "DELETE", pb_props[pb.L3].tp, DeleteFrameBase.name )
createControls( DeleteControls,  nil, DeleteFrameBase.name, 0)

-- draw menus
createMenu( Menu )
createControls( Controls )

AddPagingGroup("SHOT_B2B3", {{"TSD_SHOT_Pages_Caption"}})
-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

