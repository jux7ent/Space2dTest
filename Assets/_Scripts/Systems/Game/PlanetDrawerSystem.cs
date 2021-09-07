using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kuhpik;
using UnityEngine;

public class PlanetDrawerSystem : GameSystem, IIniting, IUpdating {
    [SerializeField] private Material _planetMaterial;

    private Vector3[] _texCoords = new Vector3[4];
    private Vector2[] _offsets = new Vector2[4];
    

    void IIniting.OnInit() {
        Init();
    }

    private void Init() {
        _texCoords[0] = new Vector3(0, 0, 0);
        _texCoords[1] = new Vector3(0, 1, 0);
        _texCoords[2] = new Vector3(1, 1, 0);
        _texCoords[3] = new Vector3(1, 0, 0);

        _offsets[0] = new Vector2(-0.5f, -0.5f);
        _offsets[1] = new Vector2(-0.5f, 0.5f);
        _offsets[2] = new Vector2(0.5f, 0.5f);
        _offsets[3] = new Vector2(0.5f, -0.5f);
    }

    private void OnRenderObject() {
        lock (game.PlanetPositionsToDraw) {
            _planetMaterial.SetPass(0);
            
            GL.Begin(GL.QUADS);
        
            foreach (var planetPos in game.PlanetPositionsToDraw) {
                GL.TexCoord(_texCoords[0]);
                GL.Vertex3(planetPos.x + _offsets[0].x, planetPos.y + _offsets[0].y, 0f);
                
                GL.TexCoord(_texCoords[1]);
                GL.Vertex3(planetPos.x + _offsets[1].x, planetPos.y + _offsets[1].y, 0f);
                
                GL.TexCoord(_texCoords[2]);
                GL.Vertex3(planetPos.x + _offsets[2].x, planetPos.y + _offsets[2].y, 0f);
                
                GL.TexCoord(_texCoords[3]);
                GL.Vertex3(planetPos.x + _offsets[3].x, planetPos.y + _offsets[3].y, 0f);
            }
        
        
            GL.End();
        }
        
        /*
        GL.Begin(GL.LINES);
        
        if (game.PlanetsToDraw != null) {
            foreach (var planet in game.PlanetsToDraw) {
                GL.Vertex3(planet.Point[0], planet.Point[1], 0f);
                GL.Vertex((Vector2)game.Ship.transform.position);
            }
        }
        
        GL.End();*/
        
    }

    void IUpdating.OnUpdate() {
        if (Time.frameCount % 1000 == 0) {
            int halfZoom = Mathf.Clamp((int)(game.Zoom / 2f), 0, 100);

         /*   game.PlanetsToDraw =
                game.PlanetsRTree.GetNearestNeighbours(
                    new[] {game.Ship.transform.position.x, game.Ship.transform.position.y}, 10);*/
        }
    }
}