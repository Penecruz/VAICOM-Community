dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local MSG_TYPE_GROUP =
{
	COMMON	= 0,
	TEXT	= 1,
	REPORT	= 2,
	QUERY	= 3
}

local tp_def_top_white = createTextProperty( nil, "WHITE", IND_MPD_MATERIAL_WHITE, "CenterTop" )
local tp_top_white_border = createTextProperty( nil, "WHITE", IND_MPD_MATERIAL_WHITE, "CenterTop", nil, true )

local STOREBase = addPlaceholder("STOREBase_PH", {0, 0}, nil, {{"MPD_COM_REC_T2_Show"}})
local LOCATIONBase = addPlaceholder("LOCATIONBase_PH", {0, 0}, STOREBase.name, {{"MPD_COM_REC_T2_Type", MSG_TYPE_GROUP.COMMON}})

local Controls = {}
Controls = 
{	
	{ pb.T1,  
		{ 
			{"SOURCE",	nil}, 
			{"DL", tp_default_border, nil} 
		}
	},	
}
local StoreControls = {}
StoreControls = 
{
	{ pb.T2, "STORE",	tp_def_top_white, 	{{"MPD_COM_REC_T2_Type", MSG_TYPE_GROUP.COMMON}} },
	{ pb.T2, "RVW",		tp_def_top_white,	{{"MPD_COM_REC_T2_Type", MSG_TYPE_GROUP.TEXT},{"MPD_COM_REC_T2_Color"}} },
	{ pb.T2, "STORE",	tp_def_top_white, 	{{"MPD_COM_REC_T2_Type", MSG_TYPE_GROUP.REPORT}} },
	{ pb.T2, "REPLY",	tp_def_top_white, 	{{"MPD_COM_REC_T2_Type", MSG_TYPE_GROUP.QUERY}} },
}

local StoreLoactionControls = {}
StoreLoactionControls = 
{
	{ pb.T3, 	"CURR",		tp_top_white_border,	nil},
	{ pb.T4, 	"MSN1",		tp_def_top_white,		nil},
	{ pb.T5, 	"MSN2",		tp_def_top_white,		nil},
}

draw_border_with_caption( pb_props[pb.T3].pos, pb_props[pb.T5].pos,  11, 1, "LOCATION", tp_def_top_white, LOCATIONBase.name )

createControls( Controls )
createControls( StoreControls, nil, STOREBase.name )
createControls( StoreLoactionControls, nil, LOCATIONBase.name )