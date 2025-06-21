using UnityEngine;

namespace EIDOS
{
    [CreateAssetMenu(fileName = "Moves", menuName = "Scriptable Objects/Moves")]
    public class Moves : ScriptableObject
    {
        [SerializeField] private Move _move;
        [SerializeField] private Agent _agent;

        public Move Move => _move;
        public Agent Agent => _agent;
    }
}
