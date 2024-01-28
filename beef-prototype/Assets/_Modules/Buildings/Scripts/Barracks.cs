using System.Collections;
using System.Collections.Generic;
using Incode.Prototype;
using UnityEngine;

[RequireComponent(typeof(BuildingEntity))]
public class Barracks : MonoBehaviour, ITickable
{
    [SerializeField] private float marineSpawnDuration = 0.0f;

    private float lastSpawnTime = 0.0f;

    private BuildingEntity buildingEntity = null;
    private UnitData unitData = null;
    private GameManager gameManager = null;

    void Awake()
    {
        buildingEntity = this.GetComponent<BuildingEntity>();
        unitData = buildingEntity.DataMaps.unitData;
        gameManager = GameManager.Instance;
    }

    void OnEnable()
    {
        lastSpawnTime = gameManager.TickTime;
    }

    public void Tick()
    {
        if (gameManager.TickTime - lastSpawnTime >= marineSpawnDuration)
        {
            UnitEntity marine = unitData.unitLookup[IDConsts.MARINE_ID];
            Vector3 spawnPosition = (transform.position + transform.right);
            spawnPosition.y = 0f;
            Instantiate(marine.gameObject, spawnPosition, Quaternion.identity);

            lastSpawnTime = gameManager.TickTime;
        }
    }
}
