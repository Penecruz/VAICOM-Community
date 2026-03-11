dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_FUEL_Symbology.lua")
-- links   Transfer fuel controls with AH64_XFER
local XFER_OFF		= 1						
local XFER_FWD		= 2						
local XFER_AFT		= 4						
local XFER_AUTO		= 8						

local Controls = {} -- Controls( pb_num,  value,  tp,  controllers, formats, margins)
local Mask = closeMaskArea( 0, nil, buildBoxVerts(110, 300, "RightTop"), default_box_indices, {-510,340} )
Controls = 
{
{ "TRANSFER", { 
						{ pb.L1, "FWD", nil, {{"FUEL_Transfer", XFER_FWD}} }, 
						{ pb.L2, "OFF",	nil, {{"FUEL_Transfer", XFER_OFF}} }, 
						{ pb.L3, "AFT", nil, {{"FUEL_Transfer", XFER_AFT}} },
						{ pb.L4, "AUTO",nil, {{"FUEL_Transfer", XFER_AUTO}} }
				} 
	}	
}
createControls( Controls )


