dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_PB_Defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Tools.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Materials.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_StringSymbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_MenuSymbology.lua")

fill_pb_props()
menu_draw_scale = 720.0 / 1024.0

-- Center of the video area
local OnOffPageMenu = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 1}})
local VideoAreaPH = addPlaceholder("VideoAreaCenterPHtpm", video_area_pos, OnOffPageMenu.name, nil)

local OnOffPageMenuCommon = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 0}})
local VideoAreaPHCommonMenu = addPlaceholder("VideoAreaCenterPHCommonMenutpm", video_area_pos,  OnOffPageMenuCommon.name)

local controls = {}
controls = 
{ 
    { pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
    { pb.L1,  {{"PROF",         nil         },  {"GEOM", tp_default_border, {{"DSPLS_FCR_AG_ShowProfCaptionTPM"}}, { "GEOM", "ARITH", "TEST"}}}},
    { pb.L2,  {{"LINES",        nil         },  {"0", tp_default_border,    {{"DSPLS_FCR_AG_ShowLineCaptionTPM"}}, { "0", "1", "2", "3", "4" }}}},

    { pb.R1,  {{"CLEARANCE",    nil         },  {"20", tp_default_border, {{"DSPLS_FCR_AG_ShowClearanceCaptionTPM"}}, { "20", "50", "100", "200" } }}},
    { pb.R2,  {{"ELEV",         nil         },  {"FAR", tp_default_border, {{"DSPLS_FCR_AG_ShowButtonElevationControlTPM", 1}}}}},
    { pb.R2,  {{"ELEV",         nil         },  {"NEAR", tp_default_border, {{"DSPLS_FCR_AG_ShowButtonElevationControlTPM", 0}}}}}, 
}
createControls( controls, nil, VideoAreaPHCommonMenu.name )

local Controls = {}
Controls = 
{ 
    { pb.L1,  {{"PROF",         nil         },  {"GEOM", tp_default_border, {{"DSPLS_FCR_AG_ShowProfCaptionTPM"}}, { "GEOM", "ARITH", "TEST"}}}},
    { pb.L2,  {{"LINES",        nil         },  {"0", tp_default_border,    {{"DSPLS_FCR_AG_ShowLineCaptionTPM"}}, { "0", "1", "2", "3", "4" }}}},
    
    { pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
    { pb.R1,  {{"CLEARANCE",    nil         },  {"20", tp_default_border, {{"DSPLS_FCR_AG_ShowClearanceCaptionTPM"}}, { "20", "50", "100", "200" } }}},
    { pb.R2,  {{"ELEV",         nil         },  {"FAR", tp_default_border, {{"DSPLS_FCR_AG_ShowButtonElevationControlTPM", 1}}}}},
    { pb.R2,  {{"ELEV",         nil         },  {"NEAR", tp_default_border, {{"DSPLS_FCR_AG_ShowButtonElevationControlTPM", 0}}}}}, 
}

createControls( Controls, nil, VideoAreaPH.name )


