dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 		
	{ pb.B1, "COM",		nil },
	{ pb.B2, "MAN",	tp_default_border,	nil}	
}

local Controls = {}
Controls = 
{		
	{ "HF RECV EMSN",
				{
					{ pb.R1, "LSB",	   nil,		  {{"MFD_COMM_HF_RECV_EMSN_Buttons", 1}}}, 
					{ pb.R2, "USB",	   nil,		  {{"MFD_COMM_HF_RECV_EMSN_Buttons", 2}}}, 
					{ pb.R3, "CW",	   nil,		  {{"MFD_COMM_HF_RECV_EMSN_Buttons", 3}}}, 
					{ pb.R4, "AME",	   nil,		  {{"MFD_COMM_HF_RECV_EMSN_Buttons", 4}}},
				}
	}
}

createMenu( Menu )
createControls( Controls )