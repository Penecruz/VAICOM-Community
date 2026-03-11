
dofile(LockOn_Options.common_script_path.."devices_defs.lua")
dofile(LockOn_Options.common_script_path.."ViewportHandling.lua")

indicator_type		= indicator_types.COMMON
purposes			= { render_purpose.GENERAL, render_purpose.HUD_ONLY_VIEW }

pages			= {{1}}
init_pageID		= 1

function declareMFD(left)

	local viewport				= make_viewport(1,left)
	dedicated_viewport			= viewport
	dedicated_viewport_arcade	= viewport

	if left then
		page_subsets	= {LockOn_Options.script_path.."Displays/MFD/indicator/LCD/page_plt_left.lua"}
		try_find_assigned_viewport("LEFT_MFCD")
	else
		try_find_assigned_viewport("RIGHT_MFCD")
		page_subsets	= {LockOn_Options.script_path.."Displays/MFD/indicator/LCD/page_plt_right.lua"}
	end

end

