dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

local Menu = {}
Menu = 
{ 			
	{ pb.B1, "COM",		nil },	
	{ pb.B4, "NET",		tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.B4}} },
	{ pb.R6, "MODEM",	tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.R6}}},
	{ pb.B6, {{"PRESET", nil },{"EDIT", nil }}  }
}

createMenu( Menu )

addBorder( pb.B6, 100, {{"MPD_COM_BORDER", pb.B6}}, 0, IND_MPD_MATERIAL_DARK_GREEN )

local pos_up_l	= pb_props[pb.L1].pos	
local pos_dn_l = {pb_props[pb.L4].pos[1], pb_props[pb.L4].pos[2]-pb_props[pb.L1].tp.height*0.9}
draw_border_with_caption( pos_up_l, pos_dn_l, 7, 1, "FM1", pb_props[pb.L1].tp, nil, false )

local pos_up_r	= pb_props[pb.R1].pos	
local pos_dn_r = {pb_props[pb.R4].pos[1], pb_props[pb.R4].pos[2]-pb_props[pb.R1].tp.height*0.9}
draw_border_with_caption( pos_up_r, pos_dn_r, 7, 1, "FM2", pb_props[pb.R1].tp, nil, false )

local Controls = {}
Controls = 
{			
	{ "EDIT", {
				{ pb.T2, "UNIT",	tp_default_border,{{"MPD_COMM_PRESET_EDIT_TYPE", 1}}},
				{ pb.T3, "V/UHF",	tp_default_border,{{"MPD_COMM_PRESET_EDIT_TYPE", 2}}},
				{ pb.T4, "FM",		tp_default_border,{{"MPD_COMM_PRESET_EDIT_TYPE", 3}}},
				{ pb.T5, "HF",		tp_default_border,{{"MPD_COMM_PRESET_EDIT_TYPE", 4}}}
			}
	},
	-- FM1
	{ pb.L1, { 
				{"MODE",	nil}, 
				{"CIPHER", tp_default_border, {{"MPD_COM_PRESET_EDIT_FM_MODE", 1}}, {"CIPHER","PLAIN"} } 
			}}, 
	{ pb.L2, { 
				{"CNV",	nil}, 
				{"", tp_default_border, {{"MPD_COM_PRESET_EDIT_FM_CNV", 1}} } 
			}},
	{ pb.L3, { 
				{"HOPSET",	nil}, 
				{"?", tp_default_border, {{"MPD_COM_PRESET_EDIT_FM_HOPSET", 1}}, nil } 
			}},
	{ pb.L4, { 
				{"FREQ>",	nil}, 
				{"45.000", tp_default_border, {{"MPD_COM_PRESET_EDIT_FM_FREQ", 1}}, nil } 
			}},
	{ pb.L5, "DATA SWAP",		nil },	
	-- FM2
	{ pb.R1, { 
				{"MODE",	nil}, 
				{"CIPHER", tp_default_border, {{"MPD_COM_PRESET_EDIT_FM_MODE", 2}}, {"CIPHER","PLAIN"} } 
			}}, 
	{ pb.R2, { 
				{"CNV",	nil}, 
				{"6", tp_default_border, {{"MPD_COM_PRESET_EDIT_FM_CNV", 2}} } 
			}},
	{ pb.R3, { 
				{"HOPSET",	nil}, 
				{"?", tp_default_border, {{"MPD_COM_PRESET_EDIT_FM_HOPSET", 2}}, nil } 
			}},
	{ pb.R4, { 
				{"FREQ>",	nil}, 
				{"41.000", tp_default_border, {{"MPD_COM_PRESET_EDIT_FM_FREQ", 2}}, nil } 
			}},	
}
createControls( Controls )
addPresetEdit_StatusWindow("MFD_COM_PRESET_Status_WindowEdit")
