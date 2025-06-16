using System;
using Cysharp.Threading.Tasks;

namespace EIDOS.UI.Main_Menu.Transitions
{
    /// <summary>
    /// A transition step that invokes a callback when executed.
    /// </summary>
    public class CallbackStep : ITransitionStep
    {
        private readonly Action callback;
        
        public CallbackStep(Action callback)
        {
            this.callback = callback;
        }
        
        /// <summary>
        /// Execute the callback and return a completed task.
        /// </summary>
        public UniTask Execute()
        {
            callback?.Invoke();
            return UniTask.CompletedTask;
        }
            
        /// <summary>
        /// Execute the callback and return a completed task
        /// </summary>
        public UniTask ExecuteReverse()
        {
            callback?.Invoke();
            return UniTask.CompletedTask;
        }
    }
}