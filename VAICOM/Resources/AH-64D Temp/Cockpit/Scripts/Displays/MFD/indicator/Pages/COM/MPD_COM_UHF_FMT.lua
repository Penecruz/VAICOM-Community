dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

addText( "COMM UHF FMT PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T3, "XPNDR",	nil },
	{ pb.T4, "UHF",		nil },
	{ pb.T5, "FM",		nil },
	{ pb.T6, "HF",		nil },
	{ pb.B1, "COM",		nil },
	{ pb.B3, "FMT",		nil }, 
}

createMenu( Menu )