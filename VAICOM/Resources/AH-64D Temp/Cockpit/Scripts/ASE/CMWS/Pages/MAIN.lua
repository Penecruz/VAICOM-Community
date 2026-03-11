dofile(LockOn_Options.script_path.."ASE/CMWS/CMWS_Symbology.lua")
local up_row = 10
local dn_row = -25
addPattern( "CMWS_SYMBOLOGY_ORANGE",		"CMWS_ThreatSector",		"CMWS_BigArrow",		nil) 
addPattern( "CMWS_SYMBOLOGY_ORANGE_DIMMED", "CMWS_ThreatSectorDimmed", "CMWS_BigArrowDimmed",	nil) 
AddText(nil, -85, up_row, nil, "F")
AddText(nil, -85, dn_row, nil, "C")

AddText(nil, -64, up_row, {{"CMWS_FlaresCount"}}, "00")
AddText(nil, -64, dn_row, {{"CMWS_ChaffsCount"}}, "00")

draw_R( {45, -20}, 0, 1, "CMWS_SYMBOLOGY_ORANGE", nil, {{"CMWS_R"}} )
draw_D( {45,  20}, 0, 1, "CMWS_SYMBOLOGY_ORANGE", nil, {{"CMWS_D"}} )

draw_R( {45, -20}, 0, 1, "CMWS_SYMBOLOGY_ORANGE_DIMMED", nil, {{"CMWS_R_Dimmed"}} )
draw_D( {45,  20}, 0, 1, "CMWS_SYMBOLOGY_ORANGE_DIMMED", nil, {{"CMWS_D_Dimmed"}} )