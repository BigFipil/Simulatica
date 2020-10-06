using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CalcEngine
{
    // IOHandler

    public class Simulation : ISimulation
    {
        private readonly SimulationConfig config;
        private SimulationState state;
        private Emitter emitter;
        private Assembly assembly;

        public Simulation(SimulationConfig C, SimulationState S, Emitter E)
        {
            config = C;
            state = S;
            emitter = E;
        }

        public void Run()
        {
            Console.WriteLine("normal");
        }
    }
}
