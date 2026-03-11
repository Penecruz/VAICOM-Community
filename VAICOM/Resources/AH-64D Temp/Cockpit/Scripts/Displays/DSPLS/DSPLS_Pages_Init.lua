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
	GRAYSCALE		= counter(),
	COMMON			= counter(),
	COMMON_TOP		= counter(),
	HOVER			= counter(),
	BOB_UP			= counter(),
	TRANSITION		= counter(),
	CRUISE			= counter(),
	WEAPONS			= counter(),
}

-- construct subsets
local DSPLS_SubsetsPath = LockOn_Options.script_path.."Displays/DSPLS/Subsets/"

page_subsets = {}
page_subsets[id_subset.BASE]			= DSPLS_SubsetsPath.."BASE.lua"
page_subsets[id_subset.GRAYSCALE]		= DSPLS_SubsetsPath.."GRAYSCALE.lua"
page_subsets[id_subset.COMMON]			= DSPLS_SubsetsPath.."COMMON.lua"
page_subsets[id_subset.COMMON_TOP]		= DSPLS_SubsetsPath.."COMMON_TOP.lua"
page_subsets[id_subset.HOVER]			= DSPLS_SubsetsPath.."HOVER.lua"
page_subsets[id_subset.BOB_UP]			= DSPLS_SubsetsPath.."BOB_UP.lua"
page_subsets[id_subset.TRANSITION]		= DSPLS_SubsetsPath.."TRANSITION.lua"
page_subsets[id_subset.CRUISE]			= DSPLS_SubsetsPath.."CRUISE.lua"
page_subsets[id_subset.WEAPONS]			= DSPLS_SubsetsPath.."WEAPONS.lua"

-- list of pages
reset_counter()
id_Page =
{
	OFF					= counter(),
	EMPTY				= counter(),
	GRAYSCALE			= counter(),
	FLIGHT_HOVER		= counter(),
	FLIGHT_BOB_UP		= counter(),
	FLIGHT_TRANSITION	= counter(),
	FLIGHT_CRUISE		= counter(),
	WEAPON				= counter(),
}

--init_pageID	= id_Page.OFF
init_pageID	= id_Page.FLIGHT_HOVER
-- construct pages
pages = {}
pages[id_Page.OFF]					= {}
pages[id_Page.EMPTY]				= {id_subset.BASE}
pages[id_Page.GRAYSCALE]			= {id_subset.BASE, id_subset.GRAYSCALE}
pages[id_Page.FLIGHT_HOVER]			= {id_subset.BASE, id_subset.COMMON, id_subset.HOVER, id_subset.COMMON_TOP}
pages[id_Page.FLIGHT_BOB_UP]		= {id_subset.BASE, id_subset.COMMON, id_subset.HOVER, id_subset.BOB_UP, id_subset.COMMON_TOP}
pages[id_Page.FLIGHT_TRANSITION]	= {id_subset.BASE, id_subset.COMMON, id_subset.HOVER, id_subset.TRANSITION, id_subset.COMMON_TOP}
pages[id_Page.FLIGHT_CRUISE]		= {id_subset.BASE, id_subset.COMMON, id_subset.HOVER, id_subset.TRANSITION, id_subset.CRUISE, id_subset.COMMON_TOP}
pages[id_Page.WEAPON]				= {id_subset.BASE, id_subset.COMMON, id_subset.WEAPONS, id_subset.COMMON_TOP}
--
dofile(LockOn_Options.script_path.."Displays/DSPLS/DSPLS_FormatIDs.lua")

local function tablelen(T)
	local count = 0
	for _ in pairs(T) do count = count + 1 end
	return count
end

pages_by_mode = {}
clear_mode_table(pages_by_mode, tablelen(DSPLS_DISPL_FMT_LEV1), tablelen(DSPLS_DISPL_FMT_LEV2), tablelen(DSPLS_DISPL_FMT_LEV3), tablelen(DSPLS_DISPL_FMT_LEV4))
--
pages_by_mode[DSPLS_DISPL_FMT_LEV1.BLANK]		[DSPLS_DISPL_FMT_LEV2.NONE]			[DSPLS_DISPL_FMT_LEV3.NONE]	[DSPLS_DISPL_FMT_LEV4.NONE]	= id_Page.OFF
pages_by_mode[DSPLS_DISPL_FMT_LEV1.EMPTY]		[DSPLS_DISPL_FMT_LEV2.NONE]			[DSPLS_DISPL_FMT_LEV3.NONE]	[DSPLS_DISPL_FMT_LEV4.NONE]	= id_Page.EMPTY
pages_by_mode[DSPLS_DISPL_FMT_LEV1.GRAYSCALE]	[DSPLS_DISPL_FMT_LEV2.NONE]			[DSPLS_DISPL_FMT_LEV3.NONE]	[DSPLS_DISPL_FMT_LEV4.NONE]	= id_Page.GRAYSCALE
pages_by_mode[DSPLS_DISPL_FMT_LEV1.FLIGHT]		[DSPLS_DISPL_FMT_LEV2.HOVER]		[DSPLS_DISPL_FMT_LEV3.NONE]	[DSPLS_DISPL_FMT_LEV4.NONE]	= id_Page.FLIGHT_HOVER
pages_by_mode[DSPLS_DISPL_FMT_LEV1.FLIGHT]		[DSPLS_DISPL_FMT_LEV2.BOB_UP]		[DSPLS_DISPL_FMT_LEV3.NONE]	[DSPLS_DISPL_FMT_LEV4.NONE]	= id_Page.FLIGHT_BOB_UP
pages_by_mode[DSPLS_DISPL_FMT_LEV1.FLIGHT]		[DSPLS_DISPL_FMT_LEV2.TRANSITION]	[DSPLS_DISPL_FMT_LEV3.NONE]	[DSPLS_DISPL_FMT_LEV4.NONE]	= id_Page.FLIGHT_TRANSITION
pages_by_mode[DSPLS_DISPL_FMT_LEV1.FLIGHT]		[DSPLS_DISPL_FMT_LEV2.CRUISE]		[DSPLS_DISPL_FMT_LEV3.NONE]	[DSPLS_DISPL_FMT_LEV4.NONE]	= id_Page.FLIGHT_CRUISE
pages_by_mode[DSPLS_DISPL_FMT_LEV1.WEAPON]		[DSPLS_DISPL_FMT_LEV2.WEAPON]		[DSPLS_DISPL_FMT_LEV3.NONE]	[DSPLS_DISPL_FMT_LEV4.NONE]	= id_Page.WEAPON

--
function get_page_by_mode(master, L2, L3, L4)
	return get_page_by_mode_global(pages_by_mode, init_pageID, master, L2, L3, L4)
end
