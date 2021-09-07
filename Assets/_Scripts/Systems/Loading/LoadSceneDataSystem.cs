using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class LoadSceneDataSystem : GameSystem, IIniting {
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _uiCamera;

    private bool _systemInitiated = false;
    
    void IIniting.OnInit() {
        if (!_systemInitiated) {
            game.LoadSceneData(_mainCamera, _uiCamera);
        }
    }
}