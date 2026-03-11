dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
local MISSION_ASE = 6
local Menu =
{ 
	{ pb.T2, "ASE",		tp_default_darkgreen_border, {{"MFD_AsePrevBorder", MISSION_ASE}} },
	{ pb.T6, "UTIL",	tp_default_border },
	{ pb.B1, "ASE",		tp_default_border, {{"MFD_AC_OriginatorAseFmt"}}, {"ASE", "WPN", "TSD"} }
}
createMenu( Menu )

local Controls =
{
	{ pb.T1, { {"CHAFF",			nil, nil}, {"SAFE",		tp_default_border, {{"ASE_CHAFF_ArmSafe_Status"}},	{"SAFE", "ARM"}} } },

	{ pb.R4, "RWR{",	nil, {{"ASE_UTIL_RLWR"}}, {"RWR}", "RWR{"} },
	{ pb.R5, { {"RLWR VOICE", nil }, {"NORM", tp_default_border , {{"ASE_UTIL_RLWR_Voice"}}, {"NORM", "TERSE"} } } },	
}
--createControls( Controls )