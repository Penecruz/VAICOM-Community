dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local GS_PH = addPlaceholder(nil, {0, 0, 0}, nil, nil ) 

local bar_w = video_area_w/64.0
local bar_h = video_area_h /8.0

for i = 0,63,1 do
	local gain = i / 63.0;
	color = 255 * (1.0 - gain);
	mat = MakeMaterial(nil, {color, color, color, 255})
	local x = bar_w*i - video_area_w_05
	addBox(nil, bar_w, bar_h, "LeftTop",   { x, video_area_h_05}, 	GS_PH.name, nil, mat)
	addBox(nil, bar_w, bar_h, "LeftBottom",{ x, 0-video_area_h_05},	GS_PH.name, nil, mat)
end

local Controls = {}
Controls = 
{
	{ pb.B6, "G/S", tp_default_border, nil, nil }
}
createControls( Controls )