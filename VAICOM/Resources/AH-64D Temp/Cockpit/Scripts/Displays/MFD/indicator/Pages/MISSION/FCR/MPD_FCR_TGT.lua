dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local OnOffPageTGT = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 3}})

local controls = {}
controls=
{
    { pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
    { pb.L4, "T\nG\nT",		tp_default_border,  nil	},
    { pb.L5,  "ALL",        tp_28,              {{"DSPLS_FCR_AG_ShowAllButtonTGT"}} },
}

local pos_y_L4 = 25

pb_props[pb.L4].pos[2] = pb_props[pb.L4].pos[2] + pos_y_L4

createControls(controls, nil, OnOffPageTGT.name)

pb_props[pb.L4].pos[2] = pb_props[pb.L4].pos[2] - pos_y_L4

local offsetRCW = 37 -- RCW = RoundCornersWindow

local GTM_PH = addPlaceholder("GTM_PH", nil, OnOffPageTGT.name, {{"DSPLS_FCR_GTM_Root"}})

local tgtPH = addPlaceholder("tgtPH", {420, 320}, GTM_PH.name, nil)
AddRoundCornersWindow(  "tgtBaze", {offsetRCW, -15}, 100, 50, nil, nil, nil, tgtPH.name, nil, "CenterTop")
addText( nil,   { 10, 0}, tp_28, {{"DSPLS_FCR_AG_SetNextAvailableTGTpoint"}}, nil, nil, nil, tgtPH.name)

local targetListPH = addPlaceholder("TargetListPH", {420, 250}, GTM_PH.name, nil)
local bigCRWHeight = 620
AddRoundCornersWindow(  "tgt", {offsetRCW, -bigCRWHeight * 0.5 + 10}, 100, 620, nil, nil, nil, targetListPH.name, nil, "CenterTop")

texateOffset = 0

for i = 0,15 do  
	addText( nil, {10 , texateOffset}, tp_28, {{"DSPLS_FCR_AG_ShowListTargetTGT", i}}, nil, nil, "Targets"..i, targetListPH.name)
	texateOffset = texateOffset - 38;
end