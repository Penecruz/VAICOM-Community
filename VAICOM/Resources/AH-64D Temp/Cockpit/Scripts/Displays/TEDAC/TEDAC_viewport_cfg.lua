--ViewportHandling
dofile(LockOn_Options.common_script_path.."ViewportHandling.lua")
----------------------------------------------------------------------
local w = LockOn_Options.screen.width;
local h = LockOn_Options.screen.height;

if LockOn_Options.screen.oculus_rift then 
	local ui_x,ui_y,ui_w,ui_h = get_UIMainView()
	w = ui_w;
	h = ui_h;
end

local size  = 0.5 * h
if size > h then
	size = h
end
if size > 0.33 * w then
	size = 0.33 * w
end

dedicated_viewport			= {0.5 * w - 0.5 * size, h - size, size, size}
dedicated_viewport_arcade	= dedicated_viewport
----------------------------------------------------------------------
if not try_find_assigned_viewport("TEDAC") then
	   try_find_assigned_viewport("CENTER_MFCD")
end