dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

AddBackground("Background_Mask", true) -- clipping mask
addMesh(nil,  buildBoxVerts( display_size_pix, display_size_pix , "CenterCenter"), default_box_indices, {0,0},"triangles", nil,nil, "MFD_BACKGROUND")
addVideoSignal()

local width = 10
local verts = {{-display_size_pix_05, display_size_pix_05 - width/2}, {display_size_pix_05, display_size_pix_05 - width/2}}
draw_line( verts, IND_MPD_MATERIAL_WHITE, nil, width, nil,  {{"MFD_SingleDP_Line", 1}}, nil, nil ) --h_clip_relation, level
local verts = {{-display_size_pix_05, -display_size_pix_05 + width/2}, {display_size_pix_05, -display_size_pix_05 + width/2}}
draw_line( verts, IND_MPD_MATERIAL_WHITE, nil, width, nil,  {{"MFD_SingleDP_Line", 0}}, nil, nil) --h_clip_relation, level

add_TADS_LOS_Reticle("TadsReticle", {0,0}, nil, {{"VIDEO_TadsReticleVisible"}} )
