dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Controls = {}
Controls = 
{	
{ pb.T1,  
		{ 
			{"SOURCE",	nil}, 
			{"DL", tp_default_border, nil} 
		}
	},	
	{ pb.T2, "RVW",	 tp_default_border, nil },
}
createControls( Controls )