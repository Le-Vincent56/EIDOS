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

        public async UniTask TransitionOut(VisualElement element)
        {
            await PerformTransition(element, 1f, maxScale, TransitionType.Exit, hideAfter: true);
        }

        public async UniTask TransitionIn(VisualElement element)
        {
            await PerformTransition(element, minScale, 1f, TransitionType.Enter, showBefore: true);
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
        
        /// <summary>
        /// Performs a simultaneous transition: one element out, another in
        /// </summary>
        public async UniTask TransitionBetween(VisualElement exitElement, VisualElement enterElement)
        {
            // Kill any existing transition
            currentTween?.Kill();
            
            // Ensure both elements are visible at the start
            exitElement.style.display = DisplayStyle.Flex;
            enterElement.style.display = DisplayStyle.Flex;
            
            // Set initial scales
            ApplyScale(exitElement, 1f);
            ApplyScale(enterElement, minScale);
            
            UniTaskCompletionSource tcs = new UniTaskCompletionSource();
            
            currentTween = DOVirtual.Float(0f, 1f, transitionDuration, progress =>
                {
                    // Evaluate the curve value for this progress
                    float curveValue = scaleCurve.Evaluate(progress);
                
                    // Exit element: interpolate from 1 to maxScale
                    float exitScale = Mathf.Lerp(1f, maxScale, curveValue);
                    ApplyScale(exitElement, exitScale);
                
                    // Enter element: interpolate from minScale to 1
                    float enterScale = Mathf.Lerp(minScale, 1f, curveValue);
                    ApplyScale(enterElement, enterScale);
                
                    // Fire events for both transitions
                    OnTransitionUpdate?.Invoke(new TransitionData
                    {
                        Progress = progress,
                        ScaleValue = curveValue,
                        Type = TransitionType.Exit
                    });
                })
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    exitElement.style.display = DisplayStyle.None;
                    tcs.TrySetResult();
                });
            
            await tcs.Task;
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
            
            UniTaskCompletionSource tcs = new UniTaskCompletionSource();
            
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
                        Type = type
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
