using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Profiling.Memory.Experimental;

namespace Incode.Prototype
{
    public class CardUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI energyCostText = null;
        [SerializeField] private TextMeshProUGUI nameText = null;
        [SerializeField] private TextMeshProUGUI descriptionText = null;

        public void UpdateCardMetadata(CardData.CardMetadata cardMetadata)
        {
            energyCostText.text = "Cost " + cardMetadata.energyCost.ToString();
            nameText.text = cardMetadata.name;
            descriptionText.text = cardMetadata.description;
        }
    }
}