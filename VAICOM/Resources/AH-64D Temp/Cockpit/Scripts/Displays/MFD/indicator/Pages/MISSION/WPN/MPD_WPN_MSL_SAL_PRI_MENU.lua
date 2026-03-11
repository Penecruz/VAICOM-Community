dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN MSL PRI MENU",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local tp_border_nomargins = createTextProperty( 36, nil, nil, nil, nil, true, {0,25,0,0} )

draw_border_with_caption( pb_props[pb.L1].pos_up, pb_props[pb.L5].pos,  6, 1, "PRIORITY", pb_props[pb.L5].tp, nil, TRANSPARENT_BACKGROUND )
local Controls_Chan = {}
Controls_Chan = 
{
	-- just for frames around both string lines
	{ pb.L1, "   \n  ",	tp_border_nomargins,	{{"WPN_MSL_SAL_PRI_Channel_Selection",0}} }, 
	{ pb.L2, "   \n  ",	tp_border_nomargins,	{{"WPN_MSL_SAL_PRI_Channel_Selection",1}} },
	{ pb.L3, "   \n  ",	tp_border_nomargins,	{{"WPN_MSL_SAL_PRI_Channel_Selection",2}} },
	{ pb.L4, "   \n  ",	tp_border_nomargins,	{{"WPN_MSL_SAL_PRI_Channel_Selection",3}} },

	{ pb.L5, "NONE",	tp_default_border,		{{"WPN_MSL_SAL_PRI_Channel_Selection",4}} },
}

local Controls = {}
Controls = 
{
	{ pb.L6, "{MSL CCM",	nil,	{{"WPN_MSL_SAL_CCM_Selected"}}, {"{MSL CCM","}MSL CCM"}},
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createControls( Controls_Chan, nil, nil, 0, TRANSPARENT_BACKGROUND )
createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

local controlItemParams = {}
controlItemParams = tp_def_left

for i=0,3 do
	local pb_num = pb.L1-i
	
	controlItemParams.pos = pb_props[pb_num].pos_up
	
	local controlItem = { pb_num, "PRI", controlItemParams, {{"WPN_MSL_SAL_Channels_Caption",i}}, {"PRI", "ALT", "CHAN "..i+1}}
	createControlItem( controlItem, nil, nil, nil, TRANSPARENT_BACKGROUND )

	controlItemParams.pos = pb_props[pb_num].pos_down
	
	controlItem = { pb_num, "A", controlItemParams, {{"WPN_MSL_SAL_ChannelsCodes_Caption",i}}, WPN_MSL_CODE_CHANNELS}
	createControlItem( controlItem, nil, nil, nil, TRANSPARENT_BACKGROUND )
end

Add_MSL_Channels_StatusWindow()

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------