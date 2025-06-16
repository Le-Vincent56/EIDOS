namespace EIDOS.UI.Main_Menu.Transitions
{
    public enum TransitionType
    {
        Exit,
        Enter
    }
    
    public struct TransitionData
    {
        public float Progress;
        public float ScaleValue;
        public TransitionType Type;
    }
}