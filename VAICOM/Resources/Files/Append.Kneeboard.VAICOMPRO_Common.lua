-- VAICOM PRO server-side script
-- VAICOMPRO_Common.lua
-- www.vaicompro.com

logcats = {}
init_logcats = function()
	logcats["ALL"]   	= "ALL"
	logcats["LOG"]   	= "LOG"
	logcats["AOCS"]   	= "AOCS"
	logcats["JTAC"]   	= "JTAC"
	logcats["TANKER"]   = "TANKER"
	logcats["AWACS"]   	= "AWACS"
	logcats["ATC"]   	= "ATC"
	logcats["FLIGHT"]   = "FLIGHT"
	logcats["REF"]	 	= "REF"
	logcats["NOTES"]   	= "NOTES"
end

headers = {}
init_headers = function()
	headers[1] = { "TopLeft", "L", 0, "T", "LeftCenter"	    }
	headers[2] = { "TopMid", "R", 4, "T", "RightCenter"	    }
	headers[3] = { "TopRight", "R", 0, "T", "RightCenter"	} 
	headers[4] = { "BottomLeft", "L", 0, "B", "LeftCenter"  }
	headers[5] = { "BottomRight", "R", 0, "B", "RightCenter"}
end

messagelog = {} 
init_messagelog_all = function()
	for a,b in pairs(logcats) do
		messagelog[a] = ""
	end
end
init_messagelog = function(str)
	messagelog[str] = ""
end
set_messagelog = function(cat, content)
	messagelog[cat] = content or ""
end
get_messagelog = function(cat)
	return messagelog[cat] or ""
end

aliasdata = {}
for i= 1,4 do 
	aliasdata[i] = {}
end
init_aliasdata_all = function()
	for a,b in pairs(logcats) do	
		for i= 1,4 do 
			aliasdata[i][a] = ""
		end
	end
end
init_aliasdata = function(str)
	for i= 1,4 do 
		aliasdata[i][str] = ""
	end	
end
set_aliasdata = function(cat, content, chunk)		
	if chunk == 0 then
		init_aliasdata(cat)
	end
	local n = 2 * 12 * chunk 
	for i,j in pairs(content) do
		n = n + 1	
		dostart = chunk == 1 and 3 or 1
		doend = chunk == 1 and #aliasdata or 2
		for p = dostart,doend do
			if (n > (p-1)*12 and n <= p*12 and n <= 46) then
				aliasdata[p][cat] = aliasdata[p][cat]..i
				if #j >= 1 and j[1] ~= "" then
					aliasdata[p][cat] = aliasdata[p][cat].." "
				end			
				for k = 1, #j do
					if string.len(aliasdata[p][cat]) + string.len(j[k]) < 500 then
						if k == 1 then 
							aliasdata[p][cat] = aliasdata[p][cat]..j[k]
						else
							aliasdata[p][cat] = aliasdata[p][cat].." / "..j[k]
						end
					end	
				end
				aliasdata[p][cat] = aliasdata[p][cat].." |".."\n"	
			end	
		end		
	end
end
get_aliasdata = function(cat)
	append = ""
		for i= 1,4 do 
			append = append..aliasdata[i][cat] 
		end
	return append
end

unitsdata = {} 
init_unitsdata_all = function()
	for a,b in pairs(logcats) do
		unitsdata[a] = "N/A"
	end	
end
init_unitsdata = function(str)
	unitsdata[str] = "N/A"		
end
set_unitsdata = function(cat, content)
	unitsdata[cat] = "ATO DCS"..serverdata["ato"].." / "..cat.."\n"
	if #content == 0 then
		unitsdata[cat] = unitsdata[cat].."N/A"
	else
       for i= 1, #content do
		  if i <= 7 then
             local line = content[i] or ""
				if line == "" then
					unitsdata[cat] = unitsdata[cat].."\n"
				else
                   unitsdata[cat] = unitsdata[cat]..line.."\n"
				end
			end
		end
	end
end
get_unitsdata = function(cat)
	return unitsdata[cat] or ""
end

getlines = function(txt)
	local array = {}
	local count = 0
	for s in txt:gmatch("[^\r\n]+") do
		count = count + 1
		array[count] = s
	end
	return array, count
end

mergearrays = function(t1,t2)
	local l = #t1
	for i = 1, #t2 do 
		t1[l+i] = t2[i] 
	end
    return t1
end

mergelog = function(log1,log2)
	local lines1 = getlines(log1)
	local lines2 = getlines(log2)
	local merged = {}
	merged = mergearrays(lines1,lines2)
	local mlog = ""
	local lim = #merged<27 and #merged or 27
	local offset = #merged - lim > 0 and #merged - lim or 0
	for i = offset + 1,offset + lim do
        mlog = mlog..merged[i].."\n"
    end
	return mlog
end

unitsdetails = {} 
init_unitsdetails_all = function()
	for a,b in pairs(logcats) do
		unitsdetails[a] = ""
	end
end
init_unitsdetails = function(str)
	unitsdetails[str] = ""	
end
set_unitsdetails = function(cat, content)
	unitsdetails[cat] = ""
   for i= 1, #content do
	   if i<= 7 then
			unitsdetails[cat] = content[i]
			messagelog[cat] = mergelog(messagelog[cat],content[i])	
		end
	end
end
get_unitsdetails = function(cat)
	return unitsdetails[cat] or ""
end

logcategories = {}
init_logcategories = function()
	for a,b in pairs(logcats) do
		logcategories[a] = a
	end
end

logkeywords = {}
init_logkeywords = function()
	for a,b in pairs(logcats) do
		logkeywords[a] = b
	end
	logkeywords["FLIGHT"] = "WINGMAN"
	logkeywords["ALL"] = "ALLIES"
end

serverdata = {}
init_serverdata = function()
	serverdata["ato"]				= ""
	serverdata["theater"] 			= ""
	serverdata["autoswitch"] 		= false
	serverdata["dictmode"] 			= 0
	serverdata["dcsversion"]		= ""
	serverdata["aircraft"]			= ""
	serverdata["groupcount"]		= 0
	serverdata["playerusername"]	= ""
	serverdata["playercallsign"]	= ""
	serverdata["coalition"]			= ""
	serverdata["missiontitle"]		= ""
	serverdata["missionbriefing"]	= ""
	serverdata["missiondetails"]	= ""
	serverdata["sortie"] 			= ""
	serverdata["task"] 				= ""
	serverdata["country"] 			= ""
	serverdata["multiplayer"]		= false
end

page_active = {}
init_page_active = function()
	for a,b in pairs(logcats) do
		page_active[a] = false
	end
end
set_page_active = function(str)
	for page, state in pairs(page_active) do	
		page_active[page] = page == str
	end
end
get_page_active = function()
	for page, state in pairs(page_active) do	
		if state then
			return page == "LOG" and "ATO" or page
		end
	end
end