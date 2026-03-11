dofile(LockOn_Options.script_path.."Displays/BRU/BRU_FormatsIDs.lua")
dynamically_update_geometry = false
	
local PagesPath			= LockOn_Options.script_path.."Displays/BRU/Pages/"

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
page_subsets = {}
page_subsets[SUBSET_MAIN]		= PagesPath.."MAIN.lua"

--------------------------------------------------------------------------------------------------
-- PAGES -----------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------
resetCounter()
PAGE_BLANK				= counter()
PAGE_MAIN				= counter()

pages = {}

pages[PAGE_BLANK]		= {}
pages[PAGE_MAIN]		= {SUBSET_MAIN}

init_pageID	= PAGE_BLANK

--------------------------------------------------------------------------------------------------
-- PAGES BY MODE ---------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------
local function tablelen(T)
	local count = 0
	for _ in pairs(T) do count = count + 1 end
	return count
end

pages_by_mode = {}
clear_mode_table(pages_by_mode, tablelen(BRU_DISPL_FMT_LEV1), tablelen(BRU_DISPL_FMT_LEV2), tablelen(BRU_DISPL_FMT_LEV3))
				
pages_by_mode[BRU_DISPL_FMT_LEV1.BLANK]	[BRU_DISPL_FMT_LEV2.NONE]	[BRU_DISPL_FMT_LEV3.NONE]	[BRU_DISPL_FMT_LEV4.NONE] = PAGE_BLANK
pages_by_mode[BRU_DISPL_FMT_LEV1.MAIN]	[BRU_DISPL_FMT_LEV2.NONE]	[BRU_DISPL_FMT_LEV3.NONE]	[BRU_DISPL_FMT_LEV4.NONE] = PAGE_MAIN


function get_page_by_mode(master, L2, L3, L4)
	return get_page_by_mode_global(pages_by_mode, init_pageID, master, L2, L3, L4)
end
