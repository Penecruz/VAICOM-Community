dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN RKT PEN PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.B5, "RKT", tp_default_border, nil},

	-- { "",
			-- { 				
				{ pb.R1, "SPQ",	tp_default_border, {{"WPN_RKT_PEN_Selection", 0}}},
				{ pb.R2, "10",	tp_default_border, {{"WPN_RKT_PEN_Selection", 1}}},
				{ pb.R3, "15",	tp_default_border, {{"WPN_RKT_PEN_Selection", 2}}},
				{ pb.R4, "20",	tp_default_border, {{"WPN_RKT_PEN_Selection", 3}}},
				{ pb.R5, "25",	tp_default_border, {{"WPN_RKT_PEN_Selection", 4}}},
			-- } 
	-- },	
	
	-- { "",
			-- { 
				{ pb.L1, "BNK",	tp_default_border, {{"WPN_RKT_PEN_Selection", 5}}},
				{ pb.L2, "30",	tp_default_border, {{"WPN_RKT_PEN_Selection", 6}}},
				{ pb.L3, "35",	tp_default_border, {{"WPN_RKT_PEN_Selection", 7}}},
				{ pb.L4, "40",	tp_default_border, {{"WPN_RKT_PEN_Selection", 8}}},
				{ pb.L5, "45",	tp_default_border, {{"WPN_RKT_PEN_Selection", 9}}},
			-- } 
	-- },	
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

draw_wide_border_without_caption( pb_props[pb.L1].pos, pb_props[pb.L5].pos, tp_def_left, 3, nil, nil, nil, TRANSPARENT_BACKGROUND)
draw_wide_border_without_caption( pb_props[pb.R1].pos, pb_props[pb.R5].pos, tp_def_right, 3, nil, nil, nil, TRANSPARENT_BACKGROUND)

createControls( Controls, nil, nil, 0, TRANSPARENT_BACKGROUND )

AddRoundCornersWindow("WPN_RKT_PEN_Lbl", {0, pb_props[pb.L1].pos[2]+tp_default.height*0.0}, nil, nil, "PENETRATION",
						nil, nil, nil, nil, nil, nil, nil, TRANSPARENT_BACKGROUND)

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------