local DbOption	= require('Options.DbOption')
local Range		= DbOption.Range
local i18n		= require('i18n')
local oms		= require('optionsModsScripts')

local _ = i18n.ptranslate


return {
	AH64PedalsTrimmingMethod	= DbOption.new():setValue(0):combo({DbOption.Item(_('INSTANT TRIM (FFB FRIENDLY)')):Value(0),
																	DbOption.Item(_('CENTRAL POSITION TRIMMER MODE')):Value(1),
																	DbOption.Item(_('PEDALS WITHOUT SPRINGS AND FFB')):Value(2),}),
	AH64CyclicTrimmingMethod	= DbOption.new():setValue(0):combo({DbOption.Item(_('INSTANT TRIM (FFB FRIENDLY)')):Value(0),
																	DbOption.Item(_('CENTRAL POSITION TRIMMER MODE')):Value(1),
																	DbOption.Item(_('JOYSTICK WITHOUT SPRINGS AND FFB')):Value(2),}),
	AH64LockoutDetent			= DbOption.new():setValue(1):combo({DbOption.Item(_('Automatically jump over')):Value(0),
																	DbOption.Item(_('Depress fingerlifts to release locks')):Value(1),}),
	AH64CockpitShake			= DbOption.new():setValue(50):slider(Range(0, 100)),
	CPLocalList					= oms.getCPLocalList("Cockpit_AH-64D"),
	AH64IhadssMonocleVisible	= DbOption.new():setValue(true):checkbox(),
	AH64HmdEye					= DbOption.new():setValue(1):combo({DbOption.Item(_('Right eye')):Value(1),
																	DbOption.Item(_('Left eye')):Value(0),
																	DbOption.Item(_('Both eyes')):Value(2),}),
	AH64AutoHandover			= DbOption.new():setValue(true):checkbox(),
	AH64BalaclavaPLT			= DbOption.new():setValue(true):checkbox(),
	AH64BalaclavaCPG			= DbOption.new():setValue(true):checkbox(),
	AH64TriggerGuardEnable		= DbOption.new():setValue(false):checkbox(),
	AH64AIIFFColorScheme		= DbOption.new():setValue(0):combo({
																		DbOption.Item(_('NATO')):Value(0),
																		DbOption.Item(_('Coalition color')):Value(1),
																	}),
	AH64AIInterfaceColorScheme	= DbOption.new():setValue(0):combo({
																		DbOption.Item(_('Normal')):Value(0),
																		DbOption.Item(_('Colorblind Safe')):Value(1),
																		DbOption.Item(_('Monochrome White')):Value(2),
																		DbOption.Item(_('Monochrome Yellow')):Value(3),
																	}),
	AH64AISubtitles				= DbOption.new():setValue(true):checkbox(),
	AH64ManTrkRampUpSpeed		= DbOption.new():setValue(5):slider(Range(1, 10)),
	AH64VRSNotificationAudio	= DbOption.new():setValue(false):checkbox(),
	AH64VRSNotificationVisual	= DbOption.new():setValue(false):checkbox(),

}
