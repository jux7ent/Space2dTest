using UnityEngine;

namespace Kuhpik
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
        }
    }
}