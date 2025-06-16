using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.Transitions
{
    public class TransitionSequenceBuilder
    {
        private readonly List<ITransitionStep> steps = new List<ITransitionStep>();
        private readonly TransitionController controller;

        public TransitionSequenceBuilder(TransitionController controller)
        {
            this.controller = controller;
        }

        public TransitionSequenceBuilder AddTransition(
            VisualElement exitElement, 
            VisualElement enterElement)
        {
            steps.Add(new TransitionStep(controller, exitElement, enterElement));
            return this;
        }

        public TransitionSequenceBuilder AddElementTransition(
            VisualElement element, 
            bool isEntering)
        {
            steps.Add(new SingleTransitionStep(controller, element, isEntering));
            return this;
        }
        
        public TransitionSequenceBuilder AddCustomTransition(
            VisualElement element, 
            float fromScale, 
            float toScale, 
            TransitionType type)
        {
            steps.Add(new CustomTransitionStep(controller, element, fromScale, toScale, type));
            return this;
        }

        public TransitionSequenceBuilder AddDelay(float seconds)
        {
            steps.Add(new DelayStep(seconds));
            return this;
        }

        public TransitionSequenceBuilder AddCallback(Action callback)
        {
            steps.Add(new CallbackStep(callback));
            return this;
        }

        /// <summary>
        /// Add a parallel group of transitions that execute simultaneously
        /// </summary>
        public TransitionSequenceBuilder AddParallel(Action<ParallelTransitionBuilder> configure)
        {
            var parallelBuilder = new ParallelTransitionBuilder(controller);
            configure(parallelBuilder);
            steps.Add(new ParallelStep(parallelBuilder.Build()));
            return this;
        }
        
        /// <summary>
        /// Execute the built sequence
        /// </summary>
        public async UniTask Execute()
        {
            foreach (ITransitionStep step in steps)
            {
                await step.Execute();
            }
        }
        
        /// <summary>
        /// Execute the sequence in reverse order
        /// </summary>
        public async UniTask ExecuteReverse()
        {
            for (int i = steps.Count - 1; i >= 0; i--)
            {
                await steps[i].ExecuteReverse();
            }
        }
    }
}