using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class ZoneDrawerSystem : GameSystem, IIniting {
    [SerializeField] private LineRenderer _lineRenderer;

    private bool _systemInitialized = false;


    void IIniting.OnInit() {
        if (!_systemInitialized) {
            _lineRenderer.positionCount = 5;
            Bootstrap.GetSystem<SpaceInflatorSystem>().NewSpacePointsEvent += OnNewSpacePoints;
        }
    }

    private void OnNewSpacePoints(Vector2Int leftBottomPoint, Vector2Int rightTopPoint) {
        DrawZone(((Vector2) leftBottomPoint) - Vector2.one / 2f/* * Sqrt2 * 0.5f*/,
            ((Vector2) rightTopPoint) + Vector2.one / 2f/* * Sqrt2 * 0.5f*/);
    }

    private void DrawZone(Vector2 leftBottomPoint, Vector2 rightTopPoint) {
        _lineRenderer.SetPosition(0, leftBottomPoint);
        _lineRenderer.SetPosition(1, new Vector3(leftBottomPoint.x, rightTopPoint.y));
        _lineRenderer.SetPosition(2, rightTopPoint);
        _lineRenderer.SetPosition(3, new Vector3(rightTopPoint.x, leftBottomPoint.y));
        _lineRenderer.SetPosition(4, leftBottomPoint);
    }
}