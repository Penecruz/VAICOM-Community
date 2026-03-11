dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
-- -----------------------------------------------------------------------------------------------
-- -----------------------------------------------------------------------------------------------
function draw_check_mark_with_dots( pos,  material )
	local scale = 1
	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(160, 42,  "CenterTop")
	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params	= {200 * texture_scale_1024, 0 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_rotor_bar( pos,  material, controllers, h_clip_relation, level )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"

	elem.vertices		= buildBoxVerts(84, 432,  "CenterBottom")
	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params	= {50 * texture_scale_1024, 652 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	if  h_clip_relation ~= nil then
        elem.h_clip_relation = h_clip_relation
        elem.level           = level
    end
	Add(elem)
	return elem
end
-- -----------------------------------------------------------------------------------------------
function draw_NP_bar( pos,  material, controllers, h_clip_relation, level )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(84, 432,  "CenterBottom")
	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params	= {200 * texture_scale_1024, 650 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	if  h_clip_relation ~= nil then
        elem.h_clip_relation = h_clip_relation
        elem.level           = level
    end
	Add(elem)
	return elem
end
-- -----------------------------------------------------------------------------------------------
function draw_TGT_bar( pos,  material, controllers, h_clip_relation, level )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(84, 432,  "CenterBottom")
	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params	= {300 * texture_scale_1024, 650 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	if  h_clip_relation ~= nil then
        elem.h_clip_relation = h_clip_relation
        elem.level           = level
    end
	Add(elem)
	return elem
end
-- -----------------------------------------------------------------------------------------------
function draw_Torque_bar( index, pos,  material, parent, controllers, h_clip_relation, level )
	local scale = 1
	local texture_start_tbl = { {400,460}, {400,570}, {500,650}, {600,650} }
	local texture_start_pos = texture_start_tbl[index]
	local elem  			= CreateElement "ceTexPoly"
	elem.vertices			= buildBoxVerts(84, 432,  "CenterBottom")
	elem.indices			= default_box_indices
	elem.material			= material
	elem.init_pos			= pos
	elem.parent_element 	= parent
	elem.tex_params	= { texture_start_pos[1]*texture_scale_1024, texture_start_pos[2]*texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	if  h_clip_relation ~= nil then
        elem.h_clip_relation = h_clip_relation
        elem.level           = level
    end
	Add(elem)
	return elem
end
-- -----------------------------------------------------------------------------------------------
function draw_check_mark( pos,  material, controllers  )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(200, 20,  "CenterCenter")
	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params	= {200 * texture_scale_1024, 59 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
-- -----------------------------------------------------------------------------------------------
function draw_timer_mark( pos,  material, controllers )
	local scale = 1

	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= buildBoxVerts(200, 12,  "CenterCenter")
	elem.indices		= default_box_indices
	elem.material		= material
	elem.init_pos		= pos
	elem.tex_params	= {200 * texture_scale_1024, 90 * texture_scale_1024, scale * texture_scale_1024, scale * texture_scale_1024} 
	if controllers ~= nil then
		elem.controllers	= controllers
	end
	Add(elem)
end
--------------------------------------------------------------------------------------------------

