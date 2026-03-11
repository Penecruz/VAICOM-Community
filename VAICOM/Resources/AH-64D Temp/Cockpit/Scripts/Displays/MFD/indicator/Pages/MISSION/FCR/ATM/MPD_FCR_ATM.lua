dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/FCR/MPD_FCR_Defs.lua")

local Root_PH = addPlaceholder("Root", nil, nil, {{"DSPLS_FCR_GTM_Root"}})

CreateMenuFCR()
TotalTargetCountStatusWindow(Root_PH.name)

local NormalField_PH = addPlaceholder("Normal Field PH", nil, Root_PH.name, {{"DSPLS_FCR_ZoomFieldShow", 0}})
ZonaCommanATM(NormalField_PH.name)
ZonaWideATM(NormalField_PH.name)
ZonaMediumATM(NormalField_PH.name)
ZonaNarrowATM(NormalField_PH.name)
ZonaZoomATM(NormalField_PH.name)
DirectionWiperATM(NormalField_PH.name, "FCR_GTM_WiperColor")
DrawTailHelicopter(NormalField_PH.name)
ElevationScaleATM(NormalField_PH.name)
DrawSymbolTargetScanSector(NormalField_PH.name)
DrawPriorityTargets(NormalField_PH.name)

add_missile_constraints_box(Root_PH.name)

addPFZ(NormalField_PH)
addNFZ(NormalField_PH)