dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

local Menu = {}
Menu = 
{ 
	{ pb.R6, "MODEM",	tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.R6}}},
	{ pb.B1, "COM",		nil },	
	{ pb.B4, "NET",		tp_default_darkgreen_border, {{"MPD_COM_BTN_BORDER", pb.B4}} },
	{ pb.B6, {{"PRESET", nil, nil },{"EDIT", nil}}  }
}
createMenu( Menu )
addBorder( pb.B6, 100, {{"MPD_COM_BORDER", pb.B6}}, 0, IND_MPD_MATERIAL_DARK_GREEN )
Controls = 
{	
	{ pb.L1, { 
			{"PROTOCOL", nil }, 
			{"",  tp_default_border, {{"MPD_COM_PRESET_MODEM", pb.L1}},  {"NONE", "DATALINK", "TACFIRE", "INTERNET", "FIRE SUPPORT"}},
		}},
		
	{ pb.L2, "{AUTO ACK",	nil, {{"MPD_COM_PRESET_MODEM", pb.L2}}, {"}AUTO ACK", "{AUTO ACK"} },	
	
	{ pb.L3, { 
			{"RETRIES", nil }, 
			{"",  tp_default_border, {{"MPD_COM_PRESET_MODEM", pb.L3}}},
		}},
	{ pb.L4, { 
			{"BAUD RATE", nil }, 
			{"16000",  tp_default_border, nil },
		}},
}
createControls( Controls )

addModem_StatusWindow()