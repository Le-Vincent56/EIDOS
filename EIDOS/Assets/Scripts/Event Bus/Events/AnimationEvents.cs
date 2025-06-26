using EIDOS.Battle.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace EIDOS.Event_Bus.Events
{
    public enum EidraAnimationState
    {
        Idle,
        Attacking,
        Damaged
    }
    
    /// <summary>
    /// Event to update animations based on standard states.
    /// </summary>
    public struct UpdateEidraAnimationState : IEvent
    {
        // TODO: Think of some other way to validate later
        public int index;
        public EidraAnimationState state;
    }

    /// <summary>
    /// Event to tell an Eidra to play an animation, but doesn't change the anim state.
    /// </summary>
    public struct PlayEidraAnimOneShot : IEvent
    {
        // TODO: Think of some other way to validate later
        public int index;
        public AnimationClip animClip;
        public AudioClip[] audioClips;
    }

    public struct PlayEidraBattleAnimation : IEvent
    {
        public int index;
        public BattleAnimation battleAnimation;
    }

    // TODO: Better name?
    public struct EidraDealtDamage : IEvent
    {
        
    }

    public struct EidraCompleteAnimation : IEvent
    {
        
    }
}