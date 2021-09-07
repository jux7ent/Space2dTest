using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;

public class SpawnShipSystem : GameSystem, IIniting {
    [SerializeField] private GameObject _shipPrefab;
    [SerializeField] private Vector2Int _startShipPos = Vector2Int.zero;

    void IIniting.OnInit() {
        SpawnShip();
    }

    private void SpawnShip() {
        game.SetShip(Instantiate(_shipPrefab, Vector2.zero, Quaternion.identity).GetComponent<Ship>(), _startShipPos);
    }
}