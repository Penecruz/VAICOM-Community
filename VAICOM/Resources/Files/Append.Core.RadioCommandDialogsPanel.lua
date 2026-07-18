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

			local function callMethod(obj, methodName)
				if obj == nil then
					return nil
				end
				local okCall, result = base.pcall(function()
					if obj[methodName] then
						return obj[methodName](obj)
					end
					return nil
				end)
				if okCall and result ~= nil then
					return result
				end
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

						-- Manually change the channel selector in the cockpit for some modules
						-- so that it correctly reflects the channel for the freqeuncy we are tuned to.
						local dcsId = base.vaicom.state.dcsid
						if dcsId == "Mi-24P" then
							-- Clickable Ids for the channel selectors were obtained from DCS-BIOS
							if communicator.displayName == "R-863" then
								manuallySetChannel(commDevice, 3007, channelNum, 20)
							elseif communicator.displayName == "R-828" then
								manuallySetChannel(commDevice, 3001, channelNum, 10)
							end
						end
					end
				else
					if commDevice:is_frequency_in_range(freqMod.frequency) then
						local dcsId = base.vaicom.state.dcsid
						-- The UH-1H needs to have the UHF radio manually tuned (unlike it's VHF radios).
						-- The normal set_frequency function does tune it, but it then reverts back
						-- to the previous frequency.
						if dcsId == "UH-1H" and communicator.displayName == "CB UHF" then
							local currentFreq = base.tostring(commDevice:get_frequency())
							local newFreq = base.tostring(freqMod.frequency)
							-- Tune each individual radio knob based on the position of the digits in the frequency
							-- associated with that knob. The clickableId is the knob to be turned and the increment
							-- is the number it goes up in.
							manuallyTuneFrequency(commDevice, 3002, 1, base.tonumber(base.string.sub(currentFreq, 1, 2)), base.tonumber(base.string.sub(newFreq, 1, 2)))
							manuallyTuneFrequency(commDevice, 3003, 1, base.tonumber(base.string.sub(currentFreq, 3, 3)), base.tonumber(base.string.sub(newFreq, 3, 3)))
							manuallyTuneFrequency(commDevice, 3004, 5, base.tonumber(base.string.sub(currentFreq, 4, 5)), base.tonumber(base.string.sub(newFreq, 4, 5)))
						else
							commDevice:set_frequency(freqMod.frequency)
						end
						haveFreq = true
					end
				end
				if haveFreq then
					if communicator.AM and communicator.FM then --try only setting modulation for radios that have both AM and FM.
						commDevice:set_modulation(freqMod.modulation)
					end
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
function manuallySetChannel(commDevice, clickableId, channelNum, positions)
	commDevice:performClickableAction(clickableId, channelNum * (1 / positions))
end
function manuallyTuneFrequency(commDevice, clickableId, increment, currentValue, newValue)
	local difference = currentValue - newValue
	-- 0.0 goes up and 1.0 goes down
	local direction = difference > 0 and 1.0 or 0.0
	local startTime = base.timer.getTime()
	local delay = 0.2
	-- Loop through each knob turn with a delay for each. The increment is due to
	-- some knobs going up/down by 1 and others by 5.
	for i = 1, (base.math.abs(difference) / increment) do
		-- DCS requires a delay between each knob turn to ensure they are processed
		local scheduledTime = startTime + (i * delay)
		base.timer.scheduleFunction(turnRadioKnob, { commDevice = commDevice, clickableId = clickableId, direction = direction }, scheduledTime)
	end
end
function turnRadioKnob(args)
	local commDevice = args.commDevice
	commDevice:performClickableAction(args.clickableId, args.direction)
	
	-- nil means don't reschedule
	return nil 
end
-- Thanks to the amazing DCS SRS folk for their logic and permission
-- to use parts of their codebase for the below functions to get the
-- currently selected radio in modules that use a radio selector.
function getListIndicatorValue(indicatorId)
    local listIindicator = base.list_indication(indicatorId)
    local result = {}

    if listIindicator == "" then
        return nil
    end

    local listindicatorMatch = listIindicator:gmatch("-----------------------------------------\n([^\n]+)\n([^\n]*)\n")
    while true do
        local key, value = listindicatorMatch()
        if not key then
            break
        end
        result[key] = value
    end

	return result
end
function getSelectorPosition(arg, step)
    local value = base.GetDevice(0):get_argument_value(arg)
    if value ~= nil then
        return base.math.abs(base.tonumber(base.string.format("%.0f", (value) / step)))
    end

    return nil
end
function nearlyEqual(a, b, diff)
	return base.math.abs(a - b) < diff
end
local function normalizeRadioName(name)
    return base.string.lower(name):gsub("[^%w]", "")
end
local function findRadioDisplayName(...)
    if data.communicators == nil then
        return nil
    end
    local lookup = {}
    for _, name in base.pairs({ ... }) do
        lookup[normalizeRadioName(name)] = true
    end
    for _, communicator in base.pairs(data.communicators) do
        if communicator.displayName ~= nil then
            local normalizedName = normalizeRadioName(communicator.displayName)
            if lookup[normalizedName] then
                return communicator.displayName
            end
        end
    end
    return nil
end
function getSelectedRadio(dcsId)
	base.print("dcsId: "..dcsId) -- print the dcsId for debugging
	local selectedRadio = ""
	if dcsId == "AH-64D_BLK_II" then
		-- get pilot or CP/G
		local seat = base.get_param_handle("SEAT"):get()
		if seat ~= nil then
			local eufdDevice = nil
			if seat == 0 then
				eufdDevice = getListIndicatorValue(18)
			else
				eufdDevice = getListIndicatorValue(19)
			end
			if eufdDevice ~= nil then
				-- get selected radio
				if eufdDevice['Rts_VHF_'] == '<' then
					selectedRadio = "VHF AM"
				elseif eufdDevice['Rts_UHF_'] == '<' then
					selectedRadio = "CB UHF"
				elseif eufdDevice['Rts_FM1_'] == '<' then
					selectedRadio = "FM1: ARC-201D"
				elseif eufdDevice['Rts_FM2_'] == '<' then
					selectedRadio = "FM2: ARC-201D"
				elseif eufdDevice['Rts_HF_'] == '<' then
					selectedRadio = "HF"
				end
			end
		end
	elseif dcsId == "CH-47Fbl1" then
		-- pilot (seat 0) has offset 591 for the selector, copilot (seat 1) has offset 624
		local selectorOffset = 591
		local selectorPosition = getSelectorPosition(selectorOffset + 22, 0.05)
		if selectorPosition ~= nil then
			if selectorPosition == 1 then
				--return "FM1: ARC-201D"  (AN/ARC-201 FM1) -- not currently implemented
			end
			if selectorPosition == 2 then
				return "CB UHF"
			end
			if selectorPosition == 3 then
				return "VHF ARC-186"
			end
			if selectorPosition == 4 then
				--return "HF"   (AN/ARC-220 HF) -- not currently implemented
			end
			if selectorPosition == 5 then
				--return "AN/ARC-201 FM2" -- not currently implemented
			end
			if selectorPosition == 9 then
				-- backup radio is in use
				local switchPosition = base.GetDevice(0):get_argument_value(1466)
				if switchPosition < 0.5 then
					return selectorOffset == 591 and "VHF ARC-186" or "CB UHF"
				else
					return selectorOffset == 591 and "CB UHF" or "VHF ARC-186"
				end
			end
		end
	elseif dcsId == "C-130J-30" then
		local seat = base.get_param_handle("SEAT"):get()
		local selectorId = nil
		if seat == 0 then
			selectorId = 294
		elseif seat == 1 then
			selectorId = 296
		elseif seat == 3 then
			selectorId = 298
		end
		if selectorId ~= nil then
			local selectorValue = base.GetDevice(0):get_argument_value(selectorId)
			if selectorValue ~= nil then
				local selectorPosition = base.math.floor(selectorValue * 9 + 0.5) + 1
				selectorPosition = selectorPosition + 1
				if selectorPosition > 9 then
					selectorPosition = 1
				end
				if selectorPosition == 1 then
					selectedRadio = findRadioDisplayName("INTERCOM", "Interphone", "INT") or selectedRadio
				elseif selectorPosition == 2 then
					selectedRadio = findRadioDisplayName("UHF1", "UHF-1", "UHF 1") or selectedRadio
				elseif selectorPosition == 3 then
					selectedRadio = findRadioDisplayName("UHF2", "UHF-2", "UHF 2") or selectedRadio
				elseif selectorPosition == 4 then
					selectedRadio = findRadioDisplayName("VHF1", "VHF-1", "VHF 1") or selectedRadio
				elseif selectorPosition == 5 then
					selectedRadio = findRadioDisplayName("VHF2", "VHF-2", "VHF 2") or selectedRadio
				elseif selectorPosition == 6 then
					selectedRadio = findRadioDisplayName("HF1", "HF-1", "HF 1") or selectedRadio -- not currently implemented
				elseif selectorPosition == 7 then
					selectedRadio = findRadioDisplayName("HF2", "HF-2", "HF 2") or selectedRadio -- not currently implemented
				elseif selectorPosition == 8 then
					selectedRadio = findRadioDisplayName("VHF AM(ARC-210)", "VHF AM", "ARC-210") or selectedRadio -- not currently implemented
				elseif selectorPosition == 9 then
					selectedRadio = findRadioDisplayName("PVT", "PVT 1", "PVT-1") or selectedRadio
				end
			end
		end
	elseif dcsId == "Mi-24P" then
		-- Pilot: 455, CP/G: 659
		local seat = base.get_param_handle("SEAT"):get()
		local selectorId = nil
		if seat == 0 then
			selectorId = 455
		elseif seat == 1 then
			selectorId = 659
		end
		if selectorId ~= nil then
			local selectorPosition = getSelectorPosition(selectorId, 0.2)
			if selectorPosition ~= nil then
				if selectorPosition == 0 then
					selectedRadio = "R-863"
				elseif selectorPosition == 2 then
					selectedRadio = "R-828"
				elseif selectorPosition == 3 then
					selectedRadio = "Jadro-1A"
				elseif selectorPosition == 4 then
					selectedRadio = "R_852"
				end
			end
		end
	elseif dcsId == "UH-1H" then
		local selectorValue = base.GetDevice(0):get_argument_value(30)
		if selectorValue ~= nil then
			if nearlyEqual(selectorValue, 0.1, 0.03) then
				selectedRadio = findRadioDisplayName("Intercom", "Interphone", "INTERCOM", "INT") or selectedRadio
			elseif nearlyEqual(selectorValue, 0.2, 0.03) then
				selectedRadio = findRadioDisplayName("AN/ARC-131", "ARC-131", "VHF FM") or selectedRadio
			elseif nearlyEqual(selectorValue, 0.3, 0.03) then
				selectedRadio = findRadioDisplayName("AN/ARC-51BX - UHF", "AN/ARC-51BX", "CB UHF", "UHF") or selectedRadio
			elseif nearlyEqual(selectorValue, 0.4, 0.03) then
				selectedRadio = findRadioDisplayName("AN/ARC-134", "ARC-134", "VHF AM") or selectedRadio
			end
		end
 elseif dcsId == "F-100D" then
		selectedRadio = findRadioDisplayName("UHF Radio AN/ARC-34", "Radio AN/ARC-34", "AN/ARC-34") or selectedRadio
	end
	return selectedRadio
end

local function radioNamesMatch(nameA, nameB)
	if nameA == nil or nameB == nil then
		return false
	end

	local normalizedA = normalizeRadioName(nameA)
	local normalizedB = normalizeRadioName(nameB)

	if normalizedA == normalizedB then
		return true
	end

	return base.string.find(normalizedA, normalizedB, 1, true) ~= nil
		or base.string.find(normalizedB, normalizedA, 1, true) ~= nil
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
  return true
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
		local now = base.Export.LoGetModelTime and base.Export.LoGetModelTime() or 0
		local cumulativeDelay = 0
		local function queueActions(actions)
			for i = 1, #actions do
				local action = actions[i]
				base.table.insert(base.vaicom.devicecontrol.queue,
					{
						executeAt = now + cumulativeDelay,
						device = action.device,
						command = action.command,
						value = action.value
					})
				local d = action.delayMs or 0
				if d > 0 then
					cumulativeDelay = cumulativeDelay + (d / 1000)
				end
			end
		end

		queueActions(clientmessage.extsequence)
		queueActions(clientmessage.devsequence)
		base.vaicom.devicecontrol.busy = true
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
	if clientmessage.type == base.vaicom.messagetype.aicomms			then
		local unitcomm, tgtunit = SetTargetComm(clientmessage.command)
		if clientmessage.command == base.Message.wMsgLeaderRequestRearming then
			base.MissionResourcesDialog.onRadioMenuRearm()
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
	if base.vaicom and base.vaicom.devicecontrol and base.vaicom.devicecontrol.busy then
		local now = base.Export.LoGetModelTime and base.Export.LoGetModelTime() or 0
		while #base.vaicom.devicecontrol.queue > 0 and base.vaicom.devicecontrol.queue[1].executeAt <= now do
			local action = base.table.remove(base.vaicom.devicecontrol.queue, 1)
			local dev = base.GetDevice(action.device)
			if dev and dev.performClickableAction then
				dev:performClickableAction(action.command, action.value)
			end
		end
		if #base.vaicom.devicecontrol.queue == 0 then
			base.vaicom.devicecontrol.busy = false
			socket.try(base.vaicom.sender:send(base.vaicom.flags.raw))
		end
	end
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
         UnitCommunicator = Locator:getCommunicator()
		end
		if UnitCommunicator then
			local okMod, mod = base.pcall(function()
				return UnitCommunicator:getModulation()
			end)
			if okMod and mod ~= nil then
				Modulation = mod
			else
				local okCount, count = base.pcall(function()
					return UnitCommunicator:countTransivers()
				end)
				if okCount and count and count > 0 then
					local okMod0, mod0 = base.pcall(function()
						return UnitCommunicator:getModulation(0)
					end)
					if okMod0 and mod0 ~= nil then
						Modulation = mod0
					end
				end
			end
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
            local okFreq, freq = base.pcall(function()
				return UnitCommunicator:getFrequency()
			end)
			if okFreq and freq ~= nil then
				Frequency = freq
			else
				local okCount, count = base.pcall(function()
					return UnitCommunicator:countTransivers()
				end)
				if okCount and count and count > 0 then
					local okFreq0, freq0 = base.pcall(function()
						return UnitCommunicator:getFrequency(0)
					end)
					if okFreq0 and freq0 ~= nil then
						Frequency = freq0
					end
				end
			end
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
            local okCount, count = base.pcall(function()
				return UnitCommunicator:countTransivers()
			end)
			if okCount and count then
				counter = count
			end
		end
		for i = 0, counter-1 do
            local okFreq, freq = base.pcall(function()
				return UnitCommunicator:getFrequency(i)
			end)
			if okFreq and freq ~= nil then
				FreqTbl[i] = freq
			end
		end
		return FreqTbl
	end,
    tacan = function(Locator)
		local tacan = ""
		local okTacan, result = base.pcall(function()
			if Locator == nil then
				return ""
			end

			local function readAnyValue(obj, keys)
				if obj == nil then
					return nil
				end
				for _, key in base.pairs(keys) do
					local okValue, value = base.pcall(function() return obj[key] end)
					if okValue and value ~= nil and base.type(value) ~= "function" then
						return value
					end
					if okValue and base.type(value) == "function" then
						local okCall, callResult = base.pcall(function() return value(obj) end)
						if okCall and callResult ~= nil then
							return callResult
						end
					end
				end
				return nil
			end

			local function normalizeBand(v)
				if v == nil then
					return ""
				end
				local s = base.tostring(v)
				if s == "" then
					return ""
				end
				local u = base.string.upper(s)
				if u == "X" or u == "Y" then
					return u
				end
				local n = base.tonumber(s)
				if n == 0 then return "X" end
				if n == 1 then return "Y" end
				return s
			end

			local function composeTacan(beaconObj)
				if beaconObj == nil then
					return ""
				end

				local channel = readAnyValue(beaconObj, {
					"channel", "Channel", "channelNumber", "tacanChannel", "TACANChannel",
					"getChannel", "get_channel", "getChannelNumber", "getTacanChannel", "getTACANChannel"
				})
              if channel == nil then
					channel = callMethod(beaconObj, "getChannel") or callMethod(beaconObj, "get_channel") or callMethod(beaconObj, "getChannelNumber") or callMethod(beaconObj, "getTacanChannel") or callMethod(beaconObj, "getTACANChannel")
				end
				if channel == nil then
					return ""
				end

				local band = readAnyValue(beaconObj, {
					"modeChannel", "modechannel", "band", "mode", "tacanBand", "TACANBand",
					"getModeChannel", "get_modechannel", "getBand", "getMode", "getTacanBand", "getTACANBand"
				})
				if band == nil then
					band = callMethod(beaconObj, "getModeChannel") or callMethod(beaconObj, "get_modechannel") or callMethod(beaconObj, "getBand") or callMethod(beaconObj, "getMode") or callMethod(beaconObj, "getTacanBand") or callMethod(beaconObj, "getTACANBand")
				end

				local channelnum = base.tonumber(channel)
				local c = channelnum ~= nil and base.tostring(base.math.floor(channelnum + 0.5)) or base.tostring(channel)
				local b = normalizeBand(band)
				return c .. b
			end

			local beacon = nil
			local okBeacon = false
			okBeacon, beacon = base.pcall(function() return Locator:getBeacon() end)
         if not okBeacon then
				beacon = nil
			end
			if (not okBeacon) or beacon == nil then
				local okComm, comm = base.pcall(function() return Locator:getCommunicator() end)
				if okComm and comm ~= nil then
					local okCommBeacon, commBeacon = base.pcall(function() return comm:getBeacon() end)
                  if not okCommBeacon then
						commBeacon = nil
					end
					if okCommBeacon and commBeacon ~= nil then
						beacon = commBeacon
					end
				end
			end

			if beacon == nil then
				return ""
			end

			local tacanValue = composeTacan(beacon)
			if tacanValue ~= "" then
				return tacanValue
			end

			if base.type(beacon) == "table" then
				for _, b in base.pairs(beacon) do
					tacanValue = composeTacan(b)
					if tacanValue ~= "" then
						return tacanValue
					end
				end
			end

			return ""
		end)

		if okTacan and result ~= nil then
			tacan = result
		end

		return tacan
	end,
  unitdiagnostics = function(Locator)
		local probe = ""
		local okProbe, result = base.pcall(function()
			if Locator == nil then
				return "LOC=nil"
			end

			local parts = {}
			local function addPart(v)
				if v == nil then return end
				if #parts >= 12 then return end
				parts[#parts + 1] = base.tostring(v)
			end

			local function readMethod(obj, methodName)
				if obj == nil then return nil end
				local okCall, value = base.pcall(function()
					if obj[methodName] then
						return obj[methodName](obj)
					end
					return nil
				end)
				if okCall then
					return value
				end
				return nil
			end

			local function readField(obj, fieldName)
				if obj == nil then return nil end
				local okRead, value = base.pcall(function() return obj[fieldName] end)
				if okRead then
					return value
				end
				return nil
			end

			local okBeacon, beacon = base.pcall(function() return Locator:getBeacon() end)
            if not okBeacon then
				beacon = nil
			end
			addPart("L:getBeacon=" .. (okBeacon and (beacon ~= nil and "ok" or "nil") or "err"))

			local comm = nil
			local okComm = false
			okComm, comm = base.pcall(function() return Locator:getCommunicator() end)
			addPart("L:getComm=" .. (okComm and (comm ~= nil and "ok" or "nil") or "err"))

			if (beacon == nil) and okComm and comm ~= nil then
				local okCommBeacon, commBeacon = base.pcall(function() return comm:getBeacon() end)
                if not okCommBeacon then
					commBeacon = nil
				end
				addPart("C:getBeacon=" .. (okCommBeacon and (commBeacon ~= nil and "ok" or "nil") or "err"))
				if okCommBeacon and commBeacon ~= nil then
					beacon = commBeacon
				end
			end

			local channel = readField(beacon, "channel")
			if channel == nil then channel = readField(beacon, "Channel") end
			if channel == nil then channel = readField(beacon, "channelNumber") end
			if channel == nil then channel = readField(beacon, "tacanChannel") end
			if channel == nil then channel = readMethod(beacon, "getChannel") end
			if channel == nil then channel = readMethod(beacon, "get_channel") end
			if channel == nil then channel = readMethod(beacon, "getChannelNumber") end
			if channel == nil then channel = readMethod(beacon, "getTacanChannel") end

			local band = readField(beacon, "modeChannel")
			if band == nil then band = readField(beacon, "modechannel") end
			if band == nil then band = readField(beacon, "band") end
			if band == nil then band = readField(beacon, "mode") end
			if band == nil then band = readField(beacon, "tacanBand") end
			if band == nil then band = readMethod(beacon, "getModeChannel") end
			if band == nil then band = readMethod(beacon, "get_modechannel") end
			if band == nil then band = readMethod(beacon, "getBand") end
			if band == nil then band = readMethod(beacon, "getMode") end
			if band == nil then band = readMethod(beacon, "getTacanBand") end

			addPart("ch=" .. base.tostring(channel))
			addPart("band=" .. base.tostring(band))

			if beacon ~= nil and base.type(beacon) == "table" then
				local i = 0
				for k,_ in base.pairs(beacon) do
					i = i + 1
					if i > 4 then break end
					addPart("bk." .. base.tostring(k))
				end
			end

			return base.table.concat(parts, ";")
		end)

		if okProbe and result ~= nil then
			probe = result
		end

		return probe
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

		local function addUniqueUnit(unit)
			if unit == nil then return end
			local uid = unit.id_
			if uid == nil then
				base.table.insert(Collection, unit)
				return
			end
			for _, existing in base.pairs(Collection) do
				if existing ~= nil and existing.id_ == uid then
					return
				end
			end
			base.table.insert(Collection, unit)
		end

		local function isAwacsLikeUnit(unit)
			if unit == nil then return false end

			local desc = nil
			local okDesc, valueDesc = base.pcall(function() return unit:getDesc() end)
			if okDesc then
				desc = valueDesc
			end

			if desc and desc.attributes and desc.attributes.AWACS then
				return true
			end

			local typeName = ""
			if desc ~= nil then
				typeName = base.string.upper(base.tostring(desc.typeName or desc.displayName or ""))
			end

			if base.string.find(typeName, "E-2", 1, true)
				or base.string.find(typeName, "E2", 1, true)
				or base.string.find(typeName, "HAWKEYE", 1, true)
				or base.string.find(typeName, "E-3", 1, true)
				or base.string.find(typeName, "E3", 1, true)
				or base.string.find(typeName, "SENTRY", 1, true)
				or base.string.find(typeName, "E-7", 1, true)
				or base.string.find(typeName, "E7", 1, true)
				or base.string.find(typeName, "WEDGETAIL", 1, true)
			then
				return true
			end

			local callsign = ""
			local okCallsign, valueCallsign = base.pcall(function()
				return base.vaicom.properties and base.vaicom.properties.missioncallsign and base.vaicom.properties.missioncallsign(unit) or ""
			end)
			if okCallsign and valueCallsign ~= nil then
				callsign = base.string.upper(base.tostring(valueCallsign))
			end

			if base.string.find(callsign, "DARKSTAR", 1, true)
				or base.string.find(callsign, "FOCUS", 1, true)
				or base.string.find(callsign, "MAGIC", 1, true)
				or base.string.find(callsign, "OVERLORD", 1, true)
				or base.string.find(callsign, "WIZARD", 1, true)
			then
				return true
			end

			return false
		end

		local okGroups, planeGroups = base.pcall(function()
			return base.coalition.getGroups and base.coalition.getGroups(getside, base.Group.Category.AIRPLANE)
		end)
		if okGroups and planeGroups ~= nil and base.type(planeGroups) == "table" then
			for _, g in base.pairs(planeGroups) do
				local okUnits, units = base.pcall(function() return g:getUnits() end)
				if okUnits and units ~= nil and base.type(units) == "table" then
					for _, u in base.pairs(units) do
						if isAwacsLikeUnit(u) then
							addUniqueUnit(u)
						end
					end
				end
			end
		end
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

		local function addUniqueUnit(unit)
			if unit == nil then return end
			local uid = unit.id_
			if uid == nil then
				base.table.insert(Collection, unit)
				return
			end
			for _, existing in base.pairs(Collection) do
				if existing ~= nil and existing.id_ == uid then
					return
				end
			end
			base.table.insert(Collection, unit)
		end

		local function addGroupUnits(group)
			if group == nil then return end
			local okUnits, units = base.pcall(function() return group:getUnits() end)
			if okUnits and units ~= nil and base.type(units) == "table" then
				for _, u in base.pairs(units) do
					addUniqueUnit(u)
				end
			end
		end

		local okPlaneGroups, planeGroups = base.pcall(function()
			return base.coalition.getGroups and base.coalition.getGroups(getside, base.Group.Category.AIRPLANE)
		end)
		if okPlaneGroups and planeGroups ~= nil and base.type(planeGroups) == "table" then
			for _, g in base.pairs(planeGroups) do
				addGroupUnits(g)
			end
		end

		local okHeliGroups, heliGroups = base.pcall(function()
			return base.coalition.getGroups and base.coalition.getGroups(getside, base.Group.Category.HELICOPTER)
		end)
		if okHeliGroups and heliGroups ~= nil and base.type(heliGroups) == "table" then
			for _, g in base.pairs(heliGroups) do
				addGroupUnits(g)
			end
		end
		return Collection
	end,	
   localOpposition = function(getside)
		local Collection = {}
		local opposite = nil
		if getside == base.coalition.side.BLUE then opposite = base.coalition.side.RED end
		if getside == base.coalition.side.RED then opposite = base.coalition.side.BLUE end

		local function addUniqueUnit(unit)
			if unit == nil then return end
			local uid = unit.id_
			if uid == nil then
				base.table.insert(Collection, unit)
				return
			end
			for _, existing in base.pairs(Collection) do
				if existing ~= nil and existing.id_ == uid then
					return
				end
			end
			base.table.insert(Collection, unit)
		end

		local function addUnits(units)
			if units ~= nil and base.type(units) == "table" then
				for _, u in base.pairs(units) do
					addUniqueUnit(u)
				end
			end
		end

		local function addGroupAircraft(group)
			if group == nil then return end
			local okUnits, units = base.pcall(function() return group:getUnits() end)
			if okUnits and units ~= nil and base.type(units) == "table" then
				for _, u in base.pairs(units) do
					addUniqueUnit(u)
				end
			end
		end
		if opposite then
           addUnits(base.vaicom.objects.localJTACs(opposite))
			addUnits(base.vaicom.objects.localAWACSs(opposite))
			addUnits(base.vaicom.objects.localTankers(opposite))
			addUnits(base.vaicom.objects.localATCs(opposite))
			addUnits(base.vaicom.objects.localAllies(opposite))

			local okPlaneGroups, planeGroups = base.pcall(function()
				return base.coalition.getGroups and base.coalition.getGroups(opposite, base.Group.Category.AIRPLANE)
			end)
			if okPlaneGroups and planeGroups ~= nil and base.type(planeGroups) == "table" then
				for _, g in base.pairs(planeGroups) do
					addGroupAircraft(g)
				end
			end

			local okHeliGroups, heliGroups = base.pcall(function()
				return base.coalition.getGroups and base.coalition.getGroups(opposite, base.Group.Category.HELICOPTER)
			end)
			if okHeliGroups and heliGroups ~= nil and base.type(heliGroups) == "table" then
				for _, g in base.pairs(heliGroups) do
					addGroupAircraft(g)
				end
			end
		end
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

		local function isRotorModule()
			local moduleCat = base.string.upper(base.tostring(base.vaicom.state and base.vaicom.state.dcsmodulecat or ""))
			if moduleCat == "HELICOPTERS" then
				return true
			end
			local dcsid = base.string.upper(base.tostring(base.vaicom.state and base.vaicom.state.dcsid or ""))
			if base.string.find(dcsid, "UH-", 1, true)
				or base.string.find(dcsid, "AH-", 1, true)
				or base.string.find(dcsid, "MI-", 1, true)
				or base.string.find(dcsid, "KA-", 1, true)
				or base.string.find(dcsid, "SA342", 1, true)
				or base.string.find(dcsid, "CH-47", 1, true)
			then
				return true
			end
			return false
		end

		local function isHeliportAtc(locator)
			if locator == nil then return false end
			local descName = ""
			local okDesc, desc = base.pcall(function() return locator:getDesc() end)
			if okDesc and desc ~= nil then
				descName = base.string.upper(base.tostring(desc.displayName or desc.typeName or ""))
			end
			local cs = ""
			local okCs, vCs = base.pcall(function()
				return base.vaicom.properties and base.vaicom.properties.missioncallsign and base.vaicom.properties.missioncallsign(locator) or ""
			end)
			if okCs and vCs ~= nil then
				cs = base.string.upper(base.tostring(vCs))
			end
			local full = descName .. " " .. cs
			return base.string.find(full, "HELI", 1, true) ~= nil
				or base.string.find(full, "HELIPAD", 1, true) ~= nil
				or base.string.find(full, "HELIPORT", 1, true) ~= nil
				or base.string.find(full, "FARP", 1, true) ~= nil
		end

		local rotor = isRotorModule()
		local filtered = {}
		local heliOnly = {}
		local nonHeliOnly = {}
		for _, atc in base.pairs(Listing) do
			if isHeliportAtc(atc) then
				heliOnly[#heliOnly + 1] = atc
			else
				nonHeliOnly[#nonHeliOnly + 1] = atc
			end
		end

		if rotor then
			for _, atc in base.pairs(heliOnly) do filtered[#filtered + 1] = atc end
			for _, atc in base.pairs(nonHeliOnly) do filtered[#filtered + 1] = atc end
		else
			for _, atc in base.pairs(nonHeliOnly) do filtered[#filtered + 1] = atc end
		end
		Listing = filtered
		return Listing
	end,
	localAWACSs = function(selectstr)
		local Listing = {}
		local coalition = data.pUnit and data.pUnit:getCoalition()
		if coalition then
			Listing = base.vaicom.helper.mergetables(Listing, base.vaicom.objects.localAWACSs(coalition))
		end
      local isMultiplayerNow = data.initialized and base.DCS.isMultiplayer() or false
		if (not isMultiplayerNow) and (not selectstr or selectstr == "radio") then
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
      local isMultiplayerNow = data.initialized and base.DCS.isMultiplayer() or false
		if (not isMultiplayerNow) and (not selectstr or selectstr == "radio") then
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
   localOpposition = function(selectstr)
		local Listing = {}
		local coalition = data.pUnit and data.pUnit:getCoalition()
		if coalition then
			Listing = base.vaicom.helper.mergetables(Listing, base.vaicom.objects.localOpposition(coalition))
		end
		if Listing ~= nil and #Listing > 1 then
			base.table.sort(Listing, base.vaicom.helper.sortby.distance)
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
              Opposition  = function(sortfunction, radio)
					local Stack = base.vaicom.list.localOpposition(radio)
					return Stack	
				end,
				},
				markers = function()
					local details = base.vaicom.get.missiondata.markerdetails()
					return (base.type(details) == "table") and #details or 0
				end,
				markerdetails = function()
					local details = {}
					local Stack = base.world.getMarkPanels()
					if base.type(Stack) ~= "table" then return details end
					local function tryget(fn)
						local ok, value = base.pcall(fn)
						if ok then return value end
						return nil
					end
					local function normalizeCoalition(v)
						if base.type(v) == "number" then return v end
						local s = base.string.upper(base.tostring(v or ""))
						if s == "RED" then return 1 end
						if s == "BLUE" then return 2 end
						if s == "NEUTRAL" then return 0 end
						return 0
					end
					local function getInitiatorName(initiator)
						if initiator == nil then return "" end
						local name = tryget(function() return initiator.getName and initiator:getName() end)
						if name ~= nil and name ~= "" then return base.tostring(name) end
						return base.tostring(initiator)
					end
					local function appendMarker(panel, fallbackId)
						if panel == nil then return end

						local p = tryget(function() return panel.pos end)
							or tryget(function() return panel.position end)
							or tryget(function() return panel.point end)
							or tryget(function() return panel.vec3 end)
							or tryget(function() return panel.coord end)
							or tryget(function() return panel.getPos and panel:getPos() end)

						local x = (base.type(p) == "table") and (p.x or p[1]) or (tryget(function() return panel.x end) or tryget(function() return panel.posX end) or tryget(function() return panel.pos_x end))
						local y = (base.type(p) == "table") and (p.y or p[2]) or (tryget(function() return panel.y end) or tryget(function() return panel.posY end) or tryget(function() return panel.pos_y end))
						local z = (base.type(p) == "table") and (p.z or p[3]) or (tryget(function() return panel.z end) or tryget(function() return panel.posZ end) or tryget(function() return panel.pos_z end))

						local markerId = tryget(function() return panel.idx end)
							or tryget(function() return panel.id end)
							or tryget(function() return panel.markId end)
							or tryget(function() return panel.markID end)
							or tryget(function() return panel.markerId end)
							or tryget(function() return panel.markerID end)
							or tryget(function() return panel.getId and panel:getId() end)
							or fallbackId

						local markerText = tryget(function() return panel.text end)
							or tryget(function() return panel.message end)
							or tryget(function() return panel.getText and panel:getText() end)
							or ""

						local markerInitiator = tryget(function() return panel.initiator end)
						local markerAuthor = tryget(function() return panel.author end)
						if markerAuthor == nil or markerAuthor == "" then
							markerAuthor = getInitiatorName(markerInitiator)
						end

						local markerCoal = tryget(function() return panel.coalition end)
							or tryget(function() return panel.side end)
							or tryget(function() return panel.getCoalition and panel:getCoalition() end)
							or 0
						local markerGroupId = tryget(function() return panel.groupID end)
							or tryget(function() return panel.groupId end)
							or tryget(function() return panel.getGroupID and panel:getGroupID() end)
							or -1

						base.table.insert(details, {
							id = markerId,
							text = base.tostring(markerText),
							author = base.tostring(markerAuthor),
							coalition = normalizeCoalition(markerCoal),
							groupID = markerGroupId,
							x = x or 0,
							y = y or 0,
							z = z or 0,
						})
					end
					for i = 1, #Stack do
						appendMarker(tryget(function() return Stack[i] end), i)
					end
					if #details == 0 then
						for k, panel in base.pairs(Stack) do
							local fallbackId = tonumber(k) or (#details + 1)
							appendMarker(panel, fallbackId)
						end
					end
					if #details == 0 then
						local getFn = tryget(function() return Stack.get end)
						if base.type(getFn) == "function" then
							for i = 1, 100 do
								local panel = tryget(function() return getFn(Stack, i) end)
								if panel == nil then break end
								appendMarker(panel, i)
							end
						end
					end
					return details
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
                                   Opposition		= {},
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
                                    Opposition		= 0,
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
               local f4eICSHot = false
				local ah64ICSHot = false
				local dcsId = base.vaicom.state.dcsid or ""
				local isF4E = base.string.find(dcsId, "F-4E", 1, true) ~= nil
				if data.initialized and isF4E and base.GetDevice(0) and base.GetDevice(0).get_argument_value then
					local pilotIcs = base.GetDevice(0):get_argument_value(1378)
                 local seatProxyLod = base.GetDevice(0):get_argument_value(3060)
					if seatProxyLod == nil then
						seatProxyLod = base.GetDevice(0):get_argument_value(3048)
					end
				   base.vaicom.state.riostate.f4ePilotIcs = pilotIcs or 0
                   base.vaicom.state.riostate.f4eSeat = seatProxyLod or -1
                 -- F-4E ICS selector: cold mic is negative, hot mic is centered, radio override is positive.
					-- Treat HOT MIC and radio override as active intercom states. (Off is inactive)
                   f4eICSHot = (pilotIcs ~= nil and pilotIcs > -0.1)
				else
					base.vaicom.state.riostate.f4ePilotIcs = 0
                 base.vaicom.state.riostate.f4eSeat = -1
				end

				if data.initialized and base.GetDevice(0) and base.GetDevice(0).get_argument_value and base.string.find(dcsId, "AH-64D", 1, true) ~= nil then
                    local seat = base.get_param_handle("SEAT"):get() -- Determine pilot or CPG seat
					local pltIcsMode = base.GetDevice(0):get_argument_value(346)
					local cpgIcsMode = base.GetDevice(0):get_argument_value(387) -- Added CPG controls for George Pilot expansion
                   -- AH-64 ICS mode switch labels are HOT MIC/VOX/PTT.
					-- Treat HOT MIC and VOX as active intercom states (PTT only is inactive).
                   if seat == 0 then
						ah64ICSHot = (pltIcsMode ~= nil and pltIcsMode < 0.5)
					elseif seat == 1 then
						ah64ICSHot = (cpgIcsMode ~= nil and cpgIcsMode < 0.5)
					else
						ah64ICSHot = (pltIcsMode ~= nil and pltIcsMode < 0.5) or (cpgIcsMode ~= nil and cpgIcsMode < 0.5)
					end
				end
			   base.vaicom.state.riostate.ics						= (base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(2044) > -1))) or f4eICSHot or ah64ICSHot -- Check for F-14 ICS state, F-4E pilot ICS hot mic position, or AH-64 ICS hot mic position
				base.vaicom.state.riostate.sngl						= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(60) >0)) or false
				base.vaicom.state.riostate.jmr						= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(151) ==1)) or false
				base.vaicom.state.riostate.AM182					= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(359) ==1)) or false
				base.vaicom.state.riostate.ejsn						= base.vaicom.state.activemessage.AIRIO and (data.initialized and base.GetDevice(0).get_argument_value and (base.GetDevice(0):get_argument_value(2049) ==1)) or false
				base.vaicom.state.riostate.markers					= (data.initialized and base.vaicom.get.missiondata.markers()) or 0
				base.vaicom.state.riostate.markerdetails				= (data.initialized and base.vaicom.get.missiondata.markerdetails()) or {}
				base.vaicom.state.availablerecipients.Player 		= data.initialized and base.vaicom.get.missiondata.listby.Player(base.vaicom.helper.sortby.index)
				base.vaicom.state.availablerecipients.Flight 		= data.initialized and base.vaicom.get.missiondata.listby.Flight(base.vaicom.helper.sortby.index,	"radio")					
				base.vaicom.state.availablerecipients.JTAC			= data.initialized and base.vaicom.get.missiondata.listby.JTAC(base.vaicom.helper.sortby.distance,	"radio")
				base.vaicom.state.availablerecipients.ATC			= data.initialized and base.vaicom.get.missiondata.listby.ATC(base.vaicom.helper.sortby.distance, 	"radio")
				base.vaicom.state.availablerecipients.AWACS			= data.initialized and base.vaicom.get.missiondata.listby.AWACS(base.vaicom.helper.sortby.distance, "radio")
				base.vaicom.state.availablerecipients.Tanker		= data.initialized and base.vaicom.get.missiondata.listby.Tanker(base.vaicom.helper.sortby.distance,"radio") 
               base.vaicom.state.availablerecipients.Opposition	= data.initialized and base.vaicom.get.missiondata.listby.Opposition(base.vaicom.helper.sortby.distance, "radio")
				base.vaicom.state.availablerecipients.Crew			= data.initialized and base.vaicom.get.missiondata.listby.Crew(base.vaicom.helper.sortby.distance, 	"radio") 
				base.vaicom.state.availablerecipients.Aux			= data.initialized and base.vaicom.get.missiondata.listby.Aux(base.vaicom.helper.sortby.distance, 	"radio")
				base.vaicom.state.availablerecipients.Moose			= data.initialized and base.vaicom.get.missiondata.listby.Moose(base.vaicom.helper.sortby.distance, "radio") -- Add moose
				base.vaicom.state.availablerecipients.Cargo			= data.initialized and base.vaicom.get.missiondata.listby.Cargo(base.vaicom.helper.sortby.distance, "radio")
				base.vaicom.state.availablerecipients.Allies		= data.initialized and base.vaicom.get.missiondata.listby.Allies(base.vaicom.helper.sortby.distance, "radio")				
				for recipientclass,_ in base.pairs(base.vaicom.state.availablerecipients) do
					base.vaicom.state.availabilitycounter[recipientclass] = base.vaicom.helper.tablelength(base.vaicom.state.availablerecipients[recipientclass])
				end
				base.vaicom.state.menuaux							= data.initialized and data.menuOther
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
               local function getMissionObject()
					local function tryget(fn)
						local ok, value = base.pcall(fn)
						if ok then return value end
						return nil
					end

					local missionObj = tryget(function() return mission end)
					if missionObj == nil then missionObj = tryget(function() return base.env and base.env.mission end) end
					if missionObj == nil then missionObj = tryget(function() return _G and _G.mission end) end
					return missionObj
				end

				local function extractIcaoToken(text)
					local s = base.string.upper(base.tostring(text or ""))
					return base.string.match(s, "%u%u%u%u")
				end

				local function normalizeIcaoKey(text)
					local s = base.string.upper(base.tostring(text or ""))
					s = base.string.gsub(s, "[_%-/%.%,%(%)]", " ")
					s = base.string.gsub(s, "%s+", " ")
					s = base.string.gsub(s, "^%s+", "")
					s = base.string.gsub(s, "%s+$", "")
					return s
				end

				local function loadIcaoOverrides()
					base.vaicom.state.icaooverrides = base.vaicom.state.icaooverrides or {}
					local now = base.Export.LoGetModelTime and base.Export.LoGetModelTime() or 0
					if base.vaicom.state.icaooverrides.lastload and (now - base.vaicom.state.icaooverrides.lastload) < 30 then
						return base.vaicom.state.icaooverrides.table or {}
					end

                 local overrides = {}
					local runtimeLoad = base.loadfile or loadfile
					local candidatePaths = {}
					if base.lfs and base.lfs.writedir then
						base.table.insert(candidatePaths, base.lfs.writedir() .. "Scripts\\VAICOMPRO\\ICAOOverrides.lua")
						base.table.insert(candidatePaths, base.lfs.writedir() .. "Scripts\\Aircrafts\\_Common\\Cockpit\\VAICOMPRO\\device\\ICAOOverrides.lua")
					end

					for _, path in base.pairs(candidatePaths) do
						if runtimeLoad then
							local okLoad, chunk = base.pcall(function()
								return runtimeLoad(path)
							end)
							if okLoad and chunk ~= nil then
								local okExec, result = base.pcall(chunk)
								if okExec and base.type(result) == "table" then
									overrides = result
									break
								end
							end
						end
					end

					base.vaicom.state.icaooverrides.table = overrides
					base.vaicom.state.icaooverrides.lastload = now
					return overrides
				end

				local function resolveIcaoMetaForAtc(callsign, atcName)
					local overrides = loadIcaoOverrides()
					local theatre = base.string.lower(base.tostring(base.vaicom.state and base.vaicom.state.theatre or ""))
					local keyCallsign = normalizeIcaoKey(callsign)
					local keyName = normalizeIcaoKey(atcName)

					local function normalizeIcaoType(value)
						local t = base.string.upper(base.tostring(value or ""))
						if t == "MIL" or t == "CIV" or t == "JOINT" then
							return t
						end
						return ""
					end

					local function readCodeAndType(scope, key)
						if base.type(scope) ~= "table" or key == "" then
							return nil, ""
						end
						local value = scope[key]
						if value == nil then
							return nil, ""
						end

						if base.type(value) == "table" then
							local code = base.string.upper(base.tostring(value.icao or ""))
							if base.string.match(code, "^%u%u%u%u$") then
								return code, normalizeIcaoType(value.type)
							end
							return nil, ""
						end

						local code = base.string.upper(base.tostring(value))
						if base.string.match(code, "^%u%u%u%u$") then
							return code, ""
						end
						return nil, ""
					end

					local theatreTable = overrides[theatre]
					local fallbackTable = overrides.default
					local code, airfieldType = readCodeAndType(theatreTable, keyCallsign)
					if code == nil then code, airfieldType = readCodeAndType(theatreTable, keyName) end
					if code == nil then code, airfieldType = readCodeAndType(fallbackTable, keyCallsign) end
					if code == nil then code, airfieldType = readCodeAndType(fallbackTable, keyName) end
					if code ~= nil then
						return {
							icao = code,
							type = airfieldType,
						}
					end

					local direct = extractIcaoToken(callsign)
					if direct ~= nil then
						return {
							icao = direct,
							type = "",
						}
					end

					return nil
				end

				local function resolveIcaoForAtc(callsign, atcName)
					local mappedMeta = resolveIcaoMetaForAtc(callsign, atcName)
					if mappedMeta ~= nil and mappedMeta.icao ~= nil then
						return mappedMeta.icao
					end

					local direct = extractIcaoToken(callsign)
					if direct ~= nil then
						return direct
					end

					return nil
				end

				local function buildAllAtcIcaoTypes()
					local result = {}
					local atcs = base.vaicom.state and base.vaicom.state.availablerecipients and base.vaicom.state.availablerecipients.ATC
					if base.type(atcs) ~= "table" then
						return result
					end

					for _, atc in base.pairs(atcs) do
						if atc then
							local callsign = ""
							local okCallsign, valueCallsign = base.pcall(function()
								return base.vaicom.properties and base.vaicom.properties.missioncallsign and base.vaicom.properties.missioncallsign(atc) or ""
							end)
							if okCallsign and valueCallsign ~= nil then
								callsign = base.tostring(valueCallsign)
							end

							local atcName = ""
							local okDescName, atcDesc = base.pcall(function() return atc:getDesc() end)
							if okDescName and atcDesc ~= nil then
								atcName = base.tostring(atcDesc.displayName or atcDesc.typeName or "")
							end

							local normalizedCallsign = normalizeIcaoKey(callsign)
							local normalizedAtcName = normalizeIcaoKey(atcName)
							local meta = resolveIcaoMetaForAtc(callsign, atcName)
							if meta ~= nil then
								local typeCode = base.string.upper(base.tostring(meta.type or ""))
								if typeCode == "MIL" or typeCode == "CIV" or typeCode == "JOINT" then
									local icao = base.string.upper(base.tostring(meta.icao or ""))
									if icao ~= "" then
										result[icao] = typeCode
									end
									if normalizedCallsign ~= "" then
										result[normalizedCallsign] = typeCode
									end
									if normalizedAtcName ~= "" then
										result[normalizedAtcName] = typeCode
									end

									local upperCallsign = base.string.upper(base.tostring(callsign or ""))
									if upperCallsign ~= "" then
										result[upperCallsign] = typeCode
									end

									local shortCallsign = ""
									local okShort, valueShort = base.pcall(function()
										return base.vaicom.properties and base.vaicom.properties.callsign and base.vaicom.properties.callsign(atc) or ""
									end)
									if okShort and valueShort ~= nil then
										shortCallsign = base.string.upper(base.tostring(valueShort))
									end
									if shortCallsign ~= "" then
										result[shortCallsign] = typeCode
									end
								end
							end
						end
					end

					return result
				end

              local function buildMetarForAtcInfo(atcInfoOverride)
					local missionObj = getMissionObject()
					if base.type(missionObj) ~= "table" then
						return ""
					end

					local function isRotorModule()
						local moduleCat = base.string.upper(base.tostring(base.vaicom.state and base.vaicom.state.dcsmodulecat or ""))
						if moduleCat == "HELICOPTERS" then
							return true
						end
						local dcsid = base.string.upper(base.tostring(base.vaicom.state and base.vaicom.state.dcsid or ""))
						if base.string.find(dcsid, "UH-", 1, true)
							or base.string.find(dcsid, "AH-", 1, true)
							or base.string.find(dcsid, "MI-", 1, true)
							or base.string.find(dcsid, "KA-", 1, true)
							or base.string.find(dcsid, "SA342", 1, true)
							or base.string.find(dcsid, "CH-47", 1, true)
						then
							return true
						end
						return false
					end

					local function isHeliportAtc(locator)
						if locator == nil then return false end
						local descName = ""
						local okDesc, desc = base.pcall(function() return locator:getDesc() end)
						if okDesc and desc ~= nil then
							descName = base.string.upper(base.tostring(desc.displayName or desc.typeName or ""))
						end
						local cs = ""
						local okCs, vCs = base.pcall(function()
							return base.vaicom.properties and base.vaicom.properties.missioncallsign and base.vaicom.properties.missioncallsign(locator) or ""
						end)
						if okCs and vCs ~= nil then
							cs = base.string.upper(base.tostring(vCs))
						end
						local full = descName .. " " .. cs
						return base.string.find(full, "HELI", 1, true) ~= nil
							or base.string.find(full, "HELIPAD", 1, true) ~= nil
							or base.string.find(full, "HELIPORT", 1, true) ~= nil
							or base.string.find(full, "FARP", 1, true) ~= nil
					end

                  local function getClosestAtcInfo()
						if not (data and data.pUnit and data.pUnit.getPoint) then
                            return { icao = "DCS", elevationFt = 0 }
						end

						local playerPoint = data.pUnit:getPoint()
						local atcs = base.vaicom.state and base.vaicom.state.availablerecipients and base.vaicom.state.availablerecipients.ATC
						if base.type(atcs) ~= "table" then
                        return { icao = "DCS", elevationFt = 0 }
						end

                     local closestIcao = nil
						local closestElevationFt = 0
						local closestDist = nil

						for _, atc in base.pairs(atcs) do
							if atc and atc.getPoint then
								local atcPoint = atc:getPoint()
								if atcPoint then
									local dx = (atcPoint.x or 0) - (playerPoint.x or 0)
									local dz = (atcPoint.z or 0) - (playerPoint.z or 0)
									local distSq = (dx * dx) + (dz * dz)

									local callsign = ""
									local okCallsign, valueCallsign = base.pcall(function()
										return base.vaicom.properties and base.vaicom.properties.missioncallsign and base.vaicom.properties.missioncallsign(atc) or ""
									end)
									if okCallsign and valueCallsign ~= nil then
										callsign = base.tostring(valueCallsign)
									end

									local atcName = ""
									local okDescName, atcDesc = base.pcall(function() return atc:getDesc() end)
									if okDescName and atcDesc ~= nil then
										atcName = base.tostring(atcDesc.displayName or atcDesc.typeName or "")
									end

									local icao = resolveIcaoForAtc(callsign, atcName)
									local rotor = isRotorModule()
									local heliport = isHeliportAtc(atc)
									if (rotor and icao ~= nil) or ((not rotor) and icao ~= nil and (not heliport)) then
										if closestDist == nil or distSq < closestDist then
											closestDist = distSq
											closestIcao = icao
                                         closestElevationFt = (base.tonumber(atcPoint.y) or 0) * 3.28084
										end
									end
								end
							end
						end

                     return {
							icao = closestIcao or "DCS",
							elevationFt = closestElevationFt or 0
						}
					end

					local weather = missionObj.weather
					if base.type(weather) ~= "table" then
						return ""
					end

                 local missionDate = missionObj.date or {}
					local reportDay = base.tonumber(missionDate.Day or missionDate.day) or 1
					if reportDay < 1 then reportDay = 1 end
					if reportDay > 31 then reportDay = 31 end
					local missionStartSec = base.tonumber(base.vaicom.state and base.vaicom.state.tod or 0) or 0
					local elapsedSec = base.tonumber(base.vaicom.state and base.vaicom.state.timer or 0) or 0
					local reportSec = (missionStartSec + elapsedSec) % 86400
					if reportSec < 0 then reportSec = reportSec + 86400 end
					local reportHour = base.math.floor(reportSec / 3600)
					local reportMin = base.math.floor((reportSec - (reportHour * 3600)) / 60)

                 local atcInfo = atcInfoOverride or getClosestAtcInfo()
					local stationElevationFt = base.tonumber(atcInfo and atcInfo.elevationFt or 0) or 0
					local groundWindDir = weather.wind and weather.wind.atGround and weather.wind.atGround.dir or nil
					local groundWindSpd = base.tonumber(weather.wind and weather.wind.atGround and weather.wind.atGround.speed)
					local upperWindDir = weather.wind and weather.wind.at2000 and weather.wind.at2000.dir or nil
					local upperWindSpd = base.tonumber(weather.wind and weather.wind.at2000 and weather.wind.at2000.speed)
					local elevatedWind = stationElevationFt >= 1600 and upperWindSpd ~= nil
					local windDir = groundWindDir
					local windSpd = groundWindSpd
					if elevatedWind then
						windDir = upperWindDir or groundWindDir
                        local upperAdjustedSpd = upperWindSpd - (5 / 1.94384)
						if upperAdjustedSpd < 0 then upperAdjustedSpd = 0 end
						if groundWindSpd ~= nil then
							local t = (stationElevationFt - 1599) / 1600
							if t < 0 then t = 0 end
							if t > 1 then t = 1 end
                          windSpd = groundWindSpd + ((upperAdjustedSpd - groundWindSpd) * t)
						else
                          windSpd = upperAdjustedSpd
						end
					end
                   local vis = weather.visibility and weather.visibility.distance or nil
				   local temp = weather.season and weather.season.temperature or nil
					local stationTemp = base.tonumber(temp)
					if stationTemp ~= nil then
						stationTemp = stationTemp - (stationElevationFt / 1000) * 2
					end
					local qnhRaw = weather.qnh
                    local clouds = weather.clouds or {}
					local cloudsDensity = clouds.density or clouds.cover or clouds.coverage or 0
					local cloudsBase = clouds.base or nil

					local function toInt(v)
						local n = base.tonumber(v)
						if n == nil then return nil end
						return base.math.floor(n + 0.5)
					end

					local function pad3(v)
						local n = toInt(v) or 0
						if n < 0 then n = 0 end
						if n > 999 then n = 999 end
						return base.string.format("%03d", n)
					end

					local function pad2(v)
						local n = toInt(v) or 0
						if n < 0 then n = 0 end
						if n > 99 then n = 99 end
						return base.string.format("%02d", n)
					end

					local function angularDiff(a, b)
						if a == nil or b == nil then return 0 end
						local d = base.math.abs(a - b) % 360
						if d > 180 then d = 360 - d end
						return d
					end

					local dirFrom = toInt(windDir)
					if dirFrom ~= nil then
						dirFrom = (dirFrom + 180) % 360
                     dirFrom = (base.math.floor((dirFrom + 5) / 10) * 10) % 360
					end

                    local groundFrom = toInt(groundWindDir)
					if groundFrom ~= nil then groundFrom = (groundFrom + 180) % 360 end
					local upperFrom = toInt(upperWindDir)
					if upperFrom ~= nil then upperFrom = (upperFrom + 180) % 360 end

                    local spdKt = toInt((base.tonumber(windSpd) or 0) * 1.94384) or 0
					local visM = toInt(vis)
					if visM == nil or visM <= 0 then
						visM = 9999
					end
					local turbulence = base.tonumber(weather.groundTurbulence)

					local function minPositive(a, b)
						local an = base.tonumber(a)
						local bn = base.tonumber(b)
						if an == nil or an <= 0 then return bn end
						if bn == nil or bn <= 0 then return an end
						return (an < bn) and an or bn
					end

					local function findFog2Visibility(obj, depth)
						if base.type(obj) ~= "table" or depth > 5 then return nil end
						local found = nil
						for k,v in base.pairs(obj) do
							if base.type(v) == "table" then
								found = minPositive(found, findFog2Visibility(v, depth + 1))
							else
								local ks = base.string.lower(base.tostring(k))
								if (base.string.find(ks, "vis", 1, true) or base.string.find(ks, "distance", 1, true)) and base.tonumber(v) ~= nil then
									found = minPositive(found, base.tonumber(v))
								end
							end
						end
						return found
					end

					local fogVis = nil
					if weather.fog and base.type(weather.fog) == "table" then
						fogVis = minPositive(fogVis, weather.fog.visibility or weather.fog.distance)
					end
					if weather.fog2 and base.type(weather.fog2) == "table" then
						fogVis = minPositive(fogVis, weather.fog2.visibility or weather.fog2.distance)
						fogVis = minPositive(fogVis, findFog2Visibility(weather.fog2, 0))
					end
                 local dustVis = nil
					if weather.enable_dust and (base.tonumber(weather.dust_density) or 0) > 0 then
						dustVis = base.tonumber(weather.dust_density)
					end

					visM = toInt(minPositive(minPositive(visM, fogVis), dustVis)) or visM
					if visM > 9999 then visM = 9999 end

					local qnhHpa = toInt(qnhRaw)
					if qnhHpa ~= nil and qnhHpa < 900 then
						qnhHpa = toInt((base.tonumber(qnhRaw) or 760) * 1.33322)
					end
					if qnhHpa == nil then qnhHpa = 1013 end

                 local presetName = base.string.upper(base.tostring(clouds.preset or clouds.name or ""))
					if (base.tonumber(cloudsDensity) or 0) <= 0 and presetName ~= "" then
						if base.string.find(presetName, "OVC", 1, true) or base.string.find(presetName, "OVERCAST", 1, true) then
							cloudsDensity = 8
						elseif base.string.find(presetName, "BKN", 1, true) or base.string.find(presetName, "BROKEN", 1, true) then
							cloudsDensity = 6
						elseif base.string.find(presetName, "SCT", 1, true) then
							cloudsDensity = 4
						elseif base.string.find(presetName, "FEW", 1, true) then
							cloudsDensity = 2
						elseif base.string.find(presetName, "CLR", 1, true) or base.string.find(presetName, "CLEAR", 1, true) then
							cloudsDensity = 0
						else
							cloudsDensity = 6
						end
					end

					local cloudCode = "SKC"
					if cloudsDensity >= 1 and cloudsDensity <= 2 then cloudCode = "FEW" end
					if cloudsDensity >= 3 and cloudsDensity <= 5 then cloudCode = "SCT" end
					if cloudsDensity >= 6 and cloudsDensity <= 7 then cloudCode = "BKN" end
					if cloudsDensity >= 8 then cloudCode = "OVC" end

                 local cloudPart = cloudCode
					local cloudAtGround = false
					local cloudBaseMslFt = nil
					local cloudBaseAglFt = nil
					if cloudsBase ~= nil then
                     cloudBaseMslFt = base.tonumber(cloudsBase)
						if cloudBaseMslFt ~= nil then
							cloudBaseMslFt = cloudBaseMslFt * 3.28084
							cloudBaseAglFt = cloudBaseMslFt - stationElevationFt
						end
					end
					if cloudCode ~= "SKC" and cloudsBase ~= nil then
                      local baseHundredsFt = toInt((cloudBaseAglFt or 0) / 100)
						if baseHundredsFt ~= nil and baseHundredsFt < 0 then baseHundredsFt = 0 end
                      if (cloudBaseAglFt or 0) <= 0 then cloudAtGround = true end
						if (baseHundredsFt or 0) < 1 then baseHundredsFt = 1 end
						cloudPart = cloudCode .. pad3(baseHundredsFt)
					end

					local wx = {}
					if weather.enable_dust and (base.tonumber(weather.dust_density) or 0) > 0 then
						base.table.insert(wx, "DU")
					end
                 local fog2Active = weather.fog2 and base.type(weather.fog2) == "table" and ((base.tonumber(weather.fog2.mode) or 0) > 0)
					if (weather.enable_fog and weather.fog and (base.tonumber(weather.fog.visibility) or 0) > 0)
						or (fog2Active and (base.tonumber(fogVis) or 0) > 0)
					then
						base.table.insert(wx, "FG")
					end
					if cloudAtGround then
						local lowCloudWx = "BCFG"
						if cloudCode == "OVC" then lowCloudWx = "FG" end
						local alreadyPresent = false
						for _, w in base.pairs(wx) do
							if w == lowCloudWx then
								alreadyPresent = true
								break
							end
						end
						if not alreadyPresent then
							base.table.insert(wx, lowCloudWx)
						end
                     local lowCloudVis = base.math.random(500, 2999)
						if visM == nil then
							visM = lowCloudVis
						elseif visM > lowCloudVis then
							visM = lowCloudVis
						end
					end

					if visM ~= nil then
						if visM >= 10000 then
							visM = 9999
						else
							visM = toInt((visM + 50) / 100) * 100
							if visM < 0 then visM = 0 end
							if visM >= 10000 then visM = 9999 end
						end
					end

					local precip = weather.clouds and (weather.clouds.iprecptns or weather.clouds.precipitation) or nil
					local precipCode = nil
					if precip ~= nil then
						local p = toInt(precip) or 0
                        if p == 1 then
							local t = stationTemp
							if t ~= nil then
								if t < 0 then
									precipCode = "SN"
								elseif t >= 0 and t <= 1 then
									precipCode = "RA SNSH"
								else
									precipCode = "RA"
								end
							else
								precipCode = "RA"
							end
						end
						if p >= 2 then precipCode = "TSRA" end
					end
                   if precipCode == nil then
						if presetName ~= "" and base.string.find(presetName, "RAIN", 1, true) then
                           local t = stationTemp
							if t ~= nil then
								if t < 0 then
									precipCode = "SN"
								elseif t >= 0 and t <= 1 then
									precipCode = "RA SNSH"
								else
									precipCode = "RA"
								end
							else
								precipCode = "RA"
							end
						end
					end
					if precipCode ~= nil then
						base.table.insert(wx, precipCode)
					end

					if visM <= 100 and #wx == 0 and (base.tonumber(cloudBaseAglFt) == nil or cloudBaseAglFt > 0) then
						visM = 9999
					end

					if precipCode ~= nil and base.string.find(precipCode, "TS", 1, true) and cloudPart ~= "SKC" and base.string.find(cloudPart, "CB", 1, true) == nil then
						cloudPart = cloudPart .. "CB"
					end

					local wxPart = ""
					if #wx > 0 then
						wxPart = base.table.concat(wx, " ") .. " "
					end

					local tempPart = "--"
                 local tempInt = toInt(stationTemp)
					if tempInt ~= nil then
						if tempInt < 0 then
							tempPart = "M" .. pad2(-tempInt)
						else
							tempPart = pad2(tempInt)
						end
					end

                    local windPart = "00000KT"
					local gustKt = nil
					if turbulence ~= nil and spdKt > 0 then
						gustKt = toInt(spdKt + base.math.max(0, ((turbulence - 20) / 4)))
						if gustKt ~= nil and gustKt > (spdKt + 35) then gustKt = spdKt + 35 end
					end
					if dirFrom ~= nil then
						if spdKt <= 2 then
							windPart = "VRB" .. pad2(spdKt) .. "KT"
						else
							windPart = pad3(dirFrom) .. pad2(spdKt)
							if gustKt ~= nil and gustKt >= (spdKt + 4) then
								windPart = windPart .. "G" .. pad2(gustKt)
							end
							windPart = windPart .. "KT"
						end
					end

					local windVarPart = ""
					if spdKt > 3 and groundFrom ~= nil and upperFrom ~= nil and angularDiff(groundFrom, upperFrom) >= 60 then
						local low = base.math.min(groundFrom, upperFrom)
						local high = base.math.max(groundFrom, upperFrom)
						windVarPart = " " .. pad3(low) .. "V" .. pad3(high)
					end

                   local useCavok = false
					local noCloudBelow5000 = false
					if cloudCode == "SKC" then
						noCloudBelow5000 = true
                  elseif cloudBaseAglFt ~= nil and cloudBaseAglFt >= 5000 then
						noCloudBelow5000 = true
					end
					if visM >= 9999 and noCloudBelow5000 then
						useCavok = true
					end

					local skyPart = ""
					if useCavok then
						skyPart = "CAVOK"
					else
                        local useVV = cloudAtGround and ((base.tonumber(fogVis) or 0) > 0 or visM < 1000)
						if useVV then
							local vvHundreds = toInt((cloudBaseAglFt or 0) / 100)
							if vvHundreds == nil or vvHundreds < 1 then vvHundreds = 1 end
							skyPart = base.string.format("%04d", visM) .. " " .. wxPart .. "VV" .. pad3(vvHundreds)
						else
							skyPart = base.string.format("%04d", visM) .. " " .. wxPart .. cloudPart
						end
					end

					local rmkPart = ""
                   if turbulence ~= nil then
						if turbulence > 40 then
							rmkPart = " RMK SEV TURB B050"
                     elseif turbulence > 27 and turbulence <= 40 then
							rmkPart = " RMK MOD TURB B050"
						end
					end

                  local station = atcInfo and atcInfo.icao or "DCS"
                 local metarTime = pad2(reportDay) .. pad2(reportHour) .. pad2(reportMin) .. "Z"
					return "METAR " .. station .. " " .. metarTime .. " " .. windPart .. windVarPart .. " " .. skyPart .. " " .. tempPart .. " Q" .. base.string.format("%04d", qnhHpa) .. rmkPart
				end

				local function buildAtcMetar()
					return buildMetarForAtcInfo(nil)
				end

				local function buildAllAtcMetars()
					local result = {}
					local atcs = base.vaicom.state and base.vaicom.state.availablerecipients and base.vaicom.state.availablerecipients.ATC
					if base.type(atcs) ~= "table" then
						return result
					end

					for _, atc in base.pairs(atcs) do
						if atc and atc.getPoint then
							local atcPoint = atc:getPoint()
							if atcPoint then
								local callsign = ""
								local okCallsign, valueCallsign = base.pcall(function()
									return base.vaicom.properties and base.vaicom.properties.missioncallsign and base.vaicom.properties.missioncallsign(atc) or ""
								end)
								if okCallsign and valueCallsign ~= nil then
									callsign = base.tostring(valueCallsign)
								end

                              local atcName = ""
								local okDescName, atcDesc = base.pcall(function() return atc:getDesc() end)
								if okDescName and atcDesc ~= nil then
									atcName = base.tostring(atcDesc.displayName or atcDesc.typeName or "")
								end

								local icao = resolveIcaoForAtc(callsign, atcName)
								local stationInfo = {
									icao = icao or "DCS",
									elevationFt = (base.tonumber(atcPoint.y) or 0) * 3.28084
								}
								local metar = buildMetarForAtcInfo(stationInfo)

								if icao ~= nil and icao ~= "" then
									result[icao] = metar
								end

								local upperCallsign = base.string.upper(base.tostring(callsign or ""))
								if upperCallsign ~= "" then
									result[upperCallsign] = metar
								end

								local shortCallsign = ""
								local okShort, valueShort = base.pcall(function()
									return base.vaicom.properties and base.vaicom.properties.callsign and base.vaicom.properties.callsign(atc) or ""
								end)
								if okShort and valueShort ~= nil then
									shortCallsign = base.string.upper(base.tostring(valueShort))
								end
								if shortCallsign ~= "" then
									result[shortCallsign] = metar
								end
							end
						end
					end

					return result
				end

              local function normalizeBand(v)
					if v == nil then return "" end
					local s = base.tostring(v)
					if s == "" then return "" end
					local u = base.string.upper(s)
					if u == "X" or u == "Y" then return u end
					local n = base.tonumber(s)
					if n == 0 then return "X" end
					if n == 1 then return "Y" end
					return s
				end

				local function extractTaskTacan(taskObj)
					if base.type(taskObj) ~= "table" then return "" end
					local id = taskObj.id
					local params = taskObj.params or {}

					if id == "WrappedAction" and params.action and base.type(params.action) == "table" then
						id = params.action.id
						params = params.action.params or {}
					elseif taskObj.action and base.type(taskObj.action) == "table" then
						id = taskObj.action.id or id
						params = taskObj.action.params or params
					end

                  local idStr = base.tostring(id or "")
					local idLower = base.string.lower(idStr)
					local isBeaconTask = (id == "ActivateBeacon") or (idLower ~= "" and base.string.find(idLower, "beacon") ~= nil)

					local channel = params.channel or params.Channel or params.channelNumber
                    if channel == nil and params.beacon and base.type(params.beacon) == "table" then
						channel = params.beacon.channel or params.beacon.Channel or params.beacon.channelNumber
					end
					if channel == nil then return "" end
					if not isBeaconTask and params.modeChannel == nil and params.band == nil and params.mode == nil and params.beacon == nil then
						return ""
					end

					local chNum = base.tonumber(channel)
					local ch = chNum ~= nil and base.tostring(base.math.floor(chNum + 0.5)) or base.tostring(channel)
                    local band = params.modeChannel or params.band or params.mode
					if band == nil and params.beacon and base.type(params.beacon) == "table" then
						band = params.beacon.modeChannel or params.beacon.band or params.beacon.mode
					end
					return ch .. normalizeBand(band)
				end

				local function extractTacanFromTaskNode(taskObj, depth)
					if base.type(taskObj) ~= "table" then return "" end
					if depth ~= nil and depth > 8 then return "" end

					local tac = extractTaskTacan(taskObj)
					if tac ~= "" then
						return tac
					end

					local params = taskObj.params or {}
					local nested = {
						params.tasks,
						params.task,
						taskObj.task,
						taskObj.tasks,
					}

					for _, node in base.pairs(nested) do
						if base.type(node) == "table" then
							for _, child in base.pairs(node) do
								local childTac = extractTacanFromTaskNode(child, (depth or 0) + 1)
								if childTac ~= "" then
									return childTac
								end
							end
						end
					end

					return ""
				end

				local function extractTacanFromRoutePoints(points)
					if base.type(points) ~= "table" then return "" end
					for _, p in base.pairs(points) do
						local tasks = p and p.task and p.task.params and p.task.params.tasks
						if base.type(tasks) == "table" then
							for _, t in base.pairs(tasks) do
								local tac = extractTacanFromTaskNode(t, 0)
								if tac ~= "" then
									return tac
								end
							end
						end
					end
					return ""
				end

				local function normalizeTacanValue(value)
					if value == nil then return "" end
					local s = base.tostring(value)
					if s == nil then return "" end
					local lower = base.string.lower(s)
					if s == "" or lower == "nil" or lower == "null" then
						return ""
					end
					return s
				end

				local function getGroupNameCandidates(groupName)
					local candidates = {}
					local seen = {}

					local function addCandidate(v)
						v = base.tostring(v or "")
						if v == "" then return end
						if seen[v] then return end
						seen[v] = true
						base.table.insert(candidates, v)
					end

					local raw = base.tostring(groupName or "")
					addCandidate(raw)
					addCandidate(base.string.gsub(raw, "#%d+$", ""))
					addCandidate(base.string.gsub(base.string.gsub(raw, "#%d+$", ""), "_%d+$", ""))

					for token in base.string.gmatch(base.string.gsub(raw, "#%d+$", ""), "[^_]+") do
						addCandidate(token)
					end

					return candidates
				end

             local function resolveTacanFromGroupMap(groupName, sourceMap)
					for _, candidate in base.pairs(getGroupNameCandidates(groupName)) do
                      local tac = normalizeTacanValue(sourceMap[candidate])
						if tac ~= "" then return tac end
					end
					return ""
				end

				local function buildMissionGroupTacanMap()
					local result = {}
					local missionObj = getMissionObject()
					if base.type(missionObj) ~= "table" then
						return result
					end

					local coal = missionObj.coalition
					if base.type(coal) ~= "table" then
						return result
					end

					for _, coalData in base.pairs(coal) do
						if base.type(coalData) == "table" and base.type(coalData.country) == "table" then
							for _, country in base.pairs(coalData.country) do
								for _, catName in base.pairs({"plane", "helicopter", "ship"}) do
									local cat = country and country[catName]
									if base.type(cat) == "table" and base.type(cat.group) == "table" then
										for _, group in base.pairs(cat.group) do
											if base.type(group) == "table" and base.type(group.route) == "table" and base.type(group.route.points) == "table" then
												local gname = base.tostring(group.name or "")
												if gname ~= "" and result[gname] == nil then
													for _, p in base.pairs(group.route.points) do
														local tasks = p and p.task and p.task.params and p.task.params.tasks
														if base.type(tasks) == "table" then
															for _, t in base.pairs(tasks) do
																local tac = extractTaskTacan(t)
																if tac ~= "" then
																	result[gname] = tac
																	break
																end
															end
														end
														if result[gname] ~= nil then break end
													end
												end
											end
										end
									end
								end
							end
						end
					end

					return result
				end

               local function getChunk12Diagnostics()
					local probe = {
						missionType = "nil",
						missionCmdsType = "nil",
                       missionKeys = {},
						missionCmdsKeys = {},
						beaconEntries = {},
                       keyHits = {},
                        tankerTaskEntries = {},
                        waypointSamples = {},
                   runtimeRadioDevices = {},
					runtimeRadioChannels = {},
                   missionRadioChannels = {},
					playerMissionRadioChannels = {},
					runtimeCmdsHits = {},
					payloadKeys = {},
						playerGroup = "",
                      playerUnitId = 0,
						playerUnitName = "",
						playerCallsign = "",
						playerGroupMatchReason = "",
						playerGroupWaypoints = {},
						playerGroupRouteFound = false,
						bullseyeX = 0,
						bullseyeY = 0,
						bullseyeCoalition = "",
						bullseyeValid = false,
                       weatherType = "nil",
						weatherKeys = {},
						weatherSummary = {},
					}

					local diagnosticsDebug = base.vaicom.state and base.vaicom.state.debugmode

					local function tryget(fn)
						local ok, value = base.pcall(fn)
						if ok then return value end
						return nil
					end

				local function addProbeRow(target, value, maxItems)
					if base.type(target) ~= "table" then return end
					if #target >= (maxItems or 40) then return end
					base.table.insert(target, base.tostring(value))
				end

				local function copyStringList(src)
					local result = {}
					if base.type(src) ~= "table" then
						return result
					end
					for i, v in base.pairs(src) do
						result[i] = base.tostring(v)
					end
					return result
				end

				local function tryCallMethod(obj, methodName, arg)
					if obj == nil then return nil end
					local fn = tryget(function() return obj[methodName] end)
					if base.type(fn) ~= "function" then return nil end
					local ok, value = base.pcall(function()
						if arg ~= nil then
							return fn(obj, arg)
						end
						return fn(obj)
					end)
					if ok then return value end
					return nil
				end

				local function scanKeywordTree(root, path, depth, maxDepth, target, maxHits, keywords, seen)
					if base.type(root) ~= "table" then return end
					if depth > maxDepth or #target >= maxHits then return end
					if seen[root] then return end
					seen[root] = true

					for k, v in base.pairs(root) do
						if #target >= maxHits then break end
						local key = base.tostring(k)
						local lowerKey = base.string.lower(key)
						local keyMatch = false
						for _, kw in base.pairs(keywords) do
							if base.string.find(lowerKey, kw, 1, true) then
								keyMatch = true
								break
							end
						end

						local valueType = base.type(v)
						if keyMatch then
							if valueType == "string" or valueType == "number" or valueType == "boolean" then
								addProbeRow(target, path.."."..key.."="..base.tostring(v), maxHits)
							else
								addProbeRow(target, path.."."..key.."("..valueType..")", maxHits)
							end
						end

						if valueType == "table" then
							scanKeywordTree(v, path.."."..key, depth + 1, maxDepth, target, maxHits, keywords, seen)
						end
					end
				end

				local function collectRuntimeRadioProbe()
					if base.type(data) ~= "table" or base.type(data.communicators) ~= "table" then
						return
					end

					local deviceCount = 0
					for n, k in base.pairs(data.communicators) do
						if #probe.runtimeRadioDevices >= 24 then break end
						deviceCount = deviceCount + 1
						if deviceCount > 24 then break end

						local dev = tryget(function() return base.GetDevice(n) end)
						local name = k and k.displayName or ""
						local freq = tryCallMethod(dev, "get_frequency") or tryCallMethod(dev, "getFrequency")
						local mod = tryCallMethod(dev, "get_modulation") or tryCallMethod(dev, "getModulation")
						local on = tryCallMethod(dev, "is_on")
						addProbeRow(
							probe.runtimeRadioDevices,
							"dev="..base.tostring(n)
							.."|name="..base.tostring(name)
							.."|type="..base.type(dev)
							.."|on="..base.tostring(on)
							.."|freq="..base.tostring(freq)
							.."|mod="..base.tostring(mod),
							24
						)

                      local commChannelTable = k and (k.channels or k.Channels or k.presets or k.Presets) or nil
						local commChannelNames = k and (k.channelsNames or k.channelNames or k.ChannelsNames or k.names or k.Names) or nil
						if base.type(commChannelTable) == "table" then
							for chIdx, chValue in base.pairs(commChannelTable) do
								if #probe.runtimeRadioChannels >= 80 then break end
								local chName = ""
								if base.type(commChannelNames) == "table" then
									chName = base.tostring(commChannelNames[chIdx] or "")
								end
								addProbeRow(
									probe.runtimeRadioChannels,
									"src=communicator|dev="..base.tostring(n)
									.."|idx="..base.tostring(chIdx)
									.."|freq="..base.tostring(chValue)
									.."|name="..chName,
									80
								)
							end
						end

						if dev ~= nil then
							for _, methodName in base.pairs({
								"get_channel", "getChannel", "get_selected_channel", "getSelectedChannel",
								"get_preset", "getPreset", "get_channel_count", "getChannelCount",
								"count_channels", "countChannels", "get_preset_count", "getPresetCount",
								"get_channel_frequency", "getChannelFrequency", "get_frequency_for_channel", "getFrequencyForChannel"
							}) do
								local mv = tryget(function() return dev[methodName] end)
								if mv ~= nil then
									addProbeRow(probe.runtimeRadioChannels, "dev="..base.tostring(n).."|method="..methodName.."|type="..base.type(mv), 80)
								end
							end

							local count = tryCallMethod(dev, "get_channel_count")
							if count == nil then count = tryCallMethod(dev, "getChannelCount") end
							if count == nil then count = tryCallMethod(dev, "count_channels") end
							if count == nil then count = tryCallMethod(dev, "countChannels") end
							if count == nil then count = tryCallMethod(dev, "get_preset_count") end
							if count == nil then count = tryCallMethod(dev, "getPresetCount") end

							local channelCount = base.tonumber(count)
							if channelCount ~= nil and channelCount > 0 then
								local maxIdx = base.math.min(base.math.floor(channelCount) - 1, 39)
								for idx = 0, maxIdx do
									if #probe.runtimeRadioChannels >= 80 then break end
									local ch = tryCallMethod(dev, "get_channel", idx)
									if ch == nil then ch = tryCallMethod(dev, "getChannel", idx) end
									if ch == nil then ch = tryCallMethod(dev, "get_preset", idx) end
									if ch == nil then ch = tryCallMethod(dev, "getPreset", idx) end

									local chFreq = tryCallMethod(dev, "get_channel_frequency", idx)
									if chFreq == nil then chFreq = tryCallMethod(dev, "getChannelFrequency", idx) end
									if chFreq == nil then chFreq = tryCallMethod(dev, "get_frequency_for_channel", idx) end
									if chFreq == nil then chFreq = tryCallMethod(dev, "getFrequencyForChannel", idx) end

									if ch ~= nil or chFreq ~= nil then
										addProbeRow(
											probe.runtimeRadioChannels,
											"dev="..base.tostring(n)
											.."|idx="..base.tostring(idx)
											.."|ch="..base.tostring(ch)
											.."|freq="..base.tostring(chFreq),
											80
										)
									end
								end
							end
						end
					end
				end

				local function collectRuntimeCmdsProbe()
					local payload = tryget(function() return base.vaicom.state and base.vaicom.state.payload end)
					if base.type(payload) == "table" then
						local keyCount = 0
						for k, _ in base.pairs(payload) do
                            if base.string.lower(base.tostring(k)) ~= "cannon" then
								keyCount = keyCount + 1
								if keyCount > 24 then break end
								addProbeRow(probe.payloadKeys, base.tostring(k), 24)
							end
						end
					end

                   local cmdKeywords = { "cmds", "countermeasure", "cms", "program", "chaff", "flare", "ecm", "jammer" }
					scanKeywordTree(payload, "payload", 0, 7, probe.runtimeCmdsHits, 80, cmdKeywords, {})
					if base.type(payload) == "table" then
						scanKeywordTree(payload.Stations, "payload.Stations", 0, 6, probe.runtimeCmdsHits, 80, cmdKeywords, {})
						scanKeywordTree(payload.CurrentStation, "payload.CurrentStation", 0, 4, probe.runtimeCmdsHits, 80, cmdKeywords, {})
					end
				end

				local function collectMissionRadioChannelLists(root)
					local coal = root and root.coalition
					if base.type(coal) ~= "table" then return end

					local function normalizeName(v)
						local s = base.tostring(v or "")
						s = base.string.gsub(s, "^%s+", "")
						s = base.string.gsub(s, "%s+$", "")
						return s
					end

					local playerGroupKey = base.string.upper(normalizeName(probe.playerGroup))

					for coalName, coalData in base.pairs(coal) do
						if #probe.missionRadioChannels >= 120 and #probe.playerMissionRadioChannels >= 80 then break end
						if base.type(coalData) == "table" and base.type(coalData.country) == "table" then
							for _, country in base.pairs(coalData.country) do
								if #probe.missionRadioChannels >= 120 and #probe.playerMissionRadioChannels >= 80 then break end
								for _, catName in base.pairs({"plane", "helicopter", "ship"}) do
									local cat = country and country[catName]
									if base.type(cat) == "table" and base.type(cat.group) == "table" then
										for _, group in base.pairs(cat.group) do
											if #probe.missionRadioChannels >= 120 and #probe.playerMissionRadioChannels >= 80 then break end
											local groupName = normalizeName(group and group.name)
											local groupTag = "coal="..base.tostring(coalName).."|group="..groupName
											local groupKey = base.string.upper(groupName)
											local isPlayerGroup = (playerGroupKey ~= "" and groupKey == playerGroupKey) or groupName == "- Player"
											local units = group and group.units
											if base.type(units) == "table" then
												for uidx, unitObj in base.pairs(units) do
													if #probe.missionRadioChannels >= 120 and #probe.playerMissionRadioChannels >= 80 then break end
													local unitName = normalizeName(unitObj and unitObj.name)
													local radios = unitObj and (unitObj.Radio or unitObj.radio)
													if base.type(radios) == "table" then
														for ridx, radioObj in base.pairs(radios) do
															if #probe.missionRadioChannels >= 120 and #probe.playerMissionRadioChannels >= 80 then break end
															local channels = radioObj and (radioObj.channels or radioObj.Channels)
															local names = radioObj and (radioObj.channelsNames or radioObj.channelNames or radioObj.ChannelsNames)
															if base.type(channels) == "table" then
																for chIdx, chFreq in base.pairs(channels) do
																	local chName = ""
																	if base.type(names) == "table" then
																		chName = normalizeName(names[chIdx])
																	end
																	local row = groupTag
																		.."|unitIdx="..base.tostring(uidx)
																		.."|unit="..unitName
																		.."|radio="..base.tostring(ridx)
																		.."|ch="..base.tostring(chIdx)
																		.."|freq="..base.tostring(chFreq)
																		.."|name="..chName

																	if #probe.missionRadioChannels < 120 then
																		addProbeRow(probe.missionRadioChannels, row, 120)
																	end
																	if isPlayerGroup and #probe.playerMissionRadioChannels < 80 then
																		addProbeRow(probe.playerMissionRadioChannels, row, 80)
																	end
																end
															end
														end
													end
												end
											end
										end
									end
								end
							end
						end
					end
				end

                    local missionObj = getMissionObject()

                    local missionCmdsObj = tryget(function() return base.vaicom.state and base.vaicom.state.missioncmds end)
					if missionCmdsObj == nil then missionCmdsObj = tryget(function() return base.vaicom.state and base.vaicom.state.mission end) end
					if missionCmdsObj == nil then missionCmdsObj = tryget(function() return base.missionCommands end) end

					probe.missionType = base.type(missionObj)
					probe.missionCmdsType = base.type(missionCmdsObj)

					if base.type(missionCmdsObj) == "table" then
						local keyCount = 0
						for k,_ in base.pairs(missionCmdsObj) do
							keyCount = keyCount + 1
							if keyCount > 10 then break end
							base.table.insert(probe.missionCmdsKeys, base.tostring(k))
						end
					end

					local diagCache = base.vaicom.state and base.vaicom.state.okb_diagcache
					if base.type(diagCache) ~= "table" then
						diagCache = {}
						if base.vaicom.state then
							base.vaicom.state.okb_diagcache = diagCache
						end
					end

					local nowTick = base.tonumber(base.vaicom.state and base.vaicom.state.timer) or 0
					local shouldRefreshStatic = nowTick - (diagCache.staticAt or -10000) >= 86400
					local missionRef = base.tostring(missionObj)
					if diagCache.missionRef ~= missionRef then
						diagCache.missionRef = missionRef
						diagCache.staticAt = -10000
						diagCache.payloadAt = -10000
						diagCache.waypointAt = -10000
						diagCache.radioAt = -10000
						diagCache.weatherAt = -10000
						diagCache.bullseyeAt = -10000
						diagCache.runtimeRadioDevices = {}
						diagCache.runtimeRadioChannels = {}
						diagCache.payloadKeys = {}
						diagCache.runtimeCmdsHits = {}
						diagCache.missionKeys = {}
						diagCache.beaconEntries = {}
						diagCache.keyHits = {}
						diagCache.tankerTaskEntries = {}
						diagCache.waypointSamples = {}
						diagCache.playerGroupWaypoints = {}
						diagCache.missionRadioChannels = {}
						diagCache.playerMissionRadioChannels = {}
						diagCache.weatherType = "nil"
						diagCache.weatherKeys = {}
						diagCache.weatherSummary = {}
						diagCache.playerGroup = ""
						diagCache.playerUnitId = 0
						diagCache.playerUnitName = ""
						diagCache.playerCallsign = ""
						diagCache.playerGroupMatchReason = ""
						diagCache.playerGroupRouteFound = false
						diagCache.bullseyeX = 0
						diagCache.bullseyeY = 0
						diagCache.bullseyeCoalition = ""
						diagCache.bullseyeValid = false
					end

					local function mapCoalitionSideToText(side)
						if side == (base.coalition and base.coalition.side and base.coalition.side.BLUE) then return "blue" end
						if side == (base.coalition and base.coalition.side and base.coalition.side.RED) then return "red" end
						if side == (base.coalition and base.coalition.side and base.coalition.side.NEUTRAL) then return "neutral" end
						return ""
					end

					local function resolvePlayerCoalitionSide()
						local side = tryget(function()
							if base.DCS and base.DCS.getPlayerCoalition then
								return base.DCS.getPlayerCoalition()
							end
							return nil
						end)
						if base.type(side) == "number" and side > 0 then
							return side
						end

						local stateCoal = base.vaicom and base.vaicom.state and base.vaicom.state.playercoalition
						if base.type(stateCoal) == "number" and stateCoal > 0 then
							return stateCoal
						end

						local text = base.string.upper(base.tostring(stateCoal or ""))
						if text == "BLUE" then return base.coalition and base.coalition.side and base.coalition.side.BLUE end
						if text == "RED" then return base.coalition and base.coalition.side and base.coalition.side.RED end
						if text == "NEUTRAL" then return base.coalition and base.coalition.side and base.coalition.side.NEUTRAL end
						return nil
					end

					local function collectBullseyePoint()
						probe.bullseyeX = 0
						probe.bullseyeY = 0
						probe.bullseyeCoalition = ""
						probe.bullseyeValid = false

						local side = resolvePlayerCoalitionSide()
						probe.bullseyeCoalition = mapCoalitionSideToText(side)
						if side == nil then return end
						if base.coalition == nil or base.coalition.getMainRefPoint == nil then return end

						local bulls = tryget(function()
							return base.coalition.getMainRefPoint(side)
						end)
						if base.type(bulls) ~= "table" then return end

						local bx = base.tonumber(bulls.x)
						local by = base.tonumber(bulls.z)
						if bx == nil or by == nil then return end

						probe.bullseyeX = bx
						probe.bullseyeY = by
						probe.bullseyeValid = true
					end

					if shouldRefreshStatic then
						probe.runtimeRadioDevices = {}
						probe.runtimeRadioChannels = {}
						probe.missionKeys = {}
						probe.beaconEntries = {}
						probe.keyHits = {}
						probe.tankerTaskEntries = {}

						collectRuntimeRadioProbe()

						diagCache.runtimeRadioDevices = copyStringList(probe.runtimeRadioDevices)
						diagCache.runtimeRadioChannels = copyStringList(probe.runtimeRadioChannels)
					else
						probe.runtimeRadioDevices = copyStringList(diagCache.runtimeRadioDevices)
						probe.runtimeRadioChannels = copyStringList(diagCache.runtimeRadioChannels)
					end

					if diagCache.bullseyeValid then
						probe.bullseyeX = base.tonumber(diagCache.bullseyeX) or 0
						probe.bullseyeY = base.tonumber(diagCache.bullseyeY) or 0
						probe.bullseyeCoalition = base.tostring(diagCache.bullseyeCoalition or "")
						probe.bullseyeValid = true
					else
						local bullseyeIntervalSec = 1.0
						if nowTick - (diagCache.bullseyeAt or -10000) >= bullseyeIntervalSec then
							collectBullseyePoint()
							diagCache.bullseyeAt = nowTick
							diagCache.bullseyeX = probe.bullseyeX or 0
							diagCache.bullseyeY = probe.bullseyeY or 0
							diagCache.bullseyeCoalition = base.tostring(probe.bullseyeCoalition or "")
							diagCache.bullseyeValid = probe.bullseyeValid and true or false
						else
							probe.bullseyeX = base.tonumber(diagCache.bullseyeX) or 0
							probe.bullseyeY = base.tonumber(diagCache.bullseyeY) or 0
							probe.bullseyeCoalition = base.tostring(diagCache.bullseyeCoalition or "")
							probe.bullseyeValid = diagCache.bullseyeValid and true or false
						end
					end

					local payloadIntervalSec = 1.0
					if nowTick - (diagCache.payloadAt or -10000) >= payloadIntervalSec then
						probe.payloadKeys = {}
						probe.runtimeCmdsHits = {}
						collectRuntimeCmdsProbe()
						diagCache.payloadAt = nowTick
						diagCache.payloadKeys = copyStringList(probe.payloadKeys)
						diagCache.runtimeCmdsHits = copyStringList(probe.runtimeCmdsHits)
					else
						probe.payloadKeys = copyStringList(diagCache.payloadKeys)
						probe.runtimeCmdsHits = copyStringList(diagCache.runtimeCmdsHits)
					end

                    if base.type(missionObj) == "table" then
						if shouldRefreshStatic then
							local missionKeyCount = 0
							for k,_ in base.pairs(missionObj) do
								missionKeyCount = missionKeyCount + 1
								if missionKeyCount > 20 then break end
								base.table.insert(probe.missionKeys, base.tostring(k))
							end
							diagCache.missionKeys = copyStringList(probe.missionKeys)
						else
							probe.missionKeys = copyStringList(diagCache.missionKeys)
						end

						local function scan(obj, path, depth)
                            if base.type(obj) ~= "table" or depth > 12 or #probe.beaconEntries >= 40 then
								return
							end

							for k,v in base.pairs(obj) do
                                if #probe.beaconEntries >= 40 then break end
								if base.type(v) == "table" then
                                   local id = v.id
									local params = v.params or {}

									if id == "WrappedAction" and params.action and base.type(params.action) == "table" then
										id = params.action.id
										params = params.action.params or {}
									elseif v.action and base.type(v.action) == "table" then
										id = v.action.id or id
										if id ~= nil then
											params = v.action.params or params
										end
									end

									local ch = params.channel or params.Channel or params.channelNumber
									local band = params.modeChannel or params.band or params.mode
									if id == "ActivateBeacon" or ch ~= nil then
										base.table.insert(probe.beaconEntries,
											path.."."..base.tostring(k).."|id="..base.tostring(id).."|ch="..base.tostring(ch).."|band="..base.tostring(band).."|name="..base.tostring(params.callsign or params.name))
									end
									scan(v, path.."."..base.tostring(k), depth + 1)
								end
							end
						end

						local function scanKeys(obj, path, depth)
                          if base.type(obj) ~= "table" or depth > 12 or #probe.keyHits >= 80 then
								return
							end
							for k,v in base.pairs(obj) do
                              if #probe.keyHits >= 80 then break end
								local ks = base.string.lower(base.tostring(k))
								if base.string.find(ks, "beacon", 1, true) or base.string.find(ks, "tacan", 1, true) or base.string.find(ks, "channel", 1, true) or base.string.find(ks, "mode", 1, true) then
									local vt = base.type(v)
									if vt == "string" or vt == "number" or vt == "boolean" then
										base.table.insert(probe.keyHits, path.."."..base.tostring(k).."="..base.tostring(v))
									else
										base.table.insert(probe.keyHits, path.."."..base.tostring(k).."("..vt..")")
									end
								end
								if base.type(v) == "table" then
									scanKeys(v, path.."."..base.tostring(k), depth + 1)
								end
							end
						end

						local function collectTankerTasks(root)
							local coal = root and root.coalition
							if base.type(coal) ~= "table" then return end

							local function looksLikeTankerGroup(group)
								if base.type(group) ~= "table" then return false end
								if group.task == "Refueling" then return true end
								local gname = base.string.lower(base.tostring(group.name or ""))
								if base.string.find(gname, "texaco", 1, true) or base.string.find(gname, "arco", 1, true) or base.string.find(gname, "shell", 1, true) then
									return true
								end
								local units = group.units
								if base.type(units) == "table" then
									for _,u in base.pairs(units) do
										local utype = base.string.lower(base.tostring(u and u.type or ""))
										if base.string.find(utype, "kc-135", 1, true) or base.string.find(utype, "il-78", 1, true) or base.string.find(utype, "s-3b", 1, true) then
											return true
										end
									end
								end
								return false
							end

							local function pushTask(groupLabel, pointIdx, taskObj)
								if #probe.tankerTaskEntries >= 60 then return end
								local id = taskObj and taskObj.id
								local params = taskObj and taskObj.params or {}
								if id == "WrappedAction" and params and base.type(params.action) == "table" then
									id = params.action.id
									params = params.action.params or {}
								elseif taskObj and taskObj.action and base.type(taskObj.action) == "table" then
									id = taskObj.action.id or id
									params = taskObj.action.params or params
								end

								local ch = params and (params.channel or params.Channel or params.channelNumber) or nil
								local band = params and (params.modeChannel or params.band or params.mode) or nil
								local name = params and (params.callsign or params.name) or nil
								base.table.insert(probe.tankerTaskEntries,
									groupLabel.."|pt="..base.tostring(pointIdx).."|id="..base.tostring(id).."|ch="..base.tostring(ch).."|band="..base.tostring(band).."|name="..base.tostring(name))
							end

							for coalName, coalData in base.pairs(coal) do
								if #probe.tankerTaskEntries >= 60 then break end
								if base.type(coalData) == "table" and base.type(coalData.country) == "table" then
									for _, country in base.pairs(coalData.country) do
										if #probe.tankerTaskEntries >= 60 then break end
										for _, catName in base.pairs({"plane", "helicopter"}) do
											local cat = country and country[catName]
											if base.type(cat) == "table" and base.type(cat.group) == "table" then
												for _, group in base.pairs(cat.group) do
													if #probe.tankerTaskEntries >= 60 then break end
													if looksLikeTankerGroup(group) then
														local label = "coal="..base.tostring(coalName).."|group="..base.tostring(group.name or "?")
														local points = group.route and group.route.points
														if base.type(points) == "table" then
															for pidx, p in base.pairs(points) do
																if #probe.tankerTaskEntries >= 60 then break end
																local tasks = p and p.task and p.task.params and p.task.params.tasks
																if base.type(tasks) == "table" then
																	for _, t in base.pairs(tasks) do
																		pushTask(label, pidx, t)
																	end
																end
															end
														end
													end
												end
											end
										end
									end
								end
							end
						end

						local function isFiniteNumber(v)
							if v == nil then return false end
							local n = base.tonumber(v)
							if n == nil then return false end
							if n ~= n then return false end
							if n == base.math.huge or n == -base.math.huge then return false end
							return true
						end

						local function getRoutePoints(group)
							if base.type(group) ~= "table" then return nil end
							if base.type(group.route) == "table" and base.type(group.route.points) == "table" then
								return group.route.points
							end
							return nil
						end

						local function appendWaypointRow(target, tag, pidx, point)
							if base.type(target) ~= "table" then return end
							if #target >= 24 then return end
							if base.type(point) ~= "table" then return end

							local x = point.x
							local y = point.y
							local alt = point.alt
							if not isFiniteNumber(x) and not isFiniteNumber(y) and not isFiniteNumber(alt) then return end

							local ptype = point.type or point.action or point.name or ""
							base.table.insert(target,
								base.tostring(tag)
								.."|pt="..base.tostring(pidx)
								.."|x="..base.tostring(x)
								.."|y="..base.tostring(y)
								.."|alt="..base.tostring(alt)
								.."|spd="..base.tostring(point.speed)
								.."|eta="..base.tostring(point.ETA)
								.."|task="..base.tostring(ptype)
							)
						end

						local function collectMissionWaypointSamples(root)
							local coal = root and root.coalition
							if base.type(coal) ~= "table" then return end

                          local function normalizeName(v)
								local s = base.tostring(v or "")
								s = base.string.gsub(s, "^%s+", "")
								s = base.string.gsub(s, "%s+$", "")
								return s
							end

							local function normalizeKey(v)
								local s = normalizeName(v)
								s = base.string.upper(s)
								s = base.string.gsub(s, "[^A-Z0-9]", "")
								return s
							end

							local function getPlayerInfo()
								local info = {
									groupName = "",
									unitId = 0,
									unitName = "",
									callsign = "",
								}

								if data and data.pUnit then
									local okId, valueId = base.pcall(function() return data.pUnit.id_ end)
									if okId and valueId ~= nil then
										local n = base.tonumber(valueId) or 0
										info.unitId = n
									end

									if data.pUnit.getName then
										local okName, valueName = base.pcall(function() return data.pUnit:getName() end)
										if okName then
											info.unitName = normalizeName(valueName)
										end
									end

									if data.pUnit.getCallsign then
										local okCall, valueCall = base.pcall(function() return data.pUnit:getCallsign() end)
										if okCall then
											info.callsign = normalizeName(valueCall)
										end
									end
								end

								info.groupName = normalizeName(base.vaicom.state and base.vaicom.state.playergroupname)
								if info.groupName == "" and data and data.pUnit and data.pUnit.getGroup then
									local okG, groupObj = base.pcall(function() return data.pUnit:getGroup() end)
									if okG and groupObj and groupObj.getName then
										local okGN, gname = base.pcall(function() return groupObj:getName() end)
										if okGN then
											info.groupName = normalizeName(gname)
										end
									end
								end

								return info
							end

							local function getMissionUnitId(unitObj)
								if base.type(unitObj) ~= "table" then return 0 end
								local raw = unitObj.unitId or unitObj.unit_id or unitObj.id or unitObj.id_
								local n = base.tonumber(raw)
								if n == nil then return 0 end
								return n
							end

							local function getMissionUnitName(unitObj)
								if base.type(unitObj) ~= "table" then return "" end
								return normalizeName(unitObj.name or unitObj.unitName or "")
							end

							local function getMissionUnitCallsign(unitObj)
								if base.type(unitObj) ~= "table" then return "" end
								local c = unitObj.callsign
								if base.type(c) == "string" or base.type(c) == "number" then
									return normalizeName(c)
								end
								if base.type(c) == "table" then
									if c.name ~= nil then return normalizeName(c.name) end
									if c.callsign ~= nil then return normalizeName(c.callsign) end
									if c[1] ~= nil then return normalizeName(c[1]) end
								end
								return ""
							end

                            local player = getPlayerInfo()
							local playerGroupName = player.groupName
							probe.playerGroup = playerGroupName
							probe.playerUnitId = player.unitId or 0
							probe.playerUnitName = player.unitName or ""
							probe.playerCallsign = player.callsign or ""

							local playerUnitId = base.tonumber(player.unitId) or 0
							local playerUnitNameKey = normalizeKey(player.unitName)
							local playerCallsignKey = normalizeKey(player.callsign)

							local function groupMatchesPlayer(group, gname)
								if playerGroupName ~= "" and gname ~= "" and gname == playerGroupName then
									return true, "group-name"
								end

								if base.type(group) ~= "table" or base.type(group.units) ~= "table" then
									return false, ""
								end

								for _, unitObj in base.pairs(group.units) do
									if playerUnitId > 0 then
										local missionUnitId = getMissionUnitId(unitObj)
										if missionUnitId > 0 and missionUnitId == playerUnitId then
											return true, "unit-id"
										end
									end

									if playerUnitNameKey ~= "" then
										local missionUnitNameKey = normalizeKey(getMissionUnitName(unitObj))
										if missionUnitNameKey ~= "" and missionUnitNameKey == playerUnitNameKey then
											return true, "unit-name"
										end
									end

									if playerCallsignKey ~= "" then
										local missionCallsignKey = normalizeKey(getMissionUnitCallsign(unitObj))
										if missionCallsignKey ~= "" and missionCallsignKey == playerCallsignKey then
											return true, "callsign"
										end
									end
								end

								return false, ""
							end

							for coalName, coalData in base.pairs(coal) do
								if #probe.waypointSamples >= 24 and #probe.playerGroupWaypoints >= 24 then break end
								if base.type(coalData) == "table" and base.type(coalData.country) == "table" then
									for _, country in base.pairs(coalData.country) do
										if #probe.waypointSamples >= 24 and #probe.playerGroupWaypoints >= 24 then break end
										for _, catName in base.pairs({"plane", "helicopter", "ship"}) do
											local cat = country and country[catName]
											if base.type(cat) == "table" and base.type(cat.group) == "table" then
												for _, group in base.pairs(cat.group) do
													local routePoints = getRoutePoints(group)
													if base.type(routePoints) == "table" then
                                                       local gname = normalizeName(group.name)
														local tag = "coal="..base.tostring(coalName).."|group="..gname

                                                      local isPlayerGroup, matchReason = groupMatchesPlayer(group, gname)
														if isPlayerGroup then
															probe.playerGroupRouteFound = true
                                                         if probe.playerGroup == "" then
																probe.playerGroup = gname
															end
															if probe.playerGroupMatchReason == "" then
																probe.playerGroupMatchReason = matchReason
															end
														end

														for pidx, point in base.pairs(routePoints) do
															if #probe.waypointSamples < 24 then
																appendWaypointRow(probe.waypointSamples, tag, pidx, point)
															end
															if isPlayerGroup and #probe.playerGroupWaypoints < 24 then
																appendWaypointRow(probe.playerGroupWaypoints, tag, pidx, point)
															end
														end
													end
													if #probe.waypointSamples >= 24 and #probe.playerGroupWaypoints >= 24 then break end
												end
											end
										end
									end
								end
							end
						end

						if shouldRefreshStatic then
							scan(missionObj, "mission", 0)
							scanKeys(missionObj, "mission", 0)
							collectTankerTasks(missionObj)
							diagCache.beaconEntries = copyStringList(probe.beaconEntries)
							diagCache.keyHits = copyStringList(probe.keyHits)
							diagCache.tankerTaskEntries = copyStringList(probe.tankerTaskEntries)
						else
							probe.beaconEntries = copyStringList(diagCache.beaconEntries)
							probe.keyHits = copyStringList(diagCache.keyHits)
							probe.tankerTaskEntries = copyStringList(diagCache.tankerTaskEntries)
						end

						if shouldRefreshStatic then
							diagCache.staticAt = nowTick
						end

						local function collectWeatherSummary(root)
							probe.weatherType = "nil"
							probe.weatherKeys = {}
							probe.weatherSummary = {}
							local weather = root and root.weather
							probe.weatherType = base.type(weather)
							if base.type(weather) ~= "table" then
								return
							end

							local wk = 0
							for k,_ in base.pairs(weather) do
								wk = wk + 1
								if wk > 20 then break end
								base.table.insert(probe.weatherKeys, base.tostring(k))
							end

							local function addWeather(path, v)
								if #probe.weatherSummary >= 40 then return end
								local vt = base.type(v)
								if vt == "string" or vt == "number" or vt == "boolean" then
									base.table.insert(probe.weatherSummary, path.."="..base.tostring(v))
								elseif vt == "table" then
									base.table.insert(probe.weatherSummary, path.."(table)")
								else
									base.table.insert(probe.weatherSummary, path.."("..vt..")")
								end
							end

							addWeather("mission.weather.atmosphere_type", weather.atmosphere_type)
							addWeather("mission.weather.qnh", weather.qnh)
							addWeather("mission.weather.season.temperature", weather.season and weather.season.temperature)
							addWeather("mission.weather.visibility.distance", weather.visibility and weather.visibility.distance)
							addWeather("mission.weather.wind.atGround.speed", weather.wind and weather.wind.atGround and weather.wind.atGround.speed)
							addWeather("mission.weather.wind.atGround.dir", weather.wind and weather.wind.atGround and weather.wind.atGround.dir)
							addWeather("mission.weather.wind.at2000.speed", weather.wind and weather.wind.at2000 and weather.wind.at2000.speed)
							addWeather("mission.weather.wind.at2000.dir", weather.wind and weather.wind.at2000 and weather.wind.at2000.dir)
							addWeather("mission.weather.wind.at8000.speed", weather.wind and weather.wind.at8000 and weather.wind.at8000.speed)
							addWeather("mission.weather.wind.at8000.dir", weather.wind and weather.wind.at8000 and weather.wind.at8000.dir)
							addWeather("mission.weather.clouds.base", weather.clouds and weather.clouds.base)
							addWeather("mission.weather.clouds.density", weather.clouds and weather.clouds.density)
							addWeather("mission.weather.clouds.thickness", weather.clouds and weather.clouds.thickness)
							addWeather("mission.weather.fog.visibility", weather.fog and weather.fog.visibility)
							addWeather("mission.weather.enable_fog", weather.enable_fog)
							addWeather("mission.weather.enable_dust", weather.enable_dust)
							addWeather("mission.weather.dust_density", weather.dust_density)
							addWeather("mission.weather.groundTurbulence", weather.groundTurbulence)
						end

						local waypointIntervalSec = 1.0
						if nowTick - (diagCache.waypointAt or -10000) >= waypointIntervalSec then
							collectMissionWaypointSamples(missionObj)
							diagCache.waypointAt = nowTick
							diagCache.waypointSamples = copyStringList(probe.waypointSamples)
							diagCache.playerGroupWaypoints = copyStringList(probe.playerGroupWaypoints)
							diagCache.playerGroup = probe.playerGroup or ""
							diagCache.playerUnitId = probe.playerUnitId or 0
							diagCache.playerUnitName = probe.playerUnitName or ""
							diagCache.playerCallsign = probe.playerCallsign or ""
							diagCache.playerGroupMatchReason = probe.playerGroupMatchReason or ""
							diagCache.playerGroupRouteFound = probe.playerGroupRouteFound and true or false
						else
							probe.waypointSamples = copyStringList(diagCache.waypointSamples)
							probe.playerGroupWaypoints = copyStringList(diagCache.playerGroupWaypoints)
							probe.playerGroup = diagCache.playerGroup or ""
							probe.playerUnitId = diagCache.playerUnitId or 0
							probe.playerUnitName = diagCache.playerUnitName or ""
							probe.playerCallsign = diagCache.playerCallsign or ""
							probe.playerGroupMatchReason = diagCache.playerGroupMatchReason or ""
							probe.playerGroupRouteFound = diagCache.playerGroupRouteFound and true or false
						end

						local radioIntervalSec = 1.0
						if nowTick - (diagCache.radioAt or -10000) >= radioIntervalSec then
							probe.missionRadioChannels = {}
							probe.playerMissionRadioChannels = {}
							collectMissionRadioChannelLists(missionObj)
							diagCache.radioAt = nowTick
							diagCache.missionRadioChannels = copyStringList(probe.missionRadioChannels)
							diagCache.playerMissionRadioChannels = copyStringList(probe.playerMissionRadioChannels)
						else
							probe.missionRadioChannels = copyStringList(diagCache.missionRadioChannels)
							probe.playerMissionRadioChannels = copyStringList(diagCache.playerMissionRadioChannels)
						end

						if nowTick - (diagCache.weatherAt or -10000) >= 86400 then
							collectWeatherSummary(missionObj)
							diagCache.weatherAt = nowTick
							diagCache.weatherType = probe.weatherType
							diagCache.weatherKeys = copyStringList(probe.weatherKeys)
							diagCache.weatherSummary = copyStringList(probe.weatherSummary)
						else
							probe.weatherType = diagCache.weatherType or "nil"
							probe.weatherKeys = copyStringList(diagCache.weatherKeys)
							probe.weatherSummary = copyStringList(diagCache.weatherSummary)
						end
					end

					return probe
				end

                local chunk = {}	
            local missionGroupTacanMap = buildMissionGroupTacanMap()
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
									selectedradio		= getSelectedRadio(base.vaicom.state.dcsid), 
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
                                   Opposition	= {},
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
									metar = buildAtcMetar(),
									atcmetars = buildAllAtcMetars(),
									atcicaotypes = buildAllAtcIcaoTypes(),
									diagnostics = getChunk12Diagnostics(),
								  }
				if base.vaicom and base.vaicom.state then
					base.vaicom.state.atcicaotypes = chunk[12].atcicaotypes
				end
				local selectedRadio = getSelectedRadio(base.vaicom.state.dcsid)
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
                                    isselected = radioNamesMatch(k.displayName, selectedRadio),
									intercom = ICS,
									on =  ICS or ((ICS_set and (( base.GetDevice(n) and base.GetDevice(n).is_on and base.GetDevice(n):is_on() ))) or false),
									frequency = ( ICS_set and (( (not ICS) and base.GetDevice(n) and base.GetDevice(n).get_frequency and base.GetDevice(n):get_frequency() ) or 0)) or 0,
									modulation = ( ICS_set and (( (not ICS) and base.GetDevice(n) and base.GetDevice(n).get_modulation and (((base.GetDevice(n):get_modulation() == 1) and "FM") or "AM") ) or "XX")) or "XX", 
									}						
					base.table.insert(chunk[2].radios, radio)
				end					
				for recipientclass,_ in base.pairs(base.vaicom.state.availablerecipients) do
                 for n,k in base.pairs(base.vaicom.state.availablerecipients[recipientclass]) do
					   local unitDiagnostics = ""
						if base.vaicom.state.debugmode and (recipientclass == "Tanker" or recipientclass == "ATC" or recipientclass == "AWACS" or recipientclass == "Flight") then
                            unitDiagnostics = base.tostring(base.vaicom.properties.unitdiagnostics(k))
						end
                        local tacanValue = normalizeTacanValue(base.vaicom.properties.tacan(k))
                       if tacanValue == "" and (recipientclass == "Tanker" or recipientclass == "ATC" or recipientclass == "AWACS" or recipientclass == "Flight") then
							local okGroup, groupObj = base.pcall(function() return k:getGroup() end)
							if okGroup and groupObj and groupObj.getName then
								local okName, groupName = base.pcall(function() return groupObj:getName() end)
                            if okName and groupName ~= nil then
                               tacanValue = resolveTacanFromGroupMap(groupName, missionGroupTacanMap)
								end
							end
						end
						local dcsunit = {
										index = n,
										id_ = base.vaicom.properties.id(k),
										callsign = base.tostring(base.vaicom.properties.missioncallsign(k)),
                                    typename = base.tostring(base.vaicom.properties.typename(k)),
										range = base.vaicom.properties.range(k),
										pos = base.vaicom.properties.pos(k),
										fullname = base.tostring(base.vaicom.properties.displayname(k)),
										coalition = base.tostring(base.vaicom.properties.coalition(k)),
										altfreq = base.vaicom.properties.altfreq(k),
                                      tacan = tacanValue,
                                      unitdiagnostics = unitDiagnostics,
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
				local function sendChunk(payload, chunkId)
					local ok, err = base.pcall(function()
						socket.try(base.vaicom.sender:send(payload))
					end)
					if not ok then
						for _, value in base.pairs(err) do
							base.env.error("VAICOM error sending chunk "..chunkId..", error: "..base.tostring(value))
						end
					end
				end
				local function addChunkHeader(tbl, cid)
					tbl.cid    = cid
					tbl.client = "VAICOMPRO"
					tbl.mode   = "normal"
					tbl.type   = "missiondata.update"
					return tbl
				end
				-- Maximum udp packet size for localhost
				-- (64K - 20 IP header - 8 UDP header)
				local maxSize = (64 * 1024) - 20 - 8
              for chunkId = 1, 12 do
					local chunkPayload = addChunkHeader(chunk[chunkId], chunkId)
					local payload = JSON:encode(chunkPayload)
					if chunkId == 9 then
						-- Large menus that exceed the maximum payload cause errors
						-- during sending and prevent sending all chunks to VAICOM.
						if #payload < maxSize then
							sendChunk(payload, chunkId)
						else
							-- Send empty menu items if menu too large
							local menuPayload = { menuaux = { items = {}, name = "other" } }
							chunkPayload = addChunkHeader(menuPayload, chunkId)
							payload = JSON:encode(chunkPayload)
							sendChunk(payload, chunkId)
						end
					else
						sendChunk(payload, chunkId)
					end
				end
			end,
}
base.vaicom.devicecontrol = {
	queue = {},
	busy = false,
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