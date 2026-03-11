dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

--addText( "DMS VERS PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T1, "DTU",			nil },
	{ pb.T2, "FAULT",		nil },
	{ pb.T3, "IBIT",		nil },
	{ pb.T4, {{"SHUT",	nil },{"DOWN",	nil }}},
	{ pb.T5, "VERS",		tp_default_border },
	{ pb.T6, "UTIL",		nil },
	{ pb.B1, "DMS",			nil },
}

createMenu( Menu )


local Controls = {}
Controls = 
{		
	{ "SUBSYSTEMS", 
				{ 
					{ pb.B2, 	"ACFT/\nCOMM",		tp_default_border,		{{"DMS_VERS_Border", 1}}},
					{ pb.B4, 	"WPN/\nSIGHT",		tp_default_border,		{{"DMS_VERS_Border", 2}}},
					{ pb.B5, 	"PROC/\nDMS",		tp_default_border,		{{"DMS_VERS_Border", 0}}},--default
					{ pb.B6, 	"NAV/\nASE",		tp_default_border,		{{"DMS_VERS_Border", 3}}},	
				}
	},
}

createControls( Controls )


local tp_center	= tp_default
tp_center.alignment = "CenterCenter"	

AddRoundCornersWindow("PageName_1",	{0.0,(pb_props[pb.L1].pos[2] + tp_default.height*2.0)},
						tp_default.width*19.00, tp_default.height*1.5,
						{
							{"SOFTWARE VERSIONS",		{0.0,	0.0}, tp_center},			
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
						

addText( "    LRU          1                2    ",  {-400,	350},		tp_def_left,		nil,	nil,	nil, "Line_0",	nil)						

draw_line( {{-400 + tp_default.width*4.0,	 	346 - tp_default.height},	{-400 + tp_default.width*7.0,	346 - tp_default.height}},		tp_default.material)
draw_line( {{-400 + tp_default.width*10.0,		346 - tp_default.height},	{-400 + tp_default.width*24.0,	346 - tp_default.height}},		tp_default.material)
draw_line( {{-400 + tp_default.width*27.0,		346 - tp_default.height},	{-400 + tp_default.width*41.0,	346 - tp_default.height}},		tp_default.material)

----------------------------------------------------------------------------------------------------------------------------------------------------------------			
local Page_ProcDms = addPlaceholder("placeholder_proc_dms", nil,	nil, {{"DMS_VERS_PageNumber", 0}})--default
						
addText( "     DP   DALOT7003000     DALOT7003000",		{-400,	300},		tp_def_left,		nil,	nil,	nil,	nil,	Page_ProcDms.name)
addText( "     SP   SALOT7003000     SALOT7003000",		{-400,	250},		tp_def_left,		nil,	nil,	nil,	nil,	Page_ProcDms.name)
addText( "     WP   WALOT7001000     WALOT7001000",		{-400,	200},		tp_def_left,		nil,	nil,	nil,	nil,	Page_ProcDms.name)	
----------------------------------------------------------------------------------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------------------------------------------------------------------------------
local Page_AcftComm = addPlaceholder("placeholder_acft_comm", nil,	nil, {{"DMS_VERS_PageNumber", 1}})

addText( "    ECS   14.1             14.1        ",		{-400,	300},		tp_def_left,		nil,	nil,	nil,	nil,	Page_AcftComm.name)
addText( "    ELC   2                2           ",		{-400,	250},		tp_def_left,		nil,	nil,	nil,	nil,	Page_AcftComm.name)
addText( "     FM   H1633 S0404      NOT INSTALLED",	{-400,	200},		tp_def_left,		nil,	nil,	nil,	nil,	Page_AcftComm.name)
----------------------------------------------------------------------------------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------------------------------------------------------------------------------
local Page_WpnSight = addPlaceholder("placeholder_wpn_sight", nil,	nil, {{"DMS_VERS_PageNumber", 2}})

addText( "   TADS   209                          ",		{-400,	300},		tp_def_left,		nil,	nil,	nil,	nil,	Page_WpnSight.name)
addText( "    FCR   P5                           ",		{-400,	250},		tp_def_left,		nil,	nil,	nil,	nil,	Page_WpnSight.name)
addText( "    LEU   021                          ",		{-400,	200},		tp_def_left,		nil,	nil,	nil,	nil,	Page_WpnSight.name)
----------------------------------------------------------------------------------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------------------------------------------------------------------------------
local Page_NavAse = addPlaceholder("placeholder_nav_ase", nil,	nil, {{"DMS_VERS_PageNumber", 3}})

addText( "    FMC   B12F E12E                    ",		{-400,	300},		tp_def_left,		nil,	nil,	nil,	nil,	Page_NavAse.name)
addText( "RWR EID   O30                          ",		{-400,	250},		tp_def_left,		nil,	nil,	nil,	nil,	Page_NavAse.name)
addText( "RWR OFP   23.9                         ",		{-400,	200},		tp_def_left,		nil,	nil,	nil,	nil,	Page_NavAse.name)
addText( "  HIADC   0201                         ",		{-400,	150},		tp_def_left,		nil,	nil,	nil,	nil,	Page_NavAse.name)
----------------------------------------------------------------------------------------------------------------------------------------------------------------

