/*
 * Copyright 2023 Heatblur Simulations. All rights reserved.
 *
 */

const font = new FontFaceObserver("Coda", {})
font.load().then(function () {
    document.documentElement.className += " fonts-loaded"

    const circles = render_objects.action_circles.circles
    for (const i in circles) {
        circles[i].symbol.updateText()
    }
})

const action_circle_ring_border = 4
const action_circle_ring_radius = 32
const dialog_wheel_ring_border = 2
const dialog_wheel_ring_radius = 61
const dialog_wheel_option_radius = {active: 8, inactive: 3}
const dialog_wheel_option_hitbox_radius = 30
const selected_option_speed_deg_per_s = 2 * Math.PI / 0.5
const click_blink_speed_ms = 90
const mouse_tracker_center_circle_radius = 3
const mouse_tracker_track_circle_radius = 10
const mouse_tracker_line_width = 3

const text_font_size = 18
const label_font_size = 14
const press_text_font_size = 13

const dialog_slot_colors = [
    {day: '#04d75e', night: '#009004'},
    {day: '#F97806', night: '#d71441'},
    {day: '#4285D2', night: '#9312da'},
]
const action_circle_ring_color = {day: '#2B2B2B', night: '#301D20'}
const text_color = {day: '#ececec', night: '#bababa'}
const shadow_color = {day: '#000000', night: '#AAAAAA'}
const tracker_color = {day: '#FFFFFF', night: '#b3b3b3'}

const render_objects = {
    question: {},
    action_circles: {circles: []},
    dialog_wheel: {options: []},
    mouse_tracker: {},
}

function setupRenderer(app, onActionCircleClick, onDialogWheelOptionClick) {
    createQuestion(app)
    createActionCircles(app, onActionCircleClick)
    createDialogWheel(app, onDialogWheelOptionClick)
    createMouseTracker(app)
}

function createQuestion(app) {
    const question = {}
    question.container = new PIXI.Container()
    app.stage.addChild(question.container)

    question.jester_name = createText(text_font_size, question.container)
    question.jester_text = createText(text_font_size, question.container)
    question.jester_name.x = 135

    render_objects.question = question
}

function createActionCircles(app, onActionCircleClick) {
    const action_circles = {circles: []}
    action_circles.container = new PIXI.Container()
    app.stage.addChild(action_circles.container)

    const symbols = ["Q", "W", "E"]
    for (let i = 0; i < symbols.length; i++) {
        const circle = {}
        circle.container = new PIXI.Container()
        circle.container.sortableChildren = true
        action_circles.container.addChild(circle.container)

        circle.symbol = createText(text_font_size, circle.container)
        circle.symbol.text = symbols[i]
        circle.symbol.anchor.set(0.5, 0.5)

        createActionCircleRing(circle)

        circle.label = createText(label_font_size, circle.container)
        circle.label.anchor.set(0.5, 0)

        circle.container.y = 86
        if (i === 0) {
            circle.container.x = 208
        } else {
            const left_circle = action_circles.circles[i - 1].container
            const bounds = left_circle.getBounds()
            circle.container.x = bounds.x + bounds.width + action_circle_ring_radius + 25
        }

        circle.label.y = circle.container.getLocalBounds().height - 22

        circle.container.eventMode = 'static'
        circle.container.cursor = 'pointer'

        const blink_color = {}
        circle.blink_tween_1 = PIXI.tweenManager.createTween(blink_color)
        circle.blink_tween_1.time = click_blink_speed_ms

        circle.blink_tween_2 = PIXI.tweenManager.createTween(circle.container)
        circle.blink_tween_2.time = click_blink_speed_ms
        circle.blink_tween_2.from({alpha: 1})
            .to({alpha: 0})
            .on('end', () => {
                onActionCircleClick(i)
                circle.container.alpha = 1
            })
        circle.container.onpointerdown = function () {
            const start_color = hexToHsl(getDayNightColor(dialog_slot_colors[i]))
            const end_color = structuredClone(start_color)
            if (is_nighttime) {
                end_color.s = 0 // Blink to grey
            } else {
                end_color.l = 100 // Blink to white
            }

            circle.blink_tween_1.from(start_color)
                .to(end_color)
                .on('start', () => {
                    circle.container.alpha = 1
                })
                .on('update', () => {
                    circle.ring.foreground_obj.tint = `hsl(${blink_color.h}, ${blink_color.s}%, ${blink_color.l}%)`
                }).start()
                .chain(circle.blink_tween_2)
        }

        action_circles.circles[i] = circle
    }

    render_objects.action_circles = action_circles
}

function createActionCircleRing(circle) {
    const ring_container = new PIXI.Container()
    ring_container.sortableChildren = true

    circle.ring = {}

    const width = action_circle_ring_radius - (action_circle_ring_border / 2)
    {
        // Background circle (invisible but clickable)
        const graphics = new PIXI.Graphics()
            .beginFill('white', 0.0001)
            .drawCircle(0, 0, width)
            .endFill()
        graphics.zIndex = 0
        ring_container.addChild(graphics)
    }
    {
        // Background ring
        const graphics = new PIXI.Graphics()
            .lineStyle(action_circle_ring_border, 'white', 0.4)
            .drawCircle(0, 0, width)
            .endFill()
        graphics.zIndex = 1
        ring_container.addChild(graphics)
        circle.ring.background_obj = graphics
    }
    {
        // Foreground ring
        const foreground = new PIXI.Container()

        const graphics = new PIXI.Graphics()
            .lineStyle(action_circle_ring_border, 'white', 0.85)
            .drawCircle(0, 0, width)
            .endFill()
        foreground.mask = new PIXI.Graphics()

        foreground.addChild(graphics)
        foreground.zIndex = 2

        ring_container.addChild(foreground)
        circle.ring.foreground = foreground
        circle.ring.foreground_obj = graphics
    }

    circle.container.addChild(ring_container)
}

function createDialogWheel(app, onDialogWheelOptionClick) {
    const dialog_wheel = {options: []}
    dialog_wheel.container = new PIXI.Container()
    app.stage.addChild(dialog_wheel.container)
    dialog_wheel.container.x = 0
    dialog_wheel.container.y = 50

    dialog_wheel.selected_option = -1

    {
        // Background Ring
        const width = dialog_wheel_ring_radius - (dialog_wheel_ring_border / 2)
        const graphics = new PIXI.Graphics()
            .lineStyle(dialog_wheel_ring_border, 'white', 0.85)
            .drawCircle(300, 75, width)
            .endFill()
        dialog_wheel.container.addChild(graphics)
        dialog_wheel.ring = graphics
    }

    // Options
    for (let i = 0; i < 4; i++) {
        const option = {}
        option.container = new PIXI.Container()
        dialog_wheel.container.addChild(option.container)

        option.circle = new PIXI.Graphics()
            .beginFill('white')
            .drawCircle(0, 0, dialog_wheel_option_radius.inactive)
            .endFill()
        option.container.addChild(option.circle)

        option.hitbox_circle = new PIXI.Graphics()
            .beginFill('white', 0.0001)
            .drawCircle(0, 0, dialog_wheel_option_hitbox_radius)
            .endFill()
        option.container.addChild(option.hitbox_circle)

        option.label = createText(text_font_size, option.container)

        option.press_text = createText(press_text_font_size, option.container)
        option.press_text.text = _((i + 1).toString())

        option.container.eventMode = 'static'
        option.container.cursor = 'pointer'

        const blink_color = {}
        option.blink_tween_1 = PIXI.tweenManager.createTween(blink_color)
        option.blink_tween_1.time = click_blink_speed_ms

        option.blink_tween_2 = PIXI.tweenManager.createTween(option.container)
        option.blink_tween_2.time = click_blink_speed_ms
        option.blink_tween_2.from({alpha: 1})
            .to({alpha: 0})
            .on('end', () => {
                onDialogWheelOptionClick(i)
                dialog_wheel.selected_option = -1
                dialog_wheel.big_circle.current_angle = null
                option.container.alpha = 1
            })
        option.container.onpointerdown = function () {
            const start_color = hexToHsl(option.label.style.fill)
            const end_color = structuredClone(start_color)
            if (is_nighttime) {
                end_color.s = 0 // Blink to grey
            } else {
                end_color.l = 100 // Blink to white
            }

            option.blink_tween_1.from(start_color)
                .to(end_color)
                .on('start', () => {
                    option.container.alpha = 1
                })
                .on('update', () => {
                    option.label.style.fill = `hsl(${blink_color.h}, ${blink_color.s}%, ${blink_color.l}%)`
                }).start()
                .chain(option.blink_tween_2)
        }
        option.container.onmouseenter = function (event, auto_click_when_reached) {
            dialog_wheel.selected_option = i
            if (auto_click_when_reached) {
                dialog_wheel.auto_click_when_reached = true
            }
        }

        dialog_wheel.options[i] = option
    }

    {
        // Big circle for selected option
        dialog_wheel.big_circle = new PIXI.Graphics()
            .beginFill('white')
            .drawCircle(0, 0, dialog_wheel_option_radius.active)
            .endFill()
        dialog_wheel.big_circle.current_angle = null
        dialog_wheel.container.addChild(dialog_wheel.big_circle)
    }

    render_objects.dialog_wheel = dialog_wheel
}

function createMouseTracker(app) {
    const mouse_tracker = {}
    render_objects.mouse_tracker = mouse_tracker

    const container = new PIXI.Container()
    mouse_tracker.container = container
    app.stage.addChild(container)

    {
        const bounds = render_objects.dialog_wheel.ring.getBounds()
        const center = {
            x: bounds.x + bounds.width / 2,
            y: bounds.y + bounds.height / 2,
        }

        const center_circle = new PIXI.Graphics()
            .beginFill('white')
            .drawCircle(0, 0, mouse_tracker_center_circle_radius)
            .endFill()
        container.addChild(center_circle)
        mouse_tracker.center_circle = center_circle
    }

    {
        const track_circle = new PIXI.Graphics()
            .beginFill('white')
            .drawCircle(0, 0, mouse_tracker_track_circle_radius)
            .endFill()
        container.addChild(track_circle)
        mouse_tracker.track_circle = track_circle
    }

    {
        // Move track circle with mouse
        const offset = mouse_tracker_track_circle_radius / 2
        app.view.onmousemove = (e) => {
            mouse_tracker.track_circle.x = e.clientX - offset
            mouse_tracker.track_circle.y = e.clientY - offset
        }
    }

    mouse_tracker.line = new PIXI.Graphics()
    container.addChild(mouse_tracker.line)

    mouse_tracker.container.alpha = 0.7
}

function createText(font_size, container) {
    const style = {
        fontFamily: 'Coda',
        fontSize: font_size,
        fill: getDayNightColor(text_color),
        align: 'center',
        dropShadowColor: getDayNightColor(shadow_color),
        dropShadow: true,
        dropShadowBlur: 0.5,
        dropShadowAlpha: 0.5,
        dropShadowDistance: 2,
    }
    const text = new PIXI.Text('', style)
    text.resolution = 2 // Sharper text

    text.anchor.set(0, 0)
    text.x = 0
    text.y = 0
    container.addChild(text)
    return text
}

function render(app, state, onActionExpired, onMouseTrackerAnchorMoved, delta_time_s) {
    if (state.dialog_queue.length === 0) {
        app.stage.visible = false
        return
    }
    app.stage.visible = true

    updateActionTimer(state, onActionExpired, delta_time_s)

    // If everything expired
    if (state.dialog_queue.length === 0) {
        return
    }

    renderActionCircles(state)
    renderQuestion(state)
    renderDialogWheel(state, delta_time_s)
    renderMouseTracker(state, onMouseTrackerAnchorMoved)
}

function updateActionTimer(state, onActionExpired, delta_time_s) {
    if (state.dialog_index !== -1) {
        // Currently inside a dialog
        return
    }

    const expired_actions = []
    const shown_actions = Math.min(state.dialog_queue.length, render_objects.action_circles.circles.length)
    for (let i = 0; i < shown_actions; i++) {
        const dialog = state.dialog_queue[i]

        if (!dialog.hasOwnProperty("timing_s")) {
            dialog.timing_s = {}
        }
        const timing_s = dialog.timing_s

        // Attach timer
        if (!timing_s.hasOwnProperty("timer")) {
            timing_s.timer = 0
        }

        timing_s.timer += delta_time_s

        if (!timing_s.hasOwnProperty("action")) {
            continue
        }

        if (timing_s.timer > timing_s.action) {
            expired_actions.push(i)
        }
    }

    // Send reverse to ensure indices are still valid after removal
    for (let i = expired_actions.length - 1; i >= 0; i--) {
        onActionExpired(expired_actions[i])
    }
}

function renderQuestion(state) {
    let dialog
    let slot_index
    if (state.dialog_index === -1) {
        const newest_dialog_index = Math.min(state.dialog_queue.length, render_objects.action_circles.circles.length) - 1
        dialog = state.dialog_queue[newest_dialog_index]
        slot_index = dialog.slot
    } else {
        dialog = state.dialog
        slot_index = state.dialog_queue[state.dialog_index].slot
    }

    const question = render_objects.question
    question.jester_name.text = _(dialog.jester[0].toUpperCase())
    question.jester_text.text = ': ' + _(dialog.jester[1])

    // Center question at middle action circle (W), ignore "Jester" name for centering
    const offset = -2
    const center_x = 296
    const name_bounds = question.jester_name.getLocalBounds()
    const text_bounds = question.jester_text.getLocalBounds()
    const total_width = name_bounds.width + text_bounds.width + offset
    const name_x = center_x - (total_width / 2) - (name_bounds.width / 2)
    question.jester_name.x = name_x
    question.jester_text.x = name_x + name_bounds.width + offset

    question.jester_name.style.fill = getDayNightColor(dialog_slot_colors[slot_index])
    question.jester_name.style.dropShadowColor = getDayNightColor(shadow_color)
    question.jester_text.style.fill = getDayNightColor(text_color)
    question.jester_text.style.dropShadowColor = getDayNightColor(shadow_color)

    // Fade in/out
    let alpha = 1
    if (state.dialog_index === -1) {
        // TODO Somewhat broken if multiple questions on stage, or during dialog
        const fade_in_time_s = 0.4
        if (dialog.timing_s.timer < fade_in_time_s) {
            alpha = rangeLerp(0, 1, 0, fade_in_time_s, dialog.timing_s.timer)
        } else {
            // Fade out question
            let fade_factor = 0
            if (dialog.timing_s?.question) {
                fade_factor = dialog.timing_s.timer / dialog.timing_s.question
            }

            const start_fade_out_at = 0.9
            if (fade_factor >= start_fade_out_at) {
                alpha = rangeLerp(1, 0, start_fade_out_at, 1, fade_factor)
            }
        }
    }
    question.container.alpha = alpha
}

function renderActionCircles(state) {
    const action_circles = render_objects.action_circles

    let circles_to_show
    if (state.dialog_index === -1) {
        circles_to_show = Math.min(state.dialog_queue.length, action_circles.circles.length)
    } else {
        circles_to_show = 0
    }

    const available_slots = new Set([1, 2, 0]) // Favor W, E, Q in that order
    // Remove already occupied slots
    for (let i = 0; i < circles_to_show; i++) {
        const dialog = state.dialog_queue[i]
        if (dialog.hasOwnProperty("slot")) {
            available_slots.delete(dialog.slot)
        }
    }

    for (let i = 0; i < circles_to_show; i++) {
        const dialog = state.dialog_queue[i]
        if (!dialog.hasOwnProperty("slot")) {
            // Request a new slot
            dialog.slot = available_slots.values().next().value
            available_slots.delete(dialog.slot)
        }
        const slot = dialog.slot
        const circle = action_circles.circles[slot]

        circle.ring.background_obj.tint = getDayNightColor(action_circle_ring_color)
        if (!circle.blink_tween_1.active && !circle.blink_tween_2.active) {
            circle.ring.foreground_obj.tint = getDayNightColor(dialog_slot_colors[slot])
        }

        circle.label.text = _(dialog.label.toUpperCase())
        circle.label.style.fill = getDayNightColor(text_color)
        circle.label.style.dropShadowColor = getDayNightColor(shadow_color)
        circle.symbol.style.fill = getDayNightColor(text_color)
        circle.symbol.style.dropShadowColor = getDayNightColor(shadow_color)

        let expiration_factor = 0
        if (dialog.timing_s?.action) {
            expiration_factor = dialog.timing_s.timer / dialog.timing_s.action
        }
        {
            // Ring mask
            const bounds = circle.ring.foreground.getBounds()

            const angle_offset = Math.PI / 2
            const start_angle = 2 * Math.PI * expiration_factor + angle_offset + 0.0001
            const end_angle = angle_offset

            const width = action_circle_ring_border
            const radius = action_circle_ring_radius - (width / 2)
            const center_x = bounds.x + bounds.width / 2
            const center_y = bounds.y + bounds.height / 2

            const mask = circle.ring.foreground.mask
            mask.clear()
            mask.lineStyle(width, 1)

            // Masks seem to be buggy with canvas rendering, requiring these hacks
            mask.moveTo(center_x, center_y)
            const radius_to_use = radius * 2

            mask.arc(center_x, center_y, radius_to_use, start_angle, end_angle)
            mask.endFill()
        }

        // Fade In/Out
        {
            let alpha = 1
            const fade_in_time_s = 0.2
            const start_fade_out_at_factor = 0.5

            if (dialog.timing_s.timer < fade_in_time_s) {
                alpha = rangeLerp(0, 1, 0, fade_in_time_s, dialog.timing_s.timer)
            } else if (expiration_factor >= start_fade_out_at_factor) {
                alpha = rangeLerp(0, 1, 1, start_fade_out_at_factor, expiration_factor)
            }

            circle.container.alpha = alpha
        }

        circle.container.visible = true
    }

    for (const unoccupied_slot of available_slots) {
        action_circles.circles[unoccupied_slot].container.visible = false
    }
}

function renderDialogWheel(state, delta_time_s) {
    const dialog_wheel = render_objects.dialog_wheel
    if (state.dialog_index === -1) {
        dialog_wheel.container.visible = false
        return
    }
    dialog_wheel.container.visible = true

    dialog_wheel.ring.tint = getDayNightColor(text_color)

    const current_option_amount = state.dialog.options.length

    let start_angle
    switch (current_option_amount) {
        case 1:
            start_angle = 0
            break
        case 2:
            start_angle = 0
            break
        case 3:
            start_angle = -1 * Math.PI / 3
            break
        case 4:
            start_angle = -3 * Math.PI / 4
            break
        default:
            start_angle = 0
    }
    const angle_step = 2 * Math.PI / current_option_amount

    const bounds = render_objects.dialog_wheel.ring.getLocalBounds()
    const center = {
        x: bounds.x + bounds.width / 2,
        y: bounds.y + bounds.height / 2,
    }
    const radius = dialog_wheel_ring_radius - dialog_wheel_ring_border / 2

    const options = render_objects.dialog_wheel.options
    let angle = start_angle

    // Active options
    for (let i = 0; i < current_option_amount; i++) {
        const option = options[i]

        option.container.x = center.x + radius * Math.cos(angle)
        option.container.y = center.y + radius * Math.sin(angle)
        option.circle.tint = getDayNightColor(text_color)

        option.label.text = _(state.dialog.options[i].option)
        option.label.style.dropShadowColor = getDayNightColor(shadow_color)

        option.press_text.style.dropShadowColor = getDayNightColor(shadow_color)

        if (dialog_wheel.selected_option === i) {
            if (!option.blink_tween_1.active && !option.blink_tween_2.active) {
                const current_slot = state.dialog_queue[state.dialog_index].slot
                option.label.style.fill = getDayNightColor(dialog_slot_colors[current_slot])
            }
        } else {
            option.label.style.fill = getDayNightColor(text_color)
        }
        option.press_text.style.fill = getDayNightColor(text_color)

        const text_bounds = option.label.getLocalBounds()
        const offset = 20
        const normalized_angle = angle % (2 * Math.PI)
        const is_right_half = -Math.PI / 2 <= normalized_angle && normalized_angle <= Math.PI / 2
        if (is_right_half) {
            option.label.x = option.circle.x + offset
        } else {
            option.label.x = option.circle.x - text_bounds.width - offset
        }
        option.label.y = option.circle.y - text_bounds.height / 2

        angle += angle_step

        option.container.visible = true
    }

    switch (current_option_amount) {
        case 1:
            options[0].press_text.x = options[0].circle.x - 20
            options[0].press_text.y = options[0].circle.y - 9
            break
        case 2:
            options[0].press_text.x = options[0].circle.x - 20
            options[0].press_text.y = options[0].circle.y - 9

            options[1].press_text.x = options[1].circle.x + 15
            options[1].press_text.y = options[1].circle.y - 9
            break
        case 3:
            options[0].press_text.x = options[0].circle.x - 13
            options[0].press_text.y = options[0].circle.y + 8

            options[1].press_text.x = options[1].circle.x - 10
            options[1].press_text.y = options[1].circle.y - 24

            options[2].press_text.x = options[2].circle.x + 13
            options[2].press_text.y = options[2].circle.y - 8
            break
        case 4:
            options[0].press_text.x = options[0].circle.x + 8
            options[0].press_text.y = options[0].circle.y

            options[1].press_text.x = options[1].circle.x - 13
            options[1].press_text.y = options[1].circle.y

            options[2].press_text.x = options[2].circle.x - 15
            options[2].press_text.y = options[2].circle.y - 20

            options[3].press_text.x = options[3].circle.x + 8
            options[3].press_text.y = options[3].circle.y - 20
            break
    }

    // Inactive options
    for (let i = current_option_amount; i < options.length; i++) {
        const option = options[i]
        option.container.visible = false
    }

    // Selected option
    {
        const big_circle = dialog_wheel.big_circle

        if (dialog_wheel.selected_option === -1) {
            big_circle.visible = false
        } else {
            const target_angle = (start_angle + dialog_wheel.selected_option * angle_step)
            const current_angle = big_circle.current_angle ?? target_angle
            let target_angle_normalized = (target_angle - start_angle) % (2 * Math.PI)
            let current_angle_normalized = (current_angle - start_angle) % (2 * Math.PI)

            // Tweaks to move around shortest path on the circle
            if (current_angle_normalized === 0 && target_angle_normalized > Math.PI) {
                current_angle_normalized = 2 * Math.PI
            } else if (target_angle_normalized === 0 && current_angle_normalized > Math.PI) {
                target_angle_normalized = 2 * Math.PI
            }

            current_angle_normalized = moveTo(current_angle_normalized, target_angle_normalized, selected_option_speed_deg_per_s, delta_time_s)
            const angle_before = big_circle.current_angle
            big_circle.current_angle = current_angle_normalized + start_angle

            big_circle.x = center.x + radius * Math.cos(big_circle.current_angle)
            big_circle.y = center.y + radius * Math.sin(big_circle.current_angle)

            big_circle.tint = getDayNightColor(text_color)

            big_circle.visible = true

            if (dialog_wheel.auto_click_when_reached && big_circle.current_angle === angle_before) {
                dialog_wheel.auto_click_when_reached = false
                options[dialog_wheel.selected_option].container.onpointerdown()
            }
        }
    }
}

function renderMouseTracker(state, onMouseTrackerAnchorMoved) {
    const mouse_tracker = render_objects.mouse_tracker
    if (!is_tracking_mouse) {
        mouse_tracker.container.visible = false
        return
    }

    const mouse_anchor = mouse_tracker.center_circle
    const current_x = mouse_anchor.x
    const current_y = mouse_anchor.y
    if (state.dialog_index === -1) {
        const center = render_objects.action_circles.circles[1].container

        mouse_anchor.x = center.x
        mouse_anchor.y = center.y + 80
    } else {
        const bounds = render_objects.dialog_wheel.ring.getBounds()
        const center = {
            x: bounds.x + bounds.width / 2,
            y: bounds.y + bounds.height / 2,
        }

        mouse_anchor.x = center.x
        mouse_anchor.y = center.y
    }

    if (current_x !== mouse_anchor.x || current_y !== mouse_anchor.y) {
        onMouseTrackerAnchorMoved(mouse_anchor.x, mouse_anchor.y)
    }

    mouse_tracker.container.visible = true

    // Draw line from center to mouse
    // Offset line a bit inwards to avoid overlapping the circles, otherwise the alpha adds up
    const center_bounds = mouse_anchor.getBounds()
    const offset = mouse_tracker_center_circle_radius
    const center_x = center_bounds.x + offset
    const center_y = center_bounds.y + offset

    const delta_x = mouse_tracker.track_circle.x - center_x
    const delta_y = mouse_tracker.track_circle.y - center_y
    const length = Math.sqrt(delta_x * delta_x + delta_y * delta_y)
    const delta_x_norm = delta_x / length
    const delta_y_norm = delta_y / length

    const overlap_px = 1

    mouse_tracker.line.clear()
    mouse_tracker.line.lineStyle(mouse_tracker_line_width, 'white')
        .moveTo(
            center_x + delta_x_norm * (mouse_tracker_center_circle_radius - overlap_px),
            center_y + delta_y_norm * (mouse_tracker_center_circle_radius - overlap_px)
        )
        .lineTo(
            mouse_tracker.track_circle.x - delta_x_norm * (mouse_tracker_track_circle_radius - overlap_px),
            mouse_tracker.track_circle.y - delta_y_norm * (mouse_tracker_track_circle_radius - overlap_px)
        )

    mouse_tracker.center_circle.tint = getDayNightColor(tracker_color)
    mouse_tracker.track_circle.tint = getDayNightColor(tracker_color)
    mouse_tracker.line.tint = getDayNightColor(tracker_color)
}

function getDayNightColor(color) {
    return is_nighttime ? color.night : color.day
}

function moveTo(current, target, speed, delta_time) {
    const change = speed * delta_time

    if (current === target) {
        return current
    } else if (current < target) {
        return Math.min(current + change, target)
    } else if (current > target) {
        return Math.max(current - change, target)
    }
}

// Factor between 0 and 1
function lerp(min, max, factor) {
    const range = max - min
    return min + range * factor
}

function inverseLerp(min, max, value) {
    const range = max - min
    return (value - min) / range
}

function rangeLerp(target_min, target_max, source_min, source_max, source_value) {
    const source_factor = inverseLerp(source_min, source_max, source_value)
    return lerp(target_min, target_max, source_factor)
}

function hexToHsl(hex_color) {
    const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex_color)

    let r = parseInt(result[1], 16)
    let g = parseInt(result[2], 16)
    let b = parseInt(result[3], 16)

    r /= 255
    g /= 255
    b /= 255

    const max = Math.max(r, g, b)
    const min = Math.min(r, g, b)

    let h, s, l = (max + min) / 2

    if (max === min) {
        h = s = 0 // achromatic
    } else {
        const d = max - min
        s = l > 0.5
            ? d / (2 - max - min)
            : d / (max + min)
        switch (max) {
            case r:
                h = (g - b) / d + (g < b ? 6 : 0)
                break;
            case g:
                h = (b - r) / d + 2
                break;
            case b:
                h = (r - g) / d + 4
                break;
        }
        h /= 6;
    }

    s = s * 100;
    s = Math.round(s);
    l = l * 100;
    l = Math.round(l);
    h = Math.round(360 * h);

    return {
        'h': h,
        's': s,
        'l': l,
    }
}
