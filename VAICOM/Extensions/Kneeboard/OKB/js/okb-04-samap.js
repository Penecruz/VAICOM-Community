    function formatDtcCmdsBlockHtml(root, selected) {
      function collectCmdsSettings(value, depth, found) {
        if (depth > 10 || value === null || value === undefined) return;
        if (Array.isArray(value)) {
          value.forEach(function (v) { collectCmdsSettings(v, depth + 1, found); });
          return;
        }
        if (typeof value !== 'object') return;

        Object.keys(value).forEach(function (k) {
          const v = value[k];
          if (/^CMDSProgramSettings$/i.test(String(k)) && v && typeof v === 'object') {
            found.push(v);
          }
          if (v && typeof v === 'object') collectCmdsSettings(v, depth + 1, found);
        });
      }

      const foundCmds = [];
      collectCmdsSettings(root, 0, foundCmds);
      const cmds = foundCmds
        .sort(function (a, b) {
          function score(obj) {
            return Object.keys(obj || {}).filter(function (k) { return /^(AUTO_?\d+|MAN_?\d+|BYP|PROG_?\d+)$/i.test(String(k)); }).length;
          }
          return score(b) - score(a);
        })[0];
      if (!cmds || typeof cmds !== 'object') return '';

      const preferredOrder = ['AUTO_1', 'AUTO1', 'AUTO_2', 'AUTO2', 'AUTO_3', 'AUTO3', 'BYP', 'MAN_1', 'MAN1', 'MAN_2', 'MAN2', 'MAN_3', 'MAN3', 'MAN_4', 'MAN4', 'MAN_5', 'MAN5', 'MAN_6', 'MAN6'];
      const keys = [];
      preferredOrder.forEach(function (k) {
        if (cmds[k] && typeof cmds[k] === 'object') keys.push(k);
      });

      Object.keys(cmds).forEach(function (k) {
        if (keys.indexOf(k) >= 0) return;
        if (!cmds[k] || typeof cmds[k] !== 'object') return;
        keys.push(k);
      });

      if (!keys.length) return '';

      function modeShortLabel(k) {
        const key = String(k || '').toUpperCase();
        const auto = key.match(/^AUTO_?(\d+)$/);
        if (auto) return 'A' + auto[1];
        const man = key.match(/^MAN_?(\d+)$/);
        if (man) return 'M' + man[1];
        if (key === 'BYP') return 'BYP';
        return key;
      }

      function num(v, fallback) {
        const n = Number(v);
        return isFinite(n) ? n : fallback;
      }

      const rowByKey = {};
      keys.forEach(function (k) {
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
        .map(function (k) { return rowByKey[String(k).toUpperCase()]; })
        .filter(function (x) { return !!x; });
      const rightRows = rightModes
        .map(function (k) { return rowByKey[String(k).toUpperCase()]; })
        .filter(function (x) { return !!x; });
      if (!leftRows.length && !rightRows.length) {
        const fallbackRows = keys
          .map(function (k) { return rowByKey[String(k).toUpperCase()]; })
          .filter(function (x) { return !!x; });
        const split = Math.ceil(fallbackRows.length / 2);
        for (let i = 0; i < split; i++) {
          leftRows.push(fallbackRows[i] || '');
          rightRows.push(fallbackRows[i + split] || '');
        }
      }

      const maxRows = Math.max(leftRows.length, rightRows.length);

      const rows = [];
      for (let i = 0; i < maxRows; i++) {
        const left = leftRows[i] || '';
        const right = rightRows[i] || '';
        rows.push('<tr><td style="width:50%;">' + left + '</td><td>' + right + '</td></tr>');
      }

      const titleAttrs = getBottomPanelTitleAttrs(selected, 'CMDS');
      return '<div class="fltPlanInfoBlock"><div class="fltPlanInfoTitle"' + titleAttrs + '>CMDS</div><div class="fltPlanInfoBody"><table class="fltPlanInfoTable"><tbody>' + rows.join('') + '</tbody></table></div></div>';
    }

    function isAh64Text(value) {
      const t = String(value || '').toUpperCase();
      return t.indexOf('AH-64') >= 0 || t.indexOf('AH64') >= 0 || t.indexOf('APACHE') >= 0;
    }

    function isRuntimeAh64Module(data) {
      const server = (data && data.Server) || {};
      const moduleConnected = !!server.ModuleConnected;
      const aircraft = String(server.Aircraft || '').trim();
      if (!moduleConnected || !aircraft || aircraft === '----') return false;
      return isAh64Text(aircraft);
    }

    function isAh64DtcRoot(root) {
      return isAh64Text(root && root.type);
    }

    function isFa18Text(value) {
      const t = String(value || '').toUpperCase();
      return t.indexOf('FA-18') >= 0 || t.indexOf('F/A-18') >= 0 || t.indexOf('HORNET') >= 0;
    }

    function isRuntimeFa18Module(data) {
      const server = (data && data.Server) || {};
      const moduleConnected = !!server.ModuleConnected;
      const aircraft = String(server.Aircraft || '').trim();
      if (!moduleConnected || !aircraft || aircraft === '----') return false;
      return isFa18Text(aircraft);
    }

    function isFa18DtcRoot(root) {
      return isFa18Text(root && root.type);
    }

    function isF16Text(value) {
      const t = String(value || '').toUpperCase();
      return t.indexOf('F-16') >= 0 || t.indexOf('F16') >= 0 || t.indexOf('VIPER') >= 0;
    }

    function isRuntimeF16Module(data) {
      const server = (data && data.Server) || {};
      const moduleConnected = !!server.ModuleConnected;
      const aircraft = String(server.Aircraft || '').trim();
      if (!moduleConnected || !aircraft || aircraft === '----') return false;
      return isF16Text(aircraft);
    }

    function isF16DtcRoot(root) {
      return isF16Text(root && root.type);
    }

    function formatCommFrequencyMhz3(value) {
      const n = Number(value);
      if (!isFinite(n) || n <= 0) return '-';
      let mhz = n;
      if (n >= 10000000) mhz = n / 1000000.0;
      else if (n >= 100000) mhz = n / 1000.0;
      return mhz.toFixed(3);
    }

    function getAh64RuntimeRadioByName(data, radioName) {
      const server = (data && data.Server) || {};
      const radios = Array.isArray(server.Radios) ? server.Radios : [];
      const key = String(radioName || '').toUpperCase();
      for (let i = 0; i < radios.length; i++) {
        const r = radios[i] || {};
        if (!!r.intercom) continue;
        const name = String(r.displayName || r.name || '').toUpperCase();
        if (name.indexOf(key) >= 0) return r;
      }
      return null;
    }

    function mapAh64Modulation(v) {
      const n = Number(v);
      if (!isFinite(n)) return '-';
      if (n === 0) return 'AM';
      if (n === 1) return 'FM';
      if (n === 2) return 'AME';
      return String(Math.round(n));
    }

    function mapAh64Encryption(v) {
      const n = Number(v);
      if (!isFinite(n) || n === 0) return 'NONE';
      return String(Math.round(n));
    }

    function getAh64DtcCommTabs(root) {
      let presets = (root && root.Presets && typeof root.Presets === 'object') ? root.Presets : null;
      if (!presets) {
        presets = findNestedAh64Presets(root, 0);
      }
      presets = (presets && typeof presets === 'object') ? presets : {};
      const rows = Array.isArray(presets.COMM) ? presets.COMM : [];
      return rows.slice(0, 10);
    }

    function findNestedAh64Presets(value, depth) {
      if (depth > 10 || value === null || value === undefined) return null;
      if (Array.isArray(value)) {
        for (let i = 0; i < value.length; i++) {
          const found = findNestedAh64Presets(value[i], depth + 1);
          if (found) return found;
        }
        return null;
      }
      if (typeof value !== 'object') return null;

      const presets = (value && value.Presets && typeof value.Presets === 'object') ? value.Presets : null;
      if (presets && Array.isArray(presets.COMM)) {
        return presets;
      }

      const keys = Object.keys(value);
      for (let i = 0; i < keys.length; i++) {
        const found = findNestedAh64Presets(value[keys[i]], depth + 1);
        if (found) return found;
      }
      return null;
    }

    function normalizeAh64CommPreset(root, data, presetObj, presetIndex) {
      const idx = isFinite(Number(presetIndex)) ? Math.max(0, Math.min(9, Math.round(Number(presetIndex)))) : 0;
      const preset = (presetObj && typeof presetObj === 'object') ? presetObj : {};
      const comms = (preset.COMMS && typeof preset.COMMS === 'object') ? preset.COMMS : {};
      const initialMode = (preset.InitialMode && typeof preset.InitialMode === 'object') ? preset.InitialMode : {};
      const doNotUploadTabData = !!preset.DoNotUploadTabData;

      const runtimeVhf = getAh64RuntimeRadioByName(data, 'VHF') || {};
      const runtimeUhf = getAh64RuntimeRadioByName(data, 'UHF') || {};
      const runtimeFm1 = getAh64RuntimeRadioByName(data, 'FM1') || {};
      const runtimeFm2 = getAh64RuntimeRadioByName(data, 'FM2') || {};
      const runtimeHfRx = getAh64RuntimeRadioByName(data, 'HF RX') || getAh64RuntimeRadioByName(data, 'HF') || {};
      const runtimeHfTx = getAh64RuntimeRadioByName(data, 'HF TX') || getAh64RuntimeRadioByName(data, 'HF') || {};

      function normalizeRadioEntry(entry, runtimeFallback) {
        const source = (entry && typeof entry === 'object') ? entry : {};
        const runtime = (runtimeFallback && typeof runtimeFallback === 'object') ? runtimeFallback : {};
        const useRuntime = doNotUploadTabData || !entry || typeof entry !== 'object';
        const rawFrequency = useRuntime
          ? Number(runtime.frequency)
          : Number(source.Frequency !== undefined ? source.Frequency : source.frequency);
        const rawModulation = useRuntime
          ? Number(runtime.modulation)
          : Number(source.Modulation !== undefined ? source.Modulation : source.modulation);
        const rawEncryption = Number(source.Encryption);
        return {
          frequency: formatCommFrequencyMhz3(rawFrequency),
          modulation: mapAh64Modulation(rawModulation),
          encryption: mapAh64Encryption(rawEncryption),
        };
      }

      const vhf = normalizeRadioEntry(comms.VHF, runtimeVhf);
      const uhf = normalizeRadioEntry(comms.UHF, runtimeUhf);
      const fm1 = normalizeRadioEntry(comms.FM1, runtimeFm1);
      const fm2 = normalizeRadioEntry(comms.FM2, runtimeFm2);
      const hfRx = normalizeRadioEntry(comms.HF_Rx, runtimeHfRx);
      const hfTx = normalizeRadioEntry(comms.HF_Tx, runtimeHfTx);

      const hfPreRaw = Number(comms.HF_Pre);
      const hfAleRaw = Number(comms.HF_ALE);

      const primaryFrequencyRaw = Number(initialMode.PrimaryFrequency);
      return {
        presetNumber: idx + 1,
        doNotUploadTabData: doNotUploadTabData,
        unitId: String(initialMode.UnitID || '').trim() || ('PRESET ' + String(idx + 1)),
        callSign: String(initialMode.CallSign || '').trim() || ('PRE ' + String(idx + 1)),
        primaryFrequency: isFinite(primaryFrequencyRaw) ? String(Math.round(primaryFrequencyRaw)) : '-',
        dlNet: initialMode.DLNet === true,
        vhf: vhf,
        uhf: uhf,
        fm1: fm1,
        fm2: fm2,
        hfRx: hfRx,
        hfTx: hfTx,
        hfPre: isFinite(hfPreRaw) ? String(Math.round(hfPreRaw)) : '-',
        hfAle: isFinite(hfAleRaw) ? String(Math.round(hfAleRaw)) : '-',
      };
    }

    function buildAh64CommPresetState(root, data) {
      const tabs = getAh64DtcCommTabs(root);
      const presets = [];
      for (let i = 0; i < 10; i++) {
        presets.push(normalizeAh64CommPreset(root, data, tabs[i], i));
      }
      return { presets: presets };
    }

    function formatAh64CommPanelHtml(root, data, selected) {
      const state = buildAh64CommPresetState(root, data);
      const presets = Array.isArray(state && state.presets) ? state.presets : [];
      const presetNumber = getAh64CommPresetBySelection(selected);
      const active = presets[presetNumber - 1] || normalizeAh64CommPreset(root, data, null, presetNumber - 1);

      const tabsHtml = Array.from({ length: 10 }, function (_, i) {
        const n = i + 1;
        const activeClass = (n === presetNumber) ? ' active' : '';
        return '<button type="button" class="fltPlanPageBtn' + activeClass + '" data-ah64-comm-preset="' + String(n) + '">' + String(n) + '</button>';
      }).join('');

      function row(label, radio, showEncryption) {
        const r = radio || {};
        const fq = String(r.frequency || '-');
        const fqText = fq !== '-' ? fq : '-';
        return '<tr>'
          + '<td style="width:44px;">' + escapeHtml(label) + '</td>'
          + '<td style="width:186px; text-align:right; white-space:nowrap; overflow:visible; text-overflow:clip;">' + escapeHtml(fqText) + '</td>'
          + '<td style="width:34px; text-align:center;">' + escapeHtml(String(r.modulation || '-')) + '</td>'
          + (showEncryption ? ('<td style="width:44px; text-align:center;">' + escapeHtml(String(r.encryption || '-')) + '</td>') : '<td style="width:44px;"></td>')
          + '</tr>';
      }

      const leftRows = [
        '<tr><td style="width:110px;">Unit ID</td><td>' + escapeHtml(String(active.unitId || '-')) + '</td></tr>',
        '<tr><td>Call Sign</td><td>' + escapeHtml(String(active.callSign || '-')) + '</td></tr>',
        '<tr><td>Primary Recv</td><td>' + escapeHtml(String(active.primaryFrequency || '-')) + '</td></tr>',
        '<tr><td>DL Net</td><td>' + (active.dlNet ? '✓' : '-') + '</td></tr>'
      ].join('');

      const freqRows = [
        row('VHF', active.vhf, true),
        row('UHF', active.uhf, true),
        row('FM1', active.fm1, true),
        row('FM2', active.fm2, true),
        row('HF Rx', active.hfRx, true),
        row('HF Tx', active.hfTx, true),
        '<tr><td>HF Pre</td><td style="text-align:right;">' + escapeHtml(String(active.hfPre || '-')) + '</td><td></td><td></td></tr>',
        '<tr><td>HF ALE</td><td style="text-align:right;">' + escapeHtml(String(active.hfAle || '-')) + '</td><td></td><td></td></tr>'
      ].join('');

      return '<div class="fltPlanPage2Section">'
        + '<div class="fltPlanPage2Title">COMMS</div>'
        + '<div class="fltPlanPage2Body">'
        + '<div style="margin:0 0 8px 0;" class="fltPlanPageSwitcher">' + tabsHtml + '</div>'
        + '<div class="fltPlanInfoBlock" style="margin:0;">'
        + '<div class="fltPlanInfoTitle">PRESET ' + escapeHtml(String(presetNumber)) + '</div>'
        + '<div class="fltPlanInfoBody">'
        + '<div class="fltPlanPage2Grid" style="grid-template-columns: 0.9fr 1.1fr;">'
        + '<div><table class="fltPlanPage2Table"><tbody>' + leftRows + '</tbody></table></div>'
        + '<div><table class="fltPlanPage2Table"><thead><tr><th colspan="4">Frequencies</th></tr></thead><tbody>' + freqRows + '</tbody></table></div>'
        + '</div>'
        + '</div>'
        + '</div>'
        + '</div>'
        + '</div>';
    }

    function readDtcCommPanelModel(root) {
      const commRoot = findFirstObjectByKeyPattern(root, /^COMM$/i, 0) || {};

      function formatCommFrequency(value) {
        const n = Number(value);
        if (!isFinite(n)) return '-';
        return n.toFixed(3);
      }

      function getCommRows(commObj) {
        if (!commObj || typeof commObj !== 'object') return [];
        const rows = [];
        Object.keys(commObj).forEach(function (k) {
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

        rows.sort(function (a, b) {
          if (a.sortGroup !== b.sortGroup) return a.sortGroup - b.sortGroup;
          if (a.sortValue !== b.sortValue) return a.sortValue - b.sortValue;
          return String(a.label).localeCompare(String(b.label));
        });

        return rows;
      }

      function looksLikeDefaultMirrorRows(rows) {
        if (!Array.isArray(rows) || rows.length !== 20) return false;

        const seen = {};
        for (let i = 0; i < rows.length; i++) {
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

        for (let ch = 1; ch <= 20; ch++) {
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

      return {
        rows1: rows1,
        rows2: rows2,
        comm1Guard: comm1Guard,
        comm2Guard: comm2Guard,
        mirror1: mirror1,
        mirror2: mirror2,
        looksLikeDefaultMirrors: looksLikeDefaultMirrors,
      };
    }

    function getRuntimeCommColumns(data) {
      const server = (data && data.Server) || {};
      const radios = Array.isArray(server.Radios) ? server.Radios : [];
      const diagnostics = (server && server.Diagnostics && typeof server.Diagnostics === 'object') ? server.Diagnostics : {};

      function fmtFreq(value) {
        const n = Number(value);
        if (isFinite(n) && n > 0) {
          let mhz = n;
          if (n >= 10000000) {
            mhz = n / 1000000.0;
          } else if (n >= 100000) {
            mhz = n / 1000.0;
          }

          return mhz.toFixed(3);
        }
        const s = String(value || '').trim();
        return s || '-';
      }

      function parseMissionRadioChannelRow(line) {
        const text = String(line || '').trim();
        if (!text) return null;
        const map = {};
        text.split('|').forEach(function (p) {
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
        .filter(function (x) { return !!x; });

      const dedupedMissionChannels = [];
      const seenMissionChannels = {};
      missionChannels.forEach(function (ch) {
        const key = String(ch.radio) + '|' + String(ch.channel) + '|' + String(ch.freq);
        if (seenMissionChannels[key]) return;
        seenMissionChannels[key] = true;
        dedupedMissionChannels.push(ch);
      });

      if (dedupedMissionChannels.length) {
        const comm1 = dedupedMissionChannels.filter(function (x) { return x.radio === 1; });
        const comm2 = dedupedMissionChannels.filter(function (x) { return x.radio === 2; });

        function sortMissionRows(list) {
          list.sort(function (a, b) {
            if (a.channel !== b.channel) return a.channel - b.channel;
            return String(a.name).localeCompare(String(b.name));
          });
        }
        sortMissionRows(comm1);
        sortMissionRows(comm2);

        return {
          col1: comm1.map(function (x) { return { label: x.label, freq: x.freq, tail: x.name ? (' ' + x.name) : '' }; }),
          col2: comm2.map(function (x) { return { label: x.label, freq: x.freq, tail: x.name ? (' ' + x.name) : '' }; })
        };
      }

      const active = radios.filter(function (r) {
        return !!(r && typeof r === 'object' && !r.intercom);
      });
      const comm1 = [];
      const comm2 = [];
      active.forEach(function (r) {
        const name = String(r.displayName || '').toUpperCase();
        if (/\b1\b|COMM\s*1|UHF/.test(name)) {
          comm1.push(r);
        } else if (/\b2\b|COMM\s*2|VHF/.test(name)) {
          comm2.push(r);
        } else if (comm1.length <= comm2.length) {
          comm1.push(r);
        } else {
          comm2.push(r);
        }
      });

      return {
        col1: comm1.map(function (r) {
          const mod = String(r.modulation || '').trim().toUpperCase() || (r.FM && !r.AM ? 'FM' : 'AM');
          const state = r.on ? 'ON' : 'OFF';
          return { label: state, freq: fmtFreq(r.frequency), tail: ' ' + mod };
        }),
        col2: comm2.map(function (r) {
          const mod = String(r.modulation || '').trim().toUpperCase() || (r.FM && !r.AM ? 'FM' : 'AM');
          const state = r.on ? 'ON' : 'OFF';
          return { label: state, freq: fmtFreq(r.frequency), tail: ' ' + mod };
        })
      };
    }

    function getCommRowParts(row) {
      if (!row) return { ch: '', freq: '' };
      const label = String(row.label || '').trim();
      const freq = (String(row.freq || '') + String(row.tail || '')).trim();
      if (/^(ON|OFF)$/i.test(label)) {
        return { ch: '', freq: (label + ' ' + freq).trim() };
      }
      return { ch: label, freq: freq };
    }

    function formatFa18CommPanelHtml(root, data, emptyWhenMissing, forceRuntimeColumns) {
      const dtc = readDtcCommPanelModel(root);
      const runtime = getRuntimeCommColumns(data);
      const useRuntime1 = !!forceRuntimeColumns || dtc.mirror1;
      const useRuntime2 = !!forceRuntimeColumns || dtc.mirror2;
      const rows1 = useRuntime1 ? runtime.col1 : dtc.rows1;
      const rows2 = useRuntime2 ? runtime.col2 : dtc.rows2;

      if (dtc.looksLikeDefaultMirrors && (!runtime.col1.length && !runtime.col2.length)) {
        return emptyWhenMissing
          ? ''
          : '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">COMMS</div><div class="fltPlanPage2Body">No comm data.</div></div>';
      }

      const maxRows = Math.max(rows1.length, rows2.length);
      if (!maxRows) {
        return emptyWhenMissing
          ? ''
          : '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">COMMS</div><div class="fltPlanPage2Body">No comm data.</div></div>';
      }

      const bodyRows = [];
      for (let i = 0; i < maxRows; i++) {
        const left = getCommRowParts(rows1[i]);
        const right = getCommRowParts(rows2[i]);
        bodyRows.push('<tr><td style="width:54px;">' + escapeHtml(left.ch) + '</td><td>' + escapeHtml(left.freq) + '</td><td style="width:54px;">' + escapeHtml(right.ch) + '</td><td>' + escapeHtml(right.freq) + '</td></tr>');
      }

      return '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">COMMS</div><div class="fltPlanPage2Body"><table class="fltPlanPage2Table"><thead><tr><th colspan="2">COMM 1' + (dtc.comm1Guard ? ' (G)' : '') + '</th><th colspan="2">COMM 2' + (dtc.comm2Guard ? ' (G)' : '') + '</th></tr><tr><th style="width:54px;">CH</th><th>FREQ</th><th style="width:54px;">CH</th><th>FREQ</th></tr></thead><tbody>' + bodyRows.join('') + '</tbody></table></div></div>';
    }

    function formatF16CommPanelHtml(root, data, emptyWhenMissing, forceRuntimeColumns) {
      const dtc = readDtcCommPanelModel(root);
      const runtime = getRuntimeCommColumns(data);
      const useRuntime1 = !!forceRuntimeColumns || dtc.mirror1;
      const useRuntime2 = !!forceRuntimeColumns || dtc.mirror2;
      const rows1 = useRuntime1 ? runtime.col1 : dtc.rows1;
      const rows2 = useRuntime2 ? runtime.col2 : dtc.rows2;

      if (dtc.looksLikeDefaultMirrors && (!runtime.col1.length && !runtime.col2.length)) {
        return emptyWhenMissing
          ? ''
          : '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">COMMS</div><div class="fltPlanPage2Body">No comm data.</div></div>';
      }

      const maxRows = Math.max(rows1.length, rows2.length);
      if (!maxRows) {
        return emptyWhenMissing
          ? ''
          : '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">COMMS</div><div class="fltPlanPage2Body">No comm data.</div></div>';
      }

      const bodyRows = [];
      for (let i = 0; i < maxRows; i++) {
        const left = getCommRowParts(rows1[i]);
        const right = getCommRowParts(rows2[i]);
        bodyRows.push('<tr><td style="width:54px;">' + escapeHtml(left.ch) + '</td><td>' + escapeHtml(left.freq) + '</td><td style="width:54px;">' + escapeHtml(right.ch) + '</td><td>' + escapeHtml(right.freq) + '</td></tr>');
      }

      return '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">COMMS</div><div class="fltPlanPage2Body"><table class="fltPlanPage2Table"><thead><tr><th colspan="2">UHF</th><th colspan="2">VHF</th></tr><tr><th style="width:54px;">CH</th><th>FREQ</th><th style="width:54px;">CH</th><th>FREQ</th></tr></thead><tbody>' + bodyRows.join('') + '</tbody></table></div></div>';
    }

    function formatDtcCommPanelHtml(root, emptyWhenMissing) {
      const comm = readDtcCommPanelModel(root);

      if (comm.looksLikeDefaultMirrors) {
        return emptyWhenMissing
          ? ''
          : '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">COMMS</div><div class="fltPlanPage2Body">No comm data.</div></div>';
      }

      const maxRows = Math.max(comm.rows1.length, comm.rows2.length);
      if (!maxRows) {
        return emptyWhenMissing
          ? ''
          : '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">COMMS</div><div class="fltPlanPage2Body">No comm data.</div></div>';
      }

      const bodyRows = [];
      for (let i = 0; i < maxRows; i++) {
        const a = comm.rows1[i];
        const b = comm.rows2[i];
        bodyRows.push('<tr><td>' + (a ? (escapeHtml(a.label) + ' ' + escapeHtml(String(a.freq))) : '') + '</td><td>' + (b ? (escapeHtml(b.label) + ' ' + escapeHtml(String(b.freq))) : '') + '</td></tr>');
      }

      return '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">COMMS</div><div class="fltPlanPage2Body"><table class="fltPlanPage2Table"><thead><tr><th>COMM 1' + (comm.comm1Guard ? ' (G)' : '') + '</th><th>COMM 2' + (comm.comm2Guard ? ' (G)' : '') + '</th></tr></thead><tbody>' + bodyRows.join('') + '</tbody></table></div></div>';
    }

    function formatAh64RuntimeCommPanelHtml(data, selected) {
      const presetNumber = getAh64CommPresetBySelection(selected);
      const server = (data && data.Server) || {};
      const diagnostics = (server && server.Diagnostics && typeof server.Diagnostics === 'object') ? server.Diagnostics : {};
      const playerUnitName = String(diagnostics.playerUnitName || '').trim();
      const tabsHtml = Array.from({ length: 10 }, function (_, i) {
        const n = i + 1;
        const activeClass = (n === presetNumber) ? ' active' : '';
        return '<button type="button" class="fltPlanPageBtn' + activeClass + '" data-ah64-comm-preset="' + String(n) + '">' + String(n) + '</button>';
      }).join('');

      function parseRuntimeRadioDeviceRow(line) {
        const text = String(line || '').trim();
        if (!text) return null;
        const map = {};
        text.split('|').forEach(function (p) {
          const idx = p.indexOf('=');
          if (idx <= 0) return;
          const key = String(p.substring(0, idx)).trim().toLowerCase();
          const value = String(p.substring(idx + 1)).trim();
          if (!key) return;
          map[key] = value;
        });
        const name = String(map.name || '').trim();
        if (!name) return null;
        return {
          name: name,
          frequency: Number(map.freq),
          modulation: Number(map.mod),
          on: String(map.on || '').toLowerCase() === 'true',
        };
      }

      function parseMissionRadioChannelRow(line) {
        const text = String(line || '').trim();
        if (!text) return null;
        const map = {};
        text.split('|').forEach(function (p) {
          const idx = p.indexOf('=');
          if (idx <= 0) return;
          const key = String(p.substring(0, idx)).trim().toLowerCase();
          const value = String(p.substring(idx + 1)).trim();
          if (!key) return;
          map[key] = value;
        });

        const radioNum = Number(map.radio);
        const channelNum = Number(map.ch);
        const freqNum = Number(map.freq);
        if (!isFinite(radioNum) || !isFinite(channelNum) || !isFinite(freqNum)) return null;

        return {
          unit: String(map.unit || '').trim(),
          radio: Math.round(radioNum),
          channel: Math.round(channelNum),
          frequency: freqNum,
        };
      }

      const runtimeDiagDevices = (Array.isArray(diagnostics.runtimeRadioDevices) ? diagnostics.runtimeRadioDevices : [])
        .map(parseRuntimeRadioDeviceRow)
        .filter(function (x) { return !!x; });

      function getRuntimeDiagRadioByName(nameKey) {
        const key = String(nameKey || '').toUpperCase();
        for (let i = 0; i < runtimeDiagDevices.length; i++) {
          const r = runtimeDiagDevices[i] || {};
          const name = String(r.name || '').toUpperCase();
          if (name.indexOf(key) >= 0) return r;
        }
        return null;
      }

      const presetChannelRowsRaw = Array.isArray(diagnostics.playerMissionRadioChannels) && diagnostics.playerMissionRadioChannels.length
        ? diagnostics.playerMissionRadioChannels
        : (Array.isArray(diagnostics.missionRadioChannels) ? diagnostics.missionRadioChannels : []);
      const presetChannelRowsParsed = presetChannelRowsRaw
        .map(parseMissionRadioChannelRow)
        .filter(function (x) { return !!x; });
      const presetChannelRows = playerUnitName
        ? presetChannelRowsParsed.filter(function (x) { return !x.unit || x.unit === playerUnitName; })
        : presetChannelRowsParsed;

      const presetChannelMap = {};
      presetChannelRows.forEach(function (x) {
        const key = String(x.radio) + '|' + String(x.channel);
        if (presetChannelMap[key] !== undefined) return;
        presetChannelMap[key] = x.frequency;
      });

      function getPresetChannelFrequencyText(radioNumber, channelNumber) {
        const key = String(Math.round(Number(radioNumber))) + '|' + String(Math.round(Number(channelNumber)));
        const value = presetChannelMap[key];
        const n = Number(value);
        if (!isFinite(n) || n <= 0) return '';
        return formatCommFrequencyMhz3(n);
      }

      function resolveRuntimeRadio(nameKey, fallbackKeys) {
        let radio = getAh64RuntimeRadioByName(data, nameKey);
        if (radio && Number(radio.frequency) > 0) return radio;

        radio = getRuntimeDiagRadioByName(nameKey);
        if (radio && Number(radio.frequency) > 0) return radio;

        const list = Array.isArray(fallbackKeys) ? fallbackKeys : [];
        for (let i = 0; i < list.length; i++) {
          const k = String(list[i] || '').trim();
          if (!k) continue;
          radio = getAh64RuntimeRadioByName(data, k);
          if (radio && Number(radio.frequency) > 0) return radio;
          radio = getRuntimeDiagRadioByName(k);
          if (radio && Number(radio.frequency) > 0) return radio;
        }
        return null;
      }

      function normalizeRuntimeRadio(radio) {
        const r = (radio && typeof radio === 'object') ? radio : {};
        const rawEnc = Number(r.encryption !== undefined ? r.encryption : r.Encryption);
        const rawMod = r.modulation;
        const rawModText = String(rawMod || '').trim().toUpperCase();
        let mappedMod = '-';
        if (rawModText === 'AM' || rawModText === 'FM' || rawModText === 'AME') {
          mappedMod = rawModText;
        } else {
          mappedMod = mapAh64Modulation(Number(rawMod));
        }
        return {
          frequency: formatCommFrequencyMhz3(Number(r.frequency)),
          modulation: mappedMod,
          encryption: mapAh64Encryption(rawEnc)
        };
      }

      function row(label, radio) {
        const r = radio || {};
        const fq = String(r.frequency || '-');
        const fqText = fq !== '-' ? fq : '-';
        return '<tr>'
          + '<td style="width:44px;">' + escapeHtml(label) + '</td>'
          + '<td style="width:186px; text-align:right; white-space:nowrap; overflow:visible; text-overflow:clip;">' + escapeHtml(fqText) + '</td>'
          + '<td style="width:34px; text-align:center;">' + escapeHtml(String(r.modulation || '-')) + '</td>'
          + '<td style="width:44px; text-align:center;">' + escapeHtml(String(r.encryption || '-')) + '</td>'
          + '</tr>';
      }

      const vhf = normalizeRuntimeRadio(resolveRuntimeRadio('VHF'));
      const uhf = normalizeRuntimeRadio(resolveRuntimeRadio('UHF', ['CB UHF']));
      const fm1 = normalizeRuntimeRadio(resolveRuntimeRadio('FM1', ['ARC-201D']));
      const fm2 = normalizeRuntimeRadio(resolveRuntimeRadio('FM2', ['ARC-201D']));
      const hf = normalizeRuntimeRadio(resolveRuntimeRadio('HF'));

      const vhfPresetFreq = getPresetChannelFrequencyText(1, presetNumber);
      const uhfPresetFreq = getPresetChannelFrequencyText(2, presetNumber);
      const fm1PresetFreq = getPresetChannelFrequencyText(3, presetNumber);
      const fm2PresetFreq = getPresetChannelFrequencyText(4, presetNumber);
      if (vhfPresetFreq) vhf.frequency = vhfPresetFreq;
      if (uhfPresetFreq) uhf.frequency = uhfPresetFreq;
      if (fm1PresetFreq) fm1.frequency = fm1PresetFreq;
      if (fm2PresetFreq) fm2.frequency = fm2PresetFreq;
      const hasAnyFreq = [vhf, uhf, fm1, fm2, hf].some(function (r) {
        const fq = String((r && r.frequency) || '').trim();
        return fq && fq !== '-';
      });
      if (!hasAnyFreq) {
        return '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">COMMS</div><div class="fltPlanPage2Body">No runtime comm data.</div></div>';
      }

      const leftRows = [
        '<tr><td style="width:110px;">Source</td><td>Runtime</td></tr>',
        '<tr><td>Preset</td><td>' + escapeHtml(String(presetNumber)) + '</td></tr>',
      ].join('');

      const freqRows = [
        row('VHF', vhf),
        row('UHF', uhf),
        row('FM1', fm1),
        row('FM2', fm2),
        row('HF Rx', hf),
        row('HF Tx', hf),
      ].join('');

      return '<div class="fltPlanPage2Section">'
        + '<div class="fltPlanPage2Title">COMMS</div>'
        + '<div class="fltPlanPage2Body">'
        + '<div style="margin:0 0 8px 0;" class="fltPlanPageSwitcher">' + tabsHtml + '</div>'
        + '<div class="fltPlanInfoBlock" style="margin:0;">'
        + '<div class="fltPlanInfoTitle">PRESET ' + escapeHtml(String(presetNumber)) + '</div>'
        + '<div class="fltPlanInfoBody">'
        + '<div class="fltPlanPage2Grid" style="grid-template-columns: 0.9fr 1.1fr;">'
        + '<div><table class="fltPlanPage2Table"><tbody>' + leftRows + '</tbody></table></div>'
        + '<div><table class="fltPlanPage2Table"><thead><tr><th colspan="4">Frequencies</th></tr></thead><tbody>' + freqRows + '</tbody></table></div>'
        + '</div>'
        + '</div>'
        + '</div>'
        + '</div>'
        + '</div>';
    }

    function formatRuntimeCommPanelHtml(data, selected) {
      if (isRuntimeAh64Module(data)) {
        return formatAh64RuntimeCommPanelHtml(data, selected);
      }
      if (isRuntimeFa18Module(data)) {
        return formatFa18CommPanelHtml(null, data, false, true);
      }
      if (isRuntimeF16Module(data)) {
        return formatF16CommPanelHtml(null, data, false, true);
      }

      const runtime = getRuntimeCommColumns(data);
      const maxRows = Math.max(runtime.col1.length, runtime.col2.length);
      if (!maxRows) {
        return '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">COMMS</div><div class="fltPlanPage2Body">No runtime comm data.</div></div>';
      }

      const bodyRows = [];
      for (let i = 0; i < maxRows; i++) {
        const a = runtime.col1[i];
        const b = runtime.col2[i];
        const left = a ? (String(a.label || '') + ' ' + String(a.freq || '') + String(a.tail || '')).trim() : '';
        const right = b ? (String(b.label || '') + ' ' + String(b.freq || '') + String(b.tail || '')).trim() : '';
        bodyRows.push('<tr><td>' + escapeHtml(left) + '</td><td>' + escapeHtml(right) + '</td></tr>');
      }

      return '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">COMMS</div><div class="fltPlanPage2Body"><table class="fltPlanPage2Table"><thead><tr><th>COMM 1</th><th>COMM 2</th></tr></thead><tbody>' + bodyRows.join('') + '</tbody></table></div></div>';
    }

    function formatRuntimeCmdsPanelHtml(data) {
      const server = (data && data.Server) || {};
      const roots = [server.Payload, server.Diagnostics];
      for (let i = 0; i < roots.length; i++) {
        const root = roots[i];
        if (!root || typeof root !== 'object') continue;
        const block = formatDtcCmdsBlockHtml(root);
        if (block) return block;
      }
      return '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">CMDS</div><div class="fltPlanPage2Body">No runtime CMDS data.</div></div>';
    }

    function formatRuntimeCmdsInfoBlockHtml(data, selected) {
      const server = (data && data.Server) || {};
      const roots = [server.Payload, server.Diagnostics];
      for (let i = 0; i < roots.length; i++) {
        const root = roots[i];
        if (!root || typeof root !== 'object') continue;
        const block = formatDtcCmdsBlockHtml(root, selected);
        if (block) return block;
      }
      const titleAttrs = getBottomPanelTitleAttrs(selected, 'CMDS');
      return '<div class="fltPlanInfoBlock"><div class="fltPlanInfoTitle"' + titleAttrs + '>CMDS</div><div class="fltPlanInfoBody">None</div></div>';
    }

    function formatRuntimePage2Html(data, pageSwitcherHtml, selected) {
      let html = '<div class="fltPlanBoard">';
      if (pageSwitcherHtml) {
        html += '<div style="margin:4px 0 6px 0;">' + pageSwitcherHtml + '</div>';
      }
      html += '<div class="fltPlanPage2Grid">';
      html += formatRuntimeCommPanelHtml(data, selected);
      html += '<div class="fltPlanPage2Stack">';
      html += formatRuntimeCmdsPanelHtml(data);
      html += formatMapMarkersPanelHtml(selected, data);
      html += '</div>';
      html += '</div></div>';
      return html;
    }

    const STORE_CLSID_LOOKUP = __VAICOM_STORE_LOOKUP_JSON__;

    function getStoreFriendlyName(clsid) {
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

    function cloneStoreStations(stations) {
      const src = Array.isArray(stations) ? stations : [];
      return src.map(function (station) {
        const s = station || {};
        return {
          CLSID: String(s.CLSID || ''),
          count: Number(s.count || 0)
        };
      });
    }

    function updateStoresBaselineSnapshot(data, liveStations) {
      const missionIdentity = String(getMissionIdentity(data || latestData || {}) || '');
      if (storesBaselineSnapshot.missionIdentity !== missionIdentity) {
        storesBaselineSnapshot.missionIdentity = missionIdentity;
        storesBaselineSnapshot.hasSnapshot = false;
        storesBaselineSnapshot.stations = [];
      }

      const incoming = Array.isArray(liveStations) ? liveStations : [];
      if (!storesBaselineSnapshot.hasSnapshot && incoming.length > 0) {
        storesBaselineSnapshot.stations = cloneStoreStations(incoming);
        storesBaselineSnapshot.hasSnapshot = true;
      }
    }

    function formatStoresPageHtml(pageSwitcherHtml, data) {
      const server = (data && data.Server) || {};
      const payload = (server && server.Payload) || {};
      const liveStations = Array.isArray(payload.Stations) ? payload.Stations : [];
      const avState = getFastAvBusDisplayState();
      const avIsLive = avState === 'LIVE';
      const avUseCurrent = avState === 'LIVE' || avState === 'ON';
      const avSource = avUseCurrent ? fastAvBus : (fastAvBus && fastAvBus.lastGood ? fastAvBus.lastGood : {});
      const aidAllowsLiveStores = avState !== 'OFF';
      updateStoresBaselineSnapshot(data, liveStations);
      const stations = aidAllowsLiveStores
        ? liveStations
        : (storesBaselineSnapshot.hasSnapshot ? storesBaselineSnapshot.stations : liveStations);

      function fmtFuel(kgValue, lbsValue) {
        const kg = formatFastAvBusNumber(kgValue, 1);
        const lbs = formatFastAvBusNumber(lbsValue, 0);
        if (kg === 'n/a' && lbs === 'n/a') return 'n/a';
        return kg + ' kg / ' + lbs + ' lb';
      }

      function applyOffNoAidData(textValue) {
        const text = String(textValue || '');
        if (avState === 'OFF' && text.toLowerCase() === 'n/a') return 'NO DATA';
        return text;
      }

      const totalFuelText = applyOffNoAidData(fmtFuel(avSource.totalFuelKg, avSource.totalFuelLbs));
      const internalFuelText = applyOffNoAidData(fmtFuel(avSource.internalFuelKg, avSource.internalFuelLbs));
      const externalFuelText = applyOffNoAidData(fmtFuel(avSource.externalFuelKg, avSource.externalFuelLbs));
      const aarOffloadKg = Number(fastAvBus && fastAvBus.aarOffloadKg);
      const aarOffloadLbs = Number(fastAvBus && fastAvBus.aarOffloadLbs);
      const aarContacts = Math.max(0, Math.round(Number(fastAvBus && fastAvBus.aarContacts) || 0));
      const aarOffloadText = (isFinite(aarOffloadKg) && aarOffloadKg > 0)
        ? (aarOffloadKg.toFixed(1) + ' kg / ' + (isFinite(aarOffloadLbs) ? aarOffloadLbs.toFixed(0) : (aarOffloadKg * 2.20462262185).toFixed(0)) + ' lb · CONTACTS: ' + String(aarContacts))
        : ((avState === 'OFF') ? 'NO DATA' : ((avState === 'ON' || avState === 'LIVE') ? 'READY' : 'n/a'));
      const freezeNote = (avState === 'OFF' || avUseCurrent) ? '' : ' (frozen last good)';

      const flowKgPerHour = Number(fastAvBus && fastAvBus.fuelFlowKgPerHour);
      const flowLbsPerHour = Number(fastAvBus && fastAvBus.fuelFlowLbsPerHour);
      const flowKgText = applyOffNoAidData(isFinite(flowKgPerHour) ? (flowKgPerHour.toFixed(0) + ' KG/H') : 'n/a');
      const flowLbsText = applyOffNoAidData(isFinite(flowLbsPerHour) ? (flowLbsPerHour.toFixed(0) + ' LB/H') : 'n/a');
      let enduranceText = applyOffNoAidData('n/a');
      if (isFinite(flowKgPerHour) && flowKgPerHour > 0 && isFinite(Number(avSource.totalFuelKg))) {
        const minutes = (Number(avSource.totalFuelKg) / flowKgPerHour) * 60;
        if (isFinite(minutes) && minutes >= 0) {
          enduranceText = minutes.toFixed(0) + ' min';
        }
      }

      let html = '<div class="fltPlanBoard">';
      if (pageSwitcherHtml) {
        html += '<div style="margin:4px 0 6px 0;">' + pageSwitcherHtml + '</div>';
      }

      html += '<div class="fltPlanAvBusHeader">';
      html += '<div class="fltPlanAvBusHeaderTitle">AIRCRAFT INTERFACE DEVICE</div>';
      html += '<div class="fltPlanAvBusHeaderStatus">DATABUS STATUS: <span class="fltPlanFuelState ' + escapeHtml(avState.toLowerCase()) + '">' + escapeHtml(avState) + '</span>' + escapeHtml(freezeNote) + '</div>';
      html += '</div>';

      html += '<div class="fltPlanStoresWrap">';
      html += '<table class="fltPlanStoresGrid">';
      html += '<thead><tr><th class="fltPlanStoresStation">STN</th><th>STORE</th></tr></thead><tbody>';

      if (!stations.length) {
        html += '<tr><td class="fltPlanStoresStation">-</td><td class="fltPlanStoresEmpty">WAITING FOR AID CONNECTION</td></tr>';
      } else {
        for (let i = 0; i < stations.length; i++) {
          const station = stations[i] || {};
          const clsid = String(station.CLSID || '').trim();
          const count = Number(station.count || 0);
          const empty = !clsid || count <= 0;
          const name = empty ? 'EMPTY' : getStoreFriendlyName(clsid);
          html += '<tr><td class="fltPlanStoresStation">' + String(i + 1) + '</td><td' + (empty ? ' class="fltPlanStoresEmpty"' : '') + '>' + escapeHtml(name) + '</td ></tr >';
        }
      }

      html += '</tbody></table></div>';

      html += '<div class="fltPlanFuelWrap">';
      html += '<div class="fltPlanFuelBlock">';
      html += '<div class="fltPlanFuelTopLine">';
      html += '<div class="fltPlanFuelHeading">FUEL</div>';
      html += '<div class="fltPlanFuelLine">FUEL FLOW: ' + escapeHtml(flowKgText) + '</div>';
      html += '</div>';
      html += '<div class="fltPlanFuelGrid">';
      html += '<div class="fltPlanFuelLeft">';
      html += '<div class="fltPlanFuelLine">TOTAL: ' + escapeHtml(totalFuelText) + '</div>';
      html += '<div class="fltPlanFuelLine">INTERNAL: ' + escapeHtml(internalFuelText) + '</div>';
      html += '<div class="fltPlanFuelLine">EXTERNAL: ' + escapeHtml(externalFuelText) + '</div>';
      html += '<div class="fltPlanFuelLine">AAR OFFLOAD: ' + escapeHtml(aarOffloadText) + '</div>';
      html += '</div>';
      html += '<div class="fltPlanFuelRight">';
      html += '<div class="fltPlanFuelLine">' + escapeHtml(flowLbsText) + '</div>';
      html += '<div class="fltPlanFuelLine">ENDURANCE: ' + escapeHtml(enduranceText) + '</div>';
      html += '</div>';
      html += '</div>';
      html += '</div>';
      html += '</div></div>';
      return html;
    }

    function formatDtcRouteSummaryHtml(root, waypoints) {
      const canonicalModel = getDtcCanonicalRouteModel(root);
      if (canonicalModel && Array.isArray(canonicalModel.routes) && canonicalModel.routes.length) {
        const rows = canonicalModel.routes.map(function (route, idx) {
          const routeKey = String((route && route.key) || ('R' + String(idx + 1))).toUpperCase();
          const routeName = String((route && route.name) || routeKey).trim() || routeKey;
          const legs = Array.isArray(route && route.legs) ? route.legs : [];
          const labels = legs
            .slice()
            .sort(function (a, b) {
              const ao = Number(a && a.order);
              const bo = Number(b && b.order);
              if (isFinite(ao) && isFinite(bo) && ao !== bo) return ao - bo;
              return Number(a && a.step) - Number(b && b.step);
            })
            .map(function (leg) {
              const step = Number(leg && leg.step);
              return isFinite(step) ? ('STP' + String(Math.round(step))) : '-';
            });
          const waypointList = labels.length ? labels.join(', ') : '-';
          return '<tr><td>' + escapeHtml(routeKey) + '</td><td>' + escapeHtml(routeName) + '</td><td>' + escapeHtml(waypointList) + '</td></tr>';
        });

        return '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">ROUTES</div><div class="fltPlanPage2Body"><table class="fltPlanPage2Table"><thead><tr><th style="width:56px;">ROUTE</th><th style="width:120px;">NAME</th><th>WAYPOINTS</th></tr></thead><tbody>' + rows.join('') + '</tbody></table></div></div>';
      }

      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];
      const navRoute = Array.isArray(wypt.NAV_ROUTE) ? wypt.NAV_ROUTE : [];
      const f14Slots = getF14RouteSlots(root);
      const routeRows = f14Slots.length
        ? f14Slots.map(function (slot) { return String((slot && slot.key) || '').toUpperCase(); }).filter(function (k) { return isValidDtcRouteKey(k); })
        : ['R1', 'R2', 'R3'];

      function isRouteSelected(v) {
        if (v === true) return true;
        if (v === false || v === null || v === undefined) return false;
        if (typeof v === 'number') return v !== 0;
        const s = String(v).trim().toLowerCase();
        return s === 'true' || s === '1' || s === 'yes' || s === 'y';
      }

      const idToStep = {};
      navPts.forEach(function (p, idx) {
        const id = String((p && p.id) || '').toUpperCase();
        const step = isFinite(Number(p && p.wypt_num)) ? Math.round(Number(p.wypt_num)) : (idx + 1);
        if (id) idToStep[id] = step;
      });

      function routeList(routeKey) {
        if (f14Slots.length) {
          const labels = (Array.isArray(waypoints) ? waypoints : [])
            .filter(function (wp) { return String((wp && wp.__routeKey) || '').toUpperCase() === routeKey; })
            .sort(function (a, b) { return Number(a && a.step) - Number(b && b.step); })
            .map(function (wp) {
              const step = String((wp && wp.step) || '-');
              const name = String((wp && wp.name) || '').trim();
              return name ? ('STP' + step + ' ' + name) : ('STP' + step);
            });
          return labels.length ? labels.join(', ') : '-';
        }

        const orderKey = routeKey + '_order';
        const points = navPts.filter(function (p) {
          return !!(p && typeof p === 'object' && isRouteSelected(p[routeKey]));
        }).sort(function (a, b) {
          const ao = Number(a && a[orderKey]);
          const bo = Number(b && b[orderKey]);
          if (isFinite(ao) && isFinite(bo) && ao !== bo) return ao - bo;
          const aw = Number(a && a.wypt_num);
          const bw = Number(b && b.wypt_num);
          if (isFinite(aw) && isFinite(bw) && aw !== bw) return aw - bw;
          return 0;
        });

        const labels = points.map(function (p) {
          const n = isFinite(Number(p && p.wypt_num)) ? Number(p && p.wypt_num) : Number(p && p.number);
          return isFinite(n) ? ('STP' + String(Math.round(n))) : '-';
        });
        if (labels.length) return labels.join(', ');

        const routeIndex = routeKey === 'R1' ? 0 : (routeKey === 'R2' ? 1 : 2);
        const routeObj = (navRoute.length > routeIndex && navRoute[routeIndex] && typeof navRoute[routeIndex] === 'object') ? navRoute[routeIndex] : {};
        const routeKeys = Object.keys(routeObj);
        if (routeKeys.length) {
          const routeSteps = routeKeys.map(function (k) {
            const rp = routeObj[k] || {};
            const wn = Number(rp.wypt_num);
            if (isFinite(wn)) return Math.round(wn);
            const mapped = idToStep[String(k || '').toUpperCase()];
            return isFinite(Number(mapped)) ? Number(mapped) : NaN;
          }).filter(function (v) { return isFinite(v); }).sort(function (a, b) { return a - b; });
          if (routeSteps.length) {
            return routeSteps.map(function (n) { return 'STP' + String(n); }).join(', ');
          }
        }

        if (routeKey === 'R1' && Array.isArray(waypoints) && waypoints.length) {
          return waypoints.map(function (wp) { return 'STP' + String(wp.step || '-'); }).join(', ');
        }

        return '-';
      }

      const body = routeRows.map(function (routeKey) {
        return '<tr><td>' + escapeHtml(routeKey) + '</td><td>' + escapeHtml(routeList(routeKey)) + '</td></tr>';
      });

      return '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">ROUTES</div><div class="fltPlanPage2Body"><table class="fltPlanPage2Table"><thead><tr><th style="width:56px;">ROUTE</th><th>WAYPOINTS</th></tr></thead><tbody>' + body.join('') + '</tbody></table></div></div>';
    }

    function formatMapMarkersPanelHtml(selected, data) {
      const server = (data && data.Server) || {};
      const rawMarkers = Array.isArray(server.MapMarkers) ? server.MapMarkers : [];
      const theatre = String(server.Theater || '').trim();
      const coordDisplayMode = getNavlogCoordDisplayMode(selected);
      const coordHeaderText = (function () {
        if (coordDisplayMode === 'dms') return 'DMS';
        if (coordDisplayMode === 'ddm') return 'DDM';
        if (coordDisplayMode === 'mgrs') return 'MGRS';
        return 'X / Y';
      })();

      const markers = rawMarkers
        .map(function (m, idx) {
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
        .filter(function (m) { return !!m; })
        .sort(function (a, b) { return Number(a.id) - Number(b.id); });

      if (!markers.length) {
        return '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">MAP MARKERS</div><div class="fltPlanPage2Body">No runtime map markers.</div></div>';
      }

      const rows = markers.map(function (m, i) {
        const displayId = i + 1;
        return '<tr><td style="width:54px;">' + escapeHtml(String(displayId)) + '</td><td>' + escapeHtml(m.text) + '</td><td style="width:220px;">' + escapeHtml(m.coordText) + '</td></tr>';
      });

      return '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">MAP MARKERS</div><div class="fltPlanPage2Body"><table class="fltPlanPage2Table fltPlanPage2MarkerTable"><thead><tr><th style="width:54px;">ID</th><th>TEXT</th><th class="fltPlanEtaHeader" style="width:220px;" data-navlog-coord-cycle="1" title="Click to cycle X/Y → DMS → DDM → MGRS">POS ' + escapeHtml(coordHeaderText) + '</th></tr></thead><tbody>' + rows.join('') + '</tbody></table></div></div>';
    }

    function getAh64MissionPartitions(root) {
      const nav = (root && typeof root === 'object' && root.NAV && typeof root.NAV === 'object')
        ? root.NAV
        : null;
      if (!nav) return ['M1'];

      function missionHasRouteData(missionNode) {
        if (!missionNode || typeof missionNode !== 'object') return false;
        const wrapped = Object.assign({}, missionNode);
        const inheritedType = String((root && root.type) || '').trim();
        if (inheritedType && !String(wrapped.type || '').trim()) wrapped.type = inheritedType;
        const canonical = getDtcCanonicalRouteModel(wrapped);
        if (!canonical || !Array.isArray(canonical.routes)) return false;
        return canonical.routes.some(function (route) {
          if (!route || route.isEnabled === false) return false;
          const legs = Array.isArray(route.legs) ? route.legs : [];
          return legs.length > 0;
        });
      }

      const parts = [];
      if (nav.Mission_1 && typeof nav.Mission_1 === 'object' && missionHasRouteData(nav.Mission_1)) parts.push('M1');
      if (nav.Mission_2 && typeof nav.Mission_2 === 'object' && missionHasRouteData(nav.Mission_2)) parts.push('M2');
      return parts.length ? parts : ['M1'];
    }

    function resolveAh64MissionRoot(root, missionKey) {
      const nav = (root && typeof root === 'object' && root.NAV && typeof root.NAV === 'object')
        ? root.NAV
        : null;
      if (!nav) return root;

      function wrapMissionNode(node, keyText) {
        if (!node || typeof node !== 'object') return root;
        const wrapped = Object.assign({}, node);
        const inheritedType = String((root && root.type) || '').trim();
        if (inheritedType && !String(wrapped.type || '').trim()) {
          wrapped.type = inheritedType;
        }
        if (!wrapped.Presets && root && typeof root === 'object' && root.Presets && typeof root.Presets === 'object') {
          wrapped.Presets = root.Presets;
        }
        wrapped.__ah64MissionKey = String(keyText || 'M1').toUpperCase();
        return wrapped;
      }

      const m = String(missionKey || 'M1').toUpperCase();
      if (m === 'M2' && nav.Mission_2 && typeof nav.Mission_2 === 'object') return wrapMissionNode(nav.Mission_2, 'M2');
      if (nav.Mission_1 && typeof nav.Mission_1 === 'object') return wrapMissionNode(nav.Mission_1, 'M1');
      if (nav.Mission_2 && typeof nav.Mission_2 === 'object') return wrapMissionNode(nav.Mission_2, 'M2');
      return root;
    }

    function getDtcAvailableRoutes(root, waypoints) {
      const canonicalModel = getDtcCanonicalRouteModel(root);
      if (canonicalModel && Array.isArray(canonicalModel.routes) && canonicalModel.routes.length) {
        const availableCanonical = ['R1'];
        const isAh64Canonical = String((canonicalModel && canonicalModel.kind) || '').toUpperCase() === 'AH64';
        canonicalModel.routes.forEach(function (route, idx) {
          const key = String((route && route.key) || ('R' + String(idx + 1))).toUpperCase();
          if (!isValidDtcRouteKey(key) || key === 'R1') return;
          if (isAh64Canonical && route && route.isEnabled === false) return;
          const legs = Array.isArray(route && route.legs) ? route.legs : [];
          if (legs.length) availableCanonical.push(key);
        });
        return availableCanonical;
      }

      const f14Slots = getF14RouteSlots(root);
      if (f14Slots.length) {
        const rows = Array.isArray(waypoints) ? waypoints : [];
        const available = ['R1'];
        f14Slots.forEach(function (slot) {
          const routeKey = String((slot && slot.key) || '').toUpperCase();
          if (!isValidDtcRouteKey(routeKey) || routeKey === 'R1') return;
          const hasRows = rows.some(function (wp) { return String((wp && wp.__routeKey) || '').toUpperCase() === routeKey; });
          if (hasRows) available.push(routeKey);
        });
        return available;
      }

      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];
      const navRoute = Array.isArray(wypt.NAV_ROUTE) ? wypt.NAV_ROUTE : [];

      function hasRouteKey(routeKey) {
        if (navPts.some(function (p) { return !!(p && typeof p === 'object' && p[routeKey] === true); })) return true;
        const idx = routeKey === 'R1' ? 0 : (routeKey === 'R2' ? 1 : 2);
        const routeObj = (navRoute.length > idx && navRoute[idx] && typeof navRoute[idx] === 'object') ? navRoute[idx] : {};
        if (Object.keys(routeObj).length > 0) return true;
        if (routeKey === 'R1' && Array.isArray(waypoints) && waypoints.length > 0) return true;
        return false;
      }

      const available = ['R1', 'R2', 'R3'].filter(hasRouteKey);
      return available.length ? available : ['R1'];
    }

    function filterDtcWaypointsByRoute(root, waypoints, routeKey) {
      const route = String(routeKey || 'R1').toUpperCase();
      if (!isValidDtcRouteKey(route)) return Array.isArray(waypoints) ? waypoints : [];

      const canonicalModel = getDtcCanonicalRouteModel(root);
      if (canonicalModel && Array.isArray(canonicalModel.routes)) {
        const rows = Array.isArray(waypoints) ? waypoints.slice() : [];
        const targetRoute = canonicalModel.routes.find(function (r) {
          return String((r && r.key) || '').toUpperCase() === route;
        });
        if (!targetRoute) return route === 'R1' ? rows : [];

        const legs = Array.isArray(targetRoute.legs) ? targetRoute.legs : [];
        if (!legs.length) return route === 'R1' ? rows : [];

        const legByStep = {};
        legs.forEach(function (leg) {
          const step = Number(leg && leg.step);
          if (!isFinite(step)) return;
          legByStep[Math.round(step)] = leg;
        });

        return rows
          .filter(function (wp) {
            const step = Number(wp && wp.step);
            return isFinite(step) && !!legByStep[Math.round(step)];
          })
          .sort(function (a, b) {
            const sa = Number(a && a.step);
            const sb = Number(b && b.step);
            const la = legByStep[Math.round(sa)] || {};
            const lb = legByStep[Math.round(sb)] || {};
            const oa = Number(la.order);
            const ob = Number(lb.order);
            if (isFinite(oa) && isFinite(ob) && oa !== ob) return oa - ob;
            return sa - sb;
          })
          .map(function (wp) {
            const step = Number(wp && wp.step);
            const leg = legByStep[Math.round(step)] || {};
            const next = Object.assign({}, wp);

            const altFeet = Number(leg.altFeet);
            if (isFinite(altFeet)) {
              next.altFeet = altFeet;
              next.alt = String(Math.round(altFeet));
            }

            const speed = Number(leg.speed);
            if (isFinite(speed)) {
              next.spd = String(Math.round(speed));
            }

            const altType = String(leg.altType || '').trim();
            if (altType) next.altType = altType;

            const speedType = String(leg.speedType || '').trim();
            if (speedType) next.speedType = speedType;

            const etaSeconds = Number(leg.etaSeconds);
            if (isFinite(etaSeconds)) {
              next.etaSourceSeconds = etaSeconds;
              next.eta = formatEtaSeconds(etaSeconds);
            }

            const waypointId = String(leg.waypointId || '').trim().toUpperCase();
            if (waypointId) next.dtcId = waypointId;

            return next;
          });
      }

      const list = Array.isArray(waypoints) ? waypoints : [];
      const hasTaggedRoutes = list.some(function (wp) { return String((wp && wp.__routeKey) || '').trim() !== ''; });
      if (list.some(function (wp) { return String((wp && wp.__routeKey) || '').toUpperCase() === route; })) {
        return list
          .filter(function (wp) { return String((wp && wp.__routeKey) || '').toUpperCase() === route; })
          .sort(function (a, b) { return Number(a && a.step) - Number(b && b.step); });
      }
      if (hasTaggedRoutes) return [];

      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];
      const navRoute = Array.isArray(wypt.NAV_ROUTE) ? wypt.NAV_ROUTE : [];
      if (!navPts.length || !list.length) return list;

      const routeNumber = Number(String(route).substring(1));
      const routeIdx = route === 'R1' ? 0 : (route === 'R2' ? 1 : 2);
      const routeObj = (navRoute.length > routeIdx && navRoute[routeIdx] && typeof navRoute[routeIdx] === 'object') ? navRoute[routeIdx] : {};
      const idToStep = {};
      navPts.forEach(function (p) {
        const id = String((p && p.id) || '').toUpperCase();
        const step = isFinite(Number(p && p.wypt_num)) ? Math.round(Number(p.wypt_num)) : (isFinite(Number(p && p.number)) ? Math.round(Number(p.number)) : NaN);
        if (id && isFinite(step)) idToStep[id] = step;
      });

      function boolish(v) {
        return v === true || String(v || '').toLowerCase() === 'true' || Number(v) === 1;
      }

      function routePointMatches(rp, allowUntyped) {
        if (!rp || typeof rp !== 'object') return false;
        const rpRouteNum = Number(rp.route_num || rp.routeNum || rp.route);
        if (isFinite(rpRouteNum) && rpRouteNum > 0) {
          return Math.round(rpRouteNum) === routeNumber;
        }
        if (boolish(rp[route])) return true;

        const hasAnyRouteTag = Object.keys(rp).some(function (k) {
          return /^R([1-9]|1[0-2])$/i.test(String(k));
        });
        return allowUntyped && !hasAnyRouteTag;
      }

      const routeEntries = [];
      Object.keys(routeObj).forEach(function (k) {
        const rp = routeObj[k] || {};
        if (!routePointMatches(rp, true)) return;
        routeEntries.push({ key: k, point: rp });
      });

      navRoute.forEach(function (obj) {
        if (!obj || typeof obj !== 'object' || obj === routeObj) return;
        Object.keys(obj).forEach(function (k) {
          const rp = obj[k] || {};
          if (!routePointMatches(rp, false)) return;
          routeEntries.push({ key: k, point: rp });
        });
      });

      const routePointByStep = {};
      const routePointById = {};
      const sourceType = String((root && root.type) || '').toUpperCase();
      const routeAltIsMeters = sourceType.indexOf('FA-18') >= 0
        || sourceType.indexOf('HORNET') >= 0
        || sourceType.indexOf('F-16') >= 0
        || sourceType.indexOf('VIPER') >= 0;
      routeEntries.forEach(function (entry) {
        const rp = entry.point || {};
        const routeId = String((entry && entry.key) || '').toUpperCase();
        if (routeId) routePointById[routeId] = rp;
        let step = Number(rp.wypt_num);
        if (!isFinite(step)) step = Number(rp.number);
        if (!isFinite(step)) step = Number(idToStep[String((entry && entry.key) || '').toUpperCase()]);
        if (!isFinite(step)) return;
        routePointByStep[Math.round(step)] = rp;
      });

      const stepSet = {};
      const routeOrder = {};
      const orderKey = route + '_order';

      navPts.forEach(function (p) {
        if (!p || typeof p !== 'object') return;
        const selected = (p[route] === true) || (String(p[route] || '').toLowerCase() === 'true') || (Number(p[route]) === 1);
        if (!selected) return;
        const step = isFinite(Number(p.wypt_num)) ? Math.round(Number(p.wypt_num)) : (isFinite(Number(p.number)) ? Math.round(Number(p.number)) : NaN);
        if (!isFinite(step)) return;
        stepSet[step] = true;
        const ord = Number(p[orderKey]);
        if (isFinite(ord)) routeOrder[step] = ord;
      });

      if (!Object.keys(stepSet).length) {
        routeEntries.forEach(function (entry) {
          const rp = entry.point || {};
          let step = Number(rp.wypt_num);
          if (!isFinite(step)) step = Number(rp.number);
          if (!isFinite(step)) step = Number(idToStep[String((entry && entry.key) || '').toUpperCase()]);
          if (!isFinite(step)) return;
          step = Math.round(step);
          stepSet[step] = true;
          const ord = Number(rp[orderKey] || rp.route_num || rp.order);
          if (isFinite(ord)) routeOrder[step] = ord;
        });
      }

      if (!Object.keys(stepSet).length) {
        return route === 'R1' ? list : [];
      }

      return list
        .filter(function (wp) { return !!stepSet[Number(wp.step)]; })
        .sort(function (a, b) {
          const sa = Number(a && a.step);
          const sb = Number(b && b.step);
          const oa = routeOrder[sa];
          const ob = routeOrder[sb];
          if (isFinite(oa) && isFinite(ob) && oa !== ob) return oa - ob;
          if (isFinite(oa) && !isFinite(ob)) return -1;
          if (!isFinite(oa) && isFinite(ob)) return 1;
          return sa - sb;
        })
        .map(function (wp) {
          const step = Number(wp && wp.step);
          const wpId = String((wp && wp.dtcId) || '').toUpperCase();
          const routePoint = routePointById[wpId] || routePointByStep[step];
          if (!routePoint || typeof routePoint !== 'object') return wp;

          const routeAlt = isFinite(Number(routePoint.alt))
            ? Number(routePoint.alt)
            : (isFinite(Number(routePoint.routeAltitude)) ? Number(routePoint.routeAltitude) : NaN);
          if (!isFinite(routeAlt)) return wp;
          const routeAltFeet = routeAltIsMeters ? (routeAlt * 3.28084) : routeAlt;

          const next = Object.assign({}, wp);
          next.altFeet = routeAltFeet;
          next.alt = String(Math.round(routeAltFeet));

          const routeAltType = String(routePoint.altitudeType || routePoint.alt_type || routePoint.altType || routePoint.alttype || '').trim();
          if (routeAltType) next.altType = routeAltType;

          return next;
        });
    }

    function formatDtcPage2Html(root, pageSwitcherHtml, waypoints, data, selected) {
      let html = '<div class="fltPlanBoard">';
      if (pageSwitcherHtml) {
        html += '<div style="margin:4px 0 6px 0;">' + pageSwitcherHtml + '</div>';
      }
      const useAh64PresetComm = isAh64DtcRoot(root) || isRuntimeAh64Module(data);
      const useFa18Comm = !useAh64PresetComm && (isFa18DtcRoot(root) || isRuntimeFa18Module(data));
      const useF16Comm = !useAh64PresetComm && !useFa18Comm && (isF16DtcRoot(root) || isRuntimeF16Module(data));
      const dtcCommHtml = useAh64PresetComm
        ? formatAh64CommPanelHtml(root, data, selected)
        : (useFa18Comm
          ? formatFa18CommPanelHtml(root, data, true)
          : (useF16Comm
            ? formatF16CommPanelHtml(root, data, true)
            : formatDtcCommPanelHtml(root, true)));
      html += '<div class="fltPlanPage2Grid">';
      html += dtcCommHtml || formatRuntimeCommPanelHtml(data);
      html += '<div class="fltPlanPage2Stack">';
      html += formatDtcRouteSummaryHtml(root, waypoints);
      html += formatMapMarkersPanelHtml(selected, data);
      html += '</div>';
      html += '</div></div>';
      return html;
    }

    function getMudMapPointType(wp) {
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

    function getMudMapSegments(points) {
      const rows = Array.isArray(points) ? points : [];
      const segs = [];
      for (let i = 1; i < rows.length; i++) {
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

    function getMudMapAssets(data, includeDlinkAssets, includeJtacAssets = true) {
      const server = (data && data.Server) || {};
      const includeDlink = (includeDlinkAssets !== false);
      const includeJtac = (includeJtacAssets !== false);
      const allFriendlyAssets = Array.isArray(server.FriendlyAssets) ? server.FriendlyAssets : [];
      const rawAssets = allFriendlyAssets.filter(function (a) {
        const category = String((a && a.Category) || '').trim().toUpperCase();
        if (category === 'PLAYER') return true;
        if (category === 'JTAC') return includeJtac;
        return includeDlink;
      });
      const mappedAssets = rawAssets
        .map(function (a) {
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
            altFrequencies: Array.isArray(a && a.AltFrequencies) ? a.AltFrequencies.map(function (v) { return String(v || '').trim(); }).filter(function (v) { return !!v; }) : [],
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
        .filter(function (a) { return !!a; });

      const rawMarkers = Array.isArray(server.MapMarkers) ? server.MapMarkers : [];
      const mappedMarkers = rawMarkers
        .map(function (m) {
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
        .filter(function (m) { return !!m; })
        .sort(function (a, b) {
          const aid = Number(a && a.markerId);
          const bid = Number(b && b.markerId);
          if (isFinite(aid) && isFinite(bid) && aid !== bid) return aid - bid;
          return 0;
        })
        .map(function (m, i) {
          m.markerDisplayId = i + 1;
          m.callsign = 'MKR ' + String(m.markerDisplayId);
          return m;
        });

      return mappedAssets.concat(mappedMarkers);
    }

    function getMudMapAssetKind(asset) {
      const category = String((asset && asset.category) || '').toUpperCase();
      if (category === 'MAP_MARKER') return 'marker';
      if (category === 'JTAC_TARGET') return 'jtac-target';
      if (category === 'TANKER') return 'tanker';
      if (category === 'AWACS') return 'awacs';
      if (category === 'JTAC') return 'jtac';
      const text = String((asset && asset.name) || '').toUpperCase();
      if (text.indexOf('HELO') >= 0 || text.indexOf('HELICOPTER') >= 0 || text.indexOf('ROTOR') >= 0) return 'rotary';
      return 'fixed';
    }

    function getDtcMpdRoot(root) {
      if (!root || typeof root !== 'object') return null;
      if (root.MPD && typeof root.MPD === 'object') return root.MPD;
      return findFirstObjectByKeyPattern(root, /^MPD$/i, 0);
    }

    function buildMapAirfields(data) {
      const model = data || latestData || {};
      const server = (model && model.Server) || {};
      const assets = Array.isArray(server.FriendlyAssets) ? server.FriendlyAssets : [];
      const atcMetars = (server && server.AtcMetars && typeof server.AtcMetars === 'object') ? server.AtcMetars : {};
      const atcIcaoTypes = (server && server.AtcIcaoTypes && typeof server.AtcIcaoTypes === 'object') ? server.AtcIcaoTypes : {};
      const metarKeys = Object.keys(atcMetars);
      const rows = [];
      const seen = {};

      function normalizeOverrideType(value) {
        const t = String(value || '').trim().toUpperCase();
        if (t === 'MIL' || t === 'CIV' || t === 'JOINT') return t;
        return '';
      }

      function normalizeOverrideLookupKey(value) {
        return String(value || '')
          .toUpperCase()
          .replace(/[\_\-\/\.,\(\)]/g, ' ')
          .replace(/\s+/g, ' ')
          .trim();
      }

      function resolveOverrideType(icao, text, fallbackKey, directType) {
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
          || (function () {
            if (!normText && !normFallback) return '';
            const keys = Object.keys(atcIcaoTypes || {});
            for (let i = 0; i < keys.length; i++) {
              const k = String(keys[i] || '');
              if (!k) continue;
              const nk = normalizeOverrideLookupKey(k);
              if (!nk) continue;
              if ((normText && (normText === nk || normText.indexOf(nk) >= 0 || nk.indexOf(normText) >= 0))
                || (normFallback && (normFallback === nk || normFallback.indexOf(nk) >= 0 || nk.indexOf(normFallback) >= 0))) {
                const t = normalizeOverrideType(atcIcaoTypes[k]);
                if (t) return t;
              }
            }
            return '';
          })()
          || '';
      }

      function tokenIcao(text) {
        const m = String(text || '').toUpperCase().match(/\b([A-Z]{4})\b/);
        return m ? String(m[1] || '') : '';
      }

      function metarIcao(text) {
        const m = String(text || '').toUpperCase().match(/\bMETAR\s+([A-Z]{4})\b/);
        return m ? String(m[1] || '') : '';
      }

      function resolveMetarKey(text) {
        const upper = String(text || '').toUpperCase();
        const compact = upper.replace(/[^A-Z0-9]/g, '');
        for (let i = 0; i < metarKeys.length; i++) {
          const k = String(metarKeys[i] || '').toUpperCase();
          if (!k) continue;
          if (upper.indexOf(k) >= 0) return metarKeys[i];
          const kc = k.replace(/[^A-Z0-9]/g, '');
          if (compact && kc && (compact.indexOf(kc) >= 0 || kc.indexOf(compact) >= 0)) return metarKeys[i];
        }
        return '';
      }

      function parseVisMeters(text) {
        const t = String(text || '').toUpperCase();
        if (!t) return NaN;
        if (t.indexOf('CAVOK') >= 0) return 10000;
        const mm = t.match(/(?:^|\s)(\d{4})(?:\s|$)/);
        const mv = mm ? Number(mm[1]) : NaN;
        return isFinite(mv) ? mv : NaN;
      }

      function isCloudVfr(text) {
        const t = String(text || '').toUpperCase();
        if (!t) return false;
        if (t.indexOf('CAVOK') >= 0) return true;
        const rx = /\b(FEW|SCT|BKN|OVC)(\d{3})\b/g;
        let m = null;
        let lowSctCount = 0;
        while ((m = rx.exec(t)) !== null) {
          const layer = String(m[1] || '').toUpperCase();
          const ft = Number(m[2]) * 100;
          if (!isFinite(ft)) continue;
          if (ft < 1500) {
            if (layer === 'BKN' || layer === 'OVC') return false;
            if (layer === 'SCT') {
              lowSctCount += 1;
              if (lowSctCount >= 2) return false;
            }
          }
        }
        return true;
      }

      function isVfrMetar(text) {
        const vis = parseVisMeters(text);
        const cloudVfr = isCloudVfr(text);
        return isFinite(vis) && cloudVfr && vis >= 5000;
      }

      assets.forEach(function (a) {
        const cat = String((a && a.Category) || (a && a.category) || '').toUpperCase();
        if (cat !== 'ATC') return;

        const north = Number(a && (a.X !== undefined ? a.X : a.x));
        const east = Number(a && (a.Y !== undefined ? a.Y : a.y));
        if (!isFinite(north) || !isFinite(east)) return;

        const text = [a && a.Callsign, a && a.Name, a && a.TypeName, a && a.Category]
          .map(function (v) { return String(v || '').trim(); })
          .filter(function (v) { return !!v; })
          .join(' ');

        const key = resolveMetarKey(text);
        const metar = key ? String(atcMetars[key] || '') : '';
        const icao = tokenIcao(text) || metarIcao(metar) || tokenIcao(key);
        const overrideType = resolveOverrideType(icao, text, key, a && a.IcaoType);
        const label = String((icao || (a && (a.Callsign || a.Name)) || 'ATC')).toUpperCase();

        const dedupeIcao = String(icao || label || 'ATC').toUpperCase();
        const dedupeKey = dedupeIcao + '|' + String(Math.round(north / 10) * 10) + '|' + String(Math.round(east / 10) * 10);
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
          altFrequencies: Array.isArray(a && a.AltFrequencies) ? a.AltFrequencies.map(function (v) { return String(v || '').trim(); }).filter(function (v) { return !!v; }) : [],
          tacan: String((a && a.Tacan) || '').trim(),
          mpClientCallsign: String((a && a.MpClientCallsign) || '').trim(),
          altFeet: Number(a && a.AltFeet),
          label: label,
          icao: String(icao || '').toUpperCase(),
          type: (u.indexOf('SEAPLANE') >= 0 ? 'seaplane' : (u.indexOf('HELI') >= 0 || u.indexOf('FARP') >= 0 ? 'heliport' : (overrideType === 'MIL' || overrideType === 'JOINT' ? 'airport' : 'airport'))),
          isMilitary: (overrideType === 'MIL' || overrideType === 'JOINT')
            ? true
            : (u.indexOf('MIL') >= 0 || u.indexOf('AIRBASE') >= 0 || u.indexOf(' AFB') >= 0 || u.indexOf('NAS') >= 0),
          isVfr: isVfrMetar(metar),
        });
      });

      return rows;
    }

    function parseAh64PointPartitions(missionRoot, routeStepSet) {
      const rootPoints = (missionRoot && missionRoot.Points && typeof missionRoot.Points === 'object')
        ? missionRoot.Points
        : {};
      const destination = [];
      const threats = [];
      const selectedSteps = (routeStepSet && typeof routeStepSet === 'object') ? routeStepSet : {};

      function normalizePointRows(items, prefix) {
        return (Array.isArray(items) ? items : [])
          .map(function (p) {
            const row = (p && typeof p === 'object') ? p : {};
            const xNum = Number(row.x);
            const yNum = Number(row.y);
            if (!isFinite(xNum) || !isFinite(yNum)) return null;
            const num = Number(row.num);
            const idText = String(row.text || '').trim();
            const noteText = String(row.note || '').trim();
            const fallbackLabel = String(prefix || 'P') + (isFinite(num) ? String(Math.round(num)) : '');
            const label = idText || noteText || fallbackLabel || 'PT';
            return {
              xNum: xNum,
              yNum: yNum,
              label: label,
              note: noteText,
              number: isFinite(num) ? Math.round(num) : NaN,
              altFeet: Number(row.alt),
              id: Number(row.id)
            };
          })
          .filter(function (r) { return !!r; });
      }

      const wpthzRows = normalizePointRows(rootPoints.WPTHZ && rootPoints.WPTHZ.POINTS, 'W');
      const ctrlmRows = normalizePointRows(rootPoints.CTRLM && rootPoints.CTRLM.POINTS, 'C');
      const tgtRows = normalizePointRows(rootPoints.TGT && rootPoints.TGT.POINTS, 'T');

      wpthzRows.forEach(function (p) {
        const pointStep = Number(p && p.number);
        if (isFinite(pointStep) && selectedSteps[Math.round(pointStep)]) return;
        destination.push({
          xNum: Number(p.xNum),
          yNum: Number(p.yNum),
          label: String(p.label || '').trim(),
          subtype: 'wpthz'
        });
      });
      ctrlmRows.forEach(function (p) {
        destination.push({
          xNum: Number(p.xNum),
          yNum: Number(p.yNum),
          label: String(p.label || '').trim(),
          subtype: 'ctrlm'
        });
      });
      tgtRows.forEach(function (p) {
        threats.push({
          xNum: Number(p.xNum),
          yNum: Number(p.yNum),
          radiusMeters: 0,
          ring: false,
          label: String(p.label || '').trim(),
          subtype: 'tgt'
        });
      });

      return {
        destinationPoints: destination,
        threatPoints: threats
      };
    }

    function getDtcMapOverlays(root, routeKey, data) {
      const mpd = getDtcMpdRoot(root);
      const sa = (root && typeof root === 'object' && root.SA && typeof root.SA === 'object')
        ? root.SA
        : findFirstObjectByKeyPattern(root, /^SA$/i, 0);
      const canonicalModel = getDtcCanonicalRouteModel(root);
      const sourceType = String((root && root.type) || '').toUpperCase();
      const isAh64Context = sourceType.indexOf('AH-64') >= 0 || sourceType.indexOf('APACHE') >= 0;
      const activeMissionKey = String((data && data.__dtcMissionKey) || 'M1').toUpperCase();

      function findAh64MissionOverlayRoot(value) {
        const candidates = [];

        function walk(node, depth) {
          if (depth > 12 || node === null || node === undefined) return;
          if (Array.isArray(node)) {
            for (let i = 0; i < node.length; i++) walk(node[i], depth + 1);
            return;
          }
          if (typeof node !== 'object') return;

          const hasLines = Array.isArray(node.Lines);
          const hasAreas = Array.isArray(node.Areas);
          const hasZones = !!(node.Zones && typeof node.Zones === 'object');
          const hasPoints = !!(node.Points && typeof node.Points === 'object');
          if (hasLines || hasAreas || hasZones || hasPoints) {
            const pointsRoot = (node.Points && typeof node.Points === 'object') ? node.Points : {};
            const wpthzCount = Array.isArray(pointsRoot.WPTHZ && pointsRoot.WPTHZ.POINTS) ? pointsRoot.WPTHZ.POINTS.length : 0;
            const ctrlmCount = Array.isArray(pointsRoot.CTRLM && pointsRoot.CTRLM.POINTS) ? pointsRoot.CTRLM.POINTS.length : 0;
            const tgtCount = Array.isArray(pointsRoot.TGT && pointsRoot.TGT.POINTS) ? pointsRoot.TGT.POINTS.length : 0;
            const zonesRoot = (node.Zones && typeof node.Zones === 'object') ? node.Zones : {};
            const nfzCount = Array.isArray(zonesRoot.NFZ) ? zonesRoot.NFZ.length : 0;
            const pfzCount = Array.isArray(zonesRoot.PFZ) ? zonesRoot.PFZ.length : 0;
            const score = (Array.isArray(node.Lines) ? node.Lines.length : 0)
              + (Array.isArray(node.Areas) ? node.Areas.length : 0)
              + nfzCount + pfzCount
              + wpthzCount + ctrlmCount + tgtCount;
            candidates.push({ node: node, score: score });
          }

          const keys = Object.keys(node);
          for (let i = 0; i < keys.length; i++) walk(node[keys[i]], depth + 1);
        }

        walk(value, 0);
        if (!candidates.length) return null;
        candidates.sort(function (a, b) { return Number(b && b.score) - Number(a && a.score); });
        return candidates[0] && candidates[0].node ? candidates[0].node : null;
      }

      const ah64MissionOverlayRoot = isAh64Context ? findAh64MissionOverlayRoot(root) : null;
      const f14Slots = getF14RouteSlots(root);
      const route = String(routeKey || 'R1').toUpperCase();
      const selectedF14Slot = f14Slots.find(function (slot) { return String((slot && slot.key) || '').toUpperCase() === route; }) || f14Slots[0] || null;
      const selectedF14Route = selectedF14Slot && selectedF14Slot.route && typeof selectedF14Slot.route === 'object'
        ? selectedF14Slot.route
        : null;

      const allGeoLines = Array.isArray(mpd && mpd.GEO_LINES) ? mpd.GEO_LINES : [];
      const geoLines = allGeoLines
        .filter(function (p) {
          if (!p || typeof p !== 'object') return false;
          return isFinite(Number(p.x)) && isFinite(Number(p.y));
        })
        .map(function (p) {
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
        .filter(function (p) { return !!p; })
        .sort(function (a, b) {
          const an = Number(a && a.number);
          const bn = Number(b && b.number);
          if (isFinite(an) && isFinite(bn) && an !== bn) return an - bn;
          return 0;
        });

      const threatPoints = (Array.isArray(mpd && mpd.THREAT_PTS) ? mpd.THREAT_PTS : [])
        .map(function (p) {
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
        .filter(function (p) { return !!p; });

      const mezThreatPoints = (Array.isArray(sa && sa.MEZ_THRTS) ? sa.MEZ_THRTS : [])
        .map(function (p) {
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
        .filter(function (p) { return !!p; });

      const destinationPoints = (Array.isArray(mpd && mpd.DEST) ? mpd.DEST : [])
        .map(function (p) {
          const xNum = Number(p && p.x);
          const yNum = Number(p && p.y);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          return {
            xNum: xNum,
            yNum: yNum,
            label: String((p && p.text) || (p && p.note) || (p && p.id) || '').trim()
          };
        })
        .filter(function (p) { return !!p; });

      function parseLineCollection(items) {
        const rows = Array.isArray(items) ? items : [];
        return rows.map(function (line) {
          const points = (Array.isArray(line && line.points) ? line.points : [])
            .map(function (pt) {
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
            .filter(function (pt) { return !!pt; });
          return {
            id: String((line && line.id) || '').trim(),
            number: Number(line && line.num),
            label: String((line && line.note) || (line && line.id) || '').trim(),
            points: points
          };
        }).filter(function (line) { return line && line.points && line.points.length > 0; });
      }

      function parseAh64LineGroups(missionRoot) {
        const lines = Array.isArray(missionRoot && missionRoot.Lines) ? missionRoot.Lines : [];
        const flot = [];
        const faor = [];

        function normalizeAh64Vertices(vertices) {
          return (Array.isArray(vertices) ? vertices : [])
            .map(function (v, idx) {
              const xNum = Number(v && v.x);
              const yNum = Number(v && v.y);
              if (!isFinite(xNum) || !isFinite(yNum)) return null;
              return {
                xNum: xNum,
                yNum: yNum,
                number: idx + 1,
                label: ''
              };
            })
            .filter(function (p) { return !!p; });
        }

        lines.forEach(function (line, idx) {
          const item = (line && typeof line === 'object') ? line : {};
          const pts = normalizeAh64Vertices(item.vertices);
          if (pts.length < 2) return;

          const lineTypeNum = Number(item.type_num);
          const typeText = String(item.text || item.note || '').trim().toUpperCase();
          const isFlot = typeText.indexOf('FLOT') >= 0 || lineTypeNum === 4;
          const isFeba = typeText.indexOf('FEBA') >= 0 || lineTypeNum === 6;
          const target = isFlot ? flot : (isFeba ? faor : flot);

          target.push({
            id: 'AH64_LINE_' + String(idx + 1),
            number: idx + 1,
            label: String(item.note || item.text || '').trim(),
            points: pts
          });
        });

        return {
          flotLines: flot,
          faorLines: faor
        };
      }

      function parseAh64AreaPolygons(missionRoot) {
        const rows = [];

        function normalizePolygon(item, idPrefix, idx, subtype, labelFallback) {
          const poly = (item && typeof item === 'object') ? item : {};
          const vertices = (Array.isArray(poly.vertices) ? poly.vertices : [])
            .map(function (v, pointIdx) {
              const xNum = Number(v && v.x);
              const yNum = Number(v && v.y);
              if (!isFinite(xNum) || !isFinite(yNum)) return null;
              return {
                xNum: xNum,
                yNum: yNum,
                number: pointIdx + 1,
                label: ''
              };
            })
            .filter(function (p) { return !!p; });
          if (vertices.length < 3) return null;

          const closed = vertices.slice();
          const first = closed[0];
          const last = closed[closed.length - 1];
          if (!last || Number(last.xNum) !== Number(first.xNum) || Number(last.yNum) !== Number(first.yNum)) {
            closed.push({
              xNum: Number(first.xNum),
              yNum: Number(first.yNum),
              number: vertices.length + 1,
              label: ''
            });
          }

          const label = String(poly.note || poly.text || labelFallback || '').trim();
          return {
            id: idPrefix + String(idx + 1),
            number: idx + 1,
            label: label,
            subtype: String(subtype || '').toLowerCase(),
            points: closed
          };
        }

        const areas = Array.isArray(missionRoot && missionRoot.Areas) ? missionRoot.Areas : [];
        areas.forEach(function (area, idx) {
          const note = String((area && area.note) || '').trim();
          const upper = note.toUpperCase();
          const subtype = upper.indexOf('ENGAGE') >= 0 ? 'engage' : 'area';
          const row = normalizePolygon(area, 'AH64_AREA_', idx, subtype, 'AREA');
          if (row) rows.push(row);
        });

        const zones = (missionRoot && missionRoot.Zones && typeof missionRoot.Zones === 'object') ? missionRoot.Zones : {};
        const nfz = Array.isArray(zones.NFZ) ? zones.NFZ : [];
        nfz.forEach(function (zone, idx) {
          const row = normalizePolygon(zone, 'AH64_NFZ_', idx, 'nfz', 'NFZ');
          if (row) rows.push(row);
        });
        const pfz = Array.isArray(zones.PFZ) ? zones.PFZ : [];
        pfz.forEach(function (zone, idx) {
          const row = normalizePolygon(zone, 'AH64_PFZ_', idx, 'pfz', 'PFZ');
          if (row) rows.push(row);
        });

        return rows;
      }

      const faorRoot = sa && sa.FAOR_FLOT && typeof sa.FAOR_FLOT === 'object' ? sa.FAOR_FLOT : null;
      const nativeFaorLines = parseLineCollection(faorRoot && faorRoot.FAOR);
      const nativeFlotLines = parseLineCollection(faorRoot && faorRoot.FLOT);
      const missingAh64OverlayHelpers = [];
      if (typeof parseAh64LineGroups !== 'function') missingAh64OverlayHelpers.push('parseAh64LineGroups');
      if (typeof parseAh64AreaPolygons !== 'function') missingAh64OverlayHelpers.push('parseAh64AreaPolygons');
      if (typeof parseAh64PointPartitions !== 'function') missingAh64OverlayHelpers.push('parseAh64PointPartitions');
      if (missingAh64OverlayHelpers.length) {
        try {
          const msg = 'AH-64 overlay helper(s) missing: ' + missingAh64OverlayHelpers.join(', ');
          if (!window.__okbMissingAh64HelpersLogged || window.__okbMissingAh64HelpersLogged !== msg) {
            window.__okbMissingAh64HelpersLogged = msg;
            setStatus(msg, 'warning');
            if (typeof console !== 'undefined' && console && typeof console.warn === 'function') {
              console.warn(msg);
            }
          }
        } catch (_) {
        }
      }
      const ah64LineGroups = (typeof parseAh64LineGroups === 'function')
        ? parseAh64LineGroups(ah64MissionOverlayRoot)
        : { faorLines: [], flotLines: [] };
      const ah64AreaPolygons = (typeof parseAh64AreaPolygons === 'function')
        ? parseAh64AreaPolygons(ah64MissionOverlayRoot)
        : [];
      const selectedRouteStepSet = {};
      const selectedCanonicalRoute = (canonicalModel && Array.isArray(canonicalModel.routes))
        ? canonicalModel.routes.find(function (r) {
          return String((r && r.key) || '').toUpperCase() === route;
        })
        : null;
      (Array.isArray(selectedCanonicalRoute && selectedCanonicalRoute.legs) ? selectedCanonicalRoute.legs : []).forEach(function (leg) {
        const step = Number(leg && leg.step);
        if (!isFinite(step)) return;
        selectedRouteStepSet[Math.round(step)] = true;
      });
      const ah64PartitionPoints = (typeof parseAh64PointPartitions === 'function')
        ? parseAh64PointPartitions(ah64MissionOverlayRoot, selectedRouteStepSet)
        : { destinationPoints: [], threatPoints: [] };
      const faorLines = nativeFaorLines.concat(Array.isArray(ah64LineGroups.faorLines) ? ah64LineGroups.faorLines : []);
      const flotLines = nativeFlotLines.concat(Array.isArray(ah64LineGroups.flotLines) ? ah64LineGroups.flotLines : []);

      const capPoints = (Array.isArray(sa && sa.CAP_PTS) ? sa.CAP_PTS : [])
        .map(function (p) {
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
        .filter(function (p) { return !!p; });

      const corridors = parseLineCollection(sa && sa.CORRIDORS);

      const f14AdditionalPoints = (Array.isArray(selectedF14Route && selectedF14Route.additional_points) ? selectedF14Route.additional_points : [])
        .map(function (p) {
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
        .filter(function (p) { return !!p; });

      const f14Lines = (Array.isArray(selectedF14Route && selectedF14Route.lines) ? selectedF14Route.lines : [])
        .map(function (line, lineIdx) {
          const points = (Array.isArray(line && line.points) ? line.points : [])
            .map(function (pt, pointIdx) {
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
            .filter(function (pt) { return !!pt; });
          return {
            id: 'F14-LINE-' + String(lineIdx + 1),
            number: lineIdx + 1,
            label: String((line && line.name) || '').trim(),
            points: points
          };
        })
        .filter(function (line) { return line && Array.isArray(line.points) && line.points.length > 1; });

      const jdamThreatPoints = (Array.isArray(root && root.JDAM && root.JDAM.stations) ? root.JDAM.stations : [])
        .reduce(function (acc, station) {
          const targets = Array.isArray(station && station.targets) ? station.targets : [];
          targets.forEach(function (target) {
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
        .filter(function (p) { return !!(p && p.isBullseye); })
        .map(function (p) {
          return {
            xNum: Number(p.xNum),
            yNum: Number(p.yNum),
            label: String(p.label || 'BULLSEYE')
          };
        });

      const f14AreaThreatPoints = f14AdditionalPoints
        .filter(function (p) {
          const typeRaw = String((p && p.typeRaw) || '').toUpperCase();
          return typeRaw === 'DP' || typeRaw === 'HA';
        })
        .map(function (p) {
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
        .filter(function (p) {
          if (!p || p.isBullseye) return false;
          const typeRaw = String((p && p.typeRaw) || '').toUpperCase();
          return typeRaw !== 'DP' && typeRaw !== 'HA';
        })
        .map(function (p) {
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

      const ah64MissionLabel = isAh64Context ? activeMissionKey : '';
      return {
        geolines: geoLines,
        threatPoints: threatPoints.concat(mezThreatPoints).concat(jdamThreatPoints).concat(f14AreaThreatPoints).concat(Array.isArray(ah64PartitionPoints.threatPoints) ? ah64PartitionPoints.threatPoints : []),
        samThreatPoints: filterAwacsSamThreatPointsBySelection(parseAwacsSamThreatMapPoints(data)),
        destinationPoints: destinationPoints.concat(f14BullseyeDestinations).concat(f14GeneralDestinations).concat(Array.isArray(ah64PartitionPoints.destinationPoints) ? ah64PartitionPoints.destinationPoints : []),
        faorLines: faorLines.concat(f14Lines),
        flotLines: flotLines,
        capPoints: capPoints,
        corridors: corridors,
        areaPolygons: ah64AreaPolygons,
        airfields: (typeof buildMapAirfields === 'function')
          ? buildMapAirfields(data || latestData)
          : ((typeof BuildMapAirfields === 'function') ? BuildMapAirfields(data || latestData) : []),
        ah64MissionKey: ah64MissionLabel
      };
    }

    function isBullseyeText(text) {
      const s = String(text || '').trim().toUpperCase();
      if (!s) return false;
      if (s === 'BULL' || s === 'BULLSEYE') return true;
      if (s.indexOf('BULLSEYE') >= 0) return true;
      return /(^|\W)BULL(\W|$)/.test(s);
    }

    function toBullseyePoint(x, y, label) {
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

    function getMapBullseyePoint(root, waypoints, overlays, data) {
      const model = data || latestData || {};
      const server = (model && model.Server) || {};
      const diagnostics = (server && typeof server.Diagnostics === 'object' && server.Diagnostics) || {};
      const diagX = Number(diagnostics && diagnostics.bullseyeX);
      const diagY = Number(diagnostics && diagnostics.bullseyeY);
      const diagValid = !!(diagnostics && diagnostics.bullseyeValid);
      if (diagValid && isFinite(diagX) && isFinite(diagY)) {
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

      for (let i = 0; i < navPts.length; i++) {
        const p = navPts[i] || {};
        const label = String(p.note || p.text_note || p.name || p.text || p.id || '').trim();
        if (!isBullseyeText(label)) continue;
        const found = toBullseyePoint(p.x, p.y, label);
        if (found) return found;
      }

      const rows = Array.isArray(waypoints) ? waypoints : [];
      for (let i = 0; i < rows.length; i++) {
        const wp = rows[i] || {};
        const label = String(wp.name || wp.label || wp.note || '').trim();
        if (!isBullseyeText(label)) continue;
        const found = toBullseyePoint(wp.xNum, wp.yNum, label);
        if (found) return found;
      }

      const mapOverlays = overlays && typeof overlays === 'object' ? overlays : {};
      const destinationPoints = Array.isArray(mapOverlays.destinationPoints) ? mapOverlays.destinationPoints : [];
      for (let i = 0; i < destinationPoints.length; i++) {
        const p = destinationPoints[i] || {};
        const label = String(p.label || p.text || p.note || p.id || '').trim();
        if (!isBullseyeText(label)) continue;
        const found = toBullseyePoint(p.xNum, p.yNum, label);
        if (found) return found;
      }

      return null;
    }

    function buildOpenFreeMapPayload(waypoints, data, overlays, bullseyePoint, historySelection) {
      const rows = Array.isArray(waypoints) ? waypoints.filter(function (wp) {
        return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum));
      }) : [];
      const allowNavlog = (efbSaShowNavlog !== false);
      const allowDtcOverlay = (efbSaShowDtcOverlay !== false);
      const allowAirports = (efbSaShowAirports !== false);
      const allowJtacTargets = (efbSaShowJtacTargets !== false);
      const allowSamThreats = (efbSaShowSamThreatRings === true);
      const visibleRows = allowNavlog ? rows : [];
      const doghouseRows = normalizeDoghouseMapLegRows(rows);
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
      for (let i = 0; i < theatreCandidates.length; i++) {
        const candidate = String(theatreCandidates[i] || '').trim();
        if (!candidate) continue;
        if (getMapProjectionByTheatre(candidate)) {
          theatre = candidate;
          break;
        }
      }
      if (!theatre) {
        openFreeMapLastPayloadStatus = 'no-theatre';
        return null;
      }

      const mapOverlays = overlays && typeof overlays === 'object'
        ? {
          geolines: allowDtcOverlay ? (Array.isArray(overlays.geolines) ? overlays.geolines : []) : [],
          threatPoints: allowDtcOverlay ? (Array.isArray(overlays.threatPoints) ? overlays.threatPoints : []) : [],
          samThreatPoints: allowSamThreats ? (Array.isArray(overlays.samThreatPoints) ? overlays.samThreatPoints : []) : [],
          destinationPoints: allowDtcOverlay ? (Array.isArray(overlays.destinationPoints) ? overlays.destinationPoints : []) : [],
          faorLines: allowDtcOverlay ? (Array.isArray(overlays.faorLines) ? overlays.faorLines : []) : [],
          flotLines: allowDtcOverlay ? (Array.isArray(overlays.flotLines) ? overlays.flotLines : []) : [],
          capPoints: allowDtcOverlay ? (Array.isArray(overlays.capPoints) ? overlays.capPoints : []) : [],
          corridors: allowDtcOverlay ? (Array.isArray(overlays.corridors) ? overlays.corridors : []) : [],
          areaPolygons: allowDtcOverlay ? (Array.isArray(overlays.areaPolygons) ? overlays.areaPolygons : []) : [],
          airfields: allowAirports ? (Array.isArray(overlays.airfields) ? overlays.airfields : []) : [],
        }
        : { geolines: [], threatPoints: [], samThreatPoints: [], destinationPoints: [], faorLines: [], flotLines: [], capPoints: [], corridors: [], areaPolygons: [] };
      const jtacTargets = allowJtacTargets ? parseJtacNineLineTargets(model) : [];
      const jtacOverlay = allowJtacTargets
        ? resolveJtacOverlayTargets(model, jtacTargets)
        : { active: null, history: [] };
      const activeJtacTarget = jtacOverlay && jtacOverlay.active ? jtacOverlay.active : null;
      const historyJtacTargets = jtacOverlay && Array.isArray(jtacOverlay.history) ? jtacOverlay.history : [];
      const historyTrackSelection = getFlightPlanEtaStartKey(historySelection || '')
        || getFlightPlanEtaStartKey(getActiveEfbSaSelectionKey())
        || getFlightPlanEtaStartKey(getActiveFlightPlanSelection(model));
      const historyTrackRows = (efbSaShowHistoryTrack === true)
        ? getEfbSaHistoryTrackByKey(historyTrackSelection)
        : [];
      const savedHistoryTracks = (efbSaShowHistoryTrack === true)
        ? getEfbSaSavedHistoryTracksBySelection(historyTrackSelection)
          .filter(function (row) { return row && row.visible !== false && Array.isArray(row.points) && row.points.length; })
        : [];

      function toLonLat(point) {
        const north = Number(point && point.xNum);
        const east = Number(point && point.yNum);
        if (!isFinite(north) || !isFinite(east)) return null;

        function validLonLat(lon, lat) {
          return isFinite(Number(lon))
            && isFinite(Number(lat))
            && Math.abs(Number(lat)) <= 90
            && Math.abs(Number(lon)) <= 180;
        }

        const llPrimary = convertDcsXYToLatLon(theatre, north, east);
        if (llPrimary && validLonLat(llPrimary.lon, llPrimary.lat)) {
          return [Number(llPrimary.lon), Number(llPrimary.lat)];
        }

        const llSwapped = convertDcsXYToLatLon(theatre, east, north);
        if (llSwapped && validLonLat(llSwapped.lon, llSwapped.lat)) {
          return [Number(llSwapped.lon), Number(llSwapped.lat)];
        }

        if (validLonLat(east, north)) {
          return [east, north];
        }
        if (validLonLat(north, east)) {
          return [north, east];
        }

        return null;
      }

      function addAreaPolygons(polygons) {
        (Array.isArray(polygons) ? polygons : []).forEach(function (poly) {
          const pts = (Array.isArray(poly && poly.points) ? poly.points : [])
            .map(function (p) {
              return {
                xNum: Number(p && p.xNum),
                yNum: Number(p && p.yNum),
                number: Number(p && p.number)
              };
            })
            .filter(function (p) { return isFinite(p.xNum) && isFinite(p.yNum); })
            .sort(function (a, b) {
              if (isFinite(a.number) && isFinite(b.number) && a.number !== b.number) return a.number - b.number;
              return 0;
            });
          if (pts.length < 3) return;

          const subtype = String((poly && poly.subtype) || '').toLowerCase();
          const stroke = (subtype === 'nfz') ? '#d96a6a'
            : (subtype === 'pfz' ? '#d9a64f' : '#9f7ad0');
          const fill = (subtype === 'nfz') ? 'rgba(217,106,106,0.18)'
            : (subtype === 'pfz' ? 'rgba(217,166,79,0.16)' : 'rgba(159,122,208,0.12)');

          addLineFeature(pts, {
            stroke: stroke,
            lineWidth: 1.8,
            dashed: true,
          });

          const labelPoint = pts[0] || null;
          const label = String((poly && poly.label) || '').trim();
          if (labelPoint && label) {
            addPointFeature(labelPoint, {
              kind: 'area-label',
              group: 'overlay',
              label: label,
              fill: stroke,
              stroke: stroke,
              textColor: stroke,
              radius: 1.2,
            });
          }

          const coords = pts
            .map(function (p) { return toLonLat(p); })
            .filter(function (c) { return Array.isArray(c) && c.length === 2; });
          if (coords.length >= 3) {
            coords.forEach(function (c) { pointsForBounds.push(c); });
            features.push({
              type: 'Feature',
              geometry: {
                type: 'Polygon',
                coordinates: [coords],
              },
              properties: {
                kind: 'area-polygon',
                group: 'overlay',
                stroke: stroke,
                fill: fill,
                lineWidth: 1.6,
                dashed: true,
                label: label,
              },
            });
          }
        });
      }

      const pointsForBounds = [];
      const features = [];

      function addPointFeature(point, props) {
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

      function addLineFeature(points, props) {
        const coords = (Array.isArray(points) ? points : [])
          .map(function (p) { return toLonLat(p); })
          .filter(function (c) { return Array.isArray(c) && c.length === 2; });
        if (coords.length < 2) return;
        coords.forEach(function (c) { pointsForBounds.push(c); });
        features.push({
          type: 'Feature',
          geometry: {
            type: 'LineString',
            coordinates: coords,
          },
          properties: props || {},
        });
      }

      function addJtacTargetSupportFeatures(target) {
        const t = target || {};
        [
          { innerNm: 1, outerNm: 5, fillOpacity: 0.18, label: 'Attack Sector' },
          { innerNm: 5, outerNm: 10, fillOpacity: 0.12, label: '' },
          { innerNm: 10, outerNm: 15, fillOpacity: 0.07, label: '' },
        ].forEach(function (band) {
          const reciprocalSector = getJtacReciprocalSectorPolygonPoints(t, band.innerNm, band.outerNm);
          if (!reciprocalSector || !Array.isArray(reciprocalSector.points) || reciprocalSector.points.length < 6) return;
          const sectorCoords = reciprocalSector.points
            .map(function (p) { return toLonLat(p); })
            .filter(function (c) { return Array.isArray(c) && c.length === 2; });
          if (sectorCoords.length < 6) return;
          const tgtLonLat = toLonLat(t);
          if (Array.isArray(tgtLonLat) && tgtLonLat.length === 2) pointsForBounds.push(tgtLonLat);
          sectorCoords.forEach(function (c) { pointsForBounds.push(c); });
          features.push({
            type: 'Feature',
            geometry: {
              type: 'Polygon',
              coordinates: [sectorCoords],
            },
            properties: {
              kind: 'jtac-attack-sector',
              group: 'overlay',
              fill: '#4faa4f',
              fillOpacity: band.fillOpacity,
              stroke: '#2f8a2f',
              lineWidth: 1.4,
              dashed: false,
              label: band.label,
            },
          });
        });

        const friendlyPoint = getJtacFriendlyPointFromTarget(t);
        if (friendlyPoint) {
          const friendlyLabel = (isFinite(Number(t.friendlyDistanceMeters))
            ? ('FRND ' + String(Math.round(Number(t.friendlyDistanceMeters))) + 'm')
            : (isFinite(Number(t.friendlyRangeNm)) ? ('FRND ' + String(Number(t.friendlyRangeNm).toFixed(1)) + 'NM') : 'FRND'));
          addPointFeature(friendlyPoint, {
            kind: 'jtac-friendly',
            group: 'overlay',
            label: friendlyLabel,
            fill: '#3a7ec1',
            stroke: '#1f5d93',
            textColor: '#1f5d93',
            radius: 4.3,
          });
          addLineFeature([t, friendlyPoint], {
            stroke: '#3a7ec1',
            lineWidth: 1.7,
            dashed: true,
          });
        }

        const egressPoint = getJtacEgressPointFromTarget(t, 5);
        if (egressPoint) {
          const targetLonLat = toLonLat(t);
          addLineFeature([t, egressPoint], {
            stroke: '#7f2e2e',
            lineWidth: 2.0,
            dashed: false,
          });
          addPointFeature(egressPoint, {
            kind: 'jtac-egress',
            group: 'overlay',
            label: 'Egress',
            fill: '#7f2e2e',
            stroke: '#7f2e2e',
            textColor: '#7f2e2e',
            radius: 2.8,
            parentTargetLonLat: targetLonLat,
          });
        }
      }

      const byStep = {};
      visibleRows.forEach(function (wp) { byStep[String(wp && wp.step)] = wp; });
      getMudMapSegments(visibleRows).forEach(function (seg) {
        const from = byStep[String(seg && seg.from && seg.from.step)] || (seg && seg.from);
        const to = byStep[String(seg && seg.to && seg.to.step)] || (seg && seg.to);
        if (!from || !to) return;
        addLineFeature([from, to], {
          stroke: '#d21fa6',
          lineWidth: seg && seg.dashed ? 2.2 : 2.8,
          dashed: !!(seg && seg.dashed)
        });
      });

      historyTrackRows.forEach(function (p) {
        addPointFeature(p, {
          kind: 'history-track-dot',
          group: 'overlay',
          fill: '#000000',
          stroke: '#000000',
          radius: 2.2,
        });
      });
      savedHistoryTracks.forEach(function (row, idx) {
        const shade = Math.max(80, 165 - (idx * 16));
        const color = 'rgb(' + String(shade) + ',' + String(shade) + ',' + String(shade) + ')';
        row.points.forEach(function (p) {
          addPointFeature(p, {
            kind: 'history-track-dot-saved',
            group: 'overlay',
            fill: color,
            stroke: color,
            radius: 2.0,
          });
        });
      });

      if (efbSaShowDoghouses !== false) {
        const doghouseLegs = buildDoghouseLegData(doghouseRows, theatre);
        doghouseLegs.forEach(function (d) {
          const heading = computeTrueHeadingDeg(
            { x: Number(d.fromXNum), y: Number(d.fromYNum) },
            { x: Number(d.toXNum), y: Number(d.toYNum) });
          addPointFeature({ xNum: Number(d.midXNum), yNum: Number(d.midYNum) }, {
            kind: 'doghouse',
            group: 'overlay',
            headingDeg: heading,
            mhText: String(d.mhText || '-'),
            distText: String(d.distText || '-'),
            eteText: String(d.eteText || '--:--'),
            altText: String(d.altText || '-'),
          });
        });
      }

      historyJtacTargets.forEach(function (h) {
        const xNum = Number(h && h.xNum);
        const yNum = Number(h && h.yNum);
        if (!isFinite(xNum) || !isFinite(yNum)) return;
        const seq = Math.max(1, Math.round(Number(h && h.seq) || 1));
        const label = 'TGT-' + String(seq);
        const selectionAsset = {
          callsign: 'JTAC TGT',
          name: 'History ' + label,
          category: 'JTAC_TARGET',
          xNum: xNum,
          yNum: yNum,
        };
        addPointFeature({ xNum: xNum, yNum: yNum }, {
          kind: 'jtac-target',
          group: 'asset',
          assetKind: 'jtac-target',
          assetKey: makeMapAssetSelectionKey(selectionAsset),
          label: label,
          fill: '#1a1a1a',
          stroke: '#000000',
          textColor: '#000000',
          radius: 4.8,
          category: 'JTAC_TARGET',
          typeName: String((h && h.unitType) || '').trim(),
          mgrsText: String((h && h.mgrsText) || '').toUpperCase().trim(),
          elevationFeet: Number(h && h.elevationFeet),
          source: 'JTAC history',
        });
      });

      if (activeJtacTarget) {
        const t = activeJtacTarget || {};
        const unitType = String(t.unitType || t.typeName || '').trim();
        const label = String(t.label || 'JTAC TGT').trim() || 'JTAC TGT';
        const mgrsText = String(t.mgrsText || '').toUpperCase().trim();
        const elevationFeet = Number(t.elevationFeet);
        const friendlyBearing = Number(t.friendlyBearing);
        const friendlyRangeNm = Number(t.friendlyRangeNm);
        const selectionAsset = {
          callsign: String(t.callsign || 'JTAC TGT').trim(),
          name: String(t.name || unitType || label).trim(),
          category: 'JTAC_TARGET',
          xNum: Number(t.xNum),
          yNum: Number(t.yNum),
        };
        addPointFeature(t, {
          kind: 'jtac-target',
          group: 'asset',
          assetKind: 'jtac-target',
          assetKey: makeMapAssetSelectionKey(selectionAsset),
          label: label,
          fill: '#d45757',
          stroke: '#7f2e2e',
          textColor: '#7f2e2e',
          radius: 4.8,
          category: 'JTAC_TARGET',
          callsign: String(t.callsign || 'JTAC TGT').trim(),
          name: String(t.name || unitType || label).trim(),
          typeName: unitType,
          unitType: unitType,
          mgrsText: mgrsText,
          elevationFeet: isFinite(elevationFeet) ? Math.round(elevationFeet) : NaN,
          altFeet: isFinite(elevationFeet) ? Math.round(elevationFeet) : 0,
          friendlyBearing: isFinite(friendlyBearing) ? Math.round(friendlyBearing) : NaN,
          friendlyRangeNm: isFinite(friendlyRangeNm) ? Number(friendlyRangeNm.toFixed(1)) : NaN,
          attackHeadingStartDeg: Number(t.attackHeadingStartDeg),
          attackHeadingEndDeg: Number(t.attackHeadingEndDeg),
          nl4Text: String(t.nl4Text || '').trim(),
          nl5Text: String(t.nl5Text || '').trim(),
          nl6Text: String(t.nl6Text || '').trim(),
          nl7Text: String(t.nl7Text || '').trim(),
          nl8Text: String(t.nl8Text || '').trim(),
          nl9Text: String(t.nl9Text || '').trim(),
          rmkText: String(t.rmkText || '').trim(),
          engagementType: String(t.engagementType || '').trim(),
          source: 'JTAC nine-line',
        });
        addJtacTargetSupportFeatures(t);
      }

      visibleRows.forEach(function (wp) {
        const step = String((wp && wp.stepDisplay !== undefined && wp.stepDisplay !== null && String(wp.stepDisplay).trim()) ? wp.stepDisplay : (wp && wp.step || '')).trim();
        const name = String(wp && wp.name || '').trim();
        const label = step ? (step + (name ? (' ' + name) : '')) : name;
        const waypointType = getMudMapPointType(wp);
        addPointFeature(wp, {
          kind: 'waypoint',
          waypointType: waypointType,
          label: label || 'WP',
          fill: '#2d8fe3',
          stroke: '#1f5d93',
          textColor: '#1f3550',
          radius: 4,
        });
      });

      (Array.isArray(mapOverlays.geolines) ? mapOverlays.geolines : []).forEach(function (p) {
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
      ['L1', 'L2', 'L3', 'L4'].forEach(function (lineKey) {
        const linePoints = geolineRows
          .filter(function (p) {
            const flags = (p && Array.isArray(p.lineFlags)) ? p.lineFlags : [];
            return flags.indexOf(lineKey) >= 0;
          })
          .sort(function (a, b) {
            const an = Number(a && a.number);
            const bn = Number(b && b.number);
            if (isFinite(an) && isFinite(bn) && an !== bn) return an - bn;
            return 0;
          });
        if (linePoints.length >= 2) {
          addLineFeature(linePoints, { stroke: '#7a57b3', lineWidth: 2.2, dashed: true });
        }
      });

      function addGroupedLines(groups, strokeColor) {
        (Array.isArray(groups) ? groups : []).forEach(function (g) {
          const pts = (Array.isArray(g && g.points) ? g.points : []);
          addLineFeature(pts, { stroke: strokeColor, lineWidth: 2, dashed: true });
        });
      }

      function addCorridorBounds(groups) {
        const corridorHalfWidthMeters = 5000;
        (Array.isArray(groups) ? groups : []).forEach(function (group) {
          const pts = (Array.isArray(group && group.points) ? group.points : [])
            .map(function (p) {
              return {
                xNum: Number(p && p.xNum),
                yNum: Number(p && p.yNum),
                number: Number(p && p.number)
              };
            })
            .filter(function (p) { return isFinite(p.xNum) && isFinite(p.yNum); })
            .sort(function (a, b) {
              if (isFinite(a.number) && isFinite(b.number) && a.number !== b.number) return a.number - b.number;
              return 0;
            });
          if (pts.length < 2) return;

          const left = [];
          const right = [];
          for (let i = 0; i < pts.length; i++) {
            const prev = pts[Math.max(0, i - 1)];
            const next = pts[Math.min(pts.length - 1, i + 1)];
            const dNorth = Number(next.xNum) - Number(prev.xNum);
            const dEast = Number(next.yNum) - Number(prev.yNum);
            const len = Math.sqrt((dNorth * dNorth) + (dEast * dEast));
            if (!isFinite(len) || len <= 0) {
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
      addAreaPolygons(mapOverlays.areaPolygons);

      (Array.isArray(mapOverlays.destinationPoints) ? mapOverlays.destinationPoints : []).forEach(function (p) {
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

      (Array.isArray(mapOverlays.threatPoints) ? mapOverlays.threatPoints : []).forEach(function (p) {
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

      (Array.isArray(mapOverlays.samThreatPoints) ? mapOverlays.samThreatPoints : []).forEach(function (p) {
        addPointFeature(p, {
          kind: 'threat',
          threatSubtype: 'sam',
          label: String((p && p.samDisplayCode) || '').trim() || String((p && p.samCode) || '').trim() || String((p && p.label) || '').trim(),
          fill: '#bf3a5f',
          stroke: '#8e2b47',
          textColor: '#8e2b47',
          radius: 4.2,
          ring: !!(p && p.ring),
          radiusMeters: Number(p && p.radiusMeters),
          ringRadiiNm: Array.isArray(p && p.ringRadiiNm) ? p.ringRadiiNm : [],
          confidenceBandsNm: Array.isArray(p && p.confidenceBandsNm) ? p.confidenceBandsNm : [],
          confidence: String((p && p.confidence) || 'medium').toLowerCase(),
          samConfirmedHostile: !!(p && p.samConfirmedHostile),
          samIconResolved: !!(p && p.samIconResolved),
          samNavalPlatformCode: String((p && p.samNavalPlatformCode) || '').trim(),
          labelMinZoom: 6.8,
          assetKey: String((p && p.assetKey) || ''),
          category: 'SAM',
          group: 'overlay',
        });
      });

      (Array.isArray(mapOverlays.capPoints) ? mapOverlays.capPoints : []).forEach(function (p) {
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

      if (bullseyePoint) {
        addPointFeature(bullseyePoint, {
          kind: 'bullseye',
          label: String((bullseyePoint && bullseyePoint.label) || 'BULLSEYE'),
          fill: '#f0c544',
          stroke: '#7a6420',
          textColor: '#5f4b1b',
          radius: 5,
        });
      }

      (Array.isArray(mapOverlays.airfields) ? mapOverlays.airfields : []).forEach(function (p) {
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
        const rawAssets = getMudMapAssets(model, dlinkOnEnabled, allowJtacTargets);
      const ownshipPoint = getPlayerMapPoint(model);
      const ownshipHeadingResolver = (typeof getOwnshipHeadingDeg === 'function')
        ? getOwnshipHeadingDeg
        : function () { return NaN; };
      const ownshipHeadingDeg = ownshipHeadingResolver(model, ownshipPoint);
      const activeJtacAsset = activeJtacTarget
        ? {
          callsign: String(activeJtacTarget.callsign || 'JTAC TGT').trim(),
          name: String(activeJtacTarget.name || activeJtacTarget.unitType || 'Target').trim(),
          category: 'JTAC_TARGET',
          typeName: String(activeJtacTarget.unitType || activeJtacTarget.typeName || '').trim(),
          frequency: '',
          altFrequencies: [],
          tacan: '',
          mpClientCallsign: '',
          altFeet: isFinite(Number(activeJtacTarget.elevationFeet)) ? Math.round(Number(activeJtacTarget.elevationFeet)) : 0,
          xNum: Number(activeJtacTarget.xNum),
          yNum: Number(activeJtacTarget.yNum),
          mgrsText: String(activeJtacTarget.mgrsText || '').toUpperCase().trim(),
          elevationFeet: isFinite(Number(activeJtacTarget.elevationFeet)) ? Math.round(Number(activeJtacTarget.elevationFeet)) : NaN,
          friendlyBearing: Number(activeJtacTarget.friendlyBearing),
          friendlyRangeNm: Number(activeJtacTarget.friendlyRangeNm),
          nl4Text: String(activeJtacTarget.nl4Text || '').trim(),
          nl5Text: String(activeJtacTarget.nl5Text || '').trim(),
          nl6Text: String(activeJtacTarget.nl6Text || '').trim(),
          nl7Text: String(activeJtacTarget.nl7Text || '').trim(),
          nl8Text: String(activeJtacTarget.nl8Text || '').trim(),
          nl9Text: String(activeJtacTarget.nl9Text || '').trim(),
          rmkText: String(activeJtacTarget.rmkText || '').trim(),
          engagementType: String(activeJtacTarget.engagementType || '').trim(),
        }
        : null;
      const historyJtacAssets = historyJtacTargets
        .map(function (h) {
          const xNum = Number(h && h.xNum);
          const yNum = Number(h && h.yNum);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          const seq = Math.max(1, Math.round(Number(h && h.seq) || 1));
          return {
            callsign: 'JTAC TGT',
            name: 'TGT-' + String(seq),
            category: 'JTAC_TARGET',
            typeName: String((h && h.unitType) || '').trim(),
            frequency: '',
            altFrequencies: [],
            tacan: '',
            mpClientCallsign: '',
            altFeet: isFinite(Number(h && h.elevationFeet)) ? Math.round(Number(h && h.elevationFeet)) : 0,
            xNum: xNum,
            yNum: yNum,
            mgrsText: String((h && h.mgrsText) || '').toUpperCase().trim(),
            elevationFeet: Number(h && h.elevationFeet),
            friendlyBearing: Number(h && h.friendlyBearing),
            friendlyRangeNm: Number(h && h.friendlyRangeNm),
            attackHeadingStartDeg: Number(h && h.attackHeadingStartDeg),
            attackHeadingEndDeg: Number(h && h.attackHeadingEndDeg),
            egressHeadingDeg: Number(h && h.egressHeadingDeg),
            nl4Text: String((h && h.nl4Text) || '').trim(),
            nl5Text: String((h && h.nl5Text) || '').trim(),
            nl6Text: String((h && h.nl6Text) || '').trim(),
            nl7Text: String((h && h.nl7Text) || '').trim(),
            nl8Text: String((h && h.nl8Text) || '').trim(),
            nl9Text: String((h && h.nl9Text) || '').trim(),
            rmkText: String((h && h.rmkText) || '').trim(),
            engagementType: String((h && h.engagementType) || '').trim(),
            isJtacHistory: true,
          };
        })
        .filter(function (x) { return !!x; });

      function mergePreferActiveJtac(assetList, active, historyList) {
        const out = Array.isArray(assetList) ? assetList.slice() : [];
        const activeTarget = active && typeof active === 'object' ? active : null;
        const history = Array.isArray(historyList) ? historyList : [];
        if (!activeTarget) return out;

        function sameJtacPosition(a, b) {
          const ax = Number(a && a.xNum);
          const ay = Number(a && a.yNum);
          const bx = Number(b && b.xNum);
          const by = Number(b && b.yNum);
          if (!isFinite(ax) || !isFinite(ay) || !isFinite(bx) || !isFinite(by)) return false;
          return Math.abs(ax - bx) <= 10 && Math.abs(ay - by) <= 10;
        }

        const activeKey = makeMapAssetSelectionKey(activeTarget);
        return out.map(function (a) {
          const item = a && typeof a === 'object' ? a : null;
          if (!item) return item;
          if (String((item.category || '')).toUpperCase() !== 'JTAC_TARGET') return item;
          if (!item.isJtacHistory) return item;

          const hasNineLine = !!String(item.nl4Text || item.nl5Text || item.nl6Text || item.nl7Text || item.nl8Text || item.nl9Text || item.rmkText || '').trim();
          if (hasNineLine) return item;

          const overlapActive = sameJtacPosition(item, activeTarget);
          if (!overlapActive) return item;

          const historyMatch = history.find(function (h) { return sameJtacPosition(h, item); }) || {};
          const seq = Math.max(1, Math.round(Number(historyMatch.seq) || 1));
          return Object.assign({}, activeTarget, {
            name: 'TGT-' + String(seq),
            isJtacHistory: true,
            historyPopupFallback: true,
            popupKey: activeKey,
          });
        });
      }

      rawAssets.concat(activeJtacAsset ? [activeJtacAsset] : []).concat(historyJtacAssets).forEach(function (asset) {
        const category = String((asset && asset.category) || '').toUpperCase();
        if (category === 'ATC') return;
        const isPlayer = category === 'PLAYER';
        const plottedAsset = (isPlayer && ownshipPoint && isFinite(Number(ownshipPoint.xNum)) && isFinite(Number(ownshipPoint.yNum)))
          ? Object.assign({}, asset || {}, { xNum: Number(ownshipPoint.xNum), yNum: Number(ownshipPoint.yNum) })
          : asset;
        const callsign = String((asset && asset.callsign) || '').trim();
        const mpClientCallsign = String((asset && asset.mpClientCallsign) || '').trim();
        const label = isPlayer
          ? (mpClientCallsign || callsign || String((asset && asset.name) || 'OWNSHIP').trim())
          : (callsign || String((asset && asset.name) || category || 'ASSET').trim());
        const assetKey = makeMapAssetSelectionKey(asset);
        const isJtacHistory = !!(asset && asset.isJtacHistory);
        addPointFeature(plottedAsset, {
          kind: isPlayer ? 'player' : 'asset',
          group: isPlayer ? 'player' : 'asset',
          label: (isJtacHistory ? String((asset && asset.name) || label) : label),
          fill: isPlayer ? '#d79cff' : (isJtacHistory ? '#1a1a1a' : '#5a7ea5'),
          stroke: isPlayer ? '#8a2f99' : (isJtacHistory ? '#000000' : '#385676'),
          textColor: isPlayer ? '#8a2f99' : (isJtacHistory ? '#000000' : '#1f3550'),
          radius: isPlayer ? 5 : 3.6,
          headingDeg: isPlayer ? ownshipHeadingDeg : NaN,
          assetKind: getMudMapAssetKind(asset),
          assetKey: assetKey,
          callsign: String((asset && asset.callsign) || '').trim(),
          name: String((asset && asset.name) || '').trim(),
          category: category,
          typeName: String((asset && asset.typeName) || '').trim(),
          frequency: String((asset && asset.frequency) || '').trim(),
          tacan: String((asset && asset.tacan) || '').trim(),
          mpClientCallsign: mpClientCallsign,
          altFeet: Number(asset && asset.altFeet),
        });
      });

      const missionDrawingObjects = getMissionDrawingObjectsForSaMap(model);
      let missionDrawingRenderedFeatures = 0;
      function parseMissionDrawingColor(colorString, fallbackRgba, forceVisible) {
        const s = String(colorString || '').trim();
        const m = s.match(/^0x([0-9a-fA-F]{8})$/);
        if (!m) return fallbackRgba;
        const n = parseInt(m[1], 16);
        if (!isFinite(n)) return fallbackRgba;
        const aabbggrr = {
          r: (n & 255),
          g: ((n >>> 8) & 255),
          b: ((n >>> 16) & 255),
          a: (((n >>> 24) & 255) / 255),
        };
        const rrggbbaa = {
          r: ((n >>> 24) & 255),
          g: ((n >>> 16) & 255),
          b: ((n >>> 8) & 255),
          a: ((n & 255) / 255),
        };
        let pick = rrggbbaa;
        if (pick.a <= 0.02 && aabbggrr.a > 0.02) pick = aabbggrr;
        const alpha = forceVisible
          ? Math.max(0.32, Math.min(1, Number(pick.a)))
          : Math.max(0, Math.min(1, Number(pick.a)));
        return 'rgba(' + Number(pick.r) + ',' + Number(pick.g) + ',' + Number(pick.b) + ',' + alpha.toFixed(3) + ')';
      }
      function missionAbsPoint(obj, point) {
        const mapX = Number(obj && obj.mapX);
        const mapY = Number(obj && obj.mapY);
        const px = Number(point && point.x);
        const py = Number(point && point.y);
        if (!isFinite(mapX) || !isFinite(mapY) || !isFinite(px) || !isFinite(py)) return null;
        return { xNum: mapX + px, yNum: mapY + py };
      }
      (Array.isArray(missionDrawingObjects) ? missionDrawingObjects : []).forEach(function (obj) {
        if (!obj || typeof obj !== 'object') return;
        const primitiveType = String(obj.primitiveType || '').trim().toUpperCase();
        const mapX = Number(obj.mapX);
        const mapY = Number(obj.mapY);
        if (!isFinite(mapX) || !isFinite(mapY)) return;

        const strokeColor = parseMissionDrawingColor(obj.colorString, 'rgba(31,54,81,0.85)', true);
        const fillColor = parseMissionDrawingColor(obj.fillColorString, 'rgba(0,0,0,0)', false);
        const lineWidth = Math.max(0.8, (Number(obj.thickness) || Number(obj.borderThickness) || 1.6) * 0.72);
        const style = String(obj.style || '').toLowerCase();
        const dashed = (style.indexOf('dot') >= 0 || style.indexOf('dash') >= 0);

        if (primitiveType === 'TEXTBOX') {
          const text = String(obj.text || '').trim();
          if (!text) return;
          const textLines = String(text || '')
            .replace(/\r/g, '')
            .split('\n')
            .map(function (line) { return String(line || '').trim(); })
            .filter(function (line) { return !!line; });
          const labelText = textLines.length
            ? textLines.join(' | ')
            : String(text || '').replace(/\s+/g, ' ').trim();
          if (!labelText) return;
          addPointFeature({ xNum: mapX, yNum: mapY }, {
            kind: 'mission-drawing-text',
            group: 'overlay',
            label: labelText,
            fill: strokeColor,
            stroke: strokeColor,
            textColor: strokeColor,
            fontSize: 18.5,
            radius: 1.2,
          });
          missionDrawingRenderedFeatures += 1;
          return;
        }

        if (primitiveType === 'LINE') {
          const linePoints = (Array.isArray(obj.points) ? obj.points : [])
            .map(function (p) { return missionAbsPoint(obj, p); })
            .filter(function (p) { return !!p; });
          if (linePoints.length < 2) return;
          addLineFeature(linePoints, {
            kind: 'mission-drawing-line',
            group: 'overlay',
            stroke: strokeColor,
            lineWidth: lineWidth,
            dashed: dashed,
          });
          missionDrawingRenderedFeatures += 1;
          return;
        }

        if (primitiveType === 'POLYGON') {
          const polygonMode = String(obj.polygonMode || '').toLowerCase();
          if (polygonMode === 'circle') {
            const radiusMeters = Number(obj.radius);
            if (!isFinite(radiusMeters) || radiusMeters <= 0) return;
            const ringPoints = [];
            const steps = 40;
            for (let i = 0; i <= steps; i++) {
              const t = (i / steps) * Math.PI * 2;
              ringPoints.push({ xNum: mapX + (Math.cos(t) * radiusMeters), yNum: mapY + (Math.sin(t) * radiusMeters) });
            }
            addLineFeature(ringPoints, {
              kind: 'mission-drawing-line',
              group: 'overlay',
              stroke: strokeColor,
              lineWidth: lineWidth,
              dashed: dashed,
            });
            missionDrawingRenderedFeatures += 1;
            return;
          }
          if (polygonMode === 'rect') {
            const rectW = Number(obj.width);
            const rectH = Number(obj.height);
            if (!isFinite(rectW) || !isFinite(rectH) || rectW <= 0 || rectH <= 0) return;
            const halfW = rectW / 2;
            const halfH = rectH / 2;
            const rectPoints = [
              { xNum: mapX - halfW, yNum: mapY - halfH },
              { xNum: mapX + halfW, yNum: mapY - halfH },
              { xNum: mapX + halfW, yNum: mapY + halfH },
              { xNum: mapX - halfW, yNum: mapY + halfH },
              { xNum: mapX - halfW, yNum: mapY - halfH },
            ];
            addLineFeature(rectPoints, {
              kind: 'mission-drawing-line',
              group: 'overlay',
              stroke: strokeColor,
              lineWidth: lineWidth,
              dashed: dashed,
            });
            missionDrawingRenderedFeatures += 1;
            return;
          }
          const polyPoints = (Array.isArray(obj.points) ? obj.points : [])
            .map(function (p) { return missionAbsPoint(obj, p); })
            .filter(function (p) { return !!p; });
          if (polyPoints.length < 2) return;
          const linePts = polyPoints.slice();
          if (polyPoints.length >= 3) {
            const first = polyPoints[0];
            const last = polyPoints[polyPoints.length - 1];
            if (Math.abs(Number(first.xNum) - Number(last.xNum)) > 1 || Math.abs(Number(first.yNum) - Number(last.yNum)) > 1) {
              linePts.push(first);
            }
          }
          addLineFeature(linePts, {
            kind: 'mission-drawing-line',
            group: 'overlay',
            stroke: strokeColor,
            lineWidth: lineWidth,
            dashed: dashed,
          });
          if (polyPoints.length >= 3) {
            const polygonCoords = linePts
              .map(function (p) { return toLonLat(p); })
              .filter(function (c) { return Array.isArray(c) && c.length === 2; });
            if (polygonCoords.length >= 4) {
              polygonCoords.forEach(function (c) { pointsForBounds.push(c); });
              features.push({
                type: 'Feature',
                geometry: {
                  type: 'Polygon',
                  coordinates: [polygonCoords],
                },
                properties: {
                  kind: 'mission-drawing-fill',
                  group: 'overlay',
                  fill: fillColor,
                  fillOpacity: 0.16,
                  stroke: strokeColor,
                  lineWidth: 0.9,
                },
              });
            }
          }
          missionDrawingRenderedFeatures += 1;
        }
      });

      if (!pointsForBounds.length) {
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

      const lons = pointsForBounds.map(function (c) { return Number(c[0]); }).filter(function (v) { return isFinite(v); });
      const lats = pointsForBounds.map(function (c) { return Number(c[1]); }).filter(function (v) { return isFinite(v); });
      if (!lons.length || !lats.length) {
        const centerOnly = getTheatreCenterLonLat(theatre);
        if (!centerOnly) {
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

    function buildMudMapSvg(waypoints, data, overlays, bullseyePoint, historySelection) {
      const rows = Array.isArray(waypoints) ? waypoints.filter(function (wp) {
        return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum));
      }) : [];
      const allowNavlog = (efbSaShowNavlog !== false);
      const allowDtcOverlay = (efbSaShowDtcOverlay !== false);
      const allowAirports = (efbSaShowAirports !== false);
      const allowJtacTargets = (efbSaShowJtacTargets !== false);
      const mapOverlays = overlays && typeof overlays === 'object'
        ? {
          geolines: allowDtcOverlay ? (Array.isArray(overlays.geolines) ? overlays.geolines : []) : [],
          threatPoints: allowDtcOverlay ? (Array.isArray(overlays.threatPoints) ? overlays.threatPoints : []) : [],
          destinationPoints: allowDtcOverlay ? (Array.isArray(overlays.destinationPoints) ? overlays.destinationPoints : []) : [],
          faorLines: allowDtcOverlay ? (Array.isArray(overlays.faorLines) ? overlays.faorLines : []) : [],
          flotLines: allowDtcOverlay ? (Array.isArray(overlays.flotLines) ? overlays.flotLines : []) : [],
          capPoints: allowDtcOverlay ? (Array.isArray(overlays.capPoints) ? overlays.capPoints : []) : [],
          corridors: allowDtcOverlay ? (Array.isArray(overlays.corridors) ? overlays.corridors : []) : [],
          areaPolygons: allowDtcOverlay ? (Array.isArray(overlays.areaPolygons) ? overlays.areaPolygons : []) : [],
          airfields: allowAirports ? (Array.isArray(overlays.airfields) ? overlays.airfields : []) : [],
        }
        : { geolines: [], threatPoints: [], destinationPoints: [], faorLines: [], flotLines: [], capPoints: [], corridors: [], areaPolygons: [] };
      const jtacTargets = allowJtacTargets ? parseJtacNineLineTargets(data || latestData || {}) : [];
      const jtacOverlay = allowJtacTargets
        ? resolveJtacOverlayTargets(data || latestData || {}, jtacTargets)
        : { active: null, history: [] };
      const activeJtacTarget = jtacOverlay && jtacOverlay.active ? jtacOverlay.active : null;
      const historyJtacTargets = jtacOverlay && Array.isArray(jtacOverlay.history) ? jtacOverlay.history : [];
      const jtacPointsForMap = (activeJtacTarget ? [activeJtacTarget] : []).concat(historyJtacTargets);
      const historyTrackSelection = getFlightPlanEtaStartKey(historySelection || '')
        || getFlightPlanEtaStartKey(getActiveEfbSaSelectionKey())
        || getFlightPlanEtaStartKey(getActiveFlightPlanSelection(data || latestData || {}));
      const historyTrackRowsForMap = (efbSaShowHistoryTrack === true)
        ? getEfbSaHistoryTrackByKey(historyTrackSelection)
        : [];
      const savedHistoryTracksForMap = (efbSaShowHistoryTrack === true)
        ? getEfbSaSavedHistoryTracksBySelection(historyTrackSelection)
          .filter(function (row) { return row && row.visible !== false && Array.isArray(row.points) && row.points.length; })
        : [];
      const savedHistoryTrackPointsForMap = savedHistoryTracksForMap.reduce(function (acc, row) {
        return acc.concat(Array.isArray(row.points) ? row.points : []);
      }, []);
      const visibleRows = allowNavlog ? rows : [];
      const geolines = Array.isArray(mapOverlays.geolines) ? mapOverlays.geolines : [];
      const threatPoints = Array.isArray(mapOverlays.threatPoints) ? mapOverlays.threatPoints : [];
      const destinationPoints = Array.isArray(mapOverlays.destinationPoints) ? mapOverlays.destinationPoints : [];
      const airfields = Array.isArray(mapOverlays.airfields) ? mapOverlays.airfields : [];
      const faorLines = Array.isArray(mapOverlays.faorLines) ? mapOverlays.faorLines : [];
      const flotLines = Array.isArray(mapOverlays.flotLines) ? mapOverlays.flotLines : [];
      const capPoints = Array.isArray(mapOverlays.capPoints) ? mapOverlays.capPoints : [];
      const corridors = Array.isArray(mapOverlays.corridors) ? mapOverlays.corridors : [];
      const areaPolygons = Array.isArray(mapOverlays.areaPolygons) ? mapOverlays.areaPolygons : [];
      const missionDrawingObjects = getMissionDrawingObjectsForSaMap(data || latestData || {});

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

      if (!visibleRows.length && !threatPoints.length && !destinationPoints.length && !geolines.length && !faorLines.length && !flotLines.length && !capPoints.length && !corridors.length && !airfields.length && !jtacPointsForMap.length && !historyTrackRowsForMap.length && !savedHistoryTrackPointsForMap.length && !missionDrawingObjects.length) {
        return '<div class="fltPlanMessage">No mappable waypoint coordinates found.</div>';
      }

      const width = 920;
      const height = 760;
      const pad = 54;
      const dtcOverlayScale = 1.35;

      const overlayPoints = [];
      geolines.forEach(function (p) { overlayPoints.push(p); });
      threatPoints.forEach(function (p) { overlayPoints.push(p); });
      destinationPoints.forEach(function (p) { overlayPoints.push(p); });
      capPoints.forEach(function (p) { overlayPoints.push(p); });
      airfields.forEach(function (p) { overlayPoints.push(p); });
      if (activeJtacTarget) overlayPoints.push(activeJtacTarget);
      historyJtacTargets.forEach(function (p) { overlayPoints.push(p); });
      historyTrackRowsForMap.forEach(function (p) { overlayPoints.push(p); });
      if (bullseye) overlayPoints.push(bullseye);
      function pushLineGroupPoints(lineGroups) {
        (Array.isArray(lineGroups) ? lineGroups : []).forEach(function (group) {
          (Array.isArray(group && group.points) ? group.points : []).forEach(function (p) { overlayPoints.push(p); });
        });
      }
      pushLineGroupPoints(faorLines);
      pushLineGroupPoints(flotLines);
      pushLineGroupPoints(corridors);
      pushLineGroupPoints(areaPolygons);
      missionDrawingObjects.forEach(function (obj) {
        if (!obj || typeof obj !== 'object') return;
        const mapX = Number(obj.mapX);
        const mapY = Number(obj.mapY);
        if (isFinite(mapX) && isFinite(mapY)) {
          overlayPoints.push({ xNum: mapX, yNum: mapY });
        }

        const points = Array.isArray(obj.points) ? obj.points : [];
        points.forEach(function (p) {
          const px = Number(p && p.x);
          const py = Number(p && p.y);
          if (!isFinite(px) || !isFinite(py) || !isFinite(mapX) || !isFinite(mapY)) return;
          overlayPoints.push({ xNum: mapX + px, yNum: mapY + py });
        });
      });

      function renderAreaPolygons(polygons) {
        const els = [];
        (Array.isArray(polygons) ? polygons : []).forEach(function (poly) {
          const pts = (Array.isArray(poly && poly.points) ? poly.points : [])
            .map(function (p) {
              return {
                xNum: Number(p && p.xNum),
                yNum: Number(p && p.yNum),
                number: Number(p && p.number)
              };
            })
            .filter(function (p) { return isFinite(p.xNum) && isFinite(p.yNum); })
            .sort(function (a, b) {
              if (isFinite(a.number) && isFinite(b.number) && a.number !== b.number) return a.number - b.number;
              return 0;
            });
          if (pts.length < 3) return;

          const subtype = String((poly && poly.subtype) || '').toLowerCase();
          const stroke = (subtype === 'nfz') ? '#d96a6a'
            : (subtype === 'pfz' ? '#d9a64f' : '#9f7ad0');
          const fill = (subtype === 'nfz') ? 'rgba(217,106,106,0.18)'
            : (subtype === 'pfz' ? 'rgba(217,166,79,0.16)' : 'rgba(159,122,208,0.12)');
          const pointsAttr = pts
            .map(function (p) {
              const m = mapPt(p);
              return m.x.toFixed(1) + ',' + m.y.toFixed(1);
            })
            .join(' ');
          if (!pointsAttr) return;

          els.push('<polygon points="' + pointsAttr + '" fill="' + fill + '" stroke="' + stroke + '" stroke-width="' + (1.8 * dtcOverlayScale).toFixed(1) + '" stroke-dasharray="7 5" />');

          const label = escapeHtml(String((poly && poly.label) || '').trim());
          if (label) {
            const first = mapPt(pts[0]);
            els.push('<text x="' + (first.x + 7).toFixed(1) + '" y="' + (first.y - 6).toFixed(1) + '" font-size="18" fill="' + stroke + '" font-weight="700">' + label + '</text>');
          }
        });
        return els;
      }

      const allPoints = visibleRows.concat(overlayPoints);
      const eastValues = allPoints.map(function (wp) { return Number(wp.yNum); }).filter(function (v) { return isFinite(v); });
      const northValues = allPoints.map(function (wp) { return Number(wp.xNum); }).filter(function (v) { return isFinite(v); });
      if (!eastValues.length || !northValues.length) {
        return '<div class="fltPlanMessage">No mappable waypoint coordinates found.</div>';
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

      function mapPt(wp) {
        const north = Number(wp.xNum);
        const east = Number(wp.yNum);
        const sx = offsetX + ((east - minEast) * scale);
        const sy = offsetY + ((maxNorth - north) * scale);
        return { x: sx, y: sy };
      }

      function parseDcsColor(colorString, fallbackRgba, forceVisible) {
        const s = String(colorString || '').trim();
        const m = s.match(/^0x([0-9a-fA-F]{8})$/);
        if (!m) return fallbackRgba;
        const n = parseInt(m[1], 16);
        if (!isFinite(n)) return fallbackRgba;

        const rgbaAabbggrr = {
          r: (n & 255),
          g: ((n >>> 8) & 255),
          b: ((n >>> 16) & 255),
          a: (((n >>> 24) & 255) / 255),
        };

        const rgbaRrggbbaa = {
          r: ((n >>> 24) & 255),
          g: ((n >>> 16) & 255),
          b: ((n >>> 8) & 255),
          a: ((n & 255) / 255),
        };

        let pick = rgbaRrggbbaa;
        if (pick.a <= 0.02 && rgbaAabbggrr.a > 0.02) {
          pick = rgbaAabbggrr;
        }

        const alpha = forceVisible
          ? Math.max(0.32, Math.min(1, Number(pick.a)))
          : Math.max(0, Math.min(1, Number(pick.a)));

        return 'rgba(' + Number(pick.r) + ',' + Number(pick.g) + ',' + Number(pick.b) + ',' + alpha.toFixed(3) + ')';
      }

      function toMapAbsPoint(obj, point) {
        const mapX = Number(obj && obj.mapX);
        const mapY = Number(obj && obj.mapY);
        const px = Number(point && point.x);
        const py = Number(point && point.y);
        if (!isFinite(mapX) || !isFinite(mapY) || !isFinite(px) || !isFinite(py)) return null;
        return { xNum: mapX + px, yNum: mapY + py };
      }

      function toSvgPointString(rows) {
        return (Array.isArray(rows) ? rows : [])
          .filter(function (m) { return !!m && isFinite(Number(m.x)) && isFinite(Number(m.y)); })
          .map(function (m) { return Number(m.x).toFixed(1) + ',' + Number(m.y).toFixed(1); })
          .join(' ');
      }

      const missionDrawingEls = missionDrawingObjects.map(function (obj) {
        if (!obj || typeof obj !== 'object') return '';
        const primitiveType = String(obj.primitiveType || '').trim().toUpperCase();
        const mapX = Number(obj.mapX);
        const mapY = Number(obj.mapY);
        if (!isFinite(mapX) || !isFinite(mapY)) return '';
        const anchor = mapPt({ xNum: mapX, yNum: mapY });
        const strokeColor = parseDcsColor(obj.colorString, isNight ? 'rgba(199,219,255,0.850)' : 'rgba(31,54,81,0.850)', true);
        const fillColor = parseDcsColor(obj.fillColorString, 'rgba(0,0,0,0)');
        const lineWidth = Math.max(0.8, (Number(obj.thickness) || Number(obj.borderThickness) || 1.6) * 0.72);
        const style = String(obj.style || '').toLowerCase();
        const dash = (style.indexOf('dot') >= 0 || style.indexOf('dash') >= 0) ? ' stroke-dasharray="6 5"' : '';
        const rotate = isFinite(Number(obj.angle))
          ? ' transform="rotate(' + Number(obj.angle).toFixed(1) + ' ' + anchor.x.toFixed(1) + ' ' + anchor.y.toFixed(1) + ')"'
          : '';

        if (primitiveType === 'TEXTBOX') {
          const text = String(obj.text || '').trim();
          if (!text) return '';
          const fontSize = 18.5;
          const textLines = text.split(/\r?\n/).filter(function (line) { return String(line || '').length > 0; });
          const rows = (textLines.length ? textLines : [text]).map(function (line, idx) {
            const dy = idx === 0 ? '0' : '1.2em';
            return '<tspan x="' + anchor.x.toFixed(1) + '" dy="' + dy + '">' + escapeHtml(String(line)) + '</tspan>';
          }).join('');
          return '<text x="' + anchor.x.toFixed(1) + '" y="' + anchor.y.toFixed(1) + '" fill="' + strokeColor + '" font-size="' + fontSize.toFixed(1) + '" font-weight="700"' + rotate + '>' + rows + '</text>';
        }

        if (primitiveType === 'LINE') {
          const absPoints = (Array.isArray(obj.points) ? obj.points : [])
            .map(function (p) { return toMapAbsPoint(obj, p); })
            .filter(function (p) { return !!p; })
            .map(function (p) { return mapPt(p); });
          if (absPoints.length < 2) return '';
          const pointsAttr = toSvgPointString(absPoints);
          return '<polyline points="' + pointsAttr + '" fill="none" stroke="' + strokeColor + '" stroke-width="' + lineWidth.toFixed(1) + '"' + dash + (obj.closed ? ' data-closed="1"' : '') + ' />';
        }

        if (primitiveType === 'POLYGON') {
          const polygonMode = String(obj.polygonMode || '').toLowerCase();
          if (polygonMode === 'circle') {
            const radiusMeters = Number(obj.radius);
            if (!isFinite(radiusMeters) || radiusMeters <= 0) return '';
            const radiusPx = Math.max(2, radiusMeters * scale);
            return '<circle cx="' + anchor.x.toFixed(1) + '" cy="' + anchor.y.toFixed(1) + '" r="' + radiusPx.toFixed(1) + '" fill="' + fillColor + '" stroke="' + strokeColor + '" stroke-width="' + lineWidth.toFixed(1) + '"' + dash + ' />';
          }
          if (polygonMode === 'rect') {
            const widthPx = Math.max(2, (Number(obj.width) || 0) * scale);
            const heightPx = Math.max(2, (Number(obj.height) || 0) * scale);
            if (!isFinite(widthPx) || !isFinite(heightPx)) return '';
            return '<rect x="' + (anchor.x - (widthPx / 2)).toFixed(1) + '" y="' + (anchor.y - (heightPx / 2)).toFixed(1) + '" width="' + widthPx.toFixed(1) + '" height="' + heightPx.toFixed(1) + '" fill="' + fillColor + '" stroke="' + strokeColor + '" stroke-width="' + lineWidth.toFixed(1) + '"' + dash + rotate + ' />';
          }

          const absPoints = (Array.isArray(obj.points) ? obj.points : [])
            .map(function (p) { return toMapAbsPoint(obj, p); })
            .filter(function (p) { return !!p; })
            .map(function (p) { return mapPt(p); });
          if (absPoints.length < 2) return '';
          const pointsAttr = toSvgPointString(absPoints);
          if (absPoints.length >= 3) {
            return '<polygon points="' + pointsAttr + '" fill="' + fillColor + '" stroke="' + strokeColor + '" stroke-width="' + lineWidth.toFixed(1) + '"' + dash + ' />';
          }
          return '<polyline points="' + pointsAttr + '" fill="none" stroke="' + strokeColor + '" stroke-width="' + lineWidth.toFixed(1) + '"' + dash + ' />';
        }

        return '';
      }).filter(function (x) { return !!x; });

      const mapped = visibleRows.map(function (wp) {
        const p = mapPt(wp);
        return { wp: wp, x: p.x, y: p.y, kind: getMudMapPointType(wp) };
      });

      const byStep = {};
      mapped.forEach(function (m) { byStep[String(m.wp.step)] = m; });
      const segments = getMudMapSegments(visibleRows)
        .map(function (seg) {
          return {
            from: byStep[String(seg.from.step)],
            to: byStep[String(seg.to.step)],
            dashed: !!seg.dashed
          };
        })
        .filter(function (seg) { return !!(seg.from && seg.to); });

      const lineEls = segments.map(function (seg) {
        return '<line x1="' + seg.from.x.toFixed(1) + '" y1="' + seg.from.y.toFixed(1) + '" x2="' + seg.to.x.toFixed(1) + '" y2="' + seg.to.y.toFixed(1) + '" stroke="' + palette.line + '" stroke-width="' + (2 * dtcOverlayScale).toFixed(1) + '"' + (seg.dashed ? ' stroke-dasharray="8 6"' : '') + ' />';
      });
      const historyLineEls = [];
      const historyPointEls = historyTrackRowsForMap
        .map(function (p) {
          const m = mapPt(p);
          if (!m || !isFinite(Number(m.x)) || !isFinite(Number(m.y))) return '';
          return '<circle cx="' + m.x.toFixed(1) + '" cy="' + m.y.toFixed(1) + '" r="2.2" fill="#000000" />';
        })
        .filter(function (x) { return !!x; });
      const savedHistoryPointEls = [];
      savedHistoryTracksForMap.forEach(function (row, idx) {
        const shade = Math.max(80, 165 - (idx * 16));
        const color = 'rgb(' + String(shade) + ',' + String(shade) + ',' + String(shade) + ')';
        (Array.isArray(row && row.points) ? row.points : []).forEach(function (p) {
          const m = mapPt(p);
          if (!m || !isFinite(Number(m.x)) || !isFinite(Number(m.y))) return;
          savedHistoryPointEls.push('<circle cx="' + m.x.toFixed(1) + '" cy="' + m.y.toFixed(1) + '" r="2.0" fill="' + color + '" />');
        });
      });

      const geoMapped = geolines.map(function (p) {
        const m = mapPt(p);
        return { p: p, x: m.x, y: m.y };
      });
      const geoLineKeys = ['L1', 'L2', 'L3', 'L4'];
      const geoLineEls = [];
      geoLineKeys.forEach(function (lineKey) {
        const group = geoMapped
          .filter(function (m) {
            const flags = (m && m.p && Array.isArray(m.p.lineFlags)) ? m.p.lineFlags : [];
            return flags.indexOf(lineKey) >= 0;
          })
          .sort(function (a, b) {
            const an = Number(a && a.p && a.p.number);
            const bn = Number(b && b.p && b.p.number);
            if (isFinite(an) && isFinite(bn) && an !== bn) return an - bn;
            return 0;
          });
        for (let i = 1; i < group.length; i++) {
          const a = group[i - 1];
          const b = group[i];
          geoLineEls.push('<line x1="' + a.x.toFixed(1) + '" y1="' + a.y.toFixed(1) + '" x2="' + b.x.toFixed(1) + '" y2="' + b.y.toFixed(1) + '" stroke="' + palette.geoLine + '" stroke-width="' + (2.2 * dtcOverlayScale).toFixed(1) + '" stroke-dasharray="5 4" />');
        }
      });
      const geoPointEls = geoMapped.map(function (m) {
        return '<circle cx="' + m.x.toFixed(1) + '" cy="' + m.y.toFixed(1) + '" r="' + (3.8 * dtcOverlayScale).toFixed(1) + '" fill="' + palette.geoLine + '" />';
      });

      const jtacSupportEls = [];
      if (activeJtacTarget) {
        const t = activeJtacTarget;
        const target = t || {};
        [
          { innerNm: 1, outerNm: 5, fillOpacity: 0.18 },
          { innerNm: 5, outerNm: 10, fillOpacity: 0.12 },
          { innerNm: 10, outerNm: 15, fillOpacity: 0.07 },
        ].forEach(function (band) {
          const reciprocalSector = getJtacReciprocalSectorPolygonPoints(target, band.innerNm, band.outerNm);
          if (!reciprocalSector || !Array.isArray(reciprocalSector.points) || reciprocalSector.points.length < 6) return;
          const polyPoints = reciprocalSector.points
            .map(function (p) {
              const mp = mapPt(p);
              return mp.x.toFixed(1) + ',' + mp.y.toFixed(1);
            })
            .join(' ');
          if (!polyPoints) return;
          jtacSupportEls.push('<polygon points="' + polyPoints + '" fill="#4faa4f" fill-opacity="' + String(band.fillOpacity) + '" stroke="#2f8a2f" stroke-width="1.4" />');
        });

        const friendlyPoint = getJtacFriendlyPointFromTarget(target);
        if (friendlyPoint && isFinite(Number(friendlyPoint.xNum)) && isFinite(Number(friendlyPoint.yNum))) {
          const fp = mapPt(friendlyPoint);
          const tp = mapPt(target);
          jtacSupportEls.push('<line x1="' + tp.x.toFixed(1) + '" y1="' + tp.y.toFixed(1) + '" x2="' + fp.x.toFixed(1) + '" y2="' + fp.y.toFixed(1) + '" stroke="#3a7ec1" stroke-width="1.7" stroke-dasharray="6 4" />');
          jtacSupportEls.push('<g>'
            + '<circle cx="' + fp.x.toFixed(1) + '" cy="' + fp.y.toFixed(1) + '" r="6.2" fill="none" stroke="#1f5d93" stroke-width="1.4" />'
            + '<line x1="' + (fp.x - 5.2).toFixed(1) + '" y1="' + fp.y.toFixed(1) + '" x2="' + (fp.x + 5.2).toFixed(1) + '" y2="' + fp.y.toFixed(1) + '" stroke="#1f5d93" stroke-width="1.3" />'
            + '<line x1="' + fp.x.toFixed(1) + '" y1="' + (fp.y - 5.2).toFixed(1) + '" x2="' + fp.x.toFixed(1) + '" y2="' + (fp.y + 5.2).toFixed(1) + '" stroke="#1f5d93" stroke-width="1.3" />'
            + '</g>');
          const frLabel = isFinite(Number(target.friendlyDistanceMeters))
            ? ('FRND ' + String(Math.round(Number(target.friendlyDistanceMeters))) + 'm')
            : (isFinite(Number(target.friendlyRangeNm)) ? ('FRND ' + String(Number(target.friendlyRangeNm).toFixed(1)) + 'NM') : 'FRND');
          jtacSupportEls.push('<text x="' + (fp.x + 9).toFixed(1) + '" y="' + (fp.y + 4).toFixed(1) + '" font-size="10" fill="#1f5d93" font-weight="700">' + escapeHtml(frLabel) + '</text>');
        }

        const egrPoint = getJtacEgressPointFromTarget(target, 5);
        if (egrPoint && isFinite(Number(egrPoint.xNum)) && isFinite(Number(egrPoint.yNum))) {
          const ep = mapPt(egrPoint);
          const tp = mapPt(target);
          jtacSupportEls.push('<line x1="' + tp.x.toFixed(1) + '" y1="' + tp.y.toFixed(1) + '" x2="' + ep.x.toFixed(1) + '" y2="' + ep.y.toFixed(1) + '" stroke="#7f2e2e" stroke-width="2.0" />');
          jtacSupportEls.push('<polygon points="' + ep.x.toFixed(1) + ',' + (ep.y - 4.6).toFixed(1) + ' ' + (ep.x - 4.0).toFixed(1) + ',' + (ep.y + 3.8).toFixed(1) + ' ' + (ep.x + 4.0).toFixed(1) + ',' + (ep.y + 3.8).toFixed(1) + '" fill="#7f2e2e" stroke="#7f2e2e" stroke-width="1.1" />');
          jtacSupportEls.push('<text x="' + (ep.x + 8).toFixed(1) + '" y="' + (ep.y + 4).toFixed(1) + '" font-size="10" fill="#7f2e2e" font-weight="700">Egress</text>');
        }
      }

      function renderLineGroups(lineGroups, strokeColor, dashPattern, strokeWidth) {
        const els = [];
        (Array.isArray(lineGroups) ? lineGroups : []).forEach(function (group) {
          const mappedGroup = (Array.isArray(group && group.points) ? group.points : [])
            .map(function (p) {
              const m = mapPt(p);
              return { p: p, x: m.x, y: m.y };
            })
            .sort(function (a, b) {
              const an = Number(a && a.p && a.p.number);
              const bn = Number(b && b.p && b.p.number);
              if (isFinite(an) && isFinite(bn) && an !== bn) return an - bn;
              return 0;
            });
          for (let i = 1; i < mappedGroup.length; i++) {
            const a = mappedGroup[i - 1];
            const b = mappedGroup[i];
            const scaledStrokeWidth = (Number(strokeWidth) * dtcOverlayScale).toFixed(1);
            els.push('<line x1="' + a.x.toFixed(1) + '" y1="' + a.y.toFixed(1) + '" x2="' + b.x.toFixed(1) + '" y2="' + b.y.toFixed(1) + '" stroke="' + strokeColor + '" stroke-width="' + scaledStrokeWidth + '"' + (dashPattern ? (' stroke-dasharray="' + dashPattern + '"') : '') + ' />');
          }
        });
        return els;
      }

      function renderCorridorBounds(corridorGroups) {
        const els = [];
        const corridorHalfWidthMeters = 5000;
        (Array.isArray(corridorGroups) ? corridorGroups : []).forEach(function (group) {
          const pts = (Array.isArray(group && group.points) ? group.points : [])
            .map(function (p) {
              return {
                xNum: Number(p && p.xNum),
                yNum: Number(p && p.yNum),
                number: Number(p && p.number)
              };
            })
            .filter(function (p) { return isFinite(p.xNum) && isFinite(p.yNum); })
            .sort(function (a, b) {
              if (isFinite(a.number) && isFinite(b.number) && a.number !== b.number) return a.number - b.number;
              return 0;
            });
          if (pts.length < 2) return;

          const left = [];
          const right = [];
          for (let i = 0; i < pts.length; i++) {
            const prev = pts[Math.max(0, i - 1)];
            const next = pts[Math.min(pts.length - 1, i + 1)];
            const dNorth = Number(next.xNum) - Number(prev.xNum);
            const dEast = Number(next.yNum) - Number(prev.yNum);
            const len = Math.sqrt((dNorth * dNorth) + (dEast * dEast));
            if (!isFinite(len) || len <= 0) {
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

          function makePolyline(points) {
            const mappedPts = points.map(function (p) { return mapPt(p); });
            return mappedPts.map(function (p) { return p.x.toFixed(1) + ',' + p.y.toFixed(1); }).join(' ');
          }

          els.push('<polyline points="' + makePolyline(left) + '" fill="none" stroke="' + palette.corridorLine + '" stroke-width="' + (1.9 * dtcOverlayScale).toFixed(1) + '" stroke-dasharray="8 5" />');
          els.push('<polyline points="' + makePolyline(right) + '" fill="none" stroke="' + palette.corridorLine + '" stroke-width="' + (1.9 * dtcOverlayScale).toFixed(1) + '" stroke-dasharray="8 5" />');
        });
        return els;
      }

      const faorEls = renderLineGroups(faorLines, palette.faorLine, '6 4', '2.2');
      const flotEls = renderLineGroups(flotLines, palette.flotLine, '6 4', '2.2');
      const corridorEls = renderCorridorBounds(corridors);
      const areaPolygonEls = renderAreaPolygons(areaPolygons);

      const capEls = capPoints.map(function (cap) {
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
          + '<rect x="' + (centerX - (widthPx / 2)).toFixed(1) + '" y="' + (centerY - (heightPx / 2)).toFixed(1) + '" width="' + widthPx.toFixed(1) + '" height="' + heightPx.toFixed(1) + '" rx="' + radiusPx.toFixed(1) + '" ry="' + radiusPx.toFixed(1) + '" fill="none" stroke="' + palette.capLine + '" stroke-width="2.0" transform="rotate(' + angleDeg.toFixed(1) + ' ' + centerX.toFixed(1) + ' ' + centerY.toFixed(1) + ')" />'
          + '<circle cx="' + anchor.x.toFixed(1) + '" cy="' + anchor.y.toFixed(1) + '" r="2.9" fill="' + palette.capLine + '" />'
          + '<text x="' + tx + '" y="' + ty + '" font-size="10" fill="' + palette.capLine + '" font-weight="700">' + label + '</text>'
          + '</g>';
      });

      const threatMapped = threatPoints.map(function (p) {
        const m = mapPt(p);
        return {
          p: p,
          x: m.x,
          y: m.y,
          radiusPx: Math.max(4, (Number(p.radiusMeters) || 0) * scale)
        };
      });
      const threatEls = threatMapped.map(function (m) {
        const label = escapeHtml(String((m && m.p && m.p.label) || 'THR'));
        const ring = (m && m.p && m.p.ring && m.radiusPx > 0)
          ? ('<circle cx="' + m.x.toFixed(1) + '" cy="' + m.y.toFixed(1) + '" r="' + m.radiusPx.toFixed(1) + '" fill="none" stroke="' + palette.threatStroke + '" stroke-width="' + (1.4 * dtcOverlayScale).toFixed(1) + '" stroke-dasharray="7 5" />')
          : '';
        const cross = '<line x1="' + (m.x - 9.5).toFixed(1) + '" y1="' + m.y.toFixed(1) + '" x2="' + (m.x + 9.5).toFixed(1) + '" y2="' + m.y.toFixed(1) + '" stroke="' + palette.threatStroke + '" stroke-width="' + (1.6 * dtcOverlayScale).toFixed(1) + '" />'
          + '<line x1="' + m.x.toFixed(1) + '" y1="' + (m.y - 9.5).toFixed(1) + '" x2="' + m.x.toFixed(1) + '" y2="' + (m.y + 9.5).toFixed(1) + '" stroke="' + palette.threatStroke + '" stroke-width="' + (1.6 * dtcOverlayScale).toFixed(1) + '" />';
        const txt = '<text x="' + (m.x + 12).toFixed(1) + '" y="' + (m.y + 5).toFixed(1) + '" font-size="22" fill="' + palette.threatLabel + '" font-weight="700">' + label + '</text>';
        return '<g>' + ring + cross + txt + '</g>';
      });

      const destinationMapped = destinationPoints.map(function (p) {
        const m = mapPt(p);
        return { p: p, x: m.x, y: m.y };
      });
      const destinationEls = destinationMapped.map(function (m) {
        const label = escapeHtml(String((m && m.p && m.p.label) || 'DEST'));
        const subtype = String((m && m.p && m.p.subtype) || '').toLowerCase();
        if (subtype === 'lantirn') {
          return '<g>'
            + '<line x1="' + (m.x - 7.5).toFixed(1) + '" y1="' + m.y.toFixed(1) + '" x2="' + (m.x + 7.5).toFixed(1) + '" y2="' + m.y.toFixed(1) + '" stroke="' + palette.destStroke + '" stroke-width="' + (1.5 * dtcOverlayScale).toFixed(1) + '" />'
            + '<line x1="' + m.x.toFixed(1) + '" y1="' + (m.y - 7.5).toFixed(1) + '" x2="' + m.x.toFixed(1) + '" y2="' + (m.y + 7.5).toFixed(1) + '" stroke="' + palette.destStroke + '" stroke-width="' + (1.5 * dtcOverlayScale).toFixed(1) + '" />'
            + '<text x="' + (m.x + 12).toFixed(1) + '" y="' + (m.y + 5).toFixed(1) + '" font-size="22" fill="' + palette.destLabel + '" font-weight="700">' + label + '</text>'
            + '</g>';
        }
        const p1 = m.x.toFixed(1) + ',' + (m.y - 10).toFixed(1);
        const p2 = (m.x - 10).toFixed(1) + ',' + m.y.toFixed(1);
        const p3 = m.x.toFixed(1) + ',' + (m.y + 10).toFixed(1);
        const p4 = (m.x + 10).toFixed(1) + ',' + m.y.toFixed(1);
        return '<g><polygon points="' + p1 + ' ' + p2 + ' ' + p3 + ' ' + p4 + '" fill="' + palette.destFill + '" stroke="' + palette.destStroke + '" stroke-width="' + (1.5 * dtcOverlayScale).toFixed(1) + '" /><text x="' + (m.x + 12).toFixed(1) + '" y="' + (m.y + 5).toFixed(1) + '" font-size="22" fill="' + palette.destLabel + '" font-weight="700">' + label + '</text></g>';
      });

      const airfieldMapped = airfields
        .map(function (p) {
          const north = Number(p && p.xNum);
          const east = Number(p && p.yNum);
          if (!isFinite(north) || !isFinite(east)) return null;
          const m = mapPt({ xNum: north, yNum: east });
          return { p: p, x: m.x, y: m.y };
        })
        .filter(function (m) { return !!m; });
      const preselectedAssetKey = getMapSelectedAssetKeyBySelection(getActiveFlightPlanSelection((typeof data === 'undefined' ? null : data)));

      const airfieldEls = airfieldMapped.map(function (m) {
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
        if (isSelected) {
          const infoLines = buildSelectedAssetInfoLines({
            category: 'ATC',
            typeName: String((m && m.p && m.p.typeName) || '').trim(),
            frequency: String((m && m.p && m.p.frequency) || '').trim(),
            altFrequencies: Array.isArray(m && m.p && m.p.altFrequencies) ? m.p.altFrequencies : [],
            tacan: String((m && m.p && m.p.tacan) || '').trim(),
            mpClientCallsign: String((m && m.p && m.p.mpClientCallsign) || '').trim(),
          });
          if (infoLines.length) {
            const fontSize = 10;
            const lineHeight = 12;
            const padX = 5;
            const padY = 4;
            const maxChars = infoLines.reduce(function (max, line) { return Math.max(max, String(line || '').length); }, 0);
            const boxWidth = Math.max(120, Math.min(300, (maxChars * 6.2) + (padX * 2)));
            const boxHeight = (infoLines.length * lineHeight) + (padY * 2);
            let boxX = m.x + 12;
            let boxY = m.y + 8;
            if ((boxX + boxWidth) > (width - 4)) boxX = m.x - boxWidth - 12;
            if ((boxY + boxHeight) > (height - 4)) boxY = m.y - boxHeight - 12;
            const textRows = infoLines.map(function (line, idx) {
              const txRow = (boxX + padX).toFixed(1);
              const tyRow = (boxY + padY + (lineHeight * (idx + 1)) - 2).toFixed(1);
              return '<text x="' + txRow + '" y="' + tyRow + '" font-size="' + fontSize + '" fill="' + palette.assetInfoText + '" font-weight="700">' + escapeHtml(String(line)) + '</text>';
            }).join('');
            selectedInfoBlock = '<g>'
              + '<rect x="' + boxX.toFixed(1) + '" y="' + boxY.toFixed(1) + '" width="' + boxWidth.toFixed(1) + '" height="' + boxHeight.toFixed(1) + '" rx="3" ry="3" fill="' + palette.assetInfoBg + '" stroke="' + palette.assetInfoStroke + '" stroke-width="1.1" />'
              + textRows
              + '</g>';
          }
        }
        let icon = '<g data-map-asset-key="' + encodeURIComponent(selectionKey) + '" data-map-asset-category="ATC" style="cursor:pointer">'
          + (isSelected ? ('<circle cx="' + m.x.toFixed(1) + '" cy="' + m.y.toFixed(1) + '" r="11" fill="none" stroke="#c94444" stroke-width="2.4" />') : '')
          + '<circle cx="' + m.x.toFixed(1) + '" cy="' + m.y.toFixed(1) + '" r="' + r.toFixed(1) + '" fill="none" stroke="' + stroke + '" stroke-width="2" />';
        if (!military) {
          for (let i = 0; i < 6; i++) {
            const a = i * (Math.PI / 3.0);
            const x1 = m.x + (Math.cos(a) * (r + 0.5));
            const y1 = m.y + (Math.sin(a) * (r + 0.5));
            const x2 = m.x + (Math.cos(a) * (r + 2.2));
            const y2 = m.y + (Math.sin(a) * (r + 2.2));
            icon += '<line x1="' + x1.toFixed(1) + '" y1="' + y1.toFixed(1) + '" x2="' + x2.toFixed(1) + '" y2="' + y2.toFixed(1) + '" stroke="' + stroke + '" stroke-width="1.2" />';
          }
        }
        if (type === 'heliport') {
          icon += '<text x="' + m.x.toFixed(1) + '" y="' + (m.y + 3.8).toFixed(1) + '" text-anchor="middle" font-size="8.8" fill="' + stroke + '" font-weight="800">H</text>';
        } else if (type === 'seaplane') {
          icon += '<text x="' + m.x.toFixed(1) + '" y="' + (m.y + 3.8).toFixed(1) + '" text-anchor="middle" font-size="9.0" fill="' + stroke + '" font-weight="700">⚓</text>';
        }
        icon += '<text x="' + (m.x + 11).toFixed(1) + '" y="' + (m.y + 4).toFixed(1) + '" font-size="12" fill="' + labelColor + '" font-weight="700">' + label + '</text>';
        icon += selectedInfoBlock;
        icon += '</g>';
        return icon;
      });

      const bullseyeEls = bullseye
        ? (function () {
          const m = mapPt(bullseye);
          const ring1 = 11;
          const ring2 = 7;
          const ring3 = 3;
          const label = 'B/E';
          return '<g>'
            + '<circle cx="' + m.x.toFixed(1) + '" cy="' + m.y.toFixed(1) + '" r="' + ring1 + '" fill="none" stroke="' + palette.bullStroke + '" stroke-width="1.6" />'
            + '<circle cx="' + m.x.toFixed(1) + '" cy="' + m.y.toFixed(1) + '" r="' + ring2 + '" fill="none" stroke="' + palette.bullStroke + '" stroke-width="1.4" />'
            + '<circle cx="' + m.x.toFixed(1) + '" cy="' + m.y.toFixed(1) + '" r="' + ring3 + '" fill="' + palette.bullFill + '" stroke="' + palette.bullStroke + '" stroke-width="1.2" />'
            + '<line x1="' + (m.x - 13).toFixed(1) + '" y1="' + m.y.toFixed(1) + '" x2="' + (m.x + 13).toFixed(1) + '" y2="' + m.y.toFixed(1) + '" stroke="' + palette.bullStroke + '" stroke-width="1.1" />'
            + '<line x1="' + m.x.toFixed(1) + '" y1="' + (m.y - 13).toFixed(1) + '" x2="' + m.x.toFixed(1) + '" y2="' + (m.y + 13).toFixed(1) + '" stroke="' + palette.bullStroke + '" stroke-width="1.1" />'
            + '<text x="' + (m.x + 14).toFixed(1) + '" y="' + (m.y - 10).toFixed(1) + '" font-size="11" fill="' + palette.bullLabel + '" font-weight="700">' + label + '</text>'
            + '</g>';
        })()
        : '';

      function iconFor(m) {
        const x = m.x.toFixed(1);
        const y = m.y.toFixed(1);
        function racetrack(colorHex, label) {
          const w = 64;
          const h = 32;
          const rx = 14;
          return '<g><rect x="' + (m.x - (w / 2)).toFixed(1) + '" y="' + (m.y - (h / 2)).toFixed(1) + '" width="' + w + '" height="' + h + '" rx="' + rx + '" ry="' + rx + '" fill="' + palette.raceFill + '" stroke="' + colorHex + '" stroke-width="2.2" /><text x="' + x + '" y="' + (m.y + 5.5).toFixed(1) + '" text-anchor="middle" font-size="14" fill="' + colorHex + '" font-weight="700">' + label + '</text></g>';
        }
        if (m.kind === 'aar') return racetrack('#111111', 'AAR');
        if (m.kind === 'cap') return racetrack('#2f5fa7', 'CAP');
        if (m.kind === 'hld') return racetrack('#2f7f4f', 'HLD');
        if (m.kind === 'ip') {
          const s = 11;
          return '<rect x="' + (m.x - s).toFixed(1) + '" y="' + (m.y - s).toFixed(1) + '" width="' + (s * 2) + '" height="' + (s * 2) + '" fill="' + palette.ipFill + '" stroke="' + palette.ipStroke + '" stroke-width="' + (1.5 * dtcOverlayScale).toFixed(1) + '" />';
        }
        if (m.kind === 'tgt') {
          const p1 = x + ',' + (m.y - 12).toFixed(1);
          const p2 = (m.x - 12).toFixed(1) + ',' + (m.y + 10).toFixed(1);
          const p3 = (m.x + 12).toFixed(1) + ',' + (m.y + 10).toFixed(1);
          return '<polygon points="' + p1 + ' ' + p2 + ' ' + p3 + '" fill="' + palette.tgtFill + '" stroke="' + palette.tgtStroke + '" stroke-width="' + (1.5 * dtcOverlayScale).toFixed(1) + '" />';
        }
        if (m.kind === 'home' || m.kind === 'ldg') {
          const r = 12;
          const roofTop = x + ',' + (m.y - 12).toFixed(1);
          const roofL = (m.x - r).toFixed(1) + ',' + (m.y - 2).toFixed(1);
          const roofR = (m.x + r).toFixed(1) + ',' + (m.y - 2).toFixed(1);
          const baseX = (m.x - 8).toFixed(1);
          const baseY = (m.y - 2).toFixed(1);
          return '<polygon points="' + roofTop + ' ' + roofL + ' ' + roofR + '" fill="' + palette.homeRoof + '" stroke="' + palette.homeStroke + '" stroke-width="' + (1.5 * dtcOverlayScale).toFixed(1) + '" /><rect x="' + baseX + '" y="' + baseY + '" width="16" height="12" fill="' + palette.homeBase + '" stroke="' + palette.homeStroke + '" stroke-width="' + (1.5 * dtcOverlayScale).toFixed(1) + '" />';
        }
        if (m.kind === 'tko') {
          return '<circle cx="' + x + '" cy="' + y + '" r="10" fill="' + palette.tkoFill + '" stroke="' + palette.tkoStroke + '" stroke-width="' + (1.5 * dtcOverlayScale).toFixed(1) + '" />';
        }
        return '<circle cx="' + x + '" cy="' + y + '" r="10" fill="' + palette.wpFill + '" stroke="' + palette.wpStroke + '" stroke-width="' + (1.5 * dtcOverlayScale).toFixed(1) + '" />';
      }

      const pointEls = mapped.map(function (m) {
        const step = escapeHtml(String((m.wp && m.wp.stepDisplay !== undefined && m.wp.stepDisplay !== null && String(m.wp.stepDisplay).trim()) ? m.wp.stepDisplay : (m.wp.step || '-')));
        const name = escapeHtml(String(m.wp.name || ''));
        const label = name && name !== '-' ? ('STP ' + step + ' ' + name) : ('STP ' + step);
        const tx = (m.x + (isFinite(Number(m.labelDx)) ? Number(m.labelDx) : 14)).toFixed(1);
        const ty = (m.y + (isFinite(Number(m.labelDy)) ? Number(m.labelDy) : -14)).toFixed(1);
        return iconFor(m)
          + '<text x="' + tx + '" y="' + ty + '" font-size="22" fill="' + palette.label + '" font-weight="700">' + label + '</text>';
      });

      function assetIconFor(a) {
        const x = a.x;
        const y = a.y;
        const stroke = palette.assetBlueDark;
        const fill = palette.assetBlue;
        const category = String((a && a.asset && a.asset.category) || '').toUpperCase();
        if (category === 'PLAYER') {
          const heading = Number(a && a.headingDeg);
          const resolvedHeading = isFinite(heading) ? normalizeHeadingDeg(heading) : 0;
          return '<g transform="rotate(' + resolvedHeading.toFixed(1) + ' ' + x.toFixed(1) + ' ' + y.toFixed(1) + ')">'
            + '<polygon points="'
            + x.toFixed(1) + ',' + y.toFixed(1) + ' '
            + (x - 10.2).toFixed(1) + ',' + (y + 22.8).toFixed(1) + ' '
            + x.toFixed(1) + ',' + (y + 17.3).toFixed(1) + ' '
            + (x + 10.2).toFixed(1) + ',' + (y + 22.8).toFixed(1)
            + '" fill="#d79cff" stroke="#8a2f99" stroke-width="1.4" />'
            + '</g>';
        }
        const kind = getMudMapAssetKind(a.asset);
          if (kind === 'awacs') {
            return '<g><rect x="' + (x - 9.6).toFixed(1) + '" y="' + (y - 7.2).toFixed(1) + '" width="19.2" height="14.4" rx="1.6" fill="' + fill + '" stroke="' + stroke + '" stroke-width="1.3" /><line x1="' + (x - 7.2).toFixed(1) + '" y1="' + y.toFixed(1) + '" x2="' + (x + 7.2).toFixed(1) + '" y2="' + y.toFixed(1) + '" stroke="' + stroke + '" stroke-width="1.2" /><circle cx="' + x.toFixed(1) + '" cy="' + (y - 10.8).toFixed(1) + '" r="2.88" fill="' + stroke + '" /></g>';
        }
        if (kind === 'tanker') {
            const p1 = (x - 9.6).toFixed(1) + ',' + y.toFixed(1);
            const p2 = x.toFixed(1) + ',' + (y - 7.2).toFixed(1);
            const p3 = (x + 9.6).toFixed(1) + ',' + y.toFixed(1);
            const p4 = x.toFixed(1) + ',' + (y + 7.2).toFixed(1);
          return '<polygon points="' + p1 + ' ' + p2 + ' ' + p3 + ' ' + p4 + '" fill="' + fill + '" stroke="' + stroke + '" stroke-width="1.3" />';
        }
        if (kind === 'jtac') {
            const p1 = x.toFixed(1) + ',' + (y - 8.4).toFixed(1);
            const p2 = (x - 8.4).toFixed(1) + ',' + (y + 8.4).toFixed(1);
            const p3 = (x + 8.4).toFixed(1) + ',' + (y + 8.4).toFixed(1);
          return '<polygon points="' + p1 + ' ' + p2 + ' ' + p3 + '" fill="' + fill + '" stroke="' + stroke + '" stroke-width="1.3" />';
        }
        if (kind === 'rotary') {
            return '<g><circle cx="' + x.toFixed(1) + '" cy="' + y.toFixed(1) + '" r="7.2" fill="' + fill + '" stroke="' + stroke + '" stroke-width="1.3" /><line x1="' + (x - 10.8).toFixed(1) + '" y1="' + y.toFixed(1) + '" x2="' + (x + 10.8).toFixed(1) + '" y2="' + y.toFixed(1) + '" stroke="' + stroke + '" stroke-width="1.2" /><line x1="' + x.toFixed(1) + '" y1="' + (y - 10.8).toFixed(1) + '" x2="' + x.toFixed(1) + '" y2="' + (y + 10.8).toFixed(1) + '" stroke="' + stroke + '" stroke-width="1.2" /></g>';
        }
        if (kind === 'marker') {
            return '<g><circle cx="' + x.toFixed(1) + '" cy="' + y.toFixed(1) + '" r="7.8" fill="' + palette.markerFill + '" stroke="' + palette.markerStroke + '" stroke-width="1.4" /><line x1="' + (x - 9.6).toFixed(1) + '" y1="' + y.toFixed(1) + '" x2="' + (x + 9.6).toFixed(1) + '" y2="' + y.toFixed(1) + '" stroke="' + palette.markerStroke + '" stroke-width="1.2" /><line x1="' + x.toFixed(1) + '" y1="' + (y - 9.6).toFixed(1) + '" x2="' + x.toFixed(1) + '" y2="' + (y + 9.6).toFixed(1) + '" stroke="' + palette.markerStroke + '" stroke-width="1.2" /></g>';
        }
          return '<rect x="' + (x - 8.4).toFixed(1) + '" y="' + (y - 6.6).toFixed(1) + '" width="16.8" height="13.2" fill="' + fill + '" stroke="' + stroke + '" stroke-width="1.3" />';
      }

      function formatAssetFrequencyText(value) {
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

      function buildSelectedAssetInfoLines(asset) {
        if (!asset || typeof asset !== 'object') return [];
        if (String(asset.category || '').trim().toUpperCase() === 'JTAC_TARGET') {
          return formatJtacNineLinePopupLines(asset);
        }
        const lines = [];
        const category = String(asset.category || '').trim().toUpperCase();
        const typeText = String(asset.typeName || '').trim();
        const freqText = formatAssetFrequencyText(asset.frequency);
        const altFreqs = Array.isArray(asset.altFrequencies) ? asset.altFrequencies : [];
        const allFreqs = [asset.frequency].concat(altFreqs);
        const normalizedFreqs = allFreqs
          .map(function (v) { return formatAssetFrequencyText(v); })
          .filter(function (v) { return !!v && isFinite(Number(v)); })
          .map(function (v) { return { text: v, mhz: Number(v) }; });
        const uhf = normalizedFreqs.find(function (f) { return f.mhz >= 225 && f.mhz <= 399.975; });
        const vhf = normalizedFreqs.find(function (f) { return f.mhz >= 30 && f.mhz < 225; });
        const tacanText = String(asset.tacan || '').trim();
        const mpText = String(asset.mpClientCallsign || '').trim();

        if (typeText) lines.push('TYPE: ' + typeText);
        const mgrsText = String(asset.mgrsText || '').trim();
        if (mgrsText) lines.push('MGRS: ' + mgrsText);
        const elevationFeet = Number(asset.elevationFeet);
        if (isFinite(elevationFeet)) lines.push('ELEV: ' + String(Math.round(elevationFeet)) + ' FT');
        const friendlyBearing = Number(asset.friendlyBearing);
        const friendlyRangeNm = Number(asset.friendlyRangeNm);
        if (isFinite(friendlyBearing) && isFinite(friendlyRangeNm) && friendlyRangeNm > 0) {
          lines.push('FRND: ' + formatHeadingDeg(friendlyBearing) + '/' + String(Number(friendlyRangeNm).toFixed(1)) + 'NM');
        }
        if (category === 'ATC') {
          if (uhf && uhf.text) lines.push('UHF: ' + uhf.text);
          if (vhf && vhf.text) lines.push('VHF: ' + vhf.text);
        } else if (freqText) lines.push('FREQ: ' + freqText);
        if (tacanText) lines.push('TACAN: ' + tacanText);
        if (mpText) lines.push('MP: ' + mpText);

        return lines;
      }

      const selected = getActiveFlightPlanSelection((typeof data === 'undefined' ? null : data));
      const rawAssets = getMudMapAssets((typeof data === 'undefined' ? null : data), dlinkOnEnabled, allowJtacTargets);
      const ownshipPoint = getPlayerMapPoint((typeof data === 'undefined' ? null : data));
      const ownshipHeadingResolver = (typeof getOwnshipHeadingDeg === 'function')
        ? getOwnshipHeadingDeg
        : function () { return NaN; };
      const ownshipHeadingDeg = ownshipHeadingResolver((typeof data === 'undefined' ? null : data), ownshipPoint);
      const selectedAssetKey = getMapSelectedAssetKeyBySelection(selected);
      const activeJtacAsset = activeJtacTarget
        ? Object.assign({}, activeJtacTarget)
        : null;
      const historyJtacAssets = historyJtacTargets
        .map(function (h) {
          const xNum = Number(h && h.xNum);
          const yNum = Number(h && h.yNum);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          const seq = Math.max(1, Math.round(Number(h && h.seq) || 1));
          return {
            callsign: 'JTAC TGT',
            name: 'TGT-' + String(seq),
            category: 'JTAC_TARGET',
            typeName: String((h && h.unitType) || '').trim(),
            frequency: '',
            altFrequencies: [],
            tacan: '',
            mpClientCallsign: '',
            altFeet: isFinite(Number(h && h.elevationFeet)) ? Math.round(Number(h && h.elevationFeet)) : 0,
            xNum: xNum,
            yNum: yNum,
            mgrsText: String((h && h.mgrsText) || '').toUpperCase().trim(),
            elevationFeet: Number(h && h.elevationFeet),
            friendlyBearing: Number(h && h.friendlyBearing),
            friendlyRangeNm: Number(h && h.friendlyRangeNm),
            attackHeadingStartDeg: Number(h && h.attackHeadingStartDeg),
            attackHeadingEndDeg: Number(h && h.attackHeadingEndDeg),
            egressHeadingDeg: Number(h && h.egressHeadingDeg),
            nl4Text: String((h && h.nl4Text) || '').trim(),
            nl5Text: String((h && h.nl5Text) || '').trim(),
            nl6Text: String((h && h.nl6Text) || '').trim(),
            nl7Text: String((h && h.nl7Text) || '').trim(),
            nl8Text: String((h && h.nl8Text) || '').trim(),
            nl9Text: String((h && h.nl9Text) || '').trim(),
            rmkText: String((h && h.rmkText) || '').trim(),
            engagementType: String((h && h.engagementType) || '').trim(),
            isJtacHistory: true,
          };
        })
        .filter(function (x) { return !!x; });

      function mergePreferActiveJtac(assetList, active, historyList) {
        const out = Array.isArray(assetList) ? assetList.slice() : [];
        const activeTarget = active && typeof active === 'object' ? active : null;
        const history = Array.isArray(historyList) ? historyList : [];
        if (!activeTarget) return out;

        function sameJtacPosition(a, b) {
          const ax = Number(a && a.xNum);
          const ay = Number(a && a.yNum);
          const bx = Number(b && b.xNum);
          const by = Number(b && b.yNum);
          if (!isFinite(ax) || !isFinite(ay) || !isFinite(bx) || !isFinite(by)) return false;
          return Math.abs(ax - bx) <= 10 && Math.abs(ay - by) <= 10;
        }

        const activeKey = makeMapAssetSelectionKey(activeTarget);
        return out.map(function (a) {
          const item = a && typeof a === 'object' ? a : null;
          if (!item) return item;
          if (String((item.category || '')).toUpperCase() !== 'JTAC_TARGET') return item;
          if (!item.isJtacHistory) return item;

          const hasNineLine = !!String(item.nl4Text || item.nl5Text || item.nl6Text || item.nl7Text || item.nl8Text || item.nl9Text || item.rmkText || '').trim();
          if (hasNineLine) return item;

          if (!sameJtacPosition(item, activeTarget)) return item;

          const historyMatch = history.find(function (h) { return sameJtacPosition(h, item); }) || {};
          const seq = Math.max(1, Math.round(Number(historyMatch.seq) || 1));
          return Object.assign({}, activeTarget, {
            name: 'TGT-' + String(seq),
            isJtacHistory: true,
            historyPopupFallback: true,
            popupKey: activeKey,
          });
        });
      }

      const mapAssets = mergePreferActiveJtac(rawAssets.concat(activeJtacAsset ? [activeJtacAsset] : []).concat(historyJtacAssets), activeJtacAsset, historyJtacTargets)
        .map(function (asset) {
          const north = Number(asset.xNum);
          const east = Number(asset.yNum);
          if (!isFinite(north) || !isFinite(east)) return null;
          const category = String((asset && asset.category) || '').toUpperCase();
          if (category === 'ATC') return null;
          asset.xNum = north;
          asset.yNum = east;
          return asset;
        })
        .filter(function (asset) { return !!asset; })
        .map(function (asset) {
          const category = String((asset && asset.category) || '').toUpperCase();
          const renderAsset = (category === 'PLAYER' && ownshipPoint && isFinite(Number(ownshipPoint.xNum)) && isFinite(Number(ownshipPoint.yNum)))
            ? Object.assign({}, asset || {}, { xNum: Number(ownshipPoint.xNum), yNum: Number(ownshipPoint.yNum) })
            : asset;
          const p = mapPt(renderAsset);
          const selectionKey = makeMapAssetSelectionKey(asset);
          return {
            asset: renderAsset,
            x: p.x,
            y: p.y,
            headingDeg: category === 'PLAYER'
              ? (isFinite(Number(ownshipHeadingDeg)) ? Number(ownshipHeadingDeg) : Number(renderAsset && renderAsset.headingDeg))
              : NaN,
            selectionKey: selectionKey,
            isSelected: !!selectionKey && !!selectedAssetKey && selectionKey === selectedAssetKey,
          };
        });

      const assetEls = mapAssets.map(function (m) {
        const isMapMarker = String((m.asset && m.asset.category) || '').toUpperCase() === 'MAP_MARKER';
        const callsignLabel = String(m.asset.callsign || '').replace(/([A-Za-z])(\d)/g, '$1 $2').replace(/\s{2,}/g, ' ').trim();
        const playerMpLabel = String((m.asset && m.asset.mpClientCallsign) || '').trim();
        const markerText = String(m.asset.markerText || '').trim();
        const markerDisplayId = Number(m.asset.markerDisplayId);
        const markerPrefix = isMapMarker
          ? (markerDisplayId > 0 ? ('MKR ' + String(markerDisplayId)) : 'MKR')
          : '';
          const markerLabel = isMapMarker
          ? (markerText ? (markerPrefix + ': ' + markerText) : markerPrefix)
          : '';
          const isJtacHistory = !!(m.asset && m.asset.isJtacHistory);
        const isPlayer = String((m.asset && m.asset.category) || '').toUpperCase() === 'PLAYER';
        const displayCallsign = isPlayer ? (playerMpLabel || callsignLabel) : callsignLabel;
        const label = escapeHtml(markerLabel || displayCallsign || m.asset.name || m.asset.category || 'ASSET');
        const tx = (m.x + 10).toFixed(1);
        const ty = (m.y + 4).toFixed(1);
        const selectionKey = String(m.selectionKey || '');
        const selectionKeyEncoded = encodeURIComponent(selectionKey);
        const selectedRing = m.isSelected
          ? ('<circle cx="' + m.x.toFixed(1) + '" cy="' + m.y.toFixed(1) + '" r="11" fill="none" stroke="#c94444" stroke-width="2.4" />')
          : '';
          const labelColor = isMapMarker ? palette.markerLabel : (isPlayer ? palette.markerLabel : (isJtacHistory ? '#000000' : palette.assetBlueDark));
        let selectedInfoBlock = '';
        if (m.isSelected && !isMapMarker) {
          const infoLines = buildSelectedAssetInfoLines(m.asset);
          if (infoLines.length) {
            const fontSize = 10;
            const lineHeight = 12;
            const padX = 5;
            const padY = 4;
            const maxChars = infoLines.reduce(function (max, line) { return Math.max(max, String(line || '').length); }, 0);
            const boxWidth = Math.max(120, Math.min(300, (maxChars * 6.2) + (padX * 2)));
            const boxHeight = (infoLines.length * lineHeight) + (padY * 2);
            let boxX = m.x + 12;
            let boxY = m.y + 8;
            if ((boxX + boxWidth) > (width - 4)) boxX = m.x - boxWidth - 12;
            if ((boxY + boxHeight) > (height - 4)) boxY = m.y - boxHeight - 12;
            const textRows = infoLines.map(function (line, idx) {
              const txRow = (boxX + padX).toFixed(1);
              const tyRow = (boxY + padY + (lineHeight * (idx + 1)) - 2).toFixed(1);
              return '<text x="' + txRow + '" y="' + tyRow + '" font-size="' + fontSize + '" fill="' + palette.assetInfoText + '" font-weight="700">' + escapeHtml(String(line)) + '</text>';
            }).join('');
            selectedInfoBlock = '<g>'
              + '<rect x="' + boxX.toFixed(1) + '" y="' + boxY.toFixed(1) + '" width="' + boxWidth.toFixed(1) + '" height="' + boxHeight.toFixed(1) + '" rx="3" ry="3" fill="' + palette.assetInfoBg + '" stroke="' + palette.assetInfoStroke + '" stroke-width="1.1" />'
              + textRows
              + '</g>';
          }
        }
          return '<g data-map-asset-key="' + selectionKeyEncoded + '" data-map-asset-category="' + escapeHtml(String(m.asset.category || '')) + '" style="cursor:pointer">'
          + selectedRing
            + (function () {
              if (!isJtacHistory) return assetIconFor(m);
              const hx = m.x.toFixed(1);
              const hy = m.y.toFixed(1);
              return '<polygon points="' + hx + ',' + (m.y - 7.2).toFixed(1) + ' ' + (m.x - 7.2).toFixed(1) + ',' + hy + ' ' + hx + ',' + (m.y + 7.2).toFixed(1) + ' ' + (m.x + 7.2).toFixed(1) + ',' + hy + '" fill="#1a1a1a" stroke="#000000" stroke-width="1.4" />';
            })()
          + '<text x="' + tx + '" y="' + ty + '" font-size="10" fill="' + labelColor + '" font-weight="700">' + label + '</text>'
          + selectedInfoBlock
          + '</g>';
      });

      const northArrow = [
        '<g transform="translate(' + (width - 46) + ',44)">',
        '<line x1="0" y1="18" x2="0" y2="-10" stroke="' + palette.north + '" stroke-width="2" />',
        '<polygon points="0,-18 -6,-6 6,-6" fill="' + palette.north + '" />',
        '<text x="0" y="32" text-anchor="middle" font-size="12" fill="' + palette.north + '" font-weight="700">N</text>',
        '</g>'
      ].join('');

      const mapView = getMapViewBySelection(selected);
      const zoom = clamp(isFinite(Number(mapView.zoom)) ? Number(mapView.zoom) : 1, 0.6, 4.0);
      const panX = isFinite(Number(mapView.panX)) ? Number(mapView.panX) : 0;
      const panY = isFinite(Number(mapView.panY)) ? Number(mapView.panY) : 0;

      return '<svg viewBox="0 0 ' + width + ' ' + height + '" class="fltPlanPage3Canvas" preserveAspectRatio="xMidYMid meet" data-map-canvas="1">'
        + '<rect x="0" y="0" width="' + width + '" height="' + height + '" fill="' + palette.bg + '" />'
        + '<g data-map-content="1" transform="translate(' + panX.toFixed(1) + ' ' + panY.toFixed(1) + ') scale(' + zoom.toFixed(3) + ')">'
        + missionDrawingEls.join('')
        + corridorEls.join('')
        + historyLineEls.join('')
        + historyPointEls.join('')
        + savedHistoryPointEls.join('')
        + lineEls.join('')
        + jtacSupportEls.join('')
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

    function buildSaMapRenderState(waypoints, data, selected, overlays, root, registerPayload) {
      let mapRows = applyTypeOverrides(Array.isArray(waypoints) ? waypoints.slice() : [], selected)
        .map(function (wp) {
          if (!wp || typeof wp !== 'object') return wp;
          const clone = Object.assign({}, wp);
          if (!isFinite(Number(clone.xNum))) {
            const xRaw = String(clone.x || '').replace(/[^0-9+\-.]/g, '');
            const xParsed = Number(xRaw);
            if (isFinite(xParsed)) clone.xNum = xParsed;
          }
          if (!isFinite(Number(clone.yNum))) {
            const yRaw = String(clone.y || '').replace(/[^0-9+\-.]/g, '');
            const yParsed = Number(yRaw);
            if (isFinite(yParsed)) clone.yNum = yParsed;
          }
          return clone;
        });
      mapRows = applyAltitudeAdjustments(mapRows, selected);
      mapRows = applyEtaPlanToWaypoints(mapRows, selected);
      mapRows = applySpeedAdjustmentsToWaypoints(mapRows, selected);
      mapRows = applyRouteTimeline(mapRows, selected);
      mapRows = applyLockedTotPlan(mapRows, selected);
      updateAutoAtaRecSpdCapture(selected, mapRows);
      let resolvedOverlays = overlays && typeof overlays === 'object'
        ? overlays
        : getDtcMapOverlays(root, getDtcRouteBySelection(selected), data);
      if (!resolvedOverlays || typeof resolvedOverlays !== 'object') {
        resolvedOverlays = {};
      }
      if (!Array.isArray(resolvedOverlays.airfields)) {
        resolvedOverlays.airfields = (typeof buildMapAirfields === 'function')
          ? buildMapAirfields(data || latestData)
          : ((typeof BuildMapAirfields === 'function') ? BuildMapAirfields(data || latestData) : []);
      }
      const planState = getFlightPlanPlanState(selected);
      ensureDirectToStateValid(mapRows, planState);
      const deletedSet = getDeletedStepSet(planState);
      const skippedSet = getSkippedStepSet(mapRows, planState);
      mapRows = mapRows.filter(function (wp) {
        const key = stepToKey(wp && wp.step);
        if (!key) return true;
        if (deletedSet[key]) return false;
        if (skippedSet[key]) return false;
        return true;
      });
      const shouldRegisterPayload = registerPayload !== false;
      const mapBackgroundEnabled = isMapBackgroundEnabledBySelection(selected);
      if (mapBackgroundEnabled) {
        openFreeMapLastPayloadStatus = 'init';
      }
      const bullseyePoint = getMapBullseyePoint(root, mapRows, resolvedOverlays, data);
      const hasPayloadBuilder = typeof buildOpenFreeMapPayload === 'function';
      if (mapBackgroundEnabled && !hasPayloadBuilder) {
        openFreeMapLastPayloadStatus = 'builder-missing';
      }
      const openFreeMapPayload = mapBackgroundEnabled && hasPayloadBuilder
        ? buildOpenFreeMapPayload(mapRows, data, resolvedOverlays, bullseyePoint, selected)
        : null;
      if (mapBackgroundEnabled && !openFreeMapPayload && openFreeMapLastPayloadStatus === 'init') {
        openFreeMapLastPayloadStatus = 'builder-null';
      }
      const openFreeMapId = (shouldRegisterPayload && openFreeMapPayload && typeof registerOpenFreeMapPayload === 'function')
        ? registerOpenFreeMapPayload(selected, openFreeMapPayload)
        : '';
      if (shouldRegisterPayload && mapBackgroundEnabled && openFreeMapPayload && !openFreeMapId) {
        openFreeMapLastPayloadStatus = 'register-failed';
      }
      const bgStatus = !mapBackgroundEnabled
        ? 'BG disabled'
        : (openFreeMapRuntimeFailed
          ? ('BG runtime unavailable' + (openFreeMapRuntimeErrorText ? (': ' + openFreeMapRuntimeErrorText) : ''))
          : (openFreeMapPayload
            ? ('BG ready [' + openFreeMapLastPayloadStatus + ']')
            : ('BG unavailable for current theatre/data (' + String((((data && data.Server) || {}).Theater) || lastKnownTheater || '?') + ')' + (openFreeMapLastPayloadStatus ? (' [' + openFreeMapLastPayloadStatus + ']') : ''))));
      const selectedAssetKey = getMapSelectedAssetKeyBySelection(selected);
      const rawAssets = getMudMapAssets(data, dlinkOnEnabled, efbSaShowJtacTargets !== false);
      const jtacTargets = (efbSaShowJtacTargets !== false) ? parseJtacNineLineTargets(data || latestData || {}) : [];
      const jtacOverlay = (efbSaShowJtacTargets !== false)
        ? resolveJtacOverlayTargets(data || latestData || {}, jtacTargets)
        : { active: null, history: [] };
      const activeJtacTarget = jtacOverlay && jtacOverlay.active ? jtacOverlay.active : null;
      const historyJtacTargets = jtacOverlay && Array.isArray(jtacOverlay.history) ? jtacOverlay.history : [];
      const jtacTargetAssets = (activeJtacTarget ? [activeJtacTarget] : []).concat(historyJtacTargets)
        .map(function (t) {
          const target = t || {};
          const xNum = Number(target.xNum);
          const yNum = Number(target.yNum);
          if (!isFinite(xNum) || !isFinite(yNum)) return null;
          const seq = Math.max(1, Math.round(Number(target.seq || target.jtacSequence) || 1));
          const isHistory = !!(target && target.seq !== undefined);
          return {
            callsign: String(target.callsign || 'JTAC TGT').trim(),
            name: isHistory ? ('TGT-' + String(seq)) : String(target.name || target.unitType || 'Target').trim(),
            category: 'JTAC_TARGET',
            typeName: String(target.unitType || target.typeName || '').trim(),
            frequency: '',
            altFrequencies: [],
            tacan: '',
            mpClientCallsign: '',
            altFeet: isFinite(Number(target.elevationFeet)) ? Math.round(Number(target.elevationFeet)) : 0,
            xNum: xNum,
            yNum: yNum,
            mgrsText: String(target.mgrsText || '').toUpperCase().trim(),
            elevationFeet: isFinite(Number(target.elevationFeet)) ? Math.round(Number(target.elevationFeet)) : NaN,
            friendlyBearing: Number(target.friendlyBearing),
            friendlyRangeNm: Number(target.friendlyRangeNm),
            nl4Text: String(target.nl4Text || '').trim(),
            nl5Text: String(target.nl5Text || '').trim(),
            nl6Text: String(target.nl6Text || '').trim(),
            nl7Text: String(target.nl7Text || '').trim(),
            nl8Text: String(target.nl8Text || '').trim(),
            nl9Text: String(target.nl9Text || '').trim(),
            rmkText: String(target.rmkText || '').trim(),
            engagementType: String(target.engagementType || '').trim(),
            isJtacHistory: isHistory,
          };
        })
        .filter(function (x) { return !!x; });

      function sameJtacSelectionPos(a, b) {
        const ax = Number(a && a.xNum);
        const ay = Number(a && a.yNum);
        const bx = Number(b && b.xNum);
        const by = Number(b && b.yNum);
        if (!isFinite(ax) || !isFinite(ay) || !isFinite(bx) || !isFinite(by)) return false;
        return Math.abs(ax - bx) <= 10 && Math.abs(ay - by) <= 10;
      }

      const normalizedJtacTargetAssets = jtacTargetAssets.map(function (asset) {
        const a = asset && typeof asset === 'object' ? asset : null;
        if (!a || !a.isJtacHistory || !activeJtacTarget) return a;
        const hasNineLine = !!String(a.nl4Text || a.nl5Text || a.nl6Text || a.nl7Text || a.nl8Text || a.nl9Text || a.rmkText || '').trim();
        if (hasNineLine) return a;
        if (!sameJtacSelectionPos(a, activeJtacTarget)) return a;
        return Object.assign({}, activeJtacTarget, {
          callsign: String(a.callsign || activeJtacTarget.callsign || 'JTAC TGT').trim(),
          name: String(a.name || '').trim() || 'TGT-1',
          category: 'JTAC_TARGET',
          isJtacHistory: true,
          historyPopupFallback: true,
        });
      });
      const airfieldAssets = (Array.isArray(resolvedOverlays && resolvedOverlays.airfields) ? resolvedOverlays.airfields : [])
        .map(function (a) {
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
        .filter(function (a) { return isFinite(Number(a.xNum)) && isFinite(Number(a.yNum)); });
      const selectionPool = rawAssets.concat(airfieldAssets).concat(normalizedJtacTargetAssets);
      const markerCount = rawAssets
        .filter(function (a) { return String((a && a.category) || '').toUpperCase() === 'MAP_MARKER'; })
        .length;
      let selectedAsset = null;
      if (selectedAssetKey) {
        selectedAsset = selectionPool.find(function (a) { return makeMapAssetSelectionKey(a) === selectedAssetKey; }) || null;
        if (!selectedAsset) {
          const selectedUserWaypointId = parseEfbSaUserWaypointAssetKey(selectedAssetKey);
          if (!selectedUserWaypointId) {
            setMapSelectedAssetKeyBySelection(selected, '');
          }
        }
      }

      const visibleOverlays = {
        geolines: efbSaShowDtcOverlay ? (Array.isArray(resolvedOverlays.geolines) ? resolvedOverlays.geolines : []) : [],
        threatPoints: efbSaShowDtcOverlay ? (Array.isArray(resolvedOverlays.threatPoints) ? resolvedOverlays.threatPoints : []) : [],
        destinationPoints: efbSaShowDtcOverlay ? (Array.isArray(resolvedOverlays.destinationPoints) ? resolvedOverlays.destinationPoints : []) : [],
        faorLines: efbSaShowDtcOverlay ? (Array.isArray(resolvedOverlays.faorLines) ? resolvedOverlays.faorLines : []) : [],
        flotLines: efbSaShowDtcOverlay ? (Array.isArray(resolvedOverlays.flotLines) ? resolvedOverlays.flotLines : []) : [],
        capPoints: efbSaShowDtcOverlay ? (Array.isArray(resolvedOverlays.capPoints) ? resolvedOverlays.capPoints : []) : [],
        corridors: efbSaShowDtcOverlay ? (Array.isArray(resolvedOverlays.corridors) ? resolvedOverlays.corridors : []) : [],
        airfields: efbSaShowAirports ? (Array.isArray(resolvedOverlays.airfields) ? resolvedOverlays.airfields : []) : [],
      };
      const mapSvg = buildMudMapSvg(mapRows, data, visibleOverlays, bullseyePoint, selected);
      let readout = formatMapBraReadout(data, selectedAsset, bullseyePoint);
      if (markerCount > 0) {
        readout += '   MKR ' + String(markerCount);
      }

      return {
        mapRows: mapRows,
        overlays: resolvedOverlays,
        bullseyePoint: bullseyePoint,
        mapSvg: mapSvg,
        openFreeMapId: openFreeMapId,
        openFreeMapPayload: openFreeMapPayload,
        mapBackgroundEnabled: mapBackgroundEnabled,
        bgStatus: bgStatus,
        readout: readout,
      };
    }

    function formatDtcPage3Html(pageSwitcherHtml, waypoints, data, selected, overlays, root) {
      const state = buildSaMapRenderState(waypoints, data, selected, overlays, root);
      if (!state) {
        return '<div class="fltPlanMessage">SA Map unavailable for current FLT PLN data.</div>';
      }

      let html = '<div class="fltPlanBoard">';
      if (pageSwitcherHtml) {
        html += '<div style="margin:4px 0 6px 0;">' + pageSwitcherHtml + '</div>';
      }
      html += '<div class="controls fltPlanControls" style="margin:0 0 6px 0;"><button type="button" class="fltPlanPageBtn" data-map-zoom="in">Map In</button><button type="button" class="fltPlanPageBtn" data-map-zoom="out">Map Out</button><button type="button" class="fltPlanPageBtn" data-map-zoom="reset">Map Reset</button><button id="mapBgToggleBtn" type="button" class="fltPlanPageBtn" data-map-bg-toggle="1">BG ' + (state.mapBackgroundEnabled ? 'ON' : 'OFF') + '</button><span id="mapBgStatus" class="fltPlanMapBgStatus">' + escapeHtml(state.bgStatus) + '</span></div>';
      html += '<div class="fltPlanPage3Wrap">';
      if (state.openFreeMapId) {
        html += '<div class="fltPlanPage3Canvas fltPlanOpenMapWrap" data-openfreemap-wrap="' + escapeHtml(state.openFreeMapId) + '">';
        html += '<div class="fltPlanOpenMapHost" data-openfreemap-map-id="' + escapeHtml(state.openFreeMapId) + '" data-openfreemap-selection="' + escapeHtml(getFlightPlanEtaStartKey(selected)) + '"></div>';
        html += '<div class="fltPlanOpenMapFallback">' + state.mapSvg + '</div>';
        html += '</div>';
      } else {
        html += state.mapSvg;
      }
      html += '<div id="mapBraReadout" class="fltPlanPage3BraReadout">' + escapeHtml(state.readout) + '</div>';
      html += '</div></div>';
      return html;
    }

    function extractJetFromCallsign(callsign) {
      const text = String(callsign || '').trim();
      if (!text) return 0;

      let m = text.match(/(\d{2})\s*$/);
      if (m) {
        const pair = String(m[1] || '');
        const jet = parseInt(pair.charAt(pair.length - 1), 10);
        if (isFinite(jet) && jet > 0 && jet <= 9) return jet;
      }

      m = text.match(/(\d)\s*$/);
      if (m) {
        const jet = parseInt(String(m[1] || ''), 10);
        if (isFinite(jet) && jet > 0 && jet <= 9) return jet;
      }

      return 0;
    }

    function buildFlightRosterRows(data) {
      const server = (data && data.Server) || {};
      const pilotName = String(server.PlayerUsername || '').trim();
      const pilotCallsign = String(server.PlayerCallsign || '').trim();
      const source = Array.isArray(server.FlightMembers) ? server.FlightMembers : [];
      const rows = [];
      const seen = {};

      function makeKey(callsign, pilot) {
        const c = String(callsign || '').trim().toUpperCase();
        if (c) return c;
        return ('P:' + String(pilot || '').trim().toUpperCase());
      }

      function upsert(callsign, pilot, jet) {
        const safeCall = String(callsign || '').trim();
        const safePilot = String(pilot || '').trim();
        const safeJet = isFinite(Number(jet)) ? Math.round(Number(jet)) : 0;
        if (!safeCall && !safePilot) return;

        const key = makeKey(safeCall, safePilot);
        if (!key) return;
        if (!seen[key]) {
          seen[key] = { callsign: safeCall, pilot: safePilot, jet: safeJet };
          rows.push(seen[key]);
          return;
        }

        if (!seen[key].pilot && safePilot) seen[key].pilot = safePilot;
        if (!seen[key].callsign && safeCall) seen[key].callsign = safeCall;
        if ((!seen[key].jet || seen[key].jet <= 0) && safeJet > 0) seen[key].jet = safeJet;
      }

      if (pilotName || pilotCallsign) {
        upsert(pilotCallsign, pilotName || 'PLAYER', extractJetFromCallsign(pilotCallsign) || 1);
      }

      source.forEach(function (member) {
        if (!member || typeof member !== 'object') return;
        const callsign = String(member.Callsign || '').trim();
        const pilot = String(member.Pilot || '').trim();
        const jet = Number(member.Jet);
        upsert(callsign, pilot, isFinite(jet) ? Math.round(jet) : extractJetFromCallsign(callsign));
      });

      const usedJets = {};
      rows.forEach(function (r) {
        if (isFinite(r.jet) && r.jet > 0) usedJets[r.jet] = true;
      });
      let fallbackJet = 1;
      rows.forEach(function (r) {
        if (isFinite(r.jet) && r.jet > 0) return;
        while (usedJets[fallbackJet]) fallbackJet++;
        r.jet = fallbackJet;
        usedJets[fallbackJet] = true;
      });

      rows.sort(function (a, b) {
        const aj = Number(a.jet) || 99;
        const bj = Number(b.jet) || 99;
        if (aj !== bj) return aj - bj;
        return String(a.callsign || '').localeCompare(String(b.callsign || ''));
      });

      return rows.slice(0, 8);
    }

    function formatFlightRosterHtml(data) {
      const rows = buildFlightRosterRows(data);
      let html = '<div class="fltPlanCrewWrap"><table class="fltPlanCrewTable"><thead><tr><th>PILOT</th><th style="width:80px;">JET</th></tr></thead><tbody>';
      if (!rows.length) {
        html += '<tr><td class="fltPlanCrewPilot">No flight members detected.</td><td class="fltPlanCrewJet">-</td></tr>';
      } else {
        rows.forEach(function (r) {
          const pilot = String(r.pilot || '').trim() || (String(r.callsign || '').trim() ? ('AI / ' + String(r.callsign || '').trim()) : '-');
          html += '<tr><td class="fltPlanCrewPilot">' + escapeHtml(pilot) + '</td><td class="fltPlanCrewJet">' + escapeHtml(String(r.jet || '-')) + '</td></tr>';
        });
      }
      html += '</tbody></table></div>';
      return html;
    }

    function renderFlightPlanBoardHtml(selected, data, primaryRouteName, sourceLabel, sourceFileName, waypoints, cmdsBlockHtml, pageSwitcherHtml) {
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
      updateAutoTakeoffCaptureFromFastOwnship(data);
      updateAutoAtaRecSpdCapture(selected, displayRows);
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
      const altDisplayMode = getNavlogAltDisplayMode(selected);
      const spdDisplayMode = getNavlogSpdDisplayMode(selected);
      const distDisplayMode = getNavlogDistDisplayMode(selected);
      const coordHeaderText = (function () {
        if (coordDisplayMode === 'dms') return 'DMS';
        if (coordDisplayMode === 'ddm') return 'DDM';
        if (coordDisplayMode === 'mgrs') return 'MGRS';
        return 'X / Y';
      })();
      const altHeaderText = altDisplayMode === 'm' ? 'ALT(m)' : 'ALT';
      const spdHeaderText = spdDisplayMode === 'kmh' ? 'SPD(km/h)' : 'SPD';
      const distHeaderText = distDisplayMode === 'km' ? 'DIST(km)' : 'DIST';
      const timingLogRows = (planState && Array.isArray(planState.timingLog))
        ? planState.timingLog.slice()
        : [];
      const postFlightOpen = !!(planState && planState.postFlightOpen);
      const postFlightSummaryRows = buildPostFlightSummaryRows(selected, displayRows, timing);

      let html = '';
      html += '<div class="fltPlanBoard">';
      html += '<div class="fltPlanHeaderGrid">';
      html += '<div class="fltPlanHeaderCell"><div class="fltPlanHeaderCellLabel">ROUTE</div><div class="fltPlanHeaderCellValue">' + escapeHtml(primaryRouteName) + '</div></div>';
      html += '<div class="fltPlanHeaderCell"><div class="fltPlanHeaderCellLabel">CALL SIGN</div><div class="fltPlanHeaderCellValue">' + escapeHtml(callsign) + '</div></div>';
      html += '<div class="fltPlanHeaderCell"><div class="fltPlanHeaderCellLabel">MISSION</div><div class="fltPlanHeaderCellValue">' + escapeHtml(mission) + '</div></div>';
      html += '<div class="fltPlanHeaderCell"><div class="fltPlanHeaderCellLabel">CONFIG</div><div class="fltPlanHeaderCellValue">' + escapeHtml(config) + '</div></div>';
      html += '</div>';
      html += '<div class="fltPlanHeaderGrid">';
      html += '<div class="fltPlanHeaderCell"><div class="fltPlanHeaderCellLabel">THEATRE</div><div class="fltPlanHeaderCellValue">' + escapeHtml(theatre) + '</div></div>';
      html += '<div class="fltPlanHeaderCell"><div class="fltPlanHeaderCellLabel">SOURCE</div><div class="fltPlanHeaderCellValue">' + escapeHtml(sourceLabel || '-') + '</div></div>';
      html += '<div class="fltPlanHeaderCell"><div class="fltPlanHeaderCellLabel">ROUTE FILE</div><div class="fltPlanHeaderCellValue">' + escapeHtml(sourceFileName || '-') + '</div></div>';
      html += '<div class="fltPlanHeaderCell"><div class="fltPlanHeaderCellLabel">WAYPOINTS</div><div class="fltPlanHeaderCellValue">' + escapeHtml(String(rows.length)) + '</div></div>';
      html += '</div>';
      if (pageSwitcherHtml) {
        html += '<div class="fltPlanToolbar"><span class="fltPlanToolbarLeft">' + pageSwitcherHtml + '</span><span class="fltPlanToolbarRight"><button type="button" class="fltPlanPageBtn" data-postflight-toggle="1">' + (postFlightOpen ? 'Hide POST FLT' : 'POST FLT') + '</button></span></div>';
      } else {
        html += '<div class="fltPlanToolbar"><span class="fltPlanToolbarLeft"></span><span class="fltPlanToolbarRight"><button type="button" class="fltPlanPageBtn" data-postflight-toggle="1">' + (postFlightOpen ? 'Hide POST FLT' : 'POST FLT') + '</button></span></div>';
      }

      const expandedAnchor = getExpandedTimeAnchor(selected);
      const stepExpandedClass = expandedAnchor === 'STEP' ? ' expanded' : '';
      const startExpandedClass = expandedAnchor === 'START' ? ' expanded' : '';
      const taxiExpandedClass = expandedAnchor === 'TAXI' ? ' expanded' : '';
      const takeoffExpandedClass = expandedAnchor === 'TAKEOFF' ? ' expanded' : '';
      const totExpandedClass = expandedAnchor === 'TOT' ? ' expanded' : '';
      const stepClock = getTimingClockTextForAnchor(selected, 'STEP');
      const startClock = getTimingClockTextForAnchor(selected, 'START');
      const taxiClock = getTimingClockTextForAnchor(selected, 'TAXI');
      const takeoffClock = getTimingClockTextForAnchor(selected, 'TAKEOFF');
      const totClock = getTimingClockTextForAnchor(selected, 'TOT');
      const navEdit = getExpandedNavEdit(selected);

      function navPopupClass(stepKey, fieldKey) {
        const step = String(stepKey || '');
        const field = String(fieldKey || '').toUpperCase();
        if (navEdit.step === step && navEdit.field === field) return ' expanded';
        if (isClosingNavEdit(selected, step, field)) return ' closing';
        return '';
      }

      function navPopupEditor(edgeClass, leftAttr, rightAttr, valueText) {
        return '<div class="fltPlanTimeCellEditor ' + edgeClass + '">'
          + '<div class="fltPlanTimeEditorRow">'
          + '<span class="fltPlanTimeEditorBtns"><button type="button" class="fltPlanMiniBtn" ' + leftAttr + '>◀</button></span>'
          + '<span class="fltPlanTimeEditorCenter">' + escapeHtml(String(valueText || '-')) + '</span>'
          + '<span class="fltPlanTimeEditorBtns"><button type="button" class="fltPlanMiniBtn" ' + rightAttr + '>▶</button></span>'
          + '</div>'
          + '</div>';
      }

      html += '<div class="fltPlanTimeGrid">';
      html += '<div class="fltPlanTimeCell clickable' + stepExpandedClass + '" data-tko-anchor="STEP" data-time-anchor="STEP" title="Set STEP to current clock (Takeoff auto = STEP +35m)"><div class="fltPlanTimeCellLabel">STEP</div><div class="fltPlanTimeCellValue">' + escapeHtml(stepClock) + '</div><div class="fltPlanTimeCellEditor edgeLeft"><div class="fltPlanTimeEditorRow"><span class="fltPlanTimeEditorBtns"><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="STEP" data-time-adjust-sec="-1" title="-1 second">«</button><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="STEP" data-time-adjust-sec="-60" title="-1 minute">◀</button></span><span class="fltPlanTimeEditorCenter">' + escapeHtml(stepClock) + '</span><span class="fltPlanTimeEditorBtns"><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="STEP" data-time-adjust-sec="60" title="+1 minute">▶</button><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="STEP" data-time-adjust-sec="1" title="+1 second">»</button></span></div></div></div>';
      html += '<div class="fltPlanTimeCell clickable' + startExpandedClass + '" data-tko-anchor="START" data-time-anchor="START" title="Set START to current clock (Takeoff auto = START +25m)"><div class="fltPlanTimeCellLabel">START</div><div class="fltPlanTimeCellValue">' + escapeHtml(startClock) + '</div><div class="fltPlanTimeCellEditor"><div class="fltPlanTimeEditorRow"><span class="fltPlanTimeEditorBtns"><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="START" data-time-adjust-sec="-1" title="-1 second">«</button><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="START" data-time-adjust-sec="-60" title="-1 minute">◀</button></span><span class="fltPlanTimeEditorCenter">' + escapeHtml(startClock) + '</span><span class="fltPlanTimeEditorBtns"><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="START" data-time-adjust-sec="60" title="+1 minute">▶</button><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="START" data-time-adjust-sec="1" title="+1 second">»</button></span></div></div></div>';
      html += '<div class="fltPlanTimeCell clickable' + taxiExpandedClass + '" data-tko-anchor="TAXI" data-time-anchor="TAXI" title="Set TAXI to current clock (Takeoff auto = TAXI +15m)"><div class="fltPlanTimeCellLabel">TAXI</div><div class="fltPlanTimeCellValue">' + escapeHtml(taxiClock) + '</div><div class="fltPlanTimeCellEditor"><div class="fltPlanTimeEditorRow"><span class="fltPlanTimeEditorBtns"><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="TAXI" data-time-adjust-sec="-1" title="-1 second">«</button><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="TAXI" data-time-adjust-sec="-60" title="-1 minute">◀</button></span><span class="fltPlanTimeEditorCenter">' + escapeHtml(taxiClock) + '</span><span class="fltPlanTimeEditorBtns"><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="TAXI" data-time-adjust-sec="60" title="+1 minute">▶</button><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="TAXI" data-time-adjust-sec="1" title="+1 second">»</button></span></div></div></div>';
      html += '<div class="fltPlanTimeCell clickable' + takeoffExpandedClass + '" data-tko-anchor="TAKEOFF" data-time-anchor="TAKEOFF" title="Set TAKEOFF to current clock"><div class="fltPlanTimeCellLabel">TAKEOFF</div><div class="fltPlanTimeCellValue">' + escapeHtml(takeoffClock) + '</div><div class="fltPlanTimeCellEditor edgeRight"><div class="fltPlanTimeEditorRow"><span class="fltPlanTimeEditorBtns"><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="TAKEOFF" data-time-adjust-sec="-1" title="-1 second">«</button><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="TAKEOFF" data-time-adjust-sec="-60" title="-1 minute">◀</button></span><span class="fltPlanTimeEditorCenter">' + escapeHtml(takeoffClock) + '</span><span class="fltPlanTimeEditorBtns"><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="TAKEOFF" data-time-adjust-sec="60" title="+1 minute">▶</button><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="TAKEOFF" data-time-adjust-sec="1" title="+1 second">»</button></span></div></div></div>';
      html += '<div class="fltPlanTimeCell clickable' + totExpandedClass + '" data-time-anchor="TOT" title="Adjust TOT"><div class="fltPlanTimeCellLabel">TOT</div><div class="fltPlanTimeCellValue">' + escapeHtml(totClock) + '</div><div class="fltPlanTimeCellEditor edgeLeft"><div class="fltPlanTimeEditorRow"><span class="fltPlanTimeEditorBtns"><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="TOT" data-time-adjust-sec="-1" title="-1 second">«</button><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="TOT" data-time-adjust-sec="-60" title="-1 minute">◀</button></span><span class="fltPlanTimeEditorCenter">' + escapeHtml(totClock) + '</span><span class="fltPlanTimeEditorBtns"><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="TOT" data-time-adjust-sec="60" title="+1 minute">▶</button><button type="button" class="fltPlanMiniBtn" data-time-adjust-anchor="TOT" data-time-adjust-sec="1" title="+1 second">»</button></span></div></div></div>';
      html += '</div>';
      if (postFlightOpen) {
        html += '<div class="fltPlanInfoBlock" style="min-height:0; margin-bottom:8px;">';
        html += '<div class="fltPlanInfoTitle">POST FLT</div>';
        html += '<div class="fltPlanInfoBody" style="font-size:12px; line-height:1.2; max-height:220px;">' + (postFlightSummaryRows.length ? postFlightSummaryRows.map(escapeHtml).join('<br>') : 'No events recorded yet.') + '</div>';
        html += '</div>';
      }

      html += formatFlightRosterHtml(data);

      html += '<div class="fltPlanWpWrap">';
      html += '<div class="fltPlanWpTitle">Route: ' + escapeHtml(primaryRouteName) + '</div>';
      html += '<div class="fltPlanWpTableWrap">';
      html += '<table class="fltPlanWpTable">';
      html += '<thead><tr><th style="width:34px;">STP</th><th style="width:58px;">TYPE</th><th style="width:110px;">NAME</th><th class="fltPlanEtaHeader" style="width:78px;" data-navlog-alt-cycle="1" title="Click to toggle ALT/ALT(m)">' + escapeHtml(altHeaderText) + '</th><th style="width:40px;">HDG</th><th class="fltPlanEtaHeader" style="width:74px;" data-navlog-spd-cycle="1" title="Click to toggle SPD/SPD(km/h)">' + escapeHtml(spdHeaderText) + '</th><th class="fltPlanEtaHeader" style="width:64px;" data-navlog-dist-cycle="1" title="Click to toggle DIST/DIST(km)">' + escapeHtml(distHeaderText) + '</th><th class="fltPlanEtaHeader" style="width:90px;" data-eta-header="1" title="Click to set ETA start from current time">' + etaHeading + '</th><th class="fltPlanEtaHeader" style="width:210px;" data-navlog-coord-cycle="1" title="Click to cycle X/Y → DMS → DDM → MGRS">' + escapeHtml(coordHeaderText) + '</th></tr></thead>';
      html += '<tbody>';

      if (!displayRows.length) {
        html += '<tr><td colspan="9">No waypoints found.</td></tr>';
      } else {
        displayRows.forEach(function (wp) {
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
          if (!isStart) {
            const showDir = (rowActionMode === 'dir' && rowActionKey === stepKey) || (rowRevealMode === 'dir' && rowRevealKey === stepKey);
            const showDel = (rowActionMode === 'del' && rowActionKey === stepKey) || (rowRevealMode === 'del' && rowRevealKey === stepKey);
            if (showDir) rowClasses.push('fltPlanWpRowActionDir');
            if (showDel) rowClasses.push('fltPlanWpRowActionDel');
          }
          const rowClassAttr = rowClasses.length ? ' class="' + rowClasses.join(' ') + '"' : '';
          const rowDataAttrs = !isStart
            ? ' data-navlog-step="' + escapeHtml(stepKey) + '" data-navlog-row="1"'
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
          const altCellHtml = formatNavlogAltitudeDisplayHtml(wp, altDisplayMode);
          const speedCellDisplay = formatNavlogSpeedDisplayText(wp, speedDisplay, speedRec, spdDisplayMode);
          const distCellDisplay = formatNavlogDistanceDisplayText(wp, distDisplayMode);
          const altPopupDisplay = formatAltitudeByMode(wp && wp.altFeet, altDisplayMode, true);
          const speedPopupDisplay = formatSpeedByMode(wp && wp.spd, spdDisplayMode, true);
          const speedReadoutTitle = speedRec
            ? 'Recommended speed: click to accept'
            : (spdDisplayMode === 'kmh' ? 'Speed shown in km/h (edit popup still adjusts KCAS)' : speedClickTitle);
          const speedModeAttrs = (speedRec || spdDisplayMode === 'kts')
            ? ' data-speed-mode-step="' + escapeHtml(stepKey) + '" data-speed-alt="' + escapeHtml(String(wp.altFeet)) + '"'
            : '';
          const stepLabel = String((wp && wp.stepDisplay !== undefined && wp.stepDisplay !== null && String(wp.stepDisplay).trim()) ? wp.stepDisplay : wp.step);
          html += '<tr' + rowClassAttr + rowDataAttrs + '>';
          if (isStart) {
            html += '<td class="fltPlanCellNum">' + escapeHtml(stepLabel) + '</td>';
          } else {
            const dirArmedClass = isNavlogRowActionArmed(selected, stepKey, 'dir') ? ' armed' : '';
            const delArmedClass = isNavlogRowActionArmed(selected, stepKey, 'del') ? ' armed' : '';
            html += '<td class="fltPlanCellNum"><span class="fltPlanStepCellWrap"><span>' + escapeHtml(stepLabel) + '</span><button type="button" class="fltPlanRowActionBtn dir' + dirArmedClass + '" data-row-action="dir" data-row-step="' + escapeHtml(stepKey) + '">DIR TO</button><button type="button" class="fltPlanRowActionBtn del' + delArmedClass + '" data-row-action="del" data-row-step="' + escapeHtml(stepKey) + '">DEL STP</button></span></td>';
          }
          if (wp.isStart) {
            html += '<td>' + escapeHtml(wp.type) + '</td>';
          } else {
            html += '<td class="fltPlanNavEditCell' + navPopupClass(stepKey, 'TYPE') + '"><div class="fltPlanNavEditHost' + navPopupClass(stepKey, 'TYPE') + '" data-nav-edit-host="1" data-nav-edit-step="' + escapeHtml(stepKey) + '" data-nav-edit-field="TYPE"><span class="fltPlanNavEditReadout" data-nav-edit-toggle="1" data-nav-edit-step="' + escapeHtml(stepKey) + '" data-nav-edit-field="TYPE">' + escapeHtml(wp.type) + '</span>' + navPopupEditor('edgeLeft', 'data-type-step="' + escapeHtml(stepKey) + '" data-type-current="' + escapeHtml(wp.type) + '" data-type-delta="-1" title="Prev type"', 'data-type-step="' + escapeHtml(stepKey) + '" data-type-current="' + escapeHtml(wp.type) + '" data-type-delta="1" title="Next type"', wp.type) + '</div></td>';
          }
          html += '<td>' + escapeHtml(wp.name || '-') + '</td>';
          if (wp.isStart) {
            html += '<td class="fltPlanCellNum">' + altCellHtml + '</td>';
          } else {
            html += '<td class="fltPlanCellNum fltPlanNavEditCell' + navPopupClass(stepKey, 'ALT') + '"><div class="fltPlanNavEditHost' + navPopupClass(stepKey, 'ALT') + '" data-nav-edit-host="1" data-nav-edit-step="' + escapeHtml(stepKey) + '" data-nav-edit-field="ALT"><span class="fltPlanNavEditReadout" data-nav-edit-toggle="1" data-nav-edit-step="' + escapeHtml(stepKey) + '" data-nav-edit-field="ALT">' + altCellHtml + '</span>' + navPopupEditor('', 'data-alt-step="' + escapeHtml(stepKey) + '" data-alt-dir="-1" data-alt-current="' + escapeHtml(String(wp.altFeet)) + '" data-alt-mode="' + escapeHtml(altDisplayMode) + '" title="Lower altitude"', 'data-alt-step="' + escapeHtml(stepKey) + '" data-alt-dir="1" data-alt-current="' + escapeHtml(String(wp.altFeet)) + '" data-alt-mode="' + escapeHtml(altDisplayMode) + '" title="Raise altitude"', altPopupDisplay) + '</div></td>';
          }
          if (wp.isStart) {
            html += '<td class="fltPlanCellNum">' + escapeHtml(wp.hdg || '-') + '</td>';
            html += '<td class="fltPlanCellNum">' + escapeHtml(formatNavlogSpeedDisplayText(wp, wp.spd, null, spdDisplayMode)) + '</td>';
          } else {
            html += '<td class="fltPlanCellNum">' + escapeHtml(wp.hdg || '-') + '</td>';
            html += '<td class="fltPlanCellNum fltPlanNavEditCell' + navPopupClass(stepKey, 'SPD') + '"><div class="fltPlanNavEditHost' + navPopupClass(stepKey, 'SPD') + '" data-nav-edit-host="1" data-nav-edit-step="' + escapeHtml(stepKey) + '" data-nav-edit-field="SPD"><span class="fltPlanNavEditReadout fltPlanSpdValue' + (speedRec ? ' fltPlanRecSpeed' : '') + '" data-nav-edit-toggle="1" data-nav-edit-step="' + escapeHtml(stepKey) + '" data-nav-edit-field="SPD"' + speedModeAttrs + (speedRec ? ' data-speed-rec-step="' + escapeHtml(stepKey) + '"' : '') + ' title="' + escapeHtml(speedReadoutTitle) + '">' + escapeHtml(speedCellDisplay) + '</span>' + navPopupEditor('', 'data-spd-step="' + escapeHtml(stepKey) + '" data-spd-dir="-1" data-spd-alt="' + escapeHtml(String(wp.altFeet)) + '" title="' + escapeHtml(speedStepTitle) + '"', 'data-spd-step="' + escapeHtml(stepKey) + '" data-spd-dir="1" data-spd-alt="' + escapeHtml(String(wp.altFeet)) + '" title="' + escapeHtml(speedStepTitle) + '"', speedPopupDisplay) + '</div></td>';
          }
          html += '<td class="fltPlanCellNum">' + escapeHtml(distCellDisplay) + '</td>';
          if (wp.isStart) {
            html += '<td class="fltPlanCellNum">' + escapeHtml(wp.etaDisplay || wp.eta) + '</td>';
          } else {
            const ataEntry = (planState && planState.ataByStep && planState.ataByStep[stepKey]) ? planState.ataByStep[stepKey] : null;
            const plannedEtaText = String(wp.etaDisplay || wp.eta || '-');
            const ataShown = ataEntry && isFinite(Number(ataEntry.actualSeconds));
            const etaClass = ataShown ? 'fltPlanAtaValue' : 'fltPlanEtaValue';
            const etaText = ataShown ? formatSecondsToClock(Number(ataEntry.actualSeconds)) : plannedEtaText;
            const etaTitle = ataShown ? 'ATA active - click to return to ETA' : 'ETA - click to mark ATA at current mission time';
            html += '<td class="fltPlanCellNum"><span class="fltPlanEtaWrap"><input type="checkbox" data-tot-lock-step="' + escapeHtml(stepKey) + '"' + lockChecked + '><span class="' + etaClass + '" data-eta-step="' + escapeHtml(stepKey) + '" data-eta-planned="' + escapeHtml(plannedEtaText) + '" title="' + escapeHtml(etaTitle) + '">' + escapeHtml(etaText) + '</span></span></td>';
          }
          const coordText = getNavlogCoordinateDisplayText(wp, theatre, coordDisplayMode);
          html += '<td class="fltPlanCellNum fltPlanCoordValue">' + escapeHtml(coordText) + '</td>';
          html += '</tr>';
        });
      }

      html += '</tbody></table></div></div>';
      const freqWrapClass = 'fltPlanInfoFreqWrap' + getBottomPanelClassSuffix(selected, 'FREQ');
      const cmdsWrapClass = 'fltPlanInfoCmdsWrap' + getBottomPanelClassSuffix(selected, 'CMDS');
      const assetsWrapClass = 'fltPlanInfoAssetsWrap' + getBottomPanelClassSuffix(selected, 'ASSETS');
      const wxWrapClass = 'fltPlanInfoWxWrap' + getBottomPanelClassSuffix(selected, 'WX');
      html += '<div class="fltPlanBottomGrid">';
      html += '<div class="' + freqWrapClass + '"><div class="' + getBottomPanelShellClass(selected, 'FREQ') + '">' + formatFrequenciesBlockHtml(data, selected) + '</div></div>';
      if (cmdsBlockHtml) {
        html += '<div class="' + cmdsWrapClass + '"><div class="' + getBottomPanelShellClass(selected, 'CMDS') + '">' + cmdsBlockHtml + '</div></div>';
      }
      html += '<div class="' + assetsWrapClass + '"><div class="' + getBottomPanelShellClass(selected, 'ASSETS') + '">' + formatAssetsBlockHtml(data, selected) + '</div></div>';
      html += '<div class="' + wxWrapClass + '"><div class="' + getBottomPanelShellClass(selected, 'WX') + '">' + formatMetarBlockHtml(data, selected) + '</div></div>';
      html += '</div>';
      html += '</div>';
      return html;
    }

    function formatRouteToolTableHtml(root, selected, data) {
      const presets = (root && typeof root === 'object') ? root : null;
      if (!presets) return '';

      const routeNames = Object.keys(presets);
      if (!routeNames.length) return '';

      routeNames.sort(function (a, b) { return String(a).localeCompare(String(b)); });
      const primaryRouteName = String(routeNames[0] || getDtcDisplayName(selected) || '-');
      const primaryRoute = presets[primaryRouteName] || {};
      const waypoints = getRouteWaypoints(primaryRoute);
      const page = getDtcPageBySelection(selected);

      const routeRows = routeNames.map(function (routeName) {
        const routeObj = presets[routeName] || {};
        const count = getRouteWaypoints(routeObj).length;
        const name = String(routeName || '-');
        return '<tr><td style="width:56px;">' + (name === primaryRouteName ? '<strong>' + escapeHtml(name) + '</strong>' : escapeHtml(name)) + '</td><td>' + escapeHtml(String(count)) + '</td></tr>';
      });
      const routeSummaryHtml = '<div class="fltPlanPage2Section"><div class="fltPlanPage2Title">ROUTES</div><div class="fltPlanPage2Body"><table class="fltPlanPage2Table"><thead><tr><th style="width:56px;">ROUTE</th><th>WAYPOINTS</th></tr></thead><tbody>' + routeRows.join('') + '</tbody></table></div></div>';
      const pageSwitcherHtml = '<span class="fltPlanPageSwitcher"><button type="button" class="fltPlanPageBtn' + (page === 1 ? ' active' : '') + '" data-dtc-page="1">NAVLOG</button><button type="button" class="fltPlanPageBtn' + (page === 2 ? ' active' : '') + '" data-dtc-page="2">COM/ROUTE</button><button type="button" class="fltPlanPageBtn' + (page === 3 ? ' active' : '') + '" data-dtc-page="3">STORES/AID</button></span>';

      if (page === 3) {
        return formatStoresPageHtml(pageSwitcherHtml, data);
      }

      if (page === 2) {
        let html = '<div class="fltPlanBoard">';
        html += '<div style="margin:4px 0 6px 0;">' + pageSwitcherHtml + '</div>';
        html += '<div class="fltPlanPage2Grid">';
        html += formatRuntimeCommPanelHtml(data);
        html += '<div class="fltPlanPage2Stack">';
        html += routeSummaryHtml;
        html += formatMapMarkersPanelHtml(selected, data);
        html += '</div>';
        html += '</div></div>';
        return html;
      }

      return renderFlightPlanBoardHtml(selected, data, primaryRouteName, 'ROUTE TOOL', getPathFileName(getFltPlnPath(selected)), waypoints, formatRuntimeCmdsInfoBlockHtml(data, selected), pageSwitcherHtml);
    }

    function parseMissionRuntimeWaypointSample(line) {
      const text = String(line || '').trim();
      if (!text) return null;

      const parts = text.split('|');
      if (!parts.length) return null;

      const groupPart = String(parts[1] || '');
      const groupMatch = groupPart.match(/^group=(.*)$/i);
      const groupName = groupMatch ? String(groupMatch[1] || '').trim() : '';

      const map = {};
      parts.forEach(function (p) {
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

      const stepNumber = Math.round(pt);
      const runtimeDisplayStep = stepNumber > 0 ? (stepNumber - 1) : 0;

      return {
        groupName: groupName,
        step: String(stepNumber),
        stepDisplay: String(runtimeDisplayStep),
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

    function cloneRuntimeWaypoints(rows) {
      const list = Array.isArray(rows) ? rows : [];
      return list.map(function (wp) {
        return wp && typeof wp === 'object' ? Object.assign({}, wp) : wp;
      });
    }

    function normalizeRuntimeWaypointNumber(value, precision) {
      const n = Number(value);
      if (!isFinite(n)) return 'NaN';
      if (!isFinite(Number(precision)) || Number(precision) < 0) {
        return String(Math.round(n));
      }
      return n.toFixed(Math.max(0, Math.min(6, Math.round(Number(precision)))));
    }

    function buildRuntimeWaypointsFingerprint(rows) {
      const list = Array.isArray(rows) ? rows : [];
      if (!list.length) return '';
      const normalized = list
        .map(function (wp) {
          const row = wp && typeof wp === 'object' ? wp : {};
          return [
            String(stepToKey(row.step) || ''),
            normalizeRuntimeWaypointNumber(row.xNum, 1),
            normalizeRuntimeWaypointNumber(row.yNum, 1),
            normalizeRuntimeWaypointNumber(row.altFeet, 0),
            normalizeRuntimeWaypointNumber(row.etaSourceSeconds, 0),
            String(row.typeRaw || row.type || '').toUpperCase().trim(),
          ].join('|');
        })
        .sort();
      return normalized.join('||');
    }

    function isRuntimeSnapshotUpdateDeferred(selected) {
      const key = String(selected || '').trim();
      if (!key) return false;
      if (key !== '__RUNTIME_PLAYER__') return false;
      const state = getFlightPlanPlanState(key);
      const navEdit = getExpandedNavEdit(key);
      const navEditActive = !!(navEdit && stepToKey(navEdit.step) && String(navEdit.field || '').trim());
      const rowActionActive = !!(state && stepToKey(state.rowActionStep));
      const draggingActive = !!(navlogRowDrag && String(navlogRowDrag.selected || '') === key);
      return navEditActive || rowActionActive || draggingActive;
    }

    function getRuntimeFlightPlanLiveLaneData(data, selected) {
      const model = data || latestData || {};
      const key = String(selected || '').trim();
      const state = getFlightPlanPlanState(key);
      const cache = (state && state.runtimeLiveLaneCache && typeof state.runtimeLiveLaneCache === 'object')
        ? state.runtimeLiveLaneCache
        : { fingerprint: '', data: null };
      if (state && (!state.runtimeLiveLaneCache || typeof state.runtimeLiveLaneCache !== 'object')) {
        state.runtimeLiveLaneCache = cache;
      }

      function lanePayloadFrom(source) {
        const src = source || {};
        return {
          unitsATC: getMergedList(src.Units, 'ATC'),
          unitsFLIGHT: getMergedList(src.Units, 'FLIGHT'),
          unitsTANKER: getMergedList(src.Units, 'TANKER'),
          unitsAWACS: getMergedList(src.Units, 'AWACS'),
          unitsJTAC: getMergedList(src.Units, 'JTAC'),
          atcMetars: (((src || {}).Server || {}).AtcMetars) || {},
          metarUseInHg: !!metarPressureInHg,
        };
      }

      const livePayload = lanePayloadFrom(model);
      let fingerprint = '';
      try {
        fingerprint = JSON.stringify(livePayload);
      } catch (_) {
        fingerprint = '';
      }

      if (!cache.data || cache.fingerprint !== fingerprint) {
        cache.data = livePayload;
        cache.fingerprint = fingerprint;
      }

      return cache.data || livePayload;
    }

    function readMissionRuntimeWaypointsFromDiagnostics(data) {
      const server = (data && data.Server) || {};
      const diagnostics = (server && server.Diagnostics) || {};
      const playerRows = Array.isArray(diagnostics.playerGroupWaypoints) ? diagnostics.playerGroupWaypoints : [];
      const rows = playerRows.length
        ? playerRows
        : (Array.isArray(diagnostics.waypointSamples) ? diagnostics.waypointSamples : []);
      const parsed = rows
        .map(parseMissionRuntimeWaypointSample)
        .filter(function (x) { return !!x; });

      if (!parsed.length) return [];

      const playerGroup = String(diagnostics.playerGroup || '').trim();
      const effective = playerGroup
        ? parsed.filter(function (wp) { return String(wp.groupName || '').trim() === playerGroup; })
        : parsed;

      if (!effective.length) return [];

      return effective
        .sort(function (a, b) { return Number(a.step) - Number(b.step); })
        .map(function (wp) {
          const clone = Object.assign({}, wp);
          delete clone.groupName;
          return clone;
        });
    }

    function getMissionRuntimeWaypoints(data) {
      const model = data || latestData || {};
      const selected = String(model.DtcSelectedFile || '');
      const hasExplicitPlanSelection = !!selected && runtimeFlightPlanUserOverride;
      const missionIdentity = getMissionIdentity(model);
      const diagnostics = ((model && model.Server) || {}).Diagnostics || {};
      const liveGroupName = String(diagnostics.playerGroup || '').trim();

      if (hasExplicitPlanSelection) {
        if (runtimeFlightPlanSnapshot
          && runtimeFlightPlanSnapshot.length
          && runtimeFlightPlanSnapshotMissionIdentity === missionIdentity) {
          return cloneRuntimeWaypoints(runtimeFlightPlanSnapshot);
        }
        return readMissionRuntimeWaypointsFromDiagnostics(model);
      }

      const live = readMissionRuntimeWaypointsFromDiagnostics(model);
      const liveFingerprint = buildRuntimeWaypointsFingerprint(live);
      const hasSnapshot = !!(runtimeFlightPlanSnapshot && runtimeFlightPlanSnapshot.length);
      const snapshotMatchesMission = runtimeFlightPlanSnapshotMissionIdentity === missionIdentity;

      // Keep runtime NAVLOG stable once captured for the active mission.
      // It should only be replaced on mission identity change/reset, not by periodic refresh nudges.
      if (live.length && (!hasSnapshot || !snapshotMatchesMission)) {
        runtimeFlightPlanSnapshot = cloneRuntimeWaypoints(live);
        runtimeFlightPlanSnapshotMissionIdentity = missionIdentity;
        runtimeFlightPlanSnapshotGroupName = liveGroupName;
        runtimeFlightPlanSnapshotFingerprint = liveFingerprint;
      } else if (liveGroupName && snapshotMatchesMission && !runtimeFlightPlanSnapshotGroupName) {
        runtimeFlightPlanSnapshotGroupName = liveGroupName;
      }

      if (runtimeFlightPlanSnapshot
        && runtimeFlightPlanSnapshot.length
        && runtimeFlightPlanSnapshotMissionIdentity === missionIdentity) {
        return cloneRuntimeWaypoints(runtimeFlightPlanSnapshot);
      }

      return live;
    }

    function formatMissionRuntimeFlightPlanHtml(data, waypoints) {
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
      const pageSwitcherHtml = '<span class="fltPlanPageSwitcher"><button type="button" class="fltPlanPageBtn' + (page === 1 ? ' active' : '') + '" data-dtc-page="1">NAVLOG</button><button type="button" class="fltPlanPageBtn' + (page === 2 ? ' active' : '') + '" data-dtc-page="2">COM/ROUTE</button><button type="button" class="fltPlanPageBtn' + (page === 3 ? ' active' : '') + '" data-dtc-page="3">STORES/AID</button></span>';
      if (page === 2) {
        return formatRuntimePage2Html(data, pageSwitcherHtml, selected);
      }
      if (page === 3) {
        return formatStoresPageHtml(pageSwitcherHtml, data);
      }
      return renderFlightPlanBoardHtml(selected, data, routeName, 'MISSION RUNTIME', '-', rows, formatRuntimeCmdsInfoBlockHtml(data, selected), pageSwitcherHtml);
    }

    function formatDtcTableHtml(root, selected, data) {
      const sourceType = String((root && root.type) || '').toUpperCase();
      const isAh64 = sourceType.indexOf('AH-64') >= 0 || sourceType.indexOf('APACHE') >= 0;
      const availableMissions = isAh64 ? getAh64MissionPartitions(root) : ['M1'];
      let missionKey = getDtcMissionBySelection(selected);
      if (availableMissions.indexOf(missionKey) < 0) {
        missionKey = availableMissions[0] || 'M1';
        setDtcMissionBySelection(selected, missionKey);
      }

      const missionRoot = isAh64 ? resolveAh64MissionRoot(root, missionKey) : root;
      const allWaypoints = getDtcWaypoints(missionRoot);
      const availableRoutes = getDtcAvailableRoutes(missionRoot, allWaypoints);
      let routeKey = getDtcRouteBySelection(selected);
      if (availableRoutes.indexOf(routeKey) < 0) {
        routeKey = availableRoutes[0] || 'R1';
        setDtcRouteBySelection(selected, routeKey);
      }
      const overlayData = Object.assign({}, data || {}, { __dtcMissionKey: missionKey });
      const mapOverlays = getDtcMapOverlays(missionRoot, routeKey, overlayData);
      const isF14 = isF14DtcContext(missionRoot, data);
      let waypoints = applyTypeOverrides(filterDtcWaypointsByRoute(missionRoot, allWaypoints, routeKey), selected);
      if (isF14 && routeKey === 'R1') {
        const runtimeWaypoints = getMissionRuntimeWaypoints(data);
        if (runtimeWaypoints.length) {
          waypoints = applyTypeOverrides(runtimeWaypoints.slice(), '__RUNTIME_PLAYER__');
        }
      }
      const cmdsBlockHtml = formatDtcCmdsBlockHtml(missionRoot, selected);
      const page = getDtcPageBySelection(selected);
      const canonicalModel = getDtcCanonicalRouteModel(missionRoot);
      const routeNameByKey = {};
      if (canonicalModel && Array.isArray(canonicalModel.routes)) {
        canonicalModel.routes.forEach(function (route, idx) {
          const key = String((route && route.key) || ('R' + String(idx + 1))).toUpperCase();
          if (!isValidDtcRouteKey(key)) return;
          const label = String((route && route.name) || key).trim();
          if (label) routeNameByKey[key] = label;
        });
      }
      const missionButtons = (isAh64 ? availableMissions : ['M1'])
        .map(function (m) {
          return '<button type="button" class="fltPlanPageBtn' + (missionKey === m ? ' active' : '') + '" data-dtc-mission="' + m + '">' + m + '</button>';
        })
        .join('');

      const routeButtons = availableRoutes.map(function (r) {
        const routeLabel = String(routeNameByKey[r] || '').trim();
        const title = routeLabel && routeLabel.toUpperCase() !== r
          ? (' title="' + escapeHtml(routeLabel) + '"')
          : '';
        return '<button type="button" class="fltPlanPageBtn' + (routeKey === r ? ' active' : '') + '" data-dtc-route="' + r + '"' + title + '>' + r + '</button>';
      }).join('');
      const pageSwitcherHtml = '<span class="fltPlanPageSwitcher"><button type="button" class="fltPlanPageBtn' + (page === 1 ? ' active' : '') + '" data-dtc-page="1">NAVLOG</button><button type="button" class="fltPlanPageBtn' + (page === 2 ? ' active' : '') + '" data-dtc-page="2">COM/ROUTE</button><button type="button" class="fltPlanPageBtn' + (page === 3 ? ' active' : '') + '" data-dtc-page="3">STORES/AID</button></span>'
        + (isAh64 ? ('<span class="fltPlanPageSwitcher">' + missionButtons + '</span>') : '')
        + '<span class="fltPlanPageSwitcher">' + routeButtons + '</span>';
      if (page === 3) {
        return formatStoresPageHtml(pageSwitcherHtml, data);
      }
      if (page === 2) {
        return formatDtcPage2Html(missionRoot, pageSwitcherHtml, allWaypoints, overlayData, selected);
      }
      return renderFlightPlanBoardHtml(selected, overlayData, getDtcDisplayName(selected) + ' ' + missionKey + ' ' + routeKey, 'DTC JSON', getPathFileName(selected), waypoints, cmdsBlockHtml, pageSwitcherHtml);
    }

    function formatRouteToolTable(root, selected) {
      const presets = (root && typeof root === 'object') ? root : null;
      if (!presets) return '';

      const routeNames = Object.keys(presets);
      if (!routeNames.length) return '';

      const lines = [];
      lines.push('ROUTE NAME : ' + getDtcDisplayName(selected));
      lines.push('ROUTE FILE : ' + getPathFileName(getFltPlnPath(selected)));
      lines.push('PATH       : ' + getFltPlnPath(selected));
      lines.push('');

      routeNames.sort(function (a, b) { return String(a).localeCompare(String(b)); });
      routeNames.forEach(function (routeName) {
        const route = presets[routeName] || {};
        const wpKeys = Object.keys(route)
          .filter(function (k) { return /^\d+$/.test(String(k)); })
          .sort(function (a, b) { return parseInt(a, 10) - parseInt(b, 10); });

        lines.push('ROUTE: ' + routeName);
        lines.push('WP  TYPE           ALT(ft)   ETA       X             Y');
        lines.push('--- -------------- -------- -------- ------------ ------------');

        if (!wpKeys.length) {
          lines.push('No waypoints found.');
          lines.push('');
          return;
        }

        wpKeys.forEach(function (wk) {
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

    function formatDtcTabContentHtml(data) {
      try {
        const files = Array.isArray(data.DtcFiles) ? data.DtcFiles : [];
        const selected = String(data.DtcSelectedFile || '');
        const jsonText = String(data.DtcJson || '').trim();
        const sourceType = String(data.DtcSourceType || '').toUpperCase();
        const runtimeWaypoints = getMissionRuntimeWaypoints(data);

        const hasExplicitPlanSelection = !!selected && runtimeFlightPlanUserOverride && selected !== '__RUNTIME_PLAYER__';
        if (!hasExplicitPlanSelection && runtimeWaypoints.length) {
          return formatMissionRuntimeFlightPlanHtml(data, runtimeWaypoints);
        }

        if (!files.length) {
          return '<div class="fltPlanMessage">No valid FLT PLN files found in Saved Games/DCS*/DTC (DTC) or Saved Games/DCS*/Config/RouteToolPresets/&lt;ActiveTerrain&gt;.lua (RouteTool).\n\nExport DTC or save RouteTool presets, then click Refresh FLT PLN.</div>';
        }

        if (!selected) {
          return '<div class="fltPlanMessage">Select a FLT PLN file from the file list to load data.</div>';
        }

        if (!jsonText) {
          return '<div class="fltPlanMessage">Selected FLT PLN file is valid but has no loadable content.</div>';
        }

        let parsed;
        try {
          parsed = JSON.parse(jsonText);
        } catch (_) {
          return '<div class="fltPlanMessage">Failed to parse selected FLT PLN payload.</div>';
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

        return '<pre class="fltPlanPlain">' + escapeHtml(formatDtcTabContent(data)) + '</pre>';
      } catch (ex) {
        const msg = (ex && ex.message) ? String(ex.message) : 'Unknown FLT PLN render error';
        return '<div class="fltPlanMessage">FLT PLN render error: ' + escapeHtml(msg) + '</div>';
      }
    }

    function resolveActiveSaMapContext(data) {
      const model = data || latestData || {};
      const selected = getActiveFlightPlanSelection(model);
      if (!selected) return null;

      if (selected === '__RUNTIME_PLAYER__') {
        const runtimeRows = applyTypeOverrides(getMissionRuntimeWaypoints(model).slice(), '__RUNTIME_PLAYER__');
        return {
          selected: '__RUNTIME_PLAYER__',
          sourceType: 'RUNTIME',
          root: null,
          overlays: getDtcMapOverlays(null, getDtcRouteBySelection('__RUNTIME_PLAYER__'), model),
          waypoints: runtimeRows,
        };
      }

      const sourceType = String(model.DtcSourceType || '').toUpperCase();
      const jsonText = String(model.DtcJson || '').trim();
      if (!jsonText) return null;

      let parsed;
      try {
        parsed = JSON.parse(jsonText);
      } catch (_) {
        return null;
      }

      const root = (parsed && parsed.data && typeof parsed.data === 'object') ? parsed.data : parsed;
      if (!root || typeof root !== 'object') return null;

      let effectiveSourceType = sourceType;
      if (effectiveSourceType !== 'DTC' && effectiveSourceType !== 'RTE') {
        const inferredDtcWaypoints = getDtcWaypoints(root);
        if (Array.isArray(inferredDtcWaypoints) && inferredDtcWaypoints.length) {
          effectiveSourceType = 'DTC';
        } else {
          const rootKeys = Object.keys(root);
          const hasRouteLikeChildren = rootKeys.some(function (name) {
            const child = root[name];
            if (!child || typeof child !== 'object') return false;
            return Object.keys(child).some(function (k) { return /^\d+$/.test(String(k)); });
          });
          if (hasRouteLikeChildren) {
            effectiveSourceType = 'RTE';
          }
        }
      }

      if (effectiveSourceType === 'RTE') {
        const presets = root;
        const routeNames = Object.keys(presets).sort(function (a, b) { return String(a).localeCompare(String(b)); });
        let primaryRouteName = String(routeNames[0] || getDtcDisplayName(selected) || '-');
        let primaryRoute = presets[primaryRouteName] || {};
        let primaryWaypoints = getRouteWaypoints(primaryRoute);
        if (!primaryWaypoints.some(function (wp) { return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum)); })) {
          for (let i = 0; i < routeNames.length; i++) {
            const candidateName = String(routeNames[i] || '');
            const candidateRoute = presets[candidateName] || {};
            const candidateRows = getRouteWaypoints(candidateRoute);
            if (candidateRows.some(function (wp) { return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum)); })) {
              primaryRouteName = candidateName;
              primaryRoute = candidateRoute;
              primaryWaypoints = candidateRows;
              break;
            }
          }
        }
        return {
          selected: selected,
          sourceType: 'RTE',
          root: root,
          overlays: getDtcMapOverlays(root, getDtcRouteBySelection(selected), model),
          waypoints: primaryWaypoints,
        };
      }

      if (effectiveSourceType === 'DTC') {
        const sourceType = String((root && root.type) || '').toUpperCase();
        const isAh64 = sourceType.indexOf('AH-64') >= 0 || sourceType.indexOf('APACHE') >= 0;
        const missionKey = isAh64 ? getDtcMissionBySelection(selected) : 'M1';
        const missionRoot = isAh64 ? resolveAh64MissionRoot(root, missionKey) : root;

        const allWaypoints = getDtcWaypoints(missionRoot);
        const availableRoutes = getDtcAvailableRoutes(missionRoot, allWaypoints);
        let routeKey = getDtcRouteBySelection(selected);
        if (availableRoutes.indexOf(routeKey) < 0) {
          routeKey = availableRoutes[0] || 'R1';
        }
        const modelWithMission = Object.assign({}, model || {}, { __dtcMissionKey: missionKey });
        const overlays = getDtcMapOverlays(missionRoot, routeKey, modelWithMission);
        const isF14 = isF14DtcContext(missionRoot, model);
        let waypoints = applyTypeOverrides(filterDtcWaypointsByRoute(missionRoot, allWaypoints, routeKey), selected);
        if (isF14 && routeKey === 'R1') {
          const runtimeWaypoints = getMissionRuntimeWaypoints(model);
          if (runtimeWaypoints.length) {
            waypoints = applyTypeOverrides(runtimeWaypoints.slice(), '__RUNTIME_PLAYER__');
          }
        }

        if (!waypoints.some(function (wp) { return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum)); })) {
          for (let i = 0; i < availableRoutes.length; i++) {
            const candidateRouteKey = String(availableRoutes[i] || '').toUpperCase();
            if (!candidateRouteKey || candidateRouteKey === routeKey) continue;
            const candidateRows = applyTypeOverrides(filterDtcWaypointsByRoute(root, allWaypoints, candidateRouteKey), selected);
            if (candidateRows.some(function (wp) { return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum)); })) {
              routeKey = candidateRouteKey;
              waypoints = candidateRows;
              setDtcRouteBySelection(selected, routeKey);
              break;
            }
          }
        }

        return {
          selected: selected,
          sourceType: 'DTC',
          root: root,
          overlays: overlays,
          waypoints: waypoints,
        };
      }

      return null;
    }

    function formatDtcTabContent(data) {
      const files = Array.isArray(data.DtcFiles) ? data.DtcFiles : [];
      const selected = String(data.DtcSelectedFile || '');
      const jsonText = String(data.DtcJson || '').trim();
      const sourceType = String(data.DtcSourceType || '').toUpperCase();

      if (!files.length) {
        return 'No valid FLT PLN files found in Saved Games/DCS*/DTC (DTC) or Saved Games/DCS*/Config/RouteToolPresets/<ActiveTerrain>.lua (RouteTool).\n\nExport DTC or save RouteTool presets, then click Refresh FLT PLN.';
      }

      if (!selected) {
        return 'Select a FLT PLN file from the file list to load data.';
      }

      if (!jsonText) {
        return 'Selected FLT PLN file is valid but has no loadable content.';
      }

      let parsed;
      try {
        parsed = JSON.parse(jsonText);
      } catch (_) {
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

      if (!rows.length) {
        return 'Selected FLT PLN file contains no tabular fields.';
      }

      const maxKeyWidth = rows.reduce(function (acc, r) { return Math.max(acc, String(r.key || '').length); }, 8);
      const keyWidth = clamp(maxKeyWidth + 1, 16, 56);
      const lines = [];
      const sourceLabel = sourceType === 'RTE' ? 'ROUTE FILE' : 'DTC FILE';
      lines.push(sourceLabel + ' : ' + getDtcDisplayName(selected));
      lines.push('PATH     : ' + selected);
      lines.push('');
      lines.push('FIELD'.padEnd(keyWidth) + 'VALUE');
      lines.push('-'.repeat(keyWidth) + '-----');
      rows.forEach(function (r) {
        const key = String(r.key || '-');
        const value = String(r.value || '-').replace(/\s+/g, ' ').trim();
        lines.push(key.padEnd(keyWidth) + value);
      });

      if (rows.length >= 220) {
        lines.push('');
        lines.push('... output truncated ...');
      }

      return lines.join('\n');
    }

    function escapeHtml(text) {
      return String(text || '')
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
    }

    function formatKeywordReferenceHtml(data, tab) {
      const text = formatKeywordReference(data, tab);
      if (text === 'No keyword reference for this tab.' || text === 'No keywords for this tab yet.') {
        return escapeHtml(text);
      }

      const rows = text.split('\n').filter(function (x) { return String(x).trim() !== ''; });
      if (!rows.length) {
        return 'No keywords for this tab yet.';
      }

      const groups = getKeywordGroupsForTab(data, tab, rows);
      if (!groups.length) return escapeHtml(text);

      const blocks = groups.map(function (group) {
        const items = Array.isArray(group.items) ? group.items : [];
        const splitIndex = Math.ceil(items.length / 2);
        const leftRows = items.slice(0, splitIndex);
        const rightRows = items.slice(splitIndex);
        const left = leftRows.map(escapeHtml).join('<br>');
        const right = rightRows.map(escapeHtml).join('<br>');

        return '<div class="kwGroup"><div class="kwGroupTitle">'
          + escapeHtml(group.title)
          + '</div><div class="kwCols"><div class="kwCol">'
          + left
          + '</div><div class="kwCol">'
          + right
          + '</div></div></div>';
      });

      return '<div class="keywordsGroups">' + blocks.join('') + '</div>';
    }

    function formatEfbTabSummaryContent(data) {
      const efb = (data && data.Efb) || {};
      const state = String(efb.State || 'Signed out');
      const message = String(efb.Message || '');
      const selectedAirport = resolveEfbSelectedAirport(data);
      const lines = [];
      lines.push('Navigraph EFB');
      lines.push('State: ' + state);
      lines.push('Feature enabled: ' + (efb.FeatureEnabled ? 'Yes' : 'No'));
      lines.push('Auth blob present: ' + (efb.HasAuthBlob ? 'Yes' : 'No'));
      lines.push('Auth readable: ' + (efb.AuthReadable ? 'Yes' : 'No'));
      if (selectedAirport) lines.push('Airport context: ' + selectedAirport);
      if (message) lines.push(message);

      if (state === 'EFB loaded') {
        lines.push('');
        lines.push('EFB loaded for this spike.');
        lines.push('Navigraph chart API rendering will be added in a later staged step.');
      }

      if (state === 'Ready/no live DCS') {
        lines.push('');
        lines.push('Data rendering is gated until a live DCS mission session is available.');
      }

      return lines.join('\n');
    }

    function formatLogTabSummaryContent(data, server) {
      const parts = [];
      const briefing = String((server && server.MissionBriefing) || '').trim();
      const details = String((server && server.MissionDetails) || '').trim();
      if (briefing) { parts.push('Mission Briefing:\n' + briefing); }
      if (details) { parts.push('Mission Details:\n' + details); }
      const log = getMergedLog(data.Logs, 'LOG');
      if (log) { parts.push('Log:\n' + log); }
      return parts.length ? parts.join('\n\n') : 'No log data yet.';
    }

    function formatNotesTabSummaryContent(data) {
      const notes = String(data.NotesBuffer || '').trim();
      return notes || 'No notes yet.';
    }

    function formatAiCrewTabSummaryContent(data) {
      const aiCrewLog = getMergedLogByCategories(data.Logs, getAiCrewCategories(data));
      const synthetic = [];
      if (aiCrewLog) synthetic.push(aiCrewLog);
      if (data.ActiveCategory === 'AI CREW') {
        synthetic.push('Active AI crew interaction detected.');
      }
      const combined = synthetic.join('\n');
      return combined ? ('Log:\n' + combined) : 'No AI CREW log data yet.';
    }

    function getTabContentRenderers() {
      return {
        'EFB': function (data) { return formatEfbTabSummaryContent(data); },
        'LOG': function (data, server) { return formatLogTabSummaryContent(data, server); },
        'NOTES': function (data) { return formatNotesTabSummaryContent(data); },
        'DTC': function (data) { return formatDtcTabContent(data); },
        'AI CREW': function (data) { return formatAiCrewTabSummaryContent(data); },
      };
    }

    function formatTabContent(data, tab) {
      const server = (data && data.Server) || {};
      const atcMetars = (server && server.AtcMetars) || {};
      const renderer = getTabContentRenderers()[tab];
      if (renderer) {
        return renderer(data, server);
      }

      const parts = [];

      const log = getMergedLog(data.Logs, tab);
      const units = getMergedList(data.Units, tab);
      const details = getMergedList(data.UnitDetails, tab);
      const unitLines = units.slice();
      if (log) parts.push('Log:\n' + log);
      if (tab === 'ATC') {
        const metarIdx = unitLines.findIndex(function (u) {
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
      if (unitLines.length) parts.push('Units:\n' + unitLines.map(function (u) { return '  ' + u; }).join('\n'));
      if (details.length) parts.push('Unit Details:\n' + details.map(function (u) { return '  ' + u; }).join('\n'));

      let outputParts = parts;
      if (tab === 'JTAC') {
        function normalizeMgrsInLine(lineText) {
          const src = String(lineText || '');
          if (!src) return src;
          return src.replace(/\b([A-HJ-NP-Z]{2})\s*(\d{4,5})\s*(\d{4,5})\b/gi, function (full) {
            return normalizeMgrsTokenDisplay(full, ((data && data.Server) || {}).Theater || lastKnownTheater || '');
          });
        }

        outputParts = parts.map(function (block) {
          return String(block || '')
            .split(/\r?\n/)
            .map(function (line) { return normalizeMgrsInLine(line); })
            .join('\n');
        });
      }

      let text = outputParts.length ? outputParts.join('\n\n') : 'No data for this tab yet.';

      function metersToSmToken(visMeters) {
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
        text = text.replace(/((?:METAR:\s+|METAR\s+)[^\n]*?)\b(\d{4}|9999)\b(\s+.*?\s+)Q(\d{4})\b/g, function (_, prefix, vism, middle, qhpa) {
          const visSm = metersToSmToken(vism);
          const hpa = parseInt(qhpa, 10);
          if (!isFinite(hpa)) return _;
          const inhg = (hpa * 0.0295299830714).toFixed(2);
          const visToken = visSm || vism;
          return prefix + visToken + middle + 'A' + inhg;
        });

        text = text.replace(/((?:METAR:\s+|METAR\s+)[^\n]*?\bCAVOK\b[^\n]*?\s+)Q(\d{4})\b/g, function (_, prefix, qhpa) {
          const hpa = parseInt(qhpa, 10);
          if (!isFinite(hpa)) return _;
          const inhg = (hpa * 0.0295299830714).toFixed(2);
          return prefix + 'A' + inhg;
        });
      }

      if (tab === 'AWACS') {
        text = decorateAwacsSamSelectionText(text, data);
      }

      return text;
    }

    function resolveAtcMetarKey(unitLine, atcMetars) {
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

      const keys = Object.keys(map).sort(function (a, b) { return String(b).length - String(a).length; });
      for (let i = 0; i < keys.length; i++) {
        const k = keys[i];
        if (!k) continue;
        if (upperLine.indexOf(String(k).toUpperCase()) >= 0) return k;
      }

      return '';
    }

    function getClickedLineFromEvent(ev, container) {
      if (!ev || !container) return '';

      const fullText = String(container.textContent || '');
      if (!fullText) return '';

      let offset = -1;
      if (document.caretPositionFromPoint) {
        const pos = document.caretPositionFromPoint(ev.clientX, ev.clientY);
        if (pos && pos.offsetNode) {
          const r = document.createRange();
          r.selectNodeContents(container);
          r.setEnd(pos.offsetNode, pos.offset);
          offset = r.toString().length;
        }
      } else if (document.caretRangeFromPoint) {
        const range = document.caretRangeFromPoint(ev.clientX, ev.clientY);
        if (range) {
          const r = document.createRange();
          r.selectNodeContents(container);
          r.setEnd(range.startContainer, range.startOffset);
          offset = r.toString().length;
        }
      }

      if (offset < 0 || offset > fullText.length) {
        return '';
      }

      let start = fullText.lastIndexOf('\n', offset - 1);
      start = (start < 0) ? 0 : (start + 1);
      let end = fullText.indexOf('\n', offset);
      if (end < 0) end = fullText.length;

      return fullText.substring(start, end).trim();
    }

    function renderTabs(data) {
      const tabsEl = document.getElementById('tabs');
      tabsEl.innerHTML = '';
      const visibleTabs = getVisibleTabs(data);
      const active = normalizeActiveCategory(data.ActiveCategory, data);
      const activeChanged = !!active && active !== lastObservedServerActiveCategory;
      if (activeChanged) {
        lastObservedServerActiveCategory = active;
      }

      if (visibleTabs.indexOf(active) >= 0) {
        if (autoBrowse) {
          selectedTab = active;
        } else if (activeChanged && active !== selectedTab && isExplicitServerCategoryChange(data)) {
          // Allow explicit server-side page commands to move tabs even when Auto Browse is off.
          selectedTab = active;
        }
      }
      if (visibleTabs.indexOf(selectedTab) < 0) selectedTab = 'LOG';

      visibleTabs.forEach(function (tab) {
        const btn = document.createElement('button');
        btn.className = 'tab ' + tabCssClass(tab) + (tab === selectedTab ? ' active' : '');
        btn.textContent = tabLabel(tab);
        btn.onclick = function () {
          if (autoBrowse) {
            autoBrowse = false;
            persistAutoBrowsePreference();
            applyAutoBrowseUi();
          }
          setSelectedTab(tab);
        };
        tabsEl.appendChild(btn);
      });
    }

    function performRender(data) {
      latestData = data;

      const displayData = getDisplayData(data);
      applyMetarUnitAutoPreference(displayData, false);
      const efbGate = selectedTab === 'EFB' ? getEfbModuleGateState(displayData) : null;

      if (selectedTab === 'EFB') {
        const activeEl = document.activeElement;
        const activeId = activeEl && activeEl.id ? String(activeEl.id) : '';
        const isEfbSelectFocused = (activeId === 'efbAirportSelect' || activeId === 'efbChartSelect');
        if (isEfbSelectFocused && !efbUiDirty && efbGate && efbGate.allowed) {
          return;
        }
      }

      updateFakeMissionControlsUi();
      maybeResetFlightPlanStateForMission(data);
      const server = displayData && displayData.Server ? displayData.Server : {};
      const aircraftId = String(server.Aircraft || '').trim();
      const aircraftUpper = aircraftId.toUpperCase();
      const hasAircraft = aircraftId.length > 0 && aircraftUpper !== '----';
      const moduleConnected = !!server.ModuleConnected;
      if (moduleConnected && !efbSaLastModuleConnected) {
        clearAllEfbSaHistoryTracks();
      }
      efbSaLastModuleConnected = moduleConnected;
      const renderTheater = String(server.Theater || '').trim();
      if (renderTheater) {
        lastKnownTheater = renderTheater;
      }
      const haveMission = moduleConnected && (hasAircraft || !!(server.MissionTitle || server.Theater));
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

      if (fakeMissionEnabled) {
        statusText = 'SIM TEST MODE ACTIVE';
        statusLevel = 'warning';
      }

      setStatus(statusText, statusLevel);
      applyAutoAtaRecPulseUi();

      const dlinkBox = document.getElementById('dlinkOn');
      if (dlinkBox) {
        dlinkBox.disabled = false;
        dlinkBox.checked = !!dlinkOnEnabled;
      }

      const quickBar = document.getElementById('fltPlanQuickBar');
      if (quickBar) {
        quickBar.className = 'fltPlanQuickBar';
      }
      const fileQuickBtn = document.getElementById('fltPlanFileBtn');
      if (fileQuickBtn) {
        fileQuickBtn.className = selectedTab === 'DTC' ? 'fltPlanQuickBtn' : 'fltPlanQuickBtn hidden';
      }
      const sessionQuickBtn = document.getElementById('fltPlanSessionBtn');
      if (sessionQuickBtn) {
        sessionQuickBtn.className = 'fltPlanQuickBtn';
      }
      updateFastAvBusToggleUi();
      updateFastOwnshipToggleUi();
      if (selectedTab !== 'DTC') {
        setFltPlanFilesOverlayOpen(false);
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

      const sessionPanel = document.querySelector('.panel.session');
      if (sessionPanel && sessionPanel.classList) {
        sessionPanel.classList.add('hidden');
        sessionPanel.classList.add('legacyControls');
      }

      const sessionOverlayText = document.getElementById('fltPlanSessionOverlayText');
      if (sessionOverlayText) {
        sessionOverlayText.textContent = document.getElementById('session').textContent || '';
      }

      renderTabs(displayData);
      updateDtcControls(displayData);
      if (fltPlanFilesOverlayOpen) {
        renderFltPlanFilesOverlay(displayData);
      }
      applyCurrentTabKeywordsSplit();
      const drawOverlayBtn = document.getElementById('drawOverlayToggleBtn');
      if (drawOverlayBtn) {
        const showDrawBtn = selectedTab === 'NOTES';
        drawOverlayBtn.classList.add('overlayToggleBtn', 'drawOverlayToggleBtn');
        drawOverlayBtn.classList.toggle('hiddenFloatingBtn', !showDrawBtn);
      }
      if (selectedTab !== 'NOTES') {
        setDrawOverlayOpen(false);
      }
      document.body.classList.toggle('notes-tab', selectedTab === 'NOTES');
      document.body.classList.toggle('flt-plan-tab', selectedTab === 'DTC');
      document.body.classList.toggle('efb-tab', selectedTab === 'EFB');
      if (helpOverlayOpen && helpOverlayLoadedTab !== selectedTab) {
        const titleEl = document.getElementById('helpOverlayTitle');
        if (titleEl) {
          titleEl.textContent = tabLabel(selectedTab) + ' Help';
        }
        loadHelpOverlayContent(selectedTab);
      }
      if (selectedTab !== 'NOTES' || !drawModeEnabled) {
        setDrawInteractionInNotes(false);
        clearDrawModeDisableTimer();
      }
      updateDrawModeToggleUi();
      document.getElementById('tabTitle').textContent = 'Tab: ' + tabLabel(selectedTab);
      const tabBody = document.getElementById('tabBody');
      let activeDtcSelection = '';
      const efbSaMapMode = selectedTab === 'EFB' && normalizeEfbViewerMode(efbViewerMode) === 'sa-map';

      if (!efbSaMapMode && typeof disposeOpenFreeMapInstances === 'function') {
        disposeOpenFreeMapInstances();
      }

      if (selectedTab === 'DTC') {
        activeDtcSelection = getActiveFlightPlanSelection(displayData);
        tabBody.className = 'content mainContent fltPlanContent';
        tabBody.innerHTML = formatDtcTabContentHtml(displayData);
        if (activeDtcSelection) {
          updateDtcRouteButtonUi(activeDtcSelection);
        }
      } else if (selectedTab === 'EFB') {
        tabBody.className = 'content mainContent';
        const efbGate = getEfbModuleGateState(displayData);
        if (!efbGate.allowed) {
          efbLoadingOverlayMessage = '';
          efbAvailableAirports = null;
          efbAirportsLoadPromise = null;
          efbAirportEntries = {};
          efbChartsByAirport = {};
          efbChartsLastFetchMsByAirport = {};
          efbChartsLoadPromiseByAirport = {};
          efbSelectedChartByAirport = {};
          efbManuallySelectedAirport = '';
          efbLastResolvedAirport = '';
          setEfbLoadingOverlay('');
          tabBody.innerHTML = '<div class="efbEmpty">' + escapeHtml(getEfbNoModuleMessage()) + '</div>';
          efbUiDirty = true;
          efbDrawerOpen = false;
          efbSelectionFlowActive = false;
          if (typeof disposeOpenFreeMapInstances === 'function') {
            disposeOpenFreeMapInstances();
          }
        } else if (!Array.isArray(efbAvailableAirports)) {
          setEfbLoadingOverlay('Loading EFB airports...');
            if (!document.getElementById('efbAirportToggleBtn')) {
              let efbHtml = '';
              try {
                efbHtml = formatEfbTabContentHtml(displayData);
              } catch (e) {
                const em = (e && e.message) ? String(e.message) : 'Unknown EFB render error';
                setStatus('EFB render error: ' + em, 'warning');
                efbHtml = '<div class="efbEmpty">EFB render error: ' + escapeHtml(em) + '</div>';
              }
              tabBody.innerHTML = efbHtml;
            bindEfbInteractions(displayData);
            efbUiDirty = false;
          }
          ensureEfbAirportsLoaded().then(function () {
            setEfbLoadingOverlay('');
            if (latestData && selectedTab === 'EFB') {
              const inSaMapMode = normalizeEfbViewerMode(efbViewerMode) === 'sa-map';
              if (inSaMapMode) {
                const saMapContext = resolveActiveSaMapContext(latestData);
                if (saMapContext && saMapContext.selected && typeof applyOpenFreeMapOwnshipCamera === 'function') {
                  applyOpenFreeMapOwnshipCamera(saMapContext.selected, latestData);
                }
              } else {
                efbUiDirty = true;
                render(latestData);
              }
            }
          }).catch(function () {
            setEfbLoadingOverlay('');
          });
        } else {
          if (efbAutoSelectOnEnter) {
            efbManuallySelectedAirport = '';
            applyEfbAutoNearestAirport(displayData);
            efbAutoSelectOnEnter = false;
            efbUiDirty = true;
          }

          const airportKey = getEfbAirportKey(displayData);

          if (!efbChartsByAirport[airportKey]) {
            setEfbLoadingOverlay('Loading EFB charts...');
            if (!document.getElementById('efbAirportToggleBtn')) {
              let efbHtml = '';
              try {
                efbHtml = formatEfbTabContentHtml(displayData);
              } catch (e) {
                const em = (e && e.message) ? String(e.message) : 'Unknown EFB render error';
                setStatus('EFB render error: ' + em, 'warning');
                efbHtml = '<div class="efbEmpty">EFB render error: ' + escapeHtml(em) + '</div>';
              }
              tabBody.innerHTML = efbHtml;
              bindEfbInteractions(displayData);
              efbUiDirty = false;
            }
            ensureEfbChartsLoaded(displayData).then(function () {
              setEfbLoadingOverlay('');
              if (latestData && selectedTab === 'EFB') {
                const inSaMapMode = normalizeEfbViewerMode(efbViewerMode) === 'sa-map';
                if (inSaMapMode) {
                  const saMapContext = resolveActiveSaMapContext(latestData);
                  if (saMapContext && saMapContext.selected && typeof applyOpenFreeMapOwnshipCamera === 'function') {
                    applyOpenFreeMapOwnshipCamera(saMapContext.selected, latestData);
                  }
                } else {
                  efbUiDirty = true;
                  render(latestData);
                }
              }
            }).catch(function () {
              setEfbLoadingOverlay('');
            });
          } else {
            setEfbLoadingOverlay('');
            const isSaMapMode = normalizeEfbViewerMode(efbViewerMode) === 'sa-map';
            const hasRenderedEfb = !!document.getElementById('efbAirportToggleBtn');
            if (!hasRenderedEfb || efbUiDirty) {
              let efbHtml = '';
              try {
                efbHtml = formatEfbTabContentHtml(displayData);
              } catch (e) {
                const em = (e && e.message) ? String(e.message) : 'Unknown EFB render error';
                setStatus('EFB render error: ' + em, 'warning');
                efbHtml = '<div class="efbEmpty">EFB render error: ' + escapeHtml(em) + '</div>';
              }
              tabBody.innerHTML = efbHtml;
              bindEfbInteractions(displayData);
              efbLastResolvedAirport = airportKey;
              efbUiDirty = false;
          } else if (isSaMapMode && efbDrawerOpen && efbDrawerMode === 'user-waypoints') {
            const list = document.getElementById('efbDrawerList');
            if (list && typeof bindEfbInteractions === 'function') {
              bindEfbInteractions(displayData);
            }
            }

            if (isSaMapMode) {
              const saMapContext = resolveActiveSaMapContext(displayData);
              if (saMapContext && saMapContext.selected) {
                const saMapState = buildSaMapRenderState(saMapContext.waypoints, displayData, saMapContext.selected, saMapContext.overlays, saMapContext.root, false);

                if (saMapState && saMapState.openFreeMapPayload) {
                  const synced = syncOpenFreeMapForSelection(saMapContext.selected, saMapState.openFreeMapPayload);
                  if (!synced) {
                    if (typeof initializeOpenFreeMapInstances === 'function') {
                      initializeOpenFreeMapInstances();
                    }
                  }
                } else {
                  const fallbackEl = document.querySelector('#efbChartViewport .fltPlanOpenMapFallback');
                  if (fallbackEl) {
                    fallbackEl.style.display = 'block';
                  }
                }

                if (saMapState && saMapState.openFreeMapPayload && typeof initializeOpenFreeMapInstances === 'function') {
                  const mapHostEl = document.querySelector('#efbChartViewport .fltPlanOpenMapHost');
                  if (mapHostEl && !mapHostEl.firstChild) {
                    if (typeof initializeOpenFreeMapInstances === 'function') {
                      initializeOpenFreeMapInstances();
                    }
                  }
                }

                if (typeof applyOpenFreeMapOwnshipCamera === 'function') {
                  applyOpenFreeMapOwnshipCamera(saMapContext.selected, displayData);
                }

                const readoutEl = document.getElementById('efbSaMapBraReadout');
                if (readoutEl && saMapState) {
                  readoutEl.textContent = String(saMapState.readout || '');
                }
              }
            }
          }
        }
      } else {
        efbDrawerOpen = false;
        efbDrawerMode = 'airport';
        efbSelectionFlowActive = false;
        tabBody.className = 'content mainContent';
        tabBody.textContent = formatTabContent(displayData, selectedTab);
      }
      applyAutoAtaRecPulseUi();
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
      if (selectedTab === 'DTC' || selectedTab === 'EFB') {
        if (keywordPanelEl) keywordPanelEl.style.display = 'none';
      } else {
        if (keywordPanelEl) keywordPanelEl.style.display = 'flex';
        document.getElementById('keywordTitle').textContent = 'Keywords: ' + tabLabel(selectedTab) + aiCrewPhaseSuffix;
        updateKeywordBodyHtml(formatKeywordReferenceHtml(displayData, selectedTab));
      }

      if (selectedTab === 'EFB' && normalizeEfbViewerMode(efbViewerMode) === 'sa-map') {
        const saMapContext = resolveActiveSaMapContext(displayData);
        const fallbackSelection = '__EFB_OWNSHIP__';
        const selectionKey = saMapContext && saMapContext.selected
          ? String(saMapContext.selected)
          : (!fakeMissionEnabled && getEfbModuleGateState(displayData).allowed ? fallbackSelection : '');
        if (selectionKey) {
          maybeCaptureEfbSaHistoryTrackSample(selectionKey, displayData);

          const saMapState = buildSaMapRenderState(
            saMapContext && Array.isArray(saMapContext.waypoints) ? saMapContext.waypoints : [],
            displayData,
            selectionKey,
            saMapContext && saMapContext.overlays ? saMapContext.overlays : {},
            saMapContext ? saMapContext.root : null,
            false);

          if (saMapState && saMapState.openFreeMapPayload) {
            const synced = syncOpenFreeMapForSelection(selectionKey, saMapState.openFreeMapPayload);
            if (!synced && typeof initializeOpenFreeMapInstances === 'function') {
              initializeOpenFreeMapInstances();
            }
          } else {
            const fallbackEl = document.querySelector('#efbChartViewport .fltPlanOpenMapFallback');
            if (fallbackEl) {
              fallbackEl.style.display = 'block';
            }
          }

          if (saMapState && saMapState.openFreeMapPayload && typeof initializeOpenFreeMapInstances === 'function') {
            const mapHostEl = document.querySelector('#efbChartViewport .fltPlanOpenMapHost');
            if (mapHostEl && !mapHostEl.firstChild) {
              initializeOpenFreeMapInstances();
            }
          }

          if (typeof applyOpenFreeMapOwnshipCamera === 'function') {
            applyOpenFreeMapOwnshipCamera(selectionKey, displayData);
          }

          const readoutEl = document.getElementById('efbSaMapBraReadout');
          if (readoutEl && saMapState) {
            readoutEl.textContent = String(saMapState.readout || '');
          }
        }
      }

      const showRawWrap = document.getElementById('showRawWrap');
      if (showRawWrap) {
        showRawWrap.style.display = debugMode ? 'inline-flex' : 'none';
      }
      if (!debugMode) {
        const showRawBox = document.getElementById('showRaw');
        if (showRawBox) showRawBox.checked = false;
        document.body.classList.remove('raw-mode');
        document.getElementById('json').className = 'hidden';
      }

      document.getElementById('json').textContent = JSON.stringify(displayData, null, 2);

      const serverMessagesEl = document.getElementById('serverMessages');
      if (serverMessagesEl) {
        const rows = Array.isArray(displayData.RawServerMessages) ? displayData.RawServerMessages : [];
        serverMessagesEl.textContent = rows.length ? rows.join('\n') : 'No server messages captured yet.';
      }

      const showServerWrap = document.getElementById('showServerWrap');
      if (showServerWrap) {
        showServerWrap.style.display = debugMode ? 'inline-flex' : 'none';
      }

      if (!debugMode) {
        showServerMessages = false;
        const showServerBox = document.getElementById('showServer');
        if (showServerBox) showServerBox.checked = false;
        if (serverMessagesEl) serverMessagesEl.className = 'hidden';
      }
    }

    function scheduleRenderFlush() {
      if (runtimeState.render.flushPending) return;
      runtimeState.render.flushPending = true;

      const flush = function () {
        runtimeState.render.flushPending = false;
        runtimeState.render.flushHandle = 0;
        const payload = runtimeState.render.scheduledData || latestData;
        runtimeState.render.scheduledData = null;
        if (!payload) return;
        render(payload);
      };

      if (typeof window !== 'undefined' && typeof window.requestAnimationFrame === 'function') {
        runtimeState.render.flushHandle = window.requestAnimationFrame(flush);
      } else {
        runtimeState.render.flushHandle = window.setTimeout(flush, 0);
      }
    }

    function requestRender(data) {
      if (data !== undefined && data !== null) {
        runtimeState.render.scheduledData = data;
      } else if (!runtimeState.render.scheduledData && latestData) {
        runtimeState.render.scheduledData = latestData;
      }
      scheduleRenderFlush();
    }

    function render(data) {
      const payload = (data !== undefined && data !== null) ? data : latestData;
      if (!payload) return;

      const nowMs = Date.now();
      if ((nowMs - runtimeState.render.lastStartUtcMs) < runtimeState.render.minGapMs) {
        runtimeState.render.scheduledData = payload;
        scheduleRenderFlush();
        return;
      }

      runtimeState.render.lastStartUtcMs = nowMs;
      performRender(payload);
    }

    function updateDtcControls(data) {
      const wrap = document.getElementById('dtcControls');
      const selector = document.getElementById('dtcSelector');
      const listEl = document.getElementById('dtcFileList');
      const selectedLabel = document.getElementById('dtcSelectedFileLabel');
      const missionClockLabel = document.getElementById('missionClockLabel');
      if (!wrap || !selector || !listEl || !selectedLabel) return;

      wrap.className = 'controls fltPlanControls hidden legacyControls';
      selector.className = 'panel dtcSelector hidden legacyControls';
      if (missionClockLabel) missionClockLabel.style.display = 'none';

      updateMissionClockLabel(data);

      const files = Array.isArray(data.DtcFiles) ? data.DtcFiles : [];
      const selected = String(data.DtcSelectedFile || '');
      selectedLabel.textContent = selected ? ('Selected: ' + getDtcDisplayName(selected)) : 'No FLT PLN selected';

      if (!files.length) {
        listEl.innerHTML = 'No FLT PLN files found.';
        applyDtcListCollapsedState(dtcListCollapsed);
        return;
      }

      const rows = [];
      files.forEach(function (filePath) {
        const fp = String(filePath || '');
        const active = selected && fp === selected;
        const type = (fp.indexOf('RTE::') === 0 || /\.(rte|lua)$/i.test(fp)) ? 'RTE' : 'DTC';
        rows.push('<button type="button" class="dtcFileItem' + (active ? ' active' : '') + '" data-file="' + encodeURIComponent(fp) + '"><span>' + escapeHtml(getDtcDisplayName(fp)) + '</span><span class="dtcFileMeta">' + type + '</span></button>');
      });
      listEl.innerHTML = rows.join('');
      applyDtcListCollapsedState(dtcListCollapsed);

      if (fltPlanFilesOverlayOpen) {
        renderFltPlanFilesOverlay(data);
      }
    }

