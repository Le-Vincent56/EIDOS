using Cysharp.Threading.Tasks;
using EIDOS.UI.Main_Menu.Transitions;
using UnityEngine;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.States
{
    public class MainMenuState : MainBaseState
    {
        private bool firstTimeEntering;
        private readonly Button startButton;
        private readonly Button settingsButton;
        private readonly Button quitButton;
        
        public MainMenuState(
            MainMenuController menuController, 
            VisualElement elementContainer, 
            TransitionController transitionController,
            bool isInitialState = false,
            bool debug = false) 
            : base(menuController, elementContainer, transitionController, isInitialState, debug)
        {
            firstTimeEntering = true;
            
            startButton = elementContainer.Query<Button>("StartButton");
            settingsButton = elementContainer.Query<Button>("SettingsButton");
            quitButton = elementContainer.Query<Button>("QuitButton");
        }

        protected override bool IsFirstTimeEntering()
        {
            bool isFirst = firstTimeEntering;
            firstTimeEntering = false;
            return isFirst;
        }
        
        protected override async UniTask OnEnterComplete()
        {
            // Subscribe to button events
            startButton.clicked += OnStartButtonClicked;
            settingsButton.clicked += OnSettingsButtonClicked;
            quitButton.clicked += OnQuitButtonClicked;

            await base.OnEnterComplete();
        }

        protected override async UniTask OnExitStart()
        {
            // Unsubscribe from button events
            startButton.clicked -= OnStartButtonClicked;
            settingsButton.clicked -= OnSettingsButtonClicked;
            quitButton.clicked -= OnQuitButtonClicked;

            await base.OnExitStart();
        }

        private void OnStartButtonClicked() => MenuController.OpenSaveMenu();

        private void OnSettingsButtonClicked() => MenuController.OpenSettingsMenu();

        private void OnQuitButtonClicked() => Application.Quit();
    }
}