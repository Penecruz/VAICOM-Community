dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "TSD BAM PAGE NF",  {0, 350}, tp_36_white)
end
-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T2, "PAN",		tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.PAN}} },
	{ pb.T2, "PAN",		nil,	nil },
	{ pb.T3, "SHOW",	nil,	nil },
}

local DeleteControls = {}
DeleteControls = 
{
	{ pb.L4, "NO",	nil, nil},
	{ pb.L5, "YES",	nil, nil},
}

local Controls = {}
Controls = 
{
	{ pb.T1, { {"ACTIVE", nil, nil}, {"MULTI", tp_default_border, {{"TSD_BAM_NFZ_ACTIVE_Caption"}}, {"MULTI","SINGLE"}} } },

	{ pb.L1, { {"TYPE", nil, nil}, {"NF", tp_default_border, nil} } },
	{ pb.L2, { {"SEL", nil, {{"TSD_BAM_NFZ_SEL_Color"}}}, {"NF1", tp_default_border, {{"TSD_BAM_NFZ_SEL_Color"},{"TSD_BAM_NFZ_SEL_Caption"}}} } },
	{ pb.L3, "ACT", tp_default_border,	{{"TSD_BAM_NFZ_ACT_Border"}}},
	
	{ pb.L4, "DEL", 	nil,		{{"TSD_BAM_DEL_Menu_Show",0},{"TSD_BAM_NFZ_L4_Caption"}},	{"DEL", "ACCEPT"}},
	{ pb.L4, "ACCEPT", 	nil,		{{"TSD_BAM_DEL_Menu_Show",0},{"TSD_BAM_NFZ_L4_Caption"}}, 	{"DEL", "ACCEPT"}},
	
	{ pb.L6, { {"DR", nil, nil}, {"BX", tp_default_border, {{"TSD_BAM_DrawingMode_Caption"}}, {"BX","LN"}} } },

}

pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - T3T4_posShiftX*1.2

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

local DeleteFrameBase = addPlaceholder("DeleteFrameBase_PH", {0, 0}, nil, {{"TSD_BAM_DEL_Menu_Show",1}})
draw_border_with_caption( pb_props[pb.L4].pos, pb_props[pb.L5].pos,  4, 1, "DELETE", pb_props[pb.L5].tp, DeleteFrameBase.name )
createControls( DeleteControls,  nil, DeleteFrameBase.name, 0)

-- draw menus

createMenu( Menu )
createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = T3_pocket