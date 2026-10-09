import Foundation
import Combine

final class ConnectionSettingsStore: ObservableObject {
	@Published var host: String {
		didSet { save() }
	}

	@Published var port: String {
		didSet { save() }
	}

	private let hostKey = "okb_host"
	private let portKey = "okb_port"

	init() {
		let defaults = UserDefaults.standard
		self.host = defaults.string(forKey: hostKey) ?? "192.168.1.100"
		self.port = defaults.string(forKey: portKey) ?? "7779"
	}

	var normalizedPort: Int {
		guard let parsed = Int(port), parsed > 0, parsed <= 65535 else {
			return 7779
		}
		return parsed
	}

	var dashboardURL: URL? {
		let trimmedHost = host.trimmingCharacters(in: .whitespacesAndNewlines)
		guard !trimmedHost.isEmpty else {
			return nil
		}

		let encodedHost = trimmedHost.addingPercentEncoding(withAllowedCharacters: .urlHostAllowed) ?? trimmedHost
		return URL(string: "http://\(encodedHost):\(normalizedPort)/okb/")
	}

	private func save() {
		let defaults = UserDefaults.standard
		defaults.set(host, forKey: hostKey)
		defaults.set(port, forKey: portKey)
	}
}
