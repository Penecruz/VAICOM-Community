dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Tools.lua")
dofile(LockOn_Options.script_path.."Displays/Common/Units.lua")

display_size_inch			= 6.25
display_size_pix			= 1024
display_size_pix_05			= display_size_pix / 2
pix_to_inch					= display_size_inch / display_size_pix

function PXtoIn(param) return (param or 1) * pix_to_inch end						-- PX to inches
function InToPX(param) return (param or 1) / pix_to_inch end						-- inches to PX
-- Scale
SetCustomScale(PXtoIn() * InToMeter) -- Display Increments
collimated	= false

--if override_materials == true then
--	default_material  = override_material
--	current_font		  = override_font
--else
--	default_material  = stroke_material
--end

DEFAULT_LEVEL	= readParameter("MFD_init_DEFAULT_LEVEL")
---------------------------------------------------------------------------

---------------------------------------------------------------------------
-- Fonts


