dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_ENG_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_AC_ENG_INCLUDE.lua")

--draw_pic()
addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "eng_torque_1_mask",		buildBoxVerts(80, 700, "CenterTop"), default_box_indices, {barCenterX[1], barBottomLineY}, nil, {{"EngMaskTorque", 1}}, "TRANSPARENT" )
local Torque1_ph = addPlaceholder("Torque1_ph", {barCenterX[1], barBottomLineY }, nil, {{"EngTorque", 1}})
draw_Torque_bar( 1, {0,0},  IND_MPD_1024_MATERIAL_GREEN,  Torque1_ph.name, nil, h_clip_relations.COMPARE, 2 )
draw_Torque_bar( 2, {0,0},  IND_MPD_1024_MATERIAL_GREEN,  Torque1_ph.name, nil, h_clip_relations.COMPARE, 2 )
draw_Torque_bar( 3, {0,0},  IND_MPD_1024_MATERIAL_GREEN,  Torque1_ph.name, nil, h_clip_relations.COMPARE, 2 )
draw_Torque_bar( 4, {0,0},  IND_MPD_1024_MATERIAL_GREEN,  Torque1_ph.name, nil,h_clip_relations.COMPARE, 2 )

addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "eng_torque_2_mask",		buildBoxVerts(80, 700, "CenterTop"), default_box_indices, {barCenterX[2], barBottomLineY}, nil, {{"EngMaskTorque", 2}}, "TRANSPARENT" )
local Torque2_ph = addPlaceholder("Torque2_ph", {barCenterX[2], barBottomLineY }, nil, {{"EngTorque", 2}} )
draw_Torque_bar( 1, {0,0},  IND_MPD_1024_MATERIAL_GREEN,  Torque2_ph.name, nil, h_clip_relations.COMPARE, 2  )
draw_Torque_bar( 2, {0,0},  IND_MPD_1024_MATERIAL_GREEN,  Torque2_ph.name, nil, h_clip_relations.COMPARE, 2 )
draw_Torque_bar( 3, {0,0},  IND_MPD_1024_MATERIAL_GREEN,  Torque2_ph.name, nil,h_clip_relations.COMPARE, 2 )
draw_Torque_bar( 4, {0,0},  IND_MPD_1024_MATERIAL_GREEN,  Torque2_ph.name, nil,h_clip_relations.COMPARE, 2 )

addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "eng_TGT_1_mask",	buildBoxVerts(80, 500, "CenterTop"), default_box_indices, {barCenterX[3], barBottomLineY}, nil, {{"EngMaskTGT", 1}}, "TRANSPARENT" )
draw_TGT_bar( {barCenterX[3], barBottomLineY },  IND_MPD_1024_MATERIAL_GREEN, {{"EngTGT", 1}}, h_clip_relations.COMPARE, 2  )

addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "eng_TGT_2_mask",	buildBoxVerts(80, 500, "CenterTop"), default_box_indices, {barCenterX[4], barBottomLineY}, nil, {{"EngMaskTGT", 2}}, "TRANSPARENT" )
draw_TGT_bar( {barCenterX[4], barBottomLineY },  IND_MPD_1024_MATERIAL_GREEN, {{"EngTGT", 2}}, h_clip_relations.COMPARE, 2   )

addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "eng_NP_1_mask",	buildBoxVerts(80, 100, "CenterTop"), default_box_indices, {barCenterX[5], barBottomLineY}, nil, {{"EngMaskNP", 1}}, "TRANSPARENT" )
addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "eng_NP_1_mask_up",	np_mask_verts_up, np_mask_ind_up, {barCenterX[5], barBottomLineY}, nil, {{"EngMaskNP_up", 1}}, "TRANSPARENT" )
draw_NP_bar( {barCenterX[5], barBottomLineY },  IND_MPD_1024_MATERIAL_GREEN, {{"EngNP", 1}}, h_clip_relations.COMPARE, 2 )

addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "rotor_mask_dn",	rotor_mask_verts_dn, rotor_mask_ind_dn, {barCenterX[6], barBottomLineY}, nil, {{"EngMaskRotor_dn"}}, "TRANSPARENT" )
addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "rotor_mask_mid",	rotor_mask_verts_mid, rotor_mask_ind_mid, {barCenterX[6], barBottomLineY}, nil, {{"EngMaskRotor_mid"}}, "TRANSPARENT" )
addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "rotor_mask_up",	rotor_mask_verts_up, rotor_mask_ind_up, {barCenterX[6], barBottomLineY}, nil, {{"EngMaskRotor_up"}}, "TRANSPARENT" )
draw_rotor_bar( {barCenterX[6], barBottomLineY},  IND_MPD_1024_MATERIAL_GREEN, {{"EngRotor"}}, h_clip_relations.COMPARE, 2  )


addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "eng_NP_2_mask",	buildBoxVerts(80, 100, "CenterTop"), default_box_indices, {barCenterX[7], barBottomLineY}, nil, {{"EngMaskNP", 2}}, "TRANSPARENT" )
addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "eng_NP_2_mask_up",	np_mask_verts_up, np_mask_ind_up, {barCenterX[7], barBottomLineY}, nil, {{"EngMaskNP_up", 2}}, "TRANSPARENT" )
draw_NP_bar( {barCenterX[7], barBottomLineY },  IND_MPD_1024_MATERIAL_GREEN, {{"EngNP", 2}}, h_clip_relations.COMPARE, 2 )

addMaskArea(1, h_clip_relations.REWRITE_LEVEL, "page_low_mask",		buildBoxVerts(1023, 700, "CenterTop"), default_box_indices, {0, lowTextY }, nil, nil, "TRANSPARENT" )
addText( "TORQUE %", { (barCenterX[1]+barCenterX[2])/2, upperTextY}, tp_28_center_bottom  )
addText( "TGT  ^C",  { (barCenterX[3]+barCenterX[4])/2, upperTextY}, tp_28_center_bottom  )

addText( "   %", { (barCenterX[5]), upperTextY}, tp_28_center_bottom )
addText( "N",  { (barCenterX[5] - text_properties_default.width), upperTextY}, tp_28_center_bottom )
addText( "P",  { barCenterX[5], upperTextY-6}, tp_28_center_bottom )

addText( "   %", { (barCenterX[6]), upperTextY}, tp_28_center_bottom )
addText( "N",  { (barCenterX[6] - text_properties_default.width), upperTextY}, tp_28_center_bottom )
addText( "R",  { barCenterX[6], upperTextY-6}, tp_28_center_bottom )

addText( "   %", { (barCenterX[7]), upperTextY}, tp_28_center_bottom )
addText( "N",  { (barCenterX[7] - text_properties_default.width), upperTextY}, tp_28_center_bottom )
addText( "P",  { barCenterX[7], upperTextY-6}, tp_28_center_bottom )

addText( "  ",  { barCenterX[1], paramBarY}, tp_36_center_bottom, {{"EngParamTorque", 1}} )-- controller will set pos
addText( "  ",  { barCenterX[2], paramBarY}, tp_36_center_bottom, {{"EngParamTorque", 2}} )-- controller will set pos

addText( "   ", { barCenterX[3], paramBarY}, tp_36_center_bottom, {{"EngParamTGT", 1}} )
addText( "   ", { barCenterX[4], paramBarY}, tp_36_center_bottom, {{"EngParamTGT", 2}} )
addText( "   ", { barCenterX[6], paramBarY}, tp_36_center_bottom, {{"EngParamRotor"}} )

addText( "1", { barCenterX[1], lowTextY}, tp_28_center_bottom, {{"EngTorqueTimer", 1}} )
addText( "2", { barCenterX[2], lowTextY}, tp_28_center_bottom, {{"EngTorqueTimer", 2}} )
addText( "", { (barCenterX[1]+ barCenterX[2])/2, lowTextY}, tp_28_center_bottom, {{"EngTorqueTimer", 3}} )

addText( "1", { barCenterX[3], lowTextY}, tp_28_center_bottom, {{"EngTGTTimer", 1}} )
addText( "2", { barCenterX[4], lowTextY}, tp_28_center_bottom, {{"EngTGTTimer", 2}} )
addText( "", { (barCenterX[3]+ barCenterX[4])/2, lowTextY}, tp_28_center_bottom, {{"EngTGTTimer", 3}} )

addText( "1", { barCenterX[5], lowTextY}, tp_28_center_bottom, {{"EngNpTimer", 1}} )
addText( "2", { barCenterX[7], lowTextY}, tp_28_center_bottom, {{"EngNpTimer", 2}} )

addText( "1", { barCenterX[8], lowTextY}, tp_28_center_bottom, {{"EngNgTimer", 1}} ) 
addText( "2", { barCenterX[9], lowTextY}, tp_28_center_bottom, {{"EngNgTimer", 2}} )
addText( "", { (barCenterX[8]+barCenterX[9])/2, lowTextY}, tp_28_center_bottom, {{"EngNgTimer", 3}} )

draw_check_mark( {(barCenterX[1]+barCenterX[2])/2,paramBarY + tp_36.height*1.2}, IND_MPD_1024_MATERIAL_RED, {{"EngTorqueCheckMark"}} ) -- controller will set pos
draw_check_mark( {(barCenterX[3]+barCenterX[4])/2,415+barBottomLineY}, IND_MPD_1024_MATERIAL_RED )
draw_check_mark_with_dots(  {barCenterX[6], 388+barBottomLineY}, IND_MPD_1024_MATERIAL_RED )

draw_timer_mark( {(barCenterX[1]+barCenterX[2])/2,paramBarY + tp_36.height*1.2},  IND_MPD_1024_MATERIAL_YELLOW, {{"EngTorqueTimerLine"}} )

draw_timer_mark( {(barCenterX[3]+barCenterX[4])/2,barBottomLineY},  IND_MPD_1024_MATERIAL_YELLOW, {{"EngTGTTimerLine", 1}} )
draw_timer_mark( {(barCenterX[3]+barCenterX[4])/2,barBottomLineY},  IND_MPD_1024_MATERIAL_YELLOW, {{"EngTGTTimerLine", 2}} )
draw_timer_mark( {(barCenterX[3]+barCenterX[4])/2,barBottomLineY},  IND_MPD_1024_MATERIAL_YELLOW, {{"EngTGTTimerLine", 3}} )

local AIRCRAFT_ENG = 10
local AIRCRAFT_FLT = 11
local AIRCRAFT_FUEL = 12
local AIRCRAFT_PERF = 13
local AIRCRAFT_UTIL = 14
local Menu = {}
Menu = 
{ 
	{ pb.T1, "ENG",		tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_ENG }}},
	{ pb.T2, "FLT",		tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_FLT }}},
	{ pb.T3, "FUEL",	tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_FUEL }}},
	{ pb.T4, "PERF",	tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_PERF }}},
	{ pb.T6, "UTIL",	tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_UTIL }}},
	{ pb.B2, "SYS",		nil },
	{ pb.B3, "ETF",		nil },
	{ pb.B6, "WCA",		nil }
}
createMenu( Menu )

Controls = 
{
	{ pb.B1, "ENG",	tp_default_border, {{"MFD_AC_OriginatorFmt"}}, {"ENG", "FLT", "FUEL", "PERF","UTIL"} }
}
createControls( Controls )








