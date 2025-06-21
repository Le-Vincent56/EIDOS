using System.Collections.Generic;
using UnityEngine;

namespace EIDOS
{
    public enum Agent
    {
        WOOD,
        FIRE,
        EARTH,
        METAL,
        WATER
    }
    public enum Echo
    {
        ECHO1,
        ECHO2
    }
    public enum Move
    {
        MOVE1,
        MOVE2
    }
    [CreateAssetMenu(fileName = "EidraData", menuName = "Scriptable Objects/EidraData")]
    public class EidraData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _vigor;
        [SerializeField] private int _might;
        [SerializeField] private int _defense;
        [SerializeField] private int _resolve;
        [SerializeField] private int _grace;
        [SerializeField] private List<Echo> _echoes;
        [SerializeField] private List<Moves> _moves;
        [SerializeField] private Agent _agent;
        [SerializeField] private List<string> _ascensionLine;

        public string Name => _name;
        public int Vigor => _vigor;
        public int Might => _might;
        public int Defense => _defense;
        public int Resolve => _resolve;
        public int Grace => _grace;
        public List<Echo> Echoes => _echoes;
        public List<Moves> Moves => _moves;
        public Agent Agent => _agent;
        public List<string> AscensionLine => _ascensionLine;
    
    }
}
