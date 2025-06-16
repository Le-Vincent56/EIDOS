using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.Transitions
{
    /// <summary>
    /// Represents a single-step transition for a visual element in the UI.
    /// This step can either transition the element in or out based on the specified mode.
    /// </summary>
    public class SingleTransitionStep : ITransitionStep
    {
        private readonly TransitionController controller;
        private readonly VisualElement element;
        private readonly bool isEntering;
            
        public SingleTransitionStep(
            TransitionController controller,
            VisualElement element,
            bool isEntering)
        {
            this.controller = controller;
            this.element = element;
            this.isEntering = isEntering;
        }

        /// <summary>
        /// Executes the transition step, either transitioning the visual element in or out based on the current configuration.
        /// </summary>
        /// <returns>A UniTask indicating the asynchronous execution of the transition step.</returns>
        public UniTask Execute() => isEntering ? controller.TransitionIn(element) : controller.TransitionOut(element);

        /// <summary>
        /// Executes the reverse transition step, switching the visual element's transition direction
        /// based on the current configuration.
        /// </summary>
        /// <returns>A UniTask indicating the asynchronous execution of the reverse transition step.</returns>
        public UniTask ExecuteReverse() => isEntering ? controller.TransitionOut(element) : controller.TransitionIn(element);
    }
}