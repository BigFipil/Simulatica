using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    public class SimulationState
    {
        public bool IsRunning { get; set; }
        public long ActualFrame { get; private set; } = 0;
        public long AproxParticleCount { get; private set; }
        public float Progress { get; private set; }
        public int ActiveThreads { get; set; }
        public List<Exception> ErrorList { get; set; } = new List<Exception>();

        public void NewFrameStatistics(long aproxParticleCount)
        {
            AproxParticleCount = aproxParticleCount;
            ActualFrame++;
        }
        public void Update()
        {
            ///TODO?
        }
    }
}
