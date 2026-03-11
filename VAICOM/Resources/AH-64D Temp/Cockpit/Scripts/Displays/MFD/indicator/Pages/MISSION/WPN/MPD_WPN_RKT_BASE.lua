dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN RKT BASE PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local tp_border_nomargins = createTextProperty( 34, nil, nil, nil, nil, true, {3,23,0,0} )

local Controls = {}
Controls = 
{
	--{ "INVENTORY",
	--			{ 
					-- just for frames around both string lines
					{ pb.L1, "   \n  ",	tp_border_nomargins,	{{"WPN_RKT_Inventory_Show",0},{"WPN_RKT_Inventory_Selection",0}} }, 
					{ pb.L2, "   \n  ",	tp_border_nomargins,	{{"WPN_RKT_Inventory_Show",1},{"WPN_RKT_Inventory_Selection",1}} },
					{ pb.L3, "   \n  ",	tp_border_nomargins,	{{"WPN_RKT_Inventory_Show",2},{"WPN_RKT_Inventory_Selection",2}} },
					{ pb.L4, "   \n  ",	tp_border_nomargins,	{{"WPN_RKT_Inventory_Show",3},{"WPN_RKT_Inventory_Selection",3}} },
					{ pb.L5, "   \n  ",	tp_border_nomargins,	{{"WPN_RKT_Inventory_Show",4},{"WPN_RKT_Inventory_Selection",4}} },
	--			} 
	--},	
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

local pos_dn = {pb_props[pb.L5].pos[1], pb_props[pb.L5].pos[2]-20}
draw_border_with_caption( pb_props[pb.L1].pos, pos_dn,  3.5, 1, "INVENTORY", pb_props[pb.L5].tp, nil, TRANSPARENT_BACKGROUND )
createControls( Controls,  nil, nil, 0, TRANSPARENT_BACKGROUND)

local controlItemParams = {}
controlItemParams = tp_def_left

for i=0,4 do
	local pb_num = pb.L1-i
	
	controlItemParams.pos = pb_props[pb_num].pos_up

	local controlItem = { pb_num, "6MP", controlItemParams, {{"WPN_RKT_Inventory_Show",i},{"WPN_RKT_Inventory_WarheadType",i}}, WPN_RKT_WARHEAD_TYPES}
	createControlItem( controlItem, nil, nil, nil, TRANSPARENT_BACKGROUND )

	controlItemParams.pos = pb_props[pb_num].pos_down
	
	controlItem = { pb_num, "6", controlItemParams, {{"WPN_RKT_Inventory_Show",i},{"WPN_RKT_Inventory_AvailableCount",i}}}
	createControlItem( controlItem, nil, nil, nil, TRANSPARENT_BACKGROUND )
end

Add_RKT_TotalRocketsWindow()

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------