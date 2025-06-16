using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.Transitions
{
    [Serializable]
    public class TransitionController
    {
        private AnimationCurve scaleCurve;

        private float transitionDuration;
        private float minScale;
        private float maxScale;

        private Tween currentTween;

        public event Action<TransitionData> OnTransitionUpdate;

        public TransitionController(TransitionPreset preset)
        {
            scaleCurve = preset.ScaleCurve;
            transitionDuration = preset.Duration;
            minScale = preset.MinScale;
            maxScale = preset.MaxScale;
        }

        public async UniTask TransitionOut(VisualElement element, TransitionDepth depth = TransitionDepth.Far)
        {
            float targetScale = depth == TransitionDepth.Far ? maxScale : minScale;
            await PerformTransition(element, 1f, targetScale, TransitionType.Exit, hideAfter: true);
        }

        public async UniTask TransitionIn(VisualElement element, TransitionDepth depth = TransitionDepth.Near)
        {
            float startScale = depth == TransitionDepth.Near ? minScale : maxScale;
            await PerformTransition(element, startScale, 1f, TransitionType.Enter, showBefore: true);
        }

        /// <summary>
        /// Performs a custom transition between two scale values
        /// </summary>
        public async UniTask TransitionScale(
            VisualElement element, 
            float fromScale,
            float toScale,
            TransitionType type,
            bool showBefore = false,
            bool hideAfter = false
        )
        {
            await PerformTransition(element, fromScale, toScale, type, showBefore, hideAfter);
        }

        private async UniTask PerformTransition(
            VisualElement element, 
            float fromScale, 
            float toScale,
            TransitionType type, 
            bool showBefore = false, 
            bool hideAfter = false
        )
        {
            // Kill any existing transition
            currentTween?.Kill();
            
            // Handle visibility
            if (showBefore)
            {
                element.style.display = DisplayStyle.Flex;
            }
            
            // Set initial scale
            ApplyScale(element, fromScale);

            // Determine the depth based on scale values
            TransitionDepth depth = DetermineDepth(fromScale, toScale, type);
            
            // Start tracking the completion source
            UniTaskCompletionSource tcs = new UniTaskCompletionSource();
            
            // Set the tween
            currentTween = DOVirtual.Float(0f, 1f, transitionDuration, progress =>
                {
                    // Evaluate the curve value for this progress
                    float curveValue = scaleCurve.Evaluate(progress);
                
                    // Interpolate scale based on the curve
                    float scale = Mathf.Lerp(fromScale, toScale, curveValue);
                    ApplyScale(element, scale);
                
                    // Fire update event
                    OnTransitionUpdate?.Invoke(new TransitionData
                    {
                        Progress = progress,
                        ScaleValue = curveValue,
                        Type = type,
                        Depth = depth
                    });
                })
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (hideAfter)
                    {
                        element.style.display = DisplayStyle.None;
                    }
                    tcs.TrySetResult();
                });
            
            await tcs.Task;
        }

        private TransitionDepth DetermineDepth(float fromScale, float toScale, TransitionType type)
        {
            if (type == TransitionType.Enter)
            {
                // Entering: check where we're coming from
                return fromScale > 1f ? TransitionDepth.Far : TransitionDepth.Near;
            }
            else
            {
                // Exiting: check where we're going
                return toScale > 1f ? TransitionDepth.Far : TransitionDepth.Near;
            }
        }
        
        private void ApplyScale(VisualElement element, float scale)
        {
            // Exit case: the element is null
            if (element == null) return;

            element.transform.scale = Vector3.one * scale;
        }

        /// <summary>
        /// Immediately sets an element to a specific scale
        /// </summary>
        public void SetScale(VisualElement element, float scale)
        {
            ApplyScale(element, scale);;
        }
        
        /// <summary>
        /// Stops any ongoing transition
        /// </summary>
        public void StopTransition() => currentTween?.Kill();
        
        /// <summary>
        /// Gets the evaluated scale value for a given progress value
        /// </summary>
        public float EvaluateScale(float progress, float fromScale, float toScale)
        {
            float curveValue = scaleCurve.Evaluate(progress);
            return Mathf.Lerp(fromScale, toScale, curveValue);
        }
    }
}
