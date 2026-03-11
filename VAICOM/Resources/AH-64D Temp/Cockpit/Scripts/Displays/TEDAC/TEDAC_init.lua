dofile(LockOn_Options.common_script_path.."devices_defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/device/MFD_device_IDs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Tools.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Opacity_Materials.lua")

opacity_sensitive_materials = 
{
	"font_TEDAC",
	"font_TEDAC_BLACK",
	"font_TEDAC_big",
	"font_TEDAC_big_BLACK",
	"TEDAC_SYMBOLOGY",
	"TEDAC_SYMBOLOGY_BLACK",
	"font_TEDAC_FCR_Target_symbol",
	"font_TEDAC_FCR_Target_symbol_black",
	"TEDAC_FCR_INDICATION_WHITE",
	"TEDAC_FCR_INDICATION_BLACK",
}

color_sensitive_materials = 
{
	"font_TEDAC",
	"font_TEDAC_big",
	"TEDAC_SYMBOLOGY",
	"TEDAC_FCR_INDICATION_WHITE",
	"TEDAC_FCR_INDICATION_BLACK",
}

brightness_sensitive_materials = 
{
	"font_TEDAC",
	"font_TEDAC_big",
	"TEDAC_SYMBOLOGY",
	"TEDAC_FCR_INDICATION_WHITE",
	"TEDAC_FCR_INDICATION_BLACK",
}


addOpacitySensitiveMaterials(opacity_sensitive_materials, "TEDAC")
addOpacitySensitiveMaterials(color_sensitive_materials, "TEDAC")

local ResourcesPath = LockOn_Options.script_path.."../IndicationResources/"
local IndicationTexturesPath = ResourcesPath.."Displays/TEDAC/"

used_render_mask_day	= IndicationTexturesPath.."TEDAC_day.dds"
used_render_mask_night	= IndicationTexturesPath.."TEDAC_night.dds"

-- Parameters handling functions
indicator_type	= indicator_types.COMMON
purposes		= {render_purpose.BAKE}--{render_purpose.GENERAL}

dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_PagesInit.lua")

--
use_parser					= false
dynamically_update_geometry	= false

writeParameter("MFD_NUM", MFD_SELF_IDS.TEDAC)
writeParameter("FONT_PREFIX", "font_TEDACMFD_")