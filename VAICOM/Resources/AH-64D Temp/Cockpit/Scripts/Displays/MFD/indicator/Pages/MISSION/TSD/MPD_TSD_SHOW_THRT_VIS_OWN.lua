dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

-- The this page is used to control the display of ownship relative rings and intervisibility shading.

local Menu = {}
Menu = 
{
	{ pb.T3, "SHOW",		tp_default_darkgreen_border,	nil},
	{ pb.T3, "SHOW",		nil,							nil},
	{ pb.T4, "SA",			nil,							nil},		-- no SA, at least in early access
	
	{ pb.T5, "THRT\nSHOW",	tp_default_border,				nil},
	{ pb.T6, "COORD\nSHOW",	nil,							nil},
}

local Controls = {}
Controls = 
{
	{ pb.L2, "ASE THREATS",	tp_default_border,	{{"TSD_SHOW_ASE_Threats_Btn_Frame"}} },		-- default ON
	{ pb.L4, { {"ALT>", nil, {{REPLACE_IT_WITH_PROPER_CONTROLLER}}}, {"?", tp_default_border,  { {REPLACE_IT_WITH_PROPER_CONTROLLER},{REPLACE_IT_WITH_PROPER_CONTROLLER},{"MFD_DataEntryButton_frame",pb.L4} }} } },
	{ pb.L6, "VIS\nSHADE",	tp_default_border,	{{"TSD_SHOW_VIS_Shade_Btn_Frame"}}},		-- default ON

	{ pb.R1, { {"VIS", nil, nil}, {"OWN", tp_default_border, nil} } },

	{ "VIS",
			{ 
				{ pb.R2, "OWN",		nil, {{"TSD_SHOW_THRT_OWN_Own_Btn_Frame"}} }, 		-- default ON
				{ pb.R3, "TRN PT",	nil, {{"TSD_SHOW_THRT_OWN_TRN_PT_Btn_Frame"}} },	-- default OFF
				{ pb.R4, "GHOST",	nil, {{"TSD_SHOW_THRT_OWN_Ghost_Btn_Frame"}} } 		-- default ON
			} 
	},
	
	{ pb.R6, "RINGS",	tp_default_border,	{{"TSD_SHOW_Rings_Btn_Frame"}}},			-- default OFF
}

local pos_shift_x	= 28
local t2_pocket		= pb_props[pb.T2].pos
local t3_pocket		= pb_props[pb.T3].pos
local t4_pocket		= pb_props[pb.T4].pos

pb_props[pb.T2].pos[1] = pb_props[pb.T2].pos[1] - pos_shift_x*0.7
pb_props[pb.T3].pos[1] = pb_props[pb.T3].pos[1] - pos_shift_x*1.2
pb_props[pb.T4].pos[1] = pb_props[pb.T4].pos[1] + pos_shift_x*0.8

-----------------------------------------------------------

local function AddL3L5ArrowsMenuItem()
	local tps_wide	= createTextProperty()
	local font_size	= tps_wide.height

	local group_x	= pb_props[pb.L3].pos[1]+font_size*1.38
	local group_pos	= {group_x, (pb_props[pb.L3].pos[2] + pb_props[pb.L5].pos[2] - font_size*2.40)/2}
	
	tps_wide.stringdefs = {tps_wide.height*GetScale(),tps_wide.height*GetScale(),tps_wide.height*GetScale()*0.05}	-- to set symbols looser
	
	local ControlledBase = addPlaceholder("L3L5Menu_BasePlaseholder", {0,0}, InfoWindowsBase.name, {{REPLACE_IT_WITH_PROPER_CONTROLLER}})
	local MenuLabelBase = addPlaceholder("L3L5Menu_plaseholder", group_pos, ControlledBase.name, nil)

	--			 	  pb_num,	name,				pos,								rot,	parent
	AddArrowMenuLabel(pb.L3,	"L3L5Menu_Arr1",	{-font_size*0.6,font_size*4.70},	nil,	MenuLabelBase.name)
	AddArrowMenuLabel(pb.L5,	"L3L5Menu_Arr2",	{-font_size*0.6,-font_size*3.85},	180,	MenuLabelBase.name)
end

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

-- 10

-- 11

-- 12
AddL3L5ArrowsMenuItem()

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
