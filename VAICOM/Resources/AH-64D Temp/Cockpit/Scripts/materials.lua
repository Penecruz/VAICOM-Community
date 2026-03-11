dofile(LockOn_Options.common_script_path.."Fonts/symbols_locale.lua")
dofile(LockOn_Options.common_script_path.."Fonts/fonts_cmn.lua")
dofile(LockOn_Options.script_path.."fonts.lua")

local ResourcesPath = LockOn_Options.script_path.."../IndicationResources/"
local IndicationTexturesPath = ResourcesPath.."Displays/"

fontdescription["font_general_loc"] = fontdescription_cmn["font_general_loc"]

materials	= {}
textures	= {}
fonts		= {}


------- COMMON MATERIALS -------
materials["INDICATION_COMMON_LBLUE"]		= {   0, 200, 255, 255 }
materials["INDICATION_COMMON_AMBER"]		= { 255, 161,  45, 255 }
materials["INDICATION_COMMON_RED"]			= { 255,   0,   0, 255 }
materials["INDICATION_COMMON_WHITE"]		= { 255, 255, 255, 255 }
materials["INDICATION_COMMON_GREEN"]		= {   0, 255,   0, 255 }
materials["INDICATION_COMMON_DARK_GREEN"]	= {   0, 100,   0, 255 }
materials["INDICATION_COMMON_YELLOW"]		= { 255, 255,   0, 255 }
materials["INDICATION_COMMON_BLUE"]			= {   0, 191, 255, 255 }
materials["INDICATION_COMMON_BROWN"]		= { 188, 143, 143, 255 }

materials["GENERAL_INFO_GOLD"]				= { 255, 197,   3, 255 }

materials["MASK_MATERIAL"]					= { 255,   0, 255,  50 }
materials["MASK_MATERIAL_PURPLE"]			= { 255,   0, 255,  30 }

materials["DBG_GREY"]						= {  25,  25,  25, 255 }
materials["DBG_BLACK"]						= {   0,   0,   0, 255 }
materials["DBG_RED"]						= { 255,   0,   0, 255 }
materials["DBG_GREEN"]						= {   0, 255,   0, 255 }

materials["BLACK"]							= {   0,   0,   0, 255 }
materials["SOLID_BLACK"]					= {   0,   0,   0, 255 }
materials["SIMPLE_WHITE"]					= { 255, 255, 255, 255 }
materials["WHITE"]							= { 255, 255, 255, 255 }
materials["RED"]							= { 255,   0,   0, 255 }
materials["GREEN"]							= {   0, 255,   0, 255 }
materials["YELLOW"]							= { 255, 255,   0, 255 }
materials["BLUE"]							= {   0, 191, 255, 255 }
materials["BROWN"]							= { 188, 143, 143, 255 }
materials["PURPLE"]							= { 255,   0, 255, 255 }
materials["LBLUE"]							= { 173, 216, 230, 255 }

materials["INDICATION_YELLOW_GREEN"]		= { 121, 255,  19, 255 }
-----------------------------------------------------------------------------------
-- MPDs ---------------------------------------------------------------------------
-----------------------------------------------------------------------------------
-- MFD materials
-- prefixes for MFD materials, textures and fonts
local MFD_NAMES = { "PLT_L", "PLT_R", "CPG_L", "CPG_R", "TEDAC" }

materials["MFD_BACKGROUND"]			= {  0,   0,   0, 255}
materials["MFD_TRANSPARENT"]		= {  0,   0,   0, 0}
materials["MFD_TRANSLUCENT"]		= {  0,   0,   0, 200}

-- function creates unique materials for each MFD
local function add_MFD_Material(mat_name, color4)
	for i,obj in pairs(MFD_NAMES) do
		if (obj ~= "TEDAC") then
			materials[obj..mat_name] = color4
		else
			materials[obj..mat_name] = { 255, 255, 255, 255 }
		end
	end
end

add_MFD_Material("MFD_SOLID_RED",		{ 255,   0,   0, 255 })
add_MFD_Material("MFD_SOLID_BLACK",		{   0,   0,   0, 255 })
add_MFD_Material("MFD_SOLID_WHITE",		{ 255, 255, 255, 255 })
add_MFD_Material("MFD_SOLID_GREEN",		{   0, 255,   0, 255 })
add_MFD_Material("MFD_SOLID_YELLOW",	{ 255, 255,   0, 255 })


add_MFD_Material("MFD_RED",				{ 255,   0,   0, 255 })
add_MFD_Material("MFD_BLACK",			{   0,   0,   0, 255 })
add_MFD_Material("MFD_WHITE",			{ 255, 255, 255, 255 })
add_MFD_Material("MFD_GREEN",			{   0, 255,   0, 255 })
add_MFD_Material("MFD_DARK_GREEN",		{   0,  80,   0, 255 })
add_MFD_Material("MFD_YELLOW",			{ 255, 255,   0, 255 })
add_MFD_Material("MFD_DARK_YELLOW",		{  40,  40,   0, 255 })
add_MFD_Material("MFD_BLUE",			{   0, 191, 255, 255 })
add_MFD_Material("MFD_BROWN",			{ 210, 105,  30, 255 }) --{ 90, 70, 70, 255 }) --{ 188, 143, 143, 255 })
add_MFD_Material("MFD_DARK_WHITE",		{ 175, 175, 175, 255 }) -- Partial Intensity White
--add_MFD_Material("MFD_BLACK_CLR",		{ 0, 0,	0,	255 })		-- DBG_BLACK

-- MFD textures

textures["MFD_BLACK"]					= {IndicationTexturesPath.."MPD/indication_MPD.tga", 			materials["MFD_BACKGROUND"]}
textures["MFD_1024_BLACK"]				= {IndicationTexturesPath.."MPD/indication_MPD_1024.dds",		materials["MFD_BACKGROUND"]}
textures["MFD_TSD_BLACK"]				= {IndicationTexturesPath.."MPD/TSD/MAP_CM_TT_symbols.tga",		materials["MFD_BACKGROUND"]}
textures["MFD_TSD_BLACK_TRANSPARENT"]	= {IndicationTexturesPath.."MPD/TSD/MAP_CM_TT_symbols.tga",		materials["MFD_TRANSLUCENT"]}
textures["MFD_WPN_DBG_MASK"]			= {IndicationTexturesPath.."MPD/indication_MPD_WPN_fon.tga",	materials["MASK_MATERIAL"]}
textures["MFD_WPN_MSL_DBG_MASK"]		= {IndicationTexturesPath.."MPD/indication_MPD_WPN_fon.tga",	materials["MASK_MATERIAL"]}
textures["MFD_WPN_BLACK"]				= {IndicationTexturesPath.."MPD/indication_MPD_WPN_fon.tga",	materials["MFD_BACKGROUND"]}
textures["MFD_WPN_BLACK_TRANSPARENT"]	= {IndicationTexturesPath.."MPD/indication_MPD_WPN_fon.tga",	materials["MFD_TRANSLUCENT"]}
textures["MFD_TSD_COMPASS_BLACK"]		= {IndicationTexturesPath.."MPD/TSD/TSD_CompassRose.tga",		materials["MFD_BACKGROUND"]}


-- function creates unique textures for each MFD
local function add_MFD_Texture(tex_name, file_name, mat_name)
	for i,obj in pairs(MFD_NAMES) do
		textures[obj..tex_name] = {IndicationTexturesPath..file_name, materials[obj..mat_name]}
	end
end

add_MFD_Texture("MFD_RED",				"MPD/indication_MPD.tga", "MFD_RED")
add_MFD_Texture("MFD_WHITE",			"MPD/indication_MPD.tga", "MFD_WHITE")
add_MFD_Texture("MFD_GREEN",			"MPD/indication_MPD.tga", "MFD_GREEN")
add_MFD_Texture("MFD_DARK_GREEN",		"MPD/indication_MPD.tga", "MFD_DARK_GREEN")
add_MFD_Texture("MFD_YELLOW",			"MPD/indication_MPD.tga", "MFD_YELLOW")
add_MFD_Texture("MFD_BLUE",				"MPD/indication_MPD.tga", "MFD_BLUE")
add_MFD_Texture("MFD_BROWN",			"MPD/indication_MPD.tga", "MFD_BROWN")
add_MFD_Texture("MFD_BLACK",			"MPD/indication_MPD.tga", "MFD_BLACK")

add_MFD_Texture("MFD_1024_RED",			"MPD/indication_MPD_1024.dds", "MFD_RED")
add_MFD_Texture("MFD_1024_WHITE",		"MPD/indication_MPD_1024.dds", "MFD_WHITE")
add_MFD_Texture("MFD_1024_GREEN",		"MPD/indication_MPD_1024.dds", "MFD_GREEN")
add_MFD_Texture("MFD_1024_DARK_GREEN",	"MPD/indication_MPD_1024.dds", "MFD_DARK_GREEN")
add_MFD_Texture("MFD_1024_YELLOW",		"MPD/indication_MPD_1024.dds", "MFD_YELLOW")
add_MFD_Texture("MFD_1024_BLUE",		"MPD/indication_MPD_1024.dds", "MFD_BLUE")
add_MFD_Texture("MFD_1024_BROWN",		"MPD/indication_MPD_1024.dds", "MFD_BROWN")

add_MFD_Texture("MFD_WPN_WHITE",		"MPD/indication_MPD_WPN.tga", "MFD_WHITE")
add_MFD_Texture("MFD_WPN_GREEN",		"MPD/indication_MPD_WPN.tga", "MFD_GREEN")
add_MFD_Texture("MFD_WPN_YELLOW",		"MPD/indication_MPD_WPN.tga", "MFD_YELLOW")

add_MFD_Texture("MFD_TSD_WHITE",			"MPD/TSD/MAP_CM_TT_symbols.tga", 	"MFD_WHITE")
add_MFD_Texture("MFD_TSD_DARK_WHITE",		"MPD/TSD/MAP_CM_TT_symbols.tga", 	"MFD_DARK_WHITE")
add_MFD_Texture("MFD_TSD_BLACK_CLR",		"MPD/TSD/MAP_CM_TT_symbols.tga", 	"MFD_BACKGROUND")
add_MFD_Texture("MFD_TSD_YELLOW",			"MPD/TSD/MAP_CM_TT_symbols.tga", 	"MFD_YELLOW")
add_MFD_Texture("MFD_TSD_GREEN",			"MPD/TSD/MAP_CM_TT_symbols.tga", 	"MFD_GREEN")
add_MFD_Texture("MFD_TSD_CYAN",				"MPD/TSD/MAP_CM_TT_symbols.tga", 	"MFD_BLUE")
add_MFD_Texture("MFD_TSD_COMPASS_GREEN",	"MPD/TSD/TSD_CompassRose.tga",		"MFD_GREEN")

add_MFD_Texture("MFD_VIDEO_SYMBOLOGY",			"MPD/MPD_VideoSymbology.dds", "MFD_GREEN")
add_MFD_Texture("MFD_VIDEO_SYMBOLOGY_BROWN",	"MPD/MPD_VideoSymbology.dds", "MFD_BROWN")
add_MFD_Texture("MFD_VIDEO_SYMBOLOGY_BLUE",		"MPD/MPD_VideoSymbology.dds", "MFD_BLUE")
add_MFD_Texture("MFD_VIDEO_SYMBOLOGY_YELLOW",	"MPD/MPD_VideoSymbology.dds", "MFD_YELLOW")
add_MFD_Texture("MFD_VIDEO_SYMBOLOGY_WHITE",	"MPD/MPD_VideoSymbology.dds", "MFD_WHITE")
add_MFD_Texture("MFD_VIDEO_SYMBOLOGY_BLACK",	"MPD/MPD_VideoSymbology.dds", "BLACK")
add_MFD_Texture("MFD_TSD_FCR_Symbols_Green",	"MPD/MPD_FCR_indication_font.dds",	"MFD_GREEN")
add_MFD_Texture("MFD_TSD_FCR_Symbols_Yellow",	"MPD/MPD_FCR_indication_font.dds",	"MFD_YELLOW")
add_MFD_Texture("MFD_TSD_FCR_Symbols_Black",	"MPD/MPD_FCR_indication_font.dds",	"MFD_BLACK")
-- DBG
add_MFD_Texture("MFD_WPN_RED",				"MPD/indication_MPD_WPN.tga", 	"MFD_RED")
add_MFD_Texture("MFD_TSD_COMPASS_RED",		"MPD/TSD/TSD_CompassRose.tga", 	"MFD_RED")
--

-- MFD fonts
fonts["font_MFD"]					= {fontdescription["font_MFD"], 	10, materials["MFD_MATERIAL"]}
fonts["font_MFD_inv"]				= {fontdescription["font_MFD_inv"], 10, materials["MFD_MATERIAL"]}

-- function creates unique fonts ( normal, normal_bold inverted, inverted_bold) for each MFD
local function add_MFD_Font(font_name, description_name, mat_name)
	for i,obj in pairs(MFD_NAMES) do
		fonts["font_"..obj..font_name]				= {fontdescription[description_name],				10, materials[obj..mat_name]}
		fonts["font_"..obj..font_name.."_inv"]		= {fontdescription[description_name.."_inv"],		10, materials[obj..mat_name]}
		fonts["font_"..obj..font_name.."_bold"]		= {fontdescription[description_name.."_bold"],		10, materials[obj..mat_name]}
		fonts["font_"..obj..font_name.."_inv_bold"]	= {fontdescription[description_name.."_inv_bold"],	10, materials[obj..mat_name]}
	end
end

local function add_MFD_Font_Simple(font_name, description_name, mat_name)
	for i,obj in pairs(MFD_NAMES) do
		fonts["font_"..obj..font_name]				= {fontdescription[description_name],				10, materials[obj..mat_name]}
	end
end

add_MFD_Font("MFD_SOLID_BLACK",	"font_MFD", "MFD_SOLID_BLACK")

add_MFD_Font("MFD_RED",			"font_MFD", "MFD_RED")
add_MFD_Font("MFD_WHITE",		"font_MFD", "MFD_WHITE")
add_MFD_Font("MFD_GREEN",		"font_MFD", "MFD_GREEN")
add_MFD_Font("MFD_DARK_GREEN",	"font_MFD", "MFD_DARK_GREEN")
add_MFD_Font("MFD_YELLOW",		"font_MFD", "MFD_YELLOW")
add_MFD_Font("MFD_BLUE",		"font_MFD", "MFD_BLUE")
add_MFD_Font("MFD_BROWN",		"font_MFD", "MFD_BROWN")

add_MFD_Font("MFD_MAP_WP",	"font_MAP_WP_SA_symbs", "MFD_GREEN")
add_MFD_Font("MFD_MAP_SA",	"font_MAP_WP_SA_symbs", "MFD_WHITE")
add_MFD_Font("MFD_MAP_CM",	"font_MAP_CM_TT_symbs", "MFD_WHITE")
add_MFD_Font("MFD_MAP_TT",	"font_MAP_CM_TT_symbs", "MFD_RED")
add_MFD_Font("MFD_MAP_IDM",	"font_MAP_CM_TT_symbs", "MFD_BLUE")

add_MFD_Font("MFD_WPN_MSL",	"font_WPN_MSL_symbs", "MFD_GREEN")
add_MFD_Font("MFD_FCR_Target_Symbol",	"font_FCR_Target_symbol", "MFD_YELLOW")

fonts["MFD_MAP_WP_SA_BLACK"]				= {fontdescription["font_MAP_WP_SA_symbs"], 10, materials["BLACK"]}
fonts["MFD_MAP_WP_SA_BLACK_TRANSPARENT"]	= {fontdescription["font_MAP_WP_SA_symbs"], 10, materials["MFD_TRANSLUCENT"]}
fonts["MFD_MAP_CM_TT_BLACK"]				= {fontdescription["font_MAP_CM_TT_symbs"], 10, materials["BLACK"]}
fonts["MFD_MAP_CM_TT_BLACK_TRANSPARENT"]	= {fontdescription["font_MAP_CM_TT_symbs"], 10, materials["MFD_TRANSLUCENT"]}

fonts["MFD_WPN_MSL_BLACK"]				= {fontdescription["font_WPN_MSL_symbs_fon"], 10, materials["BLACK"]}
fonts["MFD_WPN_MSL_BLACK_TRANSPARENT"]	= {fontdescription["font_WPN_MSL_symbs_fon"], 10, materials["MFD_TRANSLUCENT"]}

fonts["MFD_FCR_TARGET_SYMBOL_BLACK"]	= {fontdescription["font_FCR_Target_symbol"], 10, materials["MFD_BACKGROUND"]}


add_MFD_Font_Simple("MFD_VIDEO",			"font_VIDEO_MPD", 		"MFD_GREEN")
add_MFD_Font_Simple("MFD_VIDEO_BLUE",		"font_VIDEO_MPD", 		"MFD_BLUE")
add_MFD_Font_Simple("MFD_VIDEO_BROWN",		"font_VIDEO_MPD", 		"MFD_BROWN")
add_MFD_Font_Simple("MFD_VIDEO_BIG",		"font_VIDEO_MPD_BIG", 	"MFD_GREEN")
add_MFD_Font_Simple("MFD_VIDEO_BIG_YELLOW",	"font_VIDEO_MPD_BIG", 	"MFD_YELLOW")
add_MFD_Font_Simple("MFD_VIDEO_BIG_RED",	"font_VIDEO_MPD_BIG", 	"MFD_RED")
add_MFD_Font_Simple("MFD_VIDEO_BLACK",		"font_VIDEO_MPD", 		"MFD_BLACK")
add_MFD_Font_Simple("MFD_VIDEO_BIG_BLACK",	"font_VIDEO_MPD_BIG", 	"MFD_BLACK")

-----------------------------------------------------------------------------------
-- Arcade -------------------------------------------------------------------------
-----------------------------------------------------------------------------------
textures["ARCADE"]							= {"arcade.tga",	materials["INDICATION_COMMON_RED"]}
textures["ARCADE_PUPRLE"]					= {"arcade.tga",	materials["PURPLE"]}
textures["ARCADE_WHITE"]					= {"arcade.tga",	materials["INDICATION_COMMON_WHITE"]}

-----------------------------------------------------------------------------------
-- Keboard Unit (KU) --------------------------------------------------------------
-----------------------------------------------------------------------------------
fonts["font_KU"]			= {fontdescription["font_KU"], 10, materials["INDICATION_YELLOW_GREEN"]}

-----------------------------------------------------------------------------------
-- EUFD ---------------------------------------------------------------------------
-----------------------------------------------------------------------------------
fonts["font_EUFD_plt"]		= {fontdescription["font_EUFD"], 10, materials["INDICATION_YELLOW_GREEN"]}
fonts["font_EUFD_cpg"]		= {fontdescription["font_EUFD"], 10, materials["INDICATION_YELLOW_GREEN"]}


--
--fonts["font_RWR"]						= {fontdescription["font_RWR"], 10, materials["INDICATION_COMMON_GREEN"]}
--fonts["font_ARC164_sheet"]				= {fontdescription["font_Sheet"], 10, materials["ARC164_SHEET"]}
--fonts["font_SightCamera"]				= {fontdescription["font_Sheet"], 10, materials["BLACK"]}
--fonts["font_general_hints"]				= {fontdescription["font_general_loc"], 10, materials["GENERAL_INFO_GOLD"]}
--fonts["font_general_keys"]				= {fontdescription["font_general_loc"], 10, materials["RED"]}
--fonts["font_hint_gload"]				= {fontdescription["font_general_loc"], 10, materials["GENERAL_INFO_GOLD"]}
--fonts["font_hints_kneeboard"]			= {fontdescription["font_general_loc"], 10, {100,0,100,255}}
--fonts["font_general_aihelper_message"]	= {fontdescription["font_general_loc"], 10, materials["RED"]}
--fonts["font_general_aihelper_howto"]	= {fontdescription["font_general_loc"], 10, materials["YELLOW"]}

-----------------------------------------------------------------------------------
-- Controls Indicator -------------------------------------------------------------
-----------------------------------------------------------------------------------
materials["CONTROLS_INDICATOR_BACK"]		= {  0,   0,   0, 150}
materials["CONTROLS_INDICATOR_WHITE"]		= {255, 255, 255, 255}
materials["CONTROLS_INDICATOR_RED"]			= {255,   0,   0, 255}
materials["CONTROLS_INDICATOR_GREEN"]		= {  0, 255,   0, 255}
materials["CONTROLS_INDICATOR_TURQUOISE"]	= {  0, 255, 255, 255}
materials["CONTROLS_INDICATOR_SCAS"]		= {200, 191, 231,  50}

textures["CONTROLS_INDICATOR_WHITE"]		= {"arcade.tga",	materials["CONTROLS_INDICATOR_WHITE"]}
textures["CONTROLS_INDICATOR_RED"]			= {"arcade.tga",	materials["CONTROLS_INDICATOR_RED"]}
textures["CONTROLS_INDICATOR_GREEN"]		= {"arcade.tga",	materials["CONTROLS_INDICATOR_GREEN"]}
textures["CONTROLS_INDICATOR_TURQUOISE"]	= {"arcade.tga",	materials["CONTROLS_INDICATOR_TURQUOISE"]}

fonts["font_CONTROLS_INDICATOR_WHITE"]	= {fontdescription["font_general_loc"], 1, materials["CONTROLS_INDICATOR_WHITE"]}
fonts["font_CONTROLS_INDICATOR_RED"]	= {fontdescription["font_general_loc"], 1, materials["CONTROLS_INDICATOR_RED"]}
fonts["font_CONTROLS_INDICATOR_GREEN"]	= {fontdescription["font_general_loc"], 1, materials["CONTROLS_INDICATOR_GREEN"]}

-----------------------------------------------------------------------------------
-- VRS Indicator ------------------------------------------------------------------
-----------------------------------------------------------------------------------
materials["VRS_INDICATOR"]		= {255, 255, 255, 255}

textures["VRS_INDICATOR"]		= {ResourcesPath.."VRS_Indication.dds",	materials["VRS_INDICATOR"]}

-----------------------------------------------------------------------------------
-- TADS ---------------------------------------------------------------------------
-----------------------------------------------------------------------------------
materials["TADS_WHITE"]				= {255, 255, 255, 255}
materials["TADS_BLACK"]				= {  0,   0,   0, 255}

textures["TADS_SYMBOLOGY"]			= {IndicationTexturesPath.."TADS_symbology.dds",	materials["TADS_WHITE"]}
textures["TADS_SYMBOLOGY_BLACK"]	= {IndicationTexturesPath.."TADS_symbology.dds",	materials["TADS_BLACK"]}

-----------------------------------------------------------------------------------
-- RMAP ---------------------------------------------------------------------------
-----------------------------------------------------------------------------------
materials["RMAP_WHITE"]				= {255, 255, 255, 255}
materials["RMAP_BLACK"]				= {  0,   0,   0, 255}

-----------------------------------------------------------------------------------
-- TEDAC --------------------------------------------------------------------------
-----------------------------------------------------------------------------------
materials["TEDAC_WHITE"]			= {255, 255, 255, 255}
materials["TEDAC_BLACK"]			= {  0,   0,   0, 255}

textures["TEDAC_SYMBOLOGY_TEST"]		= {IndicationTexturesPath.."TEDAC/TEDAC_symbology.dds",	materials["DBG_GREEN"]}
textures["TEDAC_SYMBOLOGY"]				= {IndicationTexturesPath.."TEDAC/TEDAC_symbology.dds",	materials["TEDAC_WHITE"]}
textures["TEDAC_SYMBOLOGY_BLACK"]		= {IndicationTexturesPath.."TEDAC/TEDAC_symbology.dds",	materials["TEDAC_BLACK"]}
textures["TEDAC_FCR_INDICATION_WHITE"]	= {IndicationTexturesPath.."TEDAC/TEDAC_FCR_indication_font.dds",	materials["TEDAC_WHITE"]}
textures["TEDAC_FCR_INDICATION_BLACK"]	= {IndicationTexturesPath.."TEDAC/TEDAC_FCR_indication_font.dds",	materials["TEDAC_BLACK"]}
textures["TEDAC_TSD_INDICATION_WHITE"]	= {IndicationTexturesPath.."MPD/TSD/MAP_CM_TT_symbols.tga",			materials["TEDAC_WHITE"]}

fonts["font_TEDAC"]							= {fontdescription["font_TEDAC"], 10, materials["TEDAC_WHITE"]}
fonts["font_TEDAC_BLACK"]					= {fontdescription["font_TEDAC"], 10, materials["TEDAC_BLACK"]}
fonts["font_TEDAC_big"]						= {fontdescription["font_TEDAC_big"], 10, materials["TEDAC_WHITE"]}
fonts["font_TEDAC_big_BLACK"]				= {fontdescription["font_TEDAC_big"], 10, materials["TEDAC_BLACK"]}
fonts["font_TEDAC_FCR_Target_symbol"]		= {fontdescription["font_TEDAC_FCR_Target_symbol"], 10, materials["TEDAC_WHITE"]}
fonts["font_TEDAC_FCR_Target_symbol_black"]	= {fontdescription["font_TEDAC_FCR_Target_symbol"], 10, materials["TEDAC_BLACK"]}

-----------------------------------------------------------------------------------
-- HMD ----------------------------------------------------------------------------
-----------------------------------------------------------------------------------
materials["HMD_WHITE"]				= {255, 255, 255, 255}
materials["HMD_GREEN"]				= {  0, 255,   0, 255}
materials["HMD_BLACK"]				= {  0,   0,   0, 255}
materials["HMD_BLACK_TRANSPARENT"]	= {  0,   0,   0, 100}

materials["HMD_RED"]				= {255,   0,   0, 255}
materials["HMD_RED_TRANSPARENT"]	= {  0,   0,   0, 0} --{ 70,   0,  5, 25}-- TODO:: remove
materials["RING_BLACK"]				= {  0,   0,   0, 180}
materials["HMD_BACKGROUND"]			= {  0,   0,   0, 255}
materials["HMD_TRANSPARENT"]		= {  0,   0,   0,   0}

textures["HMD_SYMBOLOGY_TEST"]		= {IndicationTexturesPath.."HMD/HMD_symbology.dds",	materials["DBG_GREEN"]}
textures["HMD_SYMBOLOGY"]			= {IndicationTexturesPath.."HMD/HMD_symbology.dds",	materials["HMD_GREEN"]}
textures["HMD_SYMBOLOGY_BLACK"]		= {IndicationTexturesPath.."HMD/HMD_symbology.dds",	materials["HMD_BLACK"]}
textures["HMD_SYMBOLOGY_WHITE"]		= {IndicationTexturesPath.."HMD/HMD_symbology.dds",	materials["HMD_WHITE"]}
textures["HMD_SYMBOLOGY_RED"]		= {IndicationTexturesPath.."HMD/HMD_symbology.dds",	materials["HMD_RED"]}
textures["HMD_RING_BLACK"]			= {IndicationTexturesPath.."HMD/HMD_symbology.dds",	materials["RING_BLACK"]}

fonts["font_HMD"]					= {fontdescription["font_HMD"],		10, materials["HMD_GREEN"]}
fonts["font_HMD_BLACK"]				= {fontdescription["font_HMD"],		10, materials["HMD_BLACK"]}
fonts["font_HMD_big"]				= {fontdescription["font_HMD_big"],	10, materials["HMD_GREEN"]}
fonts["font_HMD_big_BLACK"]			= {fontdescription["font_HMD_big"],	10, materials["HMD_BLACK"]}

-- stroked fonts
fonts["font_stroke_HMD"]			= {fontdescription["font_stroke_HMD"], 10, materials["HMD_GREEN"]}
--fonts["font_stroke_HMD_big"]		= {fontdescription["font_stroke_HMD_big"], 10, materials["HMD_GREEN"]}

-----------------------------------------------------------------------------------
-- BRU ----------------------------------------------------------------------------
-----------------------------------------------------------------------------------
materials["BRU_PLT_WHITE"]				= {255, 200,  32, 255}
materials["BRU_CPG_WHITE"]				= {255, 200,  32, 255}

materials["BRU_BLACK"]					= {  0,   0,   0, 255}
textures["BRU_PLT_SYMBOLOGY"]			= {ResourcesPath.."BRU/BRU_symbology.dds",	materials["BRU_PLT_WHITE"]}
textures["BRU_CPG_SYMBOLOGY"]			= {ResourcesPath.."BRU/BRU_symbology.dds",	materials["BRU_CPG_WHITE"]}

----------------------------------------------------------------------------------
-- CMWS --------------------------------------------------------------------------
----------------------------------------------------------------------------------
materials["CMWS_GREEN"]				= { 50, 255, 30, 255}
materials["CMWS_RED"]				= {255,  50, 10, 255}
materials["CMWS_ORANGE"]			= {255, 140,  0, 255}
materials["CMWS_ORANGE_DIMMED"]		= { 20,  15,  0, 255}

textures["CMWS_SYMBOLOGY_RED"]				= {ResourcesPath.."CMWS/CMWS_symbology.dds",	materials["CMWS_RED"]}
textures["CMWS_SYMBOLOGY_ORANGE"]			= {ResourcesPath.."CMWS/CMWS_symbology.dds",	materials["CMWS_ORANGE"]}
textures["CMWS_SYMBOLOGY_ORANGE_DIMMED"]	= {ResourcesPath.."CMWS/CMWS_symbology.dds",	materials["CMWS_ORANGE_DIMMED"]}

fonts["font_CMWS"]					= {fontdescription["font_CMWS"], 10, materials["CMWS_GREEN"]}

-- force preload resources to avoid freeze on start
preload_texture =
{
	"triggers.tga",
}

symbologyPaths =
{
	IndicationTexturesPath.."HMD",
}

