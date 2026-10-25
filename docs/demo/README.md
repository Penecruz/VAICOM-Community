# VAICOM Web Dashboard Demo

This folder supports two demo modes:

1. A simple static demo page (`index.html`)
2. A real OKB tabbed UI demo served through a local mock endpoint

## 1) Local real-UI mock mode (recommended)

This mode serves the actual OKB UI files from:

- `VAICOM/Extensions/Kneeboard/OKB`

and responds with mock data from:

- `docs/demo/mock-api/okb`

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
- Clock JSON: `http://<your-pc-ip>:8090/okb/clock`

### Supported mock routes

- `/okb/` and `/okb/index.html`
- `/okb/css/*`, `/okb/js/*`, `/okb/assets/okb-samthreatranges.json`
- `/okb/state`, `/okb/index.json`
- `/okb/clock`
- `/okb/sa/ownship`
- `/okb/fltpln/avbus`
- `/okb/dtc/list`, `/okb/dtc/select`
- `/okb/efb/airports`, `/okb/efb/charts`, `/okb/efb/historytracks`
- `/okb/helpdoc`
- `/okb/dev/servermessages`, `/okb/dev/refresh`

## 2) Simple static page mode

You can still open:

- `docs/demo/index.html`

This is a lightweight standalone demo page and does not use the full OKB tabbed UI.

## 3) GitHub-hosted demo (master branch)

A GitHub Actions workflow is included:

- `.github/workflows/deploy-demo-pages.yml`

It publishes `docs/demo` to GitHub Pages when you push to the `master` branch.

### One-time GitHub setup

1. Push your `master` branch.
2. In GitHub repository settings, open **Pages**.
3. Set **Source** to **GitHub Actions**.
4. Run (or re-run) the **Deploy Demo to GitHub Pages** workflow.

### URL

After first successful deployment, your demo URL will be:

- `https://<org-or-user>.github.io/<repo>/`

For this repo, likely:

- `https://penecruz.github.io/VAICOM-Community/`

## Notes

- `127.0.0.1` only works from the same machine.
- For iPad/LAN testing, use your PC LAN IP and allow the port through Windows Firewall.
- This is mock data only; no VoiceAttack/VAICOM runtime process is required.
- Edit payloads in `docs/demo/mock-api/okb/*.json` to change displayed data.
- GitHub-hosted mode is static and does not run the local PowerShell mock server behavior.
