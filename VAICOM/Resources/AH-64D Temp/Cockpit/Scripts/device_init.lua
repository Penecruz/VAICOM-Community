mount_vfs_model_path(LockOn_Options.script_path.."../Shape")

dofile(LockOn_Options.script_path		.."config.lua")
dofile(LockOn_Options.script_path		.."devices.lua")
dofile(LockOn_Options.common_script_path.."tools.lua")
dofile(LockOn_Options.script_path		.."materials.lua")
dofile(LockOn_Options.script_path		.."Displays/MFD/indicator/MFD_Tools.lua")


MainPanel = {
	"AH64::ccAH64MainPanel",
	LockOn_Options.script_path.."mainpanel_init.lua",
	{
		{"FM_PROXY",			devices.FM_PROXY},
		{"CONTROL",				devices.CONTROL_INTERFACE},
		{"ELEC",				devices.ELEC_INTERFACE},
		{"FUEL",				devices.FUEL_INTERFACE},
		{"HYDRAULIC",			devices.HYDRO_INTERFACE},
		{"POWER_PLANT",			devices.ENGINE_INTERFACE},
		{"GEAR",				devices.GEAR_INTERFACE},
		{"OXYGEN",				devices.OXYGEN_INTERFACE},
		{"CPT_MECH",			devices.CPT_MECH},
		{"CPT_LIGHTS",			devices.CPTLIGHTS_SYSTEM},
		{"ECS",					devices.ECS_INTERFACE},
		{"HMD",					devices.HMD},
		{"NVG",					devices.NVG},
		{"SAI",					devices.SAI},
		{"IAS",					devices.IAS},
		{"BARO_ALT",			devices.BARO_ALTIMETER},
		{"FAT_GAGE",			devices.FATgage},
	},
}


creators = {}


creators[devices.FM_PROXY] = {
	"AH64::avFMProxy_AH64",
	"",
	{}
}

creators[devices.CONTROL_INTERFACE] = {
	"AH64::avControlInterface_AH64",
	LockOn_Options.script_path.."Systems/ControlSystem.lua",
	{
		{"FM_proxy", devices.FM_PROXY},
		{"ELEC", devices.ELEC_INTERFACE},
		{"FMC", devices.FMC}
	}
}

creators[devices.ELEC_INTERFACE] = {
	"AH64::avElectricInterface_AH64",
	LockOn_Options.script_path.."Systems/ElectricSystem.lua",
	{
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM}
	}
}

creators[devices.FUEL_INTERFACE] = {
	"AH64::avFuelInterface_AH64",
	LockOn_Options.script_path.."Systems/FuelSystem.lua",
	{
		{"FM_PROXY", devices.FM_PROXY},
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

creators[devices.HYDRO_INTERFACE] = {
	"AH64::avHydroInterface_AH64",
	LockOn_Options.script_path.."Systems/HydroSystem.lua",
	{
		{"FM_PROXY", devices.FM_PROXY},
		{"ELEC", devices.ELEC_INTERFACE}
		--{"MuxManager", devices.MUX}}
	}
}

creators[devices.ENGINE_INTERFACE] = {
	"AH64::avEngineInterface_AH64",
	LockOn_Options.script_path.."Systems/PowerPlant.lua",
	{
		{"FM_proxy", devices.FM_PROXY},
		{"ELEC", devices.ELEC_INTERFACE},
		{"HEAD_WRAPPER", devices.HEAD_WRAPPER}
	}
}

creators[devices.GEAR_INTERFACE] = {
	"AH64::avGearInterface_AH64",
	LockOn_Options.script_path.."Systems/GearSystem.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

creators[devices.OXYGEN_INTERFACE] = {
	"AH64::avOxygenInterface_AH64",
	LockOn_Options.script_path.."Systems/OxygenSystem.lua",
	{
		{"FM_PROXY", devices.FM_PROXY},
		{"ELEC", devices.ELEC_INTERFACE},
		{"HEAD_WRAPPER", devices.HEAD_WRAPPER}
	}
}

creators[devices.CPT_MECH] = { 
	"AH64::avCockpitMechanics_AH64",
	LockOn_Options.script_path.."Systems/CockpitMechanics.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

creators[devices.EXTLIGHTS_SYSTEM] = {
	"AH64::avExtLightsSystem_AH64",
	LockOn_Options.script_path.."Systems/ExtLights.lua",
	{
		{"ElecInterface", devices.ELEC_INTERFACE},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM}
	}
}

creators[devices.CPTLIGHTS_SYSTEM] = {
	"AH64::avCptLightsSystem_AH64",
	LockOn_Options.script_path.."Systems/IntLights.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

creators[devices.ECS_INTERFACE] = {
	"AH64::avECSInterface_AH64",
	LockOn_Options.script_path.."Systems/ECSystem.lua",
	{
		{"FM_PROXY", devices.FM_PROXY},
		{"CPT_MECH", devices.CPT_MECH},
		{"MUX", devices.MUX}
	}
}

creators[devices.EMERGENCY_PANEL] = {
	"AH64::avEmergencyPanel_AH64",
	"",
	{
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

-- Instruments
creators[devices.DRVS_ASN157] = {
	"AH64::avDRVS_ASN157_AH64",
	LockOn_Options.script_path.."Instruments/DRVS_ASN157.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX}
	}
}

creators[devices.RADALT] = {
	"AH64::avRadarAltimeter_APN209_AH64",
	LockOn_Options.script_path.."Instruments/RADALT.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

creators[devices.SAI] = {
	"AH64::avSAI_AH64",
	LockOn_Options.script_path.."Instruments/SAI.lua",
	{
		{"ElecInterface", devices.ELEC_INTERFACE},
		{"FM_Proxy", devices.FM_PROXY}
	}
}

creators[devices.IAS] = {
	"AH64::avIAS_AH64",
	LockOn_Options.script_path.."Instruments/IAS.lua",
	{
		{"FM_Proxy", devices.FM_PROXY}
	}
}

creators[devices.BARO_ALTIMETER] = {
	"AH64::avBaroAltimeter_AH64",
	LockOn_Options.script_path.."Instruments/BaroAltimeter.lua",
	{
		{"FM_Proxy", devices.FM_PROXY}
	}
}

creators[devices.STANDBY_COMPASS] = {
	"avMechCompass",
	LockOn_Options.script_path.."Instruments/StandbyCompass.lua",
	{}
}

creators[devices.GPS1] = {
	"AH64::avGPS_GEM_IV_AH64",
	LockOn_Options.script_path.."Systems/GPS1.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

creators[devices.GPS2] = {
	"AH64::avGPS_GEM_IV_AH64",
	LockOn_Options.script_path.."Systems/GPS2.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

creators[devices.EGI1] = {
	"AH64::avEGI_AH64",
	LockOn_Options.script_path.."Systems/EGI1.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX},
		{"GPS", devices.GPS1},
		{"FM_PROXY", devices.FM_PROXY}
	}
}

creators[devices.EGI2] = {
	"AH64::avEGI_AH64",
	LockOn_Options.script_path.."Systems/EGI2.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX},
		{"GPS", devices.GPS2},
		{"FM_PROXY", devices.FM_PROXY}
	}
}


-- HOTAS Interface
creators[devices.HOTAS_PLT] = {
	"AH64::avHOTAS_PLT_AH64",
	LockOn_Options.script_path.."Systems/HOTAS_PLT.lua",
	{
		{"CONTROL", devices.CONTROL_INTERFACE},
		{"EXT_LIGHTS", devices.EXTLIGHTS_SYSTEM},
		{"ARM_CTRL", devices.ARMAMENT_CONTROL},
		{"ELEC", devices.ELEC_INTERFACE},
		{"HYDRO", devices.HYDRO_INTERFACE},
		{"DP", devices.DP_PLT}
	}
}

creators[devices.HOTAS_CPG] = {
	"AH64::avHOTAS_CPG_AH64",
	LockOn_Options.script_path.."Systems/HOTAS_CPG.lua",
	{
		{"CONTROL", devices.CONTROL_INTERFACE},
		{"EXT_LIGHTS", devices.EXTLIGHTS_SYSTEM},
		{"ARM_CTRL", devices.ARMAMENT_CONTROL},
		{"ELEC", devices.ELEC_INTERFACE},
		{"HYDRO", devices.HYDRO_INTERFACE},
		{"DP", devices.DP_CPG}
	}
}

creators[devices.HOTAS_INPUT] = {
	"AH64::avHOTAS_AH64_Input",
	"",
	{
		{"DEVICE_0", devices.HOTAS_PLT},
		{"DEVICE_1", devices.HOTAS_CPG}
	}
}

-- Displays
creators[devices.MFD_PLT_LEFT] = {
	"AH64::avMFD_AH64",
	LockOn_Options.script_path.."Displays/MFD/device/MFD_plt_left.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX},
		{"TADS", devices.TADS},
		{"PNVS", devices.PNVS},
		{"DP1", devices.DP_CPG},
		{"DP2", devices.DP_PLT},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM},
		{"HMD", devices.HMD},
		{"FCR", devices.FCR}
	}
}

creators[devices.MFD_PLT_RIGHT] = {
	"AH64::avMFD_AH64",
	LockOn_Options.script_path.."Displays/MFD/device/MFD_plt_right.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX},
		{"TADS", devices.TADS},
		{"PNVS", devices.PNVS},
		{"DP1", devices.DP_CPG},
		{"DP2", devices.DP_PLT},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM},
		{"HMD", devices.HMD},
		{"FCR", devices.FCR}
	}
}

creators[devices.MFD_CPG_LEFT] = {
	"AH64::avMFD_AH64",
	LockOn_Options.script_path.."Displays/MFD/device/MFD_cpg_left.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX},
		{"TADS", devices.TADS},
		{"PNVS", devices.PNVS},
		{"DP1", devices.DP_CPG},
		{"DP2", devices.DP_PLT},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM},
		{"HMD", devices.HMD},
		{"FCR", devices.FCR}
	}
}

creators[devices.MFD_CPG_RIGHT] = {
	"AH64::avMFD_AH64",
	LockOn_Options.script_path.."Displays/MFD/device/MFD_cpg_right.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX},
		{"TADS", devices.TADS},
		{"PNVS", devices.PNVS},
		{"DP1", devices.DP_CPG},
		{"DP2", devices.DP_PLT},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM},
		{"HMD", devices.HMD},
		{"FCR", devices.FCR}
	}
}

creators[devices.MFD_INPUT_LEFT] = {
	"AH64::avMFD_Input_AH64",
	"",
	{
		{"DEVICE_0", devices.MFD_PLT_LEFT},
		{"DEVICE_1", devices.MFD_CPG_LEFT}
	}
}

creators[devices.MFD_INPUT_RIGHT] = {
	"AH64::avMFD_Input_AH64",
	"",
	{
		{"DEVICE_0", devices.MFD_PLT_RIGHT},
		{"DEVICE_1", devices.MFD_CPG_RIGHT}
	}
}

creators[devices.EUFD_PLT] = {
	"AH64::avEUFD_AH64",
	"",
	{
		{"ELEC", devices.ELEC_INTERFACE},	
		{"SP1", devices.SP_PLT},
		{"SP2", devices.SP_CPG},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM}
	}
}

creators[devices.EUFD_CPG] = {
	"AH64::avEUFD_AH64",
	"",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"SP1", devices.SP_PLT},
		{"SP2", devices.SP_CPG},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM}
	}
}

creators[devices.EUFD_INPUT] = {
	"AH64::avEUFD_Input_AH64",
	"",
	{
		{"DEVICE_0", devices.EUFD_PLT},
		{"DEVICE_1", devices.EUFD_CPG}
	}
}

creators[devices.TEDAC] = {
	"AH64::avTEDAC_AH64",
	"",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX},
		{"FM_PROXY", devices.FM_PROXY},
		{"TADS", devices.TADS},
		{"PNVS", devices.PNVS},
		{"FCR", devices.FCR},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM},
		{"DP", devices.DP_CPG},
		{"HMD", devices.HMD}
	}
}

creators[devices.TEDAC_INPUT] = {
	"AH64::avTEDAC_Input_AH64",
	"",
	{
		{"DEVICE", devices.TEDAC}
	}
}

creators[devices.HMD_INPUT] = {
	"AH64::avHMD_Input_AH64",
	"",
	{
		{"DEVICE_0", devices.HMD},
		{"DEVICE_1", devices.HMD}
	}
}

-- Computers
creators[devices.MUX] = {
	"AH64::avMuxManager_AH64",
	""
}

creators[devices.HIADC] = {
	"AH64::avHIADC_AH64",
	"",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"FM_PROXY", devices.FM_PROXY},
		{"MUX", devices.MUX}
	}
}

creators[devices.FMC] = {
	"AH64::avFMC_AH64",
	LockOn_Options.script_path.."Computers/FMC.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX},
		{"FM_PROXY", devices.FM_PROXY},
		{"EGI1", devices.EGI1},
		{"EGI2", devices.EGI2}
	}
}

creators[devices.DP_PLT] = {
	"AH64::avDP_AH64",
	LockOn_Options.script_path.."Computers/DP/device/DP2.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX},
		{"DTU", devices.DATA_TRANSFER_UNIT},
		{"IHADSS", devices.IHADSS},
		{"TADS", devices.TADS},
		{"PNVS", devices.PNVS}
	}
}

creators[devices.DP_CPG] = {
	"AH64::avDP_AH64",
	LockOn_Options.script_path.."Computers/DP/device/DP1.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX},
		{"DTU", devices.DATA_TRANSFER_UNIT},
		{"IHADSS", devices.IHADSS},
		{"TADS", devices.TADS},
		{"PNVS", devices.PNVS}
	}
}
creators[devices.WP1] = {
	"AH64::avWP_AH64",
	LockOn_Options.script_path.."Computers/WP/device/WP1.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"FM_PROXY", devices.FM_PROXY},
		{"MuxManager", devices.MUX},
		{"TEDAC", devices.TEDAC},
		{"TADS", devices.TADS},
		{"DTU", devices.DATA_TRANSFER_UNIT}
	}
}
creators[devices.WP2] = {
	"AH64::avWP_AH64",
	LockOn_Options.script_path.."Computers/WP/device/WP2.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"FM_PROXY", devices.FM_PROXY},
		{"MuxManager", devices.MUX},
		{"TEDAC", devices.TEDAC},
		{"TADS", devices.TADS},
		{"DTU", devices.DATA_TRANSFER_UNIT}
	}
}
creators[devices.ELC1] = {
	"AH64::avELC_AH64",
	LockOn_Options.script_path.."Computers/ELC/device/ELC1.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX}
	}
}

creators[devices.ELC2] = {
	"AH64::avELC_AH64",
	LockOn_Options.script_path.."Computers/ELC/device/ELC2.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX}
	}
}

creators[devices.SP_PLT] = {
	"AH64::avSP_AH64",
	LockOn_Options.script_path.."Computers/SP/device/SP1.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"FM_PROXY", devices.FM_PROXY},
		{"MuxManager", devices.MUX},
		{"EUFD_PLT", devices.EUFD_PLT},
		{"EUFD_CPG", devices.EUFD_CPG},
		{"FUEL", devices.FUEL_INTERFACE},
		{"INTERCOM", devices.INTERCOM},
		{"DTU", devices.DATA_TRANSFER_UNIT}
	}
}
creators[devices.SP_CPG] = {
	"AH64::avSP_AH64",
	LockOn_Options.script_path.."Computers/SP/device/SP2.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"FM_PROXY", devices.FM_PROXY},
		{"MuxManager", devices.MUX},
		{"EUFD_PLT", devices.EUFD_PLT},
		{"EUFD_CPG", devices.EUFD_CPG},
		{"FUEL", devices.FUEL_INTERFACE},
		{"INTERCOM", devices.INTERCOM},
		{"DTU", devices.DATA_TRANSFER_UNIT}
	}
}

creators[devices.KU_PLT] = {
	"AH64::avKeyboadUnit_AH64",
	LockOn_Options.script_path.."Computers/KU_pilot.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX}
	}
}

creators[devices.KU_CPG] = {
	"AH64::avKeyboadUnit_AH64",
	LockOn_Options.script_path.."Computers/KU_cpg.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX}
	}
}

creators[devices.KU_INPUT] = {
	"AH64::avKU_AH64_Input",
	"",
	{
		{"KU_Pilot", devices.KU_PLT},
		{"KU_CPG", devices.KU_CPG}
	}
}
-- BRU
creators[devices.BRU_PLT] = {
	"AH64::avBRU_AH64",
	LockOn_Options.script_path.."Displays/BRU/BRU_plt.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM}
	}
}

creators[devices.BRU_CPG] = {
	"AH64::avBRU_AH64",
	LockOn_Options.script_path.."Displays/BRU/BRU_plt.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM}
	}
}
-- EWS

-- Radio

creators[devices.ADF_ARN_149] = {
	"AH64::avADF_ARN149_AH64",
	LockOn_Options.script_path.."Radio/ADF_ARN_149.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX}
		--{"pSynchControl", devices.NETWORK_SYNCH_CONTROLLER}
	}
}

creators[devices.INTERCOM] = {
	"AH64::avIntercom_AH64",
	LockOn_Options.script_path.."Radio/Intercom.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"COMM_Panel_front", devices.COMM_PANEL_CPG},
		{"COMM_Panel_rear", devices.COMM_PANEL_PLT},
		{"FM1", devices.FM1},
		{"FM2", devices.FM2},
		{"UHF", devices.UHF_RADIO},
		{"VHF", devices.VHF_AM_RADIO},
		{"HF", 	devices.HF_RADIO},
		{"ADF", devices.ADF_ARN_149},
		{"RWR", devices.AN_APR39 },
		{"CMWS", devices.CMWS },
		{"CIU", devices.CIU }
		--{"TACAN", devices.TACAN}
	}
}


-- Sights
creators[devices.TADS] = {
	"AH64::avTADS_AH64",
	LockOn_Options.script_path.."Sensors/TADS.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM}
	}
}



creators[devices.FCR] = {
	"AH64::avFCR_AH64",
	LockOn_Options.script_path.."Sensors/Radar/device/FCRDevice.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX},
	}
}

-- Weapon
creators[devices.GUN_TURRET_CONTROL] = {
	"AH64::avGunTurret",
	LockOn_Options.script_path.."Systems/GunTurret.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"FM_PROXY", devices.FM_PROXY}
	}
}

creators[devices.PYLONS_INPUT] = {
	"AH64::avPylonsInput_AH64",
	LockOn_Options.script_path.."Systems/PylonUnit.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX}
	}
}

creators[devices.PNVS_TURRET_CONTROL] = {
	"AH64::avPNVS_Turret",
	LockOn_Options.script_path.."Systems/PNVS_Turret.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

creators[devices.MACROS] = {
	"AH64::avAutostartDevice_AH64",
	LockOn_Options.common_script_path.."Macro_handler.lua",
	{
		{"EngineInterface", devices.ENGINE_INTERFACE},
		{"ControlInterface", devices.CONTROL_INTERFACE},
		{"HydroInterface", devices.HYDRO_INTERFACE},
		{"PLT_LMFD", devices.MFD_PLT_LEFT},
		{"CPG_RMFD", devices.MFD_CPG_RIGHT},
		{"IHADSS", devices.IHADSS}
	}
}

creators[devices.PNVS] = {
	"AH64::avPNVS_AH64",
	LockOn_Options.script_path.."Sensors/PNVS/PNVS.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX},
		{"PNVS_Turret", devices.PNVS_TURRET_CONTROL},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM}
	}
}

creators[devices.JETT_PANEL_PLT] = {
	"AH64::avJettisonPanel_PLT_AH64",
	"",
	{
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

creators[devices.JETT_PANEL_CPG] = {
	"AH64::avJettisonPanel_CPG_AH64",
	"",
	{
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

creators[devices.JETT_PANEL_INPUT] = {
	"AH64::avJettisonPanel_Input_AH64",
	"",
	{
		{"DEVICE_0", devices.JETT_PANEL_PLT},
		{"DEVICE_1", devices.JETT_PANEL_CPG}
	}
}

creators[devices.HELLFIRE_INTERFACE] = {
	"AH64::avHellfireInterface_AH64",
	"",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX},
		{"PIU", devices.PYLONS_INPUT},
		{"WP1", devices.WP1}	-- for debug only
	}
}
-- ASE
creators[devices.AN_APR39]	= {
	"AH64::avAN_APR39_AH64",
	LockOn_Options.script_path.."ASE/APR39_param.lua",
	{
		{"ELEC",devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX},
	}
}
creators[devices.CMWS]	= {
	"AH64::avCMWS_AH64",
	LockOn_Options.script_path.."ASE/CMWS/CMWS_params.lua",
	{
		{"ELEC",devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM},
		{"ADF", devices.ADF_ARN_149}
	}
}

-- Helmet
creators[devices.HMD] = {
	"AH64::avHMD_AH64",
	LockOn_Options.script_path.."Displays/HMD/device/HMD.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX},
		{"TADS", devices.TADS},
		{"PNVS", devices.PNVS},
		{"FCR", devices.FCR},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM},
		{"NVG", devices.NVG},
		{"HEAD_WRAPPER", devices.HEAD_WRAPPER}
	}
}
creators[devices.IHADSS] = {
	"AH64::avIHADSS_AH64",
	"",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX},
		{"avHMD", devices.HMD}
	}
}

creators[devices.NVG] = {
	"AH64::avNVG_AH64",
	LockOn_Options.script_path.."Sensors/NVG/NVG.lua",
	{
		{"HMD", devices.HMD},
		{"HEAD_WRAPPER", devices.HEAD_WRAPPER}
	}
}

creators[devices.PrestonAI] = {
	"AH64::avPrestonAI_AH64",
	LockOn_Options.script_path.."AI/PrestonAI.lua",
	{
		{"DP1", devices.DP_CPG},
		{"DP2", devices.DP_PLT},
		{"TADS", devices.TADS},
		{"TEDAC", devices.TEDAC},
		{"WP1", devices.WP1},
		{"WP2", devices.WP2},
		{"ELEC_INTERFACE", devices.ELEC_INTERFACE},
		{"MFD_CPG_LEFT", devices.MFD_CPG_LEFT},
		{"MFD_CPG_RIGHT", devices.MFD_CPG_RIGHT},
		{"MFD_PLT_LEFT", devices.MFD_PLT_LEFT},
		{"MFD_PLT_RIGHT", devices.MFD_PLT_RIGHT},
		{"CMWS", devices.CMWS},
		{"CPT_MECH", devices.CPT_MECH},
		{"SP1", devices.SP_CPG},
		{"SP2", devices.SP_PLT},
		{"HEAD_WRAPPER", devices.HEAD_WRAPPER},
		{"HOTAS_PLT", devices.HOTAS_PLT},
		{"IHADSS", devices.IHADSS},
		{"CPT_LIGHTS", devices.CPTLIGHTS_SYSTEM},
		{"EUFD_PLT", devices.EUFD_PLT},
		{"EUFD_CPG", devices.EUFD_CPG},
		{"HMD", devices.HMD},
		{"FCR", devices.FCR},
		{"ENGINE_INTERFACE", devices.ENGINE_INTERFACE},
		{"SAI", devices.SAI},
		{"GEAR_INTERFACE", devices.GEAR_INTERFACE},
		{"HYDRO_INTERFACE", devices.HYDRO_INTERFACE},
		{"CONTROL_INTERFACE", devices.CONTROL_INTERFACE},
		{"EXTLIGHTS_SYSTEM", devices.EXTLIGHTS_SYSTEM},
		{"INTERCOM", devices.INTERCOM},
		{"KU_PLT", devices.KU_PLT},
		{"KU_CPG", devices.KU_CPG},
	}
}

creators[devices.UHF_RADIO]	= {
	"AH64::avUHF_ARC_164_AH64",
	LockOn_Options.script_path.."Radio/UHF_Radio.lua",
		{
			{"ELEC", devices.ELEC_INTERFACE},
			{"MUX", devices.MUX},
			{"IDM", devices.IDM},
		}
}

creators[devices.VHF_AM_RADIO]	= {
		"AH64::avVHF_ARC_186_AH64",
		LockOn_Options.script_path.."Radio/AM_Radio.lua",
		{
			{"ELEC", devices.ELEC_INTERFACE},
			{"MUX", devices.MUX},
			{"IDM", devices.IDM},
		}
}

creators[devices.FM1] = {
	"AH64::avFM1_AH64",
	LockOn_Options.script_path.."Radio/ARC_210D_FM1.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX},
		{"IDM", devices.IDM},
	}
}

creators[devices.FM2] = {
	"AH64::avFM2_AH64",
	LockOn_Options.script_path.."Radio/ARC_210D_FM2.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX},
		{"IDM", devices.IDM},
	}
}

creators[devices.HF_RADIO] = {
	"AH64::avHF_ARC_220_AH64",
	LockOn_Options.script_path.."Radio/HF_Radio.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX}
	}
}

creators[devices.CIU] = {
	"AH64::avCIU_AH64",
		LockOn_Options.script_path.."Computers/CIU/device/CIU_Messages.lua",
	{
		{"ELEC", devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX},
		{"COMM_PANEL_PLT", devices.COMM_PANEL_PLT},
		{"COMM_PANEL_CPG", devices.COMM_PANEL_CPG}
	}
}

creators[devices.COMM_PANEL_CPG] = {
	"AH64::avCOMM_Panel_AH64",
	"",
	{
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

creators[devices.COMM_PANEL_PLT] = {
	"AH64::avCOMM_Panel_AH64",
	"",
	{
		{"ELEC", devices.ELEC_INTERFACE}
	}
}

creators[devices.COMM_PANEL_INPUT] = {
	"AH64::avCOMM_Panel_Input_AH64",
	"",
	{
		{"DEVICE_0", devices.COMM_PANEL_PLT},
		{"DEVICE_1", devices.COMM_PANEL_CPG}
	}
}

creators[devices.FATgage] = {
	"AH64::avFATgage_AH64",
	LockOn_Options.script_path.."Instruments/FATgage.lua",
	{
		{"FM_Proxy", devices.FM_PROXY}
	}
}

creators[devices.HEAD_WRAPPER] = {
	"AH64::avHeadWrapper_AH64",
	LockOn_Options.script_path.."Systems/HeadWrapper.lua",
	{}
}

creators[devices.DATA_TRANSFER_UNIT] = {
	"AH64::avDataTransferUnit_AH64",
	LockOn_Options.script_path.."Computers/DTU/DTC.lua",
	{
		{"MUX", devices.MUX},
		{"DP_CPG", devices.DP_CPG},
		{"DP_PLT", devices.DP_PLT},
		{"WP1", devices.WP1},
		{"WP2", devices.WP2},
		{"SP1", devices.SP_PLT},
		{"SP2", devices.SP_CPG},
		{"ELEC", devices.ELEC_INTERFACE},
		{"AN_APR39", devices.AN_APR39},
	}
}

creators[devices.IDM] = {
	"AH64::avIDM_AH64",
	"",
	{	
		{"ELEC", devices.ELEC_INTERFACE},
		{"MUX", devices.MUX},
		{"FM1", devices.FM1},
		{"FM2", devices.FM2},
		{"VHF", devices.VHF_AM_RADIO},
		{"UHF", devices.UHF_RADIO},
		{"CIU", devices.CIU },
		{"DTU", devices.DATA_TRANSFER_UNIT}
	}
}

creators[devices.AN_AVR2A]	= {
	"AH64::avAN_AVR_2A_AH64",
	"",
	{
		{"ELEC",devices.ELEC_INTERFACE},
		{"MuxManager", devices.MUX}
	}
}
-- Indicators---------------------------------------------------------------------------
indicators = {}

-- PNVS camera for bake
indicators[#indicators + 1] = {
	"AH64::ccPNVS_CAM_AH64",
	LockOn_Options.script_path.."Cameras/PNVS/PNVS_CAM_init.lua",
	devices.PNVS,
	{
		{"MFD2-L-CENTER", "MFD2-L-DOWN", "MFD2-L-RIGHT"},
		{sx_l = -0.0025},
		1
	}
}
writeParameter("PNVS_CAM_INDICATOR_INDEX", #indicators - 1)

-- TADS camera for bake
indicators[#indicators + 1] = {
	"AH64::ccTADS_CAM_AH64",
	LockOn_Options.script_path.."Cameras/TADS/TADS_CAM_init.lua",
	devices.TADS,
	{
		{"MFD2-L-CENTER", "MFD2-L-DOWN", "MFD2-L-RIGHT"},
		{sx_l = -0.0025},
		2
	}
}
writeParameter("TADS_CAM_INDICATOR_INDEX", #indicators - 1)

-- RMAP camera for bake
indicators[#indicators + 1] = {
	"AH64::ccRMAP_CAM_AH64",
	LockOn_Options.script_path.."Cameras/RMAP/RMAP_CAM_init.lua",
	devices.FCR,
	{
		{"MFD2-L-CENTER", "MFD2-L-DOWN", "MFD2-L-RIGHT"},
		{sx_l = -0.0025},
		6
	}
}
writeParameter("RMAP_CAM_INDICATOR_INDEX", #indicators - 1)


-- MAP for PLT Left MPD
indicators[#indicators + 1] = {
	"AH64::ccMAP_CAM_AH64",
	LockOn_Options.script_path.."Cameras/MAP/MAP_CAM_init.lua",
	devices.MFD_PLT_LEFT,
	{
		{"MFD2-L-CENTER", "MFD2-L-DOWN", "MFD2-L-RIGHT"},
		{sx_l = -0.0025},
		3
	}
}
writeParameter("MAP_PLT_L_INDICATOR_INDEX", #indicators - 1)

-- MAP for PLT Right MPD
indicators[#indicators + 1] = {
	"AH64::ccMAP_CAM_AH64",
	LockOn_Options.script_path.."Cameras/MAP/MAP_CAM_init.lua",
	devices.MFD_PLT_RIGHT,
	{
		{"MFD2-R-CENTER", "MFD2-R-DOWN", "MFD2-R-RIGHT"},
		{sx_l = -0.0025},
		5
	}
}
writeParameter("MAP_PLT_R_INDICATOR_INDEX", #indicators - 1)

-- MAP for CPG Left MPD
indicators[#indicators + 1] = {
	"AH64::ccMAP_CAM_AH64",
	LockOn_Options.script_path.."Cameras/MAP/MAP_CAM_init.lua",
	devices.MFD_CPG_LEFT,
	{
		{"MFD1-L-CENTER", "MFD1-L-DOWN", "MFD1-L-RIGHT"},
		{sx_l = -0.0025},
		3
	}
}
writeParameter("MAP_CPG_L_INDICATOR_INDEX", #indicators - 1)

-- MAP for CPG Right MPD
indicators[#indicators + 1] = {
	"AH64::ccMAP_CAM_AH64",
	LockOn_Options.script_path.."Cameras/MAP/MAP_CAM_init.lua",
	devices.MFD_CPG_RIGHT,
	{
		{"MFD1-R-CENTER", "MFD1-R-DOWN", "MFD1-R-RIGHT"},
		{sx_l = -0.0025},
		5
	}
}
writeParameter("MAP_CPG_R_INDICATOR_INDEX", #indicators - 1)


-- Left PLT MFD
indicators[#indicators + 1] = {
	"AH64::ccMFD_AH64",
	LockOn_Options.script_path.."Displays/MFD/indicator/MFD_plt_left_init.lua",
	devices.MFD_PLT_LEFT,
	{
		{"MFD2-L-CENTER", "MFD2-L-DOWN", "MFD2-L-RIGHT"},
		{sx_l = -0.0025}
	}
}
writeParameter("MFD_LEFT_PLT_INDICATOR_INDEX", #indicators - 1)
indicators[#indicators + 1] = {
	"AH64::ccMFD_LCD_AH64",
	LockOn_Options.script_path.."Displays/MFD/indicator/LCD/MPD_LCD_Plt_Left.lua",
	devices.MFD_PLT_LEFT,
	{
		{"MFD2-L-CENTER", "MFD2-L-DOWN", "MFD2-L-RIGHT"},
		{sx_l = -0.0025},
	}
}
--Right PLT MFD
indicators[#indicators + 1] = {
	"AH64::ccMFD_AH64",
	LockOn_Options.script_path.."Displays/MFD/indicator/MFD_plt_right_init.lua",
	devices.MFD_PLT_RIGHT,
	{
		{"MFD2-R-CENTER", "MFD2-R-DOWN", "MFD2-R-RIGHT"},
		{sx_l = -0.0025}
	}
}
writeParameter("MFD_RIGHT_PLT_INDICATOR_INDEX", #indicators - 1)
indicators[#indicators + 1] = {
	"AH64::ccMFD_LCD_AH64",
	LockOn_Options.script_path.."Displays/MFD/indicator/LCD/MPD_LCD_Plt_Right.lua",
	devices.MFD_PLT_RIGHT,
	{
		{"MFD2-R-CENTER", "MFD2-R-DOWN", "MFD2-R-RIGHT"},
		{sx_l = -0.0025},
	}
}
-- Left CPG MFD
indicators[#indicators + 1] = {
	"AH64::ccMFD_AH64",
	LockOn_Options.script_path.."Displays/MFD/indicator/MFD_cpg_left_init.lua",
	devices.MFD_CPG_LEFT,
	{
		{"MFD1-L-CENTER", "MFD1-L-DOWN", "MFD1-L-RIGHT"},
		{sx_l = -0.0025}
	}
}
writeParameter("MFD_LEFT_CPG_INDICATOR_INDEX", #indicators - 1)
indicators[#indicators + 1] = {
	"AH64::ccMFD_LCD_AH64",
	LockOn_Options.script_path.."Displays/MFD/indicator/LCD/MPD_LCD_Cpg_Left.lua",
	devices.MFD_CPG_LEFT,
	{
		{"MFD1-L-CENTER", "MFD1-L-DOWN", "MFD1-L-RIGHT"},
		{sx_l = -0.0025},
	}
}
-- Right CPG MFD
indicators[#indicators + 1] = {
	"AH64::ccMFD_AH64",
	LockOn_Options.script_path.."Displays/MFD/indicator/MFD_cpg_right_init.lua",
	devices.MFD_CPG_RIGHT,
	{
		{"MFD1-R-CENTER", "MFD1-R-DOWN", "MFD1-R-RIGHT"},
		{sx_l = -0.0025}
	}
}
writeParameter("MFD_RIGHT_CPG_INDICATOR_INDEX", #indicators - 1)
indicators[#indicators + 1] = {
	"AH64::ccMFD_LCD_AH64",
	LockOn_Options.script_path.."Displays/MFD/indicator/LCD/MPD_LCD_Cpg_Right.lua",
	devices.MFD_CPG_RIGHT,
	{
		{"MFD1-R-CENTER", "MFD1-R-DOWN", "MFD1-R-RIGHT"},
		{sx_l = -0.0025},
	}
}



indicators[#indicators + 1] = {
	"AH64::ccKU_AH64",
	LockOn_Options.script_path.."Displays/KU/KU_init.lua",
	devices.KU_CPG,
	{
		{"CU1-CENTER", "CU1-DOWN", "CU1-RIGHT"},	-- TODO: connectors
		{
			--sx_l = -0.07, sy_l = -0.18,	sz_l = 0.08			-- TODO: position
		}
	}
}

indicators[#indicators + 1] = {
	"AH64::ccKU_AH64",
	LockOn_Options.script_path.."Displays/KU/KU_init.lua",
	devices.KU_PLT,
	{
		{"CU2-CENTER", "CU2-DOWN", "CU2-RIGHT"},	-- TODO: connectors
		{
			--sx_l = -0.07, sy_l = -0.18,	sz_l = 0.08			-- TODO: position
		}
	}
}

indicators[#indicators + 1] = {
	"AH64::ccControlsIndicator_AH64",
	LockOn_Options.script_path.."ControlsIndicator/ControlsIndicator.lua",
	devices.CONTROL_INTERFACE
}

indicators[#indicators + 1] = {
	"AH64::ccEUFD_AH64",
	LockOn_Options.script_path.."Displays/EUFD/EUFD_plt_init.lua",
	devices.EUFD_PLT,
	{
		{"EUFD2-CENTER", "EUFD2-DOWN", "EUFD2-RIGHT"},	-- TODO: connectors
		{sx_l = 0.0, sy_l = 0.0, sz_l = 0.0}
		--{sx_l = -0.07, sy_l = 0.18, sz_l = -0.05}	-- TODO: position
	}
}

indicators[#indicators + 1] = {
	"AH64::ccEUFD_AH64",
	LockOn_Options.script_path.."Displays/EUFD/EUFD_cpg_init.lua",
	devices.EUFD_CPG,
	{
		{"EUFD1-CENTER", "EUFD1-DOWN", "EUFD1-RIGHT"},	-- TODO: connectors
		{sx_l = 0.0, sy_l = 0.0, sz_l = 0.0}
		--{sx_l = -0.175, sy_l = 0.18, sz_l = -0.1}	-- TODO: position
	}
}

-- TEDAC
indicators[#indicators + 1] = {
	"AH64::ccTEDAC_AH64",
	LockOn_Options.script_path.."Displays/TEDAC/TEDAC_init.lua",
	devices.TEDAC,
	{
		{"TEDAC-CENTER", "TEDAC-DOWN", "TEDAC-RIGHT"},
		{sx_l = -0.0025,}
	}
}
writeParameter("TEDAC_INDICATOR_INDEX", #indicators - 1)
indicators[#indicators + 1] = {
	"AH64::ccTEDAC_LCD_AH64",
	LockOn_Options.script_path.."Displays/TEDAC/LCD/TEDAC_LCD_init.lua",
	devices.TEDAC,
	{
		{"TEDAC-CENTER", "TEDAC-DOWN", "TEDAC-RIGHT"},
		{sx_l = -0.0025},
	}
}


-- HMD
indicators[#indicators + 1] = {
	"AH64::ccHMD_AH64",
	LockOn_Options.script_path.."Displays/HMD/indicator/HMD_init.lua",
	devices.HMD,
}

indicators[#indicators + 1] = {
	"AH64::ccBRU_AH64",
	LockOn_Options.script_path.."Displays/BRU/BRU_plt_init.lua",
	devices.BRU_PLT,
	{
		{"BRU2-CENTER", "BRU2-DOWN", "BRU2-RIGHT"},
		{sx_l = -0.0025}
	}
}

indicators[#indicators + 1] = {
	"AH64::ccBRU_AH64",
	LockOn_Options.script_path.."Displays/BRU/BRU_cpg_init.lua",
	devices.BRU_CPG,
	{
		{"BRU1-CENTER", "BRU1-DOWN", "BRU1-RIGHT"},
		{sx_l = -0.0025}
	}
}

-- CMWS
indicators[#indicators + 1] = {
	"AH64::ccCMWS_AH64",
	LockOn_Options.script_path.."ASE/CMWS/CMWS_init.lua",
	devices.CMWS,
	{
		{"CMWS-CENTER", "CMWS-DOWN", "CMWS-RIGHT"},
		{sx_l = -0.0025}
	}
}

--AI
indicators[#indicators + 1] = { --noVR
	"AH64::ccPrestonAIIndicator_AH64",
	LockOn_Options.script_path.."AI/PrestonAI_indicator_noVR.lua",
	devices.PrestonAI
}

indicators[#indicators + 1] = { --VR
	"AH64::ccPrestonAIIndicator_AH64",
	LockOn_Options.script_path.."AI/PrestonAI_indicator_VR.lua",
	devices.PrestonAI
}

kneeboard_implementation		= "AH64::ccKneeboardExtension_AH64"
disable_kneeboard_render_target	= false
---------------------------------------------
dofile(LockOn_Options.common_script_path.."KNEEBOARD/declare_kneeboard_device.lua")
---------------------------------------------
dofile(LockOn_Options.common_script_path.."PADLOCK/PADLOCK_declare.lua")
---------------------------------------------
