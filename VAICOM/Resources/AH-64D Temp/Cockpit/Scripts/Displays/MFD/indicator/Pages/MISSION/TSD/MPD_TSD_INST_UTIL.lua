dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{
	{ pb.T3, "SHOW",	nil,	nil},
	{ pb.T6, "UTIL",	tp_default_border,	nil},

	{ pb.L1, "INST",	tp_default_darkgreen_border,	nil},
	{ pb.L1, "INST",	nil,	nil},
}

local TimerControls = {}
TimerControls = 
{
	{ pb.T1, "START",	nil,	{{"TSD_INST_Timer_caption"}}, {"START", "STOP"}},
	{ pb.T2, "RESET",	nil,	nil},
}

local EmergencyControls = {}
EmergencyControls = 
{
	{ pb.B2, "500",		nil,	nil},
	{ pb.B3, "2182",	nil,	nil},
}

local Controls = {}
Controls = 
{
	{ pb.T5, "TUNE",	nil,	nil},
	
	{ pb.B1, "TSD",		nil,	nil},

	{ pb.B4, { {"ID>", nil, nil},	{"?", tp_default_border,  {{"MFD_DataEntryButton_frame",pb.B4}} } } },	
	{ pb.B5, { {"FREQ>", nil, nil},	{"?", tp_default_border,  {{"MFD_DataEntryButton_frame",pb.B5}} } } },	

	{ pb.B6, "{ADF",	nil,	{{"TSD_INST_UTIL_ADF_Caption"}}, {"{ADF","}ADF"}},

	{ pb.R1, { {"MODE", nil, nil}, {"ADF", tp_default_border,  {{"TSD_ADF_MODE_Caption"}}, {"ADF", "ANT"} } } },	

	-- beacon presets
	{ pb.L2, { {"", nil, {{"TSD_ADF_BeaconPreset_ID", 0}}}, {"", nil, {{"TSD_ADF_BeaconPreset_Freq", 0}}} } },
	{ pb.L3, { {"", nil, {{"TSD_ADF_BeaconPreset_ID", 1}}}, {"", nil, {{"TSD_ADF_BeaconPreset_Freq", 1}}} } },
	{ pb.L4, { {"", nil, {{"TSD_ADF_BeaconPreset_ID", 2}}}, {"", nil, {{"TSD_ADF_BeaconPreset_Freq", 2}}} } },
	{ pb.L5, { {"", nil, {{"TSD_ADF_BeaconPreset_ID", 3}}}, {"", nil, {{"TSD_ADF_BeaconPreset_Freq", 3}}} } },
	{ pb.L6, { {"", nil, {{"TSD_ADF_BeaconPreset_ID", 4}}}, {"", nil, {{"TSD_ADF_BeaconPreset_Freq", 4}}} } },

	{ pb.R2, { {"", nil, {{"TSD_ADF_BeaconPreset_ID", 5}}}, {"", nil, {{"TSD_ADF_BeaconPreset_Freq", 5}}} } },
	{ pb.R3, { {"", nil, {{"TSD_ADF_BeaconPreset_ID", 6}}}, {"", nil, {{"TSD_ADF_BeaconPreset_Freq", 6}}} } },
	{ pb.R4, { {"", nil, {{"TSD_ADF_BeaconPreset_ID", 7}}}, {"", nil, {{"TSD_ADF_BeaconPreset_Freq", 7}}} } },
	{ pb.R5, { {"", nil, {{"TSD_ADF_BeaconPreset_ID", 8}}}, {"", nil, {{"TSD_ADF_BeaconPreset_Freq", 8}}} } },
	{ pb.R6, { {"", nil, {{"TSD_ADF_BeaconPreset_ID", 9}}}, {"", nil, {{"TSD_ADF_BeaconPreset_Freq", 9}}} } },
}

local pos_shift_x = 28
local t3_pocket,b3_pocket = pb_props[pb.T3].pos,pb_props[pb.B3].pos
local b4_pocket_up		= pb_props[pb.B4].pos_up
local b4_pocket_down	= pb_props[pb.B4].pos_down

pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - pos_shift_x*1.2
pb_props[pb.B3].pos[1] = pb_props[pb.B3].pos[1] - pos_shift_x
pb_props[pb.B4].pos_up[1]	= pb_props[pb.B4].pos_up[1] + pos_shift_x*1.3
pb_props[pb.B4].pos_down[1]	= pb_props[pb.B4].pos_down[1] + pos_shift_x*1.3

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

if DBG_LABEL_SHOW then
addText( "TSD INST UTIL PAGE",  {0, 350}, tp_36_white)
end

-- 8
-- ********** Ownship Sensor Layer **********
AddCompassRose(true)

-- 9
-- ********** Ownship Layer **********
AddFrozenOwnship()
AddOwnshipSymbol()

-- 10
-- ********** Info Windows and Menus **********
AddMapFrozenCue()
AddCurrentHeadingLabel()
AddNextWaypointHeadingLabel()
AddNextWaypointStatusWindow(nil, {{"NAV_Heading_Valid"},{"TSD_Draw_If_PhaseNAV"},{"TSD_WPStatus_Window"}})
AddEnduranceStatusWindow()
AddWindStatusWindow()
AddGridStatusLabel()

if DBG_SA_WINDOWS_SHOW then
AddSAStatusWindowSmall()
AddSAStatusWindowLarge()
end

AddNDBStatusWindow()

createMenu( Menu )
createControls( Controls )

draw_border_with_caption( pb_props[pb.T1].pos, pb_props[pb.T2].pos,  5, 1, "TIMER", tp_def_top )
createControls( TimerControls,  nil, nil, 0)
AddTimerWindow()

local EMER_Buttons_PH = addPlaceholder("EMER_Buttons_Placeholder", {0,0}, nil, {{"NAV_ADF_getPowered"}})
draw_border_with_caption( pb_props[pb.B2].pos, pb_props[pb.B3].pos,  5, 1, "EMER", tp_def_bottom, EMER_Buttons_PH.name )
createControls( EmergencyControls,  nil, nil, 0)

do
	local arg = 0
	for pb_num=pb.L2,pb.L6, -1 do
		draw_large_border_for_PB( pb_num, {{"TSD_ADF_BeaconPreset_Selection", arg}} )
		arg = arg + 1
	end

	for pb_num=pb.R2,pb.R6, 1 do
		draw_large_border_for_PB( pb_num, {{"TSD_ADF_BeaconPreset_Selection", arg}} )
		arg = arg + 1
	end
end

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = t3_pocket
pb_props[pb.B3].pos = b3_pocket
pb_props[pb.B4].pos_up = b4_pocket_up
pb_props[pb.B4].pos_down = b4_pocket_down

