using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    public class BuildingEntity : MonoBehaviour
    {
        [SerializeField] private uint buildingID = 0;
        public uint BuildingID { get { return buildingID; } }

        private ITickable tickable = null;
        public ITickable Tickable { get { return tickable; } }

        private DataMaps dataMaps = null;
        public DataMaps DataMaps { get { return dataMaps; } }

        void Awake()
        {
            tickable = this.GetComponent<ITickable>();
            Debug.Assert(tickable != null, "Buildings must have a tickable component.");

            ReferenceManager.Instance.TryGetReference<DataMaps>(out dataMaps);
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
