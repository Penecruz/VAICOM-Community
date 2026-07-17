dofile(LockOn_Options.script_path.."paths.lua")
dofile(LockOn_Options.common_script_path.."devices_defs.lua")
dofile(base_script_path.."materials.lua")

indicator_type       = indicator_types.COMMON
purposes 	 = {render_purpose.SCREENSPACE_INSIDE_COCKPIT,
                render_purpose.SCREENSPACE_OUTSIDE_COCKPIT,
                render_purpose.HUD_ONLY_VIEW,
                render_purpose.GENERAL_AFTER_POSTEFFECTS,
                render_purpose.HUD_ONLY_VIEW_AFTER_POSTEFFECTS}
screenspace_scale    = 4;

-------PAGE IDs-------
id_Page =
{
	MAIN = 0,
}

id_pagesubset =
{
	COMMON         = 0,
	CONTEXT_CURSOR = 1,
	SUBTITLE       = 2,
}

page_subsets = {}
page_subsets[id_pagesubset.COMMON]         = base_script_path.."Scripts/JesterAI/JesterAI_Page.lua"
page_subsets[id_pagesubset.CONTEXT_CURSOR] = base_script_path.."Scripts/JesterAI/JesterContextCursor_Page.lua"
page_subsets[id_pagesubset.SUBTITLE]       = base_script_path.."Scripts/JesterAI/JesterSubtitle_Page.lua"

----------------------
pages = {}
pages[id_Page.MAIN] = { id_pagesubset.COMMON, id_pagesubset.CONTEXT_CURSOR, id_pagesubset.SUBTITLE }
init_pageID     = id_Page.MAIN


opacity_sensitive_materials =
{
"font_ROSE",
}

color_sensitive_materials =
{
"font_ROSE",
}

brightness_sensitive_materials =
{
"font_ROSE",
}

-- USE VIEWPORT: F14_JESTER_MENU
