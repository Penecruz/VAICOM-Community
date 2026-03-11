dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_PB_Defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Tools.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Materials.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_StringSymbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_MenuSymbology.lua")

fill_pb_props()

local OnOffPageACQ = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_AcqSubset"}})

-- Center of the video area
local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, OnOffPageACQ.name, nil)

local Controls = {}
Controls = 
{
	{ pb.R1, "PHS",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection", 0}, {"DSPLS_TSD_ACQ_Source_Valid", 0}}, {"?PHS","PHS"} }, 
	{ pb.R2, "GHS",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection", 1}, {"DSPLS_TSD_ACQ_Source_Valid", 1}}, {"?GHS","GHS"} }, 
	{ pb.R3, "SKR",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection", 2}, {"DSPLS_TSD_ACQ_Source_Valid", 2}}, {"?SKR","SKR"} }, 
	{ pb.R4, "RFI",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection", 3}, {"DSPLS_TSD_ACQ_Source_Valid", 3}}, {"?RFI","RFI"} },  
	{ pb.R5, "FCR",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection", 4}, {"DSPLS_TSD_ACQ_Source_Valid", 4}}, {"?FCR","FCR"} }, 
	{ pb.R6, "FXD",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection", 5}}},

	{ pb.B6, "TADS",tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection", 6}, {"DSPLS_TSD_ACQ_Source_Valid", 6}}, {"?TADS","TADS"} },
	{ pb.B5, "?00",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection", 7}, {"DSPLS_TSD_ACQ_TXX_Selected_Caption"}}},
	{ pb.B4, "TRN",	tp_default_border, {{"DSPLS_TSD_ACQ_Button_Selection", 8}, {"DSPLS_TSD_ACQ_TRN_Selected_Caption"}}},
	--{ pb.B3, "ASE",	tp_default_border, {{"TSD_ACQ_Button_Selection",9}} },
}
-- draw menus

createControls( Controls, nil, VideoAreaPH.name )
