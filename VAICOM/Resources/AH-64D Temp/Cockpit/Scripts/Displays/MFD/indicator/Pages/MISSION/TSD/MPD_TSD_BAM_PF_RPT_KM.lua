dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "TSD BAM PAGE RPT KM",  {0, 350}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T2, "PAN",		tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.PAN}} },
	{ pb.T2, "PAN",		nil,	nil},
	{ pb.T3, "SHOW",	nil,	nil},
}

local Controls = {}
Controls = 
{
	{ pb.L1, { {"TYPE", nil, nil}, {"PF", tp_default_border, nil} } },
	{ pb.L2, { {"OPT", nil, nil}, {"AUTO", tp_default_border, {{"TSD_BAM_PFZ_DrawingMethod_Caption"}}, {"AUTO","MAN","TRP"}} } },
	{ pb.L3, { {"ACT", nil, nil}, {"NONE", tp_default_border, {{"TSD_BAM_PFZ_ACT_Caption"}}} } },
	
	{ "SIZE",
				{ 
					{ pb.L4, "1",	tp_default_border, {{"TSD_BAM_PFZ_RPT_KM_Selection",1}} }, 
					{ pb.L5, "2",	tp_default_border, {{"TSD_BAM_PFZ_RPT_KM_Selection",2}} },
					{ pb.L6, "3",	tp_default_border, {{"TSD_BAM_PFZ_RPT_KM_Selection",3}} }
				} 
	},

	{ pb.T4, "ASN",		nil,	nil},
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

