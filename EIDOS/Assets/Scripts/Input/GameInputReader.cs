using System;
using Eidos.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EIDOS.Input
{
    [CreateAssetMenu(fileName = "Game Input Reader", menuName = "Input/Game Input Reader")]
    public class GameInputReader : InputReader, EIDOS_InputActions.IPlayerActions
    {
        public event Action<Vector2> Move = delegate { };
        public event Action<bool> Interact = delegate { };
        
        public int NormMoveX { get; private set; }
        public int NormMoveY { get; private set; }

        /// <summary>
        /// Enables the associated input actions, allowing them
        /// to process player inputs.
        /// If the input actions are not initialized, this method
        /// initializes them and sets their callbacks before enabling.
        /// </summary>
        public override void Enable()
        {
            // Verify that the input actions have been initialized
            if (InputActions == null)
            {
                // If not, initialize them and set its callbacks
                InputActions = new EIDOS_InputActions();
                InputActions.Player.SetCallbacks(this);
            }

            // Enable the input actions
            InputActions.Enable();
        }

        /// <summary>
        /// Disables the associated input actions,
        /// preventing further input processing until re-enabled.
        /// </summary>
        public override void Disable() => InputActions?.Disable();

        /// <summary>
        /// Handles the player's movement input by reading the raw movement vector
        /// from the input action context, invoking the movement event, and updating
        /// normalized movement properties.
        /// </summary>
        public void OnMove(InputAction.CallbackContext context)
        {
            // Get the raw movement input from the control context
            Vector2 rawMovementInput = context.ReadValue<Vector2>();
            
            // Invoke the movement event
            Move?.Invoke(rawMovementInput);
            
            // Set public movement properties
            NormMoveX = (int)(rawMovementInput * Vector2.right).normalized.x;
            NormMoveY = (int)(rawMovementInput * Vector2.up).normalized.y;
        }

        /// <summary>
        /// Handles the interaction input action from the player, triggering the associated
        /// event based on the current input action phase received from the callback context.
        /// </summary>
        public void OnInteract(InputAction.CallbackContext context)
        {
            InvokeBoolean(Interact, context.phase);
        }
    }
}
