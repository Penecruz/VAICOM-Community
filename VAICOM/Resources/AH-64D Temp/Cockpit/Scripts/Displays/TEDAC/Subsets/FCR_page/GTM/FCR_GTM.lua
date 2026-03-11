dofile(LockOn_Options.script_path.."Displays/TEDAC/Subsets/FCR_page/FCR_BASE.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, nil, nil)
local GTM_ph = addPlaceholder("GTM PH", {0, 0}, VideoAreaPH.name, {{"DSPLS_FCR_GTM_Root"}})

totalTargetCountStatusWindow(GTM_ph.name)
scaleElevation(GTM_ph.name)

local NormalField_PH = addPlaceholder("Normal Field PH", nil, GTM_ph.name, {{"DSPLS_FCR_ZoomFieldShow", 0}})
drawBaseConeScanSector(NormalField_PH.name)
directionWiper(NormalField_PH.name)
drawWideScan(NormalField_PH.name) 
drawMediumScan(NormalField_PH.name)
drawNarrowScan(NormalField_PH.name)
drawZoomScan(NormalField_PH.name)
local ZoomField_PH = addPlaceholder("Zoom Field PH", nil, GTM_ph.name, {{"DSPLS_FCR_ZoomFieldShow", 1}})
drawZoomField(ZoomField_PH.name)

showTargetsSymbols(GTM_ph.name)
drawPriorityTargets(GTM_ph.name)
addPFZ(GTM_ph.name)
addNFZ(GTM_ph.name)
boxHAD(GTM_ph.name)

add_missile_constraints_box(VideoAreaPH.name)


--addText("test dot and circle", "{}" , {0,0} , "LeftCenter" , VideoAreaPH.name)
--addText("test arrow up", "a" , {-150,0} , "CenterBottom" , VideoAreaPH.name)
--addText("test arrow down", "b" , {-150,0} , "CenterTop" , VideoAreaPH.name)
--addText("test arrow left", "c" , {150,0} , "RightCenter" , VideoAreaPH.name)
--addText("test arrow right", "d" , {150,0} , "LeftCenter" , VideoAreaPH.name)
