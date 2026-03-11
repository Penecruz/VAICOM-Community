dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_ENG_Symbology.lua")

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
	{ pb.T6, "UTIL",	tp_default_border, {{"MFD_MenuPrevBorder",AIRCRAFT_UTIL}}},
	{ pb.B2, "SYS",		tp_default_border },
}
createMenu( Menu )

local Controls = {}
Controls = 
{
	{ pb.L5, "{ GEN 1",	nil, {{"SYS_GenButton", 1}}, 	{"} GEN 1", "{ GEN 1"}},
	{ pb.L6, "{ GEN 2",	nil, {{"SYS_GenButton", 2}}, 	{"} GEN 2", "{ GEN 2"}},
	{ pb.B1, "ENG",	tp_default_border, {{"MFD_AC_OriginatorFmt"}}, {"ENG", "FLT", "FUEL", "PERF","UTIL"} }
}
createControls( Controls )

local Boxes = {}
	-- first line is required in format: 	{pos_left_up, controllers,  name, parent, h_space,  text_properties,  margins,},
	-- next : 	{{ text, text_properties, controllers, 	formats, 	margins,  	name }, {},{}...} 
	-- margins: left, down, right, up 
Boxes = 
{ 	

	{
		{{-380,400}, nil, nil, nil, nil, nil, { 10,10,15,-10 } },
		{
			{"ENGINE", tp_28, nil,nil,{1,0,0,0}},
			{" 1", tp_28_right_bottom, nil,nil,{330,0,0,0}},
			{" 2", tp_28_right_bottom, nil,nil,{120,0,0,0}} 
		},
		{
			{"OIL PSI", tp_28, nil,nil,{10,0,0,0}},
			{"000", tp_36_right_bottom, {{"EngOilPCI", 1}}, nil, {320,0,0,0}}, 
			{"000", tp_36_right_bottom, {{"EngOilPCI", 2}}, nil, {115,0,0,0}}
		},
		{
			{"NGB PSI", tp_28, nil,nil,{10,0,0,0}},
			{"000", tp_36_right_bottom, {{"SYS_NGBPCI", 1}}, nil, {320,0,0,0}}, 
			{"000", tp_36_right_bottom, {{"SYS_NGBPCI", 2}}, nil, {115,0,0,0}}
		},
		{
			{"NGB TEMP^C", tp_28, nil,nil,{10,0,0,0}},
			{"888", tp_36_right_bottom, {{"SYS_NGBTemp", 1}}, nil, {320,0,0,0}}, 
			{"899", tp_36_right_bottom, {{"SYS_NGBTemp", 2}}, nil, {120,0,0,0}} 
		}
	},
	
	{
		{{-200,150}, nil, nil, nil, nil, nil, { 10,10,15,-10 } },
		{
			{"XMSN OIL ", tp_28, nil,nil,{1,0,0,0}},
			{" 1", tp_28_right_bottom, nil, nil,{250,0,0,0}},
			{" 2", tp_28_right_bottom, nil, nil,{110,0,0,0}} 
		},
		{
			{"PSI   ", tp_28, nil,nil,{10,0,0,0}},
			{"888", tp_36_right_bottom, {{"SYS_XMSNOilPCI", 1}}, nil, {240,0,0,0}}, 
			{"999", tp_36_right_bottom, {{"SYS_XMSNOilPCI", 2}}, nil, {120,0,0,0}}
		},
		{
			{"TEMP^C", tp_28, nil,nil,{10,0,0,0}},
			{"888", tp_36_right_bottom, {{"SYS_XMSNOilTemp", 1}}, nil, {240,0,0,0}}, 
			{"999", tp_36_right_bottom, {{"SYS_XMSNOilTemp", 2}}, nil, {120,0,0,0}} 
		}
	},
	
	{
		{{-280,-30}, nil, nil, nil, nil, nil, { 10,10,15,-10 } },
		{
			{"ECS TEMP^F ", tp_28}
		},
		{
			{"EFAB", tp_28, nil, nil, {20,0,0,0} }, 
			{" L ", tp_28, nil, nil,{120,0,0,0}},
			{" R ", tp_28, nil, nil,{120,0,0,0}},
			{" COCKPIT ", tp_28, nil, nil, {80,0,0,0}}
		},
		{
			{"FWD", tp_28, nil, nil, {40,0,0,0} },
			{"100", tp_36_right_bottom, {{"SYS_EfabFwdTemp", 1}}, nil, {160,0,0,0}}, 
			{"200", tp_36_right_bottom, {{"SYS_EfabFwdTemp", 2}}, nil, {120,0,0,0}},
			{" CPG ", tp_28, nil, nil, {40,0,0,0}}, 
			{"178", tp_36_right_bottom, {{"SYS_CockpitTemp", 2}}, nil, {160,0,0,0}}
		},
		{
			{"AFT", tp_28, nil, nil, {40,0,0,0} },
			{"100", tp_36_right_bottom, {{"SYS_EfabAftTemp", 1}}, nil, {160,0,0,0}}, 
			{"200", tp_36_right_bottom, {{"SYS_EfabAftTemp", 2}}, nil, {120,0,0,0}},
			{" PLT ", tp_28, nil, nil, {40,0,0,0}}, 
			{"178", tp_36_right_bottom, {{"SYS_CockpitTemp", 1}}, nil, {160,0,0,0}}
		}
	},
	
	{
		{{150, 400}, nil, nil, nil, nil, nil, { 10,10,15,-10 } },
		{
			{"HYD PSI", tp_28, nil, nil, {1,0,0,0}}
		},
		{
			{"PRI", tp_28, nil, nil, {10,0,0,0} }, 
			{"3000", tp_36_right_bottom, {{"EngHydPCI", 1}}, nil, {200,0,0,0}}
		},
		{
			{"UTIL", tp_28, nil, nil, {10,0,0,0}}, 
			{"3000", tp_36_right_bottom, {{"EngHydPCI", 2}}, nil, {200,0,0,0} } 
		},
		{
			{"ACC", tp_28, nil, nil, {10,0,0,0}}, 
			{"3000", tp_36_right_bottom, {{"EngHydPCI", 3}}, nil, {200,0,0,0} } 
		}
	},
	{
		{{-170,-270}, nil, nil, nil, nil, nil, { 10,10,15,-10 } },
		{
			{"STAB", tp_28, nil, nil, {1,0,0,0} }, 
			{"POS", tp_28, nil, nil, {90,0,0,0} }, 
			{"NOM", tp_28, nil, nil, {90,0,0,0} }, 
			{"SPD", tp_28, nil, nil, {70,0,0,0} }, 
		},
		{
			{"12", tp_36,{{"SYS_StabPos"}}, nil, {15,0,0,0} }, 
			{"^", tp_36,{{"SYS_StabPosDeg"}}, nil,{40,0,0,0}},
			{"DN", tp_36, {{"SYS_UpDn"}}, {"UP", "DN"},{35,0,0,0}},
			{"115", tp_36, {{"SYS_StabSpeed"}}, nil, {120,0,0,0}}
		}
	}
	
}
createInfoBoxes( Boxes )
draw_line( {{70,-90},{70,-210}}, IND_MPD_MATERIAL_GREEN, nil, 3 )
draw_line( {{80,-90},{80,-210}}, IND_MPD_MATERIAL_GREEN, nil, 3 )

addRoundedBox("engSYS_OIL_PCI_1_Border", { -80,310}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngOilPCI_Border",1}})
addRoundedBox("engSYS_OIL_PCI_2_Border", { 35, 310}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngOilPCI_Border",2}}) 
																	 
addRoundedBox("engSYS_NGB_PCI_1_Border", { -80, 262}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"SYS_NGBPCI_Border",1}})
addRoundedBox("engSYS_NGB_PCI_2_Border", { 35, 262}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"SYS_NGBPCI_Border",2}}) 
																	 
addRoundedBox("engSYS_XMSN_PCI_1_Border", { 20,60}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"SYS_XMSNOilPCI_Border",1}})
addRoundedBox("engSYS_XMSN_PCI_2_Border", {140,60}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"SYS_XMSNOilPCI_Border",2}}) 

addRoundedBox("engSYS_HYD_PRI_Border", { 325, 310}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngHydPCI_Border",1}})
addRoundedBox("engSYS_HYD_UNIL_Border",{ 325, 262}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngHydPCI_Border",2}})
addRoundedBox("engSYS_HYD_ACC_Border", { 325, 214}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngHydPCI_Border",3}})