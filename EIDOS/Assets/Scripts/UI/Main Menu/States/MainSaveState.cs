using DG.Tweening;
using EIDOS.Debugging;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.States
{
    public class MainSaveState : MainBaseState
    {
        private float scaleMultiplier;
        private float transitionDuration;
        
        public MainSaveState(
            MainMenuController controller, 
            VisualElement elementContainer) 
            : base(controller, elementContainer)
        {
        }
        
        public override void Enter()
        {
            Debugger.Log("[MainSaveState]", "Entered", LogType.Info);
        }

        public override void Exit()
        {
            Debugger.Log("[MainSaveState]", "Exited", LogType.Info);
        }
    }
}