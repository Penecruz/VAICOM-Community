-- VAICOM PRO server-side script
-- RadioCommandDialogsPanel.lua (append)
-- www.vaicompro.com

base.package.path  = base.package.path..";.\\LuaSocket\\?.lua;"
base.package.cpath = base.package.cpath..";.\\LuaSocket\\?.dll;"

local	socket 		= base.require('socket')
local 	JSON    	= base.require('JSON')
local 	dcsoptions 	= base.require('optionsEditor')
local   Gui         = base.require('dxgui')

local colorByRecepientState = {
		[RecepientState.VOID] 				= utils.COLOR.LIGHT_GRAY,	 
		[RecepientState.TUNED] 				= utils.COLOR.WHITE, 		 
		[RecepientState.CAN_BE_TUNED] 		= utils.COLOR.LIGHT_GRAY, 	 
		[RecepientState.CANNOT_BE_TUNED] 	= utils.COLOR.LIGHT_GRAY     
	}

function initialize(pUnitIn, easyComm, intercomId, communicators)
	count=count+1	
	base.assert(COMMUNICATOR_VOID ~= nil)
	base.assert(COMMUNICATOR_AUTO ~= nil)
	data.curCommunicatorId = COMMUNICATOR_VOID
	base.assert(data.initialized == false)
	setEasyComm(easyComm == nil or easyComm)
	setUnit_(pUnitIn)	
	data.intercomId = data.intercomId or intercomId
	base.assert(data.communicators == nil)
	data.communicators = {}
	if communicators ~= nil then
		for communicatorId, communicator in base.pairs(communicators) do
			data.communicators[communicatorId] = communicator
		end
	end	
	local dialogsData = {
		dialogs 	= {},
		triggers 	= {}
	}	
	local function TO_STAGE(dialogName, stageName, pushToStack)
		return TO_DIALOG_STAGE(dialogsData, dialogName, stageName, pushToStack)
	end	
	local groundUnitScriptNameDefault = 'Scripts/UI/RadioCommandDialogPanel/Config/GroundUnit.lua'
	local specificScriptName = nil
	if pUnitIn ~= nil then
		specificScriptName = pUnitIn:hasAttribute('Air') and base.Objects[pUnitIn:getTypeName()].HumanCommPanelPath or groundUnitScriptNameDefault
	end
	local scriptNameDefault = 'Scripts/UI/RadioCommandDialogPanel/Config/default.lua'	
	local env =
	{
		table						= base.table,
		math						= base.math,
		pairs						= base.pairs,
		getfenv						= base.getfenv,
		require						= base.require,
		assert						= base.assert,
		print						= base.print,	
		tostring					= base.tostring,
		string						= base.string,
		_ 							= _,
		db							= base.db,	
		world						= base.world,
		Object						= base.Object,
		Airbase						= base.Airbase,
		Message						= base.Message,
		coalition					= base.coalition,
		MissionResourcesDialog		= base.MissionResourcesDialog,		
		utils						= base.utils,	
		data 						= data,
		getSelectedReceiver			= function()
			return self.curDialogIt.element_.receiver
		end,
		RecepientState 				= RecepientState,
		getRecepientsState  		= getRecepientsState,
		getRecepientColor 			= getRecepientColor,
		getRecepientsColor 			= getRecepientsColor,
		selectAndTuneCommunicator 	= selectAndTuneCommunicator,		
		setBehaviorOption			= setBehaviorOption,
		sendMessage					= sendMessage,
		buildRecepientsMenu 		= buildRecepientsMenu,
		buildRecepientsMenuATC2		= buildRecepientsMenuATC2,
		buildRecepientsMenuATC		= buildRecepientsMenuATC,
		buildCargosMenu				= buildCargosMenu, 
		buildMooseMenu				= buildMooseMenu, -- Pene Do we need to buildMooseMenu?
		buildCargosMenuForAircraft	= buildCargosMenuForAircraft,
		buildDescentsMenu			= buildDescentsMenu,		
		staticParamsEvent			= staticParamsEvent,			
		events 						= events,
		dialogsData 				= dialogsData,
		TERMINATE					= TERMINATE,
		TO_STAGE 					= TO_STAGE,
		RETURN_TO_STAGE 			= RETURN_TO_STAGE,
		DialogStartTrigger 			= DialogStartTrigger
	}	
	local scriptName = specificScriptName or scriptNameDefault	
	if scriptName ~= nil then
		local aircraftRadioCommandPanel, errMsg = utils.loadfileIn(scriptName, env)
		if aircraftRadioCommandPanel == nil then
			base.error(errMsg)
		end
		aircraftRadioCommandPanel()
	end	
	heightMenu = commandDialogsPanel.initialize(self, data.menus, data.rootItem, dialogsData)
	base.world.addEventHandler(worldEventHandler)	
	self:toggle(true)
	data.initialized = true
	setHeightCommandMenu( heightMenu )
	base.vaicom.init.stop()
	base.vaicom.init.start()
end
function checkRadioCommunicatorTuned(target, communicator, communicatorId)
	local tuned = false
	local radioavail = base.GetDevice(data.intercomId):is_communicator_available(communicatorId)	
	local freqModTbl = target:getFrequenciesModulations()	
	if radioavail and (freqModTbl ~= nil) then	
		local radiofreq = base.GetDevice(communicatorId).get_frequency and base.GetDevice(communicatorId):get_frequency() or 0
		local radiomod = base.GetDevice(communicatorId).get_modulation and base.GetDevice(communicatorId):get_modulation() or 0			
		for transiverId, freqMod in base.pairs(freqModTbl) do
			local tgtmod = freqMod.modulation
			if (radiomod == tgtmod) and (base.math.abs(radiofreq - freqMod.frequency) < 2500) then				
				tuned = true
				break
			end			
		end	
	end	
	return tuned
end
function checkRadioCommunicatorAvailability(target, communicator, communicatorId)
	if target == nil then
		return true
	end	
	local freqModTbl = target:getFrequenciesModulations()
	if freqModTbl ~= nil then
		for transiverId, freqMod in base.pairs(freqModTbl) do
			if checkCommunicator(communicator, freqMod.frequency, freqMod.modulation) then
				if base.GetDevice(data.intercomId):is_communicator_available(communicatorId) then
					return true
				end
			end
		end
	end
	return false
end
function selectCommunicatorDeviceId(targetCommunicator)
	if data.intercomId == nil or data.communicators == nil then	
		return nil
	end
	if targetCommunicator == nil then
		return data.intercomId
	end
	local ID = nil
	if (data.curCommunicatorId == COMMUNICATOR_VOID) or (data.curCommunicatorId == COMMUNICATOR_AUTO) or (data.curCommunicatorId == 0) then
		for communicatorId, communicator in base.pairs(data.communicators) do 
			if base.GetDevice(communicatorId) and base.GetDevice(communicatorId).is_on and base.GetDevice(communicatorId):is_on() and checkRadioCommunicatorTuned(targetCommunicator, communicator, communicatorId) then
				ID = communicatorId 
				break
			end
		end
		if not ID then
			for communicatorId, communicator in base.pairs(data.communicators) do 
				if base.GetDevice(communicatorId) and base.GetDevice(communicatorId).is_on and base.GetDevice(communicatorId):is_on() and checkRadioCommunicatorAvailability(targetCommunicator, communicator, communicatorId) then
					ID = communicatorId 
					break
				end
			end		
		end	 
	else
		local communicatorId = data.curCommunicatorId
		if checkRadioCommunicatorAvailability(targetCommunicator, data.communicators[communicatorId], communicatorId) then 
			ID = communicatorId 
		end
	end
	return ID
end
function selectAndTuneCommunicator(targetCommunicator)
	if data.intercomId == nil or data.communicators == nil then	
		return nil
	end
	local communicatorId = selectCommunicatorDeviceId(targetCommunicator) or data.curCommunicatorId
	if (communicatorId == COMMUNICATOR_VOID) or (communicatorId == COMMUNICATOR_AUTO) or (communicatorId == 0) then
		return nil
	end
	base.GetDevice(data.intercomId):make_setup_for_communicator(communicatorId)
	if data.radioAutoTune or ((not base.vaicom.state.activemessage.havedial) or base.vaicom.settings.operatedial) then  
		base.GetDevice(data.intercomId):set_communicator(communicatorId)
	end	
	local communicator = data.communicators[communicatorId]		
	if not communicator.interphone then 
		local commDevice = base.GetDevice(communicatorId)
		if data.radioAutoTune or base.vaicom.state.activemessage.forcetune then	
			local freqModTbl = targetCommunicator:getFrequenciesModulations()			
			for transiverId, freqMod in base.pairs(freqModTbl) do
				local haveFreq = false
				if communicator.channels then
					local channelNum = findCommunicatorChannel(communicator.channels, freqMod.frequency)
					if channelNum ~= nil then
						haveFreq = true
						commDevice:set_channel(channelNum)
					end
				else
					if commDevice:is_frequency_in_range(freqMod.frequency) then
						commDevice:set_frequency(freqMod.frequency)
						haveFreq = true
					end
				end
				if haveFreq then
						commDevice:set_modulation(freqMod.modulation) 	
					break
				end
			end	
		else
		end
	end	
	return communicatorId
end
function banMouse(self, on)
end
function setCommunicatorId(curCommunicatorIdIn)
	data.curCommunicatorId = curCommunicatorIdIn 	
	updateMainCaption()	
end
function updateMainCaption()
	if not data.initialized or not hasUnit() then
		return
	end
	local mainCaption = ''
	if base.vaicom.flags.remote then	
		mainCaption = base.tostring(base.vaicom.state.activemessage.tgtdevname or "----")
	else
		if data.curCommunicatorId == COMMUNICATOR_AUTO then
			mainCaption = _('AUTO')
		elseif data.curCommunicatorId ~= COMMUNICATOR_VOID then
			local communicator = data.communicators[data.curCommunicatorId]
			if communicator then
				mainCaption =  communicator.displayName
			else
				mainCaption = '???'
			end
		end
	end
	if data.VoIP then
		mainCaption = mainCaption.._(' ...VoIP...')
	end
	commandDialogsPanel.setMainCaption(self, mainCaption)
end
function setShowMenu(on)
	if data.initialized and hasUnit() then 
		
		on = on and not base.vaicom.settings.menuinvisible	
		self.mainCaption:setVisible(on)
		commandDialogsPanel.setShowMenu(self, on)
		return	
			
	end
end
function RemoteInputs() --check remote Inputs for errors
	local returnvalue = false			
	datareadout = base.vaicom.receiver:receive()
	if datareadout then 
		base.vaicom.state.rawcommand	= datareadout
		returnvalue = true
	else 
		base.vaicom.state.rawcommand	= base.vaicom.flags.raw
		returnvalue = false
	end
	return returnvalue
end 
function DecodeMessage(rawdata)
	local decodeerror = false	
	base.vaicom.state.activemessage = {}
	function JSON:onDecodeError(message, text, location, etc)
		if not decodeerror then
		end
		decodeerror = true
	end
	local msg = JSON:decode(rawdata)
	if decodeerror then 
		return nil 
	end
	if not decodeerror and base.type(msg) ~= "table"  then 
		decodeerror = true
		return nil
	end
	base.vaicom.state.activemessage = msg		
	returnclient = msg.client or false
	return returnclient
end		
function ProcessRemoteCommand()

	if not DecodeMessage(base.vaicom.state.rawcommand) then
		socket.try(base.vaicom.sender:send(base.vaicom.flags.raw))
		return
	end
	
	local clientmessage = base.vaicom.state.activemessage 
	
	updateMainCaption()	
	base.vaicom.state.update.all()
	
	if clientmessage.dspmsg 									~=nil	then
		base.trigger.action.outTextForGroup(data.pUnit:getGroup().id_, clientmessage.dspmsg,clientmessage.msgdur or 5)
	end
	if clientmessage.exec 										~=nil	then
		base.assert(base.loadstring(clientmessage.exec))()
		socket.try(base.vaicom.sender:send(base.vaicom.flags.raw))
		return
	end
	if clientmessage.type == base.vaicom.messagetype.undefined 			then						
		socket.try(base.vaicom.sender:send(base.vaicom.flags.raw))
		return
	end
	
	ApplySettings(clientmessage)
	
	if clientmessage.type == base.vaicom.messagetype.settingschange 	then		
		socket.try(base.vaicom.sender:send(base.vaicom.flags.raw))
		return
	end		
	if clientmessage.type == base.vaicom.messagetype.requestupdate  	then			
		base.vaicom.state.sendupdateall()		
		return
	end				
	if clientmessage.type == base.vaicom.messagetype.devicecontrol  	then
		for i= 1, #clientmessage.extsequence do
		  base.GetDevice(clientmessage.extsequence[i].device):performClickableAction(clientmessage.extsequence[i].command,clientmessage.extsequence[i].value)
		end		
		for i= 1, #clientmessage.devsequence do
		  base.GetDevice(clientmessage.devsequence[i].device):performClickableAction(clientmessage.devsequence[i].command,clientmessage.devsequence[i].value) 
		end		
		socket.try(base.vaicom.sender:send(base.vaicom.flags.raw))
		return
	end
	if clientmessage.type == base.vaicom.messagetype.commandsequence	then	
		if clientmessage.showmenu then
			self.mainCaption:setVisible(true)
			commandDialogsPanel.setShowMenu(self, true)		
		end
		for i= 1, #clientmessage.cmdsequence do
		  base.Export.LoSetCommand(clientmessage.cmdsequence[i])
		end	
		socket.try(base.vaicom.sender:send(base.vaicom.flags.raw))
		return
	end
	if clientmessage.type == base.vaicom.messagetype.actionsequence 	then
		for i= 1, #clientmessage.actionsequence do
		  base.missionCommands.doAction(clientmessage.actionsequence[i])
		end	
		socket.try(base.vaicom.sender:send(base.vaicom.flags.raw))
		return
	end
	if clientmessage.type == base.vaicom.messagetype.aicomms			then -- Rearming Menu call testing
		local unitcomm, tgtunit = SetTargetComm(clientmessage.command)
		if clientmessage.command == base.Message.wMsgLeaderRequestRearming then			
			base.MissionResourcesDialog.show(true)
			return
		end
		data.curCommunicatorId = clientmessage.tgtdevid or data.curCommunicatorId
		selectAndTuneCommunicator(unitcomm)
		local messagesendcommand	= clientmessage.command
		local messagesendparams     = SetParameters(unitcomm)
		if messagesendcommand == base.Message.wMsgLeaderSpecialCommand then
			purgeMessage =	{
							type = base.Message.type.TYPE_CONSTRUCTABLE,
							playMode = base.Message.playMode.PLAY_MODE_LIMITED_DURATION,						
							event = base.Message[clientmessage.dcsid] or messagesendcommand,
							params = clientmessage.parameters or {},
							perform = function(self,parameters)
								data.pComm:sendMessage({	type		= self.type,
															playMode	= self.playMode,
															event		= self.event,
															parameters	= self.params,
															})											
							end	
							}			
		end
		if messagesendcommand ~= base.Message.wMsgLeaderSpecialCommand then					
			purgeMessage =	{	
							type = base.Message.type.TYPE_CONSTRUCTABLE,
							playMode = base.Message.playMode.PLAY_MODE_LIMITED_DURATION,	
							event = base.Message[clientmessage.dcsid] or messagesendcommand,
							parameters = messagesendparams,
							perform = function(self, parameters)
								local messageParameters = {}
								local command = self.event
								if self.parameters then
									for i, p in base.pairs(self.parameters) do
										base.table.insert(messageParameters, p)
									end
								elseif self.getParameter then
									base.table.insert(messageParameters, self.getParameter())	
								end
								data.pComm:sendRawMessage(command, messageParameters)
							end
							}	
		end
		socket.try(base.vaicom.sender:send(base.vaicom.flags.raw))		
		base.setmetatable(purgeMessage, sendMessage)
		purgeMessage:perform()
	end			

end
function ApplySettings(message)
	base.vaicom.set.debugmode(message.debug)
	if message.menuinvisible      	~= nil 			then 
		if (message.menuinvisible ~= base.vaicom.settings.menuinvisible) then
			local on = not base.vaicom.state.activemessage.menuinvisible
			self.mainCaption:setVisible(on or data.VoIP)
			commandDialogsPanel.setShowMenu(self, on)
		end
		base.vaicom.settings.menuinvisible = message.menuinvisible
	end	
	if message.disableplayervoice 	~= nil 			then
		local on = message.disableplayervoice	
		base.vaicom.settings.playervoicedisabled = on
			if on then
				base.common.role.PLAYER.dir = 'DISABLED_Player'
				if base.common.role.PLAYER_NAVY then
					base.common.role.PLAYER_NAVY.dir = 'DISABLED_Player'
				end
			else
				base.common.role.PLAYER.dir = 'PLAYER'
				if base.common.role.PLAYER_NAVY then
					base.common.role.PLAYER_NAVY.dir = 'NAVY_Player'
				end
			end
	end		
	if message.forcelanguage      	~= nil 			then
		base.vaicom.settings.forcelanguage = message.forcelanguage
	end	
	if message.forcedlanguage      	~= nil 			then
		base.vaicom.settings.forcedlanguage = message.forcedlanguage	
	end	
	if message.forcenatoprotocol  	~= nil 			then
		local on = message.forcenatoprotocol
		base.vaicom.settings.forcenatoatcnames = on		
		if on then
			base.common.getAirdromeNameVariant = function(language)
				return 'NATO'
			end
		else
			base.common.getAirdromeNameVariant = function(language)
				if language == 'RUS' then
					return 'USSR'
				else 
					if base.vaicom.settings.forcedlanguage then
						return 'USSR'
					else
						return 'NATO'
					end
				end
			end
		end	
	end		
	if message.forcecallsigns 		~= nil 			then
		local on = message.forcecallsigns
		base.vaicom.settings.forcecallsigns = on		
		if on then
			base.common.hasNumericCallsign = function(pUnit)
				return base.vaicom.settings.forcedcallsigns == 'RUS'
			end
		else
			base.common.hasNumericCallsign = function(pUnit)
				local country = pUnit:getCountry()
				local forcesName = pUnit:getForcesName()
				return 	country == base.country.RUSSIA or
						country == base.country.UKRAINE or
						country == base.country.BELARUS or
						country == base.country.INSURGENTS or
						country == base.country.ABKHAZIA or
						country == base.country.SOUTH_OSETIA or
						country == base.country.CHINA or
						country == base.country.VIETNAM or 
						country == base.country.USSR or
						country == base.country.YUGOSLAVIA or
						country == base.country.GDR
						--or (country == nations.USA and forcesName == 'NAVY')	-- TODO: Make correct Numeric Callsign for US NAVY
			end
		end		
	end
	if message.forcedcallsigns      ~= nil 			then
		base.vaicom.settings.forcedcallsigns = message.forcedcallsigns	
	end	
	if message.operatedial 		    ~= nil 			then
		base.vaicom.settings.operatedial = message.operatedial
	end
	if message.tunenum				~= nil 			then
		local tune = message.tgtdevid and base.GetDevice(message.tgtdevid)
		local setchn = tune and message.tunechn and base.GetDevice(message.tgtdevid).set_channel and base.GetDevice(message.tgtdevid):set_channel(message.tunechn)
		for i= 1, #message.tunefrq do
			if tune and message.tunefrq and base.GetDevice(message.tgtdevid).set_frequency and base.GetDevice(message.tgtdevid):is_frequency_in_range(message.tunefrq[i]) and base.GetDevice(message.tgtdevid):set_frequency(message.tunefrq[i]) then
				break
			end
		end
		local setmod = tune and message.tunemod and base.GetDevice(message.tgtdevid).set_modulation and base.GetDevice(message.tgtdevid):set_modulation(message.tunemod)
	end
	if message.redirect_world_speech~= nil 		    then
		local on = message.redirect_world_speech
		base.vaicom.settings.redirect_world_speech = on
		local route1
		local route2
		if on then 
			route1 = ""
			route2 = base.vaicom.state.activemessage.fc3 and "" or ""
		else
			route1 = ""
			route2 = ""
		end
		base.common.role.WINGMAN.dir					= route1..'Wingman'
		base.common.role.ATC.dir 						= route1..'ATC'
		base.common.role.AWACS.dir 						= route1..'AWACS'
		base.common.role.TANKER.dir 					= route1..'Tanker'
		base.common.role.JTAC.dir 						= route1..'JTAC'
		base.common.role.CCC.dir 						= route1..'CCC'
		base.common.role.ALLIED_FLIGHT.dir 				= route1..'Allied Flight'
		base.common.role.BETTY.dir 						= route2..'Betty'
		base.common.role.ALMAZ.dir 						= route1..'ALMAZ'
		base.common.role.RI65.dir 						= route1..'RI65'
		base.common.role.ExternalCargo.dir 				= route1..'External Cargo'
		base.common.role.A10_VMU.dir 					= route2..'A-10 VMU'
		base.common.role.GROUND_CREW.dir 				= route1..'Ground Crew'		
		base.common.role.ATC_NAVY_APPROACH_TOWER.dir	= route1..'ATC_NAVY_Approach_Tower'
		base.common.role.ATC_NAVY_DEPARTURE.dir			= route1..'ATC_NAVY_Departure'
		base.common.role.ATC_NAVY_LSO.dir				= route1..'ATC_NAVY_LSO'
		base.common.role.ATC_NAVY_MARSHALL.dir			= route1..'ATC_NAVY_Marshal'		
	end
	if message.carriersuppressauto 	~= nil 			then
		base.vaicom.settings.carriersuppressauto = message.carriersuppressauto
	end	
	if message.kneeboard			~= nil			then
		base.Export.LoSetCommand(1587,message.kneeboard)
		local dev = base.GetDevice(255)
		if base.type(dev)== 'table' and dev.performClickableAction then
			dev:performClickableAction(3001,message.kneeboard)
		end
	end
	if message.dictmode				~= nil			then
		local dev = base.GetDevice(255)
		if base.type(dev)== 'table' and dev.performClickableAction then
			dev:performClickableAction(3006,message.dictmode and 1 or 0)
		end
	end
end
function SetTargetComm(sendevent)	
	local returncomm = nil
	for n, k in base.pairs(base.vaicom.state.availablerecipients[base.vaicom.state.activemessage.reccat]) do
		if k.id_ == base.vaicom.state.activemessage.selectunit then
			base.vaicom.state.selectedrecipients[base.vaicom.state.activemessage.reccat] = k
		end				
	end		
	local selectunit = nil
	if base.vaicom.state.availabilitycounter[base.vaicom.state.activemessage.reccat] > 0 then
		if base.vaicom.state.selectedrecipients[base.vaicom.state.activemessage.reccat] then 
			selectunit = base.vaicom.state.selectedrecipients[base.vaicom.state.activemessage.reccat]	
		else 
			if base.vaicom.state.activemessage.reccat == "Flight" and (#base.vaicom.state.availablerecipients[base.vaicom.state.activemessage.reccat] > 1) then
				selectunit = base.vaicom.state.availablerecipients[base.vaicom.state.activemessage.reccat][2]
			else
				selectunit = base.vaicom.state.availablerecipients[base.vaicom.state.activemessage.reccat][1]
			end
		end
	end				
	if selectunit then returncomm = selectunit:getCommunicator() end	
	if not returncomm then 
		returncomm = nil
	end 		
	return returncomm, selectunit	
end
function SetParameters(recipientcomm)
	local returnparams = {}
		if base.vaicom.state.activemessage.insert then 
		base.table.insert(returnparams,recipientcomm)  
		end	
		if base.vaicom.state.activemessage.parameters then	
			for i= 1, #base.vaicom.state.activemessage.parameters do
				local paramval = base.vaicom.state.activemessage.parameters[i]
				base.table.insert(returnparams,paramval)
			end			
		end
	return returnparams		
end
function onMsgStart(pMessage, pRecepient, text)
	if not data.initialized then
		return
	end	
	local pMsgSender	= pMessage:getSender()
	local pMsgReceiver	= pMessage:getReceiver()
	local event			= pMessage:getEvent()
	base.assert(pMsgSender.id_ ~= nil)
	local ttt = { id_ = pMsgSender.id_ }
	base.setmetatable(ttt, base.getmetatable(pMsgSender) )
	base.assert(pMsgSender == ttt)
	if pMsgSender ~= nil then
		commById[pMsgSender:tonumber()] = pMsgSender
	end
	if pMsgReceiver ~= nil then
		commById[pMsgReceiver:tonumber()] = pMsgReceiver
	end	
	if 	data.pComm == nil or
		pRecepient ~= data.pComm then
		return
	end
	local textColor = getMessageColor(pMsgSender, pMsgReceiver, event)
	if pMsgReceiver == data.pComm or pMsgSender == data.pComm then
		for msgHandlerIndex, msgHandler in base.pairs(data.msgHandlers) do
			local internalEvent, receiverAsRecepient = msgHandler:onMsg(pMessage, pRecepient)
			if internalEvent ~= nil then
				self:onEvent(internalEvent, pMsgSender and pMsgSender, pMsgReceiver:tonumber() and pMsgReceiver:tonumber(), receiverAsRecepient)
			end
		end
		self:onEvent(event, pMsgSender and pMsgSender:tonumber(), pMsgReceiver and pMsgReceiver:tonumber())
	end
	if pMsgReceiver == data.pComm or pMsgSender == data.pComm then
		commandDialogsPanel.onMsgStart(self, pMsgSender:tonumber(), pMsgReceiver and pMsgReceiver:tonumber(), text, textColor)
	end
	sendtbl = {}
	sendtbl.domsg			= true
	sendtbl.pMsgSender 		= pMsgSender
	sendtbl.pMsgReceiver	= pMsgReceiver
	sendtbl.eventid			= event
	sendtbl.eventkey		= base.vaicom.helper.messagekey(event)
	sendtbl.text			= text
	sendtbl.parameters 		= pMessage:getTable().parameters
	sendtbl.speech 			= base.vaicom.state.currentspeech
	sendtbl.fsm				= base.tostring(base.fsm.state)
	socket.try(base.vaicom.relay:send(JSON:encode(sendtbl)))
end
function onMsgFinish(pMessage, pRecepient, text)
	if not data.initialized then
		return
	end
	local pMsgSender	= pMessage:getSender()
	local pMsgReceiver	= pMessage:getReceiver()
	if pMsgSender ~= nil then
		commById[pMsgSender:tonumber()] = pMsgSender
	end
	if pMsgReceiver ~= nil then
		commById[pMsgReceiver:tonumber()] = pMsgReceiver
	end	
	if pMsgReceiver == data.pComm or pMsgSender == data.pComm then
		commandDialogsPanel.onMsgFinish(self, pMsgSender:tonumber(), pMsgReceiver and pMsgReceiver:tonumber(), text)
	end
	sendtbl = {}
	sendtbl.domsg			= false
	sendtbl.fsm				= base.tostring(base.fsm.state)
	socket.try(base.vaicom.relay:send(JSON:encode(sendtbl)))
end

base.vaicom = base.vaicom or {}
local function vaicom_loop()
	local 	JSON    	= base.require('JSON') -- is it really needed? had a weird error, maybe it was something else causing a issue
	if base.vaicom and base.vaicom.receiver and data.initialized and data.pUnit then 
		if RemoteInputs() then 	
			base.vaicom.flags.remote = true
			ProcessRemoteCommand()
		else
			if data.initialized then 
				if base.vaicom.flags.remote then
					base.vaicom.flags.remote = false	
					return	
				end
			end
		end
	else
		base.print("KILL VAICOM LOOP")
		Gui.EnableHighSpeedUpdate(true) -- default = false Pene WIP run high speed true for testing
		Gui.RemoveUpdateCallback(vaicom_loop)
		end
end
base.vaicom.config = {
	sendaddress 		= "127.0.0.1", 
	sendport 			= 33492,
	sendtimeout 		= 0,
	receiveaddress 		= "127.0.0.1",
	receiveport 		= 33334,
	receivetimeout 		= 0,
	relayaddress 		= dcsoptions.getOption("plugins.VAICOM.VAICOMClientIP") or "127.0.0.1",
	relayport 			= 44111,
	relaytimeout 		= 0,
}
base.vaicom.flags = {
	raw					= 4000,
	remote				= false,
}
base.vaicom.settings = {
	
	menuinvisible			= false,
	redirect_world_speech	= false,
	operatedial				= false,
	playervoicedisabled		= false,
	forcelanguage			= false,
	forcedlanguage 			= 'ENG',
	forcenatoatcnames		= false,
	forcecallsigns			= false,
	forcedcallsigns			= 'ENG',
	carriersuppressauto		= false,

}
base.vaicom.messagetype = {
	undefined			= "sim.undefined",
	settingschange 		= "sim.changesettings",	
	requestupdate 		= "mission.player.requestupdate",
	requestdevstate 	= "mission.player.requestdevstate",
	devicecontrol  		= "mission.player.devicecontrol",	
	commandsequence 	= "mission.player.cmdsequence",
	actionsequence  	= "mission.player.actionsequence",
	aicomms 			= "mission.player.aicomms",
}
base.vaicom.categories = {
	recipient = {					
				Player 		= "Player",
				Flight		= "Flight",
				JTAC		= "JTAC",
				ATC			= "ATC",			
				AWACS		= "AWACS",
				Tanker		= "Tanker",
				Crew		= "Crew",
				Aux			= "Aux",
				Cargo		= "Cargo",
				Allies		= "Allies",
				Moose		= "Moose", -- Adding Moose
				},		
	coalitions = {	
					[0] 	= "neutral",
					[1] 	= "red",
					[2] 	= "blue",	
				},		
}
base.vaicom.properties = {
	range = function(Locator)
		local range = 0
		if Locator ~= nil then
			local selfPoint = data.pUnit and data.pUnit:getPosition().p 
			if not selfPoint then
				selfPoint = Locator:getPoint()
			end
			local ipoint = Locator:getPoint()
			local distsq = (ipoint.x - selfPoint.x) * (ipoint.x - selfPoint.x) + (ipoint.z - selfPoint.z) * (ipoint.z - selfPoint.z)
			range = base.math.floor(base.math.sqrt(distsq))
		end
		return range
	end,
	pos = function(Locator)
		if Locator ~= nil and Locator.getPoint then
			return Locator:getPoint()
		else
			return nil
		end
	end,
	displayname = function(Locator)
		displaystr = Locator:getDesc() and Locator:getDesc().displayName or "unknown"
		return displaystr
	end,
	typename = function(Locator)
		local displaystr = "unknown"
		if Locator:getDesc() ~= nil then
			displaystr = Locator:getDesc().typeName
		end
		return displaystr
	end,
	attributes = function(Locator)
		local attr = {}
		if Locator:getDesc().attributes then
			attr = Locator:getDesc().attributes
		end
		return attr
	end,
	description = function(Locator)
	local descr = {}
		if Locator:getDesc() then
			descr = Locator:getDesc()
		end
	return descr
	end,
	missioncallsign = function(Locator)
		local callsignStr = "unknown"
		local UnitCommunicator = nil
		if Locator ~= nil then
			UnitCommunicator = Locator:getCommunicator()
		end
		if UnitCommunicator then
			local useprotocol = base.speech.defaultProtocol
			if base.vaicom.settings.forcecallsigns then
				local callsignStr1 
				local callsignStr2
				if base.vaicom.settings.forcedcallsigns == 'RUS' then  
					base.common.hasNumericCallsign = function(pUnit)
						return true
					end
					callsignStr1 = base.speech.protocols[useprotocol]:makeCallsignString(UnitCommunicator) or "unknown"
					base.common.hasNumericCallsign = function(pUnit)
						return false
					end
					callsignStr2 = base.speech.protocols[useprotocol]:makeCallsignString(UnitCommunicator) or "unknown"
					callsignStr = callsignStr1.." ("..callsignStr2..")"	
				else
					base.common.hasNumericCallsign = function(pUnit)
						return base.vaicom.settings.forcedcallsigns == 'RUS'
					end
					callsignStr = base.speech.protocols[useprotocol]:makeCallsignString(UnitCommunicator) or "unknown"
				end
			else
				base.common.hasNumericCallsign = function(pUnit)
					local country = pUnit:getCountry()
					local forcesName = pUnit:getForcesName()
					return 	country == base.country.RUSSIA or
							country == base.country.UKRAINE or
							country == base.country.BELARUS or
							country == base.country.INSURGENTS or
							country == base.country.ABKHAZIA or
							country == base.country.SOUTH_OSETIA or
							country == base.country.CHINA or
							country == base.country.VIETNAM or 
							country == base.country.USSR or
							country == base.country.YUGOSLAVIA or
							country == base.country.GDR
							--or (country == nations.USA and forcesName == 'NAVY')	-- TODO: Make correct Numeric Callsign for US NAVY
				end
				callsignStr = base.speech.protocols[useprotocol]:makeCallsignString(UnitCommunicator) or "unknown"	
			end
		end
		return callsignStr
	end,
	objectcallsign = function(Locator)
		local callsignStr = nil
		local UnitCallsign = nil
		if Locator ~= nil then
			UnitCallsign = Locator:getCallsign()
		end
		if UnitCallsign then
			callsignStr = base.tostring(UnitCallsign)		
		else 
			callsignStr = "unknown"
		end
		return callsignStr
	end,
	id = function(Locator)
		local ID = nil	
		if Locator ~= nil then
			ID = Locator.id_	
		end	
		return ID	
	end,
	modulation = function(Locator)
		local UnitCommunicator = nil
		local Modulation = nil
		local Modulationstr = "XX"
		if Locator ~= nil then
			Modulation  = Locator:getCommunicator():getModulation()
		end
		if Modulation == base.Communicator.MODULATION_AM then Modulationstr = "AM" end
		if Modulation == base.Communicator.MODULATION_FM then Modulationstr = "FM" end
		return Modulationstr
	end,
	frequency = function(Locator)
		local UnitCommunicator
		local Frequency = "0"
		if Locator ~= nil then
			UnitCommunicator = Locator:getCommunicator()
		end
		if UnitCommunicator then
			Frequency = UnitCommunicator:getFrequency() or "0"		
		else 
			Frequency = "0"
		end
		return Frequency
	end,
	altfreq = function(Locator)
		local UnitCommunicator = nil
		local FreqTbl = {}
		local counter = 0
		if Locator ~= nil then
			UnitCommunicator = Locator:getCommunicator()
		end
		if UnitCommunicator then
			counter = UnitCommunicator:countTransivers()
		end
		for i = 0, counter-1 do
			FreqTbl[i] = UnitCommunicator:getFrequency(i) or nil 	
		end
		return FreqTbl
	end,
	freqmods = function(Locator)
		local UnitCommunicator = nil
		local FreqTbl = {}
		local counter = 0
		if Locator ~= nil then
			UnitCommunicator = Locator:getCommunicator()
		end
		if UnitCommunicator then
			FreqTbl = UnitCommunicator:getFrequenciesModulations()
		end
		return FreqTbl
	end,
	human = function(Locator)
		return Locator.getPlayerName and Locator:getPlayerName() and true or false		
	end,
	playerid = function(Locator)
		return Locator.getPlayerName and Locator:getPlayerName() or "" 	
	end,
	commstatus = function(Locator)
		local State = nil
		local UnitCommunicator =nil
		local Statestring = "unknown"
		if Locator ~= nil then
			UnitCommunicator = Locator:getCommunicator()
		end
		if UnitCommunicator then
			State = getRecepientState(UnitCommunicator)
		else
			State = RecepientState.VOID 
		end
		if State == RecepientState.VOID 				then Statestring = "n/a" 			end
		if State == RecepientState.TUNED 				then Statestring = "TUNED" 			end
		if State == RecepientState.CAN_BE_TUNED 		then Statestring = "can be tuned" 	end
		if State == RecepientState.CANNOT_BE_TUNED 		then Statestring = "not tuned" 		end	
		return Statestring
	end,
	coalition = function(Locator)
		local returnstr ="unknown"
		local Coalition = nil
		if Locator ~= nil then
		Coalition = Locator:getCoalition()
		end
		if Coalition == base.coalition.side.NEUTRAL then returnstr = "NEUTRAL" end
		if Coalition == base.coalition.side.BLUE then returnstr = "BLUE" end
		if Coalition == base.coalition.side.RED then returnstr = "RED" end 
		return returnstr
	end,
	hasradio = function(Locator)
		local result = false
		if Locator:getCommunicator() then
			if Locator:getCommunicator():hasTransiver() then
			result = true
			end
		end
		return result
	end,
	isplayerunit = function(Locator)
	local playerunitID = data.pUnit.id_
	local locatorID = Locator.id_
	return locatorID == playerunitID
	end,
	refuelable = function(Locator)
		return Locator:getDesc().attributes.Refuelable
	end,
}
base.vaicom.helper = {	
	sortby ={
			index = function (l, r)
				return false
			end,
			distance = function (l, r)				
				local lcomp  = base.vaicom.properties.range(l)
				local rcomp  = base.vaicom.properties.range(r)
				return lcomp < rcomp
			end,
			},
	tablelength = function(inputlist)
		local count = 0
		if inputlist ~= nil and base.type(inputlist) == 'table'  then
				for _ in base.pairs(inputlist) do count = count + 1 end	
		end
		return count
	end,
	mergetables = function(A,B)
		local mergetable = {}		 
		if A ~= {} and base.type(A) == 'table' and #A then
			for n,k in base.pairs(A) do
				base.table.insert(mergetable,k)
			end
		end
		if B ~= {} and base.type(B) == 'table' and #B then
			for n,k in base.pairs(B) do
				base.table.insert(mergetable,k)
			end
		end
		return mergetable
	end,
	messagekey = function(id)
		for a,b in base.pairs(base.Message) do
		  if b == id then
			return a
		  end
		end
	end,
	
}
base.vaicom.filter = {
	hasradio = function(Units)
		local Collection = {}
		if base.vaicom.helper.tablelength(Units) > 0 then
			for i, unit in base.pairs(Units) do
				local communicator = unit:getCommunicator()
				if communicator ~= nil and communicator:hasTransiver() then
					base.table.insert(Collection, unit)
				end	
			end
		end					
		return Collection
	end,
	isAirfield = function(Units)
		local Collection = {}
		if base.vaicom.helper.tablelength(Units) > 0 then
			for i, unit in base.pairs(Units) do
				if unit:getDesc().category == 0 then
					base.table.insert(Collection, unit)
				end	
			end	
		end
		return Collection
	end,	
	isFarp = function(Units)
		local Collection = {}
		if base.vaicom.helper.tablelength(Units) > 0 then
			for i, unit in base.pairs(Units) do
				if unit:getDesc().category == 1 then
					base.table.insert(Collection, unit)
				end	
			end	
		end
		return Collection
	end,			
	isShip = function(Units)
		local Collection = {}
		if base.vaicom.helper.tablelength(Units) > 0 then
			for i, unit in base.pairs(Units) do
				if unit:getDesc().attributes.Ships or unit:getDesc().category and unit:getDesc().category == 2 then
					base.table.insert(Collection, unit)
				end	
			end	
		end
		return Collection
	end,
	isHuman = function(Units)
		local Collection = {}
		if base.vaicom.helper.tablelength(Units) > 0 then
			for i, unit in base.pairs(Units) do
				if unit.getPlayerName and unit:getPlayerName() then
					base.table.insert(Collection, unit)
				end	
			end	
		end
		return Collection
	end,
	}
base.vaicom.objects = {
	localRadios = function() 
		local Collection = {}
		if data.communicators ~= {} and base.vaicom.helper.tablelength(data.communicators) > 0 then
		Collection = data.communicators
		else
		Collection = {}
		end
		return Collection
	end,
	localPlayers = function()
		local Collection = {}
		base.table.insert(Collection, data.pUnit)
		return Collection
	end,
	localWingmen = function()
		local Collection = {}
		for i =1,4 do
			local wingman = data.pUnit and data.pUnit:getGroup():getUnit(i)
			if wingman then 
			base.table.insert(Collection, wingman)
			end
		end
		return Collection
	end,	
	localJTACs = function(getside)
		local Collection = {}	
			Collection = base.coalition.getServiceProviders(getside, base.coalition.service.FAC)
		return Collection
	end,	
	localATCs = function(getside)
		local Collection = {}
			Collection = base.coalition.getServiceProviders(getside, base.coalition.service.ATC)
		return Collection	
	end,
	localAWACSs = function(getside)
		local Collection = {}
			Collection = base.coalition.getServiceProviders(getside, base.coalition.service.AWACS)
		return Collection
	end,	
	localTankers = function(getside)
		local Collection = {}
			Collection = base.coalition.getServiceProviders(getside, base.coalition.service.TANKER)
		return Collection
	end,	
	localCrew = function(getside)
		local Collection = {}
		return Collection
	end,
	localAux = function(getside)
		local Collection = {}
		return Collection
	end,	
	localCargo = function(getside)
		local Collection = {}
		return Collection
	end,
	localMoose = function(getside) -- Add Moose
		local Collection = {}
		return Collection
	end,
	localAllies = function(getside)
		local Collection = {}
			Collection = base.coalition.getPlayers and base.coalition.getPlayers(getside)
		return Collection
	end,	
}
base.vaicom.list = {
	localRadios = function()
		local Listing = {}
		Listing = base.vaicom.helper.mergetables(Listing, base.vaicom.objects.localRadios())
		return Listing
	end,		
	localPlayers = function()
		local Listing = {}
		Listing = base.vaicom.helper.mergetables(Listing, base.vaicom.objects.localPlayers())
		return Listing
	end,
	localWingmen = function(selectstr)											
		local Listing = {}
			Listing = base.vaicom.helper.mergetables(Listing, base.vaicom.objects.localWingmen())
		if not selectstr or selectstr == "radio" then
			Listing = base.vaicom.filter.hasradio(Listing)
		end
		return Listing
	end,
	localJTACs = function(selectstr)
		local Listing = {}
		local coalition = data.pUnit and data.pUnit:getCoalition()
		if coalition then
			Listing = base.vaicom.helper.mergetables(Listing, base.vaicom.objects.localJTACs(coalition))
		end
		if not selectstr or selectstr =="radio" then
			Listing = base.vaicom.filter.hasradio(Listing)
		end
		return Listing
	end,	
	localATCs = function(selectstr)											
		local Listing = {}
		local coalition = data.pUnit and data.pUnit:getCoalition()			
		if coalition then
			Listing = base.vaicom.helper.mergetables(Listing, base.vaicom.objects.localATCs(base.coalition.side.NEUTRAL))
			Listing = base.vaicom.helper.mergetables(Listing, base.vaicom.objects.localATCs(coalition))
		end
		if not selectstr or selectstr == "radio" then
			Listing = base.vaicom.filter.hasradio(Listing)
		end
		return Listing
	end,
	localAWACSs = function(selectstr)
		local Listing = {}
		local coalition = data.pUnit and data.pUnit:getCoalition()
		if coalition then
			Listing = base.vaicom.helper.mergetables(Listing, base.vaicom.objects.localAWACSs(coalition))
		end
		if not selectstr or selectstr == "radio" then
			Listing = base.vaicom.filter.hasradio(Listing)
		end
		return Listing
	end,	
	localTankers = function(selectstr)
		local Listing = {}
		local coalition = data.pUnit and data.pUnit:getCoalition()
		if coalition then
			Listing = base.vaicom.helper.mergetables(Listing, base.vaicom.objects.localTankers(coalition))
		end
		if not selectstr or selectstr == "radio" then
			Listing = base.vaicom.filter.hasradio(Listing)
		end
		return Listing
	end,
	localCrew = function(selectstr)
		local Listing = {}
		return Listing
	end,	
	localAux = function(selectstr)
		local Listing = {}
		return Listing
	end,	
	localCargo = function(selectstr)
		local Listing = {}
		local coalition = data.pUnit and data.pUnit:getCoalition()
		if coalition then
			Listing = base.vaicom.helper.mergetables(Listing, base.vaicom.objects.localCargo(coalition))
		end
		return Listing
	end,
	localMoose = function(selectstr) -- Add Moose
		local Listing = {}
		return Listing
	end,
	localAllies = function(selectstr)
		local Listing = {}
		local coalition = data.pUnit and data.pUnit:getCoalition()
		if coalition then
			Listing = base.vaicom.helper.mergetables(Listing, base.vaicom.objects.localAllies(coalition))
		end
		return Listing
	end,
}
base.vaicom.get = { 
	serverdata  ={	
		dcsversion = function()
			local fullversionstring = base.tostring(base._ED_VERSION)
			local versionnumber = base.string.sub(fullversionstring,5,9) or "X.X"
			return versionnumber
		end,				
				}, 		
	missiondata ={	
		listby ={					
				Radio 	= function(sortfunction)
					local Stack = base.vaicom.list.localRadios()
					if Stack ~=nil and #Stack > 1 then base.table.sort(Stack, sortfunction) end 								
					return Stack
				end,					
				Player 	= function(sortfunction)
					local Stack = base.vaicom.list.localPlayers()
					if Stack ~=nil and #Stack > 1 then base.table.sort(Stack, sortfunction) end 			
					return Stack
				end,
				Flight 	= function(sortfunction, radio)
					local Stack = base.vaicom.list.localWingmen(radio)				
					if Stack ~=nil and #Stack > 1 then base.table.sort(Stack, sortfunction) end 								
					return Stack
				end,		
				JTAC 	= function(sortfunction, radio)
					local Stack = base.vaicom.list.localJTACs(radio)		
					if Stack ~=nil and #Stack > 1 then base.table.sort(Stack, sortfunction) end 	
					return Stack
				end,									
				ATC 	= function(sortfunction, radio)
					local Stack = base.vaicom.list.localATCs(radio)		
					if Stack ~=nil and #Stack > 1 then base.table.sort(Stack, sortfunction) end 
					return Stack
				end,									
				AWACS 	= function(sortfunction, radio)
					local Stack = base.vaicom.list.localAWACSs(radio)		
					if Stack ~=nil and #Stack > 1 then base.table.sort(Stack, sortfunction) end 
					return Stack
				end,
				Tanker 	= function(sortfunction, radio)
					local Stack = base.vaicom.list.localTankers(radio)		
					if Stack ~=nil and #Stack > 1 then base.table.sort(Stack, sortfunction) end 
					return Stack							
				end,
				Crew 	= function(sortfunction, radio)
					local Stack = base.vaicom.list.localCrew(radio)
					return Stack							
				end,
				Aux 	= function(sortfunction, radio)
					local Stack = base.vaicom.list.localAux(radio)
					return Stack	
				end,
				Cargo 	= function(sortfunction, radio)
					local Stack = base.vaicom.list.localCargo(radio)
					return Stack	
				end,
				Moose 	= function(sortfunction, radio) -- Add Moose
					local Stack = base.vaicom.list.localMoose(radio)
					return Stack	
				end,
				Allies  = function(sortfunction, radio)
					local Stack = base.vaicom.list.localAllies(radio)
					return Stack	
				end,
				},
				markers = function()
					local count = 0
					local Stack = base.world.getMarkPanels()
					if #Stack > 0 then
						for i= 1,#Stack do
							count = count + 1
						end
					end
					return count
				end,				
			  },
}
base.vaicom.set = {
	VoIP 		= function(setmode)
		setVoIP(setmode)
	end,
	easycomms 	= function(setmode)
		setEasyComm(setmode)
	end,		
	pause 		= function(setmode)
		base.DCS.setPause(setmode)
	end,
	debugmode 	= function(setmode)
		if setmode ~= base.vaicom.state.debugmode then
			base.vaicom.state.debugmode = setmode
			dcsoptions.setOption("plugins.VAICOM.VAICOMDebugModeEnabled", setmode)
		end
	end,	
}
base.vaicom.state = {
		debugmode 				= false,
		dcsversion 				= base.vaicom.get.serverdata.dcsversion(),
		root 					= base.tostring(base.lfs.writedir()),
		currentdir 				= base.tostring(base.lfs.currentdir()),
		easycomms				= data.radioAutoTune or base.DCS.getMissionOptions().difficulty.easyCommunication or true,
		riostate				= {},
		options					= {},
		currentspeech			= {},
		pause 					= false, 
		theatre					= "",
		multiplayer				= false,
		vrmode					= false,
		menuhold				= false,
		dcsid					= false,
		dcsmodulecat			= false,
		airborne				= false, 
		timer					= 0,
		playerunit				= data.pUnit,
		payload					= {},
		bpos					= {},
		cpos					= {},
		playercoalition			= base.coalition.side.NEUTRAL,			
		rawcommand 				= base.vaicom.flags.raw,
		menuaux					= {}, 
		menucargo				= {},
		menumoose				= {}, -- Add Moose
		activemessage			= {},
		availableradios			= {},
		messagesent				= false,
		availablerecipients 	=   {
									Player 			= {}, 
									Flight 			= {}, 
									JTAC			= {}, 
									ATC				= {},
									AWACS			= {}, 
									Tanker			= {},
									Crew			= {},
									Aux				= {},
									Moose		    = {}, -- Add moose
									Cargo			= {},
									Allies			= {},									
									},								
		availabilitycounter = 		{		
									Player 			= 0, 
									Flight 			= 0, 
									JTAC			= 0, 
									ATC				= 0, 
									AWACS			= 0, 
									Tanker			= 0,
									Crew			= 0,
									Aux				= 0,
									Moose			= 0, -- Add Moose
									Cargo			= 0,
									Allies			= 0,									
									},
		selectedrecipients = 		{												
									[base.vaicom.categories.recipient.Player] 	= nil,
									[base.vaicom.categories.recipient.Flight] 	= nil,
									[base.vaicom.categories.recipient.JTAC] 	= nil,
									[base.vaicom.categories.recipient.ATC] 		= nil,									
									[base.vaicom.categories.recipient.AWACS] 	= nil,
									[base.vaicom.categories.recipient.Tanker] 	= nil,
									[base.vaicom.categories.recipient.Crew] 	= nil,
									[base.vaicom.categories.recipient.Aux] 		= nil,
									[base.vaicom.categories.recipient.Moose] 	= nil, -- Add Moose
									[base.vaicom.categories.recipient.Cargo] 	= nil,
									[base.vaicom.categories.recipient.Allies] 	= nil,										
									},																	
		update =					{		
			all = function()
				base.vaicom.state.timer								= data.initialized and base.Export.LoGetModelTime()
				base.vaicom.state.tod								= data.initialized and base.Export.LoGetMissionStartTime()
				base.vaicom.state.playerunit 						= data.initialized and data.pUnit
				base.vaicom.state.payload 							= data.initialized and base.Export.LoGetPayloadInfo()
				base.vaicom.state.bpos								= data.initialized and base.Export.LoGetSelfData() and base.Export.LoGetSelfData().Position or nil
				base.vaicom.state.cpos.type							= data.initialized and base.view.getCamType()
				base.vaicom.state.cpos.loc							= data.initialized and base.view.getCamPoint()
				base.vaicom.state.playercoalition 					= data.pUnit and base.DCS.getPlayerCoalition() or base.coalition.side.NEUTRAL	
				base.vaicom.state.riostate.canopy					= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.Export.LoGetMechInfo().canopy and (base.Export.LoGetMechInfo().canopy.value >0)) or false
				base.vaicom.state.riostate.rdr						= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(2012) >0)) or false
				base.vaicom.state.riostate.pdstt					= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(11503) >0)) or false
				base.vaicom.state.riostate.pstt						= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(11504) >0)) or false
				base.vaicom.state.riostate.amt						= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(2022) == 0)) or false
				base.vaicom.state.riostate.tcn						= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(374))) or 0
				base.vaicom.state.riostate.ics						= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(2044) > -1)) or false
				base.vaicom.state.riostate.sngl						= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(60) >0)) or false
				base.vaicom.state.riostate.jmr						= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(151) ==1)) or false
				base.vaicom.state.riostate.AM182					= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(359) ==1)) or false
				base.vaicom.state.riostate.ejsn						= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(2049) ==1)) or false
				base.vaicom.state.riostate.markers					= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.vaicom.get.missiondata.markers()) or 0
				base.vaicom.state.availablerecipients.Player 		= data.initialized and base.vaicom.get.missiondata.listby.Player(base.vaicom.helper.sortby.index)
				base.vaicom.state.availablerecipients.Flight 		= data.initialized and base.vaicom.get.missiondata.listby.Flight(base.vaicom.helper.sortby.index,	"radio")					
				base.vaicom.state.availablerecipients.JTAC			= data.initialized and base.vaicom.get.missiondata.listby.JTAC(base.vaicom.helper.sortby.distance,	"radio")
				base.vaicom.state.availablerecipients.ATC			= data.initialized and base.vaicom.get.missiondata.listby.ATC(base.vaicom.helper.sortby.distance, 	"radio")
				base.vaicom.state.availablerecipients.AWACS			= data.initialized and base.vaicom.get.missiondata.listby.AWACS(base.vaicom.helper.sortby.distance, "radio")
				base.vaicom.state.availablerecipients.Tanker		= data.initialized and base.vaicom.get.missiondata.listby.Tanker(base.vaicom.helper.sortby.distance,"radio") 
				base.vaicom.state.availablerecipients.Crew			= data.initialized and base.vaicom.get.missiondata.listby.Crew(base.vaicom.helper.sortby.distance, 	"radio") 
				base.vaicom.state.availablerecipients.Aux			= data.initialized and base.vaicom.get.missiondata.listby.Aux(base.vaicom.helper.sortby.distance, 	"radio")
				base.vaicom.state.availablerecipients.Moose			= data.initialized and base.vaicom.get.missiondata.listby.Moose(base.vaicom.helper.sortby.distance, "radio") -- Add moose
				base.vaicom.state.availablerecipients.Cargo			= data.initialized and base.vaicom.get.missiondata.listby.Cargo(base.vaicom.helper.sortby.distance, "radio")
				base.vaicom.state.availablerecipients.Allies		= data.initialized and base.vaicom.get.missiondata.listby.Allies(base.vaicom.helper.sortby.distance, "radio")				
				for recipientclass,_ in base.pairs(base.vaicom.state.availablerecipients) do
					base.vaicom.state.availabilitycounter[recipientclass] = base.vaicom.helper.tablelength(base.vaicom.state.availablerecipients[recipientclass])
				end
				base.vaicom.state.menuaux							= data.initialized and data.menuOther
				base.vaicom.state.menumoose							= data.initialized and data.menuOther -- Add Moose is this required?
				base.vaicom.state.menucargo							= data.initialized and data.menuEmbarkToTransport
				base.vaicom.state.dcsversion						= data.initialized and base.vaicom.get.serverdata.dcsversion()
				base.vaicom.state.easycomms							= data.initialized and data.radioAutoTune
				base.vaicom.state.options							= {}
				base.vaicom.state.options.plugins					= data.initialized and dcsoptions.getOption("plugins") or {}
				base.vaicom.state.options.sound						= data.initialized and dcsoptions.getOption("sound") or {}
				base.vaicom.state.pause								= data.initialized and base.DCS.getPause() or false
				base.vaicom.state.theatre							= data.initialized and base.env.mission.theatre or ""
				base.vaicom.state.sortie							= data.initialized and base.env.mission.start_time or ""
				base.vaicom.state.task								= data.initialized and base.DebriefingMissionData.getPlayerUnitInfo().task or ""
				base.vaicom.state.country							= data.initialized and base.DebriefingMissionData.getPlayerUnitInfo().country or ""
				base.vaicom.state.multiplayer						= data.initialized and base.DCS.isMultiplayer() or false
				base.vaicom.state.vrmode 							= data.initialized and base.DCS.HMD_isActive() or false
				base.vaicom.state.dcsid 							= data.initialized and base.DCS.getPlayerUnitType()
				base.vaicom.state.dcsmodulecat						= data.initialized and data.pUnit and data.pUnit:getDesc().attributes and data.pUnit:getDesc().attributes.Helicopters and 'Helicopters' or 'Planes'
				base.vaicom.state.airborne							= data.initialized and data.pUnit and data.pUnit:inAir()		
			end,
									},								
			sendupdateall = function()
				local chunk = {}	
				chunk[1] 		= {
									dcsversion			= base.vaicom.state.dcsversion,
									root				= base.vaicom.state.root,
									currentdir			= base.vaicom.state.currentdir,
									multiplayer			= base.vaicom.state.multiplayer,
									vrmode				= base.vaicom.state.vrmode,						
									easycomms			= base.vaicom.state.easycomms,
									pausebasestate		= base.vaicom.state.pause,	
									theatre				= base.vaicom.state.theatre,
									sortie				= base.vaicom.state.sortie,
									task				= base.vaicom.state.task,
									country 			= base.vaicom.state.country,
									options				= base.vaicom.state.options,	
								  }				
				chunk[2] 		= {	
									timer				= base.vaicom.state.timer,	
									tod					= base.vaicom.state.tod,
									id					= base.vaicom.state.dcsid,
									playerusername  	= base.Export.LoGetPilotName(),
									playercallsign 		= data.pUnit and data.pUnit:getCallsign() or "",
									playercoalition 	= base.vaicom.state.playercoalition,
									playerunitid		= data.pUnit and data.pUnit.id_ or 0,
									playerunitcat		= base.vaicom.state.dcsmodulecat,
									airborne			= base.vaicom.state.airborne,								
									intercom			= data.intercomId,
									fsmstate 			= base.tostring(base.fsm.state),
									radios				= {},
								  }
				chunk[3] 		= {		
									missiontitle		= base.DCS.getMissionName(),
									missionbriefing		= base.DCS.getPlayerBriefing().descText,
									missiondetails		= base.DCS.getPlayerBriefing().mission_goal,	
								  }
				chunk[4] 		= {
									availablerecipients =   {						
																Player 		= {},
																Flight 		= {},
																JTAC 		= {},
																AWACS		= {},
																Tanker		= {},
																Crew		= {},
																Aux			= {},		
																Cargo		= {},
																Moose		= {}, -- Add Moose
															}		
								  }
				chunk[5] 		= {								
									availablerecipients =   {						
																ATC			= {},
															}																									
								  }		
				chunk[6] 		= {								
									availablerecipients =   {						
																ATC			= {},
															}																									
								  }
				chunk[7] 		= {	
									availablerecipients =   {						
																Allies		= {},
															}				
								  }							  
				chunk[8] 		= {	
									availablerecipients =   {						
																Allies		= {},
															}				
								  }								  
				chunk[9] 		= {
									menuaux		= (base.vaicom.state.activemessage.importmenus and base.vaicom.state.menuaux) 	or nil,
									menumoose	= (base.vaicom.state.activemessage.importmenus and base.vaicom.state.menumoose) or nil, -- Add Moose
									menucargo	= (base.vaicom.state.activemessage.importmenus and base.vaicom.state.menucargo) or nil,		
								  }						
				chunk[10] 		= {
									riostate = base.vaicom.state.riostate or nil,
									bpos	 = base.vaicom.state.bpos or nil,
									cpos	 = base.vaicom.state.cpos or nil,
								  }
				chunk[11] 		= {
									payload	 = base.vaicom.state.payload or nil,
								  }
				chunk[12] 		= {
								  }
				for n,k in base.pairs(data.communicators) do
					local Viper_VHF = (base.vaicom.state.dcsid == "F-16C_50" and n == 38) 
					local ICS = (n == data.intercomId)
					local ICS_linked = (base.GetDevice(data.intercomId) and base.GetDevice(data.intercomId):is_communicator_available(n))
					local ICS_set = (Viper_VHF or ICS_linked) 
					local radio =  	{
									deviceid = n,
									displayName = k.displayName,
									AM = k.AM,
									FM = k.FM,
									isavailable = ICS_set,  
									intercom = ICS,
									on =  ICS or ((ICS_set and (( base.GetDevice(n) and base.GetDevice(n).is_on and base.GetDevice(n):is_on() ))) or false),
									frequency = ( ICS_set and (( (not ICS) and base.GetDevice(n) and base.GetDevice(n).get_frequency and base.GetDevice(n):get_frequency() ) or 0)) or 0,
									modulation = ( ICS_set and (( (not ICS) and base.GetDevice(n) and base.GetDevice(n).get_modulation and (((base.GetDevice(n):get_modulation() == 1) and "FM") or "AM") ) or "XX")) or "XX", 
									}						
					base.table.insert(chunk[2].radios, radio)
				end					
				for recipientclass,_ in base.pairs(base.vaicom.state.availablerecipients) do
					for n,k in base.pairs(base.vaicom.state.availablerecipients[recipientclass]) do
						local dcsunit = {
										index = n,
										id_ = base.vaicom.properties.id(k),
										callsign = base.tostring(base.vaicom.properties.missioncallsign(k)),
										range = base.vaicom.properties.range(k),
										pos = base.vaicom.properties.pos(k),
										fullname = base.tostring(base.vaicom.properties.displayname(k)),
										coalition = base.tostring(base.vaicom.properties.coalition(k)),
										altfreq = base.vaicom.properties.altfreq(k),
										freq = base.tostring(base.vaicom.properties.frequency(k)),
										mod = base.tostring(base.vaicom.properties.modulation(k)),
										ishuman = base.vaicom.properties.human(k),
										playerid = base.vaicom.properties.playerid(k),
										}	
						local tbl 
						if recipientclass == "ATC" then
							tbl = ((n < 25) and 5) or ((n < 50) and 6)
						else
							if recipientclass == "Allies" then
								tbl = ((n < 25) and 7) or ((n < 50) and 8)
							else
								tbl = 4
							end
						end
						if tbl then
							base.table.insert(chunk[tbl].availablerecipients[recipientclass], dcsunit)
						end		
								
					end
				end
				for i= 1,11 do
					local sndtbl = chunk[i]
					sndtbl.cid 				= i									
					sndtbl.client 			= "VAICOMPRO"
					sndtbl.mode 			= "normal"
					sndtbl.type 			= "missiondata.update"
					socket.try(base.vaicom.sender:send(JSON:encode(sndtbl)))
				end	
			end,								
}
base.vaicom.init = {
	start = function(self)	 
		Gui.SetupApplicationUpdateCallback()
		Gui.EnableHighSpeedUpdate(true)
		Gui.AddUpdateCallback(vaicom_loop)
		base.vaicom.sender = socket.try(socket.udp()) 
		socket.try(base.vaicom.sender:setpeername(base.vaicom.config.sendaddress,base.vaicom.config.sendport))
		socket.try(base.vaicom.sender:settimeout(base.vaicom.config.sendtimeout))
		base.vaicom.receiver = socket.try(socket.udp()) 
		socket.try(base.vaicom.receiver:setsockname(base.vaicom.config.receiveaddress,base.vaicom.config.receiveport))
		socket.try(base.vaicom.receiver:settimeout(base.vaicom.config.receivetimeout))	
		base.vaicom.relay = socket.try(socket.udp()) 
		socket.try(base.vaicom.relay:setpeername(base.vaicom.config.relayaddress,base.vaicom.config.relayport))
		socket.try(base.vaicom.relay:settimeout(base.vaicom.config.relaytimeout))
end,
	stop = function(self)
		if base.vaicom.sender then
			socket.try(base.vaicom.sender:close())
			base.vaicom.sender = nil
		end
		if base.vaicom.receiver then
			socket.try(base.vaicom.receiver:close())
			base.vaicom.receiver = nil
		end
		if base.vaicom.relay then
			socket.try(base.vaicom.relay:close())
			base.vaicom.relay = nil
		end
	end,
}
