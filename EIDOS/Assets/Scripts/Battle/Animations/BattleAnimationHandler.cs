using System;
using System.Collections;
using EIDOS.Event_Bus;
using EIDOS.Event_Bus.Events;
using UnityEngine;

namespace EIDOS.Battle.Animations
{
    public class BattleAnimationHandler : MonoBehaviour
    {
        // In the future, this should be provided by the combat system/by participating Eidra
        [SerializeField] private EidraBattleAnimationHandler[] eidraHandlers;

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