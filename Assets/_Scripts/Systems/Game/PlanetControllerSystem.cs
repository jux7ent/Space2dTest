using System;
using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetControllerSystem : GameSystem, IIniting {
    private bool _systemInitialized = false;
    private float _restPlanetsCount = 0f;

    void IIniting.OnInit() {
        game.PlanetPositionsSet.Clear();

        if (!_systemInitialized) {
            Bootstrap.GetSystem<SpaceInflatorSystem>().InflateSpaceForRectsEvent += OnInflateSpaceForSpaceForRects;
        }
    }

    private void OnInflateSpaceForSpaceForRects(List<RectForFill> rectsList) {
        int rectsArea = CalculateRectsArea(rectsList);
        FillRectsWithPlanets(rectsArea, rectsList);
    }

    private int CalculateRectsArea(List<RectForFill> rectsList) {
        int area = 0;
        for (int i = 0; i < rectsList.Count; ++i) area += rectsList[i].Area;
        return area;
    }

    private void FillRectsWithPlanets(int rectsArea, List<RectForFill> rectsList) {
        int planetsSpawned = 0;

        int additionalPlanetsCount = Mathf.FloorToInt(rectsArea * config.PlanetsPercentage);

        _restPlanetsCount += rectsArea * config.PlanetsPercentage - additionalPlanetsCount;

        if (_restPlanetsCount >= 1) {
            additionalPlanetsCount += 1;
            _restPlanetsCount -= Mathf.Floor(_restPlanetsCount);
        }

        lock (game.PlanetPositionsSet) {
            while (planetsSpawned < additionalPlanetsCount) {
                RectForFill randomRect = SelectRandomRect(rectsArea, rectsList);
                while (!randomRect.ThereIsFreeUnit) {
                    randomRect = SelectRandomRect(rectsArea, rectsList);
                }
            
                int iterationsCount = 0;

                Vector2Int randomPosition;
                do {
                    randomPosition = GetRandomPositionOnRect(randomRect);
                    ++iterationsCount;
                } while (game.PlanetPositionsSet.Contains(randomPosition) && iterationsCount < 1000);

                if (iterationsCount >= 1000) {
                    throw new Exception("Attempt to spawn planets failed");
                }

                PlanetInfo newPlanet = new PlanetInfo(randomPosition, Random.Range(0, 1000000));

                game.PlanetPositionsSet.Add(newPlanet.Position);
                game.PlanetsSortedList.Add(newPlanet.Position, newPlanet);
                game.PlanetsKdTree.Add(new[] { (float)newPlanet.Position.x, (float)newPlanet.Position.y}, newPlanet.Score);

                ++planetsSpawned;

                randomRect.FillUnit();
            }
        }
    }

    private RectForFill SelectRandomRect(int rectsArea, List<RectForFill> rectsList) {
        int randomVal = Random.Range(0, rectsArea);

        int currentArea = 0;
        for (int i = 0; i < rectsList.Count; ++i) {
            currentArea += rectsList[i].Area;

            if (randomVal <= currentArea) {
                return rectsList[i];
            }
        }

        throw new Exception("RandomRect selection error");
    }

    private Vector2Int GetRandomPositionOnRect(RectForFill rect) {
        return new Vector2Int(Random.Range(rect.LeftBottom.x, rect.RightTop.x + 1),
            Random.Range(rect.LeftBottom.y, rect.RightTop.y + 1));
    }
}