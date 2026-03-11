local self_ID = "AH-64D BLK.II"
local flyable_ID = "AH-64D_BLK_II"

declare_plugin(self_ID,
{
	installed		= true,
	dirName			= current_mod_path,
	displayName		= _("AH-64D BLK.II"),
	shortName		= "AH-64D",
	fileMenuName	= _("AH-64D BLK.II"),
	update_id		= "AH-64D",
	version			= __DCS_VERSION__.." WIP",
	state			= "installed",
	developerName	= _("Eagle Dynamics"),
	info			= _("The AH-64D has served as the backbone of the    U.S. Army attack helicopter force since 2003. Based on the AH-64A, the “Delta” version is a tandem-crewed helicopter with the pilot in the back seat and the co-pilot/gunner (CP/G) in the front. Armed with a 30mm Chain Gun under the nose, Hellfire missiles and 2.75” rockets on the stub wings, the AH-64D proved its mettle in Iraq and Afghanistan against a wide array of threats."),
	
	binaries =
	{
		'AH64D',
	},
	
	Skins =
	{
		{
			name	= "AH-64D",
			dir		= "Skins/1"
		},
	},
	Missions =
	{
		{
			name	= _("AH-64D"),
			dir		= "Missions",
		},
	},
	LogBook =
	{
		{
			name	= _("AH-64D"),
			type	= flyable_ID,
		},
	},
	Options =
	{
		{
			name		= _("AH-64D"),
			nameId		= flyable_ID,
			dir			= "Options",
		},
	},
	InputProfiles =
	{
		[flyable_ID.."_PLT"]	= current_mod_path..'/Input/AH-64D_PLT/',
		[flyable_ID.."_CPG"]	= current_mod_path..'/Input/AH-64D_CPG/',
		[flyable_ID.."_AI_Menu"]	= current_mod_path..'/Input/AH-64D_AI_Menu/',
	},

})

-------------------------------------------------------------------------------
mount_vfs_texture_path	(current_mod_path.."/Cockpit/Textures/AH-64D-CPT-TEXTURES")
mount_vfs_model_path	(current_mod_path.."/Cockpit/Shape")
mount_vfs_liveries_path	(current_mod_path.."/Liveries")
mount_vfs_texture_path	(current_mod_path.."/Skins/1/ME")--for simulator loading window

local cfg_path = current_mod_path.."/FM/config.lua"
dofile(cfg_path)
AH64D[1]			= self_ID
AH64D[2]			= 'AH64D'
AH64D.config_path	= cfg_path
AH64D.user_options	= flyable_ID

dofile(current_mod_path.."/Views.lua")
make_view_settings(flyable_ID, ViewSettings, SnapViews)

make_flyable(flyable_ID, current_mod_path..'/Cockpit/Scripts/', AH64D, current_mod_path..'/comm.lua')
plugin_done()-- finish declaration , clear temporal data
