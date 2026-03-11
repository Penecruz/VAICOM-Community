dofile(LockOn_Options.script_path.."Displays/DSPLS/DSPLS_Symbology.lua")

local Video_W = DegToDI(40.0)
local Video_H = DegToDI(30.0)
local line_mat = "HMD_GREEN"
local box_side = Video_H
local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0, 0}, nil, {{"VideoAreaPH"}, {"VideoSignal_Show"}})
local color = 3 

local mat = MakeMaterial(nil, {0, 0, 0, 0})
local gs_box = addBox(nil, box_side, box_side, "CenterCenter", {0,0}, VideoAreaPH.name, nil, mat)	
local h = box_side / 10.0


local verts_border =	{ {-box_side/2, -box_side/2}, {-box_side/2, box_side/2}, {box_side/2, box_side/2}, { box_side/2,-box_side/2 }, {-box_side/2, -box_side/2} }
	addStrokeLine(nil, verts_border, THICKNESS_WIDE, FUZZINESS_WIDE, VideoAreaPH.name)
local verts_hor =	{ {-box_side/2, 0}, {box_side/2, 0} }
	addStrokeLine(nil, verts_hor, THICKNESS_WIDE, FUZZINESS_WIDE, VideoAreaPH.name)

local delta = box_side / 70.0
local x = -1.5*delta
for i = 0,3,1 do
	addStrokeLine(nil, { {x, -box_side/2}, {x, box_side/2} }, THICKNESS_WIDE, FUZZINESS_WIDE, VideoAreaPH.name)	
	x = x + delta
end

for i = 0,9,1 do
	local gain = i / 9.0;
	color = 255 * (1.0 - gain);
	mat = MakeMaterial(nil, {0, color, 0, 100})
	
	local bar_w = box_side/4
	local y = -box_side/2 + h * i
	local gs_bar_l 		= addBox(nil, bar_w, h, "CenterBottom", {-bar_w,y}, VideoAreaPH.name, {{"DSPLS_GRAYSCALE_Brightness", color/255.0}}, mat)
	local gs_bar_h 		= addBox(nil, bar_w, h, "CenterBottom", { bar_w,y}, VideoAreaPH.name, {{"DSPLS_GRAYSCALE_Brightness", color/255.0}}, mat)
end

