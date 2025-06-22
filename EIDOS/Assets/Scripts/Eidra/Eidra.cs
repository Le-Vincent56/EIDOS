using UnityEngine;

namespace EIDOS.Eidra
{
    public class Eidra : MonoBehaviour
    {
        [SerializeField] private Sprite _eidraSprite;
        [SerializeField] EidraData eidraData;

        public Sprite EidraSprite => _eidraSprite;
    }
}
