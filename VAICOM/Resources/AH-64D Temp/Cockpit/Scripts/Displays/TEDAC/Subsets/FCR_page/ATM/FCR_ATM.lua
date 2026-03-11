dofile(LockOn_Options.script_path.."Displays/TEDAC/Subsets/FCR_page/FCR_BASE.lua")

local scaleSize = 720.0 / 1024.0

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)
local ATM_PH = addPlaceholder("ATM PH", {0, 0}, VideoAreaPH.name, {{"DSPLS_FCR_GTM_Root"}})

totalTargetCountStatusWindow(ATM_PH.name)

add_missile_constraints_box(ATM_PH.name)

ZonaCommanATM(ATM_PH.name)
boxHAD(ATM_PH.name)
ZonaWideATM(ATM_PH.name)
ZonaMediumATM(ATM_PH.name)
ZonaNarrowATM(ATM_PH.name)
ZonaZoomATM(ATM_PH.name)
DirectionWiperATM(ATM_PH.name)
DrawTailHelicopter(ATM_PH.name)
ElevationScaleATM(ATM_PH.name)

showTargetsSymbols(ATM_PH.name)
drawPriorityTargets(ATM_PH.name)
addPFZ(ATM_PH.name)
addNFZ(ATM_PH.name)
