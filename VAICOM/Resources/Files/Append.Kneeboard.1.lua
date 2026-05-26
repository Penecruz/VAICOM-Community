-- VAICOM PRO server-side script
-- 1.lua
-- www.vaicompro.com

dofile(LockOn_Options.common_script_path.."KNEEBOARD/indicator/definitions.lua")
dofile(LockOn_Options.common_script_path.."VAICOMPRO/device/VAICOMPRO_Common.lua")

add_text     = function(text,UL_X,UL_Y,A,S)

local UL_X = UL_X or 0
local UL_Y = UL_Y or 0

local default_char_height  = 0.008
local default_char_width   = 0.3 * default_char_height
local txt 	    					= CreateElement "ceStringPoly"
	  txt.value 					= text
	  txt.material 					= MakeFont({used_DXUnicodeFontData = "font_dejavu_lgc_sans_22"},{0,0,0,A or 255})
	  txt.stringdefs				= S or Font_med
	  txt.init_pos					= {UL_X - 1,GetAspect() - UL_Y}
	  txt.alignment					= "LeftTop"
	  txt.use_mipfilter				= true
	  txt.h_clip_relation   		= h_clip_relations.COMPARE
	  txt.level 		    		= DEFAULT_LEVEL	
	  Add(txt)
end
texture_box_ = function(UL_X,UL_Y,W,H)
	local UL_X = UL_X or 0
	local UL_Y = UL_Y or 0
	local W    = W    or 1
	local H    = H    or 1
	return  {{UL_X	   , UL_Y},
			{(UL_X + W), UL_Y},
			{(UL_X + W),(UL_Y + H)},
			{ UL_X     ,(UL_Y + H)}}
end
add_picture  = function(picture,UL_X,UL_Y,W,H,tx_ULX,tx_ULY,tx_W,tx_H,A,name,params,controllers)

local USER_PICTURE  = MakeMaterial(picture,{255,255,255,A or 255})

local width 		= W
local height		= H

if width == nil then
   width = 2
end

if height == nil then
   height =  2*GetAspect()
end
local UL_X  = UL_X or 0
local UL_Y  = UL_Y or 0

local back   	    = CreateElement "ceTexPoly"
back.name			= name
back.material 	    = USER_PICTURE
back.init_pos   	= {UL_X - 1,GetAspect() - UL_Y}
back.vertices   	= {{0	 ,0},
					    {width   ,0},
					    {width   ,-height},
					    {0       ,-height}}
back.indices	  	= {0,1,2;0,2,3}
back.tex_coords   	= texture_box_(tx_ULX,tx_ULY,tx_W,tx_H)
back.h_clip_relation= h_clip_relations.COMPARE
back.level 		    = DEFAULT_LEVEL	

if params ~= nil then
	back.element_params = params
end

if controllers ~= nil then
	back.controllers = controllers
end

Add(back)

return back

end

SetScale(FOV)

function AddElement(object)
    object.use_mipfilter = true
    Add(object)
end

local path_textures = LockOn_Options.common_script_path.."VAICOMPRO/indicator/Textures/"

add_picture(path_textures.."Notepad.png",nil,nil,nil,nil,nil,nil,nil,nil,255,"BG_Paper",{"Master_Opacity"},{{"opacity_using_parameter",0}} )
add_picture(path_textures.."Notepad_watermark.png",0.4,0.9,1.2,1.2,nil,nil,nil,nil,10,"Watermark",{"Master_Opacity"},{{"opacity_using_parameter",0}} )

init_logcats()
for i,j in pairs(logcats) do
	add_picture(path_textures.."Tabs_"..j..".png", 0,0.02,2,2*1/20*GetAspect(),nil,nil,nil,nil,255,"Tabs_"..j, {"Page_"..j}, {{"opacity_using_parameter",0}} )
end

local vaicom_font    		= MakeFont({used_DXUnicodeFontData = "font_dejavu_lgc_sans_22"},{60,60,60,255}) 
local vaicom_font_greyed    = MakeFont({used_DXUnicodeFontData = "font_dejavu_lgc_sans_22"},{30,30,30,0.5*255})
local vaicom_font_writing   = MakeFont({used_DXUnicodeFontData = "font_dejavu_lgc_sans_22"},{18,18,118,255})
local vaicom_font_print    	= MakeFont({used_DXUnicodeFontData = "font_cockpit_usa"},{40,40,40,255})

local margin_left   = 0.10
local margin_right  = 0.10
local margin_top    = 0.23
local margin_bottom = 0.08
local line_dist_s   = 0.15
local tab_spacing 	= 0.20

local font_scale_tabs   = 0.675
local font_scale_xxs    = 0.5
local font_scale_xs     = 0.7
local font_scale_small  = 0.6
local font_scale_medium = 1.0
local font_scale_large  = 1.2

local SizeX	= 0.0075 * 0.25
local SizeY	= 0.0075

Font_xxs 	= { SizeY * font_scale_xxs,    SizeX * font_scale_xxs,    0.0, 0.0}
Font_xs 	= { SizeY * font_scale_xs,     SizeX * font_scale_xs,     0.0, 0.0}
Font_xs_pr  = { SizeY * font_scale_small,2*SizeX * font_scale_small,  0.0, 0.0}
Font_med    = { SizeY * font_scale_medium, SizeX * font_scale_medium, 0.0, 0.0}

local bottompos = function(i)
	local ypos =  GetAspect()*(-1 + margin_bottom) + (i * line_dist_s)
	return ypos
end
local linepos = function(i)
	local ypos =  GetAspect()*(1 - margin_top) - (i * line_dist_s)
	return ypos
end
local leftpos = function(i)
	local xpos =  -1 + margin_left + (i * tab_spacing)
	return xpos
end
local rightpos = function(i)
	local xpos =  1 - margin_left - (i * tab_spacing)
	return xpos
end

init_headers()
Headers = {}
local setheaderproperties = function(element,name,margin,pos,loc,align)
	element.name			= "Header_"..name
	element.init_pos		= {margin == "R" and rightpos(pos) or leftpos(pos), loc == "B" and bottompos(0) or linepos(0) , 0}
	element.material		= vaicom_font
	element.formats			= {"%s"}
	element.controllers     = name == "TopMid" and {{"opacity_using_parameter",0},{"text_using_parameter",1,0}, {"change_color_when_parameter_equal_to_number", 2, 1, 1,0,0}} or {{"opacity_using_parameter",0},{"text_using_parameter",1,0}}
	element.alignment		= align
	element.stringdefs		= Font_xs
	element.element_params  = name == "TopMid" and {"Master_Opacity", "Header_TopMid", "Dictate_Status"} or {"Master_Opacity", "Header_"..name}
end
for i,j in pairs(headers) do
	Headers[i] = CreateElement "ceStringPoly"
	setheaderproperties(Headers[i],j[1],j[2],j[3],j[4],j[5])	
	AddElement(Headers[i])
end

init_logcats()

Unitsdata = {}
Logdata   = {}
Aliasdata = {}

local setblockproperties = function(element,section,cat,pos)
	element.name			= "Page_"..section.."_"..cat
	element.init_pos		= {leftpos(0), linepos(pos) , 0}	
	element.formats			= {"%s"}
	element.controllers     = {{"opacity_using_parameter",0},{"text_using_parameter",1,0},{"rotate_using_parameter",2,1}}
	element.alignment		= "LeftTop"
	element.element_params  = {"Page_"..cat, "Page_"..section.."_"..cat, "Global_angle"}		
	if section == "Unitsdata" or cat == "LOG" then
		element.controllers     = {{"opacity_using_parameter",0},{"text_using_parameter",1,0}}
		element.stringdefs		= Font_xs_pr
		element.material		= vaicom_font_print 
	else
		element.controllers     = {{"opacity_using_parameter",0},{"text_using_parameter",1,0},{"rotate_using_parameter",2,1}}
		element.stringdefs		= Font_xs
		element.material		= vaicom_font_writing
	end
end

local setblockproperties_alias = function(element,section,cat,pos)
	element.name			= "Page_"..section.."_"..cat
	element.init_pos		= {rightpos(0), linepos(0.50+4.23*pos) , 0}
	element.material		= vaicom_font_greyed
	element.formats			= {"%s"}
	element.controllers     = {{"opacity_using_parameter",0},{"text_using_parameter",1,0}}
	element.alignment		= "RightTop"
	element.stringdefs		= Font_xxs
	element.element_params  = {"Page_"..cat, "Page_"..section.."_"..cat, "Global_angle"}	
end

for k = 1,4 do
	Logdata[k]   = {}
	Aliasdata[k] = {}
end

for i,j in pairs(logcats) do
	Unitsdata[i] = CreateElement "ceStringPoly"
	setblockproperties(Unitsdata[i],"Unitsdata",i,0.6)	
	AddElement(Unitsdata[i])
  local atc_log_start_offset = i == "ATC" and 1.95 or 0
	for k = 1,4 do
		Logdata[k][i] = CreateElement "ceStringPoly"
        setblockproperties(Logdata[k][i],"Logdata"..tostring(k),i,2.9+atc_log_start_offset+(k-1)*4.2)
		AddElement(Logdata[k][i])
		Aliasdata[k][i]	= CreateElement "ceStringPoly"
		setblockproperties_alias(Aliasdata[k][i],"Aliasdata"..tostring(k),i,k-1)
		AddElement(Aliasdata[k][i])
	end
end

add_picture(path_textures.."Notepad_smudge.png",nil,nil,nil,nil,nil,nil,nil,nil,255,"BG_Paper_Speckles",{"Smudge_Opacity"},{{"opacity_using_parameter",0}} )





		