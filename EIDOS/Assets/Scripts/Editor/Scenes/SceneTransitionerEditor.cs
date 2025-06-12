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
            
            EditorGUILayout.LabelField("Transition Testing", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Fade In"))
            {
                TestTransition(SceneTransitioner.Type.FadeIn);
            }
            
            if (GUILayout.Button("Fade Out"))
            {
                TestTransition(SceneTransitioner.Type.FadeOut);
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Swipe In"))
            {
                TestTransition(SceneTransitioner.Type.SwipeIn);
            }
            
            if (GUILayout.Button("Swipe Out"))
            {
                TestTransition(SceneTransitioner.Type.SwipeOut);
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Click the buttons above to test different transition effects during Play Mode.", MessageType.Info);
        }
        
        private void TestTransition(SceneTransitioner.Type transitionType)
        {
            // Create and raise a Transition event
            EventBus<Transition>.Raise(new Transition()
            {
                Type = transitionType
            });
        }
    }

}
