using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    [RequireComponent(typeof(UnitEntity))]
    public class Marine : MonoBehaviour, ITickable
    {
        [SerializeField] private int attackDamage = 0;
        [SerializeField] private float attackRate = 0f;
        [SerializeField] private float sensorRange = 0f;
        [SerializeField] private float attackRange = 0f;

        private float lastAttackTime = 0.0f;

        private UnitEntity unitEntity = null;
        private GameManager gameManager = null;

        private UnitEntity entityTarget = null;


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

                        entityTarget.UnitStatus.Damage(attackDamage);
                        if (entityTarget.UnitStatus.Health <= 0) { entityTarget = null; }

                        lastAttackTime = gameManager.TickTime;
                    }
                }
            }
        }
    }
}