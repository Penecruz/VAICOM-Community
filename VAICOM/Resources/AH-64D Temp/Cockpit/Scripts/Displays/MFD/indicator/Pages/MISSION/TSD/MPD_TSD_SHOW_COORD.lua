dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{
	{ pb.T3, "SHOW",		tp_default_darkgreen_border,	nil},
	{ pb.T3, "SHOW",		nil,							nil},
	{ pb.T4, "SA",			nil,							nil},
	
	{ pb.T5, "THRT\nSHOW",	nil,							nil},
	{ pb.T6, "COORD\nSHOW",	tp_default_border,				nil},
}

local Controls = {}
Controls = 
{
	{ pb.L2, "CONTROL MEASURES",		tp_default_border,	{{"TSD_SHOW_ControlMeasures_Btn_Frame"}}},						-- default ON
	{ pb.L3, "FRIENDLY UNITS",			tp_default_border,	{{"TSD_SHOW_FriendlyUnits_Btn_Frame"}}},						-- default ON in ATK phase
	{ pb.L4, "ENEMY UNITS",				tp_default_border,	{{"TSD_SHOW_EnemyUnits_Btn_Frame"}}},							-- default OFF
	{ pb.L5, "PLANNED TGTS/THREATS",	tp_default_border,	{{"TSD_SHOW_PlannedTT_Btn_Frame"}}},							-- default ON in ATK phase

	{ pb.R1, "LINES",					tp_default_border,	{{"TSD_SHOW_Lines_Btn_Frame"}}},								-- default ON
	{ pb.R2, "AREAS",					tp_default_border,	{{"TSD_SHOW_Areas_Btn_Frame"}}},								-- default ON
	{ pb.R3, "SHOT",					tp_default_border,	{{"TSD_SHOW_Shot_Btn_Frame"}},	nil, TSD_ATK_PHASE_PH.name},	-- default ON

	{ pb.B2, { {"PHASE", nil, nil}, {"NAV", tp_default_border, {{"TSD_PHASE_Btn_Caption"}}, {"NAV", "ATK"}} } },
}

local pos_shift_x	= 28
local t2_pocket		= pb_props[pb.T2].pos
local t3_pocket		= pb_props[pb.T3].pos
local t4_pocket		= pb_props[pb.T4].pos

pb_props[pb.T2].pos[1] = pb_props[pb.T2].pos[1] - pos_shift_x*0.7
pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - pos_shift_x*1.2
pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + pos_shift_x*0.8

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- 10

-- 11

-- 12

-- 13
createMenu( Menu )
createControls( Controls )

do
	local text	= "NAVIGATION PHASE"
	AddRoundCornersWindow("PhaseLbl_NAV", {0, pb_props[pb.L1].pos[2]+tp_default.height*2.6}, nil, nil, text, nil, nil, TSD_NAV_PHASE_PH.name)
	text	= "ATTACK PHASE"
	AddRoundCornersWindow("PhaseLbl_ATK", {0, pb_props[pb.L1].pos[2]+tp_default.height*2.6}, nil, nil, text, nil, nil, TSD_ATK_PHASE_PH.name)
end

-- 14

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = t3_pocket
pb_props[pb.T4].pos = t4_pocket
pb_props[pb.T2].pos = t2_pocket
