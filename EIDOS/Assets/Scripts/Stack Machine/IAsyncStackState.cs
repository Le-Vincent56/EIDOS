using Cysharp.Threading.Tasks;

namespace EIDOS.Stack_Machine
{
    public interface IAsyncStackState
    {
        UniTask Enter();
        UniTask Exit();
        UniTask Update();
    }
}