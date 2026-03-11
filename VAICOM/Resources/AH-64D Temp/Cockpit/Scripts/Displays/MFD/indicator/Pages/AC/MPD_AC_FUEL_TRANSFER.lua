dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_FUEL_Symbology.lua")

-- links Crossfeed  checkBox items with AH64_CROSSFEED_SEL
local CROSSFEED_AFT		= 2
local CROSSFEED_NORM	= 4				
local CROSSFEED_FWD		= 8		

-- links   Transfer fuel controls with AH64_XFER
local XFER_OFF		= 1						
local XFER_FWD		= 2						
local XFER_AFT		= 4						
local XFER_AUTO		= 8						
local XFER_C_AUX	= 16					
local XFER_R_AUX	= 32					
local XFER_L_AUX	= 64					

local Controls = {} -- Controls( pb_num,  value,  tp,  controllers, formats, margins)
Controls = 
{
	{ pb.R2, "BOOST{",	nil, {{"FUEL_Boost"}} , {"BOOST}", "BOOST{"} },
	{ "CROSSFEED", { 
						{ pb.R3, "FWD",	nil, {{"FUEL_Crossfeed", CROSSFEED_FWD}} }, 
						{ pb.R4, "NORM",nil, {{"FUEL_Crossfeed", CROSSFEED_NORM}} }, 
						{ pb.R5, "AFT",	nil, {{"FUEL_Crossfeed", CROSSFEED_AFT}} } 
				} 
},
	{ "TRANSFER", { 
						{ pb.L1, "FWD", nil, {{"FUEL_Transfer", XFER_FWD}} }, 
						{ pb.L2, "OFF",	nil, {{"FUEL_Transfer", XFER_OFF}} }, 
						{ pb.L3, "AFT", nil, {{"FUEL_Transfer", XFER_AFT}} },
						{ pb.L4, "AUTO",nil, {{"FUEL_Transfer", XFER_AUTO}} }
				} 
	},
	{ pb.B6, "CHECK" },	
	{ pb.R3, "FWD",	 tp_default_inv, {{"FUEL_Crossfeed_inv", CROSSFEED_FWD}}}, 
	{ pb.R4, "NORM", tp_default_inv, {{"FUEL_Crossfeed_inv", CROSSFEED_NORM}}}, 
	{ pb.R5, "AFT",	 tp_default_inv, {{"FUEL_Crossfeed_inv", CROSSFEED_AFT}}}, 
}
createControls( Controls )
