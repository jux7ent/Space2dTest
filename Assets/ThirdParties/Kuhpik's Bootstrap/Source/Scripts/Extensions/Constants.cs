using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik {
    public static class Constants {
        public static class Math {
            public const float Sqrt2 = 1.41421356237f;
        }
          
        public static class Tags {
            public const string CampFire = "CampFire";
            public const string WinZone = "WinZone";
        }

        public static class Layers {
            public const int Bead = 6;
            public const int Form = 7;
            public const int LiftedBead = 9;
            public const int Platform = 10;
        }

        public static class AdvertisementReason {
            public const string SkinFromShop = "SKIN_FROM_SHOP";
            public const string SkinFromLevel = "SKIN_FROM_LEVEL";
            public const string SilverChestReward = "SILVER_CHEST_REWARD";
            public const string EpicChestReward = "EPIC_CHEST_REWARD";
            public const string GetFreeCoinsPlace = "GET_FREE_COINS_PLACE";
            public const string GetX5Coins = "GET_X5_COINS";
            public const string RemoveObstacle = "REMOVE_OBSTACLE";
            public const string Get450Coins = "GET_450_COINS";
            public const string TripleMissionReward = "TRIPLE_MISSION_REWARD";
        }

        public static class AnimatorTags {
            public const string Idle = "Idle";
            public const string Walk = "Walk";
            public const string Attack = "Attack";
            public const string Interaction = "Interaction";
            public const string PrisonDoorOpen = "PrisonDoorOpen";
            public const string TakeDamage = "TakeDamage";
            public const string Stunned = "Stunned";
            public const string ObstacleClosed = "ObstacleClosed";
        }
    }
}