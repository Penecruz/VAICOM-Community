dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

--addText( "TSD PAN PAGE BASE",  {0, 0})

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{ 
	{ pb.T1, "RPT",		nil,	nil},
	{ pb.T2, "PAN",		tp_default_border,	nil},
	{ pb.T3, "SHOW",	nil,	nil},

	{ pb.B3, "BAM",		tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.BAM}}},
	{ pb.B3, "BAM",		nil,	nil},

	{ pb.B4, "MAP",		tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.MAP}}},
	{ pb.B4, "MAP",		nil,	nil},

	{ pb.B5, "RTE",		tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.RTE}}},
	{ pb.B5, "RTE",		nil,	nil},

	{ pb.B6, "POINT",	tp_default_darkgreen_border,	{{"TSD_OriginSubpageBtn_FrameShow", TSD_SUBPAGE_ORIGIN_FMT.POINT}}},
	{ pb.B6, "POINT",	nil,	nil},
}

local Controls_Base = {}
Controls_Base = 
{
	{ pb.T5, { {"HDG>", nil, nil}, {"360", tp_default_border, { {"TSD_PAN_HDG_Caption"},{"MFD_DataEntryButton_frame",pb.T5} }} } },

	{ pb.L1, { {"POINT>\n",		nil,	nil}, {"?", tp_default_border, { {"TSD_PAN_Point_Caption"},{"MFD_DataEntryButton_frame",pb.L1} }} } },
	{ pb.L6, { {"PAN", nil, nil}, {"CURSR", tp_default_border, {{"TSD_PAN_MODE_Caption"}}, {"CURSR","NORM"}} } },

	{ pb.B1, "TSD",		nil,	nil},
	{ pb.B2, { {"PHASE", nil, nil}, {"NAV", tp_default_border, {{"TSD_PHASE_Btn_Caption"}}, {"NAV", "ATK"}} } },
	
	{ pb.R4, { {"ALT>\n",	nil,	{{"TSD_PAN_ALT_Cntrls_Show",0}}}, {"10", tp_default_border, { {"TSD_PAN_ALT_Cntrls_Show",0},{"TSD_PAN_ALT_Caption"},{"MFD_DataEntryButton_frame",pb.R4} }} } },
}

local pos_shift_x = 28
local t3_pocket,t4_pocket,b3_pocket,b4_pocket = pb_props[pb.T3].pos,pb_props[pb.T4].pos,pb_props[pb.B3].pos,pb_props[pb.B4].pos

pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - pos_shift_x*1.2
pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + pos_shift_x*0.8
pb_props[pb.B3].pos[1] = pb_props[pb.B3].pos[1] - pos_shift_x
pb_props[pb.B4].pos[1] = pb_props[pb.B4].pos[1] + pos_shift_x

-----------------------------------------------------------
local function AddR3R5ArrowsMenuItem()
	local group_pos	= {pb_props[pb.R3].pos[1], (pb_props[pb.R3].pos[2] + pb_props[pb.R5].pos[2])/2}
	local font_size = tp_default.height

	local MenuLabelBase = addPlaceholder("TSD_R3R5Menu_plaseholder", group_pos, InfoWindowsBase.name, {{"TSD_PAN_ALT_Cntrls_Show",0}})

	--			 	  		 name,				pos,								rot,	parent
	AddArrowMenuLabel(pb.R3, "R3R5Menu_Arr1",	{-font_size*0.5,font_size*4.30},	nil,	MenuLabelBase.name)
	AddArrowMenuLabel(pb.R5, "R3R5Menu_Arr2",	{-font_size*0.5,-font_size*4.30},	180,	MenuLabelBase.name)
end

local function AddT4T6ArrowsMenuItem()
	local controllers = {nil, nil}		-- TODO: Add controllers!
	local font_size	= tp_default.height

	local group_pos	= {pb_props[pb.T5].pos[1], pb_props[pb.T5].pos[2]-font_size*0.40}

	local ControlledBase = addPlaceholder("T4T6Menu_BasePlaseholder", {0,0}, InfoWindowsBase.name, controllers[1])
	local MenuLabelBase = addPlaceholder("T4T6Menu_plaseholder", group_pos, ControlledBase.name, nil)

	--			 	  		 name,				pos,				rot,	parent
	AddArrowMenuLabel(pb.T4, "T4T6Menu_Arr1",	{-font_size*4.0,0},	90,		MenuLabelBase.name)	-- TODO: need to set position
	AddArrowMenuLabel(pb.T6, "T4T6Menu_Arr2",	{font_size*4.9,0},	-90,	MenuLabelBase.name)
end

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- 8
-- ********** Ownship Sensor Layer **********
AddCompassRose()

-- 9
-- ********** Ownship Layer **********
AddFrozenOwnship()
AddOwnshipSymbol()

-- 10
-- ********** Info Windows and Menus **********
AddMapFrozenCue()
AddTSDInfoStatusWindows()

if DBG_SA_WINDOWS_SHOW then
AddSAStatusWindowSmall()
AddSAStatusWindowLarge()
end

AddR1R2_MapRange_Arrows()
AddR3R5ArrowsMenuItem()
AddT4T6ArrowsMenuItem()

createMenu( Menu )
createControls( Controls_Base )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

pb_props[pb.T3].pos = t3_pocket
pb_props[pb.T4].pos = t4_pocket
pb_props[pb.B3].pos = b3_pocket
pb_props[pb.B4].pos = b4_pocket

