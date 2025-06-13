using EIDOS.Debugging;
using EIDOS.Stack_Machine;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.States
{
    public class MainSettingsState : MainBaseState
    {
        public MainSettingsState(
            MainMenuController controller, 
            VisualElement elementContainer) 
            : base(controller, elementContainer)
        {
        }
        
        public override void Enter()
        {
            Debugger.Log("[MainSettingsState]", "Entered", LogType.Info);
        }

        public override void Exit()
        {
            Debugger.Log("[MainMenuState]", "Exited", LogType.Info);
        }
    }
}