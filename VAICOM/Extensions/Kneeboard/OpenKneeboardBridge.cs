using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace Kneeboard
        {
            public static class OpenKneeboardBridge
            {
                private static readonly object Sync = new object();
                private static OpenKneeboardSnapshot snapshot = new OpenKneeboardSnapshot();
                private static string lastAiCrewCommand = "";
                private static readonly string IndexHtml = @"<!doctype html>
<html>
<head>
  <meta charset='utf-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1'>
  <title>VAICOM Kneeboard 1.0</title>
  <style>
    html, body { width: 100%; height: 100%; margin: 0; }
    body { font-family: Consolas, monospace; background: transparent; color: #151515; letter-spacing: 0.1px; font-size: 23px; }
    .sheet {
      width: 100%;
      height: 100%;
      background: #f1f1ef;
      overflow: hidden;
      box-sizing: border-box;
      border: 1px solid #9aa0a6;
      box-shadow: inset 0 0 0 1px rgba(0, 0, 0, 0.08);
    }
    .sheetBody { height: 100%; overflow: hidden; padding: 8px; box-sizing: border-box; display: flex; flex-direction: column; }
    .headerRow { display:flex; align-items:flex-start; justify-content:space-between; gap:10px; margin-bottom:4px; }
    h3 { margin: 0; font-size: 28px; color: #111; }
    .logo { width: 36px; height: 36px; object-fit: contain; }
    .meta { margin: 2px 0 8px 0; color: #444; font-size: 19px; }
    .status {
      padding: 6px;
      border: 1px solid #8e8e8e;
      background: #ffffff;
      margin-bottom: 8px;
      font-size: 20px;
      color: #111;
    }
    .kneeLayout {
      display: grid;
      grid-template-columns: 1fr 86px;
      gap: 8px;
      align-items: stretch;
      flex: 1 1 auto;
      min-height: 0;
    }
    .leftColumn { display: flex; flex-direction: column; gap: 8px; min-height: 0; }
    .panel {
      border: 1px solid #2a4256;
      background: #ffffff;
    }
    .panel h4 {
      margin: 0;
      padding: 6px 8px;
      border-bottom: 1px solid #29445a;
      color: #000;
      font-size: 18px;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }
    .panel .content { padding: 10px; white-space: pre-wrap; word-break: break-word; overflow: auto; }
    .panel h4.clickable { cursor: pointer; user-select: none; }
    .session { margin-bottom: 8px; }
    .session .content { min-height: 230px; font-size: 22px; overflow: hidden; }
    .tabRail {
      border: 1px solid #9aa0a6;
      background: #ececec;
      padding: 6px 4px;
      height: 100%;
      display: flex;
      min-height: 0;
    }
    .tabs { display: flex; flex-direction: column; gap: 4px; height: 100%; width: 100%; }
    .tab {
      border: 1px solid #7c8692;
      background: rgba(180, 180, 180, 0.78);
      color: #f5f7fa;
      font-family: inherit;
      font-size: 20px;
      padding: 8px 4px;
      cursor: pointer;
      text-align: center;
      font-weight: 600;
      flex: 1 1 0;
      writing-mode: vertical-rl;
      text-orientation: mixed;
      transform: rotate(180deg);
      display: flex;
      align-items: center;
      justify-content: center;
    }
    .tab.active {
      color: #ffffff;
      border-color: #f2d76a;
      border-width: 2px;
      box-shadow: inset 0 0 0 1px rgba(255, 255, 255, 0.22), 0 0 0 2px rgba(242, 215, 106, 0.75), 0 0 10px rgba(242, 215, 106, 0.45);
      filter: saturate(1.22) brightness(1.12);
      font-weight: 800;
      transform: rotate(180deg) scale(1.06);
      z-index: 2;
    }

    .tab-LOG { background: rgba(126, 85, 56, 0.82); }
    .tab-AWACS { background: rgba(63, 108, 136, 0.82); }
    .tab-JTAC { background: rgba(65, 128, 132, 0.82); }
    .tab-ATC { background: rgba(62, 128, 93, 0.82); }
    .tab-TANKER { background: rgba(125, 104, 50, 0.82); }
    .tab-FLIGHT { background: rgba(132, 70, 66, 0.82); }
    .tab-AOCS { background: rgba(112, 62, 143, 0.82); }
    .tab-REF_CREW { background: rgba(130, 129, 71, 0.82); }
    .tab-NOTES { background: rgba(73, 80, 146, 0.82); }
    .tab-AI_CREW { background: rgba(131, 87, 44, 0.82); }

    .tab.active.tab-LOG,
    .tab.active.tab-AWACS,
    .tab.active.tab-JTAC,
    .tab.active.tab-ATC,
    .tab.active.tab-TANKER,
    .tab.active.tab-FLIGHT,
    .tab.active.tab-AOCS,
    .tab.active.tab-REF_CREW,
    .tab.active.tab-NOTES,
    .tab.active.tab-AI_CREW {
      color: #ffffff;
      box-shadow: inset 0 0 0 1px rgba(0, 0, 0, 0.08), 0 0 0 2px rgba(242, 215, 106, 0.75), 0 0 10px rgba(242, 215, 106, 0.45);
    }
    .tabPanel { flex: 0 0 36%; min-height: 250px; display: flex; flex-direction: column; }
    .tabPanel .content { flex: 1 1 auto; min-height: 0; }
    .keywordsPanel { flex: 1 1 auto; min-height: 180px; display: flex; flex-direction: column; }
    .keywordsPanel .content { flex: 1 1 auto; min-height: 0; }
    body.notes-tab .tabPanel { flex: 0 0 55%; }
    body.notes-tab .keywordsPanel { flex: 1 1 auto; min-height: 110px; }
    body.notes-tab .keywordsContent { max-height: 140px; }
    .mainContent { font-size: 22px; line-height: 1.34; }
    .keywordsContent { font-size: 21px; line-height: 1.32; }
    .kwCols { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
    .kwCol { white-space: pre-wrap; word-break: break-word; }
    .controls { margin: 8px 0; color: #222; font-size: 19px; display: flex; gap: 16px; }
    pre { background: #ffffff; border: 1px solid #b7b7b7; padding: 10px; white-space: pre-wrap; word-break: break-word; font-size: 18px; color:#111; max-height: 190px; overflow: auto; }
    body.raw-mode .keywordsPanel { flex: 0 0 280px; }
    .hidden { display: none; }
  </style>
</head>
<body>
  <div class='sheet'>
  <div class='sheetBody'>
    <div class='headerRow'>
      <div>
        <h3>VAICOM KNEEBOARD</h3>
        <div class='meta'>Live export from VAICOM</div>
      </div>
      <img class='logo' src='logo.png' alt='VAICOM Logo'>
    </div>

    <div id='status' class='status'>Loading...</div>

    <div class='kneeLayout'>
      <div class='leftColumn'>
        <div class='panel session'>
          <h4 id='sessionHeader' class='clickable'>Session ▼</h4>
          <div id='session' class='content'></div>
        </div>

        <div class='panel tabPanel'>
          <h4 id='tabTitle'>Tab</h4>
          <div id='tabBody' class='content mainContent'></div>
        </div>

        <div id='keywordPanel' class='panel keywordsPanel'>
          <h4 id='keywordTitle'>Keywords</h4>
          <div id='keywordBody' class='content keywordsContent'></div>
        </div>
      </div>

      <div class='tabRail'>
        <div id='tabs' class='tabs'></div>
      </div>
    </div>

    <div class='controls'>
      <label><input id='autoBrowse' type='checkbox' checked> Auto Browse</label>
      <label id='showRawWrap'><input id='showRaw' type='checkbox'> Show raw JSON</label>
    </div>
    <pre id='json' class='hidden'></pre>
  </div>
  </div>

  <script>
    const TABS = ['LOG','ATC','AWACS','JTAC','TANKER','AOCS','FLIGHT','AI CREW','GND CREW','NOTES'];
    let selectedTab = 'LOG';
    let autoBrowse = true;
    let sessionCollapsed = false;
    let latestData = null;

    function safe(v){ return (v === null || v === undefined || v === '') ? '-' : String(v); }

    function normalizeCategory(cat){
      var c = String(cat || '').toUpperCase();
      if (c === 'RIO' || c === 'ICEMAN' || c === 'WSO' || c === 'GEORGE' || c === 'CPG' || c === 'AICPG' || c === 'AIWSO') return 'AI CREW';
      if (c === 'REF' || c === 'CREW' || c === 'REF/CREW') return 'GND CREW';
      if (c === 'ALLIES') return 'FLIGHT';
      return c;
    }

    function mergeUnique(dest, src){
      (src || []).forEach(function(v){
        if (dest.indexOf(v) < 0) dest.push(v);
      });
    }

    function tabCssClass(tab){
      return 'tab-' + String(tab || '').replace(/[^A-Za-z0-9]+/g, '_');
    }

    function getMergedLog(map, tab){
      const lines = [];
      if (!map) return '';
      Object.keys(map).forEach(function(k){
        if (normalizeCategory(k) !== tab) return;
        const text = String(map[k] || '').trim();
        if (!text) return;
        if (lines.indexOf(text) < 0) lines.push(text);
      });
      return lines.join('\n');
    }

    function getMergedList(map, tab){
      const result = [];
      if (!map) return result;
      Object.keys(map).forEach(function(k){
        if (normalizeCategory(k) !== tab) return;
        mergeUnique(result, Array.isArray(map[k]) ? map[k] : []);
      });
      return result;
    }

    function getAiCrewCategories(data){
      const server = (data && data.Server) || {};
      const aircraft = String(server.Aircraft || '').toUpperCase();

      if (aircraft.indexOf('F-14') >= 0) return ['RIO','ICEMAN','AI CREW','REF','CREW','REF/CREW','GND CREW'];
      if (aircraft.indexOf('F-4') >= 0) return ['WSO','AIWSO','AI CREW','REF','CREW','REF/CREW','GND CREW'];
      if (aircraft.indexOf('AH-64') >= 0 || aircraft.indexOf('AH64') >= 0) return ['GEORGE','CPG','AICPG','AI CREW','REF','CREW','REF/CREW','GND CREW'];

      return ['RIO','ICEMAN','WSO','GEORGE','CPG','AICPG','AIWSO','AI CREW','REF','CREW','REF/CREW','GND CREW'];
    }

    function getF4GroundCrewKeywords(){
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

    function getMergedLogByCategories(map, categories){
      const lines = [];
      if (!map) return '';
      const allowed = categories.map(function(c){ return String(c).toUpperCase(); });

      Object.keys(map).forEach(function(k){
        const key = String(k || '').toUpperCase();
        const normalized = normalizeCategory(key).toUpperCase();
        if (allowed.indexOf(key) < 0 && allowed.indexOf(normalized) < 0) return;
        const text = String(map[k] || '').trim();
        if (!text) return;
        if (lines.indexOf(text) < 0) lines.push(text);
      });

      return lines.join('\n');
    }

    function getMergedAliasesByCategories(chunkMap, categories){
      const result = {};
      if (!chunkMap) return result;
      const allowed = categories.map(function(c){ return String(c).toUpperCase(); });

      Object.keys(chunkMap).forEach(function(k){
        const key = String(k || '').toUpperCase();
        const normalized = normalizeCategory(key).toUpperCase();
        if (allowed.indexOf(key) < 0 && allowed.indexOf(normalized) < 0) return;
        const aliasObj = chunkMap[k] || {};
        Object.keys(aliasObj).forEach(function(a){
          if (!result[a]) result[a] = [];
          mergeUnique(result[a], Array.isArray(aliasObj[a]) ? aliasObj[a] : []);
        });
      });

      return result;
    }

    function getMergedAliases(chunkMap, tab){
      const result = {};
      if (!chunkMap) return result;
      Object.keys(chunkMap).forEach(function(k){
        if (normalizeCategory(k) !== tab) return;
        const aliasObj = chunkMap[k] || {};
        Object.keys(aliasObj).forEach(function(a){
          if (!result[a]) result[a] = [];
          mergeUnique(result[a], Array.isArray(aliasObj[a]) ? aliasObj[a] : []);
        });
      });
      return result;
    }

    function getKeywordPhrasesForTab(data, tab){
      if (tab === 'AI CREW'){
        const direct = Array.isArray(data.AiCrewKeywords) ? data.AiCrewKeywords.slice() : [];
        const cleaned = [];
        direct.forEach(function(k){
          const phrase = String(k || '').replace(/\s+/g, ' ').trim();
          if (!phrase) return;
          if (cleaned.indexOf(phrase) < 0) cleaned.push(phrase);
        });
        cleaned.sort(function(a,b){ return a.localeCompare(b); });
        return cleaned;
      }

      const phrases = [];

      function pushPhrase(p){
        const phrase = String(p || '').replace(/\s+/g, ' ').trim();
        if (!phrase) return;
        if (phrases.indexOf(phrase) < 0) phrases.push(phrase);
      }

      function collectFromChunk(chunkMap){
        const alias = tab === 'AI CREW'
          ? getMergedAliasesByCategories(chunkMap, getAiCrewCategories(data))
          : getMergedAliases(chunkMap, tab);
        const keys = Object.keys(alias);
        keys.forEach(function(k){
          const vals = (alias[k] || []).filter(function(v){ return String(v || '').trim() !== ''; });
          if (!vals.length){
            pushPhrase(k);
            return;
          }

          vals.forEach(function(v){
            pushPhrase(k + ' ' + v);
          });
        });
      }

      collectFromChunk(data.AliasesChunk0);
      collectFromChunk(data.AliasesChunk1);

      if (tab === 'AI CREW' && !phrases.length){
        const fallback = getKeywordPhrasesForTab(data, 'GND CREW');
        fallback.forEach(function(p){ pushPhrase(p); });
      }

      if (tab === 'GND CREW'){
        const server = (data && data.Server) || {};
        const aircraft = String(server.Aircraft || '').toUpperCase();
        if (aircraft.indexOf('F-4') >= 0){
          getF4GroundCrewKeywords().forEach(function(k){ pushPhrase(k); });
        }
      }

      if (tab === 'NOTES'){
        pushPhrase('Start Dictate');
        pushPhrase('End Dictate');
        pushPhrase('Clear Notes');
      }

      if (tab === 'GND CREW'){
        const filtered = [];
        phrases.forEach(function(p){
          if (!/^George\s/i.test(p)) filtered.push(p);
        });
        filtered.sort(function(a,b){ return a.localeCompare(b); });
        return filtered;
      }

      phrases.sort(function(a,b){ return a.localeCompare(b); });
      return phrases;
    }

    function formatKeywordReference(data, tab){
      if (tab === 'LOG') return 'No keyword reference for this tab.';
      const phrases = getKeywordPhrasesForTab(data, tab);
      if (!phrases.length) return 'No keywords for this tab yet.';
      return phrases.join('\n');
    }

    function escapeHtml(text){
      return String(text || '')
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
    }

    function formatKeywordReferenceHtml(data, tab){
      const text = formatKeywordReference(data, tab);
      if (text === 'No keyword reference for this tab.' || text === 'No keywords for this tab yet.'){
        return escapeHtml(text);
      }

      const rows = text.split('\n').filter(function(x){ return String(x).trim() !== ''; });
      if (!rows.length){
        return 'No keywords for this tab yet.';
      }

      const mid = Math.ceil(rows.length / 2);
      const left = rows.slice(0, mid).map(escapeHtml).join('<br>');
      const right = rows.slice(mid).map(escapeHtml).join('<br>');

      return '<div class=""kwCols""><div class=""kwCol"">' + left + '</div><div class=""kwCol"">' + right + '</div></div>';
    }

    function formatTabContent(data, tab){
      const server = (data && data.Server) || {};
      const parts = [];

      if (tab === 'LOG'){
        const briefing = String(server.MissionBriefing || '').trim();
        const details = String(server.MissionDetails || '').trim();
        if (briefing){ parts.push('Mission Briefing:\n' + briefing); }
        if (details){ parts.push('Mission Details:\n' + details); }
        const log = getMergedLog(data.Logs, tab);
        if (log){ parts.push('Log:\n' + log); }
        return parts.length ? parts.join('\n\n') : 'No log data yet.';
      }

      if (tab === 'NOTES'){
        const notes = String(data.NotesBuffer || '').trim();
        return notes || 'No notes yet.';
      }

      if (tab === 'AI CREW'){
        const aiCrewLog = getMergedLogByCategories(data.Logs, getAiCrewCategories(data));
        const synthetic = [];
        if (aiCrewLog) synthetic.push(aiCrewLog);
        if (data.ActiveCategory === 'AI CREW'){
          synthetic.push('Active AI crew interaction detected.');
        }
        const combined = synthetic.join('\n');
        return combined ? ('Log:\n' + combined) : 'No AI CREW log data yet.';
      }

      const log = getMergedLog(data.Logs, tab);
      const units = getMergedList(data.Units, tab);
      const details = getMergedList(data.UnitDetails, tab);
      if (log) parts.push('Log:\n' + log);
      if (units.length) parts.push('Units:\n' + units.map(function(u){ return '  ' + u; }).join('\n'));
      if (details.length) parts.push('Unit Details:\n' + details.map(function(u){ return '  ' + u; }).join('\n'));

      return parts.length ? parts.join('\n\n') : 'No data for this tab yet.';
    }

    function renderTabs(data){
      const tabsEl = document.getElementById('tabs');
      tabsEl.innerHTML = '';
      const active = normalizeCategory(data.ActiveCategory);
      if (autoBrowse && TABS.indexOf(active) >= 0){
        selectedTab = active;
      }
      if (TABS.indexOf(selectedTab) < 0) selectedTab = 'LOG';

      TABS.forEach(function(tab){
        const btn = document.createElement('button');
        btn.className = 'tab ' + tabCssClass(tab) + (tab === selectedTab ? ' active' : '');
        btn.textContent = tab;
        btn.onclick = function(){
          selectedTab = tab;
          autoBrowse = false;
          document.getElementById('autoBrowse').checked = false;
          if (latestData) render(latestData);
        };
        tabsEl.appendChild(btn);
      });
    }

    function render(data){
      latestData = data;
      const server = data && data.Server ? data.Server : {};
      const haveMission = !!(server.Aircraft || server.MissionTitle || server.Theater);
      const debugMode = !!server.DebugMode;

      document.getElementById('status').textContent = haveMission
        ? 'Live session detected.'
        : 'Waiting for mission data...';

      document.getElementById('session').textContent = [
        'Active Category : ' + safe(normalizeCategory(data.ActiveCategory)),
        'Updated (UTC)   : ' + safe(data.UpdatedUtc),
        '',
        'Theater         : ' + safe(server.Theater),
        'DCS Location    : ' + safe(server.DcsLocation || server.DcsVersion),
        'Aircraft        : ' + safe(server.Aircraft),
        'Callsign        : ' + safe(server.PlayerCallsign),
        'Mission         : ' + safe(server.MissionTitle),
        'Multiplayer     : ' + (server.Multiplayer ? 'Yes' : 'No')
      ].join('\n');

      renderTabs(data);
      document.body.classList.toggle('notes-tab', selectedTab === 'NOTES');
      document.getElementById('tabTitle').textContent = 'Tab: ' + selectedTab;
      document.getElementById('tabBody').textContent = formatTabContent(data, selectedTab);
      document.getElementById('keywordTitle').textContent = 'Keywords: ' + selectedTab;
      document.getElementById('keywordBody').innerHTML = formatKeywordReferenceHtml(data, selectedTab);

      const showRawWrap = document.getElementById('showRawWrap');
      if (showRawWrap){
        showRawWrap.style.display = debugMode ? 'inline-flex' : 'none';
      }
      if (!debugMode){
        const showRawBox = document.getElementById('showRaw');
        if (showRawBox) showRawBox.checked = false;
        document.body.classList.remove('raw-mode');
        document.getElementById('json').className = 'hidden';
      }

      document.getElementById('json').textContent = JSON.stringify(data, null, 2);
    }

    async function configureOpenKneeboard(){
      try {
        var okb = (typeof OpenKneeboard !== 'undefined') ? OpenKneeboard : window.OpenKneeboard;
        if (okb && okb.SetPreferredPixelSize) {
          await okb.SetPreferredPixelSize(1050, 1480);
          setTimeout(function(){ okb.SetPreferredPixelSize(1050, 1480); }, 300);
          setTimeout(function(){ okb.SetPreferredPixelSize(1050, 1480); }, 1200);
        }
      } catch (e) {
      }
    }

    async function tick(){
      try{
        const r = await fetch('state', { cache: 'no-store' });
        const j = await r.json();
        render(j);
      }catch(e){
        document.getElementById('status').textContent = 'Waiting for VAICOM connection...';
      }
    }

    document.getElementById('showRaw').addEventListener('change', function(ev){
      document.body.classList.toggle('raw-mode', ev.target.checked);
      document.getElementById('json').className = ev.target.checked ? '' : 'hidden';
    });

    document.getElementById('autoBrowse').addEventListener('change', function(ev){
      autoBrowse = ev.target.checked;
      if (latestData) render(latestData);
    });

    document.getElementById('sessionHeader').addEventListener('click', function(){
      sessionCollapsed = !sessionCollapsed;
      document.getElementById('session').style.display = sessionCollapsed ? 'none' : 'block';
      document.getElementById('sessionHeader').textContent = sessionCollapsed ? 'Session ►' : 'Session ▼';
    });

    configureOpenKneeboard();
    tick();
    setInterval(tick, 1000);
  </script>
</body>
</html>";

                private static HttpListener listener;
                private static Thread listenerThread;
                private static bool isRunning;

                private const string Prefix = "http://127.0.0.1:7779/okb/";

                public static void Initialize()
                {
                    ResetSnapshot();
                    StartWebHost();
                }

                public static void SetEnabled(bool enabled)
                {
                    if (State.activeconfig.OpenKneeboard_Out)
                    {
                        StartWebHost();
                    }
                    else
                    {
                        StopWebHost();
                    }
                }

                public static void Shutdown()
                {
                    StopWebHost();
                }

                public static void ResetSnapshot()
                {
                    lock (Sync)
                    {
                        snapshot = new OpenKneeboardSnapshot();
                    }
                }

                public static void UpdateActiveCategory(string category)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    lock (Sync)
                    {
                        snapshot.ActiveCategory = category.ToUpperInvariant();
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void UpdateLog(string category, string content)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    lock (Sync)
                    {
                        string key = category.ToUpperInvariant();
                        string nextEntry = NormalizeLogEntry(content);

                        if (string.IsNullOrWhiteSpace(nextEntry))
                        {
                            return;
                        }

                        List<string> entries = new List<string>();
                        if (snapshot.Logs.TryGetValue(key, out string existing) && !string.IsNullOrWhiteSpace(existing))
                        {
                            entries.AddRange(existing
                                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(e => e.Trim())
                                .Where(e => !string.IsNullOrWhiteSpace(e)));
                        }

                        entries.Add(nextEntry);
                        if (entries.Count > 4)
                        {
                            if (!key.Equals("NOTES", StringComparison.OrdinalIgnoreCase))
                            {
                                entries = entries.Skip(entries.Count - 4).ToList();
                            }
                        }

                        snapshot.Logs[key] = string.Join("\n", entries);
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void SetLastAiCrewCommand(string commandText)
                {
                    lock (Sync)
                    {
                        lastAiCrewCommand = NormalizeLogEntry(commandText);
                    }
                }

                public static string BuildAiCrewResponseEntry(string role, string response)
                {
                    lock (Sync)
                    {
                        string responseText = NormalizeLogEntry(response);
                        if (string.IsNullOrWhiteSpace(responseText))
                        {
                            return "";
                        }

                        string commandText = NormalizeLogEntry(lastAiCrewCommand);
                        if (!string.IsNullOrWhiteSpace(commandText))
                        {
                            return commandText + " -> " + responseText;
                        }

                        string roleText = string.IsNullOrWhiteSpace(role) ? "AI CREW" : role;
                        return roleText + " | " + responseText;
                    }
                }

                private static string NormalizeLogEntry(string content)
                {
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        return "";
                    }

                    return content
                        .Replace("\r", " ")
                        .Replace("\n", " ")
                        .Trim();
                }

                public static void UpdateUnits(string category, List<string> units)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    lock (Sync)
                    {
                        snapshot.Units[category.ToUpperInvariant()] = units == null ? new List<string>() : new List<string>(units);
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void UpdateUnitsDetails(string category, List<string> details)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    lock (Sync)
                    {
                        snapshot.UnitDetails[category.ToUpperInvariant()] = details == null ? new List<string>() : new List<string>(details);
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void UpdateAliasChunk(string category, int chunk, SortedDictionary<string, List<string>> aliasStrings)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    lock (Sync)
                    {
                        Dictionary<string, Dictionary<string, List<string>>> chunkMap = chunk == 0 ? snapshot.AliasesChunk0 : snapshot.AliasesChunk1;
                        chunkMap[category.ToUpperInvariant()] = CloneAliasDictionary(aliasStrings);
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void UpdateNotesBuffer(string notes)
                {
                    lock (Sync)
                    {
                        snapshot.NotesBuffer = notes ?? "";
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void UpdateServerData()
                {
                    lock (Sync)
                    {
                        snapshot.Server = BuildServerSnapshot();
                        snapshot.AiCrewKeywords = BuildAiCrewKeywords();
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                private static List<string> BuildAiCrewKeywords()
                {
                    try
                    {
                        string moduleId = State.currentmodule?.Id ?? string.Empty;
                        HashSet<string> keywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                        if (moduleId.StartsWith("F-14", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (string key in Extensions.RIO.Aliases.aicommands.Keys)
                            {
                                keywords.Add(key);
                            }
                        }
                        else if (moduleId.Equals("F-4E-45MC", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (string key in Extensions.WSO.Aliases.aicommands.Keys)
                            {
                                keywords.Add(key);
                            }
                        }
                        else if (moduleId.StartsWith("AH-64D", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (string key in Database.Aliases.aicommands.Keys)
                            {
                                if (key.StartsWith("George ", StringComparison.OrdinalIgnoreCase))
                                {
                                    keywords.Add(key);
                                }
                            }
                        }

                        return keywords
                            .Where(k => !string.IsNullOrWhiteSpace(k))
                            .OrderBy(k => k, StringComparer.OrdinalIgnoreCase)
                            .ToList();
                    }
                    catch
                    {
                        return new List<string>();
                    }
                }

                private static void StartWebHost()
                {
                    if (isRunning || !State.activeconfig.OpenKneeboard_Out)
                    {
                        return;
                    }

                    try
                    {
                        listener = new HttpListener();
                        listener.Prefixes.Add(Prefix);
                        listener.Start();

                        isRunning = true;
                        listenerThread = new Thread(ListenLoop) { IsBackground = true, Name = "OpenKneeboardWebHost" };
                        listenerThread.Start();

                        Log.Write("OpenKneeboard dashboard host started at " + Prefix, Colors.Inline);
                    }
                    catch (Exception ex)
                    {
                        isRunning = false;
                        Log.Write("OpenKneeboard dashboard host failed to start: " + ex.Message, Colors.Warning);
                    }
                }

                private static void StopWebHost()
                {
                    if (!isRunning)
                    {
                        return;
                    }

                    try
                    {
                        isRunning = false;
                        listener?.Stop();
                        listener?.Close();
                        listener = null;
                    }
                    catch
                    {
                    }
                }

                private static void ListenLoop()
                {
                    while (isRunning && listener != null)
                    {
                        try
                        {
                            HttpListenerContext context = listener.GetContext();
                            HandleContext(context);
                        }
                        catch (HttpListenerException)
                        {
                            break;
                        }
                        catch (ObjectDisposedException)
                        {
                            break;
                        }
                        catch (Exception ex)
                        {
                            Log.Write("OpenKneeboard dashboard request error: " + ex.Message, Colors.Warning);
                        }
                    }
                }

                private static void HandleContext(HttpListenerContext context)
                {
                    string path = context.Request.Url.AbsolutePath.ToLowerInvariant();

                    if (path == "/okb")
                    {
                        context.Response.StatusCode = 302;
                        context.Response.RedirectLocation = "/okb/";
                        context.Response.Close();
                        return;
                    }

                    if (path == "/okb/" || path == "/okb/index.html")
                    {
                        WriteText(context.Response, IndexHtml, "text/html; charset=utf-8");
                        return;
                    }

                    if (path == "/okb/state" || path == "/okb/index.json")
                    {
                        WriteJson(context.Response, BuildSnapshotJson());
                        return;
                    }

                    if (path == "/okb/logo.png")
                    {
                        byte[] logo = GetLogoBytes();
                        if (logo != null && logo.Length > 0)
                        {
                            WriteBinary(context.Response, logo, "image/png");
                        }
                        else
                        {
                            context.Response.StatusCode = 404;
                            context.Response.Close();
                        }
                        return;
                    }

                    context.Response.StatusCode = 404;
                    context.Response.Close();
                }

                private static string BuildSnapshotJson()
                {
                    OpenKneeboardSnapshot responseModel;

                    lock (Sync)
                    {
                        responseModel = snapshot.Clone();
                    }

                    return JsonConvert.SerializeObject(responseModel, Formatting.Indented);
                }

                private static void WriteJson(HttpListenerResponse response, string payload)
                {
                    WriteText(response, payload, "application/json; charset=utf-8");
                }

                private static void WriteText(HttpListenerResponse response, string payload, string contentType)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(payload ?? "");
                    response.ContentType = contentType;
                    response.ContentEncoding = Encoding.UTF8;
                    response.ContentLength64 = bytes.Length;
                    response.OutputStream.Write(bytes, 0, bytes.Length);
                    response.OutputStream.Close();
                }

                private static void WriteBinary(HttpListenerResponse response, byte[] payload, string contentType)
                {
                    byte[] bytes = payload ?? Array.Empty<byte>();
                    response.ContentType = contentType;
                    response.ContentLength64 = bytes.Length;
                    response.OutputStream.Write(bytes, 0, bytes.Length);
                    response.OutputStream.Close();
                }

                private static byte[] GetLogoBytes()
                {
                    try
                    {
                        Uri uri = new Uri("pack://application:,,,/VAICOMPRO;component/Resources/Images/VAICOMPRO logo icon 5 128.png", UriKind.Absolute);
                        var resourceInfo = Application.GetResourceStream(uri);
                        if (resourceInfo != null && resourceInfo.Stream != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                resourceInfo.Stream.CopyTo(ms);
                                return ms.ToArray();
                            }
                        }
                    }
                    catch
                    {
                    }

                    return null;
                }

                private static Dictionary<string, List<string>> CloneAliasDictionary(SortedDictionary<string, List<string>> source)
                {
                    Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

                    if (source == null)
                    {
                        return result;
                    }

                    foreach (KeyValuePair<string, List<string>> pair in source)
                    {
                        result[pair.Key] = pair.Value == null ? new List<string>() : new List<string>(pair.Value);
                    }

                    return result;
                }

                private static OpenKneeboardServerSnapshot BuildServerSnapshot()
                {
                    OpenKneeboardServerSnapshot server = new OpenKneeboardServerSnapshot();

                    try
                    {
                        if (State.currentstate != null)
                        {
                            server.Theater = State.currentstate.theatre;
                            server.DcsLocation = !string.IsNullOrWhiteSpace(State.currentstate.dcsversion)
                                ? State.currentstate.dcsversion
                                : (!string.IsNullOrWhiteSpace(State.currentstate.clientversion)
                                    ? State.currentstate.clientversion
                                    : State.currentstate.root);
                            server.Aircraft = State.currentstate.id;
                            server.PlayerCallsign = State.currentstate.playercallsign;
                            server.MissionTitle = State.currentstate.missiontitle;
                            server.MissionBriefing = State.currentstate.missionbriefing;
                            server.MissionDetails = State.currentstate.missiondetails;
                            server.Multiplayer = State.currentstate.multiplayer;
                            server.DebugMode = State.activeconfig.Debugmode;
                        }
                    }
                    catch
                    {
                    }

                    return server;
                }
            }

            public class OpenKneeboardSnapshot
            {
                public string ActiveCategory { get; set; } = "LOG";
                public string NotesBuffer { get; set; } = "";
                public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
                public OpenKneeboardServerSnapshot Server { get; set; } = new OpenKneeboardServerSnapshot();
                public List<string> AiCrewKeywords { get; set; } = new List<string>();
                public Dictionary<string, string> Logs { get; set; } = new Dictionary<string, string>();
                public Dictionary<string, List<string>> Units { get; set; } = new Dictionary<string, List<string>>();
                public Dictionary<string, List<string>> UnitDetails { get; set; } = new Dictionary<string, List<string>>();
                public Dictionary<string, Dictionary<string, List<string>>> AliasesChunk0 { get; set; } = new Dictionary<string, Dictionary<string, List<string>>>();
                public Dictionary<string, Dictionary<string, List<string>>> AliasesChunk1 { get; set; } = new Dictionary<string, Dictionary<string, List<string>>>();

                public OpenKneeboardSnapshot Clone()
                {
                    OpenKneeboardSnapshot clone = new OpenKneeboardSnapshot
                    {
                        ActiveCategory = ActiveCategory,
                        NotesBuffer = NotesBuffer,
                        UpdatedUtc = UpdatedUtc,
                        Server = Server == null ? new OpenKneeboardServerSnapshot() : Server.Clone(),
                        AiCrewKeywords = new List<string>(AiCrewKeywords ?? new List<string>()),
                        Logs = new Dictionary<string, string>(Logs),
                        Units = CloneListMap(Units),
                        UnitDetails = CloneListMap(UnitDetails),
                        AliasesChunk0 = CloneAliasMap(AliasesChunk0),
                        AliasesChunk1 = CloneAliasMap(AliasesChunk1),
                    };

                    return clone;
                }

                private static Dictionary<string, List<string>> CloneListMap(Dictionary<string, List<string>> source)
                {
                    Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

                    foreach (KeyValuePair<string, List<string>> pair in source)
                    {
                        result[pair.Key] = pair.Value == null ? new List<string>() : new List<string>(pair.Value);
                    }

                    return result;
                }

                private static Dictionary<string, Dictionary<string, List<string>>> CloneAliasMap(Dictionary<string, Dictionary<string, List<string>>> source)
                {
                    Dictionary<string, Dictionary<string, List<string>>> result = new Dictionary<string, Dictionary<string, List<string>>>();

                    foreach (KeyValuePair<string, Dictionary<string, List<string>>> categoryPair in source)
                    {
                        Dictionary<string, List<string>> categoryValues = new Dictionary<string, List<string>>();
                        foreach (KeyValuePair<string, List<string>> aliasPair in categoryPair.Value)
                        {
                            categoryValues[aliasPair.Key] = aliasPair.Value == null ? new List<string>() : new List<string>(aliasPair.Value);
                        }

                        result[categoryPair.Key] = categoryValues;
                    }

                    return result;
                }
            }

            public class OpenKneeboardServerSnapshot
            {
                public string Theater { get; set; } = "";
                public string DcsLocation { get; set; } = "";
                public string Aircraft { get; set; } = "";
                public string PlayerCallsign { get; set; } = "";
                public string MissionTitle { get; set; } = "";
                public string MissionBriefing { get; set; } = "";
                public string MissionDetails { get; set; } = "";
                public bool Multiplayer { get; set; }
                public bool DebugMode { get; set; }

                public OpenKneeboardServerSnapshot Clone()
                {
                    return new OpenKneeboardServerSnapshot
                    {
                        Theater = Theater,
                        DcsLocation = DcsLocation,
                        Aircraft = Aircraft,
                        PlayerCallsign = PlayerCallsign,
                        MissionTitle = MissionTitle,
                        MissionBriefing = MissionBriefing,
                        MissionDetails = MissionDetails,
                        Multiplayer = Multiplayer,
                        DebugMode = DebugMode,
                    };
                }
            }
        }
    }
}
