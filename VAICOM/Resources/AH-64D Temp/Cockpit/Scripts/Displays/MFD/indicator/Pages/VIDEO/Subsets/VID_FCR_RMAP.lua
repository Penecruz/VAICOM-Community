dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/VIDEO/VIDEO_PAGE_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/FCR/MPD_FCR_Defs.lua")

local Controls = {}
Controls = 
{
	{ pb.T6, "VSEL", tp_default_border, {{"VIDEO_Declutter_Button"}}, { "VSEL","C-FLT", "P-FLT", "TADS", "C-FCR", "P-FCR"}},
}
createControls( Controls )

local Common = addPlaceholder("Common", nil, nil, {{"DSPLS_FCR_GTM_Root"}})
local RMAP_PH = addPlaceholder("RMAP_PH", {0, 0}, Common.name, {{"DSPLS_FCR_Mode", 2}})


TotalTargetCountStatusWindow(Common.name)
boxHad(Common.name)

local RMAP_Pos = addPlaceholder("RMAP_Pos", {0, -375}, RMAP_PH.name)

ElevationScale(RMAP_PH.name)
DrawBaseSquare(RMAP_Pos.name)
DrawWideScanSquare(RMAP_Pos.name)
DrawZoomScanSquare(RMAP_Pos.name)
DrawMediumScanSquare(RMAP_Pos.name)
DrawNarrowScanSquare(RMAP_Pos.name)

local AircraftPositionY = 420 
local AircraftPositionDI_PH = addPlaceholder("AircraftPositionDI", {0, -AircraftPositionY}, RMAP_PH.name)
DrawSymbolTargetScanSectorRMAP(AircraftPositionDI_PH.name)
DrawPriorityTargets(AircraftPositionDI_PH.name)
DirectionWiperRMAP(AircraftPositionDI_PH.name, "FCR_GTM_WiperColor")
