using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    public class EnemySpawner : MonoBehaviour, ITickable
    {
        [SerializeField] private GameObject enemyMarine = null;

        [Space]
        [SerializeField] private float spawnRate = 0.0f;
        [SerializeField] private float randomSpreadDiameter = 0.2f;
        [SerializeField] private Transform[] spawnPoints = null;


        private float lastSpawnTime = 0.0f;

        private GameManager gameManager = null;

        void Awake()
        {
            gameManager = GameManager.Instance;
        }

        void OnEnable()
        {
            GameManager.Instance.simulationTickables.Add(this);
        }

        void OnDisable()
        {
            GameManager.Instance.simulationTickables.Remove(this);
        }

        public void Tick()
        {
            if (gameManager.TickTime - lastSpawnTime >= spawnRate)
            {
                int enemyCount = Random.Range(1, 4); //Spawn 1-6 enemies
                for (int i = 0; i < enemyCount; ++i)
                {
                    Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    Vector3 randomSpread = Random.onUnitSphere * randomSpreadDiameter;
                    randomSpread.y = 0;
                    Instantiate(enemyMarine, spawnPosition + randomSpread, Quaternion.identity);

                    lastSpawnTime = gameManager.TickTime;
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            foreach (Transform t in spawnPoints)
            {
                Gizmos.DrawWireSphere(t.position, randomSpreadDiameter);
            }
            Gizmos.color = Color.white;
        }
    }
}
