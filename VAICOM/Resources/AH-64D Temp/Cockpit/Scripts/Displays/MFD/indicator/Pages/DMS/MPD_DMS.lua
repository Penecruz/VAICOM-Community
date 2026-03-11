dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MPD_WCA_Messages.lua")

--addText( "DMS PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T1, "DTU",			nil },
	{ pb.T2, "FAULT",		nil },
	{ pb.T3, "IBIT",		nil },
	{ pb.T4, {{"SHUT",	nil },{"DOWN",	nil }}},
	{ pb.T5, "VERS",		nil },
	{ pb.T6, "UTIL",		nil },
	{ pb.B1, "DMS",			tp_default_border },
	{ pb.B6, "WCA",			nil },
}

createMenu( Menu )

AddPagingGroup("DMS_B2B3Menu", {{"DMS_Pages"}}, {{"DMS_PagesIsDraw"}})

draw_line( {{-5,	355},{-5,	-370}},		tp_default.material)
draw_line( {{-15,	355},{-15,	-370}},		tp_default.material)

local tp_center	= tp_default
tp_center.alignment = "CenterCenter"	

AddRoundCornersWindow("PageName_1",	{pb_props[pb.T2].pos[1],(pb_props[pb.L1].pos[2] + tp_default.height*2.0)},
						tp_default.width*20.00, tp_default.height*1.5,
						{
							{"ADVISORIES",		{0.0,	0.0}, tp_center},			
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)
						
AddRoundCornersWindow("PageName_2",	{pb_props[pb.T5].pos[1],(pb_props[pb.L1].pos[2] + tp_default.height*2.0)},
						tp_default.width*20.00, tp_default.height*1.5,
						{
							{"FAULTS",		{0.0,	0.0}, tp_center},			
						},
						tp_default,	IND_MPD_MATERIAL_GREEN,	nil,	nil)


local function AddMessage(num)	
	local pos_y = {350, 300, 250, 200, 150, 100,  50, 0, -50, -100, -150, -200, -250, -300}	
	addText( nil,  {-380,	pos_y[num + 1]},	tp_def_left,	{{"DMS_Advisory_Messages", num}},	WCA_Messages,		nil, "Advisory_"..num,	nil)
	addText( nil,  {20,		pos_y[num + 1]},	tp_def_left,	{{"DMS_Fault_Messages", num}},		DMS_Fault_Messages,	nil, "Fault_"..num,		nil)
end	
	
for i=0,13 do
AddMessage(i)
end

