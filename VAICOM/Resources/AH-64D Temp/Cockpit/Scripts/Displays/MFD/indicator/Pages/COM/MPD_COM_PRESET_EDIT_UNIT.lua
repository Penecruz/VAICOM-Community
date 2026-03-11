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

local Controls = {}
Controls = 
{		
	{ pb.L1, {{"UNIT ID>",		nil,	nil}, {"",	tp_default_border, {{"MFD_COMM_UNIT_ID", pb.L1 }}} } },
	{ pb.L2, {{"CALL SIGN>",	nil,	nil}, {"",	tp_default_border, {{"MFD_COMM_CallSign", pb.L2}}} } },
	{ pb.L4, {
				{"PRIMARY",	nil,	nil}, 
				{"", tp_default_border, {{"MPD_COM_PRESET_EDIT_PRIMARY_RADIO_BTN"}}, { "NONE", "VHF", "UHF SC", "FM1 SC", "FM2 SC", "HF SC"} } 
			} 
	},
	{ "EDIT", {
				{ pb.T2, "UNIT",	tp_default_border,{{"MPD_COMM_PRESET_EDIT_TYPE", 1}}},
				{ pb.T3, "V/UHF",	tp_default_border,{{"MPD_COMM_PRESET_EDIT_TYPE", 2}}},
				{ pb.T4, "FM",		tp_default_border,{{"MPD_COMM_PRESET_EDIT_TYPE", 3}}},
				{ pb.T5, "HF",		tp_default_border,{{"MPD_COMM_PRESET_EDIT_TYPE", 4}}}
			}
	},
}
createControls( Controls )
addPresetEdit_StatusWindow("MFD_COM_PRESET_Status_WindowEdit")
