using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace CalcEngine
{
    class MultipleSimulation : ISimulation
    {
        [JsonProperty]
        public SimulationConfig Config { get; private set; }
        [JsonProperty]
        public SimulationState State { get; private set; }
        [JsonProperty]
        public Emitter Emitter { get; private set; }
        [JsonProperty]
        public Writer Writer { get; private set; }
        public ISimulation Simulation { get; set; }

        public MultipleSimulation(SimulationConfig C, SimulationState S, Emitter E, Writer W)
        {
            Config = C;
            State = S;
            Emitter = E;
            Writer = W;
            //Simulation = Sim;
        }
        public void Run()
        {

        }
    }
}
