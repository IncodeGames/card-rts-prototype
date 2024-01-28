using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Incode.Prototype
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Data/BuildingData", order = 1)]
    public class BuildingData : SerializedScriptableObject
    {
        public Dictionary<uint, BuildingEntity> buildingLookup;
    }
}
