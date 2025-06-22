using System.Collections.Generic;
using UnityEngine;

namespace EIDOS.Eidra
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
        public int Vigor { get => _vigor; set => _vigor = value; }
        public int Might { get => _might; set => _might = value; }
        public int Defense { get => _defense; set => _defense = value; }
        public int Resolve { get => _resolve; set => _resolve = value; }
        public int Grace { get => _grace; set => _grace = value; }
        public List<Echo> Echoes { get => _echoes; set => _echoes = value; }
        public List<Moves> Moves { get => _moves; set => _moves = value; }
        public Agent Agent => _agent;
        public List<string> AscensionLine => _ascensionLine;

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(_name))
            {
                _name = name;
            }
        }
    }
}
