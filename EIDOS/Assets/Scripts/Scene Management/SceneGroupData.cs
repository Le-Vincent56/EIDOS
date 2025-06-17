using System.Linq;
using UnityEngine;

namespace EIDOS.Scene_Management
{
    [CreateAssetMenu(fileName = "Scene Group Data", menuName = "Data/Scene Group")]
    public class SceneGroupData : ScriptableObject
    {
        public int initialScene;
        public SceneGroup[] sceneGroups;

        public SceneData GetActiveScene(int index) => sceneGroups[index].scenes.FirstOrDefault(sceneData => sceneData.sceneType == SceneType.ActiveScene);
    }
}
