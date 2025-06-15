using System.Collections.Generic;
using EIDOS.Debugging;

namespace EIDOS.Stack_Machine
{
    public abstract class BaseStackMachine<TState> : IStackMachine<TState>
    {
        protected readonly Stack<TState> StateStack = new Stack<TState>();
        private readonly StackMachineConfig config;
        
        public TState CurrentState => StateStack.Count > 0 ? StateStack.Peek() : default;

        protected BaseStackMachine(StackMachineConfig config)
        {
            this.config = config ?? new StackMachineConfig();
        }

        public abstract void Update();

        protected void Log(string message, LogType type)
        {
            // Exit case: not configurated to debug
            if (!config.Debug) return;
            
            // Log message
            Debugger.Log(config.LogPrefix, message, type);
        }

        protected bool ShouldExitOnPush() => config.ExitOnPush;
        protected bool ShouldReenterOnPop() => config.ReenterOnPop;
        protected bool HasStates() => StateStack.Count > 0;
    }
}