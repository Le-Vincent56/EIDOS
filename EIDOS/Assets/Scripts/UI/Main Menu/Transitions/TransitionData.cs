namespace EIDOS.UI.Main_Menu.Transitions
{
    public enum TransitionType
    {
        Exit,
        Enter
    }
    
    public enum TransitionDepth
    {
        Near,   // Transitions from/to minScale (close to camera)
        Far     // Transitions from/to maxScale (far from camera)
    }
    
    public struct TransitionData
    {
        public float Progress;
        public float ScaleValue;
        public TransitionType Type;
        public TransitionDepth Depth;
    }
}