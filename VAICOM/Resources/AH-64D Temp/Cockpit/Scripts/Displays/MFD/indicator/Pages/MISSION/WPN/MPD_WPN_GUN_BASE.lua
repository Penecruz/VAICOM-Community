dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN GUN BASE PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local MFD_NUM = readParameter("MFD_NUM")

local Controls = {}
Controls = 
{
	{ "BURST LIMIT",
				{ 
					{ pb.L1, "10",		nil, 	{{"WPN_GUN_BurstLimit_Selection",0}} }, 
					{ pb.L2, "20",		nil,	{{"WPN_GUN_BurstLimit_Selection",1}} },
					{ pb.L3, "50",		nil, 	{{"WPN_GUN_BurstLimit_Selection",2}} },
					{ pb.L4, "100",		nil, 	{{"WPN_GUN_BurstLimit_Selection",3}} },
					{ pb.L5, "ALL",		nil, 	{{"WPN_GUN_BurstLimit_Selection",4}} }
				} 
	},
}

if MFD_NUM >= MFD_SELF_IDS.CPG_LMFD then			-- CPG only
	local controlItem = { pb.L6, "HARMONIZE", tp_default_border, {{"WPN_GUN_HARMONIZE_Frame"}}}
	createControlItem( controlItem, nil, nil, 1, TRANSPARENT_BACKGROUND )
end

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createControls( Controls, nil, nil, 0, TRANSPARENT_BACKGROUND )

Add_GUN_DH_HintWindow()

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------