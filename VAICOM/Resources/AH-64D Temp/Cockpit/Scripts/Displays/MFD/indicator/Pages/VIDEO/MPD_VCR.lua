dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

addText( "VCR PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T1, "VIDEO",	nil },
	{ pb.T3, "IMAGE",	nil },
	{ pb.B1, "VCR",		tp_default_border },
}


createMenu( Menu )