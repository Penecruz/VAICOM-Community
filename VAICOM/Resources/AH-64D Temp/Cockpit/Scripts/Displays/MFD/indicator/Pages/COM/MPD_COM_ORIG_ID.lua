dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/COM/MPD_COM_Symbology_def.lua")

local Menu = {}
Menu = 
{ 
	{ pb.B1, "COM",		nil },	
	{ pb.T2, "DL",		nil },
	{ pb.B4, {{"ORIG",nil },{ "ID",	nil }}}, 
	{ "MSG",
		{
			{ pb.B5, "REC", nil },
			{ pb.B6, "SEND", tp_default_border, {{"MPD_COM_BTN_BORDER", pb.B6}} },
		}
	},
	{ pb.L6, {{"ORIG",	nil },{"DIR", nil }}}
}
createMenu( Menu )

addBorder( pb.B4, 100, nil, 15 )

local pos_up_r	= pb_props[pb.R2].pos	
local pos_dn_r = {pb_props[pb.R4].pos[1], pb_props[pb.R4].pos[2]-pb_props[pb.R2].tp.height*0.9}
draw_border_with_caption( pos_up_r, pos_dn_r, 8, 1, "TACFIRE", pb_props[pb.R1].tp, nil, false )

local pos_up_l	= pb_props[pb.L2].pos	
local pos_dn_l = {pb_props[pb.L4].pos[1], pb_props[pb.L4].pos[2]-pb_props[pb.L2].tp.height*0.9}
draw_border_with_caption( pos_up_l, pos_dn_l, 9, 1, "DIGITAL ID", pb_props[pb.L1].tp, nil, false )

Controls = 
{	
	{ pb.L1, { 
			{"CALL SIGN>", nil }, 
			{"",  tp_default_border, {{"MFD_COM_ORIG_Buttons", pb.L1}}} 
		}},
			
	{ pb.L2,  { {"DATALINK/",   nil, nil }, {"TACFIRE", nil, nil }} },
	{ pb.L3,  "INTERNET",   tp_default_border, {{"MFD_COM_ORIG_Buttons", pb.L3}} },
	{ pb.L4,  { {"FIRE",   nil, nil }, {"SUPPORT", nil, nil }} },					

	{ pb.R1, { 
			{"DATALINK ORIG ID>", nil }, 
			{"",  tp_default_border, {{"MFD_COM_ORIG_Buttons", pb.R1}}} 
		}},
		
	{ pb.R2, { 
			{"ORIG ID>", nil }, 
			{"",  tp_default_border, {{"MFD_COM_ORIG_Buttons", pb.R2}}} 
		}},
		
	{ pb.R3, { 
			{"TEAM ID>", nil }, 
			{"",  tp_default_border, {{"MFD_COM_ORIG_Buttons", pb.R3}}} 
		}},
		
	{ pb.R4, { 
			{"BCST ID>", nil }, 
			{"",  tp_default_border, {{"MFD_COM_ORIG_Buttons", pb.R4}}} 
		}}
			
}
createControls( Controls )

addBorder( pb.L2, 195, {{"MFD_COM_ORIG_Buttons", pb.L2}} )
addBorder( pb.L4, 195, {{"MFD_COM_ORIG_Buttons", pb.L4}} )

addOwnShip_StatusWindow()