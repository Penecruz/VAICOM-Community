dofile(LockOn_Options.script_path.."Displays/TEDAC/Subsets/FCR_page/FCR_BASE.lua")

local scaleSize = 720.0 / 1024.0

local VideoAreaPH = addPlaceholder("VideoAreaCenterTPM", video_area_pos, nil, nil)
local TPM_PH = addPlaceholder("TPM_PH_TEDAC", {0, 100}, VideoAreaPH.name, {{"DSPLS_FCR_GTM_Root"}})

boxHAD(TPM_PH.name)
totalTargetCountStatusWindow(TPM_PH.name)
wideScanTPM(TPM_PH.name) 
mediumScanTPM(TPM_PH.name)
directionWiperTPM(TPM_PH.name)
local TPM_PositionY = 535
local TPM_Pos = addPlaceholder("TPM_Pos_TEDAC", {0, -TPM_PositionY}, TPM_PH.name)
addVideoTPM(TPM_Pos.name)
