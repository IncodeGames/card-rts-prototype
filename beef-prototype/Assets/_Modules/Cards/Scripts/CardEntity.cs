using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Incode.Prototype
{
    public class CardEntity : MonoBehaviour
    {
        [SerializeField] private uint cardID = 0;
        public uint CardID { get { return cardID; } }

        private RectTransform rectTransform = null;
        public RectTransform RectTransform { get { return rectTransform; } }

        private CardUI cardUI = null;
        public CardUI CardUI { get { return cardUI; } }

        private ICardActionable cardAction = null;
        public ICardActionable CardAction { get { return cardAction; } }

        private DataMaps dataMaps = null;
        public DataMaps DataMaps { get { return dataMaps; } }

        void Awake()
        {
            rectTransform = this.GetComponent<RectTransform>();
            cardUI = this.GetComponent<CardUI>();
            cardAction = this.GetComponent<ICardActionable>();
            Debug.Assert(cardAction != null, "Cards must have an associated card action component.");

            ReferenceManager.Instance.TryGetReference<DataMaps>(out dataMaps);

            cardUI.UpdateCardMetadata(dataMaps.cardData.CardLookup[cardID]);

            cardAction.Init();
        }
    }
}
