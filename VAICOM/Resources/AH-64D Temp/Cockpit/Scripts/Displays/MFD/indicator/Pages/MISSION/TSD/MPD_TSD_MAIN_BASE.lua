dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------
local Menu = {}
Menu = 
{ 
	{ pb.T1, "RPT",		nil,	nil},
	{ pb.T2, "PAN",		nil,	nil},
	{ pb.T3, "SHOW",	nil,	nil},
	{ pb.T5, "COORD",	nil,	nil},
	{ pb.T6, "UTIL",	nil,	nil},

	{ pb.L1, "INST",	nil,	nil},
}

local Controls = {}
Controls = 
{
	{ pb.T4, "PP",		tp_default_border,	{{"TSD_PP_Button_frame"}}},

	{ pb.B1, "TSD",		tp_default_border,	nil},
	{ pb.B2, { {"PHASE", nil, nil}, {"NAV", tp_default_border, {{"TSD_PHASE_Btn_Caption"}}, {"NAV", "ATK"}} } },
}

local pos_shift_x = 28
local t3_pocket,t4_pocket = pb_props[pb.T3].pos,pb_props[pb.T4].pos

pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - pos_shift_x*1.2
pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + pos_shift_x*0.8


-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- 8
-- ********** Ownship Sensor Layer **********
AddCompassRose()

-- 9
-- ********** Ownship Layer **********
AddFrozenOwnship()
AddOwnshipSymbol()

-- 10
-- ********** Info Windows and Menus **********
AddMapFrozenCue()

if DBG_SA_WINDOWS_SHOW then
AddSAStatusWindowSmall()
AddSAStatusWindowLarge()
end

createMenu( Menu )
createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = t3_pocket
pb_props[pb.T4].pos = t4_pocket

