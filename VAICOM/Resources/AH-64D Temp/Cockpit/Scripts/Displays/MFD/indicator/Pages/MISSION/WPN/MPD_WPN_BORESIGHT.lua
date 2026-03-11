dofile(LockOn_Options.script_path.."Displays/MFD/indicator/Pages/MISSION/WPN/MPD_WPN_Symbology_defs.lua")

if DBG_LABEL_SHOW then
addText( "WPN BORESIGHT PAGE",  {0, 410}, tp_36_white)
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

local TADS_OUTFRONT = 0
local TADS_INTERNAL = 1
local TADS_OFF		= 2 

local MFD_NUM = readParameter("MFD_NUM")

local Menu = {}
Menu = 
{ 
	{ pb.T1, "CHAN",		nil },
	{ pb.T2, "ASE",			tp_default_border, 	{{"MFD_AsePrevBorder", 0}} },
	{ pb.T4, "CODE",		nil },
	{ pb.T5, "COORD",		nil },
	{ pb.T6, "UTIL",		nil },

	{ pb.B1, "WPN",			nil },
	{ pb.L5, "BORESIGHT",	tp_default_border },
}

local CommonControls = {}
CommonControls = 
{
	{ pb.B6, { {"MANRNG>", nil, nil}, {"4200", tp_default_border, { {"WPN_MAN_RNG_Value"},{"MFD_DataEntryButton_frame",pb.B6} }} } },
}

local UniqControls = {}
if MFD_NUM < MFD_SELF_IDS.CPG_LMFD then					-- PLT

	UniqControls = 
	{
		{ pb.L4, "IHADSS",	tp_default_border,	{{"BORESIGHT_IHADSS", 0}}},
		{ pb.L6, "B/S NOW",	nil,				{{"BORESIGHT_NOW", 0}}},
		{ pb.R3, {{"REMOVE", nil, {{"REMOVE_MESSAGE", 0}}},{"MESSAGE", nil, {{"REMOVE_MESSAGE", 0}}}}, nil },
	}

else 								-- CPG
	UniqControls = 
	{
		{ pb.L4, "IHADSS",	tp_default_border,	{{"BORESIGHT_IHADSS", 1}}},
		{ pb.L6, "B/S NOW",	nil,				{{"BORESIGHT_NOW", 1}}},
		{ pb.R3, {{"REMOVE", nil, {{"REMOVE_MESSAGE", 1}}},{"MESSAGE", nil, {{"REMOVE_MESSAGE", 1}}}}, nil },

		{ pb.R1, { {"LASER", nil, {{"WPN_BORESIGHT_LASER_Btn_Draw"},{"WPN_BORESIGHT_LASER_Btn_Color"}} }, {"SAFE", tp_default_border, {{"WPN_BORESIGHT_LASER_Btn_Draw"},{"WPN_BORESIGHT_LASER_Btn_Color"},{"WPN_BORESIGHT_LASER_Selection"}}, {"SAFE", "ARM"}} } },

		{ "TADS",
				{ 
					--{ pb.L1, "OUTFRONT",	nil, {{"BORESIGHT_TADS", TADS_OUTFRONT}}}, 	-- SME: This is only for ORT equipped aircraft, not MTADS equipped aircraft.
					{ pb.L2, "INTERNAL",	nil, {{"BORESIGHT_TADS", TADS_INTERNAL}}}, 
					{ pb.L3, "OFF",			nil, {{"BORESIGHT_TADS", TADS_OFF}}	} 
				},  
		},
	}
end
	
-----------------------------------------------------------
--------------------- Draw something ----------------------
-----------------------------------------------------------

createMenu( Menu, nil, 1, TRANSPARENT_BACKGROUND )
createControls( CommonControls, nil, nil, 1, TRANSPARENT_BACKGROUND )
createControls( UniqControls, nil, nil, 1, TRANSPARENT_BACKGROUND )

-----------------------------------------------------------
---------------------- Finally ----------------------------
-----------------------------------------------------------

