using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Incode.Prototype
{
    public class GraphController : MonoBehaviour
    {
        [SerializeField] private AstarPath pathfinder;

        public void UpdateGraph(Collider col)
        {
            pathfinder.UpdateGraphs(col.bounds);
        }
    }
}
