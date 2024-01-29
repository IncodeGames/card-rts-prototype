using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Incode.Prototype
{
    public class UnitEntity : MonoBehaviour
    {
        [SerializeField] private uint unitID = 0;
        public uint UnitID { get { return unitID; } }

        [SerializeField] private uint teamID = 0; //NOTE(BEN): This would be set during match initializaiton in a multiplayer scenario 2024-01-27
        public uint TeamID { get { return teamID; } }

        private ITickable tickable = null;
        public ITickable Tickable { get { return tickable; } }

        private AIPath pathAgent = null;
        public AIPath PathAgent { get { return pathAgent; } }

        private UnitVFX unitVFX = null;
        public UnitVFX UnitVFX { get { return unitVFX; } }

        private UnitStatus unitStatus = null;
        public UnitStatus UnitStatus { get { return unitStatus; } }

        private DataMaps dataMaps = null;
        public DataMaps DataMaps { get { return dataMaps; } }

        void Awake()
        {
            pathAgent = this.GetComponent<AIPath>();
            tickable = this.GetComponent<ITickable>();
            unitVFX = this.GetComponent<UnitVFX>();

            ReferenceManager.Instance.TryGetReference<DataMaps>(out dataMaps);
            UnitData.UnitStatistics stats = dataMaps.unitData.unitStatsLookup[unitID];
            unitStatus = new UnitStatus(this, stats.health, stats.moveSpeed);
        }

        void OnEnable()
        {
            GameManager.Instance.simulationTickables.Add(tickable);
        }

        void OnDisable()
        {
            GameManager.Instance.simulationTickables.Remove(tickable);
        }
    }
}
