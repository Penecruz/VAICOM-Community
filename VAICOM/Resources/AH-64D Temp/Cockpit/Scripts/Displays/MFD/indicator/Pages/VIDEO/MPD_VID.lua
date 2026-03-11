--dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_FLT_Symbology.lua")

function createScaleBox(pos, width, height)
	local w05 = width/2 
	local h05 = height/ 2
	local verts = {	{pos[1] - w05, pos[2] - h05}, 
					{pos[1] + w05, pos[2] - h05}, 
					{pos[1] + w05, pos[2] + h05}, 
					{pos[1] - w05, pos[2] + h05}, 
					{pos[1] - w05, pos[2] - h05} }
	draw_line( verts,  IND_MPD_MATERIAL_GREEN, nil, 3 )
 end

function createMasks(pos )
	local space_y = 6
	local h = 10
	local w = 30
	local w05 = w/2 
	local p_y = pos[2] - 4*( h + space_y )
	local p_x = pos[1]  
	
	for i=1, 7 do
		draw_line( {{p_x - w05, p_y}, {p_x + w05, p_y}} ,  IND_MPD_MATERIAL_GREEN, nil, 7, nil, {{"VIDEO_SharpScaleMask", i}} )
		p_y =  p_y + ( h + space_y )
	end
end

function createScale(pos)
	
	local space_y = 6
	local h = 10
	local w = 30
	local p_y = pos[2] - 4*( h + space_y )
	local p_x = pos[1]  
	
	local h_tot = 6*space_y + 7*h
	local p_yc = p_y - h/2
	local p_xc = p_x + w/2 
	 
	local verts = { 
	{p_xc, p_yc }, {p_xc, p_yc + h_tot }, {p_xc - w, p_yc + h_tot}, {p_xc - w, p_yc }, {p_xc, p_yc } 
	}
	local Background = addTranslucentBackground( verts, default_box_indices, nil, nil, nil )
	for i=1, 7 do
		createScaleBox( { p_x, p_y }, w, h )
		p_y =  p_y + ( h + space_y )
	end
end


local Menu = {}
Menu = 
{ 
	{ pb.T2, "VCR",			nil },
	{ pb.T3, "IMAGE",		nil },
}
createMenu( Menu )
local Controls = {}
Controls = 
{
	{ pb.B1, "VIDEO",		tp_default_border },
	{ "UNDERLAY", 
		{ 
			{ pb.R1, "TADS",		nil, {{"VIDEO_Underlay", 1}} },
			{ pb.R2, "CPG SIGHT",	nil, {{"VIDEO_Underlay", 2}} },
			{ pb.R3, "CPG HMD",		nil, {{"VIDEO_Underlay", 3}} },
			{ pb.R4, "PLT SIGHT",	nil, {{"VIDEO_Underlay", 4}} },
			{ pb.R5, "PLT HMD",		nil, {{"VIDEO_Underlay", 5}} },
			{ pb.R6, "NO VIDEO",	nil, {{"VIDEO_Underlay", 0}} }
		}
	},
	{ "VIEW", 
		{
			{ pb.L1, "WIDE",	nil, {{"VIDEO_View", 1}} },
			{ pb.L2, "NORM",	nil, {{"VIDEO_View", 2}} },
			{ pb.L3, "ZOOM",	nil, {{"VIDEO_View", 3}} },
		}
	},
	{ pb.L6, { 
				{"COLOR",	nil}, 
				{"WHITE", tp_default_border, {{"VIDEO_Color"}}, { "WHITE", "GREEN"} } 
			} 
	},
	{ pb.L4, "a",		tp_52 },
	{ pb.L5, "b",		tp_52 },
	{ pb.B2, {
				{"SYMB", nil },
				{"NORM", tp_default_border, {{"VIDEO_Symb"}}, { "NORM","BOOST"}}
			}
	},
	{ pb.B3, "RESET",	nil },
	{ pb.B6, "G/S",	nil },
	{ pb.T6, "VSEL", tp_default_border, {{"VIDEO_Declutter_Button"}}, { "VSEL","C-FLT", "P-FLT", "TADS", "C-FCR", "P-FCR"}},
}
createControls( Controls )
local p_y = ( pb_props[pb.L4].pos[2] +  pb_props[pb.L5].pos[2] )/2
addText( "SHARP", { -490, p_y-6 }, pb_props[pb.L4].tp , nil,  nil,  nil, nil, nil, nil, nil, false)
createScale({ -350, p_y })
createMasks({ -350, p_y })


