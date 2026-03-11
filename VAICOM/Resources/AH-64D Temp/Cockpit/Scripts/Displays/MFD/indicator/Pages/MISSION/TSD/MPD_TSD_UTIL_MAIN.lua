dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.R1, { {"ASE AUTOPAGE", nil, nil}, {"SEARCH", tp_default_border, {{"TSD_UTIL_ASE_AUTOPAGE_Caption"}}, {"SEARCH", "ACQUISITION", "TRACK", "OFF"}} } },
	{ pb.R2, { {"TIME", nil, nil}, {"ZULU", tp_default_border, {{"TSD_UTIL_TIME_Btn_Caption"}}, {"ZULU","LOCAL"}} } },
	{ pb.R3, { {"SYSTEM TIME>", nil, nil}, {"11:59:59", tp_default_border, { {"TSD_UTIL_SYSTEM_TIME_Caption"},{"MFD_DataEntryButton_frame",pb.R3} }} } },
	{ pb.R4, { {"SYSTEM DATE>", nil, nil}, {"11/12/01", tp_default_border, { {"TSD_UTIL_SYSTEM_DATE_Caption"},{"MFD_DataEntryButton_frame",pb.R4} }} } }
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

if DBG_LABEL_SHOW then
addText( "TSD UTIL PAGE",  {0, 410}, tp_36_white)
end

-- draw menus

createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------
