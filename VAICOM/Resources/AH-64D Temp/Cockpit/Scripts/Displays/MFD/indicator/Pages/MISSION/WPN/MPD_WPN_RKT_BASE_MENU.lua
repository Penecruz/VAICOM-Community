dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

tprops = createTextProperty( nil, "RED", IND_MPD_MATERIAL_RED, "LeftTop" )

local Controls = {}
Controls = 
{
	{ pb.R1, { {"QTY", nil, nil}, {"4", tp_default_border, {{"WPN_RKT_QTY_Caption"}}, {"1","2","4","8","12","24","ALL" }} } },
	{ pb.R2, { {"PEN", nil, nil}, {"SPQ", tp_default_border, {{"WPN_RKT_PEN_Caption"}}, {"SPQ","10","15","20","25","BNK","30","35","40","45" }} } },
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------