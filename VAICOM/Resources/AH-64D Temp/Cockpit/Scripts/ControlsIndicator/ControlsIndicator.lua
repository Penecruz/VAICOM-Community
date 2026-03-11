dofile(LockOn_Options.common_script_path.."devices_defs.lua")

indicator_type		= indicator_types.COMMON
purposes			= {render_purpose.SCREENSPACE_INSIDE_COCKPIT,render_purpose.HUD_ONLY_VIEW}
screenspace_scale	= 4;

-------PAGE IDs-------
id_Page =
{
	MAIN = 0,
}

id_pagesubset =
{
	COMMON = 0,
}

page_subsets = {}
page_subsets[id_pagesubset.COMMON] = LockOn_Options.script_path.."ControlsIndicator\\ControlsIndicator_page.lua"

----------------------
pages = {}
pages[id_Page.MAIN] = { id_pagesubset.COMMON}
init_pageID = id_Page.MAIN

need_to_be_closed = true -- close lua state after initialization



VRS_Stage =
{
	Start		= 0,
	Evolution	= 1,
	Progress	= 2,
}

VRS_Annunciator =
{
	Vx_max = 13.0,	-- [knots] --до 19.10.24 было 30

	[VRS_Stage.Start] =
	{
		Vy_min = 0.31,--до 19.10.24 было 0.27
		sound =
		{
			track = "Aircrafts/Cockpits/VortexRing_WarningTone_1",
			t_signal = 0.50, t_pause = 0.0--0.50
		},
		visual = { t_signal = 0.50, t_pause = 0.50 }
	},
	[VRS_Stage.Evolution] =
	{
		Vy_min = 0.39,---до 19.10.24 было 0.33-
		sound =
		{
			track = "Aircrafts/Cockpits/VortexRing_WarningTone_2",
			t_signal = 0.25, t_pause = 0.0--0.30
		},
		visual = { t_signal = 0.25, t_pause = 0.30 }
	},
	[VRS_Stage.Progress] =
	{
		Vy_min = 0.53,--до 19.10.24 было 0.5
		sound =
		{
			track = "Aircrafts/Cockpits/VortexRing_WarningTone_3",
			t_signal = 1.00, t_pause = 0.0--0.75
		},
		visual = { t_signal = 1.00, t_pause = 0.75 }
	},
}
