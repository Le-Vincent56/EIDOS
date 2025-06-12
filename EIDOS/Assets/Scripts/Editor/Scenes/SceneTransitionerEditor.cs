using EIDOS.Event_Bus;
using EIDOS.Event_Bus.Events;
using EIDOS.UI.Scenes;
using UnityEditor;
using UnityEngine;

namespace EIDOS.Editor.Scenes
{
    [CustomEditor(typeof(SceneTransitioner))]
    public class SceneTransitionerEditor : UnityEditor.Editor
    {
        private static SceneTransitioner.Type? lastTransitionType = SceneTransitioner.Type.FadeIn;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            SceneTransitioner sceneTransitioner = (SceneTransitioner)target;
            
            EditorGUILayout.Space(20f);
            
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Transition testing is only available in Play Mode", MessageType.Info);
                return;
            }
            
            // Determine which buttons should be enabled
            bool canUseInTransitions = lastTransitionType == null || IsOutTransition(lastTransitionType.Value);
            bool canUseOutTransitions = lastTransitionType == null || IsInTransition(lastTransitionType.Value);
            
            EditorGUILayout.LabelField("Transition Testing", EditorStyles.boldLabel);
            
            // Create the fade transition group
            EditorGUILayout.BeginHorizontal();

            // Only enable Fade In testing if In transitions can be used
            GUI.enabled = canUseInTransitions;
            if (GUILayout.Button("Fade In"))
            {
                TestTransition(SceneTransitioner.Type.FadeIn);
            }
            
            // Only enable Fade Out testing if Out transitions can be used
            GUI.enabled = canUseOutTransitions;
            if (GUILayout.Button("Fade Out"))
            {
                TestTransition(SceneTransitioner.Type.FadeOut);
            }

            // Reset the GUI state
            GUI.enabled = true;
            
            EditorGUILayout.EndHorizontal();
            
            // Create the swipe transition group
            EditorGUILayout.BeginHorizontal();
            
            // Only enable Swipe In testing if In transitions can be used
            GUI.enabled = canUseInTransitions;
            if (GUILayout.Button("Swipe In"))
            {
                TestTransition(SceneTransitioner.Type.SwipeIn);
            }
            
            // Only enable Swipe Out testing if Out transitions can be used
            GUI.enabled = canUseOutTransitions;
            if (GUILayout.Button("Swipe Out"))
            {
                TestTransition(SceneTransitioner.Type.SwipeOut);
            }
            
            // Reset the GUI state
            GUI.enabled = true;
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            
            // Show current state information
            if (lastTransitionType.HasValue)
            {
                string lastTransitionName = lastTransitionType.Value.ToString();
                string nextAllowed = IsInTransition(lastTransitionType.Value) ? "Out transitions" : "In transitions";
                EditorGUILayout.HelpBox($"Last transition: {lastTransitionName}. Next allowed: {nextAllowed}", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("Click any button to start testing transitions. After the first transition, buttons will be conditionally enabled.", MessageType.Info);
            }
            
            // Add reset button
            if (!GUILayout.Button("Reset Transition State")) return;
            
            lastTransitionType = null;
            Repaint();
        }
        
        private void TestTransition(SceneTransitioner.Type transitionType)
        {
            lastTransitionType = transitionType;
            
            // Create and raise a Transition event
            EventBus<Transition>.Raise(new Transition()
            {
                Type = transitionType
            });

            Repaint();
        }
        
        private bool IsInTransition(SceneTransitioner.Type transitionType)
        {
            return transitionType is SceneTransitioner.Type.FadeIn or SceneTransitioner.Type.SwipeIn;
        }
        
        private bool IsOutTransition(SceneTransitioner.Type transitionType)
        {
            return transitionType is SceneTransitioner.Type.FadeOut or SceneTransitioner.Type.SwipeOut;
        }

    }

}
