// VAICOM PRO server-side script
// Jester 2.0 interface.js
// www.vaicompro.com

// Create WebSocket connection.
const socket = new WebSocket("ws://127.0.0.1:33495/vaicom/wso/");

// Connection opened
socket.addEventListener("open", (event) => {
	socket.send("F4-E: connected");
});

// Listen for messages
socket.addEventListener("message", (event) => {
	if (socket.OPEN) {
		try {
			const { category, action, value } = JSON.parse(event.data);
			if (category !== undefined && action !== undefined) {
				hb_send_proxy(category, action, value);
			}
		} catch (e) {
			socket.send(`F4E error: ${e}`);
		}
	}
});
