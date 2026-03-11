dofile(LockOn_Options.script_path.."Displays/EUFD/EUFD_FormatsIDs.lua")
-- Common initialization for all EUFD indicators
dynamically_update_geometry = false
	
local PagesPath			= LockOn_Options.script_path.."Displays/EUFD/Pages/"


local count = 0
local function counter()
	count = count + 1
	return count
end
local function resetCounter()
	count = 0
end

--------------------------------------------------------------------------------------------------
-- SUBSETS ---------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------
-- Display borders, collimator projecting areas, etc
local SUBSET_MAIN				= counter()
local SUBSET_PRESET				= counter()
local SUBSET_TEST				= counter()
local SUBSET_LOAD				= counter()

page_subsets = {}

page_subsets[SUBSET_MAIN]					= PagesPath.."MAIN.lua"
page_subsets[SUBSET_PRESET]					= PagesPath.."PRESET.lua"
page_subsets[SUBSET_TEST]					= PagesPath.."TEST.lua"
page_subsets[SUBSET_LOAD]					= PagesPath.."LOAD.lua"

--------------------------------------------------------------------------------------------------
-- PAGES -----------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------
resetCounter()
PAGE_BLANK				= counter()
PAGE_MAIN				= counter()
PAGE_PRESET				= counter()
PAGE_TEST				= counter()
PAGE_LOAD				= counter()

pages = {}

pages[PAGE_BLANK]		= {}
pages[PAGE_MAIN]		= {SUBSET_MAIN}
pages[PAGE_PRESET]		= {SUBSET_PRESET}
pages[PAGE_TEST]		= {SUBSET_TEST}
pages[PAGE_LOAD]		= {SUBSET_LOAD}

init_pageID	= PAGE_MAIN

--------------------------------------------------------------------------------------------------
-- PAGES BY MODE ---------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------
local function tablelen(T)
	local count = 0
	for _ in pairs(T) do count = count + 1 end
	return count
end

pages_by_mode = {}
clear_mode_table(pages_by_mode, tablelen(EUFD_DISPL_FMT_LEV1), tablelen(EUFD_DISPL_FMT_LEV2), tablelen(EUFD_DISPL_FMT_LEV3))
				
pages_by_mode[EUFD_DISPL_FMT_LEV1.NONE]		[EUFD_DISPL_FMT_LEV2.NONE]	[EUFD_DISPL_FMT_LEV3.NONE]	[EUFD_DISPL_FMT_LEV4.NONE] = PAGE_BLANK
pages_by_mode[EUFD_DISPL_FMT_LEV1.MAIN]		[EUFD_DISPL_FMT_LEV2.NONE]	[EUFD_DISPL_FMT_LEV3.NONE]	[EUFD_DISPL_FMT_LEV4.NONE] = PAGE_MAIN
pages_by_mode[EUFD_DISPL_FMT_LEV1.PRESET]	[EUFD_DISPL_FMT_LEV2.NONE]	[EUFD_DISPL_FMT_LEV3.NONE]	[EUFD_DISPL_FMT_LEV4.NONE] = PAGE_PRESET
pages_by_mode[EUFD_DISPL_FMT_LEV1.TEST]		[EUFD_DISPL_FMT_LEV2.NONE]	[EUFD_DISPL_FMT_LEV3.NONE]	[EUFD_DISPL_FMT_LEV4.NONE] = PAGE_TEST
pages_by_mode[EUFD_DISPL_FMT_LEV1.LOAD]		[EUFD_DISPL_FMT_LEV2.NONE]	[EUFD_DISPL_FMT_LEV3.NONE]	[EUFD_DISPL_FMT_LEV4.NONE] = PAGE_LOAD


function get_page_by_mode(master, L2, L3, L4)
	return get_page_by_mode_global(pages_by_mode, init_pageID, master, L2, L3, L4)
end
