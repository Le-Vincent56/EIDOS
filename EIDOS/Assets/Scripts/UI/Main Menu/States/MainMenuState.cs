using EIDOS.Debugging;
using UnityEngine.UIElements;
using LogType = EIDOS.Debugging.LogType;

namespace EIDOS.UI.Main_Menu.States
{
    public class MainMenuState : MainBaseState
    {
        private readonly Button startButton;
        private readonly Button settingsButton;
        private readonly Button quitButton;
        
        public MainMenuState(
            MainMenuController controller, 
            VisualElement elementContainer) 
            : base(controller, elementContainer)
        {
            startButton = elementContainer.Query<Button>("StartButton");
            settingsButton = elementContainer.Query<Button>("SettingsButton");
            quitButton = elementContainer.Query<Button>("QuitButton");
            
            startButton.clicked += OnStartButtonClicked;
            settingsButton.clicked += OnSettingsButtonClicked;
            quitButton.clicked += OnQuitButtonClicked;
        }

        public override void Enter()
        {
            Debugger.Log("[MainMenuState]", "Entered", LogType.Info);
        }

        public override void Exit()
        {
            startButton.clicked -= OnStartButtonClicked;
            settingsButton.clicked -= OnSettingsButtonClicked;
            quitButton.clicked -= OnQuitButtonClicked;
            
            Debugger.Log("[MainMenuState]", "Exited", LogType.Info);
        }

        private void OnStartButtonClicked() => Controller.OpenSaveMenu();

        private void OnSettingsButtonClicked() => Controller.OpenSettingsMenu();

        private void OnQuitButtonClicked()
        {
            
        }
    }
}