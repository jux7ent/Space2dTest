using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(CanvasGroup))]
public class ButtonWithCanvasGroup : MonoBehaviour {
    public Button Button {
        get {
            if (button == null) {
                button = GetComponent<Button>();
            }

            return button;
        }
    }

    public CanvasGroup CanvasGroup { 
        get {
            if (canvasGroup == null) {
                canvasGroup = GetComponent<CanvasGroup>();
            }

            return canvasGroup;
        } 
    }

    private Button button;
    private CanvasGroup canvasGroup;

    private void Awake() {
        button = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
}