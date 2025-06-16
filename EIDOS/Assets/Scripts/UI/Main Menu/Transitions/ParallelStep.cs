using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace EIDOS.UI.Main_Menu.Transitions
{
    /// <summary>
    /// Represents a composite transition step that executes multiple transition steps in parallel.
    /// </summary>
    /// <remarks>
    /// Each step in the collection is executed concurrently using UniTask's asynchronous functionality. Both
    /// forward and reverse execution (via `Execute` and `ExecuteReverse` methods, respectively) will wait
    /// for all steps to complete their tasks before completing.
    /// </remarks>
    public class ParallelStep : ITransitionStep
    {
        private readonly List<ITransitionStep> parallelSteps;
            
        public ParallelStep(List<ITransitionStep> parallelSteps)
        {
            this.parallelSteps = parallelSteps;
        }

        /// <summary>
        /// Executes all transition steps in the collection concurrently.
        /// </summary>
        /// <returns>A task that completes when all transition steps have finished executing.</returns>
        public async UniTask Execute()
        {
            List<UniTask> tasks = new List<UniTask>();
            
            foreach (ITransitionStep step in parallelSteps)
            {
                tasks.Add(step.Execute());
            }
            
            await UniTask.WhenAll(tasks);
        }

        /// <summary>
        /// Executes all transition steps in the collection concurrently in reverse order.
        /// </summary>
        /// <returns>A task that completes when all transition steps have finished executing in reverse.</returns>
        public async UniTask ExecuteReverse()
        {
            List<UniTask> tasks = new List<UniTask>();
            
            foreach (ITransitionStep step in parallelSteps)
            {
                tasks.Add(step.ExecuteReverse());
            }
            
            await UniTask.WhenAll(tasks);
        }
    }
}