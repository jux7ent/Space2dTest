using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using Supyrb;
using UnityEngine;

public class NewShipPositionSignal : Signal<Vector2> { }

public class ShipControllerSystem : GameSystem, IIniting, IUpdating {
    void IIniting.OnInit() {
        StartCoroutine(GameExtensions.Coroutines.WaitOneFrame(() =>
            Signals.Get<NewShipPositionSignal>().Dispatch(game.Ship.transform.position))); // =(
    }

    void IUpdating.OnUpdate() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            MoveShip(EDirection.Down);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            MoveShip(EDirection.Up);
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            MoveShip(EDirection.Left);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            MoveShip(EDirection.Right);
        }
    }

    private void MoveShip(EDirection direction) {
        game.Ship.MoveShipOnUnit(direction);
        Signals.Get<NewShipPositionSignal>().Dispatch(game.Ship.transform.position);
    }
}