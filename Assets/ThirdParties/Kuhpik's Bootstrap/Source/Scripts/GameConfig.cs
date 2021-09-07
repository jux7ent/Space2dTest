using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.AI;

namespace Kuhpik {
    [CreateAssetMenu(menuName = "Kuhpik/GameConfig")]
    public sealed class GameConfig : ScriptableObject {
        [field: SerializeField] public int GameVersion { get; private set; }
        
        [field: SerializeField] public int MinZoom { get; private set; }
        [field: SerializeField] public int MaxZoom { get; private set; }
        
        [field: SerializeField] public float PlanetsPercentage { get; private set; }
    }
}