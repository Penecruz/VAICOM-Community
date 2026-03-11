dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.R3, "CTR",		tp_default_border,	{{"TSD_CTR_Button_frame"}}},
	{ pb.R4, "FRZ",		tp_default_border,	{{"TSD_FRZ_Button_frame"}}},
	{ pb.R5, { {"ORIENT", nil, nil}, {"TRK-UP", tp_default_border, {{"TSD_MAP_Orient_Caption"}}, {"HDG-UP","TRK-UP","N-UP"}} } },
	{ pb.R6, { {"VIEW", nil, nil}, {"2D", tp_default_border, {{"TSD_MAP_View_Caption"}}, {"2D","3D"}} } },

	{ pb.L2, { {"TYPE", nil, nil}, {"DIG", tp_default_border, {{"TSD_MAP_TypeBtn_Caption"}}, {"DIG", "CHART", "SAT", "STICK"}} } },
	
	{ pb.L3, { {"LEVEL", nil, {{"TSD_MAP_LevelBtn_DIG_Show"}}}, {"2", tp_default_border, {{"TSD_MAP_LevelBtn_DIG_Show"},{"TSD_MAP_DIG_LevelCaption"}}, {"0","1","2"}} } },
	{ pb.L3, { {"SCALE", nil, {{"TSD_MAP_ScaleBtn_Show"}}}, {"1:2M", tp_default_border, {{"TSD_MAP_ScaleBtn_Show"},{"TSD_MAP_Scale_Caption"}}, {"1:5M", "1:2M", "1:1M", "1:500K", "1:250K", "1:100K", "1:50K", "1:12.5K"}} } },
	{ pb.L3, { {"LEVEL", nil, {{"TSD_MAP_LevelBtn_SAT_Show"}}}, {"2M", tp_default_border, {{"TSD_MAP_LevelBtn_SAT_Show"},{"TSD_MAP_SAT_LevelCaption"}}, {"10M", "5M"}} } },

	
	{ pb.L4, { {"COLOR BAND", nil, nil}, {"NONE", tp_default_border, {{"TSD_MAP_ColorBand_Caption"}}, {"A/C","ELEV","NONE"}} } },
	{ pb.L5, { {"CONTOURS", nil, nil}, {"NONE", tp_default_border, {{"TSD_MAP_Contours_Caption"}}, {"NONE", "50", "100", "200", "500", "1000"}} } },
	
	-- TODO: button will be barriered if FFD data cannot be displayed at the selected map viewing range.
	{ pb.L6, { {"FFD", nil, nil}, {"NONE", tp_default_border, {{"TSD_MAP_FFD_Caption"}}, {"AREAS","LINES","BOTH","NONE"}} } },
}


-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

if DBG_LABEL_SHOW then
	--addText( "TSD MAP PAGE MAIN",  {0, 350}, tp_36_white)
end

-- draw menus
createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

