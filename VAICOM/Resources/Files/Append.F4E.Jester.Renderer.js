// Appended by VAICOM: keep JesterDialog logic active but hide its pixi visual canvas.
const vaicomOriginalRender = render
render = function (app, state, onActionExpired, onMouseTrackerAnchorMoved, delta_time_s) {
    vaicomOriginalRender(app, state, onActionExpired, onMouseTrackerAnchorMoved, delta_time_s)

    if (app && app.view && app.view.style) {
        app.view.style.opacity = '0'
        app.view.style.pointerEvents = 'none'
    }

    if (render_objects && render_objects.mouse_tracker && render_objects.mouse_tracker.container) {
        render_objects.mouse_tracker.container.visible = false
        render_objects.mouse_tracker.container.alpha = 0
    }
}
