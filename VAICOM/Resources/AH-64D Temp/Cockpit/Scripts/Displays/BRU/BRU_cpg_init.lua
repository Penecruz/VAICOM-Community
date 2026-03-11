dofile(LockOn_Options.common_script_path.."devices_defs.lua")
dofile(LockOn_Options.script_path.."Displays/BRU/BRU_Pages_Init.lua")
dofile(LockOn_Options.script_path.."Displays/BRU/BRU_Tools.lua")

opacity_sensitive_materials = 
{
	"BRU_CPG_SYMBOLOGY",
	"BRU_PLT_SYMBOLOGY"
}   
brightness_sensitive_materials = 
{
	"BRU_CPG_SYMBOLOGY",
	"BRU_PLT_SYMBOLOGY"
}
writeParameter("BRU_Material", "BRU_CPG_SYMBOLOGY")
-- Parameters handling functions
indicator_type  = indicator_types.COMMON 					--indicator_type	= indicator_types.COLLIMATOR
purposes		= render_purpose.SCREENSPACE_INSIDE_COCKPIT 	--{render_purpose.GENERAL, render_purpose.HUD_ONLY_VIEW}

BasePage = LockOn_Options.script_path.."Displays/BRU/Pages/MAIN.lua"

-- optical center is 5.2 degrees below the FRL
--collimator_default_distance_factor = {auto_collimator_default_distance_factor[1], 0.110514, -0.046453}
--collimator_default_distance_factor = {auto_collimator_default_distance_factor[1], auto_collimator_default_distance_factor[1], 0}
