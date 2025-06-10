using System;

namespace EIDOS.Scene_Management
{
    public class LoadingProgress : IProgress<float>
    {
        public event Action<float> Progressed;
        private const float ratio = 1f;

        /// <summary>
        /// Report on the progress of the Loading and invoke its event
        /// </summary>
        public void Report(float value)
        {
            Progressed?.Invoke(value / ratio);
        }
    }
}
