dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Controls = {}
Controls = 
{
			{ pb.L1, 	"ALL",																		nil,	nil},
			{ pb.L2, 	{{"DATA",	nil },{"COMMUNICATION",	tp_default_border, nil }},				nil,	nil},
	{ "COM DATA", 
		{ 
			
			{ pb.R1, 	"RADIOS",																	nil,	{{"DMS_NoBorder"}}},
			{ pb.R2, 	"DL CONFIG",																nil,	{{"DMS_NoBorder"}}},
			{ pb.R3, 	"PRESETS",																	nil,	{{"DMS_NoBorder"}}},
			{ pb.R4, 	"MWOD",																		nil,	{{"DMS_NoBorder"}}},
			{ pb.R5, 	"SOI",																		nil,	{{"DMS_NoBorder"}}},
			{ pb.R6, 	"HF",																		nil,	{{"DMS_NoBorder"}}},
		}
	},
			{ pb.B3, 	{{"DTU",	nil },{"OPER",	tp_default_border, {{"DMS_SHUTDOWN_DTU_Button"}}, {"STBY", "OPER"} }},	nil,	nil},
}

createControls( Controls )