using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace EIDOS.Scene_Management
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Scenes")]
        [SerializeField] private SceneGroupData sceneGroupData;
        private bool isLoading;
        private SceneGroupManager manager;

        [Header("Fields")] 
        [SerializeField] private bool debug;

        public SceneGroup[] SceneGroups => sceneGroupData.sceneGroups;

        private void Awake()
        {
            // Create the scene group manager
            manager = new SceneGroupManager();
        }

        private async void Start()
        {
            try
            {
                await LoadSceneGroup(sceneGroupData.initialScene);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SceneLoader] Could not successfully load the scene group {sceneGroupData.name}"
                    + $"\nException: {e.Message}");
            }
        }

        /// <summary>
        /// Load a Scene Group
        /// </summary>
        private async UniTask LoadSceneGroup(int index)
        {
            // Exit case - the index is not valid
            if (index < 0 || index >= sceneGroupData.sceneGroups.Length) return;

            LoadingProgress progress = new LoadingProgress();

            // TODO: Fire off loading functions

            await manager.LoadScenes(index, sceneGroupData.sceneGroups[index], progress);
        }
        
        public void AddOnLoadListener(Action<int> listener) => manager.OnSceneGroupLoaded += listener;
        public void RemoveOnLoadListener(Action<int> listener) => manager.OnSceneGroupLoaded -= listener;

        
        private void Log(string message)
        {
            // Exit case - not debugging
            if (!debug) return;
            
            // Log the message
            Debug.Log($"[SceneLoader] {message}");
        }
    }
}
