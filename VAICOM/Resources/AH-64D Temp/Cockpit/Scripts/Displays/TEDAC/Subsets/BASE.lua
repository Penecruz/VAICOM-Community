dofile(LockOn_Options.script_path.."Displays/TEDAC/TEDAC_Symbology.lua")


--background for special cases , do not remove 
local half_w				= display_size_pix / 2
local half_h				= GetHalfHeight()/GetScale()

local back_black			= CreateElement "ceMeshPoly"
back_black.material			= MakeMaterial(nil,{0,0,0,255})
back_black.vertices			= {{-half_w,half_h},{half_w,half_h},{half_w,-half_h},{-half_w,-half_h}}
back_black.indices			= {0,1,2,0,2,3}
back_black.controllers 		= {{"render_purpose",2}}
Add(back_black)


----------------------------------------------------------------------------------------------------------------------------------
-- Bottom PB Labels --------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------
local pb_lbl_pos_y	= (-display_size_pix_05 + 25) / ScaleZoom
local pb_box_pos_y	= pb_lbl_pos_y - 3 / ScaleZoom
local pb_pos_delta	= 185 / ScaleZoom / DisplaySizeCorr
local pb_pos_left	= -155 / ScaleZoom
local pb_pos_x1		= pb_pos_left + pb_pos_delta * 0
local pb_pos_x2		= pb_pos_left + pb_pos_delta * 1
local pb_pos_x3		= pb_pos_left + pb_pos_delta * 2
local pb_pos_x4		= pb_pos_left + pb_pos_delta * 3
local box_h			= 32

local pb_b1		= addText("PB_B1_Label", "AZ/EL",					{pb_pos_x1, pb_lbl_pos_y}, "CenterBottom")
local pb_b1_box	= addRect("PB_B1_Box", getTextWidth(5.5), box_h,	{pb_pos_x1, pb_box_pos_y}, "CenterBottom", 5, nil, {{"TEDAC_BASE_AZ_EL_Box"}})
setElemsClipLevel(pb_b1, -1)
setElemsClipLevel(pb_b1_box, -1)

local pb_b2		= addText("PB_B2_Label", "ACM",						{pb_pos_x2, pb_lbl_pos_y}, "CenterBottom")
local pb_b2_box	= addRect("PB_B2_Box", getTextWidth(3.5), box_h,	{pb_pos_x2, pb_box_pos_y}, "CenterBottom", 5, nil, {{"TEDAC_BASE_ACM_Box"}})
setElemsClipLevel(pb_b2, -1)
setElemsClipLevel(pb_b2_box, -1)

local pb_b3		= addText("PB_B3_Label", "FREEZE",					{pb_pos_x3, pb_lbl_pos_y}, "CenterBottom")
local pb_b3_box	= addRect("PB_B3_Box", getTextWidth(6.5), box_h,	{pb_pos_x3, pb_box_pos_y}, "CenterBottom", 5, nil, {{"TEDAC_BASE_FREEZE_Box"}})
setElemsClipLevel(pb_b3, -1)
setElemsClipLevel(pb_b3_box, -1)

local pb_b4		= addText("PB_B4_Label", "FILTER",					{pb_pos_x4, pb_lbl_pos_y}, "CenterBottom")
local pb_b4_box	= addRect("PB_B4_Box", getTextWidth(6.5), box_h,	{pb_pos_x4, pb_box_pos_y}, "CenterBottom", 5, nil, {{"TEDAC_BASE_FILTER_Box"}})
setElemsClipLevel(pb_b4, -1)
setElemsClipLevel(pb_b4_box, -1)

----------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------

AddBackground("BackgroundMask", true)
