using EIDOS.UI.Scenes;

namespace EIDOS.Event_Bus.Events
{
    public struct LoadScene : IEvent
    {
        public int SceneIndex;
        public SceneTransitioner.Type TransitionInType;
        public SceneTransitioner.Type TransitionOutType;
    }
    
    public struct Transition : IEvent
    {
        public SceneTransitioner.Type Type;
    }
}
