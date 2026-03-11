dofile(LockOn_Options.script_path.."Computers/DTU/DataTypes.lua")

need_to_be_closed = true -- close lua state after initialization

-- FLAREs

FLARE =
{
	FlareBurstCount 	= CF_BurstCount._2,
	FlareBurstInterval	= CF_BurstInterval._04,
	FlareSalvoCount		= CF_SalvoCount._1,
	FlareSalvoInterval	= CF_SalvoInterval.RANDOM,
	FlareProgramDelay	= CF_ProgramInterval._1
}

-- ADF presets

ADF =
{
	{Name = "PR1",	Frequency = 430000},		-- [Hz]
	{Name = "PR2",	Frequency = 490000},		-- [Hz]
	{Name = "PR0",	Frequency = 923000},		-- [Hz]
}


-- Preplaned Tactical Points

Preplaned_CM =
{
	{Type = Point_ID.CF,		Position = {-358085.0, 0.0, 618653.0},	FreeText = "FUK"},
	{Type = Point_ID.FA,		Position = {-358080.0, 0.0, 618660.0},	FreeText = "ABC"},
}


-- Threat Rings

local function getTTID(PointID)
	return PointID - Point_ID.US	-- see DataTypes.lua
end

-- (*)	-	see https://jira.eagle.ru/browse/AH64D-1144
ThreatRings =
{
	{Type = getTTID(Point_ID.TG),		Finding = 0.0,					Killing = 3000.0},		-- (*)
	{Type = getTTID(Point_ID.GU),		Finding = 0.0,					Killing = 5000.0},		-- (*)
	{Type = getTTID(Point_ID.SA1),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.SA2),		Finding = 100000.0,				Killing = 43000.0},		-- \trunk\LockOnExe\CoreMods\tech\TechWeaponPack\Database\vehicles\SAM\Radar\S75_TR.lua	/	(*)
	{Type = getTTID(Point_ID.SA3),		Finding = 100000.0,				Killing = 18000.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\radar\SNR S-125 TR.lua	/	(*)
	{Type = getTTID(Point_ID.SA4),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.SA5),		Finding = 99900.0,				Killing = 99900.0},		-- \trunk\LockOnExe\CoreMods\tech\TechWeaponPack\Database\vehicles\SAM\Radar\S200_RPC_5N62V.lua	/	(*)
	{Type = getTTID(Point_ID.SA6),		Finding = 70000.0,				Killing = 25000.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\radar\1S91 KUB STR.lua	/	(*)
	{Type = getTTID(Point_ID.SA7),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.SA8),		Finding = 30000.0,				Killing = 10300.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\9A33 OSA LN.lua

	{Type = getTTID(Point_ID.SA9),		Finding = 4200.0,				Killing = 5000.0},		-- (*)
	{Type = getTTID(Point_ID.SA10),		Finding = 99900.0,				Killing = 99900.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\radar\40B6M S-300PS TR.lua	/	(*)
	{Type = getTTID(Point_ID.SA11),		Finding = 50000.0,				Killing = 35000.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\9A310M1 BUK LN.lua
	{Type = getTTID(Point_ID.SA12),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.SA13),		Finding = 8000.0,				Killing = 5000.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\9A35 STRELA-10.lua
	{Type = getTTID(Point_ID.SA14),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.SA15),		Finding = 25000.0,				Killing = 12000.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\9A331 TOR.lua
	{Type = getTTID(Point_ID.SA16),		Finding = 5200.0,				Killing = 5200.0},		-- (*)
	{Type = getTTID(Point_ID.SA17),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.S6),		Finding = 18000.0,				Killing = 8000.0},		-- (*)

	{Type = getTTID(Point_ID.ZU),		Finding = 5000.0,				Killing = 2500.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\ZSU-23-4 Shilka.lua
	{Type = getTTID(Point_ID.AS),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.M83),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.HK),		Finding = 90000.0,				Killing = 45000.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\radar\AN-MPQ-48 HAWK TR.lua	/	(*)
	{Type = getTTID(Point_ID.RO),		Finding = 12000.0,				Killing = 8000.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\Roland ADS.lua
	{Type = getTTID(Point_ID.AA),		Finding = 55000.0,				Killing = 6000.0},		-- (*)
	{Type = getTTID(Point_ID.C2),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.CT),		Finding = 30000.0,				Killing = 15000.0},		-- (*)
	{Type = getTTID(Point_ID.RA),		Finding = 30000.0,				Killing = 6800.0},		-- \trunk\LockOnExe\CoreMods\tech\TechWeaponPack\Database\vehicles\SAM\Rapier_FSA_Launcher.lua
	{Type = getTTID(Point_ID.GT),		Finding = 5000.0,				Killing = 2500.0},		-- (*)

	{Type = getTTID(Point_ID.GS),		Finding = 5000.0,				Killing = 7000.0},		-- (*)
	{Type = getTTID(Point_ID.TR),		Finding = 35000.0,				Killing = 0.0},			-- (*)
	{Type = getTTID(Point_ID.U),		Finding = 5000.0,				Killing = 2500.0},		-- (*)
	{Type = getTTID(Point_ID.SA),		Finding = 50000.0,				Killing = 15000.0},		-- (*)
	{Type = getTTID(Point_ID.SP),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.R70),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.SR),		Finding = 99900.0,				Killing = 0.0},			-- (*)
	{Type = getTTID(Point_ID.NV),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.G1),		Finding = 0.0,					Killing = 1000.0},		-- (*)
	{Type = getTTID(Point_ID.G2),		Finding = 0.0,					Killing = 2000.0},		-- (*)

	{Type = getTTID(Point_ID.G3),		Finding = 0.0,					Killing = 3000.0},		-- (*)
	{Type = getTTID(Point_ID.G4),		Finding = 0.0,					Killing = 4000.0},		-- (*)
	{Type = getTTID(Point_ID.PT),		Finding = 99900.0,				Killing = 99900.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\radar\AN-MPQ-53 PATRIOT STR.lua	/	(*)
	{Type = getTTID(Point_ID.ST),		Finding = 5200.0,				Killing = 4500.0},		-- (*)
	{Type = getTTID(Point_ID.RE),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.CH),		Finding = 10000.0,				Killing = 8500.0},		-- (*)
	{Type = getTTID(Point_ID.TC),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.SD),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.BH),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.SS),		Finding = 0.0,					Killing = 0.0},

	{Type = getTTID(Point_ID.JA),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.BP),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.SM),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.SC),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.SH),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.GP),		Finding = 15000.0,				Killing = 4000.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\Gepard.lua
	{Type = getTTID(Point_ID.VU),		Finding = 5000.0,				Killing = 2000.0},		-- \trunk\LockOnExe\Scripts\Database\vehicles\SAM\M163 Vulcan.lua
	{Type = getTTID(Point_ID.MK),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.SB),		Finding = 0.0,					Killing = 0.0},
	{Type = getTTID(Point_ID.AX),		Finding = 0.0,					Killing = 0.0},
	
	{Type = getTTID(Point_ID.AD),		Finding = 0.0,					Killing = 8000.0},		-- (*)
	{Type = getTTID(Point_ID.TRN),		Finding = 8000.0,				Killing = 0.0},
}

