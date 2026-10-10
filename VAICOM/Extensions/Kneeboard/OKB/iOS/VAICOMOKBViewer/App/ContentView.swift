import SwiftUI

struct ContentView: View {
	@EnvironmentObject private var settings: ConnectionSettingsStore

	@State private var reloadToken = UUID()
	@State private var isLoading = false
	@State private var errorMessage: String?
	@State private var showingSettings = false
	@State private var isInitialConnecting = false
	@State private var hasConnected = false
	@State private var initialConnectionToken = UUID()

	private let retryTimer = Timer.publish(every: 5, on: .main, in: .common).autoconnect()

	var body: some View {
		let url = settings.dashboardURL
		let showNoConnectionCard = shouldShowNoConnectionCard(for: url)

		ZStack {
			if let url {
				WebView(url: url, reloadToken: reloadToken, isLoading: $isLoading, errorMessage: $errorMessage)
					.ignoresSafeArea()
			} else {
				Color(.systemBackground).ignoresSafeArea()
			}

			if showNoConnectionCard {
				noConnectionCard
			}

			overlayView

			VStack {
				HStack {
					Spacer()
					HStack(spacing: 10) {
						Button {
							reloadToken = UUID()
						} label: {
							Image(systemName: "arrow.clockwise")
						}
						.disabled(settings.dashboardURL == nil)

						Button {
							showingSettings = true
						} label: {
							Image(systemName: "gearshape")
						}
					}
					.padding(.horizontal, 12)
					.padding(.vertical, 10)
					.background(.thinMaterial)
					.clipShape(Capsule())
				}
				.padding(.top, 10)
				.padding(.trailing, 12)
				Spacer()
			}
		}
		.sheet(isPresented: $showingSettings) {
			SettingsView()
				.environmentObject(settings)
		}
		.onAppear {
			startInitialConnectionAttempt()
		}
		.onChange(of: settings.dashboardURL?.absoluteString ?? "") { _ in
			hasConnected = false
			errorMessage = nil
			startInitialConnectionAttempt()
		}
		.onChange(of: isLoading) { loading in
			if !loading, settings.dashboardURL != nil, errorMessage == nil {
				hasConnected = true
				isInitialConnecting = false
			}
		}
		.onChange(of: errorMessage) { message in
			if message != nil {
				hasConnected = false
			}
		}
		.onReceive(retryTimer) { _ in
			guard settings.dashboardURL != nil else { return }
			guard !hasConnected else { return }
			guard !isLoading else { return }
			reloadToken = UUID()
		}
	}

	@ViewBuilder
	private var overlayView: some View {
		if settings.dashboardURL != nil && isInitialConnecting && isLoading {
			VStack(spacing: 10) {
				ProgressView()
				Text("Connecting…")
					.font(.footnote)
					.foregroundStyle(.secondary)
			}
			.padding(14)
			.background(.ultraThinMaterial)
			.clipShape(RoundedRectangle(cornerRadius: 12))
		}
	}

	private var noConnectionCard: some View {
		VStack(spacing: 12) {
			Image(systemName: "wifi.exclamationmark")
				.font(.system(size: 34, weight: .semibold))
			Text("VAICOM OKB Viewer")
				.font(.headline)
			Text(settings.dashboardURL == nil ? "Add your host LAN IP and port to start." : "No connection. The app will keep trying using saved settings.")
				.font(.footnote)
				.foregroundStyle(.secondary)
				.multilineTextAlignment(.center)
			Button("Connection Settings") {
				showingSettings = true
			}
			.buttonStyle(.borderedProminent)
		}
		.padding(20)
		.background(.thinMaterial)
		.clipShape(RoundedRectangle(cornerRadius: 14))
		.padding(.horizontal, 24)
	}

	private func shouldShowNoConnectionCard(for url: URL?) -> Bool {
		if url == nil {
			return true
		}

		if hasConnected {
			return false
		}

		if isInitialConnecting {
			return false
		}

		return true
	}

	private func startInitialConnectionAttempt() {
		guard settings.dashboardURL != nil else {
			isInitialConnecting = false
			return
		}

		isInitialConnecting = true
		let token = UUID()
		initialConnectionToken = token
		reloadToken = UUID()

		DispatchQueue.main.asyncAfter(deadline: .now() + 5) {
			guard initialConnectionToken == token else { return }
			if !hasConnected {
				isInitialConnecting = false
			}
		}
	}
}
