
local count = -1
local function counter()
	count = count + 1
	return count
end

MFD_DISPL_FMT_LEV1 =
{
	NONE				= counter(), --0
	BLANK				= counter(),
	STANDBY				= counter(),
	INIT				= counter(),
		
-- common pages( top&bottom lines in single DP mode )
	MENU				= counter(),
	COMM				= counter(),
	
-- cpg pages ( top line in single DP mode ) 
	MISSION_ASE			= counter(),
	MISSION_FCR			= counter(),
	MISSION_TSD			= counter(),
	MISSION_WPN			= counter(),
	
-- plt pages ( bottom line in single DP mode )
	AIRCRAFT_ENG		= counter(),
	AIRCRAFT_FLT		= counter(),
	AIRCRAFT_FUEL		= counter(),
	AIRCRAFT_PERF		= counter(),
	AIRCRAFT_UTIL		= counter(),
-- VIDEO
	VIDEO_BASE			= counter(),
}
count = -1

MFD_DISPL_FMT_LEV2 =
{
	NONE					= counter(), --0
	MENU_VID				= counter(),
	MENU_VCR				= counter(),
	MENU_ASTERISK			= counter(),
	MENU_DMS				= counter(),
	
	FUEL_FULL				= counter(),
	FUEL_TRANSFER			= counter(),
	FUEL_CHECK				= counter(),
	FUEL_CHECK_TRANSFER		= counter(),
	
	AIRCRAFT_ENG_GROUND		= counter(),
	AIRCRAFT_ENG_INFLIGHT	= counter(),
	AIRCRAFT_ENG_EMER		= counter(),
	AIRCRAFT_ENG_SYS		= counter(),
	AIRCRAFT_FLT_SET		= counter(),
	AIRCRAFT_PERF_WT		= counter(),
	
	TSD_MAIN				= counter(),
	TSD_MAIN_ACQ			= counter(),
	TSD_MAIN_REC			= counter(),
	
	TSD_MAP_BASE			= counter(),
	TSD_PAN_BASE			= counter(),
	TSD_RPT_BASE			= counter(),
	TSD_SHOW_BASE			= counter(),
	TSD_FARM_BASE			= counter(),
	TSD_UTIL_BASE			= counter(),
	TSD_BAM_BASE			= counter(),

	TSD_COORD				= counter(),
	TSD_SHOT				= counter(),
	TSD_INST				= counter(),
	TSD_INST_UTIL			= counter(),
	TSD_MAP_CONTOURS		= counter(),
	TSD_RTE					= counter(),
	TSD_POINT				= counter(),
	TSD_ABR					= counter(),

	WPN_MAIN_BASE			= counter(),

	WPN_CHAN				= counter(),
	WPN_CODE				= counter(),
	WPN_FREQ				= counter(),
	
	WPN_BORESIGHT			= counter(),
	WPN_UTIL_BASE			= counter(),

	COMM_SOI				= counter(),
	COMM_SOI_MSG_SEND		= counter(),
	COMM_SOI_SINC			= counter(),
	COMM_SOI_HQ2			= counter(),
	COMM_SOI_UTIL			= counter(),
	COMM_SOI_EXPND			= counter(),
		
	COMM_XPNDR				= counter(),
	COMM_VHF				= counter(),
	COMM_UHF				= counter(),
	COMM_UHF_WOD			= counter(),
	COMM_UHF_FMT			= counter(),
	COMM_UHF_SET			= counter(),
	COMM_FM					= counter(),
	COMM_FM_ERF				= counter(),
	COMM_FM_SET				= counter(),
	COMM_HF					= counter(),
	COMM_HF_SET				= counter(),
		
	COMM_HF_ZERO			= counter(),
	COMM_COM				= counter(),
	COMM_COM_IDM			= counter(),
	COMM_COM_MAN			= counter(),
	COMM_COM_MSG_REC		= counter(),
	COMM_COM_MSG_REC_REVIEW = counter(),
	COMM_COM_MSG_SEND		= counter(),
	
	COMM_GUARD_VHF			= counter(),
	COMM_GUARD_UHF			= counter(),
	COMM_HF_RECV_EMSN		= counter(),
	COMM_HF_XMIT_EMSN		= counter(),
	
	FCR_MODE				= counter(),
	FCR_UTIL				= counter(),
	FCR_UTIL_BIT			= counter(),
	FCR_UTIL_MISSION		= counter(),

	ASE_UTIL				= counter(),
	ASE_AUTOPAGE			= counter(),
	
	VID_FLIGHT				= counter(),
	VID_WEAPON				= counter(),
	VID_GRAYSCALE			= counter(),
}
count = -1

MFD_DISPL_FMT_LEV3 =
{
	NONE					= counter(),
	
	TSD_PAN_2D				= counter(),
	TSD_PAN_3D				= counter(),
	TSD_UTIL_MAIN			= counter(),
	TSD_UTIL_ASE			= counter(),
	TSD_BAM_PF				= counter(),
	TSD_BAM_PF_ASN			= counter(),
	TSD_BAM_PF_OPT			= counter(),
	TSD_BAM_PF_ACT			= counter(),
	TSD_BAM_PF_ZN			= counter(),
	TSD_BAM_PF_RPT_KM		= counter(),
	TSD_BAM_NF				= counter(),
	TSD_BAM_NF_SEL			= counter(),

	TSD_MAP_MAIN			= counter(),
	TSD_MAP_ORIENT			= counter(),
	TSD_MAP_TYPE			= counter(),
	TSD_MAP_COLORBAND		= counter(),
	TSD_MAP_FFD				= counter(),
	TSD_MAP_SCALE			= counter(),
	
	TSD_RPT_MAIN			= counter(),
	TSD_RPT_STAT			= counter(),
	TSD_RPT_BDA				= counter(),
	TSD_RPT_TGT				= counter(),
	TSD_RPT_PP				= counter(),
	TSD_RPT_FARM			= counter(),

	TSD_FARM_MAIN			= counter(),
	TSD_FARM_TYPE			= counter(),

	TSD_RTM					= counter(),
	
	TSD_SHOW_MAIN				= counter(),
	TSD_SHOW_SA					= counter(),
	TSD_SHOW_THRT_VIS_THRT		= counter(),
	TSD_SHOW_THRT_VIS_OWN		= counter(),
	TSD_SHOW_COORD				= counter(),

	DMS_WCA					= counter(),
	DMS_DTU					= counter(),
	DMS_FAULT				= counter(),
	DMS_IBIT_ACFTCOMM		= counter(),
	DMS_IBIT_CNTLDSPL		= counter(),
	DMS_IBIT_WPNSIGHT		= counter(),
	DMS_IBIT_PROCDMS		= counter(),
	DMS_IBIT_NAVASE			= counter(),
	DMS_IBIT_LISTING		= counter(),
	DMS_SHUTDOWN			= counter(),
	DMS_VERS				= counter(),
	DMS_UTIL				= counter(),

	WPN_MAIN				= counter(),
	WPN_MAIN_ACQ			= counter(),

	WPN_GUN					= counter(),
	WPN_GUN_ACQ				= counter(),

	WPN_MSL_SAL				= counter(),
	WPN_MSL_RF				= counter(),
	WPN_RKT					= counter(),

	WPN_UTIL_MAIN          	= counter(),
	WPN_UTIL_LOAD          	= counter(),
	WPN_UTIL_RKT_INV		= counter(),
	WPN_PLT_EOCCM_UTIL		= counter(),

	VID_EMPTY				= counter(),
	VID_COMMON				= counter(),
	VID_HOVER				= counter(),
	VID_BOP_UP				= counter(),
	VID_TRANSITION			= counter(),
	VID_CRUISE				= counter(),
	VID_WEAPON				= counter(),
	VID_FCR_GTM				= counter(),
	VID_FCR_RMAP			= counter(),
	VID_FCR_ATM				= counter(),
	VID_FCR_TPM				= counter(),

	ASE_UTIL_MAIN			= counter(),
	ASE_UTIL_B_COUNT		= counter(),
	ASE_UTIL_B_INTERVAL		= counter(),
	ASE_UTIL_S_COUNT		= counter(),
	ASE_UTIL_S_INTERVAL		= counter(),
	AIRCRAFT_ENG_			= counter(),
	COMM_COM_PRESET			= counter(),
	
	COMM_COM_ATHS			= counter(),
	COMM_COM_NET			= counter(),
	COMM_COM_MEMBER			= counter(),
	COMM_COM_ORIG			= counter(),
	
	COMM_COM_ORIG_ID		= counter(),
	COMM_COM_PRESET_MODEM	= counter(),
	
	COMM_IDM_FREE_TEXT		= counter(),
	COMM_IDM_MPS_TEXT		= counter(),
	COMM_IDM_CURR_MISSION	= counter(),
	
	COMM_COM_MSG_REC_DEL_YN	= counter(),
	COMM_COM_MSG_REC_REVIEW_DEL_YN	= counter(),
	COMM_PRIMARY_SELECT		= counter(),

	MPD_COMMON				= counter(),
	COMM_XPNDR_ANT			= counter(),
	COMM_XPNDR_REPLY		= counter(),
	
	COMM_UHF_MODE			= counter(),
	COMM_UHF_CIPHER			= counter(),
	FCR_GTM					= counter(),
	FCR_RMAP				= counter(),
	FCR_ATM					= counter(),
	FCR_TPM					= counter(),
}
count = -1

MFD_DISPL_FMT_LEV4 =
{
	NONE					= counter(),	-- 0

	WPN_MSL_SAL_MAIN		= counter(),
	WPN_MSL_SAL_ACQ			= counter(),
	WPN_MSL_SAL_MODE		= counter(),
	WPN_MSL_SAL_TRAJ		= counter(),
	WPN_MSL_SAL_SSEL		= counter(),
	WPN_MSL_SAL_PRI			= counter(),
	WPN_MSL_SAL_ALT			= counter(),

	WPN_MSL_RF_MAIN			= counter(),
	WPN_MSL_RF_ACQ			= counter(),

	WPN_RKT_MAIN			= counter(),	-- 10
	WPN_RKT_ACQ				= counter(),
	WPN_RKT_PEN				= counter(),
	WPN_RKT_QTY				= counter(),
		
	DMS_DTU_DATA			= counter(),
	DMS_DTU_MISSION			= counter(),
	DMS_DTU_COMM			= counter(),
	DMS_DTU_LOAD			= counter(),
	DMS_DTU_ROUTES			= counter(),
	DMS_DTU_STBY			= counter(),
	DMS_IBIT_COMM_RADIOS	= counter(),
	
	COMM_COM_PRESET_MODEM_PROTOCOL	= counter(),
	COMM_COM_PRESET_MODEM_RETRIES	= counter(),
	
	COMM_COM_PRESET_EDIT_UNIT	= counter(),
	COMM_COM_PRESET_EDIT_V_UHF	= counter(),
	COMM_COM_PRESET_EDIT_FM		= counter(),
	COMM_COM_PRESET_EDIT_HF		= counter(),
	COMM_COM_NET_REPLACE		= counter(),
	COMM_COM_NET_DELETE_YN		= counter(),
	
	COMM_TUNE_VHF			= counter(),
	COMM_TUNE_UHF			= counter(),
	COMM_TUNE_FM1			= counter(),
	COMM_TUNE_FM2			= counter(),
	COMM_TUNE_HF			= counter(),
	
	COMM_IDM_CURR_MISSION_ROUTE		= counter(),
	COMM_COM_PRESET_EDIT_FM1_CNV	= counter(),	
	COMM_COM_PRESET_EDIT_FM2_CNV	= counter(),	
    COMM_COM_PRESET_EDIT_UHF_CNV	= counter(),	
    COMM_COM_PRESET_EDIT_HF_CNV		= counter(),	
}