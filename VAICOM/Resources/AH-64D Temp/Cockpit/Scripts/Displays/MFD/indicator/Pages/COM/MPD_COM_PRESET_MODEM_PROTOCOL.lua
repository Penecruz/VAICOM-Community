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
		{ "PROTOCOL", {
				{ pb.L1, "DATALINK",		tp_default,	{{"MPD_COMM_PRESET_MODEM_PROTOCOL", 1}}},
				{ pb.L2, "TACFIRE",			tp_default,	{{"MPD_COMM_PRESET_MODEM_PROTOCOL", 2}}},
				{ pb.L3, "INTERNET",		tp_default,	{{"MPD_COMM_PRESET_MODEM_PROTOCOL", 3}}},
				{ pb.L4, "FIRE SUPPORT",	tp_default,	{{"MPD_COMM_PRESET_MODEM_PROTOCOL", 4}}},
				{ pb.L5, "NONE",			tp_default,	{{"MPD_COMM_PRESET_MODEM_PROTOCOL", 0}}},
			}
	}
}
createControls( Controls )

addModem_StatusWindow()