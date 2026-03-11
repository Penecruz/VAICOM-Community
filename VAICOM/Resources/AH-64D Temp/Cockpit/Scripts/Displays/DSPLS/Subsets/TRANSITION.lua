dofile(LockOn_Options.script_path.."Displays/DSPLS/DSPLS_Symbology.lua")
--local Format = FORMAT.TRANSITION
local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0, 0}, nil, {{"VideoAreaPH"}, {"VideoSignal_Show"}})
local SafeZonePH	= addPlaceholder("SafeZonePH", {0, 0}, VideoAreaPH.name)
local SafeZone_Mask = openMaskArea(0, "SafeZone", buildBoxVerts(PitchLadder_h, PitchLadder_h, "CenterCenter"), default_box_indices, {0,0}, SafeZonePH.name, nil, nil)

-- Navigation Data Status
addStrokeText("DestinationPoint",	"W01",		STROKE_FNT_DFLT_100,	"LeftBottom",	{-760, -430},  nil, {{"DSPLS_TRANSITION_DestinationPoint"}})
addStrokeText("Groundspeed",		"0",		STROKE_FNT_DFLT_120,	"LeftBottom",	{-750, -500},  nil, {{"DSPLS_TRANSITION_GroundSpeed"}})
addStrokeText("DistanceToGO",		"25.7",		STROKE_FNT_DFLT_100,	"RightBottom",	{-430, -430},  nil, {{"DSPLS_TRANSITION_DistanceToGo"}})
addStrokeText("DistanceUnits",		"KM",		STROKE_FNT_DFLT_100,	"LeftBottom",	{-400, -430},  nil, {{"DSPLS_TRANSITION_DistanceUnits"}}, {"NM", "KM"})

addStrokeText("TimeToGO_L",			"5:52",		STROKE_FNT_DFLT_100,	"LeftBottom",	{-530, -490},  nil, {{"DSPLS_TRANSITION_TimeToGo", 0}})
addStrokeText("TimeToGO_R",			"5:52",		STROKE_FNT_DFLT_100,	"RightBottom",	{-340, -490},  nil, {{"DSPLS_TRANSITION_TimeToGo", 1}})

local NavFlyToCUE = addNavFlyToCUE("NavFlyToCUE", {0, 0}, VideoAreaPH.name, {{"HMD_TRANSITION_NavFlyToCUE", video_area_h_05}})

--elements for safe zone
local FlightPath = addFlightPath("FlightPath", {0, 0}, VideoAreaPH.name, {{"HMD_TRANSITION_FPM", video_area_h_05}})
local HorizonLine = addHorizonLine("HorizonLine", {0,0}, SafeZonePH.name, {{"DSPLS_TRANSITION_HorizonLine", 3*PitchLadder_h/8}})

setElemsClipLevel(HorizonLine, 1)
resetMask(SafeZone_Mask, 0, SafeZonePH.name)
