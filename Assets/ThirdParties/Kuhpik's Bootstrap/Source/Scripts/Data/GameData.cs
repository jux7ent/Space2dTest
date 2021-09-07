using System.Collections.Generic;
using KdTree;
using KdTree.Math;
using UnityEngine;

namespace Kuhpik {
    public class GameData {
        public Camera MainCamera { get; private set; }
        public Camera UICamera { get; private set; }
        public Transform MainCameraTransform { get; private set; }

        public readonly HashSet<Vector2> PlanetPositionsSet = new HashSet<Vector2>();

        public readonly SortedList<Vector2, PlanetInfo> PlanetsSortedList =
            new SortedList<Vector2, PlanetInfo>(new PositionsComparator()); // map

        public List<Vector2> PlanetPositionsToDraw = new List<Vector2>();
        public readonly KdTree<float, int> PlanetsKdTree = new KdTree<float, int>(2, new FloatMath()); // pos, score

        public int Zoom;

        public Ship Ship { get; private set; }

        public void LoadSceneData(Camera mainCamera, Camera uiCamera) {
            MainCamera = mainCamera;
            UICamera = uiCamera;
            MainCameraTransform = MainCamera.transform;
        }

        public void SetShip(Ship newShip, Vector2Int startPosition) {
            Ship = newShip;
            Ship.SetShipPosition(startPosition.x, startPosition.y);
        }
    }
}