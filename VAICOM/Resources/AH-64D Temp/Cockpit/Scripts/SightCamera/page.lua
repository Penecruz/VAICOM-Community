dofile(LockOn_Options.common_script_path.."elements_defs.lua")
SetScale(FOV)

picture					 = CreateElement "ceTexPoly"
picture.name			 = "picture"
picture.vertices		 = {{-1, 1},
							{ 1, 1},
							{ 1,-1},
							{-1,-1}}
picture.indices			 = {0, 1, 2, 0, 2, 3}
picture.tex_coords		 = {{0, 0},
                            {1, 0},
                            {1, 1},
                            {0, 1}}
picture.material		 = "render_target_1"
Add(picture)



local halfFOV = 1000* math.tan(math.rad(20)/2)

local radius = (132/2)/halfFOV
local delta = math.rad(10)
verts = {}
for i = 0,36,1 do
	verts[i + 1]    = { radius * math.sin(delta * i),36/24 * radius * math.cos(delta * i) }
end  

local sz = 0.01
local defs = {sz, 0.68 * sz, 0.0, 0.0}

frame_time					 	= CreateElement "ceStringPoly"
frame_time.name					= "frame_time"
frame_time.alignment			= "LeftBottom"
frame_time.material				= "font_SightCamera"
frame_time.init_pos 			= {-0.97,-1,0}
frame_time.stringdefs			= defs
frame_time.use_mipfilter 		= false
frame_time.screenspace			= ScreenType.SCREENSPACE_VOLUME
frame_time.controllers			= {{"frame_time"}}
frame_time.additive_alpha		= true
Add(frame_time)

frame_number					 	= CreateElement "ceStringPoly"
frame_number.name					= "frame_number"
frame_number.alignment				= "RightBottom"
frame_number.material				= "font_SightCamera"
frame_number.init_pos 				= { 0.97,-1,0}
frame_number.stringdefs				= defs
frame_number.use_mipfilter 			= false
frame_number.screenspace			= ScreenType.SCREENSPACE_VOLUME
frame_number.controllers			= {{"frame_number"}}
frame_number.additive_alpha			= true
Add(frame_number)

-- sensitivity
local shadow_mat		= MakeMaterial("", {0, 0, 0, 100})

shadow					= CreateElement "ceMeshPoly"
shadow.name				= "shadow"
shadow.vertices			= {{-1, 1},
						   { 1, 1},
						   { 1,-1},
						   {-1,-1}}
shadow.indices			= {0, 1, 2, 0, 2, 3}
shadow.material			= shadow_mat
shadow.controllers		= {{"shadow"}}
Add(shadow)

