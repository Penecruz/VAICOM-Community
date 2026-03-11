dofile(LockOn_Options.script_path.."Displays/Common/Units.lua")

SetCustomScale(1)

function addPicture(material_name)

	local width05 = GetHalfWidth()
	local height05 = GetHalfHeight()

	local verts =	{{-width05, height05},
					 { width05, height05},
					 { width05,-height05},
					 {-width05,-height05}}
					 
	local 	  uv = {{0, 0},{1, 0},{1, 1},{0, 1}}

	local inds	= {0, 1, 2, 0, 2, 3}

	local picture				= CreateElement "ceTexPoly"
	picture.material			= MakeMaterial(material_name,{255,255,255,255})
	picture.vertices			= verts
	picture.indices				= inds
	picture.additive_alpha		= true
	picture.tex_coords			= uv
	picture.controllers			= {{"render_purpose",0}}
	Add(picture)


	local hud_only_background			= CreateElement "ceMeshPoly"
	hud_only_background.material		= MakeMaterial(nil,{0,0,0,255})
	hud_only_background.vertices		= verts
	hud_only_background.indices			= inds
	hud_only_background.additive_alpha	= false
	hud_only_background.controllers		= {{"render_purpose",1,2,3}}
	Add(hud_only_background)

	local picture_screenspace				= CreateElement "ceTexPoly"
	picture_screenspace.material			= MakeMaterial(material_name..'_SRC',{255,255,255,255})
	picture_screenspace.vertices			= verts
	picture_screenspace.indices				= inds
	picture_screenspace.tex_coords			= uv
	picture_screenspace.parent_element		= hud_only_background.name
	Add(picture_screenspace)

end

