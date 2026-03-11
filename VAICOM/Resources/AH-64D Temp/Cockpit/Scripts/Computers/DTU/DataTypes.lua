-- CHAFFs, FLAREs

CF_BurstCount =
{
	_1 = 0,
	_2 = 1,
	_3 = 2,
	_4 = 3,
	_6 = 4,
	_8 = 5
}

CF_BurstInterval =
{
	_01 = 0,
	_02 = 1,
	_03 = 2,
	_04 = 3
}

CF_SalvoCount =
{
	_1			= 0,
	_2			= 1,
	_4			= 2,
	_8			= 3,
	CONTINUOUS	= 4
}

CF_SalvoInterval =
{
	_1		= 0,
	_2		= 1,
	_3		= 2,
	_4		= 3,
	_5		= 4,
	_8		= 5,
	RANDOM	= 6
}

CF_ProgramInterval =
{
	_1 = 0,
	_2 = 1,
	_3 = 2,
	_4 = 3
}

-- Tactical Points

Point_ID =
{
	BLANK	= 0,	-- NONE
	
	-- WP, HZ
	LZ		= 1,	-- Landing Zone
	CC		= 2,	-- Communication Checkpoint
	PP		= 3,	-- Passage Point
	RP		= 4,	-- Release Point
	SP		= 5,	-- Start Point
	WP		= 6,	-- Waypoint
	TO		= 7,	-- Tower Over 1000' AGL
	TU		= 8,	-- Tower Under 1000' AGL
	WL		= 9,	-- Wires - Power Lines
	WS		= 10,	-- Wires - Telephone
	
	-- CM
	AB		= 11,	-- Friendly Airborne
	AD		= 12,	-- Friendly Air Defense
	AH		= 13,	-- Friendly Attack Helicopter
	AM		= 14,	-- Friendly Armor
	AS		= 15,	-- Friendly Air Assault
	AV		= 16,	-- Friendly Air Cavalry
	CA		= 17,	-- Friendly Armored Cavalry
	CF		= 18,	-- Friendly Chemical
	DF		= 19,	-- Friendly Decontamination
	EN		= 20,	-- Friendly Engineer
	FG		= 21,	-- Friendly General Army Helicopter
	FI		= 22,	-- Friendly Infantry
	FL		= 23,	-- Friendly Field Artillery
	FU		= 24,	-- Friendly Unit ID	
	FW		= 25,	-- Friendly Electronic Warfare
	HO		= 26,	-- Friendly Hospital / Aid Station
	MA		= 27,	-- Friendly Aviation Maintenance
	MD		= 28,	-- Friendly Medical
	MI		= 29,	-- Friendly Mechanized Infantry
	TF		= 30,	-- Friendly Tactical Operations Center
	WF		= 31,	-- Friendly Fixed Wing
	CE		= 32,	-- Enemy Chemical
	DE		= 33,	-- Enemy Decontamination
	AE		= 34,	-- Enemy Armor
	EB		= 35,	-- Enemy Airborne
	EC		= 36,	-- Enemy Armored Cavalry
	ED		= 37,	-- Enemy Air Defense
	EE		= 38,	-- Enemy Engineer
	EF		= 39,	-- Enemy Field Artillery
	EH		= 40,	-- Enemy Hospital / Aid Station
	EI		= 41,	-- Enemy Infantry
	EK		= 42,	-- Enemy Attack Helicopter
	EM		= 43,	-- Enemy Mechanized Infantry
	ES		= 44,	-- Enemy Air Assault
	ET		= 45,	-- Enemy Tactical Operations Center
	EU		= 46,	-- Enemy Unit ID
	EV		= 47,	-- Enemy Air Cavalry
	EX		= 48,	-- Enemy Medical
	HG		= 49,	-- Enemy General Army Helicopter
	ME		= 50,	-- Enemy Aviation Maintenance
	WE		= 51,	-- Enemy Fixed Wing
	WR		= 52,	-- Enemy Electronic Warfare
	BR		= 53,	-- Bridge or Gap
	CP		= 54,	-- Checkpoint
	BE		= 55,	-- Nondirectional Beacon (NDB)
	RH		= 56,	-- Railhead-point
	AA		= 57,	-- Assembly Area
	AP		= 58,	-- Air Control Point
	BP		= 59,	-- Battle Position
	FA		= 60,	-- Forward Assembly Area
	HA		= 61,	-- Holding Area
	AG		= 62,	-- Airfield - General
	AI		= 63,	-- Airfield - Instrumented
	AL		= 64,	-- Lighted Airport
	GL		= 65,	-- Ground Lights / Small Town
	F1		= 66,	-- Artillery Fire Registration / Concentration Point - Part 1
	F2		= 67,	-- Artillery Fire Registration / Concentration Point - Part 2
	FC		= 68,	-- FARP - Fuel and Ammunition
	FF		= 69,	-- FARP - Fuel Only
	FM		= 70,	-- FARP - Ammunition Only
	ID		= 71,	-- IDM Subscriber
	BD		= 72,	-- Brigade
	BN		= 73,	-- Battalion
	CO		= 74,	-- Company
	CR		= 75,	-- CORPS
	DI		= 76,	-- Division
	GP		= 77,	-- Regiment / Group
	NB		= 78,	-- Nuclear, Biological and Chemical Contaminated Area
	US		= 79,	-- US Army

	-- TT
	TG		= 80,	-- Target ID
	GU		= 81,	-- Generic ADU
	SA1		= 82,	-- SA-1 ADU
	SA2		= 83,	-- SA-2 ADU
	SA3		= 84,	-- SA-3 ADU
	SA4		= 85,	-- SA-4 ADU
	SA5		= 86,	-- SA-5 ADU
	SA6		= 87,	-- SA-6 ADU
	SA7		= 88,	-- SA-7 ADU
	SA8		= 89,	-- SA-8 ADU
	SA9		= 90,	-- SA-9 ADU
	SA10	= 91,	-- SA-10 ADU
	SA11	= 92,	-- SA-11 ADU
	SA12	= 93,	-- SA-12 ADU
	SA13	= 94,	-- SA-13/19 ADU
	SA14	= 95,	-- SA-14 ADU
	SA15	= 96,	-- SA-15 ADU
	SA16	= 97,	-- SA-16 ADU
	SA17	= 98,	-- SA-17 ADU
	S6		= 99,	-- ADU
	ZU		= 100,	-- ZSU-23/4 ADU
	AS		= 101,	-- ASIPDE ADU
	M83		= 102,	-- M1983 ADU
	HK		= 103,	-- HAWK/IHAWK ADU
	RO		= 104,	-- ROLAND ADU
	AA		= 105,	-- AAA (> 57mm) ADU
	C2		= 106,	-- CSA-21/X ADU
	CT		= 107,	-- CROTALE ADU
	RA		= 108,	-- RAPIER ADU
	GT		= 109,	-- Towed Air Defense Gun (> 57mm)
	GS		= 110,	-- Self-Propelled Air Defense Gun (< 57mm)
	TR		= 111,	-- Target Acquisition Radar
	U		= 112,	-- Unknown ADU
	SA		= 113,	-- Towed Multi-vehicle SAM ADU
	SP		= 114,	-- Self-Propelled SAM ADU
	R70		= 115,	-- RBS-70 ADU
	SR		= 116,	-- Battlefield Surveillance Radar
	NV		= 117,	-- Naval ADU
	G1		= 118,	-- Growth 1 ADU
	G2		= 119,	-- Growth 2 ADU
	G3		= 120,	-- Growth 3 ADU
	G4		= 121,	-- Growth 4 ADU
	PT		= 122,	-- M1M-104 PATRIOT ADU
	ST		= 123,	-- STINGER or LAW-ADS ADU
	RE		= 124,	-- REDEYE ADU
	CH		= 125,	-- CHAPARRAL ADU
	TC		= 126,	-- TIGERCAT Towed Multi-vehicle SAM ADU
	SD		= 127,	-- SPADA Towed Multi-vehicle SAM ADU
	BH		= 128,	-- BLOODHOUND Towed Multi-vehicle SAM ADU
	SS		= 129,	-- SHORTS STARSTREAK ADU
	JA		= 130,	-- SHORTS JAVELIN ADU
	BP		= 131,	-- SHORTS BLOWPIPW ADU
	SM		= 132,	-- SAMP ADU
	SC		= 133,	-- SATCP ADU
	SH		= 134,	-- SHAHINE/R440 ADU
	GP		= 135,	-- GEPARD Towed ADG (< 57mm)
	VU		= 136,	-- VULCAN Towed ADG (< 57mm)
	MK		= 137,	-- Marconi MARKSMAN ADU
	SB		= 138,	-- SABRE ADU
	AX		= 139,	-- AMX-13 ADU
	AD		= 140,	-- Friendly ADU
	
	TRN		= 141,	-- For Terrain T55 T56
}



