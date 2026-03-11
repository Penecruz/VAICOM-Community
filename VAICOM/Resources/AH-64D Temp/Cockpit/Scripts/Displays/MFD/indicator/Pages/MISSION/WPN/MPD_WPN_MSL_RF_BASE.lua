dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN MSL RF BASE PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------
local PWR_Controls = {}
PWR_Controls = 
{
	-- { "MSL PWR",
				-- { 
					{ pb.L1, "ALL",		tp_default_border, 	{{"WPN_MSL_RF_PWR_Show"},{"WPN_MSL_RF_PWR_Selection",0}} }, 
					{ pb.L2, "AUTO",	tp_default_border,	{{"WPN_MSL_RF_PWR_Show"},{"WPN_MSL_RF_PWR_Selection",1}} },
					{ pb.L3, "NONE",	tp_default_border, 	{{"WPN_MSL_RF_PWR_Show"},{"WPN_MSL_RF_PWR_Selection",2}} },
				-- } 
	-- },
}

local Controls = {}
Controls = 
{
	{ pb.L5, "LOBL INHIBIT",		tp_default_border,	{{"WPN_MSL_RF_LOBL_INH_Frame"}}},
	{ pb.L6, "2ND TARGET INHIBIT",	tp_default_border,	{{"WPN_MSL_RF_2ND_TARGET_Frame"}}},
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

local MSL_PWR_FrameBase = addPlaceholder("MSL_PWR_FrameBase_PH", {0, 0}, nil, {{"WPN_MSL_RF_PWR_Show"}})
draw_border_with_caption( pb_props[pb.L1].pos, pb_props[pb.L3].pos,  4, 1, "MSL PWR", pb_props[pb.L1].tp, MSL_PWR_FrameBase.name, TRANSPARENT_BACKGROUND )
createControls( PWR_Controls,  nil, nil, 0, TRANSPARENT_BACKGROUND)

createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------