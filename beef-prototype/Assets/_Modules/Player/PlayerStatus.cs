using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Incode.Prototype
{
    public class PlayerStatus : MonoBehaviour
    {
        [ReadOnly] public int currentEnergy = 0;
        [ReadOnly] public List<CardEntity> drawPile;
        [ReadOnly] public List<CardEntity> discardPile;
        [ReadOnly] public List<CardEntity> currentHand;
        public UnitEntity summonedHero = null;

        void Update()
        {
            if (summonedHero != null && summonedHero.UnitStatus.IsDead)
            {
                summonedHero = null;
            }
        }
    }
}
