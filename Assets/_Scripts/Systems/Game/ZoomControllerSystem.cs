using System;
using Kuhpik;using Supyrb;
using UnityEngine;

public class ZoomEventSignal : Signal<int> { } // current zoom value

public class ZoomControllerSystem : GameSystem, IIniting, IUpdating {
    [SerializeField] private int _zoomSensitivity = 12;

    void IIniting.OnInit() {
        game.Zoom = config.MinZoom;
        Signals.Get<ZoomEventSignal>().Dispatch(game.Zoom);
    }

    void IUpdating.OnUpdate() {
        float val = Input.GetAxis("Mouse ScrollWheel");
        if (val != 0f) {
            game.Zoom = Mathf.Clamp(game.Zoom + (val > 0 ? _zoomSensitivity : -_zoomSensitivity), config.MinZoom,
                config.MaxZoom);

            Signals.Get<ZoomEventSignal>().Dispatch(game.Zoom);
        }
    }
}