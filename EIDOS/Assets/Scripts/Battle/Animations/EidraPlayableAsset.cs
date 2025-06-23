using UnityEngine;
using UnityEngine.Playables;

namespace EIDOS.Battle.Animations
{
    [System.Serializable]
    public class EidraPlayableAsset : PlayableAsset
    {
        // Factory method that generates a playable based on this asset
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return Playable.Create(graph);
        }
    }
}
