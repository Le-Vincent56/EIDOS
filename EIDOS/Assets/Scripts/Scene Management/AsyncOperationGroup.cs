using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EIDOS.Scene_Management
{
    public readonly struct AsyncOperationGroup
    {
        public readonly List<AsyncOperation> Operations;

        /// <summary>
        /// Get the progress of the async operations within the Async Operation Group
        /// </summary>
        public float Progress => Operations.Count == 0 ? 0 : Operations.Average(o => o.progress);

        /// <summary>
        /// Get if the Async Operation Group has finished all of its operations
        /// </summary>
        public bool IsDone => Operations.All(o => o.isDone);

        public AsyncOperationGroup(int initialCapacity)
        {
            Operations = new List<AsyncOperation>(initialCapacity);
        }
    }
}