using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    public class UnitVFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem hitEffect;
        [SerializeField] private ParticleSystem attackEffect;

        public void PlayAttackEffect()
        {
            attackEffect.Play();
        }

        public void PlayHitEffects()
        {
            hitEffect.Play();
        }
    }
}
