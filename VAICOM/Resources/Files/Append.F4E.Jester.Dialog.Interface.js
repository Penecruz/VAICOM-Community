// VAICOM server-side script
// Jester 2.0 Dialog interface.js

// WebSocket connection to VAICOM
let socket;

// Create WebSocket connection and event listeners.
function openSocketConnection() {
	// Don't reconnect if we already have an open connection, or are
	// already attempting to open a new one.
	if (isSocketOpen() || isSocketConnecting()) {
		return;
	}

	socket = new WebSocket("ws://127.0.0.1:33496/vaicom/wso/dialog");

	// Connection opened
	socket.addEventListener("open", (event) => {
		socket.send("WSO Jester 2.0 Dialog: Connected");
	});

	// Listen for messages
	socket.addEventListener("message", (event) => {
		try {
			const { action, command } = JSON.parse(event.data);
			if (command !== undefined) {
                // The dialog sends a number of actions through the hb_proxy
                // along with the actual action selected so we mimic those.
				hb_send_proxy("misc", "selected_dialog");
                hb_send_proxy(action, command);
                hb_send_proxy("misc", "out_of_dialog");

				// Cleanup the dialog state
                state.dialog = {}
                state.dialog_index = -1

                // Close the dialog with a timeout
                setTimeout(() => hb_send_proxy("misc", "close"), 200);
			}
		} catch (e) {
			sendSocketMessage(`WSO Jester 2.0 Dialog: Error ${e}`);
		}
	});
}

function isSocketConnecting() {
	return socket && socket.readyState === WebSocket.CONNECTING;
}

function isSocketOpen() {
	return socket && socket.readyState === WebSocket.OPEN;
}

function checkSocketStatus() {
	if (!isSocketOpen()) {
		// If the socket wasn't open then open a new connection.This handles the
		// case when the server-side connection is closed, e.g. if VAICOM is restarted
		// after this interface page is loaded.
		openSocketConnection();
	}
}

function sendSocketMessage(data) {
	if (isSocketOpen()) {
		socket.send(data);
	} else {
		checkSocketStatus();
	}
}

function hb_send_proxy(action, command = "") {
	if (command === undefined || command === null) {
		command = "";
	}

	if (typeof window.edQuery === "function") {
		sendSocketMessage(`Jester 2.0 Dialog: ${action}|${command}`);

		window.edQuery({
			request: `${action}|${command}`,
			persistent: false,
			onSuccess: function (response) {
			},
			onFailure: function (error_code, error_message) {
			}
		});
	} else {
		console.log(action + ": " + command)
	}
}

// Connect to VAICOM during initialisation
openSocketConnection();

// Check every 3 seconds if the socket is open
setInterval(checkSocketStatus, 3000);