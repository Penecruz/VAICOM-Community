dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T1, "DTU",			tp_default_border },
	{ pb.B1, "DMS",			nil },
}

createMenu( Menu )


local Controls = {}
Controls = 
{
	{ pb.B4,	"ABORT",	nil,				{{"DMS_DTU_LOAD_Abort"}}},
	
	{ pb.L1,	"MTL",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption",  0, 1}}},	
	{ pb.L6,	"W/S",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption",  1, 1}}},
	{ pb.R2,	"TFL",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption",  2, 1}}},
	{ pb.R4,	"MISC",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption",  3, 1}}},

	{ pb.L1,	"ALL",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption",  4, 1}}},	
	{ pb.L4,	"ALL",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption",  5, 1}}},
	{ pb.L5,	"NF",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption",  6, 1}}},
	{ pb.L6,	"PF",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption",  7, 1}}},	
	{ pb.R5,	"ALL",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption",  8, 1}}},
	{ pb.R6,	"ONE",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption",  9, 1}}},	
	{ pb.T3,	"ALL",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption", 10, 1}}},
	{ pb.T4,	"ONE",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption", 11, 1}}},	
	{ pb.T5,	"ALL",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption", 12, 1}}},
	{ pb.T6,	"ONE",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption", 13, 1}}},
	{ pb.B5,	"ALL",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption", 14, 1}}},
	{ pb.B6,	"ONE",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption", 15, 1}}},		
	
	{ pb.L1,	"ALL",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption", 16, 1}}},
	{ pb.R1,	"RAD",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption", 17, 1}}},
	{ pb.R2,	"DL",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption", 18, 1}}},
	{ pb.R3,	"PRE",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption", 19, 1}}},
	{ pb.R6,	"HF",		tp_default_border,	{{"DMS_DTU_LOAD_ButtonCaption", 20, 1}}},
}

createControls( Controls )


local tp_36_center_top			= createTextProperty( 36,  nil,	nil,  "CenterTop" )
local Lx =   tp_default.width*0.05
local Ly = - tp_default.height*0.3
local Rx =   tp_default.width*0.45
local Ry = - tp_default.height*0.33
local Tx =   tp_default.width*0.25
local Ty =   tp_default.height*0.33
local Bx =   tp_default.width*0.25
local By =   tp_default.height*0.05
addText( "MTL",		{pb_props[pb.L1].pos[1] + Lx, pb_props[pb.L1].pos[2] + Ly}, 	tp_36_left_center,		{{"DMS_DTU_LOAD_ButtonCaption",  0, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "W/S",		{pb_props[pb.L6].pos[1] + Lx, pb_props[pb.L6].pos[2] + Ly}, 	tp_36_left_center,		{{"DMS_DTU_LOAD_ButtonCaption",  1, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "TFL",		{pb_props[pb.R2].pos[1] + Rx, pb_props[pb.R2].pos[2] + Ry}, 	tp_36_right_center,		{{"DMS_DTU_LOAD_ButtonCaption",  2, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "MISC",	{pb_props[pb.R4].pos[1] + Rx, pb_props[pb.R4].pos[2] + Ry}, 	tp_36_right_center,		{{"DMS_DTU_LOAD_ButtonCaption",  3, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )

addText( "ALL",		{pb_props[pb.L1].pos[1] + Lx, pb_props[pb.L1].pos[2] + Ly}, 	tp_36_left_center,		{{"DMS_DTU_LOAD_ButtonCaption",  4, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "ALL",		{pb_props[pb.L4].pos[1] + Lx, pb_props[pb.L4].pos[2] + Ly}, 	tp_36_left_center,		{{"DMS_DTU_LOAD_ButtonCaption",  5, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "NF",		{pb_props[pb.L5].pos[1] + Lx, pb_props[pb.L5].pos[2] + Ly}, 	tp_36_left_center,		{{"DMS_DTU_LOAD_ButtonCaption",  6, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "PF",		{pb_props[pb.L6].pos[1] + Lx, pb_props[pb.L6].pos[2] + Ly}, 	tp_36_left_center,		{{"DMS_DTU_LOAD_ButtonCaption",  7, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "ALL",		{pb_props[pb.R5].pos[1] + Rx, pb_props[pb.R5].pos[2] + Ry}, 	tp_36_right_center,		{{"DMS_DTU_LOAD_ButtonCaption",  8, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "ONE",		{pb_props[pb.R6].pos[1] + Rx, pb_props[pb.R6].pos[2] + Ry}, 	tp_36_right_center,		{{"DMS_DTU_LOAD_ButtonCaption",  9, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "ALL",		{pb_props[pb.T3].pos[1] + Tx, pb_props[pb.T3].pos[2] + Ty}, 	tp_36_center_top,		{{"DMS_DTU_LOAD_ButtonCaption", 10, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "ONE",		{pb_props[pb.T4].pos[1] + Tx, pb_props[pb.T4].pos[2] + Ty}, 	tp_36_center_top,		{{"DMS_DTU_LOAD_ButtonCaption", 11, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "ALL",		{pb_props[pb.T5].pos[1] + Tx, pb_props[pb.T5].pos[2] + Ty}, 	tp_36_center_top,		{{"DMS_DTU_LOAD_ButtonCaption", 12, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "ONE",		{pb_props[pb.T6].pos[1] + Tx, pb_props[pb.T6].pos[2] + Ty}, 	tp_36_center_top,		{{"DMS_DTU_LOAD_ButtonCaption", 13, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "ALL",		{pb_props[pb.B5].pos[1] + Bx, pb_props[pb.B5].pos[2] + By}, 	tp_36_center_bottom,	{{"DMS_DTU_LOAD_ButtonCaption", 14, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "ONE",		{pb_props[pb.B6].pos[1] + Bx, pb_props[pb.B6].pos[2] + By}, 	tp_36_center_bottom,	{{"DMS_DTU_LOAD_ButtonCaption", 15, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )

addText( "ALL",		{pb_props[pb.L1].pos[1] + Lx, pb_props[pb.L1].pos[2] + Ly}, 	tp_36_left_center,		{{"DMS_DTU_LOAD_ButtonCaption", 16, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "RAD",		{pb_props[pb.R1].pos[1] + Rx, pb_props[pb.R1].pos[2] + Ry}, 	tp_36_right_center,		{{"DMS_DTU_LOAD_ButtonCaption", 17, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "DL",		{pb_props[pb.R2].pos[1] + Rx, pb_props[pb.R2].pos[2] + Ry}, 	tp_36_right_center,		{{"DMS_DTU_LOAD_ButtonCaption", 18, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "PRE",		{pb_props[pb.R3].pos[1] + Rx, pb_props[pb.R3].pos[2] + Ry}, 	tp_36_right_center,		{{"DMS_DTU_LOAD_ButtonCaption", 19, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )
addText( "HF",		{pb_props[pb.R6].pos[1] + Rx, pb_props[pb.R6].pos[2] + Ry}, 	tp_36_right_center,		{{"DMS_DTU_LOAD_ButtonCaption", 20, 0},{"DMS_DTU_LOAD_ButtonInv"}},	nil,	nil,	nil,	nil,	nil,	nil,	true )


function AddCaption(text, framewidth, controllers, name, text_controllers, formats)
	local tp_center	= tp_default
	tp_center.alignment = "CenterCenter"		
	AddRoundCornersWindow(name,	{0.0,(pb_props[pb.L1].pos[2] + tp_default.height*1.5)},
							tp_default.width*framewidth, tp_default.height*1.5,
							{
								{text,		{0.0,	0.0}, tp_center, text_controllers, formats},			
							},
							tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	controllers)
end

AddCaption("MASTER LOAD",							13.0, {{"DMS_DTU_LOAD_Caption", 0}}, "Caption_0")
AddCaption("WEAPONS/SIGHTS",						16.0, {{"DMS_DTU_LOAD_Caption", 1}}, "Caption_1")
AddCaption("THRU-FLIGHT",							13.0, {{"DMS_DTU_LOAD_Caption", 2}}, "Caption_2")
AddCaption("MISCELLANEOUS",							15.0, {{"DMS_DTU_LOAD_Caption", 3}}, "Caption_3")

AddCaption("MISSION 1: ALL",						16.0, {{"DMS_DTU_LOAD_Caption", 4}}, "Caption_4",		{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1: ALL",					"MISSION 2: ALL"})
AddCaption("MISSION 1: ALL ZONES",					22.0, {{"DMS_DTU_LOAD_Caption", 5}}, "Caption_5",		{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1: ALL ZONES",				"MISSION 2: ALL ZONES"})
AddCaption("MISSION 1: NO FIRE ZONES",				26.0, {{"DMS_DTU_LOAD_Caption", 6}}, "Caption_6",		{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1: NO FIRE ZONES",			"MISSION 2: NO FIRE ZONES"})
AddCaption("MISSION 1: PRIORITY FIRE ZONES",		32.0, {{"DMS_DTU_LOAD_Caption", 7}}, "Caption_7",		{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1: PRIORITY FIRE ZONES",	"MISSION 2: PRIORITY FIRE ZONES"})
AddCaption("MISSION 1: ALL TARGETS/THREATS",		32.0, {{"DMS_DTU_LOAD_Caption", 8}}, "Caption_8",		{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1: ALL TARGETS/THREATS",	"MISSION 2: ALL TARGETS/THREATS"})
AddCaption("MISSION 1: ONE TARGET/THREAT",			30.0, {{"DMS_DTU_LOAD_Caption", 9}}, "Caption_9",		{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1: ONE TARGET/THREAT",		"MISSION 2: ONE TARGET/THREAT"})
AddCaption("MISSION 1: ALL ROUTES",					23.0, {{"DMS_DTU_LOAD_Caption", 10}}, "Caption_10",		{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1: ALL ROUTES",			"MISSION 2: ALL ROUTES"})
AddCaption("MISSION 1: ONE ROUTE",					22.0, {{"DMS_DTU_LOAD_Caption", 11}}, "Caption_11",		{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1: ONE ROUTE",				"MISSION 2: ONE ROUTE"})
AddCaption("MISSION 1: ALL WAYPOINTS/HAZARDS",		34.0, {{"DMS_DTU_LOAD_Caption", 12}}, "Caption_12",		{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1: ALL WAYPOINTS/HAZARDS",	"MISSION 2: ALL WAYPOINTS/HAZARDS"})
AddCaption("MISSION 1: ONE WAYPOINT/HAZARD",		32.0, {{"DMS_DTU_LOAD_Caption", 13}}, "Caption_13",		{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1: ONE WAYPOINT/HAZARD",	"MISSION 2: ONE WAYPOINT/HAZARD"})
AddCaption("MISSION 1: ALL CONTROL MEASURES",		33.0, {{"DMS_DTU_LOAD_Caption", 14}}, "Caption_14",		{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1: ALL CONTROL MEASURES",	"MISSION 2: ALL CONTROL MEASURES"})
AddCaption("MISSION 1: ONE CONTROL MEASURE",		32.0, {{"DMS_DTU_LOAD_Caption", 15}}, "Caption_15",		{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1: ONE CONTROL MEASURE",	"MISSION 2: ONE CONTROL MEASURE"})

AddCaption("COMMUNICATIONS: ALL",					21.0, {{"DMS_DTU_LOAD_Caption", 16}}, "Caption_16")
AddCaption("COMMUNICATIONS: RADIOS",				24.0, {{"DMS_DTU_LOAD_Caption", 17}}, "Caption_17")
AddCaption("COMMUNICATIONS: DL CONFIG",			28.0, {{"DMS_DTU_LOAD_Caption", 18}}, "Caption_18")
AddCaption("COMMUNICATIONS: PRESETS",				25.0, {{"DMS_DTU_LOAD_Caption", 19}}, "Caption_19")
AddCaption("COMMUNICATIONS: HF RADIO",				26.0, {{"DMS_DTU_LOAD_Caption", 20}}, "Caption_20")


addText( "LOAD STATUS:",		{pb_props[pb.T1].pos[1] - tp_default.width*2.0,	pb_props[pb.L1].pos[2]},		tp_def_left)
draw_line( {{pb_props[pb.T1].pos[1] - tp_default.width*2.0,	 	pb_props[pb.L1].pos[2] - tp_default.height * 1.1},	{pb_props[pb.T1].pos[1] + tp_default.width*9.5,	pb_props[pb.L1].pos[2] - tp_default.height  * 1.1}},		tp_default.material)


function AddLoadStatus( text,	pos,	text_properties,	inv,	controllers_inv,	controllers,	controllers_black,	formats,	margins,	name,	parent )
	if type(inv) == "table" then
		inv[#inv + 1] = 1
		local placeholder_inv = addPlaceholder(name.."_placeholder_inv", nil, parent, {inv})
		--addText( text, pos, text_properties, controllers, formats, margins,  name, parent, h_clip_relation, level, bold, is_transparent )		
		addText( text, pos, text_properties,	controllers_inv, 		formats, margins, name.."_inv",		placeholder_inv.name, nil, nil, true )
		addText( text, pos, tp_black_left,		controllers_black,	formats, margins, name.."_black",	placeholder_inv.name, nil, nil, true, true )
		
		inv[#inv] = 0
		local placeholder = addPlaceholder(name.."_placeholder", nil, parent, {inv})
		addText( text, pos, text_properties, controllers, formats, margins,  name, placeholder.name, nil, nil, false  )
	end
end

AddLoadStatus( "",	{-tp_default.width*7.0,	pb_props[pb.L1].pos[2]},	tp_def_left,	{"DMS_DTU_LOAD_Inverse"},	{{"DMS_DTU_LOAD_StatusInv"}},	{{"DMS_DTU_LOAD_Status"}},	{{"DMS_DTU_LOAD_StatusBlack"}}, {"LOAD IN PROGRESS", "LOAD COMPLETED", "UPLOAD ABORT"},	nil,	"DMS_LOAD_STATUS",	nil)

