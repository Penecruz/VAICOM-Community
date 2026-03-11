dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN CHAN PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{
	{ pb.T1, "CHAN",		tp_default_border },
	{ pb.T6, "UTIL",		nil },
	
	{ pb.B1, "WPN",			nil },
}

local Controls = {}
Controls = 
{
	{ "CHANNEL",
				{ 
					{ pb.T2, "1",	tp_default_border, {{"WPN_CHAN_Channel_Selection", 0}} },
					{ pb.T3, "2",	tp_default_border, {{"WPN_CHAN_Channel_Selection", 1}} },
					{ pb.T4, "3",	tp_default_border, {{"WPN_CHAN_Channel_Selection", 2}} },
					{ pb.T5, "4",	tp_default_border, {{"WPN_CHAN_Channel_Selection", 3}} },
				} 
	},		
	
	--{ "LEFT",
	--			{ 
					{ pb.L1, { {"A", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 0}}}, {"1111", nil, {{"WPN_LaserCode_Frequency_Value", 0}}} } },
					{ pb.L2, { {"B", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 1}}}, {"2111", nil, {{"WPN_LaserCode_Frequency_Value", 1}}} } },
					{ pb.L3, { {"C", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 2}}}, {"4121", nil, {{"WPN_LaserCode_Frequency_Value", 2}}} } },
					{ pb.L4, { {"D", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 3}}}, {"4311", nil, {{"WPN_LaserCode_Frequency_Value", 3}}} } },
					{ pb.L5, { {"E", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 4}}}, {"4511", nil, {{"WPN_LaserCode_Frequency_Value", 4}}} } },
					{ pb.L6, { {"F", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 5}}}, {"4711", nil, {{"WPN_LaserCode_Frequency_Value", 5}}} } },
	--			} 
	--},	
	
	--{ "BOTTOM",
	--			{ 
					{ pb.B2, { {"G", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 6}}}, {"5111", nil, {{"WPN_LaserCode_Frequency_Value", 6}}} } },
					{ pb.B3, { {"H", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 7}}}, {"5311", nil, {{"WPN_LaserCode_Frequency_Value", 7}}} } },
					{ pb.B4, { {"J", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 8}}}, {"5511", nil, {{"WPN_LaserCode_Frequency_Value", 8}}} } },
					{ pb.B5, { {"K", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 9}}}, {"5711", nil, {{"WPN_LaserCode_Frequency_Value", 9}}} } },
	--			} 
	--},	
	
	--{ "RIGHT",
	--			{ 
					{ pb.R6, { {"L", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 10}}}, {"1621", nil, {{"WPN_LaserCode_Frequency_Value", 10}}} } },
					{ pb.R5, { {"M", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 11}}}, {"1666", nil, {{"WPN_LaserCode_Frequency_Value", 11}}} } },
					{ pb.R4, { {"N", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 12}}}, {"1732", nil, {{"WPN_LaserCode_Frequency_Value", 12}}} } },
					{ pb.R3, { {"P", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 13}}}, {"1777", nil, {{"WPN_LaserCode_Frequency_Value", 13}}} } },
					{ pb.R2, { {"Q", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 14}}}, {"1782", nil, {{"WPN_LaserCode_Frequency_Value", 14}}} } },
					{ pb.R1, { {"R", tp_default_border, {{"WPN_CHAN_ChannelCode_Selection", 15}}}, {"1788", nil, {{"WPN_LaserCode_Frequency_Value", 15}}} } },
	--			} 
	--},	
}



-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

draw_wide_border_without_caption( pb_props[pb.L1].pos, pb_props[pb.L6].pos, tp_def_left,	nil, nil,					0, 		tp_default.height*0.2,	TRANSPARENT_BACKGROUND)
draw_wide_border_without_caption( pb_props[pb.B2].pos, pb_props[pb.B5].pos, tp_def_bottom,	nil, tp_default.height*1.4,	nil,	nil,					TRANSPARENT_BACKGROUND)
draw_wide_border_without_caption( pb_props[pb.R1].pos, pb_props[pb.R6].pos, tp_def_right,	nil, nil, 					0, 		tp_default.height*0.2,	TRANSPARENT_BACKGROUND)

createMenu( Menu, nil, 1, TRANSPARENT_BACKGROUND )
createControls( Controls, nil, nil, 0, TRANSPARENT_BACKGROUND )

local tps_cc = createTextProperty( nil, nil, nil, "CenterCenter" )
local tps_rc = createTextProperty( nil, nil, nil, "RightCenter" )

local Lbl_W, Lbl_H = tp_default.width*7.5, tp_default.height*1.4

AddRoundCornersWindow("WPN_CHAN_Number_Lbl", {0, pb_props[pb.L1].pos[2]+tp_default.height*0.0}, Lbl_W, Lbl_H,
						{
							{"CHAN",	{-tp_default.width*1.0,				0},	tps_cc,	nil},
							{"1",		{Lbl_W/2 - tp_default.width*0.9,	0},	tps_rc,	{{"WPN_CHAN_Channel_Lbl_Caption"}}},
						},
						tp_default, IND_MPD_MATERIAL_GREEN, CommonWPNPlaceholder.name, nil, nil, nil, nil, TRANSPARENT_BACKGROUND)

Add_CODE_RANGES_Window()

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------