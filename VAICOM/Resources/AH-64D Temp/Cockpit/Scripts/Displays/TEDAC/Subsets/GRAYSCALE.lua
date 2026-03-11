dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")


for i = 0,9,1 do
	local color = 120 + i * 15;
	local mat = MakeMaterial(nil, {color, color, color, 255})
	local h = (display_size_pix / ScaleZoom / DisplaySizeCorr) / 11.0
	local y = h * 5 - h * i
	local gs_bar = addBox("GrayScaleHorizontalBar_"..i, (display_size_pix / ScaleZoom / DisplaySizeCorr) / 2, h, "CenterTop", {0,y}, nil, {{"TEDAC_GRAYSCALE_Brightness", color/255.0}}, mat)
	setClipLevel(gs_bar, -1)
end
