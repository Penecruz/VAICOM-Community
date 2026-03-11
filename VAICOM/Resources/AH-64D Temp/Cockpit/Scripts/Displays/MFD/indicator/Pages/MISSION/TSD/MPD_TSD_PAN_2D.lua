dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.L2, "W02",		nil,	{{"TSD_PAN_NextWP_Caption"}}},
	{ pb.L5, "W01",		nil,	{{"TSD_PAN_PrevWP_Caption"}}},

	{ pb.R3, "CTR",		tp_default_border,	{{"TSD_CTR_Button_frame"},{"TSD_PAN_ALT_Cntrls_Show",1}}},
	{ pb.R4, "FRZ",		tp_default_border,	{{"TSD_FRZ_Button_frame"},{"TSD_PAN_ALT_Cntrls_Show",1}}},
	
	{ pb.R5, "LAST\nPAN", nil,					{{"TSD_PAN_ALT_Cntrls_Show",1}}},
	{ pb.R6, "ALT\nCNTL", tp_default_border, 	{{"TSD_PAN_ALT_Frame"}}},
}

-----------------------------------------------------------

local function AddL3L4ArrowsMenuItem()
	local controllers = {nil, nil}		-- TODO: Add controllers!
	local font_size	= tp_default.height

	local group_x	= pb_props[pb.L3].pos[1]+font_size*1.0
	local arr1_y	= font_size*2.15
	local arr2_y	= -font_size*2.10
	local group_pos	= {group_x, (pb_props[pb.L3].pos[2] + pb_props[pb.L4].pos[2])/2 - font_size*0.8}
	
	local ControlledBase = addPlaceholder("L3L4Menu_BasePlaseholder", {0,0}, InfoWindowsBase.name, controllers[1])
	local MenuLabelBase = addPlaceholder("L3L4Menu_plaseholder", group_pos, ControlledBase.name, nil)

	--					 text,	pos,				text_properties,	controllers,	formats,	margins,	name,				parent
	local elem = addText("RTE",	{font_size*0.38,0},	tp_default,			controllers[2],	nil,		nil,		"L3L4Menu_text",	MenuLabelBase.name)		-- TODO: Add controller to change text (RTE / PITCH))
	elem.alignment = "CenterCenter"
	
	AddArrowMenuLabel(pb.L3, "L3L4Menu_Arr1",	{-font_size*0.2,arr1_y},	nil,	MenuLabelBase.name)
	AddArrowMenuLabel(pb.L4, "L3L4Menu_Arr2",	{-font_size*0.2,arr2_y},	180,	MenuLabelBase.name)
end
-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

if DBG_LABEL_SHOW then
addText( "TSD PAN PAGE 2D",  {0, 350}, tp_36_white)
end

-- 12
AddL3L4ArrowsMenuItem()

-- 13
createControls( Controls )

-- 14

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

