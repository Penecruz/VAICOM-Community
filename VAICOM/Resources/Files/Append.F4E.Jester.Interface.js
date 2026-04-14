// VAICOM PRO server-side script
// JesterAI_Page.lua
// www.vaicompro.com

// Create WebSocket connection.
const socket = new WebSocket("ws://localhost:5000/vaicom/");

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
