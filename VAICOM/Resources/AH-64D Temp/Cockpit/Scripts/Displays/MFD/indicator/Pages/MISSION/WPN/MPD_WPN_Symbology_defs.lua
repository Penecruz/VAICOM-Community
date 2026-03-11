dofile(LockOn_Options.script_path.."Displays/MFD/indicator/MFD_Symbology.lua")

local start_enum = 0
local count = start_enum
local function counter()
	count = count + 1
	return count
end
local function reset_counter(start_i)
	count = start_i or start_enum
end

-----------------------------------------------------------
-------------------- Prepare variables --------------------
-----------------------------------------------------------

ScreenSize = 1024
common_line_width = 3		-- same as info_box_line_width
wpn_texture_scale = 1/2048

WPN_SAFE_ARM_Icon_Pos = {0, 380}

-- Sizes of info windows
WPN_FirstLineWindows_W	= tp_default.width*7.4
WPN_FirstLineWindows_H	= tp_default.height*3.4
WPN_SecondLineWindows_W	= tp_default.width*5.75
WPN_SecondLineWindows_H	= tp_default.height*2.9

WPN_InfoWindows_LX		= -255
WPN_InfoWindows_RX		= -WPN_InfoWindows_LX
WPN_FirstLineWindows_Y	= pb_props[pb.L1].pos[2]+40
WPN_SecondLineWindows_Y	= pb_props[pb.L2].pos[2]+40

-- Pylons' position
WPN_Pylon_Pos_LO = {-247,					0}
WPN_Pylon_Pos_LI = {-145,					0}
WPN_Pylon_Pos_RI = {-WPN_Pylon_Pos_LI[1],	0}
WPN_Pylon_Pos_RO = {-WPN_Pylon_Pos_LO[1],	0}

WPN_Pylons_Pos = {
	WPN_Pylon_Pos_LO,
	WPN_Pylon_Pos_LI,
	WPN_Pylon_Pos_RI,
	WPN_Pylon_Pos_RO
}

-- Missile position on the pylon
WPN_MSL_Icon_UpL_Shift = {-24, 75}
WPN_MSL_Icon_UpR_Shift = {-WPN_MSL_Icon_UpL_Shift[1],	WPN_MSL_Icon_UpL_Shift[2]}
WPN_MSL_Icon_DnL_Shift = {WPN_MSL_Icon_UpL_Shift[1],	-WPN_MSL_Icon_UpL_Shift[2]+8}
WPN_MSL_Icon_DnR_Shift = {-WPN_MSL_Icon_UpL_Shift[1],	-WPN_MSL_Icon_UpL_Shift[2]+8}

WPN_MSL_Icons_Pos = {
	WPN_MSL_Icon_UpL_Shift,
	WPN_MSL_Icon_UpR_Shift,
	WPN_MSL_Icon_DnR_Shift,
	WPN_MSL_Icon_DnL_Shift
}

-- args for payload types
WPN_Payload_NonActive = 0
WPN_Payload_Activated = 1

-- args for SAFE/ARM icon
reset_counter(-1)
WPN_ARM_Icon = {
	SAFE		= counter();
	SAFE_ACT	= counter();
	ARM		 	= counter();
	ARM_ACT 	= counter();
}

-- Pylons indexes
reset_counter()
WPN_PylonNum = {
	LO	= counter();
	LI	= counter();
	RI 	= counter();
	RO 	= counter();
}

-- Pylons' info frames
local FrameScale = 2.40		-- more scale -> less symbol
local FrameMSL_x1, FrameMSL_y1	= -126/FrameScale, 390/FrameScale	-- 387.5 estimated
local FrameMSL_x2, FrameMSL_y2	= -FrameMSL_x1, -FrameMSL_y1
local FrameMSL_verts			= {{FrameMSL_x1 , FrameMSL_y1 }, {FrameMSL_x2 , FrameMSL_y1}, {FrameMSL_x2 , FrameMSL_y2}, {FrameMSL_x1 , FrameMSL_y2}}
local FrameRKT_x1, FrameRKT_y1	= -111/FrameScale, 309/FrameScale
local FrameRKT_x2, FrameRKT_y2	= -FrameRKT_x1, -FrameRKT_y1
local FrameRKT_verts			= {{FrameRKT_x1 , FrameRKT_y1 }, {FrameRKT_x2 , FrameRKT_y1}, {FrameRKT_x2 , FrameRKT_y2}, {FrameRKT_x1 , FrameRKT_y2}}

WPN_MSL_CODE_CHANNELS = {"A","B","C","D","E","F","G","H","J","K","L","M","N","P","Q","R","NONE"}
WPN_RKT_WARHEAD_TYPES = {"", "PD7", "RA7", "IL7", "SK7", "MP7", "FL7", "6PD", "6PD", "6RC", "6RC", "6IL", "6SK", "6MP", "6FL"}
WPN_LAUNCHER_FRAME_MSG = {"SAFE","FAIL","BIT","LOAD","DEGR"}
WPN_MISSILE_STATUS_TEXT = {"O\nT","","\nS","\nR","\nT","L","L\nS","A\nR","A\nT","N\nA","M\nU","S\nF","M\nF","M\nH","M\nA"}

--collimated		= false
--additive_alpha	= true

DBG_LABEL_SHOW = false
TRANSPARENT_BACKGROUND = false

CommonWPNPlaceholder = addPlaceholder("CommonWPNPlaceholder_PH", {0, 0}, nil, nil)

-----------------------------------------------------------
-------------------- Prepare functions --------------------
-----------------------------------------------------------

function DrawWPNTexPoly( verts, tex_coords, scale, controllers, parent, material, name, pos, is_visible )
	local elem  		= CreateElement "ceTexPoly"
	elem.vertices		= verts
	elem.indices		= default_box_indices
	elem.tex_params		= {tex_coords[1] * wpn_texture_scale, tex_coords[2] * wpn_texture_scale, scale * wpn_texture_scale, scale * wpn_texture_scale} 
	elem.isvisible		= is_visible==nil or is_visible
	
	setSymbolCommonProperties(elem, name, pos, parent, controllers, material)

	Add(elem)
	return elem
end
-----------------------------------------------------------
function DrawUniqueWPNSymbol(verts, tex_coords, scale, controllers, pos, material, parent, is_transparent)
	parent = parent or CommonWPNPlaceholder.name
	
	local fon_mat = "MFD_WPN_BLACK"
	
	if is_transparent==true then
		fon_mat = "MFD_WPN_BLACK_TRANSPARENT"
	end
	
	DrawWPNTexPoly( verts, tex_coords, scale, controllers, parent, fon_mat, nil, pos )
	local elem = DrawWPNTexPoly( verts, tex_coords, scale, controllers, parent, (material or IND_MPD_WPN_MATERIAL_GREEN), nil, pos )

	return elem
end
-----------------------------------------------------------
function Draw_SAFE_ARM_Icons()
	local scale		= 3.0	-- more scale -> less symbol
	
	local x1, y1	= -228/scale, 161/scale
	local x2, y2	= -x1, -y1
	
	local verts		= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}

	DrawUniqueWPNSymbol( verts, {742, 682.5},		scale, {{"WPN_SAFE_ARM_Status", WPN_ARM_Icon.SAFE}},		WPN_SAFE_ARM_Icon_Pos, IND_MPD_WPN_MATERIAL_GREEN,	nil, TRANSPARENT_BACKGROUND )
	DrawUniqueWPNSymbol( verts, {742, 1066.5},		scale, {{"WPN_SAFE_ARM_Status", WPN_ARM_Icon.SAFE_ACT}},	WPN_SAFE_ARM_Icon_Pos, IND_MPD_WPN_MATERIAL_GREEN,	nil, TRANSPARENT_BACKGROUND )
	DrawUniqueWPNSymbol( verts, {1215.5, 682.5},	scale, {{"WPN_SAFE_ARM_Status", WPN_ARM_Icon.ARM}},			WPN_SAFE_ARM_Icon_Pos, IND_MPD_WPN_MATERIAL_YELLOW,	nil, TRANSPARENT_BACKGROUND )
	DrawUniqueWPNSymbol( verts, {1215.5, 1066.5}, 	scale, {{"WPN_SAFE_ARM_Status", WPN_ARM_Icon.ARM_ACT}},		WPN_SAFE_ARM_Icon_Pos, IND_MPD_WPN_MATERIAL_YELLOW,	nil, TRANSPARENT_BACKGROUND )
end
-----------------------------------------------------------
-----------------------------------------------------------
function AddMissileSymbol(name, font, controllers, parent, pos, size, value)
	local font_size		= size or 130

	local elem			= CreateElement "ceStringPoly"
	elem.material		= font
	elem.alignment		= "CenterCenter"
	elem.stringdefs		= {font_size*GetScale(),font_size*GetScale()}
	elem.value			= value or ""

	setSymbolCommonProperties(elem, name, pos, parent, controllers, font)
	
	Add(elem)
	
	return elem
end
-----------------------------------------------------------
function Draw_HellfireMissiles()
	local fon_mat	= "MFD_WPN_MSL_BLACK"
	local tp_cc		= createTextProperty( 28,  nil,	nil,  "CenterCenter" )
	local tp_inv	= createTextProperty( 28,  nil,	nil,  "CenterCenter", true )
	local txt_pos	= {0, -20}
	
	tp_cc.stringdefs	= {tp_cc.height*GetScale(),tp_cc.width*GetScale(),tp_cc.width*GetScale(),tp_cc.height*GetScale()*0.1}	-- to set strings looser
	tp_inv.stringdefs	= {tp_inv.height*GetScale(),tp_inv.width*GetScale(),tp_inv.width*GetScale(),tp_inv.height*GetScale()*0.1}	-- to set strings looser
	
	local MissileSystem_PH = addPlaceholder("MissileSystem_PH", {0,0}, CommonWPNPlaceholder.name, {{"WPN_UTIL_MSL_System_Draw"}})

	local function getStatusControllers(showing_type, num)
		local controllers =
			{
				{"WPN_Missile_Status_Type", WPN_Payload_NonActive, num},
				{"WPN_Missile_Status_Show", num},
				{"WPN_Missile_SetStatus", num},
				{"WPN_Missile_SetColor", num}
			}
			
		if showing_type==WPN_Payload_Activated then
			controllers[1][2] = WPN_Payload_Activated
		end
		
		return controllers
	end
	
	if TRANSPARENT_BACKGROUND then
		fon_mat = "MFD_WPN_MSL_BLACK_TRANSPARENT"
	end
	
	for pylon = WPN_PylonNum.LO, WPN_PylonNum.RO do
		local MissileSetBase = addPlaceholder("MissileSet_"..pylon.."_PH", WPN_Pylons_Pos[pylon], MissileSystem_PH.name, {{"WPN_MSL_Launcher_Show", pylon-1}})
	
		for i = 1, 4 do
			local msl_num = (pylon-1)*4 + i-1
			
			local MissileIconBase = addPlaceholder("MissileIcon_"..msl_num.."_PH", WPN_MSL_Icons_Pos[i], MissileSetBase.name)

			local fon_elem =
			AddMissileSymbol("MissileIcon_"..msl_num.."_fon",	fon_mat,							{{"WPN_Missile_SetType", msl_num}},										MissileIconBase.name)

			addText("",	txt_pos,	tp_inv,	getStatusControllers(WPN_Payload_Activated, msl_num),
					WPN_MISSILE_STATUS_TEXT,	nil,	"Missile_"..msl_num.."_InvStatus",	MissileIconBase.name, h_clip_relations.REWRITE_LEVEL, DEFAULT_LEVEL+1, true, true)

			AddMissileSymbol("MissileIcon_"..msl_num,			fontPrefix.."WPN_MSL",				{{"WPN_Missile_SetType", msl_num},{"WPN_Missile_SetColor", msl_num}},	MissileIconBase.name)

			fon_elem.h_clip_relation 	= h_clip_relations.COMPARE
			fon_elem.level    			= DEFAULT_LEVEL
			--				text,	pos,		text_properties,	controllers,											formats,					margins,	name,								parent
			addSimpleText(	"",		txt_pos,	tp_cc,				getStatusControllers(WPN_Payload_NonActive, msl_num),	WPN_MISSILE_STATUS_TEXT,	nil,		"Missile_"..msl_num.."_Status",		MissileIconBase.name)

			-- add clipping region
			local scale		= 1.95	-- more scale -> less symbol

			local function addMaskElement(texture_pos, argument)
				local x1, y1 = -43.5/scale, 128/scale
				
				if argument == 0 then
					x1 = -17.0/scale
				end
				
				local x2, y2	= -x1, -y1
				local verts		= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}

				local MaskBase = addPlaceholder("Missile_"..msl_num.."_Mask_PH_"..argument, {0,0}, MissileIconBase.name, {{"WPN_MissileMask_SetType", msl_num, argument}})
				local mask = DrawWPNTexPoly( verts, texture_pos, scale, nil, MaskBase.name, "MFD_WPN_MSL_DBG_MASK", nil, {0,0}, false )
				mask.h_clip_relation 	= h_clip_relations.INCREASE_IF_LEVEL
				mask.level    			= DEFAULT_LEVEL
			end

			addMaskElement({127, 146},		0)
			addMaskElement({127.5, 383.5},	1)
		end
		
		Add_Frame_MSL_Icon(FrameMSL_verts, FrameScale, {0,-13}, pylon-1, MissileSetBase.name)
	end
end
-----------------------------------------------------------
-----------------------------------------------------------
local function addMaskBox(pylon_ind, line_ind, init_pos, name_pref, parent)
	local k_x, k_y = 1, 1
	
	if (line_ind==0 or line_ind==2) then
		k_x = -1
	end
		
	if (line_ind==2 or line_ind==3) then
		k_y = -1
	end

	local pos = {init_pos[1]*k_x, init_pos[2]*k_y}
	
	local mask = addBox(name_pref..pylon_ind.."_Mask_"..line_ind, 6, 100, "CenterCenter", pos, parent, nil, "BLUE")
	mask.h_clip_relation 	= h_clip_relations.INCREASE_IF_LEVEL
	mask.level    			= DEFAULT_LEVEL
	mask.isvisible			= false
end
-----------------------------------------------------------
function Add_Frame_MSL_Icon(frame_verts, scale, pos, index, parent)
	local tps				= createTextProperty( nil, "WHITE",	IND_MPD_MATERIAL_WHITE, "CenterCenter" )
	local tps_inv			= createTextProperty( nil, "WHITE",	IND_MPD_MATERIAL_WHITE, "CenterCenter", true )
	local txt_pos			= {0, frame_verts[3][2] + tps.height*0.7}	-- FrameMSL_y2

	local tex_coords 		= {523, 1642.5}	-- 1641,5 estimated
	local dummy_tex_coords 	= {195.5, 1637.5}

	local function add_text_lbl(name, parent_name, frame_type, material, txt_props, verts)
		local Base = addPlaceholder(name.."_PH", txt_pos, parent_name, {{"WPN_MSL_System_Activated", frame_type}})

		if txt_props.inv~=true and TRANSPARENT_BACKGROUND then
			DrawWPNTexPoly( verts, dummy_tex_coords, scale, nil, Base.name, material, nil, {0,1} )
			
			addText("SAFE",	{0,0}, txt_props, {{"WPN_Launcher_FrameMsg_Value", index}}, WPN_LAUNCHER_FRAME_MSG, nil, name.."_lbl", Base.name,
					nil, nil, txt_props.inv, TRANSPARENT_BACKGROUND)
				
		else
			addText("SAFE",	{0,0}, txt_props, {{"WPN_Launcher_FrameMsg_Value", index}}, WPN_LAUNCHER_FRAME_MSG, nil, name.."_lbl", Base.name,
					h_clip_relations.REWRITE_LEVEL, DEFAULT_LEVEL+1, txt_props.inv, TRANSPARENT_BACKGROUND)
	
			local dummy_elem 				= DrawUniqueWPNSymbol( verts, dummy_tex_coords, scale, nil, {0,1}, material, Base.name, TRANSPARENT_BACKGROUND )
			dummy_elem.h_clip_relation		= h_clip_relations.COMPARE
			dummy_elem.level    			= DEFAULT_LEVEL
		end
	end

	-- base --
	local FrameBase = addPlaceholder("FrameMSLIcon_"..index.."_PH", pos, parent or CommonWPNPlaceholder.name, {{"WPN_Launcher_FrameIcon_Show", index}})
	local FrameElem = DrawUniqueWPNSymbol( frame_verts, tex_coords, scale, {{"WPN_Launcher_FrameIcon_Color", index}}, {0,0}, IND_MPD_WPN_MATERIAL_WHITE, FrameBase.name, TRANSPARENT_BACKGROUND )

	-- regular --
	do
		local x1, y1			= -118.0/scale, 37.5/scale
		local x2, y2			= -x1, -y1
		local dummy_verts		= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
		local fon_mat			= "MFD_WPN_BLACK"

		if TRANSPARENT_BACKGROUND==true then
			fon_mat = "MFD_WPN_BLACK_TRANSPARENT"
		end

		add_text_lbl("FrameMSLTxtPatch_"..index, FrameElem.name, WPN_Payload_NonActive, fon_mat, tps, dummy_verts)
	end

	--- inv ---
	do
		local x1, y1			= -123.5/scale, 46.5/scale
		local x2, y2			= -x1, -y1
		local dummy_verts		= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
		
		add_text_lbl("FrameMSLInvTxtPatch_"..index, FrameElem.name, WPN_Payload_Activated, IND_MPD_WPN_MATERIAL_WHITE, tps_inv, dummy_verts)
	end

	-- clipping regions (for plane symbol) --
	for i=0,3 do
		addMaskBox(index, i, {50.5, 70}, "FrameMSL_", FrameBase.name)
	end
	
	return FrameBase
end
-----------------------------------------------------------
function Add_Frame_RKT_Icon(frame_verts, scale, pos, index, parent)
	local tps				= createTextProperty( nil, "YELLOW", IND_MPD_MATERIAL_YELLOW, "CenterCenter" )
	local tps_inv			= createTextProperty( nil, "YELLOW", IND_MPD_MATERIAL_YELLOW, "CenterCenter", true )
	local txt_pos			= {0, frame_verts[3][2] + tps.height*0.7}	-- FrameRKT_y2
	
	local tex_coords 		= {778, 1721}	
	local dummy_tex_coords 	= {195.5, 1637.5}

	local function add_text_lbl(name, parent_name, frame_type, material, txt_props, verts)
		local Base = addPlaceholder(name.."_PH", txt_pos, parent_name, {{"WPN_RKT_System_Activated", frame_type}})

		if txt_props.inv~=true and TRANSPARENT_BACKGROUND then
			DrawWPNTexPoly( verts, dummy_tex_coords, scale, nil, Base.name, material, nil, {0,0} )
			
			addText("SAFE",	{0,0}, txt_props, {{"WPN_Launcher_FrameMsg_Value", index}}, WPN_LAUNCHER_FRAME_MSG, nil, name.."_lbl", Base.name,
					nil, nil, txt_props.inv, TRANSPARENT_BACKGROUND)
				
		else
			addText("SAFE",	{0,0}, txt_props, {{"WPN_Launcher_FrameMsg_Value", index}}, WPN_LAUNCHER_FRAME_MSG, nil, name.."_lbl", Base.name,
					h_clip_relations.REWRITE_LEVEL, DEFAULT_LEVEL+1, txt_props.inv, TRANSPARENT_BACKGROUND)
	
			local dummy_elem 				= DrawUniqueWPNSymbol( verts, dummy_tex_coords, scale, nil, {0,0}, material, Base.name, TRANSPARENT_BACKGROUND )
			dummy_elem.h_clip_relation		= h_clip_relations.COMPARE
			dummy_elem.level    			= DEFAULT_LEVEL
		end
	end

	-- base --
	local FrameBase = addPlaceholder("FrameRKTIcon_"..index.."_PH", pos, parent or CommonWPNPlaceholder.name, {{"WPN_Launcher_FrameIcon_Show", index}})
	local FrameElem = DrawUniqueWPNSymbol( frame_verts, tex_coords, scale, nil, {0,0}, IND_MPD_WPN_MATERIAL_YELLOW, FrameBase.name, TRANSPARENT_BACKGROUND )

	-- regular --
	do
		local x1, y1			= -103.0/scale, 39.0/scale
		local x2, y2			= -x1, -y1
		local dummy_verts		= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
		local fon_mat			= "MFD_WPN_BLACK"

		if TRANSPARENT_BACKGROUND==true then
			fon_mat = "MFD_WPN_BLACK_TRANSPARENT"
		end

		add_text_lbl("FrameRKTTxtPatch_"..index, FrameElem.name, WPN_Payload_NonActive, fon_mat, tps, dummy_verts)
	end

	--- inv ---
	do
		local x1, y1			= -108.5/scale, 46.5/scale
		local x2, y2			= -x1, -y1
		local dummy_verts		= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
		
		add_text_lbl("FrameRKTInvTxtPatch_"..index, FrameElem.name, WPN_Payload_Activated, IND_MPD_WPN_MATERIAL_YELLOW, tps_inv, dummy_verts)
	end

	-- clipping regions (for plane symbol) --
	for i=0,3 do
		addMaskBox(index, i, {44, 50}, "FrameRKT_", FrameBase.name)
	end

	return FrameBase
end
-----------------------------------------------------------
-----------------------------------------------------------
function Add_RKT_NAct_Icon(verts, scale, pos, txt_pos, index, parent)
	local tex_coords = {1664, 763}

	local RKTBase = addPlaceholder("RKTIcon_NA_"..index.."_PH", pos, parent or CommonWPNPlaceholder.name, {{"WPN_RKT_System_Activated", WPN_Payload_NonActive}})
	
	local elem = DrawUniqueWPNSymbol( verts, tex_coords, scale, nil, {0,0}, IND_MPD_WPN_MATERIAL_GREEN, RKTBase.name, TRANSPARENT_BACKGROUND )
	elem.h_clip_relation 	= h_clip_relations.REWRITE_LEVEL
	elem.level    			= DEFAULT_LEVEL
	
	local tps		= createTextProperty( 36, nil, nil, "CenterCenter" )
	addSimpleText("6RC",	txt_pos, tps, {{"WPN_RKT_SelectedInventory_CaptionShow", index},{"WPN_RKT_SelectedInventory_Caption", index}}, WPN_RKT_WARHEAD_TYPES, nil, "RocketsNA_"..index.."_lbl", elem.name)

	return elem
end
-----------------------------------------------------------
function Add_RKT_Act_Icon(verts, scale, pos, txt_pos, index, parent)
	local tex_coords = {1920, 763}
	local tps_inv	= createTextProperty( 36, nil, nil, "CenterCenter", true )

	local RKTBase = addPlaceholder("RKTIcon_Act_"..index.."_PH", pos, parent or CommonWPNPlaceholder.name, {{"WPN_RKT_System_Activated", WPN_Payload_Activated}})

	addText("6RC",	txt_pos, tps_inv, {{"WPN_RKT_SelectedInventory_CaptionShow", index},{"WPN_RKT_SelectedInventory_Caption", index}}, WPN_RKT_WARHEAD_TYPES,
			nil, "RocketsAct_"..index.."_lbl", RKTBase.name, h_clip_relations.REWRITE_LEVEL, DEFAULT_LEVEL+1, true, TRANSPARENT_BACKGROUND)
			
	local elem = DrawUniqueWPNSymbol( verts, tex_coords, scale, nil, {0,0}, IND_MPD_WPN_MATERIAL_GREEN, RKTBase.name, TRANSPARENT_BACKGROUND )
	elem.h_clip_relation 	= h_clip_relations.COMPARE
	elem.level    			= DEFAULT_LEVEL

	return elem
end
-----------------------------------------------------------
function Draw_RocketLaunchers()
	local scale		= 2.45	-- more scale -> less symbol
	
	local x1, y1	= -101/scale, 257/scale
	local x2, y2	= -x1, -y1
	
	local verts		= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	
	local txt_pos	= {0, tp_default.height*0.5}
	
	local RocketsSystem_PH = addPlaceholder("RocketsSystem_PH", {0,0}, CommonWPNPlaceholder.name, {{"WPN_UTIL_RKT_System_Draw"}})
	
	for pylon = WPN_PylonNum.LO, WPN_PylonNum.RO do
		local RocketLauncherBase = addPlaceholder("RocketLauncher_"..pylon.."_PH", WPN_Pylons_Pos[pylon], RocketsSystem_PH.name, {{"WPN_RKT_Launcher_Show", pylon-1}})
		
		Add_RKT_NAct_Icon(verts, scale, {0,0}, txt_pos, pylon-1, RocketLauncherBase.name)
		Add_RKT_Act_Icon(verts, scale, {0,0}, txt_pos, pylon-1, RocketLauncherBase.name)

		Add_Frame_RKT_Icon(FrameRKT_verts, FrameScale, {0,-18}, pylon-1, RocketLauncherBase.name)

		-- add clipping region
		local mask_scale	= 0.96
		local m_x1, m_y1	= x1/mask_scale, y1/mask_scale
		local m_x2, m_y2	= x2, y2
		local m_verts		= {{m_x1 , m_y1 }, {m_x2 , m_y1}, {m_x2 , m_y2}, {m_x1 , m_y2}}
	
		local mask = DrawWPNTexPoly( m_verts, {1920, 763}, scale*mask_scale, nil, RocketLauncherBase.name, "MFD_WPN_DBG_MASK", nil, {0.8, 0}, false )
		mask.h_clip_relation 	= h_clip_relations.INCREASE_IF_LEVEL
		mask.level    			= DEFAULT_LEVEL
	end
end
-----------------------------------------------------------
-----------------------------------------------------------
function Add_GUN_NAct_Icon(verts, scale, pos, txt_pos, parent)
	local tex_coords	= {195.5, 808.5}

	local elem = DrawUniqueWPNSymbol( verts, tex_coords, scale, {{"WPN_GUN_System_Activated", WPN_Payload_NonActive}}, pos, IND_MPD_WPN_MATERIAL_GREEN, parent, TRANSPARENT_BACKGROUND )
	elem.h_clip_relation 	= h_clip_relations.COMPARE
	elem.level    			= DEFAULT_LEVEL
	
	local tps36		= createTextProperty( 36, nil, nil, "CenterCenter" )
	addText("1150", txt_pos, tps36, {{"WPN_GUN_RoundsCounter_Show"}, {"WPN_GUN_RoundsCounter_Value"}}, nil, nil, "GUN_RoundsCounter_lbl", elem.name, nil, nil, nil, TRANSPARENT_BACKGROUND)
	
	return elem
end
-----------------------------------------------------------
function Add_GUN_Act_Icon(verts, scale, pos, txt_pos, parent)
	local tex_coords	= {195.5, 1437.5}
	local tps36_inv	= createTextProperty( 36, nil, nil, "CenterCenter", true )
	
	addText("1150", txt_pos, tps36_inv, {{"WPN_GUN_System_Activated", WPN_Payload_Activated}, {"WPN_GUN_RoundsCounter_Show"}, {"WPN_GUN_RoundsCounter_Value"}},
			nil, nil, "GUN_RoundsCounter_lbl_inv", parent, h_clip_relations.REWRITE_LEVEL, DEFAULT_LEVEL+1, true, TRANSPARENT_BACKGROUND)

	local elem = DrawUniqueWPNSymbol( verts, tex_coords, scale, {{"WPN_GUN_System_Activated", WPN_Payload_Activated}}, pos, IND_MPD_WPN_MATERIAL_GREEN, parent, TRANSPARENT_BACKGROUND )
	elem.h_clip_relation 	= h_clip_relations.COMPARE
	elem.level    			= DEFAULT_LEVEL

	return elem
end
-----------------------------------------------------------
function Draw_GUN()
	local scale		= 2.7	-- more scale -> less symbol
	
	local x1, y1	= -183/scale, 286/scale
	local x2, y2	= -x1, -y1
	
	local verts		= {{x1 , y1 }, {x2 , y1}, {x2 , y2}, {x1 , y2}}
	
	local txt_pos	= {0, y2 + tp_36.height*0.82}
	
	local GunBase = addPlaceholder("GUN_PlaceHolder", {0, 178}, CommonWPNPlaceholder.name, {{"WPN_UTIL_GUN_System_Draw"}})

	local tps36_inv	= createTextProperty( 36, "YELLOW", IND_MPD_MATERIAL_YELLOW, "CenterCenter", true )
	addText("FAIL", txt_pos, tps36_inv, {{"WPN_GUN_FAIL_Lbl_Show"}}, nil, nil, "GUN_FailIndicator_lbl", GunBase.name, h_clip_relations.REWRITE_LEVEL, DEFAULT_LEVEL+1, true, TRANSPARENT_BACKGROUND)

	Add_GUN_NAct_Icon(verts, scale, {0,0}, txt_pos, GunBase.name)
	Add_GUN_Act_Icon(verts, scale, {0,0}, txt_pos, GunBase.name)
end
-----------------------------------------------------------
-----------------------------------------------------------
function Add_Plane_Icon()
	local width = info_box_line_width
	
	local Plane_PH = addPlaceholder("PlaneFigure_PH", {0,-102}, nil, nil)
	
	local nose_tbl_len = 57
	local nose = 
	{
	-- right
	{85.00,		341.2082},
	{84.82,		342.3028},
	{84.62,		343.3922},
	{84.39,		344.4738},
	{84.13,		345.5478},
	{83.85,		346.6072},
	{83.54,		347.6689},
	{83.21,		348.7261},
	{82.85,		349.7760},
	{82.46,		350.8190},
	{82.04,		351.8453},
	{81.60,		352.8773},
	{81.12,		353.9002},
	{80.62,		354.9193},
	{80.08,		355.9296},
	{79.51,		356.9339},
	{78.91,		357.9226},
	{78.27,		358.9130},
	{77.60,		359.8980},
	{76.89,		360.9162},
	{76.15,		361.8905},
	{75.36,		362.8455},
	{74.53,		363.8038},
	{73.65,		364.7535},
	{72.73,		365.6972},
	{71.76,		366.6331},
	{70.73,		367.5494},
	{69.65,		368.4686},
	{68.50,		369.3769},
	{67.29,		370.2771},
	{66.00,		371.1655},
	{64.63,		372.0427},
	{63.17,		372.8975},
	{61.61,		373.7484},
	{59.93,		374.5823},
	{58.12,		375.3978},
	{56.14,		376.1879},
	{53.96,		376.9370},
	{51.54,		377.6556},
	{48.78,		378.5593},
	{45.53,		379.1876},
	{41.46,		379.6626},
	{35.42,		379.6387},
	-- nose top
	{35.32,		379.63},
	{34.74,		383.10},
	{33.15,		388.60},
	{32.21,		391.50},
	{30.03,		395.40},
	{28.73,		397.00},
	{26.96,		398.40},
	{25.56,		399.50},
	{23.57,		401.10},
	{21.12,		402.60},
	{19.12,		403.50},
	{18.28,		404.10},
	{15.68,		405.00},
	{10.48,		405.60},
	}	-- end
	
	-- reflect
	addVertsMirroredAboutY(nose)
	
	----------------------------
	
	local wing_length	= 247
	local tail_len		= 190
	local body_tbl_len	= 6

	local wing_left_origin = { -85, 341.51 }
	
	local body =
	{
	wing_left_origin,
	{wing_left_origin[1],				170.5},
	{wing_left_origin[1]-wing_length,	150.9},
	{wing_left_origin[1]-wing_length,	51.6},
	{wing_left_origin[1],				18.6},
	{wing_left_origin[1],				-171.4},
	}
	
	-- reflect
	addVertsMirroredAboutY(body)
	
	--			verts,	material,					parent,			width,	name,	controllers,	h_clip_relation,			level
	draw_line( body,	IND_MPD_MATERIAL_GREEN, 	Plane_PH.name,	width,	nil,	nil,			h_clip_relations.COMPARE,	DEFAULT_LEVEL )
	draw_line( nose, 	IND_MPD_MATERIAL_GREEN, 	Plane_PH.name,	width,	nil,	nil,			h_clip_relations.COMPARE,	DEFAULT_LEVEL )

end
-----------------------------------------------------------
-----------------------------------------------------------
function AddSightStatusWindow()
	local lbl_pos = {WPN_InfoWindows_LX + WPN_FirstLineWindows_W/2, WPN_FirstLineWindows_Y}
	
	local tps		= createTextProperty( nil, nil, nil, "CenterCenter" )
	tps.stringdefs	= {tps.height*GetScale(),tps.height*GetScale(),tps.height*GetScale()*0.05}	-- to set symbols looser
	
	AddRoundCornersWindow("SightStatusWindow",	lbl_pos,	WPN_FirstLineWindows_W, WPN_FirstLineWindows_H,
						{
							{"SIGHT",	{0,	tps.height*0.70},	tps,	nil},
							{"HMD",		{0,	-tps.height*0.70},	tps,	{{"WPN_SIGHT_Selection"}}, {"HMD","TADS","FCR"}}
						},
						tps,	IND_MPD_MATERIAL_GREEN,	CommonWPNPlaceholder.name,	nil, nil, nil, nil, TRANSPARENT_BACKGROUND)
end
-----------------------------------------------------------
function AddAcqusitionSelectStatusWindow()
	local lbl_pos = {WPN_InfoWindows_RX - WPN_FirstLineWindows_W/2, WPN_FirstLineWindows_Y}
	
	local tps		= createTextProperty( nil, nil, nil, "CenterCenter" )
	tps.stringdefs	= {tps.height*GetScale(),tps.height*GetScale(),tps.height*GetScale()*0.05}	-- to set symbols looser
	
	AddRoundCornersWindow("AcqusitionSelectStatusWindow",	lbl_pos,	WPN_FirstLineWindows_W, WPN_FirstLineWindows_H,
						{
							{"ACQ",		{0,	tps.height*0.70},	tps,	nil},
							{"FXD",		{0,	-tps.height*0.70},	tps,	{{"DSPLS_TSD_ACQ_Button_Caption"}}, {"PHS", "GHS", "SKR", "RFI", "FCR", "FXD", "TADS", "?00", "TRN", "ASE", "?PHS", "?GHS", "?SKR", "?RFI", "?FCR", "?FXD", "?TADS", "?00", "?TRN", "?ASE"}}
						},
						tps,	IND_MPD_MATERIAL_GREEN,	CommonWPNPlaceholder.name,	nil, nil, nil, nil, TRANSPARENT_BACKGROUND)
end
-----------------------------------------------------------
function AddLSTStatusWindow()
	local lbl_pos = {WPN_InfoWindows_LX + WPN_SecondLineWindows_W/2, WPN_SecondLineWindows_Y}
	
	local tps		= createTextProperty( nil, nil, nil, "CenterCenter" )
	tps.stringdefs	= {tps.height*GetScale(),tps.height*GetScale(),tps.height*GetScale()*0.05}	-- to set symbols looser
	
	AddRoundCornersWindow("LSTStatusWindow",	lbl_pos,	WPN_SecondLineWindows_W, WPN_SecondLineWindows_H,
						{
							{"LST",		{0,	tps.height*0.60},	tps,	nil},
							{"A",		{0,	-tps.height*0.65},	tps,	{{"WPN_LST_Code_Selection"}}, WPN_MSL_CODE_CHANNELS}
						},
						tps,	IND_MPD_MATERIAL_GREEN,	CommonWPNPlaceholder.name,	nil, nil, nil, nil, TRANSPARENT_BACKGROUND)
end
-----------------------------------------------------------
function AddLRFDStatusWindow()
	local lbl_pos = {WPN_InfoWindows_RX - WPN_SecondLineWindows_W/2, WPN_SecondLineWindows_Y}
	
	local tps		= createTextProperty( nil, nil, nil, "CenterCenter" )
	tps.stringdefs	= {tps.height*GetScale(),tps.height*GetScale(),tps.height*GetScale()*0.05}	-- to set symbols looser
	
	AddRoundCornersWindow("LRFDStatusWindow",	lbl_pos,	WPN_SecondLineWindows_W, WPN_SecondLineWindows_H,
						{
							{"LRFD",	{0,	tps.height*0.60},	tps,	nil},
							{"B",		{0,	-tps.height*0.65},	tps,	{{"WPN_LRFD_Code_Selection"}}, WPN_MSL_CODE_CHANNELS}
						},
						tps,	IND_MPD_MATERIAL_GREEN,	CommonWPNPlaceholder.name,	nil, nil, nil, nil, TRANSPARENT_BACKGROUND)
end
-----------------------------------------------------------
function AddCHAFFStatusWindow()
	local lbl_pos = {0, pb_props[pb.L5].pos[2] - tp_default.height}
	
	local tps		= createTextProperty( nil, nil, nil, "CenterCenter" )
	local tps36		= createTextProperty( 36, nil, nil, "CenterCenter" )
	local tps36_inv	= createTextProperty( 36, "YELLOW",IND_MPD_MATERIAL_YELLOW, "CenterCenter", true )
	tps36.stringdefs= {tps36.height*GetScale(),tps36.height*GetScale(),tps36.height*GetScale()*0.05}	-- to set symbols looser
		
	AddRoundCornersWindow("CHAFFStatusWindow",	lbl_pos,	WPN_FirstLineWindows_W*0.97, WPN_FirstLineWindows_H*1.55,
						{
							{"CHAFF",	{0,	tps.height*1.67},	tps,		nil},
							{"30",		{0,	tps.height*0.12},	tps36,		{{"WPN_CHAFF_Counter_Value"}}},
							{"SAFE",	{0,	-tps.height*1.65},	tps36,		{{"WPN_CHAFF_ArmedLbl_Show", WPN_Payload_NonActive}}},
							{"ARM",		{0,	-tps.height*1.65},	tps36_inv,	{{"WPN_CHAFF_ArmedLbl_Show", WPN_Payload_Activated}}}
						},
						tps,	IND_MPD_MATERIAL_GREEN,	CommonWPNPlaceholder.name,	nil, nil, nil, nil, TRANSPARENT_BACKGROUND)
end
-----------------------------------------------------------
function DrawCommonInfoWindows()
	AddSightStatusWindow()
	AddAcqusitionSelectStatusWindow()
	AddLSTStatusWindow()
	AddLRFDStatusWindow()
	
	AddCHAFFStatusWindow()
end
-----------------------------------------------------------
-----------------------------------------------------------
function Add_MSL_Channels_StatusWindow()
	local lbl_pos = {0, pb_props[pb.L6].pos[2]-81}

	local tps			= createTextProperty( nil, nil, nil, "CenterCenter" )
	local tps36			= createTextProperty( 36, nil, nil, "CenterCenter" )
	tps.stringdefs		= {tps.height*GetScale(),tps.height*GetScale(),tps.height*GetScale()*0.05}	-- to set symbols looser
	tps36.stringdefs	= {tps36.height*GetScale(),tps36.height*GetScale(),tps36.height*GetScale()*0.05}	-- to set symbols looser

	local Lbl_W, Lbl_H = tps.width*24.0, tps.height*5.7

	local dx, dy 	= Lbl_W/4.0, Lbl_H/3.9
	local line1_y	= -tps.height*0.1
	local line2_y	= line1_y - dy

	local pos_ch1 = {-Lbl_W/2 + dx/2.0,	line1_y}
	local pos_ch2 = {pos_ch1[1] + dx,	line1_y}
	local pos_ch3 = {pos_ch2[1] + dx,	line1_y}
	local pos_ch4 = {pos_ch3[1] + dx,	line1_y}

	local pos_code1 = {pos_ch1[1], line2_y}
	local pos_code2 = {pos_ch2[1], line2_y}
	local pos_code3 = {pos_ch3[1], line2_y}
	local pos_code4 = {pos_ch4[1], line2_y}

	dy = (line2_y - line1_y)/2
	local pos_frame = {
		{pos_ch1[1], line1_y + dy},
		{pos_ch2[1], line1_y + dy},
		{pos_ch3[1], line1_y + dy},
		{pos_ch4[1], line1_y + dy}
	}

	local frame_W = tps.width*3.8
	local frame_H = frame_W*1.16

	local elem = AddRoundCornersWindow("MSL_Channels_StatusWindow", lbl_pos, Lbl_W, Lbl_H,
						{
							{"CHANNELS",	{0,	tps.height*1.90},	tps,	nil},
							{"1",			pos_ch1,				tps,	{{"WPN_MSL_SAL_Channels_Caption", 0}},		{"PRI", "ALT", "1"}},
							{"A",			pos_code1,				tps36,	{{"WPN_MSL_SAL_ChannelsCodes_Caption", 0}}, WPN_MSL_CODE_CHANNELS},
							{"PRI",			pos_ch2,				tps,	{{"WPN_MSL_SAL_Channels_Caption", 1}},		{"PRI", "ALT", "2"}},
							{"B",			pos_code2,				tps36,	{{"WPN_MSL_SAL_ChannelsCodes_Caption", 1}}, WPN_MSL_CODE_CHANNELS},
							{"3",			pos_ch3,				tps,	{{"WPN_MSL_SAL_Channels_Caption", 2}},		{"PRI", "ALT", "3"}},
							{"C",			pos_code3,				tps36,	{{"WPN_MSL_SAL_ChannelsCodes_Caption", 2}}, WPN_MSL_CODE_CHANNELS},
							{"ALT",			pos_ch4,				tps,	{{"WPN_MSL_SAL_Channels_Caption", 3}},		{"PRI", "ALT", "4"}},
							{"D",			pos_code4,				tps36,	{{"WPN_MSL_SAL_ChannelsCodes_Caption", 3}}, WPN_MSL_CODE_CHANNELS},
						},
						tps,	IND_MPD_MATERIAL_GREEN,	CommonWPNPlaceholder.name,	nil, nil, nil, nil, TRANSPARENT_BACKGROUND)

	for i=1,4 do
		local Frame_PH = addPlaceholder("MSL_Channels_Status_Frame"..i.."_PH", pos_frame[i], elem.name)
		
		local verts = {
			{-frame_W/2, 	-frame_H/2},
			{-frame_W/2, 	frame_H/2},
			{frame_W/2, 	frame_H/2},
			{frame_W/2, 	-frame_H/2},
			{-frame_W/2, 	-frame_H/2}
		}

		draw_line( verts, IND_MPD_MATERIAL_GREEN, Frame_PH.name, common_line_width, "MSL_Channels_Status_ALTFrame"..i, {{"WPN_MSL_SAL_StatWindow_ALT_Chan_Frame", i-1}} )
		draw_line( verts, IND_MPD_MATERIAL_WHITE, Frame_PH.name, common_line_width, "MSL_Channels_Status_PRIFrame"..i, {{"WPN_MSL_SAL_StatWindow_PRI_Chan_Frame", i-1}} )
	end
end
-----------------------------------------------------------
function Add_GUN_DH_HintWindow()
	local STATUS_ENGAGE		= 1
	local STATUS_FIRE		= 2
	local STATUS_STORE		= 3
	local STATUS_NOTVALID	= 4

	local lbl_2s_pos = {0, pb_props[pb.L6].pos[2]-30}
	local lbl_3s_pos = {0, pb_props[pb.L6].pos[2]-55}

	local tps		= createTextProperty( nil, nil, nil, "CenterCenter" )
	tps.stringdefs	= {tps.height*GetScale(),tps.height*GetScale(),tps.height*GetScale()*0.05}	-- to set symbols looser

	local Lbl_W, Lbl_H = tps.width*15.5, tps.height*3.0

	local line1_y = tps.height*0.7
	local line2_y = -line1_y
	
	local HintWindowBase = addPlaceholder("GUN_DH_HintWindow_PlaceHolder", {0, 0}, CommonWPNPlaceholder.name, {{"WPN_GUN_HintWindow_Show"}})

	AddRoundCornersWindow("GUN_DH_HintWindow_ENGAGE", lbl_2s_pos, Lbl_W, Lbl_H,
						{
							{"ENGAGE IAT",		{0,	line1_y},	tps,	nil},
							{"NFOV DTU/FLIR",	{0,	line2_y},	tps,	nil},
						},
						tps, IND_MPD_MATERIAL_GREEN, HintWindowBase.name, {{"WPN_GUN_HintWindowState_Value", STATUS_ENGAGE}},
						nil, nil, nil, TRANSPARENT_BACKGROUND)

	AddRoundCornersWindow("GUN_DH_HintWindow_STORE", lbl_2s_pos, Lbl_W, Lbl_H,
						{
							{"STORE",			{0,	line1_y},	tps,	nil},
							{"ROUNDS IMPACT",	{0,	line2_y},	tps,	nil},
						},
						tps, IND_MPD_MATERIAL_GREEN, HintWindowBase.name, {{"WPN_GUN_HintWindowState_Value", STATUS_STORE}},
						nil, nil, nil, TRANSPARENT_BACKGROUND)

	AddRoundCornersWindow("GUN_DH_HintWindow_NOTVALID", lbl_2s_pos, Lbl_W, Lbl_H,
						{
							{"GUN DH",			{0,	line1_y},	tps,	nil},
							{"NOT VALID",		{0,	line2_y},	tps,	nil},
						},
						tps, IND_MPD_MATERIAL_GREEN, HintWindowBase.name, {{"WPN_GUN_HintWindowState_Value", STATUS_NOTVALID}},
						nil, nil, nil, TRANSPARENT_BACKGROUND)

	Lbl_W, Lbl_H = tps.width*19.0, tps.height*5.0

	line1_y			= tps.height*1.7
	local line3_y	= -line1_y

	AddRoundCornersWindow("GUN_DH_HintWindow_FIRE", lbl_3s_pos, Lbl_W, Lbl_H,
						{
							{"FIRE GUN",			{0,	line1_y},	tps,	nil},
							{"10 ROUND BURST",		{0,	0},			tps,	nil},
							{"RANGE: 500-1500 M",	{0,	line3_y},	tps,	nil},
						},
						tps, IND_MPD_MATERIAL_GREEN, HintWindowBase.name, {{"WPN_GUN_HintWindowState_Value", STATUS_FIRE}},
						nil, nil, nil, TRANSPARENT_BACKGROUND)
end
-----------------------------------------------------------
function Add_UTIL_RTCA_StatusWindow()
	local lbl_pos = {0, pb_props[pb.L1].pos[2]-25}

	local tps_cc		= createTextProperty( nil, nil, nil, "CenterCenter" )
	tps_cc.stringdefs	= {tps_cc.height*GetScale(),tps_cc.height*GetScale(),tps_cc.height*GetScale()*0.05}	-- to set symbols looser
	local tps_lc		= createTextProperty( nil, nil, nil, "LeftCenter" )
	tps_lc.stringdefs	= tps_cc.stringdefs

	local Lbl_W, Lbl_H = tps_cc.width*22.5, tps_cc.height*3.9

	local line1_y = tps_cc.height*1.3
	local line3_y = -line1_y
	
	local line23_x = -Lbl_W/2 + tps_cc.width*0.7
	local line2_x = tps_cc.width*1.05
	local line3_x = tps_cc.width*2.1

	AddRoundCornersWindow("UTIL_RTCA_StatusWindow", lbl_pos, Lbl_W, Lbl_H,
						{
							{"TESS CASUALTY",			{0,			line1_y},	tps_cc,	nil},
							{"INDICATION:",				{line23_x,	0},			tps_lc,	nil},
							{"NEAR MISS",				{line2_x,	0},			tps_lc,	{{"WPN_UTIL_TESS_RTCA_Status"}}},
							{"WPN ID CODE:",			{line23_x,	line3_y},	tps_lc,	nil},
							{"23",						{line3_x,	line3_y},	tps_lc,	{{"WPN_UTIL_TESS_RTCA_ID"}}},
						},
						tps, IND_MPD_MATERIAL_GREEN, CommonWPNPlaceholder.name, {{"WPN_UTIL_TESS_RTCA_Show"}}, nil, nil, nil, TRANSPARENT_BACKGROUND)
end
-----------------------------------------------------------
function Add_RKT_TotalRocketsWindow()
	local lbl_pos = {0, pb_props[pb.L6].pos[2]-45}
	
	local tps		= createTextProperty( nil, "WHITE",	IND_MPD_MATERIAL_WHITE, "CenterCenter" )
	tps.stringdefs	= {tps.height*GetScale(),tps.height*GetScale(),tps.height*GetScale()*0.05}	-- to set symbols looser
	
	local Lbl_W, Lbl_H = tps.width*15.0, tps.height*3.0

	local line1_y = tps.height*0.7
	local line2_y = -line1_y

	AddRoundCornersWindow("RKT_TotalRocketsWindow", lbl_pos, Lbl_W, Lbl_H,
						{
							{"TOTAL ROCKETS",	{0,	line1_y},	tps,	nil},
							{"24",				{0,	line2_y},	tps,	{{"WPN_RKT_SelectedInventory_TotalCount"}}},
						},
						tps, IND_MPD_MATERIAL_GREEN, CommonWPNPlaceholder.name, {{"WPN_RKT_TotalCount_Window_Show"}}, nil, nil, nil, TRANSPARENT_BACKGROUND)
end
-----------------------------------------------------------
function Add_CODE_RANGES_Window()
	local MSL_CODE_RANGE_STATUS = {"","FAIL","N/A","?","MSL ONLY","LRFD ONLY","LST ONLY","LRFD/LST ONLY","LRFD/MSL ONLY","LST/MSL ONLY"}

	local lbl_pos = {0, 0}
	
	local tps		= createTextProperty( nil, nil, nil, "LeftCenter" )
	tps.stringdefs	= {tps.height*GetScale(),tps.height*GetScale(),tps.height*GetScale()*0.05}	-- to set symbols looser
	
	local Lbl_W, Lbl_H = tps.width*24.5, tps.height*14.0

	local col1_x = -Lbl_W/2 + tps.width*0.7
	local col2_x = Lbl_W/2 - tps.width*9.7
	
	local line1_y		= Lbl_H/2 - tps.height*0.8
	local line2_y		= line1_y - tps.height*1.4
	local underline_y	= line1_y - tps.height*0.7
	
	local dy = -tps.height*1.2

	local elem = AddRoundCornersWindow("CODE_RANGES_StatusWindow", lbl_pos, Lbl_W, Lbl_H,
						{
							{"CODE RANGES",		{col1_x,	line1_y},		tps,	nil},
							{"1111 - 1788",		{col1_x,	line2_y},		tps,	nil},
							{"2111 - 2888",		{col1_x,	line2_y+dy*1},	tps,	nil},
							{"4111 - 4288",		{col1_x,	line2_y+dy*2},	tps,	nil},
							{"4311 - 4488",		{col1_x,	line2_y+dy*3},	tps,	nil},
							{"4511 - 4688",		{col1_x,	line2_y+dy*4},	tps,	nil},
							{"4711 - 4888",		{col1_x,	line2_y+dy*5},	tps,	nil},
							{"5111 - 5288",		{col1_x,	line2_y+dy*6},	tps,	nil},
							{"5311 - 5488",		{col1_x,	line2_y+dy*7},	tps,	nil},
							{"5511 - 5688",		{col1_x,	line2_y+dy*8},	tps,	nil},
							{"5711 - 5888",		{col1_x,	line2_y+dy*9},	tps,	nil},

							{"STATUS",			{col2_x,	line1_y},		tps,	nil},
							{"FAIL",			{col2_x,	line2_y},		tps,	{{"WPN_CODE_CodeRange_Status_Value",0}}, MSL_CODE_RANGE_STATUS},
							{"FAIL",			{col2_x,	line2_y+dy*1},	tps,	{{"WPN_CODE_CodeRange_Status_Value",1}}, MSL_CODE_RANGE_STATUS},
							{"FAIL",			{col2_x,	line2_y+dy*2},	tps,	{{"WPN_CODE_CodeRange_Status_Value",2}}, MSL_CODE_RANGE_STATUS},
							{"FAIL",			{col2_x,	line2_y+dy*3},	tps,	{{"WPN_CODE_CodeRange_Status_Value",3}}, MSL_CODE_RANGE_STATUS},
							{"FAIL",			{col2_x,	line2_y+dy*4},	tps,	{{"WPN_CODE_CodeRange_Status_Value",4}}, MSL_CODE_RANGE_STATUS},
							{"FAIL",			{col2_x,	line2_y+dy*5},	tps,	{{"WPN_CODE_CodeRange_Status_Value",5}}, MSL_CODE_RANGE_STATUS},
							{"FAIL",			{col2_x,	line2_y+dy*6},	tps,	{{"WPN_CODE_CodeRange_Status_Value",6}}, MSL_CODE_RANGE_STATUS},
							{"FAIL",			{col2_x,	line2_y+dy*7},	tps,	{{"WPN_CODE_CodeRange_Status_Value",7}}, MSL_CODE_RANGE_STATUS},
							{"FAIL",			{col2_x,	line2_y+dy*8},	tps,	{{"WPN_CODE_CodeRange_Status_Value",8}}, MSL_CODE_RANGE_STATUS},
							{"FAIL",			{col2_x,	line2_y+dy*9},	tps,	{{"WPN_CODE_CodeRange_Status_Value",9}}, MSL_CODE_RANGE_STATUS},
						},
						tps, IND_MPD_MATERIAL_GREEN, CommonWPNPlaceholder.name, nil, nil, nil, nil, TRANSPARENT_BACKGROUND)

	local verts = { { col1_x, underline_y }, { col1_x + tps.width*11.7, underline_y } }
	draw_line( verts, IND_MPD_MATERIAL_GREEN, elem.name, common_line_width, "CODE_RANGES_StatusWindow_Underline1" )
	
	verts = { { col2_x, underline_y }, { col2_x + tps.width*6.4, underline_y } }
	draw_line( verts, IND_MPD_MATERIAL_GREEN, elem.name, common_line_width, "CODE_RANGES_StatusWindow_Underline2" )
end
-----------------------------------------------------------
function draw_wide_border_without_caption( pos_up, pos_dn, tp, max_item_len, dHeight, shift_up, shift_dn, is_transparent)
	local max_len = 4
	local delta_H = 0

	if tp==nil then
		tp = tp_def_left
	end
	
	local c = parseAligment ( tp.alignment )
	
	if (shift_up~=nil) then
		if c[1] == "L" or c[1] == "R" then
			pos_up[2] = pos_up[2] + shift_up
		else
			pos_up[1] = pos_up[1] + shift_up
		end
	end
	
	if (shift_dn~=nil) then
		if c[1] == "L" or c[1] == "R" then
			pos_dn[2] = pos_dn[2] + shift_dn
		else
			pos_dn[1] = pos_dn[1] + shift_dn
		end
	end

	if (max_item_len~=nil) then
		max_len = max_item_len
	end
	max_len = (max_len+1) * tp.width
	
	if (dHeight~=nil) and (c[2] == "B" or c[2] == "T") then
		delta_H = dHeight
	end
	
	local verts	= createVertsForLines( pos_up, pos_dn,  max_len, 1, "", c, tp )
	local new_verts	=	{
							verts[1][1],
							{verts[1][2][1], verts[1][2][2]+delta_H},
							{verts[2][2][1], verts[2][2][2]+delta_H},
							verts[2][1]
						}

	addTranslucentBackground( new_verts, default_box_indices, nil, nil, nil, is_transparent )
	draw_line( new_verts, tp.material )
end
-----------------------------------------------------------
-----------------------------------------------------------
function AddDBGGrid()
	local width = 1.0
	local cell = PB_Pos[pb.T2][1]-PB_Pos[pb.T1][1]+tp_default.width*0.08
	local pos = {PB_Pos[pb.T1][1]+tp_default.width*0.32, -ScreenSize/2}

	local BasePH = addPlaceholder("DebugLines_BasePH", {0,0}, CommonWPNPlaceholder.name)
	
	for i = 0, 5 do 
		addFatLine("DebugLines_v"..i.."_fon", ScreenSize, width, {pos[1]+cell*i,pos[2]}, nil, BasePH.name, nil, "MFD_BACKGROUND")
		addFatLine("DebugLines_v"..i, ScreenSize, width, {pos[1]+cell*i,pos[2]}, nil, BasePH.name, nil, IND_MPD_MATERIAL_WHITE)
	end

	pos = {-ScreenSize/2, PB_Pos[pb.L6][2]+tp_default.height*0.4}
	cell = PB_Pos[pb.L1][2]-PB_Pos[pb.L2][2]+tp_default.height*0.15

	for i = 0, 5 do
		addFatLine("DebugLines_h"..i.."_fon", ScreenSize, width, {pos[1],pos[2]+cell*i}, -90, BasePH.name, nil, "MFD_BACKGROUND")
		addFatLine("DebugLines_h"..i, ScreenSize, width, {pos[1],pos[2]+cell*i}, -90, BasePH.name, nil, IND_MPD_MATERIAL_WHITE)
	end

	-- centerlines
	addFatLine("DebugLines_vC_fon", ScreenSize, width, {0, -ScreenSize/2}, nil, BasePH.name, nil, "MFD_BACKGROUND")
	addFatLine("DebugLines_vC", ScreenSize, width, {0, -ScreenSize/2}, nil, BasePH.name, nil, IND_MPD_MATERIAL_BLUE)
	addFatLine("DebugLines_hC_fon", ScreenSize, width, {-ScreenSize/2, 0}, -90, BasePH.name, nil, "MFD_BACKGROUND")
	addFatLine("DebugLines_hC", ScreenSize, width, {-ScreenSize/2, 0}, -90, BasePH.name, nil, IND_MPD_MATERIAL_BLUE)
	

end

function AddDBGPylonAxis()
	-- pylon axiss
	local fon_w		= 70
	local fon_h		= 200
		
	for pylon = 1, 4 do
		local PylonPH = addPlaceholder("Pylon_"..pylon.."_BasePH", WPN_Pylons_Pos[pylon], CommonWPNPlaceholder.name)
	
		addFatDashedLine("Pylon_"..pylon.."_DBG_0y",	fon_h,	5,	5,	2,	{0,-fon_h/2},	nil,	PylonPH.name,	nil,	IND_MPD_MATERIAL_BLUE)
		addFatDashedLine("Pylon_"..pylon.."_DBG_0x",	fon_w,	5,	5,	2,	{fon_w/2,0},	90,		PylonPH.name,	nil,	IND_MPD_MATERIAL_BLUE)
	end
end






