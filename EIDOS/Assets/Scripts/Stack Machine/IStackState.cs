namespace EIDOS.Stack_Machine
{
    public interface IStackState
    {
        void Enter();
        void Exit();
        void Update();
    }
}