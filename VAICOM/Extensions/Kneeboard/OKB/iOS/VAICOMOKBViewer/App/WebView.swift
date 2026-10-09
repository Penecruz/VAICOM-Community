import SwiftUI
import WebKit

struct WebView: UIViewRepresentable {
	let url: URL
	let reloadToken: UUID
	@Binding var isLoading: Bool
	@Binding var errorMessage: String?

	func makeCoordinator() -> Coordinator {
		Coordinator(isLoading: $isLoading, errorMessage: $errorMessage)
	}

	func makeUIView(context: Context) -> WKWebView {
		let configuration = WKWebViewConfiguration()
		configuration.defaultWebpagePreferences.allowsContentJavaScript = true

		let webView = WKWebView(frame: .zero, configuration: configuration)
		webView.navigationDelegate = context.coordinator
		webView.scrollView.bounces = false
		webView.allowsBackForwardNavigationGestures = false
		return webView
	}

	func updateUIView(_ webView: WKWebView, context: Context) {
		if context.coordinator.lastReloadToken != reloadToken || webView.url != url {
			context.coordinator.lastReloadToken = reloadToken
			let request = URLRequest(url: url, cachePolicy: .reloadIgnoringLocalCacheData, timeoutInterval: 20)
			webView.load(request)
		}
	}

	final class Coordinator: NSObject, WKNavigationDelegate {
		@Binding private var isLoading: Bool
		@Binding private var errorMessage: String?
		var lastReloadToken = UUID()

		init(isLoading: Binding<Bool>, errorMessage: Binding<String?>) {
			_isLoading = isLoading
			_errorMessage = errorMessage
		}

		func webView(_ webView: WKWebView, didStartProvisionalNavigation navigation: WKNavigation!) {
			isLoading = true
			errorMessage = nil
		}

		func webView(_ webView: WKWebView, didFinish navigation: WKNavigation!) {
			isLoading = false
		}

		func webView(_ webView: WKWebView, didFail navigation: WKNavigation!, withError error: Error) {
			isLoading = false
			errorMessage = error.localizedDescription
		}

		func webView(_ webView: WKWebView, didFailProvisionalNavigation navigation: WKNavigation!, withError error: Error) {
			isLoading = false
			errorMessage = error.localizedDescription
		}
	}
}
