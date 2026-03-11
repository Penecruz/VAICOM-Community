dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_formats_IDs.lua")
-- Common initialization for all MFD indicators
dynamically_update_geometry = false

local PagesPath			= LockOn_Options.script_path.."Displays/MFD/indicator/Pages/"

-- he maximum number of local variables per function is hard-coded into the Lua source; it is 200 (#define MAXVARS 200 - lparser.c)
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
local SUBSET = {}

-- Display borders, collimator projecting areas, etc
SUBSET['BASE']						= counter()
SUBSET['BASE_TSD']					= counter()
SUBSET['BASE_TOP']					= counter()
SUBSET['BLANK']						= counter()
SUBSET['MENU']						= counter()
SUBSET['INIT']						= counter()
SUBSET['ASTERISK']					= counter()

SUBSET['FCR_GTM']					= counter()
SUBSET['FCR_RMAP']					= counter()
SUBSET['FCR_ATM']					= counter()
SUBSET['FCR_TPM']					= counter()
SUBSET['FCR_ATM_MENU']				= counter()
SUBSET['FCR_MENU']					= counter()
SUBSET['FCR_TGT']					= counter()
SUBSET['FCR_ACQ']					= counter()
SUBSET['FCR_RFHO']					= counter()
SUBSET['FCR_TPM_MENU']				= counter()
SUBSET['FCR_LINE']					= counter()
SUBSET['FCR_PROF']					= counter()
SUBSET['FCR_CLEARANCE']				= counter()

SUBSET['FCR_STATUS']				= counter()
SUBSET['FCR_UTIL']					= counter()
SUBSET['FCR_UTIL_BIT']				= counter()
SUBSET['FCR_UTIL_MISSION']			= counter()

SUBSET['WPN_BASE']					= counter()
SUBSET['WPN_BASE_MENU']				= counter()
SUBSET['WPN_BASE_MENU_UPDOWN']		= counter()
SUBSET['WPN_TRAIN_MODE']			= counter()

SUBSET['WPN_MAIN']					= counter()
SUBSET['WPN_MENU_ACQ']				= counter()

SUBSET['WPN_GUN_BASE']				= counter()
SUBSET['WPN_GUN_BASE_MENU']			= counter()

SUBSET['WPN_MSL_SAL_BASE']			= counter()
SUBSET['WPN_MSL_SAL_BASE_MENU_L']	= counter()
SUBSET['WPN_MSL_SAL_BASE_MENU_R']	= counter()
SUBSET['WPN_MSL_SAL_MODE_MENU']		= counter()
SUBSET['WPN_MSL_SAL_TRAJ_MENU']		= counter()
SUBSET['WPN_MSL_SAL_SSEL_MENU']		= counter()
SUBSET['WPN_MSL_SAL_PRI_MENU']		= counter()
SUBSET['WPN_MSL_SAL_ALT_MENU']		= counter()

SUBSET['WPN_MSL_RF_BASE']			= counter()
SUBSET['WPN_MSL_RF_BASE_MENU']		= counter()

SUBSET['WPN_RKT_BASE']				= counter()
SUBSET['WPN_RKT_BASE_MENU']			= counter()
SUBSET['WPN_RKT_PEN']				= counter()
SUBSET['WPN_RKT_QTY']				= counter()

SUBSET['WPN_CHAN']					= counter()
SUBSET['WPN_CODE']					= counter()
SUBSET['WPN_FREQ']					= counter()

SUBSET['WPN_RKT_INV']				= counter()
SUBSET['WPN_BORESIGHT']				= counter()
SUBSET['WPN_LOAD']					= counter()
SUBSET['WPN_UTIL']					= counter()
SUBSET['WPN_PLT_EOCCM_UTIL']		= counter()

SUBSET['ACQ_MENU']					= counter()

SUBSET['TSD_MAIN_BASE']				= counter()
SUBSET['TSD_MAIN']					= counter()
SUBSET['TSD_MAIN_ACQ']				= counter()
SUBSET['TSD_MAIN_REC']				= counter()

SUBSET['TSD_VIDEOSIGNAL']			= counter()
SUBSET['TSD_COMMON_SYMBS']			= counter()
SUBSET['TSD_RPT_BASE']				= counter()
SUBSET['TSD_RPT_MAIN']				= counter()
SUBSET['TSD_RPT_STAT']				= counter()
SUBSET['TSD_RPT_BDA']				= counter()
SUBSET['TSD_RPT_TGT']				= counter()
SUBSET['TSD_RPT_PP']				= counter()
SUBSET['TSD_RPT_FARM']				= counter()

SUBSET['TSD_FARM_BASE']				= counter()
SUBSET['TSD_FARM_MAIN']				= counter()
SUBSET['TSD_FARM_TYPE']				= counter()

SUBSET['TSD_PAN_BASE']				= counter()
SUBSET['TSD_PAN_2D']				= counter()
SUBSET['TSD_PAN_3D']				= counter()

SUBSET['TSD_SHOW_BASE']				= counter()
SUBSET['TSD_SHOW_MAIN']				= counter()
SUBSET['TSD_SHOW_SA']				= counter()
SUBSET['TSD_SHOW_THRT_VIS_THRT']	= counter()
SUBSET['TSD_SHOW_THRT_VIS_OWN']		= counter()
SUBSET['TSD_SHOW_COORD']			= counter()

SUBSET['TSD_COORD']					= counter()
SUBSET['TSD_SHOT']					= counter()
SUBSET['TSD_INST']					= counter()

SUBSET['TSD_BAM_BASE']				= counter()
SUBSET['TSD_BAM_PF']				= counter()
SUBSET['TSD_BAM_PF_ASN']			= counter()
SUBSET['TSD_BAM_PF_OPT']			= counter()
SUBSET['TSD_BAM_PF_ACT']			= counter()
SUBSET['TSD_BAM_PF_ZN']				= counter()
SUBSET['TSD_BAM_PF_RPT_KM']			= counter()
SUBSET['TSD_BAM_NF']				= counter()
SUBSET['TSD_BAM_NF_SEL']			= counter()

SUBSET['TSD_UTIL_BASE']				= counter()
SUBSET['TSD_UTIL_MAIN']				= counter()
SUBSET['TSD_UTIL_ASE']				= counter()
	
SUBSET['TSD_MAP_MAIN']				= counter()
SUBSET['TSD_MAP_CONTOURS']			= counter()
SUBSET['TSD_MAP_BASE']				= counter()
SUBSET['TSD_MAP_ORIENT']			= counter()
SUBSET['TSD_MAP_TYPE']				= counter()
SUBSET['TSD_MAP_COLORBAND']			= counter()
SUBSET['TSD_MAP_FFD']				= counter()
SUBSET['TSD_MAP_SCALE']				= counter()

SUBSET['TSD_RTE']					= counter()
SUBSET['TSD_RTM']					= counter()
SUBSET['TSD_POINT']					= counter()
SUBSET['TSD_ABR']					= counter()

SUBSET['TSD_INST_UTIL']				= counter()

SUBSET['ASE_BASE']					= counter()
SUBSET['ASE']						= counter()
SUBSET['ASE_AUTOPAGE']				= counter()
SUBSET['ASE_UTIL_BASE']				= counter()
SUBSET['ASE_UTIL_MAIN']				= counter()
SUBSET['ASE_UTIL_B_COUNT']			= counter()
SUBSET['ASE_UTIL_B_INTERVAL']		= counter()
SUBSET['ASE_UTIL_S_COUNT']			= counter()
SUBSET['ASE_UTIL_S_INTERVAL']		= counter()
--fuel
SUBSET['AC_FUEL_BASE']				= counter()
SUBSET['AC_FUEL']					= counter()
SUBSET['AC_FUEL_TRANSFER']			= counter()
SUBSET['AC_FUEL_CHECK_BASE']		= counter()
SUBSET['AC_FUEL_CHECK']				= counter()
SUBSET['AC_FUEL_CHECK_TRANSFER']	= counter()
--engine
SUBSET['AC_ENG_BASE']				= counter()
SUBSET['AC_ENG_GROUND']				= counter()
SUBSET['AC_ENG_INFLIGHT']			= counter()
SUBSET['AC_ENG_EMER']				= counter()
SUBSET['AC_ENG_SYS']				= counter()
SUBSET['AC_FLT_BASE']				= counter()
SUBSET['AC_FLT']					= counter()
SUBSET['AC_FLT_SET']				= counter()
SUBSET['AC_PERF']					= counter()
SUBSET['AC_PERF_WT']				= counter()
SUBSET['AC_UTIL']					= counter()

------------COMMUNICATION------------------------
SUBSET['COMM_COM']							= counter()
SUBSET['COMM_COM_IDM']						= counter()
SUBSET['COMM_COM_MAN']						= counter()
SUBSET['COMM_COM_ORIG_ID']					= counter()
SUBSET['COMM_COM_MSG_REC_BASE']				= counter()
SUBSET['COMM_COM_MSG_REC_BASE_TOP_MENU']	= counter()
SUBSET['COMM_COM_MSG_REC']					= counter()
SUBSET['COMM_COM_MSG_REC_DEL_YN']			= counter()

SUBSET['COMM_COM_MSG_REC_REVIEW_BASE']			= counter()
SUBSET['COMM_COM_MSG_REC_REVIEW_BASE_TOP_MENU']	= counter()
SUBSET['COMM_COM_MSG_REC_REVIEW']				= counter()	
SUBSET['COMM_COM_MSG_REC_REVIEW_DEL_YN']		= counter()	

SUBSET['COMM_COM_MSG_SEND']					= counter()
SUBSET['COMM_COM_PRESET']					= counter()
SUBSET['COMM_COM_PRESET_EDIT_UNIT']			= counter()
SUBSET['COMM_COM_PRESET_EDIT_V_UHF']		= counter()
SUBSET['COMM_COM_PRESET_EDIT_FM']			= counter()
SUBSET['COMM_COM_PRESET_EDIT_HF']			= counter()
SUBSET['COMM_COM_PRESET_EDIT_FM1_CNV']		= counter()
SUBSET['COMM_COM_PRESET_EDIT_FM2_CNV']		= counter()
SUBSET['COMM_COM_PRESET_EDIT_UHF_CNV']		= counter()
SUBSET['COMM_COM_PRESET_EDIT_HF_CNV']		= counter()
SUBSET['COMM_COM_MEMBER']					= counter()
SUBSET['COMM_COM_ORIG']						= counter()
SUBSET['COMM_COM_PRIMARY_SELECT']			= counter()

SUBSET['COMM_IDM_FREE_TEXT']				= counter()
SUBSET['COMM_IDM_MPS_TEXT']					= counter()
SUBSET['COMM_IDM_CURR_MISSION']				= counter()
SUBSET['COMM_IDM_CURR_MISSION_ROUTE']		= counter() 

SUBSET['COMM_COM_PRESET_MODEM']				= counter()
SUBSET['COMM_COM_PRESET_MODEM_PROTOCOL']	= counter()
SUBSET['COMM_COM_PRESET_MODEM_RETRIES']		= counter()

SUBSET['COMM_COM_ATHS']				= counter()
SUBSET['COMM_COM_NET']				= counter()
SUBSET['COMM_COM_NET_DELETE_YN']	= counter()
SUBSET['COMM_COM_NET_REPLACE']		= counter()
SUBSET['COMM_SOI']					= counter()
SUBSET['COMM_VHF']					= counter()
SUBSET['COMM_UHF']					= counter()
SUBSET['COMM_UHF_WOD']				= counter()
SUBSET['COMM_UHF_FMT']				= counter()
SUBSET['COMM_UHF_SET']				= counter()
SUBSET['COMM_UHF_CIPHER']			= counter()
SUBSET['COMM_UHF_MODE']				= counter()

SUBSET['COMM_FM']					= counter()
SUBSET['COMM_FM_ERF']				= counter()
SUBSET['COMM_FM_SET']				= counter()
SUBSET['COMM_HF']					= counter()
SUBSET['COMM_HF_SET']				= counter()
SUBSET['COMM_HF_ZERO']				= counter()

SUBSET['COMM_SOI_MSG_SEND']			= counter()
SUBSET['COMM_SOI_SINC']				= counter()
SUBSET['COMM_SOI_HQ2']				= counter()
SUBSET['COMM_SOI_UTIL']				= counter()
SUBSET['COMM_SOI_EXPND']			= counter()
	
SUBSET['COMM_TUNE_VHF']				= counter()
SUBSET['COMM_TUNE_UHF']				= counter()
SUBSET['COMM_TUNE_FM1']				= counter()
SUBSET['COMM_TUNE_FM2']				= counter()
SUBSET['COMM_TUNE_HF']				= counter()
SUBSET['COMM_GUARD_VHF']			= counter()
SUBSET['COMM_GUARD_UHF']			= counter()
SUBSET['COMM_HF_RECV_EMSN']			= counter()
SUBSET['COMM_HF_XMIT_EMSN']			= counter()

-----------VIDEO-------------------------------------
SUBSET['VID']						= counter()
SUBSET['VCR']						= counter()
SUBSET['VID_BASE']					= counter()
SUBSET['VID_BOP_UP']				= counter()
SUBSET['VID_COMMON']				= counter()
SUBSET['VID_COMMON_TOP']			= counter()
SUBSET['VID_CRUISE']				= counter()
SUBSET['VID_HOVER']					= counter()
SUBSET['VID_TRANSITION']			= counter()
SUBSET['VID_WEAPON']				= counter()
SUBSET['VID_FCR_GTM']				= counter()
SUBSET['VID_FCR_RMAP']				= counter()
SUBSET['VID_FCR_ATM']				= counter()
SUBSET['VID_FCR_TPM']				= counter()
SUBSET['VID_GRAYSCALE']				= counter()

SUBSET['MENU_DMS']					= counter()
SUBSET['COMM_XPNDR']				= counter()
SUBSET['COMM_XPNDR_ANT']			= counter()
SUBSET['COMM_XPNDR_REPLY']			= counter()

SUBSET['DMS_WCA']					= counter()
SUBSET['DMS_DTU']					= counter()
SUBSET['DMS_DTU_BASE']				= counter()
SUBSET['DMS_DTU_DATA']				= counter()
SUBSET['DMS_DTU_MISSION']			= counter()
SUBSET['DMS_DTU_COMM']				= counter()
SUBSET['DMS_DTU_LOAD']				= counter()
SUBSET['DMS_DTU_ROUTES']			= counter()
SUBSET['DMS_DTU_STBY']				= counter()
SUBSET['DMS_FAULT']					= counter()
SUBSET['DMS_IBIT_ACFTCOMM']			= counter()
SUBSET['DMS_IBIT_CNTLDSPL']			= counter()
SUBSET['DMS_IBIT_WPNSIGHT']			= counter()
SUBSET['DMS_IBIT_PROCDMS']			= counter()
SUBSET['DMS_IBIT_NAVASE']			= counter()
SUBSET['DMS_IBIT_LISTING']			= counter()
SUBSET['DMS_SHUTDOWN']				= counter()
SUBSET['DMS_VERS']					= counter()
SUBSET['DMS_UTIL']					= counter()
SUBSET['DMS_COMM_RADIOS']			= counter()

SUBSET['FCR_COMMON']				= counter()

SUBSET['WAYPOINTS']					= counter()
SUBSET['CONTROL_MEASURES']			= counter()
SUBSET['TARGETS_THREATS']			= counter()
SUBSET['DEFENSE_ZONES']				= counter()
SUBSET['FCR_CONTACTS']				= counter()
SUBSET['SHOT_AT_OWN']				= counter()
SUBSET['SHOT_AT_IDM']				= counter()
SUBSET['IDM_SUBSCRIBERS']			= counter()

page_subsets = {}

local function makeSubsets_()
	page_subsets[SUBSET.ASTERISK]				= PagesPath.."MPD_ASTERISK.lua"
	page_subsets[SUBSET.FCR_GTM]				= PagesPath.."MISSION/FCR/GTM/MPD_FCR_GTM.lua"
	page_subsets[SUBSET.FCR_RMAP]				= PagesPath.."MISSION/FCR/RMAP/MPD_FCR_RMAP.lua"
	page_subsets[SUBSET.FCR_ATM]				= PagesPath.."MISSION/FCR/ATM/MPD_FCR_ATM.lua"
	page_subsets[SUBSET.FCR_ATM_MENU]			= PagesPath.."MISSION/FCR/ATM/MPD_FCR_ATM_MENU.lua"
	page_subsets[SUBSET.FCR_TPM]				= PagesPath.."MISSION/FCR/TPM/MPD_FCR_TPM.lua"
	page_subsets[SUBSET.FCR_MENU]				= PagesPath.."MISSION/FCR/MPD_FCR_MENU.lua"
	page_subsets[SUBSET.FCR_TGT]				= PagesPath.."MISSION/FCR/MPD_FCR_TGT.lua"
	page_subsets[SUBSET.FCR_RFHO]				= PagesPath.."MISSION/FCR/MPD_FCR_RFHO.lua"
	page_subsets[SUBSET.FCR_ACQ]				= PagesPath.."MISSION/FCR/MPD_FCR_ACQ.lua"
	page_subsets[SUBSET.FCR_TPM_MENU]			= PagesPath.."MISSION/FCR/TPM/MPD_FCR_TPM_MENU.lua"
	page_subsets[SUBSET.FCR_LINE]				= PagesPath.."MISSION/FCR/MPD_FCR_LINE.lua"
	page_subsets[SUBSET.FCR_PROF]				= PagesPath.."MISSION/FCR/MPD_FCR_PROF.lua"
	page_subsets[SUBSET.FCR_CLEARANCE]			= PagesPath.."MISSION/FCR/MPD_FCR_CLEARANCE.lua"
	page_subsets[SUBSET.FCR_COMMON]				= PagesPath.."MISSION/FCR/MPD_FCR_COMMON.lua"
	page_subsets[SUBSET.FCR_STATUS]				= PagesPath.."MISSION/FCR/MPD_FCR_STATUS.lua"
	page_subsets[SUBSET.FCR_UTIL]				= PagesPath.."MISSION/FCR/UTIL/MPD_FCR_UTIL.lua"
	page_subsets[SUBSET.FCR_UTIL_BIT]			= PagesPath.."MISSION/FCR/UTIL/MPD_FCR_UTIL_BIT.lua"
	page_subsets[SUBSET.FCR_UTIL_MISSION]		= PagesPath.."MISSION/FCR/UTIL/MPD_FCR_UTIL_MISSION.lua"

	page_subsets[SUBSET.ASE_BASE]				= PagesPath.."MISSION/ASE/MPD_ASE_BASE.lua"
	page_subsets[SUBSET.ASE]					= PagesPath.."MISSION/ASE/MPD_ASE.lua"
	page_subsets[SUBSET.ASE_AUTOPAGE]			= PagesPath.."MISSION/ASE/MPD_ASE_AUTOPAGE.lua"
	page_subsets[SUBSET.ASE_UTIL_BASE]			= PagesPath.."MISSION/ASE/MPD_ASE_UTIL_BASE.lua"
	page_subsets[SUBSET.ASE_UTIL_MAIN]			= PagesPath.."MISSION/ASE/MPD_ASE_UTIL_MAIN.lua"
	page_subsets[SUBSET.ASE_UTIL_B_COUNT]		= PagesPath.."MISSION/ASE/MPD_ASE_UTIL_CHAFF_BURST_COUNT.lua"
	page_subsets[SUBSET.ASE_UTIL_B_INTERVAL]	= PagesPath.."MISSION/ASE/MPD_ASE_UTIL_CHAFF_BURST_INTERVAL.lua"
	page_subsets[SUBSET.ASE_UTIL_S_COUNT]		= PagesPath.."MISSION/ASE/MPD_ASE_UTIL_CHAFF_SALVO_COUNT.lua"
	page_subsets[SUBSET.ASE_UTIL_S_INTERVAL]	= PagesPath.."MISSION/ASE/MPD_ASE_UTIL_CHAFF_SALVO_INTERVAL.lua"

	page_subsets[SUBSET.AC_FLT_BASE]			= PagesPath.."AC/MPD_AC_FLT_BASE.lua"
	page_subsets[SUBSET.AC_FLT]					= PagesPath.."AC/MPD_AC_FLT.lua"
	page_subsets[SUBSET.AC_FLT_SET]				= PagesPath.."AC/MPD_AC_FLT_SET.lua"
	page_subsets[SUBSET.AC_PERF]				= PagesPath.."AC/MPD_AC_PERF.lua"
	page_subsets[SUBSET.AC_PERF_WT]				= PagesPath.."AC/MPD_AC_PERF_WT.lua"
	page_subsets[SUBSET.AC_UTIL]				= PagesPath.."AC/MPD_AC_UTIL.lua"

	page_subsets[SUBSET.MENU_DMS]				= PagesPath.."DMS/MPD_DMS.lua"
	
	page_subsets[SUBSET.ACQ_MENU]				= PagesPath.."MISSION/TSD/MPD_ACQ_MENU.lua"
end

local function makeSubsets_VID()
	page_subsets[SUBSET.VID]					= PagesPath.."VIDEO/MPD_VID.lua"
	page_subsets[SUBSET.VCR]					= PagesPath.."VIDEO/MPD_VCR.lua"
	page_subsets[SUBSET.VID_BASE]				= PagesPath.."VIDEO/SUBSETS/VID_BASE.lua"
	page_subsets[SUBSET.VID_COMMON]				= PagesPath.."VIDEO/SUBSETS/VID_COMMON.lua"
	page_subsets[SUBSET.VID_COMMON_TOP]			= PagesPath.."VIDEO/SUBSETS/VID_COMMON_TOP.lua"
	page_subsets[SUBSET.VID_HOVER]				= PagesPath.."VIDEO/SUBSETS/VID_HOVER.lua"
	page_subsets[SUBSET.VID_BOP_UP]				= PagesPath.."VIDEO/SUBSETS/VID_BOB_UP.lua"
	page_subsets[SUBSET.VID_TRANSITION]			= PagesPath.."VIDEO/SUBSETS/VID_TRANSITION.lua"
	page_subsets[SUBSET.VID_CRUISE]				= PagesPath.."VIDEO/SUBSETS/VID_CRUISE.lua"
	page_subsets[SUBSET.VID_WEAPON]				= PagesPath.."VIDEO/SUBSETS/VID_WEAPONS.lua"
	page_subsets[SUBSET.VID_FCR_GTM]			= PagesPath.."VIDEO/SUBSETS/VID_FCR_GTM.lua"
	page_subsets[SUBSET.VID_FCR_RMAP]			= PagesPath.."VIDEO/SUBSETS/VID_FCR_RMAP.lua"
	page_subsets[SUBSET.VID_FCR_ATM]			= PagesPath.."VIDEO/SUBSETS/VID_FCR_ATM.lua"
	page_subsets[SUBSET.VID_FCR_TPM]			= PagesPath.."VIDEO/SUBSETS/VID_FCR_TPM.lua"
	page_subsets[SUBSET.VID_GRAYSCALE]			= PagesPath.."VIDEO/SUBSETS/VID_GRAYSCALE.lua"
end
-------------------------------------------------------------------------------------------------
local function makeSubsets_WPN()
	page_subsets[SUBSET.WPN_BASE]				= PagesPath.."MISSION/WPN/MPD_WPN_BASE.lua"
	page_subsets[SUBSET.WPN_BASE_MENU]			= PagesPath.."MISSION/WPN/MPD_WPN_BASE_MENU.lua"
	page_subsets[SUBSET.WPN_BASE_MENU_UPDOWN]	= PagesPath.."MISSION/WPN/MPD_WPN_BASE_MENU_UPDOWN.lua"
	page_subsets[SUBSET.WPN_TRAIN_MODE]			= PagesPath.."MISSION/WPN/MPD_WPN_TRAIN_MODE.lua"

	page_subsets[SUBSET.WPN_MAIN]				= PagesPath.."MISSION/WPN/MPD_WPN_MAIN.lua"
	page_subsets[SUBSET.WPN_MENU_ACQ]			= PagesPath.."MISSION/WPN/MPD_WPN_BASE_MENU_ACQ.lua"

	page_subsets[SUBSET.WPN_GUN_BASE]			= PagesPath.."MISSION/WPN/MPD_WPN_GUN_BASE.lua"
	page_subsets[SUBSET.WPN_GUN_BASE_MENU]		= PagesPath.."MISSION/WPN/MPD_WPN_GUN_BASE_MENU.lua"

	page_subsets[SUBSET.WPN_MSL_SAL_BASE]		= PagesPath.."MISSION/WPN/MPD_WPN_MSL_SAL_BASE.lua"
	page_subsets[SUBSET.WPN_MSL_SAL_BASE_MENU_L]= PagesPath.."MISSION/WPN/MPD_WPN_MSL_SAL_BASE_MENU_L.lua"
	page_subsets[SUBSET.WPN_MSL_SAL_BASE_MENU_R]= PagesPath.."MISSION/WPN/MPD_WPN_MSL_SAL_BASE_MENU_R.lua"
	page_subsets[SUBSET.WPN_MSL_SAL_MODE_MENU]	= PagesPath.."MISSION/WPN/MPD_WPN_MSL_SAL_MODE_MENU.lua"
	page_subsets[SUBSET.WPN_MSL_SAL_TRAJ_MENU]	= PagesPath.."MISSION/WPN/MPD_WPN_MSL_SAL_TRAJ_MENU.lua"
	page_subsets[SUBSET.WPN_MSL_SAL_SSEL_MENU]	= PagesPath.."MISSION/WPN/MPD_WPN_MSL_SAL_SSEL_MENU.lua"
	page_subsets[SUBSET.WPN_MSL_SAL_PRI_MENU]	= PagesPath.."MISSION/WPN/MPD_WPN_MSL_SAL_PRI_MENU.lua"
	page_subsets[SUBSET.WPN_MSL_SAL_ALT_MENU]	= PagesPath.."MISSION/WPN/MPD_WPN_MSL_SAL_ALT_MENU.lua"

	page_subsets[SUBSET.WPN_MSL_RF_BASE]		= PagesPath.."MISSION/WPN/MPD_WPN_MSL_RF_BASE.lua"
	page_subsets[SUBSET.WPN_MSL_RF_BASE_MENU]	= PagesPath.."MISSION/WPN/MPD_WPN_MSL_RF_BASE_MENU.lua"

	page_subsets[SUBSET.WPN_RKT_BASE]			= PagesPath.."MISSION/WPN/MPD_WPN_RKT_BASE.lua"
	page_subsets[SUBSET.WPN_RKT_BASE_MENU]		= PagesPath.."MISSION/WPN/MPD_WPN_RKT_BASE_MENU.lua"
	page_subsets[SUBSET.WPN_RKT_PEN]			= PagesPath.."MISSION/WPN/MPD_WPN_RKT_PEN.lua"
	page_subsets[SUBSET.WPN_RKT_QTY]			= PagesPath.."MISSION/WPN/MPD_WPN_RKT_QTY.lua"

	page_subsets[SUBSET.WPN_CHAN]				= PagesPath.."MISSION/WPN/MPD_WPN_CHAN.lua"
	page_subsets[SUBSET.WPN_CODE]				= PagesPath.."MISSION/WPN/MPD_WPN_CODE.lua"
	page_subsets[SUBSET.WPN_FREQ]				= PagesPath.."MISSION/WPN/MPD_WPN_FREQ.lua"

	page_subsets[SUBSET.WPN_RKT_INV]			= PagesPath.."MISSION/WPN/MPD_WPN_RKT_INV.lua"
	page_subsets[SUBSET.WPN_BORESIGHT]			= PagesPath.."MISSION/WPN/MPD_WPN_BORESIGHT.lua"
	page_subsets[SUBSET.WPN_LOAD]				= PagesPath.."MISSION/WPN/MPD_WPN_LOAD.lua"
	page_subsets[SUBSET.WPN_UTIL]				= PagesPath.."MISSION/WPN/MPD_WPN_UTIL.lua"
	page_subsets[SUBSET.WPN_PLT_EOCCM_UTIL]		= PagesPath.."MISSION/WPN/MPD_WPN_PLT_EOCCM_UTIL.lua"
end
-------------------------------------------------------------------------------------------------
local function makeSubsets_TSD()
	page_subsets[SUBSET.TSD_MAIN_BASE]			= PagesPath.."MISSION/TSD/MPD_TSD_MAIN_BASE.lua"
	page_subsets[SUBSET.TSD_MAIN]				= PagesPath.."MISSION/TSD/MPD_TSD_MAIN.lua"
	page_subsets[SUBSET.TSD_MAIN_ACQ]			= PagesPath.."MISSION/TSD/MPD_TSD_MAIN_ACQ.lua"
	page_subsets[SUBSET.TSD_MAIN_REC]			= PagesPath.."MISSION/TSD/MPD_TSD_MAIN_REC.lua"

	page_subsets[SUBSET.TSD_VIDEOSIGNAL]		= PagesPath.."MISSION/TSD/MPD_TSD_Underlay_Subset.lua"
	page_subsets[SUBSET.TSD_COMMON_SYMBS]		= PagesPath.."MISSION/TSD/MPD_TSD_CommonSymbology_Subset.lua"

	page_subsets[SUBSET.TSD_RPT_BASE]			= PagesPath.."MISSION/TSD/MPD_TSD_RPT_BASE.lua"
	page_subsets[SUBSET.TSD_RPT_MAIN]			= PagesPath.."MISSION/TSD/MPD_TSD_RPT_MAIN.lua"
	page_subsets[SUBSET.TSD_RPT_STAT]			= PagesPath.."MISSION/TSD/MPD_TSD_RPT_STAT.lua"
	page_subsets[SUBSET.TSD_RPT_BDA]			= PagesPath.."MISSION/TSD/MPD_TSD_RPT_BDA.lua"
	page_subsets[SUBSET.TSD_RPT_TGT]			= PagesPath.."MISSION/TSD/MPD_TSD_RPT_TGT.lua"
	page_subsets[SUBSET.TSD_RPT_PP]				= PagesPath.."MISSION/TSD/MPD_TSD_RPT_PP.lua"
	page_subsets[SUBSET.TSD_RPT_FARM]			= PagesPath.."MISSION/TSD/MPD_TSD_RPT_FARM.lua"
	
	page_subsets[SUBSET.TSD_FARM_BASE]			= PagesPath.."MISSION/TSD/MPD_TSD_FARM_BASE.lua"
	page_subsets[SUBSET.TSD_FARM_MAIN]			= PagesPath.."MISSION/TSD/MPD_TSD_FARM.lua"
	page_subsets[SUBSET.TSD_FARM_TYPE]			= PagesPath.."MISSION/TSD/MPD_TSD_FARM_TYPE.lua"

	page_subsets[SUBSET.TSD_PAN_BASE]			= PagesPath.."MISSION/TSD/MPD_TSD_PAN_BASE.lua"
	page_subsets[SUBSET.TSD_PAN_2D]				= PagesPath.."MISSION/TSD/MPD_TSD_PAN_2D.lua"
	page_subsets[SUBSET.TSD_PAN_3D]				= PagesPath.."MISSION/TSD/MPD_TSD_PAN_3D.lua"

	page_subsets[SUBSET.TSD_SHOW_BASE]			= PagesPath.."MISSION/TSD/MPD_TSD_SHOW_BASE.lua"
	page_subsets[SUBSET.TSD_SHOW_MAIN]			= PagesPath.."MISSION/TSD/MPD_TSD_SHOW_MAIN.lua"
	page_subsets[SUBSET.TSD_SHOW_SA]			= PagesPath.."MISSION/TSD/MPD_TSD_SHOW_SA.lua"
	page_subsets[SUBSET.TSD_SHOW_THRT_VIS_THRT]	= PagesPath.."MISSION/TSD/MPD_TSD_SHOW_THRT_VIS_THRT.lua"
	page_subsets[SUBSET.TSD_SHOW_THRT_VIS_OWN]	= PagesPath.."MISSION/TSD/MPD_TSD_SHOW_THRT_VIS_OWN.lua"
	page_subsets[SUBSET.TSD_SHOW_COORD]			= PagesPath.."MISSION/TSD/MPD_TSD_SHOW_COORD.lua"
	page_subsets[SUBSET.TSD_COORD]				= PagesPath.."MISSION/TSD/MPD_TSD_COORD.lua"
	page_subsets[SUBSET.TSD_SHOT]				= PagesPath.."MISSION/TSD/MPD_TSD_SHOT.lua"
	page_subsets[SUBSET.TSD_INST]				= PagesPath.."MISSION/TSD/MPD_TSD_INST.lua"

	page_subsets[SUBSET.TSD_BAM_BASE]			= PagesPath.."MISSION/TSD/MPD_TSD_BAM_BASE.lua"
	page_subsets[SUBSET.TSD_BAM_PF]				= PagesPath.."MISSION/TSD/MPD_TSD_BAM_PF.lua"
	page_subsets[SUBSET.TSD_BAM_PF_ASN]			= PagesPath.."MISSION/TSD/MPD_TSD_BAM_PF_ASN.lua"
	page_subsets[SUBSET.TSD_BAM_PF_OPT]			= PagesPath.."MISSION/TSD/MPD_TSD_BAM_PF_OPT.lua"
	page_subsets[SUBSET.TSD_BAM_PF_ACT]			= PagesPath.."MISSION/TSD/MPD_TSD_BAM_PF_ACT.lua"
	page_subsets[SUBSET.TSD_BAM_PF_ZN]			= PagesPath.."MISSION/TSD/MPD_TSD_BAM_PF_ZN.lua"
	page_subsets[SUBSET.TSD_BAM_PF_RPT_KM]		= PagesPath.."MISSION/TSD/MPD_TSD_BAM_PF_RPT_KM.lua"
	page_subsets[SUBSET.TSD_BAM_NF]				= PagesPath.."MISSION/TSD/MPD_TSD_BAM_NF.lua"
	page_subsets[SUBSET.TSD_BAM_NF_SEL]			= PagesPath.."MISSION/TSD/MPD_TSD_BAM_NF_SEL.lua"

	page_subsets[SUBSET.TSD_UTIL_BASE]			= PagesPath.."MISSION/TSD/MPD_TSD_UTIL_BASE.lua"
	page_subsets[SUBSET.TSD_UTIL_MAIN]			= PagesPath.."MISSION/TSD/MPD_TSD_UTIL_MAIN.lua"
	page_subsets[SUBSET.TSD_UTIL_ASE]			= PagesPath.."MISSION/TSD/MPD_TSD_UTIL_ASE.lua"

	page_subsets[SUBSET.TSD_MAP_MAIN]			= PagesPath.."MISSION/TSD/MPD_TSD_MAP_MAIN.lua"
	page_subsets[SUBSET.TSD_MAP_CONTOURS]		= PagesPath.."MISSION/TSD/MPD_TSD_MAP_CONTOURS.lua"
	page_subsets[SUBSET.TSD_MAP_BASE]			= PagesPath.."MISSION/TSD/MPD_TSD_MAP_BASE.lua"
	page_subsets[SUBSET.TSD_MAP_ORIENT]			= PagesPath.."MISSION/TSD/MPD_TSD_MAP_ORIENT.lua"
	page_subsets[SUBSET.TSD_MAP_TYPE]			= PagesPath.."MISSION/TSD/MPD_TSD_MAP_TYPE.lua"
	page_subsets[SUBSET.TSD_MAP_COLORBAND]		= PagesPath.."MISSION/TSD/MPD_TSD_MAP_COLORBAND.lua"
	page_subsets[SUBSET.TSD_MAP_FFD]			= PagesPath.."MISSION/TSD/MPD_TSD_MAP_FFD.lua"
	page_subsets[SUBSET.TSD_MAP_SCALE]			= PagesPath.."MISSION/TSD/MPD_TSD_MAP_SCALE.lua"

	page_subsets[SUBSET.TSD_RTE]				= PagesPath.."MISSION/TSD/MPD_TSD_RTE.lua"
	page_subsets[SUBSET.TSD_RTM]				= PagesPath.."MISSION/TSD/MPD_TSD_RTM.lua"
	page_subsets[SUBSET.TSD_POINT]				= PagesPath.."MISSION/TSD/MPD_TSD_POINT.lua"
	page_subsets[SUBSET.TSD_ABR]				= PagesPath.."MISSION/TSD/MPD_TSD_ABR.lua"

	page_subsets[SUBSET.TSD_INST_UTIL]			= PagesPath.."MISSION/TSD/MPD_TSD_INST_UTIL.lua"
end
-------------------------------------------------------------------------------------------------
local function makeSubsets_COMM()
	page_subsets[SUBSET.COMM_COM]						= PagesPath.."COM/MPD_COM.lua"
	page_subsets[SUBSET.COMM_COM_IDM]					= PagesPath.."COM/MPD_COM_IDM.lua"
	page_subsets[SUBSET.COMM_COM_MAN]					= PagesPath.."COM/MPD_COM_MAN.lua"
	page_subsets[SUBSET.COMM_COM_ORIG_ID]				= PagesPath.."COM/MPD_COM_ORIG_ID.lua"
	
	page_subsets[SUBSET.COMM_COM_MSG_REC_BASE]			= PagesPath.."COM/MPD_COM_MSG_REC_BASE.lua"
	page_subsets[SUBSET.COMM_COM_MSG_REC_BASE_TOP_MENU]	= PagesPath.."COM/MPD_COM_MSG_REC_BASE_TOP_MENU.lua"
	page_subsets[SUBSET.COMM_COM_MSG_REC]				= PagesPath.."COM/MPD_COM_MSG_REC.lua"
	page_subsets[SUBSET.COMM_COM_MSG_REC_DEL_YN]		= PagesPath.."COM/MPD_COM_MSG_REC_DEL_YN.lua"
	
	page_subsets[SUBSET.COMM_COM_MSG_REC_REVIEW_BASE]			= PagesPath.."COM/MPD_COM_MSG_REC_REVIEW_BASE.lua"
	page_subsets[SUBSET.COMM_COM_MSG_REC_REVIEW_BASE_TOP_MENU]	= PagesPath.."COM/MPD_COM_MSG_REC_REVIEW_BASE_TOP_MENU.lua"
	page_subsets[SUBSET.COMM_COM_MSG_REC_REVIEW]				= PagesPath.."COM/MPD_COM_MSG_REC_REVIEW.lua"
	page_subsets[SUBSET.COMM_COM_MSG_REC_REVIEW_DEL_YN]			= PagesPath.."COM/MPD_COM_MSG_REC_REVIEW_DEL_YN.lua"
	
	page_subsets[SUBSET.COMM_COM_MSG_SEND]				= PagesPath.."COM/MPD_COM_MSG_SEND.lua"
	page_subsets[SUBSET.COMM_COM_PRESET]				= PagesPath.."COM/MPD_COM_PRESET.lua"
	page_subsets[SUBSET.COMM_COM_MEMBER]				= PagesPath.."COM/MPD_COM_MEMBER.lua"
	page_subsets[SUBSET.COMM_COM_ORIG]					= PagesPath.."COM/MPD_COM_ORIG.lua"
	page_subsets[SUBSET.COMM_COM_PRIMARY_SELECT]		= PagesPath.."COM/MPD_COM_PRIMARY_SELECT.lua"
	
	page_subsets[SUBSET.COMM_IDM_FREE_TEXT]		= PagesPath.."COM/MPD_COM_IDM_FREE_TEXT.lua"
	page_subsets[SUBSET.COMM_IDM_MPS_TEXT]		= PagesPath.."COM/MPD_COM_IDM_MPS_TEXT.lua"
	page_subsets[SUBSET.COMM_IDM_CURR_MISSION]	= PagesPath.."COM/MPD_COM_CURRENT_MISSION.lua"
	page_subsets[SUBSET.COMM_IDM_CURR_MISSION_ROUTE]	= PagesPath.."COM/MPD_COM_CURRENT_MISSION_ROUTE.lua"
	
	page_subsets[SUBSET.COMM_COM_PRESET_EDIT_UNIT]	= PagesPath.."COM/MPD_COM_PRESET_EDIT_UNIT.lua"
	page_subsets[SUBSET.COMM_COM_PRESET_EDIT_V_UHF]	= PagesPath.."COM/MPD_COM_PRESET_EDIT_V_UHF.lua"
	page_subsets[SUBSET.COMM_COM_PRESET_EDIT_FM]	= PagesPath.."COM/MPD_COM_PRESET_EDIT_FM.lua"
	page_subsets[SUBSET.COMM_COM_PRESET_EDIT_HF]	= PagesPath.."COM/MPD_COM_PRESET_EDIT_HF.lua"
	
	page_subsets[SUBSET.COMM_COM_PRESET_EDIT_FM1_CNV]	= PagesPath.."COM/MPD_COM_PRESET_EDIT_FM1_CNV.lua"
	page_subsets[SUBSET.COMM_COM_PRESET_EDIT_FM2_CNV]	= PagesPath.."COM/MPD_COM_PRESET_EDIT_FM2_CNV.lua"
	page_subsets[SUBSET.COMM_COM_PRESET_EDIT_UHF_CNV]	= PagesPath.."COM/MPD_COM_PRESET_EDIT_UHF_CNV.lua"
	page_subsets[SUBSET.COMM_COM_PRESET_EDIT_HF_CNV]	= PagesPath.."COM/MPD_COM_PRESET_EDIT_HF_CNV.lua"
	
	page_subsets[SUBSET.COMM_COM_PRESET_MODEM]	= PagesPath.."COM/MPD_COM_PRESET_MODEM.lua"
	page_subsets[SUBSET.COMM_COM_PRESET_MODEM_PROTOCOL]	= PagesPath.."COM/MPD_COM_PRESET_MODEM_PROTOCOL.lua"
	page_subsets[SUBSET.COMM_COM_PRESET_MODEM_RETRIES]	= PagesPath.."COM/MPD_COM_PRESET_MODEM_RETRIES.lua"
	
	page_subsets[SUBSET.COMM_COM_ATHS]			= PagesPath.."COM/MPD_COM_ATHS.lua"
	page_subsets[SUBSET.COMM_COM_NET]			= PagesPath.."COM/MPD_COM_NET.lua"
	page_subsets[SUBSET.COMM_COM_NET_DELETE_YN]	= PagesPath.."COM/MPD_COM_NET_DEL_YN.lua"
	page_subsets[SUBSET.COMM_COM_NET_REPLACE]	= PagesPath.."COM/MPD_COM_NET_REPLACE.lua"
	page_subsets[SUBSET.COMM_SOI]				= PagesPath.."COM/MPD_COM_SOI.lua"
	page_subsets[SUBSET.COMM_VHF]				= PagesPath.."COM/MPD_COM_VHF.lua"
	page_subsets[SUBSET.COMM_UHF]				= PagesPath.."COM/MPD_COM_UHF.lua"
	page_subsets[SUBSET.COMM_UHF_CIPHER]		= PagesPath.."COM/MPD_COM_UHF_CIPHER.lua"
	page_subsets[SUBSET.COMM_UHF_MODE]			= PagesPath.."COM/MPD_COM_UHF_MODE.lua"
	page_subsets[SUBSET.COMM_UHF_WOD]			= PagesPath.."COM/MPD_COM_UHF_WOD.lua"
	page_subsets[SUBSET.COMM_UHF_FMT]			= PagesPath.."COM/MPD_COM_UHF_FMT.lua"
	page_subsets[SUBSET.COMM_UHF_SET]			= PagesPath.."COM/MPD_COM_UHF_SET.lua"
	page_subsets[SUBSET.COMM_FM]				= PagesPath.."COM/MPD_COM_FM.lua"
	page_subsets[SUBSET.COMM_FM_ERF]			= PagesPath.."COM/MPD_COM_FM_ERF.lua"
	page_subsets[SUBSET.COMM_FM_SET]			= PagesPath.."COM/MPD_COM_FM_SET.lua"
	page_subsets[SUBSET.COMM_HF]				= PagesPath.."COM/MPD_COM_HF.lua"
	page_subsets[SUBSET.COMM_HF_SET]			= PagesPath.."COM/MPD_COM_HF_SET.lua"
	page_subsets[SUBSET.COMM_HF_ZERO]			= PagesPath.."COM/MPD_COM_HF_ZERO.lua"

	page_subsets[SUBSET.COMM_SOI_MSG_SEND]		= PagesPath.."COM/MPD_COM_SOI_MSG_SEND.lua"
	page_subsets[SUBSET.COMM_SOI_SINC]			= PagesPath.."COM/MPD_COM_SOI_SINC.lua"
	page_subsets[SUBSET.COMM_SOI_HQ2]			= PagesPath.."COM/MPD_COM_SOI_HQ2.lua"
	page_subsets[SUBSET.COMM_SOI_UTIL]			= PagesPath.."COM/MPD_COM_SOI_UTIL.lua"
	page_subsets[SUBSET.COMM_SOI_EXPND]			= PagesPath.."COM/MPD_COM_SOI_EXPND.lua"
	page_subsets[SUBSET.COMM_XPNDR]				= PagesPath.."COM/MPD_COM_XPNDR.lua"
	page_subsets[SUBSET.COMM_XPNDR_ANT]			= PagesPath.."COM/MPD_COM_XPNDR_ANT.lua"
	page_subsets[SUBSET.COMM_XPNDR_REPLY]		= PagesPath.."COM/MPD_COM_XPNDR_REPLY.lua"
	
	page_subsets[SUBSET.COMM_TUNE_VHF]			= PagesPath.."COM/MPD_COM_TUNE_VHF.lua"
	page_subsets[SUBSET.COMM_TUNE_UHF]			= PagesPath.."COM/MPD_COM_TUNE_UHF.lua"
	page_subsets[SUBSET.COMM_TUNE_FM1]			= PagesPath.."COM/MPD_COM_TUNE_FM1.lua"
	page_subsets[SUBSET.COMM_TUNE_FM2]			= PagesPath.."COM/MPD_COM_TUNE_FM2.lua"
	page_subsets[SUBSET.COMM_TUNE_HF]			= PagesPath.."COM/MPD_COM_TUNE_HF.lua"
	page_subsets[SUBSET.COMM_GUARD_VHF]			= PagesPath.."COM/MPD_COM_GUARD_VHF.lua"
	page_subsets[SUBSET.COMM_GUARD_UHF]			= PagesPath.."COM/MPD_COM_GUARD_UHF.lua"
	page_subsets[SUBSET.COMM_HF_RECV_EMSN]		= PagesPath.."COM/MPD_COM_HF_RECV_EMSN.lua"
	page_subsets[SUBSET.COMM_HF_XMIT_EMSN]		= PagesPath.."COM/MPD_COM_HF_XMIT_EMSN.lua"
end
-------------------------------------------------------------------------------------------------
local function makeSubsets_Eng()
	page_subsets[SUBSET.AC_ENG_BASE]			= PagesPath.."AC/MPD_AC_ENG_BASE.lua"
	page_subsets[SUBSET.AC_ENG_GROUND]			= PagesPath.."AC/MPD_AC_ENG_GROUND.lua"
	page_subsets[SUBSET.AC_ENG_INFLIGHT]		= PagesPath.."AC/MPD_AC_ENG_INFLIGHT.lua"
	page_subsets[SUBSET.AC_ENG_EMER]			= PagesPath.."AC/MPD_AC_ENG_EMER.lua"
	page_subsets[SUBSET.AC_ENG_SYS]				= PagesPath.."AC/MPD_AC_ENG_SYS.lua"
end
---------------------------------------------------------------------------------------------------
local function makeSubsets_Fuel()
	page_subsets[SUBSET.AC_FUEL_BASE]			= PagesPath.."AC/MPD_AC_FUEL_BASE.lua"
	page_subsets[SUBSET.AC_FUEL]				= PagesPath.."AC/MPD_AC_FUEL.lua"
	page_subsets[SUBSET.AC_FUEL_TRANSFER]		= PagesPath.."AC/MPD_AC_FUEL_TRANSFER.lua"
	page_subsets[SUBSET.AC_FUEL_CHECK_BASE]		= PagesPath.."AC/MPD_AC_FUEL_CHECK_BASE.lua"
	page_subsets[SUBSET.AC_FUEL_CHECK]			= PagesPath.."AC/MPD_AC_FUEL_CHECK.lua"
	page_subsets[SUBSET.AC_FUEL_CHECK_TRANSFER]	= PagesPath.."AC/MPD_AC_FUEL_CHECK_TRANSFER.lua"
end
---------------------------------------------------------------------------------------------------
local function makeSubsets_Common()
	page_subsets[SUBSET.BASE]					= PagesPath.."MPD_base.lua"
	page_subsets[SUBSET.BASE_TSD]				= PagesPath.."MPD_base.lua"
	page_subsets[SUBSET.BASE_TOP]				= PagesPath.."MPD_base_top.lua"
	page_subsets[SUBSET.MENU]					= PagesPath.."MPD_MENU.lua"
	page_subsets[SUBSET.BLANK]					= PagesPath.."MPD_BLANK.lua"
	page_subsets[SUBSET.INIT]					= PagesPath.."MPD_INIT.lua"
end
---------------------------------------------------------------------------------------------------
local function makeSubsets_DMS()
	page_subsets[SUBSET.DMS_WCA]				= PagesPath.."DMS/MPD_DMS_WCA.lua"
	page_subsets[SUBSET.DMS_DTU]				= PagesPath.."DMS/MPD_DMS_DTU.lua"
	page_subsets[SUBSET.DMS_DTU_BASE]			= PagesPath.."DMS/MPD_DMS_DTU_BASE.lua"
	page_subsets[SUBSET.DMS_DTU_DATA]			= PagesPath.."DMS/MPD_DMS_DTU_DATA.lua"
	page_subsets[SUBSET.DMS_DTU_MISSION]		= PagesPath.."DMS/MPD_DMS_DTU_DATA_MISSION.lua"
	page_subsets[SUBSET.DMS_DTU_COMM]			= PagesPath.."DMS/MPD_DMS_DTU_DATA_COMMUNICATION.lua"
	page_subsets[SUBSET.DMS_DTU_LOAD]			= PagesPath.."DMS/MPD_DMS_DTU_LOAD.lua"
	page_subsets[SUBSET.DMS_DTU_ROUTES]			= PagesPath.."DMS/MPD_DMS_DTU_ROUTES.lua"
	page_subsets[SUBSET.DMS_DTU_STBY]			= PagesPath.."DMS/MPD_DMS_DTU_STBY.lua"
	page_subsets[SUBSET.DMS_FAULT]				= PagesPath.."DMS/MPD_DMS_FAULT.lua"
	page_subsets[SUBSET.DMS_IBIT_ACFTCOMM]		= PagesPath.."DMS/MPD_DMS_IBIT_ACFTCOMM.lua"
	page_subsets[SUBSET.DMS_IBIT_CNTLDSPL]		= PagesPath.."DMS/MPD_DMS_IBIT_CNTLDSPL.lua"
	page_subsets[SUBSET.DMS_IBIT_WPNSIGHT]		= PagesPath.."DMS/MPD_DMS_IBIT_WPNSIGHT.lua"
	page_subsets[SUBSET.DMS_IBIT_PROCDMS]		= PagesPath.."DMS/MPD_DMS_IBIT_PROCDMS.lua"
	page_subsets[SUBSET.DMS_IBIT_NAVASE]		= PagesPath.."DMS/MPD_DMS_IBIT_NAVASE.lua"
	page_subsets[SUBSET.DMS_IBIT_LISTING]		= PagesPath.."DMS/MPD_DMS_IBIT_LISTING.lua"
	page_subsets[SUBSET.DMS_SHUTDOWN]			= PagesPath.."DMS/MPD_DMS_SHUTDOWN.lua"
	page_subsets[SUBSET.DMS_VERS]				= PagesPath.."DMS/MPD_DMS_VERS.lua"
	page_subsets[SUBSET.DMS_UTIL]				= PagesPath.."DMS/MPD_DMS_UTIL.lua"
	page_subsets[SUBSET.DMS_COMM_RADIOS]		= PagesPath.."DMS/MPD_DMS_IBIT_COMM_RADIOS.lua"
end
---------------------------------------------------------------------------------------------------
local function makeSubsets_MultiSymbolsTemps()
	page_subsets[SUBSET.WAYPOINTS]				= PagesPath.."MISSION/TSD/DynamicDataPresets/MPD_TSD_Waypoints.lua"
	page_subsets[SUBSET.CONTROL_MEASURES]		= PagesPath.."MISSION/TSD/DynamicDataPresets/MPD_TSD_ControlMeasures.lua"
	page_subsets[SUBSET.TARGETS_THREATS]		= PagesPath.."MISSION/TSD/DynamicDataPresets/MPD_TSD_TargetsThreats.lua"
	page_subsets[SUBSET.DEFENSE_ZONES]			= PagesPath.."MISSION/TSD/DynamicDataPresets/MPD_TSD_ThreatsVisRings.lua"
	page_subsets[SUBSET.FCR_CONTACTS]			= PagesPath.."MISSION/TSD/DynamicDataPresets/MPD_TSD_FCR_Targets.lua"
	page_subsets[SUBSET.SHOT_AT_OWN]			= PagesPath.."MISSION/TSD/DynamicDataPresets/MPD_TSD_ShotAt_Own.lua"
	page_subsets[SUBSET.SHOT_AT_IDM]			= PagesPath.."MISSION/TSD/DynamicDataPresets/MPD_TSD_ShotAt_Idm.lua"
	page_subsets[SUBSET.IDM_SUBSCRIBERS]		= PagesPath.."MISSION/TSD/DynamicDataPresets/MPD_TSD_IDM_Subscribers.lua"
end
---------------------------------------------------------------------------------------------------

local function makeSubsets()
	makeSubsets_VID()
	makeSubsets_DMS()
	makeSubsets_Common()
	makeSubsets_Fuel()
	makeSubsets_Eng()
	makeSubsets_WPN()
	makeSubsets_TSD()
	makeSubsets_COMM()
	makeSubsets_()
	makeSubsets_MultiSymbolsTemps()
end

makeSubsets()

--------------------------------------------------------------------------------------------------
-- PAGES -----------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------
resetCounter()

PAGE_NONE				= counter()
PAGE_BLANK				= counter()
PAGE_STANDBY			= counter()
PAGE_INIT				= counter()
PAGE_MENU				= counter()
-- ---------------mission------------------------
PAGE_FCR_GTM			= counter()
PAGE_FCR_RMAP			= counter()
PAGE_FCR_ATM			= counter()
PAGE_FCR_TPM			= counter()
PAGE_FCR_UTIL			= counter()
PAGE_FCR_UTIL_BIT		= counter()
PAGE_FCR_UTIL_MISSIOM	= counter()

PAGE_WPN_MAIN			= counter()
PAGE_WPN_MAIN_ACQ		= counter()

PAGE_WPN_GUN			= counter()
PAGE_WPN_GUN_ACQ		= counter()

PAGE_WPN_MSL_SAL		= counter()
PAGE_WPN_MSL_SAL_ACQ	= counter()
PAGE_WPN_MSL_SAL_MODE	= counter()
PAGE_WPN_MSL_SAL_TRAJ	= counter()
PAGE_WPN_MSL_SAL_SSEL	= counter()
PAGE_WPN_MSL_SAL_PRI	= counter()
PAGE_WPN_MSL_SAL_ALT	= counter()
PAGE_WPN_MSL_RF			= counter()
PAGE_WPN_MSL_RF_ACQ		= counter()

PAGE_WPN_RKT			= counter()
PAGE_WPN_RKT_ACQ		= counter()
PAGE_WPN_RKT_PEN		= counter()
PAGE_WPN_RKT_QTY		= counter()

PAGE_WPN_CHAN			= counter()
PAGE_WPN_CODE			= counter()
PAGE_WPN_FREQ			= counter()

PAGE_WPN_LOAD_RKT_INV	= counter()
PAGE_WPN_BORESIGHT		= counter()

PAGE_WPN_UTIL			= counter()
PAGE_WPN_PLT_EOCCM_UTIL	= counter()
PAGE_WPN_UTIL_LOAD		= counter()


PAGE_TSD				= counter()
PAGE_TSD_ACQ			= counter()
PAGE_TSD_REC			= counter()

PAGE_TSD_RPT_MAIN		= counter()
PAGE_TSD_RPT_STAT		= counter()
PAGE_TSD_RPT_BDA		= counter()
PAGE_TSD_RPT_TGT		= counter()
PAGE_TSD_RPT_PP			= counter()
PAGE_TSD_RPT_FARM		= counter()

PAGE_TSD_PAN_2D			= counter()
PAGE_TSD_PAN_3D			= counter()

PAGE_TSD_SHOW_MAIN			= counter()
PAGE_TSD_SHOW_SA			= counter()
PAGE_TSD_SHOW_THRT_VIS_THRT	= counter()
PAGE_TSD_SHOW_THRT_VIS_OWN	= counter()
PAGE_TSD_SHOW_COORD			= counter()

PAGE_TSD_COORD			= counter()
PAGE_TSD_SHOT			= counter()
PAGE_TSD_FARM			= counter()
PAGE_TSD_FARM_TYPE		= counter()

PAGE_TSD_UTIL_MAIN		= counter()
PAGE_TSD_UTIL_ASE		= counter()

PAGE_TSD_BAM_PF			= counter()
PAGE_TSD_BAM_PF_ASN		= counter()
PAGE_TSD_BAM_PF_OPT		= counter()
PAGE_TSD_BAM_PF_ACT		= counter()
PAGE_TSD_BAM_PF_ZN		= counter()
PAGE_TSD_BAM_PF_RPT_KM	= counter()
PAGE_TSD_BAM_NF			= counter()
PAGE_TSD_BAM_NF_SEL		= counter()

PAGE_TSD_MAP_MAIN		= counter()
PAGE_TSD_MAP_CONTOURS	= counter()
PAGE_TSD_MAP_ORIENT		= counter()
PAGE_TSD_MAP_TYPE		= counter()
PAGE_TSD_MAP_COLORBAND	= counter()
PAGE_TSD_MAP_FFD		= counter()
PAGE_TSD_MAP_SCALE		= counter()

PAGE_TSD_RTE			= counter()
PAGE_TSD_RTM			= counter()
PAGE_TSD_POINT			= counter()
PAGE_TSD_ABR			= counter()

PAGE_TSD_INST			= counter()
PAGE_TSD_INST_UTIL		= counter()

PAGE_ASE						= counter()
PAGE_ASE_AUTOPAGE				= counter()
PAGE_ASE_UTIL_MAIN				= counter()
PAGE_ASE_UTIL_BURST_COUNT		= counter()
PAGE_ASE_UTIL_BURST_INTERVAL	= counter()
PAGE_ASE_UTIL_SALVO_COUNT		= counter()
PAGE_ASE_UTIL_SALVO_INTERVAL	= counter()

--fuel
PAGE_AC_FUEL				= counter()
PAGE_AC_FUEL_TRANSFER		= counter()

PAGE_AC_FUEL_CHECK_BASE		= counter()
PAGE_AC_FUEL_CHECK			= counter()
PAGE_AC_FUEL_CHECK_TRANSFER	= counter()

--engine
PAGE_AC_ENG				= counter()
PAGE_AC_ENG_BASE		= counter()
PAGE_AC_ENG_GROUND		= counter()
PAGE_AC_ENG_INFLIGHT	= counter()
PAGE_AC_ENG_EMER		= counter()
PAGE_AC_ENG_SYS			= counter()
PAGE_AC_FLT				= counter()
PAGE_AC_FLT_SET			= counter()
PAGE_AC_PERF			= counter()
PAGE_AC_PERF_WT			= counter()
PAGE_AC_UTIL			= counter()

-- ---------------COMMUNICATION------------------------
PAGE_COMM_COM						= counter()
PAGE_COMM_COM_IDM					= counter()
PAGE_COMM_COM_MAN					= counter()
PAGE_COMM_COM_ORIG_ID				= counter()
PAGE_COMM_COM_MSG_REC				= counter()
PAGE_COMM_COM_MSG_REC_DEL_YN		= counter()
PAGE_COMM_COM_MSG_REC_REVIEW		= counter()
PAGE_COMM_COM_MSG_REC_REVIEW_DEL_YN	= counter()
PAGE_COMM_COM_MSG_SEND				= counter()
PAGE_COMM_COM_PRESET				= counter()
PAGE_COMM_COM_MEMBER				= counter()
PAGE_COMM_COM_ORIG					= counter()
PAGE_COMM_PRIMARY_SELECT			= counter()

PAGE_COMM_IDM_FREE_TEXT			= counter()
PAGE_COMM_IDM_MPS_TEXT			= counter()		
PAGE_COMM_IDM_CURR_MISSION		= counter()
PAGE_COMM_IDM_CURR_MISSION_ROUTE	= counter()

PAGE_COMM_COM_PRESET_EDIT_UNIT	= counter()	
PAGE_COMM_COM_PRESET_EDIT_V_UHF	= counter()	
PAGE_COMM_COM_PRESET_EDIT_FM	= counter()		
PAGE_COMM_COM_PRESET_EDIT_HF	= counter()	

PAGE_COMM_COM_PRESET_EDIT_FM1_CNV	= counter()
PAGE_COMM_COM_PRESET_EDIT_FM2_CNV	= counter()
PAGE_COMM_COM_PRESET_EDIT_UHF_CNV	= counter()
PAGE_COMM_COM_PRESET_EDIT_HF_CNV	= counter()

PAGE_COMM_COM_MODEM				= counter()
PAGE_COMM_COM_MODEM_PROTOCOL	= counter()
PAGE_COMM_COM_MODEM_RETRIES		= counter()
	
PAGE_COMM_COM_ATHS		= counter()
PAGE_COMM_COM_NET		= counter()
PAGE_COMM_COM_NET_DELETE_YN	= counter()
PAGE_COMM_COM_NET_REPLACE	= counter()
PAGE_COMM_SOI			= counter()
PAGE_COMM_VHF			= counter()
PAGE_COMM_UHF			= counter()
PAGE_COMM_UHF_MODE		= counter()
PAGE_COMM_UHF_CIPHER	= counter()
PAGE_COMM_UHF_WOD		= counter()
PAGE_COMM_UHF_FMT		= counter()
PAGE_COMM_UHF_SET		= counter()
PAGE_COMM_FM			= counter()
PAGE_COMM_FM_ERF		= counter()
PAGE_COMM_FM_SET		= counter()
PAGE_COMM_HF			= counter()
PAGE_COMM_HF_SET		= counter()
PAGE_COMM_HF_ZERO		= counter()

PAGE_COMM_SOI_MSG_SEND	= counter()
PAGE_COMM_SOI_SINC		= counter()
PAGE_COMM_SOI_HQ2		= counter()
PAGE_COMM_SOI_UTIL		= counter()
PAGE_COMM_SOI_EXPND		= counter()
PAGE_COMM_XPNDR			= counter()
PAGE_COMM_XPNDR_ANT		= counter()
PAGE_COMM_XPNDR_REPLY	= counter()

PAGE_COMM_TUNE_VHF		= counter()
PAGE_COMM_TUNE_UHF		= counter()
PAGE_COMM_TUNE_FM1		= counter()
PAGE_COMM_TUNE_FM2		= counter()
PAGE_COMM_TUNE_HF		= counter()
PAGE_COMM_GUARD_VHF		= counter()
PAGE_COMM_GUARD_UHF		= counter()
PAGE_COMM_HF_RECV_EMSN	= counter()
PAGE_COMM_HF_XMIT_EMSN	= counter()
---------VIDEO--------------------------------
PAGE_VID				= counter()
PAGE_VCR				= counter()
PAGE_VID_EMPTY			= counter()
PAGE_VID_BOP_UP			= counter()
PAGE_VID_CRUISE			= counter()
PAGE_VID_HOVER			= counter()
PAGE_VID_TRANSITION		= counter()
PAGE_VID_WEAPON			= counter()
PAGE_VID_FCR_GTM		= counter()
PAGE_VID_FCR_RMAP		= counter()
PAGE_VID_FCR_ATM		= counter()
PAGE_VID_FCR_TPM		= counter()
PAGE_VID_GRAYSCALE		= counter()

PAGE_ASTERISK			= counter()

PAGE_DMS				= counter()

PAGE_DMS_WCA			= counter()
PAGE_DMS_DTU			= counter()
PAGE_DMS_DTU_DATA		= counter()
PAGE_DMS_DTU_MISSION	= counter()
PAGE_DMS_DTU_COMM		= counter()
PAGE_DMS_DTU_LOAD		= counter()
PAGE_DMS_DTU_ROUTES		= counter()
PAGE_DMS_DTU_STBY		= counter()
PAGE_DMS_FAULT			= counter()
PAGE_DMS_IBIT_ACFTCOMM	= counter()
PAGE_DMS_IBIT_CNTLDSPL	= counter()
PAGE_DMS_IBIT_WPNSIGHT	= counter()
PAGE_DMS_IBIT_PROCDMS	= counter()
PAGE_DMS_IBIT_NAVASE	= counter()
PAGE_DMS_IBIT_LISTING	= counter()
PAGE_DMS_SHUTDOWN		= counter()
PAGE_DMS_VERS			= counter()
PAGE_DMS_UTIL			= counter()
PAGE_DMS_COMM_RADIOS	= counter()


pages = {}

local function makePages_DMS()
	pages[PAGE_DMS]					= {SUBSET.BASE, SUBSET.MENU_DMS,			SUBSET.BASE_TOP}

	pages[PAGE_DMS_WCA]				= {SUBSET.BASE, SUBSET.DMS_WCA,				SUBSET.BASE_TOP}

	pages[PAGE_DMS_DTU]				= {SUBSET.BASE, SUBSET.DMS_DTU,				SUBSET.BASE_TOP, SUBSET.DMS_DTU_BASE}
	pages[PAGE_DMS_DTU_DATA]		= {SUBSET.BASE, SUBSET.DMS_DTU_DATA,		SUBSET.BASE_TOP}
	pages[PAGE_DMS_DTU_MISSION]		= {SUBSET.BASE, SUBSET.DMS_DTU_MISSION,		SUBSET.BASE_TOP, SUBSET.DMS_DTU_BASE}
	pages[PAGE_DMS_DTU_COMM]		= {SUBSET.BASE, SUBSET.DMS_DTU_COMM,		SUBSET.BASE_TOP, SUBSET.DMS_DTU_BASE}
	pages[PAGE_DMS_DTU_LOAD]		= {SUBSET.BASE, SUBSET.DMS_DTU_LOAD,		SUBSET.BASE_TOP}
	pages[PAGE_DMS_DTU_ROUTES]		= {SUBSET.BASE, SUBSET.DMS_DTU_ROUTES,		SUBSET.BASE_TOP}
	pages[PAGE_DMS_DTU_STBY]		= {SUBSET.BASE, SUBSET.DMS_DTU_STBY,		SUBSET.BASE_TOP}
	pages[PAGE_DMS_FAULT]			= {SUBSET.BASE, SUBSET.DMS_FAULT,			SUBSET.BASE_TOP}
	pages[PAGE_DMS_IBIT_ACFTCOMM]	= {SUBSET.BASE, SUBSET.DMS_IBIT_ACFTCOMM,	SUBSET.BASE_TOP}
	pages[PAGE_DMS_IBIT_CNTLDSPL]	= {SUBSET.BASE, SUBSET.DMS_IBIT_CNTLDSPL,	SUBSET.BASE_TOP}
	pages[PAGE_DMS_IBIT_WPNSIGHT]	= {SUBSET.BASE, SUBSET.DMS_IBIT_WPNSIGHT,	SUBSET.BASE_TOP}
	pages[PAGE_DMS_IBIT_PROCDMS]	= {SUBSET.BASE, SUBSET.DMS_IBIT_PROCDMS,	SUBSET.BASE_TOP}
	pages[PAGE_DMS_IBIT_NAVASE]		= {SUBSET.BASE, SUBSET.DMS_IBIT_NAVASE,		SUBSET.BASE_TOP}
	pages[PAGE_DMS_IBIT_LISTING]	= {SUBSET.BASE, SUBSET.DMS_IBIT_LISTING,	SUBSET.BASE_TOP}
	pages[PAGE_DMS_SHUTDOWN]		= {SUBSET.BASE, SUBSET.DMS_SHUTDOWN,		SUBSET.BASE_TOP}
	pages[PAGE_DMS_VERS]			= {SUBSET.BASE, SUBSET.DMS_VERS,			SUBSET.BASE_TOP}
	pages[PAGE_DMS_UTIL]			= {SUBSET.BASE, SUBSET.DMS_UTIL,			SUBSET.BASE_TOP}
	pages[PAGE_DMS_COMM_RADIOS]		= {SUBSET.BASE, SUBSET.DMS_COMM_RADIOS,		SUBSET.BASE_TOP}

end

local function makePages_ASE()
	pages[PAGE_ASE]						= {SUBSET.BASE, SUBSET.ASE_BASE,		SUBSET.ASE,					SUBSET.BASE_TOP}
	pages[PAGE_ASE_AUTOPAGE]			= {SUBSET.BASE, SUBSET.ASE_BASE,		SUBSET.ASE_AUTOPAGE,		SUBSET.BASE_TOP}
	pages[PAGE_ASE_UTIL_MAIN]			= {SUBSET.BASE, SUBSET.ASE_UTIL_BASE,	SUBSET.ASE_UTIL_MAIN,		SUBSET.BASE_TOP}
	pages[PAGE_ASE_UTIL_BURST_COUNT]	= {SUBSET.BASE, SUBSET.ASE_UTIL_BASE,	SUBSET.ASE_UTIL_B_COUNT,	SUBSET.BASE_TOP}
	pages[PAGE_ASE_UTIL_BURST_INTERVAL]	= {SUBSET.BASE, SUBSET.ASE_UTIL_BASE,	SUBSET.ASE_UTIL_B_INTERVAL,	SUBSET.BASE_TOP}
	pages[PAGE_ASE_UTIL_SALVO_COUNT]	= {SUBSET.BASE, SUBSET.ASE_UTIL_BASE,	SUBSET.ASE_UTIL_S_COUNT,	SUBSET.BASE_TOP}
	pages[PAGE_ASE_UTIL_SALVO_INTERVAL]	= {SUBSET.BASE, SUBSET.ASE_UTIL_BASE,	SUBSET.ASE_UTIL_S_INTERVAL,	SUBSET.BASE_TOP}
end

local function makePages_FCR()
	pages[PAGE_FCR_GTM]				= {SUBSET.BASE, SUBSET.FCR_GTM, 			SUBSET.FCR_COMMON,			SUBSET.FCR_MENU,		SUBSET.FCR_TGT,	 		SUBSET.FCR_RFHO, SUBSET.FCR_ACQ,	SUBSET.FCR_STATUS,		SUBSET.BASE_TOP}
	pages[PAGE_FCR_RMAP]			= {SUBSET.BASE, SUBSET.FCR_RMAP, 			SUBSET.FCR_COMMON,			SUBSET.FCR_MENU,		SUBSET.FCR_TGT,	 		SUBSET.FCR_RFHO, SUBSET.FCR_ACQ,	SUBSET.FCR_STATUS,		SUBSET.BASE_TOP}
	pages[PAGE_FCR_ATM]				= {SUBSET.BASE, SUBSET.FCR_ATM, 			SUBSET.FCR_COMMON,			SUBSET.FCR_ATM_MENU,	SUBSET.FCR_TGT,	 		SUBSET.FCR_RFHO, SUBSET.FCR_ACQ,	SUBSET.FCR_STATUS,		SUBSET.BASE_TOP}
	pages[PAGE_FCR_TPM]				= {SUBSET.BASE, SUBSET.FCR_TPM, 			SUBSET.FCR_COMMON,			SUBSET.FCR_TPM_MENU,  	SUBSET.FCR_LINE,	 	SUBSET.FCR_PROF, SUBSET.FCR_CLEARANCE,	SUBSET.FCR_STATUS,			SUBSET.BASE_TOP}
	pages[PAGE_FCR_UTIL]			= {SUBSET.BASE, SUBSET.FCR_UTIL, 										SUBSET.FCR_STATUS,		SUBSET.BASE_TOP}
	pages[PAGE_FCR_UTIL_BIT]		= {SUBSET.BASE, SUBSET.FCR_UTIL_BIT, 									SUBSET.FCR_STATUS,		SUBSET.BASE_TOP}
	pages[PAGE_FCR_UTIL_MISSIOM]	= {SUBSET.BASE, SUBSET.FCR_UTIL_MISSION,								SUBSET.FCR_STATUS,		SUBSET.BASE_TOP}
end

local function makePages_WPN()
	pages[PAGE_WPN_MAIN]			= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_MAIN,		SUBSET.WPN_BASE_MENU_UPDOWN,	SUBSET.WPN_BASE_MENU,	SUBSET.WPN_TRAIN_MODE,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_MAIN_ACQ]		= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_MAIN,		SUBSET.WPN_MENU_ACQ,			SUBSET.ACQ_MENU,		SUBSET.BASE_TOP}

	pages[PAGE_WPN_GUN]				= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_GUN_BASE,	SUBSET.WPN_BASE_MENU_UPDOWN,	SUBSET.WPN_BASE_MENU,	SUBSET.WPN_GUN_BASE_MENU,	SUBSET.WPN_TRAIN_MODE,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_GUN_ACQ]			= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_GUN_BASE,	SUBSET.WPN_MENU_ACQ,			SUBSET.ACQ_MENU,		SUBSET.BASE_TOP}

	pages[PAGE_WPN_MSL_SAL]			= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_BASE_MENU,		SUBSET.WPN_BASE_MENU_UPDOWN,	SUBSET.WPN_MSL_SAL_BASE,		SUBSET.WPN_MSL_SAL_BASE_MENU_L,	SUBSET.WPN_MSL_SAL_BASE_MENU_R,	SUBSET.WPN_TRAIN_MODE,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_MSL_SAL_MODE]	= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_BASE_MENU,		SUBSET.WPN_BASE_MENU_UPDOWN,	SUBSET.WPN_MSL_SAL_BASE,		SUBSET.WPN_MSL_SAL_BASE_MENU_L,	SUBSET.WPN_MSL_SAL_MODE_MENU,	SUBSET.WPN_TRAIN_MODE,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_MSL_SAL_TRAJ]	= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_BASE_MENU,		SUBSET.WPN_BASE_MENU_UPDOWN,	SUBSET.WPN_MSL_SAL_BASE,		SUBSET.WPN_MSL_SAL_BASE_MENU_L,	SUBSET.WPN_MSL_SAL_TRAJ_MENU,	SUBSET.WPN_TRAIN_MODE,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_MSL_SAL_SSEL]	= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_BASE_MENU,		SUBSET.WPN_BASE_MENU_UPDOWN,	SUBSET.WPN_MSL_SAL_BASE,		SUBSET.WPN_MSL_SAL_BASE_MENU_R,	SUBSET.WPN_MSL_SAL_SSEL_MENU,	SUBSET.WPN_TRAIN_MODE,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_MSL_SAL_PRI]		= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_BASE_MENU,		SUBSET.WPN_BASE_MENU_UPDOWN,	SUBSET.WPN_MSL_SAL_BASE_MENU_R,	SUBSET.WPN_MSL_SAL_PRI_MENU,	SUBSET.WPN_TRAIN_MODE,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_MSL_SAL_ALT]		= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_BASE_MENU,		SUBSET.WPN_BASE_MENU_UPDOWN,	SUBSET.WPN_MSL_SAL_BASE_MENU_R,	SUBSET.WPN_MSL_SAL_ALT_MENU,	SUBSET.WPN_TRAIN_MODE,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_MSL_SAL_ACQ]		= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_MSL_SAL_BASE,	SUBSET.WPN_MENU_ACQ,			SUBSET.WPN_MSL_SAL_BASE_MENU_L,	SUBSET.ACQ_MENU,										SUBSET.BASE_TOP}
	pages[PAGE_WPN_MSL_RF]			= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_MSL_RF_BASE,	SUBSET.WPN_BASE_MENU_UPDOWN,	SUBSET.WPN_BASE_MENU,			SUBSET.WPN_MSL_RF_BASE_MENU,	SUBSET.WPN_TRAIN_MODE,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_MSL_RF_ACQ]		= {SUBSET.BASE, SUBSET.WPN_BASE, SUBSET.WPN_MSL_RF_BASE,	SUBSET.WPN_MENU_ACQ,			SUBSET.ACQ_MENU,				SUBSET.BASE_TOP}

	pages[PAGE_WPN_RKT]				= {SUBSET.BASE, SUBSET.WPN_BASE,	SUBSET.WPN_BASE_MENU,	SUBSET.WPN_BASE_MENU_UPDOWN,	SUBSET.WPN_RKT_BASE,		SUBSET.WPN_RKT_BASE_MENU,		SUBSET.WPN_TRAIN_MODE,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_RKT_ACQ]			= {SUBSET.BASE, SUBSET.WPN_BASE,	SUBSET.WPN_RKT_BASE,	SUBSET.WPN_MENU_ACQ,			SUBSET.ACQ_MENU,			SUBSET.BASE_TOP}
	pages[PAGE_WPN_RKT_PEN]			= {SUBSET.BASE, SUBSET.WPN_RKT_PEN, SUBSET.WPN_BASE_MENU_UPDOWN,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_RKT_QTY]			= {SUBSET.BASE, SUBSET.WPN_BASE,	SUBSET.WPN_RKT_QTY,				SUBSET.BASE_TOP}

	pages[PAGE_WPN_CHAN]			= {SUBSET.BASE, SUBSET.WPN_CHAN, SUBSET.BASE_TOP}
	pages[PAGE_WPN_CODE]			= {SUBSET.BASE, SUBSET.WPN_CODE, SUBSET.BASE_TOP}
	pages[PAGE_WPN_FREQ]			= {SUBSET.BASE, SUBSET.WPN_FREQ, SUBSET.BASE_TOP}

	pages[PAGE_WPN_BORESIGHT]		= {SUBSET.BASE, SUBSET.WPN_BORESIGHT, SUBSET.BASE_TOP}

	pages[PAGE_WPN_UTIL]			= {SUBSET.BASE, SUBSET.WPN_UTIL,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_PLT_EOCCM_UTIL]	= {SUBSET.BASE, SUBSET.WPN_PLT_EOCCM_UTIL,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_UTIL_LOAD]		= {SUBSET.BASE, SUBSET.WPN_LOAD,	SUBSET.BASE_TOP}
	pages[PAGE_WPN_LOAD_RKT_INV]	= {SUBSET.BASE, SUBSET.WPN_RKT_INV,	SUBSET.BASE_TOP}
end

local function makePages_TSD()
	pages[PAGE_TSD]					= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_MAIN_BASE,	SUBSET.TSD_MAIN,		SUBSET.BASE_TOP}
	pages[PAGE_TSD_ACQ]				= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_MAIN_BASE,	SUBSET.TSD_MAIN_ACQ,	SUBSET.ACQ_MENU,	SUBSET.BASE_TOP}
	pages[PAGE_TSD_REC]				= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_MAIN_BASE,	SUBSET.TSD_MAIN_REC,	SUBSET.BASE_TOP}

	pages[PAGE_TSD_RPT_MAIN]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_RPT_BASE, SUBSET.TSD_RPT_MAIN,	SUBSET.BASE_TOP}
	pages[PAGE_TSD_RPT_STAT]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_RPT_BASE, SUBSET.TSD_RPT_STAT,	SUBSET.BASE_TOP}
	pages[PAGE_TSD_RPT_BDA]			= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_RPT_BASE, SUBSET.TSD_RPT_BDA,	SUBSET.BASE_TOP}
	pages[PAGE_TSD_RPT_TGT]			= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_RPT_BASE, SUBSET.TSD_RPT_TGT,	SUBSET.BASE_TOP}
	pages[PAGE_TSD_RPT_PP]			= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_RPT_BASE, SUBSET.TSD_RPT_PP,	SUBSET.BASE_TOP}
	pages[PAGE_TSD_RPT_FARM]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_RPT_BASE, SUBSET.TSD_RPT_FARM,	SUBSET.BASE_TOP}

	pages[PAGE_TSD_PAN_2D]			= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_PAN_BASE, SUBSET.TSD_PAN_2D, SUBSET.BASE_TOP}
	pages[PAGE_TSD_PAN_3D]			= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_PAN_BASE, SUBSET.TSD_PAN_3D, SUBSET.BASE_TOP}

	pages[PAGE_TSD_SHOW_MAIN]			= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_SHOW_BASE, SUBSET.TSD_SHOW_MAIN,			SUBSET.BASE_TOP}
	pages[PAGE_TSD_SHOW_SA]				= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_SHOW_BASE, SUBSET.TSD_SHOW_SA,				SUBSET.BASE_TOP}
	pages[PAGE_TSD_SHOW_THRT_VIS_THRT]	= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_SHOW_BASE, SUBSET.TSD_SHOW_THRT_VIS_THRT,	SUBSET.BASE_TOP}
	pages[PAGE_TSD_SHOW_THRT_VIS_OWN]	= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_SHOW_BASE, SUBSET.TSD_SHOW_THRT_VIS_OWN,	SUBSET.BASE_TOP}

	pages[PAGE_TSD_SHOW_COORD]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_SHOW_BASE, SUBSET.TSD_SHOW_COORD,	SUBSET.BASE_TOP}

	pages[PAGE_TSD_BAM_PF]			= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_BAM_BASE, SUBSET.TSD_BAM_PF,		SUBSET.BASE_TOP}
	pages[PAGE_TSD_BAM_PF_ASN]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_BAM_BASE, SUBSET.TSD_BAM_PF_ASN,	SUBSET.BASE_TOP}
	pages[PAGE_TSD_BAM_PF_OPT]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_BAM_BASE, SUBSET.TSD_BAM_PF_OPT,	SUBSET.BASE_TOP}
	pages[PAGE_TSD_BAM_PF_ACT]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_BAM_BASE, SUBSET.TSD_BAM_PF_ACT,	SUBSET.BASE_TOP}
	pages[PAGE_TSD_BAM_PF_ZN]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_BAM_BASE, SUBSET.TSD_BAM_PF_ZN,		SUBSET.BASE_TOP}
	pages[PAGE_TSD_BAM_PF_RPT_KM]	= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_BAM_BASE, SUBSET.TSD_BAM_PF_RPT_KM,	SUBSET.BASE_TOP}
	pages[PAGE_TSD_BAM_NF]			= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_BAM_BASE, SUBSET.TSD_BAM_NF,		SUBSET.BASE_TOP}
	pages[PAGE_TSD_BAM_NF_SEL]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_BAM_BASE, SUBSET.TSD_BAM_NF_SEL,	SUBSET.BASE_TOP}

	pages[PAGE_TSD_MAP_CONTOURS]	= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_MAP_CONTOURS, SUBSET.BASE_TOP}
	pages[PAGE_TSD_MAP_MAIN]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_MAP_BASE, SUBSET.TSD_MAP_MAIN, SUBSET.BASE_TOP}
	pages[PAGE_TSD_MAP_ORIENT]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_MAP_BASE, SUBSET.TSD_MAP_ORIENT, SUBSET.BASE_TOP}
	pages[PAGE_TSD_MAP_TYPE]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_MAP_BASE, SUBSET.TSD_MAP_TYPE, SUBSET.BASE_TOP}
	pages[PAGE_TSD_MAP_COLORBAND]	= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_MAP_BASE, SUBSET.TSD_MAP_COLORBAND, SUBSET.BASE_TOP}
	pages[PAGE_TSD_MAP_FFD]			= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_MAP_BASE, SUBSET.TSD_MAP_FFD, SUBSET.BASE_TOP}
	pages[PAGE_TSD_MAP_SCALE]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_MAP_BASE, SUBSET.TSD_MAP_SCALE, SUBSET.BASE_TOP}

	pages[PAGE_TSD_RTE]				= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_RTE, SUBSET.BASE_TOP}
	pages[PAGE_TSD_RTM]				= {SUBSET.BASE_TSD, SUBSET.TSD_RTM, SUBSET.BASE_TOP}
	pages[PAGE_TSD_POINT]			= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_POINT, SUBSET.BASE_TOP}
	pages[PAGE_TSD_ABR]				= {SUBSET.BASE_TSD, SUBSET.TSD_ABR, SUBSET.BASE_TOP}

	pages[PAGE_TSD_INST]			= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_INST, SUBSET.BASE_TOP}
	pages[PAGE_TSD_INST_UTIL]		= {SUBSET.BASE_TSD, SUBSET.TSD_VIDEOSIGNAL, SUBSET.TSD_COMMON_SYMBS, SUBSET.TSD_INST_UTIL, SUBSET.BASE_TOP}

	pages[PAGE_TSD_UTIL_MAIN]		= {SUBSET.BASE_TSD, SUBSET.TSD_UTIL_BASE, SUBSET.TSD_UTIL_MAIN, SUBSET.BASE_TOP}
	pages[PAGE_TSD_UTIL_ASE]		= {SUBSET.BASE_TSD, SUBSET.TSD_UTIL_BASE, SUBSET.TSD_UTIL_ASE, SUBSET.BASE_TOP}
	
	pages[PAGE_TSD_COORD]			= {SUBSET.BASE_TSD, SUBSET.TSD_COORD, SUBSET.BASE_TOP}
	pages[PAGE_TSD_SHOT]			= {SUBSET.BASE_TSD, SUBSET.TSD_SHOT, SUBSET.BASE_TOP}
	pages[PAGE_TSD_FARM]			= {SUBSET.BASE_TSD, SUBSET.TSD_FARM_BASE, SUBSET.TSD_FARM_MAIN,	SUBSET.BASE_TOP}
	pages[PAGE_TSD_FARM_TYPE]		= {SUBSET.BASE_TSD, SUBSET.TSD_FARM_BASE, SUBSET.TSD_FARM_TYPE,	SUBSET.BASE_TOP}

end

local function makePages_ENG()
	pages[PAGE_AC_ENG_BASE]			= {SUBSET.BASE, SUBSET.AC_ENG_BASE, SUBSET.BASE_TOP }
	pages[PAGE_AC_ENG_GROUND]		= {SUBSET.BASE, SUBSET.AC_ENG_BASE, SUBSET.AC_ENG_GROUND, SUBSET.BASE_TOP }
	pages[PAGE_AC_ENG_INFLIGHT]		= {SUBSET.BASE, SUBSET.AC_ENG_BASE, SUBSET.AC_ENG_INFLIGHT, SUBSET.BASE_TOP}
	pages[PAGE_AC_ENG_EMER]			= {SUBSET.BASE, SUBSET.AC_ENG_BASE, SUBSET.AC_ENG_EMER, SUBSET.BASE_TOP}
	pages[PAGE_AC_ENG_SYS]			= {SUBSET.BASE, SUBSET.AC_ENG_SYS,  SUBSET.BASE_TOP}
end

local function makePages_AC()
	pages[PAGE_AC_FUEL]				= {SUBSET.BASE, SUBSET.AC_FUEL_BASE, SUBSET.AC_FUEL, SUBSET.BASE_TOP}
	pages[PAGE_AC_FUEL_TRANSFER]	= {SUBSET.BASE, SUBSET.AC_FUEL_BASE, SUBSET.AC_FUEL_TRANSFER, SUBSET.BASE_TOP}

	pages[PAGE_AC_FUEL_CHECK]			= {SUBSET.BASE, SUBSET.AC_FUEL_BASE, SUBSET.AC_FUEL_CHECK_BASE, SUBSET.AC_FUEL_CHECK, SUBSET.BASE_TOP}
	pages[PAGE_AC_FUEL_CHECK_TRANSFER]	= {SUBSET.BASE, SUBSET.AC_FUEL_BASE, SUBSET.AC_FUEL_CHECK_BASE, SUBSET.AC_FUEL_CHECK_TRANSFER, SUBSET.BASE_TOP}

	pages[PAGE_AC_FLT]				= {SUBSET.BASE, SUBSET.AC_FLT_BASE, SUBSET.AC_FLT, SUBSET.BASE_TOP}
	pages[PAGE_AC_FLT_SET]			= {SUBSET.BASE, SUBSET.AC_FLT_BASE, SUBSET.AC_FLT_SET, SUBSET.BASE_TOP}
	pages[PAGE_AC_PERF]				= {SUBSET.BASE, SUBSET.AC_PERF, SUBSET.BASE_TOP}
	pages[PAGE_AC_PERF_WT]			= {SUBSET.BASE, SUBSET.AC_PERF_WT, SUBSET.BASE_TOP}
	pages[PAGE_AC_UTIL]				= {SUBSET.BASE, SUBSET.AC_UTIL, SUBSET.BASE_TOP}

	makePages_ENG()
end

local function makePages_VID()
	pages[PAGE_VID]					= {SUBSET.BASE,		SUBSET.VID,				SUBSET.BASE_TOP}
	pages[PAGE_VCR]					= {SUBSET.BASE,		SUBSET.VCR,				SUBSET.BASE_TOP}
	pages[PAGE_VID_EMPTY]			= {SUBSET.BASE,		SUBSET.VID_BASE,		SUBSET.BASE_TOP}
	pages[PAGE_VID_HOVER]			= {SUBSET.BASE,		SUBSET.VID_BASE,		SUBSET.VID_COMMON,	SUBSET.VID_HOVER,	SUBSET.VID_COMMON_TOP,	SUBSET.BASE_TOP}
	pages[PAGE_VID_BOP_UP]			= {SUBSET.BASE,		SUBSET.VID_BASE,		SUBSET.VID_COMMON,	SUBSET.VID_HOVER,	SUBSET.VID_BOP_UP,		SUBSET.VID_COMMON_TOP,	SUBSET.BASE_TOP}
	pages[PAGE_VID_TRANSITION]		= {SUBSET.BASE,		SUBSET.VID_BASE,		SUBSET.VID_COMMON,	SUBSET.VID_HOVER,	SUBSET.VID_TRANSITION,	SUBSET.VID_COMMON_TOP,	SUBSET.BASE_TOP}
	pages[PAGE_VID_CRUISE]			= {SUBSET.BASE,		SUBSET.VID_BASE,		SUBSET.VID_COMMON,	SUBSET.VID_HOVER,	SUBSET.VID_TRANSITION,	SUBSET.VID_CRUISE,	SUBSET.VID_COMMON_TOP,	SUBSET.BASE_TOP}
	pages[PAGE_VID_WEAPON]			= {SUBSET.BASE,		SUBSET.VID_BASE,		SUBSET.VID_COMMON,	SUBSET.VID_WEAPON,  SUBSET.VID_COMMON_TOP,	SUBSET.BASE_TOP}
	pages[PAGE_VID_FCR_GTM]			= {SUBSET.BASE,		SUBSET.FCR_COMMON,		SUBSET.VID_FCR_GTM,  	SUBSET.FCR_STATUS,	SUBSET.VID_COMMON_TOP,	SUBSET.BASE_TOP}
	pages[PAGE_VID_FCR_RMAP]		= {SUBSET.BASE,		SUBSET.FCR_COMMON,		SUBSET.VID_FCR_RMAP,  	SUBSET.FCR_STATUS,	SUBSET.VID_COMMON_TOP,	SUBSET.BASE_TOP}
	pages[PAGE_VID_FCR_ATM]			= {SUBSET.BASE,		SUBSET.FCR_COMMON,		SUBSET.VID_FCR_ATM,  	SUBSET.FCR_STATUS,	SUBSET.VID_COMMON_TOP,	SUBSET.BASE_TOP}
	pages[PAGE_VID_FCR_TPM]			= {SUBSET.BASE,		SUBSET.FCR_COMMON,		SUBSET.VID_FCR_TPM,  	SUBSET.FCR_STATUS,	SUBSET.VID_COMMON_TOP,	SUBSET.BASE_TOP}
	pages[PAGE_VID_GRAYSCALE]		= {SUBSET.BASE,		SUBSET.VID_GRAYSCALE,	SUBSET.BASE_TOP}
end

local function makePages_ASTERISK()
	pages[PAGE_ASTERISK]			= {SUBSET.BASE, SUBSET['ASTERISK'], SUBSET.BASE_TOP}
end

local function makePages_COM()
	pages[PAGE_COMM_COM]						= {SUBSET.BASE, SUBSET.COMM_COM, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_IDM]					= {SUBSET.BASE, SUBSET.COMM_COM_IDM, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_MAN]					= {SUBSET.BASE, SUBSET.COMM_COM_MAN, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_ORIG_ID]				= {SUBSET.BASE, SUBSET.COMM_COM_ORIG_ID, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_MSG_REC]				= {SUBSET.BASE, SUBSET.COMM_COM_MSG_REC_BASE, SUBSET.COMM_COM_MSG_REC_BASE_TOP_MENU, SUBSET.COMM_COM_MSG_REC, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_MSG_REC_DEL_YN]			= {SUBSET.BASE, SUBSET.COMM_COM_MSG_REC_BASE, SUBSET.COMM_COM_MSG_REC_DEL_YN, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_MSG_REC_REVIEW]			= {SUBSET.BASE, SUBSET.COMM_COM_MSG_REC_REVIEW_BASE, SUBSET.COMM_COM_MSG_REC_REVIEW_BASE_TOP_MENU,SUBSET.COMM_COM_MSG_REC_REVIEW, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_MSG_REC_REVIEW_DEL_YN]	= {SUBSET.BASE, SUBSET.COMM_COM_MSG_REC_REVIEW_BASE, SUBSET.COMM_COM_MSG_REC_REVIEW_DEL_YN, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_MSG_SEND]				= {SUBSET.BASE, SUBSET.COMM_COM_MSG_SEND, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_PRESET]					= {SUBSET.BASE, SUBSET.COMM_COM_PRESET, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_MEMBER]					= {SUBSET.BASE, SUBSET.COMM_COM_MEMBER, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_ORIG]					= {SUBSET.BASE, SUBSET.COMM_COM_ORIG, SUBSET.BASE_TOP}
	pages[PAGE_COMM_PRIMARY_SELECT]				= {SUBSET.BASE, SUBSET.COMM_COM_PRIMARY_SELECT, SUBSET.BASE_TOP}
	
	pages[PAGE_COMM_IDM_FREE_TEXT]			= {SUBSET.BASE, SUBSET.COMM_IDM_FREE_TEXT, SUBSET.BASE_TOP}
	pages[PAGE_COMM_IDM_MPS_TEXT]			= {SUBSET.BASE, SUBSET.COMM_IDM_MPS_TEXT, SUBSET.BASE_TOP}
	pages[PAGE_COMM_IDM_CURR_MISSION]		= {SUBSET.BASE, SUBSET.COMM_IDM_CURR_MISSION, SUBSET.BASE_TOP}
	pages[PAGE_COMM_IDM_CURR_MISSION_ROUTE]	= {SUBSET.BASE, SUBSET.COMM_IDM_CURR_MISSION_ROUTE, SUBSET.BASE_TOP}
	
	pages[PAGE_COMM_COM_PRESET_EDIT_UNIT]	= {SUBSET.BASE, SUBSET.COMM_COM_PRESET_EDIT_UNIT, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_PRESET_EDIT_V_UHF]	= {SUBSET.BASE, SUBSET.COMM_COM_PRESET_EDIT_V_UHF, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_PRESET_EDIT_FM]		= {SUBSET.BASE, SUBSET.COMM_COM_PRESET_EDIT_FM, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_PRESET_EDIT_HF]		= {SUBSET.BASE, SUBSET.COMM_COM_PRESET_EDIT_HF, SUBSET.BASE_TOP}
	
	pages[PAGE_COMM_COM_PRESET_EDIT_FM1_CNV]= {SUBSET.BASE, SUBSET.COMM_COM_PRESET_EDIT_FM1_CNV, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_PRESET_EDIT_FM2_CNV]= {SUBSET.BASE, SUBSET.COMM_COM_PRESET_EDIT_FM2_CNV, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_PRESET_EDIT_UHF_CNV]= {SUBSET.BASE, SUBSET.COMM_COM_PRESET_EDIT_UHF_CNV, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_PRESET_EDIT_HF_CNV]	= {SUBSET.BASE, SUBSET.COMM_COM_PRESET_EDIT_HF_CNV, SUBSET.BASE_TOP}
					
	pages[PAGE_COMM_COM_MODEM]				= {SUBSET.BASE, SUBSET.COMM_COM_PRESET_MODEM, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_MODEM_PROTOCOL]		= {SUBSET.BASE, SUBSET.COMM_COM_PRESET_MODEM_PROTOCOL, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_MODEM_RETRIES]		= {SUBSET.BASE, SUBSET.COMM_COM_PRESET_MODEM_RETRIES, SUBSET.BASE_TOP}
	
	
	pages[PAGE_COMM_COM_ATHS]			= {SUBSET.BASE, SUBSET.COMM_COM_ATHS, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_NET]			= {SUBSET.BASE, SUBSET.COMM_COM_NET, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_NET_DELETE_YN]	= {SUBSET.BASE, SUBSET.COMM_COM_NET_DELETE_YN, SUBSET.BASE_TOP}
	pages[PAGE_COMM_COM_NET_REPLACE]	= {SUBSET.BASE, SUBSET.COMM_COM_NET_REPLACE, SUBSET.BASE_TOP}
	pages[PAGE_COMM_SOI]				= {SUBSET.BASE, SUBSET.COMM_SOI, SUBSET.BASE_TOP}

	pages[PAGE_COMM_UHF]			= {SUBSET.BASE, SUBSET.COMM_UHF, SUBSET.BASE_TOP}
	pages[PAGE_COMM_UHF_MODE]		= {SUBSET.BASE, SUBSET.COMM_UHF_MODE, SUBSET.BASE_TOP}
	pages[PAGE_COMM_UHF_CIPHER]		= {SUBSET.BASE, SUBSET.COMM_UHF_CIPHER, SUBSET.BASE_TOP}
	pages[PAGE_COMM_VHF]			= {SUBSET.BASE, SUBSET.COMM_VHF, SUBSET.BASE_TOP}
	pages[PAGE_COMM_UHF_WOD]		= {SUBSET.BASE, SUBSET.COMM_UHF_WOD, SUBSET.BASE_TOP}
	pages[PAGE_COMM_UHF_FMT]		= {SUBSET.BASE, SUBSET.COMM_UHF_FMT, SUBSET.BASE_TOP}
	pages[PAGE_COMM_UHF_SET]		= {SUBSET.BASE, SUBSET.COMM_UHF_SET, SUBSET.BASE_TOP}

	pages[PAGE_COMM_FM]				= {SUBSET.BASE, SUBSET.COMM_FM, SUBSET.BASE_TOP}
	pages[PAGE_COMM_FM_ERF]			= {SUBSET.BASE, SUBSET.COMM_FM_ERF, SUBSET.BASE_TOP}
	pages[PAGE_COMM_FM_SET]			= {SUBSET.BASE, SUBSET.COMM_FM_SET, SUBSET.BASE_TOP}

	pages[PAGE_COMM_HF]				= {SUBSET.BASE, SUBSET.COMM_HF, SUBSET.BASE_TOP}
	pages[PAGE_COMM_HF_SET]			= {SUBSET.BASE, SUBSET.COMM_HF_SET, SUBSET.BASE_TOP}
	pages[PAGE_COMM_HF_ZERO]		= {SUBSET.BASE, SUBSET.COMM_HF_ZERO, SUBSET.BASE_TOP}

	pages[PAGE_COMM_SOI_MSG_SEND]	= {SUBSET.BASE, SUBSET.COMM_SOI_MSG_SEND, SUBSET.BASE_TOP}
	pages[PAGE_COMM_SOI_SINC]		= {SUBSET.BASE, SUBSET.COMM_SOI_SINC, SUBSET.BASE_TOP}
	pages[PAGE_COMM_SOI_HQ2]		= {SUBSET.BASE, SUBSET.COMM_SOI_HQ2, SUBSET.BASE_TOP}
	pages[PAGE_COMM_SOI_UTIL]		= {SUBSET.BASE, SUBSET.COMM_SOI_UTIL, SUBSET.BASE_TOP}
	pages[PAGE_COMM_SOI_EXPND]		= {SUBSET.BASE, SUBSET.COMM_SOI_EXPND, SUBSET.BASE_TOP}

	pages[PAGE_COMM_XPNDR]			= {SUBSET.BASE, SUBSET.COMM_XPNDR, SUBSET.BASE_TOP}
	pages[PAGE_COMM_XPNDR_ANT]		= {SUBSET.BASE, SUBSET.COMM_XPNDR_ANT, SUBSET.BASE_TOP}
	pages[PAGE_COMM_XPNDR_REPLY]	= {SUBSET.BASE, SUBSET.COMM_XPNDR_REPLY, SUBSET.BASE_TOP}

	pages[PAGE_COMM_TUNE_VHF]		= {SUBSET.BASE, SUBSET.COMM_TUNE_VHF, SUBSET.BASE_TOP}
	pages[PAGE_COMM_TUNE_UHF]		= {SUBSET.BASE, SUBSET.COMM_TUNE_UHF, SUBSET.BASE_TOP}
	pages[PAGE_COMM_TUNE_FM1]		= {SUBSET.BASE, SUBSET.COMM_TUNE_FM1, SUBSET.BASE_TOP}
	pages[PAGE_COMM_TUNE_FM2]		= {SUBSET.BASE, SUBSET.COMM_TUNE_FM2, SUBSET.BASE_TOP}
	pages[PAGE_COMM_TUNE_HF]		= {SUBSET.BASE, SUBSET.COMM_TUNE_HF, SUBSET.BASE_TOP}

	pages[PAGE_COMM_GUARD_VHF]		= {SUBSET.BASE, SUBSET.COMM_GUARD_VHF, SUBSET.BASE_TOP}
	pages[PAGE_COMM_GUARD_UHF]		= {SUBSET.BASE, SUBSET.COMM_GUARD_UHF, SUBSET.BASE_TOP}
	pages[PAGE_COMM_HF_RECV_EMSN]	= {SUBSET.BASE, SUBSET.COMM_HF_RECV_EMSN, SUBSET.BASE_TOP}
	pages[PAGE_COMM_HF_XMIT_EMSN]	= {SUBSET.BASE, SUBSET.COMM_HF_XMIT_EMSN, SUBSET.BASE_TOP}
end

local function makePages_Common()
	pages[PAGE_NONE]				= {}
	pages[PAGE_BLANK]				= {SUBSET.BASE, SUBSET.BLANK, SUBSET.BASE_TOP}
	pages[PAGE_STANDBY]				= {SUBSET.BASE, SUBSET.BASE_TOP}
	pages[PAGE_INIT]				= {SUBSET.INIT}
	pages[PAGE_MENU]				= {SUBSET.BASE, SUBSET.MENU, SUBSET.BASE_TOP}
end

local function makePages()
	makePages_Common()
	makePages_ASE()
	makePages_FCR()
	makePages_WPN()
	makePages_TSD()
	makePages_AC()
	makePages_VID()
	makePages_ASTERISK()
	makePages_COM()
	makePages_DMS()
end

makePages()

init_pageID	= PAGE_NONE

--------------------------------------------------------------------------------------------------
-- PAGES BY MODE ---------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------
local function tablelen(T)
	local count = 0
	for _ in pairs(T) do count = count + 1 end
	return count
end

pages_by_mode = {}
clear_mode_table(pages_by_mode, tablelen(MFD_DISPL_FMT_LEV1), tablelen(MFD_DISPL_FMT_LEV2), tablelen(MFD_DISPL_FMT_LEV3))

---------------------------------------------------------------------------------------------------------------------------------------------------------------------
local function pages_by_mode_TSD()							
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_MAIN]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_MAIN_ACQ]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_ACQ
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_MAIN_REC]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_REC

pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_RPT_BASE]			[MFD_DISPL_FMT_LEV3.TSD_RPT_MAIN]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_RPT_MAIN
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_RPT_BASE]			[MFD_DISPL_FMT_LEV3.TSD_RPT_STAT]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_RPT_STAT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_RPT_BASE]			[MFD_DISPL_FMT_LEV3.TSD_RPT_BDA]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_RPT_BDA
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_RPT_BASE]			[MFD_DISPL_FMT_LEV3.TSD_RPT_TGT]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_RPT_TGT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_RPT_BASE]			[MFD_DISPL_FMT_LEV3.TSD_RPT_PP]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_RPT_PP
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_RPT_BASE]			[MFD_DISPL_FMT_LEV3.TSD_RPT_FARM]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_RPT_FARM

pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_FARM_BASE]			[MFD_DISPL_FMT_LEV3.TSD_FARM_MAIN]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_FARM
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_FARM_BASE]			[MFD_DISPL_FMT_LEV3.TSD_FARM_TYPE]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_FARM_TYPE

pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_PAN_BASE]			[MFD_DISPL_FMT_LEV3.TSD_PAN_2D]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_PAN_2D
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_PAN_BASE]			[MFD_DISPL_FMT_LEV3.TSD_PAN_3D]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_PAN_3D

pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_SHOW_BASE]			[MFD_DISPL_FMT_LEV3.TSD_SHOW_MAIN]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_SHOW_MAIN
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_SHOW_BASE]			[MFD_DISPL_FMT_LEV3.TSD_SHOW_SA]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_SHOW_SA
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_SHOW_BASE]			[MFD_DISPL_FMT_LEV3.TSD_SHOW_THRT_VIS_THRT]	[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_SHOW_THRT_VIS_THRT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_SHOW_BASE]			[MFD_DISPL_FMT_LEV3.TSD_SHOW_THRT_VIS_OWN]	[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_SHOW_THRT_VIS_OWN
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_SHOW_BASE]			[MFD_DISPL_FMT_LEV3.TSD_SHOW_COORD]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_SHOW_COORD

pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_COORD]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_COORD
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_SHOT]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_SHOT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_INST]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_INST
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_BAM_BASE]			[MFD_DISPL_FMT_LEV3.TSD_BAM_PF]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_BAM_PF
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_BAM_BASE]			[MFD_DISPL_FMT_LEV3.TSD_BAM_PF_ASN]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_BAM_PF_ASN
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_BAM_BASE]			[MFD_DISPL_FMT_LEV3.TSD_BAM_PF_OPT]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_BAM_PF_OPT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_BAM_BASE]			[MFD_DISPL_FMT_LEV3.TSD_BAM_PF_ACT]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_BAM_PF_ACT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_BAM_BASE]			[MFD_DISPL_FMT_LEV3.TSD_BAM_PF_ZN]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_BAM_PF_ZN
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_BAM_BASE]			[MFD_DISPL_FMT_LEV3.TSD_BAM_PF_RPT_KM]		[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_BAM_PF_RPT_KM
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_BAM_BASE]			[MFD_DISPL_FMT_LEV3.TSD_BAM_NF]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_BAM_NF
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_BAM_BASE]			[MFD_DISPL_FMT_LEV3.TSD_BAM_NF_SEL]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_BAM_NF_SEL
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_UTIL_BASE]			[MFD_DISPL_FMT_LEV3.TSD_UTIL_MAIN]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_UTIL_MAIN
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_UTIL_BASE]			[MFD_DISPL_FMT_LEV3.TSD_UTIL_ASE]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_UTIL_ASE
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_MAP_CONTOURS]		[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_MAP_CONTOURS
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_MAP_BASE]			[MFD_DISPL_FMT_LEV3.TSD_MAP_MAIN]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_MAP_MAIN
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_MAP_BASE]			[MFD_DISPL_FMT_LEV3.TSD_MAP_ORIENT]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_MAP_ORIENT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_MAP_BASE]			[MFD_DISPL_FMT_LEV3.TSD_MAP_TYPE]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_MAP_TYPE
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_MAP_BASE]			[MFD_DISPL_FMT_LEV3.TSD_MAP_COLORBAND]		[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_MAP_COLORBAND
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_MAP_BASE]			[MFD_DISPL_FMT_LEV3.TSD_MAP_FFD]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_MAP_FFD
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_MAP_BASE]			[MFD_DISPL_FMT_LEV3.TSD_MAP_SCALE]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_MAP_SCALE
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_RTE]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_RTE
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_RTE]				[MFD_DISPL_FMT_LEV3.TSD_RTM]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_RTM
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_POINT]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_POINT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_ABR]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_ABR
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_TSD]		[MFD_DISPL_FMT_LEV2.TSD_INST_UTIL]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_TSD_INST_UTIL
end
---------------------------------------------------------------------------------------------------------------------------------------------------------------------
local function pages_by_mode_COMM()	
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.NONE]							[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM_IDM]			[MFD_DISPL_FMT_LEV3.NONE]							[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_IDM
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM_MAN]			[MFD_DISPL_FMT_LEV3.NONE]							[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_MAN
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_ORIG_ID]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_ORIG_ID
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM_MSG_REC]		[MFD_DISPL_FMT_LEV3.NONE]							[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_MSG_REC
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM_MSG_REC]		[MFD_DISPL_FMT_LEV3.COMM_COM_MSG_REC_DEL_YN]		[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_MSG_REC_DEL_YN
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM_MSG_REC_REVIEW][MFD_DISPL_FMT_LEV3.NONE]							[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_MSG_REC_REVIEW
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM_MSG_REC_REVIEW][MFD_DISPL_FMT_LEV3.COMM_COM_MSG_REC_REVIEW_DEL_YN]	[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_MSG_REC_REVIEW_DEL_YN
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM_MSG_SEND]		[MFD_DISPL_FMT_LEV3.NONE]							[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_MSG_SEND
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_PRESET
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_MEMBER]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_MEMBER
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_ORIG]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_ORIG
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_PRIMARY_SELECT]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_PRIMARY_SELECT


pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_IDM_FREE_TEXT]		[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_IDM_FREE_TEXT
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_IDM_MPS_TEXT]		[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_IDM_MPS_TEXT
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_IDM_CURR_MISSION]	[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_IDM_CURR_MISSION
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_IDM_CURR_MISSION]	[MFD_DISPL_FMT_LEV4.COMM_IDM_CURR_MISSION_ROUTE]	= PAGE_COMM_IDM_CURR_MISSION_ROUTE

pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_COM_PRESET_EDIT_UNIT]	 	= PAGE_COMM_COM_PRESET_EDIT_UNIT
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_COM_PRESET_EDIT_V_UHF]	 	= PAGE_COMM_COM_PRESET_EDIT_V_UHF
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_COM_PRESET_EDIT_FM]	 	= PAGE_COMM_COM_PRESET_EDIT_FM
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_COM_PRESET_EDIT_HF]	 	= PAGE_COMM_COM_PRESET_EDIT_HF

pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_COM_PRESET_EDIT_FM1_CNV]	 = PAGE_COMM_COM_PRESET_EDIT_FM1_CNV
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_COM_PRESET_EDIT_FM2_CNV]	 = PAGE_COMM_COM_PRESET_EDIT_FM2_CNV
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_COM_PRESET_EDIT_UHF_CNV]	 = PAGE_COMM_COM_PRESET_EDIT_UHF_CNV
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_COM_PRESET_EDIT_HF_CNV]	 = PAGE_COMM_COM_PRESET_EDIT_HF_CNV

pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET_MODEM]	[MFD_DISPL_FMT_LEV4.NONE] 							= PAGE_COMM_COM_MODEM
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET_MODEM]	[MFD_DISPL_FMT_LEV4.COMM_COM_PRESET_MODEM_PROTOCOL] = PAGE_COMM_COM_MODEM_PROTOCOL
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET_MODEM]	[MFD_DISPL_FMT_LEV4.COMM_COM_PRESET_MODEM_RETRIES] 	= PAGE_COMM_COM_MODEM_RETRIES

pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_ATHS]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_ATHS
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_NET]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_COM_NET
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_NET]			[MFD_DISPL_FMT_LEV4.COMM_COM_NET_DELETE_YN] = PAGE_COMM_COM_NET_DELETE_YN
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_NET]			[MFD_DISPL_FMT_LEV4.COMM_COM_NET_REPLACE] = PAGE_COMM_COM_NET_REPLACE
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_SOI]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_SOI
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_VHF]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_VHF
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_UHF]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_UHF
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_UHF]				[MFD_DISPL_FMT_LEV3.COMM_UHF_CIPHER]		[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_UHF_CIPHER
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_UHF]				[MFD_DISPL_FMT_LEV3.COMM_UHF_MODE]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_UHF_MODE
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_UHF_WOD]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_UHF_WOD
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_UHF_FMT]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_UHF_FMT
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_UHF_SET]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_UHF_SET
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_FM]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_FM
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_FM_ERF]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_FM_ERF
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_FM_SET]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_FM_SET
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_HF]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_HF
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_HF_SET]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_HF_SET
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_HF_ZERO]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_HF_ZERO
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_SOI_MSG_SEND]		[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_SOI_MSG_SEND
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_SOI_SINC]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_SOI_SINC
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_SOI_HQ2]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_SOI_HQ2
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_SOI_UTIL]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_SOI_UTIL
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_SOI_EXPND]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_SOI_EXPND
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_XPNDR]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_XPNDR
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_XPNDR]				[MFD_DISPL_FMT_LEV3.COMM_XPNDR_ANT]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_XPNDR_ANT
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_XPNDR]				[MFD_DISPL_FMT_LEV3.COMM_XPNDR_REPLY]		[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_XPNDR_REPLY
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_TUNE_VHF] = PAGE_COMM_TUNE_VHF
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_TUNE_UHF] = PAGE_COMM_TUNE_UHF
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_TUNE_FM1] = PAGE_COMM_TUNE_FM1
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_TUNE_FM2] = PAGE_COMM_TUNE_FM2
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_COM]				[MFD_DISPL_FMT_LEV3.COMM_COM_PRESET]		[MFD_DISPL_FMT_LEV4.COMM_TUNE_HF] = PAGE_COMM_TUNE_HF
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_GUARD_VHF]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_GUARD_VHF
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_GUARD_UHF]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_GUARD_UHF
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_HF_RECV_EMSN]		[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_HF_RECV_EMSN
pages_by_mode[MFD_DISPL_FMT_LEV1.COMM]				[MFD_DISPL_FMT_LEV2.COMM_HF_XMIT_EMSN]		[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_COMM_HF_XMIT_EMSN
end
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
local function pages_by_mode_MENU()
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_VID]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_VID
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_VCR]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_VCR
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_ASTERISK]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_ASTERISK
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_WCA]				[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS_WCA
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_DTU]				[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS_DTU
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_DTU]				[MFD_DISPL_FMT_LEV4.DMS_DTU_DATA]			= PAGE_DMS_DTU_DATA
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_DTU]				[MFD_DISPL_FMT_LEV4.DMS_DTU_MISSION]		= PAGE_DMS_DTU_MISSION
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_DTU]				[MFD_DISPL_FMT_LEV4.DMS_DTU_COMM]			= PAGE_DMS_DTU_COMM
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_DTU]				[MFD_DISPL_FMT_LEV4.DMS_DTU_LOAD]			= PAGE_DMS_DTU_LOAD
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_DTU]				[MFD_DISPL_FMT_LEV4.DMS_DTU_ROUTES]			= PAGE_DMS_DTU_ROUTES
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_DTU]				[MFD_DISPL_FMT_LEV4.DMS_DTU_STBY]			= PAGE_DMS_DTU_STBY
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_FAULT]				[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS_FAULT
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_IBIT_ACFTCOMM]		[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS_IBIT_ACFTCOMM
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_IBIT_CNTLDSPL]		[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS_IBIT_CNTLDSPL
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_IBIT_WPNSIGHT]		[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS_IBIT_WPNSIGHT
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_IBIT_PROCDMS]		[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS_IBIT_PROCDMS
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_IBIT_NAVASE]		[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS_IBIT_NAVASE
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_IBIT_LISTING]		[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS_IBIT_LISTING
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_SHUTDOWN]			[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS_SHUTDOWN
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_VERS]				[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS_VERS
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_UTIL]				[MFD_DISPL_FMT_LEV4.NONE]					= PAGE_DMS_UTIL
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.MENU_DMS]				[MFD_DISPL_FMT_LEV3.DMS_IBIT_ACFTCOMM]		[MFD_DISPL_FMT_LEV4.DMS_IBIT_COMM_RADIOS]	= PAGE_DMS_COMM_RADIOS
end
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
local function pages_by_mode_WPN()
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_MAIN]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_WPN_MAIN
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_MAIN_ACQ]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_WPN_MAIN_ACQ

pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_GUN]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_WPN_GUN
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_GUN_ACQ]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_WPN_GUN_ACQ

pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_MSL_SAL]			[MFD_DISPL_FMT_LEV4.WPN_MSL_SAL_MAIN]	= PAGE_WPN_MSL_SAL
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_MSL_SAL]			[MFD_DISPL_FMT_LEV4.WPN_MSL_SAL_ACQ]	= PAGE_WPN_MSL_SAL_ACQ
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_MSL_SAL]			[MFD_DISPL_FMT_LEV4.WPN_MSL_SAL_MODE]	= PAGE_WPN_MSL_SAL_MODE
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_MSL_SAL]			[MFD_DISPL_FMT_LEV4.WPN_MSL_SAL_TRAJ]	= PAGE_WPN_MSL_SAL_TRAJ
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_MSL_SAL]			[MFD_DISPL_FMT_LEV4.WPN_MSL_SAL_SSEL]	= PAGE_WPN_MSL_SAL_SSEL
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_MSL_SAL]			[MFD_DISPL_FMT_LEV4.WPN_MSL_SAL_PRI]	= PAGE_WPN_MSL_SAL_PRI
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_MSL_SAL]			[MFD_DISPL_FMT_LEV4.WPN_MSL_SAL_ALT]	= PAGE_WPN_MSL_SAL_ALT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_MSL_RF]				[MFD_DISPL_FMT_LEV4.WPN_MSL_RF_MAIN]	= PAGE_WPN_MSL_RF
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_MSL_RF]				[MFD_DISPL_FMT_LEV4.WPN_MSL_RF_ACQ]		= PAGE_WPN_MSL_RF_ACQ

pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_RKT]				[MFD_DISPL_FMT_LEV4.WPN_RKT_MAIN]		= PAGE_WPN_RKT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_RKT]				[MFD_DISPL_FMT_LEV4.WPN_RKT_ACQ]		= PAGE_WPN_RKT_ACQ
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_RKT]				[MFD_DISPL_FMT_LEV4.WPN_RKT_PEN]		= PAGE_WPN_RKT_PEN
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_MAIN_BASE]			[MFD_DISPL_FMT_LEV3.WPN_RKT]				[MFD_DISPL_FMT_LEV4.WPN_RKT_QTY]		= PAGE_WPN_RKT_QTY

pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_CHAN]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_WPN_CHAN
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_CODE]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_WPN_CODE
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_FREQ]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_WPN_FREQ

pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_BORESIGHT]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_WPN_BORESIGHT

pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_UTIL_BASE]			[MFD_DISPL_FMT_LEV3.WPN_UTIL_MAIN]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_WPN_UTIL
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_UTIL_BASE]			[MFD_DISPL_FMT_LEV3.WPN_PLT_EOCCM_UTIL]		[MFD_DISPL_FMT_LEV4.NONE] = PAGE_WPN_PLT_EOCCM_UTIL
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_UTIL_BASE]			[MFD_DISPL_FMT_LEV3.WPN_UTIL_LOAD]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_WPN_UTIL_LOAD
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_WPN]		[MFD_DISPL_FMT_LEV2.WPN_UTIL_BASE]			[MFD_DISPL_FMT_LEV3.WPN_UTIL_RKT_INV]		[MFD_DISPL_FMT_LEV4.NONE] = PAGE_WPN_LOAD_RKT_INV
end
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
local function pages_by_mode_AIRCRAFT()
pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_FLT]		[MFD_DISPL_FMT_LEV2.NONE]					[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_FLT
pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_FLT]		[MFD_DISPL_FMT_LEV2.AIRCRAFT_FLT_SET]		[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_FLT_SET
pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_PERF]		[MFD_DISPL_FMT_LEV2.NONE]					[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_PERF
pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_PERF]		[MFD_DISPL_FMT_LEV2.AIRCRAFT_PERF_WT]		[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_PERF_WT
pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_UTIL]		[MFD_DISPL_FMT_LEV2.NONE]					[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_UTIL

pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_FUEL]		[MFD_DISPL_FMT_LEV2.FUEL_TRANSFER]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_FUEL_TRANSFER
pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_FUEL]		[MFD_DISPL_FMT_LEV2.FUEL_FULL]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_FUEL
pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_FUEL]		[MFD_DISPL_FMT_LEV2.FUEL_CHECK]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_FUEL_CHECK
pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_FUEL]		[MFD_DISPL_FMT_LEV2.FUEL_CHECK_TRANSFER]	[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_FUEL_CHECK_TRANSFER

pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_ENG]		[MFD_DISPL_FMT_LEV2.NONE]					[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_ENG_BASE
pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_ENG]		[MFD_DISPL_FMT_LEV2.AIRCRAFT_ENG_GROUND]	[MFD_DISPL_FMT_LEV3.AIRCRAFT_ENG_]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_ENG_GROUND
pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_ENG]		[MFD_DISPL_FMT_LEV2.AIRCRAFT_ENG_INFLIGHT]	[MFD_DISPL_FMT_LEV3.AIRCRAFT_ENG_]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_ENG_INFLIGHT
pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_ENG]		[MFD_DISPL_FMT_LEV2.AIRCRAFT_ENG_EMER]		[MFD_DISPL_FMT_LEV3.AIRCRAFT_ENG_]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_ENG_EMER
pages_by_mode[MFD_DISPL_FMT_LEV1.AIRCRAFT_ENG]		[MFD_DISPL_FMT_LEV2.AIRCRAFT_ENG_SYS]		[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_AC_ENG_SYS
end
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
local function pages_by_mode_FCR()
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_FCR]		[MFD_DISPL_FMT_LEV2.FCR_MODE]				[MFD_DISPL_FMT_LEV3.FCR_GTM]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_FCR_GTM
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_FCR]		[MFD_DISPL_FMT_LEV2.FCR_MODE]				[MFD_DISPL_FMT_LEV3.FCR_RMAP]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_FCR_RMAP
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_FCR]		[MFD_DISPL_FMT_LEV2.FCR_MODE]				[MFD_DISPL_FMT_LEV3.FCR_ATM]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_FCR_ATM
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_FCR]		[MFD_DISPL_FMT_LEV2.FCR_MODE]				[MFD_DISPL_FMT_LEV3.FCR_TPM]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_FCR_TPM
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_FCR]		[MFD_DISPL_FMT_LEV2.FCR_UTIL]				[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_FCR_UTIL
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_FCR]		[MFD_DISPL_FMT_LEV2.FCR_UTIL_BIT]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_FCR_UTIL_BIT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_FCR]		[MFD_DISPL_FMT_LEV2.FCR_UTIL_MISSION]		[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_FCR_UTIL_MISSIOM
end
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
local function pages_by_mode_ASE()
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_ASE]		[MFD_DISPL_FMT_LEV2.NONE]					[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_ASE
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_ASE]		[MFD_DISPL_FMT_LEV2.ASE_AUTOPAGE]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_ASE_AUTOPAGE
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_ASE]		[MFD_DISPL_FMT_LEV2.ASE_UTIL]				[MFD_DISPL_FMT_LEV3.ASE_UTIL_MAIN]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_ASE_UTIL_MAIN
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_ASE]		[MFD_DISPL_FMT_LEV2.ASE_UTIL]				[MFD_DISPL_FMT_LEV3.ASE_UTIL_B_COUNT]		[MFD_DISPL_FMT_LEV4.NONE] = PAGE_ASE_UTIL_BURST_COUNT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_ASE]		[MFD_DISPL_FMT_LEV2.ASE_UTIL]				[MFD_DISPL_FMT_LEV3.ASE_UTIL_B_INTERVAL]	[MFD_DISPL_FMT_LEV4.NONE] = PAGE_ASE_UTIL_BURST_INTERVAL
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_ASE]		[MFD_DISPL_FMT_LEV2.ASE_UTIL]				[MFD_DISPL_FMT_LEV3.ASE_UTIL_S_COUNT]		[MFD_DISPL_FMT_LEV4.NONE] = PAGE_ASE_UTIL_SALVO_COUNT
pages_by_mode[MFD_DISPL_FMT_LEV1.MISSION_ASE]		[MFD_DISPL_FMT_LEV2.ASE_UTIL]				[MFD_DISPL_FMT_LEV3.ASE_UTIL_S_INTERVAL]	[MFD_DISPL_FMT_LEV4.NONE] = PAGE_ASE_UTIL_SALVO_INTERVAL
end
----------------------------------------------------------------------------------------------------------------------------------------------------------------
local function pages_by_mode_COMMON()			
pages_by_mode[MFD_DISPL_FMT_LEV1.NONE]				[MFD_DISPL_FMT_LEV2.NONE]					[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_NONE
pages_by_mode[MFD_DISPL_FMT_LEV1.BLANK]				[MFD_DISPL_FMT_LEV2.NONE]					[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_BLANK
pages_by_mode[MFD_DISPL_FMT_LEV1.STANDBY]			[MFD_DISPL_FMT_LEV2.NONE]					[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_STANDBY
pages_by_mode[MFD_DISPL_FMT_LEV1.MENU]				[MFD_DISPL_FMT_LEV2.NONE]					[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_MENU
pages_by_mode[MFD_DISPL_FMT_LEV1.INIT]				[MFD_DISPL_FMT_LEV2.NONE]					[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_INIT
end
----------------------------------------------------------------------------------------------------------------------------------------------------------------
local function pages_by_mode_VIDEO()
pages_by_mode[MFD_DISPL_FMT_LEV1.VIDEO_BASE]		[MFD_DISPL_FMT_LEV2.NONE]					[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_VID_EMPTY
pages_by_mode[MFD_DISPL_FMT_LEV1.VIDEO_BASE]		[MFD_DISPL_FMT_LEV2.VID_FLIGHT]				[MFD_DISPL_FMT_LEV3.VID_HOVER]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_VID_HOVER
pages_by_mode[MFD_DISPL_FMT_LEV1.VIDEO_BASE]		[MFD_DISPL_FMT_LEV2.VID_FLIGHT]				[MFD_DISPL_FMT_LEV3.VID_BOP_UP]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_VID_BOP_UP
pages_by_mode[MFD_DISPL_FMT_LEV1.VIDEO_BASE]		[MFD_DISPL_FMT_LEV2.VID_FLIGHT]				[MFD_DISPL_FMT_LEV3.VID_TRANSITION]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_VID_TRANSITION
pages_by_mode[MFD_DISPL_FMT_LEV1.VIDEO_BASE]		[MFD_DISPL_FMT_LEV2.VID_FLIGHT]				[MFD_DISPL_FMT_LEV3.VID_CRUISE]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_VID_CRUISE
pages_by_mode[MFD_DISPL_FMT_LEV1.VIDEO_BASE]		[MFD_DISPL_FMT_LEV2.VID_WEAPON]				[MFD_DISPL_FMT_LEV3.VID_WEAPON]				[MFD_DISPL_FMT_LEV4.NONE] = PAGE_VID_WEAPON
pages_by_mode[MFD_DISPL_FMT_LEV1.VIDEO_BASE]		[MFD_DISPL_FMT_LEV2.VID_WEAPON]				[MFD_DISPL_FMT_LEV3.VID_FCR_GTM]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_VID_FCR_GTM
pages_by_mode[MFD_DISPL_FMT_LEV1.VIDEO_BASE]		[MFD_DISPL_FMT_LEV2.VID_WEAPON]				[MFD_DISPL_FMT_LEV3.VID_FCR_RMAP]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_VID_FCR_RMAP
pages_by_mode[MFD_DISPL_FMT_LEV1.VIDEO_BASE]		[MFD_DISPL_FMT_LEV2.VID_WEAPON]				[MFD_DISPL_FMT_LEV3.VID_FCR_ATM]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_VID_FCR_ATM
pages_by_mode[MFD_DISPL_FMT_LEV1.VIDEO_BASE]		[MFD_DISPL_FMT_LEV2.VID_WEAPON]				[MFD_DISPL_FMT_LEV3.VID_FCR_TPM]			[MFD_DISPL_FMT_LEV4.NONE] = PAGE_VID_FCR_TPM
pages_by_mode[MFD_DISPL_FMT_LEV1.VIDEO_BASE]		[MFD_DISPL_FMT_LEV2.VID_GRAYSCALE]			[MFD_DISPL_FMT_LEV3.NONE]					[MFD_DISPL_FMT_LEV4.NONE] = PAGE_VID_GRAYSCALE
end
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
pages_by_mode_COMMON()
pages_by_mode_ASE()
pages_by_mode_FCR()
pages_by_mode_AIRCRAFT()
pages_by_mode_WPN()
pages_by_mode_MENU()
pages_by_mode_COMM()
pages_by_mode_TSD()
pages_by_mode_VIDEO()

function get_page_by_mode(master, L2, L3, L4)
	return get_page_by_mode_global(pages_by_mode, init_pageID, master, L2, L3, L4)
end


-- Multiple symbols
-- see MFD_types_AH64.h, enum MFD_MultipleSymbolsSets
multipleSymbolsIDs =
{
	WAYPOINTS				= 0,
	CONTROL_MEASURES		= 1,
	TARGETS_THREATS			= 2,

	AIR_DEFENSE_RINGS		= 3,
	FCR						= 4,
	
	SHOT_AT_OWN_Above		= 5,
	SHOT_AT_OWN_Below		= 6,
	SHOT_AT_IDM				= 7,
	
	IDM_SUBSCRIBER			= 8,
}

multipleSymbols = {}
multipleSymbols[multipleSymbolsIDs.WAYPOINTS]					= "WP_Dynamic_PH"
multipleSymbols[multipleSymbolsIDs.CONTROL_MEASURES]			= "CM_Dynamic_PH"
multipleSymbols[multipleSymbolsIDs.TARGETS_THREATS]				= "TT_Dynamic_PH"
multipleSymbols[multipleSymbolsIDs.AIR_DEFENSE_RINGS]			= "ADU_zones_PH"
multipleSymbols[multipleSymbolsIDs.FCR]							= "FCR_Targets_PH"
multipleSymbols[multipleSymbolsIDs.SHOT_AT_OWN_Above]			= "ShotAt_Own_Dynamic_PH"
multipleSymbols[multipleSymbolsIDs.SHOT_AT_OWN_Below]			= "ShotAt_Own_Dynamic_PH"
multipleSymbols[multipleSymbolsIDs.SHOT_AT_IDM]					= "ShotAt_Idm_Dynamic_PH"
multipleSymbols[multipleSymbolsIDs.IDM_SUBSCRIBER]				= "IDM_SBCR_Dynamic_PH"

function getMultipleSymbolByID(id)
	local name = ""
	if multipleSymbols[id] == nil then
		return name
	else
		return multipleSymbols[id]
	end
end

templates = {}
templates["WAYPOINTS"]				= SUBSET.WAYPOINTS
templates["CONTROL_MEASURES"]		= SUBSET.CONTROL_MEASURES
templates["TARGETS_THREATS"]		= SUBSET.TARGETS_THREATS
templates["AIR_DEFENSE"]			= SUBSET.DEFENSE_ZONES
templates["FCR"]					= SUBSET.FCR_CONTACTS
templates["SHOT_AT_OWN_Above"]		= SUBSET.SHOT_AT_OWN
templates["SHOT_AT_OWN_Below"]		= SUBSET.SHOT_AT_OWN
templates["SHOT_AT_IDM"]			= SUBSET.SHOT_AT_IDM
templates["IDM_SUBSCRIBERS"]		= SUBSET.IDM_SUBSCRIBERS

function get_template(name)
	if templates[name] ~= nil then
		return templates[name]	
	end
	return -1
end
