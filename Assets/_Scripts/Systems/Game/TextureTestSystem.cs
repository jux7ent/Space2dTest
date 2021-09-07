using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class TextureTestSystem : GameSystem, IIniting {
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private int _spaceSize = 1000;

    private int _pixelsForShip = 2;
    private int _pixelsForGrid = 1;
    
    void IIniting.OnInit() {
        int width = _spaceSize * _pixelsForShip * _pixelsForGrid * 2 + 1;
        Debug.Log($"width: {width}");
        
        Texture2D texture = new Texture2D(width, width, TextureFormat.ARGB32, false);

        for (int i = 0; i < width; ++i) {
            for (int j = 0; j < width; ++j) {
                texture.SetPixel(i, j, Color.white);
            }
        }

        for (int i = 0; i < width; i+=_pixelsForGrid+_pixelsForShip) {
            for (int j = 0; j < width; j+=_pixelsForGrid+_pixelsForShip) {
                for (int k = 0; k < _pixelsForGrid; ++k) {
                    texture.SetPixel(i + k, j + k, Color.black);
                }
            }
        }

        texture.Apply();

        _spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, width, width), Vector2.zero);
    }
}