using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    [RequireComponent(typeof(CardEntity))]
    public class SummonGlaiveHeroAction : MonoBehaviour, ICardActionable
    {
        private int energyCost = 0;
        int ICardActionable.EnergyCost { get { return energyCost; } }

        private CardEntity cardEntity = null;
        private CardData cardData = null;
        private UnitData unitData = null;

        private PlayerStatus playerStatus;
        private GraphController graphController;

        public void Init()
        {
            cardEntity = this.GetComponent<CardEntity>();
            cardData = cardEntity.DataMaps.cardData;
            unitData = cardEntity.DataMaps.unitData;

            ReferenceManager.Instance.TryGetReference<PlayerStatus>(out playerStatus);
            ReferenceManager.Instance.TryGetReference<GraphController>(out graphController);

            energyCost = cardData.CardLookup[cardEntity.CardID].energyCost;
        }

        private bool SummonGlaiveHero(Vector3 worldPoint)
        {
            if (playerStatus.summonedHero != null) { return false; }
            UnitEntity glaiveHero = unitData.unitLookup[IDConsts.GLAIVE_HERO_ID];

            Vector3 placementPosition = worldPoint;
            placementPosition.y = 0;
            Collider[] overlaps = Physics.OverlapBox(placementPosition, glaiveHero.transform.localScale * 0.5f, Quaternion.identity, 1 << LayerMask.NameToLayer("Building"));
            if (overlaps.Length == 0)
            {
                UnitEntity heroInstance = Instantiate(glaiveHero.gameObject, placementPosition, Quaternion.identity).GetComponent<UnitEntity>();
                playerStatus.summonedHero = heroInstance;
                return true;
            }
            else
            {
                Debug.Log("Cannot place glaiveHero, invalid location.");
            }

            return false;
        }

        public bool EnqueueAction(Vector3 worldPoint)
        {
            if (SummonGlaiveHero(worldPoint))
            {
                return true;
            }

            return false;
        }
    }
}
