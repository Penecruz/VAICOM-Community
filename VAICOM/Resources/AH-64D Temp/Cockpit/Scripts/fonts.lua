dofile(LockOn_Options.common_script_path.."Fonts/symbols_locale.lua")
dofile(LockOn_Options.common_script_path.."Fonts/fonts_cmn.lua")
dofile(LockOn_Options.common_script_path.."tools.lua")

local ResourcesPath = LockOn_Options.script_path.."../IndicationResources/"

fontdescription = {}



local mfd_char_w = 43
local mfd_char_w_I = 36 
local mfd_char_h = 64
local mfd_char_w_narrow = 20
local mfd_char_w_middle = 30
local mfd_char_w_dash = 64
local mfd_char_w_dot = 36
mfd_font_aspect = mfd_char_w/mfd_char_h;
local font_MFD_chars = {
	[1]   = {latin['A'], mfd_char_w, mfd_char_h},
	[2]   = {latin['B'], mfd_char_w, mfd_char_h},
	[3]   = {latin['C'], mfd_char_w, mfd_char_h},
	[4]   = {latin['D'], mfd_char_w, mfd_char_h},
	[5]   = {latin['E'], mfd_char_w, mfd_char_h},
	[6]   = {latin['F'], mfd_char_w, mfd_char_h},
	[7]   = {latin['G'], mfd_char_w, mfd_char_h},
	[8]   = {latin['H'], mfd_char_w, mfd_char_h},
	[9]   = {latin['I'], mfd_char_w_I, mfd_char_h},
	[10]  = {latin['J'], mfd_char_w, mfd_char_h},
	[11]  = {latin['K'], mfd_char_w, mfd_char_h},
	[12]  = {latin['L'], mfd_char_w, mfd_char_h},
	[13]  = {latin['M'], mfd_char_w, mfd_char_h},
	[14]  = {latin['N'], mfd_char_w, mfd_char_h},
	[15]  = {latin['O'], mfd_char_w, mfd_char_h},
	[16]  = {latin['P'], mfd_char_w, mfd_char_h},
	[17]  = {latin['Q'], mfd_char_w, mfd_char_h},
	[18]  = {latin['R'], mfd_char_w, mfd_char_h},
	[19]  = {latin['S'], mfd_char_w, mfd_char_h},
	[20]  = {latin['T'], mfd_char_w, mfd_char_h},
	[21]  = {latin['U'], mfd_char_w, mfd_char_h},
	[22]  = {latin['V'], mfd_char_w, mfd_char_h},
	[23]  = {latin['W'], mfd_char_w, mfd_char_h},
	[24]  = {latin['X'], mfd_char_w, mfd_char_h},
	[25]  = {latin['Y'], mfd_char_w, mfd_char_h},
	[26]  = {latin['Z'], mfd_char_w, mfd_char_h},
	
	[27]  = {symbol['0'], mfd_char_w, mfd_char_h},
	[28]  = {symbol['1'], mfd_char_w, mfd_char_h},
	[29]  = {symbol['2'], mfd_char_w, mfd_char_h},
	[30]  = {symbol['3'], mfd_char_w, mfd_char_h},
	[31]  = {symbol['4'], mfd_char_w, mfd_char_h},
	[32]  = {symbol['5'], mfd_char_w, mfd_char_h},
	[33]  = {symbol['6'], mfd_char_w, mfd_char_h},
	[34]  = {symbol['7'], mfd_char_w, mfd_char_h},
	[35]  = {symbol['8'], mfd_char_w, mfd_char_h},
	[36]  = {symbol['9'], mfd_char_w, mfd_char_h},
	[37]  = {symbol['-'], mfd_char_w, mfd_char_h},
	[38]  = {symbol['&'], mfd_char_w_middle, mfd_char_h}, -- nota
	[39]  = {symbol['>'], mfd_char_w, mfd_char_h},
	[40]  = {symbol['/'], mfd_char_w, mfd_char_h},
	[41]  = {symbol[','], mfd_char_w, mfd_char_h},
	[42]  = {symbol['{'], mfd_char_w, mfd_char_h}, --7  •

	[43]  = {symbol['}'], mfd_char_w, mfd_char_h}, -- 9 ○ 
	[44]  = {symbol['^'], mfd_char_w, mfd_char_h}, -- degree C wide
	[45]  = {symbol['%'], mfd_char_w, mfd_char_h},
	[46]  = {symbol['?'], mfd_char_w, mfd_char_h},
	[47]  = {symbol['*'], mfd_char_w, mfd_char_h},
	[48]  = {symbol['='], mfd_char_w, mfd_char_h},
	[49]  = {symbol['['], mfd_char_w_dot, mfd_char_h}, -- dot for morze!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	[50]  = {symbol['#'], mfd_char_w, mfd_char_h}, -- 
	[51]  = {symbol['.'], mfd_char_w_narrow, mfd_char_h}, 	--point narrow
	[52]  = {symbol['^'], mfd_char_w_middle, mfd_char_h}, 	--degree C narrow
	[53]  = {symbol[':'], mfd_char_w_narrow, mfd_char_h},
	[54]  = {symbol['$'], mfd_char_w_narrow, mfd_char_h},	-- comma narrow
	[55]  = {symbol[']'], mfd_char_w_dash, mfd_char_h},		-- dash for morze!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	[56]  = {symbol['+'], mfd_char_w_dash, mfd_char_h},	
	[57]  = {latin['a'],  mfd_char_w_dash, mfd_char_h},	
	[58]  = {latin['b'],  mfd_char_w_dash, mfd_char_h},	
	[59]  = {latin['c'],  mfd_char_w_dash, mfd_char_h},	-- <-
	[60]  = {latin['d'],  mfd_char_w_dash, mfd_char_h},	-- ->
	[61]  = {latin['e'],  mfd_char_w, mfd_char_h},		-- three points
	[62]  = {latin['f'],  mfd_char_w, mfd_char_h},		-- -> for COMM page
	[63]  = {latin['g'],  mfd_char_w, mfd_char_h},		-- <- for COMM page
	[64]  = {symbol[' '], mfd_char_w, mfd_char_h},

}

fontdescription["font_MFD"] = {
	texture			= ResourcesPath.."Displays/MPD/FontMPD_64.tga",
	size			= {8, 8},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {mfd_char_w, mfd_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_MFD_chars
}

fontdescription["font_MFD_inv"] = {
	texture			= ResourcesPath.."Displays/MPD/FontMPD_64_inv.tga",
	size			= {8, 8},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {mfd_char_w, mfd_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_MFD_chars
}
fontdescription["font_MFD_bold"] = {
	texture			= ResourcesPath.."Displays/MPD/FontMPD_64_bold.tga",
	size			= {8, 8},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {mfd_char_w, mfd_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_MFD_chars
}

fontdescription["font_MFD_inv_bold"] = {
	texture			= ResourcesPath.."Displays/MPD/FontMPD_64_inv_bold.tga",
	size			= {8, 8},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {mfd_char_w, mfd_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_MFD_chars
}

local ku_char_w = 43
local ku_char_h = 60
local font_KU_chars = {
	[1]   = {latin['A'], ku_char_w, ku_char_h},
	[2]   = {latin['B'], ku_char_w, ku_char_h},
	[3]   = {latin['C'], ku_char_w, ku_char_h},
	[4]   = {latin['D'], ku_char_w, ku_char_h},
	[5]   = {latin['E'], ku_char_w, ku_char_h},
	[6]   = {latin['F'], ku_char_w, ku_char_h},
	[7]   = {latin['G'], ku_char_w, ku_char_h},
	[8]   = {latin['H'], ku_char_w, ku_char_h},
	[9]   = {latin['I'], ku_char_w, ku_char_h},
	[10]  = {latin['J'], ku_char_w, ku_char_h},
	[11]  = {latin['K'], ku_char_w, ku_char_h},
	[12]  = {latin['L'], ku_char_w, ku_char_h},
	[13]  = {latin['M'], ku_char_w, ku_char_h},
	[14]  = {latin['N'], ku_char_w, ku_char_h},
	[15]  = {latin['O'], ku_char_w, ku_char_h},
	[16]  = {latin['P'], ku_char_w, ku_char_h},
	[17]  = {latin['Q'], ku_char_w, ku_char_h},
	[18]  = {latin['R'], ku_char_w, ku_char_h},
	[19]  = {latin['S'], ku_char_w, ku_char_h},
	[20]  = {latin['T'], ku_char_w, ku_char_h},
	[21]  = {latin['U'], ku_char_w, ku_char_h},
	[22]  = {latin['V'], ku_char_w, ku_char_h},
	[23]  = {latin['W'], ku_char_w, ku_char_h},
	[24]  = {latin['X'], ku_char_w, ku_char_h},
	[25]  = {latin['Y'], ku_char_w, ku_char_h},
	[26]  = {latin['Z'], ku_char_w, ku_char_h},

	[27]  = {symbol['0'], ku_char_w, ku_char_h},
	[28]  = {symbol['1'], ku_char_w, ku_char_h},
	[29]  = {symbol['2'], ku_char_w, ku_char_h},
	[30]  = {symbol['3'], ku_char_w, ku_char_h},
	[31]  = {symbol['4'], ku_char_w, ku_char_h},
	[32]  = {symbol['5'], ku_char_w, ku_char_h},
	[33]  = {symbol['6'], ku_char_w, ku_char_h},
	[34]  = {symbol['7'], ku_char_w, ku_char_h},
	[35]  = {symbol['8'], ku_char_w, ku_char_h},
	[36]  = {symbol['9'], ku_char_w, ku_char_h},

	[37]  = {symbol['+'], ku_char_w, ku_char_h},
	[38]  = {symbol['-'], ku_char_w, ku_char_h},
	[39]  = {symbol['*'], ku_char_w, ku_char_h},
	[40]  = {symbol['/'], ku_char_w, ku_char_h},
	[41]  = {symbol['.'], ku_char_w, ku_char_h},
	[42]  = {symbol[':'], ku_char_w, ku_char_h},

	[43]  = {symbol['#'], ku_char_w, ku_char_h}, -- cursor
	[44]  = {symbol[' '], ku_char_w, ku_char_h}, -- space

}

fontdescription["font_KU"] = {
	texture			= ResourcesPath.."Displays/KU_font_8p.dds",
	size			= {8, 8},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {ku_char_w, ku_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_KU_chars
}
--[[	-- no need for now
fontdescription["font_KU_inv"] = {
	texture			= ResourcesPath.."Displays/KU_font_7pix_inverse.dds",
	size			= {8, 8},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {ku_char_w, ku_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_KU_chars
}
]]



local EUFD_char_w = 44
local EUFD_char_wide = 58
local EUFD_char_h = 60
local font_EUFD_chars = {
	[1]   = {latin['A'], EUFD_char_w, EUFD_char_h},
	[2]   = {latin['B'], EUFD_char_w, EUFD_char_h},
	[3]   = {latin['C'], EUFD_char_w, EUFD_char_h},
	[4]   = {latin['D'], EUFD_char_w, EUFD_char_h},
	[5]   = {latin['E'], EUFD_char_w, EUFD_char_h},
	[6]   = {latin['F'], EUFD_char_w, EUFD_char_h},
	[7]   = {latin['G'], EUFD_char_w, EUFD_char_h},
	[8]   = {latin['H'], EUFD_char_w, EUFD_char_h},
	[9]   = {latin['I'], EUFD_char_w, EUFD_char_h},
	[10]  = {latin['J'], EUFD_char_w, EUFD_char_h},
	[11]  = {latin['K'], EUFD_char_w, EUFD_char_h},
	[12]  = {latin['L'], EUFD_char_w, EUFD_char_h},
	[13]  = {latin['M'], EUFD_char_w, EUFD_char_h},
	[14]  = {latin['N'], EUFD_char_w, EUFD_char_h},
	[15]  = {latin['O'], EUFD_char_w, EUFD_char_h},
	[16]  = {latin['P'], EUFD_char_w, EUFD_char_h},
	[17]  = {latin['Q'], EUFD_char_w, EUFD_char_h},
	[18]  = {latin['R'], EUFD_char_w, EUFD_char_h},
	[19]  = {latin['S'], EUFD_char_w, EUFD_char_h},
	[20]  = {latin['T'], EUFD_char_w, EUFD_char_h},
	[21]  = {latin['U'], EUFD_char_w, EUFD_char_h},
	[22]  = {latin['V'], EUFD_char_w, EUFD_char_h},
	[23]  = {latin['W'], EUFD_char_w, EUFD_char_h},
	[24]  = {latin['X'], EUFD_char_w, EUFD_char_h},
	[25]  = {latin['Y'], EUFD_char_w, EUFD_char_h},
	[26]  = {latin['Z'], EUFD_char_w, EUFD_char_h},

	[27]  = {symbol['0'], EUFD_char_w, EUFD_char_h},
	[28]  = {symbol['1'], EUFD_char_w, EUFD_char_h},
	[29]  = {symbol['2'], EUFD_char_w, EUFD_char_h},
	[30]  = {symbol['3'], EUFD_char_w, EUFD_char_h},
	[31]  = {symbol['4'], EUFD_char_w, EUFD_char_h},
	[32]  = {symbol['5'], EUFD_char_w, EUFD_char_h},
	[33]  = {symbol['6'], EUFD_char_w, EUFD_char_h},
	[34]  = {symbol['7'], EUFD_char_w, EUFD_char_h},
	[35]  = {symbol['8'], EUFD_char_w, EUFD_char_h},
	[36]  = {symbol['9'], EUFD_char_w, EUFD_char_h},

	[37]  = {symbol['<'], EUFD_char_w, EUFD_char_h},
	[38]  = {symbol['>'], EUFD_char_w, EUFD_char_h},
	[39]  = {symbol['*'], EUFD_char_w, EUFD_char_h},
	[40]  = {symbol['-'], EUFD_char_w, EUFD_char_h},
	[41]  = {symbol['='], EUFD_char_w, EUFD_char_h},
	[42]  = {symbol[':'], EUFD_char_w, EUFD_char_h},
	[43]  = {symbol['~'], EUFD_char_wide, EUFD_char_h},
	[44]  = {symbol['+'], EUFD_char_wide, EUFD_char_h},
	[45]  = {symbol['['], EUFD_char_wide, EUFD_char_h},
	[46]  = {symbol[']'], EUFD_char_wide, EUFD_char_h},
	[47]  = {symbol['.'], EUFD_char_w, EUFD_char_h},
	[48]  = {symbol[' '], EUFD_char_w, EUFD_char_h},
	[49]  = {symbol['$'], EUFD_char_wide, EUFD_char_h},
	[50]  = {symbol['?'], EUFD_char_w, EUFD_char_h},
	[51]  = {symbol['#'], EUFD_char_wide, EUFD_char_h},
	[52]  = {symbol['!'], EUFD_char_wide, EUFD_char_h},
	[53]  = {symbol['/'], EUFD_char_w, EUFD_char_h},
	[54]  = {symbol['|'], EUFD_char_w, EUFD_char_h},
	[55]  = {symbol['_'], EUFD_char_w, EUFD_char_h},
}

fontdescription["font_EUFD"] = {
	texture			= ResourcesPath.."Displays/EUFD_font.dds",
	size			= {8, 8},		-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {EUFD_char_w, EUFD_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_EUFD_chars
}


local map_char_w = 128
local map_char_h = 128
local MAP_WP_SA_symbols = {
	[1]   = {latin['A'], map_char_w, map_char_h},
	[2]   = {latin['B'], map_char_w, map_char_h},
	[3]   = {latin['C'], map_char_w, map_char_h},
	[4]   = {latin['D'], map_char_w, map_char_h},
	[5]   = {latin['E'], map_char_w, map_char_h},
	[6]   = {latin['F'], map_char_w, map_char_h},
	[7]   = {latin['G'], map_char_w, map_char_h},
	[8]   = {latin['H'], map_char_w, map_char_h},
	[9]   = {latin['I'], map_char_w, map_char_h},
	[10]  = {latin['J'], map_char_w, map_char_h},

	[11]  = {latin['a'], map_char_w, map_char_h},
	[12]  = {latin['b'], map_char_w, map_char_h},
	[13]  = {latin['c'], map_char_w, map_char_h},
	[14]  = {latin['d'], map_char_w, map_char_h},
	[15]  = {latin['e'], map_char_w, map_char_h},
	
	[16]  = {symbol[' '], map_char_w, map_char_h}
}

fontdescription["font_MAP_WP_SA_symbs"] = {
	texture			= ResourcesPath.."Displays/MPD/TSD/MAP_WP_SA_symbols.tga",
	size			= {4, 4},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {map_char_w, map_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= MAP_WP_SA_symbols
}

local MAP_CM_TT_symbs = {
	[1]   = {latin['A'], map_char_w, map_char_h},
	[2]   = {latin['B'], map_char_w, map_char_h},
	[3]   = {latin['C'], map_char_w, map_char_h},
	[4]   = {latin['D'], map_char_w, map_char_h},
	[5]   = {latin['E'], map_char_w, map_char_h},
	[6]   = {latin['F'], map_char_w, map_char_h},
	[7]   = {latin['G'], map_char_w, map_char_h},
	[8]   = {latin['H'], map_char_w, map_char_h},
	[9]   = {latin['I'], map_char_w, map_char_h},
	[10]  = {latin['J'], map_char_w, map_char_h},

	[11]  = {latin['K'], map_char_w, map_char_h},
	[12]  = {latin['L'], map_char_w, map_char_h},
	[13]  = {latin['M'], map_char_w, map_char_h},
	[14]  = {latin['N'], map_char_w, map_char_h},
	[15]  = {latin['O'], map_char_w, map_char_h},
	[16]  = {latin['P'], map_char_w, map_char_h},
	[17]  = {latin['Q'], map_char_w, map_char_h},
	[18]  = {latin['R'], map_char_w, map_char_h},
	[19]  = {latin['S'], map_char_w, map_char_h},
	[20]  = {latin['T'], map_char_w, map_char_h},

	[21]  = {latin['U'], map_char_w, map_char_h},
	[22]  = {latin['V'], map_char_w, map_char_h},
	[23]  = {latin['W'], map_char_w, map_char_h},
	[24]  = {latin['X'], map_char_w, map_char_h},
	[25]  = {latin['Y'], map_char_w, map_char_h},
	[26]  = {latin['Z'], map_char_w, map_char_h},
	[27]  = {latin['a'], map_char_w, map_char_h},
	[28]  = {latin['b'], map_char_w, map_char_h},
	[29]  = {latin['c'], map_char_w, map_char_h},
	[30]  = {latin['d'], map_char_w, map_char_h},

	[31]  = {latin['e'], map_char_w, map_char_h},
	[32]  = {latin['f'], map_char_w, map_char_h},
	[33]  = {latin['g'], map_char_w, map_char_h},
	[34]  = {latin['h'], map_char_w, map_char_h},
	[35]  = {latin['i'], map_char_w, map_char_h},
	[36]  = {latin['j'], map_char_w, map_char_h},
	[37]  = {latin['k'], map_char_w, map_char_h},
	[38]  = {latin['l'], map_char_w, map_char_h},
	[39]  = {latin['m'], map_char_w, map_char_h},
	[40]  = {latin['n'], map_char_w, map_char_h},

	[41]  = {latin['o'], map_char_w, map_char_h},
	[42]  = {latin['p'], map_char_w, map_char_h},
	[43]  = {latin['q'], map_char_w, map_char_h},
	[44]  = {latin['r'], map_char_w, map_char_h},
	[45]  = {latin['s'], map_char_w, map_char_h},
	[46]  = {latin['t'], map_char_w, map_char_h},
	[47]  = {latin['u'], map_char_w, map_char_h},
	[48]  = {latin['v'], map_char_w, map_char_h},
	[49]  = {latin['w'], map_char_w, map_char_h},
	[50]  = {latin['x'], map_char_w, map_char_h},

	[51]  = {latin['y'], map_char_w, map_char_h},
	[52]  = {latin['z'], map_char_w, map_char_h},
	[53]  = {symbol['0'], map_char_w, map_char_h},
	[54]  = {symbol['1'], map_char_w, map_char_h},
	[55]  = {symbol['2'], map_char_w, map_char_h},
	[56]  = {symbol['3'], map_char_w, map_char_h},
	[57]  = {symbol['4'], map_char_w, map_char_h},
	[58]  = {symbol['5'], map_char_w, map_char_h},
	[59]  = {symbol['6'], map_char_w, map_char_h},
	[60]  = {symbol['7'], map_char_w, map_char_h},

	[61]  = {symbol['8'], map_char_w, map_char_h},
	[62]  = {symbol['9'], map_char_w, map_char_h},
	[63]  = {symbol['+'], map_char_w, map_char_h},
	[64]  = {symbol['-'], map_char_w, map_char_h},
	[65]  = {symbol['*'], map_char_w, map_char_h},
	[66]  = {symbol['/'], map_char_w, map_char_h},
	[67]  = {symbol['.'], map_char_w, map_char_h},
	[68]  = {symbol[':'], map_char_w, map_char_h},
	[69]  = {symbol['#'], map_char_w, map_char_h},
	[70]  = {symbol[' '], map_char_w, map_char_h},

	[71]  = {symbol['?'], map_char_w, map_char_h},
	[72]  = {symbol['<'], map_char_w, map_char_h},
	[73]  = {symbol['='], map_char_w, map_char_h}
}

fontdescription["font_MAP_CM_TT_symbs"] = {
	texture			= ResourcesPath.."Displays/MPD/TSD/MAP_CM_TT_symbols.tga",
	size			= {16, 16},	-- rows, columns
	resolution		= {2048, 2048},	-- [w, h] resolution of texture in pixels
	default			= {map_char_w, map_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= MAP_CM_TT_symbs
}



local tedac_char_w = 34	-- 17
local tedac_char_h = 50	-- 25
local font_TEDAC_chars = {
	[1]   = {latin['A'], tedac_char_w, tedac_char_h},
	[2]   = {latin['B'], tedac_char_w, tedac_char_h},
	[3]   = {latin['C'], tedac_char_w, tedac_char_h},
	[4]   = {latin['D'], tedac_char_w, tedac_char_h},
	[5]   = {latin['E'], tedac_char_w, tedac_char_h},
	[6]   = {latin['F'], tedac_char_w, tedac_char_h},
	[7]   = {latin['G'], tedac_char_w, tedac_char_h},
	[8]   = {latin['H'], tedac_char_w, tedac_char_h},
	[9]   = {latin['I'], tedac_char_w, tedac_char_h},
	[10]  = {latin['J'], tedac_char_w, tedac_char_h},
	[11]  = {latin['K'], tedac_char_w, tedac_char_h},
	[12]  = {latin['L'], tedac_char_w, tedac_char_h},
	[13]  = {latin['M'], tedac_char_w, tedac_char_h},
	[14]  = {latin['N'], tedac_char_w, tedac_char_h},
	[15]  = {latin['O'], tedac_char_w, tedac_char_h},
	[16]  = {latin['P'], tedac_char_w, tedac_char_h},
	[17]  = {latin['Q'], tedac_char_w, tedac_char_h},
	[18]  = {latin['R'], tedac_char_w, tedac_char_h},
	[19]  = {latin['S'], tedac_char_w, tedac_char_h},
	[20]  = {latin['T'], tedac_char_w, tedac_char_h},
	[21]  = {latin['U'], tedac_char_w, tedac_char_h},
	[22]  = {latin['V'], tedac_char_w, tedac_char_h},
	[23]  = {latin['W'], tedac_char_w, tedac_char_h},
	[24]  = {latin['X'], tedac_char_w, tedac_char_h},
	[25]  = {latin['Y'], tedac_char_w, tedac_char_h},
	[26]  = {latin['Z'], tedac_char_w, tedac_char_h},

	[27]  = {symbol['-'], tedac_char_w, tedac_char_h},
	[28]  = {symbol[':'], tedac_char_w, tedac_char_h},
	[29]  = {symbol['%'], tedac_char_w, tedac_char_h},
	[30]  = {symbol['/'], tedac_char_w, tedac_char_h},
	[31]  = {symbol['.'], tedac_char_w, tedac_char_h},
	[32]  = {symbol[' '], tedac_char_w, tedac_char_h},

	[33]  = {symbol['0'], tedac_char_w, tedac_char_h},
	[34]  = {symbol['1'], tedac_char_w, tedac_char_h},
	[35]  = {symbol['2'], tedac_char_w, tedac_char_h},
	[36]  = {symbol['3'], tedac_char_w, tedac_char_h},
	[37]  = {symbol['4'], tedac_char_w, tedac_char_h},
	[38]  = {symbol['5'], tedac_char_w, tedac_char_h},
	[39]  = {symbol['6'], tedac_char_w, tedac_char_h},
	[40]  = {symbol['7'], tedac_char_w, tedac_char_h},
	[41]  = {symbol['8'], tedac_char_w, tedac_char_h},
	[42]  = {symbol['9'], tedac_char_w, tedac_char_h},

	[43]  = {symbol['*'], tedac_char_w, tedac_char_h},
	[44]  = {symbol['='], tedac_char_w, tedac_char_h},
	[45]  = {symbol['?'], tedac_char_w, tedac_char_h},

	[46]  = {symbol['{'], tedac_char_w, tedac_char_h}, -- 7 • "enabled"
	[47]  = {symbol['}'], tedac_char_w, tedac_char_h}, -- 9 ○ "disabled"

	[48]  = {latin['a'],  60, 62},	-- arrow up
	[49]  = {latin['b'],  60, 62},	-- arrow down
	[50]  = {latin['c'],  58, 64},	-- arrow left
	[51]  = {latin['d'],  58, 64},	-- arrow right
}

fontdescription["font_TEDAC"] = {
	texture			= ResourcesPath.."Displays/TEDAC/TEDAC_font.dds",
	size			= {8, 8},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {tedac_char_w, tedac_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_TEDAC_chars
}

local tedac_big_char_w = 40	-- 20
local tedac_big_char_h = 58	-- 29
local font_TEDAC_big_chars = {
	[1]  = {symbol['0'], tedac_big_char_w, tedac_big_char_h},
	[2]  = {symbol['1'], tedac_big_char_w, tedac_big_char_h},
	[3]  = {symbol['2'], tedac_big_char_w, tedac_big_char_h},
	[4]  = {symbol['3'], tedac_big_char_w, tedac_big_char_h},
	[5]  = {symbol['4'], tedac_big_char_w, tedac_big_char_h},
	[6]  = {symbol['5'], tedac_big_char_w, tedac_big_char_h},
	[7]  = {symbol['6'], tedac_big_char_w, tedac_big_char_h},
	[8]  = {symbol['7'], tedac_big_char_w, tedac_big_char_h},
	[9]  = {symbol['8'], tedac_big_char_w, tedac_big_char_h},
	[10] = {symbol['9'], tedac_big_char_w, tedac_big_char_h},
	[11] = {symbol[' '], tedac_big_char_w, tedac_big_char_h},
	[12] = {latin['H'], tedac_big_char_w, tedac_big_char_h},
	[13] = {latin['I'], tedac_big_char_w, tedac_big_char_h},
	[14] = {latin['L'], tedac_big_char_w, tedac_big_char_h},
	[15] = {latin['O'], tedac_big_char_w, tedac_big_char_h},
}

fontdescription["font_TEDAC_big"] = {
	texture			= ResourcesPath.."Displays/TEDAC/TEDAC_symbology.dds",
	size			= {8, 8},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {tedac_big_char_w, tedac_big_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_TEDAC_big_chars
}
-------------------------------------------------------HMD---------------------
stroke_thickness = 2
stroke_fuzziness = 2

fontdescription["font_stroke_HMD"] = {
	class     = "ceSLineFont",
	symb_storage = "stroke_font_HMD",
	thickness  = stroke_thickness,
	fuzziness  = stroke_fuzziness,
	draw_as_wire = dbg_drawStrokesAsWire,
	default    = {10.5, 10.0},
	chars	   = {
		 [1]   = {latin['A'], "A"},
		 [2]   = {latin['B'], "B"},
		 [3]   = {latin['C'], "C"},
		 [4]   = {latin['D'], "D"},
		 [5]   = {latin['E'], "E"},
		 [6]   = {latin['F'], "F"},
		 [7]   = {latin['G'], "G"},
		 [8]   = {latin['H'], "H"},
		 [9]   = {latin['I'], "I"},
		 [10]  = {latin['J'], "J"},
		 [11]  = {latin['K'], "K"},
		 [12]  = {latin['L'], "L"},
		 [13]  = {latin['M'], "M"},
		 [14]  = {latin['N'], "N"},
		 [15]  = {latin['O'], "O"},
		 [16]  = {latin['P'], "P"},
		 [17]  = {latin['Q'], "Q"},
		 [18]  = {latin['R'], "R"},
		 [19]  = {latin['S'], "S"},
		 [20]  = {latin['T'], "T"},
		 [21]  = {latin['U'], "U"},
		 [22]  = {latin['V'], "V"},
		 [23]  = {latin['W'], "W"},
		 [24]  = {latin['X'], "X"},
		 [25]  = {latin['Y'], "Y"},
		 [26]  = {latin['Z'], "Z"},
		 
		 [27]  = {symbol['0'], "0"},
		 [28]  = {symbol['1'], "1"},
		 [29]  = {symbol['2'], "2"},
		 [30]  = {symbol['3'], "3"},
		 [31]  = {symbol['4'], "4"},
		 [32]  = {symbol['5'], "5"},
		 [33]  = {symbol['6'], "6"},
		 [34]  = {symbol['7'], "7"},
		 [35]  = {symbol['8'], "8"},
		 [36]  = {symbol['9'], "9"},
		 
		 [37]  = {symbol['-'], "symbol-minus"},
		 [38]  = {symbol['+'], "symbol-plus"},
		 [39]  = {symbol['\''], "symbol-apostrophe"},
		 [40]  = {symbol['('], "symbol-parenthesis-left"},
		 [41]  = {symbol[')'], "symbol-parenthesis-right"},
		 [42]  = {symbol['*'], "symbol-asterisk"},
		 [43]  = {symbol['%'], "symbol-percent"},
		 [44]  = {symbol[','], "symbol-comma"},
		 [45]  = {symbol['°'], "symbol-degree"},
		 [46]  = {symbol['.'], "symbol-period"},
		 [47]  = {symbol['/'], "symbol-slash"},
		 [48]  = {symbol['\\'], "symbol-backslash"},
		 [49]  = {symbol['\"'], "symbol-quote"},
		 [50]  = {symbol['?'], "symbol-question"},
		 [51]  = {symbol[':'], "symbol-colon"},
		 [52]  = {symbol['='], "symbol-equal"},
	}
}

-------------------------------------------------------------------------------
local hmd_char_w =34
local hmd_char_h =50
local font_HMD_chars = {
	[1]   = {latin['A'], 	hmd_char_w, hmd_char_h},
	[2]   = {latin['B'], 	hmd_char_w, hmd_char_h},
	[3]   = {latin['C'], 	hmd_char_w, hmd_char_h},
	[4]   = {latin['D'], 	hmd_char_w, hmd_char_h},
	[5]   = {latin['E'], 	hmd_char_w, hmd_char_h},
	[6]   = {latin['F'], 	hmd_char_w, hmd_char_h},
	[7]   = {latin['G'], 	hmd_char_w, hmd_char_h},
	[8]   = {latin['H'], 	hmd_char_w, hmd_char_h},
	[9]   = {latin['I'], 	hmd_char_w, hmd_char_h},
	[10]  = {latin['J'], 	hmd_char_w, hmd_char_h},
	[11]  = {latin['K'], 	hmd_char_w, hmd_char_h},
	[12]  = {latin['L'], 	hmd_char_w, hmd_char_h},
	[13]  = {latin['M'], 	hmd_char_w, hmd_char_h},
	[14]  = {latin['N'], 	hmd_char_w, hmd_char_h},
	[15]  = {latin['O'], 	hmd_char_w, hmd_char_h},
	[16]  = {latin['P'], 	hmd_char_w, hmd_char_h},
	[17]  = {latin['Q'], 	hmd_char_w, hmd_char_h},
	[18]  = {latin['R'], 	hmd_char_w, hmd_char_h},
	[19]  = {latin['S'], 	hmd_char_w, hmd_char_h},
	[20]  = {latin['T'], 	hmd_char_w, hmd_char_h},
	[21]  = {latin['U'], 	hmd_char_w, hmd_char_h},
	[22]  = {latin['V'], 	hmd_char_w, hmd_char_h},
	[23]  = {latin['W'], 	hmd_char_w, hmd_char_h},
	[24]  = {latin['X'], 	hmd_char_w, hmd_char_h},
	[25]  = {latin['Y'], 	hmd_char_w, hmd_char_h},
	[26]  = {latin['Z'], 	hmd_char_w, hmd_char_h},

	[27]  = {symbol['-'],	hmd_char_w, hmd_char_h},
	[28]  = {symbol[':'],	hmd_char_w, hmd_char_h},
	[29]  = {symbol['%'],	hmd_char_w, hmd_char_h},
	[30]  = {symbol['/'],	hmd_char_w, hmd_char_h},
	[31]  = {symbol['.'],	hmd_char_w, hmd_char_h},
	[32]  = {symbol[' '],	hmd_char_w, hmd_char_h},

	[33]  = {symbol['0'],	hmd_char_w, hmd_char_h},
	[34]  = {symbol['1'],	hmd_char_w, hmd_char_h},
	[35]  = {symbol['2'],	hmd_char_w, hmd_char_h},
	[36]  = {symbol['3'],	hmd_char_w, hmd_char_h},
	[37]  = {symbol['4'],	hmd_char_w, hmd_char_h},
	[38]  = {symbol['5'],	hmd_char_w, hmd_char_h},
	[39]  = {symbol['6'],	hmd_char_w, hmd_char_h},
	[40]  = {symbol['7'],	hmd_char_w, hmd_char_h},
	[41]  = {symbol['8'],	hmd_char_w, hmd_char_h},
	[42]  = {symbol['9'],	hmd_char_w, hmd_char_h},
	[43]  = {symbol['*'],	hmd_char_w, hmd_char_h},
	[44]  = {symbol['='],	hmd_char_w, hmd_char_h},
	[45]  = {symbol['?'],	hmd_char_w, hmd_char_h},

	[46]  = {symbol['{'], hmd_char_w, hmd_char_h}, -- 7 • "enabled"
	[47]  = {symbol['}'], hmd_char_w, hmd_char_h}, -- 9 ○ "disabled"

	[48]  = {latin['a'],  60, 62},	-- arrow up
	[49]  = {latin['b'],  60, 62},	-- arrow down
	[50]  = {latin['c'],  58, 64},	-- arrow left
	[51]  = {latin['d'],  58, 64},	-- arrow right
}

fontdescription["font_HMD"] = {
	texture			= ResourcesPath.."Displays/HMD/HMD_font.dds",
	size			= {8, 8},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {hmd_char_w, hmd_char_h},	
	chars			= font_HMD_chars
}

local hmd_big_char_w = 40 
local hmd_big_char_h = 58
local font_HMD_big_chars = {
	[1]  = {symbol['0'], 	hmd_big_char_w, hmd_big_char_h},
	[2]  = {symbol['1'], 	hmd_big_char_w, hmd_big_char_h},
	[3]  = {symbol['2'], 	hmd_big_char_w, hmd_big_char_h},
	[4]  = {symbol['3'], 	hmd_big_char_w, hmd_big_char_h},
	[5]  = {symbol['4'], 	hmd_big_char_w, hmd_big_char_h},
	[6]  = {symbol['5'], 	hmd_big_char_w, hmd_big_char_h},
	[7]  = {symbol['6'], 	hmd_big_char_w, hmd_big_char_h},
	[8]  = {symbol['7'], 	hmd_big_char_w, hmd_big_char_h},
	[9]  = {symbol['8'], 	hmd_big_char_w, hmd_big_char_h},
	[10] = {symbol['9'], 	hmd_big_char_w, hmd_big_char_h},
	[11] = {symbol[' '], 	hmd_big_char_w, hmd_big_char_h},
	[12] = {latin['H'],		hmd_big_char_w, hmd_big_char_h},
	[13] = {latin['I'],		hmd_big_char_w, hmd_big_char_h},
	[14] = {latin['L'],		hmd_big_char_w, hmd_big_char_h},
	[15] = {latin['O'],		hmd_big_char_w, hmd_big_char_h},
}

fontdescription["font_HMD_big"] = {
	texture			= ResourcesPath.."Displays/HMD/HMD_symbology.dds",
	size			= {8, 8},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {hmd_big_char_w, hmd_big_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_HMD_big_chars
}
-----------------------------------------------------------------
fontdescription["font_VIDEO_MPD"] = {
	texture			= ResourcesPath.."Displays/MPD/MPD_VideoSymbology_font.dds",
	size			= {8, 8},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {hmd_char_w, hmd_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_HMD_chars
}
fontdescription["font_VIDEO_MPD_BIG"] = {
	texture			= ResourcesPath.."Displays/MPD/MPD_VideoSymbology.dds",
	size			= {8, 8},	-- rows, columns
	resolution		= {512, 512},	-- [w, h] resolution of texture in pixels
	default			= {hmd_big_char_w, hmd_big_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_HMD_big_chars
}
-----------------------------------------------------------------
local wpn_char_w = 256
local wpn_char_h = 256
local WPN_MSL_symbols = {
	[1]   = {latin['A'], wpn_char_w, wpn_char_h},
	[2]   = {latin['B'], wpn_char_w, wpn_char_h},
	[3]   = {latin['C'], wpn_char_w, wpn_char_h},
	[4]   = {latin['D'], wpn_char_w, wpn_char_h},
	[5]   = {latin['E'], wpn_char_w, wpn_char_h},
	[6]   = {latin['F'], wpn_char_w, wpn_char_h},
	[7]   = {latin['G'], wpn_char_w, wpn_char_h},
	[8]   = {latin['H'], wpn_char_w, wpn_char_h},
	[9]   = {latin['I'], wpn_char_w, wpn_char_h},
	[10]  = {latin['J'], wpn_char_w, wpn_char_h},
	[11]  = {latin['K'], wpn_char_w, wpn_char_h},
}

fontdescription["font_WPN_MSL_symbs"] = {
	texture			= ResourcesPath.."Displays/MPD/indication_MPD_WPN.tga",
	size			= {8, 8},					-- rows, columns
	resolution		= {2048, 2048},				-- [w, h] resolution of texture in pixels
	default			= {wpn_char_w, wpn_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= WPN_MSL_symbols
}

fontdescription["font_WPN_MSL_symbs_fon"] = {
	texture			= ResourcesPath.."Displays/MPD/indication_MPD_WPN_fon.tga",
	size			= {8, 8},					-- rows, columns
	resolution		= {2048, 2048},				-- [w, h] resolution of texture in pixels
	default			= {wpn_char_w, wpn_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= WPN_MSL_symbols
}

local CMWS_char_w = 15
local CMWS_char_h = 26
local font_CMWS_chars = {
	[1]   = {latin['A'], CMWS_char_w, CMWS_char_h},
	[2]   = {latin['B'], CMWS_char_w, CMWS_char_h},
	[3]   = {latin['C'], CMWS_char_w, CMWS_char_h},
	[4]   = {latin['D'], CMWS_char_w, CMWS_char_h},
	[5]   = {latin['E'], CMWS_char_w, CMWS_char_h},
	[6]   = {latin['F'], CMWS_char_w, CMWS_char_h},
	[7]   = {latin['G'], CMWS_char_w, CMWS_char_h},
	[8]   = {latin['H'], CMWS_char_w, CMWS_char_h},
	[9]   = {latin['I'], CMWS_char_w, CMWS_char_h},
	[10]  = {latin['J'], CMWS_char_w, CMWS_char_h},
	[11]  = {latin['K'], CMWS_char_w, CMWS_char_h},
	[12]  = {latin['L'], CMWS_char_w, CMWS_char_h},
	[13]  = {latin['M'], CMWS_char_w, CMWS_char_h},
	[14]  = {latin['N'], CMWS_char_w, CMWS_char_h},
	[15]  = {latin['O'], CMWS_char_w, CMWS_char_h},
	[16]  = {latin['P'], CMWS_char_w, CMWS_char_h},
	[17]  = {latin['Q'], CMWS_char_w, CMWS_char_h},
	[18]  = {latin['R'], CMWS_char_w, CMWS_char_h},
	[19]  = {latin['S'], CMWS_char_w, CMWS_char_h},
	[20]  = {latin['T'], CMWS_char_w, CMWS_char_h},
	[21]  = {latin['U'], CMWS_char_w, CMWS_char_h},
	[22]  = {latin['V'], CMWS_char_w, CMWS_char_h},
	[23]  = {latin['W'], CMWS_char_w, CMWS_char_h},
	[24]  = {latin['X'], CMWS_char_w, CMWS_char_h},
	[25]  = {latin['Y'], CMWS_char_w, CMWS_char_h},
	[26]  = {latin['Z'], CMWS_char_w, CMWS_char_h},
	
	[27]  = {symbol['0'], CMWS_char_w, CMWS_char_h},
	[28]  = {symbol['1'], CMWS_char_w, CMWS_char_h},
	[29]  = {symbol['2'], CMWS_char_w, CMWS_char_h},
	[30]  = {symbol['3'], CMWS_char_w, CMWS_char_h},
	[31]  = {symbol['4'], CMWS_char_w, CMWS_char_h},
	[32]  = {symbol['5'], CMWS_char_w, CMWS_char_h},
	[33]  = {symbol['6'], CMWS_char_w, CMWS_char_h},
	[34]  = {symbol['7'], CMWS_char_w, CMWS_char_h},
	[35]  = {symbol['8'], CMWS_char_w, CMWS_char_h},
	[36]  = {symbol['9'], CMWS_char_w, CMWS_char_h},
	[37]  = {symbol[' '], CMWS_char_w, CMWS_char_h},
	[38]  = {symbol['/'], CMWS_char_w, CMWS_char_h},
}
fontdescription["font_CMWS"] = {
	texture			= ResourcesPath.."CMWS/CMWS_font.tga",
	size			= {2, 32},	-- rows, columns
	resolution		= {512, 64},	-- [w, h] resolution of texture in pixels
	default			= {CMWS_char_w, CMWS_char_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_CMWS_chars
}


local FCR_char_symbol_w = 64
local FCR_char_symbol_h = 64
local font_FCR_chars = {
	[1]  = {latin['a'], FCR_char_symbol_w, FCR_char_symbol_h},
	[2]  = {latin['b'], FCR_char_symbol_w, FCR_char_symbol_h},
	[3]  = {latin['c'], FCR_char_symbol_w, FCR_char_symbol_h},
	[4]  = {latin['d'], FCR_char_symbol_w, FCR_char_symbol_h},
	[5]  = {latin['e'], FCR_char_symbol_w, FCR_char_symbol_h},
	[6]  = {latin['f'], FCR_char_symbol_w, FCR_char_symbol_h},
	[7]  = {latin['g'], FCR_char_symbol_w, FCR_char_symbol_h},
	[8]  = {latin['h'], FCR_char_symbol_w, FCR_char_symbol_h},
	[9]  = {latin['i'], FCR_char_symbol_w, FCR_char_symbol_h},
	[10]  = {latin['j'], FCR_char_symbol_w, FCR_char_symbol_h},
	[11]  = {latin['k'], FCR_char_symbol_w, FCR_char_symbol_h},
	[12]  = {latin['l'], FCR_char_symbol_w, FCR_char_symbol_h},
	[13]  = {latin['m'], FCR_char_symbol_w, FCR_char_symbol_h},
	[14]  = {latin['n'], FCR_char_symbol_w, FCR_char_symbol_h},
	[15]  = {latin['o'], FCR_char_symbol_w, FCR_char_symbol_h},
	[16]  = {latin['p'], FCR_char_symbol_w, FCR_char_symbol_h},	
	[17]  = {latin['q'], FCR_char_symbol_w, FCR_char_symbol_h},
	[18]  = {latin['r'], FCR_char_symbol_w, FCR_char_symbol_h},
	[19]  = {latin['s'], FCR_char_symbol_w, FCR_char_symbol_h},
}

fontdescription["font_FCR_Target_symbol"] = {
	texture			= ResourcesPath.."Displays/MPD/MPD_FCR_Target_font.dds",
	size			= {8, 8},					-- rows, columns
	resolution		= {512, 512},				-- [w, h] resolution of texture in pixels
	default			= {FCR_char_symbol_w, FCR_char_symbol_h},	-- [w,h] resolution for symbol in pixels
	chars			= font_FCR_chars
}

local TEDAC_FCR_char_symbol_w = 64
local TEDAC_FCR_char_symbol_h = 64
local TEDAC_font_FCR_chars = {
	[1]  = {latin['a'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[2]  = {latin['b'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[3]  = {latin['c'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[4]  = {latin['d'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[5]  = {latin['e'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[6]  = {latin['f'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[7]  = {latin['g'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[8]  = {latin['h'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[9]  = {latin['i'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[10]  = {latin['j'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[11]  = {latin['k'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[12]  = {latin['l'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[13]  = {latin['m'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[14]  = {latin['n'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[15]  = {latin['o'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[16]  = {latin['p'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},	
	[17]  = {latin['q'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[18]  = {latin['r'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
	[19]  = {latin['s'], TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},
}

fontdescription["font_TEDAC_FCR_Target_symbol"] = {
	texture			= ResourcesPath.."Displays/TEDAC/TEDAC_FCR_Target_font.dds",
	size			= {8, 8},					-- rows, columns
	resolution		= {512, 512},				-- [w, h] resolution of texture in pixels
	default			= {TEDAC_FCR_char_symbol_w, TEDAC_FCR_char_symbol_h},	-- [w,h] resolution for symbol in pixels
	chars			= TEDAC_font_FCR_chars
}
