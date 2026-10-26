(function () {
  const MOCK_ROOT_URL = new URL('../mock-api/okb/', window.location.href);
  const HELP_DOC_URL = new URL('./docs/OKBHelpDoc.html', window.location.href);
  const CHARTS_ASSETS_ROOT_URL = new URL('./assets/', window.location.href);
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
  let efbChartManifestByAirport = {};
  let efbChartManifestLoadPromiseByAirport = {};
  let routeLibrary = [];
  let selectedRouteToken = '';
  let missionStartMs = Date.now();
  let baseHelpDocHtml = '';

  const HELP_DOC_SCOPED_STYLE = `<style>.okbHelpDocScope{font-family:'Segoe UI',Tahoma,Arial,sans-serif;color:#14263a !important;line-height:1.45;background:transparent;}.okbHelpDocScope *{writing-mode:horizontal-tb !important;text-orientation:mixed !important;transform:none !important;}.okbHelpDocScope,.okbHelpDocScope p,.okbHelpDocScope li,.okbHelpDocScope span,.okbHelpDocScope div{color:#14263a !important;}.okbHelpDocScope h2{margin:0 0 10px 0;font-size:22px;border-bottom:1px solid #c7d3df;padding-bottom:4px;color:#0f2438 !important;}.okbHelpDocScope h3{margin:12px 0 8px 0;font-size:18px;color:#16324b !important;}.okbHelpDocScope h4,.okbHelpDocScope h5,.okbHelpDocScope h6{margin:10px 0 6px 0;color:#1b3953 !important;}.okbHelpDocScope p{margin:8px 0;}.okbHelpDocScope ul,.okbHelpDocScope ol{margin:6px 0 10px 24px;padding:0;text-align:left !important;list-style-position:outside;}.okbHelpDocScope li{margin:4px 0;}.okbHelpDocScope strong,.okbHelpDocScope b{color:#0f2a43 !important;font-weight:700;}.okbHelpDocScope em,.okbHelpDocScope i{color:#1a3a56 !important;}.okbHelpDocScope code{background:#eaf1f8;border:1px solid #d5e2ef;border-radius:4px;padding:1px 5px;}.okbHelpDocScope pre{background:#eef4fb;border:1px solid #d5e2ef;border-radius:8px;padding:8px;overflow:auto;color:#11283f !important;}.okbHelpDocScope .tab{background:#ffffff;border:1px solid #c7d3df;border-radius:10px;padding:12px;margin-bottom:12px;display:block;}.okbHelpDocScope .controls{display:block !important;border-left:4px solid #9fb8d4;padding-left:10px;margin-top:10px;text-align:left !important;background:rgba(245,249,255,0.62);border-radius:0 6px 6px 0;}.okbHelpDocScope .controls > h3{display:block !important;margin:0 0 6px 0 !important;text-align:left !important;clear:both;}.okbHelpDocScope .controls > ul,.okbHelpDocScope .controls > ol{display:block !important;margin:0 0 12px 28px !important;padding:0 !important;text-align:left !important;clear:both;}.okbHelpDocScope a{color:#1a4b84;text-decoration:underline;font-weight:600;}.okbHelpDocScope table{width:100%;border-collapse:collapse;margin:10px 0;color:#112a42 !important;}.okbHelpDocScope th,.okbHelpDocScope td{border:1px solid #c7d3df;padding:6px 8px;text-align:left;vertical-align:top;}.okbHelpDocScope th{background:#edf3fa;color:#0f2a43 !important;}.okbHelpDocScope hr{border:none;border-top:1px solid #c7d3df;margin:14px 0;}.okbHelpDocScope .helpImage{display:block;max-width:92%;height:auto;margin:12px auto;border:1px solid #c7d3df;border-radius:8px;background:#fff;}.okbHelpDocScope .helpImageCaption{margin:4px auto 12px auto;text-align:center;font-size:14px;color:#3c5166;}body.night-mode .okbHelpDocScope{color:#dfe8f2;}body.night-mode .okbHelpDocScope,body.night-mode .okbHelpDocScope p,body.night-mode .okbHelpDocScope li,body.night-mode .okbHelpDocScope span,body.night-mode .okbHelpDocScope div{color:#dfe8f2 !important;}body.night-mode .okbHelpDocScope .tab{background:rgba(45,57,72,0.62);border-color:#5f7184;}body.night-mode .okbHelpDocScope h2{border-bottom-color:#5f7184;color:#eef5ff !important;}body.night-mode .okbHelpDocScope h3{color:#d7e8ff !important;}body.night-mode .okbHelpDocScope h4,body.night-mode .okbHelpDocScope h5,body.night-mode .okbHelpDocScope h6{color:#cfe1f7 !important;}body.night-mode .okbHelpDocScope strong,body.night-mode .okbHelpDocScope b{color:#f0f7ff !important;}body.night-mode .okbHelpDocScope em,body.night-mode .okbHelpDocScope i{color:#d2e7ff !important;}body.night-mode .okbHelpDocScope code{background:rgba(52,67,84,0.9);border-color:#5f7184;color:#e7eef8;}body.night-mode .okbHelpDocScope pre{background:rgba(41,54,69,0.9);border-color:#5f7184;color:#e7eef8 !important;}body.night-mode .okbHelpDocScope .controls{background:rgba(49,64,81,0.65);border-left-color:#7fa6ce;}body.night-mode .okbHelpDocScope a{color:#9bc8ff;}body.night-mode .okbHelpDocScope table,body.night-mode .okbHelpDocScope th,body.night-mode .okbHelpDocScope td{border-color:#5f7184;color:#e2ebf6 !important;}body.night-mode .okbHelpDocScope th{background:rgba(57,72,90,0.92);color:#f0f7ff !important;}body.night-mode .okbHelpDocScope hr{border-top-color:#5f7184;}body.night-mode .okbHelpDocScope .helpImage{border-color:#5f7184;background:rgba(35,47,61,0.9);}body.night-mode .okbHelpDocScope .helpImageCaption{color:#bfd5ec;}</style>`;

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

  function normalizeHelpDocSectionId(tab) {
    const token = String(tab || '').trim().toUpperCase();
    switch (token) {
      case 'DTC':
      case 'FLT PLN':
      case 'FLTPLN':
        return 'fltpln';
      case 'ATC':
      case 'WX/ATC':
        return 'atc';
      case 'AWACS':
        return 'awacs';
      case 'JTAC':
        return 'jtac';
      case 'TANKER':
        return 'tanker';
      case 'AOCS':
        return 'aocs';
      case 'FLIGHT':
        return 'flight';
      case 'AI CREW':
      case 'AICREW':
        return 'aicrew';
      case 'GND CREW':
      case 'GNDCREW':
        return 'gndcrew';
      case 'NOTES':
        return 'notes';
      case 'EFB':
        return 'efb';
      case 'LOG':
      default:
        return 'log';
    }
  }

  function extractSectionById(html, sectionId) {
    if (!html || !sectionId) return '';
    const pattern = new RegExp(`<section\\b[^>]*\\bid\\s*=\\s*['\"]${sectionId.replace(/[.*+?^${}()|[\\]\\]/g, '\\$&')}['\"][^>]*>[\\s\\S]*?<\\/section>`, 'i');
    const match = pattern.exec(html);
    return match ? match[0] : '';
  }

  function transformHelpDocFragment(html) {
    return String(html || '').replace(/\/okb\/helpimg\?name=/gi, 'Images/');
  }

  function buildHelpDocTabHtml(tab) {
    try {
      const html = String(baseHelpDocHtml || '');
      if (!html.trim()) {
        return '<p>Help document is empty.</p>';
      }

      const globalSection = extractSectionById(html, 'global');
      const tabSection = extractSectionById(html, normalizeHelpDocSectionId(tab));

      if (!globalSection.trim() && !tabSection.trim()) {
        return '<p>No help content found for this tab.</p>';
      }

      const merged = !globalSection.trim()
        ? (tabSection || '')
        : (!tabSection.trim() ? globalSection : (globalSection + '\n' + tabSection));

      return HELP_DOC_SCOPED_STYLE + "<div class='okbHelpDocScope'>" + transformHelpDocFragment(merged) + '</div>';
    } catch (e) {
      const msg = (e && e.message) ? String(e.message) : 'Unknown error';
      return '<p>Failed to load help content: ' + msg.replace(/[&<>\"]/g, function (c) {
        return c === '&' ? '&amp;' : c === '<' ? '&lt;' : c === '>' ? '&gt;' : '&quot;';
      }) + '</p>';
    }
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

  function normalizeAirportCode(value) {
    return String(value || '').toUpperCase().replace(/[^A-Z0-9]/g, '').slice(0, 8) || 'UNSET';
  }

  function getAirportManifestUrl(airport) {
    return new URL(normalizeAirportCode(airport) + '/manifest.json', CHARTS_ASSETS_ROOT_URL).href;
  }

  function getLocalChartId(airport, relativePath) {
    return 'local:' + normalizeAirportCode(airport) + ':' + encodeURIComponent(String(relativePath || ''));
  }

  function decodeLocalChartId(chartId) {
    const raw = String(chartId || '');
    const marker = 'local:';
    if (raw.toLowerCase().indexOf(marker) !== 0) return null;
    const rest = raw.substring(marker.length);
    const sep = rest.indexOf(':');
    if (sep <= 0) return null;

    const airport = normalizeAirportCode(rest.substring(0, sep));
    let relativePath = rest.substring(sep + 1);
    try { relativePath = decodeURIComponent(relativePath); } catch (_) { }

    relativePath = String(relativePath || '').replace(/\\/g, '/').replace(/^\/+/, '').trim();
    if (!relativePath || relativePath.indexOf('..') >= 0) return null;

    return { airport: airport, relativePath: relativePath };
  }

  function inferChartName(relativePath) {
    const normalized = String(relativePath || '').replace(/\\/g, '/');
    const fileName = normalized.split('/').pop() || normalized;
    return fileName.replace(/\.[^.]+$/, '') || 'Chart';
  }

  function parseAirportManifestCharts(airport, manifest) {
    const source = Array.isArray(manifest)
      ? manifest
      : (Array.isArray(manifest && manifest.charts) ? manifest.charts : []);

    const charts = [];
    for (let i = 0; i < source.length; i++) {
      const item = source[i];
      const file = typeof item === 'string'
        ? item
        : String((item && (item.file || item.path || item.src || item.image || '')) || '').trim();
      if (!file) continue;

      const safeFile = file.replace(/\\/g, '/').replace(/^\/+/, '').trim();
      if (!safeFile || safeFile.indexOf('..') >= 0) continue;

      charts.push({
        id: getLocalChartId(airport, safeFile),
        name: String((item && item.name) || inferChartName(safeFile)),
        type: 'AIRPORT',
        file: safeFile
      });
    }

    return charts;
  }

  async function loadAirportCharts(airport) {
    const key = normalizeAirportCode(airport);
    if (Array.isArray(efbChartManifestByAirport[key])) return efbChartManifestByAirport[key];
    if (efbChartManifestLoadPromiseByAirport[key]) return efbChartManifestLoadPromiseByAirport[key];

    efbChartManifestLoadPromiseByAirport[key] = (async function () {
      try {
        const response = await nativeFetch(getAirportManifestUrl(key), { cache: 'no-store' });
        if (!response.ok) {
          efbChartManifestByAirport[key] = [];
          return efbChartManifestByAirport[key];
        }

        const manifest = await response.json();
        efbChartManifestByAirport[key] = parseAirportManifestCharts(key, manifest);
        return efbChartManifestByAirport[key];
      } catch (_) {
        efbChartManifestByAirport[key] = [];
        return efbChartManifestByAirport[key];
      } finally {
        delete efbChartManifestLoadPromiseByAirport[key];
      }
    })();

    return efbChartManifestLoadPromiseByAirport[key];
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
      baseHelpDocHtml = await loadText(HELP_DOC_URL.href, '');

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
      path === '/okb/efb/chart' || path === '/okb/efb/historytracks' || path === '/okb/dev/servermessages' ||
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
      const airport = normalizeAirportCode(reqUrl.searchParams.get('airport') || '');
      const charts = await loadAirportCharts(airport);
      const payload = { airport: airport, charts: charts.map(function (c) { return { id: c.id, name: c.name, type: c.type }; }) };
      return toJsonResponse(payload);
    }

    if (path === '/okb/efb/chart') {
      const chartId = reqUrl.searchParams.get('id') || '';
      const parsed = decodeLocalChartId(chartId);
      if (!parsed) {
        return new Response('', { status: 404 });
      }

      try {
        const chartUrl = new URL(parsed.relativePath, new URL(parsed.airport + '/', CHARTS_ASSETS_ROOT_URL)).href;
        const chartResponse = await nativeFetch(chartUrl, { cache: 'no-store' });
        if (!chartResponse.ok) {
          return new Response('', { status: 404 });
        }

        const contentType = chartResponse.headers.get('content-type') || 'application/octet-stream';
        const bytes = await chartResponse.arrayBuffer();
        return new Response(bytes, {
          status: 200,
          headers: { 'Content-Type': contentType }
        });
      } catch (_) {
        return new Response('', { status: 404 });
      }
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
      const tab = reqUrl.searchParams.get('tab') || '';
      const payload = buildHelpDocTabHtml(tab);
      return new Response(payload, {
        status: 200,
        headers: { 'Content-Type': 'text/html; charset=utf-8' }
      });
    }

    return nativeFetch(input, init);
  };
})();
