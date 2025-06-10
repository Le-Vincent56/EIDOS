using Eflatun.SceneReference;using System;

namespace EIDOS.Scene_Management
{
    public enum SceneType { ActiveScene, UI, Tooling }

    [Serializable]
    public class SceneData
    {
        public SceneReference reference;
        public SceneType sceneType;
        public string Name => reference.Name;
    }
}
