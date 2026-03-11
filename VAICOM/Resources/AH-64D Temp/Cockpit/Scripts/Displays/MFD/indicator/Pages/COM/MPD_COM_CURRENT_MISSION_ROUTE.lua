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
	{ pb.L1,	"ROUTE",		tp_default_border,	{{"MFD_COM_CurrentMission_Route_Name", 0}} },
	{ pb.L2,	"ROUTE",		tp_default_border,	{{"MFD_COM_CurrentMission_Route_Name", 1}} },
	{ pb.L3,	"ROUTE",		tp_default_border,	{{"MFD_COM_CurrentMission_Route_Name", 2}} },
	{ pb.L4,	"ROUTE",		tp_default_border,	{{"MFD_COM_CurrentMission_Route_Name", 3}} },
	{ pb.L5,	"ROUTE",		tp_default_border,	{{"MFD_COM_CurrentMission_Route_Name", 4}} },
	{ pb.L6,	"ROUTE",		tp_default_border,	{{"MFD_COM_CurrentMission_Route_Name", 5}} },
	{ pb.R2,	"ROUTE",		tp_default_border,	{{"MFD_COM_CurrentMission_Route_Name", 6}} },
	{ pb.R3,	"ROUTE",		tp_default_border,	{{"MFD_COM_CurrentMission_Route_Name", 7}} },
	{ pb.R4,	"ROUTE",		tp_default_border,	{{"MFD_COM_CurrentMission_Route_Name", 8}} },
	{ pb.R5,	"ROUTE",		tp_default_border,	{{"MFD_COM_CurrentMission_Route_Name", 9}} },
	
	{ pb.B3,	"ROUTE",		tp_default_border,	nil },
	{ pb.B4,	"ALL",			tp_default_border,	{{"MFD_COM_CurrentMission_Route_All"}} },
}
createControls( Controls )

AddSendBtn("CurrentMission_Route_SendMessageWindow", nil, {{"MFD_COM_CurrentMission_Route_SendVisible"}} )
