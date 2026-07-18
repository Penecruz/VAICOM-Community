using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
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
                private static readonly object StoreLookupSync = new object();
                private static OpenKneeboardSnapshot snapshot = new OpenKneeboardSnapshot();
                private static string lastAiCrewCommand = "";
                private static bool captureRawServerMessages;
                private static readonly string[] DtcFileExtensions = new[] { ".dtc", ".json" };
                private const string RouteSelectionPrefix = "RTE::";
                private const string StoreLookupJsonPlaceholder = "__VAICOM_STORE_LOOKUP_JSON__";
                private static string storeLookupResolvedPath = "";
                private static DateTime storeLookupLastWriteUtc = DateTime.MinValue;
                private static string storeLookupMapJson = "{}";
                private static readonly string IndexHtml = @"<!doctype html>
<html>
<head>
  <meta charset='utf-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1'>
  <title>VAICOM Kneeboard 1.0</title>
  <style>
    html, body { width: 100%; height: 100%; margin: 0; }
    body { font-family: Consolas, monospace; background: transparent; color: #151515; letter-spacing: 0.1px; font-size: 23px; --contentFontSize: 24px; -webkit-user-select: none; -ms-user-select: none; user-select: none; }
    .sheet, .sheet * {
      -webkit-user-select: none !important;
      -ms-user-select: none !important;
      user-select: none !important;
    }
    .sheet {
      width: 100%;
      height: 100%;
      background: #f1f1ef;
      overflow: visible;
      box-sizing: border-box;
      border: 1px solid #9aa0a6;
      box-shadow: inset 0 0 0 1px rgba(0, 0, 0, 0.08);
    }
    .sheetBody { height: 100%; overflow: visible; padding: 8px; box-sizing: border-box; display: flex; flex-direction: column; }
    .headerRow { display:flex; align-items:flex-start; justify-content:space-between; gap:10px; margin-bottom:4px; }
    h3 { margin: 0; font-size: 28px; color: #111; }
    .logo { width: 36px; height: 36px; object-fit: contain; }
    .meta { margin: 2px 0 8px 0; color: #444; font-size: 19px; }
    .meta.liveRefreshToggle { cursor: pointer; user-select: none; }
    .meta.liveRefreshToggle.liveRefreshOn { color: #1f6f43; font-weight: 700; }
    .simControls {
      margin: 0 0 6px 0;
      padding: 4px 6px;
      border: 1px solid #8b96a1;
      background: #eef2f6;
      color: #1b2a36;
      font-size: 15px;
      display: flex;
      align-items: center;
      gap: 6px;
      flex-wrap: wrap;
    }
    .simControls button {
      font-family: inherit;
      font-size: 14px;
      border: 1px solid #7c8692;
      background: #ffffff;
      color: #111;
      cursor: pointer;
      padding: 1px 7px;
      min-height: 22px;
    }
    .simControls .simRate { font-weight: 700; min-width: 56px; text-align: center; }
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
    .tab-DTC { background: rgba(66, 93, 124, 0.82); }
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
    .tab.active.tab-DTC,
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
    .mainContent { font-size: var(--contentFontSize); line-height: 1.32; }
    .mainContent.fltPlanContent,
    .mainContent.fltPlanContent * {
      -webkit-user-select: none;
      -ms-user-select: none;
      user-select: none;
    }
    .keywordsContent { font-size: var(--contentFontSize); line-height: 1.32; }
    .keywordsGroups { display: flex; flex-direction: column; gap: 8px; }
    .kwGroup {
      border: 1px solid #bcc7d2;
      background: #f8fafc;
      padding: 6px 8px;
    }
    .kwGroupTitle {
      font-size: 17px;
      font-weight: 700;
      text-transform: uppercase;
      letter-spacing: 0.4px;
      color: #203448;
      margin: 0 0 4px 0;
    }
    .kwCols { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
    .kwCol { white-space: pre-wrap; word-break: break-word; }
    .controls { margin: 8px 0; color: #222; font-size: 19px; display: flex; gap: 10px; flex-wrap: wrap; align-items: center; }
    .controls.fltPlanControls { margin: 0 0 8px 0; }
    .fltPlanSelected { font-size: 17px; color: #22303d; }
    .missionClockLabel { margin-left: auto; font-size: 17px; color: #22303d; white-space: nowrap; }
    .controls label { white-space: nowrap; }
    .controls select {
      font-family: inherit;
      font-size: 17px;
      padding: 2px 4px;
      border: 1px solid #7c8692;
      background: #ffffff;
      color: #111;
      min-width: 300px;
      max-width: 460px;
    }

    function findFirstObjectByKeyPattern(value, pattern, depth){
      if (depth > 8 || value === null || value === undefined) return null;
      if (Array.isArray(value)){
        for (let i = 0; i < value.length; i++){
          const found = findFirstObjectByKeyPattern(value[i], pattern, depth + 1);
          if (found) return found;
        }
        return null;
      }

      function getTheatreCenterLonLat(theatreName){
        const t = String(theatreName || '').replace(/[^A-Za-z0-9]/g, '').toUpperCase();
        const centers = {
          CAUCASUS: [44.5, 43.4],
          NEVADA: [-115.2, 36.2],
          NORMANDY: [0.6, 49.2],
          THECHANNEL: [1.2, 51.0],
          PERSIANGULF: [56.2, 25.5],
          SYRIA: [37.0, 35.4],
          MARIANAISLANDS: [145.6, 15.2],
          FALKLANDS: [-59.5, -52.1],
          SINAIMAP: [34.2, 30.1],
          KOLA: [29.5, 68.7],
          AFGHANISTAN: [66.0, 34.5],
          IRAQ: [44.8, 33.2],
          GERMANYCW: [10.2, 51.2],
        };
        return centers[t] || null;
      }

      if (typeof value !== 'object') return null;

      const keys = Object.keys(value);
      for (let i = 0; i < keys.length; i++){
        const k = keys[i];
        const v = value[k];
        if (pattern.test(String(k)) && v && typeof v === 'object'){
          return v;
        }
      }

      for (let i = 0; i < keys.length; i++){
        const found = findFirstObjectByKeyPattern(value[keys[i]], pattern, depth + 1);
        if (found) return found;
      }

      return null;
    }

    function collectScalarRows(prefix, value, rows, depth, maxRows){
      if (rows.length >= maxRows || depth > 8) return;
      if (value === null || value === undefined){
        rows.push({ key: prefix || '-', value: '-' });
        return;
      }

      if (Array.isArray(value)){
        for (let i = 0; i < value.length; i++){
          collectScalarRows((prefix || 'item') + '[' + i + ']', value[i], rows, depth + 1, maxRows);
          if (rows.length >= maxRows) return;
        }
        return;
      }

      if (typeof value === 'object'){
        const keys = Object.keys(value);
        if (!keys.length){
          rows.push({ key: prefix || '-', value: '{}' });
          return;
        }

        keys.forEach(function(k){
          if (rows.length >= maxRows) return;
          const nextPrefix = prefix ? (prefix + '.' + k) : k;
          collectScalarRows(nextPrefix, value[k], rows, depth + 1, maxRows);
        });
        return;
      }

      rows.push({ key: prefix || '-', value: String(value) });
    }

    function isNavPointObject(o){
      if (!o || typeof o !== 'object') return false;
      const hasXY = isFinite(Number(o.x)) && isFinite(Number(o.y));
      const hasLatLon = (isFinite(Number(o.lat)) && isFinite(Number(o.lon)))
        || (isFinite(Number(o.latitude)) && isFinite(Number(o.longitude)));
      const hasWpMeta = o.type || o.action || o.name || o.ETA || o.alt;
      return hasXY || hasLatLon || !!hasWpMeta;
    }

    function collectNavPoints(value, points, depth){
      if (points.length >= 200 || depth > 9 || value === null || value === undefined) return;

      if (Array.isArray(value)){
        value.forEach(function(item){
          if (points.length >= 200) return;
          collectNavPoints(item, points, depth + 1);
        });
        return;
      }

      if (typeof value !== 'object') return;

      if (isNavPointObject(value)){
        points.push(value);
      }

      Object.keys(value).forEach(function(k){
        if (points.length >= 200) return;
        const child = value[k];
        if (child && typeof child === 'object'){
          collectNavPoints(child, points, depth + 1);
        }
      });
    }

    function getMudMapPointType(wp){
      const typeRaw = String((wp && (wp.typeRaw || wp.type || '')) || '').toUpperCase();
      const nameRaw = String((wp && (wp.name || '')) || '').toUpperCase();
      const combined = typeRaw + ' ' + nameRaw;

      if (combined.indexOf('AAR') >= 0 || combined.indexOf('AIR REFUEL') >= 0) return 'aar';
      if (combined.indexOf('CAP') >= 0 || combined.indexOf('COMBAT AIR PATROL') >= 0) return 'cap';
      if (combined.indexOf('HLD') >= 0 || combined.indexOf('HOLD') >= 0) return 'hld';
      if (combined.indexOf('TGT') >= 0 || combined.indexOf('TARGET') >= 0) return 'tgt';
      if (combined.indexOf('IP') >= 0 || combined.indexOf('INITIAL POINT') >= 0 || combined.indexOf('INBOUND POINT') >= 0) return 'ip';
      if (combined.indexOf('DVRT') >= 0 || combined.indexOf('DIVERT') >= 0 || combined.indexOf('DIVERSION') >= 0) return 'dvrt';
      if (combined.indexOf('LDG') >= 0 || combined.indexOf('LAND') >= 0) return 'ldg';
      if (combined.indexOf('LAND') >= 0 || combined.indexOf('HOME') >= 0 || combined.indexOf('BASE') >= 0 || combined.indexOf('TAKEOFF') >= 0) return 'home';
      return 'wp';
    }

    function getMudMapSegments(points){
      const rows = Array.isArray(points) ? points : [];
      const segs = [];
      for (let i = 1; i < rows.length; i++){
        const a = rows[i - 1];
        const b = rows[i];
        const aType = getMudMapPointType(a);
        const bType = getMudMapPointType(b);
        const fromHomeLike = (aType === 'home' || aType === 'ldg');
        const toHomeLike = (bType === 'home' || bType === 'ldg');
        const dashed = (fromHomeLike && !toHomeLike);
        segs.push({ from: a, to: b, dashed: dashed });
      }
      return segs;
    }

    function getMudMapAssets(data){
      const server = (data && data.Server) || {};
      const rawAssets = Array.isArray(server.FriendlyAssets) ? server.FriendlyAssets : [];
      return rawAssets
        .map(function(a){
          const rawX = Number(a && a.X);
          const rawY = Number(a && a.Y);
          if (!isFinite(rawX) || !isFinite(rawY)) return null;
          return {
            callsign: String((a && a.Callsign) || '').trim(),
            name: String((a && a.Name) || '').trim(),
            category: String((a && a.Category) || '').trim().toUpperCase(),
            rawLine: String((a && a.RawLine) || '').trim(),
            rawX: rawX,
            rawY: rawY,
            xNum: rawX,
            yNum: rawY
          };
        })
        .filter(function(a){ return !!a; });
    }

    function normalizeAssetCallsignKey(text){
      return String(text || '').toUpperCase().replace(/[^A-Z0-9]/g, '');
    }

    function parseBraFromUnitLine(line){
      const text = String(line || '').trim();
      if (!text) return null;
      const bra = text.match(/\b(\d{3})\/(\d{1,3})(?:\/|\b)/);
      if (!bra) return null;
      const bearing = Number(bra[1]);
      if (!isFinite(bearing)) return null;
      const csMatch = text.match(/\]\s*([^\s]+)/);
      const callsign = csMatch ? String(csMatch[1] || '').trim() : '';
      if (!callsign) return null;
      return {
        callsignKey: normalizeAssetCallsignKey(callsign),
        bearing: bearing,
      };
    }

    function buildBraBearingMap(data){
      const result = {};
      const cats = ['TANKER', 'AWACS', 'JTAC', 'FLIGHT'];
      cats.forEach(function(cat){
        const lines = getMergedList(data && data.Units, cat);
        (Array.isArray(lines) ? lines : []).forEach(function(line){
          const parsed = parseBraFromUnitLine(line);
          if (!parsed || !parsed.callsignKey || !isFinite(parsed.bearing)) return;
          if (result[parsed.callsignKey] === undefined){
            result[parsed.callsignKey] = parsed.bearing;
          }
        });
      });
      return result;
    }

    function angularDifferenceDeg(a, b){
      const da = Number(a);
      const db = Number(b);
      if (!isFinite(da) || !isFinite(db)) return 180;
      return Math.abs((((da - db) % 360) + 540) % 360 - 180);
    }

    function resolveAssetAxisSwap(assets, data){
      const list = Array.isArray(assets) ? assets : [];
      if (!list.length) return false;

      const ownship = list.find(function(a){ return String(a && a.category || '').toUpperCase() === 'PLAYER'; });
      if (!ownship) return false;

      const ownX = Number(ownship.rawX);
      const ownY = Number(ownship.rawY);
      if (!isFinite(ownX) || !isFinite(ownY)) return false;

      const bearingMap = buildBraBearingMap(data);
      const samples = list.filter(function(a){
        const key = normalizeAssetCallsignKey(a && a.callsign);
        return key && bearingMap[key] !== undefined && a !== ownship;
      }).slice(0, 10);
      if (!samples.length) return false;

      function score(swapped){
        let total = 0;
        let count = 0;
        samples.forEach(function(a){
          const key = normalizeAssetCallsignKey(a.callsign);
          const braBearing = Number(bearingMap[key]);
          const north = swapped ? Number(a.rawY) : Number(a.rawX);
          const east = swapped ? Number(a.rawX) : Number(a.rawY);
          const ownNorth = swapped ? ownY : ownX;
          const ownEast = swapped ? ownX : ownY;
          if (!isFinite(north) || !isFinite(east) || !isFinite(ownNorth) || !isFinite(ownEast) || !isFinite(braBearing)) return;
          const dNorth = north - ownNorth;
          const dEast = east - ownEast;
          if (Math.abs(dNorth) < 0.001 && Math.abs(dEast) < 0.001) return;
          const bearing = normalizeHeadingDeg((Math.atan2(dEast, dNorth) * 180.0 / Math.PI));
          total += angularDifferenceDeg(bearing, braBearing);
          count++;
        });
        return count > 0 ? (total / count) : 999;
      }

      const normalScore = score(false);
      const swappedScore = score(true);
      return swappedScore + 8 < normalScore;
    }

    function getMudMapAssetKind(asset){
      const category = String((asset && asset.category) || '').toUpperCase();
      if (category === 'TANKER') return 'tanker';
      if (category === 'AWACS') return 'awacs';
      if (category === 'JTAC') return 'jtac';
      const text = String((asset && asset.name) || '').toUpperCase();
      if (text.indexOf('HELO') >= 0 || text.indexOf('HELICOPTER') >= 0 || text.indexOf('ROTOR') >= 0) return 'rotary';
      return 'fixed';
    }

    function getDtcMpdRoot(root){
      if (!root || typeof root !== 'object') return null;
      if (root.MPD && typeof root.MPD === 'object') return root.MPD;
      return findFirstObjectByKeyPattern(root, /^MPD$/i, 0);
    }

    function extractIcaoToken(text){
      const s = String(text || '').toUpperCase();
      if (!s) return '';
      const m = s.match(/\b([A-Z]{4})\b/);
      return m ? String(m[1] || '') : '';
    }

    function inferAirfieldType(text){
      const s = String(text || '').toUpperCase();
      if (!s) return 'airport';
      if (s.indexOf('SEAPLANE') >= 0 || s.indexOf('SEA PLANE') >= 0 || s.indexOf('WATER') >= 0) return 'seaplane';
      if (s.indexOf('HELIPORT') >= 0 || s.indexOf('HELI') >= 0 || s.indexOf('FARP') >= 0) return 'heliport';
      return 'airport';
    }

    function inferAirfieldMilitary(text){
      const s = String(text || '').toUpperCase();
      if (!s) return false;
      return s.indexOf('MIL') >= 0
        || s.indexOf('AIRBASE') >= 0
        || s.indexOf('AIR BASE') >= 0
        || s.indexOf('AFB') >= 0
        || s.indexOf('NAS') >= 0
        || s.indexOf('AB ') >= 0
        || s.indexOf(' AFB') >= 0;
    }

    function parseMetarVisibilityMeters(metarText){
      const text = String(metarText || '').toUpperCase().trim();
      if (!text) return NaN;
      if (text.indexOf('CAVOK') >= 0) return 10000;

      const meter = text.match(/(?:^|\s)(\d{4})(?:\s|$)/);
      if (meter){
        const mv = Number(meter[1]);
        if (isFinite(mv) && mv > 0) return mv;
      }

      const sm = text.match(/(?:^|\s)(P?\d{1,2}(?:\s+\d\/\d)?|\d\/\d)SM(?:\s|$)/);
      if (sm){
        const token = String(sm[1] || '').trim();
        let miles = NaN;
        if (token.indexOf('/') >= 0 && token.indexOf(' ') < 0){
          const parts = token.split('/');
          const n = Number(parts[0]);
          const d = Number(parts[1]);
          if (isFinite(n) && isFinite(d) && d > 0) miles = n / d;
        } else {
          const clean = token.replace(/^P/i, '').trim();
          const mixed = clean.match(/^(\d+)\s+(\d)\/(\d)$/);
          if (mixed){
            const whole = Number(mixed[1]);
            const n = Number(mixed[2]);
            const d = Number(mixed[3]);
            if (isFinite(whole) && isFinite(n) && isFinite(d) && d > 0) miles = whole + (n / d);
          } else {
            const n = Number(clean);
            if (isFinite(n)) miles = n;
          }
        }
        if (isFinite(miles) && miles > 0) return miles * 1609.344;
      }

      return NaN;
    }

    function isMetarCloudVfr(metarText){
      const text = String(metarText || '').toUpperCase().trim();
      if (!text) return false;
      if (text.indexOf('CAVOK') >= 0) return true;

      const rx = /\b(FEW|SCT|BKN|OVC)(\d{3})\b/g;
      let match = null;
      let lowSctCount = 0;
      while ((match = rx.exec(text)) !== null){
        const layer = String(match[1] || '').toUpperCase();
        const baseHundreds = Number(match[2]);
        if (!isFinite(baseHundreds)) continue;
        const ft = baseHundreds * 100;
        if (!isFinite(ft)) continue;

        if (ft < 1500){
          if (layer === 'BKN' || layer === 'OVC') return false;
          if (layer === 'SCT'){
            lowSctCount += 1;
            if (lowSctCount >= 2) return false;
          }
        }
      }

      return true;
    }

    function isMetarVfr(metarText){
      const visMeters = parseMetarVisibilityMeters(metarText);
      const cloudVfr = isMetarCloudVfr(metarText);
      return isFinite(visMeters) && cloudVfr && visMeters >= 5000;
    }

    function buildMapAirfields(data){
      const model = data || latestData || {};
      const server = (model && model.Server) || {};
      const diagnostics = (server && server.Diagnostics && typeof server.Diagnostics === 'object') ? server.Diagnostics : {};
      const atcMetars = (server && server.AtcMetars && typeof server.AtcMetars === 'object') ? server.AtcMetars : {};
      const atcIcaoTypes = (server && server.AtcIcaoTypes && typeof server.AtcIcaoTypes === 'object') ? server.AtcIcaoTypes : {};
      const results = [];
      const seen = {};
      const metarKeys = Object.keys(atcMetars).sort(function(a, b){ return String(b || '').length - String(a || '').length; });

      function normalizeOverrideType(value){
        const t = String(value || '').trim().toUpperCase();
        if (t === 'MIL' || t === 'CIV' || t === 'JOINT') return t;
        return '';
      }

      function normalizeOverrideLookupKey(value){
        return String(value || '')
          .toUpperCase()
          .replace(/[\_\-\/\.,\(\)]/g, ' ')
          .replace(/\s+/g, ' ')
          .trim();
      }

      function resolveOverrideType(icao, text, fallbackKey){
        const keyIcao = String(icao || '').toUpperCase();
        const keyText = String(text || '').toUpperCase();
        const keyFallback = String(fallbackKey || '').toUpperCase();
        const normText = normalizeOverrideLookupKey(text);
        const normFallback = normalizeOverrideLookupKey(fallbackKey);
        return normalizeOverrideType(atcIcaoTypes[keyIcao])
          || normalizeOverrideType(atcIcaoTypes[keyText])
          || normalizeOverrideType(atcIcaoTypes[keyFallback])
          || normalizeOverrideType(atcIcaoTypes[normText])
          || normalizeOverrideType(atcIcaoTypes[normFallback])
          || (function(){
            if (!normText && !normFallback) return '';
            const keys = Object.keys(atcIcaoTypes || {});
            for (let i = 0; i < keys.length; i++){
              const k = String(keys[i] || '');
              if (!k) continue;
              const nk = normalizeOverrideLookupKey(k);
              if (!nk) continue;
              if ((normText && (normText === nk || normText.indexOf(nk) >= 0 || nk.indexOf(normText) >= 0))
                || (normFallback && (normFallback === nk || normFallback.indexOf(nk) >= 0 || nk.indexOf(normFallback) >= 0))){
                const t = normalizeOverrideType(atcIcaoTypes[k]);
                if (t) return t;
              }
            }
            return '';
          })()
          || '';
      }

      function extractIcaoFromMetarText(metarText){
        const text = String(metarText || '').toUpperCase();
        if (!text) return '';
        const m = text.match(/\bMETAR\s+([A-Z]{4})\b/);
        return m ? String(m[1] || '') : '';
      }

      function resolveMetarKey(text, explicitIcao){
        const icao = String(explicitIcao || '').toUpperCase();
        if (icao && atcMetars[icao]) return icao;
        const upper = String(text || '').toUpperCase();
        const normUpper = upper.replace(/[^A-Z0-9]/g, '');
        for (let i = 0; i < metarKeys.length; i++){
          const k = String(metarKeys[i] || '').toUpperCase();
          if (!k) continue;
          if (upper.indexOf(k) >= 0) return k;
          const normKey = k.replace(/[^A-Z0-9]/g, '');
          if (normUpper && normKey && (normUpper.indexOf(normKey) >= 0 || normKey.indexOf(normUpper) >= 0)) return k;
        }
        return '';
      }

      function toNumber(v){
        const n = Number(v);
        return isFinite(n) ? n : NaN;
      }

      const atcUnitList = getMergedList(model && model.Units, 'ATC');
      (Array.isArray(atcUnitList) ? atcUnitList : []).forEach(function(line){
        const text = String(line || '').trim();
        if (!text) return;
        const metarKey = resolveMetarKey(text, extractIcaoToken(text));
        if (!metarKey) return;
        const metarText = String(atcMetars[metarKey] || '');
        const m = text.match(/\b(\d{3})\/(\d{1,3})(?:\/(\d{1,3}))?/);
        if (!m) return;

        const bearing = Number(m[1]);
        const rangeNm = Number(m[2]);
        if (!isFinite(bearing) || !isFinite(rangeNm) || rangeNm <= 0) return;

        const originNorth = Number(server && server.PlayerPosX);
        const originEast = Number(server && server.PlayerPosY);
        if (!isFinite(originNorth) || !isFinite(originEast)) return;

        const distM = rangeNm * 1852.0;
        const rad = bearing * (Math.PI / 180.0);
        const north = originNorth + (Math.cos(rad) * distM);
        const east = originEast + (Math.sin(rad) * distM);
        if (!isFinite(north) || !isFinite(east)) return;

        const icao = extractIcaoToken(text) || extractIcaoFromMetarText(metarText) || extractIcaoToken(metarKey);
        if (!icao) return;
        const overrideType = resolveOverrideType(icao, text, metarKey);

        const dedupeKey = String(icao) + '|' + String(Math.round(north)) + '|' + String(Math.round(east));
        if (seen[dedupeKey]) return;
        seen[dedupeKey] = true;

        const type = (overrideType === 'MIL') ? 'airport'
          : (overrideType === 'JOINT' ? 'airport' : inferAirfieldType(text));
        const isMilitary = (overrideType === 'MIL' || overrideType === 'JOINT') ? true : inferAirfieldMilitary(text);

        results.push({
          xNum: north,
          yNum: east,
          label: String(icao).toUpperCase(),
          icao: String(icao).toUpperCase(),
          type: type,
          isMilitary: isMilitary,
          isVfr: isMetarVfr(metarText),
        });
      });

      function tryAddAirfield(row, path){
        if (!row || typeof row !== 'object') return;

        const pos = row.pos && typeof row.pos === 'object' ? row.pos : null;
        const point = row.point && typeof row.point === 'object' ? row.point : null;
        const north = toNumber(row.x !== undefined ? row.x : (row.X !== undefined ? row.X : (pos ? (pos.x !== undefined ? pos.x : pos.X) : (point ? (point.x !== undefined ? point.x : point.X) : undefined))));
        const east = toNumber(
          row.y !== undefined ? row.y
            : (row.Y !== undefined ? row.Y
              : (row.z !== undefined ? row.z
                : (row.Z !== undefined ? row.Z
                  : (pos ? (pos.z !== undefined ? pos.z : (pos.y !== undefined ? pos.y : (pos.Z !== undefined ? pos.Z : pos.Y))) : (point ? (point.z !== undefined ? point.z : (point.y !== undefined ? point.y : (point.Z !== undefined ? point.Z : point.Y))) : undefined)))))
        );
        if (!isFinite(north) || !isFinite(east)) return;

        const textBits = [
          row.icao, row.ICAO, row.name, row.Name, row.callsign, row.Callsign,
          row.fullname, row.FullName, row.displayName, row.DisplayName,
          row.category, row.Category,
          row.airfield, row.airfieldName, row.atcName, row.text, row.note,
          path,
        ];
        const text = textBits.map(function(v){ return String(v || '').trim(); }).filter(function(v){ return !!v; }).join(' ');
        const inferredIcao = extractIcaoToken(String(row.icao || row.ICAO || '')) || extractIcaoToken(text);
        const metarKey = resolveMetarKey(text, inferredIcao);
        const metarText = metarKey ? String(atcMetars[metarKey] || '') : '';
        const metarIcao = extractIcaoFromMetarText(metarText);
        const keyIcao = extractIcaoToken(metarKey);
        const icao = String(inferredIcao || metarIcao || keyIcao || '').toUpperCase();
        const overrideType = resolveOverrideType(icao, text, metarKey);
        const rowCategory = String((row && (row.category || row.Category)) || '').toUpperCase();

        const upperText = text.toUpperCase();
        const looksLikeAirfield = !!icao
          || !!metarKey
          || upperText.indexOf('AIRFIELD') >= 0
          || upperText.indexOf('AIRPORT') >= 0
          || upperText.indexOf('AIRBASE') >= 0
          || upperText.indexOf('ATC') >= 0
          || upperText.indexOf('TOWER') >= 0
          || upperText.indexOf('HELIPORT') >= 0
          || upperText.indexOf('SEAPLANE') >= 0;
        if (!looksLikeAirfield) return;

        const vfr = isMetarVfr(metarText);
        const type = (overrideType === 'MIL') ? 'airport'
          : (overrideType === 'JOINT' ? 'airport' : inferAirfieldType(text));
        const military = (overrideType === 'MIL' || overrideType === 'JOINT') ? true : inferAirfieldMilitary(text);
        const dedupeKey = (icao || '-') + '|' + String(Math.round(north)) + '|' + String(Math.round(east));
        if (seen[dedupeKey]) return;
        seen[dedupeKey] = true;

        results.push({
          xNum: north,
          yNum: east,
          label: icao || 'AF',
          icao: icao,
          type: type,
          isMilitary: military,
          isVfr: vfr,
        });
      }

      const visited = [];
      function scan(node, path, depth){
        if (!node || typeof node !== 'object') return;
        if (depth > 6) return;
        if (visited.indexOf(node) >= 0) return;
        visited.push(node);

        if (Array.isArray(node)){
          for (let i = 0; i < node.length && results.length < 48; i++){
            scan(node[i], path + '[' + String(i) + ']', depth + 1);
          }
          return;
        }

        tryAddAirfield(node, path);
        if (results.length >= 48) return;

        Object.keys(node).forEach(function(k){
          if (results.length >= 48) return;
          const child = node[k];
          if (child && typeof child === 'object'){
            scan(child, path ? (path + '.' + k) : k, depth + 1);
          }
        });
      }

      scan(server.FriendlyAssets, 'friendlyAssets', 0);
      scan(diagnostics, 'diagnostics', 0);
      if (results.length < 48){
        scan(server.Payload, 'payload', 0);
      }

      return results;
    }

    function getDtcMapOverlays(root, routeKey, data){
      const mpd = getDtcMpdRoot(root);
      const sa = (root && typeof root === 'object' && root.SA && typeof root.SA === 'object')
        ? root.SA
        : findFirstObjectByKeyPattern(root, /^SA$/i, 0);
      const f14Slots = getF14RouteSlots(root);
      const route = String(routeKey || 'R1').toUpperCase();
      const selectedF14Slot = f14Slots.find(function(slot){ return String((slot && slot.key) || '').toUpperCase() === route; }) || f14Slots[0] || null;
      const selectedF14Route = selectedF14Slot && selectedF14Slot.route && typeof selectedF14Slot.route === 'object'
        ? selectedF14Slot.route
        : null;

      const allGeoLines = Array.isArray(mpd.GEO_LINES) ? mpd.GEO_LINES : [];
      const geoLines = allGeoLines
        .filter(function(p){
          if (!p || typeof p !== 'object') return false;
          return isFinite(Number(p.x)) && isFinite(Number(p.y));
        })
        .map(function(p){
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          const lineFlags = [];
          if (p && p.L1) lineFlags.push('L1');
          if (p && p.L2) lineFlags.push('L2');
          if (p && p.L3) lineFlags.push('L3');
          if (p && p.L4) lineFlags.push('L4');
          return {
            xNum: xNum,
            yNum: yNum,
            number: Number(p && p.number),
            label: String((p && p.id) || (p && p.note) || '').trim(),
            lineFlags: lineFlags
          };
        })
        .filter(function(p){ return !!p; })
        .sort(function(a, b){
          const an = Number(a && a.number);
          const bn = Number(b && b.number);
          if (isFinite(an) && isFinite(bn) && an !== bn) return an - bn;
          return 0;
        });

      const threatPoints = (Array.isArray(mpd && mpd.THREAT_PTS) ? mpd.THREAT_PTS : [])
        .map(function(p){
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          const radiusMeters = Number(p && p.radius);
          return {
            xNum: xNum,
            yNum: yNum,
            radiusMeters: isFinite(radiusMeters) && radiusMeters > 0 ? radiusMeters : 0,
            ring: !!(p && p.ring),
            label: String((p && p.text) || (p && p.threatName) || '').trim()
          };
        })
        .filter(function(p){ return !!p; });

      const mezThreatPoints = (Array.isArray(sa && sa.MEZ_THRTS) ? sa.MEZ_THRTS : [])
        .map(function(p){
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          const rawRadius = Number(p && p.threat_ring_radius);
          const radiusMeters = isFinite(rawRadius) && rawRadius > 0
            ? (rawRadius > 1000 ? rawRadius : (rawRadius * 1852))
            : 0;
          return {
            xNum: xNum,
            yNum: yNum,
            radiusMeters: radiusMeters,
            ring: radiusMeters > 0,
            label: String((p && p.text) || (p && p.threat_type) || (p && p.id) || '').trim()
          };
        })
        .filter(function(p){ return !!p; });

      const destinationPoints = (Array.isArray(mpd && mpd.DEST) ? mpd.DEST : [])
        .map(function(p){
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          return {
            xNum: xNum,
            yNum: yNum,
            label: String((p && p.text) || (p && p.note) || (p && p.id) || '').trim()
          };
        })
        .filter(function(p){ return !!p; });

      function parseLineCollection(items){
        const rows = Array.isArray(items) ? items : [];
        return rows.map(function(line){
          const points = (Array.isArray(line && line.points) ? line.points : [])
            .map(function(pt){
              const xNum = Number(pt && pt.x);
              const yNum = Number(pt && pt.y);
              if (!isFinite(xNum) || !isFinite(yNum)) return null;
              return {
                xNum: xNum,
                yNum: yNum,
                number: Number(pt && pt.num),
                label: String((pt && pt.id) || '').trim()
              };
            })
            .filter(function(pt){ return !!pt; });
          return {
            id: String((line && line.id) || '').trim(),
            number: Number(line && line.num),
            label: String((line && line.note) || (line && line.id) || '').trim(),
            points: points
          };
        }).filter(function(line){ return line && line.points && line.points.length > 0; });
      }

      const faorRoot = sa && sa.FAOR_FLOT && typeof sa.FAOR_FLOT === 'object' ? sa.FAOR_FLOT : null;
      const faorLines = parseLineCollection(faorRoot && faorRoot.FAOR);
      const flotLines = parseLineCollection(faorRoot && faorRoot.FLOT);

      const capPoints = (Array.isArray(sa && sa.CAP_PTS) ? sa.CAP_PTS : [])
        .map(function(p){
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          return {
            xNum: xNum,
            yNum: yNum,
            number: Number(p && p.num),
            label: String((p && p.note) || (p && p.id) || '').trim(),
            course: Number(p && p.course),
            lengthMeters: Number(p && p.length),
            diameterMeters: Number(p && p.diameter),
            turnDirection: String((p && p.turn_direction) || '').trim()
          };
        })
        .filter(function(p){ return !!p; });

      const corridors = parseLineCollection(sa && sa.CORRIDORS);

      const f14AdditionalPoints = (Array.isArray(selectedF14Route && selectedF14Route.additional_points) ? selectedF14Route.additional_points : [])
        .map(function(p){
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          const nameInfo = getF14WaypointTypeInfo((p && p.name) || '');
          return {
            xNum: xNum,
            yNum: yNum,
            label: String(nameInfo.name || (p && p.name) || '').trim(),
            typeRaw: String(nameInfo.typeRaw || 'WP'),
            isBullseye: !!nameInfo.isBullseye
          };
        })
        .filter(function(p){ return !!p; });

      const f14Lines = (Array.isArray(selectedF14Route && selectedF14Route.lines) ? selectedF14Route.lines : [])
        .map(function(line, lineIdx){
          const points = (Array.isArray(line && line.points) ? line.points : [])
            .map(function(pt, pointIdx){
              const xNum = Number(pt && pt.x);
              const yNum = Number(pt && pt.y);
              if (!isFinite(xNum) || !isFinite(yNum)) return null;
              return {
                xNum: xNum,
                yNum: yNum,
                number: isFinite(Number(pt && pt.number)) ? Number(pt.number) : (pointIdx + 1),
                label: String((pt && pt.name) || '').trim()
              };
            })
            .filter(function(pt){ return !!pt; });
          return {
            id: 'F14-LINE-' + String(lineIdx + 1),
            number: lineIdx + 1,
            label: String((line && line.name) || '').trim(),
            points: points
          };
        })
        .filter(function(line){ return line && Array.isArray(line.points) && line.points.length > 1; });

      const jdamThreatPoints = (Array.isArray(root && root.JDAM && root.JDAM.stations) ? root.JDAM.stations : [])
        .reduce(function(acc, station){
          const targets = Array.isArray(station && station.targets) ? station.targets : [];
          targets.forEach(function(target){
            const active = !!(target && target.active);
            const xNum = Number(target && target.x);
            const yNum = Number(target && target.y);
            if (!active || !isFinite(xNum) || !isFinite(yNum)) return;
            acc.push({
              xNum: xNum,
              yNum: yNum,
              radiusMeters: 0,
              ring: false,
              label: String((target && target.name) || 'DMPI').trim() || 'DMPI'
            });
          });
          return acc;
        }, []);

      const f14BullseyeDestinations = f14AdditionalPoints
        .filter(function(p){ return !!(p && p.isBullseye); })
        .map(function(p){
          return {
            xNum: Number(p.xNum),
            yNum: Number(p.yNum),
            label: String(p.label || 'BULLSEYE')
          };
        });

      const f14NamedDestinations = f14AdditionalPoints
        .filter(function(p){
          const typeRaw = String((p && p.typeRaw) || '').toUpperCase();
          return typeRaw === 'DEST' || typeRaw === 'LANTIRN' || typeRaw === 'IP';
        })
        .map(function(p){
          return {
            xNum: Number(p.xNum),
            yNum: Number(p.yNum),
            label: String(p.label || '').trim()
          };
        });

      return {
        geolines: geoLines,
        threatPoints: threatPoints.concat(mezThreatPoints).concat(jdamThreatPoints),
        destinationPoints: destinationPoints.concat(f14BullseyeDestinations).concat(f14NamedDestinations),
        faorLines: faorLines,
        flotLines: flotLines,
        capPoints: capPoints,
        corridors: corridors.concat(f14Lines)
      };
    }

    function isBullseyeText(text){
      const s = String(text || '').trim().toUpperCase();
      if (!s) return false;
      if (s === 'BULL' || s === 'BULLSEYE') return true;
      if (s.indexOf('BULLSEYE') >= 0) return true;
      return /(^|\W)BULL(\W|$)/.test(s);
    }

    function toBullseyePoint(x, y, label){
      const xNum = Number(x);
      const yNum = Number(y);
      if (!isFinite(xNum) || !isFinite(yNum)) return null;
      const safeLabel = String(label || '').trim();
      return {
        xNum: xNum,
        yNum: yNum,
        label: safeLabel || 'BULLSEYE'
      };
    }

    function getMapBullseyePoint(root, waypoints, overlays, data){
      const model = data || latestData || {};
      const server = (model && model.Server) || {};
      const diagnostics = (server && typeof server.Diagnostics === 'object' && server.Diagnostics) || {};
      const diagX = Number(diagnostics && diagnostics.bullseyeX);
      const diagY = Number(diagnostics && diagnostics.bullseyeY);
      const diagValid = !!(diagnostics && diagnostics.bullseyeValid);
      if (diagValid && isFinite(diagX) && isFinite(diagY)){
        return {
          xNum: diagX,
          yNum: diagY,
          label: 'B/E'
        };
      }

      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];

      for (let i = 0; i < navPts.length; i++){
        const p = navPts[i] || {};
        const label = String(p.note || p.text_note || p.name || p.text || p.id || '').trim();
        if (!isBullseyeText(label)) continue;
        const found = toBullseyePoint(p.x, p.y, label);
        if (found) return found;
      }

      const rows = Array.isArray(waypoints) ? waypoints : [];
      for (let i = 0; i < rows.length; i++){
        const wp = rows[i] || {};
        const label = String(wp.name || wp.label || wp.note || '').trim();
        if (!isBullseyeText(label)) continue;
        const found = toBullseyePoint(wp.xNum, wp.yNum, label);
        if (found) return found;
      }

      const mapOverlays = overlays && typeof overlays === 'object' ? overlays : {};
      const destinationPoints = Array.isArray(mapOverlays.destinationPoints) ? mapOverlays.destinationPoints : [];
      for (let i = 0; i < destinationPoints.length; i++){
        const p = destinationPoints[i] || {};
        const label = String(p.label || p.text || p.note || p.id || '').trim();
        if (!isBullseyeText(label)) continue;
        const found = toBullseyePoint(p.xNum, p.yNum, label);
        if (found) return found;
      }

      return null;
    }

    function buildOpenFreeMapPayload(waypoints, data, overlays, bullseyePoint){
      const rows = Array.isArray(waypoints) ? waypoints.filter(function(wp){
        return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum));
      }) : [];
      const model = data || latestData || {};
      const server = (model && model.Server) || {};
      const diagnostics = (server && server.Diagnostics) || {};
      const theatreCandidates = [
        server.Theater,
        diagnostics.theater,
        diagnostics.terrain,
        diagnostics.terrainName,
        lastKnownTheater,
      ];
      let theatre = '';
      for (let i = 0; i < theatreCandidates.length; i++){
        const candidate = String(theatreCandidates[i] || '').trim();
        if (!candidate) continue;
        if (getMapProjectionByTheatre(candidate)){
          theatre = candidate;
          break;
        }
      }
      if (!theatre){
        openFreeMapLastPayloadStatus = 'no-theatre';
        return null;
      }

      const mapOverlays = overlays && typeof overlays === 'object'
        ? overlays
        : { geolines: [], threatPoints: [], destinationPoints: [], faorLines: [], flotLines: [], capPoints: [], corridors: [] };

      const approxCenterLonLat = getTheatreCenterLonLat(theatre);
      let approxAnchorNorth = NaN;
      let approxAnchorEast = NaN;
      let approxAnchorReady = false;
      let usedApproxProjection = false;

      (function initApproxAnchor(){
        if (!approxCenterLonLat) return;
        const rawPoints = [];

        function pushPoint(point){
          const n = Number(point && point.xNum);
          const e = Number(point && point.yNum);
          if (!isFinite(n) || !isFinite(e)) return;
          rawPoints.push({ n: n, e: e });
        }

        rows.forEach(pushPoint);
        (Array.isArray(mapOverlays.geolines) ? mapOverlays.geolines : []).forEach(pushPoint);
        (Array.isArray(mapOverlays.threatPoints) ? mapOverlays.threatPoints : []).forEach(pushPoint);
        (Array.isArray(mapOverlays.destinationPoints) ? mapOverlays.destinationPoints : []).forEach(pushPoint);
        (Array.isArray(mapOverlays.capPoints) ? mapOverlays.capPoints : []).forEach(pushPoint);

        function pushLineGroupPoints(groups){
          (Array.isArray(groups) ? groups : []).forEach(function(group){
            (Array.isArray(group && group.points) ? group.points : []).forEach(pushPoint);
          });
        }

        pushLineGroupPoints(mapOverlays.faorLines);
        pushLineGroupPoints(mapOverlays.flotLines);
        pushLineGroupPoints(mapOverlays.corridors);

        const rawAssets = getMudMapAssets(model, dlinkOnEnabled);
        rawAssets.forEach(pushPoint);
        if (bullseyePoint) pushPoint(bullseyePoint);

        if (!rawPoints.length) return;
        const sumN = rawPoints.reduce(function(acc, p){ return acc + p.n; }, 0);
        const sumE = rawPoints.reduce(function(acc, p){ return acc + p.e; }, 0);
        approxAnchorNorth = sumN / rawPoints.length;
        approxAnchorEast = sumE / rawPoints.length;
        approxAnchorReady = isFinite(approxAnchorNorth) && isFinite(approxAnchorEast);
      })();

      function toLonLat(point){
        const north = Number(point && point.xNum);
        const east = Number(point && point.yNum);
        if (!isFinite(north) || !isFinite(east)) return null;

        function validLonLat(lon, lat){
          return isFinite(Number(lon))
            && isFinite(Number(lat))
            && Math.abs(Number(lat)) <= 90
            && Math.abs(Number(lon)) <= 180;
        }

        const llPrimary = convertDcsXYToLatLon(theatre, north, east);
        if (llPrimary && validLonLat(llPrimary.lon, llPrimary.lat)){
          return [Number(llPrimary.lon), Number(llPrimary.lat)];
        }

        const llSwapped = convertDcsXYToLatLon(theatre, east, north);
        if (llSwapped && validLonLat(llSwapped.lon, llSwapped.lat)){
          return [Number(llSwapped.lon), Number(llSwapped.lat)];
        }

        if (validLonLat(east, north)){
          return [east, north];
        }
        if (validLonLat(north, east)){
          return [north, east];
        }

        if (approxAnchorReady && Array.isArray(approxCenterLonLat) && approxCenterLonLat.length === 2){
          const centerLon = Number(approxCenterLonLat[0]);
          const centerLat = Number(approxCenterLonLat[1]);
          if (isFinite(centerLon) && isFinite(centerLat)){
            const dNorth = north - approxAnchorNorth;
            const dEast = east - approxAnchorEast;
            const lat = centerLat + (dNorth / 111320.0);
            const lonScale = 111320.0 * Math.max(0.2, Math.cos(centerLat * Math.PI / 180.0));
            const lon = centerLon + (dEast / lonScale);
            if (validLonLat(lon, lat)){
              usedApproxProjection = true;
              return [lon, lat];
            }
          }
        }

        return null;
      }

      const pointsForBounds = [];
      const features = [];

      function addPointFeature(point, props){
        const lonLat = toLonLat(point);
        if (!lonLat) return;
        pointsForBounds.push(lonLat);
        features.push({
          type: 'Feature',
          geometry: {
            type: 'Point',
            coordinates: lonLat,
          },
          properties: props || {},
        });
      }

      function addLineFeature(points, props){
        const coords = (Array.isArray(points) ? points : [])
          .map(function(p){ return toLonLat(p); })
          .filter(function(c){ return Array.isArray(c) && c.length === 2; });
        if (coords.length < 2) return;
        coords.forEach(function(c){ pointsForBounds.push(c); });
        features.push({
          type: 'Feature',
          geometry: {
            type: 'LineString',
            coordinates: coords,
          },
          properties: props || {},
        });
      }

      const byStep = {};
      rows.forEach(function(wp){ byStep[String(wp && wp.step)] = wp; });
      getMudMapSegments(rows).forEach(function(seg){
        const from = byStep[String(seg && seg.from && seg.from.step)] || (seg && seg.from);
        const to = byStep[String(seg && seg.to && seg.to.step)] || (seg && seg.to);
        if (!from || !to) return;
        addLineFeature([from, to], {
          stroke: '#2f5fa7',
          lineWidth: seg && seg.dashed ? 1.8 : 2.2,
          dashed: !!(seg && seg.dashed),
        });
      });

      rows.forEach(function(wp){
        const step = String(wp && wp.step || '').trim();
        const name = String(wp && wp.name || '').trim();
        const label = step ? (step + (name ? (' ' + name) : '')) : name;
        addPointFeature(wp, {
          kind: 'waypoint',
          group: 'overlay',
          label: label || 'WP',
          fill: '#2d8fe3',
          stroke: '#1f5d93',
          textColor: '#1f3550',
          radius: 4,
        });
      });

      (Array.isArray(mapOverlays.geolines) ? mapOverlays.geolines : []).forEach(function(p){
        addPointFeature(p, {
          kind: 'geoline',
          group: 'overlay',
          label: String((p && p.label) || '').trim(),
          fill: '#7a57b3',
          stroke: '#5a3d89',
          textColor: '#5a3d89',
          radius: 3,
        });
      });

      function addGroupedLines(groups, strokeColor){
        (Array.isArray(groups) ? groups : []).forEach(function(g){
          const pts = (Array.isArray(g && g.points) ? g.points : []);
          addLineFeature(pts, { stroke: strokeColor, lineWidth: 2, dashed: true });
        });
      }

      function addCorridorBounds(groups){
        const corridorHalfWidthMeters = 5000;
        (Array.isArray(groups) ? groups : []).forEach(function(group){
          const pts = (Array.isArray(group && group.points) ? group.points : [])
            .map(function(p){
              return {
                xNum: Number(p && p.xNum),
                yNum: Number(p && p.yNum),
                number: Number(p && p.number)
              };
            })
            .filter(function(p){ return isFinite(p.xNum) && isFinite(p.yNum); })
            .sort(function(a, b){
              if (isFinite(a.number) && isFinite(b.number) && a.number !== b.number) return a.number - b.number;
              return 0;
            });
          if (pts.length < 2) return;

          const left = [];
          const right = [];
          for (let i = 0; i < pts.length; i++){
            const prev = pts[Math.max(0, i - 1)];
            const next = pts[Math.min(pts.length - 1, i + 1)];
            const dNorth = Number(next.xNum) - Number(prev.xNum);
            const dEast = Number(next.yNum) - Number(prev.yNum);
            const len = Math.sqrt((dNorth * dNorth) + (dEast * dEast));
            if (!isFinite(len) || len <= 0){
              left.push({ xNum: pts[i].xNum, yNum: pts[i].yNum });
              right.push({ xNum: pts[i].xNum, yNum: pts[i].yNum });
              continue;
            }
            const unitEast = dEast / len;
            const unitNorth = dNorth / len;
            left.push({
              xNum: pts[i].xNum + ((-unitEast) * corridorHalfWidthMeters),
              yNum: pts[i].yNum + (unitNorth * corridorHalfWidthMeters)
            });
            right.push({
              xNum: pts[i].xNum - ((-unitEast) * corridorHalfWidthMeters),
              yNum: pts[i].yNum - (unitNorth * corridorHalfWidthMeters)
            });
          }

          addLineFeature(left, { stroke: '#3c7cc0', lineWidth: 1.9, dashed: true });
          addLineFeature(right, { stroke: '#3c7cc0', lineWidth: 1.9, dashed: true });
        });
      }

      const geolineRows = Array.isArray(mapOverlays.geolines) ? mapOverlays.geolines : [];
      ['L1', 'L2', 'L3', 'L4'].forEach(function(lineKey){
        const linePoints = geolineRows
          .filter(function(p){
            const flags = (p && Array.isArray(p.lineFlags)) ? p.lineFlags : [];
            return flags.indexOf(lineKey) >= 0;
          })
          .sort(function(a, b){
            const an = Number(a && a.number);
            const bn = Number(b && b.number);
            if (isFinite(an) && isFinite(bn) && an !== bn) return an - bn;
            return 0;
          });
        if (linePoints.length >= 2){
          addLineFeature(linePoints, { stroke: '#7a57b3', lineWidth: 2.2, dashed: true });
        }
      });

      addGroupedLines(mapOverlays.faorLines, '#1f9fd0');
      addGroupedLines(mapOverlays.flotLines, '#c74b4b');
      addCorridorBounds(mapOverlays.corridors);

      (Array.isArray(mapOverlays.destinationPoints) ? mapOverlays.destinationPoints : []).forEach(function(p){
        addPointFeature(p, {
          kind: 'destination',
          group: 'overlay',
          label: String((p && p.label) || '').trim(),
          fill: '#d4a42f',
          stroke: '#7a5a14',
          textColor: '#6a4f16',
          radius: 4.4,
        });
      });

      (Array.isArray(mapOverlays.threatPoints) ? mapOverlays.threatPoints : []).forEach(function(p){
        addPointFeature(p, {
          kind: 'threat',
          group: 'overlay',
          label: String((p && p.label) || '').trim(),
          fill: '#bc3e3e',
          stroke: '#8b2d2d',
          textColor: '#8b2d2d',
          radius: 4.2,
        });
      });

      if (bullseyePoint){
        addPointFeature(bullseyePoint, {
          kind: 'bullseye',
          group: 'overlay',
          label: String((bullseyePoint && bullseyePoint.label) || 'BULLSEYE'),
          fill: '#f0c544',
          stroke: '#7a6420',
          textColor: '#5f4b1b',
          radius: 5,
        });
      }

      (Array.isArray(mapOverlays.airfields) ? mapOverlays.airfields : []).forEach(function(p){
        const isVfr = !!(p && p.isVfr);
        const stroke = isVfr ? '#4aa360' : '#3a8fd0';
        addPointFeature(p, {
          kind: 'airfield',
          group: 'overlay',
          label: String((p && p.icao) || (p && p.label) || 'AF'),
          airfieldType: String((p && p.type) || 'airport').toLowerCase(),
          airfieldMilitary: !!(p && p.isMilitary),
          isVfr: isVfr,
          fill: '#ffffff',
          stroke: stroke,
          textColor: stroke,
          radius: 5,
        });
      });

      const selected = getActiveFlightPlanSelection(model);
      const rawAssets = getMudMapAssets(model, dlinkOnEnabled);
      rawAssets.forEach(function(asset){
        const category = String((asset && asset.category) || '').toUpperCase();
        if (category === 'ATC') return;
        const isPlayer = category === 'PLAYER';
        const callsign = String((asset && asset.callsign) || '').trim();
        const label = callsign || String((asset && asset.name) || category || 'ASSET').trim();
        addPointFeature(asset, {
          kind: isPlayer ? 'player' : 'asset',
          group: 'asset',
          assetKind: getMudMapAssetKind(asset),
          assetKey: makeMapAssetSelectionKey(asset),
          label: label,
          typeName: String((asset && asset.typeName) || ''),
          frequency: String((asset && asset.frequency) || ''),
          tacan: String((asset && asset.tacan) || ''),
          mpClientCallsign: String((asset && asset.mpClientCallsign) || ''),
          fill: isPlayer ? '#2f9e56' : '#5a7ea5',
          stroke: isPlayer ? '#1e6a39' : '#385676',
          textColor: isPlayer ? '#1e6a39' : '#1f3550',
          radius: isPlayer ? 5 : 3.6,
        });
      });

      if (!pointsForBounds.length){
        const centerOnly = getTheatreCenterLonLat(theatre);
        if (!centerOnly) {
          openFreeMapLastPayloadStatus = 'no-converted-points';
          return null;
        }
        openFreeMapLastPayloadStatus = 'base-only/no-overlay-points';
        return {
          selected: selected,
          theatre: theatre,
          centerLon: Number(centerOnly[0]),
          centerLat: Number(centerOnly[1]),
          bounds: [Number(centerOnly[0]) - 1.0, Number(centerOnly[1]) - 1.0, Number(centerOnly[0]) + 1.0, Number(centerOnly[1]) + 1.0],
          features: [],
        };
      }

      const lons = pointsForBounds.map(function(c){ return Number(c[0]); }).filter(function(v){ return isFinite(v); });
      const lats = pointsForBounds.map(function(c){ return Number(c[1]); }).filter(function(v){ return isFinite(v); });
      if (!lons.length || !lats.length){
        const centerOnly = getTheatreCenterLonLat(theatre);
        if (!centerOnly){
          openFreeMapLastPayloadStatus = 'invalid-bounds';
          return null;
        }
        openFreeMapLastPayloadStatus = 'base-only/invalid-bounds';
        return {
          selected: selected,
          theatre: theatre,
          centerLon: Number(centerOnly[0]),
          centerLat: Number(centerOnly[1]),
          bounds: [Number(centerOnly[0]) - 1.0, Number(centerOnly[1]) - 1.0, Number(centerOnly[0]) + 1.0, Number(centerOnly[1]) + 1.0],
          features: [],
        };
      }

      const minLon = Math.min.apply(null, lons);
      const maxLon = Math.max.apply(null, lons);
      const minLat = Math.min.apply(null, lats);
      const maxLat = Math.max.apply(null, lats);
      const centerLon = (minLon + maxLon) / 2;
      const centerLat = (minLat + maxLat) / 2;

      openFreeMapLastPayloadStatus = 'ok pts=' + String(pointsForBounds.length) + ' feat=' + String(features.length) + (usedApproxProjection ? ' approx=1' : '');
      return {
        selected: selected,
        theatre: theatre,
        centerLon: centerLon,
        centerLat: centerLat,
        bounds: [minLon, minLat, maxLon, maxLat],
        features: features,
      };
    }

    function buildMudMapSvg(waypoints, data, overlays, bullseyePoint){
      const rows = Array.isArray(waypoints) ? waypoints.filter(function(wp){
        return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum));
      }) : [];
      const mapOverlays = overlays && typeof overlays === 'object'
        ? overlays
        : { geolines: [], threatPoints: [], destinationPoints: [], faorLines: [], flotLines: [], capPoints: [], corridors: [] };
      const geolines = Array.isArray(mapOverlays.geolines) ? mapOverlays.geolines : [];
      const threatPoints = Array.isArray(mapOverlays.threatPoints) ? mapOverlays.threatPoints : [];
      const destinationPoints = Array.isArray(mapOverlays.destinationPoints) ? mapOverlays.destinationPoints : [];
      const airfields = Array.isArray(mapOverlays.airfields) ? mapOverlays.airfields : [];
      const faorLines = Array.isArray(mapOverlays.faorLines) ? mapOverlays.faorLines : [];
      const flotLines = Array.isArray(mapOverlays.flotLines) ? mapOverlays.flotLines : [];
      const capPoints = Array.isArray(mapOverlays.capPoints) ? mapOverlays.capPoints : [];
      const corridors = Array.isArray(mapOverlays.corridors) ? mapOverlays.corridors : [];

      if (!rows.length){
        return '<div class=""fltPlanMessage"">No mappable waypoint coordinates found.</div>';
      }

      const width = 920;
      const height = 760;
      const pad = 54;

      const eastValues = rows.map(function(wp){ return Number(wp.yNum); });
      const northValues = rows.map(function(wp){ return Number(wp.xNum); });
      const minEast = Math.min.apply(null, eastValues);
      const maxEast = Math.max.apply(null, eastValues);
      const minNorth = Math.min.apply(null, northValues);
      const maxNorth = Math.max.apply(null, northValues);

      const spanEast = Math.max(1, maxEast - minEast);
      const spanNorth = Math.max(1, maxNorth - minNorth);
      const scaleX = (width - (pad * 2)) / spanEast;
      const scaleY = (height - (pad * 2)) / spanNorth;
      const scale = Math.min(scaleX, scaleY);
      const drawW = spanEast * scale;
      const drawH = spanNorth * scale;
      const offsetX = (width - drawW) / 2;
      const offsetY = (height - drawH) / 2;

      function mapPt(wp){
        const north = Number(wp.xNum);
        const east = Number(wp.yNum);
        const sx = offsetX + ((east - minEast) * scale);
        const sy = offsetY + ((maxNorth - north) * scale);
        return { x: sx, y: sy, north: north, east: east };
      }

      const mapped = rows.map(function(wp){
        const p = mapPt(wp);
        return { wp: wp, x: p.x, y: p.y, north: p.north, east: p.east, kind: getMudMapPointType(wp) };
      });

      const labelGroups = {};
      mapped.forEach(function(m){
        const key = String(Math.round(m.x / 10)) + '|' + String(Math.round(m.y / 10));
        if (!labelGroups[key]) labelGroups[key] = [];
        labelGroups[key].push(m);
      });

      Object.keys(labelGroups).forEach(function(k){
        const group = labelGroups[k];
        if (!group || !group.length) return;
        group.sort(function(a, b){
          return Number(a && a.wp && a.wp.step) - Number(b && b.wp && b.wp.step);
        });
        const offsets = [
          { dx: 11, dy: -11 },
          { dx: 11, dy: -24 },
          { dx: 11, dy: 2 },
          { dx: 11, dy: -37 },
          { dx: 11, dy: 15 },
          { dx: -66, dy: -11 },
          { dx: -66, dy: -24 },
          { dx: -66, dy: 2 },
          { dx: -66, dy: 15 },
        ];
        for (let i = 0; i < group.length; i++){
          const m = group[i];
          const o = offsets[i % offsets.length];
          m.labelDx = o.dx;
          m.labelDy = o.dy;
        }
      });

      const byStep = {};
      mapped.forEach(function(m){ byStep[String(m.wp.step)] = m; });
      const segments = getMudMapSegments(rows)
        .map(function(seg){
          return {
            from: byStep[String(seg.from.step)],
            to: byStep[String(seg.to.step)],
            dashed: !!seg.dashed
          };
        })
        .filter(function(seg){ return !!(seg.from && seg.to); });

      const lineEls = segments.map(function(seg){
        return '<line x1=""' + seg.from.x.toFixed(1) + '"" y1=""' + seg.from.y.toFixed(1) + '"" x2=""' + seg.to.x.toFixed(1) + '"" y2=""' + seg.to.y.toFixed(1) + '"" stroke=""#3d566e"" stroke-width=""2""' + (seg.dashed ? ' stroke-dasharray=""8 6""' : '') + ' />';
      });

      function iconFor(m){
        const x = m.x.toFixed(1);
        const y = m.y.toFixed(1);
        function racetrack(colorHex, label){
          const w = 64;
          const h = 32;
          const rx = 14;
          return '<g><rect x=""' + (m.x - (w / 2)).toFixed(1) + '"" y=""' + (m.y - (h / 2)).toFixed(1) + '"" width=""' + w + '"" height=""' + h + '"" rx=""' + rx + '"" ry=""' + rx + '"" fill=""#ffffff"" stroke=""' + colorHex + '"" stroke-width=""2.2"" /><text x=""' + x + '"" y=""' + (m.y + 5.5).toFixed(1) + '"" text-anchor=""middle"" font-size=""14"" fill=""' + colorHex + '"" font-weight=""700"">' + label + '</text></g>';
        }
        if (m.kind === 'aar') return racetrack('#111111', 'AAR');
        if (m.kind === 'cap') return racetrack('#2f5fa7', 'CAP');
        if (m.kind === 'hld') return racetrack('#2f7f4f', 'HLD');
        if (m.kind === 'ip'){
          const s = 9;
          return '<rect x=""' + (m.x - s).toFixed(1) + '"" y=""' + (m.y - s).toFixed(1) + '"" width=""' + (s * 2) + '"" height=""' + (s * 2) + '"" fill=""#2f7f4f"" stroke=""#1e5535"" stroke-width=""1.5"" />';
        }
        if (m.kind === 'tgt'){
          const p1 = x + ',' + (m.y - 10).toFixed(1);
          const p2 = (m.x - 10).toFixed(1) + ',' + (m.y + 8).toFixed(1);
          const p3 = (m.x + 10).toFixed(1) + ',' + (m.y + 8).toFixed(1);
          return '<polygon points=""' + p1 + ' ' + p2 + ' ' + p3 + '"" fill=""#a33d3d"" stroke=""#682626"" stroke-width=""1.5"" />';
        }
        if (m.kind === 'home' || m.kind === 'ldg'){
          const r = 10;
          const roofTop = x + ',' + (m.y - 12).toFixed(1);
          const roofL = (m.x - r).toFixed(1) + ',' + (m.y - 2).toFixed(1);
          const roofR = (m.x + r).toFixed(1) + ',' + (m.y - 2).toFixed(1);
          const baseX = (m.x - 8).toFixed(1);
          const baseY = (m.y - 2).toFixed(1);
          return '<polygon points=""' + roofTop + ' ' + roofL + ' ' + roofR + '"" fill=""#735c2f"" stroke=""#4c3d1f"" stroke-width=""1.5"" /><rect x=""' + baseX + '"" y=""' + baseY + '"" width=""16"" height=""12"" fill=""#b1945a"" stroke=""#4c3d1f"" stroke-width=""1.5"" />';
        }
        return '<circle cx=""' + x + '"" cy=""' + y + '"" r=""8"" fill=""#3a6ea5"" stroke=""#244766"" stroke-width=""1.5"" />';
      }

      const pointEls = mapped.map(function(m){
        const step = escapeHtml(String(m.wp.step || '-'));
        const name = escapeHtml(String(m.wp.name || ''));
        const label = name && name !== '-' ? ('STP ' + step + ' ' + name) : ('STP ' + step);
        const tx = (m.x + 11).toFixed(1);
        const ty = (m.y - 11).toFixed(1);
        return iconFor(m)
          + '<text x=""' + tx + '"" y=""' + ty + '"" font-size=""11"" fill=""#1f2e3d"" font-weight=""700"">' + label + '</text>';
      });

      const northArrow = [
        '<g transform=""translate(' + (width - 46) + ',44)"">',
        '<line x1=""0"" y1=""18"" x2=""0"" y2=""-10"" stroke=""#263748"" stroke-width=""2"" />',
        '<polygon points=""0,-18 -6,-6 6,-6"" fill=""#263748"" />',
        '<text x=""0"" y=""32"" text-anchor=""middle"" font-size=""12"" fill=""#263748"" font-weight=""700"">N</text>',
        '</g>'
      ].join('');

      return '<svg viewBox=""0 0 ' + width + ' ' + height + '"" class=""fltPlanPage3Canvas"" preserveAspectRatio=""xMidYMid meet"">'
        + '<rect x=""0"" y=""0"" width=""' + width + '"" height=""' + height + '"" fill=""#ffffff"" />'
        + lineEls.join('')
        + pointEls.join('')
        + northArrow
        + '</svg>';
    }

    function formatDtcPage3Html(pageSwitcherHtml, waypoints){
      let html = '<div class=""fltPlanBoard"">';
      if (pageSwitcherHtml){
        html += '<div style=""margin:4px 0 6px 0;"">' + pageSwitcherHtml + '</div>';
      }
      html += '<div class=""fltPlanPage3Wrap"">';
      html += '<div class=""fltPlanPage3Legend"">Mud Map (North Up): WP ○, IP □, TGT △, AAR/CAP/HLD racetrack, LDG/HOME ⌂. Dashed leg indicates diversion from HOME/LAND to next point.</div>';
      html += buildMudMapSvg(waypoints);
      html += '</div></div>';
      return html;
    }

    function formatDtcFocusedTable(root, selected){
      const cmdsRoot = findFirstObjectByKeyPattern(root, /(cmds|countermeasures|countermeasure|cmds)/i, 0);
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
      if (!cmdRows.length){
        lines.push('No CMDS data found.');
      } else {
        const maxKeyWidth = cmdRows.reduce(function(acc, r){ return Math.max(acc, String(r.key || '').length); }, 8);
        const keyWidth = clamp(maxKeyWidth + 1, 20, 64);
        cmdRows.forEach(function(r){
          lines.push(String(r.key || '-').padEnd(keyWidth) + String(r.value || '-').replace(/\s+/g, ' ').trim());
        });
      }

      lines.push('');
      lines.push('NAV POINTS');
      lines.push('----------');
      lines.push('WP  NAME         TYPE           ALT      ETA       LAT/LON               X            Y');
      lines.push('--- ------------ -------------- -------- -------- --------------------- ------------ ------------');

      if (!navPoints.length){
        lines.push('No nav points found.');
      } else {
        navPoints.slice(0, 200).forEach(function(p, idx){
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
    .dtcSelector { margin: 0 0 8px 0; box-sizing: border-box; }
    .dtcFileList { max-height: 180px; overflow-y: auto; display: flex; flex-direction: column; gap: 4px; }
    .dtcFileItem {
      width: 100%;
      text-align: left;
      border: 1px solid #6d7d8d;
      background: #ffffff;
      color: #111;
      padding: 4px 6px;
      font-family: inherit;
      font-size: 16px;
      cursor: pointer;
      display: flex;
      justify-content: space-between;
      gap: 8px;
    }
    .dtcFileItem.active { border-color: #f2d76a; box-shadow: inset 0 0 0 1px rgba(242, 215, 106, 0.75); font-weight: 700; }
    .dtcFileMeta { color: #4b5f73; font-size: 14px; }
    .fltPlanBoard { border: 1px solid #7c8692; background: #f5f5f3; color: #111; min-height: 100%; box-sizing: border-box; padding: 8px; display: flex; flex-direction: column; }
    .fltPlanHeaderGrid { display: grid; grid-template-columns: 1fr 1fr 1fr 1fr; gap: 6px; margin-bottom: 8px; }
    .fltPlanHeaderCell { border: 1px solid #8b96a1; background: #ecefed; padding: 4px 6px; min-height: 36px; }
    .fltPlanHeaderCellLabel { font-size: 13px; color: #2f3d4a; margin-bottom: 2px; }
    .fltPlanHeaderCellValue { font-size: 17px; font-weight: 700; color: #111; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
    .fltPlanTimeGrid { display: grid; grid-template-columns: repeat(4, minmax(120px, 1fr)); gap: 6px; margin-bottom: 8px; }
    .fltPlanTimeCell { border: 1px solid #8b96a1; background: #edf2f6; padding: 3px 5px; }
    .fltPlanTimeCellLabel { font-size: 12px; color: #2f3d4a; }
    .fltPlanTimeCellValue { font-size: 16px; font-weight: 700; color: #111; }
    .fltPlanTimeCell.clickable { cursor: pointer; }
    .fltPlanTimeCell.clickable:hover { background: #dce8f2; }
    .fltPlanPostFlightCell { grid-column: 2 / span 3; display: flex; align-items: center; justify-content: flex-end; }
    .fltPlanCrewWrap { border: 1px solid #8b96a1; background: #f8fafc; margin-bottom: 8px; }
    .fltPlanCrewTable { width: 100%; border-collapse: collapse; table-layout: fixed; }
    .fltPlanCrewTable th, .fltPlanCrewTable td { border: 1px solid #a3adb6; padding: 3px 5px; font-size: 14px; line-height: 1.15; text-align: left; }
    .fltPlanCrewTable th { background: #e4e8ec; font-weight: 700; }
    .fltPlanCrewTable td.fltPlanCrewJet { width: 80px; text-align: center; font-weight: 700; }
    .fltPlanCrewTable td.fltPlanCrewPilot { width: auto; }
    .fltPlanWpWrap { border: 1px solid #8b96a1; background: #ffffff; flex: 1 1 auto; min-height: 0; display: flex; flex-direction: column; }
    .fltPlanWpTitle { padding: 4px 6px; border-bottom: 1px solid #8b96a1; font-size: 16px; font-weight: 700; text-transform: uppercase; }
    .fltPlanWpTable { width: 100%; border-collapse: collapse; table-layout: fixed; }
    .fltPlanWpTableWrap { flex: 1 1 auto; min-height: 0; overflow: auto; }
    .fltPlanWpTable th, .fltPlanWpTable td { border: 1px solid #a3adb6; padding: 3px 4px; font-size: 15px; line-height: 1.15; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; text-align: center; vertical-align: middle; }
    .fltPlanWpTable th { background: #e4e8ec; font-weight: 700; }
    .fltPlanWpRowSkipped { opacity: 0.45; background: #eef1f4; }
    .fltPlanWpRowDirectSource td { box-shadow: inset 0 0 0 1px rgba(31, 111, 67, 0.55); }
    .fltPlanWpRowDirectTarget td { box-shadow: inset 0 0 0 1px rgba(46, 110, 184, 0.55); }
    .fltPlanWpRowTargetCandidate { cursor: pointer; }
    .fltPlanWpRowTargetCandidate:hover td { background: #e8f2fd; }
    .fltPlanStepCellWrap { display: flex; align-items: center; justify-content: center; gap: 4px; }
    .fltPlanRowActionBtn { display: none; font-family: inherit; font-size: 10px; line-height: 1; padding: 1px 4px; border: 1px solid #6f7f90; background: #f5f7f9; color: #102030; cursor: pointer; }
    .fltPlanRowActionBtn.armed { background: #2d66c3; color: #ffffff; border-color: #1f4c92; }
    .fltPlanWpRowActionDir .fltPlanRowActionBtn.dir { display: inline-block; }
    .fltPlanWpRowActionDel .fltPlanRowActionBtn.del { display: inline-block; }
    .fltPlanWpTable th.fltPlanEtaHeader { cursor: pointer; }
    .fltPlanWpTable th.fltPlanEtaHeader:hover { background: #d7e4ef; }
    .fltPlanAltTag { font-size: 11px; margin-left: 4px; color: #455869; }
    .fltPlanMiniBtn { font-family: inherit; font-size: 10px; line-height: 1; padding: 1px 3px; min-width: 16px; border: 1px solid #7c8692; background: #f4f7fa; color: #111; cursor: pointer; }
    .fltPlanMiniBtn:hover { background: #dce8f2; }
    .fltPlanSpdValue { display: inline-block; min-width: 30px; text-align: center; margin: 0 2px; }
    .fltPlanAdjustCell { width: 100%; display: flex; align-items: center; justify-content: space-between; gap: 2px; }
    .fltPlanEtaWrap { display: inline-flex; align-items: center; justify-content: center; gap: 4px; }
    .fltPlanEtaWrap input { margin: 0; }
    .fltPlanEtaValue { cursor: pointer; }
    .fltPlanAtaValue { color: #2d66c3; font-weight: 700; cursor: pointer; }
    .fltPlanCoordValue { cursor: pointer; }
    @keyframes fltPlanRecFlash {
      0% { color: #b21f1f; }
      50% { color: #ff4a4a; }
      100% { color: #b21f1f; }
    }
    .fltPlanRecSpeed {
      color: #b21f1f;
      font-weight: 800;
      cursor: pointer;
      animation: fltPlanRecFlash 1.05s linear infinite;
    }
    .fltPlanCellNum { text-align: center; }
    .fltPlanBottomGrid { display: grid; grid-template-columns: 0.85fr 0.64fr 0.635fr; grid-template-areas: 'freq cmds cmds' 'assets wx wx'; gap: 8px; margin-top: 8px; }
    .fltPlanInfoFreqWrap { grid-area: freq; }
    .fltPlanInfoCmdsWrap { grid-area: cmds; }
    .fltPlanInfoAssetsWrap { grid-area: assets; }
    .fltPlanInfoWxWrap { grid-area: wx; }
    .fltPlanInfoBlock { border: 1px solid #8b96a1; background: #f7f9fb; min-height: 120px; display: flex; flex-direction: column; }
    .fltPlanInfoFreqWrap .fltPlanInfoBlock { min-height: 78px; }
    .fltPlanInfoTitle { border-bottom: 1px solid #8b96a1; background: #e4e8ec; font-size: 13px; font-weight: 700; padding: 3px 6px; }
    .fltPlanInfoBody { padding: 4px 6px; font-size: 13px; line-height: 1.3; overflow: auto; white-space: pre-wrap; }
    .fltPlanInfoTable { width: 100%; border-collapse: collapse; table-layout: fixed; }
    .fltPlanInfoTable th, .fltPlanInfoTable td { border: 1px solid #aeb7c0; font-size: 12px; padding: 2px 3px; text-align: left; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
    .fltPlanInfoTable th { background: #edf1f5; }
    .fltPlanInfoWxWrap .fltPlanInfoTable td { white-space: normal; overflow: visible; text-overflow: clip; word-break: break-word; }
    .fltPlanPageSwitcher { margin-left: 8px; display: inline-flex; gap: 4px; vertical-align: middle; }
    .fltPlanToolbar { display: flex; align-items: center; justify-content: space-between; gap: 8px; margin: 4px 0 2px 0; }
    .fltPlanToolbarLeft { display: inline-flex; align-items: center; flex-wrap: wrap; }
    .fltPlanToolbarRight { margin-left: auto; display: inline-flex; align-items: center; }
    .fltPlanPageBtn { font-size: 12px; border: 1px solid #8b96a1; background: #eceff3; padding: 2px 9px; cursor: pointer; min-height: 22px; }
    .fltPlanPageBtn.active { background: #d3dae2; font-weight: 700; }
    .fltPlanMapBgStatus { font-size: 13px; color: #3a4b5d; margin-left: 6px; }
    .fltPlanPage2Grid { display: grid; grid-template-columns: 1fr 1fr; gap: 8px; margin-top: 8px; }
    .fltPlanPage2Stack { display: flex; flex-direction: column; gap: 8px; min-height: 0; }
    .fltPlanPage2Section { border: 1px solid #8b96a1; background: #f7f9fb; }
    .fltPlanPage2Title { border-bottom: 1px solid #8b96a1; background: #e4e8ec; font-size: 13px; font-weight: 700; padding: 3px 6px; }
    .fltPlanPage2Body { padding: 4px 6px; }
    .fltPlanPage2Table { width: 100%; border-collapse: collapse; table-layout: fixed; }
    .fltPlanPage2Table th, .fltPlanPage2Table td { border: 1px solid #aeb7c0; font-size: 12px; padding: 2px 3px; text-align: left; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
    .fltPlanPage2Table th { background: #edf1f5; }
    .fltPlanPage2MarkerTable td { white-space: normal; overflow: visible; text-overflow: clip; word-break: break-word; vertical-align: top; }
    .fltPlanPage3Wrap { border: 1px solid #8b96a1; background: #f7f9fb; margin-top: 8px; padding: 4px; }
    .fltPlanPage3Legend { display: none; }
    .fltPlanPage3Canvas { border: 1px solid #8b96a1; background: #ffffff; width: 100%; height: 860px; box-sizing: border-box; }
    .fltPlanOpenMapWrap { position: relative; overflow: hidden; }
    .fltPlanOpenMapHost { position: absolute; inset: 0; width: 100%; height: 100%; }
    .fltPlanOpenMapVectorOverlay { position: absolute; inset: 0; width: 100%; height: 100%; z-index: 3; }
    .fltPlanOpenMapFallback { position: absolute; inset: 0; width: 100%; height: 100%; display: none; }
    .fltPlanOpenMapFallback > .fltPlanPage3Canvas { width: 100%; height: 100%; }
    .fltPlanPage3BraReadout { margin-top: 6px; border: 1px solid #8b96a1; background: #ffffff; color: #1f2e3d; min-height: 28px; padding: 4px 8px; font-size: 18px; line-height: 1.2; font-weight: 700; box-sizing: border-box; }
    .fltPlanStoresWrap { border: 1px solid #8b96a1; background: #f7f9fb; margin-top: 8px; padding: 6px; }
    .fltPlanStoresGrid { width: 100%; border-collapse: collapse; table-layout: fixed; background: #ffffff; }
    .fltPlanStoresGrid th, .fltPlanStoresGrid td { border: 1px solid #aeb7c0; font-size: 18px; padding: 3px 5px; text-align: left; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
    .fltPlanStoresGrid th { background: #edf1f5; }
    .fltPlanStoresStation { width: 72px; text-align: center; }
    .fltPlanStoresEmpty { color: #7a858f; }
    .fltPlanMessage { white-space: pre-wrap; font-size: 18px; line-height: 1.2; }
    .fltPlanPlain { margin: 0; background: #ffffff; border: 1px solid #b7b7b7; padding: 10px; white-space: pre; word-break: normal; font-size: 16px; line-height: 1.2; min-height: 100%; box-sizing: border-box; overflow: auto; }
    pre { background: #ffffff; border: 1px solid #b7b7b7; padding: 10px; white-space: pre-wrap; word-break: break-word; font-size: 18px; color:#111; max-height: 190px; overflow: auto; }
    body.raw-mode .keywordsPanel { flex: 0 0 280px; }
    .hidden { display: none; }

    body.night-mode { color: #dbe4ee; }
    body.night-mode .sheet { background: #1b2129; border-color: #4b5663; box-shadow: inset 0 0 0 1px rgba(255, 255, 255, 0.05); }
    body.night-mode h3 { color: #e9f0f8; }
    body.night-mode .meta { color: #a9b8c7; }
    body.night-mode .simControls { background: #26303a; border-color: #5c6d7f; color: #d7e3ee; }
    body.night-mode .simControls button { background: #2b3541; color: #e2eaf2; border-color: #607183; }
    body.night-mode .status { background: #27313b; border-color: #516070; color: #dce6f0; }
    body.night-mode .panel { background: #232c35; border-color: #556678; }
    body.night-mode .panel h4 { color: #e5edf6; border-bottom-color: #556678; }
    body.night-mode .panel .content { color: #dde6f0; }
    body.night-mode .tabRail { background: #2a3038; border-color: #5c6774; }
    body.night-mode .tabKeywordDivider { border-color: #5f6d7b; background: linear-gradient(to bottom, #4a5562, #3e4956); }
    body.night-mode .tabKeywordDivider::before { background: #b8c6d4; box-shadow: 0 -2px 0 #7f8d9a, 0 2px 0 #7f8d9a; }
    body.night-mode .kwGroup { background: #2a3340; border-color: #5f6f80; }
    body.night-mode .kwGroupTitle { color: #b8d1ea; }
    body.night-mode .controls { color: #d1dce8; }
    body.night-mode .meta.liveRefreshToggle.liveRefreshOn { color: #8ccaa5; }
    body.night-mode .controls button { background: #2b3541; color: #e2eaf2; border-color: #607183; }
    body.night-mode .fltPlanSelected { color: #9fb6cb; }
    body.night-mode .missionClockLabel { color: #9fb6cb; }
    body.night-mode .dtcFileItem { background: #1f2731; color: #e2eaf4; border-color: #5a6b7c; }
    body.night-mode .dtcFileItem:hover { background: #2a3440; }
    body.night-mode .dtcFileMeta { color: #9eb2c6; }
    body.night-mode .fltPlanBoard { background: #1c242d; color: #dfe8f2; border-color: #5c6d7f; }
    body.night-mode .fltPlanHeaderCell { background: #2a3440; border-color: #5c6d7f; }
    body.night-mode .fltPlanHeaderCellLabel { color: #9eb3c8; }
    body.night-mode .fltPlanHeaderCellValue { color: #e2eaf4; }
    body.night-mode .fltPlanTimeCell { background: #26303b; border-color: #5c6d7f; }
    body.night-mode .fltPlanTimeCellLabel { color: #9eb3c8; }
    body.night-mode .fltPlanTimeCellValue { color: #e2eaf4; }
    body.night-mode .fltPlanTimeCell.clickable:hover { background: #31404f; }
    body.night-mode .fltPlanPostFlightCell { background: transparent; border: 0; }
    body.night-mode .fltPlanCrewWrap { background: #202a34; border-color: #5c6d7f; }
    body.night-mode .fltPlanCrewTable th, body.night-mode .fltPlanCrewTable td { border-color: #5a6c7f; color: #dfe8f2; }
    body.night-mode .fltPlanCrewTable th { background: #2a3541; }
    body.night-mode .fltPlanWpWrap { background: #212a34; border-color: #5c6d7f; }
    body.night-mode .fltPlanWpTitle { border-bottom-color: #5c6d7f; color: #dfe8f2; }
    body.night-mode .fltPlanWpTable th,
    body.night-mode .fltPlanWpTable td { border-color: #5a6c7f; color: #dfe8f2; }
    body.night-mode .fltPlanWpTable th { background: #2a3541; }
    body.night-mode .fltPlanWpRowSkipped { background: #2a3440; }
    body.night-mode .fltPlanWpRowTargetCandidate:hover td { background: #334355; }
    body.night-mode .fltPlanRowActionBtn { background: #2b3541; color: #e2eaf4; border-color: #5f7184; }
    body.night-mode .fltPlanRowActionBtn.armed { background: #4d86e0; color: #f7fbff; border-color: #7aa5ea; }
    body.night-mode .fltPlanWpTable th.fltPlanEtaHeader:hover { background: #334253; }
    body.night-mode .fltPlanAltTag { color: #9fb5ca; }
    body.night-mode .fltPlanAtaValue { color: #74a8ff; }
    body.night-mode .fltPlanRecSpeed { color: #ff7c7c; }
    body.night-mode .fltPlanMiniBtn { background: #2b3541; color: #e2eaf4; border-color: #5f7184; }
    body.night-mode .fltPlanMiniBtn:hover { background: #364453; }
    body.night-mode .fltPlanInfoBlock,
    body.night-mode .fltPlanPage2Section,
    body.night-mode .fltPlanPage3Wrap { background: #202933; border-color: #5c6d7f; }
    body.night-mode .fltPlanInfoTitle,
    body.night-mode .fltPlanPage2Title { background: #2a3541; border-bottom-color: #5c6d7f; color: #dfe8f2; }
    body.night-mode .fltPlanInfoTable th,
    body.night-mode .fltPlanInfoTable td,
    body.night-mode .fltPlanPage2Table th,
    body.night-mode .fltPlanPage2Table td { border-color: #5a6c7f; color: #dfe8f2; }
    body.night-mode .fltPlanInfoTable th,
    body.night-mode .fltPlanPage2Table th { background: #2f3b48; }
    body.night-mode .fltPlanPageBtn { background: #2a3541; color: #e2eaf4; border-color: #5c6d7f; }
    body.night-mode .fltPlanPageBtn.active { background: #3a4a5b; color: #f5fbff; }
    body.night-mode .fltPlanMapBgStatus { color: #a8bfd6; }
    body.night-mode .fltPlanPage3Legend { color: #b7c9db; }
    body.night-mode .fltPlanPage3Canvas { background: #1a232c; border-color: #5c6d7f; }
    body.night-mode .fltPlanOpenMapFallback > .fltPlanPage3Canvas { background: #1a232c; border-color: #5c6d7f; }
    body.night-mode .fltPlanPage3BraReadout { background: #202933; border-color: #5c6d7f; color: #dfe8f2; }
    body.night-mode .fltPlanStoresWrap { background: #202933; border-color: #5c6d7f; }
    body.night-mode .fltPlanStoresGrid { background: #212a34; }
    body.night-mode .fltPlanStoresGrid th,
    body.night-mode .fltPlanStoresGrid td { border-color: #5a6c7f; color: #dfe8f2; }
    body.night-mode .fltPlanStoresGrid th { background: #2f3b48; }
    body.night-mode .fltPlanStoresEmpty { color: #9fb3c8; }
    body.night-mode .fltPlanPlain { background: #202a34; color: #dde7f2; border-color: #5d6f81; }
    body.night-mode pre { background: #202a34; color: #dde7f2; border-color: #5d6f81; }
  </style>
</head>
<body>
  <div class='sheet'>
  <div class='sheetBody'>
    <div class='headerRow'>
      <div>
        <h3>VAICOM KNEEBOARD</h3>
        <div id='liveExportMeta' class='meta liveRefreshToggle' title='Hidden dev tool: click to toggle dashboard auto-refresh (30s)'>Live export from VAICOM</div>
      </div>
      <img class='logo' src='logo.png' alt='VAICOM Logo'>
    </div>

    <div id='status' class='status'><span id='statusIndicator' class='statusIndicator'></span><span id='statusText'>Loading...</span></div>

    <div id='fakeMissionControls' class='simControls hidden'>
      <strong>SIM TEST</strong>
      <span id='fakeMissionInfo'>OFF</span>
      <span id='fakeMissionClock'>--:--:--</span>
      <button id='fakeMissionSlower' type='button'>- Rate</button>
      <span id='fakeMissionRate' class='simRate'>x1.0</span>
      <button id='fakeMissionFaster' type='button'>+ Rate</button>
      <button id='fakeMissionStop' type='button'>Stop</button>
    </div>

    <div class='kneeLayout'>
      <div class='leftColumn'>
        <div id='dtcControls' class='controls fltPlanControls hidden'>
          <button id='dtcRefresh' type='button'>Refresh FLT PLN</button>
          <span id='dtcSelectedFileLabel' class='fltPlanSelected'>No FLT PLN selected</span>
          <span id='missionClockLabel' class='missionClockLabel'>Mission Time - (- UTC)</span>
        </div>

        <div id='dtcSelector' class='panel hidden'>
          <h4 id='dtcSelectorHeader' class='clickable'>FLT PLN Files ▼</h4>
          <div id='dtcFileList' class='content dtcFileList'></div>
        </div>

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
      <label><input id='nightMode' type='checkbox'> Night Mode</label>
      <label><input id='dlinkOn' type='checkbox' checked> DLink ON</label>
      <label>Font Size <input id='fontSizeSlider' type='range' min='18' max='34' step='1' value='24'></label>
      <span id='fontSizeValue'>24</span>
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
    document.addEventListener('selectstart', function(ev){
      if (ev && ev.preventDefault) ev.preventDefault();
    }, true);

    document.addEventListener('dragstart', function(ev){
      if (ev && ev.preventDefault) ev.preventDefault();
    }, true);

    const TABS = ['LOG','DTC','ATC','AWACS','JTAC','TANKER','AOCS','FLIGHT','AI CREW','GND CREW','NOTES'];
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
    let dtcListCollapsed = false;
    let fltPlanEtaStartBySelection = {};
    let fltPlanPlanStateBySelection = {};
    let fltPlanDtcPageBySelection = {};
    let fltPlanDtcRouteBySelection = {};
    let fltPlanMapViewBySelection = {};
    let fltPlanMapBackgroundEnabledBySelection = {};
    let fltPlanOpenFreeMapViewBySelection = {};
    let fltPlanMapSelectedAssetKeyBySelection = {};
    let openFreeMapPayloadById = {};
    let openFreeMapPayloadSeq = 1;
    let openFreeMapInstancesByContainerId = {};
    let openFreeMapContainerIdBySelectionKey = {};
    let openFreeMapRuntimeReady = false;
    let openFreeMapRuntimeLoading = false;
    let openFreeMapRuntimeFailed = false;
    let openFreeMapRuntimeErrorText = '';
    let openFreeMapRuntimeLastAttemptMs = 0;
    let openFreeMapLastPayloadStatus = '';
    let openFreeMapRuntimeWaiters = [];
    let mapPanDrag = null;
    let navlogRowDrag = null;
    let runtimeFlightPlanSnapshot = null;
    let runtimeFlightPlanSnapshotMissionIdentity = '';
    let runtimeFlightPlanSnapshotGroupName = '';
    let lastKnownPlayerCallsign = '';
    let lastKnownTheater = '';
    let lastMissionIdentity = '';
    let lastMissionClockSeconds = NaN;
    let missionClockAnchorSeconds = NaN;
    let missionClockAnchorSystemMs = 0;
    let missionClockAnchorIdentity = '';
    let clockTickTimer = null;
    let fakeMissionEnabled = false;
    let fakeMissionState = null;
    let fakeMissionSpeed = 1.0;
    let fakeMissionTimer = null;
    let fakeMissionLastRealMs = 0;
    let fakeMissionLastRenderMs = 0;
    let liveRefreshEnabled = false;
    let liveRefreshTimer = null;
    const sessionCollapsedStorageKey = 'vaicom.okb.sessionCollapsed';
    const dtcListCollapsedStorageKey = 'vaicom.okb.dtcListCollapsed';
    const tabKeywordsSplitStorageKey = 'vaicom.okb.tabKeywordsSplitByTab';
    const drawModeStorageKey = 'vaicom.okb.notesDrawMode';
    const liveRefreshStorageKey = 'vaicom.okb.liveRefreshEnabled';
    const nightModeStorageKey = 'vaicom.okb.nightMode';
    const dlinkOnStorageKey = 'vaicom.okb.dlinkOn';
    const contentFontSizeStorageKey = 'vaicom.okb.contentFontSize';
    const drawModeTimeoutMs = 30000;
    const speedRecommendationTimeoutMs = 30000;
    const fakeMissionSpeedMin = 0.25;
    const fakeMissionSpeedMax = 8.0;
    let tabKeywordsSplitByTab = {};
    let nightModeEnabled = false;
    let dlinkOnEnabled = true;
    let contentFontSizePx = 24;

    function clamp(v, min, max){
      return Math.max(min, Math.min(max, v));
    }

    function updateKeywordBodyHtml(html){
      const keywordBody = document.getElementById('keywordBody');
      if (!keywordBody) return;

      const previousScrollTop = keywordBody.scrollTop;
      keywordBody.innerHTML = html;

      if (previousScrollTop > 0){
        const maxScrollTop = Math.max(0, keywordBody.scrollHeight - keywordBody.clientHeight);
        keywordBody.scrollTop = clamp(previousScrollTop, 0, maxScrollTop);
      }
    }

    function applyDtcListCollapsedState(collapsed){
      dtcListCollapsed = !!collapsed;
      const listEl = document.getElementById('dtcFileList');
      const headerEl = document.getElementById('dtcSelectorHeader');
      if (listEl) listEl.style.display = dtcListCollapsed ? 'none' : 'flex';
      if (headerEl) headerEl.textContent = dtcListCollapsed ? 'FLT PLN Files ► (click to expand)' : 'FLT PLN Files ▼';
    }

    function readInitialDtcListCollapsed(){
      try{
        return window.localStorage && window.localStorage.getItem(dtcListCollapsedStorageKey) === '1';
      }catch(_){
        return false;
      }
    }

    function persistDtcListCollapsedState(){
      try{
        if (window.localStorage){
          window.localStorage.setItem(dtcListCollapsedStorageKey, dtcListCollapsed ? '1' : '0');
        }
      }catch(_){
      }
    }

    function safe(v){ return (v === null || v === undefined || v === '') ? '-' : String(v); }

    function formatUtcToSeconds(v){
      if (v === null || v === undefined || v === '') return '-';
      const d = new Date(v);
      if (!isFinite(d.getTime())) return String(v);
      return d.toISOString().replace(/\.\d{3}Z$/, 'Z');
    }

    function normalizeCategory(cat){
      var c = String(cat || '').toUpperCase().trim();
      var compact = c.replace(/[^A-Z0-9]/g, '');
      if (c === 'RIO' || c === 'ICEMAN' || c === 'WSO' || c === 'GEORGE' || c === 'CPG' || c === 'AICPG' || c === 'AIWSO') return 'AI CREW';
      if (c === 'REF' || c === 'CREW' || c === 'REF/CREW') return 'GND CREW';
      if (compact === 'REFCREW' || compact === 'GROUNDCREW' || compact === 'GNDCREW' || compact === 'GROUND') return 'GND CREW';
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

    function getCustomActionIdFromEvent(ev, detail){
      const d = detail || {};
      const candidates = [
        d.id,
        d.actionId,
        d.customActionId,
        d.action && d.action.id,
        ev && ev.id,
        ev && ev.actionId
      ];

      for (let i = 0; i < candidates.length; i++){
        const value = candidates[i];
        if (typeof value === 'string' && value.length){
          return value;
        }
      }

      return '';
    }

    function getCustomActionExtraData(detail){
      const d = detail || {};
      if (d.extraData !== undefined) return d.extraData;
      if (d.extra !== undefined) return d.extra;
      if (d.data !== undefined) return d.data;
      return undefined;
    }

    function handleCustomActionEvent(ev){
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

    function registerCustomActionHandlers(){
      if (customActionHandlerRegistered) return;

      const okb = (typeof OpenKneeboard !== 'undefined') ? OpenKneeboard : window.OpenKneeboard;
      const targets = [okb, window];
      for (let i = 0; i < targets.length; i++){
        const t = targets[i];
        if (!t || !t.addEventListener) continue;
        try{
          t.addEventListener('plugin/tab/customAction', handleCustomActionEvent);
          t.addEventListener('plugin/tab/custom-action', handleCustomActionEvent);
          customActionHandlerRegistered = true;
        }catch(_){
        }
      }
    }

    function readNightModePreference(){
      try{
        return window.localStorage && window.localStorage.getItem(nightModeStorageKey) === '1';
      }catch(_){
        return false;
      }
    }

    function persistNightModePreference(){
      try{
        if (window.localStorage){
          window.localStorage.setItem(nightModeStorageKey, nightModeEnabled ? '1' : '0');
        }
      }catch(_){
      }
    }

    function applyNightModeUi(){
      document.body.classList.toggle('night-mode', !!nightModeEnabled);
      const box = document.getElementById('nightMode');
      if (box) box.checked = !!nightModeEnabled;
    }

    function readContentFontSizePreference(){
      try{
        if (!window.localStorage) return 24;
        const raw = window.localStorage.getItem(contentFontSizeStorageKey);
        const parsed = parseFloat(raw);
        if (!isFinite(parsed)) return 24;
        return clamp(parsed, 18, 34);
      }catch(_){
        return 24;
      }
    }

    function readDlinkOnPreference(){
      try{
        if (!window.localStorage) return true;
        const raw = window.localStorage.getItem(dlinkOnStorageKey);
        if (raw === null || raw === undefined || raw === '') return true;
        return raw === '1';
      }catch(_){
        return true;
      }
    }

    function persistDlinkOnPreference(){
      try{
        if (window.localStorage){
          window.localStorage.setItem(dlinkOnStorageKey, dlinkOnEnabled ? '1' : '0');
        }
      }catch(_){
      }
    }

    function applyDlinkOnUi(){
      const box = document.getElementById('dlinkOn');
      if (box) box.checked = !!dlinkOnEnabled;
    }

    function hasAwacsAvailable(data){
      const units = getMergedList(data && data.Units, 'AWACS');
      const hasUnitLine = Array.isArray(units) && units.some(function(line){
        return String(line || '').trim().length > 0;
      });
      if (hasUnitLine) return true;

      const server = (data && data.Server) || {};
      const assets = Array.isArray(server.FriendlyAssets) ? server.FriendlyAssets : [];
      return assets.some(function(a){
        return String((a && a.Category) || '').toUpperCase() === 'AWACS';
      });
    }

    function persistContentFontSizePreference(){
      try{
        if (window.localStorage){
          window.localStorage.setItem(contentFontSizeStorageKey, String(contentFontSizePx));
        }
      }catch(_){
      }
    }

    function applyContentFontSizeUi(){
      const safeSize = clamp(contentFontSizePx, 18, 34);
      contentFontSizePx = safeSize;
      document.body.style.setProperty('--contentFontSize', String(safeSize) + 'px');
      const slider = document.getElementById('fontSizeSlider');
      if (slider) slider.value = String(safeSize);
      const value = document.getElementById('fontSizeValue');
      if (value) value.textContent = String(safeSize);
    }

    function updateFakeMissionControlsUi(){
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

    function stopFakeMissionTimer(){
      if (!fakeMissionTimer) return;
      clearInterval(fakeMissionTimer);
      fakeMissionTimer = null;
    }

    function randomInRange(min, max){
      return min + (Math.random() * (max - min));
    }

    function makeFakeAsset(callsign, category, x, y, headingDeg, speedMps){
      return {
        Callsign: callsign,
        Name: callsign,
        Category: category,
        RawLine: '',
        X: x,
        Y: y,
        headingDeg: headingDeg,
        speedMps: speedMps,
      };
    }

    function createFakeMissionStateFromData(data){
      const source = data || latestData || {};
      const server = (source && source.Server) || {};
      const selected = getActiveFlightPlanSelection(source);
      const routeRows = getPlanWaypointsForRecommendations(selected)
        .filter(function(wp){ return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum)); });

      const simStartOffsetMeters = 5 * 1852;

      const routeStartX = routeRows.length ? Number(routeRows[0].xNum) : NaN;
      const routeStartY = routeRows.length ? Number(routeRows[0].yNum) : NaN;
      let baseX = isFinite(routeStartX)
        ? routeStartX
        : (isFinite(Number(server.PlayerPosX)) ? Number(server.PlayerPosX) : 170000);
      let baseY = isFinite(routeStartY)
        ? routeStartY
        : (isFinite(Number(server.PlayerPosY)) ? Number(server.PlayerPosY) : 105000);

      if (routeRows.length >= 2){
        const wp1 = routeRows[0];
        const wp2 = routeRows[1];
        const x1 = Number(wp1.xNum);
        const y1 = Number(wp1.yNum);
        const x2 = Number(wp2.xNum);
        const y2 = Number(wp2.yNum);
        const dx = x2 - x1;
        const dy = y2 - y1;
        const mag = Math.sqrt((dx * dx) + (dy * dy));
        if (isFinite(mag) && mag > 0.01){
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
          makeFakeAsset('PLAYER', 'PLAYER', baseX, baseY, 0, 0),
          makeFakeAsset('TEXACO11', 'TANKER', baseX + 30000, baseY + 22000, 235, 210),
          makeFakeAsset('OVERLORD1', 'AWACS', baseX - 45000, baseY + 26000, 095, 205),
          makeFakeAsset('AXEMAN11', 'JTAC', baseX + 14000, baseY - 22000, 330, 0),
          makeFakeAsset('VIPER12', 'FLIGHT', baseX - 18000, baseY - 12000, 025, 230),
          makeFakeAsset('COLT21', 'FLIGHT', baseX + 8000, baseY + 16000, 290, 220),
        ],
      };
    }

    function updateFakeOwnshipStartedState(){
      if (!fakeMissionEnabled || !fakeMissionState) return;
      const selected = getActiveFlightPlanSelection(latestData);
      if (!selected){
        fakeMissionState.ownshipStarted = false;
        return;
      }

      const timing = getResolvedTimingMarks(selected);
      const takeoff = Number(timing && timing.takeoff);
      if (!isFinite(takeoff)){
        fakeMissionState.ownshipStarted = false;
        return;
      }

      fakeMissionState.ownshipStarted = Number(fakeMissionState.simMissionSeconds) >= takeoff;
    }

    function syncFakeMissionRouteRowsFromPlan(){
      if (!fakeMissionEnabled || !fakeMissionState || !latestData) return;
      const selected = getActiveFlightPlanSelection(latestData);
      if (!selected) return;

      const plannedRows = getPlanWaypointsForRecommendations(selected);
      if (!Array.isArray(plannedRows) || !plannedRows.length) return;

      const byStep = {};
      plannedRows.forEach(function(wp){
        const key = stepToKey(wp && wp.step);
        if (!key) return;
        byStep[key] = wp;
      });

      const rows = Array.isArray(fakeMissionState.routeRows) ? fakeMissionState.routeRows : [];
      for (let i = 1; i < rows.length; i++){
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

    function advanceFakeOwnshipAlongRoute(state, dt){
      if (!state) return;
      const rows = Array.isArray(state.routeRows) ? state.routeRows : [];
      if (rows.length < 2) return;

      function legSpeedMpsFor(toWp){
        const cas = Number(toWp && toWp.spd);
        const altFeet = isFinite(Number(toWp && toWp.altFeet)) ? Number(toWp.altFeet) : 0;
        const gsKnots = isFinite(cas) && cas > 0 ? (cas * (1.0 + (Math.max(0, altFeet) / 100000.0))) : NaN;
        const mps = isFinite(gsKnots) && gsKnots > 0 ? (gsKnots * 0.514444) : NaN;
        if (isFinite(mps) && mps > 0) return mps;
        return Math.max(0, Number(state.playerSpeedMps) || 185);
      }

      let remainingSeconds = Math.max(0, Number(dt) || 0);
      if (remainingSeconds <= 0) return;

      while (remainingSeconds > 0){
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
        if (!isFinite(legMeters) || legMeters < 1){
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

        if (remainingSeconds < leftSeconds){
          progress += (remainingSeconds * speedMps);
          remainingSeconds = 0;
          state.playerRouteProgressMeters = progress;
        } else {
          remainingSeconds -= leftSeconds;
          if (i + 1 >= rows.length - 1){
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

        const playerAsset = (state.assets || []).find(function(a){ return String((a && a.Category) || '').toUpperCase() === 'PLAYER'; });
        if (playerAsset){
          playerAsset.X = state.playerX;
          playerAsset.Y = state.playerY;
          const hdg = normalizeHeadingDeg((Math.atan2(dy, dx) * 180.0 / Math.PI) + 90);
          playerAsset.headingDeg = isFinite(hdg) ? hdg : 0;
        }

        break;
      }
    }

    function stepFakeMissionState(deltaRealSeconds){
      if (!fakeMissionEnabled || !fakeMissionState) return;
      const dt = Math.max(0, Number(deltaRealSeconds) || 0) * fakeMissionSpeed;
      if (dt <= 0) return;

      fakeMissionState.simMissionSeconds += dt;
      syncFakeMissionRouteRowsFromPlan();
      updateFakeOwnshipStartedState();
      if (fakeMissionState.ownshipStarted){
        advanceFakeOwnshipAlongRoute(fakeMissionState, dt);
      }
      const bounds = 70000;
      const baseX = fakeMissionState.playerX;
      const baseY = fakeMissionState.playerY;

      fakeMissionState.assets.forEach(function(asset, idx){
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
        if (Math.abs(offX) > bounds || Math.abs(offY) > bounds){
          asset.headingDeg = ((heading + 180 + randomInRange(-20, 20)) % 360 + 360) % 360;
        }

        if (idx % 2 === 0 && Math.random() < 0.03){
          asset.speedMps = clamp(speed + randomInRange(-8, 8), 120, 280);
        }
      });
    }

    function getDisplayData(data){
      const original = data || latestData || {};
      if (!fakeMissionEnabled || !fakeMissionState) return original;

      let clone;
      try{
        clone = JSON.parse(JSON.stringify(original || {}));
      }catch(_){
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
      if (!Array.isArray(server.Diagnostics.playerGroupWaypoints)){
        server.Diagnostics.playerGroupWaypoints = [];
      }
      server.Diagnostics.playerGroup = String(server.Diagnostics.playerGroup || 'SIM-FLIGHT');

      if (!Array.isArray(server.Diagnostics.playerGroupWaypoints) || !server.Diagnostics.playerGroupWaypoints.length){
        const sel = getActiveFlightPlanSelection(clone);
        const routeRows = getPlanWaypointsForRecommendations(sel).filter(function(wp){
          return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum));
        });
        server.Diagnostics.playerGroupWaypoints = routeRows.map(function(wp, i){
          return 'RT|group=SIM-FLIGHT|pt=' + String(i + 1)
            + '|x=' + String(Math.round(Number(wp.xNum)))
            + '|y=' + String(Math.round(Number(wp.yNum)))
            + '|alt=' + String(Math.round(((Number(wp.altFeet) || 10000) / 3.28084)))
            + '|spd=180|eta=' + String(parseEtaToSeconds(wp.etaDisplay || wp.eta) || 0)
            + '|task=' + encodeURIComponent(String(wp.typeRaw || wp.type || 'WP'));
        });
      }

      server.FriendlyAssets = fakeMissionState.assets.map(function(a){
        return {
          Callsign: String(a.Callsign || ''),
          Name: String(a.Name || ''),
          Category: String(a.Category || ''),
          RawLine: String(a.RawLine || ''),
          X: Number(a.X || 0),
          Y: Number(a.Y || 0),
        };
      });
      return clone;
    }

    function startFakeMissionMode(){
      fakeMissionEnabled = true;
      fakeMissionSpeed = 1.0;
      fakeMissionState = createFakeMissionStateFromData(latestData);
      const selected = getActiveFlightPlanSelection(latestData);
      if (selected && fakeMissionState){
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
      fakeMissionTimer = setInterval(function(){
        const now = Date.now();
        const delta = (now - fakeMissionLastRealMs) / 1000.0;
        fakeMissionLastRealMs = now;
        stepFakeMissionState(delta);
        if (latestData && selectedTab === 'DTC'){
          const shouldRender = (now - fakeMissionLastRenderMs) >= 350;
          if (shouldRender){
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

    function stopFakeMissionMode(){
      fakeMissionEnabled = false;
      fakeMissionState = null;
      fakeMissionSpeed = 1.0;
      stopFakeMissionTimer();
      updateFakeMissionControlsUi();
      if (latestData) render(latestData);
    }

    function adjustFakeMissionSpeed(factor){
      if (!fakeMissionEnabled) return;
      const f = Number(factor);
      if (!isFinite(f) || f <= 0) return;
      fakeMissionSpeed = clamp(fakeMissionSpeed * f, fakeMissionSpeedMin, fakeMissionSpeedMax);
      updateFakeMissionControlsUi();
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
      if (tab === 'ATC') return 'WX/ATC';
      if (tab === 'DTC') return 'FLT PLN';
      return tab;
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

    function readLiveRefreshPreference(){
      try{
        return window.localStorage && window.localStorage.getItem(liveRefreshStorageKey) === '1';
      }catch(_){
        return false;
      }
    }

    function persistLiveRefreshPreference(){
      try{
        if (window.localStorage){
          window.localStorage.setItem(liveRefreshStorageKey, liveRefreshEnabled ? '1' : '0');
        }
      }catch(_){
      }
    }

    function updateLiveRefreshUi(){
      const meta = document.getElementById('liveExportMeta');
      if (!meta) return;
      const base = 'Live export from VAICOM';
      meta.classList.toggle('liveRefreshOn', !!liveRefreshEnabled);
      meta.textContent = liveRefreshEnabled ? (base + ' • Auto refresh ON') : base;
      meta.title = liveRefreshEnabled
        ? 'Hidden dev tool active: auto-refreshing dashboard data every 30 seconds (click to disable)'
        : 'Hidden dev tool: click to toggle dashboard auto-refresh (30s)';
    }

    async function requestLiveDashboardRefresh(){
      try{
        await fetch('dev/refresh', { method: 'POST', cache: 'no-store' });
      }catch(_){
      }
    }

    function stopLiveRefreshTimer(){
      if (!liveRefreshTimer) return;
      clearInterval(liveRefreshTimer);
      liveRefreshTimer = null;
    }

    function applyLiveRefreshState(enabled){
      liveRefreshEnabled = !!enabled;
      stopLiveRefreshTimer();

      if (liveRefreshEnabled){
        requestLiveDashboardRefresh();
        liveRefreshTimer = setInterval(function(){
          requestLiveDashboardRefresh();
        }, 30000);
      }

      persistLiveRefreshPreference();
      updateLiveRefreshUi();
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
      if (selectedTab === 'DTC'){
        const tabPanel = document.querySelector('.tabPanel');
        const keywordPanel = document.getElementById('keywordPanel');
        if (tabPanel) tabPanel.style.flex = '1 1 auto';
        if (keywordPanel) keywordPanel.style.flex = '';
        return;
      }

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

    function getDeletedStepSet(state){
      const s = state || {};
      const deleted = (s.deletedSteps && typeof s.deletedSteps === 'object') ? s.deletedSteps : {};
      const set = {};
      Object.keys(deleted).forEach(function(k){
        const key = stepToKey(k);
        if (!key) return;
        if (!!deleted[k]) set[key] = true;
      });
      return set;
    }

    function getEffectiveRouteRows(rows, state){
      const list = Array.isArray(rows) ? rows : [];
      const deletedSet = getDeletedStepSet(state);
      return list.filter(function(wp){
        if (!wp || wp.isStart) return false;
        const key = stepToKey(wp.step);
        if (!key) return false;
        return !deletedSet[key];
      });
    }

    function clearInvalidDirectToState(state){
      if (!state || typeof state !== 'object') return;
      state.directToSourceStep = '';
      state.directToTargetStep = '';
    }

    function getDirectToSpanInfo(rows, state){
      const activeRows = getEffectiveRouteRows(rows, state);
      const sourceStep = stepToKey(state && state.directToSourceStep);
      const targetStep = stepToKey(state && state.directToTargetStep);
      if (!sourceStep || !targetStep || !activeRows.length) return null;

      const sourceIdx = activeRows.findIndex(function(wp){ return stepToKey(wp.step) === sourceStep; });
      const targetIdx = activeRows.findIndex(function(wp){ return stepToKey(wp.step) === targetStep; });
      if (sourceIdx < 0 || targetIdx < 0 || targetIdx <= sourceIdx) return null;

      return {
        activeRows: activeRows,
        sourceIdx: sourceIdx,
        targetIdx: targetIdx,
      };
    }

    function ensureDirectToStateValid(rows, state){
      if (!state || typeof state !== 'object') return;
      const sourceStep = stepToKey(state.directToSourceStep);
      const targetStep = stepToKey(state.directToTargetStep);
      if (!sourceStep || !targetStep){
        if (!sourceStep || !targetStep){
          if (!sourceStep) state.directToSourceStep = '';
          if (!targetStep) state.directToTargetStep = '';
        }
        return;
      }

      const span = getDirectToSpanInfo(rows, state);
      if (!span){
        clearInvalidDirectToState(state);
      }
    }

    function getSkippedStepSet(rows, state){
      const skipped = {};
      const span = getDirectToSpanInfo(rows, state);
      if (!span) return skipped;

      for (let i = span.sourceIdx + 1; i < span.targetIdx; i++){
        const wp = span.activeRows[i];
        const key = stepToKey(wp && wp.step);
        if (!key) continue;
        skipped[key] = true;
      }

      return skipped;
    }

    function textHasAny(text, terms){
      const source = String(text || '').toLowerCase();
      for (let i = 0; i < terms.length; i++){
        if (source.indexOf(String(terms[i] || '').toLowerCase()) >= 0) return true;
      }
      return false;
    }

    function isCarrierContext(data){
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
      atcUnits.forEach(function(v){ scan.push(String(v || '')); });
      atcDetails.forEach(function(v){ scan.push(String(v || '')); });
      scan.push(atcLog);

      return textHasAny(scan.join('\n'), carrierTokens);
    }

    function isCarrierCapableAircraft(data){
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

    function classifyAtcKeyword(phrase){
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

    function reorderAtcPhrasesForContext(data, phrases){
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

      list.sort(function(a, b){
        const ra = rankMap[classifyAtcKeyword(a)] || 99;
        const rb = rankMap[classifyAtcKeyword(b)] || 99;
        if (ra !== rb) return ra - rb;
        return String(a).localeCompare(String(b));
      });

      return list;
    }

    function classifyGroundCrewKeyword(phrase){
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

    function reorderGroundCrewPhrasesForFlow(phrases){
      const list = Array.isArray(phrases) ? phrases.slice() : [];
      const rankMap = {
        servicing_arming: 0,
        startup: 1,
        ground_checks: 2,
        dispatching: 3,
        general: 4,
      };

      list.sort(function(a, b){
        const ra = rankMap[classifyGroundCrewKeyword(a)] || 99;
        const rb = rankMap[classifyGroundCrewKeyword(b)] || 99;
        if (ra !== rb) return ra - rb;
        return String(a).localeCompare(String(b));
      });

      return list;
    }

    function classifyJtacKeyword(phrase){
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

    function reorderJtacPhrasesForFlow(phrases){
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

      list.sort(function(a, b){
        const ra = rankMap[classifyJtacKeyword(a)] || 99;
        const rb = rankMap[classifyJtacKeyword(b)] || 99;
        if (ra !== rb) return ra - rb;
        return String(a).localeCompare(String(b));
      });

      return list;
    }

    function classifyFlightKeyword(phrase){
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

    function reorderFlightPhrasesForContext(phrases){
      const list = Array.isArray(phrases) ? phrases.slice() : [];
      const rankMap = { enroute: 0, tactical_formation: 1, tactical_a2a: 2, tactical_a2g: 3, general: 4 };

      list.sort(function(a, b){
        const ra = rankMap[classifyFlightKeyword(a)] || 99;
        const rb = rankMap[classifyFlightKeyword(b)] || 99;
        if (ra !== rb) return ra - rb;
        return String(a).localeCompare(String(b));
      });

      return list;
    }

    function getKeywordGroupsForTab(data, tab, phrases){
      const rows = Array.isArray(phrases) ? phrases.slice() : [];
      if (!rows.length) return [];

      if (tab === 'ATC'){
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
        rows.forEach(function(r){
          const key = classifyAtcKeyword(r);
          (buckets[key] || buckets.general).push(r);
        });

        const groups = [];
        orderedKeys.forEach(function(k){
          const vals = buckets[k] || [];
          if (!vals.length) return;
          groups.push({ title: labels[k], items: vals });
        });
        return groups;
      }

      if (tab === 'GND CREW'){
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
        rows.forEach(function(r){
          const key = classifyGroundCrewKeyword(r);
          (buckets[key] || buckets.general).push(r);
        });

        const groups = [];
        orderedKeys.forEach(function(k){
          const vals = buckets[k] || [];
          if (!vals.length) return;
          groups.push({ title: labels[k], items: vals });
        });
        return groups;
      }

      if (tab === 'JTAC'){
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
        rows.forEach(function(r){
          const key = classifyJtacKeyword(r);
          (buckets[key] || buckets.general).push(r);
        });

        const groups = [];
        orderedKeys.forEach(function(k){
          const vals = buckets[k] || [];
          if (!vals.length) return;
          groups.push({ title: labels[k], items: vals });
        });
        return groups;
      }

      if (tab === 'FLIGHT'){
        const orderedKeys = ['enroute', 'tactical_formation', 'tactical_a2a', 'tactical_a2g', 'general'];
        const labels = {
          enroute: 'Enroute',
          tactical_formation: 'Tactical Formation',
          tactical_a2a: 'Tactical Air to Air',
          tactical_a2g: 'Tactical Air to Ground',
          general: 'General',
        };
        const buckets = { enroute: [], tactical_formation: [], tactical_a2a: [], tactical_a2g: [], general: [] };
        rows.forEach(function(r){
          const key = classifyFlightKeyword(r);
          (buckets[key] || buckets.general).push(r);
        });

        const groups = [];
        orderedKeys.forEach(function(k){
          const vals = buckets[k] || [];
          if (!vals.length) return;
          groups.push({ title: labels[k], items: vals });
        });
        return groups;
      }

      if (tab === 'AI CREW'){
        const suffix = formatAiCrewPhaseLabel(data && data.AiCrewPhase);
        return [{ title: 'Primary' + suffix, items: rows }];
      }

      return [{ title: 'Reference', items: rows }];
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
        return reorderGroundCrewPhrasesForFlow(filtered);
      }

      if (tab === 'ATC'){
        return reorderAtcPhrasesForContext(data, phrases);
      }

      if (tab === 'FLIGHT'){
        return reorderFlightPhrasesForContext(phrases);
      }

      if (tab === 'JTAC'){
        return reorderJtacPhrasesForFlow(phrases);
      }

      phrases.sort(function(a,b){ return a.localeCompare(b); });
      return phrases;
    }

    function formatKeywordReference(data, tab){
      if (tab === 'LOG') return 'No keyword reference for this tab.';
      if (tab === 'DTC') return 'No keyword reference for this tab.';
      const phrases = getKeywordPhrasesForTab(data, tab);
      if (!phrases.length) return 'No keywords for this tab yet.';
      return phrases.join('\n');
    }

    function parseRteSelectionToken(value){
      const text = String(value || '');
      const prefix = 'RTE::';
      if (text.indexOf(prefix) !== 0) return null;
      const sep = text.indexOf('::', prefix.length);
      if (sep < 0) return null;
      const encodedRoute = text.substring(prefix.length, sep);
      const filePath = text.substring(sep + 2);
      let routeName = encodedRoute;
      try{ routeName = decodeURIComponent(encodedRoute); }catch(_){ }
      return { routeName: routeName, filePath: filePath };
    }

    function getPathFileName(path){
      const norm = String(path || '').replace(/\\/g, '/');
      const idx = norm.lastIndexOf('/');
      return idx >= 0 ? norm.substring(idx + 1) : norm;
    }

    function getFltPlnPath(path){
      const rte = parseRteSelectionToken(path);
      return rte ? rte.filePath : String(path || '');
    }

    function getDtcDisplayName(path){
      const text = String(path || '');
      if (!text) return '-';
      const rte = parseRteSelectionToken(text);
      if (rte){
        return String(rte.routeName || '-');
      }
      return getPathFileName(text);
    }

    function appendDtcRows(prefix, value, rows, depth){
      if (rows.length >= 220) return;
      if (depth > 8){
        rows.push({ key: prefix, value: '[depth limit]' });
        return;
      }

      if (value === null || value === undefined){
        rows.push({ key: prefix, value: '-' });
        return;
      }

      if (Array.isArray(value)){
        if (!value.length){
          rows.push({ key: prefix, value: '[]' });
          return;
        }

        for (let i = 0; i < value.length; i++){
          appendDtcRows(prefix + '[' + i + ']', value[i], rows, depth + 1);
          if (rows.length >= 220) return;
        }
        return;
      }

      if (typeof value === 'object'){
        const keys = Object.keys(value);
        if (!keys.length){
          rows.push({ key: prefix, value: '{}' });
          return;
        }

        keys.forEach(function(k){
          if (rows.length >= 220) return;
          const nextPrefix = prefix ? (prefix + '.' + k) : k;
          appendDtcRows(nextPrefix, value[k], rows, depth + 1);
        });
        return;
      }

      rows.push({ key: prefix, value: String(value) });
    }

    function findFirstObjectByKeyPattern(value, pattern, depth){
      if (depth > 8 || value === null || value === undefined) return null;
      if (Array.isArray(value)){
        for (let i = 0; i < value.length; i++){
          const found = findFirstObjectByKeyPattern(value[i], pattern, depth + 1);
          if (found) return found;
        }
        return null;
      }
      if (typeof value !== 'object') return null;

      const keys = Object.keys(value);
      for (let i = 0; i < keys.length; i++){
        const k = keys[i];
        const v = value[k];
        if (pattern.test(String(k)) && v && typeof v === 'object') return v;
      }

      for (let i = 0; i < keys.length; i++){
        const found = findFirstObjectByKeyPattern(value[keys[i]], pattern, depth + 1);
        if (found) return found;
      }
      return null;
    }

    function collectScalarRows(prefix, value, rows, depth, maxRows){
      if (rows.length >= maxRows || depth > 8) return;
      if (value === null || value === undefined){
        rows.push({ key: prefix || '-', value: '-' });
        return;
      }
      if (Array.isArray(value)){
        for (let i = 0; i < value.length; i++){
          collectScalarRows((prefix || 'item') + '[' + i + ']', value[i], rows, depth + 1, maxRows);
          if (rows.length >= maxRows) return;
        }
        return;
      }
      if (typeof value === 'object'){
        const keys = Object.keys(value);
        if (!keys.length){ rows.push({ key: prefix || '-', value: '{}' }); return; }
        keys.forEach(function(k){
          if (rows.length >= maxRows) return;
          const nextPrefix = prefix ? (prefix + '.' + k) : k;
          collectScalarRows(nextPrefix, value[k], rows, depth + 1, maxRows);
        });
        return;
      }
      rows.push({ key: prefix || '-', value: String(value) });
    }

    function isNavPointObject(o){
      if (!o || typeof o !== 'object') return false;
      const hasXY = isFinite(Number(o.x)) && isFinite(Number(o.y));
      const hasLatLon = (isFinite(Number(o.lat)) && isFinite(Number(o.lon)))
        || (isFinite(Number(o.latitude)) && isFinite(Number(o.longitude)));
      const hasWpMeta = o.type || o.action || o.name || o.ETA || o.alt;
      return hasXY || hasLatLon || !!hasWpMeta;
    }

    function collectNavPoints(value, points, depth){
      if (points.length >= 200 || depth > 9 || value === null || value === undefined) return;
      if (Array.isArray(value)){
        value.forEach(function(item){ if (points.length < 200) collectNavPoints(item, points, depth + 1); });
        return;
      }
      if (typeof value !== 'object') return;
      if (isNavPointObject(value)) points.push(value);
      Object.keys(value).forEach(function(k){
        if (points.length >= 200) return;
        const child = value[k];
        if (child && typeof child === 'object') collectNavPoints(child, points, depth + 1);
      });
    }

    function formatDtcFocusedTable(root, selected){
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
      if (!cmdRows.length){
        lines.push('No CMDS data found.');
      } else {
        const maxKeyWidth = cmdRows.reduce(function(acc, r){ return Math.max(acc, String(r.key || '').length); }, 8);
        const keyWidth = clamp(maxKeyWidth + 1, 20, 64);
        cmdRows.forEach(function(r){
          lines.push(String(r.key || '-').padEnd(keyWidth) + String(r.value || '-').replace(/\s+/g, ' ').trim());
        });
      }

      lines.push('');
      lines.push('NAV POINTS');
      lines.push('----------');
      lines.push('WP  NAME         TYPE           ALT      ETA       LAT/LON               X            Y');
      lines.push('--- ------------ -------------- -------- -------- --------------------- ------------ ------------');
      if (!navPoints.length){
        lines.push('No nav points found.');
      } else {
        navPoints.slice(0, 200).forEach(function(p, idx){
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

    function formatEtaSeconds(v){
      const n = Number(v);
      if (!isFinite(n) || n < 0) return '-';
      const total = Math.floor(n);
      const h = Math.floor(total / 3600);
      const m = Math.floor((total % 3600) / 60);
      const s = total % 60;
      return String(h).padStart(2,'0') + ':' + String(m).padStart(2,'0') + ':' + String(s).padStart(2,'0');
    }

    function getTakeoffTimeBySelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      const value = Number(fltPlanEtaStartBySelection[key]);
      return isFinite(value) ? value : NaN;
    }

    const stepToStartSeconds = 10 * 60;
    const startToTaxiSeconds = 10 * 60;
    const taxiToTakeoffSeconds = 15 * 60;

    function buildTimingFromTakeoff(takeoffSec){
      const t = Number(takeoffSec);
      if (!isFinite(t)){
        return { step: NaN, start: NaN, taxi: NaN, takeoff: NaN };
      }

      return {
        step: t - (stepToStartSeconds + startToTaxiSeconds + taxiToTakeoffSeconds),
        start: t - (startToTaxiSeconds + taxiToTakeoffSeconds),
        taxi: t - taxiToTakeoffSeconds,
        takeoff: t,
      };
    }

    function getResolvedTimingMarks(selected){
      const state = getFlightPlanPlanState(selected);
      const defaults = buildTimingFromTakeoff(getTakeoffTimeBySelection(selected));
      const marks = (state && state.timeMarks && typeof state.timeMarks === 'object') ? state.timeMarks : {};

      function pick(name){
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

    function appendTimingLog(selected, anchor, timing){
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
      if (state.timingLog.length > 12){
        state.timingLog = state.timingLog.slice(state.timingLog.length - 12);
      }
    }

    function formatSignedDeltaSeconds(deltaSeconds){
      const n = Number(deltaSeconds);
      if (!isFinite(n)) return '';
      const sign = n >= 0 ? '+' : '-';
      const abs = Math.abs(Math.round(n));
      const h = Math.floor(abs / 3600);
      const m = Math.floor((abs % 3600) / 60);
      const s = abs % 60;
      return sign + String(h).padStart(2, '0') + ':' + String(m).padStart(2, '0') + ':' + String(s).padStart(2, '0');
    }

    function formatElapsedSeconds(seconds){
      const n = Number(seconds);
      if (!isFinite(n) || n < 0) return '-';
      const total = Math.round(n);
      const h = Math.floor(total / 3600);
      const m = Math.floor((total % 3600) / 60);
      const s = total % 60;
      return String(h).padStart(2, '0') + ':' + String(m).padStart(2, '0') + ':' + String(s).padStart(2, '0');
    }

    function clearSpeedRecommendations(selected){
      const state = getFlightPlanPlanState(selected);
      state.speedRecommendations = {};
    }

    function pruneExpiredSpeedRecommendations(state){
      if (!state || !state.speedRecommendations || typeof state.speedRecommendations !== 'object') return;
      const now = Date.now();
      Object.keys(state.speedRecommendations).forEach(function(k){
        const rec = state.speedRecommendations[k] || {};
        const expires = Number(rec.expiresUtcMs);
        if (isFinite(expires) && expires > 0 && now > expires){
          delete state.speedRecommendations[k];
        }
      });
    }

    function getPlanWaypointsForRecommendations(selected){
      try{
        if (!latestData) return [];

        const sourceType = String((latestData.DtcSourceType || '')).toUpperCase();
        const dtcJson = String(latestData.DtcJson || '').trim();
        let rows = [];

        if (selected === '__RUNTIME_PLAYER__'){
          rows = getMissionRuntimeWaypoints(latestData);
        } else if (dtcJson){
          let parsed;
          try{ parsed = JSON.parse(dtcJson); }catch(_){ parsed = null; }
          const root = (parsed && parsed.data && typeof parsed.data === 'object') ? parsed.data : parsed;
          if (root){
            if (sourceType === 'DTC'){
              const route = getDtcRouteBySelection(selected);
              const all = getDtcWaypoints(root);
              rows = filterDtcWaypointsByRoute(root, all, route);
            } else if (sourceType === 'RTE'){
              const names = Object.keys(root || {}).sort(function(a,b){ return String(a).localeCompare(String(b)); });
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
      }catch(_){
        return [];
      }
    }

    function parseWindBandsFromDiagnostics(data){
      const server = (data && data.Server) || {};
      const diagnostics = (server && server.Diagnostics && typeof server.Diagnostics === 'object') ? server.Diagnostics : {};
      const rows = Array.isArray(diagnostics.weatherSummary) ? diagnostics.weatherSummary : [];
      if (!rows.length) return null;

      const bands = {
        ground: { speedMs: NaN, dirDeg: NaN },
        a2000: { speedMs: NaN, dirDeg: NaN },
        a8000: { speedMs: NaN, dirDeg: NaN },
      };

      rows.forEach(function(row){
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

    function lerp(a, b, t){
      const av = Number(a);
      const bv = Number(b);
      const tv = Number(t);
      if (!isFinite(av) || !isFinite(bv) || !isFinite(tv)) return NaN;
      return av + ((bv - av) * tv);
    }

    function clamp01(v){
      const n = Number(v);
      if (!isFinite(n)) return 0;
      if (n < 0) return 0;
      if (n > 1) return 1;
      return n;
    }

    function sampleWindAtAltitude(bands, altFeet){
      if (!bands || typeof bands !== 'object'){
        return { speedMs: NaN, dirDeg: NaN };
      }

      const alt = Math.max(0, Number(altFeet) || 0);
      if (alt <= 2000){
        const t = clamp01(alt / 2000.0);
        return {
          speedMs: lerp(bands.ground && bands.ground.speedMs, bands.a2000 && bands.a2000.speedMs, t),
          dirDeg: lerp(bands.ground && bands.ground.dirDeg, bands.a2000 && bands.a2000.dirDeg, t),
        };
      }

      if (alt <= 8000){
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

    function getAlongTrackWindKnots(fromWp, toWp, sampleAltFeet){
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

    function computeRequiredKcasForLeg(fromWp, toWp, fromTimeSec, targetTimeSec){
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

    function getRecommendationMinKcas(altFeet){
      const alt = Math.max(0, Number(altFeet) || 0);
      const machMinKcas = (0.52 * 661.47) / (1.0 + (alt / 100000.0));
      return Math.max(220, machMinKcas);
    }

    function getRecommendationMaxKcas(altFeet){
      const alt = Math.max(0, Number(altFeet) || 0);
      const machMaxKcas = (0.98 * 661.47) / (1.0 + (alt / 100000.0));
      return Math.min(700, machMaxKcas);
    }

    function isKcasWithinRecommendationLimits(kcas, altFeet){
      const v = Number(kcas);
      if (!isFinite(v) || v <= 0) return false;
      const min = getRecommendationMinKcas(altFeet);
      const max = getRecommendationMaxKcas(altFeet);
      if (!isFinite(min) || !isFinite(max) || max < min) return false;
      if (v < min || v > max) return false;

      return true;
    }

    function roundRecommendedKcas(kcas){
      const n = Number(kcas);
      if (!isFinite(n)) return NaN;
      return Math.max(80, Math.round(n / 10) * 10);
    }

    function normalizeRecommendedKcas(kcas, altFeet){
      const rounded = roundRecommendedKcas(kcas);
      if (!isFinite(rounded)) return NaN;
      const min = getRecommendationMinKcas(altFeet);
      const max = getRecommendationMaxKcas(altFeet);
      if (!isFinite(min) || !isFinite(max) || max < min) return NaN;
      const adjusted = Math.max(min, rounded);
      return adjusted <= max ? adjusted : NaN;
    }

    function setSpeedRecommendation(state, step, recommendedKcas, useRed){
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

    function buildSpeedRecommendationsForAta(selected, ataStep){
      const state = getFlightPlanPlanState(selected);
      if (!state || !state.speedRecommendations) return;
      state.speedRecommendations = {};

      const rows = getPlanWaypointsForRecommendations(selected);
      if (!rows.length) return;

      const idx = rows.findIndex(function(wp){ return stepToKey(wp && wp.step) === stepToKey(ataStep); });
      if (idx < 0 || idx + 1 >= rows.length) return;

      const ata = state.ataByStep && state.ataByStep[stepToKey(ataStep)] ? state.ataByStep[stepToKey(ataStep)] : null;
      const fromTime = ata ? Number(ata.actualSeconds) : NaN;
      if (!isFinite(fromTime)) return;

      let fromPoint = rows[idx];
      if (fakeMissionEnabled && fakeMissionState && fakeMissionState.ownshipStarted){
        const ownX = Number(fakeMissionState.playerX);
        const ownY = Number(fakeMissionState.playerY);
        if (isFinite(ownX) && isFinite(ownY)){
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
      const nextTarget = parseEtaToSeconds(nextWp && (nextWp.etaDisplay || nextWp.eta));
      const reqNext = computeRequiredKcasForLeg(fromPoint, nextWp, fromTime, nextTarget);
      const reqNextRounded = normalizeRecommendedKcas(reqNext, nextWp && nextWp.altFeet);
      if (isKcasWithinRecommendationLimits(reqNextRounded, nextWp && nextWp.altFeet)){
        setSpeedRecommendation(state, nextWp.step, reqNextRounded, true);
        return;
      }

      if (idx + 2 >= rows.length) return;

      const wpA = rows[idx + 1];
      const wpB = rows[idx + 2];
      const targetB = parseEtaToSeconds(wpB && (wpB.etaDisplay || wpB.eta));
      const legA = computeLegDistanceNm(fromPoint, wpA);
      const legB = computeLegDistanceNm(wpA, wpB);
      if (!isFinite(legA) || !isFinite(legB) || legA <= 0 || legB <= 0) return;

      const totalDt = Number(targetB) - Number(fromTime);
      if (!isFinite(totalDt) || totalDt <= 0) return;

      const dtA = totalDt * (legA / (legA + legB));
      const dtB = totalDt - dtA;

      const reqA = normalizeRecommendedKcas(computeRequiredKcasForLeg(fromPoint, wpA, fromTime, fromTime + dtA), wpA && wpA.altFeet);
      const reqB = normalizeRecommendedKcas(computeRequiredKcasForLeg(wpA, wpB, fromTime + dtA, targetB), wpB && wpB.altFeet);
      if (!isKcasWithinRecommendationLimits(reqA, wpA && wpA.altFeet)) return;
      if (!isKcasWithinRecommendationLimits(reqB, wpB && wpB.altFeet)) return;

      setSpeedRecommendation(state, wpA.step, reqA, true);
      setSpeedRecommendation(state, wpB.step, reqB, true);
    }

    function acceptSpeedRecommendation(selected, step){
      const state = getFlightPlanPlanState(selected);
      if (!state || !state.speedRecommendations) return false;
      pruneExpiredSpeedRecommendations(state);
      const key = stepToKey(step);
      const rec = state.speedRecommendations[key];
      if (!rec) return false;

      const rows = getPlanWaypointsForRecommendations(selected);
      const wp = rows.find(function(r){ return stepToKey(r && r.step) === key; });
      if (!wp) return false;

      const current = Number(wp.spd);
      const target = Number(rec.kcas);
      if (!isFinite(current) || !isFinite(target)) return false;

      changeWaypointSpeedAdjustment(selected, key, target - current);
      delete state.speedRecommendations[key];
      return true;
    }

    function togglePostFlightOpen(selected){
      const state = getFlightPlanPlanState(selected);
      state.postFlightOpen = !state.postFlightOpen;
    }

    function clearNavlogRowAction(selected){
      const state = getFlightPlanPlanState(selected);
      state.rowActionStep = '';
      state.rowActionMode = '';
    }

    function clearNavlogRowReveal(selected){
      const state = getFlightPlanPlanState(selected);
      state.rowRevealStep = '';
      state.rowRevealMode = '';
    }

    function setNavlogRowReveal(selected, step, mode){
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      const revealMode = String(mode || '').toLowerCase();
      if (!key || (revealMode !== 'dir' && revealMode !== 'del')){
        clearNavlogRowReveal(selected);
        return;
      }
      if (state.rowRevealStep === key && state.rowRevealMode === revealMode){
        clearNavlogRowReveal(selected);
        return;
      }
      state.rowRevealStep = key;
      state.rowRevealMode = revealMode;
    }

    function setNavlogRowAction(selected, step, mode){
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      const actionMode = String(mode || '').toLowerCase();
      if (!key || (actionMode !== 'dir' && actionMode !== 'del')){
        clearNavlogRowAction(selected);
        return;
      }
      if (state.rowActionStep === key && state.rowActionMode === actionMode){
        clearNavlogRowAction(selected);
        return;
      }
      state.rowActionStep = key;
      state.rowActionMode = actionMode;
    }

    function isNavlogRowActionArmed(selected, step, mode){
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      const actionMode = String(mode || '').toLowerCase();
      if (!key || (actionMode !== 'dir' && actionMode !== 'del')) return false;
      return stepToKey(state.rowActionStep) === key
        && String(state.rowActionMode || '').toLowerCase() === actionMode;
    }

    function deleteNavlogStep(selected, step){
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      if (!key) return;
      if (!state.deletedSteps || typeof state.deletedSteps !== 'object') state.deletedSteps = {};
      state.deletedSteps[key] = true;
      if (stepToKey(state.lockedStep) === key) state.lockedStep = '';
      if (state.speedAdjustments && typeof state.speedAdjustments === 'object') delete state.speedAdjustments[key];
      if (state.altAdjustments && typeof state.altAdjustments === 'object') delete state.altAdjustments[key];
      if (state.typeOverrides && typeof state.typeOverrides === 'object') delete state.typeOverrides[key];
      if (state.ataByStep && typeof state.ataByStep === 'object') delete state.ataByStep[key];
      if (state.speedRecommendations && typeof state.speedRecommendations === 'object') delete state.speedRecommendations[key];
      if (state.totPerformanceByStep && typeof state.totPerformanceByStep === 'object') delete state.totPerformanceByStep[key];
      if (state.rowActionStep === key){
        state.rowActionStep = '';
        state.rowActionMode = '';
      }
      if (state.rowRevealStep === key){
        state.rowRevealStep = '';
        state.rowRevealMode = '';
      }
      ensureDirectToStateValid(getPlanWaypointsForRecommendations(selected), state);
      clearSpeedRecommendations(selected);
    }

    function setNavlogDirectToSource(selected, step){
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      if (!key) return;
      state.directToSourceStep = key;
      state.directToTargetStep = '';
      clearSpeedRecommendations(selected);
    }

    function setNavlogDirectToTarget(selected, step){
      const state = getFlightPlanPlanState(selected);
      const targetKey = stepToKey(step);
      const sourceKey = stepToKey(state.directToSourceStep);
      if (!sourceKey || !targetKey || sourceKey === targetKey) return false;
      state.directToTargetStep = targetKey;
      const rows = getPlanWaypointsForRecommendations(selected);
      ensureDirectToStateValid(rows, state);
      if (!stepToKey(state.directToTargetStep)) return false;
      clearSpeedRecommendations(selected);
      return true;
    }

    function clearNavlogDirectTo(selected){
      const state = getFlightPlanPlanState(selected);
      clearInvalidDirectToState(state);
      clearSpeedRecommendations(selected);
    }

    function toggleWaypointAta(selected, step, plannedEtaText){
      const state = getFlightPlanPlanState(selected);
      if (!state.ataByStep || typeof state.ataByStep !== 'object') state.ataByStep = {};
      if (!Array.isArray(state.timingLog)) state.timingLog = [];

      const key = stepToKey(step);
      if (!key) return;

      if (state.ataByStep[key]){
        delete state.ataByStep[key];
        clearSpeedRecommendations(selected);
        return;
      }

      const planned = parseEtaToSeconds(plannedEtaText);
      const actual = getCurrentFlightPlanClockSeconds();
      if (!isFinite(planned) || !isFinite(actual)) return;

      state.ataByStep[key] = {
        actualSeconds: actual,
        plannedSeconds: planned,
      };

      const keys = Object.keys(state.ataByStep)
        .map(function(k){ return String(k); })
        .sort(function(a, b){ return Number(a) - Number(b); });

      const parts = keys.map(function(k){
        const entry = state.ataByStep[k] || {};
        const actualSec = Number(entry.actualSeconds);
        const plannedSec = Number(entry.plannedSeconds);
        if (!isFinite(actualSec) || !isFinite(plannedSec)) return '';
        return 'STP' + String(k) + ' ' + formatSignedDeltaSeconds(actualSec - plannedSec);
      }).filter(function(x){ return !!x; });

      if (parts.length){
        const row = new Date().toISOString() + ' | ATA ' + parts.join(' ');
        state.timingLog.push(row);
        if (state.timingLog.length > 12){
          state.timingLog = state.timingLog.slice(state.timingLog.length - 12);
        }
      }

      buildSpeedRecommendationsForAta(selected, key);
    }

    function hasTakeoffTimeBySelection(selected){
      return isFinite(getTakeoffTimeBySelection(selected));
    }

    function getFlightPlanTimingDisplay(selected){
      const timing = getResolvedTimingMarks(selected);
      const takeoffSec = Number(timing.takeoff);
      if (!isFinite(takeoffSec)){
        return {
          step: '-',
          start: '-',
          taxi: '-',
          takeoff: '-',
          tot: '-',
        };
      }

      const planState = getFlightPlanPlanState(selected);
      const totSec = isFinite(Number(planState.totSeconds)) ? Number(planState.totSeconds) : NaN;

      return {
        step: formatSecondsToClock(timing.step),
        start: formatSecondsToClock(timing.start),
        taxi: formatSecondsToClock(timing.taxi),
        takeoff: formatSecondsToClock(timing.takeoff),
        tot: isFinite(totSec) ? formatSecondsToClock(totSec) : '-',
      };
    }

    function getFlightPlanPlanState(selected){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return { speedAdjustments: {}, speedDisplayModes: {}, altAdjustments: {}, typeOverrides: {}, lockedStep: '', totSeconds: NaN, lockedStart: null, timeMarks: {}, timingLog: [], postFlightOpen: false, ataByStep: {}, speedRecommendations: {}, totPerformanceByStep: {}, lastOwnshipPos: null, deletedSteps: {}, directToSourceStep: '', directToTargetStep: '', rowActionStep: '', rowActionMode: '', rowRevealStep: '', rowRevealMode: '', coordDisplayMode: 'xy' };
      const existing = fltPlanPlanStateBySelection[key];
      if (existing && typeof existing === 'object'){
        if (!existing.speedAdjustments || typeof existing.speedAdjustments !== 'object') existing.speedAdjustments = {};
        if (!existing.speedDisplayModes || typeof existing.speedDisplayModes !== 'object') existing.speedDisplayModes = {};
        if (!existing.altAdjustments || typeof existing.altAdjustments !== 'object') existing.altAdjustments = {};
        if (!existing.typeOverrides || typeof existing.typeOverrides !== 'object') existing.typeOverrides = {};
        if (!existing.timeMarks || typeof existing.timeMarks !== 'object') existing.timeMarks = {};
        if (!Array.isArray(existing.timingLog)) existing.timingLog = [];
        if (typeof existing.postFlightOpen !== 'boolean') existing.postFlightOpen = false;
        if (!existing.ataByStep || typeof existing.ataByStep !== 'object') existing.ataByStep = {};
        if (!existing.speedRecommendations || typeof existing.speedRecommendations !== 'object') existing.speedRecommendations = {};
        if (!existing.totPerformanceByStep || typeof existing.totPerformanceByStep !== 'object') existing.totPerformanceByStep = {};
        if (!existing.lastOwnshipPos || typeof existing.lastOwnshipPos !== 'object') existing.lastOwnshipPos = null;
        if (!existing.deletedSteps || typeof existing.deletedSteps !== 'object') existing.deletedSteps = {};
        if (typeof existing.directToSourceStep !== 'string') existing.directToSourceStep = '';
        if (typeof existing.directToTargetStep !== 'string') existing.directToTargetStep = '';
        if (typeof existing.rowActionStep !== 'string') existing.rowActionStep = '';
        if (typeof existing.rowActionMode !== 'string') existing.rowActionMode = '';
        if (typeof existing.rowRevealStep !== 'string') existing.rowRevealStep = '';
        if (typeof existing.rowRevealMode !== 'string') existing.rowRevealMode = '';
        if (typeof existing.coordDisplayMode !== 'string') existing.coordDisplayMode = 'xy';
        return existing;
      }
      const created = { speedAdjustments: {}, speedDisplayModes: {}, altAdjustments: {}, typeOverrides: {}, lockedStep: '', totSeconds: NaN, lockedStart: null, timeMarks: {}, timingLog: [], postFlightOpen: false, ataByStep: {}, speedRecommendations: {}, totPerformanceByStep: {}, lastOwnshipPos: null, deletedSteps: {}, directToSourceStep: '', directToTargetStep: '', rowActionStep: '', rowActionMode: '', rowRevealStep: '', rowRevealMode: '', coordDisplayMode: 'xy' };
      fltPlanPlanStateBySelection[key] = created;
      return created;
    }

    function lockStartPositionForSelection(selected, data){
      const state = getFlightPlanPlanState(selected);
      const server = (data && data.Server) || {};
      const simActive = !!(fakeMissionEnabled && fakeMissionState);
      const x = Number(simActive ? fakeMissionState.playerX : server.PlayerPosX);
      const y = Number(simActive ? fakeMissionState.playerY : server.PlayerPosY);
      const altFeet = Number(simActive ? fakeMissionState.playerAltFeet : server.PlayerAltFeet);
      if (!isFinite(x) || !isFinite(y)) return;
      state.lockedStart = {
        x: x,
        y: y,
        altFeet: isFinite(altFeet) ? altFeet : NaN,
      };
    }

    function stepToKey(step){
      return String(step || '').trim();
    }

    function changeWaypointSpeedAdjustment(selected, step, delta){
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      if (!key) return;
      const current = Number(state.speedAdjustments[key]);
      const next = (isFinite(current) ? current : 0) + Number(delta || 0);
      state.speedAdjustments[key] = clamp(next, -600, 600);
    }

    function canUseMachDisplay(altFeet){
      return isFinite(Number(altFeet)) && Number(altFeet) > 28000;
    }

    function getWaypointSpeedDisplayMode(state, step, altFeet){
      const key = stepToKey(step);
      const safeState = state || {};
      const map = (safeState.speedDisplayModes && typeof safeState.speedDisplayModes === 'object') ? safeState.speedDisplayModes : {};
      let mode = String(map[key] || 'KCAS').toUpperCase();
      if (mode !== 'MACH') mode = 'KCAS';
      if (mode === 'MACH' && !canUseMachDisplay(altFeet)) mode = 'KCAS';
      return mode;
    }

    function toggleWaypointSpeedDisplayMode(selected, step, altFeet){
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      if (!key || !state.speedDisplayModes) return;

      if (!canUseMachDisplay(altFeet)){
        state.speedDisplayModes[key] = 'KCAS';
        return;
      }

      const current = getWaypointSpeedDisplayMode(state, key, altFeet);
      state.speedDisplayModes[key] = (current === 'MACH') ? 'KCAS' : 'MACH';
    }

    function changeWaypointSpeedAdjustmentByDirection(selected, step, direction, altFeet){
      const dir = Number(direction);
      if (!isFinite(dir) || dir === 0) return;

      const state = getFlightPlanPlanState(selected);
      const mode = getWaypointSpeedDisplayMode(state, step, altFeet);

      let deltaKcas = 10 * (dir >= 0 ? 1 : -1);
      if (mode === 'MACH' && canUseMachDisplay(altFeet)){
        const conversion = 661.47 / (1.0 + (Math.max(0, Number(altFeet) || 0) / 100000.0));
        deltaKcas = 0.01 * conversion * (dir >= 0 ? 1 : -1);
      }

      changeWaypointSpeedAdjustment(selected, step, deltaKcas);
    }

    function formatWaypointSpeedDisplay(speedKcasText, mode, altFeet){
      const kcas = Number(speedKcasText);
      if (!isFinite(kcas) || kcas <= 0) return '-';

      if (mode === 'MACH' && canUseMachDisplay(altFeet)){
        const cas = Math.max(0, kcas);
        const tas = cas * (1.0 + (Math.max(0, Number(altFeet) || 0) / 100000.0));
        const mach = tas / 661.47;
        if (!isFinite(mach) || mach <= 0) return '-';
        return 'M ' + mach.toFixed(2);
      }

      return String(Math.round(kcas));
    }

    function getWaypointSpeedAdjustment(state, step){
      const key = stepToKey(step);
      if (!key || !state || !state.speedAdjustments) return 0;
      const v = Number(state.speedAdjustments[key]);
      return isFinite(v) ? v : 0;
    }

    function setLockedTotStep(selected, step, locked, etaSeconds){
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      state.lockedStep = locked ? key : '';
      if (locked){
        const eta = Number(etaSeconds);
        if (isFinite(eta)){
          state.totSeconds = eta;
        }
      }
    }

    function setTotByMinutesDelta(selected, minutesDelta){
      const state = getFlightPlanPlanState(selected);
      let base = Number(state.totSeconds);
      if (!isFinite(base)){
        base = getCurrentFlightPlanClockSeconds();
      }
      const delta = Math.round(Number(minutesDelta || 0) * 60);
      state.totSeconds = base + delta;
    }

    function setTakeoffBySecondsDelta(selected, secondsDelta){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;

      const delta = Math.round(Number(secondsDelta || 0));
      if (!isFinite(delta) || delta === 0) return;

      const state = getFlightPlanPlanState(selected);
      const marks = getResolvedTimingMarks(selected);
      if (!isFinite(Number(marks.takeoff))){
        const now = getCurrentFlightPlanClockSeconds();
        const defaults = buildTimingFromTakeoff(now);
        marks.step = defaults.step;
        marks.start = defaults.start;
        marks.taxi = defaults.taxi;
        marks.takeoff = defaults.takeoff;
      }

      marks.step = Number(marks.step) + delta;
      marks.start = Number(marks.start) + delta;
      marks.taxi = Number(marks.taxi) + delta;
      marks.takeoff = Number(marks.takeoff) + delta;

      state.timeMarks = {
        step: marks.step,
        start: marks.start,
        taxi: marks.taxi,
        takeoff: marks.takeoff,
      };

      if (isFinite(Number(marks.takeoff))){
        fltPlanEtaStartBySelection[key] = Number(marks.takeoff);
      }

      appendTimingLog(selected, 'TAKEOFF', marks);
    }

    function setTakeoffByMinutesDelta(selected, minutesDelta){
      const delta = Math.round(Number(minutesDelta || 0) * 60);
      setTakeoffBySecondsDelta(selected, delta);
    }

    function resolveWaypointNorthEast(wp){
      if (!wp || typeof wp !== 'object') return { north: NaN, east: NaN };
      const north = Number(isFinite(Number(wp.xNum)) ? wp.xNum : wp.x);
      const east = Number(isFinite(Number(wp.yNum)) ? wp.yNum : wp.y);
      return { north: north, east: east };
    }

    function distancePointToSegmentMeters(px, py, ax, ay, bx, by){
      const vx = bx - ax;
      const vy = by - ay;
      const wx = px - ax;
      const wy = py - ay;
      const vv = (vx * vx) + (vy * vy);
      if (!isFinite(vv) || vv <= 0){
        const dx = px - ax;
        const dy = py - ay;
        return Math.sqrt((dx * dx) + (dy * dy));
      }
      let t = ((wx * vx) + (wy * vy)) / vv;
      t = clamp(t, 0, 1);
      const cx = ax + (vx * t);
      const cy = ay + (vy * t);
      const dx = px - cx;
      const dy = py - cy;
      return Math.sqrt((dx * dx) + (dy * dy));
    }

    function getOwnshipNorthEast(){
      if (fakeMissionEnabled && fakeMissionState){
        return { north: Number(fakeMissionState.playerX), east: Number(fakeMissionState.playerY) };
      }

      const server = (latestData && latestData.Server) || {};
      return { north: Number(server.PlayerPosX), east: Number(server.PlayerPosY) };
    }

    function updateTotOverflyCapture(selected, rows){
      const list = Array.isArray(rows) ? rows : [];
      if (!list.length) return;

      const state = getFlightPlanPlanState(selected);
      if (!state.totPerformanceByStep || typeof state.totPerformanceByStep !== 'object') state.totPerformanceByStep = {};
      if (!Array.isArray(state.timingLog)) state.timingLog = [];

      const lockedStep = stepToKey(state.lockedStep);
      const totSec = Number(state.totSeconds);
      if (!lockedStep || !isFinite(totSec)) return;

      if (state.totPerformanceByStep[lockedStep]) return;

      const target = list.find(function(wp){
        return wp && !wp.isStart && stepToKey(wp.step) === lockedStep;
      });
      if (!target) return;

      if (String(target.type || '').toUpperCase() !== 'TGT') return;

      const own = getOwnshipNorthEast();
      const wpPos = resolveWaypointNorthEast(target);
      if (!isFinite(own.north) || !isFinite(own.east) || !isFinite(wpPos.north) || !isFinite(wpPos.east)) return;

      const prevOwn = state.lastOwnshipPos && isFinite(Number(state.lastOwnshipPos.north)) && isFinite(Number(state.lastOwnshipPos.east))
        ? { north: Number(state.lastOwnshipPos.north), east: Number(state.lastOwnshipPos.east) }
        : null;
      state.lastOwnshipPos = { north: own.north, east: own.east };

      const dNorth = wpPos.north - own.north;
      const dEast = wpPos.east - own.east;
      const distanceMeters = Math.sqrt((dNorth * dNorth) + (dEast * dEast));
      const overflyThresholdMeters = 1 * 1852;
      let crossedWithinThreshold = isFinite(distanceMeters) && distanceMeters <= overflyThresholdMeters;
      if (!crossedWithinThreshold && prevOwn){
        const segDistance = distancePointToSegmentMeters(
          wpPos.north,
          wpPos.east,
          prevOwn.north,
          prevOwn.east,
          own.north,
          own.east
        );
        crossedWithinThreshold = isFinite(segDistance) && segDistance <= overflyThresholdMeters;
      }
      if (!crossedWithinThreshold) return;

      const actualSec = getCurrentFlightPlanClockSeconds();
      if (!isFinite(actualSec)) return;

      state.totPerformanceByStep[lockedStep] = {
        plannedSeconds: totSec,
        actualSeconds: actualSec,
        capturedUtc: new Date().toISOString(),
      };

      const perf = state.totPerformanceByStep[lockedStep];
      const row = (perf.capturedUtc || new Date().toISOString())
        + ' | TOT PERF STP' + lockedStep
        + ' ETA ' + formatSecondsToClock(Number(perf.plannedSeconds))
        + ' ATA ' + formatSecondsToClock(Number(perf.actualSeconds))
        + ' ' + formatSignedDeltaSeconds(Number(perf.actualSeconds) - Number(perf.plannedSeconds));
      state.timingLog.push(row);
      if (state.timingLog.length > 16){
        state.timingLog = state.timingLog.slice(state.timingLog.length - 16);
      }
    }

    function buildPostFlightSummaryRows(selected, rows, timing){
      const state = getFlightPlanPlanState(selected);
      const list = Array.isArray(rows) ? rows : [];
      const out = [];

      out.push('TIMING STEP ' + safe(timing && timing.step)
        + ' START ' + safe(timing && timing.start)
        + ' TAXI ' + safe(timing && timing.taxi)
        + ' TAKEOFF ' + safe(timing && timing.takeoff));

      let prevEtaSec = NaN;
      list.forEach(function(wp){
        if (!wp || wp.isStart) return;

        const step = stepToKey(wp.step);
        const etaText = String(wp.etaDisplay || wp.eta || '-');
        const etaSec = parseEtaToSeconds(etaText);
        const eteText = isFinite(prevEtaSec) && isFinite(etaSec)
          ? formatElapsedSeconds(etaSec - prevEtaSec)
          : '-';

        const ataEntry = (state && state.ataByStep && state.ataByStep[step]) ? state.ataByStep[step] : null;
        const ataSec = ataEntry ? Number(ataEntry.actualSeconds) : NaN;
        const ataText = isFinite(ataSec) ? formatSecondsToClock(ataSec) : '-';
        const ataDelta = (isFinite(ataSec) && isFinite(etaSec))
          ? formatSignedDeltaSeconds(ataSec - etaSec)
          : '';

        out.push('STP ' + step
          + ' ETE ' + eteText
          + ' ETA ' + etaText
          + ' ATA ' + ataText
          + (ataDelta ? (' ' + ataDelta) : ''));

        if (isFinite(etaSec)) prevEtaSec = etaSec;
      });

      const lockedStep = stepToKey(state && state.lockedStep);
      const perf = lockedStep && state && state.totPerformanceByStep ? state.totPerformanceByStep[lockedStep] : null;
      if (lockedStep && isFinite(Number(state && state.totSeconds))){
        if (perf && isFinite(Number(perf.actualSeconds))){
          out.push('TOT PERF STP ' + lockedStep
            + ' ETA ' + formatSecondsToClock(Number(perf.plannedSeconds))
            + ' ATA ' + formatSecondsToClock(Number(perf.actualSeconds))
            + ' ' + formatSignedDeltaSeconds(Number(perf.actualSeconds) - Number(perf.plannedSeconds)));
        } else {
          out.push('TOT PERF STP ' + lockedStep
            + ' ETA ' + formatSecondsToClock(Number(state.totSeconds))
            + ' ATA -');
        }
      }

      return out;
    }

    function setTotBySecondsDelta(selected, secondsDelta){
      const state = getFlightPlanPlanState(selected);
      let base = Number(state.totSeconds);
      if (!isFinite(base)){
        base = getCurrentFlightPlanClockSeconds();
      }
      const delta = Math.round(Number(secondsDelta || 0));
      state.totSeconds = base + delta;
    }

    function abbreviateRouteType(type){
      const raw = String(type || '').trim();
      if (!raw) return 'WP';
      const lower = raw.toLowerCase();
      if (lower === 'turning point') return 'TP';
      if (lower === 'waypoint') return 'WP';
      if (lower === 'initial point') return 'IP';
      if (lower === 'target') return 'TGT';
      if (lower === 'hold') return 'HLD';
      if (lower === 'aar' || lower.indexOf('air refuel') >= 0) return 'AAR';
      if (lower === 'cap' || lower.indexOf('combat air patrol') >= 0) return 'CAP';
      if (lower === 'tak' || lower === 'tko' || lower.indexOf('takeoff') >= 0 || lower.indexOf('take off') >= 0) return 'TKO';
      if (lower === 'ldg' || lower === 'land' || lower.indexOf('landing') >= 0) return 'LDG';
      if (lower === 'dvrt' || lower.indexOf('divert') >= 0 || lower.indexOf('diversion') >= 0) return 'DVRT';
      const compact = raw.replace(/[^A-Za-z0-9]/g, '').toUpperCase();
      if (compact === 'TAK' || compact.indexOf('TAKEOFF') === 0) return 'TKO';
      if (!compact) return 'WP';
      return compact.substring(0, 3);
    }

    function normalizeEditableWaypointType(type){
      const norm = abbreviateRouteType(type);
      const allowed = ['WP','TP','IP','TGT','AAR','CAP','HLD','TKO','LDG','DVRT'];
      return allowed.indexOf(norm) >= 0 ? norm : 'WP';
    }

    function changeWaypointType(selected, step, delta, currentType){
      const state = getFlightPlanPlanState(selected);
      if (!state.typeOverrides || typeof state.typeOverrides !== 'object') state.typeOverrides = {};
      const key = stepToKey(step);
      if (!key) return;
      const allowed = ['WP','TP','IP','TGT','AAR','CAP','HLD','TKO','LDG','DVRT'];
      const baseline = state.typeOverrides[key] || currentType || 'WP';
      const current = normalizeEditableWaypointType(baseline);
      const idx = allowed.indexOf(current);
      const d = Number(delta || 0);
      const nextIndex = ((idx + (d >= 0 ? 1 : -1)) % allowed.length + allowed.length) % allowed.length;
      state.typeOverrides[key] = allowed[nextIndex];
    }

    function applyTypeOverrides(rows, selected){
      const list = Array.isArray(rows) ? rows : [];
      const state = getFlightPlanPlanState(selected);
      const overrides = (state && state.typeOverrides && typeof state.typeOverrides === 'object') ? state.typeOverrides : {};
      list.forEach(function(wp){
        if (!wp || wp.isStart) return;
        const key = stepToKey(wp.step);
        const rawOverride = String(overrides[key] || '').trim();
        if (!rawOverride) return;
        const override = normalizeEditableWaypointType(rawOverride);
        wp.type = override;
        wp.typeRaw = override;
      });
      return list;
    }

    function parseEtaToSeconds(etaText){
      const text = String(etaText || '').trim();
      const m = text.match(/^(\d{1,2}):(\d{2}):(\d{2})$/);
      if (!m) return NaN;
      const h = parseInt(m[1], 10);
      const mm = parseInt(m[2], 10);
      const s = parseInt(m[3], 10);
      if (!isFinite(h) || !isFinite(mm) || !isFinite(s)) return NaN;
      if (mm < 0 || mm > 59 || s < 0 || s > 59) return NaN;
      return (h * 3600) + (mm * 60) + s;
    }

    function formatSecondsToClock(seconds){
      let total = Number(seconds);
      if (!isFinite(total)) return '-';
      total = Math.round(total);
      total = ((total % 86400) + 86400) % 86400;
      const h = Math.floor(total / 3600);
      const m = Math.floor((total % 3600) / 60);
      const s = total % 60;
      return String(h).padStart(2,'0') + ':' + String(m).padStart(2,'0') + ':' + String(s).padStart(2,'0');
    }

    function getMissionClockSeconds(data){
      const server = (data && data.Server) || {};
      const mission = Number(server.MissionTimeSeconds);
      if (isFinite(mission) && mission >= 0) return mission;
      return NaN;
    }

    function getSystemLocalClockSeconds(){
      const now = new Date();
      return (now.getHours() * 3600) + (now.getMinutes() * 60) + now.getSeconds();
    }

    function getTheaterUtcOffsetHours(theater){
      const t = String(theater || '').toUpperCase();
      if (t.indexOf('CAUCASUS') >= 0) return 4;
      if (t.indexOf('MARIANA') >= 0) return 10;
      if (t.indexOf('PERSIAN') >= 0) return 4;
      if (t.indexOf('SYRIA') >= 0) return 3;
      if (t.indexOf('SINAI') >= 0) return 2;
      if (t.indexOf('NEVADA') >= 0) return -8;
      if (t.indexOf('NORMANDY') >= 0) return 1;
      if (t.indexOf('KOLA') >= 0) return 2;
      if (t.indexOf('AFGHAN') >= 0) return 4.5;
      if (t.indexOf('SOUTH ATLANTIC') >= 0) return -3;
      return NaN;
    }

    function missionToUtcSeconds(missionSeconds, theater){
      const mission = Number(missionSeconds);
      const offset = Number(getTheaterUtcOffsetHours(theater));
      if (!isFinite(mission) || !isFinite(offset)) return NaN;
      return mission - (offset * 3600);
    }

    function updateMissionClockLabel(data){
      const label = document.getElementById('missionClockLabel');
      if (!label) return;

      const model = data || latestData || {};
      const server = (model && model.Server) || {};
      const mission = getCurrentFlightPlanClockSeconds();
      if (!isFinite(mission)){
        label.textContent = 'Mission Time - (- UTC)';
        return;
      }

      const utcSeconds = missionToUtcSeconds(mission, server.Theater);
      const utcText = isFinite(utcSeconds) ? formatSecondsToClock(utcSeconds) : '-';
      label.textContent = 'Mission Time ' + formatSecondsToClock(mission) + ' (' + utcText + ' UTC)';
    }

    function getCurrentFlightPlanClockSeconds(){
      if (fakeMissionEnabled && fakeMissionState && isFinite(Number(fakeMissionState.simMissionSeconds))){
        return Number(fakeMissionState.simMissionSeconds);
      }

      if (isFinite(Number(missionClockAnchorSeconds)) && missionClockAnchorSystemMs > 0){
        const elapsed = (Date.now() - missionClockAnchorSystemMs) / 1000.0;
        if (isFinite(elapsed)){
          return Number(missionClockAnchorSeconds) + Math.max(0, elapsed);
        }
      }

      const mission = getMissionClockSeconds(latestData);
      if (isFinite(mission)) return mission;
      return getSystemLocalClockSeconds();
    }

    function setMissionClockAnchor(missionSeconds, systemNowMs){
      const m = Number(missionSeconds);
      if (!isFinite(m) || m < 0) return;
      missionClockAnchorSeconds = m;
      missionClockAnchorSystemMs = isFinite(Number(systemNowMs)) ? Number(systemNowMs) : Date.now();
    }

    function resetMissionClockAnchor(){
      missionClockAnchorSeconds = NaN;
      missionClockAnchorSystemMs = 0;
      missionClockAnchorIdentity = '';
    }

    function getFlightPlanEtaStartKey(selected){
      return String(selected || '');
    }

    function getActiveFlightPlanSelection(data){
      const model = data || latestData || {};
      const selected = String(model.DtcSelectedFile || '');
      if (selected) return selected;
      const runtimeWaypoints = getMissionRuntimeWaypoints(model);
      if (runtimeWaypoints.length) return '__RUNTIME_PLAYER__';
      return '';
    }

    function getMissionIdentity(data){
      const server = (data && data.Server) || {};
      const rawCallsign = String(server.PlayerCallsign || '').trim();
      if (rawCallsign){
        lastKnownPlayerCallsign = rawCallsign;
      }
      const stableCallsign = rawCallsign || lastKnownPlayerCallsign;
      return [
        String(server.Theater || ''),
        String(server.MissionTitle || ''),
        String(server.Aircraft || ''),
        String(stableCallsign || ''),
        server.Multiplayer ? '1' : '0'
      ].join('|');
    }

    function resetRuntimeFlightPlanState(){
      const key = '__RUNTIME_PLAYER__';
      delete fltPlanEtaStartBySelection[key];
      delete fltPlanPlanStateBySelection[key];
      delete fltPlanDtcPageBySelection[key];
      delete fltPlanDtcRouteBySelection[key];
      delete fltPlanMapViewBySelection[key];
      delete fltPlanMapBackgroundEnabledBySelection[key];
      delete fltPlanOpenFreeMapViewBySelection[key];
      delete fltPlanMapSelectedAssetKeyBySelection[key];
      delete openFreeMapContainerIdBySelectionKey[key];
      if (!fakeMissionEnabled){
        resetMissionClockAnchor();
      }
    }

    function invalidateRuntimeFlightPlanSnapshot(){
      runtimeFlightPlanSnapshot = null;
      runtimeFlightPlanSnapshotMissionIdentity = '';
      runtimeFlightPlanSnapshotGroupName = '';
    }

    function maybeResetFlightPlanStateForMission(data){
      const identity = getMissionIdentity(data);
      const missionClock = getMissionClockSeconds(data);
      const identityChanged = !!identity && !!lastMissionIdentity && identity !== lastMissionIdentity;
      const missionClockRewound = isFinite(missionClock)
        && isFinite(lastMissionClockSeconds)
        && missionClock + 60 < lastMissionClockSeconds;

      if (identityChanged || missionClockRewound){
        resetRuntimeFlightPlanState();
        invalidateRuntimeFlightPlanSnapshot();
        resetMissionClockAnchor();
      }

      if (identity){
        lastMissionIdentity = identity;
      }
      if (isFinite(missionClock)){
        lastMissionClockSeconds = missionClock;
      }
    }

    function getDtcPageBySelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      const v = Number(fltPlanDtcPageBySelection[key]);
      return (v === 2 || v === 3 || v === 4) ? v : 1;
    }

    function setDtcPageBySelection(selected, page){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const p = Number(page);
      fltPlanDtcPageBySelection[key] = (p === 2 || p === 3 || p === 4) ? p : 1;
    }

    function isValidDtcRouteKey(route){
      return /^R([1-9]|1[0-2])$/.test(String(route || '').toUpperCase());
    }

    function getDtcRouteBySelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      const route = String(fltPlanDtcRouteBySelection[key] || '').toUpperCase();
      return isValidDtcRouteKey(route) ? route : 'R1';
    }

    function setDtcRouteBySelection(selected, route){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const r = String(route || '').toUpperCase();
      fltPlanDtcRouteBySelection[key] = isValidDtcRouteKey(r) ? r : 'R1';
    }

    function updateDtcRouteButtonUi(selected){
      const host = document.getElementById('tabBody');
      if (!host || !selected) return;
      const activeRoute = getDtcRouteBySelection(selected);
      const buttons = host.querySelectorAll ? host.querySelectorAll('[data-dtc-route]') : [];
      for (let i = 0; i < buttons.length; i++){
        const btn = buttons[i];
        if (!btn || !btn.classList || !btn.getAttribute) continue;
        const key = String(btn.getAttribute('data-dtc-route') || '').toUpperCase();
        btn.classList.toggle('active', key === activeRoute);
      }
    }

    function isF14DtcContext(root, data){
      const rootType = String((root && root.type) || '').toUpperCase();
      if (rootType.indexOf('F-14') >= 0 || rootType.indexOf('TOMCAT') >= 0) return true;
      const model = data || latestData || {};
      const aircraft = String((((model && model.Server) || {}).Aircraft) || '').toUpperCase();
      return aircraft.indexOf('F-14') >= 0 || aircraft.indexOf('TOMCAT') >= 0;
    }

    function getMapViewBySelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return { zoom: 1, panX: 0, panY: 0 };
      const existing = fltPlanMapViewBySelection[key];
      if (existing && isFinite(Number(existing.zoom)) && isFinite(Number(existing.panX)) && isFinite(Number(existing.panY))){
        return existing;
      }
      const state = { zoom: 1, panX: 0, panY: 0 };
      fltPlanMapViewBySelection[key] = state;
      return state;
    }

    function resetMapViewBySelection(selected){
      const state = getMapViewBySelection(selected);
      state.zoom = 1;
      state.panX = 0;
      state.panY = 0;
    }

    function zoomMapViewBySelection(selected, zoomFactor){
      const state = getMapViewBySelection(selected);
      const factor = Number(zoomFactor);
      if (!isFinite(factor) || factor <= 0) return;
      const current = isFinite(Number(state.zoom)) ? Number(state.zoom) : 1;
      state.zoom = clamp(current * factor, 0.6, 4.0);
    }

    function isMapBackgroundEnabledBySelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return true;
      if (!Object.prototype.hasOwnProperty.call(fltPlanMapBackgroundEnabledBySelection, key)){
        fltPlanMapBackgroundEnabledBySelection[key] = true;
      }
      return !!fltPlanMapBackgroundEnabledBySelection[key];
    }

    function toggleMapBackgroundEnabledBySelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const current = isMapBackgroundEnabledBySelection(selected);
      fltPlanMapBackgroundEnabledBySelection[key] = !current;
      if (!current){
        const svgState = getMapViewBySelection(selected);
        const webState = getOpenFreeMapViewBySelection(selected);
        if (!isFinite(Number(webState.zoom))){
          const svgZoom = isFinite(Number(svgState.zoom)) ? Number(svgState.zoom) : 1;
          webState.zoom = clamp(6 + Math.log(svgZoom) / Math.log(1.2), 2, 16);
        }
      } else {
        const svgState = getMapViewBySelection(selected);
        const webState = getOpenFreeMapViewBySelection(selected);
        if (isFinite(Number(webState.zoom))){
          const webZoom = Number(webState.zoom);
          svgState.zoom = clamp(Math.pow(1.2, webZoom - 6), 0.6, 4.0);
        }
      }
    }

    function getOpenFreeMapViewBySelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return { centerLon: NaN, centerLat: NaN, zoom: NaN };
      const existing = fltPlanOpenFreeMapViewBySelection[key];
      if (existing && isFinite(Number(existing.centerLon)) && isFinite(Number(existing.centerLat)) && isFinite(Number(existing.zoom))){
        return existing;
      }
      const state = { centerLon: NaN, centerLat: NaN, zoom: NaN };
      fltPlanOpenFreeMapViewBySelection[key] = state;
      return state;
    }

    function registerOpenFreeMapPayload(selected, payload){
      if (!payload || typeof payload !== 'object') return '';
      const selectedKey = getFlightPlanEtaStartKey(selected);
      Object.keys(openFreeMapPayloadById).forEach(function(id){
        const item = openFreeMapPayloadById[id];
        if (!item) return;
        if (String(item.selectedKey || '') === String(selectedKey || '')){
          delete openFreeMapPayloadById[id];
        }
      });
      const id = 'ofm_' + String(openFreeMapPayloadSeq++);
      openFreeMapPayloadById[id] = {
        selectedKey: selectedKey,
        payload: payload,
      };
      return id;
    }

    function getLatestOpenFreeMapPayloadBySelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return null;
      const ids = Object.keys(openFreeMapPayloadById);
      if (!ids.length) return null;
      let bestId = '';
      let bestSeq = -1;
      ids.forEach(function(id){
        const item = openFreeMapPayloadById[id];
        if (!item) return;
        if (String(item.selectedKey || '') !== key) return;
        const seq = Number(String(id).replace('ofm_', ''));
        if (!isFinite(seq)) return;
        if (seq > bestSeq){
          bestSeq = seq;
          bestId = id;
        }
      });
      if (!bestId) return null;
      const entry = openFreeMapPayloadById[bestId];
      return entry && entry.payload ? entry.payload : null;
    }

    function syncOpenFreeMapForSelection(selected, payload){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key || !payload) return false;
      const containerId = String(openFreeMapContainerIdBySelectionKey[key] || '');
      if (!containerId) return false;
      const item = openFreeMapInstancesByContainerId[containerId];
      if (!item || !item.map) return false;

      const map = item.map;
      if (!map || !map.isStyleLoaded || !map.isStyleLoaded()) return false;

      item.payload = payload;

      const bounds = Array.isArray(payload.bounds) ? payload.bounds.slice(0, 4) : null;
      if (bounds && bounds.length === 4) item.bounds = bounds;

      renderOpenFreeMapOverlay(item, payload);
      return true;
    }

    function renderOpenFreeMapOverlay(item, payload){
      if (!item || !item.map || !item.host) return;
      const map = item.map;
      const host = item.host;
      const features = Array.isArray(payload && payload.features) ? payload.features : [];
      const orderedFeatures = features.slice().sort(function(a, b){
        const ak = String((((a || {}).properties || {}).kind) || '').toLowerCase();
        const bk = String((((b || {}).properties || {}).kind) || '').toLowerCase();
        const ap = ak === 'airfield' ? 1 : 0;
        const bp = bk === 'airfield' ? 1 : 0;
        if (ap !== bp) return ap - bp;
        return 0;
      });

      let overlaySvg = item.overlaySvg;
      if (!overlaySvg){
        overlaySvg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        overlaySvg.setAttribute('class', 'fltPlanOpenMapVectorOverlay');
        overlaySvg.setAttribute('width', '100%');
        overlaySvg.setAttribute('height', '100%');
        overlaySvg.style.pointerEvents = 'none';
        host.appendChild(overlaySvg);
        item.overlaySvg = overlaySvg;
      }

      const overlayRoot = overlaySvg;
      if (!orderedFeatures.length){
        return;
      }

      const selectedAssetKey = getMapSelectedAssetKeyBySelection(item.selectedKey);
      const selectedAssetInfo = { x: NaN, y: NaN, props: null };
      function makeNode(name){ return document.createElementNS('http://www.w3.org/2000/svg', name); }
      const renderLayer = makeNode('g');
      overlaySvg = renderLayer;
      const metersPerPixel = (function(){
        try{
          const center = map.getCenter ? map.getCenter() : null;
          const lat = center && isFinite(Number(center.lat)) ? Number(center.lat) : 0;
          const zoom = map.getZoom ? Number(map.getZoom()) : 0;
          const mpp = (156543.03392804097 * Math.cos(lat * (Math.PI / 180))) / Math.pow(2, zoom);
          return (isFinite(mpp) && mpp > 0) ? mpp : NaN;
        }catch(_){
          return NaN;
        }
      })();

      function createAssetSymbol(px, py, props){
        const g = makeNode('g');
        const kind = String((props && props.assetKind) || '').toLowerCase();
        const fill = '#2d8fe3';
        const stroke = '#1f5d93';
        if (kind === 'awacs'){
          const r = makeNode('rect');
          r.setAttribute('x', (px - 8).toFixed(1));
          r.setAttribute('y', (py - 6).toFixed(1));
          r.setAttribute('width', '16');
          r.setAttribute('height', '12');
          r.setAttribute('rx', '1.6');
          r.setAttribute('fill', fill);
          r.setAttribute('stroke', stroke);
          r.setAttribute('stroke-width', '1.3');
          g.appendChild(r);
          const l = makeNode('line');
          l.setAttribute('x1', (px - 6).toFixed(1));
          l.setAttribute('y1', py.toFixed(1));
          l.setAttribute('x2', (px + 6).toFixed(1));
          l.setAttribute('y2', py.toFixed(1));
          l.setAttribute('stroke', stroke);
          l.setAttribute('stroke-width', '1.2');
          g.appendChild(l);
          const c2 = makeNode('circle');
          c2.setAttribute('cx', px.toFixed(1));
          c2.setAttribute('cy', (py - 9).toFixed(1));
          c2.setAttribute('r', '2.4');
          c2.setAttribute('fill', stroke);
          g.appendChild(c2);
        } else if (kind === 'tanker'){
          const p = makeNode('polygon');
          p.setAttribute('points', (px - 8).toFixed(1) + ',' + py.toFixed(1) + ' ' + px.toFixed(1) + ',' + (py - 6).toFixed(1) + ' ' + (px + 8).toFixed(1) + ',' + py.toFixed(1) + ' ' + px.toFixed(1) + ',' + (py + 6).toFixed(1));
          p.setAttribute('fill', fill);
          p.setAttribute('stroke', stroke);
          p.setAttribute('stroke-width', '1.3');
          g.appendChild(p);
        } else if (kind === 'jtac'){
          const p = makeNode('polygon');
          p.setAttribute('points', px.toFixed(1) + ',' + (py - 7).toFixed(1) + ' ' + (px - 7).toFixed(1) + ',' + (py + 7).toFixed(1) + ' ' + (px + 7).toFixed(1) + ',' + (py + 7).toFixed(1));
          p.setAttribute('fill', fill);
          p.setAttribute('stroke', stroke);
          p.setAttribute('stroke-width', '1.3');
          g.appendChild(p);
        } else if (kind === 'rotary'){
          const c = makeNode('circle');
          c.setAttribute('cx', px.toFixed(1));
          c.setAttribute('cy', py.toFixed(1));
          c.setAttribute('r', '6');
          c.setAttribute('fill', fill);
          c.setAttribute('stroke', stroke);
          c.setAttribute('stroke-width', '1.3');
          g.appendChild(c);
          const h = makeNode('line');
          h.setAttribute('x1', (px - 9).toFixed(1));
          h.setAttribute('y1', py.toFixed(1));
          h.setAttribute('x2', (px + 9).toFixed(1));
          h.setAttribute('y2', py.toFixed(1));
          h.setAttribute('stroke', stroke);
          h.setAttribute('stroke-width', '1.2');
          g.appendChild(h);
          const v = makeNode('line');
          v.setAttribute('x1', px.toFixed(1));
          v.setAttribute('y1', (py - 9).toFixed(1));
          v.setAttribute('x2', px.toFixed(1));
          v.setAttribute('y2', (py + 9).toFixed(1));
          v.setAttribute('stroke', stroke);
          v.setAttribute('stroke-width', '1.2');
          g.appendChild(v);
        } else if (kind === 'marker'){
          const c = makeNode('circle');
          c.setAttribute('cx', px.toFixed(1));
          c.setAttribute('cy', py.toFixed(1));
          c.setAttribute('r', '6.5');
          c.setAttribute('fill', '#f7edf8');
          c.setAttribute('stroke', '#8a2f99');
          c.setAttribute('stroke-width', '1.4');
          g.appendChild(c);
          const h = makeNode('line');
          h.setAttribute('x1', (px - 8).toFixed(1));
          h.setAttribute('y1', py.toFixed(1));
          h.setAttribute('x2', (px + 8).toFixed(1));
          h.setAttribute('y2', py.toFixed(1));
          h.setAttribute('stroke', '#8a2f99');
          h.setAttribute('stroke-width', '1.2');
          g.appendChild(h);
          const v = makeNode('line');
          v.setAttribute('x1', px.toFixed(1));
          v.setAttribute('y1', (py - 8).toFixed(1));
          v.setAttribute('x2', px.toFixed(1));
          v.setAttribute('y2', (py + 8).toFixed(1));
          v.setAttribute('stroke', '#8a2f99');
          v.setAttribute('stroke-width', '1.2');
          g.appendChild(v);
        } else if (kind === 'player'){
          const c = makeNode('circle');
          c.setAttribute('cx', px.toFixed(1));
          c.setAttribute('cy', py.toFixed(1));
          c.setAttribute('r', '7.5');
          c.setAttribute('fill', '#f2d76a');
          c.setAttribute('stroke', '#7a6420');
          c.setAttribute('stroke-width', '1.5');
          g.appendChild(c);
        } else {
          const r = makeNode('rect');
          r.setAttribute('x', (px - 7).toFixed(1));
          r.setAttribute('y', (py - 5.5).toFixed(1));
          r.setAttribute('width', '14');
          r.setAttribute('height', '11');
          r.setAttribute('fill', fill);
          r.setAttribute('stroke', stroke);
          r.setAttribute('stroke-width', '1.3');
          g.appendChild(r);
        }
        return g;
      }

      orderedFeatures.forEach(function(feature){
        if (!feature || !feature.geometry || !feature.properties) return;
        const geom = feature.geometry || {};
        const props = feature.properties || {};

        if (geom.type === 'LineString' && Array.isArray(geom.coordinates) && geom.coordinates.length >= 2){
          const linePts = geom.coordinates.map(function(c){
            if (!Array.isArray(c) || c.length < 2) return null;
            const p = map.project([Number(c[0]), Number(c[1])]);
            return p ? (p.x.toFixed(1) + ',' + p.y.toFixed(1)) : null;
          }).filter(function(v){ return !!v; });
          if (linePts.length >= 2){
            const pl = makeNode('polyline');
            pl.setAttribute('points', linePts.join(' '));
            pl.setAttribute('fill', 'none');
            pl.setAttribute('stroke', String((props && props.stroke) || '#3d566e'));
            pl.setAttribute('stroke-width', String(Number((props && props.lineWidth) || 2.2)));
            if (props && props.dashed) pl.setAttribute('stroke-dasharray', '8 6');
            pl.setAttribute('stroke-opacity', '0.92');
            pl.style.pointerEvents = 'none';
            overlaySvg.appendChild(pl);
          }
          return;
        }

        if (geom.type === 'Point' && Array.isArray(geom.coordinates) && geom.coordinates.length >= 2){
          const p = map.project([Number(geom.coordinates[0]), Number(geom.coordinates[1])]);
          if (!p) return;
          const kind = String((props && props.kind) || '').toLowerCase();
          const isAsset = props.group === 'asset' && kind !== 'airfield';
          const isSelectable = !!(props && props.assetKey) && (props.group === 'asset' || kind === 'airfield');
          const isSelected = isSelectable && String(selectedAssetKey || '') === String(props.assetKey || '');
          const markerNode = isAsset
            ? createAssetSymbol(p.x, p.y, props)
            : (function(){
                const fill = String((props && props.fill) || '#3a6ea5');
                const stroke = String((props && props.stroke) || '#244766');
                if (kind === 'destination'){
                  const d = makeNode('polygon');
                  const p1 = p.x.toFixed(1) + ',' + (p.y - 7).toFixed(1);
                  const p2 = (p.x - 7).toFixed(1) + ',' + p.y.toFixed(1);
                  const p3 = p.x.toFixed(1) + ',' + (p.y + 7).toFixed(1);
                  const p4 = (p.x + 7).toFixed(1) + ',' + p.y.toFixed(1);
                  d.setAttribute('points', p1 + ' ' + p2 + ' ' + p3 + ' ' + p4);
                  d.setAttribute('fill', fill);
                  d.setAttribute('stroke', stroke);
                  d.setAttribute('stroke-width', '1.5');
                  d.style.pointerEvents = 'none';
                  return d;
                }
                if (kind === 'airfield'){
                  const gField = makeNode('g');
                  const type = String((props && props.airfieldType) || 'airport').toLowerCase();
                  const military = !!(props && props.airfieldMilitary);
                  const r = 6.5;

                  const ring = makeNode('circle');
                  ring.setAttribute('cx', p.x.toFixed(1));
                  ring.setAttribute('cy', p.y.toFixed(1));
                  ring.setAttribute('r', String(r));
                  ring.setAttribute('fill', 'none');
                  ring.setAttribute('stroke', stroke);
                  ring.setAttribute('stroke-width', '2');
                  gField.appendChild(ring);

                  if (!military){
                    for (let i = 0; i < 6; i++){
                      const a = i * (Math.PI / 3.0);
                      const x1 = p.x + (Math.cos(a) * (r + 0.5));
                      const y1 = p.y + (Math.sin(a) * (r + 0.5));
                      const x2 = p.x + (Math.cos(a) * (r + 2.2));
                      const y2 = p.y + (Math.sin(a) * (r + 2.2));
                      const tick = makeNode('line');
                      tick.setAttribute('x1', x1.toFixed(1));
                      tick.setAttribute('y1', y1.toFixed(1));
                      tick.setAttribute('x2', x2.toFixed(1));
                      tick.setAttribute('y2', y2.toFixed(1));
                      tick.setAttribute('stroke', stroke);
                      tick.setAttribute('stroke-width', '1.2');
                      gField.appendChild(tick);
                    }
                  }

                  if (type === 'heliport'){
                    const tx = makeNode('text');
                    tx.setAttribute('x', p.x.toFixed(1));
                    tx.setAttribute('y', (p.y + 3.8).toFixed(1));
                    tx.setAttribute('text-anchor', 'middle');
                    tx.setAttribute('font-size', '8.8');
                    tx.setAttribute('font-weight', '800');
                    tx.setAttribute('fill', stroke);
                    tx.textContent = 'H';
                    gField.appendChild(tx);
                  } else if (type === 'seaplane'){
                    const tx = makeNode('text');
                    tx.setAttribute('x', p.x.toFixed(1));
                    tx.setAttribute('y', (p.y + 3.8).toFixed(1));
                    tx.setAttribute('text-anchor', 'middle');
                    tx.setAttribute('font-size', '9.0');
                    tx.setAttribute('font-weight', '700');
                    tx.setAttribute('fill', stroke);
                    tx.textContent = '\u2693';
                    gField.appendChild(tx);
                  }

                  gField.style.pointerEvents = 'none';
                  return gField;
                }
                if (kind === 'threat'){
                  const gThreat = makeNode('g');
                  const xh = makeNode('line');
                  xh.setAttribute('x1', (p.x - 7).toFixed(1));
                  xh.setAttribute('y1', p.y.toFixed(1));
                  xh.setAttribute('x2', (p.x + 7).toFixed(1));
                  xh.setAttribute('y2', p.y.toFixed(1));
                  xh.setAttribute('stroke', stroke);
                  xh.setAttribute('stroke-width', '1.6');
                  gThreat.appendChild(xh);
                  const xv = makeNode('line');
                  xv.setAttribute('x1', p.x.toFixed(1));
                  xv.setAttribute('y1', (p.y - 7).toFixed(1));
                  xv.setAttribute('x2', p.x.toFixed(1));
                  xv.setAttribute('y2', (p.y + 7).toFixed(1));
                  xv.setAttribute('stroke', stroke);
                  xv.setAttribute('stroke-width', '1.6');
                  gThreat.appendChild(xv);
                  const radiusMeters = Number(props && props.radiusMeters);
                  const showRing = !!(props && props.ring) && isFinite(radiusMeters) && radiusMeters > 0 && isFinite(metersPerPixel);
                  if (showRing){
                    const ringPx = Math.max(4, radiusMeters / metersPerPixel);
                    const ring = makeNode('circle');
                    ring.setAttribute('cx', p.x.toFixed(1));
                    ring.setAttribute('cy', p.y.toFixed(1));
                    ring.setAttribute('r', ringPx.toFixed(1));
                    ring.setAttribute('fill', 'none');
                    ring.setAttribute('stroke', stroke);
                    ring.setAttribute('stroke-width', '1.4');
                    ring.setAttribute('stroke-dasharray', '7 5');
                    gThreat.appendChild(ring);
                  }
                  gThreat.style.pointerEvents = 'none';
                  return gThreat;
                }
                if (kind === 'bullseye'){
                  const gBull = makeNode('g');
                  [11, 7, 3].forEach(function(r){
                    const cRing = makeNode('circle');
                    cRing.setAttribute('cx', p.x.toFixed(1));
                    cRing.setAttribute('cy', p.y.toFixed(1));
                    cRing.setAttribute('r', String(r));
                    cRing.setAttribute('fill', 'none');
                    cRing.setAttribute('stroke', stroke);
                    cRing.setAttribute('stroke-width', '1.4');
                    gBull.appendChild(cRing);
                  });
                  gBull.style.pointerEvents = 'none';
                  return gBull;
                }
                if (kind === 'cap'){
                  const gCap = makeNode('g');
                  const courseDeg = isFinite(Number(props && props.course)) ? Number(props.course) : 0;
                  const courseRad = courseDeg * (Math.PI / 180);
                  const capLengthMeters = Math.max(4000, isFinite(Number(props && props.lengthMeters)) ? Number(props && props.lengthMeters) : 12000);
                  const capDiameterMeters = Math.max(2000, isFinite(Number(props && props.diameterMeters)) ? Number(props && props.diameterMeters) : 6000);
                  const widthPx = isFinite(metersPerPixel) ? Math.max(16, ((capLengthMeters + capDiameterMeters) / metersPerPixel)) : 40;
                  const heightPx = isFinite(metersPerPixel) ? Math.max(10, (capDiameterMeters / metersPerPixel)) : 18;
                  const radiusPx = heightPx / 2;
                  const turnDir = String((props && props.turnDirection) || '').trim().toUpperCase();
                  const isRightPattern = turnDir.indexOf('RIGHT') >= 0;
                  const angleDeg = (courseDeg - 90) + (isRightPattern ? 0 : 180);
                  const angleRad = angleDeg * (Math.PI / 180);

                  const localAnchorX = isRightPattern ? ((widthPx / 2) - radiusPx) : (-(widthPx / 2) + radiusPx);
                  const localAnchorY = -(heightPx / 2);
                  const rotAnchorX = (localAnchorX * Math.cos(angleRad)) - (localAnchorY * Math.sin(angleRad));
                  const rotAnchorY = (localAnchorX * Math.sin(angleRad)) + (localAnchorY * Math.cos(angleRad));
                  const centerX = p.x - rotAnchorX;
                  const centerY = p.y - rotAnchorY;

                  const rect = makeNode('rect');
                  rect.setAttribute('x', (centerX - (widthPx / 2)).toFixed(1));
                  rect.setAttribute('y', (centerY - (heightPx / 2)).toFixed(1));
                  rect.setAttribute('width', widthPx.toFixed(1));
                  rect.setAttribute('height', heightPx.toFixed(1));
                  rect.setAttribute('rx', radiusPx.toFixed(1));
                  rect.setAttribute('ry', radiusPx.toFixed(1));
                  rect.setAttribute('fill', 'none');
                  rect.setAttribute('stroke', stroke);
                  rect.setAttribute('stroke-width', '2.0');
                  rect.setAttribute('transform', 'rotate(' + angleDeg.toFixed(1) + ' ' + centerX.toFixed(1) + ' ' + centerY.toFixed(1) + ')');
                  gCap.appendChild(rect);

                  const anchor = makeNode('circle');
                  anchor.setAttribute('cx', p.x.toFixed(1));
                  anchor.setAttribute('cy', p.y.toFixed(1));
                  anchor.setAttribute('r', '2.9');
                  anchor.setAttribute('fill', stroke);
                  gCap.appendChild(anchor);

                  const capLabel = String((props && props.label) || '').trim();
                  if (capLabel){
                    const tx = makeNode('text');
                    tx.setAttribute('x', (p.x + (Math.sin(courseRad) * 10)).toFixed(1));
                    tx.setAttribute('y', (p.y - (Math.cos(courseRad) * 10)).toFixed(1));
                    tx.setAttribute('font-size', '10');
                    tx.setAttribute('fill', stroke);
                    tx.setAttribute('font-weight', '700');
                    tx.textContent = capLabel;
                    gCap.appendChild(tx);
                  }

                  gCap.style.pointerEvents = 'none';
                  return gCap;
                }
                if (kind === 'geoline'){
                  const geo = makeNode('circle');
                  geo.setAttribute('cx', p.x.toFixed(1));
                  geo.setAttribute('cy', p.y.toFixed(1));
                  geo.setAttribute('r', '3.8');
                  geo.setAttribute('fill', fill);
                  geo.style.pointerEvents = 'none';
                  return geo;
                }
                const c = makeNode('circle');
                c.setAttribute('cx', p.x.toFixed(1));
                c.setAttribute('cy', p.y.toFixed(1));
                c.setAttribute('r', String(Number((props && props.radius) || 5.2)));
                c.setAttribute('fill', fill);
                c.setAttribute('stroke', stroke);
                c.setAttribute('stroke-width', '1.6');
                c.style.pointerEvents = 'none';
                return c;
              })();

          if (isSelectable && props.assetKey){
            markerNode.style.pointerEvents = 'auto';
            markerNode.style.cursor = 'pointer';
            markerNode.setAttribute('data-asset-key', String(props.assetKey));
            if (isSelected){
              const ring = makeNode('circle');
              ring.setAttribute('cx', p.x.toFixed(1));
              ring.setAttribute('cy', p.y.toFixed(1));
              ring.setAttribute('r', '11');
              ring.setAttribute('fill', 'none');
              ring.setAttribute('stroke', '#c94444');
              ring.setAttribute('stroke-width', '2.4');
              ring.style.pointerEvents = 'none';
              overlaySvg.appendChild(ring);
              selectedAssetInfo.x = p.x;
              selectedAssetInfo.y = p.y;
              selectedAssetInfo.props = props;
            }
          }
          overlaySvg.appendChild(markerNode);

          const label = String(props.label || '').trim();
          if (label){
            const kind = String((props && props.kind) || '').toLowerCase();
            const isAirfieldLabel = kind === 'airfield';
            const text = makeNode('text');
            text.setAttribute('x', (p.x + (isAirfieldLabel ? 11 : 9)).toFixed(1));
            text.setAttribute('y', (p.y + 4).toFixed(1));
            text.setAttribute('font-size', isAirfieldLabel ? '12.5' : '11');
            text.setAttribute('font-weight', '700');
            const labelFill = isAsset
              ? '#1f3550'
              : (isAirfieldLabel
                  ? (props && props.isVfr ? '#1e6b3d' : '#1d4f87')
                  : String((props && props.textColor) || '#1f2e3d'));
            text.setAttribute('fill', labelFill);
            text.setAttribute('stroke', '#ffffff');
            text.setAttribute('stroke-width', '0.6');
            text.style.pointerEvents = 'none';
            text.textContent = label;
            overlaySvg.appendChild(text);
          }

          if (isSelectable && props.assetKey){
            const hit = makeNode('circle');
            hit.setAttribute('cx', p.x.toFixed(1));
            hit.setAttribute('cy', p.y.toFixed(1));
            hit.setAttribute('r', '13');
            hit.setAttribute('fill', '#000000');
            hit.setAttribute('fill-opacity', '0.01');
            hit.style.pointerEvents = 'auto';
            hit.style.cursor = 'pointer';
            hit.setAttribute('data-asset-key', String(props.assetKey));
            overlaySvg.appendChild(hit);
          }
        }
      });

      if (selectedAssetInfo.props){
        const p = selectedAssetInfo.props;
        function formatFreq(v){
          const raw = String(v || '').trim();
          if (!raw) return '';
          let s = raw;
          const dot = s.indexOf('.');
          if (dot >= 0) s = s.substring(0, dot);
          s = s.replace(/[^0-9]/g, '');
          if (!s) return raw;
          s = ('000000000' + s).slice(-9);
          const main = s.substring(0, 3);
          const decRaw = Number(s.substring(3, 6));
          if (!isFinite(decRaw)) return raw;
          const decRounded = Math.round(decRaw / 25.0) * 25;
          const dec = ('000' + String(Math.round(decRounded))).slice(-3);
          return main + '.' + dec;
        }

        const infoLines = [];
        const category = String(p.category || '').trim().toUpperCase();
        const typeText = String(p.typeName || '').trim();
        const freqText = formatFreq(p.frequency);
        const altFreqs = Array.isArray(p.altFrequencies) ? p.altFrequencies : [];
        const allFreqs = [p.frequency].concat(altFreqs);
        const normalizedFreqs = allFreqs
          .map(function(v){ return formatFreq(v); })
          .filter(function(v){ return !!v && isFinite(Number(v)); })
          .map(function(v){ return { text: v, mhz: Number(v) }; });
        const uhf = normalizedFreqs.find(function(f){ return f.mhz >= 225 && f.mhz <= 399.975; });
        const vhf = normalizedFreqs.find(function(f){ return f.mhz >= 30 && f.mhz < 225; });
        const tacanText = String(p.tacan || '').trim();
        const mpText = String(p.mpClientCallsign || '').trim();
        if (typeText) infoLines.push('TYPE: ' + typeText);
        if (category === 'ATC'){
          if (uhf && uhf.text) infoLines.push('UHF: ' + uhf.text);
          if (vhf && vhf.text) infoLines.push('VHF: ' + vhf.text);
        } else if (freqText) infoLines.push('FREQ: ' + freqText);
        if (tacanText) infoLines.push('TACAN: ' + tacanText);
        if (mpText) infoLines.push('MP: ' + mpText);

        if (infoLines.length){
          const fontSize = 10;
          const lineHeight = 12;
          const padX = 5;
          const padY = 4;
          const maxChars = infoLines.reduce(function(max, line){ return Math.max(max, String(line || '').length); }, 0);
          const boxWidth = Math.max(120, Math.min(300, (maxChars * 6.2) + (padX * 2)));
          const boxHeight = (infoLines.length * lineHeight) + (padY * 2);
          let boxX = selectedAssetInfo.x + 12;
          let boxY = selectedAssetInfo.y + 8;
          const hostW = host.clientWidth || 920;
          const hostH = host.clientHeight || 760;
          if ((boxX + boxWidth) > (hostW - 4)) boxX = selectedAssetInfo.x - boxWidth - 12;
          if ((boxY + boxHeight) > (hostH - 4)) boxY = selectedAssetInfo.y - boxHeight - 12;

          const rect = makeNode('rect');
          rect.setAttribute('x', boxX.toFixed(1));
          rect.setAttribute('y', boxY.toFixed(1));
          rect.setAttribute('width', boxWidth.toFixed(1));
          rect.setAttribute('height', boxHeight.toFixed(1));
          rect.setAttribute('rx', '3');
          rect.setAttribute('ry', '3');
          rect.setAttribute('fill', '#f6f9fc');
          rect.setAttribute('stroke', '#6f879f');
          rect.setAttribute('stroke-width', '1.1');
          overlaySvg.appendChild(rect);

          infoLines.forEach(function(line, idx){
            const tx = makeNode('text');
            tx.setAttribute('x', (boxX + padX).toFixed(1));
            tx.setAttribute('y', (boxY + padY + (lineHeight * (idx + 1)) - 2).toFixed(1));
            tx.setAttribute('font-size', String(fontSize));
            tx.setAttribute('fill', '#1f3550');
            tx.setAttribute('font-weight', '700');
            tx.textContent = String(line);
            overlaySvg.appendChild(tx);
          });
        }
      }

      const previousLayer = item.overlayRenderLayer && item.overlayRenderLayer.parentNode === overlayRoot
        ? item.overlayRenderLayer
        : null;
      overlayRoot.appendChild(renderLayer);
      item.overlayRenderLayer = renderLayer;
      if (previousLayer && previousLayer !== renderLayer){
        previousLayer.style.display = 'none';
        if (typeof window !== 'undefined' && typeof window.requestAnimationFrame === 'function'){
          window.requestAnimationFrame(function(){
            if (previousLayer.parentNode === overlayRoot){
              overlayRoot.removeChild(previousLayer);
            }
          });
        } else if (previousLayer.parentNode === overlayRoot){
          overlayRoot.removeChild(previousLayer);
        }
      }

      if (!item.overlayClickBound){
        overlayRoot.addEventListener('click', function(ev){
          let node = ev.target;
          while (node && node !== overlayRoot){
            const assetKey = node.getAttribute ? String(node.getAttribute('data-asset-key') || '') : '';
            if (assetKey){
              const current = getMapSelectedAssetKeyBySelection(item.selectedKey);
              setMapSelectedAssetKeyBySelection(item.selectedKey, current === assetKey ? '' : assetKey);
              if (latestData) render(latestData);
              ev.preventDefault();
              return;
            }
            node = node.parentNode;
          }
        });
        item.overlayClickBound = true;
      }
    }

    function disposeOpenFreeMapInstances(){
      const ids = Object.keys(openFreeMapInstancesByContainerId);
      ids.forEach(function(id){
        const item = openFreeMapInstancesByContainerId[id];
        if (!item || !item.map) return;
        try{ item.map.remove(); }catch(_){ }
      });
      openFreeMapInstancesByContainerId = {};
      openFreeMapContainerIdBySelectionKey = {};
      openFreeMapPayloadById = {};
    }

    function ensureOpenFreeMapRuntime(done){
      if (typeof done !== 'function') return;
      if (openFreeMapRuntimeReady && window.maplibregl){
        done(true);
        return;
      }

      const nowMs = Date.now();
      if (openFreeMapRuntimeFailed && (nowMs - openFreeMapRuntimeLastAttemptMs) > 30000){
        openFreeMapRuntimeFailed = false;
      }

      if (openFreeMapRuntimeFailed){
        done(false);
        return;
      }

      openFreeMapRuntimeWaiters.push(done);
      if (openFreeMapRuntimeLoading) return;

      openFreeMapRuntimeLoading = true;
      openFreeMapRuntimeLastAttemptMs = nowMs;

      function flush(ok){
        const waiters = openFreeMapRuntimeWaiters.slice();
        openFreeMapRuntimeWaiters = [];
        waiters.forEach(function(cb){
          try{ cb(!!ok); }catch(_){ }
        });
      }

      function fail(reason){
        openFreeMapRuntimeReady = false;
        openFreeMapRuntimeLoading = false;
        openFreeMapRuntimeFailed = true;
        openFreeMapRuntimeErrorText = String(reason || 'Map runtime failed to load.');
        flush(false);
      }

      const cssId = 'vaicom-ofm-maplibre-css';
      if (!document.getElementById(cssId)){
        const css = document.createElement('link');
        css.id = cssId;
        css.rel = 'stylesheet';
        css.href = 'https://unpkg.com/maplibre-gl@4.7.1/dist/maplibre-gl.css';
        css.onerror = function(){
          const css2 = document.createElement('link');
          css2.rel = 'stylesheet';
          css2.href = 'https://cdn.jsdelivr.net/npm/maplibre-gl@4.7.1/dist/maplibre-gl.css';
          document.head.appendChild(css2);
        };
        document.head.appendChild(css);
      }

      if (window.maplibregl){
        openFreeMapRuntimeReady = true;
        openFreeMapRuntimeLoading = false;
        openFreeMapRuntimeErrorText = '';
        flush(true);
        return;
      }

      const scriptUrls = [
        'https://unpkg.com/maplibre-gl@4.7.1/dist/maplibre-gl.js',
        'https://cdn.jsdelivr.net/npm/maplibre-gl@4.7.1/dist/maplibre-gl.js'
      ];

      function tryLoad(index){
        if (index >= scriptUrls.length){
          fail('Unable to load MapLibre runtime from CDN.');
          return;
        }
        const script = document.createElement('script');
        script.src = scriptUrls[index];
        script.async = true;
        script.onload = function(){
          openFreeMapRuntimeReady = !!window.maplibregl;
          if (!openFreeMapRuntimeReady){
            tryLoad(index + 1);
            return;
          }
          openFreeMapRuntimeLoading = false;
          openFreeMapRuntimeErrorText = '';
          flush(true);
        };
        script.onerror = function(){
          tryLoad(index + 1);
        };
        document.head.appendChild(script);
      }

      tryLoad(0);
    }

    function handleOpenFreeMapZoomAction(selected, action){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return false;
      const containerId = String(openFreeMapContainerIdBySelectionKey[key] || '');
      if (!containerId) return false;
      const item = openFreeMapInstancesByContainerId[containerId];
      if (!item || !item.map) return false;

      const map = item.map;
      const mode = String(action || '').toLowerCase();
      if (mode === 'in'){
        map.zoomIn({ duration: 0 });
        return true;
      }
      if (mode === 'out'){
        map.zoomOut({ duration: 0 });
        return true;
      }
      if (mode === 'reset'){
        const bounds = item.bounds;
        if (bounds && bounds.length === 4){
          map.fitBounds([[bounds[0], bounds[1]], [bounds[2], bounds[3]]], { padding: 36, duration: 0, maxZoom: 11 });
          return true;
        }
      }
      return false;
    }

    function initializeOpenFreeMapInstances(){
      const hosts = Array.from(document.querySelectorAll('[data-openfreemap-map-id]'));
      if (!hosts.length) return;

      function getOpenFreeMapStyleUrl(){
        return nightModeEnabled
          ? 'https://tiles.openfreemap.org/styles/dark'
          : 'https://tiles.openfreemap.org/styles/liberty';
      }

      ensureOpenFreeMapRuntime(function(ok){
        hosts.forEach(function(host){
          const mapId = String(host.getAttribute('data-openfreemap-map-id') || '');
          if (!mapId) return;
          if (openFreeMapInstancesByContainerId[mapId]) return;

          const wrap = host.closest ? host.closest('[data-openfreemap-wrap]') : null;
          const fallback = wrap ? wrap.querySelector('.fltPlanOpenMapFallback') : null;
          if (fallback) fallback.style.display = 'none';
          const entry = openFreeMapPayloadById[mapId];
          const payload = entry && entry.payload ? entry.payload : null;

          if (!ok || !payload || !isFinite(Number(payload.centerLon)) || !isFinite(Number(payload.centerLat)) || !window.maplibregl){
            if (fallback) fallback.style.display = 'block';
            return;
          }

          host.innerHTML = '';

          try{
            const selectedKey = String((entry && entry.selectedKey) || host.getAttribute('data-openfreemap-selection') || '');
            const bounds = Array.isArray(payload.bounds) ? payload.bounds.slice(0, 4) : null;
            const viewState = getOpenFreeMapViewBySelection(selectedKey);
            const hasStoredView = isFinite(Number(viewState.centerLon)) && isFinite(Number(viewState.centerLat)) && isFinite(Number(viewState.zoom));

            const map = new maplibregl.Map({
              container: host,
              style: getOpenFreeMapStyleUrl(),
              center: hasStoredView
                ? [Number(viewState.centerLon), Number(viewState.centerLat)]
                : [Number(payload.centerLon || 0), Number(payload.centerLat || 0)],
              zoom: hasStoredView ? Number(viewState.zoom) : 6,
              attributionControl: true,
            });

            map.addControl(new maplibregl.NavigationControl({ showCompass: false }), 'top-right');

            map.on('error', function(){
              openFreeMapLastPayloadStatus = 'map-error';
            });

            const item = {
              map: map,
              selectedKey: selectedKey,
              bounds: bounds,
              host: host,
              payload: payload,
            };

            map.on('load', function(){
              renderOpenFreeMapOverlay(item, payload);
              if (!hasStoredView && bounds && bounds.length === 4){
                map.fitBounds([[bounds[0], bounds[1]], [bounds[2], bounds[3]]], { padding: 36, duration: 0, maxZoom: 11 });
              }
              if (fallback) fallback.style.display = 'none';
            });

            map.on('move', function(){
              const currentPayload = item.payload || payload;
              renderOpenFreeMapOverlay(item, currentPayload);
            });

            map.on('moveend', function(){
              const center = map.getCenter();
              if (!center) return;
              const state = getOpenFreeMapViewBySelection(selectedKey);
              state.centerLon = Number(center.lng);
              state.centerLat = Number(center.lat);
              state.zoom = Number(map.getZoom());
            });

            openFreeMapInstancesByContainerId[mapId] = item;
            if (selectedKey) openFreeMapContainerIdBySelectionKey[selectedKey] = mapId;
          }catch(_){
            if (fallback) fallback.style.display = 'block';
          }
        });
      });
    }

    function getMapSelectedAssetKeyBySelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return '';
      return String(fltPlanMapSelectedAssetKeyBySelection[key] || '');
    }

    function setMapSelectedAssetKeyBySelection(selected, assetKey){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      fltPlanMapSelectedAssetKeyBySelection[key] = String(assetKey || '');
    }

    function makeMapAssetSelectionKey(asset){
      if (!asset || typeof asset !== 'object') return '';
      const callsign = String(asset.callsign || '').trim().toUpperCase();
      const name = String(asset.name || '').trim().toUpperCase();
      const category = String(asset.category || '').trim().toUpperCase();
      const x = Number(asset.xNum);
      const y = Number(asset.yNum);
      if (callsign || name){
        return [callsign, name, category].join('|');
      }
      const xKey = isFinite(x) ? String(Math.round(x)) : '';
      const yKey = isFinite(y) ? String(Math.round(y)) : '';
      return ['POS', category, xKey, yKey].join('|');
    }

    function getPlayerMapPoint(data){
      const server = (data && data.Server) || {};
      const x = Number(server.PlayerPosX);
      const y = Number(server.PlayerPosY);
      if (isFinite(x) && isFinite(y)){
        return { xNum: x, yNum: y };
      }
      const assets = getMudMapAssets(data);
      const own = assets.find(function(a){ return String((a && a.category) || '').toUpperCase() === 'PLAYER'; });
      if (own && isFinite(Number(own.xNum)) && isFinite(Number(own.yNum))){
        return { xNum: Number(own.xNum), yNum: Number(own.yNum) };
      }
      return null;
    }

    function computeBraBetweenPoints(fromPoint, toPoint){
      if (!fromPoint || !toPoint) return null;
      const north0 = Number(fromPoint.xNum);
      const east0 = Number(fromPoint.yNum);
      const north1 = Number(toPoint.xNum);
      const east1 = Number(toPoint.yNum);
      if (!isFinite(north0) || !isFinite(east0) || !isFinite(north1) || !isFinite(east1)) return null;
      const dNorth = north1 - north0;
      const dEast = east1 - east0;
      const distMeters = Math.sqrt((dNorth * dNorth) + (dEast * dEast));
      const bearing = normalizeHeadingDeg((Math.atan2(dEast, dNorth) * 180.0 / Math.PI));
      if (!isFinite(bearing) || !isFinite(distMeters)) return null;
      return {
        bearing: formatHeadingDeg(bearing),
        range: String(Math.round(distMeters / 1852.0)),
      };
    }

    function formatMapBraReadout(data, selectedAsset, bullseyePoint){
      if (!selectedAsset) return 'Click a D-Link target to show BRA and B/E readout.';

      const ownship = getPlayerMapPoint(data);
      const target = {
        xNum: Number(selectedAsset.xNum),
        yNum: Number(selectedAsset.yNum),
      };
      const ownBra = computeBraBetweenPoints(ownship, target);

      const bull = (bullseyePoint && isFinite(Number(bullseyePoint.xNum)) && isFinite(Number(bullseyePoint.yNum)))
        ? { xNum: Number(bullseyePoint.xNum), yNum: Number(bullseyePoint.yNum) }
        : null;
      const bullBra = computeBraBetweenPoints(bull, target);

      const altFeet = Math.max(0, Math.round(Number(selectedAsset.altFeet) || 0));
      const left = ownBra
        ? ('BRA ' + ownBra.bearing + '/' + ownBra.range + '/' + String(altFeet))
        : ('BRA ---/--/' + String(altFeet));
      const right = bullBra
        ? ('B/E ' + bullBra.bearing + '/' + bullBra.range + '/' + String(altFeet))
        : ('B/E ---/--/' + String(altFeet));
      return left + '   ' + right;
    }

    function computeLegDistanceNm(prevWp, currWp){
      if (!prevWp || !currWp) return NaN;
      const prevX = Number(prevWp.x);
      const prevY = Number(prevWp.y);
      const currX = Number(currWp.x);
      const currY = Number(currWp.y);
      if (!isFinite(prevX) || !isFinite(prevY) || !isFinite(currX) || !isFinite(currY)) return NaN;
      const dx = currX - prevX;
      const dy = currY - prevY;
      const meters = Math.sqrt((dx * dx) + (dy * dy));
      return meters / 1852.0;
    }

    function estimateLegSpeedKcas(distanceNm, legSeconds, altFeet){
      const dt = Number(legSeconds);
      if (!isFinite(dt) || dt <= 0) return '-';
      const gsKnots = (Number(distanceNm) * 3600.0) / dt;
      if (!isFinite(gsKnots) || gsKnots <= 0) return '-';
      const altitude = Math.max(0, Number(altFeet) || 0);
      const tasToCasFactor = 1.0 + (altitude / 100000.0);
      const estCas = gsKnots / tasToCasFactor;
      if (!isFinite(estCas) || estCas <= 0) return '-';
      return String(Math.round(estCas));
    }

    function formatDistanceNm(distanceNm){
      const d = Number(distanceNm);
      if (!isFinite(d) || d < 0) return '-';
      if (d < 10){
        return d.toFixed(1);
      }
      return String(Math.round(d));
    }

    function normalizeHeadingDeg(deg){
      let d = Number(deg);
      if (!isFinite(d)) return NaN;
      d = ((d % 360) + 360) % 360;
      return d;
    }

    function formatHeadingDeg(deg){
      const d = normalizeHeadingDeg(deg);
      if (!isFinite(d)) return '-';
      let rounded = Math.round(d);
      if (rounded <= 0) rounded = 360;
      if (rounded > 360) rounded = 360;
      return String(rounded).padStart(3, '0');
    }

    function computeTrueHeadingDeg(fromWp, toWp){
      if (!fromWp || !toWp) return NaN;
      const north0 = Number(fromWp.x);
      const east0 = Number(fromWp.y);
      const north1 = Number(toWp.x);
      const east1 = Number(toWp.y);
      if (!isFinite(north0) || !isFinite(east0) || !isFinite(north1) || !isFinite(east1)) return NaN;

      const dNorth = north1 - north0;
      const dEast = east1 - east0;
      if (Math.abs(dNorth) < 0.001 && Math.abs(dEast) < 0.001) return NaN;

      const radians = Math.atan2(dEast, dNorth);
      return normalizeHeadingDeg((radians * 180.0 / Math.PI));
    }

    function getApproxMagVariationDeg(theater){
      const t = String(theater || '').toUpperCase();
      if (t.indexOf('CAUCASUS') >= 0) return 6.0;
      if (t.indexOf('MARIANA') >= 0) return 2.0;
      if (t.indexOf('PERSIAN') >= 0) return 2.0;
      if (t.indexOf('SYRIA') >= 0) return 5.0;
      if (t.indexOf('SINAI') >= 0) return 4.0;
      if (t.indexOf('NEVADA') >= 0) return 12.0;
      if (t.indexOf('NORMANDY') >= 0) return 1.0;
      if (t.indexOf('KOLA') >= 0) return 11.0;
      if (t.indexOf('AFGHAN') >= 0) return 2.0;
      if (t.indexOf('SOUTH ATLANTIC') >= 0) return -12.0;
      return 0.0;
    }

    function isAltTypeAgl(altType){
      const t = String(altType || '').toUpperCase();
      return t.indexOf('AGL') >= 0;
    }

    function formatAltCellHtml(wp){
      const altitude = escapeHtml(String((wp && wp.alt) || '-'));
      if (wp && isAltTypeAgl(wp.altType)){
        return altitude + '<span class=""fltPlanAltTag"">AGL</span>';
      }
      return altitude;
    }

    function getNavlogCoordDisplayMode(selected){
      const state = getFlightPlanPlanState(selected);
      const mode = String(state && state.coordDisplayMode || 'xy').toLowerCase();
      if (mode === 'dms' || mode === 'ddm' || mode === 'mgrs') return mode;
      return 'xy';
    }

    function cycleNavlogCoordDisplayMode(selected){
      const state = getFlightPlanPlanState(selected);
      const order = ['xy', 'dms', 'ddm', 'mgrs'];
      const current = getNavlogCoordDisplayMode(selected);
      const idx = order.indexOf(current);
      state.coordDisplayMode = order[(idx + 1) % order.length];
    }

    function getNavlogCoordinateDisplayText(wp, theatre, mode){
      const north = Number(wp && wp.xNum);
      const east = Number(wp && wp.yNum);
      if (!isFinite(north) || !isFinite(east)) return '- / -';

      const displayMode = String(mode || 'xy').toLowerCase();
      if (displayMode === 'xy'){
        return String(Math.round(north)) + ' / ' + String(Math.round(east));
      }

      const ll = convertDcsXYToLatLon(theatre, north, east);
      if (!ll) return String(Math.round(north)) + ' / ' + String(Math.round(east));

      if (displayMode === 'dms'){
        return formatLatLonDms(ll.lat, ll.lon);
      }
      if (displayMode === 'ddm'){
        return formatLatLonDdm(ll.lat, ll.lon);
      }
      if (displayMode === 'mgrs'){
        return formatLatLonMgrs(ll.lat, ll.lon);
      }

      return String(Math.round(north)) + ' / ' + String(Math.round(east));
    }

    function convertDcsXYToLatLon(theatre, dcsX, dcsY){
      const projection = getMapProjectionByTheatre(theatre);
      if (!projection) return null;

      const easting = Number(dcsY);
      const northing = Number(dcsX);
      if (!isFinite(easting) || !isFinite(northing)) return null;

      try{
        return inverseTransverseMercator(easting, northing, projection);
      }catch(_){
        return null;
      }
    }

    function getMapProjectionByTheatre(theatre){
      const t = String(theatre || '').trim();
      if (!t) return null;
      if (mapProjectionCatalog[t]) return mapProjectionCatalog[t];

      const normalized = t.replace(/[^A-Za-z0-9]/g, '').toUpperCase();
      if (!normalized) return null;

      if (mapProjectionAliases[normalized] && mapProjectionCatalog[mapProjectionAliases[normalized]]){
        return mapProjectionCatalog[mapProjectionAliases[normalized]];
      }

      const keys = Object.keys(mapProjectionCatalog);
      for (let i = 0; i < keys.length; i++){
        const key = keys[i];
        const keyNorm = String(key || '').replace(/[^A-Za-z0-9]/g, '').toUpperCase();
        if (keyNorm === normalized){
          return mapProjectionCatalog[key];
        }
      }

      return null;
    }

    const mapProjectionCatalog = {
      PersianGulf: { centralMeridianDeg: 57, falseEastingMeters: 75755.99999999645, falseNorthingMeters: -2894933.0000000377, scaleFactor: 0.9996 },
      Falklands: { centralMeridianDeg: -57, falseEastingMeters: 147639.99999997593, falseNorthingMeters: 5815417.000000032, scaleFactor: 0.9996 },
      Caucasus: { centralMeridianDeg: 33, falseEastingMeters: -99516.99999997323, falseNorthingMeters: -4998114.999999984, scaleFactor: 0.9996 },
      MarianaIslands: { centralMeridianDeg: 147, falseEastingMeters: 238417.99999989968, falseNorthingMeters: -1491840.000000048, scaleFactor: 0.9996 },
      Nevada: { centralMeridianDeg: -117, falseEastingMeters: -193996.80999964548, falseNorthingMeters: -4410028.063999966, scaleFactor: 0.9996 },
      Normandy: { centralMeridianDeg: -3, falseEastingMeters: -195526.00000000204, falseNorthingMeters: -5484812.999999951, scaleFactor: 0.9996 },
      Syria: { centralMeridianDeg: 39, falseEastingMeters: 282801.00000003993, falseNorthingMeters: -3879865.9999999935, scaleFactor: 0.9996 },
      SinaiMap: { centralMeridianDeg: 33, falseEastingMeters: 169221.9999999585, falseNorthingMeters: -3325312.9999999693, scaleFactor: 0.9996 },
      TheChannel: { centralMeridianDeg: 21, falseEastingMeters: -62702, falseNorthingMeters: -7543624.99999998, scaleFactor: 0.9996 },
      Afghanistan: { centralMeridianDeg: 63, falseEastingMeters: -300150.032879, falseNorthingMeters: -3759656.99243, scaleFactor: 0.9996 },
      Kola: { centralMeridianDeg: 21, falseEastingMeters: -62711, falseNorthingMeters: -7543616, scaleFactor: 0.9996 },
      GermanyCW: { centralMeridianDeg: 21, falseEastingMeters: 35444.045, falseNorthingMeters: -6061632.212, scaleFactor: 0.9996 },
      Iraq: { centralMeridianDeg: 45, falseEastingMeters: 72292, falseNorthingMeters: -3680040, scaleFactor: 0.9996 },
    };

    const mapProjectionAliases = {
      PERSIANGULF: 'PersianGulf',
      MARIANAISLANDS: 'MarianaIslands',
      MARIANAISLAND: 'MarianaIslands',
      NEVADA: 'Nevada',
      NEVADAMAP: 'Nevada',
      NTTR: 'Nevada',
      NEVADATESTANDTRAININGRANGE: 'Nevada',
      SINAI: 'SinaiMap',
      SINAIMAP: 'SinaiMap',
      THECHANNELMAP: 'TheChannel',
      CHANNEL: 'TheChannel',
      GERMANYCOLDWAR: 'GermanyCW',
      GERMANYCW: 'GermanyCW',
      FALKLANDSISLANDS: 'Falklands',
      CAUCASUSMAP: 'Caucasus',
      SYRIAMAP: 'Syria',
      IRAQMAP: 'Iraq',
      KOLAMAP: 'Kola',
      AFGHANISTANMAP: 'Afghanistan'
    };

    function inverseTransverseMercator(easting, northing, projection){
      const semiMajorAxis = 6378137.0;
      const flattening = 1.0 / 298.257223563;
      const eccentricitySquared = flattening * (2.0 - flattening);

      const x = Number(easting) - Number(projection.falseEastingMeters);
      const y = Number(northing) - Number(projection.falseNorthingMeters);
      const scaleFactor = Number(projection.scaleFactor) || 0.9996;

      const ePrimeSquared = eccentricitySquared / (1.0 - eccentricitySquared);
      const m = y / scaleFactor;
      const mu = m / (semiMajorAxis * (1.0
        - (eccentricitySquared / 4.0)
        - (3.0 * Math.pow(eccentricitySquared, 2.0) / 64.0)
        - (5.0 * Math.pow(eccentricitySquared, 3.0) / 256.0)));

      const e1 = (1.0 - Math.sqrt(1.0 - eccentricitySquared)) / (1.0 + Math.sqrt(1.0 - eccentricitySquared));
      const j1 = (3.0 * e1 / 2.0) - (27.0 * Math.pow(e1, 3.0) / 32.0);
      const j2 = (21.0 * Math.pow(e1, 2.0) / 16.0) - (55.0 * Math.pow(e1, 4.0) / 32.0);
      const j3 = (151.0 * Math.pow(e1, 3.0) / 96.0);
      const j4 = (1097.0 * Math.pow(e1, 4.0) / 512.0);

      const fp = mu
        + j1 * Math.sin(2.0 * mu)
        + j2 * Math.sin(4.0 * mu)
        + j3 * Math.sin(6.0 * mu)
        + j4 * Math.sin(8.0 * mu);

      const sinFp = Math.sin(fp);
      const cosFp = Math.cos(fp);
      const tanFp = Math.tan(fp);
      const c1 = ePrimeSquared * Math.pow(cosFp, 2.0);
      const t1 = Math.pow(tanFp, 2.0);
      const n1 = semiMajorAxis / Math.sqrt(1.0 - eccentricitySquared * Math.pow(sinFp, 2.0));
      const r1 = (semiMajorAxis * (1.0 - eccentricitySquared))
        / Math.pow(1.0 - eccentricitySquared * Math.pow(sinFp, 2.0), 1.5);
      const d = x / (n1 * scaleFactor);

      const latRad = fp - (n1 * tanFp / r1)
        * ((Math.pow(d, 2.0) / 2.0)
           - ((5.0 + 3.0 * t1 + 10.0 * c1 - 4.0 * Math.pow(c1, 2.0) - 9.0 * ePrimeSquared) * Math.pow(d, 4.0) / 24.0)
           + ((61.0 + 90.0 * t1 + 298.0 * c1 + 45.0 * Math.pow(t1, 2.0) - 252.0 * ePrimeSquared - 3.0 * Math.pow(c1, 2.0)) * Math.pow(d, 6.0) / 720.0));

      const lon0Rad = toRadians(Number(projection.centralMeridianDeg));
      const lonRad = lon0Rad
        + ((d
            - (1.0 + 2.0 * t1 + c1) * Math.pow(d, 3.0) / 6.0
            + (5.0 - 2.0 * c1 + 28.0 * t1 - 3.0 * Math.pow(c1, 2.0) + 8.0 * ePrimeSquared + 24.0 * Math.pow(t1, 2.0)) * Math.pow(d, 5.0) / 120.0)
           / cosFp);

      return {
        lat: toDegrees(latRad),
        lon: toDegrees(lonRad),
      };
    }

    function toRadians(degrees){
      return Number(degrees) * Math.PI / 180.0;
    }

    function toDegrees(radians){
      return Number(radians) * 180.0 / Math.PI;
    }

    function formatLatLonDms(lat, lon){
      return formatSingleCoordDms(lat, 'N', 'S', 2) + ' / ' + formatSingleCoordDms(lon, 'E', 'W', 3);
    }

    function formatSingleCoordDms(value, positiveHemisphere, negativeHemisphere, degreeWidth){
      const n = Number(value);
      if (!isFinite(n)) return '-';
      const hemi = n >= 0 ? positiveHemisphere : negativeHemisphere;
      const abs = Math.abs(n);
      let degrees = Math.floor(abs);
      let minutesTotal = (abs - degrees) * 60.0;
      let minutes = Math.floor(minutesTotal);
      let seconds = Math.round((minutesTotal - minutes) * 60.0);
      if (seconds >= 60){
        seconds = 0;
        minutes += 1;
      }
      if (minutes >= 60){
        minutes = 0;
        degrees += 1;
      }
      return hemi + String(degrees).padStart(degreeWidth, '0') + '°' + String(minutes).padStart(2, '0') + 'm' + String(seconds).padStart(2, '0') + 's';
    }

    function formatLatLonDdm(lat, lon){
      return formatSingleCoordDdm(lat, 'N', 'S', 2) + ' / ' + formatSingleCoordDdm(lon, 'E', 'W', 3);
    }

    function formatSingleCoordDdm(value, positiveHemisphere, negativeHemisphere, degreeWidth){
      const n = Number(value);
      if (!isFinite(n)) return '-';
      const hemi = n >= 0 ? positiveHemisphere : negativeHemisphere;
      const abs = Math.abs(n);
      let degrees = Math.floor(abs);
      let minutes = (abs - degrees) * 60.0;
      if (minutes >= 59.99995){
        minutes = 0;
        degrees += 1;
      }
      return hemi + String(degrees).padStart(degreeWidth, '0') + '°' + minutes.toFixed(3).padStart(6, '0') + 'm';
    }

    function formatLatLonMgrs(lat, lon){
      const utm = latLonToUtm(lat, lon);
      if (!utm) return 'MGRS N/A';
      const letters = getMgrsLetters(utm.zone, utm.easting, utm.northing, lat);
      if (!letters) return 'MGRS N/A';

      const eastingRemainder = Math.floor(((utm.easting % 100000) + 100000) % 100000);
      const northingRemainder = Math.floor(((utm.northing % 100000) + 100000) % 100000);
      return String(utm.zone) + utm.band + ' ' + letters + ' ' + String(eastingRemainder).padStart(5, '0') + ' ' + String(northingRemainder).padStart(5, '0');
    }

    function latLonToUtm(lat, lon){
      const latitude = Number(lat);
      const longitude = Number(lon);
      if (!isFinite(latitude) || !isFinite(longitude)) return null;
      if (latitude < -80 || latitude > 84) return null;

      const zone = Math.floor((longitude + 180) / 6) + 1;
      const lonOrigin = (zone - 1) * 6 - 180 + 3;
      const k0 = 0.9996;
      const a = 6378137.0;
      const f = 1.0 / 298.257223563;
      const e2 = f * (2 - f);
      const ePrime2 = e2 / (1 - e2);

      const latRad = toRadians(latitude);
      const lonRad = toRadians(longitude);
      const lonOriginRad = toRadians(lonOrigin);

      const n = a / Math.sqrt(1 - e2 * Math.sin(latRad) * Math.sin(latRad));
      const t = Math.tan(latRad) * Math.tan(latRad);
      const c = ePrime2 * Math.cos(latRad) * Math.cos(latRad);
      const A = Math.cos(latRad) * (lonRad - lonOriginRad);

      const m = a * ((1 - e2 / 4 - 3 * Math.pow(e2, 2) / 64 - 5 * Math.pow(e2, 3) / 256) * latRad
        - (3 * e2 / 8 + 3 * Math.pow(e2, 2) / 32 + 45 * Math.pow(e2, 3) / 1024) * Math.sin(2 * latRad)
        + (15 * Math.pow(e2, 2) / 256 + 45 * Math.pow(e2, 3) / 1024) * Math.sin(4 * latRad)
        - (35 * Math.pow(e2, 3) / 3072) * Math.sin(6 * latRad));

      let easting = k0 * n * (A + (1 - t + c) * Math.pow(A, 3) / 6
        + (5 - 18 * t + t * t + 72 * c - 58 * ePrime2) * Math.pow(A, 5) / 120) + 500000.0;

      let northing = k0 * (m + n * Math.tan(latRad) * (Math.pow(A, 2) / 2
        + (5 - t + 9 * c + 4 * c * c) * Math.pow(A, 4) / 24
        + (61 - 58 * t + t * t + 600 * c - 330 * ePrime2) * Math.pow(A, 6) / 720));

      if (latitude < 0){
        northing += 10000000.0;
      }

      easting = Math.min(999999.0, Math.max(0.0, easting));
      northing = Math.max(0.0, northing);

      return {
        zone: zone,
        band: getUtmLatitudeBand(latitude),
        easting: easting,
        northing: northing,
      };
    }

    function getUtmLatitudeBand(lat){
      const bands = 'CDEFGHJKLMNPQRSTUVWX';
      const clamped = Math.max(-80, Math.min(84, Number(lat)));
      const idx = Math.min(bands.length - 1, Math.max(0, Math.floor((clamped + 80) / 8)));
      return bands.charAt(idx);
    }

    function getMgrsLetters(zone, easting, northing, latitude){
      const zoneNum = Number(zone);
      if (!isFinite(zoneNum) || zoneNum < 1 || zoneNum > 60) return '';

      const eSet = (zoneNum - 1) % 3;
      const eSets = ['ABCDEFGH', 'JKLMNPQR', 'STUVWXYZ'];
      const eList = eSets[eSet];

      const eIndex = Math.floor(Number(easting) / 100000);
      if (!isFinite(eIndex) || eIndex < 1 || eIndex > 8) return '';
      const eLetter = eList.charAt(eIndex - 1);

      const northLettersOdd = 'ABCDEFGHJKLMNPQRSTUV';
      const northLettersEven = 'FGHJKLMNPQRSTUVABCDE';
      const nList = (zoneNum % 2 === 0) ? northLettersEven : northLettersOdd;
      const nIndex = Math.floor(Number(northing) / 100000) % 20;
      if (!isFinite(nIndex) || nIndex < 0) return '';
      const nLetter = nList.charAt(nIndex);

      if (!eLetter || !nLetter) return '';
      return eLetter + nLetter;
    }

    function truncateText(value, maxLen){
      const text = String(value || '');
      if (text.length <= maxLen) return text;
      return text.substring(0, Math.max(0, maxLen - 1)) + '…';
    }

    function pickClosestLines(lines, maxRows){
      const arr = Array.isArray(lines) ? lines : [];
      const parsed = arr.map(function(line){
        const text = String(line || '').trim();
        const m = text.match(/\b(\d{1,3})\s*NM\b/i);
        return { text: text, nm: m ? parseInt(m[1], 10) : 9999 };
      });

      parsed.sort(function(a,b){ return a.nm - b.nm; });
      return parsed.slice(0, Math.max(0, maxRows || 0)).map(function(x){ return x.text; });
    }

    function extractFreqAndUnit(line){
      const text = String(line || '').trim();
      if (!text) return null;
      if (/\bMETAR\b/i.test(text) || /^WEATHER:/i.test(text)) return null;

      const fm = text.match(/(\d{2,3}\.\d{1,3}\s*(?:AM|FM)?)/i);
      const freq = fm ? fm[1].replace(/\s+/g, ' ').trim().toUpperCase() : '';
      const icaoMatch = text.match(/\b([A-Z]{4})\b/);

      let unit = icaoMatch ? String(icaoMatch[1] || '').toUpperCase() : '';
      if (!unit) unit = '-';

      if (!freq) return null;
      return { unit: unit, freq: freq };
    }

    function formatFrequenciesBlockHtml(data){
      const atcRaw = pickClosestLines(getMergedList(data && data.Units, 'ATC'), 4);
      const flightRaw = pickClosestLines(getMergedList(data && data.Units, 'FLIGHT'), 4);
      const entries = atcRaw.concat(flightRaw)
        .map(extractFreqAndUnit)
        .filter(function(x){ return !!x; })
        .slice(0, 6);

      const rows = entries.map(function(e){
        return '<tr><td style=""width:48%;"">' + escapeHtml(truncateText(e.unit, 34)) + '</td><td>' + escapeHtml(e.freq) + '</td></tr>';
      });
      if (!rows.length){ rows.push('<tr><td colspan=""2"">No frequency data.</td></tr>'); }

      return '<div class=""fltPlanInfoBlock""><div class=""fltPlanInfoTitle"">FREQUENCIES</div><div class=""fltPlanInfoBody""><table class=""fltPlanInfoTable""><thead><tr><th>UNIT</th><th>UHF/VHF</th></tr></thead><tbody>' + rows.join('') + '</tbody></table></div></div>';
    }

    function formatAssetsBlockHtml(data){
      const tanker = pickClosestLines(getMergedList(data && data.Units, 'TANKER'), 3);
      const awacs = pickClosestLines(getMergedList(data && data.Units, 'AWACS'), 2);
      const jtac = pickClosestLines(getMergedList(data && data.Units, 'JTAC'), 2);
      const all = tanker.concat(awacs).concat(jtac).slice(0, 7);

      const rows = all.map(function(line){
        return '<tr><td>' + escapeHtml(truncateText(line, 88)) + '</td></tr>';
      });
      if (!rows.length){ rows.push('<tr><td>No asset data.</td></tr>'); }

      return '<div class=""fltPlanInfoBlock""><div class=""fltPlanInfoTitle"">ASSETS</div><div class=""fltPlanInfoBody""><table class=""fltPlanInfoTable""><thead><tr><th>CALLSIGN / UNIT / FREQ / TACAN</th></tr></thead><tbody>' + rows.join('') + '</tbody></table></div></div>';
    }

    function formatMetarBlockHtml(data){
      const server = (data && data.Server) || {};
      const metars = server.AtcMetars || {};
      const atcLines = pickClosestLines(getMergedList(data && data.Units, 'ATC'), 8);
      const keys = [];
      const seen = {};

      atcLines.forEach(function(line){
        const k = resolveAtcMetarKey(line, metars);
        if (!k || seen[k]) return;
        seen[k] = true;
        keys.push(k);
      });

      if (keys.length < 4){
        Object.keys(metars).forEach(function(k){
          if (keys.length >= 4) return;
          if (seen[k]) return;
          seen[k] = true;
          keys.push(k);
        });
      }

      const rows = keys.map(function(k){
        const text = String(metars[k] || '').trim();
        return '<tr><td>' + escapeHtml(truncateText(text, 100)) + '</td></tr>';
      });
      if (!rows.length){ rows.push('<tr><td>No METAR data.</td></tr>'); }

      return '<div class=""fltPlanInfoBlock""><div class=""fltPlanInfoTitle"">WX</div><div class=""fltPlanInfoBody""><table class=""fltPlanInfoTable""><thead><tr><th>METAR</th></tr></thead><tbody>' + rows.join('') + '</tbody></table></div></div>';
    }

    function estimateCruiseKcasForAltitude(altFeet){
      const alt = Number(altFeet);
      if (!isFinite(alt)) return 380;
      if (alt > 28000){
        const gs = 0.85 * 661.47;
        const cas = gs / (1.0 + (alt / 100000.0));
        return Math.max(200, Math.round(cas));
      }
      return 380;
    }

    function estimateRepresentativeGroundSpeedKnots(waypoints){
      const rows = Array.isArray(waypoints) ? waypoints : [];
      const samples = [];

      for (let i = 1; i < rows.length; i++){
        const prev = rows[i - 1];
        const curr = rows[i];
        const prevEta = parseEtaToSeconds(prev.eta);
        const currEta = parseEtaToSeconds(curr.eta);
        const legSeconds = (isFinite(currEta) && isFinite(prevEta)) ? (currEta - prevEta) : NaN;
        if (!isFinite(legSeconds) || legSeconds <= 0) continue;

        const legNm = computeLegDistanceNm(prev, curr);
        if (!isFinite(legNm) || legNm <= 0) continue;

        const gs = (legNm * 3600.0) / legSeconds;
        if (isFinite(gs) && gs > 0) samples.push(gs);
      }

      if (!samples.length) return NaN;
      const total = samples.reduce(function(acc, v){ return acc + v; }, 0);
      return total / samples.length;
    }

    function estimateRepresentativeKcas(waypoints){
      const rows = Array.isArray(waypoints) ? waypoints : [];
      const samples = [];
      rows.forEach(function(wp){
        const v = Number(wp && wp.spd);
        if (isFinite(v) && v > 0) samples.push(v);
      });
      if (!samples.length) return 300;
      const total = samples.reduce(function(acc, x){ return acc + x; }, 0);
      return Math.max(120, Math.min(750, total / samples.length));
    }

    function applySpeedAdjustmentsToWaypoints(waypoints, selected){
      const rows = Array.isArray(waypoints) ? waypoints : [];
      if (!rows.length) return rows;
      const state = getFlightPlanPlanState(selected);

      rows.forEach(function(wp){
        const base = Number(wp.spd);
        const adjustment = getWaypointSpeedAdjustment(state, wp.step);
        if (!isFinite(base)){
          wp.spd = '-';
          return;
        }
        wp.spd = String(Math.max(80, Math.round(base + adjustment)));
      });

      return rows;
    }

    function applyLockedTotPlan(waypoints, selected){
      const rows = Array.isArray(waypoints) ? waypoints : [];
      if (!rows.length) return rows;

      const state = getFlightPlanPlanState(selected);
      const lockedStep = stepToKey(state.lockedStep);
      const totSec = Number(state.totSeconds);
      if (!lockedStep || !isFinite(totSec)) return rows;

      const targetIndex = rows.findIndex(function(wp){ return stepToKey(wp.step) === lockedStep; });
      if (targetIndex < 0) return rows;

      const lockedEta = parseEtaToSeconds(rows[targetIndex].etaDisplay || rows[targetIndex].eta);
      if (!isFinite(lockedEta)) return rows;

      const delta = Math.round(totSec - lockedEta);
      if (!isFinite(delta) || delta === 0) return rows;

      for (let i = 0; i < rows.length; i++){
        const curr = rows[i];
        const eta = parseEtaToSeconds(curr.etaDisplay || curr.eta);
        if (isFinite(eta)){
          curr.etaDisplay = formatSecondsToClock(eta + delta);
        }
      }

      const takeoffSec = getTakeoffTimeBySelection(selected);
      if (isFinite(takeoffSec)){
        const startKey = getFlightPlanEtaStartKey(selected);
        if (startKey){
          fltPlanEtaStartBySelection[startKey] = takeoffSec + delta;
        }
      }

      return rows;
    }

    function applyStartLegPlan(startRow, waypoints){
      if (!startRow || !Array.isArray(waypoints) || !waypoints.length) return;

      const first = waypoints[0];
      const wp1Eta = parseEtaToSeconds(first.etaDisplay || first.eta);
      if (!isFinite(wp1Eta)) return;

      const distanceNm = computeLegDistanceNm(startRow, first);
      const representativeGs = estimateRepresentativeGroundSpeedKnots(waypoints);
      if (!isFinite(distanceNm) || distanceNm <= 0 || !isFinite(representativeGs) || representativeGs <= 0) return;

      const legSeconds = Math.max(1, Math.round((distanceNm * 3600.0) / representativeGs));
      startRow.etaDisplay = formatSecondsToClock(wp1Eta - legSeconds);

      const speedCas = estimateLegSpeedKcas(distanceNm, legSeconds, first.altFeet);
      if (speedCas !== '-'){
        startRow.spd = speedCas;
        first.spd = speedCas;
      }
    }

    function getRouteWaypoints(route){
      const r = (route && typeof route === 'object') ? route : {};
      const wpKeys = Object.keys(r)
        .filter(function(k){ return /^\d+$/.test(String(k)); })
        .sort(function(a,b){ return parseInt(a,10) - parseInt(b,10); });

      return wpKeys.map(function(wk){
        const wp = r[wk] || {};
        const typeText = String(wp.type || wp.action || 'WP');
        const altMeters = Number(wp.alt);
        const altFeetRaw = isFinite(altMeters) ? (altMeters * 3.28084) : NaN;
        const altValue = isFinite(altFeetRaw) ? (Math.round(altFeetRaw / 500) * 500) : NaN;
        return {
          step: String(wk),
          type: abbreviateRouteType(typeText),
          typeRaw: typeText,
          name: String(wp.name || ''),
          alt: isFinite(altValue) ? String(altValue) : '-',
          altFeet: altValue,
          altType: String(wp.alt_type || wp.altType || wp.alttype || ''),
          eta: formatEtaSeconds(wp.ETA),
          etaSourceSeconds: Number(wp.ETA),
          x: isFinite(Number(wp.x)) ? String(Math.round(Number(wp.x))) : '-',
          y: isFinite(Number(wp.y)) ? String(Math.round(Number(wp.y))) : '-',
          xNum: Number(wp.x),
          yNum: Number(wp.y)
        };
      });
    }

    function applyEtaPlanToWaypoints(waypoints, selected){
      const rows = Array.isArray(waypoints) ? waypoints : [];
      if (!rows.length) return rows;

      rows.forEach(function(wp){
        wp.spd = String(estimateCruiseKcasForAltitude(wp.altFeet));
      });

      return rows;
    }

    function applyRouteTimeline(rows, selected){
      const list = Array.isArray(rows) ? rows : [];
      if (!list.length) return list;

      const state = getFlightPlanPlanState(selected);
      ensureDirectToStateValid(list, state);
      const skippedSet = getSkippedStepSet(list, state);
      const routeRows = getEffectiveRouteRows(list, state).filter(function(wp){
        const key = stepToKey(wp && wp.step);
        return key && !skippedSet[key];
      });

      const etaMode = hasTakeoffTimeBySelection(selected);
      const baseSeconds = etaMode ? getTakeoffTimeBySelection(selected) : 0;
      let elapsed = 0;

      list[0].etaDisplay = formatSecondsToClock(baseSeconds);

      list.forEach(function(wp, idx){
        if (!wp || idx === 0 || wp.isStart) return;
        wp.etaDisplay = '-';
      });

      let prev = (list[0] && list[0].isStart) ? list[0] : null;
      let routeStartIndex = 0;
      if (!prev && routeRows.length){
        routeRows[0].etaDisplay = formatSecondsToClock(baseSeconds);
        prev = routeRows[0];
        routeStartIndex = 1;
      }

      for (let i = routeStartIndex; i < routeRows.length; i++){
        const curr = routeRows[i];
        const legNm = computeLegDistanceNm(prev, curr);
        const legCas = Number(curr.spd);
        const legAlt = isFinite(Number(curr.altFeet)) ? Number(curr.altFeet) : 0;
        const legGs = isFinite(legCas) && legCas > 0 ? (legCas * (1.0 + (legAlt / 100000.0))) : NaN;
        const legSeconds = (isFinite(legNm) && legNm > 0 && isFinite(legGs) && legGs > 0)
          ? Math.max(1, Math.round((legNm * 3600.0) / legGs))
          : 0;

        elapsed += legSeconds;
        curr.etaDisplay = formatSecondsToClock(baseSeconds + elapsed);
        prev = curr;
      }

      return list;
    }

    function applyHeadingPlan(rows, theater, selected){
      const list = Array.isArray(rows) ? rows : [];
      if (!list.length) return list;

      const state = getFlightPlanPlanState(selected);
      ensureDirectToStateValid(list, state);
      const skippedSet = getSkippedStepSet(list, state);
      const routeRows = getEffectiveRouteRows(list, state).filter(function(wp){
        const key = stepToKey(wp && wp.step);
        return key && !skippedSet[key];
      });

      list.forEach(function(wp, idx){
        if (!wp || idx === 0 || wp.isStart) return;
        wp.hdg = '-';
      });

      if (!routeRows.length) return list;

      const magVar = Number(getApproxMagVariationDeg(theater));

      const includeStart = !!(list[0] && list[0].isStart);
      if (includeStart && routeRows.length > 0){
        const firstHdg = computeTrueHeadingDeg(list[0], routeRows[0]);
        const firstMag = isFinite(firstHdg) ? normalizeHeadingDeg(firstHdg - magVar) : NaN;
        list[0].hdg = formatHeadingDeg(firstMag);
      }

      for (let i = 0; i < routeRows.length; i++){
        const curr = routeRows[i];
        let trueHdg = NaN;

        if (i === 0){
          if (includeStart){
            trueHdg = computeTrueHeadingDeg(list[0], curr);
          } else if (routeRows.length > 1){
            trueHdg = computeTrueHeadingDeg(curr, routeRows[i + 1]);
          }
        } else {
          trueHdg = computeTrueHeadingDeg(routeRows[i - 1], curr);
        }

        const magnetic = isFinite(trueHdg) ? normalizeHeadingDeg(trueHdg - magVar) : NaN;
        curr.hdg = formatHeadingDeg(magnetic);
      }

      return list;
    }

    function changeWaypointAltitude(selected, step, deltaFeet){
      const state = getFlightPlanPlanState(selected);
      if (!state.altAdjustments) state.altAdjustments = {};
      const key = stepToKey(step);
      if (!key) return;
      const current = Number(state.altAdjustments[key]);
      const next = (isFinite(current) ? current : 0) + Number(deltaFeet || 0);
      state.altAdjustments[key] = clamp(next, -40000, 40000);
    }

    function getAltitudeAdjustmentStep(altFeet){
      const alt = Number(altFeet);
      if (!isFinite(alt)) return 500;
      return alt < 1000 ? 100 : 500;
    }

    function changeWaypointAltitudeByDirection(selected, step, direction, currentAltFeet){
      const dir = Number(direction);
      if (!isFinite(dir) || dir === 0) return;
      const stepSize = getAltitudeAdjustmentStep(currentAltFeet);
      const baseAlt = Number(currentAltFeet);
      if (!isFinite(baseAlt)){
        changeWaypointAltitude(selected, step, (dir >= 0 ? 1 : -1) * stepSize);
        return;
      }

      const lower = Math.floor(baseAlt / stepSize) * stepSize;
      const upper = Math.ceil(baseAlt / stepSize) * stepSize;
      const isAligned = Math.abs(baseAlt - Math.round(baseAlt / stepSize) * stepSize) < 0.0001;

      let nextAlt = baseAlt;
      if (dir >= 0){
        nextAlt = isAligned ? (baseAlt + stepSize) : upper;
      } else {
        nextAlt = isAligned ? (baseAlt - stepSize) : lower;
      }

      nextAlt = Math.max(0, Math.round(nextAlt));
      changeWaypointAltitude(selected, step, nextAlt - baseAlt);
    }

    function applyAltitudeAdjustments(rows, selected){
      const list = Array.isArray(rows) ? rows : [];
      const state = getFlightPlanPlanState(selected);
      const map = state.altAdjustments || {};
      list.forEach(function(wp){
        if (!wp || wp.isStart) return;
        const key = stepToKey(wp.step);
        const delta = Number(map[key]);
        if (!isFinite(delta) || delta === 0) return;
        const baseAlt = Number(wp.altFeet);
        if (!isFinite(baseAlt)) return;
        const nextAlt = Math.max(0, baseAlt + delta);
        wp.altFeet = nextAlt;
        wp.alt = String(Math.round(nextAlt));
      });
      return list;
    }

    function applyDistancePlan(rows, selected){
      const list = Array.isArray(rows) ? rows : [];
      if (!list.length) return list;

      const state = getFlightPlanPlanState(selected);
      ensureDirectToStateValid(list, state);
      const skippedSet = getSkippedStepSet(list, state);
      const routeRows = getEffectiveRouteRows(list, state).filter(function(wp){
        const key = stepToKey(wp && wp.step);
        return key && !skippedSet[key];
      });

      list[0].dist = '-';

      list.forEach(function(wp, idx){
        if (!wp || idx === 0 || wp.isStart) return;
        wp.dist = '-';
      });

      let prev = (list[0] && list[0].isStart) ? list[0] : null;
      for (let i = 0; i < routeRows.length; i++){
        const curr = routeRows[i];
        if (!prev){
          curr.dist = '-';
          prev = curr;
          continue;
        }
        curr.dist = formatDistanceNm(computeLegDistanceNm(prev, curr));
        prev = curr;
      }

      return list;
    }

    function setEtaStartNowForSelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      fltPlanEtaStartBySelection[key] = getCurrentFlightPlanClockSeconds();
    }

    function setTakeoffTimeByAnchorFromNow(selected, anchorType){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;

      const anchor = String(anchorType || '').toUpperCase();
      const now = getCurrentFlightPlanClockSeconds();
      const state = getFlightPlanPlanState(selected);
      const marks = getResolvedTimingMarks(selected);

      if (anchor === 'STEP'){
        marks.step = now;
        marks.start = now + stepToStartSeconds;
        marks.taxi = marks.start + startToTaxiSeconds;
        marks.takeoff = marks.taxi + taxiToTakeoffSeconds;
      } else if (anchor === 'START'){
        marks.start = now;
        marks.taxi = now + startToTaxiSeconds;
        marks.takeoff = marks.taxi + taxiToTakeoffSeconds;
      } else if (anchor === 'TAXI'){
        marks.taxi = now;
        marks.takeoff = now + taxiToTakeoffSeconds;
      } else {
        marks.takeoff = now;
      }

      state.timeMarks = {
        step: marks.step,
        start: marks.start,
        taxi: marks.taxi,
        takeoff: marks.takeoff,
      };

      lockStartPositionForSelection(selected, latestData);
      if (isFinite(Number(marks.takeoff))){
        fltPlanEtaStartBySelection[key] = Number(marks.takeoff);
      }
      appendTimingLog(selected, anchor, marks);

      if (anchor === 'TAKEOFF' && isFinite(Number(marks.takeoff)) && !fakeMissionEnabled){
        setMissionClockAnchor(Number(marks.takeoff), Date.now());
      }

      if (fakeMissionEnabled){
        updateFakeOwnshipStartedState();
      }
    }

    function clearTakeoffTimeForSelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const state = getFlightPlanPlanState(selected);
      delete fltPlanEtaStartBySelection[key];
      state.timeMarks = {};
      state.timingLog = [];
      state.ataByStep = {};
      state.speedRecommendations = {};
      state.totPerformanceByStep = {};
      state.lastOwnshipPos = null;
      state.postFlightOpen = false;
    }

    function getFlightPlanStartRow(server, selected){
      const s = server || {};
      const key = getFlightPlanEtaStartKey(selected);
      const state = getFlightPlanPlanState(key);
      const locked = state && state.lockedStart ? state.lockedStart : null;
      const x = Number(locked ? locked.x : s.PlayerPosX);
      const y = Number(locked ? locked.y : s.PlayerPosY);
      const altFeet = Number(locked ? locked.altFeet : s.PlayerAltFeet);

      if (!isFinite(x) || !isFinite(y)) return null;

      return {
        step: '0',
        type: 'ST',
        name: 'CURRENT POS',
        alt: isFinite(altFeet) ? String(Math.round(altFeet)) : '-',
        etaDisplay: '-',
        spd: '-',
        x: String(Math.round(x)),
        y: String(Math.round(y)),
        isStart: true,
      };
    }

    function findDtcWyptObject(value, depth){
      if (depth > 10 || value === null || value === undefined) return null;
      if (Array.isArray(value)){
        for (let i = 0; i < value.length; i++){
          const found = findDtcWyptObject(value[i], depth + 1);
          if (found) return found;
        }
        return null;
      }
      if (typeof value !== 'object') return null;

      if (Array.isArray(value.NAV_PTS)) return value;
      if (value.WYPT && typeof value.WYPT === 'object' && Array.isArray(value.WYPT.NAV_PTS)) return value.WYPT;

      const keys = Object.keys(value);
      for (let i = 0; i < keys.length; i++){
        const found = findDtcWyptObject(value[keys[i]], depth + 1);
        if (found) return found;
      }
      return null;
    }

    function isDtcPrimaryRouteSteerpoint(stepNum){
      const n = Number(stepNum);
      return isFinite(n) && n >= 1 && n <= 25;
    }

    function getF14NavRoutes(root){
      if (!root || typeof root !== 'object') return [];
      const nav = Array.isArray(root.NAV) ? root.NAV : [];
      return nav.filter(function(route){
        if (!route || typeof route !== 'object') return false;
        return Array.isArray(route.waypoints)
          || Array.isArray(route.additional_points)
          || Array.isArray(route.lines);
      });
    }

    function getF14RouteSlots(root){
      const routes = getF14NavRoutes(root);
      const slots = routes.slice(0, 12).map(function(route, idx){
        return {
          key: 'R' + String(idx + 1),
          route: route
        };
      });
      return slots;
    }

    function getF14WaypointTypeInfo(rawName){
      const original = String(rawName || '').trim();
      const upper = original.toUpperCase();
      const result = {
        name: original,
        typeRaw: 'WP',
        isBullseye: false,
      };

      if (!upper) return result;

      if (isBullseyeText(upper)){
        result.typeRaw = 'BULL';
        result.isBullseye = true;
      }

      function markAndTrim(regex, typeRaw, extra){
        const match = upper.match(regex);
        if (!match) return false;
        result.typeRaw = typeRaw;
        if (extra && typeof extra === 'object'){
          Object.keys(extra).forEach(function(k){ result[k] = extra[k]; });
        }
        const trimmed = original.substring(0, match.index).trim();
        if (trimmed) result.name = trimmed;
        return true;
      }

      if (markAndTrim(/XFP$/i, 'FP')) return result;
      if (markAndTrim(/XIP$/i, 'IP')) return result;
      if (markAndTrim(/XST$/i, 'TGT')) return result;
      if (markAndTrim(/XDP$/i, 'DP')) return result;
      if (markAndTrim(/XHA$/i, 'HA')) return result;
      if (markAndTrim(/XHB$/i, 'HOME')) return result;
      if (markAndTrim(/X(?:\d{0,2})?B$/i, 'BULL', { isBullseye: true })) return result;
      if (markAndTrim(/X(?:\d{0,2})?D$/i, 'DEST')) return result;
      if (markAndTrim(/X(?:\d{0,2})?L$/i, 'LANTIRN')) return result;

      const priorityMatch = upper.match(/X([1-7])$/i);
      if (priorityMatch){
        const level = Number(priorityMatch[1]);
        result.typeRaw = (level >= 1 && level <= 3) ? ('PRIO ' + String(level)) : 'WP';
        const trimmed = original.substring(0, priorityMatch.index).trim();
        if (trimmed) result.name = trimmed;
      }

      return result;
    }

    function getDtcWaypoints(root){
      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];
      const navRoute = Array.isArray(wypt.NAV_ROUTE) ? wypt.NAV_ROUTE : [];
      const primaryRoute = (navRoute.length && navRoute[0] && typeof navRoute[0] === 'object') ? navRoute[0] : {};
      const hasExplicitNavPts = Array.isArray(wypt.NAV_PTS);

      const routeById = {};
      Object.keys(primaryRoute).forEach(function(k){
        const point = primaryRoute[k];
        if (!point || typeof point !== 'object') return;
        routeById[String(k).toUpperCase()] = point;
      });

      if (hasExplicitNavPts){
        if (!navPts.length) return [];

        return navPts.slice(0, 200).map(function(p, idx){
          const point = (p && typeof p === 'object') ? p : {};
          const id = String(point.id || ('STPT' + String(idx + 1))).trim();
          const routePoint = routeById[id.toUpperCase()] || {};

          const etaNum = isFinite(Number(routePoint.ETA)) ? Number(routePoint.ETA) : (isFinite(Number(point.ETA)) ? Number(point.ETA) : Number(point.TOS));
          const altNum = isFinite(Number(routePoint.alt)) ? Number(routePoint.alt) : (isFinite(Number(point.alt)) ? Number(point.alt) : (isFinite(Number(point.routeAltitude)) ? Number(point.routeAltitude) : Number(point.altitude)));
          const xNum = isFinite(Number(routePoint.x))
            ? Number(routePoint.x)
            : (isFinite(Number(point.x)) ? Number(point.x) : Number(point.posX));
          const yNum = isFinite(Number(routePoint.y))
            ? Number(routePoint.y)
            : (isFinite(Number(point.y)) ? Number(point.y) : Number(point.posY));
          const stepNum = isFinite(Number(point.wypt_num)) ? Math.round(Number(point.wypt_num)) : (isFinite(Number(point.number)) ? Math.round(Number(point.number)) : (idx + 1));
          if (!isDtcPrimaryRouteSteerpoint(stepNum)) return null;
          const speed = isFinite(Number(routePoint.speed)) ? Math.round(Number(routePoint.speed)) : (isFinite(Number(point.speed)) ? Math.round(Number(point.speed)) : NaN);
          const isTarget = !!routePoint.TGT;

          const noteText = String(point.note || point.text_note || '').trim();

          return {
            step: String(stepNum),
            type: abbreviateRouteType(isTarget ? 'TGT' : 'WP'),
            typeRaw: isTarget ? 'TGT' : 'WP',
            name: noteText || String(point.name || point.wp || point.waypoint || point.label || ''),
            alt: isFinite(altNum) ? String(Math.round(altNum)) : '-',
            altFeet: altNum,
            altType: String(point.altitudeType || point.alt_type || point.altType || point.alttype || ''),
            eta: formatEtaSeconds(etaNum),
            etaSourceSeconds: etaNum,
            spd: isFinite(speed) ? String(speed) : '-',
            x: isFinite(xNum) ? String(Math.round(xNum)) : '-',
            y: isFinite(yNum) ? String(Math.round(yNum)) : '-',
            xNum: xNum,
            yNum: yNum
          };
        }).filter(function(wp){ return !!wp; });
      }

      const f14Slots = getF14RouteSlots(root);
      if (f14Slots.length){
        const mapped = [];
        f14Slots.forEach(function(slot){
          const route = (slot && slot.route && typeof slot.route === 'object') ? slot.route : {};
          const routeWaypoints = Array.isArray(route.waypoints) ? route.waypoints : [];
          routeWaypoints.slice(0, 200).forEach(function(point, idx){
            const wp = (point && typeof point === 'object') ? point : {};
            const nameInfo = getF14WaypointTypeInfo(wp.name);
            const stepNum = isFinite(Number(wp.wypt_num))
              ? Math.round(Number(wp.wypt_num))
              : (isFinite(Number(wp.number)) ? Math.round(Number(wp.number)) : (idx + 1));
            if (!isDtcPrimaryRouteSteerpoint(stepNum)) return;

            const etaNum = isFinite(Number(wp.ETA)) ? Number(wp.ETA)
              : (isFinite(Number(wp.tot)) ? Number(wp.tot) : NaN);
            const altNum = isFinite(Number(wp.alt)) ? Number(wp.alt)
              : (isFinite(Number(wp.elev)) ? Number(wp.elev) : Number(wp.altitude));
            const xNum = isFinite(Number(wp.x)) ? Number(wp.x) : Number(wp.posX);
            const yNum = isFinite(Number(wp.y)) ? Number(wp.y) : Number(wp.posY);
            const spdNum = isFinite(Number(wp.spd)) ? Number(wp.spd) : Number(wp.speed);

            mapped.push({
              step: String(stepNum),
              type: abbreviateRouteType(nameInfo.typeRaw || 'WP'),
              typeRaw: String(nameInfo.typeRaw || 'WP'),
              name: nameInfo.name || String(wp.name || ''),
              alt: isFinite(altNum) ? String(Math.round(altNum)) : '-',
              altFeet: altNum,
              altType: String(wp.altitudeType || wp.alt_type || wp.altType || wp.alttype || ''),
              eta: formatEtaSeconds(etaNum),
              etaSourceSeconds: etaNum,
              spd: isFinite(spdNum) ? String(Math.round(spdNum)) : '-',
              x: isFinite(xNum) ? String(Math.round(xNum)) : '-',
              y: isFinite(yNum) ? String(Math.round(yNum)) : '-',
              xNum: xNum,
              yNum: yNum,
              __routeKey: String((slot && slot.key) || 'R1')
            });
          });
        });

        if (mapped.length){
          return mapped
            .sort(function(a, b){
              const ra = String((a && a.__routeKey) || 'R1');
              const rb = String((b && b.__routeKey) || 'R1');
              if (ra !== rb) return ra.localeCompare(rb);
              return Number(a && a.step) - Number(b && b.step);
            })
            .slice(0, 200);
        }
      }

      const candidates = [];
      collectNavPoints(root, candidates, 0);
      const filtered = candidates.filter(function(p){
        const point = (p && typeof p === 'object') ? p : {};
        const hasXY = isFinite(Number(point.x)) && isFinite(Number(point.y));
        const hasPointMeta = isFinite(Number(point.wypt_num)) || !!point.id || !!point.name || !!point.wp || !!point.waypoint || !!point.alt || !!point.altitude;
        const looksLikeComm = point.freq !== undefined || point.frequency !== undefined || point.Channel !== undefined || point.channel !== undefined || point.modulation !== undefined;
        return hasXY && hasPointMeta && !looksLikeComm;
      });

      filtered.sort(function(a, b){
        const aw = Number(a && a.wypt_num);
        const bw = Number(b && b.wypt_num);
        if (isFinite(aw) && isFinite(bw) && aw !== bw) return aw - bw;
        return 0;
      });

      return filtered.slice(0, 200).map(function(point, idx){
        const etaNum = isFinite(Number(point.ETA)) ? Number(point.ETA) : (isFinite(Number(point.eta)) ? Number(point.eta) : Number(point.TOS));
        const altNum = isFinite(Number(point.alt)) ? Number(point.alt) : (isFinite(Number(point.routeAltitude)) ? Number(point.routeAltitude) : Number(point.altitude));
        const xNum = isFinite(Number(point.x)) ? Number(point.x) : Number(point.posX);
        const yNum = isFinite(Number(point.y)) ? Number(point.y) : Number(point.posY);
        const stepNum = isFinite(Number(point.wypt_num)) ? Math.round(Number(point.wypt_num)) : (isFinite(Number(point.number)) ? Math.round(Number(point.number)) : (idx + 1));
        if (!isDtcPrimaryRouteSteerpoint(stepNum)) return null;
        const noteText = String(point.note || point.text_note || '').trim();

        return {
          step: String(stepNum),
          type: abbreviateRouteType(point.type || point.action || 'WP'),
          typeRaw: String(point.type || point.action || 'WP'),
          name: noteText || String(point.name || point.wp || point.waypoint || point.label || ''),
          alt: isFinite(altNum) ? String(Math.round(altNum)) : '-',
          altFeet: altNum,
          altType: String(point.altitudeType || point.alt_type || point.altType || point.alttype || ''),
          eta: formatEtaSeconds(etaNum),
          etaSourceSeconds: etaNum,
          spd: isFinite(Number(point.speed)) ? String(Math.round(Number(point.speed))) : '-',
          x: isFinite(xNum) ? String(Math.round(xNum)) : '-',
          y: isFinite(yNum) ? String(Math.round(yNum)) : '-',
          xNum: xNum,
          yNum: yNum
        };
      }).filter(function(wp){ return !!wp; });
    }

    function formatDtcCmdsBlockHtml(root){
      function collectCmdsSettings(value, depth, found){
        if (depth > 10 || value === null || value === undefined) return;
        if (Array.isArray(value)){
          value.forEach(function(v){ collectCmdsSettings(v, depth + 1, found); });
          return;
        }
        if (typeof value !== 'object') return;

        Object.keys(value).forEach(function(k){
          const v = value[k];
          if (/^CMDSProgramSettings$/i.test(String(k)) && v && typeof v === 'object'){
            found.push(v);
          }
          if (v && typeof v === 'object') collectCmdsSettings(v, depth + 1, found);
        });
      }

      const foundCmds = [];
      collectCmdsSettings(root, 0, foundCmds);
      const cmds = foundCmds
        .sort(function(a, b){
          function score(obj){
            return Object.keys(obj || {}).filter(function(k){ return /^(AUTO_?\d+|MAN_?\d+|BYP|PROG_?\d+)$/i.test(String(k)); }).length;
          }
          return score(b) - score(a);
        })[0];
      if (!cmds || typeof cmds !== 'object') return '';

      const preferredOrder = ['AUTO_1','AUTO1','AUTO_2','AUTO2','AUTO_3','AUTO3','BYP','MAN_1','MAN1','MAN_2','MAN2','MAN_3','MAN3','MAN_4','MAN4','MAN_5','MAN5','MAN_6','MAN6'];
      const keys = [];
      preferredOrder.forEach(function(k){
        if (cmds[k] && typeof cmds[k] === 'object') keys.push(k);
      });

      Object.keys(cmds).forEach(function(k){
        if (keys.indexOf(k) >= 0) return;
        if (!cmds[k] || typeof cmds[k] !== 'object') return;
        keys.push(k);
      });

      if (!keys.length) return '';

      function modeShortLabel(k){
        const key = String(k || '').toUpperCase();
        const auto = key.match(/^AUTO_?(\d+)$/);
        if (auto) return 'A' + auto[1];
        const man = key.match(/^MAN_?(\d+)$/);
        if (man) return 'M' + man[1];
        if (key === 'BYP') return 'BYP';
        return key;
      }

      function num(v, fallback){
        const n = Number(v);
        return isFinite(n) ? n : fallback;
      }

      const rowByKey = {};
      keys.forEach(function(k){
        const p = cmds[k] || {};
        const chaff = (p.Chaff && typeof p.Chaff === 'object') ? p.Chaff : {};
        const flare = (p.Flare && typeof p.Flare === 'object') ? p.Flare : {};
        const other1 = (p.Other1 && typeof p.Other1 === 'object') ? p.Other1 : {};
        const other2 = (p.Other2 && typeof p.Other2 === 'object') ? p.Other2 : {};

        const cInt = num(chaff.Interval, num(chaff.SalvoInterval, num(chaff.BurstInterval, 0)));
        const cQty = num(chaff.Quantity, num(chaff.BurstQuantity, num(chaff.SalvoQuantity, 0)));
        const cRpt = num(chaff.Repeat, num(chaff.SalvoQuantity, 0));
        const fQty = num(flare.Quantity, num(flare.BurstQuantity, num(flare.SalvoQuantity, 0)));
        const o1Qty = num(other1.Quantity, num(other1.BurstQuantity, num(other1.SalvoQuantity, 0)));
        const o2Qty = num(other2.Quantity, num(other2.BurstQuantity, num(other2.SalvoQuantity, 0)));

        const modeLabel = modeShortLabel(k);
        const line = '<strong>' + modeLabel + '</strong>'
          + ' <strong>C</strong> Int' + cInt
          + ' Qty' + cQty
          + ' Rpt' + cRpt
          + ' <strong>F</strong> Qty' + fQty
          + ' <strong>O1</strong>' + (o1Qty > 0 ? (' Qty' + o1Qty) : '')
          + ' <strong>O2</strong>' + (o2Qty > 0 ? (' Qty' + o2Qty) : '');

        rowByKey[String(k).toUpperCase()] = line;
      });

      const leftModes = ['AUTO_1', 'AUTO1', 'AUTO_2', 'AUTO2', 'AUTO_3', 'AUTO3', 'BYP'];
      const rightModes = ['MAN_1', 'MAN1', 'MAN_2', 'MAN2', 'MAN_3', 'MAN3', 'MAN_4', 'MAN4', 'MAN_5', 'MAN5', 'MAN_6', 'MAN6'];
      const leftRows = leftModes
        .map(function(k){ return rowByKey[String(k).toUpperCase()]; })
        .filter(function(x){ return !!x; });
      const rightRows = rightModes
        .map(function(k){ return rowByKey[String(k).toUpperCase()]; })
        .filter(function(x){ return !!x; });
      if (!leftRows.length && !rightRows.length){
        const fallbackRows = keys
          .map(function(k){ return rowByKey[String(k).toUpperCase()]; })
          .filter(function(x){ return !!x; });
        const split = Math.ceil(fallbackRows.length / 2);
        for (let i = 0; i < split; i++){
          leftRows.push(fallbackRows[i] || '');
          rightRows.push(fallbackRows[i + split] || '');
        }
      }

      const maxRows = Math.max(leftRows.length, rightRows.length);

      const rows = [];
      for (let i = 0; i < maxRows; i++){
        const left = leftRows[i] || '';
        const right = rightRows[i] || '';
        rows.push('<tr><td style=""width:50%;"">' + left + '</td><td>' + right + '</td></tr>');
      }

      return '<div class=""fltPlanInfoBlock""><div class=""fltPlanInfoTitle"">CMDS</div><div class=""fltPlanInfoBody""><table class=""fltPlanInfoTable""><tbody>' + rows.join('') + '</tbody></table></div></div>';
    }

    function formatDtcCommPanelHtml(root, emptyWhenMissing){
      const commRoot = findFirstObjectByKeyPattern(root, /^COMM$/i, 0) || {};

      function formatCommFrequency(value){
        const n = Number(value);
        if (!isFinite(n)) return '-';
        return n.toFixed(3);
      }

      function getCommRows(commObj){
        if (!commObj || typeof commObj !== 'object') return [];
        const rows = [];
        Object.keys(commObj).forEach(function(k){
          const o = commObj[k];
          if (!o || typeof o !== 'object') return;
          const fq = Number(o.frequency || o.Frequency || o.freq);
          if (!isFinite(fq)) return;
          const mod = Number(o.modulation);

          const key = String(k || '');
          const chMatch = key.match(/^Channel_(\d+)$/i);
          const chNum = chMatch ? parseInt(chMatch[1], 10) : NaN;
          const hasCustomName = String(o.name || '').trim() !== '';
          const rawLabel = String(o.name || key.replace(/^Channel_/i, 'CH '));
          const label = rawLabel.replace(/\s+/g, '');
          rows.push({
            label: label,
            freq: formatCommFrequency(fq),
            modulation: isFinite(mod) ? mod : NaN,
            hasCustomName: hasCustomName,
            sortGroup: isFinite(chNum) ? 0 : 1,
            sortValue: isFinite(chNum) ? chNum : 999,
          });
        });

        rows.sort(function(a, b){
          if (a.sortGroup !== b.sortGroup) return a.sortGroup - b.sortGroup;
          if (a.sortValue !== b.sortValue) return a.sortValue - b.sortValue;
          return String(a.label).localeCompare(String(b.label));
        });

        return rows;
      }

      function looksLikeDefaultMirrorRows(rows){
        if (!Array.isArray(rows) || rows.length !== 20) return false;

        const seen = {};
        for (let i = 0; i < rows.length; i++){
          const r = rows[i] || {};
          const m = String(r.label || '').match(/^CH\s*(\d{1,2})$/i);
          if (!m) return false;
          const ch = Number(m[1]);
          if (!isFinite(ch) || ch < 1 || ch > 20) return false;
          seen[ch] = true;

          const fq = Number(r.freq);
          if (!isFinite(fq)) return false;
          if (Math.abs(fq - Math.round(fq)) > 0.0001) return false;

          const mod = Number(r.modulation);
          if (isFinite(mod) && Math.round(mod) !== 1) return false;

          if (r.hasCustomName) return false;
        }

        for (let ch = 1; ch <= 20; ch++){
          if (!seen[ch]) return false;
        }

        return true;
      }

      const comm1 = commRoot.COMM1 || commRoot.Comm1 || commRoot.COMM_1 || {};
      const comm2 = commRoot.COMM2 || commRoot.Comm2 || commRoot.COMM_2 || {};
      const rows1 = getCommRows(comm1);
      const rows2 = getCommRows(comm2);
      const comm1Guard = !!comm1.Guard;
      const comm2Guard = !!comm2.Guard;
      const mirror1 = !!commRoot.mirror_COMM1;
      const mirror2 = !!commRoot.mirror_COMM2;

      const looksLikeDefaultMirrors = mirror1
        && mirror2
        && looksLikeDefaultMirrorRows(rows1)
        && looksLikeDefaultMirrorRows(rows2);

      if (looksLikeDefaultMirrors){
        return emptyWhenMissing
          ? ''
          : '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">COMMS</div><div class=""fltPlanPage2Body"">No comm data.</div></div>';
      }

      const maxRows = Math.max(rows1.length, rows2.length);
      if (!maxRows){
        return emptyWhenMissing
          ? ''
          : '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">COMMS</div><div class=""fltPlanPage2Body"">No comm data.</div></div>';
      }

      const bodyRows = [];
      for (let i = 0; i < maxRows; i++){
        const a = rows1[i];
        const b = rows2[i];
        bodyRows.push('<tr><td>' + (a ? (escapeHtml(a.label) + ' ' + escapeHtml(String(a.freq))) : '') + '</td><td>' + (b ? (escapeHtml(b.label) + ' ' + escapeHtml(String(b.freq))) : '') + '</td></tr>');
      }

      return '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">COMMS</div><div class=""fltPlanPage2Body""><table class=""fltPlanPage2Table""><thead><tr><th>COMM 1' + (comm1Guard ? ' (G)' : '') + '</th><th>COMM 2' + (comm2Guard ? ' (G)' : '') + '</th></tr></thead><tbody>' + bodyRows.join('') + '</tbody></table></div></div>';
    }

    function formatRuntimeCommPanelHtml(data){
      const server = (data && data.Server) || {};
      const radios = Array.isArray(server.Radios) ? server.Radios : [];
      const diagnostics = (server && server.Diagnostics && typeof server.Diagnostics === 'object') ? server.Diagnostics : {};
      const active = radios.filter(function(r){
        return !!(r && typeof r === 'object' && !r.intercom);
      });

      if (!active.length){
        return '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">COMMS</div><div class=""fltPlanPage2Body"">No runtime comm data.</div></div>';
      }

      function fmtFreq(value){
        const n = Number(value);
        if (isFinite(n) && n > 0){
          let mhz = n;
          if (n >= 10000000){
            mhz = n / 1000000.0;
          } else if (n >= 100000){
            mhz = n / 1000.0;
          }

          return mhz.toFixed(3);
        }
        const s = String(value || '').trim();
        return s || '-';
      }

      function fmtRow(r){
        const fq = fmtFreq(r.frequency);
        const mod = String(r.modulation || '').trim().toUpperCase() || (r.FM && !r.AM ? 'FM' : 'AM');
        const state = r.on ? 'ON' : 'OFF';
        return state + ' ' + fq + ' ' + mod;
      }

      function parseMissionRadioChannelRow(line){
        const text = String(line || '').trim();
        if (!text) return null;
        const map = {};
        text.split('|').forEach(function(p){
          const idx = p.indexOf('=');
          if (idx <= 0) return;
          const key = String(p.substring(0, idx)).trim().toLowerCase();
          const value = String(p.substring(idx + 1)).trim();
          if (!key) return;
          map[key] = value;
        });

        const radioNum = Number(map.radio);
        const channelNum = Number(map.ch);
        if (!isFinite(radioNum) || !isFinite(channelNum)) return null;

        const freqText = fmtFreq(map.freq);
        const nameText = String(map.name || '').trim();
        return {
          radio: Math.round(radioNum),
          channel: Math.round(channelNum),
          label: 'CH ' + String(Math.round(channelNum)).padStart(2, '0'),
          freq: freqText,
          name: nameText
        };
      }

      const missionChannelsRaw = Array.isArray(diagnostics.playerMissionRadioChannels) && diagnostics.playerMissionRadioChannels.length
        ? diagnostics.playerMissionRadioChannels
        : (Array.isArray(diagnostics.missionRadioChannels) ? diagnostics.missionRadioChannels : []);
      const missionChannels = missionChannelsRaw
        .map(parseMissionRadioChannelRow)
        .filter(function(x){ return !!x; });

      const dedupedMissionChannels = [];
      const seenMissionChannels = {};
      missionChannels.forEach(function(ch){
        const key = String(ch.radio) + '|' + String(ch.channel) + '|' + String(ch.freq);
        if (seenMissionChannels[key]) return;
        seenMissionChannels[key] = true;
        dedupedMissionChannels.push(ch);
      });

      if (dedupedMissionChannels.length){
        const comm1 = dedupedMissionChannels.filter(function(x){ return x.radio === 1; });
        const comm2 = dedupedMissionChannels.filter(function(x){ return x.radio === 2; });

        function sortMissionRows(list){
          list.sort(function(a, b){
            if (a.channel !== b.channel) return a.channel - b.channel;
            return String(a.name).localeCompare(String(b.name));
          });
        }
        sortMissionRows(comm1);
        sortMissionRows(comm2);

        const maxRows = Math.max(comm1.length, comm2.length);
        const bodyRows = [];
        for (let i = 0; i < maxRows; i++){
          const a = comm1[i];
          const b = comm2[i];
          const left = a ? (a.label + ' ' + a.freq + (a.name ? (' ' + a.name) : '')) : '';
          const right = b ? (b.label + ' ' + b.freq + (b.name ? (' ' + b.name) : '')) : '';
          bodyRows.push('<tr><td>' + escapeHtml(left) + '</td><td>' + escapeHtml(right) + '</td></tr>');
        }

        return '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">COMMS</div><div class=""fltPlanPage2Body""><table class=""fltPlanPage2Table""><thead><tr><th>COMM 1 PRESETS</th><th>COMM 2 PRESETS</th></tr></thead><tbody>' + bodyRows.join('') + '</tbody></table></div></div>';
      }

      const comm1 = [];
      const comm2 = [];
      active.forEach(function(r){
        const name = String(r.displayName || '').toUpperCase();
        if (/\b1\b|COMM\s*1|UHF/.test(name)){
          comm1.push(r);
        } else if (/\b2\b|COMM\s*2|VHF/.test(name)){
          comm2.push(r);
        } else if (comm1.length <= comm2.length){
          comm1.push(r);
        } else {
          comm2.push(r);
        }
      });

      const maxRows = Math.max(comm1.length, comm2.length);
      const bodyRows = [];
      for (let i = 0; i < maxRows; i++){
        const a = comm1[i];
        const b = comm2[i];
        bodyRows.push('<tr><td>' + (a ? escapeHtml(fmtRow(a)) : '') + '</td><td>' + (b ? escapeHtml(fmtRow(b)) : '') + '</td></tr>');
      }

      return '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">COMMS</div><div class=""fltPlanPage2Body""><table class=""fltPlanPage2Table""><thead><tr><th>COMM 1</th><th>COMM 2</th></tr></thead><tbody>' + bodyRows.join('') + '</tbody></table></div></div>';
    }

    function formatRuntimeCmdsPanelHtml(data){
      const server = (data && data.Server) || {};
      const roots = [server.Payload, server.Diagnostics];
      for (let i = 0; i < roots.length; i++){
        const root = roots[i];
        if (!root || typeof root !== 'object') continue;
        const block = formatDtcCmdsBlockHtml(root);
        if (block) return block;
      }
      return '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">CMDS</div><div class=""fltPlanPage2Body"">No runtime CMDS data.</div></div>';
    }

    function formatRuntimeCmdsInfoBlockHtml(data){
      const server = (data && data.Server) || {};
      const roots = [server.Payload, server.Diagnostics];
      for (let i = 0; i < roots.length; i++){
        const root = roots[i];
        if (!root || typeof root !== 'object') continue;
        const block = formatDtcCmdsBlockHtml(root);
        if (block) return block;
      }
      return '<div class=""fltPlanInfoBlock""><div class=""fltPlanInfoTitle"">CMDS</div><div class=""fltPlanInfoBody"">None</div></div>';
    }

    function formatRuntimePage2Html(data, pageSwitcherHtml, selected){
      let html = '<div class=""fltPlanBoard"">';
      if (pageSwitcherHtml){
        html += '<div style=""margin:4px 0 6px 0;"">' + pageSwitcherHtml + '</div>';
      }
      html += '<div class=""fltPlanPage2Grid"">';
      html += formatRuntimeCommPanelHtml(data);
      html += '<div class=""fltPlanPage2Stack"">';
      html += formatRuntimeCmdsPanelHtml(data);
      html += formatMapMarkersPanelHtml(selected, data);
      html += '</div>';
      html += '</div></div>';
      return html;
    }

    const STORE_CLSID_LOOKUP = __VAICOM_STORE_LOOKUP_JSON__;

    function getStoreFriendlyName(clsid){
      const raw = String(clsid || '').trim();
      if (!raw) return 'EMPTY';
      const normalized = (raw.length > 2 && raw[0] === '{' && raw[raw.length - 1] === '}')
        ? raw.substring(1, raw.length - 1).trim()
        : raw;
      const upper = normalized.toUpperCase();

      if (STORE_CLSID_LOOKUP && STORE_CLSID_LOOKUP[upper]) return STORE_CLSID_LOOKUP[upper];
      const rawUpper = raw.toUpperCase();
      if (STORE_CLSID_LOOKUP && STORE_CLSID_LOOKUP[rawUpper]) return STORE_CLSID_LOOKUP[rawUpper];

      const knownFallback = {
        'F376DBEE-4CAE-41BA-ADD9-B2910AC95DEC': 'Fuel tank 370 gal'
      };
      if (knownFallback[upper]) return knownFallback[upper];

      const rackMk = upper.match(/\*\s*(MK-\d+[A-Z0-9\-]*)/);
      if (rackMk && rackMk[1]) return String(rackMk[1]);

      if (upper.indexOf('EMPTY') >= 0) return 'EMPTY';
      if (upper.indexOf('ALQ_184') >= 0 || upper.indexOf('ALQ-184') >= 0) return 'ALQ-184';
      if (upper.indexOf('MK-82') >= 0) return 'MK-82';
      if (upper.indexOf('AIM-120') >= 0 || upper.indexOf('AMRAAM') >= 0) return 'AIM-120';
      if (upper.indexOf('AIM-9') >= 0 || upper.indexOf('SIDEWINDER') >= 0) return 'AIM-9';
      if (upper.indexOf('LANTIRN') >= 0 || upper.indexOf('SNIPER') >= 0 || upper.indexOf('LITENING') >= 0 || upper.indexOf('TARGET') >= 0) return 'TGP';
      if (upper.indexOf('TANK') >= 0 || upper.indexOf('GAL') >= 0 || upper.indexOf('FUEL') >= 0) return 'FUEL TANK';

      return raw;
    }

    function formatStoresPageHtml(pageSwitcherHtml, data){
      const server = (data && data.Server) || {};
      const payload = (server && server.Payload) || {};
      const stations = Array.isArray(payload.Stations) ? payload.Stations : [];

      let html = '<div class=""fltPlanBoard"">';
      if (pageSwitcherHtml){
        html += '<div style=""margin:4px 0 6px 0;"">' + pageSwitcherHtml + '</div>';
      }

      html += '<div class=""fltPlanStoresWrap"">';
      html += '<table class=""fltPlanStoresGrid"">';
      html += '<thead><tr><th class=""fltPlanStoresStation"">STN</th><th>STORE</th></tr></thead><tbody>';

      if (!stations.length){
        html += '<tr><td class=""fltPlanStoresStation"">-</td><td class=""fltPlanStoresEmpty"">No payload station data.</td></tr>';
      } else {
        for (let i = 0; i < stations.length; i++){
          const station = stations[i] || {};
          const clsid = String(station.CLSID || '').trim();
          const count = Number(station.count || 0);
          const empty = !clsid || count <= 0;
          const name = empty ? 'EMPTY' : getStoreFriendlyName(clsid);
          html += '<tr><td class=""fltPlanStoresStation"">' + String(i + 1) + '</td><td' + (empty ? ' class=""fltPlanStoresEmpty""' : '') + '>' + escapeHtml(name) + '</td></tr>';
        }
      }

      html += '</tbody></table></div></div>';
      return html;
    }

    function formatDtcRouteSummaryHtml(root, waypoints){
      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];
      const navRoute = Array.isArray(wypt.NAV_ROUTE) ? wypt.NAV_ROUTE : [];
      const f14Slots = getF14RouteSlots(root);
      const routeRows = f14Slots.length
        ? f14Slots.map(function(slot){ return String((slot && slot.key) || '').toUpperCase(); }).filter(function(k){ return isValidDtcRouteKey(k); })
        : ['R1','R2','R3'];

      function isRouteSelected(v){
        if (v === true) return true;
        if (v === false || v === null || v === undefined) return false;
        if (typeof v === 'number') return v !== 0;
        const s = String(v).trim().toLowerCase();
        return s === 'true' || s === '1' || s === 'yes' || s === 'y';
      }

      const idToStep = {};
      navPts.forEach(function(p, idx){
        const id = String((p && p.id) || '').toUpperCase();
        const step = isFinite(Number(p && p.wypt_num)) ? Math.round(Number(p.wypt_num)) : (idx + 1);
        if (id) idToStep[id] = step;
      });

      function routeList(routeKey){
        if (f14Slots.length){
          const labels = (Array.isArray(waypoints) ? waypoints : [])
            .filter(function(wp){ return String((wp && wp.__routeKey) || '').toUpperCase() === routeKey; })
            .sort(function(a, b){ return Number(a && a.step) - Number(b && b.step); })
            .map(function(wp){
              const step = String((wp && wp.step) || '-');
              const name = String((wp && wp.name) || '').trim();
              return name ? ('STP' + step + ' ' + name) : ('STP' + step);
            });
          return labels.length ? labels.join(', ') : '-';
        }

        const orderKey = routeKey + '_order';
        const points = navPts.filter(function(p){
          return !!(p && typeof p === 'object' && isRouteSelected(p[routeKey]));
        }).sort(function(a, b){
          const ao = Number(a && a[orderKey]);
          const bo = Number(b && b[orderKey]);
          if (isFinite(ao) && isFinite(bo) && ao !== bo) return ao - bo;
          const aw = Number(a && a.wypt_num);
          const bw = Number(b && b.wypt_num);
          if (isFinite(aw) && isFinite(bw) && aw !== bw) return aw - bw;
          return 0;
        });

        const labels = points.map(function(p){
          const n = isFinite(Number(p && p.wypt_num)) ? Number(p && p.wypt_num) : Number(p && p.number);
          return isFinite(n) ? ('STP' + String(Math.round(n))) : '-';
        });
        if (labels.length) return labels.join(', ');

        const routeIndex = routeKey === 'R1' ? 0 : (routeKey === 'R2' ? 1 : 2);
        const routeObj = (navRoute.length > routeIndex && navRoute[routeIndex] && typeof navRoute[routeIndex] === 'object') ? navRoute[routeIndex] : {};
        const routeKeys = Object.keys(routeObj);
        if (routeKeys.length){
          const routeSteps = routeKeys.map(function(k){
            const rp = routeObj[k] || {};
            const wn = Number(rp.wypt_num);
            if (isFinite(wn)) return Math.round(wn);
            const mapped = idToStep[String(k || '').toUpperCase()];
            return isFinite(Number(mapped)) ? Number(mapped) : NaN;
          }).filter(function(v){ return isFinite(v); }).sort(function(a,b){ return a-b; });
          if (routeSteps.length){
            return routeSteps.map(function(n){ return 'STP' + String(n); }).join(', ');
          }
        }

        if (routeKey === 'R1' && Array.isArray(waypoints) && waypoints.length){
          return waypoints.map(function(wp){ return 'STP' + String(wp.step || '-'); }).join(', ');
        }

        return '-';
      }

      const body = routeRows.map(function(routeKey){
        return '<tr><td>' + escapeHtml(routeKey) + '</td><td>' + escapeHtml(routeList(routeKey)) + '</td></tr>';
      });

      return '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">ROUTES</div><div class=""fltPlanPage2Body""><table class=""fltPlanPage2Table""><thead><tr><th style=""width:56px;"">ROUTE</th><th>WAYPOINTS</th></tr></thead><tbody>' + body.join('') + '</tbody></table></div></div>';
    }

    function formatMapMarkersPanelHtml(selected, data){
      const server = (data && data.Server) || {};
      const rawMarkers = Array.isArray(server.MapMarkers) ? server.MapMarkers : [];
      const theatre = String(server.Theater || '').trim();
      const coordDisplayMode = getNavlogCoordDisplayMode(selected);
      const coordHeaderText = (function(){
        if (coordDisplayMode === 'dms') return 'DMS';
        if (coordDisplayMode === 'ddm') return 'DDM';
        if (coordDisplayMode === 'mgrs') return 'MGRS';
        return 'X / Y';
      })();

      const markers = rawMarkers
        .map(function(m, idx){
          const north = Number(m && m.X);
          const east = Number(m && m.Z);
          if (!isFinite(north) || !isFinite(east)) return null;
          const rawId = Number(m && m.Id);
          const id = (isFinite(rawId) && rawId > 0) ? Math.round(rawId) : (idx + 1);
          const text = String((m && m.Text) || '').trim();
          const coordText = getNavlogCoordinateDisplayText({ xNum: north, yNum: east }, theatre, coordDisplayMode);
          return {
            id: id,
            text: text || '-',
            coordText: coordText
          };
        })
        .filter(function(m){ return !!m; })
        .sort(function(a, b){ return Number(a.id) - Number(b.id); });

      if (!markers.length){
        return '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">MAP MARKERS</div><div class=""fltPlanPage2Body"">No runtime map markers.</div></div>';
      }

      const rows = markers.map(function(m, i){
        const displayId = i + 1;
        return '<tr><td style=""width:54px;"">' + escapeHtml(String(displayId)) + '</td><td>' + escapeHtml(m.text) + '</td><td style=""width:220px;"">' + escapeHtml(m.coordText) + '</td></tr>';
      });

      return '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">MAP MARKERS</div><div class=""fltPlanPage2Body""><table class=""fltPlanPage2Table fltPlanPage2MarkerTable""><thead><tr><th style=""width:54px;"">ID</th><th>TEXT</th><th class=""fltPlanEtaHeader"" style=""width:220px;"" data-navlog-coord-cycle=""1"" title=""Click to cycle X/Y → DMS → DDM → MGRS"">POS ' + escapeHtml(coordHeaderText) + '</th></tr></thead><tbody>' + rows.join('') + '</tbody></table></div></div>';
    }

    function getDtcAvailableRoutes(root, waypoints){
      const f14Slots = getF14RouteSlots(root);
      if (f14Slots.length){
        const rows = Array.isArray(waypoints) ? waypoints : [];
        const available = ['R1'];
        f14Slots.forEach(function(slot){
          const routeKey = String((slot && slot.key) || '').toUpperCase();
          if (!isValidDtcRouteKey(routeKey) || routeKey === 'R1') return;
          const hasRows = rows.some(function(wp){ return String((wp && wp.__routeKey) || '').toUpperCase() === routeKey; });
          if (hasRows) available.push(routeKey);
        });
        return available;
      }

      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];
      const navRoute = Array.isArray(wypt.NAV_ROUTE) ? wypt.NAV_ROUTE : [];

      function hasRouteKey(routeKey){
        if (navPts.some(function(p){ return !!(p && typeof p === 'object' && p[routeKey] === true); })) return true;
        const idx = routeKey === 'R1' ? 0 : (routeKey === 'R2' ? 1 : 2);
        const routeObj = (navRoute.length > idx && navRoute[idx] && typeof navRoute[idx] === 'object') ? navRoute[idx] : {};
        if (Object.keys(routeObj).length > 0) return true;
        if (routeKey === 'R1' && Array.isArray(waypoints) && waypoints.length > 0) return true;
        return false;
      }

      const available = ['R1','R2','R3'].filter(hasRouteKey);
      return available.length ? available : ['R1'];
    }

    function filterDtcWaypointsByRoute(root, waypoints, routeKey){
      const route = String(routeKey || 'R1').toUpperCase();
      if (!isValidDtcRouteKey(route)) return Array.isArray(waypoints) ? waypoints : [];

      const list = Array.isArray(waypoints) ? waypoints : [];
      const hasTaggedRoutes = list.some(function(wp){ return String((wp && wp.__routeKey) || '').trim() !== ''; });
      if (list.some(function(wp){ return String((wp && wp.__routeKey) || '').toUpperCase() === route; })){
        return list
          .filter(function(wp){ return String((wp && wp.__routeKey) || '').toUpperCase() === route; })
          .sort(function(a, b){ return Number(a && a.step) - Number(b && b.step); });
      }
      if (hasTaggedRoutes) return [];

      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];
      const navRoute = Array.isArray(wypt.NAV_ROUTE) ? wypt.NAV_ROUTE : [];
      if (!navPts.length || !list.length) return list;

      const stepSet = {};
      const routeOrder = {};
      const orderKey = route + '_order';

      navPts.forEach(function(p){
        if (!p || typeof p !== 'object') return;
        const selected = (p[route] === true) || (String(p[route] || '').toLowerCase() === 'true') || (Number(p[route]) === 1);
        if (!selected) return;
        const step = isFinite(Number(p.wypt_num)) ? Math.round(Number(p.wypt_num)) : (isFinite(Number(p.number)) ? Math.round(Number(p.number)) : NaN);
        if (!isFinite(step)) return;
        stepSet[step] = true;
        const ord = Number(p[orderKey]);
        if (isFinite(ord)) routeOrder[step] = ord;
      });

      if (!Object.keys(stepSet).length){
        const idx = route === 'R1' ? 0 : (route === 'R2' ? 1 : 2);
        const routeObj = (navRoute.length > idx && navRoute[idx] && typeof navRoute[idx] === 'object') ? navRoute[idx] : {};
        const idToStep = {};
        navPts.forEach(function(p){
          const id = String((p && p.id) || '').toUpperCase();
          const step = isFinite(Number(p && p.wypt_num)) ? Math.round(Number(p.wypt_num)) : (isFinite(Number(p && p.number)) ? Math.round(Number(p.number)) : NaN);
          if (id && isFinite(step)) idToStep[id] = step;
        });
        Object.keys(routeObj).forEach(function(k){
          const rp = routeObj[k] || {};
          let step = Number(rp.wypt_num);
          if (!isFinite(step)) step = Number(idToStep[String(k || '').toUpperCase()]);
          if (!isFinite(step)) return;
          step = Math.round(step);
          stepSet[step] = true;
          const ord = Number(rp[orderKey] || rp.route_num || rp.order);
          if (isFinite(ord)) routeOrder[step] = ord;
        });
      }

      if (!Object.keys(stepSet).length){
        return route === 'R1' ? list : [];
      }

      return list
        .filter(function(wp){ return !!stepSet[Number(wp.step)]; })
        .sort(function(a, b){
          const sa = Number(a && a.step);
          const sb = Number(b && b.step);
          const oa = routeOrder[sa];
          const ob = routeOrder[sb];
          if (isFinite(oa) && isFinite(ob) && oa !== ob) return oa - ob;
          if (isFinite(oa) && !isFinite(ob)) return -1;
          if (!isFinite(oa) && isFinite(ob)) return 1;
          return sa - sb;
        });
    }

    function formatDtcPage2Html(root, pageSwitcherHtml, waypoints, data, selected){
      let html = '<div class=""fltPlanBoard"">';
      if (pageSwitcherHtml){
        html += '<div style=""margin:4px 0 6px 0;"">' + pageSwitcherHtml + '</div>';
      }
      const dtcCommHtml = formatDtcCommPanelHtml(root, true);
      html += '<div class=""fltPlanPage2Grid"">';
      html += dtcCommHtml || formatRuntimeCommPanelHtml(data);
      html += '<div class=""fltPlanPage2Stack"">';
      html += formatDtcRouteSummaryHtml(root, waypoints);
      html += formatMapMarkersPanelHtml(selected, data);
      html += '</div>';
      html += '</div></div>';
      return html;
    }

    function getMudMapPointType(wp){
      const typeRaw = String((wp && (wp.typeRaw || wp.type || '')) || '').toUpperCase();
      const nameRaw = String((wp && (wp.name || '')) || '').toUpperCase();
      const combined = typeRaw + ' ' + nameRaw;

      if (combined.indexOf('AAR') >= 0 || combined.indexOf('AIR REFUEL') >= 0) return 'aar';
      if (combined.indexOf('CAP') >= 0 || combined.indexOf('COMBAT AIR PATROL') >= 0) return 'cap';
      if (combined.indexOf('HLD') >= 0 || combined.indexOf('HOLD') >= 0) return 'hld';
      if (combined.indexOf('TGT') >= 0 || combined.indexOf('TARGET') >= 0) return 'tgt';
      if (combined.indexOf('IP') >= 0 || combined.indexOf('INITIAL POINT') >= 0 || combined.indexOf('INBOUND POINT') >= 0) return 'ip';
      if (combined.indexOf('DVRT') >= 0 || combined.indexOf('DIVERT') >= 0 || combined.indexOf('DIVERSION') >= 0) return 'dvrt';
      if (combined.indexOf('TKO') >= 0 || combined.indexOf('TAK') >= 0 || combined.indexOf('TAKEOFF') >= 0 || combined.indexOf('TAKE OFF') >= 0) return 'tko';
      if (combined.indexOf('LDG') >= 0 || combined.indexOf('LAND') >= 0) return 'ldg';
      if (combined.indexOf('LAND') >= 0 || combined.indexOf('HOME') >= 0 || combined.indexOf('BASE') >= 0 || combined.indexOf('TAKEOFF') >= 0) return 'home';
      return 'wp';
    }

    function getMudMapSegments(points){
      const rows = Array.isArray(points) ? points : [];
      const segs = [];
      for (let i = 1; i < rows.length; i++){
        const a = rows[i - 1];
        const b = rows[i];
        const aType = getMudMapPointType(a);
        const bType = getMudMapPointType(b);
        const fromHomeLike = (aType === 'home' || aType === 'ldg');
        const toHomeLike = (bType === 'home' || bType === 'ldg');
        const dashed = (fromHomeLike && !toHomeLike);
        segs.push({ from: a, to: b, dashed: dashed });
      }
      return segs;
    }

    function getMudMapAssets(data, includeDlinkAssets){
      const server = (data && data.Server) || {};
      const includeDlink = (includeDlinkAssets !== false);
      const rawAssets = includeDlink && Array.isArray(server.FriendlyAssets) ? server.FriendlyAssets : [];
      const mappedAssets = rawAssets
        .map(function(a){
          const northNum = Number(a && a.X);
          const eastNum = Number(a && a.Y);
          const xNum = isFinite(northNum) ? northNum : Number(a && a.X);
          const yNum = isFinite(eastNum) ? eastNum : Number(a && a.Y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          return {
            callsign: String((a && a.Callsign) || '').trim(),
            name: String((a && a.Name) || '').trim(),
            category: String((a && a.Category) || '').trim().toUpperCase(),
            typeName: String((a && a.TypeName) || '').trim(),
            icaoType: String((a && a.IcaoType) || '').trim().toUpperCase(),
            frequency: String((a && a.Frequency) || '').trim(),
            altFrequencies: Array.isArray(a && a.AltFrequencies) ? a.AltFrequencies.map(function(v){ return String(v || '').trim(); }).filter(function(v){ return !!v; }) : [],
            tacan: String((a && a.Tacan) || '').trim(),
            mpClientCallsign: String((a && a.MpClientCallsign) || '').trim(),
            altFeet: Number(a && a.AltFeet),
            markerId: 0,
            markerDisplayId: 0,
            markerText: '',
            xNum: xNum,
            yNum: yNum
          };
        })
        .filter(function(a){ return !!a; });

      const rawMarkers = Array.isArray(server.MapMarkers) ? server.MapMarkers : [];
      const mappedMarkers = rawMarkers
        .map(function(m){
          const northNum = Number(m && m.X);
          const eastNum = Number(m && m.Z);
          if (!isFinite(northNum) || !isFinite(eastNum)) return null;
          const markerId = Number(m && m.Id);
          const markerText = String((m && m.Text) || '').trim();
          const markerAuthor = String((m && m.Author) || '').trim();
          return {
            callsign: 'MKR',
            name: markerText,
            category: 'MAP_MARKER',
            typeName: '',
            frequency: '',
            tacan: '',
            mpClientCallsign: '',
            altFeet: 0,
            markerId: isFinite(markerId) ? Math.round(markerId) : 0,
            markerDisplayId: 0,
            markerText: markerText,
            markerAuthor: markerAuthor,
            xNum: northNum,
            yNum: eastNum
          };
        })
        .filter(function(m){ return !!m; })
        .sort(function(a, b){
          const aid = Number(a && a.markerId);
          const bid = Number(b && b.markerId);
          if (isFinite(aid) && isFinite(bid) && aid !== bid) return aid - bid;
          return 0;
        })
        .map(function(m, i){
          m.markerDisplayId = i + 1;
          m.callsign = 'MKR ' + String(m.markerDisplayId);
          return m;
        });

      return mappedAssets.concat(mappedMarkers);
    }

    function getMudMapAssetKind(asset){
      const category = String((asset && asset.category) || '').toUpperCase();
      if (category === 'MAP_MARKER') return 'marker';
      if (category === 'TANKER') return 'tanker';
      if (category === 'AWACS') return 'awacs';
      if (category === 'JTAC') return 'jtac';
      const text = String((asset && asset.name) || '').toUpperCase();
      if (text.indexOf('HELO') >= 0 || text.indexOf('HELICOPTER') >= 0 || text.indexOf('ROTOR') >= 0) return 'rotary';
      return 'fixed';
    }

    function getDtcMpdRoot(root){
      if (!root || typeof root !== 'object') return null;
      if (root.MPD && typeof root.MPD === 'object') return root.MPD;
      return findFirstObjectByKeyPattern(root, /^MPD$/i, 0);
    }

    function buildMapAirfields(data){
      const model = data || latestData || {};
      const server = (model && model.Server) || {};
      const assets = Array.isArray(server.FriendlyAssets) ? server.FriendlyAssets : [];
      const atcMetars = (server && server.AtcMetars && typeof server.AtcMetars === 'object') ? server.AtcMetars : {};
      const atcIcaoTypes = (server && server.AtcIcaoTypes && typeof server.AtcIcaoTypes === 'object') ? server.AtcIcaoTypes : {};
      const metarKeys = Object.keys(atcMetars);
      const rows = [];
      const seen = {};

      function normalizeOverrideType(value){
        const t = String(value || '').trim().toUpperCase();
        if (t === 'MIL' || t === 'CIV' || t === 'JOINT') return t;
        return '';
      }

      function normalizeOverrideLookupKey(value){
        return String(value || '')
          .toUpperCase()
          .replace(/[\_\-\/\.,\(\)]/g, ' ')
          .replace(/\s+/g, ' ')
          .trim();
      }

      function resolveOverrideType(icao, text, fallbackKey, directType){
        const direct = normalizeOverrideType(directType);
        if (direct) return direct;
        const keyIcao = String(icao || '').toUpperCase();
        const keyText = String(text || '').toUpperCase();
        const keyFallback = String(fallbackKey || '').toUpperCase();
        const normText = normalizeOverrideLookupKey(text);
        const normFallback = normalizeOverrideLookupKey(fallbackKey);
        return normalizeOverrideType(atcIcaoTypes[keyIcao])
          || normalizeOverrideType(atcIcaoTypes[keyText])
          || normalizeOverrideType(atcIcaoTypes[keyFallback])
          || normalizeOverrideType(atcIcaoTypes[normText])
          || normalizeOverrideType(atcIcaoTypes[normFallback])
          || (function(){
            if (!normText && !normFallback) return '';
            const keys = Object.keys(atcIcaoTypes || {});
            for (let i = 0; i < keys.length; i++){
              const k = String(keys[i] || '');
              if (!k) continue;
              const nk = normalizeOverrideLookupKey(k);
              if (!nk) continue;
              if ((normText && (normText === nk || normText.indexOf(nk) >= 0 || nk.indexOf(normText) >= 0))
                || (normFallback && (normFallback === nk || normFallback.indexOf(nk) >= 0 || nk.indexOf(normFallback) >= 0))){
                const t = normalizeOverrideType(atcIcaoTypes[k]);
                if (t) return t;
              }
            }
            return '';
          })()
          || '';
      }

      function tokenIcao(text){
        const m = String(text || '').toUpperCase().match(/\b([A-Z]{4})\b/);
        return m ? String(m[1] || '') : '';
      }

      function metarIcao(text){
        const m = String(text || '').toUpperCase().match(/\bMETAR\s+([A-Z]{4})\b/);
        return m ? String(m[1] || '') : '';
      }

      function resolveMetarKey(text){
        const upper = String(text || '').toUpperCase();
        const compact = upper.replace(/[^A-Z0-9]/g, '');
        for (let i = 0; i < metarKeys.length; i++){
          const k = String(metarKeys[i] || '').toUpperCase();
          if (!k) continue;
          if (upper.indexOf(k) >= 0) return metarKeys[i];
          const kc = k.replace(/[^A-Z0-9]/g, '');
          if (compact && kc && (compact.indexOf(kc) >= 0 || kc.indexOf(compact) >= 0)) return metarKeys[i];
        }
        return '';
      }

      function parseVisMeters(text){
        const t = String(text || '').toUpperCase();
        if (!t) return NaN;
        if (t.indexOf('CAVOK') >= 0) return 10000;
        const mm = t.match(/(?:^|\s)(\d{4})(?:\s|$)/);
        const mv = mm ? Number(mm[1]) : NaN;
        return isFinite(mv) ? mv : NaN;
      }

      function isCloudVfr(text){
        const t = String(text || '').toUpperCase();
        if (!t) return false;
        if (t.indexOf('CAVOK') >= 0) return true;
        const rx = /\b(FEW|SCT|BKN|OVC)(\d{3})\b/g;
        let m = null;
        let lowSctCount = 0;
        while ((m = rx.exec(t)) !== null){
          const layer = String(m[1] || '').toUpperCase();
          const ft = Number(m[2]) * 100;
          if (!isFinite(ft)) continue;
          if (ft < 1500){
            if (layer === 'BKN' || layer === 'OVC') return false;
            if (layer === 'SCT'){
              lowSctCount += 1;
              if (lowSctCount >= 2) return false;
            }
          }
        }
        return true;
      }

      function isVfrMetar(text){
        const vis = parseVisMeters(text);
        const cloudVfr = isCloudVfr(text);
        return isFinite(vis) && cloudVfr && vis >= 5000;
      }

      assets.forEach(function(a){
        const cat = String((a && a.Category) || (a && a.category) || '').toUpperCase();
        if (cat !== 'ATC') return;

        const north = Number(a && (a.X !== undefined ? a.X : a.x));
        const east = Number(a && (a.Y !== undefined ? a.Y : a.y));
        if (!isFinite(north) || !isFinite(east)) return;

        const text = [a && a.Callsign, a && a.Name, a && a.TypeName, a && a.Category]
          .map(function(v){ return String(v || '').trim(); })
          .filter(function(v){ return !!v; })
          .join(' ');

        const key = resolveMetarKey(text);
        const metar = key ? String(atcMetars[key] || '') : '';
        const icao = tokenIcao(text) || metarIcao(metar) || tokenIcao(key);
        const overrideType = resolveOverrideType(icao, text, key, a && a.IcaoType);
        const label = String((icao || (a && (a.Callsign || a.Name)) || 'ATC')).toUpperCase();

        const dedupeKey = label + '|' + String(Math.round(north)) + '|' + String(Math.round(east));
        if (seen[dedupeKey]) return;
        seen[dedupeKey] = true;

        const u = text.toUpperCase();
        rows.push({
          xNum: north,
          yNum: east,
          callsign: String((a && a.Callsign) || '').trim(),
          name: String((a && a.Name) || '').trim(),
          category: 'ATC',
          typeName: String((a && a.TypeName) || '').trim(),
          frequency: String((a && a.Frequency) || '').trim(),
          altFrequencies: Array.isArray(a && a.AltFrequencies) ? a.AltFrequencies.map(function(v){ return String(v || '').trim(); }).filter(function(v){ return !!v; }) : [],
          tacan: String((a && a.Tacan) || '').trim(),
          mpClientCallsign: String((a && a.MpClientCallsign) || '').trim(),
          altFeet: Number(a && a.AltFeet),
          label: label,
          icao: String(icao || ''),
          type: (u.indexOf('SEAPLANE') >= 0 ? 'seaplane' : (u.indexOf('HELI') >= 0 || u.indexOf('FARP') >= 0 ? 'heliport' : (overrideType === 'MIL' || overrideType === 'JOINT' ? 'airport' : 'airport'))),
          isMilitary: (overrideType === 'MIL' || overrideType === 'JOINT')
            ? true
            : (u.indexOf('MIL') >= 0 || u.indexOf('AIRBASE') >= 0 || u.indexOf(' AFB') >= 0 || u.indexOf('NAS') >= 0),
          isVfr: isVfrMetar(metar),
        });
      });

      return rows;
    }

    function getDtcMapOverlays(root, routeKey, data){
      const mpd = getDtcMpdRoot(root);
      const sa = (root && typeof root === 'object' && root.SA && typeof root.SA === 'object')
        ? root.SA
        : findFirstObjectByKeyPattern(root, /^SA$/i, 0);
      const f14Slots = getF14RouteSlots(root);
      const route = String(routeKey || 'R1').toUpperCase();
      const selectedF14Slot = f14Slots.find(function(slot){ return String((slot && slot.key) || '').toUpperCase() === route; }) || f14Slots[0] || null;
      const selectedF14Route = selectedF14Slot && selectedF14Slot.route && typeof selectedF14Slot.route === 'object'
        ? selectedF14Slot.route
        : null;

      const allGeoLines = Array.isArray(mpd && mpd.GEO_LINES) ? mpd.GEO_LINES : [];
      const geoLines = allGeoLines
        .filter(function(p){
          if (!p || typeof p !== 'object') return false;
          return isFinite(Number(p.x)) && isFinite(Number(p.y));
        })
        .map(function(p){
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          const lineFlags = [];
          if (p && p.L1) lineFlags.push('L1');
          if (p && p.L2) lineFlags.push('L2');
          if (p && p.L3) lineFlags.push('L3');
          if (p && p.L4) lineFlags.push('L4');
          return {
            xNum: xNum,
            yNum: yNum,
            number: Number(p && p.number),
            label: String((p && p.id) || (p && p.note) || '').trim(),
            lineFlags: lineFlags
          };
        })
        .filter(function(p){ return !!p; })
        .sort(function(a, b){
          const an = Number(a && a.number);
          const bn = Number(b && b.number);
          if (isFinite(an) && isFinite(bn) && an !== bn) return an - bn;
          return 0;
        });

      const threatPoints = (Array.isArray(mpd && mpd.THREAT_PTS) ? mpd.THREAT_PTS : [])
        .map(function(p){
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          const radiusMeters = Number(p && p.radius);
          return {
            xNum: xNum,
            yNum: yNum,
            radiusMeters: isFinite(radiusMeters) && radiusMeters > 0 ? radiusMeters : 0,
            ring: !!(p && p.ring),
            label: String((p && p.text) || (p && p.threatName) || '').trim()
          };
        })
        .filter(function(p){ return !!p; });

      const mezThreatPoints = (Array.isArray(sa && sa.MEZ_THRTS) ? sa.MEZ_THRTS : [])
        .map(function(p){
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          const rawRadius = Number(p && p.threat_ring_radius);
          const radiusMeters = isFinite(rawRadius) && rawRadius > 0
            ? (rawRadius > 1000 ? rawRadius : (rawRadius * 1852))
            : 0;
          return {
            xNum: xNum,
            yNum: yNum,
            radiusMeters: radiusMeters,
            ring: radiusMeters > 0,
            label: String((p && p.text) || (p && p.threat_type) || (p && p.id) || '').trim()
          };
        })
        .filter(function(p){ return !!p; });

      const destinationPoints = (Array.isArray(mpd && mpd.DEST) ? mpd.DEST : [])
        .map(function(p){
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          return {
            xNum: xNum,
            yNum: yNum,
            label: String((p && p.text) || (p && p.note) || (p && p.id) || '').trim()
          };
        })
        .filter(function(p){ return !!p; });

      function parseLineCollection(items){
        const rows = Array.isArray(items) ? items : [];
        return rows.map(function(line){
          const points = (Array.isArray(line && line.points) ? line.points : [])
            .map(function(pt){
              const xNum = Number(pt && pt.x);
              const yNum = Number(pt && pt.y);
              if (!isFinite(xNum) || !isFinite(yNum)) return null;
              return {
                xNum: xNum,
                yNum: yNum,
                number: Number(pt && pt.num),
                label: String((pt && pt.id) || '').trim()
              };
            })
            .filter(function(pt){ return !!pt; });
          return {
            id: String((line && line.id) || '').trim(),
            number: Number(line && line.num),
            label: String((line && line.note) || (line && line.id) || '').trim(),
            points: points
          };
        }).filter(function(line){ return line && line.points && line.points.length > 0; });
      }

      const faorRoot = sa && sa.FAOR_FLOT && typeof sa.FAOR_FLOT === 'object' ? sa.FAOR_FLOT : null;
      const faorLines = parseLineCollection(faorRoot && faorRoot.FAOR);
      const flotLines = parseLineCollection(faorRoot && faorRoot.FLOT);

      const capPoints = (Array.isArray(sa && sa.CAP_PTS) ? sa.CAP_PTS : [])
        .map(function(p){
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          return {
            xNum: xNum,
            yNum: yNum,
            number: Number(p && p.num),
            label: String((p && p.note) || (p && p.id) || '').trim(),
            course: Number(p && p.course),
            lengthMeters: Number(p && p.length),
            diameterMeters: Number(p && p.diameter),
            turnDirection: String((p && p.turn_direction) || '').trim()
          };
        })
        .filter(function(p){ return !!p; });

      const corridors = parseLineCollection(sa && sa.CORRIDORS);

      const f14AdditionalPoints = (Array.isArray(selectedF14Route && selectedF14Route.additional_points) ? selectedF14Route.additional_points : [])
        .map(function(p){
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          const nameInfo = getF14WaypointTypeInfo((p && p.name) || '');
          return {
            xNum: xNum,
            yNum: yNum,
            label: String(nameInfo.name || (p && p.name) || '').trim(),
            isBullseye: !!nameInfo.isBullseye,
            typeRaw: String(nameInfo.typeRaw || 'WP')
          };
        })
        .filter(function(p){ return !!p; });

      const f14Lines = (Array.isArray(selectedF14Route && selectedF14Route.lines) ? selectedF14Route.lines : [])
        .map(function(line, lineIdx){
          const points = (Array.isArray(line && line.points) ? line.points : [])
            .map(function(pt, pointIdx){
              const xNum = Number(pt && pt.x);
              const yNum = Number(pt && pt.y);
              if (!isFinite(xNum) || !isFinite(yNum)) return null;
              return {
                xNum: xNum,
                yNum: yNum,
                number: isFinite(Number(pt && pt.number)) ? Number(pt.number) : (pointIdx + 1),
                label: String((pt && pt.name) || '').trim()
              };
            })
            .filter(function(pt){ return !!pt; });
          return {
            id: 'F14-LINE-' + String(lineIdx + 1),
            number: lineIdx + 1,
            label: String((line && line.name) || '').trim(),
            points: points
          };
        })
        .filter(function(line){ return line && Array.isArray(line.points) && line.points.length > 1; });

      const jdamThreatPoints = (Array.isArray(root && root.JDAM && root.JDAM.stations) ? root.JDAM.stations : [])
        .reduce(function(acc, station){
          const targets = Array.isArray(station && station.targets) ? station.targets : [];
          targets.forEach(function(target){
            const active = !!(target && target.active);
            const xNum = Number(target && target.x);
            const yNum = Number(target && target.y);
            if (!active || !isFinite(xNum) || !isFinite(yNum)) return;
            acc.push({
              xNum: xNum,
              yNum: yNum,
              radiusMeters: 0,
              ring: false,
              label: String((target && target.name) || 'DMPI').trim() || 'DMPI'
            });
          });
          return acc;
        }, []);

      const f14BullseyeDestinations = f14AdditionalPoints
        .filter(function(p){ return !!(p && p.isBullseye); })
        .map(function(p){
          return {
            xNum: Number(p.xNum),
            yNum: Number(p.yNum),
            label: String(p.label || 'BULLSEYE')
          };
        });

      const f14AreaThreatPoints = f14AdditionalPoints
        .filter(function(p){
          const typeRaw = String((p && p.typeRaw) || '').toUpperCase();
          return typeRaw === 'DP' || typeRaw === 'HA';
        })
        .map(function(p){
          const typeRaw = String((p && p.typeRaw) || '').toUpperCase();
          const areaLabel = typeRaw === 'DP' ? 'DP' : 'HA';
          const nameLabel = String((p && p.label) || '').trim();
          return {
            xNum: Number(p.xNum),
            yNum: Number(p.yNum),
            radiusMeters: 20 * 1852,
            ring: true,
            label: (areaLabel + (nameLabel ? (' ' + nameLabel) : '')).trim(),
            subtype: areaLabel.toLowerCase()
          };
        });

      const f14GeneralDestinations = f14AdditionalPoints
        .filter(function(p){
          if (!p || p.isBullseye) return false;
          const typeRaw = String((p && p.typeRaw) || '').toUpperCase();
          return typeRaw !== 'DP' && typeRaw !== 'HA';
        })
        .map(function(p){
          const typeRaw = String((p && p.typeRaw) || '').toUpperCase();
          const prioMatch = typeRaw.match(/^PRIO\s*(\d+)$/);
          const isPriority = !!prioMatch;
          const priorityLabel = isPriority ? ('P' + String(prioMatch[1])) : '';
          const labelText = isPriority
            ? priorityLabel
            : String(p.label || '').trim();
          const subtype = typeRaw === 'LANTIRN'
            ? 'lantirn'
            : (isPriority ? 'priority' : typeRaw.toLowerCase());
          return {
            xNum: Number(p.xNum),
            yNum: Number(p.yNum),
            label: labelText,
            subtype: subtype
          };
        });

      return {
        geolines: geoLines,
        threatPoints: threatPoints.concat(mezThreatPoints).concat(jdamThreatPoints).concat(f14AreaThreatPoints),
        destinationPoints: destinationPoints.concat(f14BullseyeDestinations).concat(f14GeneralDestinations),
        faorLines: faorLines.concat(f14Lines),
        flotLines: flotLines,
        capPoints: capPoints,
        corridors: corridors,
        airfields: (typeof buildMapAirfields === 'function')
          ? buildMapAirfields(data || latestData)
          : ((typeof BuildMapAirfields === 'function') ? BuildMapAirfields(data || latestData) : [])
      };
    }

    function isBullseyeText(text){
      const s = String(text || '').trim().toUpperCase();
      if (!s) return false;
      if (s === 'BULL' || s === 'BULLSEYE') return true;
      if (s.indexOf('BULLSEYE') >= 0) return true;
      return /(^|\W)BULL(\W|$)/.test(s);
    }

    function toBullseyePoint(x, y, label){
      const xNum = Number(x);
      const yNum = Number(y);
      if (!isFinite(xNum) || !isFinite(yNum)) return null;
      const safeLabel = String(label || '').trim();
      return {
        xNum: xNum,
        yNum: yNum,
        label: safeLabel || 'BULLSEYE'
      };
    }

    function getMapBullseyePoint(root, waypoints, overlays, data){
      const model = data || latestData || {};
      const server = (model && model.Server) || {};
      const diagnostics = (server && typeof server.Diagnostics === 'object' && server.Diagnostics) || {};
      const diagX = Number(diagnostics && diagnostics.bullseyeX);
      const diagY = Number(diagnostics && diagnostics.bullseyeY);
      const diagValid = !!(diagnostics && diagnostics.bullseyeValid);
      if (diagValid && isFinite(diagX) && isFinite(diagY)){
        const diagCoal = String((diagnostics && diagnostics.bullseyeCoalition) || '').trim();
        const label = diagCoal ? ('BULL ' + diagCoal.toUpperCase()) : 'BULLSEYE';
        return {
          xNum: diagX,
          yNum: diagY,
          label: label
        };
      }

      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];

      for (let i = 0; i < navPts.length; i++){
        const p = navPts[i] || {};
        const label = String(p.note || p.text_note || p.name || p.text || p.id || '').trim();
        if (!isBullseyeText(label)) continue;
        const found = toBullseyePoint(p.x, p.y, label);
        if (found) return found;
      }

      const rows = Array.isArray(waypoints) ? waypoints : [];
      for (let i = 0; i < rows.length; i++){
        const wp = rows[i] || {};
        const label = String(wp.name || wp.label || wp.note || '').trim();
        if (!isBullseyeText(label)) continue;
        const found = toBullseyePoint(wp.xNum, wp.yNum, label);
        if (found) return found;
      }

      const mapOverlays = overlays && typeof overlays === 'object' ? overlays : {};
      const destinationPoints = Array.isArray(mapOverlays.destinationPoints) ? mapOverlays.destinationPoints : [];
      for (let i = 0; i < destinationPoints.length; i++){
        const p = destinationPoints[i] || {};
        const label = String(p.label || p.text || p.note || p.id || '').trim();
        if (!isBullseyeText(label)) continue;
        const found = toBullseyePoint(p.xNum, p.yNum, label);
        if (found) return found;
      }

      return null;
    }

    function buildOpenFreeMapPayload(waypoints, data, overlays, bullseyePoint){
      const rows = Array.isArray(waypoints) ? waypoints.filter(function(wp){
        return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum));
      }) : [];
      const model = data || latestData || {};
      const server = (model && model.Server) || {};
      const diagnostics = (server && server.Diagnostics) || {};
      const theatreCandidates = [
        server.Theater,
        diagnostics.theater,
        diagnostics.terrain,
        diagnostics.terrainName,
        lastKnownTheater,
      ];
      let theatre = '';
      for (let i = 0; i < theatreCandidates.length; i++){
        const candidate = String(theatreCandidates[i] || '').trim();
        if (!candidate) continue;
        if (getMapProjectionByTheatre(candidate)){
          theatre = candidate;
          break;
        }
      }
      if (!theatre){
        openFreeMapLastPayloadStatus = 'no-theatre';
        return null;
      }

      const mapOverlays = overlays && typeof overlays === 'object'
        ? overlays
        : { geolines: [], threatPoints: [], destinationPoints: [], faorLines: [], flotLines: [], capPoints: [], corridors: [] };

      function toLonLat(point){
        const north = Number(point && point.xNum);
        const east = Number(point && point.yNum);
        if (!isFinite(north) || !isFinite(east)) return null;

        function validLonLat(lon, lat){
          return isFinite(Number(lon))
            && isFinite(Number(lat))
            && Math.abs(Number(lat)) <= 90
            && Math.abs(Number(lon)) <= 180;
        }

        const llPrimary = convertDcsXYToLatLon(theatre, north, east);
        if (llPrimary && validLonLat(llPrimary.lon, llPrimary.lat)){
          return [Number(llPrimary.lon), Number(llPrimary.lat)];
        }

        const llSwapped = convertDcsXYToLatLon(theatre, east, north);
        if (llSwapped && validLonLat(llSwapped.lon, llSwapped.lat)){
          return [Number(llSwapped.lon), Number(llSwapped.lat)];
        }

        if (validLonLat(east, north)){
          return [east, north];
        }
        if (validLonLat(north, east)){
          return [north, east];
        }

        return null;
      }

      function getTheatreCenterLonLat(theatreName){
        const t = String(theatreName || '').replace(/[^A-Za-z0-9]/g, '').toUpperCase();
        const centers = {
          CAUCASUS: [44.5, 43.4],
          NEVADA: [-115.2, 36.2],
          NORMANDY: [0.6, 49.2],
          THECHANNEL: [1.2, 51.0],
          PERSIANGULF: [56.2, 25.5],
          SYRIA: [37.0, 35.4],
          MARIANAISLANDS: [145.6, 15.2],
          FALKLANDS: [-59.5, -52.1],
          SINAIMAP: [34.2, 30.1],
          KOLA: [29.5, 68.7],
          AFGHANISTAN: [66.0, 34.5],
          IRAQ: [44.8, 33.2],
          GERMANYCW: [10.2, 51.2],
        };
        return centers[t] || null;
      }

      const pointsForBounds = [];
      const features = [];

      function addPointFeature(point, props){
        const lonLat = toLonLat(point);
        if (!lonLat) return;
        pointsForBounds.push(lonLat);
        features.push({
          type: 'Feature',
          geometry: {
            type: 'Point',
            coordinates: lonLat,
          },
          properties: props || {},
        });
      }

      function addLineFeature(points, props){
        const coords = (Array.isArray(points) ? points : [])
          .map(function(p){ return toLonLat(p); })
          .filter(function(c){ return Array.isArray(c) && c.length === 2; });
        if (coords.length < 2) return;
        coords.forEach(function(c){ pointsForBounds.push(c); });
        features.push({
          type: 'Feature',
          geometry: {
            type: 'LineString',
            coordinates: coords,
          },
          properties: props || {},
        });
      }

      const byStep = {};
      rows.forEach(function(wp){ byStep[String(wp && wp.step)] = wp; });
      getMudMapSegments(rows).forEach(function(seg){
        const from = byStep[String(seg && seg.from && seg.from.step)] || (seg && seg.from);
        const to = byStep[String(seg && seg.to && seg.to.step)] || (seg && seg.to);
        if (!from || !to) return;
        addLineFeature([from, to], {
          stroke: '#2f5fa7',
          lineWidth: seg && seg.dashed ? 1.8 : 2.2,
          dashed: !!(seg && seg.dashed)
        });
      });

      rows.forEach(function(wp){
        const step = String(wp && wp.step || '').trim();
        const name = String(wp && wp.name || '').trim();
        const label = step ? (step + (name ? (' ' + name) : '')) : name;
        addPointFeature(wp, {
          kind: 'waypoint',
          label: label || 'WP',
          fill: '#2d8fe3',
          stroke: '#1f5d93',
          textColor: '#1f3550',
          radius: 4,
        });
      });

      (Array.isArray(mapOverlays.geolines) ? mapOverlays.geolines : []).forEach(function(p){
        addPointFeature(p, {
          kind: 'geoline',
          label: String((p && p.label) || '').trim(),
          fill: '#7a57b3',
          stroke: '#5a3d89',
          textColor: '#5a3d89',
          radius: 3,
        });
      });

      const geolineRows = Array.isArray(mapOverlays.geolines) ? mapOverlays.geolines : [];
      ['L1', 'L2', 'L3', 'L4'].forEach(function(lineKey){
        const linePoints = geolineRows
          .filter(function(p){
            const flags = (p && Array.isArray(p.lineFlags)) ? p.lineFlags : [];
            return flags.indexOf(lineKey) >= 0;
          })
          .sort(function(a, b){
            const an = Number(a && a.number);
            const bn = Number(b && b.number);
            if (isFinite(an) && isFinite(bn) && an !== bn) return an - bn;
            return 0;
          });
        if (linePoints.length >= 2){
          addLineFeature(linePoints, { stroke: '#7a57b3', lineWidth: 2.2, dashed: true });
        }
      });

      function addGroupedLines(groups, strokeColor){
        (Array.isArray(groups) ? groups : []).forEach(function(g){
          const pts = (Array.isArray(g && g.points) ? g.points : []);
          addLineFeature(pts, { stroke: strokeColor, lineWidth: 2, dashed: true });
        });
      }

      function addCorridorBounds(groups){
        const corridorHalfWidthMeters = 5000;
        (Array.isArray(groups) ? groups : []).forEach(function(group){
          const pts = (Array.isArray(group && group.points) ? group.points : [])
            .map(function(p){
              return {
                xNum: Number(p && p.xNum),
                yNum: Number(p && p.yNum),
                number: Number(p && p.number)
              };
            })
            .filter(function(p){ return isFinite(p.xNum) && isFinite(p.yNum); })
            .sort(function(a, b){
              if (isFinite(a.number) && isFinite(b.number) && a.number !== b.number) return a.number - b.number;
              return 0;
            });
          if (pts.length < 2) return;

          const left = [];
          const right = [];
          for (let i = 0; i < pts.length; i++){
            const prev = pts[Math.max(0, i - 1)];
            const next = pts[Math.min(pts.length - 1, i + 1)];
            const dNorth = Number(next.xNum) - Number(prev.xNum);
            const dEast = Number(next.yNum) - Number(prev.yNum);
            const len = Math.sqrt((dNorth * dNorth) + (dEast * dEast));
            if (!isFinite(len) || len <= 0){
              left.push({ xNum: pts[i].xNum, yNum: pts[i].yNum });
              right.push({ xNum: pts[i].xNum, yNum: pts[i].yNum });
              continue;
            }
            const unitEast = dEast / len;
            const unitNorth = dNorth / len;
            left.push({
              xNum: pts[i].xNum + ((-unitEast) * corridorHalfWidthMeters),
              yNum: pts[i].yNum + (unitNorth * corridorHalfWidthMeters)
            });
            right.push({
              xNum: pts[i].xNum - ((-unitEast) * corridorHalfWidthMeters),
              yNum: pts[i].yNum - (unitNorth * corridorHalfWidthMeters)
            });
          }

          addLineFeature(left, { stroke: '#3c7cc0', lineWidth: 1.9, dashed: true });
          addLineFeature(right, { stroke: '#3c7cc0', lineWidth: 1.9, dashed: true });
        });
      }

      addGroupedLines(mapOverlays.faorLines, '#1f9fd0');
      addGroupedLines(mapOverlays.flotLines, '#c74b4b');
      addCorridorBounds(mapOverlays.corridors);

      (Array.isArray(mapOverlays.destinationPoints) ? mapOverlays.destinationPoints : []).forEach(function(p){
        addPointFeature(p, {
          kind: 'destination',
          label: String((p && p.label) || '').trim(),
          destinationSubtype: String((p && p.subtype) || '').toLowerCase(),
          fill: '#d4a42f',
          stroke: '#7a5a14',
          textColor: '#6a4f16',
          radius: 4.4,
        });
      });

      (Array.isArray(mapOverlays.threatPoints) ? mapOverlays.threatPoints : []).forEach(function(p){
        addPointFeature(p, {
          kind: 'threat',
          label: String((p && p.label) || '').trim(),
          fill: '#bc3e3e',
          stroke: '#8b2d2d',
          textColor: '#8b2d2d',
          radius: 4.2,
          ring: !!(p && p.ring),
          radiusMeters: Number(p && p.radiusMeters),
        });
      });

      (Array.isArray(mapOverlays.capPoints) ? mapOverlays.capPoints : []).forEach(function(p){
        addPointFeature(p, {
          kind: 'cap',
          label: String((p && p.label) || '').trim(),
          fill: '#2f5fa7',
          stroke: '#2f5fa7',
          textColor: '#2f5fa7',
          radius: 3.8,
          course: Number(p && p.course),
          lengthMeters: Number(p && p.lengthMeters),
          diameterMeters: Number(p && p.diameterMeters),
          turnDirection: String((p && p.turnDirection) || '').trim(),
        });
      });

      if (bullseyePoint){
        addPointFeature(bullseyePoint, {
          kind: 'bullseye',
          label: String((bullseyePoint && bullseyePoint.label) || 'BULLSEYE'),
          fill: '#f0c544',
          stroke: '#7a6420',
          textColor: '#5f4b1b',
          radius: 5,
        });
      }

      (Array.isArray(mapOverlays.airfields) ? mapOverlays.airfields : []).forEach(function(p){
        const isVfr = !!(p && p.isVfr);
        const stroke = isVfr ? '#4aa360' : '#3a8fd0';
        const airfieldAsset = {
          callsign: String((p && p.callsign) || '').trim(),
          name: String((p && p.name) || (p && p.label) || '').trim(),
          category: 'ATC',
          xNum: Number(p && p.xNum),
          yNum: Number(p && p.yNum),
        };
        addPointFeature(p, {
          kind: 'airfield',
          group: 'asset',
          assetKey: makeMapAssetSelectionKey(airfieldAsset),
          label: String((p && p.icao) || (p && p.label) || 'AF'),
          airfieldType: String((p && p.type) || 'airport').toLowerCase(),
          airfieldMilitary: !!(p && p.isMilitary),
          isVfr: isVfr,
          category: 'ATC',
          callsign: String((p && p.callsign) || '').trim(),
          name: String((p && p.name) || '').trim(),
          typeName: String((p && p.typeName) || '').trim(),
          frequency: String((p && p.frequency) || '').trim(),
          altFrequencies: Array.isArray(p && p.altFrequencies) ? p.altFrequencies.slice(0) : [],
          tacan: String((p && p.tacan) || '').trim(),
          mpClientCallsign: String((p && p.mpClientCallsign) || '').trim(),
          altFeet: Number(p && p.altFeet),
          fill: '#ffffff',
          stroke: stroke,
          textColor: stroke,
          radius: 5,
        });
      });

      const selected = getActiveFlightPlanSelection(model);
      const rawAssets = getMudMapAssets(model, dlinkOnEnabled);
      rawAssets.forEach(function(asset){
        const category = String((asset && asset.category) || '').toUpperCase();
        if (category === 'ATC') return;
        const isPlayer = category === 'PLAYER';
        const callsign = String((asset && asset.callsign) || '').trim();
        const label = callsign || String((asset && asset.name) || category || 'ASSET').trim();
        const assetKey = makeMapAssetSelectionKey(asset);
        addPointFeature(asset, {
          kind: isPlayer ? 'player' : 'asset',
          group: isPlayer ? 'player' : 'asset',
          label: label,
          fill: isPlayer ? '#2f9e56' : '#5a7ea5',
          stroke: isPlayer ? '#1e6a39' : '#385676',
          textColor: isPlayer ? '#1e6a39' : '#1f3550',
          radius: isPlayer ? 5 : 3.6,
          assetKind: getMudMapAssetKind(asset),
          assetKey: assetKey,
          callsign: String((asset && asset.callsign) || '').trim(),
          name: String((asset && asset.name) || '').trim(),
          category: category,
          typeName: String((asset && asset.typeName) || '').trim(),
          frequency: String((asset && asset.frequency) || '').trim(),
          tacan: String((asset && asset.tacan) || '').trim(),
          mpClientCallsign: String((asset && asset.mpClientCallsign) || '').trim(),
          altFeet: Number(asset && asset.altFeet),
        });
      });

      if (!pointsForBounds.length){
        const centerOnly = getTheatreCenterLonLat(theatre);
        if (!centerOnly) {
          openFreeMapLastPayloadStatus = 'no-converted-points';
          return null;
        }
        openFreeMapLastPayloadStatus = 'base-only/no-overlay-points';
        return {
          selected: selected,
          theatre: theatre,
          centerLon: Number(centerOnly[0]),
          centerLat: Number(centerOnly[1]),
          bounds: [Number(centerOnly[0]) - 1.0, Number(centerOnly[1]) - 1.0, Number(centerOnly[0]) + 1.0, Number(centerOnly[1]) + 1.0],
          features: [],
        };
      }

      const lons = pointsForBounds.map(function(c){ return Number(c[0]); }).filter(function(v){ return isFinite(v); });
      const lats = pointsForBounds.map(function(c){ return Number(c[1]); }).filter(function(v){ return isFinite(v); });
      if (!lons.length || !lats.length){
        const centerOnly = getTheatreCenterLonLat(theatre);
        if (!centerOnly){
          openFreeMapLastPayloadStatus = 'invalid-bounds';
          return null;
        }
        openFreeMapLastPayloadStatus = 'base-only/invalid-bounds';
        return {
          selected: selected,
          theatre: theatre,
          centerLon: Number(centerOnly[0]),
          centerLat: Number(centerOnly[1]),
          bounds: [Number(centerOnly[0]) - 1.0, Number(centerOnly[1]) - 1.0, Number(centerOnly[0]) + 1.0, Number(centerOnly[1]) + 1.0],
          features: [],
        };
      }

      const minLon = Math.min.apply(null, lons);
      const maxLon = Math.max.apply(null, lons);
      const minLat = Math.min.apply(null, lats);
      const maxLat = Math.max.apply(null, lats);
      const centerLon = (minLon + maxLon) / 2;
      const centerLat = (minLat + maxLat) / 2;

      openFreeMapLastPayloadStatus = 'ok pts=' + String(pointsForBounds.length) + ' feat=' + String(features.length);
      return {
        selected: selected,
        theatre: theatre,
        centerLon: centerLon,
        centerLat: centerLat,
        bounds: [minLon, minLat, maxLon, maxLat],
        features: features,
      };
    }

    function buildMudMapSvg(waypoints, data, overlays, bullseyePoint){
      const rows = Array.isArray(waypoints) ? waypoints.filter(function(wp){
        return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum));
      }) : [];
      const mapOverlays = overlays && typeof overlays === 'object'
        ? overlays
        : { geolines: [], threatPoints: [], destinationPoints: [], faorLines: [], flotLines: [], capPoints: [], corridors: [] };
      const geolines = Array.isArray(mapOverlays.geolines) ? mapOverlays.geolines : [];
      const threatPoints = Array.isArray(mapOverlays.threatPoints) ? mapOverlays.threatPoints : [];
      const destinationPoints = Array.isArray(mapOverlays.destinationPoints) ? mapOverlays.destinationPoints : [];
      const airfields = Array.isArray(mapOverlays.airfields) ? mapOverlays.airfields : [];
      const faorLines = Array.isArray(mapOverlays.faorLines) ? mapOverlays.faorLines : [];
      const flotLines = Array.isArray(mapOverlays.flotLines) ? mapOverlays.flotLines : [];
      const capPoints = Array.isArray(mapOverlays.capPoints) ? mapOverlays.capPoints : [];
      const corridors = Array.isArray(mapOverlays.corridors) ? mapOverlays.corridors : [];

      const isNight = !!nightModeEnabled;
      const palette = isNight
        ? {
            bg: '#1a232c',
            line: '#8ba5bf',
            label: '#dbe8f5',
            north: '#c8d8e8',
            wpFill: '#3f79b4',
            wpStroke: '#79a4cb',
            ipFill: '#3f9a66',
            ipStroke: '#7fc39f',
            tgtFill: '#b45757',
            tgtStroke: '#d48b8b',
            homeRoof: '#8f7650',
            homeBase: '#b79b67',
            homeStroke: '#d9c29b',
            tkoFill: '#2f9e56',
            tkoStroke: '#86d2a1',
            raceFill: '#1f2a35',
            assetBlue: '#6eb1ff',
            assetBlueDark: '#2f6fb3',
            assetInfoBg: '#1a2734',
            assetInfoStroke: '#7fa6cc',
            assetInfoText: '#d4e6f8',
            markerFill: '#35223d',
            markerStroke: '#d79cff',
            markerLabel: '#d79cff',
            geoLine: '#bb8cff',
            geoLineLabel: '#dec7ff',
            faorLine: '#64d6ff',
            flotLine: '#ff8f8f',
            corridorLine: '#9cc8ff',
            capLine: '#7eb9ff',
            threatStroke: '#ff7b7b',
            threatFill: '#ff7b7b',
            threatLabel: '#ffd3d3',
            destFill: '#ffd37a',
            destStroke: '#8a6a21',
            destLabel: '#ffe5af',
            bullFill: '#ffd86d',
            bullStroke: '#7a6420',
            bullLabel: '#ffe7a9'
          }
        : {
            bg: '#ffffff',
            line: '#3d566e',
            label: '#1f2e3d',
            north: '#263748',
            wpFill: '#3a6ea5',
            wpStroke: '#244766',
            ipFill: '#2f7f4f',
            ipStroke: '#1e5535',
            tgtFill: '#a33d3d',
            tgtStroke: '#682626',
            homeRoof: '#735c2f',
            homeBase: '#b1945a',
            homeStroke: '#4c3d1f',
            tkoFill: '#2f9e56',
            tkoStroke: '#1e6a39',
            raceFill: '#ffffff',
            assetBlue: '#2d8fe3',
            assetBlueDark: '#1f5d93',
            assetInfoBg: '#f6f9fc',
            assetInfoStroke: '#6f879f',
            assetInfoText: '#1f3550',
            markerFill: '#f7edf8',
            markerStroke: '#8a2f99',
            markerLabel: '#8a2f99',
            geoLine: '#7a57b3',
            geoLineLabel: '#5a3d89',
            faorLine: '#1f9fd0',
            flotLine: '#c74b4b',
            corridorLine: '#3c7cc0',
            capLine: '#2f5fa7',
            threatStroke: '#bc3e3e',
            threatFill: '#bc3e3e',
            threatLabel: '#8b2d2d',
            destFill: '#d4a42f',
            destStroke: '#7a5a14',
            destLabel: '#6a4f16',
            bullFill: '#f0c544',
            bullStroke: '#7a6420',
            bullLabel: '#5f4b1b'
          };

      const bullseye = (bullseyePoint && isFinite(Number(bullseyePoint.xNum)) && isFinite(Number(bullseyePoint.yNum)))
        ? {
            xNum: Number(bullseyePoint.xNum),
            yNum: Number(bullseyePoint.yNum),
            label: String((bullseyePoint && bullseyePoint.label) || 'BULLSEYE').trim() || 'BULLSEYE'
          }
        : null;

      if (!rows.length && !threatPoints.length && !destinationPoints.length && !geolines.length && !faorLines.length && !flotLines.length && !capPoints.length && !corridors.length && !airfields.length){
        return '<div class=""fltPlanMessage"">No mappable waypoint coordinates found.</div>';
      }

      const width = 920;
      const height = 760;
      const pad = 54;

      const overlayPoints = [];
      geolines.forEach(function(p){ overlayPoints.push(p); });
      threatPoints.forEach(function(p){ overlayPoints.push(p); });
      destinationPoints.forEach(function(p){ overlayPoints.push(p); });
      capPoints.forEach(function(p){ overlayPoints.push(p); });
      airfields.forEach(function(p){ overlayPoints.push(p); });
      if (bullseye) overlayPoints.push(bullseye);
      function pushLineGroupPoints(lineGroups){
        (Array.isArray(lineGroups) ? lineGroups : []).forEach(function(group){
          (Array.isArray(group && group.points) ? group.points : []).forEach(function(p){ overlayPoints.push(p); });
        });
      }
      pushLineGroupPoints(faorLines);
      pushLineGroupPoints(flotLines);
      pushLineGroupPoints(corridors);

      const allPoints = rows.concat(overlayPoints);
      const eastValues = allPoints.map(function(wp){ return Number(wp.yNum); }).filter(function(v){ return isFinite(v); });
      const northValues = allPoints.map(function(wp){ return Number(wp.xNum); }).filter(function(v){ return isFinite(v); });
      if (!eastValues.length || !northValues.length){
        return '<div class=""fltPlanMessage"">No mappable waypoint coordinates found.</div>';
      }
      const minEast = Math.min.apply(null, eastValues);
      const maxEast = Math.max.apply(null, eastValues);
      const minNorth = Math.min.apply(null, northValues);
      const maxNorth = Math.max.apply(null, northValues);

      const spanEast = Math.max(1, maxEast - minEast);
      const spanNorth = Math.max(1, maxNorth - minNorth);
      const scaleX = (width - (pad * 2)) / spanEast;
      const scaleY = (height - (pad * 2)) / spanNorth;
      const scale = Math.min(scaleX, scaleY);
      const drawW = spanEast * scale;
      const drawH = spanNorth * scale;
      const offsetX = (width - drawW) / 2;
      const offsetY = (height - drawH) / 2;

      function mapPt(wp){
        const north = Number(wp.xNum);
        const east = Number(wp.yNum);
        const sx = offsetX + ((east - minEast) * scale);
        const sy = offsetY + ((maxNorth - north) * scale);
        return { x: sx, y: sy };
      }

      const mapped = rows.map(function(wp){
        const p = mapPt(wp);
        return { wp: wp, x: p.x, y: p.y, kind: getMudMapPointType(wp) };
      });

      const byStep = {};
      mapped.forEach(function(m){ byStep[String(m.wp.step)] = m; });
      const segments = getMudMapSegments(rows)
        .map(function(seg){
          return {
            from: byStep[String(seg.from.step)],
            to: byStep[String(seg.to.step)],
            dashed: !!seg.dashed
          };
        })
        .filter(function(seg){ return !!(seg.from && seg.to); });

      const lineEls = segments.map(function(seg){
        return '<line x1=""' + seg.from.x.toFixed(1) + '"" y1=""' + seg.from.y.toFixed(1) + '"" x2=""' + seg.to.x.toFixed(1) + '"" y2=""' + seg.to.y.toFixed(1) + '"" stroke=""' + palette.line + '"" stroke-width=""2""' + (seg.dashed ? ' stroke-dasharray=""8 6""' : '') + ' />';
      });

      const geoMapped = geolines.map(function(p){
        const m = mapPt(p);
        return { p: p, x: m.x, y: m.y };
      });
      const geoLineKeys = ['L1', 'L2', 'L3', 'L4'];
      const geoLineEls = [];
      geoLineKeys.forEach(function(lineKey){
        const group = geoMapped
          .filter(function(m){
            const flags = (m && m.p && Array.isArray(m.p.lineFlags)) ? m.p.lineFlags : [];
            return flags.indexOf(lineKey) >= 0;
          })
          .sort(function(a, b){
            const an = Number(a && a.p && a.p.number);
            const bn = Number(b && b.p && b.p.number);
            if (isFinite(an) && isFinite(bn) && an !== bn) return an - bn;
            return 0;
          });
        for (let i = 1; i < group.length; i++){
          const a = group[i - 1];
          const b = group[i];
          geoLineEls.push('<line x1=""' + a.x.toFixed(1) + '"" y1=""' + a.y.toFixed(1) + '"" x2=""' + b.x.toFixed(1) + '"" y2=""' + b.y.toFixed(1) + '"" stroke=""' + palette.geoLine + '"" stroke-width=""2.2"" stroke-dasharray=""5 4"" />');
        }
      });
      const geoPointEls = geoMapped.map(function(m){
        return '<circle cx=""' + m.x.toFixed(1) + '"" cy=""' + m.y.toFixed(1) + '"" r=""3.8"" fill=""' + palette.geoLine + '"" />';
      });

      function renderLineGroups(lineGroups, strokeColor, dashPattern, strokeWidth){
        const els = [];
        (Array.isArray(lineGroups) ? lineGroups : []).forEach(function(group){
          const mappedGroup = (Array.isArray(group && group.points) ? group.points : [])
            .map(function(p){
              const m = mapPt(p);
              return { p: p, x: m.x, y: m.y };
            })
            .sort(function(a, b){
              const an = Number(a && a.p && a.p.number);
              const bn = Number(b && b.p && b.p.number);
              if (isFinite(an) && isFinite(bn) && an !== bn) return an - bn;
              return 0;
            });
          for (let i = 1; i < mappedGroup.length; i++){
            const a = mappedGroup[i - 1];
            const b = mappedGroup[i];
            els.push('<line x1=""' + a.x.toFixed(1) + '"" y1=""' + a.y.toFixed(1) + '"" x2=""' + b.x.toFixed(1) + '"" y2=""' + b.y.toFixed(1) + '"" stroke=""' + strokeColor + '"" stroke-width=""' + strokeWidth + '""' + (dashPattern ? (' stroke-dasharray=""' + dashPattern + '""') : '') + ' />');
          }
        });
        return els;
      }

      function renderCorridorBounds(corridorGroups){
        const els = [];
        const corridorHalfWidthMeters = 5000;
        (Array.isArray(corridorGroups) ? corridorGroups : []).forEach(function(group){
          const pts = (Array.isArray(group && group.points) ? group.points : [])
            .map(function(p){
              return {
                xNum: Number(p && p.xNum),
                yNum: Number(p && p.yNum),
                number: Number(p && p.number)
              };
            })
            .filter(function(p){ return isFinite(p.xNum) && isFinite(p.yNum); })
            .sort(function(a, b){
              if (isFinite(a.number) && isFinite(b.number) && a.number !== b.number) return a.number - b.number;
              return 0;
            });
          if (pts.length < 2) return;

          const left = [];
          const right = [];
          for (let i = 0; i < pts.length; i++){
            const prev = pts[Math.max(0, i - 1)];
            const next = pts[Math.min(pts.length - 1, i + 1)];
            const dNorth = Number(next.xNum) - Number(prev.xNum);
            const dEast = Number(next.yNum) - Number(prev.yNum);
            const len = Math.sqrt((dNorth * dNorth) + (dEast * dEast));
            if (!isFinite(len) || len <= 0){
              left.push({ xNum: pts[i].xNum, yNum: pts[i].yNum });
              right.push({ xNum: pts[i].xNum, yNum: pts[i].yNum });
              continue;
            }
            const unitEast = dEast / len;
            const unitNorth = dNorth / len;
            const leftNorth = pts[i].xNum + ((-unitEast) * corridorHalfWidthMeters);
            const leftEast = pts[i].yNum + (unitNorth * corridorHalfWidthMeters);
            const rightNorth = pts[i].xNum - ((-unitEast) * corridorHalfWidthMeters);
            const rightEast = pts[i].yNum - (unitNorth * corridorHalfWidthMeters);
            left.push({ xNum: leftNorth, yNum: leftEast });
            right.push({ xNum: rightNorth, yNum: rightEast });
          }

          function makePolyline(points){
            const mappedPts = points.map(function(p){ return mapPt(p); });
            return mappedPts.map(function(p){ return p.x.toFixed(1) + ',' + p.y.toFixed(1); }).join(' ');
          }

          els.push('<polyline points=""' + makePolyline(left) + '"" fill=""none"" stroke=""' + palette.corridorLine + '"" stroke-width=""1.9"" stroke-dasharray=""8 5"" />');
          els.push('<polyline points=""' + makePolyline(right) + '"" fill=""none"" stroke=""' + palette.corridorLine + '"" stroke-width=""1.9"" stroke-dasharray=""8 5"" />');
        });
        return els;
      }

      const faorEls = renderLineGroups(faorLines, palette.faorLine, '6 4', '2.2');
      const flotEls = renderLineGroups(flotLines, palette.flotLine, '6 4', '2.2');
      const corridorEls = renderCorridorBounds(corridors);

      const capEls = capPoints.map(function(cap){
        const anchor = mapPt(cap);
        const courseDeg = isFinite(Number(cap && cap.course)) ? Number(cap.course) : 0;
        const courseRad = courseDeg * (Math.PI / 180);
        const capLengthMeters = Math.max(4000, isFinite(Number(cap && cap.lengthMeters)) ? Number(cap.lengthMeters) : 12000);
        const capDiameterMeters = Math.max(2000, isFinite(Number(cap && cap.diameterMeters)) ? Number(cap && cap.diameterMeters) : 6000);
        const widthPx = (capLengthMeters + capDiameterMeters) * scale;
        const heightPx = capDiameterMeters * scale;
        const radiusPx = heightPx / 2;
        const turnDir = String((cap && cap.turnDirection) || '').trim().toUpperCase();
        const isRightPattern = turnDir.indexOf('RIGHT') >= 0;
        const angleDeg = (courseDeg - 90) + (isRightPattern ? 0 : 180);
        const angleRad = angleDeg * (Math.PI / 180);

        const localAnchorX = isRightPattern ? ((widthPx / 2) - radiusPx) : (-(widthPx / 2) + radiusPx);
        const localAnchorY = -(heightPx / 2);
        const rotAnchorX = (localAnchorX * Math.cos(angleRad)) - (localAnchorY * Math.sin(angleRad));
        const rotAnchorY = (localAnchorX * Math.sin(angleRad)) + (localAnchorY * Math.cos(angleRad));
        const centerX = anchor.x - rotAnchorX;
        const centerY = anchor.y - rotAnchorY;

        const label = escapeHtml(String((cap && cap.label) || ('CAP ' + String((cap && cap.number) || ''))));
        const tx = (anchor.x + (Math.sin(courseRad) * 10)).toFixed(1);
        const ty = (anchor.y - (Math.cos(courseRad) * 10)).toFixed(1);

        return '<g>'
          + '<rect x=""' + (centerX - (widthPx / 2)).toFixed(1) + '"" y=""' + (centerY - (heightPx / 2)).toFixed(1) + '"" width=""' + widthPx.toFixed(1) + '"" height=""' + heightPx.toFixed(1) + '"" rx=""' + radiusPx.toFixed(1) + '"" ry=""' + radiusPx.toFixed(1) + '"" fill=""none"" stroke=""' + palette.capLine + '"" stroke-width=""2.0"" transform=""rotate(' + angleDeg.toFixed(1) + ' ' + centerX.toFixed(1) + ' ' + centerY.toFixed(1) + ')"" />'
          + '<circle cx=""' + anchor.x.toFixed(1) + '"" cy=""' + anchor.y.toFixed(1) + '"" r=""2.9"" fill=""' + palette.capLine + '"" />'
          + '<text x=""' + tx + '"" y=""' + ty + '"" font-size=""10"" fill=""' + palette.capLine + '"" font-weight=""700"">' + label + '</text>'
          + '</g>';
      });

      const threatMapped = threatPoints.map(function(p){
        const m = mapPt(p);
        return {
          p: p,
          x: m.x,
          y: m.y,
          radiusPx: Math.max(4, (Number(p.radiusMeters) || 0) * scale)
        };
      });
      const threatEls = threatMapped.map(function(m){
        const label = escapeHtml(String((m && m.p && m.p.label) || 'THR'));
        const ring = (m && m.p && m.p.ring && m.radiusPx > 0)
          ? ('<circle cx=""' + m.x.toFixed(1) + '"" cy=""' + m.y.toFixed(1) + '"" r=""' + m.radiusPx.toFixed(1) + '"" fill=""none"" stroke=""' + palette.threatStroke + '"" stroke-width=""1.4"" stroke-dasharray=""7 5"" />')
          : '';
        const cross = '<line x1=""' + (m.x - 7).toFixed(1) + '"" y1=""' + m.y.toFixed(1) + '"" x2=""' + (m.x + 7).toFixed(1) + '"" y2=""' + m.y.toFixed(1) + '"" stroke=""' + palette.threatStroke + '"" stroke-width=""1.6"" />'
          + '<line x1=""' + m.x.toFixed(1) + '"" y1=""' + (m.y - 7).toFixed(1) + '"" x2=""' + m.x.toFixed(1) + '"" y2=""' + (m.y + 7).toFixed(1) + '"" stroke=""' + palette.threatStroke + '"" stroke-width=""1.6"" />';
        const txt = '<text x=""' + (m.x + 9).toFixed(1) + '"" y=""' + (m.y + 4).toFixed(1) + '"" font-size=""10"" fill=""' + palette.threatLabel + '"" font-weight=""700"">' + label + '</text>';
        return '<g>' + ring + cross + txt + '</g>';
      });

      const destinationMapped = destinationPoints.map(function(p){
        const m = mapPt(p);
        return { p: p, x: m.x, y: m.y };
      });
      const destinationEls = destinationMapped.map(function(m){
        const label = escapeHtml(String((m && m.p && m.p.label) || 'DEST'));
        const subtype = String((m && m.p && m.p.subtype) || '').toLowerCase();
        if (subtype === 'lantirn'){
          return '<g>'
            + '<line x1=""' + (m.x - 5).toFixed(1) + '"" y1=""' + m.y.toFixed(1) + '"" x2=""' + (m.x + 5).toFixed(1) + '"" y2=""' + m.y.toFixed(1) + '"" stroke=""' + palette.destStroke + '"" stroke-width=""1.5"" />'
            + '<line x1=""' + m.x.toFixed(1) + '"" y1=""' + (m.y - 5).toFixed(1) + '"" x2=""' + m.x.toFixed(1) + '"" y2=""' + (m.y + 5).toFixed(1) + '"" stroke=""' + palette.destStroke + '"" stroke-width=""1.5"" />'
            + '<text x=""' + (m.x + 9).toFixed(1) + '"" y=""' + (m.y + 4).toFixed(1) + '"" font-size=""10"" fill=""' + palette.destLabel + '"" font-weight=""700"">' + label + '</text>'
            + '</g>';
        }
        const p1 = m.x.toFixed(1) + ',' + (m.y - 7).toFixed(1);
        const p2 = (m.x - 7).toFixed(1) + ',' + m.y.toFixed(1);
        const p3 = m.x.toFixed(1) + ',' + (m.y + 7).toFixed(1);
        const p4 = (m.x + 7).toFixed(1) + ',' + m.y.toFixed(1);
        return '<g><polygon points=""' + p1 + ' ' + p2 + ' ' + p3 + ' ' + p4 + '"" fill=""' + palette.destFill + '"" stroke=""' + palette.destStroke + '"" stroke-width=""1.5"" /><text x=""' + (m.x + 9).toFixed(1) + '"" y=""' + (m.y + 4).toFixed(1) + '"" font-size=""10"" fill=""' + palette.destLabel + '"" font-weight=""700"">' + label + '</text></g>';
      });

      const airfieldMapped = airfields
        .map(function(p){
          const north = Number(p && p.xNum);
          const east = Number(p && p.yNum);
          if (!isFinite(north) || !isFinite(east)) return null;
          const m = mapPt({ xNum: north, yNum: east });
          return { p: p, x: m.x, y: m.y };
        })
        .filter(function(m){ return !!m; });
      const preselectedAssetKey = getMapSelectedAssetKeyBySelection(getActiveFlightPlanSelection((typeof data === 'undefined' ? null : data)));

      const airfieldEls = airfieldMapped.map(function(m){
        const stroke = String((m && m.p && m.p.isVfr) ? '#4aa360' : '#3a8fd0');
        const labelColor = String((m && m.p && m.p.isVfr) ? '#1e6b3d' : '#1d4f87');
        const type = String((m && m.p && m.p.type) || 'airport').toLowerCase();
        const military = !!(m && m.p && m.p.isMilitary);
        const selectionAsset = {
          callsign: String((m && m.p && m.p.callsign) || '').trim(),
          name: String((m && m.p && m.p.name) || (m && m.p && m.p.label) || '').trim(),
          category: 'ATC',
          xNum: Number(m && m.p && m.p.xNum),
          yNum: Number(m && m.p && m.p.yNum),
        };
        const selectionKey = makeMapAssetSelectionKey(selectionAsset);
        const isSelected = !!selectionKey && !!preselectedAssetKey && selectionKey === preselectedAssetKey;
        const label = escapeHtml(String((m && m.p && (m.p.icao || m.p.label)) || 'AF'));
        const r = 6.5;
        let selectedInfoBlock = '';
        if (isSelected){
          const infoLines = buildSelectedAssetInfoLines({
            category: 'ATC',
            typeName: String((m && m.p && m.p.typeName) || '').trim(),
            frequency: String((m && m.p && m.p.frequency) || '').trim(),
            altFrequencies: Array.isArray(m && m.p && m.p.altFrequencies) ? m.p.altFrequencies : [],
            tacan: String((m && m.p && m.p.tacan) || '').trim(),
            mpClientCallsign: String((m && m.p && m.p.mpClientCallsign) || '').trim(),
          });
          if (infoLines.length){
            const fontSize = 10;
            const lineHeight = 12;
            const padX = 5;
            const padY = 4;
            const maxChars = infoLines.reduce(function(max, line){ return Math.max(max, String(line || '').length); }, 0);
            const boxWidth = Math.max(120, Math.min(300, (maxChars * 6.2) + (padX * 2)));
            const boxHeight = (infoLines.length * lineHeight) + (padY * 2);
            let boxX = m.x + 12;
            let boxY = m.y + 8;
            if ((boxX + boxWidth) > (width - 4)) boxX = m.x - boxWidth - 12;
            if ((boxY + boxHeight) > (height - 4)) boxY = m.y - boxHeight - 12;
            const textRows = infoLines.map(function(line, idx){
              const txRow = (boxX + padX).toFixed(1);
              const tyRow = (boxY + padY + (lineHeight * (idx + 1)) - 2).toFixed(1);
              return '<text x=""' + txRow + '"" y=""' + tyRow + '"" font-size=""' + fontSize + '"" fill=""' + palette.assetInfoText + '"" font-weight=""700"">' + escapeHtml(String(line)) + '</text>';
            }).join('');
            selectedInfoBlock = '<g>'
              + '<rect x=""' + boxX.toFixed(1) + '"" y=""' + boxY.toFixed(1) + '"" width=""' + boxWidth.toFixed(1) + '"" height=""' + boxHeight.toFixed(1) + '"" rx=""3"" ry=""3"" fill=""' + palette.assetInfoBg + '"" stroke=""' + palette.assetInfoStroke + '"" stroke-width=""1.1"" />'
              + textRows
              + '</g>';
          }
        }
        let icon = '<g data-map-asset-key=""' + encodeURIComponent(selectionKey) + '"" data-map-asset-category=""ATC"" style=""cursor:pointer"">'
          + (isSelected ? ('<circle cx=""' + m.x.toFixed(1) + '"" cy=""' + m.y.toFixed(1) + '"" r=""11"" fill=""none"" stroke=""#c94444"" stroke-width=""2.4"" />') : '')
          + '<circle cx=""' + m.x.toFixed(1) + '"" cy=""' + m.y.toFixed(1) + '"" r=""' + r.toFixed(1) + '"" fill=""none"" stroke=""' + stroke + '"" stroke-width=""2"" />';
        if (!military){
          for (let i = 0; i < 6; i++){
            const a = i * (Math.PI / 3.0);
            const x1 = m.x + (Math.cos(a) * (r + 0.5));
            const y1 = m.y + (Math.sin(a) * (r + 0.5));
            const x2 = m.x + (Math.cos(a) * (r + 2.2));
            const y2 = m.y + (Math.sin(a) * (r + 2.2));
            icon += '<line x1=""' + x1.toFixed(1) + '"" y1=""' + y1.toFixed(1) + '"" x2=""' + x2.toFixed(1) + '"" y2=""' + y2.toFixed(1) + '"" stroke=""' + stroke + '"" stroke-width=""1.2"" />';
          }
        }
        if (type === 'heliport'){
          icon += '<text x=""' + m.x.toFixed(1) + '"" y=""' + (m.y + 3.8).toFixed(1) + '"" text-anchor=""middle"" font-size=""8.8"" fill=""' + stroke + '"" font-weight=""800"">H</text>';
        } else if (type === 'seaplane'){
          icon += '<text x=""' + m.x.toFixed(1) + '"" y=""' + (m.y + 3.8).toFixed(1) + '"" text-anchor=""middle"" font-size=""9.0"" fill=""' + stroke + '"" font-weight=""700"">⚓</text>';
        }
        icon += '<text x=""' + (m.x + 11).toFixed(1) + '"" y=""' + (m.y + 4).toFixed(1) + '"" font-size=""12"" fill=""' + labelColor + '"" font-weight=""700"">' + label + '</text>';
        icon += selectedInfoBlock;
        icon += '</g>';
        return icon;
      });

      const bullseyeEls = bullseye
        ? (function(){
            const m = mapPt(bullseye);
            const ring1 = 11;
            const ring2 = 7;
            const ring3 = 3;
            const label = 'B/E';
            return '<g>'
              + '<circle cx=""' + m.x.toFixed(1) + '"" cy=""' + m.y.toFixed(1) + '"" r=""' + ring1 + '"" fill=""none"" stroke=""' + palette.bullStroke + '"" stroke-width=""1.6"" />'
              + '<circle cx=""' + m.x.toFixed(1) + '"" cy=""' + m.y.toFixed(1) + '"" r=""' + ring2 + '"" fill=""none"" stroke=""' + palette.bullStroke + '"" stroke-width=""1.4"" />'
              + '<circle cx=""' + m.x.toFixed(1) + '"" cy=""' + m.y.toFixed(1) + '"" r=""' + ring3 + '"" fill=""' + palette.bullFill + '"" stroke=""' + palette.bullStroke + '"" stroke-width=""1.2"" />'
              + '<line x1=""' + (m.x - 13).toFixed(1) + '"" y1=""' + m.y.toFixed(1) + '"" x2=""' + (m.x + 13).toFixed(1) + '"" y2=""' + m.y.toFixed(1) + '"" stroke=""' + palette.bullStroke + '"" stroke-width=""1.1"" />'
              + '<line x1=""' + m.x.toFixed(1) + '"" y1=""' + (m.y - 13).toFixed(1) + '"" x2=""' + m.x.toFixed(1) + '"" y2=""' + (m.y + 13).toFixed(1) + '"" stroke=""' + palette.bullStroke + '"" stroke-width=""1.1"" />'
              + '<text x=""' + (m.x + 14).toFixed(1) + '"" y=""' + (m.y - 10).toFixed(1) + '"" font-size=""11"" fill=""' + palette.bullLabel + '"" font-weight=""700"">' + label + '</text>'
              + '</g>';
          })()
        : '';

      function iconFor(m){
        const x = m.x.toFixed(1);
        const y = m.y.toFixed(1);
        function racetrack(colorHex, label){
          const w = 64;
          const h = 32;
          const rx = 14;
          return '<g><rect x=""' + (m.x - (w / 2)).toFixed(1) + '"" y=""' + (m.y - (h / 2)).toFixed(1) + '"" width=""' + w + '"" height=""' + h + '"" rx=""' + rx + '"" ry=""' + rx + '"" fill=""' + palette.raceFill + '"" stroke=""' + colorHex + '"" stroke-width=""2.2"" /><text x=""' + x + '"" y=""' + (m.y + 5.5).toFixed(1) + '"" text-anchor=""middle"" font-size=""14"" fill=""' + colorHex + '"" font-weight=""700"">' + label + '</text></g>';
        }
        if (m.kind === 'aar') return racetrack('#111111', 'AAR');
        if (m.kind === 'cap') return racetrack('#2f5fa7', 'CAP');
        if (m.kind === 'hld') return racetrack('#2f7f4f', 'HLD');
        if (m.kind === 'ip'){
          const s = 9;
          return '<rect x=""' + (m.x - s).toFixed(1) + '"" y=""' + (m.y - s).toFixed(1) + '"" width=""' + (s * 2) + '"" height=""' + (s * 2) + '"" fill=""' + palette.ipFill + '"" stroke=""' + palette.ipStroke + '"" stroke-width=""1.5"" />';
        }
        if (m.kind === 'tgt'){
          const p1 = x + ',' + (m.y - 10).toFixed(1);
          const p2 = (m.x - 10).toFixed(1) + ',' + (m.y + 8).toFixed(1);
          const p3 = (m.x + 10).toFixed(1) + ',' + (m.y + 8).toFixed(1);
          return '<polygon points=""' + p1 + ' ' + p2 + ' ' + p3 + '"" fill=""' + palette.tgtFill + '"" stroke=""' + palette.tgtStroke + '"" stroke-width=""1.5"" />';
        }
        if (m.kind === 'home' || m.kind === 'ldg'){
          const r = 10;
          const roofTop = x + ',' + (m.y - 12).toFixed(1);
          const roofL = (m.x - r).toFixed(1) + ',' + (m.y - 2).toFixed(1);
          const roofR = (m.x + r).toFixed(1) + ',' + (m.y - 2).toFixed(1);
          const baseX = (m.x - 8).toFixed(1);
          const baseY = (m.y - 2).toFixed(1);
          return '<polygon points=""' + roofTop + ' ' + roofL + ' ' + roofR + '"" fill=""' + palette.homeRoof + '"" stroke=""' + palette.homeStroke + '"" stroke-width=""1.5"" /><rect x=""' + baseX + '"" y=""' + baseY + '"" width=""16"" height=""12"" fill=""' + palette.homeBase + '"" stroke=""' + palette.homeStroke + '"" stroke-width=""1.5"" />';
        }
        if (m.kind === 'tko'){
          return '<circle cx=""' + x + '"" cy=""' + y + '"" r=""8"" fill=""' + palette.tkoFill + '"" stroke=""' + palette.tkoStroke + '"" stroke-width=""1.5"" />';
        }
        return '<circle cx=""' + x + '"" cy=""' + y + '"" r=""8"" fill=""' + palette.wpFill + '"" stroke=""' + palette.wpStroke + '"" stroke-width=""1.5"" />';
      }

      const pointEls = mapped.map(function(m){
        const step = escapeHtml(String(m.wp.step || '-'));
        const name = escapeHtml(String(m.wp.name || ''));
        const label = name && name !== '-' ? ('STP ' + step + ' ' + name) : ('STP ' + step);
        const tx = (m.x + (isFinite(Number(m.labelDx)) ? Number(m.labelDx) : 11)).toFixed(1);
        const ty = (m.y + (isFinite(Number(m.labelDy)) ? Number(m.labelDy) : -11)).toFixed(1);
        return iconFor(m)
          + '<text x=""' + tx + '"" y=""' + ty + '"" font-size=""11"" fill=""' + palette.label + '"" font-weight=""700"">' + label + '</text>';
      });

      function assetIconFor(a){
        const x = a.x;
        const y = a.y;
        const stroke = palette.assetBlueDark;
        const fill = palette.assetBlue;
        const category = String((a && a.asset && a.asset.category) || '').toUpperCase();
        if (category === 'PLAYER'){
          return '<g><circle cx=""' + x.toFixed(1) + '"" cy=""' + y.toFixed(1) + '"" r=""7.5"" fill=""#f2d76a"" stroke=""#7a6420"" stroke-width=""1.5"" /><line x1=""' + (x - 9).toFixed(1) + '"" y1=""' + y.toFixed(1) + '"" x2=""' + (x + 9).toFixed(1) + '"" y2=""' + y.toFixed(1) + '"" stroke=""#7a6420"" stroke-width=""1.2"" /><line x1=""' + x.toFixed(1) + '"" y1=""' + (y - 9).toFixed(1) + '"" x2=""' + x.toFixed(1) + '"" y2=""' + (y + 9).toFixed(1) + '"" stroke=""#7a6420"" stroke-width=""1.2"" /></g>';
        }
        const kind = getMudMapAssetKind(a.asset);
        if (kind === 'awacs'){
          return '<g><rect x=""' + (x - 8).toFixed(1) + '"" y=""' + (y - 6).toFixed(1) + '"" width=""16"" height=""12"" rx=""1.6"" fill=""' + fill + '"" stroke=""' + stroke + '"" stroke-width=""1.3"" /><line x1=""' + (x - 6).toFixed(1) + '"" y1=""' + y.toFixed(1) + '"" x2=""' + (x + 6).toFixed(1) + '"" y2=""' + y.toFixed(1) + '"" stroke=""' + stroke + '"" stroke-width=""1.2"" /><circle cx=""' + x.toFixed(1) + '"" cy=""' + (y - 9).toFixed(1) + '"" r=""2.4"" fill=""' + stroke + '"" /></g>';
        }
        if (kind === 'tanker'){
          const p1 = (x - 8).toFixed(1) + ',' + y.toFixed(1);
          const p2 = x.toFixed(1) + ',' + (y - 6).toFixed(1);
          const p3 = (x + 8).toFixed(1) + ',' + y.toFixed(1);
          const p4 = x.toFixed(1) + ',' + (y + 6).toFixed(1);
          return '<polygon points=""' + p1 + ' ' + p2 + ' ' + p3 + ' ' + p4 + '"" fill=""' + fill + '"" stroke=""' + stroke + '"" stroke-width=""1.3"" />';
        }
        if (kind === 'jtac'){
          const p1 = x.toFixed(1) + ',' + (y - 7).toFixed(1);
          const p2 = (x - 7).toFixed(1) + ',' + (y + 7).toFixed(1);
          const p3 = (x + 7).toFixed(1) + ',' + (y + 7).toFixed(1);
          return '<polygon points=""' + p1 + ' ' + p2 + ' ' + p3 + '"" fill=""' + fill + '"" stroke=""' + stroke + '"" stroke-width=""1.3"" />';
        }
        if (kind === 'rotary'){
          return '<g><circle cx=""' + x.toFixed(1) + '"" cy=""' + y.toFixed(1) + '"" r=""6"" fill=""' + fill + '"" stroke=""' + stroke + '"" stroke-width=""1.3"" /><line x1=""' + (x - 9).toFixed(1) + '"" y1=""' + y.toFixed(1) + '"" x2=""' + (x + 9).toFixed(1) + '"" y2=""' + y.toFixed(1) + '"" stroke=""' + stroke + '"" stroke-width=""1.2"" /><line x1=""' + x.toFixed(1) + '"" y1=""' + (y - 9).toFixed(1) + '"" x2=""' + x.toFixed(1) + '"" y2=""' + (y + 9).toFixed(1) + '"" stroke=""' + stroke + '"" stroke-width=""1.2"" /></g>';
        }
        if (kind === 'marker'){
          return '<g><circle cx=""' + x.toFixed(1) + '"" cy=""' + y.toFixed(1) + '"" r=""6.5"" fill=""' + palette.markerFill + '"" stroke=""' + palette.markerStroke + '"" stroke-width=""1.4"" /><line x1=""' + (x - 8).toFixed(1) + '"" y1=""' + y.toFixed(1) + '"" x2=""' + (x + 8).toFixed(1) + '"" y2=""' + y.toFixed(1) + '"" stroke=""' + palette.markerStroke + '"" stroke-width=""1.2"" /><line x1=""' + x.toFixed(1) + '"" y1=""' + (y - 8).toFixed(1) + '"" x2=""' + x.toFixed(1) + '"" y2=""' + (y + 8).toFixed(1) + '"" stroke=""' + palette.markerStroke + '"" stroke-width=""1.2"" /></g>';
        }
        return '<rect x=""' + (x - 7).toFixed(1) + '"" y=""' + (y - 5.5).toFixed(1) + '"" width=""14"" height=""11"" fill=""' + fill + '"" stroke=""' + stroke + '"" stroke-width=""1.3"" />';
      }

      function formatAssetFrequencyText(value){
        const raw = String(value || '').trim();
        if (!raw) return '';
        let s = raw;
        const dot = s.indexOf('.');
        if (dot >= 0) s = s.substring(0, dot);
        s = s.replace(/[^0-9]/g, '');
        if (!s) return raw;
        s = ('000000000' + s).slice(-9);
        const main = s.substring(0, 3);
        const decRaw = Number(s.substring(3, 6));
        if (!isFinite(decRaw)) return raw;
        const decRounded = Math.round(decRaw / 25.0) * 25;
        const dec = ('000' + String(Math.round(decRounded))).slice(-3);
        return main + '.' + dec;
      }

      function buildSelectedAssetInfoLines(asset){
        if (!asset || typeof asset !== 'object') return [];
        const lines = [];
        const category = String(asset.category || '').trim().toUpperCase();
        const typeText = String(asset.typeName || '').trim();
        const freqText = formatAssetFrequencyText(asset.frequency);
        const altFreqs = Array.isArray(asset.altFrequencies) ? asset.altFrequencies : [];
        const allFreqs = [asset.frequency].concat(altFreqs);
        const normalizedFreqs = allFreqs
          .map(function(v){ return formatAssetFrequencyText(v); })
          .filter(function(v){ return !!v && isFinite(Number(v)); })
          .map(function(v){ return { text: v, mhz: Number(v) }; });
        const uhf = normalizedFreqs.find(function(f){ return f.mhz >= 225 && f.mhz <= 399.975; });
        const vhf = normalizedFreqs.find(function(f){ return f.mhz >= 30 && f.mhz < 225; });
        const tacanText = String(asset.tacan || '').trim();
        const mpText = String(asset.mpClientCallsign || '').trim();

        if (typeText) lines.push('TYPE: ' + typeText);
        if (category === 'ATC'){
          if (uhf && uhf.text) lines.push('UHF: ' + uhf.text);
          if (vhf && vhf.text) lines.push('VHF: ' + vhf.text);
        } else if (freqText) lines.push('FREQ: ' + freqText);
        if (tacanText) lines.push('TACAN: ' + tacanText);
        if (mpText) lines.push('MP: ' + mpText);

        return lines;
      }

      const selected = getActiveFlightPlanSelection((typeof data === 'undefined' ? null : data));
      const rawAssets = getMudMapAssets((typeof data === 'undefined' ? null : data), dlinkOnEnabled);
      const selectedAssetKey = getMapSelectedAssetKeyBySelection(selected);
      const mapAssets = rawAssets
        .map(function(asset){
          const north = Number(asset.xNum);
          const east = Number(asset.yNum);
          if (!isFinite(north) || !isFinite(east)) return null;
          const category = String((asset && asset.category) || '').toUpperCase();
          if (category === 'ATC') return null;
          asset.xNum = north;
          asset.yNum = east;
          return asset;
        })
        .filter(function(asset){ return !!asset; })
        .map(function(asset){
          const p = mapPt(asset);
          const selectionKey = makeMapAssetSelectionKey(asset);
          return {
            asset: asset,
            x: p.x,
            y: p.y,
            selectionKey: selectionKey,
            isSelected: !!selectionKey && !!selectedAssetKey && selectionKey === selectedAssetKey,
          };
        });

      const assetEls = mapAssets.map(function(m){
        const isMapMarker = String((m.asset && m.asset.category) || '').toUpperCase() === 'MAP_MARKER';
        const callsignLabel = String(m.asset.callsign || '').replace(/([A-Za-z])(\d)/g, '$1 $2').replace(/\s{2,}/g, ' ').trim();
        const markerText = String(m.asset.markerText || '').trim();
        const markerDisplayId = Number(m.asset.markerDisplayId);
        const markerPrefix = isMapMarker
          ? (markerDisplayId > 0 ? ('MKR ' + String(markerDisplayId)) : 'MKR')
          : '';
        const markerLabel = isMapMarker
          ? (markerText ? (markerPrefix + ': ' + markerText) : markerPrefix)
          : '';
        const label = escapeHtml(markerLabel || callsignLabel || m.asset.name || m.asset.category || 'ASSET');
        const tx = (m.x + 10).toFixed(1);
        const ty = (m.y + 4).toFixed(1);
        const selectionKey = String(m.selectionKey || '');
        const selectionKeyEncoded = encodeURIComponent(selectionKey);
        const selectedRing = m.isSelected
          ? ('<circle cx=""' + m.x.toFixed(1) + '"" cy=""' + m.y.toFixed(1) + '"" r=""11"" fill=""none"" stroke=""#c94444"" stroke-width=""2.4"" />')
          : '';
        const labelColor = isMapMarker ? palette.markerLabel : palette.assetBlueDark;
        let selectedInfoBlock = '';
        if (m.isSelected && !isMapMarker){
          const infoLines = buildSelectedAssetInfoLines(m.asset);
          if (infoLines.length){
            const fontSize = 10;
            const lineHeight = 12;
            const padX = 5;
            const padY = 4;
            const maxChars = infoLines.reduce(function(max, line){ return Math.max(max, String(line || '').length); }, 0);
            const boxWidth = Math.max(120, Math.min(300, (maxChars * 6.2) + (padX * 2)));
            const boxHeight = (infoLines.length * lineHeight) + (padY * 2);
            let boxX = m.x + 12;
            let boxY = m.y + 8;
            if ((boxX + boxWidth) > (width - 4)) boxX = m.x - boxWidth - 12;
            if ((boxY + boxHeight) > (height - 4)) boxY = m.y - boxHeight - 12;
            const textRows = infoLines.map(function(line, idx){
              const txRow = (boxX + padX).toFixed(1);
              const tyRow = (boxY + padY + (lineHeight * (idx + 1)) - 2).toFixed(1);
              return '<text x=""' + txRow + '"" y=""' + tyRow + '"" font-size=""' + fontSize + '"" fill=""' + palette.assetInfoText + '"" font-weight=""700"">' + escapeHtml(String(line)) + '</text>';
            }).join('');
            selectedInfoBlock = '<g>'
              + '<rect x=""' + boxX.toFixed(1) + '"" y=""' + boxY.toFixed(1) + '"" width=""' + boxWidth.toFixed(1) + '"" height=""' + boxHeight.toFixed(1) + '"" rx=""3"" ry=""3"" fill=""' + palette.assetInfoBg + '"" stroke=""' + palette.assetInfoStroke + '"" stroke-width=""1.1"" />'
              + textRows
              + '</g>';
          }
        }
        return '<g data-map-asset-key=""' + selectionKeyEncoded + '"" data-map-asset-category=""' + escapeHtml(String(m.asset.category || '')) + '"" style=""cursor:pointer"">'
          + selectedRing
          + assetIconFor(m)
          + '<text x=""' + tx + '"" y=""' + ty + '"" font-size=""10"" fill=""' + labelColor + '"" font-weight=""700"">' + label + '</text>'
          + selectedInfoBlock
          + '</g>';
      });

      const northArrow = [
        '<g transform=""translate(' + (width - 46) + ',44)"">',
        '<line x1=""0"" y1=""18"" x2=""0"" y2=""-10"" stroke=""' + palette.north + '"" stroke-width=""2"" />',
        '<polygon points=""0,-18 -6,-6 6,-6"" fill=""' + palette.north + '"" />',
        '<text x=""0"" y=""32"" text-anchor=""middle"" font-size=""12"" fill=""' + palette.north + '"" font-weight=""700"">N</text>',
        '</g>'
      ].join('');

      const mapView = getMapViewBySelection(selected);
      const zoom = clamp(isFinite(Number(mapView.zoom)) ? Number(mapView.zoom) : 1, 0.6, 4.0);
      const panX = isFinite(Number(mapView.panX)) ? Number(mapView.panX) : 0;
      const panY = isFinite(Number(mapView.panY)) ? Number(mapView.panY) : 0;

      return '<svg viewBox=""0 0 ' + width + ' ' + height + '"" class=""fltPlanPage3Canvas"" preserveAspectRatio=""xMidYMid meet"" data-map-canvas=""1"">'
        + '<rect x=""0"" y=""0"" width=""' + width + '"" height=""' + height + '"" fill=""' + palette.bg + '"" />'
        + '<g data-map-content=""1"" transform=""translate(' + panX.toFixed(1) + ' ' + panY.toFixed(1) + ') scale(' + zoom.toFixed(3) + ')"">'
        + corridorEls.join('')
        + lineEls.join('')
        + geoLineEls.join('')
        + faorEls.join('')
        + flotEls.join('')
        + capEls.join('')
        + threatEls.join('')
        + destinationEls.join('')
        + airfieldEls.join('')
        + bullseyeEls
        + assetEls.join('')
        + pointEls.join('')
        + geoPointEls.join('')
        + '</g>'
        + northArrow
        + '</svg>';
    }

    function formatDtcPage3Html(pageSwitcherHtml, waypoints, data, selected, overlays, root){
      let mapRows = applyTypeOverrides(Array.isArray(waypoints) ? waypoints.slice() : [], selected);
      let resolvedOverlays = overlays && typeof overlays === 'object'
        ? overlays
        : getDtcMapOverlays(root, getDtcRouteBySelection(selected), data);
      if (!resolvedOverlays || typeof resolvedOverlays !== 'object'){
        resolvedOverlays = {};
      }
      if (!Array.isArray(resolvedOverlays.airfields)){
        resolvedOverlays.airfields = (typeof buildMapAirfields === 'function')
          ? buildMapAirfields(data || latestData)
          : ((typeof BuildMapAirfields === 'function') ? BuildMapAirfields(data || latestData) : []);
      }
      const planState = getFlightPlanPlanState(selected);
      ensureDirectToStateValid(mapRows, planState);
      const deletedSet = getDeletedStepSet(planState);
      const skippedSet = getSkippedStepSet(mapRows, planState);
      mapRows = mapRows.filter(function(wp){
        const key = stepToKey(wp && wp.step);
        if (!key) return true;
        if (deletedSet[key]) return false;
        if (skippedSet[key]) return false;
        return true;
      });
      let html = '<div class=""fltPlanBoard"">';
      if (pageSwitcherHtml){
        html += '<div style=""margin:4px 0 6px 0;"">' + pageSwitcherHtml + '</div>';
      }
      const mapBackgroundEnabled = isMapBackgroundEnabledBySelection(selected);
      if (mapBackgroundEnabled){
        openFreeMapLastPayloadStatus = 'init';
      }
      const bullseyePoint = getMapBullseyePoint(root, mapRows, resolvedOverlays, data);
      const hasPayloadBuilder = typeof buildOpenFreeMapPayload === 'function';
      if (mapBackgroundEnabled && !hasPayloadBuilder){
        openFreeMapLastPayloadStatus = 'builder-missing';
      }
      const openFreeMapPayload = mapBackgroundEnabled && hasPayloadBuilder
        ? buildOpenFreeMapPayload(mapRows, data, resolvedOverlays, bullseyePoint)
        : null;
      if (mapBackgroundEnabled && !openFreeMapPayload && openFreeMapLastPayloadStatus === 'init'){
        openFreeMapLastPayloadStatus = 'builder-null';
      }
      const openFreeMapId = (openFreeMapPayload && typeof registerOpenFreeMapPayload === 'function')
        ? registerOpenFreeMapPayload(selected, openFreeMapPayload)
        : '';
      if (mapBackgroundEnabled && openFreeMapPayload && !openFreeMapId){
        openFreeMapLastPayloadStatus = 'register-failed';
      }
      const bgStatus = !mapBackgroundEnabled
        ? 'BG disabled'
        : (openFreeMapRuntimeFailed
            ? ('BG runtime unavailable' + (openFreeMapRuntimeErrorText ? (': ' + openFreeMapRuntimeErrorText) : ''))
            : (openFreeMapPayload
                ? ('BG ready [' + openFreeMapLastPayloadStatus + ']')
                : ('BG unavailable for current theatre/data (' + String((((data && data.Server) || {}).Theater) || lastKnownTheater || '?') + ')' + (openFreeMapLastPayloadStatus ? (' [' + openFreeMapLastPayloadStatus + ']') : ''))));
      html += '<div class=""controls fltPlanControls"" style=""margin:0 0 6px 0;""><button type=""button"" class=""fltPlanPageBtn"" data-map-zoom=""in"">Map In</button><button type=""button"" class=""fltPlanPageBtn"" data-map-zoom=""out"">Map Out</button><button type=""button"" class=""fltPlanPageBtn"" data-map-zoom=""reset"">Map Reset</button><button id=""mapBgToggleBtn"" type=""button"" class=""fltPlanPageBtn"" data-map-bg-toggle=""1"">BG ' + (mapBackgroundEnabled ? 'ON' : 'OFF') + '</button><span id=""mapBgStatus"" class=""fltPlanMapBgStatus"">' + escapeHtml(bgStatus) + '</span></div>';
      const selectedAssetKey = getMapSelectedAssetKeyBySelection(selected);
      const rawAssets = getMudMapAssets(data, dlinkOnEnabled);
      const airfieldAssets = (Array.isArray(resolvedOverlays && resolvedOverlays.airfields) ? resolvedOverlays.airfields : [])
        .map(function(a){
          return {
            callsign: String((a && a.callsign) || '').trim(),
            name: String((a && a.name) || (a && a.label) || '').trim(),
            category: 'ATC',
            typeName: String((a && a.typeName) || '').trim(),
            frequency: String((a && a.frequency) || '').trim(),
            altFrequencies: Array.isArray(a && a.altFrequencies) ? a.altFrequencies.slice(0) : [],
            tacan: String((a && a.tacan) || '').trim(),
            mpClientCallsign: String((a && a.mpClientCallsign) || '').trim(),
            xNum: Number(a && a.xNum),
            yNum: Number(a && a.yNum),
            altFeet: Number(a && a.altFeet),
          };
        })
        .filter(function(a){ return isFinite(Number(a.xNum)) && isFinite(Number(a.yNum)); });
      const selectionPool = rawAssets.concat(airfieldAssets);
      const markerCount = rawAssets
        .filter(function(a){ return String((a && a.category) || '').toUpperCase() === 'MAP_MARKER'; })
        .length;
      let selectedAsset = null;
      if (selectedAssetKey){
        selectedAsset = selectionPool.find(function(a){ return makeMapAssetSelectionKey(a) === selectedAssetKey; }) || null;
        if (!selectedAsset){
          setMapSelectedAssetKeyBySelection(selected, '');
        }
      }

      html += '<div class=""fltPlanPage3Wrap"">';
      const mapSvg = buildMudMapSvg(mapRows, data, resolvedOverlays, bullseyePoint);
      if (openFreeMapId){
        html += '<div class=""fltPlanPage3Canvas fltPlanOpenMapWrap"" data-openfreemap-wrap=""' + escapeHtml(openFreeMapId) + '"">';
        html += '<div class=""fltPlanOpenMapHost"" data-openfreemap-map-id=""' + escapeHtml(openFreeMapId) + '"" data-openfreemap-selection=""' + escapeHtml(getFlightPlanEtaStartKey(selected)) + '""></div>';
        html += '<div class=""fltPlanOpenMapFallback"">' + mapSvg + '</div>';
        html += '</div>';
      } else {
        html += mapSvg;
      }
      let readout = formatMapBraReadout(data, selectedAsset, bullseyePoint);
      if (markerCount > 0){
        readout += '   MKR ' + String(markerCount);
      }
      html += '<div id=""mapBraReadout"" class=""fltPlanPage3BraReadout"">' + escapeHtml(readout) + '</div>';
      html += '</div></div>';
      return html;
    }

    function extractJetFromCallsign(callsign){
      const text = String(callsign || '').trim();
      if (!text) return 0;

      let m = text.match(/(\d{2})\s*$/);
      if (m){
        const pair = String(m[1] || '');
        const jet = parseInt(pair.charAt(pair.length - 1), 10);
        if (isFinite(jet) && jet > 0 && jet <= 9) return jet;
      }

      m = text.match(/(\d)\s*$/);
      if (m){
        const jet = parseInt(String(m[1] || ''), 10);
        if (isFinite(jet) && jet > 0 && jet <= 9) return jet;
      }

      return 0;
    }

    function buildFlightRosterRows(data){
      const server = (data && data.Server) || {};
      const pilotName = String(server.PlayerUsername || '').trim();
      const pilotCallsign = String(server.PlayerCallsign || '').trim();
      const source = Array.isArray(server.FlightMembers) ? server.FlightMembers : [];
      const rows = [];
      const seen = {};

      function makeKey(callsign, pilot){
        const c = String(callsign || '').trim().toUpperCase();
        if (c) return c;
        return ('P:' + String(pilot || '').trim().toUpperCase());
      }

      function upsert(callsign, pilot, jet){
        const safeCall = String(callsign || '').trim();
        const safePilot = String(pilot || '').trim();
        const safeJet = isFinite(Number(jet)) ? Math.round(Number(jet)) : 0;
        if (!safeCall && !safePilot) return;

        const key = makeKey(safeCall, safePilot);
        if (!key) return;
        if (!seen[key]){
          seen[key] = { callsign: safeCall, pilot: safePilot, jet: safeJet };
          rows.push(seen[key]);
          return;
        }

        if (!seen[key].pilot && safePilot) seen[key].pilot = safePilot;
        if (!seen[key].callsign && safeCall) seen[key].callsign = safeCall;
        if ((!seen[key].jet || seen[key].jet <= 0) && safeJet > 0) seen[key].jet = safeJet;
      }

      if (pilotName || pilotCallsign){
        upsert(pilotCallsign, pilotName || 'PLAYER', extractJetFromCallsign(pilotCallsign) || 1);
      }

      source.forEach(function(member){
        if (!member || typeof member !== 'object') return;
        const callsign = String(member.Callsign || '').trim();
        const pilot = String(member.Pilot || '').trim();
        const jet = Number(member.Jet);
        upsert(callsign, pilot, isFinite(jet) ? Math.round(jet) : extractJetFromCallsign(callsign));
      });

      const usedJets = {};
      rows.forEach(function(r){
        if (isFinite(r.jet) && r.jet > 0) usedJets[r.jet] = true;
      });
      let fallbackJet = 1;
      rows.forEach(function(r){
        if (isFinite(r.jet) && r.jet > 0) return;
        while (usedJets[fallbackJet]) fallbackJet++;
        r.jet = fallbackJet;
        usedJets[fallbackJet] = true;
      });

      rows.sort(function(a, b){
        const aj = Number(a.jet) || 99;
        const bj = Number(b.jet) || 99;
        if (aj !== bj) return aj - bj;
        return String(a.callsign || '').localeCompare(String(b.callsign || ''));
      });

      return rows.slice(0, 8);
    }

    function formatFlightRosterHtml(data){
      const rows = buildFlightRosterRows(data);
      let html = '<div class=""fltPlanCrewWrap""><table class=""fltPlanCrewTable""><thead><tr><th>PILOT</th><th style=""width:80px;"">JET</th></tr></thead><tbody>';
      if (!rows.length){
        html += '<tr><td class=""fltPlanCrewPilot"">No flight members detected.</td><td class=""fltPlanCrewJet"">-</td></tr>';
      } else {
        rows.forEach(function(r){
          const pilot = String(r.pilot || '').trim() || (String(r.callsign || '').trim() ? ('AI / ' + String(r.callsign || '').trim()) : '-');
          html += '<tr><td class=""fltPlanCrewPilot"">' + escapeHtml(pilot) + '</td><td class=""fltPlanCrewJet"">' + escapeHtml(String(r.jet || '-')) + '</td></tr>';
        });
      }
      html += '</tbody></table></div>';
      return html;
    }

    function renderFlightPlanBoardHtml(selected, data, primaryRouteName, sourceLabel, sourceFileName, waypoints, cmdsBlockHtml, pageSwitcherHtml){
      const rows = Array.isArray(waypoints) ? waypoints.slice() : [];
      applyTypeOverrides(rows, selected);
      applyAltitudeAdjustments(rows, selected);
      applyEtaPlanToWaypoints(rows, selected);
      const timing = getFlightPlanTimingDisplay(selected);

      const server = (data && data.Server) || {};
      const callsign = safe(server.PlayerCallsign);
      const mission = safe(server.MissionTitle);
      const config = safe(server.Aircraft);
      const theatre = safe(server.Theater);
      const startRow = getFlightPlanStartRow(server, selected);
      const displayRows = startRow ? [startRow].concat(rows) : rows;
      applySpeedAdjustmentsToWaypoints(displayRows, selected);
      applyRouteTimeline(displayRows, selected);
      applyLockedTotPlan(displayRows, selected);
      applyDistancePlan(displayRows, selected);
      applyHeadingPlan(displayRows, theatre, selected);
      updateTotOverflyCapture(selected, displayRows);
      const etaHeading = hasTakeoffTimeBySelection(selected) ? 'ETA' : 'ETE';
      const planState = getFlightPlanPlanState(selected);
      ensureDirectToStateValid(displayRows, planState);
      const deletedSet = getDeletedStepSet(planState);
      const skippedSet = getSkippedStepSet(displayRows, planState);
      const directSourceKey = stepToKey(planState.directToSourceStep);
      const directTargetKey = stepToKey(planState.directToTargetStep);
      const rowActionKey = stepToKey(planState.rowActionStep);
      const rowActionMode = String(planState.rowActionMode || '').toLowerCase();
      const rowRevealKey = stepToKey(planState.rowRevealStep);
      const rowRevealMode = String(planState.rowRevealMode || '').toLowerCase();
      pruneExpiredSpeedRecommendations(planState);
      const coordDisplayMode = getNavlogCoordDisplayMode(selected);
      const coordHeaderText = (function(){
        if (coordDisplayMode === 'dms') return 'DMS';
        if (coordDisplayMode === 'ddm') return 'DDM';
        if (coordDisplayMode === 'mgrs') return 'MGRS';
        return 'X / Y';
      })();
      const timingLogRows = (planState && Array.isArray(planState.timingLog))
        ? planState.timingLog.slice()
        : [];
      const postFlightOpen = !!(planState && planState.postFlightOpen);
      const postFlightSummaryRows = buildPostFlightSummaryRows(selected, displayRows, timing);

      let html = '';
      html += '<div class=""fltPlanBoard"">';
      html += '<div class=""fltPlanHeaderGrid"">';
      html += '<div class=""fltPlanHeaderCell""><div class=""fltPlanHeaderCellLabel"">ROUTE</div><div class=""fltPlanHeaderCellValue"">' + escapeHtml(primaryRouteName) + '</div></div>';
      html += '<div class=""fltPlanHeaderCell""><div class=""fltPlanHeaderCellLabel"">CALL SIGN</div><div class=""fltPlanHeaderCellValue"">' + escapeHtml(callsign) + '</div></div>';
      html += '<div class=""fltPlanHeaderCell""><div class=""fltPlanHeaderCellLabel"">MISSION</div><div class=""fltPlanHeaderCellValue"">' + escapeHtml(mission) + '</div></div>';
      html += '<div class=""fltPlanHeaderCell""><div class=""fltPlanHeaderCellLabel"">CONFIG</div><div class=""fltPlanHeaderCellValue"">' + escapeHtml(config) + '</div></div>';
      html += '</div>';
      html += '<div class=""fltPlanHeaderGrid"">';
      html += '<div class=""fltPlanHeaderCell""><div class=""fltPlanHeaderCellLabel"">THEATRE</div><div class=""fltPlanHeaderCellValue"">' + escapeHtml(theatre) + '</div></div>';
      html += '<div class=""fltPlanHeaderCell""><div class=""fltPlanHeaderCellLabel"">SOURCE</div><div class=""fltPlanHeaderCellValue"">' + escapeHtml(sourceLabel || '-') + '</div></div>';
      html += '<div class=""fltPlanHeaderCell""><div class=""fltPlanHeaderCellLabel"">ROUTE FILE</div><div class=""fltPlanHeaderCellValue"">' + escapeHtml(sourceFileName || '-') + '</div></div>';
      html += '<div class=""fltPlanHeaderCell""><div class=""fltPlanHeaderCellLabel"">WAYPOINTS</div><div class=""fltPlanHeaderCellValue"">' + escapeHtml(String(rows.length)) + '</div></div>';
      html += '</div>';
      if (pageSwitcherHtml){
        html += '<div class=""fltPlanToolbar""><span class=""fltPlanToolbarLeft"">' + pageSwitcherHtml + '</span><span class=""fltPlanToolbarRight""><button type=""button"" class=""fltPlanPageBtn"" data-postflight-toggle=""1"">' + (postFlightOpen ? 'Hide POST FLT' : 'POST FLT') + '</button></span></div>';
      } else {
        html += '<div class=""fltPlanToolbar""><span class=""fltPlanToolbarLeft""></span><span class=""fltPlanToolbarRight""><button type=""button"" class=""fltPlanPageBtn"" data-postflight-toggle=""1"">' + (postFlightOpen ? 'Hide POST FLT' : 'POST FLT') + '</button></span></div>';
      }

      html += '<div class=""fltPlanTimeGrid"">';
      html += '<div class=""fltPlanTimeCell clickable"" data-tko-anchor=""STEP"" title=""Set STEP to current clock (Takeoff auto = STEP +35m)""><div class=""fltPlanTimeCellLabel"">STEP</div><div class=""fltPlanTimeCellValue"">' + escapeHtml(timing.step) + '</div></div>';
      html += '<div class=""fltPlanTimeCell clickable"" data-tko-anchor=""START"" title=""Set START to current clock (Takeoff auto = START +25m)""><div class=""fltPlanTimeCellLabel"">START</div><div class=""fltPlanTimeCellValue"">' + escapeHtml(timing.start) + '</div></div>';
      html += '<div class=""fltPlanTimeCell clickable"" data-tko-anchor=""TAXI"" title=""Set TAXI to current clock (Takeoff auto = TAXI +15m)""><div class=""fltPlanTimeCellLabel"">TAXI</div><div class=""fltPlanTimeCellValue"">' + escapeHtml(timing.taxi) + '</div></div>';
      html += '<div class=""fltPlanTimeCell clickable"" data-tko-anchor=""TAKEOFF"" title=""Set TAKEOFF to current clock""><div class=""fltPlanTimeCellLabel"">TAKEOFF</div><div class=""fltPlanTimeCellValue""><div class=""fltPlanAdjustCell""><button type=""button"" class=""fltPlanMiniBtn"" data-tko-adjust-sec=""-1"" title=""-1 second"">«</button><button type=""button"" class=""fltPlanMiniBtn"" data-tko-adjust=""-60"" title=""-1 minute"">◀</button><span class=""fltPlanSpdValue"">' + escapeHtml(timing.takeoff) + '</span><button type=""button"" class=""fltPlanMiniBtn"" data-tko-adjust=""60"" title=""+1 minute"">▶</button><button type=""button"" class=""fltPlanMiniBtn"" data-tko-adjust-sec=""1"" title=""+1 second"">»</button></div></div></div>';
      html += '<div class=""fltPlanTimeCell""><div class=""fltPlanTimeCellLabel"">TOT</div><div class=""fltPlanTimeCellValue""><div class=""fltPlanAdjustCell""><button type=""button"" class=""fltPlanMiniBtn"" data-tot-adjust-sec=""-1"" title=""-1 second"">«</button><button type=""button"" class=""fltPlanMiniBtn"" data-tot-adjust=""-60"" title=""-1 minute"">◀</button><span class=""fltPlanSpdValue"">' + escapeHtml(timing.tot) + '</span><button type=""button"" class=""fltPlanMiniBtn"" data-tot-adjust=""60"" title=""+1 minute"">▶</button><button type=""button"" class=""fltPlanMiniBtn"" data-tot-adjust-sec=""1"" title=""+1 second"">»</button></div></div></div>';
      html += '</div>';
      if (postFlightOpen){
        html += '<div class=""fltPlanInfoBlock"" style=""min-height:0; margin-bottom:8px;"">';
        html += '<div class=""fltPlanInfoTitle"">POST FLT</div>';
        html += '<div class=""fltPlanInfoBody"" style=""font-size:12px; line-height:1.2; max-height:220px;"">' + (postFlightSummaryRows.length ? postFlightSummaryRows.map(escapeHtml).join('<br>') : 'No events recorded yet.') + '</div>';
        html += '</div>';
      }

      html += formatFlightRosterHtml(data);

      html += '<div class=""fltPlanWpWrap"">';
      html += '<div class=""fltPlanWpTitle"">Route: ' + escapeHtml(primaryRouteName) + '</div>';
      html += '<div class=""fltPlanWpTableWrap"">';
      html += '<table class=""fltPlanWpTable"">';
      html += '<thead><tr><th style=""width:34px;"">STP</th><th style=""width:68px;"">TYPE</th><th style=""width:110px;"">NAME</th><th style=""width:78px;"">ALT</th><th style=""width:40px;"">HDG</th><th style=""width:86px;"">SPD</th><th style=""width:42px;"">DIST</th><th class=""fltPlanEtaHeader"" style=""width:90px;"" data-eta-header=""1"" title=""Click to set ETA start from current time"">' + etaHeading + '</th><th class=""fltPlanEtaHeader"" style=""width:210px;"" data-navlog-coord-cycle=""1"" title=""Click to cycle X/Y → DMS → DDM → MGRS"">' + escapeHtml(coordHeaderText) + '</th></tr></thead>';
      html += '<tbody>';

      if (!displayRows.length){
        html += '<tr><td colspan=""9"">No waypoints found.</td></tr>';
      } else {
        displayRows.forEach(function(wp){
          const stepKey = stepToKey(wp.step);
          const isStart = !!wp.isStart;
          const isDeleted = !!(stepKey && deletedSet[stepKey]);
          if (isDeleted) return;
          const isSkipped = !!(stepKey && skippedSet[stepKey]);
          const isDirectSource = !!(stepKey && directSourceKey && stepKey === directSourceKey);
          const isDirectTarget = !!(stepKey && directTargetKey && stepKey === directTargetKey);
          const isTargetCandidate = !!(rowActionMode === 'dir' && rowActionKey && stepKey && stepKey !== rowActionKey && !isStart);
          const rowClasses = [];
          if (isSkipped) rowClasses.push('fltPlanWpRowSkipped');
          if (isDirectSource) rowClasses.push('fltPlanWpRowDirectSource');
          if (isDirectTarget) rowClasses.push('fltPlanWpRowDirectTarget');
          if (isTargetCandidate) rowClasses.push('fltPlanWpRowTargetCandidate');
          if (!isStart){
            const showDir = (rowActionMode === 'dir' && rowActionKey === stepKey) || (rowRevealMode === 'dir' && rowRevealKey === stepKey);
            const showDel = (rowActionMode === 'del' && rowActionKey === stepKey) || (rowRevealMode === 'del' && rowRevealKey === stepKey);
            if (showDir) rowClasses.push('fltPlanWpRowActionDir');
            if (showDel) rowClasses.push('fltPlanWpRowActionDel');
          }
          const rowClassAttr = rowClasses.length ? ' class=""' + rowClasses.join(' ') + '""' : '';
          const rowDataAttrs = !isStart
            ? ' data-navlog-step=""' + escapeHtml(stepKey) + '"" data-navlog-row=""1""'
            : '';
          const lockChecked = !isStart && stepKey && (stepKey === stepToKey(planState.lockedStep)) ? ' checked' : '';
          const speedMode = getWaypointSpeedDisplayMode(planState, stepKey, wp.altFeet);
          const speedRec = (!wp.isStart && planState && planState.speedRecommendations) ? planState.speedRecommendations[stepKey] : null;
          const speedDisplay = speedRec
            ? formatWaypointSpeedDisplay(speedRec.kcas, speedMode, wp.altFeet)
            : formatWaypointSpeedDisplay(wp.spd, speedMode, wp.altFeet);
          const speedClickTitle = canUseMachDisplay(wp.altFeet)
            ? 'Click to toggle KCAS/MACH display'
            : 'KCAS only below FL280';
          const speedStepTitle = (speedMode === 'MACH' && canUseMachDisplay(wp.altFeet))
            ? 'Adjust by 0.01 Mach'
            : 'Adjust by 10 KCAS';
          html += '<tr' + rowClassAttr + rowDataAttrs + '>';
          if (isStart){
            html += '<td class=""fltPlanCellNum"">' + escapeHtml(wp.step) + '</td>';
          } else {
            const dirArmedClass = isNavlogRowActionArmed(selected, stepKey, 'dir') ? ' armed' : '';
            const delArmedClass = isNavlogRowActionArmed(selected, stepKey, 'del') ? ' armed' : '';
            html += '<td class=""fltPlanCellNum""><span class=""fltPlanStepCellWrap""><span>' + escapeHtml(wp.step) + '</span><button type=""button"" class=""fltPlanRowActionBtn dir' + dirArmedClass + '"" data-row-action=""dir"" data-row-step=""' + escapeHtml(stepKey) + '"">DIR TO</button><button type=""button"" class=""fltPlanRowActionBtn del' + delArmedClass + '"" data-row-action=""del"" data-row-step=""' + escapeHtml(stepKey) + '"">DEL STP</button></span></td>';
          }
          if (wp.isStart){
            html += '<td>' + escapeHtml(wp.type) + '</td>';
          } else {
            html += '<td><div class=""fltPlanAdjustCell""><button type=""button"" class=""fltPlanMiniBtn"" data-type-step=""' + escapeHtml(stepKey) + '"" data-type-current=""' + escapeHtml(wp.type) + '"" data-type-delta=""-1"">◀</button><span class=""fltPlanSpdValue"">' + escapeHtml(wp.type) + '</span><button type=""button"" class=""fltPlanMiniBtn"" data-type-step=""' + escapeHtml(stepKey) + '"" data-type-current=""' + escapeHtml(wp.type) + '"" data-type-delta=""1"">▶</button></div></td>';
          }
          html += '<td>' + escapeHtml(wp.name || '-') + '</td>';
          if (wp.isStart){
            html += '<td class=""fltPlanCellNum"">' + formatAltCellHtml(wp) + '</td>';
          } else {
            html += '<td class=""fltPlanCellNum""><div class=""fltPlanAdjustCell""><button type=""button"" class=""fltPlanMiniBtn"" data-alt-step=""' + escapeHtml(stepKey) + '"" data-alt-dir=""-1"" data-alt-current=""' + escapeHtml(String(wp.altFeet)) + '"" title=""Adjust altitude"">◀</button><span class=""fltPlanSpdValue"">' + formatAltCellHtml(wp) + '</span><button type=""button"" class=""fltPlanMiniBtn"" data-alt-step=""' + escapeHtml(stepKey) + '"" data-alt-dir=""1"" data-alt-current=""' + escapeHtml(String(wp.altFeet)) + '"" title=""Adjust altitude"">▶</button></div></td>';
          }
          if (wp.isStart){
            html += '<td class=""fltPlanCellNum"">' + escapeHtml(wp.hdg || '-') + '</td>';
            html += '<td class=""fltPlanCellNum"">' + escapeHtml(wp.spd || '-') + '</td>';
          } else {
            html += '<td class=""fltPlanCellNum"">' + escapeHtml(wp.hdg || '-') + '</td>';
            html += '<td class=""fltPlanCellNum""><div class=""fltPlanAdjustCell""><button type=""button"" class=""fltPlanMiniBtn"" data-spd-step=""' + escapeHtml(stepKey) + '"" data-spd-dir=""-1"" data-spd-alt=""' + escapeHtml(String(wp.altFeet)) + '"" title=""' + escapeHtml(speedStepTitle) + '"">◀</button><span class=""fltPlanSpdValue' + (speedRec ? ' fltPlanRecSpeed' : '') + '"" data-speed-mode-step=""' + escapeHtml(stepKey) + '"" data-speed-alt=""' + escapeHtml(String(wp.altFeet)) + '""' + (speedRec ? ' data-speed-rec-step=""' + escapeHtml(stepKey) + '""' : '') + ' title=""' + escapeHtml(speedRec ? 'Recommended speed: click to accept' : speedClickTitle) + '"">' + escapeHtml(speedDisplay) + '</span><button type=""button"" class=""fltPlanMiniBtn"" data-spd-step=""' + escapeHtml(stepKey) + '"" data-spd-dir=""1"" data-spd-alt=""' + escapeHtml(String(wp.altFeet)) + '"" title=""' + escapeHtml(speedStepTitle) + '"">▶</button></div></td>';
          }
          html += '<td class=""fltPlanCellNum"">' + escapeHtml(wp.dist || '-') + '</td>';
          if (wp.isStart){
            html += '<td class=""fltPlanCellNum"">' + escapeHtml(wp.etaDisplay || wp.eta) + '</td>';
          } else {
            const ataEntry = (planState && planState.ataByStep && planState.ataByStep[stepKey]) ? planState.ataByStep[stepKey] : null;
            const plannedEtaText = String(wp.etaDisplay || wp.eta || '-');
            const ataShown = ataEntry && isFinite(Number(ataEntry.actualSeconds));
            const etaClass = ataShown ? 'fltPlanAtaValue' : 'fltPlanEtaValue';
            const etaText = ataShown ? formatSecondsToClock(Number(ataEntry.actualSeconds)) : plannedEtaText;
            const etaTitle = ataShown ? 'ATA active - click to return to ETA' : 'ETA - click to mark ATA at current mission time';
            html += '<td class=""fltPlanCellNum""><span class=""fltPlanEtaWrap""><input type=""checkbox"" data-tot-lock-step=""' + escapeHtml(stepKey) + '""' + lockChecked + '><span class=""' + etaClass + '"" data-eta-step=""' + escapeHtml(stepKey) + '"" data-eta-planned=""' + escapeHtml(plannedEtaText) + '"" title=""' + escapeHtml(etaTitle) + '"">' + escapeHtml(etaText) + '</span></span></td>';
          }
          const coordText = getNavlogCoordinateDisplayText(wp, theatre, coordDisplayMode);
          html += '<td class=""fltPlanCellNum fltPlanCoordValue"">' + escapeHtml(coordText) + '</td>';
          html += '</tr>';
        });
      }

      html += '</tbody></table></div></div>';
      html += '<div class=""fltPlanBottomGrid"">';
      html += '<div class=""fltPlanInfoFreqWrap"">' + formatFrequenciesBlockHtml(data) + '</div>';
      if (cmdsBlockHtml){
        html += '<div class=""fltPlanInfoCmdsWrap"">' + cmdsBlockHtml + '</div>';
      }
      html += '<div class=""fltPlanInfoAssetsWrap"">' + formatAssetsBlockHtml(data) + '</div>';
      html += '<div class=""fltPlanInfoWxWrap"">' + formatMetarBlockHtml(data) + '</div>';
      html += '</div>';
      html += '</div>';
      return html;
    }

    function formatRouteToolTableHtml(root, selected, data){
      const presets = (root && typeof root === 'object') ? root : null;
      if (!presets) return '';

      const routeNames = Object.keys(presets);
      if (!routeNames.length) return '';

      routeNames.sort(function(a,b){ return String(a).localeCompare(String(b)); });
      const primaryRouteName = String(routeNames[0] || getDtcDisplayName(selected) || '-');
      const primaryRoute = presets[primaryRouteName] || {};
      const waypoints = getRouteWaypoints(primaryRoute);
      const page = getDtcPageBySelection(selected);

      const routeRows = routeNames.map(function(routeName){
        const routeObj = presets[routeName] || {};
        const count = getRouteWaypoints(routeObj).length;
        const name = String(routeName || '-');
        return '<tr><td style=""width:56px;"">' + (name === primaryRouteName ? '<strong>' + escapeHtml(name) + '</strong>' : escapeHtml(name)) + '</td><td>' + escapeHtml(String(count)) + '</td></tr>';
      });
      const routeSummaryHtml = '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">ROUTES</div><div class=""fltPlanPage2Body""><table class=""fltPlanPage2Table""><thead><tr><th style=""width:56px;"">ROUTE</th><th>WAYPOINTS</th></tr></thead><tbody>' + routeRows.join('') + '</tbody></table></div></div>';
      const pageSwitcherHtml = '<span class=""fltPlanPageSwitcher""><button type=""button"" class=""fltPlanPageBtn' + (page === 1 ? ' active' : '') + '"" data-dtc-page=""1"">NAVLOG</button><button type=""button"" class=""fltPlanPageBtn' + (page === 2 ? ' active' : '') + '"" data-dtc-page=""2"">COM/ROUTE</button><button type=""button"" class=""fltPlanPageBtn' + (page === 3 ? ' active' : '') + '"" data-dtc-page=""3"">STORES</button><button type=""button"" class=""fltPlanPageBtn' + (page === 4 ? ' active' : '') + '"" data-dtc-page=""4"">MAP</button></span>';

      if (page === 3){
        return formatStoresPageHtml(pageSwitcherHtml, data);
      }

      if (page === 4){
        return formatDtcPage3Html(pageSwitcherHtml, waypoints, data, selected, null, root);
      }

      if (page === 2){
        let html = '<div class=""fltPlanBoard"">';
        html += '<div style=""margin:4px 0 6px 0;"">' + pageSwitcherHtml + '</div>';
        html += '<div class=""fltPlanPage2Grid"">';
        html += formatRuntimeCommPanelHtml(data);
        html += '<div class=""fltPlanPage2Stack"">';
        html += routeSummaryHtml;
        html += formatMapMarkersPanelHtml(selected, data);
        html += '</div>';
        html += '</div></div>';
        return html;
      }

      return renderFlightPlanBoardHtml(selected, data, primaryRouteName, 'ROUTE TOOL', getPathFileName(getFltPlnPath(selected)), waypoints, formatRuntimeCmdsInfoBlockHtml(data), pageSwitcherHtml);
    }

    function parseMissionRuntimeWaypointSample(line){
      const text = String(line || '').trim();
      if (!text) return null;

      const parts = text.split('|');
      if (!parts.length) return null;

      const groupPart = String(parts[1] || '');
      const groupMatch = groupPart.match(/^group=(.*)$/i);
      const groupName = groupMatch ? String(groupMatch[1] || '').trim() : '';

      const map = {};
      parts.forEach(function(p){
        const idx = p.indexOf('=');
        if (idx <= 0) return;
        const key = String(p.substring(0, idx)).trim().toLowerCase();
        const value = String(p.substring(idx + 1)).trim();
        if (!key) return;
        map[key] = value;
      });

      const pt = Number(map.pt);
      const x = Number(map.x);
      const y = Number(map.y);
      const altMeters = Number(map.alt);
      const spdMs = Number(map.spd);
      const etaSeconds = Number(map.eta);
      const task = String(map.task || '').trim();

      if (!isFinite(pt) || !isFinite(x) || !isFinite(y)) return null;

      const altFeetRaw = isFinite(altMeters) ? (altMeters * 3.28084) : NaN;
      const altFeet = isFinite(altFeetRaw) ? (Math.round(altFeetRaw / 500) * 500) : NaN;
      const speedKnots = isFinite(spdMs) ? Math.round(spdMs * 1.94384449) : NaN;

      return {
        groupName: groupName,
        step: String(Math.round(pt)),
        type: abbreviateRouteType(task || 'WP'),
        typeRaw: task || 'WP',
        name: '-',
        alt: isFinite(altFeet) ? String(altFeet) : '-',
        altFeet: altFeet,
        altType: '',
        eta: formatEtaSeconds(etaSeconds),
        etaSourceSeconds: etaSeconds,
        spd: isFinite(speedKnots) ? String(speedKnots) : '-',
        x: String(Math.round(x)),
        y: String(Math.round(y)),
        xNum: x,
        yNum: y
      };
    }

    function cloneRuntimeWaypoints(rows){
      const list = Array.isArray(rows) ? rows : [];
      return list.map(function(wp){
        return wp && typeof wp === 'object' ? Object.assign({}, wp) : wp;
      });
    }

    function readMissionRuntimeWaypointsFromDiagnostics(data){
      const server = (data && data.Server) || {};
      const diagnostics = (server && server.Diagnostics) || {};
      const playerRows = Array.isArray(diagnostics.playerGroupWaypoints) ? diagnostics.playerGroupWaypoints : [];
      const rows = playerRows.length
        ? playerRows
        : (Array.isArray(diagnostics.waypointSamples) ? diagnostics.waypointSamples : []);
      const parsed = rows
        .map(parseMissionRuntimeWaypointSample)
        .filter(function(x){ return !!x; });

      if (!parsed.length) return [];

      const playerGroup = String(diagnostics.playerGroup || '').trim();
      const effective = playerGroup
        ? parsed.filter(function(wp){ return String(wp.groupName || '').trim() === playerGroup; })
        : parsed;

      if (!effective.length) return [];

      return effective
        .sort(function(a, b){ return Number(a.step) - Number(b.step); })
        .map(function(wp){
          const clone = Object.assign({}, wp);
          delete clone.groupName;
          return clone;
        });
    }

    function getMissionRuntimeWaypoints(data){
      const model = data || latestData || {};
      const selected = String(model.DtcSelectedFile || '');
      const missionIdentity = getMissionIdentity(model);
      const diagnostics = ((model && model.Server) || {}).Diagnostics || {};
      const liveGroupName = String(diagnostics.playerGroup || '').trim();

      if (selected){
        return readMissionRuntimeWaypointsFromDiagnostics(model);
      }

      const live = readMissionRuntimeWaypointsFromDiagnostics(model);
      const hasSnapshot = !!(runtimeFlightPlanSnapshot && runtimeFlightPlanSnapshot.length);
      const snapshotMatchesMission = runtimeFlightPlanSnapshotMissionIdentity === missionIdentity;
      if (live.length && (!hasSnapshot || !snapshotMatchesMission)){
        runtimeFlightPlanSnapshot = cloneRuntimeWaypoints(live);
        runtimeFlightPlanSnapshotMissionIdentity = missionIdentity;
        runtimeFlightPlanSnapshotGroupName = liveGroupName;
      } else if (liveGroupName && snapshotMatchesMission && !runtimeFlightPlanSnapshotGroupName){
        runtimeFlightPlanSnapshotGroupName = liveGroupName;
      }

      if (runtimeFlightPlanSnapshot
        && runtimeFlightPlanSnapshot.length
        && runtimeFlightPlanSnapshotMissionIdentity === missionIdentity){
        return cloneRuntimeWaypoints(runtimeFlightPlanSnapshot);
      }

      return live;
    }

    function formatMissionRuntimeFlightPlanHtml(data, waypoints){
      const rows = applyTypeOverrides(Array.isArray(waypoints) ? waypoints.slice() : [], '__RUNTIME_PLAYER__');
      const server = (data && data.Server) || {};
      const diagnostics = (server && server.Diagnostics) || {};
      const missionIdentity = getMissionIdentity(data);
      const groupName = (runtimeFlightPlanSnapshotMissionIdentity === missionIdentity && runtimeFlightPlanSnapshotGroupName)
        ? String(runtimeFlightPlanSnapshotGroupName || '').trim()
        : String(diagnostics.playerGroup || '').trim();
      const routeName = groupName ? ('PLAYER ROUTE (' + groupName + ')') : 'PLAYER ROUTE';
      const selected = '__RUNTIME_PLAYER__';
      const page = getDtcPageBySelection(selected);
      const pageSwitcherHtml = '<span class=""fltPlanPageSwitcher""><button type=""button"" class=""fltPlanPageBtn' + (page === 1 ? ' active' : '') + '"" data-dtc-page=""1"">NAVLOG</button><button type=""button"" class=""fltPlanPageBtn' + (page === 2 ? ' active' : '') + '"" data-dtc-page=""2"">COM/ROUTE</button><button type=""button"" class=""fltPlanPageBtn' + (page === 3 ? ' active' : '') + '"" data-dtc-page=""3"">STORES</button><button type=""button"" class=""fltPlanPageBtn' + (page === 4 ? ' active' : '') + '"" data-dtc-page=""4"">MAP</button></span>';
      if (page === 2){
        return formatRuntimePage2Html(data, pageSwitcherHtml, selected);
      }
      if (page === 3){
        return formatStoresPageHtml(pageSwitcherHtml, data);
      }
      if (page === 4){
        return formatDtcPage3Html(pageSwitcherHtml, rows, data, selected);
      }
      return renderFlightPlanBoardHtml(selected, data, routeName, 'MISSION RUNTIME', '-', rows, formatRuntimeCmdsInfoBlockHtml(data), pageSwitcherHtml);
    }

    function formatDtcTableHtml(root, selected, data){
      const allWaypoints = getDtcWaypoints(root);
      const availableRoutes = getDtcAvailableRoutes(root, allWaypoints);
      let routeKey = getDtcRouteBySelection(selected);
      if (availableRoutes.indexOf(routeKey) < 0){
        routeKey = availableRoutes[0] || 'R1';
        setDtcRouteBySelection(selected, routeKey);
      }
      const mapOverlays = getDtcMapOverlays(root, routeKey, data);
      const isF14 = isF14DtcContext(root, data);
      let waypoints = applyTypeOverrides(filterDtcWaypointsByRoute(root, allWaypoints, routeKey), selected);
      if (isF14 && routeKey === 'R1'){
        const runtimeWaypoints = getMissionRuntimeWaypoints(data);
        if (runtimeWaypoints.length){
          waypoints = applyTypeOverrides(runtimeWaypoints.slice(), '__RUNTIME_PLAYER__');
        }
      }
      const cmdsBlockHtml = formatDtcCmdsBlockHtml(root);
      const page = getDtcPageBySelection(selected);
      const routeButtons = availableRoutes.map(function(r){
        return '<button type=""button"" class=""fltPlanPageBtn' + (routeKey === r ? ' active' : '') + '"" data-dtc-route=""' + r + '"">' + r + '</button>';
      }).join('');
      const pageSwitcherHtml = '<span class=""fltPlanPageSwitcher""><button type=""button"" class=""fltPlanPageBtn' + (page === 1 ? ' active' : '') + '"" data-dtc-page=""1"">NAVLOG</button><button type=""button"" class=""fltPlanPageBtn' + (page === 2 ? ' active' : '') + '"" data-dtc-page=""2"">COM/ROUTE</button><button type=""button"" class=""fltPlanPageBtn' + (page === 3 ? ' active' : '') + '"" data-dtc-page=""3"">STORES</button><button type=""button"" class=""fltPlanPageBtn' + (page === 4 ? ' active' : '') + '"" data-dtc-page=""4"">MAP</button></span><span class=""fltPlanPageSwitcher"">' + routeButtons + '</span>';
      if (page === 3){
        return formatStoresPageHtml(pageSwitcherHtml, data);
      }
      if (page === 4){
        return formatDtcPage3Html(pageSwitcherHtml, waypoints, data, selected, mapOverlays, root);
      }
      if (page === 2){
        return formatDtcPage2Html(root, pageSwitcherHtml, allWaypoints, data, selected);
      }
      return renderFlightPlanBoardHtml(selected, data, getDtcDisplayName(selected) + ' ' + routeKey, 'DTC JSON', getPathFileName(selected), waypoints, cmdsBlockHtml, pageSwitcherHtml);
    }

    function formatRouteToolTable(root, selected){
      const presets = (root && typeof root === 'object') ? root : null;
      if (!presets) return '';

      const routeNames = Object.keys(presets);
      if (!routeNames.length) return '';

      const lines = [];
      lines.push('ROUTE NAME : ' + getDtcDisplayName(selected));
      lines.push('ROUTE FILE : ' + getPathFileName(getFltPlnPath(selected)));
      lines.push('PATH       : ' + getFltPlnPath(selected));
      lines.push('');

      routeNames.sort(function(a,b){ return String(a).localeCompare(String(b)); });
      routeNames.forEach(function(routeName){
        const route = presets[routeName] || {};
        const wpKeys = Object.keys(route)
          .filter(function(k){ return /^\d+$/.test(String(k)); })
          .sort(function(a,b){ return parseInt(a,10) - parseInt(b,10); });

        lines.push('ROUTE: ' + routeName);
        lines.push('WP  TYPE           ALT(ft)   ETA       X             Y');
        lines.push('--- -------------- -------- -------- ------------ ------------');

        if (!wpKeys.length){
          lines.push('No waypoints found.');
          lines.push('');
          return;
        }

        wpKeys.forEach(function(wk){
          const wp = route[wk] || {};
          const wpNum = String(wk).padStart(2, '0');
          const type = String(wp.type || wp.action || 'WP').substring(0, 14).padEnd(14, ' ');
          const alt = isFinite(Number(wp.alt)) ? String(Math.round(Number(wp.alt))).padStart(8, ' ') : '       -';
          const eta = formatEtaSeconds(wp.ETA).padEnd(8, ' ');
          const x = isFinite(Number(wp.x)) ? String(Math.round(Number(wp.x))).padStart(12, ' ') : '           -';
          const y = isFinite(Number(wp.y)) ? String(Math.round(Number(wp.y))).padStart(12, ' ') : '           -';
          lines.push(wpNum + '  ' + type + ' ' + alt + ' ' + eta + ' ' + x + ' ' + y);
        });

        lines.push('');
      });

      lines.push('Note: X/Y are mission map coordinates. LAT/LON conversion needs map-projection/origin data from DCS runtime.');
      return lines.join('\n');
    }

    function formatDtcTabContentHtml(data){
      try{
        const files = Array.isArray(data.DtcFiles) ? data.DtcFiles : [];
        const selected = String(data.DtcSelectedFile || '');
        const jsonText = String(data.DtcJson || '').trim();
        const sourceType = String(data.DtcSourceType || '').toUpperCase();
        const runtimeWaypoints = getMissionRuntimeWaypoints(data);

        if ((!files.length || !selected) && runtimeWaypoints.length){
          return formatMissionRuntimeFlightPlanHtml(data, runtimeWaypoints);
        }

        if (!files.length){
          return '<div class=""fltPlanMessage"">No valid FLT PLN files found in Saved Games/DCS*/DTC (DTC) or Saved Games/DCS*/Config/RouteToolPresets/&lt;ActiveTerrain&gt;.lua (RouteTool).\n\nExport DTC or save RouteTool presets, then click Refresh FLT PLN.</div>';
        }

        if (!selected){
          return '<div class=""fltPlanMessage"">Select a FLT PLN file from the file list to load data.</div>';
        }

        if (!jsonText){
          return '<div class=""fltPlanMessage"">Selected FLT PLN file is valid but has no loadable content.</div>';
        }

        let parsed;
        try{
          parsed = JSON.parse(jsonText);
        }catch(_){
          return '<div class=""fltPlanMessage"">Failed to parse selected FLT PLN payload.</div>';
        }

        const root = (parsed && parsed.data && typeof parsed.data === 'object') ? parsed.data : parsed;
        if (sourceType === 'RTE') {
          const rteHtml = formatRouteToolTableHtml(root, selected, data);
          if (rteHtml) return rteHtml;
        }
        if (sourceType === 'DTC') {
          const dtcHtml = formatDtcTableHtml(root, selected, data);
          if (dtcHtml) return dtcHtml;
        }

        return '<pre class=""fltPlanPlain"">' + escapeHtml(formatDtcTabContent(data)) + '</pre>';
      }catch(ex){
        const msg = (ex && ex.message) ? String(ex.message) : 'Unknown FLT PLN render error';
        return '<div class=""fltPlanMessage"">FLT PLN render error: ' + escapeHtml(msg) + '</div>';
      }
    }

    function formatDtcTabContent(data){
      const files = Array.isArray(data.DtcFiles) ? data.DtcFiles : [];
      const selected = String(data.DtcSelectedFile || '');
      const jsonText = String(data.DtcJson || '').trim();
      const sourceType = String(data.DtcSourceType || '').toUpperCase();

      if (!files.length){
        return 'No valid FLT PLN files found in Saved Games/DCS*/DTC (DTC) or Saved Games/DCS*/Config/RouteToolPresets/<ActiveTerrain>.lua (RouteTool).\n\nExport DTC or save RouteTool presets, then click Refresh FLT PLN.';
      }

      if (!selected){
        return 'Select a FLT PLN file from the file list to load data.';
      }

      if (!jsonText){
        return 'Selected FLT PLN file is valid but has no loadable content.';
      }

      let parsed;
      try{
        parsed = JSON.parse(jsonText);
      }catch(_){
        return 'Failed to parse selected FLT PLN payload.';
      }

      const root = (parsed && parsed.data && typeof parsed.data === 'object') ? parsed.data : parsed;
      if (sourceType === 'RTE') {
        const rteText = formatRouteToolTable(root, selected);
        if (rteText) return rteText;
      }

      if (sourceType === 'DTC') {
        return formatDtcFocusedTable(root, selected);
      }

      const rows = [];
      appendDtcRows('', root, rows, 0);

      if (!rows.length){
        return 'Selected FLT PLN file contains no tabular fields.';
      }

      const maxKeyWidth = rows.reduce(function(acc, r){ return Math.max(acc, String(r.key || '').length); }, 8);
      const keyWidth = clamp(maxKeyWidth + 1, 16, 56);
      const lines = [];
      const sourceLabel = sourceType === 'RTE' ? 'ROUTE FILE' : 'DTC FILE';
      lines.push(sourceLabel + ' : ' + getDtcDisplayName(selected));
      lines.push('PATH     : ' + selected);
      lines.push('');
      lines.push('FIELD'.padEnd(keyWidth) + 'VALUE');
      lines.push('-'.repeat(keyWidth) + '-----');
      rows.forEach(function(r){
        const key = String(r.key || '-');
        const value = String(r.value || '-').replace(/\s+/g, ' ').trim();
        lines.push(key.padEnd(keyWidth) + value);
      });

      if (rows.length >= 220){
        lines.push('');
        lines.push('... output truncated ...');
      }

      return lines.join('\n');
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

      const groups = getKeywordGroupsForTab(data, tab, rows);
      if (!groups.length) return escapeHtml(text);

      const blocks = groups.map(function(group){
        const items = Array.isArray(group.items) ? group.items : [];
        const splitIndex = Math.ceil(items.length / 2);
        const leftRows = items.slice(0, splitIndex);
        const rightRows = items.slice(splitIndex);
        const left = leftRows.map(escapeHtml).join('<br>');
        const right = rightRows.map(escapeHtml).join('<br>');

        return '<div class=""kwGroup""><div class=""kwGroupTitle"">'
          + escapeHtml(group.title)
          + '</div><div class=""kwCols""><div class=""kwCol"">'
          + left
          + '</div><div class=""kwCol"">'
          + right
          + '</div></div></div>';
      });

      return '<div class=""keywordsGroups"">' + blocks.join('') + '</div>';
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

      if (tab === 'DTC'){
        return formatDtcTabContent(data);
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
      updateFakeMissionControlsUi();
      const displayData = getDisplayData(data);
      maybeResetFlightPlanStateForMission(data);
      const server = displayData && displayData.Server ? displayData.Server : {};
      const renderTheater = String(server.Theater || '').trim();
      if (renderTheater){
        lastKnownTheater = renderTheater;
      }
      const haveMission = !!(server.Aircraft || server.MissionTitle || server.Theater);
      const debugMode = !!server.DebugMode;
      const defaultStatusText = haveMission ? 'Live session detected.' : 'Waiting for mission data...';
      let statusText = defaultStatusText;
      let statusLevel = '';

      if (displayData && displayData.Status && displayData.Status.Text) {
        const updated = displayData.Status.UpdatedUtc ? Date.parse(displayData.Status.UpdatedUtc) : NaN;
        const fresh = isFinite(updated) ? ((Date.now() - updated) < 10000) : true;
        if (fresh) {
          statusText = String(displayData.Status.Text);
          statusLevel = String(displayData.Status.Level || '').toLowerCase();
        }
      }

      if (fakeMissionEnabled){
        statusText = 'SIM TEST MODE ACTIVE';
        statusLevel = 'warning';
      }

      setStatus(statusText, statusLevel);

      const dlinkBox = document.getElementById('dlinkOn');
      if (dlinkBox){
        dlinkBox.disabled = false;
        dlinkBox.checked = !!dlinkOnEnabled;
      }

      document.getElementById('session').textContent = [
        'Active Category : ' + safe(normalizeActiveCategory(displayData.ActiveCategory, displayData)),
        'Updated (UTC)   : ' + formatUtcToSeconds(displayData.UpdatedUtc),
        '',
        'Theater         : ' + safe(server.Theater),
        'DCS Location    : ' + safe(server.DcsLocation || server.DcsVersion),
        'Aircraft        : ' + safe(server.Aircraft),
        'Player Name     : ' + safe(server.PlayerUsername),
        'Callsign        : ' + safe(server.PlayerCallsign),
        'Mission         : ' + safe(server.MissionTitle),
        'Multiplayer     : ' + (server.Multiplayer ? 'Yes' : 'No')
      ].join('\n');

      renderTabs(displayData);
      updateDtcControls(displayData);
      applyCurrentTabKeywordsSplit();
      document.body.classList.toggle('notes-tab', selectedTab === 'NOTES');
      document.body.classList.toggle('flt-plan-tab', selectedTab === 'DTC');
      if (selectedTab !== 'NOTES' || !drawModeEnabled){
        setDrawInteractionInNotes(false);
        clearDrawModeDisableTimer();
      }
      updateDrawModeToggleUi();
      document.getElementById('tabTitle').textContent = 'Tab: ' + tabLabel(selectedTab);
      const tabBody = document.getElementById('tabBody');
      let activeDtcSelection = '';
      let preserveOpenFreeMapView = false;
      if (selectedTab === 'DTC'){
        activeDtcSelection = getActiveFlightPlanSelection(displayData);
        const page = activeDtcSelection ? getDtcPageBySelection(activeDtcSelection) : 1;
        const openMapHost = (tabBody && tabBody.querySelector) ? tabBody.querySelector('[data-openfreemap-map-id]') : null;
        const hasOpenMapHost = !!openMapHost;
        const hostSelection = openMapHost ? String(openMapHost.getAttribute('data-openfreemap-selection') || '') : '';
        preserveOpenFreeMapView = !!(activeDtcSelection
          && page === 4
          && hasOpenMapHost
          && hostSelection === getFlightPlanEtaStartKey(activeDtcSelection)
          && isMapBackgroundEnabledBySelection(activeDtcSelection));
      }

      if (!preserveOpenFreeMapView && typeof disposeOpenFreeMapInstances === 'function'){
        disposeOpenFreeMapInstances();
      }

      if (selectedTab === 'DTC'){
        tabBody.className = 'content mainContent fltPlanContent';
        if (!preserveOpenFreeMapView){
          tabBody.innerHTML = formatDtcTabContentHtml(displayData);
          if (typeof initializeOpenFreeMapInstances === 'function'){
            initializeOpenFreeMapInstances();
          }
        } else if (activeDtcSelection){
          formatDtcTabContentHtml(displayData);
          const payload = getLatestOpenFreeMapPayloadBySelection(activeDtcSelection);
          if (payload){
            syncOpenFreeMapForSelection(activeDtcSelection, payload);
          }
          const bgBtn = document.getElementById('mapBgToggleBtn');
          if (bgBtn){
            bgBtn.textContent = 'BG ' + (isMapBackgroundEnabledBySelection(activeDtcSelection) ? 'ON' : 'OFF');
          }
          const bgStatusEl = document.getElementById('mapBgStatus');
          if (bgStatusEl){
            const mapBackgroundEnabled = isMapBackgroundEnabledBySelection(activeDtcSelection);
            const hasPayload = !!payload;
            const bgStatus = !mapBackgroundEnabled
              ? 'BG disabled'
              : (openFreeMapRuntimeFailed
                  ? ('BG runtime unavailable' + (openFreeMapRuntimeErrorText ? (': ' + openFreeMapRuntimeErrorText) : ''))
                  : (hasPayload
                      ? ('BG ready [' + openFreeMapLastPayloadStatus + ']')
                      : ('BG unavailable for current theatre/data (' + String((((displayData && displayData.Server) || {}).Theater) || lastKnownTheater || '?') + ')' + (openFreeMapLastPayloadStatus ? (' [' + openFreeMapLastPayloadStatus + ']') : ''))));
            bgStatusEl.textContent = bgStatus;
          }

          const readoutEl = document.getElementById('mapBraReadout');
          if (readoutEl){
            const selectedAssetKey = getMapSelectedAssetKeyBySelection(activeDtcSelection);
            const rawAssets = getMudMapAssets(displayData, dlinkOnEnabled);
            const overlays = getDtcMapOverlays(null, getDtcRouteBySelection(activeDtcSelection), displayData);
            const overlayAirfields = Array.isArray(overlays && overlays.airfields) ? overlays.airfields : [];
            const airfieldAssets = overlayAirfields
              .map(function(a){
                return {
                  callsign: String((a && a.callsign) || '').trim(),
                  name: String((a && a.name) || (a && a.label) || '').trim(),
                  category: 'ATC',
                  typeName: String((a && a.typeName) || '').trim(),
                  frequency: String((a && a.frequency) || '').trim(),
                  altFrequencies: Array.isArray(a && a.altFrequencies) ? a.altFrequencies.slice(0) : [],
                  tacan: String((a && a.tacan) || '').trim(),
                  mpClientCallsign: String((a && a.mpClientCallsign) || '').trim(),
                  xNum: Number(a && a.xNum),
                  yNum: Number(a && a.yNum),
                  altFeet: Number(a && a.altFeet),
                };
              })
              .filter(function(a){ return isFinite(Number(a.xNum)) && isFinite(Number(a.yNum)); });
            const selectionPool = rawAssets.concat(airfieldAssets);
            const selectedAsset = selectedAssetKey
              ? (selectionPool.find(function(a){ return makeMapAssetSelectionKey(a) === selectedAssetKey; }) || null)
              : null;
            const bullseyePoint = getMapBullseyePoint(null, [], null, displayData);
            let readout = formatMapBraReadout(displayData, selectedAsset, bullseyePoint);
            const markerCount = rawAssets.filter(function(a){ return String((a && a.category) || '').toUpperCase() === 'MAP_MARKER'; }).length;
            if (markerCount > 0){
              readout += '   MKR ' + String(markerCount);
            }
            readoutEl.textContent = readout;
          }
        }
        if (activeDtcSelection){
          updateDtcRouteButtonUi(activeDtcSelection);
        }
      } else {
        tabBody.className = 'content mainContent';
        tabBody.textContent = formatTabContent(displayData, selectedTab);
      }
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
        ? formatAiCrewPhaseLabel(displayData.AiCrewPhase)
        : '';
      const keywordPanelEl = document.getElementById('keywordPanel');
      if (selectedTab === 'DTC'){
        if (keywordPanelEl) keywordPanelEl.style.display = 'none';
      } else {
        if (keywordPanelEl) keywordPanelEl.style.display = 'flex';
        document.getElementById('keywordTitle').textContent = 'Keywords: ' + tabLabel(selectedTab) + aiCrewPhaseSuffix;
        updateKeywordBodyHtml(formatKeywordReferenceHtml(displayData, selectedTab));
      }

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

      document.getElementById('json').textContent = JSON.stringify(displayData, null, 2);

      const serverMessagesEl = document.getElementById('serverMessages');
      if (serverMessagesEl){
        const rows = Array.isArray(displayData.RawServerMessages) ? displayData.RawServerMessages : [];
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

    function updateDtcControls(data){
      const wrap = document.getElementById('dtcControls');
      const selector = document.getElementById('dtcSelector');
      const listEl = document.getElementById('dtcFileList');
      const selectedLabel = document.getElementById('dtcSelectedFileLabel');
      const missionClockLabel = document.getElementById('missionClockLabel');
      if (!wrap || !selector || !listEl || !selectedLabel) return;

      const visible = selectedTab === 'DTC';
      wrap.className = visible ? 'controls fltPlanControls' : 'controls fltPlanControls hidden';
      selector.className = visible ? 'panel dtcSelector' : 'panel dtcSelector hidden';
      if (missionClockLabel) missionClockLabel.style.display = visible ? 'inline-block' : 'none';
      if (!visible) return;

      updateMissionClockLabel(data);

      const files = Array.isArray(data.DtcFiles) ? data.DtcFiles : [];
      const selected = String(data.DtcSelectedFile || '');
      selectedLabel.textContent = selected ? ('Selected: ' + getDtcDisplayName(selected)) : 'No FLT PLN selected';

      if (!files.length){
        listEl.innerHTML = 'No FLT PLN files found.';
        applyDtcListCollapsedState(dtcListCollapsed);
        return;
      }

      const rows = [];
      files.forEach(function(filePath){
        const fp = String(filePath || '');
        const active = selected && fp === selected;
        const type = (fp.indexOf('RTE::') === 0 || /\.(rte|lua)$/i.test(fp)) ? 'RTE' : 'DTC';
        rows.push('<button type=""button"" class=""dtcFileItem' + (active ? ' active' : '') + '"" data-file=""' + encodeURIComponent(fp) + '""><span>' + escapeHtml(getDtcDisplayName(fp)) + '</span><span class=""dtcFileMeta"">' + type + '</span></button>');
      });
      listEl.innerHTML = rows.join('');
      applyDtcListCollapsedState(dtcListCollapsed);
    }

    async function setServerMessageCapture(enabled){
      try{
        await fetch('dev/servermessages?enabled=' + (enabled ? '1' : '0'), { method: 'POST', cache: 'no-store' });
      }catch(e){
      }
    }

    async function refreshDtcFiles(){
      invalidateRuntimeFlightPlanSnapshot();
      resetRuntimeFlightPlanState();
      try{
        await fetch('/okb/dtc/select?file=', { method: 'POST', cache: 'no-store' });
      }catch(_){
      }
      try{
        await fetch('/okb/dtc/list', { method: 'POST', cache: 'no-store' });
      }catch(_){
      }
      await tick();
    }

    async function selectDtcFile(filePath){
      try{
        await fetch('/okb/dtc/select?file=' + encodeURIComponent(filePath || ''), { method: 'POST', cache: 'no-store' });
      }catch(_){
      }
      await tick();
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
        try{
          render(j);
        }catch(e){
          const msg = (e && e.message) ? String(e.message) : 'Unknown render error';
          setStatus('Render error: ' + msg, 'warning');
        }
      }catch(e){
        setStatus('Waiting for VAICOM connection...', '');
      }
    }

    async function clockTick(){
      if (fakeMissionEnabled) return;
      try{
        const requestStartedMs = Date.now();
        const r = await fetch('clock', { cache: 'no-store' });
        const j = await r.json();
        const missionClock = Number(j && j.MissionTimeSeconds);
        const identity = String((j && j.MissionIdentity) || '');
        if (!isFinite(missionClock) || missionClock < 0) return;

        if (missionClockAnchorIdentity && identity && missionClockAnchorIdentity !== identity){
          resetMissionClockAnchor();
        }

        missionClockAnchorIdentity = identity || missionClockAnchorIdentity;

        const hasAnchor = isFinite(Number(missionClockAnchorSeconds)) && missionClockAnchorSystemMs > 0;
        if (!hasAnchor){
          // Lock to first reliable mission-time sample, then run locally from system elapsed time.
          setMissionClockAnchor(missionClock, requestStartedMs);
        }
      }catch(_){
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

    document.getElementById('nightMode').addEventListener('change', function(ev){
      nightModeEnabled = !!ev.target.checked;
      persistNightModePreference();
      applyNightModeUi();
    });

    document.getElementById('fontSizeSlider').addEventListener('input', function(ev){
      contentFontSizePx = clamp(parseInt(ev.target.value, 10) || 24, 18, 34);
      applyContentFontSizeUi();
      persistContentFontSizePreference();
    });

    document.getElementById('dlinkOn').addEventListener('change', function(ev){
      dlinkOnEnabled = !!ev.target.checked;
      persistDlinkOnPreference();
      if (latestData && selectedTab === 'DTC') render(latestData);
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
      if (selectedTab === 'DTC' && latestData){
        let node = ev.target;
        while (node && node !== this){
          if (node.getAttribute && node.getAttribute('data-spd-step')){
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-spd-step') || '');
            const direction = Number(node.getAttribute('data-spd-dir') || 0);
            const altFeet = Number(node.getAttribute('data-spd-alt') || NaN);
            if (selected && step && isFinite(direction) && direction !== 0){
              changeWaypointSpeedAdjustmentByDirection(selected, step, direction, altFeet);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-speed-mode-step')){
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-speed-mode-step') || '');
            const altFeet = Number(node.getAttribute('data-speed-alt') || NaN);
            const recStep = String(node.getAttribute('data-speed-rec-step') || '');
            if (selected && recStep){
              if (acceptSpeedRecommendation(selected, recStep)){
                render(latestData);
              }
              return;
            }
            if (selected && step){
              toggleWaypointSpeedDisplayMode(selected, step, altFeet);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-alt-step')){
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-alt-step') || '');
            const direction = Number(node.getAttribute('data-alt-dir') || 0);
            const currentAlt = Number(node.getAttribute('data-alt-current') || NaN);
            if (selected && step && isFinite(direction) && direction !== 0){
              changeWaypointAltitudeByDirection(selected, step, direction, currentAlt);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-type-step')){
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-type-step') || '');
            const delta = Number(node.getAttribute('data-type-delta') || 0);
            const currentType = String(node.getAttribute('data-type-current') || '');
            if (selected && step && isFinite(delta) && delta !== 0){
              changeWaypointType(selected, step, delta, currentType);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-row-action')){
            const selected = getActiveFlightPlanSelection(latestData);
            const action = String(node.getAttribute('data-row-action') || '').toLowerCase();
            const step = String(node.getAttribute('data-row-step') || '');
            if (selected && step){
              clearNavlogRowReveal(selected);
              if (action === 'dir'){
                setNavlogRowAction(selected, step, 'dir');
                setNavlogDirectToSource(selected, step);
                render(latestData);
              } else if (action === 'del'){
                setNavlogRowAction(selected, step, 'del');
                render(latestData);
              }
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-navlog-row')){
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-navlog-step') || '');
            if (selected && step){
              const state = getFlightPlanPlanState(selected);
              const rowActionStep = stepToKey(state.rowActionStep);
              const rowActionMode = String(state.rowActionMode || '').toLowerCase();
              if (rowActionMode === 'dir' && rowActionStep && rowActionStep === step){
                clearNavlogRowAction(selected);
                render(latestData);
                return;
              }
              if (rowActionMode === 'dir' && rowActionStep && rowActionStep !== step){
                if (setNavlogDirectToTarget(selected, step)){
                  clearNavlogRowAction(selected);
                  clearNavlogRowReveal(selected);
                  render(latestData);
                }
                return;
              }
              if (rowActionMode === 'del' && rowActionStep && rowActionStep === step){
                deleteNavlogStep(selected, step);
                clearNavlogRowAction(selected);
                clearNavlogRowReveal(selected);
                render(latestData);
                return;
              }
              if (rowActionMode === 'del' && rowActionStep && rowActionStep !== step){
                deleteNavlogStep(selected, step);
                clearNavlogRowAction(selected);
                clearNavlogRowReveal(selected);
                render(latestData);
                return;
              }
              clearNavlogRowAction(selected);
              clearNavlogRowReveal(selected);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tot-adjust')){
            const selected = getActiveFlightPlanSelection(latestData);
            const seconds = Number(node.getAttribute('data-tot-adjust') || 0);
            if (selected && isFinite(seconds) && seconds !== 0){
              setTotByMinutesDelta(selected, seconds / 60.0);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tot-adjust-sec')){
            const selected = getActiveFlightPlanSelection(latestData);
            const seconds = Number(node.getAttribute('data-tot-adjust-sec') || 0);
            if (selected && isFinite(seconds) && seconds !== 0){
              setTotBySecondsDelta(selected, seconds);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tko-adjust')){
            const selected = getActiveFlightPlanSelection(latestData);
            const seconds = Number(node.getAttribute('data-tko-adjust') || 0);
            if (selected && isFinite(seconds) && seconds !== 0){
              setTakeoffByMinutesDelta(selected, seconds / 60.0);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tko-adjust-sec')){
            const selected = getActiveFlightPlanSelection(latestData);
            const seconds = Number(node.getAttribute('data-tko-adjust-sec') || 0);
            if (selected && isFinite(seconds) && seconds !== 0){
              setTakeoffBySecondsDelta(selected, seconds);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-dtc-page')){
            const selected = getActiveFlightPlanSelection(latestData);
            const page = Number(node.getAttribute('data-dtc-page') || 1);
            if (selected){
              setDtcPageBySelection(selected, page);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-postflight-toggle')){
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected){
              togglePostFlightOpen(selected);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-eta-step')){
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-eta-step') || '');
            const planned = String(node.getAttribute('data-eta-planned') || '');
            if (selected && step){
              toggleWaypointAta(selected, step, planned);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-dtc-route')){
            const selected = getActiveFlightPlanSelection(latestData);
            const route = String(node.getAttribute('data-dtc-route') || 'R1');
            if (selected){
              setDtcRouteBySelection(selected, route);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-map-zoom')){
            const selected = getActiveFlightPlanSelection(latestData);
            const action = String(node.getAttribute('data-map-zoom') || '').toLowerCase();
            if (selected){
              const handledByWebMap = (typeof handleOpenFreeMapZoomAction === 'function')
                ? handleOpenFreeMapZoomAction(selected, action)
                : false;
              if (!handledByWebMap){
                if (action === 'in') zoomMapViewBySelection(selected, 1.2);
                else if (action === 'out') zoomMapViewBySelection(selected, 1 / 1.2);
                else if (action === 'reset') resetMapViewBySelection(selected);
                render(latestData);
              }
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-map-bg-toggle')){
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected){
              toggleMapBackgroundEnabledBySelection(selected);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-map-bg-toggle')){
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected){
              toggleMapBackgroundEnabledBySelection(selected);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-map-asset-key')){
            const selected = getActiveFlightPlanSelection(latestData);
            if (!selected) return;
            const category = String(node.getAttribute('data-map-asset-category') || '').toUpperCase();
            if (category === 'PLAYER') return;
            const encodedKey = String(node.getAttribute('data-map-asset-key') || '');
            let assetKey = encodedKey;
            try{
              assetKey = decodeURIComponent(encodedKey);
            }catch(_){
              assetKey = encodedKey;
            }
            const current = getMapSelectedAssetKeyBySelection(selected);
            setMapSelectedAssetKeyBySelection(selected, current === assetKey ? '' : assetKey);
            render(latestData);
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tot-lock-step')){
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-tot-lock-step') || '');
            const checked = !!node.checked;
            if (selected && step){
              let etaSeconds = NaN;
              try{
                const row = node.closest ? node.closest('tr') : null;
                const etaCell = row ? row.cells[7] : null;
                etaSeconds = parseEtaToSeconds((etaCell && etaCell.textContent) ? etaCell.textContent : '');
              }catch(_){ }
              setLockedTotStep(selected, step, checked, etaSeconds);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tko-anchor')){
            const selected = getActiveFlightPlanSelection(latestData);
            const anchor = String(node.getAttribute('data-tko-anchor') || '');
            if (selected && anchor){
              setTakeoffTimeByAnchorFromNow(selected, anchor);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-eta-header')){
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected){
              setEtaStartNowForSelection(selected);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-navlog-coord-cycle')){
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected){
              cycleNavlogCoordDisplayMode(selected);
              render(latestData);
            }
            return;
          }
          node = node.parentNode;
        }
      }

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

    document.getElementById('tabBody').addEventListener('pointerdown', function(ev){
      if (!latestData || selectedTab !== 'DTC') return;
      const selected = getActiveFlightPlanSelection(latestData);
      if (!selected) return;

      const page = getDtcPageBySelection(selected);
      if (page === 1){
        const row = ev.target && ev.target.closest ? ev.target.closest('tr[data-navlog-row=""1""]') : null;
        if (row){
          navlogRowDrag = {
            pointerId: ev.pointerId,
            selected: selected,
            step: String(row.getAttribute('data-navlog-step') || ''),
            startX: ev.clientX,
            startY: ev.clientY,
            moved: false,
          };
          return;
        }
      }

      if (page !== 4) return;

      let assetNode = ev.target;
      while (assetNode && assetNode !== this){
        if (assetNode.getAttribute && assetNode.getAttribute('data-map-asset-key')){
          const category = String(assetNode.getAttribute('data-map-asset-category') || '').toUpperCase();
          if (category !== 'PLAYER'){
            const encodedKey = String(assetNode.getAttribute('data-map-asset-key') || '');
            let assetKey = encodedKey;
            try{
              assetKey = decodeURIComponent(encodedKey);
            }catch(_){
              assetKey = encodedKey;
            }
            const current = getMapSelectedAssetKeyBySelection(selected);
            setMapSelectedAssetKeyBySelection(selected, current === assetKey ? '' : assetKey);
            render(latestData);
            ev.preventDefault();
            return;
          }
          break;
        }
        assetNode = assetNode.parentNode;
      }

      let node = ev.target;
      let onMap = false;
      while (node && node !== this){
        if (node.getAttribute && node.getAttribute('data-map-canvas')){ onMap = true; break; }
        node = node.parentNode;
      }
      if (!onMap) return;

      const state = getMapViewBySelection(selected);
      mapPanDrag = {
        pointerId: ev.pointerId,
        selected: selected,
        startX: ev.clientX,
        startY: ev.clientY,
        basePanX: Number(state.panX) || 0,
        basePanY: Number(state.panY) || 0,
      };
      try{ if (ev.target && ev.target.setPointerCapture) ev.target.setPointerCapture(ev.pointerId); }catch(_){ }
      ev.preventDefault();
    });

    document.getElementById('tabBody').addEventListener('pointermove', function(ev){
      if (navlogRowDrag && navlogRowDrag.pointerId === ev.pointerId && latestData){
        const dx = ev.clientX - navlogRowDrag.startX;
        const dy = ev.clientY - navlogRowDrag.startY;
        if (Math.abs(dx) >= 28 && Math.abs(dx) > Math.abs(dy)){
          const step = stepToKey(navlogRowDrag.step);
          if (step){
            const mode = dx > 0 ? 'dir' : 'del';
            clearNavlogRowAction(navlogRowDrag.selected);
            setNavlogRowReveal(navlogRowDrag.selected, step, mode);
            render(latestData);
          }
          navlogRowDrag = null;
          ev.preventDefault();
          return;
        }
      }
      if (!mapPanDrag || mapPanDrag.pointerId !== ev.pointerId || !latestData) return;
      const state = getMapViewBySelection(mapPanDrag.selected);
      state.panX = mapPanDrag.basePanX + (ev.clientX - mapPanDrag.startX);
      state.panY = mapPanDrag.basePanY + (ev.clientY - mapPanDrag.startY);
      render(latestData);
      ev.preventDefault();
    });

    function stopMapPanDrag(ev){
      if (!mapPanDrag) return;
      if (ev && mapPanDrag.pointerId !== ev.pointerId) return;
      mapPanDrag = null;
    }

    function stopNavlogRowDrag(ev){
      if (!navlogRowDrag) return;
      if (ev && navlogRowDrag.pointerId !== ev.pointerId) return;
      navlogRowDrag = null;
    }

    document.getElementById('tabBody').addEventListener('pointerup', function(ev){
      stopNavlogRowDrag(ev);
      stopMapPanDrag(ev);
    });
    document.getElementById('tabBody').addEventListener('pointercancel', function(ev){
      stopNavlogRowDrag(ev);
      stopMapPanDrag(ev);
    });
    document.getElementById('tabBody').addEventListener('pointerleave', function(ev){
      stopNavlogRowDrag(ev);
      stopMapPanDrag(ev);
    });

    document.getElementById('showServer').addEventListener('change', function(ev){
      showServerMessages = ev.target.checked;
      document.getElementById('serverMessages').className = showServerMessages ? '' : 'hidden';
      setServerMessageCapture(showServerMessages);
    });

    document.getElementById('dtcRefresh').addEventListener('click', function(){
      refreshDtcFiles();
    });

    document.getElementById('dtcFileList').addEventListener('click', function(ev){
      let node = ev.target;
      while (node && node !== this && !node.getAttribute('data-file')){
        node = node.parentNode;
      }
      if (!node || node === this) return;
      const encoded = String(node.getAttribute('data-file') || '');
      const file = decodeURIComponent(encoded);
      selectDtcFile(file);
    });

    document.getElementById('dtcSelectorHeader').addEventListener('click', function(){
      applyDtcListCollapsedState(!dtcListCollapsed);
      persistDtcListCollapsedState();
    });

    document.getElementById('liveExportMeta').addEventListener('click', function(){
      applyLiveRefreshState(!liveRefreshEnabled);
    });

    (function(){
      const logo = document.querySelector('.logo');
      if (!logo) return;
      logo.title = 'Hidden test mode: Shift+Click or Triple-Click';

      let logoClickCount = 0;
      let logoClickResetTimer = null;

      function toggleFakeMissionMode(){
        if (fakeMissionEnabled){
          stopFakeMissionMode();
        } else {
          startFakeMissionMode();
        }
      }

      logo.addEventListener('click', function(ev){
        if (ev && ev.shiftKey){
          toggleFakeMissionMode();
          return;
        }

        logoClickCount++;
        if (logoClickResetTimer){
          clearTimeout(logoClickResetTimer);
          logoClickResetTimer = null;
        }

        if (logoClickCount >= 3){
          logoClickCount = 0;
          toggleFakeMissionMode();
          return;
        }

        logoClickResetTimer = setTimeout(function(){
          logoClickCount = 0;
          logoClickResetTimer = null;
        }, 1200);
      });
    })();

    document.getElementById('fakeMissionSlower').addEventListener('click', function(){
      adjustFakeMissionSpeed(1 / 1.4);
    });

    document.getElementById('fakeMissionFaster').addEventListener('click', function(){
      adjustFakeMissionSpeed(1.4);
    });

    document.getElementById('fakeMissionStop').addEventListener('click', function(){
      stopFakeMissionMode();
    });

    document.getElementById('sessionHeader').addEventListener('click', function(){
      applySessionCollapsedState(!sessionCollapsed);
      persistSessionCollapsedState();
    });

    applyDtcListCollapsedState(readInitialDtcListCollapsed());
    applySessionCollapsedState(readInitialSessionCollapsed());
    nightModeEnabled = readNightModePreference();
    applyNightModeUi();
    dlinkOnEnabled = readDlinkOnPreference();
    applyDlinkOnUi();
    contentFontSizePx = readContentFontSizePreference();
    applyContentFontSizeUi();
    drawModeEnabled = readDrawModePreference();
    updateDrawModeToggleUi();
    liveRefreshEnabled = readLiveRefreshPreference();
    updateLiveRefreshUi();
    if (liveRefreshEnabled){
      applyLiveRefreshState(true);
    }
    tabKeywordsSplitByTab = readTabKeywordsSplitRatioByTab();
    initTabKeywordsDivider();
    applyCurrentTabKeywordsSplit();
    registerCustomActionHandlers();
    window.addEventListener('resize', function(){
      applyCurrentTabKeywordsSplit();
    });
    configureOpenKneeboard();
    updateFakeMissionControlsUi();
    tick();
    clockTick();
    clockTickTimer = setInterval(clockTick, 250);
    setInterval(tick, 1000);
  </script>
</body>
</html>";

                private static HttpListener listener;
                private static Thread listenerThread;
                private static bool isRunning;
                private static long lastClientRequestUtcTicks;
                private static readonly Guid SavedGamesFolderId = new Guid("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4");

                [DllImport("shell32.dll")]
                private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr ppszPath);

                private const int DefaultOpenKneeboardOutPort = 7779;
                private const string OpenKneeboardPluginsRegistryKey = @"SOFTWARE\Fred Emmott\OpenKneeboard\Plugins\v1";
                private const string OpenKneeboardPluginId = "VAICOM-Community";
                private const string OpenKneeboardPluginTabId = OpenKneeboardPluginId + ";okb-out";
                private const string OpenKneeboardKeywordsPluginId = "github.com/Penecruz/VAICOM-Community/keywords";
                private const string OpenKneeboardKeywordsPluginTabId = OpenKneeboardKeywordsPluginId + ";keywords";

                public static void Initialize()
                {
                    ResetSnapshot();
                    RefreshDtcFilesSnapshot();
                    SetPluginRegistration(State.activeconfig != null && State.activeconfig.OpenKneeboard_Out);
                    StartWebHost();
                }

                public static void SetEnabled(bool enabled)
                {
                    if (enabled)
                    {
                        StopWebHost();
                    }

                    SetPluginRegistration(enabled);
                    SetKeywordsPluginRegistration();

                    if (enabled)
                    {
                        StartWebHost();
                    }
                    else
                    {
                        StopWebHost();
                    }
                }

                public static bool IsHostRunning
                {
                    get { return isRunning; }
                }

                public static bool HasActiveConnection
                {
                    get
                    {
                        if (!isRunning)
                        {
                            return false;
                        }

                        long ticks = Interlocked.Read(ref lastClientRequestUtcTicks);
                        if (ticks <= 0)
                        {
                            return false;
                        }

                        DateTime lastRequestUtc = new DateTime(ticks, DateTimeKind.Utc);
                        return (DateTime.UtcNow - lastRequestUtc) <= TimeSpan.FromSeconds(3);
                    }
                }

                private static void SetKeywordsPluginRegistration()
                {
                    try
                    {
                        string manifestPath = GetKeywordsPluginManifestPath();
                        if (string.IsNullOrWhiteSpace(manifestPath))
                        {
                            return;
                        }

                        WriteKeywordsPluginManifest(manifestPath, "");

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

                private static int GetOpenKneeboardOutPort()
                {
                    int configuredPort = DefaultOpenKneeboardOutPort;

                    try
                    {
                        if (State.activeconfig != null)
                        {
                            configuredPort = State.activeconfig.OpenKneeboard_Out_Port;
                        }
                    }
                    catch
                    {
                    }

                    if (configuredPort <= 0 || configuredPort > 65535)
                    {
                        configuredPort = DefaultOpenKneeboardOutPort;
                    }

                    return configuredPort;
                }

                private static string GetPrefix()
                {
                    return "http://127.0.0.1:" + GetOpenKneeboardOutPort() + "/okb/";
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

                            if (enabled)
                            {
                                DeactivateAllOpenKneeboardOutRegistrations(key);
                                CleanupStaleOpenKneeboardOutManifestFiles(manifestPath);

                                key.SetValue(manifestPath, 1, RegistryValueKind.DWord);
                            }
                            else
                            {
                                DeactivateAllOpenKneeboardOutRegistrations(key);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write("OpenKneeboard plugin registration failed: " + ex.Message, Colors.Warning);
                    }
                }

                private static void DeactivateAllOpenKneeboardOutRegistrations(RegistryKey key)
                {
                    try
                    {
                        foreach (string valueName in key.GetValueNames())
                        {
                            if (string.IsNullOrWhiteSpace(valueName))
                            {
                                continue;
                            }

                            string fileName = Path.GetFileName(valueName);
                            if (string.IsNullOrWhiteSpace(fileName))
                            {
                                continue;
                            }

                            if (!fileName.StartsWith("OpenKneeboard", StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            if (fileName.IndexOf("OpenKneeboard.Keywords", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                continue;
                            }

                            key.SetValue(valueName, 0, RegistryValueKind.DWord);
                        }
                    }
                    catch
                    {
                    }
                }

                private static void CleanupStaleOpenKneeboardOutManifestFiles(string activeManifestPath)
                {
                    try
                    {
                        string activeFullPath = Path.GetFullPath(activeManifestPath);
                        string manifestFolder = Path.GetDirectoryName(activeFullPath);
                        if (string.IsNullOrWhiteSpace(manifestFolder) || !Directory.Exists(manifestFolder))
                        {
                            return;
                        }

                        string[] staleManifests = Directory.GetFiles(manifestFolder, "OpenKneeboard*.v1.json");
                        foreach (string staleManifest in staleManifests)
                        {
                            string staleManifestFileName = Path.GetFileName(staleManifest);
                            if (string.IsNullOrWhiteSpace(staleManifestFileName))
                            {
                                continue;
                            }

                            if (staleManifestFileName.IndexOf("OpenKneeboard.Keywords", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                continue;
                            }

                            string staleFullPath = Path.GetFullPath(staleManifest);
                            if (string.Equals(staleFullPath, activeFullPath, StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            try
                            {
                                File.Delete(staleManifest);
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
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

                        return Path.Combine(outputFolder, "OpenKneeboard." + GetOpenKneeboardOutPort() + ".v1.json");
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
                                        URI = GetPrefix(),
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

                    RefreshDtcFilesSnapshot();
                }

                public static void UpdateActiveCategory(string category)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    string sourceCategory = category.Trim();
                    string sendCategory = ResolveOpenKneeboardCategory(sourceCategory);

                    lock (Sync)
                    {
                        snapshot.ActiveCategory = sendCategory.ToUpperInvariant();
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }

                    RefreshOpenKneeboardCategoryData(sourceCategory, sendCategory);
                }

                private static string ResolveOpenKneeboardCategory(string category)
                {
                    string cat = (category ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(cat)) return "LOG";

                    if (State.AIRIOactive && (cat.Equals("RIO", StringComparison.OrdinalIgnoreCase) || cat.Equals("Iceman", StringComparison.OrdinalIgnoreCase)))
                    {
                        return "REF";
                    }

                    if (cat.Equals("Crew", StringComparison.OrdinalIgnoreCase))
                    {
                        return "REF";
                    }

                    if (cat.Equals("Allies", StringComparison.OrdinalIgnoreCase))
                    {
                        return "FLIGHT";
                    }

                    if (cat.IndexOf("ATC", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return "ATC";
                    }

                    if (cat.IndexOf("AWACS", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return "AWACS";
                    }

                    if (cat.IndexOf("TANK", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return "TANKER";
                    }

                    if (cat.IndexOf("JTAC", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return "JTAC";
                    }

                    return cat;
                }

                private static void RefreshOpenKneeboardCategoryData(string sourceCategory, string sendCategory)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(sendCategory)) return;

                        if (!sendCategory.Equals("NOTES", StringComparison.OrdinalIgnoreCase)
                            && !sendCategory.Equals("LOG", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!sendCategory.Equals("REF", StringComparison.OrdinalIgnoreCase))
                            {
                                RefreshUnitsForCategory(sendCategory);
                            }
                            else if (sourceCategory.Equals("Crew", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    KneeboardUnitsData crewUnits = new KneeboardUnitsData("Crew", false);
                                    UpdateUnits("CREW", crewUnits.unitslist);
                                }
                                catch
                                {
                                }
                            }

                            if (State.KneeboardCatAliasStrings != null)
                            {
                                for (int i = 0; i < State.KneeboardCatAliasStrings.Length; i++)
                                {
                                    Dictionary<string, SortedDictionary<string, List<string>>> chunk = State.KneeboardCatAliasStrings[i];
                                    SortedDictionary<string, List<string>> aliasStrings = new SortedDictionary<string, List<string>>();

                                    if (chunk != null)
                                    {
                                        SortedDictionary<string, List<string>> fromSource;
                                        SortedDictionary<string, List<string>> fromSend;
                                        if (TryGetAliasChunkCategory(chunk, sourceCategory, out fromSource))
                                        {
                                            aliasStrings = fromSource;
                                        }
                                        else if (TryGetAliasChunkCategory(chunk, sendCategory, out fromSend))
                                        {
                                            aliasStrings = fromSend;
                                        }
                                    }

                                    UpdateAliasChunk(sendCategory, i, aliasStrings);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                private static bool TryGetAliasChunkCategory(Dictionary<string, SortedDictionary<string, List<string>>> chunk, string category, out SortedDictionary<string, List<string>> aliasStrings)
                {
                    aliasStrings = null;
                    if (chunk == null || string.IsNullOrWhiteSpace(category)) return false;

                    if (chunk.TryGetValue(category, out aliasStrings))
                    {
                        return aliasStrings != null;
                    }

                    foreach (KeyValuePair<string, SortedDictionary<string, List<string>>> entry in chunk)
                    {
                        if (entry.Key.Equals(category, StringComparison.OrdinalIgnoreCase))
                        {
                            aliasStrings = entry.Value;
                            return aliasStrings != null;
                        }
                    }

                    return false;
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

                public static void ForceRefreshOutDashboardData()
                {
                    UpdateServerData();
                    RefreshUnitsForCategory("ATC");
                    RefreshUnitsForCategory("AWACS");
                    RefreshUnitsForCategory("Tanker");
                }

                public static void ForceRefreshFriendlyAssetsData()
                {
                    UpdateServerData();
                }

                private static void RefreshUnitsForCategory(string category)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    try
                    {
                        KneeboardUnitsData units = new KneeboardUnitsData(category, false);
                        UpdateUnits(category, units.unitslist);
                    }
                    catch
                    {
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
                            foreach (KeyValuePair<string, string> alias in Database.Aliases.aicommands)
                            {
                                if (alias.Value != null && alias.Value.StartsWith("wMsgWSO_", StringComparison.OrdinalIgnoreCase))
                                {
                                    keywords.Add(alias.Key);
                                }
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
                        "Radar Lock Target [1..20]",
                        "On Station For [15; 30; 45; 60] Minutes"
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
                        string prefix = GetPrefix();
                        listener = new HttpListener();
                        listener.Prefixes.Add(prefix);
                        listener.Start();

                        Interlocked.Exchange(ref lastClientRequestUtcTicks, 0);
                        isRunning = true;
                        listenerThread = new Thread(ListenLoop) { IsBackground = true, Name = "OpenKneeboardWebHost" };
                        listenerThread.Start();

                        try
                        {
                            State.configurationwindow?.Dispatcher.BeginInvoke((Action)delegate
                            {
                                State.configurationwindow.ChangeOKHostbug();
                            });
                        }
                        catch
                        {
                        }

                        Log.Write("OpenKneeboard dashboard host started at " + prefix, Colors.Text);
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

                        try
                        {
                            State.configurationwindow?.Dispatcher.BeginInvoke((Action)delegate
                            {
                                State.configurationwindow.ChangeOKHostbug();
                            });
                        }
                        catch
                        {
                        }
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
                    Interlocked.Exchange(ref lastClientRequestUtcTicks, DateTime.UtcNow.Ticks);
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
                        WriteText(context.Response, GetIndexHtmlWithStoreLookup(), "text/html; charset=utf-8");
                        return;
                    }

                    if (path == "/okb/state" || path == "/okb/index.json")
                    {
                        WriteJson(context.Response, BuildSnapshotJson());
                        return;
                    }

                    if (path == "/okb/clock")
                    {
                        WriteJson(context.Response, BuildClockJson());
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

                    if (path == "/okb/dev/refresh")
                    {
                        ForceRefreshOutDashboardData();
                        WriteJson(context.Response, "{\"ok\":true}");
                        return;
                    }

                    if (path == "/okb/dtc/list")
                    {
                        RefreshDtcFilesSnapshot();
                        WriteJson(context.Response, BuildSnapshotJson());
                        return;
                    }

                    if (path == "/okb/dtc/select")
                    {
                        string file = context.Request.QueryString["file"] ?? "";
                        bool ok = SelectDtcFileSnapshot(file);
                        if (!ok)
                        {
                            context.Response.StatusCode = 400;
                        }

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

                private static string GetIndexHtmlWithStoreLookup()
                {
                    try
                    {
                        return IndexHtml.Replace(StoreLookupJsonPlaceholder, GetStoreLookupMapJson());
                    }
                    catch
                    {
                        return IndexHtml.Replace(StoreLookupJsonPlaceholder, "{}");
                    }
                }

                private static string GetStoreLookupMapJson()
                {
                    lock (StoreLookupSync)
                    {
                        try
                        {
                            string path = ResolveStoreLookupPath();
                            string raw = "";

                            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                            {
                                DateTime lastWrite = File.GetLastWriteTimeUtc(path);
                                bool refresh = !string.Equals(path, storeLookupResolvedPath, StringComparison.OrdinalIgnoreCase)
                                    || lastWrite != storeLookupLastWriteUtc;

                                if (!refresh)
                                {
                                    return storeLookupMapJson;
                                }

                                raw = File.ReadAllText(path);
                                storeLookupResolvedPath = path;
                                storeLookupLastWriteUtc = lastWrite;
                            }
                            else
                            {
                                if (!TryReadEmbeddedStoreLookupJson(out raw))
                                {
                                    return storeLookupMapJson;
                                }

                                storeLookupResolvedPath = "<embedded>";
                                storeLookupLastWriteUtc = DateTime.MinValue;
                            }

                            JObject parsed = JObject.Parse(raw);

                            JToken mapToken = null;
                            JProperty mapProperty = parsed.Properties()
                                .FirstOrDefault(p => string.Equals(p.Name, "map", StringComparison.OrdinalIgnoreCase));

                            if (mapProperty != null && mapProperty.Value != null && mapProperty.Value.Type == JTokenType.Object)
                            {
                                mapToken = mapProperty.Value;
                            }
                            else
                            {
                                mapToken = parsed;
                            }

                            storeLookupMapJson = mapToken.ToString(Formatting.None);

                        }
                        catch
                        {
                            if (string.IsNullOrWhiteSpace(storeLookupMapJson))
                            {
                                storeLookupMapJson = "{}";
                            }
                        }

                        return string.IsNullOrWhiteSpace(storeLookupMapJson) ? "{}" : storeLookupMapJson;
                    }
                }

                private static string ResolveStoreLookupPath()
                {
                    try
                    {
                        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                        if (string.IsNullOrWhiteSpace(baseDir))
                        {
                            return "";
                        }

                        string[] candidates = new[]
                        {
                            Path.Combine(baseDir, "Helpers", "StoreClsidLookup.json"),
                            Path.Combine(baseDir, "StoreClsidLookup.json")
                        };

                        foreach (string candidate in candidates)
                        {
                            if (!string.IsNullOrWhiteSpace(candidate) && File.Exists(candidate))
                            {
                                return candidate;
                            }
                        }
                    }
                    catch
                    {
                    }

                    return "";
                }

                private static bool TryReadEmbeddedStoreLookupJson(out string rawJson)
                {
                    rawJson = "";

                    try
                    {
                        Assembly asm = typeof(OpenKneeboardBridge).Assembly;
                        string resourceName = asm
                            .GetManifestResourceNames()
                            .FirstOrDefault(n => n.EndsWith("Helpers.StoreClsidLookup.json", StringComparison.OrdinalIgnoreCase));

                        if (string.IsNullOrWhiteSpace(resourceName))
                        {
                            return false;
                        }

                        using (Stream stream = asm.GetManifestResourceStream(resourceName))
                        {
                            if (stream == null)
                            {
                                return false;
                            }

                            using (StreamReader reader = new StreamReader(stream))
                            {
                                rawJson = reader.ReadToEnd();
                            }
                        }

                        return !string.IsNullOrWhiteSpace(rawJson);
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static string BuildSnapshotJson()
                {
                    OpenKneeboardSnapshot responseModel;

                    lock (Sync)
                    {
                        responseModel = snapshot.Clone();
                    }

                    try
                    {
                        return JsonConvert.SerializeObject(responseModel, Formatting.Indented);
                    }
                    catch
                    {
                        var fallback = new
                        {
                            ActiveCategory = responseModel.ActiveCategory,
                            NotesBuffer = responseModel.NotesBuffer,
                            AiCrewPhase = responseModel.AiCrewPhase,
                            UpdatedUtc = responseModel.UpdatedUtc,
                            Server = new
                            {
                                Theater = responseModel.Server?.Theater ?? "",
                                DcsLocation = responseModel.Server?.DcsLocation ?? "",
                                Aircraft = responseModel.Server?.Aircraft ?? "",
                                PlayerUsername = responseModel.Server?.PlayerUsername ?? "",
                                PlayerCallsign = responseModel.Server?.PlayerCallsign ?? "",
                                MissionTitle = responseModel.Server?.MissionTitle ?? "",
                                MissionBriefing = responseModel.Server?.MissionBriefing ?? "",
                                MissionDetails = responseModel.Server?.MissionDetails ?? "",
                                MissionTimeSeconds = responseModel.Server?.MissionTimeSeconds ?? 0,
                                PlayerPosX = responseModel.Server?.PlayerPosX ?? 0,
                                PlayerPosY = responseModel.Server?.PlayerPosY ?? 0,
                                PlayerAltFeet = responseModel.Server?.PlayerAltFeet ?? 0,
                                Multiplayer = responseModel.Server?.Multiplayer ?? false,
                                DebugMode = responseModel.Server?.DebugMode ?? false,
                                AtcMetars = responseModel.Server?.AtcMetars ?? new Dictionary<string, string>(),
                                FlightMembers = responseModel.Server?.FlightMembers ?? new List<OpenKneeboardFlightMember>(),
                                Diagnostics = (object)null,
                            },
                            Status = responseModel.Status,
                            AiCrewKeywords = responseModel.AiCrewKeywords,
                            RawServerMessages = responseModel.RawServerMessages,
                            Logs = responseModel.Logs,
                            Units = responseModel.Units,
                            UnitDetails = responseModel.UnitDetails,
                            AliasesChunk0 = responseModel.AliasesChunk0,
                            AliasesChunk1 = responseModel.AliasesChunk1,
                            DtcFiles = responseModel.DtcFiles,
                            DtcSelectedFile = responseModel.DtcSelectedFile,
                            DtcJson = responseModel.DtcJson,
                            DtcSourceType = responseModel.DtcSourceType,
                        };

                        return JsonConvert.SerializeObject(fallback, Formatting.Indented);
                    }
                }

                private static string BuildClockJson()
                {
                    double missionTimeSeconds = 0;
                    string missionIdentity = "";
                    DateTime sourceUtc = DateTime.UtcNow;

                    try
                    {
                        if (State.currentstate != null)
                        {
                            double missionStartSeconds = 0;
                            double missionElapsedSeconds = State.currentstate.timer;
                            bool hasMissionStart = double.TryParse(State.currentstate.sortie ?? "", out missionStartSeconds);
                            if (hasMissionStart && missionElapsedSeconds >= 0)
                            {
                                missionTimeSeconds = missionStartSeconds + missionElapsedSeconds;
                            }
                            else
                            {
                                missionTimeSeconds = State.currentstate.tod;
                            }

                            missionIdentity = string.Join("|", new[]
                            {
                                State.currentstate.theatre ?? "",
                                State.currentstate.missiontitle ?? "",
                                State.currentstate.id ?? "",
                                State.currentstate.playercallsign ?? "",
                                State.currentstate.multiplayer ? "1" : "0"
                            });
                        }
                    }
                    catch
                    {
                    }

                    var payload = new
                    {
                        MissionTimeSeconds = missionTimeSeconds,
                        SourceUtc = sourceUtc,
                        MissionIdentity = missionIdentity,
                    };

                    return JsonConvert.SerializeObject(payload, Formatting.None);
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

                private static void RefreshDtcFilesSnapshot()
                {
                    try
                    {
                        List<string> files = GetAvailableDtcFiles();

                        lock (Sync)
                        {
                            snapshot.DtcFiles = files;

                            if (!string.IsNullOrWhiteSpace(snapshot.DtcSelectedFile))
                            {
                                string selected = FindMatchingFlightPlanSelection(snapshot.DtcSelectedFile, files);
                                if (string.IsNullOrWhiteSpace(selected))
                                {
                                    snapshot.DtcSelectedFile = "";
                                    snapshot.DtcJson = "";
                                }
                                else
                                {
                                    snapshot.DtcSelectedFile = selected;
                                    if (TryReadValidatedFlightPlan(selected, out string json, out string sourceType))
                                    {
                                        snapshot.DtcJson = json;
                                        snapshot.DtcSourceType = sourceType;
                                    }
                                }
                            }

                            snapshot.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    catch
                    {
                    }
                }

                private static bool SelectDtcFileSnapshot(string file)
                {
                    lock (Sync)
                    {
                        if (string.IsNullOrWhiteSpace(file))
                        {
                            snapshot.DtcSelectedFile = "";
                            snapshot.DtcJson = "";
                            snapshot.DtcSourceType = "";
                            snapshot.UpdatedUtc = DateTime.UtcNow;
                            return true;
                        }

                        string selected = FindMatchingFlightPlanSelection(file, snapshot.DtcFiles ?? new List<string>());

                        if (string.IsNullOrWhiteSpace(selected))
                        {
                            return false;
                        }

                        if (!TryReadValidatedFlightPlan(selected, out string json, out string sourceType))
                        {
                            return false;
                        }

                        snapshot.DtcSelectedFile = selected;
                        snapshot.DtcJson = json;
                        snapshot.DtcSourceType = sourceType;
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                        return true;
                    }
                }

                private static string FindMatchingFlightPlanSelection(string requested, List<string> candidates)
                {
                    try
                    {
                        List<string> list = candidates ?? new List<string>();
                        string req = requested ?? "";

                        string direct = list.FirstOrDefault(f => string.Equals(f, req, StringComparison.OrdinalIgnoreCase));
                        if (!string.IsNullOrWhiteSpace(direct))
                        {
                            return direct;
                        }

                        string reqPath;
                        string reqRoute;
                        if (TryParseRouteSelectionToken(req, out reqPath, out reqRoute))
                        {
                            foreach (string c in list)
                            {
                                string cPath;
                                string cRoute;
                                if (!TryParseRouteSelectionToken(c, out cPath, out cRoute))
                                {
                                    continue;
                                }

                                if (string.Equals(reqRoute, cRoute, StringComparison.OrdinalIgnoreCase)
                                    && string.Equals(Path.GetFullPath(reqPath ?? ""), Path.GetFullPath(cPath ?? ""), StringComparison.OrdinalIgnoreCase))
                                {
                                    return c;
                                }
                            }
                        }

                        if (req.IndexOf(RouteSelectionPrefix, StringComparison.Ordinal) < 0)
                        {
                            string reqFull = Path.GetFullPath(req);
                            string pathMatch = list.FirstOrDefault(c =>
                            {
                                try { return string.Equals(Path.GetFullPath(c ?? ""), reqFull, StringComparison.OrdinalIgnoreCase); }
                                catch { return false; }
                            });
                            if (!string.IsNullOrWhiteSpace(pathMatch))
                            {
                                return pathMatch;
                            }
                        }
                    }
                    catch
                    {
                    }

                    return "";
                }

                private static List<string> GetAvailableDtcFiles()
                {
                    List<string> files = new List<string>();

                    foreach (string dtcDir in GetSavedGamesDtcFolders())
                    {
                        try
                        {
                            if (!Directory.Exists(dtcDir))
                            {
                                continue;
                            }

                            foreach (string candidate in Directory.EnumerateFiles(dtcDir, "*.*", SearchOption.AllDirectories))
                            {
                                if (!IsDtcExtension(candidate))
                                {
                                    continue;
                                }

                                if (!IsPathUnderDirectory(candidate, dtcDir))
                                {
                                    continue;
                                }

                                if (!TryReadValidatedFlightPlan(candidate, out _, out _))
                                {
                                    continue;
                                }

                                files.Add(Path.GetFullPath(candidate));
                                if (files.Count >= 250)
                                {
                                    break;
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (files.Count >= 250)
                        {
                            break;
                        }
                    }

                    foreach (string missionsDir in GetSavedGamesMissionFolders())
                    {
                        try
                        {
                            if (!Directory.Exists(missionsDir))
                            {
                                continue;
                            }

                            foreach (string routeCandidate in Directory.EnumerateFiles(missionsDir, "*.rte", SearchOption.AllDirectories))
                            {
                                if (!IsPathUnderDirectory(routeCandidate, missionsDir))
                                {
                                    continue;
                                }

                                if (!TryReadValidatedFlightPlan(routeCandidate, out _, out _))
                                {
                                    continue;
                                }

                                files.Add(Path.GetFullPath(routeCandidate));
                                if (files.Count >= 250)
                                {
                                    break;
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (files.Count >= 250)
                        {
                            break;
                        }
                    }

                    foreach (string routeToolFile in GetRouteToolPresetFiles())
                    {
                        try
                        {
                            if (string.IsNullOrWhiteSpace(routeToolFile) || !File.Exists(routeToolFile))
                            {
                                continue;
                            }

                            List<string> routeNames = ExtractRouteNamesFromLua(routeToolFile);
                            if (routeNames.Count == 0)
                            {
                                continue;
                            }

                            foreach (string routeName in routeNames)
                            {
                                string token = RouteSelectionPrefix + Uri.EscapeDataString(routeName) + "::" + Path.GetFullPath(routeToolFile);
                                files.Add(token);
                                if (files.Count >= 250)
                                {
                                    break;
                                }
                            }

                            if (files.Count >= 250)
                            {
                                break;
                            }
                        }
                        catch
                        {
                        }

                        if (files.Count >= 250)
                        {
                            break;
                        }
                    }

                    return files
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderByDescending(p =>
                        {
                            try
                            {
                                string routePath;
                                string routeName;
                                if (TryParseRouteSelectionToken(p, out routePath, out routeName))
                                {
                                    return File.GetLastWriteTimeUtc(routePath);
                                }

                                return File.GetLastWriteTimeUtc(p);
                            }
                            catch { return DateTime.MinValue; }
                        })
                        .ThenBy(p => p, StringComparer.OrdinalIgnoreCase)
                        .ToList();
                }

                private static List<string> GetSavedGamesDtcFolders()
                {
                    List<string> roots = new List<string>();

                    try
                    {
                        foreach (string savedGames in GetSavedGamesRoots())
                        {
                            if (!Directory.Exists(savedGames))
                            {
                                continue;
                            }

                            foreach (string dcsRoot in Directory.EnumerateDirectories(savedGames, "DCS*", SearchOption.TopDirectoryOnly))
                            {
                                try
                                {
                                    string name = Path.GetFileName(dcsRoot);
                                    if (string.IsNullOrWhiteSpace(name) || !name.StartsWith("DCS", StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }

                                    roots.Add(Path.Combine(dcsRoot, "DTC"));
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    return roots
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();
                }

                private static List<string> GetRouteToolPresetFiles()
                {
                    List<string> files = new List<string>();

                    try
                    {
                        foreach (string savedGames in GetSavedGamesRoots())
                        {
                            if (!Directory.Exists(savedGames))
                            {
                                continue;
                            }

                            foreach (string dcsRoot in Directory.EnumerateDirectories(savedGames, "DCS*", SearchOption.TopDirectoryOnly))
                            {
                                try
                                {
                                    string name = Path.GetFileName(dcsRoot);
                                    if (string.IsNullOrWhiteSpace(name) || !name.StartsWith("DCS", StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }

                                    string routeDir = Path.Combine(dcsRoot, "Config", "RouteToolPresets");
                                    if (!Directory.Exists(routeDir))
                                    {
                                        continue;
                                    }

                                    string activeTheater = string.IsNullOrWhiteSpace(State.currentstate == null ? "" : State.currentstate.theatre)
                                        ? ""
                                        : State.currentstate.theatre.Trim();

                                    if (!string.IsNullOrWhiteSpace(activeTheater))
                                    {
                                        string activeFile = Path.Combine(routeDir, activeTheater + ".lua");
                                        if (File.Exists(activeFile))
                                        {
                                            files.Add(Path.GetFullPath(activeFile));
                                            continue;
                                        }
                                    }

                                    foreach (string fallback in Directory.EnumerateFiles(routeDir, "*.lua", SearchOption.TopDirectoryOnly))
                                    {
                                        files.Add(Path.GetFullPath(fallback));
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    return files
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                        .ToList();
                }

                private static List<string> GetSavedGamesMissionFolders()
                {
                    List<string> roots = new List<string>();

                    try
                    {
                        foreach (string savedGames in GetSavedGamesRoots())
                        {
                            if (!Directory.Exists(savedGames))
                            {
                                continue;
                            }

                            foreach (string dcsRoot in Directory.EnumerateDirectories(savedGames, "DCS*", SearchOption.TopDirectoryOnly))
                            {
                                try
                                {
                                    string name = Path.GetFileName(dcsRoot);
                                    if (string.IsNullOrWhiteSpace(name) || !name.StartsWith("DCS", StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }

                                    roots.Add(Path.Combine(dcsRoot, "Missions"));
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    return roots
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();
                }

                private static List<string> GetSavedGamesRoots()
                {
                    List<string> roots = new List<string>();

                    try
                    {
                        string knownFolderPath = GetKnownFolderPath(SavedGamesFolderId);
                        if (!string.IsNullOrWhiteSpace(knownFolderPath))
                        {
                            roots.Add(Path.GetFullPath(knownFolderPath));
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        if (!string.IsNullOrWhiteSpace(userProfile))
                        {
                            roots.Add(Path.GetFullPath(Path.Combine(userProfile, "Saved Games")));
                        }
                    }
                    catch
                    {
                    }

                    return roots
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();
                }

                private static string GetKnownFolderPath(Guid folderId)
                {
                    IntPtr outPath = IntPtr.Zero;

                    try
                    {
                        int hr = SHGetKnownFolderPath(folderId, 0, IntPtr.Zero, out outPath);
                        if (hr != 0 || outPath == IntPtr.Zero)
                        {
                            return "";
                        }

                        return Marshal.PtrToStringUni(outPath) ?? "";
                    }
                    catch
                    {
                        return "";
                    }
                    finally
                    {
                        if (outPath != IntPtr.Zero)
                        {
                            Marshal.FreeCoTaskMem(outPath);
                        }
                    }
                }

                private static bool IsDtcExtension(string path)
                {
                    try
                    {
                        string ext = Path.GetExtension(path);
                        return DtcFileExtensions.Any(x => string.Equals(x, ext, StringComparison.OrdinalIgnoreCase));
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static bool IsPathUnderDirectory(string filePath, string rootDirectory)
                {
                    try
                    {
                        string fullFile = Path.GetFullPath(filePath ?? "");
                        string fullRoot = Path.GetFullPath(rootDirectory ?? "").TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                            + Path.DirectorySeparatorChar;

                        return fullFile.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase);
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static bool TryReadValidatedFlightPlan(string filePath, out string json, out string sourceType)
                {
                    json = "";
                    sourceType = "";

                    try
                    {
                        string routeSelectionPath;
                        string routeSelectionName;
                        if (TryParseRouteSelectionToken(filePath, out routeSelectionPath, out routeSelectionName))
                        {
                            return TryReadRouteSelection(routeSelectionPath, routeSelectionName, out json, out sourceType);
                        }

                        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                        {
                            return false;
                        }

                        string raw = File.ReadAllText(filePath);
                        if (string.IsNullOrWhiteSpace(raw))
                        {
                            return false;
                        }

                        string ext = Path.GetExtension(filePath ?? "");
                        if (string.Equals(ext, ".rte", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(ext, ".lua", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!TryConvertRouteToolLuaToJson(raw, out string rteJson))
                            {
                                return false;
                            }

                            JObject rteParsed = JObject.Parse(rteJson);
                            json = rteParsed.ToString(Formatting.None);
                            sourceType = "RTE";
                            return true;
                        }

                        JObject parsed = JObject.Parse(raw);
                        if (parsed == null)
                        {
                            return false;
                        }

                        JToken dataToken;
                        if (parsed.TryGetValue("data", StringComparison.OrdinalIgnoreCase, out dataToken))
                        {
                            if (dataToken == null || dataToken.Type != JTokenType.Object)
                            {
                                return false;
                            }
                        }

                        json = parsed.ToString(Formatting.None);
                        sourceType = "DTC";
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static bool TryReadRouteSelection(string luaFilePath, string routeName, out string json, out string sourceType)
                {
                    json = "";
                    sourceType = "";

                    try
                    {
                        if (string.IsNullOrWhiteSpace(luaFilePath) || !File.Exists(luaFilePath))
                        {
                            return false;
                        }

                        string raw = File.ReadAllText(luaFilePath);
                        if (!TryConvertRouteToolLuaToJson(raw, out string rteJson))
                        {
                            return false;
                        }

                        JObject parsed = JObject.Parse(rteJson);
                        JObject data = parsed["data"] as JObject;
                        if (data == null)
                        {
                            return false;
                        }

                        JToken selectedRoute;
                        if (!data.TryGetValue(routeName ?? "", StringComparison.OrdinalIgnoreCase, out selectedRoute))
                        {
                            return false;
                        }

                        JObject wrapped = new JObject();
                        JObject routeContainer = new JObject();
                        routeContainer[routeName] = selectedRoute;
                        wrapped["data"] = routeContainer;

                        json = wrapped.ToString(Formatting.None);
                        sourceType = "RTE";
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static bool TryParseRouteSelectionToken(string token, out string luaFilePath, out string routeName)
                {
                    luaFilePath = "";
                    routeName = "";

                    try
                    {
                        string text = token ?? "";
                        if (!text.StartsWith(RouteSelectionPrefix, StringComparison.Ordinal))
                        {
                            return false;
                        }

                        int sep = text.IndexOf("::", RouteSelectionPrefix.Length, StringComparison.Ordinal);
                        if (sep < 0)
                        {
                            return false;
                        }

                        string encodedRoute = text.Substring(RouteSelectionPrefix.Length, sep - RouteSelectionPrefix.Length);
                        routeName = Uri.UnescapeDataString(encodedRoute ?? "");
                        luaFilePath = text.Substring(sep + 2);

                        return !string.IsNullOrWhiteSpace(routeName) && !string.IsNullOrWhiteSpace(luaFilePath);
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static List<string> ExtractRouteNamesFromLua(string luaFilePath)
                {
                    List<string> routes = new List<string>();

                    try
                    {
                        if (string.IsNullOrWhiteSpace(luaFilePath) || !File.Exists(luaFilePath))
                        {
                            return routes;
                        }

                        string raw = File.ReadAllText(luaFilePath);
                        if (!TryConvertRouteToolLuaToJson(raw, out string rteJson))
                        {
                            return routes;
                        }

                        JObject parsed = JObject.Parse(rteJson);
                        JObject data = parsed["data"] as JObject;
                        if (data == null)
                        {
                            return routes;
                        }

                        routes = data.Properties()
                            .Select(p => p.Name)
                            .Where(n => !string.IsNullOrWhiteSpace(n))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                            .ToList();
                    }
                    catch
                    {
                    }

                    return routes;
                }

                private static bool TryConvertRouteToolLuaToJson(string luaText, out string json)
                {
                    json = "";

                    try
                    {
                        if (string.IsNullOrWhiteSpace(luaText))
                        {
                            return false;
                        }

                        string s = luaText;
                        s = Regex.Replace(s, @"--.*?$", "", RegexOptions.Multiline);
                        s = Regex.Replace(s, @"\bend of\b[\s\S]*$", "", RegexOptions.IgnoreCase);
                        s = Regex.Replace(s, @"\bpresets\s*=", "", RegexOptions.IgnoreCase);
                        s = s.Replace("\r", " ").Replace("\n", " ");
                        s = Regex.Replace(s, @"\s+", " ");

                        s = Regex.Replace(s, @"\[(\d+)\]\s*=", "\"$1\":");
                        s = Regex.Replace(s, @"\[\s*""((?:\\.|[^""\\])*)""\s*\]\s*=", "\"$1\":");
                        s = Regex.Replace(s, @"\b([A-Za-z_][A-Za-z0-9_]*)\s*=", "\"$1\":");

                        s = s.Replace("'", "\"");
                        s = Regex.Replace(s, @",\s*([}\]])", "$1");
                        s = s.Trim();

                        if (!s.StartsWith("{", StringComparison.Ordinal))
                        {
                            return false;
                        }

                        JToken parsed = JToken.Parse(s);
                        JObject wrapped = new JObject
                        {
                            ["data"] = parsed
                        };

                        json = wrapped.ToString(Formatting.None);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
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
                            server.PlayerUsername = State.currentstate.playerusername;
                            server.PlayerCallsign = State.currentstate.playercallsign;
                            server.MissionTitle = State.currentstate.missiontitle;
                            server.MissionBriefing = State.currentstate.missionbriefing;
                            server.MissionDetails = State.currentstate.missiondetails;
                            double missionStartSeconds = 0;
                            double missionElapsedSeconds = State.currentstate.timer;
                            bool hasMissionStart = double.TryParse(State.currentstate.sortie ?? "", out missionStartSeconds);
                            if (hasMissionStart && missionElapsedSeconds >= 0)
                            {
                                server.MissionTimeSeconds = missionStartSeconds + missionElapsedSeconds;
                            }
                            else
                            {
                                server.MissionTimeSeconds = State.currentstate.tod;
                            }
                            server.PlayerPosX = State.currentstate.bpos == null ? 0 : State.currentstate.bpos.x;
                            server.PlayerPosY = State.currentstate.bpos == null ? 0 : State.currentstate.bpos.z;
                            server.PlayerAltFeet = State.currentstate.bpos == null ? 0 : (State.currentstate.bpos.y * 3.28084);
                            server.Multiplayer = State.currentstate.multiplayer;
                            server.DebugMode = State.activeconfig.Debugmode;
                            server.Payload = State.currentstate.payload;
                            server.Radios = State.currentstate.radios == null
                                ? new List<Servers.Server.RadioDevice>()
                                : new List<Servers.Server.RadioDevice>(State.currentstate.radios);
                            server.AtcMetars = State.currentstate.atcmetars == null
                                ? new Dictionary<string, string>()
                                : new Dictionary<string, string>(State.currentstate.atcmetars);
                            server.AtcIcaoTypes = State.currentstate.atcicaotypes == null
                                ? new Dictionary<string, string>()
                                : new Dictionary<string, string>(State.currentstate.atcicaotypes);
                            server.Diagnostics = State.currentstate.diagnostics;
                            server.FlightMembers = BuildFlightMemberSnapshot();
                            server.FriendlyAssets = BuildFriendlyAssetsSnapshot();
                            server.MapMarkers = BuildMapMarkerSnapshot();
                        }
                    }
                    catch
                    {
                    }

                    return server;
                }

                private static List<OpenKneeboardFlightMember> BuildFlightMemberSnapshot()
                {
                    List<OpenKneeboardFlightMember> members = new List<OpenKneeboardFlightMember>();

                    try
                    {
                        if (State.currentstate == null)
                        {
                            return members;
                        }

                        string playerCallsign = string.IsNullOrWhiteSpace(State.currentstate.playercallsign) ? "" : State.currentstate.playercallsign.Trim();
                        string playerName = string.IsNullOrWhiteSpace(State.currentstate.playerusername) ? "" : State.currentstate.playerusername.Trim();
                        int playerJet = ParseJetPositionFromCallsign(playerCallsign);

                        if (!string.IsNullOrWhiteSpace(playerCallsign) || !string.IsNullOrWhiteSpace(playerName))
                        {
                            members.Add(new OpenKneeboardFlightMember
                            {
                                Callsign = playerCallsign,
                                Pilot = playerName,
                                Jet = playerJet > 0 ? playerJet : 1,
                            });
                        }

                        List<Servers.Server.DcsUnit> flightUnits = null;
                        if (State.currentstate.availablerecipients != null)
                        {
                            State.currentstate.availablerecipients.TryGetValue("Flight", out flightUnits);
                        }

                        if (flightUnits != null)
                        {
                            foreach (Servers.Server.DcsUnit unit in flightUnits
                                .Where(u => u != null)
                                .OrderBy(u => u.range)
                                .ThenBy(u => u.callsign, StringComparer.OrdinalIgnoreCase))
                            {
                                string callsign = string.IsNullOrWhiteSpace(unit.callsign)
                                    ? (unit.fullname ?? "")
                                    : unit.callsign.Trim();

                                string pilot = "";
                                if (unit.ishuman)
                                {
                                    pilot = string.IsNullOrWhiteSpace(unit.playerid) ? "" : unit.playerid.Trim();
                                }

                                int jet = ParseJetPositionFromCallsign(callsign);
                                UpsertFlightMember(members, callsign, pilot, jet);
                            }
                        }

                        FillMissingFlightJets(members);

                        members = members
                            .Where(m => !string.IsNullOrWhiteSpace(m.Callsign) || !string.IsNullOrWhiteSpace(m.Pilot))
                            .OrderBy(m => m.Jet <= 0 ? 99 : m.Jet)
                            .ThenBy(m => m.Callsign, StringComparer.OrdinalIgnoreCase)
                            .Take(8)
                            .ToList();
                    }
                    catch
                    {
                    }

                    return members;
                }

                private static void UpsertFlightMember(List<OpenKneeboardFlightMember> members, string callsign, string pilot, int jet)
                {
                    if (members == null)
                    {
                        return;
                    }

                    string safeCallsign = string.IsNullOrWhiteSpace(callsign) ? "" : callsign.Trim();
                    string safePilot = string.IsNullOrWhiteSpace(pilot) ? "" : pilot.Trim();
                    if (string.IsNullOrWhiteSpace(safeCallsign) && string.IsNullOrWhiteSpace(safePilot))
                    {
                        return;
                    }

                    OpenKneeboardFlightMember existing = null;
                    if (!string.IsNullOrWhiteSpace(safeCallsign))
                    {
                        existing = members.FirstOrDefault(m => string.Equals(m.Callsign, safeCallsign, StringComparison.OrdinalIgnoreCase));
                    }

                    if (existing == null)
                    {
                        existing = new OpenKneeboardFlightMember
                        {
                            Callsign = safeCallsign,
                            Pilot = safePilot,
                            Jet = jet > 0 ? jet : 0,
                        };
                        members.Add(existing);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(existing.Pilot) && !string.IsNullOrWhiteSpace(safePilot))
                    {
                        existing.Pilot = safePilot;
                    }

                    if (string.IsNullOrWhiteSpace(existing.Callsign) && !string.IsNullOrWhiteSpace(safeCallsign))
                    {
                        existing.Callsign = safeCallsign;
                    }

                    if ((existing.Jet <= 0) && jet > 0)
                    {
                        existing.Jet = jet;
                    }
                }

                private static int ParseJetPositionFromCallsign(string callsign)
                {
                    try
                    {
                        string text = (callsign ?? "").Trim();
                        if (text.Length == 0)
                        {
                            return 0;
                        }

                        Match pair = Regex.Match(text, @"(\d{2})\s*$");
                        if (pair.Success)
                        {
                            string value = pair.Groups[1].Value;
                            if (value.Length >= 2)
                            {
                                int jet;
                                if (int.TryParse(value.Substring(value.Length - 1, 1), out jet) && jet > 0 && jet <= 9)
                                {
                                    return jet;
                                }
                            }
                        }

                        Match single = Regex.Match(text, @"(\d)\s*$");
                        if (single.Success)
                        {
                            int jet;
                            if (int.TryParse(single.Groups[1].Value, out jet) && jet > 0 && jet <= 9)
                            {
                                return jet;
                            }
                        }
                    }
                    catch
                    {
                    }

                    return 0;
                }

                private static void FillMissingFlightJets(List<OpenKneeboardFlightMember> members)
                {
                    if (members == null || members.Count == 0)
                    {
                        return;
                    }

                    HashSet<int> used = new HashSet<int>();
                    foreach (OpenKneeboardFlightMember member in members)
                    {
                        if (member != null && member.Jet > 0)
                        {
                            used.Add(member.Jet);
                        }
                    }

                    int fallback = 1;
                    foreach (OpenKneeboardFlightMember member in members)
                    {
                        if (member == null || member.Jet > 0)
                        {
                            continue;
                        }

                        while (used.Contains(fallback))
                        {
                            fallback++;
                        }

                        member.Jet = fallback;
                        used.Add(fallback);
                    }
                }

                private static bool IsLikelyAwacsUnit(Servers.Server.DcsUnit unit)
                {
                    if (unit == null)
                    {
                        return false;
                    }

                    string callsign = (unit.callsign ?? "").Trim();
                    bool isKnownAwacsCallsign = callsign.IndexOf("Darkstar", StringComparison.OrdinalIgnoreCase) >= 0
                        || callsign.IndexOf("Focus", StringComparison.OrdinalIgnoreCase) >= 0
                        || callsign.IndexOf("Magic", StringComparison.OrdinalIgnoreCase) >= 0
                        || callsign.IndexOf("Overlord", StringComparison.OrdinalIgnoreCase) >= 0
                        || callsign.IndexOf("Wizard", StringComparison.OrdinalIgnoreCase) >= 0;

                    string typeSource = ((unit.typename ?? "") + " " + (unit.fullname ?? "")).ToUpperInvariant();
                    bool isAwacsType = typeSource.Contains("HAWKEYE")
                        || typeSource.Contains("SENTRY")
                        || typeSource.Contains("WEDGETAIL")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])E[-\s]?2[A-Z]?([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])E[-\s]?3[A-Z]?([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])E[-\s]?7[A-Z]?([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])A[-\s]?50([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])KJ[-\s]?2000([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])KJ[-\s]?500([^A-Z0-9]|$)");

                    return isKnownAwacsCallsign || isAwacsType;
                }

                private static string ResolveAtcIcaoType(string callsign, string atcName)
                {
                    try
                    {
                        if (State.currentstate == null || State.currentstate.atcicaotypes == null)
                        {
                            return string.Empty;
                        }

                        string keyCallsign = NormalizeAtcLookupKey(callsign);
                        string keyName = NormalizeAtcLookupKey(atcName);

                        string type;
                        if (!string.IsNullOrWhiteSpace(keyCallsign)
                            && State.currentstate.atcicaotypes.TryGetValue(keyCallsign, out type))
                        {
                            return NormalizeAtcIcaoTypeValue(type);
                        }

                        if (!string.IsNullOrWhiteSpace(keyName)
                            && State.currentstate.atcicaotypes.TryGetValue(keyName, out type))
                        {
                            return NormalizeAtcIcaoTypeValue(type);
                        }

                        foreach (KeyValuePair<string, string> pair in State.currentstate.atcicaotypes)
                        {
                            string normalizedKey = NormalizeAtcLookupKey(pair.Key);
                            if (!string.IsNullOrWhiteSpace(keyCallsign)
                                && string.Equals(normalizedKey, keyCallsign, StringComparison.Ordinal))
                            {
                                return NormalizeAtcIcaoTypeValue(pair.Value);
                            }

                            if (!string.IsNullOrWhiteSpace(keyName)
                                && string.Equals(normalizedKey, keyName, StringComparison.Ordinal))
                            {
                                return NormalizeAtcIcaoTypeValue(pair.Value);
                            }
                        }
                    }
                    catch
                    {
                    }

                    return string.Empty;
                }

                private static string NormalizeAtcLookupKey(string value)
                {
                    string s = (value ?? string.Empty).Trim().ToUpperInvariant();
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        return string.Empty;
                    }

                    s = Regex.Replace(s, @"[_\-/\.,\(\)]", " ");
                    s = Regex.Replace(s, @"\s+", " ").Trim();
                    return s;
                }

                private static string NormalizeAtcIcaoTypeValue(string value)
                {
                    string t = (value ?? string.Empty).Trim().ToUpperInvariant();
                    return (t == "MIL" || t == "CIV" || t == "JOINT") ? t : string.Empty;
                }

                private static List<OpenKneeboardMapMarker> BuildMapMarkerSnapshot()
                {
                    List<OpenKneeboardMapMarker> markers = new List<OpenKneeboardMapMarker>();

                    try
                    {
                        if (State.currentstate == null
                            || State.currentstate.riostate == null
                            || State.currentstate.riostate.markerdetails == null)
                        {
                            return markers;
                        }

                        foreach (Servers.Server.MapMarkerState marker in State.currentstate.riostate.markerdetails)
                        {
                            if (marker == null)
                            {
                                continue;
                            }

                            if (double.IsNaN(marker.x)
                                || double.IsInfinity(marker.x)
                                || double.IsNaN(marker.z)
                                || double.IsInfinity(marker.z))
                            {
                                continue;
                            }

                            markers.Add(new OpenKneeboardMapMarker
                            {
                                Id = marker.id,
                                Text = marker.text ?? string.Empty,
                                Author = marker.author ?? string.Empty,
                                Coalition = marker.coalition,
                                X = marker.x,
                                Y = marker.y,
                                Z = marker.z,
                            });

                            if (markers.Count >= 64)
                            {
                                break;
                            }
                        }
                    }
                    catch
                    {
                    }

                    return markers;
                }

                private static List<OpenKneeboardFriendlyAsset> BuildFriendlyAssetsSnapshot()
                {
                    List<OpenKneeboardFriendlyAsset> assets = new List<OpenKneeboardFriendlyAsset>();

                    try
                    {
                        if (State.currentstate == null || State.currentstate.availablerecipients == null)
                        {
                            return assets;
                        }

                        string playerCoalition = (State.currentstate.playercoalition ?? string.Empty).Trim();
                        string[] categories = new[] { "Player", "Flight", "Tanker", "AWACS", "JTAC", "ATC", "Allies" };
                        HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                        if (State.currentstate.bpos != null)
                        {
                            double playerNorth = State.currentstate.bpos.x;
                            double playerEast = State.currentstate.bpos.z;
                            if (!double.IsNaN(playerNorth) && !double.IsInfinity(playerNorth) && !double.IsNaN(playerEast) && !double.IsInfinity(playerEast))
                            {
                                string playerCallsign = string.IsNullOrWhiteSpace(State.currentstate.playercallsign) ? "PLAYER" : State.currentstate.playercallsign;
                                string playerName = string.IsNullOrWhiteSpace(State.currentstate.id) ? "PLAYER" : State.currentstate.id;
                                string playerKey = string.Format("{0}|{1}|{2}|{3}", playerCallsign, playerName, Math.Round(playerNorth), Math.Round(playerEast));
                                seen.Add(playerKey);
                                assets.Add(new OpenKneeboardFriendlyAsset
                                {
                                    Callsign = playerCallsign,
                                    Name = playerName,
                                    Category = "PLAYER",
                                    TypeName = string.IsNullOrWhiteSpace(State.currentstate.id) ? "" : State.currentstate.id,
                                    Frequency = "",
                                    Tacan = "",
                                    MpClientCallsign = string.IsNullOrWhiteSpace(State.currentstate.playerusername) ? "" : State.currentstate.playerusername,
                                    RawLine = string.Empty,
                                    X = playerNorth,
                                    Y = playerEast,
                                    AltFeet = State.currentstate.bpos.y * 3.28084,
                                });
                            }
                        }

                        foreach (string category in categories)
                        {
                            List<Servers.Server.DcsUnit> units;
                            if (!State.currentstate.availablerecipients.TryGetValue(category, out units) || units == null)
                            {
                                continue;
                            }

                            bool categoryIsAtc = category.Equals("ATC", StringComparison.OrdinalIgnoreCase);

                            foreach (Servers.Server.DcsUnit unit in units)
                            {
                                if (unit == null || unit.pos == null)
                                {
                                    continue;
                                }

                                if (!string.IsNullOrWhiteSpace(playerCoalition))
                                {
                                    string unitCoalition = (unit.coalition ?? string.Empty).Trim();
                                    if (!string.IsNullOrWhiteSpace(unitCoalition)
                                        && !categoryIsAtc
                                        && !playerCoalition.Equals(unitCoalition, StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }
                                }

                                double x = unit.pos.x;
                                double y = unit.pos.z;
                                if (double.IsNaN(x) || double.IsInfinity(x) || double.IsNaN(y) || double.IsInfinity(y))
                                {
                                    continue;
                                }

                                string callsign = string.IsNullOrWhiteSpace(unit.callsign) ? unit.fullname : unit.callsign;
                                string name = string.IsNullOrWhiteSpace(unit.fullname) ? unit.callsign : unit.fullname;
                                string dedupeKey = string.Format("{0}|{1}|{2}|{3}", callsign ?? "", name ?? "", Math.Round(x), Math.Round(y));
                                if (seen.Contains(dedupeKey))
                                {
                                    continue;
                                }
                                seen.Add(dedupeKey);

                                string normalizedCategory = category.ToUpperInvariant();
                                if (normalizedCategory.Equals("AWACS", StringComparison.OrdinalIgnoreCase)
                                    && !IsLikelyAwacsUnit(unit))
                                {
                                    normalizedCategory = "FLIGHT";
                                }

                                assets.Add(new OpenKneeboardFriendlyAsset
                                {
                                    Callsign = callsign ?? "",
                                    Name = name ?? "",
                                    Category = normalizedCategory,
                                    TypeName = string.IsNullOrWhiteSpace(unit.typename) ? "" : unit.typename,
                                    Frequency = string.IsNullOrWhiteSpace(unit.freq) ? "" : unit.freq,
                                    IcaoType = ResolveAtcIcaoType(callsign, name),
                                    AltFrequencies = unit.altfreq == null
                                        ? new List<string>()
                                        : unit.altfreq.Where(f => !string.IsNullOrWhiteSpace(f)).ToList(),
                                    Tacan = string.IsNullOrWhiteSpace(unit.tacan) ? "" : unit.tacan,
                                    MpClientCallsign = unit.ishuman && !string.IsNullOrWhiteSpace(unit.playerid) ? unit.playerid : "",
                                    RawLine = string.Empty,
                                    X = x,
                                    Y = y,
                                    AltFeet = unit.pos.y * 3.28084,
                                });

                                if (assets.Count >= 64)
                                {
                                    return assets;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    return assets;
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
                public List<string> DtcFiles { get; set; } = new List<string>();
                public string DtcSelectedFile { get; set; } = "";
                public string DtcJson { get; set; } = "";
                public string DtcSourceType { get; set; } = "";

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
                        DtcFiles = new List<string>(DtcFiles ?? new List<string>()),
                        DtcSelectedFile = DtcSelectedFile,
                        DtcJson = DtcJson,
                        DtcSourceType = DtcSourceType,
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
                public string PlayerUsername { get; set; } = "";
                public string PlayerCallsign { get; set; } = "";
                public string MissionTitle { get; set; } = "";
                public string MissionBriefing { get; set; } = "";
                public string MissionDetails { get; set; } = "";
                public double MissionTimeSeconds { get; set; }
                public double PlayerPosX { get; set; }
                public double PlayerPosY { get; set; }
                public double PlayerAltFeet { get; set; }
                public bool Multiplayer { get; set; }
                public bool DebugMode { get; set; }
                public object Payload { get; set; } = null;
                public List<Servers.Server.RadioDevice> Radios { get; set; } = new List<Servers.Server.RadioDevice>();
                public Dictionary<string, string> AtcMetars { get; set; } = new Dictionary<string, string>();
                public Dictionary<string, string> AtcIcaoTypes { get; set; } = new Dictionary<string, string>();
                public List<OpenKneeboardFlightMember> FlightMembers { get; set; } = new List<OpenKneeboardFlightMember>();
                public List<OpenKneeboardFriendlyAsset> FriendlyAssets { get; set; } = new List<OpenKneeboardFriendlyAsset>();
                public object Diagnostics { get; set; } = null;
                public List<OpenKneeboardMapMarker> MapMarkers { get; set; } = new List<OpenKneeboardMapMarker>();

                public OpenKneeboardServerSnapshot Clone()
                {
                    return new OpenKneeboardServerSnapshot
                    {
                        Theater = Theater,
                        DcsLocation = DcsLocation,
                        Aircraft = Aircraft,
                        PlayerUsername = PlayerUsername,
                        PlayerCallsign = PlayerCallsign,
                        MissionTitle = MissionTitle,
                        MissionBriefing = MissionBriefing,
                        MissionDetails = MissionDetails,
                        MissionTimeSeconds = MissionTimeSeconds,
                        PlayerPosX = PlayerPosX,
                        PlayerPosY = PlayerPosY,
                        PlayerAltFeet = PlayerAltFeet,
                        Multiplayer = Multiplayer,
                        DebugMode = DebugMode,
                        Payload = Payload,
                        Radios = Radios == null ? new List<Servers.Server.RadioDevice>() : new List<Servers.Server.RadioDevice>(Radios),
                        AtcMetars = new Dictionary<string, string>(AtcMetars ?? new Dictionary<string, string>()),
                        AtcIcaoTypes = new Dictionary<string, string>(AtcIcaoTypes ?? new Dictionary<string, string>()),
                        FlightMembers = FlightMembers == null
                            ? new List<OpenKneeboardFlightMember>()
                            : FlightMembers.ConvertAll(member => member == null ? null : member.Clone()).FindAll(member => member != null),
                        FriendlyAssets = FriendlyAssets == null
                            ? new List<OpenKneeboardFriendlyAsset>()
                            : FriendlyAssets.ConvertAll(functionAsset => functionAsset == null ? null : functionAsset.Clone()).FindAll(functionAsset => functionAsset != null),
                        MapMarkers = MapMarkers == null
                            ? new List<OpenKneeboardMapMarker>()
                            : MapMarkers.ConvertAll(marker => marker == null ? null : marker.Clone()).FindAll(marker => marker != null),
                        Diagnostics = Diagnostics,
                    };
                }
            }

            public class OpenKneeboardMapMarker
            {
                public int Id { get; set; }
                public string Text { get; set; } = "";
                public string Author { get; set; } = "";
                public int Coalition { get; set; }
                public double X { get; set; }
                public double Y { get; set; }
                public double Z { get; set; }

                public OpenKneeboardMapMarker Clone()
                {
                    return new OpenKneeboardMapMarker
                    {
                        Id = Id,
                        Text = Text,
                        Author = Author,
                        Coalition = Coalition,
                        X = X,
                        Y = Y,
                        Z = Z,
                    };
                }
            }

            public class OpenKneeboardFlightMember
            {
                public string Pilot { get; set; } = "";
                public string Callsign { get; set; } = "";
                public int Jet { get; set; }

                public OpenKneeboardFlightMember Clone()
                {
                    return new OpenKneeboardFlightMember
                    {
                        Pilot = Pilot,
                        Callsign = Callsign,
                        Jet = Jet,
                    };
                }
            }

            public class OpenKneeboardFriendlyAsset
            {
                public string Callsign { get; set; } = "";
                public string Name { get; set; } = "";
                public string Category { get; set; } = "";
                public string TypeName { get; set; } = "";
                public string Frequency { get; set; } = "";
                public string IcaoType { get; set; } = "";
                public List<string> AltFrequencies { get; set; } = new List<string>();
                public string Tacan { get; set; } = "";
                public string MpClientCallsign { get; set; } = "";
                public string RawLine { get; set; } = "";
                public double X { get; set; }
                public double Y { get; set; }
            public double AltFeet { get; set; }

                public OpenKneeboardFriendlyAsset Clone()
                {
                    return new OpenKneeboardFriendlyAsset
                    {
                        Callsign = Callsign,
                        Name = Name,
                        Category = Category,
                        TypeName = TypeName,
                        Frequency = Frequency,
                        IcaoType = IcaoType,
                        AltFrequencies = AltFrequencies == null ? new List<string>() : new List<string>(AltFrequencies),
                        Tacan = Tacan,
                        MpClientCallsign = MpClientCallsign,
                        RawLine = RawLine,
                        X = X,
                        Y = Y,
                        AltFeet = AltFeet,
                    };
                }
            }
        }
    }
}
