dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T1, "RPT",		nil,	nil},
	{ pb.T2, "PAN",		nil,				nil},

	{ pb.B3, "BAM",		nil,	nil},
	{ pb.B4, "MAP",		nil,	nil},
	{ pb.B5, "RTE",		nil,	nil},
	{ pb.B6, "POINT",	nil,	nil}
}

local Controls = {}
Controls = 
{
	{ pb.B1, "TSD",			nil,	nil},
}

local pos_shift_x = 28
local t4_pocket = pb_props[pb.T4].pos
local b3_pocket,b4_pocket = pb_props[pb.B3].pos, pb_props[pb.B4].pos

pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + pos_shift_x*0.8
pb_props[pb.B3].pos[1] = pb_props[pb.B3].pos[1] - pos_shift_x
pb_props[pb.B4].pos[1] = pb_props[pb.B4].pos[1] + pos_shift_x

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

if DBG_LABEL_SHOW then
addText( "TSD SHOW PAGE",  {0, 350}, tp_36_white)
end

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
AddTSDInfoStatusWindows()

if DBG_SA_WINDOWS_SHOW then
AddSAStatusWindowSmall()
AddSAStatusWindowLarge()
end

createMenu( Menu )
createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T4].pos = t4_pocket
pb_props[pb.B3].pos = b3_pocket
pb_props[pb.B4].pos = b4_pocket

