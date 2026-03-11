dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/VIDEO/VIDEO_PAGE_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/FCR/MPD_FCR_Defs.lua")

local Controls = {}
Controls = 
{
	{ pb.T6, "VSEL", tp_default_border, {{"VIDEO_Declutter_Button"}}, { "VSEL","C-FLT", "P-FLT", "TADS", "C-FCR", "P-FCR"}},
}
createControls( Controls )

local Common = addPlaceholder("Common", nil, nil, {{"DSPLS_FCR_GTM_Root"}})
local GTM_PH = addPlaceholder("GTM_PH", nil, Common.name,  {{"DSPLS_FCR_Mode", 1}})

TotalTargetCountStatusWindow(Common.name)
boxHad(Common.name)

ElevationScale(GTM_PH.name)
DrawBaseConeScanSector(GTM_PH.name)
DrawWideScan(GTM_PH.name)
DrawMediumScan(GTM_PH.name)
DrawNarrowScan(GTM_PH.name)
DrawZoomScan(GTM_PH.name)
DirectionWiper(GTM_PH.name, "FCR_GTM_WiperColor")
DrawSymbolTargetScanSector(GTM_PH.name)
DrawPriorityTargets(GTM_PH.name)
addPFZ(GTM_PH.name)
addNFZ(GTM_PH.name)