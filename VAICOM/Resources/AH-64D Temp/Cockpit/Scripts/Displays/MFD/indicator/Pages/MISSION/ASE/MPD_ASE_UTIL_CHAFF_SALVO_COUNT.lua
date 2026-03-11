dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local Controls =
{
	{ "SALVO COUNT",
			{ 
				{ pb.L1,	"CONTINUOUS",	nil,	{{"ASE_CHAFF_SalvoCount_Selection",4}}},
				{ pb.L2,	"8",			nil,	{{"ASE_CHAFF_SalvoCount_Selection",3}}},
				{ pb.L3,	"4",			nil,	{{"ASE_CHAFF_SalvoCount_Selection",2}}},
				{ pb.L4,	"2",			nil,	{{"ASE_CHAFF_SalvoCount_Selection",1}}},
				{ pb.L5,	"1",			nil,	{{"ASE_CHAFF_SalvoCount_Selection",0}}},
			}
	},	
}
createControls( Controls )