dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/Common/MPD_TSD_PointsId.lua")


local Menu = {}
Menu = 
{ 

	{ pb.T6, 	"UTIL",		nil,							nil},
	{ pb.B1, 	"TSD",		nil,							nil},
	{ pb.B5, 	"RTE",		tp_default_darkgreen_border,	nil},
	{ pb.B6, 	"RTM",		tp_default_border,				nil}
}

local Controls = {}
Controls = 
{
	{ pb.T1, 	"ROUTE",		tp_default_border, {{"TSD_RTM_RouteName", 0}}},
	{ pb.T2, 	"ROUTE",		tp_default_border, {{"TSD_RTM_RouteName", 1}}},
	{ pb.T3, 	"ROUTE",		tp_default_border, {{"TSD_RTM_RouteName", 2}}},
	{ pb.T4, 	"ROUTE",		tp_default_border, {{"TSD_RTM_RouteName", 3}}},
	{ pb.T5, 	"ROUTE",		tp_default_border, {{"TSD_RTM_RouteName", 4}}},
	
	{ "RTE", 
				{ 
					{ pb.L4, "NEW",		tp_default_border, {{"TSD_RTM_NewDel", 0}}, {"NEW", "YES"}}, 
					{ pb.L5, "DELETE",	tp_default_border, {{"TSD_RTM_NewDel", 1}}, {"DELETE", "NO"}},
				}
	},
	{ pb.R5, "REVERSE\nROUTE",		nil,	nil},
}

createMenu( Menu )
createControls( Controls )
AddPagingGroup("RTM_B2B3", {{"TSD_RTM_Pages"}})


local function AddRouteWindow(num)

	local curr_pos_y = ((pb_props[pb.T1].pos[2]-pb_props[pb.L1].pos[2])/(12/7) + pb_props[pb.L1].pos[2])
	addText("CURRENT",	{pb_props[pb.T1 + num].pos[1],curr_pos_y},					tp_28_center_bottom,	{{"TSD_RTM_CurrentRoute", num}},	nil,		nil, 		"Current_"..num,	nil)	
	
	local smallfont_size	= 28
	local tp28				= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "CenterCenter" )
	tp28.stringdefs			= {smallfont_size*GetScale(),smallfont_size*GetScale(),smallfont_size*GetScale()*0.05}
	local lbl_W, lbl_H		= tp_default.width*5.50, tp_default.height*12.0
	local lbl_pos_y			= 1.1*pb_props[pb.L2].pos[2]
	AddRoundCornersWindow("WaypointsRoute"..num,	{pb_props[pb.T1 + num].pos[1], lbl_pos_y},	lbl_W,	lbl_H,
						{
							{"WP00",		{0.0,	tp_default.height*5.0},		tp28,	{{"TSD_RTM_PointTypeAndNumber", 6 * num + 0}}, WP_HZ_CM_Types},
							{"WP00",		{0.0,	tp_default.height*3.0},		tp28,	{{"TSD_RTM_PointTypeAndNumber", 6 * num + 1}}, WP_HZ_CM_Types},
							{"WP00",		{0.0,	tp_default.height*1.0},		tp28,	{{"TSD_RTM_PointTypeAndNumber", 6 * num + 2}}, WP_HZ_CM_Types},
							{"WP00",		{0.0,	tp_default.height*-1.0},	tp28,	{{"TSD_RTM_PointTypeAndNumber", 6 * num + 3}}, WP_HZ_CM_Types},
							{"WP00",		{0.0,	tp_default.height*-3.0},	tp28,	{{"TSD_RTM_PointTypeAndNumber", 6 * num + 4}}, WP_HZ_CM_Types},
							{"WP00",		{0.0,	tp_default.height*-5.0},	tp28,	{{"TSD_RTM_PointTypeAndNumber", 6 * num + 5}}, WP_HZ_CM_Types},			
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
end	
	
for i=0,4 do
AddRouteWindow(i)
end
