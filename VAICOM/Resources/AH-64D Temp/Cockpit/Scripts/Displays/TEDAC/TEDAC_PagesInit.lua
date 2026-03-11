--
local count = 0
local function counter()
	count = count + 1
	return count
end

local function reset_counter()
	count = -1
end


-- list of subsets
reset_counter()
local id_subset =
{
	BASE			= counter(),
	BASE_TOP		= counter(),
	GRAYSCALE		= counter(),
	VIDEO			= counter(),
	COMMON			= counter(),
	COMMON_TOP		= counter(),
	HOVER			= counter(),
	BOB_UP			= counter(),
	TRANSITION		= counter(),
	CRUISE			= counter(),
	WEAPONS			= counter(),
	HMD				= counter(),
	FCR_GTM			= counter(),
	FCR_RMAP		= counter(),
	FCR_ATM			= counter(),
	FCR_TPM			= counter(),
	FCR_MENU		= counter(),
	FCR_TGT			= counter(),
	FCR_TGT_MENU	= counter(),
	FCR_STATUS		= counter(),
	FCR_ACQ_MENU	= counter(),
	FCR_ACQ			= counter(),
	FCR_RFHO_MENU	= counter(),
	FCR_RFHO		= counter(),
	FCR_MENU_TPM	= counter(),
	FCR_PROF_MENU	= counter(),
	FCR_PROF		= counter(),
	FCR_LINE_MENU	= counter(),
	FCR_LINE		= counter(),
	FCR_CLEARANCE_MENU	= counter(),
	FCR_CLEARANCE	= counter(),
	COMMON_FOR		= counter(),
	COMMON_EASW		= counter(),
}


-- construct subsets
local TEDAC_SubsetsPath = LockOn_Options.script_path.."Displays/TEDAC/Subsets/"
local TEDAC_SubsetsPathFCR = LockOn_Options.script_path.."Displays/TEDAC/Subsets/FCR_page/"
local TEDAC_SubsetsPathFCR_GTM = LockOn_Options.script_path.."/Displays/TEDAC/Subsets/FCR_page/GTM/"
local TEDAC_SubsetsPathFCR_Rmap = LockOn_Options.script_path.."/Displays/TEDAC/Subsets/FCR_page/RMAP/"
local TEDAC_SubsetsPathFCR_ATM = LockOn_Options.script_path.."/Displays/TEDAC/Subsets/FCR_page/ATM/"
local TEDAC_SubsetsPathFCR_TPM = LockOn_Options.script_path.."/Displays/TEDAC/Subsets/FCR_page/TPM/"

page_subsets = {}
page_subsets[id_subset.BASE]			= TEDAC_SubsetsPath.."BASE.lua"
page_subsets[id_subset.BASE_TOP]		= TEDAC_SubsetsPath.."BASE_TOP.lua"
page_subsets[id_subset.GRAYSCALE]		= TEDAC_SubsetsPath.."GRAYSCALE.lua"
page_subsets[id_subset.VIDEO]			= TEDAC_SubsetsPath.."VIDEO.lua"
page_subsets[id_subset.COMMON]			= TEDAC_SubsetsPath.."COMMON.lua"
page_subsets[id_subset.COMMON_TOP]		= TEDAC_SubsetsPath.."COMMON_TOP.lua"
page_subsets[id_subset.HOVER]			= TEDAC_SubsetsPath.."HOVER.lua"
page_subsets[id_subset.BOB_UP]			= TEDAC_SubsetsPath.."BOB_UP.lua"
page_subsets[id_subset.TRANSITION]		= TEDAC_SubsetsPath.."TRANSITION.lua"
page_subsets[id_subset.CRUISE]			= TEDAC_SubsetsPath.."CRUISE.lua"
page_subsets[id_subset.WEAPONS]			= TEDAC_SubsetsPath.."WEAPONS.lua"
page_subsets[id_subset.HMD]				= TEDAC_SubsetsPath.."HMD.lua"
page_subsets[id_subset.FCR_GTM]			= TEDAC_SubsetsPathFCR_GTM.."FCR_GTM.lua"
page_subsets[id_subset.FCR_RMAP]		= TEDAC_SubsetsPathFCR_Rmap.."FCR_RMAP.lua"
page_subsets[id_subset.FCR_ATM]			= TEDAC_SubsetsPathFCR_ATM.."FCR_ATM.lua"
page_subsets[id_subset.FCR_TPM]			= TEDAC_SubsetsPathFCR_TPM.."FCR_TPM.lua"
page_subsets[id_subset.FCR_MENU]		= TEDAC_SubsetsPathFCR.."FCR_MENU.lua"
page_subsets[id_subset.FCR_TGT]			= TEDAC_SubsetsPathFCR.."FCR_TGT.lua"
page_subsets[id_subset.FCR_TGT_MENU]	= TEDAC_SubsetsPathFCR.."FCR_TGT_MENU.lua"
page_subsets[id_subset.FCR_STATUS]		= TEDAC_SubsetsPathFCR.."FCR_STATUS.lua"
page_subsets[id_subset.FCR_ACQ_MENU]	= TEDAC_SubsetsPathFCR.."FCR_ACQ_MENU.lua"
page_subsets[id_subset.FCR_ACQ]			= TEDAC_SubsetsPathFCR.."FCR_ACQ.lua"
page_subsets[id_subset.FCR_RFHO_MENU]	= TEDAC_SubsetsPathFCR.."FCR_RFHO_MENU.lua"
page_subsets[id_subset.FCR_RFHO]		= TEDAC_SubsetsPathFCR.."FCR_RFHO.lua"
page_subsets[id_subset.FCR_MENU_TPM]	= TEDAC_SubsetsPathFCR.."FCR_MENU_TPM.lua"
page_subsets[id_subset.FCR_PROF_MENU]	= TEDAC_SubsetsPathFCR.."FCR_PROF_MENU.lua"
page_subsets[id_subset.FCR_PROF]		= TEDAC_SubsetsPathFCR.."FCR_PROF.lua"
page_subsets[id_subset.FCR_LINE_MENU]	= TEDAC_SubsetsPathFCR.."FCR_LINE_MENU.lua"
page_subsets[id_subset.FCR_LINE]		= TEDAC_SubsetsPathFCR.."FCR_LINE.lua"
page_subsets[id_subset.FCR_CLEARANCE_MENU]	= TEDAC_SubsetsPathFCR.."FCR_CLEARANCE_MENU.lua"
page_subsets[id_subset.FCR_CLEARANCE]	= TEDAC_SubsetsPathFCR.."FCR_CLEARANCE.lua"
page_subsets[id_subset.COMMON_FOR]		= TEDAC_SubsetsPath.."COMMON_FOR.lua"
page_subsets[id_subset.COMMON_EASW]		= TEDAC_SubsetsPath.."COMMON_EASW.lua"

-- list of pages
reset_counter()
id_Page =
{
	OFF					= counter(),
	GRAYSCALE			= counter(),
	FLIGHT_HOVER		= counter(),
	FLIGHT_BOB_UP		= counter(),
	FLIGHT_TRANSITION	= counter(),
	FLIGHT_CRUISE		= counter(),
	WEAPON_TADS			= counter(),
	WEAPON_HMD			= counter(),
	FCR_GTM				= counter(),
	FCR_RMAP			= counter(),
	FCR_ATM				= counter(),
	FCR_TPM				= counter(),
}

init_pageID	= id_Page.OFF


-- construct pages
pages = {}
pages[id_Page.OFF]					= {}
pages[id_Page.GRAYSCALE]			= {id_subset.GRAYSCALE}
pages[id_Page.FLIGHT_HOVER]			= {id_subset.BASE, id_subset.VIDEO, id_subset.COMMON, 		id_subset.COMMON_FOR,	id_subset.HOVER, 	id_subset.COMMON_TOP}
pages[id_Page.FLIGHT_BOB_UP]		= {id_subset.BASE, id_subset.VIDEO, id_subset.COMMON,		id_subset.COMMON_FOR,	id_subset.HOVER, 	id_subset.BOB_UP, 			id_subset.COMMON_TOP}
pages[id_Page.FLIGHT_TRANSITION]	= {id_subset.BASE, id_subset.VIDEO, id_subset.COMMON,		id_subset.COMMON_FOR,	id_subset.HOVER, 	id_subset.TRANSITION, 		id_subset.COMMON_TOP}
pages[id_Page.FLIGHT_CRUISE]		= {id_subset.BASE, id_subset.VIDEO, id_subset.COMMON,		id_subset.COMMON_FOR,	id_subset.HOVER, 	id_subset.TRANSITION, 		id_subset.CRUISE, 		id_subset.COMMON_TOP}
pages[id_Page.WEAPON_TADS]			= {id_subset.BASE, id_subset.VIDEO, id_subset.COMMON,		id_subset.COMMON_FOR,	id_subset.WEAPONS,	id_subset.COMMON_TOP}
pages[id_Page.WEAPON_HMD]			= {id_subset.BASE, id_subset.HMD,	id_subset.COMMON,		id_subset.COMMON_FOR,	id_subset.WEAPONS,	id_subset.COMMON_TOP}
pages[id_Page.FCR_GTM]				= {id_subset.BASE, id_subset.COMMON, id_subset.COMMON_FOR,	id_subset.FCR_GTM, 		id_subset.FCR_RFHO,	id_subset.FCR_RFHO_MENU,	id_subset.FCR_TGT_MENU,	id_subset.FCR_TGT,	id_subset.FCR_MENU, 	id_subset.FCR_ACQ,		id_subset.FCR_ACQ_MENU,	id_subset.FCR_STATUS,	id_subset.BASE_TOP}
pages[id_Page.FCR_RMAP]				= {id_subset.BASE, id_subset.COMMON, id_subset.COMMON_FOR,	id_subset.FCR_RMAP, 	id_subset.FCR_RFHO,	id_subset.FCR_RFHO_MENU,	id_subset.FCR_TGT_MENU,	id_subset.FCR_TGT,	id_subset.FCR_MENU, 	id_subset.FCR_ACQ,		id_subset.FCR_ACQ_MENU,	id_subset.FCR_STATUS,	id_subset.BASE_TOP}
pages[id_Page.FCR_ATM]				= {id_subset.BASE, id_subset.COMMON, id_subset.COMMON_EASW,	id_subset.FCR_ATM, 		id_subset.FCR_RFHO,	id_subset.FCR_RFHO_MENU,	id_subset.FCR_TGT_MENU,	id_subset.FCR_TGT,	id_subset.FCR_MENU, 	id_subset.FCR_ACQ,		id_subset.FCR_ACQ_MENU,	id_subset.FCR_STATUS,	id_subset.BASE_TOP}
pages[id_Page.FCR_TPM]				= {id_subset.BASE, id_subset.COMMON, id_subset.COMMON_FOR,	id_subset.FCR_TPM, 		id_subset.FCR_MENU_TPM, id_subset.FCR_PROF, 	id_subset.FCR_PROF_MENU,  id_subset.FCR_LINE, id_subset.FCR_LINE_MENU, id_subset.FCR_CLEARANCE, id_subset.FCR_CLEARANCE_MENU,  id_subset.FCR_STATUS, id_subset.BASE_TOP}
--

dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_FormatIDs.lua")

local function tablelen(T)
	local count = 0
	for _ in pairs(T) do count = count + 1 end
	return count
end

pages_by_mode = {}
clear_mode_table(pages_by_mode, tablelen(TEDAC_DISPL_FMT_LEV1), tablelen(TEDAC_DISPL_FMT_LEV2), tablelen(TEDAC_DISPL_FMT_LEV3), tablelen(TEDAC_DISPL_FMT_LEV4))

--
pages_by_mode[TEDAC_DISPL_FMT_LEV1.BLANK]		[TEDAC_DISPL_FMT_LEV2.NONE]			[TEDAC_DISPL_FMT_LEV3.NONE]	[TEDAC_DISPL_FMT_LEV4.NONE]	= id_Page.OFF
pages_by_mode[TEDAC_DISPL_FMT_LEV1.GRAYSCALE]	[TEDAC_DISPL_FMT_LEV2.NONE]			[TEDAC_DISPL_FMT_LEV3.NONE]	[TEDAC_DISPL_FMT_LEV4.NONE]	= id_Page.GRAYSCALE
pages_by_mode[TEDAC_DISPL_FMT_LEV1.FLIGHT]		[TEDAC_DISPL_FMT_LEV2.HOVER]		[TEDAC_DISPL_FMT_LEV3.NONE]	[TEDAC_DISPL_FMT_LEV4.NONE]	= id_Page.FLIGHT_HOVER
pages_by_mode[TEDAC_DISPL_FMT_LEV1.FLIGHT]		[TEDAC_DISPL_FMT_LEV2.BOB_UP]		[TEDAC_DISPL_FMT_LEV3.NONE]	[TEDAC_DISPL_FMT_LEV4.NONE]	= id_Page.FLIGHT_BOB_UP
pages_by_mode[TEDAC_DISPL_FMT_LEV1.FLIGHT]		[TEDAC_DISPL_FMT_LEV2.TRANSITION]	[TEDAC_DISPL_FMT_LEV3.NONE]	[TEDAC_DISPL_FMT_LEV4.NONE]	= id_Page.FLIGHT_TRANSITION
pages_by_mode[TEDAC_DISPL_FMT_LEV1.FLIGHT]		[TEDAC_DISPL_FMT_LEV2.CRUISE]		[TEDAC_DISPL_FMT_LEV3.NONE]	[TEDAC_DISPL_FMT_LEV4.NONE]	= id_Page.FLIGHT_CRUISE
pages_by_mode[TEDAC_DISPL_FMT_LEV1.WEAPON]		[TEDAC_DISPL_FMT_LEV2.WEAPON_TADS]	[TEDAC_DISPL_FMT_LEV3.NONE]	[TEDAC_DISPL_FMT_LEV4.NONE]	= id_Page.WEAPON_TADS
pages_by_mode[TEDAC_DISPL_FMT_LEV1.WEAPON]		[TEDAC_DISPL_FMT_LEV2.WEAPON_HMD]	[TEDAC_DISPL_FMT_LEV3.NONE]	[TEDAC_DISPL_FMT_LEV4.NONE]	= id_Page.WEAPON_HMD
pages_by_mode[TEDAC_DISPL_FMT_LEV1.FCR]			[TEDAC_DISPL_FMT_LEV2.FCR_GTM]		[TEDAC_DISPL_FMT_LEV3.NONE]	[TEDAC_DISPL_FMT_LEV4.NONE]	= id_Page.FCR_GTM
pages_by_mode[TEDAC_DISPL_FMT_LEV1.FCR]			[TEDAC_DISPL_FMT_LEV2.FCR_RMAP]		[TEDAC_DISPL_FMT_LEV3.NONE]	[TEDAC_DISPL_FMT_LEV4.NONE]	= id_Page.FCR_RMAP
pages_by_mode[TEDAC_DISPL_FMT_LEV1.FCR]			[TEDAC_DISPL_FMT_LEV2.FCR_ATM]		[TEDAC_DISPL_FMT_LEV3.NONE]	[TEDAC_DISPL_FMT_LEV4.NONE]	= id_Page.FCR_ATM
pages_by_mode[TEDAC_DISPL_FMT_LEV1.FCR]			[TEDAC_DISPL_FMT_LEV2.FCR_TPM]		[TEDAC_DISPL_FMT_LEV3.NONE]	[TEDAC_DISPL_FMT_LEV4.NONE]	= id_Page.FCR_TPM
--
function get_page_by_mode(master, L2, L3, L4)
	return get_page_by_mode_global(pages_by_mode, init_pageID, master, L2, L3, L4)
end
