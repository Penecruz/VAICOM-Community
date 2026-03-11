dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

local Menu = {}
Menu = 
{ 			
	{ pb.B1, "COM",		tp_default_border },	
	{ pb.B2, "MAN",		nil },
	{ pb.B4, "NET",		tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.B4}} },
	{ pb.L6, {{"PRESET",	nil },{"DIR", nil }}},
	{ pb.B6, {{"PRESET",	nil },{"EDIT", nil }}}
}

createMenu( Menu )
addBorder( pb.B6, 100, {{"MPD_COM_BORDER", pb.B6}}, 0, IND_MPD_MATERIAL_DARK_GREEN )
local Controls = {}
Controls = 
{			
--	{ pb.T1, {{"DAY", nil  },{ "1",	tp_default_border }}, nil  },	
	{ "RADIO",
		{
			{ pb.T1, "VHF",	nil,{{"MPD_COMM_RADIO", 1}}},
			{ pb.T2, "UHF",	nil,{{"MPD_COMM_RADIO", 2}}},
			{ pb.T3, "FM1",	nil,{{"MPD_COMM_RADIO", 3}}},
			{ pb.T4, "FM2",	nil,{{"MPD_COMM_RADIO", 4}}},
			{ pb.T5, "HF",	nil,{{"MPD_COMM_RADIO", 5}}},
			{ pb.T6, "",	nil, nil},
		}
	},
	{ pb.B1, "", tp_default_border, {{"MFD_COMM_OriginatorFmt", pb.B1}}, {"COM", "DL", "XPNDR", "UHF", "FM", "HF", "COM"} }
}
joinPresetBtns(Controls)
createControls( Controls )
addPresetBorders()
addSecondLabels()
addPresetEdit_StatusWindow("MFD_COM_PRESET_Status_Window")
