dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T1, "DTU",			tp_default_border },
	{ pb.B1, "DMS",			nil },
}

createMenu( Menu )

local tp_center	= tp_default
tp_center.alignment = "CenterCenter"
local tp_def_left_center_white			= createTextProperty( nil, "WHITE",	IND_MPD_MATERIAL_WHITE, "LeftCenter" )	

AddRoundCornersWindow("CardStatusWindow",	{0.0,((pb_props[pb.L1].pos[2] + pb_props[pb.L2].pos[2]) / 2 - tp_default.height)},
						tp_default.width*19.00, tp_default.height*7.5,
						{
							{"CARD STATUS",				{0.0,		80.0},		tp_center},
							{"f1 MSN- CROSSBOW",		{-150.0,	40.0},		tp_def_left_center_white,		{{"DMS_DTU_Msn", 0}}},
							{"f2 MAP- SWUSA",			{-150.0,	0.0},		tp_def_left_center_white,		{{"DMS_DTU_Map", 0}}},
							{" 3  ? -",					{-150.0,	-40.0},		tp_def_left_center,				{{"DMS_DTU_Msn", 1}}},
							{" 4  ? -",					{-150.0,	-80.0},		tp_def_left_center,				{{"DMS_DTU_Map", 1}}},							
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)