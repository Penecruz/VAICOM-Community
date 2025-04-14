--Common part of all speech construction protocols

local base = _G

module('common')
__index = base.getfenv()


local p = base.require('phrase')
local u = base.require('utils')
local math = base.math
--local gettext = base.require("i_18n")
--local _ = gettext.translate
local require = base.require
require('i18n').setup(_M)

local print 	= base.print
local tostring 	= base.tostring
local tonumber  = base.tonumber

local  BIG_CALLSIGN_MIN = 1e9
local  SHT_CALLSIGN_MAX = 1e3

function callsignPack(group,flight,wingman)
	if group < 10 and flight  < 10 and wingman < 10 then
		return wingman + 10 * flight + 100 * group
	else
		return BIG_CALLSIGN_MIN  + wingman + 1000 * flight + 1000000 * group
	end
end


function callsignUnpack(packed)
	local group,flight,wingman
	if packed < SHT_CALLSIGN_MAX then
		group		 = math.floor(packed / 100)

		local flight_wingman = packed - group * 100
		
		flight 		 =  math.floor(flight_wingman / 10)
		wingman		 =  flight_wingman - flight * 10		
	else
		packed  	 = packed - BIG_CALLSIGN_MIN

		group		 = math.floor(packed/1000000)

		local flight_wingman = packed - group * 1000000
		
		flight 		 =  math.floor(flight_wingman / 1000)
		wingman		 =  flight_wingman - flight * 1000		
	end
	
	return group,flight,wingman
end


function callsignMinimize(callsign)
	if callsign < SHT_CALLSIGN_MAX then 
		return callsign
	end
	local g,f,w = callsignUnpack(callsign)
	local str   = tostring(g) ..  tostring(f) .. tostring(w)
	return tonumber(str);
end

local function callsignTest(c)
	print("----------------------------------------")
	print("CALLSIGN IS\t"..c)

	if c >= BIG_CALLSIGN_MIN then 
		print("CALLSIGN IS\tLONG")
	elseif c < SHT_CALLSIGN_MAX then
		print("CALLSIGN IS\tSHORT")
	else
		error("UNEXPECTED CALLSIGN")
	end

	local g,f,w    = callsignUnpack(c)

	print("UNPACK group\t"..tostring(g))
	print("UNPACK flight\t"..tostring(f))
	print("UNPACK wingman\t"..tostring(w))

	local packed = callsignPack(g,f,w)

	print("PACKED\t\t"..packed)

	if packed ~= c then
		error("MISMATCH")
	end

	print("MINIMIZE\t"..callsignMinimize(c))
end

--callsignTest(937)
--callsignTest(callsignPack(11,543,4))


function encodeCallsign(callsign)
	local g,f,w = callsignUnpack(callsign)
	f		 = math.max(1, f)
	w 		 = math.max(1, math.min(w,4))
	return g, f, w
end

--Game Modules

local defaultModuleName = 'Common'

local function getModuleByLocalPlayer()
	local localPlayer = base.world.getPlayer()
	if localPlayer ~= nil then
		return localPlayer:getTypeName()
	else
		return nil
	end
end

local function getModuleName(message)
	if message.event > base.Message.wMsgLeaderNull and message.event < base.Message.wMsgLeaderMaximum then
		return message.sender:getUnit():getTypeName()
	elseif message.event > base.Message.wMsgWingmenNull and message.event < base.Message.wMsgWingmenMaximum then
		return message.sender:getUnit():getTypeName()
	elseif message.event > base.Message.wMsgServiceNull and message.event < base.Message.wMsgGroundCrewNull then
		return message.receiver:getUnit():getTypeName()
	else
		return getModuleByLocalPlayer() or defaultModuleName
	end
end



local function hasNumericCallsign(pUnit)
	local country = pUnit:getCountry()
	--local forcesName = pUnit:getForcesName()
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


--Units

local unitSystems = {
	metric = {
		distance = u.units.km,
		altitude = u.units.m,
		velocity = u.units.kmh,
		verticalVelocity = u.units.ms,
		pressure = u.units.mmHg
	},
	imperial = {
		distance = u.units.nm,
		altitude = u.units.feet,
		velocity = u.units.kts,
		verticalVelocity = u.units.fpm,
		pressure = u.units.im
	}
}

--by some reason at mission execution  time  table coutry is chnaged in format , save  original  as reference

local  nations  = base.country.id

unitSystemByCountry = {
	[nations.RUSSIA]			= unitSystems.metric,
	[nations.UKRAINE]			= unitSystems.metric,
	[nations.USA]				= unitSystems.imperial,
	[nations.TURKEY]			= unitSystems.imperial,
	[nations.UK]				= unitSystems.imperial,
	[nations.FRANCE]			= unitSystems.imperial,
	[nations.GERMANY]			= unitSystems.imperial,
	[nations.CANADA]			= unitSystems.imperial,
	[nations.SPAIN]				= unitSystems.imperial,
	[nations.THE_NETHERLANDS]	= unitSystems.imperial,
	[nations.BELGIUM]			= unitSystems.imperial,
	[nations.NORWAY]			= unitSystems.imperial,
	[nations.DENMARK]			= unitSystems.imperial,
	[nations.ISRAEL]			= unitSystems.imperial,
	[nations.GEORGIA]			= unitSystems.imperial,
	[nations.INSURGENTS]		= unitSystems.metric,
	[nations.ABKHAZIA]			= unitSystems.metric,
	[nations.SOUTH_OSETIA]		= unitSystems.metric,
	[nations.ITALY]				= unitSystems.imperial,
	[nations.AUSTRALIA]			= unitSystems.imperial,
    [nations.SWITZERLAND] 	    = unitSystems.imperial,
    [nations.AUSTRIA] 	        = unitSystems.imperial,
    [nations.BELARUS] 	        = unitSystems.metric,
    [nations.BULGARIA] 	        = unitSystems.imperial,
    [nations.CHEZH_REPUBLIC]    = unitSystems.imperial,
    [nations.CHINA]             = unitSystems.metric,
    [nations.CROATIA] 	        = unitSystems.imperial,
    [nations.EGYPT]	            = unitSystems.imperial,
    [nations.FINLAND] 	        = unitSystems.imperial,
    [nations.GREECE] 	        = unitSystems.imperial,
    [nations.HUNGARY] 	        = unitSystems.imperial,
    [nations.INDIA] 	        = unitSystems.imperial,
    [nations.IRAN] 	            = unitSystems.imperial,
    [nations.IRAQ] 	            = unitSystems.imperial,
    [nations.JAPAN]	            = unitSystems.imperial,
    [nations.KAZAKHSTAN] 	    = unitSystems.metric,
    [nations.NORTH_KOREA] 	    = unitSystems.imperial,
    [nations.PAKISTAN] 	        = unitSystems.imperial,
    [nations.POLAND] 	        = unitSystems.imperial,
    [nations.ROMANIA] 	        = unitSystems.imperial,
    [nations.SAUDI_ARABIA] 	    = unitSystems.imperial,
    [nations.SERBIA] 	        = unitSystems.imperial,
    [nations.SLOVAKIA] 	        = unitSystems.imperial,
    [nations.SOUTH_KOREA] 	    = unitSystems.imperial,
    [nations.SWEDEN] 	        = unitSystems.imperial,
    [nations.SYRIA]	            = unitSystems.imperial,
    [nations.AGGRESSORS]	    = unitSystems.imperial,

    [nations.YEMEN]	            = unitSystems.imperial,
    [nations.VIETNAM]	        = unitSystems.imperial,
    [nations.VENEZUELA]	        = unitSystems.imperial,
    [nations.TUNISIA]	        = unitSystems.imperial,
    [nations.THAILAND]	        = unitSystems.imperial,
    [nations.SUDAN]	            = unitSystems.imperial,
    [nations.PHILIPPINES]	    = unitSystems.imperial,
    [nations.MOROCCO]	        = unitSystems.imperial,
    [nations.MEXICO]	        = unitSystems.imperial,
    [nations.MALAYSIA]	        = unitSystems.imperial,
    [nations.LIBYA]			    = unitSystems.imperial,
    [nations.JORDAN]	        = unitSystems.imperial,
    [nations.INDONESIA]	        = unitSystems.imperial,
    [nations.HONDURAS]	        = unitSystems.imperial,
    [nations.ETHIOPIA]	        = unitSystems.imperial,
    [nations.CHILE]			    = unitSystems.imperial,
    [nations.BRAZIL]	        = unitSystems.imperial,
    [nations.BAHRAIN]	        = unitSystems.imperial,

	[nations.THIRDREICH]				= unitSystems.metric,	
	[nations.YUGOSLAVIA]				= unitSystems.metric,
	[nations.USSR]						= unitSystems.metric,	
	[nations.ITALIAN_SOCIAL_REPUBLIC]	= unitSystems.metric,	
	[nations.ALGERIA]					= unitSystems.metric,	

	[nations.KUWAIT]					= unitSystems.imperial,	
	[nations.QATAR]						= unitSystems.imperial,	
	[nations.OMAN]						= unitSystems.imperial,	
	[nations.UNITED_ARAB_EMIRATES]		= unitSystems.imperial,	
	[nations.CUBA]						= unitSystems.imperial,	
	[nations.CJTF_RED]					= unitSystems.metric,	
	[nations.CJTF_BLUE]					= unitSystems.imperial,	
	[nations.UN_PEACEKEEPERS]			= unitSystems.imperial,
	[nations.LEBANON]					= unitSystems.metric,
	[nations.GDR]						= unitSystems.metric,
	[nations.ARGENTINA]					= unitSystems.metric,
	
	--Combined Joint Task Forces Red
}

space_ 			= p.separator(' ')
comma_space_  	= p._p({', ',	'delimeter',	','})
CR_  			= p._p({'\n',	'CR',			'\n'})

Phrase = {
	new = function(self, phraseIn, directoryIn)
		--base.print('\t Phrase : new, phraseIn,directoryIn',phraseIn,directoryIn)
		base.assert(phraseIn ~= nil)
		local newPhrase = { phrase = phraseIn, directory = directoryIn }
		base.setmetatable(newPhrase, self)
		return newPhrase
	end,
	make = function(self)
		return p._p(self.phrase, self.directory)
	end
}
Phrase.__index = Phrase

PhraseRandom = {
	new = function(self, phrasesIn, directoryIn)
		base.assert(phrasesIn ~= nil and #phrasesIn > 0)
		local newPhrases = { phrases = phrasesIn, directory = directoryIn }
		base.setmetatable(newPhrases, self)
		return newPhrases
	end,
	make = function(self)
		local number = base.math.random(1, base.table.getn(self.phrases))
		base.assert(self.phrases[number] ~= nil)
		return p._p(self.phrases[number], self.directory)
	end,
}
PhraseRandom.__index = PhraseRandom

Phrases = {
	new = function(self, phrasesIn, directoryIn, defaultIn)
		base.assert(phrasesIn ~= nil)
		local newPhrases = { phrases = phrasesIn, directory = directoryIn, default = defaultIn }
		base.setmetatable(newPhrases, self)
		return newPhrases
	end,
	exist = function(self, key)		
		if key ~= nil and self.phrases[key] ~= nil then
			return true
		else
			return false
		end
	end,
	make = function(self, key ,custom_data)
		base.assert(key ~= nil)	
		base.print('Phrases make : key = '..key)
		--base.print(base.debug.traceback())
		local   phrase = self.phrases[key] or custom_data or self.default
		if 	not phrase then
			for i,o in base.pairs(self.phrases) do
				local str = "Phrases:"..base.tostring(i)..":"
				for j,k in base.pairs(o) do
					str = str..","..base.tostring(k)
				end
				base.print(str)
			end
			base.print('Error: Phrases: no phrase with key = '..key)
		end
		return p._p( phrase, self.directory )
	end,
}
Phrases.__index = Phrases

Digits = {
	make = function(self, number, fmt)
		return p.digits(number, fmt)
	end
}

DigitGroups = {
	make = function(self, fmt, ...)
		return p.digit_groups(fmt, ...)
	end
}

Number = {
	make = function(self, number)
		return p.number(number)
	end
}

Index = {
	make = function(self, number)
		return p.index(number)
	end
}

do
	Digits.phrases = {}
	Digits.pos3 = true
	Digits.directory = 'Digits'
	for i = 0, 9 do
		base.table.insert(Digits.phrases, base.tostring(i))
	end
	base.table.insert(Digits.phrases, {'point', nil, 'decimal'})
	Number.phrases = {}
	Number.pos3 = true
	Number.directory = 'Numbers'
	for i = 10, 19, 1 do
		base.table.insert(Number.phrases, base.tostring(i))
	end
	for i = 20, 90, 10 do
		base.table.insert(Number.phrases, base.tostring(i))
	end
	base.table.insert(Number.phrases, {'100',	nil, 'hundred'})
	base.table.insert(Number.phrases, {'1000',	nil, 'thousand'})
	base.table.insert(Number.phrases, {'point', nil, 'decimal'})
end

do
	local function checkEvents(events)
		local missedEvents = 0
		for msgName, msgValue in base.pairs(base.Message) do
			if 	base.string.find(msgName,'wMsg') == 1 and
				base.string.find(msgName,'Maximum') ~= base.string.len(msgName) - 7 + 1 and
				base.string.find(msgName,'Null') ~= base.string.len(msgName) - 4 + 1 then
				if events[msgValue] == nil then
					missedEvents = missedEvents + 1
					base.error('Error: no phrase for '..msgName..' message!')			
				end
			end
		end
		return missedEvents == 0
	end

	local space = {
		Message = base.Message,
		_		= _
	}
	local eventsLoader, errorMsg = base.loadfile('Scripts/Speech/common_events.lua')
	if eventsLoader == nil then
		base.error(errorMsg)
	end
	base.setfenv(eventsLoader, space)
	eventsLoader()
	base.assert(space.events ~= nil)
	if base._DEBUG then
		base.assert(checkEvents(space.events))
	end
	Event = Phrases:new(space.events, 'Messages')
end

EventHandler = {
	make = function(self, message)
		return Event:make(message.event)
	end,
}

CompassDirection8 = {
	make = function(self, bearing_rad)
		local cardinal_direction_num = base.math.floor(base.math.mod(bearing_rad + 2 * u.PI / 16, 2 * u.PI) / (2 * u.PI / 8)) + 1	
		return self.sub.dir:make(cardinal_direction_num)
	end,
	sub = {
		dir = Phrases:new( { {_('north'),		'N',	'north'},
							{_('northeast'),	'NE',	'northeast'},
							{_('east'),			'E',	'east'},
							{_('southeast'),	'SE',	'southeast'},
							{_('south'),		'S',	'south'},
							{_('southwest'),	'SW',	'southwest'},
							{_('west'),			'W',	'west'},
							{_('northwest'),	'NW',	'northwest'} } )
	}
}

FromCompassDirection8 = {
	make = function(self, bearing_rad)
		local cardinal_direction_num = base.math.floor(base.math.mod(bearing_rad + 2 * u.PI / 16, 2 * u.PI) / (2 * u.PI / 8)) + 1	
		return self.sub.dir:make(cardinal_direction_num)
	end,
	sub = {
		dir = Phrases:new( { 	{_('from the north'),		'from the N',	'from the north'},
								{_('from the northeast'),	'from the NE',	'from the northeast'},
								{_('from the east'),		'from the E',	'from the east'},
								{_('from the southeast'),	'from the SE',	'from the southeast'},
								{_('from the south'),		'from the S',	'from the south'},
								{_('from the southwest'),	'from the SW',	'from the southwest'},
								{_('from the west'),		'from the W',	'from the west'},
								{_('from the northwest'),	'from the NW',	'from the northwest'} } )
	}
}

CompassDirection4 = {
	make = function(self, bearing_rad)
		local cardinal_direction_num = base.math.floor(base.math.mod(bearing_rad + u.PI / 4, 2 * u.PI) / (u.PI / 2)) + 1	
		return self.sub.dir:make(cardinal_direction_num)
	end,
	sub = {
		dir = Phrases:new( { {_('north'),		'N',	'north'},
							{_('east'),			'E',	'east'},
							{_('south'),		'S',	'south'},
							{_('west'),			'W',	'west'} } )
	}
}

UnitName = {
	new = function(self, unitName)
		local unitNames = {}
		for country, unitSystem in base.pairs(unitSystemByCountry) do
			unitNames[country] = unitSystem[unitName].name
		end
		return Phrases:new(unitNames)
	end
}

Wind = {
	make = function(self, dir)
		if dir then
			local windDir = u.round(u.get_azimuth(dir) * u.units.deg.coeff, 1)
			local windVel = u.round(u.get_lengthZX(dir) + 0.5, 1)
			if windVel > 1.5 then
				return p.start() + self.sub.wind:make() + space_ + self.sub.Digits:make(windDir, '%03d') + space_ + self.sub.at:make() + space_ + self.sub.Digits:make(windVel) + space_ + self.sub.mps:make()
			end
		end
		return nil
	end,
	sub = {
		wind	= Phrase:new({_('wind'),				'wind'}),
		at		= Phrase:new({_('at wind'),				'at'}),
		mps		= Phrase:new({_('meters per second'),	'meters per second'}),
		Digits	= Digits
	}
}

Runway = {
	make = function(self, runwayCode)
		local runwaySide = base.math.floor(runwayCode / 100)
		local runwayDir = runwayCode - runwaySide * 100
		return self.sub.runway:make() + space_ + self.sub.Digits:make(runwayDir, '%02d') + self.sub.side:make(runwaySide)
	end,
	sub = {
		runway = Phrase:new({_('runway'), 'runway'}),
		side = Phrases:new({'L', 'R'}, nil, ''),
		Digits = Digits
	}	
}

do

Pressure = {
	make = function(self, pressure, aircraftType, fmt)
		local country = aircraftNativeCountry[aircraftType] or base.country.USA
		local unit = unitSystemByCountry[country].pressure
		return self.sub.Digits:make(u.round(pressure * unit.coeff, 0.01), fmt)
	end,
	sub = {
		Digits	= Digits
	}
}

end

BearingAndRange = {
	make = function(self, dir, country)
	--base.print("~~~~~~~ BearingAndRange dir[x],dir[y],country"..country)
		local distanceUnit = unitSystemByCountry[country].distance
		return 	self.sub.Digits:make(u.round(u.get_azimuth(dir) * u.units.deg.coeff, 1),'%03d') +
				space_ + self.sub.pfor:make() + space_ + self.sub.Digits:make(u.adv_round(u.get_lengthZX(dir) * distanceUnit.coeff))
	end,
	sub = {
		pfor	= Phrase:new({_('for'), 'for'}),
		Digits	= Digits
	}
}

Bearing = {
	make = function(self, dir, country)
	--base.print("~~~~~~~ BearingAndRange dir[x],dir[y],country"..country)
		local distanceUnit = unitSystemByCountry[country].distance
		return 	self.sub.Digits:make(u.round(u.get_azimuth(dir) * u.units.deg.coeff, 1)) + space_ 
	end,
	sub = {
		Digits	= Digits
	}
}

local function from_db_callname(v)
	return { v.Name, v.soundFile or v.Name }
end

Callname = {
	new = function(self, country, category, directory, default)
		--base.print('\tCommon: Callname : new')
		local callnames = base.db.getCallnames(country, category)
		local callnameList = {}
		for callnameNum, callnameValue in base.pairs(callnames) do
			base.table.insert(callnameList,from_db_callname(callnameValue))
		end
		return Phrases:new(callnameList, directory, callnameList[default])
	end
}

do

	local function findInTable(tbl, element)
		for i, v in base.pairs(tbl) do
			if v == element then
				return true
			end
		end
		return false
	end

	local function _getUnitCallnames(self,pUnit,countryCallnames, callname)
		--base.print('\t\tCommon: _getUnitCallnames , countryCallnames = ',countryCallnames)
		if countryCallnames then
			--base.print('\t\t\t pUnit:getTypeName() == '..pUnit:getTypeName())
			local typeCallname = countryCallnames.sub.callnameByTypeName.sub[pUnit:getTypeName()]
			if typeCallname ~= nil then 
				return typeCallname:make(callname)
			end	
			local cat = pUnit:getDesc().category
			--base.print('\t\t\t ~~~~_getUnitCallnames :pUnit:getDesc().category = '.. cat)
			for category, sub in base.pairs(countryCallnames.sub.callnameByAttributes.sub) do
				--base.print('\t\t\t\t ~~~~_getUnitCallnames :category '..category)
				--base.print('\t\t\t\t ~~~~_getUnitCallnames :cat      '..cat)

				-- DEBUG
				--local attr = pUnit:getAttributes()
				--for aId, anames in base.pairs(attr) do
				--	base.print('\t\t attr: ',aId)
			    --end
				-- DEBUG
				--base.print('\t\t ~~~~_getUnitCallnames : callname = '..callname)
				if pUnit:hasAttribute(category) or (pUnit:hasAttribute('EWR') and category == 'AWACS' and cat == 2) then
					return sub:make(callname)
				end
			end
			--base.print('\t\t\t countryCallnames.sub.callnameByAttributes.sub == nil')
		else
			--base.print('\t\t countryCallnames == nil')
		end
		return nil	
	end
	
	local function getUnitCallnames(self,pUnit,country_id, callname)
		--base.print('\t\ getUnitCallnames')
		--base.print('\t\t\ callname '..callname)
		--base.print('\t\t\ country_id '..country_id)
        local default          = base.db.DefaultCountry[country_id] or 2		

		--------------------------------------------------------------------------------
		-- Путаница с позывными китайскими
		-- прописаны советские а используются имперские (так озвучили китайцы)
		-- country_id == 27 == CHINA
		-- Заплатка для танкера. чтобы не было крэша. 
		-- ..have RUSSIAN style callsign for China (numerical), and NATO protocol..
		--if country_id == 27 and default == 0 and pUnit:getTypeName() == 'IL-78M' then	
		if country_id == 27 and default == 0 then	
			default = 2
		end
		--------------------------------------------------------------------------------

        local countryCallnames = self.sub[country_id]

		local res = _getUnitCallnames(self, pUnit, countryCallnames, callname)
		if res ~= nil then
			return res
		end
		--base.print('\t default '..default)
		countryCallnames = self.sub[default]

		local cat = pUnit:getDesc().category
		if hasNumericCallsign(pUnit) then
			return _getUnitCallnames(self, pUnit, countryCallnames, callname)
		elseif cat == 2 and callname == 0 then --cat == 2 - GROUNG
			return _getUnitCallnames(self, pUnit, countryCallnames, 1)
		else
			return  _getUnitCallnames(self, pUnit, countryCallnames, callname)
		end
	end	

	UnitCallname = {
		new = function(self, attribute, deep, typeName, directory)
			--base.print('\r\n\t\t\t\t\t\t UnitCallname new: ', 'attribute', attribute)
			--base.print(base.debug.traceback())
			--base.print('\t\t attribute: ',attribute)
			--base.print('\t\t	deep: '..base.tostring(deep))
			base.assert(attribute ~= nil or typeName ~= nil)
			local unitCallname = { sub = {} }		
			for countryId, countryCallnames in base.pairs(base.db.Callnames) do
				--base.print('\t\t ~countryId = '..countryId)
				unitCallname.sub[countryId] = { sub = { callnameByAttributes = { sub = {} } , callnameByTypeName  =  { sub = {} } } }
				for category, callsignList in base.pairs(countryCallnames) do
					--base.print('\t\t ~~~~~!!! UnitCallname new :  category', category,'\r\n')
					if attribute ~= nil then
						if	(deep and (	base.type(attribute) == 'table' and base.hasAttributes(category, attribute) or
										base.type(attribute) == 'string' and base.hasAttribute(category, attribute)))
								or
							(not deep and (	base.type(attribute) == 'table' and findInTable(attribute, category) or
											base.type(attribute) == 'string' and attribute == category)) then									
							local callnameTbl = {}
							for i, v in base.pairs(callsignList) do
								--base.print('\t\t ~~attribute ~= nil : callsignList = '..base.tostring(v.WorldID)..' '..v.Name )
								--base.print('\t\t ~~attribute ~= nil : soundfile = '..base.tostring(v.soundFile) )
								callnameTbl[v.WorldID] = from_db_callname(v)
							end		
							--base.print('\t\t ~~attribute countryId '..countryId..' category '..category..' directory '..directory..'\r\n')
							unitCallname.sub[countryId].sub.callnameByAttributes.sub[category] = Phrases:new(callnameTbl, directory)
						end
					else
						--base.print('\t\t ~~attribute == nil ' )
					end
					local callnameTbl = {}
					for i, v in base.pairs(callsignList) do
						--base.print('\t\t ~~typeName ~= nil : callsignList = ',base.tostring(v.WorldID),' ',v.Name )
						--base.print('\t\t ~~typeName ~= nil : soundfile = ',base.tostring(v.soundFile) )
						callnameTbl[v.WorldID] = from_db_callname(v)
					end					
					--base.print('\t\t ~~typename countryId ',countryId,' category ',category,' directory ',directory,'\r\n')
					unitCallname.sub[countryId].sub.callnameByTypeName.sub[category] = Phrases:new(callnameTbl, directory)
					--[[if typeName ~= nil then						
						if	base.type(typeName) == 'table' and findInTable(typeName, category) or
							base.type(typeName) == 'string' and typeName == category then
							local callnameTbl = {}
							for i, v in base.pairs(callsignList) do
								--base.print('\t\t ~~typeName ~= nil : callsignList = ',base.tostring(v.WorldID),' ',v.Name )
								--base.print('\t\t ~~typeName ~= nil : soundfile = ',base.tostring(v.soundFile) )
								callnameTbl[v.WorldID] = from_db_callname(v)
							end					
							--base.print('\t\t ~~typename countryId ',countryId,' category ',category,' directory ',directory,'\r\n')
							unitCallname.sub[countryId].sub.callnameByTypeName.sub[category] = Phrases:new(callnameTbl, directory)
						end
					else
						--base.print('\t\t ~****~typeName == nil ' )
					end]]--
				end
			end
			base.setmetatable(unitCallname, self)
			return unitCallname
		end,
		make = function(self, pUnit, callname)
			--base.print('\t\t ~~UnitCallname make(self, pUnit, callname)', self, pUnit, callname )
			local country = pUnit:getCountry()
			--country = 68 --USSR
			local result =	getUnitCallnames(self,pUnit,country,callname)
			--base.print('\t\t ~~UnitCallname make country', country )
			if result ~= nil then				
				return result
			end
			base.error('Callname '..base.tostring(callname)..' not found for '..pUnit:getName()..' !')
		end
	}
	UnitCallname.__index = UnitCallname
end

local airdromeNameVariants = {
	['Common'] = 'Common',
	['USSR'] = 'USSR',
	['NATO'] = 'NATO'
}

local defaultAirdromeNameVariant = airdromeNameVariants['Common']

function getAirdromeNameVariant(language)	
	if language == 'RUS' then
		return airdromeNameVariants['USSR']
	else		
		return airdromeNameVariants['NATO']
	end
end

function getBoardNumber(number)
	local a1 = math.floor(number / 100)
	local a2 = math.floor((number % 100)/10)
	local a3 = math.floor(number % 10)
	return a1,a2,a3
end


local makeNAVY_XXX = function(pUnit)
	base.assert(pUnit:hasAttribute('Air'))

	local a1 = u.round(10*pUnit:getDrawArgumentValue(442),1)
	local a2 = u.round(10*pUnit:getDrawArgumentValue(31),1)
	local a3 = u.round(10*pUnit:getDrawArgumentValue(32),1)

	return DigitGroups:make('%d%d%d', a1, a2, a3)
end

USNAVYPlayerAircraftCallsign = {
	
	make = function(self, pComm)
		if pComm == nil then
			return p.start()
		end	
		local	pUnit = pComm
		return	makeNAVY_XXX(pUnit)
	end,
	sub = {	Digits					= Digits,
			DigitGroups				= DigitGroups,
	},
}

AirGroupCallsign = {
	make_callname_base = function(self,pUnit,callsignIdx)
		if pUnit:hasAttribute('AWACS') then
			return self.sub.AWACSCallname:make(pUnit, callsignIdx)
		elseif pUnit:hasAttribute('Tankers') then
			return self.sub.TankerCallname:make(pUnit, callsignIdx)
		elseif pUnit:hasAttribute('Transports') then							
			return self.sub.TransportsCallname:make(pUnit, callsignIdx)
		else
			return self.sub.AirGroupCallname:make(pUnit, callsignIdx)
		end
	end,

	make = function(self, pComm, useIndex)
		if pComm == nil then
			return p.start()
		end
		local pUnit 	= pComm:getUnit()
		local callsign 	= pComm:getCallsign()
		local callsign2 = pUnit:getCallsign()
		if callsign2 == "" then
			return p.start()
		end
		if hasNumericCallsign(pUnit) then
			local c = callsignMinimize(callsign)
			if useIndex then
				return self.sub.Index:make(c)
			else
				return self.sub.Digits:make(c)
			end
		end
		
		local groupName, flightNum, aircraftNum = encodeCallsign(callsign)
		local base_callname  					= self:make_callname_base(pUnit,groupName)
		
		if	pUnit:hasAttribute('Strategic bombers') or 
			pUnit:hasAttribute('Aux') then
			return base_callname
		end
		
		if	pUnit:hasAttribute('AWACS') or
			pUnit:hasAttribute('Tankers') or
			pUnit:hasAttribute('Transports') then							
			return base_callname + self.sub.DigitGroups:make('%d-%d', flightNum, aircraftNum)
		else
			return base_callname + ' ' + self.sub.Digits:make(flightNum)
		end
	end,
	sub = { AirGroupCallname	= UnitCallname:new('Air', true, nil, 'Callsign'),
			AWACSCallname		= UnitCallname:new('AWACS', true, nil, 'Callsign'),
			TankerCallname		= UnitCallname:new('Tankers', true, nil, 'Callsign'),
			TransportsCallname	= UnitCallname:new('Transports', true, nil, 'Callsign'),
			Index				= Index,
			Digits 				= Digits,
			DigitGroups			= DigitGroups }
}

PlayerAircraftCallsign = {
	make = function(self, pComm, pComm2, useIndex)
		--base.print('\t PlayerAircraftCallsign:make')
		if pComm == nil then
			return p.start()
		end	

		-- Navy
		-- If negotiations witch AirbaseSHIP and unit is NAVY then use boardnumber for callsign !!!
		if pComm2 then
			local pUnit  = pComm:getUnit()
			local pUnit2 = pComm2:getUnit()
			if	pUnit and pUnit2 and 
				pUnit2:getForcesName() 		== "AirBase" and 
				pUnit2:getDesc().category 	== base.Airbase.Category.SHIP and 
				pUnit2:getCoalition() == 2 then --'NAVY'  
				return makeNAVY_XXX(pUnit)
			end
		end
		return AirGroupCallsign:make(pComm,useIndex)
	end,
	-- TODO: to remove the 'hack' with A-10 specific callnames. This features should be available for all units - removed
	sub = {	Index					= Index,
			Digits					= Digits,
			DigitGroups				= DigitGroups },
}


local function getCallname(pUnit, callnameId)
	local callnames = base.db.getCallnames(pUnit:getCountry(), pUnit:getTypeName())
	if callnames and callnames[callnameId] then
		return callnames[callnameId].Name
	end

	local checkUnitAttribute = function(pUnit, attribute)
		return pUnit:hasAttribute(attribute)
	end
	callnames = base.db.getUnitCallnames2(pUnit:getCountry(), pUnit, checkUnitAttribute)
	if callnames and callnames[callnameId] then
		return callnames[callnameId].Name
	end
end

function getCallname_(self,pComm, callnameId)
	local pUnit = pComm:getUnit()
	return getCallname(pUnit, callnameId)
end

local function makeCallsignString_(pComm, airdromeNameVariant)
	if pComm == nil then
		return nil
	end	

	local pUnit = pComm:getUnit()
	base.assert(pUnit ~= nil)
	local country = pUnit:getCountry()
	local callsign = pComm:getCallsign()

	if 	pUnit:getCategory() == base.Object.Category.BASE then
		if pUnit:getDesc().category == base.Airbase.Category.HELIPAD then
			return getCallname(pUnit, callsign)
		else
			local name = (airdromeNames[airdromeNameVariant] ~= nil and airdromeNames[airdromeNameVariant][callsign] ~= nil) and
							airdromeNames[airdromeNameVariant][callsign]
						or
							airdromeNames[defaultAirdromeNameVariant][callsign]		
			if name==nil then
				return nil
			end	
			return name[1]
		end		
	else
		if hasNumericCallsign(pUnit) then
			return callsignMinimize(callsign)
		end
		local callnameIdx, flightNum, aircraftNum = encodeCallsign(callsign)
		local callname = getCallname(pUnit, callnameIdx)
		
		if not(callname) then 
			return nil
		end
		if	pUnit:hasAttribute('AWACS') or
			pUnit:hasAttribute('Tankers') or 
			pUnit:hasAttribute('Transports') then 
			return callname..flightNum..'-'..aircraftNum
		elseif	pUnit:hasAttribute('Strategic bombers') or 
				pUnit:hasAttribute('Aux') then
			return callname
		else
			return callname..flightNum..aircraftNum
		end
	end
	return nil
end

function makeCallsignString(self, pComm)
	return makeCallsignString_(pComm, 'NATO')
end

do

local temper = {
	CALM = 1,
	BRIGHT = 2,
	LOUD = 3
}

local temperByMessage = {
	[base.Message.wMsgWingmenCopy] 						= temper.BRIGHT,
	[base.Message.wMsgWingmenFlightCheckInPositive] 	= temper.BRIGHT,
	[base.Message.wMsgWingmenTargetDestroyed]			= temper.LOUD,
	[base.Message.wMsgWingmenEjecting]					= temper.LOUD,
	[base.Message.wMsgWingmenIamHit]					= temper.LOUD,
	[base.Message.wMsgWingmenMissileAway]				= temper.BRIGHT,
	[base.Message.wMsgWingmenGuns]                 		= temper.BRIGHT,
	[base.Message.wMsgWingmenEngagedDefensive]     		= temper.BRIGHT,	
	[base.Message.wMsgWingmenEngagingSAM]				= temper.BRIGHT,
	[base.Message.wMsgWingmenEngagingAAA]				= temper.BRIGHT,
	[base.Message.wMsgWingmenEngagingArmor]				= temper.BRIGHT,
	[base.Message.wMsgWingmenEngagingArtillery]			= temper.BRIGHT,
	[base.Message.wMsgWingmenEngagingVehicle]			= temper.BRIGHT,
	[base.Message.wMsgWingmenEngagingShip]				= temper.BRIGHT,
	[base.Message.wMsgWingmenEngagingBunker]			= temper.BRIGHT,
	[base.Message.wMsgWingmenEngagingStructure]			= temper.BRIGHT,
	[base.Message.wMsgWingmenRequestPermissionToAttack] = temper.BRIGHT,
	[base.Message.wMsgWingmenBombsAway]					= temper.BRIGHT,
	[base.Message.wMsgWingmenRockets]					= temper.BRIGHT,
	[base.Message.wMsgWingmenRunningIn]					= temper.BRIGHT,
	[base.Message.wMsgWingmenWinchester]				= temper.BRIGHT,
	[base.Message.wMsgWingmenTallyBandit]				= temper.BRIGHT,
	[base.Message.wMsgWingmenSAMLaunch]					= temper.LOUD,
	[base.Message.wMsgWingmenMudSpike]					= temper.BRIGHT,
	[base.Message.wMsgWingmenSpike]						= temper.BRIGHT, 
	[base.Message.wMsgWingmenCheckYourSix]				= temper.LOUD,
	[base.Message.wMsgWingmenHelReconBearing]			= temper.BRIGHT,
	[base.Message.wMsgWingmenFollowScanMode]			= temper.BRIGHT,
	[base.Message.wMsgWingmenHelLaunchAbortTask]		= temper.BRIGHT,
	[base.Message.wMsgWingmenBugout]					= temper.LOUD,
}

WingmanMessageHandler = {
	make = function(self, message)
		local messageTemper = temperByMessage[message.event] or temper.CALM
		local number = message.sender:getUnit():getNumber()
		base.assert(number >= 2 and number <= 4)
		return self.sub[messageTemper]:make(number - 1)  + comma_space_ + Event:make(message.event)
	end,
	sub = {
		[temper.CALM] = Phrases:new({	{_('2nd'), '2-calm'},
										{_('3rd'), '3-calm'},
										{_('4th'), '4-calm'} }),
		[temper.BRIGHT] = Phrases:new({	{_('2nd'), '2-bright'},
										{_('3rd'), '3-bright'},
										{_('4th'), '4-bright'} }),
		[temper.LOUD] = Phrases:new({	{_('2nd'), '2-loud'},
										{_('3rd'), '3-loud'},
										{_('4th'), '4-loud'} })
	}
}

end

BearingOClock = {
	make = function(self, bearing_rad)
		local hour = base.math.floor((bearing_rad * u.units.deg.coeff + 15) / 30)
		if hour == 0 then
			hour = 12
		end
		return self.sub.clocks:make(hour)
	end,
	sub = {
		clocks = Phrases:new( { 	{_('1 o\'clock'), '1oclock'},
									{_('2 o\'clock'), '2oclock'},
									{_('3 o\'clock'), '3oclock'},
									{_('4 o\'clock'), '4oclock'},
									{_('5 o\'clock'), '5oclock'},
									{_('6 o\'clock'), '6oclock'},
									{_('7 o\'clock'), '7oclock'},
									{_('8 o\'clock'), '8oclock'},
									{_('9 o\'clock'), '9oclock'},
									{_('10 o\'clock'), '10oclock'},
									{_('11 o\'clock'), '11oclock'},
									{_('12 o\'clock'), '12oclock'} } )
	}
}

WingmanBearingHandler = {
	make = function(self, message)
		return self.sub.WingmanMessageHandler:make(message) + ' ' + self.sub.BearingOClock:make(u.get_azimuth(message.parameters.dir))
	end,
	sub = { WingmanMessageHandler	= WingmanMessageHandler,
			BearingOClock			= BearingOClock}
}

WingmanContactHandler = {
	make = function(self, message)
		local country = message.sender:getUnit():getCountry()
		local distanceUnit = unitSystemByCountry[country].distance
		return 	self.sub.WingmanMessageHandler:make(message) + ' ' +
				self.sub.targetType:make(message.parameters.type) + ' ' +
				self.sub.BearingOClock:make(u.get_azimuth(message.parameters.dir)) + comma_space_ +
				self.sub.pfor:make() + ' ' +
				self.sub.Digits:make(u.round(u.get_lengthZX(message.parameters.dir) * distanceUnit.coeff + 0.5, 1))
	end,
	sub = {	WingmanMessageHandler	= WingmanMessageHandler,
			targetType				= Phrases:new({	{_('armor target'), 		'armor target'},
													{_('air defence target'), 	'air defence target'},
													{_('target'), 				'target'}}),
			BearingOClock			= BearingOClock,
			pfor					= Phrase:new({_('for'), 'for'}),
			Digits					= Digits}
}

function clearAirdromeSubs()
	for moduleIndex, airdromeNameVariant in base.pairs(airdromeNameVariants) do
		airdromeNames[airdromeNameVariant] = {}
	end
end

local function initAirdromeSubs()
	for moduleIndex, airdromeNameVariant in base.pairs(airdromeNameVariants) do
		AirbaseName.sub[base.Airbase.Category.AIRDROME].sub[airdromeNameVariant] = Phrases:new(airdromeNames[airdromeNameVariant], 'Callsign')
	end
end

airdromeNames = {}
do 
	clearAirdromeSubs()
end

AirbaseName = {
	make = function(self, comm, airdromeNameVariant)
		
		local airbaseCategory = comm:getUnit():getDesc().category
		if airbaseCategory == base.Airbase.Category.AIRDROME then
			return self.sub[airbaseCategory]:make(comm:getUnit(), comm:getCallsign(), airdromeNameVariant)
		elseif airbaseCategory == base.Airbase.Category.HELIPAD then
			return self.sub[airbaseCategory]:make(comm:getUnit(), comm:getCallsign())
		else		
			return _('Ship') --return self.sub[airbaseCategory]:make(comm:getUnit(), --[[comm:getCallsign() or --]] 1) or _('Ship')
		end		
	end,
	sub = {
		[base.Airbase.Category.AIRDROME]	= {
			make = function(self, pUnit, callsign, airdromeNameVariant)
				local names = 	(airdromeNames[airdromeNameVariant] ~= nil and airdromeNames[airdromeNameVariant][callsign] ~= nil) and
									self.sub[airdromeNameVariant]
								or
									self.sub[defaultModuleName]
				return names:make(callsign)
			end,
			sub = {}
		},		
		[base.Airbase.Category.HELIPAD]		= UnitCallname:new('Helipad', false, nil, 'Callsign'),
		[base.Airbase.Category.SHIP]		= UnitCallname:new('Aircraft Carriers', false, nil,'Aircraft Carriers'),
	}
}
do 
	initAirdromeSubs()
end

ATCToLeaderHandler = {
	make = function(self, message, language)
	base.print( '\t ATCToLeaderHandler : make, coalition = '.. message.sender:getUnit():getCoalition() )
		if message.sender:getUnit():getDesc().category == base.Airbase.Category.SHIP and message.sender:getUnit():getCoalition() == 2  then
			return 	self.sub.PlayerAircraftCallsign:make(message.receiver,message.sender) +
					comma_space_ + Event:make(message.event)
		else
			--base.print( '\t ATCToLeaderHandler : make' )
			return 	self.sub.PlayerAircraftCallsign:make(message.receiver,message.sender) + comma_space_ +
					self.sub.AirbaseName:make(message.sender, getAirdromeNameVariant(language)) +
					comma_space_ + Event:make(message.event)
			end
	end,
	sub = { AirbaseName				= AirbaseName,
			PlayerAircraftCallsign	= PlayerAircraftCallsign }
}

ToWingmen = {
	make = function(self, message)
		--base.print( '\t ToWingmen : make , whom = ',message.parameters.whom )
		base.assert(message.parameters and message.parameters.whom and message.parameters.whom > 0 and message.parameters.whom <= 5)
		local whom = message.parameters.whom
		local result = p.start()
		if whom == 5 then
			return self.sub.flight:make()
		elseif whom == 4 then
			return self.sub.toWingmen:make(2)
		else
			return self.sub.toWingmen:make(whom)
		end
	end,
	sub = {
		toWingmen	= Phrases:new({	{_('to 2'), 	'to 2'},
									{_('to 3'), 	'to 3'},
									{_('to 4'), 	'to 4'}}),
		flight = Phrase:new({_('to flight'), 'to flight'})
	}
}

Direction = {
	make = function(self, dir, country)
		local distanceUnit = unitSystemByCountry[country].distance
		return 	self.sub.Digits:make(u.round(u.get_azimuth(dir) * u.units.deg.coeff, 1), '%03d') + ' ' +
				self.sub.pfor:make() + ' ' + self.sub.Number:make(u.adv_round(u.get_lengthZX(dir) * distanceUnit.coeff, 1))
	end,
	sub = {
		Digits	= Digits,
		Number	= Number,
		pfor	= Phrase:new({_('for'), 'for'})
	}
}

BullseyeCoords = {
	make = function(self, point, coalition, country)
		--base.print('\t BullseyeCoords:make')
		--base.print('\t\t BullseyeCoords:make coalition=%d, country=%d: ', coalition, country)
		local bullsEye = base.coalition.getMainRefPoint(coalition)
		local bullsEyeDir = { x = point.x - bullsEye.x, y = point.y - bullsEye.y, z = point.z - bullsEye.z }
		return self.sub.atBulls:make() + ' ' + self.sub.Direction:make(bullsEyeDir, country)
	end,
	sub = { atBulls		= PhraseRandom:new({{_('at bulls'),		'at bulls'},
											{_('at bullseye'),	'at bullseye'}}),
			Direction	= Direction	}
}

Altitude = {
	make = function(self, alt, country, accuracy)
		if alt <= 0.0 then
			base.print('\t Altitude:make')
		
			base.print('\t\t alt : '..alt)
			base.print('\t\t country : '..country)
			if accuracy ~= nil then 
				base.print('\t\t accuracy : '..accuracy)
			end
		end		
		base.assert(alt > 0.0)
		local altitudeUnit = unitSystemByCountry[country].altitude
		return self.sub.at:make() + ' ' + self.sub.Number:make(u.adv_round(alt * altitudeUnit.coeff, accuracy or 1.0))
	end,
	sub = {	at		= Phrase:new({_('at altitude'), 'at'}),
			Number	= Number }
}


Velocity = {
	make = function(self, vel, country, accuracy)
		--base.assert(vel > 0.0)  --The aircraft can stand at the airfield. vel == 0
		local velocityUnit = unitSystemByCountry[country].velocity		
		return self.sub.at:make() + ' ' + self.sub.Digits:make(u.adv_round(vel * velocityUnit.coeff, accuracy or 1.0))
	end,
	sub = {	at		= Phrase:new({_('at velocity'), 'at'}),
			Digits	= Digits }
}

--AWACS

ClientAndAWACSHandler = {
	make = function(self, message, direction, useIndex)
		local   header = self:makeHeader(message, direction, useIndex)
		return 	header+ Event:make(message.event)
	end,
	
	makeHeader = function(self, message, direction, useIndex)
		if direction  then
			return self.sub.AWACSCallsign:make(message.receiver, useIndex) + comma_space_ + self.sub.PlayerAircraftCallsign:make(message.sender,message.receiver, useIndex) + comma_space_
		else
			return self.sub.PlayerAircraftCallsign:make(message.receiver,message.sender, useIndex) + comma_space_ + self.sub.AWACSCallsign:make(message.sender, useIndex) + comma_space_
		end
	end,
	sub = {	PlayerAircraftCallsign	= PlayerAircraftCallsign,
			AWACSCallsign 			= {
										make = function(self, pComm, useIndex)
											base.print('\t AWACSCallsign:make')
											local callsign = pComm:getCallsign()
											
											local unit = pComm:getUnit()
												
											if hasNumericCallsign(unit) then
												local c = callsignMinimize(callsign)
												if useIndex then
													return self.sub.Index:make(c)
												else
													return self.sub.Digits:make(c)
												end
											else
												local callnameIdx , flightNum, aircraftNum = encodeCallsign(callsign)
												if unit:getDesc().category == 2  then --GROUND UNIT
													return self.sub.GroundUnitCallname:make(unit,callnameIdx) + self.sub.DigitGroups:make('%d', flightNum)
												else
													return self.sub.AWACSCallname:make(unit, callnameIdx) 	  + self.sub.DigitGroups:make('%d-%d', flightNum, aircraftNum)
												end
											end
										end,
										sub = { AWACSCallname = UnitCallname:new('AWACS', false, nil, 'Callsign'),
												GroundUnitCallname = UnitCallname:new('Ground Units', false, nil, 'Callsign'),
												Digits = Digits,
												Index = Index,
												DigitGroups	= DigitGroups
												},
									}	
		}
}

ClientToAWACSHandler = {
	make = function(self, message, language)
		return self.sub.ClientAndAWACSHandler:make(message, true, language == 'RUS')
	end,
	makeHeader = function(self, message, language)
		return self.sub.ClientAndAWACSHandler:makeHeader(message, true, language == 'RUS')
	end,
	sub = { ClientAndAWACSHandler = ClientAndAWACSHandler }
}

AWACSToClientHandler = {
	make = function(self, message, language)
		return self.sub.ClientAndAWACSHandler:make(message, false, language == 'RUS')
	end,
	makeHeader = function(self, message, language)
		return self.sub.ClientAndAWACSHandler:makeHeader(message, false, language == 'RUS')
	end,
	sub = { ClientAndAWACSHandler = ClientAndAWACSHandler }
}

AWACSTargetBullseye = {
	make = function(self, target, bullsEyeForCoalition, country)
		return 	self.sub.BullseyeCoords:make(target.point, bullsEyeForCoalition, country) +
				comma_space_ + self.sub.Altitude:make(target.altitude, country, 500)
	end,
	sub = {	Number			= Number,
			BullseyeCoords	= BullseyeCoords,
			Altitude		= Altitude }
}

AWACSTargetDir = {
	make = function(self, target, receiver, country)
		local aspects = {
			self.sub.cold,
			self.sub.hot,
			self.sub.flanking	
		}
		local receiverPos = receiver:getUnit():getPosition().p
		local dir = {	x = target.point.x - receiverPos.x,
						y = target.point.y - receiverPos.y,
						z = target.point.z - receiverPos.z }
		return 	self.sub.Direction:make(dir, country) + comma_space_ + self.sub.Altitude:make(target.altitude, country, 500) +
				comma_space_ + aspects[target.aspect]:make()					
	end,
	sub = {	Number			= Number,
			Direction		= Direction,
			Altitude		= Altitude,
			cold			= Phrase:new({_('cold'),		'cold'}),
			hot				= PhraseRandom:new({{_('hot'),		'hot'},
												{_('hot'),		'hot 2'},
												{_('hot'),		'hot 3'},												
												{_('hot'),		'hot 4'}}),
			flanking		= Phrase:new({_('flanking'),	'flanking'}) }
}

AWACSbanditBearingHandler = {
	make = function(self, message, language)
		local pUnit = message.sender:getUnit()
		return 	self.sub.AWACSToClientHandler:make(message, language) + comma_space_ +
				self.sub.AWACSTargetDir:make(message.parameters, message.receiver, pUnit:getCountry())
	end,
	sub = {	AWACSToClientHandler	= AWACSToClientHandler,
			AWACSTargetDir			= AWACSTargetDir }
}

AWACSContactAffiliation = {

	make = function(self, message, language)
		local header  = AWACSToClientHandler:makeHeader(message, language)
		local answer 
		local  hostility = message.parameters.affiliation or 3
		if 	   hostility == 0 then -- friendly
			answer =  self.sub.ContactIsFriendly:make()
		elseif hostility == 1 then
			answer =  self.sub.ContactIsHostile:make()
		elseif hostility == 2 then
			answer =  self.sub.ContactIsNeutral:make()
		else
			answer =  self.sub.ContactIsUnknown:make()
		end
		return header + answer
	end,
	sub = {	ContactIsFriendly 		= Phrase:new({ _('contact is friendly'), 'contact is friendly'} ,'Messages'  ),
			ContactIsHostile		= Phrase:new({ _('contact is hostile'),  'contact is hostile'} 	,'Messages'  ),
			ContactIsNeutral		= Phrase:new({ _('contact is neutral'),  'contact is neutral'} 	,'Messages'  ),
			ContactIsUnknown		= Phrase:new({ _('contact is unknown'),  'contact is unknown'} 	,'Messages'  ),
	}
}
--Flight

do

local armor		= {_('_armor'), 	'armor'}
local utility	= {_('_utility'), 	'utility'}
local ship		= {_('_ship'), 		'ship'}
local bandit	= {_('_bandit'), 	'bandit'}

TargetShortDescription = {
	make = function(self, targetDesc, level, coalition, country)		
		base.print('\t\t TargetShortDescription : targetDesc.type = '..#targetDesc.type)
		local level = base.math.min(level, #targetDesc.type)
		base.print('\t\t TargetShortDescription : level = '..level)
		return self.sub.targetType:make(targetDesc.type[level] + 1) + ' ' + self.sub.BullseyeCoords:make(targetDesc.point, coalition, country)
	end,
	sub = {
		BullseyeCoords	= BullseyeCoords,
		targetType		= Phrases:new({	{_('_ground targets'),	'ground targets'},
										{_('_vehicles'),		'vehicles'},
										armor,
										{_('_infantry'),		'infantry'},
										armor,
										armor,
										armor,
										armor,
										{_('_artillery'),		'artillery'},
										utility,
										utility,
										utility,
										{_('_bunker'),			'bunker'},
										{_('_radar'),			'radar'},
										{_('_AAA'),				'AAA',		 'tripple A',		_('tripple A') },
										{_('_SAM'), 			'SAM'},
										{_('_ground targets'),	'ground targets'},
										ship,
										ship,
										ship,
										ship,
										ship,
										ship,
										ship,
										ship,
										ship,
										ship,
										bandit,
										bandit,
										bandit}, 'Target')
	}
}

end


FlightMessageHandler = {
	make = function(self, message, language)
		return self.sub.AirGroupCallsign:make(message.sender, language == 'RUS') + comma_space_ + Event:make(message.event)
	end,
	sub = {	AirGroupCallsign	= AirGroupCallsign }
}

PassingWaypointHandler = {
	make = function(self, message, language)
		return 	self.sub.FlightMessageHandler:make(message, language) + ' ' +
				self.sub.Number:make(message.parameters.waypoint) + ' ' +
				self.sub.Altitude:make(message.parameters.altitude, message.sender:getUnit():getCountry())
	end,
	sub = {	FlightMessageHandler	= FlightMessageHandler,
			Number					= Number,
			Altitude				= Altitude }
}

OnStationHandler = {
	make = function(self, message, language)
		return 	self.sub.FlightMessageHandler:make(message, language) + ' ' +
				self.sub.BullseyeCoords:make(message.parameters.point, message.sender:getUnit():getCoalition(), message.sender:getUnit():getCountry()) +
				' ' + self.sub.Altitude:make(message.parameters.altitude, message.sender:getUnit():getCountry(), message.sender:getUnit():getCountry())
	end,
	sub = { FlightMessageHandler	= FlightMessageHandler,
			BullseyeCoords			= BullseyeCoords,
			Altitude				= Altitude}
}

DepartingWaypointHandler = {
	make = function(self, message, language)
		return	self.sub.FlightMessageHandler:make(message, language) + ' ' + self.sub.Number:make(message.parameters.waypoint) + ' ' +
				self.sub.Altitude:make(message.parameters.altitude, message.sender:getUnit():getCountry())
	end,
	sub = {	FlightMessageHandler	= FlightMessageHandler,
			Number					= Number,
			Altitude				= Altitude}
}

TargetHandler = {
	make = function(self, message, language)
		return 	self.sub.FlightMessageHandler:make(message, language) + ' ' +
				self.sub.TargetShortDescription:make(message.parameters.targetDesc, 3, message.sender:getUnit():getCoalition(), message.sender:getUnit():getCountry())
	end,
	sub = { FlightMessageHandler	= FlightMessageHandler,
			TargetShortDescription	= TargetShortDescription }
}

EngagingHandler = {
	make = function(self, message, language)
		return 	self.sub.FlightMessageHandler:make(message, language) + ' ' +
				self.sub.FromCompassDirection8:make(u.get_azimuth(message.parameters.back_dir)) + comma_space_ +
				self.sub.engaging:make() + ' ' +
				self.sub.TargetShortDescription:make(message.parameters.targetDesc, 3, message.sender:getUnit():getCoalition(), message.sender:getUnit():getCountry())
	end,
	sub = { FlightMessageHandler	= FlightMessageHandler,
			FromCompassDirection8	= FromCompassDirection8,
			engaging				= Phrase:new({_('engaging'), 'engaging'}),
			TargetShortDescription	= TargetShortDescription }
}

BanditHandler = {
	make = function(self, message, language)
		return 	self.sub.FlightMessageHandler:make(message, language) + ' ' +
				self.sub.BullseyeCoords:make(message.parameters.point, message.sender:getUnit():getCoalition(), message.sender:getUnit():getCountry()) + comma_space_ +
				self.sub.Altitude:make(message.parameters.altitude, message.sender:getUnit():getCountry())
	end,
	sub = { FlightMessageHandler	= FlightMessageHandler,
			BullseyeCoords			= BullseyeCoords,
			Altitude				= Altitude}
}

RTBHandler = {
	make = function(self, message, language)
		return 	self.sub.FlightMessageHandler:make(message, language) + ' ' +
				self.sub.Altitude:make(message.parameters.altitude, message.sender:getUnit():getCountry())
	end,
	sub = { FlightMessageHandler	= FlightMessageHandler,
			Altitude				= Altitude}
}

MemberDownHandler = {
	make = function(self, message, language)
		return 	self.sub.FlightMessageHandler:make(message, language) + ' ' +
				self.sub.BullseyeCoords:make(message.parameters.point, message.sender:getUnit():getCoalition(), message.sender:getUnit():getCountry()) +
				'. ' + self.sub.requestCSAR:make() + '. '
	end,
	sub = { FlightMessageHandler	= FlightMessageHandler,
			BullseyeCoords			= BullseyeCoords,
			requestCSAR				= Phrase:new({_('Request CSAR'), 'Request CSAR'})}
}

TurnPreStartHandler = {
	make = function(self, message, language)
		--base.print("~~~ TurnPreStartHandler side, roll, self.directory", message.parameters.side,message.parameters.roll,self.directory)
		return 	self.sub.FlightMessageHandler:make(message, language) + ' ' +
				self.sub.Side:make(message.parameters.side) + self.sub.Roll:make() + 
				self.sub.Number:make(message.parameters.roll)
	end,
	sub = { FlightMessageHandler	= FlightMessageHandler,
			Side 					= Phrases:new(	{[1] = {_('left_turn', 'turn left'), 'turn left'},		--LEFT
													[2] = {_('right_turn', 'turn right'), 'turn right'},	--RIGHT
													},   'Messages'),
			Roll				= Phrase:new({_(', roll:'), 'roll'},   'Messages'),
			Number				= Number}
} 
TurnStartHandler = {
	make = function(self, message, language)
		return 	self.sub.FlightMessageHandler:make(message, language)
	end,
	sub = { FlightMessageHandler	= FlightMessageHandler,}
}
TurnStopHandler= {
	make = function(self, message, language)
		return 	self.sub.FlightMessageHandler:make(message, language)
	end,
	sub = { FlightMessageHandler	= FlightMessageHandler}
}

local empty_string = ''
StartEndHandler = {
	make = function(self, message, language)
		return  self.sub._start:make()	+  Event:make(message.event) + self.sub._end:make()
	end,
	sub = {			_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
					_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages') }
}

Start_Sender_Callsign_End_Handler = {
	make = function(self, message, language)
		return  self.sub._start:make()	+  self.sub.PlayerAircraftCallsign:make(message.sender:getUnit()) + comma_space_ +  Event:make(message.event) + self.sub._end:make()
	end,
	sub = {			
			PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
			_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
			_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages') }
}

Start_Receiver_Callsign_End_Handler = {
	make = function(self, message, language)
		return  self.sub._start:make()	+  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit()) + comma_space_ +  Event:make(message.event) + self.sub._end:make()
	end,
	sub = {			
			PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
			_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
			_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages') }
}


handlersTable = {
	--PLAYER -> WINGMAN
	[base.Message.wMsgLeaderMakeRecon] = {
		make = function(self, message)
			return 	self.sub.ToWingmen:make(message) + comma_space_ + Event:make(message.event) +
					space_ + self.sub.distance:make(u.round(message.parameters.range * u.units.km.coeff, 1)) +
					comma_space_ + self.sub.bearing:make() + ' ' + self.sub.Digits:make(message.parameters.bearing * u.units.deg.coeff, '%03d')
		end,
		sub = {	ToWingmen = ToWingmen,
				distance = Phrases:new({[1] = {_('1 km'), '1 km'},
										[2] = {_('2 km'), '2 km'},
										[3] = {_('3 km'), '3 km'},
										[5] = {_('5 km'), '5 km'},
										[8] = {_('8 km'), '8 km'},
										[10] = {_('10 km'), '10 km'}} ),
				bearing = Phrase:new({_('bearing'), 'bearing'}),
				Digits = Digits }
	},
	--WINGMAN -> PLAYER	
	[base.Message.wMsgWingmenCopy] = {
		make = function(self, message)
			return 	self.sub.WingmanMessageHandler:make(message) + ' ' + self.sub.report:make()
		end,
		sub = {	WingmanMessageHandler	= WingmanMessageHandler,				
				report					= PhraseRandom:new({{_('copy'),		'copy'},
															{_('roger'),	'roger'},
															{_('affirm'),	'affirm'}}) }
	},
	[base.Message.wMsgWingmenNegative] = {
		make = function(self, message)
			return 	self.sub.WingmanMessageHandler:make(message) + ' ' + self.sub.report:make()
		end,
		sub = {	WingmanMessageHandler	= WingmanMessageHandler,				
				report					= PhraseRandom:new({{_('unable'),			'unable'},
															{_('negative'),			'negative'}}) }
	},	
	[base.Message.wMsgWingmenRadarContact]			= {
		make = function(self, message)
			return 	self.sub.WingmanMessageHandler:make(message) + ' ' + 
					self.sub.BearingAndRange:make(message.parameters.dir, message.sender:getUnit():getCountry())
		end,
		sub = {	WingmanMessageHandler	= WingmanMessageHandler,
				BearingAndRange			= BearingAndRange}
	},
	[base.Message.wMsgWingmenContact]				= WingmanContactHandler,
	[base.Message.wMsgWingmenHelReconBearing]		= WingmanContactHandler,
	[base.Message.wMsgWingmenTallyBandit]			= WingmanBearingHandler,
	[base.Message.wMsgWingmenNails]					= WingmanBearingHandler,
	[base.Message.wMsgWingmenSpike]					= WingmanBearingHandler,
	[base.Message.wMsgWingmenMudSpike]				= WingmanBearingHandler,
	[base.Message.wMsgWingmenSAMLaunch]				= WingmanBearingHandler,
	[base.Message.wMsgWingmenMissileLaunch]			= WingmanBearingHandler,
	[base.Message.wMsgWingmenTargetDestroyed] = {
		make = function(self, message)
			local result = self.sub.WingmanMessageHandler:make(message) + ' '
			if message.sender:getUnit():hasAttribute("Helicopters") then
				result = result + self.sub.reportHelicopter:make()
			else
				result = result + self.sub.reportAirplane:make()
			end
			return result
		end,
		sub = {	WingmanMessageHandler	= WingmanMessageHandler,				
				reportAirplane			= PhraseRandom:new({{_('target destroyed'),			'target destroyed'},
															{_('rounds on target'),			'rounds on target'}}),
				reportHelicopter		= PhraseRandom:new({{_('shack'),					'shack'},
															{_('rounds on target'),			'rounds on target'},
															{_('good hits on target'),		'good hits on target'},
															{_('target hit'),				'target hit'} }) }
	},
	-- ATC2
	[base.Message.wMsgWingmenFlightCheckInPositive] = {
		make = function(self, message)
			return 	self.sub.WingmanMessageHandler:make(message) + ' ' + self.sub.report:make()
		end,
		sub = {	WingmanMessageHandler	= WingmanMessageHandler,				
				report					= PhraseRandom:new({{_('roger'),	'roger'},
															{_('roger'),	'roger'} }) }
	},
	--ATC -> PLAYER
	[base.Message.wMsgATCClearedForEngineStartUp]	= {
		make = function(self, message, language)
			local wind = self.sub.Wind:make(message.parameters.wind)
			return self.sub.ATCToLeaderHandler:make(message, language) + (wind ~= nil and (comma_space_ + wind) or '')
		end,
		sub = { ATCToLeaderHandler = ATCToLeaderHandler, Wind = Wind }
	},
	[base.Message.wMsgATCClearedToTaxiRunWay]		= {
		make = function(self, message, language)
			--return self.sub.ATCToLeaderHandler:make(message, language) + space_ + self.sub.Digits:make(message.parameters.runway) + 
			return self.sub.ATCToLeaderHandler:make(message, language) + space_ + (message.parameters.runway ~= nil and (self.sub.Digits:make(message.parameters.runway)) or '') + 
			(message.parameters.runway_side ~= nil and (self.sub.side:make(message.parameters.runway_side)) or '')
		end,
		sub = { ATCToLeaderHandler = ATCToLeaderHandler, 
				Digits = Digits,
				--0044066: Trunk: An assertion in radio when trying to request taxi to runway
				--to match  enum class RWSide{NOT_DEFINED, LEFT, RIGHT, CENTER};
				side = Phrases:new({	[0] = {"",""}, 				--NOT_DEFINED
										[1] = {_('L'), 'left'},		--LEFT
										[2] = {_('R'), 'right'},	--RIGHT
										[3] = {"",""},				--CENTER
									})
			}
	},
	[base.Message.wMsgATCYouAreClearedForTO]		= {
		make = function(self, message, language)
			local aircraftType = message.receiver:getUnit():getTypeName()
			return 	self.sub.ATCToLeaderHandler:make(message, language) + comma_space_ +
					self.sub.climb300AtQFE:make() + space_ + self.sub.Pressure:make(message.parameters.pressure, aircraftType, '%.2f')
		end,
		sub = {	ATCToLeaderHandler	= ATCToLeaderHandler,
				Runway			= Runway,
				climb300AtQFE	= Phrase:new({_('climb 300 at QFE'), 'climb 300 at QFE', 'climb three hundred at Qu eF E'}),
				Pressure		= Pressure }
	},	
	[base.Message.wMsgATCTrafficBearing]			= nil,
	[base.Message.wMsgATCYouAreClearedForLanding]	= {
		make = function(self, message, language)
			--base.print( '\t wMsgATCYouAreClearedForLanding : make' )
			local wind = self.sub.Wind:make(message.parameters.wind)
			return 	self.sub.ATCToLeaderHandler:make(message, language) +
					(message.parameters.runway ~= nil and (comma_space_ + self.sub.Runway:make(message.parameters.runway)) or '') +
					(wind ~= nil and (comma_space_ + wind) or '')
		end,	
		sub = {	ATCToLeaderHandler	= ATCToLeaderHandler,
				Runway				= Runway,
				Wind				= Wind}
	},
	[base.Message.wMsgATCAzimuth]					= {
		make = function(self, message, language)
				--base.print( '\t wMsgATCAzimuth: make' )
				return 	self.sub.PlayerAircraftCallsign:make(message.receiver,message.sender) + comma_space_ +
						self.sub.AirbaseName:make(message.sender, getAirdromeNameVariant(language)) +
						Event:make(message.event) + ' ' +
						self.sub.Digits:make(u.round(u.get_azimuth(message.parameters.direction) * u.units.deg.coeff, 1))
		end,
		sub = {	AirbaseName				= AirbaseName,
				PlayerAircraftCallsign	= PlayerAircraftCallsign,
				Digits					= Digits}
	},
	[base.Message.wMsgATCFlyHeading]				= {
		make = function(self, message, language)
			local country = message.receiver:getUnit():getCountry()
			local aircraftType = message.receiver:getUnit():getTypeName()
			return 	self.sub.ATCToLeaderHandler:make(message, language) + space_ + self.sub.BearingAndRange:make(message.parameters.direction, country) +
					comma_space_ + self.sub.QFE:make() + space_ + self.sub.Pressure:make(message.parameters.pressure, aircraftType, '%.2f') + comma_space_ +
					(message.parameters.runway ~= nil and (self.sub.Runway:make(message.parameters.runway) + comma_space_) or '') +
					self.sub.toPatternAlt:make()
		end,
		sub = {	ATCToLeaderHandler	= ATCToLeaderHandler,
				BearingAndRange		= BearingAndRange,
				Runway				= Runway,
				QFE					= Phrase:new({_('QFE'), 'QFE', 'Qu eF E'}),
				Pressure			= Pressure,
				toPatternAlt		= Phrase:new({_('to pattern altitude'), 'to pattern altitude', 'to pattern altitude two thousand'}) }
	},
	--[SIDE NUMBER], [VISIBILITY], [CLOUDS], [CLOUD ALTITUDE], [PRESSURE]. [MARSHAL REPORT], [HEADING OF CARRIER]. [MARSHAL-SEE-ME]. 
	[base.Message.wMsgATCMarshallCopyInbound]				= {
		make = function(self, message, language)		
			local country = message.receiver:getUnit():getCountry()
			local aircraftType = message.receiver:getUnit():getTypeName()
						
			local distanceUnit = unitSystemByCountry[country].distance
			local visibility_distD = base.math.max(1, base.math.floor(message.parameters.visibility * distanceUnit.coeff))			
			if message.parameters.visibility > 19000 then
				visibility_distD = 11
			end
			
			local clouds_density = self.sub.sky_clear:make() + self.sub.altimeter:make()
			if message.parameters.clouds_density > 2.0 and message.parameters.clouds_density < 9.0 then
				clouds_density = self.sub.scattered_clouds:make()
			elseif message.parameters.clouds_density >= 9.0 then
				clouds_density = self.sub.solid_layer:make()
			end
			if  message.parameters.clouds_density > 2.0 then
				local altitudeUnit = unitSystemByCountry[message.sender:getUnit():getCountry()].altitude
				local cloudAltIdx = u.adv_round(message.parameters.clouds_ceiling * altitudeUnit.coeff/1000, 1.0)
				cloudAltIdx = math.min(11,cloudAltIdx)
				clouds_density = clouds_density + space_ + self.sub.Altitude:make(cloudAltIdx)
			end
			
			local res
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit()) + comma_space_
			res = res + self.sub.visibility:make(visibility_distD) + comma_space_  + clouds_density + space_			
			res = res + self.sub.Pressure:make(message.parameters.pressure, aircraftType, '%.2f') + comma_space_
			if message.parameters.case == 0 then
				res = res + self.sub.marshal_report:make() + space_ + Digits:make(message.parameters.BRC * u.units.deg.coeff) + comma_space_
				res = res + self.sub.SeeMeAtTen:make()
			end		
			
			res = res  + self.sub._end:make()
			return 	 res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				Pressure				= Pressure,								
				visibility				= Phrases:new({	{_('mother\'s weather is visibility one mile'), 'visibility_is_1'},
													{_('mother\'s weather is visibility two miles'), 'visibility_is_2'},
													{_('mother\'s weather is visibility 3 miles'), 'visibility_is_3'},
													{_('mother\'s weather is visibility 4 miles'), 'visibility_is_4'},
													{_('mother\'s weather is visibility 5 miles'), 'visibility_is_5'},
													{_('mother\'s weather is visibility 6 miles'), 'visibility_is_6'},
													{_('mother\'s weather is visibility 7 miles'), 'visibility_is_7'},
													{_('mother\'s weather is visibility 8 miles'), 'visibility_is_8'},
													{_('mother\'s weather is visibility 9 miles'), 'visibility_is_9'},
													{_('mother\'s weather is visibility 10 miles'), 'visibility_is_10'},
													{_('mother\'s weather is visibility ten plus miles'), 'visibility_is_11'}}, 'Messages'),
				Altitude 				= Phrases:new({	{_('1000, altimeter is'), 'MARSHAL1K'},
													{_('2000, altimeter is'), 'MARSHAL2K'},
													{_('3000, altimeter is'), 'MARSHAL3K'},
													{_('4000, altimeter is'), 'MARSHAL4K'},
													{_('5000, altimeter is'), 'MARSHAL5K'},
													{_('6000, altimeter is'), 'MARSHAL6K'},
													{_('7000, altimeter is'), 'MARSHAL7K'},
													{_('8000, altimeter is'), 'MARSHAL8K'},
													{_('9000, altimeter is'), 'MARSHAL9K'},
													{_('10000, altimeter is'), 'MARSHAL10K'},
													{_('10000+, altimeter is'), 'MARSHAL10K+'}}, 'Messages'),
				sky_clear				= Phrase:new({_('clear'), 'clear'}, 'Messages'),
				altimeter				= Phrase:new({_(', altimeter is'), 'altimeter_is'}, 'Messages'),
				scattered_clouds 		= Phrase:new({_('scattered clouds at'), 'clouds_scattered'}, 'Messages'),
				solid_layer 			= Phrase:new({_('solid layer at'), 'solid_layer'}, 'Messages'),
				marshal_report      	= Phrase:new({_('CASE I recovery, expected BRC'),'case_I_recovery_expected_BRC'}, 'Messages'),
				marshal_report_case2    = Phrase:new({_('CASE II recovery, CV-1 approach, expected BRC'),'MARSHAL-74-CASE2'}, 'Messages'),
				marshal_report_case3    = Phrase:new({_('CASE III recovery, CV-1 approach, expected final bearing'),'MARSHAL-74-CASE3'}, 'Messages'),
				marshal_mothers			= Phrase:new({_('Marshal mother \'s'),'MARSHAL-MOTHERS'}, 'Messages'),
				radial					= Phrase:new({_('radial'),'RADIAL'}, 'Messages'),
				dme_angels				= Phrase:new({_('DME, angels'),'DME_angels'}, 'Messages'),
				eatTime					= Phrase:new({_('Expected approach time is'),'EAT_TIME'}, 'Messages'),
				approachButton			= Phrase:new({_('Approach button is 15'),'MARSHAL-15'}, 'Messages'),
				
				SeeMeAtTen				= Phrase:new({_('report see me at 10.'), 'report_see_me_at_10'}, 'Messages'),
				_start					= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end					= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),
			}
	},
		
	[base.Message.wMsgATCMarshallCopyInbound2and3]  = {
		make = function(self, message, language)	
			local country = message.receiver:getUnit():getCountry()
			local aircraftType = message.receiver:getUnit():getTypeName()
						
			local distanceUnit = unitSystemByCountry[country].distance
			local visibility_distD = base.math.max(1, base.math.floor(message.parameters.visibility * distanceUnit.coeff))			
			if message.parameters.visibility > 19000 then
				visibility_distD = 11
			end
			
			local clouds_density = self.sub.sky_clear:make() + self.sub.altimeter:make()
			if message.parameters.clouds_density > 2.0 and message.parameters.clouds_density < 9.0 then
				clouds_density = self.sub.scattered_clouds:make()
			elseif message.parameters.clouds_density >= 9.0 then
				clouds_density = self.sub.solid_layer:make()
			end
			if  message.parameters.clouds_density > 2.0 then
				local altitudeUnit = unitSystemByCountry[message.sender:getUnit():getCountry()].altitude				
				local cloudAltIdx = u.adv_round(message.parameters.clouds_ceiling * altitudeUnit.coeff/1000, 1.0)
				cloudAltIdx = math.min(11,cloudAltIdx)
				clouds_density = clouds_density + space_ + self.sub.Altitude:make(cloudAltIdx)
			end
			
			local res
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit()) + comma_space_
			--res = res + self.sub.visibility:make(visibility_distD) + comma_space_  + clouds_density + space_			
			
			if message.parameters.case == 0 then
				res = res + self.sub.marshal_report:make() + space_ + Digits:make(message.parameters.BRC * u.units.deg.coeff) + comma_space_
				res = res + self.sub.SeeMeAtTen:make()			
			else
				if message.parameters.case == 1 then
					--[[[SIDE NUMBER] flight, [SHIP CALLSIGN] marshal, CASE II recovery, CV-1 approach, expected BRC [CARRIER HEADING], altimeter [PRESSURE]. [SIDE NUMBER] flight, marshal mother’s [BEARING] radial, [DISTANCE] DME, angels [ALTITUDE]. Expected approach time is [TIME], approach button is 15.]]--
					res = res + self.sub.marshal_report_case2:make((message.parameters.carrierID or 74) - 67)  + space_
					res = res + Digits:make((message.parameters.BRC or 0 )* u.units.deg.coeff)				
				else
					--[[[SIDE NUMBER], [SHIP CALLSIGN] marshal, CASE III recovery, CV-1 approach, expected final bearing [BEARING], altimeter [PRESSURE]. [SIDE NUMBER], marshal mother’s [BEARING] radial, [DISTANCE] DME, angels [ALTITUDE]. Expected approach time is [TIME], approach button is button 15.]]--
					res = res + self.sub.marshal_report_case3:make((message.parameters.carrierID or 74) - 67)  + comma_space_
					res = res + self.sub.final_bearing:make() + space + Digits:make(message.parameters.BRC * u.units.deg.coeff)
				end
				res = res + self.sub.altimeter:make() + space_+ self.sub.Pressure:make(message.parameters.pressure, aircraftType, '%.2f') + comma_space_
				res = res + self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit()) + comma_space_
				res = res + self.sub.marshal_mothers:make() + space_ + Digits:make((message.parameters.RBRC or 0 )* u.units.deg.coeff) + space_+ self.sub.radial:make()  + comma_space_
				res = res + Digits:make((message.parameters.NumberInStack or 0) + 21) + space_+ self.sub.dme_angels:make() + space_ 
				res = res + Digits:make((message.parameters.NumberInStack or 0) + 6)  + comma_space_
				res = res + self.sub.eatTime:make() + space_ + Digits:make((message.parameters.EAT or 0), '%02d')  --+ comma_space_ + self.sub.approachButton:make()
			end		
			res = res  + self.sub._end:make()			
			return 	 res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				Pressure				= Pressure,								
				visibility				= Phrases:new({	{_('mother\'s weather is visibility one mile'), 'visibility_is_1'},
													{_('mother\'s weather is visibility two miles'), 'visibility_is_2'},
													{_('mother\'s weather is visibility 3 miles'), 'visibility_is_3'},
													{_('mother\'s weather is visibility 4 miles'), 'visibility_is_4'},
													{_('mother\'s weather is visibility 5 miles'), 'visibility_is_5'},
													{_('mother\'s weather is visibility 6 miles'), 'visibility_is_6'},
													{_('mother\'s weather is visibility 7 miles'), 'visibility_is_7'},
													{_('mother\'s weather is visibility 8 miles'), 'visibility_is_8'},
													{_('mother\'s weather is visibility 9 miles'), 'visibility_is_9'},
													{_('mother\'s weather is visibility 10 miles'), 'visibility_is_10'},
													{_('mother\'s weather is visibility ten plus miles'), 'visibility_is_11'}}, 'Messages'),
				Altitude 				= Phrases:new({	{_('1000, altimeter is'), 'MARSHAL1K'},
													{_('2000, altimeter is'), 'MARSHAL2K'},
													{_('3000, altimeter is'), 'MARSHAL3K'},
													{_('4000, altimeter is'), 'MARSHAL4K'},
													{_('5000, altimeter is'), 'MARSHAL5K'},
													{_('6000, altimeter is'), 'MARSHAL6K'},
													{_('7000, altimeter is'), 'MARSHAL7K'},
													{_('8000, altimeter is'), 'MARSHAL8K'},
													{_('9000, altimeter is'), 'MARSHAL9K'},
													{_('10000, altimeter is'), 'MARSHAL10K'},
													{_('10000+, altimeter is'), 'MARSHAL10K+'}}, 'Messages'),
				sky_clear				= Phrase:new({_('clear'), 'clear'}, 'Messages'),
				altimeter				= Phrase:new({_(', altimeter is'), 'altimeter_is'}, 'Messages'),
				scattered_clouds 		= Phrase:new({_('scattered clouds at'), 'clouds_scattered'}, 'Messages'),
				solid_layer 			= Phrase:new({_('solid layer at'), 'solid_layer'}, 'Messages'),
				marshal_report      	= Phrase:new({_('CASE I recovery, expected BRC'),'case_I_recovery_expected_BRC'}, 'Messages'),
				--marshal_report_case2    = Phrase:new({_('CASE II recovery, CV-1 approach, expected BRC'),'MARSHAL-74-CASE2'}, 'Messages'),	
				marshal_report_case2    = Phrases:new(	{{_('Old Salt marshal, CASE II recovery, CV-1 approach, expected BRC'),'MARSHAL-68-CASE2'},
														 {_('Ike marshal, CASE II recovery, CV-1 approach, expected BRC'),'MARSHAL-69-CASE2'},
														 {_('Gold Eagle marshal, CASE II recovery, CV-1 approach, expected BRC'),'MARSHAL-70-CASE2'},
														 {_('Rough Rider marshal, CASE II recovery, CV-1 approach, expected BRC'),'MARSHAL-71-CASE2'},
														 {_('Union marshal, CASE II recovery, CV-1 approach, expected BRC'),'MARSHAL-72-CASE2'},
														 {_('Warfighter marshal, CASE II recovery, CV-1 approach, expected BRC'),'MARSHAL-73-CASE2'},
														 {_('Courage marshal, CASE II recovery, CV-1 approach, expected BRC'),'MARSHAL-74-CASE2'},
														 {_('Lone Warrior marshal, CASE II recovery, CV-1 approach, expected BRC'),'MARSHAL-75-CASE2'},
														 {_('Freedom marshal, CASE II recovery, CV-1 approach, expected BRC'),'MARSHAL-76-CASE2'},
														 {_('Avenger marshal, CASE II recovery, CV-1 approach, expected BRC'),'MARSHAL-77-CASE2'},		 }, 'Messages'),				
				marshal_report_case3    = Phrases:new(	{{_('Old Salt marshal, CASE III recovery, CV-1 approach'),'MARSHAL-68-CASE3'},
														 {_('Ike marshal, CASE III recovery, CV-1 approach'),'MARSHAL-69-CASE3'},
														 {_('Gold Eagle marshal, CASE III recovery, CV-1 approach'),'MARSHAL-70-CASE3'},
														 {_('Rough Rider marshal, CASE III recovery, CV-1 approach'),'MARSHAL-71-CASE3'},
														 {_('Union marshal, CASE III recovery, CV-1 approach'),'MARSHAL-72-CASE3'},
														 {_('Warfighter marshal, CASE III recovery, CV-1 approach'),'MARSHAL-73-CASE3'},
														 {_('Courage marshal, CASE III recovery, CV-1 approach'),'MARSHAL-74-CASE3'},
														 {_('Lone Warrior marshal, CASE III recovery, CV-1 approach'),'MARSHAL-75-CASE3'},
														 {_('Freedom marshal, CASE III recovery, CV-1 approach'),'MARSHAL-76-CASE3'},
														 {_('Avenger marshal, CASE III recovery, CV-1 approach'),'MARSHAL-77-CASE3'},		 }, 'Messages'),
				final_bearing			= Phrase:new({_('expected final bearing '), 'expect_fb'}, 'Messages'),
				marshal_mothers			= Phrase:new({_('Marshal mother \'s'),'MARSHAL-MOTHERS'}, 'Messages'),
				radial					= Phrase:new({_('radial'),'RADIAL'}, 'Messages'),
				dme_angels				= Phrase:new({_('DME, angels'),'DME_angels'}, 'Messages'),
				eatTime					= Phrase:new({_('Expected approach time is'),'EAT_TIME'}, 'Messages'),
				approachButton			= Phrase:new({_('Approach button is 15'),'MARSHAL-15'}, 'Messages'),				
				SeeMeAtTen				= Phrase:new({_('report see me at 10.'), 'report_see_me_at_10'}, 'Messages'),
				_start					= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end					= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),
			}
	},
	
	--			//NAVY - answer to marshall after inbound call			
	--[[[base.Message.wMsgLeaderSeeYouAtTen]				= {
		make = function(self, message, language)
			local res			
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.sender:getUnit()) + comma_space_	
			res = res + self.sub.see_you_at_ten:make()
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	
				PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				see_you_at_ten			= Phrase:new({_('see you at 10.'), 'SEE_YOU'}, 'Messages'),
				_start					= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end					= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),
				}			
	},]]--
	--[TOWER], [SIDE NUMBER], overhead, angles [ALTITUDE], [NUMBER IN FLGHT], low state [REMAINING FUEL].
	[base.Message.wMsgLeaderTowerOverhead]				= {
		make = function(self, message, language)
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
	},	
	--[TOWER], [SIDE NUMBER], overhead, angles [ALTITUDE], [NUMBER IN FLGHT], low state [REMAINING FUEL].
	[base.Message.wMsgLeaderConfirm]				= {
		make = function(self, message, language)
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit())
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	},	
	
	[base.Message.wMsgLeaderConfirmRemainingFuel]				= {
		make = function(self, message, language)
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
	},	
	
	
	[base.Message.wMsgLeaderAirborn]				= {
		make = function(self, message, language)
			local res
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit())
			res = res + space_ + self.sub.airborn:make()
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				airborn				= Phrase:new({_(',airborn'), 'airborn'}, 'Messages'),
				_start					= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end					= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	},
	
	[base.Message.wMsgLeaderPassing2_5Kilo]				= {
		make = function(self, message, language)
			local res
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit())
			res = res + space_ + self.sub.passing2_5kilo:make()
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				passing2_5kilo			= Phrase:new({_(', passing 2.5. Kilo.'), 'PASSING_25'}, 'Messages'),
				_start					= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end					= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	},
		
	--[SIDE NUMBER], marshal on the [BEARING], range, [DME] angels [ALTITUDE]. Expected approach time [TIME], approach button is 15.
	[base.Message.wMsgLeaderInboundMarshallRespond]				= {
		make = function(self, message, language)
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
	},
	
	--[[[base.Message.wMsgATCMarshallReadbackCorrect]				= {
		make = function(self, message, language)
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit())+ comma_space_
			res = res  + self.sub.readbackCorrect:make()
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				readbackCorrect		= Phrase:new({_('readback correct.'), 'READBACK'}, 'Messages'),
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	},]]--
	--[SIDE NUMBER], Tower, Roger.  BRC is [CARRIER HEADING], your signal is Charlie.
	[base.Message.wMsgATCTowerCopyOverhead]  				= {
		make = function(self, message, language)							
			local res
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit()) + comma_space_
			res = res + self.sub.Tower_Roger:make() + space_
			--res = res + self.sub.case:make() + space_+ Digits:make(message.parameters.case) + space_ + self.sub.inEffect:make() + comma_space_
			res = res + self.sub.BRC:make() + space_+ Digits:make(message.parameters.BRC * u.units.deg.coeff) + comma_space_
			res = res + self.sub.signal:make()
			res = res  + self.sub._end:make()
			return 	 res
		end,
		sub = {	Tower_Roger			= Phrase:new({_('Tower, Roger.'), 'tower_roger'}, 'Messages'),
				case				= Phrase:new({_('case'), 'case'}, 'Messages'),
				inEffect			= Phrase:new({_('in effect'), 'in effect'}, 'Messages'),
				BRC 				= Phrase:new({_('BRC is'), 'BRC'}, 'Messages'),
				signal				= Phrase:new({_('signal is Charlie'), 'tower_CHARLIE'}, 'Messages')	,
				PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),
			}
	},
	
	[base.Message.wMsgLeaderHornetBall]				= {
		make = function(self, message, language)
				--Aircraft nickname indexes to select BallCall
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
	},
	
	[base.Message.wMsgATCLSORogerBall]  = { 
				--)				//NAVY - Roger ball, [WIND OVER DECK SPEED], [OPTIONAL DIRECTION]. - Answer on BALL
		make = function(self, message, language)							
			local res			
			res = self.sub._start:make() + self.sub.Roger_ball:make() 
			if (message.parameters.wind_speed) then
				local velocityUnit = unitSystemByCountry[message.receiver:getUnit():getCountry()].velocity
				local windSpeed = base.math.ceil(message.parameters.wind_speed * velocityUnit.coeff) - 20
				--res = res + comma_space_ + Digits:make(base.math.ceil(message.parameters.wind_speed * velocityUnit.coeff)) + space_ 
				--res = res + self.sub.knots:make() 
				if (message.parameters.wind_direction and windSpeed > 0 and windSpeed <= 10) then
					--res = res + comma_space_
					--[[if message.parameters.wind_direction == 1 then
						res = res + self.sub.port:make()
					elseif  message.parameters.wind_direction == 2 then
						res = res + self.sub.axial:make()
					else
						res = res + self.sub.starboard:make()
					end--]]
					if message.parameters.wind_direction == 2 then
						res = self.sub._start:make() + self.sub.down_angle:make(windSpeed)
					else
						res = self.sub._start:make() + self.sub.side_angle:make(windSpeed)
					end
				end
			end
			if message.parameters.glideslopeError and message.parameters.localizerError then			
				if message.parameters.glideslopeError == 0 and message.parameters.localizerError == 0 then
					res = res + comma_space_ + self.sub.glideslope_error:make(4) + comma_space_ + self.sub.localizer_error:make(4) 
				elseif base.math.abs(message.parameters.glideslopeError) <= 1 and base.math.abs(message.parameters.localizerError) <= 1 then
					res = res + comma_space_ + self.sub.fly_the_ball:make()
				else
					if base.math.abs(message.parameters.glideslopeError)> 1 and message.parameters.glideslopeError ~= 4 then
						res = res + comma_space_ + self.sub.glideslope_error:make(message.parameters.glideslopeError + 4)
					end
					if base.math.abs(message.parameters.localizerError)> 1 and message.parameters.localizerError ~= 4 then
						res = res + comma_space_ + self.sub.localizer_error:make(message.parameters.localizerError + 4)
					end
				end
			end
			res = res  + self.sub._end:make()
			return 	 res
		end,
		sub = {	Roger_ball			= Phrase:new({_('Roger ball'), 'LSO-ROGER-BALL'}, 'Messages'),
				down_angle			= Phrases:new({	{_('Roger ball, 21 knots, down the angle'), 'LSO-ROGERBALL-21-ANGLE'},
													{_('Roger ball, 22 knots, down the angle'), 'LSO-ROGERBALL-22-ANGLE'},
													{_('Roger ball, 23 knots, down the angle'), 'LSO-ROGERBALL-23-ANGLE'},
													{_('Roger ball, 24 knots, down the angle'), 'LSO-ROGERBALL-24-ANGLE'},
													{_('Roger ball, 25 knots, down the angle'), 'LSO-ROGERBALL-25-ANGLE'},
													{_('Roger ball, 26 knots, down the angle'), 'LSO-ROGERBALL-26-ANGLE'},
													{_('Roger ball, 27 knots, down the angle'), 'LSO-ROGERBALL-27-ANGLE'},
													{_('Roger ball, 28 knots, down the angle'), 'LSO-ROGERBALL-28-ANGLE'},
													{_('Roger ball, 29 knots, down the angle'), 'LSO-ROGERBALL-29-ANGLE'},
													{_('Roger ball, 30 knots, down the angle'), 'LSO-ROGERBALL-30-ANGLE'}},			'Messages'),
				side_angle			= Phrases:new({	{_('Roger ball, 21 knots'), 'LSO-ROGERBALL-21'},
													{_('Roger ball, 22 knots'), 'LSO-ROGERBALL-22'},
													{_('Roger ball, 23 knots'), 'LSO-ROGERBALL-23'},
													{_('Roger ball, 24 knots'), 'LSO-ROGERBALL-24'},
													{_('Roger ball, 25 knots'), 'LSO-ROGERBALL-25'},
													{_('Roger ball, 26 knots'), 'LSO-ROGERBALL-26'},
													{_('Roger ball, 27 knots'), 'LSO-ROGERBALL-27'},
													{_('Roger ball, 28 knots'), 'LSO-ROGERBALL-28'},
													{_('Roger ball, 29 knots'), 'LSO-ROGERBALL-29'},
													{_('Roger ball, 30 knots'), 'LSO-ROGERBALL-30'}},			'Messages'),
				
				glideslope_error	= Phrases:new({	{ _('you are low')				, 'LSO-LOW' },
													{ _('you are little low')		, 'LSO-LITTLE-LOW' },
													{empty_string, ''},
													{ _('you are on glideslope')	, 'LSO-ON-GLIDESLOPE' },
													{empty_string, ''},
													{ _('you are little high')		, 'LSO-LITTLE-HIGH' },
													{ _('you are high')				, 'LSO-HIGH' },
													{empty_string, ''}},				'Messages'),
				localizer_error		= Phrases:new({	{ _('you are lined up right')	, 'LSO-LINED-UP-RIGHT' },
													{ _('little come left')			, 'LSO-LITTLE-COME-LEFT' },
													{empty_string, ''},
													{ _('you are on centerline')	, 'LSO-CENTER' },
													{empty_string, ''},
													{ _('little right for line up')	, 'LSO-LITTLE-RIGHT' },
													{ _('you are lined up left')	, 'LSO-LINED-UP-LEFT' },
													{empty_string, ''}}	,	'Messages'),
													
				fly_the_ball		= Phrase:new(	{ _('fly the ball')				, 'LSO-FLY-THE-BALL' }		,   'Messages'),
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),
				--knots				= Phrase:new({_('knots'), 'knots'}),
				--port				= Phrase:new({_('port'), 'port'}),
				--axial 				= Phrase:new({_('axial'), 'axial'}),
				--starboard			= Phrase:new({_('starboard'), 'starboard'})				
			}
	},
		
	--[SIDE NUMBER], established angels [ALTITUDE]. State [FUEL LEVEL].
	[base.Message.wMsgLeaderEstablished]				= {
		make = function(self, message, language)
			local res			
			local low_state = self.sub.Digits:make(u.round(((message.sender:getUnit():getFuel() or 0)*message.sender:getUnit():getDesc().fuelMassMax * 2.2046/ 1000), 0.1), '%.1f')
			
			--local angelsAlt = self.sub.Digits:make(u.round( message.parameters.altitude*3.281/1000, 0.1), '%.1f')
			
			local Altitude = u.round( 2*message.parameters.altitude*3.281/1000, 1.0)
			local angelsAlt = ''
			if Altitude % 2 == 0 then
				angelsAlt = self.sub.Number:make(Altitude/2)
			else
				angelsAlt = self.sub.Digits:make(Altitude/2, '%.1f')
			end
						
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.sender:getUnit())
			res = res + comma_space_ 
			res = res + self.sub.established:make() + space_+ angelsAlt
			res = res + self.sub.state:make() + space_+ (low_state ~= 0 and (low_state) or '')
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	
				established				= Phrase:new({_('established angels'), 	'established'}, 'Messages'),
				PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				Digits					= Digits,
				Number					= Number,
				state					= Phrase:new({_('. State'), 'state'}, 'Messages'),
				_start					= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end					= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),
				}			
	},
	
	--[SIDE NUMBER] commencing, [ALTIMETER], state [FUEL LEVEL].
	[base.Message.wMsgLeaderCommencing]				= {
		make = function(self, message, language)
			local res			
			local low_state = self.sub.Digits:make(u.round(((message.sender:getUnit():getFuel() or 0)*message.sender:getUnit():getDesc().fuelMassMax * 2.2046/ 1000), 0.1), '%.1f')
			
			--local angelsAlt = self.sub.Digits:make(u.round( message.parameters.altitude*3.281/1000, 0.1), '%.1f')
			
			local Altitude = u.round( 2*message.parameters.altitude*3.281/1000, 1.0)
			local angelsAlt = ''
			if Altitude % 2 == 0 then
				angelsAlt = self.sub.Number:make(Altitude/2)
			else
				angelsAlt = self.sub.Digits:make(Altitude/2, '%.1f')
			end
						
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.sender:getUnit())+ comma_space_ 
			--res = res + self.sub.commencing:make() + comma_space_ + angelsAlt			
			res = res + self.sub.commencing:make() + comma_space_ 
			res = res + self.sub.state:make() + space_+ (low_state ~= 0 and (low_state) or '')+ comma_space_
			res = res+ self.sub.altimeter:make()+ space_
			res = res+ self.sub.Pressure:make(message.parameters.pressure, aircraftType, '%.2f') 			
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	
				commencing				= Phrase:new({_('commencing'), 	'commence'}, 'Messages'),
				PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				altimeter				= Phrase:new({_('altimeter'), 	'altimeter'}, 'Messages'),
				Digits					= Digits,
				Number					= Number,
				Pressure 				= Pressure,
				state					= Phrase:new({_('State'), 'state'}, 'Messages'),
				_start					= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end					= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),
				}			
	},
	
	--[SIDE NUMBER], roger, state [FUEL LEVEL].
	[base.Message.wMsgATCMarshallRogerState]				= {
		make = function(self, message, language)
			local low_state = self.sub.Digits:make(u.round(((message.receiver:getUnit():getFuel() or 0)*message.receiver:getUnit():getDesc().fuelMassMax * 2.2046/ 1000), 0.1), '%.1f')		
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit())+ comma_space_
			res = res  + self.sub.roger_state:make() + space_ + low_state
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				roger_state			= Phrase:new({_('roger, state'), 'ROGER_STATE'}, 'Messages'),
				Digits				= Digits,
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	},
	
	--[SIDE NUMBER], radar contact [DME] miles, expected final bearing 309.
	[base.Message.wMsgATCMarshallCopyCommencing]				= {
		make = function(self, message, language)
				
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit()) + comma_space_
			res = res  + self.sub.radar_contact:make() + space_ + self.sub.Digits:make(message.parameters.DME)+ space_ 
			res = res  + self.sub.miles:make() + space_ + self.sub.Digits:make(message.parameters.BRC * u.units.deg.coeff)
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				radar_contact			= Phrase:new({_('radar contact'), 'RADAR'}, 'Messages'),
				miles				= Phrase:new({_('miles, expected final bearing'), 'MARSHAL-FINAL'}, 'Messages'),
				Digits				= Digits,
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	},
	
		
	--[SIDE NUMBER], checking in, [DISTANCE TO CARRIER] miles.
	[base.Message.wMsgLeaderCheckingIn]				= {
		make = function(self, message, language)				
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.sender:getUnit()) + comma_space_
			res = res  + self.sub.checking_in:make() + comma_space_
			--res = res  + self.sub.Digits:make(message.parameters.DME)+ space_  + self.sub.miles:make()
			res = res  + self.sub.Number:make(message.parameters.DME)+ space_  + self.sub.miles:make()
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				checking_in			= Phrase:new({_('checking in'), 'checking_in'}, 'Messages'),
				miles				= Phrase:new({_('miles.'), 'miles'}, 'Messages'),
				Digits				= Digits,
				Number				= Number,
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	},
		
	--[SIDE NUMBER], final bearing [BEARING].
	[base.Message.wMsgATCTowerFinalBearing]				= {
		make = function(self, message, language)				
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit()) + comma_space_
			if message.parameters.case == 1 then
				res = res  + self.sub.b_r_c:make()
			else
				res = res + self.sub.approach:make()+ comma_space_
				res = res + self.sub.final_bearing:make()
			end
			res = res  + space_ + self.sub.Digits:make(message.parameters.BRC * u.units.deg.coeff)
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				final_bearing		= Phrase:new({_('final bearing'), 'FINAL'}, 'Messages'),
				approach			= Phrase:new({_('approach'), 'approach'}, 'Messages'),
				b_r_c				= Phrase:new({_('BRC'), 'BRC'}, 'Messages'),
				Digits				= Digits,
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	},
	
	--[SIDE NUMBER] final radar contact, [DISTANCE TO CARRIER] miles.
	[base.Message.wMsgATCTowerFinalContact]			= {
		make = function(self, message, language)				
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit()) + comma_space_
			res = res  + self.sub.final_contact:make() + comma_space_
			res = res  + self.sub.Digits:make(message.parameters.DME)+ space_  + self.sub.miles:make()
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				final_contact		= Phrase:new({_('final radar contact'), 'RADAR_CONTACT'}, 'Messages'),
				miles				= Phrase:new({_('miles.'), 'miles'}, 'Messages'),
				Digits				= Digits,
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	},
	
	--[SIDE NUMBER], ACLS lock on [DISTANCE TO CARRIER] miles, say needles.
	[base.Message.wMsgATCTowerSayNeedles]			= {
		make = function(self, message, language)				
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit()) + comma_space_
			res = res  + self.sub.lock_on:make() + comma_space_
			res = res  + self.sub.Digits:make(message.parameters.DME)+ space_  + self.sub.miles:make() + self.sub.needles:make()
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				lock_on				= Phrase:new({_('ACLS lock on'), 'ACLS'}, 'Messages'),
				miles				= Phrase:new({_('miles,'), 'miles'}, 'Messages'),
				needles				= Phrase:new({_('say needles.'), 'SAY_NEEDLES'}, 'Messages'),
				Digits				= Digits,
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	},
	
	--[SIDE NUMBERS], [GLIDEPATH][LOCALIZER].
	[base.Message.wMsgLeaderSayNeedle] = {
		make = function(self, message, language)				
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
	},
		
	--[SIDE NDUMBER], [GLIDEPATH LOCATION], [COURSE LOCATION], ¾ mile, call the ball.
	[base.Message.wMsgATCTowerCallTheBall] = {
		make = function(self, message, language)				
			local res		
			res = self.sub._start:make() +  self.sub.PlayerAircraftCallsign:make(message.receiver:getUnit()) + comma_space_
			res = res  + self.sub.glidepath:make(message.parameters.glidepath) + comma_space_
			res = res  + self.sub.localizer:make(message.parameters.localizer) + comma_space_
			res = res  + self.sub.callTheBall:make()
			res = res  + self.sub._end:make()
			return res
		end,
		sub = {	PlayerAircraftCallsign	= USNAVYPlayerAircraftCallsign,
				callTheBall			= Phrase:new({_('3/4 mile, call the ball.'), 'CALL_THE_BALL'}, 'Messages'),
				glidepath			= Phrases:new({	    {_('bellow glidepath'), 	'BELOW_GP'},
														{_('on glidepath'), 		'ON_GP'},
														{_('above glidepath'), 		'ABOVE_GP'},},			'Messages'),
				localizer			= Phrases:new({	    {_('left of course'), 		'LEFT_OF_CRS'},
														{_('on course'), 			'ON_CRS'},
														{_('right of course'), 		'RIGHT_OF_CRS'},},			'Messages'),
				_start				= Phrase:new(	{ empty_string, '_start' }		,   'Messages'),
				_end				= Phrase:new(	{ empty_string, '_end' }		,   'Messages'),}			
	},
	
	
	[base.Message.wMsgATCDepartureRadarContact] 		= Start_Receiver_Callsign_End_Handler, --  [SIDE NUMBER],radar contact, altimeter 29.92.
	[base.Message.wMsgATCDepartureClearedToSwitch]  	= Start_Receiver_Callsign_End_Handler,--[SIDE NUMBER], cleared to switch.
		
	[base.Message.wMsgATCMarshallCopyTen]			= Start_Receiver_Callsign_End_Handler, --StartEndHandler,	-- testers request to add Callsign for clearness
	
	[base.Message.wMsgATCMarshallSwitchApproach]	= Start_Receiver_Callsign_End_Handler,	--[SIDE NUMBER], switch approach.  
	[base.Message.wMsgLeaderPlatform]				= Start_Sender_Callsign_End_Handler,	--[SIDE NUMBER], platform.
	[base.Message.wMsgATCTowerRoger]				= Start_Receiver_Callsign_End_Handler, 	--[SIDE NUMBER], roger.	
	[base.Message.wMsgATCMarshallReadbackCorrect]	= Start_Receiver_Callsign_End_Handler,
	[base.Message.wMsgLeaderSeeYouAtTen]			= Start_Sender_Callsign_End_Handler,
	[base.Message.wMsgATCTowerConcurFlyMode2]		= Start_Receiver_Callsign_End_Handler,
	[base.Message.wMsgATCTowerSwitchMenu]			= Start_Receiver_Callsign_End_Handler, 	--[SIDE NUMBER], roger.	
	
	[base.Message.wMsgATCTowerFlyBullseye]			= Start_Receiver_Callsign_End_Handler,
	[base.Message.wMsgATCTowerApproachGlidepath] 	= Start_Receiver_Callsign_End_Handler,
		
	[base.Message.wMsgATCLSOWaveOFFGear]  			= StartEndHandler,				
	[base.Message.wMsgATCLSOWaveOFFFlaps]  			= StartEndHandler,				
	[base.Message.wMsgATCLSOWaveOFFWaveOFFWaveOFF]  = StartEndHandler,	
	[base.Message.wMsgATCLSOYoureHigh]  			= StartEndHandler,	
	[base.Message.wMsgATCLSOYoureLow]  				= StartEndHandler,	
	[base.Message.wMsgATCLSOYoureGoingHigh]  		= StartEndHandler,	
	[base.Message.wMsgATCLSOYoureGoingLow]  		= StartEndHandler,	
	[base.Message.wMsgATCLSOLinedUpLeft]  			= StartEndHandler,	
	[base.Message.wMsgATCLSOLinedUpRight]  			= StartEndHandler,	
	[base.Message.wMsgATCLSODriftingLeft]  			= StartEndHandler,	
	[base.Message.wMsgATCLSODriftingRight]  		= StartEndHandler,	
	[base.Message.wMsgATCLSOYoureFast] 			 	= StartEndHandler,	
	[base.Message.wMsgATCLSOYoureSlow]  			= StartEndHandler,	
	[base.Message.wMsgATCLSOEasyNose]  				= StartEndHandler,	
	[base.Message.wMsgATCLSOEasyWings]  			= StartEndHandler,	
	[base.Message.wMsgATCLSOEasyIt]  				= StartEndHandler,	
	[base.Message.wMsgATCLSOYoureHighClose]  		= StartEndHandler,	
	[base.Message.wMsgATCLSOPower]  				= StartEndHandler,	
	[base.Message.wMsgATCLSOPowerX2]  				= StartEndHandler,	
	[base.Message.wMsgATCLSOPowerX3]  				= StartEndHandler,	
	[base.Message.wMsgATCLSOEasyItX2]  				= StartEndHandler,	
	[base.Message.wMsgATCLSORight4LineUp]  			= StartEndHandler,	
	[base.Message.wMsgATCLSOLeft4LineUp]  			= StartEndHandler,	
	[base.Message.wMsgATCLSOFoulDeck]  				= StartEndHandler,	
	[base.Message.wMsgATCLSOBolterX3] 				= StartEndHandler,	
	[base.Message.wMsgATCLSOYoureLittleHigh]  		= StartEndHandler,	
	[base.Message.wMsgATCLSOYoureLittleLow]  		= StartEndHandler,	
	[base.Message.wMsgATCLSOLittleRight4LineUp]  	= StartEndHandler,	
	[base.Message.wMsgATCLSOLittleLeft4LineUp]  	= StartEndHandler,	
	[base.Message.wMsgATCLSOFlyTheBall]  			= StartEndHandler,	
	[base.Message.wMsgATCLSOOnGlideslope]  			= StartEndHandler,	
	[base.Message.wMsgATCLSOOnCenterLine]  			= StartEndHandler,	
	--[base.Message.wMsgATCLSOOnSpeed]  				= StartEndHandler,	
	
	[base.Message.wMsgATCClearedControlHover]		= {
		make = function(self, message, language)
			local wind = self.sub.Wind:make(message.parameters.wind)
			return self.sub.ATCToLeaderHandler:make(message, language) + (wind ~= nil and (comma_space_ + wind) or '')
		end,
		sub = {	ATCToLeaderHandler	= ATCToLeaderHandler,
				Wind				= Wind }
	},
	[base.Message.wMsgATCCheckLandingGear]			= {
		make = function(self, message, language)
			local wind = self.sub.Wind:make(message.parameters.wind)
			return	self.sub.ATCToLeaderHandler:make(message, language) + (wind ~= nil and (comma_space_ + wind) or '') +
					(message.parameters.runway ~= nil and (comma_space_ + self.sub.Runway:make(message.parameters.runway)) or '')
		end,
		sub = { ATCToLeaderHandler	= ATCToLeaderHandler,
				Runway				= Runway,
				Wind				= Wind }
	},
	[base.Message.wMsgATCFlightCheckIn]	= {
		make = function(self, message, language)
			local wind = self.sub.Wind:make(message.parameters.wind)
			return self.sub.ATCToLeaderHandler:make(message, language) + (wind ~= nil and (comma_space_ + wind) or '')
		end,
		sub = { ATCToLeaderHandler = ATCToLeaderHandler, Wind = Wind }
	},
	--PLAYER -> GROUND CREW
	[base.Message.wMsgLeaderGroundToggleElecPower] = {
		make = function(self, message)
			if message.parameters.on then
				return self.sub.on:make()
			else
				return self.sub.off:make()
			end
		end,
		sub = { on	= Phrase:new({_('request to turn on ground power'),		'request to turn on ground power'}),
				off	= Phrase:new({_('request to turn off ground power'),	'request to turn off ground power'}) }
	},
	[base.Message.wMsgLeaderGroundToggleWheelChocks] = {
		make = function(self, message)
			if message.parameters.on then
				return self.sub.on:make()
			else
				return self.sub.off:make()
			end
		end,
		sub = { on	= Phrase:new({_('request to place wheel chocks'),	'request to place the wheel chocks'}),
				off	= Phrase:new({_('request to remove wheel chocks'),	'request to remove wheel chocks'}) }
	},
	[base.Message.wMsgLeaderGroundToggleExhaustScreen] = {
		make = function(self, message)
			if message.parameters.on then
				return self.sub.on:make()
			else
				return self.sub.off:make()
			end
		end,
		sub = { on	= Phrase:new({_('request to place exhaust screen'),	'request to place the exhaust screen'}),
				off	= Phrase:new({_('request to remove exhaust screen'),'request to remove exhaust screen'}) }
	},
	[base.Message.wMsgLeaderGroundToggleCanopy] = {
		make = function(self, message)
			if message.parameters.on then
				return self.sub.on:make()
			else
				return self.sub.off:make()
			end
		end,
		sub = { on	= Phrase:new({_('request to open canopy'),	'open the canopy'}),
				off	= Phrase:new({_('request to close canopy'),	'close the canopy'}) }
	},
	[base.Message.wMsgLeaderGroundToggleAir] = {
		make = function(self, message)
			if message.parameters.on then
				return self.sub.on:make()
			else
				return self.sub.off:make()
			end
		end,
		sub = { on	= Phrase:new({_('request to connect ground air supply'),	'request to connect ground air supply'}),
				off	= Phrase:new({_('request to disconnect ground air supply'),	'request to disconnect ground air supply'}) }
	},
	[base.Message.wMsgLeaderGroundApplyAir] = {
		make = function(self, message)
			if message.parameters.on then
				return self.sub.on:make()
			end
		end,
		sub = { on	= Phrase:new({_('request to apply air'),	'request to apply air'}),
			  }
	},
	[base.Message.wMsgLeaderSpecialCommand] = {
		make = function(self, message)	
			if 	   message.parameters.type == 7   then  		return self.sub.ladder:make()-- fix alert message when stow ladder was called
			elseif message.parameters.type == 9   then  		return self.sub.INERTIAL_STARTER:make()
			elseif message.parameters.type == 15  then  		return self.sub.START_PRIMING_ENGINES:make()
			elseif message.parameters.type == 5   then  		
				if message.parameters.power_source == 0 then	return self.sub.USE_TURBO:make()
				else											return self.sub.USE_REGULAR:make()
				end
			elseif message.parameters.type   == 4 then
				if message.parameters.device == 0 then			return self.sub.HMS:make()
				elseif message.parameters.device == 1 then		return self.sub.NVG:make()
				elseif message.parameters.device == 2 then
                    return Phrase:new({_('request gun sight installation'), 'CR'}):make()
				elseif message.parameters.device == 3 then
                    return Phrase:new({_('request bomb sight installation'), 'CR'}):make()
				elseif message.parameters.device == 4 then
                    if message.parameters.action == 0 then
                        return Phrase:new({_('request removal of flare pistol'), 'CR'}):make()
                    elseif message.parameters.action == 1 then
                        return Phrase:new({_('request flare pistol installation'), 'CR'}):make()
                    else
                        return Phrase:new({_('request flare pistol change over'), 'CR'}):make()
                    end
				elseif message.parameters.device == 5 then
                    return Phrase:new({_('request blind screen installation'), 'CR'}):make()
				else
                    return Phrase:new({_('request cabin reconfiguration'), 'CR'}):make()
				end
			else
				return self.sub[message.parameters.name][message.parameters.value]:make()
			end
		end,		
		sub = {
			['EPPU'] = {
				sub = {
					[true]	= Phrase:new({_('request to turn on EPPU'),		'request to turn on EPPU'}),
					[false]	= Phrase:new({_('request to turn off EPPU'),	'request to turn off EPPU'}),
				}
			},
			ladder = Phrase:new({_('request to stow boarding ladder'),	'request to stow boarding ladder'}),
			HMS    = Phrase:new({_('request HMD installation'),	'request HMD installation'}),
			NVG    = Phrase:new({_('request NVG installation'),	'request NVG installation'}),
			USE_TURBO      = Phrase:new({_('request to turn on turbo-gear'),	'request to turn on turbo-gear'}),
			USE_REGULAR    = Phrase:new({_('request to turn off turbo-gear'),	'request to turn off turbo-gear'}),			
			INERTIAL_STARTER   = Phrase:new({_('run inertial starter'),	'Run the starter'}),
			START_PRIMING_ENGINES = Phrase:new({_("start priming engines"), 'Run the starter'}),
		}
	},
	--GROUND CREW -> PLAYER
	[base.Message.wMsgGroundDone] = {
		make = function(self, message)
			return self.sub[message.parameters.name][message.parameters.value]:make()
		end,		
		sub = {
			['EPPU'] = {
				sub = {
					[true]	= Phrase:new({_('EPPU is now on'),	'EPPU is now on'}),
					[false]	= Phrase:new({_('EPPU is now off'),	'EPPU is now off'}),
				}
			}
		}
	},

	--AI flight
	[base.Message.wMsgFlightAirbone]				= FlightMessageHandler,
	[base.Message.wMsgFlightPassingWaypoint] 		= PassingWaypointHandler,
	[base.Message.wMsgFlightOnStation]				= OnStationHandler,
	[base.Message.wMsgFlightDepartingStation]		= DepartingWaypointHandler,	
	[base.Message.wMsgFlightTallyBandit]			= BanditHandler,
	[base.Message.wMsgFlightTally] 					= TargetHandler,
	[base.Message.wMsgFlightEngagingBandit]			= BanditHandler,
	[base.Message.wMsgFlightEngaging] 				= EngagingHandler,
	[base.Message.wMsgFlightSplashBandit]			= BanditHandler,
	[base.Message.wMsgFlightTargetDestroyed]		= TargetHandler,
	[base.Message.wMsgFlightDefensive] 				= TargetHandler,
	[base.Message.wMsgFlightRTB]					= RTBHandler,
	[base.Message.wMsgFlightMemberDown]				= MemberDownHandler,
	[base.Message.wMsgFlightAerobaticTurnPreStart]	= TurnPreStartHandler,
	[base.Message.wMsgFlightAerobaticTurnStart]		= TurnStartHandler,
	[base.Message.wMsgFlightAerobaticTurnStop]		= TurnStopHandler,
}

handlersTable_mt = {
	__index = handlersTable
}

LeaderToATCHandler = {
	make = function(self, message, language)
		--base.print( '\t LeaderToATCHandler : make' )
		--base.print( '\t LeaderToATCHandler : AirbaseName:make :'.. self.sub.AirbaseName:make(message.receiver, getAirdromeNameVariant(language)))
		--base.print( '\t LeaderToATCHandler : PlayerAircraftCallsign:make :'.. self.sub.PlayerAircraftCallsign:make(message.sender, language == 'RUS'))
		--base.print( '\t LeaderToATCHandler : Event:make :'.. Event:make(message.event))		
		if message.receiver:getUnit():getDesc().category == base.Airbase.Category.SHIP and message.receiver:getUnit():getCoalition() == 2  then
			return 	self.sub.PlayerAircraftCallsign:make(message.sender,message.receiver, language == 'RUS') + comma_space_ + Event:make(message.event)
		else
			return 	self.sub.AirbaseName:make(message.receiver, getAirdromeNameVariant(language)) +
					comma_space_ + self.sub.PlayerAircraftCallsign:make(message.sender,message.receiver, language == 'RUS') + comma_space_ + Event:make(message.event)
		end
	end,
	sub = { PlayerAircraftCallsign	= PlayerAircraftCallsign,
			AirbaseName				= AirbaseName }
}

SimpleHandler = {
	make = function(self, message)
		return Event:make(message.event)
	end
}

rangeHandlersTable = {
	{
		range = { base.Message.wMsgLeaderEngageGroundTargets, 	base.Message.wMsgLeaderEngageNavalTargets },
		handler = {
			make = function(self, message)
				local result = self.sub.ToWingmen:make(message) + comma_space_ + Event:make(message.event)
				if message.parameters.atSPI then
					result = result + space_ + self.sub.atMySPI:make()
				end
				if message.parameters.weaponType ~= nil then
					result = result + comma_space_ + self.sub.Weapon:make(message.parameters.weaponType)
				end
				if message.parameters.atSPI then
					if message.parameters.direction then
						result = result + space_ + self.sub.FromCompassDirection8:make(message.parameters.direction)
					end
				end				
				return result 
			end,
			sub = {
				atMySPI					= Phrase:new({_('at my SPI'),	'at my SPI'}),
				ToWingmen				= ToWingmen,
				FromCompassDirection8	= FromCompassDirection8,				
				Weapon 					= Phrases:new( { 	{_('with missiles'), 		'with missiles'},
															{_('with unguided bombs'), 	'with unguided bombs'},
															{_('with guided bombs'), 	'with guided bombs'},
															{_('with rockets'), 		'with rockets'},
															{_('with markers'), 		'with markers'},
															{_('with guns'),			'with guns'}
														}, 
														'Weapon' )
			}
		}
	},
	--PLAYER -> WINGMAN
	{
		range = { base.Message.wMsgLeaderToWingmenNull, 		base.Message.wMsgLeaderToWingmenMaximum },
		handler = {
			make = function(self, message)
				return self.sub.ToWingmen:make(message) + comma_space_ + Event:make(message.event)
			end,
			sub = { ToWingmen = ToWingmen }
		}
	},
	--WINGMAN -> PLAYER
	{
		range = { base.Message.wMsgWingmenNull,					base.Message.wMsgWingmenMaximum },
		handler = WingmanMessageHandler
	},
	--PLAYER -> ATC
	{
		range = { base.Message.wMsgLeaderToATCNull,				base.Message.wMsgLeaderToATCMaximum },
		handler = LeaderToATCHandler
	},	
	--ATC -> PLAYER
	{
		range = { base.Message.wMsgATCNull, 					base.Message.wMsgATCMaximum },
		handler = {
			make = function(self, message)				
				if message.event < base.Message.wMsgATCLSORogerBall then 
					return  self.sub.PlayerAircraftCallsign:make(message.receiver,message.sender) + comma_space_ + Event:make(message.event)
				else
					return  Event:make(message.event)
				end
			end,
			sub = { PlayerAircraftCallsign = PlayerAircraftCallsign }
		}		
	},
	--BETTY
	{
		range = { base.Message.wMsgBettyNull, 					base.Message.wMsgBettyMaximum },
		handler = SimpleHandler
	},
	--ALMAZ
	{
		range = { base.Message.wMsgALMAZ_Null, 					base.Message.wMsgALMAZ_Maximum },
		handler = SimpleHandler
	},
	--RI65
	{
		range = { base.Message.wMsgRI65_Null, 					base.Message.wMsgRI65_Maximum },
		handler = SimpleHandler
	},
	--AutopilotAdjustment
	{
		range = { base.Message.wMsgAutopilotAdjustment_Null, 	base.Message.wMsgAutopilotAdjustment_Maximum },
		handler = SimpleHandler
	},
	--ExternalCargo
	{
		range = { base.Message.wMsgExternalCargo_Null, 	base.Message.wMsgExternalCargo_Maximum },
		handler = SimpleHandler	
	},
	--Mi8 Checklist
	{
		range = { base.Message.wMsgMi8_Checklist_Null, 	base.Message.wMsgMi8_Checklist_Maximum },
		handler = SimpleHandler	
	},
	--Mi8 Procedures Messages
	{
		range = { base.Message.wMsgMi8_CrewProcedures_Null, 	base.Message.wMsgMi8_CrewProcedures_Maximum },
		handler = SimpleHandler	
	},
	--A-10 VMU
	{
		range = { base.Message.wMsgA10_VMU_Null, 				base.Message.wMsgA10_VMU_Maximum },
		handler = SimpleHandler
	},
	-- PLAYER -> GROUND CREW
	{
		range = {base.Message.wMsgLeaderToGroundCrewNull,		base.Message.wMsgLeaderToGroundCrewMaximum },
		handler = SimpleHandler
	},	
	--GROUND CREW -> PLAYER
	{
		range = {base.Message.wMsgGroundCrewNull,				base.Message.wMsgGroundCrewMaximum },
		handler = SimpleHandler
	}
}

function findRangeHandler(self, event)
	for i, v in base.pairs(self.rangeHandlersTable) do
		if v.range[1] <= event and event <= v.range[2]  then
			return v.handler
		end
	end
	for i, v in base.pairs(base.getfenv().rangeHandlersTable) do
		if v.range[1] <= event and event <= v.range[2]  then
			return v.handler
		end
	end	
	return nil
end

--Languages

local language = {
	RUS	= 'RUS',
	ENG	= 'ENG',
	GER	= 'GER',
	FR	= 'FR',
	SPA = 'SPA',
	CHN = 'CHN',--by uboats
}

local defaultLanguage = language.ENG

local languageByCountry = {
	[nations.RUSSIA]		= language.RUS,
	[nations.UKRAINE]		= language.RUS,
	[nations.BELARUS]		= language.RUS,
	[nations.USA]			= language.ENG,
	[nations.UK]			= language.ENG,
	[nations.GERMANY]		= language.GER,
	[nations.FRANCE]		= language.FR,
	[nations.SPAIN]			= language.SPA,
	[nations.INSURGENTS]	= language.RUS,
	[nations.ABKHAZIA]		= language.RUS,
	[nations.SOUTH_OSETIA]	= language.RUS,	
	[nations.ITALY]			= language.ENG,
	[nations.AUSTRALIA]		= language.ENG,
	[nations.SWITZERLAND]	= language.GER,
	[nations.CHINA]			= language.CHN,--by uboats
	[nations.THIRDREICH]	= language.GER,
	[nations.YUGOSLAVIA]	= language.RUS,
	[nations.USSR]			= language.RUS,
	[nations.KAZAKHSTAN]	= language.RUS,
	[nations.GDR]			= language.RUS,
}

--Accents

local accent = base.country

local accentTable = {
	[language.ENG] = {
		[nations.RUSSIA]			= accent.RUSSIA,
		[nations.UKRAINE]			= accent.RUSSIA,
		[nations.BELARUS]			= accent.RUSSIA,
		[nations.USA]				= accent.USA,
		[nations.TURKEY]			= accent.USA,
		[nations.UK]				= accent.UK,
		[nations.FRANCE]			= accent.FRANCE,
		[nations.GERMANY]			= accent.GERMANY,
		[nations.THIRDREICH]		= accent.GERMANY,
		[nations.CANADA]			= accent.USA,
		[nations.SPAIN]				= accent.SPAIN,
		[nations.THE_NETHERLANDS]	= accent.GERMANY,
		[nations.BELGIUM]			= accent.FRANCE,
		[nations.NORWAY]			= accent.GERMANY,
		[nations.DENMARK]			= accent.GERMANY,
		[nations.ISRAEL]			= accent.USA,
		[nations.GEORGIA]			= accent.USA,
		[nations.INSURGENTS]		= accent.RUSSIA,
		[nations.ABKHAZIA]			= accent.RUSSIA,
		[nations.SOUTH_OSETIA]		= accent.RUSSIA,
		[nations.ITALY]						= accent.USA,
		[nations.ITALIAN_SOCIAL_REPUBLIC]	= accent.USA,
		[nations.AUSTRALIA]			= accent.UK,
        [nations.SWITZERLAND]		= accent.GERMANY,
		[nations.CHINA]				= accent.CHINA,--by uboats
		[nations.KAZAKHSTAN]		= accent.RUSSIA,
	}
}

role = {
	PLAYER		= 	{
						name = _('PLAYER'),
						dir = 'PLAYER',
						range = { base.Message.wMsgLeaderNull,  	base.Message.wMsgLeaderMaximum },
						singletone = true
					},
	PLAYER_NAVY		= 	{
						name = _('Player'),
						dir = 'NAVY_Player',
						range = { base.Message.wMsgLeaderToNavyATCNull,  	base.Message.wMsgLeaderToNavyATCMaximum },
						singletone = true
					},
	WINGMAN		= 	{
						name = _('WINGMAN'),
						dir = 'WINGMAN',
						range = { base.Message.wMsgWingmenNull, 	base.Message.wMsgWingmenMaximum },
					},	
	ATC			= 	{
						name = _('ATC'),
						dir = 'ATC',
						range = { base.Message.wMsgATCNull, 		base.Message.wMsgATCMaximum },
					},
	ATC_NAVY_DEPARTURE	= 	{
						name = _('Navy Departure'),
						dir = 'ATC_NAVY_Departure',
						range = { base.Message.wMsgATCNAVYDepartureNull, 		base.Message.wMsgATCNAVYDepartureMaximum },
					},
	ATC_NAVY_MARSHALL	= 	{
						name = _('Navy Marshall'),
						dir  = 'ATC_NAVY_Marshal',
						range = { base.Message.wMsgATCNAVYMarshalNull, 		base.Message.wMsgATCNAVYMarshalMaximum },
					},
	ATC_NAVY_APPROACH_TOWER	= 	{
						name = _('Navy Approach Tower'),
						dir  = 'ATC_NAVY_Approach_Tower',
						range = { base.Message.wMsgATCNAVYApproachTowerNull, 		base.Message.wMsgATCNAVYApproachTowerMaximum },
					},
	ATC_NAVY_LSO	= 	{
						name = _('Navy LSO'),
						dir = 'ATC_NAVY_LSO',
						range = { base.Message.wMsgATCNAVYLSONull, 		base.Message.wMsgATCNAVYLSOMaximum },
					},

					
	AWACS		= 	{
						name = _('AWACS'),
						dir = 'AWACS',
						range = { base.Message.wMsgAWACSNull,		base.Message.wMsgAWACSMaximum },
					},	
	TANKER		= 	{
						name = _('TANKER'),
						dir = 'TANKER',					
						range = { base.Message.wMsgTankerNull,		base.Message.wMsgTankerMaximum },
					},	
	JTAC		= 	{
						name = _('JTAC'),
						dir = 'JTAC',
						range = { base.Message.wMsgFACNull, 		base.Message.wMsgFACMaximum },
					},	
	CCC			= 	{
						name = _('CCC'),
						dir = 'CCC',					
						range = { base.Message.wMsgCCCNull, 		base.Message.wMsgCCCMaximum },
					},
	ALLIED_FLIGHT=	{
						name = _('Allied Flight'),
						dir = 'Allied Flight',
						range = { base.Message.wMsgFlightNull, 		base.Message.wMsgFlightMaximum },
					},	
	BETTY		= 	{
						name = _('BETTY'),
						dir = 'BETTY',						
						range = { base.Message.wMsgBettyNull,		base.Message.wMsgBettyMaximum },
						singletone = true
					},
	ALMAZ		=	{
						name = _('ALMAZ'),
						dir = 'ALMAZ',						
						range = { base.Message.wMsgALMAZ_Null,		base.Message.wMsgALMAZ_Maximum },
						singletone = true
					},
	RI65		=	{
						name = _('RI65'),
						dir = 'RI65',						
						range = { base.Message.wMsgRI65_Null,		base.Message.wMsgRI65_Maximum },
						singletone = true
					},					
	AutopilotAdjustment	=	{
						name = _('AutopilotAdjustment'),
						dir = '',						
						range = { base.Message.wMsgAutopilotAdjustment_Null,	base.Message.wMsgAutopilotAdjustment_Maximum },
						singletone = true
					},	
	ExternalCargo = {
						name = _('ExternalCargo'),
						dir = 'External Cargo',						
						range = { base.Message.wMsgExternalCargo_Null,	base.Message.wMsgExternalCargo_Maximum },
						singletone = true	
					},
	Mi8_Checklist	= 	{
						name = _('Mi-8 Checklist'),
						dir = '',						
						range = { base.Message.wMsgMi8_Checklist_Null,	base.Message.wMsgMi8_Checklist_Maximum },
						singletone = true
					},
	Mi8_CrewProcedures	= 	{
						name = _('Mi-8 Crew Procedures'),
						dir = '',						
						range = { base.Message.wMsgMi8_CrewProcedures_Null,	base.Message.wMsgMi8_CrewProcedures_Maximum },
						singletone = true
					},
	A10_VMU		= 	{
						name = _('A-10 VMU'),
						dir = 'A-10 VMU',						
						range = { base.Message.wMsgA10_VMU_Null,	base.Message.wMsgA10_VMU_Maximum },
						singletone = true
					},
	GROUND_CREW	= 	{
						name = _('Ground Crew'),
						dir = 'Ground Crew',
						range = { base.Message.wMsgGroundCrewNull,	base.Message.wMsgGroundCrewMaximum },
						singletone = true
					}
}

local function load_table(fn)
	local res = {}
	local c, err = base.loadfile(fn)
	if not c then
		base.print(err)
		return res
	end
	base.setfenv(c, res)
	local ok, err = base.pcall(c)
	if not ok then
		base.print(err)
	end
	return res
end

do
	local speechIndex = load_table('Scripts/Speech/_index.lua')
	--local _debug_print = function(str) base.print(str) end
	local _debug_print = function(str) end

	local function get_subdir(dir, n)
		-- NOTE: this is necessary to handle empty roleDirs
		if n == '' then return dir end
		return dir[n]
	end

	local function findVoices(dir, tbl)
		local voiceCounter = 0
		while get_subdir(dir, ''..(voiceCounter + 1)) do
			_debug_print('\t\t\tvoice '..(voiceCounter + 1)..' found')
			voiceCounter = voiceCounter + 1
		end
		if voiceCounter > 0 then
			tbl.voices = voiceCounter
		end	
	end	
	
	local function findAccents(dir, tbl)
		local found = false
		for accentName, accentValue in base.pairs(accent) do
			local subDir = get_subdir(dir, base.string.lower(accentName))
			if subDir then
				found = true
				_debug_print('\t\t\"'..accentName..'\" accent found')
				tbl.accents = tbl.accents or {}
				tbl.accents[accentValue] = {}				
				findVoices(subDir, tbl.accents[accentValue])
			end
		end
		if not found then
			findVoices(dir, tbl)
		end
	end
	
	local function findModules(dir, languageValue, roleDir, tbl)
		local langdir = get_subdir(dir, languageValue)
		if not langdir then return end
		for moduleName, modDir in base.pairs(langdir) do
			if moduleName ~= '.' and moduleName ~= '..' then
				if modDir then
					local subDir = get_subdir(modDir, base.string.lower(roleDir))
					if subDir then						
						_debug_print('\t\"'..moduleName..'\" - \"'..languageValue..'\" found')						
						local moduleTbl = nil
						if moduleName ~= nil then
							tbl.modules = tbl.modules or {}
							tbl.modules[moduleName] = tbl.modules[moduleName] or {}
							moduleTbl = tbl.modules[moduleName]
						end
						
						if moduleTbl.language ~= nil then
							moduleTbl.languages = {
								[moduleTbl.language] = {
									accents = moduleTbl.accents,
									voices = moduleTbl.voices
								},
								[languageValue] = {}
							}
							moduleTbl.language = nil
							moduleTbl.accents = nil
							moduleTbl.voices = nil
							findAccents(subDir, moduleTbl.languages[languageValue])
						elseif moduleTbl.languages ~= nil then
							moduleTbl.languages[languageValue] = {}
							findAccents(subDir, moduleTbl.languages[languageValue])
						else
							moduleTbl.language = languageValue
							findAccents(subDir, moduleTbl)
						end
					end
				end
			end
		end
	end
		
	local function findLanguages(dir, roleDir, tbl)
		-- Modules + langugages or languages
		for languageIndex, languageValue in base.pairs(language) do
			findModules(dir, languageValue, roleDir, tbl)
		end
	end
	
	for roleIndex, roleTbl in base.pairs(role) do
		_debug_print('\"'..roleTbl.dir..'\" role')
		findLanguages(speechIndex, roleTbl.dir, roleTbl)
	end
end

function findRole(self, event)
	for roleName, roleData in base.pairs(self.role) do
		if event > roleData.range[1] and event < roleData.range[2] then
			return roleData
		end
	end
end

function getHandler(self, event)
	local handler = self.handlersTable[event]	
	if handler == nil then
		handler = self:findRangeHandler(event)
	end
	if handler == nil then
		handler = self.SimpleHandler
	end	
	return handler
end

function makeCaption(self, role, sender, airdromeNameVariant)
	local caption = ''
	if role.singletone then
		caption = role.name..': '
	else
		if sender then
			local callsignString = makeCallsignString_(sender, airdromeNameVariant)
			if callsignString ~= nil then
				caption = role.name..' ('..callsignString..'): '
			end
		end
	end
	return caption
end

local function getMessageLanguage(roleData, language)
	if roleData.languages ~= nil then
		if language ~= nil then
			if roleData.languages[language] ~= nil then
				return language
			end
		else
			return defaultLanguage
		end
	elseif roleData.language ~= nil then
		if language ~= nil then
			if roleData.language == language then
				return language
			end
		else
			return roleData.language
		end
	else
		return nil
	end
end

local function getMessageModuleAndLanguage(roleData, module, language)
	if roleData.modules[module] ~= nil then
		local messageLanguage = getMessageLanguage(roleData.modules[module], language)
		if messageLanguage ~= nil then
			return { module, messageLanguage }
		end
	end
end

function Initialize()
	initAirdromeSubs()
end

--[[
function Release()
	clearAirdromeSubs()
	--base.print( '\t Common::AirbaseName : Release()' )
end
]]

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
		messageLanguage = messageModuleAndLanguage[2]
		roleData = roleData.modules[messageModuleName]
		if roleData == nil then
			base.print('make: module name = ', messageModuleName)
		end
	else
		messageLanguage = 	getMessageLanguage(roleData, desiredLanguage) or
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

--base.print('Speech.common modules loaded')