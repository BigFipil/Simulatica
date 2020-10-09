using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace CalcEngine
{
    public class SmallSimulation : ISimulation
    {
        private readonly SimulationConfig config;
        private SimulationState state;
        private Emitter emitter;
        private Assembly assembly;

        public SmallSimulation(SimulationConfig C, SimulationState S, Emitter E)
        {
            config = C;
            state = S;
            emitter = E;
        }

        public void Run()
        {
            Console.WriteLine("small");
        }
    }
}
