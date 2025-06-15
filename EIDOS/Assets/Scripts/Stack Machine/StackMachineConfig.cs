using UnityEngine;

namespace EIDOS.Stack_Machine
{
    public class StackMachineConfig
    {
        public bool Debug { get; set; } = false;
        public bool ExitOnPush { get; set; } = false;
        public bool ReenterOnPop { get; set; } = false;
        public string LogPrefix { get; set; } = "[Stack Machine]";
    }
}
