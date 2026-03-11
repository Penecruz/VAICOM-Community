dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "TSD RPT PAGE - NON BDA",  {0, 350}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{
	{ pb.T2, "PAN",		nil,	nil},
	{ pb.T3, "SHOW",	nil,	nil},
}

local Controls = {}
Controls = 
{
	{ pb.T4, { {"STAT", nil, nil}, {"NONE", tp_default_border, {{"TSD_RPT_STAT_Btn_Caption"}}, {"NONE","MTO","AFM","SPOT","SIT"}} } },

	{ pb.L1, "BDA",		nil,	nil},
	{ pb.L2, "TGT",		nil,	nil},
	{ pb.L3, "PP",		nil,	nil},
	{ pb.L4, "FARM",	nil,	nil},
	{ pb.L5, "SIT",		nil,	nil},
	{ pb.L6, "SPOT",	nil,	nil},
	
	{ pb.R3, "CTR",		tp_default_border,	{{"TSD_CTR_Button_frame"}}},
	{ pb.R4, "FRZ",		tp_default_border,	{{"TSD_FRZ_Button_frame"}}}
}

local pos_shift_x = 28
local t3_pocket = pb_props[pb.T3].pos
local t4_pocket_up, t4_pocket_dn = pb_props[pb.T4].pos_up, pb_props[pb.T4].pos_down

pb_props[pb.T3].pos[1]		= pb_props[pb.T3].pos[1] - pos_shift_x*1.2
pb_props[pb.T4].pos_up[1]	= pb_props[pb.T4].pos[1] + pos_shift_x*1.4
pb_props[pb.T4].pos_down[1]	= pb_props[pb.T4].pos[1] + pos_shift_x*1.4

-- TODO: Wrong alignment for multilevel control caption. It must be higher on screen, like:
pb_props[pb.T4].pos_up[2]	= pb_props[pb.T4].pos_up[2] + tp_def_top.height*0.5
pb_props[pb.T4].pos_down[2]	= pb_props[pb.T4].pos_down[2] + tp_def_top.height*0.5

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

AddCurrentHeadingLabel()
AddR1R2_MapRange_Arrows()

createMenu( Menu )
createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos			= t3_pocket
pb_props[pb.T4].pos_up		= t4_pocket_up
pb_props[pb.T4].pos_down	= t4_pocket_dn
