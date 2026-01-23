-- VAICOM PRO server-side script (Optimized Version)
-- Original: VAICOMPRO.export.lua
-- Vaicom Discord at https://discord.gg/7c22BHNSCS 

package.path  = package.path..";.\\LuaSocket\\?.lua;" -- use windows path format for LuaSocket due to some picky systems
package.cpath = package.cpath..";.\\LuaSocket\\?.dll;"

local socket = require("socket")

-- Debugging
local DEBUG = false
local function log(msg)
    if DEBUG then
        base.print("[VAICOM] " .. msg)
    end
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
    beaconclose = "missiondata.update.beacon.unlock"
}

vaicom.insert = {

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
    end,

    BeforeNextFrame = function(self)
        if vaicom.receivefromclient then
            local newdata, err = vaicom.receivefromclient:receive()
            if newdata then
                log("Received data from client: " .. newdata)

                -- Check if the message is a WSO command
                if string.find(newdata, "wMsgWSO_") then
                    log("Forwarding WSO command to DCS: " .. newdata)
                end

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