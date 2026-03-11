dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_FLT_Symbology.lua")

--addText( "DMS SHUTDOWN PAGE",  {0, 100})

local Menu = {}
Menu = 
{ 
	{ pb.T1, "DTU",			nil },
	{ pb.T2, "FAULT",		nil },
	{ pb.T3, "IBIT",		nil },
	{ pb.T4, {{"SHUT",	nil },{"DOWN",	nil }},			tp_default_border},
	{ pb.T5, "VERS",		nil },
	{ pb.T6, "UTIL",		nil },
	{ pb.B1, "DMS",			nil },
	{ pb.B6, "WCA",			nil },
}

createMenu( Menu )


local Controls = {}
Controls = 
{		

	{ pb.L1, 	{{"MASTER",	nil },{"OFF",	tp_default_border,		{{"MFD_DataEntryButton_frame",pb.L1}},	nil }},				nil,		nil},
	{ pb.L2, 	"}FCR",						tp_default_inv,			{{"DMS_SHUTDOWN_FCR_Button", 1}},	{"{FCR", "}FCR"}},
	{ pb.L2, 	"}FCR",						nil,					{{"DMS_SHUTDOWN_FCR_Button", 0}},	{"{FCR", "}FCR"}},
	{ pb.L3, 	"{TADS",					nil,					{{"DMS_SHUTDOWN_TADS_Button"}},	{"{TADS", "}TADS"}},
	{ pb.L4, 	"{PNVS",					nil,					{{"DMS_SHUTDOWN_PNVS_Button"}},	{"}PNVS", "{PNVS"}},
	{ pb.R5, 	"MODE 4 HOLD",				tp_default_border,		{{"DMS_SHUTDOWN_Mode4_Hold"}}},
	{ pb.R6, 	{{"MODEM",	nil },{"OPER",	tp_default_border, 		{{"DMS_SHUTDOWN_IDM_Button"}}, 	{"STBY", "OPER"} }},		nil,		nil},
	{ pb.B3, 	{{"DTU",	nil },{"OPER",	tp_default_border, 		{{"DMS_SHUTDOWN_DTU_Button"}}, 	{"STBY", "OPER"} }},		nil,		nil},	

}

createControls( Controls )
PosX = pb_props[pb.T4].pos[1]
PosY = pb_props[pb.L4].pos[2]
local stabilizator_ph = addRotPlaceholder("stabilizator_ph", {PosX,PosY}, 0, nil, {{"FLT_Stabilizator"}})
draw_stabilizator( {0,0},  IND_MPD_1024_MATERIAL_WHITE, stabilizator_ph.name,{{"FLT_StabilizatorColor"}} )
draw_stabilizator_scale( {PosX+70,PosY},  IND_MPD_1024_MATERIAL_WHITE, nil, {{"FLT_StabilizatorColor"}} )
draw_stabilizator_question( {PosX+70,PosY},  IND_MPD_1024_MATERIAL_WHITE, nil, {{"FLT_StabilizatorQuestion"}} )