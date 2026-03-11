dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_PB_Defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Tools.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Materials.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_StringSymbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_MenuSymbology.lua")

fill_pb_props()

local OnOffPageClearanc = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 6}})
local VideoAreaPH = addPlaceholder("VideoAreaCenterPHtpm", video_area_pos, OnOffPageClearanc.name, nil)

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

    { pb.R1,    "20",    	tp_default_border,               {{"DSPLS_FCR_AG_ShowClearanceTPM", 0}}  },
    { pb.R2,    "50",       tp_default_border,             	 {{"DSPLS_FCR_AG_ShowClearanceTPM", 1}}  },
    { pb.R3,    "100",    	tp_default_border,               {{"DSPLS_FCR_AG_ShowClearanceTPM", 2}}  },
    { pb.R4,    "200",    	tp_default_border,               {{"DSPLS_FCR_AG_ShowClearanceTPM", 3}}  },

    { pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
    { pb.L1,  {{"PROF",         nil         },  {"GEOM", tp_default_border, {{"DSPLS_FCR_AG_ShowProfCaptionTPM"}}, { "GEOM", "ARITH", "TEST"}}}},
    { pb.L2,  {{"LINES",        nil         },  {"0", tp_default_border,    {{"DSPLS_FCR_AG_ShowLineCaptionTPM"}}, { "0", "1", "2", "3", "4" }}}},

}

createControls(controls, nil, VideoAreaPH.name)
