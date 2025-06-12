using System;
using DG.Tweening;
using EIDOS.Event_Bus;
using EIDOS.Event_Bus.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace EIDOS.UI.Scenes
{
    public class SceneTransitioner : MonoBehaviour
    {
        public enum Type
        {
            FadeIn,
            FadeOut,
            SwipeIn,
            SwipeOut
        }

        private UIDocument document;
        private VisualElement root;
        private VisualElement fadeElement;

        [Header("Transition Variables")] 
        [SerializeField] private Ease swipeEase;
        [SerializeField] private float transitionSpeed;
        
        private Tween transitionTween;
        
        private EventBinding<Transition> onTransition;
        
        private void Awake()
        {
            // Get the UIDocument and root VisualElement
            document = GetComponent<UIDocument>();
            root = document.rootVisualElement;
            fadeElement = root.Query<VisualElement>("FadeOverlay");
        }

        private void OnEnable()
        {
            onTransition = new EventBinding<Transition>(TransitionScene);
            EventBus<Transition>.Register(onTransition);
        }

        private void OnDisable()
        {
            EventBus<Transition>.Deregister(onTransition);
            
            // Kill any running tweens
            transitionTween?.Kill();
        }

        private void TransitionScene(Transition eventData)
        {
            // Prepare the transition based on the type
            Prepare(eventData.Type);
            
            // Perform the transition
            switch (eventData.Type)
            {
                case Type.FadeIn:
                    Fade(1f, 0f);
                    break;
                
                case Type.FadeOut:
                    Fade(0f, 1f);
                    break;
                
                case Type.SwipeIn:
                    Swipe(100f, 0f, Align.FlexEnd);
                    break;
                
                case Type.SwipeOut:
                    Swipe(0f, 100f, Align.FlexStart);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Prepare(Type transitionType)
        {
            switch (transitionType)
            {
                case Type.FadeIn:
                case Type.FadeOut:
                    fadeElement.style.width = Length.Percent(100);
                    fadeElement.style.alignSelf = Align.Auto;
                    break;
                
                case Type.SwipeIn:
                case Type.SwipeOut:
                    fadeElement.style.backgroundColor = new Color(0, 0, 0, 1);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(transitionType), transitionType, null);
            }
        }

        private void Fade(float startValue, float endValue)
        {
            // Kill any existing tween
            transitionTween?.Kill();

            transitionTween = DOTween.To(
                () => startValue,
                alpha => fadeElement.style.backgroundColor = new Color(0, 0, 0, alpha),
                endValue,
                transitionSpeed
            ).SetEase(Ease.Linear);
        }

        private void Swipe(float startValue, float endValue, Align alignSelf)
        {
            // Kill any existing tween
            transitionTween?.Kill();
            
            // Prepare the swipe in by aligning to the right
            fadeElement.style.alignSelf = alignSelf;
            
            transitionTween = DOTween.To(
                () => startValue,
                width => fadeElement.style.width = Length.Percent(width),
                endValue,
                transitionSpeed
            ).SetEase(swipeEase);
        }
    }
}
