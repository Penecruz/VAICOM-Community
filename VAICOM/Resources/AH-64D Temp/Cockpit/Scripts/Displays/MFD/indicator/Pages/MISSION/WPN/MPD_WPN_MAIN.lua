dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
--dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_FUEL_Symbology.lua")

addText( "WPN PAGE MAIN",  {0, 410}, tp_36_white)

--draw_plane( {0, 30},  IND_MPD_MATERIAL_YELLOW)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local MFD_NUM = readParameter("MFD_NUM")

local Menu = {}
Menu = 
{ 
	{ pb.L5, "BORESIGHT",	nil },
}

local Controls = {}
Controls = 
{
	{ pb.L6, "GRAYSCALE",	tp_default_border,	{{"WPN_MAIN_GRAYSCALE", MFD_NUM}}},
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createMenu( Menu, nli, 1, TRANSPARENT_BACKGROUND )
createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------