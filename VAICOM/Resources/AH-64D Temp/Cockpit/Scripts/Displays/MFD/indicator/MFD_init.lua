dofile(LockOn_Options.common_script_path.."devices_defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Tools.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Pages_Init.lua")
dofile(LockOn_Options.script_path.."config.lua")

opacity_sensitive_materials = {}
color_sensitive_materials = {}

used_render_mask_white		= LockOn_Options.script_path.."../IndicationResources/Displays/MPD/MFD_white.dds"
used_render_mask_gray		= LockOn_Options.script_path.."../IndicationResources/Displays/MPD/MFD_gray.dds"
used_render_mask_green		= LockOn_Options.script_path.."../IndicationResources/Displays/MPD/MFD_green.dds"
used_render_mask_dark_green	= LockOn_Options.script_path.."../IndicationResources/Displays/MPD/MFD_dark_green.dds"

-- Specific part of MFD initialization
-- Parameters handling functions
indicator_type		= indicator_types.COMMON
--purposes			= {render_purpose.GENERAL, render_purpose.HUD_ONLY_VIEW}
purposes			= {render_purpose.BAKE}

-- page specific for the indicator, implements indicator border/FOV
BasePage = LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MFD_base.lua"
