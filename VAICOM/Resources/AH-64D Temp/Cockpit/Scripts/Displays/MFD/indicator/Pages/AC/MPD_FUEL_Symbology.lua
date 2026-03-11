dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
-- -----------------------------------------------------------------------------------------------

function draw_plane( pos,  material )
local width = 2
--	local scale = 0.5

--	local elem  		= CreateElement "ceTexPoly"
--	elem.vertices		=  {{-330 , 310 }, {330 , 310}, {330 , -300}, {-330 , -300}}
--	elem.indices		= box_ind
--	elem.material		= material
--	elem.init_pos		= pos
--	elem.tex_params	= {210 * texture_scale, 150 * texture_scale, scale * texture_scale, scale * texture_scale} 
--	Add(elem)
	
local verts_tank1	=
{
{-62.87,	112.43},
{-157.13,	112.43},
{-139.45,	12.28},
{-157.13,	-87.86},
{-62.87,	-87.86},
{-80.55,	12.28},
{-62.87,	112.43},
}
local verts_tank2 =
{	
{157.13,	112.43},
{62.87,		112.43},
{80.55,		12.28},
{62.87,		-87.86},
{157.13,	-87.86},
{139.45,	12.28},
{157.13,	112.43},
}
--local verts = splitVerts(verts, verts1)
draw_line( verts_tank1,  material, nil, width )
draw_line( verts_tank2, material, nil, width )	
addString( "1", {-110, -4}, tp_36_center_bottom, nil, nil, nil, nil, nil, true	 )
addString( "2", { 110, -4}, tp_36_center_bottom, nil, nil, nil, nil, nil, true	 )


local plane_left =
{	
{146.77,	53.95}, 
{311.71,	71.63},
{311.71,	165.88},
{75.49,		183.55},
{75.49,		336.71},

}
local plane_rigt =
{	
{-146.77,	53.95},
{-311.71,	71.63},
{-311.71,	165.88},
{-75.49,	183.55},
{-75.49,	336.71},
}
            

local plane = 
{
{75.49,	336.71},
{75.31,	337.89},
{75.11,	339.07},
{74.88,	340.25},
{74.62,	341.43},
{74.34,	342.60},
{74.03,	343.78},
{73.70,	344.96},
{73.34,	346.14},
{72.95,	347.32},
{72.53,	348.49},
{72.09,	349.67},
{71.61,	350.85},
{71.11,	352.03},
{70.57,	353.21},
{70.00,	354.39},
{69.40,	355.56},
{68.76,	356.74},
{68.09,	357.92},
{67.38,	359.10},
{66.64,	360.28},
{65.85,	361.45},
{65.02,	362.63},
{64.14,	363.81},
{63.22,	364.99},
{62.25,	366.17},
{61.22,	367.34},
{60.14,	368.52},
{58.99,	369.70},
{57.78,	370.88},
{56.49,	372.06},
{55.12,	373.24},
{53.66,	374.41},
{52.1,	375.59},
{50.42,	376.77},
{48.61,	377.95},
{46.63,	379.13},
{44.45,	380.30},
{42.03,	381.48},
{39.27,	382.66},
{36.02,	383.84},
{31.95,	385.02},
--{25.91,	386.20},
--}
--local plane_ring_top =              
--{

--{29.07,	 386.20},
--{28.86,	 387.95},
--{28.59,	 389.13},
--{28.28,	 390.30},
--{27.91,	 391.48},
--{27.48,	 392.66},
--{27.00,	 393.84},
--{26.45,	 395.02},
--{25.91,	 381.20},
{25.84,	 391.20},
{25.16,	 392.37},
{24.41,	 393.55},
{23.57,	 394.73},
{22.63,	 395.91},
{21.59,	 397.09},
{20.45,	 398.26},
{19.15,	 399.44},
{17.68,	 400.62},
{15.98,	 401.80},
{13.99,	 402.98},
{11.54,	 404.16},
{8.7,	 405.33},
{0.9,	 406.51},
{-8.7,	 405.33},
{-11.54, 404.16},
{-13.99, 402.98},
{-15.98, 401.80},
{-17.68, 400.62},
{-19.15, 399.44},
{-20.45, 398.26},
{-21.59, 397.09},
{-22.63, 395.91},
{-23.57, 394.73},
{-24.41, 393.55},
{-25.16, 392.37},
{-25.84, 391.20},
--{-25.91, 381.20},
--{-26.45, 395.02},
--{-27.00, 393.84},
--{-27.48, 392.66},
--{-27.91, 391.48},
--{-28.28, 390.30},
--{-28.59, 389.13},
--{-28.86, 387.95},
--{-29.07, 386.20},
--}  

--local plane_ring_right = 
--{
--{-25.91,	386.20},
{-31.95,	385.02},
{-36.02,	383.84},
{-39.27,	382.66},
{-42.03,	381.48},
{-44.45,	380.30},
{-46.63,	379.13},
{-48.61,	377.95},
{-50.42,	376.77},
{-52.1,		375.59},
{-53.66,	374.41},
{-55.12,	373.24},
{-56.49,	372.06},
{-57.78,	370.88},
{-58.99,	369.70},
{-60.14,	368.52},
{-61.22,	367.34},
{-62.25,	366.17},
{-63.22,	364.99},
{-64.14,	363.81},
{-65.02,	362.63},
{-65.85,	361.45},
{-66.64,	360.28},
{-67.38,	359.10},
{-68.09,	357.92},
{-68.76,	356.74},
{-69.40,	355.56},
{-70.00,	354.39},
{-70.57,	353.21},
{-71.11,	352.03},
{-71.61,	350.85},
{-72.09,	349.67},
{-72.53,	348.49},
{-72.95,	347.32},
{-73.34,	346.14},
{-73.70,	344.96},
{-74.03,	343.78},
{-74.34,	342.60},
{-74.62,	341.43},
{-74.88,	340.25},
{-75.11,	339.07},
{-75.31,	337.89},
{-75.49,	336.71},
}  

local plane_bottom = 
{
{ -80, -87},
{ -80, -127},  
{  80, -127},
{  80, -87},   
}
draw_line( plane_left,  material, nil, width )
draw_line( plane_rigt, material, nil, width )	

draw_line( plane, material, nil, width )
draw_line( plane_bottom, material, nil, width )	

local circle1 = buildEllipseVerts({110,132}, 20, 20)
local circle2 = buildEllipseVerts({-110,132}, 20, 20)
draw_line( circle1, material, nil, width )	
draw_line( circle2, material, nil, width )

end
-- -----------------------------------------------------------------------------------------------
tank_center_pos			= {{-255, 120}, {-190, 120}, {190,120}, {255,120} }
function draw_fuel_tank( num,  material, controllers )
	local scale = 0.45
	
	local elem  		= CreateElement "ceTexPoly"
	elem.name 			= "Tank"..num
	elem.vertices		= buildBoxVerts(64, 200, "CenterCenter")
	if controllers ~= nil then
		elem.controllers= controllers
	end
	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= tank_center_pos[num]
	elem.tex_params	= {410 * texture_scale, 46 * texture_scale, scale * texture_scale, scale * texture_scale} 

	elem.h_clip_relation = h_clip_relations.COMPARE 	
	elem.level			= 2	
					
	addMaskArea(2, h_clip_relations.REWRITE_LEVEL, nil,	elem.vertices, default_box_indices, tank_center_pos[num], nil, {{"FUEL_TankMask", num}}, "TRANSPARENT" )
	Add(elem)
	addText( "E", tank_center_pos[num], tp_36_white, {{"FUEL_ExtTankEmpty",num}},nil,nil,nil,nil, h_clip_relations.COMPARE, 2 )
--	function openMaskArea(level, name, vertices, indices, pos, parent, controllers, material)
--	openMaskArea(0, nil, elem.vertices, default_box_indices, tank_center_pos[num], nil, {{"FUEL_TankMask", num}})
	
	return elem
end
------------------------------------------------------------------------
function draw_dot_line( verts, material, parent, width, name, controllers, h_clip_relation, level )
	local tex_param_default = texture_scale
	local scale = 1
	if width ~= nil then
		scale = 3 / width
	end
	local elem			= CreateElement "ceSimpleLineObject"
	elem.material			= material
	elem.width			= width or 3
	if parent ~= nil then
		elem.parent_element = parent
	end
	elem.tex_params		= {{0.0,489*texture_scale},{13/512,489*texture_scale}, {texture_scale*scale, tex_param_default*scale }} 
	elem.vertices			= verts

	if controllers ~= nil then
		elem.controllers	= controllers
	end
	if name ~= nil then
		elem.name	= name
	end
	if h_clip_relation ~= nil then
		elem.h_clip_relation	= h_clip_relation
		elem.level				= level
	end

	Add(elem)
	return elem
end
--------------------------------------------------------------------------------------------------
function draw_fuel_line( verts, index)
-- draw_line( verts, material, parent, width, name, controllers, h_clip_relation, level )
draw_line( verts, IND_MPD_MATERIAL_WHITE, nil, 4, nil,  {{"FUEL_Line", index}}, h_clip_relations.REWRITE_LEVEL, DEFAULT_LEVEL + 2  )	
end
--------------------------------------------------------------------------------------------------
function draw_fuel_dot_line( verts, index)
	local mask_verts = { {20, 280}, {40, 280}, {40, 15}, {20, 15} }
	addMaskArea(DEFAULT_LEVEL + 2, h_clip_relations.REWRITE_LEVEL, nil,	mask_verts, default_box_indices, {0, 0 }, nil, nil, "TRANSPARENT" )
	draw_dot_line( verts, IND_MPD_MATERIAL_WHITE, nil, 6, "FUEL_dot_line",  {{"FUEL_Line", index}}, h_clip_relations.COMPARE, DEFAULT_LEVEL + 2 )	
	end