dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local TPM_PH = addPlaceholder("TPM PH", {0, 0}, {{"DSPLS_FCR_GTM_Root"}})

local OnOffPageMenu = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 1}})
local OnOffPageMenuCommon = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 0}})

local Menu = {}
Menu = 
{ 
    { pb.T6, "UTIL",		nil },
}
createMenu( Menu )

local ControlsBase = {}
ControlsBase = 
{
    { pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
    { pb.L1,  {{"PROF",         nil         },  {"GEOM", tp_default_border, {{"DSPLS_FCR_AG_ShowProfCaptionTPM"}}, { "GEOM", "ARITH", "TEST"}}}},
    { pb.L2,  {{"LINES",        nil         },  {"0", tp_default_border,    {{"DSPLS_FCR_AG_ShowLineCaptionTPM"}}, { "0", "1", "2", "3", "4" }}}},

    { pb.R1,  {{"CLEARANCE",    nil         },  {"20", tp_default_border, {{"DSPLS_FCR_AG_ShowClearanceCaptionTPM"}}, { "20", "50", "100", "200" } }}},
    { pb.R2,  {{"ELEV",         nil         },  {"FAR", tp_default_border, {{"DSPLS_FCR_AG_ShowButtonElevationControlTPM", 1}}}}},
    { pb.R2,  {{"ELEV",         nil         },  {"NEAR", tp_default_border, {{"DSPLS_FCR_AG_ShowButtonElevationControlTPM", 0}}}}},
}

createControls( ControlsBase, nil, OnOffPageMenuCommon.name )

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

createControls( Controls, nil, OnOffPageMenu.name )
