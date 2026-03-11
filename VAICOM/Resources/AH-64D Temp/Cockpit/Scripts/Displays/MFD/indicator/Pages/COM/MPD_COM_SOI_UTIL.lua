dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

addText( "COMM COM PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T1, "MSG SEND",	nil },
	{ pb.T2, "SOI",			nil },
	{ pb.T3, "SINC",		nil },
	{ pb.T4, "HQ2",			nil },
	{ pb.T5, "XPNDR",		nil },
	{ pb.B1, "UTIL",		nil },
	{ pb.B6, "FALL BACK",	nil },
}

createMenu( Menu )