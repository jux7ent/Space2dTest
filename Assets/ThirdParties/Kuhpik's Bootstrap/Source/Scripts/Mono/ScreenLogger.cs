using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenLogger : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textField;
    
    void OnEnable() {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable() {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type) {
        if (type == LogType.Error || type == LogType.Exception) {
            textField.text += $"Error: \n{logString}\n{stackTrace}\n";
        } else if (type == LogType.Log) {
            textField.text += $"{logString}\n";
        }
    }
}
