dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Controls = {}
Controls = 
{	
	{ pb.T6, "DEL",	 nil, {{"MPD_COM_REC_DEL"}} },
}
createControls( Controls )