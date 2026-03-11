assert(db.Formations ~= nil)

local formation = db.FormationID.HEL_WEDGE
local overwatch = false
assert(formation ~= nil)

function specialEvent(params)
	return staticParamsEvent(Message.wMsgLeaderSpecialCommand,params)
end

function refuelEvent(percent)
	return staticParamsEvent(Message.wMsgLeaderRequestRefueling,{fuel_mass = percent})
end


local FormationDirection = {
	RIGHT = 1,
	LEFT = 2
}

local formationDirection = FormationDirection.RIGHT

local FormationVariant = {
	TIGHT = 1,
	CRUISE = 2,
	COMBAT = 3
}

local availableVariant = {
	[db.FormationID.HEL_ECHELON] = {
		[FormationVariant.TIGHT] = 1,
		[FormationVariant.CRUISE] = 2,
		[FormationVariant.COMBAT] = 3
	},
	[db.FormationID.HEL_FRONT] = {
		[FormationVariant.CRUISE] = 1,
		[FormationVariant.COMBAT] = 2
	}
}

local formationVariant = {
	[db.FormationID.HEL_ECHELON] = FormationVariant.CRUISE,
	[db.FormationID.HEL_FRONT] = FormationVariant.CRUISE
}

local menus = data.menus

menus['Engage'] = {
	name = _('Engage'),
	items = {
		{name = _('My Target'),							command = sendMessage.new(Message.wMsgLeaderEngageMyTarget)},
		{name = _('My Enemy'),							command = sendMessage.new(Message.wMsgLeaderMyEnemyAttack)},
		{name = _('Bandits'),							command = sendMessage.new(Message.wMsgLeaderEngageBandits, false, false)},
		{name = _('Air Defences'),						command = sendMessage.new(Message.wMsgLeaderEngageAirDefenses, false, false)},
		{name = _('Ground Targets'),					command = sendMessage.new(Message.wMsgLeaderEngageGroundTargets, false, false)},
		{name = _('Mission and Rejoin'),				command = sendMessage.new(Message.wMsgLeaderFulfilTheTaskAndJoinUp)},
		{name = _('Mission and RTB'),					command = sendMessage.new(Message.wMsgLeaderFulfilTheTaskAndReturnToBase)},
	}
}
menus['Go Pincher'] = {
	name = _('Go Pincher'),
	items = {
		{ name = _('Right'),							command = sendMessage.new(Message.wMsgLeaderPincerRight) },
		{ name = _('Left'),								command = sendMessage.new(Message.wMsgLeaderPincerLeft) }
	}
}
menus['Go To'] = {
	name = _('Go To'),
	items = {
		{ name = _('Return to Base'),					command = sendMessage.new(Message.wMsgLeaderReturnToBase) },
		{ name = _('Route'),							command = sendMessage.new(Message.wMsgLeaderFlyRoute) },
	}
}
menus['Reconnaissance'] = {
	name = _('Reconnaissance'),
	items = {
		{ name = _('1 km'),								command = sendMessage.new(Message.wMsgLeaderMakeRecon, 1000)},
		{ name = _('2 km'),								command = sendMessage.new(Message.wMsgLeaderMakeRecon, 2000)},
		{ name = _('3 km'),								command = sendMessage.new(Message.wMsgLeaderMakeRecon, 3000)},
		{ name = _('5 km'),								command = sendMessage.new(Message.wMsgLeaderMakeRecon, 5000)},
		{ name = _('8 km'),								command = sendMessage.new(Message.wMsgLeaderMakeRecon, 8000)},
		{ name = _('10 km'),							command = sendMessage.new(Message.wMsgLeaderMakeRecon, 10000)},
	}
}

local function makeFormationMenuItem(name, msg, getParam, value, setValue, getValue, checkContition)
	return	{
				name = name,
				command = {
					sendMessage = sendMessage.new3(	msg,
													getParam
														and
													function()
														return getParam(value)
													end),
					perform = function(self, ...)
						self.sendMessage:perform(...)
						setValue(value)
					end
				},
				condition = {
					check = function(self)
						return (checkContition == nil or checkContition(value)) and getValue() ~= value
					end
				}
			}
end

local function makeFormationTypeMenuItem(name, msg, newFormation)
	return makeFormationMenuItem(	name,
									msg,
									function(newFormation)
										return	availableVariant[newFormation] ~= nil and
												formationVariant[newFormation] ~= nil and
												availableVariant[newFormation][formationVariant[newFormation]]
									end,
									newFormation,
									function(newFormation)
										formation = newFormation
									end,
									function()
										return formation
									end)
end

local function makeFormationDirectionMenuItem(name, msg, newFormationDirection)
	return makeFormationMenuItem(	name,
									msg,
									nil,
									newFormationDirection,
									function(newFormationDirection)
										formationDirection = newFormationDirection
									end,
									function()
										return formationDirection
									end,
									function()
										return db.Formations['helicopter'].list[formation].zInverse == true
									end)
end

local function makeFormationVariantMenuItem(name, msg, newFormationVariant)
	return makeFormationMenuItem(	name,
									msg,
									function(newFormationVariant)
										return	availableVariant[formation] ~= nil and
												availableVariant[formation][newFormationVariant]
									end,
									newFormationVariant,
									function(newFormationVariant)
										assert(formationVariant[formation] ~= nil)
										formationVariant[formation] = newFormationVariant
									end,
									function(newFormationVariant)
										return formationVariant[formation]
									end,
									function(newFormationVariant)
										return	availableVariant[formation] ~= nil and
												availableVariant[formation][newFormationVariant] ~= nil
									end)
end

menus['Go Formation'] = {
	name = _('Go Formation'),
	items = {
		makeFormationTypeMenuItem(_('Heavy'),		Message.wMsgLeaderHelGoHeavy,			db.FormationID.HEL_WEDGE),
		makeFormationTypeMenuItem(_('Echelon'),		Message.wMsgLeaderHelGoEchelon,			db.FormationID.HEL_ECHELON),
		makeFormationTypeMenuItem(_('Spread'),		Message.wMsgLeaderHelGoSpread,			db.FormationID.HEL_FRONT),
		makeFormationTypeMenuItem(_('Trail'),		Message.wMsgLeaderHelGoTrail,			db.FormationID.HEL_COLUMN),
		{
			name = _('Overwatch'),
			command = {
				sendMessage = sendMessage.new(Message.wMsgLeaderHelGoOverwatch),
				perform = function(self, ...)
					self.sendMessage:perform(...)
					overwatch = not overwatch
				end
			}
		},
		makeFormationDirectionMenuItem(_('Left'),	Message.wMsgLeaderHelGoLeft,			FormationDirection.LEFT),
		makeFormationDirectionMenuItem(_('Right'),	Message.wMsgLeaderHelGoRight,			FormationDirection.RIGHT),

		makeFormationVariantMenuItem(_('Tight'),	Message.wMsgLeaderHelGoTight,			FormationVariant.TIGHT),
		makeFormationVariantMenuItem(_('Cruise'),	Message.wMsgLeaderHelGoCruise,			FormationVariant.CRUISE),
		makeFormationVariantMenuItem(_('Combat'),	Message.wMsgLeaderHelGoCombat,			FormationVariant.COMBAT),
	}
}

menus['Flight'] = {
	name = _('Flight'),
	items = {
		[1] = {
			name = _('Engage'),
			submenu = menus['Engage']
		},
		[2] = {
			name = _('Go Pincher'),
			submenu = menus['Go Pincher'],
		},
		[3] = {
			name = _('Go To'),
			submenu = menus['Go To'],
		},
		[4] = {
			name = _('Cover Me'),
			command = sendMessage.new(Message.wMsgLeaderCoverMe)
		},
		[5] = {
			name = _('Go Formation'),
			submenu = menus['Go Formation']
		},
		[6] = {
			name = _('Hold Position'),
			submenu = menus['Hold Position'],
			command = sendMessage.new(Message.wMsgLeaderAnchorHere)
		},
		[7] = {
			name = _('Rejoin Formation'),
			command = sendMessage.new(Message.wMsgLeaderJoinUp)
		},
		[10] = {
			name = _('Jettison Weapons'),
			command = sendMessage.new(Message.wMsgLeaderJettisonWeapons)
		},
	}
}

local wingmenMenuItems = {
	[1] = {
		name = _('Engage'),
		submenu = menus['Engage']
	},
	[2] = {
		name = _('Go Pincher'),
		submenu = menus['Go Pincher'],
	},
	[3] = {
		name = _('Go To'),
		submenu = menus['Go To'],
	},
	[4] = {
		name = _('Cover Me'),
		command = sendMessage.new(Message.wMsgLeaderCoverMe)
	},
	[5] = {
		name = _('Reconnaissance'),
		submenu = menus['Reconnaissance'],
	},
	[6] = {
		name = _('Hold Position'),
		submenu = menus['Hold Position'],
		command = sendMessage.new(Message.wMsgLeaderAnchorHere)
	},
	[7] = {
		name = _('Rejoin Formation'),
		command = sendMessage.new(Message.wMsgLeaderJoinUp)
	},
	[10] = {
		name = _('Jettison Weapons'),
		command = sendMessage.new(Message.wMsgLeaderJettisonWeapons)
	},
}

local function makeWingmenMenu(number)
	return {
		name = _('Wingman ')..(number + 1),
		submenu = {
			name = _('Wingman ')..(number + 1),
			items = wingmenMenuItems
		},
		condition = {
			check = function(self)
				if not data.showingOnlyPresentRecepients then
					return true
				end
				local pWingmen = data.pUnit:getGroup():getUnit(number + 1)
				return pWingmen ~= nil
			end
		},
		color = {
			get = function(self)
				local pWingmen = data.pUnit:getGroup():getUnit(number + 1)
				return getRecepientColor(pWingmen and pWingmen:getCommunicator())
			end
		},
		command = {
			perform = function(self)
				local pWingmen = data.pUnit:getGroup():getUnit(number + 1)
				selectAndTuneCommunicator(pWingmen and pWingmen:getCommunicator())
			end
		},
		parameter = number,
	}
end

local function hasFlight()
	local pGroup = data.pUnit:getGroup()
	for i = 2, 4 do
		local pWingmen = pGroup:getUnit(i)
		if pWingmen ~= nil then
			return true
		end
	end
	return false
end

data.rootItem = {
	name = _('Main'),
	getSubmenu = function(self)
		local tbl = {
			name = _('Main'),
			items = {}
		}

		if data.pUnit == nil or data.pUnit:isExist() == false then
			return tbl
		end

		if	not data.showingOnlyPresentRecepients or
			hasFlight() then
			tbl.items[1] = {
				name = _('Flight'),
				submenu = menus['Flight'],
				color = {
					get = function(self)
						local pGroup = data.pUnit:getGroup()
						for i = 2, 4 do
							local pWingmen = pGroup:getUnit(i)
							if pWingmen ~= nil then
								return getRecepientColor(pWingmen and pWingmen:getCommunicator())
							end
						end
						return getRecepientColor(nil)
					end
				},
				command = {
					perform = function(self)
						local pGroup = data.pUnit:getGroup()
						for i = 2, 4 do
							local pWingmen = pGroup:getUnit(i)
							if pWingmen ~= nil then
								selectAndTuneCommunicator(pWingmen and pWingmen:getCommunicator())
								return
							end
						end
					end
				},
				parameter = 5,
			}
		end

		for	number = 2, 4 do
			if	not data.showingOnlyPresentRecepients or
				data.pUnit:getGroup():getUnit(number + 1) ~= nil then
				tbl.items[number] = makeWingmenMenu(number - 1)
			end
		end

		if self.builders ~= nil then
			for index, builder in pairs(self.builders) do
				builder(self, tbl)
			end
		end

		if #data.menuOther.submenu.items > 0 then
			tbl.items[10] = {
				name = _('Other'),
				submenu = data.menuOther.submenu
			}
		end

		return tbl
	end,
	builders = {}
}

utils.verifyChunk(utils.loadfileIn('Scripts/UI/RadioCommandDialogPanel/Config/Common/JTAC.lua', getfenv()))(4)
utils.verifyChunk(utils.loadfileIn('Scripts/UI/RadioCommandDialogPanel/Config/Common/ATC.lua', getfenv()))(5, {[Airbase.Category.AIRDROME] = true, [Airbase.Category.HELIPAD] = true} )
utils.verifyChunk(utils.loadfileIn('Scripts/UI/RadioCommandDialogPanel/Config/Common/AWACS.lua', getfenv()))(7, {tanker = false, radar = false})
utils.verifyChunk(utils.loadfileIn('Scripts/UI/RadioCommandDialogPanel/Config/Common/Ground Crew.lua', getfenv()))(8)

local g_crew_items = menus['Ground Crew'].items;

-- Wheel Chocks
menus['Wheel chocks'] = {
	name = _('Wheel chocks'),
	items = {
		[1] = {name = _('Place_'),		command = sendMessage.new(Message.wMsgLeaderGroundToggleWheelChocks, true)},
		[2] = {name = _('Remove_'),		command = sendMessage.new(Message.wMsgLeaderGroundToggleWheelChocks, false)}
	}
}
g_crew_items[#g_crew_items + 1] = { name = _('Wheel chocks'), submenu = menus['Wheel chocks']}

-- Ground Air Supply
menus['Ground Air Supply'] = {
	name = _('Ground Air Supply'),
	items = {
		[1] = {name = _('Connect'),		command = sendMessage.new(Message.wMsgLeaderGroundToggleAir, true)},
		[2] = {name = _('Disconnect'),	command = sendMessage.new(Message.wMsgLeaderGroundToggleAir, false)},
		[3] = {name = _('Apply'),		command = sendMessage.new(Message.wMsgLeaderGroundApplyAir, true)}
	}
}
g_crew_items[#g_crew_items + 1] = { name = _('Ground Air Supply'), submenu = menus['Ground Air Supply']}



