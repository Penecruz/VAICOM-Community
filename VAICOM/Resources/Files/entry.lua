local self_ID = "VAICOM PRO"

declare_plugin(self_ID,
{
	installed	 = true,
	dirName		 = current_mod_path,
	binaries	 = {'VAICOMPRO.dll'},

	displayName	 = "VAICOM PRO",
	shortName	 = "VAICOM PRO",
	fileMenuName = "VAICOM PRO",

	version		 = "3.0.0",
	state		 = "installed", 	
	developerName= "VAICOM Community",
	info		 = _("VAICOM PRO Community Edition is a voice communications interface plugin for VoiceAttack, enabling true-to-life radio communications with all AI units in the mission."),

	Skins	=
	{
		{
			name	= "VAICOM PRO",
			dir		= "Theme"
		},
	},

	Options =
	{
		{
			name		= "VAICOM PRO",
			nameId		= "VAICOM",
			dir			= "Options",
			CLSID		= "{VAICOM options}"
		},
	},
})

plugin_done()
