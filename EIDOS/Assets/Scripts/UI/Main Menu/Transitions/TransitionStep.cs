using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.Transitions
{
    /// <summary>
    /// Represents a step in the transition process, handling the transition between two UI elements.
    /// </summary>
    public class TransitionStep : ITransitionStep
    {
        private readonly TransitionController controller;
        private readonly VisualElement exitElement;
        private readonly VisualElement enterElement;

        public TransitionStep(
            TransitionController controller, 
            VisualElement exitElement, 
            VisualElement enterElement
        )
        {
            this.controller = controller;
            this.exitElement = exitElement;
            this.enterElement = enterElement;
        }

        /// <summary>
        /// Executes the transition step, performing a transition between the specified UI elements.
        /// </summary>
        /// <returns>A UniTask that completes when the transition is finished.</returns>
        public UniTask Execute() => controller.TransitionBetween(exitElement, enterElement);

        /// <summary>
        /// Executes the reverse transition step, performing a transition in the opposite direction between the specified UI elements.
        /// </summary>
        /// <returns>A UniTask that completes when the reverse transition is finished.</returns>
        public UniTask ExecuteReverse() => controller.TransitionBetween(enterElement, exitElement);
    }
}