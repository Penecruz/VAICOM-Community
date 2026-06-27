// Appended by VAICOM: keep JesterDialog logic active but hide its pixi visual canvas.

const dialogVisibleState = {
    app: {
        opacity: 0,
        pointerEvents: 'none'
    },
    container: {
        visible: false,
        alpha: 0
    }
};

const vaicomOriginalRender = render
render = function (app, state, onActionExpired, onMouseTrackerAnchorMoved, delta_time_s) {
    vaicomOriginalRender(app, state, onActionExpired, onMouseTrackerAnchorMoved, delta_time_s)

    if (app && app.view && app.view.style) {
        app.view.style.opacity = dialogVisibleState.app.opacity;
        app.view.style.pointerEvents = dialogVisibleState.app.pointerEvents;
    }

    if (render_objects && render_objects.mouse_tracker && render_objects.mouse_tracker.container) {
        render_objects.mouse_tracker.container.visible = dialogVisibleState.container.visible;
        render_objects.mouse_tracker.container.alpha = dialogVisibleState.container.alpha;
    }
}

window.showJesterDialog = function() {
    dialogVisibleState.app.opacity = 0.9;
    dialogVisibleState.app.pointerEvents = 'inherit';
    dialogVisibleState.container.visible = false;
    dialogVisibleState.container.alpha = 0.7;
}

window.hideJesterDialog = function() {
    dialogVisibleState.app.opacity = 0;
    dialogVisibleState.app.pointerEvents = 'none';
    dialogVisibleState.container.visible = false;
    dialogVisibleState.container.alpha = 0;
}
