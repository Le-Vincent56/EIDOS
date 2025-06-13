using System;
using Eidos.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EIDOS.Input
{
    public abstract class InputReader : ScriptableObject
    {
        protected EIDOS_InputActions InputActions;
        
        protected virtual void OnEnable() => Enable();
        protected virtual void OnDisable() => Disable();
        
        public abstract void Enable();
        public abstract void Disable();

        /// <summary>
        /// Invokes a boolean action based on the input action phase.
        /// The action is invoked with a value of true when the phase is `Canceled`,
        /// and false when the phase is `Started`.
        /// </summary>
        protected static void InvokeBoolean(Action<bool> action, InputActionPhase phase)
        {
            switch (phase)
            {
                case InputActionPhase.Started:
                    action.Invoke(false);
                    break;

                case InputActionPhase.Canceled:
                    action.Invoke(true);
                    break;

                case InputActionPhase.Disabled:
                case InputActionPhase.Waiting:
                case InputActionPhase.Performed:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
