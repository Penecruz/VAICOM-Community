// VAICOM PRO server-side script
// Jester 2.0 interface.js
// www.vaicompro.com

function isSocketOpen() {
	return socket && socket.readyState === WebSocket.OPEN;
}

function collectNavCacheEntries(menu, path, entries) {
	if (!menu || !menu.items || !entries) {
		return;
	}

	for (let i = 0; i < menu.items.length; i++) {
		const item = menu.items[i];
		if (!item) {
			continue;
		}

		const itemName = item.name || "";
		const itemPath = path.concat([itemName]);

		if (item.action && typeof item.action_value === "string" && item.action_value.indexOf(";") >= 0) {
			entries.push({
				action: item.action,
				name: itemName,
				value: item.action_value,
				path: itemPath.join(" > ")
			});
		}

		if (item.menu) {
			collectNavCacheEntries(item.menu, itemPath, entries);
		}

		if (item.outer_menu) {
			collectNavCacheEntries(item.outer_menu, itemPath, entries);
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

		socket.send(JSON.stringify({
			type: "nav_cache_bulk",
			reason: reason || "menu_update",
			items: entries
		}));
	} catch (e) {
		if (isSocketOpen()) {
			socket.send(`F4E nav cache error: ${e}`);
		}
	}
}

function hb_send_proxy(category, action, value = "") {
	if (value === undefined || value === null) {
		value = "";
	}

	if (typeof window.edQuery === "function") {
      if (isSocketOpen()) {
			socket.send(`Jester Menu: ${category}|${action}|${value}`);
		}
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

// Create WebSocket connection.
const socket = new WebSocket("ws://127.0.0.1:33495/vaicom/wso/");

// Connection opened
socket.addEventListener("open", (event) => {
	socket.send("F4-E: connected");
 setTimeout(function () {
		sendNavCacheSnapshot("socket_open");
	}, 200);
});

// Listen for messages
socket.addEventListener("message", (event) => {
  if (isSocketOpen()) {
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

const vaicomOriginalUpdateMenus = window.updateMenus;
window.updateMenus = function updateMenusWithNavCache() {
	if (typeof vaicomOriginalUpdateMenus === "function") {
		vaicomOriginalUpdateMenus();
	}
	sendNavCacheSnapshot("updateMenus");
};
