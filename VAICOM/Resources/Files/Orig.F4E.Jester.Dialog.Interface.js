/*
 * Copyright 2023 Heatblur Simulations. All rights reserved.
 *
 */

function hb_send_proxy(action, command) {
    if (typeof window.edQuery === "function") {
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

function clickOn(element) {
    const event_data = {
        view: window,
        bubbles: true,
        cancelable: true,
        button: 0, // Left-click
        buttons: 1,
        isPrimary: true,
        pointerType: 'mouse',
        clientX: element.x,
        clientY: element.y,
    }

    const canvas = document.querySelector('canvas')

    canvas.dispatchEvent(new PointerEvent('pointerdown', event_data))
    canvas.dispatchEvent(new PointerEvent('pointerup', event_data))
}

window.pushQuestion = function(question) {
    dialog_queue.push(question)

    if (state.dialog_index === -1) {
        playDialogSound(question)
    }
}

window.clickAtMouse = function() {
    clickOn(render_objects.mouse_tracker.track_circle)
}

window.selectAction = function (action_index) {
    const action = render_objects.action_circles.circles[action_index].container
    if (action.visible) {
        clickOn(action)
    }
}

window.selectOption = function (option_index) {
    const option = render_objects.dialog_wheel.options[option_index].container
    if (render_objects.dialog_wheel.container.visible && option.visible) {
        // NOTE For some reason, normal click does not seem to reach the object
        option.onmouseenter(null, true)
    }
}

window.setMouseTracking = function(is_enabled) {
    is_tracking_mouse = is_enabled
}

function playDialogSound(dialog) {
    if (dialog.jester.length < 3) {
        return
    }

    const sound = dialog.jester[2]
    if (currently_playing_sound === sound) {
        return
    }

    stopDialogSound()

    hb_send_proxy("play_sound", sound)
    currently_playing_sound = sound
}

function stopDialogSound() {
    if (currently_playing_sound !== null) {
        hb_send_proxy("stop_sound", currently_playing_sound)
        currently_playing_sound = null
    }
}

let currently_playing_sound = null

let is_nighttime = false
let is_tracking_mouse = false

const dialog_queue = [
    {
        jester: ["Jester", "Did you take out your ejection seat pin?", "dialog/jester_ejection_pin"],
        label: "Ejection?",
        options: [
            {option: "Yes", action: "ejection_pin_yes"},
        ],
    },
    {
        jester: ["Jester", "You want a coarse align or fine?", "dialog/jester_align"],
        label: "Align",
        timing_s: {question: 6, action: 8},
        expire_action: "align_expired",
        options: [
            {option: "Let's do a coarse align", action: "align_coarse"},
            {option: "Do a Fine Align.", action: "align_fine"},
            {
                option: "How long for fine?",
                more: {
                    jester: ["Jester", "Uh, about five minutes.", "dialog/jester_align_time"],
                    options: [
                        {option: "Coarse Align", action: "align_coarse"},
                        {option: "Fine Align", action: "align_fine"},
                    ],
                }
            },
            {option: "Whatever."},
        ],
    },
    {
        jester: ["Jester", "Test", "dialog/test"],
        label: "Test",
        timing_s: {question: 2, action: 3},
        expire_action: "test_expired",
        options: [
            {option: "Ok", action: "test_1"},
            {option: "Ok", action: "test_1"},
            {option: "Ok", action: "test_1"},
        ],
    },
]

let state = {
    dialog_queue: dialog_queue,
    dialog: {},
    dialog_index: -1,
}
