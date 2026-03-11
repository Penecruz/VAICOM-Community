dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Controls = {}
Controls = 
{	
					{ pb.T6, 	"DP",					nil,				nil},

					{ pb.L1, 	"MASTER LOAD",			nil,				nil},
					{ pb.L2, 	{{"DATA",	nil },{"CURRENT MSN",	tp_default_border, nil }},				nil,				nil},
	{ "II DL", 
				{ 
					{ pb.L3, 	{{"MSG SECURITY",	nil, {{"DMS_NoBorder"}} },{"UNCLASSIFIED",	tp_default_border,	nil }},						nil,	{{"DMS_NoBorder"}}},
					{ pb.L4, 	{{"NAME/PASSWORD>",	nil, {{"DMS_NoBorder"}} },{"?",				nil, 				{{"DMS_NoBorder"}} }},		nil,	{{"DMS_NoBorder"}}},
				}
	},
					{ pb.L6, 	"WEAPONS/SIGHTS",		nil,				nil},
	
					{ pb.R1, 	"EMERG PROC",			nil,				nil},
					{ pb.R2, 	"THRU-FLIGHT",			nil,				{{"DMS_DTU_ThruFlight"}}},
					{ pb.R3, 	"SHOT AT",				nil,				nil},
					{ pb.R4, 	"MISCELLANEOUS",		nil,				nil},
					{ pb.R5, 	"FCR LETHAL RANGES",	nil,				nil},
					{ pb.R6, 	"NAVIGATION",			nil,				nil},
	
					{ pb.B3, 	{{"DTU",	nil },{"OPER",	tp_default_border, {{"DMS_SHUTDOWN_DTU_Button"}}, {"STBY", "OPER"} }},						nil,				nil},
					{ pb.B5, 	"WP",					nil,				nil},
					{ pb.B6, 	"SP",					nil,				nil},
}

createControls( Controls )