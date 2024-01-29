using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    [RequireComponent(typeof(CardEntity))]
    public class HealHeroAction : MonoBehaviour, ICardActionable
    {
        private int energyCost = 0;
        int ICardActionable.EnergyCost { get { return energyCost; } }

        [SerializeField] private int healAmount = 50;

        private CardEntity cardEntity = null;

        private PlayerStatus playerStatus;

        void ICardActionable.Init()
        {
            cardEntity = this.GetComponent<CardEntity>();

            ReferenceManager.Instance.TryGetReference<PlayerStatus>(out playerStatus);
        }

        private bool HealHero()
        {
            if (playerStatus.summonedHero != null)
            {
                playerStatus.summonedHero.UnitStatus.Heal(healAmount);
                return true;
            }

            return false;
        }

        public bool EnqueueAction(Vector3 worldPoint)
        {
            if (HealHero())
            {
                return true;
            }

            return false;
        }
    }
}
