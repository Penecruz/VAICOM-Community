dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/VIDEO/VIDEO_PAGE_Symbology.lua")

local VideoAreaPH = addPlaceholder("VideoAreaCenterPH", {0, 0, 0}, nil, nil)
addBobUpBox( "BobUpBox", {0, 0}, VideoAreaPH.name, {{"DSPLS_BOBUP_BobUpBox", (1-6.0/40.0)*PitchLadder_h/2 }})