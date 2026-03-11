dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T2, "DL",		nil },
	{ pb.T3, "XPNDR",	nil },
	{ pb.T4, "UHF",		nil },
	{ pb.T5, "FM",		nil },
	{ pb.T6, "HF",		nil },
	{ pb.B1, "COM",		tp_default_border },
	{ pb.B2, "MAN",		nil },
	--{ pb.B3, "FALL\nBACK", nil },	
	{ pb.B4, {{"ORIG", nil  },{ "ID",	nil }}, nil  }, -- 2 rows label
	{ "MSG",
		{
			{ pb.B5, "REC", nil },
			{ pb.B6, "SEND", tp_default_border, {{"MPD_COM_BTN_BORDER", pb.B6}} },
		}
	},
	{ pb.L6, {{"PRESET",	nil },{"DIR", nil }}}
}

createMenu( Menu )

local Controls = {}
Controls = 
{		
	{ pb.T1, {{"DAY", nil  },{ "1",	tp_default_border }}, nil  },	
	{ pb.B3, {{"FALL", 		    nil  },{"BACK",	nil, nil }}, nil, nil},
}
joinPresetBtns(Controls)
createControls( Controls )
addSecondLabels()
addOwnShip_StatusWindow()

AddDL_StatusWnd()

