dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/FCR/MPD_FCR_Defs.lua")

local GTM_PH = addPlaceholder("GTM PH", nil, nil, {{"DSPLS_FCR_GTM_Root"}})

CreateMenuFCR()
TotalTargetCountStatusWindow(GTM_PH.name)

local NormalField_PH = addPlaceholder("Normal Field PH", nil, GTM_PH.name, {{"DSPLS_FCR_ZoomFieldShow", 0}})
DrawBaseConeScanSector(NormalField_PH.name)
DrawWideScan(NormalField_PH.name)
DrawMediumScan(NormalField_PH.name)
DrawNarrowScan(NormalField_PH.name)
DrawZoomScan(NormalField_PH.name)
DirectionWiper(NormalField_PH.name, "FCR_GTM_WiperColor")
local ZoomField_PH = addPlaceholder("Zoom Field PH", nil, GTM_PH.name, {{"DSPLS_FCR_ZoomFieldShow", 1}})
DrawZoomField(ZoomField_PH.name)

ElevationScale(GTM_PH.name)
DrawSymbolTargetScanSector(GTM_PH.name)
DrawPriorityTargets(GTM_PH.name)

add_missile_constraints_box(GTM_PH.name)

addPFZ(GTM_PH.name)
addNFZ(GTM_PH.name)