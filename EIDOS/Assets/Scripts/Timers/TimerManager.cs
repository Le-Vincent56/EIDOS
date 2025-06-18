using EIDOS.Extensions;
using System.Collections.Generic;

namespace EIDOS.Timers
{
    public static class TimerManager
    {
        private static readonly List<Timer> Timers = new();
        private static readonly List<Timer> Sweep = new();

        /// <summary>
        /// Register a Timer
        /// </summary>
        public static void RegisterTimer(Timer timer) => Timers.Add(timer);

        /// <summary>
        /// Deregister a Timer
        /// </summary>
        public static void DeregisterTimer(Timer timer) => Timers.Remove(timer);

        /// <summary>
        /// Check if a Timer is registered
        /// </summary>
        public static bool CheckRegistry(Timer timer) => Timers.Contains(timer);

        /// <summary>
        /// Update the Timers
        /// </summary>
        public static void UpdateTimers()
        {
            // Exit case - if there are no Timers to manage
            if (Timers.Count == 0) return;

            // Add the timers to the sweep list
            Sweep.RefreshWith(Timers);

            // Iterate through each Timer in the sweep list
            foreach (Timer timer in Sweep)
            {
                // Tick the Timer
                timer.Tick();
            }
        }

        /// <summary>
        /// Clear the Timers list
        /// </summary>
        public static void Clear()
        {
            // Refresh the sweep list with the Timers
            Sweep.RefreshWith(Timers);

            // Iterate through each timer in the sweep list
            foreach (Timer timer in Sweep)
            {
                // Dispose of the timer
                timer.Dispose();
            }

            // Clear the lists
            Timers.Clear();
            Sweep.Clear();
        }
    }
}
