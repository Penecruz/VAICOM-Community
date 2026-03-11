dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN CODE PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{
	{ pb.T1, "CHAN",		nil },
	{ pb.T4, "CODE",		tp_default_border },
	{ pb.T5, "FREQ",		nil },
	
	{ pb.B1, "WPN",			nil },
}

local Controls = {}
Controls_Code = 
{
	--{ "LEFT",
	--			{ 
					{ pb.L1, { {"A", nil, nil}, {"1111", nil, {{"WPN_LaserCode_Frequency_Value", 0}}} } },
					{ pb.L2, { {"B", nil, nil}, {"2111", nil, {{"WPN_LaserCode_Frequency_Value", 1}}} } },
					{ pb.L3, { {"C", nil, nil}, {"4121", nil, {{"WPN_LaserCode_Frequency_Value", 2}}} } },
					{ pb.L4, { {"D", nil, nil}, {"4311", nil, {{"WPN_LaserCode_Frequency_Value", 3}}} } },
					{ pb.L5, { {"E", nil, nil}, {"4511", nil, {{"WPN_LaserCode_Frequency_Value", 4}}} } },
					{ pb.L6, { {"F", nil, nil}, {"4711", nil, {{"WPN_LaserCode_Frequency_Value", 5}}} } },
	--			} 
	--},	
	
	--{ "BOTTOM",
	--			{ 
					{ pb.B2, { {"G", nil, nil}, {"5111", nil, {{"WPN_LaserCode_Frequency_Value", 6}}} } },
					{ pb.B3, { {"H", nil, nil}, {"5311", nil, {{"WPN_LaserCode_Frequency_Value", 7}}} } },
					{ pb.B4, { {"J", nil, nil}, {"5511", nil, {{"WPN_LaserCode_Frequency_Value", 8}}} } },
					{ pb.B5, { {"K", nil, nil}, {"5711", nil, {{"WPN_LaserCode_Frequency_Value", 9}}} } },
	--			} 
	--},	
	
	--{ "RIGHT",
	--			{ 
					{ pb.R6, { {"L", nil, nil}, {"1621", nil, {{"WPN_LaserCode_Frequency_Value", 10}}} } },
					{ pb.R5, { {"M", nil, nil}, {"1666", nil, {{"WPN_LaserCode_Frequency_Value", 11}}} } },
					{ pb.R4, { {"N", nil, nil}, {"1732", nil, {{"WPN_LaserCode_Frequency_Value", 12}}} } },
					{ pb.R3, { {"P", nil, nil}, {"1777", nil, {{"WPN_LaserCode_Frequency_Value", 13}}} } },
					{ pb.R2, { {"Q", nil, nil}, {"1782", nil, {{"WPN_LaserCode_Frequency_Value", 14}}} } },
					{ pb.R1, { {"R", nil, nil}, {"1788", nil, {{"WPN_LaserCode_Frequency_Value", 15}}} } },
	--			} 
	--},	
}

local Controls = {}
Controls = 
{
	{ pb.T2, { {"SET", nil, nil}, {"LRFD", tp_default_border, {{"WPN_CODE_SET_Selection"}}, {"LRFD","LST"}} } },	
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

draw_wide_border_without_caption( pb_props[pb.L1].pos, pb_props[pb.L6].pos, tp_def_left, 	nil, nil, 					0,		tp_default.height*0.2,	TRANSPARENT_BACKGROUND)
draw_wide_border_without_caption( pb_props[pb.B2].pos, pb_props[pb.B5].pos, tp_def_bottom, 	nil, tp_default.height*1.4, nil,	nil,					TRANSPARENT_BACKGROUND)
draw_wide_border_without_caption( pb_props[pb.R1].pos, pb_props[pb.R6].pos, tp_def_right, 	nil, nil, 					0,		tp_default.height*0.2,	TRANSPARENT_BACKGROUND)

createMenu( Menu, nil, 1 ,TRANSPARENT_BACKGROUND )
createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )
createControls( Controls_Code, nil, nil, 0, TRANSPARENT_BACKGROUND )

local tps_cc = createTextProperty( nil, nil, nil, "CenterCenter" )
local Lbl_W, Lbl_H = tp_default.width*5.5, tp_default.height*1.4

AddRoundCornersWindow("WPN_CODE_Status_Lbl", {0, pb_props[pb.L1].pos[2]+tp_default.height*0.0}, Lbl_W, Lbl_H,
						{
							{"LRFD", {0, 0}, tps_cc, {{"WPN_CODE_SET_Selection"}}, {"LRFD","LST"}},
						},
						tp_default, IND_MPD_MATERIAL_GREEN, CommonWPNPlaceholder.name, nil, nil, nil, nil, TRANSPARENT_BACKGROUND)

Add_CODE_RANGES_Window()

do
	local arg = 0
	for pb_num=pb.L1,pb.L6, -1 do
		draw_large_border_for_PB( pb_num, {{"WPN_CODE_Code_Selection", arg}} )
		arg = arg + 1
	end
	
	for pb_num=pb.B2,pb.B5, -1 do
		draw_large_border_for_PB( pb_num, {{"WPN_CODE_Code_Selection", arg}} )
		arg = arg + 1
	end
	
	for pb_num=pb.R6,pb.R1, -1 do
		draw_large_border_for_PB( pb_num, {{"WPN_CODE_Code_Selection", arg}} )
		arg = arg + 1
	end
end

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------