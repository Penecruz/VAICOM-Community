dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_PB_Defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Tools.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Materials.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_StringSymbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_MenuSymbology.lua")

local offsetRCW = 0 -- RCW = RoundCornersWindow
local scale = 0.7

local OnOffPageTGT = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 3}})

local tgtPH = addPlaceholder("tgtPH", {480 * scale, 420 * scale}, OnOffPageTGT.name, nil)
AddRoundCornersWindow(  "tgtBaze", {offsetRCW * scale,-15 * scale}, 80, 40, nil, nil, nil, tgtPH.name, nil, "CenterTop")
addText( nil,   { 0, 0} , tp_28_center_top, {{"DSPLS_FCR_AG_SetNextAvailableTGTpoint"}} , nil , nil, nil, tgtPH.name)


local targetListPH = addPlaceholder("TargetListPH", {480 * scale, 320 * scale}, OnOffPageTGT.name, nil)
local bigCRWHeight = 420
AddRoundCornersWindow(  "tgt", {offsetRCW , -bigCRWHeight * 0.5 }, 80, 470, nil, nil, nil, targetListPH.name, nil, "CenterTop")

texateOffset = 10

for i = 0,15 do  
   addText( nil, { 0 , texateOffset } , tp_28_center_top, {{"DSPLS_FCR_AG_ShowListTargetTGT",i}} ,nil , nil,"Targets"..i,targetListPH.name)
   texateOffset = texateOffset - 28;
end
