dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T6, "UTIL",tp_default_border,	nil },
}

createMenu( Menu )


local controls = {}
controls=
{
	{ pb.L2, "}RFI",		nil,			{{"WPN_UTIL_RFI_Enable"}},		 	{"{RFI",	"}RFI"}		},
	{ pb.L3, "}FCR",		tp_default_inv,	{{"WPN_UTIL_FCR_Enable", 1}},		 	{"{FCR",	"}FCR"}		},
	{ pb.L4, "FCR BIT ORIDE"},
	
	{ pb.R6, { {"MMA", nil, nil}, {"PINNED", tp_default_border, {{"WPN_UTIL_MMA_Selection"}}, {"PINNED","NORM"}} } },

	{ pb.B1, "FCR", 	nil,	nil},
}
createControls( controls )