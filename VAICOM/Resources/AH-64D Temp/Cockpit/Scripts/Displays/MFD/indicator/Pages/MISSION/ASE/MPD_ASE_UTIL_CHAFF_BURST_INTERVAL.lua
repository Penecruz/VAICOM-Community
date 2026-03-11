dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Controls =
{
	{ "BURST INTERVAL",
			{ 
				{ pb.L3,	"0.4",	nil,	{{"ASE_CHAFF_BurstInterval_Selection",3}}},
				{ pb.L4,	"0.3",	nil,	{{"ASE_CHAFF_BurstInterval_Selection",2}}},
				{ pb.L5,	"0.2",	nil,	{{"ASE_CHAFF_BurstInterval_Selection",1}}},
				{ pb.L6,	"0.1",	nil,	{{"ASE_CHAFF_BurstInterval_Selection",0}}},
			}
	},
}
createControls( Controls )