local mainMenuPos, parameters = ...

--Menu
local menus = data.menus

menus['ATC'] = {
	name = _('ATC'),
	items = {
		{	name = _('Inbound'),
			command = sendMessage.new(Message.wMsgLeaderInbound),
			condition = {
				check = function(self)
					return data.pUnit:inAir()
				end
			}
		},
		{	name = _('I\'m lost'),
			command = sendMessage.new(Message.wMsgLeaderRequestAzimuth),
			condition = {
				check = function(self)
					return data.pUnit:inAir()
				end
			}
		},
		{	name = _('Request startup'),
			command = sendMessage.new(Message.wMsgLeaderRequestEnginesLaunch),
			condition = {
				check = function(self)
					return not data.pUnit:inAir() and (data.pUnit:OldCarrierMenuShow() or not data.pUnit:hasCarrier())
				end
			}
		}
	}
}
menus['ATC_SC'] = {
	name = _('ATC'),
	items = {
		{	name = _('Inbound'),
			command = sendMessage.new(Message.wMsgLeaderInboundCarrier),
			condition = {
				check = function(self)
					return data.pUnit:inAir()
				end
			}
		},
		{	name = _('I\'m lost'),
			command = sendMessage.new(Message.wMsgLeaderRequestAzimuth),
			condition = {
				check = function(self)
					return data.pUnit:inAir()
				end
			}
		},
		{	name = _('Request startup'),
			command = sendMessage.new(Message.wMsgLeaderRequestEnginesLaunch),
			condition = {
				check = function(self)
					return not data.pUnit:inAir() and (data.pUnit:OldCarrierMenuShow() or not data.pUnit:hasCarrier())
				end
			}
		}
	}
}

local function getATCs()		
	local atcs = {}
	local neutralAirbases = coalition.getServiceProviders(coalition.side.NEUTRAL, coalition.service.ATC)
	for i, airbase in pairs(neutralAirbases) do	
		table.insert(atcs, airbase)
	end	
	
	local selfCoalition = data.pUnit:getCoalition()
	local ourAirbases = coalition.getServiceProviders(selfCoalition, coalition.service.ATC)
	for i, airbase in pairs(ourAirbases) do
		--[[print('!!!!!!! getATCs() ourAirbases i,getCategory(), getTypeName():',i,airbase:getCategory(), airbase:getTypeName())
		if airbase:getDesc()['attributes'] ~= nil then
			for k,v in pairs(airbase:getDesc()['attributes']) do
				print('!!!!!!!!!! getATCs() ourAirbases airbase:getDesc()[attributes], k,v ', k,v)
			end
		end--]]
		table.insert(atcs, airbase)
	end
	
	--[[
	local enemyCoalition = selfCoalition == coalition.RED and coalition.BLUE or coalition.RED
	local ourAirbases = coalition.getServiceProviders(enemycoalition. coalition.service.ATC)
	--print("ourAirbases size = "..table.getn(ourAirbases))
	for i, airbase in pairs(ourAirbases) do
		if filterAirbaseType(atc) then
			table.insert(atcs, airbase)
		end
	end
	--]]
	local VTOLableLA = data.pUnit:vtolableLA()
	--print('!!~~~~!!!!!!~~~~!! getATCs() data.pUnit VTOLableLA', VTOLableLA)
	
	local selfPoint = data.pUnit:getPosition().p	
	local function distanceSorter(lu, ru)
		local lpoint = lu:getPoint()
		local ldist2 = (lpoint.x - selfPoint.x) * (lpoint.x - selfPoint.x) + (lpoint.z - selfPoint.z) * (lpoint.z - selfPoint.z)
		
		local rpoint = ru:getPoint()
		local rdist2 = (rpoint.x - selfPoint.x) * (rpoint.x - selfPoint.x) + (rpoint.z - selfPoint.z) * (rpoint.z - selfPoint.z)
		
		if not VTOLableLA then
			if not lu:getDesc()['attributes'] or not (lu:getDesc()['attributes']['Arresting Gear'] or lu:getDesc()['attributes']['Airfields'] ) then
				ldist2 = ldist2 + 10000000000
			end
			if not ru:getDesc()['attributes'] or not (ru:getDesc()['attributes']['Arresting Gear'] or ru:getDesc()['attributes']['Airfields'] ) then
				rdist2 = rdist2 + 10000000000
			end
		end
		
		return ldist2 < rdist2
	end
		
	table.sort(atcs, distanceSorter)
	
	return atcs
end

local function buildATCs(self, menu)
	local ATCs = getATCs()
	if 	not data.showingOnlyPresentRecepients or
		getRecepientsState(ATCs) ~= RecepientState.VOID then
		menu.items[mainMenuPos] = buildRecepientsMenuATC(ATCs, _('ATC'), { name = _('ATC'), submenu = menus['ATC'] },{ name = _('ATC_SC'), submenu = menus['ATC_SC'] })
	end
end

table.insert(data.rootItem.builders, buildATCs)

--Dialogs

--Departure Airdrome

dialogsData.dialogs['Departure Airdrome'] = {
	name = _('Departure Airdrome'),
	menus = {
		['Ready to taxi to runway'] = {
			name = _('Ready to taxi to runway'),
			items = {
				{ name = _('Request to taxi to runway'),	command = sendMessage.new(Message.wMsgLeaderRequestTaxiToRunway) },
				{ name = _('Request control hover'),
					command = sendMessage.new(Message.wMsgLeaderRequestControlHover),
					condition = {
						check = function(self)
							return data.pUnit:hasAttribute('Helicopters')
						end
					}
				},
				{ name = _('Abort takeoff'), 				command = sendMessage.new(Message.wMsgLeaderAbortTakeoff) }
			}
		},
		['Hover Check'] = {
			name = _('Hover Check'),
			items = {
				{ name = _('Request to taxi to runway'),	command = sendMessage.new(Message.wMsgLeaderRequestTaxiToRunway) },
				{ name = _('Abort takeoff'), 				command = sendMessage.new(Message.wMsgLeaderAbortTakeoff) }
			}
		},		
		['Ready to takeoff'] = {
			name = _('Ready to takeoff'),
			items = {
				{ name = _('Request takeoff'), 				command = sendMessage.new(Message.wMsgLeaderRequestTakeOff) },
				{ name = _('Abort takeoff'), 				command = sendMessage.new(Message.wMsgLeaderAbortTakeoff) }
			}
		},
		['Taking Off'] = {
			name = _('Taking Off'),
			items = {
				{ name = _('Abort takeoff'), 				command = sendMessage.new(Message.wMsgLeaderAbortTakeoff) }
			}
		}
	}
}
dialogsData.dialogs['Departure Airdrome'].stages = {
	['Closed'] = {	
		[events.NOTIFY_BIRTH_ON_RAMP_HOT]					= { menu = dialogsData.dialogs['Departure Airdrome'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' },
		[events.DENY_TAKEOFF_FROM_AIRDROME] 				= { menu = dialogsData.dialogs['Departure Airdrome'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' },
		[events.NOTIFY_BIRTH_ON_RUNWAY]						= { menu = dialogsData.dialogs['Departure Airdrome'].menus['Taking Off'], newStage = 'Taking Off' },
		[events.CLEAR_TO_TAKEOFF_FROM_AIRDROME]				= { menu = dialogsData.dialogs['Departure Airdrome'].menus['Taking Off'], newStage = 'Taking Off' },
		[events.STARTUP_PERMISSION_FROM_AIRDROME] 			= { menu = dialogsData.dialogs['Departure Airdrome'].menus['Ready to taxi to runway'], newStage = 'Ready to taxi to runway' },
		[Message.wMsgATCClearedToTaxiRunWay] 				= { menu = dialogsData.dialogs['Departure Airdrome'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' },
		[events.TAKEOFF] 									= TERMINATE(),
		[events.LANDING]									= { menu = dialogsData.dialogs['Departure Airdrome'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' }
	},
	['Hover Check'] = {
		[Message.wMsgATCClearedToTaxiRunWay] 			= { menu = dialogsData.dialogs['Departure Airdrome'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' },
		[Message.wMsgATCYouHadTakenOffWithNoPermission]	= TERMINATE(),
		[Message.wMsgATCTaxiToParkingArea]				= TERMINATE(),
		[events.ENGINE_SHUTDOWN]						= TERMINATE()
	},	
	['Ready to taxi to runway'] = {
		[Message.wMsgATCClearedToTaxiRunWay] 			= { menu = dialogsData.dialogs['Departure Airdrome'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' },
		[Message.wMsgATCClearedControlHover]			= { menu = dialogsData.dialogs['Departure Airdrome'].menus['Hover Check'], newStage = 'Hover Check' },
		[events.TAKEOFF] 								= TERMINATE(),
		[Message.wMsgATCTaxiToParkingArea]				= TERMINATE(),
		[events.ENGINE_SHUTDOWN]						= TERMINATE()
	},
	['Ready to takeoff'] = {
		[Message.wMsgATCYouAreClearedForTO]				= { menu = dialogsData.dialogs['Departure Airdrome'].menus['Taking Off'], newStage = 'Taking Off' },
		[events.TAKEOFF] 								= TERMINATE(),
		[Message.wMsgATCTaxiToParkingArea]				= TERMINATE(),
		[events.ENGINE_SHUTDOWN]						= TERMINATE()
	},
	['Taking Off'] = {
		[Message.wMsgATCClearedToTaxiRunWay] 			= { menu = dialogsData.dialogs['Departure Airdrome'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' },
		[events.TAKEOFF] 								= TERMINATE(),
		[Message.wMsgATCTaxiToParkingArea]				= TERMINATE(),
		[events.ENGINE_SHUTDOWN]						= TERMINATE()
	}
}

--Departure Helipad

dialogsData.dialogs['Departure Helipad'] = {
	name = _('Departure Helipad'),
	menus = {
		['Hover Check'] = {
			name = _('Hover Check'),
			items = {
				{ name = _('Request takeoff'), 				command = sendMessage.new(Message.wMsgLeaderRequestTakeOff) },
				{ name = _('Abort takeoff'), 				command = sendMessage.new(Message.wMsgLeaderAbortTakeoff) }
			}
		},
		['Ready to takeoff'] = {
			name = _('Ready to takeoff'),
			items = {
				{ name = _('Request takeoff'), 				command = sendMessage.new(Message.wMsgLeaderRequestTakeOff) },
				{ name = _('Request control hover'),
						command = sendMessage.new(Message.wMsgLeaderRequestControlHover),
						condition = {
							check = function(self)
								return data.pUnit:hasAttribute('Helicopters')
							end
						}
				},
				{ name = _('Abort takeoff'), 				command = sendMessage.new(Message.wMsgLeaderAbortTakeoff) }
			}
		},
		['Taking Off'] = {
			name = _('Taking Off'),
			items = {
				{ name = _('Abort takeoff'), 				command = sendMessage.new(Message.wMsgLeaderAbortTakeoff) }
			}
		}		
	}
}
dialogsData.dialogs['Departure Helipad'].stages = {
	['Closed'] = {
		[events.DENY_TAKEOFF_FROM_HELIPAD]					= { menu = dialogsData.dialogs['Departure Helipad'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' },
		[events.NOTIFY_BIRTH_ON_HELIPAD_HOT]				= { menu = dialogsData.dialogs['Departure Helipad'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' },
		[events.STARTUP_PERMISSION_FROM_HELIPAD] 			= { menu = dialogsData.dialogs['Departure Helipad'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' },
		[events.CLEAR_TO_TAKEOFF_FROM_HELIPAD]				= { menu = dialogsData.dialogs['Departure Helipad'].menus['Taking Off'], newStage = 'Taking Off' },
		[events.TAKEOFF] 									= TERMINATE(),
		[events.LANDING]									= { menu = dialogsData.dialogs['Departure Helipad'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' }
	},
	['Hover Check'] = {
		[Message.wMsgATCYouAreClearedForTO]					= { menu = dialogsData.dialogs['Departure Helipad'].menus['Taking Off'], newStage = 'Taking Off' },
		[Message.wMsgATCTaxiToParkingArea]					= TERMINATE(),
		[events.ENGINE_SHUTDOWN]							= TERMINATE(),
		[events.TAKEOFF] 									= TERMINATE(),
	},
	['Ready to takeoff'] = {		
		[Message.wMsgATCYouAreClearedForTO]					= { menu = dialogsData.dialogs['Departure Helipad'].menus['Taking Off'], newStage = 'Taking Off' },
		[Message.wMsgATCClearedControlHover]				= { menu = dialogsData.dialogs['Departure Helipad'].menus['Hover Check'], newStage = 'Hover Check' },
		[events.TAKEOFF] 									= TERMINATE(),
		[Message.wMsgATCTaxiToParkingArea]					= TERMINATE(),
		[events.ENGINE_SHUTDOWN]							= TERMINATE(),
		[events.CLEAR_TO_TAKEOFF_FROM_HELIPAD]				= { menu = dialogsData.dialogs['Departure Helipad'].menus['Taking Off'], newStage = 'Taking Off' },
	},
	['Taking Off'] = {
		[events.TAKEOFF] 									= TERMINATE(),
		[Message.wMsgATCTaxiToParkingArea]					= TERMINATE(),
		[events.ENGINE_SHUTDOWN]							= TERMINATE()
	}
}


--Departure Ship

dialogsData.dialogs['Departure Ship'] = {
	name = _('Departure Ship'),
	menus = {
		['Hover Check'] = {
			name = _('Hover Check'),
			items = {
				{ name = _('Request to taxi to runway'),	command = sendMessage.new(Message.wMsgLeaderRequestTaxiToRunway) },
				{ name = _('Abort takeoff'), 				command = sendMessage.new(Message.wMsgLeaderAbortTakeoff) }
			}
		},		
		['Ready to takeoff'] = {
			name = _('Ready to takeoff'),
			items = {
				{ name = _('Request takeoff'), 				command = sendMessage.new(Message.wMsgLeaderRequestTakeOff),
					condition = {
							check = function(self)
								return data.pUnit:OldCarrierMenuShow()
							end
						}},
				{ name = _('Abort takeoff'), 				command = sendMessage.new(Message.wMsgLeaderAbortTakeoff) ,
					condition = {
							check = function(self)
								return data.pUnit:OldCarrierMenuShow()
							end
						}},
				{ name = _('Request control hover'),
					command = sendMessage.new(Message.wMsgLeaderRequestControlHover),
					condition = {
						check = function(self)
							return data.pUnit:hasAttribute('Helicopters')
						end
					}
				}
			}
		},
		['Taking Off'] = {
			name = _('Taking Off'),
			items = {
				{ name = _('Abort takeoff'), 				command = sendMessage.new(Message.wMsgLeaderAbortTakeoff) ,
					condition = {
							check = function(self)
								return data.pUnit:OldCarrierMenuShow()
							end
						}}
			}
		}
	}
}
dialogsData.dialogs['Departure Ship'].stages = {
	['Closed'] = {	
		[events.NOTIFY_BIRTH_ON_SHIP_HOT]					= { menu = dialogsData.dialogs['Departure Ship'].menus['Taking Off'], newStage = 'Taking Off' },
		[events.DENY_TAKEOFF_FROM_SHIP] 					= { menu = dialogsData.dialogs['Departure Ship'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' },
		[events.CLEAR_TO_TAKEOFF_FROM_SHIP]					= { menu = dialogsData.dialogs['Departure Ship'].menus['Taking Off'], newStage = 'Taking Off' },
		[events.STARTUP_PERMISSION_FROM_SHIP] 				= { menu = dialogsData.dialogs['Departure Ship'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' },
		[events.TAKEOFF] 									= TERMINATE(),
		[events.LANDING]									= { menu = dialogsData.dialogs['Departure Ship'].menus['Ready to takeoff'], newStage = 'Ready to takeoff' }
	},
	['Hover Check'] = {
		[Message.wMsgATCYouHadTakenOffWithNoPermission]		= TERMINATE(),
		[Message.wMsgATCTaxiToParkingArea]					= TERMINATE(),
		[events.ENGINE_SHUTDOWN]							= TERMINATE()
	},
	['Ready to takeoff'] = {
		[Message.wMsgATCClearedControlHover]				= { menu = dialogsData.dialogs['Departure Ship'].menus['Hover Check'], newStage = 'Hover Check' },
		[Message.wMsgATCYouAreClearedForTO]					= { menu = dialogsData.dialogs['Departure Ship'].menus['Taking Off'], newStage = 'Taking Off' },
		[events.TAKEOFF] 									= TERMINATE(),
		[Message.wMsgATCTaxiToParkingArea]					= TERMINATE(),
		[events.ENGINE_SHUTDOWN]							= TERMINATE()
	},
	['Taking Off'] = {
		[events.TAKEOFF] 									= TERMINATE(),
		[Message.wMsgATCTaxiToParkingArea]					= TERMINATE(),
		[events.ENGINE_SHUTDOWN]							= TERMINATE()
	}
}

--Arrival

dialogsData.dialogs['Arrival'] = {}
dialogsData.dialogs['Arrival'].name = _('Arrival')
dialogsData.dialogs['Arrival'].menus = {}
dialogsData.dialogs['Arrival'].menus['Inbound'] = {
	name = _('Inbound'),
	items = {
		{ name = _('Abort Inbound'), 						command = sendMessage.new(Message.wMsgLeaderAbortInbound) },
		{ name = _('I\'m lost'), 							command = sendMessage.new(Message.wMsgLeaderRequestAzimuth)},		
	}
}
dialogsData.dialogs['Arrival'].menus['Inbound carrier'] = {
	name = _('Inbound carrier'),
	items = {
		{ name = _('Abort Inbound'), 						command = sendMessage.new(Message.wMsgLeaderAbortInbound) },
		{ name = _('See you at 10'), 						command = sendMessage.new(Message.wMsgLeaderSeeYouAtTen) },
		{ name = _('I\'m lost'), 							command = sendMessage.new(Message.wMsgLeaderRequestAzimuth)},		
	}
}

dialogsData.dialogs['Arrival'].menus['Inbound carrier CASE 2 and 3'] = {
	name = _('Inbound carrier CASE 2 and 3'),
	items = {
		{ name = _('Abort Inbound'), 						command = sendMessage.new(Message.wMsgLeaderAbortInbound) },
		{ name = _('ESTABLISHED'), 							command = sendMessage.new(Message.wMsgLeaderEstablished) },
		{ name = _('COMMENCING'), 							command = sendMessage.new(Message.wMsgLeaderCommencing) },
		{ name = _('I\'m lost'), 							command = sendMessage.new(Message.wMsgLeaderRequestAzimuth)},		
	}
}

dialogsData.dialogs['Arrival'].menus['Orbit'] = {
	name = _('Orbit'),
	items = {
		{ name = _('Abort Inbound'), 						command = sendMessage.new(Message.wMsgLeaderAbortInbound) },
		{ name = _('I\'m lost'), 							command = sendMessage.new(Message.wMsgLeaderRequestAzimuth)}
	}
}
dialogsData.dialogs['Arrival'].menus['Ready to land'] = {
	name = _('Ready to land'),
	items = {
		{ name = _('Request Landing'), 						command = sendMessage.new(Message.wMsgLeaderRequestLanding) },
		{ name = _('Abort Inbound'), 						command = sendMessage.new(Message.wMsgLeaderAbortInbound) },
		{ name = _('I\'m lost'), 							command = sendMessage.new(Message.wMsgLeaderRequestAzimuth)}
	}
}
dialogsData.dialogs['Arrival'].menus['Ready to land carrier'] = {
	name = _('Ready to land carrier'),
	items = {
		--{ name = _('See you at 10.'), 						command = sendMessage.new(Message.wMsgLeaderSeeYouAtTen) },-- removed due to https://jira.eagle.ru/browse/SUPCAR-473
		{ name = _('Abort Inbound'), 						command = sendMessage.new(Message.wMsgLeaderAbortInbound) },
		{ name = _('I\'m lost'), 							command = sendMessage.new(Message.wMsgLeaderRequestAzimuth)}
	}
}

dialogsData.dialogs['Arrival'].menus['Carrier approach CASE 2 and 3'] = {
	name = _('Carrier approach CASE 2 and 3'),
	items = {
		{ name = _('Abort Inbound'), 						command = sendMessage.new(Message.wMsgLeaderAbortInbound) },
		{ name = _('CHECK IN'), 							command = sendMessage.new(Message.wMsgLeaderCheckingIn) },
		{ name = _('Platform'), 							command = sendMessage.new(Message.wMsgLeaderPlatform) },
		{ name = _('I\'m lost'), 							command = sendMessage.new(Message.wMsgLeaderRequestAzimuth)},		
	}
}

dialogsData.dialogs['Arrival'].menus['Landing'] = {
	name = _('Landing'),
	items = {
		{ name = _('Abort Inbound'), 						command = sendMessage.new(Message.wMsgLeaderAbortInbound) }
	}
}


local aircraftNickNames = 
				{
					['FA-18C_hornet']	  	= _('Hornet ball'),	-- hornet
					['F/A-18C']	  			= _('Hornet ball'),	-- hornet
					['F-14A']	  			= _('Tomcat ball'),	-- tomcat
					['F-14B']	  			= _('Tomcat ball'),	-- tomcat
					['F-14A-135-GR']		= _('Tomcat ball'),	-- tomcat
					['F-14A-95-GR']			= _('Tomcat ball'),	-- tomcat
					['E-2C']	  			= _('Hawkeye ball'),	-- HAWKEYE
					['E-2D']	  			= _('Hawkeye ball'),	-- HAWKEYE
					['S-3B Tanker']	  		= _('Viking ball'),	-- VIKING
					['S-3B']	  			= _('Viking ball'),	-- VIKING
					['F-4E']	  			= _('Phantom ball'),	-- PHANTOM
					['F-4E_new']	  		= _('Phantom ball'),	-- PHANTOM
					['C-2']			  		= _('Greyhound ball'),	-- Greyhound
					['A-6']			  		= _('Intruder ball'),	-- Intruder
					['F-35']		  		= _('Lightning ball'),	-- lightning
					['EA-6B']		  		= _('Prowler ball'),	-- Prowler
					['A-4']		  			= _('Skyhawk ball'),	-- Skyhawk
				}
local call_ball_name = aircraftNickNames[data.pUnit:getTypeName()] or _('Ball')

dialogsData.dialogs['Arrival'].menus['Landing carrier CASE I Ball'] = {
	name = _('Landing carrier CASE I Ball'),
	items = {
		{ name = _('Abort Inbound'), 						command = sendMessage.new(Message.wMsgLeaderAbortInbound) },
		{ name = call_ball_name, 							command = sendMessage.new(Message.wMsgLeaderHornetBall)},
		{ name = _('Flight Kiss Off'), 						command = sendMessage.new(Message.wMsgLeaderFlightKissOff)},
	}
}

dialogsData.dialogs['Arrival'].menus['Landing carrier CASE I Clara'] = {
	name = _('Landing carrier CASE I Clara'),
	items = {
		{ name = _('Abort Inbound'), 						command = sendMessage.new(Message.wMsgLeaderAbortInbound) },
		{ name = _('Clara'), 								command = sendMessage.new(Message.wMsgLeaderCLARA)},
		{ name = _('Flight Kiss Off'), 						command = sendMessage.new(Message.wMsgLeaderFlightKissOff)},
	}
}

dialogsData.dialogs['Arrival'].menus['Parking'] = {
	name = _('Parking'),
	items = {
		{ name = _('Request takeoff'), 						command = sendMessage.new(Message.wMsgLeaderRequestTakeOff) ,
					condition = {
							check = function(self)
								return data.pUnit:OldCarrierMenuShow()
							end
						}},
		{ name = _('Abort takeoff'), 						command = sendMessage.new(Message.wMsgLeaderAbortTakeoff) ,
					condition = {
							check = function(self)
								return data.pUnit:OldCarrierMenuShow()
							end
						}}
	}
}
-- Stages
dialogsData.dialogs['Arrival'].stages = {

	['Closed'] = {
		[Message.wMsgATCFlyHeading] 					= TO_STAGE('Arrival', 'Inbound'),
		[Message.wMsgATCMarshallCopyInbound]			= TO_STAGE('Arrival', 'Inbound carrier'),
		[Message.wMsgATCMarshallCopyInbound2and3]		= TO_STAGE('Arrival', 'Inbound carrier CASE 2 and 3'),
		[Message.wMsgATCGoAround]						= TO_STAGE('Arrival', 'Ready to land'),
		[events.LANDING]								= TO_STAGE('Arrival', 'Parking'),
		[Message.wMsgATCTaxiToParkingArea]				= TO_STAGE('Arrival', 'Parking'),
		[Message.wMsgATCLSOOutOfGlideslopeClara]		= TO_STAGE('Arrival', 'Landing carrier CASE I Clara'),
		[Message.wMsgATCTowerCopyOverhead] 				= TERMINATE(),
		[Message.wMsgLeaderAbortInbound]				= TERMINATE(),
		[Message.wMsgATCYouAreNotAuthorised]			= TERMINATE()
	},
	['Inbound'] = {
		[Message.wMsgATCOrbitForSpacing] 				= TO_STAGE('Arrival', 'Orbit'),
		[Message.wMsgATCClearedForVisual] 				= TO_STAGE('Arrival', 'Ready to land'),		
		[events.LANDING]								= TO_STAGE('Arrival', 'Parking'),
		[Message.wMsgLeaderAbortInbound]				= TERMINATE(),
		[Message.wMsgATCYouAreNotAuthorised]			= TERMINATE()
	},
	['Inbound carrier'] = {
		[Message.wMsgATCOrbitForSpacing] 				= TO_STAGE('Arrival', 'Orbit'),
		[Message.wMsgATCMarshallCopyTen] 				= TO_STAGE('Arrival', 'Ready to land carrier'),
		[events.LANDING]								= TO_STAGE('Arrival', 'Parking'),
		[Message.wMsgLeaderAbortInbound]				= TERMINATE()		
	},
	['Inbound carrier CASE 2 and 3']	= {
		[Message.wMsgATCOrbitForSpacing] 				= TO_STAGE('Arrival', 'Orbit'),
		[Message.wMsgATCMarshallSwitchApproach] 		= TO_STAGE('Arrival', 'Carrier approach CASE 2 and 3'),
		[Message.wMsgLeaderAbortInbound]				= TERMINATE()		
	},
	['Orbit'] = {
		[Message.wMsgATCClearedForVisual] 				= TO_STAGE('Arrival', 'Ready to land'),
		[events.LANDING]								= TO_STAGE('Arrival', 'Parking'),
		[Message.wMsgLeaderAbortInbound]				= TERMINATE(),
	},
	['Ready to land'] = {
		[Message.wMsgATCYouAreClearedForLanding] 		= TO_STAGE('Arrival', 'Landing'),
		[Message.wMsgATCCheckLandingGear]				= TO_STAGE('Arrival', 'Landing'),
		[Message.wMsgATCOrbitForSpacing] 				= TO_STAGE('Arrival', 'Orbit'),
		[events.LANDING]								= TO_STAGE('Arrival', 'Parking'),
		[Message.wMsgLeaderAbortInbound]				= TERMINATE(),
		[Message.wMsgATCGoSecondary]					= TERMINATE(),
	},
	['Ready to land carrier'] = {
		[Message.wMsgATCYouAreClearedForLanding] 		= TO_STAGE('Arrival', 'Landing carrier CASE I Clara'),
		[Message.wMsgATCTowerCopyOverhead] 				= TO_STAGE('Arrival', 'Landing carrier CASE I Clara'),
		[Message.wMsgATCCheckLandingGear]				= TO_STAGE('Arrival', 'Landing carrier CASE I Clara'),
		[Message.wMsgATCOrbitForSpacing] 				= TO_STAGE('Arrival', 'Orbit'),
		[Message.wMsgLeaderSeeYouAtTen] 				= TO_STAGE('Arrival', 'Ready to land carrier'),
		[events.LANDING]								= TO_STAGE('Arrival', 'Parking'),
		[Message.wMsgLeaderAbortInbound]				= TERMINATE(),
		[Message.wMsgATCGoSecondary]					= TERMINATE(),
	},	
	['Carrier approach CASE 2 and 3'] = {
		[Message.wMsgATCTowerCallTheBall]				= TO_STAGE('Arrival', 'Landing carrier CASE I Clara'),
		[Message.wMsgATCTowerSwitchMenu]				= TO_STAGE('Arrival', 'Inbound carrier'),
		[events.LANDING]								= TO_STAGE('Arrival', 'Parking'),
		[Message.wMsgLeaderAbortInbound]				= TERMINATE(),
		[Message.wMsgATCGoSecondary]					= TERMINATE(),
	},	
	['Landing'] = {
		[Message.wMsgATCGoAround]						= TO_STAGE('Arrival', 'Ready to land'),
		[events.LANDING]								= TO_STAGE('Arrival', 'Parking'),
		[Message.wMsgLeaderAbortInbound]				= TERMINATE(),
		[Message.wMsgATCGoSecondary]					= TERMINATE(),
	},
	
	['Landing carrier CASE I Clara'] = {
		--[Message.wMsgATCGoAround]						= TO_STAGE('Arrival', 'Ready to land carrier'),
		[Message.wMsgATCLSOBolterX3]					= TO_STAGE('Arrival', 'Ready to land carrier'),
		[Message.wMsgATCLSOInGlideslopeBall]			= TO_STAGE('Arrival', 'Landing carrier CASE I Ball'),
		[events.LANDING]								= TO_STAGE('Arrival', 'Parking'),
		[Message.wMsgLeaderAbortInbound]				= TERMINATE(),
		[Message.wMsgATCGoSecondary]					= TERMINATE(),
		[Message.wMsgLeaderFlightKissOff]				= TO_STAGE('Arrival', 'Landing carrier CASE I Clara'),
	},
	
	['Landing carrier CASE I Ball'] = {
		--[Message.wMsgATCGoAround]						= TO_STAGE('Arrival', 'Ready to land carrier'),
		[Message.wMsgATCLSOBolterX3]					= TO_STAGE('Arrival', 'Ready to land carrier'),
		[Message.wMsgATCLSOOutOfGlideslopeClara]		= TO_STAGE('Arrival', 'Landing carrier CASE I Clara'),
		[events.LANDING]								= TO_STAGE('Arrival', 'Parking'),
		[Message.wMsgLeaderAbortInbound]				= TERMINATE(),
		[Message.wMsgATCGoSecondary]					= TERMINATE(),
		[Message.wMsgLeaderFlightKissOff]				= TO_STAGE('Arrival', 'Landing carrier CASE I Ball'),
	},
	
	['Parking'] = {
		[events.ENGINE_SHUTDOWN]						= TERMINATE(),
		[Message.wMsgATCTaxiDenied]						= TERMINATE(),
		[Message.wMsgATCClearedToTaxiRunWay]			= TERMINATE(),
		[Message.wMsgATCYouAreClearedForTO]			= { menu = dialogsData.dialogs['Departure Ship'].menus['Taking Off'], newStage = 'Taking Off' },
		[Message.wMsgATCTakeoffDenied]					= TERMINATE(),
		[events.TAKEOFF] 								= TERMINATE(),
		[events.CLEAR_TO_TAKEOFF_FROM_SHIP]				= TERMINATE(),
	}
}

local arrivalDialogTrigger = DialogStartTrigger:new(dialogsData.dialogs['Arrival'])
dialogsData.triggers[Message.wMsgLeaderFlightKissOff]			= switchToMainMenu
dialogsData.triggers[Message.wMsgATCFlyHeading] 				= arrivalDialogTrigger
dialogsData.triggers[Message.wMsgATCMarshallCopyInbound]		= arrivalDialogTrigger
dialogsData.triggers[Message.wMsgATCMarshallCopyInbound2and3]	= arrivalDialogTrigger
dialogsData.triggers[Message.wMsgATCMarshallSwitchApproach]		= arrivalDialogTrigger
dialogsData.triggers[Message.wMsgATCTowerCopyOverhead]			= arrivalDialogTrigger
dialogsData.triggers[Message.wMsgATCGoAround] 					= arrivalDialogTrigger
dialogsData.triggers[Message.wMsgATCTaxiToParkingArea]			= arrivalDialogTrigger
dialogsData.triggers[Message.wMsgATCLSOInGlideslopeBall]		= arrivalDialogTrigger
dialogsData.triggers[Message.wMsgATCLSOOutOfGlideslopeClara]	= arrivalDialogTrigger
dialogsData.triggers[Message.wMsgATCTowerCallTheBall]			= arrivalDialogTrigger
dialogsData.triggers[Message.wMsgATCTowerSwitchMenu]			= arrivalDialogTrigger
--dialogsData.triggers[Message.wMsgATCYouAreClearedForTO]			= arrivalDialogTrigger
dialogsData.triggers[events.CLEAR_TO_TAKEOFF_FROM_SHIP]				= departureShipDialogTrigger
--dialogsData.triggers[events.TAKEOFF]							= arrivalDialogTrigger
dialogsData.triggers[Message.wMsgATCYouAreNotAuthorised]		= switchToMainMenu


local departureAirdromeDialogTrigger = DialogStartTrigger:new(dialogsData.dialogs['Departure Airdrome'])
dialogsData.triggers[events.NOTIFY_BIRTH_ON_RAMP_HOT]				= departureAirdromeDialogTrigger
dialogsData.triggers[events.NOTIFY_BIRTH_ON_RUNWAY]					= departureAirdromeDialogTrigger
dialogsData.triggers[events.STARTUP_PERMISSION_FROM_AIRDROME]		= departureAirdromeDialogTrigger
dialogsData.triggers[Message.wMsgATCClearedToTaxiRunWay]			= departureAirdromeDialogTrigger
dialogsData.triggers[events.DENY_TAKEOFF_FROM_AIRDROME]				= departureAirdromeDialogTrigger
dialogsData.triggers[events.CLEAR_TO_TAKEOFF_FROM_AIRDROME]			= departureAirdromeDialogTrigger

local departureHelipadDialogTrigger = DialogStartTrigger:new(dialogsData.dialogs['Departure Helipad'])
dialogsData.triggers[events.NOTIFY_BIRTH_ON_HELIPAD_HOT]			= departureHelipadDialogTrigger
dialogsData.triggers[events.STARTUP_PERMISSION_FROM_HELIPAD]		= departureHelipadDialogTrigger
dialogsData.triggers[events.DENY_TAKEOFF_FROM_HELIPAD]				= departureHelipadDialogTrigger
dialogsData.triggers[events.CLEAR_TO_TAKEOFF_FROM_HELIPAD]			= departureHelipadDialogTrigger
dialogsData.triggers[events.TAKEOFF]								= departureHelipadDialogTrigger

local departureShipDialogTrigger = DialogStartTrigger:new(dialogsData.dialogs['Departure Ship'])
dialogsData.triggers[events.NOTIFY_BIRTH_ON_SHIP_HOT]				= departureShipDialogTrigger
dialogsData.triggers[events.STARTUP_PERMISSION_FROM_SHIP]			= departureShipDialogTrigger
dialogsData.triggers[events.DENY_TAKEOFF_FROM_SHIP]					= departureShipDialogTrigger
dialogsData.triggers[events.CLEAR_TO_TAKEOFF_FROM_SHIP]				= departureShipDialogTrigger
dialogsData.triggers[events.TAKEOFF]								= departureShipDialogTrigger

	--Event Handler

local enginesAreStarted = false


local worldEventHandler = {
	onEvent = function(self, event)
		--print('worldEventHandler onEvent data.pUnit = '..tostring(data.pUnit and data.pUnit))
		--print('worldEventHandler onEvent event.id = '..tostring(event.id))
		--print('worldEventHandler onEvent event.initiator = '..tostring(event.initiator))		
		if event.initiator == data.pUnit then
			--print('worldEventHandler onEvent event.initiator == data.pUnit')
			local airbaseCommunicator= nil
			if event.place ~= nil and event.place:isExist() then
				--print('event.place ~= nil and event.place:isExist(), event.subPlace = '..tostring(event.subPlace))
				airbaseCommunicator = event.place:getCommunicator()
			end			
			if event.id == world.event.S_EVENT_BIRTH then
				if event.place ~= nil then
					--print('event.subPlace = '..tostring(event.subPlace))
					if event.subPlace == world.BirthPlace.wsBirthPlace_Park then
						return events.NOTIFY_BIRTH_ON_RAMP_COLD, airbaseCommunicator
					elseif event.subPlace == world.BirthPlace.wsBirthPlace_Park_Hot then
						enginesAreStarted = true
						return events.NOTIFY_BIRTH_ON_RAMP_HOT, airbaseCommunicator
					elseif event.subPlace == world.BirthPlace.wsBirthPlace_RunWay then
						enginesAreStarted = true
						return events.NOTIFY_BIRTH_ON_RUNWAY, airbaseCommunicator
					elseif event.subPlace == world.BirthPlace.wsBirthPlace_Heliport_Cold then
						return events.NOTIFY_BIRTH_ON_HELIPAD_COLD, airbaseCommunicator
					elseif event.subPlace == world.BirthPlace.wsBirthPlace_Heliport_Hot then
						enginesAreStarted = true
						return events.NOTIFY_BIRTH_ON_HELIPAD_HOT, airbaseCommunicator
					elseif event.subPlace == world.BirthPlace.wsBirthPlace_Ship_Cold then
						return events.NOTIFY_BIRTH_ON_SHIP_COLD, airbaseCommunicator
					elseif event.subPlace == world.BirthPlace.wsBirthPlace_Ship then
						enginesAreStarted = true
						return events.NOTIFY_BIRTH_ON_SHIP_HOT, airbaseCommunicator
					end
				end
			elseif event.id == world.event.S_EVENT_TAKEOFF or event.id == world.event.S_EVENT_RUNWAY_TAKEOFF then
				--print('worldEventHandler return events.TAKEOFF, airbaseCommunicator POPAL')
				return events.TAKEOFF, airbaseCommunicator
			elseif event.id == world.event.S_EVENT_LAND then
				--print('worldEventHandler  events.LANDING, airbaseCommunicator POPAL')
				return events.LANDING, airbaseCommunicator
			elseif event.id == world.event.S_EVENT_ENGINE_STARTUP then
				--print('worldEventHandler  events.ENGINE_STARTUP, airbaseCommunicator POPAL')
				enginesAreStarted = true
				return events.ENGINE_STARTUP, airbaseCommunicator
			elseif event.id == world.event.S_EVENT_ENGINE_SHUTDOWN then
				enginesAreStarted = false
				return events.ENGINE_SHUTDOWN, airbaseCommunicator
			end
		end
	end
}

table.insert(data.worldEventHandlers, worldEventHandler)

--Message Handler

local msgHandler = {
	onMsg = function(self, pMessage, pRecepient)
		--print('msgHandler onMsg pMessage:getEvent() = '..tostring(pMessage:getEvent() and pMessage:getEvent()))
		--print('msgHandler onMsg pMessage:getSender() = '..tostring(pMessage:getSender()))		
		self:onMsgEvent(pMessage:getEvent(), pMessage:getSender(), pRecepient)
	end,
	onMsgEvent = function(self, event, pMsgSender, pRecepient)		
		--print('msgHandler onMsgEvent event.id = '..tostring(event))
		local pUnit = pMsgSender:getUnit()
		local nUnitCategory = pUnit:getCategory()
		if nUnitCategory == Object.Category.BASE or nUnitCategory == Object.Category.UNIT then
			local airbaseCategory = pUnit:getDesc().category
			if event == Message.wMsgATCClearedForEngineStartUp then
				if airbaseCategory == Airbase.Category.HELIPAD then
					return events.STARTUP_PERMISSION_FROM_HELIPAD
				elseif airbaseCategory == Airbase.Category.AIRDROME then
					return events.STARTUP_PERMISSION_FROM_AIRDROME
				elseif airbaseCategory == Airbase.Category.SHIP then
					return events.STARTUP_PERMISSION_FROM_SHIP
				end
			elseif event == Message.wMsgATCTakeoffDenied then
				local typeName = pMsgSender:getUnit():getTypeName()
				if airbaseCategory == Airbase.Category.HELIPAD then
					return events.DENY_TAKEOFF_FROM_HELIPAD
				elseif airbaseCategory == Airbase.Category.AIRDROME then
					return events.DENY_TAKEOFF_FROM_AIRDROME
				elseif airbaseCategory == Airbase.Category.SHIP then
					return events.DENY_TAKEOFF_FROM_SHIP
				end
			elseif event == Message.wMsgATCYouAreClearedForTO then
				local typeName = pMsgSender:getUnit():getTypeName()
				if airbaseCategory == Airbase.Category.HELIPAD then
					return events.CLEAR_TO_TAKEOFF_FROM_HELIPAD
				elseif airbaseCategory == Airbase.Category.AIRDROME then
					return events.CLEAR_TO_TAKEOFF_FROM_AIRDROME
				elseif airbaseCategory == Airbase.Category.SHIP then
					return events.CLEAR_TO_TAKEOFF_FROM_SHIP
				end
			end
		end
	end	
}

table.insert(data.msgHandlers, msgHandler)