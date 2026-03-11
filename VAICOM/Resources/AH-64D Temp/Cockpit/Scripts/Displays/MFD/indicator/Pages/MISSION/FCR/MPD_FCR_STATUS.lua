dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/FCR/MPD_FCR_Defs.lua")

local value = 
{
	"FCR NOT INSTALLED",
	"FCR NOT POWERED",
	"FCR BIT IN PROGRESS",
	"RF HANDOVER",
	"DL TARGET DATA",
}

local position = {0, 77}
local margin = 30

for i = 1, #value do
	AddRoundCornersWindow("Status: "..value[i], position, tp_default.width * #value[i] + margin, tp_default.height + margin, value[i], tp_default, nil, nil, {{"DSPLS_FCR_AG_Status", i}}, "CenterCenter")
end