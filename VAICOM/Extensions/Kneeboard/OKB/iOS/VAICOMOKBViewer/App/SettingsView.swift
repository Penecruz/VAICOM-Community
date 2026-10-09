import SwiftUI

struct SettingsView: View {
	@EnvironmentObject private var settings: ConnectionSettingsStore
	@Environment(\.dismiss) private var dismiss

	var body: some View {
		NavigationStack {
			Form {
				Section("Connection") {
					TextField("Host or IP", text: $settings.host)
						.textInputAutocapitalization(.never)
						.disableAutocorrection(true)
						.keyboardType(.URL)

					TextField("Port", text: $settings.port)
						.keyboardType(.numberPad)

					LabeledContent("Preview") {
						Text(settings.dashboardURL?.absoluteString ?? "Invalid URL")
							.font(.footnote)
							.foregroundStyle(.secondary)
							.multilineTextAlignment(.trailing)
					}
				}
			}
			.navigationTitle("OKB Viewer")
			.toolbar {
				ToolbarItem(placement: .topBarTrailing) {
					Button("Done") { dismiss() }
				}
			}
		}
	}
}
