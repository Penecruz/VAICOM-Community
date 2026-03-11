local cockpit = folder.."../../../Cockpit/Scripts/"
dofile(cockpit.."devices.lua")
dofile(cockpit.."command_defs.lua")

return {

keyCommands = {


{combos = {{key = 'V', reformers = {'LCtrl'}}},		up = preston_commands.ShowHideMenu,	down = preston_commands.ShowHideMenu,	cockpit_device_id = devices.PrestonAI, value_up = 0, value_down = 1,	name = _('George AI - Hide'),		category = {_('George AI')}},
{combos = {{key = 'W'}},							up = preston_commands.HatUp,		down = preston_commands.HatUp,			cockpit_device_id = devices.PrestonAI, value_up = 0, value_down = 1,	name = _('George AI - Up'),		category = {_('George AI')}},
{combos = {{key = 'S'}},							up = preston_commands.HatDown,		down = preston_commands.HatDown,		cockpit_device_id = devices.PrestonAI, value_up = 0, value_down = 1,	name = _('George AI - Down'),		category = {_('George AI')}},
{combos = {{key = 'A'}},							up = preston_commands.HatLeft,		down = preston_commands.HatLeft,		cockpit_device_id = devices.PrestonAI, value_up = 0, value_down = 1,	name = _('George AI - Left'),		category = {_('George AI')}},
{combos = {{key = 'D'}},							up = preston_commands.HatRight,		down = preston_commands.HatRight,		cockpit_device_id = devices.PrestonAI, value_up = 0, value_down = 1,	name = _('George AI - Right'),		category = {_('George AI')}},

--{													up = preston_commands.MfInput,		down = preston_commands.MfInput,		cockpit_device_id = devices.PrestonAI, value_up = 0, value_down = 1,	name = _('George AI - Multifunctional Input (Center)'),		category = {_('George AI')}},

}

}
