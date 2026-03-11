dofile(LockOn_Options.common_script_path.."elements_defs.lua")
dofile(LockOn_Options.script_path.."Displays/EUFD/EUFD_definitions.lua")
dofile(LockOn_Options.script_path.."Displays/EUFD/Pages/EUFD_MESSAGES.lua")

local half_width   = GetScale()
local half_height  = GetAspect() * half_width
local aspect       = GetAspect() -- GetHalfHeight()/GetHalfWidth()

background					= CreateElement "ceMeshPoly" -- untextured shape
background.name				= "background"
background.material			= "DBG_RED"
background.h_clip_relation	= h_clip_relations.REWRITE_LEVEL  -- check clipping : pixel on glass then increase level from GLASS_LEVEL to GLASS_LEVEL+1 = HUD_DEFAULT_LEVEL
background.level			= DEFAULT_LEVEL
background.collimated		= false
background.isvisible		= false
background.z_enabled		= true
background.vertices			= { {-1, aspect}, { 1,aspect}, { 1,-aspect}, {-1,-aspect}, }
background.indices			=  {0,1,2 ;  -- first triangle
								0,2,3 }  -- second
Add(background)


AddTextTable("WarningList_1",			-1.0,			aspect,					{{"WarningList", 0}}, 	nil, 	WarningMessages)
AddTextTable("WarningList_2",			-1.0,			aspect*6/7,				{{"WarningList", 1}}, 	nil, 	WarningMessages)
AddTextTable("WarningList_3",			-1.0,			aspect*5/7,				{{"WarningList", 2}}, 	nil, 	WarningMessages)
AddTextTable("WarningList_4",			-1.0,			aspect*4/7,				{{"WarningList", 3}}, 	nil, 	WarningMessages)
AddTextTable("WarningList_5",			-1.0,			aspect*3/7,				{{"WarningList", 4}}, 	nil, 	WarningMessages)
AddTextTable("WarningList_6",			-1.0,			aspect*2/7,				{{"WarningList", 5}}, 	nil, 	WarningMessages)
AddTextTable("WarningList_7",			-1.0,			aspect/7,				{{"WarningList", 6}}, 	nil, 	WarningMessages)

local up = 0.02
AddText("Symbols_1",					-10.0/28.0,		0.003 + aspect,			nil, 					"|",	EUFD_stringdefs_vert_lines_)
AddText("Symbols_2",					-10.0/28.0,		up + aspect*6/7,		nil, 					"|",	EUFD_stringdefs_vert_lines)
AddText("Symbols_3",					-10.0/28.0,		up + aspect*5/7,		nil, 					"|",	EUFD_stringdefs_vert_lines)
AddText("Symbols_4",					-10.0/28.0,		up + aspect*4/7,		nil, 					"|",	EUFD_stringdefs_vert_lines)
AddText("Symbols_5",					-10.0/28.0,		up + aspect*3/7,		nil, 					"|",	EUFD_stringdefs_vert_lines)
AddText("Symbols_6",					-10.0/28.0,		up + aspect*2/7,		{{"Arrows", 0}},		"|",	EUFD_stringdefs_vert_lines)
AddText("Symbols_7",					-10.0/28.0,		up - 0.0025 + aspect/7,	{{"Arrows", 0}},		"|",	EUFD_stringdefs_vert_lines_)

AddText("Arrows_1",						-10.0/28.0,		aspect*2/7,				{{"Arrows", 1}}, 		"_")
AddText("Arrows_2",						-10.0/28.0,		aspect/7,				{{"Arrows", 1}}, 		"_")

AddTextTable("CautionList_1",			-9.0/28.0,		aspect,					{{"CautionList", 0}}, 	nil, 	CautionMessages)
AddTextTable("CautionList_2",			-9.0/28.0,		aspect*6/7,				{{"CautionList", 1}}, 	nil, 	CautionMessages)
AddTextTable("CautionList_3",			-9.0/28.0,		aspect*5/7,				{{"CautionList", 2}}, 	nil, 	CautionMessages)
AddTextTable("CautionList_4",			-9.0/28.0,		aspect*4/7,				{{"CautionList", 3}}, 	nil, 	CautionMessages)
AddTextTable("CautionList_5",			-9.0/28.0,		aspect*3/7,				{{"CautionList", 4}}, 	nil, 	CautionMessages)
AddTextTable("CautionList_6",			-9.0/28.0,		aspect*2/7,				{{"CautionList", 5}}, 	nil, 	CautionMessages)
AddTextTable("CautionList_7",			-9.0/28.0,		aspect/7,				{{"CautionList", 6}}, 	nil, 	CautionMessages)

AddText("Symbols_8",					9.0/28.0,		0.003 + aspect,			nil, 					"|",	EUFD_stringdefs_vert_lines_)
AddText("Symbols_9",					9.0/28.0,		up + aspect*6/7,		nil, 					"|",	EUFD_stringdefs_vert_lines)
AddText("Symbols_10",					9.0/28.0,		up + aspect*5/7,		nil, 					"|",	EUFD_stringdefs_vert_lines)
AddText("Symbols_11",					9.0/28.0,		up + aspect*4/7,		nil, 					"|",	EUFD_stringdefs_vert_lines)
AddText("Symbols_12",					9.0/28.0,		up + aspect*3/7,		nil, 					"|",	EUFD_stringdefs_vert_lines)
AddText("Symbols_13",					9.0/28.0,		up + aspect*2/7,		{{"Arrows", 0}}, 		"|",	EUFD_stringdefs_vert_lines)
AddText("Symbols_14",					9.0/28.0,		up - 0.0025 + aspect/7,	{{"Arrows", 0}}, 		"|",	EUFD_stringdefs_vert_lines_)

AddText("Arrows_3",						9.0/28.0,		aspect*2/7,				{{"Arrows", 1}}, 		"_")
AddText("Arrows_4",						9.0/28.0,		aspect/7,				{{"Arrows", 1}}, 		"_")

AddTextTable("AdvisoryList_1",			10.0/28.0,		aspect,					{{"AdvisoryList", 0}}, 	nil, 	AdvisoryMessages)
AddTextTable("AdvisoryList_2",			10.0/28.0,		aspect*6/7,				{{"AdvisoryList", 1}}, 	nil, 	AdvisoryMessages)
AddTextTable("AdvisoryList_3",			10.0/28.0,		aspect*5/7,				{{"AdvisoryList", 2}}, 	nil, 	AdvisoryMessages)
AddTextTable("AdvisoryList_4",			10.0/28.0,		aspect*4/7,				{{"AdvisoryList", 3}}, 	nil, 	AdvisoryMessages)
AddTextTable("AdvisoryList_5",			10.0/28.0,		aspect*3/7,				{{"AdvisoryList", 4}}, 	nil, 	AdvisoryMessages)
AddTextTable("AdvisoryList_6",			10.0/28.0,		aspect*2/7,				{{"AdvisoryList", 5}}, 	nil, 	AdvisoryMessages)
AddTextTable("AdvisoryList_7",			10.0/28.0,		aspect/7,				{{"AdvisoryList", 6}}, 	nil, 	AdvisoryMessages)


AddTextTable("Idm_VHF",					-1.0,			0.0,					{{"Idm", 0}}, 			nil,	{"", "[", "]", "~"})
AddTextTable("Idm_UHF",					-1.0,			-aspect/7,				{{"Idm", 1}}, 			nil,	{"", "[", "]", "~"})
AddTextTable("Idm_FM1",					-1.0,			-aspect*2/7,			{{"Idm", 2}}, 			nil,	{"", "[", "]", "~"})
AddTextTable("Idm_FM2",					-1.0,			-aspect*3/7,			{{"Idm", 3}}, 			nil,	{"", "[", "]", "~"})
AddTextTable("Idm_HF",					-1.0,			-aspect*4/7,			{{"Idm", 4}}, 			nil,	{"", "[", "]", "~"})

AddTextTable("Rts_VHF_",				-26.75/28.0,	0.0,					{{"Rts", 0, 0}}, 		nil,	{"", "=", "<"})
AddTextTable("Rts_UHF_",				-26.75/28.0,	-aspect/7,				{{"Rts", 0, 1}}, 		nil,	{"", "=", "<"})
AddTextTable("Rts_FM1_",				-26.75/28.0,	-aspect*2/7,			{{"Rts", 0, 2}}, 		nil,	{"", "=", "<"})
AddTextTable("Rts_FM2_",				-26.75/28.0,	-aspect*3/7,			{{"Rts", 0, 3}}, 		nil,	{"", "=", "<"})
AddTextTable("Rts_HF_",					-26.75/28.0,	-aspect*4/7,			{{"Rts", 0, 4}}, 		nil,	{"", "=", "<"})

AddTextTable("Rts__VHF",				-25.85/28.0,	0.0,					{{"Rts", 1, 0}}, 		nil,	{"", "=", ">"})
AddTextTable("Rts__UHF",				-25.85/28.0,	-aspect/7,				{{"Rts", 1, 1}}, 		nil,	{"", "=", ">"})
AddTextTable("Rts__FM1",				-25.85/28.0,	-aspect*2/7,			{{"Rts", 1, 2}}, 		nil,	{"", "=", ">"})
AddTextTable("Rts__FM2",				-25.85/28.0,	-aspect*3/7,			{{"Rts", 1, 3}}, 		nil,	{"", "=", ">"})
AddTextTable("Rts__HF",					-25.85/28.0,	-aspect*4/7,			{{"Rts", 1, 4}}, 		nil,	{"", "=", ">"})

AddText("Radio_VHF",					-25.0/28.0,		0.0,					nil, 					"VHF")
AddText("Radio_UHF",					-25.0/28.0,		-aspect/7,				nil, 					"UHF")
AddText("Radio_FM1",					-25.0/28.0,		-aspect*2/7,			nil, 					"FM1")
AddText("Radio_FM2",					-25.0/28.0,		-aspect*3/7,			nil, 					"FM2")
AddText("Radio_HF",						-25.0/28.0,		-aspect*4/7,			nil, 					"HF ")

AddText("Squelch_VHF",					-22.0/28.0,		0.0,					{{"Squelch", 0}}, 		"*")
AddText("Squelch_UHF",					-22.0/28.0,		-aspect/7,				{{"Squelch", 1}}, 		"*")
AddText("Squelch_FM1",					-22.0/28.0,		-aspect*2/7,			{{"Squelch", 2}}, 		"*")
AddText("Squelch_FM2",					-22.0/28.0,		-aspect*3/7,			{{"Squelch", 3}}, 		"*")
AddText("Squelch_HF",					-22.0/28.0,		-aspect*4/7,			{{"Squelch", 4}}, 		"*")

AddText("Frequency_VHF",				-19.0/28.0,		0.0,					{{"Frequency", 0, 0}}, 	nil)
AddText("Frequency_UHF",				-19.0/28.0,		-aspect/7,				{{"Frequency", 1, 1}}, 	nil)
AddText("Frequency_FM1",				-19.0/28.0,		-aspect*2/7,			{{"Frequency", 2, 2}}, 	nil)
AddText("Frequency_FM2",				-19.0/28.0,		-aspect*3/7,			{{"Frequency", 3, 3}}, 	nil)
AddText("Frequency_HF",					-19.0/28.0,		-aspect*4/7,			{{"Frequency", 4, 4}}, 	nil)

AddText("Call_VHF",						-9.0/28.0,		0.0,					{{"Call", 0}}, 			nil)
AddText("Call_UHF",						-9.0/28.0,		-aspect/7,				{{"Call", 1}}, 			nil)
AddText("Call_FM1",						-9.0/28.0,		-aspect*2/7,			{{"Call", 2}}, 			nil)
AddText("Call_FM2",						-9.0/28.0,		-aspect*3/7,			{{"Call", 3}}, 			nil)
AddText("Call_HF",						-9.0/28.0,		-aspect*4/7,			{{"Call", 4}}, 			nil)
AddText("RadioStats_HF",				-9.0/28.0,		-aspect*5/7,			{{"Status", 0}}, 		nil, 	HF_RADIO_STATUS)

AddText("Cipher_UHF",					-3.0/28.0,		-aspect/7,				{{"Cipher", 0}}, 		nil)
AddText("Cipher_FM1",					-3.0/28.0,		-aspect*2/7,			{{"Cipher", 1}}, 		nil)
AddText("Cipher_FM2",					-3.0/28.0,		-aspect*3/7,			{{"Cipher", 2}}, 		nil)
AddText("Cipher_HF",					-3.0/28.0,		-aspect*4/7,			{{"Cipher", 3}}, 		nil)

AddTextTable("Guard",					0.0,			-aspect/7,				{{"Status", 1}}, 		nil,	{"", "G"})
AddTextTable("PowerStatus_FM1",			0.0,			-aspect*2/7,			{{"Status", 2}}, 		nil,	{"", "OFF", "LOW", "NORM", "HIGH"})
AddTextTable("PowerStatus_HF",			0.0,			-aspect*4/7,			{{"Status", 3}}, 		nil,	{"", "LOW", "MEDIUM", "HIGH"})

AddText("Net_VHF",						5.0/28.0,		0.0,					{{"Net", 0}}, 			nil)
AddText("Net_UHF",						5.0/28.0,		-aspect/7,				{{"Net", 1}}, 			nil)
AddText("Net_FM1",						5.0/28.0,		-aspect*2/7,			{{"Net", 2}}, 			nil)
AddText("Net_FM2",						5.0/28.0,		-aspect*3/7,			{{"Net", 3}}, 			nil)
AddText("Net_HF",						5.0/28.0,		-aspect*4/7,			{{"Net", 4}}, 			nil)

AddText("TI_VHF",						7.0/28.0,		0.0,					{{"TI", 0}}, 			"+")
AddText("TI_UHF",						7.0/28.0,		-aspect/7,				{{"TI", 1}}, 			"+")
AddText("TI_FM1",						7.0/28.0,		-aspect*2/7,			{{"TI", 2}}, 			"+")
AddText("TI_FM2",						7.0/28.0,		-aspect*3/7,			{{"TI", 3}}, 			"+")

AddText("Frequency_Standby_VHF",		10.0/28.0,		0.0,					{{"Frequency", 5, 0}}, 	nil)
AddText("Frequency_Standby_UHF",		10.0/28.0,		-aspect/7,				{{"Frequency", 6, 1}}, 	nil)
AddText("Frequency_Standby_FM1",		10.0/28.0,		-aspect*2/7,			{{"Frequency", 7, 2}}, 	nil)
AddText("Frequency_Standby_FM2",		10.0/28.0,		-aspect*3/7,			{{"Frequency", 8, 3}}, 	nil)
AddText("Frequency_Standby_HF",			10.0/28.0,		-aspect*4/7,			{{"Frequency", 9, 4}},	nil)

AddText("Call_Standby_VHF",				20.0/28.0,		0.0,					{{"Call", 5}}, 			nil)
AddText("Call_Standby_UHF",				20.0/28.0,		-aspect/7,				{{"Call", 6}}, 			nil)
AddText("Call_Standby_FM1",				20.0/28.0,		-aspect*2/7,			{{"Call", 7}}, 			nil)
AddText("Call_Standby_FM2",				20.0/28.0,		-aspect*3/7,			{{"Call", 8}}, 			nil)
AddText("Call_Standby_HF",				20.0/28.0,		-aspect*4/7,			{{"Call", 9}}, 			nil)

AddText("Net_Standby_VHF",				26.0/28.0,		0.0,					{{"Net", 5}}, 			nil)
AddText("Net_Standby_UHF",				26.0/28.0,		-aspect/7,				{{"Net", 6}}, 			nil)
AddText("Net_Standby_FM1",				26.0/28.0,		-aspect*2/7,			{{"Net", 7}}, 			nil)
AddText("Net_Standby_FM2",				26.0/28.0,		-aspect*3/7,			{{"Net", 8}}, 			nil)
AddText("Net_Standby_HF",				26.0/28.0,		-aspect*4/7,			{{"Net", 9}}, 			nil)

AddText("Fuel",							-1.0,			-aspect*6/7,			nil, 					"FUEL")
AddText("Fuel_",						-23.0/28.0,		-aspect*6/7,			{{"Fuel"}}, 			nil)

AddText("Transponder_ID",				-9.0/28.0,		-aspect*6/7,			{{"Transponder", 0}}, 	nil)
AddText("XPNDR_MODE_S",					-3.0/28.0,		-aspect*6/7,			{{"Transponder", 1}}, 	nil)
AddText("Transponder_MODE_3A",			-1.0/28.0,		-aspect*6/7,			{{"Transponder", 2}}, 	nil)
AddText("XPNDR_MODE_4",					4.0/28.0,		-aspect*6/7,			{{"Transponder", 3}}, 	nil)
AddTextTable("Transponder_MC",			6.0/28.0,		-aspect*6/7,			{{"Transponder", 4}}, 	nil,	 {"", "NORM", "STBY"})


AddText("StopWatch_",					18.0/28.0,		-aspect*5/7,			{{"StopWatch"}}, 		nil)
AddText("Watch_",						18.0/28.0,		-aspect*6/7,			{{"Watch"}}, 			nil)

