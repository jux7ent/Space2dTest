using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using Supyrb;
using UnityEngine;

public class CameraControllerSystem : GameSystem, IIniting {
    [SerializeField] private GridDrawer _gridDrawer;
    [SerializeField] private int _thresholdZoomValueToGridOff = 200;

    [SerializeField] private Transform _visibleZoneTransform;
    [SerializeField] private float _thresholdScaleForVisibleZone = 0.2f;

    private bool _systemInitialized = false;


    void IIniting.OnInit() {
        if (!_systemInitialized) {
            Signals.Get<ZoomEventSignal>().AddListener(UpdateCameraSettings, 0);

            _systemInitialized = true;
        }

        // MakeCameraSquare();
    }

    private void MakeCameraSquare() {
        float cameraWidth = (float) Screen.height / Screen.width;
        game.MainCamera.rect = new Rect((1 - cameraWidth) * 0.5f, 0f, cameraWidth, 1f);
    }

    private void UpdateCameraSettings(int newZoom) {
        game.MainCamera.orthographicSize = newZoom / 2f;

        _gridDrawer.enabled = newZoom < _thresholdZoomValueToGridOff;
        _visibleZoneTransform.localScale = new Vector3(newZoom + _thresholdScaleForVisibleZone,
            newZoom + _thresholdScaleForVisibleZone, 1f);
    }
}