using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KdTree;
using Kuhpik;
using Supyrb;
using TMPro;
using UnityEngine;

public class ShowKNearestRatingsSystem : GameSystem, IIniting {
    [SerializeField] private int _howManyNearestToShow = 10;
    [SerializeField] private Transform _ratingTMPsContainer;

    private bool _systemInitialized = false;

    private Thread _calculateNearestThread;
    private float[] _cachedShipPosition = new float[2];

    private List<TextMeshPro> _ratingTMPs;

    void IIniting.OnInit() {
        if (!_systemInitialized) {
            _systemInitialized = true;

            Signals.Get<NewShipPositionSignal>().AddListener(ShowNearest);
            _ratingTMPs = GameExtensions.GetChildAsSortedList<TextMeshPro>(_ratingTMPsContainer);
        }
    }

    private void ShowNearest(Vector2 newShipPosition) {
        if (_calculateNearestThread != null && _calculateNearestThread.IsAlive) _calculateNearestThread.Abort();

        Task<KdTreeNode<float, int>[]> task = CalculateNearestPlanets(newShipPosition);

        task.ContinueWith((result) => {
            if (result.IsCompleted) {
                KdTreeNode<float, int>[] nodes = result.Result;

                Bootstrap.InvokeInMainThread(() => {
                    for (int i = 0; i < _ratingTMPs.Count; ++i) {
                        // можно кешировать строки
                        if (i >= nodes.Length) _ratingTMPs[i].text = "";
                        else {
                            _ratingTMPs[i].text = $"{nodes[i].Value}";
                            _ratingTMPs[i].transform.position = new Vector2(nodes[i].Point[0], nodes[i].Point[1]);
                        }
                    }
                });
            }
        });

        task.Start();
    }

    private Task<KdTreeNode<float, int>[]> CalculateNearestPlanets(Vector2 newShipPosition) {
        return new Task<KdTreeNode<float, int>[]>(() => GetNearestNeighbours(newShipPosition));
    }

    private KdTreeNode<float, int>[] GetNearestNeighbours(Vector2 newShipPosition) {
        lock (game.PlanetsKdTree) {
            _cachedShipPosition[0] = newShipPosition.x;
            _cachedShipPosition[1] = newShipPosition.y;

            return game.PlanetsKdTree.GetNearestNeighbours(_cachedShipPosition, _howManyNearestToShow);
        }
    }
}