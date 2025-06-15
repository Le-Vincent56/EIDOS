using UnityEngine;

namespace EIDOS.Stack_Machine
{
    /// <summary>
    /// Common interface for all stack machines
    /// </summary>
    public interface IStackMachine<out TState>
    {
        TState CurrentState { get; }
        void Update();
    }
}
