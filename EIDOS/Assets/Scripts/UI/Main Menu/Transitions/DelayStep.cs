using Cysharp.Threading.Tasks;

namespace EIDOS.UI.Main_Menu.Transitions
{
    /// <summary>
    /// Represents a delay step in a transition sequence. This step introduces
    /// a pause for the specified duration in seconds before allowing the next
    /// transition operation to proceed.
    /// Implements the ITransitionStep interface to integrate the delay as part
    /// of a broader transition sequence or workflow.
    /// </summary>
    public class DelayStep : ITransitionStep
    {
        private readonly float seconds;
            
        public DelayStep(float seconds)
        {
            this.seconds = seconds;
        }

        /// <summary>
        /// Executes the assigned transition operation asynchronously.
        /// </summary>
        /// <returns>A UniTask representing the asynchronous operation of the implemented transition step.</returns>
        public UniTask Execute() => UniTask.WaitForSeconds(seconds);

        /// <summary>
        /// Executes the reverse transition operation asynchronously.
        /// </summary>
        /// <returns>A UniTask representing the asynchronous execution of the reverse transition step.</returns>
        public UniTask ExecuteReverse() => UniTask.WaitForSeconds((seconds));
    }
}