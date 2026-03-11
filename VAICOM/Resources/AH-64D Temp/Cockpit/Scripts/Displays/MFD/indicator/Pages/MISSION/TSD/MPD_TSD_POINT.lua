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
	{ pb.T4, "ABR",		nil,	nil},
	{ pb.T5, "COORD",	nil,	{{"TSD_POINT_XMIT_MODE"}}},
	{ pb.T6, "UTIL",	nil,	{{"TSD_POINT_XMIT_MODE"}}},

	{ pb.B3, "BAM",		nil,	nil},
	{ pb.B4, "MAP",		nil,	nil},
	{ pb.B5, "RTE",		nil,	nil},
	{ pb.B6, "POINT",	tp_default_border,	nil}
}

local Controls = {}
Controls = 
{																		--	review		add_i		add_f		add_u				add_a			edit_f		edit_u				edit_a			del			del_y_n		sto_wp			sto_tg			xmit
	{ pb.L1, { {"POINT>", 	nil, 				{{"TSD_POINT_Buttons", 0}}, {"POINT>",	"IDENT>",	"FREE>",	"UTM LAT/LONG>",	"ALTITUDE>",	"FREE>",	"UTM LAT/LONG>",	"ALTITUDE>",	"POINT>",	"POINT>",	"NOW",			"NOW",			"POINT>"}}, {"W00", tp_default_border, {{"TSD_POINT_L1_ButtonText"},{"MFD_DataEntryButton_frame",pb.L1}}, WP_HZ_CM_TG_Identifier} } },
	{ pb.L2, 	"ADD",		tp_default_border,	{{"TSD_POINT_Buttons", 1}}, {"ADD",		"ADD",		"ADD",		"ADD",				"ADD",			"",			"",					"",				"",			"",			"",				"",				""}},
	{ pb.L3, 	"EDIT",		tp_default_border,	{{"TSD_POINT_Buttons", 2}}, {"EDIT",	"WP",		"WP",		"WP",				"WP",			"EDIT",		"EDIT",				"EDIT",			"",			"YES",		"",				"",				""}},                             
	{ pb.L4, 	"DEL",		tp_default_border,	{{"TSD_POINT_Buttons", 3}}, {"DEL",		"HZ",		"HZ",		"HZ",				"HZ",			"",			"",					"",				"DEL",		"NO",		"",				"",				""}},                
	{ pb.L5, 	"STO",		tp_default_border,	{{"TSD_POINT_Buttons", 4}}, {"STO",		"CM",		"CM",		"CM",				"CM",			"",			"",					"",				"",			"",			"STO",			"STO",			""}},                 
	{ pb.L6, 	"XMIT",		tp_default_border,	{{"TSD_POINT_Buttons", 5}}, {"XMIT",	"TG",		"TG",		"TG",				"TG",			"",			"",					"",				"",			"",			"",				"",				"XMIT"}},
	{ pb.L6, { {"XMIT",		nil,				{{"TSD_POINT_Buttons", 5}}, {"",		"",			"",			"",					"",				"",			"",					"",				"",			"",			"TYPE",			"TYPE",			""}}, {"", tp_default_border, {{"TSD_STO_MODE"}}, {"WP","TG"}} } },
	
	{ "DEL", 
				{ 
					{ pb.L3, 	"EDIT",		tp_default_border,	{{"TSD_POINT_Buttons", 2}}, {"EDIT",	"WP",		"WP",		"WP",				"WP",			"EDIT",		"EDIT",				"EDIT",			"",			"YES",		"",				"",				""}},                             
					{ pb.L4, 	"DEL",		tp_default_border,	{{"TSD_POINT_Buttons", 3}}, {"DEL",		"HZ",		"HZ",		"HZ",				"HZ",			"",			"",					"",				"DEL",		"NO",		"",				"",				""}},                
	
				},  
		{{"TSD_POINT_StatusWindow_Show", 3}} 
	},
	
	{ pb.B1, "TSD",		nil,	nil},
	{ pb.B2, { {"PHASE", nil, nil}, {"NAV", tp_default_border, {{"TSD_PHASE_Btn_Caption"}}, {"NAV", "ATK"}} } },

	{ pb.R3, "CTR",		tp_default_border,	{{"TSD_CTR_Button_frame"},{"TSD_POINT_XMIT_MODE"}}},
	{ pb.R4, "FRZ",		tp_default_border,	{{"TSD_FRZ_Button_frame"},{"TSD_POINT_XMIT_MODE"}}},
	
	--for XMIT MODE
	{ pb.T5,	"",			tp_default_border,	{{"TSD_POINT_XMIT_CALLSIGN", 0}}},
	{ pb.T6,	"",			tp_default_border,	{{"TSD_POINT_XMIT_CALLSIGN", 1}}},
	{ pb.R1,	"", 		tp_default_border,	{{"TSD_POINT_XMIT_CALLSIGN", 2}}},
	{ pb.R2,	"",			tp_default_border,	{{"TSD_POINT_XMIT_CALLSIGN", 3}}},
	{ pb.R3,	"",			tp_default_border,	{{"TSD_POINT_XMIT_CALLSIGN", 4}}},
	{ pb.R4,	"",			tp_default_border,	{{"TSD_POINT_XMIT_CALLSIGN", 5}}},
	{ pb.R5,	"",			tp_default_border,	{{"TSD_POINT_XMIT_CALLSIGN", 6}}},
	--
}

local pos_shift_x = 28
local t3_pocket,t4_pocket,b3_pocket,b4_pocket = pb_props[pb.T3].pos,pb_props[pb.T4].pos,pb_props[pb.B3].pos,pb_props[pb.B4].pos

pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - pos_shift_x*1.2
pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + pos_shift_x*0.8
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
	local str2_y			= 0
	local str1_y			= tp_default.height*1.25
	local str3_y			= -str1_y
	
	tp28.stringdefs = {smallfont_size*GetScale(),smallfont_size*GetScale(),smallfont_size*GetScale()*0.05}	-- to set symbols looser
	
	--						name,								pos,		width,	height
	AddRoundCornersWindow("ActiveWaypointStatusWindowPOINT",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							-- line 1
							{"C00",					{str1_x,						str1_y},	tp28,	{{"TSD_POINT_RTE_StatusWindow_Point"}}, 		Point_Types},
							{"CP",					{str1_x+tp_default.width*4.50,	str1_y},	tp28,	{{"TSD_POINT_RTE_StatusWindow_Identifier"}},	WP_HZ_CM_TG_Identifier},
							{"BLD",					{str1_x+tp_default.width*8.00,	str1_y},	tp28,	{{"TSD_POINT_RTE_StatusWindow_Text"}}},
							
							{"ETE 00:00:00",		{str1_x+tp_default.width*12.50,	str1_y},	tp28,	{{"TSD_POINT_RTE_StatusWindow_ETE"}, {"TSD_POINT_StatusWindow_Show", 2}}},
							
							{"ETA 00:00:00L",		{lbl_W/2-tp_default.width*1.00,	str1_y},	tp28_r,	{{"TSD_POINT_RTE_StatusWindow_ETA"}, {"TSD_POINT_StatusWindow_Show", 2}}},
							-- line 2
							{"CL6",					{str1_x,						str2_y},	tp28,	{{"TSD_POINT_StatusWindow_Spheroid"}}, 			Spheroid},
							{"27",					{str1_x+tp_default.width*4.50,	str2_y},	tp28,	{{"TSD_POINT_StatusWindow_Datum"}}},
							{"28S WS 2616 1991",	{str1_x+tp_default.width*8.00,	str2_y},	tp28,	{{"TSD_POINT_StatusWindow_UTM"}}},
							
							{"000^",				{str1_x+tp_default.width*30.00,	str2_y},	tp28_r,	{{"TSD_POINT_StatusWindow_Bearing"}, {"TSD_POINT_StatusWindow_Show", 2}}},
							
							{"0.0 KM",				{lbl_W/2-tp_default.width*1.00,	str2_y},	tp28_r,	{{"TSD_POINT_RTE_StatusWindow_Distance", 0}, {"TSD_POINT_StatusWindow_Show", 2}}},				
                          
							
							-- line 3
							{"N00 30.34",			{str1_x,						str3_y},	tp28,	{{"TSD_POINT_StatusWindow_Latitude"}}},
                          
							{"W000 11.12",			{str1_x+tp_default.width*10.20,	str3_y},	tp28,	{{"TSD_POINT_StatusWindow_Longitude"}}},
                          
							{"000 FT",				{str1_x+tp_default.width*30.00,	str3_y},	tp28_r,	{{"TSD_POINT_StatusWindow_Altitude"}}},
                          
							{"0.0 NM",				{lbl_W/2-tp_default.width*1.00,	str3_y},	tp28_r,	{{"TSD_POINT_RTE_StatusWindow_Distance", 1}, {"TSD_POINT_StatusWindow_Show", 2}}},
							
						},
					--	tp,			material,				parent,							controllers
						tp_default,	IND_MPD_MATERIAL_GREEN,	InfoWindowsBase.name,	{{"TSD_POINT_StatusWindow_Show", 0}})
						
	AddRoundCornersWindow("ADDWaypointStatusWindowPOINT",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							-- line 1
							{"C00",					{str1_x,						str1_y},	tp28,	{{"TSD_POINT_ADD_StatusWindow_Point"}}, 		nil},
							{"CP",					{str1_x+tp_default.width*4.50,	str1_y},	tp28,	{{"TSD_POINT_ADD_StatusWindow_Identifier"}},	WP_HZ_CM_TG_Identifier},
							{"BLD",					{str1_x+tp_default.width*8.00,	str1_y},	tp28,	{{"TSD_POINT_ADD_StatusWindow_Text"}}},
							
							-- line 2
							{"CL6",					{str1_x,						str2_y},	tp28,	{{"TSD_POINT_StatusWindow_Spheroid"}, {"TSD_POINT_StatusWindow_Show", 4}}, 			Spheroid},
							{"27",					{str1_x+tp_default.width*4.50,	str2_y},	tp28,	{{"TSD_POINT_StatusWindow_Datum"}, {"TSD_POINT_StatusWindow_Show", 4}}},
							{"28S WS 2616 1991",	{str1_x+tp_default.width*8.00,	str2_y},	tp28,	{{"TSD_POINT_ADD_StatusWindow_UTM"}}},
															
							-- line 3
							{"N00 30.34",			{str1_x,						str3_y},	tp28,	{{"TSD_POINT_ADD_StatusWindow_Latitude"}}},                     
							{"W000 11.12",			{str1_x+tp_default.width*10.20,	str3_y},	tp28,	{{"TSD_POINT_ADD_StatusWindow_Longitude"}}},                      
							{"000 FT",				{str1_x+tp_default.width*30.00,	str3_y},	tp28_r,	{{"TSD_POINT_ADD_StatusWindow_Altitude"}}},
							
						},
					--	tp,			material,				parent,							controllers
						tp_default,	IND_MPD_MATERIAL_GREEN,	InfoWindowsBase.name,	{{"TSD_POINT_StatusWindow_Show", 1}})
end

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------


if DBG_LABEL_SHOW then
addText( "TSD POINT PAGE",  {0, 350}, tp_36_white)
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

AddR1R2_MapRange_Arrows({{"TSD_POINT_XMIT_MODE"}})

createMenu( Menu )
createControls( Controls )

AddSendBtn("TSD_Point_SendMessageWindow", nil, {{"TSD_POINT_XMIT_SEND"}} )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = t3_pocket
pb_props[pb.T4].pos = t4_pocket
pb_props[pb.B3].pos = b3_pocket
pb_props[pb.B4].pos = b4_pocket

