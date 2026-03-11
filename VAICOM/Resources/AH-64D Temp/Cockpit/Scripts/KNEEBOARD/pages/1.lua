dofile(LockOn_Options.script_path.."KNEEBOARD/common.lua")

add_picture("kneeboard_notepad_background.dds")

local gettext = require("i_18n")
_ = gettext.translate

--
local default_material = font_hints_kneeboard
local noParent = nil
local noControllers = nil

function AddElement(object)
	object.use_mipfilter = true
	Add(object)
end

HintsBase					= CreateElement "ceSimple"
HintsBase.name				= "HintsBase"
HintsBase.init_pos			= {0, GetAspect() - 3.5 * 0.0075}
AddElement(HintsBase)

-- fonts
FontSizeY1	= 0.0075
FontSizeX1	= FontSizeY1 * 0.64

predefined_font_title	= {FontSizeY1,			FontSizeX1,			0.0005,		0.0}
predefined_font_item	= {FontSizeY1 * 0.75,	FontSizeX1 * 0.75,	0.00025,	0.0}
predefined_font_key		= {FontSizeY1 * 0.75,	FontSizeX1 * 0.75,	0.00025,	0.0}

--
local function AddText(name, value, pos, align, stringdef, parent, controllers, material, formats)
	local txt		= CreateElement "ceStringPoly"
	txt.name		= name
	txt.material	= material or default_material
	txt.value		= value
	txt.stringdefs	= stringdef

	if align ~= nil then
		txt.alignment = align
	else
		txt.alignment = "CenterCenter"
	end

	local pos_		= pos or {0, 0}
	txt.init_pos	= {pos_[1], pos_[2], 0}

	if parent ~= nil then
		txt.parent_element = parent
	end
	if controllers ~= nil then
		if type(controllers) == "table" then
			txt.controllers = controllers
		end
	end

	txt.formats		= formats

	AddElement(txt)
	return txt
end


-- lines
local FirstLineY	= 1.167
local LineSizeY		= 0.092

local function getLineY(line)
	return FirstLineY - LineSizeY * (line)
end

------
local Flare_pos_x					= 0.20
local FlareBurstCount_pos_y			= getLineY(1)
local FlareBurstInterval_pos_y		= getLineY(2)
local FlareSalvoCount_pos_y			= getLineY(3)
local FlareSalvoInterval_pos_y		= getLineY(4)
local FlareProgramDelay_pos_y		= getLineY(5)

local KeyPosX = 0.85

AddText("Title_FlareInfo", _("CMWS FLARE"), {0, getLineY(0)}, "CenterBottom", predefined_font_title, noParent, noControllers, font_hints_kneeboard)

AddText("Name_FlareBurstCount", 		_("FLARE BURST COUNT - _board"),		{Flare_pos_x,	FlareBurstCount_pos_y},		"RightBottom",	predefined_font_item,	noParent,	noControllers,						font_hints_kneeboard)
AddText("Status_FlareBurstCount", 		"0",									{Flare_pos_x,	FlareBurstCount_pos_y},		"LeftBottom",	predefined_font_item,	noParent,	{{"Kneeboard_FlareBurstCount"}},	font_hints_kneeboard, {"1", "2", "3", "4", "6", "8"})
AddText("Key_FlareBurstCount", 			_("RS+RA+[1]_board"),					{KeyPosX,		FlareBurstCount_pos_y},		"RightBottom",	predefined_font_key,	noParent,	noControllers,						font_general_keys)

AddText("Name_FlareBurstInterval", 		_("FLARE BURST INTERVAL - _board"),		{Flare_pos_x,	FlareBurstInterval_pos_y},	"RightBottom",	predefined_font_item,	noParent,	noControllers,						font_hints_kneeboard)
AddText("Status_FlareBurstInterval", 	"0",									{Flare_pos_x,	FlareBurstInterval_pos_y},	"LeftBottom",	predefined_font_item,	noParent,	{{"Kneeboard_FlareBurstInterval"}},	font_hints_kneeboard, {"0.1", "0.2", "0.3", "0.4"})
AddText("Key_FlareBurstInterval", 		_("RS+RA+[2]_board"),					{KeyPosX,		FlareBurstInterval_pos_y},	"RightBottom",	predefined_font_key,	noParent,	noControllers,						font_general_keys)


AddText("Name_FlareSalvoCount", 		_("FLARE SALVO COUNT - _board"),		{Flare_pos_x,	FlareSalvoCount_pos_y},		"RightBottom",	predefined_font_item,	noParent,	noControllers,						font_hints_kneeboard)
AddText("Status_FlareSalvoCount", 		"0",									{Flare_pos_x,	FlareSalvoCount_pos_y},		"LeftBottom",	predefined_font_item,	noParent,	{{"Kneeboard_FlareSalvoCount"}},	font_hints_kneeboard, {"1", "2", "4", "8", "CONT"})
AddText("Key_FlareSalvoCount", 			_("RS+RA+[3]_board"),					{KeyPosX,		FlareSalvoCount_pos_y},		"RightBottom",	predefined_font_key,	noParent,	noControllers,						font_general_keys)


AddText("Name_FlareSalvoInterval", 		_("FLARE SALVO INTERVAL - _board"),		{Flare_pos_x,	FlareSalvoInterval_pos_y},	"RightBottom",	predefined_font_item,	noParent,	noControllers,						font_hints_kneeboard)
AddText("Status_FlareSalvoInterval", 	"0",									{Flare_pos_x,	FlareSalvoInterval_pos_y},	"LeftBottom",	predefined_font_item,	noParent,	{{"Kneeboard_FlareSalvoInterval"}},	font_hints_kneeboard, {"1", "2", "3", "4", "5", "8", "RAND"})
AddText("Key_FlareSalvoInterval", 		_("RS+RA+[4]_board"),					{KeyPosX,		FlareSalvoInterval_pos_y},	"RightBottom",	predefined_font_key,	noParent,	noControllers,						font_general_keys)


AddText("Name_FlareProgramDelay", 		_("MIN TIME BETWEEN PRGMS - _board"),	{Flare_pos_x,	FlareProgramDelay_pos_y},	"RightBottom",	predefined_font_item,	noParent,	noControllers,						font_hints_kneeboard)
AddText("Status_FlareProgramDelay", 	"0",									{Flare_pos_x,	FlareProgramDelay_pos_y},	"LeftBottom",	predefined_font_item,	noParent,	{{"Kneeboard_FlareProgramDelay"}},	font_hints_kneeboard, {"1", "2", "3", "4"})
AddText("Key_FlareProgramDelay", 		_("RS+RA+[5]_board"),					{KeyPosX,		FlareProgramDelay_pos_y},	"RightBottom",	predefined_font_key,	noParent,	noControllers,						font_general_keys)


