dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_FUEL_Symbology.lua")
boost = 1;
local Controls = {} -- Controls( pb_num,  value,  tp,  controllers, formats, margins)

-- links Crossfeed  checkBox items with AH64_CROSSFEED_SEL
local CROSSFEED_AFT		= 2
local CROSSFEED_NORM	= 4				
local CROSSFEED_FWD		= 8	

-- links   Transfer fuel controls & checkBox with AH64_XFER
local XFER_OFF		= 1						
local XFER_FWD		= 2						
local XFER_AFT		= 4						
local XFER_AUTO		= 8						
local XFER_C_AUX	= 16					
local XFER_R_AUX	= 32					
local XFER_L_AUX	= 64					

Controls = 
{
	{ pb.R2, "BOOST{",	nil, {{"FUEL_Boost", 0}}, {"BOOST}", "BOOST{"} },
	
	{ pb.L1, "{L AUX",	nil, {{"FUEL_L_Aux"}}, {"}L AUX", "{L AUX"} },
	{ pb.L2, "{C AUX",	nil, {{"FUEL_C_Aux"}}, {"}C AUX", "{C AUX"} },
	{ pb.R1, "R AUX{",	nil, {{"FUEL_R_Aux"}}, {"R AUX}", "R AUX{"} },
	{ pb.L4, { 
				{"XFER",	nil}, 
				{"AUTO", tp_default_border, {{"FUEL_TransferShort"}}, { "OFF","FWD", "AFT", "AUTO"} } 
			} 
	},
	{ pb.L5, { {"AUX GALLONS EXT>",	nil, {{"FUEL_ExtTanksInstalled"}}}, {"?",tp_default_border, {{"FUEL_ExtTanksInstalled"},{"FUEL_SET_AUX_GALLONS_EXT_Button", pb.L5}} }}},
--	{ pb.L6, { {"AUX GALLONS CTR>",	nil}, {"?",	nil} } },
	{ pb.B6, "CHECK", nil },	
	{ "CROSSFEED", { 
						{ pb.R3, "FWD",	nil, {{"FUEL_Crossfeed", CROSSFEED_FWD}}}, 
						{ pb.R4, "NORM",nil, {{"FUEL_Crossfeed", CROSSFEED_NORM}}}, 
						{ pb.R5, "AFT",	nil, {{"FUEL_Crossfeed", CROSSFEED_AFT}}} 
					},  
		{{"FUEL_Crossfeed_Caption"}}
	},	
	
	{ pb.R2, "BOOST{",	tp_default_inv, {{"FUEL_Boost_inv", 1}}, {"BOOST}", "BOOST{"} },
	{ pb.R3, "FWD",	 	tp_default_inv, {{"FUEL_Crossfeed_inv", CROSSFEED_FWD}}}, 
	{ pb.R4, "NORM", 	tp_default_inv, {{"FUEL_Crossfeed_inv", CROSSFEED_NORM}}}, 
	{ pb.R5, "AFT",	 	tp_default_inv, {{"FUEL_Crossfeed_inv", CROSSFEED_AFT}}}, 
}
createControls( Controls )
