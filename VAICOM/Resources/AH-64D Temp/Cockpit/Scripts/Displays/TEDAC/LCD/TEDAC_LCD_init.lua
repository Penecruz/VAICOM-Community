dofile(LockOn_Options.common_script_path.."devices_defs.lua")

indicator_type		= indicator_types.COMMON
purposes			= {render_purpose.GENERAL}

pages			= {{1}}
init_pageID		= 1

page_subsets	= {LockOn_Options.script_path.."Displays/TEDAC/LCD/page.lua"}

dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_viewport_cfg.lua")
