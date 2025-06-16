using Cysharp.Threading.Tasks;
using EIDOS.UI.Main_Menu.Transitions;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.States
{
    public class MainSettingsState : MainBaseState
    {
        public MainSettingsState(MainMenuController controller, 
            VisualElement elementContainer, 
            TransitionController transitionController,
            bool isInitialState = false,
            bool debug = false) 
            : base(controller, elementContainer, transitionController, isInitialState, debug)
        {
        }
    }
}