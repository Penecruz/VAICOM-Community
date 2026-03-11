dofile(LockOn_Options.common_script_path.."devices_defs.lua")

additive_alpha		= true
collimated			= true
--indicator_type = indicator_types.COLLIMATOR

opacity_sensitive_materials = 
{
	"font_stroke_HMD",
	"HMD_GREEN"
}

color_sensitive_materials = 
{
	"font_stroke_HMD",
	"HMD_GREEN"
}

brightness_sensitive_materials = 
{
	"font_stroke_HMD",
	"HMD_GREEN"
}
used_render_mask_hmd		= LockOn_Options.script_path.."../IndicationResources/Displays/HMD/HMD_mask.dds"

indicator_type = indicator_types.HELMET
purposes       = { render_purpose.GENERAL, render_purpose.HUD_ONLY_VIEW }
device_id = 2 -- 1-TEDAC; 2-HMD
dofile(LockOn_Options.script_path.."Displays/DSPLS/DSPLS_Pages_Init.lua")

use_parser					= false
dynamically_update_geometry	= false

-- optical center is 5.2 degrees below the FRL
--collimator_default_distance_factor = {auto_collimator_default_distance_factor[1], auto_collimator_default_distance_factor[1] * math.rad(-5.2), 0}

	