dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

--addText( "DMS IBIT LISTING",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T3, "IBIT",		tp_default_border },
	{ pb.B1, "DMS",			nil },
}

createMenu( Menu )

ButtonNames = 
{
	"",
	"PRFLT",
	"MAINT",

	"ECS1",
	"ECS2",
	"EPMS1",
	"EPMS2",
	"PWR LEVER",

	"RADIOS",
	"DL",
	"XPNDR",
	"CIU",
	"SWS",--COMM
	"SWS",--COMM

	"VHF",
	"UHF",
	"FM1",
	"FM2",
	"HF",
	
	"KEYBOARD",	
	"EUFD",		
	"LMPD",	
	"RMPD",
	"SWS",	
	
	"KEYBOARD",	
	"EUFD",		
	"LMPD",	
	"RMPD",
	"SWS",	
	
	"GUN",
	"HF MISSILES",
	"ROCKETS",
	"SWS",--WEAPON
	"SWS",--WEAPON
	
	"TESS",
	
	"FCR",
	"TADS CLASS A",--
	"TADS CLASS B",--
	"PNVS CLASS A",--
	"PNVS CLASS B",--
	"IHADSS",
	"SWS",--SIGHTS
	"SWS",--SIGHTS
	
	"DP1",
	"DP2",
	"SP1",
	"SP2",
	"WP1",
	"WP2",
	
	"DTU",
	"MDR",
	
	"DPLR",
	"INU1",	
	"INU2",	
	"RALT",
	
	"RFI",			
	"RJA",			
	"RLW",			
	"SWS",--ASE
	"SWS",--ASE
	
}
	
local Controls = {}
Controls = 
{	
	{ pb.L1, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 1}}, ButtonNames},
	{ pb.L2, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 2}}, ButtonNames},
	{ pb.L3, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 3}}, ButtonNames},
	{ pb.L4, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 4}}, ButtonNames},
	{ pb.L5, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 5}}, ButtonNames},	
	{ pb.L6, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 6}}, ButtonNames},
	
	{ pb.R1, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 7}}, ButtonNames},
	{ pb.R2, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 8}}, ButtonNames},
	{ pb.R3, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 9}}, ButtonNames},
	{ pb.R4, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 10}}, ButtonNames},
	{ pb.R5, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 11}}, ButtonNames},

	{ pb.T4, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 12}}, ButtonNames},
	{ pb.T5, 	"",			tp_default_inv,	{{"DMS_IBIT_ButtonSelect", 13}}, ButtonNames},

	{ pb.B4, 	"ABORT",	nil,	nil},
	{ pb.B5, 	"ACK",		nil,	nil},	
}

createControls( Controls )


function AddCaption(text, framewidth, controllers, name)
	local tp_center	= tp_default
	tp_center.alignment = "CenterCenter"		
	AddRoundCornersWindow(name,	{0.0,(pb_props[pb.L1].pos[2] * 1.3)},
							tp_default.width*framewidth, tp_default.height*1.5,
							{
								{text,		{0.0,	0.0}, tp_center},			
							},
							tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	controllers)
end

AddCaption("FLIGHT CONTROLS PREFLIGHT",				27.0, {{"DMS_IBIT_Caption", 1}}, "Caption_1")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 2}}, "Caption_2")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 3}}, "Caption_3")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 4}}, "Caption_4")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 5}}, "Caption_5")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 6}}, "Caption_6")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 7}}, "Caption_7")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 8}}, "Caption_8")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 9}}, "Caption_9")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 10}}, "Caption_10")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 11}}, "Caption_11")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 12}}, "Caption_12")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 13}}, "Caption_13")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 14}}, "Caption_14")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 15}}, "Caption_15")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 16}}, "Caption_16")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 17}}, "Caption_17")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 18}}, "Caption_18")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 19}}, "Caption_19")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 20}}, "Caption_20")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 21}}, "Caption_21")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 22}}, "Caption_22")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 23}}, "Caption_23")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 24}}, "Caption_24")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 25}}, "Caption_25")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 26}}, "Caption_26")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 27}}, "Caption_27")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 28}}, "Caption_28")
AddCaption("30MM GUN",										10.0, {{"DMS_IBIT_Caption", 29}}, "Caption_29")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 30}}, "Caption_30")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 31}}, "Caption_31")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 32}}, "Caption_32")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 33}}, "Caption_33")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 34}}, "Caption_34")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 35}}, "Caption_35")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 36}}, "Caption_36")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 37}}, "Caption_37")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 38}}, "Caption_38")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 39}}, "Caption_39")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 40}}, "Caption_40")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 41}}, "Caption_41")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 42}}, "Caption_42")
AddCaption("DISPLAY PROCESSOR #1",					22.0, {{"DMS_IBIT_Caption", 43}}, "Caption_43")
AddCaption("DISPLAY PROCESSOR #2",					22.0, {{"DMS_IBIT_Caption", 44}}, "Caption_44")
AddCaption("SYSTEM PROCESSOR #1",					21.0, {{"DMS_IBIT_Caption", 45}}, "Caption_45")
AddCaption("SYSTEM PROCESSOR #2",					21.0, {{"DMS_IBIT_Caption", 46}}, "Caption_46")
AddCaption("WEAPON PROCESSOR #1",					21.0, {{"DMS_IBIT_Caption", 47}}, "Caption_47")
AddCaption("WEAPON PROCESSOR #2",					21.0, {{"DMS_IBIT_Caption", 48}}, "Caption_48")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 49}}, "Caption_49")
AddCaption("",					20.0, {{"DMS_IBIT_Caption", 50}}, "Caption_50")
AddCaption("DOPPLER RADAR",									15.0, {{"DMS_IBIT_Caption", 51}}, "Caption_51")
AddCaption("INERTIAL NAVIGATION SYSTEM 1",					30.0, {{"DMS_IBIT_Caption", 52}}, "Caption_52")
AddCaption("INERTIAL NAVIGATION SYSTEM 2",					30.0, {{"DMS_IBIT_Caption", 53}}, "Caption_53")
AddCaption("RADAR ALTIMETER",								17.0, {{"DMS_IBIT_Caption", 54}}, "Caption_54")
AddCaption("RADAR FREQUENCY INTERFEROMETER",				32.0, {{"DMS_IBIT_Caption", 55}}, "Caption_55")
AddCaption("RADAR JAMMER",									14.0, {{"DMS_IBIT_Caption", 56}}, "Caption_56")
AddCaption("RADAR AND LASER WARNING RECEIVER",				34.0, {{"DMS_IBIT_Caption", 57}}, "Caption_57")
AddCaption("PLT ACFT SRVL EQUIP SWITCHES",					30.0, {{"DMS_IBIT_Caption", 58}}, "Caption_58")
AddCaption("CPG ACFT SRVL EQUIP SWITCHES",					30.0, {{"DMS_IBIT_Caption", 59}}, "Caption_59")

addText( "TEST STATUS:",		{pb_props[pb.T1].pos[1],	350},		tp_def_left)
draw_line( {{pb_props[pb.T1].pos[1],	 	346 - tp_default.height},	{pb_props[pb.T1].pos[1] + tp_default.width*11.0,	346 - tp_default.height}},		tp_default.material)
addText( "",		{0,	300},		tp_def_left, {{"DMS_IBIT_TestStatus"}}, {"", "TEST IN PROGRESS", "TEST ENDED", "TEST FAILED TO RUN", "TEST ABORTED"})
--addText( text, pos, text_properties, controllers, formats, margins,  name, parent )

Faults = 
{
	--"FAULT012345678911111111110000000000",
	"",
	"NO FAULTS FOUND",
	
	"DOPPLER CPU/MEMORY FAIL",
	"DOPPLER INPUT/OUTPUT FAIL",
	"DOPPLER LEF/FREQUENCY TRACK FAIL",
	"DOPPLER MALFUNCTION",
	"DOPPLER MTU FAIL",
	"DOPPLER PROGRAM PLUG NOT CONNECTED",
	"DOPPLER PROGRAM PLUG NOT CONNECTED",
	"DOPPLER RADAR XMTR ASSY FAIL",
	"DPLR SIG DATA CONVRT PWR SPLY FAIL",
	"DPLR SIG DATA CONVRT CMPUTR FAIL",
	"DOPPLER SELFTEST FAIL",
	"DOPPLER TEST FAULT",
	"DOPPLER CH 2 BUS A NO RESPONSE",
	"DOPPLER CH 2 BUS B NO RESPONSE",
	"DOPPLER FAIL",
	"DOPPLER POWER INITIALIZATION FAIL",
	"DOPPLER MODE COMMAND FAIL",
	
	"EGI 1 BATTERY FAIL",
	"EGI 1 SRU A1 GEM MODULE FAIL",
	"EGI 1 SRU A10 INERTIAL MEAS FAIL",
	"EGI 1 SRU A11 CHASSIS FAIL",
	"EGI 1 SRU A2 SYSTEM PROCESSOR FAIL",
	"EGI 1 SRU A5 POWER SUPPLY FAIL",
	"EGI 1 SRU A8 INERTIAL ELECT FAIL",
	"EGI 1 RECEIVER PROCESSOR UNIT FAIL",
	"EGI 1 CHANNEL 2 BUS A NO RESPONSE",
	"EGI 1 CHANNEL 2 BUS B NO RESPONSE",
	
	"EGI 2 BATTERY FAIL",
	"EGI 2 SRU A1 GEM MODULE FAIL",
	"EGI 2 SRU A10 INERTIAL MEAS FAIL",
	"EGI 2 SRU A11 CHASSIS FAIL",
	"EGI 2 SRU A2 SYSTEM PROCESSOR FAIL",
	"EGI 2 SRU A5 POWER SUPPLY FAIL",
	"EGI 2 SRU A8 INERTIAL ELECT FAIL",
	"EGI 2 RECEIVER PROCESSOR UNIT FAIL",
	"EGI 2 CHANNEL 2 BUS A NO RESPONSE",
	"EGI 2 CHANNEL 2 BUS B NO RESPONSE",
	
	"RADAR ALTIMETER PRESS TO TEST FAIL",
	
	"ELC 1 RDR WRN RCVR PWR CNTL FAIL",
	"RADAR WARNING LH FWD CHANNEL FAULT",
	"RADAR WARNING RH FWD CHANNEL FAULT",
	"RADAR WARNING LH AFT CHANNEL FAULT",
	"RADAR WARNING RH AFT CHANNEL FAULT",
	"RADAR WARNING CD BAND CHANNEL FAULT",
	"RADAR WARNING DP/1553 SRU FAULT",
	"RADAR WARNING CBIT FAULT",
	"RWR CHANNEL 2 BUS A NO RESPONSE",
	"RWR CHANNEL 2 BUS B NO RESPONSE",
	"RWR FAIL",
	
	"SP 1/2 SRU 2 FAIL",
	"SP 1/2 SRU 3 FAIL",
	"ELC 1 CHAFF ARM CONTROL FAIL",
	"PLT CYCLIC CHAFF SWITCH FAIL",
	"CPG CYCLIC CHAFF SWITCH FAIL",
	
	"SP 1/2 SRU 1 FAIL",
	"SP 1/2 SRU 2 FAIL",
	"SP 1/2 SRU 3 FAIL",
	"ELC 2 RADAR JAMMER CONTROL FAIL",
	"RADAR JAMMER TWTA FAULT",
	"RADAR JAMMER CKT BRKR FAULT",
	"RADAR JAMMER POWER FAULT",
	"RADAR JAMMER NO-GO FAULT",
	"RADAR JAMMER FAIL",
	
	"HPSM1 KD107 CONTACTOR FAIL",
	"HPSM1 RFI CKT BRKR",
	"FCR LRU 3 RFI PROC FAIL",
	"FCR MMA 4A6 RFI ANT",
	"FCR MMA 4A7 RFI RCVR",
	"RFI BLADE POSITION INDICATOR DEGRADED",
	"RFI FAIL",
	"RFI DEGRADED",
	"RFI CHANNEL 3 BUS A NO RESPONSE",
	"RFI CHANNEL 3 BUS B NO RESPONSE",
	"RFI FAIL",
	
	
	"APU FUEL SHUTOFF VALVE - CLOSE FAIL",
	"APU FUEL SHUTOFF VALVE - OPEN FAIL",
	"APU FAIL - GAS GENERATOR RPM OVRSPD",
	"APU FAIL - OIL PRESSURE LOW",
	"APU FAIL - OVERTEMPERATURE",
	"APU FAIL - OVERCURRENT",
	"APU ECU FUEL SOLENOID CNTLR FAIL",
	"APU ECU STARTER VALVE CNTLR FAIL",
	"APU ECU IGNITION CNTLR FAIL",
	"APU ECU PTO CLUTCH CNTLR FAIL",
	"APU ECU STOP CONTROL FAIL",
	"APU ECU START/STOP COMMAND FAIL",
	"APU START SEQ FAIL - RPM DECREASING",
	"APU START SEQ FAIL - NO IGN SPARK",
	"APU EXHAUST GAS TEMP SENSOR FAIL",
	"APU LOW OIL PRESSURE SWITCH FAIL",
	"ELC2 PTO CLUTCH CONTROL FAIL",
	"ELC2 APU ECU PWR CONTROL FAIL",
	"ELC2 APU ECU START CONTROL FAIL",
	"ELC2 APU FUEL BOOST CONTROL FAIL",
	"ELC2 APU FUEL SOV CONTROL FAIL", 
	
	
	
}

addText( TestFault,  {pb_props[pb.T1].pos[1],	300},		tp_def_left,		{{"DMS_IBIT_Fault", 0}},	Faults,	nil, "Fault_0",		nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	250},		tp_def_left,		{{"DMS_IBIT_Fault", 1}}, 	Faults,	nil, "Fault_1",		nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	200},		tp_def_left,		{{"DMS_IBIT_Fault", 2}}, 	Faults,	nil, "Fault_2",		nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	150},		tp_def_left,		{{"DMS_IBIT_Fault", 3}}, 	Faults,	nil, "Fault_3",		nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	100},		tp_def_left,		{{"DMS_IBIT_Fault", 4}}, 	Faults,	nil, "Fault_4",		nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	 50},		tp_def_left,		{{"DMS_IBIT_Fault", 5}}, 	Faults,	nil, "Fault_5",		nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	  0},		tp_def_left,		{{"DMS_IBIT_Fault", 6}}, 	Faults,	nil, "Fault_6",		nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	-50},		tp_def_left,		{{"DMS_IBIT_Fault", 7}}, 	Faults,	nil, "Fault_7",		nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	-100},		tp_def_left,		{{"DMS_IBIT_Fault", 8}}, 	Faults,	nil, "Fault_8",		nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	-150},		tp_def_left,		{{"DMS_IBIT_Fault", 9}}, 	Faults,	nil, "Fault_9",		nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	-200},		tp_def_left,		{{"DMS_IBIT_Fault", 10}},	Faults,	nil, "Fault_10",	nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	-250},		tp_def_left,		{{"DMS_IBIT_Fault", 11}},	Faults,	nil, "Fault_11",	nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	-300},		tp_def_left,		{{"DMS_IBIT_Fault", 12}},	Faults,	nil, "Fault_12",	nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	-350},		tp_def_left,		{{"DMS_IBIT_Fault", 13}},	Faults,	nil, "Fault_13",	nil)
addText( TestFault,  {pb_props[pb.T1].pos[1],	-400},		tp_def_left,		{{"DMS_IBIT_Fault", 14}},	Faults,	nil, "Fault_14",	nil)

