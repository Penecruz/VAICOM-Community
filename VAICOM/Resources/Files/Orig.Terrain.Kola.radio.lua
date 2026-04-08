dofile('Scripts/World/Radio/ModulationTypes.lua')
dofile('Scripts/World/Radio/FrequencyBands.lua')

local gettext = require("i_18n")
local       _ = gettext.translate

--WORLD RADIO

radioTableFormat = 3
radio = {
	{
		-- BAS100
		radioId = 'airfield4_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("BAS100"), "BAS100"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4300000.000000}, [UHF] = {MODULATIONTYPE_AM, 257100000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118400000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39500000.000000}};
		sceneObjects = {'t:6193155'};
	};
	{
		-- EFET_Enontekio
		radioId = 'airfield26_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Enontekio"), "Enontekio"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4200000.000000}, [UHF] = {MODULATIONTYPE_AM, 250850000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 119700000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39300000.000000}};
		sceneObjects = {'t:25367337'};
	};
	{
		-- EFIV_Ivalo
		radioId = 'airfield18_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Ivalo"), "Ivalo"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4000000.000000}, [UHF] = {MODULATIONTYPE_AM, 250450000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118000000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38900000.000000}};
		sceneObjects = {'t:26068218'};
	};
	{
		-- EFKE_Kemi_Tornio
		radioId = 'airfield3_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("KemiTornio"), "KemiTornio"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4275000.000000}, [UHF] = {MODULATIONTYPE_AM, 251000000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 119400000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39450000.000000}};
		sceneObjects = {'t:17924493'};
	};
	{
		-- EFKS_Kuusamo
		radioId = 'airfield16_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Kuusamo"), "Kuusamo"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 3950000.000000}, [UHF] = {MODULATIONTYPE_AM, 250350000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118650000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38800000.000000}};
		sceneObjects = {'t:19284702'};
	};
	{
		-- EFKT_Kittila
		radioId = 'airfield22_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Kittila"), "Kittila"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4125000.000000}, [UHF] = {MODULATIONTYPE_AM, 250700000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118950000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39150000.000000}};
		sceneObjects = {'t:1417217'};
	};
	{
		-- EFRO_Rovaniemi
		radioId = 'airfield2_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Rovaniemi"), "Rovaniemi"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4250000.000000}, [UHF] = {MODULATIONTYPE_AM, 250950000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118700000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39400000.000000}};
		sceneObjects = {'t:20760144'};
	};
	{
		-- EFSO_Sodankyla
		radioId = 'airfield27_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Sodankyla"), "Sodankyla"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4225000.000000}, [UHF] = {MODULATIONTYPE_AM, 250900000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 123400000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39350000.000000}};
		sceneObjects = {'t:22807177'};
	};
	{
		-- ENAN Andoya
		radioId = 'airfield20_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Andoya"), "Andoya"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4075000.000000}, [UHF] = {MODULATIONTYPE_AM, 250600000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118200000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39050000.000000}};
		sceneObjects = {'t:28272658'};
	};
	{
		-- ENAT_Alta
		radioId = 'airfield24_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Alta"), "Alta"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4150000.000000}, [UHF] = {MODULATIONTYPE_AM, 250750000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 120400000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39200000.000000}};
		sceneObjects = {'t:30270151'};
	};
	{
		-- ENBO_Bodo
		radioId = 'airfield7_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Bodo"), "Bodo"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4375000.000000}, [UHF] = {MODULATIONTYPE_AM, 251150000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118450000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39650000.000000}};
		sceneObjects = {'t:16384'};
	};
	{
		-- ENDU_Bardufoss
		radioId = 'airfield21_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Bardufoss"), "Bardufoss"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4100000.000000}, [UHF] = {MODULATIONTYPE_AM, 250650000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118100000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39100000.000000}};
		sceneObjects = {'t:26849081'};
	};
	{
		-- ENEV_Evenes
		radioId = 'airfield25_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Evenes"), "Evenes"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4175000.000000}, [UHF] = {MODULATIONTYPE_AM, 250800000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118000000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39250000.000000}};
		sceneObjects = {'t:7971487'};
	};
	{
		-- ENKR_Kirkenes
		radioId = 'airfield14_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Kirkenes"), "Kirkenes"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 3900000.000000}, [UHF] = {MODULATIONTYPE_AM, 250250000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 120350000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38700000.000000}};
		sceneObjects = {'t:30223511'};
	};
	{
		-- ENNA_Lakselv
		radioId = 'airfield1_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Lakselv"), "Lakselv"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4050000.000000}, [UHF] = {MODULATIONTYPE_AM, 250550000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118050000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39000000.000000}};
		sceneObjects = {'t:10289788'};
	};
	{
		-- ESNJ_Jokkmokk
		radioId = 'airfield11_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Jokkmokk"), "Jokkmokk"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 3825000.000000}, [UHF] = {MODULATIONTYPE_AM, 250150000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 123300000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38550000.000000}};
		sceneObjects = {'t:4882432'};
	};
	{
		-- ESNQ Kiruna
		radioId = 'airfield5_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Kiruna"), "Kiruna"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4325000.000000}, [UHF] = {MODULATIONTYPE_AM, 251050000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 130150000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39550000.000000}};
		sceneObjects = {'t:1376256'};
	};
	{
		-- ESPA_Kallax
		radioId = 'airfield15_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Kallax"), "Kallax"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 3925000.000000}, [UHF] = {MODULATIONTYPE_AM, 250300000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 128200000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38750000.000000}};
		sceneObjects = {'t:13558614'};
	};
	{
		-- ESPE_Vidsel
		radioId = 'airfield17_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Vidsel"), "Vidsel"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 3975000.000000}, [UHF] = {MODULATIONTYPE_AM, 250400000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 119200000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38850000.000000}};
		sceneObjects = {'t:17089891'};
	};
	{
		-- ESPG_Boden_Heli_Base
		radioId = 'airfield34_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Boden"), "Boden"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4475000.000000}, [UHF] = {MODULATIONTYPE_AM, 252250000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118750000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 40150000.000000}};
		sceneObjects = {'t:3792975'};
	};
	{
		-- ESUK Kalixfors
		radioId = 'airfield13_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Kalixfors"), "Kalixfors"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 3875000.000000}, [UHF] = {MODULATIONTYPE_AM, 301100000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118300000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38650000.000000}};
		sceneObjects = {'t:225771572'};
	};
	{
		-- ESUT_Hemavan
		radioId = 'airfield35_0';
		role = {"ground", "tower", "approach"};
		callsign = {};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4625000.000000}, [UHF] = {MODULATIONTYPE_AM, 251500000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 122900000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39950000.000000}};
		sceneObjects = {'t:18080414'};
	};
	{
		-- RU-0059_Alakurtti
		radioId = 'airfield19_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Alakourtti"), "Alakourtti"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4025000.000000}, [UHF] = {MODULATIONTYPE_AM, 250500000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118350000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38950000.000000}};
		sceneObjects = {'t:22143542'};
	};
	{
		-- ULAK_Severomorsk1
		radioId = 'airfield8_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Severomorsk-1"), "Severomorsk-1"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4400000.000000}, [UHF] = {MODULATIONTYPE_AM, 251200000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 127800000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39700000.000000}};
		sceneObjects = {'t:28930577'};
	};
	{
		-- ULMM_Emperor_Nicholas_II
		radioId = 'airfield12_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Murmansk"), "Murmansk"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 3850000.000000}, [UHF] = {MODULATIONTYPE_AM, 250200000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 127300000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38600000.000000}};
		sceneObjects = {'t:1982491'};
	};
	{
		-- ULPK_Kalevala
		radioId = 'airfield32_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Kalevala"), "Kalevala"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 3775000.000000}, [UHF] = {MODULATIONTYPE_AM, 301000000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 119300000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38400000.000000}};
		sceneObjects = {'t:32270582'};
	};
	{
		-- UMOE_Monchegorsk
		radioId = 'airfield10_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Monchegorsk"), "Monchegorsk"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 3800000.000000}, [UHF] = {MODULATIONTYPE_AM, 250100000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 118250000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38500000.000000}};
		sceneObjects = {'t:7881288'};
	};
	{
		-- UOLE_Olenegorsk
		radioId = 'airfield9_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Olenya"), "Olenya"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4425000.000000}, [UHF] = {MODULATIONTYPE_AM, 251250000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 131400000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39750000.000000}};
		sceneObjects = {'t:25960789'};
	};
	{
		-- XLMF_Afrikanda
		radioId = 'airfield33_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Afrikanda"), "Afrikanda"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4775000.000000}, [UHF] = {MODULATIONTYPE_AM, 311000000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 121300000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39900000.000000}};
		sceneObjects = {'t:24454596'};
	};
	{
		-- XLML_Luostari_Pechenga
		radioId = 'airfield29_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Luostari Pechenga"), "Luostari Pechenga"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4450000.000000}, [UHF] = {MODULATIONTYPE_AM, 281300000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 119350000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 41200000.000000}};
		sceneObjects = {'t:9814017'};
	};
	{
		-- XLMV_Severomorsk3
		radioId = 'airfield6_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Severomorsk-3"), "Severomorsk-3"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4350000.000000}, [UHF] = {MODULATIONTYPE_AM, 251100000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 124300000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39600000.000000}};
		sceneObjects = {'t:28592719'};
	};
	{
		-- XLMW_Kilpyavr
		radioId = 'airfield28_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Kilpyavr"), "Kilpyavr"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4250000.000000}, [UHF] = {MODULATIONTYPE_AM, 250950000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 119100000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 39400000.000000}};
		sceneObjects = {'t:28730064'};
	};
	{
		-- XLMY_Koshka_Yavr
		radioId = 'airfield30_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Koshka Yavr"), "Koshka Yavr"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 4850000.000000}, [UHF] = {MODULATIONTYPE_AM, 225500000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 123800000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38450000.000000}};
		sceneObjects = {'t:9557446'};
	};
	{
		-- XLPU_Poduzhemye
		radioId = 'airfield31_0';
		role = {"ground", "tower", "approach"};
		callsign = {{["common"] = {_("Poduzhemye"), "Poduzhemye"}}};
		frequency = {[HF] = {MODULATIONTYPE_AM, 3925000.000000}, [UHF] = {MODULATIONTYPE_AM, 250550000.000000}, [VHF_HI] = {MODULATIONTYPE_AM, 120100000.000000}, [VHF_LOW] = {MODULATIONTYPE_AM, 38450000.000000}};
		sceneObjects = {'t:3017360'};
	};
}
