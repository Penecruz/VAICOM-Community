import SwiftUI

@main
struct VAICOMOKBViewerApp: App {
	@StateObject private var settings = ConnectionSettingsStore()

	var body: some Scene {
		WindowGroup {
			NavigationStack {
				ContentView()
			}
			.environmentObject(settings)
		}
	}
}
