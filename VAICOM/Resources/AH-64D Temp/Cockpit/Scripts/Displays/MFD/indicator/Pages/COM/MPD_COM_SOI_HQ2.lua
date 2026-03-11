dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

addText( "COMM COM SOI HQ2 PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T1, "MSG SEND",nil },
	{ pb.T2, "SOI",		nil },
	{ pb.T3, "SINC",	nil },
	{ pb.T5, "XPNDR",	nil },
	{ pb.T6, "UTIL",	nil },
	{ pb.B4, "SEG",		nil },
	{ pb.B5, "TSET",	nil },
}

createMenu( Menu )