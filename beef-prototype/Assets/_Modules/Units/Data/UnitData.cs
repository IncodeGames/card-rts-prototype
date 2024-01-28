using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Incode.Prototype
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Data/UnitData", order = 1)]
    public class UnitData : SerializedScriptableObject
    {
        public struct UnitStatistics
        {
            public int health;
            public float moveSpeed;
        }

        public Dictionary<uint, UnitEntity> unitLookup;

        [Space]
        public Dictionary<uint, UnitStatistics> unitStatsLookup;
    }
}
