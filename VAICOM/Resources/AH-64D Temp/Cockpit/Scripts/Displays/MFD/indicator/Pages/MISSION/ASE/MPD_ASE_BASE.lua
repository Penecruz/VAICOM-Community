dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/TSD/MPD_TSD_Symbology_defs.lua")

local MISSION_ASE = 6
function getPosOnRose(radius, angle)
	return { radius * math.sin(math.rad(angle)), radius * math.cos(math.rad(angle)) }
end
local Menu = {}
Menu = 
{ 
	{ pb.T2, "ASE",		tp_default_border, {{"MFD_AsePrevBorder", MISSION_ASE}} },
	{ pb.T6, "UTIL",	nil },
	{ pb.B1, "ASE",		tp_default_border, {{"MFD_AC_OriginatorAseFmt"}}, {"ASE", "WPN", "TSD"} }
}

createMenu( Menu )

local Controls = {}
Controls = 
{
	{ pb.T1, { {"CHAFF",		nil, nil}, {"SAFE",		tp_default_border, {{"ASE_CHAFF_ArmSafe_Status"}},	{"SAFE", "ARM"}} } },
	{ pb.L1, { {"CHAFF MODE",	nil, nil}, {"PROGRAM",	tp_default_border, {{"ASE_CHAFF_Mode"}},			{"PROGRAM", "MANUAL"}} } },
}
createControls( Controls )

local Boxes = {}
Boxes = 
{ 
	{
		{{-380,-310}, nil, nil, nil, nil, nil, { 10,10,10,-12 } },
		{{"CHAFF", tp_28}},
		{ {"   ", tp_18_black},{"00", tp_28, {{"ASE_CHAFF_Cartridges_Value"}}}}	
	}
}	
createInfoBoxes( Boxes )
	
AddCurrentHeadingLabel()
AddNextWaypointHeadingLabel()
local verts_circle	= buildEllipseVerts({0,0}, r, r)
local circle = draw_line( verts_circle, IND_MPD_MATERIAL_GREEN, nil, 3.0, "ase_circle", nil)
-- debug ticks
--for i = 15,350,30 do
--		addFatLine("Clock_ "..i,		20,		5,		getPosOnRose(r, i),	180-i,	nil,	nil,	IND_MPD_MATERIAL_GREEN)
--	end
for i = 6, 0, -1 do
	Emitter(i, nil, {{"RWR_Thread", i, r - 1.41*side05 }} )
	draw_dotted_line( {{0.0, 0.0}, { 0.5*r, 0.0 }}, IND_MPD_MATERIAL_YELLOW, nil, 3, nil, {{"ASE_RWR_LaunchLine", i, r-1.41*2*side05 }} )
	
end

AddOwnshipSymbolCenter({0,0}, IND_MPD_TSD_MATERIAL_BLACK)
AddOwnshipSymbolCenter({0,0}, IND_MPD_TSD_SYMBOLS_CYAN)




