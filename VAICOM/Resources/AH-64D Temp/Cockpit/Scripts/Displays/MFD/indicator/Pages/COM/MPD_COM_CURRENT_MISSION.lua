dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T2, "DL",		nil },
	{ pb.T3, "XPNDR",	nil },
	{ pb.T4, "UHF",		nil },
	{ pb.T5, "FM",		nil },
	{ pb.T6, "HF",		nil },
	{ pb.R1, "CURRENT MISSION", tp_default_border, {{"MPD_COM_BTN_BORDER", pb.R1}} },

	{ "MSG",
		{
			{ pb.B5, "REC", nil },
			{ pb.B6, "SEND", tp_default_border, {{"MPD_COM_BTN_BORDER", pb.B6}} },
		}
	},
	
	
	{ pb.B1, "COM",		nil },
}
createMenu( Menu )

Controls = 
{	
	{ pb.L1,	"WAYPOINTS",		tp_default_border, {{"MFD_COM_CurrentMission", pb.L1}} },
	{ pb.L2,	"AREAS",			tp_default_border, {{"MFD_COM_CurrentMission", pb.L2}} },
	{ pb.L3,	"LINES",			tp_default_border, {{"MFD_COM_CurrentMission", pb.L3}} },
	{ pb.L5,	"TGT/THRT",			tp_default_border, {{"MFD_COM_CurrentMission", pb.L5}} },
	{ pb.L6,	"CONTROL MEASURES",	tp_default_border, {{"MFD_COM_CurrentMission", pb.L6}} },
	{ pb.R2,	"LASER CODES",		tp_default_border, {{"MFD_COM_CurrentMission", pb.R2}} },
	{ pb.B3,	"ROUTE",			tp_default_border, {{"MFD_COM_CurrentMission", pb.B3}} },
	{ pb.B4,	"ALL",				tp_default_border, {{"MFD_COM_CurrentMission", pb.B4}} },
}
createControls( Controls )
AddSendBtn("CurrentMission_SendMessageWindow", nil, {{"MFD_COM_CurrentMissionSendVisible"}} )
