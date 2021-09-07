using System;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Square {
    public Vector3 point1;
    public Vector3 point2;
    public Vector3 point3;
    public Vector3 point4;
    public Vector3 pivot;

    public Rect rect;

    private float _distFromCamera;

    public Square(Vector2 pivot) {
        this.pivot = pivot;
        point1 = pivot + new Vector2(-0.5f, -0.5f);
        point2 = pivot + new Vector2(-0.5f, 0.5f);
        point3 = pivot + new Vector2(0.5f, 0.5f);
        point4 = pivot + new Vector2(0.5f, -0.5f);

        rect = new Rect(pivot.x, pivot.y, 1f, 1f);

        _distFromCamera = pivot.magnitude;
    }

    public bool Visible(float cameraOrtSize) {
        return cameraOrtSize > _distFromCamera;
    }
}

public class GridDrawer : MonoBehaviour {
    [SerializeField] private float _meshSize = 20;
    [SerializeField] private Vector2 _center = Vector2.zero;
    [SerializeField] private float _cellSize = 1f;

    [SerializeField] private Material _lineMat;
    [SerializeField] private Color _lineColor;
    
    private readonly List<Vector2[]> _linePoints = new List<Vector2[]>();
    void Start() {
        InitializeGridCoordinates();
    }

    void InitializeGridCoordinates() {
        _linePoints.Clear();
        
        int rowCount = (int) (_meshSize / _cellSize) + 1;

        for (int i = 0; i < rowCount; i++) {
            Vector2[] points = new Vector2[2];
            points[0] = new Vector2(-_meshSize / 2, _meshSize / 2 - _cellSize * i) + _center;
            points[1] = new Vector2(_meshSize / 2, _meshSize / 2 - _cellSize * i) + _center;

            _linePoints.Add(points);
        }

        for (int i = 0; i < rowCount; i++) {
            Vector2[] points = new Vector2[2];
            points[0] = new Vector2(-_meshSize / 2 + _cellSize * i, _meshSize / 2) + _center;
            points[1] = new Vector2(-_meshSize / 2 + _cellSize * i, -_meshSize / 2) + _center;

            _linePoints.Add(points);
        }

        _lineMat.SetColor("_Color", _lineColor);
    }
    
    void OnPostRender() {
        _lineMat.SetPass(0);

        GL.PushMatrix();

        DrawLines();

        GL.PopMatrix();
    }

    private void DrawLines() {
        GL.Begin(GL.LINES);
        for (int i = 0; i < _linePoints.Count; i++) {
            GL.Vertex(_linePoints[i][0]);
            GL.Vertex(_linePoints[i][1]);
        }
        GL.End();
    }
}