using System;
using Cysharp.Threading.Tasks;
using EIDOS.Stack_Machine;
using EIDOS.UI.Main_Menu.Transitions;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UIElements;
using Debugger = EIDOS.Debugging.Debugger;
using LogType = EIDOS.Debugging.LogType;

namespace EIDOS.UI.Main_Menu.States
{
    public class MainBaseState : IAsyncStackState
    {
        protected readonly TransitionController TransitionController;
        protected readonly bool IsInitialState;

        protected readonly MainMenuController MenuController;
        protected readonly VisualElement ElementContainer;
        
        private readonly bool debug = false;
        
        protected MainBaseState(
            MainMenuController menuController,
            VisualElement elementContainer,
            TransitionController transitionController,
            bool isInitialState = false,
            bool debug = false)
        {
            MenuController = menuController;
            TransitionController = transitionController;
            ElementContainer = elementContainer;
            IsInitialState = isInitialState;
            this.debug = debug;
        }

        /// <summary>
        /// Handle entering the state.
        /// </summary>
        public virtual async UniTask Enter()
        {
            // Skip transition for initial state on the first entry
            if (IsInitialState && IsFirstTimeEntering())
            {
                ElementContainer.style.display = DisplayStyle.Flex;
                ElementContainer.transform.scale = Vector3.one;
                OnFirstTimeEntered();
            }
            else
            {
                await TransitionController.TransitionIn(ElementContainer);
            }
            
            await OnEnterComplete();
        }
        
        /// <summary>
        /// Handle exiting the state.
        /// </summary>
        public virtual async UniTask Exit()
        {
            await TransitionController.TransitionOut(ElementContainer);
            
            await OnExitStart();
        }
        
        /// <summary>
        /// Handle updating the state.
        /// </summary>
        public virtual UniTask Update() { return UniTask.CompletedTask; }

        /// <summary>
        /// Override to check if this is the first time entering the state
        /// </summary>
        protected virtual bool IsFirstTimeEntering() => false;
        
        /// <summary>
        /// Override to handle first time entry logic
        /// </summary>
        protected virtual void OnFirstTimeEntered() { }
        
        /// <summary>
        /// Override to perform logic after the entering transition completes
        /// </summary>
        protected virtual UniTask OnEnterComplete() => UniTask.CompletedTask;
        
        /// <summary>
        /// Override to perform logic before the exit transition starts
        /// </summary>
        protected virtual UniTask OnExitStart() => UniTask.CompletedTask;

        /// <summary>
        /// Logs a message for the specified state type and log type if debugging is enabled.
        /// </summary>
        /// <param name="stateObj">The state object that is logging the message.</param>
        /// <param name="message">The content or details of the message to be logged.</param>
        /// <param name="logType">The type of log to categorize the message (Info, Warning, Error).</param>
        protected void Log(IAsyncStackState stateObj, string message, LogType logType)
        {
            // Exit case: not debugging
            if (!debug) return;
            
            // Get the state type
            Type stateType = stateObj.GetType();

            Debugger.Log($"[{stateType.FullName}]", message, logType);
        }
    }
}
