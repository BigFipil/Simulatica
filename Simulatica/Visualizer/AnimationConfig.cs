using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Visualizer
{
    public class AnimationConfig
    {
        public List<ParticleBlueprint> particleBlueprints { get; set; }
        public string OutputPath { get; set; }
        public Vector3 SimulationBoxSize { get; set; }
        public string SimulationType { get; set; }
        public ulong IterationCount { get; private set; }
        public double SimulationStepTime { get; private set; }
        public ulong DataSaveStepTime { get; private set; }
        public int OutputAnimationFramerate { get; set; } = 5;
        //public string SimulationType { get; set; }
    }


    public class ParticleBlueprint
    {
        public string Name { get; set; }
        public Dictionary<string, string> outputInformations { get; set; }
    }
}
