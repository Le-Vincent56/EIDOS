using System;
using EIDOS.Event_Bus;
using EIDOS.Event_Bus.Events;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace EIDOS.Battle.Animations
{
    [RequireComponent(typeof(Animator))]
    public class EidraBattleAnimationHandler : MonoBehaviour
    {
        // temp
        [SerializeField] private int eidraIndex;
        
        [Header("Animations")]
        // Think about full structure later
        [SerializeField] private AnimationClip idleAnim;
        [SerializeField] private AnimationClip attackAnim;
        [SerializeField] private AnimationClip damageAnim;

        private Animator _animator;
        private PlayableGraph _playableGraph;
        private AnimationMixerPlayable _topLevelMixer;
        private AnimationMixerPlayable _combatMixer;

        private AnimationClipPlayable _oneShotPlayable;

        private EventBinding<UpdateEidraAnimationState> _onUpdateAnimationState;

        void Start()
        {
            
            _animator = GetComponent<Animator>();
            _playableGraph = PlayableGraph.Create("BattleAnimationSystem");

            AnimationPlayableOutput playableOutput =
                AnimationPlayableOutput.Create(_playableGraph, "Animation", _animator);

            _topLevelMixer = AnimationMixerPlayable.Create(_playableGraph, 2);
            playableOutput.SetSourcePlayable(_topLevelMixer);

            _combatMixer = AnimationMixerPlayable.Create(_playableGraph, 3);
            _topLevelMixer.ConnectInput(0, _combatMixer, 0);
            _playableGraph.GetRootPlayable(0).SetInputWeight(0, 1f);

            AnimationClipPlayable idlePlayable = AnimationClipPlayable.Create(_playableGraph, idleAnim);
            AnimationClipPlayable attackPlayable = AnimationClipPlayable.Create(_playableGraph, attackAnim);
            AnimationClipPlayable damagePlayable = AnimationClipPlayable.Create(_playableGraph, damageAnim);

            idlePlayable.GetAnimationClip().wrapMode = WrapMode.Loop;
            attackPlayable.GetAnimationClip().wrapMode = WrapMode.Once;
            damagePlayable.GetAnimationClip().wrapMode = WrapMode.Once;
                
            _combatMixer.ConnectInput(0, idlePlayable, 0);
            _combatMixer.ConnectInput(1, attackPlayable, 0);
            _combatMixer.ConnectInput(2, damagePlayable, 0);
                
            _playableGraph.Play();
        }
        
        private void OnDestroy()
        {
            if (_playableGraph.IsValid())
                _playableGraph.Destroy();
        }

        private void OnEnable()
        {
            _onUpdateAnimationState = new EventBinding<UpdateEidraAnimationState>(OnUpdateCombatAnimations);
            EventBus<UpdateEidraAnimationState>.Register(_onUpdateAnimationState);
        }

        private void OnDisable()
        {
            EventBus<UpdateEidraAnimationState>.Deregister(_onUpdateAnimationState);
        }

        private void OnUpdateCombatAnimations(UpdateEidraAnimationState eventdata)
        {
            if(eventdata.index != eidraIndex) return;
            
            _combatMixer.SetInputWeight(0, 0);
            _combatMixer.SetInputWeight(1, 0);
            _combatMixer.SetInputWeight(2, 0);

            switch (eventdata.state)
            {
                case EidraAnimationState.Idle:
                    _combatMixer.SetInputWeight(0, 1);
                    break;
                
                case EidraAnimationState.Attacking:
                    _combatMixer.SetInputWeight(1, 1);
                    break;
                
                case EidraAnimationState.Damaged:
                    _combatMixer.SetInputWeight(2, 1);
                    break;
            }
        }
    }
}