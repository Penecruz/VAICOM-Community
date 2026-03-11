dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

local Menu = {}
Menu = 
{ 			
	{ pb.B1, "COM",		tp_default_border },	
	{ pb.L6, {{"PRESET",	nil },{"DIR", nil }}}
}

createMenu( Menu )

local Controls = {}
Controls = 
{		
	--	{ pb.T1, {{"DAY", nil  },{ "1",	tp_default_border }}, nil  },	
	{ "RADIO",
		{
			{ pb.T1, "VHF",	nil,{{"MPD_COMM_RADIO_TUNE", 1}}},
			{ pb.T2, "UHF",	nil,{{"MPD_COMM_RADIO_TUNE", 2}}},
			{ pb.T3, "FM1",	nil,{{"MPD_COMM_RADIO_TUNE", 3}}},
			{ pb.T4, "FM2",	nil,{{"MPD_COMM_RADIO_TUNE", 4}}},
			{ pb.T5, "HF",	nil,{{"MPD_COMM_RADIO_TUNE", 5}}},
			{ pb.T6, "",	nil, nil},
		}
	},
	{ pb.B2, { {"TUNE", nil }, {"STBY", tp_default_border , {{"MPD_COM_TuneTarget", 2}}, {"PRI", "STBY"} } } },
	{ "TUNE UHF", 
				{ 
					{ pb.B4, 	"FH", nil, {{"MPD_COMM_TUNE_UHF", 1}}},  -- not supported		
					{ pb.B6, 	"SC", nil, {{"MPD_COMM_TUNE_UHF", 3}}},   
				}  		
	},
}
joinPresetBtns(Controls)
createControls( Controls )
addPresetBorders()
addSecondLabels()
addPresetEdit_StatusWindow("MFD_COM_PRESET_Status_Window")
