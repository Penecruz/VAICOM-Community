dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_FUEL_Symbology.lua")


local Controls = {} -- Controls( pb_num,  value,  tp,  controllers, formats, margins)
Controls = 
{
	{ pb.L4,{ 
				{"XFER",	nil}, 
				{"AUTO", tp_default_border, {{"FUEL_TransferShort"}}, { "OFF","FWD", "AFT", "AUTO"} } 
			} 
	},
}
createControls( Controls )


