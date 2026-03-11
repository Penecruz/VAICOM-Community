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
			{ pb.T4, 	"NONE",		tp_default_border,	nil},
			{ pb.L1, 	"ROUTE",		nil,	{{"DMS_NoBorder"},{"DMS_DTU_ROUTE", 0}}},
			{ pb.L2, 	"ROUTE",		nil,	{{"DMS_NoBorder"},{"DMS_DTU_ROUTE", 1}}},
			{ pb.L3, 	"ROUTE",		nil,	{{"DMS_NoBorder"},{"DMS_DTU_ROUTE", 2}}},
			{ pb.L4, 	"ROUTE",		nil,	{{"DMS_NoBorder"},{"DMS_DTU_ROUTE", 3}}},
			{ pb.L5, 	"ROUTE",		nil,	{{"DMS_NoBorder"},{"DMS_DTU_ROUTE", 4}}},
			{ pb.R1, 	"ROUTE",		nil,	{{"DMS_NoBorder"},{"DMS_DTU_ROUTE", 5}}},
			{ pb.R2, 	"ROUTE",		nil,	{{"DMS_NoBorder"},{"DMS_DTU_ROUTE", 6}}},
			{ pb.R3, 	"ROUTE",		nil,	{{"DMS_NoBorder"},{"DMS_DTU_ROUTE", 7}}},
			{ pb.R4, 	"ROUTE",		nil,	{{"DMS_NoBorder"},{"DMS_DTU_ROUTE", 8}}},
			{ pb.R5, 	"ROUTE",		nil,	{{"DMS_NoBorder"},{"DMS_DTU_ROUTE", 9}}},
}

createControls( Controls )


local x1 = -video_area_w_05
local x2 = pb_props[pb.T1].pos[1] - tp_default.width*1.7
local x3 = pb_props[pb.T6].pos[1] + tp_default.width*3.0
local x4 = video_area_w_05
local y1 = pb_props[pb.L1].pos[2] + tp_default.height*1.5
local y2 = pb_props[pb.L5].pos[2] - tp_default.height*2.5
local verts_1 = {{x1, y1}, {x2, y1}, {x2, y2}, {x1, y2}}
local verts_2 = {{x4, y1}, {x3, y1}, {x3, y2}, {x4, y2}}
draw_line( verts_1, tp_default.material, nil, 3, "Route_Border_1", nil)
draw_line( verts_2, tp_default.material, nil, 3, "Route_Border_2", nil)


AddRoundCornersWindow("OneRouteWindow",	{0.0, pb_props[pb.L3].pos[2]},
						tp_default.width*12.0, tp_default.height*4.5,
						{
							{"MISSION 1",		{0.0,	tp_default.height*0.5},							tp_28_center_bottom,			{{"DMS_DTU_MsnPage"}}, {"", "MISSION 1", "MISSION 2"}},
							{"ONE ROUTE",		{0.0,	-tp_default.height*1.5},		tp_28_center_bottom},						
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)