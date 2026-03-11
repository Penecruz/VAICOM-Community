dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

local Controls = {}
Controls = 
{
	{ pb.R1, { 
				{"AUTOPAGE",	nil}, 
				{"SEARCH", tp_default_border, {{"ASE_Autopage_Caption"}}, { "SEARCH","ACQUISITION", "TRACK", "OFF"} } 
			} 
	},
	{ pb.R5, "CAQ", nil, {{"ASE_FCR_Enabled"}} },	-- TODO for FCR
}
createControls( Controls )

local Boxes = {}
Boxes = 
{ 
	{
		{{250,455}, nil, nil, nil, nil, nil, { 10,10,-45,-12 } },
		{{"RFI ", tp_28,	{{"ASE_FCR_Enabled"}}}, {"0000", tp_28 , {{"ASE_RFI_ThreatsCount"},{"ASE_FCR_Enabled"}} }},
		{{"RLWR ", tp_28}, 	{"0000", tp_28 , {{"ASE_RLWR_ThreatsCount"}} } }
	},
}	
createInfoBoxes( Boxes )
