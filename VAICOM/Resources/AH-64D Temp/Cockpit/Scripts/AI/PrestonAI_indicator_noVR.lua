dofile(LockOn_Options.common_script_path.."devices_defs.lua")

indicator_type       = indicator_types.COMMON
purposes 	 = {render_purpose.HUD_ONLY_VIEW,
				render_purpose.SCREENSPACE_INSIDE_COCKPIT,
				render_purpose.SCREENSPACE_OUTSIDE_COCKPIT,
				render_purpose.AUXILLARY_SIGHT_SCREENSPACE,
}
screenspace_scale    = 4;

-------PAGE IDs-------
id_Page =
{
	MAIN = 0,
}

id_pagesubset =
{
	COMMON   = 0,
}

page_subsets = {}
page_subsets[id_pagesubset.COMMON] = LockOn_Options.script_path.."AI\\PrestonAI_page_noVR.lua"
  	
----------------------
pages = {}
pages[id_Page.MAIN] = { id_pagesubset.COMMON}
init_pageID     = id_Page.MAIN