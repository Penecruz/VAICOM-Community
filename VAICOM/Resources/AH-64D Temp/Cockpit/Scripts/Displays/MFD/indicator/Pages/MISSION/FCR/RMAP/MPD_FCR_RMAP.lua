dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/FCR/MPD_FCR_Defs.lua")

local RMAP_PH = addPlaceholder("RMAP PH", {0, 0}, {{"DSPLS_FCR_GTM_Root"}})

CreateMenuFCR()
TotalTargetCountStatusWindow(RMAP_PH.name)
add_missile_constraints_box(RMAP_PH.name)
ElevationScale(RMAP_PH.name)

local NormalField_PH = addPlaceholder("Normal Field PH RMAP", nil, RMAP_PH.name, {{"DSPLS_FCR_ZoomFieldShow", 0}})
local RMAP_PositionY = 375 -- 500 m
local RMAP_Pos = addPlaceholder("RMAP_Pos", {0, -RMAP_PositionY}, NormalField_PH.name)
addVideoRMAP(RMAP_Pos.name)
DrawBaseSquare(RMAP_Pos.name)
DrawWideScanSquare(RMAP_Pos.name)
DrawZoomScanSquare(RMAP_Pos.name)
DrawMediumScanSquare(RMAP_Pos.name)
DrawNarrowScanSquare(RMAP_Pos.name)

local ZoomField_PH = addPlaceholder("Zoom Field PH RMAP", nil, RMAP_PH.name, {{"DSPLS_FCR_ZoomFieldShow", 1}})
DrawZoomField(ZoomField_PH.name)

local AircraftPositionY = 420 
local AircraftPositionDI_PH = addPlaceholder("AircraftPositionDI", {0, -AircraftPositionY}, RMAP_PH.name)
DrawSymbolTargetScanSectorRMAP(AircraftPositionDI_PH.name)
DrawPriorityTargets(AircraftPositionDI_PH.name)

DirectionWiperRMAP(AircraftPositionDI_PH.name, "FCR_GTM_WiperColor")

