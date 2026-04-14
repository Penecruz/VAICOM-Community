/*
 * Copyright 2023 Heatblur Simulations. All rights reserved.
 *
 */

function hb_send_proxy(category, action, value = "") {
	if (value === undefined || value === null) {
		value = "";
	}

	if (typeof window.edQuery === "function") {
		window.edQuery({
			request: `${category}|${action}|${value}`,
			persistent: false,
			onSuccess: function (response) {
			},
			onFailure: function (error_code, error_message) {
			}
		});
	} else {
		console.log(category + ":" + action + ":" + value);
	}
}

function arraysEqual(a, b) {
	if (a === b) return true;
	if (a == null || b == null) return false;
	if (a.length !== b.length) return false;

	for (let i = 0; i < a.length; ++i) {
		if (a[i] !== b[i]) return false;
	}
	return true;
}

// Call this to replace existing menus with the given.
// If possible, it attempts to re-locate the current position within the new menu and retains it.
// @param menu_location - array of parent menu names indicating the location of the menu to replace, empty for main menu
window.replaceMenus = function replaceMenus(next_menu, menu_location) {
	// Locate current position by traversing back to root
	const current_pos = [current_menu.name];
	{
		let menu = current_menu;
		while (menu.parent !== undefined) {
			menu = menu.parent;
			current_pos.push(menu.name);
		}
	}

	// Swap menus
	if (menu_location.length === 0) {
		main_menu = next_menu;
	} else {
		let menu = main_menu;
		let found_pos = true;
		const remaining_menu_location = menu_location.slice();
		while (remaining_menu_location.length > 0) {
			const name = remaining_menu_location.shift();

			const sub_menu_item = menu.items.find((item) => {
				return item?.menu?.name === name;
			});

			if (sub_menu_item === undefined) {
				found_pos = false;
				break;
			}
			menu = sub_menu_item.menu;
		}

		// Swap sub-menu
		if (found_pos) {
			const parent = menu.parent;
			for (let i = 0; i < parent.items.length; i++) {
				if (parent.items[i]?.menu?.name === menu.name) {
					parent.items[i].menu = next_menu;
					break;
				}
			}
		} else {
			console.warn(
				"Unable to locate Jester Wheel menu '" +
					menu_location +
					"' while trying to replace it"
			);
			return;
		}
	}

	// Try to re-locate
	current_pos.pop(); // Ignore main menu itself, since it is the default position anyway
	{
		// Find pos within new menu structure
		let menu = main_menu;
		let found_pos = true;
		while (current_pos.length > 0) {
			const name = current_pos.pop();

			const sub_menu_item = menu.items.find((item) => {
				return item?.menu?.name === name;
			});

			if (sub_menu_item === undefined) {
				found_pos = false;
				break;
			}
			menu = sub_menu_item.menu;
		}

		// Apply position
		if (found_pos) {
			current_menu = menu;
		} else {
			current_menu = main_menu;
		}

		if (outer_menu_slot_origin !== null) {
			const next_outer_menu =
				current_menu.items[outer_menu_slot_origin]?.outer_menu;
			if (next_outer_menu) {
				outer_menu = next_outer_menu;
			} else {
				if (outer_menu.length !== 0) {
					hb_send_proxy("misc", "outer_menu_close");
				}
				outer_menu = [];
				outer_menu_slot_origin = null;
			}
		}
	}

	updateMenus();
};

// Call this to add the given item to an existing menu.
// @param menu_location - array of parent menu names indicating the location of the menu to add to, empty for main menu
window.addItemToMenu = function addItemToMenu(item, menu_location) {
	// Find menu
	let menu = main_menu;
	if (menu_location.length > 0) {
		let found_pos = true;
		const remaining_menu_location = menu_location.slice();
		while (remaining_menu_location.length > 0) {
			const name = remaining_menu_location.shift();

			const sub_menu_item = menu.items.find((item) => {
				return item?.menu?.name === name;
			});

			if (sub_menu_item === undefined) {
				found_pos = false;
				break;
			}
			menu = sub_menu_item.menu;
		}

		if (!found_pos) {
			console.warn(
				"Unable to locate Jester Wheel menu '" +
					menu_location +
					"' while trying to add item " +
					item.name +
					" to it"
			);
			return;
		}
	}

	if (menu.items.length === slot_amount) {
		console.warn(
			"Unable to add item to Jester Wheel menu '" +
				menu_location +
				"', items have reached max size already"
		);
		return;
	}

	menu.items.push(item);

	updateMenus();
};

// Call this to remove the item with given name from an existing menu.
// @param menu_location - array of parent menu names indicating the location of the menu to remove from, empty for main menu
window.removeItemFromMenu = function removeItemFromMenu(
	item_name,
	menu_location
) {
	// Locate current position by traversing back to root
	const current_pos = [current_menu.name];
	{
		let menu = current_menu;
		while (menu.parent !== undefined) {
			menu = menu.parent;
			current_pos.push(menu.name);
		}
	}
	current_pos.pop();
	current_pos.reverse();

	// Find menu
	let menu = main_menu;
	if (menu_location.length > 0) {
		let found_pos = true;
		const remaining_menu_location = menu_location.slice();
		while (remaining_menu_location.length > 0) {
			const name = remaining_menu_location.shift();

			const sub_menu_item = menu.items.find((item) => {
				return item?.menu?.name === name;
			});

			if (sub_menu_item === undefined) {
				found_pos = false;
				break;
			}
			menu = sub_menu_item.menu;
		}

		if (!found_pos) {
			console.warn(
				"Unable to locate Jester Wheel menu '" +
					menu_location +
					"' while trying to remove item '" +
					item_name +
					"' from it"
			);
			return;
		}
	}

	if (menu.items.length === 0) {
		console.warn(
			"Unable to remove item '" +
				item_name +
				"' from Jester Wheel menu '" +
				menu_location +
				"', it is the last item in the menu"
		);
		return;
	}

	const index_to_remove = menu.items.findIndex((item) => {
		return item?.name === item_name;
	});
	if (index_to_remove === -1) {
		console.warn(
			"Unable to find item '" +
				item_name +
				"' from Jester Wheel menu '" +
				menu_location +
				"' while trying to remove it"
		);
		return;
	}

	let was_current_menu_removed = false;
	if (menu.items[index_to_remove].hasOwnProperty("menu")) {
		menu_location.push(menu.items[index_to_remove].menu.name);
		const current_pos_top_hierarchy = current_pos.slice(
			0,
			menu_location.length
		);
		if (arraysEqual(current_pos_top_hierarchy, menu_location)) {
			// Removal of the item will also remove the menu hiding behind it, and we are currently inside that menu
			was_current_menu_removed = true;
		}
	}

	menu.items.splice(index_to_remove, 1);
	if (current_menu === menu) {
		if (outer_menu_slot_origin === index_to_remove) {
			if (outer_menu.length !== 0) {
				hb_send_proxy("misc", "outer_menu_close");
			}
			outer_menu = [];
			outer_menu_slot_origin = null;
		} else if (outer_menu_slot_origin > index_to_remove) {
			outer_menu_slot_origin--;
		}
	}

	if (was_current_menu_removed) {
		if (outer_menu.length !== 0) {
			hb_send_proxy("misc", "outer_menu_close");
		}
		current_menu = menu;
		outer_menu = [];
		outer_menu_slot_origin = null;
	}
};

// Call this to replace the item with given name in an existing menu.
// @param menu_location - array of parent menu names indicating the location of the menu to replace in, empty for main menu
window.replaceItemInMenu = function replaceItemInMenu(
	item,
	item_name,
	menu_location
) {
	// Find menu
	let menu = main_menu;
	if (menu_location.length > 0) {
		let found_pos = true;
		const remaining_menu_location = menu_location.slice();
		while (remaining_menu_location.length > 0) {
			const name = remaining_menu_location.shift();

			const sub_menu_item = menu.items.find((item) => {
				return item?.menu?.name === name;
			});

			if (sub_menu_item === undefined) {
				found_pos = false;
				break;
			}
			menu = sub_menu_item.menu;
		}

		if (!found_pos) {
			console.warn(
				"Unable to locate Jester Wheel menu '" +
					menu_location +
					"' while trying to replace item '" +
					item_name +
					"' in it"
			);
			return;
		}
	}

	const index_to_replace = menu.items.findIndex((item) => {
		return item?.name === item_name;
	});
	if (index_to_replace === -1) {
		console.warn(
			"Unable to find item '" +
				item_name +
				"' from Jester Wheel menu '" +
				menu_location +
				"' while trying to replace it"
		);
		return;
	}

	menu.items[index_to_replace] = item;
	if (current_menu === menu && outer_menu_slot_origin === index_to_replace) {
		// Outer menu of touched item is currently opened, update it
		if (!item.hasOwnProperty("outer_menu")) {
			if (outer_menu.length !== 0) {
				hb_send_proxy("misc", "outer_menu_close");
			}
			outer_menu = [];
			outer_menu_slot_origin = null;
		} else {
			outer_menu = item.outer_menu;
		}
	}

	updateMenus();
};

// Call this to rename the item with given name in an existing menu.
// @param menu_location - array of parent menu names indicating the location of the menu to rename in, empty for main menu
window.renameItem = function renameItem(
	new_item_name,
	current_item_name,
	menu_location
) {
	// Find menu
	let menu = main_menu;
	if (menu_location.length > 0) {
		let found_pos = true;
		const remaining_menu_location = menu_location.slice();
		while (remaining_menu_location.length > 0) {
			const name = remaining_menu_location.shift();

			const sub_menu_item = menu.items.find((item) => {
				return item?.menu?.name === name;
			});

			if (sub_menu_item === undefined) {
				found_pos = false;
				remaining_menu_location.unshift(name);
				break;
			}
			menu = sub_menu_item.menu;
		}

		if (!found_pos) {
			// Check outer menu as last effort
			const name = remaining_menu_location.shift();
			const outer_menu_item = menu.items.find((item) => {
				return item?.outer_menu?.name === name;
			});

			if (outer_menu_item !== undefined) {
				found_pos = true;
				menu = outer_menu_item.outer_menu;
			}
		}

		if (!found_pos) {
			console.warn(
				"Unable to locate Jester Wheel menu '" +
					menu_location +
					"' while trying to rename item '" +
					current_item_name +
					"' in it"
			);
			return;
		}
	}

	const index_to_rename = menu.items.findIndex((item) => {
		return item?.name === current_item_name;
	});
	if (index_to_rename === -1) {
		console.warn(
			"Unable to find item '" +
				current_item_name +
				"' from Jester Wheel menu '" +
				menu_location +
				"' while trying to replace it"
		);
		return;
	}

	menu.items[index_to_rename].name = new_item_name;
};

// Call this to set the info/description text of an existing menu. Set to an empty text to clear it.
// @param menu_location - array of parent menu names indicating the location of the menu to set in, empty for main menu
window.setMenuInfo = function setMenuInfo(info_text, menu_location) {
	// Find menu
	let menu = main_menu;
	if (menu_location.length > 0) {
		let found_pos = true;
		const remaining_menu_location = menu_location.slice();
		while (remaining_menu_location.length > 0) {
			const name = remaining_menu_location.shift();

			const sub_menu_item = menu.items.find((item) => {
				return item?.menu?.name === name;
			});

			if (sub_menu_item === undefined) {
				found_pos = false;
				remaining_menu_location.unshift(name);
				break;
			}
			menu = sub_menu_item.menu;
		}

		if (!found_pos) {
			// Check outer menu as last effort
			const name = remaining_menu_location.shift();
			const outer_menu_item = menu.items.find((item) => {
				return item?.outer_menu?.name === name;
			});

			if (outer_menu_item !== undefined) {
				found_pos = true;
				menu = outer_menu_item.outer_menu;
			}
		}

		if (!found_pos) {
			console.warn(
				"Unable to locate Jester Wheel menu '" +
					menu_location +
					"' while trying to set its info to '" +
					info_text +
					"'"
			);
			return;
		}
	}

	menu.info = info_text;
};

// Call this to navigate the menu to another location.
// @param menu_location - array of parent menu names indicating the location of the menu to navigate to, empty for main menu
window.navigateTo = function navigateTo(menu_location) {
	// Find menu
	let menu = main_menu;
	if (menu_location.length > 0) {
		let found_pos = true;
		const remaining_menu_location = menu_location.slice();
		while (remaining_menu_location.length > 0) {
			const name = remaining_menu_location.shift();

			const sub_menu_item = menu.items.find((item) => {
				return item?.menu?.name === name;
			});

			if (sub_menu_item === undefined) {
				found_pos = false;
				remaining_menu_location.unshift(name);
				break;
			}
			menu = sub_menu_item.menu;
		}

		if (!found_pos) {
			console.warn(
				"Unable to locate Jester Wheel menu '" +
					menu_location +
					"' while trying navigate to there"
			);
			return;
		}
	}

	if (outer_menu.length !== 0) {
		hb_send_proxy("misc", "outer_menu_close");
	}
	current_menu = menu;
	outer_menu = [];
	outer_menu_slot_origin = null;
};

window.updateMenus = function updateMenus() {
	// Setup parent links for back-navigation
	// Traverse menu-tree using BFS
	let unvisited = [[undefined, main_menu]];

	while (unvisited.length > 0) {
		const [parent, current] = unvisited.shift();

		current.parent = parent;

		for (let i = 0; i < current.items.length; i++) {
			const item = current.items[i];
			let sub_menu;
			if (item.hasOwnProperty("menu")) {
				sub_menu = item.menu;
			} else if (item.hasOwnProperty("outer_menu")) {
				sub_menu = item.outer_menu;
			} else {
				continue;
			}

			unvisited.push([current, sub_menu]);
		}
	}
};

window.clickAtMouse = function () {
	clickOn(mouse_tracker_track_circle);
};

function clickOn(element) {
	const event_data = {
		view: window,
		bubbles: true,
		cancelable: true,
		button: 0, // Left-click
		buttons: 1,
		isPrimary: true,
		pointerType: "mouse",
		clientX: element.x,
		clientY: element.y,
	};

	const canvas = document.querySelector("canvas");

	canvas.dispatchEvent(new PointerEvent("pointerdown", event_data));
	canvas.dispatchEvent(new PointerEvent("pointerup", event_data));
}

window.closeMenus = function () {
	is_closing = true;
};

window.openMenus = function () {
	// Changing visibility from C++ has some delay.
	// So we delay the open animation a bit, so that its fully visible.
	setTimeout(() => {
		is_closing = false;
		is_closed = false;
	}, 100);
};

window.setMouseTracking = function (is_enabled) {
	is_tracking_mouse = is_enabled;
};

window.goBack = function () {
	if (outer_menu.length === 0) {
		if (current_menu.parent === undefined) {
			window.closeMenus()
		}
		else
		{
			clickOn(inner_circle);
		}
	} else {
		hb_send_proxy("misc", "outer_menu_close");
		outer_menu = [];
		outer_menu_slot_origin = null;
	}
};

window.clickSlot = function (slot_index) {
	if (slot_index < main_menu_items_length) {
		clickOn(slots[slot_index].segment);
	}
};

window.moveSelectionCCW = function () {
	if (outer_menu.length === 0) {
		if (selected_inner_slot === null) {
			selected_inner_slot = 0;
		}
		selected_inner_slot = selected_inner_slot - 1;
		if (selected_inner_slot < 0) {
			selected_inner_slot = selected_inner_slot + main_menu_items_length;
		}
	} else {
		selected_outer_slot = selected_outer_slot - 1;
		if (selected_outer_slot < 0) {
			selected_outer_slot = selected_outer_slot + outer_menu_items_length;
		}
	}
};

window.moveSelectionCW = function () {
	if (outer_menu.length === 0) {
		if (selected_inner_slot === null) {
			selected_inner_slot = -1;
		}
		selected_inner_slot =
			(selected_inner_slot + 1) % main_menu_items_length;
	} else {
		selected_outer_slot =
			(selected_outer_slot + 1) % outer_menu_items_length;
	}
};

window.clickSelection = function () {
	let element;
	if (outer_menu.length === 0) {
		const slot_index = selected_inner_slot ?? 0;
		element = slots[slot_index].segment;
	} else {
		const slot_index =
			(selected_outer_slot + outer_start_slot) % outer_slots.length;
		element = outer_slots[slot_index].segment;
	}

	clickOn(element);
};

let is_nighttime = false;
let animation_speed_factor = 1.0;

const Reaction = {
	NOTHING: "NOTHING",
	CLOSE_REMEMBER: "CLOSE_REMEMBER",
	CLOSE_TO_MAIN_MENU: "CLOSE_TO_MAIN_MENU",
};

const icons = {
	antenna: "img/icons/antenna.svg",
	compass: "img/icons/compass.svg",
	crew_chief: "img/icons/crew_chief.png", // TODO: change to ours (current source: https://pixabay.com/vectors/screwdriver-settings-spanner-system-1294338/)
	shield: "img/icons/wrench.svg",
	skull: "img/icons/skull.svg",
	headset: "img/icons/headset.svg",
	radar: "img/icons/radar.svg",
	lock: "img/icons/lock.svg",
	focus: "img/icons/focus.svg",
	iff: "img/icons/iff.svg",
	operation: "img/icons/operation.svg",
	scanelev: "img/icons/scan-elev.svg",
	scantype: "img/icons/scan-type.svg",
	range_absolute: "img/icons/range-absolute.svg",
	jester: "img/icons/jester.svg",
	ground: "img/icons/ground.svg",
	tv: "img/icons/tv.svg",
	atc: "img/icons/atc.svg",
	tower: "img/icons/tower.svg",
	wrench: "img/icons/wrench.svg",
	freq: "img/icons/freq.svg",
	dial: "img/icons/dial.svg",
	antenna_absolute: "img/icons/range-absolute.svg",
	antenna_relative_close_low: "img/icons/range-close-low.svg",
	antenna_relative_mid_low: "img/icons/range-mid-low.svg",
	antenna_relative_far_low: "img/icons/range-far-low.svg",
	antenna_relative_mid_mid: "img/icons/range-mid-mid.svg",
	antenna_relative_close_high: "img/icons/range-close-high.svg",
	antenna_relative_mid_high: "img/icons/range-mid-high.svg",
	antenna_relative_far_high: "img/icons/range-far-high.svg",
};

const categories = {
	default: { name: "", color: { day: "#b3b3b3", night: "#898989" } },
	radio: { name: "Radio", icon: "headset" },
	navigation: { name: "Navigation", icon: "compass" },
	radar: { name: "Radar", icon: "radar" },
	weapons: { name: "Weapons", icon: "ground" },
	utility: { name: "Utility" },
	other: { name: "Other", icon: "shield" },
	crew_chief: { name: "Ground", icon: "crew_chief" },
	target_hostile: {
		name: "Bandit",
		color: { day: "#e17f7f", night: "#b06464" },
	},
	target_unknown: { name: "Bogey" },
	target_friendly: {
		name: "Friendly",
		color: { day: "#96e88b", night: "#72b06a" },
	},
	target_neutral: {
		name: "Neutral",
		color: { day: "#5ab1d0", night: "#44859b" },
	},
	bst_aspect_wide: { name: "Aspect", color: { day: "#e8d28b", night: "#b0a46a" } },
	bst_aspect_nose: { name: "Aspect", color: { day: "#e17f7f", night: "#b06464" } },
	bst_aspect_forward: { name: "Aspect", color: { day: "#b26464", night: "#804848" } },
	bst_aspect_aft: { name: "Aspect", color: { day: "#40798a", night: "#335d6b" } },
	bst_aspect_tail: { name: "Aspect", color: { day: "#67ceef", night: "#57adc9" } },
	scan_zone_absolute_low: { name: "30 NM", color: { day: "#40798a", night: "#335d6b" } },
	scan_zone_absolute_medium: { name: "30 NM", color: { day: "#51a5be", night: "#458a9f" } },
	scan_zone_absolute_high: { name: "30 NM", color: { day: "#67ceef", night: "#57adc9" } },
	scan_zone_relative_close_low: { icon: "antenna_relative_close_low", color: { day: "#e17f7f", night: "#b06464" } },
	scan_zone_relative_mid_low: { icon: "antenna_relative_mid_low", color: { day: "#e17f7f", night: "#b06464" } },
	scan_zone_relative_far_low: { icon: "antenna_relative_far_low", color: { day: "#e17f7f", night: "#b06464" } },
	scan_zone_relative_center: { icon: "antenna_relative_mid_mid", color: { day: "#e8d28b", night: "#b0a46a" } },
	scan_zone_relative_close_high: { icon: "antenna_relative_close_high", color: { day: "#96e88b", night: "#72b06a" } },
	scan_zone_relative_mid_high: { icon: "antenna_relative_mid_high", color: { day: "#96e88b", night: "#72b06a" } },
	scan_zone_relative_far_high: { icon: "antenna_relative_far_high", color: { day: "#96e88b", night: "#72b06a" } },
	lock: { icon: "lock" },
	focus: { icon: "focus" },
	iff: { icon: "iff" },
	operation: { icon: "operation" },
	boresight: { icon: "range_absolute" },
	scanelev: { icon: "scanelev" },
	scantype: { icon: "scantype" },
	jester: { icon: "jester" },
	tv: { icon: "tv" },
	atc: { icon: "atc" },
	tower: { icon: "tower" },
	wrench: { icon: "wrench" },
	freq: { icon: "freq" },
	dial: { icon: "dial" },
};

const contextual_menu = [];

const radio_menu = {
	name: "UHF Radio",
	items: [
		{
			name: "Set Manual Frequency",
			category: categories.freq,
			action: "radio_manual_freq_text",
			text_entry: { hint: "XXX XXX [MHz KHz]", max: 7, match: /[\d ]*/ },
		},
		{
			name: "Select Channel",
			category: categories.dial,
			menu: {
				name: "Select Channel",
				items: [
					{
						name: "Comm",
						outer_menu: {
							name: "Select Comm Channel",
							items: [
								{
									name: "1",
									action: "radio_comm_chan",
									action_value: "1",
								},
								{
									name: "2",
									action: "radio_comm_chan",
									action_value: "2",
								},
								{
									name: "3",
									action: "radio_comm_chan",
									action_value: "3",
								},
								{
									name: "4",
									action: "radio_comm_chan",
									action_value: "4",
								},
								{
									name: "5",
									action: "radio_comm_chan",
									action_value: "5",
								},
								{
									name: "6",
									action: "radio_comm_chan",
									action_value: "6",
								},
								{
									name: "7",
									action: "radio_comm_chan",
									action_value: "7",
								},
								{
									name: "8",
									action: "radio_comm_chan",
									action_value: "8",
								},
								{
									name: "9",
									action: "radio_comm_chan",
									action_value: "9",
								},
								{
									name: "10",
									action: "radio_comm_chan",
									action_value: "10",
								},
								{
									name: "11",
									action: "radio_comm_chan",
									action_value: "11",
								},
								{
									name: "12",
									action: "radio_comm_chan",
									action_value: "12",
								},
								{
									name: "13",
									action: "radio_comm_chan",
									action_value: "13",
								},
								{
									name: "14",
									action: "radio_comm_chan",
									action_value: "14",
								},
								{
									name: "15",
									action: "radio_comm_chan",
									action_value: "15",
								},
								{
									name: "16",
									action: "radio_comm_chan",
									action_value: "16",
								},
								{
									name: "17",
									action: "radio_comm_chan",
									action_value: "17",
								},
								{
									name: "18",
									action: "radio_comm_chan",
									action_value: "18",
								},
							],
						},
					},
					{
						name: "Aux",
						menu: {
							name: "Select Aux Channel",
							items: [
								{
									name: "1-18",
									outer_menu: {
										name: "Select Aux Channel (1-18)",
										items: [
											{
												name: "1",
												action: "radio_aux_chan",
												action_value: "1",
											},
											{
												name: "2",
												action: "radio_aux_chan",
												action_value: "2",
											},
											{
												name: "3",
												action: "radio_aux_chan",
												action_value: "3",
											},
											{
												name: "4",
												action: "radio_aux_chan",
												action_value: "4",
											},
											{
												name: "5",
												action: "radio_aux_chan",
												action_value: "5",
											},
											{
												name: "6",
												action: "radio_aux_chan",
												action_value: "6",
											},
											{
												name: "7",
												action: "radio_aux_chan",
												action_value: "7",
											},
											{
												name: "8",
												action: "radio_aux_chan",
												action_value: "8",
											},
											{
												name: "9",
												action: "radio_aux_chan",
												action_value: "9",
											},
											{
												name: "10",
												action: "radio_aux_chan",
												action_value: "10",
											},
											{
												name: "11",
												action: "radio_aux_chan",
												action_value: "11",
											},
											{
												name: "12",
												action: "radio_aux_chan",
												action_value: "12",
											},
											{
												name: "13",
												action: "radio_aux_chan",
												action_value: "13",
											},
											{
												name: "14",
												action: "radio_aux_chan",
												action_value: "14",
											},
											{
												name: "15",
												action: "radio_aux_chan",
												action_value: "15",
											},
											{
												name: "16",
												action: "radio_aux_chan",
												action_value: "16",
											},
											{
												name: "17",
												action: "radio_aux_chan",
												action_value: "17",
											},
											{
												name: "18",
												action: "radio_aux_chan",
												action_value: "18",
											},
										],
									},
								},
								{
									name: "19-20",
									outer_menu: {
										name: "Select Aux Channel (19-20)",
										items: [
											{
												name: "19",
												action: "radio_aux_chan",
												action_value: "19",
											},
											{
												name: "20",
												action: "radio_aux_chan",
												action_value: "20",
											},
										],
									},
								},
							],
						},
					},
				],
			},
		},
		{
			name: "Tune ATC",
			category: categories.tower,
			outer_menu: {
				name: "Tune ATC",
				items: [
					{ name: "Thinking...", action: "radio_tune_atc_thinking" },
				],
			},
		},
		{
			name: "Tune Assets",
			category: categories.atc,

			outer_menu: {
				name: "Tune Assets",
				items: [
					{
						name: "Thinking...",
						action: "radio_tune_asset_thinking",
					},
				],
			},
		},
		{
			name: "Select Mode",
			category: categories.wrench,
			outer_menu: {
				name: "Select Mode",
				items: [
					{ name: "OFF", action: "radio_mode", action_value: "off" },
					{
						name: "T/R, ADF",
						action: "radio_mode",
						action_value: "tr_adf",
					},
					{
						name: "T/R+G, ADF",
						action: "radio_mode",
						action_value: "trg_adf",
					},
					{
						name: "ADF+G, CMD",
						action: "radio_mode",
						action_value: "adfg_cmd",
					},
					{
						name: "ADF, G",
						action: "radio_mode",
						action_value: "adf_g",
					},
					{
						name: "G, ADF",
						action: "radio_mode",
						action_value: "g_adf",
					},
				],
			},
		},
	],
};
const radar_menu = {
	name: "Radar",
	items: [
		{
			name: "Operation",
			category: categories.operation,
			outer_menu: {
				name: "Operation",
				items: [
					{
						name: "Active",
						action: "radar_op",
						action_value: "active",
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "Standby",
						action: "radar_op",
						action_value: "standby",
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "Auto-Focus\nOn",
						action: "radar_auto_focus",
						action_value: "on",
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "Auto-Focus\nOff",
						action: "radar_auto_focus",
						action_value: "off",
						reaction: Reaction.CLOSE_REMEMBER,
					},
				],
			},
		},
		{
			name: "IFF",
			category: categories.iff,
			outer_menu: {
				name: "IFF",
				items: [
					{
						name: "Both",
						action: "radar_iff",
						action_value: "both",
					},
					{
						name: "APX-76 only",
						action: "radar_iff",
						action_value: "apx_76",
					},
				],
			},
		},
		{
			name: "Boresight",
			category: categories.boresight,
			outer_menu: {
				name: "Boresight",
				items: [
					{
						name: "Enter BST",
						action: "radar_enter_bst",
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "Wide",
						action: "radar_bst_aspect",
						action_value: "wide",
						category: categories.bst_aspect_wide,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "Nose",
						action: "radar_bst_aspect",
						action_value: "nose",
						category: categories.bst_aspect_nose,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "Forward",
						action: "radar_bst_aspect",
						action_value: "fwd",
						category: categories.bst_aspect_forward,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "Aft",
						action: "radar_bst_aspect",
						action_value: "aft",
						category: categories.bst_aspect_aft,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "Tail",
						action: "radar_bst_aspect",
						action_value: "tail",
						category: categories.bst_aspect_tail,
						reaction: Reaction.CLOSE_REMEMBER,
					},
				],
			},
		},
		{
			name: "Scan Elevation",
			category: categories.scanelev,
			outer_menu: {
				name: "Scan Elevation",
				info: "(30 NM, Altitude MSL)",
				items: [
					{
						name: "0 - 10 k ft",
						action: "radar_scan_zone",
						action_value: "30;5.250;absolute",
						category: categories.scan_zone_absolute_low,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "10 - 20 k ft",
						action: "radar_scan_zone",
						action_value: "30;14.750;absolute",
						category: categories.scan_zone_absolute_low,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "20 - 30 k ft",
						action: "radar_scan_zone",
						action_value: "30;24.750;absolute",
						category: categories.scan_zone_absolute_medium,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "30 - 40 k ft",
						action: "radar_scan_zone",
						action_value: "30;34.750;absolute",
						category: categories.scan_zone_absolute_medium,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "40 - 50 k ft",
						action: "radar_scan_zone",
						action_value: "30;44.750;absolute",
						category: categories.scan_zone_absolute_high,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "50 - 60 k ft",
						action: "radar_scan_zone",
						action_value: "30;54.750;absolute",
						category: categories.scan_zone_absolute_high,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "Center",
						action: "radar_scan_zone",
						action_value: "30;0;relative",
						category: categories.scan_zone_relative_center,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "-3,500 ft",
						action: "radar_scan_zone",
						action_value: "30;-3.5;relative",
						category: categories.scan_zone_relative_close_low,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "-5,000 ft",
						action: "radar_scan_zone",
						action_value: "30;-5;relative",
						category: categories.scan_zone_relative_mid_low,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "-7,500 ft",
						action: "radar_scan_zone",
						action_value: "30;-7.5;relative",
						category: categories.scan_zone_relative_far_low,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "+3,500 ft",
						action: "radar_scan_zone",
						action_value: "30;3.5;relative",
						category: categories.scan_zone_relative_close_high,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "+5,000 ft",
						action: "radar_scan_zone",
						action_value: "30;5;relative",
						category: categories.scan_zone_relative_mid_high,
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "+7,500 ft",
						action: "radar_scan_zone",
						action_value: "30;7.5;relative",
						category: categories.scan_zone_relative_far_high,
						reaction: Reaction.CLOSE_REMEMBER,
					},
				],
			},
		},
		{
			name: "Scan Type",
			category: categories.scantype,

			outer_menu: {
				name: "Scan Type",

				items: [
					// TODO Implement LEFT/RIGHT/CENTER
					{
						name: "25 nm\nWide",
						action: "radar_display_range",
						action_value: "nm_25;wide",
						reaction: Reaction.CLOSE_REMEMBER,
					},
					//{name: "25 nm\nNarrow Right", action: "radar_display_range", action_value: "nm_25;narrow_right", reaction: Reaction.CLOSE_REMEMBER},
					{
						name: "25 nm\nNarrow",
						action: "radar_display_range",
						action_value: "nm_25;narrow",
						reaction: Reaction.CLOSE_REMEMBER,
					},
					//{name: "25 nm\nNarrow Left", action: "radar_display_range", action_value: "nm_25;narrow_left", reaction: Reaction.CLOSE_REMEMBER},
					{
						name: "50 nm\nWide",
						action: "radar_display_range",
						action_value: "nm_50;wide",
						reaction: Reaction.CLOSE_REMEMBER,
					},
					//{name: "50 nm\nNarrow Right", action: "radar_display_range", action_value: "nm_50;narrow_right", reaction: Reaction.CLOSE_REMEMBER},
					{
						name: "50 nm\nNarrow",
						action: "radar_display_range",
						action_value: "nm_50;narrow",
						reaction: Reaction.CLOSE_REMEMBER,
					},
					//{name: "50 nm\nNarrow Left", action: "radar_display_range", action_value: "nm_50;narrow_left", reaction: Reaction.CLOSE_REMEMBER},
				],
			},
		},
		{
			name: "Focus Target",
			category: categories.focus,

			outer_menu: {
				name: "Focus Target",
				items: [
					{
						name: "Thinking...",
						action: "radar_focus_targets_thinking",
					},
				],
			},
		},
		{
			name: "Lock Target",
			category: categories.lock,

			outer_menu: {
				name: "Lock Target",
				items: [
					{
						name: "Thinking...",
						action: "radar_lock_targets_thinking",
					},
				],
			},
		},
	],
};
const a2g_menu = {
	name: "Air To Ground",
	items: [
		{
			name: "TV Video",
			category: categories.tv,
			outer_menu: {
				name: "TV Video",
				items: [
					{
						name: "Weapons",
						action: "a2g_tv_feed",
						action_value: "weapons",
					},
					{
						name: "Pave Spike",
						action: "a2g_tv_feed",
						action_value: "pave_spike",
					},
				],
			},
		},
	],
};

const nav_menu = {
	name: "Navigation",
	items: [
		{
			name: "Go To / Resume",
			menu: {
				name: "Go To / Resume",
				items: [
					{ name: "Thinking...", action: "goto_resume_thinking" },
				],
			},
		},
		{
			name: "Holding",
			menu: {
				name: "Holding",
				items: [
					{
						name: "Current Turn Point Activate",
						action: "hold_curr_wpt",
					},
					{
						name: "Set For Primary Flight Plan",
						outer_menu: {
							name: "Set For Primary Flight Plan",
							items: [
								{
									name: "Thinking...",
									action: "flightplan1_thinking",
								},
							],
						},
					},
					{
						name: "Set For Secondary Flight Plan",
						outer_menu: {
							name: "Set For Secondary Flight Plan",
							items: [
								{
									name: "Thinking...",
									action: "flightplan2_thinking",
								},
							],
						},
					},
					{
						name: "Deactivate Planned Hold",
						outer_menu: {
							name: "Deactivate Planned Hold",
							items: [
								{
									name: "Thinking...",
									action: "flightplan2_thinking",
								},
							],
						},
					},
				],
			},
		},
		{
			name: "Divert To",
			menu: {
				name: "Divert To",
				items: [
					{
						name: "Lat/Long",
						action: "nav_enter_tgt_1_lat_long_text",
						text_entry: {
							hint: "H DD MM H DDD MM",
							max: 16,
							match: /[\d NSEWnsew]*/,
						},
					},
					{
						name: "Flight Plan",
						menu: {
							name: "Flight Plan",
							items: [
								{
									name: "Primary Flight Plan",
									outer_menu: {
										name: "Primary Flight Plan",
										items: [
											{
												name: "Thinking...",
												action: "flightplan1_thinking",
											},
										],
									},
								},
								{
									name: "Secondary Flight Plan",
									outer_menu: {
										name: "Secondary Flight Plan",
										items: [
											{
												name: "Thinking...",
												action: "flightplan2_thinking",
											},
										],
									},
								},
							],
						},
					},
					{
						name: "Map Markers",
						outer_menu: {
							name: "Map Markers",
							items: [
								{
									name: "Thinking...",
									action: "map_marker_thinking",
								},
							],
						},
					},
					{
						name: "Airfields",
						outer_menu: {
							name: "Airfields",
							items: [
								{
									name: "Thinking...",
									action: "airfields_thinking",
								},
							],
						},
					},
					{
						name: "Assets",
						outer_menu: {
							name: "Assets",
							items: [
								{
									name: "Thinking...",
									action: "assets_thinking",
								},
							],
						},
					},
				],
			},
		},
		{
			name: "Edit Flight Plan",
			menu: {
				name: "Edit Flight Plan",
				items: [
					{
						name: "Primary Flight Plan",
						menu: {
							name: "Primary Flight Plan",
							items: [
								{
									name: "Thinking...",
									action: "flightplan1_edit_thinking",
								},
							],
						},
					},
					{
						name: "Secondary Flight Plan",
						menu: {
							name: "Secondary Flight Plan",
							items: [
								{
									name: "Thinking...",
									action: "flightplan1_edit_thinking",
								},
							],
						},
					},
				],
			},
		},
		{
			name: "TACAN",
			menu: {
				name: "TACAN",
				items: [
					{
						name: "Select Mode",
						outer_menu: {
							name: "Mode",
							items: [
								{
									name: "Off",
									action: "nav_tacan_mode",
									action_value: "off",
								},
								{
									name: "R",
									action: "nav_tacan_mode",
									action_value: "r",
								},
								{
									name: "T/R",
									action: "nav_tacan_mode",
									action_value: "tr",
								},
								{
									name: "A/A R",
									action: "nav_tacan_mode",
									action_value: "aar",
								},
								{
									name: "A/A T/R",
									action: "nav_tacan_mode",
									action_value: "aatr",
								},
							],
						},
					},
					{
						name: "Select Channel",
						outer_menu: {
							name: "Select Channel",
							items: [
								{
									name: "Thinking...",
									action: "nav_tacan_chan_tens_thinking",
								},
							],
						},
					},
					{
						name: "Tune Ground Station",
						outer_menu: {
							name: "Tune Ground Station",
							items: [
								{
									name: "Thinking...",
									action: "nav_tacan_ground_thinking",
								},
							],
						},
					},
					{
						name: "Tune Assets",
						outer_menu: {
							name: "Tune Assets",
							items: [
								{
									name: "Thinking...",
									action: "nav_tacan_tac_thinking",
								},
							],
						},
					},
				],
			},
		},
	],
};

const systems_menu = {
	name: "Systems",
	items: [
		{
			name: "Chaff Mode",
			outer_menu: {
				name: "Chaff Mode",
				items: [
					{
						name: "Off",
						action: "systems_chaff",
						action_value: "off",
					},
					{
						name: "Single",
						action: "systems_chaff",
						action_value: "single",
					},
					{
						name: "Multiple",
						action: "systems_chaff",
						action_value: "multiple",
					},
					{
						name: "Program",
						action: "systems_chaff",
						action_value: "program",
					},
				],
			},
		},
		{
			name: "Flare Mode",
			outer_menu: {
				name: "Flare Mode",
				items: [
					{
						name: "Off",
						action: "systems_flare",
						action_value: "off",
					},
					{
						name: "Single",
						action: "systems_flare",
						action_value: "single",
					},
					{
						name: "Program",
						action: "systems_flare",
						action_value: "program",
					},
				],
			},
		},
		{
			name: "Chaff / Flare\nQuantity",
			action: "systems_countermeasures_quantity",
		},
		{
			name: "Flares Jettison",
			outer_menu: {
				name: "Flares Jettison",
				items: [
					{
						name: "Jettison Flares",
						action: "systems_flares_jettison",
					},
				],
			},
		},
		{
			name: "Jammer",
			outer_menu: {
				name: "Jammer",
				items: [
					{
						name: "Standby",
						action: "systems_jammer",
						action_value: "standby",
					},
					{
						name: "Transmit",
						action: "systems_jammer",
						action_value: "xmit",
					},
				],
			},
		},
		{
			name: "AVTR Recorder",
			outer_menu: {
				name: "AVTR Recorder",
				items: [
					{
						name: "Record",
						action: "systems_avtr_recorder",
						action_value: "record",
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "Standby",
						action: "systems_avtr_recorder",
						action_value: "standby",
						reaction: Reaction.CLOSE_REMEMBER,
					},
					{
						name: "Off",
						action: "systems_avtr_recorder",
						action_value: "off",
						reaction: Reaction.CLOSE_REMEMBER,
					},
				],
			},
		},
	],
};

const crew_menu = {
	name: "Crew Contract",
	category: categories.jester,
	items: [
		{
			name: "Jester Presence",
			outer_menu: {
				name: "Jester Presence",
				items: [
					{
						name: "Auto",
						action: "crew_presence",
						action_value: "auto",
					},
					{
						name: "Force",
						action: "crew_presence",
						action_value: "enabled",
					},
					{
						name: "Disable",
						action: "crew_presence",
						action_value: "disabled",
					},
				],
			},
		},
		{
			name: "Jester Talking",
			outer_menu: {
				name: "Jester Talking",
				items: [
					{
						name: "Talk To Me",
						action: "crew_talking",
						action_value: "talk",
					},
					{
						name: "Silence Please",
						action: "crew_talking",
						action_value: "silence",
					},
				],
			},
		},
		{
			name: "Ejection Selector",
			outer_menu: {
				name: "Ejection Selector",
				items: [
					{
						name: "WSO",
						action: "crew_ejection",
						action_value: "wso",
					},
					{
						name: "Both",
						action: "crew_ejection",
						action_value: "both",
					},
				],
			},
		},
		{
			name: "Countermeasures Dispensing",
			outer_menu: {
				name: "Countermeasures Dispensing",
				items: [
					{
						name: "Manual",
						action: "crew_countermeasures",
						action_value: "manual",
					},
					{
						name: "Jester",
						action: "crew_countermeasures",
						action_value: "jester",
					},
				],
			},
		},
	],
};

let main_menu = {
	name: "Main Menu",
	items: [
		{ name: "UHF Radio", category: categories.radio, menu: radio_menu },
		{ name: "Radar", category: categories.radar, menu: radar_menu },
		{ name: "Air To Ground", category: categories.weapons, menu: a2g_menu },
		{ name: "Navigation", category: categories.navigation, menu: nav_menu },
		{ name: "Systems", category: categories.other, menu: systems_menu },
		{
			name: "Crew Contract",
			category: categories.jester,
			menu: crew_menu,
		},
	],
};

let outer_menu = [];
let outer_menu_slot_origin = null;
let is_closing = false;
let is_closed = true;
let is_tracking_mouse = false;
let text_entry_slot_origin = null;

const empty_menu = {
	name: "Empty Menu",
	items: [],
};

const menus = {
	contextual_menu: contextual_menu,
	main_menu: main_menu,
	outer_menu: outer_menu,
	empty_menu: empty_menu,
};

let current_menu = menus.main_menu;
