dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)

addHeadTracker("HeadTracker_HMD",	{0, 0}, VideoAreaPH.name,	{{"DSPLS_HOVER_HeadTracker",		video_area_h_05, video_area_h_05}})
addNavFlyToCUE("NavFlyToCUE_HMD",	{0, 0}, VideoAreaPH.name,	{{"DSPLS_TRANSITION_NavFlyToCUE",	video_area_h_05}})
addFlightPath("FlightPath_HMD",		{0, 0}, VideoAreaPH.name,	{{"DSPLS_TRANSITION_FPM",			video_area_h_05}})

addcScopeSymbols(VideoAreaPH.name)