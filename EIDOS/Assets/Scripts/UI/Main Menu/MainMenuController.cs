using EIDOS.Input;
using EIDOS.Stack_Machine;
using EIDOS.UI.Main_Menu.States;
using UnityEngine;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private UIInputReader inputReader;
        
        private StackMachine stackMachine;
        private UIDocument document;
        private VisualElement rootElement;
        private VisualElement mainContainer;
        private VisualElement saveContainer;
        private VisualElement settingsContainer;

        private void Awake()
        {
            // Get the UI document
            document = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            inputReader.Cancel += Back;
        }

        private void OnDisable()
        {
            inputReader.Cancel -= Back;
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
        }
        
        private void CreateStackMachine()
        {
            // Create the stack machine
            stackMachine = new StackMachine();
            
            // Create the initial main menu state
            MainMenuState menuState = new MainMenuState(this, mainContainer);
                
            // Push the main menu state onto the stack
            stackMachine.Push(menuState);
        }

        public void OpenSaveMenu()
        {
            MainSaveState saveState = new MainSaveState(this, saveContainer);
            
            stackMachine.Push(saveState);
        }

        public void OpenSettingsMenu()
        {
            MainSettingsState settingsState = new MainSettingsState(this, settingsContainer);
            
            stackMachine.Push(settingsState);
        }

        private void Back(bool released)
        {
            // Exit case: the button hasn't been released
            if (!released) return;
            
            // Exit case: we're at the initial Main Menu, in which there's no state
            // to traverse back to
            if (stackMachine.CurrentState is MainMenuState) return;
            
            stackMachine.Pop();
        }
    }
}
