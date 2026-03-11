dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_ENG_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_AC_ENG_INCLUDE.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MPD_WCA_Messages.lua")

local pos 		= { (barCenterX[8]+barCenterX[9])/2 - 90, lowTextY+430 }

local Boxes = {}
	-- first line is required in format: 	{pos_left_up, controllers,  name, parent, h_space,  text_properties,  margins,},
	-- next : 	{{ text, text_properties, controllers, 	formats, 	margins,  	name }, {},{}...} 
	-- margins: left, down, right, up 
Boxes = 
{ 	
	{
		{ pos, {{"EngHydInfoShowInFlight"}}, nil, nil, nil, nil, { 10,10,15,-10 } },
		{
			{"HYD PSI", tp_28, nil, nil, {1,0,0,0}}
		},
		{
			{"PRI", tp_28, nil, nil, {10,0,0,0} }, 
			{"3000", tp_36_right_bottom, {{"EngHydPCI", 1}}, nil, {200,0,0,0}}
		},
		{
			{"UTIL", tp_28, nil, nil, {10,0,0,0}}, 
			{"3000", tp_36_right_bottom, {{"EngHydPCI", 2}}, nil, {200,0,0,0} } 
		},
		{
			{"ACC", tp_28, nil, nil, {10,0,0,0}}, 
			{"3000", tp_36_right_bottom, {{"EngHydPCI", 3}}, nil, {200,0,0,0} } 
		}
		
	},
	{
		{ pos, {{"EngOilInfoShowInFlight"}}, nil, nil, nil, nil, { 10,10,15,-10 } },
		{
			{"ENGINE", tp_28, nil, nil, {1,0,0,0}}
		},
		{
			{" OIL PSI", tp_28,nil, nil, {10,0,0,0} }
		},
		{
			{"   ", tp_36_right_bottom, {{"EngOilPCI", 1}}, nil, {100,0,0,0} }, 
			{"   ", tp_36_right_bottom, {{"EngOilPCI", 2}}, nil, {100,0,0,0} }
		},
		{
			{"  1    2", tp_28, nil, nil, {1,0,0,0} }
		}	
	}	
}
createInfoBoxes( Boxes )

addRoundedBox("engInFlight_OIL_PCI_1_Border", { pos[1] + 70, pos[2] -140}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngOilPCI_Border",1}, {"EngOilInfoShowInFlight"}})
addRoundedBox("engInFlight_OIL_PCI_2_Border", { pos[1] +170, pos[2] -140}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngOilPCI_Border",2}, {"EngOilInfoShowInFlight"}}) 

addRoundedBox("engInFlight_HYD_PRI_Border", { pos[1] + 175, pos[2] -90},  "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngHydPCI_Border",1}, {"EngHydInfoShowInFlight"}})
addRoundedBox("engInFlight_HYD_UNIL_Border",{ pos[1] + 175, pos[2] -138}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngHydPCI_Border",2}, {"EngHydInfoShowInFlight"}})
addRoundedBox("engInFlight_HYD_ACC_Border", { pos[1] + 175, pos[2] -186}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngHydPCI_Border",3}, {"EngHydInfoShowInFlight"}})

local Y = lowTextY + 3*text_properties_default.height

Y = lowTextY + 45
addText( "  .  ", { barCenterX[8], Y }, tp_36_center_bottom	, {{"EngParamNG", 1}} )
addText( "  .  ", { barCenterX[9], Y }, tp_36_center_bottom	, {{"EngParamNG", 2}} )

addRoundedBox("engParamNG_1", { barCenterX[8], Y-3}, "CenterBottom", {110, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngParamNGBorder", 1}})
addRoundedBox("engParamNG_2", { barCenterX[9], Y-3}, "CenterBottom", {110, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngParamNGBorder", 2}})
 Y = Y + 53 
addText( "N",  { (barCenterX[8]+barCenterX[9])/2, Y }, tp_28_center_bottom	 )
addText( "G",  { (barCenterX[8]+barCenterX[9])/2+text_properties_default.width, Y-6 }, tp_28_center_bottom )
addText( "%", {  (barCenterX[8]+barCenterX[9])/2+2*text_properties_default.width,  Y }, tp_28_center_bottom )

Y = Y + 45   
addText( "   ", { barCenterX[8], Y }, tp_36_center_bottom	, {{"EngParamNP", 1}} )
addText( "  ", { barCenterX[9], Y }, tp_36_center_bottom	, {{"EngParamNP", 2}} ) 
 
 Y = Y + 50 
addText( "N",  { (barCenterX[8]+barCenterX[9])/2, Y }, tp_28_center_bottom )
addText( "P",  { (barCenterX[8]+barCenterX[9])/2+text_properties_default.width, Y-6 }, tp_28_center_bottom )
addText( "%", {  (barCenterX[8]+barCenterX[9])/2+2*text_properties_default.width, Y }, tp_28_center_bottom ) 

--function draw_line( verts, material, parent, width, name, controllers ){
	local pos_info 	= { -500, lowTextY-10 }
	local space = 20;
	local lines = 6;
	local cols = 2;
	local l_up = { -500, lowTextY-10 }
	local r_dn = { 500, lowTextY - 10 - lines*(tp_36.height+space) - space  }
	
	addInfoBoxBackground( 1000, lines*( tp_36.height+space ) + space,  l_up, nil, {{"EngCautionsWindowBox"}}, DEFAULT_LEVEL+1, h_clip_relations.REWRITE_LEVEL, false )
	local ph = addPlaceholder( nil, l_up, nil,{{"EngCautionsWindow"}} )
	draw_box( l_up, r_dn,  nil, nil,{{"EngCautionsWindowBox"}})
	local pos_cur = {  20, -20 - tp_36.height  }
	
for col=1,cols do
	for line=1,lines do 
		addSimpleText( "", {pos_cur[1]+500*(col-1), pos_cur[2]}, tp_36_yellow, nil, WCA_Messages, nil,  "ST"..col..line, ph.name, h_clip_relations.COMPARE, DEFAULT_LEVEL+1, false )
		pos_cur = { pos_cur[1], pos_cur[2] - tp_36.height - space }
	end 
	pos_cur = {  20, -20 - tp_36.height  }
end

draw_line( {{0, l_up[2]},{0, r_dn[2]}}, tp_36.material, nil, nil,nil,{{"EngCautionsWindowBox"}} )