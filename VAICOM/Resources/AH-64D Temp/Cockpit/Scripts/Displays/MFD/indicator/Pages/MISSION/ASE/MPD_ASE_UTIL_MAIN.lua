dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Controls =
{
	{ pb.T1, { {"CHAFF",			nil, nil}, {"SAFE",		tp_default_border, {{"ASE_CHAFF_ArmSafe_Status"}},	{"SAFE", "ARM"}} } },
	
	{ pb.L1, { {"CHAFF MODE",		nil, nil}, {"PROGRAM",	tp_default_border, {{"ASE_CHAFF_Mode"}},			{"PROGRAM", "MANUAL"}} } },
	{ pb.L2, { {"BURST COUNT",		nil, nil}, {"4",		tp_default_border, {{"ASE_CHAFF_BurstCount"}},		{"1", "2", "3", "4", "6", "8"}} } },
	{ pb.L3, { {"BURST INTERVAL",	nil, nil}, {"0.1",		tp_default_border, {{"ASE_CHAFF_BurstInterval"}},	{"0.1", "0.2", "0.3", "0.4"}} } },
	{ pb.L4, { {"SALVO COUNT",		nil, nil}, {"1",		tp_default_border, {{"ASE_CHAFF_SalvoCount"}},		{"1", "2", "4", "8", "CONTINUOUS"}} } },
	{ pb.L5, { {"SALVO INTERVAL",	nil, nil}, {"1",		tp_default_border, {{"ASE_CHAFF_SalvoInterval"}},	{"1", "2", "3", "4", "5", "8", "RANDOM"}} } },
	{ pb.L6, { {"CARTRIDGES>",		nil, nil}, {"30",		tp_default_border, {{"ASE_CHAFF_Cartridges_Value"},{"MFD_DataEntryButton_frame",pb.L6}}} } },

	{ pb.R4, "RLWR{",	nil, {{"ASE_UTIL_RLWR"}}, {"RLWR}", "RLWR{"} },
	{ pb.R5, { {"RLWR VOICE", nil }, {"NORM", tp_default_border , {{"ASE_UTIL_RLWR_Voice"}}, {"NORM", "TERSE"} } } },	
}
createControls( Controls )