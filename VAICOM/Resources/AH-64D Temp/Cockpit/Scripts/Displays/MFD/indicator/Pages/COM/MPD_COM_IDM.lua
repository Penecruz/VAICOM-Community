dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")


local Menu = {}
Menu = 
{ 
	{ pb.T2, "DL",		tp_default_border },
	{ pb.B4, {{"ORIG",nil },{ "ID",	nil }}}, 
	{ pb.B1, "COM",		nil },
	{ "MSG",
		{
			{ pb.B5, "REC", nil },
			{ pb.B6, "SEND", tp_default_border, {{"MPD_COM_BTN_BORDER", pb.B6}} },
		}
	},
}
createMenu( Menu )
local Controls = {}

Controls =
{
 	{"AUTO SA",
		{
			{pb.R2, {{"TIME>", nil}, {"?", nil }}},
			{pb.R3, {{"DISTANCE>", nil}, {"?", nil }}},
		}
 	},
	 { pb.R4, {{"AUTO-SA", nil}, {"OFF", tp_default_border}}},  
	 { pb.R6, 
			{
				{"MODEM", nil}, 
				{"OPER", tp_default_border, {{"MPD_COM_ModemStatus"}}, {"OPER","STBY"} }
			}
	 },

	 { pb.L2,  {{"MSN TYPE", nil}, {"OPERATION", tp_default_border}}}, 
	 { pb.L3,  {{"MSG SECURITY", nil}, {"SECRET", tp_default_border}}},
	 { pb.L4,  {{"NAME/PASSWORD>", nil}, {"EAGLE1", nil}}},
	 { pb.L5,  {{"UTO", nil}, {"1: 3ID UTO BFTD", tp_default_border}}},
	 { pb.L6,  "MODEM ZEROIZE", nil },
	 
	 { pb.B3, {{"DL", nil },{ "INHBT", nil }}} 
}
createControls( Controls )
addBorder( pb.B3, 80, {{"MPD_COM_DLInhibit"}}, 15 )
AddDL_StatusWnd()
