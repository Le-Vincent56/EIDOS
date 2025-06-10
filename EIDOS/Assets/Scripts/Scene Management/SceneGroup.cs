using System;
using System.Collections.Generic;
using System.Linq;

namespace EIDOS.Scene_Management
{
    [Serializable]
    public class SceneGroup
    {
        public string groupName = "New Scene Group";
        public List<SceneData> scenes;

        /// <summary>
        /// Get the name of the first Scene matching the given SceneType
        /// </summary>
        public string FindSceneNameByType(SceneType sceneType) =>
            scenes.FirstOrDefault(scene => scene.sceneType == sceneType)?.reference.Name;
    }
}
