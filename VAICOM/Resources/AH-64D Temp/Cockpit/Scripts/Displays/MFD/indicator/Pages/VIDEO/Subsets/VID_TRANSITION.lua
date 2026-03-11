dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/VIDEO/VIDEO_PAGE_Symbology.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0, 0}, nil, nil)
local SafeZonePH	= addPlaceholder("SafeZonePH", {0, 0}, VideoAreaPH.name)
local SafeZone_Mask = openMaskArea(0, "SafeZone", buildBoxVerts(PitchLadder_h, PitchLadder_h, "CenterCenter"), default_box_indices, {0,0}, SafeZonePH.name, nil, nil)

addText("DestinationPoint",	"W01",	{-470, -290},	"LeftBottom",	nil,	{{"DSPLS_TRANSITION_DestinationPoint"}})
addBigText("Groundspeed",	"0",	{-454, -335},	"LeftBottom",	nil,	{{"DSPLS_TRANSITION_GroundSpeed"}})
addText("DistanceToGO",		"25.7",	{-275, -290},	"RightBottom",	nil,	{{"DSPLS_TRANSITION_DistanceToGo"}})
addText("DistanceUnits",	"KM",	{-260, -290},	"LeftBottom",	nil,	{{"DSPLS_TRANSITION_DistanceUnits"}}, {"NM", "KM"})

addText("TimeToGO_L",			"5:52",	{-255, -335},	"RightBottom",	nil,	{{"DSPLS_TRANSITION_TimeToGo", 0}})
addText("TimeToGO_R",			"5:52",	{-210, -335},	"RightBottom",	nil,	{{"DSPLS_TRANSITION_TimeToGo", 1}})

addNavFlyToCUE("NavFlyToCUE", {0, 0}, VideoAreaPH.name, {{"VIDEO_TRANSITION_NavFlyToCUE",  0, 0.75*video_area_h_05, video_area_h_05, 1.5*video_area_h_05}})
--elements for safe zone
local FlightPath = addFlightPath("FlightPath", {0, 0}, VideoAreaPH.name, {{"VIDEO_TRANSITION_FPM",  0, 0.75*video_area_h_05, video_area_h_05, 1.5*video_area_h_05}})
local HorizonLine = addHorizonLine("HorizonLine", {0,0}, SafeZonePH.name, {{"DSPLS_TRANSITION_HorizonLine", 3*PitchLadder_h/8}})

setElemsClipLevel(HorizonLine, 1)
resetMask(SafeZone_Mask, 0, SafeZonePH.name)

--local Format = FORMAT.TRANSITION
--local LOS_Reticle = add_LOS_Reticle("Transition", {0,0}, VideoAreaPH.name, {{"VIDEO_HOVER_LOS_Reticle", Format}})