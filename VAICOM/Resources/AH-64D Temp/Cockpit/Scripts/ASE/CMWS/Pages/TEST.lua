dofile(LockOn_Options.script_path.."ASE/CMWS/CMWS_Symbology.lua")
local up_row = 10
local dn_row = -25
addPattern( "CMWS_SYMBOLOGY_ORANGE_DIMMED", nil) 

AddText(nil, -80, up_row, {{"CMWS_TEST_Lbl", 0}}, " ")
AddText(nil, -80, dn_row, {{"CMWS_TEST_Lbl", 1}}, " ")

draw_R( {45, -20}, 0, 1, "CMWS_SYMBOLOGY_ORANGE_DIMMED", nil, nil )
draw_D( {45,  20}, 0, 1, "CMWS_SYMBOLOGY_ORANGE_DIMMED", nil, nil )

