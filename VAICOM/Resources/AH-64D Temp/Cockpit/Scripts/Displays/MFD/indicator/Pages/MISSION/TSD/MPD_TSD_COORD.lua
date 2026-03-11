dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/Common/MPD_TSD_PointsId.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T1, "WPTHZ",		tp_default_border,	{{"TSD_COORD_PB_Border",1}}},
	{ pb.T2, "CTRLM",		tp_default_border,	{{"TSD_COORD_PB_Border",2}}},
	{ pb.T3, "LINE",		nil,				nil},
	{ pb.T4, "AREA",		nil,				nil},
	{ pb.T5, "COORD",		tp_default_border,	{{"TSD_COORD_PB_Border",0}}},
	{ pb.T6, "SHOT",		nil,				nil},
	
	{ pb.B5, "FARM",		nil,				nil}
}

local Controls = {}
Controls = 
{	
	{ pb.B1, "TSD",		nil,	{{"TSD_COORD_originPage"}}, {"TSD", "WPN"}},
	{ pb.B4, { {"SRCH>", nil, nil}, {"?", tp_default_border, {{"MFD_DataEntryButton_frame",pb.B4}}} } },
	
	{ pb.L1, "",		nil,	{{"TSD_COORD_InfoLine_Caption", 1}}, Point_Types},
	{ pb.L2, "",		nil,	{{"TSD_COORD_InfoLine_Caption", 2}}, Point_Types},
	{ pb.L3, "",		nil,	{{"TSD_COORD_InfoLine_Caption", 3}}, Point_Types},
	{ pb.L4, "",		nil,	{{"TSD_COORD_InfoLine_Caption", 4}}, Point_Types},
	{ pb.L5, "",		nil,	{{"TSD_COORD_InfoLine_Caption", 5}}, Point_Types},
	{ pb.L6, "",		nil,	{{"TSD_COORD_InfoLine_Caption", 6}}, Point_Types},
	
	{ pb.R1, "",		nil,	nil},
	{ pb.R2, "",		nil,	nil},
	{ pb.R3, "",		nil,	nil},
	{ pb.R4, "",		nil,	nil},
	{ pb.R5, "",		nil,	nil},
	{ pb.R6, "",		nil,	nil}
}

local function AddCoordString(num)

	local tp_coord = createTextProperty( nil, "GREEN",IND_MPD_MATERIAL_GREEN, "LeftTop" )
	local tp_coord_r = createTextProperty( nil, "GREEN",IND_MPD_MATERIAL_GREEN, "RightTop" )
	tp_coord.stringdefs = {tp_coord.height*GetScale(),tp_coord.height*GetScale(),tp_coord.height*GetScale()*0.03}	-- to set symbols looser
	tp_coord_r.stringdefs = {tp_coord_r.height*GetScale(),tp_coord_r.height*GetScale(),tp_coord_r.height*GetScale()*0.03}	-- to set symbols looser

	local pos_y		= pb_props[pb.L1-(num-1)].pos[2]
	local pos_x2	= pb_props[pb.L1].pos[1] + tp_coord.width*5.0	--Identifier
	local pos_x3	= pos_x2 + tp_coord.width*4.0					--Free Text
	local pos_x4	= pos_x3 + tp_coord.width*5.0					--Spheroid
	local pos_x5	= pos_x4 + tp_coord.width*5.0					--Datum
	local pos_x6	= pos_x5 + tp_coord.width*4.0					--UTM Coordinates
	local pos_x7	= pos_x6 + tp_coord.width*27.0					--MSL Elevation

	local InfoLineBase = addPlaceholder("InfoLine_"..num.."_plaseholder", {0.0,0.0}, nil, {{"TSD_COORD_InfoLine_Base",num}})
	
	local PBnum = pb.R1+num-1
	local arrow_pos = {pb_props[PBnum].pos[1], pb_props[PBnum].pos[2]-tp_default.height/2}
	--				 																					 controller, material
	AddSmallArrowMenuLabel("R"..num.."_Arrow_small", arrow_pos, tp_default.width, 90, InfoLineBase.name, nil, IND_MPD_MATERIAL_GREEN)


	-- Identifier
	elem = addText( "AA",						{pos_x2, pos_y},	tp_coord,	{{"TSD_COORD_InfoLine_Identifier",num}},	WP_HZ_CM_TG_Identifier, nil, nil, InfoLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
	-- Free Text
	elem = addText( "AAA",						{pos_x3, pos_y},	tp_coord,	{{"TSD_COORD_InfoLine_FreeText",num}},		nil, 					nil, nil, InfoLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
	-- Spheroid
	elem = addText( "AAA",						{pos_x4, pos_y},	tp_coord,	{{"TSD_POINT_StatusWindow_Spheroid",num}},	Spheroid, 				nil, nil, InfoLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
	-- Datum
	elem = addText( "XX",						{pos_x5, pos_y},	tp_coord,	{{"TSD_POINT_StatusWindow_Datum",num}},		nil, 					nil, nil, InfoLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
	-- UTM Coordinates
	elem = addText( "AAX AA XXXX XXXX",			{pos_x6, pos_y},	tp_coord,	{{"TSD_COORD_InfoLine_UTMcoords",num}},		nil, 					nil, nil, InfoLineBase.name )
	elem.stringdefs = tp_coord.stringdefs	
	
	-- MSL Elevation
	elem = addText( "XXXXX FT",					{pos_x7, pos_y},	tp_coord_r,	{{"TSD_COORD_InfoLine_MSL",num}},			nil, 					nil, nil, InfoLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
end


local function AddExpandedCoordStrings(num)

	local tp_coord = createTextProperty( nil, "GREEN",IND_MPD_MATERIAL_GREEN, "LeftTop" )
	local tp_coord_r = createTextProperty( nil, "GREEN",IND_MPD_MATERIAL_GREEN, "RightTop" )
	tp_coord.stringdefs = {tp_coord.height*GetScale(),tp_coord.height*GetScale(),tp_coord.height*GetScale()*0.03}	-- to set symbols looser
	tp_coord_r.stringdefs = {tp_coord_r.height*GetScale(),tp_coord_r.height*GetScale(),tp_coord_r.height*GetScale()*0.03}	-- to set symbols looser

	local pos_y0	= pb_props[pb.L1-(num-1)].pos[2] + tp_coord.height*1.4
	local pos_y1	= pos_y0 - tp_coord.height*1.4
	local pos_y2	= pos_y1 - tp_coord.height*1.4
	
	local pos_x0	= pb_props[pb.L1].pos[1] + tp_coord.width*5.0
	local pos_x1	= pos_x0 + tp_coord.width*0.5                --Type And Number
	local pos_x2	= pos_x1 + tp_coord.width*4.0                --Identifier
	local pos_x3	= pos_x2 + tp_coord.width*3.0                --Free Text
	local pos_x4	= pos_x3 + tp_coord.width*7.0                --Estimated Time Enroute
	local pos_x5	= pos_x4 + tp_coord.width*15.0               --Estimated Time of Arrival
	local pos_x6	= pos_x1                                     --Spheroid
	local pos_x7	= pos_x6 + tp_coord.width*4.0                --Datum
	local pos_x8	= pos_x7 + tp_coord.width*5.0                --UTM Coordinates
	local pos_x9	= pos_x8 + tp_coord.width*24.0               --Bearing
	local pos_x10	= pos_x9 + tp_coord.width*10.0               --Distance to go (KM)
	local pos_x11	= pos_x1                                     --Latitude Coordinates
	local pos_x12	= pos_x11 + tp_coord.width*11.0              --Longitude Coordinates
	local pos_x13	= pos_x12 + tp_coord.width*21.0              --MSL Elevation
	local pos_x14	= pos_x13 + tp_coord.width*11.0              --Distance to go (NM)

	local ExpandedLineBase = addPlaceholder("ExpandedLine_"..num.."_plaseholder", {0.0,0.0}, nil, {{"TSD_COORD_InfoLine_Selected",num}})

	local PBnum = pb.R1+num-1
	local arrow_pos = {pb_props[PBnum].pos[1], pb_props[PBnum].pos[2]-tp_default.height/2}
	AddSmallArrowMenuLabel("R"..num.."_Arrow_small_", arrow_pos, tp_default.width, 90, ExpandedLineBase.name, nil, IND_MPD_MATERIAL_GREEN)

	local v_x1_box	= pb_props[PBnum].pos[1]-tp_default.width
	local v_x2_box	= pb_props[PBnum].pos[1]+tp_default.width
	local v_y1_box	= pb_props[PBnum].pos[2]-tp_default.height
	local v_y2_box	= pb_props[PBnum].pos[2]
	local verts_box = {{v_x1_box, v_y1_box}, {v_x1_box, v_y2_box}, {v_x2_box, v_y2_box}, {v_x2_box, v_y1_box}, {v_x1_box, v_y1_box}}
	draw_line( verts_box, tp_default.material, ExpandedLineBase.name, 3,  nil, nil)
	
	--
	local v_y1	= pos_y0 + tp_coord.height*0.3
	local v_y2	= pos_y2 - tp_coord.height*1.3
	local v_x1	= pos_x0
	local v_x2	= pos_x0 + tp_coord.width*44.0
	local verts = {{v_x1, v_y1}, {v_x2, v_y1}, {v_x2, v_y2}, {v_x1, v_y2}, {v_x1, v_y1}}
	draw_line( verts, IND_MPD_MATERIAL_GREEN, ExpandedLineBase.name, 3, nil, nil)
	
	-- Type And Number	
	local elem = addText( "T01",				{pos_x1, pos_y0},	tp_coord,	{{"TSD_POINT_RTE_StatusWindow_Point"}},			Point_Types, 			nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs

	-- Identifier
	elem = addText( "AA",						{pos_x2, pos_y0},	tp_coord,	{{"TSD_POINT_RTE_StatusWindow_Identifier"}},	WP_HZ_CM_TG_Identifier, nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
	-- Free Text
	elem = addText( "AAA",						{pos_x3, pos_y0},	tp_coord,	{{"TSD_POINT_RTE_StatusWindow_Text"}},			nil, 					nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs	
	
	-- Estimated Time Enroute	
	local elem = addText( "ETE 00:00:00",		{pos_x4, pos_y0},	tp_coord,	{{"TSD_POINT_RTE_StatusWindow_ETE"}},			nil,					nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs

	-- Estimated Time of Arrival
	elem = addText( "ETA 00:00:00 L",			{pos_x5, pos_y0},	tp_coord,	{{"TSD_POINT_RTE_StatusWindow_ETA"}},			nil,					nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
	-- Spheroid
	elem = addText( "AAA",						{pos_x6, pos_y1},	tp_coord,	{{"TSD_POINT_StatusWindow_Spheroid"}},			Spheroid, 				nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
	-- Datum
	elem = addText( "XX",						{pos_x7, pos_y1},	tp_coord,	{{"TSD_POINT_StatusWindow_Datum"}},				nil, 					nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
	-- UTM Coordinates
	elem = addText( "AAX AA XXXX XXXX",			{pos_x8, pos_y1},	tp_coord,	{{"TSD_POINT_StatusWindow_UTM"}},				nil, 					nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs	
	
	-- Bearing
	elem = addText( "XXX",						{pos_x9, pos_y1},	tp_coord_r,	{{"TSD_POINT_StatusWindow_Bearing"}},			nil,					nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
	-- Distance to go (KM)
	elem = addText( "   0.0 KM",				{pos_x10, pos_y1},	tp_coord_r,	{{"TSD_POINT_RTE_StatusWindow_Distance",0}},	nil,					nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
	-- Latitude coordinates
	elem = addText( "AXX XX.XX",				{pos_x11, pos_y2},	tp_coord,	{{"TSD_POINT_StatusWindow_Latitude"}},			nil,					nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
	-- Longitude Coordinates
	elem = addText( "AXXX XX.XX",				{pos_x12, pos_y2},	tp_coord,	{{"TSD_POINT_StatusWindow_Longitude"}},			nil,					nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs	
	
	-- MSL Elevation
	elem = addText( "XXXXX FT",					{pos_x13, pos_y2},	tp_coord_r,	{{"TSD_POINT_StatusWindow_Altitude"}},			nil, 					nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
	-- Distance to go (NM)
	elem = addText( "   0.0 NM",				{pos_x14, pos_y2},	tp_coord_r,	{{"TSD_POINT_RTE_StatusWindow_Distance",1}},	nil,					nil,	nil,	ExpandedLineBase.name )
	elem.stringdefs = tp_coord.stringdefs
	
end


-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------


local text	= "TARGETS AND THREATS"
AddRoundCornersWindow("PhaseLbl_TT", {0, pb_props[pb.L1].pos[2]+tp_default.height*3.2}, nil, nil, text, nil, nil, nil, {{"TSD_COORD_Mode_Selection",0}})

text	= "WAYPOINTS AND HAZARDS"
AddRoundCornersWindow("PhaseLbl_WP", {0, pb_props[pb.L1].pos[2]+tp_default.height*3.2}, nil, nil, text, nil, nil, nil, {{"TSD_COORD_Mode_Selection",1}})

text	= "CONTROL MEASURES"
AddRoundCornersWindow("PhaseLbl_CM", {0, pb_props[pb.L1].pos[2]+tp_default.height*3.2}, nil, nil, text, nil, nil, nil, {{"TSD_COORD_Mode_Selection",2}})


for i=1,6 do
	AddCoordString(i)
	AddExpandedCoordStrings(i)
end


-- draw menus

createMenu( Menu )
createControls( Controls )
AddPagingGroup("COORDS_B2B3", {{"TSD_COORD_Pages_Caption"}})
-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------