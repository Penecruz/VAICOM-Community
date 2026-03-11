dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_PB_Defs.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Tools.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Materials.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_StringSymbology.lua")
dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_MenuSymbology.lua")

fill_pb_props()
menu_draw_scale = 720.0 / 1024.0

-- Center of the video area
local OnOffPageMenu = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 1}})
local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", video_area_pos, OnOffPageMenu.name, nil)

local OnOffPageMenuCommon = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 0}})
local VideoAreaPHCommonMenu = addPlaceholder("VideoAreaCenterPHCommonMenu", video_area_pos,  OnOffPageMenuCommon.name)

-- ATTENTION !!!
-- DO NOT USE ANY TEDAC SYMBOLOGY IN THIS SUBSET

pb_props[pb.L2].show_barrier = false
pb_props[pb.R2].show_barrier = false

local Controls = {}
Controls = 
{ 
	{ pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
	{ pb.R6, {{"ACQ", nil, nil}, {"FXD", tp_default_border, {{"DSPLS_TSD_ACQ_Button_Caption"}}, {"PHS", "GHS", "SKR", "RFI", "FCR", "FXD", "TADS", "?00", "TRN", "ASE", "?PHS", "?GHS", "?SKR", "?RFI", "?FCR", "?FXD", "?TADS", "?00", "?TRN", "?ASE"}} } },
}
createControls( Controls, nil, VideoAreaPHCommonMenu.name )

local controls = {}
controls=
{
	{ pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},

	{ pb.L1, "N\nT\nS",		nil,				nil},
	{ pb.L2, "c",			tp_52,				nil},
	{ pb.L4, "T\nG\nT",		nil,				nil},
	{ pb.L5, {{"ELEV",		nil,				{{"DSPLS_FCR_AG_ShowButtonElevationControl", 1}}}, {"AUTO", tp_default_border, {{"DSPLS_FCR_AG_ShowButtonElevationControl", 1}}}}},
    { pb.L5, "a",           tp_52,              {{"DSPLS_FCR_AG_ShowButtonElevationControl", 0}, {"DSPLS_FCR_OnOffShowButtonElevationGTM", 1}, {"DSPLS_FCR_OnOffShowButtonElevationATM", 1}}},
    { pb.L6, "b",           tp_52,              {{"DSPLS_FCR_AG_ShowButtonElevationControl", 0}, {"DSPLS_FCR_OnOffShowButtonElevationGTM", 0}, {"DSPLS_FCR_OnOffShowButtonElevationATM", 0}}},

	{ pb.R1, "Z\nO\nO\nM",  nil,	            {{"DSPLS_FCR_ZoomButtonShow", 0}}},
    { pb.R1, "Z\nO\nO\nM",  tp_default_border,  {{"DSPLS_FCR_ZoomButtonShow", 1}}},
    { pb.R1, "Z\nO\nO\nM",  tp_default_inv,     {{"DSPLS_FCR_ZoomButtonShow", 2}}},

	{ pb.R2, "d",			tp_52,				nil},
	{ pb.R4, "RF\nHO",		nil,				nil},
	{ pb.R6, {{"ACQ", nil, nil}, {"FXD", tp_default_border, {{"DSPLS_TSD_ACQ_Button_Caption"}}, {"PHS", "GHS", "SKR", "RFI", "FCR", "FXD", "TADS", "?00", "TRN", "ASE", "?PHS", "?GHS", "?SKR", "?RFI", "?FCR", "?FXD", "?TADS", "?00", "?TRN", "?ASE"}} } },
}

local pos_y_R1 = 30
local pos_y_L1 = 20
local pos_y_dl_v = 0
local pos_x_dl_v_R2 = 33
local pos_x_dl_v_L2 = 10
local pos_y = 5
local disable_line_width = 15

local verticesR2 =	{	{(-5) * menu_draw_scale,						(-50)	* menu_draw_scale},
						{(-5) * menu_draw_scale, 						(35)	* menu_draw_scale},
						{(-5 - disable_line_width) * menu_draw_scale, 	(35)	* menu_draw_scale},
						{(-5 - disable_line_width) * menu_draw_scale, 	(-50)	* menu_draw_scale}	}

draw_dl_v( {pb_props[pb.R2].pos[1] + 22, pb_props[pb.R2].pos[2] + 46 }, verticesR2, IND_MPD_1024_MATERIAL_DARK_GREEN, nil, {{"DSPLS_PB_Selectabled", pb.R2}} )

local verticesL2 =	{	{(12) * menu_draw_scale, 						(-50) 	* menu_draw_scale},
						{(12) * menu_draw_scale, 						(35) 	* menu_draw_scale},
						{(12 + disable_line_width) * menu_draw_scale, 	(35) 	* menu_draw_scale},
						{(12 + disable_line_width) * menu_draw_scale, 	(-50) 	* menu_draw_scale}	}

draw_dl_v( {pb_props[pb.L2].pos[1] - 18, pb_props[pb.L2].pos[2] + 46}, verticesL2, IND_MPD_1024_MATERIAL_DARK_GREEN, nil, {{"DSPLS_PB_Selectabled", pb.L2}} )

pb_props[pb.R1].pos[2] = pb_props[pb.R1].pos[2] + pos_y_R1
pb_props[pb.L1].pos[2] = pb_props[pb.L1].pos[2] + pos_y_L1

pb_props[pb.L2].pos[2] = pb_props[pb.L2].pos[2] + pos_y
pb_props[pb.R2].pos[2] = pb_props[pb.R2].pos[2] + pos_y

createControls( controls, nil, VideoAreaPH.name )

pb_props[pb.R1].pos[2] = pb_props[pb.R1].pos[2] - pos_y_R1
pb_props[pb.L1].pos[2] = pb_props[pb.L1].pos[2] - pos_y_L1

pb_props[pb.L2].pos[2] = pb_props[pb.L2].pos[2] - pos_y
pb_props[pb.R2].pos[2] = pb_props[pb.R2].pos[2] - pos_y



