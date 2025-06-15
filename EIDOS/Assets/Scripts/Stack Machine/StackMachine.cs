using EIDOS.Debugging;

namespace EIDOS.Stack_Machine
{
    public class StackMachine : BaseStackMachine<IStackState>
    {
        internal StackMachine(StackMachineConfig config) : base(config) { }

        public void Push(IStackState newState)
        {
            // Check if the state should exit on push
            if (ShouldExitOnPush() && CurrentState != null)
            {
                // Exit current state
                CurrentState.Exit();
            }
            
            // Push state and enter it
            StateStack.Push(newState);
            newState.Enter();
            
            Log($"Pushed async state: {newState.GetType().Name}", LogType.Info);
        }

        public void Pop()
        {
            // Exit case: no states to pop
            if (!HasStates()) return;

            // Exit current state
            CurrentState?.Exit();
            
            // Pop state
            StateStack.Pop();
            
            Log($"Popped async state: {CurrentState?.GetType().Name}", LogType.Info);
            
            // Exit case: not configured to reenter on pop
            if (!ShouldReenterOnPop()) return;
                
            // Reenter the current state
            CurrentState?.Enter();
        }

        public override void Update()
        {
            // Exit case: no states to update
            if (!HasStates()) return;
            
            // Update current state
            CurrentState.Update();
        }
    }
}