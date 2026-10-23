--Detection and Map Marker script for Afghanistan 2025 --Script by Pene
-- -------------
-- CAS detection markers for F10 map (load after Moose.lua or as standalone)
-- Own coalition ground units detect enemy ground -> coalition-only Map marker marker

local OWN_COALITION   = coalition.side.BLUE   -- change to RED if needed
local ENEMY_COALITION = coalition.side.RED
local SCAN_INTERVAL   = 10                    -- seconds
local MARK_TTL_SEC    = 180                   -- remove marker if not redetected
local MOVE_UPDATE_M   = 150                   -- update if moved this far
local MAX_CHECK_RANGE = 12000                 -- fallback visual range check (m)
local NM_TO_M         = 1852
local SMOKE_TRIGGER_RANGE_M = 10 * NM_TO_M    -- opposing client proximity to target marker
local FRIENDLY_SMOKE_MAX_RANGE_M = 5000       -- nearest friendly ground unit must be within this range
local SMOKE_DURATION_SEC    = 60              -- smoke active period requested
local SMOKE_REDEPLOY_DELAY_SEC = 30           -- delay before re-smoke when still in zone

local MARK_ID_TIME_MOD = 100000
local nextMarkBaseId = 700000
local marksByGroup = {} -- [groupName] = { id=, baseId=, x=, z=, lastSeen=, nextSmokeAt= }
local LOG_TAG = "[CAS/ISR DETECTION] "

local function allocMarkBaseId()
  local id = nextMarkBaseId
  nextMarkBaseId = nextMarkBaseId + MARK_ID_TIME_MOD
  return id
end

local function buildMarkId(baseId, now)
  local t = math.floor((now or 0) * 10) % MARK_ID_TIME_MOD -- tenths of a second, rolling window
  return baseId + t
end

local function dist2d(a, b)
  local dx = (a.x or 0) - (b.x or 0)
  local dz = (a.z or 0) - (b.z or 0)
  return math.sqrt(dx * dx + dz * dz)
end

local function pad5(n)
  n = math.floor(tonumber(n) or 0)
  if n < 0 then n = 0 end
  return string.format("%05d", n % 100000)
end

local function toDDM(value, isLat)
  local hemi
  if isLat then
    hemi = (value >= 0) and "N" or "S"
  else
    hemi = (value >= 0) and "E" or "W"
  end

  local absV = math.abs(value)
  local deg = math.floor(absV)
  local min = (absV - deg) * 60.0

  if isLat then
    return string.format("%s%02d°%06.3f'", hemi, deg, min)
  else
    return string.format("%s%03d°%06.3f'", hemi, deg, min)
  end
end

local function formatCoords(point)
  local lat, lon = coord.LOtoLL(point)
  local ddm = toDDM(lat, true) .. " " .. toDDM(lon, false)

  local m = coord.LLtoMGRS(lat, lon)
  local mgrs = "N/A"
  if m and m.UTMZone and m.MGRSDigraph and m.Easting and m.Northing then
    mgrs = string.format("%s%s %s %s",
      tostring(m.UTMZone),
      tostring(m.MGRSDigraph),
      pad5(m.Easting),
      pad5(m.Northing))
  end

  return ddm, mgrs
end

local function getAliveUnitsFromGroup(g)
  local out = {}
  if not g then return out end

  if g.isExist and not g:isExist() then return out end
  if g.isActive then
    local ok, active = pcall(g.isActive, g)
    if ok and not active then return out end
  end

  local units = g:getUnits() or {}
  for _, u in ipairs(units) do
    if u and u:isExist() and u:getLife() and u:getLife() > 0 then
      local include = true
      if u.isActive then
        local ok, active = pcall(u.isActive, u)
        if ok and not active then
          include = false
        end
      end
      if include then
        out[#out + 1] = u
      end
    end
  end
  return out
end

local function getCoalitionUnitsByCategory(side, category)
  local out = {}
  local groups = coalition.getGroups(side, category) or {}
  for _, g in ipairs(groups) do
    local units = getAliveUnitsFromGroup(g)
    for _, u in ipairs(units) do
      out[#out + 1] = u
    end
  end
  return out
end

local function getCoalitionGroundUnits(side)
  return getCoalitionUnitsByCategory(side, Group.Category.GROUND)
end

local function getCoalitionObserverUnits(side)
  local out = {}

  local function appendUnits(units)
    for _, u in ipairs(units) do
      out[#out + 1] = u
    end
  end

  appendUnits(getCoalitionUnitsByCategory(side, Group.Category.GROUND))
  appendUnits(getCoalitionUnitsByCategory(side, Group.Category.AIRPLANE))
  appendUnits(getCoalitionUnitsByCategory(side, Group.Category.HELICOPTER))

  return out
end

local function getDetectionMode(observer)
  local g = observer and observer.getGroup and observer:getGroup() or nil
  if g and g.getCategory then
    local cat = g:getCategory()
    if cat == Group.Category.GROUND then
      return "CAS REQUEST", true
    end
  end
  return "ISR DETECTION", false
end

local function getOppositeCoalition(side)
  if side == coalition.side.BLUE then return coalition.side.RED end
  if side == coalition.side.RED then return coalition.side.BLUE end
  return nil
end

local function getCoalitionPlayerUnits(side)
  local out = {}
  local players = coalition.getPlayers(side) or {}
  for _, u in ipairs(players) do
    if u and u.isExist and u:isExist() and u.getLife and u:getLife() and u:getLife() > 0 then
      if u.isActive then
        local ok, active = pcall(u.isActive, u)
        if ok and not active then
          -- skip inactive
        else
          out[#out + 1] = u
        end
      else
        out[#out + 1] = u
      end
    end
  end
  return out
end

local function anyUnitWithinRange(units, point, rangeM)
  for _, u in ipairs(units) do
    local up = u:getPoint()
    if up and dist2d(up, point) <= rangeM then
      return true
    end
  end
  return false
end

local function nearestGroundUnit(side, point)
  local nearest, nearestD = nil, nil
  local groups = coalition.getGroups(side, Group.Category.GROUND) or {}
  for _, g in ipairs(groups) do
    local units = getAliveUnitsFromGroup(g)
    for _, u in ipairs(units) do
      local p = u:getPoint()
      if p then
        local d = dist2d(p, point)
        if not nearestD or d < nearestD then
          nearest, nearestD = u, d
        end
      end
    end
  end
  return nearest, nearestD
end

local function averagePoint(units)
  local sx, sy, sz, c = 0, 0, 0, 0
  for _, u in ipairs(units) do
    local p = u:getPoint()
    if p then
      sx = sx + (p.x or 0)
      sy = sy + (p.y or 0)
      sz = sz + (p.z or 0)
      c = c + 1
    end
  end
  if c == 0 then return nil end
  return { x = sx / c, y = sy / c, z = sz / c }
end

local function isDetectedBy(observer, target)
  if not observer or not target then return false end

  -- Primary: DCS controller detection API
  local ctrl = observer:getController()
  if ctrl and Controller and Controller.Detection and ctrl.isTargetDetected then
    local mask = (Controller.Detection.VISUAL or 0)
      + (Controller.Detection.OPTIC or 0)
      + (Controller.Detection.RADAR or 0)
      + (Controller.Detection.IRST or 0)

    local ok, detected = pcall(ctrl.isTargetDetected, ctrl, target, mask)
    if ok and detected then
      return true
    end
  end

  -- Fallback: LOS + range
  local op = observer:getPoint()
  local tp = target:getPoint()
  if not op or not tp then return false end

  local d = dist2d(op, tp)
  if d > MAX_CHECK_RANGE then return false end

  if land and land.isVisible then
    local ok, vis = pcall(land.isVisible, op, tp)
    if ok and vis then return true end
  end

  return d <= 4000
end

local function makeMarkerText(markerTitle, enemyType, count, ddm, mgrs, detectorName)
  return table.concat({
    tostring(markerTitle or "ISR DETECTION"),
    "Type: " .. tostring(enemyType or "Unknown"),
    "Count: " .. tostring(count or 0),
    "DDM: " .. tostring(ddm or "N/A"),
    "MGRS: " .. tostring(mgrs or "N/A"),
    "Detected by: " .. tostring(detectorName or "Unknown")
  }, "\n")
end

local function upsertCoalitionMark(groupName, point, text, now, statusLine, forceRefresh)
  local rec = marksByGroup[groupName]
  if not rec then
    local baseId = allocMarkBaseId()
    local id = buildMarkId(baseId, now)
    local textWithId = text
    if statusLine and statusLine ~= "" then
      textWithId = textWithId .. "\n" .. statusLine
    end
    textWithId = textWithId .. "\nMark ID: " .. tostring(id)
    trigger.action.markToCoalition(id, textWithId, point, OWN_COALITION, true)
    marksByGroup[groupName] = { id = id, baseId = baseId, x = point.x, z = point.z, lastSeen = now, nextSmokeAt = 0 }
    return marksByGroup[groupName]
  end

  local moved = dist2d({ x = rec.x, z = rec.z }, point) >= MOVE_UPDATE_M
  if moved or forceRefresh then
    trigger.action.removeMark(rec.id)
    local id = buildMarkId(rec.baseId, now)
    local textWithId = text
    if statusLine and statusLine ~= "" then
      textWithId = textWithId .. "\n" .. statusLine
    end
    textWithId = textWithId .. "\nMark ID: " .. tostring(id)
    trigger.action.markToCoalition(id, textWithId, point, OWN_COALITION, true)
    rec.id = id
    if moved then
      rec.x, rec.z = point.x, point.z
    end
  end
  rec.lastSeen = now
  return rec
end

local function cleanupOldMarks(now)
  for gName, rec in pairs(marksByGroup) do
    if (now - (rec.lastSeen or 0)) > MARK_TTL_SEC then
      trigger.action.removeMark(rec.id)
      marksByGroup[gName] = nil
    end
  end
end

local function scanDetections()
  local now = timer.getTime()
  local observers = getCoalitionObserverUnits(OWN_COALITION)
  local enemyGroups = coalition.getGroups(ENEMY_COALITION, Group.Category.GROUND) or {}
  local oppositeCoalition = getOppositeCoalition(ENEMY_COALITION)
  local oppositePlayers = oppositeCoalition and getCoalitionPlayerUnits(oppositeCoalition) or {}

  for _, eg in ipairs(enemyGroups) do
    local eUnits = getAliveUnitsFromGroup(eg)
    if #eUnits > 0 then
      local detectedBy = nil

      for _, obs in ipairs(observers) do
        for _, tgt in ipairs(eUnits) do
          if isDetectedBy(obs, tgt) then
            detectedBy = obs
            break
          end
        end
        if detectedBy then break end
      end

      if detectedBy then
        local center = averagePoint(eUnits)
        if center then
          local ddm, mgrs = formatCoords(center)
          local eType = eUnits[1] and eUnits[1]:getTypeName() or "Unknown"
          local detectorName = detectedBy:getName() or "Unknown"
          local markerTitle, allowSmoke = getDetectionMode(detectedBy)
          local text = makeMarkerText(markerTitle, eType, #eUnits, ddm, mgrs, detectorName)
          local gName = eg:getName() or ("EnemyGroup_" .. tostring(eg:getID() or math.random(100000)))
          local rec = upsertCoalitionMark(gName, center, text, now)

          if allowSmoke and rec and oppositeCoalition and #oppositePlayers > 0 and anyUnitWithinRange(oppositePlayers, center, SMOKE_TRIGGER_RANGE_M) then
            if now >= (rec.nextSmokeAt or 0) then
              local friendUnit, friendDist = nearestGroundUnit(oppositeCoalition, center)
              if friendUnit and friendDist and friendDist <= FRIENDLY_SMOKE_MAX_RANGE_M then
                local friendPoint = friendUnit:getPoint()
                if friendPoint then
                  local color, colorLabel
                  if math.random(0, 1) == 0 then
                    color = trigger.smokeColor.Orange
                    colorLabel = "Orange"
                  else
                    color = trigger.smokeColor.White
                    colorLabel = "White"
                  end

                  trigger.action.smoke(friendPoint, color)

                  rec.nextSmokeAt = now + SMOKE_DURATION_SEC + SMOKE_REDEPLOY_DELAY_SEC
                  local smokeStatus = "Friendlies Popping " .. colorLabel .. " Smoke"
                  upsertCoalitionMark(gName, center, text, now, smokeStatus, true)
                end
              end
            end
          end
        end
      end
    end
  end

  cleanupOldMarks(now)
end

local function scheduler(_, t)
  local ok, err = pcall(scanDetections)
  if not ok and env and env.error then
    env.error(LOG_TAG .. "scanDetections error: " .. tostring(err))
  end
  return t + SCAN_INTERVAL
end

local function initScript()
  timer.scheduleFunction(scheduler, nil, timer.getTime() + 5)
  if env and env.info then
    env.info(LOG_TAG .. "loaded successfully; scheduler started.")
  end
end

local ok, err = pcall(initScript)
if not ok and env and env.error then
  env.error(LOG_TAG .. "failed to load: " .. tostring(err))
end
