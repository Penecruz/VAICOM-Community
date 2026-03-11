dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T6, "TEXT",				nil },
	{ pb.R1, "CURRENT MISSION",		nil },
	{ pb.B1, "COM",					nil },
	{ pb.B4, "ATHS",				nil },
	{ "MSG",
		{
			{ pb.B5, "REC", nil },
			{ pb.B6, "SEND", tp_default_border, {{"MPD_COM_BTN_BORDER", pb.B6}} },
		}
	},
}

local Controls = {}
Controls = 
{
	{ pb.L1,	"MISSION 1",	tp_default_border,	{{"MPD_COM_MISSION_SEND", 1}} },
	{ pb.L2,	"MISSION 2",	tp_default_border,	{{"MPD_COM_MISSION_SEND", 2}} },
}

createMenu( Menu )
createControls( Controls )
AddSendBtn("Send_SendMessageWindow", nil, {{"MFD_COM_MissionSendVisible"}} )