using UnityEngine;

public struct RectForFill {
    public readonly Vector2Int LeftBottom;
    public readonly Vector2Int RightTop;

    public readonly int Area;

    public bool ThereIsFreeUnit => _freeArea > 0;

    private int _freeArea;

    public RectForFill(Vector2Int leftBottom, Vector2Int rightTop) {
        LeftBottom = leftBottom;
        RightTop = rightTop;

        Area = (RightTop.x - LeftBottom.x + 1) * (RightTop.y - LeftBottom.y + 1);
        
        _freeArea = Area;
    }

    public void FillUnit() {
        _freeArea -= 1;
    }
}