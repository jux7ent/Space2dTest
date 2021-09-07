using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

public class MainThreadDispatcher : MonoBehaviour {
    private static MainThreadDispatcher _instance = null;
    private static readonly ConcurrentQueue<Action> _executionQueue = new ConcurrentQueue<Action>();
    
    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnDestroy() {
        _instance = null;
    }

    private void Update() {
        lock (_executionQueue) {
            while (_executionQueue.Count > 0) {
                if (_executionQueue.TryDequeue(out Action action)) {
                    action.Invoke();
                }
            }
        }
    }

    public static void Enqueue(Action action) {
        if (_instance == null) {
            Debug.Log("Error enqueue to main thread dispatcher. _instance is null");
        } else {
            lock (_executionQueue) {
                #if UNITY_EDITOR
                if (_executionQueue.Count > 100000) {
                    Debug.Log("A lot of actions for queue");
                }
                #endif
                _executionQueue.Enqueue(action);
            }
        }
    }
}