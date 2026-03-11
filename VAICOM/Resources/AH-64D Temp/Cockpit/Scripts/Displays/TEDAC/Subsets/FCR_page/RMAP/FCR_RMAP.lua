dofile(LockOn_Options.script_path.."Displays/TEDAC/Subsets/FCR_page/FCR_BASE.lua")

local scaleSize = 720.0 / 1024.0

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)
local RMAP_Common = addPlaceholder("RMAP Common", {0, 0}, VideoAreaPH.name, {{"DSPLS_FCR_GTM_Root"}})

totalTargetCountStatusWindow(RMAP_Common.name)
scaleElevation(RMAP_Common.name)
boxHAD(RMAP_Common.name)
add_missile_constraints_box(VideoAreaPH.name)

local RMAP_PH = addPlaceholder("RMAP PH", {0, -20}, RMAP_Common.name)
local RMAP_PositionY = 375 * scaleSize -- 500 m
local NormalField_PH = addPlaceholder("Normal Field PH", nil, RMAP_PH.name, {{"DSPLS_FCR_ZoomFieldShow", 0}})
local RMAP_Pos = addPlaceholder("RMAP_Pos", {0, -RMAP_PositionY }, NormalField_PH.name)
addVideoRMAP(RMAP_Pos.name)
DrawBaseSquare(RMAP_Pos.name)
DrawWideScanSquare(RMAP_Pos.name)
DrawMediumScanSquare(RMAP_Pos.name)
DrawNarrowScanSquare(RMAP_Pos.name)
DrawZoomScanSquare(RMAP_Pos.name)

local ZoomField_PH = addPlaceholder("Zoom Field PH", nil, RMAP_PH.name, {{"DSPLS_FCR_ZoomFieldShow", 1}})
drawZoomField(ZoomField_PH.name)

local AircraftPositionY = 400 * scaleSize
local AircraftPositionDI_PH = addPlaceholder("AircraftPositionDI", {0, -AircraftPositionY}, RMAP_PH.name)
DirectionWiperRMAP(AircraftPositionDI_PH.name)
ShowTargetsSymbolsRMAP(AircraftPositionDI_PH.name)
drawPriorityTargets(AircraftPositionDI_PH.name)

