dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN RKT INVENTORY PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T1, "CHAN",	nil },
	{ pb.T2, "ASE",			tp_default_border, 	{{"MFD_AsePrevBorder", 0}} },
	{ pb.T4, "CODE",	nil },
	{ pb.T5, "COORD",	nil },
	{ pb.T6, "UTIL",	tp_default_border },
	
	{ pb.B1, "WPN",		nil },
	{ pb.B6, "LOAD",	tp_default_border },
}

local Controls = {}
Controls = 
{
	--{ "",
	--		{ 
				-- WPN_RKT_WARHEAD_TYPES[1]	== "NA" - no such option on this page
				{ pb.L1, WPN_RKT_WARHEAD_TYPES[2],	tp_default_border,	{{"WPN_RKT_INV_Zone_Warhead_Selection",1}} }, 
				{ pb.L2, WPN_RKT_WARHEAD_TYPES[3],	tp_default_border,	{{"WPN_RKT_INV_Zone_Warhead_Selection",2}} },
				{ pb.L3, WPN_RKT_WARHEAD_TYPES[4],	tp_default_border,	{{"WPN_RKT_INV_Zone_Warhead_Selection",3}} },
				{ pb.L4, WPN_RKT_WARHEAD_TYPES[5],	tp_default_border,	{{"WPN_RKT_INV_Zone_Warhead_Selection",4}} },
				{ pb.L5, WPN_RKT_WARHEAD_TYPES[6],	tp_default_border,	{{"WPN_RKT_INV_Zone_Warhead_Selection",5}} },
				{ pb.L6, WPN_RKT_WARHEAD_TYPES[7],	tp_default_border,	{{"WPN_RKT_INV_Zone_Warhead_Selection",6}} },
	--		} 
	--},

	--{ "",
	--		{ 
				{ pb.R1, WPN_RKT_WARHEAD_TYPES[8],	tp_default_border,	{{"WPN_RKT_INV_Zone_Warhead_Selection",7}} }, 
				{ pb.R2, WPN_RKT_WARHEAD_TYPES[9],	tp_default_border,	{{"WPN_RKT_INV_Zone_Warhead_Selection",8}} },
				{ pb.R3, WPN_RKT_WARHEAD_TYPES[10],	tp_default_border,	{{"WPN_RKT_INV_Zone_Warhead_Selection",9}} },
				{ pb.R4, WPN_RKT_WARHEAD_TYPES[11],	tp_default_border,	{{"WPN_RKT_INV_Zone_Warhead_Selection",10}} },
				{ pb.R5, WPN_RKT_WARHEAD_TYPES[12],	tp_default_border,	{{"WPN_RKT_INV_Zone_Warhead_Selection",11}} },
				{ pb.R6, WPN_RKT_WARHEAD_TYPES[13],	tp_default_border,	{{"WPN_RKT_INV_Zone_Warhead_Selection",12}} },
	--		} 
	--},

}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

draw_wide_border_without_caption( pb_props[pb.L1].pos, pb_props[pb.L6].pos, tp_def_left, nil, nil, nil, nil, TRANSPARENT_BACKGROUND)
draw_wide_border_without_caption( pb_props[pb.R1].pos, pb_props[pb.R6].pos, tp_def_right, nil, nil, nil, nil, TRANSPARENT_BACKGROUND)

createMenu( Menu, nil, 1, TRANSPARENT_BACKGROUND )
createControls( Controls, nil, nil, 0, TRANSPARENT_BACKGROUND )

local tps_cc = createTextProperty( nil, nil, nil, "CenterCenter" )
local tps_rc = createTextProperty( nil, nil, nil, "RightCenter" )

local Lbl_W, Lbl_H = tp_default.width*7.5, tp_default.height*1.4

AddRoundCornersWindow("LOAD_RKT_INV_Zone_Lbl", {0, pb_props[pb.L1].pos[2]+tp_default.height*0.0}, Lbl_W, Lbl_H,
						{
							{"ZONE",	{-tp_default.width*1.0,				0},	tps_cc,	nil},
							{"A",		{Lbl_W/2 - tp_default.width*0.9,	0},	tps_rc,	{{"WPN_RKT_INV_Zone_Selection"}}, {"A","B","C","D","E"}},
						},
						tp_default, IND_MPD_MATERIAL_GREEN, CommonWPNPlaceholder.name, nil)	

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------
