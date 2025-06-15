using Cysharp.Threading.Tasks;
using DG.Tweening;
using EIDOS.Debugging;
using EIDOS.Extensions;
using UnityEngine;
using UnityEngine.UIElements;
using LogType = EIDOS.Debugging.LogType;

namespace EIDOS.UI.Main_Menu.States
{
    public class MainMenuState : MainBaseState
    {
        private bool firstTimeEntering;
        private readonly Button startButton;
        private readonly Button settingsButton;
        private readonly Button quitButton;
        private readonly Vector3 originalMenuScale;
        private Tween transitionTween;
        
        public MainMenuState(
            MainMenuController controller, 
            VisualElement elementContainer, 
            float scaleMultiplier,
            float transitionDuration,
            bool debug = false) 
            : base(controller, elementContainer, scaleMultiplier, transitionDuration, debug)
        {
            firstTimeEntering = true;
            
            startButton = elementContainer.Query<Button>("StartButton");
            settingsButton = elementContainer.Query<Button>("SettingsButton");
            quitButton = elementContainer.Query<Button>("QuitButton");

            originalMenuScale = elementContainer.transform.scale;
            
            startButton.clicked += OnStartButtonClicked;
            settingsButton.clicked += OnSettingsButtonClicked;
            quitButton.clicked += OnQuitButtonClicked;
        }

        public override async UniTask Enter()
        {
            // Check if it is not the first time entering the state (the menu just loaded)
            if(!firstTimeEntering) 
                // If it is, scale the menu down until it is visible
                await TransitionIn();
            else 
                // Otherwise, do nothing
                firstTimeEntering = false;
            
            Log(this, "Entered", LogType.Info);
        }

        public override async UniTask Exit()
        {
            startButton.clicked -= OnStartButtonClicked;
            settingsButton.clicked -= OnSettingsButtonClicked;
            quitButton.clicked -= OnQuitButtonClicked;
            
            await TransitionOut();
            
            Log(this, "Exited", LogType.Info);
        }

        private void OnStartButtonClicked() => Controller.OpenSaveMenu();

        private void OnSettingsButtonClicked() => Controller.OpenSettingsMenu();

        private void OnQuitButtonClicked() => Application.Quit();

        private UniTask TransitionOut()
        {
            // Kill the transition sequence if it exists
            transitionTween?.Kill();
            
            // Scale up the main menu until it's past the view
            transitionTween = DOVirtual.Float(1f, ScaleMultiplier, TransitionDuration, value =>
            {
                ElementContainer.transform.scale = originalMenuScale * value;
            }).SetEase(Ease.InQuint);
            
            // Hide the main menu after the transition is complete
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