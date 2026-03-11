dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")


local controls = {}
controls=
{
    {"A C F T", 
        { 
            { pb.L1,    "A",    tp_default_border,              {{"FCR_AG_SetMissionPriorityMissionPage",0}}    },
            { pb.L2,    "B",    tp_default_border,              {{"FCR_AG_SetMissionPriorityMissionPage",1}}    },
            { pb.L3,    "C",    tp_default_border,              {{"FCR_AG_SetMissionPriorityMissionPage",2}}    },
        }
    },

    { pb.B1, "FCR", tp_28, nil},
}
createControls( controls )


local Menu = {}
Menu = 
{ 
    { pb.T6, "UTIL", tp_default_border },
}
createMenu( Menu )

local position = {0, 200}
local margin = 30    
local value = "PRIORITY SCHEME"

AddRoundCornersWindow("Status: ",position, tp_default.width * #value + margin, tp_default.height + margin, value, tp_default, nil, nil, nil, "CenterCenter")