dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

-- -----------------------------------------------------------------------------------------------
function draw_heading_scale( pos,  material, controllers)

	local heading_scale_left_X = -240
	local heading_scale_right_X = 240
	local heading_scale_width = 480
	local heading_scale_height = 70
	local heading_value_width = 70

	local long_mark_l = 0
	local long_mark_h = 20
	local short_mark_l = 5
	local short_mark_h = 15
	local pointer_h = 30
	local steep = 24
	--show_masks = true
	--local formats = { "21", "S", "15", "12", "E", "6","3", "N", "33", "30", "W", "24"}
	local formats = { "21", "24", "W", "30", "33", "N", "3", "6", "E", "12", "15", "S"}
	
	addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "left_heading_mask",		buildBoxVerts((heading_scale_width-heading_value_width)/2, heading_scale_height - long_mark_h, "RightBottom"), default_box_indices,  {pos[1]-heading_value_width/2,pos[2] + long_mark_h}, nil, nil, "TRANSPARENT" )
	addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "right_heading_mask",	buildBoxVerts((heading_scale_width-heading_value_width)/2, heading_scale_height- long_mark_h, "LeftBottom"), default_box_indices,   {pos[1]+heading_value_width/2,pos[2] + long_mark_h}, nil, nil, "TRANSPARENT" )
	addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "low_heading_mask",		buildBoxVerts( heading_scale_width,		long_mark_h, "CenterTop"), default_box_indices,   {pos[1],pos[2] + long_mark_h }, nil, nil, "TRANSPARENT" )	
	addMaskArea(2, h_clip_relations.REWRITE_LEVEL, "low_heading_pointers",	buildBoxVerts( heading_scale_width*1.1, pointer_h,	"CenterTop"), default_box_indices,   {pos[1],pos[2] }, nil, nil, "TRANSPARENT" )
	local ph = addPlaceholder("heading_scale_ph", pos, nil, controllers)

	local x_long = - 5*3*steep
	local x_short = steep
	for i = 0, 10, 1 do 
		draw_line( { {x_long, long_mark_l}, {x_long, long_mark_h} }, IND_MPD_MATERIAL_GREEN, ph.name, 3, nil, nil, h_clip_relations.COMPARE, 2)
		addText( "", {x_long , long_mark_h+10 }, tp_28_center_bottom, nil, formats, nil, nil, ph.name, h_clip_relations.COMPARE, 2, false )
		x_long = x_long + 3*steep
		draw_line( { { x_short, short_mark_l}, {x_short, short_mark_h} }, IND_MPD_MATERIAL_GREEN, ph.name, 3, nil, nil, h_clip_relations.COMPARE, 2 )
		x_short = x_short + steep 
		draw_line( { { x_short, short_mark_l}, {x_short, short_mark_h} }, IND_MPD_MATERIAL_GREEN, ph.name, 3, nil, nil,  h_clip_relations.COMPARE, 2 )
		x_short = x_long + steep 
	end
end
-- -----------------------------------------------------------------------------------------------
function draw_turn_rate_caption( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(50, 25,  "CenterBottom")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {700 * texture_scale_1024, 900 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
	elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- ----- -----------------------------------------------------------------------------------------------
function draw_turn_rate_square( pos,  material, parent, controllers )
local scale = 1

local elem  		= CreateElement "ceTexPoly"
elem.vertices		= buildBoxVerts(35, 35,  "CenterBottom")

elem.indices		= default_box_indices
elem.material		= material
elem.init_pos		= pos
elem.tex_params		= {220 * texture_scale_1024, 910 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
if parent ~= nil then
elem.parent_element 	= parent
end
if controllers ~= nil then
	elem.controllers	= controllers
end
Add(elem)
end
--------------------------------------------------------------------------------------------
function draw_turn_rate(controllers)
	local low_level = -450;
	local steep_X = 33;
	local line_height = 33;
	local high_level = low_level + line_height;
	
	local X = steep_X/2
	
	for i = 1, 4  do 
		draw_line( { { X, low_level}, { X, high_level} }, IND_MPD_MATERIAL_GREEN, nil, 3 )
		draw_line( { {-X, low_level}, {-X, high_level} }, IND_MPD_MATERIAL_GREEN, nil, 3 )
		X = X + steep_X
	end

	draw_turn_rate_caption( {0, high_level},  IND_MPD_1024_MATERIAL_GREEN )
	draw_turn_rate_caption( {steep_X*2, high_level},  IND_MPD_1024_MATERIAL_GREEN )
	draw_turn_rate_caption( {-steep_X*2, high_level},  IND_MPD_1024_MATERIAL_GREEN )

	draw_turn_rate_square( {0,low_level + 5 },  IND_MPD_1024_MATERIAL_GREEN, nil, controllers )
end
-- ----- -----------------------------------------------------------------------------------------------
function draw_skid_slip_ball( pos,  material, parent, controllers )
	local scale = 1
	
	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(35, 35,  "CenterBottom")
	
	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {220 * texture_scale_1024, 865 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
		elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_skid_slip(controllers)
	local low_level = -490;
	local horiz_line_len = 230
	local steep_X = 33;
	local line_height = 33;
	local high_level = low_level + line_height;
	
	draw_line( { { -horiz_line_len/2, low_level}, { horiz_line_len/2, low_level} }, IND_MPD_MATERIAL_GREEN, nil, 3 )
	local X = steep_X/2
	draw_line( { { X, low_level}, { X, high_level} }, IND_MPD_MATERIAL_GREEN, nil, 3 )
	draw_line( { {-X, low_level}, {-X, high_level} }, IND_MPD_MATERIAL_GREEN, nil, 3 )

	draw_skid_slip_ball(  {0,low_level + 5 },  IND_MPD_1024_MATERIAL_GREEN, nil, controllers )
end
-- -----------------------------------------------------------------------------------------------
function draw_stabilizator( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= {{-15,-15},{-15,15},{70,15},{70,-15}}

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {388 * texture_scale_1024, 770 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
	elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
--default_box_indices  = {0, 1, 2, 0, 2, 3}
-- -----------------------------------------------------------------------------------------------
function draw_stabilizator_scale( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(40, 80,  "CenterCenter")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {460 * texture_scale_1024, 700 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
	elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_stabilizator_question( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= {{-35,-10},{-35,25},{-6,25},{-6,-10}}

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {420 * texture_scale_1024, 700 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
	elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_water_line_offset( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(200, 35,  "CenterCenter")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {100 * texture_scale_1024, 880 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
	elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_water_line( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(200, 35,  "CenterCenter")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {100 * texture_scale_1024, 930 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
	elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_flight_path_vector( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(90, 90,  "CenterCenter")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {300 * texture_scale_1024, 880 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
	elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_FLY_To_CUE( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(120, 120,  "CenterCenter")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {440 * texture_scale_1024, 870 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
	elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- ----------------------------------------------------------------------------------------------------
-- -----------------------------------------------------------------------------------------------
function draw_stabilizator_failed( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(120, 120,  "CenterCenter")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {440 * texture_scale_1024, 870 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
	elem.parent_element 	= parent
	end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
---------------------------------------------------------------------------------------------
function draw_bank_angle_mark( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(70, 30,  "CenterTop")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {940 * texture_scale_1024, 880 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
		elem.parent_element 	= parent
		end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_Climb_indicator( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(30, 40,  "RightCenter")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {270 * texture_scale_1024, 940 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
		elem.parent_element 	= parent
		end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_acceleration_indicator( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(25, 20,  "LeftCenter")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {280 * texture_scale_1024, 940 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
		elem.parent_element 	= parent
		end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_tell_tale( pos,  material, parent, controllers )
--	local mask = openMaskArea( 0, nil,  buildBoxVerts(25, 20,  "LeftCenter"), default_box_indices, pos, parent, controllers   )
	local verts = {{ 0, 0 }, { 25, 10 }, { 25, -10 }}
	local indices = { 0,1,2 }
   	local back = addMesh(nil, verts, indices, pos,"triangles", parent, controllers,"MFD_BACKGROUND")
	draw_acceleration_mark( {0,0},  material, back.name )
end
----------------------------------------------------------------------------------------------------
function draw_acceleration_mark( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(25, 20,  "LeftCenter")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {310 * texture_scale_1024, 940 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
		elem.parent_element 	= parent
		end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_acceleration_border( pos,  material, parent, controllers ) -- red circle
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(15, 15,  "CenterCenter")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {350 * texture_scale_1024, 940 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
		elem.parent_element 	= parent
		end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_acceleration()
	local mark_steep = 55
	local one_mark_len = 40
	local mark_len = 30
	local ph = addPlaceholder( nil, {-510, -30-mark_steep}, nil, {{"FLT_Acceleration"}})

	local zero_pos = {-510 + mark_len/2, -30-mark_steep}

	-- marks
	local mark_Y = 0-mark_steep
	for i = 1, 6 do
		if mark_Y ~= mark_steep then
			draw_line( {{0, mark_Y}, {mark_len, mark_Y}},		IND_MPD_MATERIAL_GREEN, ph.name, 3 )
		else	
			draw_line( {{0,mark_Y}, {one_mark_len, mark_Y}}, 	IND_MPD_MATERIAL_GREEN, ph.name, 6 )
		end
		mark_Y = mark_Y + mark_steep
	end
	
	draw_tell_tale(   zero_pos,  IND_MPD_1024_MATERIAL_GREEN, nil, {{"FLT_AccelerationMarkUp"}} )
	draw_tell_tale(  zero_pos,  IND_MPD_1024_MATERIAL_GREEN, nil, {{"FLT_AccelerationMarkDn"}}  )

	draw_acceleration_border(  zero_pos,  IND_MPD_1024_MATERIAL_RED, nil,{{"FLT_AccelerationBorderUp"}} ) 
	draw_acceleration_border(  zero_pos,  IND_MPD_1024_MATERIAL_RED, nil, {{"FLT_AccelerationBorderDn"}} )
	draw_acceleration_indicator( zero_pos,  IND_MPD_1024_MATERIAL_GREEN, nil, {{"FLT_AccelerationIndicator"}} )
end
-- -----------------------------------------------------------------------------------------------
function draw_Alternate_Bearing_indicator( pos,  material, parent, controllers, h_clip_relation, level )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(20, 20,  "CenterTop")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {820 * texture_scale_1024, 920 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
		elem.parent_element 	= parent
	end

	if controllers ~= nil then
		elem.controllers	= controllers
	end

	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
		elem.level				= level
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_ADF_Bearing_indicator( pos,  material, parent, controllers, h_clip_relation, level  )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(20, 25,  "CenterTop")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {770 * texture_scale_1024, 880 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
		elem.parent_element 	= parent
	end

	if controllers ~= nil then
		elem.controllers	= controllers
	end

	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
		elem.level				= level
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_Bobup_Heading_indicator( pos,  material, parent, controllers, h_clip_relation, level  )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(30, 30,  "CenterTop")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {700 * texture_scale_1024, 920 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
		elem.parent_element 	= parent
	end

	if controllers ~= nil then
		elem.controllers	= controllers
	end

	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
		elem.level				= level
	end

	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_FCR_Centerline_Bearing_indicator( pos,  material, parent, controllers, h_clip_relation, level  )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(50, 20,  "CenterTop")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {820 * texture_scale_1024, 880 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
		elem.parent_element 	= parent
	end

	if controllers ~= nil then
		elem.controllers	= controllers
	end

	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
		elem.level				= level
	end

	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_vertical_indicator( pos,  material, parent, controllers, h_clip_relation, level  )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(14, 480,  "CenterTop")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {700* texture_scale_1024, 0 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
		elem.parent_element 	= parent
	end

	if controllers ~= nil then
		elem.controllers	= controllers
	end

	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
		elem.level				= level
	end

	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_bias_arrows( pos,  material, parent, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(32, 152,  "CenterCenter")

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= {550* texture_scale_1024, 850 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if parent ~= nil then
		elem.parent_element 	= parent
	end

	if controllers ~= nil then
		elem.controllers	= controllers
	end

	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
		elem.level				= level
	end

	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_radar_vertical_scale( controllers, parent )
	local wide_line_steep = 120
	local narrow_line_steep = 24
	local pos_X = 475+6
	local pos_Y = -wide_line_steep
	local wide_line_len = 20;
	local narrow_line_len = 10;

	local ph = addPlaceholder(nil, {pos_X, 0}, parent, controllers)
	-- wide lines
	draw_line( {{ 0, 0 }, { wide_line_len, 0 }}, IND_MPD_MATERIAL_GREEN, ph.name, 3 )	
	for i = 1, 2 do
		draw_line( {{ 0,  wide_line_steep * i }, {  wide_line_len,  wide_line_steep * i }}, IND_MPD_MATERIAL_GREEN, ph.name, 3 )	
		draw_line( {{ 0, -wide_line_steep * i }, {  wide_line_len, -wide_line_steep * i }}, IND_MPD_MATERIAL_GREEN, ph.name, 3 )	
	end

	for i = 1, 4 do
		draw_line( {{ 0, pos_Y-narrow_line_steep * i }, { 0 + narrow_line_len, pos_Y-narrow_line_steep * i }}, IND_MPD_MATERIAL_GREEN, ph.name, 3 )	
	end

end
-- -----------------------------------------------------------------------------------------------
function draw_Climb_vertical_scale()
	local wide_line_steep = 120
	local narrow_line_steep = 24
	local pos_X = 475-6
	local wide_line_len = 20;
	local narrow_line_len = 10;

	-- wide lines
	draw_line( {{ pos_X - wide_line_len, 0 }, { pos_X + 12, 0 }}, IND_MPD_MATERIAL_GREEN, nil, 8 )	
	for i = 1, 2 do
		draw_line( {{ pos_X - wide_line_len,  wide_line_steep * i }, { pos_X ,  wide_line_steep * i }}, IND_MPD_MATERIAL_GREEN, nil, 3 )	
		draw_line( {{ pos_X - wide_line_len, -wide_line_steep * i }, { pos_X , -wide_line_steep * i }}, IND_MPD_MATERIAL_GREEN, nil, 3 )	
	end

	-- narrow lines
	for i = 1, 4 do
		draw_line( {{ pos_X - narrow_line_len,  narrow_line_steep * i }, { pos_X ,  narrow_line_steep * i }}, IND_MPD_MATERIAL_GREEN, nil, 3 )	
		draw_line( {{ pos_X - narrow_line_len, -narrow_line_steep * i }, { pos_X , -narrow_line_steep * i }}, IND_MPD_MATERIAL_GREEN, nil, 3 )	
	end
	-- add text
	addText( "",  {420,  2*wide_line_steep}, tp_36_right_center_white , {{"FLT_ClimbUpValue"}} )
	addText( "",  {420, -2*wide_line_steep}, tp_36_right_center_white , {{"FLT_ClimbDnValue"}} )
end
--------------------------------------------------------------------------------------------
function draw_attitude_line( vertices, tex_params, pos, material, parent, controllers )
	local scale = 1
	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= vertices

	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params		= tex_params
	if parent ~= nil then
		elem.parent_element = parent
		end
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
	return elem
end
--------------------------------------------------------------------------------------------
function draw_attitude_line_r_dn( pos,  material, parent, controllers )
	local vertices		= buildBoxVerts(100, 15,  "RightBottom")
	local tex_params	= {200* texture_scale_1024, 840 * texture_scale_1024, texture_scale_1024, texture_scale_1024} 
	return draw_attitude_line( vertices, tex_params, pos, material, parent, controllers )
end
--------------------------------------------------------------------------------------------
function draw_attitude_line_r_up( pos,  material, parent, controllers )
	local vertices		= buildBoxVerts(100, 15,  "RightTop")
	local tex_params	= {200* texture_scale_1024, 850 * texture_scale_1024, texture_scale_1024,  texture_scale_1024} 
	return draw_attitude_line( vertices, tex_params, pos, material, parent, controllers )
end
--------------------------------------------------------------------------------------------
function draw_attitude_line_l_dn( pos,  material, parent, controllers )
	local vertices		= buildBoxVerts(100, 15,  "LeftBottom")
	local tex_params	= {0* texture_scale_1024, 840 * texture_scale_1024, texture_scale_1024,  texture_scale_1024} 
	return draw_attitude_line( vertices, tex_params, pos, material, parent, controllers )
end
--------------------------------------------------------------------------------------------
function draw_attitude_line_l_up( pos,  material, parent, controllers )
	local vertices		= buildBoxVerts(100, 15,  "LeftTop")
	local tex_params	= {0* texture_scale_1024, 850 * texture_scale_1024, texture_scale_1024,  texture_scale_1024} 
	return draw_attitude_line( vertices, tex_params, pos, material, parent, controllers )
end
--------------------------------------------------------------------------------------------
function draw_attitude_short_line( pos,  material, parent, controllers )
	local vertices		= buildBoxVerts(20, 10,  "CenterCenter")
	local tex_params	= {40* texture_scale_1024, 850 * texture_scale_1024, texture_scale_1024,  texture_scale_1024} 
	return draw_attitude_line( vertices, tex_params, pos, material, parent, controllers )
end
--------------------------------------------------------------------
function addMaskedRotPlaceholder(level, name, pos, rot, parent, controllers)
	local placeholder			= CreateElement "ceMeshPoly"
	placeholder.name			= name
	pos							= pos or {0, 0}
	placeholder.init_pos		= {pos[1], pos[2], 0}

	if parent ~= nil then
		placeholder.parent_element	= parent
	end	

	if controllers ~= nil then
		placeholder.controllers		= controllers
	end

	if rot ~= nil then
		placeholder.init_rot	= {rot}
	end
	
	placeholder.h_clip_relation	= h_clip_relations.COMPARE
	placeholder.level = level
	Add(placeholder)
	return placeholder
end
-----------------------------------------------------------
----------------------------------------------------------------------------------------------------
function draw_ring_mask( level, pos, r_l, r_h   )
	local circle  	 		= CreateElement "ceCircle"
	--circle.name 			= "SimpleCircle"..getUnicID()
	circle.primitivetype   	= "triangles"

	circle.vertices			= buildBoxVerts(2*r_h, 2*r_h, "CenterCenter")
	circle.radius 			= { r_l, r_h, 3 }
	circle.indices	        = default_box_indices
	circle.init_pos			= pos
--	circle.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
--	circle.level           	= level
	circle.h_clip_relation	= h_clip_relations.INCREASE_IF_LEVEL
	circle.level			= DEFAULT_LEVEL + level
	circle.material         = "MASK_MATERIAL_PURPLE"
	circle.additive_alpha	= false
	circle.change_opacity	= false
    circle.isdraw           = true
    circle.isvisible        = true
	Add(circle)
	return circle
end
-----------------------------------------------------------
function getPosOnRose(radius, angle)
	return { radius * math.sin(math.rad(angle)), radius * math.cos(math.rad(angle)) }
end

function join(table, elem) 
	table[#table + 1] = elem
end

function AddBankAngleSegment_upper( controllers )
	local lubber_line_l			= 300
	local lubber_line_h			= 345

	local long_mark_radius_l	= 290 
	local long_mark_radius_h	= 310
	local short_mark_radius_l	= 300
	local short_mark_radius_h	= 310
	
	local PH = addPlaceholder( "BankAngleSegment_upper_PH", {0,0}, nil, controllers)
	local elems = { PH }
	
	-- const marks		
	local line = draw_line( {getPosOnRose(lubber_line_l, 0), getPosOnRose(lubber_line_h, 0)}, IND_MPD_MATERIAL_GREEN, PH.name, 6, nil, controllers )	
	join(elems, line)
	
	for i = 335, 355, 10 do
		line = draw_line( {getPosOnRose(short_mark_radius_l, i), getPosOnRose(short_mark_radius_h, i)}, IND_MPD_MATERIAL_GREEN, PH.name, 3, nil, controllers )
		join(elems, line)
	end

	for i = 5, 25, 10 do
		line = draw_line( {getPosOnRose(short_mark_radius_l, i), getPosOnRose(short_mark_radius_h, i)}, IND_MPD_MATERIAL_GREEN, PH.name, 3, nil, controllers )	
		join(elems, line)
	end

	local index = 10
	line = draw_line( {getPosOnRose(long_mark_radius_l, index), getPosOnRose(long_mark_radius_h, index)}, IND_MPD_MATERIAL_GREEN, PH.name, 3, nil, controllers )
	join(elems, line)	
	
	index = index + 10
	line = draw_line( {getPosOnRose(long_mark_radius_l, index), getPosOnRose(long_mark_radius_h, index)}, IND_MPD_MATERIAL_GREEN, PH.name, 3, nil, controllers )	
	join(elems, line)
	
	index = index + 10
	line = draw_line( {getPosOnRose(long_mark_radius_l, index), getPosOnRose(long_mark_radius_h, index)}, IND_MPD_MATERIAL_GREEN, PH.name, 6, nil, controllers )	
	join(elems, line)
	
	index = 330
	line = draw_line( {getPosOnRose(long_mark_radius_l, index), getPosOnRose(long_mark_radius_h, index)}, IND_MPD_MATERIAL_GREEN, PH.name, 6, nil, controllers )	
	join(elems, line)
	
	index = index + 10
	line = draw_line( {getPosOnRose(long_mark_radius_l, index), getPosOnRose(long_mark_radius_h, index)}, IND_MPD_MATERIAL_GREEN, PH.name, 3, nil, controllers )	
	join(elems, line)
	
	index = index + 10
	line = draw_line( {getPosOnRose(long_mark_radius_l, index), getPosOnRose(long_mark_radius_h, index)}, IND_MPD_MATERIAL_GREEN, PH.name, 3, nil, controllers )	
	join(elems, line)
end

function AddBankAngleSegment( ang_st, ang_end, ang_mark, ph, controllers )

	local huge_mark_radius_l = 290 	
	local huge_mark_radius_h = 335 		
	local long_mark_radius_l = 290 	
	local long_mark_radius_h = 310 
	
	for i = ang_st, ang_end, 10 do
		draw_line( {getPosOnRose(long_mark_radius_l, i), getPosOnRose(long_mark_radius_h, i)}, IND_MPD_MATERIAL_GREEN, ph, 3, nil, controllers )	
	end
	if ang_mark > 0 then 
		draw_line( {getPosOnRose(huge_mark_radius_l, ang_mark), getPosOnRose(huge_mark_radius_h, ang_mark)}, IND_MPD_MATERIAL_GREEN, ph, 6, nil, controllers )	
	end
end

function add_square(name, half_sz, pos, material, clip_rel, level, disable_mask)
    local elem               = CreateElement "ceMeshPoly"
    elem.name                = name
    elem.primitivetype       = "triangles"
    elem.vertices            = {{-half_sz, -half_sz}, {-half_sz, half_sz}, {half_sz, half_sz}, {half_sz, -half_sz}}
    elem.indices             = default_box_indices
    elem.init_pos            = pos
    elem.material            = "MASK_MATERIAL_PURPLE"
	elem.additive_alpha		= false
	elem.change_opacity		= false

    if disable_mask == false then
        elem.h_clip_relation = clip_rel
        elem.level           = level
    end
 
    elem.isdraw              = true
    elem.isvisible           = true
    Add(elem)
 
    return elem
end
function addAltitudeHold(name, pos, parent, controllers)
	local x = pos[1]
	local y = pos[2]

	local w = 28
	local w05 = 11
	local h = 36
	local h05 = h / 2
	local verts = 
	{
		{x-w, y},
		{x-w, y+h05},
		{x-w05, y+h05},
		{x, y},
		{x-w05, y-h05},
		{x-w, y-h05},
		{x-w, y},
	}
	local elems = draw_line(verts, IND_MPD_MATERIAL_GREEN, parent, 3, nil, controllers)
	return elems
end

local function buildBorderedSymbol(name, size, align, tex_pos, pos, parent, controllers)
	local elem = buildSymbol(name, IND_MPD_VIDEO_SYMBOLOGY, size, pos, align, 512, tex_pos, 2/1.4222, parent, controllers)
	
	local back_elem = Copy(elem)
	back_elem.material = IND_MPD_VIDEO_SYMBOLOGY_BLACK

	addElem(back_elem)
	addElem(elem)
	return {back_elem, elem}
end
function addSkidSlipBall(name, pos, parent, controllers)
	local elems = buildBorderedSymbol(name, {50, 50}, "CenterCenter", {100,240}, pos, parent, controllers)
	return elems
end