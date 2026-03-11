dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN MSL SAL SEL MENU",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------
local Controls = {}
Controls = 
{
	{ "SAL SEL",
			{ 
				{ pb.L3, "AUTO",	tp_default_border, {{"WPN_MSL_SAL_SSEL_Selection",0}} },
				{ pb.L4, "SAL 1",	tp_default_border, {{"WPN_MSL_SAL_SSEL_Selection",1}} },
				{ pb.L5, "SAL 2",	tp_default_border, {{"WPN_MSL_SAL_SSEL_Selection",2}} }
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