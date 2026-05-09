// VAICOM server-side script
// Jester 2.0 interface.js

// WebSocket connection to VAICOM
let socket;

// Create WebSocket connection and event listeners.
function openSocketConnection() {
	// Don't reconnect if we already have an open connection, or are
	// already attempting to open a new one.
	if (isSocketOpen() || isSocketConnecting()) {
		return;
	}

	socket = new WebSocket("ws://127.0.0.1:33495/vaicom/wso/");

	// Connection opened
	socket.addEventListener("open", (event) => {
		socket.send("WSO Jester 2.0: Connected");
		
		setTimeout(function () {
			sendNavCacheSnapshot("socket_open");
		}, 200);
	});

	// Listen for messages
	socket.addEventListener("message", (event) => {
		try {
			const { category, action, value } = JSON.parse(event.data);
			if (category !== undefined && action !== undefined) {
				hb_send_proxy(category, action, value);
			}
		} catch (e) {
			sendSocketMessage(`WSO Jester 2.0: Error ${e}`);
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

function collectNavCacheEntries(menu, path, entries, all = false) {
	if (!menu || !menu.items || !entries) {
		return;
	}

	// Allowable list of actions to be included within the sent cache data
	const allowedActions = [
		"divert_tgt1_lat_lon", "hold_flightplan_1", "hold_flightplan_2",
		"resume_flightplan_1", "resume_flightplan_2", "radio_tune_atc",
		"nav_tacan_tr", "designate_wpt"
	];

	for (let i = 0; i < menu.items.length; i++) {
		const item = menu.items[i];
		if (!item) {
			continue;
		}

		const { name, action, action_value } = item;
		const itemName = name || "";
		const itemPath = path.concat([itemName]);

		// Only add cache entries for specific actions
		if (all || allowedActions.includes(action)) {
			entries.push({
				action,
				name: itemName,
				value: action_value,
				path: itemPath.join(" > ")
			});
		}

		if (item.menu) {
			collectNavCacheEntries(item.menu, itemPath, entries, all);
		}

		if (item.outer_menu) {
			collectNavCacheEntries(item.outer_menu, itemPath, entries, all);
		}
	}
}

function sendNavCacheSnapshot(reason) {
	try {
		if (!isSocketOpen()) {
			return;
		}

		if (typeof main_menu === "undefined" || !main_menu) {
			return;
		}

		const entries = [];
		collectNavCacheEntries(main_menu, ["Main Menu"], entries);

		sendSocketMessage(JSON.stringify({
			type: "nav_cache_bulk",
			reason: reason || "menu_update",
			items: entries
		}));
	} catch (e) {
		sendSocketMessage(`WSO Jester 2.0: Cache Error ${e}`);
	}
}

function hb_send_proxy(category, action, value = "") {
	if (value === undefined || value === null) {
		value = "";
	}

	if (typeof window.edQuery === "function") {
		sendSocketMessage(`Jester 2.0 Menu: ${category}|${action}|${value}`);

		window.edQuery({
			request: `${category}|${action}|${value}`,
			persistent: false,
			onSuccess: function (response) {
			},
			onFailure: function (error_code, error_message) {
			}
		});
        

		setTimeout(function () {
			sendNavCacheSnapshot("command");
		}, 50);
	} else {
		console.log(category + ":" + action + ":" + value);
	}
}

const vaicomOriginalUpdateMenus = window.updateMenus;
window.updateMenus = function updateMenusWithNavCache() {
	if (typeof vaicomOriginalUpdateMenus === "function") {
		vaicomOriginalUpdateMenus();
	}
	sendNavCacheSnapshot("updateMenus");
};

// Connect to VAICOM during initialisation
openSocketConnection();

// Check every 3 seconds if the socket is open
setInterval(checkSocketStatus, 3000);