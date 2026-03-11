dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "TSD BAM PAGE ASN",  {0, 350}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{
	{ pb.T3, "SHOW",	nil,	nil},
}

local Controls = {}
Controls = 
{
	{ pb.T4, "ASN",		tp_default_border,	nil},
	
	{ pb.T2, "PF1",		tp_default_border,	{{"TSD_BAM_PFZ_Btn_Selection",0}}},
	{ pb.T1, "PF2",		tp_default_border,	{{"TSD_BAM_PFZ_Btn_Selection",1}}},

	{ pb.L1, "PF3", 	tp_default_border,	{{"TSD_BAM_PFZ_Btn_Selection",2}}},
	{ pb.L2, "PF4", 	tp_default_border,	{{"TSD_BAM_PFZ_Btn_Selection",3}}},
	{ pb.L3, "PF5", 	tp_default_border,	{{"TSD_BAM_PFZ_Btn_Selection",4}}},
	{ pb.L4, "PF6", 	tp_default_border,	{{"TSD_BAM_PFZ_Btn_Selection",5}}},
	{ pb.L5, "PF7", 	tp_default_border,	{{"TSD_BAM_PFZ_Btn_Selection",6}}},
	{ pb.L6, "PF8", 	tp_default_border,	{{"TSD_BAM_PFZ_Btn_Selection",7}}},

	{ pb.B6, "OWN", 	tp_default_border,	{{"TSD_BAM_Subscriber_Selection",7}}},
}

pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + T3T4_posShiftX*1.0
pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - T3T4_posShiftX*1.2

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- draw menus
createMenu( Menu )
createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = T3_pocket
pb_props[pb.T4].pos = T4_pocket

