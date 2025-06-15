namespace EIDOS.Stack_Machine
{
    public abstract class BaseStackMachine<TState> : IStackMachine<TState>
    {
        protected readonly Stack<TState> stateStack = new Stack<TState>();
    }
}