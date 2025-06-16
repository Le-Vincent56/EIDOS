using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.Transitions
{
    /// <summary>
    /// Represents a custom transition step for scaling a UI element between two scale values.
    /// </summary>
    public class CustomTransitionStep : ITransitionStep
    {
        private readonly TransitionController controller;
        private readonly VisualElement element;
        private readonly float fromScale;
        private readonly float toScale;
        private readonly TransitionType type;
            
        public CustomTransitionStep(
            TransitionController controller,
            VisualElement element,
            float fromScale,
            float toScale,
            TransitionType type)
        {
            this.controller = controller;
            this.element = element;
            this.fromScale = fromScale;
            this.toScale = toScale;
            this.type = type;
        }

        /// <summary>
        /// Executes the transition step, applying a scale transformation to a specified UI element
        /// from the initial scale value to the target scale value based on the specified transition type.
        /// </summary>
        /// <returns>A UniTask representing the asynchronous execution of the transition.</returns>
        public UniTask Execute() => controller.TransitionScale(element, fromScale, toScale, type);

        /// <summary>
        /// Executes the reverse transition step, applying a scale transformation to a specified UI element
        /// from the target scale value back to the initial scale value based on the specified transition type.
        /// </summary>
        /// <returns>A UniTask representing the asynchronous execution of the reverse transition.</returns>
        public UniTask ExecuteReverse() => controller.TransitionScale(element, toScale, fromScale, type);
    }
}