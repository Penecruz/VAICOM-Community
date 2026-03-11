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
	{ "RERTIES", {
				{ pb.L3, "2",	tp_default,	{{"MPD_COMM_PRESET_MODEM_RERTIES", 2}}},
				{ pb.L4, "1",	tp_default,	{{"MPD_COMM_PRESET_MODEM_RERTIES", 1}}},
				{ pb.L5, "0",	tp_default,	{{"MPD_COMM_PRESET_MODEM_RERTIES", 0}}},
			}
	}
}
createControls( Controls )

addModem_StatusWindow()