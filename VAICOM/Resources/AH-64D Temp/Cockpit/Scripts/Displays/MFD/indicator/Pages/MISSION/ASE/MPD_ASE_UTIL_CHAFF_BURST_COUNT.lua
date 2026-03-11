dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Controls =
{
	{ "BURST COUNT",
			{ 
				{ pb.L1,	"8",	nil,	{{"ASE_CHAFF_BurstCount_Selection",5}}},
				{ pb.L2,	"6",	nil,	{{"ASE_CHAFF_BurstCount_Selection",4}}},
				{ pb.L3,	"4",	nil,	{{"ASE_CHAFF_BurstCount_Selection",3}}},
				{ pb.L4,	"3",	nil,	{{"ASE_CHAFF_BurstCount_Selection",2}}},
				{ pb.L5,	"2",	nil,	{{"ASE_CHAFF_BurstCount_Selection",1}}},
				{ pb.L6,	"1",	nil,	{{"ASE_CHAFF_BurstCount_Selection",0}}},
			}
	},	
}
createControls( Controls )