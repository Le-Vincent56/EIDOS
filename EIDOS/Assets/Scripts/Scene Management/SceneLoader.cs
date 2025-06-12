using Cysharp.Threading.Tasks;
using System;
using EIDOS.Event_Bus;
using EIDOS.Event_Bus.Events;
using EIDOS.UI.Scenes;
using UnityEngine;

namespace EIDOS.Scene_Management
{
    /// <summary>
    /// Manages scene loading operations with transition effects and event-driven architecture.
    /// Handles loading scene groups with customizable transition animations between scenes.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SceneGroupData sceneGroupData;
        private SceneGroupManager manager;

        [Header("Fields")] 
        [SerializeField] private bool debug;
        [SerializeField] private bool isLoading;
        
        private EventBinding<LoadScene> onLoadScene;
        
        public SceneGroup[] SceneGroups => sceneGroupData.sceneGroups;
        
        private void Awake()
        {
            // Initialize the scene group manager
            manager = new SceneGroupManager();
        }

        private void OnEnable()
        {
            // Register event subscriptions
            onLoadScene = new EventBinding<LoadScene>(OnLoadScene);
            EventBus<LoadScene>.Register(onLoadScene);
        }

        private void OnDisable()
        {
            // Deregister event subscriptions
            EventBus<LoadScene>.Deregister(onLoadScene);
        }

        private void Start()
        {
            // Automatically load the initial scene specified in the configuration data
            TryLoadScene(sceneGroupData.initialScene);
        }

        /// <summary>
        /// Event handler for LoadScene events - extracts data and initiates scene loading
        /// </summary>
        /// <param name="eventData">Contains scene index and transition types for the loading operation</param>
        private void OnLoadScene(LoadScene eventData) => TryLoadScene(eventData.SceneIndex, eventData.TransitionInType, eventData.TransitionOutType);

        /// <summary>
        /// Safely attempts to load a scene group with error handling and default transition values
        /// </summary>
        /// <param name="sceneIndex">Index of the scene group to load</param>
        /// <param name="inTransition">Transition effect when entering the new scene (default: FadeIn)</param>
        /// <param name="outTransition">Transition effect when leaving the current scene (default: FadeOut)</param>
        private async void TryLoadScene(int sceneIndex, 
            SceneTransitioner.Type inTransition = SceneTransitioner.Type.FadeIn, 
            SceneTransitioner.Type outTransition = SceneTransitioner.Type.FadeOut
        )
        {
            try
            {
                await LoadSceneGroup(sceneIndex, inTransition, outTransition);
            }
            catch (Exception e)
            {
                Log($"Could not successfully load the scene group {sceneGroupData.name}", e);
            }
        }

        /// <summary>
        /// Performs the complete scene loading sequence: offload current scene, load new scene, onboard new scene
        /// </summary>
        /// <param name="index">Index of the scene group to load</param>
        /// <param name="inTransition">Transition effect for entering the new scene</param>
        /// <param name="outTransition">Transition effect for leaving the current scene</param>
        private async UniTask LoadSceneGroup(int index, 
            SceneTransitioner.Type inTransition, 
            SceneTransitioner.Type outTransition
        )
        {
            // Validate the scene index before proceeding
            if (!IsValidSceneIndex(index)) return;

            // Create a progress tracker for the loading operation
            LoadingProgress progress = new LoadingProgress();
            
            // Start exit processes for the current scene
            Offload(outTransition);
            
            // Load the new scene group asynchronously
            await manager.LoadScenes(index, sceneGroupData.sceneGroups[index], progress);
            
            // Start entrance processes for the new scene
            Onboard(inTransition);
        }

        /// <summary>
        /// Validates that the provided scene index is within the valid range of available scene groups
        /// </summary>
        /// <param name="index">Scene index to validate</param>
        /// <returns>True if the index is valid, false otherwise</returns>
        private bool IsValidSceneIndex(int index) => index >= 0 && index < sceneGroupData.sceneGroups.Length;

        /// <summary>
        /// Initiates exit processing within the current scene
        /// </summary>
        /// <param name="transitionType">Type of transition effect to play</param>
        private void Offload(SceneTransitioner.Type transitionType)
        {
            // Raise transition event to trigger visual effects
            EventBus<Transition>.Raise(new Transition { Type = transitionType });
        }

        /// <summary>
        /// Initiates entrance processing within the new scene
        /// </summary>
        /// <param name="transitionType">Type of transition effect to play</param>
        private void Onboard(SceneTransitioner.Type transitionType)
        {
            // Raise transition event to trigger visual effects
            EventBus<Transition>.Raise(new Transition { Type = transitionType });
        }

        /// <summary>
        /// Conditional logging utility that only outputs messages when debug mode is enabled
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="e">Optional exception to include in the log</param>
        private void Log(string message, Exception e = null)
        {
            // Exit early if debugging is disabled
            if (!debug) return;
                
            Debug.Log($"[SceneLoader] {message}");
        }
    }
}