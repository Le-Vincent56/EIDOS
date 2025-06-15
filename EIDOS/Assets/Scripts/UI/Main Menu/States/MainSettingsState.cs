using Cysharp.Threading.Tasks;
using EIDOS.Debugging;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.States
{
    public class MainSettingsState : MainBaseState
    {
        public MainSettingsState(MainMenuController controller, 
            VisualElement elementContainer, 
            float scaleMultiplier,
            float transitionDuration,
            bool debug = false) 
            : base(controller, elementContainer, scaleMultiplier, transitionDuration, debug)
        {
        }
        
        public override UniTask Enter()
        {
            Log(this, "Entered", LogType.Info);
            return base.Enter();
        }

        public override UniTask Exit()
        {
            Log(this, "Exited", LogType.Info);
            return base.Exit();
        }
    }
}