dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_PB_Defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Tools.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Materials.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_StringSymbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_MenuSymbology.lua")

fill_pb_props()

local OnOffPageLINE = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 4}})
local VideoAreaPH = addPlaceholder("VideoAreaCenterPHtpm", video_area_pos, OnOffPageLINE.name, nil)

local OnOffPageMenuCommon = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 0}})
local VideoAreaPHCommonMenu = addPlaceholder("VideoAreaCenterPHCommonMenutpm", video_area_pos,  OnOffPageMenuCommon.name)

local controls = {}
controls = 
{ 
	{ pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
}
createControls( controls, nil, VideoAreaPHCommonMenu.name )

local controls = {}
controls=
{
    { pb.L1,    "0",    tp_default_border,              {{"DSPLS_FCR_AG_ShowLineTPM", 0}}},
    { pb.L2,    "1",    tp_default_border,              {{"DSPLS_FCR_AG_ShowLineTPM", 1}}},
    { pb.L3,    "2",    tp_default_border,              {{"DSPLS_FCR_AG_ShowLineTPM", 2}}},
	{ pb.L4,    "3",    tp_default_border,              {{"DSPLS_FCR_AG_ShowLineTPM", 3}}},
	{ pb.L5,    "4",    tp_default_border,              {{"DSPLS_FCR_AG_ShowLineTPM", 4}}},

    { pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
    { pb.R1,  {{"CLEARANCE",    nil         },  {"20", tp_default_border, {{"DSPLS_FCR_AG_ShowClearanceCaptionTPM"}}, { "20", "50", "100", "200" } }}},
    { pb.R2,  {{"ELEV",         nil         },  {"FAR", tp_default_border, {{"DSPLS_FCR_AG_ShowButtonElevationControlTPM", 1}}}}},
    { pb.R2,  {{"ELEV",         nil         },  {"NEAR", tp_default_border, {{"DSPLS_FCR_AG_ShowButtonElevationControlTPM", 0}}}}},
}
createControls(controls, nil, VideoAreaPH.name)
