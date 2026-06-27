// VAICOM server-side script
// Jester 2.0 Dialog interface.js

// WebSocket connection to VAICOM
let socket;

// For wrapping the HB state object for dialogs.
let proxiedState = {
  dialog_queue: state.dialog_queue,
  dialog: state.dialog,
  dialog_index: state.dialog_index,
};

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
      const { action, command, value } = JSON.parse(event.data);
      if (action !== null && command !== null) {
        if (action === "action") {
          doDialogAction(action, command);
        } else if (action === "more" && state.dialog_queue.length > 0) {
          // This is for the case where we are wanting to select options
          // nested further down in the dialog tree, e.g. for displaying to
          // the user before performing an actual action.
          const dialog = getDialog(state.dialog_queue[0], value);
          if (dialog !== undefined) {
            state.dialog = dialog;
            state.dialog_index = 0;
            playDialogSound(dialog);

            // If we have requested the dialog to be shown so users can see
            // the available options then display this.
            if (command === "show_dialog_options" && typeof window.showJesterDialog === "function") {
              window.showJesterDialog();
            }
          }
        }
      }
    } catch (e) {
      sendSocketMessage(`WSO Jester 2.0 Dialog: Error ${e}`);
    }
  });
}

// Recursively look through all dialogs in the current dialog queue
// to locate the one which matches the value we are looking for.
function getDialog(dialog, value) {
  const { options } = dialog;

  for (const option of options) {
    // The value matches an option at this level so return the nested dialog it
    // opens so it can be set as the current dialog.
    if (option.option === value) {
      return option.more;
    }

    // No match here, but this option opens a nested dialog so search down into
    // it and return whichever nested dialog contains the match.
    if (option.more !== undefined) {
      const nested = getDialog(option.more, value);
      if (nested !== undefined) {
        return nested;
      }
    }
  }

  return undefined;
}

function doDialogAction(action, command) {
  // The dialog sends a number of actions through the hb_proxy
  // along with the actual action selected so we mimic those.
  hb_send_proxy("misc", "selected_dialog");
  hb_send_proxy(action, command);

  closeDialog();
}

function closeDialog() {
  hb_send_proxy("misc", "out_of_dialog");
  stopDialogSound();

  // Cleanup the dialog state
  state.dialog = {};
  state.dialog_index = -1;
  // Keep the original proxied array but just delete all entries from it.
  // The hb code also uses a splice on the array so aligns with this.
  if (state.dialog_queue.length > 0) {
    state.dialog_queue.splice(0);
  }

  // Close the dialog with a timeout
  setTimeout(() => hb_send_proxy("misc", "close"), 200);

  if (typeof window.hideJesterDialog === "function") {
    window.hideJesterDialog();
  }
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
  try {
    if (isSocketOpen()) {
      socket.send(data);
    } else {
      checkSocketStatus();
    }
  } catch (e) {
    console.log("Error sending websocket message", e);
  }
}

// Handler for when properties are set or retrieved from the HB state object.
// The "get" handler is only needed so that we can add a proxy to the nested properties.
// The "set" handler is used for for sending the dialog options to VAICOM.
const stateProxyHandler = {
  get(target, key, receiver) {
    // Check if this a proxy object so we aren't wrapping multiple times
    if (key == "isProxy") return true;

    const prop = target[key];
    if (typeof prop == "undefined") return;

    // If this is an object then wrap it with a proxy as well
    if (!prop.isProxy && typeof prop === "object")
      target[key] = new Proxy(prop, stateProxyHandler);

    return Reflect.get(target, key, receiver);
  },
  set(target, key, value, receiver) {
    const success = Reflect.set(target, key, value, receiver);

    // Send the current/updated state of the dialog and options to VAICOM
    // Ignore the timers used for expiry as these updates thousands of times
    if (key !== "timer" && key !== "timing_s") {
      sendSocketMessage(`${JSON.stringify(state)}`);
    }

    return success;
  },
};

// Wrap the HB state object with a proxy object. This is sent to the server
// to maintain state of what is in the dialogs to be cached.
state = new Proxy(proxiedState, stateProxyHandler);

function hb_send_proxy(action, command = "") {
  try {
    if (typeof window.edQuery === "function") {
      sendSocketMessage(`Jester 2.0 Dialog: ${action}|${command}`);
  
      window.edQuery({
        request: `${action}|${command}`,
        persistent: false,
        onSuccess: function (response) {},
        onFailure: function (error_code, error_message) {
            console.log(`error_code: ${error_code}, error_message: ${error_message}`);
        },
      });
    } else {
      console.log(action + ": " + command);
    }
  } catch (e) {
    console.log(`error in send proxy: ${e}`, e);
  }
}

// Connect to VAICOM during initialisation
openSocketConnection();

// Check every 3 seconds if the socket is open
setInterval(checkSocketStatus, 3000);