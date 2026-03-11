dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_init.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Opacity_Materials.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/device/MFD_device_IDs.lua")

addOpacitySensitiveMaterials(opacity_sensitive_materials, "CPG_R")
addOpacitySensitiveMaterials(color_sensitive_materials, "CPG_R")

selfID = MFD_SELF_IDS.CPG_RMFD

writeParameter("MFD_init_DEFAULT_LEVEL", 6)
writeParameter("MFD_NUM", selfID)
writeParameter( "FONT_PREFIX", "font_CPG_RMFD_" )


