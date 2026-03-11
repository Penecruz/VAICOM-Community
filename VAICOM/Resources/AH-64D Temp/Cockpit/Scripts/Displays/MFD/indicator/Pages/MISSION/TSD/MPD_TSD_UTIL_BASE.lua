dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-- TODO:
-- When the INU is reset or power is applied to the INU,
-- the MPD will not display symbols that use INU information 
-- until the INU determines that its data is valid.

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu =
{
	{ pb.T1, "RPT",		nil,	nil },
	{ pb.T2, "ASE",			tp_default_border, 	{{"MFD_AsePrevBorder", 0}} },
	{ pb.T4, "ABR",		nil,	nil },
	{ pb.T5, "COORD",	nil,	nil },
	{ pb.T6, "UTIL",	tp_default_border,	nil },
	
	{ pb.B3, "BAM",		nil,	nil},
	{ pb.B5, "RTE",		nil,	nil},
	{ pb.B6, "POINT",	nil,	nil}
}

local Controls_Base =
{
	{ pb.L1, "INU1 RESET",	nil,	nil  },
	{ pb.L2, "INU2 RESET",	nil,	nil  },
	{ pb.L3, { {"PRIMARY", nil, nil}, {"INU1", tp_default_border, {{"TSD_UTIL_PRIMARY_Caption"}}, {"INU1","INU2"}} } },

	{ pb.L6, "{DOPPLER",	nil,	{{"TSD_UTIL_DopplerOnOff_Caption"}}, {"{DOPPLER", "}DOPPLER"} },

	{ pb.B1, "TSD",		nil,	nil },
	{ pb.B2, { {"NAV", nil, nil}, {"LAND", tp_default_border, {{"TSD_UTIL_NavMode_Caption"}}, {"LAND", "SEA"} } } },

	{ pb.R6, { {"DATUM>", nil, nil}, {"27", tp_default_border, { {"NAV_Datum_Caption"},{"MFD_DataEntryButton_frame",pb.R6} }} } }
}

local AlignmentControls =
{
	{ "ALIGN",	{
					{ pb.L4, "STAT", nil, {{"TSD_UTIL_NavSeaMode_Box", 0}}},
					{ pb.L5, "MOVE", nil, {{"TSD_UTIL_NavSeaMode_Box", 1}}},
				},
		nil
	},
}
local AlignFrameBase = addPlaceholder("AlignFrameBase_PH", {0, 0}, nil, {{"TSD_UTIL_ALIGN_Show"}})

local pos_shift_x = 28
local t4_pocket = pb_props[pb.T4].pos

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

do
	local tprops		= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftCenter" )
	local lbl_W, lbl_H	= tprops.width*19.5+RoundingRadius, tprops.height*3.4
	local lbl_pos		= {0,pb_props[pb.L1].pos[2]+tprops.height*0.5}
	local str_pos_x1	= -lbl_W/2 + tprops.width*0.7
	local str_pos_x2	= str_pos_x1 + tprops.width*4.85
	
	AddRoundCornersWindow("INUPositionWindow",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"POSITION CONFIDENCE",	{str_pos_x1,	tprops.height*1.0},		tprops},
							{"INU1:",				{str_pos_x1,	tprops.height*0.0},		tprops},
							{"000.000 KM",			{str_pos_x2,	tprops.height*0.0},		tprops,		{{"TSD_UTIL_INU_PosError_Caption",0}} },

							{"INU2:",				{str_pos_x1,	-tprops.height*1.0},	tprops},
							{"000.000 KM",			{str_pos_x2,	-tprops.height*1.0},	tprops,		{{"TSD_UTIL_INU_PosError_Caption",1}} },
						},
						tprops,	IND_MPD_MATERIAL_GREEN,	nil,	nil, "LeftCenter")
end

do
	local tprops		= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftCenter" )
	local lbl_W, lbl_H	= tprops.width*14+RoundingRadius, tprops.height*3.4
	local lbl_pos		= {0,pb_props[pb.L2].pos[2]+tprops.height*0.5}
	local str_pos_x1	= -lbl_W/2 + tprops.width*0.7
	local str_pos_x2	= str_pos_x1 + tprops.width*5.5

	local DopplerWindowPH = addPlaceholder("DopplerWindow_Placeholder", {0,0}, nil, {{"TSD_UTIL_DopplerWindow_Show"}})
	AddRoundCornersWindow("DopplerDataWindow",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{" DOPPLER DATA",	{str_pos_x1,	tprops.height*1.0},		tprops},
							{"INU1:",			{str_pos_x1,	tprops.height*0.0},		tprops},
							{"REJECTED",		{str_pos_x2,	tprops.height*0.0},		tprops,		{{"TSD_UTIL_INU_DopplerStatus_Caption",0}}, {"USED", "REJECTED", "MEMORY"} },
							{"INU2:",			{str_pos_x1,	-tprops.height*1.0},	tprops},
							{"REJECTED",		{str_pos_x2,	-tprops.height*1.0},	tprops,		{{"TSD_UTIL_INU_DopplerStatus_Caption",1}}, {"USED", "REJECTED", "MEMORY"} }
						},
						tprops,	IND_MPD_MATERIAL_GREEN,	DopplerWindowPH.name,	nil, "LeftCenter")
end

do
	local tprops		= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftCenter" )
	local tp_box		= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftCenter", nil, true )
	local lbl_W, lbl_H	= tprops.width*20+RoundingRadius, tprops.height*4.9
	local lbl_pos		= {0,pb_props[pb.L3].pos[2]-tprops.height*2.2}
	local str_pos_x1	= -lbl_W/2 + tprops.width*0.5
	local str_pos_x2	= str_pos_x1 + tprops.width*5.5

	AddRoundCornersWindow("SatellitesWindow",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"SATELLITES",		{-tprops.width*5,					tprops.height*1.5},		tprops },
							{"GPS1:",			{str_pos_x1,						tprops.height*0.1},		tprops },

							{"CC",				{str_pos_x2,						tprops.height*0.1},		tp_box,		{{"TSD_UTIL_GPS1_Caption",0}, {"TSD_UTIL_GPS1_Selection",0}} },
							{"CC",				{str_pos_x2 + tprops.width*3.0,		tprops.height*0.1},		tp_box,		{{"TSD_UTIL_GPS1_Caption",1}, {"TSD_UTIL_GPS1_Selection",1}} },
							{"CC",				{str_pos_x2 + tprops.width*6.0,		tprops.height*0.1},		tp_box,		{{"TSD_UTIL_GPS1_Caption",2}, {"TSD_UTIL_GPS1_Selection",2}} },
							{"CC",				{str_pos_x2 + tprops.width*9.0,		tprops.height*0.1},		tp_box,		{{"TSD_UTIL_GPS1_Caption",3}, {"TSD_UTIL_GPS1_Selection",3}} },
							{"CC",				{str_pos_x2 + tprops.width*12.0,	tprops.height*0.1},		tp_box,		{{"TSD_UTIL_GPS1_Caption",4}, {"TSD_UTIL_GPS1_Selection",4}} },

							{"GPS2:",			{str_pos_x1,						-tprops.height*1.4},	tprops },

							{"CC",				{str_pos_x2,						-tprops.height*1.4},	tp_box,		{{"TSD_UTIL_GPS2_Caption",0}, {"TSD_UTIL_GPS2_Selection",0}} },
							{"CC",				{str_pos_x2 + tprops.width*3.0,		-tprops.height*1.4},	tp_box,		{{"TSD_UTIL_GPS2_Caption",1}, {"TSD_UTIL_GPS2_Selection",1}} },
							{"CC",				{str_pos_x2 + tprops.width*6.0,		-tprops.height*1.4},	tp_box,		{{"TSD_UTIL_GPS2_Caption",2}, {"TSD_UTIL_GPS2_Selection",2}} },
							{"CC",				{str_pos_x2 + tprops.width*9.0,		-tprops.height*1.4},	tp_box,		{{"TSD_UTIL_GPS2_Caption",3}, {"TSD_UTIL_GPS2_Selection",3}} },
							{"CC",				{str_pos_x2 + tprops.width*12.0,	-tprops.height*1.4},	tp_box,		{{"TSD_UTIL_GPS2_Caption",4}, {"TSD_UTIL_GPS2_Selection",4}} },
						},
						tprops,	IND_MPD_MATERIAL_GREEN,	nil,	nil, "LeftCenter")
end

do
	local tprops		= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftCenter" )
	local lbl_W, lbl_H	= tprops.width*9.3+RoundingRadius, tprops.height*3.4
	local lbl_pos		= {0,pb_props[pb.L5].pos[2]+tprops.height*0.9}
	local str_pos_x1	= -lbl_W/2 + tprops.width*0.5
	local str_pos_x2	= str_pos_x1 + tprops.width*5.5

	AddRoundCornersWindow("KeyTypeWindow",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"KEY TYPE",	{-tprops.width*4.1,	tprops.height*1.0},		tprops},
							{"GPS1:",		{str_pos_x1,		tprops.height*0.0},		tprops},
							{"GUV",			{str_pos_x2,		tprops.height*0.0},		tprops,		{{"TSD_UTIL_GPS_KeyStatus_Caption",0}}, {"", "GUV"} },
							{"GPS2:",		{str_pos_x1,		-tprops.height*1.0},	tprops},
							{"GUV",			{str_pos_x2,		-tprops.height*1.0},	tprops,		{{"TSD_UTIL_GPS_KeyStatus_Caption",1}}, {"", "GUV"} }
						},
						tprops,	IND_MPD_MATERIAL_GREEN,	nil,	nil, "LeftCenter")
end
do
	local tprops		= createTextProperty( 28,  "GREEN",	IND_MPD_MATERIAL_GREEN, "LeftCenter" )
	local lbl_W, lbl_H	= tprops.width*17.5+RoundingRadius, tprops.height*3.8
	local lbl_pos		= {0,pb_props[pb.L6].pos[2]+tprops.height*0.0}
	local str_pos_x1	= -lbl_W/2 + tprops.width*0.7
	local str_pos_x2	= str_pos_x1 + tprops.width*5.5

	AddRoundCornersWindow("KeysLoadWindow",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"KEYS LOAD",		{-tprops.width*4.5,	tprops.height*1.2},		tprops},
							{"GPS1:",			{str_pos_x1,		tprops.height*0.0},		tprops},
							{"CORRUPT LOAD",	{str_pos_x2,		tprops.height*0.0},		tprops,		{{"TSD_UTIL_GPS_KeysLoadStatus_Caption",0}}, {"ERASE FAIL","NONE","CORRUPT LOAD","VALID","INCORRECT","VERIFIED","LOADED"} },
							{"GPS2:",			{str_pos_x1,		-tprops.height*1.2},	tprops},
							{"CORRUPT LOAD",	{str_pos_x2,		-tprops.height*1.2},	tprops,		{{"TSD_UTIL_GPS_KeysLoadStatus_Caption",1}}, {"ERASE FAIL","NONE","CORRUPT LOAD","VALID","INCORRECT","VERIFIED","LOADED"} }
						},
						tprops,	IND_MPD_MATERIAL_GREEN,	nil,	nil, "LeftCenter")
end

-- draw menus

pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + pos_shift_x*0.8

createMenu(Menu)
createControls( Controls_Base )
createControls( AlignmentControls, nil, AlignFrameBase.name, 0)

pb_props[pb.T4].pos = t4_pocket

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------
