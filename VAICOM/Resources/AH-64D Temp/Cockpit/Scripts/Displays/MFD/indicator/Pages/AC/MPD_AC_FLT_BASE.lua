
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_FLT_Symbology.lua")

--draw_pic()

local heading_scale_Y = 345
local heading_scale_left_X = -240
local heading_scale_right_X = 240
local heading_scale_right_height = 70
local att_steep_Y = 83


local show_marks_ru_ph = addPlaceholder("show_marks_ru", {0, 0}, nil, {{"FLT_ShowMarks", 0,	20,		180,	10,		190}})
local show_marks_rd_ph = addPlaceholder("show_marks_rd", {0, 0}, nil, {{"FLT_ShowMarks", 1,	60,		300,	50,		310}})
local show_marks_lu_ph = addPlaceholder("show_marks_lu", {0, 0}, nil, {{"FLT_ShowMarks", 2,	180,	340,	170,	350}})
local show_marks_ld_ph = addPlaceholder("show_marks_ld", {0, 0}, nil, {{"FLT_ShowMarks", 3,	60,		300,	50,		310}})

AddBankAngleSegment_upper({{"FLT_BankAngleSegmentColor", 20, 340}})
AddBankAngleSegment( 40,	80,		90,		show_marks_ru_ph.name, {{"FLT_BankAngleSegmentColor", 20, 340}})
AddBankAngleSegment( 100,	170,	180,	show_marks_rd_ph.name, {{"FLT_BankAngleSegmentColor", 20, 340}})
AddBankAngleSegment( 190,	260,	-1,		show_marks_ld_ph.name, {{"FLT_BankAngleSegmentColor", 20, 340}})
AddBankAngleSegment( 280,	320,	270,	show_marks_lu_ph.name, {{"FLT_BankAngleSegmentColor", 20, 340}})


----------------------------------------------------------------------------------------
local bank_angle_ph = addPlaceholder(nil, {0,0}, nil, {{"FLT_BankAngle"}})
local PitchLadder_05 = 280
local verts = buildEllipseVerts({0, 0}, PitchLadder_05, PitchLadder_05)
local indices =  buildEllipseIndices()
local bank_pitch_ph_mask = openMaskArea(0, "PitchLadder_Mask", verts, indices, {0,0}, bank_angle_ph.name)
----------------------------------------------------------------------------------------
local pitch_ph		= addPlaceholder(nil, {0,0}, bank_angle_ph.name, {{"FLT_Pitch"}})
draw_water_line( {0,0},  IND_MPD_1024_MATERIAL_GREEN, nil, {{"FLT_WaterLine"}} )
draw_water_line_offset( {0,0},  IND_MPD_1024_MATERIAL_GREEN, nil, {{"FLT_WaterLineBias"}} )
draw_bank_angle_mark( {0, 310}, IND_MPD_1024_MATERIAL_GREEN, bank_angle_ph.name)

-- attitude lines
local pitch_ladder = {pitch_ph}
local lbl = { "10", "20", "30", "45", "60" }
local x = 100
local y = 0
local dy = {att_steep_Y, att_steep_Y, att_steep_Y, 1.5*att_steep_Y, 1.5*att_steep_Y }
for i = 1, 5, 1 do
	local ph			= addPlaceholder(nil, {0,y}, pitch_ph.name)
	local asl			= draw_attitude_short_line( { 0, dy[i]/2},	IND_MPD_1024_MATERIAL_BLUE, ph.name )
	local allu			= draw_attitude_line_l_up(	{-x, dy[i]},	IND_MPD_1024_MATERIAL_BLUE, ph.name)
	local tl, tl_name	= addText( lbl [i],  		{-x, dy[i]}, 	tp_24_blue_right_center, nil, nil,nil, nil, ph.name)
	local alru			= draw_attitude_line_r_up(	{ x, dy[i]},	IND_MPD_1024_MATERIAL_BLUE, ph.name)
	local tr, tr_name	= addText( lbl[i],  		{ x, dy[i]}, 	tp_24_blue_left_center, nil, nil,nil, nil, ph.name)
	y = y + dy[i]
	x = x + 10
	
	join(pitch_ladder, ph)
	join(pitch_ladder, asl)
	join(pitch_ladder, allu)
	join(pitch_ladder, tl)
	join(pitch_ladder, alru)
	join(pitch_ladder, tr)
end
local dl = draw_line( {{-300, 0},{300,0}}, IND_MPD_MATERIAL_BROWN, pitch_ph.name, 3 )
join(pitch_ladder, dl)
y = 0
x = 100
for i = 1, 5, 1 do
	local ph			= addPlaceholder(nil, {0,y}, pitch_ph.name)
	local asl			= draw_attitude_short_line(	{ 0,-dy[i]/2},	IND_MPD_1024_MATERIAL_BROWN, ph.name )
	local alld			= draw_attitude_line_l_dn(	{-x,-dy[i]},	IND_MPD_1024_MATERIAL_BROWN, ph.name)
	local tl, tl_name	= addText( lbl[i],			{-x,-dy[i]},	tp_24_brown_right_center, nil, nil,nil, nil, ph.name)
	local alrd			= draw_attitude_line_r_dn(	{ x,-dy[i]},	IND_MPD_1024_MATERIAL_BROWN, ph.name)
	local tr, tr_name	= addText( lbl[i],			{ x,-dy[i]},	tp_24_brown_left_center, nil, nil,nil, nil, ph.name)
	y = y - dy[i]
	x = x + 10
	
	join(pitch_ladder, ph)
	join(pitch_ladder, asl)
	join(pitch_ladder, alld)
	join(pitch_ladder, tl)
	join(pitch_ladder, alrd)
	join(pitch_ladder, tr)
end
--CLIMB&DIV 
	y = 9.0 * att_steep_Y
	local xs = 50

	local climb_ball		= addText("{", {0,y}, tp_36_blue_center, nil, nil, nil, nil, pitch_ph.name)
	local climb_lhs			= draw_line( {{-xs*0.66,y+xs/2},{-xs*0.66,y-xs/2}}, IND_MPD_MATERIAL_BLUE, pitch_ph.name, 4 )
	local climb_rhs			= draw_line( {{ xs*0.66,y+xs/2},{ xs*0.66,y-xs/2}}, IND_MPD_MATERIAL_BLUE, pitch_ph.name, 4 )
	
	local climb_PH			= addRotPlaceholder(nil, {0, y}, 180.0,  pitch_ph.name)
	local climb_up, c_up	= addText("CLIMB", {0,y+xs*0.5}, tp_24_blue_center, nil, nil, nil, nil, pitch_ph.name)
	local climb_dn, c_dn	= addText("CLIMB", {0, xs*0.5}, tp_24_blue_center, nil, nil, nil, nil, climb_PH.name)
	
	join(pitch_ladder, climb_ball)
	join(pitch_ladder, climb_lhs)
	join(pitch_ladder, climb_rhs)

	join(pitch_ladder, div_PH)
	join(pitch_ladder, climb_up)
	join(pitch_ladder, climb_dn)

	local div_ball 			= addText("{", {0,-y}, tp_36_brown_center, nil, nil, nil, nil, pitch_ph.name)
	local div_lhs			= draw_line( {{-xs*0.66,-y+xs/2},{-xs*0.66,-y-xs/2}}, IND_MPD_MATERIAL_BROWN, pitch_ph.name, 4 )
	local div_rhs			= draw_line( {{ xs*0.66,-y+xs/2},{ xs*0.66,-y-xs/2}}, IND_MPD_MATERIAL_BROWN, pitch_ph.name, 4 )
	
	local div_PH			= addRotPlaceholder(nil, {0, -y}, 180.0,  pitch_ph.name)
	local div_up, d_up		= addText("DIVE", {0,-y+xs*0.5}, tp_24_brown_center, nil, nil, nil, nil, pitch_ph.name)
	local div_dn, d_dn		= addText("DIVE", {0, xs*0.5},   tp_24_brown_center, nil, nil, nil, nil, div_PH.name)
	
	join(pitch_ladder, div_ball)
	join(pitch_ladder, div_lhs)
	join(pitch_ladder, div_rhs)

	join(pitch_ladder, div_PH)
	join(pitch_ladder, div_up)
	join(pitch_ladder, div_dn)
	
setElemsClipLevel(pitch_ladder, 1)
resetMask(bank_pitch_ph_mask, 0, bank_angle_ph.name)
------------------------------------------------------------------------
draw_heading_scale( {0,heading_scale_Y}, IND_MPD_1024_MATERIAL_GREEN, {{"NAV_Heading_Valid"},{"FLT_Heading_Scale"}} )
addText( "360",  { 0, heading_scale_Y + 30 } , tp_36_center_bottom, {{"NAV_Heading_Valid"},{"FLT_HeadingScaleLabel"}} )

addText( "52%", {-480, heading_scale_Y}, tp_36, {{"FLT_Torque"}} )
addRoundedBox("torqueBorder_w", {-485, heading_scale_Y-5},  "LeftBottom" , {100, 45}, 1, 4, nil, nil, IND_MPD_MATERIAL_GREEN, {{"FLT_TorqueBorder", 1}})
addRoundedBox("torqueBorder", {-485, heading_scale_Y-5},  "LeftBottom" , {75, 45}, 1, 4, nil, nil, IND_MPD_MATERIAL_GREEN, {{"FLT_TorqueBorder", 0}})
addText( "860C", {-480, heading_scale_Y - tp_36.height*1.4}, tp_36, {{"FLT_Tgt"}} )

local radar_alt = addText( "180", {420, 0} , tp_36_right_center, {{"FLT_RadarAltLabels"}} )

draw_Climb_vertical_scale()
draw_radar_vertical_scale( {{"FLT_RadarVertScale"}}  )

addMaskArea(2, h_clip_relations.REWRITE_LEVEL, nil, buildBoxVerts( 14, 480 , "CenterTop"), default_box_indices, {475, -240 }, nil, {{"FLT_RadarAltMask"}}, "TRANSPARENT" )
draw_vertical_indicator( {475,240},  IND_MPD_1024_MATERIAL_GREEN, nil, {{"FLT_RadarAlt"}}, h_clip_relations.COMPARE, 2 )



addText( "HI", {-70, 50} , 					tp_36_yellow, 	nil, nil, nil, nil, radar_alt.name )
addText( "LO", {-70, -50-tp_36.height} , 	tp_36_red, 		nil, nil, nil, nil, radar_alt.name )

local airspeed = addText( "   ",  {-405, -tp_36.height/6}, tp_36_center_center, {{"FLT_Airspeed"}} )

addRoundedBox("airspeedBorder", {-405, -tp_36.height/6}, "CenterCenter", {75, 50}, 1, 4, nil, nil, IND_MPD_MATERIAL_GREEN, {{"FLT_AirspeedBorder"}})
addRoundedBox("attitudeHold",	{-405, -tp_36.height/6}, "CenterCenter", {89, 64},10, 4, nil, nil, IND_MPD_MATERIAL_GREEN, {{"FLT_AttitudeHold"}})

local bias_pos = {pb_props[pb.L5].pos[1], (pb_props[pb.L5].pos[2] + pb_props[pb.L6].pos[2] - tp_def_left_center.height)/2}
addText( "BIAS", bias_pos, tp_def_left_center, {{"FLT_BIAS"}} )
-----------------------------------------------------------
draw_flight_path_vector( {0,0},  IND_MPD_1024_MATERIAL_GREEN, nil, {{"FLT_FlightPathVector", video_area_w_05}} )
draw_FLY_To_CUE( {0, 0},  IND_MPD_1024_MATERIAL_GREEN, nil, {{"FLT_FlyToCue", video_area_w_05}} )
draw_turn_rate({{"FLT_TurnRateSquare"}})
draw_skid_slip({{"FLT_SkidSlipBall"}})

draw_Climb_indicator( {450, 0},  IND_MPD_1024_MATERIAL_GREEN, nil, {{"FLT_ClimbIndicator"}} )
draw_Alternate_Bearing_indicator( {0, heading_scale_Y},  IND_MPD_1024_MATERIAL_GREEN, nil, {{"NAV_Heading_Valid"},{"FLT_AltBearingIndicator"}}, h_clip_relations.COMPARE, 2)
draw_FCR_Centerline_Bearing_indicator({0, heading_scale_Y},  IND_MPD_1024_MATERIAL_GREEN, nil, {{"FLT_FCR_CenterlineIndicator", 215}}, h_clip_relations.COMPARE, 2)
draw_ADF_Bearing_indicator( {0, heading_scale_Y},  IND_MPD_1024_MATERIAL_GREEN, nil, {{"NAV_Heading_Valid"},{"FLT_AdfBearingIndicator"}}, h_clip_relations.COMPARE, 2 )
draw_Bobup_Heading_indicator({0, heading_scale_Y},  IND_MPD_1024_MATERIAL_GREEN, nil, {{"FLT_BobupHeadingIndicator"}}, h_clip_relations.COMPARE, 2  )

AddNextWaypointStatusWindow( {-320, -300},{{"NAV_Heading_Valid"}})
addAltitudeHold("AltitudeHold", {455, 0}, nil, {{"FLT_AltitudeHold"}})
