using System;
using Eidos.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EIDOS.Input
{
    [CreateAssetMenu(fileName = "UI Input Reader", menuName = "Input/UI Input Reader")]
    public class UIInputReader : InputReader, EIDOS_InputActions.IUIActions
    {
        public event Action<Vector2> Navigate = delegate { };
        public event Action<bool> Submit = delegate { };
        public event Action<bool> Cancel = delegate { };
        public event Action<bool> Click = delegate { };
        
        private EIDOS_InputActions inputActions;

        /// <summary>
        /// Enables the associated input actions, allowing them
        /// to process player inputs.
        /// If the input actions are not initialized, this method
        /// initializes them and sets their callbacks before enabling.
        /// </summary>
        public override void Enable()
        {
            // Verify that the input actions have been initialized
            if (inputActions == null)
            {
                // If not, initialize them and set its callbacks
                inputActions = new EIDOS_InputActions();
                inputActions.UI.SetCallbacks(this);
            }

            // Enable the input actions
            inputActions.Enable();
        }

        /// <summary>
        /// Disables the associated input actions,
        /// preventing further input processing until re-enabled.
        /// </summary>
        public override void Disable() => inputActions?.Disable();

        public void OnNavigate(InputAction.CallbackContext context)
        {
            Navigate.Invoke(context.ReadValue<Vector2>());
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            InvokeBoolean(Submit, context.phase);
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            InvokeBoolean(Cancel, context.phase);
        }
        
        public void OnClick(InputAction.CallbackContext context)
        {
            InvokeBoolean(Click, context.phase);
        }

        public void OnPoint(InputAction.CallbackContext context) { /* No op */ }

        public void OnRightClick(InputAction.CallbackContext context) { /* No op */ }

        public void OnMiddleClick(InputAction.CallbackContext context) { /* No op */ }

        public void OnScrollWheel(InputAction.CallbackContext context) { /* No op */ }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context) { /* No op */ }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { /* No op */ }
    }
}
