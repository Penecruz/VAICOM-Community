    async function setServerMessageCapture(enabled) {
      try {
        await fetch('dev/servermessages?enabled=' + (enabled ? '1' : '0'), { method: 'POST', cache: 'no-store' });
      } catch (e) {
      }
    }

    async function refreshDtcFiles() {
      runtimeFlightPlanUserOverride = false;
      invalidateRuntimeFlightPlanSnapshot();
      resetRuntimeFlightPlanState();
      try {
        await fetch('/okb/dtc/select?file=', { method: 'POST', cache: 'no-store' });
      } catch (_) {
      }
      try {
        await fetch('/okb/dtc/list', { method: 'POST', cache: 'no-store' });
      } catch (_) {
      }
      await tick();
    }

    async function selectDtcFile(filePath) {
      runtimeFlightPlanUserOverride = !!String(filePath || '').trim();
      try {
        await fetch('/okb/dtc/select?file=' + encodeURIComponent(filePath || ''), { method: 'POST', cache: 'no-store' });
      } catch (_) {
      }
      await tick();
    }

    async function configureOpenKneeboard() {
      try {
        var okb = (typeof OpenKneeboard !== 'undefined') ? OpenKneeboard : window.OpenKneeboard;
        if (okb && okb.SetPreferredPixelSize) {
          await okb.SetPreferredPixelSize(1050, 1480);
          setTimeout(function () { okb.SetPreferredPixelSize(1050, 1480); }, 300);
          setTimeout(function () { okb.SetPreferredPixelSize(1050, 1480); }, 1200);
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
        setTimeout(function () { updateCursorModeForTab(); }, 50);
        setTimeout(function () { updateCursorModeForTab(); }, 500);
      } catch (e) {
      }
    }

    async function tick() {
      try {
        const r = await fetch('state', { cache: 'no-store' });
        const j = await r.json();
        try {
          requestRender(j);
        } catch (e) {
          const msg = (e && e.message) ? String(e.message) : 'Unknown render error';
          setStatus('Render error: ' + msg, 'warning');
        }
      } catch (e) {
        setStatus('Waiting for VAICOM connection...', '');
      }
    }

    async function clockTick() {
      if (fakeMissionEnabled) return;
      try {
        const requestStartedMs = Date.now();
        const r = await fetch('clock', { cache: 'no-store' });
        const j = await r.json();
        const missionClock = Number(j && j.MissionTimeSeconds);
        const identity = String((j && j.MissionIdentity) || '');
        if (!isFinite(missionClock) || missionClock < 0) return;

        if (runtimeState.missionClock.anchorIdentity && identity && runtimeState.missionClock.anchorIdentity !== identity) {
          resetMissionClockAnchor();
        }

        runtimeState.missionClock.anchorIdentity = identity || runtimeState.missionClock.anchorIdentity;

        const hasAnchor = isFinite(Number(runtimeState.missionClock.anchorSeconds)) && runtimeState.missionClock.anchorSystemMs > 0;
        if (!hasAnchor) {
          // Lock to first reliable mission-time sample, then run locally from system elapsed time.
          setMissionClockAnchor(missionClock, requestStartedMs);
        }
      } catch (_) {
      }
    }

    document.getElementById('showRaw').addEventListener('change', function (ev) {
      applyShowRawUi(!!ev.target.checked);
    });

    document.getElementById('autoBrowse').addEventListener('change', function (ev) {
      autoBrowse = ev.target.checked;
      persistAutoBrowsePreference();
      applyAutoBrowseUi();
      if (latestData) render(latestData);
    });

    document.getElementById('nightMode').addEventListener('change', function (ev) {
      nightModeEnabled = !!ev.target.checked;
      persistNightModePreference();
      applyNightModeUi();
      refreshEfbSaMapTilesIfActive();
    });

    document.getElementById('fontSizeSlider').addEventListener('input', function (ev) {
      contentFontSizePx = clamp(parseInt(ev.target.value, 10) || 24, 18, 34);
      applyContentFontSizeUi();
      persistContentFontSizePreference();
    });

    document.getElementById('dlinkOn').addEventListener('change', function (ev) {
      dlinkOnEnabled = !!ev.target.checked;
      persistDlinkOnPreference();
      applyDlinkOnUi();
      if (latestData && (selectedTab === 'DTC' || selectedTab === 'EFB')) {
        if (selectedTab === 'EFB') efbUiDirty = true;
        render(latestData);
      }
    });

    document.getElementById('overlayEfbAdLanding').addEventListener('change', function (ev) {
      efbAdLandingEnabled = !!ev.target.checked;
      persistEfbAdLandingEnabledPreference();
      applyEfbAdLandingUi();
      if (!efbAdLandingEnabled) {
        efbAdLandingAssistState.seenAirborne = false;
        efbAdLandingAssistState.triggeredThisWowOn = false;
      }
    });

    document.getElementById('overlayAutoAtaRecSpd').addEventListener('change', function (ev) {
      autoAtaRecSpdEnabled = !!ev.target.checked;
      persistAutoAtaRecSpdEnabledPreference();
      applyAutoAtaRecSpdUi();
      if (!autoAtaRecSpdEnabled) {
        autoAtaRecPulseUntilUtc = 0;
        autoAtaRecPulseSpdText = '';
        applyAutoAtaRecPulseUi();
      }
      if (latestData && selectedTab === 'DTC') {
        render(latestData);
      }
    });

    document.getElementById('overlayAutoBrowse').addEventListener('change', function (ev) {
      autoBrowse = !!ev.target.checked;
      persistAutoBrowsePreference();
      applyAutoBrowseUi();
      if (latestData) render(latestData);
    });

    document.getElementById('overlayNightMode').addEventListener('change', function (ev) {
      nightModeEnabled = !!ev.target.checked;
      persistNightModePreference();
      applyNightModeUi();
      refreshEfbSaMapTilesIfActive();
    });

    document.getElementById('overlayDlinkOn').addEventListener('change', function (ev) {
      dlinkOnEnabled = !!ev.target.checked;
      persistDlinkOnPreference();
      applyDlinkOnUi();
      if (latestData && (selectedTab === 'DTC' || selectedTab === 'EFB')) {
        if (selectedTab === 'EFB') efbUiDirty = true;
        render(latestData);
      }
    });

    document.getElementById('overlayFontSizeSlider').addEventListener('input', function (ev) {
      contentFontSizePx = clamp(parseInt(ev.target.value, 10) || 24, 18, 34);
      applyContentFontSizeUi();
      persistContentFontSizePreference();
    });

    document.getElementById('overlayToggleBtn').addEventListener('click', function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      setSettingsOverlayOpen(!settingsOverlayOpen);
    });

    document.getElementById('drawOverlayToggleBtn').addEventListener('click', function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      setDrawOverlayOpen(!drawOverlayOpen);
    });

    document.getElementById('helpOverlayToggleBtn').addEventListener('click', function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      setHelpOverlayOpen(!helpOverlayOpen);
    });

    document.getElementById('overlayCloseBtn').addEventListener('click', function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      setSettingsOverlayOpen(false);
    });

    document.getElementById('drawOverlayCloseBtn').addEventListener('click', function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      setDrawOverlayOpen(false);
    });

    document.getElementById('helpOverlayCloseBtn').addEventListener('click', function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      setHelpOverlayOpen(false);
    });

    document.getElementById('settingsOverlay').addEventListener('click', function (ev) {
      if (ev.target && ev.target.id === 'settingsOverlay') {
        setSettingsOverlayOpen(false);
      }
    });

    document.getElementById('drawOverlay').addEventListener('click', function (ev) {
      if (ev.target && ev.target.id === 'drawOverlay') {
        setDrawOverlayOpen(false);
      }
    });

    document.getElementById('helpOverlay').addEventListener('click', function (ev) {
      if (ev.target && ev.target.id === 'helpOverlay') {
        setHelpOverlayOpen(false);
      }
    });

    document.getElementById('overlayDrawMode').addEventListener('change', function (ev) {
      const shouldEnable = !!ev.target.checked;
      if (okbDoodlesOnlyForced || selectedTab !== 'NOTES') {
        applyDrawOverlayUi();
        return;
      }

      if (shouldEnable) {
        drawModeEnabled = true;
        setDrawInteractionInNotes(true);
        scheduleDrawModeAutoOff();
      } else {
        disableDrawMode();
      }

      persistDrawModePreference();
      updateDrawModeToggleUi();
      updateCursorModeForTab();
    });

    document.getElementById('drawModeToggle').addEventListener('click', function () {
      if (okbDoodlesOnlyForced || selectedTab !== 'NOTES') return;
      if (drawModeEnabled) {
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

    document.getElementById('tabBody').addEventListener('dblclick', function () {
      if (okbDoodlesOnlyForced || selectedTab !== 'NOTES') return;
      disableDrawMode();
    });

    document.getElementById('tabBody').addEventListener('mouseenter', function () {
      if (okbDoodlesOnlyForced || selectedTab !== 'NOTES' || !drawModeEnabled) return;
      setDrawInteractionInNotes(true);
      notifyDrawActivity();
    });

    document.getElementById('tabBody').addEventListener('mouseleave', function () {
      if (okbDoodlesOnlyForced) return;
      setDrawInteractionInNotes(false);
    });

    document.getElementById('tabBody').addEventListener('pointerdown', notifyDrawActivity);
    document.getElementById('tabBody').addEventListener('pointermove', notifyDrawActivity);
    document.getElementById('tabBody').addEventListener('touchstart', notifyDrawActivity);
    document.getElementById('tabBody').addEventListener('touchmove', notifyDrawActivity);

    document.querySelector('.controls').addEventListener('mouseenter', function () {
      if (okbDoodlesOnlyForced) return;
      setDrawInteractionInNotes(false);
    });

    document.querySelector('.tabRail').addEventListener('mouseenter', function () {
      if (okbDoodlesOnlyForced) return;
      setDrawInteractionInNotes(false);
    });

    document.getElementById('tabBody').addEventListener('click', function (ev) {
      if (selectedTab === 'DTC' && latestData) {
        let node = ev.target;
        while (node && node !== this) {
          if (node.getAttribute && node.getAttribute('data-nav-edit-toggle')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-nav-edit-step') || '');
            const field = String(node.getAttribute('data-nav-edit-field') || '').toUpperCase();
            if (selected && step && field) {
              if (field === 'SPD') {
                const recStep = String(node.getAttribute('data-speed-rec-step') || '');
                if (recStep && acceptSpeedRecommendation(selected, recStep)) {
                  render(latestData);
                  return;
                }
              }
              const current = getExpandedNavEdit(selected);
              if (current.step === step && current.field === field) {
                closeExpandedNavEditAnimated(selected);
              } else {
                closeExpandedNavEditAnimated(selected);
                setExpandedNavEdit(selected, step, field);
              }
              setExpandedTimeAnchor(selected, '');
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-time-adjust-anchor')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const anchor = String(node.getAttribute('data-time-adjust-anchor') || '').toUpperCase();
            const seconds = Number(node.getAttribute('data-time-adjust-sec') || 0);
            if (selected && anchor && isFinite(seconds) && seconds !== 0) {
              setTimingAnchorBySecondsDelta(selected, anchor, seconds);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-time-anchor')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const anchor = String(node.getAttribute('data-time-anchor') || '').toUpperCase();
            if (selected && anchor) {
              const expanded = getExpandedTimeAnchor(selected);
              if (expanded === anchor) {
                setExpandedTimeAnchor(selected, '');
              } else {
                if (anchor !== 'TOT') {
                  setTakeoffTimeByAnchorFromNow(selected, anchor);
                }
                setExpandedTimeAnchor(selected, anchor);
              }
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-spd-step')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-spd-step') || '');
            const direction = Number(node.getAttribute('data-spd-dir') || 0);
            const altFeet = Number(node.getAttribute('data-spd-alt') || NaN);
            if (selected && step && isFinite(direction) && direction !== 0) {
              changeWaypointSpeedAdjustmentByDirection(selected, step, direction, altFeet);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-speed-mode-step')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-speed-mode-step') || '');
            const altFeet = Number(node.getAttribute('data-speed-alt') || NaN);
            const recStep = String(node.getAttribute('data-speed-rec-step') || '');
            if (selected && recStep) {
              if (acceptSpeedRecommendation(selected, recStep)) {
                render(latestData);
              }
              return;
            }
            if (selected && step) {
              toggleWaypointSpeedDisplayMode(selected, step, altFeet);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-alt-step')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-alt-step') || '');
            const direction = Number(node.getAttribute('data-alt-dir') || 0);
            const currentAlt = Number(node.getAttribute('data-alt-current') || NaN);
            const altMode = String(node.getAttribute('data-alt-mode') || getNavlogAltDisplayMode(selected || '__RUNTIME_PLAYER__'));
            if (selected && step && isFinite(direction) && direction !== 0) {
              changeWaypointAltitudeByDirection(selected, step, direction, currentAlt, altMode);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-type-step')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-type-step') || '');
            const delta = Number(node.getAttribute('data-type-delta') || 0);
            const currentType = String(node.getAttribute('data-type-current') || '');
            if (selected && step && isFinite(delta) && delta !== 0) {
              changeWaypointType(selected, step, delta, currentType);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-row-action')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const action = String(node.getAttribute('data-row-action') || '').toLowerCase();
            const step = String(node.getAttribute('data-row-step') || '');
            if (selected && step) {
              clearNavlogRowReveal(selected);
              if (action === 'dir') {
                setNavlogRowAction(selected, step, 'dir');
                setNavlogDirectToSource(selected, step);
                render(latestData);
              } else if (action === 'del') {
                setNavlogRowAction(selected, step, 'del');
                render(latestData);
              }
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-navlog-row')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-navlog-step') || '');
            if (selected && step) {
              const state = getFlightPlanPlanState(selected);
              const rowActionStep = stepToKey(state.rowActionStep);
              const rowActionMode = String(state.rowActionMode || '').toLowerCase();
              if (rowActionMode === 'dir' && rowActionStep && rowActionStep === step) {
                clearNavlogRowAction(selected);
                render(latestData);
                return;
              }
              if (rowActionMode === 'dir' && rowActionStep && rowActionStep !== step) {
                if (setNavlogDirectToTarget(selected, step)) {
                  clearNavlogRowAction(selected);
                  clearNavlogRowReveal(selected);
                  render(latestData);
                }
                return;
              }
              if (rowActionMode === 'del' && rowActionStep && rowActionStep === step) {
                deleteNavlogStep(selected, step);
                clearNavlogRowAction(selected);
                clearNavlogRowReveal(selected);
                render(latestData);
                return;
              }
              if (rowActionMode === 'del' && rowActionStep && rowActionStep !== step) {
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
          if (node.getAttribute && node.getAttribute('data-tot-adjust')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const seconds = Number(node.getAttribute('data-tot-adjust') || 0);
            if (selected && isFinite(seconds) && seconds !== 0) {
              setTotByMinutesDelta(selected, seconds / 60.0);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tot-adjust-sec')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const seconds = Number(node.getAttribute('data-tot-adjust-sec') || 0);
            if (selected && isFinite(seconds) && seconds !== 0) {
              setTotBySecondsDelta(selected, seconds);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tko-adjust')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const seconds = Number(node.getAttribute('data-tko-adjust') || 0);
            if (selected && isFinite(seconds) && seconds !== 0) {
              setTakeoffByMinutesDelta(selected, seconds / 60.0);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tko-adjust-sec')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const seconds = Number(node.getAttribute('data-tko-adjust-sec') || 0);
            if (selected && isFinite(seconds) && seconds !== 0) {
              setTakeoffBySecondsDelta(selected, seconds);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-dtc-page')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const page = Number(node.getAttribute('data-dtc-page') || 1);
            if (selected) {
              setDtcPageBySelection(selected, page);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-postflight-toggle')) {
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected) {
              togglePostFlightOpen(selected);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-bottom-panel-toggle')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const panel = String(node.getAttribute('data-bottom-panel-toggle') || '').toUpperCase();
            if (selected && panel) {
              const expandedPanel = getExpandedBottomPanel(selected);
              if (expandedPanel === panel) {
                closeExpandedBottomPanelAnimated(selected);
              } else {
                closeExpandedBottomPanelAnimated(selected);
                setExpandedBottomPanel(selected, panel);
              }
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-eta-step')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-eta-step') || '');
            const planned = String(node.getAttribute('data-eta-planned') || '');
            if (selected && step) {
              toggleWaypointAta(selected, step, planned);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-dtc-route')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const route = String(node.getAttribute('data-dtc-route') || 'R1');
            if (selected) {
              setDtcRouteBySelection(selected, route);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-dtc-mission')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const missionKey = String(node.getAttribute('data-dtc-mission') || 'M1');
            if (selected) {
              setDtcMissionBySelection(selected, missionKey);
              setDtcRouteBySelection(selected, 'R1');
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-ah64-comm-preset')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const preset = Number(node.getAttribute('data-ah64-comm-preset') || 1);
            if (selected && typeof setAh64CommPresetBySelection === 'function') {
              setAh64CommPresetBySelection(selected, preset);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-map-zoom')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const action = String(node.getAttribute('data-map-zoom') || '').toLowerCase();
            if (selected) {
              const handledByWebMap = (typeof handleOpenFreeMapZoomAction === 'function')
                ? handleOpenFreeMapZoomAction(selected, action)
                : false;
              if (!handledByWebMap) {
                if (action === 'in') zoomMapViewBySelection(selected, 1.2);
                else if (action === 'out') zoomMapViewBySelection(selected, 1 / 1.2);
                else if (action === 'reset') resetMapViewBySelection(selected);
                render(latestData);
              }
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-map-bg-toggle')) {
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected) {
              toggleMapBackgroundEnabledBySelection(selected);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-map-bg-toggle')) {
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected) {
              toggleMapBackgroundEnabledBySelection(selected);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-map-asset-key')) {
            const selected = getActiveFlightPlanSelection(latestData);
            if (!selected) return;
            const category = String(node.getAttribute('data-map-asset-category') || '').toUpperCase();
            if (category === 'PLAYER') return;
            const encodedKey = String(node.getAttribute('data-map-asset-key') || '');
            let assetKey = encodedKey;
            try {
              assetKey = decodeURIComponent(encodedKey);
            } catch (_) {
              assetKey = encodedKey;
            }
            const current = getMapSelectedAssetKeyBySelection(selected);
            const nextKey = current === assetKey ? '' : assetKey;
            setMapSelectedAssetKeyBySelection(selected, nextKey);
            if (String(assetKey || '').indexOf('SAMTHREAT|') === 0) {
              const selectedList = Array.isArray(efbAwacsSelectedSamThreatKeys) ? efbAwacsSelectedSamThreatKeys.slice() : [];
              const idx = selectedList.indexOf(String(assetKey || ''));
              if (idx >= 0) {
                selectedList.splice(idx, 1);
              } else {
                selectedList.push(String(assetKey || ''));
              }
              efbAwacsSelectedSamThreatKeys = selectedList;
              const activeKey = selectedList.length ? selectedList[selectedList.length - 1] : '';
              setMapSelectedAssetKeyBySelection(selected, activeKey);
            } else {
              efbAwacsSelectedSamThreatKeys = [];
            }
            setEfbDebugState({
              source: 'sa-map-click',
              saAssetKey: String(nextKey || assetKey || ''),
              saCategory: category,
              saResolvedIcao: '',
              finalAirportKey: String(getEfbAirportKey(latestData) || 'UNSET'),
            });
            if (nextKey) {
              tryPreloadEfbForBuggedAirfield(latestData, nextKey).catch(function () { });
            }
            render(latestData);
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tot-lock-step')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const step = String(node.getAttribute('data-tot-lock-step') || '');
            const checked = !!node.checked;
            if (selected && step) {
              let etaSeconds = NaN;
              try {
                const row = node.closest ? node.closest('tr') : null;
                const etaCell = row ? row.cells[7] : null;
                etaSeconds = parseEtaToSeconds((etaCell && etaCell.textContent) ? etaCell.textContent : '');
              } catch (_) { }
              setLockedTotStep(selected, step, checked, etaSeconds);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-tko-anchor')) {
            const selected = getActiveFlightPlanSelection(latestData);
            const anchor = String(node.getAttribute('data-tko-anchor') || '');
            if (selected && anchor) {
              setTakeoffTimeByAnchorFromNow(selected, anchor);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-eta-header')) {
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected) {
              setEtaStartNowForSelection(selected);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-navlog-coord-cycle')) {
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected) {
              cycleNavlogCoordDisplayMode(selected);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-navlog-alt-cycle')) {
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected) {
              cycleNavlogAltDisplayMode(selected);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-navlog-spd-cycle')) {
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected) {
              cycleNavlogSpdDisplayMode(selected);
              render(latestData);
            }
            return;
          }
          if (node.getAttribute && node.getAttribute('data-navlog-dist-cycle')) {
            const selected = getActiveFlightPlanSelection(latestData);
            if (selected) {
              cycleNavlogDistDisplayMode(selected);
              render(latestData);
            }
            return;
          }
          node = node.parentNode;
        }

        const activeSelection = getActiveFlightPlanSelection(latestData);
        if (activeSelection) {
          const expanded = getExpandedTimeAnchor(activeSelection);
          const expandedNav = getExpandedNavEdit(activeSelection);
          if (expanded) {
            const target = ev && ev.target;
            const insideTimeCell = target && target.closest && target.closest('.fltPlanTimeCell');
            if (!insideTimeCell) {
              setExpandedTimeAnchor(activeSelection, '');
              if (expandedNav.step) {
                closeExpandedNavEditAnimated(activeSelection);
              }
              render(latestData);
              return;
            }
          }

          if (expandedNav.step) {
            const target = ev && ev.target;
            const insideNavPopup = target && target.closest && target.closest('.fltPlanNavEditHost');
            if (!insideNavPopup) {
              if (!closeExpandedNavEditAnimated(activeSelection)) {
                setExpandedNavEdit(activeSelection, '', '');
              }
              if (expanded) {
                setExpandedTimeAnchor(activeSelection, '');
              }
              render(latestData);
              return;
            }
          }

          const expandedBottomPanel = getExpandedBottomPanel(activeSelection);
          if (expandedBottomPanel) {
            const target = ev && ev.target;
            const insideBottomPanel = target && target.closest && target.closest('.fltPlanBottomGrid');
            if (!insideBottomPanel) {
              closeExpandedBottomPanelAnimated(activeSelection);
              render(latestData);
              return;
            }
          }
        }
      }

      if (selectedTab === 'AWACS' && latestData) {
        const clickedLine = getClickedLineFromEvent(ev, this);
        const selectedSam = getAwacsSamThreatSelectionFromLine(latestData, clickedLine);
        if (selectedSam && selectedSam.assetKey) {
          const key = String(selectedSam.assetKey);
          const selectionKey = getSamThreatSelectionKey(key);
          const selectedList = Array.isArray(efbAwacsSelectedSamThreatKeys) ? efbAwacsSelectedSamThreatKeys.slice() : [];
          const matchIdx = selectedList.findIndex(function (k) {
            return getSamThreatSelectionKey(k) === selectionKey;
          });
          if (matchIdx >= 0) {
            for (let i = selectedList.length - 1; i >= 0; i--) {
              if (getSamThreatSelectionKey(selectedList[i]) === selectionKey) {
                selectedList.splice(i, 1);
              }
            }
          } else {
            selectedList.push(key);
          }
          efbAwacsSelectedSamThreatKeys = selectedList;
          const activeSelection = getActiveFlightPlanSelection(latestData);
          if (activeSelection) {
            const activeKey = selectedList.length ? selectedList[selectedList.length - 1] : '';
            setMapSelectedAssetKeyBySelection(activeSelection, activeKey);
          }
          render(latestData);
          return;
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
        metarPressureAutoMode = false;
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
      metarPressureAutoMode = false;
      metarPressureInHg = !metarPressureInHg;
      render(latestData);
    });

    document.getElementById('tabBody').addEventListener('pointerdown', function (ev) {
      if (!latestData || (selectedTab !== 'DTC' && selectedTab !== 'EFB')) return;
      if (selectedTab === 'EFB' && normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') return;
      const selected = getActiveFlightPlanSelection(latestData);
      if (!selected) return;

      const page = (selectedTab === 'EFB' && normalizeEfbViewerMode(efbViewerMode) === 'sa-map')
        ? 4
        : getDtcPageBySelection(selected);
      if (page === 1) {
        const row = ev.target && ev.target.closest ? ev.target.closest('tr[data-navlog-row="1"]') : null;
        if (row) {
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
      while (assetNode && assetNode !== this) {
        if (assetNode.getAttribute && assetNode.getAttribute('data-map-asset-key')) {
          const category = String(assetNode.getAttribute('data-map-asset-category') || '').toUpperCase();
          if (category !== 'PLAYER') {
            const encodedKey = String(assetNode.getAttribute('data-map-asset-key') || '');
            let assetKey = encodedKey;
            try {
              assetKey = decodeURIComponent(encodedKey);
            } catch (_) {
              assetKey = encodedKey;
            }
            const current = getMapSelectedAssetKeyBySelection(selected);
            const nextKey = current === assetKey ? '' : assetKey;
            setMapSelectedAssetKeyBySelection(selected, nextKey);
            setEfbDebugState({
              source: 'sa-map-click',
              saAssetKey: String(nextKey || assetKey || ''),
              saCategory: category,
              saResolvedIcao: '',
              finalAirportKey: String(getEfbAirportKey(latestData) || 'UNSET'),
            });
            if (nextKey) {
              tryPreloadEfbForBuggedAirfield(latestData, nextKey).catch(function () { });
            }
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
      while (node && node !== this) {
        if (node.getAttribute && node.getAttribute('data-map-canvas')) { onMap = true; break; }
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
      try { if (ev.target && ev.target.setPointerCapture) ev.target.setPointerCapture(ev.pointerId); } catch (_) { }
      ev.preventDefault();
    });

    document.getElementById('tabBody').addEventListener('pointermove', function (ev) {
      if (navlogRowDrag && navlogRowDrag.pointerId === ev.pointerId && latestData) {
        const dx = ev.clientX - navlogRowDrag.startX;
        const dy = ev.clientY - navlogRowDrag.startY;
        if (Math.abs(dx) >= 28 && Math.abs(dx) > Math.abs(dy)) {
          const step = stepToKey(navlogRowDrag.step);
          if (step) {
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

    function stopMapPanDrag(ev) {
      if (!mapPanDrag) return;
      if (ev && mapPanDrag.pointerId !== ev.pointerId) return;
      mapPanDrag = null;
    }

    function stopNavlogRowDrag(ev) {
      if (!navlogRowDrag) return;
      if (ev && navlogRowDrag.pointerId !== ev.pointerId) return;
      navlogRowDrag = null;
    }

    document.getElementById('tabBody').addEventListener('pointerup', function (ev) {
      stopNavlogRowDrag(ev);
      stopMapPanDrag(ev);
    });
    document.getElementById('tabBody').addEventListener('pointercancel', function (ev) {
      stopNavlogRowDrag(ev);
      stopMapPanDrag(ev);
    });
    document.getElementById('tabBody').addEventListener('pointerleave', function (ev) {
      stopNavlogRowDrag(ev);
      stopMapPanDrag(ev);
    });

    document.getElementById('showServer').addEventListener('change', function (ev) {
      applyShowServerUi(!!ev.target.checked);
    });

    document.getElementById('overlayShowRaw').addEventListener('change', function (ev) {
      applyShowRawUi(!!ev.target.checked);
    });

    document.getElementById('overlayShowServer').addEventListener('change', function (ev) {
      applyShowServerUi(!!ev.target.checked);
    });

    document.getElementById('dtcRefresh').addEventListener('click', function () {
      refreshDtcFiles();
    });

    document.getElementById('fltPlanFileBtn').addEventListener('click', async function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      if (selectedTab !== 'DTC') return;

      if (fltPlanFilesOverlayOpen) {
        setFltPlanFilesOverlayOpen(false);
        return;
      }

      await refreshDtcFiles();
      setFltPlanFilesOverlayOpen(true);
      if (latestData) renderFltPlanFilesOverlay(getDisplayData(latestData));
    });

    document.getElementById('fltPlanSessionBtn').addEventListener('click', function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      setFltPlanSessionOverlayOpen(!fltPlanSessionOverlayOpen);
    });

    document.getElementById('fastOwnshipToggleBtn').addEventListener('click', function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      if (fastOwnshipLongPressFired) {
        fastOwnshipLongPressFired = false;
        return;
      }
      if (!(efbSaOwnshipFast && efbSaOwnshipFast.hasPosition)) return;
      efbSaUseFastOwnshipEnabled = !efbSaUseFastOwnshipEnabled;
      persistEfbSaUseFastOwnshipPreference();
      updateFastOwnshipToggleUi();

      const selectedKey = getActiveEfbSaSelectionKey();
      if (selectedKey) {
        if (efbSaUseFastOwnshipEnabled) {
          applyFastOwnshipToSaMap(selectedKey, latestData || null);
        }
        applyOpenFreeMapOwnshipCamera(selectedKey, latestData || null);
      }
      if (latestData) render(latestData);
    });

    document.getElementById('fastOwnshipToggleBtn').addEventListener('pointerdown', function () {
      if (fastOwnshipLongPressTimerId) {
        window.clearTimeout(fastOwnshipLongPressTimerId);
        fastOwnshipLongPressTimerId = 0;
      }
      fastOwnshipLongPressFired = false;
      fastOwnshipLongPressTimerId = window.setTimeout(function () {
        fastOwnshipLongPressTimerId = 0;
        fastOwnshipLongPressFired = true;
        efbFastOwnshipDebugPanelVisible = !efbFastOwnshipDebugPanelVisible;
        updateFastOwnshipToggleUi();
        if (latestData) render(latestData);
      }, fastOwnshipLongPressMs);
    });

    function clearFastOwnshipLongPressTimer() {
      if (!fastOwnshipLongPressTimerId) return;
      window.clearTimeout(fastOwnshipLongPressTimerId);
      fastOwnshipLongPressTimerId = 0;
    }

    document.getElementById('fastOwnshipToggleBtn').addEventListener('pointerup', clearFastOwnshipLongPressTimer);
    document.getElementById('fastOwnshipToggleBtn').addEventListener('pointercancel', clearFastOwnshipLongPressTimer);
    document.getElementById('fastOwnshipToggleBtn').addEventListener('pointerleave', clearFastOwnshipLongPressTimer);

    document.getElementById('fastAvBusToggleBtn').addEventListener('click', function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      if (selectedTab !== 'DTC') return;
      if (fastAvBusLongPressFired) {
        fastAvBusLongPressFired = false;
        return;
      }
      fastAvBus.enabled = !fastAvBus.enabled;
      updateFastAvBusToggleUi();
      if (latestData) render(latestData);
    });

    document.getElementById('fastAvBusToggleBtn').addEventListener('pointerdown', function () {
      if (fastAvBusLongPressTimerId) {
        window.clearTimeout(fastAvBusLongPressTimerId);
        fastAvBusLongPressTimerId = 0;
      }
      fastAvBusLongPressFired = false;
      fastAvBusLongPressTimerId = window.setTimeout(function () {
        fastAvBusLongPressTimerId = 0;
        fastAvBusLongPressFired = true;
        fastAvBusDebugPanelVisible = !fastAvBusDebugPanelVisible;
        updateFastAvBusToggleUi();
        if (latestData) render(latestData);
      }, fastAvBusLongPressMs);
    });

    function clearFastAvBusLongPressTimer() {
      if (!fastAvBusLongPressTimerId) return;
      window.clearTimeout(fastAvBusLongPressTimerId);
      fastAvBusLongPressTimerId = 0;
    }

    document.getElementById('fastAvBusToggleBtn').addEventListener('pointerup', clearFastAvBusLongPressTimer);
    document.getElementById('fastAvBusToggleBtn').addEventListener('pointercancel', clearFastAvBusLongPressTimer);
    document.getElementById('fastAvBusToggleBtn').addEventListener('pointerleave', clearFastAvBusLongPressTimer);

    document.getElementById('fltPlanFilesCloseBtn').addEventListener('click', function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      setFltPlanFilesOverlayOpen(false);
    });

    document.getElementById('fltPlanSessionCloseBtn').addEventListener('click', function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      setFltPlanSessionOverlayOpen(false);
    });

    document.getElementById('fltPlanFilesOverlay').addEventListener('click', function (ev) {
      if (ev.target && ev.target.id === 'fltPlanFilesOverlay') {
        setFltPlanFilesOverlayOpen(false);
      }
    });

    document.getElementById('fltPlanSessionOverlay').addEventListener('click', function (ev) {
      if (ev.target && ev.target.id === 'fltPlanSessionOverlay') {
        setFltPlanSessionOverlayOpen(false);
      }
    });

    document.getElementById('fltPlanOverlayFileList').addEventListener('click', async function (ev) {
      let node = ev.target;
      while (node && node !== this && !(node.getAttribute && node.getAttribute('data-overlay-file'))) {
        node = node.parentNode;
      }
      if (!node || node === this) return;

      const encoded = String(node.getAttribute('data-overlay-file') || '');
      if (!encoded) return;

      const file = decodeURIComponent(encoded);
      await selectDtcFile(file);
      if (latestData) {
        render(latestData);
      }
      setFltPlanFilesOverlayOpen(false);
    });

    document.getElementById('dtcFileList').addEventListener('click', function (ev) {
      let node = ev.target;
      while (node && node !== this && !node.getAttribute('data-file')) {
        node = node.parentNode;
      }
      if (!node || node === this) return;
      const encoded = String(node.getAttribute('data-file') || '');
      const file = decodeURIComponent(encoded);
      selectDtcFile(file);
    });

    document.getElementById('dtcSelectorHeader').addEventListener('click', function () {
      applyDtcListCollapsedState(!dtcListCollapsed);
      persistDtcListCollapsedState();
    });

    document.getElementById('liveExportMeta').addEventListener('click', function () {
      applyLiveRefreshState(!liveRefreshEnabled);
    });

    (function () {
      const logo = document.querySelector('.logo');
      if (!logo) return;
      logo.title = 'Hidden test mode: Shift+Click or Triple-Click';

      let logoClickCount = 0;
      let logoClickResetTimer = null;

      function toggleFakeMissionMode() {
        if (fakeMissionEnabled) {
          stopFakeMissionMode();
        } else {
          startFakeMissionMode();
        }
      }

      logo.addEventListener('click', function (ev) {
        if (ev && ev.shiftKey) {
          toggleFakeMissionMode();
          return;
        }

        logoClickCount++;
        if (logoClickResetTimer) {
          clearTimeout(logoClickResetTimer);
          logoClickResetTimer = null;
        }

        if (logoClickCount >= 3) {
          logoClickCount = 0;
          toggleFakeMissionMode();
          return;
        }

        logoClickResetTimer = setTimeout(function () {
          logoClickCount = 0;
          logoClickResetTimer = null;
        }, 1200);
      });
    })();

    document.getElementById('fakeMissionSlower').addEventListener('click', function () {
      adjustFakeMissionSpeed(1 / 1.4);
    });

    document.getElementById('fakeMissionFaster').addEventListener('click', function () {
      adjustFakeMissionSpeed(1.4);
    });

    document.getElementById('fakeMissionStop').addEventListener('click', function () {
      stopFakeMissionMode();
    });

    document.getElementById('sessionHeader').addEventListener('click', function () {
      applySessionCollapsedState(!sessionCollapsed);
      persistSessionCollapsedState();
    });

    applyDtcListCollapsedState(readInitialDtcListCollapsed());
    applySessionCollapsedState(readInitialSessionCollapsed());
    efbPinnedChartsByAirport = readPinnedChartsPreference();
    efbViewerMode = readEfbViewerModePreference();
    efbSaFollowOwnshipEnabled = readEfbSaFollowEnabledPreference();
    efbSaTrackUpEnabled = readEfbSaTrackUpEnabledPreference();
    efbSaAnchorMode = readEfbSaAnchorModePreference();
    efbSaUseFastOwnshipEnabled = readEfbSaUseFastOwnshipPreference();
    efbAdLandingEnabled = readEfbAdLandingEnabledPreference();
    applyEfbAdLandingUi();
    autoAtaRecSpdEnabled = readAutoAtaRecSpdEnabledPreference();
    applyAutoAtaRecSpdUi();
    efbSaNavigraphLayer = readEfbSaNavigraphLayerPreference();
    efbSaShowDoghouses = readEfbSaDoghousesEnabledPreference();
    efbSaShowMissionDrawings = readEfbSaMissionDrawingsEnabledPreference();
    efbSaShowHistoryTrack = readEfbSaHistoryTrackEnabledPreference();
    efbSaShowSamThreatRings = readEfbSaSamThreatsEnabledPreference();
    efbSaShowUserWaypoints = readEfbSaUserWaypointsEnabledPreference();
    efbSaUserWaypoints = readEfbSaUserWaypointsPreference();
    efbSaSavedHistoryTracksBySelection = readEfbSaSavedHistoryTracksPreference();
    hydrateEfbSaSavedHistoryTracksFromServer();
    autoBrowse = readAutoBrowsePreference();
    defaultNavlogAltDisplayMode = readNavlogAltDisplayModePreference();
    defaultNavlogSpdDisplayMode = readNavlogSpdDisplayModePreference();
    defaultNavlogDistDisplayMode = readNavlogDistDisplayModePreference();
    nightModeEnabled = readNightModePreference();
    applyNightModeUi();
    dlinkOnEnabled = readDlinkOnPreference();
    applyDlinkOnUi();
    applyAutoBrowseUi();
    contentFontSizePx = readContentFontSizePreference();
    applyContentFontSizeUi();
    drawModeEnabled = readDrawModePreference();
    updateDrawModeToggleUi();
    liveRefreshEnabled = readLiveRefreshPreference();
    updateLiveRefreshUi();
    if (liveRefreshEnabled) {
      applyLiveRefreshState(true);
    }
    tabKeywordsSplitByTab = readTabKeywordsSplitRatioByTab();
    initTabKeywordsDivider();
    applyCurrentTabKeywordsSplit();
    registerCustomActionHandlers();
    window.addEventListener('resize', function () {
      applyCurrentTabKeywordsSplit();
    });
    configureOpenKneeboard();
    updateFastOwnshipToggleUi();
    updateFastAvBusToggleUi();
    tickEfbSaOwnshipCamera();
    pollEfbSaOwnshipFast();
    pollFastAvBus();
    updateFakeMissionControlsUi();
    tick();
    clockTick();
    runtimeState.timers.clockTickTimer = setInterval(clockTick, 250);
    setInterval(tick, 1000);
