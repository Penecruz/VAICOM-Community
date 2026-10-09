import SwiftUI

struct ContentView: View {
	@EnvironmentObject private var settings: ConnectionSettingsStore

	@State private var reloadToken = UUID()
	@State private var isLoading = true
	@State private var errorMessage: String?
	@State private var showingSettings = false

	var body: some View {
		ZStack {
			if let url = settings.dashboardURL {
				WebView(url: url, reloadToken: reloadToken, isLoading: $isLoading, errorMessage: $errorMessage)
					.ignoresSafeArea()
			} else {
				ContentUnavailableView("Invalid Connection", systemImage: "wifi.exclamationmark", description: Text("Set a valid host and port in Settings."))
			}

			overlayView
		}
		.toolbar {
			ToolbarItemGroup(placement: .topBarTrailing) {
				Button {
					reloadToken = UUID()
				} label: {
					Image(systemName: "arrow.clockwise")
				}

				Button {
					showingSettings = true
				} label: {
					Image(systemName: "gearshape")
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
		if isLoading {
			VStack(spacing: 10) {
				ProgressView()
				Text("Connecting…")
					.font(.footnote)
					.foregroundStyle(.secondary)
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

					Button("Settings") {
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
