    // @ts-nocheck
    const TABS = ['LOG', 'DTC', 'ATC', 'AWACS', 'JTAC', 'TANKER', 'AOCS', 'FLIGHT', 'AI CREW', 'GND CREW', 'NOTES'];
    const OKB_TAB_TYPE_ID = 'VAICOM-Community;okb-out';
    const OKB_CUSTOM_ACTION_PREFIX = OKB_TAB_TYPE_ID + ';';
    const OKB_ACTION_TAB_PREV = OKB_CUSTOM_ACTION_PREFIX + 'tab-prev';
    const OKB_ACTION_TAB_NEXT = OKB_CUSTOM_ACTION_PREFIX + 'tab-next';
    const OKB_ACTION_TAB_SELECT = OKB_CUSTOM_ACTION_PREFIX + 'tab-select';
    let selectedTab = 'LOG';
    let autoBrowse = false;
    let lastObservedServerActiveCategory = '';
    let sessionCollapsed = false;
    let latestData = null;
    let showServerMessages = false;
    let metarPressureInHg = false;
    let metarPressureAutoMode = true;
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
    let fltPlanExpandedTimeAnchorBySelection = {};
    let fltPlanExpandedNavEditBySelection = {};
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
    let runtimeFlightPlanSnapshotFingerprint = '';
    let storesBaselineSnapshot = {
      missionIdentity: '',
      hasSnapshot: false,
      stations: []
    };
    let runtimeFlightPlanUserOverride = false;
    let lastKnownPlayerCallsign = '';
    let lastKnownTheater = '';
    let jtacOverlayStateByMission = {};
    let lastOwnshipPointForHeading = null;
    let lastOwnshipHeadingDeg = NaN;
    let lastMissionIdentity = '';
    let pendingMissionIdentity = '';
    let pendingMissionIdentityConfirmCount = 0;
    const runtimeState = {
      missionClock: {
        lastSeconds: NaN,
        rewindCandidateSeconds: NaN,
        rewindConfirmCount: 0,
        anchorSeconds: NaN,
        anchorSystemMs: 0,
        anchorIdentity: '',
      },
      timers: {
        clockTickTimer: null,
      },
      render: {
        lastStartUtcMs: 0,
        flushHandle: 0,
        flushPending: false,
        scheduledData: null,
        minGapMs: 16,
      }
    };
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
    const autoBrowseStorageKey = 'vaicom.okb.autoBrowse';
    const nightModeStorageKey = 'vaicom.okb.nightMode';
    const dlinkOnStorageKey = 'vaicom.okb.dlinkOn';
    const contentFontSizeStorageKey = 'vaicom.okb.contentFontSize';
    const navlogAltDisplayModeStorageKey = 'vaicom.okb.navlogAltDisplayMode';
    const navlogSpdDisplayModeStorageKey = 'vaicom.okb.navlogSpdDisplayMode';
    const navlogDistDisplayModeStorageKey = 'vaicom.okb.navlogDistDisplayMode';
    const efbPinnedChartsStorageKey = 'vaicom.okb.efbPinnedChartsByAirport';
    const efbViewerModeStorageKey = 'vaicom.okb.efbViewerMode';
    const efbSaFollowEnabledStorageKey = 'vaicom.okb.efbSaFollowEnabled';
    const efbSaTrackUpEnabledStorageKey = 'vaicom.okb.efbSaTrackUpEnabled';
    const efbSaAnchorModeStorageKey = 'vaicom.okb.efbSaAnchorMode';
    const efbSaUseFastOwnshipStorageKey = 'vaicom.okb.efbSaUseFastOwnship';
    const efbAdLandingEnabledStorageKey = 'vaicom.okb.efbAdLandingEnabled';
    const efbSaNavigraphLayerStorageKey = 'vaicom.okb.efbSaNavigraphLayer';
    const efbSaDoghousesEnabledStorageKey = 'vaicom.okb.efbSaDoghousesEnabled';
    const efbSaMissionDrawingsEnabledStorageKey = 'vaicom.okb.efbSaMissionDrawingsEnabled';
    const efbSaHistoryTrackEnabledStorageKey = 'vaicom.okb.efbSaHistoryTrackEnabled';
    const efbSaSamThreatsEnabledStorageKey = 'vaicom.okb.efbSaSamThreatsEnabled';
    const efbSaUserWaypointsEnabledStorageKey = 'vaicom.okb.efbSaUserWaypointsEnabled';
    const efbSaUserWaypointsStorageKey = 'vaicom.okb.efbSaUserWaypoints';
    const efbSaSavedHistoryTracksStorageKey = 'vaicom.okb.efbSaSavedHistoryTracksBySelection';
    const autoAtaRecSpdEnabledStorageKey = 'vaicom.okb.autoAtaRecSpdEnabled';
    const drawModeTimeoutMs = 30000;
    const speedRecommendationTimeoutMs = 30000;
    const autoAtaCaptureThresholdMeters = 0.75 * 1852;
    const autoAtaCaptureReleaseMeters = 0.35 * 1852;
    const autoAtaRecPulseDurationMs = speedRecommendationTimeoutMs;
    const autoAtaWaypointPulseDurationMs = 6000;
    const fakeMissionSpeedMin = 0.25;
    const fakeMissionSpeedMax = 8.0;
    let tabKeywordsSplitByTab = {};
    let nightModeEnabled = false;
    let dlinkOnEnabled = true;
    let contentFontSizePx = 24;
    let defaultNavlogAltDisplayMode = 'ft';
    let defaultNavlogSpdDisplayMode = 'kts';
    let defaultNavlogDistDisplayMode = 'nm';
    let settingsOverlayOpen = false;
    let drawOverlayOpen = false;
    let helpOverlayOpen = false;
    let helpOverlayLoadedTab = '';
    let fltPlanFilesOverlayOpen = false;
    let fltPlanSessionOverlayOpen = false;
    let efbAvailableAirports = null;
    let efbAirportsLoadPromise = null;
    let efbAirportEntries = {};
    let efbIcaoByIata = {};
    let efbManuallySelectedAirport = '';
    let efbChartsByAirport = {};
    let efbChartsLoadPromiseByAirport = {};
    let efbChartsLastFetchMsByAirport = {};
    let efbSelectedChartByAirport = {};
    let efbViewportByAirport = {};
    let efbLoadingOverlayMessage = '';
    let efbDragState = null;
    let efbUiDirty = true;
    let efbLastResolvedAirport = '';
    let efbPickerDocumentHandlerBound = false;
    let efbDrawerOpen = false;
    let efbDrawerMode = 'airport';
    let efbDrawerAnimateOpenOnce = false;
    let efbSelectionFlowActive = false;
    let efbPinnedChartsByAirport = {};
    let efbRightToolsOpen = false;
    let efbAutoSelectOnEnter = false;
    let efbSearchOverlayOpen = false;
    let efbSearchMode = 'airport';
    let efbSearchToken = '';
    let efbSearchAnimateOpenOnce = false;
    let efbViewerMode = 'chart';
    let efbSaShowAirports = true;
    let efbSaShowNavlog = true;
    let efbSaShowDtcOverlay = true;
    let efbSaShowJtacTargets = true;
    let efbSaFollowOwnshipEnabled = false;
    let efbSaTrackUpEnabled = false;
    let efbSaAnchorMode = 'center';
    let efbSaUseFastOwnshipEnabled = true;
    let efbAdLandingEnabled = false;
    let efbSaNavigraphLayer = 'vfr';
    let efbSaShowDoghouses = true;
    let efbSaShowMissionDrawings = true;
    let efbSaShowHistoryTrack = false;
    let efbSaShowSamThreatRings = false;
    let efbSaSamThreatCoalitionMode = 'hostile';
    let efbSaShowUserWaypoints = true;
    let autoAtaRecSpdEnabled = false;
    let autoAtaRecPulseUntilUtc = 0;
    let autoAtaRecPulseSpdText = '';
    let autoAtaRecPulseNoteText = '';
    let autoAtaRecPulseUnable = false;
    let autoAtaWaypointPulseUntilByKey = {};
    let efbSaOwnshipFast = { hasPosition: false, posX: NaN, posY: NaN, altFeet: NaN, headingDeg: NaN, hasHeading: false, hasGroundSpeed: false, groundSpeedKnots: NaN, hasWow: false, wow: -1, theater: '', updatedUtcMs: 0, lastPollMs: 0 };
    let fastAvBus = {
      enabled: true,
      state: 'OFF',
      moduleConnected: false,
      hasData: false,
      isFresh: false,
      hasFuel: false,
      hasWow: false,
      wow: -1,
      hasFuelFraction: false,
      fuelFraction: NaN,
      hasFuelMassMaxKg: false,
      fuelMassMaxKg: NaN,
      fuelMassMaxSource: '',
      anomalyDetected: false,
      anomalyReason: '',
      prevTotalFuelKg: NaN,
      prevExternalFuelKg: NaN,
      prevSampleUtcMs: 0,
      flowFuelHistory: [],
      flowEmaKgPerHour: NaN,
      fuelFlowKgPerHour: NaN,
      fuelFlowLbsPerHour: NaN,
      enginesLikelyOn: false,
      aarOffloadKg: 0,
      aarOffloadLbs: 0,
      aarContacts: 0,
      aarInProgress: false,
      aarPendingKg: 0,
      aarReductionStartUtcMs: 0,
      aarReductionKg: 0,
      aarLastEndUtcMs: 0,
      totalFuelKg: NaN,
      totalFuelLbs: NaN,
      internalFuelKg: NaN,
      internalFuelLbs: NaN,
      externalFuelKg: NaN,
      externalFuelLbs: NaN,
      updatedUtcMs: 0,
      lastPollMs: 0,
      lastGood: {
        totalFuelKg: NaN,
        totalFuelLbs: NaN,
        internalFuelKg: NaN,
        internalFuelLbs: NaN,
        externalFuelKg: NaN,
        externalFuelLbs: NaN,
        updatedUtcMs: 0,
      }
    };

    function readStoredPreferenceValue(key) {
      try {
        if (!window.localStorage) return null;
        const raw = window.localStorage.getItem(key);
        return raw === undefined ? null : raw;
      } catch (_) {
        return null;
      }
    }

    function writeStoredPreferenceValue(key, value) {
      try {
        if (!window.localStorage) return;
        window.localStorage.setItem(key, String(value));
      } catch (_) {
      }
    }

    function readOneZeroPreference(key, defaultValue) {
      const raw = readStoredPreferenceValue(key);
      if (raw === null || raw === '') return !!defaultValue;
      return raw === '1';
    }

    function readZeroDisabledPreference(key, defaultValue) {
      const raw = readStoredPreferenceValue(key);
      if (raw === null || raw === '') return !!defaultValue;
      return raw !== '0';
    }

    function persistOneZeroPreference(key, enabled) {
      writeStoredPreferenceValue(key, enabled ? '1' : '0');
    }

    function readNormalizedPreference(key, defaultValue, normalizeFn) {
      const raw = readStoredPreferenceValue(key);
      if (raw === null || raw === '') {
        return normalizeFn ? normalizeFn(defaultValue) : defaultValue;
      }
      return normalizeFn ? normalizeFn(raw) : raw;
    }

    function persistNormalizedPreference(key, value, normalizeFn) {
      const normalized = normalizeFn ? normalizeFn(value) : value;
      writeStoredPreferenceValue(key, normalized);
    }

    function readAutoBrowsePreference() {
      return readOneZeroPreference(autoBrowseStorageKey, false);
    }

    function persistAutoBrowsePreference() {
      persistOneZeroPreference(autoBrowseStorageKey, autoBrowse);
    }
    let efbAdLandingAssistState = { seenAirborne: false, triggeredThisWowOn: false };
    let autoTakeoffBySelection = {};
    let efbFastOwnshipDebugPanelVisible = false;
    let fastAvBusDebugPanelVisible = false;
    let fastOwnshipLongPressTimerId = 0;
    let fastOwnshipLongPressFired = false;
    const fastOwnshipLongPressMs = 2000;
    let fastAvBusLongPressTimerId = 0;
    let fastAvBusLongPressFired = false;
    const fastAvBusLongPressMs = 2000;
    let efbSaUserWaypoints = [];
    let efbSaUserWaypointEditingId = '';
    let efbSaUserWaypointEditDraft = '';
    let efbSaSavedHistoryTrackEditingId = '';
    let efbSaSavedHistoryTrackEditDraft = '';
    let efbSaSavedHistoryTrackEditSelectionKey = '';
    let efbSaUserWaypointDiag = {
      lastAction: '',
      lastAddLatLon: '',
      lastAddXY: '',
      lastPayloadCount: 0,
      lastRenderedCount: 0,
      lastSelectedAssetKey: '',
    };
    let efbSaFollowSuppressUntilBySelection = {};
    let efbSaLastCameraApplyMsBySelection = {};
    let efbSaOwnshipMotionBySelection = {};
    let efbSaHistoryTrackBySelection = {};
    let efbSaHistoryTrackLastSampleBySelection = {};
    let samThreatReferenceEntriesCache = null;
    let samThreatReferenceLoadStarted = false;
    let samThreatResolvedStateByAnchor = {};
    let samThreatBraAnchorByKey = {};
    let efbAwacsSelectedSamThreatKeys = [];
    let efbSaSavedHistoryTracksBySelection = {};
    let efbSaLastModuleConnected = false;
    let efbSaCameraTickerId = 0;
    let efbSaOwnshipPollTickerId = 0;
    let fastAvBusPollTickerId = 0;
    const efbSaUserWaypointHoldMs = 1000;
    const efbSaUserWaypointHoldMovePx = 16;
    const efbSaDoghouseMinZoom = 9.5;
    const efbSaDoghouseOffsetNm = 2.0;
    const efbSaHistoryTrackSampleMs = 5000;
    const efbSaHistoryTrackMinMoveMeters = 25;
    const efbSaHistoryTrackMaxPoints = 5000;
    const overlayCloseHideDelayMs = 620;
    const fastAvBusFlowAvgWindowSec = 5.0;
    const fastAvBusFlowFastWindowSec = 1.0;
    const fastAvBusFlowHistoryMaxSec = 10.0;
    const fastAvBusFlowEmaTauSec = 0.6;
    const fastAvBusFlowDeadbandKgPerHour = 30;
    const fastAvBusFlowStepBypassKgPerHour = 700;
    const fastAvBusFlowStepBlend = 0.75;
    const fastAvBusAarRiseEndKg = 0.1;
    const fastAvBusAarMinContactLbs = 25;
    const fastAvBusAarMinContactKg = fastAvBusAarMinContactLbs / 2.20462262185;
    const fastAvBusAarResetReductionSec = 2.5;
    const fastAvBusAarResetReductionKg = 1.0;
    const fastAvBusAarReconnectHoldSec = 2.5;

    function clamp(v, min, max) {
      return Math.max(min, Math.min(max, v));
    }

    function formatFastAvBusNumber(value, digits) {
      const n = Number(value);
      if (!isFinite(n)) return 'n/a';
      return n.toFixed(digits);
    }

    function interpolateFuelKgAtTime(samples, targetTs) {
      if (!Array.isArray(samples) || samples.length < 1) return NaN;
      const first = samples[0];
      const last = samples[samples.length - 1];
      if (!first || !last) return NaN;
      if (targetTs <= first.ts) return Number(first.fuelKg);
      if (targetTs >= last.ts) return Number(last.fuelKg);

      for (let i = 1; i < samples.length; i += 1) {
        const a = samples[i - 1];
        const b = samples[i];
        if (!a || !b) continue;
        if (targetTs < a.ts || targetTs > b.ts) continue;
        const span = Number(b.ts - a.ts);
        if (!isFinite(span) || span <= 0) return Number(a.fuelKg);
        const t = (targetTs - a.ts) / span;
        return Number(a.fuelKg) + (Number(b.fuelKg) - Number(a.fuelKg)) * t;
      }

      return Number(last.fuelKg);
    }

    function computeFlowFromWindowKgPerHour(samples, nowTs, windowSec) {
      const list = Array.isArray(samples) ? samples : [];
      if (list.length < 2) return NaN;
      const last = list[list.length - 1];
      if (!last || !isFinite(Number(last.fuelKg))) return NaN;

      const earliestTs = Number(list[0].ts);
      const targetStartTs = nowTs - (windowSec * 1000);
      const startTs = Math.max(targetStartTs, earliestTs);
      const spanSec = (nowTs - startTs) / 1000;
      if (!isFinite(spanSec) || spanSec < 0.2) return NaN;

      const startFuelKg = Number(interpolateFuelKgAtTime(list, startTs));
      if (!isFinite(startFuelKg)) return NaN;

      const endFuelKg = Number(last.fuelKg);
      const burnKg = Math.max(0, startFuelKg - endFuelKg);
      return burnKg * 3600 / spanSec;
    }

    function updateFastAvBusFlowSmoothing(totalFuelKg, nowTs, dtSec) {
      const totalFuel = Number(totalFuelKg);
      if (!isFinite(totalFuel) || !isFinite(nowTs) || !isFinite(dtSec) || dtSec <= 0) return;

      if (!Array.isArray(fastAvBus.flowFuelHistory)) {
        fastAvBus.flowFuelHistory = [];
      }

      const history = fastAvBus.flowFuelHistory;
      const clampedFuel = Math.max(0, totalFuel);
      const lastSample = history.length ? history[history.length - 1] : null;
      if (lastSample && Number(lastSample.ts) === nowTs) {
        lastSample.fuelKg = clampedFuel;
      } else {
        history.push({ ts: nowTs, fuelKg: clampedFuel });
      }

      const minTs = nowTs - (fastAvBusFlowHistoryMaxSec * 1000);
      while (history.length > 2 && Number(history[1].ts) < minTs) {
        history.shift();
      }

      const avgFlow = computeFlowFromWindowKgPerHour(history, nowTs, fastAvBusFlowAvgWindowSec);
      if (!isFinite(avgFlow)) return;

      const fastFlow = computeFlowFromWindowKgPerHour(history, nowTs, fastAvBusFlowFastWindowSec);
      let targetFlow = avgFlow;
      if (isFinite(fastFlow)) {
        const stepDelta = fastFlow - avgFlow;
        if (Math.abs(stepDelta) >= fastAvBusFlowStepBypassKgPerHour) {
          targetFlow = Math.max(0, avgFlow + (stepDelta * fastAvBusFlowStepBlend));
        }
      }

      const prevEma = Number(fastAvBus.flowEmaKgPerHour);
      const alpha = 1 - Math.exp(-Math.min(dtSec, 1.0) / fastAvBusFlowEmaTauSec);
      const emaFlow = isFinite(prevEma)
        ? (prevEma + alpha * (targetFlow - prevEma))
        : targetFlow;
      fastAvBus.flowEmaKgPerHour = Math.max(0, emaFlow);

      let displayFlow = Number(fastAvBus.fuelFlowKgPerHour);
      if (!isFinite(displayFlow)) {
        displayFlow = fastAvBus.flowEmaKgPerHour;
      } else {
        const delta = fastAvBus.flowEmaKgPerHour - displayFlow;
        if (Math.abs(delta) > fastAvBusFlowStepBypassKgPerHour) {
          displayFlow = fastAvBus.flowEmaKgPerHour;
        } else if (Math.abs(delta) > fastAvBusFlowDeadbandKgPerHour) {
          displayFlow = displayFlow + (delta * 0.6);
        }
      }

      displayFlow = Math.max(0, displayFlow);
      fastAvBus.fuelFlowKgPerHour = displayFlow;
      fastAvBus.fuelFlowLbsPerHour = displayFlow * 2.20462262185;
    }

    function getFastAvBusDisplayState() {
      if (!(fastAvBus && fastAvBus.enabled)) return 'OFF';

      const moduleConnected = !!(fastAvBus && fastAvBus.moduleConnected);
      if (!moduleConnected) return 'OFF';

      if (fastAvBus && fastAvBus.anomalyDetected) return 'DEGRADED';

      const state = String((fastAvBus && fastAvBus.state) || 'OFF').toUpperCase();
      const hasRecentGood = !!(fastAvBus && fastAvBus.lastGood && Number(fastAvBus.lastGood.updatedUtcMs || 0) > 0);
      const enginesOn = !!(fastAvBus && fastAvBus.enginesLikelyOn);
      const aarActive = !!(fastAvBus && fastAvBus.aarInProgress);

      if (state === 'LIVE' && fastAvBus && fastAvBus.isFresh && fastAvBus.hasFuel && (enginesOn || aarActive)) return 'LIVE';
      if (state === 'DEGRADED') return hasRecentGood ? 'ON' : 'DEGRADED';
      if (state === 'OFF') return hasRecentGood ? 'ON' : 'OFF';
      if (fastAvBus && fastAvBus.isFresh && fastAvBus.hasFuel) return 'ON';
      if (hasRecentGood) return 'ON';
      return 'ON';
    }

    function updateFastAvBusToggleUi() {
      const btn = document.getElementById('fastAvBusToggleBtn');
      const panel = document.getElementById('fastAvBusDataPanel');
      if (!btn) return;

      if (selectedTab !== 'DTC') {
        btn.className = 'fltPlanQuickBtn fastAvBusBtn hidden';
        if (panel) {
          panel.className = 'fastAvBusDataPanel hidden';
          panel.textContent = '';
        }
        return;
      }

      const state = getFastAvBusDisplayState();
      const classState = (!fastAvBus.enabled || state === 'OFF')
        ? 'off'
        : (state === 'LIVE' ? 'live' : (state === 'ON' ? 'on' : 'degraded'));
      btn.className = 'fltPlanQuickBtn fastAvBusBtn ' + classState;
      btn.textContent = 'AID';

      const totalKg = state === 'LIVE'
        ? formatFastAvBusNumber(fastAvBus.totalFuelKg, 1)
        : formatFastAvBusNumber(fastAvBus.lastGood.totalFuelKg, 1);
      const totalLbs = state === 'LIVE'
        ? formatFastAvBusNumber(fastAvBus.totalFuelLbs, 0)
        : formatFastAvBusNumber(fastAvBus.lastGood.totalFuelLbs, 0);
      const internalKg = state === 'LIVE'
        ? formatFastAvBusNumber(fastAvBus.internalFuelKg, 1)
        : formatFastAvBusNumber(fastAvBus.lastGood.internalFuelKg, 1);
      const externalKg = state === 'LIVE'
        ? formatFastAvBusNumber(fastAvBus.externalFuelKg, 1)
        : formatFastAvBusNumber(fastAvBus.lastGood.externalFuelKg, 1);
      const wowText = (fastAvBus && fastAvBus.hasWow)
        ? (Number(fastAvBus.wow) === 1 ? 'WOW ON' : (Number(fastAvBus.wow) === 0 ? 'WOW OFF' : 'WOW n/a'))
        : 'WOW n/a';

      const staleTag = state === 'LIVE' ? '' : ' (frozen)';
      btn.title = [
        'AID ' + (fastAvBus.enabled ? state : 'OFF (manual)'),
        'TOTAL ' + totalKg + 'kg / ' + totalLbs + 'lb' + staleTag,
        'INT ' + internalKg + 'kg · EXT ' + externalKg + 'kg',
        wowText,
        (fastAvBus && fastAvBus.anomalyDetected) ? ('ANOMALY ' + String(fastAvBus.anomalyReason || 'fuel trend')) : 'ANOMALY none',
      ].join(' · ');

      if (panel) {
        const showPanel = fastAvBusDebugPanelVisible && selectedTab === 'DTC';
        panel.className = showPanel ? 'fastAvBusDataPanel' : 'fastAvBusDataPanel hidden';
        if (showPanel) {
          const ageMs = Date.now() - Number(fastAvBus.updatedUtcMs || 0);
          const ageText = isFinite(ageMs) && ageMs >= 0 ? (ageMs / 1000).toFixed(2) + 's' : 'n/a';
          panel.textContent = [
            'AID RAW DEBUG',
            'enabled=' + String(!!fastAvBus.enabled)
            + ' state=' + String(state)
            + ' moduleConnected=' + String(!!fastAvBus.moduleConnected)
            + ' fresh=' + String(!!fastAvBus.isFresh)
            + ' hasData=' + String(!!fastAvBus.hasData),
            'hasFuel=' + String(!!fastAvBus.hasFuel)
            + ' wow=' + String(Number(fastAvBus.wow))
            + ' hasWow=' + String(!!fastAvBus.hasWow),
            'fuelFraction=' + formatFastAvBusNumber(fastAvBus.fuelFraction, 6)
            + ' hasFuelFraction=' + String(!!fastAvBus.hasFuelFraction),
            'fuelMassMaxKg=' + formatFastAvBusNumber(fastAvBus.fuelMassMaxKg, 3)
            + ' hasFuelMassMaxKg=' + String(!!fastAvBus.hasFuelMassMaxKg),
            'diag=' + (
              !fastAvBus.moduleConnected ? 'moduleDisconnected'
                : (!fastAvBus.hasData ? 'noData'
                  : (!fastAvBus.isFresh ? 'staleData'
                    : (!fastAvBus.hasFuel ? 'missingFuelDerivation' : 'live')))
            ),
            'totalKg=' + formatFastAvBusNumber(fastAvBus.totalFuelKg, 3)
            + ' internalKg=' + formatFastAvBusNumber(fastAvBus.internalFuelKg, 3)
            + ' externalKg=' + formatFastAvBusNumber(fastAvBus.externalFuelKg, 3),
            'age=' + ageText,
          ].join('\n');
        } else {
          panel.textContent = '';
        }
      }
    }

    function pollFastAvBus() {
      if (fastAvBusPollTickerId) return;
      fastAvBusPollTickerId = window.setInterval(function () {
        if (selectedTab !== 'DTC' && selectedTab !== 'EFB') return;

        const nowMs = Date.now();
        if ((nowMs - Number(fastAvBus.lastPollMs || 0)) < 120) return;
        fastAvBus.lastPollMs = nowMs;

        if (!fastAvBus.enabled) {
          updateFastAvBusToggleUi();
          return;
        }

        fetch('/okb/fltpln/avbus', { cache: 'no-store' })
          .then(function (res) {
            if (!res || !res.ok) return null;
            return res.json();
          })
          .then(function (payload) {
            if (!payload || typeof payload !== 'object') return;

            fastAvBus.state = String(payload.state || 'OFF').toUpperCase();
            fastAvBus.moduleConnected = payload.moduleConnected === true;
            fastAvBus.hasData = payload.hasData === true;
            fastAvBus.isFresh = payload.isFresh === true;
            fastAvBus.hasFuel = payload.hasFuel === true;
            fastAvBus.hasWow = payload.hasWow === true;
            fastAvBus.wow = Number(payload.wow);
            fastAvBus.hasFuelFraction = payload.hasFuelFraction === true;
            fastAvBus.fuelFraction = Number(payload.fuelFraction);
            fastAvBus.hasFuelMassMaxKg = payload.hasFuelMassMaxKg === true;
            fastAvBus.fuelMassMaxKg = Number(payload.fuelMassMaxKg);
            fastAvBus.fuelMassMaxSource = String(payload.fuelMassMaxSource || '');

            fastAvBus.totalFuelKg = Number(payload.totalFuelKg);
            fastAvBus.totalFuelLbs = Number(payload.totalFuelLbs);
            fastAvBus.internalFuelKg = Number(payload.internalFuelKg);
            fastAvBus.internalFuelLbs = Number(payload.internalFuelLbs);
            fastAvBus.externalFuelKg = Number(payload.externalFuelKg);
            fastAvBus.externalFuelLbs = Number(payload.externalFuelLbs);
            fastAvBus.updatedUtcMs = Date.now();

            fastAvBus.anomalyDetected = false;
            fastAvBus.anomalyReason = '';
            if (fastAvBus.hasFuel && isFinite(fastAvBus.totalFuelKg) && isFinite(fastAvBus.externalFuelKg)) {
              const prevT = Number(fastAvBus.prevTotalFuelKg);
              const prevE = Number(fastAvBus.prevExternalFuelKg);
              const prevTs = Number(fastAvBus.prevSampleUtcMs || 0);
              const nowTs = Number(fastAvBus.updatedUtcMs || Date.now());
              const dtSec = prevTs > 0 ? ((nowTs - prevTs) / 1000) : 0;
              if (isFinite(prevT) && isFinite(prevE) && dtSec > 0.05 && dtSec < 5) {
                const totalDropKg = prevT - fastAvBus.totalFuelKg;
                const totalRiseKg = fastAvBus.totalFuelKg - prevT;
                const externalDropKg = prevE - fastAvBus.externalFuelKg;
                const burnRateKgPerSec = totalDropKg / dtSec;
                updateFastAvBusFlowSmoothing(fastAvBus.totalFuelKg, nowTs, dtSec);
                const isAirborneForAar = !!fastAvBus.hasWow && Number(fastAvBus.wow) === 0;

                const finalizeAarContact = function () {
                  fastAvBus.aarInProgress = false;
                  fastAvBus.aarPendingKg = 0;
                  fastAvBus.aarReductionStartUtcMs = 0;
                  fastAvBus.aarReductionKg = 0;
                  fastAvBus.aarLastEndUtcMs = nowTs;
                };

                if (isAirborneForAar && totalRiseKg > fastAvBusAarRiseEndKg) {
                  if (!fastAvBus.aarInProgress) {
                    const holdMs = fastAvBusAarReconnectHoldSec * 1000;
                    const sinceLastEndMs = nowTs - Number(fastAvBus.aarLastEndUtcMs || 0);
                    if (sinceLastEndMs >= holdMs) {
                      fastAvBus.aarPendingKg = Math.max(0, Number(fastAvBus.aarPendingKg || 0) + totalRiseKg);
                      if (fastAvBus.aarPendingKg >= fastAvBusAarMinContactKg) {
                        fastAvBus.aarInProgress = true;
                        fastAvBus.aarContacts = Math.max(0, Math.round(Number(fastAvBus.aarContacts) || 0)) + 1;
                        fastAvBus.aarOffloadKg = Math.max(0, Number(fastAvBus.aarOffloadKg || 0) + fastAvBus.aarPendingKg);
                        fastAvBus.aarPendingKg = 0;
                        fastAvBus.aarReductionStartUtcMs = 0;
                        fastAvBus.aarReductionKg = 0;
                      }
                    }
                  } else {
                    fastAvBus.aarOffloadKg = Math.max(0, Number(fastAvBus.aarOffloadKg || 0) + totalRiseKg);
                    fastAvBus.aarReductionStartUtcMs = 0;
                    fastAvBus.aarReductionKg = 0;
                  }
                  fastAvBus.aarOffloadLbs = fastAvBus.aarOffloadKg * 2.20462262185;
                } else if (fastAvBus.aarInProgress) {
                  if (!isAirborneForAar) {
                    finalizeAarContact();
                  } else {
                    const dropKg = Math.max(0, totalDropKg);
                    if (dropKg > 0.01) {
                      if (Number(fastAvBus.aarReductionStartUtcMs || 0) <= 0) {
                        fastAvBus.aarReductionStartUtcMs = nowTs;
                        fastAvBus.aarReductionKg = 0;
                      }
                      fastAvBus.aarReductionKg = Math.max(0, Number(fastAvBus.aarReductionKg || 0) + dropKg);
                      const reductionSec = (nowTs - Number(fastAvBus.aarReductionStartUtcMs || 0)) / 1000;
                      if (reductionSec >= fastAvBusAarResetReductionSec
                        && Number(fastAvBus.aarReductionKg || 0) >= fastAvBusAarResetReductionKg) {
                        finalizeAarContact();
                      }
                    }
                  }
                } else {
                  fastAvBus.aarPendingKg = 0;
                }
                fastAvBus.enginesLikelyOn = Number(fastAvBus.fuelFlowKgPerHour) >= 20 || (isAirborneForAar && fastAvBus.aarInProgress);
                const likelyJettison = totalDropKg > 80 && externalDropKg > 0 && externalDropKg >= (totalDropKg * 0.8);
                if (totalDropKg > 10 && burnRateKgPerSec > 45 && !likelyJettison) {
                  fastAvBus.anomalyDetected = true;
                  fastAvBus.anomalyReason = 'unexplained fuel loss';
                }
              } else if (!isFinite(Number(fastAvBus.fuelFlowKgPerHour))) {
                fastAvBus.fuelFlowKgPerHour = 0;
                fastAvBus.fuelFlowLbsPerHour = 0;
                fastAvBus.enginesLikelyOn = false;
              }
              fastAvBus.prevTotalFuelKg = fastAvBus.totalFuelKg;
              fastAvBus.prevExternalFuelKg = fastAvBus.externalFuelKg;
              fastAvBus.prevSampleUtcMs = nowTs;
            }

            if (fastAvBus.state === 'LIVE' && fastAvBus.hasFuel) {
              if (isFinite(fastAvBus.totalFuelKg)) fastAvBus.lastGood.totalFuelKg = fastAvBus.totalFuelKg;
              if (isFinite(fastAvBus.totalFuelLbs)) fastAvBus.lastGood.totalFuelLbs = fastAvBus.totalFuelLbs;
              if (isFinite(fastAvBus.internalFuelKg)) fastAvBus.lastGood.internalFuelKg = fastAvBus.internalFuelKg;
              if (isFinite(fastAvBus.internalFuelLbs)) fastAvBus.lastGood.internalFuelLbs = fastAvBus.internalFuelLbs;
              if (isFinite(fastAvBus.externalFuelKg)) fastAvBus.lastGood.externalFuelKg = fastAvBus.externalFuelKg;
              if (isFinite(fastAvBus.externalFuelLbs)) fastAvBus.lastGood.externalFuelLbs = fastAvBus.externalFuelLbs;
              fastAvBus.lastGood.updatedUtcMs = Date.now();
            }

            updateFastAvBusToggleUi();
          })
          .catch(function () {
            updateFastAvBusToggleUi();
          });
      }, 100);
    }

    function maybeApplyEfbAdLandingAssist() {
      if (!efbAdLandingEnabled || selectedTab !== 'EFB') return;

      const hasWow = !!(efbSaOwnshipFast && efbSaOwnshipFast.hasWow);
      const wow = Number(efbSaOwnshipFast && efbSaOwnshipFast.wow);
      const hasGs = !!(efbSaOwnshipFast && efbSaOwnshipFast.hasGroundSpeed);
      const gs = Number(efbSaOwnshipFast && efbSaOwnshipFast.groundSpeedKnots);
      if (!hasWow || !hasGs || !isFinite(gs)) return;

      if (wow === 0) {
        efbAdLandingAssistState.seenAirborne = true;
        efbAdLandingAssistState.triggeredThisWowOn = false;
        return;
      }

      if (wow !== 1) {
        efbAdLandingAssistState.triggeredThisWowOn = false;
        return;
      }

      if (gs >= 80) return;
      if (!efbAdLandingAssistState.seenAirborne) return;
      if (efbAdLandingAssistState.triggeredThisWowOn) return;

      efbAdLandingAssistState.triggeredThisWowOn = true;

      // 1) Ensure the EFB is in SA Map mode.
      if (normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') {
        efbViewerMode = 'sa-map';
        persistEfbViewerModePreference();
        closeEfbDrawer();
        efbRightToolsOpen = false;
        efbSearchOverlayOpen = false;
        efbSearchAnimateOpenOnce = false;
        efbUiDirty = true;
      }

      const applyAdZoomIfNeeded = function () {
        if (selectedTab !== 'EFB' || normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') return;
        const context = resolveActiveSaMapContext(latestData || null);
        if (!context || !context.selected) return;

        // 2) Only force AD ON; never toggle it back OFF automatically.
        const quickState = getOpenFreeMapViewBySelection(context.selected);
        if (quickState && quickState.adQuickZoomActive) return;

        const handledByWebMap = (typeof handleOpenFreeMapZoomAction === 'function')
          ? handleOpenFreeMapZoomAction(context.selected, 'ad')
          : false;
        if (!handledByWebMap) {
          zoomMapViewBySelection(context.selected, 1.8);
          if (latestData) render(latestData);
        }
      };

      // 3) Render first, then apply AD zoom once map context exists.
      if (latestData) render(latestData);
      window.setTimeout(applyAdZoomIfNeeded, 60);
      window.setTimeout(applyAdZoomIfNeeded, 260);
    }

    function resolveModuleMetarPressureInHgPreference(data) {
      const server = (data && data.Server) || {};
      return !(server && server.MetarMetric === true);
    }

    function applyMetarUnitAutoPreference(data, forceReset) {
      const shouldReset = !!forceReset || metarPressureAutoMode;
      if (!shouldReset) return;
      metarPressureInHg = resolveModuleMetarPressureInHgPreference(data);
    }

    function formatMetarForSelectedUnits(metarRawText, useInHg) {
      let text = String(metarRawText || '');

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

      if (useInHg) {
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

      return text;
    }

    function readEfbSaUseFastOwnshipPreference() {
      return readZeroDisabledPreference(efbSaUseFastOwnshipStorageKey, true);
    }

    function persistEfbSaUseFastOwnshipPreference() {
      persistOneZeroPreference(efbSaUseFastOwnshipStorageKey, efbSaUseFastOwnshipEnabled);
    }

    function readEfbAdLandingEnabledPreference() {
      return readOneZeroPreference(efbAdLandingEnabledStorageKey, false);
    }

    function persistEfbAdLandingEnabledPreference() {
      persistOneZeroPreference(efbAdLandingEnabledStorageKey, efbAdLandingEnabled);
    }

    function applyEfbAdLandingUi() {
      const overlayBox = document.getElementById('overlayEfbAdLanding');
      if (overlayBox) overlayBox.checked = !!efbAdLandingEnabled;
    }

    function updateFastOwnshipToggleUi() {
      const btn = document.getElementById('fastOwnshipToggleBtn');
      const panel = document.getElementById('fastOwnshipDataPanel');
      if (!btn) return;
      const unitModes = getGlobalUnitModes(latestData || null);
      const hasFast = !!(efbSaOwnshipFast && efbSaOwnshipFast.hasPosition);
      const gs = Number(efbSaOwnshipFast && efbSaOwnshipFast.groundSpeedKnots);
      const gsText = (efbSaOwnshipFast && efbSaOwnshipFast.hasGroundSpeed && isFinite(gs)) ? ('GS ' + formatSpeedByMode(gs, unitModes.spd, true)) : 'GS n/a';
      const wow = Number(efbSaOwnshipFast && efbSaOwnshipFast.wow);
      const wowText = (efbSaOwnshipFast && efbSaOwnshipFast.hasWow)
        ? ('WOW ' + (wow === 1 ? 'ON' : (wow === 0 ? 'OFF' : 'n/a')))
        : 'WOW n/a';
      const alt = Number(efbSaOwnshipFast && efbSaOwnshipFast.altFeet);
      const altText = isFinite(alt) ? ('ALT ' + formatAltitudeByMode(alt, unitModes.alt, true)) : 'ALT n/a';
      const hdg = Number(efbSaOwnshipFast && efbSaOwnshipFast.headingDeg);
      const hdgText = (efbSaOwnshipFast && efbSaOwnshipFast.hasHeading && isFinite(hdg)) ? ('HDG ' + String(Math.round(hdg)) + '°') : 'HDG n/a';
      const posX = Number(efbSaOwnshipFast && efbSaOwnshipFast.posX);
      const posY = Number(efbSaOwnshipFast && efbSaOwnshipFast.posY);
      const posText = (isFinite(posX) && isFinite(posY))
        ? ('X ' + String(Math.round(posX)) + '  Y ' + String(Math.round(posY)))
        : 'X/Y n/a';
      const ageMs = Date.now() - Number(efbSaOwnshipFast && efbSaOwnshipFast.updatedUtcMs || 0);
      const ageText = isFinite(ageMs) && ageMs >= 0 ? ('AGE ' + String((ageMs / 1000).toFixed(1)) + 's') : 'AGE n/a';
      const srcText = efbSaUseFastOwnshipEnabled ? 'SRC FAST' : 'SRC SERVER';
      const showFastOwnshipButton = hasFast && selectedTab === 'EFB';
      btn.className = showFastOwnshipButton
        ? ('fltPlanQuickBtn fastOwnshipBtn' + (efbSaUseFastOwnshipEnabled ? '' : ' off'))
        : 'fltPlanQuickBtn fastOwnshipBtn hidden';
      btn.title = hasFast
        ? (efbSaUseFastOwnshipEnabled
          ? ('Fast ownship ON (click to force server fallback) · ' + gsText + ' · ' + wowText)
          : ('Fast ownship OFF (click to resume fast ownship) · ' + gsText + ' · ' + wowText))
        : 'Waiting for fast ownship stream...';

      if (panel) {
        const shouldShowPanel = efbFastOwnshipDebugPanelVisible && hasFast && selectedTab === 'EFB' && normalizeEfbViewerMode(efbViewerMode) === 'sa-map';
        panel.className = shouldShowPanel ? 'fastOwnshipDataPanel' : 'fastOwnshipDataPanel hidden';
        if (shouldShowPanel) {
          panel.innerHTML = [
            'GPS FAST OWNSHIP',
            srcText + ' · ' + ageText,
            posText,
            hdgText + ' · ' + altText,
            gsText + ' · ' + wowText,
          ].map(escapeHtml).join('<br>');
        } else {
          panel.innerHTML = '';
        }
      }
    }

    function normalizeEfbViewerMode(mode) {
      const value = String(mode || '').toLowerCase().trim();
      return value === 'sa-map' ? 'sa-map' : 'chart';
    }

    function readEfbViewerModePreference() {
      return readNormalizedPreference(efbViewerModeStorageKey, 'chart', normalizeEfbViewerMode);
    }

    function persistEfbViewerModePreference() {
      persistNormalizedPreference(efbViewerModeStorageKey, efbViewerMode, normalizeEfbViewerMode);
    }

    function normalizeEfbSaAnchorMode(mode) {
      const value = String(mode || '').toLowerCase().trim();
      return value === 'lower-third' ? 'lower-third' : 'center';
    }

    function normalizeEfbSaNavigraphLayer(value) {
      const v = String(value || '').toLowerCase().trim();
      if (v === 'ifr-lo') return 'ifr-lo';
      if (v === 'ifr-hi') return 'ifr-hi';
      return 'vfr';
    }

    function readEfbSaNavigraphLayerPreference() {
      return readNormalizedPreference(efbSaNavigraphLayerStorageKey, 'vfr', normalizeEfbSaNavigraphLayer);
    }

    function persistEfbSaNavigraphLayerPreference() {
      persistNormalizedPreference(efbSaNavigraphLayerStorageKey, efbSaNavigraphLayer, normalizeEfbSaNavigraphLayer);
    }

    function getNextEfbSaNavigraphLayer(current) {
      const now = normalizeEfbSaNavigraphLayer(current);
      if (now === 'vfr') return 'ifr-lo';
      if (now === 'ifr-lo') return 'ifr-hi';
      return 'vfr';
    }

    function getEfbSaNavigraphLayerLabel(layer) {
      const value = normalizeEfbSaNavigraphLayer(layer);
      if (value === 'ifr-lo') return 'LOW IFR';
      if (value === 'ifr-hi') return 'HI IFR';
      return 'VFR';
    }

    function readEfbSaFollowEnabledPreference() {
      return readOneZeroPreference(efbSaFollowEnabledStorageKey, false);
    }

    function persistEfbSaFollowEnabledPreference() {
      persistOneZeroPreference(efbSaFollowEnabledStorageKey, efbSaFollowOwnshipEnabled);
    }

    function readEfbSaTrackUpEnabledPreference() {
      return readOneZeroPreference(efbSaTrackUpEnabledStorageKey, false);
    }

    function persistEfbSaTrackUpEnabledPreference() {
      persistOneZeroPreference(efbSaTrackUpEnabledStorageKey, efbSaTrackUpEnabled);
    }

    function readEfbSaDoghousesEnabledPreference() {
      return readZeroDisabledPreference(efbSaDoghousesEnabledStorageKey, true);
    }

    function persistEfbSaDoghousesEnabledPreference() {
      persistOneZeroPreference(efbSaDoghousesEnabledStorageKey, efbSaShowDoghouses);
    }

    function readEfbSaMissionDrawingsEnabledPreference() {
      return readOneZeroPreference(efbSaMissionDrawingsEnabledStorageKey, true);
    }

    function persistEfbSaMissionDrawingsEnabledPreference() {
      persistOneZeroPreference(efbSaMissionDrawingsEnabledStorageKey, efbSaShowMissionDrawings);
    }

    function readEfbSaHistoryTrackEnabledPreference() {
      return readOneZeroPreference(efbSaHistoryTrackEnabledStorageKey, false);
    }

    function persistEfbSaHistoryTrackEnabledPreference() {
      persistOneZeroPreference(efbSaHistoryTrackEnabledStorageKey, efbSaShowHistoryTrack);
    }

    function readEfbSaSamThreatsEnabledPreference() {
      return readOneZeroPreference(efbSaSamThreatsEnabledStorageKey, false);
    }

    function persistEfbSaSamThreatsEnabledPreference() {
      persistOneZeroPreference(efbSaSamThreatsEnabledStorageKey, efbSaShowSamThreatRings);
    }

    function readEfbSaAnchorModePreference() {
      return readNormalizedPreference(efbSaAnchorModeStorageKey, 'center', normalizeEfbSaAnchorMode);
    }

    function persistEfbSaAnchorModePreference() {
      persistNormalizedPreference(efbSaAnchorModeStorageKey, efbSaAnchorMode, normalizeEfbSaAnchorMode);
    }

    function readEfbSaUserWaypointsEnabledPreference() {
      return readZeroDisabledPreference(efbSaUserWaypointsEnabledStorageKey, true);
    }

    function persistEfbSaUserWaypointsEnabledPreference() {
      persistOneZeroPreference(efbSaUserWaypointsEnabledStorageKey, efbSaShowUserWaypoints);
    }

    function updateKeywordBodyHtml(html) {
      const keywordBody = document.getElementById('keywordBody');
      if (!keywordBody) return;

      const previousScrollTop = keywordBody.scrollTop;
      keywordBody.innerHTML = html;

      if (previousScrollTop > 0) {
        const maxScrollTop = Math.max(0, keywordBody.scrollHeight - keywordBody.clientHeight);
        keywordBody.scrollTop = clamp(previousScrollTop, 0, maxScrollTop);
      }
    }

    function getTheatreCenterLonLat(theatreName) {
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

    function resolveBuggedAirfieldIcaoFromAssetKey(data, assetKey) {
      function tokenIcao(value) {
        const m = String(value || '').toUpperCase().match(/\b([A-Z]{4})\b/);
        return m ? String(m[1] || '') : '';
      }

      const key = String(assetKey || '').trim();
      if (!key) return '';

      const airfields = buildMapAirfields(data);
      if (!Array.isArray(airfields) || !airfields.length) {
        return tokenIcao(key) || '';
      }

      for (let i = 0; i < airfields.length; i++) {
        const p = airfields[i] || {};
        const selectionAsset = {
          callsign: String((p && p.callsign) || '').trim(),
          name: String((p && p.name) || (p && p.label) || '').trim(),
          category: 'ATC',
          xNum: Number(p && p.xNum),
          yNum: Number(p && p.yNum),
        };
        if (makeMapAssetSelectionKey(selectionAsset) !== key) continue;
        const resolved = tokenIcao(p && p.icao)
          || tokenIcao(p && p.label)
          || tokenIcao(p && p.name)
          || tokenIcao(p && p.callsign)
          || '';
        return String(resolved || '').toUpperCase().trim();
      }

      return tokenIcao(key) || '';
    }

    async function tryPreloadEfbForBuggedAirfield(data, assetKey) {
      const icao = (typeof resolveMapSelectedAirfieldIcao === 'function')
        ? resolveMapSelectedAirfieldIcao(data, assetKey)
        : resolveBuggedAirfieldIcaoFromAssetKey(data, assetKey);
      if (!icao) return;

      await ensureEfbAirportsLoaded();
      const airports = Array.isArray(efbAvailableAirports) ? efbAvailableAirports : [];
      if (airports.length && airports.indexOf(icao) < 0) return;

      efbManuallySelectedAirport = icao;
      efbLastResolvedAirport = icao;
      await ensureEfbChartsLoaded(data);
      if (selectedTab === 'EFB' && latestData) {
        const keepSaMapStable = normalizeEfbViewerMode(efbViewerMode) === 'sa-map';
        if (!keepSaMapStable) {
          efbUiDirty = true;
          render(latestData);
        }
      }
    }

    function normalizeAssetCallsignKey(text) {
      return String(text || '').toUpperCase().replace(/[^A-Z0-9]/g, '');
    }

    function normalizeSamLookupToken(text) {
      return String(text || '').toUpperCase().replace(/[^A-Z0-9]/g, '');
    }

    function getSamThreatFallbackReferenceEntries() {
      return [
        { systemKey: 'SA2_GUIDELINE', displayName: 'SA-2 Guideline', rangeNmMin: null, rangeNmMaxAssumed: 28.0, confidence: 'medium', category: 'SAM', dcsAliases: ['SA-2', 'S-75', 'GUIDELINE', 'FAN SONG', 'SNR-75', 'FS'] },
        { systemKey: 'SA3_GOA', displayName: 'SA-3 Goa', rangeNmMin: 3.2, rangeNmMaxAssumed: 13.5, confidence: 'high', category: 'SAM', dcsAliases: ['SA-3', 'S-125', 'GOA', 'LOW BLOW', 'SNR-125', 'LB'] },
        { systemKey: 'SA5_GAMMON', displayName: 'SA-5 Gammon', rangeNmMin: null, rangeNmMaxAssumed: 130.0, confidence: 'low', category: 'LONG_SAM', dcsAliases: ['SA-5', 'S-200', 'GAMMON', 'SQUARE PAIR', '5N26', 'TIN SHIELD', 'SP'] },
        { systemKey: 'SA6_GAINFUL', displayName: 'SA-6 Gainful', rangeNmMin: 0.5, rangeNmMaxAssumed: 19.2, confidence: 'high', category: 'SAM', dcsAliases: ['SA-6', '2K12', 'KUB', 'GAINFUL', 'STRAIGHT FLUSH', '1S91', 'SF'] },
        { systemKey: 'SA8_GECKO', displayName: 'SA-8 Gecko', rangeNmMin: 0.8, rangeNmMaxAssumed: 7.5, confidence: 'high', category: 'SHORAD', dcsAliases: ['SA-8', '9K33', 'OSA', 'GECKO', 'LAND ROLL', 'LR'] },
        { systemKey: 'SA10_GRUMBLE', displayName: 'SA-10 Grumble', rangeNmMin: 3.0, rangeNmMaxAssumed: 40.0, confidence: 'high', category: 'LONG_SAM', dcsAliases: ['SA-10', 'S-300', 'S-300PS', 'GRUMBLE', 'FLAP LID', 'BIG BIRD', 'CLAM SHELL', '30N6E', '64H6E', '76N6', 'FL', 'BB', 'CS'] },
        { systemKey: 'SA11_GADFLY', displayName: 'SA-11 Gadfly', rangeNmMin: null, rangeNmMaxAssumed: 19.2, confidence: 'medium', category: 'MEDIUM_SAM', dcsAliases: ['SA-11', '9K37', 'BUK', 'GADFLY', 'FIRE DOME', 'SNOW DRIFT', 'FD', 'SD'] },
        { systemKey: 'SA13_GOPHER', displayName: 'SA-13 Gopher', rangeNmMin: 0.4, rangeNmMaxAssumed: 2.8, confidence: 'high', category: 'SHORAD', dcsAliases: ['SA-13', '9K35', 'STRELA-10', 'GOPHER', 'SNAP SHOT'] },
        { systemKey: 'SA15_GAUNTLET', displayName: 'SA-15 Gauntlet', rangeNmMin: 0.8, rangeNmMaxAssumed: 6.5, confidence: 'high', category: 'SHORAD', dcsAliases: ['SA-15', '9K331', 'TOR', 'GAUNTLET', 'SCRUM HALF', 'SH'] },
        { systemKey: 'SA19_GRISON', displayName: 'SA-19 Grison', rangeNmMin: 0.0, rangeNmMaxAssumed: 4.0, confidence: 'high', category: 'SHORAD', dcsAliases: ['SA-19', '2K22', 'TUNGUSKA', 'GRISON'] },
        { systemKey: 'HAWK_MIM23', displayName: 'MIM-23 Hawk', rangeNmMin: 1.0, rangeNmMaxAssumed: 25.6, confidence: 'high', category: 'MEDIUM_SAM', dcsAliases: ['HAWK', 'MIM-23', 'HPIR', 'PAR', 'CWAR'] },
        { systemKey: 'PATRIOT_MIM104', displayName: 'MIM-104 Patriot', rangeNmMin: 1.6, rangeNmMaxAssumed: 86.0, confidence: 'high', category: 'LONG_SAM', dcsAliases: ['PATRIOT', 'MIM-104', 'AN/MPQ-53', 'AN/MPQ-65'] },
        { systemKey: 'NASAMS', displayName: 'NASAMS', rangeNmMin: null, rangeNmMaxAssumed: 8.0, confidence: 'medium', category: 'MEDIUM_SAM', dcsAliases: ['NASAMS', 'MPQ64F1', 'SENTINEL'] },
      ];
    }

    function getSamThreatReferenceEntries() {
      if (Array.isArray(samThreatReferenceEntriesCache) && samThreatReferenceEntriesCache.length) {
        return samThreatReferenceEntriesCache;
      }

      samThreatReferenceEntriesCache = getSamThreatFallbackReferenceEntries();

      if (!samThreatReferenceLoadStarted && typeof fetch === 'function') {
        samThreatReferenceLoadStarted = true;
        fetch('assets/OKB-SamThreatRanges.json', { cache: 'no-store' })
          .then(function (r) {
            if (!r || !r.ok) return null;
            return r.json();
          })
          .then(function (json) {
            const entries = Array.isArray(json && json.entries) ? json.entries : [];
            if (entries.length) {
              samThreatReferenceEntriesCache = entries;
            }
          })
          .catch(function () { });
      }

      return samThreatReferenceEntriesCache;
    }

    function resolveSamThreatEntryFromText(text) {
      const src = String(text || '');
      const token = normalizeSamLookupToken(src);
      if (!token) return null;

      const entries = getSamThreatReferenceEntries();
      let best = null;
      let bestScore = -1;

      (Array.isArray(entries) ? entries : []).forEach(function (entry) {
        if (!entry || typeof entry !== 'object') return;
        const aliases = [];
        const displayName = String(entry.displayName || '').trim();
        const systemKey = String(entry.systemKey || '').trim();
        if (displayName) aliases.push(displayName);
        if (systemKey) aliases.push(systemKey);
        (Array.isArray(entry.dcsAliases) ? entry.dcsAliases : []).forEach(function (a) {
          const t = String(a || '').trim();
          if (t) aliases.push(t);
        });

        aliases.forEach(function (alias) {
          const aliasToken = normalizeSamLookupToken(alias);
          if (!aliasToken) return;
          if (aliasToken.length < 3 && token !== aliasToken) return;
          if (token.indexOf(aliasToken) < 0) return;
          const score = aliasToken.length;
          if (score > bestScore) {
            bestScore = score;
            best = entry;
          }
        });
      });

      if (best) return best;

      const saMatch = token.match(/SA(\d{1,3})/);
      if (saMatch && saMatch[1]) {
        return {
          systemKey: 'SA' + String(saMatch[1]),
          displayName: 'SA-' + String(saMatch[1]),
          rangeNmMin: null,
          rangeNmMaxAssumed: 20,
          confidence: 'low',
          category: 'SAM',
          dcsAliases: [],
        };
      }
      return null;
    }

    function getSamThreatFallbackRangeNmByCategory(category) {
      const c = String(category || '').toUpperCase();
      if (c === 'MANPADS') return 2.5;
      if (c === 'SHORAD') return 7.0;
      if (c === 'MEDIUM_SAM') return 20.0;
      if (c === 'LONG_SAM') return 40.0;
      if (c === 'NAVAL') return 25.0;
      return 20.0;
    }

    function makeSamThreatAssetKey(systemKey, xNum, yNum, label) {
      const x = Number(xNum);
      const y = Number(yNum);
      return [
        'SAMTHREAT',
        String(systemKey || '').toUpperCase().trim(),
        isFinite(x) ? String(Math.round(x / 50) * 50) : '',
        isFinite(y) ? String(Math.round(y / 50) * 50) : '',
        normalizeSamLookupToken(label || '').substring(0, 16)
      ].join('|');
    }

    function getSamThreatSelectionKey(assetKey) {
      const parts = String(assetKey || '').split('|');
      if (parts.length < 2) return String(assetKey || '').trim();
      const prefix = String(parts[0] || '').toUpperCase();
      if (prefix !== 'SAMTHREAT') return String(assetKey || '').trim();
      const system = String(parts[1] || '').toUpperCase().trim();
      const labelToken = String(parts[4] || '').toUpperCase().trim();
      return ['SAMTHREAT', system, labelToken].join('|');
    }

    function isSamThreatConfirmedHostileFromText(text) {
      const src = String(text || '').toUpperCase();
      if (!src) return false;
      if (src.indexOf('HOSTILE') >= 0) return true;
      if (src.indexOf('ENEMY') >= 0) return true;
      if (src.indexOf('RED') >= 0) return true;
      return false;
    }

    function resolveSamThreatShortCode(displayName, systemKey) {
      const key = String(systemKey || '').toUpperCase();
      const name = String(displayName || '').toUpperCase();
      if (key.indexOf('PATRIOT') >= 0 || name.indexOf('PATRIOT') >= 0) return 'PT';
      const saMatch = (key.match(/SA[_\- ]?(\d{1,3})/) || name.match(/SA[_\- ]?(\d{1,3})/) || name.match(/S[_\- ]?(\d{2,3})/));
      if (saMatch && saMatch[1]) return String(Number(saMatch[1]));
      if (key.indexOf('HAWK') >= 0 || name.indexOf('HAWK') >= 0) return 'HK';
      if (key.indexOf('NASAMS') >= 0 || name.indexOf('NASAMS') >= 0) return 'NS';
      if (key.indexOf('ROLAND') >= 0 || name.indexOf('ROLAND') >= 0) return 'RL';
      return 'UNK';
    }

    function resolveSamThreatRadarCode(text) {
      const src = String(text || '').toUpperCase();
      if (!src) return '';
      const tokenized = ' ' + src.replace(/[^A-Z0-9]+/g, ' ').trim() + ' ';
      if (tokenized.indexOf(' DE ') >= 0) return 'DE';
      if (tokenized.indexOf(' FF ') >= 0) return 'FF';
      if (tokenized.indexOf(' FL ') >= 0) return 'FL';
      if (tokenized.indexOf(' TS ') >= 0) return 'TS';
      if (tokenized.indexOf(' SD ') >= 0) return 'SD';
      if (tokenized.indexOf(' TK ') >= 0) return 'TK';
      if (tokenized.indexOf(' FS ') >= 0) return 'FS';
      if (tokenized.indexOf(' LB ') >= 0) return 'LB';
      if (tokenized.indexOf(' SF ') >= 0) return 'SF';
      if (tokenized.indexOf(' FD ') >= 0) return 'FD';
      if (tokenized.indexOf(' BB ') >= 0) return 'BB';
      if (tokenized.indexOf(' CS ') >= 0) return 'CS';
      if (tokenized.indexOf(' SP ') >= 0) return 'SP';
      if (tokenized.indexOf(' LR ') >= 0) return 'LR';
      if (tokenized.indexOf(' SH ') >= 0) return 'SH';
      if (src.indexOf('DOG EAR') >= 0 || src.indexOf('DOGEAR') >= 0) return 'DE';
      if (src.indexOf('SPOON REST') >= 0 || src.indexOf('SPOONREST') >= 0 || src.indexOf('PR-19') >= 0 || src.indexOf('P-19') >= 0 || src.indexOf('P19') >= 0 || src.indexOf('PR19') >= 0) return 'FF';
      if (src.indexOf('FLAT FACE') >= 0 || src.indexOf('FLATFACE') >= 0) return 'FF';
      if (src.indexOf('FLAP LID') >= 0 || src.indexOf('FLAPLID') >= 0) return 'FL';
      if (src.indexOf('TIN SHIELD') >= 0 || src.indexOf('TINSHIELD') >= 0) return 'TS';
      if (src.indexOf('SNOW DRIFT') >= 0 || src.indexOf('SNOWDRIFT') >= 0) return 'SD';
      if (src.indexOf('TALL KING') >= 0 || src.indexOf('TALLKING') >= 0) return 'TK';
      if (src.indexOf('FAN SONG') >= 0 || src.indexOf('FANSONG') >= 0) return 'FS';
      if (src.indexOf('LOW BLOW') >= 0 || src.indexOf('LOWBLOW') >= 0) return 'LB';
      if (src.indexOf('STRAIGHT FLUSH') >= 0 || src.indexOf('STRAIGHTFLUSH') >= 0) return 'SF';
      if (src.indexOf('FIRE DOME') >= 0 || src.indexOf('FIREDOME') >= 0) return 'FD';
      if (src.indexOf('BIG BIRD') >= 0 || src.indexOf('BIGBIRD') >= 0) return 'BB';
      if (src.indexOf('CLAM SHELL') >= 0 || src.indexOf('CLAMSHELL') >= 0) return 'CS';
      if (src.indexOf('SQUARE PAIR') >= 0 || src.indexOf('SQUAREPAIR') >= 0) return 'SP';
      if (src.indexOf('LAND ROLL') >= 0 || src.indexOf('LANDROLL') >= 0) return 'LR';
      if (src.indexOf('SCRUM HALF') >= 0 || src.indexOf('SCRUMHALF') >= 0) return 'SH';
      if (tokenized.indexOf(' STR ') >= 0) return 'STR';
      if (tokenized.indexOf(' SR ') >= 0) return 'SR';
      return '';
    }

    function isSamSupportRadarOnlyResolution(resolved, sourceLine, systemKey) {
      const key = String((systemKey || (resolved && resolved.systemKey) || '')).toUpperCase();
      const text = String(sourceLine || '').toUpperCase();
      if (!key && !text) return false;
      if (key === 'PR19_SPOON_REST' || key === 'PR14_TIN_SHIELD') return true;
      const tokenized = ' ' + text.replace(/[^A-Z0-9]+/g, ' ').trim() + ' ';
      if (tokenized.indexOf(' PR19 ') >= 0 || tokenized.indexOf(' P19 ') >= 0 || text.indexOf('SPOON REST') >= 0 || text.indexOf('SPOONREST') >= 0) return true;
      if (tokenized.indexOf(' PR14 ') >= 0 || tokenized.indexOf(' P14 ') >= 0 || text.indexOf('TIN SHIELD') >= 0 || text.indexOf('TINSHIELD') >= 0) return true;
      return false;
    }

    function isSamThreatTrackRadarResolved(text) {
      const src = String(text || '').toUpperCase();
      if (!src) return false;
      return src.indexOf('LOW BLOW') >= 0
        || src.indexOf('LOWBLOW') >= 0
        || src.indexOf('FAN SONG') >= 0
        || src.indexOf('FANSONG') >= 0
        || src.indexOf('FLAP LID') >= 0
        || src.indexOf('FLAPLID') >= 0
        || src.indexOf('FIRE DOME') >= 0
        || src.indexOf('FIREDOME') >= 0
        || src.indexOf('STRAIGHT FLUSH') >= 0
        || src.indexOf('STRAIGHTFLUSH') >= 0
        || src.indexOf('SCRUM HALF') >= 0
        || src.indexOf('SCRUMHALF') >= 0;
    }

    function resolveSamThreatNavalPlatformCode(displayName, sourceLine, systemKey) {
      const text = (String(displayName || '') + ' ' + String(sourceLine || '') + ' ' + String(systemKey || '')).toUpperCase();
      if (!text) return '';
      if (/\bCVN\b/.test(text)) return 'CVN';
      if (/\bCV\b/.test(text)) return 'CV';
      if (/\bCG\b/.test(text) || text.indexOf('CRUISER') >= 0) return 'CG';
      if (/\bDDG\b/.test(text)) return 'DDG';
      if (/\bDD\b/.test(text) || text.indexOf('DESTROYER') >= 0) return 'DD';
      if (/\bFFG\b/.test(text) || text.indexOf('FRIGATE') >= 0) return 'FFG';
      if (/\bLHA\b/.test(text)) return 'LHA';
      if (/\bLHD\b/.test(text)) return 'LHD';
      return '';
    }

    function isLikelySamThreatRadarText(text) {
      const src = String(text || '').toUpperCase();
      if (!src) return false;
      const tokenized = ' ' + src.replace(/[^A-Z0-9]+/g, ' ').trim() + ' ';
      const hasSamToken = tokenized.indexOf(' SAM ') >= 0
        || /(^|[^A-Z0-9])SA[\-_ ]?\d{1,3}([^A-Z0-9]|$)/.test(src)
        || src.indexOf('S-300') >= 0
        || src.indexOf('S-200') >= 0
        || src.indexOf('S-125') >= 0
        || src.indexOf('S-75') >= 0
        || src.indexOf('BUK') >= 0
        || src.indexOf('KUB') >= 0
        || src.indexOf('TOR') >= 0
        || src.indexOf('ROLAND') >= 0
        || src.indexOf('PATRIOT') >= 0
        || src.indexOf('HAWK') >= 0
        || src.indexOf('NASAMS') >= 0;
      if (!hasSamToken) return false;
      const isLauncher = src.indexOf('LAUNCHER') >= 0
        || /(^|[^A-Z0-9])TEL([^A-Z0-9]|$)/.test(src)
        || /(^|[^A-Z0-9])TELAR([^A-Z0-9]|$)/.test(src)
        || /(^|[^A-Z0-9])LN([^A-Z0-9]|$)/.test(src);
      if (isLauncher) return false;
      const hasRadarToken = tokenized.indexOf(' RADAR ') >= 0
        || tokenized.indexOf(' SR ') >= 0
        || tokenized.indexOf(' STR ') >= 0
        || src.indexOf('SEARCH') >= 0
        || src.indexOf('DOG EAR') >= 0
        || src.indexOf('DOGEAR') >= 0
        || src.indexOf('FLAT FACE') >= 0
        || src.indexOf('FLATFACE') >= 0
        || src.indexOf('SPOON REST') >= 0
        || src.indexOf('SPOONREST') >= 0
        || src.indexOf('P-19') >= 0
        || src.indexOf('PR-19') >= 0
        || src.indexOf('P19') >= 0
        || src.indexOf('PR19') >= 0
        || src.indexOf('FAN SONG') >= 0
        || src.indexOf('LOW BLOW') >= 0
        || src.indexOf('SNOW DRIFT') >= 0
        || src.indexOf('FLAP LID') >= 0
        || src.indexOf('BIG BIRD') >= 0
        || src.indexOf('CLAM SHELL') >= 0
        || src.indexOf('TIN SHIELD') >= 0;
      return hasRadarToken;
    }

    function readRadarActiveStateFromStatusText(statusText) {
      const src = String(statusText || '').trim();
      if (!src) return null;
      const upper = src.toUpperCase();
      const radarActiveMatch = upper.match(/RADARACTIVE\s*[:=]\s*(TRUE|FALSE|1|0|YES|NO|ON|OFF)/);
      if (radarActiveMatch && radarActiveMatch[1]) {
        const token = String(radarActiveMatch[1]).toUpperCase();
        return token === 'TRUE' || token === '1' || token === 'YES' || token === 'ON';
      }
      return null;
    }

    function isSamRadarEmitterActiveFromNode(node) {
      const row = node && typeof node === 'object' ? node : {};
      const statusValues = [
        row.status,
        row.Status,
        row.alarm,
        row.Alarm,
        row.alarmState,
        row.AlarmState,
      ];

      for (let i = 0; i < statusValues.length; i++) {
        const parsed = readRadarActiveStateFromStatusText(statusValues[i]);
        if (parsed !== null) return parsed;
      }

      return null;
    }

    function extractSamThreatNorthEast(obj) {
      const row = obj && typeof obj === 'object' ? obj : null;
      if (!row) return null;
      const pos = row.pos && typeof row.pos === 'object' ? row.pos : null;
      const posX = Number(pos && pos.x);
      const posZ = Number(pos && pos.z);
      if (isFinite(posX) && isFinite(posZ)) {
        return { xNum: posX, yNum: posZ };
      }
      const x = Number(row.x);
      const y = Number(row.y);
      const X = Number(row.X);
      const Y = Number(row.Y);
      const Z = Number(row.Z);
      if (isFinite(X) && isFinite(Y)) {
        return { xNum: X, yNum: Y };
      }
      if (isFinite(x) && isFinite(y) && Math.abs(x) > 1000 && Math.abs(y) > 1000) {
        return { xNum: x, yNum: y };
      }
      if (isFinite(x) && isFinite(Z)) {
        return { xNum: x, yNum: Z };
      }
      return null;
    }

    function parseAwacsSamThreatLineEntries(lines) {
      const rows = Array.isArray(lines) ? lines : [];
      const entries = [];
      let inThreatBlock = false;
      let section = '';
      rows.forEach(function (line) {
        const text = String(line || '').trim();
        if (!text) return;
        if (/^DATALINK\s+THREATS\s*:/i.test(text)) {
          inThreatBlock = true;
          section = '';
          return;
        }
        if (!inThreatBlock) return;
        const sectionMatch = text.match(/^([A-Z]+)\s*:\s*$/i);
        if (sectionMatch && sectionMatch[1]) {
          section = String(sectionMatch[1]).toUpperCase();
          return;
        }
        if (text.toUpperCase().indexOf('NO DATALINK THREATS ARE DISPLAYED') >= 0) return;
        const sectionUpper = String(section || '').toUpperCase();
        const isSamSection = sectionUpper === 'SAM';
        const isEwrSamLine = sectionUpper === 'EWR' && (isLikelySamThreatRadarText(text) || /^SAM\b/i.test(text));
        if (!isSamSection && !isEwrSamLine) return;

        const textRadarActive = readRadarActiveStateFromStatusText(text);
        if (textRadarActive === false) return;

        const bra = text.match(/\b(\d{3})\s*\/\s*(\d{1,3}(?:\.\d+)?)\s*\//);
        const braIdx = bra ? text.indexOf(bra[0]) : -1;
        const sourceLabel = (braIdx > 0 ? text.substring(0, braIdx) : text)
          .replace(/\s+\d+\s+CONTACTS?$/i, '')
          .trim();
        const resolved = resolveSamThreatEntryFromText(sourceLabel || text) || {};
        const displayName = String(resolved.displayName || sourceLabel || 'SAM').trim();
        const systemKey = String(resolved.systemKey || 'SAM_UNKNOWN').trim();
        entries.push({
          text: text,
          sourceLabel: sourceLabel,
          displayName: displayName,
          systemKey: systemKey,
          resolved: resolved,
          bearing: bra ? Number(bra[1]) : NaN,
          rangeNm: bra ? Number(bra[2]) : NaN,
        });
      });
      return entries;
    }

    function collectDirectSamThreatCandidates(data) {
      const model = data || latestData || {};
      const server = (model && model.Server) || {};
      const roots = [server.Diagnostics, server.Payload];
      const seenNodes = [];
      const candidates = [];
      const dedupe = {};

      function scan(node, path, depth) {
        if (!node || typeof node !== 'object') return;
        if (depth > 7) return;
        if (seenNodes.indexOf(node) >= 0) return;
        seenNodes.push(node);

        if (Array.isArray(node)) {
          for (let i = 0; i < node.length; i++) {
            scan(node[i], path + '[' + String(i) + ']', depth + 1);
          }
          return;
        }

        const rowText = [
          node.typename, node.TypeName,
          node.fullname, node.FullName,
          node.descr, node.description,
          node.callsign, node.Callsign,
          node.name, node.Name,
        ].map(function (v) { return String(v || '').trim(); }).filter(function (v) { return !!v; }).join(' ');

        const threatPos = extractSamThreatNorthEast(node);
        if (threatPos && isLikelySamThreatRadarText(rowText)) {
          const radarActive = isSamRadarEmitterActiveFromNode(node);
          const resolved = resolveSamThreatEntryFromText(rowText) || {};
          const systemKey = String(resolved.systemKey || 'SAM_UNKNOWN').trim();
          const key = String(Math.round(Number(threatPos.xNum) / 50) * 50) + '|' + String(Math.round(Number(threatPos.yNum) / 50) * 50) + '|' + systemKey;
          if (dedupe[key] === undefined) {
            dedupe[key] = candidates.length;
            candidates.push({
              xNum: Number(threatPos.xNum),
              yNum: Number(threatPos.yNum),
              sourceText: rowText,
              systemKey: systemKey,
              resolved: resolved,
              radarActive: radarActive,
            });
          } else {
            const existing = candidates[dedupe[key]];
            if (existing) {
              if (radarActive === true) {
                existing.radarActive = true;
              } else if (existing.radarActive !== true && radarActive === false) {
                existing.radarActive = false;
              }
            }
          }
        }

        Object.keys(node).forEach(function (k) {
          const child = node[k];
          if (child && typeof child === 'object') {
            scan(child, path ? (path + '.' + k) : k, depth + 1);
          }
        });
      }

      roots.forEach(function (root, idx) {
        scan(root, idx === 0 ? 'diagnostics' : 'payload', 0);
      });

      return candidates;
    }

    function applySamRadarAssociationRules(candidates) {
      const rows = Array.isArray(candidates) ? candidates : [];
      if (!rows.length) return rows;

      const sa3Entry = resolveSamThreatEntryFromText('LOW BLOW') || resolveSamThreatEntryFromText('SA-3') || null;
      const sa2Entry = resolveSamThreatEntryFromText('FAN SONG') || resolveSamThreatEntryFromText('SA-2') || null;
      rows.forEach(function (candidate) {
        if (!candidate) return;
        const token = normalizeSamLookupToken((candidate.sourceText || '') + ' ' + (candidate.systemKey || ''));
        const candidateKey = String(candidate.systemKey || '').toUpperCase();
        const isPr19 = token.indexOf('PR19') >= 0
          || token.indexOf('P19') >= 0
          || token.indexOf('SPOONREST') >= 0
          || candidateKey === 'PR19_SPOON_REST';
        if (!isPr19) {
          return;
        }

        const hasNearbySa3 = rows.some(function (other) {
          if (!other || other === candidate) return false;
          const otherKey = String(other.systemKey || '').toUpperCase();
          const otherTextToken = normalizeSamLookupToken(other.sourceText || '');
          const isSa3 = otherKey === 'SA3_GOA' || otherTextToken.indexOf('LOWBLOW') >= 0 || otherTextToken.indexOf('SNR125') >= 0;
          if (!isSa3) return false;
          const dn = Number(other.xNum) - Number(candidate.xNum);
          const de = Number(other.yNum) - Number(candidate.yNum);
          const dist = Math.sqrt((dn * dn) + (de * de));
          return isFinite(dist) && dist <= 3500;
        });

        if (hasNearbySa3 && sa3Entry) {
          candidate.resolved = Object.assign({}, sa3Entry);
          candidate.systemKey = String(sa3Entry.systemKey || 'SA3_GOA');
          candidate.sourceText = String(candidate.sourceText || '') + ' LOW BLOW';
          candidate.systemResolved = true;
          return;
        }

        const hasNearbySa2 = rows.some(function (other) {
          if (!other || other === candidate) return false;
          const otherKey = String(other.systemKey || '').toUpperCase();
          const otherTextToken = normalizeSamLookupToken(other.sourceText || '');
          const isSa2 = otherKey === 'SA2_GUIDELINE' || otherTextToken.indexOf('FANSONG') >= 0 || otherTextToken.indexOf('SNR75') >= 0;
          if (!isSa2) return false;
          const dn = Number(other.xNum) - Number(candidate.xNum);
          const de = Number(other.yNum) - Number(candidate.yNum);
          const dist = Math.sqrt((dn * dn) + (de * de));
          return isFinite(dist) && dist <= 3500;
        });

        if (hasNearbySa2 && sa2Entry) {
          candidate.resolved = Object.assign({}, sa2Entry);
          candidate.systemKey = String(sa2Entry.systemKey || 'SA2_GUIDELINE');
          candidate.sourceText = String(candidate.sourceText || '') + ' FAN SONG';
          candidate.systemResolved = true;
        }
      });

      return rows;
    }

    function parseAwacsSamThreatMapPoints(data) {
      const model = data || latestData || {};
      const unitRows = getMergedList(model && model.Units, 'AWACS');
      const detailRows = getMergedList(model && model.UnitDetails, 'AWACS');
      const lines = (Array.isArray(unitRows) ? unitRows : []).concat(Array.isArray(detailRows) ? detailRows : []);

      const ownship = getPlayerMapPoint(model);
      const ownNorth = Number(ownship && ownship.xNum);
      const ownEast = Number(ownship && ownship.yNum);
      const lineEntries = parseAwacsSamThreatLineEntries(lines);
      const directCandidatesAll = collectDirectSamThreatCandidates(model);
      const directCandidates = directCandidatesAll.filter(function (candidate) {
        return !!(candidate && candidate.radarActive === true);
      });
      const radarStateBySystem = {};
      directCandidatesAll.forEach(function (candidate) {
        if (!candidate) return;
        const key = String(candidate.systemKey || '').toUpperCase().trim();
        if (!key) return;
        if (!radarStateBySystem[key]) {
          radarStateBySystem[key] = { active: false, inactive: false };
        }
        if (candidate.radarActive === true) radarStateBySystem[key].active = true;
        if (candidate.radarActive === false) radarStateBySystem[key].inactive = true;
      });
      const filteredLineEntries = lineEntries.filter(function (entry) {
        const systemKeyUpper = String((entry && entry.systemKey) || '').toUpperCase().trim();
        if (!systemKeyUpper) return true;
        const state = radarStateBySystem[systemKeyUpper];
        if (!state) return true;
        if (state.active) return true;
        return !state.inactive;
      });
      const points = [];
      const seen = {};
      const nextResolvedStateByAnchor = {};
      const nextBraAnchorByKey = {};

      function buildBraAnchorKey(entry) {
        const e = entry || {};
        const systemKey = String(e.systemKey || '').toUpperCase().trim();
        const labelToken = normalizeSamLookupToken(String(e.sourceLabel || e.displayName || e.text || ''));
        const bearing = Number(e.bearing);
        const rangeNm = Number(e.rangeNm);
        const bearingKey = isFinite(bearing) ? String(Math.round(bearing)) : '';
        const rangeKey = isFinite(rangeNm) ? String(Math.round(rangeNm * 10) / 10) : '';
        return ['SAMBRA', systemKey, labelToken, bearingKey, rangeKey].join('|');
      }

      function pushSamPoint(north, east, entryLike, sourceLineOverride) {
        const resolved = (entryLike && entryLike.resolved) || {};
        const displayName = String((entryLike && entryLike.displayName) || resolved.displayName || 'SAM').trim();
        let systemKey = String((entryLike && entryLike.systemKey) || resolved.systemKey || 'SAM_UNKNOWN').trim();
        const categoryUpper = String(resolved.category || 'SAM').toUpperCase();
        const anchorKey = 'SAMANCHOR|'
          + String(Math.round(Number(north) / 1000.0) * 1000)
          + '|'
          + String(Math.round(Number(east) / 1000.0) * 1000);
        const sourceLine = String(sourceLineOverride || (entryLike && entryLike.text) || displayName);
        let samCode = resolveSamThreatShortCode(displayName, systemKey);
        const radarCode = resolveSamThreatRadarCode(sourceLine + ' ' + displayName + ' ' + systemKey);
        const explicitTrackResolved = isSamThreatTrackRadarResolved(sourceLine + ' ' + displayName + ' ' + systemKey);
        const supportRadarOnly = isSamSupportRadarOnlyResolution(resolved, sourceLine, systemKey);
        let hasResolvedSystem = !!(
          (entryLike && entryLike.systemResolved === true)
          || explicitTrackResolved
          || (!supportRadarOnly && systemKey && String(systemKey).toUpperCase() !== 'SAM_UNKNOWN')
        );
        let preferredCode = (!hasResolvedSystem && radarCode) ? radarCode : samCode;
        let samIconResolved = !!(preferredCode && preferredCode !== 'UNK');
        const navalPlatformCode = (categoryUpper === 'NAVAL') ? resolveSamThreatNavalPlatformCode(displayName, sourceLine, systemKey) : '';
        let samDisplayCode = navalPlatformCode
          ? (navalPlatformCode + (preferredCode && preferredCode !== 'SAM' ? preferredCode : ''))
          : preferredCode;
        const confirmedHostile = isSamThreatConfirmedHostileFromText(sourceLine);
        const outerNm = Number(resolved.rangeNmMaxAssumed);
        let outerRangeNm = isFinite(outerNm) && outerNm > 0
          ? outerNm
          : getSamThreatFallbackRangeNmByCategory(resolved.category);
        const innerNm = Number(resolved.rangeNmMin);
        const innerRangeNm = (isFinite(innerNm) && innerNm > 0 && innerNm < outerRangeNm) ? innerNm : NaN;
        const confidence = String(resolved.confidence || 'medium').toLowerCase();

        if (!hasResolvedSystem) {
          const prior = (samThreatResolvedStateByAnchor && samThreatResolvedStateByAnchor[anchorKey]) || null;
          if (prior && prior.hasResolvedSystem) {
            hasResolvedSystem = true;
            if (prior.systemKey) systemKey = String(prior.systemKey);
            if (prior.samCode) samCode = String(prior.samCode);
            if (prior.samPreferredCode) preferredCode = String(prior.samPreferredCode);
            if (prior.samDisplayCode) samDisplayCode = String(prior.samDisplayCode);
            samIconResolved = !!(preferredCode && preferredCode !== 'UNK');
            const priorOuter = Number(prior.outerRangeNm);
            if (isFinite(priorOuter) && priorOuter > 0) {
              outerRangeNm = priorOuter;
            }
          }
        }

        const assetKey = makeSamThreatAssetKey(systemKey, north, east, displayName);
        if (seen[assetKey]) return;
        seen[assetKey] = true;

        const ringsNm = [];
        if (isFinite(innerRangeNm) && innerRangeNm > 0) ringsNm.push(innerRangeNm);
        ringsNm.push(outerRangeNm);

        const confidenceBandsNm = confidence === 'low'
          ? [outerRangeNm * 0.7, outerRangeNm]
          : (confidence === 'medium' ? [outerRangeNm * 0.85, outerRangeNm] : []);

        points.push({
          xNum: north,
          yNum: east,
          label: displayName,
          systemKey: systemKey,
          confidence: confidence,
          category: categoryUpper,
          ringRadiiNm: ringsNm,
          confidenceBandsNm: confidenceBandsNm,
          radiusMeters: outerRangeNm * 1852.0,
          ring: true,
          samCode: samCode,
          samRadarCode: radarCode,
          samPreferredCode: preferredCode,
          samHasResolvedSystem: hasResolvedSystem,
          samIconResolved: samIconResolved,
          samDisplayCode: samDisplayCode,
          samNavalPlatformCode: navalPlatformCode,
          samConfirmedHostile: confirmedHostile,
          assetKey: assetKey,
          sourceLine: sourceLine,
        });

        nextResolvedStateByAnchor[anchorKey] = {
          hasResolvedSystem: hasResolvedSystem,
          systemKey: systemKey,
          samCode: samCode,
          samPreferredCode: preferredCode,
          samDisplayCode: samDisplayCode,
          outerRangeNm: outerRangeNm,
        };
      }

      if (directCandidates.length && filteredLineEntries.length) {
        const used = {};
        filteredLineEntries.forEach(function (entry) {
          let bestIdx = -1;
          let bestScore = Number.POSITIVE_INFINITY;
          directCandidates.forEach(function (candidate, idx) {
            if (used[idx]) return;
            if (String(candidate.systemKey || '').toUpperCase() !== String(entry.systemKey || '').toUpperCase()) return;
            let score = 0;
            const candidateResolved = candidate.resolved || {};
            const candDisplay = String(candidateResolved.displayName || candidate.sourceText || '').toUpperCase();
            const entryDisplay = String(entry.displayName || '').toUpperCase();
            if (candDisplay !== entryDisplay) {
              score += 5;
            }
            if (isFinite(Number(entry.rangeNm)) && isFinite(ownNorth) && isFinite(ownEast)) {
              const dn = Number(candidate.xNum) - ownNorth;
              const de = Number(candidate.yNum) - ownEast;
              const distNm = Math.sqrt((dn * dn) + (de * de)) / 1852.0;
              score += Math.abs(distNm - Number(entry.rangeNm));
            }
            if (score < bestScore) {
              bestScore = score;
              bestIdx = idx;
            }
          });
          if (bestIdx >= 0) {
            used[bestIdx] = true;
            const c = directCandidates[bestIdx];
            const mergedEntry = {
              text: entry.text,
              displayName: entry.displayName,
              systemKey: entry.systemKey,
              resolved: Object.assign({}, c.resolved || {}, entry.resolved || {}),
            };
            pushSamPoint(Number(c.xNum), Number(c.yNum), mergedEntry, entry.text);
          }
        });
        if (points.length) {
          samThreatResolvedStateByAnchor = nextResolvedStateByAnchor;
          samThreatBraAnchorByKey = nextBraAnchorByKey;
          return points;
        }
      }

      if (directCandidates.length && !filteredLineEntries.length) {
        directCandidates.forEach(function (candidate) {
          const mergedEntry = {
            text: String(candidate && candidate.sourceText || ''),
            displayName: String((candidate && candidate.resolved && candidate.resolved.displayName) || (candidate && candidate.sourceText) || 'SAM').trim(),
            systemKey: String((candidate && candidate.systemKey) || normalizeSamLookupToken((candidate && candidate.sourceText) || '') || 'SAM_UNKNOWN').trim(),
            resolved: Object.assign({}, (candidate && candidate.resolved) || {}),
          };
          pushSamPoint(Number(candidate.xNum), Number(candidate.yNum), mergedEntry, mergedEntry.text);
        });
        if (points.length) {
          samThreatResolvedStateByAnchor = nextResolvedStateByAnchor;
          samThreatBraAnchorByKey = nextBraAnchorByKey;
          return points;
        }
      }

      if (isFinite(ownNorth) && isFinite(ownEast)) {
        filteredLineEntries.forEach(function (entry) {
          if (efbSaSamThreatCoalitionMode !== 'hostile' && efbSaSamThreatCoalitionMode !== 'all') return;
          const bearing = Number(entry && entry.bearing);
          const rangeNm = Number(entry && entry.rangeNm);
          if (!isFinite(bearing) || !isFinite(rangeNm) || rangeNm <= 0) return;
          const braAnchorKey = buildBraAnchorKey(entry);
          const priorAnchor = braAnchorKey ? samThreatBraAnchorByKey[braAnchorKey] : null;
          let north = Number(priorAnchor && priorAnchor.north);
          let east = Number(priorAnchor && priorAnchor.east);
          if (!isFinite(north) || !isFinite(east)) {
            const rad = bearing * (Math.PI / 180.0);
            const distMeters = rangeNm * 1852.0;
            north = ownNorth + (Math.cos(rad) * distMeters);
            east = ownEast + (Math.sin(rad) * distMeters);
          }
          if (!isFinite(north) || !isFinite(east)) return;
          if (braAnchorKey) {
            nextBraAnchorByKey[braAnchorKey] = { north: north, east: east };
          }
          pushSamPoint(north, east, entry, entry.text);
        });
      }
      samThreatResolvedStateByAnchor = nextResolvedStateByAnchor;
      samThreatBraAnchorByKey = nextBraAnchorByKey;
      return points;
    }

    function getAwacsSamThreatSelectionFromLine(data, line) {
      const lineToken = normalizeSamLookupToken(String(line || '').replace(/\s+/g, ' ').trim());
      if (!lineToken) return null;
      const points = parseAwacsSamThreatMapPoints(data);
      const match = points.find(function (p) {
        const src = normalizeSamLookupToken(String((p && p.sourceLine) || '').replace(/\s+/g, ' ').trim());
        return src && (src === lineToken || src.indexOf(lineToken) >= 0 || lineToken.indexOf(src) >= 0);
      });
      return match || null;
    }

    function decorateAwacsSamSelectionText(text, data) {
      const content = String(text || '');
      if (!content) return content;
      const selectedKeys = Array.isArray(efbAwacsSelectedSamThreatKeys)
        ? efbAwacsSelectedSamThreatKeys.map(function (k) { return String(k || '').trim(); }).filter(function (k) { return !!k; })
        : [];
      if (!selectedKeys.length) return content;

      const points = parseAwacsSamThreatMapPoints(data);
      const selectedMap = {};
      selectedKeys.forEach(function (k) { selectedMap[getSamThreatSelectionKey(k)] = true; });
      const selected = points.filter(function (p) {
        const key = getSamThreatSelectionKey(String((p && p.assetKey) || '').trim());
        return !!selectedMap[key];
      });
      if (!selected.length) return content;

      const lines = content.split(/\r?\n/);
      const selectedLineTokens = selected
        .map(function (p) { return normalizeSamLookupToken(String((p && p.sourceLine) || '').replace(/\s+/g, ' ').trim()); })
        .filter(function (v) { return !!v; });
      const selectedLabels = selected
        .map(function (p) { return String((p && p.label) || '').trim(); })
        .filter(function (v) { return !!v; });
      const highlighted = lines.map(function (ln) {
        const row = String(ln || '').trim();
        if (!row) return ln;
        const rowToken = normalizeSamLookupToken(row);
        const isSelectedRow = selectedLineTokens.some(function (selectedToken) {
          return !!(selectedToken && rowToken && (rowToken === selectedToken || rowToken.indexOf(selectedToken) >= 0 || selectedToken.indexOf(rowToken) >= 0));
        });
        if (isSelectedRow) {
          return '▶ ' + ln;
        }
        return ln;
      });
      highlighted.push('');
      highlighted.push('Selected SAMs: ' + (selectedLabels.length ? selectedLabels.join(', ') : String(selected.length)));
      return highlighted.join('\n');
    }

    function filterAwacsSamThreatPointsBySelection(points) {
      const rows = Array.isArray(points) ? points : [];
      const selectedKeys = Array.isArray(efbAwacsSelectedSamThreatKeys)
        ? efbAwacsSelectedSamThreatKeys.map(function (k) { return String(k || '').trim(); }).filter(function (k) { return !!k; })
        : [];
      if (!selectedKeys.length) return rows;
      const selectedMap = {};
      selectedKeys.forEach(function (k) { selectedMap[getSamThreatSelectionKey(k)] = true; });
      const filtered = rows.filter(function (p) {
        const key = getSamThreatSelectionKey(String((p && p.assetKey) || '').trim());
        return !!selectedMap[key];
      });
      if (rows.length && !filtered.length) {
        efbAwacsSelectedSamThreatKeys = [];
        return rows;
      }
      return filtered;
    }

    function parseBraFromUnitLine(line) {
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

      const drawerListCapture = document.getElementById('efbDrawerList');
      if (drawerListCapture && !drawerListCapture.__efbUserWpCaptureBound) {
        drawerListCapture.__efbUserWpCaptureBound = true;
        ['pointerdown', 'click', 'mousedown', 'mouseup', 'touchstart', 'touchend'].forEach(function (evtName) {
          drawerListCapture.addEventListener(evtName, function (ev) {
            if (typeof ev.stopPropagation === 'function') ev.stopPropagation();
          }, true);
        });
      }

    function buildBraBearingMap(data) {
      const result = {};
      const cats = ['TANKER', 'AWACS', 'JTAC', 'FLIGHT'];
      cats.forEach(function (cat) {
        const lines = getMergedList(data && data.Units, cat);
        (Array.isArray(lines) ? lines : []).forEach(function (line) {
          const parsed = parseBraFromUnitLine(line);
          if (!parsed || !parsed.callsignKey || !isFinite(parsed.bearing)) return;
          if (result[parsed.callsignKey] === undefined) {
            result[parsed.callsignKey] = parsed.bearing;
          }
        });
      });
      return result;
    }

    function angularDifferenceDeg(a, b) {
      const da = Number(a);
      const db = Number(b);
      if (!isFinite(da) || !isFinite(db)) return 180;
      return Math.abs((((da - db) % 360) + 540) % 360 - 180);
    }

    function resolveAssetAxisSwap(assets, data) {
      const list = Array.isArray(assets) ? assets : [];
      if (!list.length) return false;

      const ownship = list.find(function (a) { return String(a && a.category || '').toUpperCase() === 'PLAYER'; });
      if (!ownship) return false;

      const ownX = Number(ownship.rawX);
      const ownY = Number(ownship.rawY);
      if (!isFinite(ownX) || !isFinite(ownY)) return false;

      const bearingMap = buildBraBearingMap(data);
      const samples = list.filter(function (a) {
        const key = normalizeAssetCallsignKey(a && a.callsign);
        return key && bearingMap[key] !== undefined && a !== ownship;
      }).slice(0, 10);
      if (!samples.length) return false;

      function score(swapped) {
        let total = 0;
        let count = 0;
        samples.forEach(function (a) {
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

    function normalizeCoalitionLayerName(value) {
      const v = String(value || '').trim().toUpperCase();
      if (!v) return '';
      if (v === 'RED') return 'RED';
      if (v === 'BLUE') return 'BLUE';
      if (v === 'NEUTRAL' || v === 'NEUTRALS') return 'NEUTRAL';
      if (v === 'COMMON') return 'COMMON';
      return '';
    }

    function getOwnCoalitionLayerName(data) {
      const server = (data && data.Server) || {};
      const coalitionRaw = server.PlayerCoalition;
      const coalitionNum = Number(coalitionRaw);
      if (isFinite(coalitionNum)) {
        if (coalitionNum === 1) return 'RED';
        if (coalitionNum === 2) return 'BLUE';
        if (coalitionNum === 0) return 'NEUTRAL';
      }

      const normalizedDirect = normalizeCoalitionLayerName(coalitionRaw);
      if (normalizedDirect) return normalizedDirect;

      const diagnostics = (server && server.Diagnostics && typeof server.Diagnostics === 'object')
        ? server.Diagnostics
        : {};
      const bullseyeCoalition = diagnostics.bullseyeCoalition;
      const normalizedBullseye = normalizeCoalitionLayerName(bullseyeCoalition);
      if (normalizedBullseye) return normalizedBullseye;

      return '';
    }

    let missionDrawingSnapshotCache = null;
    let missionDrawingSnapshotCacheMissionKey = '';

    function getMissionDrawingSnapshotMissionKey(server) {
      const s = (server && typeof server === 'object') ? server : {};
      const mission = String(s.MissionTitle || '').trim();
      const theatre = String(s.Theater || '').trim();
      const mode = (s.Multiplayer === true) ? 'MP' : 'SP';
      return (mission + '|' + theatre + '|' + mode).toUpperCase();
    }

    function resolveMissionDrawingSnapshotForSaMap(server) {
      const missionKey = getMissionDrawingSnapshotMissionKey(server);
      const liveSnapshot = (server && server.MissionDrawings && typeof server.MissionDrawings === 'object')
        ? server.MissionDrawings
        : null;

      if (liveSnapshot) {
        missionDrawingSnapshotCache = liveSnapshot;
        missionDrawingSnapshotCacheMissionKey = missionKey;
        return liveSnapshot;
      }

      if (missionDrawingSnapshotCache && (!missionKey || missionKey === missionDrawingSnapshotCacheMissionKey)) {
        return missionDrawingSnapshotCache;
      }

      // MP startup can publish drawings before MissionTitle stabilizes.
      // Preserve that cache when key transitions from blank -> concrete key.
      if (missionDrawingSnapshotCache && missionKey && !missionDrawingSnapshotCacheMissionKey) {
        missionDrawingSnapshotCacheMissionKey = missionKey;
        return missionDrawingSnapshotCache;
      }

      if (missionDrawingSnapshotCache && missionKey && missionDrawingSnapshotCacheMissionKey && missionKey !== missionDrawingSnapshotCacheMissionKey) {
        missionDrawingSnapshotCache = null;
        missionDrawingSnapshotCacheMissionKey = missionKey;
      }

      return null;
    }

    function getMissionDrawingObjectsForSaMap(data) {
      if (efbSaShowMissionDrawings === false) return [];
      const server = (data && data.Server) || {};
      const isMultiplayer = (server && server.Multiplayer === true);
      const snapshot = resolveMissionDrawingSnapshotForSaMap(server);
      if (!snapshot) return [];

      const ownCoalition = getOwnCoalitionLayerName(data);
      const allowedLayers = { COMMON: true };
      if (ownCoalition) {
        allowedLayers[ownCoalition] = true;
      } else {
        // MP fallback: if own coalition isn't resolved yet, keep overlays visible.
        allowedLayers.RED = true;
        allowedLayers.BLUE = true;
        allowedLayers.NEUTRAL = true;
      }

      const layers = Array.isArray(snapshot.layers) ? snapshot.layers : (Array.isArray(snapshot.Layers) ? snapshot.Layers : []);
      const rows = [];

      layers.forEach(function (layer) {
        const rawLayerName = String((layer && (layer.name || layer.Name)) || '').trim();
        const normalizedLayerName = normalizeCoalitionLayerName(rawLayerName);
        const layerName = normalizedLayerName || 'COMMON';
        if (!isMultiplayer && !allowedLayers[layerName]) return;
        const layerVisible = (layer && (layer.visible !== undefined ? layer.visible : layer.Visible));
        if (layerVisible === false) return;

        const objects = Array.isArray(layer && layer.objects) ? layer.objects : (Array.isArray(layer && layer.Objects) ? layer.Objects : []);
        objects.forEach(function (obj) {
          if (!obj || typeof obj !== 'object') return;
          if ((obj.visible !== undefined ? obj.visible : obj.Visible) === false) return;

          const primitiveType = String((obj.primitiveType !== undefined ? obj.primitiveType : obj.PrimitiveType) || '').trim();
          const mapX = Number(obj.mapX !== undefined ? obj.mapX : obj.MapX);
          const mapY = Number(obj.mapY !== undefined ? obj.mapY : obj.MapY);
          if (!primitiveType || !isFinite(mapX) || !isFinite(mapY)) return;

          const pointsRaw = Array.isArray(obj.points) ? obj.points : (Array.isArray(obj.Points) ? obj.Points : []);
          const points = pointsRaw
            .map(function (p) {
              const px = Number(p && (p.x !== undefined ? p.x : p.X));
              const py = Number(p && (p.y !== undefined ? p.y : p.Y));
              if (!isFinite(px) || !isFinite(py)) return null;
              return { x: px, y: py };
            })
            .filter(function (p) { return !!p; });

          rows.push({
            primitiveType: primitiveType,
            layerName: layerName,
            mapX: mapX,
            mapY: mapY,
            angle: Number(obj.angle !== undefined ? obj.angle : obj.Angle),
            width: Number(obj.width !== undefined ? obj.width : obj.Width),
            height: Number(obj.height !== undefined ? obj.height : obj.Height),
            radius: Number(obj.radius !== undefined ? obj.radius : obj.Radius),
            thickness: Number(obj.thickness !== undefined ? obj.thickness : obj.Thickness),
            borderThickness: Number(obj.borderThickness !== undefined ? obj.borderThickness : obj.BorderThickness),
            polygonMode: String((obj.polygonMode !== undefined ? obj.polygonMode : obj.PolygonMode) || '').trim().toLowerCase(),
            lineMode: String((obj.lineMode !== undefined ? obj.lineMode : obj.LineMode) || '').trim().toLowerCase(),
            closed: !!(obj.closed !== undefined ? obj.closed : obj.Closed),
            style: String((obj.style !== undefined ? obj.style : obj.Style) || '').trim().toLowerCase(),
            colorString: String((obj.colorString !== undefined ? obj.colorString : obj.ColorString) || '').trim(),
            fillColorString: String((obj.fillColorString !== undefined ? obj.fillColorString : obj.FillColorString) || '').trim(),
            text: String((obj.text !== undefined ? obj.text : obj.Text) || '').trim(),
            fontSize: Number(obj.fontSize !== undefined ? obj.fontSize : obj.FontSize),
            points: points,
          });
        });
      });

      return rows;
    }

    function extractIcaoToken(text) {
      const s = String(text || '').toUpperCase();
      if (!s) return '';
      const m4 = s.match(/\b([A-Z0-9]{4})\b/);
      if (m4) return String(m4[1] || '');
      const m3 = s.match(/\b([A-Z]{3})\b/);
      return m3 ? String(m3[1] || '') : '';
    }

    function inferAirfieldType(text) {
      const s = String(text || '').toUpperCase();
      if (!s) return 'airport';
      if (s.indexOf('SEAPLANE') >= 0 || s.indexOf('SEA PLANE') >= 0 || s.indexOf('WATER') >= 0) return 'seaplane';
      if (s.indexOf('HELIPORT') >= 0 || s.indexOf('HELI') >= 0 || s.indexOf('FARP') >= 0) return 'heliport';
      return 'airport';
    }

    function inferAirfieldMilitary(text) {
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

    function parseMetarVisibilityMeters(metarText) {
      const text = String(metarText || '').toUpperCase().trim();
      if (!text) return NaN;
      if (text.indexOf('CAVOK') >= 0) return 10000;

      const meter = text.match(/(?:^|\s)(\d{4})(?:\s|$)/);
      if (meter) {
        const mv = Number(meter[1]);
        if (isFinite(mv) && mv > 0) return mv;
      }

      const sm = text.match(/(?:^|\s)(P?\d{1,2}(?:\s+\d\/\d)?|\d\/\d)SM(?:\s|$)/);
      if (sm) {
        const token = String(sm[1] || '').trim();
        let miles = NaN;
        if (token.indexOf('/') >= 0 && token.indexOf(' ') < 0) {
          const parts = token.split('/');
          const n = Number(parts[0]);
          const d = Number(parts[1]);
          if (isFinite(n) && isFinite(d) && d > 0) miles = n / d;
        } else {
          const clean = token.replace(/^P/i, '').trim();
          const mixed = clean.match(/^(\d+)\s+(\d)\/(\d)$/);
          if (mixed) {
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

    function isMetarCloudVfr(metarText) {
      const text = String(metarText || '').toUpperCase().trim();
      if (!text) return false;
      if (text.indexOf('CAVOK') >= 0) return true;

      const rx = /\b(FEW|SCT|BKN|OVC)(\d{3})\b/g;
      let match = null;
      let lowSctCount = 0;
      while ((match = rx.exec(text)) !== null) {
        const layer = String(match[1] || '').toUpperCase();
        const baseHundreds = Number(match[2]);
        if (!isFinite(baseHundreds)) continue;
        const ft = baseHundreds * 100;
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

    function isMetarVfr(metarText) {
      const visMeters = parseMetarVisibilityMeters(metarText);
      const cloudVfr = isMetarCloudVfr(metarText);
      return isFinite(visMeters) && cloudVfr && visMeters >= 5000;
    }

    function getEfbSaMapToggleIconSvg(isSaMapMode) {
      if (isSaMapMode) {
        return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
          + '<rect x="11" y="7" width="42" height="50" rx="2" ry="2" fill="none" stroke="currentColor" stroke-width="3.5"/>'
          + '<line x1="11" y1="16" x2="53" y2="16" stroke="currentColor" stroke-width="2.6"/>'
          + '<line x1="14" y1="12" x2="30" y2="12" stroke="currentColor" stroke-width="2"/>'
          + '<line x1="34" y1="12" x2="50" y2="12" stroke="currentColor" stroke-width="2"/>'
          + '<line x1="14" y1="21" x2="33" y2="21" stroke="currentColor" stroke-width="1.8" opacity="0.9"/>'
          + '<line x1="14" y1="25" x2="30" y2="25" stroke="currentColor" stroke-width="1.8" opacity="0.9"/>'
          + '<line x1="35" y1="21" x2="50" y2="21" stroke="currentColor" stroke-width="1.6" opacity="0.8"/>'
          + '<line x1="35" y1="25" x2="48" y2="25" stroke="currentColor" stroke-width="1.6" opacity="0.8"/>'
          + '<path d="M16 45c4-7 10-9 15-7 4 2 9 2 17-3" fill="none" stroke="currentColor" stroke-width="2.3" stroke-linecap="round"/>'
          + '<circle cx="31" cy="37" r="2.4" fill="none" stroke="currentColor" stroke-width="1.8"/>'
          + '<line x1="31" y1="33" x2="31" y2="41" stroke="currentColor" stroke-width="1.5"/>'
          + '<line x1="27" y1="37" x2="35" y2="37" stroke="currentColor" stroke-width="1.5"/>'
          + '<rect x="14" y="48" width="15" height="7" fill="none" stroke="currentColor" stroke-width="1.8"/>'
          + '<line x1="33" y1="50" x2="50" y2="50" stroke="currentColor" stroke-width="1.8"/>'
          + '<line x1="33" y1="54" x2="47" y2="54" stroke="currentColor" stroke-width="1.8"/>'
          + '</svg>';
      }

      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<path d="M8 50V17l14-6 12 5 12-5v33l-12 5-12-5-14 6z" fill="none" stroke="currentColor" stroke-width="4" stroke-linejoin="round"/>'
        + '<line x1="22" y1="11" x2="22" y2="56" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="34" y1="16" x2="34" y2="49" stroke="currentColor" stroke-width="3"/>'
        + '<path d="M46 17c-5.4 0-9.8 4.4-9.8 9.8 0 6.8 9.8 17.1 9.8 17.1S55.8 33.6 55.8 26.8c0-5.4-4.4-9.8-9.8-9.8z" fill="none" stroke="currentColor" stroke-width="4" stroke-linejoin="round"/>'
        + '<circle cx="46" cy="26.8" r="2.8" fill="currentColor"/>'
        + '</svg>';
    }

    function getEfbOwnshipCenterIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<path d="M8 30L56 8 43 56 31 43 22 49 19 39 8 30z" fill="currentColor"/>'
        + '<path d="M14 31L48 14 30 39z" fill="#f1f1ef" opacity="0.92"/>'
        + '</svg>';
    }

    function getEfbSaLayersIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<rect x="10" y="12" width="44" height="10" rx="2" fill="none" stroke="currentColor" stroke-width="3"/>'
        + '<rect x="10" y="27" width="44" height="10" rx="2" fill="none" stroke="currentColor" stroke-width="3"/>'
        + '<rect x="10" y="42" width="44" height="10" rx="2" fill="none" stroke="currentColor" stroke-width="3"/>'
        + '</svg>';
    }

    function getEfbUserWaypointsIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<polygon points="32,10 52,48 12,48" fill="none" stroke="currentColor" stroke-width="4" stroke-linejoin="round"/>'
        + '<circle cx="32" cy="37" r="3" fill="currentColor"/>'
        + '</svg>';
    }

    function getEfbSavedHistoryTracksIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<path d="M16 13c-5 0-9 4-9 9 0 7 9 16 9 16s9-9 9-16c0-5-4-9-9-9z" fill="none" stroke="currentColor" stroke-width="3" stroke-linejoin="round"/>'
        + '<circle cx="16" cy="22" r="2.4" fill="currentColor"/>'
        + '<path d="M24 36c6 0 9 4 14 4s9-4 13-4" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-dasharray="3 6"/>'
        + '<path d="M48 34c-5 0-9 4-9 9 0 7 9 16 9 16s9-9 9-16c0-5-4-9-9-9z" fill="none" stroke="currentColor" stroke-width="3" stroke-linejoin="round"/>'
        + '<circle cx="48" cy="43" r="2.4" fill="currentColor"/>'
        + '<circle cx="44" cy="18" r="16" fill="none" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="44" y1="18" x2="44" y2="10" stroke="currentColor" stroke-width="3" stroke-linecap="round"/>'
        + '<line x1="44" y1="18" x2="52" y2="18" stroke="currentColor" stroke-width="3" stroke-linecap="round"/>'
        + '</svg>';
    }

    function getEfbSaLayerDlinkIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<text x="32" y="48" text-anchor="middle" font-size="40" font-weight="700" fill="currentColor" font-family="Consolas, monospace">DL</text>'
        + '</svg>';
    }

    function getEfbSaLayerAirportIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<path d="M14 24h36v6h-4l2 9h-8l-2-7h-12l-2 7h-8l2-9h-4z" fill="currentColor"/>'
        + '<rect x="27" y="39" width="10" height="15" fill="currentColor"/>'
        + '<rect x="18" y="54" width="28" height="4" fill="currentColor"/>'
        + '</svg>';
    }

    function getEfbAutoNearestAirportIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<path d="M14 24h24v5h-3l2 9h-7l-2-7h-8l-2 7h-7l2-9h-3z" fill="currentColor"/>'
        + '<rect x="22" y="38" width="8" height="14" fill="currentColor"/>'
        + '<rect x="15" y="52" width="22" height="4" fill="currentColor"/>'
        + '<circle cx="48" cy="20" r="10" fill="none" stroke="currentColor" stroke-width="2.8"/>'
        + '<line x1="38" y1="20" x2="58" y2="20" stroke="currentColor" stroke-width="2.4" stroke-linecap="round"/>'
        + '<line x1="48" y1="10" x2="48" y2="30" stroke="currentColor" stroke-width="2.4" stroke-linecap="round"/>'
        + '<circle cx="48" cy="20" r="2.6" fill="currentColor"/>'
        + '</svg>';
    }

    function getEfbSaLayerNavlogIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<circle cx="20" cy="32" r="7" fill="none" stroke="currentColor" stroke-width="3"/>'
        + '<circle cx="44" cy="20" r="7" fill="none" stroke="currentColor" stroke-width="3"/>'
        + '<circle cx="44" cy="44" r="7" fill="none" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="26" y1="30" x2="38" y2="22" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="26" y1="34" x2="38" y2="42" stroke="currentColor" stroke-width="3"/>'
        + '</svg>';
    }

    function getEfbSaLayerMissionDrawingsIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<text x="32" y="47" text-anchor="middle" font-size="30" font-weight="700" fill="currentColor" font-family="Consolas, monospace">MIZ</text>'
        + '</svg>';
    }

    function getEfbSaLayerHistoryTrackIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<path d="M16 13c-5 0-9 4-9 9 0 7 9 16 9 16s9-9 9-16c0-5-4-9-9-9z" fill="none" stroke="currentColor" stroke-width="3" stroke-linejoin="round"/>'
        + '<circle cx="16" cy="22" r="2.4" fill="currentColor"/>'
        + '<path d="M24 36c6 0 9 4 14 4s9-4 13-4" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-dasharray="3 6"/>'
        + '<path d="M48 34c-5 0-9 4-9 9 0 7 9 16 9 16s9-9 9-16c0-5-4-9-9-9z" fill="none" stroke="currentColor" stroke-width="3" stroke-linejoin="round"/>'
        + '<circle cx="48" cy="43" r="2.4" fill="currentColor"/>'
        + '</svg>';
    }

    function getEfbSaLayerOverlayIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<rect x="12" y="12" width="40" height="40" rx="4" fill="none" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="20" y1="44" x2="44" y2="20" stroke="currentColor" stroke-width="3"/>'
        + '<circle cx="22" cy="22" r="4" fill="none" stroke="currentColor" stroke-width="2.5"/>'
        + '<circle cx="42" cy="42" r="4" fill="none" stroke="currentColor" stroke-width="2.5"/>'
        + '</svg>';
    }

    function getEfbSaLayerUserWaypointIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<polygon points="32,10 52,48 12,48" fill="none" stroke="currentColor" stroke-width="4" stroke-linejoin="round"/>'
        + '<circle cx="32" cy="37" r="2.8" fill="currentColor"/>'
        + '</svg>';
    }

    function getEfbSaLayerDoghouseIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<polygon points="18,27 32,13 46,27" fill="none" stroke="currentColor" stroke-width="3.4" stroke-linejoin="round"/>'
        + '<rect x="15" y="27" width="34" height="24" rx="2" fill="none" stroke="currentColor" stroke-width="3.4"/>'
        + '<line x1="22" y1="35" x2="42" y2="35" stroke="currentColor" stroke-width="2.4"/>'
        + '<line x1="22" y1="42" x2="36" y2="42" stroke="currentColor" stroke-width="2.4"/>'
        + '</svg>';
    }

    function getEfbSaLayerJtacIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<polygon points="32,11 53,32 32,53 11,32" fill="none" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="32" y1="17" x2="32" y2="47" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="17" y1="32" x2="47" y2="32" stroke="currentColor" stroke-width="3"/>'
        + '</svg>';
    }

    function getEfbSaLayerSamThreatIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<path d="M16 36c0-9.4 7.6-17 17-17s17 7.6 17 17" fill="none" stroke="currentColor" stroke-width="3.4" stroke-linecap="round"/>'
        + '<path d="M22 36c0-6 4.9-10.8 11-10.8S44 30 44 36" fill="none" stroke="currentColor" stroke-width="2.6" stroke-linecap="round"/>'
        + '<line x1="33" y1="20" x2="33" y2="44" stroke="currentColor" stroke-width="2.8"/>'
        + '<line x1="16" y1="44" x2="50" y2="44" stroke="currentColor" stroke-width="2.8"/>'
        + '<circle cx="33" cy="36" r="2.6" fill="currentColor"/>'
        + '</svg>';
    }

    function normalizeDoghouseMapLegRows(rows) {
      return (Array.isArray(rows) ? rows : []).filter(function (wp) {
        return isFinite(Number(wp && wp.xNum)) && isFinite(Number(wp && wp.yNum));
      });
    }

    function getEfbSaNavigraphLayerIconSvg(layer) {
      const value = normalizeEfbSaNavigraphLayer(layer);
      const top = value === 'vfr' ? 'VFR' : 'IFR';
      const bottom = value === 'vfr' ? '' : (value === 'ifr-lo' ? 'LOW' : 'HI');
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<rect x="6" y="6" width="52" height="52" rx="7" fill="none" stroke="currentColor" stroke-width="3.4"/>'
        + '<text x="32" y="31" text-anchor="middle" font-size="20" font-weight="700" fill="currentColor" font-family="Consolas, monospace">' + top + '</text>'
        + (bottom ? ('<text x="32" y="49" text-anchor="middle" font-size="14.6" font-weight="700" fill="currentColor" font-family="Consolas, monospace">' + bottom + '</text>') : '')
        + '</svg>';
    }

    function refreshEfbSaMapTilesIfActive() {
      if (selectedTab !== 'EFB') return;
      if (normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') return;
      if (typeof disposeOpenFreeMapInstances === 'function') {
        disposeOpenFreeMapInstances();
      }
      efbUiDirty = true;
      if (latestData) {
        render(latestData);
      }
    }

    function getEfbSaFollowIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<circle cx="32" cy="32" r="14" fill="none" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="32" y1="8" x2="32" y2="20" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="32" y1="44" x2="32" y2="56" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="8" y1="32" x2="20" y2="32" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="44" y1="32" x2="56" y2="32" stroke="currentColor" stroke-width="3"/>'
        + '<circle cx="32" cy="32" r="3.8" fill="currentColor"/>'
        + '</svg>';
    }

    function getEfbSaTrackUpIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<polygon points="32,8 46,46 32,38 18,46" fill="none" stroke="currentColor" stroke-width="3" stroke-linejoin="round"/>'
        + '<line x1="32" y1="18" x2="32" y2="40" stroke="currentColor" stroke-width="3"/>'
        + '</svg>';
    }

    function getEfbSaAnchorCenterIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<rect x="10" y="10" width="44" height="44" rx="5" fill="none" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="32" y1="16" x2="32" y2="48" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="16" y1="32" x2="48" y2="32" stroke="currentColor" stroke-width="3"/>'
        + '<circle cx="32" cy="32" r="3.2" fill="currentColor"/>'
        + '</svg>';
    }

    function getEfbSaAnchorLowerThirdIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<rect x="10" y="10" width="44" height="44" rx="5" fill="none" stroke="currentColor" stroke-width="3"/>'
        + '<line x1="16" y1="32" x2="48" y2="32" stroke="currentColor" stroke-width="2.3" opacity="0.55"/>'
        + '<line x1="16" y1="42" x2="48" y2="42" stroke="currentColor" stroke-width="3"/>'
        + '<circle cx="32" cy="42" r="3.2" fill="currentColor"/>'
        + '</svg>';
    }

    function getEfbPinnedDrawerIconSvg() {
      return '<svg class="efbRailIconSvg" viewBox="0 0 64 64" aria-hidden="true">'
        + '<path d="M32 11c8 0 14 6 14 14h-6l9 9 9-9h-6c0-11-9-20-20-20-8 0-15 5-18 12l5 2c2.2-5 7.2-8 13-8z" fill="currentColor"/>'
        + '<path d="M13 39h6c0 11 9 20 20 20 8 0 15-5 18-12l-5-2c-2.2 5-7.2 8-13 8-8 0-14-6-14-14h6l-9-9-9 9z" fill="currentColor"/>'
        + '</svg>';
    }

    function getEfbPinToggleIconSvg() {
      return '<span class="efbPinGlyph" aria-hidden="true">&#128204;&#65038;</span>';
    }

    function applyDtcListCollapsedState(collapsed) {
      dtcListCollapsed = !!collapsed;
      const listEl = document.getElementById('dtcFileList');
      const headerEl = document.getElementById('dtcSelectorHeader');
      if (listEl) listEl.style.display = dtcListCollapsed ? 'none' : 'flex';
      if (headerEl) headerEl.textContent = dtcListCollapsed ? 'FLT PLN Files ► (click to expand)' : 'FLT PLN Files ▼';
    }

    function readInitialDtcListCollapsed() {
      return readOneZeroPreference(dtcListCollapsedStorageKey, false);
    }

    function persistDtcListCollapsedState() {
      persistOneZeroPreference(dtcListCollapsedStorageKey, dtcListCollapsed);
    }

    function readPinnedChartsPreference() {
      try {
        const raw = readStoredPreferenceValue(efbPinnedChartsStorageKey);
        if (!raw) return {};
        const parsed = JSON.parse(raw);
        if (!parsed || typeof parsed !== 'object') return {};
        const clean = {};
        Object.keys(parsed).forEach(function (k) {
          const airport = String(k || '').toUpperCase().trim();
          if (!airport) return;
          const ids = Array.isArray(parsed[k]) ? parsed[k] : [];
          const seen = {};
          clean[airport] = ids
            .map(function (id) { return String(id || '').trim(); })
            .filter(function (id) {
              if (!id || seen[id]) return false;
              seen[id] = true;
              return true;
            });
        });
        return clean;
      } catch (_) {
        return {};
      }
    }

    function persistPinnedChartsPreference() {
      writeStoredPreferenceValue(efbPinnedChartsStorageKey, JSON.stringify(efbPinnedChartsByAirport || {}));
    }

    function readEfbSaUserWaypointsPreference() {
      try {
        const raw = readStoredPreferenceValue(efbSaUserWaypointsStorageKey);
        if (!raw) return [];
        const parsed = JSON.parse(raw);
        if (!Array.isArray(parsed)) return [];
        const clean = [];
        const seen = {};

        function tryReadFiniteNumber(value) {
          if (value === null || value === undefined) return null;
          const text = String(value).trim();
          if (!text) return null;
          const n = Number(text);
          return isFinite(n) ? n : null;
        }

        parsed.forEach(function (row) {
          if (!row || typeof row !== 'object') return;
          const id = String(row.id || '').trim();
          if (!id || seen[id]) return;
          const name = String(row.name || '').trim();
          const xNum = tryReadFiniteNumber(row.xNum);
          const yNum = tryReadFiniteNumber(row.yNum);
          const lat = tryReadFiniteNumber(row.lat);
          const lon = tryReadFiniteNumber(row.lon);
          const hasXY = xNum !== null && yNum !== null;
          const hasLatLon = lat !== null && lon !== null && Math.abs(lat) <= 90 && Math.abs(lon) <= 180;
          if (!hasXY && !hasLatLon) return;
          seen[id] = true;
          clean.push({
            id: id,
            name: name || '',
            xNum: hasXY ? xNum : null,
            yNum: hasXY ? yNum : null,
            lat: hasLatLon ? lat : null,
            lon: hasLatLon ? lon : null,
            createdUtcMs: Number(row.createdUtcMs) || Date.now(),
            updatedUtcMs: Number(row.updatedUtcMs) || 0,
          });
        });
        return clean;
      } catch (_) {
        return [];
      }
    }

