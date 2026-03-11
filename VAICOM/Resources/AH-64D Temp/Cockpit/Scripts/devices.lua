local count = 0
local function counter()
	count = count + 1
	return count
end

-------DEVICE ID-------
devices = {}

-- do not changed following sequence for sim
devices["FM_PROXY"]					= counter()
devices["CONTROL_INTERFACE"]		= counter()
devices["ELEC_INTERFACE"]			= counter()
devices["FUEL_INTERFACE"]			= counter()
devices["HYDRO_INTERFACE"]			= counter()
devices["ENGINE_INTERFACE"]			= counter()
devices["GEAR_INTERFACE"]			= counter()
devices["OXYGEN_INTERFACE"]			= counter()
devices["CPT_MECH"]					= counter()
devices["EXTLIGHTS_SYSTEM"]			= counter()
devices["CPTLIGHTS_SYSTEM"]			= counter()
devices["ECS_INTERFACE"]			= counter()
-- Instruments
devices["SAI"]						= counter()		-- Standby Attitude Indicator
devices["IAS"]						= counter()		-- Standby Airspeed Indicator
devices["BARO_ALTIMETER"]			= counter()		-- Standby Altimeter
devices["STANDBY_COMPASS"]			= counter()		-- Standby Magnetic Compass
devices["FATgage"]					= counter()		-- Free Air Temperature Gage
devices["GPS1"]						= counter()
devices["GPS2"]						= counter()
devices["EGI1"]						= counter()
devices["EGI2"]						= counter()

devices["MUX"]						= counter()		-- Multiplex manager, holds channels and manages remote terminals addition/remove

-- HOTAS Interface
devices["HOTAS_PLT"]				= counter()
devices["HOTAS_CPG"]				= counter()
devices["HOTAS_INPUT"]				= counter()

-- Helmet
devices["IHADSS"]					= counter()

-- Computers
devices["SP_PLT"]					= counter()		-- System Processor
devices["SP_CPG"]					= counter()		-- System Processor
devices["KU_PLT"]					= counter()		-- Keyboard Unit
devices["KU_CPG"]					= counter()		-- Keyboard Unit
devices["KU_INPUT"]					= counter()		-- for input and routing
devices["ELC1"]						= counter()		--
devices["ELC2"]						= counter()		--
devices["HIADC"]					= counter()		-- Highly Integrated Air Data Computer
devices["FMC"]						= counter()		-- Flight Management Computer
devices["WP1"]						= counter()		-- Weapon Processor
devices["WP2"]						= counter()		-- Weapon Processor
devices["DP_PLT"]					= counter()		-- Display Processor, DP2
devices["DP_CPG"]					= counter()		-- Display Processor, DP1

-- Displays
devices["HMD"]						= counter()
devices["HMD_INPUT"]				= counter()
devices["MFD_PLT_LEFT"]				= counter()		-- Multifunction Display
devices["MFD_PLT_RIGHT"]			= counter()		-- Multifunction Display
devices["MFD_CPG_LEFT"]				= counter()		-- Multifunction Display
devices["MFD_CPG_RIGHT"]			= counter()		-- Multifunction Display
devices["MFD_INPUT_LEFT"]			= counter()
devices["MFD_INPUT_RIGHT"]			= counter()
devices["EUFD_PLT"]					= counter()		-- Enhanced Up-Front Display Pilot
devices["EUFD_CPG"]					= counter()		-- Enhanced Up-Front Display CPG
devices["EUFD_INPUT"]				= counter()
devices["TEDAC"]					= counter()		-- TADS Electronic Display and Control
devices["TEDAC_INPUT"]				= counter()

-- Radio
devices["DRVS_ASN157"]				= counter()
devices["RADALT"]					= counter()
devices["ADF_ARN_149"]				= counter()
devices["EMERGENCY_PANEL"]			= counter()
devices["UHF_RADIO"]				= counter()		-- UHF-AM ARC-164(V)
devices["VHF_AM_RADIO"]				= counter()		-- VHF-AM ARC-186(V)
devices["FM1"]						= counter()		-- VHF-FM ARC-210D 
devices["FM2"]						= counter()		-- VHF-FM ARC-210D 
devices["HF_RADIO"]					= counter()		-- HF ARC-220
devices["COMM_PANEL_CPG"]			= counter()		-- Communications Panel (COMM - front)
devices["COMM_PANEL_PLT"]			= counter()		-- Communications Panel (COMM - rear)
devices["COMM_PANEL_INPUT"]			= counter()
devices["CIU"]						= counter()		-- Communications Interface Unit ( Voice messages )
devices["IFF"]						= counter()
devices["INTERCOM"]					= counter()		-- must be last in radio group

-- Sights
devices["TADS"]						= counter()		-- Target Acquisition Designation Sight
devices["PNVS"]						= counter()		-- Pilot Night Vision Sensor
devices["PNVS_TURRET_CONTROL"]		= counter()		-- PNVS control system
devices["FCR"]						= counter()		-- Fire Control Radar
devices["RFI"]						= counter()		-- Radar Frequency Interferometer

-- Weapon
devices["GUN_TURRET_CONTROL"]		= counter()		-- gun control system
devices["PYLONS_INPUT"]				= counter()		-- pylons dispatcher
devices["JETT_PANEL_PLT"]			= counter()		-- left console jettison panel
devices["JETT_PANEL_CPG"]			= counter()		-- left console jettison panel
devices["JETT_PANEL_INPUT"]			= counter()
devices["HELLFIRE_INTERFACE"]		= counter()		-- for emulate HF missiles while on board

-- ASE
devices["AN_APR39"]					= counter()		-- AN/APR-39 Radar Warning Receiver (RWR)
devices["CMWS"]						= counter()

--
devices["BRU_PLT"]					= counter()
devices["BRU_CPG"]					= counter()

--
devices["MACROS"]					= counter()
devices["KNEEBOARD"]				= counter()
devices["ARCADE"]					= counter()

-- NVG
devices["NVG"]						= counter()

--AI
devices["PrestonAI"]				= counter()

--
devices["HEAD_WRAPPER"]				= counter()
devices["DATA_TRANSFER_UNIT"]		= counter()

-- IDM
devices["IDM"]						= counter()

devices["AN_AVR2A"]					= counter()		-- AN/AVR-2A Laser Detecting Set (LDS) System
