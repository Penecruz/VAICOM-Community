dofile(LockOn_Options.common_script_path.."elements_defs.lua")
dofile(LockOn_Options.script_path.."Displays/EUFD/EUFD_definitions.lua")

local half_width   = GetScale()
local half_height  = GetAspect() * half_width
local aspect       = GetAspect() -- GetHalfHeight()/GetHalfWidth()

background					= CreateElement "ceMeshPoly" -- untextured shape
background.name				= "background"
background.material			= DBG_RED
background.h_clip_relation	= h_clip_relations.REWRITE_LEVEL  -- check clipping : pixel on glass then increase level from GLASS_LEVEL to GLASS_LEVEL+1 = HUD_DEFAULT_LEVEL
background.level			= DEFAULT_LEVEL
background.collimated		= false
background.isvisible		= false
background.z_enabled		= true
background.vertices			= { {-1, aspect}, { 1,aspect}, { 1,-aspect}, {-1,-aspect}, }
background.indices			=  {0,1,2 ;  -- first triangle
								0,2,3 }  -- second
Add(background)

AddText("Test_1",					-1.0,			aspect,				nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_2",					-1.0,			aspect*6/7,			nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_3",					-1.0,			aspect*5/7,			nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_4",					-1.0,			aspect*4/7,			nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_5",					-1.0,			aspect*3/7,			nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_6",					-1.0,			aspect*2/7,			nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_7",					-1.0,			aspect/7,			nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_8",					-1.0,			0.0,				nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_9",					-1.0,			-aspect/7,			nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_10",					-1.0,			-aspect*2/7,		nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_11",					-1.0,			-aspect*3/7,		nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_12",					-1.0,			-aspect*4/7,		nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_13",					-1.0,			-aspect*5/7,		nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
AddText("Test_14",					-1.0,			-aspect*6/7,		nil, 						"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")