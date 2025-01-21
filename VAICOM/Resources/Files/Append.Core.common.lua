-- VAICOM PRO server-side script
-- common.lua (append)
-- www.vaicompro.com

function make(self, message)

	local role = self:findRole(message.event)
	base.assert(role ~= nil)
	
	local roleData = role
	
	--Country
	local country = getCountry(message)
	base.print('make: country: ', country)

	--Module & language
	local messageModuleName = nil
	local messageLanguage = nil
	local module = getModuleName(message)
	--base.print('make: module name: ', module)
	local desiredLanguage = languageByCountry[country]
	if 	module ~= nil and
		roleData.modules ~= nil then
		local messageModuleAndLanguage = 	getMessageModuleAndLanguage(roleData, module, desiredLanguage) or
											getMessageModuleAndLanguage(roleData, defaultModuleName, desiredLanguage) or
											getMessageModuleAndLanguage(roleData, module, nil) or
											getMessageModuleAndLanguage(roleData, defaultModuleName, nil)
											
		if not messageModuleAndLanguage then
			return nil
		end
		messageModuleName = messageModuleAndLanguage[1]
		messageLanguage = base.vaicom.settings.forcelanguage and base.vaicom.settings.forcedlanguage or messageModuleAndLanguage[2]
		roleData = roleData.modules[messageModuleName]
		if roleData == nil then
			base.print('make: module name = ', messageModuleName)
		end
	else
		messageLanguage = 	base.vaicom.settings.forcelanguage and base.vaicom.settings.forcedlanguage or getMessageLanguage(roleData, desiredLanguage) or
							getMessageLanguage(roleData, nil)
	end
	if 	messageLanguage ~= nil and
		roleData.languages ~= nil then
		roleData = roleData.languages[messageLanguage]
		if roleData == nil then
			base.print('make: language name = ', messageLanguage)
		end
	end
	
	--base.print('make: messageModuleName, messageLanguage: ',messageModuleName, messageLanguage)

	--handler
	local handler = (message.parameters and message.parameters.simple) and self.SimpleHandler or self:getHandler(message.event)
	
	local result = handler:make(message, messageLanguage)
	
	if result == nil then
		return nil
	end

	result.directory = 'Speech/'
	
	--Module and language to path
	if messageLanguage ~= nil then
		result.directory = result.directory..messageLanguage..'/'
	end
	if messageModuleName ~= nil then
		result.directory = result.directory..messageModuleName..'/'
	end	
	
	--Role
	result.directory = result.directory..role.dir..'/'
	
	--if roleData ~= nil then
		--Accent
		if roleData ~= nil and roleData.accents ~= nil then			
			local messageAccent = accentTable[messageLanguage][country] or accent.USA
			result.directory = result.directory..base.country.names[messageAccent]..'/'
			roleData = roleData.accents[messageAccent]
		end
	
		--Voice
		if 	roleData ~= nil and roleData.voices ~= nil and
			roleData.voices > 0 then
			local voice = message.sender:getVoice()	or 1
			voice = base.math.fmod(voice, roleData.voices)
			if voice == 0 then
				voice = roleData.voices
			end
			result.directory = result.directory..base.tostring(voice)..'/'
		end
	--end

	--Sender caption for subtitles
	local caption = self:makeCaption(role, message.sender, getAirdromeNameVariant(messageLanguage))
	
	--Radio clicks
	if message.radio then
		p.addRadioClicks(result)
	end
	
	--Result
	result.subtitle = caption..result.subtitle
	return result
	
end

handlersTable [base.Message.wMsgLeaderTowerOverhead]				= {
		make = function(self, message, language)
			if base.vaicom.settings.carriersuppressauto and message.receiver:getUnit() == base.world.getPlayer() then
				return nil
			end		
			local res			
			local country = message.receiver:getUnit():getCountry()
			local aircraftType = message.receiver:getUnit():getTypeName()			
			local low_state = self.sub.Digits:make(u.round(((message.receiver:getUnit():getFuelLowState() or 0)*message.receiver:getUnit():getDesc().fuelMassMax * 2.2046/ 1000), 0.1), '%.1f')
			--local angelsAlt = self.sub.Digits:make(u.round( message.parameters.altitude*3.281/1000, 0.1), '%.1f')
			
			local Altitude = u.round( 2*message.parameters.altitude*3.281/1000, 1.0)
			local angelsAlt = ''
			if Altitude % 2 == 0 then
				angelsAlt = self.sub.Number:make(Altitude/2)
			else
				angelsAlt = self.sub.Digits:make(Altitude/2, '%.1f')
			end
			
			--res = self.sub._start:make() +  self.sub.AirbaseName:make(message.sender, getAirdromeNameVariant(language)) + space_ + self.sub.tower:make()
			res = self.sub._start:make()  + space_ + self.sub.tower:make()
			res = res + comma_space_ 
			res = res + self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit())
			res = res + comma_space_ 
			res = res + self.sub.overhead:make() + space_ + angelsAlt + comma_space_ 
			if message.receiver:getUnit():getGroup():getSize() > 1 and message.parameters.case == 0 then
				local wingmansHH = ''
				for i = 2, 4 do
					local pWingmen = message.receiver:getUnit():getGroup():getUnit(i)
					if pWingmen ~= nil and pWingmen ~= message.receiver:getUnit() then
						wingmansHH = wingmansHH + self.sub.PlayerAircraftCallsign:make(pWingmen) + comma_space_ 
					end
				end
				res = res + (wingmansHH ~= '' and self.sub.holding_hands:make() + wingmansHH or '')
				--res = res + self.sub.flight_of:make(message.receiver:getUnit():getGroup():getSize())
				--res = res + comma_space_			
			end
			if message.receiver:getUnit():getGroup():getSize() == 1 then 
				res = res + (low_state ~= 0 and (self.sub.state:make() + space_ + low_state) or '')	
			else
				res = res + (low_state ~= 0 and (self.sub.low_state:make() + space_ + low_state) or '')			
			end
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	AirbaseName				= AirbaseName,
				tower					= Phrase:new({_('Tower'), 	'TOWER'}, 'Messages'),
				PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				overhead				= Phrase:new({_('overhead, angels'), 	'OVERHEAD'}, 'Messages'),							
				Altitude				= Phrase:new({_('angels'), 	'PLAYER-ANGELS'}, 'Messages'),			
				flight_of				= Phrases:new({	{_('flight of 1'), 'flight_of_1'},
														{_('flight of 2'), 'flight_of_2'},
														{_('flight of 3'), 'flight_of_3'},
														{_('flight of 4'), 'flight_of_4'},},			'Messages'),
				low_state				= Phrase:new({_('low state'), 'low_state'}, 'Messages'),
				state					= Phrase:new({_('state'), 'state'}, 'Messages'),
				holding_hands			= Phrase:new({_('holding hands with '), 'hands'}, 'Messages'),
				Digits					= Digits,
				Number					= Number,
				_start					= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end					= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),
				}			
	}

handlersTable [base.Message.wMsgLeaderConfirm]				        = {
		make = function(self, message, language)
			if base.vaicom.settings.carriersuppressauto and message.receiver:getUnit() == base.world.getPlayer() then
				return nil
			end
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit())
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	}	

--handlersTable [base.Message.wMsgLeaderFlightKissOff]				= {  -- No longer exits in core.common
		--make = function(self, message, language)
			--if base.vaicom.settings.carriersuppressauto and message.receiver:getUnit() == base.world.getPlayer() then
				--return nil
			--end
			--local res		
			--res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(base.world.getPlayer())
			--res = res  + self.sub._end:make()
			--return res
		--end,
		--sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				--_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				--_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	--}	
	
handlersTable [base.Message.wMsgLeaderConfirmRemainingFuel]			= {
		make = function(self, message, language)
			if base.vaicom.settings.carriersuppressauto and message.receiver:getUnit() == base.world.getPlayer() then
				return nil
			end		
			local low_state = self.sub.Digits:make(u.round(((message.receiver:getUnit():getFuelLowState() or 0)*message.receiver:getUnit():getDesc().fuelMassMax * 2.2046/ 1000), 0.1), '%.1f')
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit())
			if message.receiver:getUnit():getGroup():getSize() == 1 then 
				res = res + (low_state ~= 0 and (space_ + self.sub.state:make() + space_ + low_state) or '')	
			else
				res = res + (low_state ~= 0 and (space_ + self.sub.low_state:make() + space_ + low_state) or '')			
			end
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				low_state				= Phrase:new({_('low state'), 'low_state'}, 'Messages'),
				state					= Phrase:new({_('state'), 'state'}, 'Messages'),
				Digits					= Digits,
				_start					= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end					= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	}
	
handlersTable [base.Message.wMsgLeaderInboundMarshallRespond]		= {
		make = function(self, message, language)
			if base.vaicom.settings.carriersuppressauto and message.receiver:getUnit() == base.world.getPlayer() then
				return nil
			end	
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit())+ comma_space_
			res = res  + self.sub.marshal_on_the:make() + space_ + Digits:make((message.parameters.RBRC  or 0) * u.units.deg.coeff) + comma_space_
			res = res + self.sub.Number:make((message.parameters.NumberInStack or 0) +21) + space_+ self.sub.dme_angels:make() + space_ 
			res = res + self.sub.Number:make((message.parameters.NumberInStack or 0) + 6)  + comma_space_
			res = res  + self.sub.EAT:make() + space_ + self.sub.Digits:make((message.parameters.EAT or 0),'%02d')-- + comma_space_
			--res = res  + self.sub.approachButtonIs:make()
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				marshal_on_the		= Phrase:new({_('marshal on the'), 'marshal_on'}, 'Messages'),
				dme_angels			= Phrase:new({_('DME, angels'), 'dme'}, 'Messages'),
				EAT					= Phrase:new({_('expected approach time is'), 'EAT_TIME_IS'}, 'Messages'),
				approachButtonIs	= Phrase:new({_('approach button is 15.'), 'approach_button'}, 'Messages'),
				Digits				= Digits,
				Number				= Number,
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	}
	
handlersTable [base.Message.wMsgLeaderSayNeedle] 					= {
		make = function(self, message, language)				
			if base.vaicom.settings.carriersuppressauto and message.receiver:getUnit() == base.world.getPlayer() then
				return nil
			end		
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit()) + comma_space_
			res = res  + self.sub.needles:make(message.parameters.needles) + comma_space_
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				needles				= Phrases:new({	    {_('down and left'), 	'down_left'},
														{_('down and on'), 		'down_on'},
														{_('down and right'), 	'down_right'},														
														{_('on and left'), 		'ON_LEFT'},
														{_('on and on'), 		'ON_ON'},
														{_('on and right'), 	'ON_RIGHT'},														
														{_('up and left'), 		'UP_LEFT'},
														{_('up and on'), 		'UP_ON'},
														{_('up and right'), 	'UP_RIGHT'}, },			'Messages'),
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	}
	
--handlersTable [base.Message.wMsgLeaderPlatform]				        = { -- no longer exist in core.common
		--make = function(self, message, language)
			--if base.vaicom.settings.carriersuppressauto and message.receiver:getUnit() == base.world.getPlayer() then
				--return nil
			--end
			--local res		
			--res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(base.world.getPlayer()) + comma_space_
			--res = res  + self.sub.platform:make() 
			--res = res  + self.sub._end:make()
			--return res
		--end,
		--sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				--_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				--_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),
				--platform 			= Phrase:new(   { _('platform'),'PLATFORM'}		,   'Messages'),
				--}			
	--}
	
handlersTable [base.Message.wMsgLeaderHornetBall]					= {
		make = function(self, message, language)
			if base.vaicom.settings.carriersuppressauto and message.receiver:getUnit() == base.world.getPlayer() then
				return nil
			end	
			local aircraftNickNames = 
				{
					['FA-18C_hornet']	  	= 1,	-- hornet
					['F/A-18C']	  			= 1,	-- hornet
					['F-14A']	  			= 2,	-- tomcat
					['F-14B']	  			= 2,	-- tomcat
					['F-14A-135-GR']		= 2,	-- tomcat
					['F-14A-95-GR']			= 2,	-- tomcat
					['E-2C']	  			= 3,	-- HAWKEYE
					['E-2D']	  			= 3,	-- HAWKEYE
					['S-3B Tanker']	  		= 4,	-- VIKING
					['S-3B']	  			= 4,	-- VIKING
					['F-4E']	  			= 5,	-- PHANTOM
					['F-4E_new']	  		= 5,	-- PHANTOM
					['C-2']			  		= 6,	-- Greyhound
					['A-6']			  		= 7,	-- Intruder
					['F-35']		  		= 8,	-- lightning
					['EA-6B']		  		= 9,	-- Prowler
					['A-4']		  			= 10,	-- Skyhawk
				}
			--base.print('	!!!!!~~~~~!!!!!!! aircraftNickNames[message.sender:getUnit():getTypeName()]:',aircraftNickNames[message.sender:getUnit():getTypeName()])
			local aircraftTypeIdx = aircraftNickNames[message.sender:getUnit():getTypeName()] or 1
			local res			
			local low_state = self.sub.Digits:make(u.round(((message.sender:getUnit():getFuel() or 0)*message.sender:getUnit():getDesc().fuelMassMax * 2.2046/ 1000), 0.1), '%.1f')
						
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.sender:getUnit())
			res = res + comma_space_ 
			res = res + self.sub.hornet_ball:make(aircraftTypeIdx) + comma_space_ 
			res = res + (low_state ~= 0 and (low_state) or '')
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	hornet_ball				= Phrases:new({								-- propper name by index from aircraftNickNames table
														{_('hornet ball'), 		'h_b'}, -- all sound files renamed to short version to reduce length calculation
														{_('tomcat ball'), 		't_b'},
														{_('hawkeye ball'), 	'ha_b'},
														{_('viking ball'), 		'v_b'},
														{_('phantom ball'), 	'p_b'},
														{_('greyhound ball'), 	'g_b'},
														{_('lightning ball'), 	'l_b'},
														{_('prowler ball'), 	'pr_b'},
														{_('skyhawk ball'), 	's_b'},
													 }, 'Messages'),
				PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				Digits					= Digits,
				_start					= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end					= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),
				}			
	}