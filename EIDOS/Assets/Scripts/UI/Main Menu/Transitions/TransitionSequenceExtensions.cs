using UnityEngine.UIElements;

namespace EIDOS.UI.Main_Menu.Transitions
{
    /// <summary>
    /// Extension methods for easier sequence building
    /// </summary>
    public static class TransitionSequenceExtensions
    {
        /// <summary>
        /// Create a sequence that transitions from the current state to a new menu
        /// </summary>
        public static TransitionSequenceBuilder CreateMenuTransition(
            this TransitionController controller,
            VisualElement currentMenu,
            VisualElement newMenu)
        {
            return new TransitionSequenceBuilder(controller)
                .AddTransition(currentMenu, newMenu);
        }
        
        /// <summary>
        /// Create a complex multi-menu transition
        /// </summary>
        public static TransitionSequenceBuilder CreateMultiMenuTransition(
            this TransitionController controller,
            params VisualElement[] menus)
        {
            TransitionSequenceBuilder builder = new TransitionSequenceBuilder(controller);
            
            for (int i = 0; i < menus.Length - 1; i++)
            {
                builder.AddTransition(menus[i], menus[i + 1]);
                if (i < menus.Length - 2)
                {
                    builder.AddDelay(0.1f); // Small delay between transitions
                }
            }
            
            return builder;
        }
    }
}