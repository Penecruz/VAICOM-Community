Point_Types =
{
	"",					-- NONE
	"W%02d",			-- Waypoint
	"H%02d",			-- Hazard
	"C%02d",			-- Control Measure
	"T%02d",			-- Target
}

WP_HZ_CM_Types =
{
	"",					-- NONE
	"LZ%02d",			-- Landing Zone
	"CC%02d",			-- Communication Checkpoint
	"PP%02d",			-- Passage Point
	"RP%02d",			-- Release Point
	"SP%02d",			-- Start Point
	"WP%02d",			-- Waypoint
	"TO%02d",			-- Tower Over 1000' AGL
	"TU%02d",			-- Tower Under 1000' AGL
	"WL%02d",			-- Wires - Power Lines
	"WS%02d",			-- Wires - Telephone
	
	"AB%02d",			-- Friendly Airborne
	"AD%02d",			-- Friendly Air Defense
	"AH%02d",			-- Friendly Attack Helicopter
	"AM%02d",			-- Friendly Armor
	"AS%02d",			-- Friendly Air Assault
	"AV%02d",			-- Friendly Air Cavalry
	"CA%02d",			-- Friendly Armored Cavalry
	"CF%02d",			-- Friendly Chemical
	"DF%02d",			-- Friendly Decontamination
	"EN%02d",			-- Friendly Engineer
	"FG%02d",			-- Friendly General Army Helicopter
	"FI%02d",			-- Friendly Infantry
	"FL%02d",			-- Friendly Field Artillery
	"FU%02d",			-- Friendly Unit ID	
	"FW%02d",			-- Friendly Electronic Warfare
	"HO%02d",			-- Friendly Hospital / Aid Station
	"MA%02d",			-- Friendly Aviation Maintenance
	"MD%02d",			-- Friendly Medical
	"MI%02d",			-- Friendly Mechanized Infantry
	"TF%02d",			-- Friendly Tactical Operations Center
	"WF%02d",			-- Friendly Fixed Wing
	"CE%02d",			-- Enemy Chemical
	"DE%02d",			-- Enemy Decontamination
	"AE%02d",			-- Enemy Armor
	"EB%02d",			-- Enemy Airborne
	"EC%02d",			-- Enemy Armored Cavalry
	"ED%02d",			-- Enemy Air Defense
	"EE%02d",			-- Enemy Engineer
	"EF%02d",			-- Enemy Field Artillery
	"EH%02d",			-- Enemy Hospital / Aid Station
	"EI%02d",			-- Enemy Infantry
	"EK%02d",			-- Enemy Attack Helicopter
	"EM%02d",			-- Enemy Mechanized Infantry
	"ES%02d",			-- Enemy Air Assault
	"ET%02d",			-- Enemy Tactical Operations Center
	"EU%02d",			-- Enemy Unit ID
	"EV%02d",			-- Enemy Air Cavalry
	"EX%02d",			-- Enemy Medical
	"HG%02d",			-- Enemy General Army Helicopter
	"ME%02d",			-- Enemy Aviation Maintenance
	"WE%02d",			-- Enemy Fixed Wing
	"WR%02d",			-- Enemy Electronic Warfare
	"BR%02d",			-- Bridge or Gap
	"CP%02d",			-- Checkpoint
	"BE%02d",			-- Nondirectional Beacon (NDB)
	"RH%02d",			-- Railhead-point
	"AA%02d",			-- Assembly Area
	"AP%02d",			-- Air Control Point
	"BP%02d",			-- Battle Position
	"FA%02d",			-- Forward Assembly Area
	"HA%02d",			-- Holding Area
	"AG%02d",			-- Airfield - General
	"AI%02d",			-- Airfield - Instrumented
	"AL%02d",			-- Lighted Airport
	"GL%02d",			-- Ground Lights / Small Town
	"F1%02d",			-- Artillery Fire Registration / Concentration Point - Part 1
	"F2%02d",			-- Artillery Fire Registration / Concentration Point - Part 2
	"FC%02d",			-- FARP - Fuel and Ammunition
	"FF%02d",			-- FARP - Fuel Only
	"FM%02d",			-- FARP - Ammunition Only
	"ID%02d",			-- IDM Subscriber
	"BD%02d",			-- Brigade
	"BN%02d",			-- Battalion
	"CO%02d",			-- Company
	"CR%02d",			-- CORPS
	"DI%02d",			-- Division
	"GP%02d",			-- Regiment / Group
	"NB%02d",			-- Nuclear, Biological and Chemical Contaminated Area
	"US%02d",			-- US Army
}

WP_HZ_CM_TG_Identifier =
{
	"",				-- NONE
	"LZ",			-- Landing Zone
	"CC",			-- Communication Checkpoint
	"PP",			-- Passage Point
	"RP",			-- Release Point
	"SP",			-- Start Point
	"WP",			-- Waypoint
	"TO",			-- Tower Over 1000' AGL
	"TU",			-- Tower Under 1000' AGL
	"WL",			-- Wires - Power Lines
	"WS",			-- Wires - Telephone
	
	"AB",			-- Friendly Airborne
	"AD",			-- Friendly Air Defense
	"AH",			-- Friendly Attack Helicopter
	"AM",			-- Friendly Armor
	"AS",			-- Friendly Air Assault
	"AV",			-- Friendly Air Cavalry
	"CA",			-- Friendly Armored Cavalry
	"CF",			-- Friendly Chemical
	"DF",			-- Friendly Decontamination
	"EN",			-- Friendly Engineer
	"FG",			-- Friendly General Army Helicopter
	"FI",			-- Friendly Infantry
	"FL",			-- Friendly Field Artillery
	"FU",			-- Friendly Unit ID	
	"FW",			-- Friendly Electronic Warfare
	"HO",			-- Friendly Hospital / Aid Station
	"MA",			-- Friendly Aviation Maintenance
	"MD",			-- Friendly Medical
	"MI",			-- Friendly Mechanized Infantry
	"TF",			-- Friendly Tactical Operations Center
	"WF",			-- Friendly Fixed Wing
	"CE",			-- Enemy Chemical
	"DE",			-- Enemy Decontamination
	"AE",			-- Enemy Armor
	"EB",			-- Enemy Airborne
	"EC",			-- Enemy Armored Cavalry
	"ED",			-- Enemy Air Defense
	"EE",			-- Enemy Engineer
	"EF",			-- Enemy Field Artillery
	"EH",			-- Enemy Hospital / Aid Station
	"EI",			-- Enemy Infantry
	"EK",			-- Enemy Attack Helicopter
	"EM",			-- Enemy Mechanized Infantry
	"ES",			-- Enemy Air Assault
	"ET",			-- Enemy Tactical Operations Center
	"EU",			-- Enemy Unit ID
	"EV",			-- Enemy Air Cavalry
	"EX",			-- Enemy Medical
	"HG",			-- Enemy General Army Helicopter
	"ME",			-- Enemy Aviation Maintenance
	"WE",			-- Enemy Fixed Wing
	"WR",			-- Enemy Electronic Warfare
	"BR",			-- Bridge or Gap
	"CP",			-- Checkpoint
	"BE",			-- Nondirectional Beacon (NDB)
	"RH",			-- Railhead-point
	"AA",			-- Assembly Area
	"AP",			-- Air Control Point
	"BP",			-- Battle Position
	"FA",			-- Forward Assembly Area
	"HA",			-- Holding Area
	"AG",			-- Airfield - General
	"AI",			-- Airfield - Instrumented
	"AL",			-- Lighted Airport
	"GL",			-- Ground Lights / Small Town
	"F1",			-- Artillery Fire Registration / Concentration Point - Part 1
	"F2",			-- Artillery Fire Registration / Concentration Point - Part 2
	"FC",			-- FARP - Fuel and Ammunition
	"FF",			-- FARP - Fuel Only
	"FM",			-- FARP - Ammunition Only
	"ID",			-- IDM Subscriber
	"BD",			-- Brigade
	"BN",			-- Battalion
	"CO",			-- Company
	"CR",			-- CORPS
	"DI",			-- Division
	"GP",			-- Regiment / Group
	"NB",			-- Nuclear, Biological and Chemical Contaminated Area
	"US",			-- US Army
	
	"TG",			-- Target ID
	"GU",			-- Generic ADU
	"1",			-- SA-1 ADU
	"2",			-- SA-2 ADU
	"3",			-- SA-3 ADU
	"4",			-- SA-4 ADU
	"5",			-- SA-5 ADU
	"6",			-- SA-6 ADU
	"7",			-- SA-7 ADU
	"8",			-- SA-8 ADU
	"9",			-- SA-9 ADU
	"10",			-- SA-10 ADU
	"11",			-- SA-11 ADU
	"12",			-- SA-12 ADU
	"13",			-- SA-13/19 ADU
	"14",			-- SA-14 ADU
	"15",			-- SA-15 ADU
	"16",			-- SA-16 ADU
	"17",			-- SA-17 ADU
	"S6",			-- ADU
	"ZU",			-- ZSU-23/4 ADU
	"AS",			-- ASIPDE ADU
	"83",			-- M1983 ADU
	"HK",			-- HAWK/IHAWK ADU
	"RO",			-- ROLAND ADU
	"AA",			-- AAA (> 57mm) ADU
	"C2",			-- CSA-21/X ADU
	"CT",			-- CROTALE ADU
	"RA",			-- RAPIER ADU
	"GT",			-- Towed Air Defense Gun (> 57mm)
	"GS",			-- Self-Propelled Air Defense Gun (< 57mm)
	"TR",			-- Target Acquisition Radar
	"U",			-- Unknown ADU
	"SA",			-- Towed Multi-vehicle SAM ADU
	"SP",			-- Self-Propelled SAM ADU
	"70",			-- RBS-70 ADU
	"SR",			-- Battlefield Surveillance Radar
	"NV",			-- Naval ADU
	"G1",			-- Growth 1 ADU
	"G2",			-- Growth 2 ADU
	"G3",			-- Growth 3 ADU
	"G4",			-- Growth 4 ADU
	"PT",			-- M1M-104 PATRIOT ADU
	"ST",			-- STINGER or LAW-ADS ADU
	"RE",			-- REDEYE ADU
	"CH",			-- CHAPARRAL ADU
	"TC",			-- TIGERCAT Towed Multi-vehicle SAM ADU
	"SD",			-- SPADA Towed Multi-vehicle SAM ADU
	"BH",			-- BLOODHOUND Towed Multi-vehicle SAM ADU
	"SS",			-- SHORTS STARSTREAK ADU
	"JA",			-- SHORTS JAVELIN ADU
	"BP",			-- SHORTS BLOWPIPW ADU
	"SM",			-- SAMP ADU
	"SC",			-- SATCP ADU
	"SH",			-- SHAHINE/R440 ADU
	"GP",			-- GEPARD Towed ADG (< 57mm)
	"VU",			-- VULCAN Towed ADG (< 57mm)
	"MK",			-- Marconi MARKSMAN ADU
	"SB",			-- SABRE ADU
	"AX",			-- AMX-13 ADU
	"AD",			-- Friendly ADU
	"TG",			-- For Terrain T55 T56 TODO:
}

Spheroid =
{
	"",		--0
	"CL0",	--1
	"CL0",	--2
	"AUS",	--3
	"BES",	--4
	"INT",	--5
	"BES",	--6
	"INT",	--7
	"INT",	--8
	"CL0",	--9
	"CL6",	--10
	"BES",	--11
	"W84",	--12
	"INT",	--13
	"INT",	--14
	"INT",	--15
	"EVE",	--16
	"MAI",	--17
	"MEV",	--18
	"CL0",	--19
	"W84",	--20
	"CL6",	--21
	"CL0",	--22
	"W84",	--23
	"CL0",	--24
	"CL6",	--25
	"CL6",	--26
	"CL6",	--27
	"CL6",	--28
	"CL6",	--29
	"AIR",	--30
	"INT",	--31
	"CL0",	--32
	"INT",	--33
	"INT",	--34
	"INT",	--35
	"INT",	--36
	"INT",	--37
	"INT",	--38
	"EVB",	--39
	"BES",	--40
	"CL0",	--41
	"EVE",	--42
	"CL6",	--43
	"BES",	--44
	"W84",	--45
	"W72",	--46
	"W84",	--47
}

