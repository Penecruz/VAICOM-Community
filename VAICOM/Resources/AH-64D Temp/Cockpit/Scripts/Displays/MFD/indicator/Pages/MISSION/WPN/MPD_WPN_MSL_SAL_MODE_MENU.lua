dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN MSL SAL MODE MENU",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------
local Controls = {}
Controls = 
{
	{ "MODE",
			{ 
				{ pb.R1, "RIPL",	tp_default_border, {{"WPN_MSL_SAL_MODE_Selection",0}} },
				{ pb.R2, "NORM",	tp_default_border, {{"WPN_MSL_SAL_MODE_Selection",1}} },
				{ pb.R3, "MAN",		tp_default_border, {{"WPN_MSL_SAL_MODE_Selection",2}} }
			},  
	},
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------