using System;
using Cysharp.Threading.Tasks;
using EIDOS.Debugging;
using EIDOS.Stack_Machine;
using UnityEngine.UIElements;
using Debugger = EIDOS.Debugging.Debugger;

namespace EIDOS.UI.Main_Menu.States
{
    public class MainBaseState : IAsyncStackState
    {
        protected readonly MainMenuController Controller;
        protected readonly VisualElement ElementContainer;
        protected readonly float ScaleMultiplier;
        protected readonly float TransitionDuration;
        private readonly bool debug = false;
        
        protected MainBaseState(
            MainMenuController controller,
            VisualElement elementContainer, 
            float scaleMultiplier,
            float transitionDuration,
            bool debug = false) 
        {
            Controller = controller;
            ElementContainer = elementContainer;
            ScaleMultiplier = scaleMultiplier;
            TransitionDuration = transitionDuration;
            this.debug = debug;
        }
        
        public virtual UniTask Enter() { return UniTask.CompletedTask; }
        public virtual UniTask Update() { return UniTask.CompletedTask; }
        public virtual UniTask Exit() { return UniTask.CompletedTask; }

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
