# Copilot Instructions

## Project Guidelines
- User prefers avoiding builds/tests during diagnostics.
- User is okay with offering builds/tests going forward again.

## Command Reference
- Use the provided AH-64D George AI control reference as canonical behavior mapping when adding contextual George commands/macros.

## F-4E ICS Implementation
- For F-4E ICS hot mic implementation, ignore WSO ICS state entirely and use only pilot ICS switch state because WSO seat occupancy disables WSO functions.

## TX5 Intercom Implementation
- For TX5 intercom hot mic, allow Options and menu navigation commands (e.g., Take 1..12) without requiring PTT press.

## OpenKneeboard Implementation
- For OpenKneeboard FLT PLN tab, prefer a full-window, scrollable, table-like kneeboard layout and avoid large path/header blocks above route data.
- For OpenKneeboard DTC map overlays, GEO_LINES should be treated as route-agnostic and displayed regardless of selected route (R1/R2/R3).

## Jester Mods Installation
- For auto-installed Jester mods, set the Saved Games path to `\Saved Games\DCS_F4E\jester\mods` (and initialize under that), instead of the default DCS/DCS.openbeta folder mapping.

## COM Frequency Display
- For runtime COM frequency display, use strict three-decimal MHz formatting for module consistency (e.g., 305.000, 127.050).
