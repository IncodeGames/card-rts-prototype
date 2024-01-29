using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    public class GlaiveHero : MonoBehaviour, ITickable
    {
        [SerializeField] private int attackDamage = 0;
        [SerializeField] private float attackRate = 0f;
        [SerializeField] private float sensorRange = 0f;
        [SerializeField] private float attackRange = 0f;
        [SerializeField] private float glaiveRange = 0f;
        [SerializeField] private float glaiveSpeed = 0f;

        [Space]
        [SerializeField] private List<GameObject> inactiveProjectiles;
        private List<GameObject> activeProjectiles = new List<GameObject>();

        private float lastAttackTime = 0.0f;

        private UnitEntity unitEntity = null;
        private GameManager gameManager = null;

        private UnitEntity entityTarget = null;
        private UnitEntity[] neighborTargets = new UnitEntity[2];

        void Awake()
        {
            unitEntity = this.GetComponent<UnitEntity>();
            gameManager = GameManager.Instance;
        }

        void OnEnable()
        {
            lastAttackTime = gameManager.TickTime; //Prevent attacking immediately after spawning
        }

        void Start()
        {
            unitEntity.PathAgent.destination = transform.position;
            unitEntity.PathAgent.maxSpeed = unitEntity.UnitStatus.MoveSpeed;
        }

        private IEnumerator ProjectileRoutine(int targetCount)
        {
            Debug.Assert(targetCount <= 3);
            Debug.Assert(inactiveProjectiles.Count > 0);

            GameObject firedProjectile = inactiveProjectiles[0];
            activeProjectiles.Add(firedProjectile);
            inactiveProjectiles.RemoveAt(0);

            firedProjectile.SetActive(true);

            Vector3 startPosition = transform.position;
            for (int i = 0; i < targetCount; ++i)
            {
                UnitEntity glaiveTarget = null;
                if (i == 0)
                {
                    glaiveTarget = entityTarget;
                }
                else
                {
                    glaiveTarget = neighborTargets[i - 1];
                }

                float lerpValue = 0.0f;
                float startTime = GameManager.Instance.TickTime;
                float duration = glaiveSpeed;
                while (lerpValue < 1.0f)
                {
                    if (glaiveTarget == null || glaiveTarget.UnitStatus.IsDead) { break; }

                    firedProjectile.transform.position = Vector3.Lerp(startPosition, glaiveTarget.transform.position, lerpValue);
                    lerpValue = (GameManager.Instance.TickTime - startTime) / duration;
                    yield return null;
                }
                if (glaiveTarget == null || glaiveTarget.UnitStatus.IsDead) { continue; }
                startPosition = glaiveTarget.transform.position;

                glaiveTarget.UnitStatus.Damage(attackDamage);
                if (glaiveTarget == entityTarget)
                {
                    if (glaiveTarget.UnitStatus.IsDead) { entityTarget = null; }
                }
                else
                {
                    neighborTargets[i - 1].UnitStatus.Damage(attackDamage);
                    if (neighborTargets[i - 1].UnitStatus.IsDead) { neighborTargets[i - 1] = null; }
                }
            }

            firedProjectile.SetActive(false);
            firedProjectile.transform.position = transform.position;

            activeProjectiles.Remove(firedProjectile);
            inactiveProjectiles.Add(firedProjectile);
        }

        private void GlaiveAttack()
        {
            Collider[] colliders = Physics.OverlapSphere(entityTarget.transform.position, glaiveRange, 1 << LayerMask.NameToLayer("Unit"));
            if (colliders.Length > 0)
            {
                float glaiveDistance = 1000.0f;
                float minGlaiveDistance = glaiveDistance;
                for (int i = 0; i < colliders.Length; ++i)
                {
                    UnitEntity nearbyEnemy = colliders[i].transform.GetComponent<UnitEntity>();
                    if (nearbyEnemy == entityTarget) { continue; }
                    glaiveDistance = Vector3.Distance(entityTarget.transform.position, colliders[i].transform.position);
                    if (glaiveDistance < minGlaiveDistance && nearbyEnemy.TeamID != this.unitEntity.TeamID)
                    {
                        neighborTargets[1] = neighborTargets[0];
                        neighborTargets[0] = nearbyEnemy;

                        minGlaiveDistance = glaiveDistance;
                    }
                }
            }

            int targetCount = 1;
            if (neighborTargets[0] != null) { ++targetCount; }
            if (neighborTargets[1] != null) { ++targetCount; }

            StartCoroutine(ProjectileRoutine(targetCount));

            //TODO(BEN): Move glaive along path points
            //Deal damage as they get to each point

            //Clear out entity and neighbortargets if they die
            //if (entityTarget.UnitStatus.Health <= 0) { entityTarget = null; }
        }

        public void Tick()
        {
            if (GameManager.Instance.CurrentGameState != GameManager.GameState.BATTLE)
            {
                unitEntity.PathAgent.destination = transform.position;
                unitEntity.PathAgent.canMove = false;
            }

            if (GameManager.Instance.CurrentGameState == GameManager.GameState.BATTLE)
            {

                //--- Find nearest target ---
                if (entityTarget == null)
                {
                    //TODO(BEN): Avoid alloc 2024-01-27
                    Collider[] colliders = Physics.OverlapSphere(transform.position, sensorRange, 1 << LayerMask.NameToLayer("Unit"));
                    if (colliders.Length > 0)
                    {
                        float targetDistance = 1000.0f;
                        float currentMinDistance = targetDistance;
                        for (int i = 0; i < colliders.Length; ++i)
                        {
                            UnitEntity otherEntity = colliders[i].transform.GetComponent<UnitEntity>();
                            if (otherEntity == this.unitEntity) { continue; }

                            targetDistance = Vector3.Distance(otherEntity.transform.position, transform.position);
                            if (targetDistance < currentMinDistance && otherEntity.TeamID != this.unitEntity.TeamID)
                            {
                                entityTarget = otherEntity;
                                currentMinDistance = targetDistance;
                            }
                        }
                    }
                }

                if (entityTarget != null)
                {
                    float targetDistance = Vector3.Distance(transform.position, entityTarget.transform.position);
                    if (targetDistance >= attackRange)
                    {
                        unitEntity.PathAgent.destination = entityTarget.transform.position;
                        unitEntity.PathAgent.canMove = true;
                    }
                    else if (unitEntity.PathAgent.canMove == true)
                    {
                        unitEntity.PathAgent.canMove = false;
                    }

                    if (gameManager.TickTime - lastAttackTime >= attackRate && targetDistance < attackRange)
                    {
                        unitEntity.UnitVFX.PlayAttackEffect();

                        GlaiveAttack();

                        lastAttackTime = gameManager.TickTime;
                    }
                }
            }
        }
    }
}
