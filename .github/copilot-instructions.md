# Copilot Instructions

## Project Guidelines
- User prefers avoiding builds/tests during diagnostics.
- User is okay with offering builds/tests going forward again.

## Command Reference
- Use the provided AH-64D George AI control reference as canonical behavior mapping when adding contextual George commands/macros.

## Coordinate Conversion
- For coordinate conversion in this repo, use `State.currentstate.theatre` names as canonical theater keys.
- Format MGRS as uppercase 10-digit (e.g., 'CA 12345 67890').
- Support both LAT/LON decimal and DMS display formats.
- Do not rely on DevPipe file at runtime; use it only as source data to build internal helpers.

## DCS Map Marker Panel
- DCS map marker panel entries in this project are expected to look like `{ idx, time, initiator, coalition, groupID, text, pos=vec3 }`, with `pos` as the runtime coordinate source.

## F-4E ICS Implementation
- For F-4E ICS hot mic implementation, ignore WSO ICS state entirely and use only pilot ICS switch state because WSO seat occupancy disables WSO functions.

## TX5 Intercom Implementation
- For TX5 intercom hot mic, allow Options and menu navigation commands (e.g., Take 1..12) without requiring PTT press.

## OpenKneeboard Implementation
- For OpenKneeboard FLT PLN tab, prefer a full-window, scrollable, table-like kneeboard layout and avoid large path/header blocks above route data.
- For OpenKneeboard DTC map overlays, GEO_LINES should be treated as route-agnostic and displayed regardless of selected route (R1/R2/R3).
- Disable text selection across the entire OpenKneeboard Out console UI to avoid messy selection highlights.
- For OpenKneeboard SA map integration, attribution is already auto-rendered in the map UI, so do not add an extra footer attribution block.

## F-14BU DTC Handling
- For F-14BU DTC handling, NAV[0] (Primary) must always map to Route 1 (R1), and runtime NAVLOG route is always R1; F-14BU DTC can contain up to 12 routes.
- For F-14BU FLT PLN behavior: when DCS runtime/Primary route (R1) is unavailable, populated NAV route data should remain only in its actual route slot (e.g., R2), while other routes (R3-R12) stay blank except ST position; CMDS section must display F-14BU CMDS program data.
- For F-14BU FLT PLN UI, show route buttons as R1 plus only routes that actually have route data (e.g., R1/R2/R3 if only R2 and R3 have data). In COM/ROUTE, always list all 12 routes and any waypoints they contain regardless of selected route button.

## F-14BU SA Map Overlays
- For F-14BU SA map overlays, NAV line markup should render as simple polyline overlays (like FAOR/FLOT), not as corridor boundary pairs.
- LANTIRN points should use small DMPI-style cross symbols; Defended Point (DP) and Hostile Area (HA) should render like threats with 20nm rings; Priority points should display labels P1/P2/P3.

## F-14BU Waypoint Token Semantics
- Support the following F-14BU waypoint token semantics: 
  - XFP = Fix Point
  - XIP = Initial Point
  - XST = Surface Target
  - XDP = Defended Point
  - XHA = Hostile Area
  - X##D/XD = Destination
  - X##L/XL = LANTIRN
  - X##B/XB = Bullseye
  - X1..X3 = Priority points
  - X4..X7 = Generic points

## F-14BU Elevation Wheel Behavior
- For F-14BU elevation wheel behavior, commands work without recipient but must also work with 'Jester' prefix; classification issues should be checked when recipient-prefixed recognition fails.

## Jester Mods Installation
- For auto-installed Jester mods, set the Saved Games path to `\Saved Games\DCS_F4E\jester\mods` (and initialize under that), instead of the default DCS/DCS.openbeta folder mapping.
- RIO/WIP content is reference-only for Jester behavior; actual deployable changes must be made in VAICOM/Resources/Files using the existing appended/original Jester patch swap files.

## COM Frequency Display
- For runtime COM frequency display, use strict three-decimal MHz formatting for module consistency (e.g., 305.000, 127.050).

## Code Clarity
- Add small step-by-step code notes/comments in complex command-flow methods for clarity.
