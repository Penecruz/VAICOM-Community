dofile('Scripts/World/Radio/ModulationTypes.lua')
dofile('Scripts/World/Radio/FrequencyBands.lua')

local gettext = require("i_18n")
local       _ = gettext.translate

--WORLD RADIO -- Appended by VAICOM

radioTableFormat = 3
radio = {
	{
		radioId = 'airfield6_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("BoulderCity"), "BoulderCity"}}};
		frequency = {[UHF] = {MODULATIONTYPE_AM, 250100000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118050000.000000}};
		sceneObjects = {'t:4816956'};
	};
	{
		radioId = 'airfield1_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("IndianSprings"), "IndianSprings"}}};
		frequency = {[UHF] = {MODULATIONTYPE_AM, 360600000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118300000.000000}};
		sceneObjects = {'t:8060933'};
	};
	{
		radioId = 'airfield2_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Dreamland"), "Dreamland"}}};
		frequency = {[UHF] = {MODULATIONTYPE_AM, 250050000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118000000.000000}};
		sceneObjects = {'t:9666583'};
	};
	{
		radioId = 'airfield8_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Henderson"), "Henderson"}}};
		frequency = {[VHF_HI] = {MODULATIONTYPE_AM, 125100000.000000}};
		sceneObjects = {'t:4784157'};
	};
	{
		radioId = 'airfield3_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Lasvegas"), "Lasvegas"}}};
		frequency = {[UHF] = {MODULATIONTYPE_AM, 257800000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 119900000.000000}};
		sceneObjects = {'t:5802229'};
	};
	{
		radioId = 'airfield10_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Bullhead"), "Bullhead"}}};
		frequency = {[VHF_HI] = {MODULATIONTYPE_AM, 123900000.000000}};
		sceneObjects = {'t:32310933'};
	};
	{
		radioId = 'airfield4_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("NellisControl"), "NellisControl"}}};
		frequency = {[UHF] = {MODULATIONTYPE_AM, 327000000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 132550000.000000}};
		sceneObjects = {'t:7444723'};
	};
	{
		radioId = 'airfield15_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("NorthLasVegas"), "NorthLasVegas"}}};
		frequency = {[UHF] = {MODULATIONTYPE_AM, 360750000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 125700000.000000}};
		sceneObjects = {'t:7212047'};
	};
	{
		radioId = 'airfield18_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Silverbow"), "Silverbow"}}};
		frequency = {[UHF] = {MODULATIONTYPE_AM, 257950000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 124750000.000000}};
		sceneObjects = {'t:10715149'};
	};
}
