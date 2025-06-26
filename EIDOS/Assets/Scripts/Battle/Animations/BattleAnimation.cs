using UnityEngine;

namespace EIDOS.Battle.Animations
{
    [CreateAssetMenu(menuName = "Battle/Create Battle Animation", fileName = "BattleAnimation", order = 0)]
    public class BattleAnimation : ScriptableObject
    {
        public AnimationClip MainAnimationClip;
        public AudioClip[] AudioClips;
    }
}