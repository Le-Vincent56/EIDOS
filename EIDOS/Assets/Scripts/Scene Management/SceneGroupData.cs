using System.Linq;
using UnityEngine;

namespace EIDOS.Scene_Management
{
    [CreateAssetMenu(fileName = "Scene Group Data", menuName = "Data/Scene Group")]
    public class SceneGroupData : ScriptableObject
    {
        public SceneGroup[] sceneGroups;
        [HideInInspector] public int initialScene;

        public SceneData GetActiveScene(int index) => sceneGroups[index].scenes.FirstOrDefault(sceneData => sceneData.sceneType == SceneType.ActiveScene);
    }
}
