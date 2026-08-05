-- Appended by Vaicom for Mini Wheel Jester AI Page
dofile(LockOn_Options.script_path.."paths.lua")
dofile(LockOn_Options.common_script_path.."devices_defs.lua")
dofile(LockOn_Options.common_script_path.."ViewportHandling.lua")

local cx = 0
local cy = 0
local aspect = 1
scale = 1

if GetSelf():is_VR() == false then 
	local viewJester = find_viewport("F14_JESTER_MENU") or find_viewport("GU_MAIN_VIEWPORT")
	if viewJester == nil then
		viewJester = {
		x = 0;
		y = 0;
		width = LockOn_Options.screen.width;
		height = LockOn_Options.screen.height;
		}
	end

	cx = ( ( ( viewJester.x + viewJester.width * 0.5 ) / LockOn_Options.screen.width ) - 0.5 ) * 2.0
	cy = ( ( ( viewJester.y + viewJester.height* 0.5 ) / LockOn_Options.screen.height ) - 0.5 ) * 2.0
	aspect = LockOn_Options.screen.width / LockOn_Options.screen.height

	scale = viewJester.height / LockOn_Options.screen.height
	if( scale > 0.95 and scale < 1.05 ) then
		scale = 1
	end
end
SetCustomScale(scale)

dofile(LockOn_Options.common_script_path.."elements_defs.lua")
dofile(base_script_path.."Scripts/Common/levels.lua")
dofile(LockOn_Options.common_script_path.."devices_defs.lua")


INDICATOR_LEVEL = JESTER_DEFAULT_LEVEL

dofile(base_script_path.."fonts.lua")
dofile(base_script_path.."Scripts/Common/common_defs.lua")
IndTexture_Path = base_script_path.."Resources/IndicationTextures/"



local alpha = 190


local BaseCircleMat = MakeMaterial(IndTexture_Path.."JUI_Base_Circle.dds", {255, 255, 255, alpha})
local OuterTriangleMat = MakeMaterial(IndTexture_Path.."JUI_Outer_Triangle.tga", {255, 255, 255, alpha})
local InnerTriangleMat = MakeMaterial(IndTexture_Path.."JUI_Inner_Triangle.tga", {255, 255, 255, alpha})
local CompassRingMat = MakeMaterial(IndTexture_Path.."JUI_CompassRing.dds", {255, 255, 255, alpha})
local CategoriesMat = MakeMaterial(IndTexture_Path.."JUI_Categories.tga", {255, 255, 255, alpha})
local HighlightItemMat = MakeMaterial(IndTexture_Path.."JUI_Highlight.dds", {255, 255, 255, alpha-60})
local ShadedItemMat = MakeMaterial(IndTexture_Path.."JUI_GreyOut.dds", {255, 255, 255, alpha})
local StatusBarMat = MakeMaterial(IndTexture_Path.."JUI_StatusBar.dds", {255, 255, 255, alpha})





function AddElement(object)
	object.screenspace = ScreenType.SCREENSPACE_TRUE
    object.use_mipfilter = true
    Add(object)
end

local base_font_size = 0.0042 * scale
local large_font_scale = 2.84
local small_font_scale = 0.705
local jester_stringdef   = {base_font_size, 0.5844 * base_font_size, 0, 0}
local jester_stringdef_large   = {large_font_scale * base_font_size, large_font_scale * 0.5844 * base_font_size, 0, 0}
local jester_stringdef_small   = {small_font_scale * base_font_size, small_font_scale * 0.5844 * base_font_size, 0, 0}
local string_shift = base_font_size * 0.116
local string_shift_large = large_font_scale * string_shift
--local jester_stringdef_even_larger   = {2.9 * base_font_size, 2.9 * 0.625 * base_font_size, 0, 0}

local grid_origin = create_origin()
grid_origin.init_pos = {cx * aspect, -cy, 0}
grid_origin.element_params = {"JESTER_WHEEL_VISIBLE"}
grid_origin.controllers = {{"show"}, {"parameter_in_range", 0, 0.9, 1.1}}

local small = 0.01 * scale

function MakePieNumber(index,pos_x,pos_y,press_x,press_y)
	local pie_origin = create_origin(create_guid_string())
		pie_origin.parent_element = grid_origin.name
		pie_origin.init_pos = {pos_x * scale, pos_y * scale}
	local dbg_vert = create_textured_box(0,0,64,64,256,64)
		dbg_vert.name = create_guid_string()
		dbg_vert.vertices = {{-small, small},
                        { small, small},
                        { small,-small},
                        {-small,-small}}
		dbg_vert.parent_element = pie_origin.name
		dbg_vert.init_pos		= {0,0}
		dbg_vert.material = CategoriesMat
		dbg_vert.isdraw = true
		dbg_vert.use_mipfilter = true
		dbg_vert.additive_alpha = false
	--AddElement(dbg_vert)
		
	local pie_num_str           = CreateElement "ceStringPoly"
		pie_num_str.name            = create_guid_string()
		pie_num_str.material        = "font_JesterUI_Grey"
		pie_num_str.parent_element = grid_origin.name
		pie_num_str.stringdefs    = jester_stringdef_large
		pie_num_str.init_pos = {pos_x * scale, pos_y * scale}
		pie_num_str.alignment     = "LeftBottom"
		pie_num_str.controllers 		= {{"rosetext",index, 1}}
		pie_num_str.isdraw = false
	AddElement(pie_num_str)
	local press = CreateElement "ceStringPoly"
		press.name            = create_guid_string()
		press.material        = "font_JesterUI_Grey"
		press.parent_element = pie_num_str.name
		press.stringdefs    = jester_stringdef_small
		press.init_pos		    = {press_x * scale, press_y * scale}
		press.alignment     = "LeftBotom"
		--press.controllers 		= {{"rosetextcenter", 1}}
		press.value = "PRESS"
		press.isdraw = true	
	AddElement(press)	
end

-- Pixel art Jester portrait
local SHOW_MASKS = false
-- Portrait sprite-sheet material name. Registered in materials.lua so this
-- page and JesterSubtitle_Page reference the same underlying material by
-- name (avoids the double-free at teardown from creating two materials
-- pointing at the same DDS).
local JesterBUMat = get_aircraft_type() == "F-14BU" and "JESTER_PORTRAIT_BU" or "JESTER_PORTRAIT_AB"

-- Clipping masks for circular portrait area
local jestercam_clip_rect          = CreateElement "ceMeshPoly"
	jestercam_clip_rect.name           = create_guid_string()
	jestercam_clip_rect.primitivetype  = "triangles"
	jestercam_clip_rect.vertices = { {-0.31 * scale,0.31 * scale},
									{0.31 * scale,0.31 * scale},
									{ 0.31 * scale,-0.07 * scale},
									{-0.31 * scale,-0.07 * scale}}
    jestercam_clip_rect.indices = {0,1,2,0,2,3}
    jestercam_clip_rect.parent_element = grid_origin.name
	jestercam_clip_rect.isdraw         = true
	jestercam_clip_rect.isvisible      = SHOW_MASKS
    jestercam_clip_rect.change_opacity	= false
    jestercam_clip_rect.h_clip_relation = h_clip_relations.REWRITE_LEVEL
    jestercam_clip_rect.level			= INDICATOR_LEVEL-1
	jestercam_clip_rect.material       = "DBG_RED"
AddElement(jestercam_clip_rect)

local jestercam_clip          = CreateElement "ceMeshPoly"
	jestercam_clip.name           = create_guid_string()
	jestercam_clip.primitivetype  = "triangles"
	set_circle(jestercam_clip, 0.307 * scale)
    jestercam_clip.parent_element = grid_origin.name
	jestercam_clip.isdraw         = true
	jestercam_clip.isvisible      = SHOW_MASKS
    jestercam_clip.h_clip_relation = h_clip_relations.INCREASE_IF_LEVEL
    jestercam_clip.level			= INDICATOR_LEVEL-1
    jestercam_clip.change_opacity	= false
	jestercam_clip.material       = "DBG_RED"
AddElement(jestercam_clip)

local camera_back          = CreateElement "ceMeshPoly"
	camera_back.name           = create_guid_string()
	camera_back.primitivetype  = "triangles"
	camera_back.vertices = { {-0.40 * scale,0.40 * scale},
									{0.40 * scale,0.40 * scale},
									{ 0.40 * scale,-0.40 * scale},
									{-0.40 * scale,-0.40 * scale}}
    camera_back.indices = {0,1,2,0,2,3}
    camera_back.parent_element = grid_origin.name
	camera_back.isdraw         = true
	camera_back.isvisible      = true
    camera_back.controllers = {{"jestercam"}}
    camera_back.h_clip_relation = h_clip_relations.COMPARE
    camera_back.level			= INDICATOR_LEVEL
	camera_back.material       = MakeMaterial(nil, {0, 0, 0, 255})
AddElement(camera_back)

-- Portrait size
local ps = 0.25 * scale -- portrait half-size

-- Helper: create a textured quad from sprite sheet region
-- u1,v1 = top-left UV, u2,v2 = bottom-right UV
local portrait_y_offset = 0.11 * scale

local function make_sprite(name_suffix, u1, v1, u2, v2, half_w, half_h, parent, controllers)
	local elem = CreateElement "ceTexPoly"
	elem.name = "jester_" .. name_suffix
	elem.material = JesterBUMat
	elem.parent_element = parent
	elem.init_pos = {0, 0, 0}
	elem.isdraw = true
	elem.isvisible = true
	elem.h_clip_relation = h_clip_relations.COMPARE
	elem.level = INDICATOR_LEVEL
	elem.vertices = {
		{-half_w, half_h},
		{ half_w, half_h},
		{ half_w, -half_h},
		{-half_w, -half_h}
	}
	elem.tex_coords = {
		{u1, v1},
		{u2, v1},
		{u2, v2},
		{u1, v2}
	}
	elem.indices = {0, 1, 2, 0, 2, 3}
	if controllers then
		elem.controllers = controllers
	end
	AddElement(elem)
	return elem
end

-- Sky background: live camera render with posterize shader (BU)
-- Uses JesterViewMaterial which reads the mfd2 render target
-- Camera is positioned behind aircraft looking at sky (set in C++)
local sky_size = 0.40 * scale
local JesterSky = CreateElement "ceTexPoly"
	JesterSky.name = "jester_sky"
	JesterSky.material = "JesterViewMaterial"
	JesterSky.parent_element = camera_back.name
	JesterSky.init_pos = {0, portrait_y_offset, 0}
	JesterSky.isdraw = true
	JesterSky.isvisible = true
	JesterSky.h_clip_relation = h_clip_relations.COMPARE
	JesterSky.level = INDICATOR_LEVEL
	JesterSky.vertices = {
		{-sky_size, sky_size},
		{ sky_size, sky_size},
		{ sky_size, -sky_size},
		{-sky_size, -sky_size}
	}
	JesterSky.tex_coords = {
		{0, 0}, {1, 0}, {1, 1}, {0, 1}
	}
	JesterSky.indices = {0, 1, 2, 0, 2, 3}
AddElement(JesterSky)

-- Layer 5: Seat (row 0, col 2-3, 2 cells wide)
make_sprite("seat", 0.5, 0.0, 1.0, 0.25, ps*2.75, ps*1.6, camera_back.name, {{"jester_seat", portrait_y_offset-0.1*scale}})

-- Layer 6: Jester portraits (one per state, controller toggles visibility)
-- 0=forward (r0c0), 1=left (r0c1), 2=right (r1c0), 3=dead (r1c1),
-- 4=highG (r1c2), 5=midG (r1c3), 6=negG (r2c0), 7=lookDown (r2c1),
-- 8=stressed (r2c2), 9=lookUp (r2c3), 10=fullLeft (r3c0), 11=fullRight (r3c1),
-- 12=deadLowG (r3c2)
local py = portrait_y_offset + 0.07 * scale
make_sprite("portrait_fwd",      0.0,  0.0,  0.25, 0.25, ps, ps, camera_back.name, {{"jester_portrait", 0, py}})
make_sprite("portrait_left",     0.25, 0.0,  0.5,  0.25, ps, ps, camera_back.name, {{"jester_portrait", 1, py}})
make_sprite("portrait_right",    0.0,  0.25, 0.25, 0.5,  ps, ps, camera_back.name, {{"jester_portrait", 2, py}})
make_sprite("portrait_dead",     0.25, 0.25, 0.5,  0.5,  ps, ps, camera_back.name, {{"jester_portrait", 3, py}})
make_sprite("portrait_highg",    0.5,  0.25, 0.75, 0.5,  ps, ps, camera_back.name, {{"jester_portrait", 4, py}})
make_sprite("portrait_midg",     0.75, 0.25, 1.0,  0.5,  ps, ps, camera_back.name, {{"jester_portrait", 5, py}})
make_sprite("portrait_negg",     0.0,  0.5,  0.25, 0.75, ps, ps, camera_back.name, {{"jester_portrait", 6, py}})
make_sprite("portrait_lookdown", 0.25, 0.5,  0.5,  0.75, ps, ps, camera_back.name, {{"jester_portrait", 7, py}})
make_sprite("portrait_stressed", 0.5,  0.5,  0.75, 0.75, ps, ps, camera_back.name, {{"jester_portrait", 8, py}})
make_sprite("portrait_lookup",   0.75, 0.5,  1.0,  0.75, ps, ps, camera_back.name, {{"jester_portrait", 9, py}})
make_sprite("portrait_fullleft", 0.0,  0.75, 0.25, 1.0,  ps, ps, camera_back.name, {{"jester_portrait", 10, py}})
make_sprite("portrait_fullright",0.25, 0.75, 0.5,  1.0,  ps, ps, camera_back.name, {{"jester_portrait", 11, py}})
make_sprite("portrait_dead_lowg",0.5,  0.75, 0.75, 1.0,  ps, ps, camera_back.name, {{"jester_portrait", 12, py}})


if get_aircraft_type() == "F-14BU" then
	local scanline_size = 0.40 * scale
	local ScanlinesOverlay = CreateElement "ceTexPoly"
		ScanlinesOverlay.name = "jester_scanlines"
		ScanlinesOverlay.material = MakeMaterial(IndTexture_Path.."scanlines.png", {255, 255, 255, 255})
		ScanlinesOverlay.parent_element = camera_back.name
		ScanlinesOverlay.init_pos = {0, 0, 0}
		ScanlinesOverlay.isdraw = true
		ScanlinesOverlay.isvisible = true
		ScanlinesOverlay.h_clip_relation = h_clip_relations.COMPARE
		ScanlinesOverlay.level = INDICATOR_LEVEL
		ScanlinesOverlay.blend_mode = 6 -- multiply
		ScanlinesOverlay.vertices = {
			{-scanline_size, scanline_size},
			{ scanline_size, scanline_size},
			{ scanline_size, -scanline_size},
			{-scanline_size, -scanline_size}
		}
		ScanlinesOverlay.tex_coords = {
			{0, 0}, {1, 0}, {1, 1}, {0, 1}
		}
		ScanlinesOverlay.indices = {0, 1, 2, 0, 2, 3}
	AddElement(ScanlinesOverlay)
end

local status_bar_x = 0.305
--local status_bar_y1 = -0.007812
local status_bar_y1 = -0.0000
local status_bar_y2 = -0.07586
local status_bar = create_textured_box(0,0,312,40,312,40)
	status_bar.vertices = { {-status_bar_x * scale,status_bar_y1 * scale},
							{status_bar_x * scale,status_bar_y1 * scale},
							{status_bar_x * scale,status_bar_y2 * scale},
							{-status_bar_x * scale,status_bar_y2 * scale} }
	status_bar.name 	= "status_bar"
	status_bar.material = StatusBarMat
	status_bar.init_pos = {0,0}
	status_bar.parent_element = grid_origin.name
	status_bar.isdraw = true
	status_bar.use_mipfilter = true
	status_bar.additive_alpha = false
	status_bar.controllers 		= {{"statusbar"}}
AddElement(status_bar)

local status_bar_name           = CreateElement "ceStringPoly"
	status_bar_name.name            = create_guid_string()
	status_bar_name.material        = "font_JesterUI_Grey"
	status_bar_name.parent_element = grid_origin.name
	status_bar_name.stringdefs    = jester_stringdef
	status_bar_name.init_pos		    = {0, -0.0386425 * scale}
	status_bar_name.alignment     = "CenterCenter"
	status_bar_name.controllers 		= {{"statusbar_str"}}
	status_bar_name.isdraw = true
AddElement(status_bar_name)

local compass 			= create_textured_box(0,0,512,512,512,512)
compass.name 	= "rosecenter"
compass.material = CompassRingMat
compass.init_pos = {0,0}
compass.parent_element = grid_origin.name
compass.isdraw = true
compass.use_mipfilter = true
compass.additive_alpha = false
--compass.h_clip_relation = h_clip_relations.REWRITE_LEVEL -- COMPARE -- REWRITE_LEVEL
--compass.level			= DEFAULT_LEVEL
compass.controllers 		= {{"compass", 1}}
AddElement(compass)


local rose_shade 			= create_textured_box(0,0,512,512,512,512)
		rose_shade.name 	= "rosecenter"
		rose_shade.material = BaseCircleMat
		rose_shade.init_pos = {0,0}
		rose_shade.parent_element = grid_origin.name
		rose_shade.isdraw = true
		rose_shade.use_mipfilter = true
		rose_shade.additive_alpha = false
		--rose_shade.h_clip_relation = h_clip_relations.REWRITE_LEVEL -- COMPARE -- REWRITE_LEVEL
		--rose_shade.level			= DEFAULT_LEVEL
AddElement(rose_shade)


local rose_ai_str           = CreateElement "ceStringPoly"
	rose_ai_str.name            = create_guid_string()
	rose_ai_str.material        = "font_JesterUI_Grey"
	rose_ai_str.parent_element = rose_shade.name
	rose_ai_str.stringdefs    = jester_stringdef_large
	rose_ai_str.init_pos		    = {0, -0.13148 * scale}
	rose_ai_str.alignment     = "CenterCenter"
	rose_ai_str.controllers 		= {{"rosetextcenter", 1}}
	rose_ai_str.isdraw = false
AddElement(rose_ai_str)


local rose_title_name           = CreateElement "ceStringPoly"
	rose_title_name.name            = create_guid_string()
	rose_title_name.material        = "font_JesterUI_Grey"
	rose_title_name.parent_element = rose_shade.name
	rose_title_name.stringdefs    = jester_stringdef
	rose_title_name.init_pos		    = {0, -0.186094 * scale}
	rose_title_name.alignment     = "CenterTop"
	rose_title_name.controllers 		= {{"rosetextcenter", 2}}
	rose_title_name.isdraw = false
AddElement(rose_title_name)



local category_size = 0.06 * scale
local positions = {1, 2, 5, 8, 7, 6, 3, 0}
for piece_num = 1,8 do

	local xpos = math.sin((piece_num-1) * math.pi * 2 / 8) * scale
	local ypos = math.cos((piece_num-1) * math.pi * 2 / 8) * scale

	local xposedgeh = math.sin((piece_num-1) * math.pi * 2 / 8 + math.pi / 8)
	local yposedgeh = math.cos((piece_num-1) * math.pi * 2 / 8 + math.pi / 8)
	local xposedgel = math.sin((piece_num-1) * math.pi * 2 / 8 - math.pi / 8)
	local yposedgel = math.cos((piece_num-1) * math.pi * 2 / 8 - math.pi / 8)


	xstart = 100 +  math.mod(positions[piece_num],3) * 300;
	ystart = 100 +  math.floor(positions[piece_num]/3) * 300;
	
	local item_anchorx = 0.615195 * xpos
	local item_anchory = 0.615195 * ypos
	
	local rose_item_origin = create_origin(create_guid_string())
		rose_item_origin.parent_element = grid_origin.name
		rose_item_origin.init_pos = {item_anchorx, item_anchory}

	local rose_str           = CreateElement "ceStringPoly"
			rose_str.name            = create_guid_string()
			rose_str.material        = "font_JesterUI_Light_Grey"
			rose_str.parent_element = rose_item_origin.name
			rose_str.stringdefs    = jester_stringdef
			rose_str.init_pos		    = {0.0, 0.0283185 * scale}
			rose_str.alignment     = "CenterTop"
			rose_str.controllers 		= {{"rosetext",piece_num, 2}}
			rose_str.isdraw = false
	AddElement(rose_str)
	
	local rose_category_str           = CreateElement "ceStringPoly"
			rose_category_str.name            = create_guid_string()
			rose_category_str.material        = "font_JesterUI_Grey"
			rose_category_str.parent_element = rose_item_origin.name
			rose_category_str.stringdefs    = jester_stringdef
			rose_category_str.init_pos		    = {0.0, -0.0205065 * scale}
			rose_category_str.alignment     = "CenterTop"
			rose_category_str.controllers 		= {{"rosecategory_str",piece_num}}
			rose_category_str.isdraw = false
	AddElement(rose_category_str)
	
	
	for cat = 1,6 do
		local rose_category_icon	= create_textured_box((cat-1)*64,0,cat*64,64,384,64)
			rose_category_icon.name 	= create_guid_string()
			rose_category_icon.material = CategoriesMat
			rose_category_icon.init_pos = {0.0, 0.076167 * scale}
			rose_category_icon.parent_element = rose_item_origin.name
			rose_category_icon.isdraw = true
			rose_category_icon.use_mipfilter = true
			rose_category_icon.additive_alpha = false
			rose_category_icon.controllers 		= {{"rosecategory",piece_num,cat}}
			rose_category_icon.vertices = { {-category_size,category_size},
									{category_size,category_size},
									{category_size,-category_size},
									{-category_size,-category_size}}
		AddElement(rose_category_icon)
	end

	local outerarrow 			= create_textured_box(0,0,512,512,512,512)
	outerarrow.name 	= create_guid_string()
	outerarrow.material = OuterTriangleMat
	outerarrow.init_pos = {xposedgeh*0.778 * scale, yposedgeh*0.778 * scale}
	outerarrow.init_rot = {180 + 22.5 - 45*piece_num,0,0}
	outerarrow.parent_element = grid_origin.name
	outerarrow.isdraw = true
	outerarrow.use_mipfilter = true
	outerarrow.additive_alpha = false
	outerarrow.controllers 		= {{"outerarrow",piece_num}}
	outerarrow.vertices = { {-0.025 * scale,0.025 * scale},
							{0.025 * scale,0.025 * scale},
							{ 0.025 * scale,-0.025 * scale},
							{-0.025 * scale,-0.025 * scale}}
	AddElement(outerarrow)

	local innerarrow 			= create_textured_box(0,0,512,512,512,512)
	innerarrow.name 	= create_guid_string()
	innerarrow.material = InnerTriangleMat
	innerarrow.init_pos = {xpos*0.305, ypos*0.305}
	innerarrow.init_rot = {-45*(piece_num-1),0,0}
	innerarrow.parent_element = grid_origin.name
	innerarrow.isdraw = true
	innerarrow.use_mipfilter = true
	innerarrow.additive_alpha = false
	innerarrow.controllers 		= {{"innerarrow",piece_num}}
	innerarrow.vertices = { {-0.025 * scale,0.025 * scale},
							{0.025 * scale,0.025 * scale},
							{ 0.025 * scale,-0.025 * scale},
							{-0.025 * scale,-0.025 * scale}}
	AddElement(innerarrow)

	
	--local rose_shade 			= create_textured_box(0,0,512,512,512,512)
	local rose_shade = CreateElement "ceTexPoly"
	rose_shade.primitivetype	= "triangles"
	rose_shade.vertices = { {0.0,0.0},
							{xposedgel * scale,yposedgel * scale},
							{xposedgeh * scale,yposedgeh * scale}}
	rose_shade.indices = {0,1,2}
	rose_shade.init_pos = {0, 0, 0}
	rose_shade.tex_coords = {{0.5,0.5},{0.5*(1.0+xposedgel),0.5*(1.0-yposedgel)},{0.5*(1.0+xposedgeh),0.5*(1.0-yposedgeh)}}	
	rose_shade.name 	= create_guid_string()
	rose_shade.material = ShadedItemMat
	rose_shade.init_pos = {0,0}
	rose_shade.parent_element = grid_origin.name
	rose_shade.isdraw = true
	rose_shade.use_mipfilter = true
	rose_shade.additive_alpha = false
	rose_shade.controllers 		= {{"disabled",piece_num}}
	AddElement(rose_shade)

	
		--local rose_shade 			= create_textured_box(0,0,512,512,512,512)
	local rose_highlight = CreateElement "ceTexPoly"
		rose_highlight.primitivetype	= "triangles"
		rose_highlight.vertices = { {0.0,0.0},
								{xposedgel * scale,yposedgel * scale},
								{xposedgeh * scale,yposedgeh * scale}}
		rose_highlight.indices = {0,1,2}
		rose_highlight.init_pos = {0, 0, 0}
		rose_highlight.tex_coords = {{0.0,0.5},{0.92387953251128675612818318939679,0.5+0.3826834323650897717284599840304},{0.92387953251128675612818318939679,0.5-0.3826834323650897717284599840304}}	
		rose_highlight.name 	= create_guid_string()
		rose_highlight.material = HighlightItemMat
		rose_highlight.init_pos = {0,0}
		rose_highlight.parent_element = grid_origin.name
		rose_highlight.isdraw = true
		rose_highlight.use_mipfilter = true
		--rose_highlight.additive_alpha = true
		--rose_highlight.blend_mode 	 	= blend_mode.IBM_REGULAR
		rose_highlight.controllers 		= {{"highlighted",piece_num}}
	AddElement(rose_highlight)

end

--0.376
MakePieNumber(1,0.0039,0.3107,-0.0762,0.0215)
MakePieNumber(2,0.2501,0.2032,-0.0762,0.0800)
MakePieNumber(3,0.3517,-0.0743,-0.0078,0.1270)
MakePieNumber(4,0.2446,-0.3321,-0.0782,0.0352)
MakePieNumber(5,-0.0156,-0.4474,-0.0762,0.0810)
MakePieNumber(6,-0.3029,-0.3322,-0.0254,0.1172)
MakePieNumber(7,-0.3986,-0.0743,-0.0078,0.1270)
MakePieNumber(8,-0.2969,0.2032,-0.0224,-0.0175)



--addVarLenStrokeLine("JesterMenuLookVector", 10, {0, 0, 0}, {0, 0, 0},  grid_origin.name, {{"lookvector"}}, true, 23, 7)

local line				= CreateElement "ceSimpleLineObject"

line.name 	= "JesterMenuLookVector"
line.material = "JESTER_VECTOR"

line.init_pos =  {0, 0, 0}
--line.h_clip_relation		= h_clip_relations.COMPARE -- this element will sit on level(.level + 1)
--line.level				= TID_DEFAULT_LEVEL
line.parent_element = grid_origin.name
line.isdraw            = true
line.isvisible			= true
line.additive_alpha = false
line.use_mipfilter = true
line.controllers = {{"lookvector"}}
line.width = 0.002


AddElement(line)
--
--
--piece_num = 3
--local shadedPiece = MakeMaterial(IndTexture_Path.."JUI_GreyOut_"..piece_num..".dds", {255, 255, 255, 255})
--local rose_shade 			= create_textured_box(0,0,512,512,512,512)
--rose_shade.name 	= create_guid_string()
--rose_shade.material = shadedPiece
--rose_shade.init_pos = {0,0}
--rose_shade.parent_element = grid_origin.name
--rose_shade.isdraw = true
--rose_shade.use_mipfilter = true
--rose_shade.additive_alpha = false
--rose_shade.h_clip_relation = h_clip_relations.REWRITE_LEVEL -- COMPARE -- REWRITE_LEVEL
--rose_shade.level			= DEFAULT_LEVEL
--AddElement(rose_shade)
--

--
--

