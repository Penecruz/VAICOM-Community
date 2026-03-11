dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Menu = {}
Menu = 
{ 
	{ pb.T1, "DTU",			tp_default_border },
	{ pb.B1, "DMS",			nil },
}

createMenu( Menu )


local Controls = {}
Controls = 
{	
	{ "DATA", 
		{ 
			{ pb.L2, 	"CURRENT MSN",			tp_default_border,	{{"DMS_DTU_Data", 0}}},
			{ pb.L3, 	"MISSION 1",			tp_default_border,	{{"DMS_DTU_Data", 1}}},
			{ pb.L4, 	"MISSION 2",			tp_default_border,	{{"DMS_DTU_Data", 2}}},
			{ pb.L5, 	"COMMUNICATION",		tp_default_border,	{{"DMS_DTU_Data", 3}}},
			{ pb.L6, 	"AVIONICS UPDT",		nil,				{{"DMS_NoBorder"}}},
		}
	},
}

createControls( Controls )					
