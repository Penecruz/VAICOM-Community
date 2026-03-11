dofile(LockOn_Options.script_path.."clickable_defs.lua")
dofile(LockOn_Options.script_path.."command_defs.lua")
dofile(LockOn_Options.script_path.."devices.lua")
--dofile(LockOn_Options.script_path.."config.lua")
dofile(LockOn_Options.script_path.."sounds.lua")
dofile(LockOn_Options.script_path.."VR_config.lua")

local gettext = require("i_18n")
_ = gettext.translate


elements = {}


-- MFD Plt Left
elements["pnt_20"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, T1"),				devices.MFD_PLT_LEFT, mpd_commands.T1,			20)
elements["pnt_21"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, T2"),				devices.MFD_PLT_LEFT, mpd_commands.T2,			21)
elements["pnt_22"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, T3"),				devices.MFD_PLT_LEFT, mpd_commands.T3,			22)
elements["pnt_23"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, T4"),				devices.MFD_PLT_LEFT, mpd_commands.T4,			23)
elements["pnt_24"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, T5"),				devices.MFD_PLT_LEFT, mpd_commands.T5,			24)
elements["pnt_25"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, T6"),				devices.MFD_PLT_LEFT, mpd_commands.T6,			25)
elements["pnt_28"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, R1"),				devices.MFD_PLT_LEFT, mpd_commands.R1,			28)
elements["pnt_29"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, R2"),				devices.MFD_PLT_LEFT, mpd_commands.R2,			29)
elements["pnt_30"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, R3"),				devices.MFD_PLT_LEFT, mpd_commands.R3,			30)
elements["pnt_31"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, R4"),				devices.MFD_PLT_LEFT, mpd_commands.R4,			31)
elements["pnt_32"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, R5"),				devices.MFD_PLT_LEFT, mpd_commands.R5,			32)
elements["pnt_33"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, R6"),				devices.MFD_PLT_LEFT, mpd_commands.R6,			33)
elements["pnt_37"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, B6"),				devices.MFD_PLT_LEFT, mpd_commands.B6,			37)
elements["pnt_38"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, B5"),				devices.MFD_PLT_LEFT, mpd_commands.B5,			38)
elements["pnt_39"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, B4"),				devices.MFD_PLT_LEFT, mpd_commands.B4,			39)
elements["pnt_40"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, B3"),				devices.MFD_PLT_LEFT, mpd_commands.B3,			40)
elements["pnt_41"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, B2"),				devices.MFD_PLT_LEFT, mpd_commands.B2,			41)
elements["pnt_42"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, B1/M(Menu)"),		devices.MFD_PLT_LEFT, mpd_commands.B1,			42)
elements["pnt_12"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, L6"),				devices.MFD_PLT_LEFT, mpd_commands.L6,			12)
elements["pnt_13"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, L5"),				devices.MFD_PLT_LEFT, mpd_commands.L5,			13)
elements["pnt_14"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, L4"),				devices.MFD_PLT_LEFT, mpd_commands.L4,			14)
elements["pnt_15"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, L3"),				devices.MFD_PLT_LEFT, mpd_commands.L3,			15)
elements["pnt_16"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, L2"),				devices.MFD_PLT_LEFT, mpd_commands.L2,			16)
elements["pnt_17"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, L1"),				devices.MFD_PLT_LEFT, mpd_commands.L1,			17)
elements["pnt_27"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, Asterisk"),			devices.MFD_PLT_LEFT, mpd_commands.Asterisk,	27)
elements["pnt_34"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, VID"),				devices.MFD_PLT_LEFT, mpd_commands.VID,			34)
elements["pnt_35"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, COM"),				devices.MFD_PLT_LEFT, mpd_commands.COM,			35)
elements["pnt_36"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, A/C"),				devices.MFD_PLT_LEFT, mpd_commands.AC,			36)
elements["pnt_43"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, TSD"),				devices.MFD_PLT_LEFT, mpd_commands.TSD,			43)
elements["pnt_10"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, WPN"),				devices.MFD_PLT_LEFT, mpd_commands.WPN,			10)
elements["pnt_11"]		= mpd_button(CREW.PLT, _("Left MPD Pushbutton, FCR"),				devices.MFD_PLT_LEFT, mpd_commands.FCR,			11)
				
elements["pnt_18"]		= default_rheostat(CREW.PLT, _("Left MPD Brightness Control Knob"),			devices.MFD_PLT_LEFT, mpd_commands.BRT_KNOB,	18)
elements["pnt_19"]		= default_rheostat(CREW.PLT, _("Left MPD Video Control Knob"),				devices.MFD_PLT_LEFT, mpd_commands.VID_KNOB,	19)
elements["pnt_26"]		= multiposition_switch(CREW.PLT, _("Left MPD Mode Knob, DAY/NIGHT/MONO"),	devices.MFD_PLT_LEFT, mpd_commands.MODE_KNOB,	26, 3, 0.5, IS_INVERSED, 0.0, anim_speed_default * 0.5, NOT_CYCLED)

-- MFD Plt Right
elements["pnt_54"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, T1"),				devices.MFD_PLT_RIGHT, mpd_commands.T1,			54)
elements["pnt_55"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, T2"),				devices.MFD_PLT_RIGHT, mpd_commands.T2,			55)
elements["pnt_56"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, T3"),				devices.MFD_PLT_RIGHT, mpd_commands.T3,			56)
elements["pnt_57"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, T4"),				devices.MFD_PLT_RIGHT, mpd_commands.T4,			57)
elements["pnt_58"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, T5"),				devices.MFD_PLT_RIGHT, mpd_commands.T5,			58)
elements["pnt_59"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, T6"),				devices.MFD_PLT_RIGHT, mpd_commands.T6,			59)
elements["pnt_62"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, R1"),				devices.MFD_PLT_RIGHT, mpd_commands.R1,			62)
elements["pnt_63"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, R2"),				devices.MFD_PLT_RIGHT, mpd_commands.R2,			63)
elements["pnt_64"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, R3"),				devices.MFD_PLT_RIGHT, mpd_commands.R3,			64)
elements["pnt_65"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, R4"),				devices.MFD_PLT_RIGHT, mpd_commands.R4,			65)
elements["pnt_66"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, R5"),				devices.MFD_PLT_RIGHT, mpd_commands.R5,			66)
elements["pnt_67"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, R6"),				devices.MFD_PLT_RIGHT, mpd_commands.R6,			67)
elements["pnt_71"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, B6"),				devices.MFD_PLT_RIGHT, mpd_commands.B6,			71)
elements["pnt_72"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, B5"),				devices.MFD_PLT_RIGHT, mpd_commands.B5,			72)
elements["pnt_73"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, B4"),				devices.MFD_PLT_RIGHT, mpd_commands.B4,			73)
elements["pnt_74"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, B3"),				devices.MFD_PLT_RIGHT, mpd_commands.B3,			74)
elements["pnt_75"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, B2"),				devices.MFD_PLT_RIGHT, mpd_commands.B2,			75)
elements["pnt_76"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, B1/M(Menu)"),		devices.MFD_PLT_RIGHT, mpd_commands.B1,			76)
elements["pnt_46"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, L6"),				devices.MFD_PLT_RIGHT, mpd_commands.L6,			46)
elements["pnt_47"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, L5"),				devices.MFD_PLT_RIGHT, mpd_commands.L5,			47)
elements["pnt_48"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, L4"),				devices.MFD_PLT_RIGHT, mpd_commands.L4,			48)
elements["pnt_49"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, L3"),				devices.MFD_PLT_RIGHT, mpd_commands.L3,			49)
elements["pnt_50"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, L2"),				devices.MFD_PLT_RIGHT, mpd_commands.L2,			50)
elements["pnt_51"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, L1"),				devices.MFD_PLT_RIGHT, mpd_commands.L1,			51)
elements["pnt_61"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, Asterisk"),			devices.MFD_PLT_RIGHT, mpd_commands.Asterisk,	61)
elements["pnt_68"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, VID"),				devices.MFD_PLT_RIGHT, mpd_commands.VID,		68)
elements["pnt_69"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, COM"),				devices.MFD_PLT_RIGHT, mpd_commands.COM,		69)
elements["pnt_70"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, A/C"),				devices.MFD_PLT_RIGHT, mpd_commands.AC,			70)
elements["pnt_77"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, TSD"),				devices.MFD_PLT_RIGHT, mpd_commands.TSD,		77)
elements["pnt_44"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, WPN"),				devices.MFD_PLT_RIGHT, mpd_commands.WPN,		44)
elements["pnt_45"]		= mpd_button(CREW.PLT, _("Right MPD Pushbutton, FCR"),				devices.MFD_PLT_RIGHT, mpd_commands.FCR,		45)
	
elements["pnt_52"]		= default_rheostat(CREW.PLT, _("Right MPD Brightness Control Knob"),		devices.MFD_PLT_RIGHT, mpd_commands.BRT_KNOB,	52)
elements["pnt_53"]		= default_rheostat(CREW.PLT, _("Right MPD Video Control Knob"),				devices.MFD_PLT_RIGHT, mpd_commands.VID_KNOB,	53)
elements["pnt_60"]		= multiposition_switch(CREW.PLT, _("Right MPD Mode Knob, DAY/NIGHT/MONO"),	devices.MFD_PLT_RIGHT, mpd_commands.MODE_KNOB,	60, 3, 0.5, IS_INVERSED, 0.0, anim_speed_default * 0.5, NOT_CYCLED)

-- MFD Cpg Left
elements["pnt_88"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, T1"),				devices.MFD_CPG_LEFT, mpd_commands.T1,			88)
elements["pnt_89"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, T2"),				devices.MFD_CPG_LEFT, mpd_commands.T2,			89)
elements["pnt_90"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, T3"),				devices.MFD_CPG_LEFT, mpd_commands.T3,			90)
elements["pnt_91"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, T4"),				devices.MFD_CPG_LEFT, mpd_commands.T4,			91)
elements["pnt_92"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, T5"),				devices.MFD_CPG_LEFT, mpd_commands.T5,			92)
elements["pnt_93"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, T6"),				devices.MFD_CPG_LEFT, mpd_commands.T6,			93)
elements["pnt_96"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, R1"),				devices.MFD_CPG_LEFT, mpd_commands.R1,			96)
elements["pnt_97"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, R2"),				devices.MFD_CPG_LEFT, mpd_commands.R2,			97)
elements["pnt_98"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, R3"),				devices.MFD_CPG_LEFT, mpd_commands.R3,			98)
elements["pnt_99"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, R4"),				devices.MFD_CPG_LEFT, mpd_commands.R4,			99)
elements["pnt_100"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, R5"),				devices.MFD_CPG_LEFT, mpd_commands.R5,			100)
elements["pnt_101"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, R6"),				devices.MFD_CPG_LEFT, mpd_commands.R6,			101)
elements["pnt_105"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, B6"),				devices.MFD_CPG_LEFT, mpd_commands.B6,			105)
elements["pnt_106"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, B5"),				devices.MFD_CPG_LEFT, mpd_commands.B5,			106)
elements["pnt_107"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, B4"),				devices.MFD_CPG_LEFT, mpd_commands.B4,			107)
elements["pnt_108"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, B3"),				devices.MFD_CPG_LEFT, mpd_commands.B3,			108)
elements["pnt_109"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, B2"),				devices.MFD_CPG_LEFT, mpd_commands.B2,			109)
elements["pnt_110"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, B1/M(Menu)"),		devices.MFD_CPG_LEFT, mpd_commands.B1,			110)
elements["pnt_80"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, L6"),				devices.MFD_CPG_LEFT, mpd_commands.L6,			80)
elements["pnt_81"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, L5"),				devices.MFD_CPG_LEFT, mpd_commands.L5,			81)
elements["pnt_82"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, L4"),				devices.MFD_CPG_LEFT, mpd_commands.L4,			82)
elements["pnt_83"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, L3"),				devices.MFD_CPG_LEFT, mpd_commands.L3,			83)
elements["pnt_84"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, L2"),				devices.MFD_CPG_LEFT, mpd_commands.L2,			84)
elements["pnt_85"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, L1"),				devices.MFD_CPG_LEFT, mpd_commands.L1,			85)
elements["pnt_95"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, Asterisk"),			devices.MFD_CPG_LEFT, mpd_commands.Asterisk,	95)
elements["pnt_102"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, VID"),				devices.MFD_CPG_LEFT, mpd_commands.VID,			102)
elements["pnt_103"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, COM"),				devices.MFD_CPG_LEFT, mpd_commands.COM,			103)
elements["pnt_104"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, A/C"),				devices.MFD_CPG_LEFT, mpd_commands.AC,			104)
elements["pnt_111"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, TSD"),				devices.MFD_CPG_LEFT, mpd_commands.TSD,			111)
elements["pnt_78"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, WPN"),				devices.MFD_CPG_LEFT, mpd_commands.WPN,			78)
elements["pnt_79"]		= mpd_button(CREW.CPG, _("Left MPD Pushbutton, FCR"),				devices.MFD_CPG_LEFT, mpd_commands.FCR,			79)
			
elements["pnt_86"]		= default_rheostat(CREW.CPG, _("Left MPD Brightness Control Knob"),			devices.MFD_CPG_LEFT, mpd_commands.BRT_KNOB,	86)
elements["pnt_87"]		= default_rheostat(CREW.CPG, _("Left MPD Video Control Knob"),				devices.MFD_CPG_LEFT, mpd_commands.VID_KNOB,	87)
elements["pnt_94"]		= multiposition_switch(CREW.CPG, _("Left MPD Mode Knob, DAY/NIGHT/MONO"),	devices.MFD_CPG_LEFT, mpd_commands.MODE_KNOB,	94, 3, 0.5, IS_INVERSED, 0.0, anim_speed_default * 0.5, NOT_CYCLED)
	
-- MFD Cpg Right
elements["pnt_122"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, T1"),				devices.MFD_CPG_RIGHT, mpd_commands.T1,			122)
elements["pnt_123"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, T2"),				devices.MFD_CPG_RIGHT, mpd_commands.T2,			123)
elements["pnt_124"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, T3"),				devices.MFD_CPG_RIGHT, mpd_commands.T3,			124)
elements["pnt_125"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, T4"),				devices.MFD_CPG_RIGHT, mpd_commands.T4,			125)
elements["pnt_126"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, T5"),				devices.MFD_CPG_RIGHT, mpd_commands.T5,			126)
elements["pnt_127"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, T6"),				devices.MFD_CPG_RIGHT, mpd_commands.T6,			127)
elements["pnt_130"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, R1"),				devices.MFD_CPG_RIGHT, mpd_commands.R1,			130)
elements["pnt_131"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, R2"),				devices.MFD_CPG_RIGHT, mpd_commands.R2,			131)
elements["pnt_132"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, R3"),				devices.MFD_CPG_RIGHT, mpd_commands.R3,			132)
elements["pnt_133"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, R4"),				devices.MFD_CPG_RIGHT, mpd_commands.R4,			133)
elements["pnt_134"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, R5"),				devices.MFD_CPG_RIGHT, mpd_commands.R5,			134)
elements["pnt_135"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, R6"),				devices.MFD_CPG_RIGHT, mpd_commands.R6,			135)
elements["pnt_139"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, B6"),				devices.MFD_CPG_RIGHT, mpd_commands.B6,			139)
elements["pnt_140"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, B5"),				devices.MFD_CPG_RIGHT, mpd_commands.B5,			140)
elements["pnt_141"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, B4"),				devices.MFD_CPG_RIGHT, mpd_commands.B4,			141)
elements["pnt_142"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, B3"),				devices.MFD_CPG_RIGHT, mpd_commands.B3,			142)
elements["pnt_143"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, B2"),				devices.MFD_CPG_RIGHT, mpd_commands.B2,			143)
elements["pnt_144"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, B1/M(Menu)"),		devices.MFD_CPG_RIGHT, mpd_commands.B1,			144)
elements["pnt_114"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, L6"),				devices.MFD_CPG_RIGHT, mpd_commands.L6,			114)
elements["pnt_115"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, L5"),				devices.MFD_CPG_RIGHT, mpd_commands.L5,			115)
elements["pnt_116"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, L4"),				devices.MFD_CPG_RIGHT, mpd_commands.L4,			116)
elements["pnt_117"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, L3"),				devices.MFD_CPG_RIGHT, mpd_commands.L3,			117)
elements["pnt_118"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, L2"),				devices.MFD_CPG_RIGHT, mpd_commands.L2,			118)
elements["pnt_119"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, L1"),				devices.MFD_CPG_RIGHT, mpd_commands.L1,			119)
elements["pnt_129"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, Asterisk"),			devices.MFD_CPG_RIGHT, mpd_commands.Asterisk,	129)
elements["pnt_136"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, VID"),				devices.MFD_CPG_RIGHT, mpd_commands.VID,		136)
elements["pnt_137"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, COM"),				devices.MFD_CPG_RIGHT, mpd_commands.COM,		137)
elements["pnt_138"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, A/C"),				devices.MFD_CPG_RIGHT, mpd_commands.AC,			138)
elements["pnt_145"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, TSD"),				devices.MFD_CPG_RIGHT, mpd_commands.TSD,		145)
elements["pnt_112"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, WPN"),				devices.MFD_CPG_RIGHT, mpd_commands.WPN,		112)
elements["pnt_113"]		= mpd_button(CREW.CPG, _("Right MPD Pushbutton, FCR"),				devices.MFD_CPG_RIGHT, mpd_commands.FCR,		113)

elements["pnt_120"]		= default_rheostat(CREW.CPG, _("Right MPD Brightness Control Knob"),		devices.MFD_CPG_RIGHT, mpd_commands.BRT_KNOB,	120)
elements["pnt_121"]		= default_rheostat(CREW.CPG, _("Right MPD Video Control Knob"),				devices.MFD_CPG_RIGHT, mpd_commands.VID_KNOB,	121)
elements["pnt_128"]		= multiposition_switch(CREW.CPG, _("Right MPD Mode Knob, DAY/NIGHT/MONO"),	devices.MFD_CPG_RIGHT, mpd_commands.MODE_KNOB,	128, 3, 0.5, IS_INVERSED, 0.0, anim_speed_default * 0.5, NOT_CYCLED)


-- ExternalLightSystem
elements["pnt_326"]		= default_3_position_tumb(CREW.PLT, _("Navigation Lights Switch, BRT/OFF/DIM"),			devices.EXTLIGHTS_SYSTEM,	extlights_commands.NavLights,			326)
elements["pnt_329"]		= default_rheostat(CREW.PLT, _("Formation Lights Control Knob"),						devices.EXTLIGHTS_SYSTEM,	extlights_commands.FormationLights,		329)
elements["pnt_332"]		= default_3_position_tumb(CREW.PLT, _("Anti-Collision Lights Switch, WHT/OFF/RED"),		devices.EXTLIGHTS_SYSTEM,	extlights_commands.AntiCollLights,		332)

-- InternalLightSystem
elements["pnt_305"]		= lighted_pushbutton(CREW.PLT, _("Master Caution Button"),								devices.CPTLIGHTS_SYSTEM,	intlights_commands.MasterCautionPLT,	305)
elements["pnt_304"]		= lighted_pushbutton(CREW.PLT, _("Master Warning Button"),								devices.CPTLIGHTS_SYSTEM,	intlights_commands.MasterWarningPLT,	304)
elements["pnt_807"]		= lighted_pushbutton(CREW.CPG, _("Master Caution Button"),								devices.CPTLIGHTS_SYSTEM,	intlights_commands.MasterCautionCPG,	807)
elements["pnt_805"]		= lighted_pushbutton(CREW.CPG, _("Master Warning Button"),								devices.CPTLIGHTS_SYSTEM,	intlights_commands.MasterWarningCPG,	805)
elements["pnt_333"]		= default_button(CREW.PLT, _("Press To Test Button"),									devices.CPTLIGHTS_SYSTEM,	intlights_commands.TestLightsPLT,		333)
elements["pnt_327"]		= default_rheostat(CREW.PLT, _("Signal Lights Control Knob"),							devices.CPTLIGHTS_SYSTEM,	intlights_commands.SignalPLT,			327)
elements["pnt_328"]		= default_rheostat(CREW.PLT, _("Primary Lights Control Knob"),							devices.CPTLIGHTS_SYSTEM,	intlights_commands.PrimaryPLT,			328)
elements["pnt_330"]		= default_rheostat(CREW.PLT, _("Flood Lights Control Knob"),							devices.CPTLIGHTS_SYSTEM,	intlights_commands.FloodPLT,			330)
elements["pnt_331"]		= default_rheostat(CREW.PLT, _("Standby Instrument Lights Control Knob"),				devices.CPTLIGHTS_SYSTEM,	intlights_commands.StbyInstPLT,			331)
--elements[""]			= default_rheostat(CREW.PLT, _("Utility Lights Rheostat Control"),						devices.CPTLIGHTS_SYSTEM,	intlights_commands.UtilityPLT,			)
--elements[""]			= default_button(CREW.PLT, _("Press To Hold Brt Button"),								devices.CPTLIGHTS_SYSTEM,	intlights_commands.UtilityButtonPLT,	)
elements["pnt_367"]		= default_button(CREW.CPG, _("Press To Test Button"),									devices.CPTLIGHTS_SYSTEM,	intlights_commands.TestLightsCPG,		367)
elements["pnt_364"]		= default_rheostat(CREW.CPG, _("Signal Lights Control Knob"),							devices.CPTLIGHTS_SYSTEM,	intlights_commands.SignalCPG,			364)
elements["pnt_365"]		= default_rheostat(CREW.CPG, _("Primary Lights Control Knob"),							devices.CPTLIGHTS_SYSTEM,	intlights_commands.PrimaryCPG,			365)
elements["pnt_366"]		= default_rheostat(CREW.CPG, _("Flood Lights Control Knob"),							devices.CPTLIGHTS_SYSTEM,	intlights_commands.FloodCPG,			366)
--elements[""]			= default_rheostat(CREW.CPG, _("Utility Lights Rheostat Control"),						devices.CPTLIGHTS_SYSTEM,	intlights_commands.UtilityCPG,			)
--elements[""]			= default_button(CREW.CPG, _("Press To Hold Brt Button"),								devices.CPTLIGHTS_SYSTEM,	intlights_commands.UtilityButtonCPG,	)

-- Standby Attitude Indicator
elements["pnt_619"]		= default_button_axis(CREW.PLT, _("SAI Cage Knob, (LMB) Pull to cage /(MW) Adjust aircraft reference symbol"),	devices.SAI, sai_commands.CageKnobPull, sai_commands.CageKnobRotate, 620, 619)

-- Standby Altimeter
elements["pnt_477"]		= default_axis_limited(CREW.PLT, _("Altimeter Pressure Setting Knob"),					devices.BARO_ALTIMETER,	baro_alt_commands.PressureSet,			477, 0.0, 0.01)

-- COMM Panel (FRONT)
elements["pnt_375"]		= comm_knob(CREW.CPG, _("VHF Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.VHF_volume,	comm_commands.VHF_disable,		375, 459)
elements["pnt_376"]		= comm_knob(CREW.CPG, _("UHF Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.UHF_volume,	comm_commands.UHF_disable,		376, 460)
elements["pnt_377"]		= comm_knob(CREW.CPG, _("FM1 Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.FM1_volume,	comm_commands.FM1_disable,		377, 461)
elements["pnt_378"]		= comm_knob(CREW.CPG, _("FM2 Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.FM2_volume,	comm_commands.FM2_disable,		378, 462)
elements["pnt_379"]		= comm_knob(CREW.CPG, _("HF Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.HF_volume,	comm_commands.HF_disable,		379, 463)
elements["pnt_389"]		= default_rheostat(CREW.CPG, _("IFF Volume Control Knob, Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.IFF_volume,		389 )		
--= comm_knob(CREW.CPG, _("IFF Volume Control Knob, (LMB) Pull to disable / (MW) Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.IFF_volume,	comm_commands.IFF_disable,		389, 464)
elements["pnt_390"]		= default_rheostat(CREW.CPG, _("RLWR Volume Control Knob, Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.RLWR_volume,	390)
--= comm_knob(CREW.CPG, _("RLWR Volume Control Knob, (LMB) Pull to disable / (MW) Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.RLWR_volume,	comm_commands.RLWR_disable,		390, 465)
elements["pnt_391"]		= default_rheostat(CREW.CPG, _("ATA Volume Control Knob, Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.ATA_volume,		391)
--= comm_knob(CREW.CPG, _("ATA Volume Control Knob, (LMB) Pull to disable / (MW) Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.ATA_volume,	comm_commands.ATA_disable,		391, 466)
elements["pnt_392"]		= comm_knob(CREW.CPG, _("VCR Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.VCR_volume,	comm_commands.VCR_disable,		392, 467)
elements["pnt_393"]		= comm_knob(CREW.CPG, _("ADF Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_CPG, comm_commands.ADF_volume,	comm_commands.ADF_disable,		393, 468)

elements["pnt_385"]		= default_rheostat(CREW.CPG, _("MASTER Volume Control Knob"),					devices.COMM_PANEL_CPG,	comm_commands.MASTER_volume,	385)
elements["pnt_386"]		= default_rheostat(CREW.CPG, _("SENS Control Knob"),							devices.COMM_PANEL_CPG,	comm_commands.SensControl,		386)
elements["pnt_380"]		= springloaded_3_pos_tumb(CREW.CPG, _("VHF SQL Switch, ON/OFF"),				devices.COMM_PANEL_CPG,	comm_commands.VHF_SQL_OFF, comm_commands.VHF_SQL_ON,  	380)
elements["pnt_381"]		= springloaded_3_pos_tumb(CREW.CPG, _("UHF SQL Switch, ON/OFF"),				devices.COMM_PANEL_CPG,	comm_commands.UHF_SQL_OFF, comm_commands.UHF_SQL_ON,	381)
elements["pnt_382"]		= springloaded_3_pos_tumb(CREW.CPG, _("FM1 SQL Switch, ON/OFF"),				devices.COMM_PANEL_CPG,	comm_commands.FM1_SQL_OFF, comm_commands.FM1_SQL_ON,	382)
elements["pnt_383"]		= springloaded_3_pos_tumb(CREW.CPG, _("FM2 SQL Switch, ON/OFF"),				devices.COMM_PANEL_CPG,	comm_commands.FM2_SQL_OFF, comm_commands.FM2_SQL_ON,	383)
elements["pnt_384"]		= springloaded_3_pos_tumb(CREW.CPG, _("HF SQL Switch, ON/OFF"),					devices.COMM_PANEL_CPG,	comm_commands.HF_SQL_OFF,  comm_commands.HF_SQL_ON,	   384)
elements["pnt_387"]		= default_3_position_tumb(CREW.CPG, _("ICS Mode Switch, HOT MIC/VOX/PTT"),		devices.COMM_PANEL_CPG,	comm_commands.ICS_MODE,			387, NOT_CYCLED, anim_speed_default, NOT_INVERSED)
elements["pnt_388"]		= default_button(CREW.CPG, _("IDENT Button"),									devices.COMM_PANEL_CPG,	comm_commands.IDENT,			388)

-- COMM Panel (REAR)
elements["pnt_334"]		= comm_knob(CREW.PLT, _("VHF Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.VHF_volume,	comm_commands.VHF_disable,		334, 449)
elements["pnt_335"]		= comm_knob(CREW.PLT, _("UHF Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.UHF_volume,	comm_commands.UHF_disable,		335, 450)
elements["pnt_336"]		= comm_knob(CREW.PLT, _("FM1 Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.FM1_volume,	comm_commands.FM1_disable,		336, 451)
elements["pnt_337"]		= comm_knob(CREW.PLT, _("FM2 Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.FM2_volume,	comm_commands.FM2_disable,		337, 452)
elements["pnt_338"]		= comm_knob(CREW.PLT, _("HF Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.HF_volume,	comm_commands.HF_disable,		338, 453)
elements["pnt_348"]		= default_rheostat(CREW.PLT,  _("IFF Volume Control Knob,  Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.IFF_volume,	348)
--= comm_knob(CREW.PLT, _("IFF Volume Control Knob, (LMB) Pull to disable / (MW) Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.IFF_volume,	comm_commands.IFF_disable,		348, 454)
elements["pnt_349"]		= default_rheostat(CREW.PLT, _("RLWR Volume Control Knob, Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.RLWR_volume,	349)
--= comm_knob(CREW.PLT, _("RLWR Volume Control Knob, (LMB) Pull to disable / (MW) Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.RLWR_volume,	comm_commands.RLWR_disable,		349, 455)
elements["pnt_350"]		= default_rheostat(CREW.PLT, _("ATA Volume Control Knob,  Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.ATA_volume,	350)
--= comm_knob(CREW.PLT, _("ATA Volume Control Knob, (LMB) Pull to disable / (MW) Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.ATA_volume,	comm_commands.ATA_disable,		350, 456)
elements["pnt_351"]		= comm_knob(CREW.PLT, _("VCR Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.VCR_volume,	comm_commands.VCR_disable,		351, 457)
elements["pnt_352"]		= comm_knob(CREW.PLT, _("ADF Volume Control Knob, RMB-Pull to mute/LMB-stow/MW-Rotate to adjust volume"),	devices.COMM_PANEL_PLT, comm_commands.ADF_volume,	comm_commands.ADF_disable,		352, 458)

elements["pnt_344"]		= default_rheostat(CREW.PLT, _("MASTER Volume Control Knob"),					devices.COMM_PANEL_PLT,	comm_commands.MASTER_volume,	344)
elements["pnt_345"]		= default_rheostat(CREW.PLT, _("SENS Control Knob"),							devices.COMM_PANEL_PLT,	comm_commands.SensControl,		345)
elements["pnt_339"]		= springloaded_3_pos_tumb(CREW.PLT, _("VHF Squelch Switch, ON/OFF"),			devices.COMM_PANEL_PLT,	comm_commands.VHF_SQL_OFF, comm_commands.VHF_SQL_ON,	339)
elements["pnt_340"]		= springloaded_3_pos_tumb(CREW.PLT, _("UHF Squelch Switch, ON/OFF"),			devices.COMM_PANEL_PLT,	comm_commands.UHF_SQL_OFF, comm_commands.UHF_SQL_ON,	340)
elements["pnt_341"]		= springloaded_3_pos_tumb(CREW.PLT, _("FM1 Squelch Switch, ON/OFF"),			devices.COMM_PANEL_PLT,	comm_commands.FM1_SQL_OFF, comm_commands.FM1_SQL_ON,	341)
elements["pnt_342"]		= springloaded_3_pos_tumb(CREW.PLT, _("FM2 Squelch Switch, ON/OFF"),			devices.COMM_PANEL_PLT,	comm_commands.FM2_SQL_OFF, comm_commands.FM2_SQL_ON,	342)
elements["pnt_343"]		= springloaded_3_pos_tumb(CREW.PLT, _("HF Squelch Switch, ON/OFF"),				devices.COMM_PANEL_PLT,	comm_commands.HF_SQL_OFF,  comm_commands.HF_SQL_ON, 	343)
elements["pnt_346"]		= default_3_position_tumb(CREW.PLT, _("ICS Mode Switch, HOT MIC/VOX/PTT"),		devices.COMM_PANEL_PLT,	comm_commands.ICS_MODE,			346, NOT_CYCLED, anim_speed_default, NOT_INVERSED)
elements["pnt_347"]		= default_button(CREW.PLT, _("IDENT Button"),									devices.COMM_PANEL_PLT,	comm_commands.IDENT,			347)

-- emergency panel (PLT)
elements["pnt_310"]		= lighted_pushbutton(CREW.PLT, _("Guard Button, ON/OFF"),					devices.EMERGENCY_PANEL,	intercom_commands.PLT_UHF_GUARD_Btn,			310)
elements["pnt_311"]		= lighted_pushbutton(CREW.PLT, _("XPNDR Button, ON/OFF"),					devices.EMERGENCY_PANEL,	intercom_commands.PLT_XPNDR_Btn,				311)
elements["pnt_312"]		= default_2_position_tumb(CREW.PLT, _("Zeroize Switch, ON/OFF"),			devices.EMERGENCY_PANEL,	intercom_commands.PLT_ZEROIZE_Sw,				312)
-- instrument panel (PLT)
elements["pnt_803"]		= default_red_cover(CREW.PLT, _("Master Zeroize Switch Cover, OPEN/CLOSE"),	devices.EMERGENCY_PANEL,	intercom_commands.PLT_MasterZeroizeSwCover,		803)
elements["pnt_804"]		= default_2_position_tumb(CREW.PLT, _("Master Zeroize Switch, ON/OFF"),		devices.EMERGENCY_PANEL,	intercom_commands.PLT_MasterZeroizeSw,			804)


-- emergency panel (CPG)
elements["pnt_358"]		= lighted_pushbutton(CREW.CPG, _("Guard Button, ON/OFF"),					devices.EMERGENCY_PANEL,	intercom_commands.CPG_UHF_GUARD_Btn,			358)
elements["pnt_359"]		= lighted_pushbutton(CREW.CPG, _("XPNDR Button, ON/OFF"),					devices.EMERGENCY_PANEL,	intercom_commands.CPG_XPNDR_Btn,				359)
elements["pnt_360"]		= default_2_position_tumb(CREW.CPG, _("Zeroize Switch, ON/OFF"),			devices.EMERGENCY_PANEL,	intercom_commands.CPG_ZEROIZE_Sw,				360)
-- instrument panel (CPG)
elements["pnt_801"]		= default_red_cover(CREW.CPG, _("Master Zeroize Switch Cover, OPEN/CLOSE"),	devices.EMERGENCY_PANEL,	intercom_commands.CPG_MasterZeroizeSwCover,		801)
elements["pnt_802"]		= default_2_position_tumb(CREW.CPG, _("Master Zeroize Switch, ON/OFF"),		devices.EMERGENCY_PANEL,	intercom_commands.CPG_MasterZeroizeSw,			802)


---- Keyboard Unit
-- CPG
elements["pnt_164"]		= ku_button(CREW.CPG, _("KU Key, A"),								devices.KU_CPG,	KU_commands.keyA,			164)
elements["pnt_165"]		= ku_button(CREW.CPG, _("KU Key, B"),								devices.KU_CPG,	KU_commands.keyB,			165)
elements["pnt_166"]		= ku_button(CREW.CPG, _("KU Key, C"),								devices.KU_CPG,	KU_commands.keyC,			166)
elements["pnt_167"]		= ku_button(CREW.CPG, _("KU Key, D"),								devices.KU_CPG,	KU_commands.keyD,			167)
elements["pnt_168"]		= ku_button(CREW.CPG, _("KU Key, E"),								devices.KU_CPG,	KU_commands.keyE,			168)
elements["pnt_169"]		= ku_button(CREW.CPG, _("KU Key, F"),								devices.KU_CPG,	KU_commands.keyF,			169)
elements["pnt_173"]		= ku_button(CREW.CPG, _("KU Key, G"),								devices.KU_CPG,	KU_commands.keyG,			173)
elements["pnt_174"]		= ku_button(CREW.CPG, _("KU Key, H"),								devices.KU_CPG,	KU_commands.keyH,			174)
elements["pnt_175"]		= ku_button(CREW.CPG, _("KU Key, I"),								devices.KU_CPG,	KU_commands.keyI,			175)
elements["pnt_176"]		= ku_button(CREW.CPG, _("KU Key, J"),								devices.KU_CPG,	KU_commands.keyJ,			176)
elements["pnt_177"]		= ku_button(CREW.CPG, _("KU Key, K"),								devices.KU_CPG,	KU_commands.keyK,			177)
elements["pnt_178"]		= ku_button(CREW.CPG, _("KU Key, L"),								devices.KU_CPG,	KU_commands.keyL,			178)
elements["pnt_182"]		= ku_button(CREW.CPG, _("KU Key, M"),								devices.KU_CPG,	KU_commands.keyM,			182)
elements["pnt_183"]		= ku_button(CREW.CPG, _("KU Key, N"),								devices.KU_CPG,	KU_commands.keyN,			183)
elements["pnt_184"]		= ku_button(CREW.CPG, _("KU Key, O"),								devices.KU_CPG,	KU_commands.keyO,			184)
elements["pnt_185"]		= ku_button(CREW.CPG, _("KU Key, P"),								devices.KU_CPG,	KU_commands.keyP,			185)
elements["pnt_186"]		= ku_button(CREW.CPG, _("KU Key, Q"),								devices.KU_CPG,	KU_commands.keyQ,			186)
elements["pnt_187"]		= ku_button(CREW.CPG, _("KU Key, R"),								devices.KU_CPG,	KU_commands.keyR,			187)
elements["pnt_191"]		= ku_button(CREW.CPG, _("KU Key, S"),								devices.KU_CPG,	KU_commands.keyS,			191)
elements["pnt_192"]		= ku_button(CREW.CPG, _("KU Key, T"),								devices.KU_CPG,	KU_commands.keyT,			192)
elements["pnt_193"]		= ku_button(CREW.CPG, _("KU Key, U"),								devices.KU_CPG,	KU_commands.keyU,			193)
elements["pnt_194"]		= ku_button(CREW.CPG, _("KU Key, V"),								devices.KU_CPG,	KU_commands.keyV,			194)
elements["pnt_195"]		= ku_button(CREW.CPG, _("KU Key, W"),								devices.KU_CPG,	KU_commands.keyW,			195)
elements["pnt_196"]		= ku_button(CREW.CPG, _("KU Key, X"),								devices.KU_CPG,	KU_commands.keyX,			196)
elements["pnt_200"]		= ku_button(CREW.CPG, _("KU Key, Y"),								devices.KU_CPG,	KU_commands.keyY,			200)
elements["pnt_201"]		= ku_button(CREW.CPG, _("KU Key, Z"),								devices.KU_CPG,	KU_commands.keyZ,			201)
elements["pnt_202"]		= ku_button(CREW.CPG, _("KU Key, /"),								devices.KU_CPG,	KU_commands.keySlash,		202)

elements["pnt_198"]		= ku_button(CREW.CPG, _("KU Key, 0"),								devices.KU_CPG,	KU_commands.key0,			198)
elements["pnt_170"]		= ku_button(CREW.CPG, _("KU Key, 1"),								devices.KU_CPG,	KU_commands.key1,			170)
elements["pnt_171"]		= ku_button(CREW.CPG, _("KU Key, 2"),								devices.KU_CPG,	KU_commands.key2,			171)
elements["pnt_172"]		= ku_button(CREW.CPG, _("KU Key, 3"),								devices.KU_CPG,	KU_commands.key3,			172)
elements["pnt_179"]		= ku_button(CREW.CPG, _("KU Key, 4"),								devices.KU_CPG,	KU_commands.key4,			179)
elements["pnt_180"]		= ku_button(CREW.CPG, _("KU Key, 5"),								devices.KU_CPG,	KU_commands.key5,			180)
elements["pnt_181"]		= ku_button(CREW.CPG, _("KU Key, 6"),								devices.KU_CPG,	KU_commands.key6,			181)
elements["pnt_188"]		= ku_button(CREW.CPG, _("KU Key, 7"),								devices.KU_CPG,	KU_commands.key7,			188)
elements["pnt_189"]		= ku_button(CREW.CPG, _("KU Key, 8"),								devices.KU_CPG,	KU_commands.key8,			189)
elements["pnt_190"]		= ku_button(CREW.CPG, _("KU Key, 9"),								devices.KU_CPG,	KU_commands.key9,			190)

elements["pnt_197"]		= ku_button(CREW.CPG, _("KU Key, ."),								devices.KU_CPG,	KU_commands.keyDot,			197)
elements["pnt_199"]		= ku_button(CREW.CPG, _("KU Key, +/-"),								devices.KU_CPG,	KU_commands.keySign,		199)
elements["pnt_203"]		= ku_button(CREW.CPG, _("KU Key, BKS"),								devices.KU_CPG,	KU_commands.keyBKS,			203)
elements["pnt_204"]		= ku_button(CREW.CPG, _("KU Key, SPC"),								devices.KU_CPG,	KU_commands.keySPC,			204)
	
elements["pnt_205"]		= ku_button(CREW.CPG, _("KU Key, *"),								devices.KU_CPG,	KU_commands.keyMultiply,	205)
elements["pnt_206"]		= ku_button(CREW.CPG, _("KU Key, / ('divide')"),					devices.KU_CPG,	KU_commands.keyDivide,		206)
elements["pnt_207"]		= ku_button(CREW.CPG, _("KU Key, +"),								devices.KU_CPG,	KU_commands.keyPlus,		207)
elements["pnt_208"]		= ku_button(CREW.CPG, _("KU Key, -"),								devices.KU_CPG,	KU_commands.keyMinus,		208)

elements["pnt_209"]		= ku_button(CREW.CPG, _("KU Key, CLR"),								devices.KU_CPG,	KU_commands.keyCLR,			209)
elements["pnt_210"]		= ku_button(CREW.CPG, _("KU Key, Left"),							devices.KU_CPG,	KU_commands.keyLeft,		210)
elements["pnt_211"]		= ku_button(CREW.CPG, _("KU Key, Right"),							devices.KU_CPG,	KU_commands.keyRight,		211)
elements["pnt_212"]		= ku_button(CREW.CPG, _("KU Key, ENTER"),							devices.KU_CPG,	KU_commands.keyEnter,		212)

elements["pnt_621"]		= default_rheostat(CREW.CPG, _("KU Scratchpad Brightness Knob"),	devices.KU_CPG,	KU_commands.BrightnessKnob,	621)

-- Pilot
elements["pnt_213"]		= ku_button(CREW.PLT, _("KU Key, A"),								devices.KU_PLT,	KU_commands.keyA,			213)
elements["pnt_214"]		= ku_button(CREW.PLT, _("KU Key, B"),								devices.KU_PLT,	KU_commands.keyB,			214)
elements["pnt_215"]		= ku_button(CREW.PLT, _("KU Key, C"),								devices.KU_PLT,	KU_commands.keyC,			215)
elements["pnt_216"]		= ku_button(CREW.PLT, _("KU Key, D"),								devices.KU_PLT,	KU_commands.keyD,			216)
elements["pnt_217"]		= ku_button(CREW.PLT, _("KU Key, E"),								devices.KU_PLT,	KU_commands.keyE,			217)
elements["pnt_218"]		= ku_button(CREW.PLT, _("KU Key, F"),								devices.KU_PLT,	KU_commands.keyF,			218)
elements["pnt_222"]		= ku_button(CREW.PLT, _("KU Key, G"),								devices.KU_PLT,	KU_commands.keyG,			222)
elements["pnt_223"]		= ku_button(CREW.PLT, _("KU Key, H"),								devices.KU_PLT,	KU_commands.keyH,			223)
elements["pnt_224"]		= ku_button(CREW.PLT, _("KU Key, I"),								devices.KU_PLT,	KU_commands.keyI,			224)
elements["pnt_225"]		= ku_button(CREW.PLT, _("KU Key, J"),								devices.KU_PLT,	KU_commands.keyJ,			225)
elements["pnt_226"]		= ku_button(CREW.PLT, _("KU Key, K"),								devices.KU_PLT,	KU_commands.keyK,			226)
elements["pnt_227"]		= ku_button(CREW.PLT, _("KU Key, L"),								devices.KU_PLT,	KU_commands.keyL,			227)
elements["pnt_231"]		= ku_button(CREW.PLT, _("KU Key, M"),								devices.KU_PLT,	KU_commands.keyM,			231)
elements["pnt_232"]		= ku_button(CREW.PLT, _("KU Key, N"),								devices.KU_PLT,	KU_commands.keyN,			232)
elements["pnt_233"]		= ku_button(CREW.PLT, _("KU Key, O"),								devices.KU_PLT,	KU_commands.keyO,			233)
elements["pnt_234"]		= ku_button(CREW.PLT, _("KU Key, P"),								devices.KU_PLT,	KU_commands.keyP,			234)
elements["pnt_235"]		= ku_button(CREW.PLT, _("KU Key, Q"),								devices.KU_PLT,	KU_commands.keyQ,			235)
elements["pnt_236"]		= ku_button(CREW.PLT, _("KU Key, R"),								devices.KU_PLT,	KU_commands.keyR,			236)
elements["pnt_240"]		= ku_button(CREW.PLT, _("KU Key, S"),								devices.KU_PLT,	KU_commands.keyS,			240)
elements["pnt_241"]		= ku_button(CREW.PLT, _("KU Key, T"),								devices.KU_PLT,	KU_commands.keyT,			241)
elements["pnt_242"]		= ku_button(CREW.PLT, _("KU Key, U"),								devices.KU_PLT,	KU_commands.keyU,			242)
elements["pnt_243"]		= ku_button(CREW.PLT, _("KU Key, V"),								devices.KU_PLT,	KU_commands.keyV,			243)
elements["pnt_244"]		= ku_button(CREW.PLT, _("KU Key, W"),								devices.KU_PLT,	KU_commands.keyW,			244)
elements["pnt_245"]		= ku_button(CREW.PLT, _("KU Key, X"),								devices.KU_PLT,	KU_commands.keyX,			245)
elements["pnt_249"]		= ku_button(CREW.PLT, _("KU Key, Y"),								devices.KU_PLT,	KU_commands.keyY,			249)
elements["pnt_250"]		= ku_button(CREW.PLT, _("KU Key, Z"),								devices.KU_PLT,	KU_commands.keyZ,			250)
elements["pnt_251"]		= ku_button(CREW.PLT, _("KU Key, /"),								devices.KU_PLT,	KU_commands.keySlash,		251)

elements["pnt_247"]		= ku_button(CREW.PLT, _("KU Key, 0"),								devices.KU_PLT,	KU_commands.key0,			247)
elements["pnt_219"]		= ku_button(CREW.PLT, _("KU Key, 1"),								devices.KU_PLT,	KU_commands.key1,			219)
elements["pnt_220"]		= ku_button(CREW.PLT, _("KU Key, 2"),								devices.KU_PLT,	KU_commands.key2,			220)
elements["pnt_221"]		= ku_button(CREW.PLT, _("KU Key, 3"),								devices.KU_PLT,	KU_commands.key3,			221)
elements["pnt_228"]		= ku_button(CREW.PLT, _("KU Key, 4"),								devices.KU_PLT,	KU_commands.key4,			228)
elements["pnt_229"]		= ku_button(CREW.PLT, _("KU Key, 5"),								devices.KU_PLT,	KU_commands.key5,			229)
elements["pnt_230"]		= ku_button(CREW.PLT, _("KU Key, 6"),								devices.KU_PLT,	KU_commands.key6,			230)
elements["pnt_237"]		= ku_button(CREW.PLT, _("KU Key, 7"),								devices.KU_PLT,	KU_commands.key7,			237)
elements["pnt_238"]		= ku_button(CREW.PLT, _("KU Key, 8"),								devices.KU_PLT,	KU_commands.key8,			238)
elements["pnt_239"]		= ku_button(CREW.PLT, _("KU Key, 9"),								devices.KU_PLT,	KU_commands.key9,			239)

elements["pnt_246"]		= ku_button(CREW.PLT, _("KU Key, ."),								devices.KU_PLT,	KU_commands.keyDot,			246)
elements["pnt_248"]		= ku_button(CREW.PLT, _("KU Key, +/-"),								devices.KU_PLT,	KU_commands.keySign,		248)
elements["pnt_252"]		= ku_button(CREW.PLT, _("KU Key, BKS"),								devices.KU_PLT,	KU_commands.keyBKS,			252)
elements["pnt_253"]		= ku_button(CREW.PLT, _("KU Key, SPC"),								devices.KU_PLT,	KU_commands.keySPC,			253)
	
elements["pnt_254"]		= ku_button(CREW.PLT, _("KU Key, *"),								devices.KU_PLT,	KU_commands.keyMultiply,	254)
elements["pnt_255"]		= ku_button(CREW.PLT, _("KU Key, / (divide)"),					devices.KU_PLT,	KU_commands.keyDivide,		255)
elements["pnt_256"]		= ku_button(CREW.PLT, _("KU Key, +"),								devices.KU_PLT,	KU_commands.keyPlus,		256)
elements["pnt_257"]		= ku_button(CREW.PLT, _("KU Key, -"),								devices.KU_PLT,	KU_commands.keyMinus,		257)

elements["pnt_258"]		= ku_button(CREW.PLT, _("KU Key, CLR"),								devices.KU_PLT,	KU_commands.keyCLR,			258)
elements["pnt_259"]		= ku_button(CREW.PLT, _("KU Key, Left"),							devices.KU_PLT,	KU_commands.keyLeft,		259)
elements["pnt_260"]		= ku_button(CREW.PLT, _("KU Key, Right"),							devices.KU_PLT,	KU_commands.keyRight,		260)
elements["pnt_261"]		= ku_button(CREW.PLT, _("KU Key, ENTER"),							devices.KU_PLT,	KU_commands.keyEnter,		261)

elements["pnt_316"]		= default_rheostat(CREW.PLT, _("KU Scratchpad Brightness Knob"),	devices.KU_PLT, KU_commands.BrightnessKnob,	316)

-- Enhanced Up-Front Display Pilot
elements["pnt_271-2"]	= Rocker_switch_positive(CREW.PLT, _("WCA Rocker Switch, Up"),		devices.EUFD_PLT,	eufd_commands.WCA_UP,	271)
elements["pnt_271-1"]	= Rocker_switch_negative(CREW.PLT, _("WCA Rocker Switch, Down"),	devices.EUFD_PLT,	eufd_commands.WCA_DOWN,	271)
elements["pnt_270-2"]	= Rocker_switch_positive(CREW.PLT, _("IDM Rocker Switch, Up"),		devices.EUFD_PLT,	eufd_commands.IDM_UP,	270)
elements["pnt_270-1"]	= Rocker_switch_negative(CREW.PLT, _("IDM Rocker Switch, Down"),	devices.EUFD_PLT,	eufd_commands.IDM_DOWN,	270)
elements["pnt_272-2"]	= Rocker_switch_positive(CREW.PLT, _("RTS Rocker Switch, Up"),		devices.EUFD_PLT,	eufd_commands.RTS_UP,	272)
elements["pnt_272-1"]	= Rocker_switch_negative(CREW.PLT, _("RTS Rocker Switch, Down"),	devices.EUFD_PLT,	eufd_commands.RTS_DOWN,	272)
elements["pnt_273"]		= default_rheostat(CREW.PLT, _("Brightness Control Knob"),			devices.EUFD_PLT,	eufd_commands.BRT,		273)
elements["pnt_275"]		= eufd_button(CREW.PLT, _("Enter Button"),							devices.EUFD_PLT,	eufd_commands.Enter,	275)
elements["pnt_277"]		= eufd_button(CREW.PLT, _("Swap Button"),							devices.EUFD_PLT,	eufd_commands.Swap,		277)
elements["pnt_274"]		= eufd_button(CREW.PLT, _("Preset Button - Press to toggle preset window"),			devices.EUFD_PLT,	eufd_commands.Preset,	274)
elements["pnt_276"]		= eufd_button(CREW.PLT, _("Stopwatch Button - Press to start/stop, Hold to reset"),	devices.EUFD_PLT,	eufd_commands.Stopwatch,276)

-- Enhanced Up-Front Display CPG
elements["pnt_263-2"]	= Rocker_switch_positive(CREW.CPG, _("WCA Rocker Switch, Up"),		devices.EUFD_CPG,	eufd_commands.WCA_UP,	263)
elements["pnt_263-1"]	= Rocker_switch_negative(CREW.CPG, _("WCA Rocker Switch, Down"),	devices.EUFD_CPG,	eufd_commands.WCA_DOWN,	263)
elements["pnt_262-2"]	= Rocker_switch_positive(CREW.CPG, _("IDM Rocker Switch, Up"),		devices.EUFD_CPG,	eufd_commands.IDM_UP,	262)
elements["pnt_262-1"]	= Rocker_switch_negative(CREW.CPG, _("IDM Rocker Switch, Down"),	devices.EUFD_CPG,	eufd_commands.IDM_DOWN,	262)
elements["pnt_264-2"]	= Rocker_switch_positive(CREW.CPG, _("RTS Rocker Switch, Up"),		devices.EUFD_CPG,	eufd_commands.RTS_UP,	264)
elements["pnt_264-1"]	= Rocker_switch_negative(CREW.CPG, _("RTS Rocker Switch, Down"),	devices.EUFD_CPG,	eufd_commands.RTS_DOWN,	264)
elements["pnt_265"]		= default_rheostat(CREW.CPG, _("Brightness Control Knob"),			devices.EUFD_CPG,	eufd_commands.BRT,		265)
elements["pnt_267"]		= eufd_button(CREW.CPG, _("Enter Button"),							devices.EUFD_CPG,	eufd_commands.Enter,	267)
elements["pnt_269"]		= eufd_button(CREW.CPG, _("Swap Button"),							devices.EUFD_CPG,	eufd_commands.Swap,		269)
elements["pnt_266"]		= eufd_button(CREW.CPG, _("Preset Button - Press to toggle preset window"),			devices.EUFD_CPG,	eufd_commands.Preset,	266)
elements["pnt_268"]		= eufd_button(CREW.CPG, _("Stopwatch Button - Press to start/stop, Hold to reset"),	devices.EUFD_CPG,	eufd_commands.Stopwatch,268)

-- TEDAC Display
elements["pnt_150"]		= mpd_button(CREW.CPG, _("TAD Video Select Button - Press to select TADS as the video source"),						devices.TEDAC, tedac_commands.TDU_VIDEO_SELECT_TAD_BTN,		150)
elements["pnt_151"]		= mpd_button(CREW.CPG, _("FCR Video Select Button - Press to select FCR targeting format"),							devices.TEDAC, tedac_commands.TDU_VIDEO_SELECT_FCR_BTN,		151)
elements["pnt_152"]		= mpd_button(CREW.CPG, _("PNV Video Select Button - Press to select PNVS as the video source"),						devices.TEDAC, tedac_commands.TDU_VIDEO_SELECT_PNV_BTN,		152)
elements["pnt_153"]		= mpd_button(CREW.CPG, _("G/S Video Select Button - Press to activate grayscale for the video display"),			devices.TEDAC, tedac_commands.TDU_VIDEO_SELECT_GS_BTN,		153)
elements["pnt_155-1"]	= Rocker_switch_negative(CREW.CPG, _("SYM Rocker Switch, Down/Decrease"),											devices.TEDAC, tedac_commands.TDU_SYM_DEC,					155)
elements["pnt_155-2"]	= Rocker_switch_positive(CREW.CPG, _("SYM Rocker Switch, Up/Increase"),												devices.TEDAC, tedac_commands.TDU_SYM_INC,					155)
elements["pnt_156-1"]	= Rocker_switch_negative(CREW.CPG, _("BRT Rocker Switch, Down/Decrease"),											devices.TEDAC, tedac_commands.TDU_BRT_DEC,					156)
elements["pnt_156-2"]	= Rocker_switch_positive(CREW.CPG, _("BRT Rocker Switch, Up/Increase"),												devices.TEDAC, tedac_commands.TDU_BRT_INC,					156)
elements["pnt_157-1"]	= Rocker_switch_negative(CREW.CPG, _("CON Rocker Switch, Down/Decrease"),											devices.TEDAC, tedac_commands.TDU_CON_DEC,					157)
elements["pnt_157-2"]	= Rocker_switch_positive(CREW.CPG, _("CON Rocker Switch, Up/Increase"),												devices.TEDAC, tedac_commands.TDU_CON_INC,					157)
elements["pnt_147-1"]	= Rocker_switch_negative(CREW.CPG, _("R/F Rocker Switch, Down/Decrease"),											devices.TEDAC, tedac_commands.TDU_RF_DOWN,					147)
elements["pnt_147-2"]	= Rocker_switch_positive(CREW.CPG, _("R/F Rocker Switch, Up/Increase"),												devices.TEDAC, tedac_commands.TDU_RF_UP,					147)
elements["pnt_146-1"]	= Rocker_switch_negative(CREW.CPG, _("EL Adjust Rocker Switch, Down/Decrease"),										devices.TEDAC, tedac_commands.TDU_EL_DOWN,					146)
elements["pnt_146-2"]	= Rocker_switch_positive(CREW.CPG, _("EL Adjust Rocker Switch, Up/Increase"),										devices.TEDAC, tedac_commands.TDU_EL_UP,					146)
elements["pnt_163-1"]	= Rocker_switch_negative(CREW.CPG, _("AZ Adjust Rocker Switch, Left"),												devices.TEDAC, tedac_commands.TDU_AZ_LEFT,					163)
elements["pnt_163-2"]	= Rocker_switch_positive(CREW.CPG, _("AZ Adjust Rocker Switch, Right"),												devices.TEDAC, tedac_commands.TDU_AZ_RIGHT,					163)
elements["pnt_158"]		= mpd_button(CREW.CPG, _("Asterisk (*) Button - Press to adjust the brightness and contrast to nominal settings"),	devices.TEDAC, tedac_commands.TDU_ASTERISK_BTN,				158)
elements["pnt_162"]		= mpd_button(CREW.CPG, _("AZ/EL Boresight Enable Button - Press to enable boresight controls"),						devices.TEDAC, tedac_commands.TDU_B1,						162)
elements["pnt_161"]		= mpd_button(CREW.CPG, _("ACM Button - Press to activate ACM"),														devices.TEDAC, tedac_commands.TDU_B2,						161)
elements["pnt_160"]		= mpd_button(CREW.CPG, _("FREEZE Button - Press to freeze the video imaging on the TDU"),							devices.TEDAC, tedac_commands.TDU_B3,						160)
elements["pnt_159"]		= mpd_button(CREW.CPG, _("FILTER Button - Press to select filter in the TADS FLIR sensor"),							devices.TEDAC, tedac_commands.TDU_B4,						159)
elements["pnt_148"]		= default_rheostat(CREW.CPG, _("FLIR GAIN Control Knob"),															devices.TEDAC, tedac_commands.TDU_GAIN_KNOB,				148)
elements["pnt_149"]		= default_rheostat(CREW.CPG, _("FLIR LEV Control Knob"),															devices.TEDAC, tedac_commands.TDU_LEV_KNOB,					149)
elements["pnt_154"]		= default_3_position_tumb(CREW.CPG, _("Display Mode Knob, DAY/NT/OFF"),		devices.TEDAC, tedac_commands.TDU_MODE_KNOB, 154, NOT_CYCLED, anim_speed_default * 0.5, IS_INVERSED, 0.5, {0,1})
elements["pnt_154"].sound = {{SOUND_SW2}}


-- Left Handgrip (LHG)
elements["pnt_491"]		= springloaded_3_pos_tumb(CREW.CPG, _("Image AutoTrack/Offset Switch, OFS(LMB)/IAT(RMB)"),			devices.TEDAC, tedac_commands.LHG_IAT_OFS_SW_OFS, tedac_commands.LHG_IAT_OFS_SW_IAT, 491)
elements["pnt_491"].side = {}
elements["pnt_492-1"]	= knuppel_button(CREW.CPG, _("TADS FOV Select Switch, Z (Zoom)"),									devices.TEDAC, tedac_commands.LHG_TADS_FOV_SW_Z,			492, 1.0)
elements["pnt_492-1"].side = {}
elements["pnt_492-2"]	= knuppel_button(CREW.CPG, _("TADS FOV Select Switch, M (Medium)"),									devices.TEDAC, tedac_commands.LHG_TADS_FOV_SW_M,			492, -1.0)
elements["pnt_492-2"].side = {}
elements["pnt_493-1"]	= knuppel_button(CREW.CPG, _("TADS FOV Select Switch, N (Narrow)"),									devices.TEDAC, tedac_commands.LHG_TADS_FOV_SW_N,			493, -1.0)
elements["pnt_493-1"].side = {}
elements["pnt_493-2"]	= knuppel_button(CREW.CPG, _("TADS FOV Select Switch, W (Wide)"),									devices.TEDAC, tedac_commands.LHG_TADS_FOV_SW_W,			493, 1.0)
elements["pnt_493-2"].side = {}
elements["pnt_494"]		= default_3_position_tumb(CREW.CPG, _("TADS Sensor Select Switch, FLIR/TV/DVO"),					devices.TEDAC, tedac_commands.LHG_TADS_SENSOR_SELECT_SW, 494, NOT_CYCLED, anim_speed_default, NOT_INVERSED)
elements["pnt_494"].side = {}
elements["pnt_495"]		= springloaded_3_pos_tumb(CREW.CPG, _("STORE/Update Switch, UPDT(LMB)/STORE(RMB)"),					devices.TEDAC, tedac_commands.LHG_STORE_UPDT_SW_UPDT, tedac_commands.LHG_STORE_UPDT_SW_STORE, 495)
elements["pnt_495"].side = {}
elements["pnt_500"]		= springloaded_3_pos_tumb(CREW.CPG, _("FCR Scan Switch, C (Continuous)(LMB)/S (Single)(RMB)"),		devices.TEDAC, tedac_commands.LHG_FCR_SCAN_SW_C, tedac_commands.LHG_FCR_SCAN_SW_S, 500)
elements["pnt_500"].side = {}
elements["pnt_501"]		= default_button(CREW.CPG, _("CUED Search Button - Press to orient the FCR centerline"),			devices.TEDAC, tedac_commands.LHG_CUED_SEARCH_BTN,			501)
elements["pnt_501"].side = {}
elements["pnt_496"]		= default_button(CREW.CPG, _("Linear Motion Compensation (LMC) Button - Press to toggle LMC mode"),	devices.TEDAC, tedac_commands.LHG_LMC_BTN,					496)
elements["pnt_496"].side = {}
elements["pnt_498-1"]	= knuppel_button(CREW.CPG, _("FCR Mode Switch, GTM (Ground Targeting Mode)"),						devices.TEDAC, tedac_commands.LHG_FCR_MODE_SW_UP,			498, 1.0)
elements["pnt_498-1"].side = {}
elements["pnt_498-2"]	= knuppel_button(CREW.CPG, _("FCR Mode Switch, ATM (Air Targeting Mode)"),							devices.TEDAC, tedac_commands.LHG_FCR_MODE_SW_DOWN,			498, -1.0)
elements["pnt_498-2"].side = {}
elements["pnt_499-1"]	= knuppel_button(CREW.CPG, _("FCR Mode Switch, TPM (Terrain Profile Mode)"),						devices.TEDAC, tedac_commands.LHG_FCR_MODE_SW_LEFT,			499, -1.0)
elements["pnt_499-1"].side = {}
elements["pnt_499-2"]	= knuppel_button(CREW.CPG, _("FCR Mode Switch, RMAP (Radar MAP)"),									devices.TEDAC, tedac_commands.LHG_FCR_MODE_SW_RIGHT,		499, 1.0)
elements["pnt_499-2"].side = {}
elements["pnt_502-1"]	= knuppel_button(CREW.CPG, _("Weapons Action (WAS) Switch, GUN"),									devices.TEDAC, tedac_commands.LHG_WEAPONS_ACTION_SW_UP,		502, 1.0)
elements["pnt_502-1"].side = {}
elements["pnt_502-2"]	= knuppel_button(CREW.CPG, _("Weapons Action (WAS) Switch, ATA"),									devices.TEDAC, tedac_commands.LHG_WEAPONS_ACTION_SW_DOWN,	502, -1.0)
elements["pnt_502-2"].side = {}
elements["pnt_503-1"]	= knuppel_button(CREW.CPG, _("Weapons Action (WAS) Switch, RKT"),									devices.TEDAC, tedac_commands.LHG_WEAPONS_ACTION_SW_LEFT,	503, -1.0)
elements["pnt_503-1"].side = {}
elements["pnt_503-2"]	= knuppel_button(CREW.CPG, _("Weapons Action (WAS) Switch, MSL"),									devices.TEDAC, tedac_commands.LHG_WEAPONS_ACTION_SW_RIGHT,	503, 1.0)
elements["pnt_503-2"].side = {}
elements["pnt_487-1"]	= knuppel_button(CREW.CPG, _("Cursor Controller, Up"),												devices.TEDAC, tedac_commands.LHG_CURSOR_UP,				487, 1.0)
elements["pnt_487-1"].side = {}
elements["pnt_487-2"]	= knuppel_button(CREW.CPG, _("Cursor Controller, Down"),											devices.TEDAC, tedac_commands.LHG_CURSOR_DOWN,				487, -1.0)
elements["pnt_487-2"].side = {}
elements["pnt_488-1"]	= knuppel_button(CREW.CPG, _("Cursor Controller, Left"),											devices.TEDAC, tedac_commands.LHG_CURSOR_LEFT,				488, -1.0)
elements["pnt_488-1"].side = {}
elements["pnt_488-2"]	= knuppel_button(CREW.CPG, _("Cursor Controller, Right"),											devices.TEDAC, tedac_commands.LHG_CURSOR_RIGHT,				488, 1.0)
elements["pnt_488-2"].side = {}
elements["pnt_489"]		= knuppel_button(CREW.CPG, _("Cursor Controller, Enter"),											devices.TEDAC, tedac_commands.LHG_CURSOR_ENTER,				489, 1.0)
elements["pnt_489"].side = {}
elements["pnt_490"]		= default_button(CREW.CPG, _("Cursor Display Select (L/R) Button - Press to move the cursor to the center of the opposite MPD"),	devices.TEDAC, tedac_commands.LHG_LR_BTN,	490)
elements["pnt_490"].side = {}

-- Right Handgrip (RHG)
elements["pnt_508-1"]	= knuppel_button(CREW.CPG, _("Sight Select Switch, HMD"),															devices.TEDAC, tedac_commands.RHG_SIGHT_SELECT_SW_UP,		508, 1.0)
elements["pnt_508-1"].side = {}
elements["pnt_508-2"]	= knuppel_button(CREW.CPG, _("Sight Select Switch, LINK"),															devices.TEDAC, tedac_commands.RHG_SIGHT_SELECT_SW_DOWN,		508, -1.0)
elements["pnt_508-2"].side = {}
elements["pnt_509-1"]	= knuppel_button(CREW.CPG, _("Sight Select Switch, FCR"),															devices.TEDAC, tedac_commands.RHG_SIGHT_SELECT_SW_LEFT,		509, -1.0)
elements["pnt_509-1"].side = {}
elements["pnt_509-2"]	= knuppel_button(CREW.CPG, _("Sight Select Switch, TADS"),															devices.TEDAC, tedac_commands.RHG_SIGHT_SELECT_SW_RIGHT,	509, 1.0)
elements["pnt_509-2"].side = {}
elements["pnt_510"]		= default_3_position_tumb(CREW.CPG, _("Laser Tracker Mode (LT) Switch, A (Automatic)/O (Off)/M (Manual)"),			devices.TEDAC, tedac_commands.RHG_LT_SW, 510, NOT_CYCLED, anim_speed_default, NOT_INVERSED)
elements["pnt_510"].side = {}
elements["pnt_511-1"]	= knuppel_button(CREW.CPG, _("FCR Scan Size Switch, Z (Zoom)"),														devices.TEDAC, tedac_commands.RHG_FCR_SCAN_SIZE_SW_UP,		511, 1.0)
elements["pnt_511-1"].side = {}
elements["pnt_511-2"]	= knuppel_button(CREW.CPG, _("FCR Scan Size Switch, M (Medium)"),													devices.TEDAC, tedac_commands.RHG_FCR_SCAN_SIZE_SW_DOWN,	511, -1.0)
elements["pnt_511-2"].side = {}
elements["pnt_512-1"]	= knuppel_button(CREW.CPG, _("FCR Scan Size Switch, N (Narrow)"),													devices.TEDAC, tedac_commands.RHG_FCR_SCAN_SIZE_SW_LEFT,	512, -1.0)
elements["pnt_512-1"].side = {}
elements["pnt_512-2"]	= knuppel_button(CREW.CPG, _("FCR Scan Size Switch, W (Wide)"),														devices.TEDAC, tedac_commands.RHG_FCR_SCAN_SIZE_SW_RIGHT,	512, 1.0)
elements["pnt_512-2"].side = {}
elements["pnt_513"]		= default_button(CREW.CPG, _("C-Scope Button"),																		devices.TEDAC, tedac_commands.RHG_C_SCOPE_SW,				513)
elements["pnt_513"].side = {}
elements["pnt_504"]		= default_button(CREW.CPG, _("FLIR Polarity Button - Press to change polarity"),									devices.TEDAC, tedac_commands.RHG_FLIR_PLRT_BTN,			504)
elements["pnt_504"].side = {}
elements["pnt_514"]		= default_button(CREW.CPG, _("Sight Slave Button - Press to slave TADS or FCR to the selected acquisition source"),	devices.TEDAC, tedac_commands.RHG_SIGHT_SLAVE_BTN,			514)
elements["pnt_514"].side = {}
elements["pnt_517"]		= default_button(CREW.CPG, _("Display Zoom Button - Press to view FCR targeting information on the NTS target"),	devices.TEDAC, tedac_commands.RHG_DISPLAY_ZOOM_BTN,			517)
elements["pnt_517"].side = {}
elements["pnt_519"]		= springloaded_3_pos_tumb(CREW.CPG, _("Spare Switch, PREVIOUS(LMB)/NEXT(RMB)"),										devices.TEDAC, tedac_commands.RHG_SPARE_SW_AFT, tedac_commands.RHG_SPARE_SW_FWD, 519)
elements["pnt_519"].side = {}
elements["pnt_505"]		= default_button(CREW.CPG, _("HDD/HOD Select Button - currently not used"),											devices.TEDAC, tedac_commands.RHG_HDD_SW,					505)
elements["pnt_505"].side = {}
elements["pnt_518"]		= default_button(CREW.CPG, _("Cursor Enter Button - Press to enter"),												devices.TEDAC, tedac_commands.RHG_ENTER_BTN,				518)
elements["pnt_518"].side = {}
elements["pnt_515-1"]	= knuppel_button(CREW.CPG, _("Sight Manual Tracker (MAN TRK) Controller, Up"),										devices.TEDAC, tedac_commands.RHG_MAN_TRK_UP,				515, 1.0)
elements["pnt_515-1"].side = {}
elements["pnt_515-2"]	= knuppel_button(CREW.CPG, _("Sight Manual Tracker (MAN TRK) Controller, Down"),									devices.TEDAC, tedac_commands.RHG_MAN_TRK_DOWN,				515, -1.0)
elements["pnt_515-2"].side = {}
elements["pnt_516-1"]	= knuppel_button(CREW.CPG, _("Sight Manual Tracker (MAN TRK) Controller, Left"),									devices.TEDAC, tedac_commands.RHG_MAN_TRK_LEFT,				516, -1.0)
elements["pnt_516-1"].side = {}
elements["pnt_516-2"]	= knuppel_button(CREW.CPG, _("Sight Manual Tracker (MAN TRK) Controller, Right"),									devices.TEDAC, tedac_commands.RHG_MAN_TRK_RIGHT,			516, 1.0)
elements["pnt_516-2"].side = {}
elements["pnt_507"]		= default_3_position_tumb(CREW.CPG, _("Image Auto Tracker (IAT) Polarity Switch, W (White)/A (Auto)/B (Black)"),	devices.TEDAC, tedac_commands.RHG_IAT_POLARITY_SW,			507, NOT_CYCLED, anim_speed_default, NOT_INVERSED)
elements["pnt_507"].side = {}

-- Video Control Panel ( plt only )
elements["pnt_278"]		= default_rheostat(CREW.PLT, _("IHADSS BRT Control Knob"),							devices.ELEC_INTERFACE, electric_commands.VCP_IHADSS_BRT_KNOB,	278)
elements["pnt_279"]		= default_rheostat(CREW.PLT, _("IHADSS CON Control Knob"),							devices.ELEC_INTERFACE, electric_commands.VCP_IHADSS_CON_KNOB,	279)
elements["pnt_280"]		= default_rheostat(CREW.PLT, _("SYM BRT Control Knob"),								devices.ELEC_INTERFACE, electric_commands.VCP_SYM_BRT_KNOB,		280)
elements["pnt_282"]		= default_rheostat(CREW.PLT, _("FLIR LVL Control Knob"),							devices.ELEC_INTERFACE, electric_commands.VCP_FLIR_LEV_KNOB,	282)
elements["pnt_283"]		= default_rheostat(CREW.PLT, _("FLIR GAIN Control Knob"),							devices.ELEC_INTERFACE, electric_commands.VCP_FLIR_GAIN_KNOB,	283)
elements["pnt_281"]		= default_2_position_tumb(CREW.PLT, _("Automatic Contrast Mode Switch, ACM/OFF"),	devices.ELEC_INTERFACE,	electric_commands.VCP_ACM_SW,			281)

-- NVS MODE PANEL ( PLT )
elements["pnt_309"]		= default_3_position_tumb(CREW.PLT, _("NVS MODE Switch, FIXED/NORM/OFF"),	devices.ELEC_INTERFACE, electric_commands.NVS_MODE_PLT_KNOB, 309, NOT_CYCLED, anim_speed_default * 0.5, NOT_INVERSED, 1.0, {-1,1})
-- NVS MODE PANEL ( CPG )
elements["pnt_363"]		= default_3_position_tumb(CREW.CPG, _("NVS MODE Switch, FIXED/NORM/OFF"),	devices.ELEC_INTERFACE, electric_commands.NVS_MODE_CPG_KNOB, 363, NOT_CYCLED, anim_speed_default * 0.5, NOT_INVERSED, 1.0, {-1,1})

-- Generator Reset PANEL ( PLT ) 
elements["pnt_355"]		= springloaded_3_pos_tumb(CREW.PLT, _("Generator Reset Switch, GEN 1(LMB)/GEN 2(RMB)"),				devices.ELEC_INTERFACE,		electric_commands.GEN1_RST_SW,		electric_commands.GEN2_RST_SW,		355)
elements["pnt_353"]		= springloaded_3_pos_tumb(CREW.PLT, _("CKT A Check Overspeed Test Switch, ENG 2(LMB)/ENG 1(RMB)"),	devices.ENGINE_INTERFACE,	engine_commands.ChkOvspTestSwENG2A,	engine_commands.ChkOvspTestSwENG1A,	353)
elements["pnt_354"]		= springloaded_3_pos_tumb(CREW.PLT, _("CKT B Check Overspeed Test Switch, ENG 2(LMB)/ENG 1(RMB)"),	devices.ENGINE_INTERFACE,	engine_commands.ChkOvspTestSwENG2B,	engine_commands.ChkOvspTestSwENG1B,	354)

-- PROCESSOR SELECT PANEL ( CPG )
elements["pnt_397"]		= default_3_position_tumb(CREW.CPG, _("Processor Select Switch, SP 1/AUTO/SP 2"),			devices.ELEC_INTERFACE, electric_commands.SP_SELECT_SW, 397, NOT_CYCLED, anim_speed_default * 0.5, IS_INVERSED, 1.0, {-1,1})
elements["pnt_397"].sound = {{SOUND_SW1}}
							
-- PILOT POWER LEVER QUADRANT
elements["pnt_315"]		= multiposition_switch(CREW.PLT, _("Master Ignition Switch, OFF/BATT/EXT PWR"),		devices.ELEC_INTERFACE, electric_commands.MIK, 315, 3, 0.5, NOT_INVERSED, 0.0, anim_speed_default * 0.22, NOT_CYCLED)
elements["pnt_315"].sound = {{SOUND_SW7_RIGHT, SOUND_SW7_LEFT}}

-- ARMAMENT PANEL ( PLT )
elements["pnt_306"]		= lighted_pushbutton(CREW.PLT, _("A/S Pushbutton, ARM/SAFE"),		devices.ELEC_INTERFACE, electric_commands.ARM_SAFE_PLT_BTN,		306)
elements["pnt_307"]		= lighted_pushbutton(CREW.PLT, _("GND ORIDE Pushbutton, ON/OFF"),	devices.ELEC_INTERFACE, electric_commands.GND_ORIDE_PLT_BTN,	307)
 
-- ARMAMENT PANEL ( CPG )
elements["pnt_293"]		= lighted_pushbutton(CREW.CPG, _("A/S Pushbutton, ARM/SAFE"),		devices.ELEC_INTERFACE, electric_commands.ARM_SAFE_CPG_BTN,		293)
elements["pnt_294"]		= lighted_pushbutton(CREW.CPG, _("GND ORIDE Pushbutton, ON/OFF"),	devices.ELEC_INTERFACE, electric_commands.GND_ORIDE_CPG_BTN,	294)

-- PLT Left Console
elements["pnt_319"]		= lighted_pushbutton(CREW.PLT, _("L OUTBD Station Select Pushbutton, ARM/SAFE"),					devices.JETT_PANEL_PLT,	JETT_commands.STORE_LO_JETTISON_ARMED,		319)
elements["pnt_320"]		= lighted_pushbutton(CREW.PLT, _("L INBD Station Select Pushbutton, ARM/SAFE"),						devices.JETT_PANEL_PLT,	JETT_commands.STORE_LI_JETTISON_ARMED,		320)
elements["pnt_321"]		= lighted_pushbutton(CREW.PLT, _("R INBD Station Select Pushbutton, ARM/SAFE"),						devices.JETT_PANEL_PLT,	JETT_commands.STORE_RI_JETTISON_ARMED,		321)
elements["pnt_322"]		= lighted_pushbutton(CREW.PLT, _("R OUTBD Station Select Pushbutton, ARM/SAFE"),					devices.JETT_PANEL_PLT,	JETT_commands.STORE_RO_JETTISON_ARMED,		322)
elements["pnt_323"]		= lighted_pushbutton(CREW.PLT, _("L TIP Station Select Pushbutton, ARM/SAFE"),						devices.JETT_PANEL_PLT,	JETT_commands.STORE_JETTISON_LEFT_WINGTIP,	323)
elements["pnt_325"]		= lighted_pushbutton(CREW.PLT, _("R TIP Station Select Pushbutton, ARM/SAFE"),						devices.JETT_PANEL_PLT,	JETT_commands.STORE_JETTISON_RIGHT_WINGTIP,	325)
elements["pnt_324"]		= default_button(CREW.PLT, _("JETT Pushbutton - Press to jettison stores from all armed stations"),	devices.JETT_PANEL_PLT,	JETT_commands.STORES_JETT_PUSHBUTTON,		324)

elements["pnt_313"]		= lighted_pushbutton(CREW.PLT, _("EMERG HYD Pushbutton, ON/OFF"),						devices.HYDRO_INTERFACE,	hydraulic_commands.Emergency_HYD_PLT,	313)
elements["pnt_308"]		= lighted_pushbutton(CREW.PLT, _("TAIL WHEEL Pushbutton, LOCK/UNLOCK"),					devices.HYDRO_INTERFACE,	hydraulic_commands.TailWheelUnLock_PLT,	308)
elements["pnt_314"]		= default_3_position_tumb(CREW.PLT, _("Rotor Brake Switch, OFF/BRK/LOCK"),				devices.HYDRO_INTERFACE,	hydraulic_commands.Rotor_Brake,			314,	NOT_CYCLED,	anim_speed_default, NOT_INVERSED)
elements["pnt_314"].sound = {{SOUND_SW1}}

elements["pnt_400"]		= lighted_pushbutton(CREW.PLT, _("APU Pushbutton - Press to start/stop APU"),			devices.ENGINE_INTERFACE,	engine_commands.APU_StartBtn,			400)
elements["pnt_401"]		= lighted_pushbutton_cover(CREW.PLT, _("APU Pushbutton Cover, OPEN/CLOSE"),				devices.ENGINE_INTERFACE,	engine_commands.APU_StartBtnCover,		401)
elements["pnt_633"]		= default_lever(CREW.PLT, _("Power Lever Friction Adjustment Lever"),					devices.CONTROL_INTERFACE,	ctrl_commands.FrictionLever,			633)

elements["pnt_317"]		= springloaded_3_pos_tumb(CREW.PLT, _("No.1 Engine Start Switch, IGN ORIDE(LMB)/START(RMB)"),	devices.ENGINE_INTERFACE, engine_commands.Eng1IgnOrideSw, engine_commands.Eng1StartSw, 317)
elements["pnt_318"]		= springloaded_3_pos_tumb(CREW.PLT, _("No.2 Engine Start Switch, IGN ORIDE(LMB)/START(RMB)"),	devices.ENGINE_INTERFACE, engine_commands.Eng2IgnOrideSw, engine_commands.Eng2StartSw, 318)

-- CPG Left Console
elements["pnt_368"]		= lighted_pushbutton(CREW.CPG, _("L OUTBD Station Select Pushbutton, ARM/SAFE"),						devices.JETT_PANEL_CPG,	JETT_commands.STORE_LO_JETTISON_ARMED,		368)
elements["pnt_369"]		= lighted_pushbutton(CREW.CPG, _("L INBD Station Select Pushbutton, ARM/SAFE"),						devices.JETT_PANEL_CPG,	JETT_commands.STORE_LI_JETTISON_ARMED,		369)
elements["pnt_370"]		= lighted_pushbutton(CREW.CPG, _("R INBD Station Select Pushbutton, ARM/SAFE"),						devices.JETT_PANEL_CPG,	JETT_commands.STORE_RI_JETTISON_ARMED,		370)
elements["pnt_371"]		= lighted_pushbutton(CREW.CPG, _("R OUTBD Station Select Pushbutton, ARM/SAFE"),						devices.JETT_PANEL_CPG,	JETT_commands.STORE_RO_JETTISON_ARMED,		371)
elements["pnt_372"]		= lighted_pushbutton(CREW.CPG, _("L TIP Station Select Pushbutton, ARM/SAFE"),						devices.JETT_PANEL_CPG,	JETT_commands.STORE_JETTISON_LEFT_WINGTIP,	372)
elements["pnt_374"]		= lighted_pushbutton(CREW.CPG, _("R TIP Station Select Pushbutton, ARM/SAFE"),						devices.JETT_PANEL_CPG,	JETT_commands.STORE_JETTISON_RIGHT_WINGTIP,	374)
elements["pnt_373"]		= default_button(CREW.CPG, _("JETT Pushbutton - Press to jettison stores from all armed stations"),	devices.JETT_PANEL_CPG,	JETT_commands.STORES_JETT_PUSHBUTTON,		373)

elements["pnt_361"]		= lighted_pushbutton(CREW.CPG, _("EMERG HYD Pushbutton, ON/OFF"),				devices.HYDRO_INTERFACE,	hydraulic_commands.Emergency_HYD_CPG,	361)
elements["pnt_362"]		= lighted_pushbutton(CREW.CPG, _("TAIL WHEEL Pushbutton, LOCK/UNLOCK"),			devices.HYDRO_INTERFACE,	hydraulic_commands.TailWheelUnLock_CPG,	362)

-- FIRE DET / EXTG Control Panel (PLT)
elements["pnt_295"]		= lighted_push_button_tumb(CREW.PLT, _("ENG 1 Fire Pushbutton - Press to arm/safe ENG 1 area's fire extinguishing system"),		devices.ENGINE_INTERFACE,	engine_commands.PLT_Eng1FireBtn,		295)
elements["pnt_296"]		= lighted_pushbutton_cover(CREW.PLT, _("ENG 1 Fire Pushbutton Cover, OPEN/CLOSE"),												devices.ENGINE_INTERFACE,	engine_commands.PLT_Eng1FireBtnCover,	296)
elements["pnt_297"]		= lighted_push_button_tumb(CREW.PLT, _("APU Fire Pushbutton - Press to arm/safe APU area's fire extinguishing system"),			devices.ENGINE_INTERFACE,	engine_commands.PLT_ApuFireBtn,			297)
elements["pnt_298"]		= lighted_pushbutton_cover(CREW.PLT, _("APU Fire Pushbutton Cover, OPEN/CLOSE"),												devices.ENGINE_INTERFACE,	engine_commands.PLT_ApuFireBtnCover,	298)
elements["pnt_299"]		= lighted_push_button_tumb(CREW.PLT, _("ENG 2 Fire Pushbutton - Press to arm/safe ENG 2 area's fire extinguishing system"),		devices.ENGINE_INTERFACE,	engine_commands.PLT_Eng2FireBtn,		299)
elements["pnt_300"]		= lighted_pushbutton_cover(CREW.PLT, _("ENG 2 Fire Pushbutton Cover, OPEN/CLOSE"),												devices.ENGINE_INTERFACE,	engine_commands.PLT_Eng2FireBtnCover,	300)
elements["pnt_301"]		= lighted_pushbutton(CREW.PLT, _("Primary Fire Extinguisher Discharge Pushbutton - Press to discharge primary fire bottle"),	devices.ENGINE_INTERFACE,	engine_commands.PLT_PrimaryDischBtn,	301)
elements["pnt_303"]		= lighted_pushbutton(CREW.PLT, _("Reserve Fire Extinguisher Discharge Pushbutton - Press to discharge reserve fire bottle"),	devices.ENGINE_INTERFACE,	engine_commands.PLT_ReserveDischBtn,	303)
elements["pnt_302"]		= springloaded_3_pos_tumb(CREW.PLT, _("Fire Detection Circuit Test Switch, 1(LMB)/2(RMB)"),		devices.ENGINE_INTERFACE, engine_commands.PLT_FireDetTestSw1, engine_commands.PLT_FireDetTestSw2,	302)

-- FIRE DET / EXTG Control Panel (CPG)
elements["pnt_284"]		= lighted_push_button_tumb(CREW.CPG, _("ENG 1 Fire Pushbutton - Press to arm/safe ENG 1 area's fire extinguishing system"),		devices.ENGINE_INTERFACE,	engine_commands.CPG_Eng1FireBtn,		284)
elements["pnt_285"]		= lighted_pushbutton_cover(CREW.CPG, _("ENG 1 Fire Pushbutton Cover, OPEN/CLOSE"),												devices.ENGINE_INTERFACE,	engine_commands.CPG_Eng1FireBtnCover,	285)
elements["pnt_286"]		= lighted_push_button_tumb(CREW.CPG, _("APU Fire Pushbutton - Press to arm/safe APU area's fire extinguishing system"),			devices.ENGINE_INTERFACE,	engine_commands.CPG_ApuFireBtn,			286)
elements["pnt_287"]		= lighted_pushbutton_cover(CREW.CPG, _("APU Fire Pushbutton Cover, OPEN/CLOSE"),												devices.ENGINE_INTERFACE,	engine_commands.CPG_ApuFireBtnCover,	287)
elements["pnt_288"]		= lighted_push_button_tumb(CREW.CPG, _("ENG 2 Fire Pushbutton - Press to arm/safe ENG 2 area's fire extinguishing system"),		devices.ENGINE_INTERFACE,	engine_commands.CPG_Eng2FireBtn,		288)
elements["pnt_289"]		= lighted_pushbutton_cover(CREW.CPG, _("ENG 2 Fire Pushbutton Cover, OPEN/CLOSE"),												devices.ENGINE_INTERFACE,	engine_commands.CPG_Eng2FireBtnCover,	289)
elements["pnt_290"]		= lighted_pushbutton(CREW.CPG, _("Primary Fire Extinguisher Discharge Pushbutton - Press to discharge primary fire bottle"),	devices.ENGINE_INTERFACE,	engine_commands.CPG_PrimaryDischBtn,	290)
elements["pnt_292"]		= lighted_pushbutton(CREW.CPG, _("Reserve Fire Extinguisher Discharge Pushbutton - Press to discharge reserve fire bottle"),	devices.ENGINE_INTERFACE,	engine_commands.CPG_ReserveDischBtn,	292)
elements["pnt_291"]		= springloaded_3_pos_tumb(CREW.CPG, _("Fire Detection Circuit Test Switch, 1(LMB)/2(RMB)"),		devices.ENGINE_INTERFACE, engine_commands.CPG_FireDetTestSw1, engine_commands.CPG_FireDetTestSw2,	291)

-- Windshield Panels
elements["pnt_356"]		= default_button(CREW.PLT, _("Defog Button - Press to defog the canopy side panels"),	devices.CPT_MECH, cpt_mech_commands.PLT_DefogBtn,	356)
elements["pnt_357"]		= wiper_multiposition_spring_switch(CREW.PLT, _("Wiper Control Switch, PARK/OFF/LO/HI"),devices.CPT_MECH, cpt_mech_commands.PLT_WiperSw_PARK, cpt_mech_commands.PLT_WiperSw,	357)
elements["pnt_394"]		= default_button(CREW.CPG, _("Defog Button - Press to defog the canopy side panels"),	devices.CPT_MECH, cpt_mech_commands.CPG_DefogBtn,	394)
elements["pnt_395"]		= wiper_multiposition_spring_switch(CREW.CPG, _("Wiper Control Switch, PARK/OFF/LO/HI"),devices.CPT_MECH, cpt_mech_commands.CPG_WiperSw_PARK, cpt_mech_commands.CPG_WiperSw,	395)

-- Gear system
elements["pnt_634"]		= default_2_position_tumb(CREW.PLT, _("Parking Brake Handle, Pull/Stow"),	devices.GEAR_INTERFACE, gear_commands.AH64_ParkingBrake, 634)
elements["pnt_634"].sound = {{SOUND_SW9_PULL,SOUND_SW9_PUSH}}
elements["pnt_634"].side = {{BOX_SIDE_Y_top},{BOX_SIDE_Y_bottom}}

-- PLT canopy
elements["pnt_796"] = default_animated_lever(CREW.PLT,_("PLT canopy, OPEN/CLOSE"), devices.CPT_MECH, cpt_mech_commands.PLT_Door_Lock, 796, 2)
elements["pnt_796"].sound = {{SOUND_SW10_OPEN, SOUND_SW10_CLOSE}}
-- CPG canopy
elements["pnt_799"] = default_animated_lever(CREW.CPG,_("CPG canopy, OPEN/CLOSE"), devices.CPT_MECH, cpt_mech_commands.CPG_Door_Lock, 799, 2)
elements["pnt_799"].sound = {{SOUND_SW10_OPEN, SOUND_SW10_CLOSE}}

-- Power Levers
elements["pnt_398"]		= default_lever(CREW.PLT, _('Power Lever Smoothly (Left)'),		devices.ENGINE_INTERFACE,	engine_commands.PLT_L_PowerLever,	398)
elements["pnt_399"]		= default_lever(CREW.PLT, _('Power Lever Smoothly (Right)'),	devices.ENGINE_INTERFACE,	engine_commands.PLT_R_PowerLever,	399)
elements["pnt_628"]		= default_lever(CREW.CPG, _('Power Lever Smoothly (Left)'),		devices.ENGINE_INTERFACE,	engine_commands.CPG_L_PowerLever,	628)
elements["pnt_629"]		= default_lever(CREW.CPG, _('Power Lever Smoothly (Right)'),	devices.ENGINE_INTERFACE,	engine_commands.CPG_R_PowerLever,	629)

-- Very essential cockpit elements
elements["pnt_825"]		=	default_button(				CREW.CPG,	_('CPG M4 Trigger'),	devices.CPT_MECH,	cpt_mech_commands.CPG_M4_Trigger,	825)
elements["pnt_825"].arg_value = {0.5}
elements["pnt_826"]		=	default_3_position_tumb(	CREW.CPG,	_("CPG M4 Safety"),		devices.CPT_MECH,	cpt_mech_commands.CPG_M4_Safety,	826)
elements["pnt_827"]		=	default_button(				CREW.PLT,	_('PLT M4 Trigger'),	devices.CPT_MECH,	cpt_mech_commands.PLT_M4_Trigger,	827)
elements["pnt_827"].arg_value = {0.5}
elements["pnt_828"]		=	default_3_position_tumb(	CREW.PLT,	_("PLT M4 Safety"),		devices.CPT_MECH,	cpt_mech_commands.PLT_M4_Safety,	828)

 default_button(CREW.CPG, _('CPG M4 Safety'), devices.CPT_MECH, cpt_mech_commands.CPG_M4_Trigger, 825)
-- Linking elements
local rudder_left = elements["pnt_398"]
if VR_device ~= nil and VR_device.pilot_engine_left ~= nil then
	rudder_left.VR_capture = VR_device.pilot_engine_left
	rudder_left.VR_capture.sync_connector = "pnt_399"
else
	rudder_left.VR_capture = {}
end

local rudder_right = elements["pnt_399"]
if VR_device ~= nil and VR_device.pilot_engine_right ~= nil then
	rudder_right.VR_capture = VR_device.pilot_engine_right
	rudder_right.VR_capture.sync_connector = "pnt_398"
else
	rudder_right.VR_capture = {}
end

local c_rudder_left = elements["pnt_628"]
if VR_device ~= nil and VR_device.cpg_engine_left ~= nil then
	c_rudder_left.VR_capture = VR_device.cpg_engine_left
	c_rudder_left.VR_capture.sync_connector = "pnt_629"
else
	c_rudder_left.VR_capture = {}
end

local c_rudder_right = elements["pnt_629"]
if VR_device ~= nil and VR_device.cpg_engine_right ~= nil then
	c_rudder_right.VR_capture = VR_device.cpg_engine_right
	c_rudder_right.VR_capture.sync_connector = "pnt_628"
else
	c_rudder_right.VR_capture = {}
end

elements["pnt_610"] 	= spring_3_position_tumb(CREW.PLT, "CMWS PWR Switch, OFF/ON/TEST(momentarily)",		devices.CMWS, CMWS_commands.CMWS_PWR, CMWS_commands.CMWS_PWR_TEST,	610)
elements["pnt_611"]		= default_rheostat(CREW.PLT, _("CMWS Audio Volume Knob"),							devices.CMWS, CMWS_commands.CMWS_AUDIO_KNOB,		611)
elements["pnt_612"]		= default_rheostat(CREW.PLT, _("CMWS Lamp Knob"),									devices.CMWS, CMWS_commands.CMWS_LAMP_KNOB,			612)
elements["pnt_614"]		= default_2_position_tumb(CREW.PLT, _("CMWS Flare Squibs Switch, ARM/SAFE"),		devices.CMWS, CMWS_commands.CMWS_ARM_SAFE_SW,		614)
elements["pnt_615"]		= default_2_position_tumb(CREW.PLT, _("CMWS Mode Switch, CMWS/NAV"),				devices.CMWS, CMWS_commands.CMWS_CMWS_NAV_SW,		615)
elements["pnt_616"]		= default_2_position_tumb(CREW.PLT, _("CMWS Operation Switch, BYPASS/AUTO"),		devices.CMWS, CMWS_commands.CMWS_BYPASS_AUTO_SW,	616)
elements["pnt_617"]		= default_red_cover(CREW.PLT, _("CMWS Flare Jettison Switch Cover, OPEN/CLOSE"),	devices.CMWS, CMWS_commands.CMWS_JETT_COVER,		617)
elements["pnt_617"].sound = {{SOUND_SW5_OPEN,SOUND_SW5_CLOSE}}
elements["pnt_618"]		= default_2_position_tumb(CREW.PLT, _("CMWS Flare Jettison Switch"),				devices.CMWS, CMWS_commands.CMWS_JETT_SW,			618)


--cpg folding stick
elements["pnt_809"] = default_2_position_tumb(CREW.CPG, _("Stick Folding, UP/DOWN"),		devices.PrestonAI,	preston_commands.StickFolding,	809, anim_speed_default * 0.2)



elements["pnt_849"]		= sunvisor_handle(CREW.PLT, _("Left Sunvisor"),		devices.CPT_MECH,	cpt_mech_commands.PLT_SunvisorLeft,		849)
elements["pnt_850"]		= sunvisor_handle(CREW.PLT, _("Right Sunvisor"),	devices.CPT_MECH,	cpt_mech_commands.PLT_SunvisorRight,	850)


