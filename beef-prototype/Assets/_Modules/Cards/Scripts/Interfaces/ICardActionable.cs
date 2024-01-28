using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incode.Prototype
{
    public interface ICardActionable
    {
        int EnergyCost { get; }

        /// <summary>
        /// Call once all necessary data is populated/cached.
        /// </summary>
        void Init();

        /// <summary>
        /// Attempts to perform a card's associated action. Returns false if the player is unable to perform the action.
        /// </summary>
        bool EnqueueAction(Vector3 worldPoint);
    }
}
