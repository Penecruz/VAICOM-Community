dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN MSL SAL BASE PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.L1, { {"PRI", nil, nil}, {"A", tp_default_border, {{"WPN_MSL_SAL_PRI_ChannelCode_Selection"}}, WPN_MSL_CODE_CHANNELS} } },
	{ pb.L2, { {"ALT", nil, nil}, {"B", tp_default_border, {{"WPN_MSL_SAL_ALT_ChannelCode_Selection"}}, WPN_MSL_CODE_CHANNELS} } },

	{ pb.L6, "{MSL CCM",	nil,	{{"WPN_MSL_SAL_CCM_Selected"}}, {"{MSL CCM","}MSL CCM"}},
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

local txt_pos = {pb_props[pb.L1].pos_down[1]+tp_def_left.width*2.0, pb_props[pb.L1].pos_down[2] + 5}
addText("PFR", txt_pos, tp_def_left, {{"WPN_MSL_SAL_PRI_ChannelCode_Type"}}, {"PIM","PRF",""}, nil, "MSL_PRI_CHAN_CodeType", nil, nil, nil, nil, TRANSPARENT_BACKGROUND)

txt_pos = {pb_props[pb.L2].pos_down[1]+tp_def_left.width*2.0, pb_props[pb.L2].pos_down[2] + 5}
addText("PIM", txt_pos, tp_def_left, {{"WPN_MSL_SAL_ALT_ChannelCode_Type"}}, {"PIM","PRF",""}, nil, "MSL_ALT_CHAN_CodeType", nil, nil, nil, nil, TRANSPARENT_BACKGROUND)

Add_MSL_Channels_StatusWindow()

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------