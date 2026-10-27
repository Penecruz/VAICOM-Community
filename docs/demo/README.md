# VAICOM Web Dashboard Demo

This folder supports three demo modes:

1. **GitHub-hosted static OKB shim** (recommended for review/demo)
2. **Local real-UI mock endpoint** (PowerShell HttpListener)
3. **Simple static dashboard page** (`index.html`)

## 1) GitHub-hosted static OKB shim (recommended)

The static shim serves the full OKB UI from `docs/demo/okb` and emulates `/okb/*` endpoints in-browser using mock data files in `docs/demo/mock-api/okb`.

### Open

- Root demo page: `https://penecruz.github.io/VAICOM-Community/`
- Simulated kneeboard: `https://penecruz.github.io/VAICOM-Community/okb/`

### What works in shim mode

- Tabbed OKB layout (including FLT PLN + EFB tabs)
- Mock FLT PLN selection using `MarianaIslands.lua`
- Mock `/okb/state`, `/okb/clock`, `/okb/sa/ownship`, `/okb/fltpln/avbus`
- Mock EFB airport/chart/history-track responses

## 2) Local real-UI mock endpoint mode

This mode serves the OKB UI from `VAICOM/Extensions/Kneeboard/OKB` and responds from `docs/demo/mock-api/okb` via local PowerShell HTTP routes.

### Start

1. Open PowerShell in `docs/demo/mock-endpoint`
2. If needed for this PowerShell window:
   - `Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass`
3. Run:
   - `./start-mock-endpoint.ps1 -Ip 0.0.0.0 -Port 8090`
4. If listener start fails, run one time in elevated PowerShell:
   - `netsh http add urlacl url=http://+:8090/ user=<your-windows-user>`

### Open

- Dashboard UI: `http://<your-pc-ip>:8090/okb/`
- State JSON: `http://<your-pc-ip>:8090/okb/state`

## 3) Simple static page mode

- `docs/demo/index.html`

This is the lightweight summary dashboard page and includes a button to open `./okb/`.

## GitHub Pages workflow

- Workflow: `.github/workflows/deploy-demo-pages.yml`
- Branch trigger: `master`
- Artifact path: `docs/demo`

## Notes

- `127.0.0.1` only works from the same machine.
- For iPad/LAN testing, use your PC LAN IP and allow the port through Windows Firewall.
- This is mock data only; no VoiceAttack/VAICOM runtime process is required.
- Edit payloads in `docs/demo/mock-api/okb/*.json` to change baseline mock values.
