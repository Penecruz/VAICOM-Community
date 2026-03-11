dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/VIDEO/VIDEO_PAGE_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/FCR/MPD_FCR_Defs.lua")

local Controls = {}
Controls = 
{
	{ pb.T6, "VSEL", tp_default_border, {{"VIDEO_Declutter_Button"}}, { "VSEL","C-FLT", "P-FLT", "TADS", "C-FCR", "P-FCR"}},
}
createControls( Controls )

local Common = addPlaceholder("Common", nil, nil, {{"DSPLS_FCR_GTM_Root"}})
local ATM_PH = addPlaceholder("ATM_PH", {0, 0}, Common.name, {{"DSPLS_FCR_Mode", 3}})

TotalTargetCountStatusWindow(Common.name)
boxHad(Common.name)

ZonaCommanATM(ATM_PH.name)
ZonaWideATM(ATM_PH.name)
ZonaMediumATM(ATM_PH.name)
ZonaNarrowATM(ATM_PH.name)
ZonaZoomATM(ATM_PH.name)
DirectionWiperATM(ATM_PH.name, "FCR_GTM_WiperColor")
DrawTailHelicopter(ATM_PH.name)
ElevationScaleATM(ATM_PH.name)
DrawSymbolTargetScanSector(ATM_PH.name)
DrawPriorityTargets(ATM_PH.name)
