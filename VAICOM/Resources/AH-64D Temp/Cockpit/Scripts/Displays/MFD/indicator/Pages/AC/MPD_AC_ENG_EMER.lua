dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_ENG_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_AC_ENG_INCLUDE.lua")

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

local lines = 6
local space = 20
local l_up = { -500, lowTextY-10 }
local r_dn = { 500, lowTextY - 10 - lines*( tp_36.height+space ) - space  }
local ph = addPlaceholder( nil, l_up, nil )
draw_box( l_up, r_dn,  nil, nil, nil, nil)
