using EIDOS.Singletons;
using UnityEditor;
using UnityEngine;

namespace EIDOS.Scene_Management
{
    public class Bootstrapper : PersistentSingleton<Bootstrapper>
    {
        private static readonly int SceneIndex = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.playModeStartScene = 
                AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[SceneIndex].path);
#endif
        }
    }
}
