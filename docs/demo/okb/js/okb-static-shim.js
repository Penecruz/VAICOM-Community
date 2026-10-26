(function () {
  const MOCK_ROOT_URL = new URL('../mock-api/okb/', window.location.href);
  const HELP_DOC_URL = new URL('./docs/OKBHelpDoc.html', window.location.href);
  const LUA_FILE = 'MarianaIslands.lua';
  const HISTORY_KEY = 'vaicom.demo.okb.historytracks';

  const nativeFetch = window.fetch ? window.fetch.bind(window) : null;
  if (!nativeFetch) return;

  let bootPromise = null;
  let baseState = null;
  let baseClock = null;
  let baseOwnship = null;
  let baseAvbus = null;
  let baseAirports = null;
  let baseCharts = null;
  let routeLibrary = [];
  let selectedRouteToken = '';
  let missionStartMs = Date.now();

  function deepClone(value) {
    return JSON.parse(JSON.stringify(value));
  }

  function toJsonResponse(value, status) {
    const body = (typeof value === 'string') ? value : JSON.stringify(value);
    return new Response(body, {
      status: status || 200,
      headers: { 'Content-Type': 'application/json; charset=utf-8' }
    });
  }

  function toTextResponse(text, status) {
    return new Response(String(text || ''), {
      status: status || 200,
      headers: { 'Content-Type': 'text/plain; charset=utf-8' }
    });
  }

  function nowIso() {
    return new Date().toISOString();
  }

  function getMissionSeconds() {
    const base = Number(baseClock && baseClock.MissionTimeSeconds) || 45296;
    const elapsed = Math.max(0, (Date.now() - missionStartMs) / 1000);
    return Number((base + elapsed).toFixed(3));
  }

  function readNum(luaBody, key, fallback) {
    const re = new RegExp('\\["' + key.replace(/[.*+?^${}()|[\\]\\]/g, '\\$&') + '"\\]\\s*=\\s*(-?\\d+(?:\\.\\d+)?)');
    const m = luaBody.match(re);
    if (!m) return fallback;
    const n = Number(m[1]);
    return Number.isFinite(n) ? n : fallback;
  }

  function readText(luaBody, key, fallback) {
    const re = new RegExp('\\["' + key.replace(/[.*+?^${}()|[\\]\\]/g, '\\$&') + '"\\]\\s*=\\s*"([^"]*)"');
    const m = luaBody.match(re);
    return m ? String(m[1]) : fallback;
  }

  function parseRouteToolLua(luaText) {
    const routes = [];
    const presetRegex = /\["([^"]+)"\]\s*=\s*\{([\s\S]*?)\}\s*,\s*--\s*end\s*of\s*\["[^"]+"\]/g;
    let presetMatch;

    while ((presetMatch = presetRegex.exec(luaText)) !== null) {
      const routeName = String(presetMatch[1] || '').trim();
      const body = String(presetMatch[2] || '');
      if (!routeName) continue;

      const wpMap = {};
      const wpRegex = /\[(\d+)\]\s*=\s*\{([\s\S]*?)\}\s*,\s*--\s*end\s*of\s*\[\d+\]/g;
      let wpMatch;
      while ((wpMatch = wpRegex.exec(body)) !== null) {
        const idx = String(wpMatch[1] || '').trim();
        const wpBody = String(wpMatch[2] || '');
        if (!idx) continue;

        const wpName = readText(wpBody, 'name', '') || ('WP' + idx);
        const wpType = readText(wpBody, 'type', 'Turning Point') || 'Turning Point';

        wpMap[idx] = {
          alt: readNum(wpBody, 'alt', 0),
          type: wpType,
          ETA: readNum(wpBody, 'ETA', 0),
          y: readNum(wpBody, 'y', 0),
          x: readNum(wpBody, 'x', 0),
          name: wpName,
          action: readText(wpBody, 'action', wpType) || wpType,
          alt_type: readText(wpBody, 'alt_type', 'BARO') || 'BARO',
          speed: readNum(wpBody, 'speed', 0),
          speed_locked: false
        };
      }

      if (!Object.keys(wpMap).length) continue;

      const encoded = encodeURIComponent(routeName);
      const token = 'RTE::' + encoded + '::' + LUA_FILE;
      const payload = {};
      payload[routeName] = wpMap;

      routes.push({
        name: routeName,
        token: token,
        sourceFile: LUA_FILE,
        payload: payload
      });
    }

    return routes;
  }

  function findRouteByToken(token) {
    const sel = String(token || '');
    if (!sel) return null;

    for (let i = 0; i < routeLibrary.length; i++) {
      if (String(routeLibrary[i].token) === sel) return routeLibrary[i];
    }

    if (sel.indexOf('RTE::') === 0) {
      const rest = sel.substring('RTE::'.length);
      const sep = rest.indexOf('::');
      const encoded = sep >= 0 ? rest.substring(0, sep) : rest;
      let routeName = encoded;
      try { routeName = decodeURIComponent(encoded); } catch (_) { }
      for (let i = 0; i < routeLibrary.length; i++) {
        if (String(routeLibrary[i].name).toLowerCase() === String(routeName).toLowerCase()) {
          return routeLibrary[i];
        }
      }
    }

    return null;
  }

  function pickSelectedRoute(explicitToken) {
    const fromExplicit = findRouteByToken(explicitToken);
    if (fromExplicit) return fromExplicit;

    const fromState = findRouteByToken(selectedRouteToken);
    if (fromState) return fromState;

    const fromBase = findRouteByToken(baseState && baseState.DtcSelectedFile);
    if (fromBase) return fromBase;

    return routeLibrary.length ? routeLibrary[0] : null;
  }

  function buildState(selectedTab, selectedFile) {
    const state = deepClone(baseState || {});
    const route = pickSelectedRoute(selectedFile);

    state.UpdatedUtc = nowIso();
    state.ActiveCategory = selectedTab ? String(selectedTab).toUpperCase() : (state.ActiveCategory || 'LOG');

    if (!state.Server) state.Server = {};
    state.Server.MissionTimeSeconds = getMissionSeconds();
    state.Server.Theater = state.Server.Theater || 'MarianaIslands';

    state.Efb = state.Efb || {};
    state.Efb.FeatureEnabled = true;

    state.DtcFiles = routeLibrary.map(function (r) { return r.token; });

    if (route) {
      selectedRouteToken = route.token;
      state.DtcSelectedFile = route.token;
      state.DtcSourceType = 'RTE';
      state.DtcSourceFile = route.sourceFile;
      state.DtcJson = JSON.stringify(route.payload);
    } else {
      state.DtcSelectedFile = '';
      state.DtcJson = '{}';
      state.DtcSourceType = 'RTE';
      state.DtcSourceFile = LUA_FILE;
    }

    if (state.DtcJson && typeof state.DtcJson !== 'string') {
      state.DtcJson = JSON.stringify(state.DtcJson);
    }

    return state;
  }

  function buildClock() {
    const clock = deepClone(baseClock || {});
    clock.MissionTimeSeconds = getMissionSeconds();
    clock.SourceUtc = nowIso();
    return clock;
  }

  function buildOwnship() {
    const ownship = deepClone(baseOwnship || {});
    ownship.updatedUtc = nowIso();
    ownship.missionTimeSeconds = getMissionSeconds();
    return ownship;
  }

  function buildAvbus() {
    const av = deepClone(baseAvbus || {});
    av.updatedUtc = nowIso();
    return av;
  }

  function loadHistoryTracks() {
    try {
      const raw = localStorage.getItem(HISTORY_KEY);
      if (raw) return raw;
    } catch (_) { }
    return '{}';
  }

  function saveHistoryTracks(text) {
    try {
      localStorage.setItem(HISTORY_KEY, text || '{}');
    } catch (_) { }
  }

  async function loadJson(path, fallback) {
    try {
      const res = await nativeFetch(path, { cache: 'no-store' });
      if (!res.ok) return fallback;
      return await res.json();
    } catch (_) {
      return fallback;
    }
  }

  async function loadText(path, fallback) {
    try {
      const res = await nativeFetch(path, { cache: 'no-store' });
      if (!res.ok) return fallback;
      return await res.text();
    } catch (_) {
      return fallback;
    }
  }

  async function ensureBoot() {
    if (bootPromise) return bootPromise;

    bootPromise = (async function () {
      baseState = await loadJson(new URL('state.json', MOCK_ROOT_URL).href, {});
      baseClock = await loadJson(new URL('clock.json', MOCK_ROOT_URL).href, {});
      baseOwnship = await loadJson(new URL('ownship.json', MOCK_ROOT_URL).href, {});
      baseAvbus = await loadJson(new URL('avbus.json', MOCK_ROOT_URL).href, {});
      baseAirports = await loadJson(new URL('airports.json', MOCK_ROOT_URL).href, { airports: [], airportNames: {}, provider: 'mock' });
      baseCharts = await loadJson(new URL('charts.json', MOCK_ROOT_URL).href, { charts: [], provider: 'mock' });

      const lua = await loadText(new URL(LUA_FILE, MOCK_ROOT_URL).href, '');
      routeLibrary = parseRouteToolLua(lua);
      missionStartMs = Date.now();
    })();

    return bootPromise;
  }

  function parseInputUrl(input) {
    if (typeof input === 'string') return new URL(input, window.location.href);
    if (input && typeof input.url === 'string') return new URL(input.url, window.location.href);
    return new URL(String(input || ''), window.location.href);
  }

  function normalizeOkbPath(pathname) {
    const lower = String(pathname || '').toLowerCase();
    if (!lower) return '/';
    const okbIdx = lower.indexOf('/okb/');
    if (okbIdx >= 0) return lower.substring(okbIdx);
    if (lower.endsWith('/okb')) return '/okb';
    return lower;
  }

  window.fetch = async function (input, init) {
    const reqUrl = parseInputUrl(input);
    const path = normalizeOkbPath(reqUrl.pathname);
    const method = String((init && init.method) || (input && input.method) || 'GET').toUpperCase();

    const intercept =
      path === '/okb/state' || path === '/okb/index.json' || path === '/okb/clock' ||
      path === '/okb/sa/ownship' || path === '/okb/fltpln/avbus' || path === '/okb/dtc/list' ||
      path === '/okb/dtc/select' || path === '/okb/efb/airports' || path === '/okb/efb/charts' ||
      path === '/okb/efb/historytracks' || path === '/okb/dev/servermessages' ||
      path === '/okb/dev/refresh' || path === '/okb/helpdoc';

    if (!intercept) {
      return nativeFetch(input, init);
    }

    await ensureBoot();

    if (path === '/okb/state' || path === '/okb/index.json') {
      const selectedTab = reqUrl.searchParams.get('selectedTab') || '';
      return toJsonResponse(buildState(selectedTab, ''));
    }

    if (path === '/okb/clock') return toJsonResponse(buildClock());
    if (path === '/okb/sa/ownship') return toJsonResponse(buildOwnship());
    if (path === '/okb/fltpln/avbus') return toJsonResponse(buildAvbus());

    if (path === '/okb/dtc/list') {
      return toJsonResponse(buildState('', ''));
    }

    if (path === '/okb/dtc/select') {
      const file = reqUrl.searchParams.get('file') || '';
      return toJsonResponse(buildState('', file));
    }

    if (path === '/okb/efb/airports') {
      return toJsonResponse(baseAirports || { airports: [], airportNames: {}, provider: 'mock' });
    }

    if (path === '/okb/efb/charts') {
      const airport = reqUrl.searchParams.get('airport') || '';
      const payload = deepClone(baseCharts || { charts: [], provider: 'mock' });
      payload.airport = airport || payload.airport || '';
      return toJsonResponse(payload);
    }

    if (path === '/okb/efb/historytracks') {
      if (method === 'POST' || method === 'PUT') {
        const text = init && init.body ? String(init.body) : '{}';
        saveHistoryTracks(text);
        return toJsonResponse({ ok: true });
      }
      return toJsonResponse(loadHistoryTracks());
    }

    if (path === '/okb/dev/servermessages' || path === '/okb/dev/refresh') {
      return toJsonResponse({ ok: true });
    }

    if (path === '/okb/helpdoc') {
      try {
        return await nativeFetch(HELP_DOC_URL.href, { cache: 'no-store' });
      } catch (_) {
        return toTextResponse('Help content is unavailable in static shim mode.', 200);
      }
    }

    return nativeFetch(input, init);
  };
})();
