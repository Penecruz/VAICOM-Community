dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")

-- Center of the video area
local scaleSize = 720.0 / 1024.0
local scanCenterX = 0
local scanCenterY = -370 * scaleSize

function scaleElevation(parent)

    bazeScale = addPlaceholder ("bazeScale1", {-380, -320}, parent)
     
    local scaleStep = 15  * scaleSize
    local currentStep = 0
    for i = 1,9 do
    local ElevationScale = addPlaceholder("elevationScale"..i, {0, currentStep}, bazeScale.name)
    
    currentStep = scaleStep + currentStep

    if i == 1 or i == 4 or i == 9 then
        addSimpleLine(nil, {{0, 0}, {30 * scaleSize , 0}}, 2,  "TEDAC_SYMBOLOGY",    ElevationScale.name)
    end 
        addSimpleLine(nil, {{0, 0}, {20 * scaleSize , 0}}, 2,  "TEDAC_SYMBOLOGY",    ElevationScale.name)
    end
    arrow = buildBorderedSymbolFCR("arror", {18, 22}, "LeftCenter",  {8, 25}, {30, 0}, bazeScale.name, {{"DSPLS_FCR_AG_SetIndicationElevationScale", currentStep - scaleStep}}, 3, "TEDAC_FCR_INDICATION_WHITE")
end

function totalTargetCountStatusWindow(parent) 
    local CountTargetsPH = addPlaceholder("CountTargetsPH", {video_area_w_05 * scaleSize, video_area_h_05 * scaleSize + 40}, parent, {{"DSPLS_FCR_AG_ShowCountTargetsRoot"}})
    
    addText("CountTarget", "0000", {-15 * scaleSize , 0}, "RightCenter", CountTargetsPH.name, {{"DSPLS_FCR_AG_ShowCountTargets"}})

    addRoundedBox("CountTargetsBox", {0, 3}, "RightCenter", {130 * scaleSize, 50 * scaleSize }, 10, 4, CountTargetsPH.name)

end

function drawBaseConeScanSector(parent)
    BazeConePH = addPlaceholder("BazeCone", {scanCenterX, scanCenterY}, parent)
    addArc( "BazeConeArc", {0, 0}, 650 * scaleSize , 45, 135, 20, 4,  BazeConePH.name,   nil,    "TEDAC_SYMBOLOGY")
    addSimpleLine( nil, {{459.6 * scaleSize,    459.6 * scaleSize}, {30 * scaleSize ,   30 * scaleSize}}, 2, "TEDAC_SYMBOLOGY", BazeConePH.name)
    addSimpleLine( nil, {{-459.6 * scaleSize,  459.6 * scaleSize}, {-30 * scaleSize,   30 * scaleSize}}, 2, "TEDAC_SYMBOLOGY", BazeConePH.name) 

    local cutoffLineSize = 16
    local posShort = 300

    local rightShort = addPlaceholder("CutoffLinesRight", {posShort * scaleSize, posShort * scaleSize}, BazeConePH.name)
    addSimpleLine( nil, {{0, 0},  {cutoffLineSize * scaleSize,   -cutoffLineSize * scaleSize }}, 6, "TEDAC_SYMBOLOGY", rightShort.name)

    local leftShort = addPlaceholder("CutoffLinesLeft", {-posShort * scaleSize, posShort * scaleSize}, BazeConePH.name)
    addSimpleLine( nil, {{0, 0},  {-cutoffLineSize * scaleSize, -cutoffLineSize * scaleSize }}, 6, "TEDAC_SYMBOLOGY", leftShort.name)
end

function directionWiper(parent)
    local wiperPH = addPlaceholder("WiperDown", {scanCenterX, scanCenterY}, parent, {{"DSPLS_FCR_AG_SetDirectionWiper", 1}})
    addSimpleLine(nil,  {{0,    325 * scaleSize },  {0, 30 * scaleSize}}, 3, "TEDAC_SYMBOLOGY", wiperPH.name, {{"DSPLS_FCR_AG_WiperPosLen", 650 * scaleSize}})
end

function drawWideScan(parent) 

    local WideZonePH = addPlaceholder("WideScan", {scanCenterX, scanCenterY}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 0}})
    
    addSimpleLine( nil, {{0,    650 * scaleSize}, {0,  630 * scaleSize}},  4, "TEDAC_SYMBOLOGY", WideZonePH.name)
    
    local xPosTopShort = {84.8, 168.2, 248.7, 459.6}
    local yPosTopShort = {644.4, 627, 600.5, 459.6}
    local xPosDownShort = {82, 163.5, 240, 30}
    local yPosDownShort = {624, 607, 580, 30}

    for i = 1, #xPosTopShort do
        addSimpleLine( nil, {{xPosTopShort[i] * scaleSize,      yPosTopShort[i] * scaleSize},       {xPosDownShort[i] * scaleSize,  yPosDownShort[i] * scaleSize}}, 4, "TEDAC_SYMBOLOGY", WideZonePH.name)
        addSimpleLine( nil, {{-xPosTopShort[i]* scaleSize,      yPosTopShort[i] * scaleSize},       {-xPosDownShort[i] * scaleSize, yPosDownShort[i] * scaleSize}}, 4, "TEDAC_SYMBOLOGY", WideZonePH.name)
    end  
    
    
    local cutoffRadiusLines = {57, 171.8, 286, 401}
    local cutoffLineSize = 12

    for i = 1, #cutoffRadiusLines do
        local i = addPlaceholder("CutoffLinesRight"..i, {cutoffRadiusLines[i]* scaleSize, cutoffRadiusLines[i] * scaleSize}, WideZonePH.name)
       addSimpleLine( nil, {{0, 0},  {-cutoffLineSize * scaleSize,   cutoffLineSize * scaleSize }}, 4, "TEDAC_SYMBOLOGY", i.name)
    end
   
   for j = 1, #cutoffRadiusLines do
    local j = addPlaceholder("CutoffLinesLeft"..j, {-cutoffRadiusLines[j]* scaleSize, cutoffRadiusLines[j] * scaleSize }, WideZonePH.name)
        addSimpleLine( nil, {{0, 0}, {cutoffLineSize * scaleSize ,   cutoffLineSize * scaleSize }}, 4, "TEDAC_SYMBOLOGY", j.name)
   end
   
   arcRadius = {650, 486, 324, 162}
   for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcWideFirst"..i,    {0, 0},  arcRadius[i] * scaleSize,   45, 135,  20, 10, WideZonePH.name, nil, "TEDAC_SYMBOLOGY")
        else
            addArc("ConeArcWideSecond"..i,    {0, 0},  arcRadius[i] * scaleSize,   45, 135,  20, 4, WideZonePH.name, nil, "TEDAC_SYMBOLOGY")
        end
   end
end

function drawMediumScan(parent)

    local MediumZonePH = addPlaceholder("MediumScan", {scanCenterX, scanCenterY}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}})

    addSimpleLine( nil, {{0,    650 * scaleSize},  {0, 630 * scaleSize}},  4, "TEDAC_SYMBOLOGY", MediumZonePH.name)

    local xPosTopShort = {84.8, 168.2, 248.7}
    local yPosTopShort = {644.4, 627, 600.5}
    local xPosDownShort = {82, 163.5, 15}
    local yPosDownShort = {624, 607, 30}

    for i = 1, #xPosTopShort do
        addSimpleLine( nil, {{xPosTopShort[i]   * scaleSize,      yPosTopShort[i] * scaleSize}, {xPosDownShort[i] * scaleSize ,  yPosDownShort[i] * scaleSize}},    4, "TEDAC_SYMBOLOGY", MediumZonePH.name)
        addSimpleLine( nil, {{-xPosTopShort[i]  * scaleSize,      yPosTopShort[i] * scaleSize}, {-xPosDownShort[i] * scaleSize,  yPosDownShort[i] * scaleSize}},    4, "TEDAC_SYMBOLOGY", MediumZonePH.name)
    end  
  
    local MediumZoneX = {75, 224.5, 374, 524}
    local MediumZoneY = {31, 93, 155, 215}
    local MediumZoneLineSize = 12

    for i = 1, #MediumZoneX do
        local i = addRotPlaceholder("MediumZoneL"..i, {MediumZoneY[i] * scaleSize,MediumZoneX[i] * scaleSize}, 20, MediumZonePH.name)
        addSimpleLine( nil, {{0, 0},  {-MediumZoneLineSize * scaleSize,   MediumZoneLineSize * scaleSize}}, 4, "TEDAC_SYMBOLOGY", i.name)
    end

    for i = 1, #MediumZoneX do
        local i = addRotPlaceholder("MediumZoneR"..i, {-MediumZoneY[i] * scaleSize, MediumZoneX[i] * scaleSize}, -20, MediumZonePH.name)
        addSimpleLine( nil, {{0, 0},  {MediumZoneLineSize * scaleSize,   MediumZoneLineSize * scaleSize }}, 4, "TEDAC_SYMBOLOGY", i.name)
    end

    arcRadius = {650, 486, 324, 162}

    for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcMediumFirst"..i,    {0, 0},  arcRadius[i] * scaleSize,   67.5, 112.5, 20, 10, MediumZonePH.name, nil, "TEDAC_SYMBOLOGY")
        else
            addArc("ConeArcMediumSecond"..i,   {0, 0},  arcRadius[i] * scaleSize,   67.5, 112.5, 20, 4, MediumZonePH.name, nil, "TEDAC_SYMBOLOGY")
        end
    end
end

function drawNarrowScan(parent)

    local NarrowZonePH = addPlaceholder("NarrowScan", {scanCenterX, scanCenterY}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 2}})
     
    addSimpleLine( nil, {{0,    650 * scaleSize },  {0, 630 * scaleSize}},  4, "TEDAC_SYMBOLOGY", NarrowZonePH.name)


    local xPosTopShort = {84.8, 168.2}
    local yPosTopShort = {644.4, 627}
    local xPosDownShort = {82, 10}
    local yPosDownShort = {624, 30}

    for i = 1, #xPosTopShort do      
        addSimpleLine( nil, {{xPosTopShort[i] * scaleSize,  yPosTopShort[i] * scaleSize},   {xPosDownShort[i] * scaleSize,  yPosDownShort[i] * scaleSize}},     4, "TEDAC_SYMBOLOGY", NarrowZonePH.name)
        addSimpleLine( nil, {{-xPosTopShort[i] * scaleSize,  yPosTopShort[i] * scaleSize},   {-xPosDownShort[i] * scaleSize,  yPosDownShort[i] * scaleSize}},   4, "TEDAC_SYMBOLOGY", NarrowZonePH.name) 
    end   

    local xPosTop = {168.2, 248.7}
    local yPosTop = {627, 600.5}

    local xPosDown = {10, 15}
    local yPosDown = {30, 30}

    for i = 1, #xPosTop do
        addSimpleLine( nil, {{xPosTop[i] * scaleSize,   yPosTop[i] * scaleSize },   {xPosDown[i] * scaleSize,   yPosDown[i] * scaleSize}},    4, "TEDAC_SYMBOLOGY", NarrowZonePH.name)
        addSimpleLine( nil, {{-xPosTop[i] * scaleSize,  yPosTop[i] * scaleSize },   {-xPosDown[i] * scaleSize,  yPosDown[i] * scaleSize}},    4, "TEDAC_SYMBOLOGY", NarrowZonePH.name) 
    end

    local NarrowZoneX = {78, 234.7, 391, 547.5}
    local NarrowZoneY = {21, 63, 105, 146.7}
    local NarrowLineSize = 12

    for i = 1, #NarrowZoneX do
        local i = addRotPlaceholder("NarrowZoneL"..i, {NarrowZoneY[i] * scaleSize, NarrowZoneX[i] * scaleSize}, 30, NarrowZonePH.name)
        addSimpleLine( nil, {{0, 0},  {-NarrowLineSize * scaleSize,   NarrowLineSize * scaleSize }}, 4, "TEDAC_SYMBOLOGY", i.name)
    end

    for i = 1, #NarrowZoneX do
        local i = addRotPlaceholder("NarrowZoneR"..i, {-NarrowZoneY[i] * scaleSize, NarrowZoneX[i] * scaleSize}, -30, NarrowZonePH.name)
        addSimpleLine( nil, {{0, 0},  {NarrowLineSize * scaleSize,   NarrowLineSize * scaleSize }}, 4, "TEDAC_SYMBOLOGY", i.name)
    end

    arcRadius = {650, 486, 324, 162}
    for i = 1, #arcRadius do

        if i == 1 then
            addArc("ConeArcNarrowFirst"..i,    {0, 0},  arcRadius[i] * scaleSize,   75,    105,    20, 10, NarrowZonePH.name, nil, "TEDAC_SYMBOLOGY")
        else
            addArc("ConeArcNarrowSecond"..i,    {0, 0},  arcRadius[i] * scaleSize,   75,    105,    20, 4, NarrowZonePH.name, nil, "TEDAC_SYMBOLOGY")
        end
    end
end

function drawZoomScan(parent)
    
    local ZoomZonePH = addPlaceholder("ZoomScan", { scanCenterX, scanCenterY }, parent, {{"DSPLS_FCR_AG_ChangeScanCenter",3}})

    addSimpleLine( nil, {{0,                      650 * scaleSize },        {0,                 630 * scaleSize}},  4, "TEDAC_SYMBOLOGY", ZoomZonePH.name)

    local xPosTop = {84.8, 168.2, 248.7}
    local yPosTop = {644.4, 627, 600.5}

    local xPosDown = {5, 10, 15}
    local yPosDown = {30, 30, 30}

    for i = 1, #xPosTop do
        addSimpleLine( nil, {{xPosTop[i] * scaleSize,      yPosTop[i] * scaleSize },      {xPosDown[i] * scaleSize,   yPosDown[i] * scaleSize}},    4, "TEDAC_SYMBOLOGY", ZoomZonePH.name)
        addSimpleLine( nil, {{-xPosTop[i]* scaleSize,      yPosTop[i] * scaleSize },      {-xPosDown[i] * scaleSize,  yPosDown[i] * scaleSize}},    4, "TEDAC_SYMBOLOGY", ZoomZonePH.name) 
    end

    local ZoomZoneX = {80, 241, 401, 562}
    local ZoomZoneY = {10, 31, 53, 74}
    local ZoomZoneLineSize = 12

    for i = 1, #ZoomZoneX do
        local i = addRotPlaceholder("ZoomZoneL"..i, {ZoomZoneY[i] * scaleSize, ZoomZoneX[i] * scaleSize }, 40, ZoomZonePH.name)
        addSimpleLine( nil, {{0, 0},  {-ZoomZoneLineSize * scaleSize,   ZoomZoneLineSize * scaleSize }}, 4, "TEDAC_SYMBOLOGY", i.name)
    end

    for i = 1, #ZoomZoneX do
        local i = addRotPlaceholder("ZoomZoneR"..i, {-ZoomZoneY[i] * scaleSize ,ZoomZoneX[i] * scaleSize }, -40,ZoomZonePH.name)
        addSimpleLine( nil, {{0, 0},  {ZoomZoneLineSize * scaleSize,   ZoomZoneLineSize * scaleSize }}, 4, "TEDAC_SYMBOLOGY", i.name)
    end

    arcRadius = {650, 486, 324, 162}
    for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcZoomFirst"..i,    {0, 0},  arcRadius[i] * scaleSize,  82.5,   97.5,   20, 10, ZoomZonePH.name, nil, "TEDAC_SYMBOLOGY")
        else
            addArc("ConeArcZoomSecond"..i,    {0, 0},  arcRadius[i] * scaleSize,  82.5,   97.5,   20, 4, ZoomZonePH.name, nil, "TEDAC_SYMBOLOGY")
        end
    end
end

function drawZoomField(parent)
    local size = 680 * (720.0 / 1024.0)
    local verts = buildBoxVerts(size, size,  "CenterCenter")
    verts[5] = verts[1]
    addSimpleLine(nil, verts, 4, "TEDAC_SYMBOLOGY", parent)
end

function drawPriorityTargets(parent)

	local mask_level 			= 0
	local sizeSymbol            = 30
	local NtsFrameSize 		    = {60, 64}
	local AntsFrameSize 	    = {59, 75}
	local scale                 = 1

	local NtsControllers 	= {{"DSPLS_FCR_AG_NTS_Show"},   {"DSPLS_FCR_AG_NTS_SetPosition"}}
	local AntsControllers 	= {{"DSPLS_FCR_AG_ANTS_Show"},  {"DSPLS_FCR_AG_ANTS_SetPosition"}}

	local function add_mask(name, parent, controllers, h_clip_relations, level, isvisible)
		local size	= NtsFrameSize[1]
		local s05	= size * 0.48

		local verts = { {-s05, 0}, {0,  s05}, {s05,  0}, {0, -s05} }

		local mask = addMesh(name, verts, default_box_indices, {1, -2}, "triangles", parent, controllers, "font_TEDAC_BLACK")
		mask.isvisible			= isvisible		-- true for DBG
		mask.additive_alpha		= false
		mask.change_opacity		= false
		mask.h_clip_relation	= h_clip_relations
		mask.level				= level
	end

    add_mask("FCR_ANTS_openmask", parent, NtsControllers, h_clip_relations.INCREASE_IF_LEVEL, DEFAULT_LEVEL + mask_level, false) 
    local secondTarget = buildBorderedSymbolFCR("SecondTarget", AntsFrameSize, "CenterCenter", {160, 20}, {0, 0}, parent, AntsControllers, scale, "TEDAC_FCR_INDICATION_BLACK")
	local secondTarget = buildBorderedSymbolFCR("SecondTarget1", AntsFrameSize, "CenterCenter", {160, 20}, {0, 0}, parent, AntsControllers, scale, "TEDAC_FCR_INDICATION_WHITE")
	add_mask("FCR_ANTS_closemask", parent, NtsControllers, h_clip_relations.REWRITE_LEVEL, DEFAULT_LEVEL + mask_level, false)

    addMapTacticalSymbol("FCRTargetSecondTarget2",  "font_TEDAC_FCR_Target_symbol_black",   {{"DSPLS_FCR_AG_ShowSecondTargetPriority"}},    parent, {0, 0}, sizeSymbol)
	addMapTacticalSymbol("FCRTargetSecondTarget ",  "font_TEDAC_FCR_Target_symbol",         {{"DSPLS_FCR_AG_ShowSecondTargetPriority"},{"DSPLS_FCR_AG_DrawSecondTargetColor", 1}},  parent, {0, 0}, sizeSymbol)

    local FirstBase 		= addPlaceholder("FCR_NTS_BasePH", 			{0, 0}, parent, 		    NtsControllers)
	local FirstSolidBase 	= addPlaceholder("FCR_NTS_Solid_BasePH", 	{0, 0}, FirstBase.name, 	{{"DSPLS_FCR_AG_NTS_Frame_Show", 1}})
	local FirstDashedBase 	= addPlaceholder("FCR_NTS_Dashed_BasePH", 	{0, 0}, FirstBase.name, 	{{"DSPLS_FCR_AG_NTS_Frame_Show", 2}})

    local FirstTarget = buildBorderedSymbolFCR("FirstTarget", NtsFrameSize, "CenterCenter", {96, 29}, {0, 0},  FirstSolidBase.name, nil, scale, "TEDAC_FCR_INDICATION_BLACK")
	local FirstTarget = buildBorderedSymbolFCR("FirstTarget1", NtsFrameSize, "CenterCenter", {96, 29}, {0, 0}, FirstSolidBase.name, nil, scale, "TEDAC_FCR_INDICATION_WHITE")  

    local FirstTargetDashed = buildBorderedSymbolFCR("FirstTargetDashedBlack", NtsFrameSize, "CenterCenter", {226, 29}, {0, 0},   FirstDashedBase.name, nil, scale, "TEDAC_FCR_INDICATION_BLACK")
	local FirstTargetDashed = buildBorderedSymbolFCR("FirstTargetDashedWhite", NtsFrameSize, "CenterCenter", {226, 29}, {0, 0},   FirstDashedBase.name, nil, scale, "TEDAC_FCR_INDICATION_WHITE")

    addMapTacticalSymbol("FCRTargetFirstTarget1",   "font_TEDAC_FCR_Target_symbol_black",   {{"DSPLS_FCR_AG_ShowFirstTargetPriority"}},     parent, {0, 0}, sizeSymbol)
	addMapTacticalSymbol("FCRTargetFirstTarget",    "font_TEDAC_FCR_Target_symbol",         {{"DSPLS_FCR_AG_ShowFirstTargetPriority"}, {"DSPLS_FCR_AG_DrawFirstTargetColor", 1}},    parent, {0, 0}, sizeSymbol)
end

function drawShotAtSymbol(name, pos, parent, controllerPH, height, width)
	pos = pos or {0, 0}
	height = height or 36
	width = width or 25

	local angle = math.floor(math.deg(math.atan(height / width)))
	local hypotenuse = math.floor(math.sqrt(height ^ 2 + width ^ 2))
	local line_width = 4

	local SymbolPH = addPlaceholder(name, pos, parent, controllerPH)

	addFatLine(name.."_line1_bg", 	hypotenuse, line_width, {-width / 2, -height / 2}, 	-90 + angle, SymbolPH.name, nil, "TEDAC_BLACK")
	addFatLine(name.."_line1", 		hypotenuse, line_width, {-width / 2, -height / 2}, 	-90 + angle, SymbolPH.name, nil, "TEDAC_WHITE")
	addFatLine(name.."_line2_bg", 	hypotenuse, line_width, {-width / 2, height / 2}, 	-90 - angle, SymbolPH.name, nil, "TEDAC_BLACK")
	addFatLine(name.."_line2", 		hypotenuse, line_width, {-width / 2, height / 2}, 	-90 - angle, SymbolPH.name, nil, "TEDAC_WHITE")
end

function showTargetsSymbols(parent)
    for i = 0, 15 do
        drawShotAtSymbol("Shoot At Symbol behind "..i, nil, parent, {{"DSPLS_FCR_AG_COMMON_TargetShootAtSymbolBehind", i}})
    end
    for i = 0, 15 do
		addMapTacticalSymbol("FCR Target SymbolBlack"..i,   "font_TEDAC_FCR_Target_symbol_black",   {{"DSPLS_FCR_AG_COMMON_DrawTargetsSymbolRoot", i}, {"DSPLS_FCR_AG_COMMON_DrawTargetsSymbol", i}}, parent, {0, 0}, 30)
		addMapTacticalSymbol("FCR Target Symbol "..i,       "font_TEDAC_FCR_Target_symbol",         {{"DSPLS_FCR_AG_COMMON_DrawTargetsSymbolRoot", i}, {"DSPLS_FCR_AG_COMMON_DrawTargetsSymbol", i}, {"DSPLS_FCR_AG_COMMON_DrawTargetsSymbolColor", i}}, parent, {0, 0}, 30)
    end
    for i = 0, 15 do
        drawShotAtSymbol("Shoot At Symbol above "..i, nil, parent, {{"DSPLS_FCR_AG_COMMON_TargetShootAtSymbolAbove", i}})
    end
end

function addPFZ(parent)
    local PZF = addPlaceholder("ActivePFZ", {0, 0}, parent, {{"DSPLS_FCR_AG_ShowPFZ"}})

	for i=0, 3 do
		draw_PFZ_frame_line_act( "ActivePFZ_"..i,	"TEDAC_TSD_INDICATION_WHITE",	PZF.name,	{{"DLPLS_FCR_AG_AcivePFZ_Line_Draw", i}}, {{"DLPLS_FCR_AG_AcivePFZ_Mask_Draw", i}} )
	end
end

function addNFZ(parent)
    for i = 0, 7 do
		local NZF = addPlaceholder("ActiveNFZ"..i, {0, 0}, parent, {{"DSPLS_FCR_AG_ShowNFZ", i}})

		for j=0, 3 do
			draw_NFZ_frame_line_act( "Active_NFZ_"..i.."_FrameLine_"..j,	"TEDAC_TSD_INDICATION_WHITE",	NZF.name,	{{"DLPLS_FCR_AG_AciveNFZ_Line_Draw", i, j}}, {{"DLPLS_FCR_AG_AciveNFZ_Mask_Draw", i, j}} )
		end
	end
end

function boxHAD(parent)
    local boxHAD = addPlaceholder("boxHAD", video_area_pos, nil )
    addRoundedBox("rightBoxHADFormat",  {-220, -320},   "CenterCenter", {215, 60}, 10, 2, boxHAD.name)
    addRoundedBox("leftBoxHADFormat",   {220, -320},    "CenterCenter", {215, 60}, 10, 2, boxHAD.name)
end

--------------------------------RMAP--------------------------------------------

local heightRMAP = 740 * scaleSize
local widthRMAP = 770 * scaleSize
local yPosLeftShort = {54 * scaleSize, 250 * scaleSize, 446 * scaleSize, 642 * scaleSize}
local yPos = {152 * scaleSize, 348 * scaleSize, 544 * scaleSize}

function DrawBaseSquare(parent)
    addRoundedBox("BaseRmap",   {0, 0},	"CenterBottom", {widthRMAP, heightRMAP}, 0, 2, parent)
    addSimpleLine( nil, {{widthRMAP * 0.5, 480 * scaleSize},      {(widthRMAP * 0.5) + 15 * scaleSize, 480 * scaleSize}},  2, "TEDAC_SYMBOLOGY", parent)
    addSimpleLine( nil, {{-widthRMAP * 0.5, 480 * scaleSize},     {-(widthRMAP * 0.5) - 15 * scaleSize, 480 * scaleSize}}, 2, "TEDAC_SYMBOLOGY", parent)
    addSimpleLine( nil, {{0, heightRMAP - 15},                    {0, heightRMAP}},    2, "TEDAC_SYMBOLOGY", parent)
end

function addVideoRMAP(parent)
	local TEDAC_ID = "_TEDAC";

    local function addRmapVideo(namePostfix, width, zoneControllerIndex)
        local	RMAP_Video			= CreateElement "ceTexPoly"
        RMAP_Video.name				= "RMAP_Video_"..namePostfix
        RMAP_Video.vertices			= buildBoxVerts(width, heightRMAP, "CenterBottom")
        RMAP_Video.indices			= default_box_indices
        RMAP_Video.tex_coords		=  {{0, 1}, {0, 0},{1, 0}, {1, 1}}
        RMAP_Video.material			= MakeMaterial("RMAP_CAM_AH64"..TEDAC_ID, {255, 255, 255, 255})
        RMAP_Video.level			= DEFAULT_LEVEL
        RMAP_Video.h_clip_relation	= h_clip_relations.COMPARE
        RMAP_Video.parent_element	= parent
        RMAP_Video.controllers		= {{"DSPLS_FCR_RMAP_Video"}, {"DSPLS_FCR_RMAP_Scan_Zone", zoneControllerIndex}}
        RMAP_Video.additive_alpha	= true
        Add(RMAP_Video)
    end

    addRmapVideo("Wide", widthRMAP, 0)
    addRmapVideo("Medium", 600 * scaleSize, 1)
    addRmapVideo("Narrow", 550 * scaleSize, 2)
    addRmapVideo("Zoom", 200 * scaleSize, 3)
end

function DrawWideScanSquare(parent)
    local WideZonePH = addPlaceholder("WideScanRMAPTedac", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 0}})
    local xPos = widthRMAP * 0.5
    local xPosTopShort = {300 * scaleSize, 275 * scaleSize, 100 * scaleSize}

    for i = 1, #yPos do
        addSimpleLine( nil, {{-xPos , yPos[i]},    {xPos, yPos[i]}}, 4, "TEDAC_SYMBOLOGY", WideZonePH.name)
    end

    for i = 1, #yPosLeftShort do
        addSimpleLine( nil, {{-xPos, yPosLeftShort[i]},        {-xPos + 15, yPosLeftShort[i]}}, 4, "TEDAC_SYMBOLOGY", WideZonePH.name)
    end

    for i = 1, #xPosTopShort do
        addSimpleLine( nil, {{-xPosTopShort[i],  heightRMAP - 15},         {-xPosTopShort[i],   heightRMAP}}, 4, "TEDAC_SYMBOLOGY", WideZonePH.name)
        addSimpleLine( nil, {{xPosTopShort[i],  heightRMAP - 15},          {xPosTopShort[i],   heightRMAP}}, 4, "TEDAC_SYMBOLOGY", WideZonePH.name)
    end

    addRoundedBox("WideSquare",   {0, 0},	"CenterBottom", {widthRMAP, heightRMAP}, 0, 2, WideZonePH.name)
end

function DrawMediumScanSquare(parent)

    width = 600 * scaleSize
    local MediumZonePH = addPlaceholder("MediumScanRMAPTedac", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}})

    local xPos = 300 * scaleSize
    local xPosTopShort = { 275 * scaleSize, 100 * scaleSize}

    for i = 1, #yPos do
        addSimpleLine( nil, {{-xPos , yPos[i]},    {xPos, yPos[i]}}, 4, "TEDAC_SYMBOLOGY", MediumZonePH.name)
    end

    for i = 1, #yPosLeftShort do
        addSimpleLine( nil, {{-xPos, yPosLeftShort[i]},        {-xPos + 15, yPosLeftShort[i]}}, 4, "TEDAC_SYMBOLOGY", MediumZonePH.name)
    end

    for i = 1, #xPosTopShort do
        addSimpleLine( nil, {{-xPosTopShort[i], heightRMAP - 15},         {-xPosTopShort[i],  heightRMAP}}, 4, "TEDAC_SYMBOLOGY", MediumZonePH.name)
        addSimpleLine( nil, {{xPosTopShort[i],  heightRMAP - 15},          {xPosTopShort[i],  heightRMAP}}, 4, "TEDAC_SYMBOLOGY", MediumZonePH.name)
    end

    addRoundedBox("MediumSquare",   {0, 0},	"CenterBottom", {width, heightRMAP}, 0, 2, MediumZonePH.name)
end

function DrawNarrowScanSquare(parent)
    width = 550 * scaleSize
    local NarrowZonePH = addPlaceholder("NarrowScanRMAPTedac", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 2}})
    local xPos = 275 * scaleSize

    for i = 1, #yPos do
        addSimpleLine( nil, {{-xPos , yPos[i]},    {xPos, yPos[i]}}, 4, "TEDAC_SYMBOLOGY", NarrowZonePH.name)
    end

    for i = 1, #yPosLeftShort do
        addSimpleLine( nil, {{-xPos, yPosLeftShort[i]},        {-xPos + 15, yPosLeftShort[i]}}, 4, "TEDAC_SYMBOLOGY", NarrowZonePH.name)
    end

    addSimpleLine( nil, {{-100, heightRMAP - 15},         {-100,  heightRMAP}}, 4, "TEDAC_SYMBOLOGY", NarrowZonePH.name)
    addSimpleLine( nil, {{100,  heightRMAP - 15},          {100,  heightRMAP}}, 4, "TEDAC_SYMBOLOGY", NarrowZonePH.name)

    addSimpleLine( nil, {{-300 * scaleSize, 0},  {-300 * scaleSize, heightRMAP}},   2, "TEDAC_SYMBOLOGY", NarrowZonePH.name)
    addSimpleLine( nil, {{300 * scaleSize, 0},   {300 * scaleSize, heightRMAP}},    2, "TEDAC_SYMBOLOGY", NarrowZonePH.name)

    addRoundedBox("NarrowSquare",   {0, 0},	"CenterBottom", {width, heightRMAP}, 0, 2, NarrowZonePH.name)
end

function DrawZoomScanSquare(parent)
    width = 200 * scaleSize
    local ZoomZonePH = addPlaceholder("ZoomScanRMAPTedac", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 3}})

    local xPos = 100 * scaleSize

    for i = 1, #yPos do
        addSimpleLine( nil, {{-xPos , yPos[i]},    {xPos, yPos[i]}}, 4, "TEDAC_SYMBOLOGY", ZoomZonePH.name)
    end

    for i = 1, #yPosLeftShort do
        addSimpleLine( nil, {{-xPos, yPosLeftShort[i]},        {-xPos + 15, yPosLeftShort[i]}}, 4, "TEDAC_SYMBOLOGY", ZoomZonePH.name)
    end

    addSimpleLine( nil, {{-300 * scaleSize, 0},  {-300 * scaleSize, heightRMAP}}, 2, "TEDAC_SYMBOLOGY", ZoomZonePH.name)
    addSimpleLine( nil, {{300 * scaleSize, 0},   {300 * scaleSize, heightRMAP}}, 2, "TEDAC_SYMBOLOGY", ZoomZonePH.name)

    addSimpleLine( nil, {{-275 * scaleSize, 0},  {-275 * scaleSize, heightRMAP}}, 2, "TEDAC_SYMBOLOGY", ZoomZonePH.name)
    addSimpleLine( nil, {{275 * scaleSize, 0},   {275 * scaleSize,  heightRMAP}}, 2, "TEDAC_SYMBOLOGY", ZoomZonePH.name)

    addRoundedBox("ZoomSquare",   {0, 0},	"CenterBottom", {width, heightRMAP}, 0, 2, ZoomZonePH.name)
end

function DirectionWiperRMAP(parent)

    local heightWiper = 790 * scaleSize
    local PosY_Zone = 25 * scaleSize

    local WideZonePH = addPlaceholder("WideScanRMAPWiperTedac", {0, -PosY_Zone}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 0}})
    local wiperPHWide = addPlaceholder("WiperWideRMAPWiperTedac", {0 , 0}, WideZonePH.name, {{"DSPLS_FCR_AG_SetDirectionWiperRMAP", 470 * scaleSize}})
    addSimpleLine( nil, {{0, 0}, {0, 0}}, 2, "TEDAC_SYMBOLOGY", wiperPHWide.name,{{"DSPLS_FCR_AG_WiperPosLen", heightWiper}})

    local MediumZonePH = addPlaceholder("MediumScanRMAPWiperTedac", {0, -PosY_Zone}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}})
    local wiperPHMedium = addPlaceholder("WiperMediumRMAPWiperTedac", {0 , 0}, MediumZonePH.name, {{"DSPLS_FCR_AG_SetDirectionWiperRMAP", 750 * scaleSize}})
    addSimpleLine( nil, {{0, 0}, {0, 0}}, 2, "TEDAC_SYMBOLOGY", wiperPHMedium.name,{{"DSPLS_FCR_AG_WiperPosLen", heightWiper}})

    local NarrowZonePH = addPlaceholder("NarrowScanRMAPWiperTedac", {0, -PosY_Zone}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 2}})
    local wiperPHNarrow = addPlaceholder("WiperNarrowRMAPWiperTedac", {0 , 0}, NarrowZonePH.name, {{"DSPLS_FCR_AG_SetDirectionWiperRMAP", 1050 * scaleSize}})
    addSimpleLine( nil, {{0, 0}, {0, 0}}, 2, "TEDAC_SYMBOLOGY", wiperPHNarrow.name,{{"DSPLS_FCR_AG_WiperPosLen", heightWiper}})

    local ZoomZonePH = addPlaceholder("ZoomScanRMAPWiperTedac", {0, -PosY_Zone}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 3}})
    local wiperPHZoom = addPlaceholder("WiperZoomRMAPWiperTedac", {0 , 0}, ZoomZonePH.name, {{"DSPLS_FCR_AG_SetDirectionWiperRMAP", 750 * scaleSize}})
    addSimpleLine( nil, {{0, 0}, {0, 0}}, 2, "TEDAC_SYMBOLOGY", wiperPHZoom.name,{{"DSPLS_FCR_AG_WiperPosLen", heightWiper}})
end

function ShowTargetsSymbolsRMAP(parent)
    for i = 0, 15 do
		addMapTacticalSymbol("FCR_TargetSymbolBlackRMAP"..i,   "font_TEDAC_FCR_Target_symbol_black",   {{"DSPLS_FCR_AG_COMMON_DrawTargetsSymbolRoot", i}, {"DSPLS_FCR_AG_COMMON_DrawTargetsSymbol", i}}, parent, {0, 0}, 30)
		addMapTacticalSymbol("FCR_TargetSymbolRMAP "..i,       "font_TEDAC_FCR_Target_symbol",         {{"DSPLS_FCR_AG_COMMON_DrawTargetsSymbolRoot", i}, {"DSPLS_FCR_AG_COMMON_DrawTargetsSymbol", i}, {"DSPLS_FCR_AG_COMMON_DrawTargetsSymbolColor", i}}, parent, {0, 0}, 30)
    end
end


-------------------------END----------------------------------------


-------------------------ATM----------------------------------------

local  arcRadius = {340 * scaleSize, 255 * scaleSize, 170 * scaleSize, 85 * scaleSize, 22 * scaleSize}

function ZonaCommanATM(parent)
    addArc("ConeArcCommanSecondATM_Tedac",   {0, 0},  arcRadius[1],   0,   360,    50,  4, parent,    nil,    "TEDAC_SYMBOLOGY")
    local lineCoorX = {340 * scaleSize, -340 * scaleSize}
    local lineCoorY = {300 * scaleSize, -300 * scaleSize}
    for i = 1, #lineCoorX do
        addSimpleLine( nil, {{lineCoorX[i], 0},          {lineCoorY[i], 0}}, 2, "TEDAC_SYMBOLOGY", parent)
        addSimpleLine( nil, {{0, lineCoorX[i]},          {0, lineCoorY[i]}}, 2, "TEDAC_SYMBOLOGY", parent)
    end
end

function ZonaWideATM(parent)

    local WideZonePH = addPlaceholder("WideScanATM_Tedac", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 0}})

    for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcWideFirstATM_Tedac"..i,   {0, 0},  arcRadius[i],   0,    360,    50,  10, WideZonePH.name ,   nil,    "TEDAC_SYMBOLOGY")
        else
            addArc("ConeArcWideSecondATM_Tedac"..i,   {0, 0},  arcRadius[i],   0,   360,    50,  4, WideZonePH.name,    nil,    "TEDAC_SYMBOLOGY")
        end
    end

end

function ZonaMediumATM(parent)

    local MediumZonePH = addPlaceholder("MediumScanATM_Tedac", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}, {"DSPLS_FCR_Cone_Root"}})

    addSimpleLine( nil, {{340* scaleSize, 0},          {-340 * scaleSize, 0}},  2, "TEDAC_SYMBOLOGY", MediumZonePH.name)
    addSimpleLine( nil, {{0, 340 * scaleSize},          {0, 0}},                2, "TEDAC_SYMBOLOGY", MediumZonePH.name)

    for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcMediumFirstATM_Tedac"..i,   {0, 0},  arcRadius[i],   0,    180,    50,  10, MediumZonePH.name ,   nil,    "TEDAC_SYMBOLOGY")
        else
            addArc("ConeArcMediumSecondATM_Tedac"..i,   {0, 0},  arcRadius[i],   0,   180,    50,  4, MediumZonePH.name,    nil,    "TEDAC_SYMBOLOGY")
        end
    end
end

function ZonaNarrowATM(parent)

    local NarrowZonePH = addPlaceholder("NarrowScanATM_Tedac", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 2}, {"DSPLS_FCR_Cone_Root"}})

    addSimpleLine( nil, {{340* scaleSize, 0},          {-340 * scaleSize, 0}},  2, "TEDAC_SYMBOLOGY", NarrowZonePH.name)
    addSimpleLine( nil, {{0, 340 * scaleSize},          {0, 0}},                2, "TEDAC_SYMBOLOGY", NarrowZonePH.name)

    addSimpleLine( nil, {{240 * scaleSize, 240 * scaleSize},    {0, 0}},  2, "TEDAC_SYMBOLOGY", NarrowZonePH.name)
    addSimpleLine( nil, {{-240 * scaleSize, 240 * scaleSize},   {0, 0}},  2, "TEDAC_SYMBOLOGY", NarrowZonePH.name)

    for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcNarrowFirstATM_Tedac"..i,   {0, 0},  arcRadius[i],   90 - 45,    90 + 45,    50,  10, NarrowZonePH.name ,   nil,    "TEDAC_SYMBOLOGY")
        else
            addArc("ConeArcNarrowSecondATM_Tedac"..i,   {0, 0},  arcRadius[i],   90 - 45,    90 + 45,    50,  4, NarrowZonePH.name,    nil,    "TEDAC_SYMBOLOGY")
        end
    end
end

function ZonaZoomATM(parent)

    local ZoomZonePH = addPlaceholder("ZoomScanATM_Tedac", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 3}, {"DSPLS_FCR_Cone_Root"}})

    addSimpleLine( nil, {{340* scaleSize, 0},          {-340 * scaleSize, 0}},  2, "TEDAC_SYMBOLOGY", ZoomZonePH.name)
    addSimpleLine( nil, {{0, 340 * scaleSize},          {0, 0}},                2, "TEDAC_SYMBOLOGY", ZoomZonePH.name)

    addSimpleLine( nil, {{240 * scaleSize, 240 * scaleSize},    {0, 0}},  2, "TEDAC_SYMBOLOGY", ZoomZonePH.name)
    addSimpleLine( nil, {{-240 * scaleSize, 240 * scaleSize},   {0, 0}},  2, "TEDAC_SYMBOLOGY", ZoomZonePH.name)

    addSimpleLine( nil, {{130 * scaleSize, 315 * scaleSize},    {0, 0}},  2, "TEDAC_SYMBOLOGY", ZoomZonePH.name)
    addSimpleLine( nil, {{-130 * scaleSize, 315 * scaleSize},   {0, 0}},  2, "TEDAC_SYMBOLOGY", ZoomZonePH.name)

    for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcZoomFirstATM_Tedac"..i,   {0, 0},  arcRadius[i],   90 - 22.5,    90 + 22.5,    50,  10, ZoomZonePH.name ,   nil,    "TEDAC_SYMBOLOGY")
        else
            addArc("ConeArcZoomSecondATM_Tedac"..i,   {0, 0},  arcRadius[i],   90 - 22.5,    90 + 22.5,    50,  4, ZoomZonePH.name,    nil,    "TEDAC_SYMBOLOGY")
        end
    end
end

function DirectionWiperATM(parent)
    local wiperPH = addPlaceholder("WiperATM_Tedac", {0, 0}, parent, {{"DSPLS_FCR_AG_SetDirectionWiper", 2}, {"DSPLS_FCR_Cone_Root"}, {"DSPLS_FCR_ATM_OnSearchTarget"}})
    addSimpleLine( nil, {{0, 340 * scaleSize},   {0, 20 * scaleSize}},  2, "TEDAC_SYMBOLOGY", wiperPH.name)
end

function DrawTailHelicopter(parent)
    local tailPH = addPlaceholder("TailATM_Tedac", {0, 0}, parent, {{"DSPLS_FCR_ATM_DrawTailHelicopter"}})

    addSimpleLine( nil, {{55 * scaleSize, -335 * scaleSize},   {0, -20 * scaleSize}},  4, "TEDAC_SYMBOLOGY", tailPH.name)
    addSimpleLine( nil, {{-55 * scaleSize, -335 * scaleSize},   {0, -20 * scaleSize}},  4, "TEDAC_SYMBOLOGY", tailPH.name)

    addSimpleLine( nil, {{55 * scaleSize, -335 * scaleSize},   {-35 * scaleSize, -220 * scaleSize}},  4, "TEDAC_SYMBOLOGY", tailPH.name)
    addSimpleLine( nil, {{-55 * scaleSize, -335 * scaleSize},   {35 * scaleSize, -220 * scaleSize}},  4, "TEDAC_SYMBOLOGY", tailPH.name)

    addArc("Tail",   {0, 0},  arcRadius[1],   270 - 10,    270 + 10,    50,  10, tailPH.name ,   nil,    "TEDAC_SYMBOLOGY")
end

function ElevationScaleATM(parent)

    Elevation_PH = addPlaceholder ("ElevationScaleATM_Tedac", {-380, -320}, parent)
     
    local scaleStep = 15 * scaleSize
    local currentStep = -1
    for i = 1,8 do
    local ElevationScale = addPlaceholder("elevationScaleATM_Tedac"..i, {0, currentStep}, Elevation_PH.name)
    
    currentStep = scaleStep + currentStep

    if i == 1 or i == 4 or i == 8 then
        addSimpleLine(nil, {{0, 0}, {30 * scaleSize , 0}}, 2,  "TEDAC_SYMBOLOGY",    ElevationScale.name)
    end 
        addSimpleLine(nil, {{0, 0}, {20 * scaleSize , 0}}, 2,  "TEDAC_SYMBOLOGY",    ElevationScale.name)
    end

    arrow = buildBorderedSymbolFCR("arrorATM", {18, 22}, "LeftCenter",  {8, 25}, {30, 2}, Elevation_PH.name, {{"DSPLS_FCR_ATM_SetIndicationElevationScale", currentStep - scaleStep}}, 3, "TEDAC_FCR_INDICATION_WHITE")
end

-------------------------END----------------------------------------

-------------------------TPM---------------------------------------

function wideScanTPM(parent)
    local WideZonePH = addPlaceholder("WideScanTPMTedac", {0, -350}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 0}})

    addArc("1",   {0, 0},  25 * scaleSize,      0,      180,    20,  10, WideZonePH.name ,   nil,     "TEDAC_SYMBOLOGY")
    addArc("2",   {0, 0},  250 * scaleSize,     0,      180,    20,  10, WideZonePH.name ,   nil,     "TEDAC_SYMBOLOGY")
    addArc("3",   {0, 0},  500 * scaleSize,     25.5,   154.5,  20,  10, WideZonePH.name ,   nil,     "TEDAC_SYMBOLOGY")
    addArc("4",   {0, 0},  625 * scaleSize,     44,     136,    20,  10, WideZonePH.name ,   nil,     "TEDAC_SYMBOLOGY")

    addSimpleLine( nil, {{450 * scaleSize,    0 * scaleSize}, {-450 * scaleSize ,   0 * scaleSize}}, 6, "TEDAC_SYMBOLOGY", WideZonePH.name)
    addSimpleLine( nil, {{450 * scaleSize,    435 * scaleSize}, {450 * scaleSize ,   0 * scaleSize}}, 6, "TEDAC_SYMBOLOGY", WideZonePH.name)
    addSimpleLine( nil, {{-450 * scaleSize,    435 * scaleSize}, {-450 * scaleSize ,   0 * scaleSize}}, 6, "TEDAC_SYMBOLOGY", WideZonePH.name)
end

function mediumScanTPM(parent)
    local MediumZonePH = addPlaceholder("MediumScanTPMTedac", {0, -350}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}})

    addSimpleLine( nil, {{443 * scaleSize,      443 * scaleSize}, {15 * scaleSize ,   15 * scaleSize}}, 4, "TEDAC_SYMBOLOGY", MediumZonePH.name)
    addSimpleLine( nil, {{-443 * scaleSize,     443 * scaleSize}, {-15 * scaleSize ,   15 * scaleSize}}, 4, "TEDAC_SYMBOLOGY", MediumZonePH.name)

    arcRadius = {625, 500, 250, 25}

    for i = 1, #arcRadius do
         addArc("ConeArcWideSecondTedac"..i,   {0, 0},  arcRadius[i] * scaleSize,   45,    135,    20,  10, MediumZonePH.name ,   nil,     "TEDAC_SYMBOLOGY")
    end

end

function directionWiperTPM(parent, colorController)

local VideoAreaPH = addPlaceholder("VideoAreaCenterPHWiper", video_area_pos, nil, nil)

    local MaskcWiperPHNone = addPlaceholder("MakscWiperPHNoneTedac", {0, 0}, parent)
    local WiperMaskLeftNone = 	openMaskArea(0, "WiperMaskLeftNoneTedac", 		buildBoxVerts(150 * scaleSize, 550, "CenterCenter"), 	default_box_indices, {-525 * scaleSize, -130 * scaleSize}, 	MaskcWiperPHNone.name)
    local WiperMaskRightNone = 	openMaskArea(0, "WiperMaskRightNoneTedac", 		buildBoxVerts(150 * scaleSize, 550, "CenterCenter"), 	default_box_indices, {525 * scaleSize, -130 * scaleSize},	MaskcWiperPHNone.name)
    
    local wiperMask = addPlaceholder("WiperMask", {0, 0}, parent)
    local wiperPH = addPlaceholder("WiperDownTedac", {0 , -350}, wiperMask.name, {{"DSPLS_FCR_AG_SetDirectionWiper", 2}, {"DSPLS_FCR_Mode", 4}})
    local wiper = addLine("Wiper", {{0 * scaleSize,    625 * scaleSize}, {0 * scaleSize ,   25 * scaleSize}}, 4, wiperPH.name)
    setClipLevel(wiper, 0)

    resetMask(WiperMaskLeftNone, 0, wiperMask.name)
    resetMask(WiperMaskRightNone, 0, wiperMask.name)
end

local function buildArcVerts(pos, radius, angle_start_deg, angle_end_deg, segments_count)
	local countDefault		= 24	-- for the whole circle
	local angleDefault	= 360 / countDefault
	local arc				= angle_end_deg - angle_start_deg
	local count				= segments_count or (math.floor(arc / angleDefault) + 1)	-- segments count
	local v_count			= count + 1	-- verts count

	local verts = {}

	local angle = math.rad(angle_start_deg)
	local delta = math.rad(arc / count)

 	local x0 = pos[1]
	local y0 = pos[2]

	for i = 0,count do
		local x = x0 + radius * math.cos(angle)
		local y = y0 + radius * math.sin(angle)
		verts[#verts + 1] = {x, y}
		angle = angle + delta
	end

	return verts
end

local function buildConeVerts(pos, radius, angle_start_deg, angle_end_deg, segments_count)
    local arcVerts = buildArcVerts(pos, radius, angle_start_deg, angle_end_deg, segments_count)
    local verts = {}
    verts[0] = { 0, 0}

    for i = 1, #arcVerts do
        verts[i] = arcVerts[i]
    end

    return verts
end

local function buildConeIndex(verts)
    local count = #verts
    local index = {}

    for i = 1, count - 1 do
        index[#index + 1] = 0
        index[#index + 1] = i
        index[#index + 1] = i + 1
    end

    return index
end

function AddVideoSignalRender_MPD(namePostfix, parent, controllers)
	
    local TEDAC_ID = "_TEDAC";
	local videoSignal_MPD				= CreateElement "ceTexPoly"
	
	local parent						= nil
	local controllers					= controllers
    local material						= MakeMaterial("RMAP_CAM_AH64"..TEDAC_ID, {255, 255, 255, 255})
	
	setSymbolCommonProperties(videoSignal_MPD, "videoSignal_TEDAC", {0, -380}, nil, controllers, material)
    
    videoSignal_MPD.tex_coords		    =  {{0, 1}, {0, 0}, {1, 0}, {1, 1}}
	videoSignal_MPD.vertices			= buildBoxVerts(900 * scaleSize, 885 * scaleSize, "CenterBottom")
	videoSignal_MPD.indices				= default_box_indices
	videoSignal_MPD.additive_alpha		= true
    videoSignal_MPD.name                = "TEDAC_TPM"..namePostfix

	Add(videoSignal_MPD)

	return videoSignal_MPD
end

function addVideoTPM(parent)
    local countVerts = 20
    local arcRadius =  625 * scaleSize

    local verts =  buildConeVerts({0, 0}, arcRadius,  45, 135, countVerts)
    local index = buildConeIndex(verts)

    local MaskVideo = 	openMaskArea(0, "VideoMaskOpenTedac", verts, index, 	{0, -200}, nil, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}})
   
    AddVideoSignalRender_MPD("Wide", parent,  {{"DSPLS_FCR_AG_ChangeScanCenter", 0}, {"VideoSignal_Color"}})
    local video = AddVideoSignalRender_MPD("Medium", parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}, {"VideoSignal_Color"}})
    setClipLevel(video, 1)

    closeMaskArea(1, "MaskCloseTedac", MaskVideo.vertices, MaskVideo.indices, MaskVideo.init_pos)
end