dofile(LockOn_Options.common_script_path.."devices_defs.lua")
dofile(LockOn_Options.script_path.."Displays/EUFD/EUFD_Pages_Init.lua")
dofile(LockOn_Options.script_path.."Displays/EUFD/EUFD_Tools.lua")

-- Parameters handling functions
indicator_type	= indicator_types.COMMON
purposes 	    = {render_purpose.GENERAL}
use_parser		= false


opacity_sensitive_materials = 
{
	"font_EUFD_plt",
}

writeParameter("EUFD_Material", "font_EUFD_plt")