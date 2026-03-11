dofile(LockOn_Options.common_script_path.."elements_defs.lua")
dofile(LockOn_Options.script_path.."Displays/EUFD/EUFD_Tools.lua")

SetScale(FOV)

local sc = GetScale()

local char_w = sc * 0.045
local char_h = sc * 0.05
local char_space = sc * 0.005
local line_space = sc * 0.03

EUFD_stringdefs = { char_h, char_w, char_space, line_space }

local char_h_vert_lines = sc * 0.085
EUFD_stringdefs_vert_lines = { char_h_vert_lines, char_w, char_space, line_space }

local char_h_vert_lines_ = sc * 0.0685
EUFD_stringdefs_vert_lines_ = { char_h_vert_lines_, char_w, char_space, line_space }

local char_w_hor_lines = sc * 0.05351
local char_space_hor_lines = sc * -0.00225
EUFD_stringdefs_hor_lines = { char_h, char_w_hor_lines, char_space_hor_lines, line_space }

Material_		= readParameter("EUFD_Material")

function AddText(Name, Xpos, Ypos, Controllers, Value, Stringdefs)
	local symb			= CreateElement "ceStringPoly"
	symb.name			= Name
	symb.material		= Material_
	if Stringdefs ~= nil then
		symb.stringdefs	= Stringdefs
	else
		symb.stringdefs	= EUFD_stringdefs
	end
	symb.alignment		= "LeftTop"
	symb.init_pos		= {Xpos, Ypos, 0.0}	
	if Controllers ~= nil then
		symb.controllers	= Controllers
	end	
	if Value ~= nil then
		symb.value		= Value
	end	
	symb.parent_element = "background"
	symb.additive_alpha = true
	symb.use_mipfilter = true
	Add(symb)
end

function AddTextTable(Name, Xpos, Ypos, Controllers, Value, Formats)
	local elem			= CreateElement "ceStringPoly"
	elem.name			= Name
	elem.material		= Material_
	elem.init_pos		= {Xpos, Ypos, 0.0}
	elem.alignment		= "LeftTop"
	elem.stringdefs		= EUFD_stringdefs
	elem.value			= Value
	elem.controllers	= Controllers
	elem.formats		= Formats
	elem.parent_element	= "background"
	elem.use_mipfilter	= true
	elem.additive_alpha	= true
	Add(elem)
end