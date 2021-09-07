using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Kuhpik;
using Supyrb;
using UnityEngine;

public class PlanetsCameraCutterSystem : GameSystem, IIniting {
    private bool _systemInitialized = false;

    private Thread _threadCameraPlanetsCatter;
    private List<Vector2> _cachedPlanetsToDrawForSwap = new List<Vector2>();
    private float[] _cachedShipPositionArr = new float[2];
    private CancellationTokenSource _cancellationTokenSource;

    void IIniting.OnInit() {
        if (!_systemInitialized) {
            Signals.Get<ZoomEventSignal>().AddListener(OnZoomEvent, 1);
            Signals.Get<NewShipPositionSignal>().AddListener(OnMoveEvent, 1);
            _systemInitialized = true;
        }
    }

    private void OnMoveEvent(Vector2 newShipPosition) {
        StartSelectDrawableThread(game.Zoom, newShipPosition);
    }

    private void OnZoomEvent(int newZoomValue) {
        StartSelectDrawableThread(newZoomValue, game.Ship.transform.position);
    }

    private void StartSelectDrawableThread(int zoomValue, Vector2 shipPosition) {
        if (_threadCameraPlanetsCatter != null && _threadCameraPlanetsCatter.IsAlive)
            _threadCameraPlanetsCatter.Abort();

        _threadCameraPlanetsCatter =
            new Thread(() => SelectDrawablePlanets(zoomValue, shipPosition));
        _threadCameraPlanetsCatter.Start();
    }

    private void SelectDrawablePlanets(int newZoomValue, Vector2 shipPosition) {
        lock (game.PlanetsKdTree) {
            // не должны попасть в дедлок
            lock (_cachedShipPositionArr) {
                _cachedShipPositionArr[0] = shipPosition.x;
                _cachedShipPositionArr[1] = shipPosition.y;

                lock (_cachedPlanetsToDrawForSwap) {
                    var nodes = game.PlanetsKdTree.RadialSearch(_cachedShipPositionArr, GetCircleRadiusSurroundingSquare(newZoomValue));

                    for (int i = 0; i < nodes.Length; ++i) {
                        _cachedPlanetsToDrawForSwap.Add(new Vector2(nodes[i].Point[0], nodes[i].Point[1]));
                    }


                    lock (game.PlanetPositionsToDraw) {
                        List<Vector2> temp = game.PlanetPositionsToDraw;
                        game.PlanetPositionsToDraw = _cachedPlanetsToDrawForSwap;
                        _cachedPlanetsToDrawForSwap = temp;
                        _cachedPlanetsToDrawForSwap.Clear();
                    }
                }
            }
        }
    }

    private float GetCircleRadiusSurroundingSquare(float squareSide) {
        return squareSide / Constants.Math.Sqrt2;
    }
}