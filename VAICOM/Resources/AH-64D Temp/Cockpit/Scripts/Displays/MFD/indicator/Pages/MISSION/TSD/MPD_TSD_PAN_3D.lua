dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.L4, { { "PITCH>\n",	nil, nil}, {"-5",	tp_default_border, { {"TSD_PAN_ALT_Cntrls_Show",0},{"TSD_PAN_PITCH_Caption"},{"MFD_DataEntryButton_frame",pb.L4} }} } },

	{ pb.R6, "3D\nEYE",		tp_default_border, REPLACE_IT_WITH_PROPER_CONTROLLER},
}

-----------------------------------------------------------

local function AddL3L5ArrowsMenuItem()
	local controllers = {nil, nil}		-- TODO: Add controllers!
	local tps_wide	= createTextProperty()
	local font_size	= tps_wide.height

	local group_x	= pb_props[pb.L3].pos[1]+font_size*1.38
	local group_pos	= {group_x, (pb_props[pb.L3].pos[2] + pb_props[pb.L5].pos[2] - font_size*2.40)/2}
	
	tps_wide.stringdefs = {tps_wide.height*GetScale(),tps_wide.height*GetScale(),tps_wide.height*GetScale()*0.05}	-- to set symbols looser
	
	local ControlledBase = addPlaceholder("L3L5Menu_BasePlaseholder", {0,0}, InfoWindowsBase.name, controllers[1])
	local MenuLabelBase = addPlaceholder("L3L5Menu_plaseholder", group_pos, ControlledBase.name, nil)

	--			 	  		 name,				pos,					rot,	parent
	AddArrowMenuLabel(pb.L3, "L3L5Menu_Arr1",	{0,font_size*2.30},		nil,	MenuLabelBase.name)
	AddArrowMenuLabel(pb.L5, "L3L5Menu_Arr2",	{0,-font_size*2.20},	180,	MenuLabelBase.name)
end

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

if DBG_LABEL_SHOW then
addText( "TSD PAN PAGE 3D",  {0, 350}, tp_36_white)
end

-- 12
AddL3L5ArrowsMenuItem()

-- 13
createControls( Controls )

-- 14

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

