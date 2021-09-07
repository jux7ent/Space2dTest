using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class ChangeGameStateSystem : GameSystem, IIniting {
    [SerializeField] private EGamestate gameState;
    
    void IIniting.OnInit() {
        Bootstrap.ChangeGameState(gameState);    
    }
    
}