using EIDOS.Stack_Machine;
using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.States
{
    public class MainBaseState : IStackState
    {
        protected readonly MainMenuController Controller;
        protected VisualElement ElementContainer;
        
        protected MainBaseState(
            MainMenuController controller, 
            VisualElement elementContainer) 
        {
            Controller = controller;
            ElementContainer = elementContainer;
        }
        
        public virtual void Enter() { /* No op */ }
        public virtual void Update() { /* No op */ }
        public virtual void Exit() { /* No op */ }
    }
}
