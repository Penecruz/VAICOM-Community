dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local OnOffPageMenu = addPlaceholder(nil, nil, nil, {{"DSPLS_FCR_CurrentSubset", 1}})

pb_props[pb.L2].show_barrier = false
pb_props[pb.R2].show_barrier = false

local controls = {}
controls=
{
    { pb.T1, "C SCP",       tp_default_border,  {{"DSPLS_FCR_cScopeBoxShow"}}},
    { pb.L1, "N\nT\nS", 	nil,	            nil},
    { pb.L2, "c", 	        tp_52,	            nil},
    { pb.L4, "T\nG\nT",		nil,	            nil},
    { pb.L5,  {{"ELEV",     nil,                {{"DSPLS_FCR_AG_ShowButtonElevationControl", 1}}}, {"AUTO", tp_default_border,{{"DSPLS_FCR_AG_ShowButtonElevationControl", 1}}}}},
    { pb.L5, "a",           tp_52,              {{"DSPLS_FCR_AG_ShowButtonElevationControl", 0}, {"DSPLS_FCR_OnOffShowButtonElevationGTM", 1}}},
    { pb.L6, "b",           tp_52,              {{"DSPLS_FCR_AG_ShowButtonElevationControl", 0}, {"DSPLS_FCR_OnOffShowButtonElevationGTM", 0}}},

    { pb.R1, "Z\nO\nO\nM",  nil,	            {{"DSPLS_FCR_ZoomButtonShow", 0}}},
    { pb.R1, "Z\nO\nO\nM",  tp_default_border,  {{"DSPLS_FCR_ZoomButtonShow", 1}}},
    { pb.R1, "Z\nO\nO\nM",  tp_default_inv,     {{"DSPLS_FCR_ZoomButtonShow", 2}}},

    { pb.R2, "d",	        tp_52,	            nil},
    { pb.R4, "RF\nHO",	    nil,	            nil},
    { pb.R6, {{"ACQ", nil, nil}, {"FXD", tp_default_border, {{"DSPLS_TSD_ACQ_Button_Caption"}}, {"PHS", "GHS", "SKR", "RFI", "FCR", "FXD", "TADS", "?00", "TRN", "ASE", "?PHS", "?GHS", "?SKR", "?RFI", "?FCR", "?FXD", "?TADS", "?00", "?TRN", "?ASE"}} } },
}

local pos_y_R1 = 35
local pos_y_L1 = 25 -- for L4
local pos_y_dl_v = 10
local pos_x_dl_v_R2 = 33
local pos_x_dl_v_L2 = 10
local pos_y = 5

local verticesR2 = {{-5, -50}, {-5,  35}, {-32,  35}, {-32, -50}}
draw_dl_v( {pb_props[pb.R2].pos[1] + pos_x_dl_v_R2, pb_props[pb.R2].pos[2] - pos_y_dl_v}, verticesR2, IND_MPD_1024_MATERIAL_DARK_GREEN, nil, {{"DSPLS_PB_Selectabled", pb.R2}} )

local verticesL2 = 	{{-5, -50}, {-5,  35}, {-32,  35}, {-32, -50}} 
draw_dl_v( {pb_props[pb.L2].pos[1] - pos_x_dl_v_L2, pb_props[pb.L2].pos[2] - pos_y_dl_v}, verticesL2, IND_MPD_1024_MATERIAL_DARK_GREEN, nil, {{"DSPLS_PB_Selectabled", pb.L2}} )

pb_props[pb.R1].pos[2] = pb_props[pb.R1].pos[2] + pos_y_R1

pb_props[pb.L1].pos[2] = pb_props[pb.L1].pos[2] + pos_y_L1
pb_props[pb.L2].pos[1] = pb_props[pb.L2].pos[1] - pos_y_dl_v

pb_props[pb.L2].pos[2] = pb_props[pb.L2].pos[2] + pos_y
pb_props[pb.R2].pos[2] = pb_props[pb.R2].pos[2] + pos_y

pb_props[pb.L4].pos[2] = pb_props[pb.L4].pos[2] + pos_y_L1

createControls( controls, nil, OnOffPageMenu.name )

pb_props[pb.R1].pos[2] = pb_props[pb.R1].pos[2] - pos_y_R1
pb_props[pb.L1].pos[2] = pb_props[pb.L1].pos[2] - pos_y_L1
pb_props[pb.L2].pos[1] = pb_props[pb.L2].pos[1] + pos_y_dl_v
pb_props[pb.L2].pos[2] = pb_props[pb.L2].pos[2] - pos_y
pb_props[pb.R2].pos[2] = pb_props[pb.R2].pos[2] - pos_y
pb_props[pb.L4].pos[2] = pb_props[pb.L4].pos[2] - pos_y_L1

