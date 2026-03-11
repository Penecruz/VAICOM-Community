dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/Common/MPD_TSD_PointsId.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

ScreenSize = display_size_pix
tsd_texture_scale = 1/2048

--collimated		= false
--additive_alpha	= true

-- TODO: to delete this patch
REPLACE_IT_WITH_PROPER_CONTROLLER = nil

DBG_LABEL_SHOW = false
DBG_SA_WINDOWS_SHOW = false

TSD_MapUnits_LEVEL = DEFAULT_LEVEL

CommonTSDPlaceholder		= addPlaceholder("CommonTSDPlaceholder_PH", 	{0, 0}, nil, 						nil)
TacticalMapBase				= addPlaceholder("TacticalMapBase_plaseholder", {0, 0}, CommonTSDPlaceholder.name, 	nil)
OwnshipBase					= addPlaceholder("OwnshipBase_PH",				{0, 0}, CommonTSDPlaceholder.name, 	{{"TSD_OwnshipPos_Move"},{"TSD_OwnshipPos_Rotate"}})
InfoWindowsBase				= addPlaceholder("InfoWindowsBase_PH", 			{0, 0}, CommonTSDPlaceholder.name, 	nil)

TSD_NAV_PHASE_PH = addPlaceholder("TSD_NAV_Phase_Placeholder", {0,0}, nil, {{"TSD_Draw_If_PhaseNAV"}})
TSD_ATK_PHASE_PH = addPlaceholder("TSD_ATK_Phase_Placeholder", {0,0}, nil, {{"TSD_Draw_If_PhaseATK"}})

T3_pocket, T4_pocket = pb_props[pb.T3].pos,pb_props[pb.T4].pos
T3T4_posShiftX = 28

TSD_SUBPAGE_ORIGIN_FMT = { BASE = 0, INST = 1, MAP = 2, BAM = 3, RTE = 4, POINT = 5, PAN = 6 }

-----------------------------------------------------------
-------------------- Prepare functions --------------------
-----------------------------------------------------------
function getPosOnRose(radius, angle)
	return { radius * math.sin(math.rad(angle)), radius * math.cos(math.rad(angle)) }
end
-----------------------------------------------------------
function AddNDBStatusWindow()
	local lbl_pos = {pb_props[pb.T6].pos[1]-10,pb_props[pb.R1].pos[2]}
	local BearingInfoBase = addPlaceholder("NDBStatusWindow _BasePH", lbl_pos, InfoWindowsBase.name, {{"NAV_ADF_getPowered"}})

	local tps		= createTextProperty( nil, nil, nil, "LeftCenter" )
	local tps_wide	= createTextProperty( nil, nil, nil, "LeftCenter" )
	local pos_x		= -tps.height*2.15
	local pos_y		= -tps.height*0.61

	tps_wide.stringdefs	= {tps.height*GetScale(),tps.height*GetScale(),tps.height*GetScale()*0.06}	-- to set symbols looser
	tps.stringdefs 		= {tps.height*GetScale(),tps.height*GetScale(),-tps.height*GetScale()*0.06}	-- to set symbols narrower
	
	AddRoundCornersWindow("NDBStatusWindow",	{0,0},	tps.height*5.25,	tps.height*5.05,
						{
							{"ABY",		{-tps.height/0.46,	tps.height*1.60},	tps_wide,	{{"TSD_BeaconName"}}},
							{"382.0",	{-tps.height/0.46,	tps.height*0.35},	tps_wide,	{{"TSD_BeaconFreq"}}},

							{"M1",		{pos_x,	pos_y},							tps,		{{"TSD_BeaconCode_Show"},{"TSD_BeaconCode",0}}},
							{"M2",		{pos_x,	pos_y - tps.height*0.64},		tps,		{{"TSD_BeaconCode_Show"},{"TSD_BeaconCode",1}}},
							{"M3",		{pos_x,	pos_y - tps.height*1.28},		tps,		{{"TSD_BeaconCode_Show"},{"TSD_BeaconCode",2}}},
						},
						tps,	IND_MPD_MATERIAL_GREEN,	BearingInfoBase.name,	nil)
end
-----------------------------------------------------------
function AddTimerWindow()
	local lbl_pos		= {(pb_props[pb.T1].pos[1]+pb_props[pb.T2].pos[1])/2, pb_props[pb.L1].pos[2]+tp_default.height*1.5}
	local lbl_W, lbl_H	= tp_default.width*8.50, tp_default.height*1.65
	
	local tprops		= createTextProperty( 36,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftCenter" )
	tprops.stringdefs	= {tprops.height*GetScale(),tprops.height*GetScale(),-tprops.height*GetScale()*0.05}	-- to set symbols narrower

	local str1_x		= -lbl_W/2+tp_default.width*0.60
	
	--						name,				pos,		width,	height
	AddRoundCornersWindow("TsdInstTimerWindow",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"0:00:00",	{str1_x, 0}, tprops,	{{"TSD_INST_Timer_readout"}}},
						},
					--	tp,			material,				parent
						tp_default,	IND_MPD_MATERIAL_GREEN,	InfoWindowsBase.name)
end
-----------------------------------------------------------
function AddCurrentRouteLine()
	local CurrentRouteLineBase = addPlaceholder("CurrentRouteLineBase_PH", {0, 0}, TacticalMapBase.name, {{"TSD_CurrentRouteLine_Show"}})

	local width = 3

	draw_line( {{0,0},{0,0}}, "MFD_BACKGROUND", 		CurrentRouteLineBase.name, width-1,	"CurrentRouteLine_Fon",		{{"TSD_CurrentRouteLine_Draw"}}, h_clip_relations.COMPARE, DEFAULT_LEVEL )	
	draw_line( {{0,0},{0,0}}, IND_MPD_MATERIAL_GREEN,	CurrentRouteLineBase.name, width,	"CurrentRouteLine_Elem",	{{"TSD_CurrentRouteLine_Draw"}, {"TSD_CurrentRouteLine_Color"}}, h_clip_relations.COMPARE, DEFAULT_LEVEL )

end-----------------------------------------------------------
function AddDirectToLine()
	local DirectToLineBase = addPlaceholder("DirectToLineBase_PH", {0, 0}, TacticalMapBase.name, {{"TSD_DirectToLine_Show"}})

	local width = 3
	
	draw_line( {{0,0},{0,0}}, "MFD_BACKGROUND",			DirectToLineBase.name, width-1,	"DirectToLine_Fon",		{{"TSD_DirectToLine_Draw"}}, h_clip_relations.COMPARE, DEFAULT_LEVEL )
	draw_line( {{0,0},{0,0}}, IND_MPD_MATERIAL_GREEN,	DirectToLineBase.name, width,	"DirectToLine_Elem",	{{"TSD_DirectToLine_Draw"}}, h_clip_relations.COMPARE, DEFAULT_LEVEL )

end
-----------------------------------------------------------
function AddOwnshipSymbol()
	local scale = 1.8	-- more scale -> less symbol
	
	local x1, y1 = -46/scale, 50/scale
	local x2, y2 = -x1, -96/scale
		
	local verts = {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords = {128, 1792}

	DrawTSDTexPoly(verts, tex_coords, scale, nil, OwnshipBase.name, IND_MPD_TSD_MATERIAL_BLACK)
	DrawTSDTexPoly(verts, tex_coords, scale, nil, OwnshipBase.name, IND_MPD_TSD_SYMBOLS_CYAN)
end
-----------------------------------------------------------
function AddR1R2_MapRange_Arrows(controller)
	local group_pos	= {pb_props[pb.R1].pos[1], (pb_props[pb.R1].pos[2] + pb_props[pb.R2].pos[2])/2}

	local R1R2MenuLabelBase = addPlaceholder("TSD_R1R2Menu_plaseholder", group_pos, InfoWindowsBase.name, controller)

	local font_size = tp_default.height
	
	AddArrowMenuLabel(pb.R1, "R1R2Menu_Arr1",	{-font_size*0.5, font_size*2.1},	nil,	R1R2MenuLabelBase.name)
	AddArrowMenuLabel(pb.R2, "R1R2Menu_Arr2",	{-font_size*0.5, -font_size*2.1},	180,	R1R2MenuLabelBase.name)
	
	addText("400",	{0,0},	tp_def_right_center, {{"TSD_MapRange_Caption_Show", 0},{"TSD_MapRange_Caption"}}, nil, nil, "R1R2_text_lbl",		R1R2MenuLabelBase.name)

	local txt 		= addText("400",	{0,0},	tp_def_right_center, {{"TSD_MapRange_Caption_Show", 1},{"TSD_MapRange_Caption"}}, nil, nil, "R1R2_text_lbl_bold",	R1R2MenuLabelBase.name)
	txt.material	= fontPrefix..tp_def_right_center.color.."_bold"
end
-----------------------------------------------------------
function AddMapFrozenCue()
	local BasePH = addPlaceholder("MapFrozenCue_BasePH", {0,0}, nil, {{"TSD_MapFrozenCue_Show"}})
	
	local line_w		= 6
	local w2			= line_w/2
	local stroke		= tp_default.height*3.415
	local gap			= tp_default.height*3.317
	
	local length		= stroke*5 + gap*4		--tp_default.height*31.0
	local pos_x, pos_y	= length/2, length/2
	
	local function add_elem(material, name_suf)

		--				name,								length, 		stroke,				gap,	width,	pos,					rot,	parent,			controllers,	material
		addFatDashedLine("MapFrozenCue_left"..name_suf,		length-line_w,	stroke,				gap,	line_w,	{-pos_x,	-pos_y+w2},	0,		BasePH.name,	nil,			material)
		addFatDashedLine("MapFrozenCue_right"..name_suf,	length-line_w,	stroke,				gap,	line_w,	{pos_x,		-pos_y+w2},	0,		BasePH.name,	nil,			material)
		addFatDashedLine("MapFrozenCue_top"..name_suf,		length+line_w,	stroke+line_w/5,	gap,	line_w,	{pos_x+w2,	pos_y},		90,		BasePH.name,	nil,			material)
		addFatDashedLine("MapFrozenCue_bottom"..name_suf,	length+line_w,	stroke+line_w/5,	gap,	line_w,	{pos_x+w2,	-pos_y},	90,		BasePH.name,	nil,			material)
	
	end

	add_elem("MFD_BACKGROUND", "_fon")
	add_elem(IND_MPD_MATERIAL_GREEN, "")
end
-----------------------------------------------------------
function AddFrozenOwnship( controllers)
	local OwnshipBase = addPlaceholder("OwnshipBase_frozen_PH", {0, 0}, nil, {{"TSD_MapFrozenCue_Show"},{"TSD_MapFrozenCue_Ownship",-GetScale()*ScreenSize/5}})

	local scale = 1.8	-- more scale -> less symbol
	
	local x1, y1 = -46/scale, 50/scale
	local x2, y2 = -x1, -96/scale
	
	local verts = {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords = {256, 1792}
	
	DrawTSDTexPoly(verts, tex_coords, scale, controllers, OwnshipBase.name, IND_MPD_TSD_MATERIAL_BLACK)
	DrawTSDTexPoly(verts, tex_coords, scale, controllers, OwnshipBase.name, IND_MPD_TSD_SYMBOLS_WHITE)
end
-----------------------------------------------------------
function AddOwnshipShadow()
	local scale = 1.8	-- more scale -> less symbol

	local x1, y1 = -46/scale, 50/scale
	local x2, y2 = -x1, -96/scale

	local verts = {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords = {384, 1792}

	DrawTSDTexPoly(verts, tex_coords, scale, nil, OwnshipBase.name, IND_MPD_TSD_MATERIAL_BLACK)
end
-----------------------------------------------------------
function AddMapTacticalSymbol(name, font, controllers, parent, pos, size, value)
	local font_size		= size or 86

	local elem			= CreateElement "ceStringPoly"
	elem.material		= font
	elem.alignment		= "CenterCenter"
	elem.stringdefs		= {font_size*GetScale(),font_size*GetScale()}
	elem.value			= value or ""

	setSymbolCommonProperties(elem, name, pos, parent, controllers, font)
	
	Add(elem)
end
-----------------------------------------------------------
function AddGridLines()
	-- Att! Width and coords are related!
	local width = 1.2

	-- local cell =		ScreenSize/5
	-- local pos =		{-ScreenSize/2+cell/2, -ScreenSize/2}
	-- vert lines:		{pos[1]+cell*i,pos[2]}
	-- pos =			{-ScreenSize/2, -ScreenSize/2+cell/2}
	-- hor lines: 		{pos[1],pos[2]+cell*i}
	-- local coords:	{-409.6, -204.8, 0.5, 204.8, 409.6 }
	
	local initial_coords = {-0.5, 204.5, 409.5, 614.5 }	-- needed to fit line (has width) into pixels

	local BasePH = addPlaceholder("GridLines_BasePH", {0,0}, TacticalMapBase.name, {{"TSD_GridLines_Show"}})

	local function addLine(name_suff, coords, rot, draw_controllers)
		local LinePH = addPlaceholder("GridLines_Line_"..name_suff.."_PH", coords, BasePH.name, draw_controllers)

		addFatLine("GridLines_"..name_suff.."_fon",	ScreenSize, width, {0,0}, rot, LinePH.name, nil, "MFD_BACKGROUND")
		addFatLine("GridLines_"..name_suff, 		ScreenSize, width, {0,0}, rot, LinePH.name, {{"TSD_ColorGridLines"}}, IND_MPD_MATERIAL_DARK_GREEN)
	end		

	local index = 0
	addLine("V_0", {initial_coords[1],	-ScreenSize/2}, 		nil)
	addLine("H_0", {-ScreenSize/2,		initial_coords[1]}, 	-90)

	for i = 1, 3 do
		addLine("V_"..index + 1, {0,	-ScreenSize/2}, 	nil,	{{"TSD_GridLines_Draw_V", i}})
		addLine("V_"..index + 2, {-1,	-ScreenSize/2}, 	nil,	{{"TSD_GridLines_Draw_V", -i}})

		addLine("H_"..index + 1, {-ScreenSize/2,	0},		-90,	{{"TSD_GridLines_Draw_H", i}})
		addLine("H_"..index + 2, {-ScreenSize/2,	-1}, 	-90,	{{"TSD_GridLines_Draw_H", -i}})

		index = index + 2
	end
end
-----------------------------------------------------------
function AddEnduranceStatusWindow()
	local tp36			= createTextProperty( 36,  "GREEN",	IND_MPD_MATERIAL_GREEN, "RightBottom" )
	local tp28			= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftBottom" )
	local lbl_W, lbl_H	= tp_default.width*8.70, tp_default.height*1.65
	local lbl_pos		= {(pb_props[pb.B5].pos[1]+pb_props[pb.B6].pos[1])/2+23,pb_props[pb.R6].pos[2]+6}
	
	tp36.stringdefs		= {tp36.height*GetScale(),tp36.height*GetScale(),-tp36.height*GetScale()*0.05}	-- to set symbols narrower
	
	local pos_x1	= -lbl_W/2 + tp28.width*0.5
	local pos_x2	= lbl_W/2 - tp36.width*0.5
	local pos_y		= -lbl_H/2 + tp28.width*0.3
	
	--						name,					pos,		width,	height
	AddRoundCornersWindow("EnduranceStatusWindow",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"2:13",{pos_x2,					pos_y},		tp36,	{{"TSD_ENStatus_Caption_Show"},{"TSD_ENStatus_Caption_Color"},{"TSD_ENStatus_Time"}}},
							{"EN",	{pos_x1,					pos_y},		tp28,	{{"TSD_ENStatus_Caption_Show"},{"TSD_ENStatus_Caption_Color"}}}
						},
					--	tp,			material,				parent,							controllers
						tp_default,	IND_MPD_MATERIAL_GREEN,	InfoWindowsBase.name,	{{"TSD_ENStatus_Window"}})
end
--------------------------------------------------------------
function AddSAStatusWindowSmall()
	local tp_left			= createTextProperty( nil,  nil,	nil,  "LeftCenter" )
	local tp_center			= createTextProperty( nil,  nil,	nil,  "CenterCenter" )
	local lbl_W, lbl_H		= tp_left.width*20.60, tp_left.height*6.2
	local lbl_pos			= {0, lbl_H/2}	-- TODO: dbg
	
	--tp_left.stringdefs	= {tp_left.height*GetScale(),tp_left.height*GetScale(),-tp_left.height*GetScale()*0.05}	-- to set symbols narrower
	
	local str1_x1	= -lbl_W/2 + tp_left.width*0.5
	local str4_x2	= str1_x1 + tp_left.width*5.0
	
	local str1_y	= tp_left.height*2.30
	local str2_y	= tp_left.height*1.15
	local str3_y	= -tp_left.height*0.00
	local str4_y	= -tp_left.height*1.15
	local str5_y	= -tp_left.height*2.30
	
	--								name,					pos,		width,	height
	local PH = AddRoundCornersWindow("SAStatusWindowSmall",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"FRIENDLY",			{-tp_center.width*0.00,			str1_y},	tp_center,	{{"TSD_SA_StatusWindow_Caption"}}, {"UNKNOWN","FRIENDLY","ENEMY"}},
							{"MISSILE LAUNCHER",	{str1_x1,	 		 			str2_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_Type"}}},
							{"INTERMEDIATE RANGE",	{str1_x1,						str3_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_SubType"}}},
							{"AGE:",				{str1_x1,						str4_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"}}},
							{"00:03:55",			{str4_x2,						str4_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_Age"}}},
							{"UNIT",				{str1_x1,						str5_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"}}},
							{"ID:",					{str1_x1+tp_left.width*4.50,	str5_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"}}},
							{"12345",				{str1_x1+tp_left.width*7.60,	str5_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_ID"}}}
						},
					--	tp,		material,				parent,							controllers
						nil,	IND_MPD_MATERIAL_GREEN,	InfoWindowsBase.name,	{{"TSD_SA_StatusWindow_Small"}}, nil, {{"TSD_SA_StatusWindow_Color"}})
						
	local icon_size = tp_left.width*0.95
	local icon_pos_2 = {lbl_W/2-icon_size*1.1, lbl_H/2-icon_size*0.9}
	local icon_pos_1 = {icon_pos_2[1]-icon_size/2, icon_pos_2[2]-icon_size/2}

	addBox("SAStatusWindowSmall_icon_1", icon_size, icon_size, "CenterCenter", icon_pos_1, PH.name, {{"TSD_SA_StatusWindow_Color"}}, IND_MPD_MATERIAL_GREEN)
	addBox("SAStatusWindowSmall_icon_m", icon_size*1.1, icon_size*1.1, "CenterCenter", icon_pos_2, PH.name, nil, "MFD_BACKGROUND")
	addBox("SAStatusWindowSmall_icon_2", icon_size, icon_size, "CenterCenter", icon_pos_2, PH.name, {{"TSD_SA_StatusWindow_Color"}}, IND_MPD_MATERIAL_GREEN)
end
----------------------------------------------------------------
function AddSAStatusWindowLarge()
	local tp_left			= createTextProperty( nil,  nil,	nil,  "LeftCenter" )
	local tp_center			= createTextProperty( nil,  nil,	nil,  "CenterCenter" )
	local tp_right			= tp_def_right_center
	local lbl_W, lbl_H		= tp_left.width*24.60, tp_left.height*10.65
	local lbl_pos			= {0, 0}
	
	--tp_left.stringdefs	= {tp_left.height*GetScale(),tp_left.height*GetScale(),-tp_left.height*GetScale()*0.05}	-- to set symbols narrower
	
	local str1_x1	= -lbl_W/2 + tp_left.width*0.5
	local str4_x2	= str1_x1 + tp_left.width*5.0
	
	local str1_y	= tp_left.height*4.60
	local str2_y	= tp_left.height*3.45
	local str3_y	= tp_left.height*2.30
	local str4_y	= tp_left.height*1.15
	local str5_y	= tp_left.height*0.00
	local str6_y	= -tp_left.height*1.15
	local str7_y	= -tp_left.height*2.30
	local str8_y	= -tp_left.height*3.45
	local str9_y	= -tp_left.height*4.60
	
	--								name,					pos,		width,	height
	local PH = AddRoundCornersWindow("SAStatusWindowLarge",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"FRIENDLY",				{-tp_center.width*0.00,			str1_y},	tp_center,	{{"TSD_SA_StatusWindow_Caption"}}, {"UNKNOWN","FRIENDLY","ENEMY"}},
							{"MISSILE LAUNCHER",		{str1_x1,	 		 			str2_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_Type"}}},
							{"INTERMEDIATE RANGE",		{str1_x1,						str3_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_SubType"}}},
							{"AGE:",					{str1_x1,						str4_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"}}},
							{"00:03:55",				{str4_x2,						str4_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_Age"}}},
							{"UNIT",					{str1_x1,						str5_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"}}},
							{"ID:",						{str1_x1+tp_left.width*4.50,	str5_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"}}},
							{"123456",					{str1_x1+tp_left.width*7.60,	str5_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_ID"}}},
							{"CRS/SPEED:",				{str1_x1,						str6_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"}}},

							--{"^",						{str1_x1+tp_left.width*13.60,	str6_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"}}},
							{"348^",					{str1_x1+tp_left.width*10.80,	str6_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_Course"}}},
							
							{"10.0",					{str1_x1+tp_left.width*20.00,	str6_y},	tp_right,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_Speed"}}},
							{"NM",						{str1_x1+tp_left.width*20.80,	str6_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{REPLACE_IT_WITH_PROPER_CONTROLLER}}},	-- TODO: May Units be different?
							
							{"CLO 32 12S VL 9717 4504",	{str1_x1,						str7_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_UTMcoords"}}},
							{"N 32",					{str1_x1,						str8_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_Latitude",0}}},
							{"02.64",					{str1_x1+tp_left.width*5.00,	str8_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_Latitude",1}}},

							{"W111",					{str1_x1+tp_left.width*10.50,	str8_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_Longitude",0}}},
							{"01.80",					{str1_x1+tp_left.width*15.50,	str8_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_Longitude",1}}},
							
							{"ALT:",					{str1_x1,						str9_y},	tp_left,	{{"TSD_SA_StatusWindow_Color"}}},
							{"7500 FT",					{-tp_left.width*3.00,			str9_y},	tp_center,	{{"TSD_SA_StatusWindow_Color"},{"TSD_SA_StatusWindow_Altitude"}}},
						},
					--	tp,		material,				parent,							controllers
						nil,	IND_MPD_MATERIAL_GREEN,	InfoWindowsBase.name,	{{"TSD_SA_StatusWindow_Large"}}, nil, {{"TSD_SA_StatusWindow_Color"}})

	local icon_size = tp_left.width*0.95
	local icon_pos_2 = {lbl_W/2-icon_size*1.1, lbl_H/2-icon_size*0.9}
	local icon_pos_1 = {icon_pos_2[1]-icon_size/2, icon_pos_2[2]-icon_size/2}

	addBox("SAStatusWindowLarge_icon_1", icon_size, icon_size, "CenterCenter", icon_pos_1, PH.name, {{"TSD_SA_StatusWindow_Color"}}, IND_MPD_MATERIAL_GREEN)
	addBox("SAStatusWindowLarge_icon_m", icon_size*1.1, icon_size*1.1, "CenterCenter", icon_pos_2, PH.name, nil, "MFD_BACKGROUND")
	addBox("SAStatusWindowLarge_icon_2", icon_size, icon_size, "CenterCenter", icon_pos_2, PH.name, {{"TSD_SA_StatusWindow_Color"}}, IND_MPD_MATERIAL_GREEN)
end
----------------------------------------------------------------
function AddWindStatusWindow()
	local lbl_pos		= {(pb_props[pb.B5].pos[1]+pb_props[pb.B6].pos[1])/2+9, pb_props[pb.R6].pos[2]-52}
	local lbl_W, lbl_H	= tp_default.width*10.0, tp_default.height*1.65
	
	local tp36_narrow		= createTextProperty( 36,  nil,	nil,  "RightCenter" )
	local tp36_loose		= createTextProperty( 36,  nil,	nil,  "RightCenter" )
	
	tp36_narrow.stringdefs	= {tp36_narrow.height*GetScale(),tp36_narrow.height*GetScale(),-tp36_narrow.height*GetScale()*0.05}	-- to set symbols narrower
	tp36_loose.stringdefs = {tp36_loose.height*GetScale(),tp36_loose.height*GetScale(),tp36_loose.height*GetScale()*0.05}	-- to set symbols looser
	
	local WindWBase = addPlaceholder("WindWBase_PH", lbl_pos, InfoWindowsBase.name, {{"TSD_WindStatus_Window"}})
	
	--						name,				pos,	width,	height
	AddRoundCornersWindow("WindStatusWindow",	{0,0},	lbl_W,	lbl_H,
						{	-- value
							{"/",	{tp36_narrow.width*1.40,			0},	tp36_narrow,	nil},
							{"120",	{-lbl_W/2+tp36_narrow.width*4.40,	0},	tp36_loose,		{{"TSD_WindStatus_Angle"}}},
							{"15",	{lbl_W/2-tp36_narrow.width*0.30,	0},	tp36_narrow,	{{"TSD_WindStatus_Speed"}}}
						},
					--	tp,		material,				parent,			controllers
						nil,	IND_MPD_MATERIAL_GREEN,	WindWBase.name,	{{"TSD_WindStatus_Label",0}})


	AddRoundCornersWindow("WindStatusWindow_CALM", {0,0}, lbl_W, lbl_H, "CALM", tp36_loose, IND_MPD_MATERIAL_GREEN,	WindWBase.name,	{{"TSD_WindStatus_Label",1}})
end
----------------------------------------------------------------
function AddCursorLocationWindow()
	local lbl_W, lbl_H	= tp_default.width*47.1, tp_default.height*1.485
	local lbl_pos		= {0,pb_props[pb.R6].pos[2]- lbl_H - 63}
	
	local tp_left		= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftCenter" )
	local tp_right		= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "RightCenter" )
	
	local pos_x1 = -lbl_W/2 + tp_default.width*0.6
	local pos_x2 = pos_x1 + tp_default.width*4.0
	local pos_x3 = pos_x2 + tp_default.width*3.0
	local pos_x4 = lbl_W/2 - tp_default.width*11.5
	
	--						name,					pos,		width,	height
	AddRoundCornersWindow("CursorLocationWindow",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"CL0",						{pos_x1,						0},	tp_left,	{{"TSD_POINT_StatusWindow_Spheroid"}}, Spheroid},
							{"32",						{pos_x2,						0},	tp_left,	{{"TSD_POINT_StatusWindow_Datum"}}},
							{"14R PK 1687 4481",		{pos_x3,						0},	tp_left,	{{"TSD_CursorLocationWindow_Color"},{"TSD_CursorLocationWindow_Coords"}}, Spheroid},
							{"?",						{pos_x4,						0},	tp_right,	{{"TSD_CursorLocationWindow_MSL_Show",0},{"TSD_CursorLocationWindow_Color"}}},
							{"900",						{pos_x4,						0},	tp_right,	{{"TSD_CursorLocationWindow_MSL_Show",1},{"TSD_CursorLocationWindow_Color"},{"TSD_CursorLocationWindow_MSL"}}},
							{"FT",						{pos_x4+tp_default.width*0.6,	0},	tp_left,	{{"TSD_CursorLocationWindow_Color"}}},
							{"?",						{pos_x4+tp_default.width*8.5, 	0},	tp_right,	{{"TSD_CursorLocationWindow_Range_Show",0},{"TSD_CursorLocationWindow_Color"}}},
							{"18.6",					{pos_x4+tp_default.width*8.5, 	0},	tp_right,	{{"TSD_CursorLocationWindow_Range_Show",1},{"TSD_CursorLocationWindow_Color"},{"TSD_CursorLocationWindow_Range"}}},
							{"KM",						{lbl_W/2-tp_default.width*0.6,	0},	tp_right,	{{"TSD_CursorLocationWindow_Color"},{"NAV_WPStatus_Units"}}, {"KM","NM"}}
						},
					--	tp,			material,				parent,							controllers
						tp_default,	IND_MPD_MATERIAL_GREEN,	InfoWindowsBase.name,	{{"TSD_CursorLocationWindow_Show"}}, nil, {{"TSD_CursorLocationWindow_Color"}})
end
----------------------------------------------------------------
function AddPresentPositionWindow()
	local lbl_W, lbl_H	= tp_default.width*32.0, tp_default.height*3.0
	local lbl_pos		= {0,pb_props[pb.R6].pos[2] - 63}
	
	local tp_left		= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftCenter" )
	local tp_right		= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "RightCenter" )
	
	local pos_x = -lbl_W/2 + tp_default.width*0.6
	local pos_x1 = pos_x + tp_default.width*4.0
	local pos_x2 = pos_x1 + tp_default.width*3.0
	local pos_y = tp_default.width*1.0
	
	--						name,					pos,		width,	height
	AddRoundCornersWindow("PresentPositionWindow",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"CL0",						{pos_x,							pos_y},		tp_left,	{{"TSD_POINT_StatusWindow_Spheroid"}}, Spheroid},
							{"32",						{pos_x1,						pos_y},		tp_left,	{{"TSD_POINT_StatusWindow_Datum"}}},
							{"14R PK 1687 4481",		{pos_x2,						pos_y},		tp_left,	{{"TSD_PresentPosWindow_UTMcoord"}}},
							{"N00 30.34 W000 11.12",	{pos_x,							-pos_y},	tp_left,	{{"TSD_PresentPosWindow_LatLon"}}},
							{"900",						{lbl_W/2-tp_default.width*3.1,	-pos_y},	tp_right,	{{"TSD_PresentPosWindow_CurrentMSL"}}},
							{"FT",						{lbl_W/2-tp_default.width*0.6,	-pos_y},	tp_right,	nil},
						},
					--	tp,			material,				parent,							controllers
						tp_default,	IND_MPD_MATERIAL_GREEN,	InfoWindowsBase.name,	{{"TSD_PresentPosWindow_Show"}})
end
----------------------------------------------------------------
function AddGridStatusLabel()
	local tprops			= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "CenterCenter" )
	tprops.stringdefs		= {tprops.height*GetScale(),tprops.height*GetScale(),tprops.height*GetScale()*0.05}
	local lbl_pos 			= {pb_props[pb.R1].pos[1]-tprops.width*1.5, pb_props[pb.T1].pos[2]+tprops.height*0.1}
	
	local GridStatus_PH = addPlaceholder("GridStatus_Placeholder", {0,0}, InfoWindowsBase.name, {{"TSD_GridLines_Show"}})

	AddRoundCornersWindow("GridStatusWindow_1",	lbl_pos,	nil,	nil,
						{	-- value
							{"5KM",		{0,0},	tprops,	{{"TSD_GridStatusWindow_Text"}}}
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	GridStatus_PH.name,	{{"TSD_GridStatusWindow_Show",0}})
						
	lbl_pos 			= {pb_props[pb.R1].pos[1]-tprops.width*2.05, pb_props[pb.T1].pos[2]+tprops.height*0.1}					
	AddRoundCornersWindow("GridStatusWindow_2",	lbl_pos,	nil,	nil,
						{	-- value
							{"80KM",		{0,0},	tprops,	{{"TSD_GridStatusWindow_Text"}}}
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	GridStatus_PH.name,	{{"TSD_GridStatusWindow_Show",1}})
	
	tprops.margins = {0,0,0,0};
	lbl_pos 			= {pb_props[pb.R1].pos[1]-tprops.width*2.55, pb_props[pb.T1].pos[2]+tprops.height*0.1}
	AddRoundCornersWindow("GridStatusWindow_fr",	lbl_pos,	tprops.width*6.70,	nil,
						{	-- value
							{"0.22KM",	{0,0},	tprops,	{{"TSD_GridStatusWindow_Text"}}},
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	GridStatus_PH.name,	{{"TSD_GridStatusWindow_Show",2}})
end
-----------------------------------------------------------
function AddCompassRose(always_draw)
	local CompasDrawController = {"TSD_CompasRose_Show"}
	
	if (always_draw ~= nil) and (always_draw ~= false) then
		CompasDrawController = nil
	end

	local CompasRoseRadius 	= 280
	local TextureScale 		= 1/1024

	local CompasRoseBase = addPlaceholder("CompasRoseBase_PH", {0, 0}, TacticalMapBase.name, {{"NAV_Heading_Valid"},CompasDrawController,{"TSD_CompasRose_Pos",-GetScale()*ScreenSize/5}})
	local CompasRoseRot = addPlaceholder("CompasRoseRot_PH", {0, 0}, CompasRoseBase.name, {{"TSD_CompasRose_Rotate"}})

	-- compase rose notches --
	do
		local scale = 1.0	-- more scale -> less symbol
		
		local x1, y1 = -281/scale, 281/scale
		local x2, y2 = -x1, -y1
		
		local verts = {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
		local tex_coords = {294, 294}

		DrawTSDTexPoly(verts, tex_coords, scale, nil, CompasRoseRot.name, IND_MPD_TSD_COMPASS_MATERIAL_BLACK, 	nil, nil, nil, nil, TextureScale)
		DrawTSDTexPoly(verts, tex_coords, scale, nil, CompasRoseRot.name, IND_MPD_TSD_COMPASS_GREEN, 			nil, nil, nil, nil, TextureScale)
	end

	-- compase rose labels --
	local CompasRoseLabelsArr = {"N", "3", "6", "E", "12", "15", "S", "21", "24", "W", "30", "33"}
	local CompasRoseLblRadius = CompasRoseRadius*0.82
	
	for i = 1,12,1 do
		local angle		= (i - 1) * 30
		local Lbl_PH	= addRotPlaceholder("CompasRose Label_"..i.." plaseholder", getPosOnRose(CompasRoseLblRadius, angle), -angle, CompasRoseRot.name)
		local txt		= addText(CompasRoseLabelsArr[i], {0, 0}, nil, nil, nil, nil, "CompaseRose Label "..i, Lbl_PH.name, h_clip_relations.COMPARE, DEFAULT_LEVEL)
	end
	
	-- 45deg notches --
	local Radius45deg = CompasRoseRadius - 22.5
	local Base45RotPH = addPlaceholder("CompasRose_45deg_BasePH", {0, 0}, CompasRoseBase.name, {{"TSD_CompasRose_45segs_Rotate"}})	

	for i = 0,315,45 do
		local scale = 1.60	-- more scale -> less symbol

		local x1, y1 = -10/scale, 38/scale
		local x2, y2 = -x1, -y1

		local verts = {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
		local tex_coords = {916, 48}

		local Figure_PH = addRotPlaceholder("CompasRose 45deg segment "..i.." plaseholder", getPosOnRose(Radius45deg, i), 180-i, Base45RotPH.name)

		DrawTSDTexPoly(verts, tex_coords, scale, nil, Figure_PH.name, IND_MPD_TSD_COMPASS_MATERIAL_BLACK,	nil, nil, nil, nil, TextureScale)
		DrawTSDTexPoly(verts, tex_coords, scale, nil, Figure_PH.name, IND_MPD_TSD_COMPASS_GREEN, 			nil, nil, nil, nil, TextureScale)			
	end
	
	-- TODO: Should these notches be fixed on Compass Rose while it rotates? If so, just set the parent parameter to CompasRoseRot.name	

	-- Heading Select Indicator --
	if(always_draw) then		-- only on TSD INST page
		local HeadingIndicator_PH = addPlaceholder("HeadingIndicator _plaseholder", {0, 0}, CompasRoseRot.name, {{"TSD_SelectedHeading"}})

		do
			local Figure_PH = addPlaceholder("HeadingIndicator up plaseholder", getPosOnRose(CompasRoseRadius + 19.5, 0), HeadingIndicator_PH.name)

			local scale = 1.29	-- more scale -> less symbol

			local x1, y1 = -32/scale, 16.5/scale
			local x2, y2 = -x1, -y1

			local verts = {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
			local tex_coords = {843, 29.5}

			DrawTSDTexPoly(verts, tex_coords, scale, nil, Figure_PH.name, IND_MPD_TSD_COMPASS_MATERIAL_BLACK,	nil, nil, nil, nil, TextureScale)
			DrawTSDTexPoly(verts, tex_coords, scale, nil, Figure_PH.name, IND_MPD_TSD_COMPASS_GREEN,			nil, nil, nil, nil, TextureScale)
		end

		do
			local Figure_PH = addPlaceholder("HeadingIndicator dn plaseholder", getPosOnRose(-CompasRoseRadius - 19, 0), HeadingIndicator_PH.name)

			local scale = 1.225	-- more scale -> less symbol

			local x1, y1 = -32/scale, 16.5/scale
			local x2, y2 = -x1, -y1

			local verts = {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
			local tex_coords = {843, 71.5}

			DrawTSDTexPoly(verts, tex_coords, scale, nil, Figure_PH.name, IND_MPD_TSD_COMPASS_MATERIAL_BLACK,	nil, nil, nil, nil, TextureScale)
			DrawTSDTexPoly(verts, tex_coords, scale, nil, Figure_PH.name, IND_MPD_TSD_COMPASS_GREEN,			nil, nil, nil, nil, TextureScale)
		end
	end

	-- Bearing Pointer --
	local BearingPointer_PH = addPlaceholder("BearingPointer _plaseholder", {0, 0}, CompasRoseBase.name,
			{
				{"NAV_ADF_getPowered"},
				{"TSD_ADF_BearingPointer_Show"},
				{"TSD_ADF_Bearing"},
				{"TSD_OwnshipPos_Rotate"}
			})

	do
		local scale = 1.33	-- more scale -> less symbol

		local x1, y1 = -CompasRoseRadius/scale, 4.5/scale
		local x2, y2 = 1750/scale, -y1

		local verts = {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
		local tex_coords = {CompasRoseRadius, 957}

		local Figure_PH = addRotPlaceholder("BearingPointer DashLine plaseholder", {0, -CompasRoseRadius - x1}, 90, BearingPointer_PH.name)

		DrawTSDTexPoly(verts, tex_coords, scale, nil, Figure_PH.name, IND_MPD_TSD_COMPASS_MATERIAL_BLACK,	nil, nil, nil, nil, TextureScale)
		DrawTSDTexPoly(verts, tex_coords, scale, nil, Figure_PH.name, IND_MPD_TSD_COMPASS_GREEN,			nil, nil, nil, nil, TextureScale)	
	end
end
-----------------------------------------------------------
function addWaypointSymbol(i, parent, pos_controllers, type_controllers, color_controllers, label_controllers)
	local WPBase	= addPlaceholder("Waypoint_"..i.."_PH", {0,0}, parent, pos_controllers)

	local txt_pos	= {-22,0}
	local tp_rc 	= createTextProperty( 28,  nil,	nil,  "RightCenter" )
	local symb_w	= tp_rc.width

	--
	-- TODO: Nope. You need to repaint textures and make a new texture MFD_MAP_WP_SA_BLACK_TRANSPARENT. Like done with MFD_WPN_MSL_BLACK_TRANSPARENT.
	-- BUT most likely that waypoints' background is NOT transparent black. So now fonts like MFD_MAP_WP_SA_BLACK_TRANSPARENT are just black, and leave it as it is.

	AddMapTacticalSymbol("Waypoint_"..i.."_fon",	"MFD_MAP_WP_SA_BLACK_TRANSPARENT",	type_controllers,								WPBase.name)
	AddMapTacticalSymbol("Waypoint_"..i,			fontPrefix.."MAP_WP",				{type_controllers[1], color_controllers[1]},	WPBase.name)

	addText("", txt_pos, tp_rc,		{{label_controllers[1][1], symb_w}, color_controllers[1]}, nil, nil, "Waypoint_"..i.."_ID",			WPBase.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)
end
-----------------------------------------------------------
function addWaypointInverseID(i, parent, show_controller)
	local txt_pos	= {-22,0}
	local tp_rc_inv	= createTextProperty( 28,  nil,	nil,  "RightCenter", true )
	local symb_w	= tp_rc_inv.width

	local WPBase = addPlaceholder("Waypoint_"..i.."_Selected_PH", {0,0}, parent, {show_controller, {"TSD_SelectedPoint_InverseID_SetPosition"}})
	addText("", txt_pos, tp_rc_inv, {{"TSD_Waypoint_InverseID_SetCaption", symb_w}, {"TSD_Waypoint_InverseID_SetColor"}}, nil, nil, "Waypoint_"..i.."_Selected_ID", WPBase.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)
end
-----------------------------------------------------------
function addControlMeasureSymbol(i, parent, pos_controllers, type_controllers, color_controllers, txt1_controllers, txt2_controllers)
	local CMBase = addPlaceholder("ControlMeasure_"..i.."_PH", {0, 0}, parent, pos_controllers)

	local tp_rc 		= createTextProperty( 28,  nil,	nil,  "RightCenter" )

	local symb_w		= tp_rc.width
	local txt1_pos		= {-35, 0}
	local txt2_pos		= { 30,40}

	AddMapTacticalSymbol("ControlMeasure_"..i.."_fon",	"MFD_MAP_CM_TT_BLACK_TRANSPARENT",	type_controllers,								CMBase.name)
	AddMapTacticalSymbol("ControlMeasure_"..i,			fontPrefix.."MAP_CM",				{type_controllers[1], color_controllers[1]},	CMBase.name)

	addText("", txt1_pos, tp_rc, 		{{txt1_controllers[1][1], symb_w}, color_controllers[1]}, nil, nil, "ControlMeasure_"..i.."_Txt1",	CMBase.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)
	addText("", txt2_pos, tp_rc, 		{{txt2_controllers[1][1], symb_w}, color_controllers[1]}, nil, nil, "ControlMeasure_"..i.."_Txt2",	CMBase.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)
end
-----------------------------------------------------------
function addControlMeasureInverseID(i, parent, show_controller)
	local tp_rc_inv		= createTextProperty( 28,  nil,	nil,  "RightCenter", true )
	local symb_w		= tp_rc_inv.width

	local txt1_pos		= {-35, 0}
	local txt2_pos		= { 30,40}

	local CMBase = addPlaceholder("ControlMeasure_"..i.."_Selected_PH", {0,0}, parent, {show_controller, {"TSD_SelectedPoint_InverseID_SetPosition"}})

	addText("", txt1_pos, tp_rc_inv,	{{"TSD_ControlMeasure_InverseID_SetText1", symb_w}, {"TSD_ControlMeasure_InverseID_SetColor"}}, nil, nil, "ControlMeasure_"..i.."_selected_Txt1",	CMBase.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)
	addText("", txt2_pos, tp_rc_inv,	{{"TSD_ControlMeasure_InverseID_SetText2", symb_w}, {"TSD_ControlMeasure_InverseID_SetColor"}}, nil, nil, "ControlMeasure_"..i.."_selected_Txt2",	CMBase.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)
end
-----------------------------------------------------------
function addIdmSubscriberSymbol(i, parent, pos_controllers, txt_controllers)
	local CMBase = addPlaceholder("IdmSubscriber_"..i.."_PH", {0, 0}, parent, pos_controllers)

	local tp_rc 		= createTextProperty( 28,  "BLUE",	IND_MPD_MATERIAL_BLUE,  "RightCenter" )

	local symb_w		= tp_rc.width
	local txt_pos		= {-35, 0}
	
	-- for the value see
	-- \trunk\Projects\EagleFM\ExternalFM\AH64D\source\Cockpit\Avionics\Displays\MFDS\TSD_Symbology_IDs_AH64.cpp
	-- const ed::unordered_map<WPTHZ_CM_Types, char> ControlMeasurePic
	AddMapTacticalSymbol("IdmSubscriber_"..i.."_fon",	"MFD_MAP_CM_TT_BLACK_TRANSPARENT",	nil,				CMBase.name, nil, nil, "8")
	AddMapTacticalSymbol("IdmSubscriber_"..i,			fontPrefix.."MAP_IDM",				nil,				CMBase.name, nil, nil, "8")

	addText("", txt_pos, tp_rc, {{txt_controllers[1][1], symb_w}}, nil, nil, "IdmSubscriber_"..i.."_Txt",	CMBase.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)
end
-----------------------------------------------------------
function addTargetOrThreatSymbol(i, parent, pos_controllers, type_controllers, label_controllers)
	local TTBase = addPlaceholder("TargetOrThreat_"..i.."_PH", {0, 0}, parent, pos_controllers)

	local tp_lb			= createTextProperty( 28,  "RED",	nil,  "LeftBottom" )
	local tp_rb			= createTextProperty( 28,  "RED",	nil,  "RightBottom" )

	local symb_w		= tp_lb.width
	local v				= symb_w * 0.1

	AddMapTacticalSymbol("TargetOrThreat_"..i.."_fon",	"MFD_MAP_CM_TT_BLACK_TRANSPARENT",	type_controllers, TTBase.name)
	AddMapTacticalSymbol("TargetOrThreat_"..i,			fontPrefix.."MAP_TT",				type_controllers, TTBase.name)

	addText("", {-symb_w,	v},	tp_rb, 		{{label_controllers[1][1], 0, symb_w}}, nil, nil, "TargetThreat_"..i.."_ID_l", TTBase.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)
	addText("", {symb_w,	v},	tp_lb, 		{{label_controllers[1][1], 1, symb_w}}, nil, nil, "TargetThreat_"..i.."_ID_r", TTBase.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)
end
-----------------------------------------------------------
function addTargetOrThreatInverseID(i, parent, show_controller)
	local TTBase = addPlaceholder("TargetOrThreat_"..i.."_Selected_PH", {0, 0}, parent, {show_controller, {"TSD_SelectedPoint_InverseID_SetPosition"}})

	local tp_lb_inv		= createTextProperty( 28,  "RED", 	IND_MPD_MATERIAL_RED,  "LeftBottom", true )
	local tp_rb_inv		= createTextProperty( 28,  "RED", 	IND_MPD_MATERIAL_RED,  "RightBottom", true )

	local symb_w		= tp_lb_inv.width
	local v				= symb_w * 0.1

	addText("", {-symb_w,	v}, tp_rb_inv, 	{{"TSD_TargetThreat_InverseID_SetCaption", 0, symb_w}}, nil, nil, "TargetThreat_"..i.."_selected_ID_l",	TTBase.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)
	addText("", {symb_w,	v}, tp_lb_inv, 	{{"TSD_TargetThreat_InverseID_SetCaption", 1, symb_w}}, nil, nil, "TargetThreat_"..i.."_selected_ID_r",	TTBase.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)
end
-----------------------------------------------------------
function AddSpecialSymbols_TSD()
	AddTRNPointSymbol("PLT", "PLT", TacticalMapBase.name, {{"TSD_TRN_Cross_PLT_Show"},{"TSD_TRN_Cross_PLT_Position"}}, true)
	AddTRNPointSymbol("CPG", "CPG", TacticalMapBase.name, {{"TSD_TRN_Cross_CPG_Show"},{"TSD_TRN_Cross_CPG_Position"}}, true)

end
-----------------------------------------------------------
function addSymbolSA(i, parent, pos_controllers, type_fon_controllers, type_color_controllers)
	local SA_Symbol_Base = addPlaceholder("SA_Symbol_"..i.."_PH", {0, 0}, parent, pos_controllers)

	AddMapTacticalSymbol("TSD_SA_"..i.."_fon",	"MFD_MAP_WP_SA_BLACK_TRANSPARENT",	type_fon_controllers,	SA_Symbol_Base.name)
	AddMapTacticalSymbol("TSD_SA_"..i,			fontPrefix.."MAP_SA",				type_color_controllers,	SA_Symbol_Base.name)
end
-----------------------------------------------------------
function AddMapUnits_SA()
	-- TODO: totally no use and we don't know even what these symbols are.
	
	--local MapSABase = addPlaceholder("MapSABase_PH", {0, 0}, TacticalMapBase.name, {{"TSD_SA_Show"}})
	--
	--for i = 0, 49 do
	--	addSymbolSA(i, MapSABase.name,
	--		{{"TSD_SA_Unit_SetPosition",i}},
	--		{{"TSD_SA_Unit_SetType",i}},
	--		{{"TSD_SA_Unit_SetTypeAndColor",i}}
	--	)
	--end
end
-----------------------------------------------------------
function AddSmallArrowMenuLabel(name, pos, size, rot, parent, controller, material, mask_controller)
	-- pos is position of center of arrow

	local TriangleW	= size*1.05
	local TriangleH	= size/2
	
	local LineL	= size*0.25
	local LineW	= size*0.55
	
	local bot_w = size/8

	rot = rot or 0

	local ArrowLabel_PH = addRotPlaceholder(name.."_plaseholder", pos, rot, parent, controller)	

	--			name,				width,		height,		align,			pos,		rot,	parent,				controllers,	material
	addTriangle(name.."_3a_fon",	TriangleW,	TriangleH,	"CenterBottom",	{0,0},		nil,	ArrowLabel_PH.name,	nil,			"MFD_BACKGROUND")
	addTriangle(name.."_3a",		TriangleW,	TriangleH,	"CenterBottom",	{0,0},		nil,	ArrowLabel_PH.name,	nil,			material)

	--			name,				length,		width,			pos,		rot,	parent,				controllers,	material
	addFatLine(name.."_bline_fon",	bot_w,		TriangleW*0.97,	{0,0},		180,	ArrowLabel_PH.name,	nil,			"MFD_BACKGROUND")
	addFatLine(name.."_bline",		bot_w,		TriangleW*0.97,	{0,0},		180,	ArrowLabel_PH.name,	nil,			material)
	
	addFatLine(name.."_line_fon",	LineL,		LineW,			{0,-bot_w},	180,	ArrowLabel_PH.name,	nil,			"MFD_BACKGROUND")
	addFatLine(name.."_line",		LineL,		LineW,			{0,-bot_w},	180,	ArrowLabel_PH.name,	nil,			material)

	-- "mask"
	local Mask_PH = addPlaceholder(name.."_mask_plaseholder", {0,0}, ArrowLabel_PH.name, mask_controller)	
	
	local coef = 0.6
	addTriangle(name.."_3a_mask",	TriangleW*coef,	TriangleH*coef,	"CenterBottom",	{0,0},			nil,	Mask_PH.name,	nil,	"MFD_BACKGROUND")
	addFatLine(name.."_line_mask",	LineL,			LineW*coef*0.85,				{-LineL*0.1,0},	180,	Mask_PH.name,	nil,	"MFD_BACKGROUND")
	
	-- debug axiss
	--addFatDashedLine(name.."_0y",	TriangleH*2,	5,	5,	2,	{0,-TriangleH},	nil,	ArrowLabel_PH.name,	nil,	IND_MPD_MATERIAL_YELLOW)
	--addFatDashedLine(name.."_0x",	TriangleW*2,	5,	5,	2,	{TriangleW,0},	90,		ArrowLabel_PH.name,	nil,	IND_MPD_MATERIAL_YELLOW)
end
-----------------------------------------------------------
function DrawTSDTexPoly( verts, tex_coords, scale, controllers, parent, material, name, pos, h_clip_relation, level, texture_scale )
	local this_texture_scale = tsd_texture_scale
	if texture_scale ~= nil then
		this_texture_scale = texture_scale
	end
	
	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= verts
	elem.indices		= default_box_indices
	elem.tex_params		= {tex_coords[1] * this_texture_scale, tex_coords[2] * this_texture_scale, scale * this_texture_scale, scale * this_texture_scale} 

	setSymbolCommonProperties(elem, name, pos, parent, controllers, material)

	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
	end
	
	if level ~= nil then
		elem.level				= level
	end

	Add(elem)
	return elem
end
-----------------------------------------------------------
local function RepackControllers(controllers_1, controllers_2)
	local new_cntrs={}
	local it=1
	
	local function copy_tbl(source)
		if source~=nil then
			for i=1,#source do
				new_cntrs[it]=source[i]
				it=it+1
			end
		end
	end

	copy_tbl(controllers_1)
	copy_tbl(controllers_2)

	return new_cntrs
end
-----------------------------------------------------------
--local JVMF_ICONS_Controllers = {{"TSD_JVMF_ICONS_Show"}}
local JVMF_ICONS_Controllers = nil
-----------------------------------------------------------
function DrawUniqueTSDSymbol(verts, tex_coords, scale, controllers, pos, material)
	local cntrl = RepackControllers(JVMF_ICONS_Controllers, controllers)

	DrawTSDTexPoly( verts, tex_coords, scale, cntrl, TacticalMapBase.name, IND_MPD_TSD_MATERIAL_BLACK, nil, pos )
	local elem = DrawTSDTexPoly( verts, tex_coords, scale, cntrl, TacticalMapBase.name, (material or IND_MPD_TSD_SYMBOLS_GREEN), nil, pos )

	return elem
end
-----------------------------------------------------------
function AddMessageToObserverSymbol()
	local scale	= 3.0	-- more scale -> less symbol
	
	local x1, y1	= -128/scale, 128/scale
	local x2, y2	= -x1, -y1
	
	local verts			= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords	= {1280, 1152}

	return DrawUniqueTSDSymbol( verts, tex_coords, scale, {{"TSD_MessageToObserver_Draw"}} )
end
-----------------------------------------------------------
function AddSpotReportSymbol()
	local scale = 3.0	-- more scale -> less symbol
	
	local x1, y1 = -128/scale, 128/scale
	local x2, y2 = -x1, -y1
	
	local verts			= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords	= {1664, 1152}

	return DrawUniqueTSDSymbol( verts, tex_coords, scale, {{"TSD_SpotReport_Draw"}} )
end
-----------------------------------------------------------
function AddAirfireMissionSymbol( pos, parent, controllers, material )
	local scale = 3.0	-- more scale -> less symbol
	
	local x1, y1 = -128/scale, 128/scale
	local x2, y2 = -x1, -y1

	local verts			= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords	= {1280, 1408}

	return DrawUniqueTSDSymbol( verts, tex_coords, scale, {{"TSD_AirfireMission_Draw"}} )
end
-----------------------------------------------------------
function AddAirfireObserverSymbol( pos, parent, controllers, material )
	local scale = 3.5	-- more scale -> less symbol
	
	local x1, y1 = -125/scale, 121/scale
	local x2, y2 = -x1, -y1
	
	local verts			= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords	= {1664, 1408}

	return DrawUniqueTSDSymbol( verts, tex_coords, scale, {{"TSD_AirfireObserver_Draw"}} )
end
-----------------------------------------------------------
function AddSituationReportSymbol( pos, parent, controllers, material )
	local scale = 3.5	-- more scale -> less symbol
	
	local x1, y1 = -125/scale, 100/scale
	local x2, y2 = -x1, -y1
	
	local verts			= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords	= {1280, 1664}

	return DrawUniqueTSDSymbol( verts, tex_coords, scale, {{"TSD_SituationReport_Draw"}} )
end
-----------------------------------------------------------
function AddCallForFireSymbol( pos, parent, controllers, material )
	local scale = 3.0	-- more scale -> less symbol
	
	local x1, y1 = -128/scale, 128/scale
	local x2, y2 = -x1, -y1
	
	local verts			= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords	= {1664, 1664}

	return DrawUniqueTSDSymbol( verts, tex_coords, scale, {{"TSD_CallForFire_Draw"}} )
end
-----------------------------------------------------------
function AddCheckFireSymbol( pos, parent, controllers, material )
	local scale = 3.0	-- more scale -> less symbol
	
	local x1, y1 = -120/scale, 120/scale
	local x2, y2 = -x1, -y1
	
	local verts			= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords	= {1280, 896}

	return DrawUniqueTSDSymbol( verts, tex_coords, scale, {{"TSD_CheckFire_Draw"}} )
end
-----------------------------------------------------------
function AddJVMFSubscriberSymbol( pos, parent, controllers, material )
	local scale = 3.5	-- more scale -> less symbol
	
	local x1, y1 = -125/scale, 125/scale
	local x2, y2 = -x1, -y1
	
	local verts			= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords	= {1664, 896}

	return DrawUniqueTSDSymbol( verts, tex_coords, scale, {{"TSD_JVMFSubscriber_Draw"}} )
end
-----------------------------------------------------------
function AddTADSFootprintSymbol()
	local TADSLosBase = addPlaceholder("TADSLosBase_PH", {0, 0}, TacticalMapBase.name, {{"TSD_TADSfootprint_Show"}})
	local TADSLineBase = addPlaceholder("TADSLineBase_PH", {0, 0}, TADSLosBase.name)
	local TADSCrossBase = addPlaceholder("TADSCrossBase_PH", {0, 0}, TADSLosBase.name, {{"TSD_TADSfootprint_Cross_Draw"}})

	local width = info_box_line_width
	
	draw_line( {{0,0},{0,0}}, "MFD_BACKGROUND",				TADSLineBase.name, width-1,	"TADSLos_Fon",	{{"TSD_TADSfootprint_Los_Draw"}},								h_clip_relations.COMPARE, DEFAULT_LEVEL )
	draw_line( {{0,0},{0,0}}, IND_MPD_MATERIAL_DARK_GREEN,	TADSLineBase.name, width,	"TADSLos_Elem",	{{"TSD_TADSfootprint_Los_Draw"},{"TSD_TADSfootprint_Color"}},	h_clip_relations.COMPARE, DEFAULT_LEVEL )

	local scale = 3.5	-- more scale -> less symbol
	
	local x1, y1 = -125/scale, 125/scale
	local x2, y2 = -x1, -y1
	
	local verts			= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords	= {1919.5, 895.5}

	DrawTSDTexPoly( verts, tex_coords, scale, nil,	TADSCrossBase.name, IND_MPD_TSD_MATERIAL_BLACK,	nil,	nil,	h_clip_relations.COMPARE, DEFAULT_LEVEL)
	DrawTSDTexPoly( verts, tex_coords, scale, nil,	TADSCrossBase.name, IND_MPD_TSD_SYMBOLS_GREEN,	nil,	nil,	h_clip_relations.COMPARE, DEFAULT_LEVEL)
end
-----------------------------------------------------------
function addTRNCrossSymbol(parent)
	local scale = 2.0	-- more scale -> less symbol
	
	local x1, y1 = -56/scale, 56/scale
	local x2, y2 = -x1, -y1
	
	local verts			= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	local tex_coords	= {319, 704}

	DrawTSDTexPoly( verts, tex_coords, scale, nil,	parent, IND_MPD_TSD_MATERIAL_BLACK)
	DrawTSDTexPoly( verts, tex_coords, scale, nil,	parent, IND_MPD_TSD_SYMBOLS_WHITE)
end	
-----------------------------------------------------------
function AddTRNPointSymbol(name, caption, parent, controllers, with_rings)
	local tp_cc 	= createTextProperty( 28,  "WHITE",	IND_MPD_MATERIAL_WHITE,  "CenterCenter" )
	local tp_cc_inv	= createTextProperty( 28,  "WHITE",	IND_MPD_MATERIAL_WHITE,  "CenterCenter", true )
	
	local TRN_Base = addPlaceholder("TRN_Point_"..name.."_PH", {0, 0}, parent, controllers)

	local caption_controller_name = "TSD_TRN_Cross_"..caption.."_Caption"
	addText(caption, {0, 50}, tp_cc, 		{{caption_controller_name, 0}}, nil, nil, "TRN_Point_"..name.."_Caption", 		TRN_Base.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)
	addText(caption, {0, 50}, tp_cc_inv,	{{caption_controller_name, 1}}, nil, nil, "TRN_Point_"..name.."_InvCaption",	TRN_Base.name, h_clip_relations.COMPARE, TSD_MapUnits_LEVEL)

	if with_rings == true then
		AddIntervisibilityRing("TRN_"..caption.."_Vis_Ring", TRN_Base.name, {{"TSD_TRN_VisRing_Show"},{"TSD_TRN_VisRing_Radius"},{"TSD_TRN_VisRing_Color"}}, IND_MPD_TSD_SYMBOLS_YELLOW)
	end

	addTRNCrossSymbol(TRN_Base.name)
end

-----------------------------------------------------------
function AddTargetReferencePointCursor(parent_name, type_controller)
	-- strictly on TSD BAM page
	local BasePH = addPlaceholder("TargetReferencePointCursor_BasePH", {0,0}, parent_name, {type_controller})
	
	local line_w = 5

	local function add_line( name, material, parent, controllers, width )
		local scale = 2
		local def_width = 5
			
		if width ~= nil then
			scale = def_width / width
		end
		
		local x1, y1 = 0.0, 2007.5*tsd_texture_scale
		local x2, y2 = 1.0, y1
	
		local verts			= { {0, 0}, {0, 0} }
		
		local elem			= CreateElement "ceSimpleLineObject"
		elem.width			= (width or def_width)/scale
		
		elem.tex_params		= {{x1,y1},{x2,y2}, {tsd_texture_scale*scale, tsd_texture_scale*scale }} 
		elem.vertices		= verts
		
		setSymbolCommonProperties( elem, name, nil, parent, controllers, (material or IND_MPD_TSD_SYMBOLS_GREEN) )
		
		Add(elem)
		return elem
	end

	local function add_elem(material, name_suf)
		add_line("TRP_Cursor_frame"..name_suf, 		material, BasePH.name, {{"TSD_BAM_PFZ_RPT_Cursor_DrawLine", 0}}, line_w)
		add_line("TRP_Cursor_CH"..name_suf, 		material, BasePH.name, {{"TSD_BAM_PFZ_RPT_Cursor_DrawLine", 1}}, line_w)
		add_line("TRP_Cursor_CV"..name_suf, 		material, BasePH.name, {{"TSD_BAM_PFZ_RPT_Cursor_DrawLine", 2}}, line_w)
	end

	add_elem(IND_MPD_TSD_MATERIAL_BLACK, "_fon")
	add_elem(IND_MPD_TSD_SYMBOLS_GREEN, "")
end

-----------------------------------------------------------
function draw_PFZ_frame_line_act( name, material, parent, line_controllers, mask_controllers, level, width )
	local DBG_HIDE_MASKS = false
	
	local line_level = 0

	if level ~= nil then
		line_level = level
	end

	if not DBG_HIDE_MASKS then
		local mask_line = draw_PFZ_frame_line( name.."_openmask", "MASK_MATERIAL_PURPLE", parent, mask_controllers, width )
		mask_line.isvisible			= false		-- true for DBG
		mask_line.additive_alpha	= false
		mask_line.change_opacity	= false
		mask_line.h_clip_relation	= h_clip_relations.INCREASE_IF_LEVEL
		mask_line.level				= DEFAULT_LEVEL + line_level
	end
	
	local line1 = draw_PFZ_frame_line( name.."_fon", 	IND_MPD_TSD_MATERIAL_BLACK, parent, line_controllers, width )
	local line2 = draw_PFZ_frame_line( name,			material,					parent, line_controllers, width )
	
	if not DBG_HIDE_MASKS then
		setElemsClipLevel(line1, line_level+1)
		setElemsClipLevel(line2, line_level+1)
	
		--reset
		local close_mask_line = draw_PFZ_frame_line( name.."_closemask", "MASK_MATERIAL_PURPLE", parent, mask_controllers, width )
		close_mask_line.isvisible		= false
		close_mask_line.additive_alpha	= false
		close_mask_line.change_opacity	= false
		close_mask_line.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
		close_mask_line.level			= DEFAULT_LEVEL + line_level
	end
	
	return line
end

-----------------------------------------------------------
function draw_PFZ_frame_line( name, material, parent, controllers, width )
	local scale = 2
	local def_width = 5
		
	if width ~= nil then
		scale = def_width / width
	end
	
	local x1, y1 = 0.0, 2035.5*tsd_texture_scale
	local x2, y2 = 1.0, y1

	local verts			= { {0, 0}, {0, 0} }
	
	local elem			= CreateElement "ceSimpleLineObject"
	elem.width			= (width or def_width)/scale
	
	elem.tex_params		= {{x1,y1},{x2,y2}, {tsd_texture_scale*scale, tsd_texture_scale*scale }} 
	elem.vertices		= verts
	
	setSymbolCommonProperties( elem, name, nil, parent, controllers, (material or IND_MPD_TSD_SYMBOLS_WHITE) )
	
	Add(elem)
	return elem
end

---------------------------------------------------
function draw_NFZ_frame_line( name, material, parent, controllers, width )
	local scale = 2
	local def_width = 5
	
	if width ~= nil then
		scale = def_width / width
	end
	
	local x1, y1 = 0, 2021.5*tsd_texture_scale
	local x2, y2 = 1, y1
		
	local verts			= { {0, 0}, {0, 0} }
	
	local elem			= CreateElement "ceSimpleLineObject"
	elem.width			= (width or def_width)/scale
	
	elem.tex_params		= {{x1,y1},{x2,y2}, {tsd_texture_scale*scale, tsd_texture_scale*scale }} 
	elem.vertices		= verts
	
	setSymbolCommonProperties( elem, name, nil, parent, controllers, (material or IND_MPD_TSD_SYMBOLS_YELLOW) )
	
	Add(elem)
	return elem
end
-----------------------------------------------------------
function draw_NFZ_frame_line_act( name, material, parent, line_controllers, mask_controllers, level, width )
	local DBG_HIDE_MASKS = false
	
	local line_level = 0

	if level ~= nil then
		line_level = level
	end

	if not DBG_HIDE_MASKS then
		local mask_line = draw_PFZ_frame_line( name.."_openmask", "MASK_MATERIAL_PURPLE", parent, mask_controllers, width )
		mask_line.isvisible			= false		-- true for DBG
		mask_line.additive_alpha	= false
		mask_line.change_opacity	= false
		mask_line.h_clip_relation	= h_clip_relations.INCREASE_IF_LEVEL
		mask_line.level				= DEFAULT_LEVEL + line_level
	end
	
	local line1 = draw_NFZ_frame_line( name.."_fon", 	IND_MPD_TSD_MATERIAL_BLACK, parent, line_controllers, width )
	local line2 = draw_NFZ_frame_line( name,			material,					parent, line_controllers, width )
	
	if not DBG_HIDE_MASKS then
		setElemsClipLevel(line1, line_level+1)
		setElemsClipLevel(line2, line_level+1)
	
		--reset
		local close_mask_line = draw_PFZ_frame_line( name.."_closemask", "MASK_MATERIAL_PURPLE", parent, mask_controllers, width )
		close_mask_line.isvisible		= false
		close_mask_line.additive_alpha	= false
		close_mask_line.change_opacity	= false
		close_mask_line.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
		close_mask_line.level			= DEFAULT_LEVEL + line_level
	end
	
	return line
end

---------------------------------------------------
function draw_PFZ_Set(parent)
	local tp_white		= createTextProperty( 28,  "WHITE",	IND_MPD_MATERIAL_WHITE,  "CenterCenter" )
	local tp_white_inv	= createTextProperty( 28,  "WHITE",	IND_MPD_MATERIAL_WHITE,  "CenterCenter", true )
	
	-- experimental
	local fon_material = IND_MPD_TSD_MATERIAL_BLACK		-- IND_MPD_TSD_SYMBOLS_BLACK_CLR
	
	if parent==nil then
		parent=TacticalMapBase.name
	end
	
	local PFZs_PH = addPlaceholder("PFZs_Placeholder", {0,0}, parent, {{"TSD_BAM_PFZs_Show"}})
	
	for i=0, 7 do
		local PFZ_Line_PH = addPlaceholder("PFZ_Line_"..i.."_Placeholder", {0,0}, PFZs_PH.name, {{"TSD_BAM_PFZ_Line_Show",i}})
	
		--					 name,							material,						parent,				controllers
		draw_PFZ_frame_line( "PFZ_FrameLine_"..i.."_fon",	fon_material,					PFZ_Line_PH.name,	{{"TSD_BAM_PFZ_DrawLine",i}} )
		draw_PFZ_frame_line( "PFZ_FrameLine_"..i,			IND_MPD_TSD_SYMBOLS_DARK_WHITE,	PFZ_Line_PH.name,	{{"TSD_BAM_PFZ_DrawLine",i}} )

		local PFZ_IDs_PH = addPlaceholder("PFZ_IDs_Placeholder_"..i, {0,0}, PFZs_PH.name, {{"TSD_BAM_PFZ_Caption_Show",i},{"TSD_BAM_PFZ_Caption_Pos",i}})
		
		-- PFZ
		addText( "PF"..(i+1), {0, 0}, tp_white, {{"TSD_BAM_PFZ_Caption_Text_Show",i,0}}, nil, nil, nil, PFZ_IDs_PH.name, h_clip_relations.COMPARE, DEFAULT_LEVEL)
		-- Selected PFZ
		addText( "PF"..(i+1), {0, 0}, tp_white_inv, {{"TSD_BAM_PFZ_Caption_Text_Show",i,1}}, nil, nil, nil, PFZ_IDs_PH.name, h_clip_relations.COMPARE, DEFAULT_LEVEL)

		-- Subscriber for PFZ
		addText( "L0"..i, {0, 0 + tp_white.height*1.05}, tp_white, {{"TSD_BAM_PFZ_Caption_Text_Show",i,0}, {"TSD_BAM_PFZ_Caption_Subscriber",i}}, nil, nil, nil, PFZ_IDs_PH.name, h_clip_relations.COMPARE, DEFAULT_LEVEL)
		-- Subscriber for Selected PFZ
		addText( "L0"..i, {0, 0 + tp_white.height*1.05}, tp_white_inv, {{"TSD_BAM_PFZ_Caption_Text_Show",i,1}, {"TSD_BAM_PFZ_Caption_Subscriber",i}}, nil, nil, nil, PFZ_IDs_PH.name, h_clip_relations.COMPARE, DEFAULT_LEVEL)

		-- Additional Subscriber for PFZ
		addText( "E0"..i, {0, 0 - tp_white.height*1.05}, tp_white, {{"TSD_BAM_PFZ_Caption_Text_Show",i,0}, {"TSD_BAM_PFZ_Caption_AddSubscriber",i}}, nil, nil, nil, PFZ_IDs_PH.name, h_clip_relations.COMPARE, DEFAULT_LEVEL)
		-- Additional Subscriber for Selected PFZ
		addText( "E0"..i, {0, 0 - tp_white.height*1.05}, tp_white_inv, {{"TSD_BAM_PFZ_Caption_Text_Show",i,1}, {"TSD_BAM_PFZ_Caption_AddSubscriber",i}}, nil, nil, nil, PFZ_IDs_PH.name, h_clip_relations.COMPARE, DEFAULT_LEVEL)
	end

	local PFZ_CurrentFrame_PH = addPlaceholder("PFZ_CurrentFrame_Placeholder", {0,0}, parent, {{"TSD_BAM_PFZ_CurrentLineShow"}})	
	draw_PFZ_frame_line( "PFZ_FrameLine_Current_fon",	fon_material,				PFZ_CurrentFrame_PH.name,	{{"TSD_BAM_FireZone_DrawCurrentLine"}} )
	draw_PFZ_frame_line( "PFZ_FrameLine_Current",		IND_MPD_TSD_SYMBOLS_WHITE,	PFZ_CurrentFrame_PH.name,	{{"TSD_BAM_FireZone_DrawCurrentLine"}} )

	local ActivePFZ_Line_PH = addPlaceholder("ActivePFZ_Line_Placeholder", {0,0}, PFZs_PH.name, {{"TSD_BAM_AcivePFZ_Line_Show"}})
	
	for i=0, 3 do
		draw_PFZ_frame_line_act( "ActivePFZ_"..i,	IND_MPD_TSD_SYMBOLS_WHITE,	ActivePFZ_Line_PH.name,	{{"TSD_BAM_AcivePFZ_Line_Draw", i}}, {{"TSD_BAM_AcivePFZ_Mask_Draw", i}} )
	end
end
---------------------------------------------------
function draw_NFZ_Set(parent)
	local tp_yellow 	= createTextProperty( 28, "YELLOW", IND_MPD_MATERIAL_YELLOW, "CenterCenter" )
	local tp_yellow_inv	= createTextProperty( 28, "YELLOW", IND_MPD_MATERIAL_YELLOW, "CenterCenter", true )

	if parent==nil then
		parent=TacticalMapBase.name
	end
	
	local NFZ_Frame_PH = addPlaceholder("NFZ_Frame_Placeholder", {0,0}, parent, nil)

	for i=0, 7 do
		local NFZ_Base_PH = addPlaceholder("NFZ_Base_Placeholder_"..i, {0,0}, NFZ_Frame_PH.name, {{"TSD_BAM_NFZ_Show",i}})
		
		local NFZ_Inactive_PH = addPlaceholder("NFZ_"..i.."_Inactive_Placeholder", {0,0}, NFZ_Base_PH.name, {{"TSD_BAM_AciveNFZ_Line_Show", i, 0}})
	
		--					 name,							material,					parent,				controllers
		draw_NFZ_frame_line( "NFZ_FrameLine_"..i.."_fon",	IND_MPD_TSD_MATERIAL_BLACK,	NFZ_Inactive_PH.name,	{{"TSD_BAM_NFZ_DrawLine",i}} )
		draw_NFZ_frame_line( "NFZ_FrameLine_"..i,			nil,						NFZ_Inactive_PH.name,	{{"TSD_BAM_NFZ_DrawLine",i}} )

		local ActiveNFZ_Line_PH = addPlaceholder("NFZ_"..i.."_Active_Placeholder", {0,0}, NFZ_Base_PH.name, {{"TSD_BAM_AciveNFZ_Line_Show", i, 1}})
		for j=0, 3 do
			draw_NFZ_frame_line_act( "Active_NFZ_"..i.."_FrameLine_"..j,	nil,	ActiveNFZ_Line_PH.name,	{{"TSD_BAM_AciveNFZ_Line_Draw", i, j}}, {{"TSD_BAM_AciveNFZ_Mask_Draw", i, j}} )
		end

		local NFZ_IDs_PH = addPlaceholder("NFZ_IDs_Placeholder_"..i, {0,0}, NFZ_Base_PH.name, {{"TSD_BAM_NFZ_Caption_Pos",i}})

		-- NFZ
		addText( "NF"..(i+1), {0, 0}, tp_yellow, {{"TSD_BAM_NFZ_Caption_Text",i,0}}, nil, nil, nil, NFZ_IDs_PH.name, h_clip_relations.COMPARE, DEFAULT_LEVEL)
		-- Selected NFZ
		addText( "NF"..(i+1), {0, 0}, tp_yellow_inv, {{"TSD_BAM_NFZ_Caption_Text",i,1}}, nil, nil, nil, NFZ_IDs_PH.name, h_clip_relations.COMPARE, DEFAULT_LEVEL)
	end

	local NewNFZ_Frame_PH = addPlaceholder("NewNFZ_Frame_Placeholder", {0,0}, parent, {{"TSD_BAM_NFZ_New_Show"}})
	draw_NFZ_frame_line( "NewNFZ_FrameLine_fon",	IND_MPD_TSD_MATERIAL_BLACK,	NewNFZ_Frame_PH.name,	{{"TSD_BAM_NFZ_New_DrawLine"}} )
	draw_NFZ_frame_line( "NewNFZ_FrameLine",		IND_MPD_TSD_SYMBOLS_YELLOW,	NewNFZ_Frame_PH.name,	{{"TSD_BAM_NFZ_New_DrawLine"}} )

	local CurrentNFZ_Frame_PH = addPlaceholder("CurrentNFZ_Frame_Placeholder", {0,0}, parent, {{"TSD_BAM_NFZ_CurrentLineShow"}})
	draw_NFZ_frame_line( "NFZ_FrameLine_Current_fon",	IND_MPD_TSD_MATERIAL_BLACK,	CurrentNFZ_Frame_PH.name,	{{"TSD_BAM_FireZone_DrawCurrentLine"}} )
	draw_NFZ_frame_line( "NFZ_FrameLine_Current",		IND_MPD_TSD_SYMBOLS_YELLOW,	CurrentNFZ_Frame_PH.name,	{{"TSD_BAM_FireZone_DrawCurrentLine"}} )
end
---------------------------------------------------
function AddTSDInfoStatusWindows()
	AddCurrentHeadingLabel()
	AddNextWaypointHeadingLabel()
	AddNextWaypointStatusWindow(nil, {{"NAV_Heading_Valid"},{"TSD_Draw_If_PhaseNAV"},{"TSD_WPStatus_Window"}}, InfoWindowsBase.name)
	AddEnduranceStatusWindow()
	AddWindStatusWindow()
	AddCursorLocationWindow()
	AddPresentPositionWindow()
	AddGridStatusLabel()
end
---------------------------------------------------
function AddUniqueTSDSymbols()
	AddMessageToObserverSymbol()
	AddSpotReportSymbol()
	AddAirfireMissionSymbol()
	AddAirfireObserverSymbol()
	AddSituationReportSymbol()
	AddCallForFireSymbol()
	AddCheckFireSymbol()
	AddJVMFSubscriberSymbol()
end
---------------------------------------------------
function drawShotAtSymbol(name, pos, parent, controllerPH, height, width, material)
	pos = pos or {0, 0}
	height = height or 36
	width = width or 25
	material = material or IND_MPD_MATERIAL_GREEN

	local angle = math.floor(math.deg(math.atan(height / width)))
	local hypotenuse = math.floor(math.sqrt(height ^ 2 + width ^ 2))
	local line_width = 4

	local SymbolPH = addPlaceholder(name, pos, parent, controllerPH)

	addFatLine(name.."_line1_bg", 	hypotenuse, line_width, {-width / 2, -height / 2}, 	-90 + angle, SymbolPH.name, nil, "MFD_BACKGROUND")
	addFatLine(name.."_line1", 		hypotenuse, line_width, {-width / 2, -height / 2}, 	-90 + angle, SymbolPH.name, nil, material)
	addFatLine(name.."_line2_bg", 	hypotenuse, line_width, {-width / 2, height / 2}, 	-90 - angle, SymbolPH.name, nil, "MFD_BACKGROUND")
	addFatLine(name.."_line2", 		hypotenuse, line_width, {-width / 2, height / 2}, 	-90 - angle, SymbolPH.name, nil, material)
end
-----------------------------------------------------------
function draw_ring( name, radius, width, parent, controllers, material, segments_count )
	local segmentsN = segments_count or 64

	local circle			= CreateElement "ceCircle"
	setSymbolCommonProperties(circle, name, {0,0}, parent, controllers, material)		-- "MASK_MATERIAL_PURPLE"

	circle.radius			= {radius - width, radius}
	circle.arc				= {0, math.pi * 2}
	circle.segment			= math.pi * 4 / segmentsN
	circle.gap				= math.pi * 4 / segmentsN
	circle.segment_detail	= 4

	circle.dashed			= false
	circle.primitivetype   	= "triangles"
	circle.indices	        = default_box_indices

	Add(circle)
	return circle
end
-----------------------------------------------------------
function AddIntervisibilityRing( name, parent, controllers, material )
	local segmentsN = 64		-- 72 or 84 differs a little
	local radius	= 210
	local width		= 4
	
	draw_ring(name.."_fon", 	radius, width, parent, controllers, IND_MPD_TSD_MATERIAL_BLACK,	segmentsN)
	draw_ring(name, 			radius, width, parent, controllers, material, 					segmentsN)
end
-----------------------------------------------------------
-----------------------------------------------------------
---------------------------------------------------
-- function AddDebugClippingMask(name, parent, level, h_clip_relation)
	-- if(parent==nil) then
		-- parent = InfoWindowsBase.name
	-- end
	
	-- local lvl	= level or 0
	-- local hcr	= h_clip_relation or h_clip_relations.INCREASE_IF_LEVEL
	
	-- local MaskSize = 512 * 1.3
	-- local element	        = CreateElement "ceMeshPoly"
	-- element.name 		    = name.."_BackgroundMask_Dbg"
	-- element.primitivetype   = "triangles"
	-- element.material 	    = "INDICATION_COMMON_BROWN"		--"MFD_TRANSPARENT"		--"MASK_MATERIAL"
	-- element.vertices        = buildBoxVerts(MaskSize, MaskSize, "CenterCenter")
	-- element.indices	     	= default_box_indices
	-- element.blend_mode 	 	= blend_mode.IBM_REGULAR
	-- element.change_opacity  = false
	
	-- element.h_clip_relation = hcr
	-- element.level			= DEFAULT_LEVEL + lvl
	
	-- element.init_pos		= {0,0}
	-- element.parent_element	= parent
	
	-- Add(element)
	-- return element
-- end
---------------------------------------------------

side05 = 30
r = video_area_w/3

function AddOwnshipSymbolCenter( pos,  material )
	local scale = 1.8
	local tsd_texture_scale = 1/2048
	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(80, 160,  "CenterCenter")
	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params	= {128*tsd_texture_scale, 1798*tsd_texture_scale, scale*tsd_texture_scale, scale*tsd_texture_scale} 
	Add(elem)
end

function Border( material, bold, parent, controllers)
	local s = side05*0.8
	local verts = {{-s, s}, {s, s}, {s, -s}, {-s, -s}, {-s, s}}
	local w  = bold and 5.0 or 3.0
	return draw_line( verts, material, parent, w, nil, controllers)
end

function BorderPolygon( material, parent, controllers)
	local s = side05*0.8 - 2
	local pos = {0,0}
	local verts = {{-s, s}, {s, s}, {s, -s}, {-s, -s}, {-s, s}}
	return createPolygon(nil, verts, default_box_indices, pos, parent, controllers, material)
end

function LabelPolygon( material, parent, controllers)
	local pos = {0,0}
	local s_l = side05 * 0.6
	local s_hh = side05 *0.6
	local s_hl = side05*0.1
	local verts = {{-s_l, s_hh}, {s_l, s_hh}, {s_l, -s_hl}, {-s_l, -s_hl}, {-s_l, s_hh}}
	return createPolygon(nil, verts, default_box_indices, pos, parent, nil, IND_MPD_MATERIAL_BLACK)
end


function InnerTrianglePolygon( material, parent, controllers)	
	local pos = {0,0}
	local tr_h = side05 * 0.7
	local verts = { {0, 4}, {-tr_h/2, -tr_h+4}, {tr_h/2, -tr_h+4} }
	local inds	= {0, 1, 2, 0, 2, 1 }
	return createPolygon(nil, verts, inds, pos, parent, controllers, material)
end

function InnerFlyerPolygon( material, parent, controllers)	
	local s_l = side05 * 0.7
	local s_h = side05 * 0.7

	local pos = {0,0}
	local verts = {{-s_l, s_h}, {s_l, s_h}, {s_l, -s_h}, {-s_l, -s_h}, {-s_l, s_h}}
	return createPolygon(nil, verts, default_box_indices, pos, parent, controllers, material)
end

function InnerAsteriskPolygon( material, parent, controllers)	
	local s_l = side05*0.75
	local s_h = side05*0.75

	local pos = {0,0}
	local verts = {{-s_l, s_h}, {s_l, s_h}, {s_l, -s_h}, {-s_l, -s_h}, {-s_l, s_h}}
	return createPolygon(nil, verts, default_box_indices, pos, parent, controllers, material)
end

function InnerTriangle( material, bold, parent, controllers)	
	local tr_h = side05 * 0.7
	local verts = { {0, 4}, {-tr_h/2, -tr_h+4}, {tr_h/2, -tr_h+4}, {0, 4} }
	local w  = bold == true and 6.0 or 3.0
	return draw_line( verts, material, parent, w, nil, controllers)
end

function InnerFlyer( material, bold, parent, controllers)	
	local s_l = side05 * 0.9
	local s_h = side05 * 0.5
	local s_w = side05 * 0.8
	
	--horizont line
	local verts_hor = { { -s_l/2, -3*s_h/4}, { s_l/2, -3*s_h/4} }
	local w  = bold == true and 6.0 or 3.0
	local line = draw_line( verts_hor, material, parent, w, nil, controllers)
	
	local verts = { {-s_w/2, 0}, { 0, -s_h}, {s_w/2, 0}}
	local w  = bold == true and 7.0 or 4.0
	draw_line( verts, material, line.name, w, nil, controllers)
	return line
end

function InnerAsterisk( material, bold, parent, controllers)	
	local s   = side05 * 0.9
	local s_w = side05 * 0.8
	
	local verts_hor = { {-s/2, 0}, {s/2, 0} }
	local w  = bold == true and 4.0 or 2.0
	local line = draw_line( verts_hor, material, parent, w, nil, controllers)
	
	local verts_vert = { {0, s/2 }, {0, -s/2} }
	local w  = bold == true and 4.0 or 2.0
	local line = draw_line( verts_vert, material, parent, w, nil, controllers)
	
	local verts_diag = { {-s_w/2, -s_w/2}, {s_w/2, s_w/2} }
	local w  = bold == true and 4.0 or 2.0
	local line = draw_line( verts_diag, material, parent, w, nil, controllers)
	
	local verts_diag1 = { {-s_w/2, s_w/2}, {s_w/2, -s_w/2} }
	local w  = bold == true and 4.0 or 2.0
	local line = draw_line( verts_diag1, material, parent, w, nil, controllers)
end

function createPolygon(name, vertices, indices, pos, parent, controllers, material)
	local polygon				= addMesh(name, vertices, indices, pos, "triangles", parent, controllers)
	polygon.additive_alpha		= false
	polygon.change_opacity		= false

	if material ~= nil then
		polygon.material		= material
	end

	return polygon
end

function Emitter( num, parent_name, controllers, level )
	local ph 		= addPlaceholder(nil, {0,0}, parent_name, controllers)	
--	ph.level		= DEFAULT_LEVEL + 1
	
	if level ~= nil then
		ph.level = level
	end
	ph.h_clip_relation = h_clip_relations.DECREASE_IF_LEVEL
	
	local border_poligon		= BorderPolygon(IND_MPD_MATERIAL_BLACK, ph.name, nil)
	local label_polygon			= LabelPolygon(IND_MPD_MATERIAL_BLACK, ph.name, nil)
	
	local border_bold_yellow	= Border(IND_MPD_MATERIAL_YELLOW, false, ph.name, nil)

	local triangle_bold_black	= InnerTrianglePolygon(IND_MPD_MATERIAL_BLACK, ph.name, nil) 
	local triangle_bold_yellow	= InnerTriangle(IND_MPD_MATERIAL_YELLOW, true, triangle_bold_black.name, nil) 
	
	local flyer_bold_black		= InnerFlyerPolygon(IND_MPD_MATERIAL_BLACK, ph.name, nil) 
	local flyer_bold_yellow		= InnerFlyer(IND_MPD_MATERIAL_YELLOW, true, flyer_bold_black.name, nil) 
	
	local asterisk_bold_black	= InnerAsteriskPolygon(IND_MPD_MATERIAL_BLACK, ph.name, nil) 
	local asterisk_bold_yellow	= InnerAsterisk(IND_MPD_MATERIAL_YELLOW, true, asterisk_bold_black.name, nil) 
	
	local triangle_black 		= InnerTrianglePolygon(IND_MPD_MATERIAL_BLACK, ph.name, nil) 
	local triangle_yellow		= InnerTriangle(IND_MPD_MATERIAL_YELLOW, false, triangle_black.name, nil) 
	
	local flyer_black 			= InnerFlyerPolygon(IND_MPD_MATERIAL_BLACK, ph.name, nil) 
	local flyer_yellow 			= InnerFlyer(IND_MPD_MATERIAL_YELLOW, false, flyer_black.name, nil) 
	
	local asterisk_black 		= InnerAsteriskPolygon(IND_MPD_MATERIAL_BLACK, ph.name, nil) 
	local asterisk_yellow 		= InnerAsterisk(IND_MPD_MATERIAL_YELLOW, false, asterisk_black.name, nil) 
	
	local triangle_black_p 		= InnerTrianglePolygon(IND_MPD_MATERIAL_BLACK, ph.name, nil) 
	local triangle_yellow_p		= InnerTriangle(IND_MPD_MATERIAL_DARK_YELLOW, false, triangle_black_p.name, nil) 
	
	local flyer_black_p 		= InnerFlyerPolygon(IND_MPD_MATERIAL_BLACK, ph.name, nil) 
	local flyer_yellow_p 		= InnerFlyer(IND_MPD_MATERIAL_DARK_YELLOW, false, flyer_black_p.name, nil) 
	
	local asterisk_black_p 		= InnerAsteriskPolygon(IND_MPD_MATERIAL_BLACK, ph.name, nil) 
	local asterisk_yellow_p 	= InnerAsterisk(IND_MPD_MATERIAL_DARK_YELLOW, false, asterisk_black_p.name, nil) 
	
	local left					= addString( "X", { -side05 * 0.35, 0 }, tp_18_yellow, nil, nil, nil, nil, ph.name )
	local right					= addString( "X", {  side05 * 0.35, 0 }, tp_18_yellow, nil, nil, nil, nil, ph.name )
end
-----------------------------------------------------------
function AddAseRect(always_draw)
	local line_width = 5											-- *** key parameter ***
	local w05 = display_size_pix/2 * 0.72							-- *** key parameter ***
	local h05 = w05
	
	local AseDrawController = {"TSD_DrawAseRect"}
	
	if (always_draw ~= nil) and (always_draw ~= false) then
		AseDrawController = nil
	end
	
	local aseRect_ph	= addPlaceholder("AseRect_PH", {0, 0}, TacticalMapBase.name, {AseDrawController,{"NAV_Heading_Valid"}}) --,{"TSD_OwnshipPos_Move"}
	local aseEmitter_ph	= addPlaceholder("Emitter_PH", {0, 0}, TacticalMapBase.name, {{"NAV_Heading_Valid"}}) 
	
	local verts = {{-w05, h05}, {w05, h05}, {w05, -h05}, {-w05, -h05}, {-w05, h05}}
	draw_line( verts, IND_MPD_MATERIAL_GREEN, aseRect_ph.name, line_width, nil, nil)
	
	for i = 6, 0, -1 do
		Emitter(i, aseEmitter_ph.name, {{"TSD_RWR_Thread", i, display_size_pix }} )
		draw_dotted_line( {{0.0, 0.0}, { 0.5*r, 0.0 }}, IND_MPD_MATERIAL_YELLOW, nil, 3, nil, {{"TSD_RWR_LaunchLine", i, GetScale(), GetScale()*display_size_pix}})
	end
end

function AddFcrCone()
	local ConePH = addPlaceholder("FCRCone_PH", {0, 0}, OwnshipBase.name, {{"TSD_FCR_Cone_Root"}, {"TSD_FCR_OnOffShowFootpringFCR"}})
	local width = 3
	draw_line( {{0,0},{0,0}}, "MFD_BACKGROUND", 		ConePH.name, width,			"FCR_Cone_Bg",					{{"TSD_FCR_Cone_Line"}}, h_clip_relations.COMPARE, DEFAULT_LEVEL )	
	draw_line( {{0,0},{0,0}}, IND_MPD_MATERIAL_GREEN,	ConePH.name, width-1,		"FCR_Cone_Elem",				{{"TSD_FCR_Cone_Line"}, {"TSD_FCR_Cone_Color"}}, h_clip_relations.COMPARE, DEFAULT_LEVEL )
	local centerLineLen = 15
	draw_line( {{0,0},{0,0}}, "MFD_BACKGROUND", 		ConePH.name, width,			"FCR_Cone_Center_LineBg",		{{"TSD_FCR_Cone_CenterLine", centerLineLen}}, h_clip_relations.COMPARE, DEFAULT_LEVEL )	
	draw_line( {{0,0},{0,0}}, IND_MPD_MATERIAL_GREEN,	ConePH.name, width-1,		"FCR_Cone_Center_LineElem",		{{"TSD_FCR_Cone_CenterLine", centerLineLen}, {"TSD_FCR_Cone_Color"}}, h_clip_relations.COMPARE, DEFAULT_LEVEL )
end

function NtsAntsSymbols()
	local scaleCoefficient 			= 0.75
	local multiplierCoefficient 	= 1.3
	local symbolSize 				= 50
	local parentName 				= TacticalMapBase.name
	local mask_level 				= 0
	
	local NtsFrameSize 		= {61 * multiplierCoefficient,61 * multiplierCoefficient}
	local AntsFrameSize 	= {63 * multiplierCoefficient,85 * multiplierCoefficient}
	
	local NtsControllers 	= {{"TSD_FCR_NTS_Show"},{"TSD_FCR_NTS_SetPosition"}, {"TSD_FCR_OnOffShowNTS_ANTS" }}
	local AntsControllers 	= {{"TSD_FCR_ANTS_Show"},{"TSD_FCR_ANTS_SetPosition"}, {"TSD_FCR_OnOffShowNTS_ANTS" }}

	local function add_mask(name, parent, controllers, h_clip_relations, level, isvisible)
		local size	= NtsFrameSize[1]
		local s05	= size * 0.5

		local verts = { {-s05, 0}, {0,  s05}, { s05,  0}, { 0, -s05} }

		local mask = addMesh(name, verts, default_box_indices, {0, 0}, "triangles", parent, controllers, "MASK_MATERIAL_PURPLE")
		mask.isvisible			= isvisible		-- true for DBG
		mask.additive_alpha		= false
		mask.change_opacity		= false
		mask.h_clip_relation	= h_clip_relations
		mask.level				= level
	end

	add_mask("TSD_FCR_ANTS_openmask", parentName, NtsControllers, h_clip_relations.INCREASE_IF_LEVEL, DEFAULT_LEVEL + mask_level, false)

	local secondTargetBlack = buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_BLACK,		AntsFrameSize, {0, 0}, "CenterCenter", 512, {160.5,19}, scaleCoefficient, parentName, AntsControllers)
	local secondTarget 		= buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_YELLOW,	AntsFrameSize, {0, 0}, "CenterCenter", 512, {160.5,19}, scaleCoefficient, parentName, AntsControllers)
	Add(secondTargetBlack)
	Add(secondTarget)

	add_mask("TSD_FCR_ANTS_closemask", parentName, NtsControllers, h_clip_relations.REWRITE_LEVEL, DEFAULT_LEVEL + mask_level, false)

	local SecondBase = addPlaceholder("TSD_FCR_ANTS_BasePH", {0, 0}, parentName, AntsControllers)
    addMapTacticalSymbol("FCRTargetSecondTarget",	fontPrefix.."FCR_Target_Symbol",    {{"TSD_FCR_ANTS_SetType"}},	SecondBase.name,	{0, 0}, symbolSize, "k")

	local FirstBase 		= addPlaceholder("TSD_FCR_NTS_BasePH", 			{0, 0}, parentName, 		NtsControllers)
	local FirstSolidBase 	= addPlaceholder("TSD_FCR_NTS_Solid_BasePH", 	{0, 0}, FirstBase.name, 	{{"TSD_FCR_NTS_FrameShow", 1}})
	local FirstDashedBase 	= addPlaceholder("TSD_FCR_NTS_Dashed_BasePH", 	{0, 0}, FirstBase.name, 	{{"TSD_FCR_NTS_FrameShow", 2}})

	local firstTargetNoSolidBlack 	= buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_BLACK,		NtsFrameSize, {0,0}, "CenterCenter", 512, {227,32}, scaleCoefficient, FirstDashedBase.name)
	local firstTargetNoSolid 		= buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_YELLOW, 	NtsFrameSize, {0,0}, "CenterCenter", 512, {227,32}, scaleCoefficient, FirstDashedBase.name)
	Add(firstTargetNoSolidBlack)
	Add(firstTargetNoSolid)

	local firstTargetSolidBlack 	= buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_BLACK, 	NtsFrameSize, {0,0}, "CenterCenter", 512, {96,32}, scaleCoefficient, FirstSolidBase.name)
	local firstTargetSolid 			= buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_YELLOW, 	NtsFrameSize, {0,0}, "CenterCenter", 512, {96,32}, scaleCoefficient, FirstSolidBase.name)
	Add(firstTargetSolidBlack)
	Add(firstTargetSolid)

	addMapTacticalSymbol("FCRTargetFirstTarget",	fontPrefix.."FCR_Target_Symbol",    {{"TSD_FCR_NTS_Target"}},	FirstBase.name,		{0, 0}, symbolSize, "h")
end



