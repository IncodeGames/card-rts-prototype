using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    public class UnitStatus
    {

        private int health = 0;
        public int Health { get { return health; } }
        private int maxHealth = 0;

        private float moveSpeed = 0f;
        public float MoveSpeed { get { return moveSpeed; } }

        private UnitEntity unitEntity;
        public UnitStatus(UnitEntity _entity, int _health, float _moveSpeed)
        {
            unitEntity = _entity;
            health = _health;
            maxHealth = health;

            moveSpeed = _moveSpeed;
        }

        public void Damage(int damage)
        {
            Debug.Log("Enemy hit!");
            Debug.Assert(damage >= 0);
            health -= damage;
            if (health <= 0)
            {
                KillUnit();
            }
        }

        public void Heal(int heal)
        {
            Debug.Assert(heal >= 0);
            health = Mathf.Clamp(health + heal, 0, maxHealth);
        }

        private void KillUnit()
        {
            Debug.Log("Enemy Killed");
            unitEntity.gameObject.SetActive(false);
        }
    }
}
