dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/AC/MPD_ENG_Symbology.lua")

addText( "AC ENG PAGE",  {0, 100})

local AIRCRAFT_ENG = 10
local AIRCRAFT_FLT = 11
local AIRCRAFT_FUEL = 12
local AIRCRAFT_PERF = 13
local AIRCRAFT_UTIL = 14

local Menu = {}
Menu = 
{ 
	{ pb.T1, "ENG",		tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_ENG}}},
	{ pb.T2, "FLT",		tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_FLT}}},
	{ pb.T3, "FUEL",	tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_FUEL}}},
	{ pb.T4, "PERF",	tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_PERF}}},
	{ pb.T6, "UTIL",	tp_default_border, {{"MFD_MenuPrevBorder", AIRCRAFT_UTIL}}},
}

createMenu( Menu )