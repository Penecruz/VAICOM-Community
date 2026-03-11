dofile(LockOn_Options.script_path.."Cameras/TADS/TADS_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/Common/Units.lua")

add_render_target({{"digital_zoom_controller"}})

-- prepare underlay level
local	verts = buildBoxVerts(GetHalfWidth() * 2, GetHalfHeight() * 2, "CenterCenter")
local	back					= CreateElement "ceMeshPoly"
		back.name				= "UnderlayBack"
		back.primitivetype		= "triangles"
		back.vertices			= verts
		back.indices			= default_box_indices
		back.isvisible			= false
		back.material			= "MASK_MATERIAL"
		back.level				= DEFAULT_LEVEL
		back.h_clip_relation	= h_clip_relations.REWRITE_LEVEL
		back.init_pos			= {0, 0, 0}
Add(back)


local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0}, nil, nil)


--
-- IAT Tracks
for i=0,2 do
	local TrackIAT_PH = addPlaceholder("TrackIAT_PH_"..i, {0,0}, VideoAreaPH.name, {{"TrackIAT_Show", i}, {"TrackIAT_Pos", i, video_area_w_05, video_area_h_05}})
	-- type
	local Primary_PH	= addPlaceholder("Primary_PH"..i,	{0,0}, TrackIAT_PH.name, {{"TrackIAT_Primary", i}})
	local Secondary_PH	= addPlaceholder("Secondary_PH"..i,	{0,0}, TrackIAT_PH.name, {{"TrackIAT_Secondary", i}})
	local PrimaryGates_PH	= addPlaceholder("PrimaryGates_PH"..i,	{0,0}, Primary_PH.name, {{"TrackIAT_PrimaryGates", i}})
	local SecondaryFlag_PH	= addPlaceholder("SecondaryFlag_PH"..i,	{0,0}, Secondary_PH.name, {{"TrackIAT_SecondaryFlag", i}})
	local Bar_PH			= addPlaceholder("Bar_PH"..i,			{0,0}, TrackIAT_PH.name, {{"TrackIAT_ShowBar", i}})
	-- primary
	local PrimaryGatesNormal_PH		= addPlaceholder("PrimaryNormal_PH"..i,		{0,0}, PrimaryGates_PH.name, {{"TrackIAT_Type", 0, i}})
	local PrimaryGatesBold_PH		= addPlaceholder("PrimaryBold_PH"..i,		{0,0}, PrimaryGates_PH.name, {{"TrackIAT_Type", 1, i}})
	local PrimaryGatesInertial_PH	= addPlaceholder("PrimaryInertial_PH"..i,	{0,0}, PrimaryGates_PH.name, {{"TrackIAT_Type", 2, i}})
	local PrimaryGatesLowConf_PH	= addPlaceholder("PrimaryLowConf_PH"..i,	{0,0}, PrimaryGates_PH.name, {{"TrackIAT_Type", 3, i}})
	local dr = 4
	-- normal gates
	local NormalGate_LT	= buildBorderedSymbol("NormalGate_LT"..i, {15,15}, "LeftTop",		{346,  2}, {-dr, dr}, PrimaryGatesNormal_PH.name, {{"TrackIAT_Size", i, -video_area_w_05,  video_area_h_05}})
	local NormalGate_RT	= buildBorderedSymbol("NormalGate_RT"..i, {15,15}, "RightTop",		{409,  2}, { dr, dr}, PrimaryGatesNormal_PH.name, {{"TrackIAT_Size", i,  video_area_w_05,  video_area_h_05}})
	local NormalGate_RB	= buildBorderedSymbol("NormalGate_RB"..i, {15,15}, "RightBottom",	{409, 65}, { dr,-dr}, PrimaryGatesNormal_PH.name, {{"TrackIAT_Size", i,  video_area_w_05, -video_area_h_05}})
	local NormalGate_LB	= buildBorderedSymbol("NormalGate_LB"..i, {15,15}, "LeftBottom",	{346, 65}, {-dr,-dr}, PrimaryGatesNormal_PH.name, {{"TrackIAT_Size", i, -video_area_w_05, -video_area_h_05}})
	-- bold gates
	local BoldGate_LT	= buildBorderedSymbol("BoldGate_LT"..i, {15,15}, "LeftTop",			{346, 70}, {-dr, dr}, PrimaryGatesBold_PH.name, {{"TrackIAT_Size", i, -video_area_w_05,  video_area_h_05}})
	local BoldGate_RT	= buildBorderedSymbol("BoldGate_RT"..i, {15,15}, "RightTop",		{409, 70}, { dr, dr}, PrimaryGatesBold_PH.name, {{"TrackIAT_Size", i,  video_area_w_05,  video_area_h_05}})
	local BoldGate_RB	= buildBorderedSymbol("BoldGate_RB"..i, {15,15}, "RightBottom",		{409,133}, { dr,-dr}, PrimaryGatesBold_PH.name, {{"TrackIAT_Size", i,  video_area_w_05, -video_area_h_05}})
	local BoldGate_LB	= buildBorderedSymbol("BoldGate_LB"..i, {15,15}, "LeftBottom",		{346,133}, {-dr,-dr}, PrimaryGatesBold_PH.name, {{"TrackIAT_Size", i, -video_area_w_05, -video_area_h_05}})
	-- inertial gates
	local InertialGate_LT	= buildBorderedSymbol("InertialGate_LT"..i, {15,15}, "LeftTop",		{414,  2}, {-dr, dr}, PrimaryGatesInertial_PH.name, {{"TrackIAT_Size", i, -video_area_w_05,  video_area_h_05}})
	local InertialGate_RT	= buildBorderedSymbol("InertialGate_RT"..i, {15,15}, "RightTop",	{443,  2}, { dr, dr}, PrimaryGatesInertial_PH.name, {{"TrackIAT_Size", i,  video_area_w_05,  video_area_h_05}})
	local InertialGate_RB	= buildBorderedSymbol("InertialGate_RB"..i, {15,15}, "RightBottom",	{443, 31}, { dr,-dr}, PrimaryGatesInertial_PH.name, {{"TrackIAT_Size", i,  video_area_w_05, -video_area_h_05}})
	local InertialGate_LB	= buildBorderedSymbol("InertialGate_LB"..i, {15,15}, "LeftBottom",	{414, 31}, {-dr,-dr}, PrimaryGatesInertial_PH.name, {{"TrackIAT_Size", i, -video_area_w_05, -video_area_h_05}})
	-- low confidence gates
	local LowConfGate_LT	= buildBorderedSymbol("LowConfGate_LT"..i, {15,15}, "LeftTop",		{448,  2}, {-dr, dr}, PrimaryGatesLowConf_PH.name, {{"TrackIAT_Size", i, -video_area_w_05,  video_area_h_05}})
	local LowConfGate_RT	= buildBorderedSymbol("LowConfGate_RT"..i, {15,15}, "RightTop",		{477,  2}, { dr, dr}, PrimaryGatesLowConf_PH.name, {{"TrackIAT_Size", i,  video_area_w_05,  video_area_h_05}})
	local LowConfGate_RB	= buildBorderedSymbol("LowConfGate_RB"..i, {15,15}, "RightBottom",	{477, 31}, { dr,-dr}, PrimaryGatesLowConf_PH.name, {{"TrackIAT_Size", i,  video_area_w_05, -video_area_h_05}})
	local LowConfGate_LB	= buildBorderedSymbol("LowConfGate_LB"..i, {15,15}, "LeftBottom",	{448, 31}, {-dr,-dr}, PrimaryGatesLowConf_PH.name, {{"TrackIAT_Size", i, -video_area_w_05, -video_area_h_05}})
	-- track number
	addText("PrimaryTrackIAT_TN_"..i,	i + 1,	{dr, -dr}, "LeftTop", PrimaryGates_PH.name, {{"TrackIAT_Size", i,  video_area_w_05, -video_area_h_05}})
	-- aim point
	local AimPoint	= buildBorderedSymbol("AimPoint_"..i, {8,8}, "CenterCenter",	{353,145}, {0,0}, PrimaryGates_PH.name, {{"TrackIAT_AimPoint", i, video_area_w_05, video_area_h_05}})

	-- secondary
	local SecondaryFlagAimPoint_PH	= addPlaceholder("SecondaryAimPoint_PH"..i,	{0,0}, SecondaryFlag_PH.name, {{"TrackIAT_AimPoint", i, video_area_w_05, video_area_h_05}})
	-- placeholders by type
	local SecondaryFlagNormal_PH	= addPlaceholder("SecondaryNormal_PH"..i,	{0,0}, SecondaryFlagAimPoint_PH.name, {{"TrackIAT_Type", 0, i}})
	local SecondaryFlagBold_PH		= addPlaceholder("SecondaryBold_PH"..i,		{0,0}, SecondaryFlagAimPoint_PH.name, {{"TrackIAT_Type", 1, i}})
	local SecondaryFlagInertial_PH	= addPlaceholder("SecondaryInertial_PH"..i,	{0,0}, SecondaryFlagAimPoint_PH.name, {{"TrackIAT_Type", 2, i}})
	local SecondaryFlagLowConf_PH	= addPlaceholder("SecondaryLowConf_PH"..i,	{0,0}, SecondaryFlagAimPoint_PH.name, {{"TrackIAT_Type", 3, i}})

	local flag_size = {41, 78}
	-- normal gates
	local NormalFlag	= buildBorderedSymbol("NormalFlag"..i,		flag_size, "LeftBottom", {  2,157}, {0, 0}, SecondaryFlagNormal_PH.name)
	local BoldFlag		= buildBorderedSymbol("BoldFlag"..i,		flag_size, "LeftBottom", { 88,157}, {0, 0}, SecondaryFlagBold_PH.name)
	local InertialFlag	= buildBorderedSymbol("InertialFlag"..i,	flag_size, "LeftBottom", {174,157}, {0, 0}, SecondaryFlagInertial_PH.name)
	local LowConfFlag	= buildBorderedSymbol("LowConfFlag"..i,		flag_size, "LeftBottom", {260,157}, {0, 0}, SecondaryFlagLowConf_PH.name)
	-- track number
	addText("SecondaryTrackIAT_TN_"..i,	i + 1,	{flag_size[1] / 2, flag_size[2] - 7}, "CenterTop", SecondaryFlagAimPoint_PH.name)

	-- bars
	local bar_pos =
	{
		up = 1,
		right = 2,
		down = 3,
		left = 4,
	}

	local bar_length = 59
	local NormalBar		= buildBorderedSymbol("NormalBar"..i,		{5, bar_length}, "CenterTop", {501,  2}, {0, 0}, Bar_PH.name, {{"TrackIAT_Type", 0, i}, {"TrackIAT_BarLength", i}})
	local InertialBar	= buildBorderedSymbol("InertialBar"..i,		{5, bar_length}, "CenterTop", {501,  2}, {0, 0}, Bar_PH.name, {{"TrackIAT_Type", 2, i}, {"TrackIAT_BarLength", i}})
	local LowConfBar	= buildBorderedSymbol("LowConfBar"..i,		{5, bar_length}, "CenterTop", {487,  2}, {0, 0}, Bar_PH.name, {{"TrackIAT_Type", 3, i}, {"TrackIAT_BarLength", i}})

	local PrimaryBarBrace_PH = addPlaceholder("PrimaryBarBrace_PH"..i,	{0,-(bar_length + 7) * ind_scale}, Bar_PH.name, {{"TrackIAT_Primary", i}, {"TrackIAT_BarBracePos", i, bar_length * ind_scale}})
	local PrimaryNormalBarBrace		= buildBorderedSymbol("PrimaryNormalBarBrace"..i,		{19, 11}, "CenterBottom", {433, 57}, {0, 0}, PrimaryBarBrace_PH.name, {{"TrackIAT_Type", 0, i}})
	local PrimaryInertialBarBrace	= buildBorderedSymbol("PrimaryInertialBarBrace"..i,		{19, 11}, "CenterBottom", {433, 57}, {0, 0}, PrimaryBarBrace_PH.name, {{"TrackIAT_Type", 2, i}})
	local PrimaryLowConfBarBrace	= buildBorderedSymbol("PrimaryLowConfBarBrace"..i,		{19, 11}, "CenterBottom", {433, 83}, {0, 0}, PrimaryBarBrace_PH.name, {{"TrackIAT_Type", 3, i}})

	local BarTN_PH	= addPlaceholder("BarTN_PH"..i,	{0,-(bar_length + 19) * ind_scale}, Bar_PH.name, {{"TrackIAT_BarTN", i}, {"TrackIAT_BarBracePos", i, bar_length * ind_scale}})
	addText("BarTrackIAT_TN_"..i,	i + 1,	{0, -4}, "CenterCenter", BarTN_PH.name)

end
--]]
