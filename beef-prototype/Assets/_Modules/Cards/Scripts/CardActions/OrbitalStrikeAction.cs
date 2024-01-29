using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    [RequireComponent(typeof(CardEntity))]
    public class OrbitalStrikeAction : MonoBehaviour, ICardActionable
    {
        private int energyCost = 0;
        int ICardActionable.EnergyCost { get { return energyCost; } }

        [SerializeField] private float orbitalStrikeRadius = 1.0f;

        [Space]
        [SerializeField] private GameObject orbitalStrikeIndicator;
        [SerializeField] private GameObject orbitalStrike;

        private CardEntity cardEntity = null;
        public void Init()
        {

        }

        private bool OrbitalStrike(Vector3 worldPoint)
        {
            Vector3 placementPosition = worldPoint;
            placementPosition.y = 0;
            Collider[] overlaps = Physics.OverlapSphere(placementPosition, orbitalStrikeRadius, 1 << LayerMask.NameToLayer("Unit"));
            if (overlaps.Length > 0)
            {
                GameObject strikeIndicator = Instantiate(orbitalStrikeIndicator, placementPosition, Quaternion.identity);
                OrbitalStrike strikeInstance = Instantiate(orbitalStrike, placementPosition, Quaternion.identity).GetComponent<OrbitalStrike>();
                strikeInstance.SetIndicatorAndRadius(strikeIndicator, orbitalStrikeRadius);
                return true;
            }
            else
            {
                Debug.Log("Cannot place orbital strike, no units nearby.");
            }
            return false;
        }

        public bool EnqueueAction(Vector3 worldPoint)
        {
            if (OrbitalStrike(worldPoint))
            {
                return true;
            }
            return false;
        }

    }
}