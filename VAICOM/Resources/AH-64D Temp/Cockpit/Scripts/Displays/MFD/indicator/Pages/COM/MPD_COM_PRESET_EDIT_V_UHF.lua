dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

local Menu = {}
Menu = 
{ 			
	{ pb.B1, "COM",		nil },	
	{ pb.B4, "NET",		tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.B4}} },
	{ pb.R6, "MODEM",	tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.R6}}},
	{ pb.B6, {{"PRESET",	nil },{"EDIT", nil }}}
}

createMenu( Menu )

addBorder( pb.B6, 100, {{"MPD_COM_BORDER", pb.B6}}, 0, IND_MPD_MATERIAL_DARK_GREEN )

local pos_up	= pb_props[pb.R1].pos	
local pos_dn = {pb_props[pb.R4].pos[1], pb_props[pb.R4].pos[2]-pb_props[pb.R1].tp.height*0.9}
draw_border_with_caption( pos_up, pos_dn, 7, 1, "UHF", pb_props[pb.R1].tp, nil, false )

local Controls = {}
Controls = 
{		
	{ pb.L1, {{"VHF FREQ>",	nil,	nil}, {"",	tp_default_border, {{"MPD_COM_PRESET_EDIT_V_UHF_VHF_FREQ" }}} } },
	{ "EDIT", {
				{ pb.T2, "UNIT",	tp_default_border,{{"MPD_COMM_PRESET_EDIT_TYPE", 1}}},
				{ pb.T3, "V/UHF",	tp_default_border,{{"MPD_COMM_PRESET_EDIT_TYPE", 2}}},
				{ pb.T4, "FM",		tp_default_border,{{"MPD_COMM_PRESET_EDIT_TYPE", 3}}},
				{ pb.T5, "HF",		tp_default_border,{{"MPD_COMM_PRESET_EDIT_TYPE", 4}}}
			}
	},
	
	{ pb.R1, { 
				{"MODE",	nil}, 
				{"CIPHER", tp_default_border, {{"MPD_COM_PRESET_EDIT_V_UHF_MODE", 1}}, {"CIPHER","PLAIN"} } 
			}}, 
	{ pb.R2, { 
				{"CNV",	nil}, 
				{"", tp_default_border, {{"MPD_COM_PRESET_EDIT_V_UHF_CNV"}} } 
			}},
	
	{ pb.R3, { 
				{"HQ NET>",	nil}, 
				{"", tp_default_border, {{"MPD_COM_PRESET_EDIT_V_UHF_HQ_FREQ"}} } 
			}},
			
	{ pb.R4, { 
				{"FREQ>",	nil}, 
				{"", tp_default_border, {{"MPD_COM_PRESET_EDIT_V_UHF_UHF_FREQ"}} } 
			}}
}
createControls( Controls )
addPresetEdit_StatusWindow("MFD_COM_PRESET_Status_WindowEdit")
