using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CalcEngine
{
    // IOHandler

    public class Simulation : Interfaces.ISymulation
    {
        private readonly SimulationConfig config;
        private SimulationState state;
        private Services.Emitter emitter;
        private Assembly assembly;

        public Simulation(SimulationConfig C, SimulationState S, Services.Emitter E)
        {
            config = C;
            state = S;
            emitter = E;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
