using UnityEngine;

namespace EIDOS.UI.Main_Menu.Transitions
{
    [CreateAssetMenu(fileName = "Transition Preset", menuName = "UI/Transition Preset")]
    public class TransitionPreset : ScriptableObject
    {
        [Header("Timing")] 
        [SerializeField] private float duration = 2f;

        [Header("Scale Settings")] 
        [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private float minScale = 0.05f;
        [SerializeField] private float maxScale = 20f;
        
        [Header("Fog Settings")]
        [SerializeField] private AnimationCurve fogDensityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public float Duration => duration;
        public AnimationCurve ScaleCurve => scaleCurve;
        public float MinScale => minScale;
        public float MaxScale => maxScale;
        public AnimationCurve FogDensityCurve => fogDensityCurve;

        [ContextMenu("Set Default")]
        private void SetDefault()
        {
            duration = 2f;
            minScale = 0.05f;
            maxScale = 20f;
            
            // Slow start, speed up, maintain, slow down
            scaleCurve = new AnimationCurve(
                new Keyframe(0f, 0f, 0f, 0f),      // Start slow
                new Keyframe(0.2f, 0.1f, 2f, 2f),  // Speed up
                new Keyframe(0.5f, 0.5f, 1f, 1f),  // Linear middle
                new Keyframe(0.8f, 0.9f, 2f, 2f),  // Start slowing
                new Keyframe(1f, 1f, 0f, 0f)       // End slow
            );
            
            // Fog peaks in the middle
            fogDensityCurve = new AnimationCurve(
                new Keyframe(0f, 0f),
                new Keyframe(0.5f, 1f),
                new Keyframe(1f, 0f)
            );
        }
    }
}