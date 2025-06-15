using System;
using EIDOS.Debugging;

namespace EIDOS.Stack_Machine
{
    public class AsyncStackMachine : BaseStackMachine<IAsyncStackState>
    {
        internal AsyncStackMachine(StackMachineConfig config) : base(config) { }

        public async void Push(IAsyncStackState newState)
        {
            try
            {
                // Check if the state should exit on push 
                if (ShouldExitOnPush() && CurrentState != null)
                {
                    await CurrentState.Exit();
                }

                // Push state and enter it
                StateStack.Push(newState);
                await newState.Enter();
                
                Log($"Pushed async state: {newState.GetType().Name}", LogType.Info);
            }
            catch (Exception e)
            {
                Log($"Couldn't push async state to the Stack Machine: {e}", LogType.Error);
            }
        }

        public async void Pop()
        {
            try
            {
                // Exit case: no states to pop
                if (!HasStates()) return;

                // Await the exit of the current state
                await CurrentState.Exit();
                
                // Pop the current state off of the stack
                StateStack.Pop();

                Log($"Popped async state: {CurrentState.GetType().Name}", LogType.Info);

                // Exit case: not configured to reenter on pop
                if (!ShouldReenterOnPop()) return;
                
                // Reenter the current state
                await CurrentState.Enter();
            }
            catch (Exception e)
            {
                Log($"Couldn't pop async state from the Stack Machine: {e}", LogType.Error);
            }
        }

        public override void Update()
        {
            // Exit case: no states to update
            if (!HasStates()) return;
            
            // Update current state
            CurrentState?.Update();
        }
    }
}