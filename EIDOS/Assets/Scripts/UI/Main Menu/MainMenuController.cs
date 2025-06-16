using EIDOS.Debugging;
using EIDOS.Input;
using EIDOS.Stack_Machine;
using EIDOS.UI.Main_Menu.States;
using EIDOS.UI.Main_Menu.Transitions;
using UnityEngine;
using UnityEngine.UIElements;
using LogType = EIDOS.Debugging.LogType;

namespace EIDOS.UI.Main_Menu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private UIInputReader inputReader;

        [Header("Debug")] 
        [SerializeField] private bool debugTransitions;
        [SerializeField] private bool debugStack;

        [Header("Transition Settings")] 
        [SerializeField] private TransitionPreset preset;
        [SerializeField] private TransitionController transitionController;
        
        private AsyncStackMachine stackMachine;
        private UIDocument document;
        private VisualElement rootElement;
        private VisualElement mainContainer;
        private VisualElement saveContainer;
        private VisualElement settingsContainer;

        private void Awake()
        {
            // Get the UI document
            document = GetComponent<UIDocument>();
            
            // Create the transition controller
            transitionController = new TransitionController(preset);
        }

        private void OnEnable()
        {
            // Subscribe to input events
            inputReader.Cancel += Back;
            
            // Subscribe to transition events
            transitionController.OnTransitionUpdate += OnTransitionUpdate;
        }

        private void OnDisable()
        {
            // Unsubscribe to input events
            inputReader.Cancel -= Back;
            
            // Unsubscribe to transition events
            transitionController.OnTransitionUpdate += OnTransitionUpdate;
        }

        private void Start()
        {
            InitializeUIElements();
            
            // Create the stack machine
            CreateStackMachine();
        }

        private void InitializeUIElements()
        {
            // Get the root element
            rootElement = document.rootVisualElement;
            
            // Get the main container and save container
            mainContainer = rootElement.Query<VisualElement>("MainMenu");
            saveContainer = rootElement.Query<VisualElement>("SaveMenu");
            settingsContainer = rootElement.Query<VisualElement>("SettingsMenu");
            
            // Ensure all containers start hidden except main menu
            saveContainer.style.display = DisplayStyle.None;
            settingsContainer.style.display = DisplayStyle.None;
        }
        
        private void CreateStackMachine()
        {
            // Create the stack machine builder
            StackMachineBuilder builder = new StackMachineBuilder();
            // Create the stack machine
            stackMachine = builder
                .WithExitOnPush()
                .WithReenterOnPop()
                .WithDebug(debugStack)
                .WithLogPrefix("[Main Menu Stack]")
                .BuildAsync();
            
            // Create the initial main menu state
            MainMenuState menuState = new MainMenuState(
                this, 
                mainContainer, 
                transitionController,
                isInitialState: true,
                debug: debugStack
            );
                
            // Push the main menu state onto the stack
            stackMachine.Push(menuState);
        }

        public void OpenSaveMenu()
        {
            MainSaveState saveState = new MainSaveState(
                this, 
                saveContainer, 
                transitionController,
                isInitialState: false,
                debug: debugStack
            );
            
            stackMachine.Push(saveState);
        }

        public void OpenSettingsMenu()
        {
            MainSettingsState settingsState = new MainSettingsState(
                this, 
                settingsContainer, 
                transitionController,
                isInitialState: false,
                debug: debugStack
            );
            
            stackMachine.Push(settingsState);
        }

        private void Back(bool released)
        {
            // Exit case: the button hasn't been released
            if (!released) return;
            
            // Exit case: we're at the initial Main Menu, in which there's no state
            // to traverse back to
            if (stackMachine.CurrentState is MainMenuState) return;

            // Stop any ongoing transition
            transitionController.StopTransition();
            
            stackMachine.Pop();
        }
        
        private void OnTransitionUpdate(TransitionData data)
        {
            // Exit case: not debugging
            if (!debugTransitions) return;
            
            // Construct the progress message
            string message = $"Transition Progress: {data.Progress:F2}, Scale: {data.ScaleValue:F2})";
            
            // Send it to the debugger
            Debugger.Log("[MainMenuController]", message, LogType.Info);
        }
    }
}
