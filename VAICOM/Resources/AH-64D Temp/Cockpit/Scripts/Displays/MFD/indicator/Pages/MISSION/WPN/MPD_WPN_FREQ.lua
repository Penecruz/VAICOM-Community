dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN FREQ PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Menu = {}
Menu = 
{
	{ pb.T1, "CHAN",		nil },
	{ pb.T4, "CODE",		tp_default_darkgreen_border },
	{ pb.T5, "FREQ",		tp_default_border },
	
	{ pb.B1, "WPN",			nil },
}

local Controls = {}
Controls = 
{
	--{ "LEFT",
	--			{ 
					{ pb.L1, { {"A >", nil, nil}, {"1111", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 0},{"MFD_DataEntryButton_frame",pb.L1}}} } },
					{ pb.L2, { {"B >", nil, nil}, {"2111", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 1},{"MFD_DataEntryButton_frame",pb.L2}}} } },
					{ pb.L3, { {"C >", nil, nil}, {"4121", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 2},{"MFD_DataEntryButton_frame",pb.L3}}} } },
					{ pb.L4, { {"D >", nil, nil}, {"4311", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 3},{"MFD_DataEntryButton_frame",pb.L4}}} } },
					{ pb.L5, { {"E >", nil, nil}, {"4511", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 4},{"MFD_DataEntryButton_frame",pb.L5}}} } },
					{ pb.L6, { {"F >", nil, nil}, {"4711", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 5},{"MFD_DataEntryButton_frame",pb.L6}}} } },
	--			} 
	--},	
	
	--{ "BOTTOM",
	--			{ 
					{ pb.B2, { {"G >", nil, nil}, {"5111", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 6},{"MFD_DataEntryButton_frame",pb.B2}}} } },
					{ pb.B3, { {"H >", nil, nil}, {"5311", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 7},{"MFD_DataEntryButton_frame",pb.B3}}} } },
					{ pb.B4, { {"J >", nil, nil}, {"5511", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 8},{"MFD_DataEntryButton_frame",pb.B4}}} } },
					{ pb.B5, { {"K >", nil, nil}, {"5711", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 9},{"MFD_DataEntryButton_frame",pb.B5}}} } },
	--			} 
	--},	
	
	--{ "RIGHT",
	--			{ 
					{ pb.R6, { {"L >", nil, nil}, {"1621", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 10},{"MFD_DataEntryButton_frame",pb.R6}}} } },
					{ pb.R5, { {"M >", nil, nil}, {"1666", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 11},{"MFD_DataEntryButton_frame",pb.R5}}} } },
					{ pb.R4, { {"N >", nil, nil}, {"1732", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 12},{"MFD_DataEntryButton_frame",pb.R4}}} } },
					{ pb.R3, { {"P >", nil, nil}, {"1777", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 13},{"MFD_DataEntryButton_frame",pb.R3}}} } },
					{ pb.R2, { {"Q >", nil, nil}, {"1782", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 14},{"MFD_DataEntryButton_frame",pb.R2}}} } },
					{ pb.R1, { {"R >", nil, nil}, {"1788", tp_default_border, {{"WPN_LaserCode_Frequency_Value", 15},{"MFD_DataEntryButton_frame",pb.R1}}} } },
	--			} 
	--},	
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createMenu( Menu, nil, 1 ,TRANSPARENT_BACKGROUND )
createControls( Controls, nil, nil, 1, TRANSPARENT_BACKGROUND )

addPB_Menu( { pb.T4, "    ", nil }, nil, 0 )		-- crutch for bright arrow

Add_CODE_RANGES_Window()

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------