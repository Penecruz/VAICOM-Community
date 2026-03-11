dofile(LockOn_Options.common_script_path.."devices_defs.lua")
dofile(LockOn_Options.script_path.."ASE/CMWS/CMWS_Pages_Init.lua")

opacity_sensitive_materials = 
{
	"font_CMWS",
    "CMWS_SYMBOLOGY_RED",
    "CMWS_SYMBOLOGY_ORANGE",
	"CMWS_SYMBOLOGY_ORANGE_DIMMED"
}   
brightness_sensitive_materials = 
{
	"font_CMWS",
    "CMWS_SYMBOLOGY_RED",
    "CMWS_SYMBOLOGY_ORANGE",
	"CMWS_SYMBOLOGY_ORANGE_DIMMED"
}

-- Parameters handling functions
indicator_type  = indicator_types.COMMON
purposes		= render_purpose.SCREENSPACE_INSIDE_COCKPIT


