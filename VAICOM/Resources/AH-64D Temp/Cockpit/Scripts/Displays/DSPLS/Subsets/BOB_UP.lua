dofile(LockOn_Options.script_path.."Displays/DSPLS/DSPLS_Symbology.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0, 0}, nil, {{"VideoAreaPH"}, {"VideoSignal_Show"}})


addBobUpBox("BobUpBox", {0, 0}, VideoAreaPH.name, {{"DSPLS_BOBUP_BobUpBox", (1-6.0/40.0)*PitchLadder_h/2 }})