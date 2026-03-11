dofile(LockOn_Options.common_script_path.."elements_defs.lua")

local display_width = 0.1255		-- [m]
local pix_per_symb = 5

local pixel_size = display_width / (22 * pix_per_symb)		-- [m]

local char_w = pixel_size * pix_per_symb			-- [m]
local char_h = char_w

local char_space = pixel_size * 1.5
local line_space = pixel_size * 8

local font_KU_stringdefs = { char_h, char_w, char_space, line_space }


local function draw_base()

	local size_w_05 = display_width/2
	local size_h_05 = 0.012

	local base				= CreateElement "ceMeshPoly"
	base.name				= "base"
	base.primitivetype		= "triangles"
	base.material			= "MASK_MATERIAL_PURPLE"	-- DBG_GREY
	base.vertices			= {	{-size_w_05, -size_h_05},
								{-size_w_05,  size_h_05},
								{ size_w_05,  size_h_05},
								{ size_w_05, -size_h_05}}
	base.indices			= default_box_indices
	base.init_pos			= {0, 0}
	--base.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
	--base.level				= DEFAULT_LEVEL
	Add(base)
	return base
end

--draw_base()		-- TODO: Debug element. Delete!

function addText(name, text, controllers)
	local elem			= CreateElement "ceStringPoly"

	elem.name			= name
	elem.material		= "font_KU"
	elem.init_pos		= {-display_width/2 + 0.42*char_w, 0, 0}	-- rel. to center of indicator
	elem.alignment		= "LeftCenter"
	elem.stringdefs		= font_KU_stringdefs
	elem.value			= text
	elem.controllers	= controllers
	--elem.formats		= formats
	elem.use_mipfilter	= true
	elem.additive_alpha	= true

	Add(elem)
	return elem
end
