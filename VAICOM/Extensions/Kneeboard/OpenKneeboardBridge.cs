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
                private static readonly string[] DtcFileExtensions = new[] { ".dtc", ".json" };
                private const string RouteSelectionPrefix = "RTE::";
                private static readonly string IndexHtml = @"<!doctype html>
<html>
<head>
  <meta charset='utf-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1'>
  <title>VAICOM Kneeboard 1.0</title>
  <style>
    html, body { width: 100%; height: 100%; margin: 0; }
    body { font-family: Consolas, monospace; background: transparent; color: #151515; letter-spacing: 0.1px; font-size: 23px; --contentFontSize: 24px; }
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
    .dtcSelector { margin: 0 0 8px 0; }
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
    .fltPlanWpWrap { border: 1px solid #8b96a1; background: #ffffff; flex: 1 1 auto; min-height: 0; display: flex; flex-direction: column; }
    .fltPlanWpTitle { padding: 4px 6px; border-bottom: 1px solid #8b96a1; font-size: 16px; font-weight: 700; text-transform: uppercase; }
    .fltPlanWpTable { width: 100%; border-collapse: collapse; table-layout: fixed; }
    .fltPlanWpTableWrap { flex: 1 1 auto; min-height: 0; overflow: auto; }
    .fltPlanWpTable th, .fltPlanWpTable td { border: 1px solid #a3adb6; padding: 3px 4px; font-size: 15px; line-height: 1.15; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; text-align: center; vertical-align: middle; }
    .fltPlanWpTable th { background: #e4e8ec; font-weight: 700; }
    .fltPlanWpTable th.fltPlanEtaHeader { cursor: pointer; }
    .fltPlanWpTable th.fltPlanEtaHeader:hover { background: #d7e4ef; }
    .fltPlanAltTag { font-size: 11px; margin-left: 4px; color: #455869; }
    .fltPlanMiniBtn { font-family: inherit; font-size: 10px; line-height: 1; padding: 1px 3px; min-width: 16px; border: 1px solid #7c8692; background: #f4f7fa; color: #111; cursor: pointer; }
    .fltPlanMiniBtn:hover { background: #dce8f2; }
    .fltPlanSpdValue { display: inline-block; min-width: 30px; text-align: center; margin: 0 2px; }
    .fltPlanAdjustCell { width: 100%; display: flex; align-items: center; justify-content: space-between; gap: 2px; }
    .fltPlanEtaWrap { display: inline-flex; align-items: center; justify-content: center; gap: 4px; }
    .fltPlanEtaWrap input { margin: 0; }
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
    .fltPlanPageBtn { font-size: 12px; border: 1px solid #8b96a1; background: #eceff3; padding: 2px 9px; cursor: pointer; min-height: 22px; }
    .fltPlanPageBtn.active { background: #d3dae2; font-weight: 700; }
    .fltPlanPage2Grid { display: grid; grid-template-columns: 1fr 1fr; gap: 8px; margin-top: 8px; }
    .fltPlanPage2Section { border: 1px solid #8b96a1; background: #f7f9fb; }
    .fltPlanPage2Title { border-bottom: 1px solid #8b96a1; background: #e4e8ec; font-size: 13px; font-weight: 700; padding: 3px 6px; }
    .fltPlanPage2Body { padding: 4px 6px; }
    .fltPlanPage2Table { width: 100%; border-collapse: collapse; table-layout: fixed; }
    .fltPlanPage2Table th, .fltPlanPage2Table td { border: 1px solid #aeb7c0; font-size: 12px; padding: 2px 3px; text-align: left; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
    .fltPlanPage2Table th { background: #edf1f5; }
    .fltPlanMessage { white-space: pre-wrap; font-size: 18px; line-height: 1.2; }
    .fltPlanPlain { margin: 0; background: #ffffff; border: 1px solid #b7b7b7; padding: 10px; white-space: pre; word-break: normal; font-size: 16px; line-height: 1.2; min-height: 100%; box-sizing: border-box; overflow: auto; }
    pre { background: #ffffff; border: 1px solid #b7b7b7; padding: 10px; white-space: pre-wrap; word-break: break-word; font-size: 18px; color:#111; max-height: 190px; overflow: auto; }
    body.raw-mode .keywordsPanel { flex: 0 0 280px; }
    .hidden { display: none; }

    body.night-mode { color: #dbe4ee; }
    body.night-mode .sheet { background: #1b2129; border-color: #4b5663; box-shadow: inset 0 0 0 1px rgba(255, 255, 255, 0.05); }
    body.night-mode h3 { color: #e9f0f8; }
    body.night-mode .meta { color: #a9b8c7; }
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
    body.night-mode .controls button { background: #2b3541; color: #e2eaf2; border-color: #607183; }
    body.night-mode pre { background: #202a34; color: #dde7f2; border-color: #5d6f81; }
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

    <div id='dtcControls' class='controls fltPlanControls hidden'>
      <button id='dtcRefresh' type='button'>Refresh FLT PLN</button>
      <span id='dtcSelectedFileLabel' class='fltPlanSelected'>No FLT PLN selected</span>
    </div>

    <div id='dtcSelector' class='panel hidden'>
      <h4 id='dtcSelectorHeader' class='clickable'>FLT PLN Files ▼</h4>
      <div id='dtcFileList' class='content dtcFileList'></div>
    </div>

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
      <label><input id='nightMode' type='checkbox'> Night Mode</label>
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
    const nightModeStorageKey = 'vaicom.okb.nightMode';
    const contentFontSizeStorageKey = 'vaicom.okb.contentFontSize';
    const drawModeTimeoutMs = 30000;
    let tabKeywordsSplitByTab = {};
    let nightModeEnabled = false;
    let contentFontSizePx = 24;

    function clamp(v, min, max){
      return Math.max(min, Math.min(max, v));
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

    function hasTakeoffTimeBySelection(selected){
      return isFinite(getTakeoffTimeBySelection(selected));
    }

    function getFlightPlanTimingDisplay(selected){
      const takeoffSec = getTakeoffTimeBySelection(selected);
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
        step: formatSecondsToClock(takeoffSec - (35 * 60)),
        start: formatSecondsToClock(takeoffSec - (25 * 60)),
        taxi: formatSecondsToClock(takeoffSec - (15 * 60)),
        takeoff: formatSecondsToClock(takeoffSec),
        tot: isFinite(totSec) ? formatSecondsToClock(totSec) : '-',
      };
    }

    function getFlightPlanPlanState(selected){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return { speedAdjustments: {}, altAdjustments: {}, lockedStep: '', totSeconds: NaN, lockedStart: null };
      const existing = fltPlanPlanStateBySelection[key];
      if (existing && typeof existing === 'object') return existing;
      const created = { speedAdjustments: {}, altAdjustments: {}, lockedStep: '', totSeconds: NaN, lockedStart: null };
      fltPlanPlanStateBySelection[key] = created;
      return created;
    }

    function lockStartPositionForSelection(selected, data){
      const state = getFlightPlanPlanState(selected);
      const server = (data && data.Server) || {};
      const x = Number(server.PlayerPosX);
      const y = Number(server.PlayerPosY);
      const altFeet = Number(server.PlayerAltFeet);
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
      const compact = raw.replace(/[^A-Za-z0-9]/g, '').toUpperCase();
      if (!compact) return 'WP';
      return compact.substring(0, 3);
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

    function getCurrentFlightPlanClockSeconds(){
      const mission = getMissionClockSeconds(latestData);
      if (isFinite(mission)) return mission;
      return getSystemLocalClockSeconds();
    }

    function getFlightPlanEtaStartKey(selected){
      return String(selected || '');
    }

    function getDtcPageBySelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      const v = Number(fltPlanDtcPageBySelection[key]);
      return v === 2 ? 2 : 1;
    }

    function setDtcPageBySelection(selected, page){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const p = Number(page);
      fltPlanDtcPageBySelection[key] = (p === 2) ? 2 : 1;
    }

    function getDtcRouteBySelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      const route = String(fltPlanDtcRouteBySelection[key] || '').toUpperCase();
      return /^R[123]$/.test(route) ? route : 'R1';
    }

    function setDtcRouteBySelection(selected, route){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const r = String(route || '').toUpperCase();
      fltPlanDtcRouteBySelection[key] = /^R[123]$/.test(r) ? r : 'R1';
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
      const keys = Object.keys(metars).slice(0, 4);

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

      const etaMode = hasTakeoffTimeBySelection(selected);
      const baseSeconds = etaMode ? getTakeoffTimeBySelection(selected) : 0;
      let elapsed = 0;

      list[0].etaDisplay = formatSecondsToClock(baseSeconds);

      for (let i = 1; i < list.length; i++){
        const prev = list[i - 1];
        const curr = list[i];
        const legNm = computeLegDistanceNm(prev, curr);
        const legCas = Number(curr.spd);
        const legAlt = isFinite(Number(curr.altFeet)) ? Number(curr.altFeet) : 0;
        const legGs = isFinite(legCas) && legCas > 0 ? (legCas * (1.0 + (legAlt / 100000.0))) : NaN;
        const legSeconds = (isFinite(legNm) && legNm > 0 && isFinite(legGs) && legGs > 0)
          ? Math.max(1, Math.round((legNm * 3600.0) / legGs))
          : 0;

        elapsed += legSeconds;
        curr.etaDisplay = formatSecondsToClock(baseSeconds + elapsed);
      }

      return list;
    }

    function applyHeadingPlan(rows, theater){
      const list = Array.isArray(rows) ? rows : [];
      if (!list.length) return list;

      const magVar = Number(getApproxMagVariationDeg(theater));

      for (let i = 0; i < list.length; i++){
        const curr = list[i];
        let trueHdg = NaN;

        if (i === 0 && list.length > 1){
          trueHdg = computeTrueHeadingDeg(curr, list[i + 1]);
        } else if (i > 0){
          trueHdg = computeTrueHeadingDeg(list[i - 1], curr);
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

    function applyDistancePlan(rows){
      const list = Array.isArray(rows) ? rows : [];
      if (!list.length) return list;

      list[0].dist = '-';
      for (let i = 1; i < list.length; i++){
        const prev = list[i - 1];
        const curr = list[i];
        curr.dist = formatDistanceNm(computeLegDistanceNm(prev, curr));
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
      let addSeconds = 0;
      if (anchor === 'STEP') addSeconds = 35 * 60;
      else if (anchor === 'START') addSeconds = 25 * 60;
      else if (anchor === 'TAXI') addSeconds = 15 * 60;
      else addSeconds = 0;

      lockStartPositionForSelection(selected, latestData);
      fltPlanEtaStartBySelection[key] = getCurrentFlightPlanClockSeconds() + addSeconds;
    }

    function clearTakeoffTimeForSelection(selected){
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      delete fltPlanEtaStartBySelection[key];
    }

    function getFlightPlanStartRow(server, timing){
      const s = server || {};
      const key = getFlightPlanEtaStartKey((latestData && latestData.DtcSelectedFile) || '');
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

    function getDtcWaypoints(root){
      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];
      const navRoute = Array.isArray(wypt.NAV_ROUTE) ? wypt.NAV_ROUTE : [];
      const primaryRoute = (navRoute.length && navRoute[0] && typeof navRoute[0] === 'object') ? navRoute[0] : {};

      const routeById = {};
      Object.keys(primaryRoute).forEach(function(k){
        const point = primaryRoute[k];
        if (!point || typeof point !== 'object') return;
        routeById[String(k).toUpperCase()] = point;
      });

      if (navPts.length){
        return navPts.slice(0, 200).map(function(p, idx){
          const point = (p && typeof p === 'object') ? p : {};
          const id = String(point.id || ('STPT' + String(idx + 1))).trim();
          const routePoint = routeById[id.toUpperCase()] || {};

          const etaNum = isFinite(Number(routePoint.ETA)) ? Number(routePoint.ETA) : (isFinite(Number(point.ETA)) ? Number(point.ETA) : Number(point.TOS));
          const altNum = isFinite(Number(routePoint.alt)) ? Number(routePoint.alt) : (isFinite(Number(point.alt)) ? Number(point.alt) : (isFinite(Number(point.routeAltitude)) ? Number(point.routeAltitude) : Number(point.altitude)));
          const xNum = isFinite(Number(point.x)) ? Number(point.x) : Number(point.posX);
          const yNum = isFinite(Number(point.y)) ? Number(point.y) : Number(point.posY);
          const stepNum = isFinite(Number(point.wypt_num)) ? Math.round(Number(point.wypt_num)) : (isFinite(Number(point.number)) ? Math.round(Number(point.number)) : (idx + 1));
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
        });
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
      });
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
            return Object.keys(obj || {}).filter(function(k){ return /^(AUTO_?\d+|MAN_?\d+|BYP)$/i.test(String(k)); }).length;
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
      const maxRows = Math.max(leftRows.length, rightRows.length);

      const rows = [];
      for (let i = 0; i < maxRows; i++){
        const left = leftRows[i] || '';
        const right = rightRows[i] || '';
        rows.push('<tr><td style=""width:50%;"">' + left + '</td><td>' + right + '</td></tr>');
      }

      return '<div class=""fltPlanInfoBlock""><div class=""fltPlanInfoTitle"">CMDS</div><div class=""fltPlanInfoBody""><table class=""fltPlanInfoTable""><tbody>' + rows.join('') + '</tbody></table></div></div>';
    }

    function formatDtcCommPanelHtml(root){
      const commRoot = findFirstObjectByKeyPattern(root, /^COMM$/i, 0) || {};

      function formatCommFrequency(value){
        const n = Number(value);
        if (!isFinite(n)) return '-';
        let s = n.toFixed(3).replace(/0+$/,'');
        if (s.endsWith('.')) s = s.substring(0, s.length - 1);
        return s;
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
          const rawLabel = String(o.name || key.replace(/^Channel_/i, 'CH '));
          const label = rawLabel.replace(/\s+/g, '');
          rows.push({
            label: label,
            freq: formatCommFrequency(fq),
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

      const comm1 = commRoot.COMM1 || commRoot.Comm1 || commRoot.COMM_1 || {};
      const comm2 = commRoot.COMM2 || commRoot.Comm2 || commRoot.COMM_2 || {};
      const rows1 = getCommRows(comm1);
      const rows2 = getCommRows(comm2);
      const comm1Guard = !!comm1.Guard;
      const comm2Guard = !!comm2.Guard;

      const maxRows = Math.max(rows1.length, rows2.length);
      if (!maxRows) return '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">COMMS</div><div class=""fltPlanPage2Body"">No comm data.</div></div>';

      const bodyRows = [];
      for (let i = 0; i < maxRows; i++){
        const a = rows1[i];
        const b = rows2[i];
        bodyRows.push('<tr><td>' + (a ? (escapeHtml(a.label) + ' ' + escapeHtml(String(a.freq))) : '') + '</td><td>' + (b ? (escapeHtml(b.label) + ' ' + escapeHtml(String(b.freq))) : '') + '</td></tr>');
      }

      return '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">COMMS</div><div class=""fltPlanPage2Body""><table class=""fltPlanPage2Table""><thead><tr><th>COMM 1' + (comm1Guard ? ' (G)' : '') + '</th><th>COMM 2' + (comm2Guard ? ' (G)' : '') + '</th></tr></thead><tbody>' + bodyRows.join('') + '</tbody></table></div></div>';
    }

    function formatDtcRouteSummaryHtml(root, waypoints){
      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];
      const navRoute = Array.isArray(wypt.NAV_ROUTE) ? wypt.NAV_ROUTE : [];

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

      const body = [
        '<tr><td>R1</td><td>' + escapeHtml(routeList('R1')) + '</td></tr>',
        '<tr><td>R2</td><td>' + escapeHtml(routeList('R2')) + '</td></tr>',
        '<tr><td>R3</td><td>' + escapeHtml(routeList('R3')) + '</td></tr>'
      ];

      return '<div class=""fltPlanPage2Section""><div class=""fltPlanPage2Title"">ROUTES</div><div class=""fltPlanPage2Body""><table class=""fltPlanPage2Table""><thead><tr><th style=""width:56px;"">ROUTE</th><th>WAYPOINTS</th></tr></thead><tbody>' + body.join('') + '</tbody></table></div></div>';
    }

    function getDtcAvailableRoutes(root, waypoints){
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
      if (!/^R[123]$/.test(route)) return Array.isArray(waypoints) ? waypoints : [];

      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];
      const navRoute = Array.isArray(wypt.NAV_ROUTE) ? wypt.NAV_ROUTE : [];
      const list = Array.isArray(waypoints) ? waypoints : [];
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

    function formatDtcPage2Html(root, pageSwitcherHtml, waypoints){
      let html = '<div class=""fltPlanBoard"">';
      if (pageSwitcherHtml){
        html += '<div style=""margin:4px 0 6px 0;"">' + pageSwitcherHtml + '</div>';
      }
      html += '<div class=""fltPlanPage2Grid"">';
      html += formatDtcCommPanelHtml(root);
      html += formatDtcRouteSummaryHtml(root, waypoints);
      html += '</div></div>';
      return html;
    }

    function renderFlightPlanBoardHtml(selected, data, primaryRouteName, sourceLabel, sourceFileName, waypoints, cmdsBlockHtml, pageSwitcherHtml){
      const rows = Array.isArray(waypoints) ? waypoints.slice() : [];
      applyAltitudeAdjustments(rows, selected);
      applyEtaPlanToWaypoints(rows, selected);
      const timing = getFlightPlanTimingDisplay(selected);

      const server = (data && data.Server) || {};
      const callsign = safe(server.PlayerCallsign);
      const mission = safe(server.MissionTitle);
      const config = safe(server.Aircraft);
      const theatre = safe(server.Theater);
      const startRow = getFlightPlanStartRow(server, timing);
      const displayRows = startRow ? [startRow].concat(rows) : rows;
      applySpeedAdjustmentsToWaypoints(displayRows, selected);
      applyRouteTimeline(displayRows, selected);
      applyLockedTotPlan(displayRows, selected);
      applyDistancePlan(displayRows);
      applyHeadingPlan(displayRows, theatre);
      const etaHeading = hasTakeoffTimeBySelection(selected) ? 'ETA' : 'ETE';

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
        html += '<div style=""margin:4px 0 2px 0;"">' + pageSwitcherHtml + '</div>';
      }

      html += '<div class=""fltPlanTimeGrid"">';
      html += '<div class=""fltPlanTimeCell clickable"" data-tko-anchor=""STEP"" title=""Set STEP to current clock (Takeoff auto = STEP +35m)""><div class=""fltPlanTimeCellLabel"">STEP</div><div class=""fltPlanTimeCellValue"">' + escapeHtml(timing.step) + '</div></div>';
      html += '<div class=""fltPlanTimeCell clickable"" data-tko-anchor=""START"" title=""Set START to current clock (Takeoff auto = START +25m)""><div class=""fltPlanTimeCellLabel"">START</div><div class=""fltPlanTimeCellValue"">' + escapeHtml(timing.start) + '</div></div>';
      html += '<div class=""fltPlanTimeCell clickable"" data-tko-anchor=""TAXI"" title=""Set TAXI to current clock (Takeoff auto = TAXI +15m)""><div class=""fltPlanTimeCellLabel"">TAXI</div><div class=""fltPlanTimeCellValue"">' + escapeHtml(timing.taxi) + '</div></div>';
      html += '<div class=""fltPlanTimeCell clickable"" data-tko-anchor=""TAKEOFF"" title=""Set TAKEOFF to current clock""><div class=""fltPlanTimeCellLabel"">TAKE OFF</div><div class=""fltPlanTimeCellValue"">' + escapeHtml(timing.takeoff) + '</div></div>';
      html += '<div class=""fltPlanTimeCell""><div class=""fltPlanTimeCellLabel"">TOT</div><div class=""fltPlanTimeCellValue""><div class=""fltPlanAdjustCell""><button type=""button"" class=""fltPlanMiniBtn"" data-tot-adjust-sec=""-1"" title=""-1 second"">«</button><button type=""button"" class=""fltPlanMiniBtn"" data-tot-adjust=""-60"" title=""-1 minute"">◀</button><span class=""fltPlanSpdValue"">' + escapeHtml(timing.tot) + '</span><button type=""button"" class=""fltPlanMiniBtn"" data-tot-adjust=""60"" title=""+1 minute"">▶</button><button type=""button"" class=""fltPlanMiniBtn"" data-tot-adjust-sec=""1"" title=""+1 second"">»</button></div></div></div>';
      html += '</div>';

      html += '<div class=""fltPlanWpWrap"">';
      html += '<div class=""fltPlanWpTitle"">Route: ' + escapeHtml(primaryRouteName) + '</div>';
      html += '<div class=""fltPlanWpTableWrap"">';
      html += '<table class=""fltPlanWpTable"">';
      html += '<thead><tr><th style=""width:48px;"">STP</th><th style=""width:56px;"">TYPE</th><th style=""width:130px;"">NAME</th><th style=""width:78px;"">ALT</th><th style=""width:56px;"">HDG</th><th style=""width:86px;"">SPD KCAS</th><th style=""width:64px;"">DIST</th><th class=""fltPlanEtaHeader"" style=""width:90px;"" data-eta-header=""1"" title=""Click to set ETA start from current time"">' + etaHeading + '</th><th style=""width:150px;"">X / Y</th></tr></thead>';
      html += '<tbody>';

      if (!displayRows.length){
        html += '<tr><td colspan=""9"">No waypoints found.</td></tr>';
      } else {
        displayRows.forEach(function(wp){
          const stepKey = stepToKey(wp.step);
          const lockChecked = !wp.isStart && stepKey && (stepKey === stepToKey(getFlightPlanPlanState(selected).lockedStep)) ? ' checked' : '';
          html += '<tr>';
          html += '<td class=""fltPlanCellNum"">' + escapeHtml(wp.step) + '</td>';
          html += '<td>' + escapeHtml(wp.type) + '</td>';
          html += '<td>' + escapeHtml(wp.name || '-') + '</td>';
          if (wp.isStart){
            html += '<td class=""fltPlanCellNum"">' + formatAltCellHtml(wp) + '</td>';
          } else {
            html += '<td class=""fltPlanCellNum""><div class=""fltPlanAdjustCell""><button type=""button"" class=""fltPlanMiniBtn"" data-alt-step=""' + escapeHtml(stepKey) + '"" data-alt-delta=""-500"">◀</button><span class=""fltPlanSpdValue"">' + formatAltCellHtml(wp) + '</span><button type=""button"" class=""fltPlanMiniBtn"" data-alt-step=""' + escapeHtml(stepKey) + '"" data-alt-delta=""500"">▶</button></div></td>';
          }
          if (wp.isStart){
            html += '<td class=""fltPlanCellNum"">' + escapeHtml(wp.hdg || '-') + '</td>';
            html += '<td class=""fltPlanCellNum"">' + escapeHtml(wp.spd || '-') + '</td>';
          } else {
            html += '<td class=""fltPlanCellNum"">' + escapeHtml(wp.hdg || '-') + '</td>';
            html += '<td class=""fltPlanCellNum""><div class=""fltPlanAdjustCell""><button type=""button"" class=""fltPlanMiniBtn"" data-spd-step=""' + escapeHtml(stepKey) + '"" data-spd-delta=""-20"">◀</button><span class=""fltPlanSpdValue"">' + escapeHtml(wp.spd || '-') + '</span><button type=""button"" class=""fltPlanMiniBtn"" data-spd-step=""' + escapeHtml(stepKey) + '"" data-spd-delta=""20"">▶</button></div></td>';
          }
          html += '<td class=""fltPlanCellNum"">' + escapeHtml(wp.dist || '-') + '</td>';
          if (wp.isStart){
            html += '<td class=""fltPlanCellNum"">' + escapeHtml(wp.etaDisplay || wp.eta) + '</td>';
          } else {
            html += '<td class=""fltPlanCellNum""><span class=""fltPlanEtaWrap""><input type=""checkbox"" data-tot-lock-step=""' + escapeHtml(stepKey) + '""' + lockChecked + '><span>' + escapeHtml(wp.etaDisplay || wp.eta) + '</span></span></td>';
          }
          html += '<td class=""fltPlanCellNum"">' + escapeHtml((wp.x || '-') + ' / ' + (wp.y || '-')) + '</td>';
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
      return renderFlightPlanBoardHtml(selected, data, primaryRouteName, 'ROUTE TOOL', getPathFileName(getFltPlnPath(selected)), waypoints, '', '');
    }

    function formatDtcTableHtml(root, selected, data){
      const allWaypoints = getDtcWaypoints(root);
      const availableRoutes = getDtcAvailableRoutes(root, allWaypoints);
      let routeKey = getDtcRouteBySelection(selected);
      if (availableRoutes.indexOf(routeKey) < 0){
        routeKey = availableRoutes[0] || 'R1';
        setDtcRouteBySelection(selected, routeKey);
      }
      const waypoints = filterDtcWaypointsByRoute(root, allWaypoints, routeKey);
      const cmdsBlockHtml = formatDtcCmdsBlockHtml(root);
      const page = getDtcPageBySelection(selected);
      const routeButtons = availableRoutes.map(function(r){
        return '<button type=""button"" class=""fltPlanPageBtn' + (routeKey === r ? ' active' : '') + '"" data-dtc-route=""' + r + '"">' + r + '</button>';
      }).join('');
      const pageSwitcherHtml = '<span class=""fltPlanPageSwitcher""><button type=""button"" class=""fltPlanPageBtn' + (page === 1 ? ' active' : '') + '"" data-dtc-page=""1"">Page 1</button><button type=""button"" class=""fltPlanPageBtn' + (page === 2 ? ' active' : '') + '"" data-dtc-page=""2"">Page 2</button></span><span class=""fltPlanPageSwitcher"">' + routeButtons + '</span>';
      if (page === 2){
        return formatDtcPage2Html(root, pageSwitcherHtml, waypoints);
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
      const files = Array.isArray(data.DtcFiles) ? data.DtcFiles : [];
      const selected = String(data.DtcSelectedFile || '');
      const jsonText = String(data.DtcJson || '').trim();
      const sourceType = String(data.DtcSourceType || '').toUpperCase();

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
      updateDtcControls(data);
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
      if (selectedTab === 'DTC'){
        tabBody.className = 'content mainContent fltPlanContent';
        tabBody.innerHTML = formatDtcTabContentHtml(data);
      } else {
        tabBody.className = 'content mainContent';
        tabBody.textContent = formatTabContent(data, selectedTab);
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
        ? formatAiCrewPhaseLabel(data.AiCrewPhase)
        : '';
      const keywordPanelEl = document.getElementById('keywordPanel');
      if (selectedTab === 'DTC'){
        if (keywordPanelEl) keywordPanelEl.style.display = 'none';
      } else {
        if (keywordPanelEl) keywordPanelEl.style.display = 'flex';
        document.getElementById('keywordTitle').textContent = 'Keywords: ' + tabLabel(selectedTab) + aiCrewPhaseSuffix;
        document.getElementById('keywordBody').innerHTML = formatKeywordReferenceHtml(data, selectedTab);
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

    function updateDtcControls(data){
      const wrap = document.getElementById('dtcControls');
      const selector = document.getElementById('dtcSelector');
      const listEl = document.getElementById('dtcFileList');
      const selectedLabel = document.getElementById('dtcSelectedFileLabel');
      if (!wrap || !selector || !listEl || !selectedLabel) return;

      const visible = selectedTab === 'DTC';
      wrap.className = visible ? 'controls fltPlanControls' : 'controls fltPlanControls hidden';
      selector.className = visible ? 'panel dtcSelector' : 'panel dtcSelector hidden';
      if (!visible) return;

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
      try{
        await fetch('dtc/list', { method: 'POST', cache: 'no-store' });
      }catch(_){
      }
      await tick();
    }

    async function selectDtcFile(filePath){
      try{
        await fetch('dtc/select?file=' + encodeURIComponent(filePath || ''), { method: 'POST', cache: 'no-store' });
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
            const selected = String((latestData && latestData.DtcSelectedFile) || '');
            const step = String(node.getAttribute('data-spd-step') || '');
            const delta = Number(node.getAttribute('data-spd-delta') || 0);
            if (selected && step && isFinite(delta) && delta !== 0){
              changeWaypointSpeedAdjustment(selected, step, delta);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-alt-step')){
            const selected = String((latestData && latestData.DtcSelectedFile) || '');
            const step = String(node.getAttribute('data-alt-step') || '');
            const delta = Number(node.getAttribute('data-alt-delta') || 0);
            if (selected && step && isFinite(delta) && delta !== 0){
              changeWaypointAltitude(selected, step, delta);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tot-adjust')){
            const selected = String((latestData && latestData.DtcSelectedFile) || '');
            const seconds = Number(node.getAttribute('data-tot-adjust') || 0);
            if (selected && isFinite(seconds) && seconds !== 0){
              setTotByMinutesDelta(selected, seconds / 60.0);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tot-adjust-sec')){
            const selected = String((latestData && latestData.DtcSelectedFile) || '');
            const seconds = Number(node.getAttribute('data-tot-adjust-sec') || 0);
            if (selected && isFinite(seconds) && seconds !== 0){
              setTotBySecondsDelta(selected, seconds);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-dtc-page')){
            const selected = String((latestData && latestData.DtcSelectedFile) || '');
            const page = Number(node.getAttribute('data-dtc-page') || 1);
            if (selected){
              setDtcPageBySelection(selected, page);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-dtc-route')){
            const selected = String((latestData && latestData.DtcSelectedFile) || '');
            const route = String(node.getAttribute('data-dtc-route') || 'R1');
            if (selected){
              setDtcRouteBySelection(selected, route);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tot-lock-step')){
            const selected = String((latestData && latestData.DtcSelectedFile) || '');
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
            const selected = String((latestData && latestData.DtcSelectedFile) || '');
            const anchor = String(node.getAttribute('data-tko-anchor') || '');
            if (selected && anchor){
              setTakeoffTimeByAnchorFromNow(selected, anchor);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-eta-header')){
            const selected = String((latestData && latestData.DtcSelectedFile) || '');
            if (selected){
              setEtaStartNowForSelection(selected);
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

    document.getElementById('sessionHeader').addEventListener('click', function(){
      applySessionCollapsedState(!sessionCollapsed);
      persistSessionCollapsedState();
    });

    applySessionCollapsedState(readInitialSessionCollapsed());
    nightModeEnabled = readNightModePreference();
    applyNightModeUi();
    contentFontSizePx = readContentFontSizePreference();
    applyContentFontSizeUi();
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
                        string prefix = GetPrefix();
                        listener = new HttpListener();
                        listener.Prefixes.Add(prefix);
                        listener.Start();

                        isRunning = true;
                        listenerThread = new Thread(ListenLoop) { IsBackground = true, Name = "OpenKneeboardWebHost" };
                        listenerThread.Start();

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

                    if (path == "/okb/dtc/list")
                    {
                        RefreshDtcFilesSnapshot();
                        WriteJson(context.Response, BuildSnapshotJson());
                        return;
                    }

                    if (path == "/okb/dtc/select")
                    {
                        string file = WebUtility.UrlDecode(context.Request.QueryString["file"] ?? "");
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
                        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        if (string.IsNullOrWhiteSpace(userProfile))
                        {
                            return roots;
                        }

                        string savedGames = Path.Combine(userProfile, "Saved Games");
                        if (!Directory.Exists(savedGames))
                        {
                            return roots;
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
                        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        if (string.IsNullOrWhiteSpace(userProfile))
                        {
                            return files;
                        }

                        string savedGames = Path.Combine(userProfile, "Saved Games");
                        if (!Directory.Exists(savedGames))
                        {
                            return files;
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
                        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        if (string.IsNullOrWhiteSpace(userProfile))
                        {
                            return roots;
                        }

                        string savedGames = Path.Combine(userProfile, "Saved Games");
                        if (!Directory.Exists(savedGames))
                        {
                            return roots;
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
                    catch
                    {
                    }

                    return roots
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();
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
                        MissionTimeSeconds = MissionTimeSeconds,
                        PlayerPosX = PlayerPosX,
                        PlayerPosY = PlayerPosY,
                        PlayerAltFeet = PlayerAltFeet,
                        Multiplayer = Multiplayer,
                        DebugMode = DebugMode,
                        AtcMetars = new Dictionary<string, string>(AtcMetars ?? new Dictionary<string, string>()),
                    };
                }
            }
        }
    }
}
