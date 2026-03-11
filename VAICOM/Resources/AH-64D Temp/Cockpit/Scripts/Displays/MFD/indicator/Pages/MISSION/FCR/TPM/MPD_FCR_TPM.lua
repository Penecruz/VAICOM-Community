dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/FCR/MPD_FCR_Defs.lua")

local TPM_PH = addPlaceholder("TPM PH", {0, 0}, {{"DSPLS_FCR_GTM_Root"}})

TotalTargetCountStatusWindow(TPM_PH.name)
DirectionWiperTPM(TPM_PH.name, "FCR_GTM_WiperColor")
WideScanTPM(TPM_PH.name) 
MediumScanTPM(TPM_PH.name)
TPM_PositionY = 610
local TPM_Pos = addPlaceholder("TPM_Pos_Pos", {0, -TPM_PositionY}, TPM_PH.name)
addVideoTPM(TPM_Pos.name)