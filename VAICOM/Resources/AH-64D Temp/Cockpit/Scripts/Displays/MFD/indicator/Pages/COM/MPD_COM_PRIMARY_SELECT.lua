dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

local Menu = {}
Menu = 
{ 			
	{ pb.B1, "COM",		nil },	
	{ pb.B4, "NET",		tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.B4}} },
	{ pb.B6, {{"PRESET",	nil },{"EDIT", nil }}}
}
createMenu( Menu )
addBorder( pb.B6, 100, {{"MPD_COM_BORDER", pb.B6}}, 0, IND_MPD_MATERIAL_DARK_GREEN )

local Controls = {}
Controls = 
{		
	{ pb.L1, "VHF SC",	tp_default_border,{{"MPD_COMM_PrimarySelect", 1}}},
	{ pb.L2, "UHF HQ",	tp_default_border,{{"MPD_COMM_PrimarySelect", 2}}},
	{ pb.L3, "UHF SC",	tp_default_border,{{"MPD_COMM_PrimarySelect", 3}}},
	{ pb.L4, "FM1 SINC",tp_default_border,{{"MPD_COMM_PrimarySelect", 4}}},
	{ pb.L5, "FM1 SC",	tp_default_border,{{"MPD_COMM_PrimarySelect", 5}}},
	{ pb.L6, "NONE",	tp_default_border,{{"MPD_COMM_PrimarySelect", 6}}},
	
	{ pb.R1, "FM2 SINC",tp_default_border,{{"MPD_COMM_PrimarySelect", 7}}},
	{ pb.R2, "FM2 SC",	tp_default_border,{{"MPD_COMM_PrimarySelect", 8}}},
	{ pb.R3, "HF PRE",	tp_default_border,{{"MPD_COMM_PrimarySelect", 9}}},
	{ pb.R4, "HF ALE",	tp_default_border,{{"MPD_COMM_PrimarySelect", 10}}},
	{ pb.R5, "HF ECCM",	tp_default_border,{{"MPD_COMM_PrimarySelect", 11}}},
	{ pb.R6, "HF SC",	tp_default_border,{{"MPD_COMM_PrimarySelect", 12}}},	
}

draw_wide_border_without_caption( pb_props[pb.L1].pos, pb_props[pb.L6].pos, tp_def_left,	8, nil,	0, 	tp_default.height*0.2,	TRANSPARENT_BACKGROUND)
draw_wide_border_without_caption( pb_props[pb.R1].pos, pb_props[pb.R6].pos, tp_def_right,	8, nil, 0, 	tp_default.height*0.2,	TRANSPARENT_BACKGROUND)

createControls( Controls )

addCOM_NET_PRESET_Status_Window()

local w = tp_default.width*17.0
local w = tp_default.width*9.0	
local h = tp_default.height*2.0
local w0 = 10 - w/2 
local h0 = h/2 - tp_default.steep_y	
AddRoundCornersWindow("COM_NET_Primary",	{ 5, pb_props[pb.L2].pos_c[2]  },
						w, h,
						{
							{"PRIMARY", { 0, h0}, tp_28_center_bottom	, nil},	
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)