dofile(LockOn_Options.common_script_path.."devices_defs.lua")

opacity_sensitive_materials = {}
color_sensitive_materials = {}


-- Specific part of MFD initialization
-- Parameters handling functions
indicator_type		= indicator_types.COMMON
--purposes			= {render_purpose.GENERAL, render_purpose.HUD_ONLY_VIEW}
purposes			= {render_purpose.BAKE}


-- page specific for the indicator, implements indicator border/FOV
MAP_SUBSET = 1

page_subsets = {}
page_subsets[MAP_SUBSET] = LockOn_Options.script_path.."Cameras/MAP/MAP_CAM_subset.lua"

MAP_PAGE = 1

pages = {}
pages[MAP_PAGE] = {MAP_SUBSET}

BasePage	= LockOn_Options.script_path.."Cameras/MAP/MAP_CAM_subset.lua"
init_pageID	= MAP_PAGE

dynamically_update_geometry = false
