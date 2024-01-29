using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    public class OrbitalStrike : MonoBehaviour, ITickable
    {
        private float orbitalStrikeRadius = 0.0f;
        [SerializeField] private int orbitalStrikeDamage = 0;
        [SerializeField] private ParticleSystem orbitalStrikeEffect;

        private GameObject indicator = null;

        private bool strikeActive = true;

        void OnEnable()
        {
            GameManager.Instance.simulationTickables.Add(this);
        }

        void OnDisable()
        {
            GameManager.Instance.simulationTickables.Remove(this);
        }

        public void SetIndicatorAndRadius(GameObject _indicator, float _radius)
        {
            indicator = _indicator;
            orbitalStrikeRadius = _radius;
        }

        public void Tick()
        {

            if (GameManager.Instance.CurrentGameState == GameManager.GameState.BATTLE)
            {
                if (indicator != null)
                {
                    Destroy(indicator);
                    indicator = null;
                }

                if (strikeActive == true)
                {
                    Collider[] overlaps = Physics.OverlapSphere(transform.position, orbitalStrikeRadius, 1 << LayerMask.NameToLayer("Unit"));
                    if (overlaps.Length > 0)
                    {
                        for (int i = 0; i < overlaps.Length; ++i)
                        {
                            UnitEntity entity = overlaps[i].transform.GetComponent<UnitEntity>();
                            entity.UnitStatus.Damage(orbitalStrikeDamage);
                        }
                    }

                    orbitalStrikeEffect.Play();
                    strikeActive = false;
                }
            }
        }
    }
}