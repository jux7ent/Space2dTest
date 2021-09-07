using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo {
    public Vector2 Position;
    public int Score;

    public PlanetInfo(Vector2 position, int score) {
        Position = position;
        Score = score;
    }
}

public class PlanetsComparator : IEqualityComparer<PlanetInfo> {
    public bool Equals(PlanetInfo first, PlanetInfo second) {
        if (first == null || second == null) return false;
        return first.Position == second.Position;
    }

    public int GetHashCode(PlanetInfo obj) {
        unchecked {
            return (obj.Position.GetHashCode() * 397) ^ obj.Score;
        }
    }
}