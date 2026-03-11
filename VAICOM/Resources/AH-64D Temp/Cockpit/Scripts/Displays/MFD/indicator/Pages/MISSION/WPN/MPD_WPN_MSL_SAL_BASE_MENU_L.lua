dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------
local Controls = {}
Controls = 
{
	{ pb.L3, { {"SAL SEL", nil, nil}, {"SAL1", tp_default_border, {{"WPN_MSL_SAL_SSEL_Caption"}}, {"AUTO","SAL1","SAL2"}} } },
	
	{ pb.L5, "DEICE",		nil,	nil},
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------