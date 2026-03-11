dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN UTIL LOAD PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T1, "CHAN",	nil },
	{ pb.T2, "ASE",		tp_default_border, 	{{"MFD_AsePrevBorder", 0}} },
	{ pb.T4, "CODE",	nil },
	{ pb.T5, "COORD",	nil },
	{ pb.T6, "UTIL",	tp_default_darkgreen_border },
	{ pb.T6, "UTIL",	nil },

	{ pb.B1, "WPN",		nil },
	{ pb.B6, "LOAD",	tp_default_border },
}

local Zones_Controls = {}
Zones_Controls = 
{
	--{ "RKT INV",
	--			{ 
					{ pb.R2, { {"ZONE A", nil, nil}, {"6RC", tp_default_border, {{"WPN_RKT_LOAD_Zone_WarheadType", 0}}, WPN_RKT_WARHEAD_TYPES} } },
					{ pb.R3, { {"ZONE B", nil, nil}, {"6RC", tp_default_border, {{"WPN_RKT_LOAD_Zone_WarheadType", 1}}, WPN_RKT_WARHEAD_TYPES} } },
					{ pb.R4, { {"ZONE C", nil, nil}, {"6RC", tp_default_border, {{"WPN_RKT_LOAD_Zone_WarheadType", 2}}, WPN_RKT_WARHEAD_TYPES} } },
					{ pb.R5, { {"ZONE D", nil, nil}, {"6RC", tp_default_border, {{"WPN_RKT_LOAD_Zone_WarheadType", 3}}, WPN_RKT_WARHEAD_TYPES} } },
					{ pb.R6, { {"ZONE E", nil, nil}, {"6RC", tp_default_border, {{"WPN_RKT_LOAD_Zone_WarheadType", 4}}, WPN_RKT_WARHEAD_TYPES} } },
	--			} 
	--},		
}

local Controls = {}
Controls = 
{
	{ pb.R1, { {"GUN ROUNDS >", nil, nil}, {"1200", tp_default_border, {{"WPN_GUN_RoundsCounter_Value"},{"MFD_DataEntryButton_frame",pb.R1}}} } },
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

local pos_dn = {pb_props[pb.R6].pos[1], pb_props[pb.R6].pos[2]-tp_default.height*0.9}
draw_border_with_caption( pb_props[pb.R2].pos, pos_dn,  6, 1, "RKT INV", pb_props[pb.R2].tp, nil, TRANSPARENT_BACKGROUND )
createControls( Zones_Controls,  nil, nil, 0, TRANSPARENT_BACKGROUND)

createMenu( Menu, nil, 1, TRANSPARENT_BACKGROUND )
createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------