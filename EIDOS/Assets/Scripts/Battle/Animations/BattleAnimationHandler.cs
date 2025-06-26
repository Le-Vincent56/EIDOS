using System;
using System.Collections;
using EIDOS.Event_Bus;
using EIDOS.Event_Bus.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace EIDOS.Battle.Animations
{
    public class BattleAnimationHandler : MonoBehaviour
    {
        // In the future, this should be provided by the combat system/by participating Eidra
        [SerializeField] private EidraBattleAnimationHandler[] eidraHandlers;
        [SerializeField] private BattleAnimation battleAnim;

        private void Start()
        {
            StartCoroutine(CombatLoop());
        }

        IEnumerator CombatLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(2f);
                
                EventBus<UpdateEidraAnimationState>.Raise(new UpdateEidraAnimationState()
                {
                    index = 0,
                    state = EidraAnimationState.Attacking
                });
                EventBus<UpdateEidraAnimationState>.Raise(new UpdateEidraAnimationState()
                {
                    index = 1,
                    state = EidraAnimationState.Damaged
                });
                EventBus<PlayEidraBattleAnimation>.Raise(new PlayEidraBattleAnimation()
                {
                    index = 0,
                    battleAnimation = battleAnim
                });
                
                yield return new WaitForSeconds(2f);
                
                EventBus<UpdateEidraAnimationState>.Raise(new UpdateEidraAnimationState()
                {
                    index = 0,
                    state = EidraAnimationState.Damaged
                });
                EventBus<UpdateEidraAnimationState>.Raise(new UpdateEidraAnimationState()
                {
                    index = 1,
                    state = EidraAnimationState.Attacking
                });
                EventBus<PlayEidraBattleAnimation>.Raise(new PlayEidraBattleAnimation()
                {
                    index = 1,
                    battleAnimation = battleAnim
                });
                
                yield return new WaitForSeconds(2f);
                
                EventBus<UpdateEidraAnimationState>.Raise(new UpdateEidraAnimationState()
                {
                    index = 0,
                    state = EidraAnimationState.Idle
                });
                EventBus<UpdateEidraAnimationState>.Raise(new UpdateEidraAnimationState()
                {
                    index = 1,
                    state = EidraAnimationState.Idle
                });
            }
        }
    }
}