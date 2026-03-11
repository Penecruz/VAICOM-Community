dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

--addText( "DMS UTIL PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T1, "DTU",			nil },
	{ pb.T2, "FAULT",		nil },
	{ pb.T3, "IBIT",		nil },
	{ pb.T4, {{"SHUT",	nil },{"DOWN",	nil }}},
	{ pb.T5, "VERS",		nil },
	{ pb.T6, "UTIL",		tp_default_border },
	{ pb.L1, "BORESIGHT",	nil },
	{ pb.B1, "DMS",			nil },
	{ pb.B2, "FTEST",		nil },
	{ pb.B3, "TEST",		nil },
	{ pb.B5, "ECS",			nil },
	{ pb.B6, "WCA",			nil },
}

createMenu( Menu )


local Controls = {}
Controls = 
{	
	{ pb.L2, 	{ {"DP SELECT",			nil, nil},	{"NORMAL",		tp_default_border, {{"DMS_UTIL_DpSelect"}},		{"NORMAL","DP1", "DP2"}} } },			
	{ pb.L3, 	{ {"WP SELECT",			nil, nil},	{"WP1",			tp_default_border, {{"DMS_UTIL_WpSelect"}},		{"WP1", "WP2"}} } },
	{ pb.L4, 	{ {"HIADC SELECT",		nil, nil},	{"AADS-AUTO",	tp_default_border, {{"DMS_UTIL_HiadcSelect"}},	{"AADS-AUTO", "AADS-LH", "AADS-RH"}} } },
	
	{ pb.R2, 	{ {"TIME",				nil, nil},	{"ZULU",		tp_default_border, {{"TSD_UTIL_TIME_Btn_Caption"}}, 	{"ZULU", "LOCAL"}} } },
	{ pb.R3, 	{ {"SYSTEM TIME>",		nil, nil},	{"11:59:59",	tp_default_border, {{"TSD_UTIL_SYSTEM_TIME_Caption"}, {"MFD_DataEntryButton_frame",pb.R3}}, } } },
	{ pb.R4, 	{ {"SYSTEM DATE>",		nil, nil},	{"11/13/01",	tp_default_border, {{"TSD_UTIL_SYSTEM_DATE_Caption"}, {"MFD_DataEntryButton_frame",pb.R4}}, } } },
	{ pb.R5, 	{ {"INPUT TAIL NUMBER>",nil, nil},	{"980049",		tp_default_border, {{"DMS_UTIL_Tail"}, {"MFD_DataEntryButton_frame",pb.R5}}, } } },
	{ pb.R6,	"DMS AUTOPAGE{",	nil, 			{{"DMS_UTIL_Autopage"}},	{"DMS AUTOPAGE}", "DMS AUTOPAGE{"} },
}

createControls( Controls )


