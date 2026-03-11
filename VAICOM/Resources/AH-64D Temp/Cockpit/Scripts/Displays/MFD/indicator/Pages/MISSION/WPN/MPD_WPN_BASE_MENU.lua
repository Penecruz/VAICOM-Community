dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local Controls = {}
Controls = 
{
	{ pb.B6, { {"MANRNG>", nil, nil}, {"4200", tp_default_border, { {"WPN_MAN_RNG_Value"},{"MFD_DataEntryButton_frame",pb.B6} }} } },
	
	{ pb.R5, { {"LRFD", nil, nil}, {"FIRST", tp_default_border, {{"WPN_LRFD_Mode_Selection"}}, {"FIRST", "LAST"}} } },
	{ pb.R6, { {"ACQ", nil, nil}, {"FXD", tp_default_border, {{"DSPLS_TSD_ACQ_Button_Caption"}}, {"PHS", "GHS", "SKR", "RFI", "FCR", "FXD", "TADS", "?00", "TRN", "ASE", "?PHS", "?GHS", "?SKR", "?RFI", "?FCR", "?FXD", "?TADS", "?00", "?TRN", "?ASE"}} } },
}

-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createControls( Controls )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------