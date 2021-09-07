using System;
using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using Supyrb;
using UnityEngine;

public class SpaceInflatorSystem : GameSystem, IIniting {
    public event Action<List<RectForFill>> InflateSpaceForRectsEvent;
    public event Action<Vector2Int, Vector2Int> NewSpacePointsEvent;

    private Vector2Int _leftBottomSpacePoint = Vector2Int.zero;
    private Vector2Int _rightTopSpacePoint = Vector2Int.zero;

    private readonly List<RectForFill> _cachedRectsList = new List<RectForFill>();

    private int _spaceZoomValue = 1;

    private bool _systemInitialized = false;

    void IIniting.OnInit() {
        if (!_systemInitialized) {
            Signals.Get<ZoomEventSignal>().AddListener(OnZoom, 0);
            NewSpacePointsEvent?.Invoke(_leftBottomSpacePoint, _rightTopSpacePoint);
        }
    }

    private void OnZoom(int nowZoomValue) {
        if (_spaceZoomValue < nowZoomValue) {
            InflateSpaceForDeltaZoom(nowZoomValue - _spaceZoomValue);
        }

        _spaceZoomValue = nowZoomValue;
    }

    private void InflateSpaceForDeltaZoom(int deltaZoom) {
        int halfDeltaZoom = deltaZoom / 2;

        int oldWidth = _rightTopSpacePoint.x - _leftBottomSpacePoint.x + 1;
        int oldHeight = _rightTopSpacePoint.y - _leftBottomSpacePoint.y + 1;

        int newWidth = _rightTopSpacePoint.x - _leftBottomSpacePoint.x + deltaZoom + 1;
        int newHeight = _rightTopSpacePoint.y - _leftBottomSpacePoint.y + deltaZoom + 1;

        _leftBottomSpacePoint -= new Vector2Int(halfDeltaZoom, halfDeltaZoom);
        _rightTopSpacePoint += new Vector2Int(halfDeltaZoom, halfDeltaZoom);

        _cachedRectsList.Clear();

        // inflate for ring
        _cachedRectsList.Add(new RectForFill(_leftBottomSpacePoint,
            new Vector2Int(_rightTopSpacePoint.x, _leftBottomSpacePoint.y + halfDeltaZoom - 1))); // bottom;
        _cachedRectsList.Add(new RectForFill(
            new Vector2Int(_leftBottomSpacePoint.x, _rightTopSpacePoint.y - halfDeltaZoom + 1),
            _rightTopSpacePoint)); // top
        _cachedRectsList.Add(new RectForFill(new Vector2Int(_leftBottomSpacePoint.x, _leftBottomSpacePoint.y + halfDeltaZoom), new Vector2Int(_leftBottomSpacePoint.x + halfDeltaZoom - 1, _rightTopSpacePoint.y - halfDeltaZoom))); // left
        _cachedRectsList.Add(new RectForFill(new Vector2Int(_rightTopSpacePoint.x - halfDeltaZoom + 1, _leftBottomSpacePoint.y + halfDeltaZoom), new Vector2Int(_rightTopSpacePoint.x, _rightTopSpacePoint.y - halfDeltaZoom))); // right

        InflateSpaceForRectsEvent?.Invoke(_cachedRectsList);
        NewSpacePointsEvent?.Invoke(_leftBottomSpacePoint, _rightTopSpacePoint);
    }
}