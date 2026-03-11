dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")
dofile(LockOn_Options.script_path.."Displays/Common/MPD_TSD_PointsId.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T1, "RPT",		nil,	nil},

	{ pb.T2, "PAN",		tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.PAN}}},
	{ pb.T2, "PAN",		nil,	nil},

	{ pb.T3, "SHOW",	nil,	nil},
	{ pb.T5, "COORD",	nil,	nil},
	{ pb.T6, "UTIL",	nil,	nil},

	{ pb.B3, "BAM",		nil,	nil},
	{ pb.B4, "MAP",		nil,	nil},
	{ pb.B5, "RTE",		tp_default_border,	nil},
	{ pb.B6, "RTM",	nil,	nil}
}

local Controls = {}
Controls = 
{
	{ pb.L1, { {"POINT>", nil, nil}, {"W00", tp_default_border, {{"TSD_RTE_PointButtonName"},{"MFD_DataEntryButton_frame",pb.L1}}, Point_Types} } },	
	{ pb.L2, "ADD",		tp_default_border,	{{"TSD_RTE_Buttons", 1}},},
	{ pb.L4, "DEL",		tp_default_border,	{{"TSD_RTE_Buttons", 2}},},
	{ pb.L5, "DIR",		tp_default_border,	{{"TSD_RTE_Buttons", 3}},},

	{ pb.B1, "TSD",		nil,	nil},
	{ pb.B2, { {"PHASE", nil, nil}, {"NAV", tp_default_border, {{"TSD_PHASE_Btn_Caption"}}, {"NAV", "ATK"}} } },
    
	{ "ROUTE", 
				{ 
					{ pb.R2, "TR66",	tp_default_border, {{"TSD_RTE_RightButtonsTypeAndNumber", 3}}, WP_HZ_CM_Types	}, 
					{ pb.R3, "CP60",	tp_default_border, {{"TSD_RTE_RightButtonsTypeAndNumber", 2}}, WP_HZ_CM_Types	}, 
					{ pb.R4, "WP08",	tp_default_border, {{"TSD_RTE_RightButtonsTypeAndNumber", 1}}, WP_HZ_CM_Types	}, 
					{ pb.R5, "WP07",	tp_default_border, {{"TSD_RTE_RightButtonsTypeAndNumber", 0}}, WP_HZ_CM_Types	},
				},  
		{{"TSD_RTE_CurrentRouteName"}} 
	},
}

local function UnderlineLabel(num)
	draw_line( {{pb_props[pb.R5 - num].pos_down[1]-tp_default.width*3.8, pb_props[pb.R5 - num].pos_down[2]-tp_default.height*0.2},	
				{pb_props[pb.R5 - num].pos_down[1]+tp_default.width*0.25, pb_props[pb.R5 - num].pos_down[2]-tp_default.height*0.2}},		
				tp_default.material, nil, 3, "Underline_"..num, {{"TSD_RTE_PointUnderline", num}})
end	

local pos_shift_x = 28
local t3_pocket,t4_pocket,b3_pocket,b4_pocket = pb_props[pb.T3].pos,pb_props[pb.T4].pos,pb_props[pb.B3].pos,pb_props[pb.B4].pos

pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - pos_shift_x*1.2
pb_props[pb.B3].pos[1] = pb_props[pb.B3].pos[1] - pos_shift_x
pb_props[pb.B4].pos[1] = pb_props[pb.B4].pos[1] + pos_shift_x

-----------------------------------------------------------

local function AddActiveWaypointStatusWindow()
	local smallfont_size	= 28
	local tp28				= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftCenter" )	
	local tp28_r			= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "RightCenter" )
	local lbl_pos			= {0, pb_props[pb.L6].pos[2]-tp_default.height*2.0}
	local lbl_W, lbl_H		= tp_default.width*41.00, tp_default.height*4.20
	
	local str1_x			= -lbl_W/2+tp_default.width*0.70
	local str1_x2			= str1_x+tp_default.width*36.50
	local str1_y			= tp_default.height*1.25
	local str2_y			= 0
	local str3_y			= -tp_default.height*1.25
	
	tp28.stringdefs = {smallfont_size*GetScale(),smallfont_size*GetScale(),smallfont_size*GetScale()*0.05}	-- to set symbols looser
	
	--						name,							pos,		width,	height
	AddRoundCornersWindow("ActiveWaypointStatusWindowRTE",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"W08",					{str1_x,						str1_y},	tp28,	{{"TSD_POINT_RTE_StatusWindow_Point"}}, 		Point_Types,				nil, nil, nil},
							{"WP",					{str1_x+tp_default.width*4.50,	str1_y},	tp28,	{{"TSD_POINT_RTE_StatusWindow_Identifier"}},	WP_HZ_CM_TG_Identifier,		nil, nil, nil},
							{"W08",					{str1_x+tp_default.width*8.00,	str1_y},	tp28,	{{"TSD_POINT_RTE_StatusWindow_Text"}}, 			nil, 						nil, nil, nil},
							
							{"ETE 00:00:00",		{str1_x+tp_default.width*12.50,	str1_y},	tp28,	{{"TSD_POINT_RTE_StatusWindow_ETE"}}, 			nil, 						nil, nil, nil},
							
							{"ETA 00:00:00L",		{lbl_W/2-tp_default.width*1.00,	str1_y},	tp28_r,	{{"TSD_POINT_RTE_StatusWindow_ETA"}}, 			nil, 						nil, nil, nil},
							
							{"   0.0 KM",			{lbl_W/2-tp_default.width*1.00,	str2_y},	tp28_r,	{{"TSD_POINT_RTE_StatusWindow_Distance", 0}}, 	nil, 						nil, nil, nil},
																		
							{"   0.0 NM",			{lbl_W/2-tp_default.width*1.00,	str3_y},	tp28_r,	{{"TSD_POINT_RTE_StatusWindow_Distance", 1}}, 	nil, 						nil, nil, nil},
							
						},
					--	tp,			material,				parent,							controllers
						tp_default,	IND_MPD_MATERIAL_GREEN,	InfoWindowsBase.name,	{{"TSD_RTE_StatusWindow_Show"}})
end

-----------------------------------------------------------

local function AddR1R6ArrowsMenuItem()
	-- TODO: need to set position
	
	local font_size = tp_default.height
	local arr1_pos = {pb_props[pb.R1].pos[1]-font_size*0.5, pb_props[pb.R1].pos[2]-font_size*0.5}
	local arr2_pos = {pb_props[pb.R6].pos[1]-font_size*0.5, pb_props[pb.R6].pos[2]-font_size*0.5}
	
	AddArrowMenuLabel(pb.R1, "R1R6Menu_Arr1",	arr1_pos,	nil,	InfoWindowsBase.name)
	AddArrowMenuLabel(pb.R6, "R1R6Menu_Arr2",	arr2_pos,	180,	InfoWindowsBase.name)
end

---------------------------------------------------------
------------------- Draw something ----------------------
---------------------------------------------------------

if DBG_LABEL_SHOW then
addText( "TSD RTE PAGE",  {0, 350}, tp_36_white)
end


-- 8
-- ********** Ownship Sensor Layer **********
AddCompassRose()

-- 9
-- ********** Ownship Layer **********
AddFrozenOwnship()
AddOwnshipSymbol()

-- 10
-- ********** Info Windows and Menus **********
AddMapFrozenCue()
AddCurrentHeadingLabel()
AddNextWaypointHeadingLabel()
AddGridStatusLabel()

if DBG_SA_WINDOWS_SHOW then
AddSAStatusWindowSmall()
AddSAStatusWindowLarge()
end

AddActiveWaypointStatusWindow()

AddR1R6ArrowsMenuItem()

createMenu( Menu )
createControls( Controls )

for i=0,3 do
UnderlineLabel(i)
end

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = t3_pocket
pb_props[pb.B3].pos = b3_pocket
pb_props[pb.B4].pos = b4_pocket

