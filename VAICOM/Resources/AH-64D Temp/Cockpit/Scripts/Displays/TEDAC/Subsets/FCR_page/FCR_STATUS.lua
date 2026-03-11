dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local value = 
{
	"FCR NOT INSTALLED",
	"FCR NOT POWERED",
	"FCR BIT IN PROGRESS",
	"RF HANDOVER",
	"DL TARGET DATA",
}

local position = {0, 100}
local margin = 30
local tp = tp_24

for i = 1, #value do
	AddRoundCornersWindow("Status: "..value[i], position, tp.width * #value[i] + margin, tp.height + margin, value[i], tp, nil, nil,{{"DSPLS_FCR_AG_Status", i}}, "CenterCenter")
end
