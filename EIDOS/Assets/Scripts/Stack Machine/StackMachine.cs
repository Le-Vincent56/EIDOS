using System.Collections.Generic;

namespace EIDOS.Stack_Machine
{
    public class StackMachine
    {
        private readonly Stack<IStackState> stateStack = new Stack<IStackState>();
        
        public IStackState CurrentState => stateStack.Count > 0 ? stateStack.Peek() : null;

        public void Push(IStackState newState)
        {
            stateStack.Push(newState);
            newState.Enter();
        }

        public void Pop()
        {
            if (stateStack.Count == 0) return;

            CurrentState.Exit();
            stateStack.Pop();
        }

        public void Update()
        {
            if (stateStack.Count == 0) return;

            CurrentState?.Update();
        }
    }
}