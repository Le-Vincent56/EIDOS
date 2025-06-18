using System;
using UnityEngine;

namespace EIDOS.Timers
{
    public class FillTimer : Timer
    {
        private readonly float maximumTime;
        private readonly float multiplier;
        private bool atMax;
        
        public override bool IsFinished => !IsRunning && CurrentTime >= maximumTime;

        public Action OnTimerMax = delegate { };
        public Action<float> OnFillUpdate = delegate { };

        public FillTimer(float maximumTime = 10.0f, float multiplier = 1.0f) : base(0)
        {
            this.maximumTime = maximumTime;
            this.multiplier = multiplier;
            atMax = false;
        }
        
        public override void Tick()
        {
            // Exit case: the timer is at the maximum time
            if (atMax) return;
            
            // Check if the Timer is running and the current time is greater or equal
            // to the time threshold
            if (IsRunning && CurrentTime >= maximumTime)
            {
                // Invoke the tick event
                OnTimerMax.Invoke();

                // Set to be at maximum
                atMax = true;
                
                // Update the fill with the percentage from 0-1
                OnFillUpdate.Invoke(CurrentTime /  maximumTime);
            }

            // Check if the current time is not yet at the maximum time
            if (!IsRunning || !(CurrentTime < maximumTime)) return;
            
            // If so, add deltaTime to the current time
            CurrentTime += Time.deltaTime * multiplier;
                
            // Call the tick event
            OnTimerTick.Invoke();
            
            // Update the fill with the percentage from 0-1
            OnFillUpdate.Invoke(CurrentTime /  maximumTime);
        }

        public override void Reset()
        {
            // Reset the current time
            CurrentTime = initialTime;

            // Set to not be at maximum time
            atMax = false;
        }
    }
}
