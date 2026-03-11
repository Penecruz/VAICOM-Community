dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_PB_Defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Tools.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Materials.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_StringSymbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_MenuSymbology.lua")

fill_pb_props()

local OnOffPageTGT = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 3}})

-- Center of the video area
local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, OnOffPageTGT.name, nil)

local controls = {}
controls=
{
    { pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
    { pb.L4, "T\nG\nT",		tp_default_border,  nil	},
    { pb.L5,  "ALL",        tp_28,              {{"DSPLS_FCR_AG_ShowAllButtonTGT"}} },
}
createControls( controls, nil, VideoAreaPH.name )
