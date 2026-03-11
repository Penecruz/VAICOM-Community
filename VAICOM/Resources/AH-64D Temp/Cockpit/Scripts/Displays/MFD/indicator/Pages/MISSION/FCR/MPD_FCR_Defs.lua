dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

coneRadius = 650
symbolSize = 50

function CreateMenuFCR()
    local Menu = {}
    Menu = 
    { 
        { pb.T6, "UTIL",		nil },
    }
    createMenu( Menu )

    local OnOffPageMenu = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 0}})

    local Controls = {}
    Controls = 
    { 
        { pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
        { pb.R6, {{"ACQ", nil, nil}, {"FXD", tp_default_border, {{"DSPLS_TSD_ACQ_Button_Caption"}}, {"PHS", "GHS", "SKR", "RFI", "FCR", "FXD", "TADS", "?00", "TRN", "ASE", "?PHS", "?GHS", "?SKR", "?RFI", "?FCR", "?FXD", "?TADS", "?00", "?TRN", "?ASE"}} } },
    }
    createControls( Controls, nil, OnOffPageMenu.name )
end

function DrawBaseConeScanSector(parent)
    BazeConePH = addPlaceholder("BazeCone", {0 , -370}, parent)
    addArc("BazeConeArc", {0, 0}, coneRadius, 45, 135, 20, 4, BazeConePH.name, nil, IND_MPD_MATERIAL_DARK_GREEN)
    draw_line({{459.6, 459.6}, {30, 30}},    IND_MPD_MATERIAL_DARK_GREEN, BazeConePH.name, 4)
    draw_line({{-459.6, 459.6}, {-30, 30}},  IND_MPD_MATERIAL_DARK_GREEN, BazeConePH.name, 4)

    local cutoffLineSize = 16
    local posShort = 300

    local rightShort = addPlaceholder("CutoffLinesRight", {posShort, posShort}, BazeConePH.name)
    draw_line({{0, 0}, {cutoffLineSize, -cutoffLineSize}}, IND_MPD_MATERIAL_DARK_GREEN, rightShort.name, 4)

    local leftShort = addPlaceholder("CutoffLinesLeft", {-posShort,posShort}, BazeConePH.name)
    draw_line({{0, 0}, {-cutoffLineSize,-cutoffLineSize}}, IND_MPD_MATERIAL_DARK_GREEN, leftShort.name, 4)
end

function DrawWideScan(parent)
    local WideZonePH = addPlaceholder("WideScanGTM", {0, -370}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 0}})

    draw_line({{0, coneRadius}, {0, 630}}, IND_MPD_MATERIAL_GREEN, WideZonePH.name, 4)

    local xPosTopShort = {84.8, 168.2, 248.7, 459.6}
    local yPosTopShort = {644.4, 627, 600.5, 459.6}
    local xPosDownShort = {82, 163.5, 240, 30}
    local yPosDownShort = {624, 607, 580, 30}

    for i = 1, #xPosTopShort do
        draw_line({{xPosTopShort[i], yPosTopShort[i]}, {xPosDownShort[i], yPosDownShort[i]}}, IND_MPD_MATERIAL_GREEN, WideZonePH.name, 4)
        draw_line({{-xPosTopShort[i], yPosTopShort[i]}, {-xPosDownShort[i], yPosDownShort[i]}}, IND_MPD_MATERIAL_GREEN, WideZonePH.name, 4)
    end  
     
   local cutoffRadiusLines = {57, 171.8, 286, 401}
   local cutoffLineSize = 12
   for i = 1, #cutoffRadiusLines do
       local i = addPlaceholder("CutoffLinesRight"..i, {cutoffRadiusLines[i], cutoffRadiusLines[i]}, WideZonePH.name)
       draw_line({{0, 0}, {-cutoffLineSize,cutoffLineSize}}, IND_MPD_MATERIAL_GREEN, i.name, 4)
   end
   for j = 1, #cutoffRadiusLines do   
       local j = addPlaceholder("CutoffLinesLeft"..j, {-cutoffRadiusLines[j], cutoffRadiusLines[j]}, WideZonePH.name)
       draw_line({{0, 0}, {cutoffLineSize, cutoffLineSize}}, IND_MPD_MATERIAL_GREEN, j.name, 4)
   end
   
   arcRadius = {coneRadius, 486, 324, 162}
   for i = 1, #arcRadius do

    if i == 1 then
        addArc("ConeArcWideFirst"..i,   {0, 0},  arcRadius[i],   45,    135,    20,  10, WideZonePH.name ,   nil,    IND_MPD_MATERIAL_GREEN)
    else
        addArc("ConeArcWideSecond"..i,   {0, 0},  arcRadius[i],   45,    135,    20,  10, WideZonePH.name ,   nil,    IND_MPD_MATERIAL_DARK_GREEN)
    end
   end
end

function DrawMediumScan(parent)

    local MediumZonePH = addPlaceholder("MediumScanGTM", {0, -370}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}})

    draw_line({{0, coneRadius}, {0, 630}}, IND_MPD_MATERIAL_GREEN, MediumZonePH.name, 4)

    local xPosTopShort = {84.8, 168.2, 248.7}
    local yPosTopShort = {644.4, 627, 600.5}
    local xPosDownShort = {82, 163.5, 15}
    local yPosDownShort = {624, 607, 30}

    for i = 1, #xPosTopShort do
        draw_line({{xPosTopShort[i], yPosTopShort[i]}, {xPosDownShort[i], yPosDownShort[i]}}, IND_MPD_MATERIAL_GREEN, MediumZonePH.name, 4)
        draw_line({{-xPosTopShort[i], yPosTopShort[i]}, {-xPosDownShort[i], yPosDownShort[i]}}, IND_MPD_MATERIAL_GREEN, MediumZonePH.name, 4)
    end   

    local MediumZoneX = {75, 224.5, 374, 524}
    local MediumZoneY = {31, 93, 155, 215}
    local MediumZoneLineSize = 12

    for i = 1, #MediumZoneX do
        local i = addRotPlaceholder("MediumZoneL"..i, {MediumZoneY[i], MediumZoneX[i]}, 20, MediumZonePH.name)
        draw_line({{0, 0}, {-MediumZoneLineSize, MediumZoneLineSize}}, IND_MPD_MATERIAL_GREEN, i.name, 4)
    end

    for i = 1, #MediumZoneX do
        local i = addRotPlaceholder("MediumZoneR"..i, {-MediumZoneY[i], MediumZoneX[i]}, -20, MediumZonePH.name)
        draw_line({{0, 0}, {MediumZoneLineSize, MediumZoneLineSize}}, IND_MPD_MATERIAL_GREEN, i.name, 4)
    end

    arcRadius = {coneRadius, 486, 324, 162}
    for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcMediumFirst"..i,   {0, 0},  arcRadius[i],   67.5,    112.5,    20,  10, MediumZonePH.name ,   nil,    IND_MPD_MATERIAL_GREEN)
        else
            addArc("ConeArcMediumSecond"..i,   {0, 0},  arcRadius[i],   67.5,    112.5,    20,  10, MediumZonePH.name,    nil,    IND_MPD_MATERIAL_DARK_GREEN)
        end

    end
end

function DrawNarrowScan(parent)

    local NarrowZonePH = addPlaceholder("NarrowScanGTM", {0, -370}, parent,{{"DSPLS_FCR_AG_ChangeScanCenter", 2}})
     
    draw_line({{0, coneRadius}, {0, 630}}, IND_MPD_MATERIAL_GREEN, NarrowZonePH.name, 4)

    local xPosTopShort = {84.8, 168.2}
    local yPosTopShort = {644.4, 627}
    local xPosDownShort = {82, 10}
    local yPosDownShort = {624, 30}

    for i = 1, #xPosTopShort do
        draw_line({{xPosTopShort[i], yPosTopShort[i]}, {xPosDownShort[i], yPosDownShort[i]}}, IND_MPD_MATERIAL_GREEN, NarrowZonePH.name, 4)
        draw_line({{-xPosTopShort[i], yPosTopShort[i]}, {-xPosDownShort[i], yPosDownShort[i]}}, IND_MPD_MATERIAL_GREEN, NarrowZonePH.name, 4)
    end   

    local xPosTop = {168.2, 248.7}
    local yPosTop = {627, 600.5}

    local xPosDown = {10, 15}
    local yPosDown = {30, 30}

    for i = 1, #xPosTop do
        if i == 1 then
            draw_line({{xPosTop[i], yPosTop[i]}, {xPosDown[i], yPosDown[i]}}, IND_MPD_MATERIAL_GREEN, NarrowZonePH.name, 4)
            draw_line({{-xPosTop[i], yPosTop[i]}, {-xPosDown[i], yPosDown[i]}}, IND_MPD_MATERIAL_GREEN, NarrowZonePH.name, 4)
        else
            draw_line({{xPosTop[i], yPosTop[i]}, {xPosDown[i], yPosDown[i]}}, IND_MPD_MATERIAL_DARK_GREEN, NarrowZonePH.name, 4)
            draw_line({{-xPosTop[i], yPosTop[i]}, {-xPosDown[i], yPosDown[i]}}, IND_MPD_MATERIAL_DARK_GREEN, NarrowZonePH.name, 4)
        end
    end


    local NarrowZoneX = {78, 234.7, 391, 547.5}
    local NarrowZoneY = {21, 63, 105, 146.7}
    local NarrowLineSize = 12
    for i = 1, #NarrowZoneX do
        local i = addRotPlaceholder("NarrowZoneL"..i, {NarrowZoneY[i], NarrowZoneX[i]}, 30, NarrowZonePH.name)
        draw_line({{0, 0}, {-NarrowLineSize, NarrowLineSize}}, IND_MPD_MATERIAL_GREEN, i.name, 4)
    end

    for i = 1, #NarrowZoneX do
        local i = addRotPlaceholder("NarrowZoneR"..i, {-NarrowZoneY[i], NarrowZoneX[i]}, -30, NarrowZonePH.name)
        draw_line({{0, 0}, {NarrowLineSize, NarrowLineSize}}, IND_MPD_MATERIAL_GREEN, i.name, 4)
    end

    arcRadius = {coneRadius,486,324,162}
    for i = 1, #arcRadius do

        if i == 1 then
            addArc("ConeArcNarrowFirst"..i,   {0, 0},  arcRadius[i],   75,     105,    20,     10, NarrowZonePH.name,   nil,    IND_MPD_MATERIAL_GREEN)
        else
            addArc("ConeArcNarrowSecond"..i,   {0, 0},  arcRadius[i],   75,     105,    20,     10, NarrowZonePH.name,   nil,    IND_MPD_MATERIAL_DARK_GREEN)
        end
    end
end

function DrawZoomScan(parent)
    
    local ZoomZonePH = addPlaceholder("ZoomScanGTM", {0, -370}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 3}})

    local lineZone = addPlaceholder(nil, {0, -370})

    draw_line({{0, coneRadius}, {0, 630}}, IND_MPD_MATERIAL_GREEN, ZoomZonePH.name, 4)

    local xPosTop = {84.8, 168.2, 248.7}
    local yPosTop = {644.4, 627, 600.5}

    local xPosDown = {5, 10, 15}
    local yPosDown = {30, 30, 30}

    for i = 1, #xPosTop do
        if i == 1 then
            draw_line({{xPosTop[i], yPosTop[i]}, {xPosDown[i], yPosDown[i]}}, IND_MPD_MATERIAL_GREEN, ZoomZonePH.name, 4)
            draw_line({{-xPosTop[i], yPosTop[i]}, {-xPosDown[i], yPosDown[i]}}, IND_MPD_MATERIAL_GREEN, ZoomZonePH.name, 4)
        else
            draw_line({{xPosTop[i], yPosTop[i]}, {xPosDown[i], yPosDown[i]}}, IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 4)
            draw_line({{-xPosTop[i], yPosTop[i]}, {-xPosDown[i], yPosDown[i]}}, IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 4)
        end
    end

    local ZoomZoneX = {80, 241, 401, 562}
    local ZoomZoneY = {10, 31, 53, 74}
    local ZoomZoneLineSize = 12

    for i = 1, #ZoomZoneX do
        local i = addRotPlaceholder("ZoomZoneL"..i, {ZoomZoneY[i], ZoomZoneX[i]}, 40, ZoomZonePH.name)
        draw_line({{0, 0}, {-ZoomZoneLineSize, ZoomZoneLineSize}}, IND_MPD_MATERIAL_GREEN, i.name, 4)
    end

    for i = 1, #ZoomZoneX do
        local i = addRotPlaceholder("ZoomZoneR"..i, {-ZoomZoneY[i], ZoomZoneX[i]}, -40, ZoomZonePH.name)
        draw_line({{0, 0}, {ZoomZoneLineSize, ZoomZoneLineSize}}, IND_MPD_MATERIAL_GREEN, i.name, 4)
    end

    arcRadius = {coneRadius, 486, 324, 162}
    for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcZoomFirst"..i,   {0, 0},  arcRadius[i],   82.5,       97.5,       20,     10, ZoomZonePH.name,    nil,    IND_MPD_MATERIAL_GREEN)
        else
            addArc("ConeArcZoomSecond"..i,   {0, 0},  arcRadius[i],   82.5,       97.5,       20,     10, ZoomZonePH.name,    nil,    IND_MPD_MATERIAL_DARK_GREEN)
        end
    end
end

function DrawZoomField(parent)
    local size = 680
    local verts = buildBoxVerts(size, size,  "CenterCenter")
    verts[5] = verts[1]
    draw_line(verts, IND_MPD_MATERIAL_WHITE, parent, 4)
end

function TotalTargetCountStatusWindow(parent)
    local CountTargetsPH = addPlaceholder("CountTargetsPH", {video_area_w_05 - 30, video_area_h_05 - 100 }, parent, {{"DSPLS_FCR_AG_ShowCountTargetsRoot"}})
    addText( "XXXX",  {-15, 0} , tp_36_right_center, {{"DSPLS_FCR_AG_ShowCountTargets"}}, nil, nil, "CountTargetsText", CountTargetsPH.name)
    addRoundedBox("CountTargetsBox", {0, 0}, "RightCenter", {120, 50}, 10, 4, CountTargetsPH.name)
end

function DrawSymbolTargetScanSector(parent)
    for i = 0, 15 do
        drawShotAtSymbol("Shoot At Symbol behind "..i, nil, parent, {{"DSPLS_FCR_AG_COMMON_TargetShootAtSymbolBehind", i}})
    end
    for i = 0, 15 do
        addMapTacticalSymbol("FCR Target Symbol "..i, fontPrefix.."FCR_Target_Symbol", {{"DSPLS_FCR_AG_COMMON_DrawTargetsSymbolRoot", i}, {"DSPLS_FCR_AG_COMMON_DrawTargetsSymbol", i}, {"DSPLS_FCR_AG_COMMON_DrawTargetsSymbolColor", i, 1}}, parent, {0, 0}, symbolSize)
    end
    for i = 0, 15 do
        drawShotAtSymbol("Shoot At Symbol above "..i, nil, parent, {{"DSPLS_FCR_AG_COMMON_TargetShootAtSymbolAbove", i}})
    end
end

function DirectionWiper(parent, colorController)
    local wiperPH = addPlaceholder("WiperDown", {0 , -370}, parent, {{"DSPLS_FCR_AG_SetDirectionWiper", 2}})
    draw_line({{0, 325}, {0, 30}}, IND_MPD_MATERIAL_GREEN, wiperPH.name, 4, nil, {{"DSPLS_FCR_AG_WiperPosLen", coneRadius}, {colorController}})
end

function ElevationScale(parent)

    bazeScale = addPlaceholder (nil, {-510, -460}, parent)
     
    local scaleStep = 15
    local currentStep = 0;
    for i = 1,9 do
    local ElevationScale = addPlaceholder(nil, {0, currentStep} ,bazeScale.name)
    
    currentStep = scaleStep + currentStep

    if i == 1 or i == 4 or i == 9 then
        draw_line({{0, 0}, {30, 0}}, IND_MPD_MATERIAL_GREEN, ElevationScale.name, 2)
    end 
        draw_line({{0, 0}, {20, 0}},IND_MPD_MATERIAL_GREEN, ElevationScale.name, 2)
    end

    arrow = buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_GREEN, {22, 22}, {35, 0}, "LeftCenter", 512, {8, 30}, 2, bazeScale.name, {{"DSPLS_FCR_AG_SetIndicationElevationScale", currentStep - scaleStep}})
    Add(arrow)
end

function DrawPriorityTargets(parent)

    local scaleCoefficient = 0.7
    local multiplierCoefficient = 1.35
	local mask_level 			= 0
	
	local NtsFrameSize 		= {64 * multiplierCoefficient, 68 * multiplierCoefficient}
	local AntsFrameSize 	= {61 * multiplierCoefficient, 76 * multiplierCoefficient}
	
	local NtsControllers 	= {{"DSPLS_FCR_AG_NTS_Show"},   {"DSPLS_FCR_AG_NTS_SetPosition"}}
	local AntsControllers 	= {{"DSPLS_FCR_AG_ANTS_Show"},  {"DSPLS_FCR_AG_ANTS_SetPosition"}}

	local function add_mask(name, parent, controllers, h_clip_relations, level, isvisible)
		local size	= NtsFrameSize[1]
		local s05	= size * 0.48

		local verts = { {-s05, 0}, {0,  s05}, {s05,  0}, {0, -s05} }

		local mask = addMesh(name, verts, default_box_indices, {1, -2}, "triangles", parent, controllers, "MFD_BACKGROUND")
		mask.isvisible			= isvisible		-- true for DBG
		mask.additive_alpha		= false
		mask.change_opacity		= false
		mask.h_clip_relation	= h_clip_relations
		mask.level				= level
	end

    add_mask(nil, parent, NtsControllers, h_clip_relations.INCREASE_IF_LEVEL, DEFAULT_LEVEL + mask_level, false)
    
    local SecondTargetBlack =       buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_BLACK, AntsFrameSize, {0, 0}, "CenterCenter", 512, {160, 20}, scaleCoefficient, parent, AntsControllers)
    local SecondTarget =            buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_YELLOW, AntsFrameSize, {0, 0}, "CenterCenter", 512, {160, 20}, scaleCoefficient, parent, AntsControllers)
    Add(SecondTargetBlack)
    Add(SecondTarget)

	add_mask(nil, parent, NtsControllers, h_clip_relations.REWRITE_LEVEL, DEFAULT_LEVEL + mask_level, false)

    addMapTacticalSymbol(nil,  fontPrefix.."FCR_Target_Symbol",    {{"DSPLS_FCR_AG_ShowSecondTargetPriority"}, {"DSPLS_FCR_AG_DrawSecondTargetColor"}}, parent, {0, 0}, symbolSize)

    local FirstBase 		= addPlaceholder(nil, 			{0, 0}, parent, 		    NtsControllers)
	local FirstSolidBase 	= addPlaceholder(nil, 	{0, 0}, FirstBase.name, 	{{"DSPLS_FCR_AG_NTS_Frame_Show", 1}})
	local FirstDashedBase 	= addPlaceholder(nil, 	{0, 0}, FirstBase.name, 	{{"DSPLS_FCR_AG_NTS_Frame_Show", 2}})

    local FirstTargetSolidBlack =   buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_BLACK, NtsFrameSize, {0, 0}, "CenterCenter", 512, {96, 29}, scaleCoefficient, FirstSolidBase.name )
    local FirstTargetSolid =        buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_YELLOW, NtsFrameSize, {0, 0}, "CenterCenter", 512, {96, 29}, scaleCoefficient, FirstSolidBase.name)
	Add(FirstTargetSolidBlack)
    Add(FirstTargetSolid)  

    local FirstTargetNoSolidBlack = buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_BLACK, NtsFrameSize, {0, 0}, "CenterCenter", 512, {226, 29}, scaleCoefficient, FirstDashedBase.name)
    local FirstTargetNoSolid =      buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_YELLOW, NtsFrameSize, {0, 0}, "CenterCenter", 512, {226, 29},scaleCoefficient, FirstDashedBase.name)
    Add(FirstTargetNoSolidBlack)
    Add(FirstTargetNoSolid)

    addMapTacticalSymbol(nil,    fontPrefix.."FCR_Target_Symbol",    {{"DSPLS_FCR_AG_ShowFirstTargetPriority"},  {"DSPLS_FCR_AG_DrawFirstTargetColor"}},  parent, {0, 0}, symbolSize)
    
end

function addPFZ(parent)
	local PZF = addPlaceholder("ActivePFZ", {0, 0}, parent, {{"DSPLS_FCR_AG_ShowPFZ"}})

	for i=0, 3 do
		draw_PFZ_frame_line_act( "ActivePFZ_"..i,	IND_MPD_TSD_SYMBOLS_WHITE,	PZF.name,	{{"DLPLS_FCR_AG_AcivePFZ_Line_Draw", i}}, {{"DLPLS_FCR_AG_AcivePFZ_Mask_Draw", i}} )
	end
end

function addNFZ(parent)
	for i = 0, 7 do
		local NZF = addPlaceholder("ActiveNFZ"..i, {0, 0}, parent, {{"DSPLS_FCR_AG_ShowNFZ", i}})

		for j=0, 3 do
			draw_NFZ_frame_line_act( "Active_NFZ_"..i.."_FrameLine_"..j,	nil,	NZF.name,	{{"DLPLS_FCR_AG_AciveNFZ_Line_Draw", i, j}}, {{"DLPLS_FCR_AG_AciveNFZ_Mask_Draw", i, j}} )
		end
	end
end


--------------------------------------- Start ---------------------------------------
--------------------------- Missile Constraints Box  Part ---------------------------
local scale_coeff = display_size_pix / 720 -- TEDAC -> MPD Video Page 

local tex_size	= 512
local LOAL_size	= 38 * scale_coeff
local LOBL_size = 158 * scale_coeff

local function buildLine(name, verts, width, parent, controllers, material)
	local width05 = width / 2
	local tex_line_width = 10
	local tex_line_width05 = tex_line_width / 2

	local elem				= CreateElement "ceSimpleLineObject"
	elem.name				= name
	elem.material			= material
	elem.width				= width05
	elem.vertices			= verts
	elem.tex_params			= {{0.0,151.0/tex_size},{1.0,151.0/tex_size}, {1.0/display_size_pix, 2 * (tex_line_width / width)/display_size_pix}}

	if parent ~= nil then
		elem.parent_element = parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end

	setClipLevel(elem)

	return elem
end

function add_LOAL_out_of_constraints_missile_box(name, pos, parent, controllers)
	local size		= LOAL_size
	local s05		= size / 2
	local dash		= 8*scale_coeff
	local d05		= dash / 2
	--local gap		= 7
	local line_width		= 3	-- line width

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	-- back rect
	local back_line = buildLine(name.."_back",
		{
			{-s05, s05},
			{ s05, s05},
			{ s05,-s05},
			{-s05,-s05},
			{-s05, s05},
		},
		line_width + 2, PH.name, nil, IND_MPD_TSD_MATERIAL_BLACK)
	addElem(back_line)
	-- corners
	local c_ul = addSimpleLine(name.."_up_left",	{{-s05, s05 - dash}, {-s05, s05}, {-s05 + dash, s05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
	local c_ur = addSimpleLine(name.."_up_right",	{{ s05, s05 - dash}, { s05, s05}, { s05 - dash, s05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
	local c_dl = addSimpleLine(name.."_down_left",	{{-s05,-s05 + dash}, {-s05,-s05}, {-s05 + dash,-s05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
	local c_dr = addSimpleLine(name.."_down_right",	{{ s05,-s05 + dash}, { s05,-s05}, { s05 - dash,-s05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
	-- centers
	local c_u = addSimpleLine(name.."_up",		{{-d05, s05}, { d05, s05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
	local c_d = addSimpleLine(name.."_down",	{{-d05,-s05}, { d05,-s05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
	local c_l = addSimpleLine(name.."_left",	{{-s05,-d05}, {-s05, d05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
	local c_r = addSimpleLine(name.."_right",	{{ s05,-d05}, { s05, d05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)

	return
	{
		PH,
		back_line,
		c_ul, c_ur, c_dl, c_dr,
		c_u, c_d, c_l, c_r,
	}
end

function add_LOBL_out_of_constraints_missile_box(name, pos, parent, controllers)
	local size			= LOBL_size
	local s05			= size / 2
	local dash			= 11*scale_coeff
	local d05			= dash / 2
	local gap			= 10*scale_coeff
	local period		= dash + gap
	local line_width	= 3	-- line width

	local PH = addPlaceholder(name.."_PH", pos, parent, controllers)

	-- back rect
	local back_line = buildLine(name.."_back",
		{
			{-s05, s05},
			{ s05, s05},
			{ s05,-s05},
			{-s05,-s05},
			{-s05, s05},
		},
		line_width + 2, PH.name, nil, IND_MPD_TSD_MATERIAL_BLACK)
	addElem(back_line)
	-- corners
	local c_ul = addSimpleLine(name.."_corner_up_left",		{{-s05, s05 - dash}, {-s05, s05}, {-s05 + dash, s05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
	local c_ur = addSimpleLine(name.."_corner_up_right",	{{ s05, s05 - dash}, { s05, s05}, { s05 - dash, s05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
	local c_dl = addSimpleLine(name.."_corner_down_left",	{{-s05,-s05 + dash}, {-s05,-s05}, {-s05 + dash,-s05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
	local c_dr = addSimpleLine(name.."_corner_down_right",	{{ s05,-s05 + dash}, { s05,-s05}, { s05 - dash,-s05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
	-- 
	local l0 = -s05 + dash + gap
	local dashes = {}

	for i = 0,5 do
		local lb = l0 + period * i
		local le = lb + dash
		local d_u = addSimpleLine(name.."_up"..i,		{{ lb, s05}, { le, s05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
		local d_d = addSimpleLine(name.."_down"..i,		{{ lb,-s05}, { le,-s05}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
		local d_l = addSimpleLine(name.."_left"..i,		{{-s05, lb}, {-s05, le}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
		local d_r = addSimpleLine(name.."_right"..i,	{{ s05, lb}, { s05, le}},	line_width, IND_MPD_MATERIAL_GREEN, PH.name, nil)
		dashes[#dashes + 1] = d_u
		dashes[#dashes + 1] = d_d
		dashes[#dashes + 1] = d_l
		dashes[#dashes + 1] = d_r
	end

	local elems =
	{
		PH,
		back_line,
		c_ul, c_ur, c_dl, c_dr,
	}

	for i,v in pairs(dashes) do
		elems[#elems + 1] = v
	end

	return elems
end

function add_LOAL_in_constraints_missile_box(name, pos, parent, controllers)
	local rect = addRect(name, LOAL_size, LOAL_size, pos, "CenterCenter", 5, parent, controllers)
	return rect
end

function add_LOBL_in_constraints_missile_box(name, pos, parent, controllers)
	local rect = addRect(name, LOBL_size, LOBL_size, pos, "CenterCenter", 5, parent, controllers)
	return rect
end

function add_missile_constraints_box(parent)
	local PH = addPlaceholder("MissileConstraintsBox_PH", {0,0}, parent, {{"DSPLS_WEAPON_MissileConstraintsBox_Show"},{"DSPLS_WEAPON_MissileConstraintsBox_Pos", video_area_w_05, video_area_h_05}})
	local isLOAL, isLOBL 	= 0, 1
	local isOK, isInvalid 	= 0, 1
	
	add_LOAL_out_of_constraints_missile_box("LOAL_out_MissileBox",	{0,0},	PH.name, {{"DSPLS_WEAPON_MissileConstraintsBox_isLOBL", isLOAL}, {"DSPLS_WEAPON_MissileConstraintsBox_Dashed", isInvalid}})
	add_LOBL_out_of_constraints_missile_box("LOBL_out_MissileBox",	{0,0},	PH.name, {{"DSPLS_WEAPON_MissileConstraintsBox_isLOBL", isLOBL}, {"DSPLS_WEAPON_MissileConstraintsBox_Dashed", isInvalid}})
	add_LOAL_in_constraints_missile_box("LOAL_in_MissileBox",		{0,0}, 	PH.name, {{"DSPLS_WEAPON_MissileConstraintsBox_isLOBL", isLOAL}, {"DSPLS_WEAPON_MissileConstraintsBox_Dashed", isOK}})
	add_LOBL_in_constraints_missile_box("LOBL_in_MissileBox",		{0,0}, 	PH.name, {{"DSPLS_WEAPON_MissileConstraintsBox_isLOBL", isLOBL}, {"DSPLS_WEAPON_MissileConstraintsBox_Dashed", isOK}})
end

function boxHad(parent)

    local HAD_margin_bottom	= 35
    local HAD_y	= -video_area_h_05 + HAD_margin_bottom
    local HAD_PH = addPlaceholder("HAD_PH", {0, HAD_y}, parent, nil)

    addRoundedBox("rightBoxHADFormat",  {285, 50},	"CenterCenter", {310, 90}, 10, 2, HAD_PH.name)
    addRoundedBox("leftBoxHADFormat",   {-285, 50},	"CenterCenter", {310, 90}, 10, 2, HAD_PH.name)
end
--------------------------- Missile Constraints Box  Part ---------------------------
---------------------------------------- End ----------------------------------------

------------------------------------RMAP---------------------------------------------

local heightRMAP = 700
local widthRMAP = 750
local yPosTopShort = {56, 240, 424, 608}
local yPos = {146, 330, 514}

function addVideoRMAP(parent)
	local MFD_ID = readParameter("MFD_NUM");

    local function addRmapVideo(namePostfix, width, zoneControllerIndex)
        local	RMAP_Video			= CreateElement "ceTexPoly"
        RMAP_Video.name				= "RMAP_Video_"..namePostfix
        RMAP_Video.vertices			= buildBoxVerts(width, heightRMAP, "CenterBottom")
        RMAP_Video.indices			= default_box_indices
        RMAP_Video.tex_coords		=  {{0, 1},{0, 0},{1, 0},{1, 1}}
        RMAP_Video.material			= MakeMaterial("RMAP_CAM_AH64"..getMFD_suffix(MFD_ID), {255,255,255,255})
        RMAP_Video.level			= DEFAULT_LEVEL
        RMAP_Video.h_clip_relation	= h_clip_relations.COMPARE
        RMAP_Video.parent_element	= parent
        RMAP_Video.controllers		= {{"DSPLS_FCR_RMAP_Video"}, {"DSPLS_FCR_RMAP_Scan_Zone", zoneControllerIndex}, {"VideoSignal_Color"}}
        RMAP_Video.additive_alpha	= true
        Add(RMAP_Video)
    end

    addRmapVideo("Wide", widthRMAP, 0)
    addRmapVideo("Medium", 600, 1)
    addRmapVideo("Narrow", 550, 2)
    addRmapVideo("Zoom", 200, 3)

end

function DrawBaseSquare(parent)
    addRoundedBox("Base",   {0, 0},	"CenterBottom", {widthRMAP, heightRMAP}, 0, 0.7, parent)
    draw_line({{375, 450},      {390, 450}},            IND_MPD_MATERIAL_DARK_GREEN, parent, 2)
    draw_line({{-375, 450},     {-390, 450}},           IND_MPD_MATERIAL_DARK_GREEN, parent, 2)
    draw_line({{0, heightRMAP - 15},        {0, heightRMAP}},              IND_MPD_MATERIAL_DARK_GREEN, parent, 4)
end

function DrawWideScanSquare(parent)
    local WideZonePH = addPlaceholder("WideScanRMAP", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 0}})

    local xPos = 375
    local xPosDownShort = {300, 275, 100}

    for i = 1, #yPos do
        draw_line({{-xPos, yPos[i]},    {xPos, yPos[i]}},                                       IND_MPD_MATERIAL_DARK_GREEN, WideZonePH.name, 4)
    end

    for i = 1, #yPosTopShort do
       draw_line({{-xPos, yPosTopShort[i]},        {-xPos + 15, yPosTopShort[i]}},             IND_MPD_MATERIAL_DARK_GREEN, WideZonePH.name, 4)
    end

    for i = 1, #xPosDownShort do
        draw_line({{-xPosDownShort[i],  heightRMAP - 15},         {-xPosDownShort[i],   heightRMAP}},              IND_MPD_MATERIAL_DARK_GREEN, WideZonePH.name, 4)
        draw_line({{xPosDownShort[i],   heightRMAP - 15},          {xPosDownShort[i],   heightRMAP}},              IND_MPD_MATERIAL_DARK_GREEN, WideZonePH.name, 4)
    end

    addRoundedBox("WideSquare",   {0, 0},	"CenterBottom", {widthRMAP, heightRMAP}, 0, 2, WideZonePH.name)
end

function DrawMediumScanSquare(parent)
    local width = 600
    local MediumZonePH = addPlaceholder("MediumScanRMAP", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}})
    local xPos = 300
    local xPosDownShort = { 275, 100}

    for i = 1, #yPos do
        draw_line({{-xPos, yPos[i]},                {xPos, yPos[i]}},                           IND_MPD_MATERIAL_DARK_GREEN, MediumZonePH.name, 4)
    end

    for i = 1, #yPosTopShort do
        draw_line({{-xPos, yPosTopShort[i]},        {-xPos + 15, yPosTopShort[i]}},             IND_MPD_MATERIAL_DARK_GREEN, MediumZonePH.name, 4)
    end

    for i = 1, #xPosDownShort do
        draw_line({{-xPosDownShort[i],   heightRMAP - 15},       {-xPosDownShort[i],   heightRMAP}},                IND_MPD_MATERIAL_DARK_GREEN, MediumZonePH.name, 4)
        draw_line({{xPosDownShort[i],    heightRMAP - 15},       {xPosDownShort[i],    heightRMAP}},                IND_MPD_MATERIAL_DARK_GREEN, MediumZonePH.name, 4)
    end

    addRoundedBox("MediumSquare",   {0, 0},	"CenterBottom", {width, heightRMAP}, 0, 2, MediumZonePH.name)
end

function DrawNarrowScanSquare(parent)
    local width = 550
    local NarrowZonePH = addPlaceholder("NarrowScanRMAP", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 2}})
    local xPos = 275

    for i = 1, #yPos do
        draw_line({{-xPos, yPos[i]},    {xPos, yPos[i]}},                                       IND_MPD_MATERIAL_DARK_GREEN, NarrowZonePH.name, 4)
    end

    for i = 1, #yPosTopShort do
        draw_line({{-xPos, yPosTopShort[i]},        {-xPos + 15, yPosTopShort[i]}},             IND_MPD_MATERIAL_DARK_GREEN, NarrowZonePH.name, 4)
    end

    draw_line({{-100,   heightRMAP - 15},           {-100,   heightRMAP}},                IND_MPD_MATERIAL_DARK_GREEN, NarrowZonePH.name, 4)
    draw_line({{100,    heightRMAP - 15},           {100,    heightRMAP}},                IND_MPD_MATERIAL_DARK_GREEN, NarrowZonePH.name, 4)

    draw_line({{-300,   0},                          {-300, heightRMAP}},                              IND_MPD_MATERIAL_DARK_GREEN, NarrowZonePH.name, 2)
    draw_line({{300,    0},                          {300, heightRMAP}},                               IND_MPD_MATERIAL_DARK_GREEN, NarrowZonePH.name, 2)

    addRoundedBox("NarrowSquare",   {0, 0},	"CenterBottom", {width, heightRMAP}, 0, 2, NarrowZonePH.name)
end

function DrawZoomScanSquare(parent)
    local width = 200
    local ZoomZonePH = addPlaceholder("ZoomScanRMAP", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 3}})
    local xPos = 100

    for i = 1, #yPos do
        draw_line({{-xPos, yPos[i]},    {xPos, yPos[i]}},                                       IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 4)
    end

    for i = 1, #yPosTopShort do
        draw_line({{-xPos, yPosTopShort[i]},        {-xPos + 15, yPosTopShort[i]}},             IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 4)
    end

    draw_line({{-300, 0},                         {-300, heightRMAP}},                              IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 2)
    draw_line({{300, 0},                          {300, heightRMAP}},                               IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 2)

    draw_line({{-275, 0},                         {-275, heightRMAP}},                              IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 2)
    draw_line({{275, 0},                          {275, heightRMAP}},                               IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 2)

    addRoundedBox("ZoomSquare",   {0, 0},	"CenterBottom", {width, heightRMAP}, 0, 2, ZoomZonePH.name)
end

function DirectionWiperRMAP(parent, colorController)

    local WideZonePH = addPlaceholder("WideScanRMAPWiper", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 0}}) 
    local wiperPHWide = addPlaceholder("WiperWideRMAPWiper", {0 , 0}, WideZonePH.name, {{"DSPLS_FCR_AG_SetDirectionWiperRMAP", 470}})
    draw_line({{0, 0}, {0, 0}}, IND_MPD_MATERIAL_GREEN, wiperPHWide.name, 4, nil, {{"DSPLS_FCR_AG_WiperPosLen", 744}, {colorController}})
    
    local MediumZonePH = addPlaceholder("MediumScanRMAPWiper", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}})
    local wiperPHMedium = addPlaceholder("WiperMediumRMAPWiper", {0 , 0}, MediumZonePH.name, {{"DSPLS_FCR_AG_SetDirectionWiperRMAP", 750}})
    draw_line({{0, 0}, {0, 0}}, IND_MPD_MATERIAL_GREEN, wiperPHMedium.name, 4, nil, {{"DSPLS_FCR_AG_WiperPosLen", 744}, {colorController}})
    
    local NarrowZonePH = addPlaceholder("NarrowScanRMAPWiper", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 2}})
    local wiperPHNarrow = addPlaceholder("WiperNarrowRMAPWiper", {0 , 0}, NarrowZonePH.name, {{"DSPLS_FCR_AG_SetDirectionWiperRMAP", 1050}})
    draw_line({{0, 0}, {0, 0}}, IND_MPD_MATERIAL_GREEN, wiperPHNarrow.name, 4, nil, {{"DSPLS_FCR_AG_WiperPosLen", 744}, {colorController}})
    
    local ZoomZonePH = addPlaceholder("ZoomScanRMAPWiper", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 3}})
    local wiperPHZoom = addPlaceholder("WiperZoomRMAPWiper", {0 , 0}, ZoomZonePH.name, {{"DSPLS_FCR_AG_SetDirectionWiperRMAP", 750}})
    draw_line({{0, 0}, {0, 0}}, IND_MPD_MATERIAL_GREEN, wiperPHZoom.name, 4, nil, {{"DSPLS_FCR_AG_WiperPosLen", 744}, {colorController}})
end

function DrawSymbolTargetScanSectorRMAP(parent)
    for i = 0, 15 do
        addMapTacticalSymbol("TargetRMAP"..i, fontPrefix.."FCR_Target_Symbol", {{"DSPLS_FCR_AG_COMMON_DrawTargetsSymbolRoot", i}, {"DSPLS_FCR_AG_COMMON_DrawTargetsSymbol", i}, {"DSPLS_FCR_AG_COMMON_DrawTargetsSymbolColor", i, 1}}, parent, {0, 0}, symbolSize)
    end
end
------------------------------------ATM---------------------------------------------

local  arcRadius = {340, 255, 170, 85, 22}

function ZonaCommanATM(parent)
    addArc("ConeArcCommanSecondATM",   {0, 0},  arcRadius[1],   0,   360,    50,  4, parent,    nil,    IND_MPD_MATERIAL_DARK_GREEN)
    local lineCoorX = {340, -340}
    local lineCoorY = {300, -300}
    for i = 1, #lineCoorX do
        draw_line({{lineCoorX[i], 0},          {lineCoorY[i], 0}},                                IND_MPD_MATERIAL_DARK_GREEN, parent, 1)
        draw_line({{0, lineCoorX[i]},          {0, lineCoorY[i]}},                                IND_MPD_MATERIAL_DARK_GREEN, parent, 1)
    end

end

function ZonaWideATM(parent)

    local WideZonePH = addPlaceholder("WideScanATM", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 0}, {"DSPLS_FCR_Cone_Root"}})

    for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcWideFirstATM"..i,   {0, 0},  arcRadius[i],   0,    360,    50,  10, WideZonePH.name ,   nil,    IND_MPD_MATERIAL_GREEN)
        else
            addArc("ConeArcWideSecondATM"..i,   {0, 0},  arcRadius[i],   0,   360,    50,  4, WideZonePH.name,    nil,    IND_MPD_MATERIAL_DARK_GREEN)
        end
    end

end

function ZonaMediumATM(parent)

    local MediumZonePH = addPlaceholder("MediumScanATM", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}, {"DSPLS_FCR_Cone_Root"}})

    draw_line({{340, 0},          {-340, 0}},                           IND_MPD_MATERIAL_DARK_GREEN, MediumZonePH.name, 4)
    draw_line({{0, 340},          {0, 0}},                           IND_MPD_MATERIAL_DARK_GREEN, MediumZonePH.name, 1)

    for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcMediumFirstATM"..i,   {0, 0},  arcRadius[i],   0,    180,    50,  10, MediumZonePH.name ,   nil,    IND_MPD_MATERIAL_GREEN)
        else
            addArc("ConeArcMediumSecondATM"..i,   {0, 0},  arcRadius[i],   0,   180,    50,  4, MediumZonePH.name,    nil,    IND_MPD_MATERIAL_DARK_GREEN)
        end
    end
end

function ZonaNarrowATM(parent)

    local NarrowZonePH = addPlaceholder("NarrowScanATM", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 2}, {"DSPLS_FCR_Cone_Root"}})

    draw_line({{340, 0},          {-340, 0}},                           IND_MPD_MATERIAL_DARK_GREEN, NarrowZonePH.name, 1)
    draw_line({{0, 340},          {0, 0}},                              IND_MPD_MATERIAL_DARK_GREEN, NarrowZonePH.name, 1)

    draw_line({{240, 240},          {0, 0}},                           IND_MPD_MATERIAL_DARK_GREEN, NarrowZonePH.name, 4)
    draw_line({{-240, 240},          {0, 0}},                           IND_MPD_MATERIAL_DARK_GREEN, NarrowZonePH.name, 4)

    for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcNarrowFirstATM"..i,   {0, 0},  arcRadius[i],   90 - 45,    90 + 45,    50,  10, NarrowZonePH.name ,   nil,    IND_MPD_MATERIAL_GREEN)
        else
            addArc("ConeArcNarrowSecondATM"..i,   {0, 0},  arcRadius[i],   90 - 45,    90 + 45,    50,  4, NarrowZonePH.name,    nil,    IND_MPD_MATERIAL_DARK_GREEN)
        end
    end
end

function ZonaZoomATM(parent)

    local ZoomZonePH = addPlaceholder("ZoomScanATM", {0, 0}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 3}, {"DSPLS_FCR_Cone_Root"}})

    draw_line({{340, 0},          {-340, 0}},                           IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 1)
    draw_line({{0, 340},          {0, 0}},                              IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 1)

    draw_line({{240, 240},          {0, 0}},                           IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 1)
    draw_line({{-240, 240},          {0, 0}},                           IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 1)


    draw_line({{130, 315},          {0, 0}},                           IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 4)
    draw_line({{-130, 315},          {0, 0}},                           IND_MPD_MATERIAL_DARK_GREEN, ZoomZonePH.name, 4)

    for i = 1, #arcRadius do
        if i == 1 then
            addArc("ConeArcZoomFirstATM"..i,   {0, 0},  arcRadius[i],   90 - 22.5,    90 + 22.5,    50,  10, ZoomZonePH.name ,   nil,    IND_MPD_MATERIAL_GREEN)
        else
            addArc("ConeArcZoomSecondATM"..i,   {0, 0},  arcRadius[i],   90 - 22.5,    90 + 22.5,    50,  4, ZoomZonePH.name,    nil,    IND_MPD_MATERIAL_DARK_GREEN)
        end
    end
end

function DirectionWiperATM(parent, colorController)
    local wiperPH = addPlaceholder("WiperATM", {0, 0}, parent, {{"DSPLS_FCR_AG_SetDirectionWiper", 2}, {"DSPLS_FCR_Cone_Root"}, {"DSPLS_FCR_ATM_OnSearchTarget"}})
    draw_line({{0, 340}, {0, 20}}, IND_MPD_MATERIAL_GREEN, wiperPH.name, 4, nil,  {{colorController}})
end

function DrawTailHelicopter(parent)
    local tailPH = addPlaceholder("TailATM", {0, 0}, parent, {{"DSPLS_FCR_ATM_DrawTailHelicopter"}})
    draw_line({{55, -335}, {0, -20}}, IND_MPD_MATERIAL_GREEN, tailPH.name, 4, nil)
    draw_line({{-55, -335}, {0, -20}}, IND_MPD_MATERIAL_GREEN, tailPH.name, 4, nil)

    draw_line({{55, -335}, {-35, -220}}, IND_MPD_MATERIAL_GREEN, tailPH.name, 4, nil)
    draw_line({{-55, -335}, {35, -220}}, IND_MPD_MATERIAL_GREEN, tailPH.name, 4, nil)

    addArc("Tail",   {0, 0},  340,   270 - 10,    270 + 10,    50,  10, tailPH.name ,   nil,    IND_MPD_MATERIAL_GREEN)
end

function ElevationScaleATM(parent)

    Elevation_PH = addPlaceholder ("ElevationScaleATM", {-510, -460}, parent)
     
    local scaleStep = 15
    local currentStep = -1;
    for i = 1,8 do
    local ElevationScale = addPlaceholder("elevationScaleATM"..i, {0, currentStep}, Elevation_PH.name)
    
    currentStep = scaleStep + currentStep

    if i == 1 or i == 4 or i == 8 then
        draw_line({{0, 0}, {30, 0}}, IND_MPD_MATERIAL_GREEN, ElevationScale.name, 2)
    end 
        draw_line({{0, 0}, {20, 0}},IND_MPD_MATERIAL_GREEN, ElevationScale.name, 2)
    end

    arrow = buildSymbol(nil, IND_MPD_FCR_SYMBOLOGY_GREEN, {22, 22}, {35, 0}, "LeftCenter", 512, {8, 30}, 2, Elevation_PH.name, {{"DSPLS_FCR_ATM_SetIndicationElevationScale", currentStep - scaleStep}})
    Add(arrow)
end


--------------------TPM-------------------------

function WideScanTPM(parent)
    local WideZonePH = addPlaceholder("WideScanTPM", {0, -350}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 0}})

    addArc("1",   {0, 0},  25,      0,      180,    20,  10, WideZonePH.name ,   nil,    IND_MPD_MATERIAL_DARK_GREEN)
    addArc("2",   {0, 0},  250,     0,      180,    20,  10, WideZonePH.name ,   nil,    IND_MPD_MATERIAL_DARK_GREEN)
    addArc("3",   {0, 0},  500,     25.5,   154.5,  20,  10, WideZonePH.name ,   nil,    IND_MPD_MATERIAL_DARK_GREEN)
    addArc("4",   {0, 0},  625,     44,     136,    20,  10, WideZonePH.name ,   nil,    IND_MPD_MATERIAL_DARK_GREEN)

    draw_line({{450, 0}, {-450, 0}},IND_MPD_MATERIAL_DARK_GREEN, WideZonePH.name, 3)
    draw_line({{450, 435}, {450,0}},IND_MPD_MATERIAL_DARK_GREEN, WideZonePH.name, 3)
    draw_line({{-450, 435}, {-450,0}},IND_MPD_MATERIAL_DARK_GREEN, WideZonePH.name, 3)
end

function MediumScanTPM(parent)
    local MediumZonePH = addPlaceholder("MediumScanTPM", {0, -350}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}})

    draw_line({{443, 443}, {15, 15}},    IND_MPD_MATERIAL_DARK_GREEN, MediumZonePH.name, 4)
    draw_line({{-443, 443}, {-15, 15}},  IND_MPD_MATERIAL_DARK_GREEN, MediumZonePH.name, 4)

    arcRadius = {625, 500, 250, 25}

    for i = 1, #arcRadius do
         addArc("ConeArcWideSecond"..i,   {0, 0},  arcRadius[i],   45,    135,    20,  10, MediumZonePH.name ,   nil,    IND_MPD_MATERIAL_DARK_GREEN)
    end

end

function DirectionWiperTPM(parent, colorController)

    local MaskcWiperPHNone = addPlaceholder("MakscWiperPHNone", {0, 0}, parent)
    local WiperMaskLeftNone = 	openMaskArea(1, "WiperMaskLeftNone", 		buildBoxVerts(150, 550, "CenterCenter"), 	default_box_indices, {-525, -100}, 	MaskcWiperPHNone.name)
    local WiperMaskRightNone = 	openMaskArea(1, "WiperMaskRightNone", 		buildBoxVerts(150, 550, "CenterCenter"), 	default_box_indices, {525, -100}, 	MaskcWiperPHNone.name)

    local wiperPH = addPlaceholder("WiperDown", {0 , -350}, parent, {{"DSPLS_FCR_AG_SetDirectionWiper", 2}})
    local wiper = draw_line({{0, 625}, {0, 25}}, IND_MPD_MATERIAL_GREEN, wiperPH.name, 4, nil, {{colorController}})
    setClipLevel(wiper, 0)

    resetMask(WiperMaskLeftNone, 1, MaskcWiperPHNone.name)
    resetMask(WiperMaskRightNone, 1, MaskcWiperPHNone.name)
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
	
    local MFD_ID = readParameter("MFD_NUM");
	local videoSignal_MPD				= CreateElement "ceTexPoly"
	
	local parent						= nil
	local controllers					= controllers
    local material						= MakeMaterial("RMAP_CAM_AH64"..getMFD_suffix(MFD_ID), {255, 255, 255, 255})
	
	setSymbolCommonProperties(videoSignal_MPD, "videoSignal_MPD", {0, -610}, nil, controllers, material)
    
    videoSignal_MPD.tex_coords		    =  {{0, 1}, {0, 0}, {1, 0}, {1, 1}}
	videoSignal_MPD.vertices			= buildBoxVerts(900, 885, "CenterBottom")
	videoSignal_MPD.indices				= default_box_indices
	videoSignal_MPD.additive_alpha		= true
    videoSignal_MPD.name                = "RMAP_Video_"..namePostfix

	Add(videoSignal_MPD)

	return videoSignal_MPD
end

function addVideoTPM(parent)

    local countVerts = 20
    local arcRadius =  625

    local verts =  buildConeVerts({0, 0}, arcRadius,  45, 135, countVerts)
    local index = buildConeIndex(verts)

    local MaskVideo = 	openMaskArea(0, "VideoMaskOpen", verts, index, 	{0, 260}, parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}})
   
    AddVideoSignalRender_MPD("Wide", parent,  {{"DSPLS_FCR_AG_ChangeScanCenter", 0}, {"VideoSignal_Color"}})
    local video = AddVideoSignalRender_MPD("Medium", parent, {{"DSPLS_FCR_AG_ChangeScanCenter", 1}, {"VideoSignal_Color"}})
    setClipLevel(video, 1)

    closeMaskArea(1, "MaskClose", MaskVideo.vertices, MaskVideo.indices, MaskVideo.init_pos)
end
