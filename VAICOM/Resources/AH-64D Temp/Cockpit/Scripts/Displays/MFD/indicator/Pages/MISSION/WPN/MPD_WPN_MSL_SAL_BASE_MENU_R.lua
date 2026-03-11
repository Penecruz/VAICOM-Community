dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN MSL SAL ACT PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------
local Controls = {}
Controls = 
{
	{ pb.R1, { {"TYPE", nil, nil}, {"SAL", tp_default_border, nil} } },
	{ pb.R2, { {"MODE", nil, nil}, {"NORM", tp_default_border, {{"WPN_MSL_SAL_MODE_Caption"}}, {"RIPL","NORM","MAN"}} } },
	{ pb.R3, { {"TRAJ", nil, nil}, {"DIR", tp_default_border, {{"WPN_MSL_SAL_TRAJ_Caption"}}, {"HI","LO","DIR"}} } },
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------