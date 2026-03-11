dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MPD_WCA_Messages.lua")
local Menu = {}
Menu = 
{ 
	{ pb.T1, "DTU",		nil },
	{ pb.T2, "FAULT",	nil },
	{ pb.T3, "IBIT",	nil },
	{ pb.T5, "VERS",	nil },
	{ pb.T6, "UTIL",	nil },	
	{ pb.B1, "DMS",		nil,				{{"WCA_DmsEngPage"}}, {"DMS", "ENG"} },
	{ pb.B6, "WCA",		tp_default_border },
}

createMenu( Menu )

local Controls = {}
Controls = 
{
	{ pb.B2, "",		nil,	nil},
	{ pb.B3, "",		nil,	nil},
	{ pb.B4, "RESET",	nil,	nil},
}

createControls( Controls )


draw_line( {{-5,	355},{-5,	-370}},		tp_default.material)
draw_line( {{-15,	355},{-15,	-370}},		tp_default.material)

	
function addWCA( text, pos, text_properties, inv, controllers_inv, controllers, controllers_black,formats, margins,  name, parent )
--addText( text, pos, text_properties, controllers, formats, margins,  name, parent, h_clip_relation, level, bold, is_transparent )
	if type(inv) == "table" then
		inv[#inv + 1] = 1
		local placeholder_inv = addPlaceholder(name.."_placeholder_inv", nil, parent, {inv})
		--text_properties.inv = true
		addText( text, pos, text_properties,	controllers_inv, 		formats, margins, name.."_inv",		placeholder_inv.name, nil, nil, true )
		addText( text, pos, tp_black_left,		controllers_black,	formats, margins, name.."_black",	placeholder_inv.name, nil, nil, true, true )
		
		inv[#inv] = 0
		local placeholder = addPlaceholder(name.."_placeholder", nil, parent, {inv})
		--text_properties.inv = false
		addText( text, pos, text_properties, controllers, formats, margins,  name, placeholder.name, nil, nil, false  )
--	else
--		text_properties_default.inv = inv		
--		addText( text, pos, text_properties, controllers, formats, margins,  name, parent, nil, nil, true  )
	end
end

local function AddMessage(num)	
	local pos_y = {350, 300, 250, 200, 150, 100,  50, 0, -50, -100, -150, -200, -250, -300, -350}
	addWCA( nil,  {-370,	pos_y[num + 1]},	tp_def_left,	{"WCA_Inverse", num + 1},		{{"WCA_Messages_inv", num}},	 {{"WCA_Messages", num}},	{{"WCA_Messages_black", num}},			WCA_Messages,	nil, "WCA_"..num,			nil)
	addWCA( nil,  {20,		pos_y[num + 1]},	tp_def_left,	{"WCA_Inverse", num + 16},		{{"WCA_Messages_inv", num + 15}},{{"WCA_Messages", num + 15}},{{"WCA_Messages_black", num + 15}},	WCA_Messages,	nil, "WCA_"..(num + 15),	nil)
end
	
for i=0,14 do
AddMessage(i)
end

AddPagingGroup("WCA_B2B3Menu", {{"WCA_Pages"}}, {{"WCA_PagesIsDraw"}})

local tp_center	= tp_default
tp_center.alignment = "CenterCenter"
AddRoundCornersWindow("PageName",	{0.0,(pb_props[pb.L1].pos[2] + tp_default.height*2.0)},
						tp_default.width*15.00, tp_default.height*1.5,
						{
							{"W/C/A HISTORY",		{0.0,	0.0}, tp_center},			
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
