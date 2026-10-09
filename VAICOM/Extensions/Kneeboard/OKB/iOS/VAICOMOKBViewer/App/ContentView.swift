import SwiftUI

struct ContentView: View {
	@EnvironmentObject private var settings: ConnectionSettingsStore

	@State private var reloadToken = UUID()
	@State private var isLoading = false
	@State private var errorMessage: String?
	@State private var showingSettings = false

	var body: some View {
		ZStack {
			if let url = settings.dashboardURL {
				WebView(url: url, reloadToken: reloadToken, isLoading: $isLoading, errorMessage: $errorMessage)
					.ignoresSafeArea()
			} else {
				VStack(spacing: 12) {
					Image(systemName: "wifi.exclamationmark")
						.font(.system(size: 34, weight: .semibold))
					Text("VAICOM OKB Viewer")
						.font(.headline)
					Text("Add your host LAN IP and port to start.")
						.font(.footnote)
						.foregroundStyle(.secondary)
					Button("Connection Settings") {
						showingSettings = true
					}
					.buttonStyle(.borderedProminent)
				}
				.padding(20)
				.background(.thinMaterial)
				.clipShape(RoundedRectangle(cornerRadius: 14))
			}

			overlayView
		}
		.navigationTitle("VAICOM")
		.toolbar {
			ToolbarItemGroup(placement: .topBarTrailing) {
				Button {
					reloadToken = UUID()
				} label: {
					Image(systemName: "arrow.clockwise")
				}
				.disabled(settings.dashboardURL == nil)

				Button {
					showingSettings = true
				} label: {
					Label("Connection Settings", systemImage: "gearshape")
				}
			}
		}
		.sheet(isPresented: $showingSettings) {
			SettingsView()
				.environmentObject(settings)
		}
	}

	@ViewBuilder
	private var overlayView: some View {
		if settings.dashboardURL != nil && isLoading {
			VStack(spacing: 10) {
				ProgressView()
				Text("Connecting…")
					.font(.footnote)
					.foregroundStyle(.secondary)
				Button("Connection Settings") {
					showingSettings = true
				}
				.buttonStyle(.bordered)
			}
			.padding(14)
			.background(.ultraThinMaterial)
			.clipShape(RoundedRectangle(cornerRadius: 12))
		} else if let errorMessage {
			VStack(spacing: 10) {
				Text("Disconnected")
					.font(.headline)
				Text(errorMessage)
					.font(.footnote)
					.multilineTextAlignment(.center)
					.foregroundStyle(.secondary)
				HStack {
					Button("Retry") {
						reloadToken = UUID()
					}
					.buttonStyle(.borderedProminent)

					Button("Connection Settings") {
						showingSettings = true
					}
					.buttonStyle(.bordered)
				}
			}
			.padding(16)
			.background(.ultraThinMaterial)
			.clipShape(RoundedRectangle(cornerRadius: 12))
			.padding(.horizontal, 24)
		}
	}
}
