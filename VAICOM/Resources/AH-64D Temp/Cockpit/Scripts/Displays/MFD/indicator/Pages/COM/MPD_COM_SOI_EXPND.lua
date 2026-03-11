dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

addText( "COMM COM SOI EXPND PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T1, "MSG SEND",nil },
	{ pb.T3, "SINC",	nil },
	{ pb.T4, "HQ2",		nil },
	{ pb.T5, "XPNDR",	nil },
	{ pb.T6, "UTIL",	nil },
	{ pb.B1, "SOI",		nil },
	{ pb.B3, "XPND",	nil },
}

createMenu( Menu )