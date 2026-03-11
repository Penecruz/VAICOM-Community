dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

addText( "COMM SOI SINC PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T1, "MSG SEND",nil },
	{ pb.T2, "SOI",		nil },
	{ pb.T4, "HQ2",		nil },
	{ pb.T5, "XPNDR",	nil },
	{ pb.T6, "UTIL",	nil },
	{ pb.B1, "SINC",	nil },
	{ pb.B2, "ERF",		nil },
}

createMenu( Menu )