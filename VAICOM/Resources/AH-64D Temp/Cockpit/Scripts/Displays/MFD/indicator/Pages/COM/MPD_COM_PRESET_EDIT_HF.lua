dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

local Menu = {}
Menu = 
{ 			
	{ pb.B1, "COM",		nil },	
	{ pb.B4, "NET",		tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.B4}} },
	{ pb.R6, "MODEM",	tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.R6}}},
	{ pb.B6, {{"PRESET", tp_default_border, {{"MPD_COMM_PRESET_EDIT_LABEL"}} },{"EDIT", tp_default_border}}  }
}

createMenu( Menu )

addBorder( pb.B6, 100, {{"MPD_COM_BORDER", pb.B6}}, 0, IND_MPD_MATERIAL_DARK_GREEN )

local pos_up_l	= pb_props[pb.L1].pos	
--local pos_dn_l = {pb_props[pb.L2].pos[1], pb_props[pb.L2].pos[2]-pb_props[pb.L1].tp.height*0.9}
local pos_dn_l = pb_props[pb.L2].pos
draw_border_with_caption( pos_up_l, pos_dn_l, 7, 1, "CRYPTO", pb_props[pb.L1].tp, nil, false )

local pos_up_rh	= pb_props[pb.R1].pos	
local pos_dn_rh = {pb_props[pb.R2].pos[1], pb_props[pb.R2].pos[2]-pb_props[pb.R1].tp.height*0.9}
draw_border_with_caption( pos_up_rh, pos_dn_rh, 7, 1, "HF RECV", pb_props[pb.R1].tp, nil, false )

local pos_up_rl	= pb_props[pb.R3].pos	
local pos_dn_rl = {pb_props[pb.R4].pos[1], pb_props[pb.R4].pos[2]-10}
draw_border_with_caption( pos_up_rl, pos_dn_rl, 7, 1, "HF XMIT", pb_props[pb.R3].tp, nil, false )

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
				{"CIPHER", tp_default_border, {{"MPD_COM_PRESET_EDIT_HF_MODE", 1}}, {"CIPHER","PLAIN"} } 
			}}, 
	{ pb.L2, { 
				{"CNV",	nil}, 
				{"", tp_default_border, {{"MPD_COM_PRESET_EDIT_HF_CNV", 4}} } 
			}},
	
	{ pb.L3, { 
				{"PRESET CHAN>",	nil}, 
				{"20", tp_default_border, {{"MPD_COM_PRESET_EDIT_HF_PRESET_CHAN"}}, nil } 
			}},
			
	{ pb.L4, { 
				{"ALE NET>",	nil}, 
				{"20", tp_default_border, {{"MPD_COM_PRESET_EDIT_HF_ALE_NET"}}, nil } 
			}},
			
	{ pb.L5, { 
				{"ECCM NET>",	nil}, 
				{"12", tp_default_border, {{"MPD_COM_PRESET_EDIT_HF_ECCM_NET"}}, nil } 
			}},
-------------------------------
	{ pb.R1, { 
				{"FREQ>",	nil}, 
				{"21.000", tp_default_border, {{"MPD_COM_PRESET_EDIT_HF_RECV_FREQ"}}, nil } 
			}},	
	{ pb.R2, { 
				{"EMISSION",	nil}, 
				{"", tp_default_border, {{"MPD_COM_PRESET_EDIT_HF_RECV_EMISSION"}}, { "LSB", "USB", "CW", "AME"  } } 
			}}, 
	{ pb.R3, { 
				{"FREQ>",	nil}, 
				{"21.000", tp_default_border, {{"MPD_COM_PRESET_EDIT_HF_XMIT_FREQ"}}, nil } 
			}},
	{ pb.R4, { 
				{"EMISSION",	nil}, 
				{"", tp_default_border, {{"MPD_COM_PRESET_EDIT_HF_XMIT_EMISSION"}}, { "LSB", "USB", "CW", "AME"  } } 
			}},
	
}
createControls( Controls )
addPresetEdit_StatusWindow("MFD_COM_PRESET_Status_WindowEdit")
