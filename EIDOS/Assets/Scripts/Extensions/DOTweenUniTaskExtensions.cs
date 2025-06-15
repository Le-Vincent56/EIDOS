using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace EIDOS.Extensions
{
    public static class DOTweenUniTaskExtensions
    {
        /// <summary>
        /// Converts a DOTween Tween into a UniTask that completes when the tween is finished or canceled.
        /// </summary>
        /// <param name="tween">The DOTween Tween to be converted into a UniTask.</param>
        /// <returns>A UniTask that completes when the specified tween finishes or is canceled.</returns>
        public static UniTask ToUniTask(this Tween tween)
        {
            // Exit case: the tween is not active
            if(!tween.IsActive()) return UniTask.CompletedTask;
            
            // Create a completion source
            UniTaskCompletionSource completionSource = new UniTaskCompletionSource();
            
            // When the tween is completed, set the result for the completion source
            tween.OnComplete(() => completionSource.TrySetResult());
            
            // If the tween is killed, set the completion source as cancelled
            tween.OnKill(() => completionSource.TrySetCanceled());

            return completionSource.Task;
        }
    }
}
