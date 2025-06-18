using EIDOS.Timers;
using UnityEngine;

namespace EIDOS.Battle.Economy
{
    public class ATBGuage : MonoBehaviour
    {
        private FillTimer fillTimer;

        private void Awake()
        {
            fillTimer = new FillTimer(10f, 1f);
        }

        private void OnEnable()
        {
            fillTimer.OnFillUpdate += OnTick;
        }

        private void OnDisable()
        {
            fillTimer.OnFillUpdate -= OnTick;
        }

        private void Start()
        {
            fillTimer.Start();
        }

        private void OnDestroy()
        {
            // Dispose of the timer
            fillTimer?.Dispose();
        }

        private void OnTick(float value)
        {
            
        }
    }
}
