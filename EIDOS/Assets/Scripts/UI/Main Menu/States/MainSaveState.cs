using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using LogType = EIDOS.Debugging.LogType;

namespace EIDOS.UI.Main_Menu.States
{
    public class MainSaveState : MainBaseState
    {
        private readonly Vector3 originalMenuScale;
        private Tween transitionTween;
        
        public MainSaveState(
            MainMenuController controller, 
            VisualElement elementContainer, 
            float scaleMultiplier,
            float transitionDuration,
            bool debug = false) 
            : base(controller, elementContainer, scaleMultiplier, transitionDuration, debug)
        {
            originalMenuScale = elementContainer.transform.scale;
            elementContainer.transform.scale = originalMenuScale * scaleMultiplier;
        }
        
        public override async UniTask Enter()
        {
            await TransitionIn();
            
            Log(this, "Entered", LogType.Info);
        }

        public override async UniTask Exit()
        {
            await TransitionOut();
            
            Log(this, "Exited", LogType.Info);
        }

        private UniTask TransitionOut()
        {
            // Kill the transition sequence if it exists
            transitionTween?.Kill();
            
            // Scale down the save menu until it's invisible
            transitionTween = DOVirtual.Float(1f, ScaleMultiplier, TransitionDuration, value =>
            {
                ElementContainer.transform.scale = originalMenuScale * value;
            }).SetEase(Ease.InQuint);
            
            // Hide the save menu after the transition is complete
            transitionTween.OnComplete(() =>
            {
                ElementContainer.style.display = DisplayStyle.None;
            });

            return transitionTween.ToUniTask();
        }

        private UniTask TransitionIn()
        {
            // Kill the transition sequence if it exists
            transitionTween?.Kill();
            
            // Ensure the element is displayed
            ElementContainer.style.display = DisplayStyle.Flex;
            
            // Scale the main menu in until it's in the original view
            transitionTween = DOVirtual.Float(ScaleMultiplier, 1f, TransitionDuration, value =>
            {
                ElementContainer.transform.scale = originalMenuScale * value;
            }).SetEase(Ease.OutQuint);

            return transitionTween.ToUniTask();
        }
    }
}