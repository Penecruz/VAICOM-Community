-- VAICOM PRO server-side script (Optimized Version)
-- Original: VAICOMPRO.export.lua
-- Vaicom Discord at https://discord.gg/7c22BHNSCS 

package.path  = package.path..";.\\LuaSocket\\?.lua;" -- use windows path format for LuaSocket due to some picky systems
package.cpath = package.cpath..";.\\LuaSocket\\?.dll;"

local socket = require("socket")
local base = _G

-- Debugging
local DEBUG = false
local function log(msg)
    if DEBUG then
        base.print("[VAICOM] " .. msg)
    end
end

local function safe_tostring(v)
    if v == nil then return "nil" end
    return tostring(v)
end

local function serialize_payload(payload)
    if type(payload) ~= "table" then
        return safe_tostring(payload)
    end

    local parts = {}

    if payload.CurrentWeapon ~= nil then
        table.insert(parts, "CurrentWeapon=" .. safe_tostring(payload.CurrentWeapon))
    end
    if type(payload.Cannon) == "table" then
        if payload.Cannon.shells ~= nil then
            table.insert(parts, "Cannon.shells=" .. safe_tostring(payload.Cannon.shells))
        end
    elseif payload.Cannon ~= nil then
        table.insert(parts, "Cannon=" .. safe_tostring(payload.Cannon))
    end
    if payload.Chaff ~= nil then
        table.insert(parts, "Chaff=" .. safe_tostring(payload.Chaff))
    end
    if payload.Flare ~= nil then
        table.insert(parts, "Flare=" .. safe_tostring(payload.Flare))
    end

    if type(payload.Stations) == "table" then
        local stationParts = {}
        for i, station in ipairs(payload.Stations) do
            if type(station) == "table" then
                local clsid = safe_tostring(station.CLSID)
                local count = safe_tostring(station.count)
                stationParts[#stationParts + 1] = i .. ":" .. clsid .. "x" .. count
            else
                stationParts[#stationParts + 1] = i .. ":" .. safe_tostring(station)
            end
        end
        table.insert(parts, "Stations=" .. table.concat(stationParts, "|"))
    end

    -- Capture any other simple top-level payload fields that may be module-specific
    for k, v in pairs(payload) do
        local key = safe_tostring(k)
        if key ~= "CurrentWeapon" and key ~= "Cannon" and key ~= "Chaff" and key ~= "Flare" and key ~= "Stations" then
            local vt = type(v)
            if vt == "number" or vt == "string" or vt == "boolean" then
                table.insert(parts, key .. "=" .. safe_tostring(v))
            end
        end
    end

    return table.concat(parts, ";")
end

-- Socket utility function
local function create_udp_socket(address, port, timeout, is_server)
    local sock = socket.try(socket.udp())
    if is_server then
        socket.try(sock:setsockname(address, port))
    else
        socket.try(sock:setpeername(address, port))
    end
    socket.try(sock:settimeout(timeout))
    return sock
end

vaicom = {}

vaicom.config = {
    sendtoradio = { address = "127.0.0.1", port = 33334, timeout = 0 },
    receivefromclient = { address = "*", port = 33491, timeout = 0 },
    sendtoclient = { address = "127.0.0.1", port = 33492, timeout = 0 },
    beaconclose = "missiondata.update.beacon.unlock",
    ah64stateprefix = "missiondata.update.ah64state"
}

vaicom.insert = {

    probe = {
        enabled = true,
        debugenabled = false,
        intervalSeconds = 0.25,
        lastPoll = 0,
        lastState = nil,
        lastWeaponState = nil,
        logfile = nil,
        keylogfile = nil,
        statusfile = nil,
        loggedKeys = {},
    },

    ClassifyPayloadWeapons = function(self, payload)
        local gun = false
        local rockets = false
        local missiles = false

        local function contains_any_token(value, tokens, depth)
            depth = depth or 0
            if depth > 3 then return false end

            local vt = type(value)
            if vt == "string" then
                local text = string.upper(value)
                for _, token in ipairs(tokens) do
                    if string.find(text, token, 1, true) then
                        return true
                    end
                end
                return false
            end

            if vt == "table" then
                for k, v in pairs(value) do
                    if contains_any_token(k, tokens, depth + 1) or contains_any_token(v, tokens, depth + 1) then
                        return true
                    end
                end
            end

            return false
        end

        local function is_rocket_clsid(clsid)
            return string.find(clsid, "M261", 1, true)
                or string.find(clsid, "HYDRA", 1, true)
                or string.find(clsid, "APKWS", 1, true)
        end

        local function is_missile_clsid(clsid)
            return string.find(clsid, "AGM_114", 1, true)
                or string.find(clsid, "HELLFIRE", 1, true)
                or clsid == "{88D18A5E-99C8-4B04-B40B-1C02F2018B6E}"
        end

        local function is_guid_clsid(clsid)
            return string.find(clsid, "{", 1, true)
                and string.find(clsid, "}", 1, true)
                and string.find(clsid, "-", 1, true)
        end

        if type(payload) ~= "table" then
            return gun, rockets, missiles
        end

        if type(payload.Cannon) == "table" then
            local shells = tonumber(payload.Cannon.shells) or 0
            gun = shells > 0
        end

        if type(payload.Stations) == "table" then
            for _, station in ipairs(payload.Stations) do
                if type(station) == "table" then
                    local count = tonumber(station.count) or 0
                    if count > 0 then
                        local clsid = string.upper(safe_tostring(station.CLSID))
                        local hasRocketToken = contains_any_token(station, {"M261", "HYDRA", "APKWS", "ROCKET"})
                        local hasMissileToken = contains_any_token(station, {"AGM_114", "HELLFIRE", "MISSILE"})
                        local hasNonWeaponToken = contains_any_token(station, {"EFT", "FUEL", "TANK", "IAFS", "EMPTY"})

                        if is_missile_clsid(clsid) or hasMissileToken then
                            missiles = true
                        end

                        if is_rocket_clsid(clsid) or hasRocketToken then
                            rockets = true
                        end

                        if not missiles and not rockets and is_guid_clsid(clsid) and not hasNonWeaponToken then
                            missiles = true
                        end
                    end
                end
            end
        end

        return gun, rockets, missiles
    end,

    DetectOnGroundState = function(self)
        if type(LoGetMechInfo) == "function" then
            local ok, mech = pcall(LoGetMechInfo)
            if ok and type(mech) == "table" then
                for k, v in pairs(mech) do
                    if type(k) == "string" and string.find(string.upper(k), "WOW", 1, true) then
                        if v == true then
                            return true
                        end
                        if type(v) == "number" and v > 0 then
                            return true
                        end
                    end
                end
            end
        end

        if type(LoGetSelfData) == "function" then
            local ok, selfData = pcall(LoGetSelfData)
            if ok and type(selfData) == "table" then
                if selfData.InAir ~= nil then
                    return not selfData.InAir
                end
            end
        end

        return false
    end,

    SendWeaponStateUpdate = function(self, payload)
        if not vaicom.sendtoclient then return end

        local gun, rockets, missiles = self:ClassifyPayloadWeapons(payload)
        local wow = self:DetectOnGroundState()
        local msg = string.format("%s;gun=%d;rockets=%d;missiles=%d;wow=%d", vaicom.config.ah64stateprefix, gun and 1 or 0, rockets and 1 or 0, missiles and 1 or 0, wow and 1 or 0)

        if msg == self.probe.lastWeaponState then
            return
        end

        self.probe.lastWeaponState = msg
        pcall(function() vaicom.sendtoclient:send(msg) end)
    end,

    WriteProbeStatus = function(self, msg)
        if self.probe and self.probe.statusfile then
            self.probe.statusfile:write(string.format("%.3f;%s\n", socket.gettime(), safe_tostring(msg)))
            self.probe.statusfile:flush()
        end
    end,

    IsProbeFileLoggingEnabled = function(self)
        return self.probe and self.probe.debugenabled == true
    end,

    UpdateProbeDebugFromClientMessage = function(self, raw)
        if type(raw) ~= "string" then
            return
        end

        local enabled = nil
        if string.find(raw, '"debug"%s*:%s*true') then
            enabled = true
        elseif string.find(raw, '"debug"%s*:%s*false') then
            enabled = false
        end

        if enabled == nil then
            return
        end

        if self.probe.debugenabled ~= enabled then
            self.probe.debugenabled = enabled
            if not enabled then
                self:CloseProbe()
            end
        end
    end,

    EnsureProbeFiles = function(self, enableFileLogging)
        if not enableFileLogging then
            self:CloseProbe()
            return
        end

        if self.probe.logfile and self.probe.keylogfile and self.probe.statusfile then
            return
        end

        local ok, lfs = pcall(require, "lfs")
        if not ok or not lfs or type(lfs.writedir) ~= "function" then
            log("Probe init failed: lfs.writedir unavailable")
            return
        end

        if not self.probe.statusfile then
            local statusPath = lfs.writedir() .. [[Logs\VAICOM_AH64D_ExportProbe_Status.csv]]
            local sf = io.open(statusPath, "a")
            if sf then
                self.probe.statusfile = sf
                self:WriteProbeStatus("InitProbe")
            end
        end

        if not self.probe.logfile then
            local path = lfs.writedir() .. [[Logs\VAICOM_AH64D_ExportProbe.csv]]
            local f, err = io.open(path, "a")
            if not f then
                log("Probe init failed: " .. safe_tostring(err))
                self:WriteProbeStatus("Main log open failed: " .. safe_tostring(err))
                return
            end

            self.probe.logfile = f
            self.probe.logfile:write("time;module;unit;payload\n")
            self.probe.logfile:flush()
        end

        if not self.probe.keylogfile then
            local keyPath = lfs.writedir() .. [[Logs\VAICOM_AH64D_ExportProbe_Keys.csv]]
            local kf, kerr = io.open(keyPath, "a")
            if not kf then
                log("Probe key file init failed: " .. safe_tostring(kerr))
                self:WriteProbeStatus("Key log open failed: " .. safe_tostring(kerr))
            else
                self.probe.keylogfile = kf
                self.probe.keylogfile:write("time;module;key;type\n")
                self.probe.keylogfile:flush()
                self:WriteProbeStatus("Key log open OK")
            end
        end

        if self.probe.logfile then
            self:WriteProbeStatus("Main log open OK")
        end
    end,

    InitProbe = function(self)
        if not self.probe.enabled then return end
        self:EnsureProbeFiles(self:IsProbeFileLoggingEnabled())
    end,

    LogPayloadKey = function(self, key, valuetype)
        if not self.probe.keylogfile then return end
        if self.probe.loggedKeys[key] then return end
        self.probe.loggedKeys[key] = true
        self.probe.keylogfile:write(string.format("%.3f;AH-64D;%s;%s\n", socket.gettime(), key, valuetype))
        self.probe.keylogfile:flush()
    end,

    RegisterPayloadSchema = function(self, payload)
        if type(payload) ~= "table" then return end

        for k, v in pairs(payload) do
            local key = safe_tostring(k)
            local vt = type(v)
            self:LogPayloadKey("payload." .. key, vt)

            if key == "Stations" and vt == "table" then
                for _, station in ipairs(v) do
                    if type(station) == "table" then
                        for sk, sv in pairs(station) do
                            self:LogPayloadKey("payload.Stations." .. safe_tostring(sk), type(sv))
                        end
                    end
                end
            elseif vt == "table" then
                for sk, sv in pairs(v) do
                    if type(sk) == "string" or type(sk) == "number" then
                        local nested = "payload." .. key .. "." .. safe_tostring(sk)
                        self:LogPayloadKey(nested, type(sv))
                    end
                end
            end
        end
    end,

    BuildProbeSnapshot = function(self)
        local moduleName = ""
        local unitName = ""
        local payloadText = ""
        local payloadTable = nil

        if type(LoGetSelfData) == "function" then
            local ok, selfData = pcall(LoGetSelfData)
            if ok and type(selfData) == "table" then
                moduleName = safe_tostring(selfData.Name)
                unitName = safe_tostring(selfData.UnitName)
            end
        end

        if type(LoGetPayloadInfo) == "function" then
            local ok, payload = pcall(LoGetPayloadInfo)
            if ok then
                payloadTable = payload
                self:RegisterPayloadSchema(payload)
                payloadText = serialize_payload(payload)
            end
        end

        return moduleName, unitName, payloadText, payloadTable
    end,

    PollProbe = function(self)
        if not self.probe.enabled then return end

        local enableFileLogging = self:IsProbeFileLoggingEnabled()
        self:EnsureProbeFiles(enableFileLogging)

        local now = socket.gettime()
        if (now - self.probe.lastPoll) < self.probe.intervalSeconds then
            return
        end
        self.probe.lastPoll = now

        local moduleName, unitName, payloadText, payloadTable = self:BuildProbeSnapshot()

        if not string.find(moduleName or "", "AH-64D", 1, true) then
            return
        end

        self:SendWeaponStateUpdate(payloadTable)

        local state = moduleName .. ";" .. unitName .. ";" .. payloadText
        if enableFileLogging and self.probe.logfile and state ~= self.probe.lastState then
            self.probe.lastState = state
            self.probe.logfile:write(string.format("%.3f;%s\n", now, state))
            self.probe.logfile:flush()
        end
    end,

    CloseProbe = function(self)
        if self.probe and self.probe.logfile then
            pcall(function() self.probe.logfile:close() end)
            self.probe.logfile = nil
        end
        if self.probe and self.probe.keylogfile then
            pcall(function() self.probe.keylogfile:close() end)
            self.probe.keylogfile = nil
        end
        if self.probe and self.probe.statusfile then
            pcall(function() self.probe.statusfile:close() end)
            self.probe.statusfile = nil
        end
    end,

    Start = function(self)
        log("Initializing sockets")
        vaicom.sendtoradio = create_udp_socket(
            vaicom.config.sendtoradio.address,
            vaicom.config.sendtoradio.port,
            vaicom.config.sendtoradio.timeout,
            false
        )

        vaicom.receivefromclient = create_udp_socket(
            vaicom.config.receivefromclient.address,
            vaicom.config.receivefromclient.port,
            vaicom.config.receivefromclient.timeout,
            true
        )

        vaicom.sendtoclient = create_udp_socket(
            vaicom.config.sendtoclient.address,
            vaicom.config.sendtoclient.port,
            vaicom.config.sendtoclient.timeout,
            false
        )

        self:InitProbe()
    end,

    BeforeNextFrame = function(self)
        self:PollProbe()

        if vaicom.receivefromclient then
            local newdata, err = vaicom.receivefromclient:receive()
            if newdata then
                self:UpdateProbeDebugFromClientMessage(newdata)
                log("Received data from client: " .. newdata)

                -- Check if the message is a WSO command
                if string.find(newdata, "wMsgWSO_") then
                    log("Forwarding WSO command to DCS: " .. newdata)
                end

                -- Check if the message is an hb_send_proxy command
                if string.find(newdata, "hb_send_proxy") then
                    log("Forwarding hb_send_proxy command to DCS: " .. newdata)
                end

                -- Forward the command to the backend
                local ok, send_err = vaicom.sendtoradio:send(newdata)
                if not ok then
                    log("Failed to send to radio: " .. tostring(send_err))
                end
            elseif err ~= "timeout" then
                log("Error receiving data from client: " .. tostring(err))
            end
        end
    end,

    AfterNextFrame = function(self)
        -- Reserved for future use
    end,

    Stop = function(self)
        log("Stopping sockets")

        self:CloseProbe()

        -- Send beacon close message to the client
        if vaicom.sendtoclient then
            local ok, err = pcall(function()
                vaicom.sendtoclient:send(vaicom.config.beaconclose)
            end)
            if not ok then
                log("Error sending beacon close message: " .. tostring(err))
            end
        end

        -- Close all sockets
        for _, sock in ipairs({"sendtoradio", "receivefromclient", "sendtoclient"}) do
            if vaicom[sock] then
                local ok, err = pcall(function() socket.try(vaicom[sock]:close()) end)
                if not ok then
                    log("Error closing socket " .. sock .. ": " .. tostring(err))
                end
                vaicom[sock] = nil
            end
        end
    end
}

-- Safe export hook integration
local function append_export_hook(hook_name, fn)
    local existing = _G[hook_name]
    _G[hook_name] = function(...)
        fn(vaicom.insert)
        if existing then existing(...) end
    end
end

append_export_hook("LuaExportStart", function(self) self:Start() end)
append_export_hook("LuaExportBeforeNextFrame", function(self) self:BeforeNextFrame() end)
append_export_hook("LuaExportAfterNextFrame", function(self) self:AfterNextFrame() end)
append_export_hook("LuaExportStop", function(self) self:Stop() end)