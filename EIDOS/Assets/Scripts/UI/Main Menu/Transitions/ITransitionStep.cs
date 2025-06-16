using Cysharp.Threading.Tasks;

namespace EIDOS.UI.Main_Menu.Transitions
{
    public interface ITransitionStep
    {
        UniTask Execute();
        UniTask ExecuteReverse();
    }
}