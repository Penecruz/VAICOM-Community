dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

local ASE_AUTOPAGE_SEARCH	= 0
local ASE_AUTOPAGE_ACQ		= 1				
local ASE_AUTOPAGE_TRACK	= 2
local ASE_AUTOPAGE_OFF		= 3	

local Controls = {} -- Controls( pb_num,  value,  tp,  controllers, formats, margins)
Controls = 
{
	{ "AUTOPAGE", { 
						{ pb.R1, "SEARCH",		nil, {{"ASE_Autopage", ASE_AUTOPAGE_SEARCH}} }, 
						{ pb.R2, "ACQUISION",	nil, {{"ASE_Autopage", ASE_AUTOPAGE_ACQ}} }, 
						{ pb.R3, "TRACK",		nil, {{"ASE_Autopage", ASE_AUTOPAGE_TRACK}} },
						{ pb.R4, "OFF",			nil, {{"ASE_Autopage", ASE_AUTOPAGE_OFF}} }
				} 
	},
}
createControls( Controls )
