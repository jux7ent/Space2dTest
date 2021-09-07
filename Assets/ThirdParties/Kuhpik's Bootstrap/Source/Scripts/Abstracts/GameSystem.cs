using System;
using UnityEngine;

namespace Kuhpik
{
    public abstract class GameSystem : MonoBehaviour
    {
        protected PlayerData player;
        protected GameConfig config;
        protected GameData game;
    }

    public abstract class ExpectedGameSystem : GameSystem {
        public Action readyAction;
        
        protected void Ready() {
            readyAction?.Invoke();    
        }
    }

    public abstract class WaitingGameSystem : GameSystem {
        public ExpectedGameSystem waitingFor;

        private void Awake() {
            waitingFor.readyAction += OnPrevGameSystemReady;
        }

        protected abstract void OnPrevGameSystemReady();
    }
    
    [Serializable]
    public abstract class ExpectedWaitingGameSystem : ExpectedGameSystem {
        [SerializeField] public ExpectedGameSystem waitingFor;

        protected void Awake() {
            waitingFor.readyAction += OnPrevGameSystemReady;
        }

        protected abstract void OnPrevGameSystemReady();
    }
}