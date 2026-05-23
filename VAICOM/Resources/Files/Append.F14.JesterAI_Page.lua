-- VAICOM PRO server-side script
-- JesterAI_Page.lua
-- www.vaicompro.com

dofile(LockOn_Options.common_script_path.."devices_defs.lua")
dofile(LockOn_Options.common_script_path.."ViewportHandling.lua")

scale = 1.0 
local jester_viewport_scale_factor = 0.70
local cx = 0 
local cy = 0 
local aspect = 1
local alpha = 0 

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

	if jester_viewport_scale_factor < 0.9999 then
		local scaled_width = viewJester.width * jester_viewport_scale_factor
		local scaled_height = viewJester.height * jester_viewport_scale_factor
		viewJester = {
			x = viewJester.x + (viewJester.width - scaled_width) * 0.5;
			y = viewJester.y + (viewJester.height - scaled_height) * 0.5;
			width = scaled_width;
			height = scaled_height;
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
dofile(LockOn_Options.script_path.."Scripts/Common/levels.lua")
dofile(LockOn_Options.common_script_path.."devices_defs.lua")

INDICATOR_LEVEL = JESTER_DEFAULT_LEVEL

dofile(LockOn_Options.script_path.."fonts.lua")
dofile(LockOn_Options.script_path.."Scripts/Common/common_defs.lua")
IndTexture_Path = LockOn_Options.script_path.."Resources/IndicationTextures/"


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
local jester_stringdef   	   = {base_font_size, 0.5844 * base_font_size, 0, 0}
local jester_stringdef_large   = {large_font_scale * base_font_size, large_font_scale * 0.5844 * base_font_size, 0, 0}
local jester_stringdef_small   = {small_font_scale * base_font_size, small_font_scale * 0.5844 * base_font_size, 0, 0}
local string_shift = base_font_size * 0.116
local string_shift_large = large_font_scale * string_shift

local grid_origin = create_origin()
grid_origin.init_pos = {cx * aspect, -cy, 0}
grid_origin.controllers = {{"show"}}


function MakePieNumber(index,pos_x,pos_y,press_x,press_y)
	local pie_origin = create_origin(create_guid_string())
		pie_origin.parent_element = grid_origin.name
		pie_origin.init_pos = {pos_x * scale, pos_y * scale}
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
end

local SHOW_MASKS=  false

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
			rose_category_str.init_pos		= {0.0, -0.0205065 * scale}
			rose_category_str.alignment     = "CenterTop"
			rose_category_str.controllers 		= {{"rosecategory_str",piece_num}}
			rose_category_str.isdraw = false
	AddElement(rose_category_str)
	
end

MakePieNumber(1,0.0039,0.3107,-0.0762,0.0215)
MakePieNumber(2,0.2501,0.2032,-0.0762,0.0800)
MakePieNumber(3,0.3517,-0.0743,-0.0078,0.1270)
MakePieNumber(4,0.2446,-0.3321,-0.0782,0.0352)
MakePieNumber(5,-0.0156,-0.4474,-0.0762,0.0810)
MakePieNumber(6,-0.3029,-0.3322,-0.0254,0.1172)
MakePieNumber(7,-0.3986,-0.0743,-0.0078,0.1270)
MakePieNumber(8,-0.2969,0.2032,-0.0224,-0.0175)