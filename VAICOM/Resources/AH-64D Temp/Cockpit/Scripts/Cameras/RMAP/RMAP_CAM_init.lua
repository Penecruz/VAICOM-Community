dofile(LockOn_Options.common_script_path.."devices_defs.lua")

opacity_sensitive_materials = {}
color_sensitive_materials = {}


-- Parameters handling functions
indicator_type		= indicator_types.COMMON
--purposes			= {render_purpose.GENERAL, render_purpose.HUD_ONLY_VIEW}
purposes			= {render_purpose.BAKE}


-- page specific for the indicator, implements indicator border/FOV
RMAP_SUBSET = 1

page_subsets = {}
page_subsets[RMAP_SUBSET] = LockOn_Options.script_path.."Cameras/RMAP/RMAP_CAM_subset.lua"

RMAP_PAGE = 1

pages = {}
pages[RMAP_PAGE] = {RMAP_SUBSET}

BasePage	= LockOn_Options.script_path.."Cameras/RMAP/RMAP_CAM_subset.lua"
init_pageID	= RMAP_PAGE

dynamically_update_geometry = false
