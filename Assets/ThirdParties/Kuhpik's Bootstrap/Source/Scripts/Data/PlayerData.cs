using System;
using System.Collections.Generic;
using System.Linq;
using Kuhpik;
using NaughtyAttributes;
using Supyrb;
using UnityEngine;

namespace Kuhpik {
    /// <summary>
    /// Used to store player's data. Change it the way you want.
    /// </summary>
    [System.Serializable]
    public class PlayerData {
        public int levelIndex;
        public bool soundOn;
        public bool hapticOn;
        public bool tutorialPassed;
        public int UserId = -1;
        public string LastConnectedGameServer;
    }
}