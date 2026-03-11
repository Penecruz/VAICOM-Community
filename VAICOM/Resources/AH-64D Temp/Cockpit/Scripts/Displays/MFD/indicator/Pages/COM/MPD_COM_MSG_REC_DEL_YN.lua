dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Controls = {}
Controls = 
{	
	{ "DELETE",
		{
			{ pb.T5, "YES",	nil, {{"MPD_COM_YN"}} },
			{ pb.T6, "NO", nil, {{"MPD_COM_YN"}} },
		}
	},
}
createControls( Controls )