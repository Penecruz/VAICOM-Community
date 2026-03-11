dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

local Menu = {}
Menu = 
{ 			
	{ pb.B1, "COM",		nil },	
	{ pb.B4, "NET",		tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.B4}} },
	{ pb.B6, {{"PRESET", nil },{"EDIT", nil }}  }
}

createMenu( Menu )
addBorder( pb.B6, 100, {{"MPD_COM_BORDER", pb.B6}}, 0, IND_MPD_MATERIAL_DARK_GREEN )

local Controls = {}
Controls = 
{			
	{ "UHF CNV", {
				{ pb.R1, "1",	tp_default_border,{{"MPD_COMM_PRESET_EDIT_UHF_CNV", 1}}},
				{ pb.R2, "2",	tp_default_border,{{"MPD_COMM_PRESET_EDIT_UHF_CNV", 2}}},
				{ pb.R3, "3",	tp_default_border,{{"MPD_COMM_PRESET_EDIT_UHF_CNV", 3}}},
				{ pb.R4, "4",	tp_default_border,{{"MPD_COMM_PRESET_EDIT_UHF_CNV", 4}}},
				{ pb.R5, "5",	tp_default_border,{{"MPD_COMM_PRESET_EDIT_UHF_CNV", 5}}},
				{ pb.R6, "6",	tp_default_border,{{"MPD_COMM_PRESET_EDIT_UHF_CNV", 6}}},
			}
	},
}
createControls( Controls )
addPresetEdit_StatusWindow("MFD_COM_PRESET_Status_WindowEdit")