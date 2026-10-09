# VAICOM OKB Viewer (iPad MVP)

This folder contains a minimal SwiftUI app skeleton that wraps the existing VAICOM OKB Out dashboard in a native iPad app.

## MVP scope
- Fullscreen dashboard viewer (`WKWebView`)
- Host/IP + Port settings
- URL preview (`http://<host>:<port>/okb/`)
- Basic connection state + retry button
- Local network privacy support for iOS

## Structure
- `App/VAICOMOKBViewerApp.swift` - app entry point
- `App/ConnectionSettings.swift` - persisted host/port settings
- `App/ContentView.swift` - main viewer UI shell
- `App/SettingsView.swift` - settings form
- `App/WebView.swift` - `WKWebView` bridge for SwiftUI
- `Config/Info.plist.template.xml` - required privacy/network keys

## Xcode setup (later on Mac)
1. Create a new iOS App project in Xcode (SwiftUI + Swift).
2. Project name: `VAICOMOKBViewer`.
3. Replace generated Swift files with files from this `App/` folder.
4. Merge `Config/Info.plist.template.xml` keys into your app `Info.plist`.
5. Build and run on iPad.

## TestFlight flow
1. Join Apple Developer Program.
2. Create app record in App Store Connect.
3. Configure signing in Xcode.
4. Archive and upload.
5. Add internal/external testers in TestFlight.

## LAN requirements
- Host PC and iPad must be on the same network.
- Use LAN URL from VAICOM host log (example: `http://192.168.1.20:7779/okb/`).
- Ensure Windows Firewall allows the selected TCP port.
