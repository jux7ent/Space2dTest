using Kuhpik.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kuhpik
{
    [DefaultExecutionOrder(500)]
    public class Bootstrap : MonoBehaviour {
        private Thread _mainThread;
        private const string saveKey = "saveKey";

        [field: SerializeField] public GameConfig config { get; private set; }

        public static PlayerData playerData;
        private static FSMProcessor<GameState> fsm;
        private static Dictionary<Type, GameSystem> systems;
        private static EGamestate currGameState;

        public static Bootstrap instance;

        private static Queue<string> messagesQueue = new Queue<string>();

        public void AddMessage(string message) {
            messagesQueue.Enqueue(message);
        }

        public static void InvokeInMainThread(Action action) {
            MainThreadDispatcher.Enqueue(action);
        }

        private void Awake() {
            instance = this;
            _mainThread = Thread.CurrentThread;
        }
        
        private void Start()
        {
            InitSystems();
        }

        private void Update()
        {
            if (fsm.State.IsInited)
            {
                for (int i = 0; i < fsm.State.UpdateSystems.Length; i++)
                {
                    fsm.State.UpdateSystems[i].OnUpdate();
                }
            }
        }

        private void FixedUpdate()
        {
            while (messagesQueue.Count > 0) {
                Debug.Log(messagesQueue.Dequeue());
            }
            
            if (fsm.State.IsInited)
            {
                for (int i = 0; i < fsm.State.FixedUpdateSystems.Length; i++)
                {
                    fsm.State.FixedUpdateSystems[i].OnFixedUpdate();
                }
            }
        }

        public static void GameRestart(int sceneIndex)
        {
            foreach (var system in systems.Values)
            {
                (system as IGameSystem).PerformAction<IDisposing>();
            }

            SaveExtension.Save(playerData, saveKey);
            SceneManager.LoadScene(sceneIndex);
            PoolingSystem.Clear();
        }

        public static void SavePlayerData() {
            SaveExtension.Save(playerData, saveKey);
        }

        public static void ChangeGameState(EGamestate type)
        {
            fsm.State.Deactivate();
            fsm.ChangeState(type.GetName());
            fsm.State.Activate();

            _SetGameState(type);
        }

        public static EGamestate GetCurrentGameState() {
            return currGameState;
        }

        internal static void _SetGameState(EGamestate state) {
            currGameState = state;
        }

        public static T GetSystem<T>() where T : class
        {
            return systems[typeof(T)] as T;
        }

        private void InitSystems()
        {
            CreatePools();
            ResolveSystems();
            LoadPlayerData();
            HandleGameStates();
            HandleInjections();
            HandleCamerasFOV();

            fsm.State.Activate();
        }

        private void ResolveSystems()
        {
            systems = FindObjectsOfType<GameSystem>().ToDictionary(system => system.GetType(), system => system);
        }

        private void HandleGameStates()
        {
            fsm = GetComponentInChildren<GameStateInstaller>().InstallGameStates();
        }

        private void LoadPlayerData()
        {
            playerData = GetComponentInChildren<PlayerDataInstaller>().InstallData(saveKey);
        }

        private void HandleInjections()
        {
            GetComponentInChildren<InjectionsInstaller>().Inject(systems.Values, config, playerData, new GameData());
        }

        private void HandleCamerasFOV()
        {
            GetComponentInChildren<CameraInstaller>().Process();
        }

        public static void ForceCamerasFOV() {
            instance.GetComponentInChildren<CameraInstaller>().Process();
        }

        public static bool IsMainThread() {
            return Thread.CurrentThread.Equals(instance._mainThread);
        }

        private void CreatePools()
        {
            GetComponentInChildren<PoolInstaller>().Init();
        }
    }
}