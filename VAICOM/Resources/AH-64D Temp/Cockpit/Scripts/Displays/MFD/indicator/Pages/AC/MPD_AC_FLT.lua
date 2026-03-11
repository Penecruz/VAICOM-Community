dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_FLT_Symbology.lua")

local heading_scale_Y = 345;

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
	{ pb.T6, "UTIL",	tp_default_border, {{"MFD_MenuPrevBorder",AIRCRAFT_UTIL}}}
}
createMenu( Menu )

local Controls = {}
Controls = 
{
	{ pb.L5, "-W-",	tp_default_border, {{"FLT_BIAS_Border"}}},
	{ pb.B1, "FLT",	tp_default_border, {{"MFD_AC_OriginatorFmt"}}, {"ENG", "FLT", "FUEL", "PERF","UTIL"} },
	{ pb.B6, "SET",		nil }
}
createControls( Controls )

addText( "1.3G", {-440, -70}, tp_36,{{"FLT_G_FLT"}} )

local elem, elem_name = addText( "1237", {400, heading_scale_Y }, tp_36, {{"FLT_SET_BaroInertAltitude"}} ) 
addText( "INRTL", {0, -tp_36.height*1.3 }, tp_36_white, {{"FLT_InertAltitudeEnabled"}},nil,nil,nil,elem_name )

local tas_pos = {pb_props[pb.L5].pos[1]+90, pb_props[pb.L5].pos[2]+10}
addText( "115", tas_pos , tp_24, {{"FLT_StabilizatorTas"}} )
local stabilizator_ph = addRotPlaceholder("stabilizator_ph", {-440,-105}, 0, nil, {{"FLT_Stabilizator"}})
draw_stabilizator( {0,0},  IND_MPD_1024_MATERIAL_WHITE, stabilizator_ph.name,{{"FLT_StabilizatorColor"}} )
draw_stabilizator_scale( {-370,-105},  IND_MPD_1024_MATERIAL_WHITE, nil, {{"FLT_StabilizatorColor"}} )
draw_stabilizator_question( {-370,-105},  IND_MPD_1024_MATERIAL_WHITE, nil, {{"FLT_StabilizatorQuestion"}} )
