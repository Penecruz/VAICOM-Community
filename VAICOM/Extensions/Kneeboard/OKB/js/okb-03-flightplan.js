    function togglePostFlightOpen(selected) {
      const state = getFlightPlanPlanState(selected);
      state.postFlightOpen = !state.postFlightOpen;
    }

    function clearNavlogRowAction(selected) {
      const state = getFlightPlanPlanState(selected);
      state.rowActionStep = '';
      state.rowActionMode = '';
    }

    function clearNavlogRowReveal(selected) {
      const state = getFlightPlanPlanState(selected);
      state.rowRevealStep = '';
      state.rowRevealMode = '';
    }

    function setNavlogRowReveal(selected, step, mode) {
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      const revealMode = String(mode || '').toLowerCase();
      if (!key || (revealMode !== 'dir' && revealMode !== 'del')) {
        clearNavlogRowReveal(selected);
        return;
      }
      if (state.rowRevealStep === key && state.rowRevealMode === revealMode) {
        clearNavlogRowReveal(selected);
        return;
      }
      state.rowRevealStep = key;
      state.rowRevealMode = revealMode;
    }

    function setNavlogRowAction(selected, step, mode) {
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      const actionMode = String(mode || '').toLowerCase();
      if (!key || (actionMode !== 'dir' && actionMode !== 'del')) {
        clearNavlogRowAction(selected);
        return;
      }
      if (state.rowActionStep === key && state.rowActionMode === actionMode) {
        clearNavlogRowAction(selected);
        return;
      }
      state.rowActionStep = key;
      state.rowActionMode = actionMode;
    }

    function isNavlogRowActionArmed(selected, step, mode) {
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      const actionMode = String(mode || '').toLowerCase();
      if (!key || (actionMode !== 'dir' && actionMode !== 'del')) return false;
      return stepToKey(state.rowActionStep) === key
        && String(state.rowActionMode || '').toLowerCase() === actionMode;
    }

    function deleteNavlogStep(selected, step) {
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
      if (state.rowActionStep === key) {
        state.rowActionStep = '';
        state.rowActionMode = '';
      }
      if (state.rowRevealStep === key) {
        state.rowRevealStep = '';
        state.rowRevealMode = '';
      }
      ensureDirectToStateValid(getPlanWaypointsForRecommendations(selected), state);
      clearSpeedRecommendations(selected);
    }

    function setNavlogDirectToSource(selected, step) {
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      if (!key) return;
      state.directToSourceStep = key;
      state.directToTargetStep = '';
      clearSpeedRecommendations(selected);
    }

    function setNavlogDirectToTarget(selected, step) {
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

    function clearNavlogDirectTo(selected) {
      const state = getFlightPlanPlanState(selected);
      clearInvalidDirectToState(state);
      clearSpeedRecommendations(selected);
    }

    function toggleWaypointAta(selected, step, plannedEtaText) {
      const state = getFlightPlanPlanState(selected);
      if (!state.ataByStep || typeof state.ataByStep !== 'object') state.ataByStep = {};
      if (!Array.isArray(state.timingLog)) state.timingLog = [];

      const key = stepToKey(step);
      if (!key) return;

      if (state.ataByStep[key]) {
        delete state.ataByStep[key];
        if (stepToKey(state.autoAtaLastCapturedStep) === key) {
          state.autoAtaLastCapturedStep = '';
        }
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
        .map(function (k) { return String(k); })
        .sort(function (a, b) { return Number(a) - Number(b); });

      const parts = keys.map(function (k) {
        const entry = state.ataByStep[k] || {};
        const actualSec = Number(entry.actualSeconds);
        const plannedSec = Number(entry.plannedSeconds);
        if (!isFinite(actualSec) || !isFinite(plannedSec)) return '';
        return 'STP' + String(k) + ' ' + formatSignedDeltaSeconds(actualSec - plannedSec);
      }).filter(function (x) { return !!x; });

      if (parts.length) {
        const row = new Date().toISOString() + ' | ATA ' + parts.join(' ');
        state.timingLog.push(row);
        if (state.timingLog.length > 12) {
          state.timingLog = state.timingLog.slice(state.timingLog.length - 12);
        }
      }

      buildSpeedRecommendationsForAta(selected, key);
      const pulseState = getAutoAtaRecDisplayState(selected);
      if (pulseState && pulseState.spdText) {
        triggerAutoAtaRecPulse(pulseState.spdText, pulseState.noteText, pulseState.unable);
      }
    }

    function hasTakeoffTimeBySelection(selected) {
      return isFinite(getTakeoffTimeBySelection(selected));
    }

    function getFlightPlanTimingDisplay(selected) {
      const timing = getResolvedTimingMarks(selected);
      const takeoffSec = Number(timing.takeoff);
      if (!isFinite(takeoffSec)) {
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

    function getFlightPlanPlanState(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return { speedAdjustments: {}, speedDisplayModes: {}, altAdjustments: {}, typeOverrides: {}, lockedStep: '', totSeconds: NaN, lockedStart: null, timeMarks: {}, timingLog: [], postFlightOpen: false, ataByStep: {}, speedRecommendations: {}, totPerformanceByStep: {}, lastOwnshipPos: null, autoAtaLastOwnshipPos: null, autoAtaLastCapturedStep: '', deletedSteps: {}, directToSourceStep: '', directToTargetStep: '', rowActionStep: '', rowActionMode: '', rowRevealStep: '', rowRevealMode: '', coordDisplayMode: 'xy', navlogAltDisplayMode: defaultNavlogAltDisplayMode, navlogSpdDisplayMode: defaultNavlogSpdDisplayMode, navlogDistDisplayMode: defaultNavlogDistDisplayMode, expandedBottomPanel: '', closingBottomPanel: '', closingBottomPanelUntilUtc: 0 };
      const existing = fltPlanPlanStateBySelection[key];
      if (existing && typeof existing === 'object') {
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
        if (!existing.autoAtaLastOwnshipPos || typeof existing.autoAtaLastOwnshipPos !== 'object') existing.autoAtaLastOwnshipPos = null;
        if (typeof existing.autoAtaLastCapturedStep !== 'string') existing.autoAtaLastCapturedStep = '';
        if (!existing.deletedSteps || typeof existing.deletedSteps !== 'object') existing.deletedSteps = {};
        if (typeof existing.directToSourceStep !== 'string') existing.directToSourceStep = '';
        if (typeof existing.directToTargetStep !== 'string') existing.directToTargetStep = '';
        if (typeof existing.rowActionStep !== 'string') existing.rowActionStep = '';
        if (typeof existing.rowActionMode !== 'string') existing.rowActionMode = '';
        if (typeof existing.rowRevealStep !== 'string') existing.rowRevealStep = '';
        if (typeof existing.rowRevealMode !== 'string') existing.rowRevealMode = '';
        if (typeof existing.coordDisplayMode !== 'string') existing.coordDisplayMode = 'xy';
        existing.navlogAltDisplayMode = normalizeNavlogAltDisplayMode(existing.navlogAltDisplayMode || defaultNavlogAltDisplayMode);
        existing.navlogSpdDisplayMode = normalizeNavlogSpdDisplayMode(existing.navlogSpdDisplayMode || defaultNavlogSpdDisplayMode);
        existing.navlogDistDisplayMode = normalizeNavlogDistDisplayMode(existing.navlogDistDisplayMode || defaultNavlogDistDisplayMode);
        if (typeof existing.expandedBottomPanel !== 'string') existing.expandedBottomPanel = '';
        if (typeof existing.closingBottomPanel !== 'string') existing.closingBottomPanel = '';
        if (!isFinite(Number(existing.closingBottomPanelUntilUtc))) existing.closingBottomPanelUntilUtc = 0;
        return existing;
      }
      const created = { speedAdjustments: {}, speedDisplayModes: {}, altAdjustments: {}, typeOverrides: {}, lockedStep: '', totSeconds: NaN, lockedStart: null, timeMarks: {}, timingLog: [], postFlightOpen: false, ataByStep: {}, speedRecommendations: {}, totPerformanceByStep: {}, lastOwnshipPos: null, autoAtaLastOwnshipPos: null, autoAtaLastCapturedStep: '', deletedSteps: {}, directToSourceStep: '', directToTargetStep: '', rowActionStep: '', rowActionMode: '', rowRevealStep: '', rowRevealMode: '', coordDisplayMode: 'xy', navlogAltDisplayMode: defaultNavlogAltDisplayMode, navlogSpdDisplayMode: defaultNavlogSpdDisplayMode, navlogDistDisplayMode: defaultNavlogDistDisplayMode, expandedBottomPanel: '', closingBottomPanel: '', closingBottomPanelUntilUtc: 0 };
      fltPlanPlanStateBySelection[key] = created;
      return created;
    }

    function lockStartPositionForSelection(selected, data) {
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

    function stepToKey(step) {
      return String(step || '').trim();
    }

    function changeWaypointSpeedAdjustment(selected, step, delta) {
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      if (!key) return;
      const current = Number(state.speedAdjustments[key]);
      const next = (isFinite(current) ? current : 0) + Number(delta || 0);
      state.speedAdjustments[key] = clamp(next, -600, 600);
    }

    function canUseMachDisplay(altFeet) {
      return isFinite(Number(altFeet)) && Number(altFeet) > 28000;
    }

    function getWaypointSpeedDisplayMode(state, step, altFeet) {
      const key = stepToKey(step);
      const safeState = state || {};
      const map = (safeState.speedDisplayModes && typeof safeState.speedDisplayModes === 'object') ? safeState.speedDisplayModes : {};
      let mode = String(map[key] || 'KCAS').toUpperCase();
      if (mode !== 'MACH') mode = 'KCAS';
      if (mode === 'MACH' && !canUseMachDisplay(altFeet)) mode = 'KCAS';
      return mode;
    }

    function toggleWaypointSpeedDisplayMode(selected, step, altFeet) {
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      if (!key || !state.speedDisplayModes) return;

      if (!canUseMachDisplay(altFeet)) {
        state.speedDisplayModes[key] = 'KCAS';
        return;
      }

      const current = getWaypointSpeedDisplayMode(state, key, altFeet);
      state.speedDisplayModes[key] = (current === 'MACH') ? 'KCAS' : 'MACH';
    }

    function changeWaypointSpeedAdjustmentByDirection(selected, step, direction, altFeet) {
      const dir = Number(direction);
      if (!isFinite(dir) || dir === 0) return;

      const state = getFlightPlanPlanState(selected);
      const mode = getWaypointSpeedDisplayMode(state, step, altFeet);

      let deltaKcas = 10 * (dir >= 0 ? 1 : -1);
      if (mode === 'MACH' && canUseMachDisplay(altFeet)) {
        const conversion = 661.47 / (1.0 + (Math.max(0, Number(altFeet) || 0) / 100000.0));
        deltaKcas = 0.01 * conversion * (dir >= 0 ? 1 : -1);
      }

      changeWaypointSpeedAdjustment(selected, step, deltaKcas);
    }

    function formatWaypointSpeedDisplay(speedKcasText, mode, altFeet) {
      const kcas = Number(speedKcasText);
      if (!isFinite(kcas) || kcas <= 0) return '-';

      if (mode === 'MACH' && canUseMachDisplay(altFeet)) {
        const cas = Math.max(0, kcas);
        const tas = cas * (1.0 + (Math.max(0, Number(altFeet) || 0) / 100000.0));
        const mach = tas / 661.47;
        if (!isFinite(mach) || mach <= 0) return '-';
        return 'M ' + mach.toFixed(2);
      }

      return String(Math.round(kcas));
    }

    function getWaypointSpeedAdjustment(state, step) {
      const key = stepToKey(step);
      if (!key || !state || !state.speedAdjustments) return 0;
      const v = Number(state.speedAdjustments[key]);
      return isFinite(v) ? v : 0;
    }

    function setLockedTotStep(selected, step, locked, etaSeconds) {
      const state = getFlightPlanPlanState(selected);
      const key = stepToKey(step);
      state.lockedStep = locked ? key : '';
      if (locked) {
        const eta = Number(etaSeconds);
        if (isFinite(eta)) {
          state.totSeconds = eta;
        }
      }
    }

    function setTotByMinutesDelta(selected, minutesDelta) {
      const state = getFlightPlanPlanState(selected);
      let base = Number(state.totSeconds);
      if (!isFinite(base)) {
        base = getCurrentFlightPlanClockSeconds();
      }
      const delta = Math.round(Number(minutesDelta || 0) * 60);
      state.totSeconds = base + delta;
    }

    function setTakeoffBySecondsDelta(selected, secondsDelta) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;

      const delta = Math.round(Number(secondsDelta || 0));
      if (!isFinite(delta) || delta === 0) return;

      const state = getFlightPlanPlanState(selected);
      const marks = getResolvedTimingMarks(selected);
      if (!isFinite(Number(marks.takeoff))) {
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

      if (isFinite(Number(marks.takeoff))) {
        fltPlanEtaStartBySelection[key] = Number(marks.takeoff);
      }

      appendTimingLog(selected, 'TAKEOFF', marks);
    }

    function setTakeoffByMinutesDelta(selected, minutesDelta) {
      const delta = Math.round(Number(minutesDelta || 0) * 60);
      setTakeoffBySecondsDelta(selected, delta);
    }

    function setTimingAnchorBySecondsDelta(selected, anchorType, secondsDelta) {
      const delta = Math.round(Number(secondsDelta || 0));
      if (!isFinite(delta) || delta === 0) return;

      const anchor = String(anchorType || '').toUpperCase();
      if (!anchor) return;
      if (anchor === 'TOT') {
        setTotBySecondsDelta(selected, delta);
        return;
      }
      if (anchor === 'TAKEOFF') {
        setTakeoffBySecondsDelta(selected, delta);
        return;
      }

      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;

      const state = getFlightPlanPlanState(selected);
      const marks = getResolvedTimingMarks(selected);
      if (!isFinite(Number(marks.takeoff))) {
        const now = getCurrentFlightPlanClockSeconds();
        const defaults = buildTimingFromTakeoff(now);
        marks.step = defaults.step;
        marks.start = defaults.start;
        marks.taxi = defaults.taxi;
        marks.takeoff = defaults.takeoff;
      }

      if (anchor === 'STEP') {
        marks.step = Number(marks.step) + delta;
        marks.start = Number(marks.step) + stepToStartSeconds;
        marks.taxi = Number(marks.start) + startToTaxiSeconds;
        marks.takeoff = Number(marks.taxi) + taxiToTakeoffSeconds;
      } else if (anchor === 'START') {
        marks.start = Number(marks.start) + delta;
        marks.taxi = Number(marks.start) + startToTaxiSeconds;
        marks.takeoff = Number(marks.taxi) + taxiToTakeoffSeconds;
      } else if (anchor === 'TAXI') {
        marks.taxi = Number(marks.taxi) + delta;
        marks.takeoff = Number(marks.taxi) + taxiToTakeoffSeconds;
      } else {
        return;
      }

      state.timeMarks = {
        step: marks.step,
        start: marks.start,
        taxi: marks.taxi,
        takeoff: marks.takeoff,
      };

      if (isFinite(Number(marks.takeoff))) {
        fltPlanEtaStartBySelection[key] = Number(marks.takeoff);
      }
      appendTimingLog(selected, anchor + ' ADJ', marks);
    }

    function resolveWaypointNorthEast(wp) {
      if (!wp || typeof wp !== 'object') return { north: NaN, east: NaN };
      const north = Number(isFinite(Number(wp.xNum)) ? wp.xNum : wp.x);
      const east = Number(isFinite(Number(wp.yNum)) ? wp.yNum : wp.y);
      return { north: north, east: east };
    }

    function distancePointToSegmentMeters(px, py, ax, ay, bx, by) {
      const vx = bx - ax;
      const vy = by - ay;
      const wx = px - ax;
      const wy = py - ay;
      const vv = (vx * vx) + (vy * vy);
      if (!isFinite(vv) || vv <= 0) {
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

    function getOwnshipNorthEast() {
      if (fakeMissionEnabled && fakeMissionState) {
        return { north: Number(fakeMissionState.playerX), east: Number(fakeMissionState.playerY) };
      }

      const fastOwnship = getFastOwnshipMapPoint(2000);
      if (fastOwnship && isFinite(Number(fastOwnship.xNum)) && isFinite(Number(fastOwnship.yNum))) {
        return { north: Number(fastOwnship.xNum), east: Number(fastOwnship.yNum) };
      }

      const server = (latestData && latestData.Server) || {};
      return { north: Number(server.PlayerPosX), east: Number(server.PlayerPosY) };
    }
      // WIP auto takeoff logging incomplete, but this is the best we can do for now until we have a better ownship data source.
    function updateAutoTakeoffCaptureFromFastOwnship(data) {
      if (!autoAtaRecSpdEnabled) return;
      if (fakeMissionEnabled) return;
      if (!efbSaUseFastOwnshipEnabled) return;

      const selected = getActiveFlightPlanSelection(data || latestData);
      if (!selected) return;

      const state = getFlightPlanPlanState(selected);
      const key = getFlightPlanEtaStartKey(selected);
      if (!state || !key) return;
      if (!state.timeMarks || typeof state.timeMarks !== 'object') state.timeMarks = {};
      if (!Array.isArray(state.timingLog)) state.timingLog = [];

      const wow = Number(efbSaOwnshipFast && efbSaOwnshipFast.wow);
      const hasWow = !!(efbSaOwnshipFast && efbSaOwnshipFast.hasWow);
      const gs = Number(efbSaOwnshipFast && efbSaOwnshipFast.groundSpeedKnots);
      const hasGs = !!(efbSaOwnshipFast && efbSaOwnshipFast.hasGroundSpeed) && isFinite(gs);
      const mark = autoTakeoffBySelection[key] || { wasWowOn: false, captured: false };

      if (!hasWow) {
        autoTakeoffBySelection[key] = mark;
        return;
      }

      if (wow === 1) {
        mark.wasWowOn = true;
        autoTakeoffBySelection[key] = mark;
        return;
      }

      if (wow !== 0) {
        autoTakeoffBySelection[key] = mark;
        return;
      }

      if (!mark.wasWowOn || mark.captured) {
        autoTakeoffBySelection[key] = mark;
        return;
      }

      // Small speed gate to avoid WOW bounce/noise while still on deck.
      if (hasGs && gs < 35) {
        autoTakeoffBySelection[key] = mark;
        return;
      }

      // Always lock actual takeoff on first WOW ON->OFF transition, even if pre-populated by STEP/START/TAXI.
      // Subsequent user edits are respected because we only auto-capture once per flight selection.
      setTakeoffTimeByAnchorFromNow(selected, 'TAKEOFF');
      mark.captured = true;
      autoTakeoffBySelection[key] = mark;
    }

    function updateTotOverflyCapture(selected, rows) {
      const list = Array.isArray(rows) ? rows : [];
      if (!list.length) return;

      const state = getFlightPlanPlanState(selected);
      if (!state.totPerformanceByStep || typeof state.totPerformanceByStep !== 'object') state.totPerformanceByStep = {};
      if (!Array.isArray(state.timingLog)) state.timingLog = [];

      const lockedStep = stepToKey(state.lockedStep);
      const totSec = Number(state.totSeconds);
      if (!lockedStep || !isFinite(totSec)) return;

      if (state.totPerformanceByStep[lockedStep]) return;

      const target = list.find(function (wp) {
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
      if (!crossedWithinThreshold && prevOwn) {
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
      if (state.timingLog.length > 16) {
        state.timingLog = state.timingLog.slice(state.timingLog.length - 16);
      }
    }

    function buildPostFlightSummaryRows(selected, rows, timing) {
      const state = getFlightPlanPlanState(selected);
      const list = Array.isArray(rows) ? rows : [];
      const out = [];

      out.push('TIMING STEP ' + safe(timing && timing.step)
        + ' START ' + safe(timing && timing.start)
        + ' TAXI ' + safe(timing && timing.taxi)
        + ' TAKEOFF ' + safe(timing && timing.takeoff));

      let prevEtaSec = NaN;
      list.forEach(function (wp) {
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
      if (lockedStep && isFinite(Number(state && state.totSeconds))) {
        if (perf && isFinite(Number(perf.actualSeconds))) {
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

    function setTotBySecondsDelta(selected, secondsDelta) {
      const state = getFlightPlanPlanState(selected);
      let base = Number(state.totSeconds);
      if (!isFinite(base)) {
        base = getCurrentFlightPlanClockSeconds();
      }
      const delta = Math.round(Number(secondsDelta || 0));
      state.totSeconds = base + delta;
    }

    function abbreviateRouteType(type) {
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

    function normalizeEditableWaypointType(type) {
      const norm = abbreviateRouteType(type);
      const allowed = ['WP', 'TP', 'IP', 'TGT', 'AAR', 'CAP', 'HLD', 'TKO', 'LDG', 'DVRT'];
      return allowed.indexOf(norm) >= 0 ? norm : 'WP';
    }

    function changeWaypointType(selected, step, delta, currentType) {
      const state = getFlightPlanPlanState(selected);
      if (!state.typeOverrides || typeof state.typeOverrides !== 'object') state.typeOverrides = {};
      const key = stepToKey(step);
      if (!key) return;
      const allowed = ['WP', 'TP', 'IP', 'TGT', 'AAR', 'CAP', 'HLD', 'TKO', 'LDG', 'DVRT'];
      const baseline = state.typeOverrides[key] || currentType || 'WP';
      const current = normalizeEditableWaypointType(baseline);
      const idx = allowed.indexOf(current);
      const d = Number(delta || 0);
      const nextIndex = ((idx + (d >= 0 ? 1 : -1)) % allowed.length + allowed.length) % allowed.length;
      state.typeOverrides[key] = allowed[nextIndex];
    }

    function applyTypeOverrides(rows, selected) {
      const list = Array.isArray(rows) ? rows : [];
      const state = getFlightPlanPlanState(selected);
      const overrides = (state && state.typeOverrides && typeof state.typeOverrides === 'object') ? state.typeOverrides : {};
      list.forEach(function (wp) {
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

    function parseEtaToSeconds(etaText) {
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

    function formatSecondsToClock(seconds) {
      let total = Number(seconds);
      if (!isFinite(total)) return '-';
      total = Math.round(total);
      total = ((total % 86400) + 86400) % 86400;
      const h = Math.floor(total / 3600);
      const m = Math.floor((total % 3600) / 60);
      const s = total % 60;
      return String(h).padStart(2, '0') + ':' + String(m).padStart(2, '0') + ':' + String(s).padStart(2, '0');
    }

    function getMissionClockSeconds(data) {
      const server = (data && data.Server) || {};
      const mission = Number(server.MissionTimeSeconds);
      if (isFinite(mission) && mission >= 0) return mission;
      return NaN;
    }

    function getSystemLocalClockSeconds() {
      const now = new Date();
      return (now.getHours() * 3600) + (now.getMinutes() * 60) + now.getSeconds();
    }

    function getTheaterUtcOffsetHours(theater) {
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

    function missionToUtcSeconds(missionSeconds, theater) {
      const mission = Number(missionSeconds);
      const offset = Number(getTheaterUtcOffsetHours(theater));
      if (!isFinite(mission) || !isFinite(offset)) return NaN;
      return mission - (offset * 3600);
    }

    function updateMissionClockLabel(data) {
      const label = document.getElementById('missionClockLabel');
      if (!label) return;

      const model = data || latestData || {};
      const server = (model && model.Server) || {};
      const mission = getCurrentFlightPlanClockSeconds();
      if (!isFinite(mission)) {
        label.textContent = 'Mission Time - (- UTC)';
        return;
      }

      const utcSeconds = missionToUtcSeconds(mission, server.Theater);
      const utcText = isFinite(utcSeconds) ? formatSecondsToClock(utcSeconds) : '-';
      label.textContent = 'Mission Time ' + formatSecondsToClock(mission) + ' (' + utcText + ' UTC)';
    }

    function getCurrentFlightPlanClockSeconds() {
      if (fakeMissionEnabled && fakeMissionState && isFinite(Number(fakeMissionState.simMissionSeconds))) {
        return Number(fakeMissionState.simMissionSeconds);
      }

      if (isFinite(Number(runtimeState.missionClock.anchorSeconds)) && runtimeState.missionClock.anchorSystemMs > 0) {
        const elapsed = (Date.now() - runtimeState.missionClock.anchorSystemMs) / 1000.0;
        if (isFinite(elapsed)) {
          return Number(runtimeState.missionClock.anchorSeconds) + Math.max(0, elapsed);
        }
      }

      const mission = getMissionClockSeconds(latestData);
      if (isFinite(mission)) return mission;
      return getSystemLocalClockSeconds();
    }

    function setMissionClockAnchor(missionSeconds, systemNowMs) {
      const m = Number(missionSeconds);
      if (!isFinite(m) || m < 0) return;
      runtimeState.missionClock.anchorSeconds = m;
      runtimeState.missionClock.anchorSystemMs = isFinite(Number(systemNowMs)) ? Number(systemNowMs) : Date.now();
    }

    function resetMissionClockAnchor() {
      runtimeState.missionClock.anchorSeconds = NaN;
      runtimeState.missionClock.anchorSystemMs = 0;
      runtimeState.missionClock.anchorIdentity = '';
    }

    function getFlightPlanEtaStartKey(selected) {
      return String(selected || '');
    }

    function getExpandedTimeAnchor(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return '';
      return String(fltPlanExpandedTimeAnchorBySelection[key] || '').toUpperCase();
    }

    function setExpandedTimeAnchor(selected, anchor) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const value = String(anchor || '').toUpperCase();
      if (value) {
        fltPlanExpandedTimeAnchorBySelection[key] = value;
      } else {
        delete fltPlanExpandedTimeAnchorBySelection[key];
      }
    }

    function getExpandedNavEdit(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return { step: '', field: '' };
      const state = getExpandedNavEditState(selected);
      if (!state || typeof state !== 'object') return { step: '', field: '' };
      return {
        step: String(state.step || ''),
        field: String(state.field || '').toUpperCase(),
      };
    }

    function getExpandedNavEditState(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return null;
      let state = fltPlanExpandedNavEditBySelection[key];
      if (!state || typeof state !== 'object') {
        state = {
          step: '',
          field: '',
          closingStep: '',
          closingField: '',
          closingUntilUtc: 0,
        };
        fltPlanExpandedNavEditBySelection[key] = state;
      }
      const until = Number(state.closingUntilUtc || 0);
      if (!isFinite(until) || until <= 0 || Date.now() >= until) {
        state.closingStep = '';
        state.closingField = '';
        state.closingUntilUtc = 0;
      }
      return state;
    }

    function setExpandedNavEdit(selected, step, field) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const stepKey = String(step || '');
      const fieldKey = String(field || '').toUpperCase();
      const state = getExpandedNavEditState(selected);
      if (!state) return;
      if (!stepKey || !fieldKey) {
        state.step = '';
        state.field = '';
        state.closingStep = '';
        state.closingField = '';
        state.closingUntilUtc = 0;
        return;
      }
      state.step = stepKey;
      state.field = fieldKey;
    }

    function closeExpandedNavEditAnimated(selected) {
      const state = getExpandedNavEditState(selected);
      if (!state) return false;
      const stepKey = stepToKey(state.step);
      const fieldKey = String(state.field || '').toUpperCase();
      if (!stepKey || !fieldKey) return false;
      state.closingStep = stepKey;
      state.closingField = fieldKey;
      state.closingUntilUtc = Date.now() + overlayCloseHideDelayMs;
      state.step = '';
      state.field = '';
      return true;
    }

    function isClosingNavEdit(selected, step, field) {
      const state = getExpandedNavEditState(selected);
      if (!state) return false;
      const until = Number(state.closingUntilUtc || 0);
      if (!isFinite(until) || until <= 0 || Date.now() >= until) {
        state.closingStep = '';
        state.closingField = '';
        state.closingUntilUtc = 0;
        return false;
      }
      return stepToKey(state.closingStep) === stepToKey(step)
        && String(state.closingField || '').toUpperCase() === String(field || '').toUpperCase();
    }

    function getActiveFlightPlanSelection(data) {
      const model = data || latestData || {};
      const selected = String(model.DtcSelectedFile || '');
      if (selected && runtimeFlightPlanUserOverride) return selected;
      const runtimeWaypoints = getMissionRuntimeWaypoints(model);
      if (runtimeWaypoints.length) return '__RUNTIME_PLAYER__';
      if (selected) return selected;
      return '';
    }

    function getMissionIdentity(data) {
      const server = (data && data.Server) || {};
      const rawCallsign = String(server.PlayerCallsign || '').trim();
      if (rawCallsign) {
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

    function resetRuntimeFlightPlanState() {
      const key = '__RUNTIME_PLAYER__';
      delete fltPlanEtaStartBySelection[key];
      delete fltPlanPlanStateBySelection[key];
      delete fltPlanDtcPageBySelection[key];
      delete fltPlanDtcRouteBySelection[key];
      delete fltPlanDtcMissionBySelection[key];
      delete fltPlanAh64CommPresetBySelection[key];
      delete fltPlanMapViewBySelection[key];
      delete fltPlanMapBackgroundEnabledBySelection[key];
      delete fltPlanOpenFreeMapViewBySelection[key];
      delete fltPlanMapSelectedAssetKeyBySelection[key];
      delete openFreeMapContainerIdBySelectionKey[key];
      if (!fakeMissionEnabled) {
        resetMissionClockAnchor();
      }
    }

    function invalidateRuntimeFlightPlanSnapshot() {
      runtimeFlightPlanSnapshot = null;
      runtimeFlightPlanSnapshotMissionIdentity = '';
      runtimeFlightPlanSnapshotGroupName = '';
      runtimeFlightPlanSnapshotFingerprint = '';
    }

    function invalidateEfbAirportCatalogCache() {
      efbAvailableAirports = null;
      efbAirportsLoadPromise = null;
      efbAirportEntries = {};
      efbChartsByAirport = {};
      efbChartsLastFetchMsByAirport = {};
      efbChartsLoadPromiseByAirport = {};
      efbSelectedChartByAirport = {};
      efbManuallySelectedAirport = '';
      efbLastResolvedAirport = '';
      efbUiDirty = true;
      setEfbLoadingOverlay('');
    }

    function maybeResetFlightPlanStateForMission(data) {
      const identity = getMissionIdentity(data);
      const missionClock = getMissionClockSeconds(data);
      const server = (data && data.Server) || {};
      const aircraft = String(server.Aircraft || '').trim();
      const hasActiveModule = aircraft.length > 0 && aircraft.toUpperCase() !== '----';
      const moduleConnected = !!server.ModuleConnected;
      const hasStableIdentityNow = moduleConnected
        && hasActiveModule
        && !!String(server.Theater || '').trim()
        && !!String(server.MissionTitle || '').trim();

      if (!hasActiveModule || !moduleConnected) {
        runtimeState.missionClock.rewindCandidateSeconds = NaN;
        runtimeState.missionClock.rewindConfirmCount = 0;
        pendingMissionIdentity = '';
        pendingMissionIdentityConfirmCount = 0;
        return;
      }

      let identityChanged = false;
      if (hasStableIdentityNow && !!identity && !!lastMissionIdentity && identity !== lastMissionIdentity) {
        if (pendingMissionIdentity === identity) {
          pendingMissionIdentityConfirmCount += 1;
        } else {
          pendingMissionIdentity = identity;
          pendingMissionIdentityConfirmCount = 1;
        }
        // Require repeated identical alternate identity before forcing FLT PLN runtime reset.
        identityChanged = pendingMissionIdentityConfirmCount >= 3;
      } else {
        pendingMissionIdentity = '';
        pendingMissionIdentityConfirmCount = 0;
      }

      let missionClockRewound = false;
      if (isFinite(missionClock)
        && isFinite(runtimeState.missionClock.lastSeconds)
        && missionClock + 60 < runtimeState.missionClock.lastSeconds) {
        const nearStartClock = missionClock <= 180;
        const hadPriorProgress = runtimeState.missionClock.lastSeconds >= 300;
        if (nearStartClock && hadPriorProgress) {
          if (isFinite(runtimeState.missionClock.rewindCandidateSeconds)
            && Math.abs(missionClock - runtimeState.missionClock.rewindCandidateSeconds) <= 20) {
            runtimeState.missionClock.rewindConfirmCount += 1;
          } else {
            runtimeState.missionClock.rewindCandidateSeconds = missionClock;
            runtimeState.missionClock.rewindConfirmCount = 1;
          }
          missionClockRewound = runtimeState.missionClock.rewindConfirmCount >= 2;
        } else {
          runtimeState.missionClock.rewindCandidateSeconds = NaN;
          runtimeState.missionClock.rewindConfirmCount = 0;
        }
      } else {
        runtimeState.missionClock.rewindCandidateSeconds = NaN;
        runtimeState.missionClock.rewindConfirmCount = 0;
      }

      if (identityChanged || missionClockRewound) {
        resetRuntimeFlightPlanState();
        invalidateRuntimeFlightPlanSnapshot();
        invalidateEfbAirportCatalogCache();
        resetMissionClockAnchor();
        runtimeFlightPlanUserOverride = false;
        efbSaOwnshipFast.hasPosition = false;
        efbSaOwnshipFast.updatedUtcMs = 0;
        pendingMissionIdentity = '';
        pendingMissionIdentityConfirmCount = 0;
        runtimeState.missionClock.rewindCandidateSeconds = NaN;
        runtimeState.missionClock.rewindConfirmCount = 0;
      }

      if (hasStableIdentityNow && identity) {
        lastMissionIdentity = identity;
      }
      if (isFinite(missionClock)) {
        runtimeState.missionClock.lastSeconds = missionClock;
      }
    }

    function getDtcPageBySelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      const v = Number(fltPlanDtcPageBySelection[key]);
      return (v === 2 || v === 3) ? v : 1;
    }

    function setDtcPageBySelection(selected, page) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const p = Number(page);
      fltPlanDtcPageBySelection[key] = (p === 2 || p === 3) ? p : 1;
    }

    function isValidDtcRouteKey(route) {
      return /^R([1-9]|1[0-2])$/.test(String(route || '').toUpperCase());
    }

    function isValidDtcMissionKey(missionKey) {
      return /^M([1-2])$/.test(String(missionKey || '').toUpperCase());
    }

    function getDtcRouteBySelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      const route = String(fltPlanDtcRouteBySelection[key] || '').toUpperCase();
      return isValidDtcRouteKey(route) ? route : 'R1';
    }

    function getDtcMissionBySelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      const missionKey = String(fltPlanDtcMissionBySelection[key] || '').toUpperCase();
      return isValidDtcMissionKey(missionKey) ? missionKey : 'M1';
    }

    function setDtcRouteBySelection(selected, route) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const r = String(route || '').toUpperCase();
      fltPlanDtcRouteBySelection[key] = isValidDtcRouteKey(r) ? r : 'R1';
    }

    function setDtcMissionBySelection(selected, missionKey) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const m = String(missionKey || '').toUpperCase();
      fltPlanDtcMissionBySelection[key] = isValidDtcMissionKey(m) ? m : 'M1';
    }

    function getAh64CommPresetBySelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      const raw = Number(fltPlanAh64CommPresetBySelection[key]);
      if (!isFinite(raw)) return 1;
      const n = Math.round(raw);
      return n >= 1 && n <= 10 ? n : 1;
    }

    function setAh64CommPresetBySelection(selected, presetNumber) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const n = Number(presetNumber);
      const bounded = isFinite(n) ? Math.max(1, Math.min(10, Math.round(n))) : 1;
      fltPlanAh64CommPresetBySelection[key] = bounded;
    }

    function updateDtcRouteButtonUi(selected) {
      const host = document.getElementById('tabBody');
      if (!host || !selected) return;
      const activeRoute = getDtcRouteBySelection(selected);
      const buttons = host.querySelectorAll ? host.querySelectorAll('[data-dtc-route]') : [];
      for (let i = 0; i < buttons.length; i++) {
        const btn = buttons[i];
        if (!btn || !btn.classList || !btn.getAttribute) continue;
        const key = String(btn.getAttribute('data-dtc-route') || '').toUpperCase();
        btn.classList.toggle('active', key === activeRoute);
      }

      const activeMission = getDtcMissionBySelection(selected);
      const missionButtons = host.querySelectorAll ? host.querySelectorAll('[data-dtc-mission]') : [];
      for (let i = 0; i < missionButtons.length; i++) {
        const btn = missionButtons[i];
        if (!btn || !btn.classList || !btn.getAttribute) continue;
        const key = String(btn.getAttribute('data-dtc-mission') || '').toUpperCase();
        btn.classList.toggle('active', key === activeMission);
      }
    }

    function isF14DtcContext(root, data) {
      const rootType = String((root && root.type) || '').toUpperCase();
      if (rootType.indexOf('F-14') >= 0 || rootType.indexOf('TOMCAT') >= 0) return true;
      const model = data || latestData || {};
      const aircraft = String((((model && model.Server) || {}).Aircraft) || '').toUpperCase();
      return aircraft.indexOf('F-14') >= 0 || aircraft.indexOf('TOMCAT') >= 0;
    }

    function getMapViewBySelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return { zoom: 1, panX: 0, panY: 0 };
      const existing = fltPlanMapViewBySelection[key];
      if (existing && isFinite(Number(existing.zoom)) && isFinite(Number(existing.panX)) && isFinite(Number(existing.panY))) {
        return existing;
      }
      const state = { zoom: 1, panX: 0, panY: 0 };
      fltPlanMapViewBySelection[key] = state;
      return state;
    }

    function resetMapViewBySelection(selected) {
      const state = getMapViewBySelection(selected);
      state.zoom = 1;
      state.panX = 0;
      state.panY = 0;
    }

    function zoomMapViewBySelection(selected, zoomFactor) {
      const state = getMapViewBySelection(selected);
      const factor = Number(zoomFactor);
      if (!isFinite(factor) || factor <= 0) return;
      const current = isFinite(Number(state.zoom)) ? Number(state.zoom) : 1;
      state.zoom = clamp(current * factor, 0.6, 4.0);
    }

    function isMapBackgroundEnabledBySelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return true;
      if (!Object.prototype.hasOwnProperty.call(fltPlanMapBackgroundEnabledBySelection, key)) {
        fltPlanMapBackgroundEnabledBySelection[key] = true;
      }
      return !!fltPlanMapBackgroundEnabledBySelection[key];
    }

    function toggleMapBackgroundEnabledBySelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const current = isMapBackgroundEnabledBySelection(selected);
      fltPlanMapBackgroundEnabledBySelection[key] = !current;
      if (!current) {
        const svgState = getMapViewBySelection(selected);
        const webState = getOpenFreeMapViewBySelection(selected);
        if (!isFinite(Number(webState.zoom))) {
          const svgZoom = isFinite(Number(svgState.zoom)) ? Number(svgState.zoom) : 1;
          webState.zoom = clamp(6 + Math.log(svgZoom) / Math.log(1.2), 2, 16);
        }
      } else {
        const svgState = getMapViewBySelection(selected);
        const webState = getOpenFreeMapViewBySelection(selected);
        if (isFinite(Number(webState.zoom))) {
          const webZoom = Number(webState.zoom);
          svgState.zoom = clamp(Math.pow(1.2, webZoom - 6), 0.6, 4.0);
        }
      }
    }

    function getOpenFreeMapViewBySelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return { centerLon: NaN, centerLat: NaN, zoom: NaN, bearing: NaN };
      const existing = fltPlanOpenFreeMapViewBySelection[key];
      if (existing && isFinite(Number(existing.centerLon)) && isFinite(Number(existing.centerLat)) && isFinite(Number(existing.zoom))) {
        return existing;
      }
      const state = { centerLon: NaN, centerLat: NaN, zoom: NaN, bearing: NaN };
      fltPlanOpenFreeMapViewBySelection[key] = state;
      return state;
    }

    function registerOpenFreeMapPayload(selected, payload) {
      if (!payload || typeof payload !== 'object') return '';
      const selectedKey = getFlightPlanEtaStartKey(selected);
      Object.keys(openFreeMapPayloadById).forEach(function (id) {
        const item = openFreeMapPayloadById[id];
        if (!item) return;
        if (String(item.selectedKey || '') === String(selectedKey || '')) {
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

    function getLatestOpenFreeMapPayloadBySelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return null;
      const ids = Object.keys(openFreeMapPayloadById);
      if (!ids.length) return null;
      let bestId = '';
      let bestSeq = -1;
      ids.forEach(function (id) {
        const item = openFreeMapPayloadById[id];
        if (!item) return;
        if (String(item.selectedKey || '') !== key) return;
        const seq = Number(String(id).replace('ofm_', ''));
        if (!isFinite(seq)) return;
        if (seq > bestSeq) {
          bestSeq = seq;
          bestId = id;
        }
      });
      if (!bestId) return null;
      const entry = openFreeMapPayloadById[bestId];
      return entry && entry.payload ? entry.payload : null;
    }

    function syncOpenFreeMapForSelection(selected, payload) {
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
      const wrap = item.host && item.host.closest ? item.host.closest('[data-openfreemap-wrap]') : null;
      const fallback = wrap ? wrap.querySelector('.fltPlanOpenMapFallback') : null;
      if (fallback) fallback.style.display = 'none';
      return true;
    }

    function renderOpenFreeMapOverlay(item, payload) {
      if (!item || !item.map || !item.host) return;
      const map = item.map;
      const host = item.host;
      const features = Array.isArray(payload && payload.features) ? payload.features : [];
      const orderedFeatures = features.slice().sort(function (a, b) {
        const ap = ((a || {}).properties || {});
        const bp = ((b || {}).properties || {});
        const ak = String(ap.kind || '').toLowerCase();
        const bk = String(bp.kind || '').toLowerCase();
        const aAirfield = ak === 'airfield' ? 1 : 0;
        const bAirfield = bk === 'airfield' ? 1 : 0;
        if (aAirfield !== bAirfield) return aAirfield - bAirfield;

        if (ak === 'jtac-target' && bk === 'jtac-target') {
          const aHistory = !!ap.isJtacHistory;
          const bHistory = !!bp.isJtacHistory;
          if (aHistory !== bHistory) return aHistory ? -1 : 1;
        }
        return 0;
      });

      let overlaySvg = item.overlaySvg;
      if (!overlaySvg) {
        overlaySvg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        overlaySvg.setAttribute('class', 'fltPlanOpenMapVectorOverlay');
        overlaySvg.setAttribute('width', '100%');
        overlaySvg.setAttribute('height', '100%');
        overlaySvg.style.pointerEvents = 'none';
        host.appendChild(overlaySvg);
        item.overlaySvg = overlaySvg;
      }

      const overlayRoot = overlaySvg;

      const selectedAssetKey = getMapSelectedAssetKeyBySelection(item.selectedKey);
      const selectedAssetInfo = { x: NaN, y: NaN, props: null };
      const modelForWaypoints = latestData || {};
      const theatreForWaypoints = String(resolveOpenFreeMapFallbackTheatreText(modelForWaypoints) || '').trim();
      function makeNode(name) { return document.createElementNS('http://www.w3.org/2000/svg', name); }
      const renderLayer = makeNode('g');
      overlaySvg = renderLayer;
      const metersPerPixel = (function () {
        try {
          const center = map.getCenter ? map.getCenter() : null;
          const lat = center && isFinite(Number(center.lat)) ? Number(center.lat) : 0;
          const zoom = map.getZoom ? Number(map.getZoom()) : 0;
          const mpp = (156543.03392804097 * Math.cos(lat * (Math.PI / 180))) / Math.pow(2, zoom);
          return (isFinite(mpp) && mpp > 0) ? mpp : NaN;
        } catch (_) {
          return NaN;
        }
      })();
      let mapZoom = NaN;
      try {
        mapZoom = Number(map && map.getZoom ? map.getZoom() : NaN);
      } catch (_) {
        mapZoom = NaN;
      }
      const doghousePlacedAnchors = [];

      function createAssetSymbol(px, py, props) {
        const g = makeNode('g');
        const kind = String((props && props.assetKind) || '').toLowerCase();
        const fill = '#2d8fe3';
        const stroke = '#1f5d93';
        if (kind === 'awacs') {
          const r = makeNode('rect');
          r.setAttribute('x', (px - 9.6).toFixed(1));
          r.setAttribute('y', (py - 7.2).toFixed(1));
          r.setAttribute('width', '19.2');
          r.setAttribute('height', '14.4');
          r.setAttribute('rx', '1.6');
          r.setAttribute('fill', fill);
          r.setAttribute('stroke', stroke);
          r.setAttribute('stroke-width', '1.3');
          g.appendChild(r);
          const l = makeNode('line');
          l.setAttribute('x1', (px - 7.2).toFixed(1));
          l.setAttribute('y1', py.toFixed(1));
          l.setAttribute('x2', (px + 7.2).toFixed(1));
          l.setAttribute('y2', py.toFixed(1));
          l.setAttribute('stroke', stroke);
          l.setAttribute('stroke-width', '1.2');
          g.appendChild(l);
          const c2 = makeNode('circle');
          c2.setAttribute('cx', px.toFixed(1));
          c2.setAttribute('cy', (py - 10.8).toFixed(1));
          c2.setAttribute('r', '2.88');
          c2.setAttribute('fill', stroke);
          g.appendChild(c2);
        } else if (kind === 'tanker') {
          const p = makeNode('polygon');
          p.setAttribute('points', (px - 9.6).toFixed(1) + ',' + py.toFixed(1) + ' ' + px.toFixed(1) + ',' + (py - 7.2).toFixed(1) + ' ' + (px + 9.6).toFixed(1) + ',' + py.toFixed(1) + ' ' + px.toFixed(1) + ',' + (py + 7.2).toFixed(1));
          p.setAttribute('fill', fill);
          p.setAttribute('stroke', stroke);
          p.setAttribute('stroke-width', '1.3');
          g.appendChild(p);
        } else if (kind === 'jtac') {
          const p = makeNode('polygon');
          p.setAttribute('points', px.toFixed(1) + ',' + (py - 8.4).toFixed(1) + ' ' + (px - 8.4).toFixed(1) + ',' + (py + 8.4).toFixed(1) + ' ' + (px + 8.4).toFixed(1) + ',' + (py + 8.4).toFixed(1));
          p.setAttribute('fill', fill);
          p.setAttribute('stroke', stroke);
          p.setAttribute('stroke-width', '1.3');
          g.appendChild(p);
        } else if (kind === 'jtac-target') {
          const p = makeNode('polygon');
          p.setAttribute('points', px.toFixed(1) + ',' + (py - 7.2).toFixed(1) + ' ' + (px - 7.2).toFixed(1) + ',' + py.toFixed(1) + ' ' + px.toFixed(1) + ',' + (py + 7.2).toFixed(1) + ' ' + (px + 7.2).toFixed(1) + ',' + py.toFixed(1));
          p.setAttribute('fill', String((props && props.fill) || '#d45757'));
          p.setAttribute('stroke', String((props && props.stroke) || '#7f2e2e'));
          p.setAttribute('stroke-width', '1.4');
          g.appendChild(p);
        } else if (kind === 'rotary') {
          const c = makeNode('circle');
          c.setAttribute('cx', px.toFixed(1));
          c.setAttribute('cy', py.toFixed(1));
          c.setAttribute('r', '7.2');
          c.setAttribute('fill', fill);
          c.setAttribute('stroke', stroke);
          c.setAttribute('stroke-width', '1.3');
          g.appendChild(c);
          const h = makeNode('line');
          h.setAttribute('x1', (px - 10.8).toFixed(1));
          h.setAttribute('y1', py.toFixed(1));
          h.setAttribute('x2', (px + 10.8).toFixed(1));
          h.setAttribute('y2', py.toFixed(1));
          h.setAttribute('stroke', stroke);
          h.setAttribute('stroke-width', '1.2');
          g.appendChild(h);
          const v = makeNode('line');
          v.setAttribute('x1', px.toFixed(1));
          v.setAttribute('y1', (py - 10.8).toFixed(1));
          v.setAttribute('x2', px.toFixed(1));
          v.setAttribute('y2', (py + 10.8).toFixed(1));
          v.setAttribute('stroke', stroke);
          v.setAttribute('stroke-width', '1.2');
          g.appendChild(v);
        } else if (kind === 'marker') {
          const c = makeNode('circle');
          c.setAttribute('cx', px.toFixed(1));
          c.setAttribute('cy', py.toFixed(1));
          c.setAttribute('r', '7.8');
          c.setAttribute('fill', '#f7edf8');
          c.setAttribute('stroke', '#8a2f99');
          c.setAttribute('stroke-width', '1.4');
          g.appendChild(c);
          const h = makeNode('line');
          h.setAttribute('x1', (px - 9.6).toFixed(1));
          h.setAttribute('y1', py.toFixed(1));
          h.setAttribute('x2', (px + 9.6).toFixed(1));
          h.setAttribute('y2', py.toFixed(1));
          h.setAttribute('stroke', '#8a2f99');
          h.setAttribute('stroke-width', '1.2');
          g.appendChild(h);
          const v = makeNode('line');
          v.setAttribute('x1', px.toFixed(1));
          v.setAttribute('y1', (py - 9.6).toFixed(1));
          v.setAttribute('x2', px.toFixed(1));
          v.setAttribute('y2', (py + 9.6).toFixed(1));
          v.setAttribute('stroke', '#8a2f99');
          v.setAttribute('stroke-width', '1.2');
          g.appendChild(v);
        } else if (kind === 'player') {
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
          r.setAttribute('x', (px - 8.4).toFixed(1));
          r.setAttribute('y', (py - 6.6).toFixed(1));
          r.setAttribute('width', '16.8');
          r.setAttribute('height', '13.2');
          r.setAttribute('fill', fill);
          r.setAttribute('stroke', stroke);
          r.setAttribute('stroke-width', '1.3');
          g.appendChild(r);
        }
        return g;
      }

      function createJtacFriendlySymbol(px, py, props) {
        const g = makeNode('g');
        const ring = makeNode('circle');
        ring.setAttribute('cx', px.toFixed(1));
        ring.setAttribute('cy', py.toFixed(1));
        ring.setAttribute('r', '6.2');
        ring.setAttribute('fill', 'none');
        ring.setAttribute('stroke', '#1f5d93');
        ring.setAttribute('stroke-width', '1.4');
        g.appendChild(ring);
        const h = makeNode('line');
        h.setAttribute('x1', (px - 5.2).toFixed(1));
        h.setAttribute('y1', py.toFixed(1));
        h.setAttribute('x2', (px + 5.2).toFixed(1));
        h.setAttribute('y2', py.toFixed(1));
        h.setAttribute('stroke', '#1f5d93');
        h.setAttribute('stroke-width', '1.3');
        g.appendChild(h);
        const v = makeNode('line');
        v.setAttribute('x1', px.toFixed(1));
        v.setAttribute('y1', (py - 5.2).toFixed(1));
        v.setAttribute('x2', px.toFixed(1));
        v.setAttribute('y2', (py + 5.2).toFixed(1));
        v.setAttribute('stroke', '#1f5d93');
        v.setAttribute('stroke-width', '1.3');
        g.appendChild(v);
        g.style.pointerEvents = 'none';
        return g;
      }

      orderedFeatures.forEach(function (feature) {
        if (!feature || !feature.geometry || !feature.properties) return;
        const geom = feature.geometry || {};
        const props = feature.properties || {};

        if (geom.type === 'LineString' && Array.isArray(geom.coordinates) && geom.coordinates.length >= 2) {
          const linePts = geom.coordinates.map(function (c) {
            if (!Array.isArray(c) || c.length < 2) return null;
            const p = map.project([Number(c[0]), Number(c[1])]);
            return p ? (p.x.toFixed(1) + ',' + p.y.toFixed(1)) : null;
          }).filter(function (v) { return !!v; });
          if (linePts.length >= 2) {
            const pl = makeNode('polyline');
            pl.setAttribute('points', linePts.join(' '));
            pl.setAttribute('fill', 'none');
            pl.setAttribute('stroke', String((props && props.stroke) || '#3d566e'));
            pl.setAttribute('stroke-width', String(Number((props && props.lineWidth) || 2.2) * 1.2));
            if (props && props.dashed) pl.setAttribute('stroke-dasharray', '8 6');
            pl.setAttribute('stroke-opacity', '0.92');
            pl.style.pointerEvents = 'none';
            overlaySvg.appendChild(pl);
          }
          return;
        }

        if (geom.type === 'Polygon' && Array.isArray(geom.coordinates) && geom.coordinates.length) {
          const firstRing = Array.isArray(geom.coordinates[0]) ? geom.coordinates[0] : [];
          const polyPts = firstRing.map(function (c) {
            if (!Array.isArray(c) || c.length < 2) return null;
            const p = map.project([Number(c[0]), Number(c[1])]);
            return p ? (p.x.toFixed(1) + ',' + p.y.toFixed(1)) : null;
          }).filter(function (v) { return !!v; });
          if (polyPts.length >= 3) {
            const poly = makeNode('polygon');
            poly.setAttribute('points', polyPts.join(' '));
            poly.setAttribute('fill', String((props && props.fill) || '#4faa4f'));
            poly.setAttribute('fill-opacity', String(isFinite(Number(props && props.fillOpacity)) ? Number(props.fillOpacity) : 0.18));
            poly.setAttribute('stroke', String((props && props.stroke) || '#2f8a2f'));
            poly.setAttribute('stroke-width', String(Number((props && props.lineWidth) || 1.4) * 1.2));
            if (props && props.dashed) poly.setAttribute('stroke-dasharray', '8 6');
            poly.style.pointerEvents = 'none';
            overlaySvg.appendChild(poly);
          }
          return;
        }

        if (geom.type === 'Point' && Array.isArray(geom.coordinates) && geom.coordinates.length >= 2) {
          const p = map.project([Number(geom.coordinates[0]), Number(geom.coordinates[1])]);
          if (!p) return;
          const kind = String((props && props.kind) || '').toLowerCase();
          const isAsset = props.group === 'asset' && kind !== 'airfield';
          const isSelectable = !!(props && props.assetKey) && (props.group === 'asset' || kind === 'airfield' || kind === 'user-waypoint');
          const isSelected = isSelectable && String(selectedAssetKey || '') === String(props.assetKey || '');
          const markerNode = isAsset
            ? createAssetSymbol(p.x, p.y, props)
            : (function () {
              const fill = String((props && props.fill) || '#3a6ea5');
              const stroke = String((props && props.stroke) || '#244766');
              if (kind === 'destination') {
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
              if (kind === 'user-waypoint') {
                const tri = makeNode('polygon');
                tri.setAttribute('points',
                  p.x.toFixed(1) + ',' + (p.y - 8.0).toFixed(1) + ' '
                  + (p.x - 7.2).toFixed(1) + ',' + (p.y + 5.8).toFixed(1) + ' '
                  + (p.x + 7.2).toFixed(1) + ',' + (p.y + 5.8).toFixed(1));
                tri.setAttribute('fill', 'none');
                tri.setAttribute('stroke', stroke);
                tri.setAttribute('stroke-width', '1.9');
                tri.style.pointerEvents = 'none';
                return tri;
              }
              if (kind === 'waypoint') {
                const wpType = String((props && props.waypointType) || '').toLowerCase();
                if (wpType === 'ip') {
                  const s = 8.8;
                  const ip = makeNode('rect');
                  ip.setAttribute('x', (p.x - s).toFixed(1));
                  ip.setAttribute('y', (p.y - s).toFixed(1));
                  ip.setAttribute('width', (s * 2).toFixed(1));
                  ip.setAttribute('height', (s * 2).toFixed(1));
                  ip.setAttribute('fill', 'none');
                  ip.setAttribute('stroke', '#2f7f4f');
                  ip.setAttribute('stroke-width', (props && props.autoAtaPulseActive) ? '2.8' : '2.0');
                  if (props && props.autoAtaPulseActive) {
                    ip.setAttribute('stroke', '#d13a3a');
                  }
                  ip.style.pointerEvents = 'none';
                  return ip;
                }
                if (wpType === 'tgt') {
                  const tgt = makeNode('polygon');
                  tgt.setAttribute('points',
                    p.x.toFixed(1) + ',' + (p.y - 8.8).toFixed(1) + ' '
                    + (p.x - 8.8).toFixed(1) + ',' + p.y.toFixed(1) + ' '
                    + p.x.toFixed(1) + ',' + (p.y + 8.8).toFixed(1) + ' '
                    + (p.x + 8.8).toFixed(1) + ',' + p.y.toFixed(1));
                  tgt.setAttribute('fill', 'none');
                  tgt.setAttribute('stroke', '#a33d3d');
                  tgt.setAttribute('stroke-width', (props && props.autoAtaPulseActive) ? '3.0' : '2.0');
                  if (props && props.autoAtaPulseActive) {
                    tgt.setAttribute('stroke', '#ff4d4d');
                  }
                  tgt.style.pointerEvents = 'none';
                  return tgt;
                }
                if (wpType === 'home' || wpType === 'ldg') {
                  const roofTop = p.x.toFixed(1) + ',' + (p.y - 10.4).toFixed(1);
                  const roofL = (p.x - 8.8).toFixed(1) + ',' + (p.y - 2.0).toFixed(1);
                  const roofR = (p.x + 8.8).toFixed(1) + ',' + (p.y - 2.0).toFixed(1);
                  const homeRoof = makeNode('polygon');
                  homeRoof.setAttribute('points', roofTop + ' ' + roofL + ' ' + roofR);
                  homeRoof.setAttribute('fill', 'none');
                  homeRoof.setAttribute('stroke', '#735c2f');
                  if (props && props.autoAtaPulseActive) {
                    homeRoof.setAttribute('stroke', '#d04444');
                  }
                  homeRoof.setAttribute('stroke-width', '2.0');
                  const homeBase = makeNode('rect');
                  homeBase.setAttribute('x', (p.x - 7.2).toFixed(1));
                  homeBase.setAttribute('y', (p.y - 2.0).toFixed(1));
                  homeBase.setAttribute('width', '14.4');
                  homeBase.setAttribute('height', '8.8');
                  homeBase.setAttribute('fill', 'none');
                  homeBase.setAttribute('stroke', '#b1945a');
                  if (props && props.autoAtaPulseActive) {
                    homeBase.setAttribute('stroke', '#ff6666');
                  }
                  homeBase.setAttribute('stroke-width', '2.0');
                  const gHome = makeNode('g');
                  gHome.appendChild(homeRoof);
                  gHome.appendChild(homeBase);
                  gHome.style.pointerEvents = 'none';
                  return gHome;
                }
                if (wpType === 'aar' || wpType === 'cap' || wpType === 'hld') {
                  const gRacetrack = makeNode('g');
                  const raceRect = makeNode('rect');
                  const raceColor = wpType === 'aar' ? '#111111' : (wpType === 'cap' ? '#2f5fa7' : '#2f7f4f');
                  raceRect.setAttribute('x', (p.x - 19).toFixed(1));
                  raceRect.setAttribute('y', (p.y - 10).toFixed(1));
                  raceRect.setAttribute('width', '38');
                  raceRect.setAttribute('height', '20');
                  raceRect.setAttribute('rx', '9');
                  raceRect.setAttribute('ry', '9');
                  raceRect.setAttribute('fill', 'none');
                  raceRect.setAttribute('stroke', (props && props.autoAtaPulseActive) ? '#d13a3a' : raceColor);
                  raceRect.setAttribute('stroke-width', (props && props.autoAtaPulseActive) ? '2.8' : '2.0');
                  gRacetrack.appendChild(raceRect);
                  const raceTxt = makeNode('text');
                  raceTxt.setAttribute('x', p.x.toFixed(1));
                  raceTxt.setAttribute('y', (p.y + 4.4).toFixed(1));
                  raceTxt.setAttribute('text-anchor', 'middle');
                  raceTxt.setAttribute('font-size', '10.8');
                  raceTxt.setAttribute('font-weight', '700');
                  raceTxt.setAttribute('fill', raceColor);
                  raceTxt.textContent = wpType.toUpperCase();
                  gRacetrack.appendChild(raceTxt);
                  gRacetrack.style.pointerEvents = 'none';
                  return gRacetrack;
                }

                const wpCircle = makeNode('circle');
                wpCircle.setAttribute('cx', p.x.toFixed(1));
                wpCircle.setAttribute('cy', p.y.toFixed(1));
                wpCircle.setAttribute('r', '8.8');
                wpCircle.setAttribute('fill', 'none');
                wpCircle.setAttribute('stroke', fill);
                wpCircle.setAttribute('stroke-width', '2.3');
                wpCircle.style.pointerEvents = 'none';
                return wpCircle;
              }
              if (kind === 'airfield') {
                const gField = makeNode('g');
                const type = String((props && props.airfieldType) || 'airport').toLowerCase();
                const military = !!(props && props.airfieldMilitary);
                const r = 7.8;

                const ring = makeNode('circle');
                ring.setAttribute('cx', p.x.toFixed(1));
                ring.setAttribute('cy', p.y.toFixed(1));
                ring.setAttribute('r', String(r));
                ring.setAttribute('fill', 'none');
                ring.setAttribute('stroke', stroke);
                ring.setAttribute('stroke-width', '2');
                gField.appendChild(ring);

                if (!military) {
                  for (let i = 0; i < 6; i++) {
                    const a = i * (Math.PI / 3.0);
                    const x1 = p.x + (Math.cos(a) * (r + 0.6));
                    const y1 = p.y + (Math.sin(a) * (r + 0.6));
                    const x2 = p.x + (Math.cos(a) * (r + 2.64));
                    const y2 = p.y + (Math.sin(a) * (r + 2.64));
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

                if (type === 'heliport') {
                  const tx = makeNode('text');
                  tx.setAttribute('x', p.x.toFixed(1));
                  tx.setAttribute('y', (p.y + 4.56).toFixed(1));
                  tx.setAttribute('text-anchor', 'middle');
                  tx.setAttribute('font-size', '10.6');
                  tx.setAttribute('font-weight', '800');
                  tx.setAttribute('fill', stroke);
                  tx.textContent = 'H';
                  gField.appendChild(tx);
                } else if (type === 'seaplane') {
                  const tx = makeNode('text');
                  tx.setAttribute('x', p.x.toFixed(1));
                  tx.setAttribute('y', (p.y + 4.56).toFixed(1));
                  tx.setAttribute('text-anchor', 'middle');
                  tx.setAttribute('font-size', '10.8');
                  tx.setAttribute('font-weight', '700');
                  tx.setAttribute('fill', stroke);
                  tx.textContent = '\u2693';
                  gField.appendChild(tx);
                }

                gField.style.pointerEvents = 'none';
                return gField;
              }
              if (kind === 'player') {
                const gPlayer = makeNode('g');
                const heading = Number(props && props.headingDeg);
                let mapBearing = 0;
                try {
                  mapBearing = Number(map && map.getBearing ? map.getBearing() : 0);
                } catch (_) {
                  mapBearing = 0;
                }
                const rotation = isFinite(heading) ? normalizeHeadingDeg(heading - mapBearing) : 0;
                gPlayer.setAttribute('transform', 'rotate(' + rotation.toFixed(1) + ' ' + p.x.toFixed(1) + ' ' + p.y.toFixed(1) + ')');

                const tri = makeNode('polygon');
                tri.setAttribute('points',
                  p.x.toFixed(1) + ',' + p.y.toFixed(1) + ' '
                  + (p.x - 10.2).toFixed(1) + ',' + (p.y + 22.8).toFixed(1) + ' '
                  + p.x.toFixed(1) + ',' + (p.y + 17.3).toFixed(1) + ' '
                  + (p.x + 10.2).toFixed(1) + ',' + (p.y + 22.8).toFixed(1));
                tri.setAttribute('fill', '#d79cff');
                tri.setAttribute('stroke', '#8a2f99');
                tri.setAttribute('stroke-width', '1.4');
                gPlayer.appendChild(tri);

                gPlayer.style.pointerEvents = 'none';
                return gPlayer;
              }
              if (kind === 'threat') {
                const gThreat = makeNode('g');
                const isSamThreat = String((props && props.threatSubtype) || '').toLowerCase() === 'sam';
                const isSamNaval = isSamThreat && String((props && props.samNavalPlatformCode) || '').trim().length > 0;
                const samConfirmedHostile = !!(props && props.samConfirmedHostile);
                const samIconResolved = isSamThreat ? !!(props && props.samIconResolved) : true;
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
                if (isSamThreat) {
                  if (isSamNaval) {
                    if (samConfirmedHostile) {
                      const hostileDiamond = makeNode('polygon');
                      hostileDiamond.setAttribute('points',
                        p.x.toFixed(1) + ',' + (p.y - 9.2).toFixed(1) + ' '
                        + (p.x - 9.2).toFixed(1) + ',' + p.y.toFixed(1) + ' '
                        + p.x.toFixed(1) + ',' + (p.y + 9.2).toFixed(1) + ' '
                        + (p.x + 9.2).toFixed(1) + ',' + p.y.toFixed(1));
                      hostileDiamond.setAttribute('fill', 'none');
                      hostileDiamond.setAttribute('stroke', stroke);
                      hostileDiamond.setAttribute('stroke-width', '1.8');
                      gThreat.appendChild(hostileDiamond);
                    } else {
                      const unknownV = makeNode('path');
                      unknownV.setAttribute('d', 'M ' + (p.x - 9.2).toFixed(1) + ' ' + (p.y - 7.8).toFixed(1) + ' L ' + p.x.toFixed(1) + ' ' + (p.y + 8.2).toFixed(1) + ' L ' + (p.x + 9.2).toFixed(1) + ' ' + (p.y - 7.8).toFixed(1));
                      unknownV.setAttribute('fill', 'none');
                      unknownV.setAttribute('stroke', stroke);
                      unknownV.setAttribute('stroke-width', '1.8');
                      gThreat.appendChild(unknownV);
                    }
                  } else {
                    if (samIconResolved) {
                      const resolvedBox = makeNode('rect');
                      resolvedBox.setAttribute('x', (p.x - 8.5).toFixed(1));
                      resolvedBox.setAttribute('y', (p.y - 8.5).toFixed(1));
                      resolvedBox.setAttribute('width', '17');
                      resolvedBox.setAttribute('height', '17');
                      resolvedBox.setAttribute('fill', 'none');
                      resolvedBox.setAttribute('stroke', stroke);
                      resolvedBox.setAttribute('stroke-width', '1.8');
                      gThreat.appendChild(resolvedBox);
                    } else {
                      const unresolvedBottomOpen = makeNode('path');
                      unresolvedBottomOpen.setAttribute('d', 'M ' + (p.x - 8.8).toFixed(1) + ' ' + (p.y + 7.8).toFixed(1) + ' L ' + (p.x - 8.8).toFixed(1) + ' ' + (p.y - 7.8).toFixed(1) + ' L ' + (p.x + 8.8).toFixed(1) + ' ' + (p.y - 7.8).toFixed(1) + ' L ' + (p.x + 8.8).toFixed(1) + ' ' + (p.y + 7.8).toFixed(1));
                      unresolvedBottomOpen.setAttribute('fill', 'none');
                      unresolvedBottomOpen.setAttribute('stroke', stroke);
                      unresolvedBottomOpen.setAttribute('stroke-width', '1.8');
                      gThreat.appendChild(unresolvedBottomOpen);
                    }
                  }
                }
                const radiusMeters = Number(props && props.radiusMeters);
                const showRing = !!(props && props.ring) && isFinite(radiusMeters) && radiusMeters > 0 && isFinite(metersPerPixel);
                if (showRing) {
                  const rawRingsNm = Array.isArray(props && props.ringRadiiNm) ? props.ringRadiiNm : [];
                  const ringRadiiMeters = rawRingsNm
                    .map(function (nm) { return Number(nm) * 1852.0; })
                    .filter(function (m) { return isFinite(m) && m > 0; })
                    .sort(function (a, b) { return a - b; });
                  if (!ringRadiiMeters.length) {
                    ringRadiiMeters.push(radiusMeters);
                  }

                  const confidenceBandsNm = Array.isArray(props && props.confidenceBandsNm) ? props.confidenceBandsNm : [];
                  confidenceBandsNm
                    .map(function (nm) { return Number(nm) * 1852.0; })
                    .filter(function (m) { return isFinite(m) && m > 0; })
                    .sort(function (a, b) { return a - b; })
                    .forEach(function (bandMeters, idx) {
                      const bandPx = Math.max(4, bandMeters / metersPerPixel);
                      const band = makeNode('circle');
                      band.setAttribute('cx', p.x.toFixed(1));
                      band.setAttribute('cy', p.y.toFixed(1));
                      band.setAttribute('r', bandPx.toFixed(1));
                      band.setAttribute('fill', stroke);
                      band.setAttribute('fill-opacity', idx === confidenceBandsNm.length - 1 ? '0.05' : '0.03');
                      band.setAttribute('stroke', 'none');
                      band.style.pointerEvents = 'none';
                      gThreat.appendChild(band);
                    });

                  ringRadiiMeters.forEach(function (ringMeters, idx) {
                    const ringPx = Math.max(4, ringMeters / metersPerPixel);
                    const ring = makeNode('circle');
                    ring.setAttribute('cx', p.x.toFixed(1));
                    ring.setAttribute('cy', p.y.toFixed(1));
                    ring.setAttribute('r', ringPx.toFixed(1));
                    ring.setAttribute('fill', 'none');
                    ring.setAttribute('stroke', stroke);
                    ring.setAttribute('stroke-width', idx === ringRadiiMeters.length - 1 ? '1.5' : '1.1');
                    ring.setAttribute('stroke-dasharray', idx === ringRadiiMeters.length - 1 ? '7 5' : '4 5');
                    ring.style.pointerEvents = 'none';
                    gThreat.appendChild(ring);
                  });
                }
                gThreat.style.pointerEvents = (props && props.assetKey) ? 'auto' : 'none';
                return gThreat;
              }
              if (kind === 'bullseye') {
                const gBull = makeNode('g');
                [11, 7, 3].forEach(function (r) {
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
              if (kind === 'cap') {
                const gCap = makeNode('g');
                const courseDeg = isFinite(Number(props && props.course)) ? Number(props.course) : 0;
                const courseRad = courseDeg * (Math.PI / 180);
                let mapBearing = 0;
                try {
                  mapBearing = Number(map && map.getBearing ? map.getBearing() : 0);
                } catch (_) {
                  mapBearing = 0;
                }
                const capLengthMeters = Math.max(4000, isFinite(Number(props && props.lengthMeters)) ? Number(props && props.lengthMeters) : 12000);
                const capDiameterMeters = Math.max(2000, isFinite(Number(props && props.diameterMeters)) ? Number(props && props.diameterMeters) : 6000);
                const widthPx = isFinite(metersPerPixel) ? Math.max(16, ((capLengthMeters + capDiameterMeters) / metersPerPixel)) : 40;
                const heightPx = isFinite(metersPerPixel) ? Math.max(10, (capDiameterMeters / metersPerPixel)) : 18;
                const radiusPx = heightPx / 2;
                const turnDir = String((props && props.turnDirection) || '').trim().toUpperCase();
                const isRightPattern = turnDir.indexOf('RIGHT') >= 0;
                const angleDeg = (courseDeg - 90) + (isRightPattern ? 0 : 180);
                const renderAngleDeg = normalizeHeadingDeg(angleDeg - mapBearing);
                const angleRad = renderAngleDeg * (Math.PI / 180);

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
                rect.setAttribute('transform', 'rotate(' + renderAngleDeg.toFixed(1) + ' ' + centerX.toFixed(1) + ' ' + centerY.toFixed(1) + ')');
                gCap.appendChild(rect);

                const anchor = makeNode('circle');
                anchor.setAttribute('cx', p.x.toFixed(1));
                anchor.setAttribute('cy', p.y.toFixed(1));
                anchor.setAttribute('r', '2.9');
                anchor.setAttribute('fill', stroke);
                gCap.appendChild(anchor);

                const capLabel = String((props && props.label) || '').trim();
                if (capLabel) {
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
              if (kind === 'geoline') {
                const geo = makeNode('circle');
                geo.setAttribute('cx', p.x.toFixed(1));
                geo.setAttribute('cy', p.y.toFixed(1));
                geo.setAttribute('r', '3.8');
                geo.setAttribute('fill', fill);
                geo.style.pointerEvents = 'none';
                return geo;
              }
              if (kind === 'doghouse') {
                if (!isFinite(mapZoom) || mapZoom < efbSaDoghouseMinZoom) {
                  return null;
                }
                const headingDeg = Number(props && props.headingDeg);
                const roofHeight = 26;
                const bodyWidth = 120;
                const bodyHeight = 108;
                const rowHeight = bodyHeight / 3;
                const zoomFactor = clamp((mapZoom - efbSaDoghouseMinZoom) / 3.0, 0, 1);
                const offsetNmDynamic = Math.max(0.6, Number(efbSaDoghouseOffsetNm) * (1.0 - (0.65 * zoomFactor)));
                const offsetPxFromNm = isFinite(metersPerPixel) && metersPerPixel > 0
                  ? ((offsetNmDynamic * 1852.0) / metersPerPixel)
                  : 0;
                const minRouteClearancePx = (bodyWidth / 2) + 10;
                const offsetPx = Math.max(minRouteClearancePx, offsetPxFromNm);

                const gDog = makeNode('g');
                let mapBearing = 0;
                try {
                  mapBearing = Number(map && map.getBearing ? map.getBearing() : 0);
                } catch (_) {
                  mapBearing = 0;
                }
                const renderHeading = isFinite(headingDeg)
                  ? normalizeHeadingDeg(headingDeg - mapBearing)
                  : 0;
                const headingRad = renderHeading * (Math.PI / 180.0);
                const rightX = Math.cos(headingRad);
                const rightY = Math.sin(headingRad);
                const minSepPx = Math.max(118, Math.max(bodyWidth, bodyHeight) + 12);
                function hasConflict(x, y) {
                  return doghousePlacedAnchors.some(function (a) {
                    const dx = Number(a.x) - Number(x);
                    const dy = Number(a.y) - Number(y);
                    return ((dx * dx) + (dy * dy)) < (minSepPx * minSepPx);
                  });
                }

                let sideSign = 1;
                let anchorX = p.x + (rightX * offsetPx * sideSign);
                let anchorY = p.y + (rightY * offsetPx * sideSign);
                if (hasConflict(anchorX, anchorY)) {
                  sideSign = -1;
                  anchorX = p.x + (rightX * offsetPx * sideSign);
                  anchorY = p.y + (rightY * offsetPx * sideSign);
                }
                const bodyX = anchorX - (bodyWidth / 2);
                const bodyY = anchorY - (bodyHeight / 2);
                gDog.setAttribute('transform', 'rotate(' + renderHeading.toFixed(1) + ' ' + anchorX.toFixed(1) + ' ' + anchorY.toFixed(1) + ')');

                const roof = makeNode('polygon');
                roof.setAttribute('points',
                  anchorX.toFixed(1) + ',' + (bodyY - roofHeight).toFixed(1) + ' '
                  + bodyX.toFixed(1) + ',' + bodyY.toFixed(1) + ' '
                  + (bodyX + bodyWidth).toFixed(1) + ',' + bodyY.toFixed(1));
                roof.setAttribute('fill', '#ffffff');
                roof.setAttribute('fill-opacity', '0.55');
                roof.setAttribute('stroke', '#000000');
                roof.setAttribute('stroke-width', '2.6');
                gDog.appendChild(roof);

                const body = makeNode('rect');
                body.setAttribute('x', bodyX.toFixed(1));
                body.setAttribute('y', bodyY.toFixed(1));
                body.setAttribute('width', bodyWidth.toFixed(1));
                body.setAttribute('height', bodyHeight.toFixed(1));
                body.setAttribute('rx', '2');
                body.setAttribute('ry', '2');
                body.setAttribute('fill', '#ffffff');
                body.setAttribute('fill-opacity', '0.55');
                body.setAttribute('stroke', '#000000');
                body.setAttribute('stroke-width', '2.6');
                gDog.appendChild(body);

                const sep1 = makeNode('line');
                sep1.setAttribute('x1', bodyX.toFixed(1));
                sep1.setAttribute('y1', (bodyY + rowHeight).toFixed(1));
                sep1.setAttribute('x2', (bodyX + bodyWidth).toFixed(1));
                sep1.setAttribute('y2', (bodyY + rowHeight).toFixed(1));
                sep1.setAttribute('stroke', '#000000');
                sep1.setAttribute('stroke-width', '2.0');
                gDog.appendChild(sep1);

                const sep2 = makeNode('line');
                sep2.setAttribute('x1', bodyX.toFixed(1));
                sep2.setAttribute('y1', (bodyY + (rowHeight * 2)).toFixed(1));
                sep2.setAttribute('x2', (bodyX + bodyWidth).toFixed(1));
                sep2.setAttribute('y2', (bodyY + (rowHeight * 2)).toFixed(1));
                sep2.setAttribute('stroke', '#000000');
                sep2.setAttribute('stroke-width', '2.0');
                gDog.appendChild(sep2);

                function row(y, text, center) {
                  const tx = makeNode('text');
                  tx.setAttribute('x', (center ? anchorX : (bodyX + 8)).toFixed(1));
                  tx.setAttribute('y', y.toFixed(1));
                  tx.setAttribute('text-anchor', center ? 'middle' : 'start');
                  tx.setAttribute('dominant-baseline', 'middle');
                  tx.setAttribute('font-size', '18');
                  tx.setAttribute('font-weight', '700');
                  tx.setAttribute('fill', '#000000');
                  tx.setAttribute('font-family', 'Consolas, monospace');
                  tx.textContent = String(text || '');
                  gDog.appendChild(tx);
                }

                row(bodyY - (roofHeight * 0.42), String((props && props.mhText) || '-'), true);
                row(bodyY + (rowHeight * 0.5), 'DIST ' + String((props && props.distText) || '-') + ' ' + String((props && props.distUnit) || 'NM'), true);
                row(bodyY + (rowHeight * 1.5), 'ETE ' + String((props && props.eteText) || '--:--'), true);
                row(bodyY + (rowHeight * 2.5), 'ALT ' + String((props && props.altText) || '-') + ' ' + String((props && props.altUnit) || 'ft'), true);

                doghousePlacedAnchors.push({
                  x: anchorX,
                  y: anchorY
                });

                gDog.style.pointerEvents = 'none';
                return gDog;
              }
            if (kind === 'jtac-friendly') {
              return createJtacFriendlySymbol(p.x, p.y, props);
            }
            if (kind === 'jtac-egress') {
              const tip = makeNode('polygon');
              tip.setAttribute('points', p.x.toFixed(1) + ',' + (p.y - 4.6).toFixed(1) + ' ' + (p.x - 4.0).toFixed(1) + ',' + (p.y + 3.8).toFixed(1) + ' ' + (p.x + 4.0).toFixed(1) + ',' + (p.y + 3.8).toFixed(1));
              tip.setAttribute('fill', '#7f2e2e');
              tip.setAttribute('stroke', '#7f2e2e');
              tip.setAttribute('stroke-width', '1.1');
              tip.style.pointerEvents = 'none';
              return tip;
            }
              const c = makeNode('circle');
              c.setAttribute('cx', p.x.toFixed(1));
              c.setAttribute('cy', p.y.toFixed(1));
              c.setAttribute('r', String(Number((props && props.radius) || 5.2)));
              c.setAttribute('fill', fill);
              c.setAttribute('stroke', stroke);
              c.setAttribute('stroke-width', (props && props.autoAtaPulseActive) ? '2.6' : '1.6');
              if (props && props.autoAtaPulseActive) {
                c.setAttribute('fill', '#ff5a5a');
                c.setAttribute('stroke', '#bf2b2b');
              }
              c.style.pointerEvents = 'none';
              return c;
            })();

          if (!markerNode) {
            return;
          }

          if (markerNode && (kind === 'waypoint' || kind === 'airfield' || kind === 'user-waypoint')) {
            markerNode.setAttribute('transform', 'translate(' + p.x.toFixed(1) + ' ' + p.y.toFixed(1) + ') scale(1.5) translate(' + (-p.x).toFixed(1) + ' ' + (-p.y).toFixed(1) + ')');
            if (props && props.autoAtaPulseActive) {
              markerNode.setAttribute('filter', 'drop-shadow(0 0 4px rgba(255,72,72,0.95)) drop-shadow(0 0 9px rgba(255,72,72,0.85))');
            }
          }

          if (isSelectable && props.assetKey) {
            markerNode.style.pointerEvents = 'auto';
            markerNode.style.cursor = 'pointer';
            markerNode.setAttribute('data-asset-key', String(props.assetKey));
            if (isSelected) {
              const selectedRingRadius = (kind === 'airfield' || kind === 'waypoint' || kind === 'user-waypoint') ? 16.5 : 11;
              const ring = makeNode('circle');
              ring.setAttribute('cx', p.x.toFixed(1));
              ring.setAttribute('cy', p.y.toFixed(1));
              ring.setAttribute('r', selectedRingRadius.toFixed(1));
              ring.setAttribute('fill', 'none');
              ring.setAttribute('stroke', '#c94444');
              ring.setAttribute('stroke-width', (kind === 'airfield' || kind === 'waypoint' || kind === 'user-waypoint') ? '3.0' : '2.4');
              ring.style.pointerEvents = 'none';
              overlaySvg.appendChild(ring);
              selectedAssetInfo.x = p.x;
              selectedAssetInfo.y = p.y;
              selectedAssetInfo.props = props;
            }
          }
          overlaySvg.appendChild(markerNode);

          const label = String(props.label || '').trim();
          if (label) {
            const labelMinZoom = Number(props && props.labelMinZoom);
            const hideForDeclutter = isFinite(labelMinZoom) && isFinite(mapZoom) && mapZoom < labelMinZoom && !isSelected;
            if (hideForDeclutter) {
              return;
            }
            const kind = String((props && props.kind) || '').toLowerCase();
              const isSamThreatLabel = kind === 'threat' && String((props && props.threatSubtype) || '').toLowerCase() === 'sam';
              const isNavalSamLabel = isSamThreatLabel && String((props && props.samNavalPlatformCode) || '').trim().length > 0;
            const isAirfieldLabel = kind === 'airfield';
            const isWaypointLabel = kind === 'waypoint' || kind === 'user-waypoint';
            const isAreaLabel = kind === 'area-label';
            const isOverlayLabel = String((props && props.group) || '').toLowerCase() === 'overlay';
              const useScaledLabel = isAirfieldLabel || isWaypointLabel || isSamThreatLabel || isAreaLabel || isOverlayLabel;
            const text = makeNode('text');
              text.setAttribute('x', (p.x + (isAirfieldLabel ? 16 : (isWaypointLabel ? 16 : (isAreaLabel ? 16 : (isOverlayLabel ? 16 : (isNavalSamLabel ? 18 : (isSamThreatLabel ? 13 : 9))))))).toFixed(1));
            text.setAttribute('y', (p.y + 4).toFixed(1));
              text.setAttribute('font-size', useScaledLabel ? '18.5' : '11');
            text.setAttribute('font-weight', '700');
            const labelFill = isAsset
              ? '#1f3550'
              : (isAirfieldLabel
                ? (props && props.isVfr ? '#1e6b3d' : '#1d4f87')
                : String((props && props.textColor) || '#1f2e3d'));
            text.setAttribute('fill', labelFill);
            text.setAttribute('stroke', '#ffffff');
            text.setAttribute('stroke-width', useScaledLabel ? '1.0' : '0.6');
            text.style.pointerEvents = 'none';
            text.textContent = label;
            overlaySvg.appendChild(text);
          }

          if (geom.type === 'Point' && String((props && props.kind) || '').toLowerCase() === 'jtac-egress') {
            const parentTarget = props && props.parentTargetLonLat;
            if (Array.isArray(parentTarget) && parentTarget.length >= 2) {
              const start = map.project([Number(parentTarget[0]), Number(parentTarget[1])]);
              if (start) {
                const egrLine = makeNode('line');
                egrLine.setAttribute('x1', start.x.toFixed(1));
                egrLine.setAttribute('y1', start.y.toFixed(1));
                egrLine.setAttribute('x2', p.x.toFixed(1));
                egrLine.setAttribute('y2', p.y.toFixed(1));
                egrLine.setAttribute('stroke', '#7f2e2e');
                egrLine.setAttribute('stroke-width', '2.0');
                egrLine.style.pointerEvents = 'none';
                overlaySvg.insertBefore(egrLine, markerNode);
              }
            }
          }

          if (isSelectable && props.assetKey) {
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

      let renderedUserWaypointCount = 0;
      if (efbSaShowUserWaypoints !== false) {
        const userWaypointLayer = makeNode('g');
        userWaypointLayer.setAttribute('data-efb-user-waypoint-layer', '1');
        (Array.isArray(efbSaUserWaypoints) ? efbSaUserWaypoints : []).forEach(function (wp) {
          const id = String((wp && wp.id) || '').trim();
          if (!id) return;
          const ll = resolveEfbSaUserWaypointLonLat(theatreForWaypoints, wp);
          if (!ll) return;
          let p = null;
          try {
            p = map.project([Number(ll.lon), Number(ll.lat)]);
          } catch (_) {
            p = null;
          }
          if (!p || !isFinite(Number(p.x)) || !isFinite(Number(p.y))) return;

          renderedUserWaypointCount++;
          const stroke = '#205a99';
          const name = String((wp && wp.name) || '').trim() || 'UWP';
          const assetKey = buildEfbSaUserWaypointAssetKey(id);
          const isSelected = String(selectedAssetKey || '') === assetKey;

          const tri = makeNode('polygon');
          tri.setAttribute('points',
            p.x.toFixed(1) + ',' + (p.y - 12.0).toFixed(1) + ' '
            + (p.x - 10.8).toFixed(1) + ',' + (p.y + 8.7).toFixed(1) + ' '
            + (p.x + 10.8).toFixed(1) + ',' + (p.y + 8.7).toFixed(1));
          tri.setAttribute('fill', 'none');
          tri.setAttribute('stroke', stroke);
          tri.setAttribute('stroke-width', '2.8');
          tri.style.pointerEvents = 'auto';
          tri.style.cursor = 'pointer';
          tri.setAttribute('data-asset-key', assetKey);
          userWaypointLayer.appendChild(tri);

          const hit = makeNode('circle');
          hit.setAttribute('cx', p.x.toFixed(1));
          hit.setAttribute('cy', p.y.toFixed(1));
          hit.setAttribute('r', '16');
          hit.setAttribute('fill', '#000000');
          hit.setAttribute('fill-opacity', '0.01');
          hit.style.pointerEvents = 'auto';
          hit.style.cursor = 'pointer';
          hit.setAttribute('data-asset-key', assetKey);
          userWaypointLayer.appendChild(hit);

          const label = makeNode('text');
          label.setAttribute('x', (p.x + 16).toFixed(1));
          label.setAttribute('y', (p.y + 4).toFixed(1));
          label.setAttribute('font-size', '16.5');
          label.setAttribute('font-weight', '700');
          label.setAttribute('fill', stroke);
          label.setAttribute('stroke', '#ffffff');
          label.setAttribute('stroke-width', '0.9');
          label.style.pointerEvents = 'none';
          label.textContent = name;
          userWaypointLayer.appendChild(label);

          if (isSelected) {
            const ring = makeNode('circle');
            ring.setAttribute('cx', p.x.toFixed(1));
            ring.setAttribute('cy', p.y.toFixed(1));
            ring.setAttribute('r', '18.5');
            ring.setAttribute('fill', 'none');
            ring.setAttribute('stroke', '#c94444');
            ring.setAttribute('stroke-width', '3.0');
            ring.style.pointerEvents = 'none';
            userWaypointLayer.appendChild(ring);

            selectedAssetInfo.x = p.x;
            selectedAssetInfo.y = p.y;
            selectedAssetInfo.props = {
              category: 'USER_WAYPOINT',
              typeName: 'USER WAYPOINT',
              latLonText: String(formatEfbSaUserWaypointLatLon(theatreForWaypoints, wp) || '').trim(),
              mgrsText: String(formatEfbSaUserWaypointMgrs(theatreForWaypoints, wp) || '').toUpperCase().trim(),
              xNum: (wp && wp.xNum !== null && wp.xNum !== undefined && String(wp.xNum) !== '') ? Number(wp.xNum) : NaN,
              yNum: (wp && wp.yNum !== null && wp.yNum !== undefined && String(wp.yNum) !== '') ? Number(wp.yNum) : NaN,
            };
          }
        });
        overlaySvg.appendChild(userWaypointLayer);
      }

      setEfbSaUserWaypointDiag({
        lastRenderedCount: renderedUserWaypointCount,
        lastSelectedAssetKey: String(selectedAssetKey || '')
      });

      if (selectedAssetInfo.props) {
        const p = selectedAssetInfo.props;
        function formatFreq(v) {
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
        if (category === 'JTAC_TARGET') {
          const jtacLines = formatJtacNineLinePopupLines(p);
          jtacLines.forEach(function (x) { infoLines.push(String(x || '')); });

          const jtacLatLonText = (function () {
            const direct = String(p.latLonText || '').trim();
            if (direct) return direct;
            const xNum = Number(p.xNum);
            const yNum = Number(p.yNum);
            if (!isFinite(xNum) || !isFinite(yNum) || !theatreForWaypoints) return '';
            const llPrimary = convertDcsXYToLatLon(theatreForWaypoints, xNum, yNum);
            const llSwap = convertDcsXYToLatLon(theatreForWaypoints, yNum, xNum);
            const ll = (llPrimary && isFinite(Number(llPrimary.lat)) && isFinite(Number(llPrimary.lon)))
              ? llPrimary
              : ((llSwap && isFinite(Number(llSwap.lat)) && isFinite(Number(llSwap.lon))) ? llSwap : null);
            if (!ll) return '';
            return formatLatLonDms(Number(ll.lat), Number(ll.lon));
          })();
          if (jtacLatLonText) infoLines.push('LAT/LON: ' + jtacLatLonText);

          const jtacMgrsText = (function () {
            const direct = String(p.mgrsText || '').toUpperCase().trim();
            if (direct) return direct;
            const xNum = Number(p.xNum);
            const yNum = Number(p.yNum);
            if (!isFinite(xNum) || !isFinite(yNum) || !theatreForWaypoints) return '';
            const llPrimary = convertDcsXYToLatLon(theatreForWaypoints, xNum, yNum);
            const llSwap = convertDcsXYToLatLon(theatreForWaypoints, yNum, xNum);
            const ll = (llPrimary && isFinite(Number(llPrimary.lat)) && isFinite(Number(llPrimary.lon)))
              ? llPrimary
              : ((llSwap && isFinite(Number(llSwap.lat)) && isFinite(Number(llSwap.lon))) ? llSwap : null);
            if (!ll) return '';
            return String(formatLatLonMgrs(Number(ll.lat), Number(ll.lon)) || '').toUpperCase().trim();
          })();
          if (jtacMgrsText) infoLines.push('MGRS: ' + jtacMgrsText);
        }
        const typeText = String(p.typeName || '').trim();
        const freqText = formatFreq(p.frequency);
        const altFreqs = Array.isArray(p.altFrequencies) ? p.altFrequencies : [];
        const allFreqs = [p.frequency].concat(altFreqs);
        const normalizedFreqs = allFreqs
          .map(function (v) { return formatFreq(v); })
          .filter(function (v) { return !!v && isFinite(Number(v)); })
          .map(function (v) { return { text: v, mhz: Number(v) }; });
        const uhf = normalizedFreqs.find(function (f) { return f.mhz >= 225 && f.mhz <= 399.975; });
        const vhf = normalizedFreqs.find(function (f) { return f.mhz >= 30 && f.mhz < 225; });
        const tacanText = String(p.tacan || '').trim();
        const mpText = String(p.mpClientCallsign || '').trim();
        if (category !== 'JTAC_TARGET') {
          if (typeText) infoLines.push('TYPE: ' + typeText);
          const latLonText = String(p.latLonText || '').trim();
          if (latLonText) infoLines.push('LAT/LON: ' + latLonText);
          const mgrsText = String(p.mgrsText || '').trim();
          if (mgrsText) infoLines.push('MGRS: ' + mgrsText);
          const elevFeet = Number(p.elevationFeet);
          if (isFinite(elevFeet)) infoLines.push('ELEV: ' + String(Math.round(elevFeet)) + ' FT');
          const friendlyBearing = Number(p.friendlyBearing);
          const friendlyRangeNm = Number(p.friendlyRangeNm);
          if (isFinite(friendlyBearing) && isFinite(friendlyRangeNm) && friendlyRangeNm > 0) {
            infoLines.push('FRND: ' + formatHeadingDeg(friendlyBearing) + '/' + String(Number(friendlyRangeNm).toFixed(1)) + 'NM');
          }
        }
        if (category === 'ATC') {
          if (uhf && uhf.text) infoLines.push('UHF: ' + uhf.text);
          if (vhf && vhf.text) infoLines.push('VHF: ' + vhf.text);
        } else if (freqText) infoLines.push('FREQ: ' + freqText);
        if (tacanText) infoLines.push('TACAN: ' + tacanText);
        if (mpText) infoLines.push('MP: ' + mpText);

        if (infoLines.length) {
          const fontSize = 10;
          const lineHeight = 12;
          const padX = 5;
          const padY = 4;
          const maxChars = infoLines.reduce(function (max, line) { return Math.max(max, String(line || '').length); }, 0);
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

          infoLines.forEach(function (line, idx) {
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
      if (previousLayer && previousLayer !== renderLayer) {
        previousLayer.style.display = 'none';
        if (typeof window !== 'undefined' && typeof window.requestAnimationFrame === 'function') {
          window.requestAnimationFrame(function () {
            if (previousLayer.parentNode === overlayRoot) {
              overlayRoot.removeChild(previousLayer);
            }
          });
        } else if (previousLayer.parentNode === overlayRoot) {
          overlayRoot.removeChild(previousLayer);
        }
      }

      if (!item.overlayClickBound) {
        let lastAssetBugEventMs = 0;
        let longPressState = null;

        function clearLongPressState() {
          if (!longPressState) return;
          if (longPressState.timerId) {
            clearTimeout(longPressState.timerId);
          }
          longPressState = null;
        }

        function beginLongPressDetection(ev) {
          if (!ev) return;
          if (ev.pointerType === 'mouse' && ev.button !== 0) return;
          const rawTarget = getEventElementTarget(ev);
          if (rawTarget && rawTarget.closest && rawTarget.closest('.maplibregl-ctrl, .maplibregl-control-container, .maplibregl-marker')) return;
          const selectedKey = item.selectedKey;
          if (!selectedKey) return;

          const key = getFlightPlanEtaStartKey(selectedKey);
          if (!key) return;

          const pointerId = (typeof ev.pointerId === 'number') ? ev.pointerId : 0;
          const startX = Number(ev.clientX);
          const startY = Number(ev.clientY);
          const assetNode = rawTarget && rawTarget.closest ? rawTarget.closest('[data-asset-key]') : null;
          const targetAssetKey = assetNode && assetNode.getAttribute
            ? String(assetNode.getAttribute('data-asset-key') || '').trim()
            : '';

          longPressState = {
            pointerId: pointerId,
            startX: startX,
            startY: startY,
            moved: false,
            fired: false,
            targetAssetKey: targetAssetKey,
            timerId: 0,
          };

          longPressState.timerId = setTimeout(function () {
            if (!longPressState || longPressState.fired || longPressState.moved) return;
            longPressState.fired = true;

            const current = longPressState;
            const userWaypointId = parseEfbSaUserWaypointAssetKey(current.targetAssetKey);
            if (userWaypointId) {
              setMapSelectedAssetKeyBySelection(selectedKey, buildEfbSaUserWaypointAssetKey(userWaypointId));
              openEfbDrawer('user-waypoints');
              efbUiDirty = true;
              if (latestData) render(latestData);
              return;
            }

            const rect = host.getBoundingClientRect ? host.getBoundingClientRect() : null;
            if (!rect || !isFinite(rect.left) || !isFinite(rect.top)) return;
            const px = current.startX - rect.left;
            const py = current.startY - rect.top;
            if (!isFinite(px) || !isFinite(py)) return;

            let lonLat = null;
            try {
              lonLat = map.unproject([px, py]);
            } catch (_) {
              lonLat = null;
            }
            if (!lonLat) return;

            const lon = Number(lonLat.lng);
            const lat = Number(lonLat.lat);
            if (!isFinite(lat) || !isFinite(lon)) return;

            const model = latestData || {};
            const theater = String(resolveOpenFreeMapFallbackTheatreText(model) || '').trim();
            const dcsPoint = convertLatLonToDcsXY(theater, lat, lon);
            const created = createEfbSaUserWaypoint(
              dcsPoint && isFinite(Number(dcsPoint.xNum)) ? Number(dcsPoint.xNum) : NaN,
              dcsPoint && isFinite(Number(dcsPoint.yNum)) ? Number(dcsPoint.yNum) : NaN,
              lat,
              lon);
            if (!created) return;
            setEfbSaUserWaypointDiag({
              lastAction: 'add',
              lastAddLatLon: Number(lat).toFixed(5) + ',' + Number(lon).toFixed(5),
              lastAddXY: ((created.xNum !== null && created.xNum !== undefined) && (created.yNum !== null && created.yNum !== undefined))
                ? (String(Math.round(Number(created.xNum))) + '/' + String(Math.round(Number(created.yNum))))
                : 'null/null'
            });

            renderOpenFreeMapOverlay(item, item.payload || { features: [] });
            const fallback = host.querySelector('.fltPlanOpenMapFallback');
            if (fallback) fallback.style.display = 'none';

            const assetKey = buildEfbSaUserWaypointAssetKey(created.id);
            setMapSelectedAssetKeyBySelection(selectedKey, assetKey);
            openEfbDrawer('user-waypoints');
            efbUiDirty = true;
            if (latestData) render(latestData);
          }, efbSaUserWaypointHoldMs);
        }

        function handleOverlayAssetBugEvent(ev, immediate) {
          let node = ev.target;
          while (node && node !== overlayRoot) {
            const assetKey = node.getAttribute ? String(node.getAttribute('data-asset-key') || '') : '';
            if (assetKey) {
              const nowMs = Date.now();
              if (!immediate && (nowMs - lastAssetBugEventMs) < 250) {
                ev.preventDefault();
                return;
              }
              lastAssetBugEventMs = nowMs;

              const suppressKey = getFlightPlanEtaStartKey(item.selectedKey);
              let resolvedAssetKey = assetKey;
              try {
                const payload = item.payload || {};
                const feats = Array.isArray(payload.features) ? payload.features : [];
                const selectedFeature = feats.find(function (f) {
                  const p = (f && f.properties) || {};
                  return String(p.assetKey || '') === String(assetKey || '');
                }) || null;
                const sp = selectedFeature ? (selectedFeature.properties || {}) : {};
                const selectedIsJtacHistory = String(sp.kind || '').toLowerCase() === 'jtac-target' && !!sp.isJtacHistory;
                const selectedHasNineLine = !!String(sp.nl4Text || sp.nl5Text || sp.nl6Text || sp.nl7Text || sp.nl8Text || sp.nl9Text || sp.rmkText || '').trim();
                if (selectedIsJtacHistory && !selectedHasNineLine) {
                  const sx = Number(sp.xNum);
                  const sy = Number(sp.yNum);
                  const activeFeature = feats.find(function (f) {
                    const p = (f && f.properties) || {};
                    if (String(p.kind || '').toLowerCase() !== 'jtac-target') return false;
                    if (p.isJtacHistory) return false;
                    const ax = Number(p.xNum);
                    const ay = Number(p.yNum);
                    if (!isFinite(ax) || !isFinite(ay) || !isFinite(sx) || !isFinite(sy)) return false;
                    return Math.abs(ax - sx) <= 10 && Math.abs(ay - sy) <= 10;
                  }) || null;
                  if (activeFeature && activeFeature.properties && activeFeature.properties.assetKey) {
                    resolvedAssetKey = String(activeFeature.properties.assetKey);
                  }
                }
              } catch (_) { }

              const current = getMapSelectedAssetKeyBySelection(item.selectedKey);
              const nextKey = current === resolvedAssetKey ? '' : resolvedAssetKey;

              let buggedAirfieldIcao = '';
              try {
                buggedAirfieldIcao = String(resolveMapSelectedAirfieldIcao(latestData, nextKey) || '').toUpperCase().trim();
              } catch (_) {
                buggedAirfieldIcao = '';
              }

              if (suppressKey && efbSaFollowOwnshipEnabled) {
                if (buggedAirfieldIcao) {
                  efbSaFollowSuppressUntilBySelection[suppressKey] = 0;
                } else {
                  efbSaFollowSuppressUntilBySelection[suppressKey] = Date.now() + 1200;
                }
              }

              setMapSelectedAssetKeyBySelection(item.selectedKey, nextKey);
              setEfbDebugState({
                source: 'sa-map-click-openfreemap',
                saAssetKey: String(nextKey || assetKey || ''),
                finalAirportKey: String(getEfbAirportKey(latestData) || 'UNSET'),
              });
              if (nextKey) {
                tryPreloadEfbForBuggedAirfield(latestData, nextKey).catch(function () { });
              }
              if (latestData) {
                const selectedAirfieldIcao = String(resolveMapSelectedAirfieldIcao(latestData, nextKey) || '').toUpperCase().trim();
                const inSaMapMode = selectedTab === 'EFB' && normalizeEfbViewerMode(efbViewerMode) === 'sa-map';
                if (inSaMapMode && selectedAirfieldIcao) {
                  if (item && item.selectedKey) {
                    applyOpenFreeMapOwnshipCamera(item.selectedKey, latestData);
                  }
                } else {
                  render(latestData);
                }
              }
              ev.preventDefault();
              if (typeof ev.stopPropagation === 'function') {
                ev.stopPropagation();
              }
              return;
            }
            node = node.parentNode;
          }
        }

        function onPointerDown(ev) {
          beginLongPressDetection(ev);
          handleOverlayAssetBugEvent(ev, true);
        }

        overlayRoot.addEventListener('pointerdown', onPointerDown);
        host.addEventListener('pointerdown', onPointerDown);

        function onPointerMove(ev) {
          if (!longPressState) return;
          const pointerId = (typeof ev.pointerId === 'number') ? ev.pointerId : 0;
          if (pointerId !== longPressState.pointerId) return;
          const dx = Number(ev.clientX) - Number(longPressState.startX);
          const dy = Number(ev.clientY) - Number(longPressState.startY);
          if (!isFinite(dx) || !isFinite(dy)) return;
          if ((dx * dx) + (dy * dy) > (efbSaUserWaypointHoldMovePx * efbSaUserWaypointHoldMovePx)) {
            longPressState.moved = true;
            clearLongPressState();
          }
        }

        overlayRoot.addEventListener('pointermove', onPointerMove);
        host.addEventListener('pointermove', onPointerMove);

        function onPointerDone() {
          clearLongPressState();
        }

        overlayRoot.addEventListener('pointerup', onPointerDone);
        overlayRoot.addEventListener('pointercancel', onPointerDone);
        overlayRoot.addEventListener('pointerleave', onPointerDone);
        host.addEventListener('pointerup', onPointerDone);
        host.addEventListener('pointercancel', onPointerDone);
        host.addEventListener('pointerleave', onPointerDone);

        overlayRoot.addEventListener('click', function (ev) {
          if (longPressState && longPressState.fired) {
            clearLongPressState();
            ev.preventDefault();
            if (typeof ev.stopPropagation === 'function') ev.stopPropagation();
            return;
          }
          handleOverlayAssetBugEvent(ev, false);
        });
        item.overlayClickBound = true;
      }
    }

    function disposeOpenFreeMapInstances() {
      const ids = Object.keys(openFreeMapInstancesByContainerId);
      ids.forEach(function (id) {
        const item = openFreeMapInstancesByContainerId[id];
        if (!item || !item.map) return;
        try { item.map.remove(); } catch (_) { }
      });
      openFreeMapInstancesByContainerId = {};
      openFreeMapContainerIdBySelectionKey = {};
      openFreeMapPayloadById = {};
    }

    function ensureOpenFreeMapRuntime(done) {
      if (typeof done !== 'function') return;
      if (openFreeMapRuntimeReady && window.maplibregl) {
        done(true);
        return;
      }

      const nowMs = Date.now();
      if (openFreeMapRuntimeFailed && (nowMs - openFreeMapRuntimeLastAttemptMs) > 30000) {
        openFreeMapRuntimeFailed = false;
      }

      if (openFreeMapRuntimeFailed) {
        done(false);
        return;
      }

      openFreeMapRuntimeWaiters.push(done);
      if (openFreeMapRuntimeLoading) return;

      openFreeMapRuntimeLoading = true;
      openFreeMapRuntimeLastAttemptMs = nowMs;

      function flush(ok) {
        const waiters = openFreeMapRuntimeWaiters.slice();
        openFreeMapRuntimeWaiters = [];
        waiters.forEach(function (cb) {
          try { cb(!!ok); } catch (_) { }
        });
      }

      function fail(reason) {
        openFreeMapRuntimeReady = false;
        openFreeMapRuntimeLoading = false;
        openFreeMapRuntimeFailed = true;
        openFreeMapRuntimeErrorText = String(reason || 'Map runtime failed to load.');
        flush(false);
      }

      const cssId = 'vaicom-ofm-maplibre-css';
      if (!document.getElementById(cssId)) {
        const css = document.createElement('link');
        css.id = cssId;
        css.rel = 'stylesheet';
        css.href = 'https://unpkg.com/maplibre-gl@4.7.1/dist/maplibre-gl.css';
        css.onerror = function () {
          const css2 = document.createElement('link');
          css2.rel = 'stylesheet';
          css2.href = 'https://cdn.jsdelivr.net/npm/maplibre-gl@4.7.1/dist/maplibre-gl.css';
          document.head.appendChild(css2);
        };
        document.head.appendChild(css);
      }

      if (window.maplibregl) {
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

      function tryLoad(index) {
        if (index >= scriptUrls.length) {
          fail('Unable to load MapLibre runtime from CDN.');
          return;
        }
        const script = document.createElement('script');
        script.src = scriptUrls[index];
        script.async = true;
        script.onload = function () {
          openFreeMapRuntimeReady = !!window.maplibregl;
          if (!openFreeMapRuntimeReady) {
            tryLoad(index + 1);
            return;
          }
          openFreeMapRuntimeLoading = false;
          openFreeMapRuntimeErrorText = '';
          flush(true);
        };
        script.onerror = function () {
          tryLoad(index + 1);
        };
        document.head.appendChild(script);
      }

      tryLoad(0);
    }

    function handleOpenFreeMapZoomAction(selected, action) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return false;
      const containerId = String(openFreeMapContainerIdBySelectionKey[key] || '');
      if (!containerId) return false;
      const item = openFreeMapInstancesByContainerId[containerId];
      if (!item || !item.map) return false;

      const map = item.map;
      const mode = String(action || '').toLowerCase();

      function resolveOwnshipLonLat() {
        try {
          const payload = item.payload || {};
          const features = Array.isArray(payload.features) ? payload.features : [];
          const ownshipFeature = features.find(function (f) {
            const props = (f && f.properties) || {};
            const geom = (f && f.geometry) || {};
            const c = Array.isArray(geom.coordinates) ? geom.coordinates : [];
            return String(props.kind || '').toLowerCase() === 'player'
              && c.length >= 2
              && isFinite(Number(c[0]))
              && isFinite(Number(c[1]));
          });
          if (ownshipFeature) {
            return {
              lon: Number(ownshipFeature.geometry.coordinates[0]),
              lat: Number(ownshipFeature.geometry.coordinates[1]),
            };
          }

          const model = latestData || {};
          const ownship = getPlayerMapPoint(model);
          if (!ownship || !isFinite(Number(ownship.xNum)) || !isFinite(Number(ownship.yNum))) return null;
          const theater = String(resolveOpenFreeMapFallbackTheatreText(model) || '').trim();
          const llPrimary = convertDcsXYToLatLon(theater, Number(ownship.xNum), Number(ownship.yNum));
          const llSwap = convertDcsXYToLatLon(theater, Number(ownship.yNum), Number(ownship.xNum));
          const ll = (llPrimary && isFinite(Number(llPrimary.lon)) && isFinite(Number(llPrimary.lat)))
            ? llPrimary
            : ((llSwap && isFinite(Number(llSwap.lon)) && isFinite(Number(llSwap.lat))) ? llSwap : null);
          if (!ll) return null;
          return { lon: Number(ll.lon), lat: Number(ll.lat) };
        } catch (_) {
          return null;
        }
      }

      if (mode === 'in') {
        if (efbSaFollowOwnshipEnabled) {
          const ownship = resolveOwnshipLonLat();
          if (ownship) {
            map.easeTo({
              zoom: Number(map.getZoom()) + 1,
              around: [ownship.lon, ownship.lat],
              duration: 0,
            });
            return true;
          }
        }
        map.zoomIn({ duration: 0 });
        return true;
      }
      if (mode === 'out') {
        if (efbSaFollowOwnshipEnabled) {
          const ownship = resolveOwnshipLonLat();
          if (ownship) {
            map.easeTo({
              zoom: Number(map.getZoom()) - 1,
              around: [ownship.lon, ownship.lat],
              duration: 0,
            });
            return true;
          }
        }
        map.zoomOut({ duration: 0 });
        return true;
      }
      if (mode === 'reset') {
        const stateReset = getOpenFreeMapViewBySelection(selected);
        stateReset.adQuickZoomActive = false;
        const bounds = item.bounds;
        if (bounds && bounds.length === 4) {
          map.fitBounds([[bounds[0], bounds[1]], [bounds[2], bounds[3]]], { padding: 36, duration: 0, maxZoom: 11 });
          return true;
        }
      }
      if (mode === 'ad') {
        const state = getOpenFreeMapViewBySelection(selected);
        const wasActive = !!state.adQuickZoomActive;
        if (wasActive && isFinite(Number(state.adQuickZoomPrevZoom))) {
          const previousZoom = Number(state.adQuickZoomPrevZoom);
          if (efbSaFollowOwnshipEnabled) {
            const ownship = resolveOwnshipLonLat();
            if (ownship) {
              map.easeTo({
                zoom: previousZoom,
                around: [Number(ownship.lon), Number(ownship.lat)],
                duration: 0,
              });
            } else {
              map.easeTo({ zoom: previousZoom, duration: 0 });
            }
          } else {
            map.easeTo({ zoom: previousZoom, duration: 0 });
          }
          state.adQuickZoomActive = false;
          state.zoom = previousZoom;
          return true;
        }

        const currentCenter = map.getCenter ? map.getCenter() : null;
        const ownship = resolveOwnshipLonLat();
        const targetLon = ownship && isFinite(Number(ownship.lon))
          ? Number(ownship.lon)
          : (currentCenter && isFinite(Number(currentCenter.lng)) ? Number(currentCenter.lng) : NaN);
        const targetLat = ownship && isFinite(Number(ownship.lat))
          ? Number(ownship.lat)
          : (currentCenter && isFinite(Number(currentCenter.lat)) ? Number(currentCenter.lat) : NaN);
        const targetZoom = 14.5;
        if (!isFinite(targetLon) || !isFinite(targetLat)) return false;

        state.adQuickZoomPrevCenterLon = currentCenter && isFinite(Number(currentCenter.lng)) ? Number(currentCenter.lng) : NaN;
        state.adQuickZoomPrevCenterLat = currentCenter && isFinite(Number(currentCenter.lat)) ? Number(currentCenter.lat) : NaN;
        state.adQuickZoomPrevZoom = Number(map.getZoom());
        state.adQuickZoomPrevBearing = Number(map.getBearing ? map.getBearing() : 0);
        state.adQuickZoomActive = true;

        map.easeTo({
          center: [targetLon, targetLat],
          zoom: targetZoom,
          duration: 0,
          pitch: 0,
        });
        return true;
      }
      return false;
    }

    function handleOpenFreeMapCenterOwnship(selected, data) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return false;
      const containerId = String(openFreeMapContainerIdBySelectionKey[key] || '');
      if (!containerId) return false;
      const item = openFreeMapInstancesByContainerId[containerId];
      if (!item || !item.map) return false;

      const map = item.map;
      const payload = item.payload || {};
      const features = Array.isArray(payload.features) ? payload.features : [];
      const ownshipFeature = features.find(function (f) {
        const props = (f && f.properties) || {};
        const geom = (f && f.geometry) || {};
        const c = Array.isArray(geom.coordinates) ? geom.coordinates : [];
        return String(props.kind || '').toLowerCase() === 'player'
          && c.length >= 2
          && isFinite(Number(c[0]))
          && isFinite(Number(c[1]));
      });

      let centerLon = NaN;
      let centerLat = NaN;
      if (ownshipFeature) {
        centerLon = Number(ownshipFeature.geometry.coordinates[0]);
        centerLat = Number(ownshipFeature.geometry.coordinates[1]);
      } else {
        const model = data || latestData || {};
        const ownship = getPlayerMapPoint(model);
        if (!ownship || !isFinite(Number(ownship.xNum)) || !isFinite(Number(ownship.yNum))) return false;
        const theater = String(resolveOpenFreeMapFallbackTheatreText(model) || '').trim();
        const llPrimary = convertDcsXYToLatLon(theater, Number(ownship.xNum), Number(ownship.yNum));
        const llSwap = convertDcsXYToLatLon(theater, Number(ownship.yNum), Number(ownship.xNum));
        const ll = (llPrimary && isFinite(Number(llPrimary.lon)) && isFinite(Number(llPrimary.lat)))
          ? llPrimary
          : ((llSwap && isFinite(Number(llSwap.lon)) && isFinite(Number(llSwap.lat))) ? llSwap : null);
        if (!ll) return false;
        centerLon = Number(ll.lon);
        centerLat = Number(ll.lat);
      }

      if (!isFinite(centerLon) || !isFinite(centerLat)) return false;
      map.easeTo({ center: [centerLon, centerLat], duration: 0 });

      const state = getOpenFreeMapViewBySelection(selected);
      state.centerLon = centerLon;
      state.centerLat = centerLat;
      state.zoom = Number(map.getZoom());
      map.easeTo({ pitch: 0, duration: 0 });
      return true;
    }

    function handleOpenFreeMapCenterUserWaypoint(selected, waypoint, data) {
      if (!waypoint) return false;
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return false;
      const containerId = String(openFreeMapContainerIdBySelectionKey[key] || '');
      if (!containerId) return false;
      const item = openFreeMapInstancesByContainerId[containerId];
      if (!item || !item.map) return false;

      const model = data || latestData || {};
      const theater = String(resolveOpenFreeMapFallbackTheatreText(model) || '').trim();
      const ll = resolveEfbSaUserWaypointLonLat(theater, waypoint);
      if (!ll) return false;

      const map = item.map;
      map.easeTo({ center: [Number(ll.lon), Number(ll.lat)], duration: 0 });
      const state = getOpenFreeMapViewBySelection(selected);
      state.centerLon = Number(ll.lon);
      state.centerLat = Number(ll.lat);
      state.zoom = Number(map.getZoom());
      return true;
    }

    function applyOpenFreeMapOwnshipCamera(selected, data) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key || !efbSaFollowOwnshipEnabled) return false;

      const suppressUntil = Number(efbSaFollowSuppressUntilBySelection[key] || 0);
      if (isFinite(suppressUntil) && suppressUntil > Date.now()) return false;

      const nowMs = Date.now();
      const lastApplyMs = Number(efbSaLastCameraApplyMsBySelection[key] || 0);
      if (isFinite(lastApplyMs) && (nowMs - lastApplyMs) < 40) return false;

      const containerId = String(openFreeMapContainerIdBySelectionKey[key] || '');
      if (!containerId) return false;
      const item = openFreeMapInstancesByContainerId[containerId];
      if (!item || !item.map || !item.payload) return false;

      const map = item.map;
      const payload = item.payload || {};
      const features = Array.isArray(payload.features) ? payload.features : [];
      const ownshipFeature = features.find(function (f) {
        const props = (f && f.properties) || {};
        const geom = (f && f.geometry) || {};
        const c = Array.isArray(geom.coordinates) ? geom.coordinates : [];
        return String(props.kind || '').toLowerCase() === 'player'
          && c.length >= 2
          && isFinite(Number(c[0]))
          && isFinite(Number(c[1]));
      });

      let centerLon = NaN;
      let centerLat = NaN;
      let motionSourceLon = NaN;
      let motionSourceLat = NaN;
      if (ownshipFeature) {
        centerLon = Number(ownshipFeature.geometry.coordinates[0]);
        centerLat = Number(ownshipFeature.geometry.coordinates[1]);
        motionSourceLon = centerLon;
        motionSourceLat = centerLat;
      } else {
        const model = data || latestData || {};
        const ownship = getPlayerMapPoint(model);
        if (!ownship || !isFinite(Number(ownship.xNum)) || !isFinite(Number(ownship.yNum))) return false;
        const theater = String(resolveOpenFreeMapFallbackTheatreText(model) || '').trim();
        const llPrimary = convertDcsXYToLatLon(theater, Number(ownship.xNum), Number(ownship.yNum));
        const llSwap = convertDcsXYToLatLon(theater, Number(ownship.yNum), Number(ownship.xNum));
        const ll = (llPrimary && isFinite(Number(llPrimary.lon)) && isFinite(Number(llPrimary.lat)))
          ? llPrimary
          : ((llSwap && isFinite(Number(llSwap.lon)) && isFinite(Number(llSwap.lat))) ? llSwap : null);
        if (!ll) return false;
        centerLon = Number(ll.lon);
        centerLat = Number(ll.lat);
        motionSourceLon = centerLon;
        motionSourceLat = centerLat;
      }

      if (!isFinite(centerLon) || !isFinite(centerLat)) return false;

      const motionState = efbSaOwnshipMotionBySelection[key] || { lon: NaN, lat: NaN, vx: 0, vy: 0, t: 0, srcT: 0 };
      const prevLon = Number(motionState.lon);
      const prevLat = Number(motionState.lat);
      const prevT = Number(motionState.t);
      const sourceChanged = isFinite(prevLon) && isFinite(prevLat)
        ? (Math.abs(motionSourceLon - prevLon) > 0.0000001 || Math.abs(motionSourceLat - prevLat) > 0.0000001)
        : true;
      if (sourceChanged && isFinite(prevLon) && isFinite(prevLat) && isFinite(prevT) && nowMs > prevT) {
        const dtSec = (nowMs - prevT) / 1000.0;
        if (dtSec > 0.02 && dtSec < 5.0) {
          motionState.vx = (motionSourceLon - prevLon) / dtSec;
          motionState.vy = (motionSourceLat - prevLat) / dtSec;
        }
      }
      if (sourceChanged) {
        motionState.lon = motionSourceLon;
        motionState.lat = motionSourceLat;
        motionState.srcT = nowMs;
      }
      motionState.t = nowMs;
      efbSaOwnshipMotionBySelection[key] = motionState;

      const staleDataMs = nowMs - Number(motionState.srcT || nowMs);
      if (isFinite(staleDataMs) && staleDataMs > 60 && staleDataMs < 900) {
        const dtPredSec = staleDataMs / 1000.0;
        centerLon = Number(motionState.lon) + ((Number(motionState.vx) || 0) * dtPredSec);
        centerLat = Number(motionState.lat) + ((Number(motionState.vy) || 0) * dtPredSec);
      }

      const anchorMode = normalizeEfbSaAnchorMode(efbSaAnchorMode);

      let bearing = Number(map.getBearing ? map.getBearing() : 0);
      if (efbSaTrackUpEnabled) {
        const model = data || latestData || {};
        const ownshipPoint = getPlayerMapPoint(model);
        const heading = getOwnshipHeadingDeg(model, ownshipPoint);
        if (isFinite(Number(heading))) {
          bearing = normalizeHeadingDeg(Number(heading));
        }
      } else {
        bearing = 0;
      }

      map.easeTo({
        center: [centerLon, centerLat],
        bearing: bearing,
        pitch: 0,
        offset: anchorMode === 'lower-third' ? [0, (map.getContainer && map.getContainer() ? Math.round((map.getContainer().clientHeight || 0) * 0.166) : 120)] : [0, 0],
        duration: 0,
      });

      const state = getOpenFreeMapViewBySelection(selected);
      state.centerLon = centerLon;
      state.centerLat = centerLat;
      state.zoom = Number(map.getZoom());
      efbSaLastCameraApplyMsBySelection[key] = nowMs;
      return true;
    }

    function tickEfbSaOwnshipCamera() {
      if (efbSaCameraTickerId) return;
      efbSaCameraTickerId = window.setInterval(function () {
        if (selectedTab !== 'EFB') return;
        if (normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') return;
        if (!efbSaFollowOwnshipEnabled) return;
        const context = resolveActiveSaMapContext(latestData || null);
        if (!context || !context.selected) return;
        applyOpenFreeMapOwnshipCamera(context.selected, latestData || null);
      }, 66);
    }

    function initializeOpenFreeMapInstances() {
      const hosts = Array.from(document.querySelectorAll('[data-openfreemap-map-id]'));
      if (!hosts.length) return;

      function shouldUseNavigraphSaTiles() {
        try {
          const model = latestData || {};
          const efb = (model && model.Efb) || {};
          return !!(efb && efb.AuthPresent);
        } catch (_) {
          return false;
        }
      }

      function buildOpenFreeMapStyleSpec() {
        if (!shouldUseNavigraphSaTiles()) {
          return nightModeEnabled
            ? 'https://tiles.openfreemap.org/styles/dark'
            : 'https://tiles.openfreemap.org/styles/liberty';
        }

        const mode = nightModeEnabled ? 'night' : 'day';
        const layer = normalizeEfbSaNavigraphLayer(efbSaNavigraphLayer);
        const tilesUrl = '/okb/efb/navtile?z={z}&x={x}&y={y}&mode=' + mode + '&layer=' + encodeURIComponent(layer);
        return {
          version: 8,
          name: 'navigraph-' + layer + '-' + mode,
          sources: {
            navigraphVfr: {
              type: 'raster',
              tiles: [tilesUrl],
              tileSize: 256,
              minzoom: 0,
              maxzoom: 18,
            }
          },
          layers: [
            {
              id: 'navigraph-vfr-layer',
              type: 'raster',
              source: 'navigraphVfr',
              minzoom: 0,
              maxzoom: 22,
            }
          ]
        };
      }

      ensureOpenFreeMapRuntime(function (ok) {
        hosts.forEach(function (host) {
          const mapId = String(host.getAttribute('data-openfreemap-map-id') || '');
          if (!mapId) return;
          if (openFreeMapInstancesByContainerId[mapId]) return;

          const wrap = host.closest ? host.closest('[data-openfreemap-wrap]') : null;
          const fallback = wrap ? wrap.querySelector('.fltPlanOpenMapFallback') : null;
          if (fallback) fallback.style.display = 'none';
          const entry = openFreeMapPayloadById[mapId];
          const payload = entry && entry.payload ? entry.payload : null;

          if (!ok || !payload || !isFinite(Number(payload.centerLon)) || !isFinite(Number(payload.centerLat)) || !window.maplibregl) {
            if (fallback) fallback.style.display = 'block';
            return;
          }

          host.innerHTML = '';

          try {
            const selectedKey = String((entry && entry.selectedKey) || host.getAttribute('data-openfreemap-selection') || '');
            const bounds = Array.isArray(payload.bounds) ? payload.bounds.slice(0, 4) : null;
            const viewState = getOpenFreeMapViewBySelection(selectedKey);
            const hasStoredView = isFinite(Number(viewState.centerLon)) && isFinite(Number(viewState.centerLat)) && isFinite(Number(viewState.zoom));

            let initialCenterLon = hasStoredView ? Number(viewState.centerLon) : Number(payload.centerLon || 0);
            let initialCenterLat = hasStoredView ? Number(viewState.centerLat) : Number(payload.centerLat || 0);
            let initialBearing = isFinite(Number(viewState.bearing)) ? Number(viewState.bearing) : 0;

            if (selectedTab === 'EFB' && normalizeEfbViewerMode(efbViewerMode) === 'sa-map' && efbSaFollowOwnshipEnabled) {
              const model = latestData || null;
              const ownship = getPlayerMapPoint(model);
              if (ownship && isFinite(Number(ownship.xNum)) && isFinite(Number(ownship.yNum))) {
                const theater = String(resolveOpenFreeMapFallbackTheatreText(model) || '').trim();
                const llPrimary = convertDcsXYToLatLon(theater, Number(ownship.xNum), Number(ownship.yNum));
                const llSwap = convertDcsXYToLatLon(theater, Number(ownship.yNum), Number(ownship.xNum));
                const ll = (llPrimary && isFinite(Number(llPrimary.lon)) && isFinite(Number(llPrimary.lat)))
                  ? llPrimary
                  : ((llSwap && isFinite(Number(llSwap.lon)) && isFinite(Number(llSwap.lat))) ? llSwap : null);
                if (ll) {
                  initialCenterLon = Number(ll.lon);
                  initialCenterLat = Number(ll.lat);
                }

                if (efbSaTrackUpEnabled) {
                  const heading = getOwnshipHeadingDeg(model, ownship);
                  if (isFinite(Number(heading))) {
                    initialBearing = normalizeHeadingDeg(Number(heading));
                  }
                } else {
                  initialBearing = 0;
                }
              }
            }

            const defaultMapZoom = 6;
            const doghouseStartupZoom = (efbSaShowDoghouses !== false)
              ? Math.max(defaultMapZoom, Number(efbSaDoghouseMinZoom) + 0.2)
              : defaultMapZoom;
            const initialZoom = hasStoredView
              ? Number(viewState.zoom)
              : ((selectedTab === 'EFB' && normalizeEfbViewerMode(efbViewerMode) === 'sa-map' && efbSaFollowOwnshipEnabled)
                ? doghouseStartupZoom
                : defaultMapZoom);

            const map = new maplibregl.Map({
              container: host,
              style: buildOpenFreeMapStyleSpec(),
              center: [initialCenterLon, initialCenterLat],
              zoom: initialZoom,
              bearing: initialBearing,
              attributionControl: true,
            });

            map.addControl(new maplibregl.NavigationControl({ showCompass: false }), 'top-right');

            map.on('error', function () {
              openFreeMapLastPayloadStatus = 'map-error';
            });

            const item = {
              map: map,
              selectedKey: selectedKey,
              bounds: bounds,
              host: host,
              payload: payload,
            };

            map.on('load', function () {
              renderOpenFreeMapOverlay(item, payload);
              if (!hasStoredView && bounds && bounds.length === 4 && !efbSaFollowOwnshipEnabled) {
                map.fitBounds([[bounds[0], bounds[1]], [bounds[2], bounds[3]]], { padding: 36, duration: 0, maxZoom: 11 });
              }
              if (selectedKey && selectedTab === 'EFB' && normalizeEfbViewerMode(efbViewerMode) === 'sa-map') {
                applyOpenFreeMapOwnshipCamera(selectedKey, latestData || null);
              }
              if (fallback) fallback.style.display = 'none';
            });

            map.on('move', function () {
              const currentPayload = item.payload || payload;
              renderOpenFreeMapOverlay(item, currentPayload);
            });

            map.on('idle', function () {
              if (!selectedKey) return;
              if (selectedTab !== 'EFB') return;
              if (normalizeEfbViewerMode(efbViewerMode) !== 'sa-map') return;
              applyOpenFreeMapOwnshipCamera(selectedKey, latestData || null);
            });

            map.on('moveend', function () {
              const center = map.getCenter();
              if (!center) return;
              const state = getOpenFreeMapViewBySelection(selectedKey);
              state.centerLon = Number(center.lng);
              state.centerLat = Number(center.lat);
              state.zoom = Number(map.getZoom());
              state.bearing = Number(map.getBearing());
            });

            function suppressFollowByUserInput(delayMs) {
              const k = getFlightPlanEtaStartKey(selectedKey);
              if (!k) return;
              if (!efbSaFollowOwnshipEnabled) return;
              const delay = Math.max(250, Number(delayMs) || 0);
              efbSaFollowSuppressUntilBySelection[k] = Date.now() + delay;
            }

            map.on('mousedown', function () {
              suppressFollowByUserInput(1800);
            });

            map.on('touchstart', function () {
              suppressFollowByUserInput(1800);
            });

            map.on('dragstart', function () {
              suppressFollowByUserInput(12000);
            });

            map.on('zoomstart', function () {
              suppressFollowByUserInput(12000);
            });

            map.on('rotatestart', function () {
              suppressFollowByUserInput(12000);
            });

            openFreeMapInstancesByContainerId[mapId] = item;
            if (selectedKey) openFreeMapContainerIdBySelectionKey[selectedKey] = mapId;
          } catch (_) {
            if (fallback) fallback.style.display = 'block';
          }
        });
      });
    }

    function getMapSelectedAssetKeyBySelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return '';
      return String(fltPlanMapSelectedAssetKeyBySelection[key] || '');
    }

    function setMapSelectedAssetKeyBySelection(selected, assetKey) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      fltPlanMapSelectedAssetKeyBySelection[key] = String(assetKey || '');
    }

    function makeMapAssetSelectionKey(asset) {
      if (!asset || typeof asset !== 'object') return '';
      const callsign = String(asset.callsign || '').trim().toUpperCase();
      const name = String(asset.name || '').trim().toUpperCase();
      const category = String(asset.category || '').trim().toUpperCase();
      const x = Number(asset.xNum);
      const y = Number(asset.yNum);
      if (callsign || name) {
        return [callsign, name, category].join('|');
      }
      const xKey = isFinite(x) ? String(Math.round(x)) : '';
      const yKey = isFinite(y) ? String(Math.round(y)) : '';
      return ['POS', category, xKey, yKey].join('|');
    }

    function getPlayerMapPoint(data) {
      const fastOwnship = getFastOwnshipMapPoint(0);
      if (fastOwnship) return fastOwnship;

      const server = (data && data.Server) || {};
      const x = Number(server.PlayerPosX);
      const y = Number(server.PlayerPosY);
      if (isFinite(x) && isFinite(y)) {
        return { xNum: x, yNum: y };
      }
      const assets = getMudMapAssets(data);
      const own = assets.find(function (a) { return String((a && a.category) || '').toUpperCase() === 'PLAYER'; });
      if (own && isFinite(Number(own.xNum)) && isFinite(Number(own.yNum))) {
        return { xNum: Number(own.xNum), yNum: Number(own.yNum) };
      }
      return null;
    }

    function getFastOwnshipMapPoint(maxAgeMs) {
      if (fakeMissionEnabled) return null;
      if (!efbSaUseFastOwnshipEnabled) return null;
      if (!efbSaOwnshipFast || efbSaOwnshipFast.hasPosition !== true) return null;
      const fastX = Number(efbSaOwnshipFast.posX);
      const fastY = Number(efbSaOwnshipFast.posY);
      if (!isFinite(fastX) || !isFinite(fastY)) return null;

      const maxAge = Number(maxAgeMs);
      if (isFinite(maxAge) && maxAge > 0) {
        const minAge = Math.max(100, maxAge);
        const updatedMs = Number(efbSaOwnshipFast.updatedUtcMs || 0);
        if (updatedMs > 0) {
          const ageMs = Date.now() - updatedMs;
          if (isFinite(ageMs) && ageMs > minAge) return null;
        }
      }

      return { xNum: fastX, yNum: fastY };
    }

    function applyFastOwnshipToSaMap(selected, data) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return false;
      let containerId = String(openFreeMapContainerIdBySelectionKey[key] || '');
      let item = containerId ? openFreeMapInstancesByContainerId[containerId] : null;
      if (!item) {
        const host = document.querySelector('#efbChartViewport .fltPlanOpenMapHost[data-openfreemap-map-id]');
        const hostMapId = host ? String(host.getAttribute('data-openfreemap-map-id') || '') : '';
        if (hostMapId) {
          containerId = hostMapId;
          item = openFreeMapInstancesByContainerId[containerId] || null;
          if (item && key) {
            openFreeMapContainerIdBySelectionKey[key] = containerId;
          }
        }
      }
      if (!item || !item.map || !item.payload) return false;

      const ownship = getFastOwnshipMapPoint(0);
      if (!ownship) return false;

      const model = data || latestData || {};
      const theater = String(resolveOpenFreeMapFallbackTheatreText(model) || efbSaOwnshipFast.theater || '').trim();
      const llPrimary = convertDcsXYToLatLon(theater, Number(ownship.xNum), Number(ownship.yNum));
      const llSwap = convertDcsXYToLatLon(theater, Number(ownship.yNum), Number(ownship.xNum));
      const ll = (llPrimary && isFinite(Number(llPrimary.lon)) && isFinite(Number(llPrimary.lat)))
        ? llPrimary
        : ((llSwap && isFinite(Number(llSwap.lon)) && isFinite(Number(llSwap.lat))) ? llSwap : null);
      if (!ll) return false;

      const payload = item.payload || {};
      if (!Array.isArray(payload.features)) return false;
      const features = payload.features;
      let removedStalePlayers = false;
      const playerIndexes = [];
      for (let i = 0; i < features.length; i++) {
        const f = features[i];
        const props = (f && f.properties) || {};
        if (String(props.kind || '').toLowerCase() === 'player') {
          playerIndexes.push(i);
        }
      }

      let keepPlayerIndex = -1;
      for (let i = 0; i < playerIndexes.length; i++) {
        const idx = playerIndexes[i];
        const f = features[idx];
        const props = (f && f.properties) || {};
        const key = String(props.assetKey || '').toUpperCase();
        const cat = String(props.assetCategory || '').toUpperCase();
        const isOwnshipTagged = (cat === 'PLAYER') || key.indexOf('PLAYER') >= 0;
        if (isOwnshipTagged) {
          if (keepPlayerIndex < 0) {
            keepPlayerIndex = idx;
          } else {
            features[idx] = null;
            removedStalePlayers = true;
          }
        }
      }

      if (keepPlayerIndex < 0 && playerIndexes.length) {
        keepPlayerIndex = playerIndexes[0];
      }

      for (let i = playerIndexes.length - 1; i >= 0; i--) {
        const idx = playerIndexes[i];
        if (idx === keepPlayerIndex) continue;
        if (features[idx] !== null) {
          features[idx] = null;
          removedStalePlayers = true;
        }
      }

      if (removedStalePlayers) {
        for (let i = features.length - 1; i >= 0; i--) {
          if (!features[i]) features.splice(i, 1);
        }
      }

      let ownshipFeature = (keepPlayerIndex >= 0 && features[keepPlayerIndex])
        ? features[keepPlayerIndex]
        : features.find(function (f) {
          const props = (f && f.properties) || {};
          return String(props.kind || '').toLowerCase() === 'player';
        });

      if (!ownshipFeature) {
        ownshipFeature = {
          type: 'Feature',
          geometry: { type: 'Point', coordinates: [Number(ll.lon), Number(ll.lat)] },
          properties: {
            kind: 'player',
            assetCategory: 'PLAYER',
            assetKind: 'player',
            assetKey: 'PLAYER||PLAYER',
            label: 'OWN',
            fill: '#d11bff',
            stroke: '#8307a8',
            textColor: '#f2a8ff',
            radius: 5,
          },
        };
        features.push(ownshipFeature);
      }

      if (!ownshipFeature.geometry || !Array.isArray(ownshipFeature.geometry.coordinates)) {
        ownshipFeature.geometry = { type: 'Point', coordinates: [Number(ll.lon), Number(ll.lat)] };
      }

      ownshipFeature.geometry.coordinates[0] = Number(ll.lon);
      ownshipFeature.geometry.coordinates[1] = Number(ll.lat);

      if (!ownshipFeature.properties || typeof ownshipFeature.properties !== 'object') {
        ownshipFeature.properties = { kind: 'player' };
      }
      const heading = getOwnshipHeadingDeg(model, ownship);
      if (isFinite(Number(heading))) {
        ownshipFeature.properties.headingDeg = Number(heading);
      }

      if (removedStalePlayers) {
        item.payload = payload;
      }

      renderOpenFreeMapOverlay(item, payload);
      return true;
    }

    function getActiveEfbSaSelectionKey() {
      try {
        const host = document.querySelector('#efbChartViewport .fltPlanOpenMapHost[data-openfreemap-map-id]');
        if (host) {
          const mapId = String(host.getAttribute('data-openfreemap-map-id') || '');
          if (mapId) {
            const item = openFreeMapInstancesByContainerId[mapId];
            if (item && item.selectedKey) return String(item.selectedKey);
          }
          const hostSelection = String(host.getAttribute('data-openfreemap-selection') || '');
          if (hostSelection) return hostSelection;
        }
      } catch (_) {
      }

      const context = resolveActiveSaMapContext(latestData || null);
      return context && context.selected ? String(context.selected) : '';
    }

    function getEfbSaHistoryTrackForSelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return [];
      if (!Array.isArray(efbSaHistoryTrackBySelection[key])) {
        efbSaHistoryTrackBySelection[key] = [];
      }
      return efbSaHistoryTrackBySelection[key];
    }

    function getEfbSaHistoryTrackByKey(selectionKey) {
      const key = String(selectionKey || '').trim();
      if (!key) return [];
      if (!Array.isArray(efbSaHistoryTrackBySelection[key])) {
        efbSaHistoryTrackBySelection[key] = [];
      }
      return efbSaHistoryTrackBySelection[key];
    }

    function clearAllEfbSaHistoryTracks() {
      efbSaHistoryTrackBySelection = {};
      efbSaHistoryTrackLastSampleBySelection = {};
    }

    function maybeCaptureEfbSaHistoryTrackSample(selected, data) {
      const model = data || latestData || {};
      const ownship = getPlayerMapPoint(model);
      if (!ownship) return;
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;

      const nowMs = Date.now();
      const lastSample = efbSaHistoryTrackLastSampleBySelection[key] || null;
      const x = Number(ownship.xNum);
      const y = Number(ownship.yNum);
      if (!isFinite(x) || !isFinite(y)) return;

      if (lastSample && isFinite(Number(lastSample.ts)) && (nowMs - Number(lastSample.ts)) < efbSaHistoryTrackSampleMs) {
        return;
      }

      const track = getEfbSaHistoryTrackByKey(key);
      const lastPoint = track.length ? track[track.length - 1] : null;
      if (lastPoint) {
        const dx = x - Number(lastPoint.xNum);
        const dy = y - Number(lastPoint.yNum);
        const movedMeters = Math.sqrt((dx * dx) + (dy * dy));
        if (!isFinite(movedMeters) || movedMeters < efbSaHistoryTrackMinMoveMeters) {
          efbSaHistoryTrackLastSampleBySelection[key] = { xNum: x, yNum: y, ts: nowMs };
          return;
        }
      }

      track.push({ xNum: x, yNum: y, ts: nowMs });
      if (track.length > efbSaHistoryTrackMaxPoints) {
        track.splice(0, track.length - efbSaHistoryTrackMaxPoints);
      }
      efbSaHistoryTrackLastSampleBySelection[key] = { xNum: x, yNum: y, ts: nowMs };
    }

    function pollEfbSaOwnshipFast() {
      if (efbSaOwnshipPollTickerId) return;
      efbSaOwnshipPollTickerId = window.setInterval(function () {
        const isSaMapTabActive = selectedTab === 'EFB' && normalizeEfbViewerMode(efbViewerMode) === 'sa-map';
        const wantsBackgroundHistoryCapture = efbSaShowHistoryTrack === true && efbSaUseFastOwnshipEnabled === true;
        if (!isSaMapTabActive && !wantsBackgroundHistoryCapture) return;

        const nowMs = Date.now();
        if ((nowMs - Number(efbSaOwnshipFast.lastPollMs || 0)) < 120) return;
        efbSaOwnshipFast.lastPollMs = nowMs;

        fetch('/okb/sa/ownship', { cache: 'no-store' })
          .then(function (res) {
            if (!res || !res.ok) return null;
            return res.json();
          })
          .then(function (payload) {
            if (!payload || payload.hasPosition !== true) return;
            const posX = Number(payload.posX);
            const posY = Number(payload.posY);
            if (!isFinite(posX) || !isFinite(posY)) return;
            efbSaOwnshipFast.hasPosition = true;
            efbSaOwnshipFast.posX = posX;
            efbSaOwnshipFast.posY = posY;
            efbSaOwnshipFast.altFeet = Number(payload.altFeet);
            efbSaOwnshipFast.headingDeg = Number(payload.headingDeg);
            efbSaOwnshipFast.hasHeading = payload.hasHeading === true && isFinite(Number(payload.headingDeg));
            efbSaOwnshipFast.hasGroundSpeed = payload.hasGroundSpeed === true && isFinite(Number(payload.groundSpeedKnots));
            efbSaOwnshipFast.groundSpeedKnots = Number(payload.groundSpeedKnots);
            efbSaOwnshipFast.hasWow = payload.hasWow === true;
            efbSaOwnshipFast.wow = Number(payload.wow);
            efbSaOwnshipFast.theater = String(payload.theater || '');
            efbSaOwnshipFast.updatedUtcMs = Date.now();
            updateFastOwnshipToggleUi();
            maybeApplyEfbAdLandingAssist();

            let selectedKey = '';
            if (isSaMapTabActive) {
              selectedKey = getActiveEfbSaSelectionKey();
            }
            if (!selectedKey && wantsBackgroundHistoryCapture) {
              const saMapContext = resolveActiveSaMapContext(latestData || null);
              const fallbackSelection = '__EFB_OWNSHIP__';
              selectedKey = saMapContext && saMapContext.selected
                ? String(saMapContext.selected)
                : (!fakeMissionEnabled && getEfbModuleGateState(latestData || null).allowed ? fallbackSelection : '');
            }

            if (wantsBackgroundHistoryCapture && selectedKey) {
              maybeCaptureEfbSaHistoryTrackSample(selectedKey, latestData || null);
            }

            if (efbSaFollowOwnshipEnabled && isSaMapTabActive) {
              if (selectedKey) {
                applyFastOwnshipToSaMap(selectedKey, latestData || null);
                applyOpenFreeMapOwnshipCamera(selectedKey, latestData || null);
              }
            } else if (isSaMapTabActive) {
              if (selectedKey) {
                applyFastOwnshipToSaMap(selectedKey, latestData || null);
              }
            }
          })
          .catch(function () { });
      }, 100);
    }

    function isUserMapInteractionEvent(ev) {
      if (!ev || typeof ev !== 'object') return false;
      if (ev.originalEvent) return true;
      if (ev.type === 'wheel') return true;
      if (typeof ev.pointerType === 'string' && ev.pointerType.length > 0) return true;
      return false;
    }

    function getOwnshipHeadingDeg(data, ownshipPoint) {
      if (fakeMissionEnabled && fakeMissionState) {
        const simPlayer = (Array.isArray(fakeMissionState.assets) ? fakeMissionState.assets : []).find(function (a) {
          return String((a && a.Category) || '').toUpperCase() === 'PLAYER';
        });
        const simHeading = Number(simPlayer && simPlayer.headingDeg);
        if (isFinite(simHeading)) {
          lastOwnshipHeadingDeg = normalizeHeadingDeg(simHeading);
          return Number(lastOwnshipHeadingDeg);
        }
      }

      if (!fakeMissionEnabled && efbSaOwnshipFast && efbSaOwnshipFast.hasHeading) {
        const fastHeading = Number(efbSaOwnshipFast.headingDeg);
        if (isFinite(fastHeading)) {
          lastOwnshipHeadingDeg = normalizeHeadingDeg(fastHeading);
        }
      }

      const server = (data && data.Server) || {};
      const headingCandidates = [
        Number(server.PlayerHeadingDeg),
        Number(server.PlayerHeading),
        Number(server.PlayerHdg),
        Number(server.PlayerCourse),
        Number(server.PlayerTrack),
        Number(server.PlayerYawDeg),
      ];

      for (let i = 0; i < headingCandidates.length; i++) {
        const v = Number(headingCandidates[i]);
        if (isFinite(v)) {
          lastOwnshipHeadingDeg = normalizeHeadingDeg(v);
          break;
        }
      }

      const pt = ownshipPoint && isFinite(Number(ownshipPoint.xNum)) && isFinite(Number(ownshipPoint.yNum))
        ? { xNum: Number(ownshipPoint.xNum), yNum: Number(ownshipPoint.yNum) }
        : null;

      if (pt && lastOwnshipPointForHeading) {
        const dx = pt.xNum - Number(lastOwnshipPointForHeading.xNum);
        const dy = pt.yNum - Number(lastOwnshipPointForHeading.yNum);
        const dist = Math.sqrt((dx * dx) + (dy * dy));
        if (isFinite(dist) && dist >= 6) {
          lastOwnshipHeadingDeg = normalizeHeadingDeg((Math.atan2(dy, dx) * 180.0 / Math.PI));
        }
      }

      if (pt) {
        lastOwnshipPointForHeading = pt;
      }

      return isFinite(Number(lastOwnshipHeadingDeg)) ? Number(lastOwnshipHeadingDeg) : NaN;
    }

    function computeBraBetweenPoints(fromPoint, toPoint) {
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
        distNm: distMeters / 1852.0,
      };
    }

    function formatMapBraReadout(data, selectedAsset, bullseyePoint) {
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

      const unitModes = getGlobalUnitModes(data);
      const altFeet = Math.max(0, Math.round(Number(selectedAsset.altFeet) || 0));
      const altText = formatAltitudeByMode(altFeet, unitModes.alt, true);
      const ownRange = ownBra ? formatDistanceByMode(ownBra.distNm, unitModes.dist, true) : '--';
      const bullRange = bullBra ? formatDistanceByMode(bullBra.distNm, unitModes.dist, true) : '--';
      const left = ownBra
        ? ('BRA ' + ownBra.bearing + '/' + ownRange + '/' + String(altText))
        : ('BRA ---/--/' + String(altText));
      const right = bullBra
        ? ('B/E ' + bullBra.bearing + '/' + bullRange + '/' + String(altText))
        : ('B/E ---/--/' + String(altText));
      return left + '   ' + right;
    }

    function computeLegDistanceNm(prevWp, currWp) {
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

    function parseWaypointSpeedKnots(spd) {
      const v = Number(spd);
      if (!isFinite(v) || v <= 0) return NaN;
      return v;
    }

    function formatDoghouseEteSeconds(seconds) {
      const n = Number(seconds);
      if (!isFinite(n) || n <= 0) return '--:--';
      const total = Math.max(1, Math.round(n));
      const m = Math.floor(total / 60);
      const s = total % 60;
      return String(m).padStart(2, '0') + ':' + String(s).padStart(2, '0');
    }

    function buildDoghouseLegData(rows, theater) {
      const list = Array.isArray(rows) ? rows : [];
      if (list.length < 2) return [];
      const unitModes = getGlobalUnitModes(latestData || null);
      const magVar = Number(getApproxMagVariationDeg(theater));
      const doghouses = [];
      for (let i = 1; i < list.length; i++) {
        const fromWp = list[i - 1];
        const toWp = list[i];
        const fromX = Number(fromWp && fromWp.xNum);
        const fromY = Number(fromWp && fromWp.yNum);
        const toX = Number(toWp && toWp.xNum);
        const toY = Number(toWp && toWp.yNum);
        if (!isFinite(fromX) || !isFinite(fromY) || !isFinite(toX) || !isFinite(toY)) continue;

        const headingTrue = computeTrueHeadingDeg({ x: fromX, y: fromY }, { x: toX, y: toY });
        const headingMag = isFinite(headingTrue) ? normalizeHeadingDeg(headingTrue - magVar) : NaN;
        const distanceNm = computeLegDistanceNm({ x: fromX, y: fromY }, { x: toX, y: toY });

        let eteSeconds = NaN;
        const fromEta = parseEtaToSeconds(fromWp && (fromWp.etaDisplay || fromWp.eta));
        const toEta = parseEtaToSeconds(toWp && (toWp.etaDisplay || toWp.eta));
        if (isFinite(fromEta) && isFinite(toEta) && toEta > fromEta) {
          eteSeconds = toEta - fromEta;
        } else {
          const speedKts = parseWaypointSpeedKnots(toWp && toWp.spd);
          if (isFinite(distanceNm) && distanceNm > 0 && isFinite(speedKts) && speedKts > 0) {
            eteSeconds = (distanceNm * 3600.0) / speedKts;
          }
        }

        const altFeet = isFinite(Number(toWp && toWp.altFeet))
          ? Number(toWp.altFeet)
          : (isFinite(Number(toWp && toWp.alt)) ? Number(toWp.alt) : NaN);

        const midXNum = (fromX + toX) / 2.0;
        const midYNum = (fromY + toY) / 2.0;

        const distTextRaw = formatDistanceByMode(distanceNm, unitModes.dist, false);
        const altTextRaw = formatAltitudeByMode(altFeet, unitModes.alt, false);
        doghouses.push({
          fromXNum: fromX,
          fromYNum: fromY,
          toXNum: toX,
          toYNum: toY,
          midXNum: midXNum,
          midYNum: midYNum,
          mhText: formatHeadingDeg(headingMag),
          distText: distTextRaw,
          distUnit: unitModes.dist === 'km' ? 'km' : 'NM',
          eteText: formatDoghouseEteSeconds(eteSeconds),
          altText: altTextRaw,
          altUnit: unitModes.alt === 'm' ? 'm' : 'ft',
        });
      }
      return doghouses;
    }

    function estimateLegSpeedKcas(distanceNm, legSeconds, altFeet) {
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

    function formatDistanceNm(distanceNm) {
      const d = Number(distanceNm);
      if (!isFinite(d) || d < 0) return '-';
      if (d < 10) {
        return d.toFixed(1);
      }
      return String(Math.round(d));
    }

    function normalizeHeadingDeg(deg) {
      let d = Number(deg);
      if (!isFinite(d)) return NaN;
      d = ((d % 360) + 360) % 360;
      return d;
    }

    function formatHeadingDeg(deg) {
      const d = normalizeHeadingDeg(deg);
      if (!isFinite(d)) return '-';
      let rounded = Math.round(d);
      if (rounded <= 0) rounded = 360;
      if (rounded > 360) rounded = 360;
      return String(rounded).padStart(3, '0');
    }

    function computeTrueHeadingDeg(fromWp, toWp) {
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

    function getApproxMagVariationDeg(theater) {
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

    function isAltTypeAgl(altType) {
      const t = String(altType || '').toUpperCase();
      return t.indexOf('AGL') >= 0;
    }

    function formatAltCellHtml(wp) {
      const altitude = escapeHtml(String((wp && wp.alt) || '-'));
      if (wp && isAltTypeAgl(wp.altType)) {
        return altitude + '<span class="fltPlanAltTag">AGL</span>';
      }
      return altitude;
    }

    function getNavlogCoordDisplayMode(selected) {
      const state = getFlightPlanPlanState(selected);
      const mode = String(state && state.coordDisplayMode || 'xy').toLowerCase();
      if (mode === 'dms' || mode === 'ddm' || mode === 'mgrs') return mode;
      return 'xy';
    }

    function cycleNavlogCoordDisplayMode(selected) {
      const state = getFlightPlanPlanState(selected);
      const order = ['xy', 'dms', 'ddm', 'mgrs'];
      const current = getNavlogCoordDisplayMode(selected);
      const idx = order.indexOf(current);
      state.coordDisplayMode = order[(idx + 1) % order.length];
    }

    function getNavlogAltDisplayMode(selected) {
      const state = getFlightPlanPlanState(selected);
      return normalizeNavlogAltDisplayMode(state && state.navlogAltDisplayMode);
    }

    function cycleNavlogAltDisplayMode(selected) {
      const state = getFlightPlanPlanState(selected);
      state.navlogAltDisplayMode = getNavlogAltDisplayMode(selected) === 'm' ? 'ft' : 'm';
      defaultNavlogAltDisplayMode = normalizeNavlogAltDisplayMode(state.navlogAltDisplayMode);
      persistNavlogAltDisplayModePreference(defaultNavlogAltDisplayMode);
    }

    function getNavlogSpdDisplayMode(selected) {
      const state = getFlightPlanPlanState(selected);
      return normalizeNavlogSpdDisplayMode(state && state.navlogSpdDisplayMode);
    }

    function cycleNavlogSpdDisplayMode(selected) {
      const state = getFlightPlanPlanState(selected);
      state.navlogSpdDisplayMode = getNavlogSpdDisplayMode(selected) === 'kmh' ? 'kts' : 'kmh';
      defaultNavlogSpdDisplayMode = normalizeNavlogSpdDisplayMode(state.navlogSpdDisplayMode);
      persistNavlogSpdDisplayModePreference(defaultNavlogSpdDisplayMode);
    }

    function getNavlogDistDisplayMode(selected) {
      const state = getFlightPlanPlanState(selected);
      return normalizeNavlogDistDisplayMode(state && state.navlogDistDisplayMode);
    }

    function cycleNavlogDistDisplayMode(selected) {
      const state = getFlightPlanPlanState(selected);
      state.navlogDistDisplayMode = getNavlogDistDisplayMode(selected) === 'km' ? 'nm' : 'km';
      defaultNavlogDistDisplayMode = normalizeNavlogDistDisplayMode(state.navlogDistDisplayMode);
      persistNavlogDistDisplayModePreference(defaultNavlogDistDisplayMode);
    }

    function resolveGlobalUnitSelection(data) {
      const model = data || latestData || null;

      const dtcSelected = getActiveFlightPlanSelection(model || {});
      if (dtcSelected) return String(dtcSelected);

      const saContext = resolveActiveSaMapContext(model);
      if (saContext && saContext.selected) return String(saContext.selected);

      const activeSaSelection = getActiveEfbSaSelectionKey();
      if (activeSaSelection) return String(activeSaSelection);

      return '__RUNTIME_PLAYER__';
    }

    function formatAltitudeByMode(altFeet, mode, withSuffix) {
      const n = Number(altFeet);
      if (!isFinite(n)) return '-';
      const metric = String(mode || 'ft').toLowerCase() === 'm';
      const v = metric ? Math.round(n * 0.3048) : Math.round(n);
      if (withSuffix !== true) return String(v);
      return String(v) + (metric ? 'm' : 'ft');
    }

    function formatSpeedByMode(speedKts, mode, withSuffix) {
      const n = Number(speedKts);
      if (!isFinite(n) || n <= 0) return '-';
      const metric = String(mode || 'kts').toLowerCase() === 'kmh';
      const v = metric ? Math.round(n * 1.852) : Math.round(n);
      if (withSuffix !== true) return String(v);
      return String(v) + (metric ? 'km/h' : 'kt');
    }

    function formatDistanceByMode(distanceNm, mode, withSuffix) {
      const n = Number(distanceNm);
      if (!isFinite(n) || n < 0) return '-';
      const metric = String(mode || 'nm').toLowerCase() === 'km';
      const v = metric
        ? (n * 1.852)
        : n;
      const text = metric
        ? (v < 10 ? v.toFixed(1) : String(Math.round(v)))
        : (v < 10 ? v.toFixed(1) : String(Math.round(v)));
      if (withSuffix !== true) return text;
      return text + (metric ? 'km' : 'nm');
    }

    function getGlobalUnitModes(data) {
      const selected = resolveGlobalUnitSelection(data);
      return {
        selected: selected,
        alt: getNavlogAltDisplayMode(selected),
        spd: getNavlogSpdDisplayMode(selected),
        dist: getNavlogDistDisplayMode(selected),
      };
    }

    function convertAltitudeDisplayDeltaToFeet(displayDelta, mode) {
      const delta = Number(displayDelta);
      if (!isFinite(delta) || delta === 0) return 0;
      return String(mode || 'ft').toLowerCase() === 'm'
        ? (delta / 0.3048)
        : delta;
    }

    function getAltitudeAdjustmentStepByMode(currentAltFeet, mode) {
      if (String(mode || 'ft').toLowerCase() === 'm') {
        const altFeet = Number(currentAltFeet);
        const altMeters = isFinite(altFeet) ? (altFeet * 0.3048) : NaN;
        return (!isFinite(altMeters) || altMeters < 300) ? 50 : 100;
      }
      return getAltitudeAdjustmentStep(currentAltFeet);
    }

    function formatNavlogAltitudeDisplayHtml(wp, mode) {
      const displayMode = String(mode || 'ft').toLowerCase();
      if (displayMode !== 'm') {
        return formatAltCellHtml(wp);
      }
      const altFeet = Number(wp && wp.altFeet);
      if (!isFinite(altFeet)) return '-';
      const altMeters = Math.round(altFeet * 0.3048);
      const altitude = escapeHtml(String(altMeters));
      if (wp && isAltTypeAgl(wp.altType)) {
        return altitude + '<span class="fltPlanAltTag">AGL</span>';
      }
      return altitude;
    }

    function formatNavlogSpeedDisplayText(wp, speedDisplay, speedRec, mode) {
      const displayMode = String(mode || 'kts').toLowerCase();
      if (displayMode !== 'kmh') {
        return String(speedDisplay || '-');
      }
      const speedKcas = speedRec ? Number(speedRec.kcas) : Number(wp && wp.spd);
      if (!isFinite(speedKcas) || speedKcas <= 0) return '-';
      return String(Math.round(speedKcas * 1.852));
    }

    function formatNavlogDistanceDisplayText(wp, mode) {
      const displayMode = String(mode || 'nm').toLowerCase();
      if (displayMode !== 'km') {
        return String((wp && wp.dist) || '-');
      }
      const distNm = Number(wp && wp.dist);
      if (!isFinite(distNm) || distNm < 0) return '-';
      return (distNm * 1.852).toFixed(1);
    }

    function getNavlogCoordinateDisplayText(wp, theatre, mode) {
      const north = Number(wp && wp.xNum);
      const east = Number(wp && wp.yNum);
      if (!isFinite(north) || !isFinite(east)) return '- / -';

      const displayMode = String(mode || 'xy').toLowerCase();
      if (displayMode === 'xy') {
        return String(Math.round(north)) + ' / ' + String(Math.round(east));
      }

      const ll = convertDcsXYToLatLon(theatre, north, east);
      if (!ll) return String(Math.round(north)) + ' / ' + String(Math.round(east));

      if (displayMode === 'dms') {
        return formatLatLonDms(ll.lat, ll.lon);
      }
      if (displayMode === 'ddm') {
        return formatLatLonDdm(ll.lat, ll.lon);
      }
      if (displayMode === 'mgrs') {
        return formatLatLonMgrs(ll.lat, ll.lon);
      }

      return String(Math.round(north)) + ' / ' + String(Math.round(east));
    }

    function convertDcsXYToLatLon(theatre, dcsX, dcsY) {
      const projection = getMapProjectionByTheatre(theatre);
      if (!projection) return null;

      const easting = Number(dcsY);
      const northing = Number(dcsX);
      if (!isFinite(easting) || !isFinite(northing)) return null;

      try {
        return inverseTransverseMercator(easting, northing, projection);
      } catch (_) {
        return null;
      }
    }

    function getMapProjectionByTheatre(theatre) {
      const t = String(theatre || '').trim();
      if (!t) return null;
      if (mapProjectionCatalog[t]) return mapProjectionCatalog[t];

      const normalized = t.replace(/[^A-Za-z0-9]/g, '').toUpperCase();
      if (!normalized) return null;

      if (mapProjectionAliases[normalized] && mapProjectionCatalog[mapProjectionAliases[normalized]]) {
        return mapProjectionCatalog[mapProjectionAliases[normalized]];
      }

      const keys = Object.keys(mapProjectionCatalog);
      for (let i = 0; i < keys.length; i++) {
        const key = keys[i];
        const keyNorm = String(key || '').replace(/[^A-Za-z0-9]/g, '').toUpperCase();
        if (keyNorm === normalized) {
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

    function inverseTransverseMercator(easting, northing, projection) {
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

    function toRadians(degrees) {
      return Number(degrees) * Math.PI / 180.0;
    }

    function toDegrees(radians) {
      return Number(radians) * 180.0 / Math.PI;
    }

    function formatLatLonDms(lat, lon) {
      return formatSingleCoordDms(lat, 'N', 'S', 2) + ' / ' + formatSingleCoordDms(lon, 'E', 'W', 3);
    }

    function formatSingleCoordDms(value, positiveHemisphere, negativeHemisphere, degreeWidth) {
      const n = Number(value);
      if (!isFinite(n)) return '-';
      const hemi = n >= 0 ? positiveHemisphere : negativeHemisphere;
      const abs = Math.abs(n);
      let degrees = Math.floor(abs);
      let minutesTotal = (abs - degrees) * 60.0;
      let minutes = Math.floor(minutesTotal);
      let seconds = Math.round((minutesTotal - minutes) * 60.0);
      if (seconds >= 60) {
        seconds = 0;
        minutes += 1;
      }
      if (minutes >= 60) {
        minutes = 0;
        degrees += 1;
      }
      return hemi + String(degrees).padStart(degreeWidth, '0') + '°' + String(minutes).padStart(2, '0') + 'm' + String(seconds).padStart(2, '0') + 's';
    }

    function formatLatLonDdm(lat, lon) {
      return formatSingleCoordDdm(lat, 'N', 'S', 2) + ' / ' + formatSingleCoordDdm(lon, 'E', 'W', 3);
    }

    function formatSingleCoordDdm(value, positiveHemisphere, negativeHemisphere, degreeWidth) {
      const n = Number(value);
      if (!isFinite(n)) return '-';
      const hemi = n >= 0 ? positiveHemisphere : negativeHemisphere;
      const abs = Math.abs(n);
      let degrees = Math.floor(abs);
      let minutes = (abs - degrees) * 60.0;
      if (minutes >= 59.99995) {
        minutes = 0;
        degrees += 1;
      }
      return hemi + String(degrees).padStart(degreeWidth, '0') + '°' + minutes.toFixed(3).padStart(6, '0') + 'm';
    }

    function formatLatLonMgrs(lat, lon) {
      const utm = latLonToUtm(lat, lon);
      if (!utm) return 'MGRS N/A';
      const letters = getMgrsLetters(utm.zone, utm.easting, utm.northing, lat);
      if (!letters) return 'MGRS N/A';

      const eastingRemainder = Math.floor(((utm.easting % 100000) + 100000) % 100000);
      const northingRemainder = Math.floor(((utm.northing % 100000) + 100000) % 100000);
      return String(utm.zone) + utm.band + ' ' + letters + ' ' + String(eastingRemainder).padStart(5, '0') + ' ' + String(northingRemainder).padStart(5, '0');
    }

    function latLonToUtm(lat, lon) {
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

      if (latitude < 0) {
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

    function getUtmLatitudeBand(lat) {
      const bands = 'CDEFGHJKLMNPQRSTUVWX';
      const clamped = Math.max(-80, Math.min(84, Number(lat)));
      const idx = Math.min(bands.length - 1, Math.max(0, Math.floor((clamped + 80) / 8)));
      return bands.charAt(idx);
    }

    function getMgrsLetters(zone, easting, northing, latitude) {
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

    function forwardTransverseMercator(lat, lon, projection) {
      const latitude = Number(lat);
      const longitude = Number(lon);
      if (!isFinite(latitude) || !isFinite(longitude) || !projection) return null;

      const semiMajorAxis = 6378137.0;
      const flattening = 1.0 / 298.257223563;
      const eccentricitySquared = flattening * (2.0 - flattening);
      const ePrimeSquared = eccentricitySquared / (1.0 - eccentricitySquared);
      const scaleFactor = Number(projection.scaleFactor) || 0.9996;

      const latRad = toRadians(latitude);
      const lonRad = toRadians(longitude);
      const lonOriginRad = toRadians(Number(projection.centralMeridianDeg));

      const n = semiMajorAxis / Math.sqrt(1 - eccentricitySquared * Math.sin(latRad) * Math.sin(latRad));
      const t = Math.tan(latRad) * Math.tan(latRad);
      const c = ePrimeSquared * Math.cos(latRad) * Math.cos(latRad);
      const A = Math.cos(latRad) * (lonRad - lonOriginRad);

      const m = semiMajorAxis
        * ((1 - eccentricitySquared / 4 - 3 * Math.pow(eccentricitySquared, 2) / 64 - 5 * Math.pow(eccentricitySquared, 3) / 256) * latRad
          - (3 * eccentricitySquared / 8 + 3 * Math.pow(eccentricitySquared, 2) / 32 + 45 * Math.pow(eccentricitySquared, 3) / 1024) * Math.sin(2 * latRad)
          + (15 * Math.pow(eccentricitySquared, 2) / 256 + 45 * Math.pow(eccentricitySquared, 3) / 1024) * Math.sin(4 * latRad)
          - (35 * Math.pow(eccentricitySquared, 3) / 3072) * Math.sin(6 * latRad));

      const easting = Number(projection.falseEastingMeters)
        + scaleFactor * n * (A
          + (1 - t + c) * Math.pow(A, 3) / 6
          + (5 - 18 * t + t * t + 72 * c - 58 * ePrimeSquared) * Math.pow(A, 5) / 120);

      const northing = Number(projection.falseNorthingMeters)
        + scaleFactor * (m
          + n * Math.tan(latRad) * (Math.pow(A, 2) / 2
            + (5 - t + 9 * c + 4 * c * c) * Math.pow(A, 4) / 24
            + (61 - 58 * t + t * t + 600 * c - 330 * ePrimeSquared) * Math.pow(A, 6) / 720));

      if (!isFinite(easting) || !isFinite(northing)) return null;
      return {
        xNum: northing,
        yNum: easting,
      };
    }

    function convertLatLonToDcsXY(theatre, lat, lon) {
      const projection = getMapProjectionByTheatre(theatre);
      if (!projection) return null;
      try {
        return forwardTransverseMercator(lat, lon, projection);
      } catch (_) {
        return null;
      }
    }

    function getMgrsBandLatRange(bandLetter) {
      const bands = 'CDEFGHJKLMNPQRSTUVWX';
      const band = String(bandLetter || '').toUpperCase();
      const idx = bands.indexOf(band);
      if (idx < 0) return null;
      const minLat = -80 + (idx * 8);
      const maxLat = (band === 'X') ? 84 : (minLat + 8);
      return { minLat: minLat, maxLat: maxLat };
    }

    function getMgrsEastingFromDigraph(zone, eastingLetter) {
      const zoneNum = Number(zone);
      const letter = String(eastingLetter || '').toUpperCase();
      if (!isFinite(zoneNum) || zoneNum < 1 || zoneNum > 60 || !letter) return NaN;
      const sets = ['ABCDEFGH', 'JKLMNPQR', 'STUVWXYZ'];
      const eSet = (Math.round(zoneNum) - 1) % 3;
      const list = sets[eSet] || '';
      const idx = list.indexOf(letter);
      if (idx < 0) return NaN;
      return (idx + 1) * 100000;
    }

    function getMgrsNorthingFromDigraph(zone, northingLetter) {
      const zoneNum = Number(zone);
      const letter = String(northingLetter || '').toUpperCase();
      if (!isFinite(zoneNum) || zoneNum < 1 || zoneNum > 60 || !letter) return NaN;
      const odd = 'ABCDEFGHJKLMNPQRSTUV';
      const even = 'FGHJKLMNPQRSTUVABCDE';
      const list = (Math.round(zoneNum) % 2 === 0) ? even : odd;
      const idx = list.indexOf(letter);
      if (idx < 0) return NaN;
      return idx * 100000;
    }

    function parseMgrsTarget(text, theatre) {
      const input = String(text || '').toUpperCase();
      if (!input) return null;

      const zoneBandMatch = input.match(/\b(\d{1,2})([C-HJ-NP-X])\s*([A-HJ-NP-Z]{2})\s*(\d{2,5})\s*(\d{2,5})\b/);
      const digraphMatch = input.match(/\b([A-HJ-NP-Z]{2})\s*(\d{4,5})\s*(\d{4,5})\b/);

      let zone = NaN;
      let band = '';
      let digraph = '';
      let eastingDigits = '';
      let northingDigits = '';

      if (zoneBandMatch) {
        zone = Number(zoneBandMatch[1]);
        band = String(zoneBandMatch[2] || '').toUpperCase();
        digraph = String(zoneBandMatch[3] || '').toUpperCase();
        eastingDigits = String(zoneBandMatch[4] || '');
        northingDigits = String(zoneBandMatch[5] || '');
      } else if (digraphMatch) {
        const center = getTheatreCenterLonLat(theatre);
        if (!Array.isArray(center) || center.length < 2) return null;
        const utm = latLonToUtm(Number(center[1]), Number(center[0]));
        if (!utm || !isFinite(Number(utm.zone))) return null;
        zone = Number(utm.zone);
        band = String(utm.band || '').toUpperCase();
        digraph = String(digraphMatch[1] || '').toUpperCase();
        eastingDigits = String(digraphMatch[2] || '');
        northingDigits = String(digraphMatch[3] || '');
      } else {
        return null;
      }

      if (!isFinite(zone) || zone < 1 || zone > 60 || !digraph || digraph.length !== 2) return null;
      const len = Math.min(5, Math.max(1, Math.min(eastingDigits.length, northingDigits.length)));
      const eRaw = String(eastingDigits || '').substring(0, len).padEnd(5, '0');
      const nRaw = String(northingDigits || '').substring(0, len).padEnd(5, '0');

      const e100k = getMgrsEastingFromDigraph(zone, digraph.charAt(0));
      const n100kBase = getMgrsNorthingFromDigraph(zone, digraph.charAt(1));
      const easting = e100k + Number(eRaw);
      if (!isFinite(easting) || !isFinite(n100kBase)) return null;

      const bandRange = getMgrsBandLatRange(band);
      if (!bandRange) return null;
      const zoneCentralMeridian = ((Math.round(zone) - 1) * 6) - 180 + 3;
      const minProbe = latLonToUtm(Number(bandRange.minLat) + 0.0001, zoneCentralMeridian);
      const maxProbe = latLonToUtm(Number(bandRange.maxLat) - 0.0001, zoneCentralMeridian);
      if (!minProbe || !maxProbe) return null;
      const minNorthing = Number(minProbe.northing);
      const maxNorthing = Number(maxProbe.northing);
      if (!isFinite(minNorthing) || !isFinite(maxNorthing)) return null;

      let northing = n100kBase + Number(nRaw);
      while (northing < minNorthing) northing += 2000000;
      while (northing > maxNorthing + 1000000) northing -= 2000000;
      if (northing < minNorthing || northing > (maxNorthing + 120000)) return null;

      const latLon = inverseTransverseMercator(easting, northing, {
        centralMeridianDeg: zoneCentralMeridian,
        falseEastingMeters: 500000,
        falseNorthingMeters: (bandRange.maxLat <= 0 ? 10000000 : 0),
        scaleFactor: 0.9996,
      });
      if (!latLon || !isFinite(Number(latLon.lat)) || !isFinite(Number(latLon.lon))) return null;

      const printedMgrs = String(Math.round(zone)) + band + ' ' + digraph + ' ' + eRaw + ' ' + nRaw;
      return {
        zone: Math.round(zone),
        band: band,
        digraph: digraph,
        eastingDigits: eRaw,
        northingDigits: nRaw,
        mgrsText: printedMgrs,
        lat: Number(latLon.lat),
        lon: Number(latLon.lon),
      };
    }

    function formatMgrsDigraphDisplayFromParsed(parsedMgrs) {
      const p = parsedMgrs || {};
      const digraph = String(p.digraph || '').toUpperCase().trim();
      const e = String(p.eastingDigits || '').replace(/[^0-9]/g, '').padEnd(5, '0').substring(0, 5);
      const n = String(p.northingDigits || '').replace(/[^0-9]/g, '').padEnd(5, '0').substring(0, 5);
      if (!digraph || digraph.length !== 2 || e.length !== 5 || n.length !== 5) return '';
      return digraph + ' ' + e + ' ' + n;
    }

    function normalizeMgrsTokenDisplay(tokenText, theatre) {
      const parsed = parseMgrsTarget(tokenText, theatre);
      if (!parsed) return String(tokenText || '').trim();
      const formatted = formatMgrsDigraphDisplayFromParsed(parsed);
      return formatted || String(tokenText || '').trim();
    }

    function normalizeJtacDirectionToBearing(text) {
      const v = String(text || '').trim().toUpperCase();
      if (!v) return NaN;
      if (/^\d{1,3}$/.test(v)) return normalizeHeadingDeg(Number(v));
      const map = {
        N: 0,
        NORTH: 0,
        NE: 45,
        NORTHEAST: 45,
        E: 90,
        EAST: 90,
        SE: 135,
        SOUTHEAST: 135,
        S: 180,
        SOUTH: 180,
        SW: 225,
        SOUTHWEST: 225,
        W: 270,
        WEST: 270,
        NW: 315,
        NORTHWEST: 315,
      };
      return map[v] !== undefined ? Number(map[v]) : NaN;
    }

    function parseJtacLineValue(blockText, lineNo) {
      const src = String(blockText || '').replace(/\s+/g, ' ').trim();
      if (!src) return '';
      const sep = '[-:\\u2013\\u2014\\u2212]';
      const rx = new RegExp('\\b' + String(lineNo) + '\\s*' + sep + '\\s*(.*?)(?=\\b[1-9]\\s*' + sep + '|\\bRMK\\b|\\bREMARKS?\\b|$)', 'i');
      const m = src.match(rx);
      if (!m) return '';
      return String(m[1] || '').replace(/\s+/g, ' ').trim();
    }

    function parseJtacNineLineBurst(text) {
      const src = String(text || '').replace(/\s+/g, ' ').trim();
      if (!src) return { lines: {}, rmkText: '', engagementType: '' };
      const lines = {};
      const sep = '[-:\\u2013\\u2014\\u2212]';
      const rx = new RegExp('\\b([4-9])\\s*' + sep + '\\s*(.*?)(?=\\b[4-9]\\s*' + sep + '|\\bRMK\\b|\\bREMARKS?\\b|$)', 'ig');
      let m;
      while ((m = rx.exec(src)) !== null) {
        const key = String(m[1] || '').trim();
        const value = String(m[2] || '').replace(/\s+/g, ' ').trim();
        if (key && value && !lines[key]) lines[key] = value;
      }
      const rmkMatch = src.match(/\bRMK\b\s*[:\-]?\s*(.*)$/i)
        || src.match(/\bREMARKS?\b\s*[:\-]?\s*(.*)$/i)
        || src.match(/\bPREF\b\s+(.+)$/i);
      const engagementMatch = src.match(/\bTYPE\s*(I|II|III)\b/i);
      return {
        lines: lines,
        rmkText: rmkMatch ? String(rmkMatch[1] || '').replace(/\s+/g, ' ').trim() : '',
        engagementType: engagementMatch ? ('TYPE ' + String(engagementMatch[1] || '').toUpperCase()) : '',
      };
    }

    function formatJtacNineLinePopupLines(source) {
      const s = source || {};
      const lines = [];

      function wrapJtacPopupLine(text, maxLen) {
        const src = String(text || '').replace(/\s+/g, ' ').trim();
        if (!src) return [];
        const words = src.split(' ');
        const out = [];
        let cur = '';
        const limit = Math.max(24, Number(maxLen) || 48);
        for (let i = 0; i < words.length; i++) {
          const w = String(words[i] || '').trim();
          if (!w) continue;
          const next = cur ? (cur + ' ' + w) : w;
          if (next.length <= limit) {
            cur = next;
            continue;
          }
          if (cur) out.push(cur);
          cur = w;
        }
        if (cur) out.push(cur);
        return out;
      }

      function pushWrappedPrefix(prefix, value) {
        const v = String(value || '').trim();
        if (!v) return;
        const wrapped = wrapJtacPopupLine(prefix + v, 48);
        wrapped.forEach(function (w) { lines.push(w); });
      }

      let line4 = String(s.nl4Text || '').trim();
      let line5 = String(s.nl5Text || '').trim();
      let line6 = String(s.nl6Text || '').trim();
      let line7 = String(s.nl7Text || '').trim();
      let line8 = String(s.nl8Text || '').trim();
      let line9 = String(s.nl9Text || '').trim();
      let rmkText = String(s.rmkText || '').trim();

      if (!line4 && !line5 && !line6 && !line7 && !line8 && !line9) {
        const model = (typeof latestData !== 'undefined' && latestData) ? latestData : {};
        const burst = String(getMergedLog((model && model.Logs) || {}, 'JTAC') || '').trim();
        if (burst) {
          line4 = parseJtacLineValue(burst, 4);
          line5 = parseJtacLineValue(burst, 5);
          line6 = parseJtacLineValue(burst, 6);
          line7 = parseJtacLineValue(burst, 7);
          line8 = parseJtacLineValue(burst, 8);
          line9 = parseJtacLineValue(burst, 9);
          if (!rmkText) {
            const rmkMatch = burst.match(/\bRMK\b\s*[:\-]?\s*(.*)$/i)
              || burst.match(/\bREMARKS?\b\s*[:\-]?\s*(.*)$/i)
              || burst.match(/\bPREF\b\s+(.+)$/i);
            rmkText = rmkMatch ? String(rmkMatch[1] || '').replace(/\s+/g, ' ').trim() : '';
          }
        }
      }

      lines.push('1-2-3 N/A');
      pushWrappedPrefix('4 - ', line4);
      pushWrappedPrefix('5 - ', line5);
      pushWrappedPrefix('6 - ', line6);
      pushWrappedPrefix('7 - ', line7);
      pushWrappedPrefix('8 - ', line8);
      pushWrappedPrefix('9 - ', line9);
      if (rmkText) {
        lines.push('');
        pushWrappedPrefix('RMK ', rmkText);
      }
      return lines;
    }

    function getJtacFriendlyPointFromTarget(target) {
      const t = target || {};
      const bearing = Number(t.friendlyBearing);
      const distanceMeters = Number(t.friendlyDistanceMeters);
      const north = Number(t.xNum);
      const east = Number(t.yNum);
      if (!isFinite(bearing) || !isFinite(distanceMeters) || distanceMeters <= 0) return null;
      if (!isFinite(north) || !isFinite(east)) return null;
      const rad = bearing * (Math.PI / 180.0);
      return {
        xNum: north + (Math.cos(rad) * distanceMeters),
        yNum: east + (Math.sin(rad) * distanceMeters),
      };
    }

    function getJtacEgressPointFromTarget(target, distanceNm) {
      const t = target || {};
      const heading = Number(t.egressHeadingDeg);
      const north = Number(t.xNum);
      const east = Number(t.yNum);
      const nm = isFinite(Number(distanceNm)) ? Number(distanceNm) : 5;
      if (!isFinite(heading) || !isFinite(north) || !isFinite(east) || nm <= 0) return null;
      const distMeters = nm * 1852.0;
      const rad = heading * (Math.PI / 180.0);
      return {
        xNum: north + (Math.cos(rad) * distMeters),
        yNum: east + (Math.sin(rad) * distMeters),
      };
    }

    function parseJtacHeadingRangeFromText(text) {
      const src = String(text || '').toUpperCase();
      if (!src) return null;
      const m = src.match(/\b(\d{1,3})\s*[-–]\s*(\d{1,3})\b/);
      if (!m) return null;
      const startDeg = normalizeHeadingDeg(Number(m[1]));
      const endDeg = normalizeHeadingDeg(Number(m[2]));
      if (!isFinite(startDeg) || !isFinite(endDeg)) return null;
      return {
        startDeg: startDeg,
        endDeg: endDeg,
      };
    }

    function getJtacReciprocalSectorPolygonPoints(target, innerNm, outerNm) {
      const t = target || {};
      const north = Number(t.xNum);
      const east = Number(t.yNum);
      if (!isFinite(north) || !isFinite(east)) return null;

      let attackStart = Number(t.attackHeadingStartDeg);
      let attackEnd = Number(t.attackHeadingEndDeg);
      if (!isFinite(attackStart) || !isFinite(attackEnd)) {
        const parsed = parseJtacHeadingRangeFromText(t.rmkText || '');
        if (!parsed) return null;
        attackStart = parsed.startDeg;
        attackEnd = parsed.endDeg;
      }

      const recipStart = normalizeHeadingDeg(attackStart + 180);
      const recipEnd = normalizeHeadingDeg(attackEnd + 180);
      const delta = ((recipEnd - recipStart) + 360) % 360;
      const sweepDeg = delta === 0 ? 360 : delta;

      const innerMeters = Math.max(0.1, Number(innerNm) || 1) * 1852.0;
      const outerMeters = Math.max(innerMeters + 50, (Math.max(0.2, Number(outerNm) || 5) * 1852.0));
      const steps = Math.max(8, Math.ceil(sweepDeg / 8));
      const points = [];

      for (let i = 0; i <= steps; i++) {
        const bearing = normalizeHeadingDeg(recipStart + ((sweepDeg * i) / steps));
        const rad = bearing * (Math.PI / 180.0);
        points.push({
          xNum: north + (Math.cos(rad) * outerMeters),
          yNum: east + (Math.sin(rad) * outerMeters),
        });
      }
      for (let i = steps; i >= 0; i--) {
        const bearing = normalizeHeadingDeg(recipStart + ((sweepDeg * i) / steps));
        const rad = bearing * (Math.PI / 180.0);
        points.push({
          xNum: north + (Math.cos(rad) * innerMeters),
          yNum: east + (Math.sin(rad) * innerMeters),
        });
      }

      return {
        startDeg: recipStart,
        endDeg: recipEnd,
        sweepDeg: sweepDeg,
        points: points,
      };
    }

    function parseJtacNineLineTargets(data) {
      const model = data || latestData || {};
      const server = (model && model.Server) || {};
      const theatreInfo = resolveOpenFreeMapTheatreInfo(model);
      const theatre = String((theatreInfo && theatreInfo.theatre) || resolveOpenFreeMapFallbackTheatreText(model) || server.Theater || lastKnownTheater || '').trim();
      if (!theatre) return [];

      const logs = getMergedLog(model.Logs, 'JTAC');
      const details = getMergedList(model.UnitDetails, 'JTAC');
      const units = getMergedList(model.Units, 'JTAC');
      const burstFromLogs = parseJtacNineLineBurst(logs || '');
      const sourceText = [logs].concat(details).concat(units).map(function (s) { return String(s || '').trim(); }).filter(function (s) { return !!s; }).join('\n');
      const burstFromSource = parseJtacNineLineBurst(sourceText);
      if (!sourceText) return [];

      const lines = sourceText.split(/\r?\n/).map(function (l) { return String(l || '').trim(); }).filter(function (l) { return !!l; });
      if (!lines.length) return [];

      const targetRegex = /(type\s*[:\-]?\s*([^\n,;]+)|target\s*[:\-]?\s*([^\n,;]+))/i;
      const engagementTypeRegex = /\bTYPE\s*(I|II|III)\b/i;
      const elevRegex = /(elev(?:ation)?\s*[:\-]?\s*([0-9]{2,6})\s*(ft|feet|m|meters?)?)/i;
      const friendBraRegex = /\bfriendly\b[^\n]*?\b(\d{3})\s*[\/-]\s*(\d{1,3}(?:\.\d+)?)\b/i;
      const friendDirectionRegex = /\b(N|S|E|W|NE|NW|SE|SW|NORTH|SOUTH|EAST|WEST)\b[^0-9]{0,12}(\d{2,6}(?:\.\d+)?)\s*(M|METERS?|KM|NM)?\b/i;

      const targets = [];
      const seen = {};
      for (let i = 0; i < lines.length; i++) {
        const line = lines[i];
        const mgrs = parseMgrsTarget(line, theatre);
        if (!mgrs) continue;

        const dcs = convertLatLonToDcsXY(theatre, mgrs.lat, mgrs.lon);
        if (!dcs || !isFinite(Number(dcs.xNum)) || !isFinite(Number(dcs.yNum))) continue;

        const key = mgrs.mgrsText + '|' + String(Math.round(Number(dcs.xNum))) + '|' + String(Math.round(Number(dcs.yNum)));
        if (seen[key]) continue;
        seen[key] = true;

        const ctxStart = Math.max(0, i - 4);
        const ctxEnd = Math.min(lines.length - 1, i + 4);
        const context = lines.slice(ctxStart, ctxEnd + 1).join(' | ');
        const flatContext = String(context || '').replace(/\|/g, ' ').replace(/\s+/g, ' ').trim();

        let line4 = parseJtacLineValue(context, 4);
        let line5 = parseJtacLineValue(context, 5);
        let line6 = parseJtacLineValue(context, 6);
        let line7 = parseJtacLineValue(context, 7);
        let line8 = parseJtacLineValue(context, 8);
        let line9 = parseJtacLineValue(context, 9);
        if (!line4 && !line5 && !line6 && !line7 && !line8 && !line9) {
          line4 = parseJtacLineValue(sourceText, 4);
          line5 = parseJtacLineValue(sourceText, 5);
          line6 = parseJtacLineValue(sourceText, 6);
          line7 = parseJtacLineValue(sourceText, 7);
          line8 = parseJtacLineValue(sourceText, 8);
          line9 = parseJtacLineValue(sourceText, 9);
        }
        line4 = line4 || String((burstFromLogs.lines || {})['4'] || (burstFromSource.lines || {})['4'] || '').trim();
        line5 = line5 || String((burstFromLogs.lines || {})['5'] || (burstFromSource.lines || {})['5'] || '').trim();
        line6 = line6 || String((burstFromLogs.lines || {})['6'] || (burstFromSource.lines || {})['6'] || '').trim();
        line7 = line7 || String((burstFromLogs.lines || {})['7'] || (burstFromSource.lines || {})['7'] || '').trim();
        line8 = line8 || String((burstFromLogs.lines || {})['8'] || (burstFromSource.lines || {})['8'] || '').trim();
        line9 = line9 || String((burstFromLogs.lines || {})['9'] || (burstFromSource.lines || {})['9'] || '').trim();
        const engagementMatch = flatContext.match(engagementTypeRegex)
          || line.match(engagementTypeRegex)
          || sourceText.match(engagementTypeRegex);
        const engagementType = engagementMatch
          ? ('TYPE ' + String(engagementMatch[1] || '').toUpperCase())
          : String(burstFromLogs.engagementType || burstFromSource.engagementType || '').trim();

        const rmkMatch = flatContext.match(/\bRMK\b\s*[:\-]?\s*(.*)$/i)
          || flatContext.match(/\bREMARKS?\b\s*[:\-]?\s*(.*)$/i)
          || flatContext.match(/\bPREF\b\s+(.+)$/i)
          || sourceText.match(/\bRMK\b\s*[:\-]?\s*(.*)$/im)
          || sourceText.match(/\bREMARKS?\b\s*[:\-]?\s*(.*)$/im)
          || sourceText.match(/\bPREF\b\s+(.+)$/im);
        const rmkText = rmkMatch
          ? String(rmkMatch[1] || '').replace(/\s+/g, ' ').trim()
          : String(burstFromLogs.rmkText || burstFromSource.rmkText || '').trim();
        const attackHeadingRange = parseJtacHeadingRangeFromText(rmkText);
        const attackHeadingStartDeg = attackHeadingRange ? Number(attackHeadingRange.startDeg) : NaN;
        const attackHeadingEndDeg = attackHeadingRange ? Number(attackHeadingRange.endDeg) : NaN;

        const typeMatch = context.match(targetRegex) || line.match(targetRegex);
        let unitType = '';
        if (typeMatch) {
          unitType = String(typeMatch[2] || typeMatch[3] || '').trim();
        }
        if (!unitType && line5) {
          unitType = String(line5).replace(/^tgt\s*/i, '').replace(/^target\s*/i, '').trim();
        }
        if (!unitType) {
          unitType = 'Unknown';
        }

        const elevMatch = line4.match(elevRegex) || context.match(elevRegex) || line.match(elevRegex);
        let elevationFeet = NaN;
        if (elevMatch) {
          const elevNum = Number(elevMatch[2]);
          const elevUnit = String(elevMatch[3] || 'ft').toLowerCase();
          if (isFinite(elevNum)) {
            elevationFeet = elevUnit.indexOf('m') === 0 ? (elevNum * 3.28084) : elevNum;
          }
        }

        let friendlyBearing = NaN;
        let friendlyRangeNm = NaN;
        let friendlyDistanceMeters = NaN;
        const friendSource = line8 || context;
        const fBraMatch = friendSource.match(friendBraRegex) || line.match(friendBraRegex);
        if (fBraMatch) {
          friendlyBearing = Number(fBraMatch[1]);
          friendlyRangeNm = Number(fBraMatch[2]);
          if (isFinite(friendlyRangeNm) && friendlyRangeNm > 0) {
            friendlyDistanceMeters = friendlyRangeNm * 1852.0;
          }
        } else {
          const fDirMatch = friendSource.match(friendDirectionRegex);
          if (fDirMatch) {
            const dirBearing = normalizeJtacDirectionToBearing(fDirMatch[1]);
            const distRaw = Number(fDirMatch[2]);
            const distUnit = String(fDirMatch[3] || 'm').toLowerCase();
            if (isFinite(dirBearing)) friendlyBearing = dirBearing;
            if (isFinite(distRaw) && distRaw > 0) {
              if (distUnit === 'nm') {
                friendlyRangeNm = distRaw;
                friendlyDistanceMeters = distRaw * 1852.0;
              } else if (distUnit === 'km') {
                friendlyRangeNm = distRaw * 0.5399568035;
                friendlyDistanceMeters = distRaw * 1000.0;
              } else {
                friendlyRangeNm = distRaw / 1852.0;
                friendlyDistanceMeters = distRaw;
              }
            }
          }
        }

        const egressSource = line9 || context;
        const egrMatch = egressSource.match(/\b(?:EGR|EGRESS)\b[^A-Z0-9]{0,6}(NORTHWEST|NORTHEAST|SOUTHWEST|SOUTHEAST|NORTH|SOUTH|EAST|WEST|NE|NW|SE|SW|N|S|E|W|\d{1,3})\b/i)
          || egressSource.match(/\b(NORTHWEST|NORTHEAST|SOUTHWEST|SOUTHEAST|NORTH|SOUTH|EAST|WEST|NE|NW|SE|SW|N|S|E|W|\d{1,3})\b/i);
        const egressHeadingDeg = egrMatch ? normalizeJtacDirectionToBearing(egrMatch[1]) : NaN;

        const nl4Text = line4 || (isFinite(elevationFeet) ? ('Elev ' + String(Math.round(elevationFeet)) + ' ft') : '');
        const nl5Text = line5 || ('TGT ' + unitType);
        const normalizedMgrsDisplay = formatMgrsDigraphDisplayFromParsed(mgrs);
        const nl6Text = normalizedMgrsDisplay ? ('MGRS ' + normalizedMgrsDisplay) : (line6 || '');
        const nl7Text = line7 || '';
        const nl8Text = line8 || (isFinite(friendlyBearing) && isFinite(friendlyDistanceMeters)
          ? ('Friendlies ' + formatHeadingDeg(friendlyBearing) + ' ' + String(Math.round(friendlyDistanceMeters)) + 'm')
          : '');
        const nl9Text = line9 || (isFinite(egressHeadingDeg) ? ('Egr ' + formatHeadingDeg(egressHeadingDeg)) : '');

        const row = {
          callsign: 'JTAC TGT',
          name: unitType,
          category: 'JTAC_TARGET',
          typeName: unitType,
          frequency: '',
          tacan: '',
          mpClientCallsign: '',
          altFeet: isFinite(elevationFeet) ? Math.max(0, Math.round(elevationFeet)) : 0,
          xNum: Number(dcs.xNum),
          yNum: Number(dcs.yNum),
          jtacTarget: true,
          mgrsText: mgrs.mgrsText,
          mgrsDisplayText: normalizedMgrsDisplay,
          unitType: unitType,
          elevationFeet: isFinite(elevationFeet) ? Math.round(elevationFeet) : NaN,
          label: 'JTAC TGT',
          rawLine: line,
          nl4Text: nl4Text,
          nl5Text: nl5Text,
          nl6Text: nl6Text,
          nl7Text: nl7Text,
          nl8Text: nl8Text,
          nl9Text: nl9Text,
          rmkText: rmkText,
          engagementType: engagementType,
          attackHeadingStartDeg: isFinite(attackHeadingStartDeg) ? normalizeHeadingDeg(attackHeadingStartDeg) : NaN,
          attackHeadingEndDeg: isFinite(attackHeadingEndDeg) ? normalizeHeadingDeg(attackHeadingEndDeg) : NaN,
          egressHeadingDeg: isFinite(egressHeadingDeg) ? normalizeHeadingDeg(egressHeadingDeg) : NaN,
        };

        if (isFinite(friendlyBearing) && isFinite(friendlyRangeNm) && friendlyRangeNm > 0) {
          row.friendlyBearing = Math.round(friendlyBearing);
          row.friendlyRangeNm = Number(friendlyRangeNm.toFixed(1));
        }
        if (isFinite(friendlyDistanceMeters) && friendlyDistanceMeters > 0) {
          row.friendlyDistanceMeters = Number(friendlyDistanceMeters.toFixed(1));
        }

        targets.push(row);
      }

      return targets;
    }

    function getJtacTargetIdentityKey(target) {
      const t = target || {};
      const mgrs = String(t.mgrsText || t.mgrsDisplayText || '').toUpperCase().trim();
      const xNum = Number(t.xNum);
      const yNum = Number(t.yNum);
      const nx = isFinite(xNum) ? Math.round(xNum / 10) * 10 : NaN;
      const ny = isFinite(yNum) ? Math.round(yNum / 10) * 10 : NaN;
      if (mgrs) return 'MGRS|' + mgrs;
      if (isFinite(nx) && isFinite(ny)) return 'XY|' + String(nx) + '|' + String(ny);
      const typeText = String(t.unitType || t.typeName || t.name || '').trim().toUpperCase();
      return 'TXT|' + typeText;
    }

    function getJtacOverlayMissionState(data) {
      const missionKey = String(getMissionIdentity(data || latestData || {}) || '').trim() || '__UNKNOWN__';
      if (!jtacOverlayStateByMission || typeof jtacOverlayStateByMission !== 'object') jtacOverlayStateByMission = {};
      const keep = {};
      keep[missionKey] = true;
      Object.keys(jtacOverlayStateByMission).forEach(function (k) {
        if (!keep[k]) delete jtacOverlayStateByMission[k];
      });
      let state = jtacOverlayStateByMission[missionKey];
      if (!state || typeof state !== 'object') {
        state = {
          missionKey: missionKey,
          activeKey: '',
          activeSeq: 0,
          nextSeq: 1,
          activeTarget: null,
          history: []
        };
        jtacOverlayStateByMission[missionKey] = state;
      }
      if (!Array.isArray(state.history)) state.history = [];
      if (!state.activeTarget || typeof state.activeTarget !== 'object') state.activeTarget = null;
      if (!isFinite(Number(state.nextSeq)) || Number(state.nextSeq) < 1) state.nextSeq = 1;
      return state;
    }

    function resolveJtacOverlayTargets(data, parsedTargets) {
      function mergeJtacPopupFields(primary, fallback) {
        const p = (primary && typeof primary === 'object') ? Object.assign({}, primary) : {};
        const f = (fallback && typeof fallback === 'object') ? fallback : {};
        ['nl4Text', 'nl5Text', 'nl6Text', 'nl7Text', 'nl8Text', 'nl9Text', 'rmkText', 'engagementType', 'mgrsText', 'mgrsDisplayText'].forEach(function (k) {
          const pv = String(p[k] || '').trim();
          const fv = String(f[k] || '').trim();
          if (!pv && fv) p[k] = fv;
        });
        ['friendlyBearing', 'friendlyRangeNm', 'friendlyDistanceMeters', 'attackHeadingStartDeg', 'attackHeadingEndDeg', 'egressHeadingDeg', 'elevationFeet'].forEach(function (k) {
          const pv = Number(p[k]);
          const fv = Number(f[k]);
          if (!isFinite(pv) && isFinite(fv)) p[k] = fv;
        });
        return p;
      }

      const rows = Array.isArray(parsedTargets) ? parsedTargets : [];
      const state = getJtacOverlayMissionState(data || latestData || {});
      if (!rows.length) {
        const heldActive = (state.activeTarget && typeof state.activeTarget === 'object')
          ? Object.assign({}, state.activeTarget, { jtacSequence: Number(state.activeSeq) || 1 })
          : null;
        return {
          active: heldActive,
          history: Array.isArray(state.history) ? state.history.slice() : []
        };
      }

      const activeRaw = rows[rows.length - 1] || null;
      const active = mergeJtacPopupFields(activeRaw, state.activeTarget);
      if (!active) {
        return { active: null, history: Array.isArray(state.history) ? state.history.slice() : [] };
      }

      const activeKey = getJtacTargetIdentityKey(active);
      if (!activeKey) {
        return { active: active, history: Array.isArray(state.history) ? state.history.slice() : [] };
      }

      if (state.activeKey && state.activeKey !== activeKey) {
        const priorSeq = Number(state.activeSeq);
        const seq = (isFinite(priorSeq) && priorSeq > 0) ? Math.round(priorSeq) : Math.round(Number(state.nextSeq) || 1);
        const priorFromRows = rows.find(function (t) { return getJtacTargetIdentityKey(t) === state.activeKey; }) || null;
        const prior = mergeJtacPopupFields(
          state.activeTarget && typeof state.activeTarget === 'object' ? state.activeTarget : null,
          priorFromRows
        );
        if (prior) {
          const historyKey = getJtacTargetIdentityKey(prior);
          const already = (state.history || []).some(function (h) { return String(h && h.key || '') === String(historyKey || ''); });
          if (!already) {
            state.history.push({
              key: historyKey,
              seq: seq,
              xNum: Number(prior.xNum),
              yNum: Number(prior.yNum),
              mgrsText: String(prior.mgrsText || '').toUpperCase().trim(),
              elevationFeet: Number(prior.elevationFeet),
              unitType: String(prior.unitType || prior.typeName || '').trim(),
              friendlyBearing: Number(prior.friendlyBearing),
              friendlyRangeNm: Number(prior.friendlyRangeNm),
              attackHeadingStartDeg: Number(prior.attackHeadingStartDeg),
              attackHeadingEndDeg: Number(prior.attackHeadingEndDeg),
              egressHeadingDeg: Number(prior.egressHeadingDeg),
              nl4Text: String(prior.nl4Text || '').trim(),
              nl5Text: String(prior.nl5Text || '').trim(),
              nl6Text: String(prior.nl6Text || '').trim(),
              nl7Text: String(prior.nl7Text || '').trim(),
              nl8Text: String(prior.nl8Text || '').trim(),
              nl9Text: String(prior.nl9Text || '').trim(),
              rmkText: String(prior.rmkText || '').trim(),
              engagementType: String(prior.engagementType || '').trim(),
            });
            if (state.history.length > 10) {
              state.history = state.history.slice(state.history.length - 10);
            }
          }
        }
      }

      if (!state.activeKey || state.activeKey !== activeKey) {
        state.activeKey = activeKey;
        state.activeSeq = Math.round(Number(state.nextSeq) || 1);
        state.nextSeq = Math.max(state.activeSeq + 1, 1);
      }
      state.activeTarget = Object.assign({}, active || {});

      return {
        active: Object.assign({}, active || {}, { jtacSequence: Number(state.activeSeq) || 1 }),
        history: Array.isArray(state.history) ? state.history.slice() : []
      };
    }

    function truncateText(value, maxLen) {
      const text = String(value || '');
      if (text.length <= maxLen) return text;
      return text.substring(0, Math.max(0, maxLen - 1)) + '…';
    }

    function pickClosestLines(lines, maxRows) {
      const arr = Array.isArray(lines) ? lines : [];
      const parsed = arr.map(function (line) {
        const text = String(line || '').trim();
        const m = text.match(/\b(\d{1,3})\s*NM\b/i);
        return { text: text, nm: m ? parseInt(m[1], 10) : 9999 };
      });

      parsed.sort(function (a, b) { return a.nm - b.nm; });
      return parsed.slice(0, Math.max(0, maxRows || 0)).map(function (x) { return x.text; });
    }

    function extractFreqAndUnit(line) {
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

    function normalizeBottomPanelKey(panel) {
      const key = String(panel || '').trim().toUpperCase();
      return (key === 'FREQ' || key === 'CMDS' || key === 'ASSETS' || key === 'WX') ? key : '';
    }

    function getExpandedBottomPanel(selected) {
      const state = getFlightPlanPlanState(selected);
      if (!state) return '';
      return normalizeBottomPanelKey(state.expandedBottomPanel);
    }

    function setExpandedBottomPanel(selected, panel) {
      const state = getFlightPlanPlanState(selected);
      if (!state) return;
      const key = normalizeBottomPanelKey(panel);
      state.expandedBottomPanel = key;
      if (key) {
        state.closingBottomPanel = '';
        state.closingBottomPanelUntilUtc = 0;
      }
    }

    function closeExpandedBottomPanelAnimated(selected) {
      const state = getFlightPlanPlanState(selected);
      if (!state) return false;
      const current = normalizeBottomPanelKey(state.expandedBottomPanel);
      if (!current) return false;
      state.closingBottomPanel = current;
      state.closingBottomPanelUntilUtc = Date.now() + overlayCloseHideDelayMs;
      state.expandedBottomPanel = '';
      return true;
    }

    function isClosingBottomPanel(selected, panel) {
      const state = getFlightPlanPlanState(selected);
      if (!state) return false;
      const until = Number(state.closingBottomPanelUntilUtc || 0);
      if (!isFinite(until) || until <= 0 || Date.now() >= until) {
        state.closingBottomPanel = '';
        state.closingBottomPanelUntilUtc = 0;
        return false;
      }
      return normalizeBottomPanelKey(state.closingBottomPanel) === normalizeBottomPanelKey(panel);
    }

    function getBottomPanelShellClass(selected, panel) {
      const key = normalizeBottomPanelKey(panel);
      if (!key) return 'fltPlanInfoShell';
      if (getExpandedBottomPanel(selected) === key) return 'fltPlanInfoShell expanded';
      if (isClosingBottomPanel(selected, key)) return 'fltPlanInfoShell closing';
      return 'fltPlanInfoShell';
    }

    function getBottomPanelClassSuffix(selected, panel) {
      const key = normalizeBottomPanelKey(panel);
      if (!key) return '';
      if (getExpandedBottomPanel(selected) === key) return ' expanded';
      if (isClosingBottomPanel(selected, key)) return ' closing';
      return '';
    }

    function getBottomPanelTitleAttrs(selected, panel) {
      const key = normalizeBottomPanelKey(panel);
      if (!key) return '';
      return ' data-bottom-panel-toggle="' + key + '" title="Click to expand/collapse panel"';
    }

    function formatFrequenciesBlockHtml(data, selected) {
      const laneData = getRuntimeFlightPlanLiveLaneData(data, selected);
      const atcRaw = pickClosestLines((laneData && laneData.unitsATC) || getMergedList(data && data.Units, 'ATC'), 4);
      const flightRaw = pickClosestLines((laneData && laneData.unitsFLIGHT) || getMergedList(data && data.Units, 'FLIGHT'), 4);
      const entries = atcRaw.concat(flightRaw)
        .map(extractFreqAndUnit)
        .filter(function (x) { return !!x; })
        .slice(0, 6);

      const rows = entries.map(function (e) {
        return '<tr><td style="width:48%;">' + escapeHtml(truncateText(e.unit, 34)) + '</td><td>' + escapeHtml(e.freq) + '</td></tr>';
      });
      if (!rows.length) { rows.push('<tr><td colspan="2">No frequency data.</td></tr>'); }

      const titleAttrs = getBottomPanelTitleAttrs(selected, 'FREQ');
      return '<div class="fltPlanInfoBlock"><div class="fltPlanInfoTitle"' + titleAttrs + '>FREQUENCIES</div><div class="fltPlanInfoBody"><table class="fltPlanInfoTable"><thead><tr><th>UNIT</th><th>UHF/VHF</th></tr></thead><tbody>' + rows.join('') + '</tbody></table></div></div>';
    }

    function formatAssetsBlockHtml(data, selected) {
      const laneData = getRuntimeFlightPlanLiveLaneData(data, selected);
      const tanker = pickClosestLines((laneData && laneData.unitsTANKER) || getMergedList(data && data.Units, 'TANKER'), 3);
      const awacs = pickClosestLines((laneData && laneData.unitsAWACS) || getMergedList(data && data.Units, 'AWACS'), 2);
      const jtac = pickClosestLines((laneData && laneData.unitsJTAC) || getMergedList(data && data.Units, 'JTAC'), 2);
      const all = tanker.concat(awacs).concat(jtac).slice(0, 7);

      const rows = all.map(function (line) {
        return '<tr><td>' + escapeHtml(truncateText(line, 88)) + '</td></tr>';
      });
      if (!rows.length) { rows.push('<tr><td>No asset data.</td></tr>'); }

      const titleAttrs = getBottomPanelTitleAttrs(selected, 'ASSETS');
      return '<div class="fltPlanInfoBlock"><div class="fltPlanInfoTitle"' + titleAttrs + '>ASSETS</div><div class="fltPlanInfoBody"><table class="fltPlanInfoTable"><thead><tr><th>CALLSIGN / UNIT / FREQ / TACAN</th></tr></thead><tbody>' + rows.join('') + '</tbody></table></div></div>';
    }

    function formatMetarBlockHtml(data, selected) {
      const server = (data && data.Server) || {};
      const laneData = getRuntimeFlightPlanLiveLaneData(data, selected);
      const metars = (laneData && laneData.atcMetars) || server.AtcMetars || {};
      const useInHg = laneData ? !!laneData.metarUseInHg : !!metarPressureInHg;
      const atcLines = pickClosestLines((laneData && laneData.unitsATC) || getMergedList(data && data.Units, 'ATC'), 8);
      const keys = [];
      const seen = {};

      atcLines.forEach(function (line) {
        const k = resolveAtcMetarKey(line, metars);
        if (!k || seen[k]) return;
        seen[k] = true;
        keys.push(k);
      });

      if (keys.length < 4) {
        Object.keys(metars).forEach(function (k) {
          if (keys.length >= 4) return;
          if (seen[k]) return;
          seen[k] = true;
          keys.push(k);
        });
      }

      const rows = keys.map(function (k) {
        const text = String(metars[k] || '').trim();
        const converted = formatMetarForSelectedUnits(text, useInHg);
        return '<tr><td>' + escapeHtml(truncateText(converted, 100)) + '</td></tr>';
      });
      if (!rows.length) { rows.push('<tr><td>No METAR data.</td></tr>'); }

      const titleAttrs = getBottomPanelTitleAttrs(selected, 'WX');
      return '<div class="fltPlanInfoBlock"><div class="fltPlanInfoTitle"' + titleAttrs + '>WX</div><div class="fltPlanInfoBody"><table class="fltPlanInfoTable"><thead><tr><th>METAR</th></tr></thead><tbody>' + rows.join('') + '</tbody></table></div></div>';
    }

    function estimateCruiseKcasForAltitude(altFeet) {
      const alt = Number(altFeet);
      if (!isFinite(alt)) return 380;
      if (alt > 28000) {
        const gs = 0.85 * 661.47;
        const cas = gs / (1.0 + (alt / 100000.0));
        return Math.max(200, Math.round(cas));
      }
      return 380;
    }

    function estimateRepresentativeGroundSpeedKnots(waypoints) {
      const rows = Array.isArray(waypoints) ? waypoints : [];
      const samples = [];

      for (let i = 1; i < rows.length; i++) {
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
      const total = samples.reduce(function (acc, v) { return acc + v; }, 0);
      return total / samples.length;
    }

    function estimateRepresentativeKcas(waypoints) {
      const rows = Array.isArray(waypoints) ? waypoints : [];
      const samples = [];
      rows.forEach(function (wp) {
        const v = Number(wp && wp.spd);
        if (isFinite(v) && v > 0) samples.push(v);
      });
      if (!samples.length) return 300;
      const total = samples.reduce(function (acc, x) { return acc + x; }, 0);
      return Math.max(120, Math.min(750, total / samples.length));
    }

    function applySpeedAdjustmentsToWaypoints(waypoints, selected) {
      const rows = Array.isArray(waypoints) ? waypoints : [];
      if (!rows.length) return rows;
      const state = getFlightPlanPlanState(selected);

      rows.forEach(function (wp) {
        const base = Number(wp.spd);
        const adjustment = getWaypointSpeedAdjustment(state, wp.step);
        if (!isFinite(base)) {
          wp.spd = '-';
          return;
        }
        wp.spd = String(Math.max(80, Math.round(base + adjustment)));
      });

      return rows;
    }

    function applyLockedTotPlan(waypoints, selected) {
      const rows = Array.isArray(waypoints) ? waypoints : [];
      if (!rows.length) return rows;

      const state = getFlightPlanPlanState(selected);
      const lockedStep = stepToKey(state.lockedStep);
      const totSec = Number(state.totSeconds);
      if (!lockedStep || !isFinite(totSec)) return rows;

      const targetIndex = rows.findIndex(function (wp) { return stepToKey(wp.step) === lockedStep; });
      if (targetIndex < 0) return rows;

      const lockedEta = parseEtaToSeconds(rows[targetIndex].etaDisplay || rows[targetIndex].eta);
      if (!isFinite(lockedEta)) return rows;

      const delta = Math.round(totSec - lockedEta);
      if (!isFinite(delta) || delta === 0) return rows;

      for (let i = 0; i < rows.length; i++) {
        const curr = rows[i];
        const eta = parseEtaToSeconds(curr.etaDisplay || curr.eta);
        if (isFinite(eta)) {
          curr.etaDisplay = formatSecondsToClock(eta + delta);
        }
      }

      const takeoffSec = getTakeoffTimeBySelection(selected);
      if (isFinite(takeoffSec)) {
        const startKey = getFlightPlanEtaStartKey(selected);
        if (startKey) {
          fltPlanEtaStartBySelection[startKey] = takeoffSec + delta;
        }
      }

      return rows;
    }

    function applyStartLegPlan(startRow, waypoints) {
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
      if (speedCas !== '-') {
        startRow.spd = speedCas;
        first.spd = speedCas;
      }
    }

    function getRouteWaypoints(route) {
      const r = (route && typeof route === 'object') ? route : {};
      const wpKeys = Object.keys(r)
        .filter(function (k) { return /^\d+$/.test(String(k)); })
        .sort(function (a, b) { return parseInt(a, 10) - parseInt(b, 10); });

      return wpKeys.map(function (wk) {
        const wp = r[wk] || {};
        const typeText = String(wp.type || wp.action || 'WP');
        const altMeters = Number(wp.alt);
        const altFeetRaw = isFinite(altMeters) ? (altMeters * 3.28084) : NaN;
        const altValue = isFinite(altFeetRaw) ? (Math.round(altFeetRaw / 500) * 500) : NaN;
        const speedValue = Number(wp.speed);
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
          spd: isFinite(speedValue) && speedValue > 0 ? String(Math.round(speedValue)) : '-',
          x: isFinite(Number(wp.x)) ? String(Math.round(Number(wp.x))) : '-',
          y: isFinite(Number(wp.y)) ? String(Math.round(Number(wp.y))) : '-',
          xNum: Number(wp.x),
          yNum: Number(wp.y)
        };
      });
    }

    function applyEtaPlanToWaypoints(waypoints, selected) {
      const rows = Array.isArray(waypoints) ? waypoints : [];
      if (!rows.length) return rows;

      rows.forEach(function (wp) {
        const current = Number(wp && wp.spd);
        if (isFinite(current) && current > 0) {
          wp.spd = String(Math.round(current));
          return;
        }
        wp.spd = String(estimateCruiseKcasForAltitude(wp.altFeet));
      });

      return rows;
    }

    function applyRouteTimeline(rows, selected) {
      const list = Array.isArray(rows) ? rows : [];
      if (!list.length) return list;

      const state = getFlightPlanPlanState(selected);
      ensureDirectToStateValid(list, state);
      const skippedSet = getSkippedStepSet(list, state);
      const routeRows = getEffectiveRouteRows(list, state).filter(function (wp) {
        const key = stepToKey(wp && wp.step);
        return key && !skippedSet[key];
      });

      const etaMode = hasTakeoffTimeBySelection(selected);
      const baseSeconds = etaMode ? getTakeoffTimeBySelection(selected) : 0;
      let elapsed = 0;

      list[0].etaDisplay = formatSecondsToClock(baseSeconds);

      list.forEach(function (wp, idx) {
        if (!wp || idx === 0 || wp.isStart) return;
        wp.etaDisplay = '-';
      });

      let prev = (list[0] && list[0].isStart) ? list[0] : null;
      let routeStartIndex = 0;
      if (!prev && routeRows.length) {
        routeRows[0].etaDisplay = formatSecondsToClock(baseSeconds);
        prev = routeRows[0];
        routeStartIndex = 1;
      }

      for (let i = routeStartIndex; i < routeRows.length; i++) {
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

    function applyHeadingPlan(rows, theater, selected) {
      const list = Array.isArray(rows) ? rows : [];
      if (!list.length) return list;

      const state = getFlightPlanPlanState(selected);
      ensureDirectToStateValid(list, state);
      const skippedSet = getSkippedStepSet(list, state);
      const routeRows = getEffectiveRouteRows(list, state).filter(function (wp) {
        const key = stepToKey(wp && wp.step);
        return key && !skippedSet[key];
      });

      list.forEach(function (wp, idx) {
        if (!wp || idx === 0 || wp.isStart) return;
        wp.hdg = '-';
      });

      if (!routeRows.length) return list;

      const magVar = Number(getApproxMagVariationDeg(theater));

      const includeStart = !!(list[0] && list[0].isStart);
      if (includeStart && routeRows.length > 0) {
        const firstHdg = computeTrueHeadingDeg(list[0], routeRows[0]);
        const firstMag = isFinite(firstHdg) ? normalizeHeadingDeg(firstHdg - magVar) : NaN;
        list[0].hdg = formatHeadingDeg(firstMag);
      }

      for (let i = 0; i < routeRows.length; i++) {
        const curr = routeRows[i];
        let trueHdg = NaN;

        if (i === 0) {
          if (includeStart) {
            trueHdg = computeTrueHeadingDeg(list[0], curr);
          } else if (routeRows.length > 1) {
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

    function changeWaypointAltitude(selected, step, deltaFeet) {
      const state = getFlightPlanPlanState(selected);
      if (!state.altAdjustments) state.altAdjustments = {};
      const key = stepToKey(step);
      if (!key) return;
      const current = Number(state.altAdjustments[key]);
      const next = (isFinite(current) ? current : 0) + Number(deltaFeet || 0);
      state.altAdjustments[key] = clamp(next, -40000, 40000);
    }

    function getAltitudeAdjustmentStep(altFeet) {
      const alt = Number(altFeet);
      if (!isFinite(alt)) return 500;
      return alt < 1000 ? 100 : 500;
    }

    function changeWaypointAltitudeByDirection(selected, step, direction, currentAltFeet, displayMode) {
      const dir = Number(direction);
      if (!isFinite(dir) || dir === 0) return;
      const mode = String(displayMode || 'ft').toLowerCase();
      const stepSizeDisplay = getAltitudeAdjustmentStepByMode(currentAltFeet, mode);
      const stepSize = convertAltitudeDisplayDeltaToFeet(stepSizeDisplay, mode);
      const baseAlt = Number(currentAltFeet);
      if (!isFinite(baseAlt)) {
        changeWaypointAltitude(selected, step, (dir >= 0 ? 1 : -1) * stepSize);
        return;
      }

      const baseDisplay = mode === 'm' ? (baseAlt * 0.3048) : baseAlt;
      const lowerDisplay = Math.floor(baseDisplay / stepSizeDisplay) * stepSizeDisplay;
      const upperDisplay = Math.ceil(baseDisplay / stepSizeDisplay) * stepSizeDisplay;
      const isAligned = Math.abs(baseDisplay - Math.round(baseDisplay / stepSizeDisplay) * stepSizeDisplay) < 0.0001;

      let nextDisplay = baseDisplay;
      if (dir >= 0) {
        nextDisplay = isAligned ? (baseDisplay + stepSizeDisplay) : upperDisplay;
      } else {
        nextDisplay = isAligned ? (baseDisplay - stepSizeDisplay) : lowerDisplay;
      }

      let nextAlt = mode === 'm' ? (nextDisplay / 0.3048) : nextDisplay;
      nextAlt = Math.max(0, Math.round(nextAlt));
      changeWaypointAltitude(selected, step, nextAlt - baseAlt);
    }

    function applyAltitudeAdjustments(rows, selected) {
      const list = Array.isArray(rows) ? rows : [];
      const state = getFlightPlanPlanState(selected);
      const map = state.altAdjustments || {};
      list.forEach(function (wp) {
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

    function applyDistancePlan(rows, selected) {
      const list = Array.isArray(rows) ? rows : [];
      if (!list.length) return list;

      const state = getFlightPlanPlanState(selected);
      ensureDirectToStateValid(list, state);
      const skippedSet = getSkippedStepSet(list, state);
      const routeRows = getEffectiveRouteRows(list, state).filter(function (wp) {
        const key = stepToKey(wp && wp.step);
        return key && !skippedSet[key];
      });

      list[0].dist = '-';

      list.forEach(function (wp, idx) {
        if (!wp || idx === 0 || wp.isStart) return;
        wp.dist = '-';
      });

      let prev = (list[0] && list[0].isStart) ? list[0] : null;
      for (let i = 0; i < routeRows.length; i++) {
        const curr = routeRows[i];
        if (!prev) {
          curr.dist = '-';
          prev = curr;
          continue;
        }
        curr.dist = formatDistanceNm(computeLegDistanceNm(prev, curr));
        prev = curr;
      }

      return list;
    }

    function setEtaStartNowForSelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      fltPlanEtaStartBySelection[key] = getCurrentFlightPlanClockSeconds();
    }

    function getTimingClockTextForAnchor(selected, anchorType) {
      const timing = getFlightPlanTimingDisplay(selected);
      const anchor = String(anchorType || '').toUpperCase();
      if (anchor === 'STEP') return String((timing && timing.step) || '-');
      if (anchor === 'START') return String((timing && timing.start) || '-');
      if (anchor === 'TAXI') return String((timing && timing.taxi) || '-');
      if (anchor === 'TAKEOFF') return String((timing && timing.takeoff) || '-');
      if (anchor === 'TOT') return String((timing && timing.tot) || '-');
      return '-';
    }

    function setTakeoffTimeByAnchorFromNow(selected, anchorType) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;

      const anchor = String(anchorType || '').toUpperCase();
      const now = getCurrentFlightPlanClockSeconds();
      const state = getFlightPlanPlanState(selected);
      const marks = getResolvedTimingMarks(selected);

      if (anchor === 'STEP') {
        marks.step = now;
        marks.start = now + stepToStartSeconds;
        marks.taxi = marks.start + startToTaxiSeconds;
        marks.takeoff = marks.taxi + taxiToTakeoffSeconds;
      } else if (anchor === 'START') {
        marks.start = now;
        marks.taxi = now + startToTaxiSeconds;
        marks.takeoff = marks.taxi + taxiToTakeoffSeconds;
      } else if (anchor === 'TAXI') {
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
      if (isFinite(Number(marks.takeoff))) {
        fltPlanEtaStartBySelection[key] = Number(marks.takeoff);
      }
      appendTimingLog(selected, anchor, marks);

      if (anchor === 'TAKEOFF' && isFinite(Number(marks.takeoff)) && !fakeMissionEnabled) {
        setMissionClockAnchor(Number(marks.takeoff), Date.now());
      }

      if (fakeMissionEnabled) {
        updateFakeOwnshipStartedState();
      }
    }

    function clearTakeoffTimeForSelection(selected) {
      const key = getFlightPlanEtaStartKey(selected);
      if (!key) return;
      const state = getFlightPlanPlanState(selected);
      delete fltPlanEtaStartBySelection[key];
      delete autoTakeoffBySelection[key];
      state.timeMarks = {};
      state.timingLog = [];
      state.ataByStep = {};
      state.speedRecommendations = {};
      state.totPerformanceByStep = {};
      state.lastOwnshipPos = null;
      state.autoAtaLastOwnshipPos = null;
      state.autoAtaLastCapturedStep = '';
      state.postFlightOpen = false;
    }

    function getFlightPlanStartRow(server, selected) {
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

    function findDtcWyptObject(value, depth) {
      if (depth > 10 || value === null || value === undefined) return null;
      if (Array.isArray(value)) {
        for (let i = 0; i < value.length; i++) {
          const found = findDtcWyptObject(value[i], depth + 1);
          if (found) return found;
        }
        return null;
      }
      if (typeof value !== 'object') return null;

      if (Array.isArray(value.NAV_PTS)) return value;
      if (value.WYPT && typeof value.WYPT === 'object' && Array.isArray(value.WYPT.NAV_PTS)) return value.WYPT;

      const keys = Object.keys(value);
      for (let i = 0; i < keys.length; i++) {
        const found = findDtcWyptObject(value[keys[i]], depth + 1);
        if (found) return found;
      }
      return null;
    }

    function isDtcPrimaryRouteSteerpoint(stepNum) {
      const n = Number(stepNum);
      return isFinite(n) && n >= 1 && n <= 25;
    }

    function getF14NavRoutes(root) {
      if (!root || typeof root !== 'object') return [];
      const nav = Array.isArray(root.NAV) ? root.NAV : [];
      return nav.filter(function (route) {
        if (!route || typeof route !== 'object') return false;
        return Array.isArray(route.waypoints)
          || Array.isArray(route.additional_points)
          || Array.isArray(route.lines);
      });
    }

    function getF14RouteSlots(root) {
      const routes = getF14NavRoutes(root);
      const slots = routes.slice(0, 12).map(function (route, idx) {
        return {
          key: 'R' + String(idx + 1),
          route: route
        };
      });
      return slots;
    }

    function getF14WaypointTypeInfo(rawName) {
      const original = String(rawName || '').trim();
      const upper = original.toUpperCase();
      const result = {
        name: original,
        typeRaw: 'WP',
        isBullseye: false,
      };

      if (!upper) return result;

      if (isBullseyeText(upper)) {
        result.typeRaw = 'BULL';
        result.isBullseye = true;
      }

      function markAndTrim(regex, typeRaw, extra) {
        const match = upper.match(regex);
        if (!match) return false;
        result.typeRaw = typeRaw;
        if (extra && typeof extra === 'object') {
          Object.keys(extra).forEach(function (k) { result[k] = extra[k]; });
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
      if (priorityMatch) {
        const level = Number(priorityMatch[1]);
        result.typeRaw = (level >= 1 && level <= 3) ? ('PRIO ' + String(level)) : 'WP';
        const trimmed = original.substring(0, priorityMatch.index).trim();
        if (trimmed) result.name = trimmed;
      }

      return result;
    }

    function getDtcCanonicalRouteModel(root) {
      if (!root || typeof root !== 'object') return null;

      function resolveDtcAircraftTypeText(value, depth) {
        if (depth > 12 || value === null || value === undefined) return '';
        if (Array.isArray(value)) {
          for (let i = 0; i < value.length; i++) {
            const found = resolveDtcAircraftTypeText(value[i], depth + 1);
            if (found) return found;
          }
          return '';
        }
        if (typeof value !== 'object') return '';

        const directType = String((value && value.type) || '').trim();
        const upperDirect = directType.toUpperCase();
        if (upperDirect.indexOf('F-16') >= 0 || upperDirect.indexOf('VIPER') >= 0
          || upperDirect.indexOf('FA-18') >= 0 || upperDirect.indexOf('HORNET') >= 0
          || upperDirect.indexOf('AH-64') >= 0 || upperDirect.indexOf('APACHE') >= 0
          || upperDirect.indexOf('F-14') >= 0 || upperDirect.indexOf('TOMCAT') >= 0) {
          return upperDirect;
        }

        const keys = Object.keys(value);
        for (let i = 0; i < keys.length; i++) {
          const found = resolveDtcAircraftTypeText(value[keys[i]], depth + 1);
          if (found) return found;
        }
        return '';
      }

      const sourceType = resolveDtcAircraftTypeText(root, 0) || String((root && root.type) || '').toUpperCase();

      function isMeterBasedDtcAltitudeSource() {
        return sourceType.indexOf('FA-18') >= 0
          || sourceType.indexOf('HORNET') >= 0
          || sourceType.indexOf('F-16') >= 0
          || sourceType.indexOf('VIPER') >= 0
          || sourceType.indexOf('AH-64') >= 0
          || sourceType.indexOf('APACHE') >= 0;
      }

      function isKmhBasedDtcSpeedSource() {
        return sourceType.indexOf('FA-18') >= 0
          || sourceType.indexOf('HORNET') >= 0
          || sourceType.indexOf('F-16') >= 0
          || sourceType.indexOf('VIPER') >= 0
          || sourceType.indexOf('AH-64') >= 0
          || sourceType.indexOf('APACHE') >= 0;
      }

      function normalizeAltFeet(rawAlt) {
        const alt = Number(rawAlt);
        if (!isFinite(alt)) return NaN;
        return isMeterBasedDtcAltitudeSource() ? (alt * 3.28084) : alt;
      }

      function normalizeSpeedKnots(rawSpeed) {
        const spd = Number(rawSpeed);
        if (!isFinite(spd)) return NaN;
        const looksLikeKmh = spd > 420 && spd < 1800;
        return (isKmhBasedDtcSpeedSource() || looksLikeKmh) ? (spd / 1.852) : spd;
      }

      function boolish(v) {
        return v === true || String(v || '').toLowerCase() === 'true' || Number(v) === 1;
      }

      function normalizeStep(value) {
        const n = Number(value);
        if (!isFinite(n)) return NaN;
        const step = Math.round(n);
        return isDtcPrimaryRouteSteerpoint(step) ? step : NaN;
      }

      function normalizeAh64Step(value) {
        const n = Number(value);
        if (!isFinite(n)) return NaN;
        const step = Math.round(n);
        return step >= 1 && step <= 999 ? step : NaN;
      }

      function buildFromNavPtsModel() {
        const wypt = findDtcWyptObject(root, 0) || {};
        const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];
        if (!navPts.length) return null;

        const navRoute = Array.isArray(wypt.NAV_ROUTE) ? wypt.NAV_ROUTE : [];
        const routeKeys = ['R1', 'R2', 'R3'];

        const pointById = {};
        const pointByStep = {};
        navPts.forEach(function (p, idx) {
          const point = (p && typeof p === 'object') ? p : {};
          const id = String(point.id || ('STPT' + String(idx + 1))).trim().toUpperCase();
          const step = normalizeStep(point.wypt_num);
          const fallbackStep = normalizeStep(point.number);
          const finalStep = isFinite(step) ? step : fallbackStep;
          if (id) pointById[id] = point;
          if (isFinite(finalStep)) pointByStep[finalStep] = point;
        });

        const routes = routeKeys.map(function (routeKey, routeIdx) {
          const legsByStep = {};
          const orderByStep = {};
          const routeObj = (navRoute.length > routeIdx && navRoute[routeIdx] && typeof navRoute[routeIdx] === 'object')
            ? navRoute[routeIdx]
            : {};

          Object.keys(routeObj).forEach(function (idKeyRaw) {
            const idKey = String(idKeyRaw || '').toUpperCase();
            const rp = routeObj[idKeyRaw] || {};
            const navPoint = pointById[idKey] || {};
            const step = normalizeStep(rp.wypt_num);
            const fallbackStep = normalizeStep(navPoint.wypt_num);
            const rpNumberStep = normalizeStep(rp.number);
            const navPointNumberStep = normalizeStep(navPoint.number);
            const legStep = isFinite(step) ? step : fallbackStep;
            const finalLegStep = isFinite(legStep)
              ? legStep
              : (isFinite(rpNumberStep) ? rpNumberStep : navPointNumberStep);
            if (!isFinite(finalLegStep)) return;

            const altRaw = isFinite(Number(rp.routeAltitude)) ? Number(rp.routeAltitude)
              : (isFinite(Number(rp.alt)) ? Number(rp.alt)
                : (isFinite(Number(navPoint.routeAltitude)) ? Number(navPoint.routeAltitude)
                  : (isFinite(Number(navPoint.alt)) ? Number(navPoint.alt) : Number(navPoint.altitude))));
            const speedRaw = isFinite(Number(rp.speed)) ? Number(rp.speed)
              : (isFinite(Number(navPoint.speed)) ? Number(navPoint.speed) : NaN);
            const etaRaw = isFinite(Number(rp.ETA)) ? Number(rp.ETA)
              : (isFinite(Number(navPoint.ETA)) ? Number(navPoint.ETA) : Number(navPoint.TOS));
            const orderKey = routeKey + '_order';
            const routeOrder = isFinite(Number(rp[orderKey])) ? Number(rp[orderKey])
              : (isFinite(Number(navPoint[orderKey])) ? Number(navPoint[orderKey]) : NaN);

            const leg = {
              routeKey: routeKey,
              waypointId: idKey,
              step: finalLegStep,
              order: isFinite(routeOrder) ? routeOrder : finalLegStep,
              altRaw: altRaw,
              altFeet: normalizeAltFeet(altRaw),
              altType: String(rp.altitudeType || rp.alt_type || rp.altType || rp.alttype || navPoint.altitudeType || navPoint.alt_type || navPoint.altType || navPoint.alttype || ''),
              speedRaw: speedRaw,
              speed: normalizeSpeedKnots(speedRaw),
              speedType: String(navPoint.velocityType || rp.velocityType || ''),
              etaSeconds: etaRaw,
            };

            legsByStep[finalLegStep] = leg;
            if (isFinite(routeOrder)) orderByStep[finalLegStep] = routeOrder;
          });

          navPts.forEach(function (p, idx) {
            const point = (p && typeof p === 'object') ? p : {};
            if (!boolish(point[routeKey])) return;
            const step = normalizeStep(point.wypt_num);
            const fallbackStep = normalizeStep(point.number);
            const finalStep = isFinite(step) ? step : fallbackStep;
            if (!isFinite(finalStep)) return;
            if (legsByStep[finalStep]) return;

            const id = String(point.id || ('STPT' + String(idx + 1))).trim().toUpperCase();
            const orderKey = routeKey + '_order';
            const routeOrder = isFinite(Number(point[orderKey])) ? Number(point[orderKey]) : step;
            const altRaw = isFinite(Number(point.routeAltitude)) ? Number(point.routeAltitude)
              : (isFinite(Number(point.alt)) ? Number(point.alt) : Number(point.altitude));
            const speedRaw = isFinite(Number(point.speed)) ? Number(point.speed) : NaN;
            const etaRaw = isFinite(Number(point.ETA)) ? Number(point.ETA) : Number(point.TOS);

            legsByStep[finalStep] = {
              routeKey: routeKey,
              waypointId: id,
              step: finalStep,
              order: routeOrder,
              altRaw: altRaw,
              altFeet: normalizeAltFeet(altRaw),
              altType: String(point.altitudeType || point.alt_type || point.altType || point.alttype || ''),
              speedRaw: speedRaw,
              speed: normalizeSpeedKnots(speedRaw),
              speedType: String(point.velocityType || ''),
              etaSeconds: etaRaw,
            };
            orderByStep[finalStep] = routeOrder;
          });

          const legs = Object.keys(legsByStep)
            .map(function (k) { return legsByStep[k]; })
            .filter(function (leg) { return !!leg && isFinite(Number(leg.step)); })
            .sort(function (a, b) {
              const ao = Number(a && a.order);
              const bo = Number(b && b.order);
              if (isFinite(ao) && isFinite(bo) && ao !== bo) return ao - bo;
              return Number(a && a.step) - Number(b && b.step);
            });

          return {
            key: routeKey,
            name: routeKey,
            legs: legs,
          };
        });

        return {
          kind: 'NAV',
          sourceType: sourceType,
          routes: routes,
          pointsByStep: pointByStep,
          pointsById: pointById,
        };
      }

      function collectRouteContainers(value, depth, out) {
        if (depth > 12 || value === null || value === undefined) return;
        if (Array.isArray(value)) {
          for (let i = 0; i < value.length; i++) collectRouteContainers(value[i], depth + 1, out);
          return;
        }
        if (typeof value !== 'object') return;

        if (Array.isArray(value.Routes)) {
          out.push(value);
        }

        const keys = Object.keys(value);
        for (let i = 0; i < keys.length; i++) {
          collectRouteContainers(value[keys[i]], depth + 1, out);
        }
      }

      function buildFromAh64Model() {
        if (sourceType.indexOf('AH-64') < 0 && sourceType.indexOf('APACHE') < 0) return null;

        const containers = [];
        collectRouteContainers(root, 0, containers);
        if (!containers.length) return null;

        function scoreContainer(c) {
          const routes = Array.isArray(c && c.Routes) ? c.Routes : [];
          let enabledLegs = 0;
          for (let i = 0; i < routes.length; i++) {
            const r = routes[i] || {};
            const pts = Array.isArray(r.POINTS) ? r.POINTS : [];
            if (r.isEnabled === false) continue;
            enabledLegs += pts.length;
          }
          return enabledLegs;
        }

        let best = containers[0];
        let bestScore = scoreContainer(best);
        for (let i = 1; i < containers.length; i++) {
          const s = scoreContainer(containers[i]);
          if (s > bestScore) {
            best = containers[i];
            bestScore = s;
          }
        }

        const pointsRoot = (best && best.Points && typeof best.Points === 'object') ? best.Points : {};
        const routesRaw = Array.isArray(best && best.Routes) ? best.Routes : [];

        const pointByNum = {};
        const pointByText = {};
        Object.keys(pointsRoot).forEach(function (bucketKey) {
          const bucket = pointsRoot[bucketKey] || {};
          const rows = Array.isArray(bucket.POINTS) ? bucket.POINTS : [];
          rows.forEach(function (p) {
            const point = (p && typeof p === 'object') ? p : {};
            const num = Number(point.num);
            if (isFinite(num)) pointByNum[Math.round(num)] = point;
            const txt = String(point.text || '').trim().toUpperCase();
            if (txt) pointByText[txt] = point;
          });
        });

        const routes = routesRaw.slice(0, 12).map(function (routeRaw, idx) {
          const route = (routeRaw && typeof routeRaw === 'object') ? routeRaw : {};
          const routeKey = 'R' + String(idx + 1);
          const routeName = String(route.Name || route.name || routeKey).trim() || routeKey;
          const routePoints = Array.isArray(route.POINTS) ? route.POINTS : [];

          const legs = routePoints
            .map(function (rp, pointIdx) {
              const point = (rp && typeof rp === 'object') ? rp : {};
              const step = normalizeAh64Step(point.num);
              if (!isFinite(step)) return null;
              const catalog = pointByNum[step] || pointByText[String(point.text || '').trim().toUpperCase()] || {};
              const waypointId = String(catalog.id || point.id || point.text || ('W' + String(step))).trim().toUpperCase();
              const altRaw = isFinite(Number(point.alt)) ? Number(point.alt) : Number(catalog.alt);
              const speedRaw = isFinite(Number(point.speed)) ? Number(point.speed) : Number(catalog.speed);
              const etaRaw = isFinite(Number(point.eta)) ? Number(point.eta) : Number(point.ETA);

              return {
                routeKey: routeKey,
                waypointId: waypointId,
                step: step,
                order: pointIdx + 1,
                altRaw: altRaw,
                altFeet: normalizeAltFeet(altRaw),
                altType: String(point.altitudeType || point.alt_type || point.altType || point.alttype || catalog.altitudeType || catalog.altType || ''),
                speedRaw: speedRaw,
                speed: normalizeSpeedKnots(speedRaw),
                speedType: String(point.velocityType || catalog.velocityType || ''),
                etaSeconds: etaRaw,
                distanceMeters: isFinite(Number(point.dist)) ? Number(point.dist) : NaN,
                fix: !!point.fix,
              };
            })
            .filter(function (leg) { return !!leg; });

          return {
            key: routeKey,
            name: routeName,
            legs: legs,
            isEnabled: route.isEnabled !== false,
          };
        });

        const waypointByStep = {};
        routes.forEach(function (route) {
          const legs = Array.isArray(route && route.legs) ? route.legs : [];
          legs.forEach(function (leg) {
            const step = Number(leg && leg.step);
            if (!isFinite(step)) return;
            if (waypointByStep[step]) return;

            const catalog = pointByNum[step] || {};
            const nameText = String(catalog.text || catalog.note || leg.waypointId || ('W' + String(step))).trim();
            const xNum = Number(catalog.x);
            const yNum = Number(catalog.y);
            waypointByStep[step] = {
              step: String(step),
              dtcId: String(leg.waypointId || '').toUpperCase(),
              type: 'WP',
              typeRaw: 'WP',
              name: nameText,
              alt: isFinite(Number(leg.altFeet)) ? String(Math.round(Number(leg.altFeet))) : '-',
              altFeet: Number(leg.altFeet),
              altType: String(leg.altType || ''),
              eta: formatEtaSeconds(Number(leg.etaSeconds)),
              etaSourceSeconds: Number(leg.etaSeconds),
              spd: isFinite(Number(leg.speed)) ? String(Math.round(Number(leg.speed))) : '-',
              speedType: String(leg.speedType || ''),
              x: isFinite(xNum) ? String(Math.round(xNum)) : '-',
              y: isFinite(yNum) ? String(Math.round(yNum)) : '-',
              xNum: xNum,
              yNum: yNum,
            };
          });
        });

        const waypoints = Object.keys(waypointByStep)
          .map(function (k) { return waypointByStep[k]; })
          .sort(function (a, b) { return Number(a && a.step) - Number(b && b.step); });

        const hasAnyLegs = routes.some(function (r) {
          return Array.isArray(r && r.legs) && r.legs.length > 0;
        });
        if (!hasAnyLegs) return null;

        return {
          kind: 'AH64',
          sourceType: sourceType,
          routes: routes,
          waypoints: waypoints,
        };
      }

      const prefersAh64Model = sourceType.indexOf('AH-64') >= 0 || sourceType.indexOf('APACHE') >= 0;
      if (prefersAh64Model) {
        const ah64FirstModel = buildFromAh64Model();
        if (ah64FirstModel) return ah64FirstModel;
      }

      const navModel = buildFromNavPtsModel();
      if (navModel) return navModel;

      const ah64Model = buildFromAh64Model();
      if (ah64Model) return ah64Model;

      return null;
    }

    function getDtcWaypoints(root) {
      function isAh64DtcSource(modelRoot) {
        const t = String((modelRoot && modelRoot.type) || '').toUpperCase();
        return t.indexOf('AH-64') >= 0 || t.indexOf('APACHE') >= 0;
      }

      function isMeterBasedDtcAltitudeSource(modelRoot) {
        const t = String((modelRoot && modelRoot.type) || '').toUpperCase();
        return t.indexOf('FA-18') >= 0
          || t.indexOf('HORNET') >= 0
          || t.indexOf('F-16') >= 0
          || t.indexOf('VIPER') >= 0;
      }

      function normalizeDtcAltitudeFeet(rawAlt) {
        const alt = Number(rawAlt);
        if (!isFinite(alt)) return NaN;
        return isMeterBasedDtcAltitudeSource(root) ? (alt * 3.28084) : alt;
      }

      const wypt = findDtcWyptObject(root, 0) || {};
      const navPts = Array.isArray(wypt.NAV_PTS) ? wypt.NAV_PTS : [];
      const navRoute = Array.isArray(wypt.NAV_ROUTE) ? wypt.NAV_ROUTE : [];
      const primaryRoute = (navRoute.length && navRoute[0] && typeof navRoute[0] === 'object') ? navRoute[0] : {};
      const hasExplicitNavPts = Array.isArray(wypt.NAV_PTS);

      const routeById = {};
      Object.keys(primaryRoute).forEach(function (k) {
        const point = primaryRoute[k];
        if (!point || typeof point !== 'object') return;
        routeById[String(k).toUpperCase()] = point;
      });

      if (hasExplicitNavPts && !isAh64DtcSource(root)) {
        if (!navPts.length) return [];

        return navPts.slice(0, 200).map(function (p, idx) {
          const point = (p && typeof p === 'object') ? p : {};
          const id = String(point.id || ('STPT' + String(idx + 1))).trim();
          const routePoint = routeById[id.toUpperCase()] || {};

          const etaNum = isFinite(Number(routePoint.ETA)) ? Number(routePoint.ETA) : (isFinite(Number(point.ETA)) ? Number(point.ETA) : Number(point.TOS));
          const altRaw = isFinite(Number(routePoint.alt)) ? Number(routePoint.alt) : (isFinite(Number(point.alt)) ? Number(point.alt) : (isFinite(Number(point.routeAltitude)) ? Number(point.routeAltitude) : Number(point.altitude)));
          const altNum = normalizeDtcAltitudeFeet(altRaw);
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
            dtcId: id.toUpperCase(),
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
        }).filter(function (wp) { return !!wp; });
      }

      const canonicalModel = getDtcCanonicalRouteModel(root);
      if (canonicalModel && canonicalModel.kind === 'AH64' && Array.isArray(canonicalModel.waypoints) && canonicalModel.waypoints.length) {
        return canonicalModel.waypoints.slice(0, 200);
      }

      const f14Slots = getF14RouteSlots(root);
      if (f14Slots.length) {
        const mapped = [];
        f14Slots.forEach(function (slot) {
          const route = (slot && slot.route && typeof slot.route === 'object') ? slot.route : {};
          const routeWaypoints = Array.isArray(route.waypoints) ? route.waypoints : [];
          routeWaypoints.slice(0, 200).forEach(function (point, idx) {
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

        if (mapped.length) {
          return mapped
            .sort(function (a, b) {
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
      const filtered = candidates.filter(function (p) {
        const point = (p && typeof p === 'object') ? p : {};
        const hasXY = isFinite(Number(point.x)) && isFinite(Number(point.y));
        const hasPointMeta = isFinite(Number(point.wypt_num)) || !!point.id || !!point.name || !!point.wp || !!point.waypoint || !!point.alt || !!point.altitude;
        const looksLikeComm = point.freq !== undefined || point.frequency !== undefined || point.Channel !== undefined || point.channel !== undefined || point.modulation !== undefined;
        return hasXY && hasPointMeta && !looksLikeComm;
      });

      filtered.sort(function (a, b) {
        const aw = Number(a && a.wypt_num);
        const bw = Number(b && b.wypt_num);
        if (isFinite(aw) && isFinite(bw) && aw !== bw) return aw - bw;
        return 0;
      });

      return filtered.slice(0, 200).map(function (point, idx) {
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
      }).filter(function (wp) { return !!wp; });
    }

