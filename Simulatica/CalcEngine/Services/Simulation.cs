using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CalcEngine
{
    // IOHandler

    public class Simulation : ISimulation
    {
        [JsonProperty]
        public SimulationConfig Config { get; private set; }
        [JsonProperty]
        public SimulationState State { get; private set; }
        [JsonProperty]
        public Emitter Emitter { get; private set; }
        [JsonProperty]
        public Assembly Assembly { get; private set; }

        public Simulation(SimulationConfig C, SimulationState S, Emitter E)
        {
            Config = C;
            State = S;
            Emitter = E;
        }

        public void Run()
        {
            Console.WriteLine("normal");
        }
    }
}
