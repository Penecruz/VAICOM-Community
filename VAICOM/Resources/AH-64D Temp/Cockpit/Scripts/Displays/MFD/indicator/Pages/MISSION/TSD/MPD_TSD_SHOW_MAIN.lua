dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------
	
local MFD_NUM = readParameter("MFD_NUM")

local CURSR_lbl = "PILOT CURSOR"

if MFD_NUM < MFD_SELF_IDS.CPG_LMFD then 	-- PLT
	CURSR_lbl = "CPG CURSOR"
end
	
local Menu = {}
Menu = 
{
	{ pb.T3, "SHOW",		tp_default_border,	nil},
	{ pb.T4, "SA",			nil,				nil},
	
	{ pb.T5, "THRT\nSHOW",	nil, 				nil},
	{ pb.T6, "COORD\nSHOW",	nil, 				nil},
}

local Controls = {}
Controls = 
{
	{ pb.L2, "WAYPOINT DATA",		tp_default_border,	{{"TSD_WPData_Btn_Frame"}}, nil, TSD_NAV_PHASE_PH.name},	-- default ON
	{ pb.L2, "CURRENT ROUTE",		tp_default_border,	{{"TSD_SHOW_CURROUTE_Button_frame"}},	nil, TSD_ATK_PHASE_PH.name},
	{ pb.L3, "INACTIVE ZONES",		tp_default_border,	{{"TSD_SHOW_INACT_ZONES_Btn_Frame"}}},			-- default OFF
	{ pb.L5, CURSR_lbl,				tp_default_border,	{{"TSD_OtherCrewmemberCursor_Btn_Frame"}}},		-- default OFF
	{ pb.L6, "CURSR\nINFO",			tp_default_border,	{{"TSD_CursorInfoBtn_Frame"}}},					-- default OFF

	{ pb.B2, { {"PHASE", nil, nil}, {"NAV", tp_default_border, {{"TSD_PHASE_Btn_Caption"}}, {"NAV", "ATK"}} } },

	{ pb.R4, "HSI",		tp_default_border,	{{"TSD_SHOW_HSI_Btn_Frame"}}},	-- default OFF
	{ pb.R5, "ENDR",	tp_default_border,	{{"TSD_ENDR_Btn_Frame"}}},		-- default ON
	{ pb.R6, "WIND",	tp_default_border,	{{"TSD_WIND_Btn_Frame"}}},		-- default ON
	
	{ pb.L4, "OBSTACLES",			tp_default_border,	{{"TSD_SHOW_OBS_Button_frame"}},		nil, TSD_NAV_PHASE_PH.name},
	{ pb.L4, "FCR TGTS/OBSTACLES",	tp_default_border,	{{"TSD_SHOW_OBS_Button_frame"}},		nil, TSD_ATK_PHASE_PH.name}
}

local pos_shift_x = 28
local t1_pocket,t2_pocket,t3_pocket = pb_props[pb.T1].pos, pb_props[pb.T2].pos, pb_props[pb.T3].pos
local t4_pocket				= pb_props[pb.T4].pos

pb_props[pb.T1].pos[1] = pb_props[pb.T1].pos[1] - pos_shift_x*0.5
pb_props[pb.T2].pos[1] = pb_props[pb.T2].pos[1] - pos_shift_x*0.7
pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - pos_shift_x*1.2
pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + pos_shift_x*0.8


-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

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

pb_props[pb.T1].pos = t1_pocket
pb_props[pb.T2].pos = t2_pocket
pb_props[pb.T3].pos = t3_pocket
pb_props[pb.T4].pos = t4_pocket

