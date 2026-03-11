dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "TSD BAM PAGE PF",  {0, 350}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------
addTwoLineMenuBarrierLR( pb.L5 )-- for special case: three labels appointed to PB and one of then is one line.

local Menu = {}
Menu = 
{ 
	{ pb.T2, "PAN",		tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.PAN}}},
	{ pb.T2, "PAN",		nil,	nil},
	{ pb.T3, "SHOW",	nil,	nil},
}

local DeleteControls = {}
DeleteControls = 
{
	{ pb.L4, "NO",	nil, nil},
	{ pb.L5, "YES",	nil, nil},
}

local NormalControls = {}
NormalControls = 
{
	{ pb.L4, "DEL", nil, nil},

	{ pb.L5, { {"#Z", nil, {{"TSD_BAM_ZN_Show",0}}}, {"8", tp_default_border, {{"TSD_BAM_ZN_Show",0},{"TSD_BAM_ZonesCount"}}} } },
	{ pb.L5, { {"KM", nil, {{"TSD_BAM_ZN_Show",1}}}, {"2", tp_default_border, {{"TSD_BAM_ZN_Show",1},{"TSD_BAM_ZonesSize"}}} } },
}

local Controls = {}
Controls = 
{
	{ pb.T4, "ASN",		nil,	nil},
	
	{ pb.L1, { {"TYPE", nil, nil}, {"PF", tp_default_border, nil} } },
	{ pb.L2, { {"OPT", nil, nil}, {"AUTO", tp_default_border, {{"TSD_BAM_PFZ_DrawingMethod_Caption"}}, {"AUTO","MAN","TRP"}} } },
	{ pb.L3, { {"ACT", nil, nil}, {"NONE", tp_default_border, {{"TSD_BAM_PFZ_ACT_Caption"}}} } },

	{ pb.L6, { {"DR", nil, nil}, {"BX", tp_default_border, {{"TSD_BAM_DrawingMode_Caption"}}, {"BX","LN"}} } },
}

pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + T3T4_posShiftX*1.0
pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - T3T4_posShiftX*1.2

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

local DeleteFrameBase 		= addPlaceholder("DeleteFrameBase_PH", 		{0, 0}, nil, {{"TSD_BAM_DEL_Menu_Show",1}})
local NormalControlsBase 	= addPlaceholder("NormalControlsBase_PH", 	{0, 0}, nil, {{"TSD_BAM_DEL_Menu_Show",0}})

draw_border_with_caption( pb_props[pb.L4].pos, pb_props[pb.L5].pos,  4, 1, "DELETE", pb_props[pb.L5].tp, DeleteFrameBase.name )

-- draw menus

createMenu( Menu )
createControls( Controls )
createControls( DeleteControls,  nil, DeleteFrameBase.name, 0)
createControls( NormalControls,  nil, NormalControlsBase.name)

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = T3_pocket
pb_props[pb.T4].pos = T4_pocket

