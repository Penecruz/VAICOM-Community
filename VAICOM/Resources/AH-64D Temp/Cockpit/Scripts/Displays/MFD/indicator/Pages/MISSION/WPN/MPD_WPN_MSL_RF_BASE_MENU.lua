dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN MSL RF ACT PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------
local Controls = {}
Controls = 
{
	{ pb.R1, { {"TYPE", nil, nil}, {"RF", tp_default_border, nil} } },
	{ pb.R2, { {"MODE", nil, nil}, {"NORM", tp_default_border, {{"WPN_MSL_RF_MODE_Selection"}}, {"NORM","MAN"}} } },
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------