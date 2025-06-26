using System;
using System.Collections;
using EIDOS.Event_Bus;
using EIDOS.Event_Bus.Events;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace EIDOS.Battle.Animations
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class EidraBattleAnimationHandler : MonoBehaviour
    {
        // temp
        [SerializeField] private int eidraIndex;
        
        // animations
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
        
        // audio
        [SerializeField] private AudioClip defaultClip;
        private AudioClip[] _audioClips = Array.Empty<AudioClip>();
        private int _audioClipIndex;
        private AudioSource _audioSource;
        private ScriptPlayable<AudioPlayableBehaviour> _audioPlayable;
        
        // coroutines
        private IEnumerator _blendInMixer;
        private IEnumerator _blendOutMixer;
        
        // events
        private EventBinding<UpdateEidraAnimationState> _onUpdateAnimationState;
        private EventBinding<PlayEidraAnimOneShot> _onPlayAnimOneShot;
        private EventBinding<PlayEidraBattleAnimation> _onPlayBattleAnimation;

        void Start()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _playableGraph = PlayableGraph.Create("BattleAnimationSystem");

            SetupAnimations();
            
            SetupAudio();

            _playableGraph.Play();
        }

        private void SetupAudio()
        {
            ScriptPlayableOutput scriptOutput = ScriptPlayableOutput.Create(_playableGraph, "Audio");

            _audioPlayable = ScriptPlayable<AudioPlayableBehaviour>.Create(_playableGraph);
            var behaviour = _audioPlayable.GetBehaviour();
            behaviour.clip = defaultClip;
            behaviour.audioSource = new ExposedReference<AudioSource> { defaultValue = _audioSource };
            
            scriptOutput.SetSourcePlayable(_audioPlayable);
        }

        private void SetupAnimations()
        {
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

            _onPlayAnimOneShot = new EventBinding<PlayEidraAnimOneShot>(OnPlayOneShot);
            EventBus<PlayEidraAnimOneShot>.Register(_onPlayAnimOneShot);
            
            _onPlayBattleAnimation = new EventBinding<PlayEidraBattleAnimation>(OnPlayBattleAnimation);
            EventBus<PlayEidraBattleAnimation>.Register(_onPlayBattleAnimation);
        }

        private void OnDisable()
        {
            EventBus<UpdateEidraAnimationState>.Deregister(_onUpdateAnimationState);
            EventBus<PlayEidraAnimOneShot>.Deregister(_onPlayAnimOneShot);
            EventBus<PlayEidraBattleAnimation>.Deregister(_onPlayBattleAnimation);
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

        private void OnPlayOneShot(PlayEidraAnimOneShot eventdata)
        {
            if(eventdata.index != eidraIndex) return;
            if (_playableGraph.IsValid() && _oneShotPlayable.IsValid() && eventdata.animClip == _oneShotPlayable.GetAnimationClip()) return;

            InterruptOneShot();
            _audioClipIndex = 0;
            _audioClips = eventdata.audioClips;
            UpdateAudioClip();
            
            _oneShotPlayable = AnimationClipPlayable.Create(_playableGraph, eventdata.animClip);
            _oneShotPlayable.GetAnimationClip().wrapMode = WrapMode.Once;
            _topLevelMixer.ConnectInput(1, _oneShotPlayable, 0);
            _topLevelMixer.SetInputWeight(1, 1);
            
            // Calculate blendDuration
            float blendDuration = Mathf.Min(eventdata.animClip.length * 0.1f, eventdata.animClip.length / 2);
            
            BlendIn(blendDuration);
            BlendOut(blendDuration, eventdata.animClip.length - blendDuration);
        }
        
        private void OnPlayBattleAnimation(PlayEidraBattleAnimation eventdata)
        {
            if(eventdata.index != eidraIndex) return;
            if (_playableGraph.IsValid() && _oneShotPlayable.IsValid() && eventdata.battleAnimation.MainAnimationClip == _oneShotPlayable.GetAnimationClip()) return;

            InterruptOneShot();
            _audioClipIndex = 0;
            _audioClips = eventdata.battleAnimation.AudioClips;
            UpdateAudioClip();
            
            _oneShotPlayable = AnimationClipPlayable.Create(_playableGraph, eventdata.battleAnimation.MainAnimationClip);
            _oneShotPlayable.GetAnimationClip().wrapMode = WrapMode.Once;
            _topLevelMixer.ConnectInput(1, _oneShotPlayable, 0);
            _topLevelMixer.SetInputWeight(1, 1);
            
            // Calculate blendDuration
            float blendDuration = Mathf.Min(eventdata.battleAnimation.MainAnimationClip.length * 0.1f, eventdata.battleAnimation.MainAnimationClip.length / 2);
            
            BlendIn(blendDuration);
            BlendOut(blendDuration, eventdata.battleAnimation.MainAnimationClip.length - blendDuration);
        }

        private void UpdateAudioClip()
        {
            if (_audioClipIndex < _audioClips.Length)
            {
                if (_audioClips[_audioClipIndex] != null)
                {
                    _audioPlayable.GetBehaviour().clip = _audioClips[_audioClipIndex];
                }
                else
                {
                    _audioPlayable.GetBehaviour().clip = defaultClip;
                }
                _audioClipIndex++;
            }
            else
            {
                _audioPlayable.GetBehaviour().clip = defaultClip;
            }
        }

        public void PlayAudioOneShot()
        {
            _audioPlayable.GetBehaviour().PlayAudioClip();
            UpdateAudioClip();
        }

        private void BlendIn(float duration)
        {
            _blendInMixer = Blend(duration, blendTime =>
            {
                float weight = Mathf.Lerp(1f, 0f, blendTime);
                _topLevelMixer.SetInputWeight(0, weight);
                _topLevelMixer.SetInputWeight(1, 1f - weight);
            });

            StartCoroutine(_blendInMixer);
        }
        
        private void BlendOut(float duration, float delay)
        {
            _blendInMixer = Blend(duration, blendTime =>
            {
                float weight = Mathf.Lerp(0f, 1f, blendTime);
                _topLevelMixer.SetInputWeight(0, weight);
                _topLevelMixer.SetInputWeight(1, 1f - weight);
            }, delay, DisconnectOneShot);

            StartCoroutine(_blendInMixer);
        }

        IEnumerator Blend(float duration, Action<float> blendCallback, float delay = 0f, Action finishedCallback = null)
        {
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            float blendTime = 0f;
            while (blendTime < 1f)
            {
                blendTime += Time.deltaTime / duration;
                blendCallback(blendTime);
                yield return blendTime;
            }

            blendCallback(1f);
            
            finishedCallback?.Invoke();
        }

        void InterruptOneShot()
        {
            if(_blendInMixer != null) StopCoroutine(_blendInMixer);
            if(_blendOutMixer != null) StopCoroutine(_blendOutMixer);
            
            if (_oneShotPlayable.IsValid())
            {
                DisconnectOneShot();
            }
        }

        private void DisconnectOneShot()
        {
            _topLevelMixer.DisconnectInput(1);
            _playableGraph.DestroyPlayable(_oneShotPlayable);
        }
    }
}