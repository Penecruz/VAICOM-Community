dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

addText( "COMM FM ERF PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	
	{ pb.T5, "FM",		nil },
	{ pb.B1, "COM",		nil },
	{ pb.B2, "ERF",		nil },
}

createMenu( Menu )