dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "TSD BAM PAGE OPT",  {0, 350}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T2, "PAN",		tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.PAN}}},
	{ pb.T2, "PAN",		nil,	nil},
	{ pb.T3, "SHOW",	nil,	nil},
}

local Controls = {}
Controls = 
{
	{ "OPTION",
				{ 
					{ pb.L1, "AUTO",	tp_default_border, {{"TSD_BAM_PFZ_DrawingMethod_Selection",0}} }, 
					{ pb.L2, "MAN",		tp_default_border, {{"TSD_BAM_PFZ_DrawingMethod_Selection",1}} },
					{ pb.L3, "TRP",		tp_default_border, {{"TSD_BAM_PFZ_DrawingMethod_Selection",2}} }
				} 
	},

	{ pb.L4, "DEL", nil, nil},
	
	{ pb.L5, { {"#Z", nil, {{"TSD_BAM_ZN_Show",0}}}, {"8", tp_default_border, {{"TSD_BAM_ZN_Show",0},{"TSD_BAM_ZonesCount"}}} } },
	{ pb.L5, { {"KM", nil, {{"TSD_BAM_ZN_Show",1}}}, {"2", tp_default_border, {{"TSD_BAM_ZN_Show",1},{"TSD_BAM_ZonesSize"}}} } },
	
	{ pb.L6, { {"DR", nil, nil}, {"BX", tp_default_border, {{"TSD_BAM_DrawingMode_Caption"}}, {"BX","LN"}} } },
	
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

