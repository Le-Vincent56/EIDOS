using System.Collections.Generic;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.Transitions
{
    /// <summary>
    /// Builder for parallel transition groups
    /// </summary>
    public class ParallelTransitionBuilder
    {
        private readonly TransitionController controller;
        private readonly List<ITransitionStep> steps = new List<ITransitionStep>();
            
        internal ParallelTransitionBuilder(TransitionController controller)
        {
            this.controller = controller;
        }
            
        public ParallelTransitionBuilder AddTransition(
            VisualElement exitElement,
            VisualElement enterElement)
        {
            steps.Add(new TransitionStep(controller, exitElement, enterElement));
            return this;
        }
            
        public ParallelTransitionBuilder AddElementTransition(
            VisualElement element,
            bool isEntering)
        {
            steps.Add(new SingleTransitionStep(controller, element, isEntering));
            return this;
        }
            
        internal List<ITransitionStep> Build() => steps;
    }
}