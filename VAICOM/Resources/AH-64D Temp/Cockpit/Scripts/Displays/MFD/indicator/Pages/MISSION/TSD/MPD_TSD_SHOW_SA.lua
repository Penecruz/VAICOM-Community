dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

-- The SA page is used to control the display of situational awareness data and other JVMF related icons.
-- The options on this page are not independent between phases.
-- Selection of the SA page will clear SA degraded advisory.		-- TODO

local Menu = {}
Menu = 
{
	{ pb.T3, "SHOW",		tp_default_darkgreen_border,	nil},
	{ pb.T3, "SHOW",		nil,							nil},
	{ pb.T4, "SA",			tp_default_border,				nil},
	
	{ pb.T5, "THRT\nSHOW",	nil, 							nil},
	{ pb.T6, "COORD\nSHOW",	nil, 							nil},
}

local Controls = {}
Controls = 
{
	{ pb.L2, "FRIENDLY",	tp_default_border,	{{"TSD_SHOW_SA_Friendly_Btn_Frame"}}},		-- default ON
	{ pb.L3, "UNKNOWN",		tp_default_border,	{{"TSD_SHOW_SA_Unknown_Btn_Frame"}}},		-- default ON
	{ pb.L4, "ENEMY",		tp_default_border,	{{"TSD_SHOW_SA_Enemy_Btn_Frame"}}},			-- default ON
	{ pb.L6, "SA",			tp_default_border,	{{"TSD_SA_Btn_Frame"}}},					-- default ON

	{ pb.R1, "JVMF ICONS",	tp_default_border,	{{"TSD_SHOW_JVMF_Icons_Btn_Frame"}}},		-- default ON

	{ "ADA",
			{ 
				{ pb.R2, "ENEMY",	nil, {{"TSD_SHOW_SA_Enemy_ADA_Btn_Frame"}} }, 
				{ pb.R3, "UNKNONW",	nil, {{"TSD_SHOW_SA_Unknown_ADA_Btn_Frame"}} }
			} 
	}
}

local pos_shift_x	= 28
local t2_pocket		= pb_props[pb.T2].pos
local t3_pocket		= pb_props[pb.T3].pos
local t4_pocket		= pb_props[pb.T4].pos

pb_props[pb.T2].pos[1] = pb_props[pb.T2].pos[1] - pos_shift_x*0.7
pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - pos_shift_x*1.2
pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + pos_shift_x*0.8

-----------------------------------------------------------

local function AddSADegradedStatusWindow()	-- TODO
	local lbl_pos		= {0, pb_props[pb.L2].pos[2]+tp_default.height*1.0}
	local lbl_W, lbl_H	= tp_default.width*19.10, tp_default.height*4.6

	local str1_x		= 0
	local str2_x		= -lbl_W/2+tp_default.width*0.60

	local str1_y		= tp_default.width*3.00
	local str2_y		= tp_default.width*1.40
	local str3_y		= -tp_default.width*0.10
	local str4_y		= -tp_default.width*1.70

	--						name,					pos,		width,	height
	AddRoundCornersWindow("SADegradedStatusWindow",	lbl_pos,	lbl_W,	lbl_H,
						{	-- value
							{"SA DEGRADED",			{str1_x, str1_y}, tp_default,	nil},
							{"FRIENDLY DB FULL",	{str2_x, str2_y}, tp_def_left,	{{"TSD_SHOW_SA_Degraded_Window_Caption", 0}}},
							{"ENEMY/UNKN DB FULL",	{str2_x, str3_y}, tp_def_left,	{{"TSD_SHOW_SA_Degraded_Window_Caption", 1}}},
							{"TI DOWN",				{str2_x, str4_y}, tp_def_left,	{{"TSD_SHOW_SA_Degraded_Window_Caption", 2}}},
						},
					--	tp,			material,				parent
						tp_default,	IND_MPD_MATERIAL_GREEN,	InfoWindowsBase.name, {{"TSD_SHOW_SA_Degraded_Window_Show"}})
end

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- 10
AddSADegradedStatusWindow()

-- 11

-- 12

-- 13
createMenu( Menu )
createControls( Controls )

-- 14

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = t3_pocket
pb_props[pb.T4].pos = t4_pocket
pb_props[pb.T2].pos = t2_pocket
