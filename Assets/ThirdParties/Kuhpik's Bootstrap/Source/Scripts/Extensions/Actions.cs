using System;
using UnityEngine;

public class Actions {
    public static class LevelActions {
        public static Action<int> LevelStart; // levelIndex + 1
        public static Action<int, bool, int> LevelFinish; // levelIndex + 1, isRestart, Progress(0, 100)
    }

    public static class Advertisement {
        public static Action<Action> ShowInterstitial;
        public static Action<string, Action, Action> ShowRewardedVideo;
    }

    public static class Settings {
        public static Action<bool> soundStateChanged; // curr sound state
        public static Action<bool> hapticStateChanged; // curr sound state
    }
}