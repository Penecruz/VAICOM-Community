dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
--local Format = FORMAT.TRANSITION
local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)
local SafeZonePH	= addPlaceholder("SafeZonePH", {0, 0}, VideoAreaPH.name)
local SafeZone_Mask = openMaskArea(0, "SafeZone", buildBoxVerts(PitchLadder_h, PitchLadder_h, "CenterCenter"), default_box_indices, {0,0}, SafeZonePH.name, nil, nil)

-- Navigation Data Status
local NavDataStatusWindowPH = addPlaceholder("NavDataStatusWindowPH", {-375, -170}, nil, nil)
addText("DestinationPoint",	"W01",		{   0,   0}, "LeftBottom",	NavDataStatusWindowPH.name,	{{"DSPLS_TRANSITION_DestinationPoint"}})
addBigText("Groundspeed",	"0",		{   5, -32}, "LeftBottom",	NavDataStatusWindowPH.name,	{{"DSPLS_TRANSITION_GroundSpeed"}})
addText("DistanceToGO",		"25.7",		{ 150,   0}, "RightBottom",	NavDataStatusWindowPH.name,	{{"DSPLS_TRANSITION_DistanceToGo"}})
addText("DistanceUnits",	"KM",		{ 155,   0}, "LeftBottom",	NavDataStatusWindowPH.name,	{{"DSPLS_TRANSITION_DistanceUnits"}}, {"NM", "KM"})
addText("TimeToGO_L",		"5:52",		{ 165, -32}, "RightBottom",	NavDataStatusWindowPH.name,	{{"DSPLS_TRANSITION_TimeToGo", 0}})
addText("TimeToGO_R",		"5:52",		{ 190, -32}, "RightBottom",	NavDataStatusWindowPH.name,	{{"DSPLS_TRANSITION_TimeToGo", 1}})

--
addNavFlyToCUE("NavFlyToCUE",	{0, 0}, VideoAreaPH.name, {{"DSPLS_TRANSITION_NavFlyToCUE", video_area_h_05}})
addFlightPath("FlightPath",		{0, 0}, VideoAreaPH.name, {{"DSPLS_TRANSITION_FPM", video_area_h_05}})

--elements for safe zone
local HorizonLine = addHorizonLine("HorizonLine", {0,0}, SafeZonePH.name, {{"DSPLS_TRANSITION_HorizonLine", 3*PitchLadder_h/8}})

setElemsClipLevel(HorizonLine, 1)
resetMask(SafeZone_Mask, 0, SafeZonePH.name)
--local LOS_Reticle = add_TRANSITION_LOS_Reticle(nil, {0,0}, VideoAreaPH.name, {{"DSPLS_HOVER_LOS_Reticle", Format}})