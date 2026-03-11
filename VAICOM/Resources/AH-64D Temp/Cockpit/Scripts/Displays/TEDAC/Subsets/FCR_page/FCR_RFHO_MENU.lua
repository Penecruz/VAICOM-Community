dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_PB_Defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Tools.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Materials.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_StringSymbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_MenuSymbology.lua")

fill_pb_props()

-- Center of the video area
local OnOffPageRFHO = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 2}})

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, OnOffPageRFHO.name, nil)

local controls = {}
controls=
{
    { pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
    { pb.L1, "N\nT\nS", 	nil,	            nil},
    { pb.L2, "c", 	        tp_52,	            nil},
    { pb.L4, "T\nG\nT",		nil,	            nil},
    { pb.L5,  {{"ELEV",     nil,                {{"DSPLS_FCR_AG_ShowButtonElevationControl", 1}}}, {"AUTO", tp_default_border,{{"DSPLS_FCR_AG_ShowButtonElevationControl", 1}}}}},
    { pb.L5, "a",           tp_52,              {{"DSPLS_FCR_AG_ShowButtonElevationControl", 0}, {"DSPLS_FCR_OnOffShowButtonElevationGTM", 1}, {"DSPLS_FCR_OnOffShowButtonElevationATM", 1}}},
    { pb.L6, "b",           tp_52,              {{"DSPLS_FCR_AG_ShowButtonElevationControl", 0}, {"DSPLS_FCR_OnOffShowButtonElevationGTM", 0}, {"DSPLS_FCR_OnOffShowButtonElevationATM", 0}}},
}

local Controls_Subscribers = {}
Controls_Subscribers = 
{
	{ pb.T5, "L07",		tp_default_border,  {{"DSPLS_TSD_Subscriber_Caption", 0}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 0}}},
	{ pb.T6, "L33",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 1}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 1}}},

	{ pb.R1, "L40",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 2}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 2}}},
	{ pb.R2, "L52",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 3}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 3}}},
	{ pb.R3, "L14",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 4}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 4}}},
	{ pb.R4, "L88",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 5}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 5}}},
	{ pb.R5, "L09",		tp_default_border,	{{"DSPLS_TSD_Subscriber_Caption", 6}, {"DSPLS_FCR_RHFO_Subscriber_Selection", 6}}},
}

local ControlsSend =
{
    { pb.R6, "SEND",	tp_28_white, {{"DSPLS_COM_SendBtn"}}},
    { pb.R6, "SEND",	tp_28_white_inv,  {{"DSPLS_COM_SendBtn_inv"}}},
}

createControls( controls,  nil, VideoAreaPH.name )
createControls( Controls_Subscribers, nil, VideoAreaPH.name )
createControls( ControlsSend, nil, VideoAreaPH.name )