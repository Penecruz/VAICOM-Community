dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

local REPLACE_IT_WITH_PROPER_CONTROLLER = nil
-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------
local Controls = {}
Controls = 
{
	{ pb.R4, "TRAIN",		nil,	nil},
	--{ pb.R4, "TRAIN",		nil,	{{"WPN_TrainBtn_Show",0}, {"WPN_TrainBtn_Frame"}} },
	--{ pb.R4, "AKI{",		nil,	{{"WPN_TrainBtn_Show",1}, {REPLACE_IT_WITH_PROPER_CONTROLLER}}, {"AKI{","AKI}"} },
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

draw_disable_line( pb.R4,  IND_MPD_1024_MATERIAL_DARK_GREEN )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------