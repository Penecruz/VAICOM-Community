dofile(LockOn_Options.common_script_path.."devices_defs.lua")
dofile(LockOn_Options.script_path.."Displays/KU/KU_Pages_Init.lua")

opacity_sensitive_materials = 
{
	"font_KU",
	--"font_KU_inv",
}

-- Parameters handling functions
indicator_type	= indicator_types.COMMON
purposes		= {render_purpose.GENERAL}

-- page specific for the indicator, implements indicator border/FOV
BasePage = LockOn_Options.script_path.."Displays/KU/Pages/MAIN.lua"
