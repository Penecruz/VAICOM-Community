dofile(LockOn_Options.common_script_path.."devices_defs.lua")

indicator_type       = indicator_types.HELMET
purposes 	 		 = {render_purpose.GENERAL_AFTER_POSTEFFECTS, render_purpose.HUD_ONLY_VIEW_AFTER_POSTEFFECTS, render_purpose.AUXILLARY_SIGHT}

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
page_subsets[id_pagesubset.COMMON] = LockOn_Options.script_path.."AI\\PrestonAI_page_VR.lua"
  	
----------------------
pages = {}
pages[id_Page.MAIN] = { id_pagesubset.COMMON}
init_pageID     = id_Page.MAIN