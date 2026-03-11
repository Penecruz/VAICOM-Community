dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T6, "UTIL",tp_default_border,	nil },
}

createMenu( Menu )


local controlsMain = {}
controlsMain=
{
	{ pb.L2, "}RFI",		nil,	{{"WPN_UTIL_RFI_Enable"}},		 	{"{RFI",	"}RFI"}		},
	{ pb.L3, "}FCR",		nil,	{{"WPN_UTIL_FCR_Enable", 0}},		 {"{FCR",	"}FCR"}		},

	{ pb.L4, {{"FCR LETHAL RANGES", nil, nil}, 	{"DEFAULT", tp_default_border, nil}}},
	{ pb.L5, {{"ELEV", 		nil, 	nil}, 		{"AUTO", tp_default_border, {{"FCR_AG_SetControlElevation"}}, {"AUTO","MAN"}}}},
	{ pb.L6, {{"TERRAIN", 	nil, 	nil}, 		{"AUTO", tp_default_border,nil}}},

	{ pb.R1, "RFI TRAIN",	nil,	nil},
	{ pb.R2, {{"RFI MODE", 	nil, 	nil}, 		{"HOSTILE", tp_default_border, nil}}},
	{ pb.R3, "FCR STOW",	tp_default_border,	{{ "WPN_UTIL_FCR_STOW_Frame" }}},
	{ pb.R6, {{"MMA", 		nil, 	nil}, 		{"PINNED",	tp_default_border, {{"WPN_UTIL_MMA_Selection"}}, {"PINNED", "NORM"}} } },

	{ pb.B1, "FCR", 		nil,	nil},
}


local controlsPriority = {}
controlsPriority=
{
	{ pb.R4, {{"SCHEME",  tp_default, nil}, 	{" ", tp_default_border, {{"FCR_AG_SetMissionPriority"}}, {"A", "B", "C"}}}},
	{ pb.R5, {{"MISSION", tp_default, nil}, 	{"1", tp_default_border}}},
}

local GTM_PH = addPlaceholder("GTM PH", nil, nil, {{"DSPLS_FCR_GTM_Root"}})

local pos_up			= pb_props[pb.R4].pos
local pos_dn			= {pb_props[pb.R5].pos[1], pb_props[pb.R5].pos[2] - 20}
draw_border_with_caption(pos_up, pos_dn, 7, 2, "PRIORITY", pb_props[pb.R5].tp, GTM_PH.name, false)

createControls( controlsMain )
createControls( controlsPriority, nil, GTM_PH.name )