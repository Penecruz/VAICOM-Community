dofile(LockOn_Options.script_path.."Displays/Common/Units.lua")

SetCustomScale(1)


local display_size_inch		= 5.0
local display_size05		= display_size_inch * InToMeter * 0.5

local verts	=				{{-display_size05, display_size05},
							 { display_size05, display_size05},
							 { display_size05,-display_size05},
							 {-display_size05,-display_size05}}

local inds	= {0, 1, 2, 0, 2, 3}


local night_green_color = { r = 0.0196, g = 0.753, b = 0.0 }

local color_control = {"TEDAC_Color", night_green_color.r, night_green_color.g, night_green_color.b}

local picture			= CreateElement "ceTexPoly"
picture.material		= MakeMaterial("TEDAC_LCD_AH64",{255,255,255,255})
picture.vertices		= verts
picture.indices			= inds
picture.additive_alpha	= true
picture.tex_coords		= {{0, 0},{1, 0},{1, 1},{0, 1}}
picture.controllers		= {{"render_purpose",0}, color_control}
Add(picture)


local hud_only_background		= CreateElement "ceMeshPoly"
hud_only_background.material	= MakeMaterial("",{0,0,0,255})
hud_only_background.vertices	= verts
hud_only_background.indices		= inds
hud_only_background.controllers	= {{"render_purpose",1,2,3}}
Add(hud_only_background)

local picture_screenspace				= CreateElement "ceTexPoly"
picture_screenspace.material			= MakeMaterial('TEDAC_LCD_AH64_SRC',{255,255,255,255})
picture_screenspace.vertices			= verts
picture_screenspace.indices				= inds
picture_screenspace.tex_coords			= {{0, 0},{1, 0},{1, 1},{0, 1}}
picture_screenspace.parent_element		= hud_only_background.name
picture_screenspace.controllers			= {color_control}
Add(picture_screenspace)


