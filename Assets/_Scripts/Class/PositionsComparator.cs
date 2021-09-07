using System;
using System.Collections.Generic;
using UnityEngine;

public class PositionsComparator : IComparer<Vector2> {
    public int Compare(Vector2 first, Vector2 second) {
        if (first == second) return 0;
        if (Mathf.Abs(first.x - second.x) < Single.Epsilon) return first.y < second.y ? -1 : 1;
        else return first.x < second.x ? -1 : 1;
    }
}