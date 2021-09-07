using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDirection {
    Up,
    Down,
    Right,
    Left
}

public class Ship : MonoBehaviour {
    public void SetShipPosition(int x, int y) {
        transform.position = new Vector3(x, y, 0f);
    }

    public void MoveShipOnUnit(EDirection direction) {
        switch (direction) {
            case EDirection.Up: {
                transform.position += Vector3.up;
                break;
            }
            case EDirection.Down: {
                transform.position += Vector3.down;
                break;
            }
            case EDirection.Left: {
                transform.position += Vector3.left;
                break;
            }
            case EDirection.Right: {
                transform.position += Vector3.right;
                break;
            }
        }
    }
}