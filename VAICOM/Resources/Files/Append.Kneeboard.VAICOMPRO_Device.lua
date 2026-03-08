-- VAICOM PRO server-side script
-- VAICOM_Device.lua
-- www.vaicompro.com

package.path  = package.path..";.\\Scripts\\?.lua;"
package.path  = package.path..";.\\LuaSocket\\?.lua;"
package.cpath = package.cpath..";.\\LuaSocket\\?.dll;"

local	socket 		= require('socket')
local 	JSON    	= require('JSON')

local 	dev = GetSelf()
local 	dev_timer
local 	update_time_step = 0.02
local 	master_opacity = 0.5

make_default_activity(update_time_step)
need_to_be_closed = false

local sender
local receiver

local config = {
	sendaddress = "127.0.0.1",
	sendport = 51876,
	sendtimeout = 0,
	receiveaddress = "127.0.0.1",
	receiveport = 52341,
	receivetimeout = 0	
}
local start = function()	
	sender = socket.try(socket.udp()) 
	socket.try(sender:setpeername(config.sendaddress,config.sendport))
	socket.try(sender:settimeout(config.sendtimeout))	
	receiver = socket.try(socket.udp()) 
	socket.try(receiver:setsockname(config.receiveaddress,config.receiveport))
	socket.try(receiver:settimeout(config.receivetimeout))
end
local stop  = function()	
	if sender then
		socket.try(sender:close())
		sender = nil
	end	
	if receiver then
		socket.try(receiver:close())
		receiver = nil
	end
end

local stringstartswith = function(str, pat) 
    return string.find(str,'^'..pat) ~= nil
end
local file_exists = function(filename)
   local f=io.open(filename,"r")
   if f~=nil then 
	io.close(f) 
	return true 
   else 
	return false 
   end
end

getmodeltime = function()
  local seconds = get_model_time()
  if seconds <= 0 then
    return "00:00:00"
  else
    local hours= string.format("%02.f", math.floor(seconds/3600))
    local mins = string.format("%02.f", math.floor(seconds/60 - (hours*60)))
    local secs = string.format("%02.f", math.floor(seconds - hours*3600 - mins *60))
    return hours..":"..mins..":"..secs
  end
end

inserttime = function(show)
	if not show then
		return ""
	end
	local seconds = get_model_time()
	if seconds <= 0 then
		return "T+00.00"
	else
		local hours= string.format("%02.f", math.floor(seconds/3600))
		local mins = string.format("%02.f", math.floor(seconds/60 - (hours*60)))
		local secs = string.format("%02.f", math.floor(seconds - hours*3600 - mins *60))
		return "T+"..mins.."."..secs.." "
	end
end

dofile(LockOn_Options.common_script_path.."VAICOMPRO/device/VAICOMPRO_Common.lua")
dofile(LockOn_Options.common_script_path.."VAICOMPRO/device/command_defs.lua")

if file_exists(LockOn_Options.script_path.."devices.lua") then
	dofile(LockOn_Options.script_path.."devices.lua") 
end

local kneeboard = GetDevice(devices and devices["KNEEBOARD"] or 100)
local sensor_data = get_base_data()

function post_initialize() 
	init_logcats()
	init_logcategories()
	init_logkeywords()
	init_serverdata()
	init_unitsdata_all()
	init_messagelog_all()
	init_aliasdata_all()
	init_page_active()
	set_page_active("LOG")	
	stop()
	start()
	if kneeboard then
		kneeboard:performClickableAction(3004,1) 
	end
	dev_timer = 0
end

function SetCommand(command,value)
	if command == 3001 and kneeboard and value >0 then 
		kneeboard:performClickableAction(3006,0) 
	end	
	if command == 3002 and kneeboard then 
		kneeboard:performClickableAction(3005,value)
	end	
	if command == 3003 and kneeboard then
		kneeboard:performClickableAction(3004,1)
	end
	if command == 3004 and kneeboard then 
		kneeboard:performClickableAction(3002,1)
	end	
	if command == 3005 and kneeboard then
		kneeboard:performClickableAction(3001,1)
	end
	if command == 3006 then
		serverdata["dictmode"] = value
		Dictate_Status:set(serverdata["dictmode"])
	end	
	if command == 3010 then 
		set_page_active("ALL")
	end
	if command == 3011 then 
		set_page_active("LOG")
	end
	if command == 3012 then
		set_page_active("AWACS")
	end	
	if command == 3013 then
		set_page_active("JTAC")
	end	
	if command == 3014 then
		set_page_active("ATC")
	end
	if command == 3015 then
		set_page_active("TANKER")
	end	
	if command == 3016 then
		set_page_active("FLIGHT")
	end	
	if command == 3017 then
		set_page_active("AOCS")
	end
	if command == 3018 then
		set_page_active("REF")
	end	
	if command == 3019 then
		set_page_active("NOTES")
	end
	if command == 3020 then
		local opamsg = {}
		opamsg.opacity = value
		update_opacity(opamsg)
	end	
	update_contents()	
end

local function getdictmode()
	return serverdata["dictmode"] == 0 and "DICT OFF" or "[DICT ON]"
end
local function getdcsversion()
	return string.sub(tostring(_ED_VERSION),0,9) or ""
end
local function getaircraft()
	local acmod = serverdata["aircraft"] or get_aircraft_type()
	return acmod and string.sub(string.gsub(string.gsub(acmod, "-", ""),"_","") or "",0,5) or ""
end
local function getplayername()
	return serverdata["playerusername"] or ""  
end
local function getmultiplayer()
	return serverdata["multiplayer"] and "MP" or "SP"
end
local function getflightcount()
	return serverdata["groupcount"] > 0 and serverdata["groupcount"] or 1
end
local function getplayercallsign()
	return serverdata["playercallsign"] or ""
end
local function getmissiontitle()
	return serverdata["missiontitle"]  or ""
end
local function getcoalition()
	return serverdata["coalition"]  or "BLUE"
end
local function getmissionheader()
	return getcoalition() == "RED" and "Red Forces Air Command | TASKING ORDER" or "JFACC Joint Air Operations Planning | TASKING ORDER" 
end

Dictate_Status 		= get_param_handle("Dictate_Status")

Header_TopLeft  	= get_param_handle("Header_TopLeft") 
Header_TopMid   	= get_param_handle("Header_TopMid") 
Header_TopRight   	= get_param_handle("Header_TopRight") 
Header_BottomLeft 	= get_param_handle("Header_BottomLeft") 
Header_BottomRight  = get_param_handle("Header_BottomRight") 

init_logcats()

for a,b in pairs(logcats) do
	_G["Page_"..a] 				= get_param_handle("Page_"..a)
	_G["Page_Unitsdata_"..a] 	= get_param_handle("Page_Unitsdata_"..a)
	_G["Page_Unitsdetails_"..a] = get_param_handle("Page_Unitsdetails_"..a)
	for i = 1,4 do
		_G["Page_Logdata"..i.."_"..a] 	= get_param_handle("Page_Logdata"..i.."_"..a)
		_G["Page_Aliasdata"..i.."_"..a] = get_param_handle("Page_Aliasdata"..i.."_"..a)
	end
end

Master_Opacity  = get_param_handle("Master_Opacity") 
Smudge_Opacity  = get_param_handle("Smudge_Opacity") 

function update_Header_TopLeft()
	local headerstr = ""
	headerstr = string.sub("Notepad | "..get_page_active().." | " or "",0,45)
    Header_TopLeft:set(headerstr)
end
function update_Header_TopMid()
	local headerstr = ""
	headerstr = getdictmode()
    Header_TopMid:set(headerstr)
end
function update_Header_TopRight()
	local headerstr = ""
	headerstr = "| VAICOM PRO".." | "..getmodeltime()
    Header_TopRight:set(headerstr)
end
function update_Header_BottomLeft()
	local headerstr = ""
	headerstr = string.sub(getaircraft().." | 1/"..getflightcount().." | "..getplayercallsign().." | "..getmissiontitle(),0,45)
    Header_BottomLeft:set(headerstr)
end
function update_Header_BottomRight()
	local headerstr = "| "..getmultiplayer().." | "..getdcsversion() or ""
    Header_BottomRight:set(headerstr)
end

Global_angle = get_param_handle("Global_angle")
local angle = 0.5
Global_angle:set(angle*(3.14159265/180))

update_opacity = function(msg)
	if  LockOn_Options.screen.oculus_rift then 
		master_opacity = 1.0
	else
		master_opacity = msg.opacity or master_opacity
	end
    Master_Opacity:set(master_opacity)
end
get_page_opacity = function(str)
	return page_active[str] and master_opacity or 0 
end
update_Pages_Opacity = function()
	for i,j in pairs(logcats) do
		_G["Page_"..i]:set(get_page_opacity(i))
	end
end
update_Layers = function()
	local opac = master_opacity*(0.1 + 0.7*(get_model_time()/(6*3600)))
	Smudge_Opacity:set(opac <= 0.8 and opac or 0.8)
end 

local wraptext = function(cat, str, keeplines)
	local line = (cat == "LOG" or cat == "NOTES") and 55 or 40
	local formatstr = keeplines and str or str:gsub("\n", " ")
	local outputstr = ""
	for i = 0, string.len(formatstr)/line do	
		outputstr = outputstr..string.sub(formatstr,i*line,-1+(i+1)*line).."\n"
	end
	return outputstr
end

local linescount = function(str)
	local _, count = string.gsub(str, "\n", "")
	return count
end

local update_serverdata = function(receivedmsg)
	if receivedmsg.dictmode ~= nil then
		local activemode = serverdata["dictmode"]
		serverdata["dictmode"] = receivedmsg.dictmode and 1 or 0 
		Dictate_Status:set(serverdata["dictmode"])
	end
	if receivedmsg.autoswitch ~= nil then
		serverdata["autoswitch"] 		= receivedmsg.autoswitch
	end
	if receivedmsg and receivedmsg.serverdata then	
		serverdata["ato"]				= string.sub(tostring(os.time()),0,5) or ""
		serverdata["theater"] 			= receivedmsg.serverdata.theater or serverdata["theater"] 	
		serverdata["dcsversion"] 		= receivedmsg.serverdata.dcsversion	or serverdata["dcsversion"] 
		serverdata["aircraft"] 			= receivedmsg.serverdata.aircraft or serverdata["aircraft"] 	
		serverdata["groupcount"] 		= receivedmsg.serverdata.flightsize or serverdata["groupcount"] 	
		serverdata["playerusername"] 	= receivedmsg.serverdata.playerusername or serverdata["playerusername"] 
		serverdata["playercallsign"] 	= receivedmsg.serverdata.playercallsign or serverdata["playercallsign"]
		serverdata["missiontitle"] 		= receivedmsg.serverdata.missiontitle or serverdata["missiontitle"] 
		serverdata["coalition"] 		= receivedmsg.serverdata.coalition or serverdata["coalition"] 		
		serverdata["missionbriefing"] 	= receivedmsg.serverdata.missionbriefing or serverdata["missionbriefing"] 
		serverdata["missiondetails"] 	= receivedmsg.serverdata.missiondetails or serverdata["missiondetails"] 
		serverdata["sortie"] 			= receivedmsg.serverdata.sortie or serverdata["sortie"] 
		serverdata["task"] 				= receivedmsg.serverdata.task or serverdata["task"] 
		serverdata["country"] 			= receivedmsg.serverdata.country or serverdata["country"] 
		serverdata["multiplayer"] 		= receivedmsg.serverdata.multiplayer or serverdata["multiplayer"] 	
		unitsdata["LOG"] = getmissionheader().."\nDCS"..serverdata["ato"].." "..serverdata["task"].." / "..serverdata["playercallsign"].."("..serverdata["groupcount"]..") "..serverdata["sortie"].."\n("..getmultiplayer()..") "..serverdata["missiontitle"].."\n"..string.sub(serverdata["missiondetails"],0,55)
		messagelog["LOG"] = wraptext("LOG",serverdata["missionbriefing"])	
		unitsdata["NOTES"] = "Mission Notes "..serverdata["playercallsign"].." / "..wraptext("LOG",serverdata["playerusername"])
	end
 end
local update_unitsdata  = function(receivedmsg)
	if receivedmsg and receivedmsg.unitsdata then
		set_unitsdata(receivedmsg.unitsdata.category, receivedmsg.unitsdata.unitslist)
	end
end
local update_unitsdetails = function(receivedmsg)
	if receivedmsg and receivedmsg.unitsdetails then
		set_unitsdetails(receivedmsg.unitsdetails.category, receivedmsg.unitsdetails.unitsummary)
	end
end

local update_messagelog = function(receivedmsg)
	if receivedmsg and receivedmsg.logdata then
		if receivedmsg.switchpage or (serverdata["autoswitch"] and receivedmsg.logdata.cat == receivedmsg.logdata.cont) then
			dev:performClickableAction(3001,1)
		end	
		for i,j in pairs(logcats) do
			local cat = i	
			local entry = receivedmsg.logdata.content and (stringstartswith(receivedmsg.logdata.content,logcategories[cat]) or stringstartswith(receivedmsg.logdata.content,logkeywords[cat]))
			if receivedmsg.logdata.category == cat or entry then
				if receivedmsg.switchpage then
					set_page_active(cat)
				end
				if receivedmsg.logdata.content then		
					if cat == "NOTES" and receivedmsg.logdata.content == "" then
						messagelog[cat] = ""
					else
						local append = receivedmsg.logdata.content:gsub(logkeywords[cat], ""):gsub(logcategories[cat], "")
						append = string.sub(append,2)
						local timestamp = stringstartswith(append,"*")
						if timestamp then
							append = string.sub(append,2)
						end
						if append ~= "" then
							if cat == "NOTES" then 
								messagelog[cat] = mergelog("",receivedmsg.logdata.content:gsub(logkeywords[cat], ""))	
							else
								messagelog[cat] = mergelog(messagelog[cat],inserttime(timestamp)..append)	
							end
						end
					end
				end
			end
		end		
	end
end
local update_aliasdata  = function(receivedmsg)
	if receivedmsg and receivedmsg.aliasdata then
		set_aliasdata(receivedmsg.aliasdata.category, receivedmsg.aliasdata.content,receivedmsg.aliasdata.chunk or 0 )
	end
end

function process_message(msg)
	update_opacity(msg)
	update_serverdata(msg)
	update_unitsdata(msg)
	update_unitsdetails(msg)
	update_messagelog(msg)
	update_aliasdata(msg)
end

function update_contents()
	update_Pages_Opacity()
	update_Header_TopLeft()
	update_Header_TopMid()
	update_Header_TopRight()
	update_Header_BottomLeft()
	update_Header_BottomRight()	
	for i,j in pairs(logcats) do
		_G["Page_Unitsdata_"..i]:set(unitsdata[i])
		_G["Page_Unitsdetails_"..i]:set(unitsdetails[i])
		_G["Page_Logdata1_"..i]:set(messagelog[i])
		_G["Page_Logdata2_"..i]:set(wraptext(i,"",true))
		_G["Page_Logdata3_"..i]:set(wraptext(i,"",true))
		_G["Page_Logdata4_"..i]:set(wraptext(i,"",true))
		_G["Page_Aliasdata1_"..i]:set(aliasdata[1][i])
		_G["Page_Aliasdata2_"..i]:set(aliasdata[2][i])
		_G["Page_Aliasdata3_"..i]:set(aliasdata[3][i])
		_G["Page_Aliasdata4_"..i]:set(aliasdata[4][i])	
	end
end

local last_update_time = 0

function update()
    dev_timer = dev_timer + update_time_step
    local datareadout = receiver and receiver:receive()
    receiveddata = datareadout or false
    if receiveddata then
        receivedmsg = DecodeMessage(receiveddata)
        process_message(receivedmsg or "")
    end

    -- Add a 1-second delay before calling update_contents
    if dev_timer - last_update_time >= 1 then
        update_contents()
        last_update_time = dev_timer
    end

    update_Layers()
    update_Header_TopMid()
    update_Header_TopRight()
end

function DecodeMessage(rawdata)
	local decodeerror = false	
	function JSON:onDecodeError(message, text, location, etc)
		if not decodeerror then
		end
		decodeerror = true
	end
	local msg = JSON:decode(rawdata)
	if decodeerror then 
		return nil 
	end
	if type(msg) ~= "table"  then 
		decodeerror = true
		return nil
	end
	return msg
end		