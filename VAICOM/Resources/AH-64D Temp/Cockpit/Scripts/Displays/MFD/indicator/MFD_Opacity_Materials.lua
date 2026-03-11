local MFD_opacity_sensitive_materials =
{
	"MFD_RED",
	"MFD_WHITE",
	"MFD_GREEN",
	"MFD_DARK_GREEN",
	"MFD_YELLOW",
	"MFD_DARK_YELLOW",
	"MFD_BLUE",
	"MFD_BROWN",
	"MFD_DARK_WHITE",
	--"MFD_BLACK_CLR",
}

local MFD_opacity_sensitive_textures = 
{
	"MFD_RED",
	"MFD_WHITE",
	"MFD_GREEN",
	"MFD_DARK_GREEN",
	"MFD_YELLOW",
	"MFD_DARK_YELLOW",
	"MFD_BLUE",
	"MFD_BROWN",

	"MFD_1024_RED",
	"MFD_1024_WHITE",
	"MFD_1024_GREEN",
	"MFD_1024_DARK_GREEN",
	"MFD_1024_YELLOW",
	"MFD_1024_BLUE",
	"MFD_1024_BROWN",

	"MFD_WPN_WHITE",
	"MFD_WPN_GREEN",
	"MFD_WPN_YELLOW",
	"MFD_WPN_RED",

	"MFD_TSD_WHITE",
	"MFD_TSD_DARK_WHITE",
	"MFD_TSD_BLACK_CLR",
	"MFD_TSD_YELLOW",
	"MFD_TSD_GREEN",
	"MFD_TSD_CYAN",
	
	"MFD_VIDEO_SYMBOLOGY",
	"MFD_VIDEO_SYMBOLOGY_BROWN",
	"MFD_VIDEO_SYMBOLOGY_BLUE",
	"MFD_VIDEO_SYMBOLOGY_YELLOW",
	"MFD_VIDEO_SYMBOLOGY_WHITE",
	"MFD_TSD_FCR_Symbols_Green",
	"MFD_TSD_FCR_Symbols_Yellow",
}

local MFD_opacity_sensitive_fonts = 
{
	"MFD_RED",
	"MFD_WHITE",
	"MFD_GREEN",
	"MFD_YELLOW",
	"MFD_BLUE",
	"MFD_BROWN",
	"MFD_VIDEO",
	"MFD_VIDEO_BLUE",
	"MFD_VIDEO_BROWN",
	"MFD_VIDEO_WHITE",
	"MFD_VIDEO_YELLOW",
	"MFD_VIDEO_BIG",
	"MFD_VIDEO_BIG_RED",
	"MFD_VIDEO_BIG_YELLOW",

	"MFD_MAP_WP",
	"MFD_MAP_SA",
	"MFD_MAP_CM",
	"MFD_MAP_TT",
	"MFD_WPN_MSL",
	"MFD_FCR_Target_Symbol",
}

function addOpacitySensitiveMaterials(tbl, prefix)
	-- materials
	-- TODO: ???
	-- textures
	for i,obj in pairs(MFD_opacity_sensitive_textures) do
		tbl[#tbl+1] = prefix..obj;
	end
	-- fonts
	for i,obj in pairs(MFD_opacity_sensitive_fonts) do
		tbl[#tbl+1] = "font_"..prefix..obj;
		tbl[#tbl+1] = "font_"..prefix..obj.."_inv";
		tbl[#tbl+1] = "font_"..prefix..obj.."_bold";
		tbl[#tbl+1] = "font_"..prefix..obj.."_inv_bold";
	end
end