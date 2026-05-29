using Newtonsoft.Json;
using Microsoft.Win32;
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
                private static readonly object RawServerLogSync = new object();
                private static OpenKneeboardSnapshot snapshot = new OpenKneeboardSnapshot();
                private static string lastAiCrewCommand = "";
                private static bool captureRawServerMessages;
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
      display: flex;
      align-items: center;
      gap: 8px;
    }
    .statusIndicator { width: 12px; height: 12px; border: 1px solid #666; background: #9aa0a6; flex: 0 0 12px; }
    .status.status-error .statusIndicator { background: #c73a36; border-color: #8d201d; }
    .status.status-warning .statusIndicator { background: #d18627; border-color: #8b5719; }
    .status.status-sent .statusIndicator { background: #2e8b57; border-color: #1c5c39; }
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
    #tabTitle, #keywordTitle { font-size: 23px; }
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
    .tabKeywordDivider {
      height: 8px;
      border: 1px solid #7e8fa1;
      background: linear-gradient(to bottom, #d9dde2, #c6ccd2);
      cursor: row-resize;
      flex: 0 0 8px;
      display: flex;
      align-items: center;
      justify-content: center;
      user-select: none;
    }
    .tabKeywordDivider::before {
      content: '';
      width: 40px;
      height: 2px;
      background: #667788;
      box-shadow: 0 -2px 0 #92a1af, 0 2px 0 #92a1af;
    }
    .keywordsPanel { flex: 1 1 auto; min-height: 180px; display: flex; flex-direction: column; }
    .keywordsPanel .content { flex: 1 1 auto; min-height: 0; }
    body.notes-tab .tabPanel { flex: 0 0 55%; }
    body.notes-tab .keywordsPanel { flex: 1 1 auto; min-height: 110px; }
    body.notes-tab .keywordsContent { max-height: 140px; }
    .mainContent { font-size: 24px; line-height: 1.32; }
    .keywordsContent { font-size: 24px; line-height: 1.32; }
    .kwCols { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
    .kwCol { white-space: pre-wrap; word-break: break-word; }
    .controls { margin: 8px 0; color: #222; font-size: 19px; display: flex; gap: 10px; flex-wrap: wrap; align-items: center; }
    .controls label { white-space: nowrap; }
    .controls button {
      font-family: inherit;
      font-size: 18px;
      padding: 3px 8px;
      border: 1px solid #7c8692;
      background: #ffffff;
      color: #111;
      cursor: pointer;
    }
    .controls button.draw-on {
      background: #2e8b57;
      border-color: #1c5c39;
      color: #ffffff;
      font-weight: 700;
    }
    .controls button:disabled {
      opacity: 0.6;
      cursor: default;
    }
    .drawTimer {
      min-width: 62px;
      font-size: 17px;
      color: #2e8b57;
      font-weight: 700;
    }
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

    <div id='status' class='status'><span id='statusIndicator' class='statusIndicator'></span><span id='statusText'>Loading...</span></div>

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

        <div id='tabKeywordDivider' class='tabKeywordDivider' title='Drag to resize Tab and Keywords'></div>

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
      <button id='drawModeToggle' type='button'>Draw OFF</button>
      <span id='drawTimer' class='drawTimer hidden'>30s</span>
      <label id='showRawWrap'><input id='showRaw' type='checkbox'> Show raw JSON</label>
      <label id='showServerWrap'><input id='showServer' type='checkbox'> Show server messages</label>
    </div>
    <pre id='json' class='hidden'></pre>
    <pre id='serverMessages' class='hidden'></pre>
  </div>
  </div>

  <script>
    const TABS = ['LOG','ATC','AWACS','JTAC','TANKER','AOCS','FLIGHT','AI CREW','GND CREW','NOTES'];
    const OKB_TAB_TYPE_ID = 'VAICOM-Community;okb-out';
    const OKB_CUSTOM_ACTION_PREFIX = OKB_TAB_TYPE_ID + ';';
    const OKB_ACTION_TAB_PREV = OKB_CUSTOM_ACTION_PREFIX + 'tab-prev';
    const OKB_ACTION_TAB_NEXT = OKB_CUSTOM_ACTION_PREFIX + 'tab-next';
    const OKB_ACTION_TAB_SELECT = OKB_CUSTOM_ACTION_PREFIX + 'tab-select';
    let selectedTab = 'LOG';
    let autoBrowse = true;
    let sessionCollapsed = false;
    let latestData = null;
    let showServerMessages = false;
    let metarPressureInHg = false;
    let selectedAtcMetarKey = '';
    let okbExperimentalEnabled = false;
    let okbCursorMode = '';
    let okbDoodlesOnlyForced = false;
    let drawModeEnabled = false;
    let drawInteractionInNotes = false;
    let drawModeDisableTimer = null;
    let drawModeDeadlineUtcMs = 0;
    let drawModeCountdownTimer = null;
    let customActionHandlerRegistered = false;
    const sessionCollapsedStorageKey = 'vaicom.okb.sessionCollapsed';
    const tabKeywordsSplitStorageKey = 'vaicom.okb.tabKeywordsSplitByTab';
    const drawModeStorageKey = 'vaicom.okb.notesDrawMode';
    const drawModeTimeoutMs = 30000;
    let tabKeywordsSplitByTab = {};

    function clamp(v, min, max){
      return Math.max(min, Math.min(max, v));
    }

    function safe(v){ return (v === null || v === undefined || v === '') ? '-' : String(v); }

    function formatUtcToSeconds(v){
      if (v === null || v === undefined || v === '') return '-';
      const d = new Date(v);
      if (!isFinite(d.getTime())) return String(v);
      return d.toISOString().replace(/\.\d{3}Z$/, 'Z');
    }

    function normalizeCategory(cat){
      var c = String(cat || '').toUpperCase();
      if (c === 'RIO' || c === 'ICEMAN' || c === 'WSO' || c === 'GEORGE' || c === 'CPG' || c === 'AICPG' || c === 'AIWSO') return 'AI CREW';
      if (c === 'REF' || c === 'CREW' || c === 'REF/CREW') return 'GND CREW';
      if (c === 'ALLIES') return 'FLIGHT';
      return c;
    }

    function normalizeActiveCategory(cat, data){
      var c = String(cat || '').toUpperCase();
      return normalizeCategory(c);
    }

    function setSelectedTab(tab){
      var normalized = normalizeCategory(tab);
      if (normalized === 'WX/ATC') normalized = 'ATC';
      if (TABS.indexOf(normalized) < 0) return false;

      selectedTab = normalized;
      autoBrowse = false;
      const autoBrowseEl = document.getElementById('autoBrowse');
      if (autoBrowseEl) autoBrowseEl.checked = false;
      if (latestData) render(latestData);
      return true;
    }

    function selectRelativeTab(step){
      const idx = TABS.indexOf(selectedTab);
      const currentIndex = idx >= 0 ? idx : 0;
      const delta = step >= 0 ? 1 : -1;
      const nextIndex = (currentIndex + delta + TABS.length) % TABS.length;
      setSelectedTab(TABS[nextIndex]);
    }

    function getTabFromCustomExtraData(extraData){
      if (extraData === null || extraData === undefined) return '';
      if (typeof extraData === 'string') return extraData;
      if (typeof extraData === 'number' && isFinite(extraData)) {
        const idx = ((Math.floor(extraData) % TABS.length) + TABS.length) % TABS.length;
        return TABS[idx];
      }
      if (typeof extraData !== 'object') return '';

      if (typeof extraData.tab === 'string') return extraData.tab;
      if (typeof extraData.category === 'string') return extraData.category;
      if (typeof extraData.name === 'string') return extraData.name;

      if (typeof extraData.index === 'number' && isFinite(extraData.index)) {
        const idx = ((Math.floor(extraData.index) % TABS.length) + TABS.length) % TABS.length;
        return TABS[idx];
      }

      return '';
    }

    function handleCustomActionEvent(ev){
      const detail = (ev && ev.detail) ? ev.detail : {};
      const id = String((detail && detail.id) || '');
      const extraData = detail ? detail.extraData : undefined;

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

    function registerCustomActionHandlers(){
      if (customActionHandlerRegistered) return;

      const okb = (typeof OpenKneeboard !== 'undefined') ? OpenKneeboard : window.OpenKneeboard;
      const targets = [okb, window];
      for (let i = 0; i < targets.length; i++){
        const t = targets[i];
        if (!t || !t.addEventListener) continue;
        try{
          t.addEventListener('plugin/tab/customAction', handleCustomActionEvent);
          customActionHandlerRegistered = true;
        }catch(_){
        }
      }
    }

    function mergeUnique(dest, src){
      (src || []).forEach(function(v){
        if (dest.indexOf(v) < 0) dest.push(v);
      });
    }

    function tabCssClass(tab){
      return 'tab-' + String(tab || '').replace(/[^A-Za-z0-9]+/g, '_');
    }

    function tabLabel(tab){
      return tab === 'ATC' ? 'WX/ATC' : tab;
    }

    function formatAiCrewPhaseLabel(phase){
      const text = String(phase || '').trim();
      if (!text || text.toLowerCase() === 'unknown') return '';
      return ' (' + text + ')';
    }

    async function updateCursorModeForTab(){
      try{
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

        if (!okbExperimentalEnabled && okb.EnableExperimentalFeatures){
          await okb.EnableExperimentalFeatures([
            { name: 'DoodlesOnly', version: 2024071802 },
            { name: 'SetCursorEventsMode', version: 2024071801 },
          ]);
          okbExperimentalEnabled = true;
        }

        if (!okb.SetCursorEventsMode) return;

        if (targetMode === 'DoodlesOnly'){
          await okb.SetCursorEventsMode('DoodlesOnly');
          okbCursorMode = 'DoodlesOnly';
          return;
        }

        const restoreModes = ['MouseEmulation', 'Mouse', 'Normal', 'Default'];
        for (let i = 0; i < restoreModes.length; i++){
          const mode = restoreModes[i];
          try{
            await okb.SetCursorEventsMode(mode);
            okbCursorMode = mode;
            return;
          }catch(_){
          }
        }
      }catch(_){
      }
    }

    function setStatus(text, level){
      const statusEl = document.getElementById('status');
      const statusTextEl = document.getElementById('statusText');
      if (statusTextEl) statusTextEl.textContent = text;
      if (!statusEl) return;
      statusEl.classList.remove('status-error', 'status-warning', 'status-sent');
      if (level === 'error') statusEl.classList.add('status-error');
      if (level === 'warning') statusEl.classList.add('status-warning');
      if (level === 'sent') statusEl.classList.add('status-sent');
    }

    function applySessionCollapsedState(collapsed){
      sessionCollapsed = !!collapsed;
      document.getElementById('session').style.display = sessionCollapsed ? 'none' : 'block';
      document.getElementById('sessionHeader').textContent = sessionCollapsed ? 'Session ► (click to expand)' : 'Session ▼';
    }

    function readInitialSessionCollapsed(){
      try{
        return window.localStorage && window.localStorage.getItem(sessionCollapsedStorageKey) === '1';
      }catch(_){
        return false;
      }
    }

    function persistSessionCollapsedState(){
      try{
        if (window.localStorage){
          window.localStorage.setItem(sessionCollapsedStorageKey, sessionCollapsed ? '1' : '0');
        }
      }catch(_){
      }
    }

    function readDrawModePreference(){
      try{
        return window.localStorage && window.localStorage.getItem(drawModeStorageKey) === '1';
      }catch(_){
        return false;
      }
    }

    function persistDrawModePreference(){
      try{
        if (window.localStorage){
          window.localStorage.setItem(drawModeStorageKey, drawModeEnabled ? '1' : '0');
        }
      }catch(_){
      }
    }

    function updateDrawModeToggleUi(){
      const button = document.getElementById('drawModeToggle');
      if (!button) return;
      button.classList.toggle('draw-on', drawModeEnabled);

      if (okbDoodlesOnlyForced){
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
    }

    function setDrawInteractionInNotes(active){
      const next = !!active;
      if (drawInteractionInNotes === next) return;
      drawInteractionInNotes = next;
      updateCursorModeForTab();

      if (drawInteractionInNotes){
        scheduleDrawModeAutoOff();
      }
    }

    function clearDrawModeDisableTimer(){
      if (drawModeDisableTimer){
        clearTimeout(drawModeDisableTimer);
        drawModeDisableTimer = null;
      }
      drawModeDeadlineUtcMs = 0;
      updateDrawTimerUi();
    }

    function updateDrawTimerUi(){
      const timerEl = document.getElementById('drawTimer');
      if (!timerEl) return;

      const shouldShow = drawModeEnabled && drawModeDeadlineUtcMs > 0;
      timerEl.className = shouldShow ? 'drawTimer' : 'drawTimer hidden';
      if (!shouldShow){
        timerEl.textContent = '30s';
        return;
      }

      const remainingMs = Math.max(0, drawModeDeadlineUtcMs - Date.now());
      const remainingSeconds = Math.ceil(remainingMs / 1000);
      timerEl.textContent = String(remainingSeconds) + 's';
    }

    function ensureDrawCountdownTicking(){
      if (drawModeCountdownTimer) return;
      drawModeCountdownTimer = setInterval(function(){
        updateDrawTimerUi();
      }, 200);
    }

    function stopDrawCountdownTicking(){
      if (!drawModeCountdownTimer) return;
      clearInterval(drawModeCountdownTimer);
      drawModeCountdownTimer = null;
    }

    function scheduleDrawModeAutoOff(){
      if (!drawModeEnabled || okbDoodlesOnlyForced || selectedTab !== 'NOTES'){
        clearDrawModeDisableTimer();
        return;
      }

      clearDrawModeDisableTimer();
      drawModeDeadlineUtcMs = Date.now() + drawModeTimeoutMs;
      ensureDrawCountdownTicking();
      updateDrawTimerUi();
      drawModeDisableTimer = setTimeout(function(){
        disableDrawMode();
      }, drawModeTimeoutMs);
    }

    function notifyDrawActivity(){
      if (!drawModeEnabled || okbDoodlesOnlyForced || selectedTab !== 'NOTES') return;
      scheduleDrawModeAutoOff();
    }

    function disableDrawMode(){
      if (!drawModeEnabled) return;
      drawModeEnabled = false;
      setDrawInteractionInNotes(false);
      clearDrawModeDisableTimer();
      stopDrawCountdownTicking();
      persistDrawModePreference();
      updateDrawModeToggleUi();
      updateCursorModeForTab();
    }

    function getTabKeywordsMetrics(){
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

    function getTabSplitStorageKey(tab){
      return String(tab || 'LOG')
        .toUpperCase()
        .replace(/\s+/g, '_')
        .replace(/[^A-Z0-9_]/g, '');
    }

    function resetTabKeywordsSplitToDefault(){
      const metrics = getTabKeywordsMetrics();
      if (!metrics) return;
      metrics.tabPanel.style.flex = '';
      metrics.keywordPanel.style.flex = '';
    }

    function getCurrentTabKeywordsSplitRatio(){
      const key = getTabSplitStorageKey(selectedTab);
      const value = tabKeywordsSplitByTab[key];
      return isFinite(value) ? value : NaN;
    }

    function setCurrentTabKeywordsSplitRatio(ratio){
      if (!isFinite(ratio)) return;
      const key = getTabSplitStorageKey(selectedTab);
      tabKeywordsSplitByTab[key] = ratio;
      try{
        if (window.localStorage){
          window.localStorage.setItem(tabKeywordsSplitStorageKey, JSON.stringify(tabKeywordsSplitByTab));
        }
      }catch(_){
      }
    }

    function applyTabKeywordsSplitRatio(ratio, persist){
      const metrics = getTabKeywordsMetrics();
      if (!metrics) return;

      const safeRatio = isFinite(ratio) ? ratio : (metrics.topHeight / metrics.total);
      let targetTop = metrics.total * clamp(safeRatio, 0.15, 0.85);
      const maxTop = Math.max(metrics.minTop, metrics.total - metrics.minBottom);
      targetTop = clamp(targetTop, metrics.minTop, maxTop);

      metrics.tabPanel.style.flex = '0 0 ' + Math.round(targetTop) + 'px';
      metrics.keywordPanel.style.flex = '1 1 auto';

      if (persist){
        setCurrentTabKeywordsSplitRatio(targetTop / metrics.total);
      }
    }

    function readTabKeywordsSplitRatioByTab(){
      try{
        if (!window.localStorage) return {};
        const raw = window.localStorage.getItem(tabKeywordsSplitStorageKey);
        if (!raw) return {};
        const parsed = JSON.parse(raw);
        if (!parsed || typeof parsed !== 'object') return {};

        const cleaned = {};
        Object.keys(parsed).forEach(function(k){
          const value = parseFloat(parsed[k]);
          if (isFinite(value)){
            cleaned[String(k)] = value;
          }
        });

        return cleaned;
      }catch(_){
        return {};
      }
    }

    function applyCurrentTabKeywordsSplit(){
      const ratio = getCurrentTabKeywordsSplitRatio();
      if (isFinite(ratio)){
        applyTabKeywordsSplitRatio(ratio, false);
        return;
      }

      resetTabKeywordsSplitToDefault();
    }

    function initTabKeywordsDivider(){
      const divider = document.getElementById('tabKeywordDivider');
      if (!divider) return;

      let drag = null;

      divider.addEventListener('mousedown', function(ev){
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

      document.addEventListener('mousemove', function(ev){
        if (!drag) return;
        const targetTop = clamp(drag.startTop + (ev.clientY - drag.startY), drag.minTop, drag.maxTop);
        const ratio = targetTop / drag.total;
        applyTabKeywordsSplitRatio(ratio, false);
      });

      document.addEventListener('mouseup', function(){
        if (!drag) return;
        const metrics = getTabKeywordsMetrics();
        if (metrics){
          const ratio = metrics.topHeight / metrics.total;
          applyTabKeywordsSplitRatio(ratio, true);
        }
        drag = null;
      });
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

    function isF4Aircraft(data){
      const server = (data && data.Server) || {};
      const aircraft = String(server.Aircraft || '').toUpperCase();
      return aircraft.indexOf('F-4') >= 0;
    }

    function rankF4KeywordByPhase(phase, phrase){
      const p = String(phrase || '').trim();
      if (!p) return 100;

      const pp = p.toLowerCase();
      function hasAny(terms){
        for (let i = 0; i < terms.length; i++){
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

      if (phase === 'startup and taxi'){
        if (/^Ground\s+/i.test(p)) return 0;
        if (startupMisc.indexOf(p) >= 0) return 1;
        if (crewControl.indexOf(p) >= 0) return 2;
        return 3;
      }

      if (phase === 'enroute'){
        if (hasAny([
          'navigation', 'tacan', 'waypoint', 'flight plan', 'resume', 'hold ', 'hold at',
          'divert', 'tune radio', 'select mode',
          'radar', 'iff', 'boresight', 'scan', 'auto focus', 'go radar'
        ])) return 0;
        return 1;
      }

      if (phase === 'fence/target'){
        if (hasAny([
          'countermeasures', 'chaff', 'flare', 'jammer', 'jettison',
          'pave spike', 'tv weapons', 'designate', 'undesignate', 'lock target', 'focus target', 'context '
        ])) return 0;
        return 1;
      }

      if (phase === 'approach/landing'){
        if (hasAny([
          'navigation', 'tacan', 'waypoint', 'flight plan', 'resume', 'hold ', 'divert', 'tune radio', 'select mode'
        ])) return 0;
        if (p === 'Fuel Is Looking Good') return 1;
        return 2;
      }

      if (phase === 'divert/low fuel'){
        if (hasAny(['divert', 'fuel'])) return 0;
        if (hasAny([
          'navigation', 'tacan', 'waypoint', 'flight plan', 'resume', 'hold ', 'tune radio', 'select mode'
        ])) return 1;
        return 2;
      }

      if (phase === 'taxi in/shutdown'){
        if (/^Ground\s+/i.test(p)) return 0;
        return 1;
      }

      return 0;
    }

    function reorderAiCrewPhrasesForPhase(data, phrases){
      const list = Array.isArray(phrases) ? phrases.slice() : [];
      const phase = String((data && data.AiCrewPhase) || '').trim().toLowerCase();

      if (!isF4Aircraft(data)){
        list.sort(function(a,b){ return a.localeCompare(b); });
        return list;
      }

      if (phase === 'startup and taxi'
        || phase === 'enroute'
        || phase === 'fence/target'
        || phase === 'approach/landing'
        || phase === 'divert/low fuel'
        || phase === 'taxi in/shutdown'){
        list.sort(function(a, b){
          const rankDiff = rankF4KeywordByPhase(phase, a) - rankF4KeywordByPhase(phase, b);
          if (rankDiff !== 0) return rankDiff;
          return String(a).localeCompare(String(b));
        });
        return list;
      }

      list.sort(function(a,b){ return a.localeCompare(b); });
      return list;
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
        return reorderAiCrewPhrasesForPhase(data, cleaned);
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

      const leftRows = [];
      const rightRows = [];
      for (let i = 0; i < rows.length; i++){
        if ((i % 2) === 0) leftRows.push(rows[i]);
        else rightRows.push(rows[i]);
      }

      const left = leftRows.map(escapeHtml).join('<br>');
      const right = rightRows.map(escapeHtml).join('<br>');

      return '<div class=""kwCols""><div class=""kwCol"">' + left + '</div><div class=""kwCol"">' + right + '</div></div>';
    }

    function formatTabContent(data, tab){
      const server = (data && data.Server) || {};
      const atcMetars = (server && server.AtcMetars) || {};
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
      const unitLines = units.slice();
      if (log) parts.push('Log:\n' + log);
      if (tab === 'ATC') {
        const metarIdx = unitLines.findIndex(function(u){
          const t = String(u || '').trim();
          return t.indexOf('METAR:') === 0 || t === 'METAR';
        });
        if (metarIdx >= 0) {
          let metarLine = String(unitLines.splice(metarIdx, 1)[0] || '').trim();

          if (metarLine === 'METAR') {
            const metarParts = [];
            while (metarIdx < unitLines.length) {
              const segment = String(unitLines[metarIdx] || '');
              if (!segment.trim()) {
                unitLines.splice(metarIdx, 1);
                break;
              }

              metarParts.push(segment.replace(/\s+/g, ' ').trim());
              unitLines.splice(metarIdx, 1);
            }

            metarLine = metarParts.length ? metarParts.join(' ') : '-';
          } else {
            metarLine = metarLine.replace(/\s*\n\s*/g, ' ').trim();
          }

          metarLine = metarLine.replace(/^METAR:\s*/i, '').trim();

          parts.push('Weather:\n  ' + metarLine);

          if (selectedAtcMetarKey) {
            const selectedMetar = atcMetars[selectedAtcMetarKey] || '';
            if (selectedMetar) {
              parts.push('Selected Airfield Weather:\n  ' + String(selectedMetar).replace(/^METAR:\s*/i, '').trim());
            }
          }
        }
      }
      if (unitLines.length) parts.push('Units:\n' + unitLines.map(function(u){ return '  ' + u; }).join('\n'));
      if (details.length) parts.push('Unit Details:\n' + details.map(function(u){ return '  ' + u; }).join('\n'));

      let text = parts.length ? parts.join('\n\n') : 'No data for this tab yet.';

      function metersToSmToken(visMeters){
        const m = parseInt(visMeters, 10);
        if (!isFinite(m)) return null;
        const sm = m / 1609.344;
        if (sm > 6) return 'P6SM';
        if (sm <= 0.25) return '1/4SM';

        if (sm >= 2) {
          return String(Math.round(sm)) + 'SM';
        }

        const quarter = Math.round(sm * 4) / 4;
        if (quarter <= 0.25) return '1/4SM';
        if (quarter <= 0.5) return '1/2SM';
        if (quarter <= 0.75) return '3/4SM';

        const whole = Math.floor(quarter + 1e-9);
        const frac = quarter - whole;
        if (Math.abs(frac) < 1e-6) return String(whole) + 'SM';
        if (Math.abs(frac - 0.25) < 1e-6) return String(whole) + ' 1/4SM';
        if (Math.abs(frac - 0.5) < 1e-6) return String(whole) + ' 1/2SM';
        if (Math.abs(frac - 0.75) < 1e-6) return String(whole) + ' 3/4SM';
        return String(whole) + 'SM';
      }

      if (tab === 'ATC' && metarPressureInHg) {
        text = text.replace(/((?:METAR:\s+|METAR\s+)[^\n]*?)\b(\d{4}|9999)\b(\s+.*?\s+)Q(\d{4})\b/g, function(_, prefix, vism, middle, qhpa){
          const visSm = metersToSmToken(vism);
          const hpa = parseInt(qhpa, 10);
          if (!isFinite(hpa)) return _;
          const inhg = (hpa * 0.0295299830714).toFixed(2);
          const visToken = visSm || vism;
          return prefix + visToken + middle + 'A' + inhg;
        });

        text = text.replace(/((?:METAR:\s+|METAR\s+)[^\n]*?\bCAVOK\b[^\n]*?\s+)Q(\d{4})\b/g, function(_, prefix, qhpa){
          const hpa = parseInt(qhpa, 10);
          if (!isFinite(hpa)) return _;
          const inhg = (hpa * 0.0295299830714).toFixed(2);
          return prefix + 'A' + inhg;
        });
      }

      return text;
    }

    function resolveAtcMetarKey(unitLine, atcMetars){
      const line = String(unitLine || '');
      if (!line) return '';

      const map = atcMetars || {};
      const upperLine = line.toUpperCase();
      const directIcao = line.match(/\b([A-Z]{4})\b/);
      if (directIcao && map[directIcao[1]]) return directIcao[1];

      const aliasMatch = line.match(/\[([^\]]+)\]/);
      if (aliasMatch && aliasMatch[1]) {
        const alias = String(aliasMatch[1]).toUpperCase();
        if (map[alias]) return alias;
      }

      const callsignMatch = line.match(/\]\s*([^\d\n][^\n]*?)\s+\d{3}\s+/);
      if (callsignMatch && callsignMatch[1]) {
        const cs = String(callsignMatch[1]).trim().toUpperCase();
        if (map[cs]) return cs;
      }

      const keys = Object.keys(map).sort(function(a, b){ return String(b).length - String(a).length; });
      for (let i = 0; i < keys.length; i++) {
        const k = keys[i];
        if (!k) continue;
        if (upperLine.indexOf(String(k).toUpperCase()) >= 0) return k;
      }

      return '';
    }

    function getClickedLineFromEvent(ev, container){
      if (!ev || !container) return '';

      const fullText = String(container.textContent || '');
      if (!fullText) return '';

      let offset = -1;
      if (document.caretPositionFromPoint){
        const pos = document.caretPositionFromPoint(ev.clientX, ev.clientY);
        if (pos && pos.offsetNode){
          const r = document.createRange();
          r.selectNodeContents(container);
          r.setEnd(pos.offsetNode, pos.offset);
          offset = r.toString().length;
        }
      } else if (document.caretRangeFromPoint){
        const range = document.caretRangeFromPoint(ev.clientX, ev.clientY);
        if (range){
          const r = document.createRange();
          r.selectNodeContents(container);
          r.setEnd(range.startContainer, range.startOffset);
          offset = r.toString().length;
        }
      }

      if (offset < 0 || offset > fullText.length){
        return '';
      }

      let start = fullText.lastIndexOf('\n', offset - 1);
      start = (start < 0) ? 0 : (start + 1);
      let end = fullText.indexOf('\n', offset);
      if (end < 0) end = fullText.length;

      return fullText.substring(start, end).trim();
    }

    function renderTabs(data){
      const tabsEl = document.getElementById('tabs');
      tabsEl.innerHTML = '';
      const active = normalizeActiveCategory(data.ActiveCategory, data);
      if (autoBrowse && TABS.indexOf(active) >= 0){
        selectedTab = active;
      }
      if (TABS.indexOf(selectedTab) < 0) selectedTab = 'LOG';

      TABS.forEach(function(tab){
        const btn = document.createElement('button');
        btn.className = 'tab ' + tabCssClass(tab) + (tab === selectedTab ? ' active' : '');
        btn.textContent = tabLabel(tab);
        btn.onclick = function(){
          setSelectedTab(tab);
        };
        tabsEl.appendChild(btn);
      });
    }

    function render(data){
      latestData = data;
      const server = data && data.Server ? data.Server : {};
      const haveMission = !!(server.Aircraft || server.MissionTitle || server.Theater);
      const debugMode = !!server.DebugMode;
      const defaultStatusText = haveMission ? 'Live session detected.' : 'Waiting for mission data...';
      let statusText = defaultStatusText;
      let statusLevel = '';

      if (data && data.Status && data.Status.Text) {
        const updated = data.Status.UpdatedUtc ? Date.parse(data.Status.UpdatedUtc) : NaN;
        const fresh = isFinite(updated) ? ((Date.now() - updated) < 10000) : true;
        if (fresh) {
          statusText = String(data.Status.Text);
          statusLevel = String(data.Status.Level || '').toLowerCase();
        }
      }

      setStatus(statusText, statusLevel);

      document.getElementById('session').textContent = [
        'Active Category : ' + safe(normalizeActiveCategory(data.ActiveCategory, data)),
        'Updated (UTC)   : ' + formatUtcToSeconds(data.UpdatedUtc),
        '',
        'Theater         : ' + safe(server.Theater),
        'DCS Location    : ' + safe(server.DcsLocation || server.DcsVersion),
        'Aircraft        : ' + safe(server.Aircraft),
        'Callsign        : ' + safe(server.PlayerCallsign),
        'Mission         : ' + safe(server.MissionTitle),
        'Multiplayer     : ' + (server.Multiplayer ? 'Yes' : 'No')
      ].join('\n');

      renderTabs(data);
      applyCurrentTabKeywordsSplit();
      document.body.classList.toggle('notes-tab', selectedTab === 'NOTES');
      if (selectedTab !== 'NOTES' || !drawModeEnabled){
        setDrawInteractionInNotes(false);
        clearDrawModeDisableTimer();
      }
      updateDrawModeToggleUi();
      document.getElementById('tabTitle').textContent = 'Tab: ' + tabLabel(selectedTab);
      document.getElementById('tabBody').textContent = formatTabContent(data, selectedTab);
      updateCursorModeForTab();
      const tabBodyEl = document.getElementById('tabBody');
      const hasMetar = selectedTab === 'ATC' && /\bMETAR\s+[A-Z]{4}\b/i.test(tabBodyEl.textContent || '');
      if (hasMetar) {
        tabBodyEl.style.cursor = 'pointer';
        tabBodyEl.title = 'Click METAR to toggle pressure units (hPa/inHg)';
      } else {
        tabBodyEl.style.cursor = '';
        tabBodyEl.title = '';
      }
      const aiCrewPhaseSuffix = selectedTab === 'AI CREW'
        ? formatAiCrewPhaseLabel(data.AiCrewPhase)
        : '';
      document.getElementById('keywordTitle').textContent = 'Keywords: ' + tabLabel(selectedTab) + aiCrewPhaseSuffix;
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

      const serverMessagesEl = document.getElementById('serverMessages');
      if (serverMessagesEl){
        const rows = Array.isArray(data.RawServerMessages) ? data.RawServerMessages : [];
        serverMessagesEl.textContent = rows.length ? rows.join('\n') : 'No server messages captured yet.';
      }

      const showServerWrap = document.getElementById('showServerWrap');
      if (showServerWrap){
        showServerWrap.style.display = debugMode ? 'inline-flex' : 'none';
      }

      if (!debugMode){
        showServerMessages = false;
        const showServerBox = document.getElementById('showServer');
        if (showServerBox) showServerBox.checked = false;
        if (serverMessagesEl) serverMessagesEl.className = 'hidden';
      }
    }

    async function setServerMessageCapture(enabled){
      try{
        await fetch('dev/servermessages?enabled=' + (enabled ? '1' : '0'), { method: 'POST', cache: 'no-store' });
      }catch(e){
      }
    }

    async function configureOpenKneeboard(){
      try {
        var okb = (typeof OpenKneeboard !== 'undefined') ? OpenKneeboard : window.OpenKneeboard;
        if (okb && okb.SetPreferredPixelSize) {
          await okb.SetPreferredPixelSize(1050, 1480);
          setTimeout(function(){ okb.SetPreferredPixelSize(1050, 1480); }, 300);
          setTimeout(function(){ okb.SetPreferredPixelSize(1050, 1480); }, 1200);
        }

        const q = (window.location && window.location.search) ? window.location.search : '';
        if (q.indexOf('doodles=1') >= 0 || q.indexOf('ink=1') >= 0) {
          if (okb && okb.EnableExperimentalFeatures) {
            await okb.EnableExperimentalFeatures([
              { name: 'DoodlesOnly', version: 2024071802 },
              { name: 'SetCursorEventsMode', version: 2024071801 },
            ]);
            okbExperimentalEnabled = true;
          }
          if (okb && okb.SetCursorEventsMode) {
            await okb.SetCursorEventsMode('DoodlesOnly');
            okbCursorMode = 'DoodlesOnly';
            okbDoodlesOnlyForced = true;
          }
        }
        setTimeout(function(){ updateCursorModeForTab(); }, 50);
        setTimeout(function(){ updateCursorModeForTab(); }, 500);
      } catch (e) {
      }
    }

    async function tick(){
      try{
        const r = await fetch('state', { cache: 'no-store' });
        const j = await r.json();
        render(j);
      }catch(e){
        setStatus('Waiting for VAICOM connection...', '');
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

    document.getElementById('drawModeToggle').addEventListener('click', function(){
      if (okbDoodlesOnlyForced || selectedTab !== 'NOTES') return;
      if (drawModeEnabled){
        disableDrawMode();
        return;
      }

      drawModeEnabled = true;
      setDrawInteractionInNotes(true);
      scheduleDrawModeAutoOff();
      persistDrawModePreference();
      updateDrawModeToggleUi();
      updateCursorModeForTab();
    });

    document.getElementById('tabBody').addEventListener('dblclick', function(){
      if (okbDoodlesOnlyForced || selectedTab !== 'NOTES') return;
      disableDrawMode();
    });

    document.getElementById('tabBody').addEventListener('mouseenter', function(){
      if (okbDoodlesOnlyForced || selectedTab !== 'NOTES' || !drawModeEnabled) return;
      setDrawInteractionInNotes(true);
      notifyDrawActivity();
    });

    document.getElementById('tabBody').addEventListener('mouseleave', function(){
      if (okbDoodlesOnlyForced) return;
      setDrawInteractionInNotes(false);
    });

    document.getElementById('tabBody').addEventListener('pointerdown', notifyDrawActivity);
    document.getElementById('tabBody').addEventListener('pointermove', notifyDrawActivity);
    document.getElementById('tabBody').addEventListener('touchstart', notifyDrawActivity);
    document.getElementById('tabBody').addEventListener('touchmove', notifyDrawActivity);

    document.querySelector('.controls').addEventListener('mouseenter', function(){
      if (okbDoodlesOnlyForced) return;
      setDrawInteractionInNotes(false);
    });

    document.querySelector('.tabRail').addEventListener('mouseenter', function(){
      if (okbDoodlesOnlyForced) return;
      setDrawInteractionInNotes(false);
    });

    document.getElementById('tabBody').addEventListener('click', function(ev){
      if (selectedTab !== 'ATC' || !latestData) return;
      const text = this.textContent || '';
      const selection = window.getSelection ? window.getSelection() : null;
      const selectedText = selection ? String(selection.toString() || '').trim() : '';
      const clickedLine = getClickedLineFromEvent(ev, this);
      const clickedUpper = String(clickedLine || '').toUpperCase();
      const selectedUpper = String(selectedText || '').toUpperCase();
      const hasMetar = /\bMETAR\s+[A-Z]{4}\b/i.test(text);

      if (okbCursorMode === 'DoodlesOnly' || okbDoodlesOnlyForced) return;

      const clickedMetarArea = clickedUpper.indexOf('METAR:') >= 0
        || clickedUpper.indexOf('METAR ') >= 0
        || clickedUpper.indexOf('WEATHER:') === 0
        || clickedUpper.indexOf('SELECTED AIRFIELD WEATHER:') === 0;
      const selectedMetarArea = selectedUpper.indexOf('METAR:') >= 0 || selectedUpper.indexOf('METAR ') >= 0;

      if (clickedMetarArea || selectedMetarArea) {
        if (!hasMetar) return;
        metarPressureInHg = !metarPressureInHg;
        render(latestData);
        return;
      }

      const server = latestData && latestData.Server ? latestData.Server : {};
      const atcMetars = server.AtcMetars || {};
      const candidateKey = resolveAtcMetarKey(selectedText, atcMetars);
      const lineKey = resolveAtcMetarKey(clickedLine, atcMetars);

      if (candidateKey) {
        selectedAtcMetarKey = candidateKey;
        render(latestData);
        return;
      }

      if (lineKey) {
        selectedAtcMetarKey = lineKey;
        render(latestData);
        return;
      }

      if (!hasMetar) return;
      metarPressureInHg = !metarPressureInHg;
      render(latestData);
    });

    document.getElementById('showServer').addEventListener('change', function(ev){
      showServerMessages = ev.target.checked;
      document.getElementById('serverMessages').className = showServerMessages ? '' : 'hidden';
      setServerMessageCapture(showServerMessages);
    });

    document.getElementById('sessionHeader').addEventListener('click', function(){
      applySessionCollapsedState(!sessionCollapsed);
      persistSessionCollapsedState();
    });

    applySessionCollapsedState(readInitialSessionCollapsed());
    drawModeEnabled = readDrawModePreference();
    updateDrawModeToggleUi();
    tabKeywordsSplitByTab = readTabKeywordsSplitRatioByTab();
    initTabKeywordsDivider();
    applyCurrentTabKeywordsSplit();
    registerCustomActionHandlers();
    window.addEventListener('resize', function(){
      applyCurrentTabKeywordsSplit();
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
                private const string OpenKneeboardPluginsRegistryKey = @"SOFTWARE\Fred Emmott\OpenKneeboard\Plugins\v1";
                private const string OpenKneeboardPluginId = "VAICOM-Community";
                private const string OpenKneeboardPluginTabId = OpenKneeboardPluginId + ";okb-out";
                private const string OpenKneeboardKeywordsPluginId = "github.com/Penecruz/VAICOM-Community/keywords";
                private const string OpenKneeboardKeywordsPluginTabId = OpenKneeboardKeywordsPluginId + ";keywords";

                public static void Initialize()
                {
                    ResetSnapshot();
                    SetPluginRegistration(State.activeconfig != null && State.activeconfig.OpenKneeboard_Out);
                    StartWebHost();
                }

                public static void SetEnabled(bool enabled)
                {
                    SetPluginRegistration(enabled);

                    if (enabled)
                    {
                        StartWebHost();
                    }
                    else
                    {
                        StopWebHost();
                    }
                }

                public static void RegisterKeywordsHtmlPlugin(string keywordsHtmlPath)
                {
                    try
                    {
                        string manifestPath = GetKeywordsPluginManifestPath();
                        if (string.IsNullOrWhiteSpace(manifestPath))
                        {
                            return;
                        }

                        WriteKeywordsPluginManifest(manifestPath, keywordsHtmlPath);

                        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(OpenKneeboardPluginsRegistryKey))
                        {
                            if (key == null)
                            {
                                return;
                            }

                            key.SetValue(manifestPath, 1, RegistryValueKind.DWord);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write("OpenKneeboard keywords plugin registration failed: " + ex.Message, Colors.Warning);
                    }
                }

                public static void UpdateStatus(string text, string level)
                {
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        return;
                    }

                    lock (Sync)
                    {
                        snapshot.Status = new OpenKneeboardStatusSnapshot
                        {
                            Text = text,
                            Level = string.IsNullOrWhiteSpace(level) ? "" : level,
                            UpdatedUtc = DateTime.UtcNow,
                        };
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void Shutdown()
                {
                    StopWebHost();
                }

                private static void SetPluginRegistration(bool enabled)
                {
                    string manifestPath = GetPluginManifestPath();
                    if (string.IsNullOrWhiteSpace(manifestPath))
                    {
                        return;
                    }

                    if (enabled)
                    {
                        WritePluginManifest(manifestPath);
                    }

                    try
                    {
                        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(OpenKneeboardPluginsRegistryKey))
                        {
                            if (key == null)
                            {
                                return;
                            }

                            key.SetValue(manifestPath, enabled ? 1 : 0, RegistryValueKind.DWord);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write("OpenKneeboard plugin registration failed: " + ex.Message, Colors.Warning);
                    }
                }

                private static string GetPluginManifestPath()
                {
                    try
                    {
                        string appsRoot = string.IsNullOrWhiteSpace(State.VA_APPS)
                            ? (State.Proxy == null ? "" : Convert.ToString(State.Proxy.SessionState["VA_APPS"]))
                            : State.VA_APPS;

                        if (string.IsNullOrWhiteSpace(appsRoot))
                        {
                            return "";
                        }

                        string pluginRoot = Path.Combine(appsRoot, Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername);
                        string configFolder = AppData.SubFolders.ContainsKey("config")
                            ? AppData.SubFolders["config"]
                            : "config";
                        string outputFolder = Path.Combine(pluginRoot, configFolder);

                        return Path.Combine(outputFolder, "OpenKneeboard.v1.json");
                    }
                    catch
                    {
                        return "";
                    }
                }

                private static string GetKeywordsPluginManifestPath()
                {
                    try
                    {
                        string appsRoot = string.IsNullOrWhiteSpace(State.VA_APPS)
                            ? (State.Proxy == null ? "" : Convert.ToString(State.Proxy.SessionState["VA_APPS"]))
                            : State.VA_APPS;

                        if (string.IsNullOrWhiteSpace(appsRoot))
                        {
                            return "";
                        }

                        string pluginRoot = Path.Combine(appsRoot, Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername);
                        string configFolder = AppData.SubFolders.ContainsKey("config")
                            ? AppData.SubFolders["config"]
                            : "config";
                        string outputFolder = Path.Combine(pluginRoot, configFolder);

                        return Path.Combine(outputFolder, "OpenKneeboard.Keywords.v1.json");
                    }
                    catch
                    {
                        return "";
                    }
                }

                private static void WritePluginManifest(string manifestPath)
                {
                    try
                    {
                        string folder = Path.GetDirectoryName(manifestPath);
                        if (!string.IsNullOrWhiteSpace(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        string readableVersion = string.IsNullOrWhiteSpace(State.versionstring)
                            ? State.pluginversionnumber
                            : State.versionstring;
                        string semanticVersion = string.IsNullOrWhiteSpace(State.pluginversionnumber)
                            ? "3.1.0"
                            : State.pluginversionnumber;

                        var manifest = new
                        {
                            ID = OpenKneeboardPluginId,
                            Metadata = new
                            {
                                PluginName = "VAICOM OpenKneeboard Out",
                                PluginReadableVersion = readableVersion,
                                PluginSemanticVersion = semanticVersion,
                                OKBMinimumVersion = "1.9",
                                Author = "VAICOM Community",
                                Website = "https://github.com/Penecruz/VAICOM-Community",
                            },
                            TabTypes = new[]
                            {
                                new
                                {
                                    ID = OpenKneeboardPluginTabId,
                                    Name = "VAICOM OpenKneeboard Out",
                                    CustomActions = new[]
                                    {
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-prev",
                                            Name = "Tab Previous",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-next",
                                            Name = "Tab Next",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-select",
                                            Name = "Tab Select (via ExtraData)",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-log",
                                            Name = "Tab LOG",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-atc",
                                            Name = "Tab WX/ATC",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-awacs",
                                            Name = "Tab AWACS",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-jtac",
                                            Name = "Tab JTAC",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-tanker",
                                            Name = "Tab TANKER",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-aocs",
                                            Name = "Tab AOCS",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-flight",
                                            Name = "Tab FLIGHT",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-ai-crew",
                                            Name = "Tab AI CREW",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-gnd-crew",
                                            Name = "Tab GND CREW",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-notes",
                                            Name = "Tab NOTES",
                                        },
                                    },
                                    Implementation = "WebBrowser",
                                    ImplementationArgs = new
                                    {
                                        URI = Prefix,
                                        InitialSize = new
                                        {
                                            Width = 1050,
                                            Height = 1480,
                                        },
                                    },
                                },
                            },
                        };

                        string json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
                        File.WriteAllText(manifestPath, json, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        Log.Write("OpenKneeboard plugin manifest update failed: " + ex.Message, Colors.Warning);
                    }
                }

                private static void WriteKeywordsPluginManifest(string manifestPath, string keywordsHtmlPath)
                {
                    try
                    {
                        string folder = Path.GetDirectoryName(manifestPath);
                        if (!string.IsNullOrWhiteSpace(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        string readableVersion = string.IsNullOrWhiteSpace(State.versionstring)
                            ? State.pluginversionnumber
                            : State.versionstring;
                        string semanticVersion = string.IsNullOrWhiteSpace(State.pluginversionnumber)
                            ? "3.1.0"
                            : State.pluginversionnumber;

                        string targetHtmlPath = keywordsHtmlPath;
                        if (string.IsNullOrWhiteSpace(targetHtmlPath))
                        {
                            string appsRoot = string.IsNullOrWhiteSpace(State.VA_APPS)
                                ? (State.Proxy == null ? "" : Convert.ToString(State.Proxy.SessionState["VA_APPS"]))
                                : State.VA_APPS;
                            if (!string.IsNullOrWhiteSpace(appsRoot))
                            {
                                targetHtmlPath = Path.Combine(appsRoot, Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername, AppData.SubFolders["export"], "keywords.html");
                            }
                        }

                        string uri = "";
                        if (!string.IsNullOrWhiteSpace(targetHtmlPath))
                        {
                            uri = new Uri(targetHtmlPath).AbsoluteUri;
                            uri = uri + (uri.IndexOf("?", StringComparison.Ordinal) >= 0 ? "&" : "?") + "okb=1";
                        }

                        var manifest = new
                        {
                            ID = OpenKneeboardKeywordsPluginId,
                            Metadata = new
                            {
                                PluginName = "VAICOM Keywords",
                                PluginReadableVersion = readableVersion,
                                PluginSemanticVersion = semanticVersion,
                                OKBMinimumVersion = "1.9",
                                Author = "VAICOM Community",
                                Website = "https://github.com/Penecruz/VAICOM-Community",
                            },
                            TabTypes = new[]
                            {
                                new
                                {
                                    ID = OpenKneeboardKeywordsPluginTabId,
                                    Name = "VAICOM Keywords",
                                    Implementation = "WebBrowser",
                                    ImplementationArgs = new
                                    {
                                        URI = uri,
                                        InitialSize = new
                                        {
                                            Width = 1050,
                                            Height = 1480,
                                        },
                                    },
                                },
                            },
                        };

                        string json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
                        File.WriteAllText(manifestPath, json, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        Log.Write("OpenKneeboard keywords plugin manifest update failed: " + ex.Message, Colors.Warning);
                    }
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
                        snapshot.AiCrewPhase = InferF4EAiCrewPhase(lastAiCrewCommand, snapshot.AiCrewPhase);
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                private static string InferF4EAiCrewPhase(string commandText, string currentPhase)
                {
                    try
                    {
                        string moduleId = State.currentmodule == null ? "" : (State.currentmodule.Id ?? "");
                        if (!moduleId.Equals("F-4E-45MC", StringComparison.OrdinalIgnoreCase))
                        {
                            return "Unknown";
                        }

                        string text = (commandText ?? "").ToLowerInvariant();
                        if (string.IsNullOrWhiteSpace(text))
                        {
                            return string.IsNullOrWhiteSpace(currentPhase) ? "Unknown" : currentPhase;
                        }

                        bool hasAny(params string[] terms)
                        {
                            return terms.Any(t => text.IndexOf(t, StringComparison.Ordinal) >= 0);
                        }

                        if (hasAny("divert", "fuel"))
                        {
                            return "Divert/Low Fuel";
                        }

                        if (hasAny("below 50", "below 100", "below 150", "below 200", "approach", "landing"))
                        {
                            return "Approach/Landing";
                        }

                        if (hasAny(
                            "lock target",
                            "focus target",
                            "unlock target",
                            "pave spike",
                            "tv weapons",
                            "countermeasures",
                            "chaff mode",
                            "flare mode",
                            "jammer",
                            "flares jettison",
                            "context select",
                            "context hold",
                            "context double"))
                        {
                            return "Fence/Target";
                        }

                        if (hasAny(
                            "tacan",
                            "waypoint",
                            "flight plan",
                            "resume",
                            "hold",
                            "tune radio",
                            "select mode",
                            "auto focus",
                            "radar",
                            "iff",
                            "boresight",
                            "scan"))
                        {
                            return "Enroute";
                        }

                        bool groundOperation = hasAny(
                            "ground ",
                            "chocks",
                            "power",
                            "air connect",
                            "air disconnect",
                            "air on",
                            "air off",
                            "ladder",
                            "steps",
                            "pitot check",
                            "spoilers check",
                            "flight controls check",
                            "trim check",
                            "start alignment",
                            "alignment");

                        if (groundOperation)
                        {
                            if (string.Equals(currentPhase, "Enroute", StringComparison.OrdinalIgnoreCase)
                                || string.Equals(currentPhase, "Fence/Target", StringComparison.OrdinalIgnoreCase)
                                || string.Equals(currentPhase, "Approach/Landing", StringComparison.OrdinalIgnoreCase)
                                || string.Equals(currentPhase, "Divert/Low Fuel", StringComparison.OrdinalIgnoreCase))
                            {
                                return "Taxi In/Shutdown";
                            }

                            return "Startup and Taxi";
                        }

                        return string.IsNullOrWhiteSpace(currentPhase) ? "Unknown" : currentPhase;
                    }
                    catch
                    {
                        return string.IsNullOrWhiteSpace(currentPhase) ? "Unknown" : currentPhase;
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

                            foreach (string key in GetF4EManualAiCrewKeywords())
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

                private static IEnumerable<string> GetF4EManualAiCrewKeywords()
                {
                    return new[]
                    {
                        "Hold at [Flight plan 1; Primary flight plan; Flight plan 2; Secondary flight plan] waypoint [1..9]",
                        "Go to [Flight Plan; Flight plan 1; Primary flight plan; Flight plan 2; Secondary flight plan] [at] waypoint [1..9]",
                        "Resume [Flight Plan; Flight plan 1; Primary flight plan; Flight plan 2; Secondary flight plan] [at] waypoint [1..9]",
                        "Resume At Waypoint [1..9]",
                        "Hold at current Waypoint",
                        "Designate [Flight plan 1; Primary flight plan; Flight plan 2; Secondary flight plan] waypoint [1..9] as [Turn point; Nav fix; Navigation fix; Target; CAP; I P; Inbound point; Fence in; Fence out; Alternate; Homebase]",
                        "Divert to <Airfield;Asset>",
                        "Tune Radio [to] <Airfield;Asset>",
                        "Tune/Set Radio [Frequency] [2..3] [0..9] [0..9] decimal [0..9] [0;25;50;75]",
                        "Push/Select [Comm;Radio] [Button;Channel] [1..18]",
                        "Push/Select [Aux] [Button;Channel] [1..20]",
                        "Tune TACAN <asset name>",
                        "Tune TACAN station [Alpha-Zulu] [Alpha-Zulu] [Alpha-Zulu]",
                        "Set/Select TACAN [channel] [zero;0;1] [0..9] [0..9] [X-ray;Yankee]",
                        "Radar Focus Target [1..20]",
                        "Radar Lock Target [1..20]"
                    };
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

                        Log.Write("OpenKneeboard dashboard host started at " + Prefix, Colors.Text);
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

                    if (path == "/okb/dev/servermessages")
                    {
                        bool enable = string.Equals(context.Request.QueryString["enabled"], "1", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(context.Request.QueryString["enabled"], "true", StringComparison.OrdinalIgnoreCase);

                        SetRawServerCaptureEnabled(enable);
                        WriteJson(context.Response, "{\"ok\":true}");
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

                public static void SetRawServerCaptureEnabled(bool enabled)
                {
                    bool allow = enabled && State.activeconfig != null && State.activeconfig.Debugmode;

                    lock (Sync)
                    {
                        captureRawServerMessages = allow;
                        if (!allow)
                        {
                            snapshot.RawServerMessages.Clear();
                        }
                    }
                }

                public static void AppendRawServerMessage(string rawMessage)
                {
                    if (string.IsNullOrWhiteSpace(rawMessage))
                    {
                        return;
                    }

                    bool doCapture;
                    lock (Sync)
                    {
                        doCapture = captureRawServerMessages && State.activeconfig != null && State.activeconfig.Debugmode;
                        if (!doCapture)
                        {
                            return;
                        }

                        string entry = DateTime.UtcNow.ToString("o") + " | " + rawMessage;
                        snapshot.RawServerMessages.Add(entry);
                        if (snapshot.RawServerMessages.Count > 200)
                        {
                            snapshot.RawServerMessages = snapshot.RawServerMessages.Skip(snapshot.RawServerMessages.Count - 200).ToList();
                        }
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }

                    try
                    {
                        string logsFolder = Path.Combine(State.VA_APPS, Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername, AppData.SubFolders["logfiles"]);
                        string filePath = Path.Combine(logsFolder, "VAICOMPRO.ServerMessages.log");

                        lock (RawServerLogSync)
                        {
                            Directory.CreateDirectory(logsFolder);
                            File.AppendAllText(filePath, DateTime.UtcNow.ToString("o") + " | " + rawMessage + Environment.NewLine);
                        }
                    }
                    catch
                    {
                    }
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
                            server.AtcMetars = State.currentstate.atcmetars == null
                                ? new Dictionary<string, string>()
                                : new Dictionary<string, string>(State.currentstate.atcmetars);
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
                public string AiCrewPhase { get; set; } = "Unknown";
                public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
                public OpenKneeboardServerSnapshot Server { get; set; } = new OpenKneeboardServerSnapshot();
                public OpenKneeboardStatusSnapshot Status { get; set; } = new OpenKneeboardStatusSnapshot();
                public List<string> AiCrewKeywords { get; set; } = new List<string>();
                public List<string> RawServerMessages { get; set; } = new List<string>();
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
                        AiCrewPhase = AiCrewPhase,
                        UpdatedUtc = UpdatedUtc,
                        Server = Server == null ? new OpenKneeboardServerSnapshot() : Server.Clone(),
                        Status = Status == null ? new OpenKneeboardStatusSnapshot() : Status.Clone(),
                        AiCrewKeywords = new List<string>(AiCrewKeywords ?? new List<string>()),
                        RawServerMessages = new List<string>(RawServerMessages ?? new List<string>()),
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

            public class OpenKneeboardStatusSnapshot
            {
                public string Text { get; set; } = "";
                public string Level { get; set; } = "";
                public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

                public OpenKneeboardStatusSnapshot Clone()
                {
                    return new OpenKneeboardStatusSnapshot
                    {
                        Text = Text,
                        Level = Level,
                        UpdatedUtc = UpdatedUtc,
                    };
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
                public Dictionary<string, string> AtcMetars { get; set; } = new Dictionary<string, string>();

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
                        AtcMetars = new Dictionary<string, string>(AtcMetars ?? new Dictionary<string, string>()),
                    };
                }
            }
        }
    }
}
