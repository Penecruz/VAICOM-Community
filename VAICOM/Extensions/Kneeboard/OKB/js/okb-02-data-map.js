    function persistEfbSaUserWaypointsPreference() {
      const rows = Array.isArray(efbSaUserWaypoints) ? efbSaUserWaypoints : [];
      writeStoredPreferenceValue(efbSaUserWaypointsStorageKey, JSON.stringify(rows));
    }

    function readEfbSaSavedHistoryTracksPreference() {
      try {
        const raw = readStoredPreferenceValue(efbSaSavedHistoryTracksStorageKey);
        if (!raw) return {};
        const parsed = JSON.parse(raw);
        if (!parsed || typeof parsed !== 'object') return {};

        function toFiniteNumberOrNull(value) {
          if (value === null || value === undefined) return null;
          const text = String(value).trim();
          if (!text) return null;
          const n = Number(text);
          return isFinite(n) ? n : null;
        }

        const result = {};
        Object.keys(parsed).forEach(function (selectionKeyRaw) {
          const selectionKey = String(selectionKeyRaw || '').trim();
          if (!selectionKey) return;
          const rows = parsed[selectionKeyRaw];
          if (!Array.isArray(rows)) return;
          const cleanRows = [];
          const seenTrackIds = {};
          rows.forEach(function (track) {
            if (!track || typeof track !== 'object') return;
            const id = String(track.id || '').trim();
            if (!id || seenTrackIds[id]) return;
            const pointsRaw = Array.isArray(track.points) ? track.points : [];
            const cleanPoints = pointsRaw.map(function (p) {
              if (!p || typeof p !== 'object') return null;
              const xNum = toFiniteNumberOrNull(p.xNum);
              const yNum = toFiniteNumberOrNull(p.yNum);
              const ts = toFiniteNumberOrNull(p.ts);
              if (xNum === null || yNum === null) return null;
              return {
                xNum: xNum,
                yNum: yNum,
                ts: ts !== null ? ts : 0,
              };
            }).filter(function (p) { return !!p; });
            if (!cleanPoints.length) return;
            seenTrackIds[id] = true;
            cleanRows.push({
              id: id,
              name: String(track.name || '').trim() || ('Track ' + String(cleanRows.length + 1)),
              visible: track.visible !== false,
              createdUtcMs: Number(track.createdUtcMs) || Date.now(),
              updatedUtcMs: Number(track.updatedUtcMs) || 0,
              points: cleanPoints,
            });
          });
          if (cleanRows.length) {
            result[selectionKey] = cleanRows;
          }
        });
        return result;
      } catch (_) {
        return {};
      }
    }

    function persistEfbSaSavedHistoryTracksPreference() {
      try {
        const source = (efbSaSavedHistoryTracksBySelection && typeof efbSaSavedHistoryTracksBySelection === 'object')
          ? efbSaSavedHistoryTracksBySelection
          : {};
        const payload = JSON.stringify(source);
        writeStoredPreferenceValue(efbSaSavedHistoryTracksStorageKey, payload);
        fetch('/okb/efb/historytracks', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: payload,
          cache: 'no-store'
        }).catch(function () { });
      } catch (_) {
      }
    }

    async function hydrateEfbSaSavedHistoryTracksFromServer() {
      try {
        const res = await fetch('/okb/efb/historytracks', { cache: 'no-store' });
        if (!res || !res.ok) return;
        const text = await res.text();
        if (!text) return;
        writeStoredPreferenceValue(efbSaSavedHistoryTracksStorageKey, text);
        efbSaSavedHistoryTracksBySelection = readEfbSaSavedHistoryTracksPreference();
        efbUiDirty = true;
        if (latestData) render(latestData);
      } catch (_) {
      }
    }

    function getEfbSaSavedHistoryTracksStorageKey(selected) {
      const model = latestData || {};
      const theatre = String(resolveOpenFreeMapFallbackTheatreText(model) || inferTheatreCandidateFromSelection(selected) || '').trim();
      if (theatre) {
        return 'THEATRE::' + theatre.toUpperCase();
      }
      return getFlightPlanEtaStartKey(selected);
    }

    function cloneSavedHistoryTrackRow(row) {
      return {
        id: String((row && row.id) || ''),
        name: String((row && row.name) || ''),
        visible: row && row.visible !== false,
        createdUtcMs: Number(row && row.createdUtcMs) || Date.now(),
        updatedUtcMs: Number(row && row.updatedUtcMs) || 0,
        points: (Array.isArray(row && row.points) ? row.points : []).map(function (p) {
          return {
            xNum: Number(p && p.xNum),
            yNum: Number(p && p.yNum),
            ts: Number(p && p.ts) || 0,
          };
        }).filter(function (p) {
          return isFinite(p.xNum) && isFinite(p.yNum);
        })
      };
    }

    function getEfbSaSavedHistoryTracksBySelection(selected) {
      const key = getEfbSaSavedHistoryTracksStorageKey(selected);
      if (!key) return [];

      if (!Array.isArray(efbSaSavedHistoryTracksBySelection[key])) {
        efbSaSavedHistoryTracksBySelection[key] = [];
      }

      if (efbSaSavedHistoryTracksBySelection[key].length) {
        return efbSaSavedHistoryTracksBySelection[key];
      }

      if (key.indexOf('THEATRE::') === 0) {
        const targetTheatre = key.substring('THEATRE::'.length);
        const merged = [];
        const seen = {};
        Object.keys(efbSaSavedHistoryTracksBySelection || {}).forEach(function (legacyKey) {
          if (legacyKey === key) return;
          const legacyTheatre = String(inferTheatreCandidateFromSelection(legacyKey) || '').trim().toUpperCase();
          if (!legacyTheatre || legacyTheatre !== targetTheatre) return;
          const rows = Array.isArray(efbSaSavedHistoryTracksBySelection[legacyKey]) ? efbSaSavedHistoryTracksBySelection[legacyKey] : [];
          rows.forEach(function (row) {
            const id = String((row && row.id) || '').trim();
            if (!id || seen[id]) return;
            const clean = cloneSavedHistoryTrackRow(row);
            if (!clean.id || !Array.isArray(clean.points) || !clean.points.length) return;
            seen[id] = true;
            merged.push(clean);
          });
        });
        if (merged.length) {
          efbSaSavedHistoryTracksBySelection[key] = merged;
          persistEfbSaSavedHistoryTracksPreference();
        }
      }

      return efbSaSavedHistoryTracksBySelection[key];
    }

    function findEfbSaSavedHistoryTrack(selectionKey, id) {
      const key = getFlightPlanEtaStartKey(selectionKey);
      const trackId = String(id || '').trim();
      if (!key || !trackId) return null;
      const rows = getEfbSaSavedHistoryTracksBySelection(key);
      for (let i = 0; i < rows.length; i++) {
        const row = rows[i] || {};
        if (String(row.id || '') === trackId) return row;
      }
      return null;
    }

    function getNextEfbSaSavedHistoryTrackName(selectionKey) {
      const key = getFlightPlanEtaStartKey(selectionKey);
      const rows = key ? getEfbSaSavedHistoryTracksBySelection(key) : [];
      let maxN = 0;
      rows.forEach(function (row) {
        const m = String((row && row.name) || '').trim().toUpperCase().match(/^TRACK\s+(\d+)$/);
        if (!m) return;
        const n = Number(m[1]);
        if (isFinite(n) && n > maxN) maxN = n;
      });
      return 'Track ' + String(maxN + 1);
    }

    function saveActiveEfbSaHistoryTrack(selectionKey) {
      const key = getFlightPlanEtaStartKey(selectionKey);
      if (!key) return null;
      const active = getEfbSaHistoryTrackByKey(key);
      if (!Array.isArray(active) || !active.length) return null;
      const points = active
        .map(function (p) {
          const x = Number(p && p.xNum);
          const y = Number(p && p.yNum);
          const ts = Number(p && p.ts);
          if (!isFinite(x) || !isFinite(y)) return null;
          return { xNum: x, yNum: y, ts: isFinite(ts) ? ts : Date.now() };
        })
        .filter(function (p) { return !!p; });
      if (!points.length) return null;
      const id = 'hst' + Date.now().toString(36) + Math.floor(Math.random() * 100000).toString(36);
      const nowMs = Date.now();
      const row = {
        id: id,
        name: getNextEfbSaSavedHistoryTrackName(key),
        visible: true,
        createdUtcMs: nowMs,
        updatedUtcMs: nowMs,
        points: points,
      };
      const tracks = getEfbSaSavedHistoryTracksBySelection(key);
      tracks.push(row);
      persistEfbSaSavedHistoryTracksPreference();
      return row;
    }

    function setSavedEfbSaHistoryTrackVisible(selectionKey, id, visible) {
      const row = findEfbSaSavedHistoryTrack(selectionKey, id);
      if (!row) return false;
      row.visible = visible !== false;
      row.updatedUtcMs = Date.now();
      persistEfbSaSavedHistoryTracksPreference();
      return true;
    }

    function beginEfbSaSavedHistoryTrackRename(selectionKey, id) {
      const key = getFlightPlanEtaStartKey(selectionKey);
      const row = findEfbSaSavedHistoryTrack(key, id);
      if (!key || !row) return false;
      efbSaSavedHistoryTrackEditSelectionKey = key;
      efbSaSavedHistoryTrackEditingId = String(row.id || '');
      efbSaSavedHistoryTrackEditDraft = String(row.name || '').trim();
      return true;
    }

    function cancelEfbSaSavedHistoryTrackRename() {
      efbSaSavedHistoryTrackEditSelectionKey = '';
      efbSaSavedHistoryTrackEditingId = '';
      efbSaSavedHistoryTrackEditDraft = '';
    }

    function commitEfbSaSavedHistoryTrackRename() {
      const key = getFlightPlanEtaStartKey(efbSaSavedHistoryTrackEditSelectionKey);
      const id = String(efbSaSavedHistoryTrackEditingId || '').trim();
      const name = String(efbSaSavedHistoryTrackEditDraft || '').trim();
      if (!key || !id || !name) return false;
      const row = findEfbSaSavedHistoryTrack(key, id);
      if (!row) return false;
      row.name = name;
      row.updatedUtcMs = Date.now();
      persistEfbSaSavedHistoryTracksPreference();
      cancelEfbSaSavedHistoryTrackRename();
      return true;
    }

    function deleteEfbSaSavedHistoryTrack(selectionKey, id) {
      const key = getFlightPlanEtaStartKey(selectionKey);
      const trackId = String(id || '').trim();
      if (!key || !trackId) return false;
      const rows = getEfbSaSavedHistoryTracksBySelection(key);
      const idx = rows.findIndex(function (row) { return String((row && row.id) || '') === trackId; });
      if (idx < 0) return false;
      rows.splice(idx, 1);
      persistEfbSaSavedHistoryTracksPreference();
      if (String(efbSaSavedHistoryTrackEditingId || '') === trackId
        && getFlightPlanEtaStartKey(efbSaSavedHistoryTrackEditSelectionKey) === key) {
        cancelEfbSaSavedHistoryTrackRename();
      }
      return true;
    }

    function clearActiveEfbSaHistoryTrack(selectionKey) {
      const key = getFlightPlanEtaStartKey(selectionKey);
      if (!key) return false;
      if (!Array.isArray(efbSaHistoryTrackBySelection[key])) {
        efbSaHistoryTrackBySelection[key] = [];
      }
      efbSaHistoryTrackBySelection[key] = [];
      delete efbSaHistoryTrackLastSampleBySelection[key];
      return true;
    }

    function buildEfbSaUserWaypointAssetKey(id) {
      const key = String(id || '').trim();
      if (!key) return '';
      return 'USER-WP|' + key;
    }

    function parseEfbSaUserWaypointAssetKey(assetKey) {
      const text = String(assetKey || '').trim();
      if (!text) return '';
      const prefix = 'USER-WP|';
      if (text.indexOf(prefix) !== 0) return '';
      return String(text.substring(prefix.length) || '').trim();
    }

    function findEfbSaUserWaypointById(id) {
      const key = String(id || '').trim();
      if (!key) return null;
      const rows = Array.isArray(efbSaUserWaypoints) ? efbSaUserWaypoints : [];
      for (let i = 0; i < rows.length; i++) {
        const row = rows[i] || {};
        if (String(row.id || '') === key) return row;
      }
      return null;
    }

    function getNextEfbSaUserWaypointName() {
      const rows = Array.isArray(efbSaUserWaypoints) ? efbSaUserWaypoints : [];
      const used = {};
      rows.forEach(function (row) {
        const name = String((row && row.name) || '').toUpperCase().trim();
        const m = name.match(/^U(\d+)$/);
        if (!m) return;
        const n = Number(m[1]);
        if (isFinite(n) && n > 0) used[n] = true;
      });
      let idx = 1;
      while (used[idx]) idx++;
      return 'U' + String(idx);
    }

    function createEfbSaUserWaypoint(xNum, yNum, lat, lon) {
      const north = (xNum === null || xNum === undefined || String(xNum).trim() === '') ? NaN : Number(xNum);
      const east = (yNum === null || yNum === undefined || String(yNum).trim() === '') ? NaN : Number(yNum);
      const latNum = (lat === null || lat === undefined || String(lat).trim() === '') ? NaN : Number(lat);
      const lonNum = (lon === null || lon === undefined || String(lon).trim() === '') ? NaN : Number(lon);
      const hasXY = isFinite(north) && isFinite(east);
      const hasLatLon = isFinite(latNum) && isFinite(lonNum) && Math.abs(latNum) <= 90 && Math.abs(lonNum) <= 180;
      if (!hasXY && !hasLatLon) return null;
      const id = 'u' + Date.now().toString(36) + Math.floor(Math.random() * 100000).toString(36);
      const wp = {
        id: id,
        name: getNextEfbSaUserWaypointName(),
        xNum: hasXY ? north : null,
        yNum: hasXY ? east : null,
        lat: hasLatLon ? latNum : null,
        lon: hasLatLon ? lonNum : null,
        createdUtcMs: Date.now(),
        updatedUtcMs: Date.now(),
      };
      if (!Array.isArray(efbSaUserWaypoints)) efbSaUserWaypoints = [];
      efbSaUserWaypoints.push(wp);
      persistEfbSaUserWaypointsPreference();
      return wp;
    }

    function renameEfbSaUserWaypoint(id, name) {
      const wp = findEfbSaUserWaypointById(id);
      if (!wp) return false;
      const next = String(name || '').trim();
      if (!next) return false;
      if (wp.name === next) return true;
      wp.name = next;
      wp.updatedUtcMs = Date.now();
      persistEfbSaUserWaypointsPreference();
      return true;
    }

    function beginEfbSaUserWaypointRename(id) {
      const key = String(id || '').trim();
      const wp = findEfbSaUserWaypointById(key);
      if (!wp) return false;
      efbSaUserWaypointEditingId = key;
      efbSaUserWaypointEditDraft = String((wp && wp.name) || '').trim() || getNextEfbSaUserWaypointName();
      return true;
    }

    function cancelEfbSaUserWaypointRename() {
      efbSaUserWaypointEditingId = '';
      efbSaUserWaypointEditDraft = '';
    }

    function commitEfbSaUserWaypointRename() {
      const id = String(efbSaUserWaypointEditingId || '').trim();
      if (!id) return false;
      const next = String(efbSaUserWaypointEditDraft || '').trim();
      if (!next) return false;
      const ok = renameEfbSaUserWaypoint(id, next);
      if (ok) {
        efbSaUserWaypointEditingId = '';
        efbSaUserWaypointEditDraft = '';
      }
      return ok;
    }

    function setEfbSaUserWaypointDiag(partial) {
      const p = partial && typeof partial === 'object' ? partial : {};
      efbSaUserWaypointDiag = Object.assign({}, efbSaUserWaypointDiag || {}, p);
    }

    function deleteEfbSaUserWaypoint(id) {
      const key = String(id || '').trim();
      if (!key) return false;
      if (!Array.isArray(efbSaUserWaypoints) || !efbSaUserWaypoints.length) return false;
      const idx = efbSaUserWaypoints.findIndex(function (row) { return String((row && row.id) || '') === key; });
      if (idx < 0) return false;
      efbSaUserWaypoints.splice(idx, 1);
      persistEfbSaUserWaypointsPreference();
      return true;
    }

    function formatEfbSaUserWaypointLatLon(theatre, wp) {
      const ll = resolveEfbSaUserWaypointLonLat(theatre, wp);
      if (!ll) return '-';
      return formatLatLonDms(ll.lat, ll.lon);
    }

    function resolveEfbSaUserWaypointLonLat(theatre, wp) {
      const rawLat = wp && wp.lat;
      const rawLon = wp && wp.lon;
      const latDirect = (rawLat === null || rawLat === undefined || String(rawLat).trim() === '') ? NaN : Number(rawLat);
      const lonDirect = (rawLon === null || rawLon === undefined || String(rawLon).trim() === '') ? NaN : Number(rawLon);
      if (isFinite(latDirect) && isFinite(lonDirect) && Math.abs(latDirect) <= 90 && Math.abs(lonDirect) <= 180) {
        return { lat: latDirect, lon: lonDirect };
      }

      const rawNorth = wp && wp.xNum;
      const rawEast = wp && wp.yNum;
      const north = (rawNorth === null || rawNorth === undefined || String(rawNorth).trim() === '') ? NaN : Number(rawNorth);
      const east = (rawEast === null || rawEast === undefined || String(rawEast).trim() === '') ? NaN : Number(rawEast);
      if (!isFinite(north) || !isFinite(east)) return null;
      const llPrimary = convertDcsXYToLatLon(theatre, north, east);
      const llSwap = convertDcsXYToLatLon(theatre, east, north);
      const ll = (llPrimary && isFinite(Number(llPrimary.lat)) && isFinite(Number(llPrimary.lon)))
        ? llPrimary
        : ((llSwap && isFinite(Number(llSwap.lat)) && isFinite(Number(llSwap.lon))) ? llSwap : null);
      return ll || null;
    }

    function formatEfbSaUserWaypointMgrs(theatre, wp) {
      const ll = resolveEfbSaUserWaypointLonLat(theatre, wp);
      if (!ll) return 'MGRS N/A';
      const mgrs = String(formatLatLonMgrs(ll.lat, ll.lon) || '').toUpperCase().trim();
      return mgrs || 'MGRS N/A';
    }

    function safe(v) { return (v === null || v === undefined || v === '') ? '-' : String(v); }

    function formatUtcToSeconds(v) {
      if (v === null || v === undefined || v === '') return '-';
      const d = new Date(v);
      if (!isFinite(d.getTime())) return String(v);
      return d.toISOString().replace(/\.\d{3}Z$/, 'Z');
    }

    function resolveClosestAirfieldIcao(data, allowedAirports) {
      function tokenIcao(value) {
        const m4 = String(value || '').toUpperCase().match(/\b([A-Z0-9]{4})\b/);
        if (m4) return String(m4[1] || '');
        const m3 = String(value || '').toUpperCase().match(/\b([A-Z]{3})\b/);
        return m3 ? String(m3[1] || '') : '';
      }

      const airfields = buildMapAirfields(data);
      if (!Array.isArray(airfields) || !airfields.length) return '';

      const own = getPlayerMapPoint(data);
      if (!own || !isFinite(Number(own.xNum)) || !isFinite(Number(own.yNum))) return '';

      const allow = Array.isArray(allowedAirports) ? allowedAirports : [];
      const allowSet = {};
      if (allow.length) {
        allow.forEach(function (code) {
          const key = String(code || '').toUpperCase().trim();
          if (key) allowSet[key] = true;
        });
      }

      let nearest = null;
      let nearestDist = Number.POSITIVE_INFINITY;
      airfields.forEach(function (a) {
        if (!a) return;
        const ax = Number(a.xNum);
        const ay = Number(a.yNum);
        if (!isFinite(ax) || !isFinite(ay)) return;

        const icao = normalizeEfbAirportCode(String(tokenIcao(a.icao) || tokenIcao(a.label) || tokenIcao(a.name) || tokenIcao(a.callsign) || '').toUpperCase().trim());
        if (!icao) return;
        if (allow.length && !allowSet[icao]) return;

        const dx = ax - Number(own.xNum);
        const dy = ay - Number(own.yNum);
        const d2 = (dx * dx) + (dy * dy);
        if (d2 < nearestDist) {
          nearestDist = d2;
          nearest = icao;
        }
      });

      return String(nearest || '').toUpperCase().trim();
    }

    function resolveAtcContextIcao(data, allowedAirports) {
      const server = (data && data.Server) || {};
      const atcMetars = (server && server.AtcMetars && typeof server.AtcMetars === 'object') ? server.AtcMetars : {};
      const available = Array.isArray(allowedAirports) ? allowedAirports : [];

      function acceptIcao(icaoCandidate) {
        const raw = String(icaoCandidate || '').toUpperCase();
        const m4 = raw.match(/\b([A-Z0-9]{4})\b/);
        const m3 = m4 ? null : raw.match(/\b([A-Z]{3})\b/);
        const icao = normalizeEfbAirportCode(m4 ? String(m4[1] || '') : (m3 ? String(m3[1] || '') : ''));
        if (!icao) return '';
        if (available.length && available.indexOf(icao) < 0) return '';
        return icao;
      }

      const selectedMetarKey = String(selectedAtcMetarKey || '').toUpperCase().trim();
      if (selectedMetarKey) {
        const metarText = String(atcMetars[selectedMetarKey] || '');
        const selectedIcao = acceptIcao(selectedMetarKey)
          || acceptIcao(metarText);
        if (selectedIcao) return selectedIcao;
      }

      const atcLines = getMergedList(data && data.Units, 'ATC');
      for (let i = 0; i < atcLines.length; i++) {
        const line = String(atcLines[i] || '').trim();
        if (!line) continue;
        if (/^METAR\b/i.test(line) || /^WEATHER\s*:/i.test(line)) continue;
        const key = resolveAtcMetarKey(line, atcMetars);
        if (!key) continue;
        const metarText = String(atcMetars[key] || '');
        const lineIcao = acceptIcao(key)
          || acceptIcao(metarText)
          || acceptIcao(line);
        if (lineIcao) return lineIcao;
      }

      return '';
    }

    function resolveMapSelectedAirfieldIcao(data, assetKey) {
      function tokenIcao(value) {
        const m4 = String(value || '').toUpperCase().match(/\b([A-Z0-9]{4})\b/);
        if (m4) return String(m4[1] || '');
        const m3 = String(value || '').toUpperCase().match(/\b([A-Z]{3})\b/);
        return m3 ? String(m3[1] || '') : '';
      }

      const key = String(assetKey || '').trim();
      if (!key) return '';

      const parts = key.split('|').map(function (v) { return String(v || '').toUpperCase().trim(); });
      const keyCallsign = String(parts[0] || '');
      const keyName = String(parts[1] || '');

      const airfields = (typeof buildMapAirfields === 'function') ? buildMapAirfields(data) : [];
      if (Array.isArray(airfields) && airfields.length) {
        for (let i = 0; i < airfields.length; i++) {
          const p = airfields[i] || {};
          const callsign = String((p && p.callsign) || '').toUpperCase().trim();
          const name = String((p && p.name) || (p && p.label) || '').toUpperCase().trim();
          const selectionAsset = {
            callsign: String((p && p.callsign) || '').trim(),
            name: String((p && p.name) || (p && p.label) || '').trim(),
            category: 'ATC',
            xNum: Number(p && p.xNum),
            yNum: Number(p && p.yNum),
          };
          const selectionKey = (typeof makeMapAssetSelectionKey === 'function') ? String(makeMapAssetSelectionKey(selectionAsset) || '') : '';
          if (selectionKey === key || (keyCallsign && keyCallsign === callsign) || (keyName && keyName === name)) {
            const resolved = tokenIcao(p && p.icao)
              || tokenIcao(p && p.label)
              || tokenIcao(p && p.name)
              || tokenIcao(p && p.callsign)
              || '';
            if (resolved) return normalizeEfbAirportCode(resolved);
          }
        }
      }

      return normalizeEfbAirportCode(tokenIcao(key));
    }

    function setEfbDebugState(next) {
      return;
    }

    function normalizeEfbAirportCode(value) {
      const raw = String(value || '').toUpperCase().replace(/[^A-Z0-9]/g, '').trim();
      if (!raw) return '';
      if (raw.length === 4) return raw;
      if (raw.length === 3 && efbIcaoByIata && efbIcaoByIata[raw]) {
        return String(efbIcaoByIata[raw] || '').toUpperCase().trim();
      }
      return raw;
    }

    function resolveEfbSelectedAirport(data) {
      try {
        const available = Array.isArray(efbAvailableAirports) ? efbAvailableAirports : [];

        const atcIcao = normalizeEfbAirportCode(resolveAtcContextIcao(data, available));
        if (atcIcao) return atcIcao;

        const nearestIcao = normalizeEfbAirportCode(resolveClosestAirfieldIcao(data, available));
        if (nearestIcao) return nearestIcao;

        const selected = getActiveFlightPlanSelection(data);
        if (selected) {
          const routeRows = getPlanWaypointsForRecommendations(selected);
          const picks = [];
          if (routeRows.length) {
            picks.push(routeRows[routeRows.length - 1]);
            picks.push(routeRows[0]);
          }

          for (let i = 0; i < picks.length; i++) {
            const wp = picks[i] || {};
            const text = String(wp.name || wp.nameRaw || wp.type || wp.typeRaw || '');
            const m = text.toUpperCase().match(/\b[A-Z0-9]{3,4}\b/);
            const code = normalizeEfbAirportCode(m && m[0]);
            if (code) {
              return code;
            }
          }
        }

        return normalizeEfbAirportCode(resolveClosestAirfieldIcao(data, null));
      } catch (_) {
        return '';
      }
    }

    function getEfbAirportKey(data) {
      const available = Array.isArray(efbAvailableAirports) ? efbAvailableAirports : [];

      function tokenIcao(value) {
        const m = String(value || '').toUpperCase().match(/\b([A-Z]{4})\b/);
        return m ? String(m[1] || '') : '';
      }

      function resolveMapIcaoFromKey(assetKey) {
        const key = String(assetKey || '').trim();
        if (!key) return '';
        if (typeof resolveMapSelectedAirfieldIcao === 'function') {
          const fromGlobal = normalizeEfbAirportCode(resolveMapSelectedAirfieldIcao(data, key));
          if (fromGlobal) return fromGlobal;
        }
        if (typeof resolveBuggedAirfieldIcaoFromAssetKey === 'function') {
          const fromLegacy = normalizeEfbAirportCode(resolveBuggedAirfieldIcaoFromAssetKey(data, key));
          if (fromLegacy) return fromLegacy;
        }
        return normalizeEfbAirportCode(tokenIcao(key));
      }

      const selected = getActiveFlightPlanSelection(data);
      const selectedMapAssetKey = selected ? String(getMapSelectedAssetKeyBySelection(selected) || '').trim() : '';
      if (selectedMapAssetKey) {
        const mapIcao = normalizeEfbAirportCode(resolveMapIcaoFromKey(selectedMapAssetKey));
        if (mapIcao && (!available.length || available.indexOf(mapIcao) >= 0)) {
          efbManuallySelectedAirport = mapIcao;
          efbLastResolvedAirport = mapIcao;
          setEfbDebugState({
            source: 'map-selected',
            mapAssetKey: selectedMapAssetKey,
            mapIcao: mapIcao,
            manual: String(efbManuallySelectedAirport || '').toUpperCase().trim(),
            sticky: String(efbLastResolvedAirport || '').toUpperCase().trim(),
            finalAirportKey: mapIcao,
          });
          return mapIcao;
        }
      }

      const manual = normalizeEfbAirportCode(efbManuallySelectedAirport);
      if (manual) {
        setEfbDebugState({
          source: 'manual',
          manual: manual,
          sticky: String(efbLastResolvedAirport || '').toUpperCase().trim(),
          atcIcao: '',
          nearestIcao: '',
          finalAirportKey: manual,
        });
        return manual;
      }

      const sticky = normalizeEfbAirportCode(efbLastResolvedAirport);
      const atcIcao = normalizeEfbAirportCode(resolveAtcContextIcao(data, available));
      const nearestIcao = normalizeEfbAirportCode(resolveClosestAirfieldIcao(data, available));

      function finalize(source, key) {
        const resolved = normalizeEfbAirportCode(key) || 'UNSET';
        setEfbDebugState({
          source: source,
          manual: '',
          sticky: sticky,
          atcIcao: atcIcao,
          nearestIcao: nearestIcao,
          availableCount: available.length,
          containsAtc: !!(atcIcao && available.indexOf(atcIcao) >= 0),
          containsNearest: !!(nearestIcao && available.indexOf(nearestIcao) >= 0),
          finalAirportKey: resolved,
        });
        return resolved;
      }

      const server = (data && data.Server) || {};
      const haveMission = !!(server.Aircraft || server.MissionTitle || server.Theater);

      if (haveMission) {
        const inferred = normalizeEfbAirportCode(resolveEfbSelectedAirport(data));
        if (inferred && (!available.length || available.indexOf(inferred) >= 0)) {
          return finalize('mission-inferred', inferred);
        }
      }

      if (sticky && (!available.length || available.indexOf(sticky) >= 0)) {
        return finalize('sticky', sticky);
      }

      if (available.length) {
        return finalize('available-first', String(available[0] || '').toUpperCase().trim() || 'UNSET');
      }

      return finalize('unset', 'UNSET');
    }

    function applyEfbAutoNearestAirport(data) {
      const available = Array.isArray(efbAvailableAirports) ? efbAvailableAirports : [];
      // Auto-nearest must prefer geometric nearest from the current airport set,
      // not ATC/manual/sticky inference.
      let inferred = normalizeEfbAirportCode(resolveClosestAirfieldIcao(data, available));
      if (!inferred) {
        inferred = normalizeEfbAirportCode(resolveEfbSelectedAirport(data));
      }
      if (!inferred) return '';
      if (available.length && available.indexOf(inferred) < 0) {
        setEfbDebugState({
          source: 'auto-nearest-rejected',
          inferredIcao: inferred,
          availableCount: available.length,
          finalAirportKey: String(getEfbAirportKey(data) || 'UNSET'),
        });
        return '';
      }
      efbManuallySelectedAirport = inferred;
      efbLastResolvedAirport = inferred;
      setEfbDebugState({
        source: 'auto-nearest-applied',
        inferredIcao: inferred,
        availableCount: available.length,
        finalAirportKey: inferred,
      });
      return inferred;
    }

    async function ensureEfbAirportsLoaded() {
      if (Array.isArray(efbAvailableAirports)) return;
      if (efbAirportsLoadPromise) {
        await efbAirportsLoadPromise;
        return;
      }

      efbAirportsLoadPromise = (async function () {
        try {
          const res = await fetch('/okb/efb/airports', { cache: 'no-store' });
          if (res.ok) {
            const payload = await res.json();
            const airports = Array.isArray(payload && payload.airports) ? payload.airports : [];
            const entries = Array.isArray(payload && payload.airportEntries) ? payload.airportEntries : [];
            efbAvailableAirports = airports
              .map(function (v) { return String(v || '').toUpperCase().trim(); })
              .filter(function (v) { return !!v; });
            efbAirportEntries = {};
            efbIcaoByIata = {};
            entries.forEach(function (entry) {
              const icao = String((entry && entry.icao) || '').toUpperCase().trim();
              if (!icao) return;
              const iata = String((entry && entry.iata) || '').toUpperCase().trim();
              efbAirportEntries[icao] = {
                icao: icao,
                iata: iata,
                name: String((entry && entry.name) || '').trim(),
              };
              if (iata && /^[A-Z]{3}$/.test(iata)) {
                efbIcaoByIata[iata] = icao;
              }
            });
          } else {
            efbAvailableAirports = [];
            efbAirportEntries = {};
            efbIcaoByIata = {};
          }
        } catch (_) {
          efbAvailableAirports = [];
          efbAirportEntries = {};
          efbIcaoByIata = {};
        }
      })();

      try {
        await efbAirportsLoadPromise;
      } finally {
        efbAirportsLoadPromise = null;
      }
    }

    function getEfbAirportDisplayText(icao) {
      const code = String(icao || '').toUpperCase().trim();
      if (!code) return '';
      const entry = efbAirportEntries && efbAirportEntries[code] ? efbAirportEntries[code] : null;
      const name = String((entry && entry.name) || '').trim();
      return name ? (code + ', ' + name) : code;
    }

    function getEfbViewport(key) {
      const k = String(key || 'UNSET');
      if (!efbViewportByAirport[k]) {
        efbViewportByAirport[k] = { scale: 1, tx: 0, ty: 0 };
      }
      return efbViewportByAirport[k];
    }

    function openEfbDrawer(mode) {
      const wasOpen = !!efbDrawerOpen;
      efbDrawerMode = mode === 'chart'
        ? 'chart'
        : (mode === 'pinned'
          ? 'pinned'
          : (mode === 'sa-layers'
            ? 'sa-layers'
            : (mode === 'user-waypoints'
              ? 'user-waypoints'
              : (mode === 'history-tracks' ? 'history-tracks' : 'airport'))));
      efbDrawerOpen = true;
      efbDrawerAnimateOpenOnce = !wasOpen;
      efbSelectionFlowActive = true;
    }

    function closeEfbDrawer() {
      efbDrawerOpen = false;
      efbDrawerAnimateOpenOnce = false;
      efbSelectionFlowActive = false;
    }

    function getPinnedChartsForAirport(airportKey) {
      const k = String(airportKey || 'UNSET').toUpperCase().trim() || 'UNSET';
      if (!Array.isArray(efbPinnedChartsByAirport[k])) {
        efbPinnedChartsByAirport[k] = [];
      }
      return efbPinnedChartsByAirport[k];
    }

    function isChartPinned(airportKey, chartId) {
      const id = String(chartId || '');
      if (!id) return false;
      return getPinnedChartsForAirport(airportKey).indexOf(id) >= 0;
    }

    function togglePinnedChart(airportKey, chartId) {
      const id = String(chartId || '');
      if (!id) return;
      const list = getPinnedChartsForAirport(airportKey);
      const idx = list.indexOf(id);
      if (idx >= 0) {
        list.splice(idx, 1);
      } else {
        list.push(id);
      }
      persistPinnedChartsPreference();
    }

    function prunePinnedChartsForAirport(airportKey) {
      const airport = String(airportKey || 'UNSET').toUpperCase().trim() || 'UNSET';
      const list = Array.isArray(efbPinnedChartsByAirport[airport]) ? efbPinnedChartsByAirport[airport] : [];
      if (!list.length) return false;
      const charts = Array.isArray(efbChartsByAirport[airport]) ? efbChartsByAirport[airport] : [];
      const available = {};
      charts.forEach(function (c) {
        const id = String((c && c.id) || '').trim();
        if (id) available[id] = true;
      });
      const pruned = list.filter(function (id) { return !!available[String(id || '').trim()]; });
      if (pruned.length === list.length) return false;
      if (pruned.length) {
        efbPinnedChartsByAirport[airport] = pruned;
      } else {
        delete efbPinnedChartsByAirport[airport];
      }
      return true;
    }

    function getAllPinnedCharts() {
      const rows = [];
      const byAirport = efbPinnedChartsByAirport || {};
      Object.keys(byAirport).forEach(function (k) {
        const airport = String(k || '').toUpperCase().trim() || 'UNSET';
        const ids = Array.isArray(byAirport[k]) ? byAirport[k] : [];
        const charts = Array.isArray(efbChartsByAirport[airport]) ? efbChartsByAirport[airport] : [];
        ids.forEach(function (idRaw) {
          const id = String(idRaw || '');
          if (!id) return;
          const chart = charts.find(function (c) { return String((c && c.id) || '') === id; }) || null;
          const name = String((chart && chart.name) || id || 'Chart');
          rows.push({ airport: airport, id: id, name: name });
        });
      });
      return rows;
    }

    function getEfbCycleChartsForAirport(airportKey) {
      const airport = String(airportKey || 'UNSET').toUpperCase().trim() || 'UNSET';
      const charts = Array.isArray(efbChartsByAirport[airport]) ? efbChartsByAirport[airport] : [];
      if (!charts.length) return [];

      const byId = {};
      charts.forEach(function (c) {
        const id = String((c && c.id) || '').trim();
        if (id) byId[id] = c;
      });

      const pinnedIds = getPinnedChartsForAirport(airport);
      const pinnedCharts = pinnedIds
        .map(function (id) { return byId[String(id || '').trim()] || null; })
        .filter(function (c) { return !!c; });

      return pinnedCharts.length ? pinnedCharts : charts;
    }

    function applyEfbViewportToImage(img, viewport) {
      if (!img || !viewport) return;
      const s = clamp(Number(viewport.scale) || 1, 0.5, 3.5);
      const tx = Number(viewport.tx) || 0;
      const ty = Number(viewport.ty) || 0;
      const rot = Number(viewport.rot) || 0;
      img.style.transformOrigin = 'center center';
      img.style.transform = 'translate(' + tx + 'px,' + ty + 'px) rotate(' + rot + 'deg) scale(' + s + ')';
    }

    function normalizeRotationDeg(v) {
      let d = Number(v) || 0;
      while (d >= 180) d -= 360;
      while (d < -180) d += 360;
      return d;
    }

    function applyEfbViewportDefaults(viewport) {
      viewport.scale = clamp(Number(viewport.scale) || 1, 0.5, 3.5);
      viewport.tx = Number(viewport.tx) || 0;
      viewport.ty = Number(viewport.ty) || 0;
      viewport.rot = normalizeRotationDeg(Number(viewport.rot) || 0);
    }

    function setEfbLoadingOverlay(message) {
      efbLoadingOverlayMessage = String(message || '').trim();
      const el = document.getElementById('efbLoadingOverlay');
      const text = document.getElementById('efbLoadingOverlayText');
      if (!el || !text) return;
      text.textContent = efbLoadingOverlayMessage;
      el.classList.toggle('visible', !!efbLoadingOverlayMessage);
    }

    async function ensureEfbChartsLoaded(data) {
      const key = getEfbAirportKey(data);
      const existingCharts = Array.isArray(efbChartsByAirport[key]) ? efbChartsByAirport[key] : null;
      const lastFetchMs = Number(efbChartsLastFetchMsByAirport[key] || 0);
      const nowMs = Date.now();
      const emptyRetryMs = 3000;
      if (existingCharts && existingCharts.length > 0) return key;
      if (existingCharts && existingCharts.length === 0 && (nowMs - lastFetchMs) < emptyRetryMs) return key;
      if (efbChartsLoadPromiseByAirport[key]) {
        await efbChartsLoadPromiseByAirport[key];
        return key;
      }

      efbChartsLoadPromiseByAirport[key] = (async function () {
        try {
          const res = await fetch('/okb/efb/charts?airport=' + encodeURIComponent(key), { cache: 'no-store' });
          efbChartsLastFetchMsByAirport[key] = Date.now();
          if (res.ok) {
            const payload = await res.json();
            efbChartsByAirport[key] = Array.isArray(payload && payload.charts) ? payload.charts : [];
            if (prunePinnedChartsForAirport(key)) {
              persistPinnedChartsPreference();
            }
            if (!efbSelectedChartByAirport[key] && efbChartsByAirport[key].length) {
              efbSelectedChartByAirport[key] = String((efbChartsByAirport[key][0] && efbChartsByAirport[key][0].id) || '');
            }
          } else {
            efbChartsByAirport[key] = [];
          }
        } catch (_) {
          efbChartsLastFetchMsByAirport[key] = Date.now();
          efbChartsByAirport[key] = [];
        }
      })();

      try {
        await efbChartsLoadPromiseByAirport[key];
      } finally {
        delete efbChartsLoadPromiseByAirport[key];
      }

      return key;
    }

    function getEfbModuleGateState(data) {
      const efb = (data && data.Efb) || {};
      const server = (data && data.Server) || {};
      const aircraftId = String(server.Aircraft || '').trim();
      const aircraftUpper = aircraftId.toUpperCase();
      const hasAircraft = aircraftId.length > 0 && aircraftUpper !== '----';
      const moduleConnected = !!server.ModuleConnected;
      const hasPosition = Math.abs(Number(server.PlayerPosX) || 0) > 0.001
        || Math.abs(Number(server.PlayerPosY) || 0) > 0.001
        || Math.abs(Number(server.PlayerAltFeet) || 0) > 0.5;
      const liveSession = moduleConnected && hasAircraft && hasPosition;
      const devBypass = !!efb.DevBypass;
      const simModeBypass = !!(fakeMissionEnabled && fakeMissionState);
      return {
        liveSession: liveSession,
        devBypass: devBypass,
        simModeBypass: simModeBypass,
        allowed: liveSession || simModeBypass,
      };
    }

    function getEfbNoModuleMessage() {
      return 'No Module Connected - You must enter the cockpit for content to be displayed.';
    }

    function normalizeEfbSearchToken(value) {
      return String(value || '')
        .toUpperCase()
        .replace(/[^A-Z0-9]/g, '')
        .slice(0, 4);
    }

    function normalizeEfbWaypointRenameToken(value) {
      return String(value || '')
        .toUpperCase()
        .replace(/[^A-Z0-9]/g, '')
        .slice(0, 10);
    }

    function getEventElementTarget(ev) {
      const raw = ev && ev.target ? ev.target : null;
      if (!raw) return null;
      if (raw.nodeType === 3 && raw.parentElement) return raw.parentElement;
      return raw;
    }

    function getEfbSearchCandidates(token, maxRows) {
      const list = Array.isArray(efbAvailableAirports) ? efbAvailableAirports : [];
      const rows = list
        .map(function (v) { return String(v || '').toUpperCase().trim(); })
        .filter(function (v) { return !!v; });
      if (!rows.length) return [];

      const seen = {};
      const deduped = rows.filter(function (v) {
        if (seen[v]) return false;
        seen[v] = true;
        return true;
      }).sort();

      const t = normalizeEfbSearchToken(token);
      if (!t) return deduped.slice(0, maxRows || 120);

      const prefix = deduped.filter(function (v) { return v.indexOf(t) === 0; });
      const contains = deduped.filter(function (v) { return v.indexOf(t) > 0; });
      return prefix.concat(contains).slice(0, maxRows || 120);
    }

    function buildEfbSearchRingButtons(chars, radius, className) {
      const rows = Array.isArray(chars) ? chars : [];
      if (!rows.length) return '';
      return rows.map(function (ch, i) {
        const angle = (360 / rows.length) * i;
        const style = 'transform:translate(-50%,-50%) rotate(' + angle.toFixed(2) + 'deg) translateY(-' + radius + 'px) rotate(' + (-angle).toFixed(2) + 'deg);';
        return '<button type="button" class="efbSearchRingBtn ' + className + '" style="' + style + '" data-efb-search-char="' + escapeHtml(ch) + '">' + escapeHtml(ch) + '</button>';
      }).join('');
    }

    function formatEfbSaMapViewportHtml(data) {
      const model = data || latestData || {};
      const context = resolveActiveSaMapContext(model);
      const fallbackContext = {
        selected: '__EFB_OWNSHIP__',
        sourceType: 'EFB',
        root: null,
        overlays: {},
        waypoints: [],
      };
      const useFallback = !context
        && !fakeMissionEnabled
        && getEfbModuleGateState(model).allowed;
      const effectiveContext = context || (useFallback ? fallbackContext : null);

      if (!effectiveContext || !effectiveContext.selected || !Array.isArray(effectiveContext.waypoints)) {
        return '<div class="efbSaMapWrap fltPlanOpenMapWrap"><div id="efbSaMapBraReadout" class="fltPlanPage3BraReadout efbSaMapBraReadout"></div></div>';
      }

      const state = buildSaMapRenderState(effectiveContext.waypoints, model, effectiveContext.selected, effectiveContext.overlays, effectiveContext.root);
      if (!state) {
        return '<div class="efbSaMapWrap fltPlanOpenMapWrap"><div id="efbSaMapBraReadout" class="fltPlanPage3BraReadout efbSaMapBraReadout"></div></div>';
      }

      let html = '<div class="efbSaMapWrap fltPlanOpenMapWrap" data-openfreemap-wrap="' + escapeHtml(state.openFreeMapId || '') + '">';
      if (state.openFreeMapId) {
        html += '<div class="fltPlanOpenMapHost" data-openfreemap-map-id="' + escapeHtml(state.openFreeMapId) + '" data-openfreemap-selection="' + escapeHtml(getFlightPlanEtaStartKey(effectiveContext.selected)) + '"></div>';
        html += '<div class="fltPlanOpenMapFallback">' + state.mapSvg + '</div>';
      } else {
        html += state.mapSvg;
      }
      html += '<div id="efbSaMapBraReadout" class="fltPlanPage3BraReadout efbSaMapBraReadout">' + escapeHtml(state.readout) + '</div>';
      html += '</div>';
      return html;
    }

    function formatEfbTabContentHtml(data) {
      const gate = getEfbModuleGateState(data);
      if (!gate.allowed) {
        return '<div class="efbEmpty">' + escapeHtml(getEfbNoModuleMessage()) + '</div>';
      }

      const airportKey = getEfbAirportKey(data);
      const charts = efbChartsByAirport[airportKey] || [];
      const cycleCharts = getEfbCycleChartsForAirport(airportKey);
      const airports = Array.isArray(efbAvailableAirports) ? efbAvailableAirports : [];
      const selected = String(efbSelectedChartByAirport[airportKey] || (charts[0] && charts[0].id) || '');
      if (!efbSelectedChartByAirport[airportKey] && selected) {
        efbSelectedChartByAirport[airportKey] = selected;
      }
      const canCycle = cycleCharts.length > 1;
      const isRenameSearchMode = efbSearchMode === 'user-waypoint-rename' || efbSearchMode === 'history-track-rename';
      const searchToken = isRenameSearchMode
        ? normalizeEfbWaypointRenameToken(efbSearchToken)
        : normalizeEfbSearchToken(efbSearchToken);
      const searchCandidates = getEfbSearchCandidates(searchToken, 120);
      const outerChars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'.split('');
      const innerChars = '0123456789'.split('');
      const wheelOuterHtml = buildEfbSearchRingButtons(outerChars, 154, 'efbSearchOuterBtn');
      const wheelInnerHtml = buildEfbSearchRingButtons(innerChars, 106, 'efbSearchInnerBtn');
      const tokenDisplay = searchToken || (isRenameSearchMode ? 'NAME' : '----');
      const searchRows = isRenameSearchMode
        ? '<div class="efbSearchEmpty">Use wheel to build name, then Apply.</div>'
        : searchCandidates.map(function (code) {
          const active = code === airportKey ? ' active' : '';
          return '<button type="button" class="efbSearchCandidateBtn' + active + '" data-efb-search-candidate="' + escapeHtml(code) + '">' + escapeHtml(getEfbAirportDisplayText(code)) + '</button>';
        }).join('');
      let context = null;
      try {
        context = resolveActiveSaMapContext(data || latestData);
      } catch (_) {
        context = null;
      }
      const saMapTheater = String(resolveOpenFreeMapFallbackTheatreText(data || latestData) || '').trim();
      const historySelectionKey = context && context.selected ? getFlightPlanEtaStartKey(context.selected) : '';
      const selectedAssetKey = context && context.selected ? getMapSelectedAssetKeyBySelection(context.selected) : '';
      const selectedUserWaypointId = parseEfbSaUserWaypointAssetKey(selectedAssetKey);

      const airportOptions = airports.map(function (a) {
        const code = String(a || '').toUpperCase().trim();
        if (!code) return '';
        const sel = code === airportKey ? ' selected' : '';
        return '<button type="button" class="efbPickerItem' + (sel ? ' active' : '') + '" data-efb-airport="' + escapeHtml(code) + '">' + escapeHtml(getEfbAirportDisplayText(code)) + '</button>';
      }).join('');

      const options = charts.map(function (c) {
        const id = String((c && c.id) || '');
        const label = String((c && c.name) || id || 'Chart');
        const active = id === selected ? ' active' : '';
        const pinnedClass = isChartPinned(airportKey, id) ? ' pinned' : '';
        const pinTitle = isChartPinned(airportKey, id) ? 'Unpin chart' : 'Pin chart';
        return '<div class="efbPickerRow' + active + '"><button type="button" class="efbPickerItemMain" data-efb-chart="' + escapeHtml(encodeURIComponent(id)) + '">' + escapeHtml(label) + '</button><button type="button" class="efbPinBtn' + pinnedClass + '" data-efb-pin-toggle="' + escapeHtml(encodeURIComponent(id)) + '" title="' + escapeHtml(pinTitle) + '">📌</button></div>';
      }).join('');

      const selectedChart = charts.find(function (c) { return String((c && c.id) || '') === selected; }) || null;
      const selectedChartId = String((selectedChart && selectedChart.id) || '');
      const isNavigraphChart = selectedChartId.toLowerCase().indexOf('nav:') === 0;
      const chartMode = (nightModeEnabled && isNavigraphChart) ? '&mode=night' : '';
      const chartUrl = selectedChart
        ? ('/okb/efb/chart?id=' + encodeURIComponent(selectedChartId) + chartMode)
        : '';
      const drawerTitle = efbDrawerMode === 'chart'
        ? ('Charts · ' + escapeHtml(airportKey || 'UNSET'))
        : (efbDrawerMode === 'sa-layers' ? 'SA Map Layers'
        : (efbDrawerMode === 'pinned' ? ('Pinned · ' + escapeHtml(airportKey || 'UNSET'))
          : (efbDrawerMode === 'user-waypoints' ? 'User Waypoints' : (efbDrawerMode === 'history-tracks' ? 'History Tracks' : 'Airports'))));
      const pinned = getPinnedChartsForAirport(airportKey);
      const pinnedRows = charts
        .filter(function (c) {
          const id = String((c && c.id) || '');
          return id && pinned.indexOf(id) >= 0;
        })
        .map(function (c) {
          const id = String((c && c.id) || '');
          const label = String((c && c.name) || id || 'Chart');
          const active = id === selected ? ' active' : '';
          return '<div class="efbPickerRow' + active + '"><button type="button" class="efbPickerItemMain" data-efb-chart="' + escapeHtml(encodeURIComponent(id)) + '">' + escapeHtml(label) + '</button><button type="button" class="efbPinBtn pinned" data-efb-pin-toggle="' + escapeHtml(encodeURIComponent(id)) + '" title="Unpin chart">📌</button></div>';
        }).join('');
      const saLayerDrawerOpen = efbDrawerOpen && efbDrawerMode === 'sa-layers';
      const navAuth = !!(((data && data.Efb) || {}).AuthPresent);
      const navLayer = normalizeEfbSaNavigraphLayer(efbSaNavigraphLayer);
      const navLayerLabel = getEfbSaNavigraphLayerLabel(navLayer);
      const userWaypointRows = (Array.isArray(efbSaUserWaypoints) ? efbSaUserWaypoints : [])
        .slice()
        .sort(function (a, b) {
          const an = String((a && a.name) || '').toUpperCase();
          const bn = String((b && b.name) || '').toUpperCase();
          if (an < bn) return -1;
          if (an > bn) return 1;
          return String((a && a.id) || '').localeCompare(String((b && b.id) || ''));
        })
        .map(function (wp) {
          const id = String((wp && wp.id) || '').trim();
          if (!id) return '';
          const name = String((wp && wp.name) || '').trim() || id;
          const latLon = formatEfbSaUserWaypointLatLon(saMapTheater, wp);
          const mgrs = formatEfbSaUserWaypointMgrs(saMapTheater, wp);
          const active = id === selectedUserWaypointId ? ' active' : '';
          const editing = id === String(efbSaUserWaypointEditingId || '').trim();
          const nameLine = editing
            ? ('<div class="efbWaypointActions"><input type="text" class="efbWaypointNameBtn" data-efb-user-waypoint-edit-input="' + escapeHtml(id) + '" value="' + escapeHtml(String(efbSaUserWaypointEditDraft || name)) + '" /><button type="button" class="efbWaypointActionBtn" data-efb-user-waypoint-edit-save="' + escapeHtml(id) + '">Save</button><button type="button" class="efbWaypointActionBtn" data-efb-user-waypoint-edit-cancel="' + escapeHtml(id) + '">Cancel</button></div>')
            : ('<button type="button" class="efbWaypointNameBtn" data-efb-user-waypoint-select="' + escapeHtml(id) + '">' + escapeHtml(name) + '</button>');
          return '<div class="efbWaypointRow' + active + '" data-efb-user-waypoint="' + escapeHtml(id) + '">'
            + nameLine
            + '<div class="efbWaypointMeta">LAT/LON: ' + escapeHtml(latLon) + '\nMGRS: ' + escapeHtml(mgrs) + '</div>'
            + '<div class="efbWaypointActions">'
            + '<button type="button" class="efbWaypointActionBtn" data-efb-user-waypoint-center="' + escapeHtml(id) + '">Center</button>'
            + '<button type="button" class="efbWaypointActionBtn" data-efb-user-waypoint-rename="' + escapeHtml(id) + '">Rename</button>'
            + '<button type="button" class="efbWaypointActionBtn delete" data-efb-user-waypoint-delete="' + escapeHtml(id) + '">Delete</button>'
            + '</div>'
            + '</div>';
        })
        .join('');
      const drawerRows = efbDrawerMode === 'chart'
        ? (options || '<span class="efbPickerEmpty">No charts for this airport</span>')
        : (efbDrawerMode === 'pinned'
          ? (pinnedRows || '<span class="efbPickerEmpty">No pinned charts for this airport</span>')
          : (efbDrawerMode === 'user-waypoints'
            ? (userWaypointRows || '<span class="efbPickerEmpty">No user waypoints yet. Long-press on SA map to add.</span>')
            : (airportOptions || '<span class="efbPickerEmpty">No airport folders</span>')));
      const drawerClass = efbDrawerOpen ? 'efbLeftActionDrawer' : 'efbLeftActionDrawer closed';
      const isSaMapMode = normalizeEfbViewerMode(efbViewerMode) === 'sa-map';
      const saQuickZoomState = (context && context.selected)
        ? getOpenFreeMapViewBySelection(context.selected)
        : null;
      const saQuickZoomActive = !!(isSaMapMode && saQuickZoomState && saQuickZoomState.adQuickZoomActive);
      const topNavClass = isSaMapMode ? 'efbTopNav efbTopNavHidden' : 'efbTopNav';
      const saToggleTitle = isSaMapMode ? 'Switch to Chart view' : 'Switch to SA Map view';

      return ''
        + '<div class="efbShell">'
        + '<div id="efbAutoAtaRecBalloon" class="efbAutoRecBalloon">---</div>'
        + '<div id="efbLoadingOverlay" class="efbLoadingOverlay' + (efbLoadingOverlayMessage ? ' visible' : '') + '"><div id="efbLoadingOverlayText" class="efbLoadingOverlayText">' + escapeHtml(efbLoadingOverlayMessage || '') + '</div></div>'
        + '<div class="' + topNavClass + '">'
        + '<button id="efbPrevChartBtn" type="button" class="efbTopNavBtn" title="Previous chart (uses pinned charts when available)"' + (canCycle ? '' : ' disabled') + '>&lt;&lt; PREV</button>'
        + '<button id="efbNextChartBtn" type="button" class="efbTopNavBtn" title="Next chart (uses pinned charts when available)"' + (canCycle ? '' : ' disabled') + '>NEXT &gt;&gt;</button>'
        + '</div>'
        + '<div class="efbLeftRail">'
        + '<button id="efbSaMapToggleBtn" type="button" class="efbRailBtn' + (isSaMapMode ? ' active' : '') + '" title="' + escapeHtml(saToggleTitle) + '">' + getEfbSaMapToggleIconSvg(isSaMapMode) + '</button>'
        + '<span class="efbRailSpacer" aria-hidden="true"></span>'
        + '<button id="efbCenterOwnshipBtn" type="button" class="efbRailBtn' + (isSaMapMode ? '' : ' efbRailBtnHidden') + '" title="Center SA Map on ownship">' + getEfbOwnshipCenterIconSvg() + '</button>'
        + '<button id="efbUserWaypointsBtn" type="button" class="efbRailBtn' + (isSaMapMode ? '' : ' efbRailBtnHidden') + (efbDrawerOpen && efbDrawerMode === 'user-waypoints' ? ' active' : '') + '" title="User waypoint drawer">' + getEfbUserWaypointsIconSvg() + '</button>'
        + '<button id="efbSavedHistoryTracksBtn" type="button" class="efbRailBtn' + (isSaMapMode ? '' : ' efbRailBtnHidden') + (efbDrawerOpen && efbDrawerMode === 'history-tracks' ? ' active' : '') + '" title="Saved history track drawer">' + getEfbSavedHistoryTracksIconSvg() + '</button>'
        + '<button id="efbSaFollowBtn" type="button" class="efbRailBtn' + (isSaMapMode ? '' : ' efbRailBtnHidden') + (efbSaFollowOwnshipEnabled ? ' active' : '') + '" title="Follow ownship ' + (efbSaFollowOwnshipEnabled ? 'ON' : 'OFF') + '">' + getEfbSaFollowIconSvg() + '</button>'
        + '<button id="efbSaTrackUpBtn" type="button" class="efbRailBtn' + (isSaMapMode ? '' : ' efbRailBtnHidden') + (efbSaTrackUpEnabled ? ' active' : '') + '" title="Track-up ' + (efbSaTrackUpEnabled ? 'ON' : 'OFF') + '">' + getEfbSaTrackUpIconSvg() + '</button>'
        + '<button id="efbSaAnchorBtn" type="button" class="efbRailBtn' + (isSaMapMode && efbSaFollowOwnshipEnabled ? '' : ' efbRailBtnHidden') + '" title="Anchor: ' + (normalizeEfbSaAnchorMode(efbSaAnchorMode) === 'lower-third' ? 'Lower third' : 'Center') + '">' + (normalizeEfbSaAnchorMode(efbSaAnchorMode) === 'lower-third' ? getEfbSaAnchorLowerThirdIconSvg() : getEfbSaAnchorCenterIconSvg()) + '</button>'
        + '<button id="efbSaLayersBtn" type="button" class="efbRailBtn' + (isSaMapMode ? '' : ' efbRailBtnHidden') + (saLayerDrawerOpen ? ' active' : '') + '" title="SA map layer controls">' + getEfbSaLayersIconSvg() + '</button>'
        + '<button id="efbSearchToggleBtn" type="button" class="efbRailBtn' + (isSaMapMode ? ' efbRailBtnHidden' : '') + '" title="Search charts">⌕</button>'
        + '<button id="efbAutoAirportBtn" type="button" class="efbRailBtn' + (isSaMapMode ? ' efbRailBtnHidden' : '') + '" title="Auto-select closest airport">' + getEfbAutoNearestAirportIconSvg() + '</button>'
        + '<button id="efbAirportToggleBtn" type="button" class="efbRailBtn' + (isSaMapMode ? ' efbRailBtnHidden' : '') + '" title="Select airport and chart"><svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true"><path d="M14 20h36v6h-4l2 9h-8l-2-7h-12l-2 7h-8l2-9h-4z" fill="currentColor"/><rect x="27" y="35" width="10" height="19" fill="currentColor"/><rect x="18" y="54" width="28" height="5" fill="currentColor"/><circle cx="32" cy="10" r="2.5" fill="currentColor"/><path d="M25 11a7 7 0 0 1 14 0" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round"/><path d="M21 11a11 11 0 0 1 22 0" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round"/></svg></button>'
        + '<button id="efbPinnedBtn" type="button" class="efbRailBtn' + (isSaMapMode ? ' efbRailBtnHidden' : '') + '" title="Pinned charts drawer">' + getEfbPinnedDrawerIconSvg() + '</button>'
        + '<button id="efbQuickPinBtn" type="button" class="efbRailBtn' + (isSaMapMode ? ' efbRailBtnHidden' : '') + (isChartPinned(airportKey, selected) ? ' active' : '') + '" title="' + escapeHtml(isChartPinned(airportKey, selected) ? 'Unpin current chart' : 'Pin current chart') + '">' + getEfbPinToggleIconSvg() + '</button>'
        + '</div>'
        + '<div class="efbRightRail">'
        + '<button id="efbZoomOutBtn" type="button" class="efbRailBtn" title="Zoom out">−</button>'
        + '<button id="efbZoomResetBtn" type="button" class="efbRailBtn" title="Reset view">⟳</button>'
        + '<button id="efbZoomInBtn" type="button" class="efbRailBtn" title="Zoom in">+</button>'
        + '<span class="efbRailSpacer" aria-hidden="true"></span>'
        + '<button id="efbRotateToolsBtn" type="button" class="efbRailBtn' + (isSaMapMode ? ' efbRailBtnHidden' : '') + '" title="Chart orientation tools">⤾</button>'
        + '<button id="efbQuickAdZoomBtn" type="button" class="efbRailBtn' + (isSaMapMode ? '' : ' efbRailBtnHidden') + (saQuickZoomActive ? ' active' : '') + '" title="Aerodrome quick zoom">AD</button>'
        + '</div>'
        + '<div id="efbRightActionDrawer" class="' + (isSaMapMode ? 'efbRightActionDrawer closed' : (efbRightToolsOpen ? 'efbRightActionDrawer' : 'efbRightActionDrawer closed')) + '">'
        + '<button id="efbRotateLeftBtn" type="button" class="efbRightDrawerBtn" title="Rotate left">⟲</button>'
        + '<button id="efbRotateRightBtn" type="button" class="efbRightDrawerBtn" title="Rotate right">⟳</button>'
        + '<button id="efbFitHorizontalBtn" type="button" class="efbRightDrawerBtn" title="Fit horizontally">↔</button>'
        + '<button id="efbFitVerticalBtn" type="button" class="efbRightDrawerBtn" title="Fit vertically">↕</button>'
        + '</div>'
        + '<div id="efbLeftActionDrawer" class="' + drawerClass + '">'
        + '<div class="efbDrawerHeader"><span id="efbDrawerTitle">' + drawerTitle + '</span><button id="efbDrawerCloseBtn" class="efbDrawerCloseBtn" type="button">✕</button></div>'
        + '<div id="efbDrawerList" class="efbDrawerList">' + drawerRows + '</div>'
        + '</div>'
        + '<div id="efbSaLayerDrawer" class="' + (saLayerDrawerOpen ? 'efbSaLayerDrawer' : 'efbSaLayerDrawer closed') + '">'
        + (navAuth
          ? ('<button type="button" class="efbRightDrawerBtn active" data-efb-sa-layer="nav-base" title="Navigraph base layer: ' + escapeHtml(navLayerLabel) + '">' + getEfbSaNavigraphLayerIconSvg(navLayer) + '</button>')
          : '')
        + '<button type="button" class="efbRightDrawerBtn' + (dlinkOnEnabled ? ' active' : '') + '" data-efb-sa-layer="dlink" title="D-Link assets ' + (dlinkOnEnabled ? 'ON' : 'OFF') + '">' + getEfbSaLayerDlinkIconSvg() + '</button>'
        + '<button type="button" class="efbRightDrawerBtn' + (efbSaShowJtacTargets !== false ? ' active' : '') + '" data-efb-sa-layer="jtac" title="JTAC targets ' + (efbSaShowJtacTargets !== false ? 'ON' : 'OFF') + '">' + getEfbSaLayerJtacIconSvg() + '</button>'
        + '<button type="button" class="efbRightDrawerBtn' + (efbSaShowAirports ? ' active' : '') + '" data-efb-sa-layer="airports" title="Airport symbols ' + (efbSaShowAirports ? 'ON' : 'OFF') + '">' + getEfbSaLayerAirportIconSvg() + '</button>'
        + '<button type="button" class="efbRightDrawerBtn' + (efbSaShowNavlog ? ' active' : '') + '" data-efb-sa-layer="navlog" title="Navlog route and waypoints ' + (efbSaShowNavlog ? 'ON' : 'OFF') + '">' + getEfbSaLayerNavlogIconSvg() + '</button>'
        + '<button type="button" class="efbRightDrawerBtn' + (efbSaShowMissionDrawings ? ' active' : '') + '" data-efb-sa-layer="miz" title="Mission snapshot (MIZ) ' + (efbSaShowMissionDrawings ? 'ON' : 'OFF') + '">' + getEfbSaLayerMissionDrawingsIconSvg() + '</button>'
        + '<button type="button" class="efbRightDrawerBtn' + (efbSaShowHistoryTrack ? ' active' : '') + '" data-efb-sa-layer="history-track" title="History track ' + (efbSaShowHistoryTrack ? 'ON' : 'OFF') + '">' + getEfbSaLayerHistoryTrackIconSvg() + '</button>'
        + '<button type="button" class="efbRightDrawerBtn' + (efbSaShowSamThreatRings ? ' active' : '') + '" data-efb-sa-layer="sam-threats" title="SAM threat rings ' + (efbSaShowSamThreatRings ? 'ON' : 'OFF') + '">' + getEfbSaLayerSamThreatIconSvg() + '</button>'
        + '<button type="button" class="efbRightDrawerBtn' + (efbSaShowDoghouses !== false ? ' active' : '') + '" data-efb-sa-layer="doghouses" title="Doghouse leg data ' + (efbSaShowDoghouses !== false ? 'ON' : 'OFF') + '">' + getEfbSaLayerDoghouseIconSvg() + '</button>'
        + '<button type="button" class="efbRightDrawerBtn' + (efbSaShowUserWaypoints !== false ? ' active' : '') + '" data-efb-sa-layer="user-waypoints" title="User waypoints ' + (efbSaShowUserWaypoints !== false ? 'ON' : 'OFF') + '">' + getEfbSaLayerUserWaypointIconSvg() + '</button>'
        + '<button type="button" class="efbRightDrawerBtn' + (efbSaShowDtcOverlay ? ' active' : '') + '" data-efb-sa-layer="dtc" title="DTC SA overlay ' + (efbSaShowDtcOverlay ? 'ON' : 'OFF') + '">' + getEfbSaLayerOverlayIconSvg() + '</button>'
        + '</div>'
        + '<div id="efbSearchOverlay" class="' + ((efbSearchOverlayOpen && !efbSearchAnimateOpenOnce) ? 'efbSearchOverlay open' : 'efbSearchOverlay') + '">'
        + '<div class="efbSearchPanel">'
        + '<button id="efbSearchCloseBtn" class="efbSearchCloseBtn" type="button">✕</button>'
        + '<div class="efbSearchWheelWrap">'
        + wheelOuterHtml
        + wheelInnerHtml
        + '<div class="efbSearchCenter">'
        + '<div id="efbSearchToken" class="efbSearchToken">' + escapeHtml(tokenDisplay) + '</div>'
        + '<div class="efbSearchCenterActions">'
        + '<button type="button" class="efbSearchActionBtn" data-efb-search-action="back">Back</button>'
        + '<button type="button" class="efbSearchActionBtn" data-efb-search-action="clear">Clear</button>'
        + (isRenameSearchMode
          ? '<button type="button" class="efbSearchActionBtn" data-efb-search-action="apply-rename">Apply</button>'
          : '')
        + '</div>'
        + '</div>'
        + '</div>'
        + '<div class="efbSearchResults">'
        + '<div class="efbSearchResultsHead">' + escapeHtml(isRenameSearchMode ? 'Name · ' : 'Code · ') + escapeHtml(tokenDisplay) + '</div>'
        + '<div id="efbSearchCandidates" class="efbSearchCandidates">' + (searchRows || '<div class="efbSearchEmpty">No matching airports.</div>') + '</div>'
        + '</div>'
        + '</div>'
        + '</div>'
        + '<div id="efbChartViewport" class="efbViewport' + (isSaMapMode ? ' saMapMode' : '') + '">'
        + (isSaMapMode
          ? formatEfbSaMapViewportHtml(data)
          : (chartUrl
            ? ('<img id="efbChartImage" class="efbChartImage ' + (isNavigraphChart ? 'efbChartImageNavigraph' : 'efbChartImageLocal') + '" src="' + chartUrl + '" alt="EFB chart">')
            : '<div class="efbEmpty">No chart available for selected airport.</div>'))
        + '</div>'
        + '</div>';
    }

    function bindEfbInteractions(data) {
      const airportKey = getEfbAirportKey(data);
      const viewport = getEfbViewport(airportKey);
      applyEfbViewportDefaults(viewport);

      function getCurrentEfbChartId() {
        const charts = efbChartsByAirport[airportKey] || [];
        return String(efbSelectedChartByAirport[airportKey] || (charts[0] && charts[0].id) || '');
      }

      function applyEfbQuickPinButtonUi() {
        const quickPinBtn = document.getElementById('efbQuickPinBtn');
        if (!quickPinBtn) return;
        const selectedId = getCurrentEfbChartId();
        const hasSelected = !!selectedId;
        const pinned = hasSelected && isChartPinned(airportKey, selectedId);
        quickPinBtn.classList.toggle('active', !!pinned);
        quickPinBtn.disabled = !hasSelected;
        quickPinBtn.title = !hasSelected
          ? 'No chart selected to pin'
          : (pinned ? 'Unpin current chart' : 'Pin current chart');
      }

      function applyEfbDrawerUi() {
        const drawer = document.getElementById('efbLeftActionDrawer');
        const saLayerDrawer = document.getElementById('efbSaLayerDrawer');
        const title = document.getElementById('efbDrawerTitle');
        const list = document.getElementById('efbDrawerList');
        if (!drawer || !title || !list) return;

        const isSaLayerDrawer = efbDrawerOpen && efbDrawerMode === 'sa-layers';
        drawer.className = (efbDrawerOpen && !isSaLayerDrawer) ? 'efbLeftActionDrawer' : 'efbLeftActionDrawer closed';
        if (saLayerDrawer) {
          saLayerDrawer.className = isSaLayerDrawer ? 'efbSaLayerDrawer' : 'efbSaLayerDrawer closed';
        }
        if (efbDrawerOpen && efbDrawerAnimateOpenOnce) {
          const animateNode = isSaLayerDrawer ? saLayerDrawer : drawer;
          const closedClass = isSaLayerDrawer ? 'efbSaLayerDrawer closed' : 'efbLeftActionDrawer closed';
          const openClass = isSaLayerDrawer ? 'efbSaLayerDrawer' : 'efbLeftActionDrawer';
          if (animateNode) {
            animateNode.className = closedClass;
            if (typeof window !== 'undefined' && typeof window.requestAnimationFrame === 'function') {
              window.requestAnimationFrame(function () {
                animateNode.className = openClass;
              });
            } else {
              animateNode.className = openClass;
            }
          }
          efbDrawerAnimateOpenOnce = false;
        }
        title.textContent = efbDrawerMode === 'chart'
          ? ('Charts · ' + (airportKey || 'UNSET'))
          : (efbDrawerMode === 'pinned'
            ? 'Pinned charts'
            : (efbDrawerMode === 'sa-layers'
              ? 'SA Map Layers'
              : (efbDrawerMode === 'user-waypoints' ? 'User Waypoints' : (efbDrawerMode === 'history-tracks' ? 'History Tracks' : 'Airports'))));

        if (efbDrawerMode === 'chart') {
          const charts = efbChartsByAirport[airportKey] || [];
          const selected = String(efbSelectedChartByAirport[airportKey] || (charts[0] && charts[0].id) || '');
          const rows = charts.map(function (c) {
            const id = String((c && c.id) || '');
            const label = String((c && c.name) || id || 'Chart');
            const active = id === selected ? ' active' : '';
            const pinnedClass = isChartPinned(airportKey, id) ? ' pinned' : '';
            const pinTitle = isChartPinned(airportKey, id) ? 'Unpin chart' : 'Pin chart';
            return '<div class="efbPickerRow' + active + '"><button type="button" class="efbPickerItemMain" data-efb-chart="' + escapeHtml(encodeURIComponent(id)) + '">' + escapeHtml(label) + '</button><button type="button" class="efbPinBtn' + pinnedClass + '" data-efb-pin-toggle="' + escapeHtml(encodeURIComponent(id)) + '" title="' + escapeHtml(pinTitle) + '">📌</button></div>';
          }).join('');
          list.innerHTML = rows || '<span class="efbPickerEmpty">No charts for this airport</span>';
        } else if (efbDrawerMode === 'pinned') {
          const charts = efbChartsByAirport[airportKey] || [];
          const selected = String(efbSelectedChartByAirport[airportKey] || (charts[0] && charts[0].id) || '');
          const pinned = getPinnedChartsForAirport(airportKey);
          const rows = charts
            .filter(function (c) {
              const id = String((c && c.id) || '');
              return id && pinned.indexOf(id) >= 0;
            })
            .map(function (c) {
              const id = String((c && c.id) || '');
              const label = String((c && c.name) || id || 'Chart');
              const active = id === selected ? ' active' : '';
              return '<div class="efbPickerRow' + active + '"><button type="button" class="efbPickerItemMain" data-efb-chart="' + escapeHtml(encodeURIComponent(id)) + '">' + escapeHtml(label) + '</button><button type="button" class="efbPinBtn pinned" data-efb-pin-toggle="' + escapeHtml(encodeURIComponent(id)) + '" title="Unpin chart">📌</button></div>';
            }).join('');
          list.innerHTML = rows || '<span class="efbPickerEmpty">No pinned charts for this airport</span>';
        } else if (efbDrawerMode === 'sa-layers') {
          list.innerHTML = '';
        } else if (efbDrawerMode === 'user-waypoints') {
          const model = latestData || data;
          const theater = String(resolveOpenFreeMapFallbackTheatreText(model) || '').trim();
          const context = resolveActiveSaMapContext(model);
          const selectedAssetKey = context && context.selected ? getMapSelectedAssetKeyBySelection(context.selected) : '';
          const selectedUserWaypointId = parseEfbSaUserWaypointAssetKey(selectedAssetKey);
          const rows = (Array.isArray(efbSaUserWaypoints) ? efbSaUserWaypoints : [])
            .slice()
            .sort(function (a, b) {
              const an = String((a && a.name) || '').toUpperCase();
              const bn = String((b && b.name) || '').toUpperCase();
              if (an < bn) return -1;
              if (an > bn) return 1;
              return String((a && a.id) || '').localeCompare(String((b && b.id) || ''));
            })
            .map(function (wp) {
              const id = String((wp && wp.id) || '').trim();
              if (!id) return '';
              const name = String((wp && wp.name) || '').trim() || id;
              const latLon = formatEfbSaUserWaypointLatLon(theater, wp);
              const mgrs = formatEfbSaUserWaypointMgrs(theater, wp);
              const active = id === selectedUserWaypointId ? ' active' : '';
              const editing = id === String(efbSaUserWaypointEditingId || '').trim();
              const nameLine = editing
                ? ('<div class="efbWaypointActions"><input type="text" class="efbWaypointNameBtn" data-efb-user-waypoint-edit-input="' + escapeHtml(id) + '" value="' + escapeHtml(String(efbSaUserWaypointEditDraft || name)) + '" /><button type="button" class="efbWaypointActionBtn" data-efb-user-waypoint-edit-save="' + escapeHtml(id) + '">Save</button><button type="button" class="efbWaypointActionBtn" data-efb-user-waypoint-edit-cancel="' + escapeHtml(id) + '">Cancel</button></div>')
                : ('<button type="button" class="efbWaypointNameBtn" data-efb-user-waypoint-select="' + escapeHtml(id) + '">' + escapeHtml(name) + '</button>');
              return '<div class="efbWaypointRow' + active + '" data-efb-user-waypoint="' + escapeHtml(id) + '">'
                + nameLine
                + '<div class="efbWaypointMeta">LAT/LON: ' + escapeHtml(latLon) + '\nMGRS: ' + escapeHtml(mgrs) + '</div>'
                + '<div class="efbWaypointActions">'
                + '<button type="button" class="efbWaypointActionBtn" data-efb-user-waypoint-center="' + escapeHtml(id) + '">Center</button>'
                + '<button type="button" class="efbWaypointActionBtn" data-efb-user-waypoint-rename="' + escapeHtml(id) + '">Rename</button>'
                + '<button type="button" class="efbWaypointActionBtn delete" data-efb-user-waypoint-delete="' + escapeHtml(id) + '">Delete</button>'
                + '</div>'
                + '</div>';
            }).join('');
          list.innerHTML = rows || '<span class="efbPickerEmpty">No user waypoints yet. Long-press on SA map to add.</span>';
        } else if (efbDrawerMode === 'history-tracks') {
          const model = latestData || data;
          const context = resolveActiveSaMapContext(model);
          const selectionKey = context && context.selected ? getFlightPlanEtaStartKey(context.selected) : '';
          const rows = (selectionKey
            ? getEfbSaSavedHistoryTracksBySelection(selectionKey)
            : [])
            .slice()
            .sort(function (a, b) {
              return Number((b && b.updatedUtcMs) || 0) - Number((a && a.updatedUtcMs) || 0);
            })
            .map(function (row) {
              const id = String((row && row.id) || '').trim();
              if (!id) return '';
              const name = String((row && row.name) || '').trim() || id;
              const points = Array.isArray(row && row.points) ? row.points : [];
              const active = row && row.visible !== false ? ' active' : '';
          const nameLine = '<button type="button" class="efbWaypointNameBtn" data-efb-history-track-toggle-visible="' + escapeHtml(id) + '">' + escapeHtml(name) + '</button>';
              const stateLabel = (row && row.visible !== false) ? 'Showing' : 'Hidden';
              return '<div class="efbWaypointRow' + active + '" data-efb-history-track="' + escapeHtml(id) + '">'
                + nameLine
                + '<div class="efbWaypointMeta">' + escapeHtml(stateLabel) + ' · ' + escapeHtml(String(points.length)) + ' pts</div>'
                + '<div class="efbWaypointActions">'
                + '<button type="button" class="efbWaypointActionBtn" data-efb-history-track-rename="' + escapeHtml(id) + '">Rename</button>'
                + '<button type="button" class="efbWaypointActionBtn delete" data-efb-history-track-delete="' + escapeHtml(id) + '">Delete</button>'
                + '</div>'
                + '</div>';
            }).join('');
          list.innerHTML = '<div class="efbWaypointActions" style="margin-bottom:8px;"><button type="button" class="efbWaypointActionBtn" data-efb-history-track-save-active="1">Save Active</button><button type="button" class="efbWaypointActionBtn delete" data-efb-history-track-reset-active="1">Clear Active</button></div>'
            + (rows || '<span class="efbPickerEmpty">No saved history tracks yet.</span>');
        } else {
          const airports = Array.isArray(efbAvailableAirports) ? efbAvailableAirports : [];
          const rows = airports.map(function (a) {
            const code = String(a || '').toUpperCase().trim();
            if (!code) return '';
            const active = code === airportKey ? ' active' : '';
            return '<button type="button" class="efbPickerItem' + active + '" data-efb-airport="' + escapeHtml(code) + '">' + escapeHtml(getEfbAirportDisplayText(code)) + '</button>';
          }).join('');
          list.innerHTML = rows || '<span class="efbPickerEmpty">No airport folders</span>';
        }
      }

      function applyEfbSaLayerDrawerButtonUi() {
        const drawer = document.getElementById('efbSaLayerDrawer');
        if (!drawer) return;
        const dlinkBtn = drawer.querySelector('[data-efb-sa-layer="dlink"]');
        const navBaseBtn = drawer.querySelector('[data-efb-sa-layer="nav-base"]');
        const jtacBtn = drawer.querySelector('[data-efb-sa-layer="jtac"]');
        const airportsBtn = drawer.querySelector('[data-efb-sa-layer="airports"]');
        const navlogBtn = drawer.querySelector('[data-efb-sa-layer="navlog"]');
        const mizBtn = drawer.querySelector('[data-efb-sa-layer="miz"]');
        const historyTrackBtn = drawer.querySelector('[data-efb-sa-layer="history-track"]');
        const samThreatsBtn = drawer.querySelector('[data-efb-sa-layer="sam-threats"]');
        const doghousesBtn = drawer.querySelector('[data-efb-sa-layer="doghouses"]');
        const userWaypointsBtn = drawer.querySelector('[data-efb-sa-layer="user-waypoints"]');
        const dtcBtn = drawer.querySelector('[data-efb-sa-layer="dtc"]');
        if (navBaseBtn) {
          const layer = normalizeEfbSaNavigraphLayer(efbSaNavigraphLayer);
          navBaseBtn.classList.add('active');
          navBaseBtn.title = 'Navigraph base layer: ' + getEfbSaNavigraphLayerLabel(layer);
          navBaseBtn.innerHTML = getEfbSaNavigraphLayerIconSvg(layer);
        }
        if (dlinkBtn) {
          dlinkBtn.classList.toggle('active', !!dlinkOnEnabled);
          dlinkBtn.title = 'D-Link assets ' + (dlinkOnEnabled ? 'ON' : 'OFF');
        }
        if (jtacBtn) {
          jtacBtn.classList.toggle('active', efbSaShowJtacTargets !== false);
          jtacBtn.title = 'JTAC targets ' + (efbSaShowJtacTargets !== false ? 'ON' : 'OFF');
        }
        if (airportsBtn) {
          airportsBtn.classList.toggle('active', efbSaShowAirports !== false);
          airportsBtn.title = 'Airports ' + (efbSaShowAirports !== false ? 'ON' : 'OFF');
        }
        if (navlogBtn) {
          navlogBtn.classList.toggle('active', efbSaShowNavlog !== false);
          navlogBtn.title = 'Navlog route ' + (efbSaShowNavlog !== false ? 'ON' : 'OFF');
        }
        if (mizBtn) {
          mizBtn.classList.toggle('active', efbSaShowMissionDrawings !== false);
          mizBtn.title = 'Mission snapshot (MIZ) ' + (efbSaShowMissionDrawings !== false ? 'ON' : 'OFF');
        }
        if (historyTrackBtn) {
          historyTrackBtn.classList.toggle('active', efbSaShowHistoryTrack === true);
          historyTrackBtn.title = 'History track ' + (efbSaShowHistoryTrack === true ? 'ON' : 'OFF');
        }
        if (samThreatsBtn) {
          samThreatsBtn.classList.toggle('active', efbSaShowSamThreatRings === true);
          samThreatsBtn.title = 'SAM threat rings ' + (efbSaShowSamThreatRings === true ? 'ON' : 'OFF');
        }
        if (doghousesBtn) {
          doghousesBtn.classList.toggle('active', efbSaShowDoghouses !== false);
          doghousesBtn.title = 'Doghouse leg data ' + (efbSaShowDoghouses !== false ? 'ON' : 'OFF');
        }
        if (userWaypointsBtn) {
          userWaypointsBtn.classList.toggle('active', efbSaShowUserWaypoints !== false);
          userWaypointsBtn.title = 'User waypoints ' + (efbSaShowUserWaypoints !== false ? 'ON' : 'OFF');
        }
        if (dtcBtn) {
          dtcBtn.classList.toggle('active', efbSaShowDtcOverlay !== false);
          dtcBtn.title = 'DTC overlays ' + (efbSaShowDtcOverlay !== false ? 'ON' : 'OFF');
        }
      }

      const towerBtn = document.getElementById('efbAirportToggleBtn');
      if (towerBtn) {
        towerBtn.onclick = function (ev) {
          ev.stopPropagation();
          if (efbDrawerOpen) {
            closeEfbDrawer();
          } else {
            openEfbDrawer('airport');
          }
          applyEfbDrawerUi();
          applyEfbSaLayerDrawerButtonUi();
        };
      }

      const quickPinBtn = document.getElementById('efbQuickPinBtn');
      if (quickPinBtn) {
        quickPinBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          const selectedId = getCurrentEfbChartId();
          if (!selectedId) return;
          togglePinnedChart(airportKey, selectedId);
          applyEfbQuickPinButtonUi();
          applyEfbDrawerUi();
        };
      }

      const savedHistoryTracksBtn = document.getElementById('efbSavedHistoryTracksBtn');
      if (savedHistoryTracksBtn) {
        savedHistoryTracksBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          if (normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') return;
          if (efbDrawerOpen && efbDrawerMode === 'history-tracks') {
            closeEfbDrawer();
          } else {
            openEfbDrawer('history-tracks');
          }
          applyEfbDrawerUi();
        };
      }

      const userWaypointsBtn = document.getElementById('efbUserWaypointsBtn');
      if (userWaypointsBtn) {
        userWaypointsBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          if (normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') return;
          if (efbDrawerOpen && efbDrawerMode === 'user-waypoints') {
            closeEfbDrawer();
          } else {
            openEfbDrawer('user-waypoints');
          }
          applyEfbDrawerUi();
        };
      }

      applyEfbQuickPinButtonUi();

      const saLayerDrawer = document.getElementById('efbSaLayerDrawer');
      if (saLayerDrawer) {
        saLayerDrawer.onclick = function (ev) {
          const layerItem = ev && ev.target && ev.target.closest ? ev.target.closest('[data-efb-sa-layer]') : null;
          if (!layerItem) return;
          const layer = String(layerItem.getAttribute('data-efb-sa-layer') || '').toLowerCase();
          if (layer === 'nav-base') {
            efbSaNavigraphLayer = getNextEfbSaNavigraphLayer(efbSaNavigraphLayer);
            persistEfbSaNavigraphLayerPreference();
            refreshEfbSaMapTilesIfActive();
          } else if (layer === 'dlink') {
            dlinkOnEnabled = !dlinkOnEnabled;
            persistDlinkOnPreference();
            applyDlinkOnUi();
          } else if (layer === 'jtac') {
            efbSaShowJtacTargets = !efbSaShowJtacTargets;
          } else if (layer === 'airports') {
            efbSaShowAirports = !efbSaShowAirports;
          } else if (layer === 'navlog') {
            efbSaShowNavlog = !efbSaShowNavlog;
          } else if (layer === 'miz') {
            efbSaShowMissionDrawings = !efbSaShowMissionDrawings;
            persistEfbSaMissionDrawingsEnabledPreference();
          } else if (layer === 'history-track') {
            efbSaShowHistoryTrack = !efbSaShowHistoryTrack;
            persistEfbSaHistoryTrackEnabledPreference();
          } else if (layer === 'sam-threats') {
            efbSaShowSamThreatRings = !efbSaShowSamThreatRings;
            persistEfbSaSamThreatsEnabledPreference();
          } else if (layer === 'doghouses') {
            efbSaShowDoghouses = !efbSaShowDoghouses;
            persistEfbSaDoghousesEnabledPreference();
          } else if (layer === 'user-waypoints') {
            efbSaShowUserWaypoints = !efbSaShowUserWaypoints;
            persistEfbSaUserWaypointsEnabledPreference();
          } else if (layer === 'dtc') {
            efbSaShowDtcOverlay = !efbSaShowDtcOverlay;
          }
          applyEfbSaLayerDrawerButtonUi();
          applyEfbDrawerUi();

          const isSaMapMode = (selectedTab === 'EFB' && normalizeEfbViewerMode(efbViewerMode) === 'sa-map');
          if (isSaMapMode) {
            efbUiDirty = false;
          } else {
            efbUiDirty = true;
          }
          if (latestData) render(latestData);
        };
      }

      applyEfbSaLayerDrawerButtonUi();

      const saLayersBtn = document.getElementById('efbSaLayersBtn');
      if (saLayersBtn) {
        saLayersBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          if (normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') return;
          if (efbDrawerOpen && efbDrawerMode === 'sa-layers') {
            closeEfbDrawer();
          } else {
            openEfbDrawer('sa-layers');
          }
          applyEfbDrawerUi();
        };
      }

      const autoBtn = document.getElementById('efbAutoAirportBtn');
      if (autoBtn) {
        autoBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          efbManuallySelectedAirport = '';
          applyEfbAutoNearestAirport(latestData || data);
          efbUiDirty = true;
          openEfbDrawer('chart');
          if (latestData) render(latestData);
        };
      }

      const pinnedBtn = document.getElementById('efbPinnedBtn');
      if (pinnedBtn) {
        pinnedBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          if (efbDrawerOpen && efbDrawerMode === 'pinned') {
            closeEfbDrawer();
          } else {
            openEfbDrawer('pinned');
          }
          applyEfbDrawerUi();
        };
      }

      const searchToggleBtn = document.getElementById('efbSearchToggleBtn');
      const saMapToggleBtn = document.getElementById('efbSaMapToggleBtn');
      const centerOwnshipBtn = document.getElementById('efbCenterOwnshipBtn');
      const saFollowBtn = document.getElementById('efbSaFollowBtn');
      const saTrackUpBtn = document.getElementById('efbSaTrackUpBtn');
      const saAnchorBtn = document.getElementById('efbSaAnchorBtn');
      if (saMapToggleBtn) {
        saMapToggleBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          efbViewerMode = normalizeEfbViewerMode(efbViewerMode) === 'sa-map' ? 'chart' : 'sa-map';
          const contextBeforeToggle = resolveActiveSaMapContext(latestData || data);
          if (contextBeforeToggle && contextBeforeToggle.selected) {
            const quickState = getOpenFreeMapViewBySelection(contextBeforeToggle.selected);
            if (quickState) quickState.adQuickZoomActive = false;
          }
          persistEfbViewerModePreference();
          closeEfbDrawer();
          efbRightToolsOpen = false;
          efbSearchOverlayOpen = false;
          efbSearchAnimateOpenOnce = false;
          efbUiDirty = true;
          if (latestData) render(latestData);
        };
      }

      if (saFollowBtn) {
        saFollowBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          if (normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') return;
          efbSaFollowOwnshipEnabled = !efbSaFollowOwnshipEnabled;
          persistEfbSaFollowEnabledPreference();
          if (efbSaFollowOwnshipEnabled) {
            const context = resolveActiveSaMapContext(latestData || data);
            if (context && context.selected) {
              const k = getFlightPlanEtaStartKey(context.selected);
              if (k) efbSaFollowSuppressUntilBySelection[k] = 0;
            }
          }
          efbUiDirty = true;
          if (latestData) render(latestData);
        };
      }

      if (saTrackUpBtn) {
        saTrackUpBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          if (normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') return;
          efbSaTrackUpEnabled = !efbSaTrackUpEnabled;
          persistEfbSaTrackUpEnabledPreference();
          efbUiDirty = true;
          if (latestData) render(latestData);
        };
      }

      if (saAnchorBtn) {
        saAnchorBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          if (normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') return;
          if (!efbSaFollowOwnshipEnabled) return;
          efbSaAnchorMode = normalizeEfbSaAnchorMode(efbSaAnchorMode) === 'center' ? 'lower-third' : 'center';
          persistEfbSaAnchorModePreference();
          efbUiDirty = true;
          if (latestData) render(latestData);
        };
      }

      if (centerOwnshipBtn) {
        centerOwnshipBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          if (normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') return;
          const context = resolveActiveSaMapContext(latestData || data);
          if (!context || !context.selected) return;
          const mapKey = getFlightPlanEtaStartKey(context.selected);
          if (mapKey) {
            efbSaFollowSuppressUntilBySelection[mapKey] = 0;
          }
          const handled = (typeof handleOpenFreeMapCenterOwnship === 'function')
            ? handleOpenFreeMapCenterOwnship(context.selected, latestData || data)
            : false;
          if (!handled && latestData) {
            render(latestData);
          }
        };
      }

      const closeSearchOverlayAnimated = function (afterClose) {
        const finalizeClose = function () {
          efbSearchOverlayOpen = false;
          efbSearchAnimateOpenOnce = false;
          efbSearchMode = 'airport';
          if (typeof afterClose === 'function') {
            afterClose();
          }
          efbUiDirty = true;
          if (latestData) render(latestData);
        };

        const overlay = document.getElementById('efbSearchOverlay');
        if (!efbSearchOverlayOpen || !overlay) {
          finalizeClose();
          return;
        }

        overlay.classList.remove('open');
        window.setTimeout(finalizeClose, 280);
      };

      const refreshEfbSearchOverlayUi = function () {
        const renameSearchMode = efbSearchMode === 'user-waypoint-rename' || efbSearchMode === 'history-track-rename';
        const normalizedToken = renameSearchMode
          ? normalizeEfbWaypointRenameToken(efbSearchToken)
          : normalizeEfbSearchToken(efbSearchToken);
        efbSearchToken = normalizedToken;
        const tokenDisplay = normalizedToken || (renameSearchMode ? 'NAME' : '----');

        const tokenEl = document.getElementById('efbSearchToken');
        if (tokenEl) {
          tokenEl.textContent = tokenDisplay;
        }

        const headEl = document.querySelector('#efbSearchOverlay .efbSearchResultsHead');
        if (headEl) {
          headEl.textContent = (renameSearchMode ? 'Name · ' : 'Code · ') + tokenDisplay;
        }

        const actionsWrap = document.querySelector('#efbSearchOverlay .efbSearchCenterActions');
        if (actionsWrap) {
          const existingApplyBtn = actionsWrap.querySelector('[data-efb-search-action="apply-rename"]');
          if (renameSearchMode) {
            if (!existingApplyBtn) {
              const applyBtn = document.createElement('button');
              applyBtn.type = 'button';
              applyBtn.className = 'efbSearchActionBtn';
              applyBtn.setAttribute('data-efb-search-action', 'apply-rename');
              applyBtn.textContent = 'Apply';
              actionsWrap.appendChild(applyBtn);
            }
          } else if (existingApplyBtn) {
            existingApplyBtn.remove();
          }
        }

        const candidatesEl = document.getElementById('efbSearchCandidates');
        if (!candidatesEl) return;
        if (renameSearchMode) {
          candidatesEl.innerHTML = '<div class="efbSearchEmpty">Use wheel to build name, then Apply.</div>';
          return;
        }
        const candidates = getEfbSearchCandidates(normalizedToken, 120);
        const rows = candidates.map(function (code) {
          const active = code === airportKey ? ' active' : '';
          return '<button type="button" class="efbSearchCandidateBtn' + active + '" data-efb-search-candidate="' + escapeHtml(code) + '">' + escapeHtml(getEfbAirportDisplayText(code)) + '</button>';
        }).join('');
        candidatesEl.innerHTML = rows || '<div class="efbSearchEmpty">No matching airports.</div>';
      };

      if (searchToggleBtn) {
        searchToggleBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          const willOpen = !efbSearchOverlayOpen;
          if (willOpen) {
            efbSearchMode = 'airport';
            efbSearchOverlayOpen = true;
            closeEfbDrawer();
            efbRightToolsOpen = false;
            efbSearchAnimateOpenOnce = true;
            efbUiDirty = true;
            if (latestData) {
              render(latestData);
              window.requestAnimationFrame(function () {
                const overlay = document.getElementById('efbSearchOverlay');
                if (overlay) overlay.classList.add('open');
                efbSearchAnimateOpenOnce = false;
              });
            } else {
              efbSearchAnimateOpenOnce = false;
            }
            return;
          }
          closeSearchOverlayAnimated(function () {
            efbSearchToken = '';
          });
        };
      }

      const searchCloseBtn = document.getElementById('efbSearchCloseBtn');
      if (searchCloseBtn) {
        searchCloseBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          closeSearchOverlayAnimated(function () {
            efbSearchToken = '';
          });
        };
      }

      const searchOverlay = document.getElementById('efbSearchOverlay');
      if (searchOverlay) {
        searchOverlay.onclick = function (ev) {
          const target = getEventElementTarget(ev);
          if (!target) return;

          if (target === searchOverlay) {
            closeSearchOverlayAnimated(function () {
              efbSearchToken = '';
            });
            return;
          }

          const charBtn = target.closest ? target.closest('[data-efb-search-char]') : null;
          if (charBtn) {
            const ch = String(charBtn.getAttribute('data-efb-search-char') || '').trim().toUpperCase();
            const renameSearchMode = efbSearchMode === 'user-waypoint-rename' || efbSearchMode === 'history-track-rename';
            const normalizer = renameSearchMode
              ? normalizeEfbWaypointRenameToken
              : normalizeEfbSearchToken;
            const maxLen = renameSearchMode ? 10 : 4;
            if (ch && normalizer(efbSearchToken).length < maxLen) {
              efbSearchToken = normalizer(efbSearchToken + ch);
              refreshEfbSearchOverlayUi();
            }
            return;
          }

          const actionBtn = target.closest ? target.closest('[data-efb-search-action]') : null;
          if (actionBtn) {
            const action = String(actionBtn.getAttribute('data-efb-search-action') || '').toLowerCase();
            if (action === 'back') {
              efbSearchToken = ((efbSearchMode === 'user-waypoint-rename' || efbSearchMode === 'history-track-rename')
                ? normalizeEfbWaypointRenameToken(efbSearchToken)
                : normalizeEfbSearchToken(efbSearchToken)).slice(0, -1);
            } else if (action === 'clear') {
              efbSearchToken = '';
            } else if (action === 'apply-rename') {
              const tokenName = normalizeEfbWaypointRenameToken(efbSearchToken);
              if (efbSearchMode === 'history-track-rename') {
                const id = String(efbSaSavedHistoryTrackEditingId || '').trim();
                const row = findEfbSaSavedHistoryTrack(efbSaSavedHistoryTrackEditSelectionKey, id);
                if (row && tokenName) {
                  efbSaSavedHistoryTrackEditDraft = tokenName;
                  commitEfbSaSavedHistoryTrackRename();
                }
                closeSearchOverlayAnimated(function () {
                  openEfbDrawer('history-tracks');
                });
                return;
              }

              const id = String(efbSaUserWaypointEditingId || '').trim();
              const wp = findEfbSaUserWaypointById(id);
              if (wp && tokenName) {
                efbSaUserWaypointEditDraft = tokenName;
                commitEfbSaUserWaypointRename();
                setEfbSaUserWaypointDiag({ lastAction: 'edit-save-radial' });
              }
              closeSearchOverlayAnimated(function () {
                openEfbDrawer('user-waypoints');
              });
              return;
            }
            refreshEfbSearchOverlayUi();
            return;
          }

          const candidateBtn = target.closest ? target.closest('[data-efb-search-candidate]') : null;
          if (candidateBtn && efbSearchMode !== 'user-waypoint-rename' && efbSearchMode !== 'history-track-rename') {
            const nextAirport = String(candidateBtn.getAttribute('data-efb-search-candidate') || '').toUpperCase().trim();
            if (!nextAirport) return;
            closeSearchOverlayAnimated(function () {
              efbManuallySelectedAirport = nextAirport;
              efbSearchToken = '';
              openEfbDrawer('chart');
            });
          }
        };
      }

      const drawerCloseBtn = document.getElementById('efbDrawerCloseBtn');
      if (drawerCloseBtn) {
        drawerCloseBtn.onclick = function (ev) {
          ev.stopPropagation();
          closeEfbDrawer();
          applyEfbDrawerUi();
        };
      }

      const drawerList = document.getElementById('efbDrawerList');
      if (drawerList) {
        const runUserWaypointDrawerAction = function (ev) {
          if (!ev) return false;
          if (typeof ev.preventDefault === 'function') ev.preventDefault();
          if (typeof ev.stopPropagation === 'function') ev.stopPropagation();
          const target = getEventElementTarget(ev);
          if (!target || !target.closest) return false;
          const context = resolveActiveSaMapContext(latestData || data);

          const editSave = target.closest('[data-efb-user-waypoint-edit-save]');
          if (editSave) {
            const id = String(editSave.getAttribute('data-efb-user-waypoint-edit-save') || '').trim();
            if (!id || id !== String(efbSaUserWaypointEditingId || '').trim()) return true;
            if (commitEfbSaUserWaypointRename() && latestData) {
              setEfbSaUserWaypointDiag({ lastAction: 'edit-save' });
              applyEfbDrawerUi();
              efbUiDirty = false;
              render(latestData);
            }
            return true;
          }

          const editCancel = target.closest('[data-efb-user-waypoint-edit-cancel]');
          if (editCancel) {
            setEfbSaUserWaypointDiag({ lastAction: 'edit-cancel' });
            cancelEfbSaUserWaypointRename();
            applyEfbDrawerUi();
            efbUiDirty = false;
      requestRender();
            return true;
          }

          const userWaypointSelect = target.closest('[data-efb-user-waypoint-select]');
          if (userWaypointSelect) {
            const id = String(userWaypointSelect.getAttribute('data-efb-user-waypoint-select') || '').trim();
            const wp = findEfbSaUserWaypointById(id);
            if (!wp) return true;
            if (context && context.selected) {
              setMapSelectedAssetKeyBySelection(context.selected, buildEfbSaUserWaypointAssetKey(id));
            }
            setEfbSaUserWaypointDiag({ lastAction: 'select' });
            if (latestData) render(latestData);
            return true;
          }

          const userWaypointCenter = target.closest('[data-efb-user-waypoint-center]');
          if (userWaypointCenter) {
            const id = String(userWaypointCenter.getAttribute('data-efb-user-waypoint-center') || '').trim();
            const wp = findEfbSaUserWaypointById(id);
            if (!wp) return true;
            if (context && context.selected) {
              setMapSelectedAssetKeyBySelection(context.selected, buildEfbSaUserWaypointAssetKey(id));
              handleOpenFreeMapCenterUserWaypoint(context.selected, wp, latestData || data);
            }
            applyEfbDrawerUi();
            efbUiDirty = false;
            setEfbSaUserWaypointDiag({ lastAction: 'center' });
            if (latestData) render(latestData);
            return true;
          }

          const userWaypointRename = target.closest('[data-efb-user-waypoint-rename]');
          if (userWaypointRename) {
            const id = String(userWaypointRename.getAttribute('data-efb-user-waypoint-rename') || '').trim();
            const wp = findEfbSaUserWaypointById(id);
            if (!wp) return true;
            setEfbSaUserWaypointDiag({ lastAction: 'rename-open' });
            beginEfbSaUserWaypointRename(id);
            efbSearchMode = 'user-waypoint-rename';
            efbSearchToken = normalizeEfbWaypointRenameToken(efbSaUserWaypointEditDraft || wp.name || '');
            efbSearchOverlayOpen = true;
            efbSearchAnimateOpenOnce = true;
            closeEfbDrawer();
            applyEfbDrawerUi();
            refreshEfbSearchOverlayUi();
            window.requestAnimationFrame(function () {
              const overlay = document.getElementById('efbSearchOverlay');
              if (overlay) overlay.classList.add('open');
              efbSearchAnimateOpenOnce = false;
            });
            return true;
          }

          const userWaypointDelete = target.closest('[data-efb-user-waypoint-delete]');
          if (userWaypointDelete) {
            const id = String(userWaypointDelete.getAttribute('data-efb-user-waypoint-delete') || '').trim();
            const wp = findEfbSaUserWaypointById(id);
            if (!wp) return true;
            setEfbSaUserWaypointDiag({ lastAction: 'delete' });
            deleteEfbSaUserWaypoint(id);
            if (String(efbSaUserWaypointEditingId || '').trim() === id) {
              cancelEfbSaUserWaypointRename();
            }
            if (context && context.selected) {
              const currentKey = getMapSelectedAssetKeyBySelection(context.selected);
              if (parseEfbSaUserWaypointAssetKey(currentKey) === id) {
                setMapSelectedAssetKeyBySelection(context.selected, '');
              }
            }
            applyEfbDrawerUi();
            efbUiDirty = false;
            if (latestData) render(latestData);
            return true;
          }

          return false;
        };

        const runHistoryTrackDrawerAction = function (ev) {
          if (!ev) return false;
          if (typeof ev.preventDefault === 'function') ev.preventDefault();
          if (typeof ev.stopPropagation === 'function') ev.stopPropagation();
          const target = getEventElementTarget(ev);
          if (!target || !target.closest) return false;
          const context = resolveActiveSaMapContext(latestData || data);
          const selectionKey = context && context.selected ? getFlightPlanEtaStartKey(context.selected) : '';
          if (!selectionKey) return false;

          const saveActiveBtn = target.closest('[data-efb-history-track-save-active]');
          if (saveActiveBtn) {
            const created = saveActiveEfbSaHistoryTrack(selectionKey);
            if (created) {
              applyEfbDrawerUi();
              efbUiDirty = false;
              if (latestData) render(latestData);
            }
            return true;
          }

          const resetActiveBtn = target.closest('[data-efb-history-track-reset-active]');
          if (resetActiveBtn) {
            clearActiveEfbSaHistoryTrack(selectionKey);
            applyEfbDrawerUi();
            efbUiDirty = false;
            if (latestData) render(latestData);
            return true;
          }

          const editSave = target.closest('[data-efb-history-track-edit-save]');
          if (editSave) {
            const id = String(editSave.getAttribute('data-efb-history-track-edit-save') || '').trim();
            if (!id || id !== String(efbSaSavedHistoryTrackEditingId || '').trim()) return true;
            if (commitEfbSaSavedHistoryTrackRename()) {
              applyEfbDrawerUi();
              efbUiDirty = false;
              if (latestData) render(latestData);
            }
            return true;
          }

          const editCancel = target.closest('[data-efb-history-track-edit-cancel]');
          if (editCancel) {
            cancelEfbSaSavedHistoryTrackRename();
            applyEfbDrawerUi();
            efbUiDirty = false;
            if (latestData) render(latestData);
            return true;
          }

          const toggleVisible = target.closest('[data-efb-history-track-toggle-visible]');
          if (toggleVisible) {
            const id = String(toggleVisible.getAttribute('data-efb-history-track-toggle-visible') || '').trim();
            const row = findEfbSaSavedHistoryTrack(selectionKey, id);
            if (!row) return true;
            setSavedEfbSaHistoryTrackVisible(selectionKey, id, !(row.visible !== false));
            applyEfbDrawerUi();
            efbUiDirty = false;
            if (latestData) render(latestData);
            return true;
          }

          const renameBtn = target.closest('[data-efb-history-track-rename]');
          if (renameBtn) {
            const id = String(renameBtn.getAttribute('data-efb-history-track-rename') || '').trim();
            const row = findEfbSaSavedHistoryTrack(selectionKey, id);
            if (!row) return true;
            if (!beginEfbSaSavedHistoryTrackRename(selectionKey, id)) return true;
            efbSearchMode = 'history-track-rename';
            efbSearchToken = normalizeEfbWaypointRenameToken(efbSaSavedHistoryTrackEditDraft || row.name || '');
            efbSearchOverlayOpen = true;
            efbSearchAnimateOpenOnce = true;
            closeEfbDrawer();
            applyEfbDrawerUi();
            refreshEfbSearchOverlayUi();
            window.requestAnimationFrame(function () {
              const overlay = document.getElementById('efbSearchOverlay');
              if (overlay) overlay.classList.add('open');
              efbSearchAnimateOpenOnce = false;
            });
            return true;
          }

          const deleteBtn = target.closest('[data-efb-history-track-delete]');
          if (deleteBtn) {
            const id = String(deleteBtn.getAttribute('data-efb-history-track-delete') || '').trim();
            if (!deleteEfbSaSavedHistoryTrack(selectionKey, id)) return true;
            applyEfbDrawerUi();
            efbUiDirty = false;
            if (latestData) render(latestData);
            return true;
          }

          return false;
        };

        drawerList.onpointerdown = function (ev) {
          const target = getEventElementTarget(ev);
          if (!target || !target.closest) return;
          if (target.closest('[data-efb-user-waypoint-rename], [data-efb-user-waypoint-delete], [data-efb-user-waypoint-center], [data-efb-user-waypoint-select], [data-efb-user-waypoint-edit-save], [data-efb-user-waypoint-edit-cancel], [data-efb-history-track-save-active], [data-efb-history-track-reset-active], [data-efb-history-track-toggle-visible], [data-efb-history-track-rename], [data-efb-history-track-delete]')) {
            if (typeof ev.stopPropagation === 'function') ev.stopPropagation();
          }
        };

        drawerList.onpointerup = function (ev) {
          if (runUserWaypointDrawerAction(ev)) return;
        };
        drawerList.onclick = function (ev) {
          if (runUserWaypointDrawerAction(ev)) return;
          if (runHistoryTrackDrawerAction(ev)) return;
          const target = getEventElementTarget(ev);
          const context = resolveActiveSaMapContext(latestData || data);

          const pinItem = target && target.closest ? target.closest('[data-efb-pin-toggle]') : null;
          if (pinItem) {
            let pinId = String(pinItem.getAttribute('data-efb-pin-toggle') || '').trim();
            const pinAirport = String(pinItem.getAttribute('data-efb-pin-airport') || airportKey || '').toUpperCase().trim() || 'UNSET';
            if (!pinId) return;
            try { pinId = decodeURIComponent(pinId); } catch (_) { }
            togglePinnedChart(pinAirport, pinId);
            applyEfbDrawerUi();
            applyEfbQuickPinButtonUi();
            return;
          }

          const airportItem = target && target.closest ? target.closest('[data-efb-airport]') : null;
          if (airportItem) {
            const nextAirport = String(airportItem.getAttribute('data-efb-airport') || '').toUpperCase().trim();
            if (!nextAirport) return;
            efbManuallySelectedAirport = nextAirport;
            efbUiDirty = true;
            openEfbDrawer('chart');
            if (latestData) render(latestData);
            return;
          }

          const item = target && target.closest ? target.closest('[data-efb-chart]') : null;
          if (!item) return;
          let id = String(item.getAttribute('data-efb-chart') || '').trim();
          const sourceAirport = String(item.getAttribute('data-efb-chart-airport') || airportKey || '').toUpperCase().trim() || airportKey;
          if (!id) return;
          try { id = decodeURIComponent(id); } catch (_) { }
          efbManuallySelectedAirport = sourceAirport;
          efbSelectedChartByAirport[sourceAirport] = id;
          applyEfbQuickPinButtonUi();
          const targetViewport = getEfbViewport(sourceAirport);
          targetViewport.scale = 1;
          targetViewport.tx = 0;
          targetViewport.ty = 0;
          efbUiDirty = true;
          closeEfbDrawer();
          if (latestData) render(latestData);
        };

        drawerList.oninput = function (ev) {
          const target = getEventElementTarget(ev);
          if (!target || !target.closest) return;
          const input = target.closest('[data-efb-user-waypoint-edit-input]');
          if (!input) return;
          const id = String(input.getAttribute('data-efb-user-waypoint-edit-input') || '').trim();
          if (!id || id !== String(efbSaUserWaypointEditingId || '').trim()) return;
          efbSaUserWaypointEditDraft = String(input.value || '').slice(0, 24);
          setEfbSaUserWaypointDiag({ lastAction: 'edit-input' });
        };

        drawerList.onkeydown = function (ev) {
          const target = getEventElementTarget(ev);
          if (!target || !target.closest) return;
          const input = target.closest('[data-efb-user-waypoint-edit-input]');
          if (!input) return;
          if (ev.key === 'Enter') {
            if (commitEfbSaUserWaypointRename() && latestData) {
              render(latestData);
            }
            ev.preventDefault();
            return;
          }
          if (ev.key === 'Escape') {
            cancelEfbSaUserWaypointRename();
            if (latestData) render(latestData);
            ev.preventDefault();
          }
        };
      }

      if (!efbPickerDocumentHandlerBound) {
        document.addEventListener('click', function (ev) {
          if (selectedTab !== 'EFB') return;
          if (efbSelectionFlowActive) return;
          const target = getEventElementTarget(ev);
          const insideDrawer = target && target.closest && (target.closest('.efbLeftActionDrawer') || target.closest('.efbSaLayerDrawer') || target.closest('.efbLeftRail') || target.closest('.efbRightRail') || target.closest('.efbRightActionDrawer') || target.closest('.efbSearchOverlay'));
          if (!insideDrawer) {
            const wasSearchOpen = efbSearchOverlayOpen;
            closeEfbDrawer();
            efbRightToolsOpen = false;
            applyEfbDrawerUi();
            const rightDrawer = document.getElementById('efbRightActionDrawer');
            if (rightDrawer) rightDrawer.className = efbRightToolsOpen ? 'efbRightActionDrawer' : 'efbRightActionDrawer closed';
            if (wasSearchOpen) {
              closeSearchOverlayAnimated(function () {
                efbSearchToken = '';
              });
            }
          }
        });
        efbPickerDocumentHandlerBound = true;
      }

      applyEfbDrawerUi();

      const img = document.getElementById('efbChartImage');
      if (img) {
        applyEfbViewportToImage(img, viewport);
      }

      const isSaMapMode = normalizeEfbViewerMode(efbViewerMode) === 'sa-map';

      const applySaMapZoomAction = function (action) {
        if (!isSaMapMode) return false;
        const context = resolveActiveSaMapContext(data || latestData || null);
        const selectedKey = (context && context.selected)
          ? String(context.selected)
          : String(getActiveEfbSaSelectionKey() || '');
        if (!selectedKey) return false;
        const handledByWebMap = (typeof handleOpenFreeMapZoomAction === 'function')
          ? handleOpenFreeMapZoomAction(selectedKey, action)
          : false;
        if (!handledByWebMap) {
          if (action === 'in') zoomMapViewBySelection(selectedKey, 1.2);
          else if (action === 'out') zoomMapViewBySelection(selectedKey, 1 / 1.2);
          else if (action === 'reset') resetMapViewBySelection(selectedKey);
          if (latestData) render(latestData);
        }
        return true;
      };

      const zoomDelta = function (mult) {
        viewport.scale = clamp((Number(viewport.scale) || 1) * mult, 0.5, 3.5);
        const image = document.getElementById('efbChartImage');
        if (image) applyEfbViewportToImage(image, viewport);
      };

      const cycleChart = function (delta) {
        const cycleCharts = getEfbCycleChartsForAirport(airportKey);
        if (!cycleCharts.length) return;

        const selectedId = String(efbSelectedChartByAirport[airportKey] || (cycleCharts[0] && cycleCharts[0].id) || '');
        let index = cycleCharts.findIndex(function (c) { return String((c && c.id) || '') === selectedId; });
        if (index < 0) index = 0;

        const nextIndex = (((index + delta) % cycleCharts.length) + cycleCharts.length) % cycleCharts.length;
        const nextId = String((cycleCharts[nextIndex] && cycleCharts[nextIndex].id) || '');
        if (!nextId) return;

        efbSelectedChartByAirport[airportKey] = nextId;
        viewport.scale = 1;
        viewport.tx = 0;
        viewport.ty = 0;
        efbUiDirty = true;
        if (latestData) render(latestData);
      };

      const prevChartBtn = document.getElementById('efbPrevChartBtn');
      if (prevChartBtn) prevChartBtn.onclick = function (ev) {
        ev.preventDefault();
        ev.stopPropagation();
        cycleChart(-1);
      };

      const nextChartBtn = document.getElementById('efbNextChartBtn');
      if (nextChartBtn) nextChartBtn.onclick = function (ev) {
        ev.preventDefault();
        ev.stopPropagation();
        cycleChart(1);
      };

      const outBtn = document.getElementById('efbZoomOutBtn');
      if (outBtn) outBtn.onclick = function () {
        if (!applySaMapZoomAction('out')) {
          zoomDelta(0.9);
        }
      };
      const inBtn = document.getElementById('efbZoomInBtn');
      if (inBtn) inBtn.onclick = function () {
        if (!applySaMapZoomAction('in')) {
          zoomDelta(1.1);
        }
      };
      const resetBtn = document.getElementById('efbZoomResetBtn');
      if (resetBtn) resetBtn.onclick = function () {
        if (!applySaMapZoomAction('reset')) {
          viewport.scale = 1;
          viewport.tx = 0;
          viewport.ty = 0;
          viewport.rot = 0;
          const image = document.getElementById('efbChartImage');
          if (image) applyEfbViewportToImage(image, viewport);
        }
      };

      const rightDrawer = document.getElementById('efbRightActionDrawer');
      const rotateToolsBtn = document.getElementById('efbRotateToolsBtn');
      if (rotateToolsBtn) {
        rotateToolsBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          if (isSaMapMode) return;
          efbRightToolsOpen = !efbRightToolsOpen;
          if (rightDrawer) rightDrawer.className = efbRightToolsOpen ? 'efbRightActionDrawer' : 'efbRightActionDrawer closed';
        };
      }

      const quickAdZoomBtn = document.getElementById('efbQuickAdZoomBtn');
      if (quickAdZoomBtn) {
        quickAdZoomBtn.onclick = function (ev) {
          ev.preventDefault();
          ev.stopPropagation();
          applySaMapZoomAction('ad');
        };
      }

      const rotateLeftBtn = document.getElementById('efbRotateLeftBtn');
      if (rotateLeftBtn) rotateLeftBtn.onclick = function () {
        viewport.rot = normalizeRotationDeg((Number(viewport.rot) || 0) - 90);
        const image = document.getElementById('efbChartImage');
        if (image) applyEfbViewportToImage(image, viewport);
      };

      const rotateRightBtn = document.getElementById('efbRotateRightBtn');
      if (rotateRightBtn) rotateRightBtn.onclick = function () {
        viewport.rot = normalizeRotationDeg((Number(viewport.rot) || 0) + 90);
        const image = document.getElementById('efbChartImage');
        if (image) applyEfbViewportToImage(image, viewport);
      };

      const fitHorizontalBtn = document.getElementById('efbFitHorizontalBtn');
      if (fitHorizontalBtn) fitHorizontalBtn.onclick = function () {
        const image = document.getElementById('efbChartImage');
        const host = document.getElementById('efbChartViewport');
        if (!image || !host || !image.naturalWidth) return;
        const baseWidth = host.clientWidth;
        const baseHeight = (baseWidth * image.naturalHeight) / image.naturalWidth;
        const rotRad = (Math.PI / 180) * (Number(viewport.rot) || 0);
        const absCos = Math.abs(Math.cos(rotRad));
        const absSin = Math.abs(Math.sin(rotRad));
        const bboxWidthAtScale1 = (baseWidth * absCos) + (baseHeight * absSin);
        const scale = clamp(host.clientWidth / Math.max(1, bboxWidthAtScale1), 0.5, 3.5);
        viewport.scale = scale;
        viewport.tx = 0;
        viewport.ty = 0;
        applyEfbViewportToImage(image, viewport);
      };

      const fitVerticalBtn = document.getElementById('efbFitVerticalBtn');
      if (fitVerticalBtn) fitVerticalBtn.onclick = function () {
        const image = document.getElementById('efbChartImage');
        const host = document.getElementById('efbChartViewport');
        if (!image || !host || !image.naturalHeight) return;
        const baseWidth = host.clientWidth;
        const baseHeight = (baseWidth * image.naturalHeight) / image.naturalWidth;
        const rotRad = (Math.PI / 180) * (Number(viewport.rot) || 0);
        const absCos = Math.abs(Math.cos(rotRad));
        const absSin = Math.abs(Math.sin(rotRad));
        const bboxHeightAtScale1 = (baseWidth * absSin) + (baseHeight * absCos);
        const scale = clamp(host.clientHeight / Math.max(1, bboxHeightAtScale1), 0.5, 3.5);
        const fittedBboxHeight = bboxHeightAtScale1 * scale;
        viewport.scale = scale;
        viewport.tx = 0;
        viewport.ty = (fittedBboxHeight - baseHeight) / 2;
        applyEfbViewportToImage(image, viewport);
      };

      const viewEl = document.getElementById('efbChartViewport');
      if (viewEl) {
        viewEl.onmousedown = function (ev) {
          if (normalizeEfbViewerMode(efbViewerMode) === 'sa-map') return;
          const dragAirportKey = String(airportKey || 'UNSET').toUpperCase().trim() || 'UNSET';
          const dragViewport = getEfbViewport(dragAirportKey);
          applyEfbViewportDefaults(dragViewport);
          efbDragState = {
            airportKey: dragAirportKey,
            startX: ev.clientX,
            startY: ev.clientY,
            startTx: Number(dragViewport.tx) || 0,
            startTy: Number(dragViewport.ty) || 0,
          };
          ev.preventDefault();
        };
      }

      if (!bindEfbInteractions._moveRegistered) {
        document.addEventListener('mousemove', function (ev) {
          if (!efbDragState || selectedTab !== 'EFB') return;
          const dragAirportKey = String(efbDragState.airportKey || 'UNSET').toUpperCase().trim() || 'UNSET';
          const dragViewport = getEfbViewport(dragAirportKey);
          applyEfbViewportDefaults(dragViewport);
          dragViewport.tx = efbDragState.startTx + (ev.clientX - efbDragState.startX);
          dragViewport.ty = efbDragState.startTy + (ev.clientY - efbDragState.startY);
          const image = document.getElementById('efbChartImage');
          if (image) applyEfbViewportToImage(image, dragViewport);
        });

        document.addEventListener('mouseup', function () {
          efbDragState = null;
        });

        bindEfbInteractions._moveRegistered = true;
      }
    }

    function normalizeCategory(cat) {
      var c = String(cat || '').toUpperCase().trim();
      var compact = c.replace(/[^A-Z0-9]/g, '');
      if (c === 'RIO' || c === 'ICEMAN' || c === 'WSO' || c === 'GEORGE' || c === 'CPG' || c === 'AICPG' || c === 'AIWSO') return 'AI CREW';
      if (c === 'REF' || c === 'CREW' || c === 'REF/CREW') return 'GND CREW';
      if (compact === 'REFCREW' || compact === 'GROUNDCREW' || compact === 'GNDCREW' || compact === 'GROUND') return 'GND CREW';
      if (c === 'ALLIES') return 'FLIGHT';
      return c;
    }

    function normalizeActiveCategory(cat, data) {
      var c = String(cat || '').toUpperCase();
      return normalizeCategory(c);
    }

    function isExplicitServerCategoryChange(data) {
      const mode = String(data && data.ActiveCategoryUpdateMode || '').toLowerCase().trim();
      return mode === 'explicit';
    }

    function getVisibleTabs(data) {
      const tabs = TABS.slice(0);
      const efb = (data && data.Efb) || {};
      if (efb && efb.FeatureEnabled) {
        const existingIndex = tabs.indexOf('EFB');
        if (existingIndex >= 0) {
          tabs.splice(existingIndex, 1);
        }

        const dtcIndex = tabs.indexOf('DTC');
        const insertIndex = dtcIndex >= 0 ? (dtcIndex + 1) : 1;
        tabs.splice(insertIndex, 0, 'EFB');
      }
      return tabs;
    }

    function setSelectedTab(tab) {
      var normalized = normalizeCategory(tab);
      if (normalized === 'WX/ATC') normalized = 'ATC';
      const visibleTabs = getVisibleTabs(latestData);
      if (visibleTabs.indexOf(normalized) < 0) return false;

      const previousTab = selectedTab;
      if (previousTab === 'EFB' && normalized !== 'EFB') {
        efbLoadingOverlayMessage = '';
        efbDrawerOpen = false;
        efbDrawerMode = 'airport';
        efbSelectionFlowActive = false;
        efbSearchOverlayOpen = false;
        efbSearchAnimateOpenOnce = false;
        efbSearchToken = '';
      }

      selectedTab = normalized;
      try {
        const token = encodeURIComponent(String(selectedTab || '').trim());
        fetch('state?selectedTab=' + token, { cache: 'no-store' }).catch(function () { });
      } catch (_) {
      }
      if (latestData) render(latestData);
      return true;
    }

    function selectRelativeTab(step) {
      const tabs = getVisibleTabs(latestData);
      const idx = tabs.indexOf(selectedTab);
      const currentIndex = idx >= 0 ? idx : 0;
      const delta = step >= 0 ? 1 : -1;
      const nextIndex = (currentIndex + delta + tabs.length) % tabs.length;
      setSelectedTab(tabs[nextIndex]);
    }

    function getTabFromCustomExtraData(extraData) {
      if (extraData === null || extraData === undefined) return '';
      if (typeof extraData === 'string') return extraData;
      if (typeof extraData === 'number' && isFinite(extraData)) {
        const tabs = getVisibleTabs(latestData);
        const idx = ((Math.floor(extraData) % tabs.length) + tabs.length) % tabs.length;
        return tabs[idx];
      }
      if (typeof extraData !== 'object') return '';

      if (typeof extraData.tab === 'string') return extraData.tab;
      if (typeof extraData.category === 'string') return extraData.category;
      if (typeof extraData.name === 'string') return extraData.name;

      if (typeof extraData.index === 'number' && isFinite(extraData.index)) {
        const tabs = getVisibleTabs(latestData);
        const idx = ((Math.floor(extraData.index) % tabs.length) + tabs.length) % tabs.length;
        return tabs[idx];
      }

      return '';
    }

    function getCustomActionIdFromEvent(ev, detail) {
      const d = detail || {};
      const candidates = [
        d.id,
        d.actionId,
        d.customActionId,
        d.action && d.action.id,
        ev && ev.id,
        ev && ev.actionId
      ];

      for (let i = 0; i < candidates.length; i++) {
        const value = candidates[i];
        if (typeof value === 'string' && value.length) {
          return value;
        }
      }

      return '';
    }

    function getCustomActionExtraData(detail) {
      const d = detail || {};
      if (d.extraData !== undefined) return d.extraData;
      if (d.extra !== undefined) return d.extra;
      if (d.data !== undefined) return d.data;
      return undefined;
    }

    function handleCustomActionEvent(ev) {
      const detail = (ev && ev.detail) ? ev.detail : {};
      const id = getCustomActionIdFromEvent(ev, detail);
      const extraData = getCustomActionExtraData(detail);

      if (!id || id.indexOf(OKB_CUSTOM_ACTION_PREFIX) !== 0) return;

      if (id === OKB_ACTION_TAB_PREV) {
        selectRelativeTab(-1);
        return;
      }

      if (id === OKB_ACTION_TAB_NEXT) {
        selectRelativeTab(1);
        return;
      }

      if (id === OKB_ACTION_TAB_SELECT) {
        const requestedTab = getTabFromCustomExtraData(extraData);
        if (requestedTab) setSelectedTab(requestedTab);
        return;
      }

      const directTabIdPrefix = OKB_CUSTOM_ACTION_PREFIX + 'tab-';
      if (id.indexOf(directTabIdPrefix) !== 0) return;

      const tabToken = id.substring(directTabIdPrefix.length).replace(/-/g, ' ').toUpperCase();
      setSelectedTab(tabToken);
    }

    function registerCustomActionHandlers() {
      if (customActionHandlerRegistered) return;

      const okb = (typeof OpenKneeboard !== 'undefined') ? OpenKneeboard : window.OpenKneeboard;
      const targets = [okb, window];
      for (let i = 0; i < targets.length; i++) {
        const t = targets[i];
        if (!t || !t.addEventListener) continue;
        try {
          t.addEventListener('plugin/tab/customAction', handleCustomActionEvent);
          t.addEventListener('plugin/tab/custom-action', handleCustomActionEvent);
          customActionHandlerRegistered = true;
        } catch (_) {
        }
      }
    }

    function readNightModePreference() {
      return readOneZeroPreference(nightModeStorageKey, false);
    }

    function persistNightModePreference() {
      persistOneZeroPreference(nightModeStorageKey, nightModeEnabled);
    }

    function applyNightModeUi() {
      document.body.classList.toggle('night-mode', !!nightModeEnabled);
      const box = document.getElementById('nightMode');
      if (box) box.checked = !!nightModeEnabled;
      const overlayBox = document.getElementById('overlayNightMode');
      if (overlayBox) overlayBox.checked = !!nightModeEnabled;

      if (latestData && selectedTab === 'EFB' && normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') {
        render(latestData);
      }
    }

    function readContentFontSizePreference() {
      try {
        const raw = readStoredPreferenceValue(contentFontSizeStorageKey);
        const parsed = parseFloat(raw);
        if (!isFinite(parsed)) return 24;
        return clamp(parsed, 18, 34);
      } catch (_) {
        return 24;
      }
    }

    function readDlinkOnPreference() {
      return readZeroDisabledPreference(dlinkOnStorageKey, true);
    }

    function normalizeNavlogAltDisplayMode(value) {
      return String(value || '').toLowerCase() === 'm' ? 'm' : 'ft';
    }

    function normalizeNavlogSpdDisplayMode(value) {
      return String(value || '').toLowerCase() === 'kmh' ? 'kmh' : 'kts';
    }

    function normalizeNavlogDistDisplayMode(value) {
      const mode = String(value || '').toLowerCase();
      return (mode === 'km' || mode === 'm') ? 'km' : 'nm';
    }

    function readNavlogAltDisplayModePreference() {
      return readNormalizedPreference(navlogAltDisplayModeStorageKey, 'ft', normalizeNavlogAltDisplayMode);
    }

    function persistNavlogAltDisplayModePreference(mode) {
      persistNormalizedPreference(navlogAltDisplayModeStorageKey, mode, normalizeNavlogAltDisplayMode);
    }

    function readNavlogSpdDisplayModePreference() {
      return readNormalizedPreference(navlogSpdDisplayModeStorageKey, 'kts', normalizeNavlogSpdDisplayMode);
    }

    function persistNavlogSpdDisplayModePreference(mode) {
      persistNormalizedPreference(navlogSpdDisplayModeStorageKey, mode, normalizeNavlogSpdDisplayMode);
    }

    function readNavlogDistDisplayModePreference() {
      return readNormalizedPreference(navlogDistDisplayModeStorageKey, 'nm', normalizeNavlogDistDisplayMode);
    }

    function persistNavlogDistDisplayModePreference(mode) {
      persistNormalizedPreference(navlogDistDisplayModeStorageKey, mode, normalizeNavlogDistDisplayMode);
    }

    function persistDlinkOnPreference() {
      persistOneZeroPreference(dlinkOnStorageKey, dlinkOnEnabled);
    }

    function applyDlinkOnUi() {
      const box = document.getElementById('dlinkOn');
      if (box) box.checked = !!dlinkOnEnabled;
      const overlayBox = document.getElementById('overlayDlinkOn');
      if (overlayBox) overlayBox.checked = !!dlinkOnEnabled;
    }

    function applyAutoBrowseUi() {
      const box = document.getElementById('autoBrowse');
      if (box) box.checked = !!autoBrowse;
      const overlayBox = document.getElementById('overlayAutoBrowse');
      if (overlayBox) overlayBox.checked = !!autoBrowse;
    }

    function readAutoAtaRecSpdEnabledPreference() {
      return readOneZeroPreference(autoAtaRecSpdEnabledStorageKey, false);
    }

    function persistAutoAtaRecSpdEnabledPreference() {
      persistOneZeroPreference(autoAtaRecSpdEnabledStorageKey, autoAtaRecSpdEnabled);
    }

    function applyAutoAtaRecSpdUi() {
      const overlayBox = document.getElementById('overlayAutoAtaRecSpd');
      if (overlayBox) overlayBox.checked = !!autoAtaRecSpdEnabled;
    }

    function getAutoAtaRecDisplayState(selected) {
      const state = getFlightPlanPlanState(selected);
      if (!state) return { spdText: '', noteText: '', unable: false };

      if (state.speedRecommendations && typeof state.speedRecommendations === 'object') {
        const keys = Object.keys(state.speedRecommendations)
          .map(function (k) { return stepToKey(k); })
          .filter(function (k) { return !!k; })
          .sort(function (a, b) { return Number(a) - Number(b); });
        if (keys.length) {
          const rec = state.speedRecommendations[keys[0]] || {};
          const kcas = Math.round(Number(rec.kcas));
          if (isFinite(kcas) && kcas > 0) {
            return {
              spdText: kcas >= 1000 ? String(kcas) : String(kcas).padStart(3, '0'),
              noteText: '',
              unable: false,
            };
          }
        }
      }

      const fallbackCode = String(state.autoAtaLastFallbackCode || '').toUpperCase();
      if (fallbackCode) {
        const fallbackKcas = fallbackCode === 'HIGH' ? 700 : 220;
        return {
          spdText: String(fallbackKcas).padStart(3, '0'),
          noteText: fallbackCode === 'HIGH' ? 'UNABLE ACCEL' : 'UNABLE DELAY',
          unable: true,
        };
      }

      return { spdText: '', noteText: '', unable: false };
    }

    function triggerAutoAtaRecPulse(spdText, noteText, unableState) {
      const text = String(spdText || '').trim();
      if (!text || text === '---') return;
      autoAtaRecPulseSpdText = text;
      autoAtaRecPulseNoteText = String(noteText || '').trim();
      autoAtaRecPulseUnable = !!unableState;
      autoAtaRecPulseUntilUtc = Date.now() + autoAtaRecPulseDurationMs;
      applyAutoAtaRecPulseUi();
    }

    function applyAutoAtaRecPulseUi(nowUtcMs) {
      const pulseEl = document.getElementById('efbAutoAtaRecBalloon');
      const now = isFinite(Number(nowUtcMs)) ? Number(nowUtcMs) : Date.now();
      const hasSpd = !!String(autoAtaRecPulseSpdText || '').trim();
      const active = !!pulseEl && hasSpd && autoAtaRecPulseUntilUtc > now && selectedTab === 'EFB';
      if (pulseEl) {
        const speedText = hasSpd ? autoAtaRecPulseSpdText : '';
        const noteText = String(autoAtaRecPulseNoteText || '').trim();
        pulseEl.innerHTML = speedText
          ? ('<span class="efbAutoRecBalloonMain">' + escapeHtml(speedText) + '</span>'
            + (noteText ? ('<span class="efbAutoRecBalloonNote">' + escapeHtml(noteText) + '</span>') : ''))
          : '';
        pulseEl.classList.toggle('active', active);
        pulseEl.classList.toggle('unable', active && !!autoAtaRecPulseUnable);
      }
    }

    function markAutoAtaWaypointPulse(step) {
      const key = stepToKey(step);
      if (!key) return;
      autoAtaWaypointPulseUntilByKey[key] = Date.now() + autoAtaWaypointPulseDurationMs;
    }

    function isAutoAtaWaypointPulseActive(step, nowUtcMs) {
      const key = stepToKey(step);
      if (!key) return false;
      const until = Number(autoAtaWaypointPulseUntilByKey[key] || 0);
      const now = isFinite(Number(nowUtcMs)) ? Number(nowUtcMs) : Date.now();
      if (!isFinite(until) || until <= now) {
        delete autoAtaWaypointPulseUntilByKey[key];
        return false;
      }
      return true;
    }

    function captureAutoWaypointAta(selected, step, plannedEtaText, modeTag) {
      const state = getFlightPlanPlanState(selected);
      if (!state.ataByStep || typeof state.ataByStep !== 'object') state.ataByStep = {};
      if (!Array.isArray(state.timingLog)) state.timingLog = [];

      const key = stepToKey(step);
      if (!key || state.ataByStep[key]) return false;

      const planned = parseEtaToSeconds(plannedEtaText);
      const actual = getCurrentFlightPlanClockSeconds();
      if (!isFinite(planned) || !isFinite(actual)) return false;

      state.ataByStep[key] = {
        actualSeconds: actual,
        plannedSeconds: planned,
      };

      const deltaText = formatSignedDeltaSeconds(actual - planned);
      state.timingLog.push(new Date().toISOString() + ' | ATA AUTO STP' + key + ' ' + String(modeTag || 'CAP') + ' ' + deltaText);
      if (state.timingLog.length > 16) {
        state.timingLog = state.timingLog.slice(state.timingLog.length - 16);
      }

      buildSpeedRecommendationsForAta(selected, key);
      const pulseState = getAutoAtaRecDisplayState(selected);
      lockInSpeedRecommendations(selected);
      if (pulseState && pulseState.spdText) {
        triggerAutoAtaRecPulse(pulseState.spdText, pulseState.noteText, pulseState.unable);
      }
      markAutoAtaWaypointPulse(key);
      return true;
    }

    function updateAutoAtaRecSpdCapture(selected, rows) {
      if (!autoAtaRecSpdEnabled) return;
      const list = Array.isArray(rows) ? rows : [];
      if (!list.length) return;

      const state = getFlightPlanPlanState(selected);
      const activeRows = getEffectiveRouteRows(list, state)
        .filter(function (wp) { return !wp.isStart; })
        .sort(function (a, b) { return Number(a && a.step) - Number(b && b.step); });
      if (!activeRows.length) return;

      const own = getOwnshipNorthEast();
      if (!isFinite(own.north) || !isFinite(own.east)) return;

      const prevOwn = state.autoAtaLastOwnshipPos
        && isFinite(Number(state.autoAtaLastOwnshipPos.north))
        && isFinite(Number(state.autoAtaLastOwnshipPos.east))
        ? { north: Number(state.autoAtaLastOwnshipPos.north), east: Number(state.autoAtaLastOwnshipPos.east) }
        : null;
      state.autoAtaLastOwnshipPos = { north: own.north, east: own.east };

      const isRuntimeSelection = String(selected || '').trim() === '__RUNTIME_PLAYER__';
      const lastActiveKey = stepToKey(activeRows[activeRows.length - 1] && activeRows[activeRows.length - 1].step);
      const pendingRows = activeRows.filter(function (wp) {
        const k = stepToKey(wp && wp.step);
        if (!k) return false;
        if (k === lastActiveKey) return false;
        if (isRuntimeSelection) {
          const displayStep = Number(wp && wp.stepDisplay);
          if (isFinite(displayStep) && displayStep <= 0) return false;
        }
        const stepNum = Number(k);
        if (k === '0' || (isFinite(stepNum) && stepNum === 0)) return false;
        return !(state.ataByStep && state.ataByStep[k]);
      });
      if (!pendingRows.length) return;

      const releaseKey = stepToKey(state.autoAtaLastCapturedStep);
      if (releaseKey) {
        const lastWp = activeRows.find(function (wp) { return stepToKey(wp && wp.step) === releaseKey; }) || null;
        if (lastWp) {
          const lastPos = resolveWaypointNorthEast(lastWp);
          if (isFinite(lastPos.north) && isFinite(lastPos.east)) {
            const dn = own.north - lastPos.north;
            const de = own.east - lastPos.east;
            const releaseDistance = Math.sqrt((dn * dn) + (de * de));
            if (isFinite(releaseDistance) && releaseDistance < autoAtaCaptureReleaseMeters) {
              return;
            }
          }
        }
      }

      let captured = false;
      for (let i = 0; i < pendingRows.length; i++) {
        const candidate = pendingRows[i];
        const candidateKey = stepToKey(candidate && candidate.step);
        if (!candidateKey) continue;

        const wpPos = resolveWaypointNorthEast(candidate);
        if (!isFinite(wpPos.north) || !isFinite(wpPos.east)) continue;

        const dNorth = own.north - wpPos.north;
        const dEast = own.east - wpPos.east;
        const distanceMeters = Math.sqrt((dNorth * dNorth) + (dEast * dEast));
        if (!isFinite(distanceMeters)) continue;

        let crossedWithinThreshold = distanceMeters <= autoAtaCaptureThresholdMeters;
        if (!crossedWithinThreshold && prevOwn) {
          const segDistance = distancePointToSegmentMeters(
            wpPos.north,
            wpPos.east,
            prevOwn.north,
            prevOwn.east,
            own.north,
            own.east
          );
          crossedWithinThreshold = isFinite(segDistance) && segDistance <= autoAtaCaptureThresholdMeters;
        }
        if (!crossedWithinThreshold) continue;

        const etaText = String(candidate.etaDisplay || candidate.eta || '').trim();
        if (!etaText) continue;

        if (captureAutoWaypointAta(selected, candidateKey, etaText, '3/4NM')) {
          state.autoAtaLastCapturedStep = candidateKey;
          captured = true;
          break;
        }
      }

      if (!captured) return;
    }

    function setOverlayAnimated(overlay, open, baseClass) {
      if (!overlay) return;
      const base = String(baseClass || 'overlayBackdrop');
      if (open) {
        if (overlay._closeTimer) {
          clearTimeout(overlay._closeTimer);
          overlay._closeTimer = null;
        }
        overlay.className = base;
        void overlay.offsetWidth;
        overlay.className = base + ' overlayOpen';
        return;
      }

      overlay.className = base;
      if (overlay._closeTimer) {
        clearTimeout(overlay._closeTimer);
      }
      overlay._closeTimer = setTimeout(function () {
        overlay.className = base + ' hidden';
        overlay._closeTimer = null;
      }, overlayCloseHideDelayMs);
    }

    function applyDrawOverlayUi() {
      const drawBox = document.getElementById('overlayDrawMode');
      if (drawBox) {
        drawBox.checked = !!drawModeEnabled;
        drawBox.disabled = !!okbDoodlesOnlyForced || selectedTab !== 'NOTES';
      }

      const timerEl = document.getElementById('overlayDrawTimer');
      if (timerEl) {
        if (drawModeEnabled && drawModeDeadlineUtcMs > 0) {
          const remainingMs = Math.max(0, drawModeDeadlineUtcMs - Date.now());
          const remainingSeconds = Math.ceil(remainingMs / 1000);
          timerEl.textContent = String(remainingSeconds) + 's';
          timerEl.className = 'overlayHint';
        } else {
          timerEl.textContent = '30s';
          timerEl.className = 'overlayHint hidden';
        }
      }
    }

    function applyShowRawUi(show) {
      const value = !!show;
      const box = document.getElementById('showRaw');
      if (box) box.checked = value;
      const overlayBox = document.getElementById('overlayShowRaw');
      if (overlayBox) overlayBox.checked = value;
      document.body.classList.toggle('raw-mode', value);
      const jsonEl = document.getElementById('json');
      if (jsonEl) jsonEl.className = value ? '' : 'hidden';
    }

    function applyShowServerUi(show) {
      const value = !!show;
      showServerMessages = value;
      const box = document.getElementById('showServer');
      if (box) box.checked = value;
      const overlayBox = document.getElementById('overlayShowServer');
      if (overlayBox) overlayBox.checked = value;
      const serverMessagesEl = document.getElementById('serverMessages');
      if (serverMessagesEl) serverMessagesEl.className = value ? '' : 'hidden';
      setServerMessageCapture(value);
    }

    function setSettingsOverlayOpen(open) {
      settingsOverlayOpen = !!open;
      if (settingsOverlayOpen) {
        setDrawOverlayOpen(false);
        setHelpOverlayOpen(false);
      }
      const overlay = document.getElementById('settingsOverlay');
      if (!overlay) return;
      setOverlayAnimated(overlay, settingsOverlayOpen, 'overlayBackdrop');
      if (settingsOverlayOpen) {
        applyAutoBrowseUi();
        applyNightModeUi();
        applyDlinkOnUi();
        applyEfbAdLandingUi();
        applyAutoAtaRecSpdUi();
        applyContentFontSizeUi();
        const showRawBox = document.getElementById('showRaw');
        const showServerBox = document.getElementById('showServer');
        applyShowRawUi(showRawBox ? !!showRawBox.checked : false);
        applyShowServerUi(showServerBox ? !!showServerBox.checked : false);
      }

      const debugMode = !!(latestData && latestData.Server && latestData.Server.DebugMode);
      const rawWrap = document.getElementById('overlayShowRawWrap');
      const serverWrap = document.getElementById('overlayShowServerWrap');
      if (rawWrap) rawWrap.style.display = debugMode ? 'flex' : 'none';
      if (serverWrap) serverWrap.style.display = debugMode ? 'flex' : 'none';
      if (!debugMode) {
        applyShowRawUi(false);
        applyShowServerUi(false);
      }
    }

    function setDrawOverlayOpen(open) {
      drawOverlayOpen = !!open;
      if (drawOverlayOpen) {
        setSettingsOverlayOpen(false);
        setHelpOverlayOpen(false);
      }
      const overlay = document.getElementById('drawOverlay');
      if (!overlay) return;
      setOverlayAnimated(overlay, drawOverlayOpen, 'overlayBackdrop');
      if (drawOverlayOpen) {
        applyDrawOverlayUi();
      }
    }

    function setHelpOverlayOpen(open) {
      helpOverlayOpen = !!open;
      if (helpOverlayOpen) {
        setSettingsOverlayOpen(false);
        setDrawOverlayOpen(false);
      }
      const overlay = document.getElementById('helpOverlay');
      if (!overlay) return;
      setOverlayAnimated(overlay, helpOverlayOpen, 'overlayBackdrop helpOverlayBackdrop');
      if (helpOverlayOpen) {
        const titleEl = document.getElementById('helpOverlayTitle');
        if (titleEl) {
          titleEl.textContent = tabLabel(selectedTab) + ' Help';
        }
        loadHelpOverlayContent(selectedTab);
      } else {
        helpOverlayLoadedTab = '';
      }
    }

    async function loadHelpOverlayContent(tab) {
      const contentEl = document.getElementById('helpOverlayContent');
      if (!contentEl) return;
      const tabToken = String(tab || selectedTab || 'LOG');
      contentEl.innerHTML = '<p>Loading help content...</p>';
      try {
        const res = await fetch('/okb/helpdoc?tab=' + encodeURIComponent(tabToken), { cache: 'no-store' });
        if (!res.ok) throw new Error('HTTP ' + res.status);
        const html = await res.text();
        contentEl.innerHTML = html || '<p>No help content available for this tab.</p>';
        helpOverlayLoadedTab = tabToken;
      } catch (ex) {
        const msg = ex && ex.message ? String(ex.message) : 'Unknown error';
        contentEl.innerHTML = '<p>Failed to load help content.</p><p>' + escapeHtml(msg) + '</p>';
      }
    }

    function setFltPlanFilesOverlayOpen(open) {
      fltPlanFilesOverlayOpen = !!open;
      if (fltPlanFilesOverlayOpen) {
        fltPlanSessionOverlayOpen = false;
        const sessionOverlay = document.getElementById('fltPlanSessionOverlay');
        if (sessionOverlay) setOverlayAnimated(sessionOverlay, false, 'overlayBackdrop overlayBackdropTop');
      }

      const overlay = document.getElementById('fltPlanFilesOverlay');
      if (!overlay) return;
      setOverlayAnimated(overlay, fltPlanFilesOverlayOpen, 'overlayBackdrop overlayBackdropTop');
    }

    function setFltPlanSessionOverlayOpen(open) {
      fltPlanSessionOverlayOpen = !!open;
      if (fltPlanSessionOverlayOpen) {
        fltPlanFilesOverlayOpen = false;
        const filesOverlay = document.getElementById('fltPlanFilesOverlay');
        if (filesOverlay) setOverlayAnimated(filesOverlay, false, 'overlayBackdrop overlayBackdropTop');
      }

      const overlay = document.getElementById('fltPlanSessionOverlay');
      if (!overlay) return;
      setOverlayAnimated(overlay, fltPlanSessionOverlayOpen, 'overlayBackdrop overlayBackdropTop');
    }

    function renderFltPlanFilesOverlay(data) {
      const selectedLabel = document.getElementById('fltPlanOverlaySelectedLabel');
      const missionClock = document.getElementById('fltPlanOverlayMissionClock');
      const listEl = document.getElementById('fltPlanOverlayFileList');
      if (!selectedLabel || !missionClock || !listEl) return;

      const files = Array.isArray(data && data.DtcFiles) ? data.DtcFiles : [];
      const selected = String((data && data.DtcSelectedFile) || '');
      selectedLabel.textContent = selected ? ('Selected: ' + getDtcDisplayName(selected)) : 'No FLT PLN selected';

      const missionClockLabel = document.getElementById('missionClockLabel');
      missionClock.textContent = missionClockLabel && missionClockLabel.textContent
        ? missionClockLabel.textContent
        : 'Mission Time - (- UTC)';

      if (!files.length) {
        listEl.innerHTML = '<div class="overlayHint">No FLT PLN files found.</div>';
        return;
      }

      const rows = files.map(function (filePath) {
        const fp = String(filePath || '');
        const active = selected && fp === selected;
        const type = (fp.indexOf('RTE::') === 0 || /\.(rte|lua)$/i.test(fp)) ? 'RTE' : 'DTC';
        return '<button type="button" class="dtcFileItem' + (active ? ' active' : '') + '" data-overlay-file="' + encodeURIComponent(fp) + '"><span>' + escapeHtml(getDtcDisplayName(fp)) + '</span><span class="dtcFileMeta">' + type + '</span></button>';
      });
      listEl.innerHTML = rows.join('');
    }

    function hasAwacsAvailable(data) {
      const units = getMergedList(data && data.Units, 'AWACS');
      const hasUnitLine = Array.isArray(units) && units.some(function (line) {
        return String(line || '').trim().length > 0;
      });
      if (hasUnitLine) return true;

      const server = (data && data.Server) || {};
      const assets = Array.isArray(server.FriendlyAssets) ? server.FriendlyAssets : [];
      return assets.some(function (a) {
        return String((a && a.Category) || '').toUpperCase() === 'AWACS';
      });
    }

    function persistContentFontSizePreference() {
      writeStoredPreferenceValue(contentFontSizeStorageKey, String(contentFontSizePx));
    }

    function applyContentFontSizeUi() {
      const safeSize = clamp(contentFontSizePx, 18, 34);
      contentFontSizePx = safeSize;
      document.body.style.setProperty('--contentFontSize', String(safeSize) + 'px');
      const slider = document.getElementById('fontSizeSlider');
      if (slider) slider.value = String(safeSize);
      const value = document.getElementById('fontSizeValue');
      if (value) value.textContent = String(safeSize);
      const overlaySlider = document.getElementById('overlayFontSizeSlider');
      if (overlaySlider) overlaySlider.value = String(safeSize);
      const overlayValue = document.getElementById('overlayFontSizeValue');
      if (overlayValue) overlayValue.textContent = String(safeSize);
    }

    function updateFakeMissionControlsUi() {
      const wrap = document.getElementById('fakeMissionControls');
      const info = document.getElementById('fakeMissionInfo');
      const clock = document.getElementById('fakeMissionClock');
      const rate = document.getElementById('fakeMissionRate');
      const slower = document.getElementById('fakeMissionSlower');
      const faster = document.getElementById('fakeMissionFaster');
      const stop = document.getElementById('fakeMissionStop');
      if (!wrap || !info || !clock || !rate || !slower || !faster || !stop) return;

      wrap.className = fakeMissionEnabled ? 'simControls' : 'simControls hidden';
      info.textContent = fakeMissionEnabled ? 'ACTIVE' : 'OFF';
      clock.textContent = (fakeMissionEnabled && fakeMissionState)
        ? formatSecondsToClock(Number(fakeMissionState.simMissionSeconds || 0))
        : '--:--:--';
      rate.textContent = 'x' + Number(fakeMissionSpeed).toFixed(2);
      slower.disabled = !fakeMissionEnabled || fakeMissionSpeed <= fakeMissionSpeedMin + 0.0001;
      faster.disabled = !fakeMissionEnabled || fakeMissionSpeed >= fakeMissionSpeedMax - 0.0001;
      stop.disabled = !fakeMissionEnabled;
    }

    function stopFakeMissionTimer() {
      if (!fakeMissionTimer) return;
      clearInterval(fakeMissionTimer);
      fakeMissionTimer = null;
    }

    function randomInRange(min, max) {
      return min + (Math.random() * (max - min));
    }

    function makeFakeAsset(callsign, category, x, y, headingDeg, speedMps, extras) {
      const meta = (extras && typeof extras === 'object') ? extras : {};
      return {
        Callsign: callsign,
        Name: callsign,
        Category: category,
        RawLine: '',
        X: x,
        Y: y,
        headingDeg: headingDeg,
        speedMps: speedMps,
        TypeName: String(meta.TypeName || ''),
        Frequency: String(meta.Frequency || ''),
        AltFrequencies: Array.isArray(meta.AltFrequencies) ? meta.AltFrequencies.slice(0) : [],
        Tacan: String(meta.Tacan || ''),
        MpClientCallsign: String(meta.MpClientCallsign || ''),
        AltFeet: isFinite(Number(meta.AltFeet)) ? Number(meta.AltFeet) : NaN,
      };
    }

    function createFakeMissionStateFromData(data) {
      const source = data || latestData || {};
      const server = (source && source.Server) || {};
      const serverPlayerCallsign = String(server.PlayerCallsign || '').trim();
      const serverPlayerUsername = String(server.PlayerUsername || '').trim();
      const isMultiplayerSession = !!server.Multiplayer;
      const simOwnshipCallsign = isMultiplayerSession
        ? (serverPlayerUsername || serverPlayerCallsign || 'OWNSHIP')
        : (serverPlayerCallsign || serverPlayerUsername || 'OWNSHIP');
      const selected = getActiveFlightPlanSelection(source);
      const routeRows = getPlanWaypointsForRecommendations(selected)
        .filter(function (wp) { return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum)); });

      const simStartOffsetMeters = 5 * 1852;

      const routeStartX = routeRows.length ? Number(routeRows[0].xNum) : NaN;
      const routeStartY = routeRows.length ? Number(routeRows[0].yNum) : NaN;
      let baseX = isFinite(routeStartX)
        ? routeStartX
        : (isFinite(Number(server.PlayerPosX)) ? Number(server.PlayerPosX) : 170000);
      let baseY = isFinite(routeStartY)
        ? routeStartY
        : (isFinite(Number(server.PlayerPosY)) ? Number(server.PlayerPosY) : 105000);

      if (routeRows.length >= 2) {
        const wp1 = routeRows[0];
        const wp2 = routeRows[1];
        const x1 = Number(wp1.xNum);
        const y1 = Number(wp1.yNum);
        const x2 = Number(wp2.xNum);
        const y2 = Number(wp2.yNum);
        const dx = x2 - x1;
        const dy = y2 - y1;
        const mag = Math.sqrt((dx * dx) + (dy * dy));
        if (isFinite(mag) && mag > 0.01) {
          const ux = dx / mag;
          const uy = dy / mag;
          baseX = x1 - (ux * simStartOffsetMeters);
          baseY = y1 - (uy * simStartOffsetMeters);
        }
      }
      const startMission = isFinite(Number(server.MissionTimeSeconds)) ? Number(server.MissionTimeSeconds) : 8 * 3600;

      const routeRowsForMotion = routeRows.length
        ? ([{
          step: '0',
          xNum: baseX,
          yNum: baseY,
          altFeet: routeRows.length ? Number(routeRows[0].altFeet || 0) : Number(server.PlayerAltFeet || 0)
        }]).concat(routeRows)
        : routeRows;

      return {
        routeSelectionKey: String(selected || '').trim(),
        startMissionSeconds: startMission,
        simMissionSeconds: startMission,
        playerX: baseX,
        playerY: baseY,
        playerAltFeet: isFinite(Number(server.PlayerAltFeet)) ? Number(server.PlayerAltFeet) : 15000,
        ownshipStarted: false,
        routeRows: routeRowsForMotion,
        playerRouteIndex: 0,
        playerRouteProgressMeters: 0,
        playerSpeedMps: 185,
        assets: [
          makeFakeAsset(simOwnshipCallsign, 'PLAYER', baseX, baseY, 0, 0, { MpClientCallsign: serverPlayerUsername }),
          makeFakeAsset('TEXACO11', 'TANKER', baseX + 30000, baseY + 22000, 235, 210, { TypeName: 'KC-135 MPRS', Frequency: '251.000', Tacan: '31Y', AltFeet: 22000 }),
          makeFakeAsset('OVERLORD1', 'AWACS', baseX - 45000, baseY + 26000, 95, 205, { TypeName: 'E-3A', Frequency: '305.000', AltFeet: 28000 }),
          makeFakeAsset('AXEMAN11', 'JTAC', baseX + 14000, baseY - 22000, 330, 0),
          makeFakeAsset('VIPER12', 'FLIGHT', baseX - 18000, baseY - 12000, 25, 230),
          makeFakeAsset('COLT21', 'FLIGHT', baseX + 8000, baseY + 16000, 290, 220),
        ],
      };
    }

    function getFakeMissionRouteSelectionKey() {
      const active = String(getActiveFlightPlanSelection(latestData) || '').trim();
      if (active) return active;
      return fakeMissionState ? String(fakeMissionState.routeSelectionKey || '').trim() : '';
    }

    function updateFakeOwnshipStartedState() {
      if (!fakeMissionEnabled || !fakeMissionState) return;
      const selected = getFakeMissionRouteSelectionKey();
      if (!selected) {
        const rows = Array.isArray(fakeMissionState.routeRows) ? fakeMissionState.routeRows : [];
        fakeMissionState.ownshipStarted = rows.length >= 2;
        return;
      }

      const timing = getResolvedTimingMarks(selected);
      const takeoff = Number(timing && timing.takeoff);
      if (!isFinite(takeoff)) {
        fakeMissionState.ownshipStarted = true;
        return;
      }

      fakeMissionState.ownshipStarted = Number(fakeMissionState.simMissionSeconds) >= takeoff;
    }

    function syncFakeMissionRouteRowsFromPlan() {
      if (!fakeMissionEnabled || !fakeMissionState || !latestData) return;
      const selected = getFakeMissionRouteSelectionKey();
      if (!selected) return;

      const plannedRows = getPlanWaypointsForRecommendations(selected);
      if (!Array.isArray(plannedRows) || !plannedRows.length) return;

      const byStep = {};
      plannedRows.forEach(function (wp) {
        const key = stepToKey(wp && wp.step);
        if (!key) return;
        byStep[key] = wp;
      });

      const rows = Array.isArray(fakeMissionState.routeRows) ? fakeMissionState.routeRows : [];
      for (let i = 1; i < rows.length; i++) {
        const row = rows[i];
        const key = stepToKey(row && row.step);
        if (!key || !byStep[key]) continue;
        const src = byStep[key];
        row.spd = src.spd;
        row.altFeet = src.altFeet;
        row.xNum = src.xNum;
        row.yNum = src.yNum;
      }
    }

    function advanceFakeOwnshipAlongRoute(state, dt) {
      if (!state) return;
      const rows = Array.isArray(state.routeRows) ? state.routeRows : [];
      if (rows.length < 2) return;

      function legSpeedMpsFor(toWp) {
        const cas = Number(toWp && toWp.spd);
        const altFeet = isFinite(Number(toWp && toWp.altFeet)) ? Number(toWp.altFeet) : 0;
        const gsKnots = isFinite(cas) && cas > 0 ? (cas * (1.0 + (Math.max(0, altFeet) / 100000.0))) : NaN;
        const mps = isFinite(gsKnots) && gsKnots > 0 ? (gsKnots * 0.514444) : NaN;
        if (isFinite(mps) && mps > 0) return mps;
        return Math.max(0, Number(state.playerSpeedMps) || 185);
      }

      let remainingSeconds = Math.max(0, Number(dt) || 0);
      if (remainingSeconds <= 0) return;

      while (remainingSeconds > 0) {
        let i = Number(state.playerRouteIndex);
        if (!isFinite(i) || i < 0) i = 0;
        if (i >= rows.length - 1) i = rows.length - 2;

        const a = rows[i];
        const b = rows[i + 1];
        const ax = Number(a && a.xNum);
        const ay = Number(a && a.yNum);
        const bx = Number(b && b.xNum);
        const by = Number(b && b.yNum);
        if (!isFinite(ax) || !isFinite(ay) || !isFinite(bx) || !isFinite(by)) break;

        const dx = bx - ax;
        const dy = by - ay;
        const legMeters = Math.sqrt((dx * dx) + (dy * dy));
        if (!isFinite(legMeters) || legMeters < 1) {
          state.playerRouteIndex = i + 1;
          state.playerRouteProgressMeters = 0;
          continue;
        }

        let progress = Number(state.playerRouteProgressMeters);
        if (!isFinite(progress) || progress < 0) progress = 0;

        const leftOnLeg = Math.max(0, legMeters - progress);
        const speedMps = legSpeedMpsFor(b);
        if (!isFinite(speedMps) || speedMps <= 0) break;
        const leftSeconds = leftOnLeg / speedMps;

        if (remainingSeconds < leftSeconds) {
          progress += (remainingSeconds * speedMps);
          remainingSeconds = 0;
          state.playerRouteProgressMeters = progress;
        } else {
          remainingSeconds -= leftSeconds;
          if (i + 1 >= rows.length - 1) {
            state.playerRouteIndex = 0;
            state.playerRouteProgressMeters = 0;
          } else {
            state.playerRouteIndex = i + 1;
            state.playerRouteProgressMeters = 0;
          }
          continue;
        }

        const t = legMeters > 0 ? (progress / legMeters) : 0;
        state.playerX = ax + (dx * t);
        state.playerY = ay + (dy * t);

        const playerAsset = (state.assets || []).find(function (a) { return String((a && a.Category) || '').toUpperCase() === 'PLAYER'; });
        if (playerAsset) {
          playerAsset.X = state.playerX;
          playerAsset.Y = state.playerY;
          const hdg = normalizeHeadingDeg((Math.atan2(dy, dx) * 180.0 / Math.PI));
          playerAsset.headingDeg = isFinite(hdg) ? hdg : 0;
        }

        break;
      }
    }

    function stepFakeMissionState(deltaRealSeconds) {
      if (!fakeMissionEnabled || !fakeMissionState) return;
      const dt = Math.max(0, Number(deltaRealSeconds) || 0) * fakeMissionSpeed;
      if (dt <= 0) return;

      fakeMissionState.simMissionSeconds += dt;
      syncFakeMissionRouteRowsFromPlan();
      updateFakeOwnshipStartedState();
      if (fakeMissionState.ownshipStarted) {
        advanceFakeOwnshipAlongRoute(fakeMissionState, dt);
      }
      const bounds = 70000;
      const baseX = fakeMissionState.playerX;
      const baseY = fakeMissionState.playerY;

      fakeMissionState.assets.forEach(function (asset, idx) {
        if (!asset || String(asset.Category).toUpperCase() === 'PLAYER') return;

        const speed = Math.max(0, Number(asset.speedMps) || 0);
        let heading = Number(asset.headingDeg);
        if (!isFinite(heading)) heading = randomInRange(0, 360);

        heading += randomInRange(-4.5, 4.5);
        heading = ((heading % 360) + 360) % 360;
        asset.headingDeg = heading;

        const rad = heading * Math.PI / 180.0;
        const dx = Math.sin(rad) * speed * dt;
        const dy = Math.cos(rad) * speed * dt;
        asset.X = Number(asset.X || baseX) + dx;
        asset.Y = Number(asset.Y || baseY) + dy;

        const offX = asset.X - baseX;
        const offY = asset.Y - baseY;
        if (Math.abs(offX) > bounds || Math.abs(offY) > bounds) {
          asset.headingDeg = ((heading + 180 + randomInRange(-20, 20)) % 360 + 360) % 360;
        }

        if (idx % 2 === 0 && Math.random() < 0.03) {
          asset.speedMps = clamp(speed + randomInRange(-8, 8), 120, 280);
        }
      });
    }

    function getDisplayData(data) {
      const original = data || latestData || {};
      if (!fakeMissionEnabled || !fakeMissionState) return original;

      let clone;
      try {
        clone = JSON.parse(JSON.stringify(original || {}));
      } catch (_) {
        clone = {};
      }
      if (!clone.Server || typeof clone.Server !== 'object') clone.Server = {};
      if (!clone.Status || typeof clone.Status !== 'object') clone.Status = {};
      const server = clone.Server;
      server.MissionTimeSeconds = Number(fakeMissionState.simMissionSeconds);
      server.PlayerPosX = Number(fakeMissionState.playerX);
      server.PlayerPosY = Number(fakeMissionState.playerY);
      server.PlayerAltFeet = Number(fakeMissionState.playerAltFeet);
      clone.UpdatedUtc = new Date().toISOString();
      clone.Status.Text = 'SIM TEST MODE ACTIVE';
      clone.Status.Level = 'warning';
      clone.Status.UpdatedUtc = clone.UpdatedUtc;
      server.Diagnostics = server.Diagnostics || {};
      if (!Array.isArray(server.Diagnostics.playerGroupWaypoints)) {
        server.Diagnostics.playerGroupWaypoints = [];
      }
      server.Diagnostics.playerGroup = String(server.Diagnostics.playerGroup || 'SIM-FLIGHT');

      const fakeSelectedFile = String(clone.DtcSelectedFile || original.DtcSelectedFile || '').trim();
      const fakeKnownFiles = Array.isArray(clone.DtcFiles) ? clone.DtcFiles : (Array.isArray(original.DtcFiles) ? original.DtcFiles : []);
      let fakeRouteSeedSelection = fakeSelectedFile;
      if (!fakeRouteSeedSelection && fakeKnownFiles.length) {
        fakeRouteSeedSelection = String(fakeKnownFiles[0] || '').trim();
      }
      if (!fakeRouteSeedSelection) {
        fakeRouteSeedSelection = String(getActiveFlightPlanSelection(original) || '').trim();
      }

      if (!String(server.Theater || '').trim() && fakeRouteSeedSelection) {
        const inferredTheatre = inferTheatreCandidateFromSelection(fakeRouteSeedSelection);
        if (inferredTheatre && getMapProjectionByTheatre(inferredTheatre)) {
          server.Theater = inferredTheatre;
        }
      }

      if (!Array.isArray(server.Diagnostics.playerGroupWaypoints) || !server.Diagnostics.playerGroupWaypoints.length) {
        const routeRows = getPlanWaypointsForRecommendations(fakeRouteSeedSelection).filter(function (wp) {
          return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum));
        });
        server.Diagnostics.playerGroupWaypoints = routeRows.map(function (wp, i) {
          return 'RT|group=SIM-FLIGHT|pt=' + String(i + 1)
            + '|x=' + String(Math.round(Number(wp.xNum)))
            + '|y=' + String(Math.round(Number(wp.yNum)))
            + '|alt=' + String(Math.round(((Number(wp.altFeet) || 10000) / 3.28084)))
            + '|spd=180|eta=' + String(parseEtaToSeconds(wp.etaDisplay || wp.eta) || 0)
            + '|task=' + encodeURIComponent(String(wp.typeRaw || wp.type || 'WP'));
        });
      }

      server.FriendlyAssets = fakeMissionState.assets.map(function (a) {
        return {
          Callsign: String(a.Callsign || ''),
          Name: String(a.Name || ''),
          Category: String(a.Category || ''),
          RawLine: String(a.RawLine || ''),
          X: Number(a.X || 0),
          Y: Number(a.Y || 0),
          TypeName: String(a.TypeName || ''),
          Frequency: String(a.Frequency || ''),
          AltFrequencies: Array.isArray(a.AltFrequencies) ? a.AltFrequencies.slice(0) : [],
          Tacan: String(a.Tacan || ''),
          MpClientCallsign: String(a.MpClientCallsign || ''),
          AltFeet: isFinite(Number(a.AltFeet)) ? Number(a.AltFeet) : 0,
        };
      });
      return clone;
    }

    function startFakeMissionMode() {
      fakeMissionEnabled = true;
      fakeMissionSpeed = 1.0;
      fakeMissionState = createFakeMissionStateFromData(latestData);
      clearTakeoffTimeForSelection('__RUNTIME_PLAYER__');
      const selected = getActiveFlightPlanSelection(latestData);
      if (selected && fakeMissionState) {
        const state = getFlightPlanPlanState(selected);
        state.lockedStart = {
          x: Number(fakeMissionState.playerX),
          y: Number(fakeMissionState.playerY),
          altFeet: Number(fakeMissionState.playerAltFeet),
        };
      }
      dlinkOnEnabled = true;
      applyDlinkOnUi();
      fakeMissionLastRealMs = Date.now();
      fakeMissionLastRenderMs = 0;
      stopFakeMissionTimer();
      fakeMissionTimer = setInterval(function () {
        const now = Date.now();
        const delta = (now - fakeMissionLastRealMs) / 1000.0;
        fakeMissionLastRealMs = now;
        stepFakeMissionState(delta);
        if (latestData && (selectedTab === 'DTC' || selectedTab === 'EFB')) {
          const shouldRender = (now - fakeMissionLastRenderMs) >= 350;
          if (shouldRender) {
            fakeMissionLastRenderMs = now;
            render(latestData);
          } else {
            updateFakeMissionControlsUi();
            updateMissionClockLabel(latestData);
          }
        } else {
          updateFakeMissionControlsUi();
          updateMissionClockLabel(latestData);
        }
      }, 120);
      updateFakeMissionControlsUi();
      if (latestData) render(latestData);
    }

    function stopFakeMissionMode() {
      fakeMissionEnabled = false;
      fakeMissionState = null;
      fakeMissionSpeed = 1.0;
      clearTakeoffTimeForSelection('__RUNTIME_PLAYER__');
      stopFakeMissionTimer();
      updateFakeMissionControlsUi();
      if (latestData) render(latestData);
    }

    function adjustFakeMissionSpeed(factor) {
      if (!fakeMissionEnabled) return;
      const f = Number(factor);
      if (!isFinite(f) || f <= 0) return;
      fakeMissionSpeed = clamp(fakeMissionSpeed * f, fakeMissionSpeedMin, fakeMissionSpeedMax);
      updateFakeMissionControlsUi();
    }

    function mergeUnique(dest, src) {
      (src || []).forEach(function (v) {
        if (dest.indexOf(v) < 0) dest.push(v);
      });
    }

    function tabCssClass(tab) {
      return 'tab-' + String(tab || '').replace(/[^A-Za-z0-9]+/g, '_');
    }

    function tabLabel(tab) {
      if (tab === 'ATC') return 'WX/ATC';
      if (tab === 'DTC') return 'FLT PLN';
      if (tab === 'EFB') return 'EFB';
      return tab;
    }

    function formatAiCrewPhaseLabel(phase) {
      const text = String(phase || '').trim();
      if (!text || text.toLowerCase() === 'unknown') return '';
      return ' (' + text + ')';
    }

    async function updateCursorModeForTab() {
      try {
        const okb = (typeof OpenKneeboard !== 'undefined') ? OpenKneeboard : window.OpenKneeboard;
        if (!okb) return;

        const isNotes = selectedTab === 'NOTES';
        if (okbDoodlesOnlyForced && !isNotes) {
          return;
        }
        if (okbDoodlesOnlyForced && isNotes) {
          okbCursorMode = 'DoodlesOnly';
          return;
        }
        const targetMode = (isNotes && drawModeEnabled && drawInteractionInNotes) ? 'DoodlesOnly' : 'MouseEmulation';
        if (okbCursorMode === targetMode) return;

        if (!okbExperimentalEnabled && okb.EnableExperimentalFeatures) {
          await okb.EnableExperimentalFeatures([
            { name: 'DoodlesOnly', version: 2024071802 },
            { name: 'SetCursorEventsMode', version: 2024071801 },
          ]);
          okbExperimentalEnabled = true;
        }

        if (!okb.SetCursorEventsMode) return;

        if (targetMode === 'DoodlesOnly') {
          await okb.SetCursorEventsMode('DoodlesOnly');
          okbCursorMode = 'DoodlesOnly';
          return;
        }

        const restoreModes = ['MouseEmulation', 'Mouse', 'Normal', 'Default'];
        for (let i = 0; i < restoreModes.length; i++) {
          const mode = restoreModes[i];
          try {
            await okb.SetCursorEventsMode(mode);
            okbCursorMode = mode;
            return;
          } catch (_) {
          }
        }
      } catch (_) {
      }
    }

    function setStatus(text, level) {
      const statusEl = document.getElementById('status');
      const statusTextEl = document.getElementById('statusText');
      if (statusTextEl) statusTextEl.textContent = text;
      if (!statusEl) return;
      statusEl.classList.remove('status-error', 'status-warning', 'status-sent');
      if (level === 'error') statusEl.classList.add('status-error');
      if (level === 'warning') statusEl.classList.add('status-warning');
      if (level === 'sent') statusEl.classList.add('status-sent');
    }

    function applySessionCollapsedState(collapsed) {
      sessionCollapsed = !!collapsed;
      document.getElementById('session').style.display = sessionCollapsed ? 'none' : 'block';
      document.getElementById('sessionHeader').textContent = sessionCollapsed ? 'Session ► (click to expand)' : 'Session ▼';
    }

    function readInitialSessionCollapsed() {
      return readOneZeroPreference(sessionCollapsedStorageKey, false);
    }

    function persistSessionCollapsedState() {
      persistOneZeroPreference(sessionCollapsedStorageKey, sessionCollapsed);
    }

    function readDrawModePreference() {
      return readOneZeroPreference(drawModeStorageKey, false);
    }

    function readLiveRefreshPreference() {
      return readOneZeroPreference(liveRefreshStorageKey, false);
    }

    function persistLiveRefreshPreference() {
      persistOneZeroPreference(liveRefreshStorageKey, liveRefreshEnabled);
    }

    function updateLiveRefreshUi() {
      const meta = document.getElementById('liveExportMeta');
      if (!meta) return;
      const base = 'Live export from VAICOM';
      meta.classList.toggle('liveRefreshOn', !!liveRefreshEnabled);
      meta.textContent = liveRefreshEnabled ? (base + ' • Auto refresh ON') : base;
      meta.title = liveRefreshEnabled
        ? 'Hidden dev tool active: auto-refreshing dashboard data every 30 seconds (click to disable)'
        : 'Hidden dev tool: click to toggle dashboard auto-refresh (30s)';
    }

    async function requestLiveDashboardRefresh() {
      try {
        await fetch('dev/refresh', { method: 'POST', cache: 'no-store' });
      } catch (_) {
      }
    }

    function stopLiveRefreshTimer() {
      if (!liveRefreshTimer) return;
      clearInterval(liveRefreshTimer);
      liveRefreshTimer = null;
    }

    function applyLiveRefreshState(enabled) {
      liveRefreshEnabled = !!enabled;
      stopLiveRefreshTimer();

      if (liveRefreshEnabled) {
        requestLiveDashboardRefresh();
        liveRefreshTimer = setInterval(function () {
          requestLiveDashboardRefresh();
        }, 30000);
      }

      persistLiveRefreshPreference();
      updateLiveRefreshUi();
    }

    function persistDrawModePreference() {
      persistOneZeroPreference(drawModeStorageKey, drawModeEnabled);
    }

    function updateDrawModeToggleUi() {
      const button = document.getElementById('drawModeToggle');
      if (!button) return;
      button.classList.toggle('draw-on', drawModeEnabled);

      if (okbDoodlesOnlyForced) {
        button.disabled = true;
        button.textContent = 'Draw FORCED';
        button.title = 'Draw mode forced by URL parameter';
        return;
      }

      const notesActive = selectedTab === 'NOTES';
      button.disabled = !notesActive;
      button.textContent = drawModeEnabled ? 'Draw ON' : 'Draw OFF';
      button.title = notesActive
        ? 'Toggle Notes drawing mode'
        : 'Switch to NOTES tab to toggle drawing';
      applyDrawOverlayUi();
    }

    function setDrawInteractionInNotes(active) {
      const next = !!active;
      if (drawInteractionInNotes === next) return;
      drawInteractionInNotes = next;
      updateCursorModeForTab();

      if (drawInteractionInNotes) {
        scheduleDrawModeAutoOff();
      }
    }

    function clearDrawModeDisableTimer() {
      if (drawModeDisableTimer) {
        clearTimeout(drawModeDisableTimer);
        drawModeDisableTimer = null;
      }
      drawModeDeadlineUtcMs = 0;
      updateDrawTimerUi();
    }

    function updateDrawTimerUi() {
      const timerEl = document.getElementById('drawTimer');
      if (!timerEl) return;

      const shouldShow = drawModeEnabled && drawModeDeadlineUtcMs > 0;
      timerEl.className = shouldShow ? 'drawTimer' : 'drawTimer hidden';
      if (!shouldShow) {
        timerEl.textContent = '30s';
        return;
      }

      const remainingMs = Math.max(0, drawModeDeadlineUtcMs - Date.now());
      const remainingSeconds = Math.ceil(remainingMs / 1000);
      timerEl.textContent = String(remainingSeconds) + 's';
      applyDrawOverlayUi();
    }

    function ensureDrawCountdownTicking() {
      if (drawModeCountdownTimer) return;
      drawModeCountdownTimer = setInterval(function () {
        updateDrawTimerUi();
      }, 200);
    }

    function stopDrawCountdownTicking() {
      if (!drawModeCountdownTimer) return;
      clearInterval(drawModeCountdownTimer);
      drawModeCountdownTimer = null;
    }

    function scheduleDrawModeAutoOff() {
      if (!drawModeEnabled || okbDoodlesOnlyForced || selectedTab !== 'NOTES') {
        clearDrawModeDisableTimer();
        return;
      }

      clearDrawModeDisableTimer();
      drawModeDeadlineUtcMs = Date.now() + drawModeTimeoutMs;
      ensureDrawCountdownTicking();
      updateDrawTimerUi();
      drawModeDisableTimer = setTimeout(function () {
        disableDrawMode();
      }, drawModeTimeoutMs);
    }

    function notifyDrawActivity() {
      if (!drawModeEnabled || okbDoodlesOnlyForced || selectedTab !== 'NOTES') return;
      scheduleDrawModeAutoOff();
    }

    function disableDrawMode() {
      if (!drawModeEnabled) return;
      drawModeEnabled = false;
      setDrawInteractionInNotes(false);
      clearDrawModeDisableTimer();
      stopDrawCountdownTicking();
      persistDrawModePreference();
      updateDrawModeToggleUi();
      updateCursorModeForTab();
    }

    function getTabKeywordsMetrics() {
      const tabPanel = document.querySelector('.tabPanel');
      const keywordPanel = document.getElementById('keywordPanel');
      if (!tabPanel || !keywordPanel) return null;

      const topHeight = tabPanel.offsetHeight;
      const bottomHeight = keywordPanel.offsetHeight;
      const total = topHeight + bottomHeight;
      if (total <= 0) return null;

      const tabMin = parseFloat(window.getComputedStyle(tabPanel).minHeight) || 120;
      const keywordMin = parseFloat(window.getComputedStyle(keywordPanel).minHeight) || 100;

      return {
        tabPanel: tabPanel,
        keywordPanel: keywordPanel,
        total: total,
        topHeight: topHeight,
        minTop: tabMin,
        minBottom: keywordMin,
      };
    }

    function getTabSplitStorageKey(tab) {
      return String(tab || 'LOG')
        .toUpperCase()
        .replace(/\s+/g, '_')
        .replace(/[^A-Z0-9_]/g, '');
    }

    function resetTabKeywordsSplitToDefault() {
      const metrics = getTabKeywordsMetrics();
      if (!metrics) return;
      metrics.tabPanel.style.flex = '';
      metrics.keywordPanel.style.flex = '';
    }

    function getCurrentTabKeywordsSplitRatio() {
      const key = getTabSplitStorageKey(selectedTab);
      const value = tabKeywordsSplitByTab[key];
      return isFinite(value) ? value : NaN;
    }

    function setCurrentTabKeywordsSplitRatio(ratio) {
      if (!isFinite(ratio)) return;
      const key = getTabSplitStorageKey(selectedTab);
      tabKeywordsSplitByTab[key] = ratio;
      writeStoredPreferenceValue(tabKeywordsSplitStorageKey, JSON.stringify(tabKeywordsSplitByTab));
    }

    function applyTabKeywordsSplitRatio(ratio, persist) {
      const metrics = getTabKeywordsMetrics();
      if (!metrics) return;

      const safeRatio = isFinite(ratio) ? ratio : (metrics.topHeight / metrics.total);
      let targetTop = metrics.total * clamp(safeRatio, 0.15, 0.85);
      const maxTop = Math.max(metrics.minTop, metrics.total - metrics.minBottom);
      targetTop = clamp(targetTop, metrics.minTop, maxTop);

      metrics.tabPanel.style.flex = '0 0 ' + Math.round(targetTop) + 'px';
      metrics.keywordPanel.style.flex = '1 1 auto';

      if (persist) {
        setCurrentTabKeywordsSplitRatio(targetTop / metrics.total);
      }
    }

    function readTabKeywordsSplitRatioByTab() {
      try {
        const raw = readStoredPreferenceValue(tabKeywordsSplitStorageKey);
        if (!raw) return {};
        const parsed = JSON.parse(raw);
        if (!parsed || typeof parsed !== 'object') return {};

        const cleaned = {};
        Object.keys(parsed).forEach(function (k) {
          const value = parseFloat(parsed[k]);
          if (isFinite(value)) {
            cleaned[String(k)] = value;
          }
        });

        return cleaned;
      } catch (_) {
        return {};
      }
    }

    function applyCurrentTabKeywordsSplit() {
      if (selectedTab === 'DTC' || selectedTab === 'EFB') {
        const tabPanel = document.querySelector('.tabPanel');
        const keywordPanel = document.getElementById('keywordPanel');
        if (tabPanel) tabPanel.style.flex = '1 1 auto';
        if (keywordPanel) keywordPanel.style.flex = '';
        return;
      }

      const ratio = getCurrentTabKeywordsSplitRatio();
      if (isFinite(ratio)) {
        applyTabKeywordsSplitRatio(ratio, false);
        return;
      }

      resetTabKeywordsSplitToDefault();
    }

    function initTabKeywordsDivider() {
      const divider = document.getElementById('tabKeywordDivider');
      if (!divider) return;

      let drag = null;

      divider.addEventListener('mousedown', function (ev) {
        const metrics = getTabKeywordsMetrics();
        if (!metrics) return;

        const maxTop = Math.max(metrics.minTop, metrics.total - metrics.minBottom);
        drag = {
          startY: ev.clientY,
          startTop: metrics.topHeight,
          minTop: metrics.minTop,
          maxTop: maxTop,
          total: metrics.total,
        };

        ev.preventDefault();
      });

      document.addEventListener('mousemove', function (ev) {
        if (!drag) return;
        const targetTop = clamp(drag.startTop + (ev.clientY - drag.startY), drag.minTop, drag.maxTop);
        const ratio = targetTop / drag.total;
        applyTabKeywordsSplitRatio(ratio, false);
      });

      document.addEventListener('mouseup', function () {
        if (!drag) return;
        const metrics = getTabKeywordsMetrics();
        if (metrics) {
          const ratio = metrics.topHeight / metrics.total;
          applyTabKeywordsSplitRatio(ratio, true);
        }
        drag = null;
      });
    }

    function getMergedLog(map, tab) {
      const lines = [];
      if (!map) return '';
      Object.keys(map).forEach(function (k) {
        if (normalizeCategory(k) !== tab) return;
        const text = String(map[k] || '').trim();
        if (!text) return;
        if (lines.indexOf(text) < 0) lines.push(text);
      });
      return lines.join('\n');
    }

    function getMergedList(map, tab) {
      const result = [];
      if (!map) return result;
      Object.keys(map).forEach(function (k) {
        if (normalizeCategory(k) !== tab) return;
        mergeUnique(result, Array.isArray(map[k]) ? map[k] : []);
      });
      return result;
    }

    function getAiCrewCategories(data) {
      const server = (data && data.Server) || {};
      const aircraft = String(server.Aircraft || '').toUpperCase();

      if (aircraft.indexOf('F-14') >= 0) return ['RIO', 'ICEMAN', 'AI CREW'];
      if (aircraft.indexOf('F-4') >= 0) return ['WSO', 'AIWSO', 'AI CREW', 'REF', 'CREW', 'REF/CREW', 'GND CREW'];
      if (aircraft.indexOf('AH-64') >= 0 || aircraft.indexOf('AH64') >= 0) return ['GEORGE', 'CPG', 'AICPG', 'AI CREW', 'REF', 'CREW', 'REF/CREW', 'GND CREW'];

      return ['RIO', 'ICEMAN', 'WSO', 'GEORGE', 'CPG', 'AICPG', 'AIWSO', 'AI CREW', 'REF', 'CREW', 'REF/CREW', 'GND CREW'];
    }

    function getF4GroundCrewKeywords() {
      return [
        'Ground Chocks Place',
        'Ground Chocks Remove',
        'Ground Power Connect',
        'Ground Power Disconnect',
        'Ground Air Connect Right',
        'Ground Air Connect Left',
        'Ground Air On',
        'Ground Air Off',
        'Ground Air Disconnect',
        'Ground Load Start Cartridges',
        'Ground Remove Start Cartridges',
        'Ground Place the Ladder',
        'Ground Remove the Ladder',
        'Ground Extend Steps',
        'Ground Retract Steps',
        'Ground Comms Check',
        'Ground Pitot Check',
        'Ground Spoilers Check',
        'Ground Flight Controls Check',
        'Ground A R I Check',
        'Ground Stab Aug Check',
        'Ground Trim Check'
      ];
    }

    function isF4Aircraft(data) {
      const server = (data && data.Server) || {};
      const aircraft = String(server.Aircraft || '').toUpperCase();
      return aircraft.indexOf('F-4') >= 0;
    }

    function isF14Aircraft(data) {
      const server = (data && data.Server) || {};
      const aircraft = String(server.Aircraft || '').toUpperCase();
      return aircraft.indexOf('F-14') >= 0 || aircraft.indexOf('F14') >= 0;
    }

    function getF14AiCrewKeywordGroups(data, phrases) {
      const sectionOrder = [
        'Startup and Shutdown',
        'Radio',
        'Radar',
        'Utility/Navigation',
        'Weapons',
        'LANTIRN',
        'Defensive/Countermeasures',
        'Datalink',
        'TACAN',
        'Walkman',
        'Crew Contract',
        'Miscellaneous',
        'Supercarriers',
        'AI Pilot'
      ];

      const sectionMap = (data && data.AiCrewKeywordSections) || {};
      const rows = Array.isArray(phrases) ? phrases : [];
      if (!rows.length) return [];

      let hasAnySection = false;
      for (let i = 0; i < sectionOrder.length; i++) {
        const vals = sectionMap[sectionOrder[i]];
        if (Array.isArray(vals) && vals.length) {
          hasAnySection = true;
          break;
        }
      }
      if (!hasAnySection && !isF14Aircraft(data)) return [];

      function classifyF14Phrase(text) {
        const t = String(text || '').toLowerCase();
        if (!t) return '';
        if (t.indexOf('radar ') === 0 || t.indexOf('track ') === 0 || t.indexOf('scan ') === 0 || t.indexOf('tid ') === 0 || t.indexOf('vsl ') === 0 || t.indexOf('break lock') === 0 || t.indexOf('go bvr') === 0 || t.indexOf('go active') === 0 || t.indexOf('go standby') === 0 || t.indexOf('switch stt') === 0) return 'Radar';
        if (t.indexOf('lantern') >= 0 || t.indexOf('lantirn') >= 0 || t.indexOf('look for ') === 0 || t.indexOf('arm laser') === 0 || t.indexOf('aspect switch') === 0) return 'LANTIRN';
        if (t.indexOf('weapon ') >= 0 || t.indexOf('attack mode') === 0 || t.indexOf('air to ground') >= 0 || t.indexOf('air to air') >= 0 || t.indexOf('drop ') === 0 || t.indexOf('set ripple ') === 0 || t.indexOf('select stations') === 0 || t.indexOf('send pre planned') === 0 || t.indexOf('send designation') === 0) return 'Weapons';
        if (t.indexOf('radio ') === 0) return 'Radio';
        if (t.indexOf('link ') === 0) return 'Datalink';
        if (t.indexOf('tacan') === 0) return 'TACAN';
        if (t.indexOf('navigate') === 0 || t.indexOf('direct steerpoint') === 0 || t.indexOf('nav mode') === 0 || t.indexOf('restore') === 0 || t.indexOf('load flight plan') === 0 || t.indexOf('reload flight plan') === 0 || t.indexOf('grid ') === 0) return 'Utility/Navigation';
        if (t.indexOf('rock and roll') === 0 || t.indexOf('cut it out') === 0 || t.indexOf('skip this part') === 0 || t.indexOf('go back a little') === 0) return 'Walkman';
        if (t.indexOf('contract') === 0 || t.indexOf('no talking') === 0 || t.indexOf('talk to me') === 0 || t.indexOf('set eject') === 0 || t.indexOf('landing callouts') === 0 || t.indexOf('back to work') === 0 || t.indexOf('knock it off') === 0 || t.indexOf('wake up') === 0) return 'Crew Contract';
        if (t.indexOf('link host ') === 0 || t.indexOf('tacan tune ') === 0) return 'Supercarriers';
        if (t.indexOf('chaff') === 0 || t.indexOf('flare') === 0 || t.indexOf('flares') === 0 || t.indexOf('countermeasure') === 0 || t.indexOf('jammer') === 0 || t.indexOf('black hot') === 0 || t.indexOf('white hot') === 0) return 'Defensive/Countermeasures';
        if (t.indexOf('startup') === 0 || t.indexOf('assisted startup') === 0 || t.indexOf('align ') === 0 || t.indexOf('abort startup') === 0 || t.indexOf('commence shutdown') === 0 || t === 'check' || t.indexOf('hold it') === 0 || t.indexOf('loud and clear') === 0) return 'Startup and Shutdown';
        if (t.indexOf('open canopy') === 0 || t.indexOf('close canopy') === 0 || t.indexOf('open menu') === 0 || t.indexOf('close menu') === 0 || t.indexOf('toggle menu') === 0 || t.indexOf('do option ') === 0 || t.indexOf('do menu ') === 0) return 'Miscellaneous';
        if (t.indexOf('set altitude') === 0 || t.indexOf('go angels') === 0 || t.indexOf('change altitude') === 0 || t.indexOf('climb ') === 0 || t.indexOf('descent ') === 0 || t.indexOf('slow down') === 0 || t.indexOf('speed up') === 0 || t.indexOf('heading ') === 0 || t.indexOf('set heading') === 0 || t.indexOf('turn ') === 0 || t.indexOf('change speed') === 0 || t.indexOf('fly to destination') === 0 || t.indexOf('orbit destination') === 0 || t.indexOf('head straight') === 0) return 'AI Pilot';
        return '';
      }

      const canonicalByKey = {};
      rows.forEach(function (p) {
        const text = String(p || '').replace(/\s+/g, ' ').trim();
        if (!text) return;
        canonicalByKey[text.toUpperCase()] = text;
      });

      const groups = [];
      sectionOrder.forEach(function (section) {
        let sectionItems = Array.isArray(sectionMap[section]) ? sectionMap[section] : [];
        if (!hasAnySection) {
          sectionItems = rows.filter(function (item) { return classifyF14Phrase(item) === section; });
        }
        const unique = [];
        sectionItems.forEach(function (item) {
          const text = String(item || '').replace(/\s+/g, ' ').trim();
          if (!text) return;
          const canonical = canonicalByKey[text.toUpperCase()] || text;
          if (unique.indexOf(canonical) < 0) unique.push(canonical);
        });

        if (!unique.length) return;
        unique.sort(function (a, b) { return String(a).localeCompare(String(b)); });
        groups.push({ title: section, items: unique });
      });

      return groups;
    }

    // Handles non-F14 aircraft (e.g. AH-64D) whose sections come straight from AiCrewKeywordSections.
    function getAiCrewKeywordGroups(data, phrases) {
      const sectionMap = (data && data.AiCrewKeywordSections) || {};
      const sectionNames = Object.keys(sectionMap);
      if (!sectionNames.length) return [];

      const rows = Array.isArray(phrases) ? phrases : [];
      if (!rows.length) return [];

      const canonicalByKey = {};
      rows.forEach(function (p) {
        const text = String(p || '').replace(/\s+/g, ' ').trim();
        if (!text) return;
        canonicalByKey[text.toUpperCase()] = text;
      });

      const groups = [];
      sectionNames.forEach(function (section) {
        const sectionItems = Array.isArray(sectionMap[section]) ? sectionMap[section] : [];
        const unique = [];
        sectionItems.forEach(function (item) {
          const text = String(item || '').replace(/\s+/g, ' ').trim();
          if (!text) return;
          const canonical = canonicalByKey[text.toUpperCase()] || text;
          if (unique.indexOf(canonical) < 0) unique.push(canonical);
        });

        if (!unique.length) return;
        groups.push({ title: section, items: unique });
      });

      return groups;
    }

    function rankF4KeywordByPhase(phase, phrase) {
      const p = String(phrase || '').trim();
      if (!p) return 100;

      const pp = p.toLowerCase();
      function hasAny(terms) {
        for (let i = 0; i < terms.length; i++) {
          if (pp.indexOf(terms[i]) >= 0) return true;
        }
        return false;
      }

      const crewControl = [
        'Countermeasures Yours',
        'Countermeasures Mine',
        'Crew Auto',
        'Crew Disable',
        'Crew Force',
        'Eject Both',
        'Eject WSO',
        'Report Speed',
        'Some Silence',
        'Start Alignment Now',
        'Talk to Me'
      ];

      const startupMisc = [
        'Going Below 100 Feet',
        'Going Below 150 Feet',
        'Going Below 200 Feet',
        'Going Below 50 Feet',
        'Negative Not Going Low',
        'Negative On Alignment',
        'Start BATH Alignment',
        'Start Full Alignment',
        'Start Stored Alignment',
        'Will Let You Know',
        'Yes Start Alignment'
      ];

      if (phase === 'startup and taxi') {
        if (/^Ground\s+/i.test(p)) return 0;
        if (startupMisc.indexOf(p) >= 0) return 1;
        if (crewControl.indexOf(p) >= 0) return 2;
        return 3;
      }

      if (phase === 'enroute') {
        if (hasAny([
          'navigation', 'tacan', 'waypoint', 'flight plan', 'resume', 'hold ', 'hold at',
          'divert', 'tune radio', 'select mode',
          'radar', 'iff', 'boresight', 'scan', 'auto focus', 'go radar'
        ])) return 0;
        return 1;
      }

      if (phase === 'fence/target') {
        if (hasAny([
          'countermeasures', 'chaff', 'flare', 'jammer', 'jettison',
          'pave spike', 'tv weapons', 'designate', 'undesignate', 'lock target', 'focus target', 'context '
        ])) return 0;
        return 1;
      }

      if (phase === 'approach/landing') {
        if (hasAny([
          'navigation', 'tacan', 'waypoint', 'flight plan', 'resume', 'hold ', 'divert', 'tune radio', 'select mode'
        ])) return 0;
        if (p === 'Fuel Is Looking Good') return 1;
        return 2;
      }

      if (phase === 'divert/low fuel') {
        if (hasAny(['divert', 'fuel'])) return 0;
        if (hasAny([
          'navigation', 'tacan', 'waypoint', 'flight plan', 'resume', 'hold ', 'tune radio', 'select mode'
        ])) return 1;
        return 2;
      }

      if (phase === 'taxi in/shutdown') {
        if (/^Ground\s+/i.test(p)) return 0;
        return 1;
      }

      return 0;
    }

    function reorderAiCrewPhrasesForPhase(data, phrases) {
      const list = Array.isArray(phrases) ? phrases.slice() : [];
      const phase = String((data && data.AiCrewPhase) || '').trim().toLowerCase();

      if (!isF4Aircraft(data)) {
        list.sort(function (a, b) { return a.localeCompare(b); });
        return list;
      }

      if (phase === 'startup and taxi'
        || phase === 'enroute'
        || phase === 'fence/target'
        || phase === 'approach/landing'
        || phase === 'divert/low fuel'
        || phase === 'taxi in/shutdown') {
        list.sort(function (a, b) {
          const rankDiff = rankF4KeywordByPhase(phase, a) - rankF4KeywordByPhase(phase, b);
          if (rankDiff !== 0) return rankDiff;
          return String(a).localeCompare(String(b));
        });
        return list;
      }

      list.sort(function (a, b) { return a.localeCompare(b); });
      return list;
    }

    function getDeletedStepSet(state) {
      const s = state || {};
      const deleted = (s.deletedSteps && typeof s.deletedSteps === 'object') ? s.deletedSteps : {};
      const set = {};
      Object.keys(deleted).forEach(function (k) {
        const key = stepToKey(k);
        if (!key) return;
        if (!!deleted[k]) set[key] = true;
      });
      return set;
    }

    function getEffectiveRouteRows(rows, state) {
      const list = Array.isArray(rows) ? rows : [];
      const deletedSet = getDeletedStepSet(state);
      return list.filter(function (wp) {
        if (!wp || wp.isStart) return false;
        const key = stepToKey(wp.step);
        if (!key) return false;
        return !deletedSet[key];
      });
    }

    function clearInvalidDirectToState(state) {
      if (!state || typeof state !== 'object') return;
      state.directToSourceStep = '';
      state.directToTargetStep = '';
    }

    function getDirectToSpanInfo(rows, state) {
      const activeRows = getEffectiveRouteRows(rows, state);
      const sourceStep = stepToKey(state && state.directToSourceStep);
      const targetStep = stepToKey(state && state.directToTargetStep);
      if (!sourceStep || !targetStep || !activeRows.length) return null;

      const sourceIdx = activeRows.findIndex(function (wp) { return stepToKey(wp.step) === sourceStep; });
      const targetIdx = activeRows.findIndex(function (wp) { return stepToKey(wp.step) === targetStep; });
      if (sourceIdx < 0 || targetIdx < 0 || targetIdx <= sourceIdx) return null;

      return {
        activeRows: activeRows,
        sourceIdx: sourceIdx,
        targetIdx: targetIdx,
      };
    }

    function ensureDirectToStateValid(rows, state) {
      if (!state || typeof state !== 'object') return;
      const sourceStep = stepToKey(state.directToSourceStep);
      const targetStep = stepToKey(state.directToTargetStep);
      if (!sourceStep || !targetStep) {
        if (!sourceStep || !targetStep) {
          if (!sourceStep) state.directToSourceStep = '';
          if (!targetStep) state.directToTargetStep = '';
        }
        return;
      }

      const span = getDirectToSpanInfo(rows, state);
      if (!span) {
        clearInvalidDirectToState(state);
      }
    }

    function getSkippedStepSet(rows, state) {
      const skipped = {};
      const span = getDirectToSpanInfo(rows, state);
      if (!span) return skipped;

      for (let i = span.sourceIdx + 1; i < span.targetIdx; i++) {
        const wp = span.activeRows[i];
        const key = stepToKey(wp && wp.step);
        if (!key) continue;
        skipped[key] = true;
      }

      return skipped;
    }

    function textHasAny(text, terms) {
      const source = String(text || '').toLowerCase();
      for (let i = 0; i < terms.length; i++) {
        if (source.indexOf(String(terms[i] || '').toLowerCase()) >= 0) return true;
      }
      return false;
    }

    function isCarrierContext(data) {
      const carrierTokens = [
        'carrier', 'supercarrier', 'cvn', 'lso', 'paddles', 'marshal', 'platform',
        'roosevelt', 'lincoln', 'washington', 'stennis', 'truman', 'vinson',
        'kuznetsov', 'tarawa', 'perry', 'normandy'
      ];

      const server = (data && data.Server) || {};
      const scan = [];
      scan.push(String(server.MissionTitle || ''));
      scan.push(String(server.MissionBriefing || ''));
      scan.push(String(server.MissionDetails || ''));

      const atcUnits = getMergedList(data && data.Units, 'ATC');
      const atcDetails = getMergedList(data && data.UnitDetails, 'ATC');
      const atcLog = getMergedLog(data && data.Logs, 'ATC');
      atcUnits.forEach(function (v) { scan.push(String(v || '')); });
      atcDetails.forEach(function (v) { scan.push(String(v || '')); });
      scan.push(atcLog);

      return textHasAny(scan.join('\n'), carrierTokens);
    }

    function isCarrierCapableAircraft(data) {
      const server = (data && data.Server) || {};
      const aircraft = String(server.Aircraft || '').toUpperCase();
      return textHasAny(aircraft, [
        'F/A-18', 'FA-18', 'HORNET',
        'F-14', 'TOMCAT',
        'AV-8', 'AV8', 'HARRIER',
        'A-4', 'SKYHAWK',
        'SU-33'
      ]);
    }

    function classifyAtcKeyword(phrase) {
      const p = String(phrase || '').toLowerCase();
      if (!p) return 'general';

      if (textHasAny(p, ['salute', 'request launch', 'airborne', 'passing 2.5 kilo'])) return 'launch_ops';
      if (textHasAny(p, ['case i', 'case one', 'see you at ten', 'overhead', 'kiss off', 'charlie'])) return 'case_i';
      if (textHasAny(p, [
        'case ii', 'case two',
        'case iii', 'case three',
        'expected on time', 'platform', 'approach check in', 'checking in',
        'commencing', 'established', 'needles', 'up and left', 'up and on', 'up and right'
      ])) return 'case_ii_iii';
      if (textHasAny(p, ['marking moms', 'inbound for carrier', 'low state', 'confirm remaining fuel', 'lso', 'paddles'])) return 'carrier_common';

      if (textHasAny(p, [
        'catapult', 'marshal', 'ball', 'meatball', 'clara'
      ])) return 'carrier';

      if (textHasAny(p, [
        'startup', 'engine start', 'engines start', 'request startup', 'hover', 'taxi',
        'wheelchocks', 'chocks'
      ])) return 'startup_taxi';

      if (textHasAny(p, [
        'takeoff', 'departure'
      ])) return 'departure';

      if (textHasAny(p, [
        'inbound', 'vector', 'initial', 'overhead', 'straight in', 'approach', 'final', 'request landing'
      ])) return 'arrival_approach';

      if (textHasAny(p, ['parking', 'abort', 'cancel'])) return 'shutdown';

      return 'general';
    }

    function reorderAtcPhrasesForContext(data, phrases) {
      const list = Array.isArray(phrases) ? phrases.slice() : [];
      const carrierContext = isCarrierContext(data);
      const carrierCapable = isCarrierCapableAircraft(data);

      const rankMap = (carrierContext && carrierCapable)
        ? {
          launch_ops: 0,
          case_i: 1,
          case_ii_iii: 2,
          carrier_common: 4,
          carrier: 5,
          startup_taxi: 6,
          departure: 7,
          arrival_approach: 8,
          shutdown: 9,
          general: 10,
        }
        : {
          startup_taxi: 0,
          departure: 1,
          arrival_approach: 2,
          shutdown: 3,
          general: 4,
          carrier: 5,
          launch_ops: 6,
          case_i: 7,
          case_ii_iii: 8,
          carrier_common: 10,
        };

      list.sort(function (a, b) {
        const ra = rankMap[classifyAtcKeyword(a)] || 99;
        const rb = rankMap[classifyAtcKeyword(b)] || 99;
        if (ra !== rb) return ra - rb;
        return String(a).localeCompare(String(b));
      });

      return list;
    }

    function classifyGroundCrewKeyword(phrase) {
      const p = String(phrase || '').toLowerCase();
      if (!p) return 'general';

      if (textHasAny(p, ['request repair'])) return 'servicing_arming';

      if (textHasAny(p, [
        'refuel', 'refueling', 'cannon', 'rearming', 'load water', 'request hmd', 'request nvg',
        'start cartridges', 'remove start cartridges', 'turbo on', 'turbo off'
      ])) return 'servicing_arming';

      if (textHasAny(p, ['apply air', 'connect air supply', 'disconnect air supply'])) return 'startup';

      if (textHasAny(p, [
        'ground power', 'power connect', 'power disconnect', 'air connect', 'air disconnect', 'air on', 'air off',
        'run inertial starter', 'request engines start', 'request startup'
      ])) return 'startup';

      if (textHasAny(p, [
        'comms check', 'a r i check', 'flight controls check', 'pitot check', 'spoilers check', 'stab aug check', 'trim check'
      ])) return 'ground_checks';

      if (textHasAny(p, [
        'chocks', 'wheelchocks', 'ladder', 'steps', 'taxi', 'dispatch'
      ])) return 'dispatching';

      return 'general';
    }

    function reorderGroundCrewPhrasesForFlow(phrases) {
      const list = Array.isArray(phrases) ? phrases.slice() : [];
      const rankMap = {
        servicing_arming: 0,
        startup: 1,
        ground_checks: 2,
        dispatching: 3,
        general: 4,
      };

      list.sort(function (a, b) {
        const ra = rankMap[classifyGroundCrewKeyword(a)] || 99;
        const rb = rankMap[classifyGroundCrewKeyword(b)] || 99;
        if (ra !== rb) return ra - rb;
        return String(a).localeCompare(String(b));
      });

      return list;
    }

    function classifyJtacKeyword(phrase) {
      const p = String(phrase || '').toLowerCase();
      if (!p) return 'general';

      if (textHasAny(p, ['playtime', 'check in'])) return 'establish_checkin';
      if (textHasAny(p, ['ready to copy', 'ready for remarks', 'nine line', 'readback', 'copy', 'reading back', 'remarks', 'what is my target'])) return 'tasking';
      if (textHasAny(p, ['ip inbound', 'one minute'])) return 'ip_inbound';
      if (textHasAny(p, ['sparkle', 'snake', 'steady', 'pulse', 'rope', 'laser on', 'shift', 'spot', 'contact sparkle', 'contact the mark'])) return 'setup_talkon';
      if (textHasAny(p, ['in from', ' in ', 'off', 'guns', 'bombs away', 'rifles', 'rockets', 'attack complete', 'in hot', 'ten seconds', 'terminate'])) return 'engage';
      if (textHasAny(p, ['request bda', 'bda', 'no joy', 'unable to comply', 'request target', 'request tasking', 'confirm kill', 'copy kill', 'standby for bda', 'advise ready for bda'])) return 'retasking';
      if (textHasAny(p, ['check out', 'checkout'])) return 'establish_checkout';

      return 'general';
    }

    function reorderJtacPhrasesForFlow(phrases) {
      const list = Array.isArray(phrases) ? phrases.slice() : [];
      const rankMap = {
        establish_checkin: 0,
        tasking: 1,
        ip_inbound: 2,
        setup_talkon: 3,
        engage: 4,
        retasking: 5,
        establish_checkout: 6,
        general: 7,
      };

      list.sort(function (a, b) {
        const ra = rankMap[classifyJtacKeyword(a)] || 99;
        const rb = rankMap[classifyJtacKeyword(b)] || 99;
        if (ra !== rb) return ra - rb;
        return String(a).localeCompare(String(b));
      });

      return list;
    }

    function classifyFlightKeyword(phrase) {
      const p = String(phrase || '').toLowerCase();
      if (!p) return 'general';

      if (textHasAny(p, [
        '30 left go', '30 right go', '45 left go', '45 right go',
        '60 left go', '60 right go', '90 left go', '90 right go',
        'turnabout left go', 'turnabout right go', 'rotate go', 'shackle go',
        'helos go spread', 'go helo left', 'go helo right', 'go helo tight', 'close group',
        'kick out to '
      ])) return 'tactical_formation';

      if (textHasAny(p, [
        'ground target', 'armor', 'artillery', 'air defense', 'aaa', 'sam', 'utility', 'infantry', 'ship',
        'd-link target', 'ray target', 'attack', 'task and return to base', 'rifle', 'rockets', 'bombs away',
        'reference my spee', 'reference my steerpoint', 'reference point', 'reference ', 'check my spee'
      ])) return 'tactical_a2g';

      if (p.indexOf('..') >= 0) return 'enroute';

      if (textHasAny(p, [
        'bandit', 'bogey', 'hostile', 'my enemy', 'my target', 'cover me', 'pincer', 'break ', 'clear ', 'pump',
        'radar on', 'radar off', 'ecm', 'music on', 'music off', 'fence in', 'fence out', 'out cold', 'off cold'
      ])) return 'tactical_a2a';

      if (textHasAny(p, [
        'check in', 'join up', 'rejoin', 'fly route', 'anchor', 'hold position', 'return to base', 'go home', 'rtb',
        'tanker', 'line abreast', 'trail', 'wedge', 'echelon', 'finger four', 'spread four', 'formation',
        'heading ', 'flow ', 'widen', 'close up', 'go heavy', 'go cruise', 'go combat'
      ])) return 'enroute';

      return 'general';
    }

    function reorderFlightPhrasesForContext(phrases) {
      const list = Array.isArray(phrases) ? phrases.slice() : [];
      const rankMap = { enroute: 0, tactical_formation: 1, tactical_a2a: 2, tactical_a2g: 3, general: 4 };

      list.sort(function (a, b) {
        const ra = rankMap[classifyFlightKeyword(a)] || 99;
        const rb = rankMap[classifyFlightKeyword(b)] || 99;
        if (ra !== rb) return ra - rb;
        return String(a).localeCompare(String(b));
      });

      return list;
    }

    function getKeywordGroupsForTab(data, tab, phrases) {
      const rows = Array.isArray(phrases) ? phrases.slice() : [];
      if (!rows.length) return [];

      if (tab === 'ATC') {
        const carrierContext = isCarrierContext(data);
        const carrierCapable = isCarrierCapableAircraft(data);
        const labels = {
          launch_ops: 'Launch Ops',
          case_i: 'Recovery CASE I',
          case_ii_iii: 'Recovery CASE II / III',
          carrier_common: 'Carrier Common',
          carrier: carrierContext ? 'Carrier Ops Priority' : 'Carrier Ops',
          startup_taxi: 'Startup and Taxi',
          departure: 'Departure',
          arrival_approach: 'Arrival and Approach',
          shutdown: 'Taxi In and Shutdown',
          general: 'General',
        };

        const orderedKeys = (carrierContext && carrierCapable)
          ? ['launch_ops', 'case_i', 'case_ii_iii', 'carrier_common', 'carrier', 'startup_taxi', 'departure', 'arrival_approach', 'shutdown', 'general']
          : ['startup_taxi', 'departure', 'arrival_approach', 'shutdown', 'general', 'carrier', 'launch_ops', 'case_i', 'case_ii_iii', 'carrier_common'];

        const buckets = {
          launch_ops: [],
          case_i: [],
          case_ii_iii: [],
          carrier_common: [],
          carrier: [],
          startup_taxi: [],
          departure: [],
          arrival_approach: [],
          shutdown: [],
          general: []
        };
        rows.forEach(function (r) {
          const key = classifyAtcKeyword(r);
          (buckets[key] || buckets.general).push(r);
        });

        const groups = [];
        orderedKeys.forEach(function (k) {
          const vals = buckets[k] || [];
          if (!vals.length) return;
          groups.push({ title: labels[k], items: vals });
        });
        return groups;
      }

      if (tab === 'GND CREW') {
        const orderedKeys = ['servicing_arming', 'startup', 'ground_checks', 'dispatching', 'general'];
        const labels = {
          servicing_arming: 'Servicing and Arming',
          startup: 'Startup',
          ground_checks: 'Ground Checks',
          dispatching: 'Dispatching',
          general: 'General',
        };
        const buckets = {
          servicing_arming: [],
          startup: [],
          ground_checks: [],
          dispatching: [],
          general: []
        };
        rows.forEach(function (r) {
          const key = classifyGroundCrewKeyword(r);
          (buckets[key] || buckets.general).push(r);
        });

        const groups = [];
        orderedKeys.forEach(function (k) {
          const vals = buckets[k] || [];
          if (!vals.length) return;
          groups.push({ title: labels[k], items: vals });
        });
        return groups;
      }

      if (tab === 'JTAC') {
        const orderedKeys = ['establish_checkin', 'tasking', 'ip_inbound', 'setup_talkon', 'engage', 'retasking', 'establish_checkout', 'general'];
        const labels = {
          establish_checkin: 'Stage Establish (Check In)',
          tasking: 'Stage Tasking',
          ip_inbound: 'Stage IP Inbound',
          setup_talkon: 'Stage Setup and Talk On',
          engage: 'Stage Engage',
          retasking: 'Stage Re-Engage / Re-Tasking',
          establish_checkout: 'Stage Establish (Check Out)',
          general: 'General',
        };
        const buckets = {
          establish_checkin: [],
          tasking: [],
          ip_inbound: [],
          setup_talkon: [],
          engage: [],
          retasking: [],
          establish_checkout: [],
          general: []
        };
        rows.forEach(function (r) {
          const key = classifyJtacKeyword(r);
          (buckets[key] || buckets.general).push(r);
        });

        const groups = [];
        orderedKeys.forEach(function (k) {
          const vals = buckets[k] || [];
          if (!vals.length) return;
          groups.push({ title: labels[k], items: vals });
        });
        return groups;
      }

      if (tab === 'FLIGHT') {
        const orderedKeys = ['enroute', 'tactical_formation', 'tactical_a2a', 'tactical_a2g', 'general'];
        const labels = {
          enroute: 'Enroute',
          tactical_formation: 'Tactical Formation',
          tactical_a2a: 'Tactical Air to Air',
          tactical_a2g: 'Tactical Air to Ground',
          general: 'General',
        };
        const buckets = { enroute: [], tactical_formation: [], tactical_a2a: [], tactical_a2g: [], general: [] };
        rows.forEach(function (r) {
          const key = classifyFlightKeyword(r);
          (buckets[key] || buckets.general).push(r);
        });

        const groups = [];
        orderedKeys.forEach(function (k) {
          const vals = buckets[k] || [];
          if (!vals.length) return;
          groups.push({ title: labels[k], items: vals });
        });
        return groups;
      }

      if (tab === 'AI CREW') {
        if (isF14Aircraft(data)) {
          const f14Groups = getF14AiCrewKeywordGroups(data, rows);
          if (f14Groups.length) return f14Groups;
        } else {
          const keywordGroups = getAiCrewKeywordGroups(data, rows);
          if (keywordGroups.length) return keywordGroups;
        }

        const suffix = formatAiCrewPhaseLabel(data && data.AiCrewPhase);
        return [{ title: 'Primary' + suffix, items: rows }];
      }

      return [{ title: 'Reference', items: rows }];
    }

    function getMergedLogByCategories(map, categories) {
      const lines = [];
      if (!map) return '';
      const allowed = categories.map(function (c) { return String(c).toUpperCase(); });

      Object.keys(map).forEach(function (k) {
        const key = String(k || '').toUpperCase();
        const normalized = normalizeCategory(key).toUpperCase();
        if (allowed.indexOf(key) < 0 && allowed.indexOf(normalized) < 0) return;
        const text = String(map[k] || '').trim();
        if (!text) return;
        if (lines.indexOf(text) < 0) lines.push(text);
      });

      return lines.join('\n');
    }

    function getMergedAliasesByCategories(chunkMap, categories) {
      const result = {};
      if (!chunkMap) return result;
      const allowed = categories.map(function (c) { return String(c).toUpperCase(); });

      Object.keys(chunkMap).forEach(function (k) {
        const key = String(k || '').toUpperCase();
        const normalized = normalizeCategory(key).toUpperCase();
        if (allowed.indexOf(key) < 0 && allowed.indexOf(normalized) < 0) return;
        const aliasObj = chunkMap[k] || {};
        Object.keys(aliasObj).forEach(function (a) {
          if (!result[a]) result[a] = [];
          mergeUnique(result[a], Array.isArray(aliasObj[a]) ? aliasObj[a] : []);
        });
      });

      return result;
    }

    function getMergedAliases(chunkMap, tab) {
      const result = {};
      if (!chunkMap) return result;
      Object.keys(chunkMap).forEach(function (k) {
        if (normalizeCategory(k) !== tab) return;
        const aliasObj = chunkMap[k] || {};
        Object.keys(aliasObj).forEach(function (a) {
          if (!result[a]) result[a] = [];
          mergeUnique(result[a], Array.isArray(aliasObj[a]) ? aliasObj[a] : []);
        });
      });
      return result;
    }

    function getKeywordPhrasesForTab(data, tab) {
      if (tab === 'AI CREW') {
        const direct = Array.isArray(data.AiCrewKeywords) ? data.AiCrewKeywords.slice() : [];
        const cleaned = [];
        direct.forEach(function (k) {
          const phrase = String(k || '').replace(/\s+/g, ' ').trim();
          if (!phrase) return;
          if (cleaned.indexOf(phrase) < 0) cleaned.push(phrase);
        });
        return reorderAiCrewPhrasesForPhase(data, cleaned);
      }

      const phrases = [];

      function pushPhrase(p) {
        const phrase = String(p || '').replace(/\s+/g, ' ').trim();
        if (!phrase) return;
        if (phrases.indexOf(phrase) < 0) phrases.push(phrase);
      }

      function collectFromChunk(chunkMap) {
        const alias = tab === 'AI CREW'
          ? getMergedAliasesByCategories(chunkMap, getAiCrewCategories(data))
          : getMergedAliases(chunkMap, tab);
        const keys = Object.keys(alias);
        keys.forEach(function (k) {
          const vals = (alias[k] || []).filter(function (v) { return String(v || '').trim() !== ''; });
          if (!vals.length) {
            pushPhrase(k);
            return;
          }

          vals.forEach(function (v) {
            pushPhrase(k + ' ' + v);
          });
        });
      }

      collectFromChunk(data.AliasesChunk0);
      collectFromChunk(data.AliasesChunk1);

      if (tab === 'AI CREW' && !phrases.length) {
        const fallback = getKeywordPhrasesForTab(data, 'GND CREW');
        fallback.forEach(function (p) { pushPhrase(p); });
      }

      if (tab === 'GND CREW') {
        const server = (data && data.Server) || {};
        const aircraft = String(server.Aircraft || '').toUpperCase();
        if (aircraft.indexOf('F-4') >= 0) {
          getF4GroundCrewKeywords().forEach(function (k) { pushPhrase(k); });
        }
      }

      if (tab === 'NOTES') {
        pushPhrase('Start Dictate');
        pushPhrase('End Dictate');
        pushPhrase('Clear Notes');
      }

      if (tab === 'GND CREW') {
        const filtered = [];
        const aiCrewSet = new Set((Array.isArray(data.AiCrewKeywords) ? data.AiCrewKeywords : []).map(function (k) {
          return String(k || '').replace(/\s+/g, ' ').trim().toUpperCase();
        }).filter(function (k) { return k.length > 0; }));

        phrases.forEach(function (p) {
          const normalized = String(p || '').replace(/\s+/g, ' ').trim().toUpperCase();
          if (!/^George\s/i.test(p) && !aiCrewSet.has(normalized)) filtered.push(p);
        });
        return reorderGroundCrewPhrasesForFlow(filtered);
      }

      if (tab === 'ATC') {
        return reorderAtcPhrasesForContext(data, phrases);
      }

      if (tab === 'FLIGHT') {
        return reorderFlightPhrasesForContext(phrases);
      }

      if (tab === 'JTAC') {
        return reorderJtacPhrasesForFlow(phrases);
      }

      phrases.sort(function (a, b) { return a.localeCompare(b); });
      return phrases;
    }

    function formatKeywordReference(data, tab) {
      if (tab === 'LOG') return 'No keyword reference for this tab.';
      if (tab === 'DTC') return 'No keyword reference for this tab.';
      const phrases = getKeywordPhrasesForTab(data, tab);
      if (!phrases.length) return 'No keywords for this tab yet.';
      return phrases.join('\n');
    }

    function parseRteSelectionToken(value) {
      const text = String(value || '');
      const prefix = 'RTE::';
      if (text.indexOf(prefix) !== 0) return null;
      const sep = text.indexOf('::', prefix.length);
      if (sep < 0) return null;
      const encodedRoute = text.substring(prefix.length, sep);
      const filePath = text.substring(sep + 2);
      let routeName = encodedRoute;
      try { routeName = decodeURIComponent(encodedRoute); } catch (_) { }
      return { routeName: routeName, filePath: filePath };
    }

    function getPathFileName(path) {
      const norm = String(path || '').replace(/\\/g, '/');
      const idx = norm.lastIndexOf('/');
      return idx >= 0 ? norm.substring(idx + 1) : norm;
    }

    function getFltPlnPath(path) {
      const rte = parseRteSelectionToken(path);
      return rte ? rte.filePath : String(path || '');
    }

    function inferTheatreCandidateFromSelection(selectedPath) {
      const fullPath = String(getFltPlnPath(selectedPath) || '').trim();
      if (!fullPath) return '';
      const normalized = fullPath.replace(/\\/g, '/');
      const fileName = getPathFileName(normalized);
      const noExt = String(fileName || '').replace(/\.[^.]+$/, '').trim();
      if (!noExt) return '';
      return noExt;
    }

    function resolveOpenFreeMapTheatreInfo(model) {
      const source = model || latestData || {};
      const server = (source && source.Server) || {};
      const diagnostics = (server && server.Diagnostics) || {};

      const liveCandidates = [
        server.Theater,
        diagnostics.theater,
        diagnostics.terrain,
        diagnostics.terrainName,
        lastKnownTheater,
      ];

      for (let i = 0; i < liveCandidates.length; i++) {
        const candidate = String(liveCandidates[i] || '').trim();
        if (!candidate) continue;
        if (getMapProjectionByTheatre(candidate)) {
          return { theatre: candidate, inferred: false };
        }
      }

      const selected = String(source.DtcSelectedFile || '');
      const inferredCandidate = inferTheatreCandidateFromSelection(selected);
      if (inferredCandidate && getMapProjectionByTheatre(inferredCandidate)) {
        return { theatre: inferredCandidate, inferred: true };
      }

      return { theatre: '', inferred: false };
    }

    function resolveOpenFreeMapFallbackTheatreText(model) {
      const source = model || latestData || {};
      const server = (source && source.Server) || {};
      const diagnostics = (server && server.Diagnostics) || {};

      const liveCandidates = [
        server.Theater,
        diagnostics.theater,
        diagnostics.terrain,
        diagnostics.terrainName,
        lastKnownTheater,
      ];

      for (let i = 0; i < liveCandidates.length; i++) {
        const candidate = String(liveCandidates[i] || '').trim();
        if (!candidate) continue;
        if (getMapProjectionByTheatre(candidate)) {
          return candidate;
        }
      }

      const selected = String(source.DtcSelectedFile || '');
      const inferredCandidate = inferTheatreCandidateFromSelection(selected);
      if (inferredCandidate && getMapProjectionByTheatre(inferredCandidate)) {
        return inferredCandidate;
      }

      return '';
    }

    function getTheatreZeroPointLonLat(theatre) {
      function validLonLat(ll) {
        return !!ll
          && isFinite(Number(ll.lon))
          && isFinite(Number(ll.lat))
          && Math.abs(Number(ll.lat)) <= 90
          && Math.abs(Number(ll.lon)) <= 180;
      }

      const llPrimary = convertDcsXYToLatLon(theatre, 0, 0);
      if (validLonLat(llPrimary)) {
        return [Number(llPrimary.lon), Number(llPrimary.lat)];
      }

      const llSwap = convertDcsXYToLatLon(theatre, 0, 0);
      if (validLonLat(llSwap)) {
        return [Number(llSwap.lon), Number(llSwap.lat)];
      }

      return null;
    }

    function getDtcDisplayName(path) {
      const text = String(path || '');
      if (!text) return '-';
      const rte = parseRteSelectionToken(text);
      if (rte) {
        return String(rte.routeName || '-');
      }
      return getPathFileName(text);
    }

    function appendDtcRows(prefix, value, rows, depth) {
      if (rows.length >= 220) return;
      if (depth > 8) {
        rows.push({ key: prefix, value: '[depth limit]' });
        return;
      }

      if (value === null || value === undefined) {
        rows.push({ key: prefix, value: '-' });
        return;
      }

      if (Array.isArray(value)) {
        if (!value.length) {
          rows.push({ key: prefix, value: '[]' });
          return;
        }

        for (let i = 0; i < value.length; i++) {
          appendDtcRows(prefix + '[' + i + ']', value[i], rows, depth + 1);
          if (rows.length >= 220) return;
        }
        return;
      }

      if (typeof value === 'object') {
        const keys = Object.keys(value);
        if (!keys.length) {
          rows.push({ key: prefix, value: '{}' });
          return;
        }

        keys.forEach(function (k) {
          if (rows.length >= 220) return;
          const nextPrefix = prefix ? (prefix + '.' + k) : k;
          appendDtcRows(nextPrefix, value[k], rows, depth + 1);
        });
        return;
      }

      rows.push({ key: prefix, value: String(value) });
    }

    function findFirstObjectByKeyPattern(value, pattern, depth) {
      if (depth > 8 || value === null || value === undefined) return null;
      if (Array.isArray(value)) {
        for (let i = 0; i < value.length; i++) {
          const found = findFirstObjectByKeyPattern(value[i], pattern, depth + 1);
          if (found) return found;
        }
        return null;
      }
      if (typeof value !== 'object') return null;

      const keys = Object.keys(value);
      for (let i = 0; i < keys.length; i++) {
        const k = keys[i];
        const v = value[k];
        if (pattern.test(String(k)) && v && typeof v === 'object') return v;
      }

      for (let i = 0; i < keys.length; i++) {
        const found = findFirstObjectByKeyPattern(value[keys[i]], pattern, depth + 1);
        if (found) return found;
      }
      return null;
    }

    function collectScalarRows(prefix, value, rows, depth, maxRows) {
      if (rows.length >= maxRows || depth > 8) return;
      if (value === null || value === undefined) {
        rows.push({ key: prefix || '-', value: '-' });
        return;
      }
      if (Array.isArray(value)) {
        for (let i = 0; i < value.length; i++) {
          collectScalarRows((prefix || 'item') + '[' + i + ']', value[i], rows, depth + 1, maxRows);
          if (rows.length >= maxRows) return;
        }
        return;
      }
      if (typeof value === 'object') {
        const keys = Object.keys(value);
        if (!keys.length) { rows.push({ key: prefix || '-', value: '{}' }); return; }
        keys.forEach(function (k) {
          if (rows.length >= maxRows) return;
          const nextPrefix = prefix ? (prefix + '.' + k) : k;
          collectScalarRows(nextPrefix, value[k], rows, depth + 1, maxRows);
        });
        return;
      }
      rows.push({ key: prefix || '-', value: String(value) });
    }

    function isNavPointObject(o) {
      if (!o || typeof o !== 'object') return false;
      const hasXY = isFinite(Number(o.x)) && isFinite(Number(o.y));
      const hasLatLon = (isFinite(Number(o.lat)) && isFinite(Number(o.lon)))
        || (isFinite(Number(o.latitude)) && isFinite(Number(o.longitude)));
      const hasWpMeta = o.type || o.action || o.name || o.ETA || o.alt;
      return hasXY || hasLatLon || !!hasWpMeta;
    }

    function collectNavPoints(value, points, depth) {
      if (points.length >= 200 || depth > 9 || value === null || value === undefined) return;
      if (Array.isArray(value)) {
        value.forEach(function (item) { if (points.length < 200) collectNavPoints(item, points, depth + 1); });
        return;
      }
      if (typeof value !== 'object') return;
      if (isNavPointObject(value)) points.push(value);
      Object.keys(value).forEach(function (k) {
        if (points.length >= 200) return;
        const child = value[k];
        if (child && typeof child === 'object') collectNavPoints(child, points, depth + 1);
      });
    }

    function formatDtcFocusedTable(root, selected) {
      const cmdsRoot = findFirstObjectByKeyPattern(root, /(cmds|countermeasures|countermeasure)/i, 0);
      const navRoot = findFirstObjectByKeyPattern(root, /(nav|waypoint|waypoints|route|flight\s*plan|flightplan|steer)/i, 0);

      const cmdRows = [];
      if (cmdsRoot) collectScalarRows('CMDS', cmdsRoot, cmdRows, 0, 220);

      const navPoints = [];
      if (navRoot) collectNavPoints(navRoot, navPoints, 0);
      if (!navPoints.length) collectNavPoints(root, navPoints, 0);

      const lines = [];
      lines.push('DTC FILE   : ' + getDtcDisplayName(selected));
      lines.push('PATH       : ' + selected);
      lines.push('');
      lines.push('CMDS SUMMARY');
      lines.push('------------');
      if (!cmdRows.length) {
        lines.push('No CMDS data found.');
      } else {
        const maxKeyWidth = cmdRows.reduce(function (acc, r) { return Math.max(acc, String(r.key || '').length); }, 8);
        const keyWidth = clamp(maxKeyWidth + 1, 20, 64);
        cmdRows.forEach(function (r) {
          lines.push(String(r.key || '-').padEnd(keyWidth) + String(r.value || '-').replace(/\s+/g, ' ').trim());
        });
      }

      lines.push('');
      lines.push('NAV POINTS');
      lines.push('----------');
      lines.push('WP  NAME         TYPE           ALT      ETA       LAT/LON               X            Y');
      lines.push('--- ------------ -------------- -------- -------- --------------------- ------------ ------------');
      if (!navPoints.length) {
        lines.push('No nav points found.');
      } else {
        navPoints.slice(0, 200).forEach(function (p, idx) {
          const wp = String(idx + 1).padStart(2, '0');
          const name = String(p.name || '').trim() || '-';
          const type = String(p.type || p.action || 'WP').trim() || 'WP';
          const alt = isFinite(Number(p.alt)) ? String(Math.round(Number(p.alt))) : '-';
          const eta = formatEtaSeconds(p.ETA);
          const lat = isFinite(Number(p.lat)) ? Number(p.lat) : (isFinite(Number(p.latitude)) ? Number(p.latitude) : NaN);
          const lon = isFinite(Number(p.lon)) ? Number(p.lon) : (isFinite(Number(p.longitude)) ? Number(p.longitude) : NaN);
          const latLon = (isFinite(lat) && isFinite(lon)) ? (lat.toFixed(5) + ', ' + lon.toFixed(5)) : '-';
          const x = isFinite(Number(p.x)) ? String(Math.round(Number(p.x))) : '-';
          const y = isFinite(Number(p.y)) ? String(Math.round(Number(p.y))) : '-';
          lines.push(
            wp + '  '
            + name.substring(0, 12).padEnd(12, ' ') + ' '
            + type.substring(0, 14).padEnd(14, ' ') + ' '
            + alt.padStart(8, ' ') + ' '
            + eta.padEnd(8, ' ') + ' '
            + latLon.substring(0, 21).padEnd(21, ' ') + ' '
            + x.padStart(12, ' ') + ' '
            + y.padStart(12, ' ')
          );
        });
      }

      lines.push('');
      lines.push('Note: LAT/LON requires DCS runtime map projection/origin if not provided directly by source data.');
      return lines.join('\n');
    }

    function formatEtaSeconds(v) {
      const n = Number(v);
      if (!isFinite(n) || n < 0) return '-';
      const total = Math.floor(n);
      const h = Math.floor(total / 3600);
      const m = Math.floor((total % 3600) / 60);
      const s = total % 60;
      return String(h).padStart(2, '0') + ':' + String(m).padStart(2, '0') + ':' + String(s).padStart(2, '0');
    }

    function getTakeoffTimeBySelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      const value = Number(fltPlanEtaStartBySelection[key]);
      return isFinite(value) ? value : NaN;
    }

    const stepToStartSeconds = 10 * 60;
    const startToTaxiSeconds = 10 * 60;
    const taxiToTakeoffSeconds = 15 * 60;

    function buildTimingFromTakeoff(takeoffSec) {
      const t = Number(takeoffSec);
      if (!isFinite(t)) {
        return { step: NaN, start: NaN, taxi: NaN, takeoff: NaN };
      }

      return {
        step: t - (stepToStartSeconds + startToTaxiSeconds + taxiToTakeoffSeconds),
        start: t - (startToTaxiSeconds + taxiToTakeoffSeconds),
        taxi: t - taxiToTakeoffSeconds,
        takeoff: t,
      };
    }

    function getResolvedTimingMarks(selected) {
      const state = getFlightPlanPlanState(selected);
      const defaults = buildTimingFromTakeoff(getTakeoffTimeBySelection(selected));
      const marks = (state && state.timeMarks && typeof state.timeMarks === 'object') ? state.timeMarks : {};

      function pick(name) {
        const fromState = Number(marks[name]);
        if (isFinite(fromState)) return fromState;
        return Number(defaults[name]);
      }

      return {
        step: pick('step'),
        start: pick('start'),
        taxi: pick('taxi'),
        takeoff: pick('takeoff'),
      };
    }

    function appendTimingLog(selected, anchor, timing) {
      const state = getFlightPlanPlanState(selected);
      if (!Array.isArray(state.timingLog)) state.timingLog = [];

      const eventUtc = new Date();
      const eventClock = formatSecondsToClock(getCurrentFlightPlanClockSeconds());
      const text = eventUtc.toISOString()
        + ' | ' + String(anchor || '').toUpperCase()
        + ' set=' + eventClock
        + ' | STEP ' + (isFinite(Number(timing.step)) ? formatSecondsToClock(Number(timing.step)) : '-')
        + ' START ' + (isFinite(Number(timing.start)) ? formatSecondsToClock(Number(timing.start)) : '-')
        + ' TAXI ' + (isFinite(Number(timing.taxi)) ? formatSecondsToClock(Number(timing.taxi)) : '-')
        + ' TAKEOFF ' + (isFinite(Number(timing.takeoff)) ? formatSecondsToClock(Number(timing.takeoff)) : '-');

      state.timingLog.push(text);
      if (state.timingLog.length > 12) {
        state.timingLog = state.timingLog.slice(state.timingLog.length - 12);
      }
    }

    function formatSignedDeltaSeconds(deltaSeconds) {
      const n = Number(deltaSeconds);
      if (!isFinite(n)) return '';
      const sign = n >= 0 ? '+' : '-';
      const abs = Math.abs(Math.round(n));
      const h = Math.floor(abs / 3600);
      const m = Math.floor((abs % 3600) / 60);
      const s = abs % 60;
      return sign + String(h).padStart(2, '0') + ':' + String(m).padStart(2, '0') + ':' + String(s).padStart(2, '0');
    }

    function formatElapsedSeconds(seconds) {
      const n = Number(seconds);
      if (!isFinite(n) || n < 0) return '-';
      const total = Math.round(n);
      const h = Math.floor(total / 3600);
      const m = Math.floor((total % 3600) / 60);
      const s = total % 60;
      return String(h).padStart(2, '0') + ':' + String(m).padStart(2, '0') + ':' + String(s).padStart(2, '0');
    }

    function clearSpeedRecommendations(selected) {
      const state = getFlightPlanPlanState(selected);
      state.speedRecommendations = {};
    }

    function pruneExpiredSpeedRecommendations(state) {
      if (!state || !state.speedRecommendations || typeof state.speedRecommendations !== 'object') return;
      const now = Date.now();
      Object.keys(state.speedRecommendations).forEach(function (k) {
        const rec = state.speedRecommendations[k] || {};
        const expires = Number(rec.expiresUtcMs);
        if (isFinite(expires) && expires > 0 && now > expires) {
          delete state.speedRecommendations[k];
        }
      });
    }

    function getPlanWaypointsForRecommendations(selected) {
      try {
        if (!latestData) return [];

        const sourceType = String((latestData.DtcSourceType || '')).toUpperCase();
        const dtcJson = String(latestData.DtcJson || '').trim();
        let rows = [];

        if (selected === '__RUNTIME_PLAYER__') {
          rows = getMissionRuntimeWaypoints(latestData);
        } else if (dtcJson) {
          let parsed;
          try { parsed = JSON.parse(dtcJson); } catch (_) { parsed = null; }
          const root = (parsed && parsed.data && typeof parsed.data === 'object') ? parsed.data : parsed;
          if (root) {
            if (sourceType === 'DTC') {
              const route = getDtcRouteBySelection(selected);
              const all = getDtcWaypoints(root);
              rows = filterDtcWaypointsByRoute(root, all, route);
            } else if (sourceType === 'RTE') {
              const names = Object.keys(root || {}).sort(function (a, b) { return String(a).localeCompare(String(b)); });
              const routeName = String(names[0] || '');
              rows = routeName ? getRouteWaypoints(root[routeName] || {}) : [];
            }
          }
        }

        rows = applyTypeOverrides(Array.isArray(rows) ? rows.slice() : [], selected);
        rows = applyAltitudeAdjustments(rows, selected);
        rows = applyEtaPlanToWaypoints(rows, selected);
        rows = applySpeedAdjustmentsToWaypoints(rows, selected);
        rows = applyRouteTimeline(rows, selected);
        rows = applyLockedTotPlan(rows, selected);
        return rows;
      } catch (_) {
        return [];
      }
    }

    function parseWindBandsFromDiagnostics(data) {
      const server = (data && data.Server) || {};
      const diagnostics = (server && server.Diagnostics && typeof server.Diagnostics === 'object') ? server.Diagnostics : {};
      const rows = Array.isArray(diagnostics.weatherSummary) ? diagnostics.weatherSummary : [];
      if (!rows.length) return null;

      const bands = {
        ground: { speedMs: NaN, dirDeg: NaN },
        a2000: { speedMs: NaN, dirDeg: NaN },
        a8000: { speedMs: NaN, dirDeg: NaN },
      };

      rows.forEach(function (row) {
        const text = String(row || '').trim();
        if (!text) return;
        const idx = text.indexOf('=');
        if (idx <= 0) return;
        const key = String(text.substring(0, idx)).trim().toLowerCase();
        const val = Number(text.substring(idx + 1));
        if (!isFinite(val)) return;

        if (key === 'mission.weather.wind.atground.speed') bands.ground.speedMs = val;
        if (key === 'mission.weather.wind.atground.dir') bands.ground.dirDeg = val;
        if (key === 'mission.weather.wind.at2000.speed') bands.a2000.speedMs = val;
        if (key === 'mission.weather.wind.at2000.dir') bands.a2000.dirDeg = val;
        if (key === 'mission.weather.wind.at8000.speed') bands.a8000.speedMs = val;
        if (key === 'mission.weather.wind.at8000.dir') bands.a8000.dirDeg = val;
      });

      return bands;
    }

    function lerp(a, b, t) {
      const av = Number(a);
      const bv = Number(b);
      const tv = Number(t);
      if (!isFinite(av) || !isFinite(bv) || !isFinite(tv)) return NaN;
      return av + ((bv - av) * tv);
    }

    function clamp01(v) {
      const n = Number(v);
      if (!isFinite(n)) return 0;
      if (n < 0) return 0;
      if (n > 1) return 1;
      return n;
    }

    function sampleWindAtAltitude(bands, altFeet) {
      if (!bands || typeof bands !== 'object') {
        return { speedMs: NaN, dirDeg: NaN };
      }

      const alt = Math.max(0, Number(altFeet) || 0);
      if (alt <= 2000) {
        const t = clamp01(alt / 2000.0);
        return {
          speedMs: lerp(bands.ground && bands.ground.speedMs, bands.a2000 && bands.a2000.speedMs, t),
          dirDeg: lerp(bands.ground && bands.ground.dirDeg, bands.a2000 && bands.a2000.dirDeg, t),
        };
      }

      if (alt <= 8000) {
        const t = clamp01((alt - 2000.0) / 6000.0);
        return {
          speedMs: lerp(bands.a2000 && bands.a2000.speedMs, bands.a8000 && bands.a8000.speedMs, t),
          dirDeg: lerp(bands.a2000 && bands.a2000.dirDeg, bands.a8000 && bands.a8000.dirDeg, t),
        };
      }

      return {
        speedMs: Number(bands.a8000 && bands.a8000.speedMs),
        dirDeg: Number(bands.a8000 && bands.a8000.dirDeg),
      };
    }

    function getAlongTrackWindKnots(fromWp, toWp, sampleAltFeet) {
      const trackDeg = computeTrueHeadingDeg(fromWp, toWp);
      if (!isFinite(trackDeg)) return 0;

      const windBands = parseWindBandsFromDiagnostics(latestData);
      const wind = sampleWindAtAltitude(windBands, sampleAltFeet);
      const speedMs = Number(wind && wind.speedMs);
      const dirFromDeg = Number(wind && wind.dirDeg);
      if (!isFinite(speedMs) || speedMs <= 0 || !isFinite(dirFromDeg)) return 0;

      const speedKnots = speedMs * 1.9438444924406;
      if (!isFinite(speedKnots) || speedKnots <= 0) return 0;

      const dirToDeg = normalizeHeadingDeg(dirFromDeg + 180.0);
      const windRad = (dirToDeg * Math.PI) / 180.0;
      const trackRad = (trackDeg * Math.PI) / 180.0;

      const windNorth = Math.cos(windRad) * speedKnots;
      const windEast = Math.sin(windRad) * speedKnots;
      const trackNorth = Math.cos(trackRad);
      const trackEast = Math.sin(trackRad);

      const along = (windNorth * trackNorth) + (windEast * trackEast);
      return isFinite(along) ? along : 0;
    }

    function computeRequiredKcasForLeg(fromWp, toWp, fromTimeSec, targetTimeSec) {
      const distNm = computeLegDistanceNm(fromWp, toWp);
      if (!isFinite(distNm) || distNm <= 0) return NaN;

      const dt = Number(targetTimeSec) - Number(fromTimeSec);
      if (!isFinite(dt) || dt <= 0) return NaN;

      const gs = (distNm * 3600.0) / dt;
      if (!isFinite(gs) || gs <= 0) return NaN;

      const fromAlt = isFinite(Number(fromWp && fromWp.altFeet)) ? Number(fromWp.altFeet) : 0;
      const toAlt = isFinite(Number(toWp && toWp.altFeet)) ? Number(toWp.altFeet) : fromAlt;
      const sampleAlt = (fromAlt + toAlt) / 2.0;
      const alongWind = getAlongTrackWindKnots(fromWp, toWp, sampleAlt);
      const requiredTas = gs - alongWind;
      if (!isFinite(requiredTas) || requiredTas <= 0) return NaN;

      const alt = isFinite(Number(toWp && toWp.altFeet)) ? Number(toWp.altFeet) : 0;
      const kcas = requiredTas / (1.0 + (Math.max(0, alt) / 100000.0));
      return isFinite(kcas) ? kcas : NaN;
    }

    function getRecommendationMinKcas(altFeet) {
      return 220;
    }

    function getRecommendationMaxKcas(altFeet) {
      return 700;
    }

    function isKcasWithinRecommendationLimits(kcas, altFeet) {
      const v = Number(kcas);
      if (!isFinite(v) || v <= 0) return false;
      const min = getRecommendationMinKcas(altFeet);
      const max = getRecommendationMaxKcas(altFeet);
      if (!isFinite(min) || !isFinite(max) || max < min) return false;
      if (v < min || v > max) return false;

      return true;
    }

    function roundRecommendedKcas(kcas) {
      const n = Number(kcas);
      if (!isFinite(n)) return NaN;
      return Math.max(80, Math.round(n / 10) * 10);
    }

    function normalizeRecommendedKcas(kcas, altFeet) {
      const rounded = roundRecommendedKcas(kcas);
      if (!isFinite(rounded)) return NaN;
      const min = getRecommendationMinKcas(altFeet);
      const max = getRecommendationMaxKcas(altFeet);
      if (!isFinite(min) || !isFinite(max) || max < min) return NaN;
      if (rounded < min || rounded > max) return NaN;
      return rounded;
    }

    function setSpeedRecommendation(state, step, recommendedKcas, useRed) {
      if (!state || !state.speedRecommendations) return;
      const key = stepToKey(step);
      const rk = Number(recommendedKcas);
      if (!key || !isFinite(rk) || rk <= 0) return;
      state.speedRecommendations[key] = {
        kcas: rk,
        red: !!useRed,
        expiresUtcMs: Date.now() + speedRecommendationTimeoutMs,
      };
    }

    function resolveEtaTargetSecondsFromAta(plannedEtaSeconds, ataActualSeconds, ataPlannedSeconds) {
      const plannedEta = Number(plannedEtaSeconds);
      const actualAta = Number(ataActualSeconds);
      if (!isFinite(plannedEta) || !isFinite(actualAta)) return NaN;

      // Target the listed plan ETA directly so REC SPD reflects required recovery to schedule.
      let target = plannedEta;

      // Keep the target in the future relative to the ATA event.
      while (target <= actualAta) {
        target += 86400;
      }

      return target;
    }

    function buildSpeedRecommendationsForAta(selected, ataStep) {
      const state = getFlightPlanPlanState(selected);
      if (!state || !state.speedRecommendations) return;
      state.speedRecommendations = {};
      state.autoAtaLastFallbackCode = '';

      const rows = getPlanWaypointsForRecommendations(selected);
      if (!rows.length) return;

      const idx = rows.findIndex(function (wp) { return stepToKey(wp && wp.step) === stepToKey(ataStep); });
      if (idx < 0 || idx + 1 >= rows.length) return;

      const ata = state.ataByStep && state.ataByStep[stepToKey(ataStep)] ? state.ataByStep[stepToKey(ataStep)] : null;
      const fromTime = ata ? Number(ata.actualSeconds) : NaN;
      const fromPlanned = ata ? Number(ata.plannedSeconds) : NaN;
      if (!isFinite(fromTime)) return;

      let fromPoint = rows[idx];
      if (fakeMissionEnabled && fakeMissionState && fakeMissionState.ownshipStarted) {
        const ownX = Number(fakeMissionState.playerX);
        const ownY = Number(fakeMissionState.playerY);
        if (isFinite(ownX) && isFinite(ownY)) {
          fromPoint = {
            x: ownX,
            y: ownY,
            xNum: ownX,
            yNum: ownY,
            altFeet: isFinite(Number(rows[idx] && rows[idx].altFeet)) ? Number(rows[idx].altFeet) : 0,
          };
        }
      }

      const nextWp = rows[idx + 1];
      const nextTargetPlanned = parseEtaToSeconds(nextWp && (nextWp.etaDisplay || nextWp.eta));
      const nextTarget = resolveEtaTargetSecondsFromAta(nextTargetPlanned, fromTime, fromPlanned);
      const reqNext = computeRequiredKcasForLeg(fromPoint, nextWp, fromTime, nextTarget);
      const reqNextRounded = normalizeRecommendedKcas(reqNext, nextWp && nextWp.altFeet);
      if (isKcasWithinRecommendationLimits(reqNextRounded, nextWp && nextWp.altFeet)) {
        setSpeedRecommendation(state, nextWp.step, reqNextRounded, true);
        return;
      }
      if (isFinite(Number(reqNext))) {
        const minNext = getRecommendationMinKcas(nextWp && nextWp.altFeet);
        const maxNext = getRecommendationMaxKcas(nextWp && nextWp.altFeet);
        if (isFinite(minNext) && Number(reqNext) < minNext) state.autoAtaLastFallbackCode = 'LOW';
        else if (isFinite(maxNext) && Number(reqNext) > maxNext) state.autoAtaLastFallbackCode = 'HIGH';
      }

      if (idx + 2 >= rows.length) return;

      const wpA = rows[idx + 1];
      const wpB = rows[idx + 2];
      const targetBPlanned = parseEtaToSeconds(wpB && (wpB.etaDisplay || wpB.eta));
      const targetB = resolveEtaTargetSecondsFromAta(targetBPlanned, fromTime, fromPlanned);
      const legA = computeLegDistanceNm(fromPoint, wpA);
      const legB = computeLegDistanceNm(wpA, wpB);
      if (!isFinite(legA) || !isFinite(legB) || legA <= 0 || legB <= 0) return;

      const totalDt = Number(targetB) - Number(fromTime);
      if (!isFinite(totalDt) || totalDt <= 0) return;

      const dtA = totalDt * (legA / (legA + legB));
      const dtB = totalDt - dtA;

      const reqA = normalizeRecommendedKcas(computeRequiredKcasForLeg(fromPoint, wpA, fromTime, fromTime + dtA), wpA && wpA.altFeet);
      const reqB = normalizeRecommendedKcas(computeRequiredKcasForLeg(wpA, wpB, fromTime + dtA, targetB), wpB && wpB.altFeet);
      const reqARaw = computeRequiredKcasForLeg(fromPoint, wpA, fromTime, fromTime + dtA);
      const reqBRaw = computeRequiredKcasForLeg(wpA, wpB, fromTime + dtA, targetB);
      if (!isKcasWithinRecommendationLimits(reqA, wpA && wpA.altFeet)) {
        const minA = getRecommendationMinKcas(wpA && wpA.altFeet);
        const maxA = getRecommendationMaxKcas(wpA && wpA.altFeet);
        if (isFinite(Number(reqARaw))) {
          if (isFinite(minA) && Number(reqARaw) < minA) state.autoAtaLastFallbackCode = 'LOW';
          else if (isFinite(maxA) && Number(reqARaw) > maxA) state.autoAtaLastFallbackCode = 'HIGH';
        }
        return;
      }
      if (!isKcasWithinRecommendationLimits(reqB, wpB && wpB.altFeet)) {
        const minB = getRecommendationMinKcas(wpB && wpB.altFeet);
        const maxB = getRecommendationMaxKcas(wpB && wpB.altFeet);
        if (isFinite(Number(reqBRaw))) {
          if (isFinite(minB) && Number(reqBRaw) < minB) state.autoAtaLastFallbackCode = 'LOW';
          else if (isFinite(maxB) && Number(reqBRaw) > maxB) state.autoAtaLastFallbackCode = 'HIGH';
        }
        return;
      }

      setSpeedRecommendation(state, wpA.step, reqA, true);
      setSpeedRecommendation(state, wpB.step, reqB, true);
    }

    function acceptSpeedRecommendation(selected, step) {
      const state = getFlightPlanPlanState(selected);
      if (!state || !state.speedRecommendations) return false;
      pruneExpiredSpeedRecommendations(state);
      const key = stepToKey(step);
      const rec = state.speedRecommendations[key];
      if (!rec) return false;

      const rows = getPlanWaypointsForRecommendations(selected);
      const wp = rows.find(function (r) { return stepToKey(r && r.step) === key; });
      if (!wp) return false;

      const current = Number(wp.spd);
      const target = Number(rec.kcas);
      if (!isFinite(current) || !isFinite(target)) return false;

      changeWaypointSpeedAdjustment(selected, key, target - current);
      delete state.speedRecommendations[key];
      return true;
    }

    function lockInSpeedRecommendations(selected) {
      const state = getFlightPlanPlanState(selected);
      if (!state || !state.speedRecommendations || typeof state.speedRecommendations !== 'object') return false;
      const keys = Object.keys(state.speedRecommendations)
        .map(function (k) { return stepToKey(k); })
        .filter(function (k) { return !!k; })
        .sort(function (a, b) { return Number(a) - Number(b); });
      if (!keys.length) return false;

      let applied = false;
      keys.forEach(function (stepKey) {
        if (acceptSpeedRecommendation(selected, stepKey)) {
          applied = true;
        }
      });
      return applied;
    }

