using System.Collections.Generic;
using System.Linq;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace EIDOS.Scene_Management
{
    public readonly struct AsyncOperationHandleGroup
    {
        public readonly List<AsyncOperationHandle<SceneInstance>> Handles;

        /// <summary>
        /// Get the progress of the async operation handles within the Async Operation Handle Group
        /// </summary>
        public float Progress => Handles.Count == 0 ? 0 : Handles.Average(h => h.PercentComplete);

        /// <summary>
        /// Get if the Async Operation Handle Group has finished all of its operations
        /// </summary>
        public bool IsDone => Handles.Count == 0 || Handles.All(o => o.IsDone);

        public AsyncOperationHandleGroup(int initialCapacity)
        {
            Handles = new List<AsyncOperationHandle<SceneInstance>>(initialCapacity);
        }
    }
}