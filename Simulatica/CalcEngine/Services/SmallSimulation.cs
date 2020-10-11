using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;

namespace CalcEngine
{
    public class SmallSimulation : ISimulation
    {
        [JsonProperty]
        public SimulationConfig Config { get; private set; }
        [JsonProperty]
        public SimulationState State { get; private set; }
        [JsonProperty]
        public Emitter Emitter { get; private set; }
        [JsonProperty]
        public Assembly Assembly { get; private set; }

        public SmallSimulation(SimulationConfig C, SimulationState S, Emitter E)
        {
            Config = C;
            State = S;
            Emitter = E;
        }

        public void Run()
        {
            Console.WriteLine("small");
        }
    }
}
