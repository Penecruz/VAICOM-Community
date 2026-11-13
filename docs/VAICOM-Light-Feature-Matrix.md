# VAICOM Light Feature Matrix (Phase 0 Scope)

## Goals
- Deliver a standalone Windows app for kneeboard-focused users.
- Reuse existing VAICOM UI and bridge logic wherever possible.
- Keep Full VAICOM packaging and installer unchanged.

## In Scope (Initial Light Release)
- Standalone Windows host process (`VAICOM.Light.exe`).
- Minimize-to-tray lifecycle.
- OpenKneeboard bridge hosting and status.
- Abbreviated UI with these tabs/pages:
  - OKB Out
  - Config
  - Help
  - About
  - Reset
- Light-specific update channel/artifact target.
- Shared semantic version with Full releases.

## Out of Scope (Initial Light Release)
- Voice command recognition core.
- VoiceAttack plugin/runtime integration.
- PTT-focused pages and TX controls.
- MP/Editor/Audio pages unless needed for reused dependencies.
- Bundling Light in Full installer or Full zip package.

## UI Tab Mapping
| Full UI Area | Light Status | Notes |
|---|---|---|
| OKB Out / Extensions page | Keep | Primary feature area |
| Config | Keep | Keep only settings required for bridge/kneeboard/runtime |
| Help | Keep | Include Light-specific startup/tray/update notes |
| About | Keep | Product identity and version |
| Reset | Keep | Keep safe reset/migration actions only |
| PTT | Remove | Voice/tx workflows out of scope |
| Preferences (voice-centric) | Remove/Trim | Re-add only if needed by shared controls |
| Editor | Remove | Not required for Light mission |
| MP | Remove | Not required for Light mission |
| Audio | Remove | Not required for Light mission |

## Runtime/Service Requirements
- Continue using provider-based runtime abstractions in OpenKneeboard bridge.
- Keep VoiceAttack-backed providers as default for Full app.
- Light app provides standalone runtime/settings providers.

## Distribution Rules
- Light built and shipped as separate artifact.
- Full installer/zip does not include Light executable.
- Light updater checks only Light channel metadata.
- Full updater checks only Full channel metadata.

## Versioning Rules
- Shared semantic version for Full and Light (same release tag).
- Distinct assets and update manifests per product target.

## Acceptance Criteria for Scope Completion
- `VAICOM.Light` project exists and launches.
- Window minimizes to tray and restores cleanly.
- Abbreviated tabs render and navigate.
- OpenKneeboard bridge starts/stops from Light host.
- Light updater resolves only Light artifact.
- Full build and behavior remain unchanged.
