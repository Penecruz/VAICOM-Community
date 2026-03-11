dofile(LockOn_Options.common_script_path.."devices_defs.lua")
dofile(LockOn_Options.common_script_path.."ViewportHandling.lua")


render_target		 = 0 -- mfd0  
--used_render_mask	 = LockOn_Options.script_path.."../IndicationTextures/gunCameraNoise.bmp"
indicator_type       = indicator_types.COMMON
page_subsets    	 = {LockOn_Options.script_path.."SightCamera/page.lua"}
pages 				 = {{1}}
init_pageID     	 = 1


camera = 
{
	pos		  = {3.692, 0.733, 0.0},
	--elevation = math.rad(-0.4),
	fov 	  = 2 * math.atan(0.5 * 12.7/30.0)	-- film 16mm: 10.26 x 7.49 (mm), diagonal 12.7 mm
}

update_screenspace_diplacement(16/12,false,0)
options_source 			  = "F-5E-3"
dedicated_viewport_arcade = dedicated_viewport