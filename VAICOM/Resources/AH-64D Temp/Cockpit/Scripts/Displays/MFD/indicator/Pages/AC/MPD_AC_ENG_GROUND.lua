dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_ENG_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_AC_ENG_INCLUDE.lua")

local Boxes = {}
	-- first line is required in format: 	{pos_left_up, controllers,  name, parent, h_space,  text_properties,  margins,},
	-- next : 	{{ text, text_properties, controllers, 	formats, 	margins,  	name }, {},{}...} 
	-- margins: left, down, right, up 
Boxes = 
{ 	
	{
		{{-310,-130}, nil, nil, nil, nil, nil, { 10,10,15,-10 } },
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
	},
	{
		{{-50,-130}, nil, nil, nil, nil, nil, { 10,10,15,-10 } },
		{
			{"HYD PSI", tp_28, nil, nil, {1,0,0,0}}
		},
		{
			{"PRI  ", tp_28, nil, nil, {10,0,0,0}}, 
			{"3000", tp_36_right_bottom, {{"EngHydPCI", 1}}, nil, {200,0,0,0} } 
		},
		{
			{"UTIL ", tp_28, nil, nil, {10,0,0,0}}, 
			{"3000", tp_36_right_bottom, {{"EngHydPCI", 2}}, nil, {200,0,0,0} } 
		},
		{
			{"ACC  ", tp_28, nil, nil, {10,0,0,0}}, 
			{"3000", tp_36_right_bottom, {{"EngHydPCI", 3}}, nil, {200,0,0,0} } 
		}
	}
}
createInfoBoxes( Boxes, 1 )

addRoundedBox("engGround_OIL_PCI_1_Border", { -240,-270}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngOilPCI_Border",1}})
addRoundedBox("engGround_OIL_PCI_2_Border", { -140,-270}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngOilPCI_Border",2}}) 

addRoundedBox("engGround_HYD_PRI_Border", { 125, -220}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngHydPCI_Border",1}})
addRoundedBox("engGround_HYD_UNIL_Border",{ 125, -268}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngHydPCI_Border",2}})
addRoundedBox("engGround_HYD_ACC_Border", { 125, -316}, "CenterBottom", {90, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngHydPCI_Border",3}})

local Y = lowTextY + 3*text_properties_default.height
local l_up = { (barCenterX[8]+barCenterX[9])/2 - 120, lowTextY+130 }
local r_dn = { (barCenterX[8]+barCenterX[9])/2 + 120, lowTextY-100  }
local w = math.abs(r_dn[1]-l_up[1])
local h = math.abs(r_dn[2]-l_up[2])

addInfoBoxBackground( w, h,  l_up, nil, {{"EngSTART"}},  DEFAULT_LEVEL+1, h_clip_relations.REWRITE_LEVEL, false )

addText( "1", { barCenterX[8], lowTextY}, tp_28_center_bottom, {{"EngNgTimer", 1}} ) 
addText( "2", { barCenterX[9], lowTextY}, tp_28_center_bottom, {{"EngNgTimer", 2}} )

addText( "", { (barCenterX[8]+barCenterX[9])/2, lowTextY}, tp_28_center_bottom, {{"EngNgTimer", 3}} )

Y = lowTextY - 50
addText( "ON", { barCenterX[8], Y }, tp_36_white_center_bottom, {{"EngParamSTARTER", 1}}, {"", "ON", "OVRD"} )
addText( "ON", { barCenterX[9], Y }, tp_36_white_center_bottom, {{"EngParamSTARTER", 2}}, {"", "ON", "OVRD"} )
 Y = Y - 15 
addText( "START",  { (barCenterX[8]+barCenterX[9])/2, Y }, tp_28_center_top, {{"EngSTART"}} )

Y = lowTextY + 45
addText( "  .  ", { barCenterX[8], Y }, tp_36_center_bottom	, {{"EngParamNG", 1}} )
addText( "  .  ", { barCenterX[9], Y }, tp_36_center_bottom	, {{"EngParamNG", 2}} )
addRoundedBox("engParamNG_1", { barCenterX[8], Y-3}, "CenterBottom", {110, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngParamNGBorder", 1}})
addRoundedBox("engParamNG_2", { barCenterX[9], Y-3}, "CenterBottom", {110, 46}, 1, 4, nil, nil, IND_MPD_MATERIAL_RED, {{"EngParamNGBorder", 2}}) 

Y = Y + 50 
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

draw_box( l_up, r_dn,  nil, nil, {{"EngSTART"}}, nil)