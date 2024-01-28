using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Incode.Prototype
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Data/CardData", order = 1)]
    public class CardData : SerializedScriptableObject
    {
        [System.Serializable]
        public struct CardMetadata
        {
            public int energyCost;
            public string name;
            [TextArea] public string description;
        }

        public Dictionary<uint, CardMetadata> CardLookup;
    }
}
