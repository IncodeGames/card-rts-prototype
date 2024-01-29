using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    [RequireComponent(typeof(CardEntity))]
    public class DeployBarracksAction : MonoBehaviour, ICardActionable
    {
        private int energyCost = 0;
        int ICardActionable.EnergyCost { get { return energyCost; } }

        private CardEntity cardEntity = null;
        private CardData cardData = null;
        private BuildingData buildingData = null;

        private GraphController graphController;

        public void Init()
        {
            cardEntity = this.GetComponent<CardEntity>();
            cardData = cardEntity.DataMaps.cardData;
            buildingData = cardEntity.DataMaps.buildingData;

            ReferenceManager.Instance.TryGetReference<GraphController>(out graphController);

            energyCost = cardData.CardLookup[cardEntity.CardID].energyCost;
        }

        private bool DeployBarracks(Vector3 worldPoint)
        {
            BuildingEntity barracks = buildingData.buildingLookup[IDConsts.BARRACKS_ID];

            Vector3 placementPosition = worldPoint;
            placementPosition.y = barracks.transform.localScale.y * 0.5f;
            Collider[] overlaps = Physics.OverlapBox(placementPosition, barracks.transform.localScale * 0.5f, Quaternion.identity, 1 << LayerMask.NameToLayer("Building"));
            if (overlaps.Length == 0)
            {
                GameObject deployedBarracks = Instantiate(barracks.gameObject, placementPosition, Quaternion.identity);
                graphController.UpdateGraph(deployedBarracks.GetComponent<Collider>());
                return true;
            }
            else
            {
                Debug.Log("Cannot place barracks, invalid location.");
            }

            return false;
        }

        public bool EnqueueAction(Vector3 worldPoint)
        {
            if (DeployBarracks(worldPoint))
            {
                return true;
            }

            return false;
        }
    }
}
